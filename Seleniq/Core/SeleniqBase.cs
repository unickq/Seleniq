using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Threading;
using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Extensions;

namespace Seleniq.Core
{
    /// <summary>
    /// Base class of Seleniq. Handles IWebDriver
    /// </summary>
    public class SeleniqBase
    {
        protected virtual string BaseUrlEnvVar => "SeleniqBaseUrl";

        protected virtual string BaseUrl
        {
            get
            {
                try
                {
                    var envar = Environment.GetEnvironmentVariable(BaseUrlEnvVar);
                    if (!string.IsNullOrEmpty(envar))
                    {
                        return envar;
                    }
                }
                catch (SecurityException e)
                {
                    Debug.WriteLine(e.Message);
                }

                return ConfigurationManager.AppSettings["BaseUrl"];
            }
        }

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
        [Obsolete("Obsolete. Use InstanceOf<T>(NavigateBy navigation)")]
        protected virtual TPage InstanceOf<TPage>(bool useUrlFromClass) where TPage : SeleniqBase, new()
        {
            var urlAttributeValue = typeof(TPage).GetAttributeValue((NavUrlAttribute attr) => attr.Url);
            if (useUrlFromClass)
            {
                var url = urlAttributeValue;
                if (!urlAttributeValue.StartsWith("http"))
                {
                    BaseUrl.CheckNotNullOrEmpty("BaseUrl", "BaseUrl property is not found on app.config");
                    url = string.Concat(BaseUrl, urlAttributeValue);
                }
                Driver.Navigate().GoToUrl(url);
            }
            return new TPage();
        }

        protected virtual T InstanceOf<T>(NavigateBy navigation) where T : SeleniqBase, new()
        {
            NavUrlAttribute attrType;
            switch (navigation)
            {
                case NavigateBy.PageUrl:
                    attrType = typeof(T).GetCustomAttribute<PageUrlAttribute>();
                    break;
                case NavigateBy.ElementUrl:
                    attrType = typeof(T).GetCustomAttribute<ElementUrlAttribute>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigation), navigation, null);
            }

            if (attrType == null)
            {
                throw new SeleniqException($"{typeof(T).Name} class doesn't have {navigation} attribute");
            }

            var pageUrl = attrType.Url;


            if (!pageUrl.StartsWith("http"))
            {
                BaseUrl.CheckNotNullOrEmpty("BaseUrl", "BaseUrl property is not found on app.config");
                pageUrl = string.Concat(BaseUrl, pageUrl);
            }
            Driver.Navigate().GoToUrl(pageUrl);

            return new T();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Seleniq.Core.PageObject" /> class.
        /// </summary>
        /// <typeparam name="TPage">The type of the page.</typeparam>
        /// <param name="url">Url to navigate before </param>
        /// <returns></returns>
        protected virtual TPage InstanceOf<TPage>(string url)  where TPage : SeleniqBase, new()
        {
            Driver.Navigate().GoToUrl(url);
            return new TPage();
        }
    
        /// <summary>
        /// Initializes an instance of <see cref="T:Seleniq.Core.SeleniqBaseElement" /> class.
        /// </summary>
        /// <typeparam name="TComponent">The element type.</typeparam>
        /// <returns></returns>
        [Obsolete("Page Approach is no longer supported. Please switch to component model")]
        public virtual TComponent InitComponent<TComponent>() where TComponent : SeleniqBaseElement, new()
        {
            return new TComponent();
        }
    }
}