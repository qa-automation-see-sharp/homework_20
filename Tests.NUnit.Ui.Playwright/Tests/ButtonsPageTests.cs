using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;
namespace Tests.NUnit.Ui.Playwright.Tests;

[TestFixture]
public class ButtonsPageTests
{
    private IPage ButtonsPage { get; set; }
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var playwright = await CreateAsync();
        var browserType = playwright.Chromium;
        Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        ButtonsPage = await Browser.NewPageAsync();
        await ButtonsPage.GotoAsync("https://demoqa.com/buttons");
    }

    [Test]
    public async Task CheckLabelsAreDisplayed()
    {
        var doubleClickButton = ButtonsPage.Locator("#doubleClickBtn");
        await doubleClickButton.DblClickAsync();

        var rightClickButton = ButtonsPage.Locator("#rightClickBtn");
        await rightClickButton.ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });

        var dynamicClickButton = ButtonsPage.Locator("button:has-text('Click Me')").Nth(2);
        await dynamicClickButton.ClickAsync();

        var doubleClickMessage = ButtonsPage.Locator("#doubleClickMessage");
        var rightClickMessage = ButtonsPage.Locator("#rightClickMessage");
        var dynamicClickMessage = ButtonsPage.Locator("#dynamicClickMessage");

        Assert.Multiple(async () =>
        {
            Assert.That(await doubleClickMessage.InnerTextAsync(), Is.EqualTo("You have done a double click"));
            Assert.That(await rightClickMessage.InnerTextAsync(), Is.EqualTo("You have done a right click"));
            Assert.That(await dynamicClickMessage.InnerTextAsync(), Is.EqualTo("You have done a dynamic click"));
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await ButtonsPage.CloseAsync();
        await Browser.CloseAsync();
    }
}