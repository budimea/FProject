using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class CreateNewProjectPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".kit-form")]
        private IWebElement _fieldsContainer;
        [FindsBy(How = How.ClassName, Using = "slider")]
        private IWebElement _toggleVersion;

        [FindsBy(How = How.ClassName, Using = "kit-portal-container")]
        private IWebElement _pageContentContaier;
        
        public CreateNewProjectPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        private IWebElement FindExitButton() => _pageContentContaier
            .FindElement(By.CssSelector(".kit-svg.kit-image-button.right.kit-element.isClickable"));

        private IList<ValidatedInputFields> FindProjectInputField()=>_fieldsContainer.FindInputFields();

        public ListReleasesViewPage CreateNewProjectWithRequiredData(string value)
        {
            FindAndPopulateInputField(ReleaseMetaDataBatches.Title, value);
            Proceed();
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage ClickToProceedWithWarnings()
        {
            IWebElement _btnProceed = FindProceedButton(); 
            _btnProceed.Click();
            return new CreateNewProjectPage(this._driver);
        }

        public ListReleasesViewPage Proceed()
        {
            IWebElement _btnProceed = FindProceedButton();
            _btnProceed.Click();
            Thread.Sleep(5000);
            return new ListReleasesViewPage(this._driver);
        }

        private IWebElement FindProceedButton()=> _fieldsContainer.FindElement(By.TagName("button"));

        public CreateNewProjectPage FindAndPopulateInputField(int field, string value)
        {
            FindInputFieldAndPopulateIt(field, value);
            return new CreateNewProjectPage(this._driver);
        }

        private void FindInputFieldAndPopulateIt(int field, string value)
        {
            var inputField = default(IWebElement);
            var input = FindProjectInputField();
            if(input.Count-1<field)
                if (!CheckIfToggleIsOn())
                {
                    EnableToggleForAddingProjectVersion();
                    input = FindProjectInputField();
                }
            inputField=input.ElementAt(field).InputField;    
            //if (!inputField.Displayed)
            if (inputField.Equals("emoji"))
                inputField.SendKeys("\uD83C");
            else inputField.SendKeys(value);
        }

        private bool CheckIfToggleIsOn() =>
            _pageContentContaier.FindElements(By.CssSelector(".kit-toggle.isOn")).Count > 0;

        public ListReleasesViewPage CreateNewProjectWithByPopulatingAllFields(string requiredValue, string optionalValue)
        {
            FindAndPopulateInputField(ReleaseMetaDataBatches.Title,requiredValue)
                .FindAndPopulateInputField(ReleaseMetaDataBatches.Version, optionalValue);
            Proceed();
            Thread.Sleep(2000);
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage EnableToggleForAddingProjectVersion()
        {
            _toggleVersion.Click();
            return new CreateNewProjectPage(this._driver);
        }

        public bool CheckIfUserIsAbleToProceed()
        {
            var titleErrors = ReadAllValidationMessagesForSpecificField(ReleaseMetaDataBatches.Title,ValidationsMessage.Hard);
            var versionErrors = ReadAllValidationMessagesForSpecificField(ReleaseMetaDataBatches.Version,ValidationsMessage.Hard);
            return titleErrors.Count==0 && versionErrors.Count==0;
        }

        public int CountErrorMessagesReturnedForInputField(int field, string validationType)
        {
            return CountWarnings(field, validationType);
        }

        private int CountWarnings(int field, String validationType)
        {
            var elem = FindProjectInputField().ElementAt(field).InputField;
            return elem.ReturnWarnings(validationType).Count;
        }

        public List<string> ReadAllValidationMessagesForSpecificField(int field, string validationType) =>
            _fieldsContainer.ReadAllValidationMessagesForInputField(field,validationType);

        public ListReleasesViewPage CloseTheCreateProjectScreen()
        {
            var btnExit = FindExitButton();
            btnExit.Click();
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage DeleteNumberOfCharactersFromInputField(int field, int numberOfCharacter)
        {
            var inputField = FindProjectInputField().ElementAt(field).InputField;
            inputField.RemoveNumberOfCharactersFromInputField(numberOfCharacter);
            return new CreateNewProjectPage(this._driver);
        }
    }
}