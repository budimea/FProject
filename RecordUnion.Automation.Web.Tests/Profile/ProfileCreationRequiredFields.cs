using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;

namespace RecordUnion.Automation.Web.Tests.Profile
{
    class ProfileCreationRequiredFields
    {
        private static IWebDriver driver;
        public LogInPage logInPage;
        public DashboardPage dashboardPage;
        public ListProfilesViewPage listProfilesViewPage;
        private String ExpectedProfileName;
        public CreateProfileNamePage createProfileNamePage;
        public CreateProfileLocationPage createProfileLocationPage;
        public CreateProfileRoleTagsPage createProfileRoleTagPage;
        public ProfileCard profileCard;
        public ProfilePage profilePage;
        public ProfileReleasesAndTracksPage profileReleasesAndTracksPage;
        public ProfileMembersPage profileMembersPage;
        private Random _rand;
        public int initialNumberOfCreatedProfiles;

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.FullScreen();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            logInPage = new LogInPage(driver);
            dashboardPage = logInPage
                .SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
            listProfilesViewPage = dashboardPage.GoToProfilesScreen();
            initialNumberOfCreatedProfiles = listProfilesViewPage.NumberOfCreatedProfiles();
            _rand = new Random();
            Thread.Sleep(5000);
        }

        [Test, Order(0), Description("Profile Name is a required field")]
        public void ProfileNameMustNotBeEmpty()
        {
            createProfileNamePage = listProfilesViewPage
                .CreateNewProfile();

            Assert.IsFalse(createProfileNamePage.CheckIfNextButtonIsEnabled(), "Button is not disabled.");
        }

        [Test, Order(1), Description("User cannot provide spaces as a valid Profile Name")]
        public void ProfileNameIsNotValidIfItOnlyContainsSpaces()
        {
            createProfileNamePage = createProfileNamePage
                .PopulateProfileName(CommonUsedVariables.OnlySpacesString);

            Assert.IsFalse(createProfileNamePage.CheckIfNextButtonIsEnabled(), "Button is not disabled.");
        }

        [Test, Order(2), Description("ProfileName is a Required field.")]
        public void ProfileNameIsRequiredField()
        {
            ExpectedProfileName = "Profile_" + _rand.NextDouble();
            createProfileNamePage = createProfileNamePage
              .ClickOnLogoToReturnOnAllProfilesPage()
              .GoToProfilesScreen()
              .CreateNewProfile()
              .PopulateProfileName(ExpectedProfileName);

            Assert.IsTrue(createProfileNamePage.CheckIfNextButtonIsEnabled(), "Next button is not enabled.");
        }

        [Test, Order(3), Description("User cannot proceed until he provides a location")]
        public void ProfileLocationMustNotBeEmpty()
        {
            createProfileLocationPage = createProfileNamePage
                .ClickOnNextButtonToNavigateOnLocationScreen();

            Assert.IsFalse(createProfileLocationPage.CheckIfNextButtonIsEnabled(), "Next button is not disabled.");
        }

        [Test, Order(4), Description("Location is required field")]
        public void ProfileLocationIsRequiredField()
        {
            createProfileLocationPage = createProfileLocationPage
                .ChooseProfileLocation(Locations.Stockholm);

            bool actualResults = createProfileLocationPage.CheckIfNextButtonIsEnabled();


            //toDo add a soft assertImplementation for asserting  multiple conditions in same time
            Assert.IsTrue(actualResults, "Next Button is not enabled.");
        }

        [Test, Order(5), Description("User must specify at least 1 role tag")]
        public void UserCannotProceedIfNoRoleTagIsProvided()
        {
            createProfileRoleTagPage = createProfileLocationPage
                .ClickOnNextButtonToNavigateOnRoleTagScreen();

            bool actualResults = createProfileRoleTagPage.CheckIfCreateButtonIsEnabled();
            Assert.IsFalse(actualResults, "Next Button is enabled.");
        }

        [Test, Order(6), Description("Role Tag is a required field for the Profile to be created.")]
        public void RoleTagIsARequiredFieldForProfileNameCreation()
        {
            //TODO I will need to add additional checks for clearing the state of the buttons
            createProfileRoleTagPage = createProfileRoleTagPage
                .ChooseMemberRoleTag(RoleTags.Drummer);

            bool actualResults = createProfileRoleTagPage.CheckIfCreateButtonIsEnabled();
            listProfilesViewPage = createProfileRoleTagPage
                .ClickToCreateProfile();

            Assert.IsTrue(actualResults, "Create Profile Button Is not enabled.");
        }

        [Test, Order(7), Description("When providing all required infomation profile is created and presented on Profiles Page")]
        public void CreatedProfileIsPresentOnProfilesPage()
        {
            int actualNumberOfCreateProfiles = listProfilesViewPage.NumberOfCreatedProfiles();

            Assert.AreEqual(initialNumberOfCreatedProfiles+1, actualNumberOfCreateProfiles, 
                "Profile hasn't been successfully created and listed on Profiles screen");
        }

        [Test, Order(9), Description("Profile name is same as the intended one")]
        public void ProfileNameIsSameAsTheOneEnteredDuringProfileCreaton()
        {
            IList<ProfileCard> actualProfieleCreated= listProfilesViewPage
                .FindProfileByName(ExpectedProfileName);

            Assert.AreEqual(1, actualProfieleCreated.Count, "The Number of Profiles is not one");
        }

        
        [Test, Order(10), Description("When created, the user becomes a member of the Profile with permission level Admin")]
        public void ProfileCreatorIsProfileAdmin()
        {
            //TODO 1. FIX THE PROBLM
            //TODO create smarter tubs somehow with calling a general submenu object

            MemberCard CurrentCreator = listProfilesViewPage
                 .ClickToViewProfile(ExpectedProfileName)
                 .navigateToProfileMembersPage()
                 .readCurrentMembers()
                 .findCurrentCreator();

            Assert.AreEqual(EnvironmentVariables.QALoginName, CurrentCreator.MemberName.Text, "Creator is not admin");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }

}
