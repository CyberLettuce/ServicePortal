using Microsoft.Playwright;
using NUnit.Framework;

namespace ServicePortal.AutomationTests;

public sealed class LoginTests
{
    [Test]
    public async Task Test_user_can_sign_in_to_selected_portal()
    {
        var url = Required("PORTAL_TEST_URL");
        var email = Required("PORTAL_TEST_EMAIL");
        var password = Required("PORTAL_TEST_PASSWORD");

        var showBrowser = string.Equals(Environment.GetEnvironmentVariable("PORTAL_SHOW_BROWSER"), "true", StringComparison.OrdinalIgnoreCase);
        using var playwright = await Playwright.CreateAsync();
        var launchOptions = new BrowserTypeLaunchOptions { Channel = "msedge", Headless = !showBrowser };
        if (showBrowser) launchOptions.SlowMo = 600;
        await using var browser = await playwright.Chromium.LaunchAsync(launchOptions);
        var page = await browser.NewPageAsync();

        await page.GotoAsync(url, new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60_000
        });
        var emailField = page.Locator("input[type='email']:visible, input[name*='email' i]:visible, input[id*='email' i]:visible").First;
        var passwordField = page.Locator("input[type='password']:visible").First;
        Assert.That(await emailField.IsVisibleAsync(), Is.True,
            "The login page did not show a usable username field.");
        Assert.That(await passwordField.IsVisibleAsync(), Is.True,
            "The login page did not show a usable password field.");
        await emailField.FillAsync(email);
        await passwordField.FillAsync(password);
        await page.Locator("button[type='submit'], input[type='submit']").First.ClickAsync();
        await page.WaitForTimeoutAsync(2_000);
        Assert.That(await passwordField.IsVisibleAsync(), Is.False,
            "The sign-in form was submitted but the portal remained on the login page.");
    }

    private static string Required(string name) => Environment.GetEnvironmentVariable(name)
        ?? throw new InvalidOperationException($"{name} is required for this test.");
}
