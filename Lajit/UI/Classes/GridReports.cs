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
using Gios.Pdf;
using ICSharpCode.SharpZipLib.Zip;
using NLog;


namespace LAjitDev
{
    public enum ReportType
    {
        PDF,
        Excel,
        HTML,
        Word
    };

    public enum PagesToPrint
    {
        /// <summary>
        /// If the current page is to be printed, intialise only BPOut property
        /// </summary>
        Current,
        /// <summary>
        /// If all the pages are to be printed, intialise only BPInfo property
        /// </summary>
        All
    };

    public class GridReports
    {
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        private CommonUI m_ObjCommonUI;//= new CommonUI();//Get the reference from the calling page 
        private PdfDocument myPdfDocument;
        private Hashtable m_htGVColumns = new Hashtable();
        private string m_BPInfo;
        private string m_BPOut;
        private bool m_ShowPDF = false;
        XmlDocument XDocUserInfo = new XmlDocument();
        public int m_CustReptHidden = Convert.ToInt32(ConfigurationManager.AppSettings["CustReptHidden"].ToString());
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Holds the reference for the instance of CommonUI which was instantiated in the BasePage.
        /// </summary>
        public CommonUI ObjCommonUI
        {
            get
            {
                return m_ObjCommonUI;
            }
            set
            {
                m_ObjCommonUI = value;
            }
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

        public string BPInfo
        {
            get
            {
                return m_BPInfo;
            }
            set
            {
                m_BPInfo = value;
            }
        }

        public string BPOut
        {
            get
            {
                return m_BPOut;
            }
            set
            {
                m_BPOut = value;
            }
        }

        public GridReports(CommonUI obj)
        {
            m_ObjCommonUI = obj;
        }

        private string m_FilePath;

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

        private string m_SessionUserInfo;

        public string SessionUserInfo
        {
            get { return m_SessionUserInfo; }
            set { m_SessionUserInfo = value; }
        }

        private string m_Theme;

        public string Theme
        {
            get { return m_Theme; }
            set { m_Theme = value; }
        }



        public void PrintData(string reportType, PagesToPrint pages, string PBDval, string groupByCol, params string[] arrSelectedColumns)
        {
            ////***********************************
            //System.Threading.Thread.Sleep(20000);

            //string strFilePath = ConfigurationManager.AppSettings["ReportsPathVirtual"] + "/PDFRpt.pdf";
            //return strFilePath;
            ////***********************************

            if (BPOut == null || BPOut.Length == 0 || pages == PagesToPrint.All)
            {
                string pageSize = string.Empty;
                if (pages == PagesToPrint.All)
                {
                    pageSize = "-1";
                }
                //If the print request is for all the pages hit the DB with PageSize=-1 to get data for all the 
                //pages.Use the BPInfo for this.
                string reqXML = GenerateRequestXML(BPInfo, pageSize);
                BPOut = reportsBO.GetReportBPEOut(reqXML);
            }
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(BPOut);
            GenerateRptOutXML(xDocOut, reportType, PBDval, groupByCol, arrSelectedColumns);
        }

        public void GenerateRptOutXML(XmlDocument xDocOut, string reportType, string PBDVal, string groupByCol, params string[] arrSelectedColumns)
        {
            try
            {
                XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                clsReportsUI objReportsUI = new clsReportsUI();
                string treeNodeName = nodeTreenode.FirstChild.InnerText;
                int isPrint = Convert.ToInt32(xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value);
                if (isPrint == 1)
                {
                    XmlNode xReportStyleNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree");
                    if (nodeTreenode.Attributes["ReportStyle"] == null)
                    {
                        XmlAttribute attrReportStyle = xDocOut.CreateAttribute("ReportStyle");
                        xReportStyleNode.Attributes.Append(attrReportStyle);
                        //Check for the web.config key as the final override
                        if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[@ControlType='GView']") != null)
                        {
                            attrReportStyle.Value = "5";
                        }
                        else
                        {
                            attrReportStyle.Value = "1";
                        }
                    }
                    if (PBDVal == "0")
                    {
                        nodeTreenode.Attributes["ReportStyle"].Value = "1";
                    }
                }
                if (groupByCol.Length > 0)
                {
                    objReportsUI.GenerateReport(xDocOut, reportType, groupByCol, arrSelectedColumns);
                }
                else
                {
                    objReportsUI.GenerateReport(xDocOut, reportType, arrSelectedColumns);
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }
        public string GenerateRequestXML(string requestXML, string pageSize, params string[] xmlPath)
        {
            //Creating the Root and bpe node
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            if (!string.IsNullOrEmpty(SessionUserInfo))
            {
                #region NLog
                logger.Debug("If User Session Info is not null."); 
                #endregion

                XDocUserInfo = m_ObjCommonUI.loadXmlFile(SessionUserInfo);
            }
            else
            {
                if (xmlPath.Length > 0)
                {
                    XDocUserInfo = m_ObjCommonUI.loadXmlFile(xmlPath[0].ToString());
                }
            }
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            nodeRoot.InnerXml = strBPE;
            //Creating the bpinfo node
            nodeRoot.InnerXml += requestXML;
            XmlNode nodePageSize = nodeRoot.SelectSingleNode("bpinfo//Pagesize");
            if (nodePageSize.InnerText != null && !string.IsNullOrEmpty(pageSize))
            {
                nodePageSize.InnerXml = pageSize;
            }
            if (pageSize == "-1")
            {
                XmlNode nodePageNo = nodeRoot.SelectSingleNode("bpinfo//Pagenumber");
                nodePageNo.InnerXml = "";
            }
            return nodeRoot.OuterXml;
        }

        public void GenerateEXCEL(XmlDocument xDocOut, params string[] arrSelectedColumns)
        {
            DataTable NotesDT = new DataTable();
            DataTable dt = new DataTable();
            DataTable pNotesDT = new DataTable();
            ArrayList arAllColumns = new ArrayList();
            Hashtable htColCaptions = new Hashtable();
            XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string m_treeNodeName = string.Empty;
            if (nodeTreenode.ToString() != null)
            {
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    m_treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
            }
            //newly added on 13-11-09
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");
            {
                for (int col = 0; col < arrSelectedColumns.Length; col++)
                {
                    XmlNode xnode = null;
                    if (m_CustReptHidden == 0)
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@FullViewLength!='0' and @Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    else
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    if (xnode != null)
                    {
                        XmlAttributeCollection xmlattr = xnode.Attributes;
                        if ((xmlattr["ControlType"] != null))
                        {
                            if (!string.IsNullOrEmpty(xmlattr["Caption"].Value.ToString()))
                            {
                                if (!htColCaptions.Contains(xmlattr["Caption"].Value.ToString()))
                                {
                                    htColCaptions.Add(xmlattr["Caption"].Value, xmlattr["Caption"].Value);
                                }
                            }
                        }
                    }
                }
            }
            //newly added on 13-11-09
            //Get the Datatable to print
            dt = GetDataToPrint(xDocOut, m_treeNodeName, "EXCEL");
            Hashtable htAllColumns = new Hashtable();
            if (dt.Rows.Count > 0)
            {
                //Removing TrxID column
                if (dt.Columns.Contains("TrxID"))
                {
                    dt.Columns.Remove("TrxID");
                }
                if (dt.Columns.Contains("Notes"))
                {
                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    dt.Columns.Remove("Notes");
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    arAllColumns.Add(dt.Columns[i].ColumnName);
                }
                //NEED TO REMOVE NOT SELECTED COLUMNS
                if (htColCaptions.Count > 0)
                {
                    IDictionaryEnumerator enumerator = htColCaptions.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        arAllColumns.Remove(enumerator.Key.ToString());
                    }
                    for (int i = 0; i < arAllColumns.Count; i++)
                    {
                        dt.Columns.Remove(arAllColumns[i].ToString());
                    }
                }
                //Export to Excel
                ExportDatatable(dt, "ExcelRpt", NotesDT, "EXCEL");
            }
        }

        public void ExportDatatable(DataTable dt, string fileName, DataTable NotesDT, string exportType)
        {
            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string filename = fileName;
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "";
            switch (exportType)
            {
                case "EXCEL":
                    {

                        response.ContentType = "application/vnd.ms-excel";
                        response.ContentType = "application/ms-excel";
                        response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));
                        break;
                    }
                case "WORD":
                    {
                        response.ContentType = "application/vnd.ms-word";
                        response.ContentType = "application/ms-word";
                        response.AddHeader("content-disposition", string.Format("attachment;filename={0}.doc", fileName));
                        break;
                    }
            }
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    GridView dv = new GridView();
                    dv.DataSource = dt;
                    dv.DataBind();
                    dv.RenderControl(htw);
                    response.Write(style);
                    response.Write(sw.ToString());
                }
            }
            response.Flush();
            response.Close();
        }

        /// <summary>
        /// Gets the DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public DataTable GetDataToPrint(XmlDocument xDocOut, string treeNodeName, string rptType)
        {
            DataTable dt = new DataTable();
            //Getting the dataset to be printed
            XmlNode nodeRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
            if (nodeRows != null)
            {
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                DataSet dsRows = new DataSet();
                dsRows.ReadXml(read);

                //Getting the datatable                
                dt = dsRows.Tables[0];

                int rowCount = dt.Rows.Count;

                //Getting the columns to be displayed in grid
                XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
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
                        //else
                        //{
                        //    //Remove the column if FullViewLength is zero.
                        //    if (dt.Columns.Contains(currentLabel))
                        //    {
                        //        dt.Columns.Remove(currentLabel);
                        //        continue;
                        //    }
                        //}
                    }

                    //Set the column order in the DataTable(RowsList) as per the Columns Node
                    DataColumn dc = dt.Columns[currentLabel];
                    if (dc != null && dc.Ordinal != colPos)
                    {
                        if (colPos >= dt.Columns.Count)
                        {
                            colPos = dt.Columns.Count - 1;//Put it at the last
                        }
                        dc.SetOrdinal(colPos);
                    }
                    colPos++;
                }

                //Removing unwanted columns and setting the captions as per the XML.
                for (int index = 0; index < dt.Columns.Count; index++)
                {
                    bool isAmount = false;
                    bool sumExists = false;
                    bool isChkBx = false;
                    bool isDateTime = false;//DateTime column
                    string colName = dt.Columns[index].ColumnName;
                    if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
                    {
                        if (colName.Trim() != "Notes" && colName.Trim() != "TrxID")
                        {
                            dt.Columns.Remove(colName);
                            index--;
                        }
                    }
                    else//Column is present
                    {
                        XmlNode nodeCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName + "']");
                        //Checking for isSummed value for that column
                        if (nodeCol != null)
                        {
                            //Checking for IsSummed attribute
                            XmlAttribute attIsSummed = nodeCol.Attributes["IsSummed"];
                            if (attIsSummed != null)
                            {
                                if (attIsSummed.Value == "1")
                                {
                                    sumExists = true;
                                }
                            }

                            XmlAttribute attControlType = nodeCol.Attributes["ControlType"];
                            if (attControlType != null)
                            {
                                switch (attControlType.Value)
                                {
                                    case "Check":
                                        {
                                            isChkBx = true;
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            isAmount = true;
                                            break;
                                        }
                                    case "Cal":
                                        {
                                            isDateTime = true;
                                            break;
                                        }
                                    default:
                                        break;
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
                                if (m_htGVColumns.ContainsKey(dt.Columns[index].ColumnName))
                                {
                                    //Test code
                                    if ((dRow[index].ToString() != dRow[dt.Columns[index]].ToString())
                                        || (dRow[index].ToString() != dRow[dt.Columns[index].ColumnName].ToString()))
                                    {
                                        throw new Exception("DataRow Array referenced via index and via Column Name are not matching!!");
                                    }
                                    //Test code ends here..

                                    if (dRow[index].ToString() != string.Empty)
                                    {
                                        string currentDataItem = dRow[index].ToString();
                                        if (colFVL != 0)
                                        {
                                            //Formatting Amount data fields
                                            if (isAmount)
                                            {
                                                decimal amount;
                                                if (Decimal.TryParse(currentDataItem, out amount))
                                                {
                                                    dRow[index] = string.Format("{0:N}", amount);
                                                }
                                            }
                                            else if (isDateTime)//Formatting Date field 
                                            {
                                                DateTime dateTime;
                                                if (DateTime.TryParse(currentDataItem, out dateTime))
                                                {
                                                    dRow[index] = dateTime.ToString("MM/dd/yyyy");
                                                }
                                            }

                                            //Truncating the data if it is greater than its FullView Length
                                            int datavalLength = dRow[index].ToString().Length;//Get the updated data item after formatting is done.
                                            if (datavalLength > colFVL)
                                            {
                                                string dotString = "...";
                                                if (rptType != "EXCEL")//No need of truncation in Excel as fields can be expanded as required.
                                                {
                                                    dRow[index] = currentDataItem.Remove(colFVL - dotString.Length) + dotString;
                                                }
                                            }
                                        }
                                        if (isChkBx)
                                        {
                                            if (currentDataItem == "1")
                                            {
                                                dRow[index] = "x";//"&radic;";
                                                //dRow[index] =;
                                            }
                                            else if (currentDataItem == "0")
                                            {
                                                dRow[index] = "";
                                            }
                                        }

                                        //Summing the values in each row for that column
                                        if (sumExists)
                                        {
                                            sum = sum + Convert.ToDecimal(currentDataItem);
                                        }
                                    }
                                }
                            }

                            //Adding the row to dt if isSummed exists and updating the row count
                            if (sumExists)
                            {
                                if (dt.Rows.Count == rowCount)
                                {
                                    DataRow dr = dt.NewRow();
                                    dt.Rows.Add(dr);
                                }
                                dt.Rows[dt.Rows.Count - 1][index] = sum;
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
                for (int rowCnt = 0; rowCnt < dt.Rows.Count; rowCnt++)
                {
                    bool emptyRow = true;
                    for (int colCnt = 0; colCnt < dt.Columns.Count; colCnt++)
                    {
                        if (dt.Columns[colCnt].ColumnName.ToUpper().Trim() != "TRXID")
                        {
                            if (dt.Rows[rowCnt][colCnt].ToString() != string.Empty)
                            {
                                emptyRow = false;
                                break;
                            }
                        }
                    }
                    if (emptyRow)
                    {
                        dt.Rows[rowCnt].Delete();
                        rowCnt--;
                    }
                }
            }

            return dt;
        }

        ///// <summary>
        ///// Gets the DataTable to be printed
        ///// </summary>
        ///// <param name=""></param>
        ///// <param name=""></param>
        //public DataTable GetDataToPrint(XmlDocument xDocOut, string treeNodeName, string rptType)
        //{
        //    DataTable dt = new DataTable();
        //    //Getting the dataset to be printed
        //    XmlNode nodeRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
        //    if (nodeRows != null)
        //    {
        //        XmlNodeReader read = new XmlNodeReader(nodeRows);
        //        DataSet dsRows = new DataSet();
        //        dsRows.ReadXml(read);

        //        //Getting the datatable                
        //        dt = dsRows.Tables[0];

        //        int rowCnt = dt.Rows.Count;

        //        //Getting the columns to be displayed in grid
        //        XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
        //        m_htGVColumns.Clear();
        //        int colPos = 0;
        //        //Storing the columns names and captions in the HashTable
        //        foreach (XmlNode node in nodeCols)
        //        {
        //            string currentLabel = node.Attributes["Label"].Value;
        //            if (!m_htGVColumns.Contains(currentLabel))
        //            {
        //                if (node.Attributes["FullViewLength"].Value != "0")
        //                {
        //                    m_htGVColumns.Add(currentLabel, node.Attributes["Caption"].Value);
        //                }
        //            }

        //            //Set the column order in the DataTable(RowsList) as per the Columns Node
        //            DataColumn dc = dt.Columns[currentLabel];
        //            if (dc != null && dc.Ordinal != colPos)
        //            {
        //                int tableColPos = colPos;
        //                if (tableColPos >= dt.Columns.Count)
        //                {
        //                    tableColPos = dt.Columns.Count - 1;//Put it at the last
        //                }
        //                dc.SetOrdinal(tableColPos);
        //            }
        //            colPos++;
        //        }

        //        //Removing unwanted columns and setting the captions as per the XML.
        //        for (int index = 0; index < dt.Columns.Count; index++)
        //        {
        //            bool numericExists = false;
        //            bool sumExists = false;
        //            bool chkbx = false;
        //            bool cal = false;//DateTime column
        //            string colName = dt.Columns[index].ColumnName;
        //            if (!(m_htGVColumns.ContainsKey(colName)))//Column not present(FullViewLength=0)
        //            {
        //                if (colName.Trim() != "Notes" && colName.Trim() != "TrxID")
        //                {
        //                    dt.Columns.Remove(colName);
        //                    index--;
        //                }
        //            }
        //            else//Present..Set the caption
        //            {
        //                XmlNode nodeCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dt.Columns[index].ColumnName.Trim().ToString() + "']");
        //                //Checking for isSummed value for that column
        //                if (nodeCol != null)
        //                {
        //                    //Checking for IsNumeric attribute and Keeping commas
        //                    //ControlType="Amount"
        //                    //if (nodeCol.Attributes["IsNumeric"] != null)
        //                    //{
        //                    //    if (nodeCol.Attributes["IsNumeric"].Value == "1")
        //                    //    {
        //                    //        numericExists = true;
        //                    //    }
        //                    //}

        //                    //Checking for IsSummed attribute
        //                    if (!sumExists)
        //                    {
        //                        if (nodeCol.Attributes["IsSummed"] != null)
        //                        {
        //                            if (nodeCol.Attributes["IsSummed"].Value == "1")
        //                            {
        //                                sumExists = true;
        //                            }
        //                        }
        //                    }

        //                    if (nodeCol.Attributes["ControlType"] != null)
        //                    {
        //                        if (nodeCol.Attributes["ControlType"].Value == "Check")
        //                        {
        //                            chkbx = true;
        //                        }
        //                        else if (nodeCol.Attributes["ControlType"].Value == "Amount")
        //                        {
        //                            numericExists = true;
        //                        }
        //                        else if (nodeCol.Attributes["ControlType"].Value == "Cal")
        //                        {
        //                            cal = true;
        //                        }
        //                    }

        //                    int colFVL = 0;
        //                    if (nodeCol.Attributes["FullViewLength"] != null)
        //                    {
        //                        colFVL = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value);
        //                    }

        //                    decimal sum = 0;

        //                    //Iterating through each row and truncating the data and summing value 
        //                    foreach (DataRow dRow in dt.Rows)
        //                    {
        //                        if (m_htGVColumns.ContainsValue(dt.Columns[index].ColumnName.Trim().ToString()))
        //                        {
        //                            if (dRow[dt.Columns[index]].ToString() != string.Empty)
        //                            {
        //                                //Truncating the data if it is greater than its FullView Length
        //                                int datavalLength = dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length;
        //                                if (colFVL != 0)
        //                                {
        //                                    if (dRow[dt.Columns[index].ColumnName.Trim()].ToString().Length > colFVL)
        //                                    {
        //                                        //Keeping commas if IsNumeric ="1"
        //                                        if (numericExists)
        //                                        {
        //                                            decimal amount;
        //                                            if (Decimal.TryParse(dRow[dt.Columns[index]].ToString(), out amount))
        //                                            {
        //                                                dRow[dt.Columns[index]] = string.Format("{0:N}", amount);
        //                                            }
        //                                        }
        //                                        //Formatting Date field 
        //                                        if (cal)
        //                                        {
        //                                            DateTime dateTime;
        //                                            if (DateTime.TryParse(dRow[dt.Columns[index]].ToString(), out dateTime))
        //                                            {
        //                                                dRow[dt.Columns[index]] = dateTime.ToString("MM/dd/yyyy");
        //                                            }
        //                                        }

        //                                        if (rptType.ToString().ToUpper().Trim() != "EXCEL")
        //                                        {
        //                                            if (dRow[dt.Columns[index]].ToString().Trim().Length > colFVL)
        //                                            {
        //                                                dRow[dt.Columns[index]] = dRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                if (chkbx)
        //                                {
        //                                    if (dRow[dt.Columns[index]].ToString() == "1")
        //                                    {
        //                                        dRow[dt.Columns[index]] = "x";//"&radic;";
        //                                    }
        //                                    else if (dRow[dt.Columns[index]].ToString() == "0")
        //                                    {
        //                                        dRow[dt.Columns[index]] = "";
        //                                    }
        //                                }

        //                                //Summing the values in each row for that column
        //                                if (sumExists)
        //                                {
        //                                    sum = sum + Convert.ToDecimal(dRow[dt.Columns[index]]);
        //                                }

        //                            }
        //                        }
        //                    }

        //                    //Adding the row to dt if isSummed exists and updating the row count
        //                    if (sumExists)
        //                    {
        //                        if (dt.Rows.Count == rowCnt)
        //                        {
        //                            DataRow dr = dt.NewRow();
        //                            dt.Rows.Add(dr);
        //                        }
        //                        dt.Rows[dt.Rows.Count - 1][dt.Columns[index]] = sum;
        //                    }
        //                }
        //                //Changing the column name to caption 
        //                dt.Columns[index].ColumnName = m_htGVColumns[colName].ToString();
        //            }
        //        }
        //        if (dt.Columns.Contains("TrxID"))
        //        {
        //            dt.Columns["TrxID"].SetOrdinal(dt.Columns.Count - 1);
        //        }

        //        //Removing empty rows
        //        for (int rwCnt = 0; rwCnt < dt.Rows.Count; rwCnt++)
        //        {
        //            bool emptyRow = true;
        //            for (int colcnt = 0; colcnt < dt.Columns.Count; colcnt++)
        //            {
        //                if (dt.Columns[colcnt].ColumnName.ToUpper().Trim() != "TRXID")
        //                {
        //                    if (dt.Rows[rwCnt][colcnt].ToString() != string.Empty)
        //                        emptyRow = false;
        //                }
        //            }
        //            if (emptyRow)
        //            {
        //                dt.Rows[rwCnt].Delete();
        //                rwCnt--;
        //            }
        //        }
        //    }

        //    return dt;
        //}

        /// <summary>
        /// Gets the Branch DataTable to be printed
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public DataTable GetBranchDataToPrint(XmlDocument xDocOut, string parentTrxID, string branchNodeName)
        {
            DataTable dt = new DataTable();

            //Get the Grid Layout nodes
            string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNodeList nodeResRows = xDocOut.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");

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
                //XmlNode nodeRowsList = branchDoc.SelectSingleNode("Root/RowList");
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
                    XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
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

                            //}

                            //if (dt.Columns.Contains(colName))
                            //{
                            XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Label = '" + dt.Columns[index].ColumnName + "']");
                            //Checking for isSummed value for that column
                            if (nodeBranchCol != null)
                            {
                                //Checking for IsNumeric attribute and Keeping commas
                                //if (nodeBranchCol.Attributes["IsNumeric"] != null)
                                //{
                                //    if (nodeBranchCol.Attributes["IsNumeric"].Value == "1")
                                //    {
                                //        numericExists = true;
                                //    }
                                //}

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
                                        ////Formatting Date field 
                                        //if (commonObjUI.IsDate(dBranchRow[dt.Columns[index]].ToString()))
                                        //{
                                        //    //if the value is IsDate then change format MM/DD/YYYY
                                        //    DateTime date = new DateTime();
                                        //    date = Convert.ToDateTime(dBranchRow[dt.Columns[index]].ToString());
                                        //    string FinalDate = date.ToString("MM/dd/yyyy");
                                        //    dBranchRow[dt.Columns[index]] = FinalDate;
                                        //}

                                        //Truncating the data if it is greater than its FullView Length
                                        int datavalLength = dBranchRow[dt.Columns[index].ColumnName].ToString().Length;
                                        if (dBranchRow[dt.Columns[index].ColumnName].ToString().Length > colFVL)
                                        {
                                            //if (dBranchRow[dt.Columns[index]].ToString().Length > colFVL)
                                            {
                                                dBranchRow[dt.Columns[index]] = dBranchRow[dt.Columns[index]].ToString().Remove(colFVL - 3) + "...";
                                            }
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

        ///// <summary>
        ///// Gets the Header DataTable to be printed
        ///// </summary>
        ///// <param name="Title">Title of the report</param>
        ///// <param name=""></param>
        ///// <returns>DataTable.</returns
        //public DataTable GetHeaderDT(string title, string subTitle)
        //{
        //    //Header table
        //    //Adding 3 columns
        //    DataTable HeaderDT = new DataTable();
        //    for(int col = 0; col < 4; col++)
        //    {
        //        DataColumn colNames = new DataColumn();
        //        colNames.ColumnName = "";
        //        HeaderDT.Columns.Add(colNames);
        //    }
        //    //Adding Header row's 
        //    //DataRow dHeaderPageNoRow = HeaderDT.NewRow();//PageNo Row
        //    //dHeaderPageNoRow[0] = string.Empty;
        //    //dHeaderPageNoRow[1] = string.Empty;
        //    //dHeaderPageNoRow[2] = "Page: ";//+ pageNumber + "Of" + totalPageNumber;
        //    //HeaderDT.Rows.Add(dHeaderPageNoRow);

        //    DataRow dHeaderRow = HeaderDT.NewRow();//Date Row
        //    string userName = string.Empty;
        //    XDocUserInfo = m_ObjCommonUI.loadXmlFile(SessionUserInfo);
        //    XmlNode nodeSmallName = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/SmallName");
        //    if(nodeSmallName != null)
        //    {
        //        userName = nodeSmallName.InnerText.Trim().ToString().ToUpper();
        //    }

        //    dHeaderRow[0] = "Requested By : " + userName;

        //    if(title != null)
        //    {
        //        dHeaderRow[1] = title;
        //    }
        //    else
        //    {
        //        dHeaderRow[1] = string.Empty;
        //    }
        //    dHeaderRow[2] = "Run Date: " + DateTime.Today.ToShortDateString();
        //    HeaderDT.Rows.Add(dHeaderRow);

        //    DataRow dHeaderTimeRow = HeaderDT.NewRow();//Time Row
        //    dHeaderTimeRow[0] = string.Empty;
        //    if(subTitle != null)
        //    {
        //        dHeaderTimeRow[1] = subTitle;
        //    }
        //    else
        //    {
        //        dHeaderTimeRow[1] = string.Empty;
        //    }
        //    dHeaderTimeRow[2] = "Run Time: " + DateTime.Now.ToLongTimeString();
        //    HeaderDT.Rows.Add(dHeaderTimeRow);

        //    return HeaderDT;
        //}

        public DataTable GetHeaderDT(string title, string subTitle)
        {

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

            string userName = string.Empty;

            XDocUserInfo = m_ObjCommonUI.loadXmlFile(SessionUserInfo);

            //

            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            XmlNode nodeSmallName = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/SmallName");

            if (nodeSmallName != null)
            {

                userName = nodeSmallName.InnerText.Trim().ToString().ToUpper();

            }

            dCompanyRow[0] = "Requested By : " + userName;

            dCompanyRow[1] = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;

            dCompanyRow[2] = "Date Time: " + DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();

            //

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

        #region Number To Words

        public string ConvertNumberToWord(long nNumber)
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
        }

        #endregion

        public void GeneratePDF(XmlDocument xDocOut, params string[] arrSelectedColumns)
        {
            try
            {
                XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                clsReportsUI objReportsUI = new clsReportsUI();
                string treeNodeName = nodeTreenode.FirstChild.InnerText;
                int isPrint = Convert.ToInt32(xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value);
                if (isPrint == 1)
                {
                    XmlNode xReportStyleNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree");
                    if (nodeTreenode.Attributes["ReportStyle"] == null)
                    {
                        XmlAttribute attrReportStyle = xDocOut.CreateAttribute("ReportStyle");
                        xReportStyleNode.Attributes.Append(attrReportStyle);
                        //Check for the web.config key as the final override
                        if (ConfigurationManager.AppSettings["PrintBranchData"] == "0")
                        {
                            attrReportStyle.Value = "1";
                        }
                        else
                        {
                            if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[@ControlType='GView']") != null)
                            {
                                attrReportStyle.Value = "5";
                            }
                            else
                            {
                                attrReportStyle.Value = "1";
                            }
                        }
                    }
                    else
                    {
                        #region NLog
                        logger.Debug("If report style was not specified."); 
                        #endregion

                        if (ConfigurationManager.AppSettings["PrintBranchData"] == "0")
                        {
                            nodeTreenode.Attributes["ReportStyle"].Value = "1";
                        }
                    }
                }
                objReportsUI.GenerateReport(xDocOut, "PDF", arrSelectedColumns);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        #region Export To PDF
        /// <summary>
        /// Exports the grid view to PDF
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="dt">Filename to be printed</param>
        public void GVExportToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {
            try
            {
                bool dataPrinted = false;
                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                int pageCnt = 0;
                Hashtable m_htPagebrk = new Hashtable();
                Hashtable m_htRightAlign = new Hashtable();
                Hashtable m_htDateFormat = new Hashtable();
                string m_AmountNodes = string.Empty;
                string m_DateFormats = string.Empty;
                //Storing the fullview length of cols in hashtable                    
                XmlNode nodeGridLayout = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                foreach (XmlNode nodetreenode in nodeGridLayout.ChildNodes)
                {
                    string treeNodeName = nodetreenode.SelectSingleNode("Node").InnerText;
                    dt = GetDataToPrint(xDocOut, treeNodeName, "");
                    clsReportsUI objReportsUI = new clsReportsUI();
                    dt = objReportsUI.ChangeDateAndAmountFormats(dt, xDocOut.OuterXml.ToString(), treeNodeName);
                    if (dt.Rows.Count > 0)
                    {
                        if (!dataPrinted)
                            dataPrinted = true;
                        if (dt.Columns.Contains("Notes"))
                        {
                            NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            dt.Columns.Remove("Notes");
                        }
                        //Getting the columns to be displayed in grid
                        XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                        //Getting the rows to print
                        XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                        int pageBreaks = 0;
                        //Considering the PLayout only for the first treenode
                        if (myPdfDocument.PageCount == 0)
                        {
                            if (nodeRowList != null)
                            {
                                if (nodeRowList.FirstChild != null)
                                {
                                    if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
                                    {
                                        string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
                                        if (pLayout == "1")//Landscape
                                        {
                                            myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                                            //myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);                    
                                        }
                                    }
                                }
                            }
                        }
                        if (nodeRowList != null)
                        {
                            if (nodeRowList.ChildNodes != null)
                            {
                                foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
                                {
                                    if (nodeRow.Attributes["TrxID"] != null)
                                    {
                                        string trxID = nodeRow.Attributes["TrxID"].Value;
                                        DataRow[] foundRows;
                                        foundRows = dt.Select("TrxID ='" + trxID + "'");

                                        if (foundRows.Length > 0)
                                        {
                                            int rowIndex = dt.Rows.IndexOf(foundRows[0]);

                                            if (nodeRow.Attributes["pLnSkip"] != null)
                                            {
                                                if (nodeRow.Attributes["pLnSkip"].Value.ToString().Trim() != "0")
                                                {
                                                    for (int skipCnt = 0; skipCnt < Convert.ToInt32(nodeRow.Attributes["pLnSkip"].Value.Trim()); skipCnt++)
                                                    {
                                                        // Adding each row at a time
                                                        DataRow dSkipRow = dt.NewRow();
                                                        for (int col = 0; col < dt.Columns.Count; col++)
                                                        {
                                                            dSkipRow[dt.Columns[col].ColumnName] = "SKIP";
                                                        }
                                                        dt.Rows.InsertAt(dSkipRow, rowIndex + 1);
                                                        dt.AcceptChanges();
                                                    }
                                                }
                                            }

                                            if (nodeRow.Attributes["pPgBreak"] != null)
                                            {
                                                if (nodeRow.Attributes["pPgBreak"].Value.ToString().Trim() == "1")
                                                {
                                                    pageBreaks++;
                                                    m_htPagebrk.Add(pageBreaks, rowIndex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        int colsInTab = dt.Columns.Count - 1;
                        int[] Arraywidth = new int[colsInTab];
                        bool isSumExists = false;
                        if (tableLayout.ToUpper().Trim() == "NORMAL")
                        {
                            foreach (XmlNode node in nodeCols)
                            {
                                if (node.Attributes["Caption"] != null)
                                {
                                    DataColumn dc = dt.Columns[node.Attributes["Caption"].Value];
                                    if (dc != null)
                                    {
                                        //Set the column width based on FVL
                                        if (node.Attributes["FullViewLength"] != null)
                                        {
                                            if (node.Attributes["FullViewLength"].Value != "0")
                                            {
                                                Arraywidth[dc.Ordinal] = Convert.ToInt32(node.Attributes["FullViewLength"].Value);
                                            }
                                            else
                                            {
                                                Arraywidth[dc.Ordinal] = 15;
                                            }
                                        }
                                        //Getting the cols having Issummed=1 
                                        if (node.Attributes["IsSummed"] != null)
                                        {
                                            if (node.Attributes["IsSummed"].Value == "1")
                                            {
                                                if (!isSumExists)
                                                    isSumExists = true;
                                                if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                                    m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["IsSummed"].Value);
                                            }
                                        }
                                        //Getting the cols having ControlType="Amount"//Isnumeric=1
                                        if (node.Attributes["ControlType"] != null)
                                        {
                                            if (node.Attributes["ControlType"].Value == "Cal")
                                            {
                                                m_DateFormats = node.Attributes["Caption"].Value;
                                                if (!m_htDateFormat.Contains(node.Attributes["Caption"].Value))
                                                {
                                                    m_htDateFormat.Add(node.Attributes["Caption"].Value, node.Attributes["ControlType"].Value);
                                                }
                                            }
                                            if (node.Attributes["ControlType"].Value == "Amount")
                                            {
                                                m_AmountNodes = node.Attributes["Caption"].Value;
                                                if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                                {
                                                    m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["ControlType"].Value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Setting columns width for cols not present in node columns
                            for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                            {
                                if (Arraywidth[colCnt].ToString() == string.Empty)
                                    Arraywidth[colCnt] = 15;
                            }
                        }
                        else
                        {
                            #region NLog
                            logger.Debug("Setting the column width based on no of columns."); 
                            #endregion

                            //Setting columns width based on No of cols
                            for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                            {
                                Arraywidth[colCnt] = 20;
                            }
                        }
                        //Setting the X and Y positons, width and height of the tables
                        double posX = 20;
                        double posY = 70;
                        double width = myPdfDocument.PageWidth - 50;
                        double height = myPdfDocument.PageHeight - 50;
                        double currentYPos = 70;
                        string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                        string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";
                        PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
                        Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
                        Font HeaderFont = new Font("Verdana", 8, FontStyle.Bold | FontStyle.Underline);
                        Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
                        Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
                        Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
                        Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
                        Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
                        Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                        Font RowFontBold = new Font("Verdana", 8, FontStyle.Bold);
                        Font FontUnderline = new Font("Verdana", 8, FontStyle.Regular | FontStyle.Underline);
                        Font RowBoxFontBold = new Font("Verdana", 9, FontStyle.Bold);
                        //Getting Header table
                        string title = string.Empty;
                        XmlNode nodeTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                        if (nodeTitle != null)
                        {
                            title = nodeTitle.InnerText.Trim().ToString();
                        }
                        string subTitle = string.Empty;
                        XmlNode nodeSubTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
                        if (nodeSubTitle != null)
                        {
                            subTitle = nodeSubTitle.InnerText.Trim().ToString();
                        }
                        //Getting header DT for this treenode
                        DataTable HeaderDT = GetHeaderDT(title, subTitle);
                        PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 2);
                        //Import HeaderDT to PDF table
                        myHeaderPdfTable.ImportDataTable(HeaderDT);
                        myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
                        myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
                        myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
                        myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                        myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 155, 55 });
                        int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                        myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                        if (titleLength < 75)
                        {
                            myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
                        int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                        if (subTitleLength < 75)
                        {
                            myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle             
                        if (myHeaderPdfTable.Rows.Length > 2)
                        {
                            int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                            if (dateLength < 75)
                            {
                                myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            }
                            myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
                        }
                        PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));

                        double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
                        double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

                        //Adding New page for the first time
                        PdfPage newPdfPage = myPdfDocument.NewPage();
                        //Adding header table in the first page
                        newPdfPage.Add(myHeaderPdfTablePage);
                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));

                        if (pageBreaks != 0)
                        {
                            for (int pgbrk = 1; pgbrk <= pageBreaks + 1; pgbrk++)
                            {
                                posY = 70;
                                currentYPos = 70;
                                //if (m_htPagebrk.Contains(pgbrk))
                                {
                                    int dtStRow = 0;
                                    int dtEndRow = 0;

                                    if (pgbrk == 1)
                                    {
                                        dtStRow = 0;
                                    }
                                    else
                                    {
                                        dtStRow = Convert.ToInt32(m_htPagebrk[pgbrk - 1].ToString()) + 1;
                                    }
                                    if (pgbrk == pageBreaks + 1)
                                    {
                                        dtEndRow = dt.Rows.Count - 1;
                                    }
                                    else
                                    {
                                        dtEndRow = Convert.ToInt32(m_htPagebrk[pgbrk].ToString());
                                    }

                                    int rowsInTab = dtEndRow - dtStRow + 1;//dt.Rows.Count;
                                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                                    //PdfPage newPdfPage = myPdfDocument.NewPage();

                                    //Import DT to PDF table
                                    dt = objReportsUI.WrapFullViewLength(dt, Arraywidth);
                                    myPdfTable.ImportDataTable(dt, 0, 0, dtStRow, dtEndRow);
                                    myPdfTable.SetBorders(Color.Black, 1, BorderType.None);

                                    myPdfTable.SetColumnsWidth(Arraywidth);
                                    //Now we set some alignment... for the whole table and then, for a column:
                                    myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfTable.HeadersRow.SetFont(HeaderFont);
                                    myPdfTable.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                                    myPdfTable.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                                    if (m_htRightAlign.Count > 0)
                                    {
                                        if (pgbrk == pageBreaks + 1)
                                        {
                                            if (isSumExists)
                                            {
                                                //Right justifying Summed row content
                                                myPdfTable.Rows[rowsInTab - 1].SetFont(SumRowFont);
                                                myPdfTable.Rows[rowsInTab - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                        //Right justifying Summed column content and IsNumeric column content
                                        for (int pdfcol = 0; pdfcol < colsInTab; pdfcol++)
                                        {
                                            //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                            if (myPdfTable.HeadersRow[pdfcol].Content != null)
                                            {
                                                if (myPdfTable.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                                {
                                                    if (m_htRightAlign.Contains(myPdfTable.HeadersRow[pdfcol].Content.ToString()))
                                                    {
                                                        myPdfTable.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                        myPdfTable.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    //Setting Row font
                                    //for (int pdfRow = 0; pdfRow < rowsInTab; pdfRow++)
                                    int pdftableRow = 0;
                                    for (int pdfRow = dtStRow; pdfRow <= dtEndRow; pdfRow++)
                                    {
                                        string trxID = dt.Rows[pdfRow]["TrxID"].ToString();
                                        // XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@Caption = '" + dBranchCol.ColumnName + "']");
                                        XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID = '" + trxID + "']");
                                        //XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@" + firstColLabel + " = '" + myPdfTable.Rows[pdfRow][0].Content.ToString() + "']");
                                        if (nodeRow != null)
                                        {
                                            if (nodeRow.Attributes["pFont"] != null)
                                            {
                                                if (nodeRow.Attributes["pFont"].Value.ToString().Trim() == "1")
                                                {
                                                    myPdfTable.Rows[pdftableRow].SetFont(RowFontBold);
                                                }
                                            }
                                            if (nodeRow.Attributes["pBox"] != null)
                                            {
                                                if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "1")
                                                {
                                                    myPdfTable.Rows[pdftableRow].SetFont(RowBoxFontBold);
                                                }
                                            }
                                        }
                                        if (trxID.ToUpper().Trim() == "SKIP")
                                        {
                                            myPdfTable.Rows[pdftableRow].SetForegroundColor(Color.White);
                                        }
                                        pdftableRow++;
                                    }

                                    //
                                    if (pgbrk != 1)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo
                                        newPdfPage = myPdfDocument.NewPage();
                                        newPdfPage.Add(myHeaderPdfTablePage);
                                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                    }

                                    while (!myPdfTable.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            posY = currentYPos;// +25;
                                        }

                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }

                                        PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                        //Printing Row boxes
                                        for (int index = newPdfTablePage.FirstRow; index <= newPdfTablePage.LastRow; index++)
                                        {
                                            string trxID = dt.Rows[index]["TrxID"].ToString();
                                            XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID = '" + trxID + "']");
                                            if (nodeRow != null)
                                            {
                                                if (nodeRow.Attributes["pBox"] != null)
                                                {
                                                    if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "1")
                                                    {
                                                        for (int cellcnt = 0; cellcnt < myPdfTable.Rows[index].Cells.Count; cellcnt++)
                                                        {
                                                            PdfRectangle pr = newPdfTablePage.CellArea(index, cellcnt).ToRectangle(Color.Black, 1, Color.LightGray);
                                                            pr.StrokeWidth = 1;
                                                            newPdfPage.Add(pr);
                                                        }
                                                    }
                                                    if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "2")
                                                    {
                                                        for (int cellcnt = 0; cellcnt < myPdfTable.Rows[index].Cells.Count; cellcnt++)
                                                        {
                                                            if (myPdfTable.Rows[index][cellcnt].Content.ToString() != string.Empty)
                                                            {
                                                                PdfRectangle pr = newPdfTablePage.CellArea(index, cellcnt).ToRectangle(Color.Black, 1, Color.LightGray);
                                                                pr.StrokeWidth = 1;
                                                                newPdfPage.Add(pr);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //Rows content
                                        newPdfPage.Add(newPdfTablePage);
                                        currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int rowsInTab = dt.Rows.Count;
                            PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                            //Import DT to PDF table
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
                                        if (m_ObjCommonUI.IsDate(dr[m_DateFormats].ToString()))
                                        {
                                            //if the value is IsDate then change format MM/DD/YYYY
                                            DateTime.TryParse(dr[m_DateFormats].ToString(), out date);
                                            string dates = date.ToString("MM/dd/yy");
                                            dr[m_DateFormats] = dates;
                                        }
                                    }
                                }
                            }
                            dt = objReportsUI.WrapFullViewLength(dt, Arraywidth);
                            myPdfTable.ImportDataTable(dt, 0, 0, 0, rowsInTab);
                            myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfTable.SetColumnsWidth(Arraywidth);
                            //Now we set some alignment... for the whole table and then, for a column:
                            myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable.HeadersRow.SetFont(HeaderFont);
                            myPdfTable.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                            myPdfTable.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                            if (m_htRightAlign.Count > 0)
                            {
                                if (isSumExists)
                                {
                                    //Right justifying Summed row content
                                    myPdfTable.Rows[dt.Rows.Count - 1].SetFont(SumRowFont);
                                    myPdfTable.Rows[dt.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                }
                                //Right justifying Summed column content and IsNumeric column content
                                for (int pdfcol = 0; pdfcol < colsInTab; pdfcol++)
                                {
                                    //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                    if (myPdfTable.HeadersRow[pdfcol].Content != null)
                                    {
                                        if (myPdfTable.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                        {
                                            if (m_htRightAlign.Contains(myPdfTable.HeadersRow[pdfcol].Content.ToString()))
                                            {
                                                myPdfTable.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfTable.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                    }
                                }
                            }

                            //Setting Row font
                            for (int pdfRow = 0; pdfRow < rowsInTab; pdfRow++)
                            {
                                string trxID = dt.Rows[pdfRow]["TrxID"].ToString();
                                // XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@Caption = '" + dBranchCol.ColumnName + "']");
                                XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID = '" + trxID + "']");
                                //XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@" + firstColLabel + " = '" + myPdfTable.Rows[pdfRow][0].Content.ToString() + "']");
                                if (nodeRow != null)
                                {
                                    if (nodeRow.Attributes["pFont"] != null)
                                    {
                                        if (nodeRow.Attributes["pFont"].Value.ToString().Trim() == "1")
                                        {
                                            myPdfTable.Rows[pdfRow].SetFont(RowFontBold);
                                        }
                                    }
                                    if (nodeRow.Attributes["pBox"] != null)
                                    {
                                        if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "1")
                                        {
                                            myPdfTable.Rows[pdfRow].SetFont(RowBoxFontBold);
                                        }
                                    }
                                }
                                if (trxID.ToUpper().Trim() == "SKIP")
                                {
                                    myPdfTable.Rows[pdfRow].SetForegroundColor(Color.White);
                                }
                            }
                            //
                            while (!myPdfTable.AllTablePagesCreated)
                            {
                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }
                                else
                                {
                                    posY = currentYPos;// +25;
                                }

                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }
                                PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));

                                //Printing Row boxes
                                for (int index = newPdfTablePage.FirstRow; index <= newPdfTablePage.LastRow; index++)
                                {
                                    string trxID = dt.Rows[index]["TrxID"].ToString();
                                    XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID = '" + trxID + "']");
                                    if (nodeRow != null)
                                    {
                                        if (nodeRow.Attributes["pBox"] != null)
                                        {
                                            if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "1")
                                            {
                                                for (int cellcnt = 0; cellcnt < myPdfTable.Rows[index].Cells.Count; cellcnt++)
                                                {
                                                    PdfRectangle pr = newPdfTablePage.CellArea(index, cellcnt).ToRectangle(Color.Black, 1, Color.LightGray);
                                                    pr.StrokeWidth = 1;
                                                    newPdfPage.Add(pr);
                                                }
                                            }
                                            if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "2")
                                            {
                                                for (int cellcnt = 0; cellcnt < myPdfTable.Rows[index].Cells.Count; cellcnt++)
                                                {
                                                    if (myPdfTable.Rows[index][cellcnt].Content.ToString() != string.Empty)
                                                    {
                                                        PdfRectangle pr = newPdfTablePage.CellArea(index, cellcnt).ToRectangle(Color.Black, 1, Color.LightGray);
                                                        pr.StrokeWidth = 1;
                                                        newPdfPage.Add(pr);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //Adding Rows content
                                newPdfPage.Add(newPdfTablePage);
                                currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                            }
                        }//page brk


                        //Notes DT
                        if (NotesDT.Rows.Count > 0)
                        {
                            if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
                            {

                                int rowsInNotesDT = NotesDT.Rows.Count;
                                int colsInNotesDT = NotesDT.Columns.Count;
                                PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
                                myPdfTable1.ImportDataTable(NotesDT);
                                myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
                                myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
                                myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                                myPdfTable1.HeadersRow.SetFont(HeaderFont);
                                myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                                myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                                while (!myPdfTable1.AllTablePagesCreated)
                                {
                                    //Setting the Y position and if required creating new page
                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo
                                        newPdfPage = myPdfDocument.NewPage();
                                        newPdfPage.Add(myHeaderPdfTablePage);
                                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                    }
                                    else
                                    {
                                        posY = currentYPos;// +25;
                                    }

                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                        newPdfPage = myPdfDocument.NewPage();
                                        newPdfPage.Add(myHeaderPdfTablePage);
                                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                    }

                                    PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height - posY - 10));
                                    newPdfPage.Add(newPdfTablePage1);
                                    currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                                }
                            }
                        }
                        newPdfPage.SaveToDocument();
                    }
                }
                if (dataPrinted)
                {
                    SaveToResponse(fileName);
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

        private void SaveToResponse(string fileName)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            if (ShowPDF)
            {
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", fileName));
            }
            else
            {
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", fileName));
            }
            myPdfDocument.SaveToStream(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();

            //SaveToFileSystem(fileName, myPdfDocument);
        }

        private void SaveToFileSystem(string fileName, PdfDocument pDoc)
        {
            string folderPath = ConfigurationManager.AppSettings["ReportsPath"];
            string filePath = folderPath + "\\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            pDoc.SaveToFile(filePath);
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
                clsReportsUI objReportsUI = new clsReportsUI();
                dt = objReportsUI.ChangeDateAndAmountFormats(dt, strOutXml, treeNodeName);
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
                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";

                //PdfPage newPdfPage = myPdfDocument.NewPage();

                //double pageht = myPdfDocument.PageHeight;
                //double pagewidth = myPdfDocument.PageWidth;

                PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
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
                    Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
                    Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
                    Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
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
                        #region NLog
                        logger.Debug("If table layout is nor normal."); 
                        #endregion

                        for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                        {
                            Arraywidth[colCnt] = 20;
                        }
                    }
                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                    //Import DT to PDF table
                    pDT = objReportsUI.WrapFullViewLength(pDT, Arraywidth);
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

                    PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
                    //Import DT to PDF table
                    myHeaderPdfTable.ImportDataTable(HeaderDT);
                    myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
                    myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
                    myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
                    myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 145, 55 });
                    int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                    if (titleLength < 75)
                    {
                        myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
                    int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    if (subTitleLength < 75)
                    {
                        myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle
                    if (myHeaderPdfTable.Rows.Length > 2)
                    {
                        int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                        if (dateLength < 75)
                        {
                            myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
                    }
                    //while (!myHeaderPdfTable.AllTablePagesCreated)
                    {
                        PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));
                        double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
                        double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

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
                                BranchDT = GetBranchDataToPrint(xDoc, parentTrxID, branchNodeName);

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
                        #region NLog
                        logger.Debug("Deleting the printed row from the parent datatable because the  table layout is PIVOT."); 
                        #endregion

                        pDT.Columns.RemoveAt(1);
                        parentDT.Rows[0].Delete();
                    }
                }

                SaveToResponse(fileName);
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

        #region GVPivotParentNewPgExpToPDF

        /// <summary>
        /// Exports the data to PDF
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="dt">Filename to be printed</param>
        private void GVPivotParentNewPgExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {
            try
            {
                DataTable pNotesDT = new DataTable();
                DataTable pDT = new DataTable();
                //Get the treeNodeName
                string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                clsReportsUI objReportsUI = new clsReportsUI();
                dt = objReportsUI.ChangeDateAndAmountFormats(dt, xDocOut.OuterXml.ToString(), treeNodeName);
                //Getting the columns to be displayed in grid
                XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

                //Getting the rows to print
                XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                ////Default portrait pLayout="0"
                ////                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11);
                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);

                //Considering the PLayout only for the first treenode
                if (nodeRowList != null)
                {
                    if (myPdfDocument.PageCount == 0)
                    {
                        if (nodeRowList.FirstChild != null)
                        {
                            if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
                            {
                                string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
                                if (pLayout == "1")//Landscape
                                {
                                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                                    //myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);                    
                                }
                            }
                        }
                    }
                }

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
                }

                //Removing the Summed row from parent table
                if (isSumExists)
                {
                    #region NLog
                    logger.Debug("Removing the summed row from the parent table."); 
                    #endregion

                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

                //PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);
                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";

                //PdfPage newPdfPage = myPdfDocument.NewPage();

                PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
                double posX = 20;//50
                double posY = 70;//90
                double width = myPdfDocument.PageWidth - 50;// 690; 
                double height = myPdfDocument.PageHeight - 50;// 250;
                double currentYPos = 70;
                int pageCnt = 0;

                foreach (DataRow dRow in dt.Rows)
                {
                    PdfPage newPdfPage = myPdfDocument.NewPage();

                    posY = 70;
                    currentYPos = 70;

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
                        dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                    }
                    parentDT.Rows.Add(dNewRow);

                    string parentTrxID = string.Empty;
                    parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                    //Removing TrxID column
                    if (parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Remove("TrxID");
                    }

                    //Pivoting the tables
                    pNotesDT = reportsBO.PivotTable(NotesDT);
                    pDT = reportsBO.PivotTable(parentDT);

                    // Varaible to get the Row and Column count of three tables
                    int rowsInTab = pDT.Rows.Count;
                    int colsInTab = pDT.Columns.Count;
                    int[] Arraywidth = new int[colsInTab];
                    //Setting columns width based on the No of columns
                    for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                    {
                        Arraywidth[colCnt] = 20;
                    }

                    //Font FontBold = new Font("Verdana", 9, FontStyle.Bold);
                    Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
                    Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
                    Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
                    Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                    //Import DT to PDF table
                    pDT = objReportsUI.WrapFullViewLength(pDT, Arraywidth);
                    myPdfTable.ImportDataTable(pDT);
                    //Setting the header row text color as white
                    myPdfTable.HeadersRow.SetForegroundColor(Color.White);
                    //myPdfTable.HeadersRow.SetFont(HeaderFont);
                    myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    myPdfTable.Columns[0].SetFont(HeaderFont);


                    myPdfTable.SetColumnsWidth(Arraywidth);
                    //myPdfTable.SetRowHeight(15);                
                    //Now we set some alignment... for the whole table and then, for a column:
                    myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);

                    //Getting Header table
                    string title = string.Empty;
                    XmlNode nodeTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                    if (nodeTitle != null)
                    {
                        title = nodeTitle.InnerText.Trim().ToString();
                    }
                    string subTitle = string.Empty;
                    XmlNode nodeSubTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
                    if (nodeSubTitle != null)
                    {
                        subTitle = nodeSubTitle.InnerText.Trim().ToString();
                    }
                    DataTable HeaderDT = GetHeaderDT(title, subTitle);

                    PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
                    //Import DT to PDF table
                    myHeaderPdfTable.ImportDataTable(HeaderDT);
                    myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
                    myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
                    myHeaderPdfTable.VisibleHeaders = false;
                    myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
                    myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 145, 55 });
                    int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                    if (titleLength < 75)
                    {
                        myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
                    int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    if (subTitleLength < 75)
                    {
                        myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle
                    if (myHeaderPdfTable.Rows.Length > 2)
                    {
                        int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                        if (dateLength < 75)
                        {
                            myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
                    }
                    PdfArea pdfArea = new PdfArea(myPdfDocument, posX, 70, width, 60);

                    PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));
                    double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
                    double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

                    newPdfPage.Add(myHeaderPdfTablePage);
                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));

                    while (!myPdfTable.AllTablePagesCreated)
                    {
                        //Setting the Y position and if required creating new page
                        if (currentYPos > myPdfDocument.PageHeight - 50)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }
                        else
                        {
                            posY = currentYPos;// +25;
                        }

                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }

                        PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, 300, height));
                        newPdfPage.Add(newPdfTablePage);
                        currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                    }

                    //Notes DT
                    if (NotesDT.Rows.Count > 0)
                    {
                        if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
                        {

                            int rowsInNotesDT = NotesDT.Rows.Count;
                            int colsInNotesDT = NotesDT.Columns.Count;
                            PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
                            myPdfTable1.ImportDataTable(NotesDT);
                            myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
                            myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable1.HeadersRow.SetFont(HeaderFont);
                            myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                            myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                            //Setting the Y position and if required creating new page
                            if (currentYPos > myPdfDocument.PageHeight - 50)
                            {
                                posY = 70;
                                currentYPos = 70;
                                newPdfPage.SaveToDocument();
                                //Adding new page and adding Header table,logo image and pageNo 
                                newPdfPage = myPdfDocument.NewPage();
                                newPdfPage.Add(myHeaderPdfTablePage);
                                //Adding logo
                                newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                            }
                            else
                            {
                                posY = currentYPos + 25;
                            }

                            if (myPdfDocument.PageHeight - 50 - posY < 50)
                            {
                                posY = 70;
                                currentYPos = 70;
                                newPdfPage.SaveToDocument();
                                //Adding new page and adding Header table,logo image and pageNo 
                                newPdfPage = myPdfDocument.NewPage();
                                newPdfPage.Add(myHeaderPdfTablePage);
                                //Adding logo
                                newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                            }

                            while (!myPdfTable1.AllTablePagesCreated)
                            {
                                PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                newPdfPage.Add(newPdfTablePage1);
                                currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                            }
                        }
                    }

                    //Getting the dataset to be bound to the grid.
                    XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                        {
                            //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                            {
                                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                                DataTable BranchDT = new DataTable();
                                BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                                if (BranchDT.Rows.Count > 0)
                                {
                                    Hashtable m_htBranchRightAlign = new Hashtable();
                                    ArrayList m_alCalendarControls = new ArrayList();

                                    Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                                    bool sumExists = false;

                                    int rowsInBranchDT = BranchDT.Rows.Count;
                                    int colsInBranchDT = BranchDT.Columns.Count;

                                    //Getting the columns to be displayed in grid
                                    XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                    int[] BranchArraywidth = new int[colsInBranchDT];


                                    int dcindex = 0;
                                    //int arindex=0;
                                    //Storing the columns names and captions in the HashTable
                                    foreach (DataColumn dBranchCol in BranchDT.Columns)
                                    {
                                        XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");



                                        //Checking for isSummed value for that column
                                        if (nodeBranchCol != null)
                                        {
                                            //Getting the cols having Issummed=1 
                                            if (nodeBranchCol.Attributes["IsSummed"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                                {
                                                    if (!sumExists)
                                                        sumExists = true;
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
                                                }
                                            }
                                            //Getting the cols having ControlType="Amount"//Isnumeric=1
                                            if (nodeBranchCol.Attributes["ControlType"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                                {
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
                                                }
                                                //getting columns having ControlType="Cal"
                                                if (nodeBranchCol.Attributes["ControlType"].Value == "Cal")
                                                {
                                                    m_alCalendarControls.Add(dcindex);
                                                    //myPdfTable.Columns[dcindex].SetContentFormat("{0:MM/dd/yyyy}");
                                                }

                                            }
                                        }

                                        //Setting the column width of branch table
                                        int dcPos = dBranchCol.Ordinal;
                                        int colFVL = 0;
                                        if (nodeBranchCol != null)
                                        {
                                            if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                            {
                                                colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                            }
                                        }
                                        if (colFVL != 0)
                                        {
                                            BranchArraywidth[dcPos] = colFVL;
                                        }
                                        else
                                        {
                                            BranchArraywidth[dcPos] = 15;
                                        }
                                        //increment column index
                                        dcindex++;
                                    }
                                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);

                                    Font myHeaderFont = new Font("Verdana", 10, FontStyle.Bold);

                                    myPdfTable1.ImportDataTable(BranchDT);
                                    myPdfTable1.HeadersRow.SetFont(myHeaderFont);
                                    //myPdfTable1.HeadersRow.SetFont(HeaderFont);

                                    DateTime dateTime;


                                    for (int i = 0; i <= m_alCalendarControls.Count - 1; i++)
                                    {

                                        myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].SetContentFormat("{0:MM/DD/YYYY}");


                                        //if (DateTime.TryParse(((Gios.Pdf.PdfCell)(myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].Cells[0])).Content, out dateTime))
                                        //{
                                        //    ((Gios.Pdf.PdfCell)(myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].Cells[0])).Content = dateTime.ToString("MM/dd/yyyy");
                                        //}
                                        // myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].SetColors(Color.Red, Color.Blue, Color.Gainsboro);
                                        //for (int j = 0; j <= myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].Cells.Cells.Count - 1; j++)
                                        //{
                                        //    myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].Cells[j] = "PSKR" + myPdfTable1.Columns[int.Parse(m_alCalendarControls[i].ToString())].Cells[j];
                                        //    //.SetContentFormat("{0:MM/dd/yyyy}");
                                        //    //.SetContentFormat("{0:MM/dd/yyyy}");
                                        //}
                                    }




                                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    myPdfTable1.SetColumnsWidth(BranchArraywidth);
                                    myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //myPdfTable1.SetRowHeight(15);
                                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //Format




                                    if (m_htBranchRightAlign.Count > 0)
                                    {
                                        if (sumExists)
                                        {
                                            //Right justifying Summed row content
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        //Right justifying Summed column content and IsNumeric column content
                                        for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
                                        {
                                            //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                            if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                            {
                                                if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
                                                {
                                                    myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                    myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                            }
                                        }
                                    }

                                    while (!myPdfTable1.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            posY = currentYPos + 25;
                                        }

                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }

                                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                        newPdfPage.Add(newPdfTablePage1);
                                        currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
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
                        //Adding TrxID column
                        if (!parentDT.Columns.Contains("TrxID"))
                        {
                            parentDT.Columns.Add("TrxID");
                        }
                    }
                }

                SaveToResponse(fileName);
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

        #region GVPivotParentCntPgExpToPDF

        /// <summary>
        /// Exports the data to PDF
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="dt">Filename to be printed</param>
        private void GVPivotParentCntPgExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {
            try
            {
                DataTable pNotesDT = new DataTable();
                DataTable pDT = new DataTable();

                //Get the treeNodeName
                string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                clsReportsUI objReportsUI = new clsReportsUI();
                dt = objReportsUI.ChangeDateAndAmountFormats(dt, xDocOut.OuterXml.ToString(), treeNodeName);
                //Getting the columns to be displayed in grid
                XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                //Getting the rows to print
                XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                ////Default portrait pLayout="0"
                ////                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11);
                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);

                //Considering the PLayout only for the first treenode
                if (nodeRowList != null)
                {
                    if (myPdfDocument.PageCount == 0)
                    {
                        if (nodeRowList.FirstChild != null)
                        {
                            if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
                            {
                                string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
                                if (pLayout == "1")//Landscape
                                {
                                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                                    //myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);                    
                                }
                            }
                        }
                    }
                }

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
                }

                //Removing the Summed row from parent table
                if (isSumExists)
                {
                    #region NLog
                    logger.Debug("Removing the summed row from the parent table."); 
                    #endregion

                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

                //PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);
                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";

                //PdfPage newPdfPage = myPdfDocument.NewPage();

                PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
                double posX = 20;//50
                double posY = 70;//90
                double width = myPdfDocument.PageWidth - 50;// 690; 
                double height = myPdfDocument.PageHeight - 50;// 250;
                double currentYPos = 70;

                //Font FontBold = new Font("Verdana", 9, FontStyle.Bold);
                Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
                Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
                Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
                Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
                Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
                Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
                Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
                int pageCnt = 0;

                //Getting Header table
                string title = string.Empty;
                XmlNode nodeTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                if (nodeTitle != null)
                {
                    title = nodeTitle.InnerText.Trim().ToString();
                }
                string subTitle = string.Empty;
                XmlNode nodeSubTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
                if (nodeSubTitle != null)
                {
                    subTitle = nodeSubTitle.InnerText.Trim().ToString();
                }
                DataTable HeaderDT = GetHeaderDT(title, subTitle);

                PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
                //Import DT to PDF table
                myHeaderPdfTable.ImportDataTable(HeaderDT);
                myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
                myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
                myHeaderPdfTable.VisibleHeaders = false;
                myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
                myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 145, 55 });
                int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                if (titleLength < 75)
                {
                    myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
                int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                if (subTitleLength < 75)
                {
                    myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle
                if (myHeaderPdfTable.Rows.Length > 2)
                {
                    int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                    if (dateLength < 75)
                    {
                        myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
                }
                PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));
                double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
                double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

                //Adding new page and adding Header table,logo image and pageNo 
                PdfPage newPdfPage = myPdfDocument.NewPage();
                //Adding Header table
                newPdfPage.Add(myHeaderPdfTablePage);
                //Adding logo
                newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));

                foreach (DataRow dRow in dt.Rows)
                {
                    //PdfPage newPdfPage = myPdfDocument.NewPage();

                    //newPdfPage.Add(LogoImage, imgPosX, imgPosY);
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
                        dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                    }
                    parentDT.Rows.Add(dNewRow);

                    string parentTrxID = string.Empty;
                    parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                    //Removing TrxID column
                    if (parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Remove("TrxID");
                    }

                    //Pivoting the tables
                    pNotesDT = reportsBO.PivotTable(NotesDT);
                    pDT = reportsBO.PivotTable(parentDT);

                    // Varaible to get the Row and Column count of three tables
                    int rowsInTab = pDT.Rows.Count;
                    int colsInTab = pDT.Columns.Count;

                    int[] Arraywidth = new int[colsInTab];
                    //Setting columns width based on the No of columns
                    for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                    {
                        Arraywidth[colCnt] = 20;
                    }

                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);
                    //Import DT to PDF table
                    pDT = objReportsUI.WrapFullViewLength(pDT, Arraywidth);
                    myPdfTable.ImportDataTable(pDT);
                    //Setting the header row text color as white
                    myPdfTable.HeadersRow.SetForegroundColor(Color.White);
                    //myPdfTable.HeadersRow.SetFont(HeaderFont);
                    myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    myPdfTable.Columns[0].SetFont(HeaderFont);
                    myPdfTable.SetColumnsWidth(Arraywidth);
                    //myPdfTable.SetRowHeight(15);                
                    //Now we set some alignment... for the whole table and then, for a column:
                    myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                    while (!myPdfTable.AllTablePagesCreated)
                    {
                        //Setting the Y position and if required creating new page
                        if (currentYPos > myPdfDocument.PageHeight - 50)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }
                        else
                        {
                            //if(posY!=130)
                            posY = currentYPos + 35;
                        }

                        if (myPdfDocument.PageHeight - posY < 200)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }

                        PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, 300, height));
                        newPdfPage.Add(newPdfTablePage);
                        currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                    }

                    //Notes DT
                    if (NotesDT.Rows.Count > 0)
                    {
                        if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
                        {
                            int rowsInNotesDT = NotesDT.Rows.Count;
                            int colsInNotesDT = NotesDT.Columns.Count;
                            PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
                            myPdfTable1.ImportDataTable(NotesDT);
                            myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
                            myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable1.HeadersRow.SetFont(HeaderFont);
                            myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                            myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                            //myPdfTable.Columns[2].SetContentFormat("{0:dd/MM/yyyy}");

                            while (!myPdfTable1.AllTablePagesCreated)
                            {

                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    //Adding logo
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }
                                else
                                {
                                    posY = currentYPos + 25;
                                }

                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    //Adding logo
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }

                                PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                newPdfPage.Add(newPdfTablePage1);
                                currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                            }
                        }
                    }

                    //Getting the dataset to be bound to the grid.
                    XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                        {
                            //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                            {
                                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                                DataTable BranchDT = new DataTable();
                                BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                                if (BranchDT.Rows.Count > 0)
                                {
                                    Hashtable m_htBranchRightAlign = new Hashtable();
                                    Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                                    bool sumExists = false;

                                    int rowsInBranchDT = BranchDT.Rows.Count;
                                    int colsInBranchDT = BranchDT.Columns.Count;

                                    //Getting the columns to be displayed in grid
                                    XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                    int[] BranchArraywidth = new int[colsInBranchDT];

                                    //Storing the columns names and captions in the HashTable
                                    foreach (DataColumn dBranchCol in BranchDT.Columns)
                                    {
                                        XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");

                                        //Checking for isSummed value for that column
                                        if (nodeBranchCol != null)
                                        {
                                            //Getting the cols having Issummed=1 
                                            if (nodeBranchCol.Attributes["IsSummed"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                                {
                                                    if (!sumExists)
                                                        sumExists = true;
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
                                                }
                                            }
                                            //Getting the cols having ControlType="Amount"//Isnumeric=1
                                            if (nodeBranchCol.Attributes["ControlType"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                                {
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
                                                }

                                            }
                                        }

                                        //Setting the column width of branch table
                                        int dcPos = dBranchCol.Ordinal;
                                        int colFVL = 0;
                                        if (nodeBranchCol != null)
                                        {
                                            if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                            {
                                                colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                            }
                                        }
                                        if (colFVL != 0)
                                        {
                                            BranchArraywidth[dcPos] = colFVL;
                                        }
                                        else
                                        {
                                            BranchArraywidth[dcPos] = 15;
                                        }
                                    }
                                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);

                                    Font myHeaderFont = new Font("Verdana", 10, FontStyle.Bold);

                                    myPdfTable1.ImportDataTable(BranchDT);
                                    myPdfTable1.HeadersRow.SetFont(myHeaderFont);
                                    //myPdfTable1.HeadersRow.SetFont(HeaderFont);
                                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    myPdfTable1.SetColumnsWidth(BranchArraywidth);
                                    myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //myPdfTable1.SetRowHeight(15);
                                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    if (m_htBranchRightAlign.Count > 0)
                                    {
                                        if (sumExists)
                                        {
                                            //Right justifying Summed row content
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        //Right justifying Summed column content and IsNumeric column content
                                        for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
                                        {
                                            //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                            if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                            {
                                                if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
                                                {
                                                    myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                    myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                            }
                                        }
                                    }

                                    while (!myPdfTable1.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            posY = currentYPos + 10;
                                        }
                                        if (myPdfDocument.PageHeight - posY < 200)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }

                                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                        newPdfPage.Add(newPdfTablePage1);
                                        currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                                    }
                                }


                            }
                        }
                    }
                    //newPdfPage.SaveToDocument();

                    //Deleting the printed row from parentDT
                    if (tableLayout.ToUpper().Trim() == "PIVOT")
                    {
                        #region NLog
                        logger.Debug("Deleting the printed row from the parent datatable because the  table layout is PIVOT.");  
                        #endregion

                        pDT.Columns.RemoveAt(1);
                        parentDT.Rows[0].Delete();
                        //Adding TrxID column
                        if (!parentDT.Columns.Contains("TrxID"))
                        {
                            parentDT.Columns.Add("TrxID");
                        }
                    }
                }
                newPdfPage.SaveToDocument();

                SaveToResponse(fileName);
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

        #region GVNormalParentNewPgExpToPDF

        private void GVNormalParentNewPgExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {
            try
            {
                Hashtable m_htRightAlign = new Hashtable();

                //Get the treeNodeName
                string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                clsReportsUI objReportsUI = new clsReportsUI();
                dt = objReportsUI.ChangeDateAndAmountFormats(dt, xDocOut.OuterXml.ToString(), treeNodeName);
                //Getting the columns to be displayed in grid
                XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

                //Getting the rows to print
                XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

                ////Default portrait pLayout="0"
                ////                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11);
                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);

                //Considering the PLayout only for the first treenode
                if (nodeRowList != null)
                {
                    if (myPdfDocument.PageCount == 0)
                    {
                        if (nodeRowList.FirstChild != null)
                        {
                            if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
                            {
                                string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
                                if (pLayout == "1")//Landscape
                                {
                                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                                    //myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);                    
                                }
                            }
                        }
                    }
                }

                bool isSumExists = false;

                int[] Arraywidth = new int[dt.Columns.Count - 1];//not considering trxid col whose ordinal is 0
                //Storing the column width based on FVL in an array
                //Storing the captions of cols having IsSummed and IsNumeric as 1 in the HashTable
                foreach (XmlNode node in nodeColumns)
                {
                    if (node.Attributes["Caption"] != null)
                    {
                        //if (node.Attributes["Caption"].Value.ToString().Trim() != "TrxID")
                        {
                            DataColumn dc = dt.Columns[node.Attributes["Caption"].Value];
                            if (dc != null)
                            {
                                //Set the column width based on FVL
                                if (node.Attributes["FullViewLength"] != null)
                                {
                                    if (node.Attributes["FullViewLength"].Value != "0")
                                    {
                                        Arraywidth[dc.Ordinal] = Convert.ToInt32(node.Attributes["FullViewLength"].Value);
                                    }
                                    else
                                    {
                                        Arraywidth[dc.Ordinal] = 15;
                                    }
                                }
                                //Getting the cols having Issummed=1 
                                if (node.Attributes["IsSummed"] != null)
                                {
                                    if (node.Attributes["IsSummed"].Value == "1")
                                    {
                                        if (!isSumExists)
                                            isSumExists = true;
                                        if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                            m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["IsSummed"].Value);
                                    }
                                }
                                //Getting the cols having ControlType="Amount"//Isnumeric=1
                                if (node.Attributes["ControlType"] != null)
                                {
                                    if (node.Attributes["ControlType"].Value == "Amount")
                                    {
                                        if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                            m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["ControlType"].Value);
                                    }
                                }
                            }
                        }
                    }
                }
                //Setting columns width for cols not present in node columns
                for (int colCnt = 0; colCnt < dt.Columns.Count - 1; colCnt++)
                {
                    if (Arraywidth[colCnt].ToString() == string.Empty)
                        Arraywidth[colCnt] = 15;
                }

                //Removing the Summed row from parent table
                if (isSumExists)
                {
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

                //PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);
                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";

                //PdfPage newPdfPage = myPdfDocument.NewPage();

                PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
                double posX = 20;//50
                double posY = 70;//90
                double width = myPdfDocument.PageWidth - 50;// 690; 
                double height = myPdfDocument.PageHeight - 50;// 250;
                double currentYPos = 70;
                int pageCnt = 0;

                foreach (DataRow dRow in dt.Rows)
                {
                    PdfPage newPdfPage = myPdfDocument.NewPage();

                    posY = 70;
                    currentYPos = 70;

                    //Adding each row at a time
                    DataRow dNewRow = parentDT.NewRow();
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                    }
                    parentDT.Rows.Add(dNewRow);

                    string parentTrxID = string.Empty;
                    parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                    //Removing TrxID column
                    if (parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Remove("TrxID");
                    }

                    // Varaible to get the Row and Column count of three tables
                    int rowsInTab = parentDT.Rows.Count;
                    int colsInTab = parentDT.Columns.Count;

                    //Font FontBold = new Font("Verdana", 9, FontStyle.Bold);
                    Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
                    Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
                    Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
                    Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
                    Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
                    PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);

                    //Import DT to PDF table
                    parentDT = objReportsUI.WrapFullViewLength(parentDT, Arraywidth);
                    myPdfTable.ImportDataTable(parentDT);
                    //Setting the header row text color as white
                    //myPdfTable.HeadersRow.SetForegroundColor(Color.White);
                    myPdfTable.HeadersRow.SetFont(HeaderFont);
                    myPdfTable.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                    myPdfTable.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    //myPdfTable.Columns[0].SetFont(HeaderFont);

                    myPdfTable.SetColumnsWidth(Arraywidth);
                    //myPdfTable.SetRowHeight(15);                
                    //Now we set some alignment... for the whole table and then, for a column:
                    myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                    if (m_htRightAlign.Count > 0)
                    {
                        //Right justifying Summed column content and IsNumeric column content
                        for (int pdfcol = 0; pdfcol < colsInTab; pdfcol++)
                        {
                            //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                            if (myPdfTable.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                            {
                                if (m_htRightAlign.Contains(myPdfTable.HeadersRow[pdfcol].Content.ToString()))
                                {
                                    myPdfTable.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                    myPdfTable.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                }
                            }
                        }
                    }

                    //Getting Header table
                    string title = string.Empty;
                    XmlNode nodeTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                    if (nodeTitle != null)
                    {
                        title = nodeTitle.InnerText.Trim().ToString();
                    }
                    string subTitle = string.Empty;
                    XmlNode nodeSubTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
                    if (nodeSubTitle != null)
                    {
                        subTitle = nodeSubTitle.InnerText.Trim().ToString();
                    }
                    DataTable HeaderDT = GetHeaderDT(title, subTitle);

                    PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
                    //Import DT to PDF table
                    myHeaderPdfTable.ImportDataTable(HeaderDT);
                    myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
                    myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
                    myHeaderPdfTable.VisibleHeaders = false;
                    myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
                    myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                    myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 145, 55 });
                    int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                    if (titleLength < 75)
                    {
                        myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
                    int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
                    if (subTitleLength < 75)
                    {
                        myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle
                    if (myHeaderPdfTable.Rows.Length > 2)
                    {
                        int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                        if (dateLength < 75)
                        {
                            myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
                    }
                    //PdfArea pdfArea = new PdfArea(myPdfDocument, posX, 70, width, 60);
                    PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));
                    double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
                    double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

                    newPdfPage.Add(myHeaderPdfTablePage);
                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));

                    while (!myPdfTable.AllTablePagesCreated)
                    {
                        //Setting the Y position and if required creating new page
                        if (currentYPos > myPdfDocument.PageHeight - 50)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }
                        else
                        {
                            posY = currentYPos;// +25;
                        }

                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                        {
                            posY = 70;
                            currentYPos = 70;
                            newPdfPage.SaveToDocument();
                            //Adding new page and adding Header table,logo image and pageNo 
                            newPdfPage = myPdfDocument.NewPage();
                            newPdfPage.Add(myHeaderPdfTablePage);
                            //Adding logo
                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                        }

                        PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height));
                        newPdfPage.Add(newPdfTablePage);
                        currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                    }

                    //Notes DT
                    if (NotesDT.Rows.Count > 0)
                    {
                        if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
                        {

                            int rowsInNotesDT = NotesDT.Rows.Count;
                            int colsInNotesDT = NotesDT.Columns.Count;
                            PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
                            myPdfTable1.ImportDataTable(NotesDT);
                            myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
                            myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable1.HeadersRow.SetFont(HeaderFont);
                            myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                            myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                            while (!myPdfTable1.AllTablePagesCreated)
                            {

                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    //Adding logo
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }
                                else
                                {
                                    posY = currentYPos;// +25;
                                }

                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    //Adding logo
                                    newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                }

                                PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                newPdfPage.Add(newPdfTablePage1);
                                currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                            }
                        }
                    }
                    //

                    //Getting the dataset to be bound to the grid.
                    XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                        {
                            //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                            {
                                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                                DataTable BranchDT = new DataTable();
                                BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                                if (BranchDT.Rows.Count > 0)
                                {

                                    Hashtable m_htBranchRightAlign = new Hashtable();
                                    Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                                    bool sumExists = false;

                                    int rowsInBranchDT = BranchDT.Rows.Count;
                                    int colsInBranchDT = BranchDT.Columns.Count;

                                    //Getting the columns to be displayed in grid
                                    XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                    int[] BranchArraywidth = new int[colsInBranchDT];

                                    //Storing the columns names and captions in the HashTable
                                    foreach (DataColumn dBranchCol in BranchDT.Columns)
                                    {
                                        XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");

                                        //Checking for isSummed value for that column
                                        if (nodeBranchCol != null)
                                        {
                                            //Getting the cols having Issummed=1 
                                            if (nodeBranchCol.Attributes["IsSummed"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                                {
                                                    if (!sumExists)
                                                        sumExists = true;
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
                                                }
                                            }
                                            //Getting the cols having ControlType="Amount"//Isnumeric=1
                                            if (nodeBranchCol.Attributes["ControlType"] != null)
                                            {
                                                if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                                {
                                                    if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                        m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
                                                }
                                            }
                                        }

                                        //Setting the column width of branch table
                                        int dcPos = dBranchCol.Ordinal;
                                        int colFVL = 0;
                                        if (nodeBranchCol != null)
                                        {
                                            if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                            {
                                                colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                            }
                                        }
                                        if (colFVL != 0)
                                        {
                                            BranchArraywidth[dcPos] = colFVL;
                                        }
                                        else
                                        {
                                            BranchArraywidth[dcPos] = 15;
                                        }
                                    }
                                    PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);

                                    Font myHeaderFont = new Font("Verdana", 10, FontStyle.Bold);
                                    myPdfTable1.ImportDataTable(BranchDT);
                                    myPdfTable1.HeadersRow.SetFont(myHeaderFont);
                                    //myPdfTable1.HeadersRow.SetFont(HeaderFont);
                                    myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    myPdfTable1.SetColumnsWidth(BranchArraywidth);
                                    myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //myPdfTable1.SetRowHeight(15);
                                    myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    if (m_htBranchRightAlign.Count > 0)
                                    {
                                        if (sumExists)
                                        {
                                            //Right justifying Summed row content
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
                                            myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        //Right justifying Summed column content and IsNumeric column content
                                        for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
                                        {
                                            //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                            if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                            {
                                                if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
                                                {
                                                    myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                    myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                            }
                                        }
                                    }

                                    while (!myPdfTable1.AllTablePagesCreated)
                                    {

                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            posY = currentYPos + 25;
                                        }

                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            //Adding logo
                                            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                        }

                                        PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                        newPdfPage.Add(newPdfTablePage1);
                                        currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                                    }
                                }
                            }
                        }
                    }
                    newPdfPage.SaveToDocument();

                    //Deleting the printed row from parentDT
                    if (tableLayout.ToUpper().Trim() == "NORMAL")
                    {
                        #region NLog
                        logger.Debug("Deleting the printed row from the parent datatable because the  table layout is PIVOT.");  
                        #endregion

                        parentDT.Rows[0].Delete();
                        //Adding TrxID column
                        if (!parentDT.Columns.Contains("TrxID"))
                        {
                            parentDT.Columns.Add("TrxID");
                        }
                    }
                }

                SaveToResponse(fileName);
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

        #region GVNormalParentCntPgExpToPDF

        private void GVNormalParentCntPgExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {

            Hashtable m_htRightAlign = new Hashtable();

            //Get the treeNodeName
            string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            clsReportsUI objReportsUI = new clsReportsUI();
            dt = objReportsUI.ChangeDateAndAmountFormats(dt, xDocOut.OuterXml.ToString(), treeNodeName);

            //Getting the columns to be displayed in grid
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

            //Getting the rows to print
            XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");

            ////Default portrait pLayout="0"
            ////                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11);
            myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);

            //Considering the PLayout only for the first treenode
            if (nodeRowList != null)
            {
                if (myPdfDocument.PageCount == 0)
                {
                    if (nodeRowList.FirstChild != null)
                    {
                        if (nodeRowList.FirstChild.Attributes["pLayout"] != null)
                        {
                            string pLayout = nodeRowList.FirstChild.Attributes["pLayout"].Value.ToString().Trim();
                            if (pLayout == "1")//Landscape
                            {
                                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                                //myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);                    
                            }
                        }
                    }
                }
            }

            //int colPos = 0;
            bool isSumExists = false;

            int[] Arraywidth = new int[dt.Columns.Count - 1];//not considering trxid col whose ordinal is 0
            //Storing the column width based on FVL in an array
            //Storing the captions of cols having IsSummed and IsNumeric as 1 in the HashTable
            foreach (XmlNode node in nodeColumns)
            {
                if (node.Attributes["Caption"] != null)
                {
                    //if (node.Attributes["Caption"].Value.ToString().Trim() != "TrxID")
                    {
                        DataColumn dc = dt.Columns[node.Attributes["Caption"].Value];
                        if (dc != null)
                        {
                            //Set the column width based on FVL
                            if (node.Attributes["FullViewLength"] != null)
                            {
                                if (node.Attributes["FullViewLength"].Value != "0")
                                {
                                    Arraywidth[dc.Ordinal] = Convert.ToInt32(node.Attributes["FullViewLength"].Value);
                                }
                                else
                                {
                                    Arraywidth[dc.Ordinal] = 15;
                                }
                            }
                            //Getting the cols having Issummed=1 
                            if (node.Attributes["IsSummed"] != null)
                            {
                                if (node.Attributes["IsSummed"].Value == "1")
                                {
                                    if (!isSumExists)
                                        isSumExists = true;
                                    if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                        m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["IsSummed"].Value);
                                }
                            }
                            //Getting the cols having ControlType="Amount"//Isnumeric=1
                            if (node.Attributes["ControlType"] != null)
                            {
                                if (node.Attributes["ControlType"].Value == "Amount")
                                {
                                    if (!m_htRightAlign.Contains(node.Attributes["Caption"].Value))
                                        m_htRightAlign.Add(node.Attributes["Caption"].Value, node.Attributes["ControlType"].Value);
                                }
                            }
                        }
                    }
                }
            }
            //Setting columns width for cols not present in node columns
            for (int colCnt = 0; colCnt < dt.Columns.Count - 1; colCnt++)
            {
                if (Arraywidth[colCnt].ToString() == string.Empty)
                    Arraywidth[colCnt] = 15;
            }

            //Removing the Summed row from parent table
            if (isSumExists)
            {
                #region NLog
                logger.Debug("Removing the summed row from the parent table."); 
                #endregion

                dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

            //PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);
            string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit_small-greylogo_03.JPG";

            //PdfPage newPdfPage = myPdfDocument.NewPage();

            PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
            double posX = 20;//50
            double posY = 70;//90
            double width = myPdfDocument.PageWidth - 50;// 690; 
            double height = myPdfDocument.PageHeight - 50;// 250;
            double currentYPos = 70;

            //Setting the different font styles
            //Font FontBold = new Font("Verdana", 9, FontStyle.Bold);
            Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
            Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
            Font HeaderRowFont = new Font("Verdana", 1, FontStyle.Regular, 0);
            Font HeaderPageTitleFont = new Font("Verdana", 9, FontStyle.Bold);
            Font HeaderPageTitleFont1 = new Font("Verdana", 9, FontStyle.Bold);
            Font HeaderPageTitleFont2 = new Font("Verdana", 8, FontStyle.Bold);
            Font HeaderPageTitleFont3 = new Font("Verdana", 7, FontStyle.Bold);
            Font RowFontBold = new Font("Verdana", 8, FontStyle.Bold);
            Font FontUnderline = new Font("Verdana", 8, FontStyle.Regular | FontStyle.Underline);
            Font RowBoxFontBold = new Font("Verdana", 10, FontStyle.Bold);

            int pageCnt = 0;

            //Getting Header table
            string title = string.Empty;
            XmlNode nodeTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
            if (nodeTitle != null)
            {
                title = nodeTitle.InnerText.Trim();
            }
            string subTitle = string.Empty;
            XmlNode nodeSubTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/SubTitle");
            if (nodeSubTitle != null)
            {
                subTitle = nodeSubTitle.InnerText.Trim();
            }
            DataTable HeaderDT = GetHeaderDT(title, subTitle);

            PdfTable myHeaderPdfTable = myPdfDocument.NewTable(FontRegular, HeaderDT.Rows.Count, HeaderDT.Columns.Count, 1);
            //Import DT to PDF table
            myHeaderPdfTable.ImportDataTable(HeaderDT);
            myHeaderPdfTable.HeadersRow.SetColors(Color.LightGray, Color.LightGray);
            myHeaderPdfTable.HeadersRow.SetFont(HeaderRowFont);
            myHeaderPdfTable.VisibleHeaders = false;
            myHeaderPdfTable.SetBackgroundColor(Color.LightGray);
            myHeaderPdfTable.SetBorders(Color.Black, 1, BorderType.None);
            myHeaderPdfTable.SetColumnsWidth(new int[] { 140, 300, 145, 55 });
            int titleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
            myHeaderPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
            if (titleLength < 75)
            {
                myHeaderPdfTable.Rows[0][1].SetContentAlignment(ContentAlignment.MiddleCenter);
            }
            myHeaderPdfTable.Rows[0][1].SetFont(HeaderPageTitleFont1);//Page Title
            int subTitleLength = myHeaderPdfTable.Rows[0][1].Content.ToString().Length;
            if (subTitleLength < 75)
            {
                myHeaderPdfTable.Rows[1][1].SetContentAlignment(ContentAlignment.MiddleCenter);
            }
            myHeaderPdfTable.Rows[1][1].SetFont(HeaderPageTitleFont2);//Page subTitle
            if (myHeaderPdfTable.Rows.Length > 2)
            {
                int dateLength = myHeaderPdfTable.Rows[2][1].Content.ToString().Length;
                if (dateLength < 75)
                {
                    myHeaderPdfTable.Rows[2][1].SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                myHeaderPdfTable.Rows[2][1].SetFont(HeaderPageTitleFont3);//Page subTitle
            }
            PdfTablePage myHeaderPdfTablePage = myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, 20, width, 70));
            double imgPosX = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.X;
            double imgPosY = myHeaderPdfTablePage.CellArea(0, 3).TopLeftVertex.Y;

            //Adding new page and adding Header table,logo image and pageNo 
            PdfPage newPdfPage = myPdfDocument.NewPage();
            //Adding Header table
            newPdfPage.Add(myHeaderPdfTablePage);
            //Adding logo
            newPdfPage.Add(LogoImage, imgPosX, imgPosY);
            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));

            foreach (DataRow dRow in dt.Rows)
            {
                //Adding each row at a time
                DataRow dNewRow = parentDT.NewRow();
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                }
                parentDT.Rows.Add(dNewRow);

                string parentTrxID = string.Empty;
                parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                //Removing TrxID column
                if (parentDT.Columns.Contains("TrxID"))
                {
                    parentDT.Columns.Remove("TrxID");
                }

                // Varaible to get the Row and Column count of three tables
                int rowsInTab = parentDT.Rows.Count;
                int colsInTab = parentDT.Columns.Count;
                PdfTable myPdfTable = myPdfDocument.NewTable(FontRegular, rowsInTab, colsInTab, 1);

                //Import DT to PDF table
                parentDT = objReportsUI.WrapFullViewLength(parentDT, Arraywidth);
                myPdfTable.ImportDataTable(parentDT);
                //Setting the header row text color as white
                //myPdfTable.HeadersRow.SetForegroundColor(Color.White);
                myPdfTable.HeadersRow.SetFont(HeaderFont);
                myPdfTable.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                myPdfTable.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfTable.SetBorders(Color.Black, 1, BorderType.None);
                //myPdfTable.Columns[0].SetFont(HeaderFont);

                myPdfTable.SetColumnsWidth(Arraywidth);
                //myPdfTable.SetRowHeight(15);                
                //Now we set some alignment... for the whole table and then, for a column:
                myPdfTable.SetContentAlignment(ContentAlignment.MiddleLeft);
                if (m_htRightAlign.Count > 0)
                {
                    //Right justifying Summed column content and IsNumeric column content
                    for (int pdfcol = 0; pdfcol < colsInTab; pdfcol++)
                    {
                        //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                        if (myPdfTable.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                        {
                            if (m_htRightAlign.Contains(myPdfTable.HeadersRow[pdfcol].Content.ToString()))
                            {
                                myPdfTable.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                myPdfTable.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                            }
                        }
                    }
                }

                //HeaderTable placed on top

                while (!myPdfTable.AllTablePagesCreated)
                {
                    //Setting the Y position and if required creating new page
                    if (currentYPos > myPdfDocument.PageHeight - 50)
                    {
                        posY = 70;
                        currentYPos = 70;
                        newPdfPage.SaveToDocument();
                        //Adding new page and adding Header table,logo image and pageNo 
                        newPdfPage = myPdfDocument.NewPage();
                        newPdfPage.Add(myHeaderPdfTablePage);
                        //Adding logo
                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                    }
                    else
                    {
                        if (posY != 130)
                            posY = currentYPos + 35;
                    }

                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                    {
                        posY = 70;
                        currentYPos = 70;
                        newPdfPage.SaveToDocument();
                        //Adding new page and adding Header table,logo image and pageNo 
                        newPdfPage = myPdfDocument.NewPage();
                        newPdfPage.Add(myHeaderPdfTablePage);
                        //Adding logo
                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                    }

                    PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height));
                    newPdfPage.Add(newPdfTablePage);
                    currentYPos = newPdfTablePage.Area.BottomLeftVertex.Y;
                }

                //Notes DT
                if (NotesDT.Rows.Count > 0)
                {
                    if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
                    {
                        int rowsInNotesDT = NotesDT.Rows.Count;
                        int colsInNotesDT = NotesDT.Columns.Count;
                        PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInNotesDT, colsInNotesDT, 1);
                        myPdfTable1.ImportDataTable(NotesDT);
                        myPdfTable1.SetBorders(Color.Black, 1, BorderType.None);
                        myPdfTable1.SetColumnsWidth(new int[] { 50, 100 });
                        myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                        myPdfTable1.HeadersRow.SetFont(HeaderFont);
                        myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                        myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);

                        while (!myPdfTable1.AllTablePagesCreated)
                        {
                            //Setting the Y position and if required creating new page
                            if (currentYPos > myPdfDocument.PageHeight - 50)
                            {
                                posY = 70;
                                currentYPos = 70;
                                newPdfPage.SaveToDocument();
                                //Adding new page and adding Header table,logo image and pageNo 
                                newPdfPage = myPdfDocument.NewPage();
                                newPdfPage.Add(myHeaderPdfTablePage);
                                //Adding logo
                                newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                            }
                            else
                            {
                                posY = currentYPos + 25;
                            }

                            if (myPdfDocument.PageHeight - 50 - posY < 50)
                            {
                                posY = 70;
                                currentYPos = 70;
                                newPdfPage.SaveToDocument();
                                //Adding new page and adding Header table,logo image and pageNo 
                                newPdfPage = myPdfDocument.NewPage();
                                newPdfPage.Add(myHeaderPdfTablePage);
                                //Adding logo
                                newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                            }

                            PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                            newPdfPage.Add(newPdfTablePage1);
                            currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                        }
                    }
                }

                //Branch DT
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                            DataTable BranchDT = new DataTable();
                            BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                            if (BranchDT.Rows.Count > 0)
                            {
                                Hashtable m_htBranchRightAlign = new Hashtable();
                                Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                                bool sumExists = false;

                                int rowsInBranchDT = BranchDT.Rows.Count;
                                int colsInBranchDT = BranchDT.Columns.Count;

                                //Hashtable m_htBranchPagebrk = new Hashtable();
                                //int branchPageBreaks = 0;
                                //XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");

                                //if (nodeResRows != null)
                                //{
                                //    if (nodeResRows.Count>0)
                                //    {
                                //        //foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
                                //        foreach (XmlNode nodeResRow in nodeResRows)
                                //        {
                                //            if (nodeResRow.Attributes["TrxID"] != null)
                                //            {
                                //                string trxID = nodeResRow.Attributes["TrxID"].Value;
                                //                DataRow[] foundRows;
                                //                foundRows = BranchDT.Select("TrxID ='" + trxID + "'");

                                //                if (foundRows.Length > 0)
                                //                {
                                //                    int rowIndex = BranchDT.Rows.IndexOf(foundRows[0]);

                                //                    if (nodeResRow.Attributes["pLnSkip"] != null)
                                //                    {
                                //                        if (nodeResRow.Attributes["pLnSkip"].Value.ToString().Trim() != "0")
                                //                        {
                                //                            for (int skipCnt = 0; skipCnt < Convert.ToInt32(nodeResRow.Attributes["pLnSkip"].Value.Trim()); skipCnt++)
                                //                            {
                                //                                // Adding each row at a time
                                //                                DataRow dSkipRow = BranchDT.NewRow();
                                //                                for (int col = 0; col < BranchDT.Columns.Count; col++)
                                //                                {
                                //                                    dSkipRow[BranchDT.Columns[col].ColumnName] = "SKIP";
                                //                                }
                                //                                BranchDT.Rows.InsertAt(dSkipRow, rowIndex + 1);
                                //                                BranchDT.AcceptChanges();
                                //                            }
                                //                        }
                                //                    }

                                //                    if (nodeResRow.Attributes["pPgBreak"] != null)
                                //                    {
                                //                        if (nodeResRow.Attributes["pPgBreak"].Value.ToString().Trim() == "1")
                                //                        {
                                //                            branchPageBreaks++;
                                //                            m_htBranchPagebrk.Add(branchPageBreaks, rowIndex);
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}


                                //Getting the columns to be displayed in grid
                                XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                                int branchColPos = 0;
                                int[] BranchArraywidth = new int[colsInBranchDT];

                                foreach (DataColumn dBranchCol in BranchDT.Columns)
                                {
                                    XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = " + ParseXpathString(dBranchCol.ColumnName) + "]");

                                    //Checking for isSummed value for that column
                                    if (nodeBranchCol != null)
                                    {
                                        //Getting the cols having Issummed=1 
                                        if (nodeBranchCol.Attributes["IsSummed"] != null)
                                        {
                                            if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                            {
                                                if (!sumExists)
                                                    sumExists = true;
                                                if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                    m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
                                            }
                                        }
                                        //Getting the cols having ControlType="Amount"//Isnumeric=1
                                        if (nodeBranchCol.Attributes["ControlType"] != null)
                                        {
                                            if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                            {
                                                if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                    m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
                                            }
                                        }
                                    }

                                    //Setting the column width of branch table
                                    int dcPos = dBranchCol.Ordinal;
                                    int colFVL = 0;
                                    if (nodeBranchCol != null)
                                    {
                                        if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                        {
                                            colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                        }
                                    }
                                    if (colFVL != 0)
                                    {
                                        BranchArraywidth[dcPos] = colFVL;
                                    }
                                    else
                                    {
                                        BranchArraywidth[dcPos] = 15;
                                    }
                                }
                                PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);

                                Font myHeaderFont = new Font("Verdana", 10, FontStyle.Bold);

                                myPdfTable1.ImportDataTable(BranchDT);
                                myPdfTable1.HeadersRow.SetFont(myHeaderFont);
                                //myPdfTable1.HeadersRow.SetFont(HeaderFont);
                                myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                myPdfTable1.SetColumnsWidth(BranchArraywidth);
                                myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                //myPdfTable1.SetRowHeight(15);
                                myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (m_htBranchRightAlign.Count > 0)
                                {
                                    if (sumExists)
                                    {
                                        //Right justifying Summed row content
                                        myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
                                        myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                    }
                                    //Right justifying Summed column content and IsNumeric column content
                                    for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
                                    {
                                        //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                        if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                        {
                                            if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
                                            {
                                                myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                    }
                                }

                                ////Setting Row font
                                //for (int pdfRow = 0; pdfRow < rowsInBranchDT; pdfRow++)
                                //{
                                //    string trxID = BranchDT.Rows[pdfRow]["TrxID"].ToString();                                        
                                //    //XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID = '" + trxID + "']");
                                //    XmlNode nodeRow = xDoc.SelectSingleNode("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "' and @" + "TrxID= '" + trxID + "']");
                                //    //string xPath = "Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID='" + parentTrxID + "' and @" + treeNodeName + "_TrxType='" + parentTrxType + "']";

                                //    //XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@" + firstColLabel + " = '" + myPdfTable.Rows[pdfRow][0].Content.ToString() + "']");
                                //    if (nodeRow != null)
                                //    {
                                //        if (nodeRow.Attributes["pFont"] != null)
                                //        {
                                //            if (nodeRow.Attributes["pFont"].Value.ToString().Trim() == "1")
                                //            {
                                //                myPdfTable.Rows[pdfRow].SetFont(RowFontBold);
                                //            }
                                //        }
                                //        if (nodeRow.Attributes["pBox"] != null)
                                //        {
                                //            if (nodeRow.Attributes["pBox"].Value.ToString().Trim() == "1")
                                //            {
                                //                myPdfTable.Rows[pdfRow].SetFont(RowBoxFontBold);
                                //            }
                                //        }
                                //    }
                                //    if (trxID.ToUpper().Trim() == "SKIP")
                                //    {
                                //        myPdfTable.Rows[pdfRow].SetForegroundColor(Color.White);
                                //    }
                                //}


                                while (!myPdfTable1.AllTablePagesCreated)
                                {
                                    //Setting the Y position and if required creating new page
                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo 
                                        newPdfPage = myPdfDocument.NewPage();
                                        newPdfPage.Add(myHeaderPdfTablePage);
                                        //Adding logo
                                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                    }
                                    else
                                    {
                                        posY = currentYPos + 10;
                                    }
                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo 
                                        newPdfPage = myPdfDocument.NewPage();
                                        newPdfPage.Add(myHeaderPdfTablePage);
                                        //Adding logo
                                        newPdfPage.Add(LogoImage, imgPosX, imgPosY);
                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, myPdfDocument.PageWidth - 400, myPdfDocument.PageHeight - 50, myPdfDocument.PageWidth - 200, 50), ContentAlignment.MiddleCenter, "Page " + Convert.ToString((++pageCnt))));
                                    }

                                    PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                    newPdfPage.Add(newPdfTablePage1);
                                    currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;
                                }

                            }
                        }
                    }
                }
                //newPdfPage.SaveToDocument();

                //Deleting the printed row from parentDT
                if (tableLayout.ToUpper().Trim() == "NORMAL")
                {
                    #region NLog
                    logger.Debug("Deleting the printed row from the parent datatable because the  table layout is NORMAL.");  
                    #endregion

                    parentDT.Rows[0].Delete();
                    //Adding TrxID column
                    if (!parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Add("TrxID");
                    }
                }
            }
            newPdfPage.SaveToDocument();

            SaveToResponse(fileName);

        }

        private static string ParseXpathString(string input)
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


        #endregion

        #region ChecksExpToPDF

        private void ChecksExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {

            Hashtable m_htRightAlign = new Hashtable();

            //Get the treeNodeName
            string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            //Getting the columns to be displayed in grid
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
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
            }

            //Removing the Summed row from parent table
            if (isSumExists)
            {
                #region NLog
                logger.Debug("Removin the summed row from the parent table."); 
                #endregion

                dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);

            double posX = 20;
            double posY = 30;
            double width = myPdfDocument.PageWidth - 50;
            double height = myPdfDocument.PageHeight - 50;
            double currentYPos = 30;

            Font HeaderFont = new Font("Verdana", 9, FontStyle.Bold);
            Font FontRegular = new Font("Verdana", 7, FontStyle.Regular);
            int amountColPos = 0;
            double amtColPosX = 0;
            double amountColWidth = 0;

            foreach (DataRow dRow in dt.Rows)
            {
                PdfPage newPdfPage = myPdfDocument.NewPage();

                posY = 30;
                currentYPos = 30;

                //Adding each row at a time
                DataRow dNewRow = parentDT.NewRow();
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                }
                parentDT.Rows.Add(dNewRow);

                string parentTrxID = string.Empty;
                parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                //Removing TrxID column
                if (parentDT.Columns.Contains("TrxID"))
                {
                    parentDT.Columns.Remove("TrxID");
                }

                //Printing BranchDT
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                            DataTable BranchDT = new DataTable();
                            BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                            if (BranchDT.Rows.Count > 0)
                            {
                                //Removing the Rows if greater than 25 and adding a row with ...
                                if (BranchDT.Rows.Count > 25)
                                {
                                    for (int rwCnt = 26; rwCnt < BranchDT.Rows.Count; rwCnt++)
                                    {
                                        BranchDT.Rows.RemoveAt(rwCnt - 1);
                                        BranchDT.AcceptChanges();
                                    }
                                    // Adding last row with ...
                                    DataRow dLastRow = BranchDT.NewRow();
                                    for (int col = 0; col < BranchDT.Columns.Count; col++)
                                    {
                                        dLastRow[BranchDT.Columns[col].ColumnName] = "...";
                                    }
                                    BranchDT.Rows.InsertAt(dLastRow, BranchDT.Rows.Count);
                                    BranchDT.AcceptChanges();
                                }

                                Hashtable m_htBranchRightAlign = new Hashtable();
                                Font SumRowFont = new Font("Verdana", 9, FontStyle.Bold);
                                bool sumExists = false;

                                int rowsInBranchDT = BranchDT.Rows.Count;
                                int colsInBranchDT = BranchDT.Columns.Count;

                                //Getting the columns to be displayed in grid
                                XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                int[] BranchArraywidth = new int[colsInBranchDT];

                                //Storing the columns names and captions in the HashTable
                                foreach (DataColumn dBranchCol in BranchDT.Columns)
                                {
                                    XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");

                                    //Checking for isSummed value for that column
                                    if (nodeBranchCol != null)
                                    {
                                        //Getting the cols having Issummed=1 
                                        if (nodeBranchCol.Attributes["IsSummed"] != null)
                                        {
                                            if (nodeBranchCol.Attributes["IsSummed"].Value == "1")
                                            {
                                                if (!sumExists)
                                                    sumExists = true;
                                                if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                    m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["IsSummed"].Value);
                                            }
                                        }
                                        //Getting the cols having ControlType="Amount"//Isnumeric=1
                                        if (nodeBranchCol.Attributes["ControlType"] != null)
                                        {
                                            if (nodeBranchCol.Attributes["ControlType"].Value == "Amount")
                                            {
                                                if (!m_htBranchRightAlign.Contains(nodeBranchCol.Attributes["Caption"].Value))
                                                    m_htBranchRightAlign.Add(nodeBranchCol.Attributes["Caption"].Value, nodeBranchCol.Attributes["ControlType"].Value);
                                            }
                                        }
                                    }

                                    //Setting the column width of branch table
                                    int dcPos = dBranchCol.Ordinal;
                                    int colFVL = 0;
                                    if (nodeBranchCol != null)
                                    {
                                        if (nodeBranchCol.Attributes["FullViewLength"] != null)
                                        {
                                            colFVL = Convert.ToInt32(nodeBranchCol.Attributes["FullViewLength"].Value);
                                        }
                                    }
                                    if (colFVL != 0)
                                    {
                                        BranchArraywidth[dcPos] = colFVL;
                                    }
                                    else
                                    {
                                        BranchArraywidth[dcPos] = 15;
                                    }

                                    //Setting amount col width
                                    if (dBranchCol.ColumnName.ToString().Trim() == "Amount Entered")
                                    {
                                        amountColPos = dcPos;
                                        amountColWidth = BranchArraywidth[dcPos];
                                    }
                                }
                                PdfTable myPdfTable1 = myPdfDocument.NewTable(FontRegular, rowsInBranchDT, colsInBranchDT, 1);

                                Font myHeaderFont = new Font("Verdana", 10, FontStyle.Underline);
                                myPdfTable1.ImportDataTable(BranchDT);
                                myPdfTable1.HeadersRow.SetFont(myHeaderFont);
                                myPdfTable1.HeadersRow.SetFont(HeaderFont);
                                myPdfTable1.HeadersRow.SetColors(Color.Black, Color.Gainsboro);
                                //myPdfTable1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                myPdfTable1.SetColumnsWidth(BranchArraywidth);
                                myPdfTable1.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                //myPdfTable1.SetRowHeight(15);
                                myPdfTable1.SetContentAlignment(ContentAlignment.MiddleLeft);

                                if (m_htBranchRightAlign.Count > 0)
                                {
                                    if (sumExists)
                                    {
                                        //Right justifying Summed row content
                                        myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetFont(SumRowFont);
                                        myPdfTable1.Rows[BranchDT.Rows.Count - 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                    }
                                    //Right justifying Summed column content and IsNumeric column content
                                    for (int pdfcol = 0; pdfcol < colsInBranchDT; pdfcol++)
                                    {
                                        //if (myPdfTable.Rows[dt.Rows.Count - 1][pdfcol].Content.ToString() != string.Empty)
                                        if (myPdfTable1.HeadersRow[pdfcol].Content.ToString() != string.Empty)
                                        {
                                            if (m_htBranchRightAlign.Contains(myPdfTable1.HeadersRow[pdfcol].Content.ToString()))
                                            {
                                                myPdfTable1.HeadersRow[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfTable1.Columns[pdfcol].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                    }
                                }

                                while (!myPdfTable1.AllTablePagesCreated)
                                {
                                    //Setting the Y position and if required creating new page
                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                    {
                                        posY = 30;
                                        currentYPos = 30;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo
                                        newPdfPage = myPdfDocument.NewPage();
                                    }
                                    else
                                    {
                                        if (posY != currentYPos)
                                            posY = currentYPos + 25;
                                    }

                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                    {
                                        posY = 30;
                                        currentYPos = 30;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo
                                        newPdfPage = myPdfDocument.NewPage();
                                    }

                                    PdfTablePage newPdfTablePage1 = myPdfTable1.CreateTablePage(new PdfArea(myPdfDocument, posX, posY + 10, width, height));
                                    newPdfPage.Add(newPdfTablePage1);
                                    currentYPos = newPdfTablePage1.Area.BottomLeftVertex.Y;

                                    amtColPosX = newPdfTablePage1.CellArea(0, amountColPos).TopLeftVertex.X;
                                    //double imgPosY = newPdfTablePage1.CellArea(0, amountColPos).TopLeftVertex.Y;
                                }
                            }
                        }
                    }
                }


                //Printing Parent DT                   
                //while (!myPdfTable.AllTablePagesCreated)
                if (parentDT.Rows.Count > 0)
                {
                    //Setting the Y position and if required creating new page
                    if (currentYPos > myPdfDocument.PageHeight - 50)
                    {
                        posY = 30;
                        currentYPos = 30;
                        newPdfPage.SaveToDocument();
                        //Adding new page and adding Header table,logo image and pageNo
                        newPdfPage = myPdfDocument.NewPage();
                    }
                    else
                    {
                        if (posY != currentYPos)
                            posY = 350;// currentYPos + 150;
                    }

                    if (myPdfDocument.PageHeight - posY < 150)
                    {
                        posY = 30;
                        currentYPos = 30;
                        newPdfPage.SaveToDocument();
                        //Adding new page and adding Header table,logo image and pageNo
                        newPdfPage = myPdfDocument.NewPage();
                    }

                    foreach (DataColumn dcol in parentDT.Columns)
                    {
                        switch (dcol.ColumnName.Trim())
                        {
                            case "Check Amount":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, amtColPosX + posX + 70, posY, 100, 50),
                                                                ContentAlignment.MiddleLeft, "Check Total : " + parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta);
                                    }
                                    break;
                                }
                            case "(Pay To) Name":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, posX, posY + 15, 100, 50),
                                                                ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta);
                                    }
                                    break;
                                }
                            case "Vendor":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, posX + 200, posY + 15, 100, 50),
                                                                ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta);
                                    }
                                    break;
                                }
                            case "Check Date":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, 300, posY + 15, 100, 50),
                                                                ContentAlignment.MiddleLeft, dcol.ColumnName.Trim() + " : " + parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta);

                                        PdfTextArea pta1 = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, 350, posY + 45, 100, 50),
                                                                ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta1);
                                    }
                                    break;
                                }
                            case "Check Number":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        double chkNoWidth = (width - amountColWidth - 115) - (500);
                                        string star = " ";


                                        for (int strCnt = 0; strCnt < Convert.ToInt32(chkNoWidth / 4) - (parentDT.Rows[0][dcol].ToString().Length); strCnt++)
                                        {
                                            star += "*";
                                        }

                                        //"***********************"

                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, 500, posY + 45, 100, 50),
                                                                ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                        newPdfPage.Add(pta);

                                        //width - amountColWidth - 115

                                        PdfTextArea pta1 = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, 500 + (parentDT.Rows[0][dcol].ToString().Length * 5), posY + 45, 100, 50),
                                                                ContentAlignment.MiddleLeft, star + parentDT.Rows[0]["Check Number"].ToString());
                                        newPdfPage.Add(pta1);
                                    }
                                    break;
                                }
                            case "Written Amount":
                                {
                                    if (parentDT.Rows[0]["Check Amount"] != null)
                                    {
                                        //Converting the amount into decimal
                                        Decimal decimalNo = Convert.ToDecimal(parentDT.Rows[0]["Check Amount"]);
                                        long wholeNumber = Convert.ToInt64(decimalNo);//Getting the whole no portion of the number
                                        double decimalPortion = Convert.ToDouble(decimalNo - Convert.ToDecimal(wholeNumber));//getting the decimal portion of the number
                                        string decimalPart = string.Empty;
                                        //Formatting the decimal portion
                                        //if (decimalPortion != 0)
                                        {
                                            decimalPart = " AND " + decimalPortion.ToString().Trim() + "/100";
                                        }
                                        //Converting the number into words
                                        // string noInWords = ConvertNumberToWord(wholeNumber).Trim().ToUpper() + decimalPart + " DOLLARS";
                                        string noInWords = m_ObjCommonUI.SpellDecimal(Convert.ToDecimal(wholeNumber)).Trim().ToUpper() + decimalPart + " DOLLARS";

                                        PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, 300, posY + 60, 500, 100),
                                                                ContentAlignment.MiddleLeft, noInWords);
                                        newPdfPage.Add(pta);
                                    }
                                    break;
                                }
                            case "Sent To:":
                                {
                                    if (parentDT.Rows[0][dcol] != null)
                                    {
                                        if (parentDT.Rows[0]["(Pay To) Name"] != null)
                                        {
                                            PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                    Color.Black, new PdfArea(myPdfDocument, posX + 40, posY + 75, 150, 100),
                                                                    ContentAlignment.MiddleLeft, parentDT.Rows[0]["(Pay To) Name"].ToString());
                                            newPdfPage.Add(pta);
                                        }
                                        string[] strarr = parentDT.Rows[0][dcol].ToString().Split('~');

                                        double addPosY = posY + 75;

                                        for (int arrLgh = 0; arrLgh < strarr.Length; arrLgh++)
                                        {
                                            addPosY += 10;
                                            PdfTextArea pta1 = new PdfTextArea(FontRegular,
                                                                Color.Black, new PdfArea(myPdfDocument, posX + 40, addPosY, 150, 100),
                                                                ContentAlignment.MiddleLeft, strarr[arrLgh].ToString());
                                            newPdfPage.Add(pta1);

                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    //Printing the Check total in words if column is not being sent
                    if (!parentDT.Columns.Contains("Written Amount"))
                    {
                        if (parentDT.Columns.Contains("Check Amount"))
                        {
                            if (parentDT.Rows[0]["Check Amount"] != null)
                            {
                                //Converting the amount into decimal
                                Decimal decimalNo = Convert.ToDecimal(parentDT.Rows[0]["Check Amount"]);
                                long wholeNumber = Convert.ToInt64(decimalNo);//Getting the whole no portion of the number
                                double decimalPortion = Convert.ToDouble(decimalNo - Convert.ToDecimal(wholeNumber));//getting the decimal portion of the number
                                string decimalPart = string.Empty;
                                //Formatting the decimal portion
                                //if (decimalPortion != 0)
                                {
                                    decimalPart = " AND " + decimalPortion.ToString().Trim() + "/100";
                                }
                                //Converting the number into words
                                //string noInWords = ConvertNumberToWord(wholeNumber).Trim().ToUpper() + decimalPart + " DOLLARS";
                                string noInWords = m_ObjCommonUI.SpellDecimal(Convert.ToDecimal(wholeNumber)).Trim().ToUpper() + decimalPart + " DOLLARS";
                                PdfTextArea pta = new PdfTextArea(FontRegular,
                                                        Color.Black, new PdfArea(myPdfDocument, 300, posY + 60, 500, 100),
                                                        ContentAlignment.MiddleLeft, noInWords);
                                newPdfPage.Add(pta);
                            }
                        }
                    }
                }

                newPdfPage.SaveToDocument();

                //Deleting the printed row from parentDT
                if (tableLayout.ToUpper().Trim() == "NORMAL")
                {
                    #region NLog
                    logger.Debug("Deleting the printed row from the parent datatable because the  table layout is NORMAL.");
                    #endregion
                    
                    parentDT.Rows[0].Delete();
                    //Adding TrxID column
                    if (!parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Add("TrxID");
                    }
                }
            }


            SaveToResponse(fileName);



        }

        #endregion

        #region ARInvoiceExpToPDF

        private void ARInvoiceExpToPDF(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes, XmlDocument xDocOut, string tableLayout)
        {

            //Get the treeNodeName
            string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            //Getting the columns to be displayed in grid
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
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
            }

            //Removing the Summed row from parent table
            if (isSumExists)
            {
                #region NLog
                logger.Debug("Removing the summed row from the parent table."); 
                #endregion

                dt.Rows.RemoveAt(dt.Rows.Count - 1);
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

            PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);

            double posX = 20;//50
            double posY = 70;//90
            double width = myPdfDocument.PageWidth - 50;// 690; 
            double height = myPdfDocument.PageHeight - 50;// 250;
            double currentYPos = 70;

            Font FontBold = new Font("Verdana", 15, FontStyle.Bold);
            Font FontRegular = new Font("Verdana", 11, FontStyle.Regular);

            foreach (DataRow dRow in dt.Rows)
            {
                PdfPage newPdfPage = myPdfDocument.NewPage();

                posY = 70;
                currentYPos = 70;

                //Adding each row at a time
                DataRow dNewRow = parentDT.NewRow();
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    dNewRow[dt.Columns[col].ColumnName] = dRow[dt.Columns[col].ColumnName];
                }
                parentDT.Rows.Add(dNewRow);

                string parentTrxID = string.Empty;
                parentTrxID = parentDT.Rows[0]["TrxID"].ToString();

                //Removing TrxID column
                if (parentDT.Columns.Contains("TrxID"))
                {
                    parentDT.Columns.Remove("TrxID");
                }

                //Printing Parent DT                   
                //while (!myPdfTable.AllTablePagesCreated)
                if (parentDT.Rows.Count > 0)
                {
                    foreach (DataColumn dcol in parentDT.Columns)
                    {
                        switch (dcol.ColumnName.Trim())
                        {
                            case "Image":
                                {
                                    string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                                    string imgSrc = "lajit - logos.JPG";// parentDT.Rows[0][dcol].ToString();                                   
                                    string imgpath = appDir + "App_Themes\\" + Theme + "\\Images \\lajit-logos.JPG";// +imgSrc;

                                    PdfImage LogoImage = myPdfDocument.NewImage(imgpath);
                                    newPdfPage.Add(LogoImage, width / 2, posY + 20, 96);
                                    //posY = posY + LogoImage.Height;
                                    break;
                                }
                            case "Customer":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX, posY + 20, 200, 50),
                                                            ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString().ToUpper().Trim());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Address":
                                {
                                    string[] strarr = parentDT.Rows[0][dcol].ToString().Split('~');

                                    double addPosY = posY + 20;

                                    for (int arrLgh = 0; arrLgh < strarr.Length; arrLgh++)
                                    {
                                        addPosY += 15;
                                        PdfTextArea pta1 = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX, addPosY, 200, 50),
                                                            ContentAlignment.MiddleLeft, strarr[arrLgh].ToString());
                                        newPdfPage.Add(pta1);
                                    }
                                    break;
                                }
                            case "Attention":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX, posY + 120, 200, 50),
                                                            ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }

                            case "Invoice Date":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 20, 100, 50),
                                                            ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Invoice Number":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 40, 150, 50),
                                                            ContentAlignment.MiddleLeft, "Invoice No :     " + parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Our Job#":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 60, 150, 50),
                                                            ContentAlignment.MiddleLeft, dcol.ColumnName.Trim() + " :     " + parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Agency No:":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 80, 150, 50),
                                                            ContentAlignment.MiddleLeft, dcol.ColumnName.Trim() + " :     " + parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Agency PO#:":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 100, 150, 50),
                                                            ContentAlignment.MiddleLeft, dcol.ColumnName.Trim() + " :     " + parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            case "Title":
                                {
                                    PdfTextArea pta = new PdfTextArea(FontBold,
                                                            Color.Black, new PdfArea(myPdfDocument, (width / 2) - 100, posY + 170, 300, 50),
                                                            ContentAlignment.MiddleLeft, parentDT.Rows[0][dcol].ToString());
                                    newPdfPage.Add(pta);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }

                //Printing Line Break
                //posY = posY + 150;
                Point pStart = new Point(10, Convert.ToInt32(posY + 160));
                Point pEnd = new Point(Convert.ToInt32(width), Convert.ToInt32(posY + 160));
                PdfLine pdfLineBrk = new PdfLine(myPdfDocument, pStart, pEnd, Color.Black, 1);
                newPdfPage.Add(pdfLineBrk);

                currentYPos = posY + 200;

                //Printing BranchDT
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        //if (nodeBranch.Attributes["ControlType"] == null)//Need to check
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                            DataTable BranchDT = new DataTable();
                            BranchDT = GetBranchDataToPrint(xDocOut, parentTrxID, branchNodeName);

                            if (BranchDT.Rows.Count > 0)
                            {
                                bool sumExists = false;

                                //Getting the columns to be displayed in grid
                                XmlNode nodeCols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                                foreach (DataColumn dBranchCol in BranchDT.Columns)
                                {
                                    XmlNode nodeBranchCol = nodeCols.SelectSingleNode("Col[@Caption = '" + dBranchCol.ColumnName + "']");

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
                                    }
                                }

                                //Removing the Summed row from branch table
                                if (sumExists)
                                {
                                    BranchDT.Rows.RemoveAt(BranchDT.Rows.Count - 1);
                                }

                                foreach (DataRow dBranchRow in BranchDT.Rows)
                                {
                                    //Setting the Y position and if required creating new page
                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo 
                                        newPdfPage = myPdfDocument.NewPage();
                                    }
                                    else
                                    {
                                        if (posY != currentYPos)
                                            posY = currentYPos + 20;
                                    }

                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                    {
                                        posY = 70;
                                        currentYPos = 70;
                                        newPdfPage.SaveToDocument();
                                        //Adding new page and adding Header table,logo image and pageNo 
                                        newPdfPage = myPdfDocument.NewPage();
                                    }

                                    foreach (DataColumn dcol in BranchDT.Columns)
                                    {
                                        switch (dcol.ColumnName.Trim())
                                        {
                                            case "Description":
                                                {
                                                    PdfTextArea pta = new PdfTextArea(FontBold,
                                                                            Color.Black, new PdfArea(myPdfDocument, posX + 70, posY, 350, 50),
                                                                            ContentAlignment.MiddleLeft, dBranchRow[dcol].ToString() + ":");
                                                    newPdfPage.Add(pta);
                                                    currentYPos = currentYPos + 20;
                                                    break;
                                                }
                                            case "Invoice Instructions":
                                                {
                                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                            Color.Black, new PdfArea(myPdfDocument, posX + 70, posY + 40, 100, 50),
                                                                            ContentAlignment.MiddleLeft, dBranchRow[dcol].ToString());
                                                    newPdfPage.Add(pta);
                                                    currentYPos = currentYPos + 40;
                                                    break;
                                                }
                                            case "Invoice Amount":
                                                {
                                                    PdfTextArea pta = new PdfTextArea(FontRegular,
                                                                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY, 100, 50),
                                                                            ContentAlignment.MiddleLeft, "$" + dBranchRow[dcol].ToString());
                                                    newPdfPage.Add(pta);
                                                    currentYPos = currentYPos + 20;
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                //Printing Amount Line Break                
                Point pAmtStart = new Point(Convert.ToInt32(posX + 600), Convert.ToInt32(posY + 80));
                Point pAmtEnd = new Point(Convert.ToInt32(posX + 650), Convert.ToInt32(posY + 80));
                PdfLine pdfAmtLineBrk = new PdfLine(myPdfDocument, pAmtStart, pAmtEnd, Color.Black, 1);
                newPdfPage.Add(pdfAmtLineBrk);

                //Printing Parent Invoice Amount
                if (parentDT.Columns.Contains("Invoice Amount"))
                {
                    PdfTextArea ptainvoice = new PdfTextArea(FontRegular,
                            Color.Black, new PdfArea(myPdfDocument, posX + 600, posY + 90, 100, 50),
                            ContentAlignment.MiddleLeft, "$" + parentDT.Rows[0]["Invoice Amount"].ToString());
                    newPdfPage.Add(ptainvoice);
                }

                //Printing Footer
                if (parentDT.Columns.Contains("Footer"))
                {
                    PdfTextArea ptafooter = new PdfTextArea(FontRegular,
                            Color.Black, new PdfArea(myPdfDocument, posX + 220, posY + 220, 100, 50),
                            ContentAlignment.MiddleLeft, parentDT.Rows[0]["Footer"].ToString());
                    newPdfPage.Add(ptafooter);
                }

                newPdfPage.SaveToDocument();

                //Deleting the printed row from parentDT
                if (tableLayout.ToUpper().Trim() == "NORMAL")
                {
                    #region NLog
                    logger.Debug("Deleting the printed row from the parent datatable because the  table layout is NORMAL.");
                    #endregion
                    
                    parentDT.Rows[0].Delete();
                    //Adding TrxID column
                    if (!parentDT.Columns.Contains("TrxID"))
                    {
                        parentDT.Columns.Add("TrxID");
                    }
                }
            }
            SaveToResponse(fileName);

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

        #region  GENERATE HTML
        public string GenerateHTML(XmlDocument xDocOut, bool saveFile, params string[] arrSelectedColumns)
        {
            string htmltext = string.Empty;
            DataTable dtGV = new DataTable();
            DataTable NotesDT1 = new DataTable();
            ArrayList arAllColumns = new ArrayList();
            Hashtable htColCaptions = new Hashtable();
            XmlNode nodeTreenode1 = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string m_treeNodeName = string.Empty;
            if (nodeTreenode1.ToString() != null)
            {
                if (nodeTreenode1.SelectSingleNode("Node") != null)
                {
                    m_treeNodeName = nodeTreenode1.SelectSingleNode("Node").InnerText;
                }
            }
            //newly added on 13-11-09
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");
            {
                for (int col = 0; col < arrSelectedColumns.Length; col++)
                {
                    XmlNode xnode = null;
                    if (m_CustReptHidden == 0)
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@FullViewLength!='0' and @Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    else
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    if (xnode != null)
                    {
                        XmlAttributeCollection xmlattr = xnode.Attributes;
                        if ((xmlattr["ControlType"] != null))
                        {
                            if (!string.IsNullOrEmpty(xmlattr["Caption"].Value.ToString()))
                            {
                                if (!htColCaptions.Contains(xmlattr["Caption"].Value.ToString()))
                                {
                                    htColCaptions.Add(xmlattr["Caption"].Value, xmlattr["Caption"].Value);
                                }
                            }
                        }
                    }
                }
            }
            //Get the Datatable to print
            dtGV = GetDataToPrint(xDocOut, m_treeNodeName, "HTML");
            Hashtable htAllColumns = new Hashtable();
            if (dtGV.Rows.Count > 0)
            {
                if (dtGV.Columns.Contains("Notes"))
                {
                    NotesDT1 = reportsBO.GenerateNotesDatatable(dtGV);
                    dtGV.Columns.Remove("Notes");
                }
                for (int i = 0; i < dtGV.Columns.Count; i++)
                {
                    arAllColumns.Add(dtGV.Columns[i].ColumnName);
                }
                //NEED TO REMOVE NOT SELECTED COLUMNS
                if (htColCaptions.Count > 0)
                {
                    IDictionaryEnumerator enumerator = htColCaptions.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        arAllColumns.Remove(enumerator.Key.ToString());
                    }
                    for (int i = 0; i < arAllColumns.Count; i++)
                    {
                        dtGV.Columns.Remove(arAllColumns[i].ToString());
                    }
                }
            }
            if (dtGV.Rows.Count > 0)
            {
                #region VARIABLES
                string treeNodeName = string.Empty;
                string reportStyle = string.Empty;
                string titleName = string.Empty;

                XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                clsReportsUI objReportsUI = new clsReportsUI();
                treeNodeName = nodeTreenode.FirstChild.InnerText;
                int isPrint = Convert.ToInt32(xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value);
                if (isPrint == 1)
                {
                    XmlNode xReportStyleNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree");
                    if (nodeTreenode.Attributes["ReportStyle"] == null)
                    {
                        XmlAttribute attrReportStyle = xDocOut.CreateAttribute("ReportStyle");
                        xReportStyleNode.Attributes.Append(attrReportStyle);
                        //Check for the web.config key as the final override
                        if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[@ControlType='GView']") != null)
                        {
                            attrReportStyle.Value = "5";
                        }
                        else
                        {
                            attrReportStyle.Value = "1";
                        }
                    }
                }
                titleName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/PageTitle").InnerText;
                string fileName = string.Empty;
                if (titleName.Contains("(") || titleName.Contains(" "))
                {
                    fileName = titleName.Split('(').GetValue(0).ToString();
                    fileName = titleName.Replace(" ", "_");
                }
                else
                {
                    fileName = titleName;
                }
                #endregion
                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    reportStyle = nodeTreenode.Attributes["ReportStyle"].Value.ToString();
                }
                else
                {
                    reportStyle = "";
                }
                rptHTML objHTML = new rptHTML();
                string[] printType = new string[1];
                printType[0] = "inline";
                htmltext = objHTML.CreateHTMLTemplate(xDocOut, dtGV, reportStyle, fileName, "inline");
            }
            return htmltext;
        }

        public void SaveHTMLFile(string htmlText, string fileName)
        {
            if (htmlText != string.Empty)
            {
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/html";
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("attachment;filename=" + fileName + ".HTML"));
                HttpContext.Current.Response.Write(htmlText);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
        }

        public void zipfiles(string zipfilename, string createdFileNames, string[] filenames)
        {
            try
            {
                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string zipPath = string.Empty;
                //zipPath = appDir + ConfigurationManager.AppSettings["TempFilePath"].ToString();
                zipPath = ConfigurationManager.AppSettings["TempFilePath"].ToString();
                if (zipfilename.Contains("("))
                {
                    zipfilename = zipfilename.Split('(').GetValue(0).ToString();
                    zipfilename = zipfilename.Trim();
                }
                if (!Directory.Exists(zipPath))
                {
                    Directory.CreateDirectory(zipPath);
                }
                else
                {
                    string[] files = Directory.GetFiles(zipPath);
                    foreach (string s in files)
                    {
                        if (createdFileNames != s)
                        {
                            string extn = Path.GetExtension(s);
                            switch (extn)
                            {
                                case ".zip":
                                    File.Delete(s);
                                    break;
                                case ".html":
                                    File.Delete(s);
                                    break;
                            }
                        }
                    }
                }
                string actualname = zipPath + "\\" + zipfilename + ".zip";
                using (ZipOutputStream s = new ZipOutputStream(File.Create(actualname)))
                {
                    s.SetLevel(9); // 0 - store only to 9 - means best compression
                    byte[] buffer = new byte[4096];
                    foreach (string file in filenames)
                    {
                        string extension = Path.GetExtension(file);
                        if (extension.ToString().Trim().ToUpper() != ".ZIP")
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            entry.DateTime = DateTime.Now;
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                    }
                    s.Finish();
                    s.Close();
                }

                FileInfo file1 = new FileInfo(actualname);
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + zipfilename + ".zip");
                HttpContext.Current.Response.WriteFile(file1.FullName);
                HttpContext.Current.Response.End();
                if (!Directory.Exists(zipPath))
                {
                    Directory.Delete(zipPath);
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                string k = ex.Message;
            }
        }
        #endregion

        #region Generate WORD
        public void GenerateWord(XmlDocument xDocOut, params string[] arrSelectedColumns)
        {
            DataTable NotesDT = new DataTable();
            DataTable dt = new DataTable();
            DataTable pNotesDT = new DataTable();
            ArrayList arAllColumns = new ArrayList();
            Hashtable htColCaptions = new Hashtable();
            XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string m_treeNodeName = string.Empty;
            if (nodeTreenode.ToString() != null)
            {
                if (nodeTreenode.SelectSingleNode("Node") != null)
                {
                    m_treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                }
            }
            //newly added on 13-11-09
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");
            {
                for (int col = 0; col < arrSelectedColumns.Length; col++)
                {
                    XmlNode xnode = null;
                    if (m_CustReptHidden == 0)
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@FullViewLength!='0' and @Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    else
                    {
                        xnode = nodeColumns.SelectSingleNode("Col[@Label='" + arrSelectedColumns[col].ToString() + "']");
                    }
                    if (xnode != null)
                    {
                        XmlAttributeCollection xmlattr = xnode.Attributes;
                        if ((xmlattr["ControlType"] != null))
                        {
                            if (!string.IsNullOrEmpty(xmlattr["Caption"].Value.ToString()))
                            {
                                if (!htColCaptions.Contains(xmlattr["Caption"].Value.ToString()))
                                {
                                    htColCaptions.Add(xmlattr["Caption"].Value, xmlattr["Caption"].Value);
                                }
                            }
                        }
                    }
                }
            }
            //newly added on 13-11-09
            //Get the Datatable to print
            dt = GetDataToPrint(xDocOut, m_treeNodeName, "WORD");
            Hashtable htAllColumns = new Hashtable();
            if (dt.Rows.Count > 0)
            {
                //Removing TrxID column
                if (dt.Columns.Contains("TrxID"))
                {
                    dt.Columns.Remove("TrxID");
                }
                if (dt.Columns.Contains("Notes"))
                {
                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    dt.Columns.Remove("Notes");
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    arAllColumns.Add(dt.Columns[i].ColumnName);
                }
                //NEED TO REMOVE NOT SELECTED COLUMNS
                if (htColCaptions.Count > 0)
                {
                    IDictionaryEnumerator enumerator = htColCaptions.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        arAllColumns.Remove(enumerator.Key.ToString());
                    }
                    for (int i = 0; i < arAllColumns.Count; i++)
                    {
                        dt.Columns.Remove(arAllColumns[i].ToString());
                    }
                }
                //Export to Excel
                ExportDatatable(dt, "WordRpt", NotesDT, "WORD");
            }
        }
        #endregion
    }
}
