using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebExtraction.Application.Interfaces;
using Microsoft.Extensions.Logging;
using WebExtraction.Application.Exceptions;
using WebExtraction.Application.Extensions;

namespace WebExtraction.Application.Implementations
{
    public class HtmlFileService : IHtmlFileService
    {
        private readonly ILogger _logger;

        public HtmlFileService(ILogger<HtmlFileService> logger)
        {
            _logger = logger;
        }
        public async Task<HtmlDocument> LoadHtml(string filePath)
        {
            try
            {
                _logger.LogInformation($"Start Loading Html : {filePath}");
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string htmlFile = await streamReader.ReadToEndAsync();
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlFile);
                    if (!IsHtmlDocumentValid(htmlDocument))
                    {
                        throw new CustomException("file: {filePath}, is not a valid html file.");
                    }
                    _logger.LogInformation($"Html is loaded successfully.");
                    return htmlDocument;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("LoadHtml", exception);
                throw exception.ToCustomException();
            }

        }
        public string GetNodeStringValue(HtmlDocument htmlDocument, string xPath)
        {
            try
            {
                _logger.LogInformation($"GetNodeStringValue is Called. xPath: {xPath}");
                HtmlNode node = htmlDocument.DocumentNode
                    .SelectNodes(xPath)
                    .FirstOrDefault();
                string value = node != null ? node.InnerText.TrimStart().TrimEnd() : "Not found";
                _logger.LogInformation($"GetNodeStringValue response: {value}");
                return value;
            }
            catch (Exception exception)
            {
                _logger.LogError("GetNodeStringValue", exception);
                throw exception.ToCustomException();
            }

        }
        public string GetChildParagraphsValueAsText(HtmlDocument htmlDocument, string xPath)
        {
            try
            {
                _logger.LogInformation($"GetChildParagraphsValueAsText is Called. xPath: {xPath}");
                HtmlNode node = htmlDocument.DocumentNode
                .SelectNodes(xPath)
                .FirstOrDefault();

                string value = node != null ? string.Join("     \n ",
                    node.ChildNodes.Where(x => x.Name == "p"
                                               && !string.IsNullOrWhiteSpace(x.InnerText))
                    .Select(x => x.InnerText.TrimStart().TrimEnd())) : "";
                _logger.LogInformation($"GetChildParagraphsValueAsText response: {value}");
                return value;
            }
            catch (Exception exception)
            {
                _logger.LogError("GetChildParagraphsValueAsText", exception);
                throw exception.ToCustomException();
            }
        }
        public float GetNodeFloatValue(HtmlDocument htmlDocument, string xPath)
        {
            try
            {
                _logger.LogInformation($"GetNodeFloatValue is Called. xPath: {xPath}");
                HtmlNode node = htmlDocument.DocumentNode
                .SelectNodes(xPath)
                .FirstOrDefault();

                if (node == null ||
                    !float.TryParse(node.InnerText.Trim(), out float value))
                {
                    return 0;
                }
                _logger.LogInformation($"GetNodeFloatValue response: {value}");
                return value;
            }
            catch (Exception exception)
            {
                _logger.LogError("GetNodeFloatValue", exception);
                throw exception.ToCustomException();
            }
        }
        public int GetNodeIntValue(HtmlDocument htmlDocument, string xPath)
        {
            try
            {
                _logger.LogInformation($"GetNodeIntValue is Called. xPath: {xPath}");
                HtmlNode node = htmlDocument.DocumentNode
                    .SelectNodes(xPath)
                    .FirstOrDefault();


                if (node == null ||
                    !int.TryParse(node.InnerText.Trim(), out int value))
                {
                    return 0;
                }
                _logger.LogInformation($"GetNodeIntValue response: {value}");
                return value;
            }
            catch (Exception exception)
            {
                _logger.LogError("GetNodeIntValue", exception);
                throw exception.ToCustomException();
            }

        }
        private bool IsHtmlDocumentValid(HtmlDocument htmlDocument)
        {
            return (htmlDocument.DocumentNode.SelectSingleNode("html/head") != null
                    && htmlDocument.DocumentNode.SelectSingleNode("html/body") != null);
        }
    }
}
