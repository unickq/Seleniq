using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Seleniq.Attributes;
using Seleniq.Example.PageObjects;
using Seleniq.Example.PageObjects.MainPage;

namespace Seleniq.Example.NUnitTests
{
    [TestFixture]
    [TestFixture(typeof(ChromeDriver))]
//    [TestFixture(typeof(FirefoxDriver))]
    public class TestFixture2<TWebDriver> : NUnitBase<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test, TestCaseSource(typeof(DataProvider), nameof(DataProvider.TestCases))]
        public void T2(string query, int index, string url)
        {
            var mainPage = InstanceOf<BingMain>(NavigateBy.PageUrl);
            var searchPage = mainPage.SearchElement.SetQuery(query).ClickSubmit();
            searchPage.ValidateResultsForUrl(index, url);
        }
    }
}