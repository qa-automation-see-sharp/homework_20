using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.NUnit.Ui.Playwright.Tests;

[TestFixture]
public class RadioButtonPageTests
{
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }
    private IPage Page { get; set; }

    [SetUp]
    public async Task Setup()
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
        await Page.GotoAsync("https://demoqa.com/radio-button");
    }

    [Test]
    public async Task RadioButtonYes()
    {
        await Page.Locator("label.custom-control-label[for='yesRadio']").ClickAsync();
        var isButtonSelected = await Page.Locator("id=yesRadio").IsCheckedAsync();
        var isButtonEnabled = await Page.Locator("id=yesRadio").IsEnabledAsync();

        var outputSelection = "You have selected";
        var outputYes = "Yes";
        var isOutputDisplayed = await Page.Locator(".mt-3").IsVisibleAsync();
        var isSuccessMessageDisplayed = await Page.Locator(".text-success").IsVisibleAsync();
        var output = await Page.Locator(".mt-3").TextContentAsync();
        var successMessage = await Page.Locator(".text-success").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(isButtonSelected, Is.True);
            Assert.That(isButtonEnabled, Is.True);
            Assert.That(isOutputDisplayed, Is.True);
            Assert.That(isSuccessMessageDisplayed, Is.True);            
            Assert.That(output, Does.Contain(outputSelection));
            Assert.That(successMessage, Does.Contain(outputYes));
        });
    }

    [Test]
    public async Task RadioButtonImpressive()
    {
        await Page.Locator("label.custom-control-label[for='impressiveRadio']").ClickAsync();
        var isButtonSelected = await Page.Locator("id=impressiveRadio").IsCheckedAsync();
        var isButtonEnabled = await Page.Locator("id=impressiveRadio").IsEnabledAsync();

        var outputSelection = "You have selected";
        var outputImpresive = "Impressive";
        var isOutputDisplayed = await Page.Locator(".mt-3").IsVisibleAsync();
        var isSuccessMessageDisplayed = await Page.Locator(".text-success").IsVisibleAsync();
        var output = await Page.Locator(".mt-3").TextContentAsync();
        var successMessage = await Page.Locator(".text-success").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(isButtonSelected, Is.True);
            Assert.That(isButtonEnabled, Is.True);
            Assert.That(isOutputDisplayed, Is.True);
            Assert.That(isSuccessMessageDisplayed, Is.True);            
            Assert.That(output, Does.Contain(outputSelection));
            Assert.That(successMessage, Does.Contain(outputImpresive));
        });
    }

    [Test]
    public async Task RadioButtonNo()
    {
        var isButtonEnabled = await Page.Locator("id=noRadio").IsEnabledAsync();

        Assert.That(isButtonEnabled, Is.False);
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
}
