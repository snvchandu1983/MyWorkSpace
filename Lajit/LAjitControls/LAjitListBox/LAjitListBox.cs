using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    public enum XMLType
    {
        ParentOnly,
        ParentChild,
        HybridParent
    }

    public class LAjitListBox : ListBox
    {
        private string m_MapXML;
        private XMLType m_XMLType;
        private string m_MapBranchNode;

        public XMLType XMLType
        {
            get { return m_XMLType; }
            set { m_XMLType = value; }
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

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = "LajitListBox";
            base.Render(writer);
        }
    }
}
