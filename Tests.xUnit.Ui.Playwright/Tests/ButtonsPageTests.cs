using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests
{
    public class ButtonsPageTests : IAsyncLifetime
    {
        private IPage _buttonsPage;
        private IBrowser _browser;
        
        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            var browserType = playwright.Chromium;
            _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _buttonsPage = await _browser.NewPageAsync();
            await _buttonsPage.GotoAsync("https://demoqa.com/buttons");
        }

        [Fact]
        public async Task CheckLabelsAreDisplayed()
        {
            var doubleClickButton = _buttonsPage.Locator("#doubleClickBtn");
            await doubleClickButton.DblClickAsync();

            var rightClickButton = _buttonsPage.Locator("#rightClickBtn");
            await rightClickButton.ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });

            var dynamicClickButton = _buttonsPage.Locator("button:has-text('Click Me')").Nth(2);
            await dynamicClickButton.ClickAsync();

            var doubleClickMessage = _buttonsPage.Locator("#doubleClickMessage");
            var rightClickMessage = _buttonsPage.Locator("#rightClickMessage");
            var dynamicClickMessage = _buttonsPage.Locator("#dynamicClickMessage");

            Assert.Equal("You have done a double click", await doubleClickMessage.InnerTextAsync());
            Assert.Equal("You have done a right click", await rightClickMessage.InnerTextAsync());
            Assert.Equal("You have done a dynamic click", await dynamicClickMessage.InnerTextAsync());
        }

        public async Task DisposeAsync()
        {
            await _buttonsPage.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}
