Feature: BingSearch1
	In order to check test works fine
	I open Bing page and perform some test

@Browser:ChromeLocal
#@Browser:FirefoxLocal
Scenario Outline: Open Bing and 
	Given I have opened Bing main page
	And I have entered <searchQuery> into search box and clicked search button
	Then Page title should contain <searchQuery>
	And I see result with index <index> url is equal <url>
	Examples: 
	| searchQuery | index | url     |
	| github      | 0     | github  |
	| youtube     | 0     | youtube |
	