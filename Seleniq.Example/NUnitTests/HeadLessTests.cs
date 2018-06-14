using System;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using Seleniq.Core;
using Seleniq.Example.PageObjects;
using Seleniq.Util;

namespace Seleniq.Example.NUnitTests
{
    public class HeadLessTests : SeleniqBase
    {
        [SetUp]
        public void SetUp()
        {
            InitWebDriver(new HeadlessDriver<ChromeDriver>());
        }

        [TearDown]
        public void TearDown()
        {
            KillWebDriver();
        }

        [Test, TestCaseSource(typeof(DataProvider), nameof(DataProvider.TestCases))]
        public void Test(string query, int index, string url)
        {
            var mainPage = InstanceOf<BingMain>(true);
            var searchPage = mainPage.SetQuery(query).ClickSubmit();
            searchPage.ValidateResultsForUrl(index, url);
            Console.WriteLine(((HeadlessDriver<ChromeDriver>) Driver).DriverService.ProcessId);
        }
    }
}
