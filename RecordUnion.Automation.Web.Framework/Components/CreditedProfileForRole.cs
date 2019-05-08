using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class CreditedProfileForRole: IEquatable<CreditedProfileForRole>
    {
        private IWebElement _profileImage;
        private string _profileName;
        private string _profileCredit;
        private string _profileLocation;
        private IWebElement _editBtn;
        private IWebElement _removeBtn;

        public string ProfileName
        {
            get => _profileName;
            set => _profileName = value;
        }

        public String ProfileCredit
        {
            get => _profileCredit;
            set => _profileCredit = value;
        }

        public string ProfileLocation
        {
            get => _profileLocation;
            set => _profileLocation = value;
        }

        public IWebElement EditBtn
        {
            get => _editBtn;
            set => _editBtn = value;
        }

        public IWebElement RemoveBtn
        {
            get => _removeBtn;
            set => _removeBtn = value;
        }

        public CreditedProfileForRole(string profileName, string profileCredit, string profileLocation, IWebElement editBtn, IWebElement removeBtn)
        {
            _profileName = profileName;
            _profileCredit = profileCredit;
            _profileLocation = profileLocation;
            _editBtn = editBtn;
            _removeBtn = removeBtn;
        }
        
        public CreditedProfileForRole(string profileName, string profileCredit)
        {
            _profileName = profileName;
            _profileCredit = profileCredit;
        }

        public CreditedProfileForRole(string profileName, string profileCredit, string profileLocation)
        {
            _profileName = profileName;
            _profileCredit = profileCredit;
            _profileLocation = profileLocation;
        }

        public override string ToString()
            {
                return $"{_profileName} Release Role: {_profileCredit} Location {_profileLocation}";
            }

        public bool Equals(CreditedProfileForRole other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_profileName, other._profileName) && string.Equals(_profileCredit, other._profileCredit) && string.Equals(_profileLocation, other._profileLocation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CreditedProfileForRole) obj);
        }
    }
}