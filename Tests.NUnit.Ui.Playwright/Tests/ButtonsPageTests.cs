using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static Microsoft.Playwright.Playwright;
namespace Tests.NUnit.Ui.Playwright.Tests;

[TestFixture]
public class ButtonsPageTests: PageTest
{ private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }
    private IPage Page { get; set; }
    
    
    [SetUp]
    public async Task Setup()
    {
        using var playwright = await CreateAsync();
        playwright.Selectors.SetTestIdAttribute("aria-label");
        
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 50,
            Timeout = 10000,
            Args = ["--start-maximized"],
            
        });
        
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport,
            Locale = "en-US",
            ColorScheme = ColorScheme.NoPreference,
        });

        Page = await Context.NewPageAsync();
        Page.SetDefaultTimeout(10000);
        await Page.GotoAsync("https://demoqa.com/buttons");
    }
    
    [Test]
    public async Task GoToButtonsPage_TitleIsCorrect()
    {
        var title = await Page.TextContentAsync("h1");

        Assert.That(title, Is.EqualTo("Buttons"));
    }
    
    [Test]
    public async Task DoubleClickButton_ReturnsMessage()
    {
        await Page.DblClickAsync("xpath=/html//button[@id='doubleClickBtn']");
        var doubleClickMessage = await Page.Locator("xpath=/html//p[@id='doubleClickMessage']")
            .TextContentAsync();
        
        Assert.That(doubleClickMessage, Is.EqualTo("You have done a double click"));
    }
    
    [Test]
    public async Task RightClickButton_ReturnsMessage()
    {
        await Page.GetByText("Right Click Me").ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });
        var rightClickMessage = await Page.Locator("xpath=/html//p[@id='rightClickMessage']")
            .TextContentAsync();
        
        Assert.That(rightClickMessage, Is.EqualTo("You have done a right click"));
    }
    
    [Test]
    public async Task ClickButton_ReturnsMessage()
    {
        await Page.ClickAsync("xpath=/html//button[@id='doubleClickBtn']");
        var clickMessage = await Page.Locator("xpath=/html//p[@id='dynamicClickMessage']")
            .TextContentAsync();
        
        Assert.That(clickMessage, Is.EqualTo("You have done a dynamic click"));
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
}