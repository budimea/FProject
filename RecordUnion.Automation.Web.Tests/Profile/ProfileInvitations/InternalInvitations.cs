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
    class InternalInvitations
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
        private ProfileMemberInvitationPage ProfileMemberInvitationPage;

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

        [Test, Order(1), Description("Profile Admin is able to invite existing RU members")]
        public void verifyProfileAdminIsAbleToInviteExistingRUMember()
        {
            ProfileMemberPage = ProfileMemberPage
                .clickToInviteNewMember()
                .AddExistingRUMember(EnvironmentVariables.QARUMemberEmail)
                .SendInvitation();


            ProfileMemberPage = DashboardPage
                .GoToProfilesScreen()
                .ClickToViewProfile(expectedProfileName)
                .navigateToProfileMembersPage()
                .readCurrentMembers();
          

            int expectedNumberOfMembers = ProfileMemberPage
                .CurrentNumberOfProfileMembersCards();

            Assert.AreEqual(expectedNumberOfMembers, 2, "Invitation is not send.");
        }

        [Test, Order(2), Description("It is not possible for a user who currently has an Active invitation to Join the Profile to be invited again")]
        public void UserCanHaveOnlyOneActiveInvitationFromOneProfile()
        {
            ProfileMemberInvitationPage = ProfileMemberPage
               .clickToInviteNewMember()
               .SearchAndSelectExistingRUMember(EnvironmentVariables.QARUMemberEmail);
        }


        [TearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }

    }
}
