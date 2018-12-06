using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;

namespace RecordUnion.Automation.Web.Tests.Profile
{
    public class CreateProfileTests
    {
        private static IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        public ListProfilesViewPage ListProfilesViewPage;
        public CreateProfileNamePage _createProfileNamePage;
        public CreateProfileNamePage CreateProfileNamePage;


        [OneTimeSetUp]
        public void SetUp()
        {
            
            driver =new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            LogInPage=new LogInPage(driver);
            DashboardPage = LogInPage
                .SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
            ListProfilesViewPage = DashboardPage.GoToProfilesScreen();
            
            Thread.Sleep(5000);
        }

        [Test, Order(0)]
        public void validateProfileCanBeSuccessfullyCreated()
        {
            //toDo create a RandomName Generator to populate the profile name, Location and RoleTag
            double expectedResults = ListProfilesViewPage
                //.readNumberofMyProfiles()
                .NumberOfCreatedProfiles();
            ListProfilesViewPage = ListProfilesViewPage
               .CreateNewProfile()
               .PopulateProfileNameAndClickNext("FirstCreate" + DateTime.Now.ToUniversalTime())
               .ChooseProfileLocationAndCLickNext(Locations.Stockholm)
               .ChooseProfileRoleTagsClickCreate(RoleTags.Drummer);
             double actualResults = ListProfilesViewPage
                //.readNumberofMyProfiles()
                .NumberOfCreatedProfiles();

            Assert.AreEqual(expectedResults+1, actualResults, "Newly Created profile is not present");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }
    }
}