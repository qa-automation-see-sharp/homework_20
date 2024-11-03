using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Playwright;

namespace Tests.xUnit.Ui.Playwright.Tests
{
    public class TextBoxPageTests : IAsyncLifetime
    {
        private IPage TextPage { get; set; }
        private IBrowser Browser { get; set; }

        // Setup method for xUnit
        public async Task InitializeAsync()
        {
            var playwright = await CreateAsync();
            var browserType = playwright.Chromium;
            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            TextPage = await Browser.NewPageAsync();
            await TextPage.GotoAsync("https://demoqa.com/text-box");
        }

        [Fact]
        public async Task CheckLabelsAreDisplayed()
        {
            var userNameLabel = await TextPage.TextContentAsync("label[id='userName-label']");
            var userEmailLabel = await TextPage.TextContentAsync("label[id='userEmail-label']");
            var currentAddressLabel = await TextPage.TextContentAsync("label[id='currentAddress-label']");
            var permanentAddressLabel = await TextPage.TextContentAsync("label[id='permanentAddress-label']");

            Assert.Equal("Full Name", userNameLabel);
            Assert.Equal("Email", userEmailLabel);
            Assert.Equal("Current Address", currentAddressLabel);
            Assert.Equal("Permanent Address", permanentAddressLabel);
        }

        [Fact]
        public async Task CompleteTheFormWithData_OutputDisplaysEnteredData()
        {
            // Fill in the form fields
            await TextPage.FillAsync("#userName", "Oleh Kutafin");
            await TextPage.FillAsync("#userEmail", "kutafin.o.v@gmail.com");
            await TextPage.FillAsync("#currentAddress", "7270 W Manchester Ave, Los Angeles, CA 90045");
            await TextPage.FillAsync("#permanentAddress", "13200 Pacific Promenade, Playa Vista, CA 90094");
            await TextPage.ClickAsync("#submit");

            // Get output text content
            var outputText = await TextPage.TextContentAsync("#output");

            // Assertions
            Assert.NotNull(outputText);
            Assert.NotEmpty(outputText);
            Assert.Contains("Oleh Kutafin", outputText);
            Assert.Contains("kutafin.o.v@gmail.com", outputText);
            Assert.Contains("7270 W Manchester Ave, Los Angeles, CA 90045", outputText);
            Assert.Contains("13200 Pacific Promenade, Playa Vista, CA 90094", outputText);
        }

        // Teardown method for xUnit
        public async Task DisposeAsync()
        {
            await TextPage.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}
