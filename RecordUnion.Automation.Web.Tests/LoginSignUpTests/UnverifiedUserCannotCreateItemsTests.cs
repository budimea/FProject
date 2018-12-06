using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;
using RecordUnion.Automation.Web.Framework.Pages.Projects;
using RecordUnion.Automation.Web.Framework.Pages.SignUp;

namespace RecordUnion.Automation.Web.Tests.LoginSignUpTests
{
    class UnverifiedUserCannotCreateItemsTests
    {
        private static IWebDriver driver;
        private SignUpPage signUpPage;
        private DashboardPage dashboardPage;
        private ListProfilesViewPage ListProfilesViewPage;
        private ListReleasesViewPage ListReleasesViewPage;
        private static Random signUpEmail = new Random();
        private static Random signUpName = new Random();
        private static Random signUpSurname = new Random();

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaSignUpEmailUrl);
            signUpPage = new SignUpPage(driver);
            dashboardPage = signUpPage.SuccessfullSignUpUnVerified(RandomString(signUpName), RandomString(signUpSurname), RandomString(signUpEmail) + "@test.com");
        }

        [Test, Order(1), Description("User is unable to create Profiles if unverified.")]
        public void verifyUnverifiedUserIsUnableToCreateProfiles()
        {
            ListProfilesViewPage = dashboardPage.GoToProfilesScreen();
            ListProfilesViewPage = ListProfilesViewPage
                .ClickToCreateProfileUnverified();

            bool actualResults = ListProfilesViewPage.UnverifiedUserPopupIsPresent();
            ListProfilesViewPage = ListProfilesViewPage.CloseUnverifiedPopup();

            Assert.IsTrue(actualResults, "User Is Not Blocked From Creating Profile");
        }

        [Test, Order(2), Description("User is unable to create Projects if unverified.")]
        public void verifyUnverifiedUserIsUanableToCreateProjects()
        {
            ListReleasesViewPage = dashboardPage.GoToProjectScreen();
            ListReleasesViewPage = ListReleasesViewPage
                .ClickToCreateReleaseUnverified();

            bool actualResults = ListReleasesViewPage.UnverifiedUserPopupIsPresent();
            ListReleasesViewPage = ListReleasesViewPage.CloseUnverifiedPopup();

            Assert.IsTrue(actualResults, "User Is Not Blocked From Creating Profile");
        }

        public string RandomString(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).
                Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
