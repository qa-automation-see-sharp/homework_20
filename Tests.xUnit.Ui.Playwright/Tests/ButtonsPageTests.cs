using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests
{
    public class ButtonsPageTests : IAsyncLifetime
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
            await _page.GotoAsync("https://demoqa.com/buttons");
        }

        [Fact]
        public async Task GoToButtonsPage_TitleIsCorrect()
        {
            var title = await _page.TextContentAsync("h1");
            Assert.Equal("Buttons", title);
        }

        [Fact]
        public async Task DoubleClickButton_ReturnsMessage()
        {
            await _page.DblClickAsync("xpath=/html//button[@id='doubleClickBtn']");
            var doubleClickMessage = await _page.Locator("xpath=/html//p[@id='doubleClickMessage']")
                .TextContentAsync();

            Assert.Equal("You have done a double click", doubleClickMessage);
        }

        [Fact]
        public async Task RightClickButton_ReturnsMessage()
        {
            await _page.GetByText("Right Click Me").ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });
            var rightClickMessage = await _page.Locator("xpath=/html//p[@id='rightClickMessage']")
                .TextContentAsync();

            Assert.Equal("You have done a right click", rightClickMessage);
        }

        [Fact]
        public async Task ClickButton_ReturnsMessage()
        {
            await _page.ClickAsync("xpath=//button[text()='Click Me']");
            var clickMessage = await _page.Locator("id=dynamicClickMessage")
                .TextContentAsync();

            Assert.Equal("You have done a dynamic click", clickMessage);
        }

        public async Task DisposeAsync()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}