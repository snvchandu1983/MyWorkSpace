using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace LAjitControls.JQGridView
{
    public class SearchFilter
    {

        private string m_groupOp;

        public string GroupOp
        {
            get { return m_groupOp; }
            set { m_groupOp = value; }
        }

        private SearchParams[] m_rules;

        public SearchParams[] Rules
        {
            get { return m_rules; }
            set { m_rules = value; }
        }
    }
}
