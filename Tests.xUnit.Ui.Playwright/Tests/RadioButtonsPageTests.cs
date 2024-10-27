using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests;

public class RadioButtonsPageTests : IAsyncLifetime
{
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    public async Task InitializeAsync()
    {
        var playwright = await CreateAsync();
        playwright.Selectors.SetTestIdAttribute("aria-label");

        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 50,
            Timeout = 10000,
            Args = new[] { "--start-maximized" }
        });

        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport,
            Locale = "en-US",
            ColorScheme = ColorScheme.NoPreference
        });

        _page = await _context.NewPageAsync();
        _page.SetDefaultTimeout(10000);
        await _page.GotoAsync("https://demoqa.com/radio-button");
    }
    
    [Fact]
    public async Task OpenRadioButtonPage_TitleIsCorrect()
    {
        var title = await _page.TextContentAsync("h1");

        Assert.Equal("Radio Button", title);
    }
    
    [Fact]
    public async Task ClickYesRadioButton_DisplayMessage()
    {
        await _page.ClickAsync("[for='yesRadio']");
        var doubleClickMessage = await _page.Locator("[class='mt-3']")
            .TextContentAsync();
        
        Assert.Equal("You have selected Yes", doubleClickMessage);
    }
    
    [Fact]
    public async Task ClickImpressiveRadioButton_DisplayMessage()
    {
        await _page.ClickAsync("[for='impressiveRadio']");
        var doubleClickMessage = await _page.Locator("[class='mt-3']")
            .TextContentAsync();
        
        Assert.Equal("You have selected Impressive", doubleClickMessage);
    }
    
    [Fact]
    public async Task CheckNoRadioButtonIsEnabled_ReturnFalse()
    {
        var isEnabledButton = await _page.Locator("[for='noRadio']").IsEnabledAsync();

        Assert.False(isEnabledButton);
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _browser.CloseAsync();
    }

}