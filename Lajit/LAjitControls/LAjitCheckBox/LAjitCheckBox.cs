using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    [ToolboxData(@"<{0}:LAjitCheckBox runat=""server""></{0}:LAjitCheckBox>")]
    public class LAjitCheckBox : System.Web.UI.WebControls.CheckBox
    {
        private string m_MapXML;
        private string m_MapBranchNode;
        private RequiredFieldValidator m_rfvCheckBox;
        private string m_ValidationErrorMessage;
        private string m_ValidationToolTip;
        private string m_ValidationText;
        private bool m_ValidationEnabled;

      
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
            this.CssClass = "LajitCheckBox";
            base.Render(writer);
        }
    }
}
