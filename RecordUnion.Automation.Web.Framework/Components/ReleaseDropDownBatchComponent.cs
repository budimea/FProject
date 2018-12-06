using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class ReleaseDropDownBatchComponent
    {
        public List<EditReleaseMetadataDropDownComponent> ListOfDropDown { get; set; }

        public IWebElement SubmitButton { get; set; }

        public IWebElement CancelButton { get; set; }

        public IList<IWebElement> ListOfValidationMessages { get; set; }

        public ReleaseDropDownBatchComponent(List<EditReleaseMetadataDropDownComponent> listOfDropDown, IWebElement submitButton, IWebElement cancelButton, IList<IWebElement> listOfValidationMessages)
        {
            ListOfDropDown = listOfDropDown;
            SubmitButton = submitButton;
            CancelButton = cancelButton;
            ListOfValidationMessages = listOfValidationMessages;
        }

            public bool Equals(ReleaseDropDownBatchComponent x, ReleaseDropDownBatchComponent y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.ListOfDropDown, y.ListOfDropDown) && Equals(x.SubmitButton, y.SubmitButton) && Equals(x.CancelButton, y.CancelButton);
            }
    }
}