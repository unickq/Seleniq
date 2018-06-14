using System;

namespace Seleniq.Attributes
{
    /// <summary>
    /// Url attribute of of the <see cref="T:Seleniq.Core.PageObject" /> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PageUrlAttribute : Attribute
    {
        public PageUrlAttribute(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; }
    }
}