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
using System.Web.SessionState;
using System.Collections;
using System.Drawing;
using System.Text;
using LAjitControls;
using LAjit_BO;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Xml.Xsl;
using System.Xml.XPath;
using LAjitDev.UserControls;
using Gios.Pdf;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using NLog;


namespace LAjitDev
{
    public class clsReportsUI
    {
        
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        Report objItextReport = new Report();
        public CommonUI commonObjUI = new CommonUI();
        Hashtable m_htGVColumns = new Hashtable();
        public PdfDocument myPdfDocument;
        XmlDocument XDocUserInfo = new XmlDocument();
        public bool m_EmailStatus = false;
        private bool m_ShowPDF = true;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        //
        public bool m_PageNumber;
        public bool m_WaterMark;
        private string m_PageLayOut;
        private string m_PageSize;
        private string m_FontName;
        private string m_Password;
        public bool m_Annotations;
        //
        PdfTable myHeaderPdfTable = null;

        #region Report Declarations
        int pageCnt = 0;
        int pgHeight = 0;
        int pgWidth = 0;
        double posX = 20;
        double posY = 70;
        double width;
        double height;
        double currentYPos = 70;
        double imgPosX;
        double imgPosY;
        Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
        Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
        Font GridHeaderFont = new Font("Verdana", 7, FontStyle.Bold);
        Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
        Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
        Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
        Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
        Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
        Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
        Font SumRowFont1 = new Font("Verdana", 7, FontStyle.Bold);
        Font RowFontBold = new Font("Verdana", 8, FontStyle.Bold);
        Font FontUnderline = new Font("Verdana", 8, FontStyle.Regular | FontStyle.Underline);
        Font RowBoxFontBold = new Font("Verdana", 10, FontStyle.Bold);
        PdfTable myPdfTable = null;

        PdfTablePage myHeaderPdfTablePage = null;
        PdfPage newPdfPage = null;
        #endregion

        #region Properties
        /// <summary>
        /// Accessor for the Session["LinkBPinfo"]
        /// </summary>
        public string SessionLinkBPInfo
        {
            get { return Convert.ToString(HttpContext.Current.Session["LinkBPinfo"]); }
            set { HttpContext.Current.Session["LinkBPinfo"] = value; }
        }

        public bool EmailStatus
        {
            get { return m_EmailStatus; }
            set { m_EmailStatus = value; }
        }

        public bool PageNumber
        {
            get { return m_PageNumber; }
            set { m_PageNumber = value; }
        }

        public bool WaterMark
        {
            get { return m_WaterMark; }
            set { m_WaterMark = value; }
        }

        public string PageLayout
        {
            get { return m_PageLayOut; }
            set { m_PageLayOut = value; }
        }

        public string PageSize
        {
            get { return m_PageSize; }
            set { m_PageSize = value; }
        }

        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        public bool Annotaions
        {
            get { return m_Annotations; }
            set { m_Annotations = value; }
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
        /// Display the PDF when download is complete if true, else false.
        /// </summary>
        public bool ShowPDF
        {
            get
            {
                return m_ShowPDF;
            }
            set
            {
                m_ShowPDF = value;
            }
        }
        #endregion

        /// Common Submit method for Add, Modify, Delete, Find 
        /// </summary>
        /// <param name="clickAction">Action Parameter</param>
        /// 
        public string SubmitReport(string clickAction, Control CurrentPage, string currentBPGID)
        {
            #region NLog
            logger.Info("Common Submit method for action : "+clickAction+ " and Page : "+CurrentPage+"and  with BPGID : "+currentBPGID);
            #endregion

            string strReqXml = GetReportRequestXML(CurrentPage, currentBPGID);
            //BPOUT from DB
            string strOutXml = reportsBO.GetReportBPEOut(strReqXml);
            return strOutXml;
        }

        private string GetReportRequestXML(Control CurrentPage, string currentBPGID)
        {
            #region NLog
            logger.Info("Gets the Report Request XMl for the current page as :"+CurrentPage+" and with current BPGID as : "+currentBPGID);
            #endregion

            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            string pageReturnXml = string.Empty;
            UserControls.ButtonsUserControl BtnsUC = new LAjitDev.UserControls.ButtonsUserControl();
            Page pg = (Page)CurrentPage.TemplateControl;
            PropertyInfo pInfo = pg.GetType().GetProperty("RetXML");
            BtnsUC.GVDataXml = pInfo.GetValue(pg, null).ToString();
            pageReturnXml = pInfo.GetValue(pg, null).ToString();
            commonObjUI.ButtonsUserControl = BtnsUC;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(pageReturnXml);
            string m_treeNodeName = string.Empty;
            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            {
                m_treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            }
            //get new row(Parent)
            string CntrlValues = commonObjUI.GetNewRow(pnlEntryForm, xDoc, "Report");
            ChildGridView CGVUC = (ChildGridView)pnlEntryForm.FindControl("CGVUC");
            //get new row (child grid view)
            string trxID = commonObjUI.GVSelectedRow.Attributes["TrxID"].Value;
            string trxType = commonObjUI.GVSelectedRow.Attributes["TrxType"].Value;
            string gvXML = (CGVUC != null) ? CGVUC.GetGridViewXML(pnlEntryForm, BtnsUC.GVDataXml, trxID, trxType, "Report") : string.Empty;
            //For filtering the checkbox selected rows from all child grid rows.
            if (!string.IsNullOrEmpty(gvXML))
            {
                XmlDocument xdocGVXML = new XmlDocument();
                xdocGVXML.LoadXml(gvXML);
                XmlNodeList nodeSelectedRws = xdocGVXML.FirstChild.SelectNodes("RowList//Rows[@Selectit='1']");
                string BranchNodeName = xdocGVXML.FirstChild.Name;
                string filteredXML = string.Empty;
                foreach (XmlNode nodeRw in nodeSelectedRws)
                {
                    filteredXML = filteredXML + nodeRw.OuterXml.ToString();
                }
                gvXML = "<" + BranchNodeName + ">" + "<RowList>" + filteredXML + "</RowList>" + "</" + BranchNodeName + ">";
            }
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            //Getting request XML
            //Passing the TrxID and TrxType to the method so that they reflect in the parent row.
            reportsBO.TrxID = trxID;
            reportsBO.TrxType = trxType;
            return reportsBO.GenGenericProcessRequestXML("Report", currentBPGID, CntrlValues, strBPE, false, "", "", SessionLinkBPInfo, m_treeNodeName, gvXML);
        }

        public void BindPage(string bpInfo, Control CurrentPage, out string retXML)
        {
            #region NLog
            logger.Info("Binds the data for a given GridView or EntryForm depending on different scenarios with BPINFO as :"+bpInfo);
            #endregion

            retXML = string.Empty;
            //Keeping Parent BPInfo backup in the hidden variable            
            HiddenField parentBPInfo = (HiddenField)CurrentPage.FindControl("parentBPInfo");
            if (parentBPInfo.Value != null || parentBPInfo.Value != string.Empty)
            {
                parentBPInfo.Value = bpInfo;
            }
            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            Panel pnlEntryFormTitle = (Panel)CurrentPage.FindControl("pnlCPGV1Title");
            Page curntPage = (Page)CurrentPage.Page;
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string strReqXml = reportsBO.GenGenericProcessRequestXML("PAGELOAD", bpInfo, "", strBPE, false, "", "", bpInfo, "", "");
            //BPOUT from DB
            string strOutXml = reportsBO.GetReportBPEOut(strReqXml);
            retXML = strOutXml;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(strOutXml);
            ////Keeping Parent page XML backup in the hidden variable used while closing the child.
            //success messge            
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeMsgStatus != null)
            {
                if (nodeMsgStatus.InnerText == "Success")
                {
                    retXML = strOutXml;
                    string m_treeNodeName = string.Empty;
                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                    {
                        m_treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                    }
                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");
                    if (nodeColumns != null)
                    {
                        //Invisible all the columns whose IsHidden Attribute is one
                        commonObjUI.HandleIsHidden(pnlEntryForm, xDoc, false);
                        //For Filling DD
                        ArrayList alColIslink = new ArrayList();
                        SetLabelText(xDoc, out alColIslink, pnlEntryForm, m_treeNodeName);
                        //Filling dropdown data   
                        if (alColIslink.Count > 0)
                        {
                            commonObjUI.FillDropDownData(strOutXml, alColIslink, pnlEntryForm);
                        }
                        //To set the default values for all page controls(both parent and child).
                        commonObjUI.SetDefault(pnlEntryForm, nodeColumns);
                    }
                    //Branch objects
                    commonObjUI.InitialiseBranchObjects(xDoc, pnlEntryForm);
                    //Visible or Invisible the EntryForm and CPGV based on different scenarios.            
                    XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList");
                    //Empty EntryForm for 'Add'
                    if (nodeRowList == null)
                    {
                        Label lblStatus = (Label)CurrentPage.FindControl("lblStatus");
                        if (xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label") != null)
                        {
                            lblStatus.Text = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;
                        }
                    }
                    else
                    {
                        //bool isMultipleBranch = false;//True when control type is null otherwise false for Gridview and ListBox
                        //Filling Tree node controls
                        commonObjUI.EnableDisableAndFillUI(true, pnlEntryForm, xDoc, nodeRowList.ChildNodes[0], "FIND", true);
                        //Setting the Description label value
                        Label lbldescValue = (Label)pnlEntryForm.FindControl("lblDescriptionValue");
                        if (lbldescValue != null)
                        {
                            if (nodeRowList.ChildNodes[0] != null)
                            {
                                if (nodeRowList.ChildNodes[0].Attributes["Description"] != null)
                                {
                                    lbldescValue.Text = nodeRowList.ChildNodes[0].Attributes["Description"].Value;
                                }
                            }
                        }
                    }
                    HtmlTableCell htcEntryForm = (HtmlTableCell)CurrentPage.FindControl("pnlCPGV1Title").FindControl("htcCPGV1");
                    //HtmlTableCell htcEntryFormAuto = (HtmlTableCell)CurrentPage.FindControl("pnlEntryFormTitle").FindControl("htcEntryFormAuto");
                    commonObjUI.SetPanelHeading(htcEntryForm, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Title").InnerText);
                    //htcEntryFormAuto.Width = Convert.ToString(825 - Convert.ToInt32(htcEntryForm.Width) - 50);
                    //Set the Page Title for the current page.
                    ((Page)CurrentPage.TemplateControl).Title = xDoc.SelectSingleNode("Root/bpeout/FormInfo/Title").InnerText;
                }
                else
                {
                    if (pnlEntryFormTitle != null)
                    {
                        pnlEntryFormTitle.Visible = false;
                    }
                    pnlEntryForm.Visible = false;
                    Panel pnlContentError = (Panel)CurrentPage.FindControl("pnlContentError");
                    pnlContentError.Visible = true;
                    Label lblError = (Label)CurrentPage.FindControl("lblError");
                    XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                    if (nodeMsg != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                        else if (nodeMsg.SelectSingleNode("Label") != null)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else
                    {
                        lblError.Text = nodeMsgStatus.InnerText;
                    }
                }
            }
        }

        public void SetLabelText(XmlDocument xDocout, out ArrayList alColIslink, Panel pnlEntryForm, string treenodeName)
        {
            #region NLog
            logger.Info("Setting the values to all the associated lables in the current page.");
            #endregion

            alColIslink = new ArrayList();
            //Filling Tree Labels
            XmlNode nodeColumns = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + treenodeName + "/GridHeading/Columns");
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
                }
            }
        }

        /// <summary>
        /// Gets the DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public DataTable GetSingleRowDataToPrint(string GVXml, string treeNodeName, string rptType, int trxID)
        {
            #region NLog
            logger.Info("Gets the DataTable to be printed with : "+GVXml+ " and treenode as : "+treeNodeName+ " with report type as : "+rptType+" and trxID :"+trxID);
            #endregion

            DataTable dt = new DataTable();
            if (GVXml != string.Empty)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode nodeRows;
                string filenodename = string.Empty;
                xDoc.LoadXml(GVXml);
                //Get the Grid Layout nodes
                //Getting the dataset to be printed
                nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                if (nodeRows != null)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable
                    dt = dsRows.Tables[0];
                    DataView dV = dt.DefaultView;
                    dV.RowFilter = "[TrxID]=" + trxID;
                    dt.Clear();
                    DataRow dr1 = dV.Table.Rows[0];
                    dt.Rows.Add(dr1);
                    int rowCnt = dt.Rows.Count;
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    int colPos = 0;
                    //Storing the columns names and captions in the HashTable
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    //Removing unwanted columns and setting the captions as per the XML.
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;//DateTime column
                        string colName = dt.Columns[index].ColumnName;
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "TrxID")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Set the caption
                        {
                            XmlNode nodeCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim().ToString() + "']");
                            //Checking for isSummed value for that column
                            if (nodeCol != null)
                            {
                                //Checking for IsSummed attribute
                                if (!sumExists)
                                {
                                    if (nodeCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    if (m_htGVColumns.ContainsKey(dt.Columns[index].ColumnName.Trim().ToString()))
                                    {
                                        if (dRow[dt.Columns[index]].ToString() != string.Empty)
                                        {
                                            //Truncating the data if it is greater than its FullView Length
                                            int datavalLength = dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length;
                                            if (colFVL != 0)
                                            {
                                                if (dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length > colFVL)
                                                {
                                                    //Keeping commas if IsNumeric ="1"
                                                    if (numericExists)
                                                    {
                                                        decimal amount;
                                                        if (Decimal.TryParse(dRow[dt.Columns[index]].ToString(), out amount))
                                                        {
                                                            dRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                                        }
                                                    }
                                                    //Formatting Date field 
                                                    if (cal)
                                                    {
                                                        DateTime dateTime;
                                                        if (DateTime.TryParse(dRow[dt.Columns[index]].ToString(), out dateTime))
                                                        {
                                                            dRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yy");
                                                        }
                                                    }
                                                    if (rptType.ToString().ToUpper().Trim() != "EXCEL")
                                                    {
                                                        if (dRow[dt.Columns[index]].ToString().Trim().Length > colFVL)
                                                        {
                                                            dRow[dt.Columns[index]] = dRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                                        }
                                                    }
                                                }
                                            }
                                            if (chkbx)
                                            {
                                                if (dRow[dt.Columns[index]].ToString() == "1")
                                                {
                                                    dRow[dt.Columns[index]] = "x";//"&radic;";
                                                }
                                                else if (dRow[dt.Columns[index]].ToString() == "0")
                                                {
                                                    dRow[dt.Columns[index]] = "";
                                                }
                                            }
                                            //Summing the values in each row for that column
                                            if (sumExists)
                                            {
                                                sum = sum + Convert.ToDecimal(dRow[dt.Columns[index]]);
                                            }
                                        }
                                    }
                                }
                                //Adding the row to dt if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    if (dt.Rows.Count == rowCnt)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dt.Rows.Add(dr);
                                    }
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption 
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    if (dt.Columns.Contains("TrxID"))
                    {
                        dt.Columns["TrxID"].SetOrdinal(dt.Columns.Count - 1);
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Gets the DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        //public DataTable XMLToDataTable(string GVXml, string treeNodeName, string rptType)
        public DataTable XMLToDataTable(string GVXml, string treeNodeName, string rptType)
        {
            #region NLog
            logger.Info("Loading the data from the XML file to the DataTable.");
            #endregion

            DataTable dt = new DataTable();
            if (GVXml != string.Empty)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode nodeRows;
                string filenodename = string.Empty;
                xDoc.LoadXml(GVXml);
                nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                if (nodeRows != null)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    int rowCnt = dt.Rows.Count;
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    int colPos = 0;
                    //Storing the columns names and captions in the HashTable
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    //Removing unwanted columns and setting the captions as per the XML.
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;//DateTime column
                        string colName = dt.Columns[index].ColumnName;
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "TrxID")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Set the caption
                        {
                            XmlNode nodeCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim().ToString() + "']");
                            //Checking for isSummed value for that column
                            if (nodeCol != null)
                            {
                                //Checking for IsSummed attribute
                                if (!sumExists)
                                {
                                    if (nodeCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    if (m_htGVColumns.ContainsKey(dt.Columns[index].ColumnName.Trim().ToString()))
                                    {
                                        if (dRow[dt.Columns[index]].ToString() != string.Empty)
                                        {
                                            //Truncating the data if it is greater than its FullView Length
                                            int datavalLength = dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length;
                                            if (colFVL != 0)
                                            {
                                                if (dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length > colFVL)
                                                {
                                                    //Keeping commas if IsNumeric ="1"
                                                    if (numericExists)
                                                    {
                                                        decimal amount;
                                                        if (Decimal.TryParse(dRow[dt.Columns[index]].ToString(), out amount))
                                                        {
                                                            dRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                                        }
                                                    }
                                                    //Formatting Date field 
                                                    if (cal)
                                                    {
                                                        DateTime dateTime;
                                                        if (DateTime.TryParse(dRow[dt.Columns[index]].ToString(), out dateTime))
                                                        {
                                                            dRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yy");
                                                        }
                                                    }
                                                    if (rptType.ToString().ToUpper().Trim() != "EXCEL")
                                                    {
                                                        if (dRow[dt.Columns[index]].ToString().Trim().Length > colFVL)
                                                        {
                                                            dRow[dt.Columns[index]] = dRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                                        }
                                                    }
                                                }
                                            }
                                            if (chkbx)
                                            {
                                                if (dRow[dt.Columns[index]].ToString() == "1")
                                                {
                                                    dRow[dt.Columns[index]] = "x";//"&radic;";
                                                }
                                                else if (dRow[dt.Columns[index]].ToString() == "0")
                                                {
                                                    dRow[dt.Columns[index]] = "";
                                                }
                                            }
                                            //Summing the values in each row for that column
                                            if (sumExists)
                                            {
                                                sum = sum + Convert.ToDecimal(dRow[dt.Columns[index]]);
                                            }
                                        }
                                    }
                                }
                                //Adding the row to dt if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    if (dt.Rows.Count == rowCnt)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dt.Rows.Add(dr);
                                    }
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption 
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    if (dt.Columns.Contains("TrxID"))
                    {
                        dt.Columns["TrxID"].SetOrdinal(dt.Columns.Count - 1);
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Gets Job Costing DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public DataTable GetJobCostingDataToPrint(string GVXml, string treeNodeName, string rptType)
        {
            #region NLog
            logger.Info("Gets Job Costing DataTable to be printed with treenode as : "+treeNodeName+" and report type : "+rptType);
            #endregion

            DataTable dt = new DataTable();
            if (GVXml != string.Empty)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode nodeRows;
                string filenodename = string.Empty;
                xDoc.LoadXml(GVXml);
                nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                if (nodeRows != null)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    int rowCnt = dt.Rows.Count;
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    int colPos = 0;
                    //Storing the columns names and captions in the HashTable
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (treeNodeName == "BudgetTotal")
                        {
                            if (!m_htGVColumns.Contains(currentLabel))
                            {
                                if (currentLabel.Contains("AccountID"))
                                {
                                    m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                    if (!m_htGVColumns.ContainsKey("JobID"))
                                    {
                                        m_htGVColumns.Add("JobID", "JobID");
                                    }
                                }
                                else
                                {
                                    if (node.Attributes["FullViewLength"].Value != "0")
                                    {
                                        m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                    }
                                }
                            }
                        }
                        else if (treeNodeName == "TrxInfo")
                        {
                            if (!m_htGVColumns.Contains(currentLabel))
                            {
                                if (currentLabel.Contains("JobID") || currentLabel.Contains("AccountID"))
                                {
                                    m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                }
                                else
                                {
                                    if (node.Attributes["FullViewLength"].Value != "0")
                                    {
                                        m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!m_htGVColumns.Contains(currentLabel))
                            {
                                if (currentLabel.Contains("JobID") || currentLabel.Contains("AccountID"))
                                {
                                    m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                }
                                else
                                {
                                    if (node.Attributes["FullViewLength"].Value != "0")
                                    {
                                        m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                                    }
                                }
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    //Removing unwanted columns and setting the captions as per the XML.
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;//DateTime column
                        string colName = dt.Columns[index].ColumnName;
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "TrxID")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                            if (treeNodeName == "BudgetTotal")
                            {
                                if (colName.Trim() == "JobID")
                                {
                                    continue;
                                }
                            }
                        }
                        else//Present..Set the caption
                        {
                            XmlNode nodeCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim().ToString() + "']");
                            //Checking for isSummed value for that column
                            if (nodeCol != null)
                            {
                                //Checking for IsSummed attribute
                                if (!sumExists)
                                {
                                    if (nodeCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dRow in dt.Rows)
                                {
                                    if (m_htGVColumns.ContainsKey(dt.Columns[index].ColumnName.Trim().ToString()))
                                    {
                                        if (dRow[dt.Columns[index]].ToString() != string.Empty)
                                        {
                                            //Truncating the data if it is greater than its FullView Length
                                            int datavalLength = dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length;
                                            if (colFVL != 0)
                                            {
                                                if (dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length > colFVL)
                                                {
                                                    //Keeping commas if IsNumeric ="1"
                                                    if (numericExists)
                                                    {
                                                        decimal amount;
                                                        if (Decimal.TryParse(dRow[dt.Columns[index]].ToString(), out amount))
                                                        {
                                                            dRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                                        }
                                                    }
                                                    //Formatting Date field 
                                                    if (cal)
                                                    {
                                                        DateTime dateTime;
                                                        if (DateTime.TryParse(dRow[dt.Columns[index]].ToString(), out dateTime))
                                                        {
                                                            dRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yy");
                                                        }
                                                    }
                                                    //if (rptType.ToString().ToUpper().Trim() != "EXCEL")
                                                    //{
                                                    //    if (dRow[dt.Columns[index]].ToString().Trim().Length > colFVL)
                                                    //    {
                                                    //        dRow[dt.Columns[index]] = dRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                                    //    }
                                                    //}
                                                }
                                            }
                                            if (chkbx)
                                            {
                                                if (dRow[dt.Columns[index]].ToString() == "1")
                                                {
                                                    dRow[dt.Columns[index]] = "x";//"&radic;";
                                                }
                                                else if (dRow[dt.Columns[index]].ToString() == "0")
                                                {
                                                    dRow[dt.Columns[index]] = "";
                                                }
                                            }
                                            //Summing the values in each row for that column
                                            if (sumExists)
                                            {
                                                sum = sum + Convert.ToDecimal(dRow[dt.Columns[index]]);
                                            }
                                        }
                                    }
                                }
                                //Adding the row to dt if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    if (dt.Rows.Count == rowCnt)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dt.Rows.Add(dr);
                                    }
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption 
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    if (dt.Columns.Contains("TrxID"))
                    {
                        dt.Columns["TrxID"].SetOrdinal(dt.Columns.Count - 1);
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable GetReportStyle501AllTotalsToPrint(string GVXml, string branchNodeName)
        {
            #region NLog
            logger.Info("Printing All Totals for the report style 501 with branch node name : "+branchNodeName);
            #endregion

            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);
            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows");
            XmlDocument branchDoc = new XmlDocument();
            //Creating the new node
            XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);
            if (nodeResRows.Count > 0)
            {
                XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                nodeRoot.AppendChild(nodeNewBranch);
                XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                foreach (XmlNode nodeResRow in nodeResRows)
                {
                    nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                }
                if (nodeNewBranchRowlist != null)
                {
                    foreach (XmlNode nodeNewBranchRow in nodeNewBranchRowlist.SelectNodes("Rows"))
                    {
                        if (nodeNewBranchRow.HasChildNodes)
                        {
                            nodeNewBranchRow.InnerXml = string.Empty;
                        }
                    }
                    //To read the data in to dataset
                    XmlNodeReader read = new XmlNodeReader(nodeNewBranchRowlist);
                    Hashtable htColumns = new Hashtable();
                    Hashtable htRemoveColumns = new Hashtable();
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    foreach (DataColumn dc in dt.Columns)
                    {
                        htColumns.Add(dc.Ordinal, dc.ColumnName);
                    }
                    Hashtable htNeedCols = new Hashtable();
                    for (int i = 0; i < 5; i++)
                    {
                        htNeedCols.Add(i, "Range" + (i + 1));
                    }
                    IDictionaryEnumerator enumerator = htNeedCols.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (htColumns.ContainsValue(enumerator.Value))
                        {
                            htRemoveColumns.Add(enumerator.Key, enumerator.Value);
                        }
                    }
                    IDictionaryEnumerator enumerator1 = htRemoveColumns.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        if (htColumns.ContainsValue(enumerator1.Value))
                        {
                            htNeedCols.Remove(enumerator1.Key);
                        }
                    }
                    foreach (DictionaryEntry items in htNeedCols)
                    {
                        DataColumn dcl = new DataColumn();
                        dt.Columns.Add(items.Value.ToString());
                    }
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    //Storing the columns names and captions in the HashTable
                    int colPos = 0;
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;
                        string colName = dt.Columns[index].ColumnName;
                        //Removing unwanted columns
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "Rows_Id")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Setting the captions as per the XML.
                        {
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim() + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                if (!sumExists)
                                {
                                    if (nodeBranchCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeBranchCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeBranchCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dBranchRow in dt.Rows)
                                {
                                    if (dBranchRow[dt.Columns[index]].ToString() != string.Empty)
                                    {
                                        //Keeping commas if IsNumeric ="1"
                                        if (numericExists)
                                        {
                                            decimal amount;
                                            if (Decimal.TryParse(dBranchRow[dt.Columns[index]].ToString(), out amount))
                                            {
                                                dBranchRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                            }
                                        }
                                        //Formatting Date field 
                                        if (cal)
                                        {
                                            DateTime dateTime;
                                            if (DateTime.TryParse(dBranchRow[dt.Columns[index]].ToString(), out dateTime))
                                            {
                                                dBranchRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                        }
                                        if (chkbx)
                                        {
                                            if (dBranchRow[dt.Columns[index]].ToString() == "1")
                                            {
                                                dBranchRow[dt.Columns[index]] = "x";//"&radic;";
                                            }
                                            else if (dBranchRow[dt.Columns[index]].ToString() == "0")
                                            {
                                                dBranchRow[dt.Columns[index]] = "";
                                            }
                                        }
                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            sum = sum + Convert.ToDecimal(dBranchRow[dt.Columns[index]]);
                                        }
                                    }
                                }
                                //Adding the row to BranchDT if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    DataRow dr = dt.NewRow();
                                    if (htNeedCols.ContainsValue(dt.Columns[index].ColumnName))
                                    {
                                        if (sum != 0)
                                        {
                                            dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                        }
                                    }
                                    else
                                    {
                                        if (dt.Rows[dt.Rows.Count - 1][0].ToString() == string.Empty)
                                        {
                                            if (sum != 0)
                                            {
                                                dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                            }
                                        }
                                        else
                                        {
                                            dt.Rows.Add(dr);
                                            if (sum != 0)
                                            {
                                                dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                            }
                                        }
                                    }
                                }
                            }
                            //Changing the column name to caption
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            int drs = dt.Rows.Count - 1;
            for (; drs < dt.Rows.Count; )
            {
                if (drs == dt.Rows.Count)
                {
                    dt.Rows.RemoveAt(drs);
                    drs = drs - 1;
                }
                if (dt.Rows.Count == 1)
                {
                    break;
                }
                else
                {
                    drs = (dt.Rows.Count - 1) - drs;
                    dt.Rows.RemoveAt(drs);
                    drs = dt.Rows.Count - 1;
                }
            }
            return dt;
        }

        public DataTable GetReportStyle501DataToPrint(string GVXml, string parentTrxID, string branchNodeName)
        {
            #region NLog
            logger.Info("Data to print for the report style 501 with Parent Trx Id as : "+parentTrxID+ " and  Branch Node Name : " + branchNodeName);
            #endregion

            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);
            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
            XmlDocument branchDoc = new XmlDocument();
            //Creating the new node
            XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);
            if (nodeResRows.Count > 0)
            {
                XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                nodeRoot.AppendChild(nodeNewBranch);
                XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                foreach (XmlNode nodeResRow in nodeResRows)
                {
                    nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                }
                if (nodeNewBranchRowlist != null)
                {
                    foreach (XmlNode nodeNewBranchRow in nodeNewBranchRowlist.SelectNodes("Rows"))
                    {
                        if (nodeNewBranchRow.HasChildNodes)
                        {
                            nodeNewBranchRow.InnerXml = string.Empty;
                        }
                    }
                    //To read the data in to dataset
                    XmlNodeReader read = new XmlNodeReader(nodeNewBranchRowlist);
                    Hashtable htColumns = new Hashtable();
                    Hashtable htRemoveColumns = new Hashtable();
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    foreach (DataColumn dc in dt.Columns)
                    {
                        htColumns.Add(dc.Ordinal, dc.ColumnName);
                    }
                    Hashtable htNeedCols = new Hashtable();
                    for (int i = 0; i < 5; i++)
                    {
                        htNeedCols.Add(i, "Range" + (i + 1));
                    }
                    IDictionaryEnumerator enumerator = htNeedCols.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (htColumns.ContainsValue(enumerator.Value))
                        {
                            htRemoveColumns.Add(enumerator.Key, enumerator.Value);
                        }
                    }
                    IDictionaryEnumerator enumerator1 = htRemoveColumns.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        if (htColumns.ContainsValue(enumerator1.Value))
                        {
                            htNeedCols.Remove(enumerator1.Key);
                        }
                    }
                    foreach (DictionaryEntry items in htNeedCols)
                    {
                        DataColumn dcl = new DataColumn();
                        dt.Columns.Add(items.Value.ToString());
                    }
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    //Storing the columns names and captions in the HashTable
                    int colPos = 0;
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;
                        string colName = dt.Columns[index].ColumnName;
                        //Removing unwanted columns
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "Rows_Id")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Setting the captions as per the XML.
                        {
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim() + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                if (!sumExists)
                                {
                                    if (nodeBranchCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeBranchCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeBranchCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dBranchRow in dt.Rows)
                                {
                                    if (dBranchRow[dt.Columns[index]].ToString() != string.Empty)
                                    {
                                        //Keeping commas if IsNumeric ="1"
                                        if (numericExists)
                                        {
                                            decimal amount;
                                            if (Decimal.TryParse(dBranchRow[dt.Columns[index]].ToString(), out amount))
                                            {
                                                dBranchRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                            }
                                        }
                                        //Formatting Date field 
                                        if (cal)
                                        {
                                            DateTime dateTime;
                                            if (DateTime.TryParse(dBranchRow[dt.Columns[index]].ToString(), out dateTime))
                                            {
                                                dBranchRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                        }
                                        if (chkbx)
                                        {
                                            if (dBranchRow[dt.Columns[index]].ToString() == "1")
                                            {
                                                dBranchRow[dt.Columns[index]] = "x";//"&radic;";
                                            }
                                            else if (dBranchRow[dt.Columns[index]].ToString() == "0")
                                            {
                                                dBranchRow[dt.Columns[index]] = "";
                                            }
                                        }
                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            if (!string.IsNullOrEmpty(dBranchRow[dt.Columns[index]].ToString()))
                                            {
                                                sum = sum + Convert.ToDecimal(dBranchRow[dt.Columns[index]]);
                                            }
                                        }
                                    }
                                }
                                //Adding the row to BranchDT if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    DataRow dr = dt.NewRow();
                                    if (htNeedCols.ContainsValue(dt.Columns[index].ColumnName))
                                    {
                                        if (sum != 0)
                                        {
                                            dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                        }
                                    }
                                    else
                                    {
                                        if (dt.Rows[dt.Rows.Count - 1][0].ToString() == string.Empty)
                                        {
                                            if (sum != 0)
                                            {
                                                dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                            }
                                        }
                                        else
                                        {
                                            dt.Rows.Add(dr);
                                            if (sum != 0)
                                            {
                                                dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                            }
                                        }
                                    }
                                }
                            }
                            //Changing the column name to caption
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Gets the Branch DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public DataTable GetBranchDataToPrint(string GVXml, string parentTrxID, string branchNodeName)
        {
            #region NLog
            logger.Info("Gets the Branch DataTable to be printed.");
            #endregion

            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);
            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
            XmlDocument branchDoc = new XmlDocument();
            //Creating the new node
            XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);
            if (nodeResRows.Count > 0)
            {
                XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                nodeRoot.AppendChild(nodeNewBranch);
                XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                foreach (XmlNode nodeResRow in nodeResRows)
                {
                    nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                }
                if (nodeNewBranchRowlist != null)
                {
                    foreach (XmlNode nodeNewBranchRow in nodeNewBranchRowlist.SelectNodes("Rows"))
                    {
                        if (nodeNewBranchRow.HasChildNodes)
                        {
                            nodeNewBranchRow.InnerXml = string.Empty;
                        }
                    }
                    //To read the data in to dataset
                    XmlNodeReader read = new XmlNodeReader(nodeNewBranchRowlist);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    //Storing the columns names and captions in the HashTable
                    int colPos = 0;
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;
                        string colName = dt.Columns[index].ColumnName;
                        //Removing unwanted columns
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "Rows_Id")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Setting the captions as per the XML.
                        {
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim() + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                if (!sumExists)
                                {
                                    if (nodeBranchCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeBranchCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeBranchCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dBranchRow in dt.Rows)
                                {
                                    if (dBranchRow[dt.Columns[index]].ToString() != string.Empty)
                                    {
                                        //Keeping commas if IsNumeric ="1"
                                        if (numericExists)
                                        {
                                            decimal amount;
                                            if (Decimal.TryParse(dBranchRow[dt.Columns[index]].ToString(), out amount))
                                            {
                                                dBranchRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                            }
                                        }
                                        //Formatting Date field 
                                        if (cal)
                                        {
                                            DateTime dateTime;
                                            if (DateTime.TryParse(dBranchRow[dt.Columns[index]].ToString(), out dateTime))
                                            {
                                                dBranchRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                        }
                                        if (chkbx)
                                        {
                                            if (dBranchRow[dt.Columns[index]].ToString() == "1")
                                            {
                                                dBranchRow[dt.Columns[index]] = "x";//"&radic;";
                                            }
                                            else if (dBranchRow[dt.Columns[index]].ToString() == "0")
                                            {
                                                dBranchRow[dt.Columns[index]] = "";
                                            }
                                        }
                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            sum = sum + Convert.ToDecimal(dBranchRow[dt.Columns[index]]);
                                        }
                                    }
                                }
                                //Adding the row to BranchDT if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    DataRow dr = dt.NewRow();
                                    dt.Rows.Add(dr);
                                    //rowsInBranchDT = BranchDT.Rows.Count;
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }



        /// <summary>
        /// This Data for report style 201
        /// </summary>
        /// <returns></returns>
        public DataTable RP201GetBranchDataToPrint(string GVXml, string parentTrxID, string branchNodeName, Hashtable htFontFormats)
        {
            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);
            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
            XmlDocument branchDoc = new XmlDocument();
            //Creating the new node
            XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);
            if (nodeResRows.Count > 0)
            {
                XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                nodeRoot.AppendChild(nodeNewBranch);
                XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                foreach (XmlNode nodeResRow in nodeResRows)
                {
                    nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                }
                if (nodeNewBranchRowlist != null)
                {
                    foreach (XmlNode nodeNewBranchRow in nodeNewBranchRowlist.SelectNodes("Rows"))
                    {
                        if (nodeNewBranchRow.HasChildNodes)
                        {
                            nodeNewBranchRow.InnerXml = string.Empty;
                        }
                    }
                    //To read the data in to dataset
                    XmlNodeReader read = new XmlNodeReader(nodeNewBranchRowlist);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable  
                    dt = dsRows.Tables[0];
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    htFontFormats.Clear();
                    //Storing the columns names and captions in the HashTable
                    int colPos = 0;
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;
                        string colName = dt.Columns[index].ColumnName;
                        //Removing unwanted columns
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "Rows_Id")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Setting the captions as per the XML.
                        {
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName.Trim() + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                if (!sumExists)
                                {
                                    if (nodeBranchCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeBranchCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeBranchCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                int rowFont = 0;
                                foreach (DataRow dBranchRow in dt.Rows)
                                {
                                    if (dBranchRow[dt.Columns[index]].ToString() != string.Empty)
                                    {
                                        //Keeping commas if IsNumeric ="1"
                                        if (numericExists)
                                        {
                                            decimal amount;
                                            if (Decimal.TryParse(dBranchRow[dt.Columns[index]].ToString(), out amount))
                                            {
                                                dBranchRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                            }
                                        }
                                        //Formatting Date field 
                                        if (cal)
                                        {
                                            DateTime dateTime;
                                            if (DateTime.TryParse(dBranchRow[dt.Columns[index]].ToString(), out dateTime))
                                            {
                                                dBranchRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                        }
                                        if (chkbx)
                                        {
                                            if (dBranchRow[dt.Columns[index]].ToString() == "1")
                                            {
                                                dBranchRow[dt.Columns[index]] = "x";//"&radic;";
                                            }
                                            else if (dBranchRow[dt.Columns[index]].ToString() == "0")
                                            {
                                                dBranchRow[dt.Columns[index]] = "";
                                            }
                                        }
                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            sum = sum + Convert.ToDecimal(dBranchRow[dt.Columns[index]]);
                                        }
                                        //Add formats

                                        // dBranchRow[dt.Columns[index]]
                                        string strfonts = string.Empty;
                                        if (dt.Columns.Contains("LargeFont"))
                                        {
                                            strfonts = strfonts + "LF~" + dBranchRow[dt.Columns["LargeFont"]].ToString() + ";";
                                        }
                                        if (dt.Columns.Contains("SmallFont"))
                                        {
                                            strfonts = strfonts + "SF~" + dBranchRow[dt.Columns["SmallFont"]].ToString() + ";";
                                        }
                                        if (dt.Columns.Contains("UnderLine"))
                                        {
                                            strfonts = strfonts + "UL~" + dBranchRow[dt.Columns["UnderLine"]].ToString() + ";";
                                        }
                                        if (dt.Columns.Contains("Shade"))
                                        {
                                            strfonts = strfonts + "SH~" + dBranchRow[dt.Columns["Shade"]].ToString() + ";";
                                        }

                                        if (strfonts != string.Empty)
                                        {
                                            strfonts = strfonts.Remove(strfonts.Length - 1, 1);
                                            if (!(bool)htFontFormats.ContainsKey(rowFont))
                                            {
                                                htFontFormats.Add(rowFont, strfonts);
                                            }
                                        }
                                    }
                                    rowFont++;
                                }
                                //Adding the row to BranchDT if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    DataRow dr = dt.NewRow();
                                    dt.Rows.Add(dr);
                                    //rowsInBranchDT = BranchDT.Rows.Count;
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    //Inserting empty rows with EMPTY CELL
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() == string.Empty)
                                {
                                    dt.Rows[rwCnt][colcnt] = "EMPTY"; 
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }


        /// <summary>
        /// Gets the Job Costing Branch DataTable to be printed
        /// </summary>
        public DataTable GetJobCostingBranchDataToPrint(string GVXml, string parentTrxID, string branchNodeName, string branchIDName)
        {
            #region NLog
            logger.Info("Gets the Job Costing Branch DataTable to be printed.");
            #endregion

            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);
            //Get the Grid Layout nodes
            XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + branchIDName + " = '" + parentTrxID + "']");
            XmlDocument branchDoc = new XmlDocument();
            //Creating the new node
            XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);
            if (nodeResRows.Count > 0)
            {
                XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                nodeRoot.AppendChild(nodeNewBranch);
                XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                foreach (XmlNode nodeResRow in nodeResRows)
                {
                    nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                }
                if (nodeNewBranchRowlist != null)
                {
                    foreach (XmlNode nodeNewBranchRow in nodeNewBranchRowlist.SelectNodes("Rows"))
                    {
                        if (nodeNewBranchRow.HasChildNodes)
                        {
                            nodeNewBranchRow.InnerXml = string.Empty;
                        }
                    }
                    XmlNodeReader read = new XmlNodeReader(nodeNewBranchRowlist);
                    DataSet dsRows = new DataSet();
                    dsRows.ReadXml(read);
                    //Getting the datatable                
                    dt = dsRows.Tables[0];
                    //Getting the columns to be displayed in grid
                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                    m_htGVColumns.Clear();
                    //Storing the columns names and captions in the HashTable
                    int colPos = 0;
                    foreach (XmlNode node in nodeCols)
                    {
                        string currentLabel = node.Attributes["Label"].Value;
                        if (!m_htGVColumns.Contains(currentLabel))
                        {
                            if (node.Attributes["FullViewLength"].Value != "0")
                            {
                                m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
                            }
                        }
                        //Set the column order in the DataTable(RowsList) as per the Columns Node
                        DataColumn dc = dt.Columns[currentLabel];
                        if (dc != null && dc.Ordinal != colPos)
                        {
                            int tableColPos = colPos;
                            if (tableColPos >= dt.Columns.Count)
                            {
                                tableColPos = dt.Columns.Count - 1;//Put it at the last
                            }
                            dc.SetOrdinal(tableColPos);
                        }
                        colPos++;
                    }
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        bool numericExists = false;
                        bool sumExists = false;
                        bool chkbx = false;
                        bool cal = false;
                        string colName = dt.Columns[index].ColumnName;
                        //Removing unwanted columns
                        if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                        {
                            if (colName.Trim() != "Notes" && colName.Trim() != "Rows_Id")
                            {
                                dt.Columns.Remove(colName);
                                index--;
                            }
                        }
                        else//Present..Setting the captions as per the XML.
                        {
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dt.Columns[index].ColumnName + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                if (!sumExists)
                                {
                                    if (nodeBranchCol.Attributes["IsSummed"] != null)
                                    {
                                        if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                        {
                                            sumExists = true;
                                        }
                                    }
                                }
                                if (nodeBranchCol.Attributes["ControlType"] != null)
                                {
                                    if (nodeBranchCol.Attributes["ControlType"].Value == "Check")
                                    {
                                        chkbx = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                    {
                                        numericExists = true;
                                    }
                                    else if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                    {
                                        cal = true;
                                    }
                                }
                                int colFVL = 0;
                                if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                {
                                    colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                }
                                decimal sum = 0;
                                //Iterating through each row and truncating the data and summing value 
                                foreach (DataRow dBranchRow in dt.Rows)
                                {
                                    if (dBranchRow[dt.Columns[index]].ToString() != string.Empty)
                                    {
                                        //Keeping commas if IsNumeric ="1"
                                        if (numericExists)
                                        {
                                            decimal amount;
                                            if (Decimal.TryParse(dBranchRow[dt.Columns[index]].ToString(), out amount))
                                            {
                                                dBranchRow[dt.Columns[index]] = string.Format("{0:N}", amount);
                                            }
                                        }
                                        //Formatting Date field 
                                        if (cal)
                                        {
                                            DateTime dateTime;
                                            if (DateTime.TryParse(dBranchRow[dt.Columns[index]].ToString(), out dateTime))
                                            {
                                                dBranchRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                        }
                                        if (chkbx)
                                        {
                                            if (dBranchRow[dt.Columns[index]].ToString() == "1")
                                            {
                                                dBranchRow[dt.Columns[index]] = "x";//"&radic;";
                                            }
                                            else if (dBranchRow[dt.Columns[index]].ToString() == "0")
                                            {
                                                dBranchRow[dt.Columns[index]] = "";
                                            }
                                        }
                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            sum = sum + Convert.ToDecimal(dBranchRow[dt.Columns[index]]);
                                        }
                                    }
                                }
                                //Adding the row to BranchDT if isSummed exists and updating the row count
                                if (sumExists)
                                {
                                    DataRow dr = dt.NewRow();
                                    dt.Rows.Add(dr);
                                    dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
                                }
                            }
                            //Changing the column name to caption
                            dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
                        }
                    }
                    //Removing empty rows
                    for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
                    {
                        bool emptyRow = true;
                        for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
                        {
                            if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
                            {
                                if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
                                    emptyRow = false;
                            }
                        }
                        if (emptyRow)
                        {
                            dt.Rows[rwCnt].Delete();
                            rwCnt--;
                        }
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// Gets the Header Job Costing DataTable to be printed
        /// </summary>
        /// <param name="Title">Title of the report</param>
        /// <param name=""></param>
        /// <returns>DataTable.</returns
        public DataTable GetHeaderJobCostingDT(string title, string subTitle, XmlDocument xdoc, string JobID)
        {
            #region NLog
            logger.Info("Gets the Header Job Costing DataTable to be printed.");
            #endregion

            string dDirector = string.Empty;
            string dProducer = string.Empty;
            string dSalesRep = string.Empty;
            //
            XmlDocument xBPE = new XmlDocument();
            string userName = string.Empty;
            xBPE = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            //
            DataTable HeaderDT = new DataTable();
            for (int col = 0; col < 4; col++)
            {
                DataColumn colNames = new DataColumn();
                colNames.ColumnName = "";
                HeaderDT.Columns.Add(colNames);
            }
            //
            DataRow dCompanyName = HeaderDT.NewRow();
            dCompanyName[0] = "Project Name:" + xdoc.SelectSingleNode("Root/bpeout/FormControls/Job/RowList/Rows[@TrxID='" + JobID + "']").Attributes["Project"].Value;
            dCompanyName[1] = xBPE.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;
            dCompanyName[2] = "Date Time : " + DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            //
            DataRow dDirectorRow = HeaderDT.NewRow();
            XmlAttribute xDirector = xdoc.SelectSingleNode("Root/bpeout/FormControls/Job/RowList/Rows[@TrxID='" + JobID + "']").Attributes["Director"];
            if (xDirector != null)
            {
                dDirectorRow[0] = "Director : " + xDirector.Value;
            }
            else
            {
                dDirectorRow[0] = "Director : " + string.Empty;
            }
            if (title != null)
            {
                dDirectorRow[1] = title;
            }
            else
            {
                dDirectorRow[1] = string.Empty;
            }
            //
            DataRow dProducerRow = HeaderDT.NewRow();
            XmlAttribute xProducer = xdoc.SelectSingleNode("Root/bpeout/FormControls/Job/RowList/Rows[@TrxID='" + JobID + "']").Attributes["Producer"];
            if (xProducer != null)
            {
                dProducerRow[0] = "Producer : " + xProducer.Value;
            }
            else
            {
                dProducerRow[0] = "Producer : " + string.Empty;
            }
            //
            if (subTitle != null)
            {
                dProducerRow[1] = subTitle;
            }
            else
            {
                dProducerRow[1] = string.Empty;
            }
            //
            DataRow dSalesRepRow = HeaderDT.NewRow();
            XmlAttribute xSalerRep = xdoc.SelectSingleNode("Root/bpeout/FormControls/Job/RowList/Rows[@TrxID='" + JobID + "']").Attributes["SalesRep"];
            if (xSalerRep != null)
            {
                dSalesRepRow[0] = "Sales Rep : " + xSalerRep.Value;
            }
            else
            {
                dSalesRepRow[0] = "Sales Rep : " + string.Empty;
            }
            //
            XmlNode nodeSmallName = xBPE.SelectSingleNode("Root/bpe/userinfo/SmallName");
            if (nodeSmallName != null)
            {
                userName = nodeSmallName.InnerText.Trim().ToString().ToUpper();
            }
            else
            {
                userName = string.Empty;
            }
            dDirectorRow[2] = "Requested By: " + userName;
            //
            HeaderDT.Rows.Add(dCompanyName);
            HeaderDT.Rows.Add(dDirectorRow);
            HeaderDT.Rows.Add(dProducerRow);
            HeaderDT.Rows.Add(dSalesRepRow);
            //
            return HeaderDT;
        }

        /// <summary>
        /// Gets the Header DataTable to be printed
        /// </summary>
        /// <param name="Title">Title of the report</param>
        /// <param name=""></param>
        /// <returns>DataTable.</returns
        public DataTable GetHeaderDT(string title, string subTitle)
        {
            #region NLog
            logger.Info("Gets the Header DataTable to be printed with Title as : "+title+" and Sub Title as : "+subTitle );
            #endregion

            XmlDocument XDocUserInfo = new XmlDocument();
            DataTable HeaderDT = new DataTable();
            for (int col = 0; col < 4; col++)
            {
                DataColumn colNames = new DataColumn();
                colNames.ColumnName = "";
                HeaderDT.Columns.Add(colNames);
            }
            //
            DataRow dCompanyRow = HeaderDT.NewRow();
            DataRow dHeaderRow = HeaderDT.NewRow();
            //
            CommonUI commonObjUI = new CommonUI();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string userName = string.Empty;
            //
            XmlNode nodeSmallName = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/SmallName");
            if (nodeSmallName != null)
            {
                userName = nodeSmallName.InnerText.Trim().ToString().ToUpper();
            }
            dCompanyRow[0] = "Requested By : " + userName;
            dCompanyRow[1] = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;
            //
            XmlNode nodeGMT = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/GMTAdjustment");
            XmlNode nodeDayLight = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/DayLightSaving");
            int DayLightSavingVal = Convert.ToInt32(nodeDayLight.InnerText);
            string gmtValue = nodeGMT.InnerText.ToString();
            int gmtHr = 0;
            int gmtMin = 0;

            if (gmtValue == "0")
            {
                gmtValue = ConfigurationManager.AppSettings["DefGMT"].ToString();
            }

            if (gmtValue.Contains(":"))
            {
                string[] splitTime = new string[2];
                splitTime = gmtValue.Split(':');
                //
                gmtHr = Convert.ToInt32(splitTime[0].ToString());
                gmtMin = Convert.ToInt32(splitTime[1].ToString());
            }
            else if (gmtValue.Contains("."))
            {
                string[] splitTime = new string[2];
                splitTime = gmtValue.Split('.');
                //
                gmtHr = Convert.ToInt32(splitTime[0].ToString());
                gmtMin = Convert.ToInt32(splitTime[1].ToString());
            }
            else
            {
                gmtHr = Convert.ToInt32(gmtValue);
                gmtMin = 0;
            }
            // condition for DayLightSaving
            if (DayLightSavingVal > 0)
            {
                gmtHr = gmtHr - DayLightSavingVal;
            }
            DateTime dtGMT = DateTime.Now.ToUniversalTime() + new TimeSpan(gmtHr, gmtMin, 0);
            dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + dtGMT.Hour + ":" + dtGMT.Minute + ":00";


            #region commented
            //DateTime dtGMT = DateTime.UtcNow;
            //if (nodeGMT != null)
            //{
            //    string gmtValue = nodeGMT.InnerText.ToString();
            //    int curMM = 0;
            //    int gmtMM = 0;
            //    int remMM = 0;
            //    TimeSpan timeSpan = new TimeSpan();
            //    if (gmtValue.Contains(":"))
            //    {
            //        string[] splitTime = new string[2];
            //        splitTime = gmtValue.Split(':');
            //        //
            //        int splitTimeHH = Convert.ToInt32(splitTime[0].ToString());
            //        int splitTimeMM = Convert.ToInt32(splitTime[1].ToString());
            //        //
            //        string splittedValues = string.Empty;

            //        if (splitTimeHH.ToString().Contains("-"))
            //        {
            //            splittedValues = splitTimeHH.ToString().Replace("-", "");
            //            gmtMM = Convert.ToInt32(splittedValues) * 60 + splitTimeMM;
            //            curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //            remMM = gmtMM - curMM;
            //            if (remMM.ToString().Contains("-"))
            //            {
            //                remMM = Convert.ToInt32(remMM.ToString().Replace("-", ""));
            //            }
            //        }
            //        else
            //        {
            //            gmtMM = Convert.ToInt32(splitTimeHH) * 60 + Convert.ToInt32(splitTimeMM);
            //            curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //            remMM = gmtMM + curMM;
            //        }
            //    }
            //    else
            //    {
            //        curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //        gmtMM = Convert.ToInt32(gmtValue) * 60;
            //        remMM = gmtMM + curMM;
            //    }
            //    timeSpan = TimeSpan.FromMinutes(remMM);
            //    // gives you the rounded down value 
            //    int hours = timeSpan.Hours;
            //    // gives you the minutes left of the hour
            //    int minutes = remMM - (hours * 60);
            //    string mins = string.Empty;
            //    if (minutes.ToString().Length == 1)
            //    {
            //        mins = "0" + Convert.ToString(minutes);
            //    }
            //    else
            //    {
            //        mins = Convert.ToString(minutes);
            //    }
            //    dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + hours + ":" + mins + ":00";
            //}
            //else
            //{
            //    dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + dtGMT.Hour.ToString() + ":" + dtGMT.Minute.ToString() + ":" + dtGMT.Second.ToString();
            //}
            //
            #endregion
            if (title != null)
            {
                dHeaderRow[1] = title;
            }
            else
            {
                dHeaderRow[1] = string.Empty;
            }
            HeaderDT.Rows.Add(dCompanyRow);
            HeaderDT.Rows.Add(dHeaderRow);
            //
            if (subTitle != string.Empty)
            {
                DataRow dHeaderTimeRow = HeaderDT.NewRow();
                dHeaderTimeRow[1] = subTitle;
                HeaderDT.Rows.Add(dHeaderTimeRow);
            }
            return HeaderDT;
        }

        /// <summary>
        /// Creating The Process Links 
        /// </summary>
        /// <param name="pageXML"></param>
        /// <returns></returns>
        public Table CreateProcessLinks(string pageXML)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(pageXML);
            XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (nodeBPC == null)
            {
                //No links to add so return an empty table.
                return new Table();
            }
            ArrayList arrFormLevelProcs = new ArrayList();
            foreach (XmlNode nodeBP in nodeBPC.ChildNodes)
            {
                if (nodeBP.Attributes["ID"] != null)
                {
                    if (!arrFormLevelProcs.Contains(nodeBP.Attributes["ID"].Value.Trim()))
                    {
                        arrFormLevelProcs.Add(nodeBP.Attributes["ID"].Value.Trim());
                    }
                }
            }
            string treeNodeName = string.Empty;
            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            {
                treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            }
            Table tblProcessLinks = new Table();
            //Add a new row to the table.
            TableCell tcNBSP = new TableCell();
            tcNBSP.Width = Unit.Pixel(6);
            TableRow tr = new TableRow();
            tr.ID = "trDynamicProcessLinks";
            tblProcessLinks.Rows.Add(tr);
            int rowCntr = 0;
            foreach (string process in arrFormLevelProcs)
            {
                XmlNode nodeProc = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + process + "']");
                string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                string pageInfo = nodeProc.Attributes["PageInfo"].Value;
                string confirmMessage = string.Empty;
                //Get the attribute which the text to be displayed while confirming the execution of the proc.
                XmlAttribute attConfirm = nodeProc.Attributes["MsgPrompt"];
                if (attConfirm != null)
                {
                    confirmMessage = attConfirm.Value;
                    confirmMessage = confirmMessage.Replace("'", "\\'").Replace(" ", "~::~"); ;
                    confirmMessage = commonObjUI.HtmlEncode(confirmMessage);
                }

                Label lblCurrent = new Label();
                lblCurrent.Text = "<a oncontextmenu='return false;' Title='" + nodeProc.Attributes["Label"].Value
               + "'" + " href=javascript:OnProcessBPCLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','"
               + "" + "','" + nodeProc.Attributes["ID"].Value + "','" + 1 + "','" + treeNodeName + "','" + confirmMessage + "')>"
               + nodeProc.Attributes["Label"].Value + "</a>";
                TableCell tdReport = new TableCell();
                tdReport.Controls.Add(lblCurrent);
                tr.Cells.Add(tdReport);
                //TD for the separator
                TableCell tdLinkSeparator = new TableCell();
                tdLinkSeparator.Width = Unit.Pixel(5);
                tdLinkSeparator.Text = "|";
                tr.Cells.Add(tdLinkSeparator);
                //Accomodates 7(the number/2) links in each row.
                if (tr.Cells.Count >= 14)
                {
                    //Remove the trailing separator before going to a new row.
                    tr.Cells.Remove(tr.Cells[tr.Cells.Count - 1]);
                    tr = new TableRow();
                    tblProcessLinks.Rows.Add(tr);
                    rowCntr++;
                }
            }
            //Remove the trailing separator for each row.
            foreach (TableRow trToRemove in tblProcessLinks.Rows)
            {
                if (trToRemove.Cells.Count > 0)
                {
                    trToRemove.Cells.Remove(trToRemove.Cells[trToRemove.Cells.Count - 1]);
                    trToRemove.Cells.Add(tcNBSP);
                }
            }
            return tblProcessLinks;
        }

        /// <summary>
        /// Creates the Process Links for Process Engine.aspx
        /// </summary>
        /// <param name="pageXML"></param>
        /// <returns></returns>
        public Table CreateProcessLinksTable(string pageXML)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(pageXML);
            XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (nodeBPC == null)
            {
                //No links to add so return an empty table.
                return new Table();
            }
            ArrayList arrFormLevelProcs = new ArrayList();
            foreach (XmlNode nodeBP in nodeBPC.ChildNodes)
            {
                if (nodeBP.Attributes["ID"] != null)
                {
                    if (!arrFormLevelProcs.Contains(nodeBP.Attributes["ID"].Value.Trim()))
                    {
                        arrFormLevelProcs.Add(nodeBP.Attributes["ID"].Value.Trim());
                    }
                }
            }
            Table tblProcessLinks = new Table();
            //Add a new row to the table.
            TableCell tcNBSP = new TableCell();
            tcNBSP.Width = Unit.Pixel(6);
            TableRow tr = new TableRow();
            tr.ID = "trDynamicProcessLinks";
            tblProcessLinks.Rows.Add(tr);
            int rowCntr = 0;
            foreach (string process in arrFormLevelProcs)
            {
                XmlNode nodeProc = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + process + "']");
                string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                string pageInfo = nodeProc.Attributes["PageInfo"].Value;
                LAjitLinkButton lnkBtnReport = new LAjitLinkButton();
                lnkBtnReport.MapXML = currentBPGID;
                lnkBtnReport.Text = nodeProc.Attributes["Label"].Value;
                lnkBtnReport.ID = "lnkBtnReport" + currentBPGID;//"lnkBtnReport";
                lnkBtnReport.ValidationGroup = "LAJITEntryForm";
                lnkBtnReport.Attributes.Add("oncontextmenu", "return false;");
                if (lnkBtnReport.Text.Contains("Create Invoice"))
                {
                    lnkBtnReport.OnClientClick = "return ValidateSelection();";
                }
                lnkBtnReport.Click += new EventHandler(lnkBtnReport_Click);
                TableCell tdReport = new TableCell();
                tdReport.Controls.Add(lnkBtnReport);
                tr.Cells.Add(tdReport);
                //TD for the separator
                TableCell tdLinkSeparator = new TableCell();
                tdLinkSeparator.Width = Unit.Pixel(5);
                tdLinkSeparator.Text = "|";
                tr.Cells.Add(tdLinkSeparator);
                //Accomodates 7(the number/2) links in each row.
                if (tr.Cells.Count >= 14)
                {
                    //Remove the trailing separator before going to a new row.
                    tr.Cells.Remove(tr.Cells[tr.Cells.Count - 1]);
                    tr = new TableRow();
                    tblProcessLinks.Rows.Add(tr);
                    rowCntr++;
                }
            }
            //Remove the trailing separator for each row.
            foreach (TableRow trToRemove in tblProcessLinks.Rows)
            {
                if (trToRemove.Cells.Count > 0)
                {
                    trToRemove.Cells.Remove(trToRemove.Cells[trToRemove.Cells.Count - 1]);
                    trToRemove.Cells.Add(tcNBSP);
                }
            }
            return tblProcessLinks;
        }

        /// <summary>
        /// Link Button Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lnkBtnReport_Click(object sender, EventArgs e)
        {
            Page currentPage = ((LAjitLinkButton)sender).Page;
            LAjitLinkButton lnkBtn = (LAjitLinkButton)sender;
            string currentBPGID = string.Empty;
            currentBPGID = lnkBtn.MapXML;
            Page pg = (Page)currentPage.TemplateControl;
            PropertyInfo pInfo = pg.GetType().GetProperty("RetXML");
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(pInfo.GetValue(pg, null).ToString());
            ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)currentPage.Master.FindControl("cphPageContents");
            if (cntPlaceHolder != null)
            {
                Panel panel = (Panel)cntPlaceHolder.FindControl("pnlContent");
                if (panel != null)
                {
                    Panel pnlEntryForm = (Panel)panel.FindControl("pnlEntryForm");
                    if (pnlEntryForm != null)
                    {
                        Label lblmsg = (Label)pnlEntryForm.FindControl("lblmsg");
                        if (lblmsg != null)
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = string.Empty;
                        }
                        string m_treeNodeName = string.Empty;
                        if (xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                        {
                            m_treeNodeName = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                        }
                        XmlNode xNodeRow = xdoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows");
                        commonObjUI.GVSelectedRow = xNodeRow;
                        if (currentPage.ToString().Contains("_processengine_aspx"))
                        {
                            string strOUTXML = string.Empty;
                            strOUTXML = SubmitReport("Report", pnlEntryForm, currentBPGID);
                            HttpContext.Current.Session["strOUTXML"] = strOUTXML;
                        }
                        else
                        {
                            //Generate the request XML
                            string requestXML = GetReportRequestXML(pnlEntryForm, currentBPGID);
                            XmlDocument xDocBPInfo = new XmlDocument();
                            xDocBPInfo.LoadXml(requestXML);
                            XmlNode nodeBPInfo = xDocBPInfo.SelectSingleNode("Root/bpinfo");
                            if (nodeBPInfo != null)
                            {
                                //Get rid of the Gridview node
                                XmlNode nodeGridView = nodeBPInfo.SelectSingleNode("//Gridview");
                                if (nodeGridView != null)
                                {
                                    nodeGridView.ParentNode.RemoveChild(nodeGridView);
                                }
                                //Set the Session with the BPEInfo so that the same can be requested in the redirect page.
                                //SessionLinkBPInfo = nodeBPInfo.OuterXml;
                                //string qspDepth = currentPage.Request.QueryString["depth"];
                                //int popUpDepth = 0;
                                //if (!string.IsNullOrEmpty(qspDepth))
                                //{
                                //    popUpDepth = Convert.ToInt32(qspDepth);
                                //}
                                //if (popUpDepth > 1)
                                //{
                                //    SessionLinkBPInfo = nodeBPInfo.OuterXml;
                                //}
                                //else
                                //{
                                SessionBPInfo = "1" + nodeBPInfo.OuterXml;//1 - Clear the BPINFO once its has been requested, handled in basepage load.
                                //}
                                //Close the current Iframe and post the page to that of the parent's.
                                string js = "javascript:CreatePaymentsForPOs();"; //JS function can be found in SelectRequest.aspx
                                ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "PO", js, true);
                            }
                        }
                        //Finding Pagelevel Update Panel
                        UpdatePanel updPnlPrint = (UpdatePanel)cntPlaceHolder.FindControl("updPnlPrint");
                        Panel pnlPrint = null;
                        if (updPnlPrint != null)
                        {
                            pnlPrint = (Panel)updPnlPrint.FindControl("pnlPrint");
                            string s = "javascript:Openframe('iframePrint','PrintPopUp.aspx?DisplayDetails=0');"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
                            ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "Print", s, true);
                            pnlPrint.Height = Unit.Pixel(490);
                            pnlPrint.Width = Unit.Pixel(932);
                            HiddenField hdnResolution = (HiddenField)currentPage.Master.FindControl("hdnResolution");
                            string[] coordinates = hdnResolution.Value.Split('x');
                            //AjaxControlToolkit.ModalPopupExtender mpePrint = (AjaxControlToolkit.ModalPopupExtender)updPnlPrint.FindControl("mpePrint");
                            //mpePrint.DropShadow = false;
                            //mpePrint.PopupControlID = "pnlPrint";
                            //mpePrint.Show();
                        }
                    }
                }
            }
        }

        #region Number To Words
        /* public string ConvertNumberToWord(long nNumber)
        {
            long CurrentNumber = nNumber;
            string sReturn = "";
            if (CurrentNumber >= 1000000000)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber / 1000000000, "Billion");
                CurrentNumber = CurrentNumber % 1000000000;
            }
            if (CurrentNumber >= 1000000)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber / 1000000, "Million");
                CurrentNumber = CurrentNumber % 1000000;
            }
            if (CurrentNumber >= 1000)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber / 1000, "Thousand");
                CurrentNumber = CurrentNumber % 1000;
            }
            if (CurrentNumber >= 100)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber / 100, "Hundred");
                CurrentNumber = CurrentNumber % 100;
            }
            if (CurrentNumber >= 20)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber, "");
                CurrentNumber = CurrentNumber % 10;
            }
            else if (CurrentNumber > 0)
            {
                sReturn = sReturn + " " + GetWord(CurrentNumber, "");
                CurrentNumber = 0;
            }
            return sReturn.Replace("  ", " ").Trim();
        }

        private string GetWord(long nNumber, string sPrefix)
        {
            long nCurrentNumber = nNumber;
            string sReturn = "";
            while (nCurrentNumber > 0)
            {
                if (nCurrentNumber > 100)
                {
                    sReturn = sReturn + " " + GetWord(nCurrentNumber / 100, "Hundred");
                    nCurrentNumber = nCurrentNumber % 100;
                }
                else if (nCurrentNumber > 20)
                {
                    sReturn = sReturn + " " + GetTwentyWord(nCurrentNumber / 10);
                    nCurrentNumber = nCurrentNumber % 10;
                }
                else
                {
                    sReturn = sReturn + " " + GetLessThanTwentyWord(nCurrentNumber);
                    nCurrentNumber = 0;
                }
            }
            sReturn = sReturn + " " + sPrefix;
            return sReturn;
        }

        private string GetTwentyWord(long nNumber)
        {
            string sReturn = "";
            switch (nNumber)
            {
                case 2:
                    sReturn = "Twenty";
                    break;
                case 3:
                    sReturn = "Thirty";
                    break;
                case 4:
                    sReturn = "Forty";
                    break;
                case 5:
                    sReturn = "Fifty";
                    break;
                case 6:
                    sReturn = "Sixty";
                    break;
                case 7:
                    sReturn = "Seventy";
                    break;
                case 8:
                    sReturn = "Eighty";
                    break;
                case 9:
                    sReturn = "Ninety";
                    break;
            }
            return sReturn;
        }

        private string GetLessThanTwentyWord(long nNumber)
        {
            string sReturn = "";
            switch (nNumber)
            {
                case 1:
                    sReturn = "One";
                    break;
                case 2:
                    sReturn = "Two";
                    break;
                case 3:
                    sReturn = "Three";
                    break;
                case 4:
                    sReturn = "Four";
                    break;
                case 5:
                    sReturn = "Five";
                    break;
                case 6:
                    sReturn = "Six";
                    break;
                case 7:
                    sReturn = "Seven";
                    break;
                case 8:
                    sReturn = "Eight";
                    break;
                case 9:
                    sReturn = "Nine";
                    break;
                case 10:
                    sReturn = "Ten";
                    break;
                case 11:
                    sReturn = "Eleven";
                    break;
                case 12:
                    sReturn = "Twelve";
                    break;
                case 13:
                    sReturn = "Thirteen";
                    break;
                case 14:
                    sReturn = "Forteen";
                    break;
                case 15:
                    sReturn = "Fifteen";
                    break;
                case 16:
                    sReturn = "Sixteen";
                    break;
                case 17:
                    sReturn = "Seventeen";
                    break;
                case 18:
                    sReturn = "Eighteen";
                    break;
                case 19:
                    sReturn = "Nineteen";
                    break;
            }
            return sReturn;
        }*/
        #endregion

        #region Generate Report
        public void GenerateReport(XmlDocument xDoc, string reportType, params string[] arrSelectedColumns)
        {
            #region NLog
            logger.Info("Generating the report based on the xDoc , report type and selected columns for report type as "+reportType);
            #endregion

            //xDoc.Load("C:/Documents and Settings/rknandamuri/Desktop/665.xml");
            //xDoc.Load("D:\\ZSwami\\My Needs\\2010\\July\\06\\RP201.xml");
           // xDoc.Load("C:\\sparadesiTFS\\MyNeeds\\Sample.xml");
            //xDoc.Load("D:\\ZSwami\\My Needs\\2010\\July\\09\\Sample.xml");

            try
            {
                DataTable NotesDT = new DataTable();
                DataTable dt = new DataTable();
                string strOutXml = xDoc.OuterXml.ToString();
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                string reportStyle = string.Empty;
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    reportStyle = nodeTreenode.Attributes["ReportStyle"].Value;
                }
                rptPDF objPDF = new rptPDF();
                rptHTML objHTML = new rptHTML();
                rptEXCEL objExcel = new rptEXCEL();
                rptDOC objWord = new rptDOC();
                //
                if (arrSelectedColumns != null)
                {
                    if (arrSelectedColumns.Length > 0)
                    {
                        Hashtable htArrSelectedColumns = new Hashtable();
                        foreach (string s in arrSelectedColumns)
                        {
                            htArrSelectedColumns.Add(s.ToString(), xDoc.SelectSingleNode("//" + treeNodeName + "//GridHeading/Columns/Col[@Label='" + s.ToString() + "' ]").Attributes["FullViewLength"].Value);
                        }
                        XmlNode xArrSelectedNodes = xDoc.SelectSingleNode("//" + treeNodeName + "//GridHeading/Columns");
                        foreach (XmlNode xNode in xArrSelectedNodes)
                        {
                            if (htArrSelectedColumns.ContainsKey(xNode.Attributes["Label"].Value.ToString()))
                            {
                                xNode.Attributes["FullViewLength"].Value = htArrSelectedColumns[xNode.Attributes["Label"].Value.ToString()].ToString();
                            }
                            else
                            {
                                xNode.Attributes["FullViewLength"].Value = "0";
                            }
                        }
                    }
                }
                //Declarations For ITEXT SHARP
                if (m_FontName != null)
                {
                    objItextReport.DocPassWord = m_Password;
                    if (m_FontName == "TNR")
                    {
                        objItextReport.FontName = "Times";
                    }
                    else if (m_FontName == "Ar")
                    {
                        objItextReport.FontName = "Arial";
                    }
                    else if (m_FontName == "Ve")
                    {
                        objItextReport.FontName = "Verdana";
                    }
                    else if (m_FontName == "Co")
                    {
                        objItextReport.FontName = "Courier";
                    }
                    objItextReport.ShowWatermark = m_WaterMark;
                    objItextReport.ShowPageNumber = m_PageNumber;
                    objItextReport.ShowAnnotations = m_Annotations;
                    if (m_PageLayOut == "La")
                    {
                        objItextReport.ShowLandscape = true;
                    }
                    else
                    {
                        objItextReport.ShowLandscape = false;
                    }
                    if (m_PageSize == "A4")
                    {
                        objItextReport.PaperSize = PaperSize.A4;
                    }
                    else if (m_PageSize == "Le")
                    {
                        objItextReport.PaperSize = PaperSize.LetterUS;
                    }
                }
                FileInfo fileName1 = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\Report " + reportStyle + " " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".PDF");
                //
                switch (reportStyle.ToUpper().Trim())
                {
                    //"PARENTCHILDPIVOT": New page
                    //case "2":
                    //    {
                    //        //dt = XMLToDataTable(strOutXml, treeNodeName, "");
                    //        //if (dt.Rows.Count > 0)
                    //        //{
                    //        //    if (dt.Columns.Contains("Notes"))
                    //        //    {
                    //        //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    //        //        dt.Columns.Remove("Notes");
                    //        //    }
                    //        //    GVPivotParentNewPgExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "PIVOT");
                    //        //}
                    //        GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //        break;
                    //    }
                    //"PARENTCHILDPIVOT" Continuos page:
                    //case "3":
                    //    {
                    //        //dt = XMLToDataTable(strOutXml, treeNodeName, "");
                    //        //if (dt.Rows.Count > 0)
                    //        //{
                    //        //    if (dt.Columns.Contains("Notes"))
                    //        //    {
                    //        //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    //        //        dt.Columns.Remove("Notes");
                    //        //    }
                    //        //    GVPivotParentCntPgExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "PIVOT");
                    //        //}
                    //        GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //        break;
                    //    }
                    //case "4"://"PARENTCHILDNORMAL" New page:
                    //    {
                    //        //dt = XMLToDataTable(strOutXml, treeNodeName, "");
                    //        //if (dt.Rows.Count > 0)
                    //        //{
                    //        //    if (dt.Columns.Contains("Notes"))
                    //        //    {
                    //        //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    //        //        dt.Columns.Remove("Notes");
                    //        //    }
                    //        //    GVNormalParentNewPgExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                    //        //}
                    //        GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //        break;
                    //    }

                    //case "10":
                    //    {
                    //        switch (reportType)
                    //        {
                    //            case "PDF":
                    //                {
                    //                    objPDF.OldReportStyle10(dt, "PDFRpt", NotesDT, "NO", strOutXml, "NORMAL");
                    //                    break;
                    //                }
                    //            case "EXCEL":
                    //                {
                    //                    GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //                    break;
                    //                }
                    //            case "HTML":
                    //            case "WORD":
                    //                {
                    //                    GenerateReportPDF(xDoc, reportStyle, reportType);
                    //                    break;
                    //                }
                    //        }
                    //        //GenerateReportPDF(strOutXml);
                    //        break;
                    //    }
                    case "11":
                        {
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.OldReportStyle11(dt, "PDFRpt", NotesDT, "NO", strOutXml, "NORMAL");
                                        break;
                                    }
                                case "EXCEL":
                                case "HTML":
                                case "WORD":
                                    {
                                        GenerateReportPDF(xDoc, reportStyle, reportType);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "100"://"CHECKS - PARENTCHILDNORMAL" New page:
                        {
                            //dt = XMLToDataTable(strOutXml, treeNodeName, "");
                            //if (dt.Rows.Count > 0)
                            //{
                            //    if (dt.Columns.Contains("Notes"))
                            //    {
                            //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            //        dt.Columns.Remove("Notes");
                            //    }
                            //    ChecksExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                            //}
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "101":
                        {
                            //dt = XMLToDataTable(strOutXml, treeNodeName, "");
                            //if (dt.Rows.Count > 0)
                            //{
                            //    if (dt.Columns.Contains("Notes"))
                            //    {
                            //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            //        dt.Columns.Remove("Notes");
                            //    }
                            //    ReportStyle101(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                            //}
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "102":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "103":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "104":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "105":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "106":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "107":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                    case "200"://"ARInvoice - PARENTCHILDNORMAL" New page:
                        {
                            dt = XMLToDataTable(strOutXml, treeNodeName, "");
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("Notes"))
                                {
                                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                                    dt.Columns.Remove("Notes");
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                             //objItextReport.GenerateReport(xDoc, fileName1);
                                            objPDF.OldARInvoiceExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                                            break;
                                        }
                                    case "HTML":
                                    case "WORD":
                                        {
                                            GenerateReportPDF(xDoc, reportStyle, reportType);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "201": //"ARInvoice - PARENTCHILDNORMAL" New page: Child details displayed in table 
                        {
                            dt = XMLToDataTable(strOutXml, treeNodeName, "");
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("Notes"))
                                {
                                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                                    dt.Columns.Remove("Notes");
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objItextReport.GenerateReport(xDoc, fileName1);
                                            objPDF.ARInvoice201ExpToPDF(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                                            break;
                                        }
                                    case "HTML":
                                    case "WORD":
                                        {
                                            GenerateReportPDF(xDoc, reportStyle, reportType);
                                            break;
                                        }
                                }
                            }
                            break;
                        }





                    case "400":
                        {

                            DataSet dsAll = new DataSet("ALL");
                            XmlDocument xdoc = new XmlDocument();
                            xdoc.LoadXml(strOutXml);
                            XmlNode m_childNodes = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode node in m_childNodes.ChildNodes)
                            {
                                string Node = node.FirstChild.InnerText;
                                DataTable dtAll = new DataTable();
                                dtAll = GetJobCostingDataToPrint(strOutXml, Node, "");
                                dtAll = ChangeDateAndAmountFormats(dtAll, strOutXml, Node);
                                dtAll.TableName = Node;
                                if (dtAll.Rows.Count > 0)
                                {
                                    if (dtAll.Columns.Contains("Notes"))
                                    {
                                        NotesDT = reportsBO.GenerateNotesDatatable(dtAll);
                                        dtAll.Columns.Remove("Notes");
                                    }
                                }
                                dsAll.Tables.Add(dtAll.Copy());
                            }

                            if (reportType == "EXCEL")
                            {
                                GenerateExcelReport400(xDoc, dsAll, reportStyle, reportType, arrSelectedColumns);
                                break;
                            }
                            else
                            {
                                objItextReport.GenerateReport(xDoc, fileName1);
                                //objItextReport.GenerateReport400(xDoc, fileName1, dsAll);
                                //objPDF.OldGVJobExportToPDF2(dsAll, "PDFRpt", NotesDT, "NO", strOutXml, "NORMAL");
                                break;
                            }
                        }
                    //case "405"://"PARENTCHILDNORMAL" Continuous page:
                    //    {
                    //        dt = XMLToDataTable(strOutXml, treeNodeName, "");
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            if (dt.Columns.Contains("Notes"))
                    //            {
                    //                NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    //                dt.Columns.Remove("Notes");
                    //            }
                    //        }
                    //        switch (reportType)
                    //        {
                    //            case "PDF":
                    //                {
                    //                    objItextReport.GenerateReport(xDoc, fileName1);
                    //                    //objPDF.ReportStyle405(dt, "PDFRpt", NotesDT, "YES", strOutXml, "NORMAL");
                    //                    break;
                    //                }
                    //            case "EXCEL":
                    //                {
                    //                    GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //                    break;
                    //                }
                    //            case "HTML":
                    //            case "WORD":
                    //                {
                    //                    GenerateReportPDF(xDoc, reportStyle, reportType);
                    //                    break;
                    //                }
                    //        }
                    //        //GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //        break;
                    //    }
                    //case "622":
                    //    {
                    //        GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                    //        break;
                    //    }
                    default:
                    case "1"://PARENT
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "7":
                    case "10":
                    case "405":
                    case "501":
                    case "502":
                    case "503":
                    case "601":
                    case "602":
                    case "604":
                    case "621":
                    case "641":
                    case "642":
                    case "643":
                    case "651":
                    case "652":
                    case "653":
                    case "660":
                    case "661":
                    case "662":
                    case "663":
                    case "664":
                    case "665":
                    case "670":
                        {
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objItextReport.GenerateReport(xDoc, fileName1);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                                        break;
                                    }
                                case "HTML":
                                case "WORD":
                                    {
                                        GenerateReportPDF(xDoc, reportStyle, reportType);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "603":
                    case "622":
                    case "510":
                    case "512":
                        {
                            GenerateReportPDF(xDoc, reportStyle, reportType, arrSelectedColumns);
                            break;
                        }
                }
                string fileName = string.Empty;
                XmlNode xEmailsNode = xDoc.SelectSingleNode("//EmailIDS");
                fileName = xDoc.SelectSingleNode("Root/bpeout/FormInfo/Title").InnerText;
                if (xEmailsNode != null)
                {
                    m_EmailStatus = objPDF.SendEmails(xDoc.OuterXml.ToString(), fileName);
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }

        public void GenerateReport(XmlDocument xDoc, string reportType, string strOrderByCol, params string[] arrSelectedColumns)
        {
            #region NLog
            logger.Info("Generating the report base on the xDoc , report type , order-by-column and selected columns.");
            #endregion

            try
            {
                DataTable NotesDT = new DataTable();
                DataTable dt = new DataTable();
                string strOutXml = xDoc.OuterXml.ToString();
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                string reportStyle = string.Empty;
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    reportStyle = nodeTreenode.Attributes["ReportStyle"].Value;
                }
                rptPDF objPDF = new rptPDF();
                rptHTML objHTML = new rptHTML();
                rptEXCEL objExcel = new rptEXCEL();
                rptDOC objWord = new rptDOC();
                //
                if (arrSelectedColumns != null)
                {
                    if (arrSelectedColumns.Length > 0)
                    {
                        Hashtable htArrSelectedColumns = new Hashtable();
                        foreach (string s in arrSelectedColumns)
                        {
                            htArrSelectedColumns.Add(s.ToString(), xDoc.SelectSingleNode("//" + treeNodeName + "//GridHeading/Columns/Col[@Label='" + s.ToString() + "' ]").Attributes["FullViewLength"].Value);
                        }
                        XmlNode xArrSelectedNodes = xDoc.SelectSingleNode("//" + treeNodeName + "//GridHeading/Columns");
                        foreach (XmlNode xNode in xArrSelectedNodes)
                        {
                            if (htArrSelectedColumns.ContainsKey(xNode.Attributes["Label"].Value.ToString()))
                            {
                                xNode.Attributes["FullViewLength"].Value = htArrSelectedColumns[xNode.Attributes["Label"].Value.ToString()].ToString();
                            }
                            else
                            {
                                xNode.Attributes["FullViewLength"].Value = "0";
                            }
                        }
                    }
                }
                //Declarations For ITEXT SHARP
                if (m_FontName != null)
                {
                    objItextReport.DocPassWord = m_Password;
                    if (m_FontName == "TNR")
                    {
                        objItextReport.FontName = "Times";
                    }
                    else if (m_FontName == "Ar")
                    {
                        objItextReport.FontName = "Arial";
                    }
                    else if (m_FontName == "Ve")
                    {
                        objItextReport.FontName = "Verdana";
                    }
                    else if (m_FontName == "Co")
                    {
                        objItextReport.FontName = "Courier";
                    }
                    objItextReport.ShowWatermark = m_WaterMark;
                    objItextReport.ShowPageNumber = m_PageNumber;
                    objItextReport.ShowAnnotations = m_Annotations;
                    if (m_PageLayOut == "La")
                    {
                        objItextReport.ShowLandscape = true;
                    }
                    else
                    {
                        objItextReport.ShowLandscape = false;
                    }
                    if (m_PageSize == "A4")
                    {
                        objItextReport.PaperSize = PaperSize.A4;
                    }
                    else if (m_PageSize == "Le")
                    {
                        objItextReport.PaperSize = PaperSize.LetterUS;
                    }
                }

                FileInfo fileName1 = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\Report " + reportStyle + " " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".PDF");
                //
                switch (reportStyle.ToUpper().Trim())
                {
                    case "1":
                        {
                            GenerateOrderByColReport(xDoc, reportStyle, reportType, fileName1, strOrderByCol, arrSelectedColumns);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }

        private void GenerateOrderByColReport(XmlDocument xDoc, string reportStyle, string reportType, FileInfo file, string strOrderByCol, string[] arrSelectedColumns)
        {
            #region NLog
            logger.Info("Generating the Order-By-Column report.");
            #endregion

            try
            {
                #region REPORTING DECLARATIONS
                //
                DataTable dt = new DataTable();
                DataTable dtChild = null;
                DataTable dtCGbranch = null;
                int[] colWidths = null;
                int[] arrWidths = null;
                bool PLayout = new bool();
                bool bRptTrimStatus = false;
                //
                Hashtable htPFormats = new Hashtable();
                Hashtable htColFormats = new Hashtable();
                Hashtable htColNameValues = new Hashtable();
                DataTable dtParentAll = new DataTable();
                //Set Branch DataTable
                string branchName = string.Empty;
                string CGbranchName = string.Empty;
                XmlNode nodeBranches = null;
                // Set branch Table
                int[] colBranchWidths = null;
                int[] arrBranchWidths = null;
                bool bPLayout = false;
                //
                Hashtable htBFormats = new Hashtable();
                Hashtable htBColFormats = new Hashtable();
                Hashtable htBColNameValues = new Hashtable();
                //
                //Set Branch CGrid DataTable
                int[] colCGbranchWidths = null;
                int[] arrCGbranchWidths = null;
                bool CGbPLayout = false;
                //
                Hashtable htCGbFormats = new Hashtable();
                Hashtable htCGbColFormats = new Hashtable();
                Hashtable htCGbColNameValues = new Hashtable();
                //
                rptPDF objPDF = new rptPDF();
                rptHTML objHTML = new rptHTML();
                rptEXCEL objExcel = new rptEXCEL();
                rptDOC objDoc = new rptDOC();
                rptDOC objWord = new rptDOC();
                rptTXT objTxt = new rptTXT();
                //
                #endregion
                #region REPORT PROCESSING
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);
                #endregion
                if (reportType == "PDF")
                {
                    bRptTrimStatus = true;
                }
                switch (reportStyle)
                {
                    case "1":
                    default:
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objItextReport.GenerateReport(xDoc, file, dtParentAll, strOrderByCol, htColNameValues, htColFormats);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.CustomRptStyle1(dtParentAll, dtHeader, strOrderByCol, htColFormats, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        //objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        //objWord.ReportStyle1(dtParentAll, treeNodeName);
                                        break;
                                    }
                                case "TXT":
                                    {
                                        //objTxt.ReportStyle1(dtParentAll, dtHeader, htColFormats, htColNameValues);
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }
        #endregion

        #region Generate Report New Method
        public void GenerateReportPDF(XmlDocument xDoc, string reportStyle, string reportType, params string[] arrSelectedColumns)
        {
            #region NLog
            logger.Info("Generating PDF report.");
            #endregion

            try
            {
                #region REPORTING DECLARATIONS
                //
                DataTable dt = new DataTable();
                DataTable dtChild = null;
                DataTable dtCGbranch = null;
                int[] colWidths = null;
                int[] arrWidths = null;
                bool PLayout = new bool();
                bool bRptTrimStatus = false;
                //
                Hashtable htPFormats = new Hashtable();
                Hashtable htColFormats = new Hashtable();
                Hashtable htColNameValues = new Hashtable();
                DataTable dtParentAll = new DataTable();
                //Set Branch DataTable
                string branchName = string.Empty;
                string CGbranchName = string.Empty;
                XmlNode nodeBranches = null;
                // Set branch Table
                int[] colBranchWidths = null;
                int[] arrBranchWidths = null;
                bool bPLayout = false;
                //
                Hashtable htBFormats = new Hashtable();
                Hashtable htBColFormats = new Hashtable();
                Hashtable htBColNameValues = new Hashtable();
                //
                //Set Branch CGrid DataTable
                int[] colCGbranchWidths = null;
                int[] arrCGbranchWidths = null;
                bool CGbPLayout = false;
                //
                Hashtable htCGbFormats = new Hashtable();
                Hashtable htCGbColFormats = new Hashtable();
                Hashtable htCGbColNameValues = new Hashtable();
                //
                rptPDF objPDF = new rptPDF();
                rptHTML objHTML = new rptHTML();
                rptEXCEL objExcel = new rptEXCEL();
                rptDOC objDoc = new rptDOC();
                rptDOC objWord = new rptDOC();
                rptTXT objTxt = new rptTXT();
                //
                #endregion
                #region REPORT PROCESSING
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);
                #endregion
                if (reportType == "PDF")
                {
                    bRptTrimStatus = true;
                }
                switch (reportStyle)
                {
                    #region CASE 1
                    //PARENT
                    case "1":
                    default:
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle1(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle1(dtParentAll, dtHeader, htColFormats, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle1(dtParentAll, treeNodeName);
                                        break;
                                    }
                                case "TXT":
                                    {
                                        objTxt.ReportStyle1(dtParentAll, dtHeader, htColFormats, htColNameValues);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 2
                    //"PARENTCHILDPIVOT": New page
                    case "2":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle2(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle2(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle2(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 3
                    //"PARENTCHILDPIVOT" Continuos page:
                    case "3":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle3(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle3(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle3(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 4
                    //"PARENTCHILDNORMAL" New page:
                    case "4":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle4(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle4(dtParentAll, dtChild, dtHeader, htColFormats, htColNameValues, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 5
                    //"PARENTCHILDNORMAL" Continuous Page
                    case "5":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle5(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        if (dtCGbranch != null && dtChild == null)
                                        {
                                            objExcel.ReportStyle5(dtParentAll, dtCGbranch, dtHeader, htColFormats, htColNameValues, htCGbColFormats, htCGbColNameValues);
                                        }
                                        else
                                        {
                                            if (dtCGbranch == null && dtChild != null)
                                            {
                                                objExcel.ReportStyle5(dtParentAll, dtChild, dtHeader, htColFormats, htColNameValues, htBColFormats, htBColNameValues);
                                            }
                                            else
                                            {
                                                objExcel.ReportStyle5(dtParentAll, dtChild, dtHeader, htColFormats, htColNameValues, htBColFormats, htBColNameValues);
                                            }
                                        }

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle5(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 7
                    case "7":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        //objPDF.ReportStyle3(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle7(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        //objWord.ReportStyle3(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 10
                    case "10":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle10(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle10(dtParentAll, dtHeader);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle10(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 11
                    case "11":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        //TO DO NEW
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 100
                    //CHECKS - PARENTCHILDNORMAL-New page
                    case "100":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle100(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                    #region CASE 101
                    case "101":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle101(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                    #region CASE 102
                    case "102":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle102(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                    #region CASE 103
                    case "103":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle103(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 104
                    case "104":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle104(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                    #region CASE 105
                    case "105":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle105(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 106
                    case "106":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle106(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 107
                    case "107":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle107(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtCGbranch, htCGbFormats, CGbPLayout, arrCGbranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 200
                    case "200":// ARInvoice - PARENTCHILDNORMAL-New page
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        //TO DO NEW
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle200(dtParentAll, dtCGbranch, htColNameValues, htCGbColNameValues, dtHeader);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {

                                        objDoc.ReportStyle200(dtParentAll, dtCGbranch, htColNameValues, htCGbColNameValues, dtHeader);
                                        //objWord.ReportStyle1(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 400
                    case "400":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        //TO DO NEW
                                        break;
                                    }
                                case "EXCEL":
                                    {

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle400(dt, treeNodeName);
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                    #region CASE 405
                    //"PARENTCHILDNORMAL" Continuous Page
                    case "405":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                if (nodeBranchRowList != null)
                                {
                                    dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                if (nodeCGbranchRowList != null)
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        if (dtCGbranch != null && dtChild == null)
                                        {
                                            objExcel.ReportStyle405(dtParentAll, dtCGbranch, dtHeader, htColFormats, htColNameValues, htCGbColFormats, htCGbColNameValues);
                                        }
                                        else
                                        {
                                            if (dtCGbranch == null && dtChild != null)
                                            {
                                                objExcel.ReportStyle405(dtParentAll, dtChild, dtHeader, htColFormats, htColNameValues, htBColFormats, htBColNameValues);
                                            }
                                            else
                                            {
                                                objExcel.ReportStyle405(dtParentAll, dtChild, dtHeader, htColFormats, htColNameValues, htBColFormats, htBColNameValues);
                                            }
                                        }

                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle405(dtParentAll, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 501
                    case "501":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle501(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle501(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle501(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 502
                    case "502":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle502(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle502(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle502(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 503
                    case "503":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }
                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle502(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle503(dtParentAll, dtChild, dtHeader, htBColFormats, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle503(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 510
                    case "510":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns1(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable1(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable1(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle510(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        break;
                                    }
                                case "HTML":
                                    {
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle503(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 512
                    case "512":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns1(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable1(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, bRptTrimStatus, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable1(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle512(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        break;
                                    }
                                case "HTML":
                                    {
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 601
                    case "601":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle601(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, htColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle601(dtParentAll, dtHeader, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle601(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 602
                    case "602":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                            dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle602(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, htColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle602(dtParentAll, dtHeader, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle602(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 603
                    case "603":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode xnl in nodeParents.ChildNodes)
                            {
                                if (xnl != null)
                                {
                                    treeNodeName = xnl.SelectSingleNode("Node").InnerText.ToString();
                                    switch (treeNodeName)
                                    {
                                        case "ReportDetail":
                                            {
                                                dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                break;
                                            }
                                        case "SubDetail":
                                            {
                                                XmlNode nodeColumns1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                XmlNode nodeRowList1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeColumns1, "Parent", treeNodeName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                                dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Parent", nodeRowList1, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                                break;
                                            }
                                    }
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle603(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, htColNameValues, htBColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle603(dtParentAll, dtChild, dtHeader, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle603(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 604
                    case "604":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode xnl in nodeParents.ChildNodes)
                            {
                                if (xnl != null)
                                {
                                    treeNodeName = xnl.SelectSingleNode("Node").InnerText.ToString();
                                    switch (treeNodeName)
                                    {
                                        case "ReportDetail":
                                            {
                                                dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                break;
                                            }
                                        case "SubDetail":
                                            {
                                                XmlNode nodeColumns1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                XmlNode nodeRowList1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeColumns1, "Parent", treeNodeName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                                dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Parent", nodeRowList1, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);

                                                break;
                                            }
                                    }
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle604(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, htColNameValues, htBColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle604(dtParentAll, dtChild, dtHeader, htColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle604(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 621
                    case "621":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode xnl in nodeParents.ChildNodes)
                            {
                                if (xnl != null)
                                {
                                    treeNodeName = xnl.SelectSingleNode("Node").InnerText.ToString();
                                    switch (treeNodeName)
                                    {
                                        case "ReportDetail":
                                            {
                                                dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                break;
                                            }
                                        case "SubDetail":
                                            {
                                                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList") != null)
                                                {
                                                    XmlNode nodeColumns1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                    XmlNode nodeRowList1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                                    dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeColumns1, "Parent", treeNodeName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Parent", nodeRowList1, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        objPDF.ReportStyle621(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, htColNameValues, htBColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle621(dtParentAll, dtChild, dtHeader, htColNameValues, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle621(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 622
                    case "622":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode xnl in nodeParents.ChildNodes)
                            {
                                if (xnl != null)
                                {
                                    treeNodeName = xnl.SelectSingleNode("Node").InnerText.ToString();
                                    switch (treeNodeName)
                                    {
                                        case "ReportDetail":
                                            {
                                                dtParentAll = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                dtParentAll = clsReportsUICore.ConvertToDataTable1(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                break;
                                            }
                                        case "SubDetail":
                                            {
                                                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList") != null)
                                                {
                                                    XmlNode nodeColumns1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                    XmlNode nodeRowList1 = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                    dtChild = clsReportsUICore.ConvertToArrayColumns1(nodeColumns1, "Parent", treeNodeName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);
                                                    dtChild = clsReportsUICore.ConvertToDataTable1(dtChild, "Parent", nodeRowList1, htBColFormats, htBColNameValues, colBranchWidths, bRptTrimStatus, out arrBranchWidths, out htBFormats, out bPLayout);
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        FileInfo fileName1 = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\Report " + reportStyle + " " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".PDF");
                                        LAjitDev.Report objReport = new Report();
                                        objReport.GenerateReport(xDoc, fileName1, dtParentAll, dtChild, htColNameValues, htBColNameValues);
                                        //objPDF.ReportStyle622(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, htColNameValues, htBColNameValues, treeNodeName);
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle622(dtParentAll, dtChild, dtHeader, htColNameValues, htBColNameValues);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        objWord.ReportStyle622(dt, treeNodeName);
                                        break;
                                    }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 641
                    case "641":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle641(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle641(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 642
                    case "642":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            //bool[] ArrPLayout = new bool[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                //foreach (XmlNode xnl in nodeParents.ChildNodes)
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        //arrWidths.CopyTo(ArrallWidths[cnt],0);
                                        //Array.Copy(arrWidths, ArrallWidths[cnt], arrWidths.Length);
                                        ArrallWidths[cnt] = arrWidths;

                                        //ArrPLayout[cnt] = PLayout;

                                        ArrhtColFormats[cnt] = htPFormats;
                                        //
                                        //if (htColFormats.Count > 0)
                                        //{
                                        //    ArrhtColFormats.Add(htColFormats);
                                        //}
                                        //if (htColNameValues.Count > 0)
                                        //{
                                        //    ArrhtColNameValues.Add(htColNameValues);
                                        //}
                                        //if (htPFormats.Count > 0)
                                        //{
                                        //    ArrhtPFormats.Add(htPFormats);
                                        //}
                                        //if (colWidths.Length > 0)
                                        //{
                                        //    ArrColWidths.Add(colWidths);
                                        //}
                                        //if (arrWidths.Length > 0)
                                        //{
                                        //    ArrArrWidths.Add(arrWidths);
                                        //}
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle641(dtParentAll, dtHeader, htPFormats, PLayout, arrWidths, dtChild, htBFormats, bPLayout, arrBranchWidths, treeNodeName);
                                            objPDF.ReportStyle642(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, treeNodeName);

                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                //foreach (XmlNode xnl in nodeParents.ChildNodes)
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                                        //
                                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                                        //
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                        //
                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }
                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle642(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle642(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 643
                    case "643":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                //foreach (XmlNode xnl in nodeParents.ChildNodes)
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        ArrallWidths[cnt] = arrWidths;

                                        ArrhtColFormats[cnt] = htPFormats;
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle643(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                //foreach (XmlNode xnl in nodeParents.ChildNodes)
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                                        //
                                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                                        //
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                                        //
                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }

                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle643(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            //objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle643(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 651
                    case "651":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        ArrallWidths[cnt] = arrWidths;
                                        ArrhtColFormats[cnt] = htPFormats;
                                        ArrhtCNameValues[cnt] = htColNameValues;
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle651(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, ArrhtCNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                                        ArrallWidths[cnt] = arrWidths;

                                                        ArrhtColFormats[cnt] = htPFormats;

                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }
                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle651(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle651(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 652
                    case "652":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        ArrallWidths[cnt] = arrWidths;
                                        ArrhtColFormats[cnt] = htPFormats;
                                        ArrhtColNameValues[cnt] = htColNameValues;
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle652(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                                        ArrallWidths[cnt] = arrWidths;

                                                        ArrhtColFormats[cnt] = htPFormats;

                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }
                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle652(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle652(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 653
                    case "653":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        ArrallWidths[cnt] = arrWidths;
                                        ArrhtColFormats[cnt] = htPFormats;
                                        ArrhtColNameValues[cnt] = htColNameValues;
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle653(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                                        ArrallWidths[cnt] = arrWidths;

                                                        ArrhtColFormats[cnt] = htPFormats;

                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }
                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle653(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle653(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 654
                    case "654":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            Hashtable[] ArrhtColFormats = new Hashtable[nodeParents.ChildNodes.Count];
                            Hashtable[] ArrhtColNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            int[][] ArrallWidths = new int[nodeParents.ChildNodes.Count][];
                            Hashtable[] ArrhtPFormats = new Hashtable[nodeParents.ChildNodes.Count];

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];

                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                        ArrallWidths[cnt] = arrWidths;
                                        ArrhtColFormats[cnt] = htPFormats;
                                        ArrhtColNameValues[cnt] = htColNameValues;
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle654(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrallWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            if (nodeParents.ChildNodes.Count > 0)
                                            {
                                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                                {
                                                    if (nodeParents.ChildNodes[cnt] != null)
                                                    {
                                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();

                                                        XmlNode xnodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                                        XmlNode xnodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(xnodeColumns, "Parent", treeNodeName, string.Empty, out htPFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", xnodeRowList, htPFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out ArrhtPFormats[cnt], out PLayout);

                                                        ArrallWidths[cnt] = arrWidths;

                                                        ArrhtColFormats[cnt] = htPFormats;

                                                        if (htColNameValues.Count > 0)
                                                        {
                                                            ArrhtCNameValues[cnt] = htColNameValues;
                                                        }
                                                    }
                                                }
                                            }
                                            objExcel.ReportStyle654(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle654(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 660
                    case "660":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle660(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle660(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 661
                    case "661":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle661(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 662
                    case "662":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle662(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 663
                    case "663":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle663(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 664
                    case "664":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle664(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 665
                    case "665":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                        //
                                        if (htColFormats.Count > 0)
                                        {
                                            ArrhtColFormats.Add(htColFormats);
                                        }
                                        if (htColNameValues.Count > 0)
                                        {
                                            ArrhtColNameValues.Add(htColNameValues);
                                            ArrhtCNameValues[cnt] = htColNameValues;
                                        }
                                        if (htPFormats.Count > 0)
                                        {
                                            ArrhtPFormats.Add(htPFormats);
                                        }
                                        if (colWidths.Length > 0)
                                        {
                                            ArrColWidths.Add(colWidths);
                                        }
                                        if (arrWidths.Length > 0)
                                        {
                                            ArrArrWidths.Add(arrWidths);
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    case "PDF":
                                        {
                                            //objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                            break;
                                        }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle665(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region CASE 670
                    case "670":
                        {
                            XmlNode nodeParents = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            //
                            DataTable[] dtAllArray = new DataTable[nodeParents.ChildNodes.Count];
                            //
                            ArrayList ArrhtColFormats = new ArrayList();
                            ArrayList ArrhtColNameValues = new ArrayList();
                            ArrayList ArrhtPFormats = new ArrayList();
                            ArrayList ArrColWidths = new ArrayList();
                            ArrayList ArrArrWidths = new ArrayList();

                            Hashtable[] ArrhtCNameValues = new Hashtable[nodeParents.ChildNodes.Count];
                            //
                            if (nodeParents.ChildNodes.Count > 0)
                            {
                                for (int cnt = 0; cnt < nodeParents.ChildNodes.Count; cnt++)
                                {
                                    if (nodeParents.ChildNodes[cnt] != null)
                                    {
                                        treeNodeName = nodeParents.ChildNodes[cnt].SelectSingleNode("Node").InnerText.ToString();
                                        //
                                        nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                                        nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                                        //
                                        if (nodeColumns != null)
                                        {
                                            dtAllArray[cnt] = clsReportsUICore.ConvertToArrayColumns1(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);
                                        }
                                        if (nodeRowList != null)
                                        {
                                            dtAllArray[cnt] = clsReportsUICore.ConvertToDataTable1(dtAllArray[cnt], "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, bRptTrimStatus, out arrWidths, out htPFormats, out PLayout);
                                            //
                                            if (htColFormats.Count > 0)
                                            {
                                                ArrhtColFormats.Add(htColFormats);
                                            }
                                            if (htColNameValues.Count > 0)
                                            {
                                                ArrhtColNameValues.Add(htColNameValues);
                                                //ArrhtCNameValues[cnt] = htColNameValues;
                                            }
                                            if (htPFormats.Count > 0)
                                            {
                                                ArrhtPFormats.Add(htPFormats);
                                            }
                                            if (colWidths.Length > 0)
                                            {
                                                ArrColWidths.Add(colWidths);
                                            }
                                            if (arrWidths.Length > 0)
                                            {
                                                ArrArrWidths.Add(arrWidths);
                                            }
                                        }
                                    }
                                }
                                switch (reportType)
                                {
                                    //case "PDF":
                                    //    {
                                    //        objPDF.ReportStyle661(dtAllArray, dtHeader, ArrhtPFormats, PLayout, ArrArrWidths, ArrhtColNameValues, treeNodeName);
                                    //        break;
                                    //    }
                                    case "EXCEL":
                                        {
                                            objExcel.ReportStyle661(dtAllArray, dtHeader, ArrhtCNameValues);
                                            break;
                                        }
                                    case "HTML":
                                        {
                                            objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                            break;
                                        }
                                    case "WORD":
                                        {
                                            objWord.ReportStyle641(dt, treeNodeName);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }


        public void GenerateExcelReport400(XmlDocument xDoc, DataSet dsAll, string reportStyle, string reportType, params string[] arrSelectedColumns)
        {
            #region Nlog
            logger.Info("Generating the Excel Report with style 400");
            #endregion

            try
            {
                #region REPORTING DECLARATIONS
                //
                DataTable dt = new DataTable();
                DataTable dtChild = null;
                DataTable dtCGbranch = null;
                int[] colWidths = null;
                int[] arrWidths = null;
                bool PLayout = new bool();
                //
                Hashtable htPFormats = new Hashtable();
                Hashtable htColFormats = new Hashtable();
                Hashtable htColNameValues = new Hashtable();
                DataTable dtParentAll = new DataTable();
                //Set Branch DataTable
                string branchName = string.Empty;
                string CGbranchName = string.Empty;
                XmlNode nodeBranches = null;
                // Set branch Table
                int[] colBranchWidths = null;
                int[] arrBranchWidths = null;
                bool bPLayout = false;
                //
                Hashtable htBFormats = new Hashtable();
                Hashtable htBColFormats = new Hashtable();
                Hashtable htBColNameValues = new Hashtable();
                //
                //Set Branch CGrid DataTable
                int[] colCGbranchWidths = null;
                int[] arrCGbranchWidths = null;
                bool CGbPLayout = false;
                //
                Hashtable htCGbFormats = new Hashtable();
                Hashtable htCGbColFormats = new Hashtable();
                Hashtable htCGbColNameValues = new Hashtable();
                //
                rptPDF objPDF = new rptPDF();
                rptHTML objHTML = new rptHTML();
                rptEXCEL objExcel = new rptEXCEL();
                rptDOC objWord = new rptDOC();
                //
                #endregion

                #region REPORT PROCESSING
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);
                #endregion

                switch (reportStyle)
                {
                    #region CASE 400
                    case "400":
                        {
                            dtParentAll = clsReportsUICore.ConvertToArrayColumns(nodeColumns, "Parent", treeNodeName, string.Empty, out htColFormats, out htColNameValues, out colWidths, arrSelectedColumns);

                            if (reportType == "EXCEL")
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, true, out arrWidths, out htPFormats, out PLayout);
                            }
                            else
                            {
                                dtParentAll = clsReportsUICore.ConvertToDataTable(dtParentAll, "Parent", nodeRowList, htColFormats, htColNameValues, colWidths, false, out arrWidths, out htPFormats, out PLayout);
                            }

                            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch") != null)
                            {
                                nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                                {
                                    if (nodeBranches != null)
                                    {
                                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                        {
                                            XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                                            if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                                            {
                                                CGbranchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                            else
                                            {
                                                branchName = nodeBranch.SelectSingleNode("Node").InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                            if (branchName != string.Empty)
                            {
                                XmlNode nodeBranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/GridHeading/Columns");
                                XmlNode nodeBranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList");
                                dtChild = clsReportsUICore.ConvertToArrayColumns(nodeBranchColumns, "Branch", branchName, treeNodeName, out htBColFormats, out htBColNameValues, out colBranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, true, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                                else
                                {
                                    dtChild = clsReportsUICore.ConvertToDataTable(dtChild, "Branch", nodeBranchRowList, htBColFormats, htBColNameValues, colBranchWidths, false, out arrBranchWidths, out htBFormats, out bPLayout);
                                }
                            }
                            if (CGbranchName != string.Empty)
                            {
                                XmlNode nodeCGbranchColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/GridHeading/Columns");
                                XmlNode nodeCGbranchRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + CGbranchName + "/RowList");
                                dtCGbranch = clsReportsUICore.ConvertToArrayColumns(nodeCGbranchColumns, "GView", CGbranchName, treeNodeName, out htCGbColFormats, out htCGbColNameValues, out colCGbranchWidths, arrSelectedColumns);

                                if (reportType == "EXCEL")
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, true, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                                else
                                {
                                    dtCGbranch = clsReportsUICore.ConvertToDataTable(dtCGbranch, "GView", nodeCGbranchRowList, htCGbColFormats, htCGbColNameValues, colCGbranchWidths, false, out arrCGbranchWidths, out htCGbFormats, out CGbPLayout);
                                }
                            }
                            switch (reportType)
                            {
                                case "PDF":
                                    {
                                        //TO DO NEW
                                        break;
                                    }
                                case "EXCEL":
                                    {
                                        objExcel.ReportStyle400(dsAll, dtHeader);
                                        break;
                                    }
                                case "HTML":
                                    {
                                        objHTML.CreateHTMLTemplate(xDoc, dtParentAll, reportStyle, treeNodeName);
                                        break;
                                    }
                                case "WORD":
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }

        private DataTable GetReportHeader(XmlDocument xDoc, string treeNodeName)
        {
            #region Nlog
            logger.Info("Generating Report header with tree node name as : "+treeNodeName);
            #endregion

            string title = string.Empty;
            XmlNode nodeTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
            if (nodeTitle != null)
            {
                title = nodeTitle.InnerText.Trim().ToString();
            }

            string subTitle = string.Empty;
            XmlNode nodeSubTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
            if (nodeSubTitle != null)
            {
                subTitle = nodeSubTitle.InnerText.Trim().ToString();
            }

            DataTable dtHeader = new DataTable("Header");
            dtHeader = GetHeaderDT(title, subTitle);
            return dtHeader;
        }
        #endregion

        #region GVParentPivotExpToPDF To PDf
        /// <summary>
        /// Exports the data to PDF
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="dt">Filename to be printed</param>
        private void GVParentPivotExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, string strOutXml, string tableLayout)
        {
            #region Nlog
            logger.Info("Exporting the report data to PDF with filename as : "+fileName);
            #endregion

            try
            {
                DataTable pNotesDT = new DataTable();
                DataTable pDT = new DataTable();
                Hashtable m_htGVColsFVL = new Hashtable();
                //Storing the fullview length of cols in hashtable                    
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(strOutXml);
                //Get the treeNodeName
                string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                dt = ChangeDateAndAmountFormats(dt, strOutXml, treeNodeName);

                //Getting the columns to be displayed in grid

                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

                int colPos = 0;

                bool isSumExists = false;

                //Storing the columns names and captions in the HashTable

                foreach (XmlNode node in nodeColumns)
                {

                    //Checking for IsSummed attribute value

                    if (!isSumExists)
                    {
                        if (node.Attributes["IsSummed"] != null)
                        {

                            if (node.Attributes["IsSummed"].Value == "1")
                            {
                                isSumExists = true;
                            }
                        }
                    }
                    if (!m_htGVColsFVL.Contains(node.Attributes["Caption"].Value))
                    {

                        if (node.Attributes["FullViewLength"].Value != "0")
                        {

                            //m_htGVColsFVL.Add(node.Attributes["Caption"].Value, node.Attributes["FullViewLength"].Value + "-" + node.Attributes["SortOrder"].Value);

                            m_htGVColsFVL.Add(node.Attributes["Caption"].Value, node.Attributes["FullViewLength"].Value + "-" + colPos);

                        }

                    }

                    colPos++;

                }



                //Creating Data table containing the column names

                DataTable parentDT = new DataTable();

                for (int col = 0; col < dt.Columns.Count; col++)
                {

                    DataColumn colNames = new DataColumn();

                    colNames.ColumnName = dt.Columns[col].ColumnName;

                    colNames.DataType = dt.Columns[col].DataType;

                    parentDT.Columns.Add(colNames);
                }
                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);
                string imgpath = PDFImagePath();
                PdfImage LogoImage = null;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    LogoImage = myPdfDocument.NewImage(imgpath);
                }
                double posX = 20;//50
                double posY = 70;//90
                double width = myPdfDocument.PageWidth - 50;// 690; 
                double height = myPdfDocument.PageHeight - 50;// 250;
                PdfPage newPdfPage = myPdfDocument.NewPage();
                int pageCnt = 0;
                foreach (DataRow dRow in dt.Rows)
                {
                    newPdfPage.Add(LogoImage, 650, 20, 96);
                    //posY = 70;
                    if (!NotesDT.Columns.Contains("New"))
                    {
                        NotesDT.Columns.Add("New");
                    }
                    NotesDT.Columns["New"].SetOrdinal(0);
                    if (!parentDT.Columns.Contains("New"))
                    {

                        parentDT.Columns.Add("New");

                    }

                    parentDT.Columns["New"].SetOrdinal(0);



                    //Adding each row at a time

                    DataRow dNewRow = parentDT.NewRow();

                    for (int col = 0; col < dt.Columns.Count; col++)
                    {

                        dNewRow[col + 1] = dRow[col];

                    }

                    parentDT.Rows.Add(dNewRow);



                    string parentTrxID = string.Empty;

                    parentTrxID = parentDT.Rows[0]["TrxID"].ToString();



                    //Pivoting the tables

                    pNotesDT = reportsBO.PivotTable(NotesDT);

                    pDT = reportsBO.PivotTable(parentDT);



                    ////Removing TrxID column

                    //if (parentDT.Columns.Contains("TrxID"))

                    //{

                    //    parentDT.Columns..Remove("TrxID");

                    //}



                    // Varaible to get the Row and Column count of three tables

                    int rowsInTab = pDT.Rows.Count;

                    int colsInTab = pDT.Columns.Count;



                    //Font FontBold = new Font("Verdana", 9, FontStyle.Bold);

                    Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);

                    Font HeaderFont = new Font("Verdana", 8, FontStyle.Bold);

                    Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);

                    Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);

                    Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);

                    int[] Arraywidth = new int[colsInTab];

                    //Setting columns width based on the No of columns

                    if (tableLayout.ToUpper().Trim() == "NORMAL")
                    {

                        foreach (DictionaryEntry de in m_htGVColsFVL)
                        {

                            string currentColCaption = de.Key.ToString().Trim().ToUpper();

                            string[] strarr = de.Value.ToString().Split('-');

                            int currentColFVL = Convert.ToInt32(strarr[0].ToString());

                            int currentColSortOrder = Convert.ToInt32(strarr[1].ToString());

                            foreach (DataColumn dc in parentDT.Columns)
                            {

                                if (dc.ColumnName.Trim().ToUpper() == currentColCaption)
                                {

                                    Arraywidth[currentColSortOrder + 1] = 20;//currentColFVL;

                                }

                                else if (dc.ColumnName.Trim().ToUpper() == "TRXID")
                                {

                                    Arraywidth[1] = 15;

                                }

                            }

                        }

                    }

                    else
                    {

                        for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                        {

                            Arraywidth[colCnt] = 20;

                        }

                    }

                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                    //Import DT to PDF table
                    pDT = WrapFullViewLength(pDT, Arraywidth);
                    myPdfTable.ImportDataTable(pDT);

                    //Setting the header row text color as white

                    myPdfTable.HeadersRow.SetForegroundColor(Color.White);

                    //myPdfTable.HeadersRow.SetFont(HeaderFont);

                    myPdfTable.SetBorders(Color.Black, 1, BorderType.None);

                    myPdfTable.Columns[0].SetFont(HeaderFont);

                    if (isSumExists)
                    {

                        myPdfTable.Columns[dt.Columns.Count - 1].SetFont(SumRowFont);

                    }





                    myPdfTable.SetColumnsWidth(Arraywidth);

                    //myPdfTable.SetRowHeight(15);                

                    //Now we set some alignment... for the whole table and then, for a column:

                    myPdfTable.SetContentAlignment(ContentAlignment.MiddleCenter);



                    //Getting Header table

                    string title = string.Empty;

                    XmlNode nodeTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");

                    if (nodeTitle != null)
                    {

                        title = nodeTitle.InnerText.Trim().ToString();

                    }

                    string subTitle = string.Empty;

                    XmlNode nodeSubTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");

                    if (nodeSubTitle != null)
                    {

                        subTitle = nodeSubTitle.InnerText.Trim().ToString();

                    }

                    DataTable HeaderDT = GetHeaderDT(title, subTitle);



                    myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);

                    //Import DT to PDF table

                    myHeaderPdfTable.ImportDataTable(HeaderDT);

                    //myHeaderPdfTable.HeadersRow.SetColors(Color.White, Color.White);

                    myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);

                    //myHeaderPdfTable.SetBackgroundColor(Color.White);

                    myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);

                    myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 155, 55 });

                    int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;

                    myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);

                    if (titleLength < 75)
                    {

                        myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);

                    }

                    myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont);//Page Title

                    int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;

                    if (subTitleLength < 75)
                    {

                        myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);

                    }

                    myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont);//Page subTitle



                    //while (!myHeaderPdfTable.AllTablePagesCreated)

                    {

                        PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));

                        imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;

                        imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;



                        newPdfPage.Add(myHeaderPdfTablePage);





                    }



                    while (!myPdfTable.AllTablePagesCreated)
                    {

                        PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height));

                        newPdfPage.Add(newPdfTablePage);

                    }

                    if (posY > myPdfDocument.PageHeight)
                    {

                        //posY = 70;

                        //newPdfPage = myPdfDocument.NewPage();

                    }

                    else
                    {

                        posY = posY + 200;

                    }



                    // Need to add NotesDT



                    //Getting the dataset to be bound to the grid.

                    XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                    if (nodeBranches != null)
                    {

                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                        {

                            //if (nodeBranch.Attributes["ControlType"] == null)//Need to check

                            {

                                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;



                                DataTable BranchDT = new DataTable();

                                BranchDT = GetBranchDataToPrint(strOutXml, parentTrxID, branchNodeName);

                                BranchDT = ChangeDateAndAmountFormats(BranchDT, strOutXml, branchNodeName);

                                if (BranchDT.Rows.Count > 0)
                                {

                                    Hashtable m_htBranchColsFVL = new Hashtable();



                                    int rowsInBranchDT = BranchDT.Rows.Count;

                                    int colsInBranchDT = BranchDT.Columns.Count;

                                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);



                                    //Getting the columns to be displayed in grid

                                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                    int branchColPos = 0;

                                    //Storing the columns names and captions in the HashTable

                                    foreach (DataColumn dBranchCol in BranchDT.Columns)
                                    {

                                        foreach (XmlNode node in nodeCols)
                                        {

                                            if (node.Attributes["Caption"].Value == dBranchCol.ColumnName)
                                            {

                                                if (!m_htBranchColsFVL.Contains(node.Attributes["Caption"].Value))
                                                {

                                                    //if (node.Attributes["FullViewLength"].Value != "0")

                                                    {

                                                        m_htBranchColsFVL.Add(node.Attributes["Caption"].Value, node.Attributes["FullViewLength"].Value + "-" + branchColPos);

                                                    }

                                                }

                                                branchColPos++;

                                            }

                                        }

                                    }



                                    //Truncating the data if it is greater than its FullView Length

                                    foreach (DataRow dBranchRow in BranchDT.Rows)
                                    {

                                        foreach (DictionaryEntry de in m_htBranchColsFVL)
                                        {

                                            string currentColCaption = de.Key.ToString().Trim().ToUpper();

                                            string[] strarr = de.Value.ToString().Split('-');

                                            int currentColFVL = Convert.ToInt32(strarr[0].ToString());

                                            int currentColSortOrder = Convert.ToInt32(strarr[1].ToString());

                                            if (currentColFVL != 0)
                                            {

                                                foreach (DataColumn dBranchCol in BranchDT.Columns)
                                                {

                                                    if (dBranchCol.ColumnName.Trim().ToUpper() == currentColCaption)
                                                    {

                                                        int datavalLength = dBranchRow[dBranchCol.ColumnName].ToString().Length;

                                                        if (dBranchRow[dBranchCol.ColumnName].ToString().Length > currentColFVL)
                                                        {

                                                            dBranchRow[dBranchCol] = dBranchRow[dBranchCol].ToString().Remove(currentColFVL - 3) + "...";

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                    }



                                    int[] BranchArraywidth = new int[colsInBranchDT];

                                    //for (int colCnt = 0; colCnt < colsInBranchDT; colCnt++)

                                    //{

                                    //    BranchArraywidth[colCnt] = 20;

                                    //}

                                    foreach (DictionaryEntry de in m_htBranchColsFVL)
                                    {

                                        string currentColCaption = de.Key.ToString().Trim().ToUpper();

                                        string[] strarr = de.Value.ToString().Split('-');

                                        int currentColFVL = Convert.ToInt32(strarr[0].ToString());

                                        int currentColSortOrder = Convert.ToInt32(strarr[1].ToString());

                                        foreach (DataColumn dc in BranchDT.Columns)
                                        {

                                            if (dc.ColumnName.Trim().ToUpper() == currentColCaption)
                                            {

                                                if (currentColFVL != 0)
                                                {

                                                    BranchArraywidth[currentColSortOrder] = currentColFVL;

                                                }

                                                else
                                                {

                                                    BranchArraywidth[currentColSortOrder] = 15;

                                                }

                                            }

                                        }

                                    }



                                    myPdfTable1.ImportDataTable(BranchDT);

                                    myPdfTable1.HeadersRow.SetFont(HeaderFont);

                                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);

                                    myPdfTable1.SetColumnsWidth(BranchArraywidth);



                                    //myPdfTable1.SetRowHeight(15);

                                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleCenter);



                                    while (!myPdfTable1.AllTablePagesCreated)
                                    {

                                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height));

                                        newPdfPage.Add(newPdfTablePage1);

                                    }
                                    if (posY > myPdfDocument.PageHeight)
                                    {
                                        //posY = 70;
                                        //newPdfPage = myPdfDocument.NewPage();
                                    }
                                    else
                                    {
                                        posY = posY + 200;
                                    }
                                }
                            }
                        }
                    }
                    newPdfPage.SaveToDocument();
                    //Deleting the printed row from parentDT
                    if (tableLayout.ToUpper().Trim() == "PIVOT")
                    {
                        pDT.Columns.RemoveAt(1);
                        parentDT.Rows[0].Delete();
                    }
                }
                //SaveToResponse("PDFReport");

                if (ConfigurationManager.AppSettings["ShowPDF"].ToString() == "0")
                {

                    HttpContext.Current.Response.ClearHeaders();

                    HttpContext.Current.Response.ClearContent();

                    HttpContext.Current.Response.Clear();

                    HttpContext.Current.Response.Buffer = true;

                    HttpContext.Current.Response.ContentType = "application/pdf";

                    HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("attachment;filename={0}.pdf", fileName));

                    myPdfDocument.SaveToStream(HttpContext.Current.Response.OutputStream);

                    HttpContext.Current.Response.Flush();

                    HttpContext.Current.Response.Close();

                }

                else
                {

                    HttpContext.Current.Response.ClearHeaders();

                    HttpContext.Current.Response.ClearContent();

                    HttpContext.Current.Response.Clear();

                    HttpContext.Current.Response.Buffer = true;

                    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("inline;filename={0}.pdf", fileName));

                    HttpContext.Current.Response.ContentType = "application/pdf";

                    myPdfDocument.SaveToStream(HttpContext.Current.Response.OutputStream);

                    HttpContext.Current.Response.Flush();

                    HttpContext.Current.Response.Close();

                }

            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
                //throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Wrapping Full view length in to Specific FullView Length
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="arrayWidth"></param>
        /// <returns></returns>
        public DataTable WrapFullViewLength(DataTable dt, int[] arrayWidth)
        {
            #region Nlog
            logger.Info("Wrapping Full view length in to Specific FullView Length.");
            #endregion

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 0; col < dt.Columns.Count - 1; col++)
                {
                    int drLength = dt.Rows[row][col].ToString().Length;
                    int arrLength = Convert.ToInt32(arrayWidth[col].ToString());
                    arrLength = arrLength + 3;
                    if (drLength != 0)
                    {
                        if (drLength > arrLength)
                        {
                            //string splittedString = dt.Rows[row][col].ToString().Substring(0, arrLength - 3);
                            //dt.Rows[row][col] = splittedString.ToString() + "...";

                            //Above lines commented and the following block added by Danny on 22/09/09
                            string origStr = dt.Rows[row][col].ToString();
                            int i = 1;
                            while (i * arrLength < origStr.Length)
                            {
                                //Insert a line break at every specified length.
                                //Get the immediate space in the text.
                                int indexOfSpace = origStr.IndexOf(" ", i * arrLength);
                                if (indexOfSpace != -1)
                                {
                                    origStr = origStr.Insert(++indexOfSpace, Environment.NewLine);
                                }
                                i++;
                            }
                            dt.Rows[row][col] = origStr;
                        }
                        else
                        {
                            if (drLength > 18)
                            {
                                //string splittedString = dt.Rows[row][col].ToString().Substring(0, 16);
                                //dt.Rows[row][col] = splittedString.ToString() + "...";
                            }
                        }
                    }
                }
            }
            return dt;
        }

        #region GenerateReportStyle
        public void GenerateReportStyle(string currentBPGID, Panel pnlEntryForm)
        {
            #region Nlog
            logger.Info("Generating the report style with BPGID as : "+currentBPGID );
            #endregion

            //try

            //{

            //Calling the Submit method by passing the content page Update Panel as parameter



            string strOutXml = SubmitReport("Report", pnlEntryForm, currentBPGID);

            if (strOutXml != string.Empty)
            {

                XmlDocument returnXMLDoc = new XmlDocument();

                returnXMLDoc.LoadXml(strOutXml);



                //success messge

                XmlNode nodeMsg = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");

                XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");

                if (nodeMsgStatus != null)
                {

                    Label lblmsg = (Label)pnlEntryForm.FindControl("lblmsg");

                    if (lblmsg != null)
                    {

                        lblmsg.Visible = true;

                        lblmsg.Text = string.Empty;

                    }



                    if (nodeMsgStatus.InnerText == "Success")
                    {

                        //ClearUIControls(pnlEntryForm);

                        if (nodeMsg != null)
                        {

                            if (lblmsg != null)

                                lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;

                        }

                        else
                        {

                            if (lblmsg != null)

                                lblmsg.Text = nodeMsgStatus.InnerText;

                        }



                        string reportStyle = string.Empty;

                        XmlNode nodeTreenode = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");

                        XmlNode nodeRows = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + nodeTreenode.SelectSingleNode("Node").InnerText + "/RowList");

                        if (nodeRows != null)
                        {

                            if (nodeRows.ChildNodes.Count > 0)
                            {

                                if (nodeTreenode.Attributes["ReportStyle"] != null)
                                {

                                    reportStyle = nodeTreenode.Attributes["ReportStyle"].Value;

                                }

                                if (reportStyle != string.Empty)
                                {

                                    //Generates report only if report style is not 0

                                    if (reportStyle != "0")
                                    {

                                        GenerateReport(returnXMLDoc, "PDF");

                                    }

                                }

                            }

                        }

                    }

                    else if (nodeMsgStatus.InnerText == "Error")
                    {

                        if (nodeMsg != null)
                        {

                            if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                            {

                                if (lblmsg != null)

                                    lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;

                            }

                            else if (nodeMsg.SelectSingleNode("Label") != null)
                            {

                                if (lblmsg != null)

                                    lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;

                            }

                            else
                            {

                                if (lblmsg != null)

                                    lblmsg.Text = nodeMsgStatus.InnerText;

                            }

                        }

                        else
                        {

                            if (lblmsg != null)

                                lblmsg.Text = nodeMsgStatus.InnerText;

                        }

                    }

                }

            }

            //}

            //catch (Exception ex)

            //{ 



            //}



        }

        #endregion

        #region GetBranchDTWithSkipLines

        public DataTable GetBranchDTWithSkipLines(DataTable BranchDT, XmlNodeList nodeResRows)
        {

            if (nodeResRows != null)
            {

                if (nodeResRows.Count > 0)
                {

                    foreach (XmlNode nodeResRow in nodeResRows)
                    {

                        if (nodeResRow.Attributes["TrxID"] != null)
                        {

                            string trxID = nodeResRow.Attributes["TrxID"].Value;

                            DataRow[] foundRows;

                            foundRows = BranchDT.Select("TrxID ='" + trxID + "'");



                            if (foundRows.Length > 0)
                            {

                                int rowIndex = BranchDT.Rows.IndexOf(foundRows[0]);



                                if (nodeResRow.Attributes["pLnSkip"] != null)
                                {

                                    if (nodeResRow.Attributes["pLnSkip"].Value.ToString().Trim() != "0")
                                    {

                                        for (int skipCnt = 0; skipCnt < Convert.ToInt32(nodeResRow.Attributes["pLnSkip"].Value.Trim()); skipCnt++)
                                        {

                                            // Adding each row at a time

                                            DataRow dSkipRow = BranchDT.NewRow();

                                            for (int col = 0; col < BranchDT.Columns.Count; col++)
                                            {

                                                dSkipRow[BranchDT.Columns[col].ColumnName] = "SKIP";

                                            }

                                            BranchDT.Rows.InsertAt(dSkipRow, rowIndex + 1);

                                            BranchDT.AcceptChanges();

                                        }

                                    }

                                }



                                //if (nodeResRow.Attributes["pPgBreak"] != null)

                                //{

                                //    if (nodeResRow.Attributes["pPgBreak"].Value.ToString().Trim() == "1")

                                //    {

                                //        branchPageBreaks++;

                                //        m_htBranchPagebrk.Add(branchPageBreaks, rowIndex);

                                //    }

                                //}

                            }

                        }

                    }

                }

            }



            return BranchDT;

        }

        #endregion

        //#region PARENT CHILD JOB COSTING
        //public void GVParentChildJobCosting(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, string strOutXml, string tableLayout, string treeNodeName, string parentID, int PageCount)
        //{
        //    try
        //    {
        //        Hashtable m_htRightAlign = new Hashtable();
        //        //Storing the fullview length of cols in hashtable                    
        //        XmlDocument xDoc = new XmlDocument();
        //        xDoc.LoadXml(strOutXml);
        //        XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
        //        //Getting the rows to print
        //        XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
        //        //Considering the PLayout only for the first treenode
        //        if (nodeRowList != null)
        //        {
        //            if (myPdfDocument.PageCount == 0)
        //            {
        //                if (nodeRowList.FirstChild != null)
        //                {
        //                    if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
        //                    {
        //                        string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
        //                        if (pLayout == "1")//Landscape
        //                        {
        //                            myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        bool isSumExists = false;
        //        int[] Arraywidth = new int[dt.Columns.Count - 3];//not considering trxid col whose ordinal is 0                //Storing the column width based on FVL in an array
        //        //Storing the captions of cols having IsSummed and IsNumeric as 1 in the HashTable
        //        int cntr = 0;
        //        foreach (XmlNode node in nodeColumns)
        //        {
        //            if (node.Attributes["Caption"] != null)
        //            {
        //                DataColumn dc = dt.Columns[node.Attributes["Caption"].Value];
        //                if (dc != null)
        //                {
        //                    if (dc.ColumnName.Trim().ToString() == "JobID")
        //                    {
        //                        cntr++;
        //                        continue;
        //                    }
        //                    else if (dc.ColumnName.Trim().ToString() == "AccountID")
        //                    {
        //                        cntr++;
        //                        continue;
        //                    }
        //                    //Set the column width based on FVL
        //                    else if (node.Attributes["FullViewLength"] != null)
        //                    {
        //                        if (node.Attributes["FullViewLength"].Value != "0")
        //                        {
        //                            Arraywidth[dc.Ordinal - cntr] = Convert.ToInt32(node.Attributes["FullViewLength"].Value);
        //                        }
        //                        else
        //                        {
        //                            Arraywidth[dc.Ordinal - cntr] = 15;
        //                        }
        //                    }
        //                    //Getting the cols having Issummed=1 
        //                    if (node.Attributes["IsSummed"] != null)
        //                    {
        //                        if (node.Attributes["IsSummed"].Value == "1")
        //                        {
        //                            if (!isSumExists)
        //                                isSumExists = true;
        //                            if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
        //                                m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["IsSummed"].Value);
        //                        }
        //                    }
        //                    //Getting the cols having ControlType="Amount"//Isnumeric=1
        //                    if (node.Attributes["ControlType"] != null)
        //                    {
        //                        if (node.Attributes["ControlType"].Value == "Amount")
        //                        {
        //                            if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
        //                                m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["ControlType"].Value);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //Setting columns width for cols not present in node columns
        //        for (int colCnt = 0; colCnt < dt.Columns.Count - 3; colCnt++)
        //        {
        //            if (Arraywidth[colCnt].ToString() == string.Empty)
        //                Arraywidth[colCnt] = 15;
        //        }
        //        dt = ChangeDateAndAmountFormats(dt, strOutXml, treeNodeName);
        //        //Removing the Summed row from parent table
        //        if (isSumExists)
        //        {
        //            dt.Rows.RemoveAt(dt.Rows.Count - 1);
        //        }
        //        //Creating Data table containing the column names
        //        DataTable parentDT = new DataTable();
        //        for (int col = 0; col < dt.Columns.Count; col++)
        //        {
        //            DataColumn colNames = new DataColumn();
        //            colNames.ColumnName = dt.Columns[col].ColumnName;
        //            colNames.DataType = dt.Columns[col].DataType;
        //            parentDT.Columns.Add(colNames);
        //        }
        //        //Setting the X and Y positons, width and height of the tables
        //        double posX = 20;
        //        double posY = 70;
        //        double width = myPdfDocument.PageWidth - 50;
        //        double height = myPdfDocument.PageHeight - 50;
        //        double currentYPos = 70;

        //        string imgpath = PDFImagePath();
        //        PdfImage LogoImage = null;
        //        if (!string.IsNullOrEmpty(imgpath))
        //        {
        //            LogoImage = myPdfDocument.NewImage(imgpath);
        //        }
        //        Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
        //        Font HeaderFont = new Font("Verdana", 8, FontStyle.Bold | FontStyle.Underline);
        //        Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
        //        Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
        //        Font RowFontBold = new Font("Verdana", 8, FontStyle.Bold);
        //        Font FontUnderline = new Font("Verdana", 8, FontStyle.Regular | FontStyle.Underline);
        //        Font RowBoxFontBold = new Font("Verdana", 10, FontStyle.Bold);
        //        Font dateFont = new Font("Verdana", 8, FontStyle.Bold);
        //        //Getting header DT for this treenode
        //        DataTable HeaderDT = new DataTable();
        //        //Getting Header table
        //        string title = string.Empty;
        //        XmlNode nodeTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/Job/GridHeading/Title");
        //        if (nodeTitle != null)
        //        {
        //            title = nodeTitle.InnerText.Trim().ToString();
        //        }
        //        string subTitle = string.Empty;
        //        XmlNode nodeSubTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/Job/GridHeading/SubTitle");
        //        if (nodeSubTitle != null)
        //        {
        //            subTitle = nodeSubTitle.InnerText.Trim().ToString();
        //        }
        //        //Getting header DT for this treenode
        //        HeaderDT = GetHeaderJobCostingDT(title, subTitle, xDoc, parentID);
        //        //
        //        myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
        //        //Import HeaderDT to PDF table
        //        myHeaderPdfTable.ImportDataTable(HeaderDT);
        //        //myHeaderPdfTable.HeadersRow.SetColors(Color.White, Color.White);
        //        myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
        //        //myHeaderPdfTable.SetBackgroundColor(Color.White);
        //        myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
        //        myHeaderPdfTable.SetColumnsWidth(new int[] { 190, 300, 135, 35 });
        //        int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
        //        myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
        //        Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
        //        Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
        //        Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
        //        if (titleLength < 75)
        //        {
        //            myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
        //        }
        //        myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);
        //        //
        //        int subTitleLength = myHeaderPdfTable.Rows[1][1].Content.ToString().Length;
        //        if (subTitleLength < 75)
        //        {
        //            myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
        //        }
        //        myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);
        //        //
        //        int dateLength = myHeaderPdfTable.Rows[0][2].Content.ToString().Length;
        //        if (dateLength < 75)
        //        {
        //            myHeaderPdfTable.Rows[0][2].SetContentAlignment(ContentAlignment.MiddleCenter);
        //        }
        //        myHeaderPdfTable.Rows[0][2].SetFont(HeaderPageTitleFont3);
        //        if (HeaderDT.Rows.Count > 2)
        //        {
        //            if (HeaderDT.Rows[2][1].ToString() != string.Empty)
        //            {
        //                myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
        //            }
        //        }
        //        if (HeaderDT.Columns.Count > 1)
        //        {
        //            if (HeaderDT.Rows.Count > 2)
        //            {
        //                if (HeaderDT.Rows[2][1].ToString() != string.Empty)
        //                {
        //                    myHeaderPdfTable.Rows[0][2].SetContentAlignment(ContentAlignment.MiddleLeft);
        //                    myHeaderPdfTable.Rows[1][2].SetContentAlignment(ContentAlignment.MiddleLeft);
        //                }
        //            }
        //        }
        //        PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 140));
        //        imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
        //        imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;
        //        //Adding New page for the first time
        //        PdfPage newPdfPage = myPdfDocument.NewPage();
        //        //Adding header table in the first page
        //        newPdfPage.Add(myHeaderPdfTablePage);
        //        if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //        foreach (DataRow dRow in dt.Rows)
        //        {
        //            //Adding each row at a time
        //            DataRow dNewRow = parentDT.NewRow();
        //            for (int col = 0; col < dt.Columns.Count; col++)
        //            {
        //                dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
        //            }
        //            parentDT.Rows.Add(dNewRow);
        //            string parentTrxID = string.Empty;
        //            parentTrxID = parentDT.Rows[0]["TrxID"].ToString();
        //            string JobID = string.Empty;
        //            JobID = parentDT.Rows[0]["JobID"].ToString();
        //            string AccountID = string.Empty;
        //            AccountID = parentDT.Rows[0]["AccountID"].ToString();
        //            //Removing TrxID column
        //            if (parentDT.Columns.Contains("TrxID"))
        //            {
        //                parentDT.Columns.Remove("TrxID");
        //            }
        //            //Removing JobID column
        //            if (parentDT.Columns.Contains("JobID"))
        //            {
        //                parentDT.Columns.Remove("JobID");
        //            }
        //            //Removing AccountID column
        //            if (parentDT.Columns.Contains("AccountID"))
        //            {
        //                parentDT.Columns.Remove("AccountID");
        //            }
        //            // Varaible to get the Row and Column count of three tables
        //            int rowsInTab = parentDT.Rows.Count;
        //            int colsInTab = parentDT.Columns.Count;
        //            PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
        //            //Import DT to PDF table
        //            parentDT = WrapFullViewLength(parentDT, Arraywidth);
        //            myPdfTable.ImportDataTable(parentDT);
        //            myPdfTable.HeadersRow.SetFont(HeaderFont);
        //            //myPdfTable.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
        //            myPdfTable.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
        //            myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
        //            myPdfTable.SetColumnsWidth(Arraywidth);
        //            myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
        //            if (m_htRightAlign.Count > 0)
        //            {
        //                //Right justifying Summed column content and IsNumeric column content
        //                for (int pdfcol = 0; pdfcol < colsInTab; pdfcol++)
        //                {
        //                    if (myPdfTable.HeadersRow[pdfcol].Content.ToString() != string.Empty)
        //                    {
        //                        if (m_htRightAlign.Contains(myPdfTable.HeadersRow[pdfcol].Content.ToString()))
        //                        {
        //                            myPdfTable.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
        //                            myPdfTable.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
        //                        }
        //                    }
        //                }
        //            }
        //            //HeaderTable placed on top
        //            while (!myPdfTable.AllTablePagesCreated)
        //            {
        //                //Setting the Y position and if required creating new page
        //                if (currentYPos > myPdfDocument.PageHeight - 50)
        //                {
        //                    posY = 70;
        //                    currentYPos = 70;
        //                    newPdfPage.SaveToDocument();
        //                    //Adding new page and adding Header table,logo image and pageNo 
        //                    newPdfPage = myPdfDocument.NewPage();
        //                    newPdfPage.Add(myHeaderPdfTablePage);
        //                    //Adding logo
        //                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                }
        //                else
        //                {
        //                    if (posY != 130)
        //                        posY = currentYPos + 35;
        //                }
        //                if (myPdfDocument.PageHeight - 50 - posY < 50)
        //                {
        //                    posY = 70;
        //                    currentYPos = 70;
        //                    newPdfPage.SaveToDocument();
        //                    //Adding new page and adding Header table,logo image and pageNo 
        //                    newPdfPage = myPdfDocument.NewPage();
        //                    newPdfPage.Add(myHeaderPdfTablePage);
        //                    //Adding logo
        //                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                }
        //                PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height));
        //                newPdfPage.Add(newPdfTablePage);
        //                currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
        //            }
        //            //Notes DT
        //            if (NotesDT.Rows.Count > 0)
        //            {
        //                if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
        //                {
        //                    int rowsInNotesDT = NotesDT.Rows.Count;
        //                    int colsInNotesDT = NotesDT.Columns.Count;
        //                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
        //                    myPdfTable1.ImportDataTable(NotesDT);
        //                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
        //                    myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
        //                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
        //                    myPdfTable1.HeadersRow.SetFont(HeaderFont);
        //                    myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
        //                    myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
        //                    while (!myPdfTable1.AllTablePagesCreated)
        //                    {
        //                        //Setting the Y position and if required creating new page
        //                        if (currentYPos > myPdfDocument.PageHeight - 50)
        //                        {
        //                            posY = 70;
        //                            currentYPos = 70;
        //                            newPdfPage.SaveToDocument();
        //                            //Adding new page and adding Header table,logo image and pageNo 
        //                            newPdfPage = myPdfDocument.NewPage();
        //                            newPdfPage.Add(myHeaderPdfTablePage);
        //                            //Adding logo
        //                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                        }
        //                        else
        //                        {
        //                            posY = currentYPos + 25;
        //                        }
        //                        if (myPdfDocument.PageHeight - 50 - posY < 50)
        //                        {
        //                            posY = 70;
        //                            currentYPos = 70;
        //                            newPdfPage.SaveToDocument();
        //                            //Adding new page and adding Header table,logo image and pageNo 
        //                            newPdfPage = myPdfDocument.NewPage();
        //                            newPdfPage.Add(myHeaderPdfTablePage);
        //                            //Adding logo
        //                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                        }
        //                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
        //                        newPdfPage.Add(newPdfTablePage1);
        //                        currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
        //                    }
        //                }
        //            }
        //            //Branch DT
        //            foreach (XmlNode nodes in xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout"))
        //            {
        //                if (nodes.InnerText.Trim().ToString() == "TrxInfo")
        //                {
        //                    {
        //                        {

        //                            {
        //                                string branchNodeName = nodes.InnerText;
        //                                DataTable BranchDT = new DataTable();
        //                                BranchDT = GetJobCostingBranchDataToPrint(strOutXml, parentTrxID, branchNodeName);
        //                                BranchDT = ChangeDateAndAmountFormats(BranchDT, strOutXml, branchNodeName);
        //                                if (BranchDT.Rows.Count > 0)
        //                                {
        //                                    Hashtable m_htBranchRightAlign = new Hashtable();
        //                                    Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
        //                                    bool sumExists = false;
        //                                    int rowsInBranchDT = BranchDT.Rows.Count;
        //                                    int colsInBranchDT = BranchDT.Columns.Count;
        //                                    //Getting the columns to be displayed in grid
        //                                    XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
        //                                    int branchColPos = 0;
        //                                    int[] BranchArraywidth = new int[colsInBranchDT];
        //                                    foreach (DataColumn dBranchCol in BranchDT.Columns)
        //                                    {
        //                                        XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");
        //                                        //Checking for isSummed value for that column
        //                                        if (nodeBranchCol != null)
        //                                        {
        //                                            //Getting the cols having Issummed=1 
        //                                            if (nodeBranchCol.Attributes["IsSummed"] != null)
        //                                            {
        //                                                if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
        //                                                {
        //                                                    if (!sumExists)
        //                                                        sumExists = true;
        //                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
        //                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
        //                                                }
        //                                            }
        //                                            //Getting the cols having ControlType="Amount"//Isnumeric=1
        //                                            if (nodeBranchCol.Attributes["ControlType"] != null)
        //                                            {
        //                                                if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
        //                                                {
        //                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
        //                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
        //                                                }
        //                                            }
        //                                        }
        //                                        //Setting the column width of branch table
        //                                        int dcPos = dBranchCol.Ordinal;
        //                                        int colFVL = 0;
        //                                        if (nodeBranchCol != null)
        //                                        {
        //                                            if (nodeBranchCol.Attributes["FullViewLength"] != null)
        //                                            {
        //                                                colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
        //                                            }
        //                                        }
        //                                        if (colFVL != 0)
        //                                        {
        //                                            BranchArraywidth[dcPos] = colFVL;
        //                                        }
        //                                        else
        //                                        {
        //                                            BranchArraywidth[dcPos] = 15;
        //                                        }
        //                                    }
        //                                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);
        //                                    Font myHeaderFont = new Font("Verdana", 7, FontStyle.Bold);
        //                                    myPdfTable1.ImportDataTable(BranchDT);
        //                                    myPdfTable1.HeadersRow.SetFont(myHeaderFont);
        //                                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
        //                                    myPdfTable1.SetColumnsWidth(BranchArraywidth);
        //                                    myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
        //                                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
        //                                    if (m_htBranchRightAlign.Count > 0)
        //                                    {
        //                                        if (sumExists)
        //                                        {
        //                                            //Right justifying Summed row content
        //                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
        //                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
        //                                        }
        //                                        //Right justifying Summed column content and IsNumeric column content
        //                                        for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
        //                                        {
        //                                            if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
        //                                            {

        //                                                if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
        //                                                {
        //                                                    myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
        //                                                    myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    while (!myPdfTable1.AllTablePagesCreated)
        //                                    {
        //                                        //Setting the Y position and if required creating new page
        //                                        if (currentYPos > myPdfDocument.PageHeight - 50)
        //                                        {
        //                                            posY = 70;
        //                                            currentYPos = 70;
        //                                            newPdfPage.SaveToDocument();
        //                                            //Adding new page and adding Header table,logo image and pageNo 
        //                                            newPdfPage = myPdfDocument.NewPage();
        //                                            newPdfPage.Add(myHeaderPdfTablePage);
        //                                            //Adding logo
        //                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                                        }
        //                                        else
        //                                        {
        //                                            posY = currentYPos + 10;
        //                                        }
        //                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
        //                                        {
        //                                            posY = 70;
        //                                            currentYPos = 70;
        //                                            newPdfPage.SaveToDocument();
        //                                            //Adding new page and adding Header table,logo image and pageNo 
        //                                            newPdfPage = myPdfDocument.NewPage();
        //                                            newPdfPage.Add(myHeaderPdfTablePage);
        //                                            //Adding logo
        //                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
        //                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++PageCount))));
        //                                        }
        //                                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
        //                                        newPdfPage.Add(newPdfTablePage1);
        //                                        currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (tableLayout.ToUpper().Trim() == "NORMAL")
        //                    {
        //                        parentDT.Rows[0].Delete();
        //                        //Adding TrxID column
        //                        if (!parentDT.Columns.Contains("TrxID"))
        //                        {
        //                            parentDT.Columns.Add("TrxID");
        //                        }
        //                        if (!parentDT.Columns.Contains("JobID"))
        //                        {
        //                            parentDT.Columns.Add("JobID");
        //                        }
        //                        if (!parentDT.Columns.Contains("AccountID"))
        //                        {
        //                            parentDT.Columns.Add("AccountID");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        newPdfPage.SaveToDocument();
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //    }
        //}
        //#endregion

        public void CreateXSLSheet(string m_outXML)
        {
            #region Nlog
            logger.Info("Creating the XSL Sheet form the outXML.");
            #endregion

            StringReader sr = new StringReader(m_outXML);
            XPathDocument surveyDoc = new XPathDocument(sr);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(m_outXML);
            XslCompiledTransform xTransform = new XslCompiledTransform();
            xTransform.Load(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ReportStyle1.XSLT");
            // Get the transformed result
            StringWriter sw = new StringWriter();
            xTransform.Transform(surveyDoc, null, sw);
            string result = sw.ToString();
        }

        /// <summary>
        /// Change Date and Amount Formats
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outxml"></param>
        /// <param name="treeNodeName"></param>
        /// <returns></returns>
        public DataTable ChangeDateAndAmountFormats(DataTable dt, string outxml, string treeNodeName)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(outxml);
            string m_AmountNodes = string.Empty;
            string m_DateFormats = string.Empty;
            foreach (XmlNode xns in xDoc.SelectSingleNode("//" + treeNodeName + "/GridHeading/Columns").ChildNodes)
            {
                if (xns.Attributes["ControlType"] != null)
                {
                    if (xns.Attributes["ControlType"].Value == "Cal")
                    {
                        m_DateFormats = xns.Attributes["Caption"].Value;
                    }
                    if (xns.Attributes["ControlType"].Value == "Amount" || xns.Attributes["ControlType"].Value == "Calc")
                    {
                        m_AmountNodes = xns.Attributes["Caption"].Value;
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ColumnName == m_AmountNodes)
                            {
                                decimal amount;
                                if (Decimal.TryParse(dr[m_AmountNodes].ToString(), out amount))
                                {
                                    string amt = string.Format("{0:N}", amount);
                                    dr[m_AmountNodes] = amt;
                                }
                            }
                            if (dc.ColumnName == m_DateFormats)
                            {
                                DateTime date;
                                if (commonObjUI.IsDate(dr[m_DateFormats].ToString()))
                                {
                                    //if the value is IsDate then change format MM/DD/YYYY
                                    DateTime.TryParse(dr[m_DateFormats].ToString(), out date);
                                    string dates = date.ToString("MM/dd/yy");
                                    dr[m_DateFormats] = dates;
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public string PDFImagePath()
        {
            XmlDocument xDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            XmlNode m_CompanyWhiteNode = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/WhiteReportLogo");
            XmlNode m_CompanyGreyNode = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/GreyReportLogo");
            string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string m_CompanyImageSrc = string.Empty;
            string m_ImgPath = string.Empty;

            //SET cdn image path
            string m_strImagesCDNPath = ConfigurationManager.AppSettings["ImagesPath"].ToString() + "/" + HttpContext.Current.Session["MyTheme"] + "/";

            if (m_CompanyWhiteNode != null)
            {
                m_CompanyImageSrc = m_CompanyWhiteNode.InnerXml.ToString();
                m_ImgPath = Convert.ToString(ConfigurationSettings.AppSettings["AttachmentsPath"]) + "/" + HttpContext.Current.Session["CompanyEntityID"].ToString() + "/" + m_CompanyImageSrc;
                if ((!m_CompanyImageSrc.ToString().ToUpper().Contains("JPG")) && (!m_CompanyImageSrc.ToString().ToUpper().Contains("JPEG")))
                {
                    m_ImgPath = m_strImagesCDNPath + "Images/lajit_small-greylogo_03.JPG";
                }
                else
                {
                    if (!File.Exists(m_ImgPath))
                    {
                        m_ImgPath = m_strImagesCDNPath + "Images/lajit_small-greylogo_03.JPG";
                    }
                }
            }
            else if (m_CompanyGreyNode != null)
            {
                m_CompanyImageSrc = m_CompanyGreyNode.InnerXml.ToString();
                m_ImgPath = Convert.ToString(ConfigurationSettings.AppSettings["AttachmentsPath"]) + "/" + HttpContext.Current.Session["CompanyEntityID"].ToString() + "/" + m_CompanyImageSrc;

                if ((!m_CompanyImageSrc.ToString().ToUpper().Contains("JPG")) && (!m_CompanyImageSrc.ToString().ToUpper().Contains("JPEG")))
                {
                    m_ImgPath = m_strImagesCDNPath + "Images/lajit_small-greylogo_03.JPG";
                }
                else
                {
                    if (!File.Exists(m_ImgPath))
                    {
                        m_ImgPath = m_strImagesCDNPath + "Images/lajit_small-greylogo_03.JPG";
                    }
                }
            }
            else
            {
                m_ImgPath = m_strImagesCDNPath + "Images/lajit_small-greylogo_03.JPG";
            }
            return m_ImgPath;
        }

        #region Grand Totals
        public DataTable CreateGrandTotals(DataTable dt, Hashtable htGrandCols)
        {
            #region Nlog
            logger.Info("Calculate the grand totals in the entire report.");
            #endregion

            DataTable dtNew = new DataTable();
            int tot = 0;
            IDictionaryEnumerator enumerator = htGrandCols.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (dt.Columns.Contains(enumerator.Key.ToString()))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows.Add(dt.NewRow());

                        tot = tot + Convert.ToInt32(dt.Rows[i][enumerator.Key.ToString()].ToString());
                    }
                    dt.Rows[dt.Rows.Count - 1][enumerator.Key.ToString()] = tot;
                }
            }
            return dtNew;
        }
        #endregion





    }
}
