using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Constants;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.SignUp
{
    public class SignUpPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.Name, Using = "firstname")]
        private IWebElement _firstName;

        [FindsBy(How = How.Name, Using = "lastname")]
        private IWebElement _lastName;

        [FindsBy(How = How.CssSelector, Using = ".kit-autocomplete")]
        private IWebElement _locationsContainer;

        [FindsBy(How = How.CssSelector, Using = "input[type='email']")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "input[type='password']")]
        private IWebElement _inputPassword;

        [FindsBy(How = How.CssSelector, Using = ".kit-checkbox.interactive")]
        private IWebElement _termsAndConditions;

        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        private IWebElement _buttonSignUp;

        public SignUpPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public DashboardPage SuccessfullSignUpUnVerified(string Name, string Surname, string Email)
        {
            _firstName.SendKeys(Name);
            _lastName.SendKeys(Surname);
            IWebElement locationsInput = FindLocationsInput();
            this._driver.SelectFromInputDropDown(locationsInput, Locations.Stockholm);
            _email.SendKeys(Email);
            _inputPassword.SendKeys(EnvironmentVariables.QASignUpPassword);
            _termsAndConditions.Click();
            _buttonSignUp.Click();
            return new DashboardPage(this._driver);
        }

        private IWebElement FindLocationsInput() => _locationsContainer.FindElement(By.TagName("input"));
    }
}
