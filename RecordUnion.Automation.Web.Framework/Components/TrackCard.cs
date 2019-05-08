using OpenQA.Selenium;

namespace RecordUnion.Automation.Web.Framework.Components
{
    public class TrackCard
    {
        private string _trackTitle;
        private int _orderNumber;
        private IWebElement _deleteBtn;

        public TrackCard(string trackTitle, int orderNumber, IWebElement deleteBtn)
        {
            _trackTitle = trackTitle;
            _orderNumber = orderNumber;
            _deleteBtn = deleteBtn;
        }
        
        
    }
}