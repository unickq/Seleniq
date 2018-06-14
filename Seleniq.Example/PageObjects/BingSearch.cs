using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Example.PageObjects
{
    public class BingSearch : SeleniqBasePage
    {
        private IList<IWebElement> ResultUrlsCollection => Driver.GetElements(By.XPath(".//*[@id='b_results']/li[*]//cite"));
        private IList<IWebElement> ResultTitlesCollection => Driver.GetElements(By.XPath(".//*[@id='b_results']/li[*]//h2"));

        public void ValidateResultsForUrl(int index, string value)
        {
             StringAssert.Contains(value.ToLowerInvariant(), ResultUrlsCollection[index].CheckNotNull("Result").JsHighlight().Text.ToLowerInvariant());
        }
    }
}
