using System;
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
    class ConstraisForProfileCreation
    {
        private static IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        public ListProfilesViewPage ListProfilesViewPage;
        private String ExpectedProfileName;
        public CreateProfileNamePage CreateProfileNamePage;
        public CreateProfileLocationPage CreateProfileLocationPage;
        public CreateProfileRoleTagsPage CreateProfileRoleTagPage;
        public ProfileCard ProfileCard;

        public String expectedProfileName = "Profile" + DateTime.Now.ToLongTimeString();

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.FullScreen();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            LogInPage = new LogInPage(driver);
            DashboardPage = LogInPage
                .SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
            CreateProfileRoleTagPage = DashboardPage
                .GoToProfilesScreen()
                .CreateNewProfile()
                .PopulateProfileNameAndClickNext(expectedProfileName)
                .ChooseProfileLocationAndCLickNext(Locations.Stockholm);

            Thread.Sleep(5000);
        }

        [Test, Order(1), Description("User can choose maximum of 10 roletags per profile")]
        public void UserCanChooseTenRoleTagsPerProfile()
        {
            CreateProfileRoleTagPage = CreateProfileRoleTagPage
                .InserAndChooseFromValidRoleTag(RoleTags.DJ)
                .InserAndChooseFromValidRoleTag(RoleTags.Drummer)
                .InserAndChooseFromValidRoleTag(RoleTags.Engineer)
                .InserAndChooseFromValidRoleTag(RoleTags.Diplo)
                .InserAndChooseFromValidRoleTag(RoleTags.Guitarist)
                .InserAndChooseFromValidRoleTag(RoleTags.HandClapper)
                .InserAndChooseFromValidRoleTag(RoleTags.MC)
                .InserAndChooseFromValidRoleTag(RoleTags.Musician)
                .InserAndChooseFromValidRoleTag(RoleTags.Pianist)
                .InserAndChooseFromValidRoleTag(RoleTags.Rapper);

            bool actualResults = CreateProfileRoleTagPage.CheckIfCreateButtonIsEnabled();

            Assert.IsTrue(actualResults, "Create Profile Button Is not enabled.");
        }

        [Test, Order(2), Description("User cannot choose more than 10 role tags.")]
        public void UserCannotChooseMoreThanTenRoleTags()
        {

            CreateProfileRoleTagPage = CreateProfileRoleTagPage
                .InserAndChooseFromValidRoleTag(RoleTags.Vocalist);

            bool actualResults = CreateProfileRoleTagPage.CheckIfCreateButtonIsEnabled();

            Assert.IsFalse(actualResults, "Create Profile Button Is not enabled.");

            //IList<String> actualErrors = CreateProfileRoleTagPage
            //.GetAllErrorsForExceedingNumberOfRoleTags();

            //TODO Create an Assertion for the error messages
            //Assert.AreEqual(actualErrors, "User is not promped for exceeding number of Errors on screen.");
        }

        [Test, Order(3), Description("Profile can be created with 10 roletags")]
        public void ByRemovingTheExtraUserRoleTagUserCanCreateTheProfile()
        {
            //TODO Add remove Role Tag and test successfull creation
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
