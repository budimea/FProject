using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Pages.Profiles;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class SubMenu
    {
        private readonly IWebDriver _driver;

        private IList<IWebElement> _listOfSubMenuItems=new List<IWebElement>();

        [FindsBy(How = How.ClassName, Using = "kit-tabs")]
        private IWebElement _tabContainer;

        public SubMenu(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(driver, this);
        }

        private void FindAllSubMenuItems()
        {
            _listOfSubMenuItems = readSubMenuItems();
        }

        private IList<IWebElement> readSubMenuItems() => _tabContainer.FindElements(By.CssSelector(".kit-tabs-tab"));
 
        //TODO 1. Refactor this thing ASAP
        public ProfileMembersPage NavitateToSpecificSubPage(string subPage)
        {
            FindAllSubMenuItems();
            IWebElement sub = _listOfSubMenuItems.First(e => e.Text == subPage);
            sub.Click();
            return new ProfileMembersPage(this._driver);
        }

    }
}