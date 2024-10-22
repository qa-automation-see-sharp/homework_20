using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static Microsoft.Playwright.Playwright;
namespace Tests.NUnit.Ui.Playwright.Tests;

[TestFixture]
public class TextBoxPageTests : PageTest
{
    private IBrowser Browser { get; set; }
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
        await Page.GotoAsync("https://demoqa.com/text-box");
    }

    [Test]
    public async Task OpenTextBoxPage_TitleIsCorrect()
    {
        
    }
}