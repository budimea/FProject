using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class CreateProfileRoleTagsPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.TagName, Using = "input")]
        private IWebElement inputRoleTag;

        [FindsBy(How = How.XPath,
            Using = "//*[@id='root']/div[2]/div/div[3]/div/div[1]/div/div[2]/div[7]/div[1]/form/div[3]/div[2]/button")]
        private IWebElement _btnCreateProfile;

        [FindsBy(How = How.ClassName, Using = "rows")]
        private IWebElement _matchingRoleTags;

        public CreateProfileRoleTagsPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public ListProfilesViewPage ChooseProfileRoleTagsClickCreate(String RoleTag)
        {
            InserAndChooseFromValidRoleTag(RoleTag);
            ClickToCreateNewProfile();
            return new ListProfilesViewPage(this._driver);
        }

        private void ClickToCreateNewProfile() => _btnCreateProfile.Click();

        public CreateProfileRoleTagsPage InserAndChooseFromValidRoleTag(string roleTag)
        {
            populateInputRoleTag(roleTag);
            Thread.Sleep(5000);
            chooseFromTheResultSet(roleTag);
            return new CreateProfileRoleTagsPage(_driver);
        }

        private CreateProfileRoleTagsPage chooseFromTheResultSet(string roleTag)
        {
            var listOfReturnedRoleTags = _matchingRoleTags.FindElements(By.ClassName("row"));
            var matchingRoleTag=listOfReturnedRoleTags.FirstOrDefault(e=>e.Text==roleTag);

            if (matchingRoleTag == null)
            {
                throw new Exception("The Searched Role tag doesn't exist");
            }
            matchingRoleTag.Click();

            return this;
        }

        private CreateProfileRoleTagsPage populateInputRoleTag(string roleTag)
        {
            inputRoleTag.SendKeys(roleTag);
            return this;
        }

        public bool CheckIfCreateButtonIsEnabled()=>_btnCreateProfile.IsEnabled();

        public CreateProfileRoleTagsPage ChooseMemberRoleTag(string roleTag)
        {
            populateInputRoleTag(roleTag);
            Thread.Sleep(5000);
            chooseFromTheResultSet(roleTag);
            return new CreateProfileRoleTagsPage(this._driver);
        }

        public ListProfilesViewPage ClickToCreateProfile()
        {
            _btnCreateProfile.Click();
            return new ListProfilesViewPage(this._driver);
        }

        public IList<string> GetAllErrorsForExceedingNumberOfRoleTags()
        {
            //TODO handle this properly -> I need to create a handler which will count properly the number of errors  bs expected results
            IList<String> listOfErrors = new List<String>();
            if (CheckIfInputIsBorderredWithRead())
                listOfErrors.Add("Component is validated and errornes.");
            if(CheckIfErrorMessageIsDisplayed())
                listOfErrors.Add("Error message is displayed.");
            return listOfErrors;
        }

        private bool CheckIfErrorMessageIsDisplayed()
        {
            throw new NotImplementedException();
        }

        private bool CheckIfInputIsBorderredWithRead()
        {

            //TODO this is not implemented
            return false; 
        }

        /*private CreateProfileRoleTagsPage InsertValidRoleTag(string roleTag)
        {
            inputRoleTag.SendKeys(roleTag);
            chooseFromTheResultSet(roleTag);
        }
        */

    }
}