using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LAjitDev.UserControls;
using System.Xml;
using System.Web.SessionState;
using System.Collections;
using System.Drawing;
using System.Text;
using LAjitControls;
using LAjit_BO;
using System.Collections.Generic;
using System.IO;
using NLog;


namespace LAjitDev
{
    public partial class CommonUI
    {
        #region Variables
        private GridViewControl GVUC;
        private ChildGridView CGVUC;

        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public string nlog = string.Empty;

        private ButtonsUserControl BtnsUC;
        CommonBO objBO = new CommonBO();
        Dictionary<string, Control> m_PageControls;

        int m_TxtBoxCntr = 0;
        int m_DDLCntr = 0;
        int m_LblCntr = 0;
        int m_ChkBxCntr = 0;
        int m_LnkBtnCntr = 0;
        int m_LstBoxCntr = 0;
        int m_ValidateReqCntr = 0;
        int m_ValidateRegCntr = 0;
        int m_htmlTblRwCntr = 0;
        private int m_NumberOfLinks;
        private ParentChildGV PCGVUC;
        private string m_pnlTitleID;
        private string m_pnlContentID;
        private string m_imgID;
        private bool m_ShowLinksInTitle = true;
        private bool m_IsHelpAuthPage = false;
        public ArrayList m_alCalendarTBoxIDS = new ArrayList();
        public ArrayList m_alMaskTBoxIDS = new ArrayList();
        public ArrayList m_alCalcTBoxIDS = new ArrayList();
        public Hashtable m_htAutoCompleteTBoxIDS = new Hashtable();
        public ArrayList m_alPhoneMaskTBoxIDS = new ArrayList();

        //To store child grid view autofill ids and column names
        public Hashtable m_htCGAutoCompleteTBoxIDS = new Hashtable();
        public ArrayList m_alCGCalendarTBoxIDS = new ArrayList();
        public ArrayList m_alCGMaskTBoxIDS = new ArrayList();

        public static bool IsAutoFillCache = false;
        #endregion

        #region Properties

        public string TitleID
        {
            get { return m_pnlTitleID; }
            set { m_pnlTitleID = value; }
        }
        public string ContentID
        {
            get { return m_pnlContentID; }
            set { m_pnlContentID = value; }
        }
        public string ImageID
        {
            get { return m_imgID; }
            set { m_imgID = value; }
        }
        public bool IsHelpAuthPage
        {
            get { return m_IsHelpAuthPage; }
            set { m_IsHelpAuthPage = value; }
        }

        public int NumberOfLinks
        {
            get { return m_NumberOfLinks; }
            set { m_NumberOfLinks = value; }
        }

        /// <summary>
        /// Accessor for the Session["BPInfo"]
        /// </summary>
        public string SessionBPInfo
        {
            get { return Convert.ToString(HttpContext.Current.Session["BPINFO"]); }
            set { HttpContext.Current.Session["BPINFO"] = value; }
        }

        /// <summary>
        /// Accessor for the Session["LinkBPinfo"]
        /// </summary>
        public string SessionLinkBPInfo
        {
            get { return Convert.ToString(HttpContext.Current.Session["LinkBPinfo"]); }
            set { HttpContext.Current.Session["LinkBPinfo"] = value; }
        }

        public Dictionary<string, Control> PageControls
        {
            get { return m_PageControls; }
            set { m_PageControls = value; }
        }

        public bool ShowLinksInTitle
        {
            get { return m_ShowLinksInTitle; }
            set { m_ShowLinksInTitle = value; }
        }

        private UpdatePanel m_UpdatePanelContent;
        public UpdatePanel UpdatePanelContent
        {
            get { return m_UpdatePanelContent; }
            set { m_UpdatePanelContent = value; }
        }


        public GridViewControl GridViewUserControl
        {
            get { return GVUC; }
            set { GVUC = value; }
        }

        public ChildGridView ChildGridUserControl
        {
            get { return CGVUC; }
            set { CGVUC = value; }
        }

        public ParentChildGV ParentChildGVUC
        {
            get { return PCGVUC; }
            set { PCGVUC = value; }
        }

        public ButtonsUserControl ButtonsUserControl
        {
            get { return BtnsUC; }
            set { BtnsUC = value; }
        }
        #endregion

        #region Pageload Methods
        /// <summary>
        /// Binds the data for a given GridView or EntryForm depending on different scenarios.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void BindPage(string BPInfo, Control CurrentPage)
        {

            #region NLog
            logger.Info("Rendering the page with BPInfo : " + BPInfo);
            #endregion

            //---------------Parameters used to form strReqXml to get the successful pagexml.
            //if (SessionBPInfo != string.Empty)
            //{
            //    HttpContext.Current.Session["BPINFO"] = SessionBPInfo;
            //}
            bool gridExists = false;
            if (!string.IsNullOrEmpty(BPInfo))
            {
                gridExists = BtnsUC.SetGVData(BPInfo);
            }
            //DefaultGridSize
            string pageSize = GetUserPreferenceValue("59");

            HiddenField hdnCurrPgNo;
            string pageNo = "1";
            if (GVUC != null)
            {
                hdnCurrPgNo = (HiddenField)GVUC.FindControl("hdnCurrPageNo");
                pageNo = hdnCurrPgNo.Value;
            }
            else if (PCGVUC != null)
            {
                hdnCurrPgNo = (HiddenField)PCGVUC.FindControl("hdnCurrPageNo");
                pageNo = hdnCurrPgNo.Value;
            }
            //BPE xml
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            //---------------------End of Parameters used to form strReqXml.

            //generate Request XMl 
            string strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPInfo, "", "", "", strBPE, gridExists, pageSize, pageNo, null);
            //BPOUT from DB
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);

            BindOutXML(CurrentPage, gridExists, strReqXml, strOutXml);
        }

        public void BindOutXML(Control CurrentPage, bool gridExists, string strReqXml, string strOutXml)
        {

            #region NLog
            logger.Info("This gives the Bind OUT xml in the CommonUI");
            #endregion

            BtnsUC.PreloadImageButtonImages();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(strOutXml);

            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            if (nodeMsgStatus.InnerText != "Success")
            {
                //No need to show BtnsUC if error comes while loading the page.
                BtnsUC.Visible = false;
                pnlEntryForm.Attributes.Add("style", "DISPLAY: none;");
                Panel pnlGVContent = (Panel)CurrentPage.FindControl("pnlGVContent");
                pnlGVContent.Attributes.Add("style", "DISPLAY: none;");

                Panel pnlGVTitle = (Panel)CurrentPage.FindControl("pnlCPGV1Title");
                pnlGVTitle.Attributes.Add("style", "DISPLAY: none;");

                Panel pnlContentError = (Panel)CurrentPage.FindControl("pnlContentError");
                pnlContentError.Visible = true;

                Label lblError = (Label)CurrentPage.FindControl("lblError");
                if (nodeMsg != null)
                {
                    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo").InnerText != null && nodeMsg.SelectSingleNode("OtherInfo").InnerText != string.Empty)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                        else
                        {
                            lblError.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else
                        lblError.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                }
                return;
            }
            else
            {
                BtnsUC.GVDataXml = strOutXml;

                clsBranchUI objBranchUI = new clsBranchUI();
                objBranchUI.ObjCommonUI = this;
                //Setting BranchColumns in hdnBranchColsXML
                objBranchUI.setHiddenBranchXML(xDoc);

                //setting Treenode name 
                XmlNode nodeTree = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node");
                if (nodeTree != null)
                {
                    BtnsUC.TreeNode = nodeTree.InnerText;
                }
                string m_treeNodeName = BtnsUC.TreeNode;

                //---------Clientside functionality(shanti)
                //To enale timer for add mode.
                HiddenField hdnTimerValue = (HiddenField)BtnsUC.FindControl("hdnTimerValue");
                hdnTimerValue.Value = SetTimer(xDoc, CurrentPage);

                //The SetCurrentAction function will set the hidden current action value 
                //and call the function InvokeOnButtonClick() which will enable/colr/setdefault control based on the mode
                LAjitImageButton imgbtnAdd = (LAjitImageButton)BtnsUC.FindControl("imgbtnAdd");
                imgbtnAdd.OnClientClick = "SetCurrentAction('Add');return false;";

                LAjitImageButton imgbtnClone = (LAjitImageButton)BtnsUC.FindControl("imgbtnClone");
                imgbtnClone.OnClientClick = "SetCurrentAction('Clone');return false;";

                LAjitImageButton imgbtnModify = (LAjitImageButton)BtnsUC.FindControl("imgbtnModify");
                imgbtnModify.OnClientClick = "SetCurrentAction('Modify');return false;";

                LAjitImageButton imgbtnDelete = (LAjitImageButton)BtnsUC.FindControl("imgbtnDelete");
                imgbtnDelete.OnClientClick = "SetCurrentAction('Delete');return false;";

                LAjitImageButton imgbtnFind = (LAjitImageButton)BtnsUC.FindControl("imgbtnFind");
                imgbtnFind.OnClientClick = "SetCurrentAction('Find');return false;";

                ImageButton imgbtnCancel = (ImageButton)pnlEntryForm.FindControl("imgbtnCancel");
                imgbtnCancel.OnClientClick = "OnButtonClick('PageLoad');return false;";
                //----------End Clientside functionality

                if (gridExists)
                {
                    XmlDocument xBPinfo = new XmlDocument();
                    xBPinfo.LoadXml(strReqXml);
                    XmlNode nodeJobType = xBPinfo.CreateNode(XmlNodeType.Element, m_treeNodeName, null);
                    XmlNode nodeBPInfo = xBPinfo.SelectSingleNode("Root/bpinfo");
                    XmlNode nodegridView = xBPinfo.SelectSingleNode("Root/bpinfo/Gridview");
                    if (nodegridView != null)
                    {
                        string gridview = nodegridView.OuterXml;
                        nodeBPInfo.RemoveChild(nodegridView);
                        nodeJobType.InnerXml = gridview;
                        nodeBPInfo.AppendChild(nodeJobType);
                    }

                    string BPInfo = xBPinfo.SelectSingleNode("Root/bpinfo").OuterXml;
                    if (GVUC != null)
                    {
                        GVUC.GridViewBPInfo = BPInfo;
                        GVUC.GridViewType = GridViewType.Default;
                        GVUC.GridViewInitData = strOutXml;

                        HiddenField hdnFldBPInfo = (HiddenField)BtnsUC.FindControl("hdnFldBPInfo");
                        hdnFldBPInfo.Value = BPInfo;
                    }
                    else if (PCGVUC != null)
                    {
                        PCGVUC.GridViewBPInfo = BPInfo;
                        PCGVUC.PCGridViewType = PCGridViewType.Default;
                        PCGVUC.GridViewInitData = strOutXml;
                    }
                    if (GVUC == null)
                    {
                        ImageButton imgbtnNext = (ImageButton)pnlEntryForm.FindControl("imgbtnNext");
                        ImageButton imgbtnPrevious = (ImageButton)pnlEntryForm.FindControl("imgbtnPrevious");
                        if (imgbtnNext != null && imgbtnPrevious != null)
                        {
                            imgbtnNext.Visible = false;
                            imgbtnPrevious.Visible = false;
                        }
                    }
                    //25May09HttpContext.Current.Session["BPINFO"] = BPInfo;//Assign the BPINFO session with the Gridview Parameters.
                    SessionLinkBPInfo = BPInfo;
                }

                HandleBpeOut(strOutXml, CurrentPage, hdnTimerValue.Value);

                //Set the Page Title for the current page.
                ((Page)CurrentPage.TemplateControl).Title = xDoc.SelectSingleNode("Root/bpeout/FormInfo/Title").InnerText;

            }
            //Keeping Parent BPInfo backup in the hidden variable
            if (BtnsUC != null)
            {
                HiddenField parentBPInfo = (HiddenField)BtnsUC.FindControl("parentBPInfo");
                if (parentBPInfo != null)
                    parentBPInfo.Value = SessionLinkBPInfo;
            }

            //Next & Previous grid records functionality
            if (GVUC != null)
            {
                GridView grdVwContents = (GridView)GVUC.FindControl("grdVwContents");

                if (grdVwContents.Rows.Count > 1)
                {
                    ImageButton imgNavigate = (ImageButton)grdVwContents.Rows[0].FindControl("imgBtnTTnNavigate");
                    string postBackStr = GVUC.Page.ClientScript.GetPostBackEventReference(imgNavigate, "");

                    ImageButton imgbtnNext = (ImageButton)pnlEntryForm.FindControl("imgbtnNext");
                    if (imgbtnNext != null)
                    {
                        imgbtnNext.OnClientClick = "NavigateRows('F',\"" + postBackStr + "\");return false";
                    }
                    ImageButton imgbtnPrevious = (ImageButton)pnlEntryForm.FindControl("imgbtnPrevious");
                    if (imgbtnPrevious != null)
                    {
                        imgbtnPrevious.OnClientClick = "NavigateRows('B',\"" + postBackStr + "\");return false";
                    }
                }
                else
                {
                    ImageButton imgbtnNext = (ImageButton)pnlEntryForm.FindControl("imgbtnNext");
                    ImageButton imgbtnPrevious = (ImageButton)pnlEntryForm.FindControl("imgbtnPrevious");
                    if (imgbtnNext != null && imgbtnPrevious != null)
                    {
                        imgbtnNext.Visible = false;
                        imgbtnPrevious.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Visibility of EntryForm and GridView based on rowlist while pageload.
        /// </summary>
        /// <param name="returnXML">BPOut getting for the given BPGID</param
        private void HandleBpeOut(string returnXML, Control CurrentPage, string setTimer)
        {
            #region NLog
            logger.Info("Visibility of EntryForm and GridView based on rowlist while pageload.");
            #endregion

            clsBranchUI objBranchUI = new clsBranchUI();
            objBranchUI.ObjCommonUI = this;

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(returnXML);

            //Set the hidden variable in BtnsUC for further reference.
            string currentBPGID = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            string currentPageInfo = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
            HiddenField hdnFormInfoBtnsUC = (HiddenField)BtnsUC.FindControl("hdnFormInfo");
            hdnFormInfoBtnsUC.Value = currentBPGID + "~::~" + currentPageInfo;

            //Finding all the page controls
            Panel pnlGVContent = (Panel)CurrentPage.FindControl("pnlGVContent");
            Panel pnlCPGV1Title = (Panel)CurrentPage.FindControl("pnlCPGV1Title");
            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            Panel pnlEntryFormTitle = (Panel)CurrentPage.FindControl("pnlEntryFormTitle");
            ImageButton imgbtnSubmit = (ImageButton)pnlEntryForm.FindControl("imgbtnSubmit");
            ImageButton imgbtnContinueAdd = (ImageButton)pnlEntryForm.FindControl("imgbtnContinueAdd");
            ImageButton imgbtnAddClone = (ImageButton)pnlEntryForm.FindControl("imgbtnAddClone");
            ImageButton imgbtnIsApproved = (ImageButton)pnlEntryForm.FindControl("imgbtnIsApproved");
            Label lblmsg = (Label)pnlEntryForm.FindControl("lblmsg");
            LAjitControls.LAjitImage imgTypeOfJob = (LAjitControls.LAjitImage)pnlEntryForm.FindControl("imgTypeOfJob");

            //setting default conditions(changed by shanti from invisible to display:none, to access controls in java script)
            pnlEntryForm.Attributes.Add("style", "DISPLAY: none;");
            pnlGVContent.Attributes.Add("style", "DISPLAY: none;");
            imgbtnContinueAdd.Attributes.Add("style", "DISPLAY: none;");
            imgbtnAddClone.Attributes.Add("style", "DISPLAY: none;");
            lblmsg.Attributes.Add("style", "DISPLAY: none;");


            HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
            string strImagesCDNPath = (String)ctx.Application["ImagesCDNPath"];



            if (imgbtnIsApproved != null)
            {
                imgbtnIsApproved.Attributes.Add("style", "DISPLAY: none;");
                imgbtnIsApproved.ImageUrl = strImagesCDNPath + "images/Waiting-Approval.png";
            }
            if (imgTypeOfJob != null)
            {
                imgTypeOfJob.Attributes.Add("style", "DISPLAY: none;");
                imgTypeOfJob.ImageUrl = strImagesCDNPath + "images/spacer.gif";
            }

            HiddenField hdnCurrAction = (HiddenField)BtnsUC.FindControl("hdnCurrAction");
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/GridHeading/Columns");
            if (nodeColumns == null)
            {
                return;
            }
            //IsRequired or IsNumeric validations
            ValidateControls(nodeColumns, pnlEntryForm);

            //forming nodeColumns and setting it into a hidden fld to be used in javascript.(shanti)
            HiddenField hdnParentColNode = (HiddenField)BtnsUC.FindControl("hdnParentColNode");
            hdnParentColNode.Value = GetPageColumns(xDocOut, pnlEntryForm);

            //For Filling DD
            ArrayList alColIslink = new ArrayList();
            SetLabelText(xDocOut, out alColIslink, pnlEntryForm);

            //Filling dropdown data
            if (alColIslink.Count > 0)
            {
                FillDropDownData(returnXML, alColIslink, pnlEntryForm);
            }

            //Invisible all the columns whose IsHidden Attribute is one
            HandleIsHidden(pnlEntryForm, xDocOut, false);

            //Visible or Invisible the EntryForm and CPGV based on different scenarios.
            XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/RowList");

            //Empty EntryForm for 'Add'
            if (nodeRowList == null)
            {
                //If no Add BPC then show no records msg else land the page in Add mode.
                if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null)
                {
                    pnlGVContent.Attributes.Add("style", "DISPLAY: Block;");
                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.GetType(), "ForcedClose", "javascript:CloseEntryForm();", true);

                    string jscript = "javascript:setTimeout('ExpandGrid()',50);";
                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceExpand/Collapse", jscript, true);

                    //This can be used in menu opening popups case.
                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.GetType(), "ResetCurrAction", "javascript:ResetCurrAction();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.GetType(), "AddLoadCase", "javascript:SetCurrentAction('Add');", true);
                    pnlGVContent.Visible = false;
                }

                //Filling listbox initial data
                InitialiseBranchObjects(xDocOut, pnlEntryForm);
            }
            else//Entry form for 'Modify'
            {
                if (nodeRowList.ChildNodes.Count == 1)
                {
                    XmlNode nodeResParent = nodeRowList.SelectSingleNode("Rows");
                    string parentTrxID = string.Empty;
                    if (nodeResParent != null)
                    {
                        if (nodeResParent.Attributes["TrxID"] != null)
                        {
                            parentTrxID = nodeResParent.Attributes["TrxID"].Value;
                        }
                    }
                    //setting Branch XML
                    objBranchUI.SetBranchXML(returnXML, parentTrxID);

                    if (BtnsUC.Page.ToString().Contains("fullview"))
                    {
                        string js = string.Empty;
                        if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/Singleton") != null)
                        {
                            if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/Singleton").Attributes["Value"].Value == "1")
                            {
                                //Setting the RwToBeModified value in pageload for modufy condition.
                                hdnCurrAction.Value = "Select";
                                BtnsUC.RwToBeModified = nodeRowList.InnerXml;

                                InvokeOnButtonClick("Select", this.BtnsUC.Page);

                                //Showing data in html table.
                                GVUC.ShowHtmlVw(pnlEntryForm, parentTrxID, xDocOut, BtnsUC.TreeNode);

                                //Setting hdnBPInfo and COXMl  which is accessed in OnFormLinkClick
                                SetCOXML(xDocOut, nodeColumns, "");

                                js = "javascript:setTimeout('CollapseGrid()',50);";
                            }
                        }
                        else
                        {
                            pnlGVContent.Attributes.Add("style", "DISPLAY: Block;");
                            InvokeOnButtonClick("PageLoad", this.BtnsUC.Page);
                            js = "javascript:setTimeout('ExpandGrid()',50);";
                        }

                        ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceExpand/Collapse", js, true);
                    }
                    else
                    {
                        hdnCurrAction.Value = "Select";

                        //Setting the RwToBeModified value in pageload for modify condition.
                        BtnsUC.RwToBeModified = nodeRowList.InnerXml;
                        if (GVUC != null)
                        {
                            Control ctrlGVBPInfo = GVUC.FindControl("hdnGVBPEINFO");
                            if (ctrlGVBPInfo != null)
                            {
                                ((HiddenField)ctrlGVBPInfo).Value = nodeRowList.InnerXml;
                            }
                        }

                        lblmsg.Text = string.Empty;
                        //Grid as well as form will be displayed.
                        pnlGVContent.Attributes.Add("style", "DISPLAY: Block;");

                        //To show trSOXApproval and hide trProcessLinks.
                        BtnsUC.HandleFormLinks(pnlEntryForm, xDocOut);

                        //Reset the note attachment secure image based on data availabity
                        BtnsUC.NoteAttachSecurePicChange(hdnCurrAction.Value);

                        //Setting hdnBPInfo and COXMl  which is accessed in OnFormLinkClick
                        SetCOXML(xDocOut, nodeColumns, "");

                        //Only to fill all controls
                        EnableDisableAndFillUI(false, pnlEntryForm, xDocOut, nodeRowList.ChildNodes[0], "DISABLEMODE", true);
                        //Only to disable all controls client side by invoking a common javascript. Added by shanti(20/02/08)
                        InvokeOnButtonClick("Select", this.BtnsUC.Page);

                        //Opening the grid. Need to change to clientside.
                        /* AjaxControlToolkit.CollapsiblePanelExtender cntrlpnlTitleobj = (AjaxControlToolkit.CollapsiblePanelExtender)CurrentPage.FindControl("cpeCPGV1");
                         cntrlpnlTitleobj.Collapsed = true;
                         cntrlpnlTitleobj.ClientState = "true";*/
                        //string js = "javascript:setTimeout('CollapseGrid()',50);";//not working
                        //ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceCollapse", js, true);
                    }
                    //ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ValidateBPGIDRow", "ValidateBPGIDRow();", true);
                }
                else
                {
                    pnlGVContent.Attributes.Add("style", "DISPLAY: Block;");
                    InvokeOnButtonClick("PageLoad", this.BtnsUC.Page);

                    //Filling listbox initial data
                    InitialiseBranchObjects(xDocOut, pnlEntryForm);
                }
            }
            //setting default pagesize for fullview page.
            if (BtnsUC.Page.ToString().Contains("fullview"))
            {
                if (GVUC != null)
                {
                    GVUC.DefaultPageSize = "22";
                }
                else if (PCGVUC != null)
                {
                    PCGVUC.DefaultPageSize = "22";
                }
            }
            if (GVUC != null)
            {
                GVUC.DataBind();
            }
            else if (PCGVUC != null)
            {
                PCGVUC.DataBind();
            }

            //To initialize POs of a particular vendor if exist in bpc.
            InitBPCIcon(xDocOut, pnlEntryForm);

            //Only for fullview page
            if (BtnsUC.Page.ToString().Contains("fullview"))
            {
                hdnCurrAction.Value = string.Empty;
                Panel pnlSubmit = (Panel)pnlEntryForm.FindControl("pnlSubmit");
                pnlSubmit.Attributes.Add("style", "DISPLAY: none;");
            }
        }
        #endregion

        #region All types of Submit Functionalities
        /// <summary>
        /// Common Submit method for Add, Modify, Delete, Find 
        /// </summary>
        /// <param name="clickAction">Action Parameter</param>
        public bool SubmitEntries(string clickAction, Control CurrentPage)
        {
            #region NLog
            logger.Info("Common Submit method for action : "+clickAction+" and current page as : "+CurrentPage);
            #endregion

            //To update only the specific part of the page i.e controls in updtPnlContent.
            //UpdatePanel cntrlUpdtPnl = (UpdatePanel)CurrentPage;
            //if (cntrlUpdtPnl.UpdateMode == UpdatePanelUpdateMode.Conditional)
            //{
            //    cntrlUpdtPnl.Update();
            //}

            clsBranchUI objBranchUI = new clsBranchUI();
            objBranchUI.ObjCommonUI = this;

            string currBPGID = string.Empty;
            string CntrlValues = string.Empty;
            string trxID = string.Empty;
            string trxType = string.Empty;
            string rowVersion = string.Empty;
            string strReqXml = string.Empty;

            //Finding all the page controls
            Panel pnlGVContent = (Panel)CurrentPage.FindControl("pnlGVContent");
            Panel pnlCPGV1Title = (Panel)CurrentPage.FindControl("pnlCPGV1Title");
            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            Panel pnlEntryFormTitle = (Panel)CurrentPage.FindControl("pnlEntryFormTitle");

            Label lblmsg = (Label)CurrentPage.FindControl("pnlEntryForm").FindControl("lblmsg");
            ImageButton imgbtnSubmit = (ImageButton)CurrentPage.FindControl("pnlEntryForm").FindControl("imgbtnSubmit");
            ImageButton imgbtnCancel = (ImageButton)CurrentPage.FindControl("pnlEntryForm").FindControl("imgbtnCancel");
            ImageButton imgContinueAddBtn = (ImageButton)CurrentPage.FindControl("pnlEntryForm").FindControl("imgbtnContinueAdd");
            ImageButton imgAddCloneBtn = (ImageButton)CurrentPage.FindControl("pnlEntryForm").FindControl("imgbtnAddClone");

            XmlDocument xDocGVData = new XmlDocument();
            xDocGVData.LoadXml(BtnsUC.GVDataXml);
            HiddenField hdnSubmitstatus = (HiddenField)BtnsUC.FindControl("hdnSubmitstatus");
            HiddenField hdnCurrAction = (HiddenField)BtnsUC.FindControl("hdnCurrAction");
            string CurrAction = hdnCurrAction.Value;
            if (hdnCurrAction.Value == "Clone")
            {
                CurrAction = "Add";
            }
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            if (xDocGVData.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[@ControlType='GView']") != null)
            {
                //For calculator control hiding
                string s = "javascript:Hidden();";
                ScriptManager.RegisterStartupScript(BtnsUC.Page, this.GetType(), "Hidden", s, true);
            }

            if (clickAction.ToUpper().Trim() == "SOXAPPROVAL")
            {
                //SOXAPPROVAL
                if (xDocGVData.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='SOXApproval']") != null)
                {
                    currBPGID = xDocGVData.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='SOXApproval']").Attributes["BPGID"].Value;
                }

                XmlDocument xDoc = new XmlDocument();
                if (BtnsUC.RwToBeModified != null)
                {
                    xDoc.LoadXml(BtnsUC.RwToBeModified);
                    XmlNode nodeRow = xDoc.SelectSingleNode("Rows");
                    XmlAttribute attIsApproved = nodeRow.Attributes["IsApproved"];
                    if (attIsApproved.Value == "0")
                    {
                        attIsApproved.InnerXml = "1";
                    }
                    else
                    {
                        attIsApproved.InnerXml = "0";
                    }
                }
                trxID = xDoc.SelectSingleNode("Rows").Attributes["TrxID"].Value;
                CntrlValues = xDoc.OuterXml;
            }
            else
            {
                currBPGID = GetCurrentActionBPGID(CurrAction, xDocGVData);
                //get new row
                CntrlValues = GetNewRow(pnlEntryForm, xDocGVData, CurrAction);
            }

            if (CurrAction.ToUpper().Trim() == "MODIFY" || CurrAction.ToUpper().Trim() == "DELETE")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(BtnsUC.RwToBeModified);
                trxID = doc.SelectSingleNode("Rows").Attributes["TrxID"].Value;
                trxType = doc.SelectSingleNode("Rows").Attributes["TrxType"].Value;
                XmlAttribute attRVer = doc.SelectSingleNode("Rows").Attributes["RVer"];
                if (attRVer != null)
                {
                    rowVersion = attRVer.Value;
                }
            }
            string treeNodeName = string.Empty;
            ArrayList arrBranches = new ArrayList();
            string listBoxXML = (GridViewUserControl != null) ? GetListBoxSelectionXML(GridViewUserControl.FormTempData, pnlEntryForm,
                CurrAction, trxID, trxType, rowVersion, out treeNodeName, out arrBranches) : string.Empty;

            string gvXML = string.Empty;// GetGridViewXML(pnlEntryForm, ButtonsUserControl.GVDataXml, trxID, trxType, CurrAction);
            ChildGridView CGVUC = (ChildGridView)pnlEntryForm.FindControl("CGVUC");
            if (gvXML.Length == 0)
            {
                if (CGVUC != null)
                {
                    gvXML = CGVUC.GetGridViewXML(pnlEntryForm, ButtonsUserControl.GVDataXml, trxID, trxType, CurrAction);
                }
            }

            //Ardetailschild child grid for ARinvDetail.aspx only
            string gvArDescrptionXML = string.Empty;
            ChildGridView CGVUCArdescription = (ChildGridView)pnlEntryForm.FindControl("CGVUCArdescription");
            if (CGVUCArdescription != null)
            {
                gvArDescrptionXML = CGVUCArdescription.GetGridViewXML(pnlEntryForm, ButtonsUserControl.GVDataXml, trxID, trxType, CurrAction);
                gvXML = gvXML + gvArDescrptionXML;
            }

            string branchXML = string.Empty;
            XmlDocument xBranchDoc = new XmlDocument();
            if (BtnsUC.BranchXML != null && BtnsUC.BranchXML != string.Empty)
            {
                xBranchDoc.LoadXml(BtnsUC.BranchXML);
                XmlNode nodeRoot = xBranchDoc.SelectSingleNode("Root");
                if (nodeRoot != null)//(branchExists)
                {
                    //Added by Danny.
                    FormatPhoneTypes(xDocGVData, xBranchDoc);
                    branchXML = nodeRoot.InnerXml;
                }
            }

            string childObjectXML = listBoxXML.Trim() + gvXML.Trim() + branchXML.Trim();
            //DefaultGridSize
            string pageSize = GetUserPreferenceValue("59");

            HiddenField hdnCurrPgNo = (GVUC != null) ? (HiddenField)GVUC.FindControl("hdnCurrPageNo") : (HiddenField)PCGVUC.FindControl("hdnCurrPageNo");
            string pageNo = hdnCurrPgNo.Value;

            pnlEntryForm.Attributes.Add("style", "DISPLAY: block;");

            //Validating AutoFill Controls...
            if (CurrAction.ToUpper().Trim() == "MODIFY" || CurrAction.ToUpper().Trim() == "ADD")
            {
                string validateMessage = string.Empty;
                validateMessage = ValidateAutoFillEntry(xDocGVData, childObjectXML);
                if ((validateMessage != string.Empty) && (validateMessage != ""))
                {
                    //Re-enable the child objects in the page.
                    if (CGVUC != null)
                    {
                        CGVUC.ReEnableChildObject(CurrAction.ToUpper().Trim());
                        //Re-enable the ardescription child in the arinvDetail.aspx page.
                        if (CGVUCArdescription != null)
                        {
                            CGVUCArdescription.ReEnableChildObject(CurrAction.ToUpper().Trim());
                        }
                    }
                    else
                    {
                        ReEnableChildObject(pnlEntryForm, CurrAction.ToUpper().Trim());
                    }

                    hdnSubmitstatus.Value = "Error";
                    lblmsg.Text = "Please enter valid data in " + validateMessage;
                    //State of page in error case should be the current state. If error comes while adding a record then CurrAction should be Add.                    
                    InvokeOnButtonClick(CurrAction, this.BtnsUC.Page);
                    PersistAutoFillEntries(CurrentPage, pnlEntryForm);
                    return false;
                }
            }

            bool gridExists = true;
            if (pnlGVContent.Style.Value == "DISPLAY: none;")
            {
                gridExists = false;
            }

            if (clickAction.ToUpper().Trim() == "SOXAPPROVAL")
            {
                strReqXml = objBO.GenActionRequestXML("Modify", currBPGID, CntrlValues, trxID, BtnsUC.GVDataXml.ToString(), strBPE, gridExists, pageSize, pageNo, SessionLinkBPInfo, childObjectXML);
            }
            else
            {
                //Sending the form values to generate Request XMl hdnFldAction.Value.ToUpper().Trim()
                if (imgbtnSubmit.Attributes["attrSave"].ToUpper().Trim() == "SAVE")
                {
                    strReqXml = objBO.GenActionRequestXML("Save", currBPGID, CntrlValues, trxID, BtnsUC.GVDataXml.ToString(), strBPE, gridExists, pageSize, pageNo, SessionLinkBPInfo, childObjectXML);
                }
                else
                {
                    strReqXml = objBO.GenActionRequestXML(CurrAction, currBPGID, CntrlValues, trxID, BtnsUC.GVDataXml.ToString(), strBPE, gridExists, pageSize, pageNo, SessionLinkBPInfo, childObjectXML);
                }
            }

            if (IsHelpAuthPage)
            {
                //If HelpAuth is being run then the HelpBPGID should be sent for every request to the DB.
                if (SessionLinkBPInfo.Length > 0)
                {
                    XmlDocument xDocBpInfo = new XmlDocument();
                    xDocBpInfo.LoadXml(SessionLinkBPInfo);
                    XmlNode nodeBPGID = xDocBpInfo.SelectSingleNode("bpinfo/HelpBPGID");
                    string helpBPGID = nodeBPGID.InnerText;

                    XmlDocument xDocReq = new XmlDocument();
                    xDocReq.LoadXml(strReqXml);
                    XmlNode nodeBpInfo = xDocReq.SelectSingleNode("Root/bpinfo");
                    XmlNode nodeHelpBPGID = xDocReq.CreateNode(XmlNodeType.Element, "HelpBPGID", "");
                    nodeHelpBPGID.InnerText = helpBPGID;
                    nodeBpInfo.PrependChild(nodeHelpBPGID);
                    strReqXml = xDocReq.OuterXml;
                }
            }

            //Before submitting request to the DB if this event has fired as a result of Confirm Submit(Override)
            //functionality.
            HiddenField hdnCnfmSbmt = (HiddenField)BtnsUC.FindControl("hdnCnfmSbmt");
            if (hdnCnfmSbmt != null && hdnCnfmSbmt.Value == "True")
            {
                //Insert a node named Override into the BpInfo.
                XmlDocument xDocReq = new XmlDocument();
                xDocReq.LoadXml(strReqXml);
                XmlNode nodeBPInfo = xDocReq.SelectSingleNode("Root/bpinfo");
                nodeBPInfo.InnerXml += "<Override>1</Override>";
                strReqXml = xDocReq.OuterXml;
                hdnCnfmSbmt.Value = "False";

                //string js = "javascript:alert('Testing the functionality after Override Confirm');";
                //ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "TestJs", js, true);

                //if (UpdatePanelContent.UpdateMode == UpdatePanelUpdateMode.Conditional)
                //{
                //    UpdatePanelContent.Update();
                //}
                //InvokeOnButtonClick("Submit", this.BtnsUC.Page);
                //pnlEntryForm.Visible = false;
            }


            //BPOUT from DB
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);
            XmlDocument returnXMLDoc = new XmlDocument();
            returnXMLDoc.LoadXml(strOutXml);

            //Making branchXML empty
            BtnsUC.BranchXML = string.Empty;
            XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");

            //To update viewstate xml after any operation
            if (clickAction.ToUpper().Trim() == "SOXAPPROVAL")
            {
                BtnsUC.UpdateViewStateXml("SOXAPPROVAL", trxID, strOutXml);
            }
            else
            {
                BtnsUC.UpdateViewStateXml(CurrAction.ToUpper().Trim(), trxID, strOutXml);
                objBranchUI.UpdateBranchViewStateXml(CurrAction.ToUpper().Trim(), trxID, strOutXml);
                XmlNode nodeResParent = returnXMLDoc.SelectSingleNode("//" + BtnsUC.TreeNode + "/RowList/Rows[1]");
                if (nodeResParent != null)
                {
                    trxID = nodeResParent.Attributes["TrxID"].Value;
                }
            }

            //setting Branch XML
            objBranchUI.SetBranchXML(BtnsUC.GVDataXml.ToString(), trxID);

            //Confirm Submit Ovveride
            bool refer = false;
            bool confirmSubmit = false;
            if (nodeMsgStatus.InnerText == "Error")// && nodeMsgStatus.InnerText != "Success")// || true)
            {
                PersistAutoFillEntries(CurrentPage, pnlEntryForm);

                XmlNode nodeMI = nodeMsgStatus.NextSibling;
                XmlNode nodeMILabel = nodeMI.SelectSingleNode("Label");
                if (nodeMILabel != null && nodeMILabel.InnerText == "Override")
                {
                    //confirmSubmit = true;
                    string strConfirm = string.Empty;
                    string status = string.Empty;
                    XmlNode nodeMIStatus = nodeMI.SelectSingleNode("Status");
                    if (nodeMIStatus != null)
                    {
                        status = nodeMIStatus.InnerText;
                    }
                    XmlNode nodeMIOther = nodeMI.SelectSingleNode("OtherInfo");
                    if (nodeMIOther != null)
                    {
                        strConfirm += nodeMIOther.InnerText;
                    }
                    if (strConfirm.Length == 0)
                    {
                        strConfirm = status;
                    }
                    if (strConfirm.Length > 0)
                    {
                        string suggestText = "Click \"OK\" to continue or \"Cancel\" to make changes.";
                        string CGVUCID = string.Empty;
                        if (CGVUC != null)
                        {
                            CGVUCID = CGVUC.ClientID;
                            CGVUC.ClearDeletions = false;
                        }
                        else
                        {
                            CGVUCID = "";
                        }

                        //Handling ArDescriptionChildGrid for the ARinvDetail.aspx page
                        string CGVUCArdescriptionID = string.Empty;
                        if (CGVUCArdescription != null)
                        {
                            CGVUCArdescriptionID = CGVUCArdescription.ClientID;
                            CGVUCArdescription.ClearDeletions = false;
                        }
                        else
                        {
                            CGVUCArdescriptionID = "";
                        }
                        ScriptManager.RegisterClientScriptBlock(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ConfirmSubmit", "javascript:setTimeout(\"ConfirmSubmit('" + HtmlEncode(strConfirm) + "','" + HtmlEncode(suggestText) + "','" + CGVUCID + "','" + CGVUCArdescriptionID + "' )\",400)", true);
                    }
                }
            }

            //success messge
            if (nodeMsgStatus.InnerText == "Success" || confirmSubmit)
            {
                hdnSubmitstatus.Value = clickAction;

                //This is to Find the CommonUI Partial Set COXML for select Invoice
                if (CurrentPage.Page.ToString().Trim().Contains("selectinvoice"))
                {
                    SetCOXML(returnXMLDoc, returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/GridHeading/Columns"), gvXML);
                }

                if (CurrAction.ToUpper() == "ADD" || CurrAction.ToUpper() == "MODIFY")
                {
                    //Set the Hidden Variable hdnGVBPEINFO once a new record has been added with its RowXML,CO etc.,
                    string[] arrSplitParams = { "~::~" };
                    HiddenField hdnFormInfo = (HiddenField)GVUC.FindControl("hdnFormInfo");
                    if (hdnFormInfo == null)
                    {
                        hdnFormInfo = new HiddenField();
                    }
                    if (hdnFormInfo.Value.Trim().Length == 0)
                    {
                        string currentBPGID = xDocGVData.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
                        string currentPageInfo = xDocGVData.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
                        hdnFormInfo.Value = currentBPGID + "~::~" + currentPageInfo;
                    }

                    string[] hdnFormInfoSplit = hdnFormInfo.Value.Split(arrSplitParams, StringSplitOptions.RemoveEmptyEntries);
                    string BPGID = hdnFormInfoSplit[0];
                    string navigatePage = hdnFormInfoSplit[1];
                    XmlNode nodeUpdatedRow = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/RowList/Rows");
                    string containerRowXML = "<" + BtnsUC.TreeNode + "><RowList>" + nodeUpdatedRow.OuterXml + "</RowList></" + BtnsUC.TreeNode + ">";
                    string TrxID = nodeUpdatedRow.Attributes["TrxID"].Value;
                    string TrxType = nodeUpdatedRow.Attributes["TrxType"].Value;
                    string COCaption = string.Empty;

                    //Set the CO Caption to the the field's value whose sort order in the Columns node is 1
                    XmlNode nodeFirstColumn = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/GridHeading/Columns/Col[@SortOrder='1']");
                    if (nodeFirstColumn != null)
                    {
                        if (nodeUpdatedRow.Attributes[nodeFirstColumn.Attributes["Label"].Value] != null)
                        {
                            COCaption = nodeUpdatedRow.Attributes[nodeFirstColumn.Attributes["Label"].Value].Value;
                        }
                    }
                    //else
                    //{
                    //    Classes.ErrorLogger.LogError("No Column with SortOrder=1 in " + BtnsUC.Page.ToString(), Classes.LogType.Mail);
                    //}
                    if (!CurrentPage.Page.ToString().Contains("selectinvoice"))
                    {
                        string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + navigatePage + "</PageInfo><Caption>" + this.CharactersToHtmlCodes(COCaption) + "</Caption></CallingObject>";
                        string formLinkBPInfo = "<bpinfo><BPGID>" + BPGID + "</BPGID>" + containerRowXML + callingObjXML + "</bpinfo>";
                        HiddenField hdnGVBPEINFO = (HiddenField)GVUC.FindControl("hdnGVBPEINFO");
                        if (hdnGVBPEINFO != null)
                        {
                            hdnGVBPEINFO.Value = containerRowXML + callingObjXML;//Set the BPEinfo so that it can be later used by form-level process link functions
                        }
                        //Also update the corresponding variable in BtnsUC
                        HiddenField hdnBtnsUCGVBPEINFO = (HiddenField)BtnsUC.FindControl("hdnGVBPEINFO");
                        if (hdnBtnsUCGVBPEINFO != null)
                        {
                            hdnBtnsUCGVBPEINFO.Value = containerRowXML + callingObjXML;//Set the BPEinfo so that it can be later used by form-level process link functions
                        }
                    }
                    //Checking whether valid BPGID's exist for current row.
                    //ScriptManager.RegisterStartupScript(CurrentPage.Page, CurrentPage.Page.GetType(), "ValidateBPGIDRow", "ValidateBPGIDRow();", true);
                }

                if (imgbtnSubmit.Attributes["attrSave"].ToUpper().Trim() == "FIND")
                {
                    if (returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/RowList") != null)
                    {
                        if (returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/RowList/Rows") != null)
                        {
                            refer = true;
                        }
                    }
                    else
                    {
                        if (clickAction.ToUpper().Trim() != "SOXAPPROVAL")
                        {
                            //Re-enable the child objects in the page.                            
                            if (CGVUC != null)
                            {
                                CGVUC.ReEnableChildObject(CurrAction.ToUpper().Trim());
                                //Re-enable the ardescription child in the arinvDetail.aspx page.
                                if (CGVUCArdescription != null)
                                {
                                    CGVUCArdescription.ReEnableChildObject(CurrAction.ToUpper().Trim());
                                }
                            }
                            else
                            {
                                ReEnableChildObject(pnlEntryForm, CurrAction.ToUpper().Trim());
                            }
                        }
                        if (!BtnsUC.Page.MasterPageFile.Contains("PopUp.Master"))
                        {
                            imgbtnCancel.Visible = true;
                            imgbtnSubmit.Visible = true;
                        }
                    }
                }
                else
                {
                    refer = true;
                }

                if (refer)
                {
                    //Set the Hidden field to prevent the Page-Exit Validation.(Added by Danny)
                    ((HiddenField)BtnsUC.FindControl("hdnNeedToConfirmExit")).Value = "False";

                    if (imgbtnSubmit.Attributes["attrSave"].ToUpper().Trim() == "SAVE")
                    {
                        InitialiseBranchObjects(returnXMLDoc, CurrentPage);
                    }
                    else
                    {
                        //Update local copy(BtnsUC.GVDataXML)'s branch data with the return XML version
                        //Inserted by Danny
                        if (listBoxXML.Trim().Length > 0)// && (CurrAction.ToUpper().Trim() != "FIND"))
                        {
                            UpdateBranchViewStateXML(strOutXml, treeNodeName, arrBranches);
                        }
                        if (gvXML.Trim().Length > 0)// && (CurrAction.ToUpper().Trim() != "FIND"))
                        {
                            CGVUC.UpdateGVBranchViewStateXML(strOutXml, treeNodeName, arrBranches, pnlEntryForm);
                        }
                        if (gvArDescrptionXML.Trim().Length > 0)
                        {
                            if (CGVUCArdescription != null)
                            {
                                CGVUCArdescription.UpdateGVBranchViewStateXML(strOutXml, treeNodeName, arrBranches, pnlEntryForm);
                            }
                        }



                        XmlDocument xDoc = new XmlDocument();
                        XmlNode nodeRow = null;
                        if (BtnsUC.RwToBeModified != null && BtnsUC.RwToBeModified != "")
                        {
                            xDoc.LoadXml(BtnsUC.RwToBeModified.ToString());
                            nodeRow = xDoc.SelectSingleNode("Rows");
                        }

                        XmlDocument xDocBtnsGVDataXML = new XmlDocument();
                        xDocBtnsGVDataXML.LoadXml(BtnsUC.GVDataXml.ToString());

                        //Block the grid to display results.
                        pnlGVContent.Visible = true;
                        pnlGVContent.Attributes.Add("style", "DISPLAY: Block;");

                        //To hide trSoxApprovals in view mode.
                        BtnsUC.HandleFormLinks(pnlEntryForm, xDocBtnsGVDataXML);

                        // Add/ Modify/ Delete/ Find/ Clone
                        if (clickAction.ToUpper().Trim() == "ISSUBMITCLICK")
                        {
                            //Status no need. This function is only to fill the controls.
                            EnableDisableAndFillUI(false, pnlEntryForm, xDocBtnsGVDataXML, nodeRow, string.Empty, false);
                            //To disable/Change clor/ResetActionButtons/ShowHideFormButtons depending on current action.
                            InvokeOnButtonClick("Submit", this.BtnsUC.Page);
                        }
                        else if (clickAction.ToUpper().Trim() == "ISCONTINUEADDCLICK")
                        {
                            InvokeOnButtonClick("Add", this.BtnsUC.Page);

                        }
                        else if (clickAction.ToUpper().Trim() == "ISADDCLONECLICK")
                        {
                            //Status no need. This function is only to fill the controls.
                            EnableDisableAndFillUI(false, pnlEntryForm, xDocBtnsGVDataXML, nodeRow, string.Empty, false);
                            InvokeOnButtonClick("Clone", this.BtnsUC.Page);
                        }
                        else
                        {
                            //Should not show msg when it is success.                            
                            lblmsg.Text = string.Empty;
                            InvokeOnButtonClick("Select", this.BtnsUC.Page);
                        }

                        if (CurrAction.ToUpper().Trim() == "FIND")
                        {
                            XmlDocument xBPinfo = new XmlDocument();
                            xBPinfo.LoadXml(strReqXml);
                            string BPInfo = xBPinfo.SelectSingleNode("Root/bpinfo").OuterXml;
                            if (GVUC != null)
                            {
                                GVUC.GridViewBPInfo = BPInfo;
                            }
                            else if (PCGVUC != null)
                            {
                                PCGVUC.GridViewBPInfo = BPInfo;
                            }
                            hdnCurrPgNo.Value = string.Empty;
                        }

                        if (pnlGVContent.Style.Value == "DISPLAY: Block;")
                        {
                            if (CurrAction.ToUpper().Trim() == "DELETE")
                            {
                                XmlDocument Xdoc = new XmlDocument();
                                Xdoc.LoadXml(BtnsUC.GVDataXml.ToString());
                                string m_GVTreeNodeName = Xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                                bool noRows = false;
                                if (Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList") == null)
                                {
                                    noRows = true;
                                }
                                else
                                {
                                    if (Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList/Rows") == null)
                                    {
                                        noRows = true;
                                    }
                                }
                                if (noRows)
                                {
                                    pageNo = Convert.ToString(Convert.ToInt32(hdnCurrPgNo.Value) - 1);
                                    if (Convert.ToInt32(pageNo) > 0)
                                    {
                                        hdnCurrPgNo.Value = pageNo;

                                        bool gridExist = true;
                                        if (pnlGVContent.Style.Value == "DISPLAY: none;")
                                        {
                                            gridExist = false;
                                        }
                                        //Sending the form values to generate Request XMl hdnFldAction.Value.ToUpper().Trim()
                                        strReqXml = objBO.GenActionRequestXML("PREVIOUSPAGELOAD", SessionLinkBPInfo, "", "", BtnsUC.GVDataXml, strBPE, gridExist, pageSize, pageNo, null);
                                        //BPOUT from DB
                                        strOutXml = objBO.GetDataForCPGV1(strReqXml);

                                        //To update viewstate xml after any operation
                                        BtnsUC.UpdateViewStateXml("PREVIOUSPAGELOAD", string.Empty, strOutXml);

                                        XmlDocument xBPinfo = new XmlDocument();
                                        xBPinfo.LoadXml(strReqXml);
                                        string BPInfo = xBPinfo.SelectSingleNode("Root/bpinfo").OuterXml;
                                        if (GVUC != null)
                                            GVUC.GridViewBPInfo = BPInfo;
                                        else if (PCGVUC != null)
                                            PCGVUC.GridViewBPInfo = BPInfo;
                                    }
                                }
                            }
                            GridView grdVwContent = (GridView)GVUC.FindControl("grdVwContents");
                            if (GVUC != null)
                            {
                                GVUC.GridViewType = GridViewType.Default;
                                GVUC.GridViewInitData = BtnsUC.GVDataXml;
                                GVUC.DataBind();
                            }
                            else if (PCGVUC != null)
                            {
                                PCGVUC.PCGridViewType = PCGridViewType.Default;
                                PCGVUC.GridViewInitData = BtnsUC.GVDataXml;
                                PCGVUC.DataBind();
                                grdVwContent = (GridView)PCGVUC.FindControl("grdVwContents");
                            }

                            //To navigate the GV rows Forward/Backward even after Successful operation
                            setHdnSelectedRowIndex(grdVwContent);
                        }
                        //Setting Grid Collapse explicitly according to 'GridCollapse' attribute.
                        //Add/Modify/Clone(Add)
                        if (CurrAction == "Add" || CurrAction == "Modify")
                        {
                            string GridCollapse = string.Empty;
                            //Grid collapsing based on Page level hidden variable
                            HiddenField pageGVCollapse = (HiddenField)CurrentPage.FindControl("hdnPageGVCollapse");
                            if (pageGVCollapse != null)
                            {
                                GridCollapse = pageGVCollapse.Value;
                            }
                            else
                            {
                                //Grid collapsing based on User Preference value
                                //GridCollapse
                                GridCollapse = GetUserPreferenceValue("432");
                            }
                            if (GridCollapse != string.Empty)
                            {
                                if (GridCollapse == "1")
                                {
                                    //Opening the grid after find results
                                    string js = "javascript:setTimeout('ExpandGrid()',50);";
                                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceExpand", js, true);
                                }
                                else if (GridCollapse == "0")
                                {
                                    //Opening the grid after find results
                                    string js = "javascript:setTimeout('CollapseGrid()',50);";
                                    ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceCollapse", js, true);
                                }
                            }
                        }
                        else if (CurrAction == "Delete" || CurrAction == "Find")
                        {
                            //Opening the grid after find results
                            string js = "javascript:setTimeout('ExpandGrid()',50);";
                            ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceExpand", js, true);
                        }
                    }
                }

                //Emit the response markup of the gridview as collapsed by default.
                if (pnlGVContent != null)
                {
                    pnlGVContent.Style["display"] = "none";
                }

            }
            else
            {   // This is "Error" when there is an error in submit.
                hdnSubmitstatus.Value = "Error";

                if (clickAction.ToUpper().Trim() != "SOXAPPROVAL")
                {
                    //Re-enable the child objects in the page.
                    if (CGVUC != null)
                    {
                        CGVUC.ReEnableChildObject(CurrAction.ToUpper().Trim());
                        //Re-enable the ardescription child in the arinvDetail.aspx page.
                        if (CGVUCArdescription != null)
                        {
                            CGVUCArdescription.ReEnableChildObject(CurrAction.ToUpper().Trim());
                        }
                    }
                    else
                    {
                        ReEnableChildObject(pnlEntryForm, CurrAction.ToUpper().Trim());
                    }
                }

                //State of page in error case should be the current state.
                //If error comes while adding a record then CurrAction should be Add.
                InvokeOnButtonClick(CurrAction, this.BtnsUC.Page);
                return false;
            }



            imgbtnSubmit.Attributes["attrSave"] = string.Empty;
            UpdateTypeOfJobImage(pnlEntryForm, strReqXml);
            return true;
        }

        /// <summary>
        /// Persist the text of the Parent/Branch(excluding child grid's) AutoFill Clone in the case of error.
        /// Bug was:Clone text was either getting replaced by a previous value of AfText or getting the datafieldvalue
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="pnlEntryForm"></param>
        private void PersistAutoFillEntries(Control CurrentPage, Panel pnlEntryForm)
        {
            #region NLog
            logger.Info("Persist the text of the Parent/Branch(excluding child grid's) AutoFill Clone in the case of error. Bug was:Clone text was either getting replaced by a previous value of AfText or getting the datafieldvalue");
            #endregion

            System.Collections.Specialized.NameValueCollection nvcForm = CurrentPage.Page.Request.Form;
            for (int i = 0; i < nvcForm.Count; i++)
            {
                string name = nvcForm.GetKey(i);
                if (name.StartsWith("txt"))
                {
                    string val = nvcForm.Get(i);
                    LAjitTextBox txtAF = (LAjitTextBox)pnlEntryForm.FindControl(name.Replace("$", "_"));
                    if (txtAF != null)
                    {
                        txtAF.Attributes["AFText"] = val;
                    }
                }
            }
        }

        public void setHdnSelectedRowIndex(GridView grdVwContent)
        {
            if (BtnsUC.RwToBeModified != string.Empty)
            {
                for (int i = 0; i < grdVwContent.Rows.Count; i++)
                {
                    XmlDocument xdocRw = new XmlDocument();
                    xdocRw.LoadXml(BtnsUC.RwToBeModified);
                    string rowNodeTrxId = xdocRw.SelectSingleNode("Rows").Attributes["TrxID"].Value;

                    HiddenField hdnCurrentRow = (HiddenField)grdVwContent.Rows[i].Cells[0].FindControl("hdnRowInfo");

                    XmlDocument xdocHdn = new XmlDocument();
                    xdocHdn.LoadXml(hdnCurrentRow.Value);
                    XmlNode nodeRow = xdocHdn.SelectSingleNode("//Rows");

                    string hdnRowTrxId = (nodeRow != null) ? nodeRow.Attributes["TrxID"].Value : "";

                    if (rowNodeTrxId == hdnRowTrxId)
                    {
                        HiddenField hdngrdRow = (HiddenField)GVUC.FindControl("hdnSelectedRowIndex");
                        hdngrdRow.Value = i.ToString();
                    }
                }
            }
        }

        public bool AddMultipleRecords(Control CurrentPage)
        {
            return SubmitEntries("ISCONTINUEADDCLICK", CurrentPage);
        }

        public bool AddCloneRecords(Control CurrentPage)
        {
            return SubmitEntries("ISADDCLONECLICK", CurrentPage);
        }

        public void SaveEntries(Control CurrentPage)
        {
            #region NLog
            logger.Info("Saving the entries of the current page : "+CurrentPage);
            #endregion

            ImageButton imgbtnSubmit = (ImageButton)CurrentPage.FindControl("pnlEntryForm").FindControl("imgbtnSubmit");
            imgbtnSubmit.Attributes["attrSave"] = "Save";
            SubmitEntries("ISSUBMITCLICK", CurrentPage);
        }
        #endregion

        #region NewRow Formation
        /// <summary>
        /// To form the row for parent controls.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="treeNodeName"></param>
        /// <returns></returns>
        public string GetNewRow(Control c, XmlDocument pgXml, string clkAction)
        {
            StringBuilder sbNewRowXML = new StringBuilder();
            sbNewRowXML.Append("<Rows ");
            sbNewRowXML.Append(GetNewRowSub(c, pgXml, clkAction));
            sbNewRowXML.Append("/>");
            return sbNewRowXML.ToString();
        }

        private string GetNewRowSub(Control c, XmlDocument xdcPgXml, string clkAction)
        {
            StringBuilder sbNewRowXML = new StringBuilder();

            if (xdcPgXml.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText.ToString() != string.Empty)
            {
                //Parent Details
                string parentTreeNode = xdcPgXml.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                XmlNode nodeColumns = xdcPgXml.SelectSingleNode("Root/bpeout/FormControls/" + parentTreeNode + "/GridHeading/Columns");

                //Keeping backups
                XmlNode parentCols = nodeColumns;

                //Looping through all page controls to change Color,Clear,Enable and Default
                if (m_PageControls == null)
                {
                    m_PageControls = GetPageControls(xdcPgXml, (Panel)c);
                }

                foreach (KeyValuePair<string, Control> kvPair in m_PageControls)
                {
                    string strCntrl = kvPair.Key.Substring(0, Convert.ToInt32(kvPair.Key.Length) - 3);
                    string[] arr = kvPair.Value.ID.Split('_');
                    if (arr.Length > 1)
                    {
                        XmlNode brnchcols = xdcPgXml.SelectSingleNode("Root/bpeout/FormControls/" + arr[1] + "/GridHeading/Columns");
                        nodeColumns = brnchcols;
                    }
                    else
                    {
                        nodeColumns = parentCols;
                    }
                    switch (strCntrl)
                    {
                        case "TextBox":
                            {
                                LAjitTextBox txtCurrent = (LAjitTextBox)kvPair.Value;
                                if (txtCurrent.MapBranchNode == null || txtCurrent.MapBranchNode == string.Empty)
                                {
                                    if (!string.IsNullOrEmpty(txtCurrent.MapXML))
                                    {
                                        string txtboxValue = txtCurrent.Text.TrimEnd().TrimStart().ToString();
                                        bool AutoFillStatus = false;
                                        string UpperCaseValue = string.Empty;
                                        //Currency IsNumeric controls
                                        if (BtnsUC != null)
                                        {
                                            XmlNode nodecols = nodeColumns.SelectSingleNode("Col [@Label='" + txtCurrent.MapXML + "']");
                                            if (nodecols != null)
                                            {
                                                XmlAttribute attrIsNumeric = nodecols.Attributes["IsNumeric"];
                                                if (attrIsNumeric != null)
                                                {
                                                    // IsNumberic is 1 or ControlType is Amount remove comma characters added on 06-11-08
                                                    if ((attrIsNumeric.Value == "1") || (nodecols.Attributes["ControlType"].Value == "Amount"))
                                                    {
                                                        if (txtCurrent.Text.Contains(","))
                                                        {
                                                            txtboxValue = txtCurrent.Text.Replace(",", "");
                                                        }
                                                    }
                                                }
                                                //AutoFill Textboxes IsLink="1" 
                                                XmlAttribute attrIsLink = nodecols.Attributes["IsLink"];
                                                if (attrIsLink != null)
                                                {
                                                    if (attrIsLink.Value == "1")
                                                    {

                                                        // string autofill = txtCurrent.Attributes["autofillid"].ToString();
                                                        //sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtboxValue) + "\" ");

                                                        string autofillrow = string.Empty;
                                                        if (IsAutoFillCache)
                                                        {
                                                            autofillrow = GetNewAutoFillRow(txtCurrent.MapXML, txtCurrent.Text, nodecols);

                                                        }
                                                        else
                                                        {
                                                            string[] autofillTrxIDS = txtCurrent.ToString().Split('~');
                                                            if (autofillTrxIDS.Length > 0)
                                                            {
                                                                autofillrow = txtCurrent.MapXML + "_TrxID=\"" + autofillTrxIDS[0].ToString() + "\"  "
                                                               + txtCurrent.MapXML + "_TrxType=\"" + autofillTrxIDS[1].ToString() + "\"  ";
                                                            }
                                                        }
                                                        if (autofillrow != string.Empty)
                                                        {
                                                            AutoFillStatus = true;
                                                            sbNewRowXML.Append(autofillrow);
                                                        }

                                                        //MODIFY CASE TEXTBOX IS EMPTY CREATE EMPTY ATTRIBUTES
                                                        if ((clkAction.ToUpper() == "MODIFY") && (autofillrow == string.Empty) && (txtCurrent.Text == string.Empty))
                                                        {
                                                            autofillrow = txtCurrent.MapXML + "_TrxID=\"" + "\"  "
                                                              + txtCurrent.MapXML + "_TrxType=\"" + "\"  "
                                                              + txtCurrent.MapXML.Trim() + "=\"" + "\"  ";
                                                            AutoFillStatus = true;
                                                            sbNewRowXML.Append(autofillrow);
                                                        }
                                                    }

                                                }

                                            }
                                        }


                                        if (!AutoFillStatus)
                                        {
                                            //Check Entry To Upper key
                                            //EnterAllUpperCase
                                            UpperCaseValue = GetPreferenceValue("16");
                                            if (UpperCaseValue == "1" && !IsHelpAuthPage)
                                            {
                                                sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtboxValue.ToUpper()) + "\" ");
                                            }
                                            else
                                            {
                                                sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtboxValue) + "\" ");
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "DDL":
                            {
                                LAjitDropDownList ddlCurrent = (LAjitDropDownList)kvPair.Value;
                                if (ddlCurrent.MapBranchNode == null || ddlCurrent.MapBranchNode == string.Empty)
                                {
                                    if (ddlCurrent.MapXML.ToString() != string.Empty)
                                    {
                                        string ddlRow = string.Empty;
                                        string newTrxIDType = string.Empty;
                                        newTrxIDType = ddlCurrent.SelectedValue.Trim();
                                        if (newTrxIDType.Length == 0)
                                        {
                                            continue;
                                        }
                                        string[] strarr = newTrxIDType.Split('~');
                                        string trxID = strarr[0].ToString();
                                        string trxType = strarr[1].ToString();

                                        if (clkAction.ToUpper() != "MODIFY")
                                        {
                                            if (trxID != string.Empty && trxType != string.Empty)
                                            {
                                                //TrxID should not -1
                                                if (trxID != "-1")
                                                {
                                                    ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + trxID + "\"  "
                                                            + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + trxType + "\"  ";
                                                }
                                            }
                                            else
                                            {
                                                ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                            }
                                        }
                                        else
                                        {
                                            if (trxID != string.Empty && trxType != string.Empty)
                                            {
                                                if (trxID == "-1")
                                                {
                                                    ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + string.Empty + "\"  "
                                                            + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + string.Empty + "\"  ";
                                                }
                                                else
                                                {
                                                    ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + trxID + "\"  "
                                                            + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + trxType + "\"  ";
                                                }
                                            }
                                            else
                                            {
                                                //Appending only text when TrxId and TrxType are empty
                                                if (trxID == "-1")
                                                {
                                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                                                }
                                                else
                                                {
                                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                                }
                                            }
                                        }
                                        if (ddlRow != string.Empty)
                                        {
                                            sbNewRowXML.Append(ddlRow);
                                        }
                                    }
                                }
                            }
                            break;
                        case "CheckBox":
                            {
                                LAjitCheckBox chkbxCurrent = (LAjitCheckBox)kvPair.Value;
                                if (chkbxCurrent.MapBranchNode == null || chkbxCurrent.MapBranchNode == string.Empty)
                                {
                                    if (chkbxCurrent.Checked)
                                    {
                                        //True
                                        sbNewRowXML.Append(chkbxCurrent.MapXML.Trim() + "=\"1\" ");
                                    }
                                    else
                                    {   //False
                                        sbNewRowXML.Append(chkbxCurrent.MapXML.Trim() + "=\"0\" ");
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return sbNewRowXML.ToString();
        }

        /// <summary>
        /// //This method will be called only for Note, Attach, Secure opeartions.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="treeNodeName"></param>
        /// <returns></returns>
        public string GetNewRowForAtchSecrNote(Control c, XmlDocument pgXml, string clkAction)
        {
            StringBuilder sbNewRowXML = new StringBuilder();
            sbNewRowXML.Append("<Rows ");
            sbNewRowXML.Append(GetNewRowSubForAtchSecrNote(c, pgXml, clkAction));
            sbNewRowXML.Append("/>");
            return sbNewRowXML.ToString();
        }

        private string GetNewRowSubForAtchSecrNote(Control c, XmlDocument xdoc, string clkAction)
        {
            StringBuilder sbNewRowXML = new StringBuilder();
            foreach (Control ctrl in c.Controls)
            {
                if (ctrl is LAjitTextBox)
                {
                    LAjitTextBox txtCurrent = ((LAjitTextBox)ctrl);
                    if (txtCurrent.MapBranchNode == null || txtCurrent.MapBranchNode == string.Empty)
                    {
                        if (txtCurrent.MapXML != null)
                        {
                            string txtboxValue = txtCurrent.Text.TrimEnd().TrimStart().ToString();

                            //Currency IsNumeric controls
                            if (BtnsUC != null)
                            {
                                //XmlDocument xdoc = new XmlDocument();
                                //xdoc.LoadXml(BtnsUC.GVDataXml);
                                string xPath = "//Columns/Col[@Label='" + (txtCurrent).MapXML.Trim() + "']";
                                XmlNode nodecols = xdoc.SelectSingleNode(xPath);
                                if (nodecols != null)
                                {
                                    if (nodecols.Attributes["IsNumeric"] != null)
                                    {
                                        // IsNumberic is 1 or ControlType is Amount remove comma characters added on 06-11-08
                                        if ((nodecols.Attributes["IsNumeric"].Value == "1") || (nodecols.Attributes["ControlType"].Value == "Amount"))
                                        {
                                            if (txtCurrent.Text.Contains(","))
                                            {
                                                txtboxValue = txtCurrent.Text.Replace(",", "");
                                            }
                                        }
                                    }
                                }
                            }
                            sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtboxValue) + "\" ");
                        }
                    }
                }
                else if (ctrl is LAjitDropDownList)
                {
                    LAjitDropDownList ddlCurrent = (LAjitDropDownList)ctrl;
                    if (ddlCurrent.MapXML.ToString() != string.Empty)
                    {
                        if (ddlCurrent.MapBranchNode == null || ddlCurrent.MapBranchNode == string.Empty)
                        {
                            string ddlRow = string.Empty;
                            string newTrxIDType = string.Empty;
                            newTrxIDType = ddlCurrent.SelectedValue.Trim();
                            if (newTrxIDType.Length == 0)
                            {
                                continue;
                            }
                            string[] strarr = newTrxIDType.Split('~');
                            string trxID = strarr[0].ToString();
                            string trxType = strarr[1].ToString();

                            if (clkAction.ToUpper() != "MODIFY")
                            {
                                if (trxID != string.Empty && trxType != string.Empty)
                                {
                                    //TrxID should not -1
                                    if (trxID != "-1")
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + trxID + "\"  "
                                                + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + trxType + "\"  ";
                                    }
                                }
                                else
                                {
                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                }
                            }
                            else
                            {
                                if (trxID != string.Empty && trxType != string.Empty)
                                {
                                    if (trxID == "-1")
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + string.Empty + "\"  "
                                                + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + string.Empty + "\"  ";

                                    }
                                    else
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + trxID + "\"  "
                                                + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + trxType + "\"  ";
                                    }
                                }
                                else
                                {
                                    //Appending only text when TrxId and TrxType are empty
                                    if (trxID == "-1")
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                                    }
                                    else
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                    }
                                }
                            }
                            if (ddlRow != string.Empty)
                            {
                                sbNewRowXML.Append(ddlRow);
                            }
                        }
                    }
                }
                else if (ctrl is LAjitCheckBox)
                {
                    LAjitCheckBox chkCurrent = (LAjitCheckBox)ctrl;
                    if (chkCurrent.MapBranchNode == null || chkCurrent.MapBranchNode == string.Empty)
                    {
                        if (chkCurrent.Checked)
                        {
                            //True
                            sbNewRowXML.Append(chkCurrent.MapXML.Trim() + "=\"1\" ");
                        }
                        else
                        {   //False
                            sbNewRowXML.Append(chkCurrent.MapXML.Trim() + "=\"0\" ");
                        }
                    }
                }
                else if (ctrl is HtmlTableRow)
                {
                    foreach (Control contrl in ctrl.Controls)
                    {
                        sbNewRowXML.Append(GetNewRowSubForAtchSecrNote(contrl, xdoc, clkAction));
                    }
                }
                else if (ctrl is HtmlTable)
                {
                    sbNewRowXML.Append(GetNewRowSubForAtchSecrNote(ctrl, xdoc, clkAction));
                }
            }
            return sbNewRowXML.ToString();
        }

        private string GetNewAutoFillRow(string CurrentMapXML, string CurrentText, XmlNode nodecol)
        {
            //Reading from cache
            string cacheName = Classes.AutoFill.GetLoggedInCompanyID() + CurrentMapXML;
            string cacheTrxID = string.Empty;
            string cacheTrxType = string.Empty;
            string cacheCreatedRow = string.Empty;
            string finalCreatedRow = string.Empty;


            if ((System.Web.HttpContext.Current.Cache[cacheName] != null) && (CurrentText.Trim() != ""))
            {
                DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];
                if (dsAutoFill.Tables[CurrentMapXML].Rows.Count > 0)
                {
                    //Step 1 look Exact match word
                    if (dsAutoFill.Tables[CurrentMapXML].Rows.Count > 0)
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(CurrentMapXML + "='" + CurrentText.Replace("'", "''") + "'");

                        if (drfoundrows.Length > 0)
                        {
                            cacheTrxID = drfoundrows[0][CurrentMapXML + "_TrxID"].ToString();
                            cacheTrxType = drfoundrows[0][CurrentMapXML + "_TrxType"].ToString();
                        }
                    }

                    // Step 2  Exact match not found take like first record
                    if ((cacheTrxID == string.Empty) && (cacheTrxType == string.Empty))
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(CurrentMapXML + " like '" + CurrentText.Trim().Replace("'", "''") + "%'");

                        if (drfoundrows.Length > 0)
                        {
                            cacheTrxID = drfoundrows[0][CurrentMapXML + "_TrxID"].ToString();
                            cacheTrxType = drfoundrows[0][CurrentMapXML + "_TrxType"].ToString();
                        }
                    }
                }
                // Step 3  Genearate row
                if ((cacheTrxID != string.Empty) && (cacheTrxType != string.Empty))
                {
                    if (cacheTrxID == "-1")
                    {
                        cacheCreatedRow = CurrentMapXML + "_TrxID=\"" + string.Empty + "\"  "
                                + CurrentMapXML + "_TrxType=\"" + string.Empty + "\"  ";
                        // + txtCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                    }
                    else
                    {
                        cacheCreatedRow = CurrentMapXML + "_TrxID=\"" + cacheTrxID + "\"  "
                                + CurrentMapXML + "_TrxType=\"" + cacheTrxType + "\"  ";
                        //  + txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                    }
                }
                else
                {
                    //trxid and trxtype is empty send text only
                    // cacheCreatedRow = txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                    // No need to create attribute  both trxid and trxtype are empty. we are removing these items at the end. on 04-11-03
                    // ADD operation sending both trxid and trxtype as empty and MODIFY case trxtype and trxtype are removed.
                    cacheCreatedRow = CurrentMapXML + "_TrxID=\"\" "
                                + CurrentMapXML + "_TrxType=\"\"  "
                                + CurrentMapXML + "=\"\"  ";
                }

                if (cacheCreatedRow != string.Empty)
                {
                    // sbGridViewXML.Append(cacheCreatedRow);
                    // isRowOk = true;
                    finalCreatedRow = cacheCreatedRow.ToString();
                }
                //Reading from cache
            }
            else
            {
                //if ((nodecol.Attributes["IsRequired"].Value == "1") && (BPAction != "Find"))
                if ((nodecol.Attributes["IsRequired"].Value == "1"))
                {
                    //Get Default value 

                    if ((System.Web.HttpContext.Current.Cache[cacheName] != null))
                    {
                        DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];

                        if (dsAutoFill.Tables[CurrentMapXML].Rows.Count > 0)
                        {
                            DataRow[] drfoundrows;
                            drfoundrows = dsAutoFill.Tables[0].Select(CurrentMapXML + "_TrxID " + "<>'-1'");

                            if (drfoundrows.Length > 0)
                            {
                                cacheTrxID = drfoundrows[0][CurrentMapXML + "_TrxID"].ToString();
                                cacheTrxType = drfoundrows[0][CurrentMapXML + "_TrxType"].ToString();
                                //txtCurrent.Text = drfoundrows[0][txtCurrent.MapXML.Trim()].ToString();
                            }
                        }
                    }
                    //Row Create default
                    if ((cacheTrxID != string.Empty) && (cacheTrxType != string.Empty))
                    {
                        if (cacheTrxID == "-1")
                        {
                            cacheCreatedRow = CurrentMapXML + "_TrxID=\"" + string.Empty + "\"  "
                                    + CurrentMapXML + "_TrxType=\"" + string.Empty + "\"  ";
                            // + txtCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                        }
                        else
                        {
                            cacheCreatedRow = CurrentMapXML + "_TrxID=\"" + cacheTrxID + "\"  "
                                    + CurrentMapXML + "_TrxType=\"" + cacheTrxType + "\"  ";
                            //  + txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                        }
                    }
                    //Row Create
                    if (cacheCreatedRow != string.Empty)
                    {
                        //  sbGridViewXML.Append(cacheCreatedRow);
                        //isRowOk = false;
                        finalCreatedRow = cacheCreatedRow.ToString();
                    }
                }
            }
            return finalCreatedRow;
        }
        #endregion

        #region Getting Page controls
        private string GetZeroPrefixString(int number)
        {
            if (number == 0)
            {
                return "000";
            }
            else
            {
                if (number <= 9)
                {
                    return "00" + number;
                }
                else if (number > 99)
                {
                    return number.ToString();
                }
                else
                {
                    return "0" + number;
                }
            }
        }

        public Dictionary<string, Control> GetPageControls(XmlDocument xDocForm, Panel container)
        {
            Dictionary<string, Control> dictPageControls = new Dictionary<string, Control>();

            //XmlNode nodeGridLayOut = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            //foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
            //{
            string treeNodeName = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            //Go the tree node and pick up the columns node.
            XmlNode nodeTreeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
            dictPageControls = FindPageControls(nodeTreeColumns, container, treeNodeName, null);

            //XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
            XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (nodeBranches != null)
            {
                int grdVwCntr = 0;
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                {
                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                    XmlAttribute attControlType = nodeBranch.Attributes["ControlType"];
                    if (attControlType != null && attControlType.Value == "GView")
                    {
                        //Add the GridView exclusively.
                        string controlType = attControlType.Value;
                        GridView grdVw = (GridView)container.FindControl("grdVw" + branchNodeName);
                        if (grdVw != null)
                        {
                            dictPageControls.Add("GridView" + GetZeroPrefixString(grdVwCntr++), grdVw);
                        }
                    }
                    else
                    {
                        //Rest of the Branch Controls.
                        XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                        Dictionary<string, Control> dictCurrentBranchControls = FindPageControls(nodeColumns, container, treeNodeName, branchNodeName);
                        //Add the above Branch controls to the Master Page Controls Container.
                        foreach (KeyValuePair<string, Control> kvPair in dictCurrentBranchControls)
                        {
                            dictPageControls.Add(kvPair.Key, kvPair.Value);
                        }
                    }
                }
            }
            //}
            return dictPageControls;
        }

        private Dictionary<string, Control> FindPageControls(XmlNode nodeColumns, Panel container, string treeNodeName, string branchNodeName)
        {
            Dictionary<string, Control> pageControls = new Dictionary<string, Control>();
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                string currentControlType = string.Empty;
                XmlAttribute attrCntrlType = nodeCol.Attributes["ControlType"];
                if (attrCntrlType != null)
                {
                    currentControlType = attrCntrlType.Value;
                }
                else
                {
                    continue;
                }
                string colLbl = nodeCol.Attributes["Label"].Value;
                switch (currentControlType)
                {
                    case "TBox":
                    case "Cal":
                    case "Calc":
                    case "Amount":
                    case "Passwd":
                    case "Phone":
                        {
                            string controlID = string.Empty;
                            if (branchNodeName != null && branchNodeName != string.Empty)
                            {
                                if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                                {
                                    controlID = "txt" + colLbl + "_" + branchNodeName;
                                }
                                else //means Parent Control
                                {
                                    controlID = "txt" + colLbl;
                                }
                            }
                            else
                            {
                                controlID = "txt" + colLbl;
                            }
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("TextBox" + GetZeroPrefixString(m_TxtBoxCntr++), control);
                            }
                        }
                        break;
                    case "DDL":
                        {
                            string controlID = string.Empty;

                            if (branchNodeName != null && branchNodeName != string.Empty)
                            {
                                if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                                {
                                    controlID = "ddl" + colLbl + "_" + branchNodeName;
                                }
                                else //means Parent Control
                                {
                                    controlID = "ddl" + colLbl;
                                }
                            }
                            else
                            {
                                controlID = "ddl" + colLbl;
                            }
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("DDL" + GetZeroPrefixString(m_DDLCntr++), control);
                            }
                        }
                        break;
                    case "Check":
                        {
                            string controlID = string.Empty;
                            if (branchNodeName != null && branchNodeName != string.Empty)
                            {
                                if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                                {
                                    controlID = "chk" + colLbl + "_" + branchNodeName;
                                }
                                else //means Parent Control
                                {
                                    controlID = "chk" + colLbl;
                                }
                            }
                            else
                            {
                                controlID = "chk" + colLbl;
                            }
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("CheckBox" + GetZeroPrefixString(m_ChkBxCntr++), control);
                            }
                        }
                        break;
                    case "Lbl"://need to check the control type from xml
                        {
                            string controlID = string.Empty;
                            if (branchNodeName != null && branchNodeName != string.Empty)
                            {
                                if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                                {
                                    controlID = "lbl" + colLbl + "_" + branchNodeName + "_Value";
                                }
                                else //means Parent Control
                                {
                                    //controlID = "lbl" + colLbl;
                                    controlID = "lbl" + colLbl + "_Value";
                                }
                            }
                            else
                            {
                                controlID = "lbl" + colLbl;
                            }
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("Label" + GetZeroPrefixString(m_LblCntr++), control);
                            }
                        }
                        break;
                    case "lnkbtn"://need to check the control type from xml
                        {
                            string controlID = string.Empty;
                            if (branchNodeName != null && branchNodeName != string.Empty)
                            {
                                if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                                {
                                    controlID = "lnkbtn" + colLbl + "_" + branchNodeName;
                                }
                                else //means Parent Control
                                {
                                    controlID = "lnkbtn" + colLbl;
                                }
                            }
                            else
                            {
                                controlID = "lnkbtn" + colLbl;
                            }
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("LinkButton" + GetZeroPrefixString(m_LnkBtnCntr++), control);
                            }
                        }
                        break;
                    case "LBox":
                        {
                            string controlID = "lstBx" + colLbl;
                            Control control = container.FindControl(controlID);
                            if (control != null)
                            {
                                pageControls.Add("ListBox" + GetZeroPrefixString(m_LstBoxCntr++), control);
                            }
                        }
                        break;
                    default:
                        break;
                }

                //To add Required Filed Validator also to the controlls collection.
                string cntrlID = string.Empty;
                if (branchNodeName != null && branchNodeName != string.Empty)
                {
                    if (branchNodeName != treeNodeName) //means Branch Control with control type is null
                    {
                        cntrlID = "req" + colLbl + "_" + branchNodeName;
                    }
                    else //means Parent Control
                    {
                        cntrlID = "req" + colLbl;
                    }
                }
                else
                {
                    cntrlID = "req" + colLbl;
                }
                Control cntrl = container.FindControl(cntrlID);
                if (cntrl != null)
                {
                    pageControls.Add("reqFldValidator" + GetZeroPrefixString(m_ValidateReqCntr++), cntrl);
                }

                //To add Regular Expression validator controls also to the controls collection.
                if (branchNodeName != null && branchNodeName != string.Empty)
                {
                    if (branchNodeName != treeNodeName)
                    {
                        cntrlID = "reg" + colLbl + "_" + branchNodeName;
                    }
                    else
                    {
                        cntrlID = "reg" + colLbl;
                    }
                }
                else
                {
                    cntrlID = "reg" + colLbl;
                }
                cntrl = container.FindControl(cntrlID);
                if (cntrl != null)
                {
                    pageControls.Add("regExpValidator" + GetZeroPrefixString(m_ValidateRegCntr++), cntrl);
                }

                //To add all HTMLTableRows also to the controlls collection.
                if (branchNodeName != null && branchNodeName != string.Empty)
                {
                    if (branchNodeName != treeNodeName)
                    {
                        cntrlID = "tr" + colLbl + "_" + branchNodeName;
                    }
                    else
                    {
                        cntrlID = "tr" + colLbl;
                    }
                }
                else
                {
                    cntrlID = "tr" + colLbl;
                }
                cntrl = container.FindControl(cntrlID);
                if (cntrl != null)
                {
                    pageControls.Add("htmlTblRw" + GetZeroPrefixString(m_htmlTblRwCntr++), cntrl);
                }
            }

            ////For Operational links
            //Control ctrl = container.FindControl("tblOperationLinks");
            //if (ctrl != null)
            //{
            //    pageControls.Add("opLinks" + GetZeroPrefixString(m_OpLinksCntr++), ctrl);
            //}

            return pageControls;
        }

        private string GetPageColumns(XmlDocument xDocForm, Panel container)
        {
            string strPageControls = string.Empty;
            XmlNode nodeGridLayOut = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
            {
                string treeNodeName = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                //Go the tree node and pick up the columns node.
                XmlNode nodeTreeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                strPageControls = FindPageColumns(nodeTreeColumns, null, string.Empty);

                //XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    //int grdVwCntr = 0;
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlAttribute attControlType = nodeBranch.Attributes["ControlType"];
                        if (attControlType != null && attControlType.Value == "GView")
                        {
                            //Add the GridView exclusively.
                            //string controlType = attControlType.Value;
                            //GridView grdVw = (GridView)container.FindControl("grdVw" + branchNodeName);
                            //if (grdVw != null)
                            //{
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, attControlType.Value);
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                            //}
                        }
                        else
                        {
                            //Rest of the Branch Controls.
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, string.Empty);
                            //Add the above Branch controls to the Master Page Controls Container.
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                        }
                    }
                }
            }
            return strPageControls;
        }

        public string FindPageColumns(XmlNode nodeColumns, string branchNodeName, string brnchCtrlType)
        {
            string formCol = string.Empty;
            string strCol = string.Empty;
            string delimiter = "|~|";
            StringBuilder sbDefaults = new StringBuilder();//Create a Defaults Variable script.
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                XmlAttribute attrLabel = nodeCol.Attributes["Label"];
                XmlAttribute attrControlType = nodeCol.Attributes["ControlType"];
                XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                XmlAttribute attrDisplayOnly = nodeCol.Attributes["IsDisplayOnly"];
                XmlAttribute attrIsRequired = nodeCol.Attributes["IsRequired"];
                XmlAttribute attrIsNumeric = nodeCol.Attributes["IsNumeric"];
                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                XmlAttribute attrIsSearched = nodeCol.Attributes["IsSearched"];
                if (attrLabel != null)
                {
                    formCol = attrLabel.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrControlType != null)
                {
                    FormatDefaultAttributes(nodeCol);
                    formCol = formCol + attrControlType.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrDefault != null)
                {
                    string varName = "def_" + branchNodeName + "_" + attrLabel.Value;
                    formCol += varName + delimiter;
                    sbDefaults.Append("var " + varName + "='" + this.HtmlEncode(attrDefault.Value) + "';");
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrDisplayOnly != null)
                {
                    formCol = formCol + attrDisplayOnly.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrIsRequired != null)
                {
                    formCol = formCol + attrIsRequired.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrIsNumeric != null)
                {
                    formCol = formCol + attrIsNumeric.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrIsHidden != null)
                {
                    formCol = formCol + attrIsHidden.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                if (attrIsSearched != null)
                {
                    formCol = formCol + attrIsSearched.Value + delimiter;
                }
                else
                {
                    formCol = formCol + "-1" + delimiter;
                }
                strCol = strCol + formCol + ":";
            }
            //Emit the Defaults variables script to the client.
            if (this.ButtonsUserControl != null)
            {
                ScriptManager.RegisterStartupScript(this.ButtonsUserControl.Page, this.ButtonsUserControl.GetType(), "Defaults" + branchNodeName, sbDefaults.ToString(), true);
            }
            return branchNodeName + "/" + brnchCtrlType + ";" + strCol;
        }
        #endregion

        #region Other supporting methods

        // This method is replaing the EnableUI, Clear UI and Color UI functionmality written previously 
        // in the server side with the client side by invoking a common javascript.
        public void InvokeOnButtonClick(string mode, Page page)
        {
            string s = "javascript:OnButtonClick('" + mode + "');";
            ScriptManager.RegisterStartupScript(page, this.GetType(), "ButtonClick", s, true);
        }

        public void FillDropDownData(string returnXML, ArrayList listColumns, Control pnlEntryForm)
        {
            #region NLog
            logger.Info("This method is replaing the EnableUI, Clear UI and Color UI functionmality written previously in the server side with the client side by invoking a common javascript.");
            #endregion

            try
            {
                for (int count = 0; count <= listColumns.Count - 1; count++)
                {
                    string control = "ddl" + listColumns[count].ToString();
                    DropDownList ddlist = (DropDownList)pnlEntryForm.FindControl(control);
                    if (ddlist != null)
                    {
                        XmlDataSource xDS = new XmlDataSource();
                        xDS.EnableCaching = false;
                        xDS.Data = returnXML;
                        string colLabel = string.Empty;
                        if (listColumns[count].ToString().Contains("_"))
                        {
                            string[] strarr = listColumns[count].ToString().Split('_');
                            if (strarr[0].ToString() != string.Empty)
                            {
                                colLabel = strarr[0].ToString().Trim(); ;
                            }
                        }
                        if (colLabel != string.Empty)
                        {
                            xDS.XPath = "Root/bpeout/FormControls/" + colLabel + "/RowList/Rows";
                        }
                        else
                        {
                            xDS.XPath = "Root/bpeout/FormControls/" + listColumns[count].ToString() + "/RowList/Rows";
                        }

                        ddlist.DataValueField = "DataValueField";
                        if (colLabel != string.Empty)
                        {
                            ddlist.DataTextField = colLabel;
                        }
                        else
                        {
                            ddlist.DataTextField = listColumns[count].ToString().Trim();
                        }

                        ddlist.DataSource = xDS;
                        ddlist.DataBind();

                        //Adding the Initial value from the XML to validate the DDL against.
                        if (ddlist.Items.Count > 0)
                        {
                            LAjitRequiredFieldValidator reqCntrlId = (LAjitRequiredFieldValidator)pnlEntryForm.FindControl("req" + listColumns[count].ToString());
                            if (reqCntrlId != null)
                            {
                                if (ddlist.Items[0].Text == "Select" || ddlist.Items[0].Text == "Selection Required" || ddlist.Items[0].Text == "Choose")
                                {
                                    //XmlDocument xmlDoc = new XmlDocument();
                                    //xmlDoc.LoadXml(returnXML);
                                    //string DataValueField = xmlDoc.SelectSingleNode("Root/bpeout/FormControls/" + colLabel + "/RowList/Rows[@TrxId='-1']").Attributes[""].Value;
                                    reqCntrlId.InitialValue = ddlist.Items[0].Value;
                                    ddlist.Attributes["MapPreviousSelItem"] = ddlist.Items[0].Text;
                                }
                            }
                        }
                        //Disposing data src
                        xDS.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void ValidateControls(XmlNode nodeColumns, Panel container)
        {
            #region NLog
            logger.Info("This method is used to validate the page controls in the container");
            #endregion

            //Looping through all page controls both parent and child.
            if (m_PageControls == null)
            {
                m_PageControls = GetPageControls(nodeColumns.OwnerDocument, container);
            }
            foreach (KeyValuePair<string, Control> kvPair in m_PageControls)
            {
                string strCntrl = kvPair.Key.Substring(0, Convert.ToInt32(kvPair.Key.Length) - 3);

                string[] arr = kvPair.Value.ID.Split('_');
                XmlNode parentCols = nodeColumns;
                if (arr.Length > 1)
                {
                    XmlNode brnchcols = nodeColumns.OwnerDocument.SelectSingleNode("Root/bpeout/FormControls/" + arr[1] + "/GridHeading/Columns");
                    nodeColumns = brnchcols;
                }
                else
                {
                    nodeColumns = parentCols;
                }
                switch (strCntrl)
                {
                    case "reqFldValidator":
                        {
                            LAjitRequiredFieldValidator reqCurrent = (LAjitRequiredFieldValidator)kvPair.Value;
                            XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + reqCurrent.MapXML + "']");

                            if (nodeCol != null)
                            {
                                XmlAttribute attrIsRequired = nodeCol.Attributes["IsRequired"];
                                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                                if ((attrIsRequired != null) && (attrIsHidden != null))
                                {
                                    if ((attrIsRequired.Value != "0") && (attrIsHidden.Value != "1"))
                                    {
                                        reqCurrent.Enabled = true;
                                        if (nodeCol.Attributes["Caption"] != null)
                                        {
                                            reqCurrent.ErrorMessage = nodeCol.Attributes["Caption"].Value;
                                        }
                                    }
                                    else
                                    {
                                        reqCurrent.Enabled = false;
                                    }
                                }
                            }
                        }
                        break;
                    case "regExpValidator":
                        {
                            LAjitRegularExpressionValidator regCurrent = (LAjitRegularExpressionValidator)kvPair.Value;
                            XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + regCurrent.MapXML + "']");
                            if (nodeCol != null)
                            {
                                XmlAttribute attrIsNumeric = nodeCol.Attributes["IsNumeric"];
                                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                                if (attrIsNumeric != null)
                                {
                                    if ((attrIsNumeric.Value != "0") && (attrIsHidden.Value != "1"))
                                    {
                                        regCurrent.Enabled = true;
                                        if (nodeCol.Attributes["Caption"] != null)
                                        {
                                            regCurrent.ErrorMessage = nodeCol.Attributes["Caption"].Value + " " + "should be Numeric";
                                        }
                                    }
                                    //else
                                    //{
                                    //    if (!regCurrent.MapXML.Contains("Email"))
                                    //        regCurrent.Enabled = false;
                                    //}
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetLabelText(XmlDocument xDocout, out ArrayList alColIslink, Panel pnlEntryForm)
        {
            ContentPlaceHolder cphPageContents = new ContentPlaceHolder();
            cphPageContents = (ContentPlaceHolder)pnlEntryForm.Page.Master.FindControl("cphPageContents");
            Panel pnlContent = new Panel();
            pnlContent = (Panel)cphPageContents.FindControl("pnlContent");
            UpdatePanel updtPnlContent = new UpdatePanel();
            updtPnlContent = (UpdatePanel)pnlContent.FindControl("updtPnlContent");
            Panel pnlCPGV1Title = new Panel();
            pnlCPGV1Title = (Panel)updtPnlContent.FindControl("pnlCPGV1Title");
            Panel pnlGVContent = new Panel();
            pnlGVContent = (Panel)updtPnlContent.FindControl("pnlGVContent");
            if (pnlCPGV1Title != null)
            {
                m_pnlTitleID = pnlCPGV1Title.ClientID;
            }
            if (pnlGVContent != null)
            {
                m_pnlContentID = pnlGVContent.ClientID;
            }
            m_imgID = "imgCPGV1Expand";

            alColIslink = new ArrayList();

            //SET cdn image path
            HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
            string strImagesCDNPath = (String)ctx.Application["ImagesCDNPath"];


            //Filling Tree Labels
            XmlNode nodeColumns = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/GridHeading/Columns");
            if (nodeColumns != null)
            {
                foreach (XmlNode colnode in nodeColumns.ChildNodes)
                {
                    //Collecting all the columns which have IsRequired property.                    
                    if (colnode.Attributes["ControlType"] != null)
                    {
                        if (colnode.Attributes["ControlType"].Value == "DDL")
                        {
                            alColIslink.Add(colnode.Attributes["Label"].Value);
                        }
                    }
                    //Initialising the corresponding label for the current column.
                    string control = "lbl" + colnode.Attributes["Label"].Value.ToString();
                    Label lblCurrent = (Label)pnlEntryForm.FindControl(control);
                    if (lblCurrent != null)
                    {
                        lblCurrent.Text = colnode.Attributes["Caption"].Value;
                    }
                    //Adding TextboxClientIDs for Calendar,Calculator,AutoComplete for JQUERY
                    if (colnode.Attributes["ControlType"] != null)
                    {
                        if (colnode.Attributes["ControlType"].Value == "Cal")
                        {
                            string txtcontrol = "txt" + colnode.Attributes["Label"].Value.ToString();
                            LAjitTextBox txtCurrent = (LAjitTextBox)pnlEntryForm.FindControl(txtcontrol);
                            if (txtCurrent != null)
                            {
                                m_alCalendarTBoxIDS.Add(txtCurrent.ClientID);
                                m_alMaskTBoxIDS.Add(txtCurrent.ClientID);
                                //string themeName = Convert.ToString(HttpContext.Current.Session["MyTheme"]);

                                txtCurrent.Attributes.Add("ShowIcon", "true");
                                txtCurrent.Attributes.Add("IconAlign", "Right");
                                txtCurrent.Attributes.Add("IconOnClick", "javascript:ShowCalendar('" + txtCurrent.ClientID + "');");
                                txtCurrent.Attributes.Add("IconAlternateText", "Show Calendar");
                                txtCurrent.Attributes.Add("IconPath", strImagesCDNPath + "images/calendar-icon.gif");
                            }
                        }
                        if (colnode.Attributes["ControlType"].Value == "Calc")
                        {
                            string CalControl = "txt" + colnode.Attributes["Label"].Value.ToString();
                            LAjitTextBox txtCurrent = (LAjitTextBox)pnlEntryForm.FindControl(CalControl.Trim());
                            //txtCurrent.Attributes.Add("ondblclick", "javascript:ShowCalculator('" + txtCurrent.ClientID + "')");
                            //m_alCalcTBoxIDS.Add(txtCurrent.ClientID);

                            //string themeName = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
                            txtCurrent.Attributes.Add("ShowIcon", "true");
                            txtCurrent.Attributes.Add("IconAlign", "Right");
                            txtCurrent.Attributes.Add("MaintainWidth", "true");
                            txtCurrent.Attributes.Add("IconOnClick", "javascript:ShowCalculator('" + txtCurrent.ClientID + "');");
                            txtCurrent.Attributes.Add("IconAlternateText", "Show Calculator");
                            //txtCurrent.Attributes.Add("IconPath", "../App_Themes/" + themeName + "/images/calendar-icon.gif");
                            txtCurrent.Attributes.Add("IconPath", strImagesCDNPath + "images/calculator.png");
                        }
                        if (colnode.Attributes["ControlType"].Value == "TBox")
                        {
                            if ((colnode.Attributes["IsLink"] != null) && (colnode.Attributes["IsLink"].Value == "1"))
                            {
                                string Control = "txt" + colnode.Attributes["Label"].Value.ToString();
                                TextBox txtCurrent = (TextBox)pnlEntryForm.FindControl(Control.Trim());
                                //m_alAutoCompleteTBoxIDS.Add(txtCurrent.ClientID);
                                m_htAutoCompleteTBoxIDS.Add(txtCurrent.ClientID, colnode.Attributes["Label"].Value.ToString());
                            }
                        }
                        //Phone
                        if (colnode.Attributes["ControlType"].Value == "Phone")
                        {
                            string Control = "txt" + colnode.Attributes["Label"].Value.ToString();
                            TextBox txtCurrent = (TextBox)pnlEntryForm.FindControl(Control.Trim());
                            if (txtCurrent != null)
                            {
                                m_alPhoneMaskTBoxIDS.Add(txtCurrent.ClientID);
                            }
                        }
                    }
                }
            }
            //Filling Branch Labels
            //Checking if branches exists
            XmlNode nodeBranches = xDocout.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (nodeBranches != null)
            {
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    if (nodeBranch.Attributes["ControlType"] == null)
                    {
                        //Each branch in branches
                        XmlNode nodeBranchcol = nodeBranch.SelectSingleNode("Node");
                        if (nodeBranchcol != null)
                        {
                            //Columns node for each branch
                            XmlNode nodeBranchColumns = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + nodeBranchcol.InnerXml + "/GridHeading/Columns");
                            if (nodeBranchColumns != null)
                            {
                                foreach (XmlNode colBranchnode in nodeBranchColumns.ChildNodes)
                                {
                                    if (colBranchnode.Attributes["ControlType"] != null)
                                    {
                                        if (colBranchnode.Attributes["ControlType"].Value == "DDL")
                                        {
                                            alColIslink.Add(colBranchnode.Attributes["Label"].Value + "_" + nodeBranchcol.InnerXml.ToString());
                                        }
                                    }
                                    //Initialising the corresponding label for the current column.
                                    string control = "lbl" + colBranchnode.Attributes["Label"].Value.ToString() + "_" + nodeBranchcol.InnerXml.ToString();
                                    Label lblCurrent = (Label)pnlEntryForm.FindControl(control.Trim());
                                    if (lblCurrent != null)
                                    {
                                        lblCurrent.Text = colBranchnode.Attributes["Caption"].Value;
                                    }
                                    //Adding TextboxClientIDs for Calendar,Calculator,AutoComplete for JQUERY
                                    if (colBranchnode.Attributes["ControlType"] != null)
                                    {
                                        if (colBranchnode.Attributes["ControlType"].Value == "Cal")
                                        {
                                            string txtcontrol = "txt" + colBranchnode.Attributes["Label"].Value.ToString() + "_" + nodeBranchcol.InnerXml.ToString();
                                            LAjitTextBox txtCurrent = (LAjitTextBox)pnlEntryForm.FindControl(txtcontrol.Trim());
                                            if (txtCurrent != null)
                                            {
                                                m_alCalendarTBoxIDS.Add(txtCurrent.ClientID);
                                                m_alMaskTBoxIDS.Add(txtCurrent.ClientID);
                                                //string themeName = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
                                                txtCurrent.Attributes.Add("ShowIcon", "true");
                                                txtCurrent.Attributes.Add("IconAlign", "Right");
                                                txtCurrent.Attributes.Add("IconOnClick", "javascript:ShowCalendar('" + txtCurrent.ClientID + "');");
                                                txtCurrent.Attributes.Add("IconAlternateText", "Show Calendar");
                                                txtCurrent.Attributes.Add("IconPath", strImagesCDNPath + "images/calendar-icon.gif");
                                            }
                                        }
                                        if (colBranchnode.Attributes["ControlType"].Value == "TBox")
                                        {
                                            if ((colBranchnode.Attributes["IsLink"] != null) && (colBranchnode.Attributes["IsLink"].Value == "1"))
                                            {
                                                string Control = "txt" + colBranchnode.Attributes["Label"].Value.ToString() + "_" + nodeBranchcol.InnerXml.ToString();
                                                TextBox txtCurrent = (TextBox)pnlEntryForm.FindControl(Control.Trim());
                                                //m_alAutoCompleteTBoxIDS.Add(txtCurrent.ClientID);
                                                if (txtCurrent != null)
                                                {
                                                    m_htAutoCompleteTBoxIDS.Add(txtCurrent.ClientID, colBranchnode.Attributes["Label"].Value.ToString());
                                                }
                                            }
                                        }
                                        //Phone
                                        if (colBranchnode.Attributes["ControlType"].Value == "Phone")
                                        {
                                            string Control = "txt" + colBranchnode.Attributes["Label"].Value.ToString() + "_" + nodeBranchcol.InnerXml.ToString();
                                            TextBox txtCurrent = (TextBox)pnlEntryForm.FindControl(Control.Trim());
                                            if (txtCurrent != null)
                                            {
                                                m_alPhoneMaskTBoxIDS.Add(txtCurrent.ClientID);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public void HandleIsHidden(Control c, XmlDocument xDoc, bool status)
        {
            string val = string.Empty;
            string parentTreeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + parentTreeNode + "/GridHeading/Columns");

            if (m_PageControls == null)
            {
                m_PageControls = GetPageControls(nodeColumns.OwnerDocument, (Panel)c);
            }
            foreach (KeyValuePair<string, Control> kvPair in m_PageControls)
            {
                string strCntrl = kvPair.Key.Substring(0, Convert.ToInt32(kvPair.Key.Length) - 3);
                string[] arr = kvPair.Value.ID.Split('_');

                //To get a particular column for 'IsHidden' attribute as we dont have MapXML for htmlTblRw.
                string refer = arr[0].Substring(0, 2);
                if (refer == "tr")
                {
                    val = arr[0].Substring(2, Convert.ToInt32(arr[0].Length) - 2);
                }
                XmlNode parentCols = nodeColumns;
                if (arr.Length > 1)
                {
                    XmlNode brnchcols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + arr[1] + "/GridHeading/Columns");
                    nodeColumns = brnchcols;
                }
                else
                {
                    nodeColumns = parentCols;
                }

                if (strCntrl == "htmlTblRw")
                {
                    HtmlTableRow trCurrent = (HtmlTableRow)kvPair.Value;
                    if (!string.IsNullOrEmpty(val))
                    {
                        XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + val + "']");
                        XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                        if (attrIsHidden != null)
                        {
                            if (attrIsHidden.Value != "0")
                            {
                                trCurrent.Visible = status;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>This method is to set default values specially in ProcessEngine/SelectRequest pages</summary>
        /// <param name="c">EntryForm control object to get page controls</param>
        /// <param name="nodeColumns">To get IsDefault attribute of every column</param>        
        public void SetDefault(Control c, XmlNode nodeColumns)
        {
            //Looping through all page controls to change Color,Clear,Enable and Default
            if (m_PageControls == null)
            {
                m_PageControls = GetPageControls(nodeColumns.OwnerDocument, (Panel)c);
            }
            foreach (KeyValuePair<string, Control> kvPair in m_PageControls)
            {
                string strCntrl = kvPair.Key.Substring(0, Convert.ToInt32(kvPair.Key.Length) - 3);

                string[] arr = kvPair.Value.ID.Split('_');
                XmlNode parentCols = nodeColumns;
                if (arr.Length > 1)
                {
                    XmlNode brnchcols = nodeColumns.OwnerDocument.SelectSingleNode("Root/bpeout/FormControls/" + arr[1] + "/GridHeading/Columns");
                    nodeColumns = brnchcols;
                }
                else
                {
                    nodeColumns = parentCols;
                }
                switch (strCntrl)
                {
                    case "TextBox":
                        {
                            LAjitTextBox txtCurrent = (LAjitTextBox)kvPair.Value;
                            if (!string.IsNullOrEmpty(txtCurrent.MapXML))
                            {
                                XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + txtCurrent.MapXML + "']");
                                //To Set Default values
                                if (nodeCol != null)
                                {
                                    XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                                    if (attrDefault != null)
                                    {
                                        DateTime date;
                                        if (IsDate(attrDefault.Value))
                                        {
                                            //if the value is IsDate then change format MM/DD/YYYY 13-11-2008
                                            DateTime.TryParse(attrDefault.Value, out date);
                                            txtCurrent.Text = date.ToString("MM/dd/yy");
                                        }
                                        else
                                        {
                                            //Control type is Amount or Calc format the amount added on 13-11-08.
                                            if (nodeCol.Attributes["ControlType"] != null)
                                            {
                                                if ((nodeCol.Attributes["ControlType"].Value == "Amount") || (nodeCol.Attributes["ControlType"].Value == "Calc"))
                                                {
                                                    decimal amount;
                                                    if (Decimal.TryParse(attrDefault.Value, out amount))
                                                    {
                                                        txtCurrent.Text = string.Format("{0:N}", amount);
                                                    }
                                                }
                                                else
                                                {
                                                    txtCurrent.Text = attrDefault.Value;
                                                }
                                            }
                                            else
                                            {
                                                txtCurrent.Text = attrDefault.Value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "DDL":
                        {
                            LAjitDropDownList ddlCurrent = (LAjitDropDownList)kvPair.Value;
                            if (ddlCurrent.MapXML != string.Empty)
                            {
                                if (ddlCurrent.Items.Count > 0)
                                {
                                    XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + ddlCurrent.MapXML.ToString() + "']");
                                    //To Set Default values                                    
                                    if (nodeCol != null)
                                    {
                                        XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                                        if (attrDefault != null)
                                        {
                                            string dataValueField = string.Empty;
                                            string[] strarr = ddlCurrent.Items[0].Value.Split('~');
                                            //Getting TrxType
                                            if (strarr[1] != string.Empty)
                                            {
                                                dataValueField = attrDefault.Value.Trim() + "~" + strarr[1];
                                            }
                                            if (dataValueField != string.Empty)
                                            {
                                                ddlCurrent.SelectedIndex = ddlCurrent.Items.IndexOf(ddlCurrent.Items.FindByValue(dataValueField));
                                                ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.SelectedItem.Text;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "CheckBox":
                        {
                            LAjitCheckBox chkbxCurrent = (LAjitCheckBox)kvPair.Value;
                            if (chkbxCurrent.MapXML != string.Empty)
                            {
                                XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + chkbxCurrent.MapXML + "']");
                                //To Set Default values                                
                                if (nodeCol != null)
                                {
                                    XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                                    if (attrDefault != null)
                                    {
                                        if (attrDefault.Value == "1")
                                        {
                                            chkbxCurrent.Checked = true;
                                        }
                                        else
                                        {
                                            chkbxCurrent.Checked = false;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "Label":
                        {
                            LAjitLabel lblCurrent = (LAjitLabel)kvPair.Value;
                            if (lblCurrent.MapXML != string.Empty)
                            {
                                //To Set Default values                                
                                XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + lblCurrent.MapXML + "']");
                                if (nodeCol != null)
                                {
                                    XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                                    if (attrDefault != null)
                                    {
                                        lblCurrent.Text = nodeCol.Attributes["Default"].Value;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public string SetTimer(XmlDocument xDoc, Control CurrentPage)
        {
            string setTimer = string.Empty;
            if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='AutoSave']") != null))
            {
                Timer timer = (Timer)CurrentPage.FindControl("timerEntryForm");
                string refreshRate = GetTimerRefreshRate();
                if (refreshRate != string.Empty)
                {
                    if (Convert.ToInt32(refreshRate) != 0)
                    {
                        //Assigning the timer's interval in milliseconds.
                        timer.Interval = Convert.ToInt32(refreshRate) * 1000;
                        setTimer = "Set";
                    }
                }
            }
            return setTimer;
        }

        // <summary>
        /// As hdnParentColNode string(used in BtnsUC.js) is formed by cols seperated by ':' and dafault date consists dd/mm/yy min:sec,
        /// here min:sec being removed from that string as we require only date in that string.Similarly Amount col also.
        /// </summary>
        /// <param name="nodeColumns">page columns</param
        public void FormatDefaultAttributes(XmlNode nodeCol)
        {
            XmlAttribute attrControlType = nodeCol.Attributes["ControlType"];
            XmlAttribute attrDefault = nodeCol.Attributes["Default"];
            if (attrDefault != null)
            {
                if (attrControlType.Value == "Cal")
                {
                    DateTime date;
                    if (IsDate(attrDefault.Value))
                    {
                        //Parsing is to convert any string val to date object.
                        DateTime.TryParse(attrDefault.Value, out date);
                        attrDefault.InnerXml = date.ToString("MM/dd/yy");
                    }
                }
                else if (attrControlType.Value == "Amount" || attrControlType.Value == "Calc")
                {
                    decimal amount;
                    if (Decimal.TryParse(attrDefault.Value, out amount))
                    {
                        attrDefault.InnerXml = string.Format("{0:N}", amount);
                    }
                }
            }
        }

        private string GetCurrentActionBPGID(string CurrAction, XmlDocument returnXML)
        {
            #region NLog
            logger.Info("This method gets the current Action as " +CurrAction+ " from the return XML");
            #endregion

            string actionBPGID = string.Empty;
            //set ACTIONBPGID attribute
            if (returnXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='" + CurrAction + "']") != null)
            {
                actionBPGID = returnXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='" + CurrAction + "']").Attributes["BPGID"].Value;
            }
            return actionBPGID;
        }

        /// <summary>
        /// Gets the value of the user preference from the given XML.
        /// </summary>
        /// <param name="ruleName">Preference name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        public string GetUserPreferenceValue(string ruleID)
        {

            string retVal = string.Empty;
            //Requesting page size for full view page.
            if (BtnsUC != null)
            {
                if (BtnsUC.Page.ToString().Contains("fullview") && ruleID == "59")
                {
                    retVal = "22";
                }
                else
                {
                    retVal = GetPreferenceValue(ruleID);
                }
            }
            else
            {
                retVal = GetPreferenceValue(ruleID);
            }
            #region NLog
            logger.Info("Retrieved the User Preference value of " + ruleID + " as " + retVal);
            #endregion
            return retVal;
        }

        public string GetPreferenceValue(string bRuleID)
        {
            //Getting the corresponding rule name ID from the config file.
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
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
        /// Gets the BPGID of the Businees Process Control from GBPC Session.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <returns>string BPGID</returns>
        public string GetBPCBPGID(string processName)
        {
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string retVal = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["BPGID"].Value;

            #region NLog
            logger.Info("The BPGID of the Business Process from GBPC Session retreived as : " + retVal);
            #endregion

            return retVal;

        }

        public string GetTimerRefreshRate()
        {
            string m_BusinessRules = string.Empty;

            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference") != null)
            {
                m_BusinessRules = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference").OuterXml;
            }
            else
            {
                m_BusinessRules = string.Empty;
            }
            //Settimg timer to refresh the form
            string refreshRate = string.Empty;
            //EntryFormRefreshInterval
            refreshRate = GetBusinessRulesValue("205", m_BusinessRules);
            return refreshRate;
        }

        /// <summary>
        /// Gets the value of the business rule from the given XML.
        /// </summary>
        /// <param name="ruleName">Rule name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns>String Business Rules value(A number actually).</returns>
        private string GetBusinessRulesValue(string PrefRuleID, string userPreferenceNode)
        {
            #region NLog
            logger.Info("Getting the value of the business rule from the given XML with preference ruled id as : "+PrefRuleID+" and user preference node as : "+userPreferenceNode);
            #endregion

            if (userPreferenceNode == string.Empty)
            {
                return string.Empty;
            }
            //Getting the corresponding rule name ID from the config file.
            //string PrefRuleID = ConfigurationManager.AppSettings[prefName];
            XmlDocument xDocUsrPref = new XmlDocument();
            xDocUsrPref.LoadXml(userPreferenceNode);
            if (xDocUsrPref.SelectSingleNode("userpreference/Preference[@TypeOfPreferenceID='"
                                   + PrefRuleID + "']") != null)
            {
                return xDocUsrPref.SelectSingleNode("userpreference/Preference[@TypeOfPreferenceID='"
                                   + PrefRuleID + "']").Attributes["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool IsDate(string strDate)
        {
            try
            {
                decimal decString;
                if (!decimal.TryParse(strDate, out decString))
                {
                    DateTime dateString;
                    if (DateTime.TryParse(strDate, out dateString))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts symbols to html codes. 
        /// </summary>
        public string CharactersToHtmlCodes(string InputString)
        {
            char[] Findchars = { '&', '<', '>', '"' };
            string[] Replacechars = { "&amp;", "&lt;", "&gt;", "&quot;" };

            string OutputString = "";
            OutputString = InputString;
            for (int i = 0; i <= Findchars.Length - 1; i++)
            {
                OutputString = OutputString.Replace(Findchars[i].ToString(), Replacechars[i].ToString());
            }
            return OutputString;
        }

        /// <summary>
        /// Converts html codes to symbols.
        /// </summary>
        /// <param name="InputString">string</param>
        /// <returns>string</returns>
        public string HtmlCodesToCharacters(string InputString)
        {
            string[] Findchars = { "&amp;", "&lt;", "&gt;", "&quot;", "&apos;" };
            char[] Replacechars = { '&', '<', '>', '"', '\'' };

            string OutputString = "";
            OutputString = InputString;
            for (int i = 0; i <= Findchars.Length - 1; i++)
            {
                OutputString = OutputString.Replace(Findchars[i].ToString(), Replacechars[i].ToString());
            }
            return OutputString;
        }

        /// <summary>
        /// Encodes the given string into HTML-safe text.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>HTML Encoded string.</returns>
        public string HtmlEncode(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        /// <summary>
        /// Downloading file from given file path 
        /// </summary>
        /// <param name="filePath">string</param>
        public void DownloadFile(string filePath)
        {

            #region NLog
            logger.Info("Downloading file from given file path : " +filePath);
            #endregion
            try
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filePath));
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.WriteFile(filePath);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {

                #region NLog
                logger.Fatal(ex);
                #endregion
                throw ex;
            }
        }

        /// <summary>
        /// Sets the title of the given grid view.
        /// </summary>
        /// <param name="htcWork">Target HTML Table Cell.</param>
        /// <param name="gridTitle">String title to be set.</param>
        public void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            //Clip thte Grid Title if more than specified
            if (gridTitle.Length > 100)
            {
                gridTitle = gridTitle.Substring(0, 100) + "...";
            }

            //Clear any previously persisting text within the HeaderCell
            htcWork.InnerText = "";

            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // need bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;

            string[] strarr = gridTitle.Replace(">>>", ">").Split('>');
            if (strarr.Length > 1 && m_ShowLinksInTitle)//Popup page with more than 1 link
            {
                Table tblNavLinks = new Table();//Creating Nav Links table
                TableRow tr = new TableRow();//Creating Nav Links Row
                tblNavLinks.Rows.Add(tr);

                for (int linksCnt = 1; linksCnt <= strarr.Length; linksCnt++)
                {
                    //TD for the Link Text
                    TableCell tdLinkText = new TableCell();
                    tdLinkText.Wrap = false;

                    if (linksCnt != strarr.Length)//All the Middel links are Hyper Links
                    {
                        string isPopUp = "0";
                        if (htcWork.Page.Request.QueryString["PopUp"] != null && htcWork.Page.Request.QueryString["PopUp"] != string.Empty)
                        {
                            isPopUp = "1";
                        }
                        if (isPopUp == "1" && linksCnt == 1)
                        {
                            Label lblCurrent = new Label();
                            lblCurrent.Text = strarr[linksCnt - 1];
                            tdLinkText.Controls.Add(lblCurrent);
                        }
                        else
                        {
                            LAjitLinkButton lnkBtnNav = new LAjitLinkButton();
                            lnkBtnNav.Text = strarr[linksCnt - 1];
                            lnkBtnNav.OnClientClick = "return OnNavLinkClick('" + "Link" + Convert.ToString(linksCnt) + "');";
                            lnkBtnNav.Attributes.Add("oncontextmenu", "return false;");
                            tdLinkText.Controls.Add(lnkBtnNav);
                        }
                    }
                    else//Last link
                    {
                        Label lblCurrent = new Label();
                        lblCurrent.Text = strarr[linksCnt - 1];
                        tdLinkText.Controls.Add(lblCurrent);
                    }

                    tr.Cells.Add(tdLinkText);

                    //TD for the separator
                    TableCell tdLinkSeparator = new TableCell();
                    tdLinkSeparator.Width = Unit.Pixel(5);
                    tdLinkSeparator.Text = ">>>";
                    tr.Cells.Add(tdLinkSeparator);
                }
                //Remove the Link separator.
                if (tr.Cells.Count > 0)
                {
                    tr.Cells.Remove(tr.Cells[tr.Cells.Count - 1]);
                }
                htcWork.Controls.Add(tblNavLinks);//Adding Nav Links table to HtmlTableCell
            }
            else//Parent page
            {
                htcWork.InnerText = gridTitle;
            }
            htcWork.Width = intScrollLength.ToString();
            objGraphics.Dispose();
            objBitmap.Dispose();
        }

        /// <summary>
        /// Gets rid of characters such as hyphens, underscores in a given phone number.
        /// </summary>
        /// <param name="xDocGVData">The form XML document.</param>
        /// <param name="xBranchDoc">The Branch Doc to be sent to DB.</param>
        private static void FormatPhoneTypes(XmlDocument xDocGVData, XmlDocument xBranchDoc)
        {
            //Get all the controls with the ControlType as Phone in order to remove the hyphens.
            XmlNodeList xnlPhoneControls = xDocGVData.SelectNodes("Root/bpeout/FormControls//Col[@ControlType='Phone']");
            foreach (XmlNode nodePhoneCol in xnlPhoneControls)
            {
                string colLabel = nodePhoneCol.Attributes["Label"].Value;
                string parentNode = nodePhoneCol.ParentNode.ParentNode.ParentNode.Name;//Get the owner grid layout node name.
                XmlNode nodeTargetPhones = xBranchDoc.SelectSingleNode("Root/" + parentNode + "/RowList");
                //Remove the hyphens and underscores from the request XML.
                if (nodeTargetPhones != null)
                {
                    foreach (XmlNode nodeTargetPhone in nodeTargetPhones)
                    {
                        XmlAttribute attPhoneNode = nodeTargetPhone.Attributes[colLabel];
                        if (attPhoneNode != null)
                        {
                            attPhoneNode.Value = attPhoneNode.Value.Replace("-", "").Replace("_", "");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the server-side image source of the image with the selected one.
        /// </summary>
        private void UpdateTypeOfJobImage(Panel pnlEntryForm, string strReqXml)
        {
            LAjitControls.LAjitImage imgTypeOfJob = (LAjitControls.LAjitImage)pnlEntryForm.FindControl("imgTypeOfJob");
            if (imgTypeOfJob != null)
            {
                try
                {
                    XmlDocument xBPinfo = new XmlDocument();
                    xBPinfo.LoadXml(strReqXml);
                    XmlNode nodeModRow = xBPinfo.SelectSingleNode("Root/bpinfo/" + BtnsUC.TreeNode + "/RowList").ChildNodes[0];
                    string trxIDTrxType = nodeModRow.Attributes["TypeOfJob_TrxID"].Value + "~" + nodeModRow.Attributes["TypeOfJob_TrxType"].Value;
                    HiddenField hdnTypeOfJob = (HiddenField)pnlEntryForm.NamingContainer.FindControl("hdnFldTypeOfJob");

                    HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
                    string strImagesCDNPath = (String)ctx.Application["ImagesCDNPath"];


                    if (hdnTypeOfJob != null)
                    {
                        string hdnValue = hdnTypeOfJob.Value;
                        int indexOfTrxIDTrxType = hdnValue.IndexOf(trxIDTrxType);
                        if (hdnValue[indexOfTrxIDTrxType - 1] == '-')
                        {
                            indexOfTrxIDTrxType = hdnValue.IndexOf(trxIDTrxType, trxIDTrxType.Length);
                        }
                        int startIndex = indexOfTrxIDTrxType + trxIDTrxType.Length + 1;//Get rid of the hyphen also
                        int endIndex = hdnValue.IndexOf(",", startIndex);
                        string imageName = hdnValue.Substring(startIndex, endIndex - startIndex);
                        imgTypeOfJob.ImageUrl = strImagesCDNPath + "Images/" + imageName;
                        //imgTypeOfJob.Visible = true;
                        imgTypeOfJob.Attributes.Add("style", "DISPLAY: Block;");
                        //imgTypeOfJob.Style.Add("display", "block");
                    }
                }
                catch (Exception ex)
                {

                    #region NLog
                    logger.Fatal(ex);
                    #endregion

                    //Do nothing for now.
                    string errorMessage = HttpContext.Current.User.Identity.Name + Environment.NewLine + ex.Message + Environment.NewLine +
                            ex.StackTrace + Environment.NewLine +
                            pnlEntryForm.Page.ToString();
                    Classes.ErrorLogger.LogError(errorMessage, Classes.LogType.LogFile);
                }
            }
        }

        /// <summary>
        /// Finds the child objects in a page and explicitly enables/disables as required.
        /// </summary>
        /// <param name="pnlEntryForm">Panel container.</param>
        private void ReEnableChildObject(Panel pnlEntryForm, string gridMode)
        {
            //If DB operation has failed then keep the child gridview(if present) enabled.
            Panel pnlGVBranch = (Panel)pnlEntryForm.FindControl("pnlGVBranch");
            if (pnlGVBranch != null)
            {
                Color gridColor = Color.Empty;
                if (gridMode == "FIND")
                {
                    gridColor = Color.LightGoldenrodYellow;
                }
                else
                {
                    gridColor = Color.White;
                }

                foreach (Control ctrl in pnlGVBranch.Controls)
                {
                    if (ctrl is GridView)
                    {
                        GridView grdVwChild = (GridView)ctrl;

                        //Clear the Rows to delete selection in this case.
                        //Let the user reselect the deletions to be made.
                        string branchNodeName = grdVwChild.ID.Replace("grdVw", "");
                        HiddenField hdnRowsToDelete = (HiddenField)pnlEntryForm.FindControl("hdnRowsToDelete" + branchNodeName);
                        hdnRowsToDelete.Value = string.Empty;

                        foreach (GridViewRow gvr in grdVwChild.Rows)
                        {
                            foreach (TableCell tc in gvr.Cells)
                            {
                                tc.Enabled = true;
                                int cellIndex = gvr.Cells.GetCellIndex(tc);
                                if (m_arrIsDisplayOnlyCols.Contains(cellIndex - 1))
                                {
                                    continue;
                                }
                                foreach (Control cellControl in tc.Controls)
                                {
                                    if (cellControl is LAjitDropDownList)
                                    {
                                        ((LAjitDropDownList)cellControl).BackColor = gridColor;
                                    }
                                    else if (cellControl is LAjitTextBox)
                                    {
                                        ((LAjitTextBox)cellControl).BackColor = gridColor;
                                    }
                                }
                            }
                        }
                    }
                }
                Table tblOperations = (Table)pnlEntryForm.FindControl("tblOperationLinks");
                TableRow tr = tblOperations.Rows[0];//Only one biigg row.
                //Enable the operations link buttons explicitly
                int amtLabelsCount = m_arrAmountCols.Count * 2;//Count the number of Labels for Amount Columns
                int cntr = 0;
                for (cntr = tr.Cells.Count - (amtLabelsCount + 1); cntr >= tr.Cells.Count - (9 + amtLabelsCount); cntr--)//Loop only the last 9 cells only
                {
                    tr.Cells[cntr].Enabled = true;
                }
                //Disable the BPC links explicitly
                for (; cntr >= 0; cntr--)//Loop the remaining cells if any.
                {
                    tr.Cells[cntr].Enabled = false;
                }
            }
        }

        /// <summary>This method will Set the hdnBPInfo in case of Modify Load .</summary>
        /// <param name="xDocOut">XmlDocument</param>
        /// <param name="nodeColumns">XmlNode</param>
        /// <returns>nothing</returns>
        public void SetCOXML(XmlDocument xDocOut, XmlNode nodeColumns, string gvXML)
        {
            #region NLog
            logger.Info("This method will Set the hdnBPInfo in case of Modify Load. ");
            #endregion

            //setting Treenode name 
            string m_treeNodeName = BtnsUC.TreeNode;
            StringBuilder sbRowXML = new StringBuilder();
            sbRowXML.Append("<" + m_treeNodeName + "><RowList>" + BtnsUC.RwToBeModified + "</RowList></" + m_treeNodeName + ">");

            XmlDocument xDocRowInfoXml = new XmlDocument();
            xDocRowInfoXml.LoadXml(BtnsUC.RwToBeModified);
            string TrxID = xDocRowInfoXml.FirstChild.Attributes["TrxID"].Value;
            string TrxType = xDocRowInfoXml.FirstChild.Attributes["TrxType"].Value;

            //Retrieving the BPGID and Navigate page from GVDataXML
            string BPGID = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            string pageInfo = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;

            if (pageInfo.Contains("VoidCheckHistory"))
            {
                //Append the Branch node row XML also to the rowXML.
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        string branchName = nodeBranch.FirstChild.InnerText;
                        //Find the child row of the Parent Row in the RowList of the current branch.
                        XmlNode nodeBranchRow = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList/Rows[@" + m_treeNodeName + "_TrxID='" + TrxID + "' and @" + m_treeNodeName + "_TrxType='" + TrxType + "']");
                        if (nodeBranchRow != null)
                        {
                            //If there is a associated branch child row.
                            sbRowXML.Append("<" + branchName + "><RowList>" + nodeBranchRow.OuterXml + "</RowList></" + branchName + ">");
                        }
                    }
                }
            }
            if (pageInfo.Contains("payables/SelectInvoice.aspx"))
            {
                sbRowXML = new StringBuilder();
                //Append the Branch node row XML also to the rowXML.
                XmlDocument xDocGVXML = new XmlDocument();
                XmlDocument xDocLocalCopy = new XmlDocument();
                //bool compare = false;
                //if (gvXML.Length > 0)
                //{
                //    xDocGVXML.LoadXml(gvXML);
                //    xDocLocalCopy.LoadXml(BtnsUC.GVDataXml);
                //    compare = true;
                //}

                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        string branchName = nodeBranch.FirstChild.InnerText;
                        if (branchName != string.Empty)
                        {
                            string childRows = string.Empty;
                            bool selectionExists = false;
                            XmlNode nodeChildRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                            if (nodeChildRows == null)
                            {
                                return;
                            }
                            //if (compare)
                            //{
                            //    XmlNode nodeRowListGrid = xDocGVXML.SelectSingleNode(branchName + "/RowList");
                            //    XmlNode nodeRowListLocal = xDocLocalCopy.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");

                            //    for (int index = 0; index < nodeRowListLocal.ChildNodes.Count; index++)
                            //    {
                            //        XmlNode nodeRowLocal = nodeRowListLocal.ChildNodes[index];
                            //        XmlNode nodeRowGrid = nodeRowListGrid.ChildNodes[index];
                            //        //int diff = nodeRowLocal.OuterXml.CompareTo(nodeRowGrid.OuterXml);
                            //        //if (!CompareNodes(nodeRowGrid, nodeRowLocal))
                            //        //{
                            //        //    //Row Changed so send it
                            //        //}

                            //        if (nodeRowLocal.Attributes["Selectit"].Value !=
                            //                nodeRowGrid.Attributes["Selectit"].Value)
                            //        {
                            //            childRows += nodeRowGrid.OuterXml;
                            //        }
                            //        if (!selectionExists && nodeRowGrid.Attributes["Selectit"].Value == "1")
                            //        {
                            //            selectionExists = true;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            for (int index = 0; index < nodeChildRows.ChildNodes.Count; index++)
                            {
                                XmlNode nodeChildRow = nodeChildRows.ChildNodes[index];
                                if (nodeChildRow.Attributes["Selectit"].Value == "1")
                                {
                                    childRows += nodeChildRow.OuterXml;
                                    selectionExists = true;
                                }
                            }
                            //}
                            if (selectionExists)//childRows != string.Empty)
                            {
                                //If there is a associated branch child row.
                                sbRowXML.Append("<" + branchName + "><RowList>" + childRows + "</RowList></" + branchName + ">");
                                ((HiddenField)BtnsUC.FindControl("hdnSelected")).Value = string.Empty;
                                ((HiddenField)BtnsUC.FindControl("hdnSelected")).Value = "TRUE";
                            }
                            else
                            {
                                ((HiddenField)BtnsUC.FindControl("hdnSelected")).Value = string.Empty;
                                ((HiddenField)BtnsUC.FindControl("hdnSelected")).Value = "FALSE";
                            }
                        }
                    }
                }
            }

            //Assign the first FVL>0 column value to the caption.
            string COCaption = string.Empty;
            string firstFVLColName = string.Empty;
            if (nodeColumns.ChildNodes.Count > 0)
            {
                foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
                {
                    if (nodeCol.Attributes["FullViewLength"] != null)
                    {
                        if (Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value) > 0)
                        {
                            firstFVLColName = nodeCol.Attributes["Label"].Value;
                            break;
                        }
                    }
                }
            }
            if (xDocRowInfoXml.FirstChild.Attributes[firstFVLColName] != null)// && xDocRowInfoXml.SelectSingleNode(m_treeNodeName + "/RowList/Rows").Attributes[firstFVLColName] != "")
            {
                COCaption = xDocRowInfoXml.FirstChild.Attributes[firstFVLColName].Value;
            }

            string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + pageInfo + "</PageInfo><Caption>" + CharactersToHtmlCodes(COCaption) + "</Caption></CallingObject>";
            if (BtnsUC != null && BtnsUC.FindControl("hdnGVBPEINFO") != null)
            {
                ((HiddenField)BtnsUC.FindControl("hdnGVBPEINFO")).Value = sbRowXML.ToString() + callingObjXML;
            }
            if (GVUC != null && GVUC.FindControl("hdnGVBPEINFO") != null)
            {
                ((HiddenField)GVUC.FindControl("hdnGVBPEINFO")).Value = sbRowXML.ToString() + callingObjXML;
            }
        }

        /// <summary>
        /// Checks the availabiltiy of all nodes except BPAction to present in node1 in node2 as well.
        /// Assumes that the given nodes don't have any child nodes, only attributes.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>true if equal, false otherwise.</returns>
        public bool CompareNodes(XmlNode node1, XmlNode node2)
        {
            foreach (XmlAttribute att1 in node1.Attributes)
            {
                if (att1.Name == "BPAction")
                {
                    continue;
                }

                if (node2.Attributes[att1.Name] != null)
                {
                    string compareTo = node2.Attributes[att1.Name].Value;
                    DateTime dt;
                    if (DateTime.TryParse(compareTo, out dt))
                    {
                        compareTo = dt.ToString("MM/dd/yy");
                    }

                    if (att1.Value != compareTo)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                //else if (att1.Value != node2.Attributes[att1.Name].Value)
                //{
                //    return false;
                //}
            }
            return true;
        }

        /// <summary>Validating autofill entry columns in branch nodes</summary>
        /// <param name="xDocForm"></param>
        /// <param name="childObjectXML"></param>
        /// <returns>column captions</returns>
        public string ValidateAutoFillEntry(XmlDocument xDocForm, string childObjectXML)
        {
            #region NLog
            logger.Info("Validating autofill entry columns in branch nodes. ");
            #endregion

            //Get the branch nodes of the above tree.
            XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            //If there are no branches nothing to do just return.
            if (nodeBranches == null)
            {
                return string.Empty;
            }

            string finalMessage = string.Empty;

            foreach (XmlNode nodeBranch in nodeBranches.ChildNodes) //Each branch corresponds to a child object
            {
                string branchname = nodeBranch.InnerText;

                XmlNode branchnodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchname + "/GridHeading/Columns");
                ArrayList alBranchColumns = new ArrayList();
                Hashtable htColumnLabel = new Hashtable();
                foreach (XmlNode nodeCol in branchnodeColumns.ChildNodes)
                {
                    int IsLink = Convert.ToInt32(nodeCol.Attributes["IsLink"].Value);
                    int IsRequired = Convert.ToInt32(nodeCol.Attributes["IsRequired"].Value);
                    int FullViewLength = 0;
                    if (nodeCol.Attributes["FullViewLength"] != null)
                    {
                        FullViewLength = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value);
                    }
                    string ControlType = string.Empty;

                    if (nodeCol.Attributes["ControlType"] != null)
                    {
                        ControlType = nodeCol.Attributes["ControlType"].Value;
                    }
                    if ((FullViewLength != 0) && (IsLink == 1) && (IsRequired == 1) && (ControlType == "TBox"))
                    {
                        alBranchColumns.Add(nodeCol.Attributes["Label"].Value);
                        htColumnLabel.Add(nodeCol.Attributes["Label"].Value, nodeCol.Attributes["Caption"].Value);
                    }
                }


                XmlDocument xDc = new XmlDocument();
                xDc.LoadXml("<Root>" + childObjectXML + "</Root>");
                XmlNode xNodeMod = xDc.SelectSingleNode("Root/" + branchname + "/RowList");
                if ((xNodeMod != null) && (htColumnLabel.Count > 0))
                {
                    foreach (XmlNode noderow in xNodeMod.ChildNodes)
                    {
                        for (int i = 0; i < alBranchColumns.Count; i++)
                        {
                            string attTrxID = alBranchColumns[i].ToString() + "_TrxID";
                            string attTrxType = alBranchColumns[i].ToString() + "_TrxType";

                            if ((noderow.Attributes[attTrxID] == null) && (noderow.Attributes[attTrxType] == null))
                            {
                                if (!finalMessage.Contains(htColumnLabel[alBranchColumns[i]].ToString()))
                                {
                                    if (finalMessage != string.Empty)
                                    {
                                        finalMessage = finalMessage + ", " + htColumnLabel[alBranchColumns[i]].ToString();
                                    }
                                    else
                                    {
                                        finalMessage = htColumnLabel[alBranchColumns[i]].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if ((noderow.Attributes[attTrxID].Value == string.Empty) && ((noderow.Attributes[attTrxType].Value == string.Empty)))
                                {
                                    if (!finalMessage.Contains(htColumnLabel[alBranchColumns[i]].ToString()))
                                    {
                                        if (finalMessage != string.Empty)
                                        {
                                            finalMessage = finalMessage + ", " + htColumnLabel[alBranchColumns[i]].ToString();
                                        }
                                        else
                                        {
                                            finalMessage = htColumnLabel[alBranchColumns[i]].ToString(); ;
                                        }
                                    }
                                }
                            }
                        }
                    }//for 
                }
            }
            return finalMessage;
        }

        public XmlDocument loadXmlFile(string filePath)
        {
            #region NLog
            logger.Info("Loading the BPE XML from physical file : " + filePath);
            #endregion

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
                throw;
            }

            return xmlDoc;
        }
        #endregion

        #region Ajax Methods
        /// <summary>
        /// To Set the Session "LinkBPinfo". This method will be called from javascript...
        /// </summary>
        /// <param name="selectedRow"></param>
        ///[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetLinkPopUpSession(string processBPGID, string COXML)
        {
            if (processBPGID != null && COXML != null)
            {
                System.Web.HttpContext.Current.Session["LinkBPinfo"] = "<bpinfo><BPGID>" + processBPGID + "</BPGID>" + COXML + "</bpinfo>";
            }
        }

        /// <summary>Ajax Method To Get AutoFill Selected Item TrxID and TrxType</summary>
        /// <param name="cacheName"></param>
        /// <param name="selectedValue"></param>
        /// <param name="MapXML"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string GetAutoFillValues(string cacheName, string selectedValue, string MapXML)
        {
            string autoFillTrxID = string.Empty;
            string autoFillTrxType = string.Empty;
            string POFound = string.Empty;
            string finalValues = string.Empty;
            if (System.Web.HttpContext.Current.Cache[cacheName] != null)
            {
                DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];
                if (dsAutoFill.Tables[MapXML].Rows.Count > 0)
                {
                    //Step 1 look Exact match word
                    if (dsAutoFill.Tables[MapXML].Rows.Count > 0)
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(MapXML + "='" + selectedValue.Trim().Replace("'", "''") + "'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][MapXML + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][MapXML + "_TrxType"].ToString();

                        }
                    }

                    // Step 2  Exact match not found take like first record
                    if ((autoFillTrxID == string.Empty) && (autoFillTrxType == string.Empty))
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(MapXML + " like '" + selectedValue.Trim().Replace("'", "''") + "%'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][MapXML + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][MapXML + "_TrxType"].ToString();

                        }
                    }
                }
            }

            if ((autoFillTrxID != string.Empty) && (autoFillTrxType != string.Empty))
            {
                finalValues = MapXML + "_TrxID='" + autoFillTrxID + "';" + MapXML + "_TrxType='" + autoFillTrxType + "'";
            }
            return finalValues;
        }
        #endregion

        #region Return Extension
        public static string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";

                case ".txt":
                    return "text/plain";

                case ".doc":
                    return "application/ms-word";

                case ".tiff":
                case ".tif":
                    return "image/tiff";

                case ".asf":
                    return "video/x-ms-asf";

                case ".avi":
                    return "video/avi";

                case ".zip":
                    return "application/zip";

                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";

                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                case ".docx":
                    return "application/vnd.docx";

                case ".gif":
                    return "image/gif";

                case ".jpg":
                case "jpeg":
                    return "image/jpeg";

                case ".bmp":
                    return "image/bmp";

                case ".wav":
                    return "audio/wav";

                case ".mp3":
                    return "audio/mpeg3";

                case ".mpg":
                case "mpeg":
                    return "video/mpeg";

                case ".rtf":
                    return "application/rtf";

                case ".asp":
                    return "text/asp";

                case ".pdf":
                    return "application/pdf";

                case ".fdf":
                    return "application/vnd.fdf";

                case ".ppt":
                    return "application/mspowerpoint";

                case ".dwg":
                    return "image/vnd.dwg";

                case ".msg":
                    return "application/msoutlook";

                case ".xml":
                case ".sdxl":
                    return "application/xml";

                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";

                default:
                    return "application/octet-stream";
            }
        }
        #endregion

        public static string ParseXpathString(string input)
        {
            string ret = "";

            if (input.Contains("'"))
            {
                string[] inputstrs = input.Split('\'');
                foreach (string inputstr in inputstrs)
                {
                    if (ret != "")
                        ret += ",\"'\",";
                    ret += "\"" + inputstr + "\"";
                }
                ret = "concat(" + ret + ")";
            }
            else
            {
                ret = "'" + input + "'";
            }
            return ret;
        }

        public static string ParseXPath(string value)
        {
            // if the value contains only single or double quotes, construct
            // an XPath literal
            if (!value.Contains("\""))
            {
                return "\"" + value + "\"";
            }
            if (!value.Contains("'"))
            {
                return "'" + value + "'";
            }

            // if the value contains both single and double quotes, construct an
            // expression that concatenates all non-double-quote substrings with
            // the quotes, e.g.:
            //
            //    concat("foo", '"', "bar")
            StringBuilder sb = new StringBuilder();
            sb.Append("concat(");
            string[] substrings = value.Split('\"');
            for (int i = 0; i < substrings.Length; i++)
            {
                bool needComma = (i > 0);
                if (substrings[i] != "")
                {

                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("\"");
                    sb.Append(substrings[i]);
                    sb.Append("\"");
                    needComma = true;
                }
                if (i < substrings.Length - 1)
                {
                    if (needComma)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("'\"'");
                }

            }
            sb.Append(")");
            return sb.ToString();
        }

        public void SaveToClient(FileInfo newFile)
        {
            #region NLog
            logger.Info("Saving to Response, File : " + newFile.FullName);
            #endregion

            HttpContext context = HttpContext.Current;
            // Get the physical Path of the file(test.doc)
            //string filepath = Path.GetFullPath(newFile.FullName);

            // Create New instance of FileInfo class to get the properties of the file being downloaded
            FileInfo file = new FileInfo(newFile.FullName);

            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                //context.Response.ClearContent();

                context.Response.Clear();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                context.Response.AddHeader("Content-Disposition", "attachment; filename= \"" + Path.GetFileName(file.FullName) + "\"");

                // Add the file size into the response header
                context.Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                context.Response.ContentType = CommonUI.ReturnExtension(file.Extension.ToLower());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                context.Response.TransmitFile(file.FullName);

                // End the response
                //context.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}