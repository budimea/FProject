using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class FindOrCreateProfileToCredit
    {
        private IWebDriver _driver;
        
        [FindsBy(How = How.CssSelector, 
            Using =".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthFORM_770.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;

        public FindOrCreateProfileToCredit(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void SearchAndCreditExistingProfile(string selection)
        {
            PopulateInputFieldAndChooseExistingProfile(selection);
            ClickDone();
            //return new ProjectReleaseInformationPage(this._driver);
        }

        private void ClickDone()
        {
            var btnDone = _elementContainer.FindElement(By.CssSelector("button[type='submit']"));
            btnDone.Click();
        }

        private FindOrCreateProfileToCredit PopulateInputFieldAndChooseExistingProfile(string selection)
        {
            PopulateInputField(selection);
            ChooseFromReturnedResultSet(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private FindOrCreateProfileToCredit ChooseFromReturnedResultSet(string selection)
        {
            var rootElement = _elementContainer.FindElement(By.ClassName("kit-suggestions"));
            rootElement.SelectFromResultsByMatchingStart(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private FindOrCreateProfileToCredit PopulateInputField(string selection)
        {
            var inputFiled = _elementContainer.FindElements(By.CssSelector("input[type='text']"))[0];
            inputFiled.SendKeys(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        public ProjectReleaseInformationPage SearchAndCreditExistingProfileWithCollaboratorsRole(string profile, string collaboratorsRole)
        {
            PopulateInputField(profile).ChooseFromReturnedResultSet(profile).PopulateRole(collaboratorsRole).ChooseCollaboratorRole(collaboratorsRole);
            ClickDone();
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage ChooseCollaboratorRole(string collaboratorsRole)
        {
            ChooseFromReturnedStaticResultSet(collaboratorsRole);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private FindOrCreateProfileToCredit PopulateRole(string collaboratorsRole)
        {
            var inputCollaborators = _elementContainer.FindElements(By.CssSelector("input[type='text']"))[1];
            inputCollaborators.SendKeys(collaboratorsRole);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private FindOrCreateProfileToCredit ChooseFromReturnedStaticResultSet(string selection)
        {
            var rootElement = _elementContainer.FindElement(By.ClassName("kit-suggestions"));
            Thread.Sleep(2000);
            rootElement.SelectFromStaticDropDown(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        public FindOrCreateProfileToCredit CreateProfileInline(string profileName, string location)
        {
            var createProfileInline=PopulateInputField(profileName).ClickToCreateNewProfile();
            createProfileInline.createProfileWithFollowingData(profileName, location);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private ProfileInlineCreationPage ClickToCreateNewProfile()
        {
            var rootElement = _elementContainer.FindElement(By.ClassName("kit-suggestions"));
            var createProfileElement = rootElement.FindElements(By.ClassName("row")).First();
            createProfileElement.Click();
            return new ProfileInlineCreationPage(this._driver);
        }

        public ProjectReleaseInformationPage CreateAndSelectProfileInline(string expectedProfileName, string location)
        {
            CreateProfileInline(expectedProfileName,location);
            Thread.Sleep(5000);
            ClickDone();
            return new ProjectReleaseInformationPage(this._driver);
        }
    }
}