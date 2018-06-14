using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Seleniq.Util
{
    public class HeadlessDriver<T> : IWebDriver where T : RemoteWebDriver
    {
        public T Driver { get; }
        public DriverService DriverService { get; }
        public DriverOptions DriverOptions { get; }

        public HeadlessDriver(DriverOptions options = null, bool hideCommandPromptWindow = true)
        {
            if (typeof(T) == typeof(ChromeDriver))
            {
                DriverService = ChromeDriverService.CreateDefaultService();
                DriverOptions = options;
                if (options == null)
                {
                    var opt = new ChromeOptions();
                    opt.AddArgument("--headless");
                    DriverOptions = opt;
                }
            }
            else if (typeof(T) == typeof(FirefoxDriver))
            {
                DriverService = FirefoxDriverService.CreateDefaultService();
                DriverOptions = options;
                if (options == null)
                {
                    var opt = new FirefoxOptions();
                    opt.AddArgument("--headless");
                    DriverOptions = opt;
                }
            }
            else
            {
                throw new NotImplementedException($"{typeof(T).Name} doesn't support headless mode.");
            }
            DriverService.HideCommandPromptWindow = hideCommandPromptWindow;
            try
            {
                Driver = (T) Activator.CreateInstance(typeof(T), DriverService, DriverOptions,
                    TimeSpan.FromSeconds(60.0));
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException(
                    $"Couldn't initialize {typeof(T).Name} with {options?.GetType().Name}.");
            }
        }

        public IWebElement FindElement(By by)
        {
            return Driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Driver.FindElements(by);
        }

        public void Dispose()
        {
            Driver.Dispose();
        }

        public void Close()
        {
            Driver.Quit();
        }

        public void Quit()
        {
            Driver.Quit();
        }

        public IOptions Manage()
        {
            return Driver.Manage();
        }

        public INavigation Navigate()
        {
            return Driver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return Driver.SwitchTo();
        }

        public string Url
        {
            get { return Driver.Url; }
            set { Driver.Url = value; }
        }

        public string Title => Driver.Title;
        public string PageSource => Driver.PageSource;
        public string CurrentWindowHandle => Driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;
    }
}