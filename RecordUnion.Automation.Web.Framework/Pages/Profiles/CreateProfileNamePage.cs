using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class CreateProfileNamePage
    {

        private IWebDriver _driver;
        

        [FindsBy(How = How.TagName, Using = "input")]
        private IWebElement _profileName;

        [FindsBy(How = How.ClassName, Using = "actions-bar")]
        private IWebElement _actionMenu;

        [FindsBy(How = How.ClassName, Using = "logo")]
        private IWebElement logo;

        private IWebElement nextButton;

        public CreateProfileNamePage(IWebDriver driver)
        {
            _driver = driver;
          //  PageFactory.InitElements(driver, this);
        }

        public bool CheckIfNextButtonIsEnabled()
        {
            FindNextButton();
            return nextButton.IsEnabled();
        }

        private void FindNextButton() => nextButton = _actionMenu.FindElement(By.TagName("button"));

        public CreateProfileLocationPage PopulateProfileNameAndClickNext(String profileName)
        {
            PopulateProfileName(profileName);
            if (nextButton == null)
            {
                FindNextButton();
            }
            if (nextButton.IsEnabled())
                nextButton.Click();
            else throw new Exception("Test Flow exception: The button is expected to be enabled and it isn't");
            Thread.Sleep(5000);
            return new CreateProfileLocationPage(this._driver);
        }

        public CreateProfileNamePage PopulateProfileName(string profileName)
        {
            _profileName.SendKeys(profileName);
            return new CreateProfileNamePage(this._driver);
        }

        public CreateProfileLocationPage ClickOnNextButtonToNavigateOnLocationScreen()
        {
            if (nextButton==null)
            {
                FindNextButton();
            }
            nextButton.Click();
            Thread.Sleep(5000);
            return new CreateProfileLocationPage(this._driver);
        }


        public DashboardPage ClickOnLogoToReturnOnAllProfilesPage()
        {
            logo.Click();
            return new DashboardPage(_driver);
        }

        public IList<SuggestedMatchingResults> findAlreadyCreatedProfileWithExactName(string expectedProfileName)
        {
            IList<SuggestedMatchingResults> ListOfSuggestedExistingProfiles = new List<SuggestedMatchingResults>();

            IWebElement listContainer = _driver.FindElement(By.ClassName("rows"));
            var Rows = listContainer.FindElements(By.CssSelector(".kit-row.overflowed.done"));
            foreach(IWebElement elem in Rows)
            {
                var SuggestedProfileName = elem.FindElement(By.ClassName("kit-element"));
                
                var SuggestedProfileLocation = elem.FindElement(By.CssSelector(".typography-b3.wrappable.kit-element"));
                var SuggestedProfileLink = elem.FindElement(By.CssSelector(".kit-svg"));
                ListOfSuggestedExistingProfiles.Add(new SuggestedMatchingResults(SuggestedProfileName, SuggestedProfileLocation, SuggestedProfileLink));
            }

            return ListOfSuggestedExistingProfiles;
        }

        public CreateProfileNamePage ClickNextButton()
        {
            FindNextButton();
            nextButton.Click();
            return new CreateProfileNamePage(this._driver);
        }

    }
}