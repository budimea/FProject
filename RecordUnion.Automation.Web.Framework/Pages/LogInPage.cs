using System.Threading;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages
{
    public class LogInPage
    {
        private IWebDriver _driver;

        public LogInPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input[type='email']")]
        private IWebElement inputEmail;

        [FindsBy(How = How.CssSelector, Using = "input[type='password']")]
        private IWebElement inputPassword;

        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        private IWebElement btnLoginEmail;

        public DashboardPage SuccessFullLogin(string username, string password)
        {
            
            Thread.Sleep(10000);
            inputEmail.SendKeys(username);
            inputPassword.SendKeys(password);
            btnLoginEmail.Click();
            return new DashboardPage(_driver);
        }
    }
}