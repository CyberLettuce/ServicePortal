using System.Diagnostics;
using System.Text.Json;
using ServicePortal.Data;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed record AutomationRunResult(
    bool Succeeded,
    string Summary,
    string Output,
    string TechnicalOutput = "");

public sealed class AutomationRunnerService(
    IWebHostEnvironment environment,
    IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<AutomationRunResult> RunLoginAsync(
        string product,
        string target,
        string url,
        string email,
        string password,
        bool showBrowser,
        string startedBy)
    {
        var startedAt = DateTime.UtcNow;
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            return new(false, "Enter a valid test URL.", string.Empty);
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return new(false, "Enter the test email address and password.", string.Empty);

        var project = Path.Combine(environment.ContentRootPath, "AutomationTests", "ServicePortal.AutomationTests.csproj");
        if (!File.Exists(project))
            return new(false, "The automated test project has not been installed on this server.", string.Empty);

        var startInfo = new ProcessStartInfo("dotnet", $"test \"{project}\" --filter FullyQualifiedName~LoginTests")
        {
            WorkingDirectory = Path.GetDirectoryName(project)!,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var runtimeFolder = Path.Combine(environment.ContentRootPath, "App_Data", "automation-runtime");
        Directory.CreateDirectory(runtimeFolder);
        startInfo.Environment["DOTNET_CLI_HOME"] = Path.Combine(runtimeFolder, "dotnet");
        startInfo.Environment["NUGET_PACKAGES"] = Path.Combine(runtimeFolder, "nuget-packages");
        startInfo.Environment["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        startInfo.Environment["DOTNET_NOLOGO"] = "1";
        startInfo.Environment["PLAYWRIGHT_BROWSERS_PATH"] = Path.Combine(runtimeFolder, "ms-playwright");
        startInfo.Environment["PORTAL_TEST_PRODUCT"] = product;
        startInfo.Environment["PORTAL_TEST_URL"] = url.Trim();
        startInfo.Environment["PORTAL_TEST_EMAIL"] = email.Trim();
        startInfo.Environment["PORTAL_TEST_PASSWORD"] = password;
        startInfo.Environment["PORTAL_SHOW_BROWSER"] = showBrowser ? "true" : "false";

        using var process = Process.Start(startInfo);
        if (process is null) return new(false, "The automated test runner could not start.", string.Empty);

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        var waitForExit = process.WaitForExitAsync();
        var completed = await Task.WhenAny(waitForExit, Task.Delay(TimeSpan.FromMinutes(2)));
        if (completed != waitForExit)
        {
            try { process.Kill(true); } catch { }
            var timedOut = new AutomationRunResult(
                false,
                $"The {product} {target} login check timed out.",
                "The page did not finish the sign-in check within two minutes. The portal may be unavailable or responding slowly.");
            await SaveResultAsync(product, target, url, startedBy, startedAt, timedOut);
            return timedOut;
        }

        var output = (await outputTask) + Environment.NewLine + (await errorTask);
        var result = CreateUserFacingResult(product, target, process.ExitCode == 0, output);

        await SaveResultAsync(product, target, url, startedBy, startedAt, result);
        return result;
    }

    public async Task<AutomationRunResult> RunBrowserTestAsync(BrowserTest test, bool showBrowser, string startedBy)
    {
        var startedAt = DateTime.UtcNow;
        if (!Uri.TryCreate(test.StartUrl, UriKind.Absolute, out _))
            return new(false, "This saved test does not have a valid starting URL.", string.Empty);

        var project = Path.Combine(environment.ContentRootPath, "AutomationTests", "ServicePortal.AutomationTests.csproj");
        if (!File.Exists(project))
            return new(false, "The automated test project has not been installed on this server.", string.Empty);

        var startInfo = new ProcessStartInfo("dotnet", $"test \"{project}\" --filter FullyQualifiedName~BrowserTestBuilderTests")
        {
            WorkingDirectory = Path.GetDirectoryName(project)!, RedirectStandardOutput = true, RedirectStandardError = true,
            UseShellExecute = false, CreateNoWindow = true
        };
        ConfigureRuntime(startInfo);
        startInfo.Environment["PORTAL_SHOW_BROWSER"] = showBrowser ? "true" : "false";
        startInfo.Environment["PORTAL_BROWSER_TEST_JSON"] = JsonSerializer.Serialize(new
        {
            url = test.StartUrl,
            steps = test.Steps.OrderBy(step => step.SortOrder).Select(step => new { action = step.Action, target = step.Target, value = step.Value })
        });

        using var process = Process.Start(startInfo);
        if (process is null) return new(false, "The automated test runner could not start.", string.Empty);
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        var waitForExit = process.WaitForExitAsync();
        var completed = await Task.WhenAny(waitForExit, Task.Delay(TimeSpan.FromMinutes(2)));
        if (completed != waitForExit)
        {
            try { process.Kill(true); } catch { }
            var timedOut = new AutomationRunResult(false, $"{test.Name} timed out.", "The test did not complete within two minutes.");
            await SaveResultAsync(test.Name, test.Product, test.StartUrl, startedBy, startedAt, timedOut);
            return timedOut;
        }

        var output = (await outputTask) + Environment.NewLine + (await errorTask);
        var result = process.ExitCode == 0
            ? new AutomationRunResult(true, $"{test.Name} passed.", "Every saved browser-test step completed successfully.", output)
            : new AutomationRunResult(false, $"{test.Name} failed.", "A saved browser-test step could not complete. Open the technical details to see which step failed.", output);
        await SaveResultAsync(test.Name, test.Product, test.StartUrl, startedBy, startedAt, result);
        return result;
    }

    private void ConfigureRuntime(ProcessStartInfo startInfo)
    {
        var runtimeFolder = Path.Combine(environment.ContentRootPath, "App_Data", "automation-runtime");
        Directory.CreateDirectory(runtimeFolder);
        startInfo.Environment["DOTNET_CLI_HOME"] = Path.Combine(runtimeFolder, "dotnet");
        startInfo.Environment["NUGET_PACKAGES"] = Path.Combine(runtimeFolder, "nuget-packages");
        startInfo.Environment["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        startInfo.Environment["DOTNET_NOLOGO"] = "1";
        startInfo.Environment["PLAYWRIGHT_BROWSERS_PATH"] = Path.Combine(runtimeFolder, "ms-playwright");
    }

    private static AutomationRunResult CreateUserFacingResult(
        string product,
        string target,
        bool succeeded,
        string technicalOutput)
    {
        if (succeeded)
        {
            return new(
                true,
                $"The {product} {target} login check passed.",
                "The page address opened, the username and password fields were available, and the supplied account signed in successfully.",
                technicalOutput);
        }

        var failure = technicalOutput.ToLowerInvariant();
        var explanation = failure.Contains("username field") || failure.Contains("email field")
            ? "The page address opened, but it did not show a usable username field. Check that this is the correct login address for the country."
            : failure.Contains("password field")
                ? "The username field was found, but the page did not show a usable password field."
                : failure.Contains("remained open") || failure.Contains("waitforurl")
                    ? "The sign-in form was submitted, but the portal did not leave the login page. Check the test account credentials or whether the account needs an additional sign-in step."
                    : failure.Contains("navigating") || failure.Contains("gotoasync")
                        ? "The portal could not open the login page in time. Check the page address and whether the test portal is available."
                        : "The login check could not complete. Open Technical details below if further investigation is needed.";

        return new(false, $"The {product} {target} login check failed.", explanation, technicalOutput);
    }

    public async Task<List<AutomationTestResult>> GetRecentResultsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.AutomationTestResults
            .OrderByDescending(result => result.CompletedAt)
            .Take(20)
            .ToListAsync();
    }

    private async Task SaveResultAsync(
        string product,
        string target,
        string url,
        string startedBy,
        DateTime startedAt,
        AutomationRunResult result)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.AutomationTestResults.Add(new AutomationTestResult
        {
            TestName = $"{product} login check",
            Country = target,
            Url = url,
            Succeeded = result.Succeeded,
            Summary = result.Summary,
            Output = result.Output,
            StartedBy = startedBy,
            StartedAt = startedAt,
            CompletedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }
}
