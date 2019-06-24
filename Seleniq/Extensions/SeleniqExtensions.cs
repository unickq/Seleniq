using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;

namespace Seleniq.Extensions
{
    public static class SeleniqExtensions
    {
        public static IWebElement GetWebElementByName(this SeleniqBaseElement element, string name)
        {
            foreach (var property in element.GetType().GetProperties())
            {
                if (!property.PropertyType.Name.Equals(nameof(IWebElement))) continue;
                if (property.Name.Equals(name))
                {
                    try
                    {
                        return property.GetValue(element) as IWebElement;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException != null) throw new NoSuchElementException($"Unable to locate {name} on {element.GetType().Name}");
                    }
                }
                    
            }
            throw new SeleniqException($"Unable to find {name} IWebElement");
        }

        public static IList<T> BuildListOf<T>(this IWebElement root, By by, bool visible = false, params object[] args)
            where T : SeleniqBaseElement
        {
            var elements = root.GetElements(by);
            if (visible) elements = elements.GetVisibleElements();
            return elements.Select(el =>
            {
                var newArgs = new object[args.Length + 1];
                args.CopyTo(newArgs, 1);
                newArgs[0] = el;
                return (T) Activator.CreateInstance(typeof(T), newArgs);
            }).ToList();
        }
    }
}
