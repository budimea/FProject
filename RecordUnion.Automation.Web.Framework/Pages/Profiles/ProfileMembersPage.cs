using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Profiles
{
    public class ProfileMembersPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".kit-grid")]
        private IWebElement _kitCardGrid;

        [FindsBy(How = How.CssSelector, Using = ".kit-card.kit-card-add")]
        private IWebElement _kitCardInviteMember;

        private IList<MemberCard> _listMemberCards;

        public ProfileMembersPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public ProfileMembersPage readCurrentMembers()
        {
            FindAllMembersCards();
            return this;
        }

        private void FindAllMembersCards()
        {
            _listMemberCards = new List<MemberCard>();
            var listOfMembers = new List<IWebElement>(_kitCardGrid.FindElements(By.CssSelector(".kit-card.kit-element.isClickable")));
            listOfMembers.RemoveAt(0);
            foreach (IWebElement elem in listOfMembers)
            {
                var Role = elem.FindElement(By.CssSelector(".kit-card-tag"));
                var ProfileMemberName = elem.FindElement(By.CssSelector(".kit-card-title"));
                var ProfileMemberRoleTags = elem.FindElements(By.CssSelector(".kit-tags"));
                var btnViewProfileMember = elem.FindElement(By.TagName("button"));
                _listMemberCards.Add(new MemberCard(Role, ProfileMemberRoleTags,ProfileMemberName, btnViewProfileMember));
            }
        }

        private IList<MemberCard> findListOfAdmis()=>_listMemberCards.Where(elem => elem.MemberRole.Text == "Admin").ToList();

        public MemberCard findCurrentCreator()
        {
            MemberCard CurrentCreator;
            IList<MemberCard> ProfileAdmins = findListOfAdmis();
            return ProfileAdmins.Where(elem => elem.MemberName.Text.Contains("(you)")).Single();
        }

        public ProfileMemberInvitationPage clickToInviteNewMember()
        {
            _kitCardInviteMember.Click();
            return new ProfileMemberInvitationPage(this._driver);
        }

        public int CurrentNumberOfProfileMembersCards()=> _listMemberCards.Count();
    }
}