using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    [ToolboxData(@"<{0}:LAjitRegularExpressionValidator runat=""server""></{0}:LAjitRegularExpressionValidator>")]
    public class LAjitRegularExpressionValidator : System.Web.UI.WebControls.RegularExpressionValidator
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
            base.Render(writer);
        }
    }
}
