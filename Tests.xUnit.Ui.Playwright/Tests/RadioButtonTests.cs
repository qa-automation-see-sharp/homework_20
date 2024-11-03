using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Playwright;

namespace Tests.xUnit.Ui.Playwright.Tests
{
    public class RadioButtonTests : IAsyncLifetime
    {
        private IPage RadioButtonPage { get; set; }
        private IBrowser Browser { get; set; }

        // Setup for xUnit
        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            var browserType = playwright.Chromium;
            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            RadioButtonPage = await Browser.NewPageAsync();
            await RadioButtonPage.GotoAsync("https://demoqa.com/radio-button");
        }

        [Fact]
        public async Task OpenRadioBoxPage_TitleIsCorrect()
        {
            var titleElement = RadioButtonPage.Locator("//h1[contains(text(),'Radio Button')]");
            var title = await titleElement.InnerTextAsync();

            Assert.Equal("Radio Button", title);
        }

        [Fact]
        public async Task CheckYesRadioOutput()
        {
            var yesRadio = RadioButtonPage.Locator("//label[@class='custom-control-label' and @for='yesRadio']");
            await yesRadio.ClickAsync();

            var output = await RadioButtonPage.Locator("//p[@class='mt-3']").InnerTextAsync();

            Assert.Equal("You have selected Yes", output);
        }

        // Teardown for xUnit
        public async Task DisposeAsync()
        {
            await RadioButtonPage.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}
