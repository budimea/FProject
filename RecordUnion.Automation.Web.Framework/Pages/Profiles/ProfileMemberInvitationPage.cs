using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class ProfileMemberInvitationPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.ClassName, Using = "kit-suggestions")]
        private IWebElement _listOfSuggestedMembers;

        [FindsBy(How = How.ClassName, Using = "input")]
        private IWebElement _inputAddPerson;

        [FindsBy(How = How.CssSelector, Using = ".kit-autocomplete")]
        private IWebElement _searchAddPerson;

        public ProfileMemberInvitationPage(IWebDriver driver)
        {
            _driver = driver;
          //  PageFactory.InitElements(_driver, this);
        }

        public ProfileMemberInvitationPage AddExistingRUMember(string existingMember)
        {
            SearchAndSelectExistingRUMember(existingMember);
            return new ProfileMemberInvitationPage(this._driver);
        }

        private void ClickToSendInvitation()
        {
            IWebElement elem = _driver.FindElement(By.CssSelector("button[type='submit']"));
            elem.Click();
        }

        public ProfileMemberInvitationPage SearchAndSelectExistingRUMember(string existingMember)
        {
            InsertSearchCriteriaAndSearch(existingMember);
            ChooseFromTheResultSetExactMatch(existingMember);
            return this;
        }

        private ProfileMemberInvitationPage ChooseFromTheResultSetExactMatch(string existingMember)
        {
            IList<IWebElement> _suggestedResults = _listOfSuggestedMembers.FindElements(By.ClassName("row"));
            _suggestedResults[1].Click();
            return new ProfileMemberInvitationPage(this._driver);
        }

        private ProfileMemberInvitationPage InsertSearchCriteriaAndSearch(string existingMember)
        {
            Thread.Sleep(1000);
            _inputAddPerson.SendKeys(existingMember);
            ClickOnSearch();
            Thread.Sleep(1000);
            return this;
        }

        private void ClickOnSearch()
        {
            IWebElement elem = _searchAddPerson.FindElement(By.ClassName("kit-mint"));
            elem.Click();
        }

        public ProfileMembersPage SendInvitation()
        {
            ClickToSendInvitation();
            return new ProfileMembersPage(this._driver);
        }
    }
}
