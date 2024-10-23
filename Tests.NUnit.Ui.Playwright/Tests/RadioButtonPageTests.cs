using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.NUnit.Ui.Playwright.Tests;

//TODO: Add tests
[TestFixture]
public class RadioButtonPageTests
{
    private IPage RadioButtonsPage { get; set; }
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var playwright = await CreateAsync();
        var browserType = playwright.Chromium;
        Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        RadioButtonsPage = await Browser.NewPageAsync();
        await RadioButtonsPage.GotoAsync("https://demoqa.com/radio-button");
    }

    [Test]
    public async Task OpenRadioBoxPage_TitleIsCorrect()
    {
        var titleElement = RadioButtonsPage.Locator("//h1[contains(text(),'Radio Button')]");
        var title = await titleElement.InnerTextAsync();

        Assert.That(title, Is.EqualTo("Radio Button"));
    }

    [Test]
    public async Task CheckYesRadioOutput()
    {
        var yesRadio = RadioButtonsPage.Locator("//label[@class='custom-control-label' and @for='yesRadio']");
        await yesRadio.ClickAsync();

        var output = await RadioButtonsPage.Locator("//p[@class='mt-3']").InnerTextAsync();

        Assert.Multiple(() =>
        {
            Assert.That(output, Is.EqualTo("You have selected Yes"));
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await RadioButtonsPage.CloseAsync();
        await Browser.CloseAsync();
    }
}