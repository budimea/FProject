using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.UtilHelper
{
    public static class HelperSelection
    {
        
        public static void SelectFromInputDropDown(this IWebDriver driver, IWebElement inputElement, String seleciton)
        {
            inputElement.SendKeys(seleciton.Split(',')[0]);
            Thread.Sleep(5000);
            IWebElement parentContainer = driver.FindElement(By.ClassName("rows"));
            IList<IWebElement> searchResults = driver.FindElements(By.ClassName("row"));
            var matchingResultSet = searchResults.FirstOrDefault(e => e.Text == seleciton);

            if (matchingResultSet == null)
                throw new Exception();

            matchingResultSet.Click();
        }
        
        public static IList<IWebElement> ReturnHeaderNavigationItems(this IWebDriver driver)
        {
            Thread.Sleep(10000);
            IWebElement _HeaderNavigationMenu = driver.FindElement(By.XPath("//*[@id='root']/div/div[3]/div[3]"));
            IList<IWebElement> listHeaderElements=_HeaderNavigationMenu.FindElements(By.CssSelector(".kit-nav"));
            return listHeaderElements;
        }

        public static void SelectFromStaticDropDown(this IWebElement element, string selection)
        {
            
            IList<IWebElement> listOfAllElements = element.FindElements(By.ClassName("row"));
            var matchingResult = listOfAllElements.FirstOrDefault(e => e.Text == selection);
            matchingResult.Click();
        }
        
        public static void SelectFromResultsByMatchingStart(this IWebElement element, string selection)
        {

            IList<IWebElement> listOfAllElements =
                element.FindElements(By.ClassName("row")).Skip(1).Select(e =>
                    e.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-B3.suggestion.kit-element"))).ToList();
            //IList<IWebElement> listOfSubElements=listOfAllElements.Select(e =>
                //e.FindElement(By.CssSelector(".kit-typography.enTypographyTypes-B3.suggestion.kit-element"))).ToList();
            var matchingResult = listOfAllElements.FirstOrDefault(e => e.Text.Contains(selection));
            matchingResult.Click();
        }
    }
}
