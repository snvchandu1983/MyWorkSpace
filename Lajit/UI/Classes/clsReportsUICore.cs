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
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using NLog;


namespace LAjitDev
{
    public class clsReportsUICore
    {
        public static int m_CustReptHidden = Convert.ToInt32(ConfigurationManager.AppSettings["CustReptHidden"].ToString());
        #region Convert To ArrayColumns
        public static DataTable ConvertToArrayColumns(XmlNode nodeColumns, string treeType, string treeNodeName, string parentNodeName, out Hashtable htColFormats, out Hashtable htColNameValues, out int[] colWidths, params string[] arrSelectedColumn)
        {

            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Converting the XML mode columns to Array Columns with tree type as : "+treeType+ " and parent node name as : "+parentNodeName ); 
            #endregion

            int columsCount = nodeColumns.ChildNodes.Count;
            int attributeCount = nodeColumns.ChildNodes[0].Attributes.Count;
            int noOfFVCols = nodeColumns.SelectNodes("Col[@FullViewLength!='0']").Count;//Taking FullViewLength !=0 Columns
            htColFormats = new Hashtable();
            htColNameValues = new Hashtable();
            //
            XmlNodeList colnodelist = nodeColumns.SelectNodes("Col[@FullViewLength!='0']");
            DataTable dtMain = new DataTable();
            colWidths = null;
            ArrayList alWidths = new ArrayList();
            try
            {
                int widthcount = 0;
                dtMain.Columns.Add("TrxID");
                htColNameValues.Add("TrxID", "TrxID");
                switch (treeType)
                {
                    case "Parent":
                        {
                            //colWidths = new int[colnodelist.Count + 1];
                            //colWidths = new int[1];
                            //colWidths[0] = 1;
                            //widthcount = 1;
                            alWidths.Add(1);
                            break;
                        }
                    case "Branch":
                    case "GView":
                        {
                            //colWidths = new int[colnodelist.Count + 2];
                            //colWidths = new int[2];
                            //colWidths[0] = 1;
                            //colWidths[1] = 1;
                            //widthcount = 2;
                            alWidths.Add(1);
                            alWidths.Add(2);
                            dtMain.Columns.Add(parentNodeName + "_TrxID");
                            htColNameValues.Add(parentNodeName + "_TrxID", parentNodeName + "_TrxID");
                            break;
                        }
                }
                if ((arrSelectedColumn != null) && (treeType == "Parent"))
                {
                    if (arrSelectedColumn.Length > 0)
                    {
                        for (int col = 0; col < arrSelectedColumn.Length; col++)
                        {
                            XmlNode xnode = null;
                            if (m_CustReptHidden == 0)
                            {
                                xnode = nodeColumns.SelectSingleNode("Col[@FullViewLength!='0' and @Label='" + arrSelectedColumn[col].ToString() + "']");
                            }
                            else
                            {
                                xnode = nodeColumns.SelectSingleNode("Col[@Label='" + arrSelectedColumn[col].ToString() + "']");
                            }

                            if (xnode != null)
                            {
                                XmlAttributeCollection xmlattr = xnode.Attributes;
                                if ((xmlattr["ControlType"] != null))
                                {
                                    AddColumnsToDataTable(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int colNode = 0; colNode < colnodelist.Count; colNode++)
                        {
                            XmlAttributeCollection xmlattr = colnodelist[colNode].Attributes;
                            if (xmlattr["ControlType"] != null)
                            {
                                AddColumnsToDataTable(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                            }
                        }
                        //CHECK LINK ATTRIBUTES
                        string[] arrlinks ={ "Link1", "Link2", "Link3" };
                        for (int i = 0; i < arrlinks.Length; i++)
                        {
                            if (!dtMain.Columns.Contains(arrlinks[i]))
                            {
                                XmlNode nodeLink = nodeColumns.SelectSingleNode("Col[@Label='" + arrlinks[i] + "']");
                                if (nodeLink != null)
                                {
                                    XmlAttributeCollection xmlattr = nodeLink.Attributes;
                                    if (xmlattr["ControlType"] != null)
                                    {
                                        AddColumnsToDataTable(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {

                    for (int colNode = 0; colNode < colnodelist.Count; colNode++)
                    {
                        XmlAttributeCollection xmlattr = colnodelist[colNode].Attributes;
                        if (xmlattr["ControlType"] != null)
                        {
                            AddColumnsToDataTable(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                        }
                    }
                }
                if (!dtMain.Columns.Contains("Link1"))
                {
                    XmlNode nodeLink1 = nodeColumns.SelectSingleNode("Col[@Label='Link1']");
                    if (nodeLink1 != null)
                    {
                        XmlAttributeCollection xmlattr = nodeLink1.Attributes;
                        if (xmlattr["ControlType"] != null)
                        {
                            AddColumnsToDataTable(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
            colWidths = (int[])alWidths.ToArray(typeof(int));
            return dtMain;
        }

        private static void AddColumnsToDataTable(Hashtable htColFormats, Hashtable htColNameValues, ArrayList alWidths, DataTable dtMain, XmlAttributeCollection xmlattr)
        {
            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Adding the columns to the DataTable");
            #endregion

            if (Convert.ToInt32(xmlattr["Caption"].Value.Length + 5) > Convert.ToInt32(xmlattr["FullViewLength"].Value))
            {
                alWidths.Add(Convert.ToInt32(xmlattr["Caption"].Value.Length + 5));
            }
            else
            {
                alWidths.Add(Convert.ToInt32(xmlattr["FullViewLength"].Value));
            }
            dtMain.Columns.Add(xmlattr["Caption"].Value.ToString());
            htColNameValues.Add(xmlattr["Label"].Value.ToString(), xmlattr["Caption"].Value.ToString());
            switch (xmlattr["ControlType"].Value)
            {
                case "Amount":
                case "Calc":
                    {
                        if (xmlattr["IsSummed"] != null)
                        {
                            if (xmlattr["IsSummed"].Value.ToString() == "1")
                            {
                                htColFormats.Add(xmlattr["Label"].Value.ToString(), "SUMMED");
                            }
                            else
                            {
                                htColFormats.Add(xmlattr["Label"].Value.ToString(), "AMOUNT");
                            }
                        }
                        else
                        {
                            htColFormats.Add(xmlattr["Label"].Value.ToString(), "AMOUNT");
                        }
                        break;
                    }
                case "Cal":
                    {
                        htColFormats.Add(xmlattr["Label"].Value.ToString(), "DATE");
                        break;
                    }
                case "Check":
                    {
                        htColFormats.Add(xmlattr["Label"].Value.ToString(), "CHECK");
                        break;
                    }
            }
        }
        #endregion

        #region Convert To Data Table
        public static DataTable ConvertToDataTable(DataTable dtMain, string strNodeType, XmlNode nodeRowList, Hashtable htColFormats, Hashtable htColNameValues, int[] colWidths, bool trimStatus, out int[] arrayWidth, out Hashtable htPFormats, out bool PLayout)
        {
            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Converting XML NodeRowlist to a DataTable");
            #endregion

            dtMain.TableName = nodeRowList.ParentNode.LocalName.ToString();
            arrayWidth = null;
            PLayout = new bool();
            htPFormats = new Hashtable();
            string strCustom = string.Empty;
            int ID = 0;
            string strTotalAmount = string.Empty;
            double TotalAmount = 0;
            int colFVL = 0;
            Hashtable htSumCols = new Hashtable();
            List<string> lstFormats = new List<string>();
            try
            {
                for (int i = 0; i < nodeRowList.ChildNodes.Count; i++)
                {
                    dtMain.Rows.Add(dtMain.NewRow());
                    lstFormats = new List<string>();
                    foreach (XmlAttribute attr in nodeRowList.ChildNodes[i].Attributes)
                    {
                        if (attr != null)
                        {
                            string colName = Convert.ToString(attr.Name);
                            string colValue = Convert.ToString(attr.Value).Trim();
                            if (htColFormats.ContainsKey(colName))
                            {
                                string colLabelName = htColFormats[colName].ToString();
                                string colCaptionName = htColNameValues[colName].ToString();
                                switch (colLabelName)
                                {
                                    case "CHECK":
                                        {
                                            strCustom = string.Empty;
                                            if (colValue == "1")
                                            {
                                                strCustom = "x";
                                                if (dtMain.Columns.Contains(colCaptionName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colCaptionName] = strCustom;
                                                }
                                            }
                                            else
                                            {
                                                strCustom = "";
                                                if (dtMain.Columns.Contains(colCaptionName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colCaptionName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "DATE":
                                        {
                                            strCustom = string.Empty;
                                            DateTime dateTime;
                                            if (DateTime.TryParse(colValue, out dateTime))
                                            {
                                                strCustom = dateTime.ToString("MM/dd/yy");
                                                if (dtMain.Columns.Contains(colCaptionName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colCaptionName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "AMOUNT":
                                        {
                                            strCustom = string.Empty;
                                            decimal amount;
                                            if (Decimal.TryParse(colValue, out amount))
                                            {
                                                string amt = string.Format("{0:N}", amount);
                                                strCustom = amt;
                                                lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[colCaptionName].Ordinal));
                                                if (dtMain.Columns.Contains(colCaptionName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colCaptionName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "SUMMED":
                                        {
                                            strCustom = string.Empty;
                                            decimal amount;
                                            TotalAmount = 0;
                                            if (Decimal.TryParse(colValue, out amount))
                                            {
                                                string amt = string.Format("{0:N}", amount);
                                                strCustom = amt;
                                                lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[colCaptionName].Ordinal) + "~BOLD");
                                                if (dtMain.Columns.Contains(colCaptionName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colCaptionName] = strCustom;
                                                    if (!htSumCols.ContainsKey(colCaptionName))
                                                    {
                                                        TotalAmount = TotalAmount + Convert.ToDouble(strCustom);
                                                        strTotalAmount = Convert.ToString(TotalAmount);
                                                        htSumCols.Add(colCaptionName, Convert.ToString(strTotalAmount));
                                                    }
                                                    else
                                                    {
                                                        TotalAmount = Convert.ToDouble(htSumCols[colCaptionName].ToString());
                                                        TotalAmount = TotalAmount + Convert.ToDouble(strCustom);
                                                        strTotalAmount = Convert.ToString(TotalAmount);
                                                        htSumCols[colCaptionName] = strTotalAmount;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                if (htColNameValues.ContainsKey(colName))
                                {
                                    string colLabelName = htColNameValues[colName].ToString();
                                    switch (colLabelName)
                                    {
                                        case "TrxID":
                                            {
                                                lstFormats.Add("TrxID~" + colValue);
                                                if (dtMain.Columns.Contains(colLabelName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                if (colLabelName.Contains("TrxID"))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                }
                                                else if (dtMain.Columns.Contains(colLabelName))
                                                {
                                                    switch (colLabelName)
                                                    {
                                                        case "Link1":
                                                        case "Link2":
                                                        case "Link3":
                                                            {
                                                                string[] splittedColValue = new string[colValue.Split('-').Length];
                                                                if ((bool)trimStatus)
                                                                {
                                                                    colFVL = colWidths[dtMain.Columns[colLabelName].Ordinal];
                                                                    if (colValue.Contains("-"))
                                                                    {
                                                                        splittedColValue = colValue.Split('-');
                                                                    }
                                                                    if (splittedColValue.Length > 1)
                                                                    {
                                                                        if (splittedColValue[0].Length > colFVL)
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].Remove(colFVL - 3) + "...";
                                                                        }
                                                                        else
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].ToString();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (colValue.Length > colFVL)
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue.Remove(colFVL - 3) + "...";
                                                                        }
                                                                        else
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (colValue.Length > colFVL)
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                //Remaining columns
                                                                if ((bool)trimStatus)
                                                                {
                                                                    //dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                    colFVL = colWidths[dtMain.Columns[colLabelName].Ordinal];
                                                                    if (colValue.Length > colFVL)
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue.Remove(colFVL - 3) + "...";
                                                                    }
                                                                    else
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    switch (colName)
                                    {
                                        case "pLayout":
                                            {
                                                if (colValue == "1") { if ((bool)PLayout == false) { PLayout = true; } }
                                                break;
                                            }
                                        case "pFont":
                                        case "pUnderLine":
                                        case "pBox":
                                            {
                                                if (colValue == "1")
                                                {
                                                    lstFormats.Add(colName);
                                                }
                                                break;
                                            }
                                        case "pLnSkip":
                                            {
                                                if (colValue == "1")
                                                {
                                                    lstFormats.Add(colName);
                                                    dtMain.Rows.Add(dtMain.NewRow());
                                                    for (int col = 0; col < dtMain.Columns.Count; col++)
                                                    {
                                                        dtMain.Rows[dtMain.Rows.Count - 1][dtMain.Columns[col].ToString()] = "SKIP";
                                                    }
                                                    if (!htPFormats.ContainsKey(ID))
                                                    {
                                                        lstFormats.Add("~");
                                                        htPFormats.Add(ID, lstFormats.ToArray());
                                                        ID++;
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                    htPFormats.Add(ID, lstFormats.ToArray());
                    ID++;
                }
                arrayWidth = new int[dtMain.Columns.Count];
                if (strTotalAmount != string.Empty)
                {
                    dtMain.Rows.Add(dtMain.NewRow());
                    IDictionaryEnumerator enumerator = htSumCols.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        decimal amount;
                        Decimal.TryParse(enumerator.Value.ToString(), out amount);
                        string amt = string.Format("{0:N}", amount);

                        dtMain.Rows[dtMain.Rows.Count - 1][enumerator.Key.ToString()] = amt;
                        lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[enumerator.Key.ToString()].Ordinal) + "~BOLD");
                    }
                    htPFormats.Add(ID, lstFormats.ToArray());
                }
                dtMain = WrapFullViewLength(dtMain, colWidths, out arrayWidth);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion

                throw ex;
            }
            return dtMain;
        }
        #endregion

        private static void AddColumns(DataTable dtMain, string[,] arrColumns)
        {
            int arrLen = arrColumns.GetUpperBound(0);

            for (int i = 0; i <= arrLen; i++)
            {
                string[] columnNames = arrColumns[i, 0].Split('~');
                string colLabel = string.Empty;
                string colCaption = string.Empty;
                string colSortOrder = string.Empty;
                if (columnNames.Length > 1)
                {
                    colLabel = columnNames[0];
                    colCaption = columnNames[1];
                    colSortOrder = columnNames[2];
                }
                switch (arrColumns[i, 0])
                {
                    case "pLayout":
                    case "pFont":
                    case "pUnderLine":
                    case "pBox":
                    case "pLnSkip":
                    case "pPgBreak":
                        {
                            break;
                        }
                    default:
                        {
                            if (!dtMain.Columns.Contains(colCaption))
                            {
                                dtMain.Columns.Add(colCaption);
                            }
                            break;
                        }
                }

            }

        }


        #region Set Ordinals
        /// <summary>
        /// Set DataTable Ordinals based on SortOrder
        /// </summary>
        /// <param name="dtMain">DataTable</param>
        /// <param name="arrColumns">Two dimensional array of columns</param>
        /// <param name="ordPos">integer or ordinalposition to look on arrColumns</param>
        /// <param name="sortPos"></param>
        private static void SetOrdinals(DataTable dtMain, string[,] arrColumns, int ordPos, int sortPos)
        {
            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Set DataTable Ordinals based on order position as : "+ordPos+" and with sort position as : "+sortPos);
            #endregion

            int arrLen = arrColumns.GetUpperBound(0);

            int calSort = 0;
            int prevSort = 0;
            for (int i = ordPos; i <= arrLen; i++)
            {
                string[] columnNames = arrColumns[i, 0].Split('~');
                string colLabel = string.Empty;
                string colCaption = string.Empty;
                string colSortOrder = string.Empty;
                int ordinal = 0;
                if (columnNames.Length > 2)
                {
                    colLabel = columnNames[0];
                    colCaption = columnNames[1];
                    colSortOrder = columnNames[2];
                }
                if (dtMain.Columns.Contains(colCaption))
                {
                    ordinal = Convert.ToInt32(columnNames[2].ToString()); //+sortPos;
                    ordinal = ordinal - calSort;
                    if (ordinal < dtMain.Columns.Count)
                    {
                        dtMain.Columns[colCaption].SetOrdinal(ordinal);
                        prevSort = ordinal;
                    }
                    else
                    {

                        prevSort += 1;
                        if (prevSort < dtMain.Columns.Count)
                        {
                            dtMain.Columns[colCaption].SetOrdinal(prevSort);
                        }

                    }
                }
                else
                {
                    calSort += 1;
                }
            }
        }
        #endregion

        #region Add Column
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtMain"></param>
        /// <param name="i"></param>
        /// <param name="colLabel"></param>
        /// <param name="colCaption"></param>
        /// <param name="attColValue"></param>
        private static void AddColumn(DataTable dtMain, string colLabel, string colCaption, string attColValue)
        {
            if (!string.IsNullOrEmpty(colCaption))
            {
                if (!dtMain.Columns.Contains(colCaption))
                {
                    dtMain.Columns.Add(colCaption);
                }
                dtMain.Rows[dtMain.Rows.Count - 1][colCaption] = attColValue;
            }
            else
            {
                if (!dtMain.Columns.Contains(colLabel))
                {
                    dtMain.Columns.Add(colLabel);
                }
                dtMain.Rows[dtMain.Rows.Count - 1][colLabel] = attColValue;
            }
        }
        #endregion

        #region Wrap Full View Length
        public static DataTable WrapFullViewLength(DataTable dt, int[] arrayWidth, out int[] colWidths)
        {
            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Wrapping Full view length in to Specific FullView Length");
            #endregion

            colWidths = new int[dt.Columns.Count];
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    int drLength = dt.Rows[row][col].ToString().Length;
                    int arrLength = Convert.ToInt32(arrayWidth[col].ToString());
                    arrLength = arrLength;
                    if (drLength != 0)
                    {
                        if (drLength > arrLength)
                        {
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
                            colWidths[col] = arrLength;
                        }
                        else
                        {
                            colWidths[col] = arrLength;
                        }
                    }
                    else
                    {
                        colWidths[col] = arrLength;
                    }
                }
            }
            return dt;
        }
        #endregion

        #region Convert To CurrencyFormat
        public static String ConvertToCurrencyFormat(string strAmount)
        {
            string stramount = string.Empty;
            decimal amount;
            stramount = Convert.ToString(strAmount);
            Decimal.TryParse(stramount, out amount);
            stramount = string.Format("{0:N}", amount);
            return stramount;
        }
        #endregion

        #region Convert To ArrayColumns1
        public static DataTable ConvertToArrayColumns1(XmlNode nodeColumns, string treeType, string treeNodeName, string parentNodeName, out Hashtable htColFormats, out Hashtable htColNameValues, out int[] colWidths, params string[] arrSelectedColumn)
        {
            int columsCount = nodeColumns.ChildNodes.Count;
            int attributeCount = nodeColumns.ChildNodes[0].Attributes.Count;
            int noOfFVCols = nodeColumns.SelectNodes("Col[@FullViewLength!='0']").Count;//Taking FullViewLength !=0 Columns
            htColFormats = new Hashtable();
            htColNameValues = new Hashtable();
            //
            XmlNodeList colnodelist = nodeColumns.SelectNodes("Col[@FullViewLength!='0']");
            DataTable dtMain = new DataTable();
            colWidths = null;
            ArrayList alWidths = new ArrayList();
            try
            {
                int widthcount = 0;
                dtMain.Columns.Add("TrxID");
                htColNameValues.Add("TrxID", "TrxID");
                switch (treeType)
                {
                    case "Parent":
                        {
                            alWidths.Add(1);
                            break;
                        }
                    case "Branch":
                    case "GView":
                        {
                            alWidths.Add(1);
                            alWidths.Add(2);
                            dtMain.Columns.Add(parentNodeName + "_TrxID");
                            htColNameValues.Add(parentNodeName + "_TrxID", parentNodeName + "_TrxID");
                            break;
                        }
                }
                if ((arrSelectedColumn != null) && (treeType == "Parent"))
                {
                    if (arrSelectedColumn.Length > 0)
                    {
                        for (int col = 0; col < arrSelectedColumn.Length; col++)
                        {
                            XmlNode xnode = null;
                            if (m_CustReptHidden == 0)
                            {
                                xnode = nodeColumns.SelectSingleNode("Col[@FullViewLength!='0' and @Label='" + arrSelectedColumn[col].ToString() + "']");
                            }
                            else
                            {
                                xnode = nodeColumns.SelectSingleNode("Col[@Label='" + arrSelectedColumn[col].ToString() + "']");
                            }

                            if (xnode != null)
                            {
                                XmlAttributeCollection xmlattr = xnode.Attributes;
                                if ((xmlattr["ControlType"] != null))
                                {
                                    AddColumnsToDataTable1(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int colNode = 0; colNode < colnodelist.Count; colNode++)
                        {
                            XmlAttributeCollection xmlattr = colnodelist[colNode].Attributes;
                            if (xmlattr["ControlType"] != null)
                            {
                                AddColumnsToDataTable1(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                            }
                        }
                        //CHECK LINK ATTRIBUTES
                        string[] arrlinks ={ "Link1", "Link2", "Link3" };
                        for (int i = 0; i < arrlinks.Length; i++)
                        {
                            if (!dtMain.Columns.Contains(arrlinks[i]))
                            {
                                XmlNode nodeLink = nodeColumns.SelectSingleNode("Col[@Label='" + arrlinks[i] + "']");
                                if (nodeLink != null)
                                {
                                    XmlAttributeCollection xmlattr = nodeLink.Attributes;
                                    if (xmlattr["ControlType"] != null)
                                    {
                                        AddColumnsToDataTable1(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {

                    for (int colNode = 0; colNode < colnodelist.Count; colNode++)
                    {
                        XmlAttributeCollection xmlattr = colnodelist[colNode].Attributes;
                        if (xmlattr["ControlType"] != null)
                        {
                            AddColumnsToDataTable1(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                        }
                    }
                }
                if (!dtMain.Columns.Contains("Link1"))
                {
                    XmlNode nodeLink1 = nodeColumns.SelectSingleNode("Col[@Label='Link1']");
                    if (nodeLink1 != null)
                    {
                        XmlAttributeCollection xmlattr = nodeLink1.Attributes;
                        if (xmlattr["ControlType"] != null)
                        {
                            AddColumnsToDataTable1(htColFormats, htColNameValues, alWidths, dtMain, xmlattr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                NLog.Logger logger = LogManager.GetCurrentClassLogger();
                logger.Fatal(ex);
                #endregion
            }
            colWidths = (int[])alWidths.ToArray(typeof(int));
            return dtMain;
        }

        private static void AddColumnsToDataTable1(Hashtable htColFormats, Hashtable htColNameValues, ArrayList alWidths, DataTable dtMain, XmlAttributeCollection xmlattr)
        {
            if (Convert.ToInt32(xmlattr["Caption"].Value.Length + 5) > Convert.ToInt32(xmlattr["FullViewLength"].Value))
            {
                alWidths.Add(Convert.ToInt32(xmlattr["Caption"].Value.Length + 5));
            }
            else
            {
                alWidths.Add(Convert.ToInt32(xmlattr["FullViewLength"].Value));
            }
            dtMain.Columns.Add(xmlattr["Label"].Value.ToString());
            htColNameValues.Add(xmlattr["Label"].Value.ToString(), xmlattr["Caption"].Value.ToString());
            switch (xmlattr["ControlType"].Value)
            {
                case "Amount":
                case "Calc":
                    {
                        if (xmlattr["IsSummed"] != null)
                        {
                            if (xmlattr["IsSummed"].Value.ToString() == "1")
                            {
                                htColFormats.Add(xmlattr["Label"].Value.ToString(), "SUMMED");
                            }
                            else
                            {
                                htColFormats.Add(xmlattr["Label"].Value.ToString(), "AMOUNT");
                            }
                        }
                        else
                        {
                            htColFormats.Add(xmlattr["Label"].Value.ToString(), "AMOUNT");
                        }
                        break;
                    }
                //case "TBox":
                //    {
                //        if (xmlattr["IsSummed"] != null)
                //        {
                //            if (xmlattr["IsSummed"].Value.ToString() == "1")
                //            {
                //                htColFormats.Add(xmlattr["Label"].Value.ToString(), "TXTSUMMED");
                //            }
                //        }
                //        break;
                //    }
                case "Cal":
                    {
                        htColFormats.Add(xmlattr["Label"].Value.ToString(), "DATE");
                        break;
                    }
                case "Check":
                    {
                        htColFormats.Add(xmlattr["Label"].Value.ToString(), "CHECK");
                        break;
                    }
            }
        }

        #endregion

        #region Convert To Data Table1
        public static DataTable ConvertToDataTable1(DataTable dtMain, string strNodeType, XmlNode nodeRowList, Hashtable htColFormats, Hashtable htColNameValues, int[] colWidths, bool trimStatus, out int[] arrayWidth, out Hashtable htPFormats, out bool PLayout)
        {
            dtMain.TableName = nodeRowList.ParentNode.LocalName.ToString();
            arrayWidth = null;
            PLayout = new bool();
            htPFormats = new Hashtable();
            string strCustom = string.Empty;
            int ID = 0;
            string strTotalAmount = string.Empty;
            double TotalAmount = 0;
            int colFVL = 0;
            Hashtable htSumCols = new Hashtable();
            List<string> lstFormats = new List<string>();
            try
            {
                for (int i = 0; i < nodeRowList.ChildNodes.Count; i++)
                {
                    dtMain.Rows.Add(dtMain.NewRow());
                    lstFormats = new List<string>();
                    foreach (XmlAttribute attr in nodeRowList.ChildNodes[i].Attributes)
                    {
                        if (attr != null)
                        {
                            string colName = Convert.ToString(attr.Name);
                            string colValue = Convert.ToString(attr.Value).Trim();
                            if (htColFormats.ContainsKey(colName))
                            {
                                string colLabelName = htColFormats[colName].ToString();
                                string colCaptionName = htColNameValues[colName].ToString();

                                switch (colLabelName)
                                {
                                    case "CHECK":
                                        {
                                            strCustom = string.Empty;
                                            if (colValue == "1")
                                            {
                                                strCustom = "x";
                                                if (dtMain.Columns.Contains(colName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = strCustom;
                                                }
                                            }
                                            else
                                            {
                                                strCustom = "";
                                                if (dtMain.Columns.Contains(colName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "DATE":
                                        {
                                            strCustom = string.Empty;
                                            DateTime dateTime;
                                            if (DateTime.TryParse(colValue, out dateTime))
                                            {
                                                strCustom = dateTime.ToString("MM/dd/yy");
                                                if (dtMain.Columns.Contains(colName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "AMOUNT":
                                        {
                                            strCustom = string.Empty;
                                            decimal amount;
                                            if (Decimal.TryParse(colValue, out amount))
                                            {
                                                string amt = string.Format("{0:N}", amount);
                                                strCustom = amt;
                                                lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[colName].Ordinal));
                                                if (dtMain.Columns.Contains(colName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = strCustom;
                                                }
                                            }
                                            break;
                                        }
                                    case "SUMMED":
                                        {
                                            strCustom = string.Empty;
                                            decimal amount;
                                            TotalAmount = 0;
                                            if (Decimal.TryParse(colValue, out amount))
                                            {
                                                string amt = string.Format("{0:N}", amount);
                                                strCustom = amt;
                                                lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[colName].Ordinal) + "~BOLD");
                                                if (dtMain.Columns.Contains(colName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = strCustom;
                                                    if (!htSumCols.ContainsKey(colName))
                                                    {
                                                        TotalAmount = TotalAmount + Convert.ToDouble(strCustom);
                                                        strTotalAmount = Convert.ToString(TotalAmount);
                                                        htSumCols.Add(colName, Convert.ToString(strTotalAmount));
                                                    }
                                                    else
                                                    {
                                                        TotalAmount = Convert.ToDouble(htSumCols[colName].ToString());
                                                        TotalAmount = TotalAmount + Convert.ToDouble(strCustom);
                                                        strTotalAmount = Convert.ToString(TotalAmount);
                                                        htSumCols[colName] = strTotalAmount;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                if (htColNameValues.ContainsKey(colName))
                                {
                                    string colLabelName = htColNameValues[colName].ToString();
                                    switch (colLabelName)
                                    {
                                        case "TrxID":
                                            {
                                                lstFormats.Add("TrxID~" + colValue);
                                                if (dtMain.Columns.Contains(colLabelName))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                if (colLabelName.Contains("TrxID"))
                                                {
                                                    dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                }
                                                else if (dtMain.Columns.Contains(colName))
                                                {
                                                    switch (colName)
                                                    {
                                                        case "Link1":
                                                        case "Link2":
                                                        case "Link3":
                                                            {
                                                                string[] splittedColValue = new string[colValue.Split('-').Length];
                                                                if ((bool)trimStatus)
                                                                {
                                                                    colFVL = colWidths[dtMain.Columns[colLabelName].Ordinal];
                                                                    if (colValue.Contains("-"))
                                                                    {
                                                                        splittedColValue = colValue.Split('-');
                                                                    }
                                                                    if (splittedColValue.Length > 1)
                                                                    {
                                                                        if (splittedColValue[0] != null)
                                                                        {
                                                                            if (splittedColValue[0].Length > colFVL)
                                                                            {
                                                                                dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].Remove(colFVL - 3) + "...";
                                                                            }
                                                                            else
                                                                            {
                                                                                dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (colValue.Length > colFVL)
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue.Remove(colFVL - 3) + "...";
                                                                        }
                                                                        else
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (colValue.Length > colFVL)
                                                                    {
                                                                        if (splittedColValue[0] != null)
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = splittedColValue[0].ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colLabelName] = colValue;
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                //Remaining columns
                                                                if ((bool)trimStatus)
                                                                {
                                                                    colFVL = colWidths[dtMain.Columns[colName].Ordinal];
                                                                    if (colValue.Length > colFVL)
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colName] = colValue.Remove(colFVL - 3) + "...";
                                                                    }
                                                                    else
                                                                    {
                                                                        dtMain.Rows[dtMain.Rows.Count - 1][colName] = colValue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dtMain.Rows[dtMain.Rows.Count - 1][colName] = colValue;
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    switch (colName)
                                    {
                                        case "pLayout":
                                            {
                                                if (colValue == "1") { if ((bool)PLayout == false) { PLayout = true; } }
                                                break;
                                            }
                                        case "pFont":
                                        case "pUnderLine":
                                        case "pBox":
                                            {
                                                if (colValue == "1")
                                                {
                                                    lstFormats.Add(colName);
                                                }
                                                break;
                                            }
                                        case "pLnSkip":
                                            {
                                                if (colValue == "1")
                                                {
                                                    lstFormats.Add(colName);
                                                    dtMain.Rows.Add(dtMain.NewRow());
                                                    for (int col = 0; col < dtMain.Columns.Count; col++)
                                                    {
                                                        dtMain.Rows[dtMain.Rows.Count - 1][dtMain.Columns[col].ToString()] = "SKIP";
                                                    }
                                                    if (!htPFormats.ContainsKey(ID))
                                                    {
                                                        lstFormats.Add("~");
                                                        htPFormats.Add(ID, lstFormats.ToArray());
                                                        ID++;
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                    htPFormats.Add(ID, lstFormats.ToArray());
                    ID++;
                }
                arrayWidth = new int[dtMain.Columns.Count];
                if (strTotalAmount != string.Empty)
                {
                    dtMain.Rows.Add(dtMain.NewRow());
                    IDictionaryEnumerator enumerator = htSumCols.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        decimal amount;
                        Decimal.TryParse(enumerator.Value.ToString(), out amount);
                        string amt = string.Format("{0:N}", amount);

                        dtMain.Rows[dtMain.Rows.Count - 1][enumerator.Key.ToString()] = amt;
                        lstFormats.Add("RAlign" + "~" + Convert.ToString(dtMain.Columns[enumerator.Key.ToString()].Ordinal) + "~BOLD");
                    }
                    htPFormats.Add(ID, lstFormats.ToArray());
                }
                dtMain = WrapFullViewLength(dtMain, colWidths, out arrayWidth);
            }
            catch (Exception ex)
            {
                #region NLog
                NLog.Logger logger = LogManager.GetCurrentClassLogger();
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
            return dtMain;
        }
        #endregion
    }
}
