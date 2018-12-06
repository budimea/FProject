using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
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

        private IWebElement _inputTitle = null;

        private IWebElement _inputVersion=null;

        private IWebElement _btnProceed = null;


        [FindsBy(How = How.ClassName, Using = "kit-portal-container")]
        private IWebElement _pageContentContaier;

        private IWebElement _btnExit;

        private List<String> SoftWarnings=null;

        private List<String> HardWarnings=null;

        private IList<ValidatedInputFields> _projectInputFields;

        public CreateNewProjectPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(_driver, this);
            findInputFields();
            _inputTitle = FindProjectTitle();
            _inputVersion = FindProjectVersion();
            _btnExit = FindExitButton();
            //I need to solve the problem of recreating the buttons;//...
        }

        private IWebElement FindExitButton() => _pageContentContaier
            .FindElement(By.CssSelector(".kit-svg.kit-image-button.right.kit-element.isClickable"));

        private IWebElement FindProjectVersion()=>_projectInputFields.ElementAt(1).InputField;

        private void findInputFields()
        {
            _projectInputFields = new List<ValidatedInputFields>();
            IList<IWebElement> Inputs = _fieldsContainer.FindElements(By.ClassName("kit-input"));
            foreach (IWebElement elem in Inputs)
            {
                IWebElement Input = elem.FindElement(By.ClassName("input"));
                IWebElement Validations = elem.FindElement(By.CssSelector(".kit-list"));
                _projectInputFields.Add(new ValidatedInputFields(Input, Validations));
            }
        }

        private IWebElement FindProjectTitle() => _projectInputFields.ElementAt(0).InputField;

        public ListReleasesViewPage CreateNewProjectWithRequiredData(string ProjectTitle)
        {
            findAndPopulateProjectTitle(ProjectTitle);
            Proceed();
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage ClickToProceedWithWarnings()
        {
            IWebElement _btnProceed = findProceedButton(); 
            _btnProceed.Click();
            return new CreateNewProjectPage(this._driver);
        }

        public ListReleasesViewPage Proceed()
        {
            IWebElement _btnProceed = findProceedButton();
            _btnProceed.Click();
            Thread.Sleep(5000);
            return new ListReleasesViewPage(this._driver);
        }

        private IWebElement findProceedButton()=> _fieldsContainer.FindElement(By.TagName("button"));

        public CreateNewProjectPage findAndPopulateProjectTitle(string projectTitle)
        {
            findTitleInputFieldAndPopulateIt(projectTitle);
            return new CreateNewProjectPage(this._driver);
        }

        private void findTitleInputFieldAndPopulateIt(string ProjectTitle)
        {
            if (_inputTitle == null)
            {
                FindProjectTitle();
            }
            if (ProjectTitle.Equals("emoji"))
                _inputTitle.SendKeys("\uD83C");
            else _inputTitle.SendKeys(ProjectTitle);
        }

        public ListReleasesViewPage CreateNewProjectWithTitleAndVersion(string ProjectTitle, string ProjectVersion)
        {
            findAndPopulateProjectTitle(ProjectTitle);
            findAndPopulateProjectVersion(ProjectVersion);
            Proceed();
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage findAndPopulateProjectVersion(string projectVersion)
        {
            CheckIfVersionIsToggledOn();
            _inputVersion.SendKeys(projectVersion);
            return new CreateNewProjectPage(this._driver);
        }

        private CreateNewProjectPage CheckIfVersionIsToggledOn()
        {
            if(!_inputVersion.Displayed)
                enableToggleForAddingProjectVersion();
            return new CreateNewProjectPage(this._driver);
        }

        public CreateNewProjectPage enableToggleForAddingProjectVersion()
        {
            _toggleVersion.Click();
            return new CreateNewProjectPage(this._driver);
        }

        public bool CheckIfUserIsAbleToProceed()
        {
            if (_btnProceed==null)
                _btnProceed = findProceedButton();
            return _btnProceed.IsEnabled();
        }

        public int countErrorMessagesReturnedForProjectTitle()
        {
            return countWarnings(_inputTitle, "Hard");
        }

        public int countSoftWarningsReturnedProjectTitle()
        {
            return countWarnings(_inputTitle, "Soft");
        }

        private int countWarnings(IWebElement elem, String validationType)
        {
            return readListOfErrorMessages(elem, validationType).Count();
        }

        private List<IWebElement> ReturnWarnings(IWebElement elem, String validationType)
        {
            Thread.Sleep(2500);
            var Messages = new List<IWebElement>();
            var tmp = new List<IWebElement>();
            if (validationType == "Hard")
            {
                Messages= elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-RASPBERRY_-2")).ToList();
            }
            else Messages=elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-MANGO_-2")).ToList();
            foreach (IWebElement element in Messages)
                tmp.Add(element.FindElement(By.XPath("..")));
            return tmp;
        }

        public List<String> readAllWarningMessagesForProjectTitle()
        {
            List<IWebElement> errorMessages = readListOfErrorMessages(_projectInputFields.ElementAt(0).ListOfErrorMessages, "Soft");
            return readAllTextMesaggesReturnedInWarning(errorMessages);
        }

        public List<String> readAllErrorMessagesForProjectTitle()
        {
            List<IWebElement> errorMessages = readListOfErrorMessages(_projectInputFields.ElementAt(0).ListOfErrorMessages, "Hard");
            return readAllTextMesaggesReturnedInWarning(errorMessages);
        }

        public List<string> readAllErrorMessagesForProjectVersion()
        {
            List<IWebElement> errorMessages = readListOfErrorMessages(_projectInputFields.ElementAt(1).ListOfErrorMessages, "Hard");
            return readAllTextMesaggesReturnedInWarning(errorMessages);
        }

        public List<string> readAllWarningMessagesForProjectVersion()
        {
            List<IWebElement> errorMessages = readListOfErrorMessages(_projectInputFields.ElementAt(1).ListOfErrorMessages, "Soft");
            return readAllTextMesaggesReturnedInWarning(errorMessages);
        }

        private List<String> readAllTextMesaggesReturnedInWarning(IList<IWebElement> Messages)
        {
            var warningTextMessages = new List<String>();
            foreach (IWebElement elem in Messages)
            {
                IWebElement message = elem.FindElement(By.ClassName("title"));
                warningTextMessages.Add(message.Text);
            }
            return warningTextMessages;
        }

        private List<IWebElement> readListOfErrorMessages(IWebElement elem, String ValidationType) => ReturnWarnings(elem, ValidationType);

        public ListReleasesViewPage CloseTheCreateProjectScreen()
        {
            _btnExit.Click();
            return new ListReleasesViewPage(this._driver);
        }

        public CreateNewProjectPage DeleteNumberOfCharactersFromTitle(int NumberOfCharacters)
        {
            while (NumberOfCharacters > 0)
            {
                _inputTitle.SendKeys(Keys.Backspace);
                NumberOfCharacters--;
            }
            return new CreateNewProjectPage(this._driver);
        }
    }
}