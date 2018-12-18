using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class ReleaseInputBatchComponent : IEquatable<ReleaseInputBatchComponent>
    {
        private List<ValidatedInputFields> _inputFields;
        private IWebElement _toggle;
        private IWebElement _submit;
        private IWebElement _cancel;
        
        public List<ValidatedInputFields> InputFields { get => _inputFields; set => _inputFields = value; }
        public IWebElement Toggle { get => _toggle; set => _toggle = value; }        
        public IWebElement Submit { get => _submit; set => _submit = value; }
        public IWebElement Cancel { get => _cancel; set => _cancel = value; }

        public ReleaseInputBatchComponent(List<ValidatedInputFields> inputFields, IWebElement toggle, IWebElement submit, IWebElement cancel)
        {
            _inputFields = inputFields;
            _toggle = toggle;
            _submit = submit;
            _cancel = cancel;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ReleaseInputBatchComponent);
        }

        public bool Equals(ReleaseInputBatchComponent other)
        {
            return other != null &&
                   EqualityComparer<List<ValidatedInputFields>>.Default.Equals(_inputFields, other._inputFields) &&
                   EqualityComparer<IWebElement>.Default.Equals(_toggle, other._toggle) &&
                   EqualityComparer<IWebElement>.Default.Equals(_submit, other._submit) &&
                   EqualityComparer<IWebElement>.Default.Equals(_cancel, other._cancel);
        }
    }
}
