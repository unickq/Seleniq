using System;
using System.Configuration;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Seleniq.Attributes;
using Seleniq.Extensions;

namespace Seleniq.Core
{
    /// <summary>
    /// Base class of Seleniq. Handles IWebDriver
    /// </summary>
    public class SeleniqBase
    {
        protected virtual string BaseUrl { get; } = ConfigurationManager.AppSettings["BaseUrl"];

        /// <summary>
        /// Gets the WebDriver.
        /// </summary>
        /// <value>
        /// The IWebDriver.
        /// </value>
        protected static IWebDriver Driver =>
            ThreadLocalContext.Value.CheckNotNull(nameof(Driver),
                "Driver = null. Probably driver was not initialized - use InitWebDriver() method first");

        private static readonly ThreadLocal<IWebDriver> ThreadLocalContext = new ThreadLocal<IWebDriver>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.IWebDriver" /> class.
        /// </summary>
        /// <param name="driver">The IWebDriver.</param>
        protected void InitWebDriver(IWebDriver driver)
        {
            ThreadLocalContext.Value = driver;
        }

        /// <summary>
        /// Kills the Driver in current context.
        /// </summary>
        protected void KillWebDriver()
        {
            ThreadLocalContext.Value.Quit();
            ThreadLocalContext.Value = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Seleniq.Core.Initable" /> class.
        /// </summary>
        /// <typeparam name="TPage">The type of the page.</typeparam>
        /// <param name="useUrlFromClass">if set to <c>true</c> [use URL from class].</param>
        /// <returns></returns>
        protected virtual TPage InstanceOf<TPage>(bool useUrlFromClass = false) where TPage : IInitiable
        {
            var urlAttributeValue = typeof(TPage).GetAttributeValue((PageUrlAttribute attr) => attr.Url);
            if (useUrlFromClass)
            {
                var url = urlAttributeValue;
                if (!urlAttributeValue.StartsWith("http"))
                {
                    BaseUrl.CheckNotNullOrEmpty("BaseUrl", "BaseUrl property is not found on app.config");
                    url = string.Concat(BaseUrl, urlAttributeValue);
                }
                return InstanceOf<TPage>(url);
            }
            return (TPage) Activator.CreateInstance(typeof(TPage));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Seleniq.Core.PageObject" /> class.
        /// </summary>
        /// <typeparam name="TPage">The type of the page.</typeparam>
        /// <param name="url">Url to navigate before </param>
        /// <returns></returns>
        protected virtual TPage InstanceOf<TPage>(string url)  where TPage : IInitiable
        {
            Driver.Navigate().GoToUrl(url);
            return InstanceOf<TPage>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.UI.WebDriverWait" /> class.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        protected WebDriverWait BuildWebDriverWait(double seconds = 3)
        {
            return new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
        }
    }
}