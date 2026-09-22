using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;

namespace ServicePortal.AutomationTests;

public sealed class BrowserTestBuilderTests
{
    [Test]
    public async Task Saved_browser_test_runs_each_step()
    {
        var definition = JsonSerializer.Deserialize<TestDefinition>(Required("PORTAL_BROWSER_TEST_JSON"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("The saved browser test could not be read.");
        using var playwright = await Playwright.CreateAsync();
        var showBrowser = string.Equals(Environment.GetEnvironmentVariable("PORTAL_SHOW_BROWSER"), "true", StringComparison.OrdinalIgnoreCase);
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "msedge", Headless = !showBrowser });
        var page = await browser.NewPageAsync();
        await page.GotoAsync(definition.Url, new() { WaitUntil = WaitUntilState.DOMContentLoaded, Timeout = 60_000 });

        foreach (var step in definition.Steps)
        {
            switch (step.Action)
            {
                case "Click":
                    await page.Locator(step.Target).First.ClickAsync();
                    break;
                case "Enter text":
                    await page.Locator(step.Target).First.FillAsync(step.Value);
                    break;
                case "Check text":
                    Assert.That(await page.Locator(step.Target).First.TextContentAsync(), Does.Contain(step.Value), $"Expected text was not found for {step.Target}.");
                    break;
                case "Wait":
                    await page.WaitForTimeoutAsync(int.TryParse(step.Value, out var milliseconds) ? milliseconds : 1_000);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported test action: {step.Action}");
            }
        }
    }

    private static string Required(string name) => Environment.GetEnvironmentVariable(name) ?? throw new InvalidOperationException($"{name} is required.");
    private sealed class TestDefinition { public string Url { get; set; } = string.Empty; public List<TestStep> Steps { get; set; } = []; }
    private sealed class TestStep { public string Action { get; set; } = string.Empty; public string Target { get; set; } = string.Empty; public string Value { get; set; } = string.Empty; }
}
