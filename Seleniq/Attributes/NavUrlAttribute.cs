using System;

namespace Seleniq.Attributes
{
    public enum NavigateBy
    {
        PageUrl,
        ElementUrl
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class PageUrlAttribute : NavUrlAttribute
    {
        public PageUrlAttribute(string url) : base(url)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ElementUrlAttribute : NavUrlAttribute
    {
        public ElementUrlAttribute(string url) : base(url)
        {
        }
    }

    public class NavUrlAttribute : Attribute
    {
        protected NavUrlAttribute(string url)
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