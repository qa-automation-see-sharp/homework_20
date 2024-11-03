using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using static Microsoft.Playwright.Playwright;

namespace Tests.NUnit.Ui.Playwright.Tests
{
    [TestFixture]
    internal class RadioButtonTests
    {

        private IPage RadioButtonPage { get; set; }
        private IBrowser Browser { get; set; }
        private IBrowserContext Context { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            var playwright = await CreateAsync();
            var browserType = playwright.Chromium;
            Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            RadioButtonPage = await Browser.NewPageAsync();
            // Adjusted URL to match the actual test intention
            await RadioButtonPage.GotoAsync("https://demoqa.com/radio-button");
        }

        [Test]
        public async Task OpenRadioBoxPage_TitleIsCorrect()
        {
            var titleElement = RadioButtonPage.Locator("//h1[contains(text(),'Radio Button')]");
            var title = await titleElement.InnerTextAsync();

            Assert.That(title, Is.EqualTo("Radio Button"));
        }

        [Test]
        public async Task CheckYesRadioOutput()
        {
            var yesRadio = RadioButtonPage.Locator("//label[@class='custom-control-label' and @for='yesRadio']");
            await yesRadio.ClickAsync();

            var output = await RadioButtonPage.Locator("//p[@class='mt-3']").InnerTextAsync();

            Assert.Multiple(() =>
            {
                Assert.That(output, Is.EqualTo("You have selected Yes"));
            });
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await RadioButtonPage.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}

