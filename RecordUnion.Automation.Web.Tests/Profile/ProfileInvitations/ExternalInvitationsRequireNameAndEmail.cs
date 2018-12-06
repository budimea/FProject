using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;

namespace RecordUnion.Automation.Web.Tests.Profile.ProfileInvitations
{
    class ExternalInvitationsRequireNameAndEmail
    {
        private static IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        public ListProfilesViewPage ListProfilesViewPage;
        public CreateProfileNamePage CreateProfileNamePage;
        public CreateProfileLocationPage CreateProfileLocationPage;
        public CreateProfileRoleTagsPage CreateProfileRoleTagPage;

        public String expectedProfileName = "Profile" + DateTime.Now.ToLongTimeString();
        private ProfileMembersPage ProfileMemberPage;

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            LogInPage = new LogInPage(driver);
            DashboardPage = LogInPage
                .SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
            ListProfilesViewPage = DashboardPage
                .GoToProfilesScreen()
                .CreateNewProfile()
                .PopulateProfileNameAndClickNext(expectedProfileName)
                .ChooseProfileLocationAndCLickNext(Locations.Stockholm)
                .ChooseProfileRoleTagsClickCreate(RoleTags.Diplo);

            Thread.Sleep(5000);

            ProfileMemberPage = ListProfilesViewPage
                .ClickToViewProfile(expectedProfileName)
                .navigateToProfileMembersPage();

        }
    }
}
