using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class ProjectReleaseInformationPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".kit-element.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;

        private ValidatedInputFields _releaseTitle;
        private ValidatedInputFields _releaseVersion;
        private FindOrCreateProfileToCredit _findOrCreateProfileToCredit;
        

        private List<ReleaseInputBatchComponent> _webInputBatches;

        public ProjectReleaseInformationPage(IWebDriver driver)
        {
            _driver = driver;
            _webInputBatches = new List<ReleaseInputBatchComponent>();
            PageFactory.InitElements(_driver, this);
        }

        private List<IWebElement> FindAllInputFieldBatches() => _elementContainer
            .FindElements(By.ClassName("kit-form")).ToList();

        private List<IWebElement> FindAllCreditsWithMultipleProfiles() => _elementContainer
            .FindElements(By.CssSelector(".form-projects-primary-artists")).ToList();

        public ProjectReleaseInformationPage ChangeReleaseTitle(string NewProjectTitle)
        {
            ChangeReleaseTitleWithNewValue(NewProjectTitle);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private void ChangeReleaseTitleWithNewValue(string newProjectTitle)
        {
            if (_releaseTitle == null)
            {
                _releaseTitle=FindElementInBatchInputs(ReleaseMetaDataBatches
                    .ReleaseTitleVersionBatch, ReleaseMetaDataBatches.Title);
            }
            _releaseTitle.InputField.Clear();
            _releaseTitle.InputField.SendKeys(newProjectTitle);
        }

        private ValidatedInputFields FindElementInBatchInputs(int batch, int index)
        {
            ReleaseInputBatchComponent batchInput= FindSpecificComponent(batch);
            return batchInput.InputFields.ElementAt(index);
        }

        private ReleaseInputBatchComponent FindBatch(int index)
        {
            if (_webInputBatches.Count==0)
                FindAllBatchesWithInput();
            return _webInputBatches.ElementAt(index);
        }

        private ReleaseInputBatchComponent FindSpecificComponent(int batch)
        {
            var _rootElement = readSpecificBatch(batch);
            var _batchInputs = _rootElement.FindInputFieldsGeneric();
            IWebElement batchToggle = null;
            IWebElement _submit = null;
            IWebElement _cancel = null;
            try
            {
                _submit = _rootElement.FindElement(
                    By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypePRIMARY"));
                _cancel = _rootElement.FindElement(
                    By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypeTERTIARY"));
                batchToggle =
                    _rootElement.FindElement(By.CssSelector(".kit-toggle"));
            }
            catch(NoSuchElementException)
            {
                System.Console.WriteLine("This batch does not have a toggle");
            }
            return new ReleaseInputBatchComponent(_batchInputs, batchToggle, _submit, _cancel);
        }

        private IWebElement readSpecificBatch(int batch)=>FindAllInputFieldBatches().ElementAt(batch);
        /*{
            return FindAllInputFieldBatches().ElementAt(batch);
        }*/
            
        private void SaveSpecificInputBatchChange(int batch)
        {
            IWebElement submitElement = getSubmitButton(batch);
            Thread.Sleep(2000);
            submitElement.Click();
        }

        private IWebElement getSubmitButton(int batch)
        {
            ReleaseInputBatchComponent changedBatch = null;
            if (_webInputBatches.Count==0)
                //changedBatch=FindBatch(batch);
                changedBatch=FindSpecificComponent(batch);
            if(changedBatch==null)
                throw new Exception("The batch is not changed");
            return changedBatch.Submit;
        }

        private void FindAllBatchesWithInput()
        {
            IWebElement batchToggle = null;
            IWebElement submit=null;
            IWebElement cancel=null;
            _webInputBatches =new List<ReleaseInputBatchComponent>();
            Thread.Sleep(1000);
            IList<IWebElement> WebInputBatches = FindAllInputFieldBatches();
            foreach(IWebElement elem in WebInputBatches)
            {
                var BatchInputs = elem.FindInputFieldsGeneric();
                //List<IWebElement> ActionButtonsContainer = elem.FindElements(By.TagName("button")).ToList();
                try
                {
                    submit = elem.FindElement(
                        By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypePRIMARY"));
                    cancel = elem.FindElement(
                        By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypeTERTIARY"));
                    batchToggle = elem.FindElement(By.CssSelector(".kit-toggle"));
                }
                catch(NoSuchElementException)
                {
                    System.Console.WriteLine("This batch does not have a toggle");
                }
                _webInputBatches.Add(new ReleaseInputBatchComponent(BatchInputs, batchToggle, submit, cancel));
            }
        }

        public ListReleasesViewPage NavigateToProjectsScreen()
        {
            var btnReleases=_driver.ReturnHeaderNavigationItems().ElementAt(2);
            btnReleases.Click();
            return new ListReleasesViewPage(_driver);;
        }

        public bool IsFirstBatchSubmitButtonEnabled()=> getSubmitButton(ReleaseMetaDataBatches.ReleaseTitleVersionBatch).IsEnabled();

        public List<string> readSoftValidationsForBatch(int batch) =>
            readValidationsForBatch(batch, CommonUsedVariables.Soft);

        public List<String> readHardValidationForBatch(int batch) =>
            readValidationsForBatch(batch, CommonUsedVariables.Hard);

        private List<string> readValidationsForBatch(int batch, String validationType)
        {
            ReleaseInputBatchComponent InputBatch = null;
            if (_webInputBatches.Count==0)
                //InputBatch = FindBatch(batch);
                InputBatch = FindSpecificComponent(batch);
            else InputBatch = _webInputBatches.ElementAt(batch);
            var _listOfErrors = InputBatch.InputFields.ElementAt(0).ListOfErrorMessages;
            var errorMessages =_listOfErrors.ReturnWarnings(validationType);
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

        public ProjectReleaseInformationPage CancelChangesOfBatch(int batch)
        {
            //IWebElement _cancel;
            var _cancel = getCancelButton(batch);
            _cancel.Click();
            return new ProjectReleaseInformationPage(this._driver);
        }
        
        private IWebElement getCancelButton(int batch)
        {
            ReleaseInputBatchComponent changedBatch = null;
            if (_webInputBatches.Count == 0)
                changedBatch = FindSpecificComponent(batch);
            if(changedBatch==null)
                throw new Exception("The batch is not changed");
            return changedBatch.Cancel;
        }

        public ProjectReleaseInformationPage ChangeReleaseVersion(string projectVersion)
        {
            if (_releaseVersion==null)
                _releaseVersion = FindElementInBatchInputs(ReleaseMetaDataBatches.ReleaseTitleVersionBatch,
                    ReleaseMetaDataBatches.Version);
            if (!_releaseVersion.InputField.Displayed)
                CheckIfVersionIsEnabled();
            _releaseVersion.InputField.Clear();
            _releaseVersion.InputField.SendKeys(projectVersion);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage CheckIfVersionIsEnabled()
        {
            enableToggleForAddingProjectVersion();
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage enableToggleForAddingProjectVersion()
        {
            var batch= readSpecificBatch(ReleaseMetaDataBatches.ReleaseTitleVersionBatch);
            enableToggle(batch);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public IWebElement GetInputValueInBatch(int Batch, int field)
        {
            var InputBatch = FindSpecificComponent(Batch);
            return InputBatch.InputFields.ElementAt(field).InputField;
        }

        public ProjectReleaseInformationPage SetMainGenre(string selection)
        {
            //if (_releaseMainGenre == null)
                var dropDownField = FindElementInBatchDropDown(ReleaseMetaDataBatches.MainGenreBatch,
                    ReleaseMetaDataBatches.MainGenre);
            
           // Thread.Sleep(15000);
            ExpandNonSearchableDropDown(dropDownField)
                .ChooseFromNonSearchableDropDown(dropDownField, ReleaseMetaDataBatches.MainGenre,selection);
 
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage ChooseFromNonSearchableDropDown(
            EditReleaseMetadataDropDownComponent dropDownField, int field, string selection)
        {
            if (dropDownField == null)
                dropDownField = FindElementInBatchDropDown(ReleaseMetaDataBatches.MainGenreBatch,
                    field);
            Thread.Sleep(1000);
            dropDownField.RootDropDownElement.SelectFromStaticDropDown(selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage ExpandNonSearchableDropDown(EditReleaseMetadataDropDownComponent dropDownField)
        {
            dropDownField.DropDownExpander.Click();
            return new ProjectReleaseInformationPage(this._driver);
        }
        
        private EditReleaseMetadataDropDownComponent FindElementInBatchDropDown(int batch, int field)
        {
            var _rootDropDownContainer = readSpecificBatch(batch);
            return FindDropDownGeneric(_rootDropDownContainer, field);
        }
        
        private EditReleaseMetadataDropDownComponent FindDropDownGeneric(IWebElement rootDropDownContainer, int field)
        {
            var _rootDropDown = rootDropDownContainer.FindElements(By.CssSelector(".kit-dropdown")).ElementAt(field);
            var _inputElement = _rootDropDown.FindElement(By.CssSelector(".kit-input"));
            var _dropDownExpander = _inputElement.FindElements(By.ClassName("kit-mint")).ElementAt(0);
            return new EditReleaseMetadataDropDownComponent(_rootDropDown, _inputElement,_dropDownExpander);
        }

        public ProjectReleaseInformationPage SetMainSubGenre(string selection)
        {
            var dropdownField = FindElementInBatchDropDown(ReleaseMetaDataBatches.MainGenreBatch,
                    ReleaseMetaDataBatches.MainSubGenre);
            ExpandNonSearchableDropDown(dropdownField)
                .ChooseFromNonSearchableDropDown(dropdownField,ReleaseMetaDataBatches.MainSubGenre,selection);
            return new ProjectReleaseInformationPage(this._driver);
        }


        public ProjectReleaseInformationPage SetAlternateMainGenre(string selection)
        {
            var batch = readSpecificBatch(ReleaseMetaDataBatches.AlternateGenreBatch);
            enableToggle(batch);
            var alternatemaingenre=FindElementInBatchDropDown(ReleaseMetaDataBatches.AlternateGenreBatch,
                ReleaseMetaDataBatches.MainGenre);
            ExpandNonSearchableDropDown(alternatemaingenre)
                .ChooseFromNonSearchableDropDown(alternatemaingenre,ReleaseMetaDataBatches.MainGenre,selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private void enableToggle(IWebElement element)
        {
            IWebElement toggle = element.FindElement(By.ClassName("slider"));
            toggle.Click();
        }

        public ProjectReleaseInformationPage SetAlternateSubGenre(string selection)
        {
            var dropdownField = FindElementInBatchDropDown(ReleaseMetaDataBatches.AlternateGenreBatch,
                ReleaseMetaDataBatches.MainSubGenre);
            ExpandNonSearchableDropDown(dropdownField)
                .ChooseFromNonSearchableDropDown(dropdownField,ReleaseMetaDataBatches.MainSubGenre,selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage SaveBatchWithoutValidations(int batch)
        {
            SaveSpecificInputBatchChange(batch);
            Thread.Sleep(1500);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddPrimaryArtistFromExistingProfile(string primaryArtistOne)
        {
            var primaryArtistsComponent=FindAllCreditsWithMultipleProfiles().ElementAt(ReleaseMetaDataBatches.PrimaryArtistRole);
            AddNewCreditToProfile(primaryArtistsComponent, primaryArtistOne);

            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage AddNewCreditToProfile(IWebElement credit, string selection)
        {
            var creatorPrimaryArtist = credit.FindElement(By.CssSelector(".kit-bar.isCreator"));
            creatorPrimaryArtist.Click();
            Thread.Sleep(10000);
            CreditAProfile(selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage CreditAProfile(string selection)
        {
            _findOrCreateProfileToCredit=new FindOrCreateProfileToCredit(this._driver);
            _findOrCreateProfileToCredit.SearchAndCreditExistingProfile(selection);
            return new ProjectReleaseInformationPage(this._driver);
        }
    }
}
