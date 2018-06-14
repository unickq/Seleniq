using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace Seleniq.Extensions.Selenium
{
    /// <summary>
    ///     Extension class for ISearchContext.
    /// </summary>
    public static class SearchContextExtensions
    {
        public const int MediumTimeout = 4;

        public static IWebElement GetElement(this ISearchContext context, By locator, double timeout = MediumTimeout,
            string message = null)
        {
            return context.GetElement(locator, e => e.Displayed & e.Enabled, timeout, message);
        }

        public static IWebElement GetElement(this ISearchContext context, By locator, Func<IWebElement, bool> condition,
            double timeout = MediumTimeout, string message = null)
        {
            try
            {
                var wait = context.Wait(timeout, message);
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(driver => condition(context.FindElement(locator)));
                return context.FindElement(locator);
            }
            catch (WebDriverTimeoutException e)
            {
                throw new WebDriverTimeoutException(
                    $"Unable to find locator {locator} on page {context.ToDriver().Url}\n{e.Message}");
            }
        }

        public static IWebElement GetElement(this ISearchContext context, Func<IWebDriver, IWebElement> condition,
            double timeout = MediumTimeout, string message = null)
        {
            var wait = context.Wait(timeout, message);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            return wait.Until(condition);
        }


        public static IList<IWebElement> GetElements(this ISearchContext context, By by)
        {
            return context.GetElements(by, e => e.Displayed && e.Enabled);
        }

        public static IList<IWebElement> GetElements(this ISearchContext context, By by,
            Func<IWebElement, bool> condition)
        {
            return context.FindElements(by).Where(condition).ToList();
        }

        public static IList<IWebElement> GetElements(this ISearchContext context,
            Func<IWebDriver, ReadOnlyCollection<IWebElement>> condition, double timeout = MediumTimeout,
            string message = null)
        {
            return context.Wait(timeout, message).Until(condition).ToList();
        }

        /// <summary>
        ///  Casts to IWebDriver.
        /// </summary>
        /// <param name="context">ISearchContext.</param>
        /// <returns></returns>
        public static IWebDriver ToDriver(this ISearchContext context)
        {
            var iWrappedElement = context as IWrapsDriver;
            if (iWrappedElement == null)
            {
                try
                {
                    return (IWebDriver) context;
                }
                catch (InvalidCastException)
                {
                    var fieldInfo = context.GetType()
                        .GetField("underlyingElement", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo == null) return null;
                    iWrappedElement = fieldInfo.GetValue(context) as IWrapsDriver;
                    if (iWrappedElement == null)
                        throw new ArgumentException("Element must wrap a web driver", nameof(context));
                }
            }
            return iWrappedElement.WrappedDriver;
        }

        /// <summary>
        /// Return an instance of WebDriverWait.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="message">The exception message.</param>
        /// <returns></returns>
        public static WebDriverWait Wait(this ISearchContext context, double timeout = MediumTimeout,
            string message = null)
        {
            return new WebDriverWait(context.ToDriver(), TimeSpan.FromSeconds(timeout)) {Message = message};
        }

        private static IJavaScriptExecutor JsExecutor(this IWebDriver driver)
        {
            return (IJavaScriptExecutor) driver;
        }

        /// <summary>
        ///     Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        public static object ExecuteScript(this ISearchContext context, string script, params object[] args)
        {
            return context.ToDriver().JsExecutor().ExecuteScript(script, args);
        }

        /// <summary>
        ///     Executes JavaScript asynchronously in the context of the currently selected frame or window.
        /// </summary>
        public static object ExecuteAsyncScript(this ISearchContext context, string script, params object[] args)
        {
            return context.ToDriver().JsExecutor().ExecuteAsyncScript(script, args);
        }
    }
}