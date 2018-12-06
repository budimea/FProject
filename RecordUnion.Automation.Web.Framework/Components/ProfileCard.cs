using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class ProfileCard : IEquatable<ProfileCard>
    {

        private IWebElement _image;

        private IWebElement _profileName;

        private IWebElement _viewProfileButton;

        public IWebElement Image { get => _image; set => _image = value; }
        public IWebElement ProfileName { get => _profileName; set => _profileName = value; }
        public IWebElement ViewProfileButton { get => _viewProfileButton; set => _viewProfileButton = value; }

        public ProfileCard(IWebElement profileName, IWebElement viewProfileButton)
        {
            //TODO finish the implementation when image componenet has its final implementation_impage = impage;
            _profileName = profileName;
            _viewProfileButton = viewProfileButton;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProfileCard);
        }

        public bool Equals(ProfileCard other)
        {
            return other != null &&
                   EqualityComparer<IWebElement>.Default.Equals(_profileName, other._profileName);
        }


    }
}
