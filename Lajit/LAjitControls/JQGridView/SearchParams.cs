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
    public class SearchParams
    {

        private string m_field;

        public string Field
        {
            get { return m_field; }
            set { m_field = value; }
        }
        private string m_op;

        public string Op
        {
            get { return m_op; }
            set { m_op = value; }
        }
        private string m_data;

        public string Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

    }
}
