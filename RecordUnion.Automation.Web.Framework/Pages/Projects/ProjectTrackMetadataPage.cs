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
    public class ProjectTrackMetadataPage
    {
        private IWebDriver _driver;
        private FindOrCreateProfileToCredit _findOrCreateProfileToCredit;
        private SubMenu _subMenu;
        
        [FindsBy(How = How.CssSelector, Using = ".kit-element.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;
        public ProjectTrackMetadataPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver,this);
            _subMenu=new SubMenu(_driver);
        }

        public ProjectTrackMetadataPage ChangeTrackInputBatchField(int batch, int field, string value)
        {
            if (batch == TrackMetadataBatches.TrackISRCNumber||batch == TrackMetadataBatches.TrackISWCCode)
                _driver.ChangeValueForToggledInputField(_elementContainer,batch, value);
            else if (batch == TrackMetadataBatches.TrackLyricsLanguage)
                ChooseValueFromDropDownList(batch, field, value);
            else _driver.ChangeReleaseInputWithNewValue(_elementContainer,batch, field, value);
            return new ProjectTrackMetadataPage(this._driver);
        }

        public ProjectTrackMetadataPage SaveTrackBatchWithoutValidations(int batch)
        {
            _elementContainer.SaveSpecificInputBatchChange(batch);
            Thread.Sleep(1500);
            return new ProjectTrackMetadataPage(this._driver);
        }
        public string ReadInputValueForField(int batch, int field)=> _elementContainer.GetInputFieldValue(batch, field);
        
        public ProjectTrackMetadataPage ChooseValueFromDropDownList(int batch, int field, string selection)
        {
            _elementContainer.SelectValueFromDropDownInput(batch, field, selection);
            return new ProjectTrackMetadataPage(this._driver);
        }

        public string ReadDropDownFieldValue(int batch, int field)=> _elementContainer.GetDropDownFieldValue(batch, field);
        
        public ListReleasesViewPage NavigateToProjectsScreen()
        {
            var btnReleases=_driver.ReturnHeaderNavigationItems().ElementAt(2);
            btnReleases.Click();
            return new ListReleasesViewPage(_driver);;
        }

        public ProjectTrackMetadataPage ChangeTextArea(string lyrics)
        {
            var TextArea = FindTextArea();
            TextArea.SendKeys(lyrics);
            return new ProjectTrackMetadataPage(this._driver);
        }
        public string ReadLyricsValue()
        {
            var TextArea = FindTextArea();
            return TextArea.Text;
        }
        
        public ProjectTrackMetadataPage CreditProfileToTrackCopyrightOwner(int role, string existingProfile)
        {
            _elementContainer.FindAndClickToCreditProfileForRoleAcceptingOneCredit(role);
            Thread.Sleep(5000);
            CreditAProfile(existingProfile);
            return new ProjectTrackMetadataPage(this._driver);
        }

        public ProjectTrackMetadataPage CreditProfileToRoleAcceptingOneProfile(int role, string existingProfile)
        {
            _elementContainer.FindAndClickToCreditProfileForRoleAcceptingOneCredit(role);
            Thread.Sleep(10000);
            return new ProjectTrackMetadataPage(this._driver);
        }
        
        private IWebElement FindTextArea()=> _elementContainer.FindElement(By.ClassName("kit-text-text"));

        private ProjectTrackMetadataPage CreditAProfile(string selection)
        {
            _findOrCreateProfileToCredit=new FindOrCreateProfileToCredit(this._driver);
            _findOrCreateProfileToCredit.SearchAndCreditExistingProfile(selection);
            return new ProjectTrackMetadataPage(this._driver);
        }

        public CreditedProfileForRole GetProfileCreditedForRoleWithSingleCredit(int batch)=> _elementContainer.GetCreditedProfilesForRole(batch);
        
        public ProjectTrackArtistCollaboratorsPage NavigateToArtistAndCollaborators()
        {
            Thread.Sleep(2000);
            _subMenu.NavitateToSpecificSubPage(EnvironmentVariables.ProjectTracksArtistCollaborators);
            Thread.Sleep(2000);
            return new ProjectTrackArtistCollaboratorsPage(this._driver);
        }
        
        public List<string> ReadAllMessagesForGivenField(int batch, int field, string validationType)
        {
            var inputBatch = _elementContainer.FindAllInputFieldBatches().ElementAt(batch);
            return inputBatch.ReadAllValidationMessagesForInputField(field, validationType);
        }

        public bool AreActionButtonsDisplayed(int batch)
        {
            var component = _elementContainer.FindSpecificComponent(batch);
            return component.AreActionButtonsDisplayed();
        }

        public ProjectTrackMetadataPage CancelTrackInputBatchFieldChange(int batch)
        {
            _elementContainer.CancelSpecificInputBatchChange(batch);
            Thread.Sleep(1500);
            return new ProjectTrackMetadataPage(this._driver);
        }
    }
}