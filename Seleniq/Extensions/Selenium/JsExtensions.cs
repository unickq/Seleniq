using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Seleniq.Extensions.Selenium
{
    /// <summary>
    /// Utils class for JavaScript actions.
    /// </summary>
    public static class JsExtensions
    {
        /// <summary>
        /// Draws a frame around IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="style">Style</param>
        /// <param name="cssColor">Color of the CSS.</param>
        /// <param name="frameSize">Frame size</param>
        public static IWebElement JsHighlight(this IWebElement element, string style = "solid", string cssColor = "blue", int frameSize = 2)
        {
            element.ExecuteScript($"arguments[0].style.border='{frameSize}px {style} {cssColor}'", element);
            return element;
        }

        /// <summary>
        /// Draws a frame around IWebElements collection.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="cssColor">Color of the CSS.</param>
        /// <param name="frameSize">Frame size</param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> JsHighlight(this IEnumerable<IWebElement> elements,
            string cssColor = "blue", int frameSize = 3)
        {
            var webElements = elements.ToList();
            foreach (var element in webElements)
            {
                element.JsHighlight("dotted", cssColor, frameSize);
            }
            return webElements;
        }

        /// <summary>
        ///  Cleans the frame around IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void JsDehighlight(this IWebElement element)
        {
            element.ExecuteScript("arguments[0].style.border='0px solid white'", element);
        }

        /// <summary>
        /// Scrolls to IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IWebElement JsFocus(this IWebElement element)
        {
            const string scrollElementIntoMiddle =
                "var vph = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                + "var elTop = arguments[0].getBoundingClientRect().top;"
                + "window.scrollBy(0, elTop-(vph/2));";
            element.ExecuteScript(scrollElementIntoMiddle, element);
            return element;
        }

        /// <summary>
        /// Perfroms JavaScript click on IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        public static void JsClick(this IWebElement element, bool highlight = true)
        {
            JsFocus(element);
            if (highlight) element.JsHighlight();
            element.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Simulates MouseOver JavaScript action.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void JsMouseOver(this IWebElement element)
        {
            element.JsHighlight();
            const string mouseOverScript =
                "var element = arguments[0];"
                + "var mouseEventObj = document.createEvent('MouseEvents');"
                + "mouseEventObj.initEvent( 'mouseover', true, true );"
                + "element.dispatchEvent(mouseEventObj);";

            element.ExecuteScript(mouseOverScript, element);
        }

        /// <summary>
        /// Determines whether IWebElement is loaded.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        ///   <c>true</c> if [is image loaded] [the specified image]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImageLoaded(IWebElement image)
        {
            var isLoaded = false;
            var result = image.ExecuteScript(
                "return arguments[0].complete && " +
                "typeof arguments[0].naturalWidth != \"undefined\" && " +
                "arguments[0].naturalWidth > 0", image);
            if (result is bool) isLoaded = (bool) result;
            return isLoaded;
        }

        /// <summary>
        /// Scrolls page using window.scroll.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="pixels">The pixels.</param>
        public static void JsScrollByHeight(IWebDriver driver, int pixels)
        {
            driver.ExecuteScript($"window.scroll(0, {pixels});");
            Sleep(0.5);
        }

        /// <summary>
        /// Scrolls page using window.scrollBy.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="pixels">The pixels.</param>
        public static void JsScrollBy(this IWebDriver driver, int pixels)
        {
            driver.ExecuteScript($"window.scrollBy(0, {pixels})", "");
            Sleep(0.5);
        }

        /// <summary>
        /// Scrolls page to the end (document.body.scrollHeight).
        /// </summary>
        public static void JsScrollToEnd(this IWebDriver driver)
        {
            driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Sleep(0.5);
        }

        /// <summary>
        /// Scrolls to page top.
        /// </summary>
        public static void JsScrollToTop(this IWebDriver driver)
        {
            driver.ExecuteScript("window.scrollTo(0, 0)");
            Sleep(0.5);
        }

        /// <summary>
        /// Gets IWebElement attributes.
        /// </summary>
        public static IDictionary<string, object> JsGetAttributes(this IWebElement element)
        {
            var arttributesObject = element.ExecuteScript(
                "var items = {}; " +
                "for (index = 0; index < arguments[0].attributes.length; ++index) " +
                "{ items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; " +
                "return items;",
                element);
            return (IDictionary<string, object>) arttributesObject;
        }

        private static void Sleep(double sec)
        {
            Thread.Sleep(TimeSpan.FromSeconds(sec));
        }
    }
}