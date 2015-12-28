using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LAjitDev.Classes;

namespace LAjitDev.Common
{
    public partial class SessionExpire : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddCSSReferences();
            //Make the the sessions are terminated in case this point as a result of redirect from javascript code.
            Session.Abandon();
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Common/Login.aspx", true);
        }

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink hlCDNCss = new HtmlLink();
            hlCDNCss.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            hlCDNCss.Attributes["rel"] = "stylesheet";
            hlCDNCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlCDNCss);
           
            //shortcut icon favicon.ico
            HtmlLink hlShortCutIcon = new HtmlLink();
            hlShortCutIcon.Href = Application["ImagesCDNPath"].ToString() + "Images/favicon.ico";
            hlShortCutIcon.Attributes["rel"] = "shortcut icon";
            hlShortCutIcon.Attributes["type"] = "image/x-icon";
            Page.Header.Controls.Add(hlShortCutIcon);
        }
    }
}
