using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class SuggestedMatchingResults : IEquatable<SuggestedMatchingResults>
    {
        private IWebElement _image;
        private IWebElement _profileName;
        private IWebElement _profileLocation;
        private IWebElement _goToProfile;


        //TODO refactor this so it is not duplicating again... understadn first this
        public IWebElement Image { get => _image; set => _image = value; }
        public IWebElement ProfileName { get => _profileName; set => _profileName = value; }
        public IWebElement ProfileLocation { get => _profileLocation; set => _profileLocation = value; }
        public IWebElement GoToProfile { get => _goToProfile; set => _goToProfile = value; }

        public SuggestedMatchingResults(IWebElement profileName, IWebElement profileLocation, IWebElement goToProfile)
        {
            _profileName= profileName;
            _profileLocation= profileLocation;
            _goToProfile=goToProfile;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SuggestedMatchingResults);
        }

        public bool Equals(SuggestedMatchingResults other)
        {
            return other != null &&
                   EqualityComparer<IWebElement>.Default.Equals(ProfileName, other.ProfileName) &&
                   EqualityComparer<IWebElement>.Default.Equals(ProfileLocation, other.ProfileLocation);
        }
    }
}
