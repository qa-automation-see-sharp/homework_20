using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests
{
    public class TextBoxPageTests : IAsyncLifetime
    {
        private IPage _textPage;
        private IBrowser _browser;

        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _textPage = await _browser.NewPageAsync();
            await _textPage.GotoAsync("https://demoqa.com/text-box");
        }

        [Fact]
        public async Task CheckLabelsAreDisplayed()
        {
            var userNameLabel = await _textPage.TextContentAsync("label[id='userName-label']");
            var userEmailLabel = await _textPage.TextContentAsync("label[id='userEmail-label']");
            var currentAddressLabel = await _textPage.TextContentAsync("label[id='currentAddress-label']");
            var permanentAddressLabel = await _textPage.TextContentAsync("label[id='permanentAddress-label']");

            Assert.Equal("Full Name", userNameLabel);
            Assert.Equal("Email", userEmailLabel);
            Assert.Equal("Current Address", currentAddressLabel);
            Assert.Equal("Permanent Address", permanentAddressLabel);
        }

        [Fact]
        public async Task CompleteTheFormWithData_OutputDisplaysEnteredData()
        {
            await _textPage.FillAsync("#userName", "Oleh Kutafin");
            await _textPage.FillAsync("#userEmail", "kutafin.o.v@gmail.com");
            await _textPage.FillAsync("#currentAddress", "7270 W Manchester Ave, Los Angeles, CA 90045");
            await _textPage.FillAsync("#permanentAddress", "13200 Pacific Promenade, Playa Vista, CA 90094");
            await _textPage.ClickAsync("#submit");

            var outputText = await _textPage.TextContentAsync("#output");

            Assert.NotNull(outputText);
            Assert.NotEmpty(outputText);
            Assert.Contains("Oleh Kutafin", outputText);
            Assert.Contains("kutafin.o.v@gmail.com", outputText);
            Assert.Contains("7270 W Manchester Ave, Los Angeles, CA 90045", outputText);
            Assert.Contains("13200 Pacific Promenade, Playa Vista, CA 90094", outputText);
        }

        public async Task DisposeAsync()
        {
            await _textPage.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}
