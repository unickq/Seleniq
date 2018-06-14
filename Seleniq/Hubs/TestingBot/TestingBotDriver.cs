using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleniq.Hubs.TestingBot
{
    public class TestingBotDriver : PaidHub
    {
        private const string HubUrl = "http://hub.testingbot.com/wd/hub/";
        private const string KeyText = "key";
        private const string SecretText = "secret";

        public sealed override HubType HubType => HubType.Testingbot;

        protected sealed override Uri RestApiUri => new Uri($"https://api.testingbot.com/v1/tests/{SessionId}");

        public TestingBotDriver(ICapabilities capabilities) : base(HubUrl, KeyText, SecretText, capabilities)
        {
        }

        public TestingBotDriver(string user, string key, DesiredCapabilities capabilities) : base(HubUrl, KeyText, user,
            SecretText, key, capabilities)
        {
        }
    }
}