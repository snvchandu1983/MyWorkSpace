using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    public class LAjitImage : System.Web.UI.WebControls.Image
    {
        private string m_MapXML;
      
        public string MapXML
        {
            get { return m_MapXML; }
            set { m_MapXML = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
