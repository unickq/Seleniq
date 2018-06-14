using System;
using System.ComponentModel;

namespace Seleniq.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T) attributes[0] : null;
        }

        /// <summary>
        /// Gets Description attribute value.
        /// </summary>
        /// <param name="enumVal">Enum.</param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumVal)
        {
            return enumVal.GetAttributeOfType<DescriptionAttribute>().Description;
        }
    }
}