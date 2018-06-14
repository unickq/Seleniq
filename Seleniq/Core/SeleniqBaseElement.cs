using System.Linq;
using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Core
{
    public abstract class SeleniqBaseElement : SeleniqBase
    {
        protected By RootBy { get; }
        protected IWebElement Root { get; }
        protected SeleniqBaseElement(By by)
        {
            RootBy = by;
            if (Driver.FindElements(by).Count == 0) throw new NoSuchElementException($"Unable to init component {by} on page {Driver.Url}");
            var elements = Driver.FindElements(by);
            Root = elements.First();
            Root.JsFocus().JsHighlight();
            if (elements.Count > 1) System.Console.WriteLine($"WARN: There are {elements.Count} elements {by} on page {Driver.Url}");
        }

        protected SeleniqBaseElement(IWebElement root)
        {
            Root = root;
            Root.JsFocus().JsHighlight();
            RootBy = Root.GetBy();
        }
    }
}
