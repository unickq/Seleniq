using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Seleniq.Util;

namespace Seleniq.Extensions.Selenium
{
    /// <summary>
    /// Extensions class for IWebDriver
    /// </summary>
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Maximizes the browser.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <returns></returns>
        public static IWebDriver Maximize(this IWebDriver driver)
        {
            driver.CheckNotNull(nameof(driver));
            driver.Manage().Window.Maximize();
            return driver;
        }

        /// <summary>
        /// Tries the maximize without throwing an exception.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public static void TryMaximize(this IWebDriver driver)
        {
            try
            {
                Maximize(driver);
            }
            catch (Exception)
            {
                //ignored
            }
        }

        /// <summary>
        /// Sets the size of the outer browser window, including title bars and window borders.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static IWebDriver SetSize(this IWebDriver driver, int width, int height)
        {
            driver.CheckNotNull(nameof(driver));
            driver.Manage().Window.Size = new Size(width, height);
            return driver;
        }

        /// <summary>
        /// Sets the position of the browser window relative to the upper-left corner of the screen.
        /// </summary>
        /// <returns></returns>
        public static IWebDriver SetPosition(this IWebDriver driver, int x, int y)
        {
            driver.CheckNotNull(nameof(driver));
            driver.Manage().Window.Position = new Point(x, y);
            return driver;
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="uri">The URI.</param>
        public static void NavigateTo(this IWebDriver driver, Uri uri)
        {
            driver.Navigate().GoToUrl(uri);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="uri">The URI.</param>
        public static void NavigateTo(this IWebDriver driver, string uri)
        {
            driver.Navigate().GoToUrl(uri);
        }

        /// <summary>
        /// Checks whether WebDriver title contains text
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool TitleContains(this IWebDriver driver, string text)
        {
            driver.CheckNotNull(nameof(driver));
            text.CheckNotNullOrEmpty(nameof(text));
            return driver?.Title.Contains(text) ?? false;
        }

        /// <summary>
        /// Load a new web page in the current browser window and prints load type in milliseconds.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="uri">The URI.</param>
        /// <param name="prefix">Message prefix</param>
        public static void NavigateAndPrintTime(this IWebDriver driver, string uri, string prefix = "")
        {
            var perf = new PerformanceTool();
            perf.StartMeasure();
            driver.Navigate().GoToUrl(uri);
            Console.WriteLine(perf.StopMeasure(prefix + uri));
        }

        /// <summary>
        /// Performs actions.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        public static IWebDriver Perform(this IWebDriver driver, Func<Actions, Actions> actions)
        {
            driver.CheckNotNull(nameof(driver));
            actions.CheckNotNull(nameof(actions));
            var act = new Actions(driver);
            act = actions(act);
            act.Perform();           
            return driver;
        }

        public static bool IsMobileDevice(this IWebDriver driver)
        {
            var userAgent = driver.ExecuteScript("return navigator.userAgent;").ToString().ToLower();
            if (userAgent.Contains("android")) return true;
            if (userAgent.Contains("iphone")) return true;
            if (userAgent.Contains("ipad")) return true;
            if (userAgent.Contains("windows phone")) return true;
            return false;
        }
    }
}