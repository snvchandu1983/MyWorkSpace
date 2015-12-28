using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using LAjit_BO;
using System.Web.UI.HtmlControls;

namespace LAjitDev.Common
{
    public partial class DetailedView : System.Web.UI.Page
    {
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();
        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            
            AddCSSReferences();

            if (!Page.IsPostBack)
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(Classes.AjaxMethods));
                Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
                ctl00_hdnThemeName.Value = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
                string bpInfo = Convert.ToString(HttpContext.Current.Session["LinkBPinfo"]);

                CommonBO objBO = new CommonBO();
                string returnXML = objBO.GetDataForCPGV1(GetRequestXML(bpInfo));
                string defaultPageSize = "20";

                //Get the Find BPGID and set the value into an hidden variable which will be used later for Quick Search.
                XmlDocument xDocOut = new XmlDocument();
                xDocOut.LoadXml(returnXML);
                XmlNode xNodeBPC = xDocOut.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                string BPCXML = string.Empty;
                if (xNodeBPC != null)
                {
                    BPCXML = xNodeBPC.OuterXml;
                }
                hdnFindBPGID.Value = ucGridView.GetBPCBPGID("Find", BPCXML);

                //Initialising the corresponding user control in the pop-up panel.
                ucGridView.DefaultPageSize = defaultPageSize;
                ucGridView.GridViewBPInfo = bpInfo;
                ucGridView.GridViewInitData = returnXML;
                ucGridView.DataBind();

                string headerTitle = ucGridView.GridTitle;
                //Rename the close hyperlink in the Popup frame according to the header text.s
                string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + "');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ChangeCloseText", changeCloseJS, true);
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
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

            //jquery.qtip.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery.qtip.js");
            Page.Header.Controls.Add(hgcScript2);

            //jquery.dialog.js
            HtmlGenericControl hgcScript3 = new HtmlGenericControl();
            hgcScript3.TagName = "script";
            hgcScript3.Attributes.Add("type", "text/javascript");
            hgcScript3.Attributes.Add("language", "javascript");
            hgcScript3.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery.dialog.js");
            Page.Header.Controls.Add(hgcScript3);

            //Utility.js
            HtmlGenericControl hgcScript4 = new HtmlGenericControl();
            hgcScript4.TagName = "script";
            hgcScript4.Attributes.Add("type", "text/javascript");
            hgcScript4.Attributes.Add("language", "javascript");
            hgcScript4.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Utility.js");
            Page.Header.Controls.Add(hgcScript4);


            //Common.js
            HtmlGenericControl hgcScript5 = new HtmlGenericControl();
            hgcScript5.TagName = "script";
            hgcScript5.Attributes.Add("type", "text/javascript");
            hgcScript5.Attributes.Add("language", "javascript");
            hgcScript5.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Common.js");
            Page.Header.Controls.Add(hgcScript5);

            //GridViewUserControl.js
            HtmlGenericControl hgcScript6 = new HtmlGenericControl();
            hgcScript6.TagName = "script";
            hgcScript6.Attributes.Add("type", "text/javascript");
            hgcScript6.Attributes.Add("language", "javascript");
            hgcScript6.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "GridViewUserControl.js");
            Page.Header.Controls.Add(hgcScript6);

            //FrameManager.js
            HtmlGenericControl hgcScript7 = new HtmlGenericControl();
            hgcScript7.TagName = "script";
            hgcScript7.Attributes.Add("type", "text/javascript");
            hgcScript7.Attributes.Add("language", "javascript");
            hgcScript7.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "FrameManager.js");
            Page.Header.Controls.Add(hgcScript7);
        }

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink hlCDNCss = new HtmlLink();
            hlCDNCss.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            hlCDNCss.Attributes["rel"] = "stylesheet";
            hlCDNCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlCDNCss);
        }



        /// <summary>
        /// Returns the string request XML for the passed bpInfo node XML.
        /// </summary>
        /// <param name="bpInfo">The bpInfo node XML string.</param>
        /// <returns></returns>
        public string GetRequestXML(string bpInfo)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

            if (bpInfo.Length > 0)
            {
                return "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + bpInfo + "</Root>";
            }
            return string.Empty;
        }

        protected override void OnPreInit(EventArgs e)
        {
            Page.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
            base.OnPreInit(e);
        }

        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        private string GenerateGVRequestXML(string BPGID, string pageSize)
        {
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            // nodeRoot.InnerXml = Session["BPE"].ToString();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            nodeRoot.InnerXml = XDocUserInfo.InnerXml;

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeRoot.OuterXml;
        }

        /// <summary>
        /// Generic method to be used for requesting data for all the GridViews in the current page.
        /// </summary>
        /// <param name="BPGID">The BPGID to request for.</param>
        /// <returns>Return XML.</returns>
        private string GenerateGVBPEInfo(string BPGID)
        {
            XmlDocument xDocGV = new XmlDocument();

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);

            //string formInfo = Session["PriorFormInfo"].ToString();
            //nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            //No paging stuff is required when requesting data for the first page.
            //Page size will be same as the Default page size in the User Preferences.
            //nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeBPInfo.OuterXml;
        }
    }
}
