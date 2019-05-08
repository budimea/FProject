using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class ProfileInlineCreationPage
    {
        private IWebDriver _driver;
        
        [FindsBy(How = How.CssSelector, 
            Using =".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthFORM_770.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;

        public ProfileInlineCreationPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void createProfileWithFollowingData(string profileName, string location)
        {
            FindAndPopulateProfileName(profileName, location);
        }

        private void FindAndPopulateProfileName(string profileName, string location)
        {
            var profileNameElement = FindElements();
            profileNameElement.RemoveAllCharactersFromInputField(profileName);
            profileNameElement.SendKeys(profileName);
            var nextButton = FindCreateButton();
            nextButton.Click();
            Thread.Sleep(1500);
            var profileLocationElement = FindElements();
            _driver.SelectFromInputDropDown(profileLocationElement,location);
            Thread.Sleep(500);
            nextButton = FindCreateButton();
            nextButton.Click();
        }

        private IWebElement FindElements()
        {
            var profileNameContainer = _elementContainer.FindElement(By.CssSelector(".kit-form"));
            var profileNameElement = profileNameContainer.FindElement(By.CssSelector("input[type='text']"));
            return profileNameElement;
        }

        private IWebElement FindCreateButton() =>_elementContainer.FindElement(By.CssSelector("button[type='submit']"));

        private IWebElement FindBackButton() => _elementContainer.FindElement(By.CssSelector("button[type='button']"));
    }
}