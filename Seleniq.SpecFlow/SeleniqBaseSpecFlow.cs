using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Core;
using Seleniq.Extensions;
using TechTalk.SpecFlow;

namespace Seleniq.SpecFlow
{
    public class SeleniqBaseSpecFlow : SeleniqBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SeleniqBaseSpecFlow" /> class.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <exception cref="System.ArgumentNullException">scenarioContext</exception>
        protected SeleniqBaseSpecFlow(ScenarioContext scenarioContext)
        {
            LocalScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            InitWebDriver(scenarioContext.Get<IWebDriver>("Driver"));
//            Driver
        }

        /// <summary>
        ///     Thread safe SpecFlow scenario context.
        /// </summary>
        protected ScenarioContext LocalScenarioContext { get; }

        protected SeleniqBasePage CurrentPage
        {
            get
            {
                if (LocalScenarioContext.ContainsKey(nameof(CurrentPage)))
                    return LocalScenarioContext.Get<SeleniqBasePage>(nameof(CurrentPage));
                throw new SpecFlowException("CurrentPage key doesn't exist in ScenarioContext. Set it first");
            }
            set => LocalScenarioContext[nameof(CurrentPage)] = value;
        }

        /// <summary>
        ///     Gets/Sets value to ScenarioContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reInit">if set to <c>true</c> inits new instance of T.</param>
        /// <returns></returns>
        protected T Cache<T>(bool reInit = false) where T : new()
        {
            var key = typeof(T).FullName;
            if (LocalScenarioContext.ContainsKey(key))
            {
                if (reInit)
                {
                    LocalScenarioContext[key] = new T();
                }
            }
            else
            {
                LocalScenarioContext[key] = new T();
            }

            return LocalScenarioContext.Get<T>(key);
        }

        /// <summary>
        ///     Gets/Sets value to ScenarioContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="reInit">if set to <c>true</c> overwrites object.</param>
        /// <returns></returns>
        protected T Cache<T>(string key, object value, bool reInit = false)
        {
            if (LocalScenarioContext.ContainsKey(key))
            {
                if (reInit) LocalScenarioContext[key] = value;
            }
            else
            {
                LocalScenarioContext[key] = value;
            }

            return LocalScenarioContext.Get<T>(key);
        }

        /// <summary>
        /// Gets value from cache.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        protected T Cache<T>(string key)
        {
            if (LocalScenarioContext.ContainsKey(key))
                return LocalScenarioContext.Get<T>(key);
            throw new KeyNotFoundException($"{key} doesn't exist in cache");
        }

        protected T Cache<T>(NavigateBy navigation) where T : SeleniqBase, new()
        {
            LocalScenarioContext[typeof(T).FullName ?? throw new InvalidOperationException("Type is null")] =
                InstanceOf<T>(navigation);
            return LocalScenarioContext.Get<T>(typeof(T).FullName);
        }
    }
}