using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WebExtraction.Application.Implementations;
using WebExtraction.Application.Interfaces;
using WebExtraction.Application.Settings;
using WebExtraction.Domain;

namespace WebExtraction.UnitTest
{
    public class HotelServiceUnitTest
    {
        private readonly HotelService _hotelService;

        public HotelServiceUnitTest()
        {
            var htmlFileServiceMock = new Mock<IHtmlFileService>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appSettings.Test.json")
                .Build();

            ApplicationSettings applicationSettings = configuration.GetSection("ApplicationSettings")
                .Get<ApplicationSettings>();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlFilesForTest/KempinskiHotel.html");
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(path);

            htmlFileServiceMock.Setup(service =>
                service.LoadHtml(It.IsAny<string>()))
                .ReturnsAsync(htmlDocument);

            htmlFileServiceMock.Setup(service =>
                    service.GetNodeStringValue(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns("Name1");

            htmlFileServiceMock.Setup(service =>
                    service.GetNodeIntValue(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns(10);

            htmlFileServiceMock.Setup(service =>
                    service.GetNodeFloatValue(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns(8.0F);

            htmlFileServiceMock.Setup(service =>
                    service.GetChildParagraphsValueAsText(It.IsAny<HtmlDocument>(), It.IsAny<string>()))
                .Returns("");

           

            

            IOptions<ApplicationSettings> settings = Options.Create(applicationSettings);

            Mock<ILogger<HotelService>> logger = new Mock<ILogger<HotelService>>();

            _hotelService = new HotelService(htmlFileServiceMock.Object, settings, logger.Object);
        }

        [Test]
        public async Task GetHotelDetailShouldReturnJson()
        {
            string value = await _hotelService.GetHotelDetail();
            JsonConvert.DeserializeObject<HotelInformation>(value);
        }
    }
}
