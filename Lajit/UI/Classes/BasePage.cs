using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using LAjit_BO;
using System.Reflection;
using System.Text;
using LAjitDev.UserControls;
using NLog;

namespace LAjitDev.Classes
{
    public class BasePage : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public string nlog = string.Empty;
        public CommonBO objBO = new CommonBO();
        public CommonUI commonObjUI = new CommonUI();
        public clsBranchUI objBranchUI = new clsBranchUI();
        //Setup the name of the hidden field on the client for storing the viewstate key
        public const string VIEW_STATE_FIELD_NAME = "__VIEWSTATE1";
        private static string m_RedirectUponSessionExpire = string.Empty;
        //Setup a formatter for the viewstate information
        private LosFormatter _formatter = null;
        public bool SetFocus = false;

        private bool m_IsHelpPage = false;

        public bool IsHelpPage
        {
            get { return m_IsHelpPage; }
            set { m_IsHelpPage = value; }
        }



        public BasePage()
        {
            if (string.IsNullOrEmpty(m_RedirectUponSessionExpire))
            {
                m_RedirectUponSessionExpire = ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"];
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Registering types for ajax methods.
                RegisterTypesForAjax();
            }

            //changing masterpage file dynamically
            string masterfile = string.Empty;
            if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
            {
                masterfile = "../MasterPages/PopUp.Master";
            }
            else
            {
                masterfile = "../MasterPages/TopLeft.Master";
            }
            if (!masterfile.Equals(string.Empty))
            {
                base.MasterPageFile = masterfile;
            }
            Page.Theme = (Convert.ToString(HttpContext.Current.Session["MyTheme"]));



            base.OnPreInit(e);

            //if (HttpContext.Current.Session["BPE"] != null)
            //{
            //    XmlDocument xDocUsrPref = new XmlDocument();
            //    xDocUsrPref.LoadXml(HttpContext.Current.Session["BPE"].ToString());
            //    string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
            //    if (xDocUsrPref.SelectSingleNode("bpe/companyinfo") != null)
            //    {
            //        string theme = xDocUsrPref.SelectSingleNode("bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
            //        Session.Add("MyTheme", theme);
            //    }
            //    Page.Theme = ((string)HttpContext.Current.Session["MyTheme"]);
            //}
        }

        /// <summary>
        /// Page initialisation event.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DoSessionValidation();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["NEWGUID"] = System.Guid.NewGuid().ToString();
            }
            objBranchUI.ObjCommonUI = commonObjUI;
            if (this.Master != null)
            {
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Master.FindControl("cphPageContents");
                UpdatePanel updPanel = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
                if (updPanel != null)
                {
                    //Assigning the ButtonsUC ans GridViewUC objects to the properties present in the CommonUI
                    commonObjUI.ButtonsUserControl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
                    commonObjUI.GridViewUserControl = (LAjitDev.GridViewControl)cntPlaceHolder.FindControl("GVUC");
                    commonObjUI.ChildGridUserControl = (ChildGridView)cntPlaceHolder.FindControl("CGVUC");
                    commonObjUI.UpdatePanelContent = updPanel;
                    commonObjUI.ButtonsUserControl.ObjCommonUI = commonObjUI;
                    //Entry Form Panel making invisible
                    Panel pnlEntryForm = (Panel)updPanel.FindControl("pnlEntryForm");
                    XmlDocument xDocOut = new XmlDocument();
                    if (!Page.IsPostBack)
                    {
                        //Error Panel making invisible
                        Panel pnlContentError = (Panel)updPanel.FindControl("pnlContentError");
                        if (pnlContentError != null)
                        {
                            pnlContentError.Visible = false;
                        }
                        //Calling the below method to bind the data for a given GridView or EntryForm depending on different scenarios.
                        BindPage(updPanel);

                        //Setting child grid columns autoFill data in Cache
                        if (!string.IsNullOrEmpty(commonObjUI.ButtonsUserControl.GVDataXml))
                        {
                            xDocOut.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
                            //Classes.AutoFill.SetBranchsAutoFillCache(xDocOut);
                        }



                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(commonObjUI.ButtonsUserControl.GVDataXml))
                        {
                            xDocOut.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
                        }
                    }


                    if (!string.IsNullOrEmpty(commonObjUI.ButtonsUserControl.GVDataXml))
                    {
                        SetUIHeaders(updPanel, commonObjUI.ButtonsUserControl, pnlEntryForm, xDocOut);
                    }

                    //updated code on 09-12-08  imgBtnSubmit.AlternateText = "Add"
                    HtmlTableRow trProcessLinks = (HtmlTableRow)cntPlaceHolder.FindControl("trProcessLinks");
                    Table tblBPCLinks = commonObjUI.GetBusinessProcessLinksTable(commonObjUI.ButtonsUserControl.GVDataXml);
                    if (tblBPCLinks.Rows.Count > 0)
                    {
                        if (tblBPCLinks.Rows[0].Cells.Count > 0)
                        {
                            //Get the Form-Level BPC links table and add it to the panel in the form.
                            Panel pnlBPCContainer = (Panel)cntPlaceHolder.FindControl("pnlBPCContainer");
                            pnlBPCContainer.Controls.Clear();
                            pnlBPCContainer.Controls.Add(tblBPCLinks);
                        }
                        else
                        {
                            trProcessLinks.Attributes.Add("style", "DISPLAY: none;");
                        }
                    }
                    else
                    {
                        trProcessLinks.Attributes.Add("style", "DISPLAY: none;");
                    }
                }
            }
            base.OnLoad(e);

        }

        /// <summary>
        /// Regiter the Classes which contain any Ajax Methods being used.
        /// </summary>
        private void RegisterTypesForAjax()
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxMethods));
            Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
        }

        public void SetUIHeaders(UpdatePanel updPanel, ButtonsUserControl BtnsUC, Panel pnlEntryForm, XmlDocument xDocOut)
        {
            #region NLog
                logger.Info("This method has been used to set the UI Headers in the BasePage");
            #endregion

            HtmlTableCell htcCPGV1 = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1");
            HtmlTableCell htcCPGV1Auto = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1Auto");
            //HtmlTableCell htcEntryForm = (HtmlTableCell)updPanel.FindControl("pnlEntryFormTitle").FindControl("htcEntryForm");
            //HtmlTableCell htcEntryFormAuto = (HtmlTableCell)updPanel.FindControl("pnlEntryFormTitle").FindControl("htcEntryFormAuto");
            ChildGridView CGVUC = commonObjUI.ChildGridUserControl;
            string treeNodeName = BtnsUC.TreeNode;
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title").InnerText;

            //Navigation Links functionality
            string navigationInfo = Context.Request["ctl00$cphPageContents$BtnsUC$hdnNavBPInfo"];
            if (navigationInfo == null || navigationInfo.Length == 0)
            {
                //Avoid showing the links in the Title
                commonObjUI.ShowLinksInTitle = false;
            }

            XmlNode nodePageSubject = xDocOut.SelectSingleNode("Root/bpeout/FormControls/PageSubject");
            if (nodePageSubject != null)
            {
                string pageSubject = commonObjUI.HtmlCodesToCharacters(nodePageSubject.InnerText);
                commonObjUI.SetPanelHeading(htcCPGV1, pageSubject + " >>> " + headerTitle);
                //commonObjUI.SetPanelHeading(htcEntryForm, pageSubject + " >>> " + headerTitle);
            }
            else
            {
                commonObjUI.SetPanelHeading(htcCPGV1, headerTitle);
                //commonObjUI.SetPanelHeading(htcEntryForm, headerTitle);
            }

            HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
            string strImagesCDNPath = (String)ctx.Application["ImagesCDNPath"];


            //Added on 16 Feb 2010 - Page Redesigning
            htcCPGV1Auto.InnerHtml = "<img alt='Spacer' src='" + Convert.ToString(strImagesCDNPath)
                    + "Images/spacer.gif' height='1' />";

            if (pnlEntryForm.Page.MasterPageFile.Contains("PopUp.Master"))//Pop Up
            {
                //Modified By Danny.
                //Originally 876 when width of vertical scrollbars were ignored
                int entryFormWidth = 860;//IframeWidth - BtnUCWidth -SpacerWidth-Vertical scrollbar width.
                int entryFormHeight = 483;//-24 for the title element
                int btnsUCHeight = 540; //473;

                ////string isPopUp = CurrentPage.Page.Request.QueryString["PopUp"];
                //string strDepth = pnlEntryForm.Page.Request.QueryString["depth"];
                //if (!string.IsNullOrEmpty(strDepth))
                //{
                //    int depth = Convert.ToInt32(strDepth);
                //    if (depth != 1 && depth != 0)
                //    {
                //        depth--;
                //        entryFormWidth = entryFormWidth - (depth * 30);
                //        entryFormHeight = entryFormHeight - (depth * 30);
                //    }
                //}

                ////Setting the header widths
                //htcCPGV1Auto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcCPGV1.Width) - 31);
                //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31);

                //Set the heights of Entry Form
                if ((pnlEntryForm.Page.ToString() != "ASP.financials_commercial_aspx") && (pnlEntryForm.Page.ToString() != "ASP.financials_profit_aspx"))
                {
                    if (this.ToString() == "ASP.financials_employee_aspx")
                    {
                        //entryFormWidth -= 20;//To accomodate the width of the vertical scollbar.
                        //pnlEntryForm.ScrollBars = ScrollBars.Vertical;
                        entryFormHeight += 25;
                    }
                    pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
                    //pnlEntryForm.Width = Unit.Pixel(entryFormWidth);
                }

                ////Find the immediate table in the Entry Panel
                //HtmlTable tblEntryForm = (HtmlTable)pnlEntryForm.FindControl("tblEntryForm");
                //if (tblEntryForm != null)
                //{
                //    tblEntryForm.Style["height"] = pnlEntryForm.Height.ToString();
                //    tblEntryForm.Style["width"] = Convert.ToString(pnlEntryForm.Width.Value - 1) + "px";
                //}

                ////BtnsUC
                //Panel pnlBtnsUCContainer = (Panel)BtnsUC.FindControl("pnlBtnsUC");
                //pnlBtnsUCContainer.Height = Unit.Pixel(btnsUCHeight);

                ////Set the widths of child panel, if present
                //if (CGVUC != null)
                //{
                //    if (this.ToString() != "ASP.financials_commercial_aspx" && this.ToString() != "ASP.financials_directorprofit_aspx")
                //    {
                //        CGVUC.SetPanelWidth(entryFormWidth);
                //    }
                //}

                InitialiseHelpLinks(htcCPGV1Auto, null);

                //Rename the close hyperlink in the Popup frame according to the header text.s
                string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + "');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ChangeCloseText", changeCloseJS, true);
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
            }
            //else
            //{
            //int entryFormWidth = 768;//TotalWindowWidth - BtnUCWidth -SpacerWidth
            ////If Left panel is collapsed the width increases by an amount equal to that of Left Panel
            //if (Convert.ToString(Session["LPCollapsed"]) == "1")
            //{
            //    entryFormWidth += 149;
            //}

            //htcCPGV1Auto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcCPGV1.Width) - 31);
            //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31);//50

            ////Set the widths of child panel, if present
            //Panel pnlGVBranch = (Panel)updPanel.FindControl("pnlGVBranch");
            //if (pnlGVBranch != null)
            //{
            //    if ((pnlGVBranch.Page.ToString() != "ASP.financials_commercial_aspx") && (pnlEntryForm.Page.ToString() != "ASP.financials_directorprofit_aspx"))
            //    {
            //        pnlGVBranch.Width = Unit.Pixel(entryFormWidth);
            //    }
            //}
            //if (CGVUC != null)
            //{
            //    if (this.ToString() != "ASP.financials_commercial_aspx" && this.ToString() != "ASP.financials_directorprofit_aspx")
            //    {
            //        CGVUC.SetPanelWidth(entryFormWidth);
            //    }
            //}

            //}
        }

        /// <summary>
        /// Adds Help links to the header title if help authoring is present.
        /// </summary>
        /// <param name="htcCPGV1Auto">Grid Title Element.</param>
        /// <param name="htcEntryFormAuto">Entry Form Title Element</param>
        /// <seealso cref="InitialiseHelpLinks() in Classes.PCBasePage"/>
        private void InitialiseHelpLinks(HtmlTableCell htcCPGV1Auto, HtmlTableCell htcEntryFormAuto)
        {
            if (!IsHelpPage)
            {
                XmlDocument xDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                XmlNode nodeBPC = xDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls");
                LinkButton lnkBtnHelpAuth = new LinkButton();

                Table tblHelpLinks = new Table();

                TableRow trhelp = new TableRow();



                //Help 
                string helpID = "Help";
                XmlNode nodeHelp = nodeBPC.SelectSingleNode("BusinessProcess[@ID='" + helpID + "']");
                LinkButton lnkBtnHelp = new LinkButton();
                if (nodeHelp != null)
                {
                    string pageInfo = nodeHelp.Attributes["PageInfo"].Value;
                    string isPopUp = "1";
                    string clientHelpDetails = "g_HelpAuth='" + pageInfo + "~" + isPopUp + "';";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Help", clientHelpDetails, true);
                    string script = "javascript:OpenHelpModalPopup('" + isPopUp + "','" + pageInfo + "',event);return false;";
                    lnkBtnHelp.OnClientClick = script;
                    lnkBtnHelp.Text = nodeHelp.Attributes["Label"].Value;
                    lnkBtnHelp.CssClass = "topboldlinks";

                    TableCell tdhelp = new TableCell();
                    tdhelp.Controls.Add(lnkBtnHelp);
                    trhelp.Controls.Add(tdhelp);
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
                    lnkBtnHelpAuth.CssClass = "topboldlinks";

                    if (nodeHelp != null)
                    {
                        TableCell tdSeparator = new TableCell();
                        tdSeparator.Width = Unit.Pixel(5);
                        tdSeparator.Text = "|";
                        trhelp.Cells.Add(tdSeparator);
                    }

                    TableCell tdhelpAuth = new TableCell();
                    tdhelpAuth.Controls.Add(lnkBtnHelpAuth);
                    trhelp.Controls.Add(tdhelpAuth);
                }

                tblHelpLinks.Rows.Add(trhelp);

                if (tblHelpLinks.Rows[0].Cells.Count > 0)
                {

                    if (htcCPGV1Auto.Parent.Visible)
                    {
                        htcCPGV1Auto.Align = "right";
                        htcCPGV1Auto.Controls.Add(tblHelpLinks);
                    }
                    //else
                    //{
                    //    htcEntryFormAuto.Align = "right";
                    //    htcEntryFormAuto.Controls.Add(tblHelpLinks);
                    //}
                }

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            AddScriptReferences();
            ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Master.FindControl("cphPageContents");
            UpdatePanel updPanel = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");

            if (updPanel != null)
            {
                Panel pnlEntryForm = (Panel)updPanel.FindControl("pnlEntryForm");
                if (pnlEntryForm != null)
                {
                    //bind parent and branch jquery contorls
                    BindJQueryControls(pnlEntryForm.Page);
                    //bind child grid view jquery controls
                    BindChildGridJQueryControls(pnlEntryForm.Page);
                }
            }
            else//masterpage
            {
                //RegisterJQueryHoverMenu();
            }
            ScriptManager masterScriptMngr = ScriptManager.GetCurrent(this.Page);
            ScriptReference sRefAjax = new ScriptReference();
            sRefAjax.Name = "jquery.clean.js";
            sRefAjax.ScriptMode = ScriptMode.Auto;
            //sRefAjax.Path = "../JavaScript/jquery.clean.js";
            sRefAjax.Path = Application["ScriptsCDNPath"].ToString() + "jquery.clean.js";
            base.OnPreRender(e);
        }

        #region Methods

        /// <summary>
        /// Does all the Session Validations like Session-Expire, Session is null both on the client 
        /// and server sides.
        /// </summary>
        private void DoSessionValidation()
        {

            #region NLog
                logger.Info("Initialising script to handle Session-Expire on the Client-side.");
            #endregion
            //If relevant sessions have not been initialised, redirect the user to the Login Page.
            //To handle the case where the user directly types in a page name other than Login and ends up with an error.
            if (HttpContext.Current.Session["USERINFOXML"] == null && Context.Session != null && !HttpContext.Current.Session.IsNewSession)
            {
                Response.Redirect("../Common/Login.aspx");
            }

            if (m_RedirectUponSessionExpire == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
            }

            if (Context.Session != null)
            {
                //Tested and the IsNewSession is more advanced then simply checking if 
                // a cookie is present, it does take into account a session timeout,
                if (Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must 
                    // have timed out.
                    string szCookieHeader = Request.Headers["Cookie"];
                    if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        Response.Redirect("../Common/SessionExpire.aspx");
                    }
                }
            }
        }



        /// <summary>
        /// To overcome the bug in Safari, add the custom scripts to the script manager.
        /// </summary>
        public void AddScriptReferences()
        {
            ScriptManager masterScriptMngr = ScriptManager.GetCurrent(this.Page);
            if (masterScriptMngr != null && Request.Browser.Browser.Contains("Safari"))
            {
                ScriptReference sRefAjax = new ScriptReference();
                sRefAjax.Name = "MicrosoftAjax.js";
                sRefAjax.ScriptMode = ScriptMode.Auto;
                //sRefAjax.Path = "../JavaScript/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.js";
                sRefAjax.Path = Application["ScriptsCDNPath"].ToString() + "System.Web.Extensions/1.0.61025.0/MicrosoftAjax.js";

                ScriptReference sRefAjaxDebug = new ScriptReference();
                sRefAjaxDebug.Name = "MicrosoftAjax.debug.js";
                sRefAjaxDebug.ScriptMode = ScriptMode.Auto;
                //sRefAjaxDebug.Path = "../JavaScript/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js";
                sRefAjaxDebug.Path = Application["ScriptsCDNPath"].ToString() + "System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js";

                masterScriptMngr.Scripts.Add(sRefAjax);
                masterScriptMngr.Scripts.Add(sRefAjaxDebug);
            }
        }

        /// <summary>
        /// Binds the data for a given GridView or EntryForm depending on different scenarios.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void BindPage(Control updPanel)
        {

            #region NLog
                logger.Info("Binds the data for a given GridView or EntryForm depending on different scenarios.");
            #endregion

            //Find out whether the page is being displayed in a popup or not.
            string popUp = Page.Request.QueryString["PopUp"];
            bool isPopUp = false;
            if (!string.IsNullOrEmpty(popUp) && popUp == "PopUp")
            {
                isPopUp = true;
            }

            string navigationBPInfo = Context.Request["ctl00$cphPageContents$BtnsUC$hdnNavBPInfo"];
            if (!string.IsNullOrEmpty(navigationBPInfo))
            {
                HtmlInputHidden navBPInfo = (HtmlInputHidden)commonObjUI.ButtonsUserControl.FindControl("hdnNavBPInfo");
                if (navBPInfo != null)
                {
                    navBPInfo.Value = navigationBPInfo;
                }
            }

            //Setting the BPInfo when the data is posted
            string BPInfo = string.Empty;
            if (!string.IsNullOrEmpty(Context.Request["hdnBPInfo"]))
            {
                BPInfo = Context.Request["hdnBPInfo"];
            }
            else if (isPopUp)
            {
                //only for pages whose master page is 'PopUp', we need to take Session 'LinkBPinfo'.
                if (HttpContext.Current.Session["LinkBPinfo"] != null)
                {
                    //setting the  Session for childpage.
                    BPInfo = HttpContext.Current.Session["LinkBPinfo"].ToString();
                }
            }
            else
            {
                BPInfo = HttpContext.Current.Session["BPINFO"].ToString();
            }

            string methodBpInfo = string.Empty;
            HiddenField hdnMasterBPIn = null;
            if (BPInfo.StartsWith("1"))
            {
                methodBpInfo = BPInfo.Substring(1, BPInfo.Length - 1);
                //Extract only the BPGID node
                XmlDocument xDocMethodInfo = new XmlDocument();
                xDocMethodInfo.LoadXml(methodBpInfo);
                BPInfo = "<bpinfo>" + xDocMethodInfo.SelectSingleNode("bpinfo/BPGID").OuterXml + "</bpinfo>";
                if (commonObjUI.GridViewUserControl != null)
                {
                    hdnMasterBPIn = (HiddenField)commonObjUI.GridViewUserControl.FindControl("hdnMasterBPIn");
                }
            }
            else
            {
                methodBpInfo = BPInfo;
            }

            //Set the Session BPInfo only if the page is not a pop up page-25th May 2009.
            if (!isPopUp)
            {
                if (BPInfo != string.Empty)
                {
                    HttpContext.Current.Session["BPINFO"] = BPInfo;
                    HttpContext.Current.Session["LinkBPinfo"] = BPInfo;
                }
                else
                {
                    //If BPInfo is still found to be empty then use the Bpinfo present in SessionBPInfo.
                    BPInfo = Convert.ToString(HttpContext.Current.Session["BPINFO"]);
                }
            }

            //Converting  & symbol to &amp;
            methodBpInfo = methodBpInfo.Replace("&", "&amp;");

            //Passing the BPInfo and Content Page Update panel as parameters to bind the page
            commonObjUI.BindPage(methodBpInfo, updPanel);

            //Reset the hdnMasterBPIn variable to the BPInfo value so that this is requested when the page is refreshed.
            if (hdnMasterBPIn != null)
            {
                hdnMasterBPIn.Value = BPInfo;
            }

            //Calling Page specific method
            MethodInfo minfo = this.Page.GetType().GetMethod("PageSpecific");
            if (minfo != null)
            {
                minfo.Invoke(this.Page, new object[] { null });
            }
        }
        #endregion

        #region ViewState Management

        ////overriding method of Page class
        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    string i = this.Page.MasterPageFile;
        //    //If server side enabled use it, otherwise use original base class implementation
        //    if (true == viewStateSvrMgr.GetViewStateSvrMgr().ServerSideEnabled && this.Page.MasterPageFile == "../MasterPages/TopLeft.Master")
        //    {
        //        return LoadViewState();
        //    }
        //    else
        //    {
        //        return base.LoadPageStateFromPersistenceMedium();
        //    }
        //}

        ////overriding method of Page class
        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    if (true == viewStateSvrMgr.GetViewStateSvrMgr().ServerSideEnabled && this.Page.MasterPageFile == "../MasterPages/TopLeft.Master")
        //    {
        //        SaveViewState(viewState);
        //    }
        //    else
        //    {
        //        base.SavePageStateToPersistenceMedium(viewState);
        //    }
        //}

        ////implementation of method
        //private object LoadViewState()
        //{
        //    if (_formatter == null)
        //    {
        //        _formatter = new LosFormatter();
        //    }

        //    //Check if the client has form field that stores request key
        //    if (null == Request.Form[VIEW_STATE_FIELD_NAME])
        //    {
        //        //Did not see form field for viewstate, return null to try to continue (could log event...)
        //        return null;
        //    }

        //    //Make sure it can be converted to request number (in case of corruption)
        //    long lRequestNumber = 0;
        //    try
        //    {
        //        lRequestNumber = Convert.ToInt64(Request.Form[VIEW_STATE_FIELD_NAME]);
        //    }
        //    catch
        //    {
        //        //Could not covert to request number, return null (could log event...)
        //        return null;
        //    }

        //    //Get the viewstate for this page
        //    string _viewState = viewStateSvrMgr.GetViewStateSvrMgr().GetViewState(lRequestNumber);

        //    //If find the viewstate on the server, convert it so ASP.Net can use it
        //    if (_viewState == null)
        //        return null;
        //    else
        //        return _formatter.Deserialize(_viewState);
        //}


        ////implementation of method
        //private void SaveViewState(object viewState)
        //{
        //    if (_formatter == null)
        //    {
        //        _formatter = new LosFormatter();
        //    }

        //    //Save the viewstate information
        //    StringBuilder _viewState = new StringBuilder();
        //    StringWriter _writer = new StringWriter(_viewState);
        //    _formatter.Serialize(_writer, viewState);
        //    long lRequestNumber = viewStateSvrMgr.GetViewStateSvrMgr().SaveViewState(_viewState.ToString());

        //    //Need to register the viewstate hidden field (must be present or postback things don't 
        //    // work, we use in our case to store the request number)
        //    RegisterHiddenField(VIEW_STATE_FIELD_NAME, lRequestNumber.ToString());
        //}

        #endregion

        public void BindJQueryControls(Page page)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("function JQueryEvents(){");
            //sb.Append("var $j = jQuery.noConflict();");
            //Autocomplete default focus
            //sb.Append("$j('input[type=text]').focus(function(){this.select();});");

            //JQUERY CollapasablePanel Extender
            if (commonObjUI.TitleID != null && commonObjUI.ContentID != null && commonObjUI.ImageID != null)
            {
                sb.Append(JQueryUI.RegisterJQueryCollapsableExtender(page, commonObjUI.TitleID, commonObjUI.ContentID, commonObjUI.ImageID));
            }
            //JQUERY calculator
            if (commonObjUI.m_alCalcTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryCalculator(page, commonObjUI.m_alCalcTBoxIDS));
            }
            //JQUERY datepicker
            if (commonObjUI.m_alCalendarTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryDatePicker(page, commonObjUI.m_alCalendarTBoxIDS, "mm/dd/y", "Parent"));
            }
            //JQUERY maskeditor for calculator
            if (commonObjUI.m_alMaskTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryMaskEditor(page, commonObjUI.m_alMaskTBoxIDS, "99/99/99"));
            }
            //JQUERY maskeditor for phone
            if (commonObjUI.m_alPhoneMaskTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryMaskEditor(page, commonObjUI.m_alPhoneMaskTBoxIDS, "(999) 999-9999? Ex:99999"));
            }

            //JQUERY autocomplete
            if (commonObjUI.m_htAutoCompleteTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryAutoComplete(page, commonObjUI.m_htAutoCompleteTBoxIDS, string.Empty));
            }

            sb.Append("}");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
        }



        public void BindChildGridJQueryControls(Page page)
        {
            StringBuilder sb = new StringBuilder();
            //JQUERY autocomplete for child grid only
            if (commonObjUI.m_htCGAutoCompleteTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryAutoComplete(page, commonObjUI.m_htCGAutoCompleteTBoxIDS, GetChildGridAmountColumn()));
            }
            //JQUERY datepicker for child grid only
            if (commonObjUI.m_alCGCalendarTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryDatePicker(page, commonObjUI.m_alCGCalendarTBoxIDS, "mm/dd/y", string.Empty));
            }
            //JQUERY maskeditor for child grid only
            if (commonObjUI.m_alCGMaskTBoxIDS.Count > 0)
            {
                sb.Append(JQueryUI.RegisterJQueryMaskEditor(page, commonObjUI.m_alCGMaskTBoxIDS, "99/99/99"));
            }
            ScriptManager.RegisterStartupScript(page.Page, page.GetType(), "JQueryChildGridEvents", sb.ToString(), true);
        }

        //public void RegisterJQueryHoverMenu()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("$(document).ready(function(){$('img').imghover({suffix: '-hovered'});});");
        //    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
        //}


        /// <summary>
        /// To Get AmountColumn field in Child Grid to set attribute own from autofill SelectCustInvoice item
        /// added on 16-11-09
        /// </summary>
        /// <returns></returns>
        public string GetChildGridAmountColumn()
        {

            #region NLog
                logger.Info("To Get AmountColumn field in Child Grid to set attribute own from autofill SelectCustInvoice item");
            #endregion

            XmlDocument xDocOut = new XmlDocument();
            string strColumnLabel = string.Empty;
            if (!string.IsNullOrEmpty(commonObjUI.ButtonsUserControl.GVDataXml))
            {
                xDocOut.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
                XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/AccountingItem/GridHeading/Columns");
                if (nodeColumns != null)
                {
                    foreach (XmlNode colnode in nodeColumns.ChildNodes)
                    {
                        if ((colnode.Attributes["FullViewLength"].Value.Trim() != "0"))
                        {

                            if (colnode.Attributes["IsSummed"] != null)
                            {
                                if (colnode.Attributes["IsSummed"].Value == "1")
                                {
                                    if (colnode.Attributes["BalanceMethod"] != null)
                                    {
                                        if (colnode.Attributes["BalanceMethod"].Value.Trim() != "0")
                                        {
                                            strColumnLabel = colnode.Attributes["Label"].Value;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return strColumnLabel;
        }



    }
}
