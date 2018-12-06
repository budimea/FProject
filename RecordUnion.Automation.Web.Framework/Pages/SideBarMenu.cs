using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages
{
    public class SideBarMenu
    {
        public SideBarMenu(IWebDriver driver)
        {
            //_driver = driver;
            PageFactory.InitElements(driver, this);
        }
        
        [FindsBy(How=How.CssSelector , Using=".typography-c2")]
        private IWebElement elemAvatar;

        public SideBarMenu ExpandTheNavigationMenu()
        {
            elemAvatar.Click();
            return this;
        }

        public string GetLoggedInUser()
        {
            //toDo I need to finish the avatar in order to assert this.
            ExpandTheNavigationMenu();
            //var extendedSideBar = _driver.FindElement(By.ClassName("component expanded"));
            //var logInName=extendedSideBar.
            //return find
            return "I will find the avatar";
        }        
        
        //toDo  I need to finish the side bar elements and concepts ... this needs to be implemented as part of everypage in a way... it should not have the driver 
    }
}