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

        /*public static List<IWebElement> ReturnWarnings<T>(this IWebElement elem, String validationType)
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
        }*/

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

        private static List<string> ReadAllTextMessagesReturnedInWarning(this IWebElement element, IList<IWebElement> Messages)
        {
            var warningTextMessages = new List<string>();
            foreach (IWebElement elem in Messages)
            {
                IWebElement message = elem.FindElement(By.ClassName("title"));
                if (message.Text.StartsWith("\""))
                {
                    string noQuotes=message.Text.Replace("\"", String.Empty);
                    warningTextMessages.Add(noQuotes);
                }
                else warningTextMessages.Add(message.Text);
            }
            return warningTextMessages;
        }
        
        public static IList<ValidatedInputFields> FindInputFields(this IWebElement fieldsContainer)
        {
            var projectInputFields = new List<ValidatedInputFields>();
            IList<IWebElement> Inputs = fieldsContainer.FindElements(By.ClassName("kit-input"));
            foreach (IWebElement elem in Inputs)
            {
                IWebElement Input = elem.FindElement(By.ClassName("input"));
                IWebElement Validations = elem.FindElement(By.CssSelector(".kit-list"));
                projectInputFields.Add(new ValidatedInputFields(Input, Validations));
            }
            return projectInputFields;
        }
        
        public static List<string> ReadAllValidationMessagesForInputField(this IWebElement fieldContainer, int field, string validationType)
        {
            var validatedInputField = fieldContainer.FindInputFields().ElementAt(field);
            IList<IWebElement> errorMessages = validatedInputField.ListOfErrorMessages.ReturnWarnings(validationType);
            return validatedInputField.InputField.ReadAllTextMessagesReturnedInWarning(errorMessages);
        }

        public static List<string> ReadAllValidationMessagesForDropDownField(this EditReleaseMetadataDropDownComponent field, string validationType)
        {
            IList<IWebElement> errorMessages = field.ListOfValidationMessages.ReturnWarnings(validationType);
            return field.InputDropDown.ReadAllTextMessagesReturnedInWarning(errorMessages);
        }

        public static void RemoveNumberOfCharactersFromInputField(this IWebElement elem, int number)
        {
            while (number > 0)
            {
                elem.SendKeys(Keys.Backspace);
                number--;
            }
        }

        public static void RemoveAllCharactersFromInputField(this IWebElement elem, string inputValue)
        {
            for(int i=0;i<inputValue.Length;i++)
                elem.SendKeys(Keys.Backspace);
        }
        
        public static IWebElement CreatorPad(this IWebElement batch)
        {
            var creatorPad = batch.FindElements(By.CssSelector(".kit-bar.isCreator"));
            return creatorPad.SingleOrDefault();
        }

    }
}
