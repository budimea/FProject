using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class TrackListPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".qa-kit-bar-creator")]
        private IWebElement _createTracksButton;

        [FindsBy(How = How.ClassName, Using = "kit-form")]
        private IWebElement _tracksContainer;

        [FindsBy(How = How.ClassName, Using = "actions-bar")]
        private IWebElement _buttonPlaceholder;
        
        public TrackListPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public AddTracksPage ClickToAddTracks()
        {
            _createTracksButton.Click();
            Thread.Sleep(2000);
            return new AddTracksPage(this._driver);
        }

        public int TotalNumberOfTracksToBeCreated()
        {
            var totalNumberOfTracks = GetAllTracks();
            return totalNumberOfTracks.Count;
        }
        public TrackListPage SaveTracks()
        {
            var buttons = _buttonPlaceholder.FindElements(By.TagName("button"));
            buttons[1].Click();
            Thread.Sleep(3000);
            return new TrackListPage(this._driver);
        }

        public ProjectTrackMetadataPage AccessTrackMetadata(string trackName)
        {
            var listOfAllTracks = GetAllTracks();
            foreach (var element in listOfAllTracks)
            {
                var title = element.FindElement(By.ClassName("kit-bar-track-content-track-title"));
                if (title.Text == trackName)
                {
                    title.Click();
                    break;
                }
            }
            return new ProjectTrackMetadataPage(this._driver);
        }
        
        private List<IWebElement> GetAllTracks()
        {
            var totalNumberOfTracks = _tracksContainer.FindElements(By.ClassName("kit-bar")).ToList();
            totalNumberOfTracks.RemoveAt(0);
            return totalNumberOfTracks;
        }
    }
}