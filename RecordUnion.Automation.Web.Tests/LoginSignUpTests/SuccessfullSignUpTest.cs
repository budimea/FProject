using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.Pages;
using RecordUnion.Automation.Web.Framework.Pages.SignUp;

namespace RecordUnion.Automation.Web.Tests.LoginSignUpTests
{
    class SuccessfullSignUpTest
    {
        private static IWebDriver driver;
        private SignUpPage signUpPage;
        private DashboardPage dashboardPage;

        private static Random signUpEmail=new Random();
        private static Random signUpName=new Random();
        private static Random signUpSurname = new Random();

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Navigate().GoToUrl(EnvironmentVariables.QaSignUpEmailUrl);
            signUpPage = new SignUpPage(driver);
            dashboardPage = signUpPage.SuccessfullSignUpUnVerified(RandomString(signUpName), RandomString(signUpSurname), RandomString(signUpEmail)+"@test.com");
        }

        [Test, Order(1), Description("User has successfully Signed Up")]
        public void SignUpUsingEmailSuccessfully()
        {
            bool actualResults=dashboardPage.verifyEmailBannerIsPresent();
            //Add reading for the signUp name and surname
            Assert.IsTrue(actualResults, "Banner is not present when user is not verified.");
        }

        public string RandomString(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).
                Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
