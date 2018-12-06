using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class MemberCard : IEquatable<MemberCard>
    {
        private IWebElement _memberAvatarImage;
        private IWebElement _memberRole;
        private IList<IWebElement> _roleTags;
        private IWebElement _memberName;
        private IWebElement _viewMemberButton;

        public IWebElement MemberAvatar { get => _memberAvatarImage; set =>_memberAvatarImage = value; }
        public IWebElement MemberName { get => _memberName; set => _memberName = value; }
        public IWebElement ViewMemberButton { get => _viewMemberButton; set => _viewMemberButton = value; }
        public IList<IWebElement> RoleTags{ get => _roleTags; set => _roleTags = value; }
        public IWebElement MemberRole { get => _memberRole; set => _memberRole = value; }

        public MemberCard(IWebElement MemberRole, IList<IWebElement> RoleTags, IWebElement MemberName, IWebElement ViewMemberButton)
        {
            //TODO finish the implementation when image componenet has its final implementation_impage = impage;
            _memberRole = MemberRole;
            _roleTags = RoleTags;
            _memberName = MemberName;
            _viewMemberButton = ViewMemberButton;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MemberCard);
        }

        public bool Equals(MemberCard other)
        {
            return other != null &&
                   EqualityComparer<IWebElement>.Default.Equals(_memberRole, other._memberRole) &&
                   EqualityComparer<IList<IWebElement>>.Default.Equals(_roleTags, other._roleTags) &&
                   EqualityComparer<IWebElement>.Default.Equals(_memberName, other._memberName) &&
                   EqualityComparer<IWebElement>.Default.Equals(MemberName, other.MemberName) &&
                   EqualityComparer<IList<IWebElement>>.Default.Equals(RoleTags, other.RoleTags) &&
                   EqualityComparer<IWebElement>.Default.Equals(MemberRole, other.MemberRole);
        }
    }
}
