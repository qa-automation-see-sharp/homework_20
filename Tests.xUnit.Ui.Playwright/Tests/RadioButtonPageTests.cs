using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests
{
    public class RadioButtonPageTests : IAsyncLifetime
    {
        private IPage _radioButtonsPage;
        private IBrowser _browser;

        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _radioButtonsPage = await _browser.NewPageAsync();
            await _radioButtonsPage.GotoAsync("https://demoqa.com/radio-button");
        }

        [Fact]
        public async Task OpenRadioBoxPage_TitleIsCorrect()
        {
            var titleElement = _radioButtonsPage.Locator("//h1[contains(text(),'Radio Button')]");
            var title = await titleElement.InnerTextAsync();

            Assert.Equal("Radio Button", title);
        }

        [Fact]
        public async Task CheckYesRadioOutput()
        {
            var yesRadio = _radioButtonsPage.Locator("//label[@class='custom-control-label' and @for='yesRadio']");
            await yesRadio.ClickAsync();

            var output = await _radioButtonsPage.Locator("//p[@class='mt-3']").InnerTextAsync();

            Assert.Equal("You have selected Yes", output);
        }

        public async Task DisposeAsync()
        {
            await _radioButtonsPage.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}
