using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebExtraction.Application.Implementations;
using WebExtraction.Application.Settings;

namespace WebExtraction.UnitTest
{
    [TestFixture]
    public class HtmlFileServerXpathMethodsUnitTest
    {
        private readonly HtmlFileService _htmlFileService;
        private readonly  ApplicationSettings _applicationSettings;
        public HtmlFileServerXpathMethodsUnitTest()
        {

            Mock<ILogger<HtmlFileService>> logger = new Mock<ILogger<HtmlFileService>>();
            _htmlFileService = new HtmlFileService(logger.Object);

             IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appSettings.Test.json")
                .Build();

            _applicationSettings = configuration.GetSection("ApplicationSettings")
                .Get<ApplicationSettings>();
        }

        [Test]
        public async Task GetNodeStringValueShouldReturnExpectedValue()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            HtmlDocument html = await _htmlFileService.LoadHtml(path);

            Assert.AreEqual("Kempinski Hotel Bristol Berlin"
                , _htmlFileService.GetNodeStringValue(html, _applicationSettings.XPaths.HotelNameXpath));

            Assert.AreEqual("Kurfürstendamm 27, Charlottenburg-Wilmersdorf, 10719 Berlin, Germany"
                , _htmlFileService.GetNodeStringValue(html, _applicationSettings.XPaths.HotelAddressXpath));

            Assert.AreEqual("Very good"
                , _htmlFileService.GetNodeStringValue(html, _applicationSettings.XPaths.HotelPointDescriptionXpath));
        }
        [Test]
        public async Task GetNodeFloatValueShouldReturnExpectedValue()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            HtmlDocument html = await _htmlFileService.LoadHtml(path);
            Assert.AreEqual( 8.3F
                , _htmlFileService.GetNodeFloatValue(html, _applicationSettings.XPaths.HotelPointXpath));
        }
        [Test]
        public async Task GetNodeIntValueShouldReturnExpectedValue()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            HtmlDocument html = await _htmlFileService.LoadHtml(path);
            Assert.AreEqual(10
                , _htmlFileService.GetNodeFloatValue(html, _applicationSettings.XPaths.HotelBestPointXpath));
        }
        [Test]
        public async Task GetChildParagraphsValueAsText()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            HtmlDocument html = await _htmlFileService.LoadHtml(path);
            string descriptionOne = _htmlFileService.GetChildParagraphsValueAsText(html,
                _applicationSettings.XPaths.HotelDescriptionOneXPath);
            Assert.AreEqual(989
                , descriptionOne.Length);
            string descriptionTwo = _htmlFileService.GetChildParagraphsValueAsText(html,
                _applicationSettings.XPaths.HotelDescriptionTwoXPath);
            Assert.AreEqual(131
                , descriptionTwo.Length);

        }
    }
}
