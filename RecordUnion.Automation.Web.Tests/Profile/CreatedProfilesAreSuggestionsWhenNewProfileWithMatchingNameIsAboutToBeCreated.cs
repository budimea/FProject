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
    class CreatedProfilesAreSuggestionsWhenNewProfileWithMatchingNameIsAboutToBeCreated
    {
        private static IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        public ListProfilesViewPage ListProfilesViewPage;
        public CreateProfileNamePage CreateProfileNamePage;
        public CreateProfileLocationPage CreateProfileLocationPage;
        public CreateProfileRoleTagsPage CreateProfileRoleTagPage;

        public String expectedProfileName = "Profile" + DateTime.Now.ToLongTimeString();

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
        }

        [Test, Order(1), Description("If user adds the exact same name that another Profile has on the platform," +
            " they are asked to verify that the Profile they want to create is not the same as that one")]
        public void ExistingProfileIsRetrievedAsSuggesetedWhileCreatingNewProfileWithExactSameName()
        {
            CreateProfileNamePage = ListProfilesViewPage
                .CreateNewProfile()
                .PopulateProfileName(expectedProfileName);

            IList<SuggestedMatchingResults> SuggestedMatchingResults = CreateProfileNamePage
                .ClickNextButton()
                .findAlreadyCreatedProfileWithExactName(expectedProfileName);

            //TODO Assert if the objects are equal
            Assert.AreEqual(SuggestedMatchingResults.Count, 1, "The suggested results are not correct.");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
