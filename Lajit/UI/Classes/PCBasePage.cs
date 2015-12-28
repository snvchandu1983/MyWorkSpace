using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using LAjitControls;
using LAjit_BO;
using System.Reflection;
using System.Text;
using System.IO;
using NLog;


namespace LAjitDev.Classes
{
    public class PCBasePage : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public CommonBO objBO = new CommonBO();
        public CommonUI commonObjUI = new CommonUI();
        public clsBranchUI objBranchUI = new clsBranchUI();
        //Setup the name of the hidden field on the client for storing the viewstate key
        public const string VIEW_STATE_FIELD_NAME = "__VIEWSTATE1";
        private static string m_RedirectUponSessionExpire = string.Empty;
        //Setup a formatter for the viewstate information
        private LosFormatter _formatter = null;
        public bool SetFocus = false;

        public PCBasePage()
        {
            if (string.IsNullOrEmpty(m_RedirectUponSessionExpire))
            {
                m_RedirectUponSessionExpire = ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"];
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            RegisterTypesForAjax();
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
            Page.Theme = ((string)HttpContext.Current.Session["MyTheme"]);
            base.OnPreInit(e);
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

        protected override void OnLoad(EventArgs e)
        {
            objBranchUI.ObjCommonUI = commonObjUI;
            if (this.Master != null)
            {
                if (this.Page.MasterPageFile.Contains("TopLeft.Master"))
                {
                    //setting the images for cpeLeftPanel
                    /*AjaxControlToolkit.CollapsiblePanelExtender cpeLeftPanel = (AjaxControlToolkit.CollapsiblePanelExtender)this.Master.FindControl("cpeLeftPanel");
                    cpeLeftPanel.CollapsedImage = "../App_Themes/" + Convert.ToString(Session["MyTheme"]) + "/Images/open-arrow.gif";
                    cpeLeftPanel.ExpandedImage = "../App_Themes/" + Convert.ToString(Session["MyTheme"]) + "/Images/close-arrow.gif";*/
                }
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Master.FindControl("cphPageContents");
                UpdatePanel updPanel = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
                if (updPanel != null)
                {
                    //Instantiated CommonObjUI object in Base Page
                    //Assigning the ButtonsUC ans GridViewUC objects to the properties present in the CommonUI
                    UserControl BtnsUC = (UserControl)cntPlaceHolder.FindControl("BtnsUC");
                    UserControl PCGVUC = (UserControl)cntPlaceHolder.FindControl("PCGVUC");
                    commonObjUI.ButtonsUserControl = (LAjitDev.UserControls.ButtonsUserControl)BtnsUC;
                    commonObjUI.ParentChildGVUC = (LAjitDev.ParentChildGV)PCGVUC;
                    commonObjUI.UpdatePanelContent = updPanel;
                    commonObjUI.ButtonsUserControl.ObjCommonUI = commonObjUI;
                    //Entry Form Panel making invisible
                    Panel pnlEntryForm = (Panel)updPanel.FindControl("pnlEntryForm");
                    if (!Page.IsPostBack)
                    {
                        //pnlEntryForm.ScrollBars = ScrollBars.Vertical;//Changed by Danny from Auto.                      

                        //Error Panel making invisible
                        Panel pnlContentError = (Panel)updPanel.FindControl("pnlContentError");
                        if (pnlContentError != null)
                        {
                            pnlContentError.Visible = false;
                        }
                        //Calling the below method to bind the data for a given GridView or EntryForm depending on different scenarios.
                        BindPage(updPanel);

                        //Setting child grid columns autoFill data in Cache
                        if (Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != null && Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != string.Empty)
                        {
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml.ToString());
                            //Classes.AutoFill.SetChildGridAutoFillCache(xDoc);
                            //Classes.AutoFill.SetBranchsAutoFillCache(xDoc);
                        }
                    }
                    else
                    {
                        ImageButton imgContinueAddBtn = (ImageButton)updPanel.FindControl("imgbtnContinueAdd");
                        imgContinueAddBtn.AlternateText = string.Empty;
                    }

                    XmlDocument xDocOut = new XmlDocument();
                    if ((((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).GVDataXml) != null && (((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).GVDataXml) != string.Empty)
                    {
                        xDocOut.LoadXml(((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).GVDataXml);
                    }

                    Control ctrlPostBack = CommonUI.GetPostBackControl(this);
                    //|| ctrlPostBack.ID.Contains("imgbtnAttachment") || ctrlPostBack.ID.Contains("imgbtnSecure") || ctrlPostBack.ID.Contains("imgbtnNote") || ctrlPostBack.ID.Contains("imgbtnPrint") 
                    //|| ctrlPostBack.ID.Contains("Cancel")No need of binding the grid on Cancel as grid is hidden off anyway.
                    if (ctrlPostBack != null && (ctrlPostBack.ID.Contains("AddNewRow") || ctrlPostBack.ID.Contains("Submit") || ctrlPostBack.ID.Contains("timer")
                            || ctrlPostBack.ID.Contains("Cancel") || ctrlPostBack.ID.Contains("lnkBtn") || ctrlPostBack.ID.Contains("ContinueAdd") || ctrlPostBack.ID.Contains("grdVwContents") || ctrlPostBack.ID.Contains("lbtn") || ctrlPostBack.ID.Contains("ddlPages") || ctrlPostBack.ID.Contains("ddlReport") || ctrlPostBack.ID.Contains("imgbtnIsApproved") || ctrlPostBack.ID.Contains("imgBtnSelect")) && !ctrlPostBack.ID.Contains("lnkBtnLogout"))
                    {
                        commonObjUI.InitialiseBranchObjects(xDocOut, pnlEntryForm);
                    }

                    //Get the Form-Level BPC links table and add it to the panel in the form.
                    Panel pnlBPCContainer = (Panel)cntPlaceHolder.FindControl("pnlBPCContainer");
                    pnlBPCContainer.Controls.Clear();
                    pnlBPCContainer.Controls.Add(commonObjUI.GetBusinessProcessLinksTable(commonObjUI.ButtonsUserControl.GVDataXml));

                    if (((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).GVDataXml != null && ((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).GVDataXml != string.Empty)
                    {
                        SetUIHeaders(updPanel, BtnsUC, pnlEntryForm, xDocOut);
                    }
                }
            }
            base.OnLoad(e);

        }

        private void RegisterTypesForAjax()
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxMethods));
            Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
            //Ajax.Utility.RegisterTypeForAjax(typeof(LAjitDev.UserControls.ButtonsUserControl));
            //Ajax.Utility.RegisterTypeForAjax(typeof(LAjitDev.Common.ProcessEngine));
            //Ajax.Utility.RegisterTypeForAjax(typeof(LAjitDev.Common.UploadText));
        }

        /// <summary>
        /// Sets the dimensions of UI elements such as Tab Heading and UI Form dimensions etc.,
        /// </summary>
        /// <param name="updPanel">Page Update Panel.</param>
        /// <param name="BtnsUC">Page BTNSUC.</param>
        /// <param name="pnlEntryForm">Page Entry Form Panel.</param>
        /// <param name="xDocOut">Page XML Doc.</param>
        private void SetUIHeaders(UpdatePanel updPanel, UserControl BtnsUC, Panel pnlEntryForm, XmlDocument xDocOut)
        {
            #region NLog
            logger.Info("Sets the dimensions of UI elements such as Tab Heading and UI Form dimensions etc."); 
            #endregion

            HtmlTableCell htcCPGV1 = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1");
            HtmlTableCell htcCPGV1Auto = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1Auto");

            //HtmlTableCell htcEntryForm = (HtmlTableCell)updPanel.FindControl("pnlEntryFormTitle").FindControl("htcEntryForm");
            //HtmlTableCell htcEntryFormAuto = (HtmlTableCell)updPanel.FindControl("pnlEntryFormTitle").FindControl("htcEntryFormAuto");
            UserControls.ChildGridView CGVUC = (UserControls.ChildGridView)updPanel.FindControl("CGVUC");
            string m_treeNodeName = ((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).TreeNode;
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Title").InnerText;

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
            if (((LAjitDev.UserControls.ButtonsUserControl)BtnsUC).Page.MasterPageFile.Contains("PopUp.Master"))
            {
                //Modified By Danny.
                //int entryFormWidth = 764;//IframeWidth - BtnUCWidth -SpacerWidth.
                int entryFormWidth = 860;
                int entryFormHeight = 483;//-24 for the title element
                int btnsUCHeight = 473;

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
                //htcCPGV1Auto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcCPGV1.Width) - 31 + 60);
                //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31 + 60);

                //Set the heights of Entry Form
                if ((pnlEntryForm.Page.ToString() != "ASP.financials_commercial_aspx") && (pnlEntryForm.Page.ToString() != "ASP.financials_profit_aspx"))
                {
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

                InitialiseHelpLinks(htcCPGV1Auto, null);

                //Rename the close hyperlink in the Popup frame according to the header text.s
                string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + "');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ChangeCloseText", changeCloseJS, true);
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
            }
            else
            {
                //htcCPGV1Auto.Width = Convert.ToString(768 - Convert.ToInt32(htcCPGV1.Width) - 31);
                //htcEntryFormAuto.Width = Convert.ToString(825 - Convert.ToInt32(htcEntryForm.Width) - 50);

                int entryFormWidth = 768;//TotalWindowWidth - BtnUCWidth -SpacerWidth
                //If Left panel is collapsed the width increases by an amount equal to that of Left Panel
                if (Convert.ToString(Session["LPCollapsed"]) == "1")
                {
                    entryFormWidth += 149;
                }

                htcCPGV1Auto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcCPGV1.Width) - 31);
                //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31);//50

                //Set the widths of child panel, if present
                if (CGVUC != null)
                {
                    if (this.ToString() != "ASP.financials_commercial_aspx" && this.ToString() != "ASP.financials_directorprofit_aspx")
                    {
                        CGVUC.SetPanelWidth(entryFormWidth);
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            AddScriptReferences();
            ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Master.FindControl("cphPageContents");
            UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
            if (cntrlUpdtPnl != null)
            {
                Panel pnlEntryForm = (Panel)cntrlUpdtPnl.FindControl("pnlEntryForm");

                SetFocusFirstControl(pnlEntryForm);
            }
            base.OnPreRender(e);
        }

        #region Methods

        /// <summary>
        /// Adds Help links to the header title if help authoring is present.
        /// </summary>
        /// <param name="htcCPGV1Auto">Grid Title Element.</param>
        /// <param name="htcEntryFormAuto">Entry Form Title Element</param>
        private void InitialiseHelpLinks(HtmlTableCell htcCPGV1Auto, HtmlTableCell htcEntryFormAuto)
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

        /// <summary>
        /// Setting focus on first control in the page added on 16-10-08
        /// </summary>
        /// <param name="c">EntryPanel</param>
        public void SetFocusFirstControl(Control c)
        {
            System.Web.UI.ScriptManager scrmgr = (System.Web.UI.ScriptManager)this.Page.Master.FindControl("topLeftScriptMngr");

            foreach (Control ctrl in c.Controls)
            {
                if (ctrl is LAjitTextBox)
                {
                    if (((LAjitTextBox)ctrl).MapXML != null)
                    {
                        scrmgr.SetFocus(((LAjitTextBox)ctrl));
                        SetFocus = true;
                        break;
                    }
                }
                else if (ctrl is LAjitDropDownList)
                {
                    if (((LAjitDropDownList)ctrl).MapXML.ToString() != string.Empty)
                    {
                        scrmgr.SetFocus(((LAjitDropDownList)ctrl));
                        SetFocus = true;
                        break;
                    }
                }
                else if (ctrl is HtmlTableRow)
                {
                    foreach (Control contrl in ctrl.Controls)
                    {
                        if (!SetFocus)
                        {
                            SetFocusFirstControl(contrl);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (ctrl is Panel)
                {
                    if (!SetFocus)
                    {
                        SetFocusFirstControl(ctrl);
                    }
                }
            }
        }

        /// <summary>
        /// Does all the Session Validations like Session-Expire, Session is null both on the client 
        /// and server sides.
        /// </summary>
        private void DoSessionValidation()
        {
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
            else//Handle it on the server-side
            {
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
                sRefAjax.Path = Application["ScriptsCDNPath"].ToString()+ "System.Web.Extensions/1.0.61025.0/MicrosoftAjax.js";

                ScriptReference sRefAjaxDebug = new ScriptReference();
                sRefAjaxDebug.Name = "MicrosoftAjax.debug.js";
                sRefAjaxDebug.ScriptMode = ScriptMode.Auto;
                //sRefAjaxDebug.Path = "../JavaScript/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js";
                sRefAjaxDebug.Path = Application["ScriptsCDNPath"].ToString() + "System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js";

                masterScriptMngr.Scripts.Add(sRefAjax);
                masterScriptMngr.Scripts.Add(sRefAjaxDebug);
            }
            //    ScriptManager.RegisterClientScriptInclude(Page, typeof(Page),
            //"MyScript",
            //Page.ResolveClientUrl("~/scripts/SomeJavascript.js"));
        }

        /// <summary>
        /// Binds the data for a given GridView or EntryForm depending on different scenarios.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void BindPage(Control updPanel)
        {
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
                navigationBPInfo = commonObjUI.HtmlCodesToCharacters(navigationBPInfo);
                HtmlInputHidden navBPInfo = (HtmlInputHidden)commonObjUI.ButtonsUserControl.FindControl("hdnNavBPInfo");
                if (navBPInfo != null)
                {
                    navBPInfo.Value = navigationBPInfo;
                }
            }

            //Setting the BPInfo when the data is posted
            string BPInfo = string.Empty;
            if (Convert.ToString(Context.Request["hdnBPInfo"]) != null && Convert.ToString(Context.Request["hdnBPInfo"]) != string.Empty)
            {
                BPInfo = Convert.ToString(Context.Request["hdnBPInfo"]);
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

            //Set the Session BPInfo only if the page is not a pop up page-25th May 2009.
            if (!isPopUp)
            {
                if (BPInfo != string.Empty)
                {
                    HttpContext.Current.Session["BPINFO"] = BPInfo;
                }
                else
                {
                    //If BPInfo is still found to be empty then use the Bpinfo present in SessionBPInfo.
                    BPInfo = Convert.ToString(HttpContext.Current.Session["BPINFO"]);
                }
            }

            //Passing the BPInfo and Content Page Update panel as parameters to bind the page
            commonObjUI.BindPage(BPInfo, updPanel);

            //Calling Page specific method
            MethodInfo minfo = this.Page.GetType().GetMethod("PageSpecific");
            if (minfo != null)
            {
                minfo.Invoke(this.Page, new object[] { null });
            }
        }
        #endregion

        #region ViewState Management

        //overriding method of Page class
        protected override object LoadPageStateFromPersistenceMedium()
        {
            string i = this.Page.MasterPageFile;
            //If server side enabled use it, otherwise use original base class implementation
            if (true == viewStateSvrMgr.GetViewStateSvrMgr().ServerSideEnabled && this.Page.MasterPageFile == "../MasterPages/TopLeft.Master")
            {
                return LoadViewState();
            }
            else
            {
                return base.LoadPageStateFromPersistenceMedium();
            }
        }

        //overriding method of Page class
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            if (true == viewStateSvrMgr.GetViewStateSvrMgr().ServerSideEnabled && this.Page.MasterPageFile == "../MasterPages/TopLeft.Master")
            {
                SaveViewState(viewState);
            }
            else
            {
                base.SavePageStateToPersistenceMedium(viewState);
            }
        }

        //implementation of method
        private object LoadViewState()
        {
            if (_formatter == null)
            {
                _formatter = new LosFormatter();
            }

            //Check if the client has form field that stores request key
            if (null == Request.Form[VIEW_STATE_FIELD_NAME])
            {
                //Did not see form field for viewstate, return null to try to continue (could log event...)
                return null;
            }

            //Make sure it can be converted to request number (in case of corruption)
            long lRequestNumber = 0;
            try
            {
                lRequestNumber = Convert.ToInt64(Request.Form[VIEW_STATE_FIELD_NAME]);
            }
            catch(Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
                //Could not covert to request number, return null (could log event...)
                return null;
            }

            //Get the viewstate for this page
            string _viewState = viewStateSvrMgr.GetViewStateSvrMgr().GetViewState(lRequestNumber);

            //If find the viewstate on the server, convert it so ASP.Net can use it
            if (_viewState == null)
                return null;
            else
                return _formatter.Deserialize(_viewState);
        }


        //implementation of method
        private void SaveViewState(object viewState)
        {
            if (_formatter == null)
            {
                _formatter = new LosFormatter();
            }

            //Save the viewstate information
            StringBuilder _viewState = new StringBuilder();
            StringWriter _writer = new StringWriter(_viewState);
            _formatter.Serialize(_writer, viewState);
            long lRequestNumber = viewStateSvrMgr.GetViewStateSvrMgr().SaveViewState(_viewState.ToString());

            //Need to register the viewstate hidden field (must be present or postback things don't 
            // work, we use in our case to store the request number)
            RegisterHiddenField(VIEW_STATE_FIELD_NAME, lRequestNumber.ToString());
        }

        #endregion
    }
}