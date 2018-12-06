using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class ProjectCard
    {
        private IWebElement _image;

        private IWebElement _projectName;

        private IWebElement _viewProjectButton;

        public IWebElement Image { get => _image; set => _image = value; }
        public IWebElement ProjectName { get => _projectName; set => _projectName = value; }
        public IWebElement ViewProjectButton { get => _viewProjectButton; set => _viewProjectButton = value; }

        public ProjectCard(IWebElement projectName, IWebElement viewProjectButton)
        {
            //TODO finish the implementation when image componenet has its final implementation_impage = impage;
            _projectName = projectName;
            _viewProjectButton = viewProjectButton;
        }
    }
}
