using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RecordUnion.Automation.Web.Framework.UtilHelper
{
    public static class Driver
    {
        public static WebDriverWait browserWait;
        private static IWebDriver browser;
        
        public static IWebDriver Browser
        {
            get
            {
                if (browser == null)
                {
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method Start.");
                }
                return browser;
            }
            private set
            {
                browser = value;
            }
        }

        public static WebDriverWait BrowserWait
        {
            get
            {
                if (browserWait == null||browser==null)
                {
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method Start.");
                }
                return browserWait;
            }
            private set
            {
                browserWait = value;
            }
        }

    }  
}