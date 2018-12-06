using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Models;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class ProfilePage
    {
        private IWebDriver _driver;
        private SubMenu _subMenu;
        private Profile _profile;

        private ProfileReleasesAndTracksPage _profileReleasesAndTracksPage;
        //private ProfileRoyaltySplits _profileRoyaltySplits;
        private ProfileMembersPage members;
        //TODO move this to members page 
        //private List<Member> _listOfProfileMembers;
        //private Analytics _analytics;
        //private ProfileInfo _profileInfo;

        public ProfilePage(IWebDriver driver, SubMenu subMenu, Profile profile, ProfileReleasesAndTracksPage profileReleasesAndTracksPage)
        {
            //TODO if the initial navigation changes than the Contractor will have to change
            //I will need to change this to be more random
            _driver = driver;
            this._subMenu = new SubMenu(_driver);
            _profile = profile;
            _profileReleasesAndTracksPage = profileReleasesAndTracksPage;
        }
    }
}
