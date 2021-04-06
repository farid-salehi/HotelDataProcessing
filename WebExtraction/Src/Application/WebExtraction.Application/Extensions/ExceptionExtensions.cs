using System;
using System.Xml.XPath;
using WebExtraction.Application.Exceptions;

namespace WebExtraction.Application.Extensions
{
    public static class ExceptionExtensions
    {
        public static CustomException ToCustomException(this Exception exceptions)
        {
            try
            {
                throw exceptions;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (NullReferenceException)
            {
                throw new CustomException("HtmlDocument is not valid");
            }
            catch (ArgumentNullException)
            {
                throw new CustomException("HtmlDocument is not valid");
            }
            catch (XPathException)
            {
                throw new CustomException("Xpath is not valid");
            }
            catch (Exception)
            {
                throw new CustomException("HtmlDocument Or Xpath is not valid");
            }
        }
    }
}
