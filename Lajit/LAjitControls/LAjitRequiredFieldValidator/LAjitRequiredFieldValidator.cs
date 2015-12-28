using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    [ToolboxData(@"<{0}:LAjitRequiredFieldValidator runat=""server""></{0}:LAjitRequiredFieldValidator>")]
    public class LAjitRequiredFieldValidator : System.Web.UI.WebControls.RequiredFieldValidator
    {
        private string m_MapXML;
        private string m_MapBranchNode;

        public string MapXML
        {
            get { return m_MapXML; }
            set { m_MapXML = value; }
        }

        public string MapBranchNode
        {
            get { return m_MapBranchNode; }
            set { m_MapBranchNode = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = "LajitControls";
            //this.ErrorMessage = "*";
            this.Display = ValidatorDisplay.None;
            base.Render(writer);
        }
    }
}
