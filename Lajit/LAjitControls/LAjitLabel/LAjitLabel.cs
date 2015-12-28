using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{

    [ToolboxData(@"<{0}:LAjitLabel runat=""server""></{0}:LAjitLabel>")]
    public class LAjitLabel : System.Web.UI.WebControls.Label
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
            /*if (this.CssClass == string.Empty)
            {
                this.CssClass = "LajitLabel";
            }
            else if (this.CssClass == "mbodybig")
            {
                this.BackColor = System.Drawing.Color.Transparent;
            }*/
            this.CssClass = "mbodybig";
            this.BackColor = System.Drawing.Color.Transparent;

            base.Render(writer);
        }
    }
}
