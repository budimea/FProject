using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class FindOrCreateProfileToCredit
    {
        private IWebDriver _driver;
        
        /*[FindsBy(How = How.CssSelector, 
            Using =".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthNULL.enMaxWidthFORM_770")]
        private IWebElement _elementContainer;*/

        public FindOrCreateProfileToCredit(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public ProjectReleaseInformationPage SearchAndCreditExistingProfile(string selection)
        {
            PopulateInputFieldAndChooseExistingProfile(selection);
            ClickDone();
            return new ProjectReleaseInformationPage(this._driver);
        }

        private void ClickDone()
        {
            var _elementContainer = _driver.FindElement(By.CssSelector(
                ".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthFORM_770.enMaxWidthFORM_770"));
            var btnDone = _elementContainer.FindElement(By.CssSelector("button[type='submit']"));
            btnDone.Click();
        }

        private FindOrCreateProfileToCredit PopulateInputFieldAndChooseExistingProfile(string selection)
        {
            PopulateInputField(selection);
            ChooseFromReturnedResultSet(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private FindOrCreateProfileToCredit ChooseFromReturnedResultSet(string selection)
        {
            var _elementContainer = _driver.FindElement(By.CssSelector(
                ".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthFORM_770.enMaxWidthFORM_770"));
            var rootElement = _elementContainer.FindElement(By.ClassName("kit-suggestions"));
            rootElement.SelectFromResultsByMatchingStart(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }

        private FindOrCreateProfileToCredit PopulateInputField(string selection)
        {
            var _elementContainer = _driver.FindElement(By.CssSelector(
                ".kit-element.enDisplayFLEX.enFlexDirectionCOLUMN.enFlexAUTO.enJustifyContentCENTER.enWidthFULL.enMinWidthFORM_770.enMaxWidthFORM_770"));
            var inputFiled = _elementContainer.FindElement(By.CssSelector("input[type='text']"));
            inputFiled.SendKeys(selection);
            return new FindOrCreateProfileToCredit(this._driver);
        }
    }
}