using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class ProjectTrackArtistCollaboratorsPage
    {
        private IWebDriver _driver;
        
        private FindOrCreateProfileToCredit _findOrCreateProfileToCredit;
        private SubMenu _subMenu;
        
        [FindsBy(How = How.CssSelector, Using = ".kit-element.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;
        public ProjectTrackArtistCollaboratorsPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public ProjectTrackArtistCollaboratorsPage CreditProfilesToRolesAcceptingMoreProfiles(int role, List<CreditedProfileForRole> listOfProfiles)
        {
            var creditComponent = _elementContainer.FindAllCreditsWithMultipleProfiles(role);
            AddNewCreditToProfile(creditComponent, listOfProfiles);
            Thread.Sleep(9000);
            return new ProjectTrackArtistCollaboratorsPage(this._driver);
        }

        public ProjectTrackArtistCollaboratorsPage CreditProfilesToRolesAcceptingMoreProfilesWithCISRoles(int role,
            List<CreditedProfileForRole> listOfProfiles)
        {
//            var creditComponent=_elementContainer.FindAllCreditsWithCISRolesWithMultipleProfiles().ElementAt(batch);
//            foreach (var profile in listOfCollaboratorsWithRoles)
//            {
//                AddSingleCollaboratorCreditToExistingProfile(creditComponent, profile.ProfileName, profile.ProfileCredit);
//            }
//            Thread.Sleep(9000);
            return new ProjectTrackArtistCollaboratorsPage(this._driver);
        }
        
        private ProjectTrackArtistCollaboratorsPage AddNewCreditToProfile(IWebElement credit, List<CreditedProfileForRole> listOfProfiles)
        {
            var creditPad = credit.CreatorPad();
            foreach (var profile in listOfProfiles)
            {
                creditPad.Click();
                Thread.Sleep(10000);
                CreditAProfile(profile.ProfileName);
            }
            return new ProjectTrackArtistCollaboratorsPage(this._driver);
        }
        
        private ProjectTrackArtistCollaboratorsPage CreditAProfile(string selection)
        {
            _findOrCreateProfileToCredit=new FindOrCreateProfileToCredit(this._driver);
            _findOrCreateProfileToCredit.SearchAndCreditExistingProfile(selection);
            return new ProjectTrackArtistCollaboratorsPage(this._driver);
        }

        public List<CreditedProfileForRole> GetCreditedProfilesForRoleWithMultipleCredits(int role)
        {
            var listOfCreditedProfiles=new List<CreditedProfileForRole>();
            var requestedBatch = _elementContainer.FindAllCreditsWithMultipleProfiles(role);
            listOfCreditedProfiles=requestedBatch.FindAllCreditedProfiles(listOfCreditedProfiles);
            return listOfCreditedProfiles;
        }
        
    }
}