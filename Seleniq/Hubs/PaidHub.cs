using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleniq.Hubs
{
    public abstract class PaidHub : RemoteWebDriver
    {
//        protected PaidHub(Uri remoteAddress, ICapabilities capabilities) : base(remoteAddress, capabilities)
//        {
//        }

        protected PaidHub(string hubUrl, string keyText, string secretText, ICapabilities capabilities) : base(
            new Uri(hubUrl), capabilities)
        {
            //ToDo auth from App.Config
            Api = new PaidHubApi(capabilities.GetCapability(keyText).ToString(),
                capabilities.GetCapability(secretText).ToString(), RestApiUri, SessionId.ToString(), HubType);
        }

        protected PaidHub(string hubUrl, string keyText, string user, string secretText, string key,
            DesiredCapabilities capabilities) : base(new Uri(hubUrl),
            Auth(keyText, user, secretText, key, capabilities))
        {
            Api = new PaidHubApi(user, key, RestApiUri, SessionId.ToString(), HubType);
        }

        public PaidHubApi Api { get; }
        public abstract HubType HubType { get; }
        protected abstract Uri RestApiUri { get; }

        protected static ICapabilities Auth(string userCap, string user, string keyCap, string key,
            DesiredCapabilities capabilities)
        {
            capabilities.SetCapability(userCap, user);
            capabilities.SetCapability(keyCap, key);
            return capabilities;
        }
    }
}