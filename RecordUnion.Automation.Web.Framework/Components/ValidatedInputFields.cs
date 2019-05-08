using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class ValidatedInputFields : IEquatable<ValidatedInputFields>
    {
        private IWebElement _inputField;
        private IWebElement _listOfErrorMessages;

        public ValidatedInputFields(IWebElement inputField, IWebElement listOfErrorMessages)
        {
            InputField = inputField;
            ListOfErrorMessages = listOfErrorMessages;
        }

        public IWebElement InputField { get => _inputField; set => _inputField = value; }
        public IWebElement ListOfErrorMessages { get => _listOfErrorMessages; set => _listOfErrorMessages = value; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ValidatedInputFields);
        }

        public bool Equals(ValidatedInputFields other)
        {
            return other != null &&
                   EqualityComparer<IWebElement>.Default.Equals(InputField, other.InputField) &&
                   EqualityComparer<IWebElement>.Default.Equals(ListOfErrorMessages, other.ListOfErrorMessages);
        }
    }
}
