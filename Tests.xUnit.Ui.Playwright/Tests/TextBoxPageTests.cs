using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace Tests.XUnit.Ui.Playwright.Tests
{
    public class TextBoxPageTests : IAsyncLifetime
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
            await _page.GotoAsync("https://demoqa.com/text-box");
        }

        [Fact]
        public async Task GoToTextBoxPage_TitleIsCorrect()
        {
            var title = await _page.TextContentAsync("h1");
            Assert.Equal("Text Box", title);
        }

        [Fact]
        public async Task CheckCorrectLabelsAreDisplayed()
        {
            var textBoxLabelName = await _page.TextContentAsync("#userName-label");
            var textBoxLabelEmail = await _page.TextContentAsync("#userEmail-label");
            var textBoxLabelCurrentAddress = await _page.TextContentAsync("#currentAddress-label");
            var textBoxLabelPermanentAddress = await _page.TextContentAsync("#permanentAddress-label");

            Assert.Equal("Full Name", textBoxLabelName);
            Assert.Equal("Email", textBoxLabelEmail);
            Assert.Equal("Current Address", textBoxLabelCurrentAddress);
            Assert.Equal("Permanent Address", textBoxLabelPermanentAddress);
        }

        [Fact]
        public async Task CompleteTheForm_EnteredDataIsDisplayed()
        {
            // id selector
            await _page.Locator("id=userName").FillAsync("Liudmyla Savinska");
            // css selector
            await _page.Locator("#userEmail").FillAsync("liudatest@test.com");
            await _page.FillAsync("#currentAddress", "200 Crescent Ave, Covington, KY 41011, United States");
            // xpath selector
            await _page.FillAsync("xpath=//*[@id='permanentAddress']", "6834 Hollywood Blvd\\nLos Angeles, California 90028-6116");

            await _page.GetByText("Submit").ClickAsync();

            var textToAssert = await _page.Locator("xpath=//div[@id='output']/div[@class='border col-md-12 col-sm-12']")
                .TextContentAsync();

            Assert.NotNull(textToAssert);
            Assert.NotEmpty(textToAssert);
            Assert.Contains("Liudmyla Savinska", textToAssert);
            Assert.Contains("liudatest@test.com", textToAssert);
            Assert.Contains("200 Crescent Ave, Covington, KY 41011, United States", textToAssert);
            Assert.Contains("6834 Hollywood Blvd\\nLos Angeles, California 90028-6116", textToAssert);
        }

        public async Task DisposeAsync()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}