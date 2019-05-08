using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RecordUnion.Automation.Web.Framework.Constants;

namespace RecordUnion.Automation.Web.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
           
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(EnvironmentVariables.QaLogin);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            IWebElement inputEmailLogin = driver.FindElement(By.CssSelector("input[type='email']"));
            inputEmailLogin.SendKeys(EnvironmentVariables.QaLoginEmail);
            IWebElement inputEmailPassword = driver.FindElement(By.CssSelector("input[type='password']"));
            inputEmailPassword.SendKeys(EnvironmentVariables.QaLoginPassword);
            for(int i=0; i<EnvironmentVariables.QaLoginPassword.Length;i++)
            inputEmailPassword.SendKeys(Keys.Backspace);

            IWebElement btnLogin = driver.FindElement(By.CssSelector("button[type='submit']"));
            
            btnLogin.Click();
            
            //Thread.Sleep(10000);
            IWebElement headerBarMenu = driver.FindElement(By.XPath("//*[@id='root']/div/div[3]/div[3]/div[5]"));
            IList<IWebElement> hedermenuItest = headerBarMenu.FindElements(By.CssSelector(".kit-nav"));
            hedermenuItest[0].Click();
            //Thread.Sleep(10000);
            hedermenuItest[1].Click();
            
            
            driver.Close();
            driver.Quit();
        }
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }
}