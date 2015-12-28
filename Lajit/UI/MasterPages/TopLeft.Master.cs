using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web;
using System.Xml.XPath;
using LAjit_BO;
using NLog;

namespace LAjitDev
{
    public partial class TopLeft : System.Web.UI.MasterPage
    {
        private int m_IndexOfIsProtected;
        private int m_IndexOfIsActive;
        private int m_IndexOfTypeOfInactiveStatusID;
        private bool m_IsFirstRun = true;
        private bool m_IsRowAlternating = false;
        Label lblNewShrtCut;
        CommonBO objBO = new CommonBO();
        public CommonUI commonObjUI = new CommonUI();
        private string m_CurrentBPGID;
        private string m_CurrentPageInfo;

        Hashtable m_htBPCntrls = new Hashtable();
        string m_primaryKeyFieldName = string.Empty;
        string m_BPCXml = string.Empty;
        string m_BusinessRules = string.Empty;
        private string m_GVTreeNodeName = string.Empty;
        private const int m_NoOfGVStaticColumns = 1;
        private string m_HyperLinksEnabled = string.Empty;
        //Contains the column names and their corresponding index values in the gridview.
        private Hashtable m_htGVColumns = new Hashtable();
        //Contains the column full view and small view lenghts.
        private Hashtable m_htGVColWidths = new Hashtable();
        private Hashtable m_htBPCColumns = new Hashtable();
        private string m_RowHoverColour = string.Empty;
        XmlDocument XDocUserInfo = new XmlDocument();
        protected LAjitDev.UserControls.ButtonsUserControl ucButtonsUserControl;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Contains the indices of the Amount or IsNumeric columns in the grid view.
        /// </summary>
        private ArrayList m_arrAmountCols = new ArrayList();

        /// <summary>
        /// Holds the reference of the CommonUI class instance created in the BasePage.cs.
        /// </summary>
        public CommonUI ObjCommonUI
        {
            get { return commonObjUI; }
            set { commonObjUI = value; }
        }

        public LAjitDev.UserControls.ButtonsUserControl BtnsUserCtrl
        {
            get
            {
                return ucButtonsUserControl;
            }
            set
            {
                ucButtonsUserControl = value;
            }
        }

        /// <summary>
        /// Contains the Session["GBPC"]/BusinessProcessControls string.
        /// </summary>
        string m_GBPCXml = string.Empty;
        // To store lava menu ids 
        public static int m_li_Id = 0;

        // public string ScriptsCDNPath = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Add JavaScript Files To Header
            AddScriptReferences();

            //Add CSS Files To Header
            AddCSSReferences();

            if (!Page.IsPostBack)
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                lblLogoText.Text = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;
                //Disable the context menus for link buttons.
                DisableContextMenu();
                InitialiseParentReports(XDocUserInfo);
                //InitialiseMenuPanel(XDocUserInfo);
                InitialiseGridViews(XDocUserInfo);
                InitialiseCollapsibles();
                InitialiseBPCLinks(XDocUserInfo);
                InitialiseShortcuts(XDocUserInfo);
                InitialiseRoles();
                InitialiseImages(XDocUserInfo);

                //The following two blocks of code were moved to in here from Dashboard by Danny on 06/1/2009
                //XmlDocument xDocMenu = new XmlDocument();
                //xDocMenu.LoadXml(Session["GBPC"].ToString());
                //Assigning the hdnInput variable with the MenuPanel data which will be used later on for Exploded View.
                //hdnMenuPanel.Value = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel").OuterXml;

                //To set the Theme.Run this only once that is in the Dashboard
                if (this.Page.ToString().Contains("dashboard"))
                {
                    //XmlDocument xDocUsrPref = new XmlDocument();
                    //xDocUsrPref.LoadXml(Convert.ToString(Session["BPE"]));
                    //string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
                    string keyTheme = "430";
                    if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo") != null)
                    {
                        string theme = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                        Session.Add("MyTheme", theme);
                    }
                    //isDashBoard = true;
                }
                hdnThemeName.Value = Convert.ToString(Session["MyTheme"]);//Will be used in the ExplodedView.js file..

                hdnImagesCDNPath.Value = Application["ImagesCDNPath"].ToString();

                hdnScriptCDNPath.Value = Application["ScriptsCDNPath"].ToString();
                //CreateMenuSchema(XDocUserInfo);
                //RegisterJQueryMenu(this.Page, XDocUserInfo);

                //forming MB menu schema
                //CreateMenuSchema(XDocUserInfo);

                //forming megamenu schema
                //MegaMenuSchema(XDocUserInfo);

                //forming LavaLampMenu schema
                BlueMenuSchema(XDocUserInfo);
                //ApycomDropDownMenu(XDocUserInfo);

                //forming LavaLamp with MegaMenu schema
                //BlueMegaMenu(XDocUserInfo);

            }

            //if (Session["LPCollapsed"] == null && !isDashBoard)
            //{
            //    Session["LPCollapsed"] = "1";//Collapse initially
            //}

            ////Check the Session if previously collapsed.
            //if (Session["LPCollapsed"] != null && Session["LPCollapsed"].ToString() == "1")
            //{
            //    pnlLeftPanel.Style.Add("display", "none");
            //    pnlArrow.Src = "../App_Themes/" + hdnThemeName.Value + "/Images/open-arrow.gif";
            //}
            //else
            //{
            //    pnlArrow.Src = "../App_Themes/" + hdnThemeName.Value + "/Images/close-arrow.gif";
            //}
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //Theme applying transfer from basepage

            //string masterfile = string.Empty;
            //if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
            //{
            //    masterfile = "../MasterPages/PopUp.Master";
            //}
            //else
            //{
            //    masterfile = "../MasterPages/TopLeft.Master";
            //}
            ////if (!masterfile.Equals(string.Empty))
            ////{
            ////    this.MasterPageFile = masterfile;
            ////}
            //Page.Theme = (Convert.ToString(HttpContext.Current.Session["MyTheme"]));
        }


        private void AddScriptReferences()
        {

            //CDN Added Scripts

            //jquery-1.3.2.min.js
            HtmlGenericControl hgcjQuery132Script = new HtmlGenericControl();
            hgcjQuery132Script.TagName = "script";
            hgcjQuery132Script.Attributes.Add("type", "text/javascript");
            hgcjQuery132Script.Attributes.Add("language", "javascript");

            //hgcjQuery132Script.Attributes.Add("src", "http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js");
            hgcjQuery132Script.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-1.3.2.min.js");
            Page.Header.Controls.Add(hgcjQuery132Script);

            //jquery-ui-1.7.2.custom.min.js
            HtmlGenericControl hgcjQueryUI172Script = new HtmlGenericControl();
            hgcjQueryUI172Script.TagName = "script";
            hgcjQueryUI172Script.Attributes.Add("type", "text/javascript");
            hgcjQueryUI172Script.Attributes.Add("language", "javascript");
            hgcjQueryUI172Script.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-ui-1.7.2.custom.min.js");
            Page.Header.Controls.Add(hgcjQueryUI172Script);

            //Masterlist.js
            HtmlGenericControl hgcMasterListScript = new HtmlGenericControl();
            hgcMasterListScript.TagName = "script";
            hgcMasterListScript.Attributes.Add("type", "text/javascript");
            hgcMasterListScript.Attributes.Add("language", "javascript");
            hgcMasterListScript.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Masterlist.js");
            Page.Header.Controls.Add(hgcMasterListScript);

            //ljq.js
            HtmlGenericControl hgcljqScript = new HtmlGenericControl();
            hgcljqScript.TagName = "script";
            hgcljqScript.Attributes.Add("type", "text/javascript");
            hgcljqScript.Attributes.Add("language", "javascript");
            hgcljqScript.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ljq.js");
            Page.Header.Controls.Add(hgcljqScript);



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
            HtmlLink hlCustomCss = new HtmlLink();
            hlCustomCss.Href = Application["ImagesCDNPath"].ToString() + "jquery-ui-1.7.2.custom.css";
            hlCustomCss.Attributes["rel"] = "stylesheet";
            hlCustomCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlCustomCss);

            //LajitCDN.css
            HtmlLink hlCDNCss = new HtmlLink();
            hlCDNCss.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            hlCDNCss.Attributes["rel"] = "stylesheet";
            hlCDNCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlCDNCss);

            //menu.css
            HtmlLink hlMenuCss = new HtmlLink();
            hlMenuCss.Href = Application["ImagesCDNPath"].ToString() + "menu.css";
            hlMenuCss.Attributes["rel"] = "stylesheet";
            hlMenuCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlMenuCss);

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

            //shortcut icon favicon.ico
            HtmlLink hlShortCutIcon = new HtmlLink();
            hlShortCutIcon.Href = Application["ImagesCDNPath"].ToString() + "Images/favicon.ico";
            hlShortCutIcon.Attributes["rel"] = "shortcut icon";
            hlShortCutIcon.Attributes["type"] = "image/x-icon";
            Page.Header.Controls.Add(hlShortCutIcon);
        }


        ///<summary>
        /// This is To Intialise Images
        /// </summary>
        public void InitialiseImages(XmlDocument XDocUserInfo)
        {
            XmlNode m_CompanyNode = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/DashboardLogo");
            string imgpath = string.Empty;
            string m_CompanyImageSrc = string.Empty;
            string m_PhysicalImgPath = string.Empty;
            if (m_CompanyNode != null)
            {
                m_CompanyImageSrc = m_CompanyNode.InnerXml.ToString();

                m_PhysicalImgPath = Convert.ToString(ConfigurationSettings.AppSettings["AttachmentsPath"]) + "/" + Session["CompanyEntityID"].ToString() + "/" + m_CompanyImageSrc;
                imgpath = Convert.ToString(ConfigurationSettings.AppSettings["AttachmentsVirtualPath"]) + Session["CompanyEntityID"].ToString() + "/" + m_CompanyImageSrc;

                if (System.IO.File.Exists(m_PhysicalImgPath))
                {
                    imgMainLogo.Src = imgpath;
                }
                else
                {
                    imgpath = Application["ImagesCDNPath"].ToString() + "Images/lajit_logo.jpg";
                    imgMainLogo.Src = imgpath;
                }
            }
            else
            {
                imgpath = Application["ImagesCDNPath"].ToString() + "Images/lajit_logo.jpg";
                imgMainLogo.Src = imgpath;
            }
        }

        /// <summary>
        /// To get the xpath of the particular node.
        /// </summary>
        public int GetNodePosition(XmlNode child, string compare)
        {
            for (int i = 0; i <= child.ParentNode.ChildNodes.Count; i++)
            {
                if (child.ParentNode.ChildNodes[i].Name == compare)
                {
                    // tricksy XPath, not starting its positions at 0 like a normal language
                    return i + 1;
                }
            }
            throw new InvalidOperationException("Child node somehow not found in its parent's ChildNodes property.");
        }

        //public void CreateMenuSchema(XmlDocument XDocUserInfo)
        //{
        //    int cnt = 1;
        //    int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<td style=\"background: url('../App_Themes/" + Session["MyTheme"].ToString() + "/Images/but-bg.gif')repeat-x; width:62px\"><img src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/logo-bottom.gif' alt='LogoBottom' title='LogoBottom' width='62' style='border:0px; height:26px' /></td>");
        //    sb.Append("<td align='left' style='width: 45px; height: 26px; cursor: pointer' valign='bottom'><img src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/home-but.gif' alt='Go Home' title='Go Home' width='45px' height='26px' border='0' onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' onclick='OnHomeImgClick();'/></td>");
        //    XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");
        //    string datapath = string.Empty;
        //    foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
        //    {
        //        datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";
        //        //datapath = "/*[position()=1]/*[position()=6]/*[position()=" + cnt + "]";
        //        sb.Append("<td><a class='ui-corner-all' tabindex='" + cnt + "' href='#" + nodeCurrPanel.Attributes["Label"].Value + "' id='myMenuButton" + cnt + "'>" + "<img alt=\"\" onclick=\"RenderFullMenuView('" + datapath + "')\" onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' border='0' src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/" + nodeCurrPanel.Attributes["ImgSrc"].Value + "'></a>");
        //        //sb.Append("<td nowrap=true class='body'><a tabindex='0' href=#" + nodeCurrPanel.Attributes["Label"].Value + " class='fg-button fg-button-icon-right ui-widget ui-state-default ui-corner-all'" + " id=myMenuButton" + cnt + ">" + "<img src=../App_Themes/LAjit/Images/"+nodeCurrPanel.Attributes["ImgSrc"].Value + "border=\"0\"></a>");
        //        //sb.Append("<td><a tabindex='0' href=#" + nodeCurrPanel.Attributes["Label"].Value + " class='fg-button fg-button-icon-right ui-widget ui-state-default ui-corner-all'" + " id=myMenuButton" + cnt + ">" + nodeCurrPanel.Attributes["Label"].Value + "</a>");<img src="../App_Themes/LAjit/Images/financials-but-over.gif" />
        //        sb.Append("<div id='" + nodeCurrPanel.Attributes["Label"].Value + "' class='hidden'><ul>");
        //        foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
        //        {
        //            sb.Append("<li><a href='#' class=\" ui-corner-all\">" + nodeCurrItemElement.Attributes["Label"].Value + "</a><ul>");
        //            foreach (XmlNode nodeCurrProcessElement in nodeCurrItemElement.ChildNodes)
        //            {
        //                sb.Append("<li><a href='#' onclick=\"MainMenuItemClick('" + nodeCurrProcessElement.Attributes["BPGID"].Value + "','" + nodeCurrProcessElement.Attributes["PageInfo"].Value + "')\" class='ui-corner-all'>" + nodeCurrProcessElement.Attributes["Label"].Value + "</a></li>");
        //            }
        //            sb.Append("</ul></li>");
        //        }
        //        sb.Append("</ul></div></td>");
        //        cnt++;
        //    }
        //    tdmnuTopMenu.InnerHtml = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>" + sb.ToString() + "<td align='left' style=\"background: url(../App_Themes/" + Session["MyTheme"].ToString() + "/Images/but-bg.gif);repeat-x;\"><img src=../App_Themes/" + Session["MyTheme"] .ToString()+ "/Images/spacer.gif alt=Spacer width=\"113px\" height='1' /></td>" + "</table>";
        //}

        //public void RegisterJQueryMenu(Page page, XmlDocument XDocUserInfo)
        //{
        //    int i = 1;
        //    StringBuilder sb = new StringBuilder();
        //    XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");
        //    foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
        //    {
        //        sb.Append("jQuery(function(){jQuery('#myMenuButton" + i + "').menu({content: jQuery('#myMenuButton" + i + "').next().html(),flyOut: true});});");
        //        i++;
        //    }
        //    ScriptManager.RegisterClientScriptBlock(page, page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
        //}

        /// <summary>
        /// Disable the context menus for link styled buttons.
        /// </summary>

        /* public string GenerateMBMenuDivs(XmlDocument XDocUserInfo)
         {
             int i = 1;
             StringBuilder sb = new StringBuilder();
             XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");
             foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
             {
                 foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
                 {
                     sb.Append("<div id=\"submenu_" + i + "\" class=\"mbmenu\">");
                     foreach (XmlNode nodeCurrProcessElement in nodeCurrItemElement.ChildNodes)
                     {
                         sb.Append("<a action=\"MainMenuItemClick('" + nodeCurrProcessElement.Attributes["BPGID"].Value + "','" + nodeCurrProcessElement.Attributes["PageInfo"].Value + "')\" >" + nodeCurrProcessElement.Attributes["Label"].Value + "</a>");
                     }
                     sb.Append("</div>");
                     i++;
                 }
             }
             return sb.ToString();
         }*/

        /* public void CreateMenuSchema(XmlDocument XDocUserInfo)
         {
             int cnt = 1; int i = 1;
             string datapath = string.Empty;
             StringBuilder sb = new StringBuilder();
             int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");

             //sb.Append("<td style=\"background: url('../App_Themes/" + Session["MyTheme"].ToString() + "/Images/but-bg.gif')repeat-x; width:62px\"><img src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/logo-bottom.gif' alt='LogoBottom' title='LogoBottom' width='62' style='border:0px; height:26px' /></td>");
             //sb.Append("<td class=\"rootVoice\" align='left' style='width: 45px; height: 26px; cursor: pointer' valign='bottom'><img src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/home-but.gif' alt='Go Home' title='Go Home' width='45px' height='26px' border='0' onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' onclick='OnHomeImgClick();'/></td>");
             //sb.Append("<td class=\"rootVoice {menu: 'menu_home'}\" align='left' style='width: 45px; height: 26px; cursor: pointer' valign='bottom'><img src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/home-but.gif' alt='Go Home' title='Go Home' width='45px' height='26px' border='0' onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' onclick='OnHomeImgClick();'/></td>");
             //sb.Append("<div id=\"menu_home\" class=\"mbmenu\">");
             //sb.Append("<a rel=\"text\"><input style=\"width:105px\" name=\"txtMenuSearch\" type=\"text\" id=\"txtMenuSearch\"/><img src='../App_Themes/Lajit/Images/grid-find-icon.png'/></a>");

             XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

             foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
             {
                 datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";
                 sb.Append("<td class=\"rootVoice {menu: 'menu_" + cnt + "'}\"><img onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' border='0' onclick=\"RenderFullMenuView('" + datapath + "')\" src='" + Application["ImagesCDNPath"].ToString() + "Images/" + nodeCurrPanel.Attributes["ImgSrc"].Value + "'/></td>");
                 sb.Append("<div id=\"menu_" + cnt + "\" class=\"mbmenu\">");

                 foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
                 {
                     //if (i == 1)
                     //{
                     //    sb.Append("<a rel=\"text\"><input style=\"width:105px\" name=\"txtMenuSearch\" type=\"text\" id=\"txtMenuSearch\"/><img src='../App_Themes/Lajit/Images/grid-find-icon.png'/></a>");
                     //}
                     sb.Append("<a menu=\"submenu_" + i + "\">" + nodeCurrItemElement.Attributes["Label"].Value + "</a>");
                     i++;
                 }
                 sb.Append("</div>");
                 cnt++;
             }
             sb.Append(GenerateMBMenuDivs(XDocUserInfo));
             //tdmnuTopMenu.InnerHtml = "<table class=\"myMenu rootVoices\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>" + sb.ToString() + "</tr></table>";
         }*/



        /*public void MegaMenuSchema(XmlDocument XDocUserInfo)
        {
            int cnt = 1; int i = 1;
            int j = 1;
            string datapath = string.Empty;
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string strtabs = string.Empty;
            int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");
            string strHref = string.Empty;

            XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

            foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
            {
                datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";
                //sb.Append("<td class=\"rootVoice {menu: 'menu_" + cnt + "'}\"><img onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' border='0' onclick=\"RenderFullMenuView('" + datapath + "')\" src='../App_Themes/" + Session["MyTheme"] + "/Images/" + nodeCurrPanel.Attributes["ImgSrc"].Value + "'/></td>");
                //sb.Append("<div id=\"menu_" + cnt + "\" class=\"mbmenu\">");

                sb1.Append("<td><a id=\"megaanchor" + j + "\" href=\"#\"><img onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' border='0' src='" + Application["ImagesCDNPath"].ToString() + "Images/" + nodeCurrPanel.Attributes["ImgSrc"].Value + "'/></a></td>");
                sb.Append("<div id=\"megamenu" + cnt + "\" class=\"megamenu\">");
              
                int chCnt = 0;
                int chCnt2 = 0;
                foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
                {
                    chCnt2 = (chCnt % 3);
                    if (chCnt2 == 0)
                    {
                        sb.Append("<br style=\"clear: left\" />");
                    }
                    sb.Append("<div class=\"column\">");
                    sb.Append("<ul>");
                    // sb.Append("<li><a href=\"http://www.google.com\">" + nodeCurrItemElement.Attributes["Label"].Value + "</a></li>");
                    sb.Append("<h3>" + nodeCurrItemElement.Attributes["Label"].Value + "</h3>");
                    foreach (XmlNode nodeItems in nodeCurrItemElement.ChildNodes)
                    {
                        strHref = "javascript:MainMenuItemClick('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                        sb.Append("<li><a href=\"" + strHref + "\">" + nodeItems.Attributes["Label"].Value + "</a></li>");
                    }
                    i++;
                    //eAC
                    //sb.Append("<li><img src='../App_Themes/" + Session["MyTheme"] + "/Images/base_tabs_line.png'></li>");

                    sb.Append("</ul>");
                    sb.Append("</div>");
                    chCnt++;

                }
                sb.Append("</div>");
                cnt++;
                j++;
            }
            //sb.Append(GenerateMBMenuDivs(XDocUserInfo));
            //tdmnuTopMenu.InnerHtml = "<table class=\"myMenu rootVoices\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>" + sb.ToString() + "</tr></table>";

           // tdmnuTopMenu.InnerHtml = "<table class=\"myMenu rootVoices\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>" + sb1.ToString() + "</tr></table>";
            
            strtabs="<table class=\"myMenu rootVoices\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>" + sb1.ToString() + "</tr></table>";

            litmenu.Text = strtabs.ToString() + sb.ToString();
        }*/
        /// <summary>
        /// To load lava lamp menu
        /// jQuery Menu Drop Down Style 04 (Steel Blue) from apycom.com
        /// </summary>
        /// <param name="XDocUserInfo"></param>
        private void BlueMenuSchema(XmlDocument XDocUserInfo)
        {

            int cnt = 1;
            int i = 1;
            int j = 1;

            string datapath = string.Empty;
            string strHref = string.Empty;
            string strMenuText = string.Empty;
            string strPopupHref = string.Empty;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();


            int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");

            XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

            sb.Append("<div id=menu>");
            sb.Append("<ul class=menu>");
            sb.Append("<li><a id='MenuHome' href='javascript:OnHomeImgClick();' class=\"parent\"><span>Home</span></a>");
            sb.Append("</li>");

            foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
            {
                datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";

                strMenuText = nodeCurrPanel.Attributes["Label"].Value;
                //Onclick rendar full view menu
                sb.Append("<li><a href=\"javascript:RenderFullMenuView('" + datapath + "');\" class=\"parent\"><span>" + strMenuText + "</span></a>");

                int chCnt = 0;
                int chCnt2 = 0;

                sb.Append("<div><ul>");
                foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
                {
                    sb.Append("<li><a href=\"#\" class=\"parent\"><span>" + nodeCurrItemElement.Attributes["Label"].Value + "</span></a>");
                    sb.Append("<div><ul>");
                    foreach (XmlNode nodeItems in nodeCurrItemElement.ChildNodes)
                    {
                        //Onclick navigatingpage open in same location
                        strHref = "javascript:MainMenuItemClick('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                        //Onclick navigatingpage open in a popup.
                        strPopupHref = "return PopUpPage('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "',event)";
                        //Adding right corner popup icon
                        //sb.Append("<li id=" + m_li_Id + "><img style='float:right;padding:6px 8px' align='right' height='14' width='14' src='../App_Themes/" + Session["MyTheme"] + "/Images/max.gif' border='0' onclick=\"" + strPopupHref + "\"><a href=\"" + strHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");

                        if (Request.Browser.Browser == "IE")
                        {
                            sb.Append("<li id=" + m_li_Id + "><a href=\"" + strHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");
                        }
                        else
                        {
                            sb.Append("<li id=" + m_li_Id + "><a href=\"" + strHref + "\"><img style='float:right;padding:6px 8px' align='right' height='14' width='14' src='" + Application["ImagesCDNPath"].ToString() + "Images/max.gif' border='0' onclick=\"" + strPopupHref + "\" /><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");
                        }


                        m_li_Id++;
                    }
                    sb.Append("</ul></div>");
                    i++;
                    sb.Append("</li>");
                    chCnt++;
                }
                sb.Append("</ul></div>");
                sb.Append("</li>");
                cnt++;
                j++;
            }
            sb.Append("</ul></div>");
            litmenu.Text = sb.ToString();
        }



        /// <summary>
        /// Jquery dropdownmenu with popup as third level
        /// </summary>
        /// <param name="XDocUserInfo"></param>
        private void ApycomDropDownMenu(XmlDocument XDocUserInfo)
        {

            int cnt = 1;
            int i = 1;
            int j = 1;

            string datapath = string.Empty;
            string strHref = string.Empty;
            string strMenuText = string.Empty;
            string strPopupHref = string.Empty;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();


            int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");

            XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

            sb.Append("<div id=menu>");
            sb.Append("<ul class=menu>");
            sb.Append("<li><a id='MenuHome' href='javascript:OnHomeImgClick();' class=\"parent\"><span>Home</span></a>");
            sb.Append("</li>");

            foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
            {
                datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";

                strMenuText = nodeCurrPanel.Attributes["Label"].Value;
                //Onclick rendar full view menu
                sb.Append("<li><a href=\"javascript:RenderFullMenuView('" + datapath + "');\" class=\"parent\"><span>" + strMenuText + "</span></a>");

                int chCnt = 0;
                int chCnt2 = 0;

                sb.Append("<div><ul>");
                foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
                {
                    sb.Append("<li><a href=\"#\" class=\"parent\"><span>" + nodeCurrItemElement.Attributes["Label"].Value + "</span></a>");
                    sb.Append("<div><ul>");
                    foreach (XmlNode nodeItems in nodeCurrItemElement.ChildNodes)
                    {
                        //Onclick navigatingpage open in same location
                        strHref = "javascript:MainMenuItemClick('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                        //Onclick navigatingpage open in a popup.
                        strPopupHref = "return PopUpPage('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "',event)";
                        //Adding right corner popup icon
                        //sb.Append("<li id=" + m_li_Id + "><img style='float:right;padding:6px 8px' align='right' height='14' width='14' src='../App_Themes/" + Session["MyTheme"] + "/Images/max.gif' border='0' onclick=\"" + strPopupHref + "\"><a href=\"" + strHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");

                        //sb.Append("<li id=" + m_li_Id + "><a href=\"" + strHref + "\"><img style='float:right;padding:6px 8px' align='right' height='14' width='14' src='" + Application["ImagesCDNPath"].ToString() + "Images/max.gif' border='0' onclick=\"" + strPopupHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");
                        //another for popup
                        sb.Append("<li id=" + m_li_Id + "><a href=\"" + strHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a>");
                        //
                        sb.Append("<div><ul>");
                        sb.Append("<li><a href='#'><img  align='center'  height='14' width='14' src='" + Application["ImagesCDNPath"].ToString() + "Images/max.gif' border='0' onclick=\"" + strPopupHref + "\"></a></li>");
                        sb.Append("</ul></div>");
                        sb.Append("</li>");
                        //
                        m_li_Id++;
                    }
                    sb.Append("</ul></div>");
                    i++;
                    sb.Append("</li>");
                    chCnt++;
                }
                sb.Append("</ul></div>");
                sb.Append("</li>");
                cnt++;
                j++;
            }
            sb.Append("</ul></div>");
            litmenu.Text = sb.ToString();
        }




















        /* private void BlueMegaMenu(XmlDocument XDocUserInfo)
         {
             //TESTING
             int cnt = 1; int i = 1;
             int j = 1;
             string datapath = string.Empty;
             StringBuilder sb = new StringBuilder();
             StringBuilder sb1 = new StringBuilder();
             int PositionMnupnl = GetNodePosition(XDocUserInfo.SelectSingleNode("//menupanel"), "menupanel");
             string strHref = string.Empty;
             string strMenuText = string.Empty;
             string strLI_Ids = string.Empty;
             string strPopupHref = string.Empty;
             XmlNode nodePanelElement = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

             sb.Append("<div id='menu' align='center'>");
             sb.Append("<ul class=menu>");
             sb.Append("<li><a id='MenuHome' href='javascript:OnHomeImgClick();' class=\"parent\"><span>Home</span></a>");
             sb.Append("</li>");
         */
        /* old one commented
         * foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
        {
            datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";
            strMenuText = nodeCurrPanel.Attributes["Label"].Value; //nodeCurrPanel.Attributes["ImgSrc"].Value.Replace("-but.png","");  
            sb.Append("<li><a href=\"javascript:RenderFullMenuView('" + datapath + "');\" class=\"parent\"><span>" + strMenuText + "</span></a>");
            int chCnt = 0;
            int chCnt2 = 0;
            sb.Append("<div><ul>");
            foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
            {
                sb.Append("<li><a href=\"#\" class=\"parent\"><span>" + nodeCurrItemElement.Attributes["Label"].Value + "</span></a>");
                sb.Append("<div><ul>");
                foreach (XmlNode nodeItems in nodeCurrItemElement.ChildNodes)
                {
                    strHref = "javascript:MainMenuItemClick('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                    strPopupHref = "PopUpPage('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                    sb.Append("<li id=" + IdLI + "><img style='float:right;padding:6px 8px' align='right' height='14' width='14' src='../App_Themes/" + Session["MyTheme"] + "/Images/max.gif' border='0' onclick=\"" + strPopupHref + "\"><a href=\"" + strHref + "\"><span>" + nodeItems.Attributes["Label"].Value + "</span></a></li>");
                    IdLI++;
                }
                sb.Append("</ul></div>");
                i++;
                sb.Append("</li>");
                chCnt++;

            }
            sb.Append("</ul></div>");
            sb.Append("</li>");
            cnt++;
            j++;

               
        }*/


        /*foreach (XmlNode nodeCurrPanel in nodePanelElement.ChildNodes)
        {
            datapath = "/*[position()=1]/*[position()=" + PositionMnupnl + "]/*[position()=" + cnt + "]";


            strMenuText = nodeCurrPanel.Attributes["Label"].Value; //nodeCurrPanel.Attributes["ImgSrc"].Value.Replace("-but.png","");  
            sb.Append("<li><a id=\"megaanchor" + j + "\" href=\"javascript:RenderFullMenuView('" + datapath + "');\" class=\"parent\"><span>" + strMenuText + "</span></a></li>");
               

            //sb1.Append("<td><a id=\"megaanchor" + j + "\" href=\"#\"><img onmouseover='javascript:toggleImage(this);' onmouseout='javascript:toggleImage(this);' border='0' src='../App_Themes/" + Session["MyTheme"] + "/Images/" + nodeCurrPanel.Attributes["ImgSrc"].Value + "'/></a></td>");

            int chCnt = 0;
            int chCnt2 = 0;
            //sb.Append("<div><ul>");


            sb1.Append("<div id=\"megamenu" + cnt + "\" class=\"megamenu\">");

            //int chCnt = 0;
            //int chCnt2 = 0;
            foreach (XmlNode nodeCurrItemElement in nodeCurrPanel.ChildNodes)
            {
                chCnt2 = (chCnt % 3);
                if (chCnt2 == 0)
                {
                    sb1.Append("<br style=\"clear: left\" />");
                }
                sb1.Append("<div class=\"column\">");
                sb1.Append("<ul>");
                // sb.Append("<li><a href=\"http://www.google.com\">" + nodeCurrItemElement.Attributes["Label"].Value + "</a></li>");
                sb1.Append("<h3>" + nodeCurrItemElement.Attributes["Label"].Value + "</h3>");
                foreach (XmlNode nodeItems in nodeCurrItemElement.ChildNodes)
                {
                    strHref = "javascript:MainMenuItemClick('" + nodeItems.Attributes["BPGID"].Value + "','" + nodeItems.Attributes["PageInfo"].Value + "')";
                    sb1.Append("<li><a href=\"" + strHref + "\">" + nodeItems.Attributes["Label"].Value + "</a></li>");
                }
                i++;
                //eAC
               // sb.Append("<li><img src='../App_Themes/" + Session["MyTheme"] + "/Images/base_tabs_line.png'></li>");

                sb1.Append("</ul>");
                sb1.Append("</div>");
                chCnt++;

            }
            sb1.Append("</div>");
            cnt++;
            j++;
        }
        sb.Append("</ul></div>");
       // hdnMegaMenuTabCount.Value = j.ToString();
        litmenu.Text = sb.ToString();
       // litmegamenu.Text = sb1.ToString();
    }*/

        private void DisableContextMenu()
        {
            lnkBtnModRequest.Attributes["oncontextmenu"] = "return false;";
            lnkBtnHelp.Attributes["oncontextmenu"] = "return false;";
            lnkBtnLogout.Attributes["oncontextmenu"] = "return false;";
            lnkBtnRefreshGrids.Attributes["oncontextmenu"] = "return false;";
            lnkBtnChangeRole.Attributes["oncontextmenu"] = "return false;";
            lnkBtnRemoveRole.Attributes["oncontextmenu"] = "return false;";
            lnkBtnReloadPreference.Attributes["oncontextmenu"] = "return false;";
        }

        private void InitialiseBPCLinks(XmlDocument XDocUserInfo)
        {
            XmlNode nodeBPC = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls");

            //Modification Request.
            string process = "ModRequest";
            XmlNode nodeModReq = nodeBPC.SelectSingleNode("BusinessProcess[@ID='" + process + "']");
            if (nodeModReq != null)
            {
                tdModReqSeparator.Visible = true;
                lnkBtnModRequest.Visible = true;
                lnkBtnModRequest.Text = nodeModReq.Attributes["Label"].Value;
                string formBPGID = string.Empty;
                if (Session["BPINFO"] != null)
                {
                    //Setting formBPGID
                    XmlDocument xDocformBPGID = new XmlDocument();
                    xDocformBPGID.LoadXml(Session["BPINFO"].ToString());
                    XmlNode nodeformBPGID = xDocformBPGID.SelectSingleNode("bpinfo/BPGID");
                    if (nodeformBPGID != null)
                    {
                        formBPGID = nodeformBPGID.InnerText;
                    }
                }
                string hdnVarName = this.ClientID + "_hdnCurrentAction";
                string processBPGID = nodeModReq.Attributes["BPGID"].Value;
                string pageInfo = nodeModReq.Attributes["PageInfo"].Value;
                string isPopUp = "1";
                string s = "OnMasterPgBPCLinkClick('" + processBPGID + "','" + pageInfo + "','" + formBPGID + "','" + isPopUp + "','" + hdnVarName + "');return false;";
                lnkBtnModRequest.OnClientClick = s;
            }

            //Help
            string helpprocess = "Help";
            XmlNode nodeHelp = nodeBPC.SelectSingleNode("BusinessProcess[@ID='" + helpprocess + "']");
            if (nodeHelp != null)
            {
                string pageInfo = nodeHelp.Attributes["PageInfo"].Value;
                string isPopUp = "1";
                string script = "javascript:OpenHelpModalPopup('" + isPopUp + "','" + pageInfo + "',event);return false;";
                lnkBtnHelp.OnClientClick = script;
            }

            //Help Authoring
            string helpAuthProcID = "Help Authoring";
            XmlNode nodeHelpAuth = nodeBPC.SelectSingleNode("BusinessProcess[@ID='" + helpAuthProcID + "']");
            if (nodeHelpAuth != null)
            {
                string pageInfo = nodeHelpAuth.Attributes["PageInfo"].Value;
                string isPopUp = "1";
                string clientHelpAuthDetails = "g_HelpAuth='" + pageInfo + "~" + isPopUp + "';";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "HelpAuth", clientHelpAuthDetails, true);
                string script = "javascript:OpenHelpModalPopup('" + isPopUp + "','" + pageInfo + "',event);return false;";
                lnkBtnHelpAuth.OnClientClick = script;
                lnkBtnHelpAuth.Text = nodeHelpAuth.Attributes["Label"].Value;
                lnkBtnHelpAuth.Visible = true;
                tdSepHelpAuth.Visible = true;
            }

        }

        private void InitialiseCollapsibles()
        {
            pnlExpViewTitle.Style.Add("visibility", "hidden");
            pnlExpViewTitle.Style.Add("height", "0px");
            pnlExpViewContent.Style.Add("visibility", "hidden");
            pnlExpViewContent.Style.Add("height", "0px");
            pnlExpViewContent.BackImageUrl = Application["ImagesCDNPath"].ToString() + "Images/predash-bg.gif";
        }

        private void InitialiseGridViews(XmlDocument XDocUserInfo)
        {
            string currentBPGID = string.Empty;
            string returnXML = string.Empty;
            string bpcXML = string.Empty;

            //Display the timer forever updated on 13-02-09

            /*if (timerLPUpdater.Enabled == false)
            {
                string refreshRate = GetUserPreferenceValue("GridRefreshInterval");
                //refreshRate = "2";
                if (refreshRate != string.Empty)
                {
                    timerLPUpdater.Enabled = true;
                    //Assigning the timer's interval in milliseconds.(Assuming input is in seconds)
                    timerLPUpdater.Interval = Convert.ToInt32(refreshRate) * 1000;
                }
                else
                {
                    timerLPUpdater.Enabled = false;
                }
            }*/

            //LPGV1
            BindLPGV1(XDocUserInfo);

            //LPGV2
            //DashBoardLPGV2
            currentBPGID = GetBPGID("52", "Left", XDocUserInfo);
            if (currentBPGID == string.Empty)
            {
                pnlLPGV2.Visible = false;
            }
            else
            {
                returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(currentBPGID, XDocUserInfo));
                DisplayFoundResults(returnXML, grdVwLeftPanel2, htcLPGV2, XDocUserInfo);
                lnkBtnLPGV2FV.Attributes["oncontextmenu"] = "return false;";
                lnkBtnLPGV2FV.OnClientClick = "return ShowDetailedView('" + currentBPGID + "', '20');return false;";

                //New Feature:Onlogout based onrows display confirmation alert
                //Added on27012010
                int rowsCount = GetRowsCount(returnXML);
                if (rowsCount != 0)
                {  //You  have 1 pending transaction(s). Please click “Ok” to logout else press “Cancel”.
                    //lnkBtnLogout.OnClientClick = "return OnLogOut(" + rowsCount + ");";
                    lnkBtnLogout.OnClientClick = "return OnLogOut();";
                }


                ////Initialising the corresponding user control in the pop-up panel.
                ////ucLPGV2.GridViewBPE = Session["bpe"].ToString();
                //ucLPGV2.GridViewBPInfo = GenerateGVBPEInfo(currentBPGID);
                //ucLPGV2.GridViewInitData = returnXML;
                ////((HtmlTableRow)ucLPGV2.FindControl("trTitle")).Visible = true;
                //ucLPGV2.DataBind();
            }
            updtPnlLPGV.Update();
        }

        private void BindLPGV1(XmlDocument XDocUserInfo)
        {
            //DashBoardLPGV1
            string currentBPGID = GetBPGID("51", "Left", XDocUserInfo);
            if (currentBPGID == string.Empty)
            {
                pnlLPGV1.Visible = false;
            }
            else
            {
                string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(currentBPGID, XDocUserInfo));
                DisplayFoundResults(returnXML, grdVwLeftPanel1, htcLPGV1, XDocUserInfo);
                lnkBtnLPGV1FV.Attributes["oncontextmenu"] = "return false;";
                lnkBtnLPGV1FV.OnClientClick = "return ShowDetailedView('" + currentBPGID + "', '20');return false;";
                ////Initialising the corresponding user control in the pop-up panel.
                ////ucLPGV1.GridViewBPE = Session["BPE"].ToString();
                //ucLPGV1.GridViewBPInfo = GenerateGVBPEInfo(currentBPGID);
                //ucLPGV1.GridViewInitData = returnXML;
                ////((HtmlTableRow)ucLPGV1.FindControl("trTitle")).Visible = true;
                //ucLPGV1.DataBind();
            }
        }

        //Generic method to be used for requesting data for all the GridViews in the current page.
        private string GenerateGVBPEInfo(string BPGID)
        {
            XmlDocument xDocGV = new XmlDocument();

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);

            ////Adding the PriorFormInfo Node.
            //if (Session["PriorFormInfo"] != null)
            //{
            //    string formInfo = Session["PriorFormInfo"].ToString();
            //    nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");
            //}

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            //No paging stuff is required when requesting data for the first page.
            //Page size will be same as the Default page size in the User Preferences.
            //nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeBPInfo.OuterXml;
        }

        /// <summary>
        /// Gets the BPGID for a given CenterPanel GridView.
        /// </summary>
        /// <param name="index">The position of the CenterPanel node in the returned XML.</param>
        /// <returns></returns>
        private string GetBPGID(string typeOfPrefId, string GVContainer, XmlDocument XDocUserInfo)
        {
            //string keyValue = ConfigurationManager.AppSettings[typeOfPrefId];
            XmlNode nodeBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/" + GVContainer + "Panel/" + GVContainer + "Process[@TypeOfPreferenceID = '" + typeOfPrefId + "']");
            if (nodeBPGID != null)
            {
                return nodeBPGID.Attributes["BPGID"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        //Generic method to be used for requesting data for all the GridViews in the current page.
        private string GenerateGVRequestXML(string BPGID, XmlDocument XDocUserInfo)
        {
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml;

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            ////Adding the PriorFormInfo Node.
            //if (Session["PriorFormInfo"] != null)
            //{
            //    string formInfo = Session["PriorFormInfo"].ToString();
            //    nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");
            //}

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeRoot.OuterXml;
        }

        private void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            string toolTip = gridTitle;
            //Clip any overflowing text.
            if (gridTitle.Length > 22)
            {
                gridTitle = gridTitle.Substring(0, 22) + "..";
            }
            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // need bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;
            htcWork.InnerText = gridTitle;
            htcWork.Width = intScrollLength.ToString();
            htcWork.Attributes.Add("title", toolTip);
            htcWork.Style.Add("cursor", "pointer");
            objGraphics.Dispose();
            objBitmap.Dispose();
        }

        /// <summary>
        /// Binds the specified grid view with the given data.
        /// </summary>
        /// <param name="returnXML">The XML data consisting of Column and row data.</param>
        /// <param name="grdVwToBind"></param>
        public void DisplayFoundResults(string returnXML, GridView grdVwToBind, HtmlTableCell htcCurrent, XmlDocument XDocUserInfo)
        {
            XmlDocument XDocreturnXML = new XmlDocument();
            XDocreturnXML.LoadXml(returnXML);
            ////Re-initialising the Session["PriorFormInfo"] variable with most recently called BPID's return xml's FORMInfo.
            //Session["PriorFormInfo"] = XDocUserInfo.SelectSingleNode("Root/bpeout/FormInfo").OuterXml;

            //Get the Grid Layout nodes
            string treeNodeName = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            m_GVTreeNodeName = treeNodeName;

            //Initialising the variables m_CurrentBPGID and m_CurrentPageInfo for the view details functionality
            m_CurrentBPGID = XDocreturnXML.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            m_CurrentPageInfo = XDocreturnXML.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
            //Keep one variable for the entire formInfo..
            hdnFormInfo.Value = m_CurrentBPGID + "~::~" + m_CurrentPageInfo;

            //Setting the title of the grid view container panel.
            SetPanelHeading(htcCurrent, XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title").InnerText);

            //Getting the dataset to be bound to the grid.
            XmlNode nodeRows = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
            if (nodeRows == null)
            {
                return;
            }
            XmlNodeReader read = new XmlNodeReader(nodeRows);
            DataSet dsRows = new DataSet();
            dsRows.ReadXml(read);

            ArrayList arrAmountCols = new ArrayList();
            m_htBPCntrls.Clear();
            m_htGVColumns.Clear();
            m_htGVColWidths.Clear();
            m_arrAmountCols.Clear();

            int colCntr = 0;
            //Creating the Columns.
            XmlNode nodeColumns = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

            if (grdVwToBind.Columns.Count == 0 || grdVwToBind.Columns.Count == 1)
            {
                foreach (XmlNode colNode in nodeColumns.ChildNodes)
                {
                    int smallViewLength = Convert.ToInt32(colNode.Attributes["SmallViewLength"].Value);
                    string label = colNode.Attributes["Label"].Value;
                    //Adding the current column node the ROWS dataset if not present.
                    if (!dsRows.Tables[0].Columns.Contains(label))
                    {
                        DataColumn dcNew = new DataColumn(label, typeof(string));
                        dcNew.AllowDBNull = true;
                        dsRows.Tables[0].Columns.Add(dcNew);
                    }
                    if (smallViewLength != 0)
                    {
                        //For tooltip functionality.
                        m_htGVColumns.Add(label, colCntr);
                        m_htGVColWidths.Add(colCntr, smallViewLength);

                        BoundField newField = new BoundField();
                        newField.DataField = label;
                        newField.HeaderText = colNode.Attributes["Caption"].Value;

                        //Set justification to Right-Justify for columns with ControlType as Amount
                        if (colNode.Attributes["ControlType"] != null && colNode.Attributes["ControlType"].Value == "Amount")
                        {
                            newField.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            newField.ItemStyle.CssClass = "grdVwColRightJustify";
                            arrAmountCols.Add(label);
                        }

                        XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                        if (attBPControl != null && attBPControl.Value.Trim().Length > 0)
                        {
                            m_htBPCntrls.Add(attBPControl.Value.Trim(), GetColumnIndex(label, dsRows.Tables[0]));
                            m_htBPCColumns.Add(attBPControl.Value.Trim(), colCntr);
                        }

                        grdVwToBind.Columns.Add(newField);
                        colCntr++;
                    }
                }
            }

            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                string label = colNode.Attributes["Label"].Value;
                //Adding the current column node the ROWS dataset if not present.
                if (!dsRows.Tables[0].Columns.Contains(label))
                {
                    DataColumn dcNew = new DataColumn(label, typeof(string));
                    dcNew.AllowDBNull = true;
                    dsRows.Tables[0].Columns.Add(dcNew);
                }
            }

            //Format the Amount columns specified by the m_arrIsNumericCols object in the data source
            foreach (string colName in arrAmountCols)
            {
                int colIndex = dsRows.Tables[0].Columns[colName].Ordinal;
                foreach (DataRow dr in dsRows.Tables[0].Rows)
                {
                    decimal amount;
                    if (Decimal.TryParse(dr[colIndex].ToString(), out amount))
                    {
                        dr[colIndex] = string.Format("{0:N}", amount);
                    }
                }
            }

            //Initialsing variables to be used in OnRowDataBound
            string bpcXML = string.Empty;
            XmlNode nodeBPC = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (nodeBPC != null)
            {
                bpcXML = nodeBPC.OuterXml;
            }
            else
            {
                bpcXML = string.Empty;
            }
            m_BPCXml = bpcXML;

            ////Initialising the Business rules used for ColumnHyperlinks enabling in the OnRowDataBound
            //m_BusinessRules = XDocUserInfo.SelectSingleNode("Root/bpeout/BusinessRules").OuterXml;


            m_GBPCXml = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls").OuterXml;

            m_primaryKeyFieldName = dsRows.Tables[0].Columns[0].ColumnName;

            //Setting the Alternating Row Style of the GridView
            //SmallViewAlternatingStyle
            string isAlternating = commonObjUI.GetUserPreferenceValue("55");

            if (isAlternating == string.Empty)
            {
                //FullViewAlternatingStyle
                isAlternating = commonObjUI.GetUserPreferenceValue("56");
            }
            m_RowHoverColour = string.Empty;
            if (isAlternating == "1")
            {
                grdVwToBind.AlternatingRowStyle.CssClass = "AlternatingRowStyle";
                m_RowHoverColour = ConfigurationManager.AppSettings["GridRowHoverColor"];

                //Apply Row Hovering effects only if there is an alternating style applied.
                //Get the Row Hover Colour from the Config file which will be used in the RowDataBound event.
                m_IsRowAlternating = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OnHover", "g_AllowRowHover=true;", true);
            }

            grdVwToBind.DataSource = dsRows;
            grdVwToBind.DataBind();
        }

        /// <summary>
        /// Gets the position of the column in the given data table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="dtSearch">Data table to search in.</param>
        /// <returns>Integer column position, zero being the first column.</returns>
        private int GetColumnIndex(string columnName, DataTable dtSearch)
        {
            int colCntr = 0;
            foreach (DataColumn dc in dtSearch.Columns)
            {
                if (dc.ColumnName.ToUpper() == columnName.ToUpper())
                {
                    return colCntr;
                }
                colCntr++;
            }
            return -1;
        }
        private void InitialiseParentReports(XmlDocument XDocUserInfo)
        {

            XmlNode nodePanels = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");

            if (nodePanels == null)
            {
                return;
            }
            foreach (XmlNode panel in nodePanels.ChildNodes)
            {
                string panelLabel = panel.Attributes["Label"].Value;
                string panelID = panel.Attributes["ID"].Value;
                foreach (XmlNode itemNode in panel.ChildNodes)
                {
                    foreach (XmlNode procNode in itemNode.ChildNodes)
                    {
                        if (procNode.Attributes["IsReport"].Value.Trim() == "True")
                        {
                            //if (!ddlReportParent.Items.Contains(new ListItem(panelLabel, panelID)))
                            //{
                            //    ddlReportParent.Items.Add(new ListItem(panelLabel, panelID));
                            //}
                        }
                    }
                }
            }
            //FillReports(ddlReportParent.SelectedValue);
            //ddlReportParent.Items.Insert(0, "Select");
            //ddlReportParent.SelectedIndex = 0;
            //ddlReport.Items.Insert(0, new ListItem("Select Report"));
            //ddlReport.SelectedIndex = 0;
            //ddlReport.Enabled = false;
        }

        /// <summary>
        /// Initialises the Left Panel Shortcut links.
        /// </summary>
        public void InitialiseShortcuts(XmlDocument XDocUserInfo)
        {
            string xPathShrtCuts = "GlobalBusinessProcessControls/menupanel//Panel//Item//Process[@IsShortcut='True']";
            string sortBy = "@Label";//@AttributeName

            TableRow trShrtCutsImg = new TableRow();
            System.Web.UI.WebControls.Image imgShrtCuts = new System.Web.UI.WebControls.Image();
            imgShrtCuts.ImageUrl = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/myshortcuts.gif";
            imgShrtCuts.Width = Unit.Pixel(147);

            //Clear the table before adding anything.
            tableShortcuts.Rows.Clear();
            TableCell tdShrtCutsImg = new TableCell();
            tdShrtCutsImg.Controls.Add(imgShrtCuts);
            trShrtCutsImg.Cells.Add(tdShrtCutsImg);
            tableShortcuts.Rows.Add(trShrtCutsImg);

            System.IO.StringReader strReaderGBPC = new System.IO.StringReader(XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml);
            XPathDocument xpDoc = new XPathDocument(strReaderGBPC);
            XPathNavigator xpNav = xpDoc.CreateNavigator();

            // Compile the XPath query expression to select all the ShortCut elements.
            // The Compile method of the XPathNavigator generates an XPathExpression 
            // object that encapsulates the compiled query.
            XPathExpression xpExpr = xpNav.Compile(xPathShrtCuts);

            // Execute the AddSort method of the XPathExpression object to define the 
            // Label Attribute as the sort key.
            xpExpr.AddSort(sortBy, XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text);

            // Create the XPathNodeIterator by executing the Select method of the XPathNavigator.
            XPathNodeIterator iterator = xpNav.Select(xpExpr);
            while (iterator.MoveNext())
            {
                XPathNavigator nodeShrtCut = iterator.Current;

                string hRef = "MainMenuItemClick('" + nodeShrtCut.GetAttribute("BPGID", "") + "','"
                                           + nodeShrtCut.GetAttribute("PageInfo", "") + "')";
                lblNewShrtCut = new Label();
                TableRow tr = new TableRow();
                tr.Width = Unit.Pixel(147);
                TableCell td = new TableCell();
                string innerText = nodeShrtCut.GetAttribute("Label", "").Trim();
                if (innerText.Contains(" "))
                {
                    innerText = innerText.Replace(" ", "");
                }
                //Make sure that the text fits into the cell without over-flowing else clip it and append dots.
                if (innerText.Length > 20)
                {
                    innerText = innerText.Substring(0, 18) + "..";
                }
                lblNewShrtCut.Text = "<a oncontextmenu='return false;' class=\"shourtcutslinks\" href=\"javascript:" + hRef + "\">" + innerText + "</a>";
                td.Controls.Add(lblNewShrtCut);
                td.CssClass = "shortcutsbg";
                td.Height = Unit.Pixel(20);
                td.Width = Unit.Pixel(147);
                td.Wrap = false;
                tr.Cells.Add(td);

                tableShortcuts.Rows.Add(tr);
                //
                string label1 = string.Empty;
                string label2 = string.Empty;
                string label2ID = string.Empty;
                string TotString = string.Empty;
                XmlNode xNode3 = XDocUserInfo.SelectSingleNode("//GlobalBusinessProcessControls/menupanel//Panel//Item//Process[@BPGID='" + nodeShrtCut.GetAttribute("BPGID", "").ToString() + "']");
                XmlDocument xDocs3 = new XmlDocument();
                XmlDocument xDocs2 = new XmlDocument();
                xDocs3.LoadXml(xNode3.ParentNode.OuterXml.ToString());
                label2 = xDocs3.SelectSingleNode("Item").Attributes["Label"].Value.ToString();
                label2ID = xDocs3.SelectSingleNode("Item").Attributes["ID"].Value.ToString();
                XmlNode xNode2 = XDocUserInfo.SelectSingleNode("//GlobalBusinessProcessControls/menupanel//Panel//Item[@ID='" + label2ID + "']");
                xDocs2.LoadXml(xNode2.ParentNode.OuterXml.ToString());
                label1 = xDocs2.SelectSingleNode("Panel").Attributes["Label"].Value.ToString();
                TotString = label1 + " >> " + label2;
                //
                string val = TotString;
                td.Attributes.Add("QTIP", val);
            }
        }

        //Parent
        protected void ddlReportParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillReports(ddlReportParent.SelectedValue);
            //if (ddlReportParent.SelectedItem.ToString() != "Select")
            //{
            //    ddlReport.Enabled = true;
            //}
            //else
            //{
            //    ddlReport.SelectedIndex = 0;
            //    ddlReport.Enabled = false;
            //}
        }

        //Child
        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FillReports(string panelID)
        {
            //if (ddlReport.Items.Count <= 1)
            //{
            //    string xPath = "//GlobalBusinessProcessControls/menupanel//Panel[@ID='" + panelID + "']//Item//Process[@IsReport='True']";
            //    XmlDocument xDocMenu = new XmlDocument();
            //    xDocMenu.LoadXml(Session["GBPC"].ToString());
            //    XmlNodeList nodeReports = xDocMenu.SelectNodes(xPath);

            //    foreach (XmlNode nodeReport in nodeReports)
            //    {
            //        string text = nodeReport.Attributes["Label"].Value;
            //        string nodeValue = nodeReport.Attributes["BPGID"].Value;
            //        ddlReport.Items.Add(new ListItem(text, nodeValue));
            //    }
            //}
        }

        //private string InitialiseMenuPanel(XmlDocument XDocUserInfo)
        //{
        //    xdsMenu.Data = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml);
        //    xdsMenu.XPath = "GlobalBusinessProcessControls/menupanel/Panel";
        //    mnuTopMenu.DataSourceID = "xdsMenu";

        //    MenuItemBinding mibPanel = new MenuItemBinding();
        //    mibPanel.DataMember = "Panel";
        //    mibPanel.ValueField = "ImgSrc";
        //    mibPanel.Selectable = false;

        //    MenuItemBinding mibItem = new MenuItemBinding();
        //    mibItem.DataMember = "Item";
        //    mibItem.TextField = "Label";
        //    mibItem.Selectable = false;

        //    MenuItemBinding mibProc = new MenuItemBinding();
        //    mibProc.DataMember = "Process";
        //    mibProc.TextField = "Label";

        //    mnuTopMenu.DataBindings.Add(mibPanel);
        //    mnuTopMenu.DataBindings.Add(mibItem);
        //    mnuTopMenu.DataBindings.Add(mibProc);

        //    //No Image was being sent for the panel so handling that case using try catch
        //    try
        //    {
        //        mnuTopMenu.DataBind();
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        mibPanel.ValueField = "Label";
        //        mnuTopMenu.DataBindings.Add(mibPanel);
        //    }
        //    return xdsMenu.Data;
        //}

        protected void mnuTopMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

            string redirectPage = string.Empty;
            string BPGID = string.Empty;
            GetNavigateString(e.Item.DataPath, out redirectPage, out BPGID);

            //if (redirectPage.Contains("vendor.aspx"))
            //{
            //    BPGID = "417";
            //    redirectPage = "Payables/PaymentRequest.aspx";
            //}
            //string script = "<script>OnMenuItemClick(" + BPGID + ",'" + redirectPage + "')</script>";
            //Page.RegisterClientScriptBlock("MenuClick", script);

            //Setting a cache variable
            //Cache.Insert("hdnBPGID", BPGID, null, System.Web.Caching.Cache.NoAbsoluteExpiration,TimeSpan.FromMinutes(10));
            Session["BPINFO"] = "<bpinfo><BPGID>" + BPGID + "</BPGID></bpinfo>";
            if (redirectPage != string.Empty)
            {
                Response.Redirect("../" + redirectPage);
            }
        }

        protected void mnuTopMenu_MenuItemBound(object sender, MenuEventArgs e)
        {
            if (e.Item.Depth == 0)
            {
                string currentImage = e.Item.Value;
                if (currentImage == "NoImage.png")
                {
                    currentImage = "documents-but.png";
                }
                //currentImage = currentImage.Replace(".jpg", ".gif");
                string imgName = "imgMenuPnl" + e.Item.ChildItems.Count + 1;
                e.Item.Text = "<img border=\"0\" alt=\"Full View\" title=\"Full View\" name=\"" + imgName + "\" src=\"" + Application["ImagesCDNPath"].ToString() + "Images/" + currentImage
                    + "\" onclick=\"RenderFullMenuView('" + e.Item.DataPath + "');\" style=\"cursor:pointer;\" onmouseout=\"toggleImage(this)\" onmouseover=\"toggleImage(this)\" />";
            }
            else if (e.Item.Depth == 2)
            {
                string redirectPage = string.Empty;
                string BPGID = string.Empty;
                GetNavigateString(e.Item.DataPath, out redirectPage, out BPGID);
                e.Item.NavigateUrl = "javascript:MainMenuItemClick(" + BPGID + ",'" + redirectPage + "');";
            }
            //if (e.Item.ImageUrl != string.Empty)
            //{
            //    e.Item.ImageUrl = "~/App_Themes/" + Session["MyTheme"].ToString() + "/Images/" + e.Item.ImageUrl;
            //}

        }

        private void GetNavigateString(string dataPath, out string redirectPage, out string BPGID)
        {

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            XmlDocument xDocPanel = new XmlDocument();
            xDocPanel.LoadXml(XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml);
            XmlNode selectedNode = xDocPanel.SelectSingleNode(dataPath);
            if (selectedNode.Attributes["PageInfo"] != null && selectedNode.Attributes["BPGID"] != null)
            {
                redirectPage = selectedNode.Attributes["PageInfo"].Value;
                BPGID = selectedNode.Attributes["BPGID"].Value;
            }
            else
            {
                redirectPage = string.Empty;
                BPGID = string.Empty;
            }
        }

        //private string GetNavigateString(string dataPath)
        //{
        //    XmlDocument xDocPanel = new XmlDocument();
        //    xDocPanel.LoadXml(Session["GBPC"].ToString());
        //    XmlNode selectedNode = xDocPanel.SelectSingleNode(dataPath);
        //    string redirectPage = string.Empty;
        //    if (selectedNode.Attributes["PageInfo"] != null && selectedNode.Attributes["BPGID"] != null)
        //    {
        //        redirectPage = selectedNode.Attributes["PageInfo"].Value;
        //        redirectPage += "?BPGID=" + selectedNode.Attributes["BPGID"].Value;
        //    }
        //    return redirectPage;
        //}

        protected void grdVwLeftPanel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
            if (drvCurrent != null)//m_IsFirstRun
            {
                m_IsFirstRun = false;
                InitialiseIndexVariables(drvCurrent);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Row Highlighting upon hover.
                if (m_IsRowAlternating)
                {
                    if (e.Row.RowState == DataControlRowState.Normal)
                    {
                        e.Row.CssClass = "GVRowHover";
                    }
                }
                else
                {
                    e.Row.CssClass = "GVRowHover";
                }

                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();

                if (m_HyperLinksEnabled.Length == 0)
                {  //FullViewHyperlinks
                    m_HyperLinksEnabled = GetUserPreferenceValue("58");
                }

                //Adding the row XML to the template fields.
                HiddenField hdnCurrentRow = (HiddenField)e.Row.Cells[0].FindControl("hdnRowInfo");
                if (hdnCurrentRow == null)
                {
                    hdnCurrentRow = new HiddenField();
                    hdnCurrentRow.ID = "hdnRowInfo";
                    e.Row.Cells[0].Controls.Add(hdnCurrentRow);
                }
                string rowXML = GetRowXml(drvCurrent.DataView.Table, primaryKeyValue);
                string rowXMLWithOuterNode = "<" + m_GVTreeNodeName + "><RowList>" + rowXML + "</RowList></" + m_GVTreeNodeName + ">";
                hdnCurrentRow.Value = rowXMLWithOuterNode;

                #region Tooltip functionality
                foreach (DictionaryEntry de in m_htGVColumns)
                {
                    string colName = de.Key.ToString();
                    int currentColIndex = Convert.ToInt32(de.Value);
                    int colLength = Convert.ToInt32(m_htGVColWidths[de.Value]);
                    TableCell tcCurrent = e.Row.Cells[currentColIndex];
                    if (tcCurrent.Controls.Count == 0)
                    {
                        //tcCurrent.Attributes.Add("Title", DataBinder.Eval(e.Row.DataItem, colName).ToString());
                        tcCurrent.ToolTip = DataBinder.Eval(e.Row.DataItem, colName).ToString();

                        if (tcCurrent.Text.StartsWith("<a") && tcCurrent.Text.EndsWith("</a>"))
                        {
                            int startIndex = tcCurrent.Text.IndexOf(">") + 1;
                            int endIndex = tcCurrent.Text.IndexOf("</a>", startIndex);
                            string strtoInsert = tcCurrent.Text.Substring(startIndex, endIndex - startIndex);
                            if (strtoInsert.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Remove(startIndex, endIndex - startIndex);
                                strtoInsert = strtoInsert.Substring(0, colLength - 3) + "...";
                                tcCurrent.Text = tcCurrent.Text.Insert(startIndex, strtoInsert);
                            }
                        }
                        else
                        {
                            if (tcCurrent.Text.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Substring(0, colLength - 3) + "...";
                            }
                        }
                    }
                }
                #endregion

                //If BusinessProcessControls node is absent or Show hyperlinks is false dont display the hyperlinks
                if (m_BPCXml != string.Empty && m_HyperLinksEnabled == "1")
                {
                    foreach (DictionaryEntry de in m_htBPCntrls)
                    {
                        string processName = de.Key.ToString();
                        //Index of the column to which BPC is assigned.
                        int currentBPCIndex = Convert.ToInt32(de.Value);
                        int grdVwColIndex = Convert.ToInt32(m_htBPCColumns[processName]);

                        //Calling Object functionality.
                        string BPCColName = GetColumnName(Convert.ToInt32(m_htBPCntrls[processName])
                                                            , drvCurrent.DataView.Table);
                        string TrxID = string.Empty;
                        string TrxType = string.Empty;
                        int indexOfColNameTrxID = GetColumnIndex(BPCColName + "_TrxID", drvCurrent.DataView.Table);
                        int indexOfColNameTrxType = GetColumnIndex(BPCColName + "_TrxType", drvCurrent.DataView.Table);
                        if (indexOfColNameTrxID != -1 && indexOfColNameTrxType != -1)
                        {
                            //ColumnName_TrxID and ColumnName_TrxType are present
                            TrxID = drvCurrent.Row.ItemArray[indexOfColNameTrxID].ToString();
                            TrxType = drvCurrent.Row.ItemArray[indexOfColNameTrxType].ToString();
                        }
                        else
                        {
                            //Take the normal TrxID and TrxType present at the first and second positions respectively.
                            TrxID = drvCurrent.Row.ItemArray[0].ToString();
                            TrxType = drvCurrent.Row.ItemArray[1].ToString();
                        }

                        //Try catch the below block to know whether the given process has an associated BPGID
                        //Exception occurs only in the GetBPCBPGID method
                        try
                        {
                            string currentBPGID = GetBPCBPGID(processName, m_BPCXml);
                            string pageInfo = GetBPCPageInfo(processName, m_BPCXml);
                            string isPopUp = GetBPCAttributeValue(processName, m_BPCXml, "IsPopup");
                            //Dont allow PopUps in DashBoard
                            if (isPopUp == "1" && Page.Request.Url.LocalPath.Contains("DashBoard"))
                            {
                                isPopUp = "1";
                            }
                            int BPCColIndex = GetColumnIndex(processName, drvCurrent.DataView.Table);
                            string processLink = string.Empty;//Specifies whether the cell should contain a link or not.
                            if (BPCColIndex == -1)
                            {
                                processLink = "1";
                            }
                            else
                            {
                                processLink = drvCurrent.Row.ItemArray[BPCColIndex].ToString();
                            }
                            if (processLink != "0")
                            {
                                string processLabel = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                string linkText = drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace("'", "\\'").Replace(" ", "~::~"); ;

                                //string str1 = System.Security.SecurityElement.Escape(linkText);
                                //string str2 = HttpUtility.HtmlEncode(linkText);
                                //string str3 = ObjCommonUI.HtmlEncode(linkText);

                                linkText = ObjCommonUI.HtmlEncode(linkText);


                                if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() != "1")
                                {
                                    e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].Wrap = false;
                                    //The +4 is for compensating the presence of an Image Field, the checkbox column ,radio button list, View Details field in the beginning
                                    e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                    //e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].ToolTip = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                }
                                else if (m_IndexOfIsProtected == -1)
                                {
                                    e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].Wrap = false;
                                    //The +4 is for compensating the presence of an Image Field, the checkbox column ,radio button list, View Details field in the beginning
                                    e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                    //e.Row.Cells[grdVwColIndex + m_NoOfGVStaticColumns].ToolTip = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            //Do nothing
                            //Dont show any link for the current row.
                        }
                    }
                }
            }
        }

        protected void grdVwLeftPanel2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add javascript to highlight row
                if (m_RowHoverColour.Length > 0)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:ChangeColor(this, true, '" + m_RowHoverColour + "');";
                    e.Row.Attributes["onmouseout"] = "javascript:ChangeColor(this, false, '');";
                }

                //For Tooltip functionality
                foreach (DictionaryEntry de in m_htGVColumns)
                {
                    string colName = de.Key.ToString();
                    int currentColIndex = Convert.ToInt32(de.Value);
                    int colLength = Convert.ToInt32(m_htGVColWidths[de.Value]);
                    TableCell tcCurrent = e.Row.Cells[currentColIndex];
                    if (tcCurrent.Controls.Count == 0)
                    {
                        //tcCurrent.Attributes.Add("Title", DataBinder.Eval(e.Row.DataItem, colName).ToString());
                        tcCurrent.ToolTip = DataBinder.Eval(e.Row.DataItem, colName).ToString();

                        if (tcCurrent.Text.StartsWith("<a") && tcCurrent.Text.EndsWith("</a>"))
                        {
                            int startIndex = tcCurrent.Text.IndexOf(">") + 1;
                            int endIndex = tcCurrent.Text.IndexOf("</a>", startIndex);
                            string strtoInsert = tcCurrent.Text.Substring(startIndex, endIndex - startIndex);
                            if (strtoInsert.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Remove(startIndex, endIndex - startIndex);
                                strtoInsert = strtoInsert.Substring(0, colLength - 3) + "...";
                                tcCurrent.Text = tcCurrent.Text.Insert(startIndex, strtoInsert);
                            }
                        }
                        else
                        {
                            if (tcCurrent.Text.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Substring(0, colLength - 3) + "...";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates the xml structure of the given row.
        /// </summary>
        /// <param name="dtGridData">The table which contains the row.</param>
        /// <param name="primaryKey">The unique key to identify the row.</param>
        /// <returns>xml string.</returns>
        private string GetRowXml(DataTable dtGridData, string primaryKey)
        {
            DataRow[] drFoundRows = dtGridData.Select(m_primaryKeyFieldName + "='" + primaryKey + "'");
            if (drFoundRows.Length == 0)
            {
                return string.Empty;
            }
            XmlDocument xDocRow = new XmlDocument();
            XmlNode nodeRow = xDocRow.CreateNode(XmlNodeType.Element, dtGridData.TableName, null);
            for (int i = 0; i < dtGridData.Columns.Count; i++)
            {
                XmlAttribute attCurrentCol = xDocRow.CreateAttribute(dtGridData.Columns[i].ColumnName);
                if (m_arrAmountCols.Contains(dtGridData.Columns[i].ColumnName))
                {
                    attCurrentCol.Value = drFoundRows[0][i].ToString().Replace(",", "");
                }
                else
                {
                    attCurrentCol.Value = drFoundRows[0][i].ToString();
                }
                nodeRow.Attributes.Append(attCurrentCol);
            }

            return nodeRow.OuterXml;
        }

        /// <summary>
        /// Get the name of the column at the given index in the column collection.
        /// </summary>
        /// <param name="grdVwColIndex">The postion where the column is required.</param>
        /// <param name="dataTable">The Datatable consisting of the column collection.</param>
        /// <returns>String column name.</returns>
        private string GetColumnName(int grdVwColIndex, DataTable dataTable)
        {
            return dataTable.Columns[grdVwColIndex].ColumnName;
        }

        /// <summary>
        /// Gets the BPGID of the Businees Process Control.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="bpcXml">The BPC node XML</param>
        /// <returns>string BPGID</returns>
        public string GetBPCBPGID(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Getting the BPGID of the Businees Process Name : " + processName);
            #endregion

            if (bpcXml.Trim().Length == 0)
            {
                //In Find mode when the "FIND" Bpeinfo is requested the Out XML no longer has the BPC node,so get it from the GVDataXML.
                BtnsUserCtrl = (UserControls.ButtonsUserControl)this.NamingContainer.FindControl("BtnsUC");
                XmlDocument xDocGVDataXML = new XmlDocument();
                if (BtnsUserCtrl != null)
                {
                    xDocGVDataXML.LoadXml(BtnsUserCtrl.GVDataXml);
                    XmlNode xNodeBPC = xDocGVDataXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                    if (xNodeBPC != null)
                    {
                        bpcXml = xNodeBPC.OuterXml;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            XmlNode nodeProcess = xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
            if (nodeProcess != null)
            {
                return nodeProcess.Attributes["BPGID"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the Page Info value from the given XML.
        /// </summary>
        /// <param name="processName">Process Name</param>
        /// <param name="bpcXml">Business Process Controls XML.</param>
        /// <returns></returns>
        private string GetBPCPageInfo(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Getting the Page Info value from the given XML for Process Name : " + processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["PageInfo"].Value;
        }

        /// <summary>
        /// Gets the value of the user preference from the given XML.
        /// </summary>
        /// <param name="ruleName">Preference name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        private string GetUserPreferenceValue(string bRuleID)
        {
            #region NLog
            logger.Info("Getting the value of the user preference for rule id : " + bRuleID + " from the given XML.");
            #endregion

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            //Getting the corresponding rule name ID from the config file.
            //string bRuleID = ConfigurationManager.AppSettings[ruleName];
            XmlNode xNodeUserPrefValue = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='"
                                  + bRuleID + "']");
            if (xNodeUserPrefValue != null)
            {
                return xNodeUserPrefValue.Attributes["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the value of the business rule from the given XML.
        /// </summary>
        /// <param name="ruleName">Rule name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        private string GetBusinessRulesValue(string ruleName, string businessRulesNode)
        {
            #region NLog
            logger.Info(" Getting the value of the business rule name : " + ruleName + " and business rule node : " + businessRulesNode + " from the given XML.");
            #endregion

            //Getting the corresponding rule name ID from the config file.
            string bRuleID = ConfigurationManager.AppSettings[ruleName];
            XmlDocument xDocBR = new XmlDocument();
            xDocBR.LoadXml(businessRulesNode);
            XmlNode xNodeBRV = xDocBR.SelectSingleNode("BusinessRules/BusinessRule[@TypeOfPreferenceID='"
                                   + bRuleID + "']");
            if (xNodeBRV != null)
            {
                return xNodeBRV.Attributes["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the attribute value for a given attribute name based on a process.
        /// </summary>
        /// <param name="processName">The Process name to look out for.</param>
        /// <param name="m_BPCXml">The BPC node XML</param>
        /// <param name="AttributeName">The attribute name in the row matching the process</param>
        /// <returns>Attribute Value</returns>
        private string GetBPCAttributeValue(string processName, string m_BPCXml, string AttributeName)
        {
            #region NLog
            logger.Info("Getting the attribute value for a given attribute name : " + AttributeName + " based on a process : " + processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(m_BPCXml);
            XmlNode nodeProcess = xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
            if (nodeProcess != null)
            {
                return nodeProcess.Attributes[AttributeName].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Initialises the index variables used for referring to various columsn in the data source.
        /// </summary>
        /// <param name="drvCurrent">DataRowView object of the row.</param>
        private void InitialiseIndexVariables(DataRowView drvCurrent)
        {
            if (drvCurrent.DataView.Table.Columns.Contains("IsProtected"))
            {
                m_IndexOfIsProtected = drvCurrent.DataView.Table.Columns["IsProtected"].Ordinal;
            }
            else
            {
                m_IndexOfIsProtected = -1;
            }
            if (drvCurrent.DataView.Table.Columns.Contains("IsActive"))
            {
                m_IndexOfIsActive = drvCurrent.DataView.Table.Columns["IsActive"].Ordinal;
            }
            else
            {
                m_IndexOfIsActive = -1;
            }
            if (drvCurrent.DataView.Table.Columns.Contains("TypeOfInactiveStatusID"))
            {
                m_IndexOfTypeOfInactiveStatusID = drvCurrent.DataView.Table.Columns["TypeOfInactiveStatusID"].Ordinal;
            }
            else
            {
                m_IndexOfTypeOfInactiveStatusID = -1;
            }
        }

        /// <summary>
        /// Timer tick event for the Left Panel GV refresh functionality.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        protected void timerLPUpdater_Tick(object sender, EventArgs e)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            InitialiseGridViews(XDocUserInfo);
        }

        protected void lnlbtnLogout_Click(object sender, EventArgs e)
        {
            logger.Debug("User has logged out. Abandoning session and redirecting to Login Page. ");
            Session.Abandon();
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now.AddDays(-1));

            Response.Redirect("../Common/Login.aspx");
        }

        protected void lnkBtnRefresh_Click(object sender, EventArgs e)
        {
            //string bpInfo = HttpContext.Current.Session["BPINFO"].ToString();
            Response.Redirect(Request.UrlReferrer.ToString());
        }

        protected void lnkBtnRefreshGrids_onClick(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            commonObjUI.InvokeOnButtonClick("PageLoad", this.Page);

            string js = "javascript:setTimeout('ExpandGrid()',50);";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ForceExpand", js, true);

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            InitialiseGridViews(XDocUserInfo);
        }

        //protected void lnkBtnClosePopUp_Click(object sender, EventArgs e)
        //{
        //    //Resetting the parent BPInfo to the page after closing the child(Page PopUP).
        //    mpePagePopUp.Hide();
        //    UserControls.ButtonsUserControl objBtnsUC = ((UserControls.ButtonsUserControl)Page.FindControl("cphPageContents"));
        //    HiddenField hdnParentBPInfo = (HiddenField)Page.FindControl("parentBPInfo");
        //    Session["BPInfo"] = hdnParentBPInfo.Value;

        //    objBtnsUC.InitialiseBranchObjects();
        //}

        protected void lnkBtnModRequest_Click(object sender, EventArgs e)
        {

            //XmlDocument xDocBPGID = new XmlDocument();
            //xDocBPGID.LoadXml(Session["GBPC"].ToString());
            //string process = "ModRequest";
            //XmlNode nodeModReq = xDocBPGID.SelectSingleNode("GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='" + process + "']");

            //string currentBPGID = nodeModReq.Attributes["BPGID"].Value;
            //string pageInfo = nodeModReq.Attributes["PageInfo"].Value;

            ////function OnMasterPgBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp)

            //string s = "javascript:OnMasterPgBPCLinkClick(currentBPGID,pageInfo,bpInfo);";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModRequest", s, true);
        }


        protected void lnkBtnCloseIFrame_Click(object sender, EventArgs e)
        {
            UserControls.ButtonsUserControl BtnsUC = (UserControls.ButtonsUserControl)cphPageContents.FindControl("BtnsUC");
            if (BtnsUC != null)
            {
                BtnsUC.OnCloseIframeClick(sender);
            }
        }





        #region ChangeRole Block
        /// <summary>
        /// Entity Company Roles Counts Hide/Show Controls and Fill dropdownlist data
        /// </summary>
        private void InitialiseRoles()
        {
            //Get Entity Company Roles Count
            string strRole = EntityCompanyRoleCount();
            string[] strRoles = strRole.Split(';');
            int EntityCount = Convert.ToInt32(strRoles[0]);
            int CompanyCount = Convert.ToInt32(strRoles[1]);
            int RoleCount = Convert.ToInt32(strRoles[2]);

            //Entity,Company,Role Counts are 1 then hide dropdownlist and table cell

            if ((EntityCount == 1) && (CompanyCount == 1) && (RoleCount == 1))
            {
                //Hide Company Label Dropdownlist and Related TableRows
                lblChangeRole.Visible = false;
                ddlEntity.Visible = false;
                trChangeRoleTitle.Visible = false;
                trChangeRoleDDL.Visible = false;
                trChangeRoleSpace.Visible = false;
                lnkBtnChangeRole.Visible = false;
                lnkBtnRemoveRole.Visible = false;
            }
            else
            {
                //Show Company Label Dropdownlist and Related TableRows
                lblChangeRole.Visible = true;
                ddlEntity.Visible = true;
                trChangeRoleTitle.Visible = true;
                trChangeRoleDDL.Visible = true;
                trChangeRoleSpace.Visible = true;
                lnkBtnChangeRole.Visible = true;
                lnkBtnRemoveRole.Visible = true;

                //string strRole = "OnMasterPgBPCLinkClick(' ','Common/SetCompany.aspx','','1','');return false;";
                //string ScriptRole = "CallBPCProcess('', 'Common/SetCompany.aspx', '', '1','');return false;";
                string strSetRole = "SetCompanyRole('SET');return false;";
                lnkBtnChangeRole.OnClientClick = strSetRole;
                string strRemoveRole = "SetCompanyRole('REMOVE');return false;";
                lnkBtnRemoveRole.OnClientClick = strRemoveRole;
                //Fill Role DropDowns
                FillRoleDropDowns(EntityCount, CompanyCount, RoleCount);
            }
        }

        /// <summary>
        /// Fill Roles DropDownlist Data
        /// </summary>
        /// <param name="EntityCount"></param>
        /// <param name="CompanyCount"></param>
        /// <param name="RoleCount"></param>
        private void FillRoleDropDowns(int EntityCount, int CompanyCount, int RoleCount)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            //Test
            //2 2
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"2\" UserRoleID=\"16\" RoleID=\"7\" Description=\"Assistant To The Regional Director\" Caption=\"Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity><Entity EntityID=\"3\" Description=\"CAPS  Gemini\" Caption=\"Proto Gemini\" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"3\" Description=\"Valuelabs\" Caption=\"CAPS VL\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"3\" UserRoleID=\"1\" RoleID=\"6\" Description=\"VL SEE (Full Access)\" Caption=\"SSE Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"3\" UserRoleID=\"16\" RoleID=\"7\" Description=\"SE Director\" Caption=\"SE Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //1 1
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //XDocUserInfo.LoadXml(test);

            //To Store all entityids
            ArrayList alEntity = new ArrayList();
            //ArrayList alDescription = new ArrayList();


            //PreSelect DropDown with Current Loged Role
            string LogInCompanyID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText;
            string LogInRoleID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/RoleID").InnerText;

            // Get Entities
            XmlNode nodeEntities = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities");
            foreach (XmlNode nodeEntity in nodeEntities.ChildNodes)
            {
                if ((nodeEntity.Attributes["EntityID"] != null) && (nodeEntity.Attributes["Description"] != null))
                {
                    alEntity.Add(nodeEntity.Attributes["EntityID"].Value);
                    //alDescription.Add(nodeEntity.Attributes["Description"].Value);
                }
            }

            //Get Comapnies
            for (int i = 0; i < alEntity.Count; i++)
            {
                XmlNode nodeEntity = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities/Entity[@EntityID='" + alEntity[i].ToString() + "']");
                XmlNode nodeCompanies = nodeEntity.SelectSingleNode("Companies");
                /*if (ddlEntity.Items.Count == 0)
                {
                    ddlEntity.Items.Add(new ListItem("Choose", "-1~1"));
                    reqEntity.Enabled = true;
                    reqEntity.ErrorMessage = "Company";
                    reqEntity.InitialValue = ddlEntity.Items[0].Value;
                    imgbtnRoleSubmit.ValidationGroup = "LAJITChangeRole";
                }*/

                string Roles = string.Empty;
                int currentIndex = 0;
                foreach (XmlNode nodeCompany in nodeCompanies.ChildNodes)
                {

                    //Get Roles
                    XmlNode nodeRoles = nodeCompany.SelectSingleNode("Roles");
                    foreach (XmlNode nodeRole in nodeRoles.ChildNodes)
                    {
                        if (CompanyCount == 1)
                        {
                            ddlEntity.Items.Add(new ListItem(nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["RoleID"].Value));
                        }
                        else if ((CompanyCount > 1) && (RoleCount == 1))
                        {
                            ddlEntity.Items.Add(new ListItem(nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["RoleID"].Value));
                            // ddlEntity.Items.Add(new ListItem(nodeRole.Attributes["Description"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                        }
                        else
                        {
                            ddlEntity.Items.Add(new ListItem(nodeCompany.Attributes["Description"].Value + "-" + nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["RoleID"].Value));
                            //ddlEntity.Items.Add(new ListItem(alDescription[i].ToString() + "-" + nodeCompany.Attributes["Description"].Value + "-" + nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                        }

                        //Pre Select LogIn Role
                        if ((LogInCompanyID == nodeRole.Attributes["RoleCompanyID"].Value) && (LogInRoleID == nodeRole.Attributes["RoleID"].Value))
                        {

                            string dataValueField = nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["RoleID"].Value;

                            ddlEntity.SelectedIndex = ddlEntity.Items.IndexOf(ddlEntity.Items.FindByValue(dataValueField));
                        }
                        currentIndex++;

                    }
                }
            }
        }
        /// <summary>
        /// Get Available Entity Company Role Counts
        /// </summary>
        /// <returns>string array</returns>
        private string EntityCompanyRoleCount()
        {
            int Entities = 0;
            int Companies = 0;
            int Roles = 0;

            string Result = string.Empty;

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

            //Test
            //2 2
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"2\" UserRoleID=\"16\" RoleID=\"7\" Description=\"Assistant To The Regional Director\" Caption=\"Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity><Entity EntityID=\"3\" Description=\"CAPS  Gemini\" Caption=\"Proto Gemini\" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"3\" Description=\"Valuelabs\" Caption=\"CAPS VL\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"3\" UserRoleID=\"1\" RoleID=\"6\" Description=\"VL SEE (Full Access)\" Caption=\"SSE Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"3\" UserRoleID=\"16\" RoleID=\"7\" Description=\"SE Director\" Caption=\"SE Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //1 1
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //XDocUserInfo.LoadXml(test);


            XmlNode nodeEntities = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities");

            foreach (XmlNode nodeEntity in nodeEntities.ChildNodes)
            {
                Entities = Entities + 1;

                XmlNode nodeCompanies = nodeEntity.SelectSingleNode("Companies");

                foreach (XmlNode nodeCompany in nodeCompanies.ChildNodes)
                {
                    Companies = Companies + 1;

                    XmlNode nodeRoles = nodeCompany.SelectSingleNode("Roles");

                    foreach (XmlNode nodeRole in nodeRoles.ChildNodes)
                    {
                        Roles = Roles + 1;
                    }
                }
            }

            Result = Entities.ToString() + ";" + Companies.ToString() + ";" + Roles.ToString();

            return Result;
        }

        /// <summary>
        ///  Navigating next page based on selected role
        /// </summary>
        /// <param name="RoleCompanyID"></param>
        /// <param name="UserRoleID"></param>
        private void RoleNavigation(string RoleCompanyID, string UserRoleID)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            string BPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Dashboard']").Attributes["BPGID"].Value;
            string PageInfo = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Dashboard']").Attributes["PageInfo"].Value;
            string navigatePage = "../" + PageInfo + "?rcId=" + RoleCompanyID + "&ruId=" + UserRoleID + "&bpgId=" + BPGID;
            Response.Redirect(navigatePage);
        }

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string RoleCompanyID = string.Empty;
            string UserRoleID = string.Empty;
            if (ddlEntity.Items.Count == 0)
            {
                //Single Role
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                XmlNode xnode = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities/Entity/Companies/Company/Roles/Role");
                if (xnode.Attributes["RoleCompanyID"] != null)
                {
                    RoleCompanyID = xnode.Attributes["RoleCompanyID"].Value;
                }

                if (xnode.Attributes["RoleID"] != null)
                {
                    UserRoleID = xnode.Attributes["RoleID"].Value;
                }
                RoleNavigation(RoleCompanyID, UserRoleID);
            }
            else
            {
                //Multiple Roles
                string[] strarr = ddlEntity.SelectedValue.Split('~');
                RoleCompanyID = strarr[0].ToString();
                UserRoleID = strarr[1].ToString();
                RoleNavigation(RoleCompanyID, UserRoleID);
            }
        }
        #endregion

        /// <summary>
        /// Get total rows in given xml
        /// </summary>
        /// <param name="returnXML"></param>
        /// <returns></returns>
        private int GetRowsCount(string returnXML)
        {
            XmlDocument XDocreturnXML = new XmlDocument();
            XDocreturnXML.LoadXml(returnXML);

            int rowsCount = 0;

            //Get the Grid Layout nodes
            string treeNodeName = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            //Get the rows
            XmlNode nodeRows = XDocreturnXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
            if (nodeRows != null)
            {
                rowsCount = nodeRows.ChildNodes.Count;
            }
            return rowsCount;
        }

    }
}