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
using LAjit_BO;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using NLog;

// Namespace for PDF generation
using Gios.Pdf;

namespace LAjitDev
{
    public enum PCGridViewType
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

    public partial class ParentChildGV : System.Web.UI.UserControl
    {
        private const int m_NoOfGVStaticColumns = 3;
        private const int m_NoOfChildGVStaticColumns = 1;
        private int m_CurrPageNo;
        private int m_MaxPages;
        private string m_BPCXml;
        private string m_primaryKeyFieldName;
        private string m_HyperLinksEnabled = string.Empty;
        private string m_RowHoverColour = string.Empty;
        int m_IndexOfIsProtected;
        int m_IndexOfIsActive;
        int m_IndexOfTypeOfInactiveStatusID;
        bool m_IsFirstRun = true;
        bool m_IsRowAlternating = false;
        XmlDocument XDocUserInfo = new XmlDocument();

        /// <summary>
        /// Contains the Session["GBPC"]/BusinessProcessControls string.
        /// </summary>
        //string m_GBPCXml = string.Empty;
        string m_BusinessRules = string.Empty;
        string m_GVTreeNodeName = string.Empty;
        private Hashtable m_htBPCntrls = new Hashtable();
        private Hashtable m_htColLinkDDL = new Hashtable();
        private string m_ReturnXML = string.Empty;
        private Hashtable m_htBPCColumns = new Hashtable();

        private Hashtable m_htChildBPCntrls = new Hashtable();
        private Hashtable m_htChildBPCColumns = new Hashtable();

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

        //Store the current BPGID of the return XML(For the view details functionality)
        private string m_CurrentBPGID = string.Empty;

        private string m_GridInitData;

        //Store the current Page Info of the return XML(For the view details functionality)
        private string m_CurrentPageInfo = string.Empty;

        private XmlDocument m_XDOCBPE;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        CommonUI m_ObjCommonUI = new CommonUI();

        protected LAjitDev.UserControls.ButtonsUserControl ucButtonsUserControl;

        /// <summary>
        /// Holds the reference of the CommonUI class instance created in the BasePage.cs.
        /// </summary>
        public CommonUI ObjCommonUI
        {
            get { return m_ObjCommonUI; }
            set { m_ObjCommonUI = value; }
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
            //get { return ViewState["GridViewInitData"].ToString(); }
            //set { ViewState["GridViewInitData"] = value; }
            get { return m_GridInitData; }
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

        public PCGridViewType PCGridViewType
        {
            set { ViewState["PCGridViewType"] = value; }
            get { return (PCGridViewType)ViewState["PCGridViewType"]; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.IsPostBack && hdnMainViewState.Value == "1")
            //{
            //    Control ctrlPostBack = CommonUI.GetPostBackControl(Page);
            //    //if (ctrlPostBack.NamingContainer.ToString().Contains("gridviewcontrol"))
            //    if (ctrlPostBack.NamingContainer != this)
            //    {
            //        this.DataBind();
            //    }
            //}
            //this.DataBind();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ChangeDisplays", "RevertStyles('" + this.ClientID + "')", true);
            //restore_columns();

            PreloadImages();
        }

        private void PreloadImages()
        { 
            string cdnImagePath=Application["ImagesCDNPath"].ToString();
            imgBtnPrint.ImageUrl = cdnImagePath + "Images/grid-print.png";
            imgBtnChart.ImageUrl = cdnImagePath + "Images/grid-chart.png";
            imgBtnPDF.ImageUrl = cdnImagePath + "Images/grid-pdf.png";
            imgBtnExcel.ImageUrl = cdnImagePath + "Images/grid-excel.png";
            imgBtnHtml.ImageUrl = cdnImagePath + "Images/grid-html.png";
            imgBtnMSWord.ImageUrl = cdnImagePath + "Images/Grid_Word.png";
            imgBtnEmailPDF.ImageUrl = cdnImagePath + "Images/email_icon2.png";
        }


        //protected override void LoadViewState(object savedState)
        //{
        //    DataControlFieldCollection collection1;
        //    collection1 = grdVwContents.Columns.CloneFields();
        //    base.LoadViewState(savedState);
        //    grdVwContents.Columns.Clear();
        //    foreach (DataControlField field in collection1)
        //    {
        //        grdVwContents.Columns.Add(field);
        //    }
        //}

        //protected void restore_columns()
        //{
        //    DataControlFieldCollection columns = (DataControlFieldCollection)Cache["cols"];
        //    grdVwContents.Columns.Clear();	// gvSchluessel is the GridView
        //    foreach (DataControlField field in columns)
        //    {
        //        grdVwContents.Columns.Add(field);
        //        if (field.GetType() == typeof(TemplateField))
        //        {
        //            TemplateField tf = (TemplateField)field;
        //            int i = grdVwContents.Columns.IndexOf(tf);
        //            if (tf.HeaderTemplate != null)
        //            {
        //                tf.HeaderTemplate.InstantiateIn(grdVwContents.HeaderRow.Cells[i]);
        //            }
        //            foreach (GridViewRow row in grdVwContents.Rows)
        //            {
        //                if (tf.ItemTemplate != null)
        //                {
        //                    tf.ItemTemplate.InstantiateIn(row.Cells[i]);
        //                }
        //            }
        //            if (tf.FooterTemplate != null)
        //            {
        //                tf.FooterTemplate.InstantiateIn(grdVwContents.FooterRow.Cells[i]);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Binds the first set of data to the user control without hitting the BPE.
        /// </summary>
        public override void DataBind()
        {
            #region NLog
            logger.Info("Binds the first set of data to the user control without hitting the BPE.");
            #endregion
            
            LoadGVType();
            ViewState["SORTDIRECTION"] = "ASC";
            ViewState["SORTEXP"] = "";
            hdnPreviewInPopup.Value = "0";//TODO:Get it from web.config.
            if (GridViewInitData == null)
            {
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
                if (GridViewInitData == null)
                {
                    return;
                }
            }
            //Getting the returnXml from the BPE which also consists of the gridview contents.
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(GridViewInitData);

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeColumns = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

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

            ////
            ////Creating the Columns.
            //int indx = 4;
            //foreach (XmlNode colNode in nodeColumns.ChildNodes)
            //{
            //    if (colNode.Attributes["FullViewLength"].Value != "0" && dsRowList.Tables[0].Columns.Contains(colNode.Attributes["Label"].Value))
            //    {
            //        BoundField newField = new BoundField();
            //        newField.DataField = colNode.Attributes["Label"].Value;
            //        newField.HeaderText = colNode.Attributes["Caption"].Value;
            //        newField.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            //        //newField.ItemStyle.CssClass = "myreportItemText";
            //        //grdVwContents.Columns.Add(newField);
            //        grdVwContents.Columns.Insert(indx, newField);
            //        indx++;
            //    }
            //}
            ////

            int colCntr = 0;
            int indx = m_NoOfGVStaticColumns - 1;
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
                        BoundColumn newField = new BoundColumn();
                        newField.DataField = colNode.Attributes["Label"].Value;
                        newField.HeaderText = colNode.Attributes["Caption"].Value;
                        newField.ItemStyle.Wrap = false;

                        //For Right-Justify issue
                        BoundColumn dummyField = null;//A dummy field at the end of the columns to fill in the gap
                        bool addDummyField = false;

                        if (PCGridViewType != PCGridViewType.DashBoard && PCGridViewType != PCGridViewType.Master)//Don't set widths in Dash
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
                                    dummyField = new BoundColumn();
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
                        }

                        //Set justification to Right-Justify for columns with ControlType=Amount
                        if (colNode.Attributes["ControlType"] != null && colNode.Attributes["ControlType"].Value == "Amount")
                        {
                            newField.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            newField.ItemStyle.CssClass = "grdVwColRightJustify";
                        }
                        //grdVwContents.Columns.Add(newField);
                        //grdVwContents.Columns.Insert(indx, newField);
                        grdVwContents.Columns.AddAt(indx, newField);
                        indx++;
                        if (addDummyField)
                        {

                            //grdVwContents.Columns.Insert(indx, dummyField);
                            grdVwContents.Columns.AddAt(indx, dummyField);
                            indx++;
                        }
                        colCntr++;
                    }
                }
            }

            int defaultGridPageSize = 0;
            if (DefaultPageSize == "")//If the variable was not set externly.
            {   //DefaultGridSize
                defaultGridPageSize = Convert.ToInt32(GetUserPreferenceValue("59"));
            }
            else
            {
                defaultGridPageSize = Convert.ToInt32(DefaultPageSize);
            }
            grdVwContents.PageSize = defaultGridPageSize;
            //Calculating the page size of the grid view
            if (hdnCurrPageNo.Value == string.Empty)
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

            hdnMaxPages.Value = m_MaxPages.ToString();
            hdnGVTreeNodeName.Value = m_GVTreeNodeName;

            //Setting the Alternating Row Style of the GridView
            //FullViewAlternatingStyle
            string isAlternating = GetUserPreferenceValue("56");
            if (isAlternating != "1")
            {
                grdVwContents.AlternatingItemStyle.Reset();
                //ViewState["ALTRowStyle"] = string.Empty;
                m_IsRowAlternating = false;
            }
            else
            {
                m_IsRowAlternating = true;
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
            InitialisePagesDDL();
            DisplayFoundResults(GridViewInitData);
            LoadLinkbuttonImages();
            FillXYDropdownData(GridViewInitData);

            //Check for correct format of BPInfo.If GridView node is wrapped by the Tree Node
            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(GridViewBPInfo);
            XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("//Gridview");
            if (nodeGridView.ParentNode.LocalName != hdnGVTreeNodeName.Value)
            {
                //Then embed the GridView node into a New Node named as the Tree Node.
                XmlNode xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                //Remove the GridView from the current parent(bpinfo) and append it as child to the above node.
                nodeGridView.ParentNode.RemoveChild(nodeGridView);
                xNodeTreeNode.AppendChild(nodeGridView);
                xDocBPEInfo.DocumentElement.AppendChild(xNodeTreeNode);
                GridViewBPInfo = xDocBPEInfo.OuterXml;
                HttpContext.Current.Session["BPINFO"] = xDocBPEInfo.OuterXml;
            }

            //updtPnlGrdVw.Update();
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
            //string themeName = Convert.ToString(Session["MyTheme"]);
            string cdnImagesPath = Convert.ToString(Application["ImagesCDNPath"]);
            lbtnFirst.Text = "<img border=0 src="+cdnImagesPath + "images/arrow-first.gif>";
            lbtnPrevious.Text = "<img border=0 src="+ cdnImagesPath + "images/arrow-left.gif>";
            lbtnNext.Text = "<img border=0 src="+ cdnImagesPath + "images/arrow-right.gif>";
            lbtnLast.Text = "<img border=0 src="+ cdnImagesPath + "images/arrow-last.gif>";
        }

        /// <summary>
        /// To the change the ViewState["PCGridViewType"] to the actual XML node name from that of the enum value.
        /// </summary>
        private void LoadGVType()
        {
            switch ((PCGridViewType)ViewState["PCGridViewType"])
            {
                case PCGridViewType.DashBoard:
                    {
                        grdVwContents.AlternatingItemStyle.CssClass = "AlternatingRowStyle";
                        break;
                    }
                case PCGridViewType.COA:
                    {
                        trTitle.Visible = false;
                        break;
                    }
                case PCGridViewType.AccountingLayout:
                    {
                        trTitle.Visible = false;
                        break;
                    }
                case PCGridViewType.Master:
                    {
                        grdVwContents.AlternatingItemStyle.CssClass = "AlternatingRowStyle";
                        break;
                    }
                default:
                    {
                        grdVwContents.AlternatingItemStyle.CssClass = "AlternatingCOARowStyle";
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


            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);


            //xDoc.Load("c:\\treeviewBPschema.xml");
            //returnXML = xDoc.OuterXml;

            m_ReturnXML = returnXML;

            if (ViewState["GridViewInitData"] == null)
                ViewState["GridViewInitData"] = returnXML;

            //Initialising the variables m_CurrentBPGID and m_CurrentPageInfo for the view details functionality
            m_CurrentBPGID = xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            m_CurrentPageInfo = xDoc.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
            //Keep one variable for the entire formInfo..
            hdnFormInfo.Value = m_CurrentBPGID + "~::~" + m_CurrentPageInfo;

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

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
                grdVwContents.DataSource = new DataTable();
                grdVwContents.DataBind();
                pnlPagingCtrls.Visible = false;
                return;
            }

            m_htBPCntrls.Clear();
            m_htBPCColumns.Clear();
            m_htGVColumns.Clear();
            m_htGVColWidths.Clear();
            m_arrImageColIndices.Clear();
            m_arrAmountCols.Clear();

            int colCntr = 0;
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");
            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                //Adding the current column node the ROWS dataset if not present.
                if (!dsRowList.Tables[0].Columns.Contains(colNode.Attributes["Label"].Value))
                {
                    DataColumn dcNew = new DataColumn(colNode.Attributes["Label"].Value, typeof(string));
                    dcNew.AllowDBNull = true;
                    dsRowList.Tables[0].Columns.Add(dcNew);
                }

                int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)
                {
                    //For tooltip functionality.
                    m_htGVColumns.Add(colNode.Attributes["Label"].Value, colCntr);
                    //Add the Column Length
                    m_htGVColWidths.Add(colCntr, colFullViewLength);

                    XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                    if (attBPControl != null && attBPControl.Value.Trim() != string.Empty)
                    {
                        m_htBPCntrls.Add(attBPControl.Value.Trim(), GetColumnIndex(colNode.Attributes["Label"].Value, dsRowList.Tables[0]));
                        m_htBPCColumns.Add(attBPControl.Value.Trim(), colCntr);
                    }

                    XmlAttribute attControlType = colNode.Attributes["ControlType"];
                    if (attControlType != null)
                    {
                        //Check for ControlType to be Amount
                        if (attControlType.Value == "Amount")
                        {
                            m_arrAmountCols.Add(colNode.Attributes["Label"].Value);
                        }

                        //Check for Control Type to be DateTime
                        if (attControlType.Value == "Cal")
                        {
                            foreach (DataRow dr in dsRowList.Tables[0].Rows)
                            {
                                DateTime dateTime;
                                if (DateTime.TryParse(dr[colNode.Attributes["Label"].Value.Trim()].ToString(), out dateTime))
                                {
                                    dr[colNode.Attributes["Label"].Value.Trim()] = dateTime.ToString("MM/dd/yyyy");
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

            //Setting the title of the grid view container panel.
            SetPanelHeading(htcGridTitle, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Title").InnerText);

            //Initialising variables to be used in OnRowDataBound
            XmlNode xNodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (xNodeBPC != null)
            {
                m_BPCXml = xNodeBPC.OuterXml;
            }
            else
            {
                m_BPCXml = string.Empty;
            }

            m_primaryKeyFieldName = dsRowList.Tables[0].Columns[0].ColumnName;

            ////Initialising the Business rules used for ColumnHyperlinks enabling in the OnRowDataBound
            //xDoc = new XmlDocument();
            //xDoc.LoadXml(Session["BPE"].ToString());
            //XmlNode xNode = xDoc.SelectSingleNode("Root/bpeout/BusinessRules");
            //if (xNode != null)
            //{
            //    m_BusinessRules = xNode.OuterXml;
            //}
            //else
            //{
            //    m_BusinessRules = string.Empty;
            //}

            //Apply Row Hovering effects only if there is an alternating style applied.
            if (grdVwContents.AlternatingItemStyle.CssClass.Length > 0)
            {
                //Get the Row Hover Colour from the Config file which will be used in the RowDataBound event.
                m_RowHoverColour = ConfigurationManager.AppSettings["GridRowHoverColor"];
                m_IsRowAlternating = true;
            }

            DataTable dtToolTipData = UpdateToolTipInfo(dsRowList.Tables[0]);
            ////Set the Grid view's alternating row style
            //grdVwContents.AlternatingRowStyle.CssClass = Convert.ToString(ViewState["ALTRowStyle"]);

            grdVwContents.DataSource = dtToolTipData;
            grdVwContents.DataBind();
            if (hdnMaxPages.Value != "0")
            {
                ddlPages.SelectedIndex = Convert.ToInt32(hdnCurrPageNo.Value) - 1;
            }
            lblPageStatus.Text = String.Concat(hdnCurrPageNo.Value, " of ", hdnMaxPages.Value);
            lblPageNo.Text = "Go To Page";



            //updtPnlGrdVw.Update();
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
            logger.Info("Sets the title of the given grid view with : "+gridTitle);
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
            logger.Info("Getting the value of the business rule name as :"+ruleName+" and business rile node as : "+businessRulesNode +" from the given XML");
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
            logger.Info("Getting the value of the user preference from the given XML with rule id : "+bRuleID );
            #endregion

            XDocUserInfo = m_ObjCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
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
        /// On RowDataBound Event for grdVwContents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdVwContents_DataBound(object sender, DataGridItemEventArgs e)
        {
            DataRowView drvCurrent = (DataRowView)e.Item.DataItem;
            if (drvCurrent != null)//m_IsFirstRun
            {
                m_IsFirstRun = false;
                InitialiseIndexVariables(drvCurrent);
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Row Highlighting upon hover.
                if (m_IsRowAlternating)
                {
                    if (e.Item.ItemType == ListItemType.Item)
                    {
                        e.Item.CssClass = "GVRowHover";
                    }
                }
                else
                {
                    e.Item.CssClass = "GVRowHover";
                }

                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();

                if (m_HyperLinksEnabled.Length == 0)
                {
                    //FullViewHyperlinks
                    m_HyperLinksEnabled = GetUserPreferenceValue("58");
                }

                //Adding the row XML to the template fields.
                HiddenField hdnCurrentRow = (HiddenField)e.Item.Cells[0].FindControl("hdnRowInfo");
                if (hdnCurrentRow == null)
                {
                    hdnCurrentRow = new HiddenField();
                    hdnCurrentRow.ID = "hdnRowInfo";
                    e.Item.Cells[0].Controls.Add(hdnCurrentRow);
                }
                string rowXML = GetRowXml(drvCurrent.DataView.Table, primaryKeyValue);
                string rowXMLWithOuterNode = "<" + m_GVTreeNodeName + "><RowList>" + rowXML + "</RowList></" + m_GVTreeNodeName + ">";
                hdnCurrentRow.Value = rowXMLWithOuterNode;

                //Find the Child Panel to pass it's reference to javascript
                Control pnlChildContainer = e.Item.Cells[2].FindControl("pnlChildGV");

                System.Web.UI.WebControls.Image imgExpand = (System.Web.UI.WebControls.Image)e.Item.Cells[1].FindControl("imgExpand");
                imgExpand.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add_symbol.png";
                imgExpand.Attributes.Add("onclick", "javascript:expandcollapse(this, '" + pnlChildContainer.ClientID + "')");
                imgExpand.Style.Add("cursor", "pointer");

                //AjaxControlToolkit.CollapsiblePanelExtender cpe = (AjaxControlToolkit.CollapsiblePanelExtender)e.Item.Cells[2].FindControl("cpeChild");
                //cpe.ExpandedImage = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/minus-icon.png";
                //cpe.CollapsedImage = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/plus-icon.png";

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

                ////Tooltip Popup functionality using custom js file.    
                ////Check for the presence of Tooltip attributes. If there are no attributes then dont show the ToolTip the image.
                //// Programmatically refer the Image control
                //System.Web.UI.WebControls.ImageButton imgBtnTTnNavigate =
                //    (System.Web.UI.WebControls.ImageButton)e.Row.Cells[2].FindControl("imgBtnTTnNavigate");
                //imgBtnTTnNavigate.ImageUrl = "~/App_Themes/" + Session["MyTheme"].ToString() + "/Images/GridViewSelect.png";
                //if (drvCurrent.Row.ItemArray[drvCurrent.Row.ItemArray.Length - 1].ToString() != string.Empty)
                //{
                //    imgBtnTTnNavigate.Visible = true;
                //    imgBtnTTnNavigate.ImageUrl = "~/App_Themes/" + Convert.ToString(Session["MyTheme"]) + "/Images/GridViewToolTip.png";
                //    // Add the client-side attributes (onmouseover & onmouseout)
                //    string onMouseOverScript = string.Format("ShowToolTipPopup('{0}');", hdnCurrentRow.ClientID);
                //    string onMouseOutScript = "HideToolTipPopup();";
                //    imgBtnTTnNavigate.Style.Add("cursor", "pointer");
                //    imgBtnTTnNavigate.Attributes.Add("onmouseover", onMouseOverScript);
                //    imgBtnTTnNavigate.Attributes.Add("onmouseout", onMouseOutScript);
                //}
                //else
                //{
                //    imgBtnTTnNavigate.ToolTip = "View Details";
                //}

                //Row Effects based on IsActive, TypeOfInactiveStatusID etc.,
                System.Web.UI.WebControls.ImageButton imgBtnTTnNavigate = new System.Web.UI.WebControls.ImageButton();
                SetRowColours(e, drvCurrent, imgBtnTTnNavigate);

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
                            string pageInfo = GetBPCPageInfo(processName, m_BPCXml).Replace(" ", "");
                            string isPopUp = GetBPCAttributeValue(de.Key.ToString(), m_BPCXml, "IsPopup");

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
                            if (processLink == "1")
                            {
                                string processLabel = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                int linkIndex = grdVwColIndex + m_NoOfGVStaticColumns - 1;
                                if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() != "1")
                                {
                                    e.Item.Cells[linkIndex].Wrap = false;
                                    //The +4 is for compensating the presence of an Image Field, the checkbox column ,radio button list, View Details field in the beginning
                                    e.Item.Cells[linkIndex].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace(" ", "~::~") + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                }
                                else if (m_IndexOfIsProtected == -1)
                                {
                                    e.Item.Cells[linkIndex].Wrap = false;
                                    //The +4 is for compensating the presence of an Image Field, the checkbox column ,radio button list, View Details field in the beginning
                                    e.Item.Cells[linkIndex].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace(" ", "~::~") + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
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
                    string imageName = e.Item.Cells[m_NoOfGVStaticColumns + imgColIndex].Text;
                    e.Item.Cells[m_NoOfGVStaticColumns + imgColIndex].Text = "";
                    System.Web.UI.WebControls.Image myImage = new System.Web.UI.WebControls.Image();
                    myImage.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/" + imageName;
                    myImage.ImageAlign = ImageAlign.Middle;
                    myImage.Height = Unit.Pixel(18);
                    myImage.AlternateText = imageName;

                    e.Item.Cells[m_NoOfGVStaticColumns + imgColIndex].Controls.Add(myImage);
                }

                //For Tooltip functionality
                InitialiseToolTips(e);

                //Binding the ChildGrid
                GridView grdVwBranch = (GridView)e.Item.FindControl("gvBranch");
                if (grdVwBranch != null)
                {
                    //grdVwBranch.AlternatingRowStyle.CssClass = "AlternatingRowStyle";
                    string parentTrxID = drvCurrent.Row.ItemArray[0].ToString();
                    string parentTrxType = drvCurrent.Row.ItemArray[1].ToString();
                    BindChildGrid(grdVwBranch, parentTrxID, parentTrxType);
                }
            }
        }


        private void BindChildGrid(GridView grdVwBranch, string parentTrxID, string parentTrxType)
        {
            #region NLog
            logger.Info("Binding the child grid :"+grdVwBranch+" with parent trx id : "+parentTrxID+" and parent trx type :"+parentTrxType);
            #endregion

            XmlDocument xDocOut = new XmlDocument();
            if (ViewState["GridViewInitData"] != null)
            {
                xDocOut.LoadXml(ViewState["GridViewInitData"].ToString());

                string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches == null)
                {
                    return;
                }
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    if (nodeBranch.Attributes["ControlType"] != null)
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlNode nodeBranchColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                        //Check for ListBox or GridView
                        if (nodeBranch.Attributes["ControlType"].Value == "GView")
                        {
                            DataSet dsRowList = new DataSet();
                            XmlNode nodeRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                            if (nodeRows == null)
                            {
                                continue;
                            }
                            XmlNodeReader read = new XmlNodeReader(nodeRows);
                            DataSet ds = new DataSet();
                            ds.ReadXml(read);
                            DataTable ChildDataTable = new DataTable();
                            ChildDataTable = ds.Tables[0];
                            DataView ChildDataView = new DataView(ChildDataTable);
                            ChildDataView.RowFilter = treeNodeName + "_TrxID='" + parentTrxID + "' and " + treeNodeName + "_TrxType='" + parentTrxType + "'";
                            if (ChildDataView.Count > 0)
                            {
                                //Add the columns only if no columns are present.
                                if (grdVwBranch.Columns.Count == 1)
                                {
                                    m_htChildBPCntrls.Clear();
                                    m_htChildBPCColumns.Clear();
                                    m_arrAmountCols.Clear();
                                    //Creating the Columns.
                                    int colCntr = 0;
                                    foreach (XmlNode colNode in nodeBranchColumns.ChildNodes)
                                    {
                                        //Add the column only if FullViewLength is not equal to zero
                                        int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                                        string currentLabel = colNode.Attributes["Label"].Value;
                                        if (colFullViewLength != 0 && ds.Tables[0].Columns.Contains(currentLabel))
                                        {
                                            BoundField newField = new BoundField();
                                            newField.DataField = colNode.Attributes["Label"].Value;
                                            newField.HeaderText = colNode.Attributes["Caption"].Value;
                                            newField.ItemStyle.Wrap = false;
                                            ////Set the sort expression as inidicated in the xml.
                                            //if (colNode.Attributes["IsSortable"].Value == "1")
                                            //{
                                            //    newField.SortExpression = colNode.Attributes["Label"].Value;
                                            //}

                                            //Data formatting stuff..
                                            newField.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                            XmlAttribute attControlType = colNode.Attributes["ControlType"];
                                            if (attControlType != null)
                                            {
                                                switch (attControlType.Value)
                                                {
                                                    case "Cal":
                                                        {
                                                            //Calendar
                                                            //Apply formatting to the DateTime fields in the DataSet.
                                                            foreach (DataRow dr in ds.Tables[0].Rows)
                                                            {
                                                                //If the value is IsDate then change format MM/DD/YY
                                                                DateTime date;
                                                                if (DateTime.TryParse(dr[currentLabel].ToString(), out date))
                                                                {
                                                                    dr[currentLabel] = date.ToString("MM/dd/yy");
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    case "Amount":
                                                        {
                                                            m_arrAmountCols.Add(colCntr);
                                                            newField.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                                                            //newField.ItemStyle.CssClass = "grdVwColRightJustify";
                                                            newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            break;
                                                        }
                                                }
                                            }
                                            //newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                                            XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                                            if (attBPControl != null && attBPControl.Value.Trim() != string.Empty)
                                            {
                                                m_htChildBPCntrls.Add(attBPControl.Value.Trim(), GetColumnIndex(currentLabel, ds.Tables[0]));
                                                m_htChildBPCColumns.Add(attBPControl.Value.Trim(), colCntr);
                                            }

                                            //Columns excluding the last one.
                                            newField.ItemStyle.Width = Unit.Percentage(colFullViewLength);
                                            grdVwBranch.Columns.Add(newField);
                                            colCntr++;
                                        }
                                    }
                                }

                                grdVwBranch.DataSource = ChildDataView;
                            }
                            else
                            {
                                grdVwBranch.DataSource = new DataTable();
                            }
                            grdVwBranch.RowDataBound += new GridViewRowEventHandler(grdVwBranch_RowDataBound);
                            grdVwBranch.Sorting += new GridViewSortEventHandler(grdVwBranch_Sorting);
                            grdVwBranch.DataBind();
                        }
                        else
                        {
                            grdVwContents.Columns[3].Visible = false;
                        }
                    }
                    else
                    {
                        grdVwContents.Columns[3].Visible = false;
                    }
                }
            }
        }

        void grdVwBranch_Sorting(object sender, GridViewSortEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void grdVwBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
            if (drvCurrent != null)//m_IsFirstRun
            {
                m_IsFirstRun = false;
                InitialiseIndexVariables(drvCurrent);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.CssClass = "GVRowHover";

                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();

                foreach (int colIndex in m_arrAmountCols)
                {
                    string unformattedAmt = e.Row.Cells[colIndex + m_NoOfChildGVStaticColumns].Text;
                    decimal amount;
                    if (Decimal.TryParse(unformattedAmt, out amount))
                    {
                        e.Row.Cells[colIndex + m_NoOfChildGVStaticColumns].Text = string.Format("{0:N}", amount);
                        e.Row.Cells[colIndex + m_NoOfChildGVStaticColumns].ToolTip = e.Row.Cells[colIndex + m_NoOfChildGVStaticColumns].Text;
                    }
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

                //If BusinessProcessControls node is absent or Show hyperlinks is false dont display the hyperlinks
                if (m_BPCXml != string.Empty && m_HyperLinksEnabled == "1")
                {
                    foreach (DictionaryEntry de in m_htChildBPCntrls)
                    {
                        string processName = de.Key.ToString();
                        //Index of the column to which BPC is assigned.
                        int currentBPCIndex = Convert.ToInt32(de.Value);
                        int grdVwColIndex = Convert.ToInt32(m_htChildBPCColumns[processName]);

                        //Calling Object functionality.
                        string BPCColName = GetColumnName(Convert.ToInt32(m_htChildBPCntrls[processName])
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
                            if (processLink != "0")//Previously processLink == "1"
                            {
                                string processLabel = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                string linkText = drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace("'", "\\'").Replace(" ", "~::~"); ;
                                linkText = ObjCommonUI.HtmlEncode(linkText);
                                if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() != "1")
                                {
                                    e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Wrap = false;
                                    e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "~::~") + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Text + "</a>";
                                }
                                else if (m_IndexOfIsProtected == -1)
                                {
                                    e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Wrap = false;
                                    e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "~::~") + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + e.Row.Cells[grdVwColIndex + m_NoOfChildGVStaticColumns].Text + "</a>";
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
                #region NLog
                logger.Info("Attribute value : "+nodeProcess.Attributes[AttributeName].Value+" for a given attribute name : " + AttributeName);
                #endregion

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
        private void InitialiseToolTips(DataGridItemEventArgs e)
        {
            foreach (DictionaryEntry de in m_htGVColumns)
            {
                string colName = de.Key.ToString();
                int currentColIndex = Convert.ToInt32(de.Value) + m_NoOfGVStaticColumns - 1;
                int colLength = Convert.ToInt32(m_htGVColWidths[de.Value]);
                TableCell tcCurrent = e.Item.Cells[currentColIndex];
                if (tcCurrent.Controls.Count == 0)
                {
                    //tcCurrent.Attributes.Add("Title", DataBinder.Eval(e.Row.DataItem, colName).ToString());
                    tcCurrent.ToolTip = DataBinder.Eval(e.Item.DataItem, colName).ToString();

                    if (tcCurrent.Text.StartsWith("<a") && tcCurrent.Text.EndsWith("</a>"))
                    {
                        int startIndex = tcCurrent.Text.IndexOf(">") + 1;
                        int endIndex = tcCurrent.Text.IndexOf("</a>", startIndex);
                        string strToInsert = tcCurrent.Text.Substring(startIndex, endIndex - startIndex);
                        if (strToInsert.Length > colLength)
                        {
                            tcCurrent.Text = tcCurrent.Text.Remove(startIndex, endIndex - startIndex);
                            strToInsert = strToInsert.Substring(0, colLength - 3) + "...";
                            tcCurrent.Text = tcCurrent.Text.Insert(startIndex, strToInsert);
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

        /// <summary>
        /// Creates row effects like setting colors and disabling as specified in the XML.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        /// <param name="drvCurrent">Current grid view row.</param>
        private void SetRowColours(DataGridItemEventArgs e, DataRowView drvCurrent, ImageButton imgBtnTTnNavigate)
        {
            if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() == "1")
            {
                //Disable the row and show a lock symbol
                //e.Row.Enabled = false;
                for (int index = m_NoOfGVStaticColumns; index < e.Item.Cells.Count; index++)
                {
                    e.Item.Cells[index].Enabled = false;
                }
                imgBtnTTnNavigate.ImageUrl = "~/App_Themes/" + Convert.ToString(Session["MyTheme"]) + "Images/grid_lock.png";
            }
            if (m_IndexOfTypeOfInactiveStatusID != -1)
            {
                switch (drvCurrent.Row.ItemArray[m_IndexOfTypeOfInactiveStatusID].ToString())
                {
                    case "1":
                        {
                            //Uncommitted
                            e.Item.BackColor = Color.Red;
                            break;
                        }
                    case "2":
                        {
                            //Deleted
                            e.Item.BackColor = Color.Brown;
                            break;
                        }
                    case "3":
                        {
                            //Waiting for SOX Approval
                            e.Item.BackColor = Color.Orange;
                            break;
                        }
                    case "4":
                        {
                            //Closed
                            e.Item.BackColor = Color.MintCream;
                            break;
                        }
                    case "5":
                        {
                            //Auto save
                            e.Item.BackColor = Color.Green;
                            break;
                        }
                    case "6":
                        {
                            //Not Used
                            e.Item.BackColor = Color.LightYellow;
                            break;
                        }
                    case "7":
                        {
                            //Waiting Approval
                            e.Item.BackColor = Color.LightBlue;
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

        private string GetIDString(int rowID)
        {
            if (rowID <= 9)
            {
                return string.Format("0{0}", rowID);
            }
            return rowID.ToString();
        }

        ///// <summary>
        ///// Selected change event for radBtnSelect.
        ///// </summary>
        ///// <param name="sender">Calling Object</param>
        ///// <param name="e">Event Arguments.</param>
        //protected void radBtnSelect_CheckedChanged(object sender, EventArgs e)
        //{
        //    ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");

        //    //Mutually Exclusive RdBtn
        //    RadioButton rbselected = (RadioButton)sender;
        //    foreach (GridViewRow dr in grdVwContents.Rows)
        //    {
        //        RadioButton rb = (RadioButton)dr.FindControl("radBtnSelect");
        //        if (rb == rbselected)
        //        {
        //            rb.Checked = true;
        //        }
        //        else
        //        {
        //            rb.Checked = false;
        //        }
        //    }
        //    string[] arrSplitParams = { "~::~" };
        //    string[] formInfoSplit = hdnFormInfo.Value.Split(arrSplitParams, StringSplitOptions.RemoveEmptyEntries);
        //    GridViewRow gvrCurrent = (GridViewRow)rbselected.NamingContainer;
        //    //string navigatePage = rbselected.Attributes["NavigatePage"];
        //    string navigatePage = formInfoSplit[1];
        //    string thisPage = HttpContext.Current.Request.ServerVariables["URL"].ToString().Trim();

        //    string[] splitCurrPageName = thisPage.Split('/');
        //    string currAspxParentFolder = splitCurrPageName[splitCurrPageName.Length - 2];
        //    string currAspxPageName = splitCurrPageName[splitCurrPageName.Length - 1];

        //    string[] splitNavPageName = navigatePage.Split('/');
        //    string navAspxParentFolder = splitNavPageName[splitNavPageName.Length - 2];
        //    string navAspxPageName = splitNavPageName[splitNavPageName.Length - 1];

        //    string BPGID = formInfoSplit[0];
        //    string containerRowXML = ((HiddenField)gvrCurrent.FindControl("hdnRowInfo")).Value;
        //    XmlDocument xDocRowInfoXml = new XmlDocument();
        //    xDocRowInfoXml.LoadXml(containerRowXML);
        //    string TrxID = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TrxID"].Value;
        //    string TrxType = xDocRowInfoXml.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows").Attributes["TrxType"].Value;
        //    string COCaption = string.Empty;
        //    if (grdVwContents.Columns.Count > 4)
        //    {
        //        //Assign the first column cell's text to the caption.
        //        COCaption = ((GridViewRow)rbselected.NamingContainer).Cells[m_NoOfGVStaticColumns].ToolTip;
        //    }
        //    string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + navigatePage + "</PageInfo><Caption>" + COCaption + "</Caption></CallingObject>";
        //    string BPInfo = "<bpinfo><BPGID>" + BPGID + "</BPGID>" + containerRowXML + callingObjXML + "</bpinfo>";
        //    hdnGVBPEINFO.Value = containerRowXML + callingObjXML;//Set the BPEinfo so that it can be later used by form-level process link functions

        //    if (currAspxParentFolder.ToUpper().Trim() == navAspxParentFolder.ToUpper().Trim()
        //        && currAspxPageName.ToUpper().Trim() == navAspxPageName.ToUpper().Trim())
        //    {
        //        //Set the Page Level hidden field with current row XML
        //        hdnSelectedRows.Value = BPInfo;

        //        ////Set the page level hidden field to access footnote and attachment
        //        //RowXMLForNote(BPGID, containerRowXML);                
        //        //Showing The Process Panel

        //        //pnlBPCContainer
        //        //Opening the panel                                               
        //        Control cntrlpnlEntryFormobj = cntPlaceHolder.FindControl("pnlEntryForm");
        //        cntrlpnlEntryFormobj.Visible = true;

        //        //Collapsing the grid
        //        AjaxControlToolkit.CollapsiblePanelExtender cntrlpnlTitleobj = (AjaxControlToolkit.CollapsiblePanelExtender)cntPlaceHolder.FindControl("cpeCPGV1");
        //        cntrlpnlTitleobj.Collapsed = true;
        //        cntrlpnlTitleobj.ClientState = "True";

        //        //UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
        //        //BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)commonObjUI.FindControlRecursive(this.Page, "BtnsUC");
        //        BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
        //        BtnsUserCtrl.RestoreButtonImages();

        //        ////Visibling the Business Process Panel
        //        //commonObjUI.GridViewUserControl = this;
        //        //Control pnlBPCContainer = cntrlpnlEntryFormobj.FindControl("pnlBPCContainer");

        //        //if (pnlBPCContainer != null)
        //        //{
        //        //    pnlBPCContainer.Controls.Add(commonObjUI.GetBusinessProcessLinksTable(BtnsUserCtrl.GVDataXml));
        //        //}
        //        //To Restore the original images of ImageButtons of a page.


        //        //For Footnotes gridview preloading 
        //        // BtnsUserCtrl.PreloadNotesGV();  

        //        XmlDocument xDocRowInfo = new XmlDocument();
        //        xDocRowInfo.LoadXml(hdnSelectedRows.Value);
        //        XmlNode xNodeRow = xDocRowInfo.SelectSingleNode("bpinfo/" + BtnsUserCtrl.TreeNode + "/RowList/Rows");
        //        if (xNodeRow.Attributes["ToolTipInfo"] != null)
        //        {
        //            xNodeRow.Attributes.Remove(xNodeRow.Attributes["ToolTipInfo"]);
        //        }
        //        BtnsUserCtrl.RwToBeModified = xNodeRow.OuterXml;

        //        XmlDocument xdc = new XmlDocument();
        //        xdc.LoadXml(GridViewInitData.ToString());
        //        XmlNode colNode = xdc.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUserCtrl.TreeNode + "/GridHeading/Columns");
        //        //commonObjUI.EnableDisableAndFillUI(false, cntrlpnlEntryFormobj, xNodeRow, BtnsUserCtrl.TreeNode, colNode);
        //        if (this.FormTempData.Length == 0)
        //        {
        //            this.FormTempData = GridViewInitData;
        //        }
        //        XmlDocument xDocBtnsGVDataXML = new XmlDocument();
        //        xDocBtnsGVDataXML.LoadXml(this.FormTempData);
        //        commonObjUI.GridViewUserControl = this;
        //        commonObjUI.XDocFormXML = xdc;
        //        commonObjUI.GVSelectedRow = xNodeRow;

        //        commonObjUI.EnableDisableAndFillUI(false, cntrlpnlEntryFormobj, xDocBtnsGVDataXML, xNodeRow);//xdc

        //        BtnsUserCtrl.ResetUI("ISRDBTNCLICK");
        //        commonObjUI.ChangeUIColor("DISABLEMODE", cntrlpnlEntryFormobj);
        //    }
        //    else
        //    {
        //        //Set a session variable with the current row XML and redirect to the navigate page.
        //        Session["BPINFO"] = BPInfo;
        //        Response.Redirect("../" + navigatePage);
        //    }
        //}

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
                m_ObjCommonUI.ParentChildGVUC = this;
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

                //Find the ChildGV User Control and bind it.
                UserControls.ChildGridView CGVUC = (UserControls.ChildGridView)cntPlaceHolder.FindControl("CGVUC");
                if (CGVUC != null)
                {
                    CGVUC.InitialiseBranchGrid(xDocBtnsGVDataXML, (Control)sender);
                }

                //Showing data in html table.
                ShowHtmlVw(cntrlpnlEntryFormobj, parentTrxID, xDocBtnsGVDataXML, treeNode);

                //Checking whether valid BPGID's exist for current row.
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ValidateBPGIDRow", "ValidateBPGIDRow()", true);
            }
            else
            {
                //Set a session variable with the current row XML and redirect to the navigate page.
                Session["BPINFO"] = BPInfo;
                Response.Redirect("../" + navigatePage);
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
                if (nodeDDLRow.Attributes["ImgSrcLarge"] != null)//Ref:Project.aspx.cs>SetTypeOfJobHdnValue()
                {
                    string imageSrc = nodeDDLRow.Attributes["ImgSrcLarge"].Value;
                    imgTypeOfJob.Attributes.Add("style", "DISPLAY: Block;");
                    imgTypeOfJob.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/" + imageSrc;
                }
                else
                {
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
        /// Gets the specified cell contents.
        /// </summary>
        /// <param name="dtSearch">The search table</param>
        /// <param name="colName">The Name of the column.</param>
        /// <param name="rowIndex">The index of the row.</param>
        /// <returns>String cell contents.</returns>
        private string GetCellValue(DataTable dtSearch, string colName, int rowIndex)
        {
            #region NLog
            logger.Info("Getting the specified cell contents for column name : "+colName+" and row index :"+rowIndex);
            #endregion

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
        private string GetBPCBPGID(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Getting the BPGID of the Businees Process Control with process name : "+processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);

            #region NLog
            logger.Info("BPGID of the Businees Process Control is : " + xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["BPGID"].Value);
            #endregion

            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["BPGID"].Value;
        }

        /// <summary>
        /// Gets the Page Info value from the given XML.
        /// </summary>
        /// <param name="processName">Process Name</param>
        /// <param name="bpcXml">Business Process Controls XML.</param>
        /// <returns></returns>
        private string GetBPCPageInfo(string processName, string bpcXml)
        {
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

        ///// <summary>
        ///// Setting Rowxml data for footnote and attachment section
        ///// </summary>
        ///// <param name="BPGID"></param>
        ///// <param name="SelectedRow"></param>
        //private void RowXMLForNote(string BPGID, string SelectedRow)
        //{

        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.LoadXml(SelectedRow);
        //    XmlNode nodeRow = xDoc.SelectSingleNode(hdnGVTreeNodeName.Value + "/RowList/Rows");
        //    XmlAttributeCollection attCol = nodeRow.Attributes;

        //    System.Text.StringBuilder sbObject = new System.Text.StringBuilder();

        //    sbObject.Append("<Object>");

        //    sbObject.Append("<BPGID>" + BPGID + "</BPGID>");

        //    for (int i = 0; i < attCol.Count; i++)
        //    {
        //        switch (attCol[i].Name.ToString())
        //        {
        //            //case "BPGID":
        //            //    sbObject.Append("<BPGID>" + drFoundRows[0][i].ToString() + "</BPGID");
        //            //    break;
        //            case "TrxType":
        //                sbObject.Append("<TrxType>" + attCol[i].Value.ToString() + "</TrxType>");
        //                break;
        //            case "TrxID":
        //                sbObject.Append("<TrxID>" + attCol[i].Value.ToString() + "</TrxID>");
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    sbObject.Append("</Object>");
        //    hdnRowObject.Value = sbObject.ToString();
        //}



        /// <summary>
        /// Generates the request xml to be sent to the XML processor
        /// </summary>
        /// <returns>string XML</returns>
        private string GenerateRequestXML(string pageNo, string sortColumn, string sortOrder)
        {
            #region NLog
            logger.Info("Generates the request xml to be sent to the XML processor with page no : "+pageNo+" and sort column : "+sortColumn+" and sort order as : "+sortOrder);
            #endregion

            XDocUserInfo = m_ObjCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            XmlNode nodeRoot = XDocUserInfo.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = strBPE;

            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(GridViewBPInfo);
            //string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
            string defaultPageSize = grdVwContents.PageSize.ToString();
            if (xDocBPEInfo.SelectSingleNode("//Gridview") == null)
            {
                XmlNode xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                xNodeTreeNode.InnerXml = @"<Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>"
                    + defaultPageSize + "</Pagesize><Sortcolumn>" + sortColumn
                    + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder></Gridview>";
                xDocBPEInfo.ChildNodes[0].AppendChild(xNodeTreeNode);
            }
            else
            {
                XmlNode nodeGridResults = xDocBPEInfo.SelectSingleNode("bpinfo//Gridview");
                nodeGridResults.SelectSingleNode("Pagesize").InnerText = defaultPageSize;// grdVwContents.PageSize.ToString();
                nodeGridResults.SelectSingleNode("Pagenumber").InnerText = pageNo;
                nodeGridResults.SelectSingleNode("Sortcolumn").InnerText = sortColumn;
                nodeGridResults.SelectSingleNode("Sortorder").InnerText = sortOrder;
                if (nodeGridResults.ParentNode.LocalName != hdnGVTreeNodeName.Value)
                {
                    //Then embed the GridView node into a New Node named as the Tree Node.
                    XmlNode xNodeTreeNode = xDocBPEInfo.CreateElement(hdnGVTreeNodeName.Value);
                    //Remove the GridView from the current parent(bpinfo) and append it as child to the above node.
                    nodeGridResults.ParentNode.RemoveChild(nodeGridResults);
                    xNodeTreeNode.AppendChild(nodeGridResults);
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
            CommonBO objBO = new CommonBO();
            //GridViewInitData 
            string strOutXml = objBO.GetVendorHistory(nodeRoot.OuterXml);
            if (this.Page.Master != null)
            {
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
                if (cntPlaceHolder != null)
                {
                    BtnsUserCtrl = (LAjitDev.UserControls.ButtonsUserControl)cntPlaceHolder.FindControl("BtnsUC");
                    if (BtnsUserCtrl != null)
                    {
                        //BtnsUserCtrl.GVDataXml = GridViewInitData;
                        XmlDocument Xdoc = new XmlDocument();
                        Xdoc.LoadXml(BtnsUserCtrl.GVDataXml);
                        XmlDocument returnXML = new XmlDocument();
                        returnXML.LoadXml(strOutXml);
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
            return strOutXml;
        }

        protected void lnkBtnFirst_Click(object sender, EventArgs e)
        {
            m_CurrPageNo = 1;
            if (hdnCurrPageNo.Value != "1")
            {
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();
                string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
                DisplayFoundResults(result);
            }
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;
        }

        protected void lnkBtnPrev_Click(object sender, EventArgs e)
        {
            m_CurrPageNo = Convert.ToInt32(hdnCurrPageNo.Value);
            if (m_CurrPageNo != 1)
            {
                m_CurrPageNo--;
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();

            }
            string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
            DisplayFoundResults(result);
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;
        }

        protected void lnkBtnNext_Click(object sender, EventArgs e)
        {
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
        }

        protected void lnkBtnLast_Click(object sender, EventArgs e)
        {
            m_CurrPageNo = Convert.ToInt32(hdnMaxPages.Value);
            if (hdnCurrPageNo.Value != hdnMaxPages.Value)
            {
                hdnCurrPageNo.Value = m_CurrPageNo.ToString();
                string result = GenerateRequestXML(m_CurrPageNo.ToString(), ViewState["SORTEXP"].ToString(), ViewState["SORTDIRECTION"].ToString());
                DisplayFoundResults(result);
            }
            //ddlVListPages.SelectedIndex = m_CurrPageNo - 1;
        }

        //protected void lnkBtnProc1_Click(object sender, EventArgs e)
        //{
        //    System.Threading.Thread.Sleep(3000);
        //    string selRowsXML = string.Empty;
        //    foreach (GridViewRow gvrChkBx in grdVwContents.Rows)
        //    {
        //        CheckBox chkBxCurrent = (CheckBox)gvrChkBx.FindControl("chkBxProcess");
        //        if (chkBxCurrent.Checked)
        //        {
        //            selRowsXML += ((Label)gvrChkBx.Cells[0].FindControl("lblRowInfo")).Text;
        //        }
        //    }
        //}

        //protected void lnkBtnProc2_Click(object sender, EventArgs e)
        //{

        //}

        //protected void lnkBtnProc3_Click(object sender, EventArgs e)
        //{

        //}

        protected void grdVwContents_OnSorting(object sender, DataGridSortCommandEventArgs e)
        {
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

        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortDirection = string.Empty;
            hdnCurrPageNo.Value = ddlPages.SelectedItem.ToString();
            if (Convert.ToString(ViewState["SORTDIRECTION"]) == "ASC")
            {
                sortDirection = "ASC";
            }
            else
            {
                sortDirection = "DESC";
            }
            string result = GenerateRequestXML(ddlPages.SelectedItem.ToString(), ViewState["SORTEXP"].ToString(), sortDirection);
            DisplayFoundResults(result);
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
