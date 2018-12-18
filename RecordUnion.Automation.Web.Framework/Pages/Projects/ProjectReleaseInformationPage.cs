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

        public ProjectReleaseInformationPage ChangeInputBatchField(int batch, int field, string value)
        {
            ChangeReleaseInputWithNewValue(batch, field, value);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private void ChangeReleaseInputWithNewValue(int batch, int field, string newValue)
        {
            var fieldBatch=FindElementInBatchInputs(batch, field);
            if (!fieldBatch.InputField.Displayed)
                EnableToggleForFieldOrComponent(batch);
            fieldBatch.InputField.Clear();
            fieldBatch.InputField.SendKeys(newValue);
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

        public bool IsFirstBatchSubmitButtonEnabled()=> getSubmitButton(ReleaseMetaDataBatches.ReleaseTitleVersionBatch).IsEnabled();

        public List<string> readSoftValidationsForBatch(int batch) =>
            readValidationsForBatch(batch, ValidationsMessage.Soft);

        public List<String> readHardValidationForBatch(int batch) =>
            readValidationsForBatch(batch, ValidationsMessage.Hard);

        private List<string> readValidationsForBatch(int batch, String validationType)
        {
            ReleaseInputBatchComponent InputBatch = null;
            if (_webInputBatches.Count==0)
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

        public ProjectReleaseInformationPage EnableToggleForFieldOrComponent(int releaseBatch)
        {
            var batch= readSpecificBatch(releaseBatch);
            if(!CheckIfToggleIsEnabled(batch))
                ChangeToggle(batch);
            else throw new Exception("Test Flow Exception");
            return new ProjectReleaseInformationPage(this._driver);
        }

        public IWebElement GetInputValueInBatch(int batch, int field)=> FindElementInBatchInputs(batch, field).InputField;

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
            var listOfInputFieldExtensions = _inputElement.FindElements(By.ClassName("kit-mint"));
            var _dropDownExpander = default(IWebElement);
            var _clearInputField = default(IWebElement);
            var _validationMessages = _inputElement.FindElement(By.CssSelector(".kit-list"));
            if (listOfInputFieldExtensions.Count == 3)
            {
                _dropDownExpander = listOfInputFieldExtensions.ElementAt(1);
                _clearInputField = listOfInputFieldExtensions.ElementAt(0);
            }
            else _dropDownExpander = listOfInputFieldExtensions.ElementAt(0);
            return new EditReleaseMetadataDropDownComponent(_rootDropDown, _inputElement,_dropDownExpander, _clearInputField, _validationMessages);
        }

        private void ChangeToggle(IWebElement element)
        {
            IWebElement toggle = element.FindElement(By.ClassName("slider"));
            toggle.Click();
        }

        public ProjectReleaseInformationPage SaveBatchWithoutValidations(int batch)
        {
            SaveSpecificInputBatchChange(batch);
            Thread.Sleep(1500);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddCreditsToAcceptingMultipleProfiles(int role, string profile)
        {
            var CreditComponent=FindAllCreditsWithMultipleProfiles().ElementAt(role);
            AddNewCreditToProfile(CreditComponent, profile);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage AddNewCreditToProfile(IWebElement credit, string selection)
        {
            var creditPad = credit.FindElement(By.CssSelector(".kit-bar.isCreator"));
            creditPad.Click();
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

        public ProjectReleaseInformationPage AddSingleCollaboratorCreditToExistingProfile(int role, string existingProfile, string collaboratorsRole)
        {
            var creditComponent = FindAllCreditsWithMultipleProfiles().ElementAt(role);
            var creatorPad = creditComponent.FindElement(By.CssSelector(".kit-bar.isCreator"));
            creatorPad.Click();
            Thread.Sleep(2000);
            AddNewCollaboratorRoleToProfile(existingProfile, collaboratorsRole);
            Thread.Sleep(3000);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage AddNewCollaboratorRoleToProfile(string existingProfile, string collaboratorsRole)
        {
            _findOrCreateProfileToCredit=new FindOrCreateProfileToCredit(this._driver);
            _findOrCreateProfileToCredit.SearchAndCreditExistingProfileWithCollaboratorsRole(existingProfile,collaboratorsRole);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddCreditToRoleAcceptingOneProfile(int role, string existingProfile)
        {
            var creditComponent = FindTheRootElementForRoleWithSingleCredit(role);
            var creatorPad = creditComponent.FindElement(By.CssSelector(".kit-bar.isCreator"));
            Thread.Sleep(1000);
            creatorPad.Click();
            Thread.Sleep(10000);
            CreditAProfile(existingProfile);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private IWebElement FindTheRootElementForRoleWithSingleCredit(int role)=> _elementContainer.FindElement(role == 0 ? By.CssSelector(".form-projects-copyright-owner") : By.CssSelector(".form-projects-label"));

        public ProjectReleaseInformationPage AddLabelFromExistingProfile(string existingProfile)
        {
            var creditLabel = _elementContainer.FindElement(By.CssSelector(".form-projects-label"));
            var creatorPad = creditLabel.FindElement(By.CssSelector(".kit-bar.isCreator"));
            creatorPad.Click();
            Thread.Sleep(1000);
            CreditAProfile(existingProfile);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddCatalogueNumber(int field, string catalogueNum)
        {
            var catalogueNumber = FindAllInputFieldBatches().ElementAt(field);
            if(!CheckIfToggleIsEnabled(catalogueNumber))
                ChangeToggle(catalogueNumber);
            PopulateCatalogueNumber(catalogueNumber, catalogueNum);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage PopulateCatalogueNumber(IWebElement catalogueNumber, string catalogueNum)
        {
            var catalogueInput = catalogueNumber.FindElement(By.CssSelector("input[type='text']"));
            catalogueInput.SendKeys(catalogueNum);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddEanOrUpcCode(int field, string eanUpcCode)
        {
            var eanContainer = FindAllInputFieldBatches().ElementAt(field);
            var eanInputField = eanContainer.FindElement(By.CssSelector("input[type='text']"));
            eanInputField.SendKeys(eanUpcCode);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage SetReleaseLanguage(int field, string selection)
        {
            var languageComponent = FindElementInBatchDropDown(field, 0);
            ExpandNonSearchableDropDown(languageComponent)
                .ChooseFromNonSearchableDropDown(languageComponent, 0, selection);
            return new ProjectReleaseInformationPage(this._driver);
        }
        
        public ListReleasesViewPage NavigateToProjectsScreen()
        {
            var btnReleases=_driver.ReturnHeaderNavigationItems().ElementAt(2);
            btnReleases.Click();
            return new ListReleasesViewPage(_driver);;
        }

        public string GetInputFieldValue(int batch, int field)=>GetInputValueInBatch(batch, field).GetAttribute("value");

        public ProjectReleaseInformationPage RemoveOptionalFieldFromBatch(int batch, int field)
        {
            var batchWithToggle = FindAllInputFieldBatches().ElementAt(batch);
            if(!CheckIfToggleIsEnabled(batchWithToggle))
                throw new Exception("Field is not enabled");
            var toggle= FindSpecificComponent(batch).Toggle;
            ChangeToggle(toggle);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private bool CheckIfToggleIsEnabled(IWebElement parentElement) =>
            parentElement.FindElements(By.CssSelector(".kit-toggle.isOn")).Count > 0;
        
        public List<string> ReadAllMessagesForGivenField(int batch, int field, string validationType)
        {
            var inputBatch = FindAllInputFieldBatches().ElementAt(batch);
            return inputBatch.ReadAllValidationMessagesForInputField(field, validationType);
        }

        public List<string> ReadAllMessagesForGivenDropDownField(int batch, string validationType)
        {
            var dropDownBatch = readSpecificBatch(batch);
            var dropDownElements = FindAllDropDownElements(dropDownBatch);
            if (dropDownElements.Count == 2)
                return dropDownElements.ElementAt(1).ReadAllValidationMessagesForDropDownField(validationType);
            else return dropDownElements.ElementAt(0).ReadAllValidationMessagesForDropDownField(validationType);
        }

        private List<EditReleaseMetadataDropDownComponent> FindAllDropDownElements(IWebElement rootBatchElement)
        {
            var listOfDropDownFields = new List<EditReleaseMetadataDropDownComponent>();
            var _rootDropDown = rootBatchElement.FindElements(By.CssSelector(".kit-dropdown"));
            foreach (var elem in _rootDropDown)
            {
                var _inputElement = elem.FindElement(By.CssSelector(".kit-input"));
                var listOfInputFieldExtensions = _inputElement.FindElements(By.ClassName("kit-mint"));
                var _validationMessages = _inputElement.FindElement(By.CssSelector(".kit-list"));
                var _dropDownExpander = default(IWebElement);
                var _clearInputField = default(IWebElement);
                if (listOfInputFieldExtensions.Count == 3)
                {
                    _dropDownExpander = listOfInputFieldExtensions.ElementAt(1);
                    _clearInputField = listOfInputFieldExtensions.ElementAt(0);
                }
                else _dropDownExpander = listOfInputFieldExtensions.ElementAt(0); 
                listOfDropDownFields.Add(new EditReleaseMetadataDropDownComponent(elem, _inputElement,_dropDownExpander, _clearInputField, _validationMessages));
            }
            return listOfDropDownFields;
        }

        public string GetDropDownFieldValue(int batch, int field)
        {
            var dropDownElement = GetDropDownInputField(batch, field);
            var elementText = dropDownElement.FindElement(By.TagName("input"));
            return elementText.GetAttribute("value");
        }

        private IWebElement GetDropDownInputField(int batch, int field) =>
            FindElementInBatchDropDown(batch, field).InputDropDown;

        public ProjectReleaseInformationPage ChooseAValueFromDropDownList(int batch, int field, string selection)
        {
            var dropDownField = FindElementInBatchDropDown(batch, field);
            if (!dropDownField.InputDropDown.Displayed)
                EnableToggleForFieldOrComponent(batch);
            ExpandNonSearchableDropDown(dropDownField)
                .ChooseFromNonSearchableDropDown(dropDownField, field,selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public bool CheckIfLookUpFieldReturnsValues(int batch, int field)
        {
            var dropDownField = FindElementInBatchDropDown(batch, field);
            if (dropDownField.InputDropDown.Displayed)
                return dropDownField.InputDropDown.IsEnabled();
            return false;
        }

        public ProjectReleaseInformationPage ClearValueFromField(int batch, int field)
        {
            var dropDownInput = FindElementInBatchDropDown(batch, field);
            dropDownInput.ClearButton.Click();
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage ChooseAValueFromDropDownList(int batch, string selection)
        {
            return ChooseAValueFromDropDownList(batch, 0, selection);
        }

        public string GetDropDownFieldValue(int batch)
        {
            return GetDropDownFieldValue(batch, 0);
        }
    }
}
