using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Seleniq.Extensions.Selenium
{
    /// <summary>
    /// Extension class for IWebElement.
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Gets value attribute of IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        /// <summary>
        /// Simulates typing text into the element after clearing text.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static IWebElement TypeInto(this IWebElement element, string text)
        {
            element.Clear();
            if (!string.IsNullOrEmpty(text))
                element.SendKeys(text);
            return element;
        }

        /// <summary>
        /// Determines whether the specified IWebElement has attribute.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        ///   <c>true</c> if the specified attribute name has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(this IWebElement e, string attributeName)
        {
            return !e.GetAttribute(attributeName).Equals(string.Empty);
        }

        /// <summary>
        /// Returns all attributes of IWebElement.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static string ToStringElement(this IWebElement e)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}{{{1}{2}{3}{4}{5}{6}{7}{8}}}",
                e.TagName,
                AppendAttribute(e, "id"),
                AppendAttribute(e, "name"),
                AppendAttribute(e, "value"),
                AppendAttribute(e, "class"),
                AppendAttribute(e, "type"),
                AppendAttribute(e, "role"),
                AppendAttribute(e, "text"),
                AppendAttribute(e, "href"));
        }

        private static string AppendAttribute(this IWebElement e, string attribute)
        {
            var attrValue = attribute == "text" ? e.Text : e.GetAttribute(attribute);
            return string.IsNullOrEmpty(attrValue)
                ? string.Empty
                : string.Format(CultureInfo.CurrentCulture, $" {attribute}='{attrValue}' ");
        }

        /// <summary>
        /// Removes '\r', '\n' from IWebElement text.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>Element text without '\r', '\n'</returns>
        public static string TextClear(this IWebElement element)
        {
            return element.Text.Trim('\r', '\n');
        }

        /// <summary>
        /// Performs Click() and if it throws any exception performs JsClick()
        /// </summary>
        /// <param name="element">The element.</param>
        public static void TryClick(this IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine($"WARN: Unable to click on {element.ToDriver().GetType()} - {e}");
                Console.WriteLine(element.ToStringElement());
                Console.WriteLine("WARN: Going to perform JsClick");
                element.JsClick();
            }
        }

        /// <summary>
        /// Returns Displayed elements in collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static List<IWebElement> GetVisibleElements(this IEnumerable<IWebElement> collection)
        {
            return collection.Where(el => el.Displayed).ToList();
        }

        /// <summary>
        /// Returns Text values of collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static List<string> GetElemetsText(this IEnumerable<IWebElement> collection)
        {
            return collection.Select(el => el.TextClear()).ToList();
        }

        /// <summary>
        /// Highlights the IWebElement, perform action, dehighlights the IWebElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="action">The action.</param>
        public static void HighlightPerformRelease(this IWebElement element, Action action)
        {
            element.JsHighlight();
            action.Invoke();
            element.JsDehighlight();
        }

        /// <summary>
        /// Gets the specified elements parent element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The parent element</returns>
        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath("./parent::*"));
        }

        /// <summary>
        /// Gets the specified elements child element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The child element</returns>
        public static IWebElement GetChild(this IWebElement element)
        {
            return element.FindElement(By.XPath("./child::*"));
        }

        public static void DoubleClick(this IWebElement element)
        {
            var actionsBuilder = new Actions(element.ToDriver());
            var action = actionsBuilder.DoubleClick(element).Build();
            action.Perform();
        }

        public static void RightClick(this IWebElement element)
        {
            var actionsBuilder = new Actions(element.ToDriver());
            var action = actionsBuilder.ContextClick(element).Build();
            action.Perform();
        }

        public static void ClickAndHold(this IWebElement element)
        {
            var actionsBuilder = new Actions(element.ToDriver());
            var action = actionsBuilder.ClickAndHold(element).Build();
            action.Perform();
        }

        public static void DragAndDrop(this IWebElement element, IWebElement targetElement)
        {
            var actionsBuilder = new Actions(element.ToDriver());
            var action = actionsBuilder.DragAndDrop(element, targetElement).Build();
            action.Perform();
        }

        public static By GetBy(this IWebElement element)
        {
            return GetBy(element, out _);
        }

        public static By GetBy(this IWebElement element, out string xpath)
        {
            var attributesDict = element.JsGetAttributes();
            var sb = new StringBuilder();
            sb.Append("//*[");
            foreach (var el in element.JsGetAttributes())
            {
                sb.Append($"@{el.Key}='{el.Value}'");
                if (!el.Equals(attributesDict.Last())) sb.Append(" and ");
            }
            sb.Append("]");
            xpath = sb.ToString();
            return By.XPath(xpath);
        }
    }
}