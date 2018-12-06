using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class ListProfilesViewPage
    {

        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".kit-grid")]
        private IWebElement _kitCardPadsGrid;

        [FindsBy(How = How.CssSelector, Using = ".kit-card-add")]
        private IWebElement _kitCardCreateProfilePad;

        private IList<ProfileCard> _listOfMyProfiles;

        public ListProfilesViewPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public string ValidateIAmOnProfilesPage()=> _driver.Url.ToString();

        public void getAllCreateProfilePads()
        {
            _listOfMyProfiles = new List<ProfileCard>();
            var listOfProfiles= new List<IWebElement>(_kitCardPadsGrid.FindElements(By.CssSelector(".kit-card.kit-element.isClickable")));
            listOfProfiles.RemoveAt(0);
            foreach (IWebElement elem in listOfProfiles)
            {
                var profileName = elem.FindElement(By.CssSelector(".kit-card-title"));
                var btnViewProfile = elem.FindElement(By.TagName("button"));
                _listOfMyProfiles.Add(new ProfileCard(profileName, btnViewProfile));
            }
        }

        public IWebElement CreateProfilePad()=> _kitCardCreateProfilePad;

        public ListProfilesViewPage ClickToCreateProfileUnverified()
        {
            _kitCardCreateProfilePad.Click();
            return new ListProfilesViewPage(this._driver);
        }

        public ListProfilesViewPage CloseUnverifiedPopup()
        {
            if (UnverifiedUserPopupIsPresent())
            {
                IWebElement popUpContaier = _driver.FindElement(By.ClassName("kit-dialog-container"));
                IWebElement closeButton = popUpContaier.FindElement(By.CssSelector(".kit-svg"));
                closeButton.Click();
                return new ListProfilesViewPage(this._driver);
            }
            else throw new Exception("Unverified User popup is missing");
        }

        public bool UnverifiedUserPopupIsPresent()
        {
            if (this._driver.IsElementPresent(By.ClassName("kit-dialog-container")))
                return true;
            else return false;
        }

        public int NumberOfCreatedProfiles()
        {
            Thread.Sleep(10000);
            getAllCreateProfilePads();
            return _listOfMyProfiles.Count;
        } 


        public CreateProfileNamePage CreateNewProfile()
        {
            _kitCardCreateProfilePad.Click();
            Thread.Sleep(5000);
            return new CreateProfileNamePage(this._driver);
        }

        public IList<ProfileCard> FindProfileByName(String ProfileName)
        {
            getAllCreateProfilePads();
            return FindProfileCardWithGivenProfileName(ProfileName);

        }

        private IList<ProfileCard> FindProfileCardWithGivenProfileName(string profileName)
        {
            IList<ProfileCard> matchingCard = _listOfMyProfiles.Where(elem => elem.ProfileName.Text == profileName).ToList();
            return matchingCard;
        }

        public ProfileReleasesAndTracksPage ClickToViewProfile(string profileName)
        {
            getAllCreateProfilePads();
            ProfileCard profileCard= FindProfileCardWithGivenProfileName(profileName).First();
            Thread.Sleep(1000);
            profileCard.ViewProfileButton.Click();
            return new ProfileReleasesAndTracksPage(this._driver);
        }

    }
}