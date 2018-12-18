using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class EditReleaseMetadataDropDownComponent
    {
        public IWebElement ClearButton { get; set; }

        public IWebElement RootDropDownElement { get; set; }

        public IWebElement InputDropDown { get; set; }

        public IWebElement DropDownExpander { get; set; }
        
        public IWebElement ListOfValidationMessages { get; set; }


        public EditReleaseMetadataDropDownComponent(IWebElement rootDropDownElement, IWebElement inputDropDown,
            IWebElement dropDownExpander, IWebElement clearButton, IWebElement listOfValidationMessages)
        {
            RootDropDownElement = rootDropDownElement;
            InputDropDown = inputDropDown;
            DropDownExpander = dropDownExpander;
            ClearButton=clearButton;
            ListOfValidationMessages = listOfValidationMessages;
        }

            public bool Equals(EditReleaseMetadataDropDownComponent x, EditReleaseMetadataDropDownComponent y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.ClearButton, y.ClearButton) && Equals(x.RootDropDownElement, y.RootDropDownElement) && Equals(x.InputDropDown, y.InputDropDown) && Equals(x.DropDownExpander, y.DropDownExpander)&& Equals(x.ListOfValidationMessages, y.ListOfValidationMessages);
            }
    }
}