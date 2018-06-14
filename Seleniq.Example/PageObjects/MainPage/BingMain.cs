using Seleniq.Attributes;
using Seleniq.Core;

namespace Seleniq.Example.PageObjects.MainPage
{
    [PageUrl("/")]
    public class BingMain : SeleniqBasePage
    {
        public SearchElement SearchElement => new SearchElement();
    }
}
