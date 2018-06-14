using System;
using System.Reflection;

namespace Seleniq.Extensions
{
    public static class ValidateExtensions
    {
        /// <summary>
        /// Checks that T is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T CheckNotNull<T>(this T value, string argumentName, string errorMessage = null)
        {
            if (value != null) return value;
            throw new ArgumentNullException(argumentName, errorMessage);
        }

        /// <summary>
        /// Checks that string is not null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string CheckNotNullOrEmpty(this string value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (value == string.Empty)
                throw new ArgumentException($"String {value} shouldn't be empty - {errorMessage}", argumentName);
            return value;
        }

        internal static T GetFieldValue<T>(this object obj, string name)
        {
            var field = obj.GetType()
                .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (T) field?.GetValue(obj);
        }
    }
}