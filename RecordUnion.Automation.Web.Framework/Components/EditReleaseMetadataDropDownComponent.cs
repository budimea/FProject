using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class EditReleaseMetadataDropDownComponent
    {
        private IWebElement _rootDropDownElement;
        private IWebElement _inputDropDown;
        private IWebElement _dropDownExpander;

        public IWebElement RootDropDownElement
        {
            get => _rootDropDownElement;
            set => _rootDropDownElement = value;
        }

        public IWebElement InputDropDown
        {
            get => _inputDropDown;
            set => _inputDropDown = value;
        }

        public IWebElement DropDownExpander
        {
            get => _dropDownExpander;
            set => _dropDownExpander = value;
        }


        public EditReleaseMetadataDropDownComponent(IWebElement rootDropDownElement, IWebElement inputDropDown,
            IWebElement dropDownExpander)
        {
            _rootDropDownElement = rootDropDownElement;
            _inputDropDown = inputDropDown;
            _dropDownExpander = dropDownExpander;
        }

            public bool Equals(EditReleaseMetadataDropDownComponent x, EditReleaseMetadataDropDownComponent y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x._inputDropDown, y._inputDropDown) && Equals(x._dropDownExpander, y._dropDownExpander);
            }
    }
}