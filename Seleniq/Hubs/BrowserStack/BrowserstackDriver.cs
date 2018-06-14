using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleniq.Hubs.BrowserStack
{
    public class BrowserstackDriver : PaidHub
    {
        private const string HubUrl = "http://hub-cloud.browserstack.com/wd/hub/";

        private const string KeyText = "browserstack.user";
        private const string SecretText = "browserstack.key";

        public sealed override HubType HubType => HubType.Browserstack;

        protected sealed override Uri RestApiUri =>
            new Uri($"https://www.browserstack.com/automate/sessions/{SessionId}.json");

        public BrowserstackDriver(ICapabilities capabilities) : base(HubUrl, KeyText, SecretText, capabilities)
        {
        }

        public BrowserstackDriver(string user, string key, DesiredCapabilities capabilities) : base(HubUrl, KeyText,
            user, SecretText, key, capabilities)
        {
        }
    }
}