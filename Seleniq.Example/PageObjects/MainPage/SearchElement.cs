using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Core;
using Seleniq.Example.PageObjects.SearchPage;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Example.PageObjects.MainPage
{
    [ElementUrl("/")]
    public class SearchElement : SeleniqBaseElement
    {
        public SearchElement() : base(By.CssSelector(".b_searchboxForm"))
        {
        }

        public SearchElement SetQuery(string value)
        {
            Root.GetElement(By.Id("sb_form_q"), message: "Can't find input field").JsHighlight().SendKeys(value);
            return this;
        }

        public BingSearch ClickSubmit()
        {
            Root.GetElement(By.Id("sb_form_go"), message: "Can't search icn button").JsHighlight().Click();
            Root.Wait().Until(ExpectedConditions.UrlContains("search?"));
            return new BingSearch();
        }
    }
}