using System;

namespace Seleniq.Core
{
    /// <summary>
    /// SeleniqBasePage - parent class for all classed built by PageObject Pattert
    /// </summary>
    /// <seealso cref="T:Seleniq.Core.SeleniqBase" />
    public abstract class SeleniqBasePage : SeleniqBase
    {
        /// <summary>
        /// Casts <see cref="T:Seleniq.Core.SeleniqBasePage" /> class.
        /// </summary>
        /// <typeparam name="TPage">The type of the page.</typeparam>
        /// <param name="unsafe">if set to <c>false</c> return new instance [unsafe].</param>
        /// <returns></returns>
        public virtual TPage As<TPage>(bool @unsafe = false) where TPage : SeleniqBasePage, new()
        {
            try
            {
                return (TPage) this;
            }
            catch (InvalidCastException)
            {
                if (@unsafe)
                {
                    throw;
                }
                return new TPage();
            }
        }
    }
}