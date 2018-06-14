using System;
using System.Linq;

namespace Seleniq.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att
                ? valueSelector(att)
                : default(TValue);
        }
    }
}