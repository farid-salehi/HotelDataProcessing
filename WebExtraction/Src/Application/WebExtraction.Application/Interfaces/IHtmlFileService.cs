using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebExtraction.Application.Interfaces
{
    public interface IHtmlFileService
    {
        Task<HtmlDocument> LoadHtml(string filePath);
        string GetNodeStringValue(HtmlDocument htmlDocument, string xPath);
        float GetNodeFloatValue(HtmlDocument htmlDocument, string xPath);
        int GetNodeIntValue(HtmlDocument htmlDocument, string xPath);
        string GetChildParagraphsValueAsText(HtmlDocument htmlDocument, string xPath);
    }
}
