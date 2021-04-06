using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebExtraction.Application.Exceptions;
using WebExtraction.Application.Implementations;

namespace WebExtraction.UnitTest
{
    [TestFixture]
    public class HtmlFileServerXpathMethodExceptionUnitTest
    {
        private readonly HtmlFileService _htmlFileService;
        public HtmlFileServerXpathMethodExceptionUnitTest()
        {
            Mock<ILogger<HtmlFileService>> logger = new Mock<ILogger<HtmlFileService>>();
            _htmlFileService = new HtmlFileService(logger.Object);
        }

        [Test]
        public void ShouldThrowCustomExceptionForNullHtmlDocument()
        {
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeStringValue(null, ""));
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeIntValue(null, ""));
            Assert.Throws<CustomException>(() => _htmlFileService.GetChildParagraphsValueAsText(null, ""));
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeFloatValue(null, ""));
        }

        [TestCase("")]
        [TestCase("***")]
        [TestCase("//*[@id=\"hp_hotel_name\"")]
        public async Task ShouldThrowCustomExceptionWithInvalidXpath(string xPath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            HtmlDocument html = await _htmlFileService.LoadHtml(path);
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeStringValue(html, xPath));
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeIntValue(html, xPath));
            Assert.Throws<CustomException>(() => _htmlFileService.GetChildParagraphsValueAsText(html, xPath));
            Assert.Throws<CustomException>(() => _htmlFileService.GetNodeFloatValue(html, xPath));
        }
    }
}
