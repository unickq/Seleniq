﻿using NUnit.Framework;
using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Example.PageObjects;
using Seleniq.Example.PageObjects.MainPage;
using Seleniq.Example.PageObjects.SearchPage;
using Seleniq.Extensions.Selenium;
using Seleniq.SpecFlow;
using TechTalk.SpecFlow;

namespace Seleniq.Example.StepsDefinitions
{
    [Binding]
    public class BingSteps : SeleniqBaseSpecFlow
    {
        public BingSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"I have opened Bing main page")]
        public void GivenIHaveOpenedBingMainPage()
        {
            Cache<BingMain>(NavigateBy.PageUrl);
        }

        [Given(@"I have entered (.*) into search box and clicked search button")]
        public void GivenIHaveEnteredIntoSearchBoxAndClickedSearchButton(string value)
        {
            Cache<SearchElement>(NavigateBy.ElementUrl).SetQuery(value).ClickSubmit();
        }

        [Then(@"Page title should contain (.*)")]
        public void ThenPageTitleShouldContain(string value)
        {
            StringAssert.Contains(value, Driver.Title);
        }

        [Then(@"I see result with index (.*) url is equal (.*)")]
        public void ThenISeeResultWithIndexUrlIsEqual(int index, string value)
        {
            Cache<BingSearch>().ValidateResultsForUrl(index, value);
        }

    }
}
