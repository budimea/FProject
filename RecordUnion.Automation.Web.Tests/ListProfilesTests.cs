using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;

namespace RecordUnion.Automation.Web.Tests
{
    public class ListProfilesTests
    {
        private static IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        public ListProfilesViewPage ListProfilesViewPage;
        //public IWebElement createProfilePad;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            driver=new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait =TimeSpan.FromSeconds(50);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            LogInPage=new LogInPage(driver);
            DashboardPage = LogInPage
                .SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
        }
        
        [Test, Order(0)]
        public void AUserIsNavigatedOnProfilesPage()
        {
            ListProfilesViewPage = DashboardPage.GoToProfilesScreen();
            Assert.AreEqual("https://qa-spa.recordunion.com/profiles", ListProfilesViewPage.ValidateIAmOnProfilesPage());
        }

        [Test, Order(2)]
        public void BCreateProfilePadIsPresent()
        {
            Assert.NotNull(ListProfilesViewPage.CreateProfilePad());
        }

        [Test, Order(3)]
        public void CverifyWhenUserCreatesNewProfileItIsDisplayedOnTheScreen()
        {
            //Write this test after writing tests for create a profile
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}