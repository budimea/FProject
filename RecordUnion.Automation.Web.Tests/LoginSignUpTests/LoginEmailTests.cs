using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;

namespace RecordUnion.Automation.Web.Tests.LoginSignUpTests
{
    public class LoginEmailTests
    {
        private IWebDriver driver;
        public LogInPage LogInPage;
        public DashboardPage DashboardPage;
        
        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait =TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            driver.Manage().Window.FullScreen();
            LogInPage=new LogInPage(driver);
        }

        [Test]
        public void SuccessfullLoginEmail()
        {
            DashboardPage=LogInPage.SuccessFullLogin(EnvironmentVariables.QaLoginEmail, EnvironmentVariables.QaLoginPassword);
            Assert.AreEqual(true, true);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }

    }
}