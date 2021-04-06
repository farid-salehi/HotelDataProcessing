using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebExtraction.Application.Exceptions;
using WebExtraction.Application.Implementations;

namespace WebExtraction.UnitTest
{
    [TestFixture]
    public class HtmlFileServiceLoadHtmlUnitTests
    {
        private readonly HtmlFileService _htmlFileService;
        public HtmlFileServiceLoadHtmlUnitTests()
        {
            Mock<ILogger<HtmlFileService>> logger = new Mock<ILogger<HtmlFileService>>();
            _htmlFileService = new HtmlFileService(logger.Object);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShodCustomAExceptionIfPathIsNotValid(string path)
        {
            Assert.ThrowsAsync<CustomException>(() => _htmlFileService.LoadHtml(path));

        }

        [TestCase("HtmlFilesForTest/NotExist.html")]
        [TestCase("HtmlFilesForTest/test.png")]
        [TestCase("HtmlFilesForTest/test.txt")]
        public void ShodThrowCustomExceptionIfFileIsNotValid(string file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), file);
            Assert.ThrowsAsync<CustomException>(() => _htmlFileService.LoadHtml(path));

        }
    }
}
