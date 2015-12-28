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
using System.Drawing;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using Gios.Pdf;
using LAjit_BO;
using LAjitControls;
using NLog;


namespace LAjitDev
{
    public enum GridViewType
    {
        /// <summary>
        /// The most default settings will be used.
        /// </summary>
        Default,
        /// <summary>
        /// Gridview customised for DashBoard i.e with the Header Title.
        /// </summary>
        DashBoard,
        /// <summary>
        /// Gridview customised for COA Page.
        /// </summary>
        COA,
        /// <summary>
        /// Gridview customised for AccountingLayout i.e no Header Title.
        /// </summary>
        AccountingLayout,
        /// <summary>
        /// To be used in the Master Page.
        /// </summary>
        Master
    };

    public partial class GridViewControl : System.Web.UI.UserControl
    {
        #region Private Field Variables
        CommonUI commonObjUI = new CommonUI();
        private const int m_NoOfGVStaticColumns = 3;
        private int m_CurrPageNo;
        private int m_MaxPages;
        private int m_IndexOfIsProtected;
        private int m_IndexOfIsActive;
        private int m_IndexOfTypeOfInactiveStatusID;
        private bool m_IsFirstRun = true;
        bool m_IsRowAlternating = false;
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

        private LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        private CommonUI m_ObjCommonUI = new CommonUI();
        protected LAjitDev.UserControls.ButtonsUserControl ucButtonsUserControl;
        XmlDocument XDocUserInfo = new XmlDocument();
        #endregion
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

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
                if (m_GridInitData == null || m_GridInitData.Length == 0)
                {
                    Control cntrlBtnsUC = this.Parent.NamingContainer.FindControl("BtnsUC");
                    if (cntrlBtnsUC != null)
                    {
                        m_GridInitData = ((UserControls.ButtonsUserControl)cntrlBtnsUC).GVDataXml;
                    }
                    else
                    {
                        m_GridInitData = Convert.ToString(ViewState["GridViewInitData"]);
                    }
                }
                return m_GridInitData;
            }
            set { m_GridInitData = value; }
        }

        public string DefaultPageSize
        {
            get { return hdnDefaultPageSize.Value; }
            set { hdnDefaultPageSize.Value = value; }
        }

        public string GridViewBPInfo
        {
            get
            {
                //return ViewState["GridBPEInfo"].ToString();
                return hdnSearchBPInfo.Value;
            }
            set
            {
                //ViewState["GridBPEInfo"] = value;
                hdnSearchBPInfo.Value = value;
            }
        }

        public GridViewType GridViewType
        {
            set { ViewState["GridViewType"] = value; }
            get { return (GridViewType)ViewState["GridViewType"]; }
        }

        /// <summary>
        /// Spicified whether the Quick Search 
        /// </summary>
        public bool IsFindEnabled
        {
            get { return (hdnFindEnabled.Value != "1") ? false : true; }
            set { hdnFindEnabled.Value = (value) ? "1" : "0"; }
        }

        public string ShowViewerInPopUp
        {
            get { return hdnPreviewInPopup.Value; }
            set { hdnFindEnabled.Value = value; }
        }

        //For storing response xml.
        public string FormTempData
        {
            get
            {
                if (ViewState["FormTempData"] == null)
                {
                    ViewState["FormTempData"] = string.Empty;
                }
                return ViewState["FormTempData"].ToString();
            }
            set { ViewState["FormTempData"] = value; }
        }

        #endregion

        //public override bool Visible
        //{
        //    get
        //    {
        //        if (ViewState["Visible"] != null)
        //        {
        //            return Convert.ToBoolean(ViewState["Visible"]);
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    set
        //    {
        //        ViewState["Visible"] = value;
        //        pnlWrapper.Visible = value;
        //    }
        //}

        /// <summary>
        /// Page Load event of the Container Page
        /// </summary>
        /// <param name="sender">Caller</param>
        /// <param name="e">Event Arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Control ctrlPostBack = CommonUI.GetPostBackControl(Page);

            //string postBackCtrlID = CommonUI.GetPostBackControlID(Page);

            ////Test Code Block
            //if (ctrlPostBack != null)
            //{
            //    if (ctrlPostBack.ID != postBackCtrlID.Replace('$', '_'))
            //    {
            //        Classes.ErrorLogger.LogError(
            //            string.Format("PostBack Control ID's are different,Original ID:{0} New:{1}", ctrlPostBack.ID
            //        , postBackCtrlID.Replace('$', '_')), Classes.LogType.LogFile);

            //    }
            //}
            ////Test Code Block End


            //if (Page.IsPostBack && hdnMainViewState.Value == "1")
            //{
            //    if (ctrlPostBack != null)
            //    {
            //        if (ctrlPostBack.NamingContainer != this && !ctrlPostBack.ClientID.Contains("grdVw"))
            //        {
            //            this.DataBind();
            //        }
            //    }
            //}

            #region NLog
            logger.Debug("Now in GridViewControl PageLoad"); 
            #endregion

            //Due to the dynamic nature of the find row it needs to be rendered again in the click event of the Find button in order to
            //retreive the entered values in the Search boxes.
            if (ctrlPostBack != null && ctrlPostBack.ID != null && ctrlPostBack.ID.Contains("imgBtnQuickSearch"))
            {
                ShowFindRow();
            }

            if (!Page.IsPostBack)
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                if (hdnCSFindVisible.Value.Length == 0)
                {//GridSearchVisible
                    hdnCSFindVisible.Value = GetUserPreferenceValue("437");
                }

                //load grid images
                PreloadImages();

                //if (this.GridViewType == GridViewType.DashBoard)
                //{
                //    IsFindEnabled = false;
                //}
                //else
                //{
                //    IsFindEnabled = true;
                //}
            }
        }

        private void PreloadImages()
        {

            if (grdVwContents.HeaderRow != null)
            {
                ImageButton ibtnserach = (ImageButton)grdVwContents.HeaderRow.FindControl("imgBtnQuickSearch");

                if (ibtnserach != null)
                {
                    ibtnserach.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-find-icon.png";
                }
            }
            imgBtnPrint.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-print.png";

            imgBtnChart.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-chart.png";

            imgBtnPDF.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-pdf.png";

            imgBtnExcel.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-excel.png";

            imgBtnHtml.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-html.png";

            imgBtnMSWord.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Grid_Word.png";

            imgBtnEmailPDF.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/email_icon2.png";
        }


        private void LoadQuickSearchImage()
        {
            if (grdVwContents.HeaderRow != null)
            {
                ImageButton ibtnserach = (ImageButton)grdVwContents.HeaderRow.FindControl("imgBtnQuickSearch");

                if (ibtnserach != null)
                {
                    ibtnserach.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-find-icon.png";
                }
            }
        }

        /// <summary>
        /// Binds the first set of data to the user control without hitting the BPE.
        /// </summary>
        public override void DataBind()
        {
            #region NLog
            logger.Info("Binding the Gridview User control.");
            #endregion

            LoadGVType();
            ViewState["SORTDIRECTION"] = "ASC";
            ViewState["SORTEXP"] = "";

            hdnPreviewInPopup.Value = "0";//TODO:Get it from web.config.
            if (GridViewInitData == null)
            {
                if (this.Parent.NamingContainer.ToString() == "ASP.masterpages_topleft_master")
                {
                    return;
                }

                //Case of the postback other than from within the grid
                //Check for presence of BtnUC, if present use it's view state
                Control cntrlBtnsUC = this.Parent.NamingContainer.FindControl("BtnsUC");
                if (cntrlBtnsUC != null)
                {
                    GridViewInitData = ((UserControls.ButtonsUserControl)cntrlBtnsUC).GVDataXml;
                }
                else
                {
                    GridViewInitData = ViewState["GridViewInitData"].ToString();
                }
            }
            //Getting the returnXml from the BPE which also consists of the gridview contents.
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(GridViewInitData);

            //In Find if search returns no results..
            XmlNode nodeMsgStatus = xDocReturn.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeMsgStatus.InnerText != "Success")
            {
                #region NLog
                logger.Debug("The Search has returned no results. So binding an empty datatable."); 
                #endregion

                grdVwContents.DataSource = new DataTable();
                grdVwContents.DataBind();
                pnlPagingCtrls.Visible = false;
                return;
            }

            //Set the hdnMasterBPIn value with request when the page has first loaded
            if (!Page.IsPostBack)
            {
                hdnMasterBPIn.Value = GridViewBPInfo;
            }

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeColumns = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

            #region NLog
            logger.Debug("GridView Control Tree Node Name : " + m_GVTreeNodeName); 
            #endregion

            //Setting the variables for calculation of widths of the columns.
            uint colFVWidths = new uint();
            int gridColumnCount = new int();
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                uint colFullViewLength = Convert.ToUInt32(nodeCol.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)
                {
                    gridColumnCount++;
                    colFVWidths += colFullViewLength;
                }
            }

            int gridTotalUsableWidth = 95;//In percentage - 5% of the width is allocated to the View Column in the beginning
            //int gridColumnCount = nodeColumns.ChildNodes.Count - nodeColumns.SelectNodes("Col[@FullViewLength='0']").Count;
            int percentWidth = 0;
            int gridColWidth = 20;//In percentage
            if (gridColumnCount != 0)
            {
                percentWidth = Convert.ToInt32(System.Math.Ceiling((double)gridTotalUsableWidth / (double)gridColumnCount));
            }
            int colCntr = 0;
            //Add the columns only if no columns are present.
            if (grdVwContents.Columns.Count == m_NoOfGVStaticColumns)
            {
                //Creating the Columns.
                foreach (XmlNode colNode in nodeColumns.ChildNodes)
                {
                    //Add the column only if FullViewLength is not equal to zero
                    int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                    if (colFullViewLength != 0)
                    {
                        percentWidth = Convert.ToInt32(System.Math.Ceiling(((double)colFullViewLength / (double)colFVWidths) * gridTotalUsableWidth));
                        BoundField newField = new BoundField();
                        newField.DataField = colNode.Attributes["Label"].Value;
                        newField.HeaderText = colNode.Attributes["Caption"].Value;
                        newField.HeaderStyle.VerticalAlign = VerticalAlign.Bottom;
                        newField.ItemStyle.Wrap = false;
                        newField.HtmlEncode = true;

                        //For Right-Justify issue
                        BoundField dummyField = null;//A dummy field at the end of the columns to fill in the gap
                        bool addDummyField = false;

                        if (GridViewType != GridViewType.DashBoard && GridViewType != GridViewType.Master)//Don't set widths in Dash
                        {
                            //gridColWidth = Convert.ToInt32(System.Math.Ceiling(((double)colFullViewLength / (double)colFVWidths)*100));
                            if (colCntr == gridColumnCount - 1)
                            {
                                //Last Column - Make it span the remaining width
                                //newField.ItemStyle.Width = Unit.Percentage(gridTotalUsableWidth - (gridColWidth * colCntr));
                                newField.ItemStyle.Width = Unit.Percentage(colFullViewLength);//Add the column normally
                                //Check for the no. of columns <= 4
                                if (gridTotalUsableWidth > colFVWidths)
                                {
                                    //Add a new column with blank contents
                                    dummyField = new BoundField();
                                    dummyField.HeaderText = "";
                                    //dummyField.ItemStyle.Width = Unit.Percentage(gridTotalUsableWidth - (gridColWidth * (colCntr + 1)));
                                    dummyField.ItemStyle.Width = Unit.Percentage(gridTotalUsableWidth - colFVWidths);
                                    addDummyField = true;
                                }
                            }
                            else
                            {
                                //Columns excluding the last one.
                                newField.ItemStyle.Width = Unit.Percentage(colFullViewLength);
                            }
                        }
                        //newField.ItemStyle.Width = Unit.Percentage(gridColWidth);
                        //Set the sort expression as inidicated in the xml.
                        if (colNode.Attributes["IsSortable"].Value == "1")
                        {
                            newField.SortExpression = colNode.Attributes["Label"].Value;
                            //Add this column name to the GroupBy Dropdowns of the respective report types hover tip for report 
                            //generation.
                            string sText = colNode.Attributes["Caption"].Value;
                            string sValue = newField.SortExpression;
                            ddlPDFGrpBy.Items.Add(new ListItem(sText, sValue));
                            ddlEXCELGrpBy.Items.Add(new ListItem(sText, sValue));
                        }

                        XmlAttribute attControlType = colNode.Attributes["ControlType"];
                        if (attControlType != null)
                        {
                            //Set justification to Right-Justify for columns with ControlType=Amount Calc
                            if ((attControlType.Value == "Amount") || (attControlType.Value == "Calc"))
                            {
                                newField.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                                newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                newField.ItemStyle.CssClass = "grdVwColRightJustify";
                            }

                        }
                        grdVwContents.Columns.Add(newField);
                        if (addDummyField)
                        {
                            grdVwContents.Columns.Add(dummyField);
                        }
                        colCntr++;
                    }
                }
            }

            //If there are no columns specified with FullViewLength, then add a dummy column
            if (colFVWidths == 0)
            {
                #region NLog
                logger.Debug("No Column Full View lengths specified. Binding an empty data column."); 
                #endregion

                //Add a new column with blank contents
                BoundField dummyField = new BoundField();
                dummyField.HeaderText = "";
                dummyField.ItemStyle.Width = Unit.Percentage(gridTotalUsableWidth);
                grdVwContents.Columns.Add(dummyField);
            }

            int defaultGridPageSize = 0;
            if (DefaultPageSize == "")//If the variable was not set externly.
            {   //DefaultGridSize
                defaultGridPageSize = Convert.ToInt32(GetUserPreferenceValue("59"));

                #region NLog
                logger.Debug("DefaultPageSize from the BPE : " + defaultGridPageSize); 
                #endregion
            }
            else
            {
                defaultGridPageSize = Convert.ToInt32(DefaultPageSize);

                #region NLog
                logger.Debug("DefaultPageSize from the instance property : " + defaultGridPageSize); 
                #endregion
            }
            grdVwContents.PageSize = defaultGridPageSize;
            //Calculating the page size of the grid view
            if (hdnCurrPageNo.Value.Length == 0)
            {
                hdnCurrPageNo.Value = "1";
            }
            //Setting a global variable for the TotalPageSize i.e total no of records.
            int totalPageSize = Convert.ToInt32(xDocReturn.
                SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/Gridresults/Totalpage").Attributes["Pagesize"].Value);
            int currentPageSize = Convert.ToInt32(xDocReturn.
                SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/Gridresults/Currentpage").Attributes["Pagesize"].Value);

            if (currentPageSize != 0)
            {
                m_MaxPages = Convert.ToInt32(System.Math.Ceiling((double)(totalPageSize / (double)defaultGridPageSize)));
            }

            #region NLog
            logger.Debug("TotalPageSize : " + totalPageSize + " CurrentPageSize : " + currentPageSize); 
            #endregion

            hdnMaxPages.Value = m_MaxPages.ToString();
            hdnGVTreeNodeName.Value = m_GVTreeNodeName;

            //Setting the Alternating Row Style of the GridView
            //FullViewAlternatingStyle
            string isAlternating = GetUserPreferenceValue("56");
            if (isAlternating != "1")
            {
                grdVwContents.AlternatingRowStyle.Reset();
                //ViewState["ALTRowStyle"] = string.Empty;
            }

            //Handling the display of the pager template based on the "IsOkToPage" and the number of pages.
            string isPagingEnabled = "";
            if (xDocReturn.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPage"] != null)
            {
                isPagingEnabled = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPage"].Value;
                if (isPagingEnabled == "0" || m_MaxPages == 1)
                {
                    pnlPagingCtrls.Visible = false;
                }
                else
                {
                    pnlPagingCtrls.Visible = true;
                }
            }

            //Check for correct format of BPInfo.If GridView node is wrapped by the Tree Node
            if (GridViewBPInfo.Length > 0)
            {
                XmlDocument xDocBPEInfo = new XmlDocument();
                xDocBPEInfo.LoadXml(GridViewBPInfo);
                XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("//Gridview");
                if (nodeGridView == null)
                {
                    logger.Warn("No GridView node in GridView BPInfo. Explicitly inserting the same.");
                    nodeGridView = xDocBPEInfo.CreateNode(XmlNodeType.Element, "Gridview", "");
                    nodeGridView.InnerXml = GetGridViewNodeXML("1", "", "ASC", defaultGridPageSize.ToString());
                    xDocBPEInfo.DocumentElement.AppendChild(nodeGridView);
                }

                if (nodeGridView.ParentNode.LocalName != hdnGVTreeNodeName.Value)
                {
                    //Then embed the GridView node into a New Node named as the Tree Node.
                    XmlNode xNodeTreeNode = xDocBPEInfo.SelectSingleNode("bpinfo/" + hdnGVTreeNodeName.Value);
                    if (xNodeTreeNode == null)
                    {
                        xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                    }
                    //Remove the GridView from the current parent(bpinfo) and append it as child to the above node.
                    nodeGridView.ParentNode.RemoveChild(nodeGridView);
                    xNodeTreeNode.AppendChild(nodeGridView);
                    xDocBPEInfo.DocumentElement.AppendChild(xNodeTreeNode);
                    GridViewBPInfo = xDocBPEInfo.OuterXml;
                    //HttpContext.Current.Session["BPINFO"] = xDocBPEInfo.OuterXml;
                    SessionLinkBPInfo = xDocBPEInfo.OuterXml;
                }
                XmlNode nodePgNo = nodeGridView.SelectSingleNode("Pagenumber");
                if (nodePgNo != null)
                {
                    hdnCurrPageNo.Value = nodePgNo.InnerText;
                }
            }
            InitialisePagesDDL();
            DisplayFoundResults(GridViewInitData);
            LoadLinkbuttonImages();
            FillXYDropdownData(GridViewInitData);
            //updtPnlGrdVw.Update();
        }

        private void FillXYDropdownData(string returnXML)
        {
            string m_GVTreeNodeName = string.Empty;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);

            XmlNode nodeStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeStatus != null && nodeStatus.InnerText == "Error")
            {
                return;
            }

            //Clear dropdownlist
            ddlXAxis.Items.Clear();
            ddlYAxis.Items.Clear();

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            int colCntr = 0;
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

            foreach (XmlNode colNode in nodeColumns.ChildNodes)
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
            //If y axis items default add  Count Item
            ddlYAxis.Items.Add(new ListItem("Count", "Count"));
        }

        /// <summary>
        /// Binds the page numbers the drop down list.
        /// </summary>
        private void InitialisePagesDDL()
        {
            ddlPages.Items.Clear();
            int maxPages = Convert.ToInt32(hdnMaxPages.Value);
            for (int index = 1; index <= maxPages; index++)
            {
                ddlPages.Items.Add(new ListItem(index.ToString(), index.ToString()));
            }
        }

        /// <summary>
        /// Loads images to the Link buttons.
        /// </summary>
        private void LoadLinkbuttonImages()
        {
            string cdnImgePath = Convert.ToString(Application["ImagesCDNPath"]);
            lbtnFirst.Text = "<img border=0 src=" + cdnImgePath + "images/arrow-first.gif>";
            lbtnPrevious.Text = "<img border=0 src=" + cdnImgePath + "images/arrow-left.gif>";
            lbtnNext.Text = "<img border=0 src=" + cdnImgePath + "images/arrow-right.gif>";
            lbtnLast.Text = "<img border=0 src=" + cdnImgePath + "images/arrow-last.gif>";

            //added by shanti(12/08/09)
            GridViewRow gvhr = grdVwContents.HeaderRow;
            if (gvhr != null)
            {
                LAjitImageButton imgBtnQuickSearch = (LAjitImageButton)gvhr.FindControl("imgBtnQuickSearch");
                imgBtnQuickSearch.ImageUrl = cdnImgePath + "Images/grid-find-icon.png";
            }
            imgBtnPrint.ImageUrl = cdnImgePath + "Images/grid-print.png";
            imgBtnChart.ImageUrl = cdnImgePath + "Images/grid-chart.png";
            imgBtnPDF.ImageUrl = cdnImgePath + "Images/grid-pdf.png";
            imgBtnExcel.ImageUrl = cdnImgePath + "Images/grid-excel.png";
            imgBtnHtml.ImageUrl = cdnImgePath + "Images/grid-html.png";
            imgBtnEmailPDF.ImageUrl = cdnImgePath + "Images/email_icon2.png";

            //imgBtnPDF.Attributes.Add("onmouseover", "ShowHoverMenu('" + pnlHoverPageSelect.ClientID+ "', this, true)");
            //imgBtnExcel.Attributes.Add("onmouseover", "ShowHoverMenu('" + pnlHoverPageSelect.ClientID + "', this, true)");
            //imgBtnHtml.Attributes.Add("onmouseover", "ShowHoverMenu('" + pnlHoverPageSelect.ClientID + "', this, true)");
            //imgBtnPDF.Attributes.Add("onmouseout", "HideHoverMenu('" + pnlHoverPageSelect.ClientID + "', this)");
            //imgBtnExcel.Attributes.Add("onmouseout", "HideHoverMenu('" + pnlHoverPageSelect.ClientID + "', this)");
            //imgBtnHtml.Attributes.Add("onmouseout", "HideHoverMenu('" + pnlHoverPageSelect.ClientID + "', this)");

            //pnlHoverPageSelect.Attributes.Add("onmouseover", "ShowHoverMenu('" + pnlHoverPageSelect.ClientID + "', this, false)");
            //pnlHoverPageSelect.Attributes.Add("onmouseout", "ShowHoverMenu('" + pnlHoverPageSelect.ClientID + "', this, false)");
        }

        /// <summary>
        /// To the change the ViewState["GridViewType"] to the actual XML node name from that of the enum value.
        /// </summary>
        private void LoadGVType()
        {
            switch ((GridViewType)ViewState["GridViewType"])
            {
                case GridViewType.DashBoard:
                    {
                        grdVwContents.AlternatingRowStyle.CssClass = "AlternatingRowStyle";
                        break;
                    }
                case GridViewType.COA:
                    {
                        trTitle.Visible = false;
                        break;
                    }
                case GridViewType.AccountingLayout:
                    {
                        trTitle.Visible = false;
                        break;
                    }
                case GridViewType.Master:
                    {
                        grdVwContents.AlternatingRowStyle.CssClass = "AlternatingRowStyle";
                        break;
                    }
                default:
                    {
                        grdVwContents.AlternatingRowStyle.CssClass = "AlternatingCOARowStyle";
                        trTitle.Visible = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// Binds the grid view with the passed data.
        /// </summary>
        /// <param name="returnXML">The grid view XML data</param>
        public void DisplayFoundResults(string returnXML)
        {
            #region NLog
            logger.Info("Binding the grid view with the data returned in the XML.");
            #endregion

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);
            m_ReturnXML = returnXML;

            XmlNode nodeStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeStatus != null && nodeStatus.InnerText == "Error")
            {
                logger.Error("Status node not found. Aborting Grid data bind.");
                return;
            }

            //If the GridView belongs to Master page, then ViewState has to be maintained as the data contained in 
            //BtnsUC might be different from the data of the Master grids.
            if (GridViewType == GridViewType.Master)
            {
                hdnMainViewState.Value = "1";
                ViewState["GridViewInitData"] = returnXML;
            }

            //Initialising the variables m_CurrentBPGID and m_CurrentPageInfo for the view details functionality
            m_CurrentBPGID = xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            m_CurrentPageInfo = xDoc.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;

            if (!m_CurrentPageInfo.Contains(".aspx") || !m_CurrentPageInfo.Contains("/"))
            {
                string absPath = this.Page.Request.Url.AbsolutePath;
                if (!string.IsNullOrEmpty(absPath) && absPath.Length > 1)
                {
                    m_CurrentPageInfo = absPath.Substring(1, absPath.Length - 1);
                }
            }

            //Keep one variable for the entire formInfo..
            hdnFormInfo.Value = m_CurrentBPGID + "~::~" + m_CurrentPageInfo;

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            // Indicates whether the current datasource has any rows or not.
            bool noRows = false;

            //Getting the dataset to be bound to the grid.
            XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList");
            DataSet dsRowList = new DataSet();
            if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
            {
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                dsRowList.ReadXml(read);
            }
            else
            {
                InitBPCXML(returnXML);//Init the BPC XML to a Global variable.
                //Check the BusinessProcessControls if "Add" process is present or not.
                //Show the GridView search row expanded(with a single row) only if Add BPGID is not present
                //as the Add Entry screen will be shown anyway if the Add BPGID is present.
                if (GetBPCBPGID("Add", m_BPCXml).Length == 0)
                {
                    //Add a new DataTable to the DataSet explicitly
                    dsRowList.Tables.Add(new DataTable());
                    //Show the Search Header row which is otherwise hidden by default.
                    hdnCSFindVisible.Value = "1";
                    pnlPagingCtrls.Visible = false;
                    //pnlPrint.Visible = false;
                    noRows = true;
                }
                else
                {
                    grdVwContents.DataSource = new DataTable();
                    grdVwContents.DataBind();
                    pnlPagingCtrls.Visible = false;
                    return;
                }
            }

            //Hide the arrow buttons according to the page number.
            if (hdnCurrPageNo.Value == "1")
            {
                //lbtnFirst.Visible = false;
                //lbtnPrevious.Visible = false;
                //lbtnLast.Visible = true;
                //lbtnNext.Visible = true;
                lbtnFirst.Style["visibility"] = "hidden";
                lbtnPrevious.Style["visibility"] = "hidden";
                lbtnLast.Style["visibility"] = "";
                lbtnNext.Style["visibility"] = "";
            }
            else if (hdnCurrPageNo.Value == hdnMaxPages.Value)
            {
                //lbtnLast.Visible = false;
                //lbtnNext.Visible = false;
                //lbtnFirst.Visible = true;
                //lbtnPrevious.Visible = true;
                lbtnLast.Style["visibility"] = "hidden";
                lbtnNext.Style["visibility"] = "hidden";
                lbtnFirst.Style["visibility"] = "";
                lbtnPrevious.Style["visibility"] = "";
            }
            else
            {
                //lbtnFirst.Visible = true;
                //lbtnPrevious.Visible = true;
                //lbtnLast.Visible = true;
                //lbtnNext.Visible = true;
                lbtnFirst.Style["visibility"] = "";
                lbtnPrevious.Style["visibility"] = "";
                lbtnLast.Style["visibility"] = "";
                lbtnNext.Style["visibility"] = "";
            }


            m_htBPCntrls.Clear();
            m_htBPCColumns.Clear();
            m_htGVColumns.Clear();
            m_htGVColWidths.Clear();
            m_arrImageColIndices.Clear();
            m_arrAmountCols.Clear();
            m_htSearchCols.Clear();
            //SearchColIndices.Clear();

            int colCntr = 0;
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");
            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                string label = colNode.Attributes["Label"].Value;
                //Adding the current column node the ROWS dataset if not present.
                if (!dsRowList.Tables[0].Columns.Contains(label))
                {
                    DataColumn dcNew = new DataColumn(label, typeof(string));
                    dcNew.AllowDBNull = true;
                    dsRowList.Tables[0].Columns.Add(dcNew);
                }

                int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)//All displayable columns processing...
                {
                    //Check for HelpAuthor
                    if (ObjCommonUI.IsHelpAuthPage)
                    {
                        if (label == "HelpFile")
                        {
                            m_IndexOfHelpFile = colCntr;
                        }
                    }

                    //For tooltip functionality.
                    m_htGVColumns.Add(label, colCntr);
                    //Add the Column Length
                    m_htGVColWidths.Add(colCntr, colFullViewLength);

                    XmlAttribute attIsSearchable = colNode.Attributes["IsSearched"];
                    if (attIsSearchable != null && attIsSearchable.Value == "1")
                    {
                        m_htSearchCols.Add(colCntr, "");
                        //SearchColIndices.Add(colCntr);
                    }

                    XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                    if (attBPControl != null && attBPControl.Value.Trim().Length > 0)
                    {
                        m_htBPCntrls.Add(attBPControl.Value.Trim(), GetColumnIndex(label, dsRowList.Tables[0]));
                        m_htBPCColumns.Add(attBPControl.Value.Trim(), colCntr);
                    }

                    XmlAttribute attControlType = colNode.Attributes["ControlType"];
                    if (attControlType != null)
                    {
                        m_arrColControlTypes.Add(attControlType.Value);
                        //Check for ControlType to be Amount
                        if ((attControlType.Value == "Amount") || (attControlType.Value == "Calc"))
                        {
                            m_arrAmountCols.Add(label);
                        }

                        //Check for Control Type to be DateTime
                        if (attControlType.Value == "Cal")
                        {
                            foreach (DataRow dr in dsRowList.Tables[0].Rows)
                            {
                                DateTime dateTime;
                                if (DateTime.TryParse(dr[label.Trim()].ToString(), out dateTime))
                                {
                                    dr[label.Trim()] = dateTime.ToString("MM/dd/yyyy");
                                }
                            }
                        }

                        if (attControlType.Value == "Img")
                        {
                            m_arrImageColIndices.Add(colCntr);
                            //Adding images dynamically to the grid requires a viewstate to be maintained and data 
                            //from this view state needs to be rendered on every postback
                            if (hdnMainViewState.Value != "1")
                            {
                                hdnMainViewState.Value = "1";
                                //Check for the existence of BtnsUC in the Page.If not present create your own ViewState.
                                Control ucBtnsControl = this.Parent.NamingContainer.FindControl("BtnsUC");
                                if (ucBtnsControl == null)
                                {
                                    ViewState["GridViewInitData"] = returnXML;
                                }
                            }
                        }
                    }
                    colCntr++;
                    dsRowList.AcceptChanges();
                }
            }

            this.ViewState["SearchCols"] = m_htSearchCols;

            //Format the Amount columns specified by the m_arrAmountCols object in the data source
            foreach (string colName in m_arrAmountCols)
            {
                int colIndex = dsRowList.Tables[0].Columns[colName].Ordinal;
                foreach (DataRow dr in dsRowList.Tables[0].Rows)
                {
                    decimal amount;
                    if (Decimal.TryParse(dr[colIndex].ToString(), out amount))
                    {
                        dr[colIndex] = string.Format("{0:N}", amount);
                    }
                }
            }

            m_GridTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Title").InnerText;
            //Setting the title of the grid view container panel.
            SetPanelHeading(htcGridTitle, GridTitle);

            //Initialising variables to be used in OnRowDataBound
            //Get the BPC node from the BtnsUC.GVDataXML as the node might go missing in Find case.
            InitBPCXML(returnXML);

            //Check the BusinessProcessControls if "Find" process is present or not.
            if (GetBPCBPGID("Find", m_BPCXml).Length != 0)
            {
                IsFindEnabled = true;
            }

            m_primaryKeyFieldName = dsRowList.Tables[0].Columns[0].ColumnName;

            //Apply Row Hovering effects only if there is an alternating style applied.
            if (grdVwContents.AlternatingRowStyle.CssClass.Length > 0)
            {
                //Get the Row Hover Colour from the Config file which will be used in the RowDataBound event.
                m_RowHoverColour = ConfigurationManager.AppSettings["GridRowHoverColor"];
                m_IsRowAlternating = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OnHover", "g_AllowRowHover=true;", true);
            }

            //If current page is an Help Auth page then remove the Time stamps for the file names.
            DataTable dtToolTipData = UpdateToolTipInfo(dsRowList.Tables[0]);

            m_sbToolTipJS.Remove(0, m_sbToolTipJS.Length);


            if (noRows)
            {
                #region NLog
                logger.Debug("No rows found in the XML. Adding an empty row to facilitate display of the search row."); 
                #endregion

                //Case where the search row has to be displayed when no rows are sent in the XML.So add a new row to the table.
                dtToolTipData.Rows.Add(dtToolTipData.NewRow());
                grdVwContents.DataSource = dtToolTipData;
                grdVwContents.DataBind();

                //Hide the row rendered as a result of the above operations.
                grdVwContents.Rows[0].Visible = false;

            }
            else
            {
                grdVwContents.DataSource = dtToolTipData;
                grdVwContents.DataBind();
            }
            if (hdnMaxPages.Value != "0")
            {
                ddlPages.SelectedIndex = Convert.ToInt32(hdnCurrPageNo.Value) - 1;
            }
            lblPageStatus.Text = String.Concat(hdnCurrPageNo.Value, " of ", hdnMaxPages.Value);
            lblPageNo.Text = "Go To Page";
            //Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ToolTip", "g_ToolTipJS='" + m_sbToolTipJS.ToString() + "';", true);
            hdnToolTipJS.Value = m_sbToolTipJS.ToString();
            updtPnlGrdVw.Update();
        }

        private void InitBPCXML(string returnXML)
        {
            if (BtnsUserCtrl == null)
            {
                BtnsUserCtrl = (UserControls.ButtonsUserControl)this.NamingContainer.FindControl("BtnsUC");
            }
            XmlDocument xDocGVDataXML = new XmlDocument();
            if (BtnsUserCtrl != null)
            {
                xDocGVDataXML.LoadXml(BtnsUserCtrl.GVDataXml);
            }
            else
            {
                xDocGVDataXML.LoadXml(returnXML);
            }
            XmlNode xNodeBPC = xDocGVDataXML.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (xNodeBPC != null)
            {
                m_BPCXml = xNodeBPC.OuterXml;
            }
            else
            {
                m_BPCXml = string.Empty;
            }
        }

        /// <summary>
        /// On RowDataBound Event for grdVwContents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdVwContents_RowDataBound(object sender, GridViewRowEventArgs e)
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

                /*****Don't delete
                //CheckBox chkBxProcess = (CheckBox)e.Row.FindControl("chkBxProcess");
                //if (chkBxProcess == null)
                //{
                //    chkBxProcess = new CheckBox();
                //    chkBxProcess.ID = "chkBxProcess";
                //    e.Row.Cells[1].Controls.Add(chkBxProcess);
                //}
                //chkBxProcess.Attributes.Add("RowXML", rowXML);
                 */

                //Tooltip Popup functionality using custom js file.    
                //Check for the presence of Tooltip attributes. If there are no attributes then dont show the ToolTip the image.
                // Programmatically refer the Image control
                System.Web.UI.WebControls.ImageButton imgBtnTTnNavigate =
                    (System.Web.UI.WebControls.ImageButton)e.Row.Cells[2].FindControl("imgBtnTTnNavigate");
                imgBtnTTnNavigate.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/GridViewSelect.png";
                if (drvCurrent.Row.ItemArray[drvCurrent.Row.ItemArray.Length - 1].ToString() != string.Empty)
                {
                    imgBtnTTnNavigate.Visible = true;
                    imgBtnTTnNavigate.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/GridViewToolTip.png";
                    // Add the client-side attributes (onmouseover & onmouseout)
                    //string onMouseOverScript = string.Format("ShowToolTipPopup('{0}','{1}');", hdnCurrentRow.ClientID, imgBtnTTnNavigate.ClientID);
                    m_sbToolTipJS.Append((e.Row.RowIndex + 2) + ":");
                    //Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ToolTip" + e.Row.RowIndex, onMouseOverScript);
                    //string onMouseOutScript = "HideToolTipPopup();";
                    imgBtnTTnNavigate.Style.Add("cursor", "pointer");
                    //imgBtnTTnNavigate.Attributes.Add("onmouseover", onMouseOverScript);
                }
                else
                {
                    imgBtnTTnNavigate.ToolTip = "View Details";
                }

                //Row Effects based on IsActive, TypeOfInactiveStatusID etc.,
                SetRowColours(e, drvCurrent, imgBtnTTnNavigate);

                //For Tooltip functionality
                InitialiseToolTips(e);

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

                //Initialise Image Columns if any
                foreach (int imgColIndex in m_arrImageColIndices)
                {
                    string imageName = e.Row.Cells[m_NoOfGVStaticColumns + imgColIndex].Text;
                    e.Row.Cells[m_NoOfGVStaticColumns + imgColIndex].Text = "";
                    System.Web.UI.WebControls.Image myImage = new System.Web.UI.WebControls.Image();
                    myImage.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/" + imageName;
                    myImage.ImageAlign = ImageAlign.Middle;
                    myImage.Height = Unit.Pixel(18);
                    myImage.AlternateText = imageName;

                    e.Row.Cells[m_NoOfGVStaticColumns + imgColIndex].Controls.Add(myImage);
                }
            }
        }

        /// <summary>
        /// Gridview pre-render event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdVwContents_PreRender(object sender, EventArgs e)
        {
            //SaveQuickSearchRow();
            ShowFindRow();
        }

        /// <summary>
        /// Gridview Sort event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdVwContents_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);

            string sortDirection = string.Empty;
            m_CurrPageNo = Convert.ToInt32(hdnCurrPageNo.Value);
            if (ViewState["SORTDIRECTION"].ToString() == "ASC")
            {
                sortDirection = "DESC";
                ViewState["SORTDIRECTION"] = "DESC";
            }
            else
            {
                sortDirection = "ASC";
                ViewState["SORTDIRECTION"] = "ASC";
            }
            ViewState["SORTEXP"] = e.SortExpression;
            string result = GenerateRequestXML(m_CurrPageNo.ToString(), e.SortExpression, sortDirection);
            DisplayFoundResults(result);
        }

        /// <summary>
        /// Displays textboxes beneath headings of each of the columns for the search functionality.
        /// </summary>
        private void ShowFindRow()
        {
            try
            {
                GridViewRow gvhr = grdVwContents.HeaderRow;
                if (gvhr == null)
                {
                    return;
                }
                if (IsFindEnabled)
                {
                    #region NLog
                    logger.Debug("GridView Search is enabled.Rendering the search toolbar."); 
                    #endregion

                    Hashtable htSearchCols = (Hashtable)this.ViewState["SearchCols"];
                    if (htSearchCols != null && htSearchCols.Count == 0)
                    {
                        //No Search columns specified.
                        //Hide the View Find Icon in the first column.
                        gvhr.FindControl("tblFind").Visible = false;
                        gvhr.Cells[2].Text = "View";
                    }

                    LAjitImageButton imgBtnQuickSearch = (LAjitImageButton)gvhr.FindControl("imgBtnQuickSearch");
                    imgBtnQuickSearch.OnClientClick = "BuildQuickSearchRow('" + this.ClientID + "');return true;";
                    imgBtnQuickSearch.Attributes["title"] = "Right-Click to clear Search Fields.";

                    LAjitImageButton imgBtnExpandFind = (LAjitImageButton)gvhr.FindControl("imgBtnExpandFind");
                    imgBtnExpandFind.OnClientClick = "ShowSearch('" + grdVwContents.ClientID + "');return false;";
                    if (hdnCSFindVisible.Value == "0")
                    {
                        imgBtnQuickSearch.Style.Add("display", "none");
                        imgBtnExpandFind.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add_symbol.png";
                    }
                    else
                    {
                        imgBtnQuickSearch.Style.Add("display", "");
                        imgBtnExpandFind.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/minus-icon.png";
                    }

                    string[] arrFindCriteria = null;
                    string[] delimiter = { "~::~" };
                    int criteriaCntr = 0;
                    if (hdnFindCriteria.Value.Length > 0)
                    {
                        arrFindCriteria = hdnFindCriteria.Value.Split(delimiter, StringSplitOptions.None);
                    }

                    foreach (TableCell tc in gvhr.Cells)
                    {
                        int cellIndex = gvhr.Cells.GetCellIndex(tc);
                        if (cellIndex < m_NoOfGVStaticColumns)
                        {
                            continue;
                        }
                        int searchKey = cellIndex - m_NoOfGVStaticColumns;
                        if (htSearchCols.ContainsKey(searchKey))
                        {
                            string colDataField = ((BoundField)grdVwContents.Columns[cellIndex]).DataField;
                            string colHeaderText = ((BoundField)grdVwContents.Columns[cellIndex]).HeaderText;
                            LinkButton lnkBtnHeader = null;
                            Label lblHeader = null;
                            if (tc.Controls.Count > 0)
                            {
                                lnkBtnHeader = (LinkButton)tc.Controls[0];
                                tc.Controls.Clear();
                            }
                            else
                            {
                                lblHeader = new Label();
                                lblHeader.Text = colHeaderText;
                                tc.Text = "";
                            }

                            LAjitTextBox txtCurrent = new LAjitTextBox();
                            //txtCurrent.Text = htSearchCols[searchKey].ToString();
                            if (arrFindCriteria != null)
                                txtCurrent.Text = arrFindCriteria[criteriaCntr++];
                            txtCurrent.ID = "txtFind" + cellIndex;
                            txtCurrent.Width = Unit.Percentage(92);
                            txtCurrent.Height = Unit.Pixel(16);
                            txtCurrent.Attributes.Add("onkeyup", "BuildQuickSearchRow('" + this.ClientID + "');return true;");
                            //tc.Controls.Add(txtCurrent);


                            Table tblContainer = new Table();
                            //tblContainer.Style.Add("height", "100%");
                            tblContainer.CellPadding = 0;
                            tblContainer.CellSpacing = 2;
                            tblContainer.Width = Unit.Percentage(100);
                            TableRow tr1 = new TableRow();
                            TableCell tc1 = new TableCell();
                            if (m_arrAmountCols.Contains(colDataField) || colDataField == "Amount")
                            {
                                tc1.HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                tc1.HorizontalAlign = HorizontalAlign.Left;
                            }
                            if (lnkBtnHeader != null)
                            {
                                tc1.Controls.Add(lnkBtnHeader);
                            }
                            else
                            {
                                tc1.Controls.Add(lblHeader);
                            }
                            tr1.Cells.Add(tc1);
                            tblContainer.Rows.Add(tr1);

                            TableRow tr2 = new TableRow();
                            tr2.ID = "trFind" + cellIndex;
                            if (hdnCSFindVisible.Value == "0")
                            {
                                tr2.Style.Add("display", "none");
                            }
                            else
                            {
                                tr2.Style.Add("display", "");
                            }

                            //Panel pnlEnvelope = new Panel();
                            //pnlEnvelope.Controls.Add(txtCurrent);

                            TableCell tc21 = new TableCell();
                            tc21.Controls.Add(txtCurrent);
                            tr2.Cells.Add(tc21);
                            tblContainer.Rows.Add(tr2);

                            tc.Controls.Add(tblContainer);
                            //Give two spaces for Search Text as it can be identified while row formation and overidden even if the search criterion is "Search"(Without the 2spaced ofcourse) itself.
                            LAjitControls.LAjitWaterMark.WaterMarkHelper.ApplyWaterMarkToTextBox(txtCurrent, "Search  ", "WaterMarkedTextBox", "WaterMarkNormal");

                        }
                    }
                }
                else
                {
                    logger.Debug("GridView Search is disabled.");
                    gvhr.FindControl("tblFind").Visible = false;
                    gvhr.Cells[2].Text = "View";
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }

        /// <summary>
        /// Called when the User has submitted the search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnQuickSearch_Click(object sender, ImageClickEventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);
            try
            {
                string searchRow = BuildQuickSearchRow();
                if (searchRow.Length > 0)
                {
                    string bpgIdFind = string.Empty;
                    LAjitImageButton imgBtnSender = (LAjitImageButton)sender;
                    UserControls.ButtonsUserControl btnsUC = null;
                    XmlDocument xDocGVData = new XmlDocument();
                    if (this.GridViewType == GridViewType.DashBoard)
                    {
                        bpgIdFind = ((HiddenField)imgBtnSender.NamingContainer.NamingContainer.NamingContainer.NamingContainer.FindControl("hdnFindBPGID")).Value; ;
                        btnsUC = new LAjitDev.UserControls.ButtonsUserControl();
                        btnsUC.GVDataXml = "<Root></Root>";
                    }
                    else
                    {
                        ContentPlaceHolder cphPageContents = (ContentPlaceHolder)imgBtnSender.NamingContainer.NamingContainer.NamingContainer.NamingContainer;
                        Panel pnlEntryForm = (Panel)cphPageContents.FindControl("pnlEntryForm");
                        btnsUC = (UserControls.ButtonsUserControl)cphPageContents.FindControl("BtnsUC");
                        xDocGVData.LoadXml(btnsUC.GVDataXml);
                        XmlNode xNodeBPC = xDocGVData.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                        string BPCXML = string.Empty;
                        if (xNodeBPC != null)
                        {
                            BPCXML = xNodeBPC.OuterXml;
                        }
                        bpgIdFind = GetBPCBPGID("Find", BPCXML);
                    }

                    //LAjitImageButton imgBtnFind = (LAjitImageButton)btnsUC.FindControl("imgBtnFind");
                    string defaultPageSize = grdVwContents.PageSize.ToString();
                    CommonBO objBO = new CommonBO();
                    XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                    string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                    string requestXML = string.Empty;
                    if (this.GridViewType == GridViewType.DashBoard)
                    {
                        XmlDocument xDoc = new XmlDocument();
                        XmlNode nodeRoot = xDoc.CreateNode(XmlNodeType.Element, "Root", null);
                        xDoc.AppendChild(nodeRoot);
                        nodeRoot.InnerXml = strBPE;
                        XmlNode nodeBpInfo = xDoc.CreateNode(XmlNodeType.Element, "bpinfo", null);
                        nodeRoot.AppendChild(nodeBpInfo);

                        XmlNode nodeBPGID = xDoc.CreateNode(XmlNodeType.Element, "BPGID", null);
                        nodeBpInfo.AppendChild(nodeBPGID);
                        nodeBPGID.InnerText = bpgIdFind;

                        XmlNode nodeTree = xDoc.CreateNode(XmlNodeType.Element, hdnGVTreeNodeName.Value, null);
                        nodeBpInfo.AppendChild(nodeTree);
                        nodeTree.InnerXml += "<RowList>" + searchRow + "</RowList>";

                        nodeBpInfo.InnerXml += " <Gridview><Pagenumber>1</Pagenumber><Pagesize>" + defaultPageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";

                        XmlNode nodeFindRow = nodeBpInfo.SelectSingleNode("//" + hdnGVTreeNodeName.Value + "/RowList/Rows");
                        XmlAttribute attBPAction = xDoc.CreateAttribute("BPAction");
                        attBPAction.Value = "Find";
                        nodeFindRow.Attributes.Append(attBPAction);
                        requestXML = nodeRoot.OuterXml;
                    }
                    else
                    {
                        requestXML = objBO.GenActionRequestXML("Find", bpgIdFind, searchRow, null, btnsUC.GVDataXml,
                            strBPE, true, defaultPageSize, "1", SessionLinkBPInfo);
                    }

                    XmlDocument xDocReqXML = new XmlDocument();
                    xDocReqXML.LoadXml(requestXML);
                    //Request the Find results from the DB.
                    string strOutXml = objBO.GetDataForCPGV1(requestXML);

                    XmlDocument xDocOut = new XmlDocument();
                    xDocOut.LoadXml(strOutXml);

                    XmlNode nodeMessageInfo = xDocOut.SelectSingleNode("Root/bpeout/MessageInfo");

                    if (nodeMessageInfo.SelectSingleNode("Status").InnerText == "Error")
                    {
                        //pnlEntryForm.Attributes.Add("style", "DISPLAY: none;");
                        string errorLabel = "alert('" + nodeMessageInfo.SelectSingleNode("Message/Label").InnerText + "');";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "QuickFind", errorLabel, true);
                    }
                    else
                    {
                        //This bpeInfo will be used for Print requests for the current Find Results.
                        XmlNode nodeBPInfo = xDocReqXML.SelectSingleNode("Root/bpinfo").CloneNode(true);
                        nodeBPInfo.SelectSingleNode("//Gridview/Pagesize").InnerText = "-1";
                        hdnSearchBPInfo.Value = nodeBPInfo.OuterXml;
                        //HttpContext.Current.Session["BPINFO"] = nodeBPInfo.OuterXml;
                        SessionLinkBPInfo = nodeBPInfo.OuterXml;
                        if (this.GridViewType != GridViewType.DashBoard)
                        {
                            if (btnsUC != null)
                            {
                                ((HiddenField)btnsUC.FindControl("parentBPInfo")).Value = nodeBPInfo.OuterXml;
                            }


                            //Update only the Tree/Branch node of the GVDataXML with the Find results.
                            //Get both the Tree and Branch nodes.
                            XmlNodeList nlTreeBranches = xDocGVData.SelectNodes("Root/bpeout/FormControls/GridLayout//Node");
                            foreach (XmlNode node in nlTreeBranches)
                            {
                                string nodeName = node.InnerText;
                                XmlNode nodeToReplace = xDocGVData.SelectSingleNode("Root/bpeout/FormControls//" + nodeName);
                                XmlNode nodeSource = xDocOut.SelectSingleNode("Root/bpeout/FormControls//" + nodeName);
                                if (nodeSource != null && nodeToReplace != null)
                                {
                                    nodeToReplace.InnerXml = nodeSource.InnerXml;
                                }
                            }
                            btnsUC.GVDataXml = xDocGVData.OuterXml;
                        }


                        this.GridViewBPInfo = xDocReqXML.SelectSingleNode("Root/bpinfo").OuterXml;
                        if (xDocGVData.OuterXml.Length > 0)
                        {
                            this.GridViewInitData = xDocGVData.OuterXml;
                        }
                        else
                        {
                            this.GridViewInitData = strOutXml;
                        }
                        hdnCurrPageNo.Value = "1";
                        this.DataBind();

                        //Collapse the Find Bar on a successful search
                        hdnCSFindVisible.Value = "0";
                        string js = "jQuery.FindVisible=false";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "QuickFind", js, true);
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion

                Classes.ErrorLogger.LogError(ex.Message);
            }
        }

        private void SaveQuickSearchRow()
        {
            Hashtable htSearchCols = (Hashtable)this.ViewState["SearchCols"];
            GridViewRow gvRowFind = grdVwContents.HeaderRow;
            if (htSearchCols == null || gvRowFind == null)
            {
                return;
            }
            for (int cellIndex = m_NoOfGVStaticColumns; cellIndex < gvRowFind.Cells.Count; cellIndex++)
            {
                //N varies from FindStartColumn to FindEndColumn
                LAjitTextBox txtFindN = ((LAjitTextBox)gvRowFind.Cells[cellIndex].FindControl("txtFind" + cellIndex));
                if (txtFindN != null)
                {
                    htSearchCols[cellIndex - m_NoOfGVStaticColumns] = txtFindN.Text;
                }
            }
        }

        /// <summary>
        /// Generates the XML based string accoring to the inputs entered in the Quick Search Bar.
        /// </summary>
        /// <returns>XML-like string.</returns>
        private string BuildQuickSearchRow()
        {
            #region NLog
            logger.Info("Generating the XML based string accoring to the inputs entered in the Quick Search Bar."); 
            #endregion

            GridViewRow gvRowFind = grdVwContents.HeaderRow;
            bool isRowOk = false;
            System.Text.StringBuilder sbFindRow = new System.Text.StringBuilder();
            sbFindRow.Append("<Rows ");
            for (int cellIndex = m_NoOfGVStaticColumns; cellIndex < gvRowFind.Cells.Count; cellIndex++)
            {
                //N varies from FindStartColumn to FindEndColumn
                LAjitTextBox txtFindN = ((LAjitTextBox)gvRowFind.Cells[cellIndex].FindControl("txtFind" + cellIndex));
                string mapXML = ((BoundField)grdVwContents.Columns[cellIndex]).DataField;
                if (txtFindN != null)// && txtFindN.Text.Length > 0 && txtFindN.Text != "Search  ")
                {
                    string searchCriterion = txtFindN.Text;
                    if (searchCriterion == "Search  ")
                    {
                        searchCriterion = string.Empty;
                    }
                    sbFindRow.Append(mapXML + "=\"" + m_ObjCommonUI.CharactersToHtmlCodes(searchCriterion) + "\" ");
                    isRowOk = true;
                }
            }
            if (isRowOk)
            {
                sbFindRow.Append(" />");
                return sbFindRow.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Looks for tool tip attribute columns and adds their indices in a new column at the end to the data table.
        /// </summary>
        /// <param name="dtRowList">Table to search.</param>
        /// <returns>Table with an additional column containing the indices of tool tip columns.</returns>
        private DataTable UpdateToolTipInfo(DataTable dtRowList)
        {
            string[] arrToolTipFields = { "Notes", "Attachments", "SecuredBy", "ChangedBy", "ToolTip" };
            //Add the column into the table first.
            DataColumn dcNew = new DataColumn("ToolTipInfo", typeof(string));
            dcNew.AllowDBNull = true;
            dtRowList.Columns.Add(dcNew);


            foreach (DataRow dr in dtRowList.Rows)
            {
                string ttColIndices = string.Empty;
                for (int index = 0; index < arrToolTipFields.Length; index++)
                {
                    if (dtRowList.Columns.Contains(arrToolTipFields[index]) && dr[arrToolTipFields[index]].ToString() != "")
                    {
                        ttColIndices += GetColumnIndex(arrToolTipFields[index].ToString(), dtRowList).ToString() + ",";
                    }

                    //if (ObjCommonUI.IsHelpAuthPage)
                    //{
                    //    if (dtRowList.Columns.Contains("HelpFile"))
                    //    {
                    //        dr["HelpFile"] = ObjCommonUI.TrimTimeStamp(dr["HelpFile"].ToString());
                    //    }
                    //}
                }
                if (ttColIndices.Contains(","))
                {
                    dr["ToolTipInfo"] = ttColIndices.Remove(ttColIndices.Length - 1, 1);
                }
            }
            return dtRowList;
        }

        /// <summary>
        /// Sets the title of the given grid view.
        /// </summary>
        /// <param name="htcWork">Target HTML Table Cell.</param>
        /// <param name="gridTitle">String title to be set.</param>
        private void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            #region NLog
            logger.Info("Setting the title of the given grid view as : "+gridTitle);
            #endregion

            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // Need a bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;
            htcWork.InnerText = gridTitle;
            htcWork.Width = intScrollLength.ToString();
            objGraphics.Dispose();
            objBitmap.Dispose();
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
            //DataColumn dc = dtSearch.Columns[columnName];
            //if (dc != null)
            //{
            //    return dc.Ordinal;
            //}
            //else
            //{
            //    return -1;
            //}
            //throw new Exception("Specified column" + columnName + " not found in the data source!!");
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
            logger.Info(" Getting the value of the business rule name : "+ruleName+" and business rule node : "+businessRulesNode +" from the given XML.");
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
        /// Gets the value of the user preference from the given XML.
        /// </summary>
        /// <param name="ruleName">Preference name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        private string GetUserPreferenceValue(string bRuleID)
        {
            #region NLog
            logger.Info("Getting the value of the user preference for rule id : "+bRuleID+" from the given XML.");
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
        /// Gets the attribute value for a given attribute name based on a process.
        /// </summary>
        /// <param name="processName">The Process name to look out for.</param>
        /// <param name="m_BPCXml">The BPC node XML</param>
        /// <param name="AttributeName">The attribute name in the row matching the process</param>
        /// <returns>Attribute Value</returns>
        private string GetBPCAttributeValue(string processName, string m_BPCXml, string AttributeName)
        {
            #region NLog
            logger.Info("Getting the attribute value for a given attribute name : "+AttributeName+" based on a process : "+processName);
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
            int colCntr = 0;
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

            //m_IndexOfIsProtected = -1;
            //m_IndexOfIsActive = -1;
            //m_IndexOfTypeOfInactiveStatusID = -1;
            //foreach (DataColumn dc in drvCurrent.DataView.Table.Columns)
            //{
            //    switch (dc.ColumnName)
            //    {
            //        case "IsProtected":
            //            {
            //                m_IndexOfIsProtected = colCntr;
            //                break;
            //            }
            //        case "IsActive":
            //            {
            //                m_IndexOfIsActive = colCntr;
            //                break;
            //            }
            //        case "TypeOfInactiveStatusID":
            //            {
            //                m_IndexOfTypeOfInactiveStatusID = colCntr;
            //                break;
            //            }
            //        default:
            //            {
            //                break;
            //            }
            //    }
            //    colCntr++;
            //}
        }

        /// <summary>
        /// Initilises tool tips for the cells in the row based on FullViewLength attribute in the XML.
        /// </summary>
        /// <param name="e">Row Event Arguments.</param>
        private void InitialiseToolTips(GridViewRowEventArgs e)
        {
            foreach (DictionaryEntry de in m_htGVColumns)
            {
                //Don't apply any text formatting for image columns.
                if (m_arrImageColIndices.Contains(de.Value))
                {
                    continue;
                }
                string colName = de.Key.ToString();
                int currentColIndex = Convert.ToInt32(de.Value) + m_NoOfGVStaticColumns;
                int colLength = Convert.ToInt32(m_htGVColWidths[de.Value]);
                TableCell tcCurrent = e.Row.Cells[currentColIndex];
                if (tcCurrent.Controls.Count == 0)
                {
                    string actualString = DataBinder.Eval(e.Row.DataItem, colName).ToString();
                    //Remove the File Time Stamps in Help Authoring Page.
                    if (ObjCommonUI.IsHelpAuthPage)
                    {
                        if (colName == "HelpFile")
                        {
                            actualString = ObjCommonUI.TrimTimeStamp(actualString);
                            tcCurrent.Text = actualString;
                        }
                    }

                    if (actualString.Trim().Length > 0)//Add the ToolTip only if is not empty.
                    {
                        tcCurrent.ToolTip = actualString;
                    }
                    if (actualString.Length <= colLength)
                    {
                        continue;
                    }

                    string collapsedString = actualString.Substring(0, colLength - 3) + "...";
                    if (tcCurrent.Text.StartsWith("<a") && tcCurrent.Text.EndsWith("</a>"))
                    {
                        int startIndex = tcCurrent.Text.IndexOf(">") + 1;
                        int endIndex = tcCurrent.Text.IndexOf("</a>", startIndex);
                        //string strToInsert = tcCurrent.Text.Substring(startIndex, endIndex - startIndex);
                        if (collapsedString.Length > colLength)
                        {
                            tcCurrent.Text = tcCurrent.Text.Remove(startIndex, endIndex - startIndex);
                            //strToInsert = strToInsert.Substring(0, colLength - 3) + "...";
                            //tcCurrent.Text = tcCurrent.Text.Insert(startIndex, strToInsert);
                            tcCurrent.Text = tcCurrent.Text.Insert(startIndex, collapsedString);
                        }
                    }
                    else
                    {
                        //if (tcCurrent.Text.Length > colLength)
                        //{
                        tcCurrent.Text = collapsedString;
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Creates row effects like setting colors and disabling as specified in the XML.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        /// <param name="drvCurrent">Current grid view row.</param>
        private void SetRowColours(GridViewRowEventArgs e, DataRowView drvCurrent, ImageButton imgBtnTTnNavigate)
        {
            if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() == "1")
            {
                //Disable the row and show a lock symbol
                //e.Row.Enabled = false;
                for (int index = m_NoOfGVStaticColumns; index < e.Row.Cells.Count; index++)
                {
                    e.Row.Cells[index].Enabled = false;
                }
                imgBtnTTnNavigate.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid_lock.png";
            }
            if (m_IndexOfTypeOfInactiveStatusID != -1)
            {
                switch (drvCurrent.Row.ItemArray[m_IndexOfTypeOfInactiveStatusID].ToString())
                {
                    case "1":
                        {
                            //Uncommitted
                            e.Row.BackColor = Color.Red;
                            break;
                        }
                    case "2":
                        {
                            //Deleted
                            e.Row.BackColor = Color.Brown;
                            break;
                        }
                    case "3":
                        {
                            //Waiting for SOX Approval
                            e.Row.BackColor = Color.Violet;
                            break;
                        }
                    case "4":
                        {
                            //Closed
                            e.Row.BackColor = Color.MintCream;
                            break;
                        }
                    case "5":
                        {
                            //Auto save
                            e.Row.BackColor = Color.Green;
                            break;
                        }
                    case "6":
                        {
                            //Not Used
                            e.Row.BackColor = Color.LightYellow;
                            break;
                        }
                    case "7":
                        {
                            //Waiting Approval
                            e.Row.BackColor = Color.LightBlue;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
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
        /// Trigger event for the GridView selection changed event.
        /// </summary>
        /// <param name="sender">ImageButton.</param>
        /// <param name="e">Event Arguments.</param>
        protected void imgBtnTTnNavigate_Click(object sender, ImageClickEventArgs e)
        {
            string[] arrSplitParams = { "~::~" };
            string[] formInfoSplit = hdnFormInfo.Value.Split(arrSplitParams, StringSplitOptions.RemoveEmptyEntries);
            GridViewRow gvrCurrent = (GridViewRow)((ImageButton)sender).NamingContainer;
            string navigatePage = formInfoSplit[1];
            string thisPage = HttpContext.Current.Request.ServerVariables["URL"].Trim();

            //Hidden variable to store selected row index which can be used to get previous/next record.
            hdnSelectedRowIndex.Value = gvrCurrent.RowIndex.ToString();

            string[] splitCurrPageName = thisPage.Split('/');
            string currAspxParentFolder = splitCurrPageName[splitCurrPageName.Length - 2];
            string currAspxPageName = splitCurrPageName[splitCurrPageName.Length - 1];

            string[] splitNavPageName = navigatePage.Split('/');
            string navAspxParentFolder = splitNavPageName[splitNavPageName.Length - 2];
            string navAspxPageName = splitNavPageName[splitNavPageName.Length - 1];

            string BPGID = formInfoSplit[0];
            string containerRowXML = ((HiddenField)gvrCurrent.FindControl("hdnRowInfo")).Value;
            XmlDocument xDocRowInfoXml = new XmlDocument();
            xDocRowInfoXml.LoadXml(containerRowXML);
            string TrxID = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TrxID"].Value;
            string TrxType = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TrxType"].Value;
            string COCaption = string.Empty;
            if (grdVwContents.Columns.Count > m_NoOfGVStaticColumns)
            {
                //Assign the first column cell's text to the caption.
                COCaption = gvrCurrent.Cells[m_NoOfGVStaticColumns].ToolTip;
            }
            string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + navigatePage + "</PageInfo><Caption>" + m_ObjCommonUI.CharactersToHtmlCodes(COCaption) + "</Caption></CallingObject>";
            string BPInfo = "<bpinfo><BPGID>" + BPGID + "</BPGID>" + containerRowXML + callingObjXML + "</bpinfo>";
            hdnGVBPEINFO.Value = containerRowXML + callingObjXML;//Set the BPEinfo so that it can be later used by form-level process link functions

            Control pnlParent = this.Parent;
            int loopCntr = 0;
            while (pnlParent.ID != "pnlGVContent" && loopCntr < 5)//Loop for not more than 5 loops
            {
                if (pnlParent.Parent != null)
                {
                    pnlParent = pnlParent.Parent;
                }
                loopCntr++;
            }
            if (pnlParent.ID == "pnlGVContent")
            {
                ((Panel)pnlParent).Style["display"] = "none";
            }

            Panel cntrlpnlEntryFormobj = null;
            if (currAspxParentFolder.ToUpper().Trim() == navAspxParentFolder.ToUpper().Trim()
                && currAspxPageName.ToUpper().Trim() == navAspxPageName.ToUpper().Trim())
            {
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
                UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");

                //To update only the specific part of the page i.e controls in updtPnlContent.
                if (cntrlUpdtPnl.UpdateMode == UpdatePanelUpdateMode.Conditional)
                {
                    cntrlUpdtPnl.Update();
                }

                cntrlpnlEntryFormobj = (Panel)cntPlaceHolder.FindControl("pnlEntryForm");
                cntrlpnlEntryFormobj.Attributes.Add("style", "DISPLAY: Block;");

                //Set the Page Level hidden field with current row XML
                //hdnSelectedRows.Value = BPInfo;

                //XmlDocument xDocRowInfo = new XmlDocument();
                //xDocRowInfo.LoadXml(hdnSelectedRows.Value);
                //XmlNode xNodeRow = xDocRowInfo.SelectSingleNode("bpinfo//RowList/Rows");//Changed by Danny due to BUG:33031.Previous XPath-"bpinfo/" + BtnsUserCtrl.TreeNode + "/RowList/Rows"
                XmlNode xNodeRow = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows");
                if (xNodeRow.Attributes["ToolTipInfo"] != null)
                {
                    xNodeRow.Attributes.Remove(xNodeRow.Attributes["ToolTipInfo"]);
                }

                BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
                BtnsUserCtrl.RwToBeModified = xNodeRow.OuterXml;

                string parentTrxID = string.Empty;
                if (xNodeRow != null)
                {
                    if (xNodeRow.Attributes["TrxID"] != null)
                    {
                        parentTrxID = xNodeRow.Attributes["TrxID"].Value;
                    }
                }

                m_ObjCommonUI.ButtonsUserControl = BtnsUserCtrl;
                clsBranchUI objBranchUI = new clsBranchUI();
                objBranchUI.ObjCommonUI = m_ObjCommonUI;
                //Setting BranchXML
                objBranchUI.SetBranchXML(BtnsUserCtrl.GVDataXml.ToString(), parentTrxID);

                if (this.FormTempData.Length == 0)
                {
                    this.FormTempData = BtnsUserCtrl.GVDataXml;
                }
                XmlDocument xDocBtnsGVDataXML = new XmlDocument();
                xDocBtnsGVDataXML.LoadXml(BtnsUserCtrl.GVDataXml);
                m_ObjCommonUI.GridViewUserControl = this;
                m_ObjCommonUI.XDocFormXML = xDocBtnsGVDataXML;//xdc
                m_ObjCommonUI.GVSelectedRow = xNodeRow;
                string treeNode = BtnsUserCtrl.TreeNode;

                HiddenField hdnCurrAction = (HiddenField)BtnsUserCtrl.FindControl("hdnCurrAction");
                hdnCurrAction.Value = "Select";

                Label lblMessage = (Label)cntPlaceHolder.FindControl("lblmsg");
                lblMessage.Text = string.Empty;

                //To handle all the form links including sox approval.
                if (BtnsUserCtrl != null)
                {
                    BtnsUserCtrl.HandleFormLinks(cntrlpnlEntryFormobj, xDocBtnsGVDataXML);
                }

                //Reset the note attachment secure image based on data availabity
                BtnsUserCtrl.NoteAttachSecurePicChange(hdnCurrAction.Value);

                //Only to fill all controls
                m_ObjCommonUI.EnableDisableAndFillUI(false, cntrlpnlEntryFormobj, xDocBtnsGVDataXML, xNodeRow, "DISABLEMODE", true);

                //Only to disable all controls client side by invoking a common javascript. Added by shanti(20/02/08)
                m_ObjCommonUI.InvokeOnButtonClick("Select", this.Page);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShwNavRwBtns", "javascript:ShowNavRwBtns('Block');", true);

                //Find the ChildGV User Control and bind it.
                UserControls.ChildGridView CGVUC = (UserControls.ChildGridView)cntPlaceHolder.FindControl("CGVUC");
                if (CGVUC != null)
                {
                    CGVUC.PageIndex = 0;
                    CGVUC.ResetRowsToDisplay();
                    CGVUC.InitialiseBranchGrid(xDocBtnsGVDataXML, (Control)sender);
                }

                //ardescription childgrid usage for arinvoicedetails.aspx
                UserControls.ChildGridView CGVUCArdescription = (UserControls.ChildGridView)cntPlaceHolder.FindControl("CGVUCArdescription");
                if (CGVUCArdescription != null)
                {
                    CGVUCArdescription.PageIndex = 0;
                    CGVUCArdescription.ResetRowsToDisplay();
                    CGVUCArdescription.InitialiseBranchGrid(xDocBtnsGVDataXML, (Control)sender);
                }


                //Showing data in html table.
                ShowHtmlVw(cntrlpnlEntryFormobj, parentTrxID, xDocBtnsGVDataXML, treeNode);

                //Checking whether valid BPGID's exist for current row. script commented by shanti on 18/08/09. Handled in Btns.js
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ValidateBPGIDRow", "ValidateBPGIDRow();", true);
            }
            else
            {
                //Set a session variable with the current row XML and redirect to the navigate page.
                Session["BPINFO"] = BPInfo;//Valid
                navigatePage = "../" + navigatePage;
                if (this.Page.ToString().Contains("detailedview"))
                {
                    //Close the DetailedView IFrame first and then Change the URL to the target page which is done 
                    //through the below function.
                    string js = "javascript:CloseDetailedView('" + navigatePage + "');"; //JS function can be found in DetailedView.aspx
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseDBDetailedView", js, true);
                }
                else
                {
                    Response.Redirect(navigatePage);
                }
            }

            //Set the TypeOfJob images in Projects & ProjectTypes Pages
            if (this.Page.ToString() == "ASP.financials_project_aspx" || this.Page.ToString() == "ASP.financials_jobtype_aspx")
            {
                LAjitControls.LAjitDropDownList ddlTypeOfJob = (LAjitControls.LAjitDropDownList)cntrlpnlEntryFormobj.FindControl("ddlTypeOfJob");
                string typeOfJobTrxId = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TypeOfJob_TrxID"].Value;
                string typeOfJobTrxType = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TypeOfJob_TrxType"].Value;
                XmlDocument xDocGVData = new XmlDocument();
                xDocGVData.LoadXml(BtnsUserCtrl.GVDataXml);
                XmlNode nodeDDLRow = xDocGVData.SelectSingleNode("Root/bpeout/FormControls/TypeOfJob/RowList/Rows[@TrxID='" + typeOfJobTrxId + "' and @TrxType='" + typeOfJobTrxType + "']");
                //Find the Image
                LAjitControls.LAjitImage imgTypeOfJob = (LAjitControls.LAjitImage)cntrlpnlEntryFormobj.FindControl("imgTypeOfJob");
                imgTypeOfJob.Visible = true;
                if (nodeDDLRow.Attributes["ImgSrcLarge"] != null)//Ref:Project.aspx.cs>SetTypeOfJobHdnValue()
                {
                    string imageSrc = nodeDDLRow.Attributes["ImgSrcLarge"].Value;
                    imgTypeOfJob.Attributes.Add("style", "DISPLAY: Block;");
                    imgTypeOfJob.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/" + imageSrc;
                }
                else
                {
                    imgTypeOfJob.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/spacer.gif";
                    imgTypeOfJob.Attributes.Add("style", "DISPLAY: none;");
                }
            }

            //Enable memo controls in Voidcheckhistory page exclusive
            if (this.Page.ToString() == "ASP.payables_voidcheckhistory_aspx")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "EnableMemoControls", "JavaScript:EnableMemoControls();", true);
            }
        }

        public void ShowHtmlVw(Panel cntrlpnlEntryFormobj, string parentTrxID, XmlDocument xDocBtnsGVDataXML, string treeNode)
        {
            //To distinguish UI in FullView Page(Done by Shanti).
            HtmlTableRow trHTML = (HtmlTableRow)cntrlpnlEntryFormobj.FindControl("trHTML");
            //HtmlTableRow trDummy = (HtmlTableRow)cntrlpnlEntryFormobj.FindControl("trDummy");
            if (trHTML != null)
            {
                trHTML.Visible = true;
                //Panel pnlGVEditable = (Panel)cntrlpnlEntryFormobj.FindControl("pnlGVEditable");
                //pnlGVEditable.Visible = false;
                //if (trDummy != null)
                //{
                //    trDummy.Visible = false;
                //}
                Literal lit = (Literal)cntrlpnlEntryFormobj.FindControl("litRwTbl");
                System.Text.StringBuilder sbTable = new System.Text.StringBuilder();

                //XmlDocument xDoc = new XmlDocument();
                //xDoc.LoadXml(BtnsUserCtrl.GVDataXml.ToString());

                //Columns Reading from XML
                XmlNode nodeColumns = xDocBtnsGVDataXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns");

                sbTable.Append("<table cellpading='0' width='100%' cellspacing='0' border='1' bordercolor='black' rules='all' class='white-left'>");

                //Rows Reading from XML
                XmlNode nodeRows = xDocBtnsGVDataXML.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList//Rows[@TrxID=" + parentTrxID + "]");

                //Binding rows to dataset
                DataSet dsReports = new DataSet();
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                dsReports.ReadXml(read);

                foreach (XmlNode columnNode in nodeColumns.ChildNodes)
                {
                    if (columnNode.Attributes["Caption"].Value != " ")
                    {
                        sbTable.Append("<tr class='myreportHeaderText'>");
                        sbTable.Append("<td NOWRAP> <b>" + columnNode.Attributes["Caption"].Value + "</b></td>");
                        if (dsReports.Tables[0].Columns.Contains(columnNode.Attributes["Label"].Value))
                        {
                            sbTable.Append("<td NOWRAP>" + dsReports.Tables[0].Rows[0][columnNode.Attributes["Label"].Value].ToString() + "</td>");
                        }
                        else
                        {
                            sbTable.Append("<td>&nbsp;</td>");
                        }
                        sbTable.Append("</tr>");
                    }
                }
                sbTable.Append("</table>");
                if (lit != null)
                {
                    lit.Text = sbTable.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the specified cell contents.
        /// </summary>
        /// <param name="dtSearch">The search table</param>
        /// <param name="colName">The Name of the column.</param>
        /// <param name="rowIndex">The index of the row.</param>
        /// <returns>String cell contents.</returns>
        private string GetCellValue(DataTable dtSearch, string colName, int rowIndex)
        {
            int colCntr = 0;
            foreach (DataColumn dc in dtSearch.Columns)
            {
                if (dc.ColumnName == colName)
                {
                    break;
                }
                colCntr++;
            }
            return dtSearch.Rows[rowIndex][colCntr].ToString();
        }

        /// <summary>
        /// Gets the DDL object with the data to be bound for the IsLink column drop down lists.
        /// </summary>
        /// <param name="colName">The name of the column where the DDL is being added.</param>
        /// <param name="primaryKeyName">The key based upon whick the data has to be picked up.</param>
        /// <param name="selectedIndex">The selected index in the data.</param>
        /// <param name="primaryKeyValue">The primary key column name in the data in Gridview(Ex: TrxID.</param>
        /// <param name="selectedIndexValue">The second column name in the gridview(ex: TrxType).</param>
        /// <returns>DropDownList  object.</returns>
        private DropDownList GetIsLinkDDL(string colName, string primaryKeyName, string selectedIndex, string primaryKeyValue, string selectedIndexValue)
        {
            DropDownList ddlCurrent = new DropDownList();
            ddlCurrent.Width = Unit.Pixel(80);
            XmlDocument xdoc = new XmlDocument();
            if (GridViewInitData != string.Empty)
            {
                xdoc.LoadXml(GridViewInitData);
            }
            else
            {
                xdoc.LoadXml(m_ReturnXML);
            }
            string xPath = "Root/bpeout/FormControls/" + colName + "//Rows[@" + primaryKeyName + "='" + primaryKeyValue + "']";
            XmlNodeList nodeLstFiltered = xdoc.SelectNodes(xPath);
            foreach (XmlNode nodeDDLRow in nodeLstFiltered)
            {
                string ddlText = nodeDDLRow.Attributes["Description"].Value;
                string ddlValue = nodeDDLRow.Attributes[selectedIndex].Value;
                ddlCurrent.Items.Add(new ListItem(ddlText, ddlValue));
            }
            return ddlCurrent;
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
            logger.Info("Getting the BPGID of the Businees Process Name : "+processName);
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
        /// Generates the request xml to be sent to the XML processor
        /// </summary>
        /// <returns>string XML</returns>
        private string GenerateRequestXML(string pageNo, string sortColumn, string sortOrder)
        {
            #region NLog
            logger.Info("Generating the request xml to be sent to the XML processor with pageNo : "+pageNo+" and sort column : "+sortColumn+" and sort Order :"+sortOrder);
            #endregion

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            XmlNode nodeRoot = XDocUserInfo.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = strBPE;
            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(GridViewBPInfo);
            //string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
            string defaultPageSize = grdVwContents.PageSize.ToString();
            XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("bpinfo//Gridview");
            if (nodeGridView == null)
            {
                XmlNode xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                xNodeTreeNode.InnerXml = GetGridViewNodeXML(pageNo, sortColumn, sortOrder, defaultPageSize);

                xDocBPEInfo.ChildNodes[0].AppendChild(xNodeTreeNode);
            }
            else
            {
                nodeGridView.SelectSingleNode("Pagesize").InnerText = defaultPageSize;// grdVwContents.PageSize.ToString();
                nodeGridView.SelectSingleNode("Pagenumber").InnerText = pageNo;
                nodeGridView.SelectSingleNode("Sortcolumn").InnerText = sortColumn;
                nodeGridView.SelectSingleNode("Sortorder").InnerText = sortOrder;

                if (nodeGridView.ParentNode.LocalName != hdnGVTreeNodeName.Value)
                {
                    //Then embed the GridView node into a New Node named as the Tree Node.
                    XmlNode xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                    //Remove the GridView from the current parent(bpinfo) and append it as child to the above node.
                    nodeGridView.ParentNode.RemoveChild(nodeGridView);
                    xNodeTreeNode.AppendChild(nodeGridView);
                    xDocBPEInfo.DocumentElement.AppendChild(xNodeTreeNode);
                }
            }

            ////Adding the PriorFormInfo XML.
            //string formInfo = Session["PriorFormInfo"].ToString();
            //XmlNode xNodePFI = xDocBPEInfo.FirstChild.SelectSingleNode("PriorFormInfo");
            //if (xNodePFI != null)
            //{
            //    xNodePFI.InnerXml = formInfo.Replace("<FormInfo>", "").Replace("</FormInfo>", "");
            //}
            //else
            //{
            //    xDocBPEInfo.FirstChild.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");
            //}

            nodeRoot.InnerXml += xDocBPEInfo.OuterXml;

            //Updating the BPEINFO with the latest version.
            GridViewBPInfo = xDocBPEInfo.OuterXml;
            //hdnSearchBPInfo.Value = xDocBPEInfo.OuterXml;
            //HttpContext.Current.Session["BPINFO"] = xDocBPEInfo.OuterXml;
            SessionLinkBPInfo = xDocBPEInfo.OuterXml;
            CommonBO objBO = new CommonBO();
            //GridViewInitData  
            string strOutXml = objBO.GetVendorHistory(nodeRoot.OuterXml);

            if (this.Page.Master == null)
            {
                return strOutXml;
            }

            Control ctrlUP = this.Page.Master.FindControl("cphPageContents");
            if (ctrlUP != null)
            {
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)ctrlUP;
                BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
            }
            if (BtnsUserCtrl != null)
            {
                ((HiddenField)BtnsUserCtrl.FindControl("parentBPInfo")).Value = xDocBPEInfo.OuterXml;
            }

            if (this.Page.Master != null)
            {
                if (this.ID != "GVUC")
                {
                    return strOutXml;
                }

                //if (cntPlaceHolder != null)
                {
                    //BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
                    if (BtnsUserCtrl != null)
                    {
                        //BtnsUserCtrl.GVDataXml = GridViewInitData;
                        XmlDocument Xdoc = new XmlDocument();
                        Xdoc.LoadXml(BtnsUserCtrl.GVDataXml);
                        XmlDocument returnXML = new XmlDocument();
                        returnXML.LoadXml(strOutXml);
                        XmlNode nodeMessageInfo = returnXML.SelectSingleNode("Root/bpeout/MessageInfo");
                        if (nodeMessageInfo != null && nodeMessageInfo.InnerText != "Error")
                        {
                            string gridTreenode = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                            if (gridTreenode.ToUpper().Trim() == BtnsUserCtrl.TreeNode.ToUpper().Trim())
                            {
                                XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/RowList");
                                nodeRowList.RemoveAll();
                                XmlNode nodeResRowList = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/RowList");
                                nodeRowList.InnerXml += nodeResRowList.InnerXml;

                                //Updating the Total Page Size
                                XmlNode nodeNewRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/RowList");
                                if (nodeNewRowList != null)
                                {
                                    XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/Gridresults");
                                    nodeGridResults.RemoveAll();
                                    XmlNode nodeResGridResults = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/Gridresults");
                                    nodeGridResults.InnerXml += nodeResGridResults.InnerXml;
                                }
                                //Updating GvDataXml with Parent xml
                                BtnsUserCtrl.GVDataXml = Xdoc.OuterXml;
                            }
                            //Updating BranchXml
                            //Branch nodes 
                            XmlNode nodeBranches = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                            if (nodeBranches != null)
                            {
                                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                {
                                    //if (nodeBranch.Attributes["ControlType"] == null)
                                    {
                                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                                        //Creating branch node if not present in GVDataXMl
                                        if (Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList") == null)
                                        {
                                            //Creating the Row List node
                                            XmlNode nodeBranchRowList = Xdoc.CreateNode(XmlNodeType.Element, "RowList", null);
                                            //Appending Row List Node
                                            Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName).AppendChild(nodeBranchRowList);
                                        }
                                        //Updating child Rowlist                                        
                                        XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                        nodeRowList.RemoveAll();
                                        XmlNode nodeResRowList = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                        if (nodeResRowList != null)
                                        {
                                            nodeRowList.InnerXml += nodeResRowList.InnerXml;
                                        }

                                        //Updating the  child Total Page Size
                                        XmlNode nodeNewRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                        if (nodeNewRowList != null)
                                        {
                                            XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                            nodeGridResults.RemoveAll();
                                            XmlNode nodeResGridResults = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                            nodeGridResults.InnerXml += nodeResGridResults.InnerXml;
                                        }
                                    }
                                }
                                //Updating GvDataXml with branch xml
                                BtnsUserCtrl.GVDataXml = Xdoc.OuterXml;
                            }
                        }
                    }
                }
            }
            return strOutXml;
        }

        private static string GetGridViewNodeXML(string pageNo, string sortColumn, string sortOrder, string defaultPageSize)
        {
            return @"<Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>"
                + defaultPageSize + "</Pagesize><Sortcolumn>" + sortColumn
                + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder></Gridview>";
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

            m_CurrPageNo = 1;
            if (hdnCurrPageNo.Value != "1")
            {
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();
                string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
                DisplayFoundResults(result);
            }
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;

            LoadQuickSearchImage();
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

            m_CurrPageNo = Convert.ToInt32(hdnCurrPageNo.Value);
            if (m_CurrPageNo != 1)
            {
                m_CurrPageNo--;
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();

            }
            string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
            DisplayFoundResults(result);
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;

            LoadQuickSearchImage();
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

            m_CurrPageNo = Convert.ToInt32(hdnCurrPageNo.Value);
            m_MaxPages = Convert.ToInt32(hdnMaxPages.Value);
            if (m_CurrPageNo != m_MaxPages)
            {
                m_CurrPageNo++;
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();
            }
            string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
            DisplayFoundResults(result);
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;

            LoadQuickSearchImage();
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

            m_CurrPageNo = Convert.ToInt32(hdnMaxPages.Value);
            if (hdnCurrPageNo.Value != hdnMaxPages.Value)
            {
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();
                string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
                DisplayFoundResults(result);
            }
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;

            LoadQuickSearchImage();
        }

        /// <summary>
        /// Page Selected Index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //To balance serverside and clientside
            m_ObjCommonUI.InvokeOnButtonClick("PageLoad", this.Page);

            string sortDirection = string.Empty;
            hdnCurrPageNo.Value = ddlPages.SelectedItem.ToString();
            if (grdVwContents.SortDirection == SortDirection.Ascending)
            {
                sortDirection = "ASC";
            }
            else
            {
                sortDirection = "DESC";
            }

            string result = GenerateRequestXML(ddlPages.SelectedItem.ToString(), ViewState["SORTEXP"].ToString(), sortDirection);
            DisplayFoundResults(result);

            LoadQuickSearchImage();

        }

        //protected void imgBtnPDF_Click(object sender, ImageClickEventArgs e)
        //{
        //    //GridReports objGridReports = new GridReports();
        //    //objGridReports.BPOut = GridViewInitData;
        //    //objGridReports.BPInfo = GridViewBPInfo;
        //    //objGridReports.PrintData(ReportType.PDF, PagesToPrint.Current);

        //    //string[] arrReportStyle = { "0", "1", "2", "3", "4", "5", "100", "200" };

        //    //XmlDocument xDocOut = new XmlDocument();
        //    //xDocOut.LoadXml(GridViewInitData);
        //    //XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
        //    //XmlAttribute attSTyle = xDocOut.CreateAttribute("ReportStyle");
        //    //attSTyle.Value = arrReportStyle[3];
        //    //nodeTreenode.Attributes.Append(attSTyle);
        //    //objGridReports.BPOut = xDocOut.OuterXml;
        //    //objGridReports.BPInfo = GridViewBPInfo;
        //    //objGridReports.PrintData(ReportType.PDF, PagesToPrint.Current);


        //}

        //protected void imgBtnExcel_Click(object sender, ImageClickEventArgs e)
        //{

        //    //GridReports objGridReports = new GridReports();
        //    //objGridReports.BPOut = GridViewInitData;
        //    //objGridReports.BPInfo = GridViewBPInfo;
        //    //objGridReports.PrintData(ReportType.Excel, PagesToPrint.All);
        //}

        //protected void imgBtnHtml_Click(object sender, ImageClickEventArgs e)
        //{

        //    //GridReports objGridReports = new GridReports();
        //    //objGridReports.BPOut = GridViewInitData;
        //    //objGridReports.BPInfo = GridViewBPInfo;
        //    //objGridReports.PrintData(ReportType.HTML, PagesToPrint.All);
        //}

        //protected void imgBtnPDFCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.PDF, PagesToPrint.Current);
        //}

        //protected void imgBtnPDFAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.PDF, PagesToPrint.All);
        //}

        //protected void imgBtnExcelCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.Excel, PagesToPrint.Current);
        //}

        //protected void imgBtnExcelAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.Excel, PagesToPrint.All);
        //}

        //protected void imgBtnMSWord_Click(object sender, ImageClickEventArgs e)
        //{
        //}

        //protected void imgBtnWordCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.Word, PagesToPrint.Current);
        //}

        //protected void imgBtnWordAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.Word, PagesToPrint.All);
        //}

        //protected void imgBtnHTMLCurrent_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.HTML, PagesToPrint.Current);
        //}

        //protected void imgBtnHTMLAll_Click(object sender, EventArgs e)
        //{
        //    GridReports objGridReports = new GridReports(ObjCommonUI);
        //    objGridReports.BPOut = GridViewInitData;
        //    objGridReports.BPInfo = GridViewBPInfo;
        //    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
        //    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
        //    objGridReports.PrintData(ReportType.HTML, PagesToPrint.All);
        //}
    }
}
