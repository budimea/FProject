using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;

namespace RecordUnion.Automation.Web.Framework.UtilHelper
{
    public static class WebElementExtensions
    {
        public static bool IsEnabled(this IWebElement element)
            => element.Enabled;

        public static bool IsElementPresent(this IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static List<IWebElement> ReturnWarnings<T>(this IWebElement elem, String validationType)
        {
            Thread.Sleep(2500);
            var Messages = new List<IWebElement>();
            var tmp = new List<IWebElement>();
            if (validationType == "Hard")
            {
                Messages = elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-RASPBERRY_-2")).ToList();
            }
            else Messages = elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-MANGO_-2")).ToList();
            foreach (IWebElement element in Messages)
                tmp.Add(element.FindElement(By.XPath("..")));
            return tmp;
        }

        public static List<ValidatedInputFields> FindInputFieldsGeneric(this IWebElement element)
        {
            var _projectInputFields = new List<ValidatedInputFields>();
            IList<IWebElement> Inputs = element.FindElements(By.CssSelector(".kit-input"));
            foreach (IWebElement elem in Inputs)
            {
                IWebElement Input = elem.FindElement(By.ClassName("input"));
                IWebElement Validations = elem.FindElement(By.CssSelector(".kit-list"));
                _projectInputFields.Add(new ValidatedInputFields(Input, Validations));
            }
            return _projectInputFields;
        }
        
        public static List<IWebElement> ReturnWarnings(this IWebElement elem, String validationType)
        {
            Thread.Sleep(2500);
            var Messages = new List<IWebElement>();
            var tmp = new List<IWebElement>();
            if (validationType == "Hard")
            {
                Messages= elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-RASPBERRY_-2")).ToList();
            }
            else Messages=elem.FindElements(By.CssSelector(".kit-tag.inline.kit-element.enBackgroundColor-MANGO_-2")).ToList();
            foreach (IWebElement element in Messages)
                tmp.Add(element.FindElement(By.XPath("..")));
            return tmp;
        }
        
        
        

    }
}
