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
using System.Xml;
using LAjitControls.JQGridView;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Text;
using NLog;


namespace LAjitDev.Common
{
    public partial class GenProcessEngine : BasicPage
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        #region Private Field Variables
        private const int m_NoOfGVStaticColumns = 3;
        private int m_CurrPageNo;
        private int m_MaxPages;
        private int m_IndexOfIsProtected;
        private int m_IndexOfIsActive;
        private int m_IndexOfTypeOfInactiveStatusID;
        private bool m_IsFirstRun = true;
        //The index of the HelpFile Column in the GridView(Applies only to HelpAuthor)
        private int m_IndexOfHelpFile = -1;
        /// <summary>
        /// Describes whether the Find Row is to diplayed as the top row or not.
        /// </summary>
        private string m_BPCXml;
        private string m_BusinessRules = string.Empty;
        private string m_primaryKeyFieldName;
        private string m_HyperLinksEnabled = string.Empty;
        private string m_ReturnXML = string.Empty;
        private string m_GVTreeNodeName = string.Empty;
        //Store the current BPGID of the return XML(For the view details functionality)
        private string m_CurrentBPGID = string.Empty;
        private string m_GridInitData;
        //Store the current Page Info of the return XML(For the view details functionality)
        private string m_CurrentPageInfo = string.Empty;
        private string m_GridTitle = string.Empty;
        private string m_RowHoverColour = string.Empty;
        private System.Text.StringBuilder m_sbToolTipJS = new System.Text.StringBuilder();
        private Hashtable m_htCTypes;
        private Hashtable m_htBPCntrls = new Hashtable();
        private Hashtable m_htColLinkDDL = new Hashtable();
        private Hashtable m_htBPCColumns = new Hashtable();
        /// <summary>
        /// Contains the column names and their corresponding index values in the gridview.
        /// </summary>
        private Hashtable m_htGVColumns = new Hashtable();
        /// <summary>
        /// Contains the column full view and small view lengths.
        /// </summary>
        private Hashtable m_htGVColWidths = new Hashtable();
        /// <summary>
        /// Contains the indices of the image columns in the grid view.
        /// </summary>
        private ArrayList m_arrImageColIndices = new ArrayList();
        /// <summary>
        /// Contains the indices of the Amount or IsNumeric columns in the grid view.
        /// </summary>
        private ArrayList m_arrAmountCols = new ArrayList();
        /// <summary>
        /// Holds the most recently bound grid's column Control Types.Used for Find functinality.
        /// </summary>
        private ArrayList m_arrColControlTypes = new ArrayList();
        private Hashtable m_htSearchCols = new Hashtable();
        private XmlDocument m_XDOCBPE;

        private XmlDocument m_xDoc;
        private string m_bpcXML;

        JQCommonUI JQcommonObjUI = new JQCommonUI();

        private LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        private CommonUI m_ObjCommonUI = new CommonUI();
        protected LAjitDev.UserControls.ButtonsUserControl ucButtonsUserControl;
        XmlDocument XDocUserInfo = new XmlDocument();
        private string strBPE;
        public bool BPCAdd = false;
        public bool BPCModify = false;
        public bool BPCDelete = false;
        public bool BPCFind = false;
        public bool enablePrint = false;
        public bool enableNote = false;
        public bool enableSecurity = false;
        public bool enableAttachment = false;
        public int noteBPGID;
        public int secureBPGID;
        public int attachmentBPGID;
        public bool IsPopUp = false;
        public string m_DefaultPageSize;

        #endregion

        #region Properties
        /// <summary>
        /// Accessor for the Session["LinkBPinfo"]
        /// </summary>
        private string SessionLinkBPInfo
        {
            get { return Convert.ToString(HttpContext.Current.Session["LinkBPinfo"]); }
            set { HttpContext.Current.Session["LinkBPinfo"] = value; }
        }

        /// <summary>
        /// Holds the reference of the CommonUI class instance created in the BasePage.cs.
        /// </summary>
        public CommonUI ObjCommonUI
        {
            get { return m_ObjCommonUI; }
            set { m_ObjCommonUI = value; }
        }
        /// <summary>
        /// Gets the title text displayed in the title bar.(Tree Node Title)
        /// </summary>
        /// <remarks>To be called only after DataBind operation.</remarks>
        public string GridTitle
        {
            get { return m_GridTitle; }
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

        public string GridViewInitData
        {
            get
            {
                return m_GridInitData;
            }
            set { m_GridInitData = value; }
        }

        public string DefaultPageSize
        {
            get { return m_DefaultPageSize; }
            set { m_DefaultPageSize = value; }
        }

        public string GridViewBPInfo
        {
            get
            {
                //return ViewState["GridBPEInfo"].ToString();
                return hdnBPInfo.Value;
            }
            set
            {
                //ViewState["GridBPEInfo"] = value;
                hdnBPInfo.Value = value;
            }
        }

        public GridViewType GridViewType
        {
            set { ViewState["GridViewType"] = value; }
            get { return (GridViewType)ViewState["GridViewType"]; }
        }

        #region Commented Code
        ///// <summary>
        ///// Spicified whether the Quick Search 
        ///// </summary>
        //public bool IsFindEnabled
        //{
        //    get { return (hdnFindEnabled.Value != "1") ? false : true; }
        //    set { hdnFindEnabled.Value = (value) ? "1" : "0"; }
        //}

        //public string ShowViewerInPopUp
        //{
        //    get { return hdnPreviewInPopup.Value; }
        //    set { hdnFindEnabled.Value = value; }
        //}

        ////For storing response xml.
        //public string FormTempData
        //{
        //    get
        //    {
        //        if (ViewState["FormTempData"] == null)
        //        {
        //            ViewState["FormTempData"] = string.Empty;
        //        }
        //        return ViewState["FormTempData"].ToString();
        //    }
        //    set { ViewState["FormTempData"] = value; }
        //}
        #endregion

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {

            AddScriptReferences();
            if (!IsPostBack)
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                LoadBPInfo();


                if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
                {
                    jqGrid.Style.Value = Page.Request.QueryString["Resolution"];
                }

                //To Enable or Disable Access Buttons(Print,Note,Security,Attachment)
                AccessBtnsDisplay(m_xDoc, m_GVTreeNodeName);

                //To Enable or Disable BPControl Buttons (Add,Modify,Delete etc) 
                BPCBtnsDisplay(m_xDoc);

                //LoadDynamicControls();

                FillChartDropdownData();

                //To Load BPGIDs of AcsessButtons (Note,Security and Attachment)
                LoadControlBPGID();

                jqGrid.HandlerUrl = Application["VirtualPath"].ToString() + "HttpHandlers/GenView.ashx";

                //Response.Write(Application["VirtualPath"].ToString() + "/HttpHandlers/GenView.ashx");

                    // "../HttpHandlers/GenView.ashx";

                BindJqGrid(m_xDoc, m_bpcXML, m_GVTreeNodeName, jqGrid);
                //Get the Index of the Parent jqGrid Control
                int indexOfParent = this.Controls.IndexOf(jqGrid);

                ////Branch Grids Initialisation.
                //XmlNode nodeBranches = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                //if (nodeBranches != null)
                //{
                //    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes) //Each branch corresponds to a child object
                //    {
                //        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                //        //Create a new instance of the JQGridView and add it to the form
                //        JQGridView jqGridBranch = new JQGridView();
                //        jqGridBranch.ID = "jqGrid" + branchNodeName;
                //        this.Controls.Add(jqGridBranch);
                //        BindJqGrid(m_xDoc, m_bpcXML, branchNodeName, jqGridBranch);
                //    }
                //}
            }
        }

        private void FillChartDropdownData()
        {
            string m_GVTreeNodeName = string.Empty;


            XmlNode nodeStatus = m_xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeStatus != null && nodeStatus.InnerText == "Error")
            {
                return;
            }

            //Clear dropdownlist
            ddlXAxis.Items.Clear();
            ddlYAxis.Items.Clear();

            //Get the Grid Layout nodes
            m_GVTreeNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            int colCntr = 0;
            XmlNode nodeColumns = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                if (colNode.Attributes["FullViewLength"] != null)
                {
                    int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                    if (colFullViewLength != 0)//All displayable columns processing...
                    {
                        //Add the Column Length
                        XmlAttribute attControlType = colNode.Attributes["ControlType"];
                        if (attControlType != null)
                        {
                            if ((attControlType.Value == "Amount") || (attControlType.Value == "Calc"))
                            {
                                //Y AXIS
                                ddlYAxis.Items.Add(new ListItem(colNode.Attributes["Caption"].Value, colNode.Attributes["Label"].Value));
                            }
                            else
                            {
                                //X AXIS
                                ddlXAxis.Items.Add(new ListItem(colNode.Attributes["Caption"].Value, colNode.Attributes["Label"].Value));
                            }
                        }
                        colCntr++;
                    }
                }
            }
            //If y axis items default add  Count Item
            ddlYAxis.Items.Add(new ListItem("Count", "Count"));
        }

        protected void jqGrid_OnBindComplete(object sender)
        {

        }
        #endregion

        #region Private Methods
         private void AddScriptReferences()
        {
            

            //CDN Added Scripts

            //HtmlGenericControl hgcHeader = (HtmlGenericControl)(this.Master).FindControl("Header");

            //Content hgcHeader = (Content)Page.FindControl("cphPageContents");
            //JQMethods.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "JQMethods.js");
            Page.Controls.Add(hgcScript1);
            

            //JqGridDebug/grid.locale-en.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "JqGridDebug/grid.locale-en.js");
            Page.Controls.Add(hgcScript2);
            //hgcHeader.Controls.Add(hgcScript2);

            //JqGridDebug/grid.loader.js
            HtmlGenericControl hgcScript3 = new HtmlGenericControl();
            hgcScript3.TagName = "script";
            hgcScript3.Attributes.Add("type", "text/javascript");
            hgcScript3.Attributes.Add("language", "javascript");
            hgcScript3.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "JqGridDebug/grid.loader.js");
            Page.Controls.Add(hgcScript3);
            //hgcHeader.Controls.Add(hgcScript3);

            ////jquery-ui-1.7.2.custom.min.js
            //HtmlGenericControl hgcScript4 = new HtmlGenericControl();
            //hgcScript4.TagName = "script";
            //hgcScript4.Attributes.Add("type", "text/javascript");
            //hgcScript4.Attributes.Add("language", "javascript");
            //hgcScript4.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-ui-1.7.2.custom.min.js");
            //Page.Header.Controls.Add(hgcScript4);

            //AjaxFileUpload.js
            HtmlGenericControl hgcScript5 = new HtmlGenericControl();
            hgcScript5.TagName = "script";
            hgcScript5.Attributes.Add("type", "text/javascript");
            hgcScript5.Attributes.Add("language", "javascript");
            hgcScript5.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "AjaxFileUpload.js");
            Page.Controls.Add(hgcScript5);

            //FrameManager.js for this page JqScripts.js loaded instead of parentchild.js.
            HtmlGenericControl hgcScript6 = new HtmlGenericControl();
            hgcScript6.TagName = "script";
            hgcScript6.Attributes.Add("type", "text/javascript");
            hgcScript6.Attributes.Add("language", "javascript");
            hgcScript6.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "FrameManager.js");
            Page.Controls.Add(hgcScript6);

        }




        private void LoadControlBPGID()
        {
            XmlDocument xDocBPE = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));

            XmlNode BPCNodes = xDocBPE.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls");

            foreach (XmlNode BPC in BPCNodes.ChildNodes)
            {
                if (BPC.Attributes["ID"].Value == "Notes")
                {
                    noteBPGID = int.Parse(BPC.Attributes["BPGID"].Value);
                }

                if (BPC.Attributes["ID"].Value == "SecureItems")
                {
                    secureBPGID = int.Parse(BPC.Attributes["BPGID"].Value);
                }

                if (BPC.Attributes["ID"].Value == "Attachments")
                {
                    attachmentBPGID = int.Parse(BPC.Attributes["BPGID"].Value);
                }
            }
        }

        private EditType GetCtypeMapName(string cType)
        {
            if (m_htCTypes == null)
            {
                m_htCTypes.Add("Check", EditType.checkbox);
                m_htCTypes.Add("DDL", EditType.select);
                m_htCTypes.Add("Phone", EditType.text);
                m_htCTypes.Add("Passwd", EditType.password);
                m_htCTypes.Add("Amount", EditType.text);
                m_htCTypes.Add("Calc", EditType.text);
                m_htCTypes.Add("Cal", EditType.text);
                m_htCTypes.Add("TBox", EditType.text);
                m_htCTypes.Add("Img", EditType.image);
            }
            return (EditType)m_htCTypes[cType];
        }

        private void LoadBPInfo()
        {
            #region NLog
            logger.Info(" Loading user BPInfo"); 
            #endregion

            //if (Session["BPE"] == null)
            //{
            //    XmlDocument XDocUserInfo = (new CommonUI()).loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            //    string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            //    Session["BPE"] = strBPE;
            //}

            string bpInfo = "";
            if (!string.IsNullOrEmpty(BPINFO))
            {
                bpInfo = BPINFO;
            }
            else
            {
                return;
            }

            LAjit_BO.CommonBO objBO = new LAjit_BO.CommonBO();
            string returnXML = objBO.GetLoginInfo("<Root>" + strBPE + bpInfo + "</Root>");
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);
           


            XmlNode nodeStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeStatus != null && nodeStatus.InnerText == "Error")
            {
                return;
            }

            //Initialising the variables m_CurrentBPGID and m_CurrentPageInfo for the view details functionality
            string currentBPGID = xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            string currentPageInfo = xDoc.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
            string GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            string bpcXML = string.Empty;
            if (nodeBPC != null)
            {
                bpcXML = nodeBPC.OuterXml;
            }
            //DefaultGridSize
            DefaultPageSize = ObjCommonUI.GetPreferenceValue("59");
            string defaultPageSize = DefaultPageSize;
            XmlDocument xDocBPINFO = new XmlDocument();
            xDocBPINFO.LoadXml(bpInfo);
            if (xDocBPINFO.SelectSingleNode("bpinfo/" + GVTreeNodeName) == null)
            {
                xDocBPINFO.FirstChild.InnerXml += "<" + GVTreeNodeName + "><Gridview><Pagenumber>1</Pagenumber><Pagesize>"
                        + DefaultPageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder>ASC</Sortorder></Gridview></" + GVTreeNodeName + ">";
            }

            ////Update the BPINFO
            //Session["BPINFO"] = xDocBPINFO.OuterXml;

            GridViewBPInfo = xDocBPINFO.OuterXml;

            //Grid Title
            jqGrid.Caption = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/GridHeading/Title").InnerText;

            //Rename the close hyperlink in the Popup frame according to the header text.
            string changeCloseJS = "ChangeCloseLinkText('" + jqGrid.Caption + "');";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ChangeCloseText", changeCloseJS, true);
            ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);

            XmlNode nodePageSubject = xDoc.SelectSingleNode("Root/bpeout/FormControls/PageSubject");
            if (nodePageSubject != null)
            {
                string pageSubject = commonObjUI.HtmlCodesToCharacters(nodePageSubject.InnerText);
                jqGrid.Caption = pageSubject + " >>> " + jqGrid.Caption;
            }
            this.Title = jqGrid.Caption;

            //In Find if search returns no results..
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");

            if (nodeMsgStatus.InnerText != "Success")
            {
                return;
            }

            ////***********Temp Code
            ////xDoc.Load(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\ProcessEngine2x.xml");

            //XmlNode BPCNodes = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            //if (BPCNodes == null)
            //{
            //    BPCNodes = xDoc.CreateNode(XmlNodeType.Element, "BusinessProcessControls", "");
            //    BPCNodes.InnerXml = "<BusinessProcess FormID=\"297\" ID=\"Process1\" BPGID=\"527\" Label=\"Print Report\" PageInfo=\"Popups/ShowPDF.aspx\" IsPopup=\"1\" MsgPrompt=\" \"/>";
            //    XmlNode nodeFormControls = xDoc.SelectSingleNode("Root/bpeout/FormControls");
            //    nodeFormControls.AppendChild(BPCNodes);
            //}
            ////***********Temp Code

            m_xDoc = xDoc;
            m_bpcXML = bpcXML;
            m_GVTreeNodeName = GVTreeNodeName;
        }

        private void AccessBtnsDisplay(XmlDocument xDoc, string GVTreeNodeName)
        {
            string strPrint = string.Empty;

            XmlAttribute attOkToPrint = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPrint"];
            if (attOkToPrint != null)
            {
                strPrint = attOkToPrint.Value;
            }

            XmlNode nodeExtendedColoumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/GridHeading/GridExtendedColumns");

            string strSecured = nodeExtendedColoumns.SelectSingleNode("IsSecured").InnerText;
            string strNote = nodeExtendedColoumns.SelectSingleNode("IsNoted").InnerText;
            string strAttached = nodeExtendedColoumns.SelectSingleNode("IsAttached").InnerText;

            if (strPrint != null || strPrint != "")
            {
                if (strPrint == "1")
                    enablePrint = true;
            }

            if (strNote != null || strNote != "")
            {
                if (strNote == "1")
                    enableNote = true;
            }

            if (strSecured != null || strSecured != "")
            {
                if (strSecured == "1")
                    enableSecurity = true;
            }

            if (strAttached != null || strAttached != "")
            {
                if (strAttached == "1")
                    enableAttachment = true;
            }
        }

        private void LoadDynamicControls()
        {
            #region NLog
            logger.Info("Loading the dynamic controls in the page");
            #endregion

            ContentPlaceHolder cphPageContents = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
            Panel pnlBtns = (Panel)cphPageContents.FindControl("pnlBtns");

            string pnlContent = null;

            pnlContent = ("<table border='0' cellpadding='0' cellspacing='0' width='40px'><tr style='height: 50px;' valign='top'><td><input type='image' name='Add' title='Add' id='btnAdd' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/add-icon.png' onclick='return false;' /></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Clone' title='Clone' id='btnAddClone'  src='" + Application["ImagesCDNPath"].ToString() + "Images/add-clone.png' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add-clone.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add-clone-over.png' onclick='return false;' /></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='AddDetails' title='Add Details' id='btnAddDetails' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "'Images/add-icon.png' onmouseover='this.src=" + Application["ImagesCDNPath"].ToString() + "Images/add-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/add-icon.png' onclick='return false;' /></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Edit' title='Modify' id='btnEdit' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/modify-icon.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Delete' title='Delete' id='btnDelete' onmouseout='this.src=" + Application["ImagesCDNPath"].ToString() + "Images/delete_icon.png' onmouseover='this.src=" + Application["ImagesCDNPath"].ToString() + "Images/delete-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_icon.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Search' title='Find' id='btnSearch' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/find-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/find-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/find-icon.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Print' title='Print' id='btnPrint' src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon.png' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon-over.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Note' title='Note' id='btnNote' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Secure' title='Secure' id='btnSecure'  onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "'Images/security-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon.png' onclick='return false;'/></td></tr><tr style='height: 50px;' valign='top'><td><input type='image' name='Attachment' title='Attachment' id='btnAttachment' onmouseout='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png' onmouseover='this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-over.png' src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png' onclick='return false;'/></td></tr><table>");
    
            pnlBtns.Controls.Add(new LiteralControl(pnlContent));
        }

        private void BPCBtnsDisplay(XmlDocument xDoc)
        {
            XmlNode BPCNodes = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (BPCNodes != null)
            {
                foreach (XmlNode BPC in BPCNodes.ChildNodes)
                {
                    if (BPC.Attributes["ID"].Value == "Add")
                    {
                        BPCAdd = true;
                    }

                    if (BPC.Attributes["ID"].Value == "Modify")
                    {
                        BPCModify = true;
                    }

                    if (BPC.Attributes["ID"].Value == "Delete")
                    {
                        BPCDelete = true;
                    }

                    if (BPC.Attributes["ID"].Value == "Find")
                    {
                        BPCFind = true;
                    }
                }
            }
        }

        private void BindJqGrid(XmlDocument xDoc, string bpcXML, string nodeName, JQGridView jqGridObj)
        {
            #region NLog
            logger.Info("Binding the XMl data to the JQ Grid");
            #endregion

            //Set the Width of the Grid based on the Expanded/Collapsed state of the Left Panel
            int gridWidth = 768;
            if (base.IsLeftPanelCollapsed)
            {
                gridWidth += 149;
            }
            jqGrid.Width = Unit.Pixel(gridWidth);


            int defaultGridPageSize = Convert.ToInt32(DefaultPageSize); ;
            int colCntr = 0;
            //To add the Select Coloumn
            ColModel cmSelectImg = new ColModel();
            cmSelectImg.ColumnName = "Select";
            cmSelectImg.Name = "Select";
            cmSelectImg.Index = "";
            cmSelectImg.XmlMap = "";
            cmSelectImg.Width = 10;
            cmSelectImg.Hidden = false;
            cmSelectImg.Editable = false;
            cmSelectImg.EditRules.EditHidden = true;
            cmSelectImg.Sortable = false;
            cmSelectImg.Search = false;
            jqGridObj.Columns.Add(cmSelectImg);

            //To create a Coloumn field for SOXApproval Image
            ColModel cmFd = new ColModel();
            cmFd.ColumnName = "";
            cmFd.Name = "SOXAppStatus";
            cmFd.Index = "";
            cmFd.XmlMap = "";
            cmFd.Width = 0;
            cmFd.Hidden = true;
            cmFd.EditType = EditType.image;
            cmFd.Editable = true;
            cmFd.EditRules.EditHidden = true;

            ImageEditOptions ieoImage = new ImageEditOptions();
            ieoImage.Source = Application["ImagesCDNPath"].ToString() + "images/spacer.gif";
            //ieoImage.onClick = "";
            cmFd.EditOptions = ieoImage;
            jqGridObj.Columns.Add(cmFd);

            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + nodeName + "/GridHeading/Columns");

            //Creating the Columns.
            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                //ControlType Distiguishment.
                string currentControlType = string.Empty;

                XmlAttribute attrCntrlType = colNode.Attributes["ControlType"];
                if (attrCntrlType != null)
                {
                    currentControlType = attrCntrlType.Value;
                }

                XmlAttribute attrDefault = colNode.Attributes["Default"];

                if (currentControlType != "" && currentControlType != null)
                {
                    if (colNode.Attributes["FullViewLength"] == null)
                    {
                        continue;
                    }
                    //Add the column only if FullViewLength is not equal to zero
                    int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                    string label = colNode.Attributes["Label"].Value;
                    bool isLink = false;
                    XmlAttribute attIsLink = colNode.Attributes["IsLink"];
                    if (attIsLink != null && attIsLink.Value == "1" && currentControlType == "TBox")
                    {
                        isLink = true;
                    }

                    ColModel cmField = new ColModel();

                    cmField.Name = label;
                    cmField.Index = label;
                    cmField.XmlMap = label;

                    if (colFullViewLength != 0)
                        cmField.Width = colFullViewLength;

                    cmField.Editable = true;
                    cmField.ColumnName = colNode.Attributes["Caption"].Value;

                    TextBoxEditOptions opt = new TextBoxEditOptions();
                    opt.EditType = EditType.text;
                    opt.Size = 40;

                    SearchOptions sOptions = new SearchOptions();

                    switch (currentControlType)
                    {
                        case "Calc":
                        //To display Search Options for Control Type 'Calculator'
                        //sOptions.Sopt = new string[] { "eq", "ne", "lt", "le", "gt", "ge" };
                        ////sOptions.DataInit = "function (elem){jQuery(elem).calculator();}";
                        //cmField.SearchOptions = sOptions;



                        //cmField.Formatter = FormatterType.number;
                        //NumberFormatter formatter = new NumberFormatter();
                        //formatter.DecimalPlaces = 2;
                        //formatter.ThousandsSeparator = ",";
                        //cmField.FormatOptions = formatter;
                        case "Amount":
                            {
                                if (currentControlType == "Amount")
                                {
                                    //To Set Default Value
                                    if (attrDefault != null)
                                    {
                                        decimal amount;
                                        if (Decimal.TryParse(attrDefault.Value, out amount))
                                        {
                                            cmField.EditOptions = opt;
                                            cmField.EditOptions.DefaultValue = string.Format("{0:N}", amount);
                                        }
                                    }
                                }
                                else
                                {
                                    if (colNode.Attributes["IsDisplayOnly"].Value != "1")
                                    {
                                        opt.DataInit = "InitShowCalculator";//Js Function found in Common.js
                                    }
                                }
                                //To display Search Options for Control Type 'Amount' (&Calculator)
                                sOptions.Sopt = new string[] { "eq", "ne", "lt", "le", "gt", "ge" };
                                cmField.SearchOptions = sOptions;

                                cmField.Formatter = FormatterType.number;
                                NumberFormatter formatter = new NumberFormatter();
                                formatter.DecimalPlaces = 2;
                                formatter.ThousandsSeparator = ",";
                                cmField.FormatOptions = formatter;

                                goto case "TBox";
                            }
                        case "Cal":
                            {
                                if (attrDefault != null)
                                {
                                    //setting default
                                    DateTime date;
                                    DateTime.TryParse(attrDefault.Value, out date);
                                    opt.DefaultValue = date.ToString("MM/dd/yy");
                                }

                                //To display Search Options for Control Type 'Calender'
                                sOptions.Sopt = new string[] { "eq", "ne", "lt", "le", "gt", "ge" };
                                sOptions.DataInit = "function (elem){jQuery(elem).datepicker({dateFormat:'mm/dd/y', changeMonth: true, changeYear: true});jQuery(elem).mask('99/99/99');}";
                                cmField.SearchOptions = sOptions;

                                //opt.DataInit = "function (elem){jQuery(elem).datepicker({dateFormat:'mm/dd/y', changeMonth: true, changeYear: true});jQuery(elem).mask('99/99/99');}";

                                if (colNode.Attributes["IsDisplayOnly"].Value != "1")
                                {
                                    opt.DataInit = "ShowCalender";//Js Function found in Common.js
                                }

                                cmField.Formatter = FormatterType.date;
                                DateFormatter formatter = new DateFormatter();
                                formatter.SourceFormat = "ISO8601Long";
                                formatter.NewFormat = "m/d/y";
                                cmField.FormatOptions = formatter;

                                goto case "TBox";
                            }
                        case "Passwd":
                        case "Phone":
                            {
                                //To display Search Options for Control Type 'Phone'
                                sOptions.Sopt = new string[] { "eq", "bw", "cn" };
                                cmField.SearchOptions = sOptions;
                                goto case "TBox";
                            }
                        case "DDLEX":
                            {
                                //Inherits all the characteristics of the TextBox and an additional AutoFill Extender is attached.
                                //DDLEX-Textbox with an AutoFill extender and submits whatever is filled in the TB upon 
                                //form submit.
                                opt.DataInit = "function(el){AttachAutoFillExt(el,true);}";
                                goto case "TBox";
                            }
                        case "TBox":
                            {
                                cmField.EditType = EditType.text;

                                if (attrDefault != null)
                                {
                                    if (currentControlType != "Cal" && currentControlType != "Calc" && currentControlType != "Amount")
                                    {
                                        cmField.EditOptions = opt;
                                        cmField.EditOptions.DefaultValue = attrDefault.Value;
                                    }
                                    //Set JobCOA default value to hidden field
                                    if (label == "JobCOA")
                                    {
                                        hdnJobCOADefault.Value = attrDefault.Value;
                                    }
                                }

                                //To display Search Options for Control Type 'TextBox'
                                if (currentControlType == "TBox")
                                {
                                    if (colNode.Attributes["IsNumeric"].Value != "1")
                                    {
                                        //sOptions.sopt = new string[] {eq','ne','lt','le','gt','ge','bw','bn','in','ni','ew','en','cn','nc'};
                                        sOptions.Sopt = new string[] { "eq", "ne", "bw", "bn", "in", "ni", "ew", "en", "cn", "nc" };
                                        cmField.SearchOptions = sOptions;
                                    }
                                }

                                //Check for IsLink to attach AutoFill.
                                if (isLink && currentControlType != "DDLEX")
                                {
                                    opt.DataInit = "AttachAutoFill";
                                }


                                cmField.EditOptions = opt;

                                #region Commented Code
                                //if (attrDefault != null)
                                //{
                                //    if (currentControlType == "Cal")
                                //    {
                                //        //setting default
                                //        DateTime date;
                                //        DateTime.TryParse(attrDefault.Value, out date);
                                //        cmField.EditOptions.DefaultValue = date.ToString("MM/dd/yy");
                                //    }
                                //    else if (currentControlType == "Amount" || currentControlType == "Calc")
                                //    {

                                //        decimal amount;
                                //        if (Decimal.TryParse(attrDefault.Value, out amount))
                                //        {
                                //            cmField.EditOptions.DefaultValue = string.Format("{0:N}", amount);
                                //        }

                                //        //To Display Calculator on CalcImage Click
                                //        //cmField.FormOptions.ElementSuffix = @"<input type=""image"" id=""imgCalc"" onclick=""javascript:if(InitShowCalculator(\'" + label + @"\')==false){return false;}"" src=""../App_Themes/Lajit/images/calendar-icon.gif""/>";

                                //    }
                                //    else
                                //    {
                                //        cmField.EditOptions.DefaultValue = attrDefault.Value;
                                //    }
                                //}
                                #endregion
                            }
                            break;
                        case "File":
                            {
                                cmField.EditType = EditType.file;
                                string js = "function InitAjaxUpload(){var UploadData;$.ajaxFileUpload({url:'" + Application["VirtualPath"].ToString() + "/HttpHandlers/GenView.ashx', secureuri:false, fileElementId:'" + label + "', dataType: 'xml', success: function (data, status){FileInfo = data;if(typeof(data.error) != 'undefined'){if(data.error != ''){alert(data.error);}else{alert(data.msg);}}},error: function (data, status, e){alert(e);}}) return false;}";
                                //string js = "function InitAjaxUpload(){$.ajaxFileUpload({url:'FileHandler.aspx',secureuri:false,fileElementId:'"+ label +"',dataType: 'json',success: function (data, status){if(typeof(data.error) != 'undefined'){if(data.error != ''){alert(data.error);}else {alert(data.msg);}}},error: function (data, status, e){alert(e);}})return false;}";
                                //string js = "function InitAjaxUpload(){$.ajaxFileUpload({url:'/HttpHandlers/GenView.ashx',secureuri:false,fileElementId:'fileToUpload',dataType: 'json',success: function (data, status){if(typeof(data.error) != 'undefined'){if(data.error != ''){alert(data.error);}else{alert(data.msg);}}},error: function (data, status, e){alert(e);}})return false;}";
                                ClientScript.RegisterStartupScript(this.Page.GetType(), "AjaxUploadScript", js, true);

                                //sOptions.Sopt = new string[] { "eq", "ne" };
                                cmField.SearchOptions = sOptions;
                                cmField.EditOptions = opt;
                            }
                            break;
                        case "DDL":
                            {
                                cmField.EditType = EditType.select;
                                XmlNode nodeDDLRows = xDoc.SelectSingleNode("//" + label);
                                SelectEditOptions SEopt = new SelectEditOptions(nodeDDLRows, "", "");
                                cmField.EditOptions = SEopt;
                                cmField.SearchType = "select";

                                //To display Search Options for Control Type 'DropDown'
                                //SearchOptions sOptions = new SearchOptions();
                                sOptions.Sopt = new string[] { "eq", "ne" };
                                cmField.SearchOptions = sOptions;

                                //setting default values
                                if (attrDefault != null)
                                {
                                    XmlNode nodeDefVal = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + label + "/RowList/Rows[@TrxID='" + attrDefault.Value + "']");
                                    if (nodeDefVal != null)
                                    {
                                        XmlAttribute attrDefVal = nodeDefVal.Attributes[label];
                                        if (attrDefVal != null)
                                        {
                                            cmField.EditOptions.DefaultValue = attrDefVal.Value;
                                        }
                                    }
                                }
                            }
                            break;
                        case "Check":
                            {
                                cmField.EditType = EditType.checkbox;
                                cmField.Formatter = FormatterType.checkbox;
                                CheckBoxEditOptions CBEopt = new CheckBoxEditOptions();
                                CBEopt.Value = "1:0";
                                cmField.EditOptions = CBEopt;

                                sOptions.Sopt = new string[] { "eq", "ne" };
                                cmField.SearchOptions = sOptions;

                                if (attrDefault != null)
                                {
                                    cmField.EditOptions.DefaultValue = attrDefault.Value;
                                }
                            }
                            break;
                        case "Img":
                            {
                                cmField.EditType = EditType.image;
                                cmField.Editable = false;
                            }
                            break;
                        case "Lbl"://need to check the control type from xml
                            {

                            }
                            break;
                        case "lnkbtn"://need to check the control type from xml
                            {

                            }
                            break;
                        case "LBox":
                            {

                            }
                            break;
                        default:
                            break;
                    }

                    #region Commented Code
                    ////grdVwContents.Columns.Add(cmField);
                    //cmField.Formatter = FormatterType.integer;

                    //IntegerFormatter objIntFrmt = new IntegerFormatter();
                    //objIntFrmt.DefaultValue = "2";
                    //objIntFrmt.ThousandsSeparator = ",";
                    //cmField.FormatOptions = objIntFrmt;
                    //cmField.EditRules.EditHidden = true;
                    //cmField.EditRules.Email = true;
                    //cmField.EditRules.MaxValue = 100;
                    //cmField.EditRules.Number = true;
                    #endregion

                    if (colFullViewLength == 0)
                    {
                        cmField.Hidden = true;
                        cmField.EditRules.EditHidden = true;
                        cmField.Editable = true;
                    }

                    //Set the various properties as dictated by the XML.

                    //Set the sort expression as inidicated in the xml.
                    if (colNode.Attributes["IsSortable"].Value != "1")
                    {
                        cmField.Sortable = false;
                    }
                    else
                    {
                        cmField.ColumnName = "<span class='sortColHdr'>" + cmField.ColumnName + "</span>";
                    }

                    if (colNode.Attributes["IsHidden"].Value == "1")
                    {
                        cmField.Editable = false;
                    }

                    string formBPGID = xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
                    string formPageInfo = xDoc.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
                    hdnFormInfo.Value = formBPGID + "~::~" + formPageInfo;

                    XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                    if (attBPControl != null && attBPControl.Value.Trim().Length > 0)
                    {
                        XmlNode nodeBPCProc = GetBPCNode(attBPControl.Value, bpcXML);
                        if (nodeBPCProc != null)
                        {
                            string processBPGID = nodeBPCProc.Attributes["BPGID"].Value;
                            string pageInfo = nodeBPCProc.Attributes["PageInfo"].Value;
                            string isPopUp = nodeBPCProc.Attributes["IsPopup"].Value;
                            string processName = nodeBPCProc.Attributes["ID"].Value;

                            cmField.Formatter = FormatterType.showlink;
                            ShowLinkFormatter frmtOpts = new ShowLinkFormatter();
                            frmtOpts.BaseLinkUrl = string.Format("javascript:OnJQColLinkClick('{0}','{1}','{2}','{3}')"
                                                    , processName, processBPGID, pageInfo, isPopUp).Replace("'", "\\'");
                            cmField.FormatOptions = frmtOpts;
                        }
                    }

                    if (colNode.Attributes["IsRequired"].Value == "1")
                    {
                        if (currentControlType != "DDL")
                        {
                            cmField.EditRules.Required = true;

                            if (currentControlType == "File")
                            {
                                cmField.EditRules.Custom = true;
                            }
                        }
                        else
                        {
                            cmField.EditRules.Custom = true;
                            cmField.EditRules.CustomFunction = "ValidateDropDown";
                        }

                        cmField.FormOptions.ElementPrefix = "<span class=\"ReqFldVisible\">*</span><b></b>";
                    }
                    else
                    {
                        cmField.FormOptions.ElementPrefix = "<span class=\"ReqFldInVisible\">*</span>";
                    }

                    if (colNode.Attributes["IsDisplayOnly"].Value == "1")
                    {
                        if (cmField.EditOptions != null)
                        {
                            //cmField.EditOptions.ReadOnly = true;
                            cmField.EditOptions.Disable = "disabled";

                        }
                        //cmField.EditOptions.Disable = "disabled";
                    }

                    if (currentControlType != "DDL" && currentControlType != "DDLEX" && !isLink)
                    {
                        if (colNode.Attributes["IsNumeric"].Value == "1")
                        {
                            cmField.EditRules.Number = true;
                        }
                    }

                    if (colNode.Attributes["IsSearched"].Value == "1")
                    {
                        cmField.Search = true;
                    }
                    else
                    {
                        cmField.Search = false;
                    }


                    colCntr++;

                    //if (colCntr == 0)
                    //    jqGridObj.Columns.Add(cmFd);
                    jqGridObj.Columns.Add(cmField);
                }
            }

            ////showing the action buttons depending on BPC in xml.
            //UIVisibility(xDoc, nodeName);
            //can be used in JQMethods.js. Contains FormLevelLinks info as a string.
            hdnFormLvlLinks.Value = JQcommonObjUI.GetBusinessProcessLinksTable(xDoc);

            jqGridObj.SortOrder = SortDirection.Ascending;
            jqGridObj.RowDisplayCount = Convert.ToInt32(DefaultPageSize);
            jqGridObj.DataBind();

            #region Commented Code
            //int defaultGridPageSize = 0;
            //if (DefaultPageSize == "")//If the variable was not set externly.
            //{
            //    defaultGridPageSize = Convert.ToInt32(GetUserPreferenceValue("DefaultGridSize"));
            //}
            //else
            //{
            //    defaultGridPageSize = Convert.ToInt32(DefaultPageSize);
            //}

            ////Calculating the page size of the grid view
            //if (hdnCurrPageNo.Value.Length == 0)
            //{
            //    hdnCurrPageNo.Value = "1";
            //}
            #endregion

            //Setting a global variable for the TotalPageSize i.e total no of records.
            int totalPageSize = Convert.ToInt32(xDoc.
                SelectSingleNode("Root/bpeout/FormControls/" + nodeName + "/Gridresults/Totalpage").Attributes["Pagesize"].Value);
            int currentPageSize = Convert.ToInt32(xDoc.
                SelectSingleNode("Root/bpeout/FormControls/" + nodeName + "/Gridresults/Currentpage").Attributes["Pagesize"].Value);

            #region Commented Code
            //if (currentPageSize != 0)
            //{
            //    m_MaxPages = Convert.ToInt32(System.Math.Ceiling((double)(totalPageSize / (double)defaultGridPageSize)));
            //}

            //hdnMaxPages.Value = m_MaxPages.ToString();
            //hdnGVTreeNodeName.Value = m_GVTreeNodeName;

            //Setting the Alternating Row Style of the GridView
            //string isAlternating = GetUserPreferenceValue("FullViewAlternatingStyle");
            //if (isAlternating != "1")
            //{
            //    //grdVwContents.AlternatingRowStyle.Reset();
            //    //ViewState["ALTRowStyle"] = string.Empty;
            //}
            #endregion

            //Handling the display of the pager template based on the "IsOkToPage" and the number of pages.
            string isPagingEnabled = "";
            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPage"] != null)
            {
                isPagingEnabled = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPage"].Value;
                if (isPagingEnabled == "0" || m_MaxPages == 1)
                {
                    //pnlPagingCtrls.Visible = false;
                }
                else
                {
                    //pnlPagingCtrls.Visible = true;
                }
            }

            GetSoxApprovalStatus(xDoc);

            //Check for correct format of BPInfo.If GridView node is wrapped by the Tree Node
            if (GridViewBPInfo.Length > 0)
            {
                XmlDocument xDocBPEInfo = new XmlDocument();
                xDocBPEInfo.LoadXml(GridViewBPInfo);
                XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("//Gridview");
                if (nodeGridView == null)
                {
                    nodeGridView = xDocBPEInfo.CreateNode(XmlNodeType.Element, "Gridview", "");
                    nodeGridView.InnerXml = CommonUI.GetGridViewNodeXML("1", "", "ASC", defaultGridPageSize.ToString());
                    xDocBPEInfo.DocumentElement.AppendChild(nodeGridView);
                }

                if (nodeGridView.ParentNode.LocalName != nodeName)
                {
                    //Then embed the GridView node into a New Node named as the Tree Node.
                    XmlNode xNodeTreeNode = xDocBPEInfo.SelectSingleNode("bpinfo/" + nodeName);
                    if (xNodeTreeNode == null)
                    {
                        xNodeTreeNode = xDocBPEInfo.CreateElement(nodeName);
                    }
                    //Remove the GridView from the current parent(bpinfo) and append it as child to the above node.
                    nodeGridView.ParentNode.RemoveChild(nodeGridView);
                    xNodeTreeNode.AppendChild(nodeGridView);
                    xDocBPEInfo.DocumentElement.AppendChild(xNodeTreeNode);
                    GridViewBPInfo = xDocBPEInfo.OuterXml;
                    //HttpContext.Current.Session["BPINFO"] = xDocBPEInfo.OuterXml;
                    //SessionLinkBPInfo = xDocBPEInfo.OuterXml;
                }
                XmlNode nodePgNo = nodeGridView.SelectSingleNode("Pagenumber");
                if (nodePgNo != null)
                {
                    // hdnCurrPageNo.Value = nodePgNo.InnerText;
                }
            }

            SessionLinkBPInfo = GridViewBPInfo;

            #region Commented Code
            ////DisplayResults()
            //int colCntr = 0;
            //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/GridHeading/Columns");
            //foreach (XmlNode colNode in nodeColumns.ChildNodes)
            //{
            //    string label = colNode.Attributes["Label"].Value;
            //    //Adding the current column node the ROWS dataset if not present.
            //    if (!dsRowList.Tables[0].Columns.Contains(label))
            //    {
            //        DataColumn dcNew = new DataColumn(label, typeof(string));
            //        dcNew.AllowDBNull = true;
            //        dsRowList.Tables[0].Columns.Add(dcNew);
            //    }

            //    int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
            //    if (colFullViewLength != 0)//All displayable columns processing...
            //    {
            //        //Check for HelpAuthor
            //        if (ObjCommonUI.IsHelpAuthPage)
            //        {
            //            if (label == "HelpFile")
            //            {
            //                m_IndexOfHelpFile = colCntr;
            //            }
            //        }

            //        //For tooltip functionality.
            //        m_htGVColumns.Add(label, colCntr);
            //        //Add the Column Length
            //        m_htGVColWidths.Add(colCntr, colFullViewLength);

            //        XmlAttribute attIsSearchable = colNode.Attributes["IsSearched"];
            //        if (attIsSearchable != null && attIsSearchable.Value == "1")
            //        {
            //            m_htSearchCols.Add(colCntr, "");
            //            //SearchColIndices.Add(colCntr);
            //        }

            //        XmlAttribute attBPControl = colNode.Attributes["BPControl"];
            //        if (attBPControl != null && attBPControl.Value.Trim().Length > 0)
            //        {
            //            m_htBPCntrls.Add(attBPControl.Value.Trim(), GetColumnIndex(label, dsRowList.Tables[0]));
            //            m_htBPCColumns.Add(attBPControl.Value.Trim(), colCntr);
            //        }

            //        XmlAttribute attControlType = colNode.Attributes["ControlType"];
            //        if (attControlType != null)
            //        {
            //            m_arrColControlTypes.Add(attControlType.Value);
            //            //Check for ControlType to be Amount
            //            if ((attControlType.Value == "Amount") || (attControlType.Value == "Calc"))
            //            {
            //                m_arrAmountCols.Add(label);
            //            }

            //            //Check for Control Type to be DateTime
            //            if (attControlType.Value == "Cal")
            //            {
            //                foreach (DataRow dr in dsRowList.Tables[0].Rows)
            //                {
            //                    DateTime dateTime;
            //                    if (DateTime.TryParse(dr[label.Trim()].ToString(), out dateTime))
            //                    {
            //                        dr[label.Trim()] = dateTime.ToString("MM/dd/yyyy");
            //                    }
            //                }
            //            }

            //            if (attControlType.Value == "Img")
            //            {
            //                m_arrImageColIndices.Add(colCntr);
            //                //Adding images dynamically to the grid requires a viewstate to be maintained and data 
            //                //from this view state needs to be rendered on every postback
            //                if (hdnMainViewState.Value != "1")
            //                {
            //                    hdnMainViewState.Value = "1";
            //                    //Check for the existence of BtnsUC in the Page.If not present create your own ViewState.
            //                    Control ucBtnsControl = this.Parent.NamingContainer.FindControl("BtnsUC");
            //                    if (ucBtnsControl == null)
            //                    {
            //                        ViewState["GridViewInitData"] = returnXML;
            //                    }
            //                }
            //            }
            //        }
            //        colCntr++;
            //        dsRowList.AcceptChanges();
            //    }
            //}

            //this.ViewState["SearchCols"] = m_htSearchCols;

            ////Format the Amount columns specified by the m_arrAmountCols object in the data source
            //foreach (string colName in m_arrAmountCols)
            //{
            //    int colIndex = dsRowList.Tables[0].Columns[colName].Ordinal;
            //    foreach (DataRow dr in dsRowList.Tables[0].Rows)
            //    {
            //        decimal amount;
            //        if (Decimal.TryParse(dr[colIndex].ToString(), out amount))
            //        {
            //            dr[colIndex] = string.Format("{0:N}", amount);
            //        }
            //    }
            //}

            //m_GridTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Title").InnerText;
            ////Setting the title of the grid view container panel.
            //SetPanelHeading(htcGridTitle, GridTitle);

            ////Initialising variables to be used in OnRowDataBound
            ////Get the BPC node from the BtnsUC.GVDataXML as the node might go missing in Find case.
            //if (BtnsUserCtrl == null)
            //{
            //    BtnsUserCtrl = (UserControls.ButtonsUserControl)this.NamingContainer.FindControl("BtnsUC");
            //}
            //XmlDocument xDocGVDataXML = new XmlDocument();
            //if (BtnsUserCtrl != null)
            //{
            //    xDocGVDataXML.LoadXml(BtnsUserCtrl.GVDataXml);
            //}
            //else
            //{
            //    xDocGVDataXML.LoadXml(returnXML);
            //}
            //XmlNode xNodeBPC = xDocGVDataXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            //if (xNodeBPC != null)
            //{
            //    m_BPCXml = xNodeBPC.OuterXml;
            //}
            //else
            //{
            //    m_BPCXml = string.Empty;
            //}
            ////Check the BusinessProcessControls if "Find" process is present or not.
            //if (GetBPCBPGID("Find", m_BPCXml).Length != 0)
            //{
            //    IsFindEnabled = true;
            //}

            //m_primaryKeyFieldName = dsRowList.Tables[0].Columns[0].ColumnName;

            ////Apply Row Hovering effects only if there is an alternating style applied.
            //if (grdVwContents.AlternatingRowStyle.CssClass.Length > 0)
            //{
            //    //Get the Row Hover Colour from the Config file which will be used in the RowDataBound event.
            //    m_RowHoverColour = ConfigurationManager.AppSettings["GridRowHoverColor"];
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "OnHover", "g_AllowRowHover=true;", true);
            //}

            ////If current page is an Help Auth page then remove the Time stamps for the file names.
            //DataTable dtToolTipData = UpdateToolTipInfo(dsRowList.Tables[0]);

            //m_sbToolTipJS.Remove(0, m_sbToolTipJS.Length);


            //grdVwContents.DataSource = dtToolTipData;
            //grdVwContents.DataBind();
            //if (hdnMaxPages.Value != "0")
            //{
            //    ddlPages.SelectedIndex = Convert.ToInt32(hdnCurrPageNo.Value) - 1;
            //}
            //lblPageStatus.Text = String.Concat(hdnCurrPageNo.Value, " of ", hdnMaxPages.Value);
            //lblPageNo.Text = "Go To Page";
            ////Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ToolTip", "g_ToolTipJS='" + m_sbToolTipJS.ToString() + "';", true);
            //hdnToolTipJS.Value = m_sbToolTipJS.ToString();
            //updtPnlGrdVw.Update();
            #endregion
        }

        public void GetSoxApprovalStatus(XmlDocument xDoc)
        {
            #region NLog
            logger.Info("Getting the SOXApproval status");
            #endregion

            XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");

            if (nodeBPC != null)
            {
                foreach (XmlNode nodeProcess in nodeBPC.ChildNodes)
                {
                    if (nodeProcess.Attributes["ID"].Value.Trim() == "SOXApproval")
                    {
                        if (nodeProcess.Attributes["BPGID"].Value.Trim() != null)
                            hdnSOXApprStatus.Value = nodeProcess.Attributes["BPGID"].Value.Trim();
                        else
                            hdnSOXApprStatus.Value = "No SOXAppr";
                    }
                    else
                    {
                        hdnSOXApprStatus.Value = "No SOXAppr";
                    }
                }

            }
        }

        //public void UIVisibility(XmlDocument xDocRPGV, string TreeNode)
        //{
        //    XmlDocument XDocUserInfo = (new CommonUI()).loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));

        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") != null))
        //    {
        //        jqGrid.ShowAddButton = true;
        //    }
        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']") != null))
        //    {
        //        jqGrid.ShowEditButton = true;
        //    }
        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Find']") != null))
        //    //|| this.Page.ToString().Contains("fullview"))
        //    {
        //        //Should not show 'Find' button in FullView page.
        //        jqGrid.ShowSearchButton = true;
        //    }
        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") != null))
        //    {
        //        jqGrid.ShowDeleteButton = true;
        //    }
        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsSecured").InnerText != "1"))
        //    {
        //        //trImgbtnSecure.Visible = false;
        //    }
        //    if ((XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='32']") == null)
        //    && (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsNoted").InnerText != "1"))
        //    {
        //        //trImgbtnNote.Visible = false;
        //    }
        //    if ((XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='33']") == null)
        //    && (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsAttached").InnerText != "1"))
        //    {
        //        //trImgbtnAttachment.Visible = false;
        //    }
        //    if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value) != "1")
        //    {
        //        //trImgbtnPrint.Visible = false;
        //    }
        //}

        /// <summary>
        /// Gets the matched BPC node from the given XML.
        /// </summary>
        /// <param name="processName">Process Name</param>
        /// <param name="bpcXml">Business Process Controls XML.</param>
        /// <returns></returns>
        private XmlNode GetBPCNode(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Getting the matched BPC node from the given XML for proces name as : "+processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
        }
        #endregion

        //#region Reporting Methods
        //protected void imgBtnExcelCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("EXCEL", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("EXCEL", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}

        //protected void imgBtnExcelAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    //objGridReports.PrintData(ReportType.Excel, PagesToPrint.All);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("EXCEL", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("EXCEL", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}

        //protected void imgBtnMSWord_Click(object sender, ImageClickEventArgs e)
        //{
        //}

        //protected void imgBtnWordCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("WORD", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("WORD", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}

        //protected void imgBtnWordAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    //objGridReports.PrintData(ReportType.Word, PagesToPrint.All);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("WORD", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("WORD", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}

        //protected void imgBtnHTMLCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    //objGridReports.PrintData(ReportType.HTML, PagesToPrint.Current);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("HTML", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("HTML", PagesToPrint.Current, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}

        //protected void imgBtnHTMLAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = SessionLinkBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    char[] arrDelemiter = { ':' };
        //    string[] arrColumns = hdnSelectedCols.Value.Split(arrDelemiter, StringSplitOptions.RemoveEmptyEntries);
        //    //objGridReports.PrintData(ReportType.HTML, PagesToPrint.All);
        //    if (arrColumns.Length > 0)
        //    {
        //        objGridReports.PrintData("HTML", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString(), arrColumns);
        //    }
        //    else
        //    {
        //        objGridReports.PrintData("HTML", PagesToPrint.All, ConfigurationManager.AppSettings["PrintBranchData"].ToString());
        //    }
        //}
        //#endregion
    }
}