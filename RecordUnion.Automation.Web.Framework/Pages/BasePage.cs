using System;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver _driver { get; }

        protected BasePage(IWebDriver driver) => _driver = driver;


        public class Page
        {
            private IWebDriver driver;
            private TimeSpan defaultTimeSpan;

            public Page(IWebDriver driver, TimeSpan defaultTimeSpan)
            {
                this.driver = driver;
                this.defaultTimeSpan = defaultTimeSpan;
            }
        }
    }
}
