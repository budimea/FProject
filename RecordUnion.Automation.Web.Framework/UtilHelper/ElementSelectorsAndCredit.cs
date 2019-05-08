using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Pages.Projects;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.UtilHelper
{
    public static class ElementSelectorsAndCredit
    {
        public static void ChangeReleaseInputWithNewValue(this IWebDriver driver, IWebElement _elementContainer, int batch, int field, string newValue)
        {
            var fieldBatch=FindElementInBatchInputs(_elementContainer, batch, field);
            Thread.Sleep(2000);
            if (!fieldBatch.InputField.Displayed)
                driver.EnableToggleForFieldOrComponent(_elementContainer, batch);
            var existingValue = string.Empty;
            existingValue = GetInputFieldValue(_elementContainer,batch, field);
            fieldBatch.InputField.RemoveAllCharactersFromInputField(existingValue);
            Thread.Sleep(1000);
            fieldBatch.InputField.SendKeys(newValue);
        }
        
        private static ValidatedInputFields FindElementInBatchInputs(this IWebElement elementContainer, int batch, int index)
        {
            ReleaseInputBatchComponent batchInput= FindSpecificComponent(elementContainer, batch);
            return batchInput.InputFields.ElementAt(index);
        } 
        public static ReleaseInputBatchComponent FindSpecificComponent(this IWebElement elementContainer, int batch)
        {
            var _rootElement = ReadSpecificBatch(elementContainer, batch);
            var _batchInputs = _rootElement.FindInputFieldsGeneric();
            var _submit = _rootElement.FindElement(
                By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypePRIMARY"));
            var _cancel = _rootElement.FindElement(
                By.CssSelector(".kit-button.button.enButtonSizeLARGE.enButtonTypeTERTIARY"));
            var batchToggle = TryGetToggle(_rootElement);
            return new ReleaseInputBatchComponent(_batchInputs, batchToggle, _submit, _cancel);
        }
        private static IWebElement ReadSpecificBatch(IWebElement elementContainer, int batch)=>elementContainer.FindAllInputFieldBatches().ElementAt(batch);
        
        public static List<IWebElement> FindAllInputFieldBatches(this IWebElement _elementContainer)
        {
            var list = _elementContainer
                .FindElements(By.ClassName("kit-form")).ToList();
            return list;
        }

        private static IWebElement TryGetToggle(IWebElement root)
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
        private static void EnableToggleForFieldOrComponent(this IWebDriver driver, IWebElement elementContainer, int releaseBatch)
        {
            var batch= ReadSpecificBatch(elementContainer,releaseBatch);
            if(!CheckIfToggleIsEnabled(batch))
                ChangeToggle(batch);
            else throw new Exception("Test Flow Exception");
        }
        
        private static bool CheckIfToggleIsEnabled(IWebElement parentElement) =>
            parentElement.FindElements(By.CssSelector(".kit-toggle.isOn")).Count > 0;
        
        private static void ChangeToggle(IWebElement element)
        {
            IWebElement toggle = element.FindElement(By.ClassName("slider"));
            toggle.Click();
            Thread.Sleep(1000);
        }
        public static string GetInputFieldValue(this IWebElement elementContainer, int batch, int field)=>elementContainer.GetInputValueInBatch(batch, field).GetAttribute("value");
        
        public static IWebElement GetInputValueInBatch(this IWebElement _elementContainer, int batch, int field)=> _elementContainer.FindElementInBatchInputs(batch, field).InputField;
        
        public static void SaveSpecificInputBatchChange(this IWebElement elementContainer,int batch)
        {
            IWebElement submitElement = getSubmitButton(elementContainer,batch);
            Thread.Sleep(5000);
            submitElement.Click();
        }
        
        private static IWebElement getSubmitButton(IWebElement elementContainer,int batch)
        {
            ReleaseInputBatchComponent changedBatch = null;
            changedBatch=FindSpecificComponent(elementContainer,batch);
            if(changedBatch==null)
                throw new Exception("The batch is not changed");
            return changedBatch.Submit;
        }
        
        public static void SelectValueFromDropDownInput(this IWebElement elementContainer, int batch, int field, string selection)
        {
            var dropDownField = FindElementInBatchDropDown(elementContainer,batch, field);
            if (!checkIfDropDownFieldIsVisible(dropDownField))
                elementContainer.EnableToggleForFieldOrComponent1(batch);
            elementContainer.ExpandNonSearchableDropDown(dropDownField);
            ChooseFromNonSearchableDropDown(dropDownField, field,selection);
        }
        
        private static void EnableToggleForFieldOrComponent1(this IWebElement elementContainer, int releaseBatch)
        {
            var batch= ReadSpecificBatch(elementContainer,releaseBatch);
            if(!CheckIfToggleIsEnabled(batch))
                ChangeToggle(batch);
            else throw new Exception("Test Flow Exception");
        }
        private static EditReleaseMetadataDropDownComponent FindElementInBatchDropDown(IWebElement elementContainer, int batch, int field)
        {
            var _rootDropDownContainer = ReadSpecificBatch(elementContainer, batch);
            return FindDropDownGeneric(_rootDropDownContainer, field);
        }
        
        private static EditReleaseMetadataDropDownComponent FindDropDownGeneric(IWebElement rootDropDownContainer, int field)
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
        
        private static bool checkIfDropDownFieldIsVisible(EditReleaseMetadataDropDownComponent dropDownField)
        {
            var elementExist =
                dropDownField.InputDropDown.FindElements(
                    By.CssSelector(".kit-validatee.kit-validatee-base-BLACKCURRANT_14.isCollapsed"));
            return elementExist.Count != 1;
        }
        
        private static void ExpandNonSearchableDropDown(this IWebElement element, EditReleaseMetadataDropDownComponent dropDownField)
        {
            dropDownField.DropDownExpander.Click();
        }
        
        private static void ChooseFromNonSearchableDropDown(
            EditReleaseMetadataDropDownComponent dropDownField, int field, string selection)=>dropDownField.RootDropDownElement.SelectFromStaticDropDown(selection);
        
        public static string GetDropDownFieldValue(this IWebElement elementContainer, int batch, int field)
        {
            var dropDownElement = GetDropDownInputField(elementContainer, batch, field);
            var elementText = dropDownElement.FindElement(By.TagName("input"));
            return elementText.GetAttribute("value");
        }
        
        private static IWebElement GetDropDownInputField(IWebElement elementContainer,int batch, int field) =>
            FindElementInBatchDropDown(elementContainer, batch, field).InputDropDown;
        
        public static void ChangeValueForToggledInputField(this IWebDriver driver, IWebElement elementContainer, int field, string catalogueNum)
        {
            var catalogueNumber = FindAllInputFieldBatches(elementContainer).ElementAt(field);
            if (catalogueNumber.FindElements(By.CssSelector("input[type='text']")).Count==0||!CheckIfToggleIsEnabled(catalogueNumber))
                    ChangeToggle(catalogueNumber);
            /*if(!CheckIfToggleIsEnabled(catalogueNumber))
                ChangeToggle(catalogueNumber);*/
            PopulateCatalogueNumber(catalogueNumber, catalogueNum);
        }
        private static void PopulateCatalogueNumber(IWebElement catalogueNumber, string catalogueNum)
        {
            var catalogueInput = catalogueNumber.FindElement(By.CssSelector("input[type='text']"));
            catalogueInput.SendKeys(catalogueNum);
        }
        
        public static void FindAndClickToCreditProfileForRoleAcceptingOneCredit(this IWebElement elementContainer, int role)
        {
            var creditComponent = elementContainer.FindTheRootElementForRoleWithSingleCreditForTracks(role);
            var creatorPad = creditComponent.CreatorPad();
            Thread.Sleep(1000);
            creatorPad.Click();
        }

        private static IWebElement FindTheRootElementForRoleWithSingleCreditForTracks(this IWebElement elementContainer, int role) =>elementContainer.FindElement(By.CssSelector(".form-projects-view-tracks-copyright-owner"));

        public static CreditedProfileForRole GetCreditedProfilesForRole(this IWebElement elementContainer, int batch)
        {
            var requestedBatch = FindTheRootElementForRoleWithSingleCreditForTracks(elementContainer, batch);
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
        
        public static IWebElement FindAllCreditsWithMultipleProfiles(this IWebElement _elementContainer, int role) => _elementContainer
            .FindElements(By.CssSelector(".form-projects-primary-artists")).ToList().ElementAt(role);

        public static IWebElement FindAllCreditsWithCISRolesWithMultipleProfiles(this IWebElement elementContainer, int role)
        {
            return elementContainer
                .FindElements(By.CssSelector(".qa-kit-bar-creator")).ToList().ElementAt(role);      
        }

        public static List<CreditedProfileForRole> FindAllCreditedProfiles(this IWebElement elementContainer, List<CreditedProfileForRole> list)
        {
            var multipleCredits = elementContainer.FindElements(By.CssSelector(".kit-bar")).ToList();
            if (multipleCredits.Count==1) return list;
            multipleCredits.RemoveAt(0);
            foreach (var elem in multipleCredits)
            {
                var profileName = elem.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-H3.qa-profile-name"));
                var profileCredit = elem.FindElement(By.CssSelector(".kit-tag"));
                var profileLocation =
                    elem.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-B3.qa-profile-location"));
                var actionButtons = elem.FindElements(By.ClassName("kit-mint"));
                list.Add(new CreditedProfileForRole(profileName.Text,profileCredit.Text, 
                    profileLocation.Text,null,actionButtons.ElementAt(0)));
            }
            return list;
        }

        public static void CancelSpecificInputBatchChange(this IWebElement elementContainer, int batch)
        {
            IWebElement cancelButton = getCancelButton(elementContainer,batch);
            Thread.Sleep(5000);
            cancelButton.Click();
        }
        
        private static IWebElement getCancelButton(IWebElement elementContainer,int batch)
        {
            ReleaseInputBatchComponent changedBatch = null;
            changedBatch=FindSpecificComponent(elementContainer,batch);
            if(changedBatch==null)
                throw new Exception("The batch is not changed");
            return changedBatch.Cancel;
        }
            
        public static bool AreActionButtonsDisplayed(this ReleaseInputBatchComponent element)
        {
            return element.Submit.Displayed;
        }

    }
}