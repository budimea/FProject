using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Constants;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class ProfileReleasesAndTracksPage
    {
        private IWebDriver _driver;
        //TODO I will need to remove this stupid thing from here
        private SubMenu _subMenu;

        public ProfileReleasesAndTracksPage(IWebDriver driver)
        {
            _subMenu = new SubMenu(driver);
            _driver = driver;
        }

        public ProfileInfoPage navigateToProfileInfo()
        {
            _subMenu.NavitateToSpecificSubPage(EnvironmentVariables.ProfileInfoSubPage);
            return new ProfileInfoPage(this._driver);
        }

        public ProfileMembersPage navigateToProfileMembersPage()
        {
            Thread.Sleep(5000);
            _subMenu.NavitateToSpecificSubPage(EnvironmentVariables.ProfileMembersSubPage);
            return new ProfileMembersPage(this._driver);
        }

    }
}
