using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class CreateProfileLocationPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.ClassName, Using = "rows")]
        private IWebElement _fullResultSet;
        
        [FindsBy(How = How.ClassName, Using = "actions-bar")]
        private IWebElement _actionMenu;

        [FindsBy(How = How.TagName, Using = "input")]
        private IWebElement inputLocations;

        private IWebElement _nextButton;


        public CreateProfileLocationPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        private IList<IWebElement> getAllLocationsReturened(String classifier)
        {
            return _fullResultSet.FindElements(By.ClassName(classifier));
        }

        public CreateProfileLocationPage ChooseProfileLocation(String location)
         {
             this._driver.SelectFromInputDropDown(inputLocations, location);
             return this;
         }

        private IWebElement FindNextButton()=> _nextButton=_actionMenu.FindElement(By.XPath("//*[@id='root']/div[2]/div/div[3]/div/div[1]/div/div[2]/div[7]/div[1]/form/div[3]/div[2]/button"));

        public CreateProfileRoleTagsPage ChooseProfileLocationAndCLickNext(String location)
        {
            ChooseProfileLocation(location);
            FindNextButton().Click();
            return new CreateProfileRoleTagsPage(this._driver);
        }

        public CreateProfileRoleTagsPage ClickOnNextButtonToNavigateOnRoleTagScreen()
        {
            if (_nextButton == null)
                FindNextButton();
            if (!CheckIfNextButtonIsEnabled())
                throw new Exception("Test flow exception: The next button is not enabled.");

            _nextButton.Click();
            return new CreateProfileRoleTagsPage(this._driver);
        }

        public bool CheckIfNextButtonIsEnabled()
        {
            FindNextButton();
            return _nextButton.IsEnabled();
        }
    }
}