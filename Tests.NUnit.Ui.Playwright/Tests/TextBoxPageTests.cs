using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.NUnit.Ui.Playwright.Tests;

[TestFixture]
public class TextBoxPageTests
{
    private IPage TextPage { get; set; }
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var playwright = await CreateAsync();
        var browserType = playwright.Chromium;
        Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        TextPage = await Browser.NewPageAsync();
        await TextPage.GotoAsync("https://demoqa.com/text-box");
    }

    [Test]
    public async Task CheckLabelsAreDisplayed()
    {
        var userNameLabel = await TextPage.TextContentAsync("label[id='userName-label']");
        var userEmailLabel = await TextPage.TextContentAsync("label[id='userEmail-label']");
        var currentAddressLabel = await TextPage.TextContentAsync("label[id='currentAddress-label']");
        var permanentAddressLabel = await TextPage.TextContentAsync("label[id='permanentAddress-label']");

        Assert.Multiple(() =>
        {
            Assert.That(userNameLabel, Is.EqualTo("Full Name"));
            Assert.That(userEmailLabel, Is.EqualTo("Email"));
            Assert.That(currentAddressLabel, Is.EqualTo("Current Address"));
            Assert.That(permanentAddressLabel, Is.EqualTo("Permanent Address"));
        });
    }

    [Test]
    public async Task CompleteTheFormWithData_OutputDisplaysEnteredData()
    {
        await TextPage.FillAsync("#userName", "Oleh Kutafin");
        await TextPage.FillAsync("#userEmail", "kutafin.o.v@gmail.com");
        await TextPage.FillAsync("#currentAddress", "7270 W Manchester Ave, Los Angeles, CA 90045");
        await TextPage.FillAsync("#permanentAddress", "13200 Pacific Promenade, Playa Vista, CA 90094");
        await TextPage.ClickAsync("#submit");

        var outputText = await TextPage.TextContentAsync("#output");

        Assert.Multiple(() =>
        {
            Assert.That(outputText, Is.Not.Null);
            Assert.That(outputText, Is.Not.Empty);
            Assert.That(outputText!.Contains("Oleh Kutafin"));
            Assert.That(outputText.Contains("kutafin.o.v@gmail.com"));
            Assert.That(outputText.Contains("7270 W Manchester Ave, Los Angeles, CA 90045"));
            Assert.That(outputText.Contains("13200 Pacific Promenade, Playa Vista, CA 90094"));
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await TextPage.CloseAsync();
        await Browser.CloseAsync();
    }
}


