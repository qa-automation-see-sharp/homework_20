//namespace Tests.xUnit.Ui.Playwright.Tests;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Playwright;

namespace Tests.xUnit.Ui.Playwright.Tests
{
    public class ButtonsPageTests : IAsyncLifetime
    {
        private IPage ButtonsPage { get; set; }
        private IBrowser Browser { get; set; }

        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            var browserType = playwright.Chromium;
            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            ButtonsPage = await Browser.NewPageAsync();
            await ButtonsPage.GotoAsync("https://demoqa.com/buttons");
        }

        [Fact]
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

            Assert.Equal("You have done a double click", await doubleClickMessage.InnerTextAsync());
            Assert.Equal("You have done a right click", await rightClickMessage.InnerTextAsync());
            Assert.Equal("You have done a dynamic click", await dynamicClickMessage.InnerTextAsync());
        }

        public async Task DisposeAsync()
        {
            await ButtonsPage.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}
