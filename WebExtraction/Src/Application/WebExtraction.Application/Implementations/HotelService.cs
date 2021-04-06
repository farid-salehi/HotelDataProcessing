using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebExtraction.Application.Exceptions;
using WebExtraction.Application.Extensions;
using WebExtraction.Application.Interfaces;
using WebExtraction.Application.Settings;
using WebExtraction.Domain;

namespace WebExtraction.Application.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IHtmlFileService _htmlFileService;
        private readonly ILogger _logger;
        private readonly ApplicationSettings _settings;
        private HtmlDocument _html;
        public HotelService(IHtmlFileService htmlFileService
            , IOptions<ApplicationSettings> settings
            , ILogger<HotelService> logger)
        {
            _htmlFileService = htmlFileService;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<string> GetHotelDetail()
        {
            try
            {
                _logger.LogInformation("GetHotelDetail is called.");
                string path = Path.Combine(Directory.GetCurrentDirectory(), _settings.HotelHtmlFileAddress);
                _html = await _htmlFileService.LoadHtml(path);
                HotelInformation hotelInformation = new HotelInformation()
                {
                    HotelName = _htmlFileService.GetNodeStringValue(_html, _settings.XPaths.HotelNameXpath),
                    Address = _htmlFileService.GetNodeStringValue(_html, _settings.XPaths.HotelAddressXpath),
                    Star = GetHotelStar(),
                    Point = new HotelPoint()
                    {
                        PointDescription =
                            _htmlFileService.GetNodeStringValue(_html, _settings.XPaths.HotelPointDescriptionXpath),
                        Point = _htmlFileService.GetNodeFloatValue(_html, _settings.XPaths.HotelPointXpath),
                        BestPoint = _htmlFileService.GetNodeIntValue(_html, _settings.XPaths.HotelBestPointXpath)
                    },

                    NumberOfReviews = _htmlFileService.GetNodeIntValue(_html, _settings.XPaths.HotelReviewNumberXpath),
                    Description =
                        (_htmlFileService.GetChildParagraphsValueAsText(_html, _settings.XPaths.HotelDescriptionOneXPath)
                         + "\n" + _htmlFileService.GetChildParagraphsValueAsText(_html, _settings.XPaths.HotelDescriptionTwoXPath))
                        .TrimEnd(),

                    RoomCategories = _html.DocumentNode
                        .SelectNodes(_settings.XPaths.HotelRoomCategoryXPath)
                        .Select(x => x.InnerText.TrimStart().TrimEnd())
                        .ToList(),

                    AlternativeHotels = _html.DocumentNode
                        .SelectNodes(_settings.XPaths.AlternativeHotelXPath)
                        .Select(x => new AlternativeHotel()
                        {
                            AlternativeHotelName = x.InnerText.TrimStart().TrimEnd(),
                            Link = x.Attributes
                                .Where(attribute => attribute.Name == "href")
                                .Select(attribute => attribute.Value)
                                .FirstOrDefault()
                        }).ToList()

                };
                string json = JsonConvert.SerializeObject(hotelInformation, Formatting.Indented);
                _logger.LogInformation($"json result is: {json}");
                return json;
            }
            catch (CustomException exception)
            {
                _logger.LogInformation($"Extraction is failed.");
                return exception.Message;
            }
            catch (Exception exception)
            {
                _logger.LogError("Bad request", exception);
                _logger.LogInformation($"Extraction is failed.");
                return "Bad request.";
            }

        }

        private int GetHotelStar()
        {
            try
            {
                HtmlNode node = _html.DocumentNode
                    .SelectNodes(_settings.XPaths.HotelStarXpath)
                    .FirstOrDefault();

                var className
                    = node?.Attributes.Where(x => x.Name == "class")
                        .Select(x => x.Value)
                        .FirstOrDefault();

                if (className != null
                    && int.TryParse(Regex.Match(className, @"\d+").Value, out int star)
                    && star > 0 && star <= 5)
                    return star;

                return 0;
            }
            catch (Exception exception)
            {

                _logger.LogError("GetHotelStar", exception);
                throw exception.ToCustomException();
            }
            

        }
    }
}
