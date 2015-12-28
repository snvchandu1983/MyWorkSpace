using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    [ToolboxData(@"<{0}:LAjitDropDownList runat=""server""></{0}:LAjitDropDownList>")]
    public class LAjitDropDownList : System.Web.UI.WebControls.DropDownList
    {
        private string m_MapXML;
        private string m_MapBranchNode;
        private string m_MapPreviousSelItem;
        private RequiredFieldValidator m_rfvDropDown;
        private string m_ValidationErrorMessage;
        private string m_ValidationToolTip;
        private string m_ValidationText;
        private bool m_ValidationEnabled;
        private CustomValidator m_cvDropDown;

        public bool ValidationEnabled
        {
            get { return m_ValidationEnabled; }
            set { m_ValidationEnabled = value; }
        }

        public string ValidationText
        {
            get { return m_ValidationText; }
            set { m_ValidationText = value; }
        }

        public string ValidationToolTip
        {
            get { return m_ValidationToolTip; }
            set { m_ValidationToolTip = value; }
        }

        public string ValidationErrorMessage
        {
            get { return m_ValidationErrorMessage; }
            set { m_ValidationErrorMessage = value; }
        }

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

        public string MapPreviousSelItem
        {
            get { return m_MapPreviousSelItem; }
            set { m_MapPreviousSelItem = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            m_rfvDropDown = new RequiredFieldValidator();
            m_cvDropDown = new CustomValidator();
            if (m_ValidationEnabled == false)
            {
                m_rfvDropDown.Enabled = false;
            }
            m_rfvDropDown.ControlToValidate = this.ID;
            m_rfvDropDown.ErrorMessage = this.m_ValidationErrorMessage;
            m_rfvDropDown.ToolTip = this.m_ValidationToolTip;
            m_rfvDropDown.Text = this.m_ValidationText;
            //this.Controls.Add(m_rfvDropDown);
            //Controls.Add(m_rfvDropDown);
            m_cvDropDown.ServerValidate += new ServerValidateEventHandler(m_cvDropDown_ServerValidate);
        }

        void m_cvDropDown_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value == "Selection Required")
            {
                args.IsValid = false;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = "LajitDropDown";
            base.Render(writer);
        }
    }
}
