using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Example.PageObjects
{
    [PageUrl("/")]
    public class BingMain : SeleniqBasePage
    {
        private IWebElement InputField => Driver.GetElement(By.Id("sb_form_q"));
        private IWebElement SubmitBtn => Driver.GetElement(By.Id("sb_form_go"));

        public BingMain SetQuery(string value)
        {
            InputField.JsHighlight().SendKeys(value);
            return this;
        }

        public BingSearch ClickSubmit()
        {
            SubmitBtn.JsHighlight().Click();
            return new BingSearch();
        }
    }
}
