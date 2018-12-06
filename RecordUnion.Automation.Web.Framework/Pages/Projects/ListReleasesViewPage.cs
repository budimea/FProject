using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RecordUnion.Automation.Web.Framework.Components;
using RecordUnion.Automation.Web.Framework.UtilHelper;
using SeleniumExtras.PageObjects;

namespace RecordUnion.Automation.Web.Framework.Pages.Projects
{
    public class ListReleasesViewPage
    {
        private IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".kit-grid")]
        private IWebElement _projectListGrid;

        [FindsBy(How = How.CssSelector, Using = ".kit-card.kit-card-add")]
        private IWebElement _cardCreateProject;

        private IList<ProjectCard> _listOfMyProjects;

        public ListReleasesViewPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public CreateNewProjectPage CreateNewProject()
        {
            Thread.Sleep(2000);
            _cardCreateProject.Click();
            return new CreateNewProjectPage(this._driver);
        }

        public void getAllCreateProjectPads()
        {
            _listOfMyProjects = new List<ProjectCard>();
            var listOfProjects = new List<IWebElement>(_projectListGrid.FindElements(By.CssSelector(".kit-card.kit-element.isClickable")));
            listOfProjects.RemoveAt(0);
            foreach (IWebElement elem in listOfProjects)
            {
                var projectName = elem.FindElement(By.CssSelector(".kit-card-title"));
                var btnViewProject = elem.FindElement(By.TagName("button"));
                _listOfMyProjects.Add(new ProjectCard(projectName, btnViewProject));
            }
        }

        public int NumberOfCreatedProjects()
        {
            Thread.Sleep(10000);
            getAllCreateProjectPads();
            return _listOfMyProjects.Count;
        }

        public ProjectReleaseInformationPage ClickToAccessReleaseInformation(string projectTitle)
        {
            var Search=FindProjectByName(projectTitle);
            var ProjectPad = Search.First();
            ProjectPad.ViewProjectButton.Click();
            return new ProjectReleaseInformationPage(this._driver);
        }

        public IList<ProjectCard> FindProjectByName(String ProjectTitle)
        {
            getAllCreateProjectPads();
            return FindProjectCardWithGivenProjectTitle(ProjectTitle);
        }

        private ProjectReleaseInformationPage AccessUnpublushedProject(ProjectCard UnpublishedProject)
        {
            return new ProjectReleaseInformationPage(this._driver);
        }

        private IList<ProjectCard> FindProjectCardWithGivenProjectTitle(string projectTitle)
        {
            IList<ProjectCard> matchingCard = _listOfMyProjects.Where(elem => elem.ProjectName.Text == projectTitle).ToList();
            return matchingCard;
        }

        public ListReleasesViewPage ClickToCreateReleaseUnverified()
        {
            _cardCreateProject.Click();
            Thread.Sleep(5000);
            return new ListReleasesViewPage(this._driver);
        }

        public ListReleasesViewPage CloseUnverifiedPopup()
        {
            if (UnverifiedUserPopupIsPresent())
            {
                IWebElement popUpContaier = _driver.FindElement(By.ClassName("kit-dialog-container"));
                IWebElement closeButton = popUpContaier.FindElement(By.CssSelector(".kit-svg"));
                closeButton.Click();
                return new ListReleasesViewPage(this._driver);
            }
            else throw new Exception("Unverified User popup is missing");
        }

        public bool UnverifiedUserPopupIsPresent()
        {
            if (this._driver.IsElementPresent(By.ClassName("kit-dialog-container")))
                return true;
            else return false;
        }
    }
}
