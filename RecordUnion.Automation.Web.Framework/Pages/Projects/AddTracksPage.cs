using System.Threading;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    
    public class AddTracksPage
    {
        private IWebDriver _driver;
       

        [FindsBy(How = How.ClassName, Using = "input")]
        private IWebElement _inputText;

//        [FindsBy(How = How.CssSelector, Using = ".kit-button.button.enButtonSizeLARGE.enButtonTypePRIMARY.kit-element")]
//        private IWebElement _createTracksBtn;
        public AddTracksPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public AddTracksPage AddNumberOfTracks(string numberOfTracks)
        {
            Thread.Sleep(1500);
            _inputText.SendKeys(numberOfTracks);
            return this;
        }

        public TrackListPage ClickToCreateTracks()
        {
            var _createTracksBtn =
                _driver.FindElement(
                    By.XPath("//*[@id='root']/div[2]/div/div[3]/div/div[1]/div/div[2]/div[7]/div[1]/form/div[3]/div/button"));
            _createTracksBtn.SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            return new TrackListPage(this._driver);
        }
    }
}