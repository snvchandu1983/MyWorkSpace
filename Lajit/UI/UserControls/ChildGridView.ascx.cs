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
using LAjitControls;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLog;


namespace LAjitDev.UserControls
{
    public partial class ChildGridView : System.Web.UI.UserControl
    {
        private bool m_ShowOpLinks = true;
        //The form XMl for the current page.
        private XmlDocument m_xDocFormXML;
        //The current selected row in the gridview.
        private XmlNode m_nodeGVSelectedRow;
        private string m_primaryKeyFieldName;
        private int m_CurrPageNo;

        private bool m_StoreRowXML = false;
        private string m_OnGridLoadJSCall = string.Empty;
        private bool m_IsChildGridEnabled = false;
        private bool m_SelectColVisible = false;
        private bool m_IsFindMode = false;
        private ArrayList m_arrXMLDS = new ArrayList();

        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// To differentiate whether controls have already been added during multiple method calls in the same postback event.
        /// </summary>
        private bool m_AddAmountLabels = true;

        /// <summary>
        /// Holds the indices of the Drop Down Lists in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htDDLIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the Extended Drop Down Lists in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htEDDLIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the CheckBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htCheckBoxIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the Label in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htLabelIndices = new Hashtable();

        /// <summary>
        /// Stores the Label attributes of each of the columns
        /// </summary>
        private ArrayList m_arrColLabels = new ArrayList();

        /// <summary>
        /// Contains the list of all the IsRequired Columns
        /// </summary>
        private ArrayList m_arrIsRequiredCols = new ArrayList();

        /// <summary>
        /// Contains the list of indices of all the IsDisplayOnly Columns
        /// </summary>
        private ArrayList m_arrIsDisplayOnlyCols = new ArrayList();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htTextBoxIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htGVColWidths = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBox Calcualator in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htTextBoxCalcIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View with auto complete feature
        /// </summary>
        private Hashtable m_htTextBoxIsLink = new Hashtable();

        /// <summary>
        /// Holds the indices of the Links(ControlType=Link) in the Data Source of the Grid View with auto complete feature
        /// </summary>
        private Hashtable m_htLinkIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the Calendar in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htCalendarIndices = new Hashtable();

        /// <summary>
        /// Keeps record of the indices of columns with ControlType="Amount"
        /// </summary>
        private ArrayList m_arrAmountCols = new ArrayList();

        private CommonUI m_ObjCommonUI;

        private string m_PostBackControlID;

        private string m_BalanceMethodControl;

        public string PostBackControlID
        {
            get { return m_PostBackControlID; }
            set { m_PostBackControlID = value; }
        }


        private bool m_IsSelectInvoice = false;

        public bool IsSelectInvoice
        {
            get { return m_IsSelectInvoice; }
            set { m_IsSelectInvoice = value; }
        }

        private bool m_GridAlwaysEnabled = false;
        /// <summary>
        /// Specify that the grid is always in Enabled mode
        /// </summary>
        /// <example>SelectInvoice page requires grid to be enabled with all the unwanted columns are specified 
        /// to be IsDisplayOnly=1.</example>
        public bool GridAlwaysEnabled
        {
            get { return m_GridAlwaysEnabled; }
            set { m_GridAlwaysEnabled = value; }
        }


        private int m_Height;
        /// <summary>
        /// Set the height of the control in pixels.Default is 230px.
        /// </summary>
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        private bool m_EnableCollapse = false;
        /// <summary>
        /// Allows collpsing of GridView when clicked on the Plus icon.
        /// </summary>
        public bool EnableCollapse
        {
            get { return m_EnableCollapse; }
            set { m_EnableCollapse = value; }
        }

        private bool m_FitWidth = false;
        /// <summary>
        /// If true overrides any default widths set in the page. Default is false.
        /// </summary>
        public bool FitWidth
        {
            get { return m_FitWidth; }
            set { m_FitWidth = value; }
        }

        private bool m_HideSelectColumn = false;
        /// <summary>
        /// Hides the static checkbox column in the beginning if true.Default is false.
        /// </summary>
        public bool HideSelectColumn
        {
            get { return m_HideSelectColumn; }
            set { m_HideSelectColumn = value; }
        }

        private bool m_OnlyChildGVPresent = false;

        /// <summary>
        /// Specifies whether the containing page has only Child Grid View and not other Controls like GVUC and BtnsUC.
        /// </summary>
        /// <example> SelectRequest.aspx</example>
        public bool OnlyChildGVPresent
        {
            get { return m_OnlyChildGVPresent; }
            set { m_OnlyChildGVPresent = value; }
        }

        /// <summary>
        /// Contains the Row XML of the currently selected row in the grid view.
        /// </summary>
        public XmlNode GVSelectedRow
        {
            get { return m_nodeGVSelectedRow; }
            set { m_nodeGVSelectedRow = value; }
        }

        /// <summary>
        /// Set this reference of the CommonUI instance in the page load of the pages containing childGV
        /// </summary>
        public CommonUI ObjCommonUI
        {
            get { return m_ObjCommonUI; }
            set { m_ObjCommonUI = value; }
        }

        /// <summary>
        /// Whether to show the bottom operation links or not.
        /// </summary>
        public bool ShowOpLinks
        {
            get { return m_ShowOpLinks; }
            set { m_ShowOpLinks = value; }
        }

        private int m_RowsToDisplay;

        /// <summary>
        /// The no of rows to be displayed when the grid has intially loaded.
        /// </summary>
        public int RowsToDisplay
        {
            get { return m_RowsToDisplay == 0 ? 10 : m_RowsToDisplay; }
            set { m_RowsToDisplay = value; }
        }

        /// <summary>
        /// Holds the current number of records in the grid.
        /// </summary>
        protected int RowsInDisplay
        {
            get
            {
                if (hdnRowsToDisplay.Value.Trim().Length > 0)
                {
                    return Convert.ToInt32(hdnRowsToDisplay.Value);
                }
                else
                {
                    return 0;//Default
                }
            }
            set { hdnRowsToDisplay.Value = value.ToString(); }

        }

        /// <summary>
        /// Wrapper for hdnModRows.
        /// </summary>
        public string ModifiedRows
        {
            set { hdnModRows.Value = value; }
        }

        /// <summary>
        /// The name of the Branch Node with ControlType GView which is to be used to bind this grid.
        /// </summary>
        public string BranchNodeName
        {
            get { return ViewState["BranchNodeName"].ToString(); }
            set { ViewState["BranchNodeName"] = value; }
        }

        /// <summary>
        /// Set the page index of the user control explicitly.
        /// (Case wherein a new parent row has been selected and the ChildGV still maintains the old page index).
        /// </summary>
        public int PageIndex
        {
            get { return grdVwBranch.PageIndex; }
            set { grdVwBranch.PageIndex = value; }
        }

        /// <summary>
        /// Set the Paging property of the User Control.
        /// </summary>
        public bool AllowPaging
        {
            get { return grdVwBranch.AllowPaging; }
            set
            {
                grdVwBranch.AllowPaging = value;
                //Also set a varialble indicating the client-side of the status of paging.
                if (value == true)
                {
                    hdnIsPaging.Value = "1";
                }
            }
        }

        /// <summary>
        /// Allow Paging descriptor that stays constant throughout the page life cycle.
        /// </summary>
        public bool AllowPagingInner
        {
            get { return hdnIsPaging.Value == "1" ? true : false; }
        }


        private bool m_ClearDeletions = true;

        public bool ClearDeletions
        {
            get { return m_ClearDeletions; }
            set { m_ClearDeletions = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitialiseServerControls();
            Control ctrlPostBack = CommonUI.GetPostBackControl(this.Page);//ctrlPostBack.ClientID.Contains("BtnsUC") ||
            if (ctrlPostBack == null || (ctrlPostBack != null && (ctrlPostBack.ID.Contains("AddClone") || ctrlPostBack.ID.Contains("grdVwBranch")
               || ctrlPostBack.ID.Contains("Submit") || ctrlPostBack.ID.Contains("ContinueAdd")
               || ctrlPostBack.ID.Contains("lnkBtnAddRows") || ctrlPostBack.ID.Contains("imgbtnIsApproved")
               || ctrlPostBack.ID.Contains("lnkBtnReport") || ctrlPostBack.ID == "lnkBtnCloseIFrame")))//|| ctrlPostBack.ID == "lnkBtn"
            {
                BindUserControl(ctrlPostBack);
            }
        }

        private void BindUserControl(Control ctrlPostBack)
        {
            XmlDocument xDocOut = new XmlDocument();
            string strGVDataXML = ObjCommonUI.ButtonsUserControl.GVDataXml;
            if (!string.IsNullOrEmpty(strGVDataXML))
            {
                xDocOut.LoadXml(strGVDataXML);
                InitialiseBranchGrid(xDocOut, ctrlPostBack);
                /*if (alCalendarTBOX.Count > 0)
                {
                    JQueryUI.RegisterTBoxForDatePicker(ObjCommonUI.UpdatePanelContent.Page, alCalendarTBOX);
                }*/
            }
        }

        /// <summary>
        /// Initilises any properties of controls which require dynamic binding of the data.
        /// </summary>
        private void InitialiseServerControls()
        {
            if (!Page.IsPostBack)
            {
                lnkBtnAddRows.OnClientClick = "javascript:AddRowsClick('" + grdVwBranch.ClientID + "');return true;";
                lnkBtnDeleteRow.OnClientClick = "javascript:DeleteRow('" + grdVwBranch.ClientID + "', this);return false;";
                lnkBtnToggle.OnClientClick = "javascript:ToggleSelection('" + grdVwBranch.ClientID + "', this);return false;";
                lnkBtnCopy.OnClientClick = "javascript:CopyGridRows('" + grdVwBranch.ClientID + "',this);return false;";
                lnkBtnPaste.OnClientClick = "javascript:PasteGridRows('" + grdVwBranch.ClientID + "',this);return false;";

                if (AllowPaging)
                {
                    //Paging Stuff
                    RowsToDisplay = 10;
                    //To display success message and reducing the height based on the number of rows.
                    //RowsToDisplay = 8;
                    string themeName = Convert.ToString(Session["MyTheme"]);
                    grdVwBranch.PagerStyle.CssClass = "ChildGridPager";
                    grdVwBranch.PagerSettings.Mode = PagerButtons.NumericFirstLast;
                    //grdVwBranch.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
                    //grdVwBranch.PagerStyle.Width = Unit.Pixel(40);
                    grdVwBranch.PagerSettings.FirstPageImageUrl = Application["ImagesCDNPath"].ToString() + "images/arrow-first.gif";
                    //= "~/App_Themes/" + themeName + "/images/arrow-first.gif";
                    grdVwBranch.PagerSettings.PreviousPageImageUrl = Application["ImagesCDNPath"].ToString() + "images/arrow-left.gif";
                    grdVwBranch.PagerSettings.NextPageImageUrl =Application["ImagesCDNPath"].ToString() + "images/arrow-right.gif";
                    grdVwBranch.PagerSettings.LastPageImageUrl = Application["ImagesCDNPath"].ToString() + "images/arrow-last.gif";
                    grdVwBranch.PageSize = RowsToDisplay;//Convert.ToInt32(ConfigurationManager.AppSettings["BranchGVMaxRows"]);
                }
            }
        }

        /// <summary>
        /// Initialises the objects present under the Tree node in the GridLayout node.
        /// </summary>
        /// <param name="xDocOut">The return document for the current page.</param>
        /// <param name="pnlEntryForm">The parent control which contains all the UI I/O elements.</param>
        public void InitialiseBranchGrid(XmlDocument xDocOut, Control postBackCtrl)
        {
            #region NLog
            logger.Info("Initialises the objects present under the Tree node in the GridLayout node."); 
            #endregion

            hdnAmounts.Value = "";
            BindGrid(xDocOut, BranchNodeName, postBackCtrl);
            upCGVUC.Update();
        }

        /// <summary>
        /// Performs all the grid view initialisation operations.
        /// </summary>
        /// <param name="xDocOut">The current page's document.</param>
        /// <param name="branchNodeName">The Branch Node Name.</param>
        /// <param name="nodeBranchColumns">The Col Node</param>
        /// <param name="grdVwBranch">Gridview Object</param>
        private void BindGrid(XmlDocument xDocOut, string branchNodeName, Control postBackCtrl)
        {
            #region NLog
            logger.Info("Performs all the grid view initialisation operations.");
            #endregion

            DataSet dsRowList = new DataSet();
            //Get the child node rows only belonging to the selected parent
            string parentTrxID = string.Empty;
            string parentTrxType = string.Empty;
            string GVTreeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            XmlNode nodeBranchColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
            if (nodeBranchColumns == null)
            {
                return;
            }
            if (postBackCtrl != null && (postBackCtrl.ID.Contains("imgBtnTTnNavigate") || postBackCtrl.ID.Contains("imgbtnAdd") || postBackCtrl.ID.Contains("imgbtnSubmit")))
            {
                InitCheckBoxSelection(true);
            }
            else
            {
                InitCheckBoxSelection(false);
            }

            string gridMode = string.Empty;
            if (ObjCommonUI.UpdatePanelContent != null)
            {
                //Added by shanti.
                if (ObjCommonUI.ButtonsUserControl != null)
                {
                    gridMode = ObjCommonUI.ButtonsUserControl.CurrentAction.ToUpper();
                }

                //Calcuting the enabled/disabled/colour status of the child gridview.
                if (postBackCtrl != null && !postBackCtrl.ID.Contains("Cancel")
                    && (postBackCtrl.ID.Contains("Add") || postBackCtrl.ID.Contains("Modify") || postBackCtrl.ID.Contains("Find") || postBackCtrl.ID.Contains("timer")))
                {
                    m_IsChildGridEnabled = true;
                    if (postBackCtrl.ID.Contains("Find") || gridMode == "FIND")
                    {
                        m_IsFindMode = true;
                    }
                }
                else if (postBackCtrl == null || postBackCtrl.ID == "imgbtnCancel")//Implies page load(mostly) or Cancel Click.If it is cancel click then show the user the view similar to that of page load.
                {
                    //Find the parent node to know the number of rows. No rows implies that page is in ADD mode
                    XmlNode nodeParentRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");
                    if (nodeParentRows == null || nodeParentRows.ChildNodes.Count == 0)
                    {
                        //Case of no parent records present, so lands in the page in "ADD" mode
                        m_IsChildGridEnabled = true;
                    }
                    else if (nodeParentRows.ChildNodes.Count == 1)
                    {
                        //if (xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText.Contains("SelectRequest"))
                        //{
                        //    m_IsChildGridEnabled = true;
                        //    m_IsFindMode = true;
                        //}
                        //else
                        //{
                        m_IsChildGridEnabled = false;
                        //}
                    }
                }
                else if (postBackCtrl.ID == "imgbtnSubmit" && gridMode == "FIND")
                {
                    //After find criteria submitted remain in the FIND mode so that upon error it will be already in FIND mode.
                    m_IsChildGridEnabled = true;
                    m_IsFindMode = true;
                }
                else if ((gridMode == "MODIFY" || gridMode == "ADD"))//Changed from if to Else If on NOV 3rd
                {
                    //19-12-2008-Added the below 3 conditions for Note,Attachment and secure.
                    if (postBackCtrl != null && (postBackCtrl.ID.Contains("Submit") || postBackCtrl.ID.Contains("Note") || postBackCtrl.ID.Contains("Secure") || postBackCtrl.ID.Contains("Attachment")))
                    {
                        m_IsChildGridEnabled = false;//Success Process Complete
                    }
                    else
                    {
                        if (!postBackCtrl.ID.Contains("Cancel"))//25-11-08 - For DisabledModifyLoad case, cancel click
                        {
                            m_IsChildGridEnabled = true;
                        }
                    }
                }
            }
            else
            {
                //Select Request Page
                if (OnlyChildGVPresent)
                {
                    gridMode = "SELECT";
                    m_IsChildGridEnabled = true;
                    m_IsFindMode = true;
                    pnlGVBranch.Width = Unit.Percentage(100);
                    tblOpLinks.Visible = false;
                }
            }

            //Retreiving the TrxID and TrxType of the Parent row.
            if (ObjCommonUI.ButtonsUserControl == null || ObjCommonUI.ButtonsUserControl.RwToBeModified == null || ObjCommonUI.ButtonsUserControl.RwToBeModified.Length == 0)
            {
                if (GVSelectedRow != null)
                {
                    parentTrxID = GVSelectedRow.Attributes["TrxID"].Value;
                    parentTrxType = GVSelectedRow.Attributes["TrxType"].Value;
                }
                else if (OnlyChildGVPresent)//(this.Page.ToString().Contains("selectrequest_aspx"))
                {
                    XmlNode nodeParentRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");//Only one parent node.
                    if (nodeParentRows != null)
                    {
                        parentTrxID = nodeParentRows.ChildNodes[0].Attributes["TrxID"].Value;
                        parentTrxType = nodeParentRows.ChildNodes[0].Attributes["TrxType"].Value;
                    }
                }
            }
            else
            {
                XmlDocument xDocSelectedRow = new XmlDocument();
                xDocSelectedRow.LoadXml(ObjCommonUI.ButtonsUserControl.RwToBeModified);
                parentTrxID = xDocSelectedRow.SelectSingleNode("Rows").Attributes["TrxID"].Value;
                parentTrxType = xDocSelectedRow.SelectSingleNode("Rows").Attributes["TrxType"].Value;
            }

            if (FitWidth)
            {
                //Example Commercial.aspx
                pnlGVBranch.Width = Unit.Empty;
                tblCGVTop.Style.Clear();
                pnlGVBranch.ScrollBars = ScrollBars.Vertical;
            }
            if (EnableCollapse)
            {
                htcStartSpacer.Visible = true;
                htcExpandImage.Visible = true;//Show the collapse/expand button.

                //Check if the cpeObj has been added earlier.(Case of submit)
                if (this.FindControl("cpeGrid") == null)
                {
                    //JQUERY Collpasable Extendar javascript adding onclick on image and panel
                    htcExpandImage.Attributes.Add("onclick", "CPGVExpandCollapse('ctl00_cphPageContents_CGVUC_pnlTitle');");
                    htcHeader.Attributes.Add("onclick", "CPGVExpandCollapse('ctl00_cphPageContents_CGVUC_pnlTitle');");

                }
            }
            if (GridAlwaysEnabled)
            {
                m_IsChildGridEnabled = true;
            }
            if (HideSelectColumn)
            {
                grdVwBranch.Columns[0].Visible = false;
            }

            string xPath = "Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + GVTreeNodeName + "_TrxID='" + parentTrxID + "' and @" + GVTreeNodeName + "_TrxType='" + parentTrxType + "']";
            XmlNodeList nodeBranchRows = xDocOut.SelectNodes(xPath);

            //************************Create a XML node object for the dataset to read the XML.
            XmlNode nodeRows = xDocOut.CreateElement("RowList");
            foreach (XmlNode nodeChildRow in nodeBranchRows)
            {
                nodeRows.AppendChild(nodeChildRow);
            }

            //|| m_IsFindMode
            if (postBackCtrl != null && (postBackCtrl.ID.ToUpper() == "IMGBTNADD" || postBackCtrl.ID == "imgbtnContinueAdd") && !postBackCtrl.ID.Contains("AddNew"))
            {
                dsRowList = GetNewTableSchema(xDocOut, branchNodeName, dsRowList);
            }
            else
            {
                bool executeReader = true;
                //TODO:The AND Condition.
                if (gridMode == "ADD")
                {
                    if (postBackCtrl != null && (postBackCtrl.ID.Contains("lnkBtnAutoFillVendor") || postBackCtrl.ID.Contains("imgbtnSubmit")))
                    {
                        executeReader = true;
                    }
                    else
                    {
                        executeReader = false;
                    }
                }

                //Added a condition to prevent the grid binding with the previously selected parent row's data upon SelectRequest operation on 17/10/08
                if (nodeRows != null && nodeRows.ChildNodes.Count > 0 && executeReader)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    dsRowList.ReadXml(read);
                }
            }

            if (dsRowList.Tables.Count == 0)
            {
                dsRowList = GetNewTableSchema(xDocOut, branchNodeName, dsRowList);
            }

            //Persist the number of rows originally present in the gridview
            while (dsRowList.Tables[0].Rows.Count < Math.Max(RowsToDisplay, RowsInDisplay))
            {
                dsRowList.Tables[0].Rows.Add(dsRowList.Tables[0].NewRow());
            }

            RowsInDisplay = dsRowList.Tables[0].Rows.Count;

            ////Dataset is ready at this point.Check if number of rows is greater than 100 to introduce paging.
            ////The grd.PageSize is already intialised with the LAjit DefaultPageSize value fed from the XML.
            //if (dsRowList.Tables[0].Rows.Count > grdVwBranch.PageSize)
            //{
            //    grdVwBranch.AllowPaging = true;
            //    lnkBtnDeleteRow.OnClientClick = "alert('Under Development(Paging functionality)');return false;";
            //}
            //else
            //{
            //    grdVwBranch.AllowPaging = false;
            //}

            //if (this.AllowPaging)
            //{
            //    lnkBtnDeleteRow.OnClientClick = "alert('Under Development(Paging functionality)');return false;";
            //}

            //Initialising the Bound fields.
            bool isOkToAddFields = false;
            if (grdVwBranch.Columns.Count == 1)
            {
                isOkToAddFields = true;
            }

            //Set the Grid Heading
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Title").InnerText;
            htcHeader.InnerText = headerTitle;


            //Reset necessary fieds.
            int colCntr = 0;
            int processColCntr = 0;//Tracks the number of columns added due to BusinessProcesses.
            m_htDDLIndices.Clear();
            m_htEDDLIndices.Clear();
            m_htLabelIndices.Clear();
            m_htCheckBoxIndices.Clear();
            m_htCalendarIndices.Clear();
            m_htTextBoxIndices.Clear();
            m_htTextBoxCalcIndices.Clear();
            m_htTextBoxIsLink.Clear();
            m_arrAmountCols.Clear();

            bool columnsExist = false;

            //Loop the columns.
            foreach (XmlNode colNode in nodeBranchColumns.ChildNodes)
            {
                //Add the column only if FullViewLength is not equal to zero
                int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)
                {
                    columnsExist = true;
                    string currentLabel = colNode.Attributes["Label"].Value.Trim();
                    //Adding the current column to the dataset if not present.
                    if (!dsRowList.Tables[0].Columns.Contains(currentLabel))
                    {
                        DataColumn dcNew = new DataColumn(currentLabel, typeof(string));
                        dcNew.AllowDBNull = true;
                        dsRowList.Tables[0].Columns.Add(dcNew);
                    }

                    BoundField newField = new BoundField();
                    newField.DataField = currentLabel;
                    newField.HeaderText = colNode.Attributes["Caption"].Value.Trim();
                    newField.HeaderStyle.Wrap = true;

                    //Check for IsDisplayOnly
                    bool isDisplayOnly = false;
                    if (colNode.Attributes["IsDisplayOnly"] != null
                                && colNode.Attributes["IsDisplayOnly"].Value.Trim() == "1")
                    {
                        m_arrIsDisplayOnlyCols.Add(colCntr);
                    }

                    if (colNode.Attributes["ControlType"] != null)
                    {
                        switch (colNode.Attributes["ControlType"].Value)
                        {
                            case "EDDL":
                                {
                                    //Extended DropDownList
                                    m_htDDLIndices.Add(colCntr, currentLabel);
                                    m_htEDDLIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "DDL":
                                {
                                    //DropDownList
                                    m_htDDLIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Label":
                                {
                                    //Label
                                    m_htLabelIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Cal":
                                {
                                    //Calendar
                                    //Apply formatting to the DateTime fields in the DataSet.
                                    foreach (DataRow dr in dsRowList.Tables[0].Rows)
                                    {
                                        //If the value is IsDate then change format MM/DD/YY
                                        DateTime date;
                                        if (DateTime.TryParse(dr[currentLabel].ToString(), out date))
                                        {
                                            dr[currentLabel] = date.ToString("MM/dd/yy");
                                        }
                                    }
                                    m_htCalendarIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Check":
                                {
                                    //CheckBox
                                    m_htCheckBoxIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "TBox":
                                {
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    //If textbox contains islink attribute
                                    if ((colNode.Attributes["IsLink"] != null) && (colNode.Attributes["IsLink"].Value == "1"))
                                    {
                                        //if (currentLabel == "EntryJob")
                                        //{
                                        //    HttpContext.Current.Session["EntryJob"] = GetDropDownData("EntryJob");
                                        //}
                                        m_htTextBoxIsLink.Add(colCntr, currentLabel);
                                    }
                                    break;
                                }
                            case "Amount":
                                {
                                    //Presumptions for this Control Type
                                    //It is a textbox
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    m_arrAmountCols.Add(colCntr);
                                    break;
                                }
                            case "Calc":
                                {
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    m_arrAmountCols.Add(colCntr);
                                    m_htTextBoxCalcIndices.Add(colCntr, currentLabel);
                                    //If textbox contains islink attribute
                                    if ((colNode.Attributes["IsLink"] != null) && (colNode.Attributes["IsLink"].Value == "1"))
                                    {
                                        m_htTextBoxIsLink.Add(colCntr, currentLabel);
                                    }
                                    break;
                                }
                            case "Link":
                                {
                                    m_htLinkIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    //Set the sort expression as inidicated in the xml.
                    if (colNode.Attributes["IsSortable"].Value == "1")
                    {
                        newField.SortExpression = currentLabel;
                    }

                    if (colNode.Attributes["IsRequired"] != null && colNode.Attributes["IsRequired"].Value == "1")
                    {
                        m_arrIsRequiredCols.Add(colCntr);
                    }

                    ////Get the name of the control to which Balance Method is assigned.
                    //if (colNode.Attributes["IsSummed"] != null)
                    //{
                    //    if (colNode.Attributes["IsSummed"].Value == "1")
                    //    {
                    //        if (colNode.Attributes["BalanceMethod"] != null)
                    //        {
                    //            if (colNode.Attributes["BalanceMethod"].Value.Trim() == "3")
                    //            {
                    //Commented the above lines because AutoBalance was not not working when Balance method was turned off.
                    XmlAttribute attBalLabel = colNode.Attributes["BalanceLabel"];
                    if (attBalLabel != null) //Auto Balance should not have any dependency on Balance Method.
                    {
                        m_BalanceMethodControl = attBalLabel.Value + "~" + colCntr;
                    }
                    //            }
                    //        }
                    //    }
                    //}
                    //Check if the current column node has any processes assigned
                    XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                    if (attBPControl != null && attBPControl.Value.Trim().Length > 0)
                    {
                        TableRow trBPCLinks = tblBPCLinks.Rows[0];
                        string procName = attBPControl.Value;
                        XmlNode nodeProc = GetBPCNode(procName, xDocOut);
                        string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                        string pageInfo = nodeProc.Attributes["PageInfo"].Value;

                        TableCell tdLinkButton = new TableCell();
                        LAjitLinkButton lnkBtnBPC = new LAjitLinkButton();
                        lnkBtnBPC.Enabled = true;// !m_IsChildGridEnabled;//Being handled in the client-side
                        lnkBtnBPC.ID = "lnkBtnBPC" + processColCntr;
                        lnkBtnBPC.Text = nodeProc.Attributes["Label"].Value;
                        lnkBtnBPC.OnClientClick = "javascript:OnChildBPCLinkClick(this,'" + currentBPGID + "','" + pageInfo + "','"
                                    + grdVwBranch.ClientID + "','" + nodeProc.Attributes["ID"].Value + "','" + nodeProc.Attributes["IsPopup"].Value + "');return false";
                        tdLinkButton.Controls.Add(lnkBtnBPC);

                        TableCell tdSeparator = new TableCell();
                        tdSeparator.Width = Unit.Pixel(6);
                        tdSeparator.Text = "|";
                        Control ctrlBPC = tblBPCLinks.FindControl(lnkBtnBPC.ID);
                        if (ctrlBPC == null)
                        {
                            trBPCLinks.Controls.Add(tdLinkButton);
                            trBPCLinks.Controls.Add(tdSeparator);
                        }
                        else
                        {
                            ((LAjitLinkButton)ctrlBPC).Enabled = true;// !m_IsChildGridEnabled;
                        }
                        processColCntr++;
                        m_StoreRowXML = true;
                    }

                    //Add the field now.
                    if (isOkToAddFields)
                    {
                        grdVwBranch.Columns.Add(newField);
                    }
                    //Set the widhts of the controls in the column as per the FullView Length
                    grdVwBranch.Columns[colCntr + 1].ControlStyle.Width = Unit.Pixel(colFullViewLength * 7);//Assuming one character to be around 7px in width
                    grdVwBranch.Columns[colCntr + 1].ItemStyle.Width = Unit.Pixel(colFullViewLength * 7);
                    m_arrColLabels.Add(currentLabel);
                    colCntr++;
                }
                else
                {   // fullview length zero
                    // Get Default JobCOA for AutoFillJob column
                    if (colNode.Attributes["Label"].Value == "JobCOA")
                    {
                        if (colNode.Attributes["Default"] != null)
                        {
                            //m_Default_JobCOA = colNode.Attributes["Default"].Value;
                            //Access This Value in Client Side
                            hdnJobCOADefault.Value = colNode.Attributes["Default"].Value;
                        }
                    }
                }

            }

            if (!columnsExist)
            {
                //If no columns with FullViewLength exist then hide the entire ChildGV.
                this.Visible = false;
                return;
            }

            ///int cellCount = 9;//No of cells containing the operationlinks, their separators.
            if (m_ShowOpLinks == false)
            {
                tblOpLinks.Visible = false;
            }



            //Shanti
            //Displaying the CGVOpLinks dependng on particular attributes. If IsGVAddDisabled=0, then only lnkBtnAddRows should b visible
            DisplayOpLinks(xDocOut);

            m_SelectColVisible = grdVwBranch.Columns[0].Visible;

            //Check for SelectInvoice.aspx-Display only sum of selected rows.
            string addSelected = "false";
            if (this.Page.ToString().Contains("selectinvoice"))
            {
                addSelected = "true";
            }

            TableRow tr = tblAmounts.Rows[0];//Only one biigg row.
            StringBuilder sbAmountCols = new StringBuilder(m_arrAmountCols.Count * 2);
            foreach (int amtColIndex in m_arrAmountCols)
            {
                //int amtColIndexClient = (m_SelectColVisible) ? amtColIndex : amtColIndex - 1;
                int amtColIndexClient = amtColIndex;

                sbAmountCols.Append(amtColIndexClient + "*");
                string amtColName = ((BoundField)grdVwBranch.Columns[amtColIndex + 1]).DataField;//+1 override the static column.
                //Calculate the column's initial sum.
                decimal totalAmount = 0;
                decimal rowAmount;
                //Commented code's functionality is being acheived through javascript.
                //if (Page.IsPostBack)
                //{
                if (!IsSelectInvoice)
                {
                    foreach (DataRow dr in dsRowList.Tables[0].Rows)
                    {
                        if (decimal.TryParse(Convert.ToString(dr[amtColName]), out rowAmount))
                        {
                            totalAmount += rowAmount;
                        }
                    }
                }
                if (addSelected.Equals("true"))
                {
                    //Get the LabelID of the select row checkbox passed through the XML.
                    //TODO: Get it from the XML.XmlNode nodeSelectit =
                    string selectLabel = "Selectit";
                    if (dsRowList.Tables[0].Columns.Contains(selectLabel))
                    {
                        int indexOfSelect = dsRowList.Tables[0].Columns[selectLabel].Ordinal;
                        foreach (DataRow dr in dsRowList.Tables[0].Rows)
                        {
                            if (dr[indexOfSelect].ToString() == "1")
                            {
                                if (decimal.TryParse(Convert.ToString(dr[amtColName]), out rowAmount))
                                {
                                    totalAmount += rowAmount;
                                }
                            }
                        }
                    }
                }
                hdnAmounts.Value += string.Format("{0:N}|{1}", totalAmount, amtColIndex) + "~";
                //}


                if (m_AddAmountLabels && !AllowPaging)
                {
                    TableCell tdSeparatorAmt = new TableCell();
                    tdSeparatorAmt.Width = Unit.Pixel(6);
                    tdSeparatorAmt.Text = "|";

                    TableCell tdAmountSum = new TableCell();
                    tdAmountSum.ID = "tdAmount" + amtColIndexClient;
                    tdAmountSum.CssClass = "mbodyb";
                    tdAmountSum.Text = grdVwBranch.Columns[amtColIndex + 1].HeaderText + " : <span id='tdAmt" + amtColIndexClient + "'>" + string.Format("{0:N}", totalAmount) + "</span>";

                    if (tr.FindControl("tdAmount" + amtColIndexClient) == null)
                    {
                        //IF current col is the first column being added and there no other BPC links then...
                        if ((amtColIndex == Convert.ToInt32(m_arrAmountCols[0])) && tr.Cells.Count == 0)
                        {
                            tr.Controls.Add(tdAmountSum);//Add only the text
                        }
                        else
                        {
                            tr.Controls.Add(tdSeparatorAmt);
                            tr.Controls.Add(tdAmountSum);
                        }
                    }
                }
                else
                {
                    TableCell tdAmountSum = (TableCell)tr.FindControl("tdAmount" + amtColIndexClient);
                    if (tdAmountSum != null)
                    {
                        tdAmountSum.Text = grdVwBranch.Columns[amtColIndex + 1].HeaderText + " : <span id='tdAmt" + amtColIndexClient + "'>" + totalAmount + "</span>";
                    }

                }
            }
            m_AddAmountLabels = false;
            //Added by shanti
            if (m_IsChildGridEnabled == true)
            {
                tblOpLinks.Attributes.Add("style", "DISPLAY: Block;");
            }
            else
            {
                tblOpLinks.Attributes.Add("style", "DISPLAY: none;");
            }


            m_OnGridLoadJSCall = "if(typeof DisplayOnGridLoadSum== 'function')DisplayOnGridLoadSum('" + grdVwBranch.ClientID + "', '" + sbAmountCols.ToString() + "');var g_AddSelected=" + addSelected + ";";
            ScriptManager.RegisterClientScriptBlock(grdVwBranch.Page, grdVwBranch.Page.GetType(), "ShowOnGridLoadSum", m_OnGridLoadJSCall, true);

            //If the data source consists of more than specified records implement vertical scrolling.
            //Implement vertical scrolling by default
            pnlGVBranch.ScrollBars = ScrollBars.Both;
            if (Height != 0)
            {
                pnlGVBranch.Height = Unit.Pixel(Height);
            }
            else
            {
                if (this.AllowPaging)
                {
                    pnlGVBranch.Height = Unit.Empty;
                    pnlGVBranch.ScrollBars = ScrollBars.Auto;
                }
                else
                {
                    //pnlGVBranch.Height = Unit.Pixel(230);
                    pnlGVBranch.Height = Unit.Pixel(186);
                }
            }

            ////The following code was commented by Danny on 18 Dec 09 to avoid display of scrollbars when 
            ////paging is available.
            //string currentAction = ObjCommonUI.ButtonsUserControl.CurrentAction;
            //if (!string.IsNullOrEmpty(currentAction))
            //{
            //    if (currentAction == "Add" && AllowPaging)
            //    {
            //        pnlGVBranch.Height = Unit.Pixel(230);
            //        //pnlGVBranch.Height = Unit.Pixel(186);
            //        pnlGVBranch.ScrollBars = ScrollBars.Both;
            //    }
            //}

            int maxRowSize = Convert.ToInt32(ConfigurationManager.AppSettings["BranchGVMaxRows"]);
            if (dsRowList.Tables[0].Rows.Count < maxRowSize)
            {
                //Find the immediate parent container panel of the grid view 
                pnlGVBranch.Style.Add("height", "auto");
                pnlGVBranch.ScrollBars = ScrollBars.Horizontal;

            }

            bool isPaging = false;
            if (this.AllowPaging)
            {
                isPaging = true;
                if (ObjCommonUI.ButtonsUserControl.CurrentAction == "Add")
                {
                    //Check if the postback has not originated from controls within the gridview.
                    //Ex: Javascript setting of the page explicitly to the first page when in ADD.
                    if (postBackCtrl != null && postBackCtrl.ID != grdVwBranch.ID)
                    {
                        //Disable the paging temporarily when adding more records in ADD mode.
                        this.AllowPaging = false;
                    }
                }
            }

            m_primaryKeyFieldName = dsRowList.Tables[0].Columns[0].ColumnName;
            grdVwBranch.DataSource = dsRowList;
            grdVwBranch.DataBind();

            //Reset the Paging property to the earlier value.
            this.AllowPaging = isPaging;

            //Add validation for Cancel(Ex Changes submitted to server or not..)
            if (ObjCommonUI.UpdatePanelContent != null)
            {
                ImageButton imgbtnCancel = (ImageButton)ObjCommonUI.UpdatePanelContent.FindControl("imgbtnCancel");
                imgbtnCancel.OnClientClick = "return ValidateChangesSubmitted('" + this.ClientID + "')";
            }

            //Client-side functionality done Shanti requires no disabling or changing of colour be done on the server-side
            m_IsChildGridEnabled = true;
            m_IsFindMode = false;
        }

        public void ResetRowsToDisplay()
        {
            //RowsToDisplay = 10;
        }

        /// <summary>
        /// Hide the OpLinks based on the corresponding attributes.
        /// </summary>
        /// <param name="xDocOut">Form XML Document.</param>
        private void DisplayOpLinks(XmlDocument xDocOut)
        {
            XmlNode nodeBranch = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[Node='" + BranchNodeName + "']");
            if (nodeBranch != null)
            {
                short disabledOpLinksCntr = 0;
                XmlAttribute attTemp = nodeBranch.Attributes["IsGVAddDisabled"];
                if (attTemp != null && attTemp.Value == "1")
                {
                    lnkBtnAddRows.Visible = false;
                    addSeperator.Visible = false;
                    disabledOpLinksCntr++;
                }
                attTemp = nodeBranch.Attributes["IsGVDeleteDisabled"];
                if (attTemp != null && attTemp.Value == "1")
                {
                    lnkBtnDeleteRow.Visible = false;
                    deleteSeperator.Visible = false;
                    disabledOpLinksCntr++;
                }
                attTemp = nodeBranch.Attributes["IsGVToggleDisabled"];
                if (attTemp != null && attTemp.Value == "1")
                {
                    lnkBtnToggle.Visible = false;
                    toggleSeperator.Visible = false;
                    disabledOpLinksCntr++;
                }
                attTemp = nodeBranch.Attributes["IsGVCopyDisabled"];
                if (attTemp != null && attTemp.Value == "1")
                {
                    lnkBtnCopy.Visible = false;
                    copySeperator.Visible = false;
                    disabledOpLinksCntr++;
                }
                attTemp = nodeBranch.Attributes["ISGVPasteDisabled"];
                if (attTemp != null && attTemp.Value == "1")
                {
                    lnkBtnPaste.Visible = false;
                    disabledOpLinksCntr++;
                }
                attTemp = nodeBranch.Attributes["ISGVSelectDisabled"];
                if ((disabledOpLinksCntr == 5) || (attTemp != null && attTemp.Value == "1"))
                {
                    grdVwBranch.Columns[0].Visible = false;
                    m_HideSelectColumn = true;
                }
            }
        }

        /// <summary>
        /// Row Data bound event for the grid view object
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments.</param>
        public void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            string[] checkBoxSelection = ((GridView)sender).Attributes["CheckBoxSel"].Split(',');
            string gridClientID = grdVwBranch.ClientID;
            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Set an ID to row(To be used in the client-side fading effects)
                e.Row.ID = "GVRow" + e.Row.RowIndex;
                int cellCntr = 0;

                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();
                //Create the row xml to append to the checkbox as attribute
                string rowXML = m_StoreRowXML ? GetRowXml(drvCurrent.DataView.Table, primaryKeyValue) : string.Empty;

                //IsOkToDelete functionality
                int indexOfIOTD = GetColumnIndex("IsOkToDelete", drvCurrent.DataView.Table);
                if (indexOfIOTD != -1)
                {
                    string isOkToDelete = e.Row.Cells[indexOfIOTD].Text;
                    if (isOkToDelete == "1")
                    {
                        e.Row.Attributes.Add("isOkToDelete", "1");
                    }
                }
                foreach (TableCell tc in e.Row.Cells)
                {
                    int currentCellIndex = e.Row.Cells.GetCellIndex(tc);
                    string cellText = tc.Text.Trim();
                    if (cellText == "&nbsp;")
                    {
                        cellText = string.Empty;
                    }
                    //Re-Init the checkbox
                    if (currentCellIndex == 0)//First cell consists of the checkbox.
                    {
                        CheckBox chkBxSelect = (CheckBox)tc.FindControl("chkBxSelectRow");
                        if (m_StoreRowXML)
                        {
                            chkBxSelect.Attributes["RowInfo"] = rowXML;
                        }

                        chkBxSelect.Attributes["RowIndex"] = e.Row.RowIndex.ToString();

                        for (int i = 0; i < checkBoxSelection.Length; i++)
                        {
                            if (checkBoxSelection[i] == e.Row.RowIndex.ToString())
                            {
                                chkBxSelect.Checked = true;
                                break;
                            }
                        }
                        continue;
                    }
                    tc.Controls.Clear();
                    //tc.Enabled = m_IsChildGridEnabled;
                    if (m_htDDLIndices.ContainsKey(cellCntr))
                    {
                        //Implies this cell should contain a Drop Down List
                        string labelColumn = m_htDDLIndices[cellCntr].ToString();
                        string ddlTrxID = labelColumn + "_TrxID";
                        string ddlTrxType = labelColumn + "_TrxType";
                        string selectedValue = string.Empty;
                        string cellContent = drvCurrent.Row[labelColumn].ToString();
                        if (drvCurrent.DataView.Table.Columns.Contains(ddlTrxID) && drvCurrent.DataView.Table.Columns.Contains(ddlTrxType))
                        {
                            selectedValue = drvCurrent.Row[ddlTrxID].ToString() + "~" + drvCurrent.Row[ddlTrxType].ToString();
                        }
                        else if (cellContent.Length > 0)
                        {
                            //Check for the existence of a selected value string in the column
                            //For rebinding the data which has been entered in the UI before submission to the server.
                            if (cellContent.Contains("~"))//Validate for correct selected value format(TrxId~TrxType)
                            {
                                selectedValue = cellContent;
                            }
                        }

                        if (selectedValue.Trim().Length == 1)
                        {
                            selectedValue = "";
                        }

                        string rowType = string.Empty;
                        if (drvCurrent.DataView.Table.Columns.Contains("RowType"))
                        {
                            rowType = drvCurrent.Row["RowType"].ToString();
                        }

                        //Check for an ExtendedDDL
                        string EDDLTrxType = string.Empty;
                        if (m_htEDDLIndices.Contains(cellCntr))
                        {
                            EDDLTrxType = drvCurrent.Row["EDDL_TrxType"].ToString();
                        }

                        LAjitDropDownList ddlCurrent = CreateDropDownList(labelColumn, selectedValue, rowType, e.Row.ClientID.ToString(), EDDLTrxType);
                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            ddlCurrent.Attributes.Add("onblur", "javascript:ValidateChildControl(this)");
                        }

                        //Update only the rows which were modified.
                        if (AllowPagingInner)
                        {
                            ddlCurrent.Attributes["onchange"] += "SetModRowIndices('" + hdnModRows.ClientID + "','" + e.Row.RowIndex + "')";
                        }

                        tc.Controls.Add(ddlCurrent);
                    }
                    else if (m_htCalendarIndices.ContainsKey(cellCntr))
                    {
                        //Calendar Control Textbox with cal image
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        string currentControlName = Convert.ToString(m_arrColLabels[cellCntr]);
                        LAjitTextBox txtCurrentCell = new LAjitTextBox();
                        txtCurrentCell.ID = "txt" + currentControlName;
                        txtCurrentCell.Text = cellText;
                        txtCurrentCell.MapXML = currentControlName;

                        if (m_arrIsDisplayOnlyCols.Contains(cellCntr))//Show the Control in its actual form but disable it.
                        {
                            //txtCurrentCell.Enabled = false;
                            //txtCurrentCell.BackColor = Color.LightGray;
                            tc.Controls.Add(txtCurrentCell);
                            cellCntr++;
                            continue;
                        }
                        //Add client-side script to validate the date selected/entered.
                        txtCurrentCell.Attributes.Add("onchange", "javascript:CheckDate(this);");

                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            txtCurrentCell.Attributes.Add("onblur", "ValidateChildControl(this);");
                        }
                        //Update only the rows which were modified.
                        if (AllowPagingInner)
                        {
                            txtCurrentCell.Attributes["onchange"] += "SetModRowIndices('" + hdnModRows.ClientID + "','" + e.Row.RowIndex + "')";
                        }

                        tc.Controls.Add(txtCurrentCell);


                        if (!ObjCommonUI.m_alCGCalendarTBoxIDS.Contains(txtCurrentCell.ClientID))
                        {
                            ObjCommonUI.m_alCGCalendarTBoxIDS.Add(txtCurrentCell.ClientID);
                        }
                        if (!ObjCommonUI.m_alCGMaskTBoxIDS.Contains(txtCurrentCell.ClientID))
                        {
                            ObjCommonUI.m_alCGMaskTBoxIDS.Add(txtCurrentCell.ClientID);
                        }
                    }
                    else if (m_htTextBoxIndices.ContainsKey(cellCntr))
                    {
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        LAjitTextBox txtCurrentCell = new LAjitTextBox();
                        tc.Controls.Add(txtCurrentCell);//Make sure the first control to be added to the cell is the Texbox rather than any extender objects.

                        if (m_htTextBoxIndices.ContainsKey(cellCntr) != m_htTextBoxCalcIndices.ContainsKey(cellCntr))
                        {
                            txtCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                            txtCurrentCell.ID = "txt" + Convert.ToString(m_arrColLabels[cellCntr]);
                            txtCurrentCell.Text = ObjCommonUI.HtmlCodesToCharacters(cellText);
                        }
                        else
                        {
                            if (m_htTextBoxCalcIndices.ContainsKey(cellCntr))
                            {
                                txtCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                                txtCurrentCell.ID = "txt" + Convert.ToString(m_arrColLabels[cellCntr]);
                                decimal amount;
                                if (Decimal.TryParse(cellText, out amount))
                                {
                                    txtCurrentCell.Text = string.Format("{0:N}", amount);
                                }
                                //ObjCommonUI.m_alCalcTBoxIDS.Add(txtCurrentCell.ClientID);
                                //txtCurrentCell.Attributes.Add("ondblclick", "javascript:ShowCalculator('" + txtCurrentCell.ClientID + "')");
                                //string themeName = Convert.ToString(Session["MyTheme"]);
                                string cdnImagesPath = Convert.ToString(Application["ImagesCDNPath"]);
                                txtCurrentCell.ShowIcon = true;
                                txtCurrentCell.IconAlign = IconAlign.Right;
                                txtCurrentCell.IconOnClick = "javascript:ShowCalculator('" + txtCurrentCell.ClientID + "');";
                                txtCurrentCell.IconAlternateText = "Show Calculator";
                                txtCurrentCell.IconPath =  cdnImagesPath + "images/calculator.png";

                                //on lost focus change amount to decimals 
                                if (!m_IsFindMode)
                                {
                                    txtCurrentCell.Attributes["onblur"] = "FilterAmount(this);";
                                    //geninvoice page new line focus is removed on 15-02-2010 based on client requirement
                                    if ((this.Page.ToString().Contains("journals")) || (this.Page.ToString().Contains("cashjournal")))
                                    {
                                        //Set tab focus new line after amount field
                                        txtCurrentCell.Attributes["onkeydown"] = "SetFocusNextRow(event,this);";
                                    }
                                }

                            }
                        }
                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            txtCurrentCell.Attributes["onblur"] += "ValidateChildControl(this);";
                        }

                        //Update only the rows which were modified.
                        if (AllowPagingInner)
                        {
                            //Calculation of paging amount fields
                            txtCurrentCell.Attributes.Add("OriginalAmount", ObjCommonUI.HtmlCodesToCharacters(cellText));

                            //txtCurrentCell.Attributes["onchange"] += "SetModRowIndices('" + hdnModRows.ClientID + "','" + e.Row.RowIndex + "')";
                            txtCurrentCell.Attributes["onchange"] += "SetModRowIndices('" + hdnModRows.ClientID + "','" + e.Row.RowIndex + "','" + cellCntr + "')";
                        }

                        if (m_htTextBoxIsLink.ContainsKey(cellCntr))
                        {
                            //Add the text of AutoFill as its attribute and change the text to TrxId~TrxType
                            string colLabel = txtCurrentCell.MapXML;
                            string txtValue = txtCurrentCell.Text.Trim();
                            string TrxID = drvCurrent.DataView.Table.Columns.Contains(colLabel + "_TrxID") ? drvCurrent.Row[colLabel + "_TrxID"].ToString() : "";
                            string TrxType = drvCurrent.DataView.Table.Columns.Contains(colLabel + "_TrxType") ? drvCurrent.Row[colLabel + "_TrxType"].ToString() : "";

                            txtCurrentCell.Attributes["AFText"] = txtValue;
                            txtCurrentCell.Attributes["AutoFill"] = "1";
                            if (TrxID.Length > 0 && TrxType.Length > 0)
                            {
                                txtCurrentCell.Text = TrxID + "~" + TrxType;
                            }
                            if (!ObjCommonUI.m_htCGAutoCompleteTBoxIDS.Contains(txtCurrentCell.ClientID))
                            {
                                ObjCommonUI.m_htCGAutoCompleteTBoxIDS.Add(txtCurrentCell.ClientID, Convert.ToString(m_arrColLabels[cellCntr]));
                            }

                            switch (txtCurrentCell.MapXML)
                            {
                                case "AutoFillJob":
                                    {
                                        if (txtCurrentCell.Text != string.Empty)
                                        {
                                            //MODIFY mode or containing text
                                            string Row_JobCOA = string.Empty;
                                            if (drvCurrent.DataView.Table.Columns.Contains("JobCOA"))
                                            {
                                                Row_JobCOA = drvCurrent.Row["JobCOA"].ToString();
                                            }
                                            txtCurrentCell.Attributes.Add("COA", Row_JobCOA);
                                        }
                                        else
                                        {
                                            //ADD mode or empty text
                                            txtCurrentCell.Attributes.Add("COA", hdnJobCOADefault.Value);
                                        }
                                        break;
                                    }
                                case "AutoFillAccount":
                                    {
                                        //Reset COA Attribute for Job/Division when TextBox is empty using javascript.
                                        // Added on 27-11-09
                                        txtCurrentCell.Attributes.Add("onfocus", "javascript:ReSetCOA('" + txtCurrentCell.ClientID.ToString() + "');");
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }



                        }

                        //Normal TextBox with Amount formatting.
                        //Check for Amount;Right-Justify and apply formatting.
                        //if (((DataControlFieldCell)tc).ContainingField.HeaderText == "Amount")
                        if (m_arrAmountCols.Contains(cellCntr))
                        {
                            decimal amount;
                            if (Decimal.TryParse(txtCurrentCell.Text, out amount))
                            {
                                txtCurrentCell.Text = string.Format("{0:N}", amount);
                            }
                            txtCurrentCell.Attributes["IsAmount"] = cellCntr.ToString();// "1";-Changed by Danny on 31/12/2008
                            txtCurrentCell.Style.Add("TEXT-ALIGN", "right");

                            int cellIndex = cellCntr;// (grdVwBranch.Columns[0].Visible) ? cellCntr : cellCntr - 1;
                            txtCurrentCell.Attributes.Add("onkeyup", "javascript:ShowInsantaneousSum('" + gridClientID + "','" + cellIndex + "')");
                            txtCurrentCell.Attributes.Add("onpaste", "javascript:ShowInsantaneousSum('" + gridClientID + "','" + cellIndex + "')");


                            if (!string.IsNullOrEmpty(m_BalanceMethodControl))
                            {
                                string[] split = m_BalanceMethodControl.Split('~');
                                if (m_arrAmountCols.Contains(Convert.ToInt32(split[1])))
                                {
                                    txtCurrentCell.Attributes["onfocus"] = "AutoBalance(this,'" + split[0] + "');"; //+ txtCurrentCell.Attributes["onblur"];
                                }
                            }
                        }
                    }
                    else if (m_htCheckBoxIndices.ContainsKey(cellCntr))
                    {
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        LAjitCheckBox chkCurrentCell = new LAjitCheckBox();
                        chkCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                        chkCurrentCell.ID = "chkBx" + Convert.ToString(m_arrColLabels[cellCntr]);
                        if (cellText == "1")
                        {
                            chkCurrentCell.Checked = true;
                        }

                        if (IsSelectInvoice)
                        {
                            chkCurrentCell.Attributes["RowIndex"] = e.Row.RowIndex.ToString();
                        }

                        //Add the OnGridLoad summation for SelectInvoice page.
                        if (this.Page.ToString().Contains("selectinvoice"))
                        {
                            chkCurrentCell.Attributes.Add("onclick", m_OnGridLoadJSCall.Replace("var", ""));
                        }
                        //Update only the rows which were modified.
                        if (AllowPagingInner)
                        {
                            chkCurrentCell.Attributes["onchange"] += "SetModRowIndices('" + hdnModRows.ClientID + "','" + e.Row.RowIndex + "')";
                        }
                        tc.Controls.Add(chkCurrentCell);
                    }
                    else if (m_htLinkIndices.ContainsKey(cellCntr))
                    {
                        tc.Text = "";
                    }
                    else
                    {
                        //This is a Label
                        LAjitLabel lblCellText = new LAjitLabel();
                        lblCellText.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);

                        lblCellText.BorderStyle = BorderStyle.None;
                        lblCellText.Style.Add("background-color", "Transparent");
                        lblCellText.Style.Add("font-size", "11px");
                        lblCellText.Style.Add("font-family", "Arial, Helvetica, sans-serif");

                        //Check for IsNumeric-apply formatting.
                        if (m_arrAmountCols.Contains(cellCntr))
                        {
                            decimal amount;
                            if (Decimal.TryParse(cellText, out amount))
                            {
                                lblCellText.Text = string.Format("{0:N}", amount);
                                lblCellText.ToolTip = lblCellText.Text;
                            }
                            lblCellText.Attributes["IsAmount"] = "1";
                        }
                        tc.Controls.Add(lblCellText);
                    }
                    cellCntr++;
                }
            }
        }

        /// <summary>
        /// Updates the local copy of branch grid view's XML
        /// </summary>
        /// <param name="strOutXml">The new XML from the DB just after an operation has been performed.</param>
        /// <param name="treeNodeName">TreeNode in the current form XML.</param>
        /// <param name="arrBranches">Array of all the branches.</param>
        public void UpdateGVBranchViewStateXML(string strOutXml, string treeNodeName, ArrayList arrBranches, Panel pnlContainer)
        {
            #region NLog
            logger.Info("Updates the local copy of branch grid view's XML for tree node name : "+treeNodeName);
            #endregion

            string GVDataXML = ObjCommonUI.ButtonsUserControl.GVDataXml;
            //if (GVDataXML.Length == 0)
            //{
            //    GVDataXML = ButtonsUserControl.GVDataXml;
            //}
            XmlDocument xDocLocalCopy = new XmlDocument();
            xDocLocalCopy.LoadXml(GVDataXML);
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(strOutXml);
            foreach (string branchNode in arrBranches)
            {
                //updating default columns in branchxml newly added on 26-08-08
                XmlNode nodeColumnsList = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/GridHeading/Columns");
                if (nodeColumnsList != null)
                {
                    //Method 1 Replace the columns default value
                    XmlNodeList nodeDefaultlist = nodeColumnsList.SelectNodes("Col[@Default]");
                    if (nodeDefaultlist != null)
                    {
                        foreach (XmlNode nodeDefault in nodeDefaultlist)
                        {
                            if (nodeDefault != null)
                            {
                                XmlNode nodeColumn = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns//Col[@Label='" + nodeDefault.Attributes["Label"].Value + "']");
                                if (nodeColumn != null)
                                {
                                    XmlNode nodeColParent = nodeColumn.ParentNode;
                                    if (nodeColParent != null)
                                    {
                                        //nodeColParent.RemoveChild(nodeColumn);
                                        //nodeColParent.InnerXml += nodeDefault.OuterXml;
                                        XmlNode nodeDefClone = xDocLocalCopy.ImportNode(nodeDefault, true);
                                        nodeColParent.ReplaceChild(nodeDefClone, nodeColumn);
                                    }
                                }
                            }
                        }
                    }
                }

                XmlNode nodeRowList = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/RowList");
                if (nodeRowList != null)
                {
                    HiddenField hdnCurrAction = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnCurrAction");
                    bool isModify = (hdnCurrAction != null && hdnCurrAction.Value == "Modify") ? true : false;
                    if (AllowPaging && nodeRowList.ParentNode.Name == this.BranchNodeName && isModify)
                    {
                        string[] arrModRows = hdnModRows.Value.Split(',');
                        foreach (string modRow in arrModRows)
                        {
                            if (modRow.Trim().Length > 0)
                            {
                                int modRowIndex = Convert.ToInt32(modRow) + (grdVwBranch.PageIndex * grdVwBranch.PageSize);
                                XmlNode nodeReturn = nodeRowList.ChildNodes[modRowIndex];
                                if (nodeReturn != null)
                                {
                                    string returnRowTrxID = nodeReturn.Attributes["TrxID"].Value;
                                    string returnRowTrxType = nodeReturn.Attributes["TrxType"].Value;
                                    XmlNode nodeLocal = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList/Rows[@TrxID='" + returnRowTrxID + "' and @TrxType='" + returnRowTrxType + "']");
                                    XmlNode nodeLocalPrev = null;
                                    XmlNode nodeLocalParent = null;
                                    if (nodeLocal != null)
                                    {
                                        //Replace it with the return node.
                                        nodeLocalParent = nodeLocal.ParentNode;
                                        nodeLocalPrev = nodeLocal.PreviousSibling;
                                        nodeLocalParent.RemoveChild(nodeLocal);
                                    }
                                    else
                                    {
                                        nodeLocalParent = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList");
                                    }
                                    XmlNode nodeReturnCopy = xDocLocalCopy.ImportNode(nodeReturn, true);
                                    if (nodeLocalPrev != null)
                                    {
                                        nodeLocalParent.InsertAfter(nodeReturnCopy, nodeLocalPrev);
                                    }
                                    else
                                    {
                                        nodeLocalParent.PrependChild(nodeReturnCopy);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Loop to update added/modified rows.
                        foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
                        {
                            //Get the row's ID
                            string returnRowTrxID = nodeRow.Attributes["TrxID"].Value;
                            string returnRowTrxType = nodeRow.Attributes["TrxType"].Value;

                            //Get the corresponding row in the local copy XML
                            XmlNode nodeLocal = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList/Rows[@TrxID='" + returnRowTrxID + "' and @TrxType='" + returnRowTrxType + "']");
                            if (nodeLocal == null)
                            {
                                //No node exists implies it has been added so add it to the local copy's RowList
                                XmlNode nodeChildRowList = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList");
                                if (nodeChildRowList == null)
                                {
                                    //Brand new addition of both the parent and the child, so create the rowlist node before adding rows.
                                    nodeChildRowList = xDocLocalCopy.CreateElement("RowList");
                                    nodeChildRowList.InnerXml += nodeRow.OuterXml;
                                    xDocLocalCopy.SelectSingleNode("//" + branchNode).AppendChild(nodeChildRowList);
                                }
                                else
                                {
                                    nodeChildRowList.InnerXml += nodeRow.OuterXml;
                                }
                            }
                            else
                            {
                                //Replace the node
                                xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList").RemoveChild(nodeLocal);
                                xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList").InnerXml += nodeRow.OuterXml;
                            }
                        }
                    }

                }
            }

            if (ObjCommonUI.ButtonsUserControl.CurrentAction != "Clone" && ObjCommonUI.ButtonsUserControl.CurrentAction != "Add")
            {
                //A case of delete.
                DataSet dsCurrent = (DataSet)grdVwBranch.DataSource;
                string[] arrDeletions = hdnRowsToDelete.Value.Split(',');
                //Delete the rows in the gridview/ViewState as indicated in the above hidden field.
                int deleteCntr = 0;

                foreach (string strRowIndex in arrDeletions)
                {
                    if (strRowIndex.Length > 0)
                    {
                        int actualIndex = Convert.ToInt32(strRowIndex);
                        if (AllowPaging)
                        {
                            actualIndex = Convert.ToInt32(strRowIndex) + (grdVwBranch.PageIndex * grdVwBranch.PageSize);
                        }
                        DataRow drToDelete = dsCurrent.Tables[0].Rows[actualIndex - deleteCntr];
                        string trxIDToDelete = string.Empty;
                        string trxTypeToDelete = string.Empty;
                        try
                        {
                            trxIDToDelete = drToDelete["TrxID"].ToString();
                            trxTypeToDelete = drToDelete["TrxType"].ToString();
                        }
                        catch (ArgumentException)
                        {
                            //Current row is present at the client-side only(not in DB)
                            continue;
                        }
                        if (trxIDToDelete.Length == 0 && trxTypeToDelete.Length == 0)
                        {
                            //No row to delete or empty row case
                            continue;
                        }
                        //Delete the child rows in the GVDataXML as per the TrxID and TrxType
                        XmlNode nodeBranchRow = xDocLocalCopy.SelectSingleNode("Root/bpeout/FormControls/" + BranchNodeName + "/RowList/Rows[@TrxID='" + trxIDToDelete + "' and @TrxType='" + trxTypeToDelete + "']");
                        XmlNode nodeParent = xDocLocalCopy.SelectSingleNode("Root/bpeout/FormControls/" + BranchNodeName + "/RowList");
                        if (nodeBranchRow != null && nodeParent != null)
                        {
                            nodeParent.RemoveChild(nodeBranchRow);
                        }
                        //Also update the datasource for the child grid view.
                        dsCurrent.Tables[0].Rows.Remove(drToDelete);
                        deleteCntr++;

                        //Also update the checkbox selection attribute of the gridview.
                        grdVwBranch.Attributes["CheckBoxSel"] = grdVwBranch.Attributes["CheckBoxSel"].Replace(strRowIndex + ",", "");
                    }
                }

                if (arrDeletions.Length > 1)
                {
                    //Clear the checkboxes in the grid
                    ClearGridViewSelection(grdVwBranch);
                }
            }

            //Clear the Hidden Field once the deletions are performed.
            hdnRowsToDelete.Value = string.Empty;
            ModifiedRows = string.Empty;
            ObjCommonUI.ButtonsUserControl.GVDataXml = xDocLocalCopy.OuterXml;
            ObjCommonUI.GridViewUserControl.FormTempData = xDocLocalCopy.OuterXml;

            InitialiseBranchGrid(xDocLocalCopy, CommonUI.GetPostBackControl(this.Page));
        }

        /// Gets the Business Process Control node based on the passed process ID.
        /// </summary>
        /// <param name="processName">The Process ID.</param>
        /// <param name="xDocForm">The XML Document containing the BPC node.</param>
        /// <returns>The BPC node.</returns>
        private XmlNode GetBPCNode(string processName, XmlDocument xDocForm)
        {
            #region NLog
            logger.Info("Getting the Business Process Control node based on the passed process ID where process name : "+processName);
            #endregion

            return xDocForm.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
        }

        /// <summary>
        /// Initialises the an attribute of the grid view conisting of the checkbox selection.
        /// Purpose: To maintain checkbox selection across postbacks.
        /// </summary>
        /// <param name="grdVwBranch">GridView Object.</param>
        /// <param name="reset">Resets all the checkboxes to unchecked state.</param>
        private void InitCheckBoxSelection(bool reset)
        {
            StringBuilder sbSelection = new StringBuilder(grdVwBranch.Rows.Count);
            foreach (GridViewRow row in grdVwBranch.Rows)
            {
                CheckBox chkBxSelectRow = (CheckBox)row.Cells[0].FindControl("chkBxSelectRow");
                if (chkBxSelectRow == null)
                {
                    chkBxSelectRow = new CheckBox();
                    row.Cells[0].Controls.Add(chkBxSelectRow);
                    continue;
                }
                if (chkBxSelectRow.Checked)
                {
                    if (!reset)
                    {
                        sbSelection.Append(row.RowIndex.ToString() + ",");
                    }
                }
                else
                {
                    chkBxSelectRow.Checked = false;
                }
            }
            grdVwBranch.Attributes["CheckBoxSel"] = sbSelection.ToString();
        }

        /// <summary>
        /// Gets the table schema from the XML and creates a table with a default number of blank rows.
        /// </summary>
        /// <param name="xDocOut">The form XML.</param>
        /// <param name="branchNodeName">The name of the branch.</param>
        /// <param name="grdVwBranch">Gridview object.</param>
        /// <param name="dsRowList">The parent dataset to bind to the gridview.</param>
        private DataSet GetNewTableSchema(XmlDocument xDocOut, string branchNodeName, DataSet dsRowList)
        {
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
            if (nodeColumns == null)
            {
                return new DataSet();
            }
            DataTable dtAddNew = new DataTable("Rows");
            //Create the schema for the table.
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                DataColumn dcNew = new DataColumn(nodeCol.Attributes["Label"].Value, typeof(string));
                dtAddNew.Columns.Add(dcNew);
            }
            //For fullview page(Reuires single row only).
            if (grdVwBranch.Page.ToString().Contains("fullview"))
            {
                dtAddNew.Rows.Add(dtAddNew.NewRow());
            }
            else
            {
                //Add some rows now.
                int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
                for (int i = 0; i < blockSize; i++)
                {
                    dtAddNew.Rows.Add(dtAddNew.NewRow());
                }
            }
            //Add the table to a dataset.
            if (dsRowList.Tables.Count == 0)
            {
                dsRowList.Tables.Add(dtAddNew);
                dsRowList.AcceptChanges();
            }
            return dsRowList;
        }


        /// <summary>
        /// Creates the xml structure of the given row.
        /// </summary>
        /// <param name="dtGridData">The table which contains the row.</param>
        /// <param name="primaryKey">The unique key to identify the row.</param>
        /// <returns>xml string.</returns>
        private string GetRowXml(DataTable dtGridData, string primaryKey)
        {
            if (!dtGridData.Columns.Contains(m_primaryKeyFieldName))
            {
                return string.Empty;
            }
            DataRow[] drFoundRows = dtGridData.Select(m_primaryKeyFieldName + "='" + primaryKey + "'");
            if (drFoundRows.Length == 0)
            {
                return string.Empty;
            }
            XmlDocument xDocRow = new XmlDocument();
            XmlNode nodeRow = xDocRow.CreateNode(XmlNodeType.Element, dtGridData.TableName, null);
            for (int i = 0; i < dtGridData.Columns.Count; i++)
            {
                XmlAttribute attCurrentCol = xDocRow.CreateAttribute(dtGridData.Columns[i].ColumnName.Trim());
                attCurrentCol.Value = drFoundRows[0][i].ToString();
                nodeRow.Attributes.Append(attCurrentCol);
            }
            return nodeRow.OuterXml;
        }

        /// <summary>
        /// Creates a new DDL object to be added into the grid view cell.
        /// </summary>
        /// <param name="cellText">Specifies the node in the XML from the Items data is found</param>
        /// <param name="rowType">If rowType is specified get only those rows in the DDL Items node whose TrxType matches the 
        ///     specified rowType, else fill all the items.</param>
        private LAjitDropDownList CreateDropDownList(string cellText, string selectedValue, string rowType, string GridRowIndex, string EDDLTrxType)
        {
            LAjitDropDownList ddlGridItem = new LAjitDropDownList();
            ddlGridItem.MapXML = cellText;
            ddlGridItem.Enabled = true;
            XmlDataSource xDS = new XmlDataSource();
            xDS.EnableCaching = false;

            if (ObjCommonUI.ButtonsUserControl != null)
            {
                xDS.Data = ObjCommonUI.ButtonsUserControl.GVDataXml;
            }

            //Add only the items which match the TrxType/RowType
            string xPathFilter = "Root/bpeout/FormControls/" + cellText + "/RowList/Rows";


            //XmlDocument xDoc = new XmlDocument();
            //xDoc.LoadXml(xDS.Data);
            //int i = xDoc.SelectNodes("Root/bpeout/FormControls/" + cellText + "/RowList/Rows[@EDDL_TrxType='7']").Count;

            if (rowType.Length != 0)
            {
                xPathFilter += "[@TrxType='" + rowType + "']";
            }
            else if (EDDLTrxType.Length > 0)
            {
                xPathFilter += "[@EDDL_TrxType='" + EDDLTrxType + "']";
            }
            //else
            //{
            //    xPathFilter = "Root/bpeout/FormControls/" + cellText + "/RowList/Rows";
            //}

            xDS.XPath = xPathFilter;
            ddlGridItem.DataValueField = "DataValueField";
            ddlGridItem.DataTextField = cellText;

            Random num = new Random();
            ddlGridItem.ID = "ddl" + cellText;
            ddlGridItem.DataSource = xDS;
            if (xDS.Data.Trim().Length > 0)
            {
                ddlGridItem.DataBind();
                //Assign the selected value for the DDL.
                if (selectedValue.Length > 0)
                {
                    ddlGridItem.SelectedValue = selectedValue;
                }
                else
                {
                    if (ddlGridItem.Items.Count > 0)
                    {
                        ddlGridItem.SelectedIndex = 0;
                    }
                }
            }

            ////Adding onchage event exclusive for the column EntryJob
            //if (cellText == "EntryJob")
            //{
            //    if (ddlGridItem != null)
            //    {
            //        ddlGridItem.Attributes.Add("onchange", "javascript:FillCascadingDropDowns('" + GridRowIndex + "');");
            //    }
            //}

            return ddlGridItem;
        }

        /// <summary>
        /// Adds a new row to the grid with current assigned data source's schmema
        /// </summary>
        /// <param name="gridView">The Grid view in which the row is to be added.</param>
        public void AddNewRowToGrid()
        {
            //To balance serverside and clientside
            HiddenField hdnCurrAction = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnCurrAction");
            HiddenField hdnSubmitstatus = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnSubmitstatus");
            hdnSubmitstatus.Value = string.Empty;//To hide the success msg.
            //To enable/Change clor/ResetActionButtons/ShowHideFormButtons depending on current action.
            if (hdnCurrAction.Value == "Modify")
            {
                ObjCommonUI.InvokeOnButtonClick("Modify", ObjCommonUI.ButtonsUserControl.Page);
            }
            else
            {
                ObjCommonUI.InvokeOnButtonClick("Clone", ObjCommonUI.ButtonsUserControl.Page);
            }

            //Enable validation for confirm page exit
            ((HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnNeedToConfirmExit")).Value = "True";

            DataSet ds = (DataSet)grdVwBranch.DataSource;
            DataTable dtCurrent = ds.Tables[0];

            //Match the gridview's row data to it's data source
            //Data source may be different from that of the gridview's in the case where the user has
            //entered some information and clicked AddMoreRows
            for (int i = 0; i < grdVwBranch.Rows.Count; i++)
            {
                GridViewRow gvrRow = grdVwBranch.Rows[i];
                for (int j = 1; j < gvrRow.Cells.Count; j++)
                {
                    string dataField = ((BoundField)grdVwBranch.Columns[j]).DataField;
                    foreach (Control ctrl in gvrRow.Cells[j].Controls)
                    {
                        if (ctrl is LAjitTextBox)
                        {
                            LAjitTextBox txtCurrent = (LAjitTextBox)ctrl;
                            //Match if the textbox contents are in the format TrxID~TrxType
                            Match matchAutoFill = Regex.Match(txtCurrent.Text, "[1-9]{1}[0-9]{0,}[~]{1}[1-9]{1}[0-9]{0,}");
                            if (matchAutoFill.Success)
                            //if (txtCurrent.Text.Contains("~"))
                            {
                                //AutoFill Cloning functionality.
                                string cloneId = txtCurrent.ClientID.Replace("ctl00_cphPageContents_", "").Replace("_", "$");
                                string AFText = Page.Request[cloneId];
                                string[] arrTrxIDType = txtCurrent.Text.Split('~');
                                //AutoFill No Caching funtionality.
                                string TrxIDCol = dataField + "_TrxID";
                                string TrxTypeCol = dataField + "_TrxType";
                                //Create the columns if not present
                                if (!dtCurrent.Columns.Contains(TrxIDCol))
                                {
                                    DataColumn dc = new DataColumn(TrxIDCol, typeof(string));
                                    dtCurrent.Columns.Add(dc);
                                }
                                if (!dtCurrent.Columns.Contains(TrxTypeCol))
                                {
                                    DataColumn dc = new DataColumn(TrxTypeCol, typeof(string));
                                    dtCurrent.Columns.Add(dc);
                                }
                                dtCurrent.Rows[i][dataField] = AFText;
                                dtCurrent.Rows[i][TrxIDCol] = arrTrxIDType[0];
                                dtCurrent.Rows[i][TrxTypeCol] = arrTrxIDType[1];
                            }
                            else
                            {
                                dtCurrent.Rows[i][dataField] = txtCurrent.Text;
                            }

                        }
                        else if (ctrl is LAjitDropDownList)
                        {
                            LAjitDropDownList ddlCurrent = (LAjitDropDownList)ctrl;
                            string mapXML = ddlCurrent.MapXML;
                            dtCurrent.Rows[i][mapXML] = ddlCurrent.SelectedValue;
                        }
                        else if (ctrl is LAjitCheckBox)
                        {
                            LAjitCheckBox chkBxCurrent = (LAjitCheckBox)ctrl;
                            string mapXML = chkBxCurrent.MapXML;
                            if (chkBxCurrent.Checked)
                            {
                                dtCurrent.Rows[i][mapXML] = "1";
                            }
                            else
                            {
                                dtCurrent.Rows[i][mapXML] = "0";
                            }
                        }
                    }
                    //dtCurrent.Rows[i].ItemArray[dtCurrent.Columns[dataField].Ordinal] = ;
                }
            }
            dtCurrent.AcceptChanges();

            //int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
            //for (int i = 0; i < blockSize; i++)
            //{
            //    GridViewRow gvrNew = new GridViewRow(gridView.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            //    gvrNew.Cells.AddRange(CreateTableCells(gridView));
            //    Table tblChild = gridView.Controls[0] as Table;
            //    tblChild.Rows.Add(gvrNew);
            //}

            int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
            for (int i = 0; i < blockSize; i++)
            {
                DataRow drNewRow = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(drNewRow);
            }
            ds.AcceptChanges();
            //If Paging is enabled set the pageindex to the last page
            bool isPaging = false;
            if (this.AllowPaging)
            {
                int rowCount = ds.Tables[0].Rows.Count;//Get the latest value of RowCount from the DataSource
                int lastPageIndex = Convert.ToInt32(Math.Ceiling((double)rowCount / grdVwBranch.PageSize));
                grdVwBranch.PageIndex = lastPageIndex - 1;// grdVwBranch.PageCount - 1;
                isPaging = true;
                if (ObjCommonUI.ButtonsUserControl.CurrentAction == "Add")
                {
                    //Disable the paging temporarily when adding more records in ADD mode.
                    this.AllowPaging = false;
                    grdVwBranch.PageIndex = 0;
                }
            }
            grdVwBranch.DataSource = ds;
            grdVwBranch.DataBind();

            //Reset the Paging property to the earlier value.
            this.AllowPaging = isPaging;

            //Get the hidden field with the info about rows to delete
            string[] arrDeletions = hdnRowsToDelete.Value.Split(',');
            foreach (string strRowIndex in arrDeletions)
            {
                if (strRowIndex.Trim().Length == 0)
                {
                    continue;
                }
                int rowIndex = Convert.ToInt32(strRowIndex.Trim());
                grdVwBranch.Rows[rowIndex].Visible = false;
            }

            //Set the RowCount variable accordingly
            RowsInDisplay = ds.Tables[0].Rows.Count;// -arrDeletions.Length + 1;

            //If the data source consists of more than specified records implement vertical scrolling.
            int maxRowSize = Convert.ToInt32(ConfigurationManager.AppSettings["BranchGVMaxRows"]);
            if (ds.Tables[0].Rows.Count - (arrDeletions.Length - 1) > maxRowSize)
            {
                //Find the immediate parent container panel of the grid view 
                if (!pnlGVBranch.Page.ToString().Contains("commercial") && !IsSelectInvoice && !pnlGVBranch.Page.ToString().Contains("arinvdetail"))
                {
                    pnlGVBranch.Style.Remove("height");
                    //pnlGVBranch.Height = Unit.Pixel(230);
                    pnlGVBranch.Height = Unit.Pixel(186);
                    pnlGVBranch.ScrollBars = ScrollBars.Both;
                }
            }
            else
            {
                //pnlGVBranch.Style.Add("height", "auto");
                pnlGVBranch.Height = Unit.Pixel(186);
                pnlGVBranch.ScrollBars = ScrollBars.Horizontal;
            }
        }

        private TableCell[] CreateTableCells(GridView gridView)
        {
            TableCell[] rowCells = new TableCell[gridView.Columns.Count];
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                rowCells[i] = new TableCell();
                foreach (Control ctrl in gridView.Rows[0].Cells[i].Controls)
                {
                    if (ctrl is LAjitTextBox)
                    {
                        LAjitTextBox txtReference = (LAjitTextBox)ctrl;
                        LAjitTextBox txtCurrent = new LAjitTextBox();
                        txtCurrent.ID = txtReference.ID;
                        txtCurrent.Width = txtReference.Width;
                        rowCells[i].Controls.Add(txtCurrent);
                    }
                    else if (ctrl is CheckBox)
                    {
                        CheckBox chkBxReference = (CheckBox)ctrl;
                        CheckBox chkBxCurrent = new CheckBox();
                        chkBxCurrent.ID = chkBxReference.ID;
                        rowCells[i].Controls.Add(chkBxCurrent);
                    }
                }
            }
            return rowCells;
        }

        /// <summary>
        /// Builds the XML for the Grid View contents.
        /// </summary>
        /// <param name="pnlContainer">The Grid View Container.</param>
        /// <param name="formXML">The form XML for the current page.</param>
        /// <returns>XML string.</returns>
        public string GetGridViewXML(Panel pnlContainer, string formXML, string parentTrxID, string parentTrxType, string BPAction)
        {
            #region NLog
            logger.Info("Builds the XML for the Grid View contents with parent trx id : "+parentTrxID+" and parent trx type : "+parentTrxType+" and BP Action : "+BPAction);
            #endregion

            XmlDocument xDocForm = new XmlDocument();
            xDocForm.LoadXml(formXML);
            //Get the root tree name.
            string treeNodeName = ObjCommonUI.ButtonsUserControl.TreeNode;
            //Get the branch nodes of the above tree.
            XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            //If there are no branches nothing to do just return.
            if (nodeBranches == null)
            {
                return string.Empty;
            }
            //Trim as required
            parentTrxID = parentTrxID.Trim();
            parentTrxType = parentTrxType.Trim();
            string parentIDString = string.Empty;

            //Build the XML string
            StringBuilder sbGridViewXML = new StringBuilder();
            string branchNodeName = BranchNodeName;
            XmlNode nodeBranch = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName);
            XmlNode nodeBranchColumns = nodeBranch.SelectSingleNode("GridHeading/Columns");
            XmlNode nodeRowList = nodeBranch.SelectSingleNode("RowList");
            GridView gridView = grdVwBranch;
            DataSet dsCurrent = (DataSet)gridView.DataSource;
            sbGridViewXML.Append("<" + branchNodeName + "><RowList>");

            //Get the hidden field with the info about rows to delete
            string[] arrDeletions = hdnRowsToDelete.Value.Split(',');
            string[] arrModifications = hdnModRows.Value.Split(',');

            int rowCntr = 0;
            if (AllowPaging)
            {
                rowCntr = grdVwBranch.PageIndex * grdVwBranch.PageSize;
            }

            //Init a Column Dictionary container.(Defaults)
            Dictionary<string, LAjitColumn> dicColumns = new Dictionary<string, LAjitColumn>();
            for (int i = 0; i < nodeBranchColumns.ChildNodes.Count; i++)
            {
                XmlNode nodeCol = nodeBranchColumns.ChildNodes[i];
                string label = nodeCol.Attributes["Label"].Value;
                XmlAttribute att;
                //att = nodeCol.Attributes["Default"];
                //string defValue = att != null ? att.Value.Trim() : "";
                att = nodeCol.Attributes["IsLink"];
                bool isLink = (att != null && att.Value == "1") ? true : false;
                att = nodeCol.Attributes["ControlType"];
                if (att != null)//If control type is present.
                {
                    LAjitColumn col = new LAjitColumn();
                    col.ColNode = nodeCol;
                    //col.Default = defValue;
                    col.IsLink = isLink;
                    col.ControlType = att.Value;
                    dicColumns.Add(label, col);
                }
            }

            foreach (GridViewRow row in gridView.Rows)
            {
                //Get the TrxID and TrxType of the current row from the DataSource.
                string trxID = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[0].ToString().Trim();
                string trxType = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[1].ToString().Trim();
                int rVerIndex = GetColumnIndex("RVer", dsCurrent.Tables[0]);
                parentIDString = treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\" ";
                string rowVersion = string.Empty;
                bool isRowDeleted = false;//Indicates that row is meant for deletion.

                if (rVerIndex != -1)
                {
                    rowVersion = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[rVerIndex].ToString();
                }

                //Check whether current row is in RowsToDelete List
                foreach (string strRowIndex in arrDeletions)
                {
                    if (strRowIndex == row.RowIndex.ToString())
                    {
                        isRowDeleted = true;
                        break;
                    }
                }

                if (this.AllowPaging)
                {
                    //If Paging is ON then consider only the modified rows to form the return XML;
                    //Check if current row is in the Modified Rows List
                    bool found = false;
                    foreach (string rowIndex in arrModifications)
                    {
                        if (rowIndex == row.RowIndex.ToString())
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found && !isRowDeleted)
                    {
                        rowCntr++;
                        continue;
                    }
                }

                //Set the BPAction according to the operation being performed
                string actionLocal = BPAction;
                if (trxID.Length == 0 && trxType.Length == 0)
                {
                    if (isRowDeleted)
                    {
                        continue;//User has entered a new row but deleted it without submitting it to the server.
                    }
                    if (!m_IsFindMode)
                    {
                        actionLocal = "Add";
                    }
                    if (parentTrxID.Length != 0)
                    {
                        //Parent already exists and a new child is being added.
                        sbGridViewXML.Append("<Rows " + treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\"  ");
                    }
                    else
                    {
                        //Both parent and child are being added
                        sbGridViewXML.Append("<Rows ");
                    }
                }
                else
                {
                    sbGridViewXML.Append("<Rows " + treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\"  TrxID=\"" + trxID + "\" TrxType=\"" + trxType + "\" RVer=\"" + rowVersion + "\" ");
                }

                if (isRowDeleted)
                {
                    actionLocal = "Delete";
                }

                //ResetColumns(dicColumns);//Defaults functionality

                bool isRowOk = false;//Describes that the row contains some data rather than being empty.
                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Controls.Count == 0)
                    {
                        //Implies the cell is IsDisplayOnly, no modifications made, so
                        continue;
                    }
                    Control currentCellCtrl = cell.Controls[0];
                    if (currentCellCtrl is LAjitTextBox)
                    {
                        LAjitTextBox txtCurrent = ((LAjitTextBox)currentCellCtrl);
                        string mapXML = txtCurrent.MapXML.Trim();
                        LAjitColumn lcCol = dicColumns[mapXML];
                        bool isLink = lcCol.IsLink;

                        if (txtCurrent.Text.Trim().Length == 0 && !isLink)
                        {
                            continue;
                        }
                        //Check for Amount.If it is so remove the formatting such as commas etc.,
                        if (txtCurrent.Attributes["IsAmount"] != null)
                        {
                            //Check it commas are present.
                            if (txtCurrent.Text.Contains(","))
                            {
                                txtCurrent.Text = txtCurrent.Text.Replace(",", "");
                            }
                        }

                        if (!isLink)
                        {
                            if (txtCurrent.Text != string.Empty)
                            {
                                sbGridViewXML.Append(mapXML + "=\"" + ObjCommonUI.CharactersToHtmlCodes(txtCurrent.Text.Trim()) + "\" ");
                                //lcCol.HasAddedFromUI = true;
                                isRowOk = true;
                            }
                        }
                        else //AutoFills
                        {
                            string[] autofillTrxIDS = txtCurrent.Text.ToString().Split('~');
                            if (autofillTrxIDS.Length == 2)
                            {
                                string afRow = mapXML + "_TrxID=\"" + autofillTrxIDS[0] + "\"  "
                                             + mapXML + "_TrxType=\"" + autofillTrxIDS[1] + "\"  ";
                                sbGridViewXML.Append(afRow);
                                //lcCol.HasAddedFromUI = true;
                                isRowOk = true;
                            }
                        }
                    }
                    else if (currentCellCtrl is LAjitDropDownList)
                    {
                        LAjitDropDownList ddlCurrent = (LAjitDropDownList)currentCellCtrl;
                        string newTrxIDType = ddlCurrent.SelectedValue.Trim();
                        string ddlRow = string.Empty;
                        if (newTrxIDType.Length == 0)
                        {
                            continue;
                        }
                        string[] strarr = newTrxIDType.Split('~');
                        string ddlTrxID = strarr[0].ToString();
                        string ddlTrxType = strarr[1].ToString();
                        string mapXML = ddlCurrent.MapXML.Trim();
                        //LAjitColumn lcCol = dicColumns[mapXML];
                        if (ddlTrxID.Length > 0 && ddlTrxType.Length > 0)
                        {
                            //TrxID should not -1
                            if (ddlTrxID != "-1")
                            {
                                ddlRow = mapXML + "_TrxID=\"" + ddlTrxID + "\"  "
                                        + mapXML + "_TrxType=\"" + ddlTrxType + "\"  ";
                            }
                        }
                        else
                        {
                            ddlRow = mapXML + "=\"" + ObjCommonUI.CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.Trim().ToString()) + "\" ";
                        }

                        if (ddlRow.Length > 0)
                        {
                            sbGridViewXML.Append(ddlRow);
                            //lcCol.HasAddedFromUI = true;
                            isRowOk = true;
                        }
                    }
                    else if (currentCellCtrl is LAjitCheckBox)
                    {
                        LAjitCheckBox chkCurrent = (LAjitCheckBox)currentCellCtrl;
                        string mapXML = chkCurrent.MapXML.Trim();
                        //LAjitColumn lcCol = dicColumns[mapXML];
                        if (chkCurrent.Checked)
                        {
                            //True
                            sbGridViewXML.Append(mapXML + "=\"1\" ");
                            //lcCol.HasAddedFromUI = true;
                            isRowOk = true;
                        }
                        else
                        {   //False
                            sbGridViewXML.Append(mapXML + "=\"0\" ");
                        }
                        #region SelectInvoice Customisation
                        if (IsSelectInvoice && !isRowDeleted)//Generate XML for only modified rows if SelectInvoice.
                        {
                            if (nodeRowList != null && nodeRowList.ChildNodes.Count > 0)
                            {
                                XmlNode nodeRow = nodeRowList.ChildNodes[rowCntr];
                                XmlNode nodeRowCheck = nodeRowList.SelectSingleNode("//RowList/Rows[@TrxID='" + trxID + "' and @TrxType='" + trxType + "']");
                                if (nodeRow != nodeRowCheck)
                                {
                                    throw new Exception("Nodes not matching!!");
                                }
                                if (nodeRow != null)
                                {
                                    string rowSelectItValue = nodeRow.Attributes["Selectit"].Value;
                                    bool origSelection = rowSelectItValue == "1" ? true : false;
                                    if (chkCurrent.Checked == origSelection)
                                    {
                                        //No modification was made so ignore the row.
                                        isRowOk = false;
                                        break;
                                    }
                                }
                                //else
                                //{
                                //    //If nodeRow is null it implies it is a new row being added.But for SelectInvoice all the childrows are readonly
                                //    //Code functionality in response to Bug No : 33816
                                //    isRowOk = false;
                                //}
                            }
                        }
                        #endregion
                    }
                }
                if (!isRowOk && !isRowDeleted)
                {
                    int strLength = sbGridViewXML.Length;
                    int startIndex = sbGridViewXML.ToString().LastIndexOf("<");
                    sbGridViewXML.Remove(startIndex, strLength - startIndex);
                    rowCntr++;
                    continue;
                }

                //if (isRowOk && BPAction == "Add")//Add the Defaults
                //{
                //    foreach (KeyValuePair<string, LAjitColumn> kvp in dicColumns)
                //    {
                //        string colLabel = kvp.Key;
                //        LAjitColumn colDef = kvp.Value;
                //        if (!colDef.HasAddedFromUI && colDef.Default.Length > 0)
                //        {
                //            if (colDef.IsLink)//DDLs & AutoFills
                //            {
                //                string trxIdName = colLabel + "_TrxID";
                //                string trxTypeName = colLabel + "_TrxType";
                //                string[] idSplit = colDef.Default.Split('~');
                //                sbGridViewXML.AppendFormat("{0}=\"{1}\" ", trxIdName, idSplit[0]);
                //                sbGridViewXML.AppendFormat("{0}=\"{1}\" ", trxTypeName, idSplit[1]);
                //            }
                //            else if (colDef.ControlType == "TBox")//TextBoxes
                //            {
                //                sbGridViewXML.AppendFormat("{0}=\"{1}\" ", colLabel, colDef.Default);
                //            }
                //        }
                //    }
                //}
                sbGridViewXML.Append("BPAction=\"" + actionLocal + "\" />");
                rowCntr++;
            }
            sbGridViewXML.Append("</RowList></" + branchNodeName + ">");
            return sbGridViewXML.ToString();
        }

        /// <summary>
        /// Resets the HasColAddedFromUI property to false so that it does not affect the following rows.
        /// </summary>
        /// <param name="dicColumns">Dictionary of Columns.</param>
        private void ResetColumns(Dictionary<string, LAjitColumn> dicColumns)
        {
            foreach (KeyValuePair<string, LAjitColumn> kvp in dicColumns)
            {
                kvp.Value.HasAddedFromUI = false;
            }
        }

        /// <summary>
        /// Finds the child objects in a page and explicitly enables/disables as required.
        /// </summary>
        /// <param name="pnlEntryForm">Panel container.</param>
        public void ReEnableChildObject(string gridMode)
        {
            #region NLog
            logger.Info("Finds the child objects in a page and explicitly enables/disables as required where mode : "+gridMode);
            #endregion

            //If DB operation has failed then keep the child gridview(if present) enabled.
            Color gridColor = Color.Empty;
            if (gridMode == "FIND")
            {
                gridColor = Color.LightGoldenrodYellow;
            }
            else
            {
                gridColor = Color.White;
            }

            //Clear the Rows to delete selection in this case.
            //Let the user reselect the deletions to be made.

            //Before deleting the selection check if the flow is ConfirmSubmit
            //i.e if ConfirmSubmit script has been registered don't clear the selection
            if (this.ClearDeletions)
            {
                hdnRowsToDelete.Value = string.Empty;
            }

            foreach (GridViewRow gvr in grdVwBranch.Rows)
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
                            LAjitTextBox txtCurrent = (LAjitTextBox)cellControl;
                            //Maintain the text contents of the Autofill textboxes by setting the AFText attribute 
                            //of the original element.
                            if (txtCurrent.Attributes["AutoFill"] == "1")
                            {
                                //Check if there is any clone element's POST info available.
                                string cloneId = txtCurrent.ClientID.Replace("ctl00_cphPageContents_", "").Replace("_", "$");
                                string AFText = Page.Request[cloneId];
                                if (!string.IsNullOrEmpty(AFText))
                                {
                                    txtCurrent.Attributes["AFText"] = AFText;
                                }
                            }
                            txtCurrent.BackColor = gridColor;
                        }
                    }
                }
            }
            //Handled in the Client-side.
            ////Enable the operations link buttons explicitly
            //tblOpLinks.Enabled = true;
            ////Disable the BPC links explicitly
            //tblBPCLinks.Enabled = false;
        }

        /// <summary>
        /// Sets the Gridview wrapper panel's width.
        /// </summary>
        /// <param name="width">Integer.</param>
        public void SetPanelWidth(int width)
        {
            pnlGVBranch.Width = Unit.Pixel(width - 2);//-2 for avoiding overflow.
        }

        private string GetAddedRowsXML(string parentIDString, string branchNodeName, GridViewRow gridViewRow)
        {
            //Get the hidden field with the info about rows to delete
            string[] arrDelimiters = { "||" };
            string[] arrRowAdditions = hdnAddedRows.Value.Split(arrDelimiters, StringSplitOptions.RemoveEmptyEntries);
            //Infer the schema
            ArrayList arrSchema = new ArrayList();
            foreach (TableCell tc in gridViewRow.Cells)
            {
                int x = tc.Controls.Count;
                Control currentCellCtrl = tc.Controls[0];
                if (currentCellCtrl is LAjitTextBox)
                {
                    LAjitTextBox txtCurrent = ((LAjitTextBox)currentCellCtrl);
                    arrSchema.Add(txtCurrent.MapXML);
                }
                else if (currentCellCtrl is LAjitDropDownList)
                {
                    LAjitDropDownList ddlCurrent = (LAjitDropDownList)currentCellCtrl;
                    arrSchema.Add(ddlCurrent.MapXML);
                }
                else if (currentCellCtrl is LAjitCheckBox)
                {
                    LAjitCheckBox chkCurrent = (LAjitCheckBox)currentCellCtrl;
                    arrSchema.Add(chkCurrent.MapXML);
                }
            }
            StringBuilder sbAddedRows = new StringBuilder();
            foreach (string newRow in arrRowAdditions)
            {
                sbAddedRows.Append("<Rows " + parentIDString);
                string[] rowDelimiter = { "~::~" };
                string[] cellContents = newRow.Split(rowDelimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cellItem in cellContents)
                {
                    string[] cellDelimiter = { "*#*" };
                    string[] cellValues = cellItem.Split(cellDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    string currentCellXML = arrSchema[Convert.ToInt32(cellValues[0])].ToString() + "=\"" + cellValues[1] + "\" ";
                    sbAddedRows.Append(currentCellXML);
                }
                sbAddedRows.Append("BPAction=\"Add\"/>");
            }

            return sbAddedRows.ToString();
        }

        /// <summary>
        /// Clears the checkbox selections in the grid view.
        /// </summary>
        /// <param name="gridView"></param>
        private static void ClearGridViewSelection(GridView gridView)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chkBxSelectRow = (CheckBox)row.Cells[0].FindControl("chkBxSelectRow");
                if (chkBxSelectRow.Checked)
                {
                    chkBxSelectRow.Checked = false;
                }
            }
        }

        /// <summary>
        /// Gets the position of the column in the given data table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="dtSearch">Data table to search in.</param>
        /// <returns>Integer column position, zero being the first column.</returns>
        /// <returns>Returns -1 if column is not found.</returns>
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
            //throw new Exception("Specified column" + columnName + " not found in the data source!!");
        }

        protected void lnkBtnAddNewRow_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void grdVwBranch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0 && e.NewPageIndex < grdVwBranch.PageCount)
            {
                grdVwBranch.PageIndex = e.NewPageIndex;
            }

            BindUserControl((Control)sender);

            //Do the needful client-side enabling etc.,
            HiddenField hdnCurrAction = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnCurrAction");
            if (hdnCurrAction != null)
            {
                //if (hdnCurrAction.Value == "Add" && AllowPaging)
                //{
                //    pnlGVBranch.Height = Unit.Pixel(230);
                //    pnlGVBranch.ScrollBars = ScrollBars.Both;
                //}
                string action = (hdnCurrAction.Value.Trim().Length == 0) ? "Select" : hdnCurrAction.Value;
                ObjCommonUI.InvokeOnButtonClick(action, this.Page);
            }
        }

        /// <summary>
        /// Paging Controls Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBtnFirst_Click(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);


            grdVwBranch.PageIndex = 0;
            BindUserControl((Control)sender);
        }

        /// <summary>
        /// Paging Controls Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBtnPrev_Click(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);
            if (grdVwBranch.PageIndex != 0)
            {
                grdVwBranch.PageIndex--;
            }
            BindUserControl((Control)sender);
        }

        /// <summary>
        /// Paging Controls Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBtnNext_Click(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);

            if (grdVwBranch.PageIndex != grdVwBranch.PageCount)
            {
                grdVwBranch.PageIndex++;
            }
            BindUserControl((Control)sender);
        }

        /// <summary>
        /// Paging Controls Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBtnLast_Click(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);
            grdVwBranch.PageIndex = grdVwBranch.PageCount;
            BindUserControl((Control)sender);
        }
    }

    public class LAjitColumn
    {
        private XmlNode m_ColNode;

        public XmlNode ColNode
        {
            get { return m_ColNode; }
            set { m_ColNode = value; }
        }

        private bool m_HasAddedFromUI;

        public bool HasAddedFromUI
        {
            get { return m_HasAddedFromUI; }
            set { m_HasAddedFromUI = value; }
        }

        private string m_Default;

        public string Default
        {
            get { return m_Default; }
            set { m_Default = value; }
        }

        private bool m_IsLink;

        public bool IsLink
        {
            get { return m_IsLink; }
            set { m_IsLink = value; }
        }


        private string m_ControlType;

        public string ControlType
        {
            get { return m_ControlType; }
            set { m_ControlType = value; }
        }


    }
}