using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    public class LAjitLinkButton:System.Web.UI.WebControls.LinkButton
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = "LajitLinkButton";
            base.Render(writer);
        }
    }
}
