using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class ProjectReleaseInformationPage
    {
        private IWebDriver _driver;
        private SubMenu _subMenu;

        [FindsBy(How = How.CssSelector, Using = ".kit-element.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;
        private FindOrCreateProfileToCredit _findOrCreateProfileToCredit;
        

        private List<ReleaseInputBatchComponent> _webInputBatches;

        public ProjectReleaseInformationPage(IWebDriver driver)
        {
            _driver = driver;
            _webInputBatches = new List<ReleaseInputBatchComponent>();
            PageFactory.InitElements(_driver, this);
            _subMenu = new SubMenu(_driver);
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

        //ToDo find loader in order to remove the Thread sleep
        private void ChangeReleaseInputWithNewValue(int batch, int field, string newValue)
        {
            var fieldBatch=FindElementInBatchInputs(batch, field);
            Thread.Sleep(2000);
            if (!fieldBatch.InputField.Displayed)
                EnableToggleForFieldOrComponent(batch);
            var existingValue = string.Empty;
            existingValue = GetInputFieldValue(batch, field);
            //for(int i=0;i<existingValue.Length;i++)
            //fieldBatch.InputField.SendKeys(Keys.Backspace);
            fieldBatch.InputField.RemoveAllCharactersFromInputField(existingValue);
            Thread.Sleep(1000);
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
            var _submit = _rootElement.FindElement(
                By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypePRIMARY"));
            var _cancel = _rootElement.FindElement(
                By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypeTERTIARY"));
            var batchToggle = TryGetToggle(_rootElement);
            return new ReleaseInputBatchComponent(_batchInputs, batchToggle, _submit, _cancel);
        }

        private IWebElement TryGetToggle(IWebElement root)
        {
            try
            {
                var element = root.FindElement(By.CssSelector(".kit-toggle"));
                return element;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        private IWebElement readSpecificBatch(int batch)=>FindAllInputFieldBatches().ElementAt(batch);

        private void SaveSpecificInputBatchChange(int batch)
        {
            IWebElement submitElement = getSubmitButton(batch);
            Thread.Sleep(5000);
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
            Thread.Sleep(1000);
        }

        public ProjectReleaseInformationPage SaveBatchWithoutValidations(int batch)
        {
            SaveSpecificInputBatchChange(batch);
            Thread.Sleep(1500);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddCreditsToRoleAcceptingMultipleProfiles(int role, List<string> profile)
        {
            var creditComponent=FindAllCreditsWithMultipleProfiles().ElementAt(role);
            AddNewCreditToProfile(creditComponent, profile);
            Thread.Sleep(9000);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage AddNewCreditToProfile(IWebElement credit, List<string> selection)
        {
            var creditPad = credit.CreatorPad();
            foreach (var profile in selection)
            {
                creditPad.Click();
                Thread.Sleep(10000);
                CreditAProfile(profile);
            }
            //CreditAProfile(selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private ProjectReleaseInformationPage CreditAProfile(string selection)
        {
            _findOrCreateProfileToCredit=new FindOrCreateProfileToCredit(this._driver);
            _findOrCreateProfileToCredit.SearchAndCreditExistingProfile(selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddSingleCollaboratorCreditToExistingProfile(IWebElement batch, string existingProfile, string collaboratorsRole)
        {
            var creatorPad =  batch.CreatorPad();
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
            var creatorPad = creditComponent.CreatorPad();
            Thread.Sleep(1000);
            creatorPad.Click();
            Thread.Sleep(10000);
            CreditAProfile(existingProfile);
            Thread.Sleep(1500);
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
        //ToDo extract 2 in enum
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
            if (!checkIfDropDownFieldIsVisible(dropDownField))
                EnableToggleForFieldOrComponent(batch);
            ExpandNonSearchableDropDown(dropDownField)
                .ChooseFromNonSearchableDropDown(dropDownField, field,selection);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public bool CheckIfLookUpFieldReturnsValues(int batch, int field)
        {
            var dropDownField = FindElementInBatchDropDown(batch, field);
            return checkIfDropDownFieldIsVisible(dropDownField);
        }

        private bool checkIfDropDownFieldIsVisible(EditReleaseMetadataDropDownComponent dropDownField)
        {
            var elementExist =
                dropDownField.InputDropDown.FindElements(
                    By.CssSelector(".kit-validatee.kit-validatee-base-BLACKCURRANT_14.isCollapsed"));
            return elementExist.Count != 1;
        }

        public ProjectReleaseInformationPage ClearValueFromField(int batch, int field)
        {
            var dropDownInput = FindElementInBatchDropDown(batch, field);
            dropDownInput.ClearButton.Click();
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage ChooseAValueFromDropDownList(int batch, string selection) => ChooseAValueFromDropDownList(batch, 0, selection);

        public string GetDropDownFieldValue(int batch)=> GetDropDownFieldValue(batch, 0);

        public CreditedProfileForRole GetCreditedProfileForRoleWithSingleCredit(int batch)
        {
            var requestedBatch = FindTheRootElementForRoleWithSingleCredit(batch);
            try
            {
                var profileName = requestedBatch.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-H3.qa-profile-name"));
                var profileCredit = requestedBatch.FindElement(By.CssSelector(".kit-tag"));
                var profileLocation =
                    requestedBatch.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-B3.qa-profile-location"));
                var actionButtons = requestedBatch.FindElements(By.ClassName("kit-mint")).ToList();
                if (actionButtons.Count==3) actionButtons.RemoveAt(0);
                return new CreditedProfileForRole(profileName.Text,profileCredit.Text, profileLocation.Text,actionButtons.ElementAt(0),  actionButtons.ElementAt(1));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ProjectReleaseInformationPage RemoveCreditFromRoleAcceptingOneProfile(int batch)
        {
            var getCreditedProfile = GetCreditedProfileForRoleWithSingleCredit(batch);
            getCreditedProfile.RemoveBtn.Click();
            Thread.Sleep(2500);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage CreateAndAddProfileToRoleAcceptingOneRole(int batch, string expectedProfileName, string location)
        {
            CreateProfileInlineForRole(batch, expectedProfileName, location);
            return new ProjectReleaseInformationPage(this._driver);
        }

        private FindOrCreateProfileToCredit CreateProfileInlineForRole(int batch, string expectedProfileName, string location)
        {
            var createdProfile =new FindOrCreateProfileToCredit(this._driver);
            var requiredBatch = FindTheRootElementForRoleWithSingleCredit(batch);
            var creatorPad = requiredBatch.CreatorPad();
            creatorPad.Click();
            createdProfile.CreateAndSelectProfileInline(expectedProfileName, location);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        /*private IWebElement CreatorPad(IWebElement requiredBatch)
        {
                var creatorPad = requiredBatch.FindElements(By.CssSelector(".kit-bar.isCreator"));
                return creatorPad.SingleOrDefault();
        }*/

        public bool IsCreatorButtonPresentForSingleProfileRoles(int batch)
        {
            var searchedBatch = FindTheRootElementForRoleWithSingleCredit(batch);
            var creatorPad = searchedBatch.CreatorPad();
            return creatorPad == null;
        }

        public ProjectReleaseInformationPage EditRoleAcceptingOneRole(int batch, string profile)
        {
            var getCreditedProfile = GetCreditedProfileForRoleWithSingleCredit(batch);
            getCreditedProfile.EditBtn.Click();
            CreditAProfile(profile);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public int getNumberOfCreatedProfilesForRolesWithSingleCredit(int batch)
        {
            var searchedBatch = FindTheRootElementForRoleWithSingleCredit(batch);
            var numberOfProfiles = searchedBatch.FindElements(By.CssSelector(".kit-row"));
            return numberOfProfiles.Count();
        }

        public List<CreditedProfileForRole> GetCreditedProfilesForRoleWithMultipleCredits(int batch)
        {
            var listOfCreditedProfiles=new List<CreditedProfileForRole>();
            var requestedBatch = FindAllCreditsWithMultipleProfiles().ElementAt(batch);
            var multipleCredits = requestedBatch.FindElements(By.CssSelector(".kit-bar")).ToList();
            if (multipleCredits.Count==1) return listOfCreditedProfiles;
            multipleCredits.RemoveAt(0);
            foreach (var elem in multipleCredits)
            {
                var profileName = elem.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-H3.qa-profile-name"));
                var profileCredit = elem.FindElement(By.CssSelector(".kit-tag"));
                var profileLocation =
                    elem.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-B3.qa-profile-location"));
                var actionButtons = elem.FindElements(By.ClassName("kit-mint"));
                listOfCreditedProfiles.Add(new CreditedProfileForRole(profileName.Text,profileCredit.Text, 
                    profileLocation.Text,null,actionButtons.ElementAt(0)));
            }
            return listOfCreditedProfiles;
        }

        public ProjectReleaseInformationPage RemoveCreditFromRoleAcceptingMultipleProfiles(int batch, List<string> profiles)
        {
            var getCreditedProfiles = GetCreditedProfilesForRoleWithMultipleCredits(batch);
            foreach (var elem in getCreditedProfiles)
            {
                if(profiles.Contains(elem.ProfileName))
                    elem.RemoveBtn.Click();
                Thread.Sleep(2500);
                PageFactory.InitElements(_driver, this);
            }
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage CreateAndAddProfileToRoleAcceptingMultipleProfiles(int batch, string profileName, string location)
        {
            var createdProfile =new FindOrCreateProfileToCredit(this._driver);
            var locatedBatch = FindAllCreditsWithMultipleProfiles().ElementAt(batch);
            var creatorPad = locatedBatch.CreatorPad();
            creatorPad.Click();
            createdProfile.CreateAndSelectProfileInline(profileName, location);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage ToggleOffLabelOption(int batch)
        {
            var labelProfile = FindTheRootElementForRoleWithSingleCredit(batch);
                ChangeToggle(labelProfile);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public string GetCatalogueNumber(int batch)
        {
            var catalogueNumber = FindAllInputFieldBatches().ElementAt(batch);
            return catalogueNumber.FindElement(By.TagName("input")).GetAttribute("value");
        }

        public ProjectReleaseInformationPage RemoveCatalogueNumber(int batch)
        {
            //var catalogueNumber = FindAllInputFieldBatches().ElementAt(batch);
            var catalogueNumber = GetInputValueInBatch(batch, 0);
            var catalogueInput = GetInputFieldValue(batch, 0);
            //catalogueNumber.GetAttribute("value").ToString();
            catalogueNumber.RemoveAllCharactersFromInputField(catalogueInput);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public ProjectReleaseInformationPage AddCreditsToRoleAcceptingMultipleProfiles(int batch,
            List<CreditedProfileForRole> listOfCollaboratorsWithRoles)
        {
            var creditComponent=FindAllCreditsWithMultipleProfiles().ElementAt(batch);
            foreach (var profile in listOfCollaboratorsWithRoles)
            {
                AddSingleCollaboratorCreditToExistingProfile(creditComponent, profile.ProfileName, profile.ProfileCredit);
            }
            Thread.Sleep(9000);
            return new ProjectReleaseInformationPage(this._driver);
        }

        public TrackListPage NavigateToTrackListPage()
        {
            Thread.Sleep(2000);
            _subMenu.NavitateToSpecificSubPage(EnvironmentVariables.ProjectTracks);
            Thread.Sleep(2000);
            return new TrackListPage(this._driver);
        }
    }
}
