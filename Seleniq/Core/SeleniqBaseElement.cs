using System;
using OpenQA.Selenium;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Core
{
    public abstract class SeleniqBaseElement : SeleniqBase
    {
        private readonly int _elIndex;
        protected By RootBy { get; }
        protected IWebElement Root { get; private set; }

        protected SeleniqBaseElement ReInit()
        {
            var elements = Driver.Wait().Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(RootBy));
            Root = elements[_elIndex];
            return this;
        }

        protected SeleniqBaseElement(By by, int index = 0)
        {
            _elIndex = index;
            RootBy = by;
            try
            {
                var elements = Driver.Wait().Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                if (elements.Count == 0)
                    throw new NoSuchElementException($"Unable to init component {by} on page {Driver.Url}");
                Root = elements[_elIndex];
                Root.JsFocus().JsHighlight("dashed");
            }
            catch (WebDriverTimeoutException)
            {
                throw new NoSuchElementException($"Unable to init component {by} on page {Driver.Url}");
            }
            catch (Exception)
            {
                throw new NoSuchElementException($"Unable to init component {by} with index {index} on page {Driver.Url}");
            }
        }

        protected SeleniqBaseElement(IWebElement root)
        {
            Root = root;
            Root.JsFocus().JsHighlight();
            RootBy = Root.GetBy();
        }

        protected SeleniqBaseElement(Func<IWebDriver, IWebElement> condition)
        {
            Root = Driver.Wait().Until(condition).JsFocus().JsHighlight();
            RootBy = Root.GetBy();
        }
    }
}
