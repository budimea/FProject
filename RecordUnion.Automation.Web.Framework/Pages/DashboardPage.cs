using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;
using RecordUnion.Automation.Web.Framework.Pages.Projects;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages
{
    public class DashboardPage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;
        
        [FindsBy(How = How.XPath, Using = "//*[@id='root']/div/div[3]/div[3]")]
        private IWebElement _HeaderNavigationMenu;


        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            wait=new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Timeouts().PageLoad=TimeSpan.FromSeconds(10);
            PageFactory.InitElements(_driver, this);
        }


        private IList<IWebElement> ReturnHeaderNavigationItems()
        {
            Thread.Sleep(10000);
            IList<IWebElement> listHeaderElements=_HeaderNavigationMenu.FindElements(By.CssSelector(".kit-nav"));
            return listHeaderElements;
        }

        public ListProfilesViewPage GoToProfilesScreen()
        {   
            IWebElement btnProfiles = ReturnHeaderNavigationItems()[1];
            btnProfiles.Click();
            Console.WriteLine("I am on Profile page now");
            return new ListProfilesViewPage(_driver);
        }

        public ListReleasesViewPage GoToProjectScreen()
        {
            IWebElement btnReleases = ReturnHeaderNavigationItems()[2];
            btnReleases.Click();
            return new ListReleasesViewPage(_driver);
        }

        public bool verifyEmailBannerIsPresent()
        {
            if (this._driver.IsElementPresent(By.CssSelector(".kit-alert.done")))
                return true;
            else return false;
            
        }
    }

}