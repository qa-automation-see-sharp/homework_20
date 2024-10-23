using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.xUnit.Ui.Playwright.Tests;

public class TextBoxPageTests : IAsyncLifetime
{
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }
    private IPage Page { get; set; }

    public async Task InitializeAsync()
    {
        var playwright = await CreateAsync();        

        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 50,
            Timeout = 10000,
            Args = ["--start-maximized"]
        });

        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport,
            Locale = "en-US",
            ColorScheme = ColorScheme.NoPreference
        });

        Page = await Context.NewPageAsync();

        Page.SetDefaultTimeout(10000);
        await Page.GotoAsync("https://demoqa.com/text-box");
    }

    [Fact]
    public async Task FillOutAndSubmitForm()
    {
        var fullName = "Full Name";
        var email = "email@mail.com";
        var currentAddress = "Current Address";
        var permanentAddress = "Permanent Address";

        await Page.Locator("id=userName").FillAsync(fullName);
        await Page.Locator("id=userEmail").FillAsync(email);
        await Page.Locator("id=currentAddress").FillAsync(currentAddress);
        await Page.Locator("id=permanentAddress").FillAsync(permanentAddress);
        await Page.Locator("id=submit").ClickAsync();

        var textToAssert = await Page.Locator("id=output").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.False(string.IsNullOrEmpty(textToAssert));
            Assert.Contains(fullName, textToAssert);
            Assert.Contains(email, textToAssert);
            Assert.Contains(currentAddress, textToAssert);
            Assert.Contains(permanentAddress, textToAssert);
        });
    }

    public async Task DisposeAsync()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
}