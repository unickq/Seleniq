using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Seleniq.Example.PageObjects;

namespace Seleniq.Example.NUnitTests
{
    [TestFixture]
    [TestFixture(typeof(ChromeDriver))]
//    [TestFixture(typeof(FirefoxDriver))]
    public class TestFixture1<TWebDriver> : NUnitBase<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test, TestCaseSource(typeof(DataProvider), nameof(DataProvider.TestCases))]
        public void T1(string query, int index, string url)
        {
            var mainPage = InstanceOf<BingMain>(true);
            var searchPage = mainPage.SetQuery(query).ClickSubmit();
            searchPage.ValidateResultsForUrl(index, url);
        }
    }
}
