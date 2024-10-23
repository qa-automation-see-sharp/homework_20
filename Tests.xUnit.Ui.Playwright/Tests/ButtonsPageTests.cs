using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.xUnit.Ui.Playwright.Tests;

public class ButtonsPageTests : IAsyncLifetime
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
        await Page.GotoAsync("https://demoqa.com/buttons");
    }

    [Fact]
    public async Task DoubleClickButtonTest()
    {
        var isButtonDisplayed = await Page.Locator("id=doubleClickBtn").IsVisibleAsync();
        var isButtonEnabled = await Page.Locator("id=doubleClickBtn").IsEnabledAsync();
        await Page.Locator("id=doubleClickBtn").DblClickAsync();

        var outputDoubleClick = "You have done a double click";
        var textOutput = await Page.Locator("id=doubleClickMessage").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.True(isButtonDisplayed);
            Assert.True(isButtonEnabled);
            Assert.False(string.IsNullOrEmpty(textOutput));            
            Assert.Contains(outputDoubleClick, textOutput);            
        });
    }

    [Fact]
    public async Task RightClickButtonTest()
    {
        var isButtonDisplayed = await Page.Locator("id=rightClickBtn").IsVisibleAsync();
        var isButtonEnabled = await Page.Locator("id=rightClickBtn").IsEnabledAsync();
        await Page.Locator("id=rightClickBtn").ClickAsync(new() { Button = MouseButton.Right });

        var outputRightClick = "You have done a right click";
        var textOutput = await Page.Locator("id=rightClickMessage").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.True(isButtonDisplayed);
            Assert.True(isButtonEnabled);
            Assert.False(string.IsNullOrEmpty(textOutput));
            Assert.Contains(outputRightClick, textOutput);            
        });
    }

    [Fact]
    public async Task LeftClickButtonTest()
    {
        var isButtonDisplayed = await Page.Locator("xpath=//button[text()='Click Me']").IsVisibleAsync();
        var isButtonEnabled = await Page.Locator("xpath=//button[text()='Click Me']").IsEnabledAsync();
        await Page.Locator("xpath=//button[text()='Click Me']").ClickAsync();

        var outputLeftClick = "You have done a dynamic click";
        var textOutput = await Page.Locator("id=dynamicClickMessage").TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.True(isButtonDisplayed);
            Assert.True(isButtonEnabled);
            Assert.False(string.IsNullOrEmpty(textOutput));
            Assert.Contains(outputLeftClick, textOutput);            
        });
    }

    public async Task DisposeAsync()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
}