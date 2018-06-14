using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleniq.Hubs.CrossBrowserTesting
{
    public class CbtDriver : PaidHub
    {
        private const string HubUrl = "http://hub.crossbrowsertesting.com:80/wd/hub";

        private const string KeyText = "username";
        private const string SecretText = "password";

        public sealed override HubType HubType => HubType.CrossbrowserTesting;

        protected sealed override Uri RestApiUri => new Uri($"https://crossbrowsertesting.com/api/v3/selenium");

        public CbtDriver(ICapabilities capabilities) : base(HubUrl, KeyText, SecretText, capabilities)
        {
        }

        public CbtDriver(string user, string key, DesiredCapabilities capabilities) : base(HubUrl, KeyText, user,
            SecretText, key, capabilities)
        {
        }
    }
}