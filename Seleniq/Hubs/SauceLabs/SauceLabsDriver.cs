using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleniq.Hubs.SauceLabs
{
    public class SauceLabsDriver : PaidHub
    {
        private const string HubUrl = "http://ondemand.saucelabs.com:80/wd/hub";
        private const string KeyText = "username";
        private const string SecretText = "accessKey";
        private readonly string _secretUser;

        protected sealed override Uri RestApiUri =>
            new Uri($"https://saucelabs.com/rest/v1/{_secretUser}/jobs/{SessionId}");

        public sealed override HubType HubType => HubType.Saucelabs;

        public SauceLabsDriver(ICapabilities capabilities) : base(HubUrl, KeyText, SecretText, capabilities)
        {
            _secretUser = capabilities.GetCapability(KeyText).ToString();
        }

        public SauceLabsDriver(string user, string key, DesiredCapabilities capabilities) : base(HubUrl, KeyText, user,
            SecretText, key, capabilities)
        {
            _secretUser = user;
        }
    }
}