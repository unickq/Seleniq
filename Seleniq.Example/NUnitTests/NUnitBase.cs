using NUnit.Framework;
using OpenQA.Selenium;
using Seleniq.Core;

namespace Seleniq.Example.NUnitTests
{
    public class NUnitBase<TWebDriver> : SeleniqBase where TWebDriver : IWebDriver, new()
    {
        [SetUp]
        public void SetUp()
        {
            InitWebDriver(new TWebDriver());
        }

        [TearDown]
        public void TearDown()
        {
            KillWebDriver();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }
    }
}
