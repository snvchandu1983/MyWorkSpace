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
using System.Xml;
using LAjit_BO;
using System.Drawing;
using System.Text;


namespace LAjitDev
{
    public partial class PopUp : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            AddCSSReferences();
            if (!Page.IsPostBack)
            {
                hdnThemeName.Value = Convert.ToString(Session["MyTheme"]);
                hdnImagesCDNPath.Value = Convert.ToString(Application["ImagesCDNPath"]);
            }
        }

        private void AddScriptReferences()
        {

            //CDN Added Scripts

            //jquery-1.3.2.min.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-1.3.2.min.js");
            Page.Header.Controls.Add(hgcScript1);

            //jquery-ui-1.7.2.custom.min.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-ui-1.7.2.custom.min.js");
            Page.Header.Controls.Add(hgcScript2);

            //ljq.js
            HtmlGenericControl hgcScript4 = new HtmlGenericControl();
            hgcScript4.TagName = "script";
            hgcScript4.Attributes.Add("type", "text/javascript");
            hgcScript4.Attributes.Add("language", "javascript");
            hgcScript4.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ljq.js");
            Page.Header.Controls.Add(hgcScript4);
       
            //ParentChild.js or JqScrips.js
            HtmlGenericControl hgcScript = new HtmlGenericControl();
            hgcScript.TagName = "script";
            hgcScript.Attributes.Add("type", "text/javascript");
            hgcScript.Attributes.Add("language", "javascript");
            if (this.Page.ToString().Contains("genprocessengine_aspx") || this.Page.ToString().Contains("genview_aspx"))
            {
                //hgcScript.Attributes.Add("src", "../JavaScript/JqScripts.js");
                hgcScript.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "JqScripts.js");
            }
            else
            {
                //hgcScript.Attributes.Add("src", "../JavaScript/ParentChild.js");
                hgcScript.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ParentChild.js");
               
            }
            Page.Header.Controls.Add(hgcScript);
            
        }

        private void AddCSSReferences()
        {
            //jquery-ui-1.7.2.custom.css
            HtmlLink css = new HtmlLink();
            css.Href = Application["ImagesCDNPath"].ToString() + "jquery-ui-1.7.2.custom.css";
            css.Attributes["rel"] = "stylesheet";
            css.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css);


            //LajitCDN.css
            HtmlLink css1 = new HtmlLink();
            css1.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            css1.Attributes["rel"] = "stylesheet";
            css1.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css1);

            //ui.jqgrid.css
            HtmlLink hljqgridCss = new HtmlLink();
            hljqgridCss.Href = Application["ImagesCDNPath"].ToString() + "ui.jqgrid.css";
            hljqgridCss.Attributes["rel"] = "stylesheet";
            hljqgridCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hljqgridCss);

            //ui.multiselect.css
            HtmlLink hlMultiSelectCss = new HtmlLink();
            hlMultiSelectCss.Href = Application["ImagesCDNPath"].ToString() + "ui.multiselect.css";
            hlMultiSelectCss.Attributes["rel"] = "stylesheet";
            hlMultiSelectCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlMultiSelectCss);
        }

        protected void topLeftScriptMngr_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            if (e.Exception.Data["ExtraInfo"] != null)
            {
                topLeftScriptMngr.AsyncPostBackErrorMessage =
                    e.Exception.Message +
                    e.Exception.Data["ExtraInfo"].ToString();
            }
            else
            {
                topLeftScriptMngr.AsyncPostBackErrorMessage =
                    "An unspecified error occurred.";
            }

        }


        protected void lnkBtnCloseIFrame_Click(object sender, EventArgs e)
        {
            UserControls.ButtonsUserControl BtnsUC = (UserControls.ButtonsUserControl)cphPageContents.FindControl("BtnsUC");
            BtnsUC.OnCloseIframeClick(sender);
        }
    }


}
