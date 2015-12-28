using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using OfficeOpenXml;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using LAjitDev.Classes;
using NLog;


namespace LAjitDev
{
    public class rptEXCEL
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        #region ReportStyles

        #region Commented ReportStyle1
        ////Financials-Reporting-LedgerReport
        //public void ReportStyle1(DataTable dtParent, DataTable dtHeader)
        //{
        //    string excelDocPath = string.Empty;
        //    string excelDir = string.Empty;
        //    string strHeader = string.Empty;

        //    string compName = string.Empty;
        //    string compDetails = string.Empty;
        //    string compDate = string.Empty;

        //    DataRow drHeader;
        //    DataRow drCompDetails;
        //    DataRow drCompDate;

        //    const int startRow = 6;
        //    int row = startRow;
        //    int col = 0;
        //    int totalRowValue = 0;
        //    int acctheaderStyleID = 0;
        //    int amtStyleID = 0;
        //    int centerTextStyleID = 0;

        //    int arrayCount = 0;

        //    try
        //    {
        //        excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
        //        excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

        //        if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
        //        {
        //            System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
        //        }

        //        FileInfo newFile = new FileInfo(excelDocPath + @"\Ledger Detail" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

        //        if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
        //        {
        //            System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
        //        }

        //        if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-1.xlsx"))
        //        {
        //            FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-1.xlsx");
        //            //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
        //            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
        //            {
        //                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ledger Detail"];
        //                ExcelCell cell;

        //                drHeader = dtHeader.Rows[0];
        //                strHeader = drHeader["Column1"].ToString();
        //                string[] strRequestedBy = strHeader.Split(':');
        //                cell = worksheet.Cell(2, 2);

        //                if (strRequestedBy.Length > 1)
        //                {
        //                    cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
        //                }
        //                else
        //                {
        //                    cell.Value = strRequestedBy[arrayCount].Trim().ToString();
        //                }

        //                acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
        //                amtStyleID = worksheet.Cell(3, 1).StyleID;
        //                centerTextStyleID = worksheet.Cell(4, 1).StyleID;

        //                worksheet.Cell(3, 1).StyleID = 0;
        //                worksheet.Cell(4, 1).StyleID = 0;

        //                compName = drHeader["Column2"].ToString();
        //                cell = worksheet.Cell(1, 5);
        //                cell.Value = compName;
        //                cell.StyleID = acctheaderStyleID;

        //                drCompDetails = dtHeader.Rows[1];
        //                compDetails = drCompDetails["Column2"].ToString();
        //                cell = worksheet.Cell(2, 5);
        //                cell.Value = compDetails;
        //                cell.StyleID = acctheaderStyleID;

        //                drCompDate = dtHeader.Rows[2];
        //                compDate = drCompDate["Column2"].ToString();
        //                cell = worksheet.Cell(3, 5);
        //                cell.Value = compDate;
        //                cell.StyleID = acctheaderStyleID;

        //                cell = worksheet.Cell(3, 11);
        //                string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

        //if (strReportDate != null && strReportDate != "")
        //{
        //    string[] dateArray = strReportDate.Split(' ');
        //    string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
        //    strReportDate = dateFormat + " " + dateArray[3].ToString();
        //}

        //cell.Value = strReportDate;

        //                int coloumn = 1;

        //                for (int x = 0; x < dtParent.Columns.Count - 1; x++)
        //                {
        //                    if (dtParent.Columns[x].ColumnName != "TrxID")
        //                    {
        //                        cell = worksheet.Cell(row, coloumn);
        //                        cell.Value = dtParent.Columns[x].ColumnName;
        //                        cell.StyleID = centerTextStyleID;
        //                        coloumn = coloumn + 2;
        //                    }
        //                }

        //                row++;
        //                worksheet.InsertRow(row);

        //                row++;

        //                for (int i = 0; i < dtParent.Rows.Count - 1; i++)
        //                {
        //                    DataRow dRow = dtParent.Rows[i];
        //                    coloumn = 1;


        //                    if (row > startRow)
        //                    {
        //                        worksheet.InsertRow(row);
        //                    }

        //                    if (dRow[col].ToString() != null && dRow[col].ToString() != "SKIP")
        //                    {
        //                        for (int y = 0; y < dtParent.Columns.Count - 1; y++)
        //                        {
        //                            cell = worksheet.Cell(row, coloumn);

        //                            if (dtParent.Columns[y].ColumnName == "TrxID")
        //                            {
        //                                y = y + 1;
        //                            }

        //                            cell.Value = System.Security.SecurityElement.Escape(dRow[y].ToString().Replace(",", ""));

        //                            if (y >= dtParent.Columns.Count - 4)
        //                            {
        //                                cell.StyleID = amtStyleID;
        //                            }
        //                            coloumn = coloumn + 2;
        //                        }
        //                    }

        //                    row++;
        //                    totalRowValue = row + 1;
        //                }

        //                xlPackage.Save();
        //            }
        //        }
        //        SaveToClient(newFile);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //Payables-Reporting-VendorHistory
        #endregion

        public void ReportStyle1(DataTable dtParent, DataTable dtHeader, Hashtable htColFormats, Hashtable htColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            Hashtable htParentColNames = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 5;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int clm = 0;
            int col = 0;

            int arrayCount = 0;

            try
            {
                IDictionaryEnumerator enumColFormats = htColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htParentColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }
                drHeader = dtHeader.Rows[0];
                compName = drHeader["Column2"].ToString();

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\" + compName + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-1.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-1.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Report"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 1;

                        foreach (DataColumn column in dtParent.Columns)
                        {
                            if (column.ColumnName != "TrxID")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = centerTextStyleID;
                                clm = clm + 2;
                            }
                        }
                        if (dtParent != null)
                        {
                            for (int rowCount = 0; rowCount < dtParent.Rows.Count; rowCount++)
                            {
                                DataRow dRow = dtParent.Rows[rowCount];
                                clm = 1;
                                row++;

                                if (row > startRow)
                                {
                                    worksheet.InsertRow(row);
                                }

                                if (dRow[col].ToString() != null && dRow[col].ToString() != "SKIP")
                                {
                                    foreach (DataColumn column in dtParent.Columns)
                                    {
                                        if (column.ColumnName != "TrxID")
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(dRow[column.ColumnName].ToString());
                                            cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");
                                            clm = clm + 2;

                                            if (htParentColNames[column.ColumnName] != null)
                                            {
                                                if (htParentColNames[column.ColumnName] == "SUMMED" || htParentColNames[column.ColumnName] == "AMOUNT")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
        }

        public void CustomRptStyle1(DataTable dtParent, DataTable dtHeader, string strOrderByCol, Hashtable htColFormats, Hashtable htColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            Hashtable htParentColNames = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 5;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int orderByColStyleID = 0;
            int clm = 0;
            int col = 0;

            int arrayCount = 0;

            try
            {
                IDictionaryEnumerator enumColFormats = htColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htParentColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                DataView dView = dtParent.DefaultView;
                dView.Sort = htColNameValues[strOrderByCol] + " ASC";
                dtParent = dView.ToTable();

                drHeader = dtHeader.Rows[0];
                compName = drHeader["Column2"].ToString();

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\" + compName + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-1.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-1.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Report"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;
                        orderByColStyleID = worksheet.Cell(4, 3).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 3).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        string colData = null;
                        string orderByColName = htColNameValues[strOrderByCol].ToString();

                        if (dtParent != null)
                        {
                            for (int rowCount = 0; rowCount < dtParent.Rows.Count; rowCount++)
                            {
                                string currColData = dtParent.Rows[rowCount][orderByColName].ToString().Trim();
                                if (colData == null || colData != currColData)
                                {
                                    if (rowCount > 0)
                                    {
                                        row++;
                                        row++;
                                        row++;
                                    }
                                    clm = 1;
                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = System.Security.SecurityElement.Escape(orderByColName).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = orderByColStyleID;

                                    cell = worksheet.Cell(row, clm + 1);
                                    cell.StyleID = orderByColStyleID;

                                    row++;

                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = System.Security.SecurityElement.Escape(currColData).Replace("amp;", "").Replace("&apos;", "");

                                    row++;

                                    for (int colNum = 0; colNum < dtParent.Columns.Count; colNum++)
                                    {
                                        if (dtParent.Columns[colNum].ColumnName != "TrxID" && dtParent.Columns[colNum].ColumnName != orderByColName)
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            string colName = dtParent.Columns[colNum].ColumnName;
                                            cell.Value = System.Security.SecurityElement.Escape(colName).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = centerTextStyleID;
                                            clm = clm + 2;
                                        }
                                    }

                                    colData = currColData;
                                }

                                DataRow dRow = dtParent.Rows[rowCount];
                                clm = 1;
                                row++;

                                //if (row > startRow)
                                //{
                                //    worksheet.InsertRow(row);
                                //}

                                if (dRow[col].ToString() != null && dRow[col].ToString() != "SKIP")
                                {
                                    foreach (DataColumn column in dtParent.Columns)
                                    {
                                        if (column.ColumnName != "TrxID" && column.ColumnName != orderByColName)
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(dRow[column.ColumnName].ToString());
                                            cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");
                                            clm = clm + 2;

                                            if (htParentColNames[column.ColumnName] != null)
                                            {
                                                if (htParentColNames[column.ColumnName].ToString() == "SUMMED" || htParentColNames[column.ColumnName].ToString() == "AMOUNT")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        public void ReportStyle2(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            int clmCount = 0;
            const int startRow = 6;
            int row = startRow;
            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int arrayCount = 0;
            int col = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\VendorHistory " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-2.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-2.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["VendorHistory"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                clmCount = 1;

                                for (int colVal = 1; colVal < dtParent.Columns.Count; colVal++)
                                {
                                    if (dtParent.Columns[colVal].ColumnName != "" && dtParent.Columns[colVal].ColumnName != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount);
                                        cell.Value = dtParent.Columns[colVal].ColumnName;
                                        cell.StyleID = acctheaderStyleID;
                                    }

                                    if (dParentRow[colVal].ToString() != "" && dParentRow[1].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount + 2);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow[colVal].ToString());
                                        cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");
                                    }

                                    if (colVal < dtParent.Columns.Count - 1)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                    //clmCount = clmCount + 4;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        childCol = 0;
                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (childCount == 0)
                                            {
                                                for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                                {
                                                    if (dtChild.Columns[childColCount].ColumnName != null && dtChild.Columns[childColCount].ColumnName != "")
                                                    {
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        childCol++;
                                                        cell.Value = dtChild.Columns[childColCount].ColumnName;
                                                        cell.StyleID = centerTextStyleID;
                                                        childCol++;
                                                    }
                                                }
                                                row++;
                                            }

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 0;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(row, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString());
                                                cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                if (dtChild.Columns[childColCount].ColumnName == "Invoice Amount" || dtChild.Columns[childColCount].ColumnName == "Amount")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }
                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED"))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                        }
                                        //For Totals
                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 0;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(row - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);
                                                        cell.StyleID = amtStyleID;

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                            rowInserted = false;
                                            //row++;
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }
                            if (dtChild != null)
                            {
                                //For Grand Totals
                                row++;
                                worksheet.InsertRow(row);

                                childCol = 0;
                                cell = worksheet.Cell(row, childCol + 1);
                                cell.Value = "Grand Total";
                                cell.StyleID = acctheaderStyleID;

                                row++;

                                for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                {
                                    IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                    while (enumCol.MoveNext())
                                    {
                                        if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                        {
                                            cell = worksheet.Cell(row - 1, childCol + 1);
                                            cell.RemoveValue();
                                            cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                    }

                                    childCol = childCol + 2;
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }

            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Receivables-Reporting-Customer History
        public void ReportStyle3(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            int clmCount = 0;
            const int startRow = 6;
            int row = startRow;
            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int col = 0;

            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\CustomerHistory " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-3.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-3.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["CustomerHistory"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                clmCount = 1;

                                for (int colVal = 1; colVal < dtParent.Columns.Count; colVal++)
                                {
                                    if (dtParent.Columns[colVal].ColumnName != "" && dtParent.Columns[colVal].ColumnName != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount);
                                        cell.Value = dtParent.Columns[colVal].ColumnName;
                                        cell.StyleID = acctheaderStyleID;
                                    }

                                    if (dParentRow[colVal].ToString() != "" && dParentRow[1].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount + 2);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow[colVal].ToString());
                                    }

                                    if (colVal < dtParent.Columns.Count - 1)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                    //clmCount = clmCount + 4;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        childCol = 0;
                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (childCount == 0)
                                            {
                                                for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                                {
                                                    if (dtChild.Columns[childColCount].ColumnName != null && dtChild.Columns[childColCount].ColumnName != "")
                                                    {
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        childCol++;
                                                        cell.Value = dtChild.Columns[childColCount].ColumnName;
                                                        cell.StyleID = centerTextStyleID;
                                                        childCol++;
                                                    }
                                                }
                                                row++;
                                            }

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 0;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(row, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString());
                                                cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                if (childColCount > 5)
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED"))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                        }
                                        //For Totals
                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 0;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(row - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);
                                                        cell.StyleID = amtStyleID;

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                            rowInserted = false;
                                            //row++;
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }
                            //For Grand Totals
                            if (dtChild != null)
                            {
                                row++;
                                worksheet.InsertRow(row);

                                childCol = 0;
                                cell = worksheet.Cell(row, childCol + 1);
                                cell.Value = "Grand Total";
                                cell.StyleID = acctheaderStyleID;

                                row++;

                                for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                {
                                    IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                    while (enumCol.MoveNext())
                                    {
                                        if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                        {
                                            cell = worksheet.Cell(row - 1, childCol + 1);
                                            cell.RemoveValue();
                                            cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                    }

                                    childCol = childCol + 2;
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }

            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //
        public void ReportStyle4(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColFormats, Hashtable htColNameValues, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htParentColNames = new Hashtable();
            Hashtable htChildColNames = new Hashtable();
            Hashtable htTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            int startRow = 6;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int boldAmtStyleID = 0;
            int centerTextStyleID = 0;
            int childColStyleID = 0;
            int col = 0;
            int childColStartCell = 0;
            int parentBorderStyleID = 0;

            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htColNameValues.GetEnumerator();

                if (htColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htParentColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htParentColNames.Add(enumColNameValues.Value, "");
                    }
                }

                enumColFormats = htBColFormats.GetEnumerator();
                enumColNameValues = htBColNameValues.GetEnumerator();

                if (htBColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htChildColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htChildColNames.Add(enumColNameValues.Value, "");
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Vendor Information " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-4.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-4.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Vendor Information"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(2, 2).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;
                        childColStyleID = worksheet.Cell(4, 1).StyleID;
                        parentBorderStyleID = worksheet.Cell(4, 2).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(2, 2).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 2).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        //cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            //cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            //cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                        {
                            if (row > startRow)
                            {
                                if (rowInserted)
                                {
                                    row = row + 1;
                                    worksheet.InsertRow(row);
                                }
                                else
                                {
                                    worksheet.InsertRow(row);
                                }
                            }

                            int clm = 1;

                            foreach (DataColumn column in dtParent.Columns)
                            {
                                if (column.ColumnName != "TrxID")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = column.ColumnName;
                                    cell.StyleID = centerTextStyleID;
                                    clm = clm + 2;
                                }
                            }

                            if (dtChild != null)
                            {
                                foreach (DataColumn column in dtChild.Columns)
                                {
                                    if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.Columns[1].ColumnName + "_TrxID")
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        cell.Value = column.ColumnName;
                                        cell.StyleID = childColStyleID;
                                        clm = clm + 2;
                                    }
                                }
                            }

                            clm = 1;

                            row++;
                            worksheet.InsertRow(row);

                            foreach (DataColumn column in dtParent.Columns)
                            {
                                if (column.ColumnName != "TrxID")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[parentCount][column.ColumnName].ToString()).Replace("amp;", "");

                                    if (htParentColNames[column.ColumnName] == "AMOUNT" || htParentColNames[column.ColumnName] == "SUMMED")
                                    {
                                        cell.Value = cell.Value.ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    clm = clm + 2;
                                }
                            }

                            childColStartCell = clm;
                            //row++;
                            //worksheet.InsertRow(row);

                            DataRow dParentRow = dtParent.Rows[parentCount];

                            startRow = row;
                            if (dtChild != null)
                            {
                                if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                {
                                    int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                    DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                    //row++;
                                    //worksheet.InsertRow(row);

                                    bool noChild = false;

                                    if (drLinkedChildRows.Length == 0)
                                        noChild = true;

                                    for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                    {
                                        clm = childColStartCell;
                                        DataRow dRow = drLinkedChildRows[childCount];

                                        #region CommentedCode
                                        //if (childCount == 0)
                                        //{
                                        //    foreach (DataColumn column in dtChild.Columns)
                                        //    {
                                        //        if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.Columns[1].ColumnName + "_TrxID")
                                        //        {
                                        //            cell = worksheet.Cell(row, clm);
                                        //            cell.Value = column.ColumnName;
                                        //            cell.StyleID = centerTextStyleID;
                                        //            clm = clm + 2;
                                        //        }
                                        //    }
                                        //    row++;
                                        //}
                                        #endregion

                                        if (row > startRow)
                                        {
                                            worksheet.InsertRow(row);
                                        }

                                        //clm = 1;

                                        foreach (DataColumn column in dtChild.Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.Columns[1].ColumnName + "_TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[column.ColumnName].ToString());
                                                cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                if (htChildColNames[column.ColumnName] == "AMOUNT" || htChildColNames[column.ColumnName] == "SUMMED")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;

                                                    if (childCount == 0)
                                                    {
                                                        htTotal[column.ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                    }
                                    //For Totals
                                    worksheet.InsertRow(row);
                                    clm = childColStartCell;

                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = "Total: ";
                                    cell.StyleID = acctheaderStyleID;

                                    foreach (DataColumn column in dtChild.Columns)
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.Columns[1].ColumnName + "_TrxID")
                                        {
                                            if (htChildColNames[column.ColumnName] == "AMOUNT" || htChildColNames[column.ColumnName] == "SUMMED")
                                            {
                                                endRowCellAddr = worksheet.Cell(row - 1, clm).CellAddress;
                                                cell.RemoveValue();
                                                cell.Formula = string.Format("SUM({0}:{1})", htTotal[column.ColumnName], endRowCellAddr);
                                                cell.StyleID = boldAmtStyleID;
                                            }
                                            clm = clm + 2;
                                        }
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    int cellCount = 1;

                                    while (cellCount < clm - 1)
                                    {
                                        cell = worksheet.Cell(row, cellCount);
                                        cell.StyleID = parentBorderStyleID;
                                        cellCount++;
                                    }

                                    row++;
                                }
                            }
                            else
                            {
                                row++;
                            }
                        }

                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }

            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //ReportStyle for Parent-Child Grid
        public void ReportStyle5(DataTable dtParent, DataTable dtBranch, DataTable dtHeader, Hashtable htColFormats, Hashtable htColNameValues, Hashtable htCGbColFormats, Hashtable htCGbColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string strAmtEndAddr = string.Empty;

            Hashtable htParentColNames = new Hashtable();
            Hashtable htChildColNames = new Hashtable();
            Hashtable htAmtStartAddr = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 5;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int colNameStyleID = 0;
            int bStyleID = 0;
            int bCellStyleID = 0;
            int oStyleID = 0;
            int parentColStyleID = 0;
            int parentSpanColStyleID = 0;
            int trxInfoAmtStyleID = 0;
            int boldAmtStyleID = 0;
            int clm = 0;

            int arrayCount = 0;

            try
            {
                IDictionaryEnumerator enumColFormats = htColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htColNameValues.GetEnumerator();

                if (htColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htParentColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htParentColNames.Add(enumColNameValues.Value, "");
                    }
                }

                enumColFormats = htCGbColFormats.GetEnumerator();
                enumColNameValues = htCGbColNameValues.GetEnumerator();

                if (htCGbColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htChildColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htChildColNames.Add(enumColNameValues.Value, "");
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Grid Report " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-5.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-5.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Grid Report"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        bStyleID = worksheet.Cell(2, 2).StyleID;
                        bCellStyleID = worksheet.Cell(2, 3).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        oStyleID = worksheet.Cell(3, 2).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;
                        parentColStyleID = worksheet.Cell(4, 2).StyleID;
                        trxInfoAmtStyleID = worksheet.Cell(4, 3).StyleID;
                        colNameStyleID = worksheet.Cell(5, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(5, 2).StyleID;
                        parentSpanColStyleID = worksheet.Cell(5, 3).StyleID;

                        worksheet.Cell(2, 2).StyleID = 0;
                        worksheet.Cell(2, 3).StyleID = 0;
                        worksheet.Cell(3, 2).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 2).StyleID = 0;
                        worksheet.Cell(4, 3).StyleID = 0;
                        worksheet.Cell(5, 1).StyleID = 0;
                        worksheet.Cell(5, 2).StyleID = 0;
                        worksheet.Cell(5, 3).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            //DisplayParentsOnly(dtParent, dtBranch, ref strAmtEndAddr, htParentColNames, htAmtStartAddr, startRow, ref row, amtStyleID, parentColStyleID, parentSpanColStyleID, ref clm, worksheet, ref cell);

                            row++;
                            worksheet.InsertRow(row);

                            for (int bgtRowCount = 0; bgtRowCount < dtParent.Rows.Count; bgtRowCount++)
                            {
                                strAmtEndAddr = string.Empty;
                                htAmtStartAddr.Clear();

                                clm = 1;

                                if (row > startRow)
                                {
                                    if (dtBranch != null)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }

                                clm = 1;

                                if (dtBranch != null)
                                {
                                    foreach (DataColumn column in dtParent.Columns)
                                    {
                                        if (column.ColumnName != "TrxID")
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = parentColStyleID;

                                            cell = worksheet.Cell(row, clm + 1);
                                            cell.StyleID = parentSpanColStyleID;

                                            clm = clm + 2;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bgtRowCount == 0)
                                    {
                                        foreach (DataColumn column in dtParent.Columns)
                                        {
                                            if (column.ColumnName != "TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                cell.StyleID = parentColStyleID;

                                                cell = worksheet.Cell(row, clm + 1);
                                                cell.StyleID = parentSpanColStyleID;

                                                clm = clm + 2;
                                            }
                                        }
                                    }
                                }

                                #region Commented Code
                                //cell = worksheet.Cell(row, arrayCount + 1);
                                //cell.Value = "Group";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 3);
                                //cell.Value = "Description";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 5);
                                //cell.Value = "Caption";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 7);
                                //cell.Value = "Bid / Contract";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 9);
                                //cell.Value = "Budget Variance";
                                //cell.StyleID = colNameStyleID;
                                #endregion

                                //New Row Insertion
                                row++;
                                worksheet.InsertRow(row);

                                clm = 1;

                                foreach (DataColumn column in dtParent.Columns)
                                {
                                    if (column.ColumnName != "TrxID")
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount][column.ColumnName].ToString());
                                        cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                        if (htParentColNames[column.ColumnName] != null)
                                        {
                                            if (htParentColNames[column.ColumnName] == "SUMMED" || htParentColNames[column.ColumnName] == "AMOUNT")
                                            {
                                                cell.Value = cell.Value.ToString().Replace(",", "");
                                                cell.StyleID = amtStyleID;
                                            }
                                        }

                                        clm = clm + 2;
                                    }
                                }

                                #region Commented Code
                                //cell = worksheet.Cell(row, arrayCount + 1);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Group"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 3);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Description"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 5);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Caption"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 7);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Bid / Contract"].ToString().Replace(",", ""));
                                //cell.StyleID = amtStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 9);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Budget Variance"].ToString().Replace(",", ""));
                                //cell.StyleID = amtStyleID;
                                #endregion


                                if (dtBranch != null)
                                {
                                    //New Row Insertion
                                    row++;
                                    worksheet.InsertRow(row);

                                    if (dtParent.Rows[bgtRowCount]["TrxID"] != null && dtParent.Rows[bgtRowCount]["TrxID"].ToString() != "")
                                    {
                                        int trxID = Convert.ToInt32(dtParent.Rows[bgtRowCount]["TrxID"].ToString());

                                        DataRow[] drBgtTotal = dtBranch.Select(dtParent.TableName + "_TrxID='" + trxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        //int colCount = 1;
                                        clm = 5;

                                        foreach (DataColumn column in dtBranch.Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                cell.StyleID = colNameStyleID;

                                                clm = clm + 2;
                                            }
                                        }

                                        #region Commented Code
                                        //cell = worksheet.Cell(row, arrayCount + 1);
                                        //cell.Value = "AICP Line";
                                        //cell.StyleID = colNameStyleID;

                                        //cell = worksheet.Cell(row, arrayCount + 7);
                                        //cell.Value = "Bid / Contract";
                                        //cell.StyleID = colNameStyleID;

                                        //cell = worksheet.Cell(row, arrayCount + 9);
                                        //cell.Value = "Budget Variance";
                                        //cell.StyleID = colNameStyleID;
                                        #endregion

                                        if (drBgtTotal.Length > 0)
                                        {
                                            for (int selBgtTotalRows = 0; selBgtTotalRows < drBgtTotal.Length; selBgtTotalRows++)
                                            {
                                                clm = 5;

                                                if (row > startRow)
                                                {
                                                    row++;
                                                    worksheet.InsertRow(row);
                                                }

                                                foreach (DataColumn column in dtBranch.Columns)
                                                {
                                                    if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                                    {
                                                        cell = worksheet.Cell(row, clm);
                                                        cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows][column.ColumnName].ToString());
                                                        cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                        if (htChildColNames[column.ColumnName] != null)
                                                        {
                                                            if (htChildColNames[column.ColumnName] == "SUMMED" || htChildColNames[column.ColumnName] == "AMOUNT")
                                                            {
                                                                cell.Value = cell.Value.ToString().ToString().Replace(",", "");
                                                                cell.StyleID = amtStyleID;

                                                                if (selBgtTotalRows == 0)
                                                                {
                                                                    htAmtStartAddr[column.ColumnName] = cell.CellAddress;
                                                                }
                                                            }
                                                        }

                                                        clm = clm + 2;
                                                    }
                                                }

                                                #region Commented Code
                                                //cell = worksheet.Cell(row, arrayCount + 1);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["AICP Line"].ToString().Replace(",", ""));

                                                //cell = worksheet.Cell(row, arrayCount + 7);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["Bid / Contract"].ToString().Replace(",", ""));
                                                //cell.StyleID = amtStyleID;

                                                //if (selBgtTotalRows == 0)
                                                //    bidCtStartAddr = cell.CellAddress;

                                                //cell = worksheet.Cell(row, arrayCount + 9);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["Budget Variance"].ToString().Replace(",", ""));
                                                //cell.StyleID = amtStyleID;

                                                //if (selBgtTotalRows == 0)
                                                //    bidVarStartAddr = cell.CellAddress;
                                                #endregion
                                            }

                                            row++;
                                            worksheet.InsertRow(row);

                                            cell = worksheet.Cell(row, arrayCount + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            clm = 5;
                                            foreach (DataColumn column in dtBranch.Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                                {
                                                    if (htChildColNames[column.ColumnName] != null)
                                                    {
                                                        if (htChildColNames[column.ColumnName] == "SUMMED" || htChildColNames[column.ColumnName] == "AMOUNT")
                                                        {
                                                            cell = worksheet.Cell(row, clm);
                                                            cell.RemoveValue();

                                                            strAmtEndAddr = worksheet.Cell(row - 1, clm).CellAddress;
                                                            cell.Formula = string.Format("SUM({0}:{1})", htAmtStartAddr[column.ColumnName], strAmtEndAddr);
                                                            cell.StyleID = boldAmtStyleID;
                                                        }
                                                    }
                                                    clm = clm + 2;
                                                }
                                            }

                                            #region Commented Code
                                            ////Formula for Bid / Contract
                                            //cell = worksheet.Cell(row, arrayCount + 7);
                                            //cell.RemoveValue();
                                            //bidCtEndAddr = worksheet.Cell(row - 1, arrayCount + 7).CellAddress;
                                            //cell.Formula = string.Format("SUM({0}:{1})", bidCtStartAddr, bidCtEndAddr);
                                            //cell.StyleID = boldAmtStyleID;

                                            ////Formula for Bid Variance
                                            //cell = worksheet.Cell(row, arrayCount + 9);
                                            //cell.RemoveValue();
                                            //bidVarEndAddr = worksheet.Cell(row - 1, arrayCount + 9).CellAddress;
                                            //cell.Formula = string.Format("SUM({0}:{1})", bidVarStartAddr, bidVarEndAddr);
                                            //cell.StyleID = boldAmtStyleID;
                                            #endregion

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }
                        }
                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        private static void DisplayParentsOnly(DataTable dtParent, DataTable dtBranch, ref string strAmtEndAddr, Hashtable htParentColNames, Hashtable htAmtStartAddr, int startRow, ref int row, int amtStyleID, int parentColStyleID, int parentSpanColStyleID, ref int clm, ExcelWorksheet worksheet, ref ExcelCell cell)
        {
            for (int bgtRowCount = 0; bgtRowCount < dtParent.Rows.Count; bgtRowCount++)
            {
                strAmtEndAddr = string.Empty;
                htAmtStartAddr.Clear();

                clm = 1;

                //if (row > startRow)
                //{
                //    if (dtBranch != null)
                //    {
                //        row++;
                //        worksheet.InsertRow(row);
                //    }
                //}

                clm = 1;

                if (bgtRowCount == 0)
                {
                    foreach (DataColumn column in dtParent.Columns)
                    {
                        if (column.ColumnName != "TrxID" && column.ColumnName != "Group")
                        {
                            cell = worksheet.Cell(row, clm);
                            cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = parentColStyleID;

                            cell = worksheet.Cell(row, clm + 1);
                            cell.StyleID = parentSpanColStyleID;

                            clm = clm + 2;
                        }
                    }
                }

                #region Commented Code
                //cell = worksheet.Cell(row, arrayCount + 1);
                //cell.Value = "Group";
                //cell.StyleID = colNameStyleID;

                //cell = worksheet.Cell(row, arrayCount + 3);
                //cell.Value = "Description";
                //cell.StyleID = colNameStyleID;

                //cell = worksheet.Cell(row, arrayCount + 5);
                //cell.Value = "Caption";
                //cell.StyleID = colNameStyleID;

                //cell = worksheet.Cell(row, arrayCount + 7);
                //cell.Value = "Bid / Contract";
                //cell.StyleID = colNameStyleID;

                //cell = worksheet.Cell(row, arrayCount + 9);
                //cell.Value = "Budget Variance";
                //cell.StyleID = colNameStyleID;
                #endregion

                //New Row Insertion
                row++;
                worksheet.InsertRow(row);

                clm = 1;

                foreach (DataColumn column in dtParent.Columns)
                {
                    if (column.ColumnName != "TrxID" && column.ColumnName != "Group")
                    {
                        cell = worksheet.Cell(row, clm);
                        cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount][column.ColumnName].ToString());
                        cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                        if (htParentColNames[column.ColumnName] != null)
                        {
                            if (htParentColNames[column.ColumnName] == "SUMMED" || htParentColNames[column.ColumnName] == "AMOUNT")
                            {
                                cell.Value = cell.Value.ToString().Replace(",", "");
                                cell.StyleID = amtStyleID;
                            }
                        }

                        clm = clm + 2;
                    }
                }

                #region Commented Code
                //cell = worksheet.Cell(row, arrayCount + 1);
                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Group"].ToString().Replace(",", ""));

                //cell = worksheet.Cell(row, arrayCount + 3);
                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Description"].ToString().Replace(",", ""));

                //cell = worksheet.Cell(row, arrayCount + 5);
                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Caption"].ToString().Replace(",", ""));

                //cell = worksheet.Cell(row, arrayCount + 7);
                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Bid / Contract"].ToString().Replace(",", ""));
                //cell.StyleID = amtStyleID;

                //cell = worksheet.Cell(row, arrayCount + 9);
                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Budget Variance"].ToString().Replace(",", ""));
                //cell.StyleID = amtStyleID;
                #endregion
            }
        }

        //Report Style 7
        public void ReportStyle7(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            int clmCount = 0;
            const int startRow = 6;
            int row = startRow;
            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int col = 0;

            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\CustomerHistory " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-3.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-3.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["CustomerHistory"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                clmCount = 1;

                                int currentRow = row;

                                for (int colVal = 1; colVal < dtParent.Columns.Count; colVal++)
                                {
                                    if (dtParent.Columns[colVal].ColumnName != "" && dtParent.Columns[colVal].ColumnName != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount);
                                        cell.Value = dtParent.Columns[colVal].ColumnName;
                                        cell.StyleID = acctheaderStyleID;
                                    }

                                    if (dParentRow[colVal].ToString() != "" && dParentRow[1].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount + 2);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow[colVal].ToString());
                                    }

                                    if (colVal < dtParent.Columns.Count - 1)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                    //clmCount = clmCount + 4;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        //row++;
                                        //worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        childCol = 6;
                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (childCount == 0)
                                            {
                                                for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                                {
                                                    if (dtChild.Columns[childColCount].ColumnName != null && dtChild.Columns[childColCount].ColumnName != "")
                                                    {
                                                        cell = worksheet.Cell(currentRow, childCol + 1);
                                                        childCol++;
                                                        cell.Value = dtChild.Columns[childColCount].ColumnName;
                                                        cell.StyleID = centerTextStyleID;
                                                        childCol++;
                                                    }
                                                }
                                                row++;
                                                currentRow++;
                                            }

                                            if (currentRow > dtParent.Columns.Count - 1)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 6;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(currentRow, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString());
                                                cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                if (childColCount > 5)
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED"))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                            currentRow++;
                                        }
                                        //For Totals
                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 6;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(currentRow - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);
                                                        cell.StyleID = amtStyleID;

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                            rowInserted = false;
                                            //row++;
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }
                            //For Grand Totals
                            if (dtChild != null)
                            {
                                row++;
                                worksheet.InsertRow(row);

                                childCol = 0;
                                cell = worksheet.Cell(row, childCol + 1);
                                cell.Value = "Grand Total";
                                cell.StyleID = acctheaderStyleID;

                                row++;
                                childCol = 6;
                                for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                {
                                    IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                    while (enumCol.MoveNext())
                                    {
                                        if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                        {
                                            cell = worksheet.Cell(row - 1, childCol + 1);
                                            cell.RemoveValue();
                                            cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                    }

                                    childCol = childCol + 2;
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }

            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Financials-Reporting-Income Statement
        public void ReportStyle10(DataTable dtParent, DataTable dtHeader)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int totalRowValue = 0;
            int headerStyleID = 0;
            int centerTextStyleID = 0;
            int amtStyleID = 0;

            int arrayCount = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Report " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-10.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-10.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Report"];
                        ExcelCell cell;

                        headerStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = headerStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = headerStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = headerStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        cell = worksheet.Cell(row, col + 2);
                        cell.Value = "Description";
                        cell.StyleID = centerTextStyleID;

                        cell = worksheet.Cell(row, col + 6);
                        cell.Value = "Ending Balance";
                        cell.StyleID = centerTextStyleID;

                        row++;
                        worksheet.InsertRow(row);

                        row++;

                        if (dtParent != null)
                        {
                            for (int i = 0; i < dtParent.Rows.Count - 1; i++)
                            {
                                if (row > startRow)
                                {
                                    worksheet.InsertRow(row);
                                }

                                DataRow dRow = dtParent.Rows[i];

                                if (dRow["Account Type"].ToString() != "SKIP")
                                {
                                    if (dRow["Account Type"].ToString() != "" && dRow["Account Type"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dRow["Account Type"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                        cell.StyleID = headerStyleID;
                                    }

                                    if (dRow["Description"].ToString() != "" && dRow["Description"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 2);
                                        if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                        {
                                            dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                            cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", ""); ;
                                        }
                                    }

                                    if (dRow["Ending Balance"].ToString() != "" && dRow["Ending Balance"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 6);
                                        cell.Value = dRow["Ending Balance"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }
                                    else
                                    {
                                        if (dRow["Total"].ToString() != "" && dRow["Total"].ToString() != null)
                                        {
                                            cell = worksheet.Cell(row, col + 6);
                                            cell.Value = dRow["Total"].ToString().Replace(",", "");
                                            cell.StyleID = amtStyleID;
                                        }
                                    }

                                    row++;
                                    totalRowValue = row + 1;
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //AR-Invoice History (Excel)
        public void ReportStyle200(DataTable dtParent, DataTable dtChild, Hashtable htColNameValues, Hashtable htCGbColNameValues, DataTable dtHeader)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 9;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int boldAmtStyleID = 0;
            int centerTextStyleID = 0;
            int lftAlgnStyleID = 0;
            int col = 0;
            int clm = 0;

            int arrayCount = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Invoice " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-200.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-200.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Invoice"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(3, 2).StyleID;
                        lftAlgnStyleID = worksheet.Cell(4, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(5, 3).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(3, 2).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(5, 3).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            cell = worksheet.Cell(6, 1);
                            cell.Value = dtParent.Rows[arrayCount][htColNameValues["InvDate"].ToString()].ToString().Replace(",", "");

                            cell = worksheet.Cell(7, 1);
                            cell.Value = dtParent.Rows[arrayCount][htColNameValues["Customer"].ToString()].ToString().Replace(",", "");

                            string[] strAddress = dtParent.Rows[arrayCount][htColNameValues["Address1"].ToString()].ToString().Split('~');

                            for (int arrayLength = 0; arrayLength < strAddress.Length; arrayLength++)
                            {
                                if (row > startRow)
                                {
                                    worksheet.InsertRow(row);
                                }

                                cell = worksheet.Cell(row, col + 1);
                                cell.Value = System.Security.SecurityElement.Escape(strAddress[arrayLength]).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = lftAlgnStyleID;

                                row++;
                            }

                            worksheet.InsertRow(row);

                            cell = worksheet.Cell(row, col + 1);
                            cell.Value = System.Security.SecurityElement.Escape(htColNameValues["InvNumber"] + ": ").Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctheaderStyleID;

                            cell = worksheet.Cell(row, col + 2);
                            cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[arrayCount][htColNameValues["InvNumber"].ToString()].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = lftAlgnStyleID;

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);

                            cell = worksheet.Cell(row, col + 1);
                            cell.Value = System.Security.SecurityElement.Escape(htColNameValues["ARInfo1"] + ": ").Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctheaderStyleID;

                            cell = worksheet.Cell(row, col + 2);
                            cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[arrayCount][htColNameValues["ARInfo1"].ToString()].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = lftAlgnStyleID;

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);

                            cell = worksheet.Cell(row, col + 1);
                            cell.Value = System.Security.SecurityElement.Escape(htColNameValues["ForProd"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctheaderStyleID;

                            cell = worksheet.Cell(row, col + 2);
                            cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[arrayCount][htColNameValues["ForProd"].ToString()].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = lftAlgnStyleID;

                            row++;
                            worksheet.InsertRow(row);

                            clm = 1;

                            row++;
                            worksheet.InsertRow(row);

                            clm = 2;

                            if (dtChild != null)
                            {
                                //To Print Child Coloumn Names
                                foreach (DataColumn column in dtChild.Columns)
                                {
                                    if (column.ColumnName == htCGbColNameValues["Description"].ToString() || column.ColumnName == htCGbColNameValues["InvoiceAmount"].ToString())
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = centerTextStyleID;

                                        clm = clm + 2;
                                    }
                                }

                                row++;
                                worksheet.InsertRow(row);

                                //To Print Child Rows
                                for (int rowCount = 0; rowCount < dtChild.Rows.Count; rowCount++)
                                {
                                    clm = 2;
                                    foreach (DataColumn column in dtChild.Columns)
                                    {
                                        if (column.ColumnName == htCGbColNameValues["Description"].ToString() || column.ColumnName == htCGbColNameValues["InvoiceAmount"].ToString())
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(dtChild.Rows[rowCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                            if (column.ColumnName == htCGbColNameValues["InvoiceAmount"].ToString().Replace(",", ""))
                                            {
                                                cell.Value = cell.Value.ToString().Replace(",", "");
                                                cell.StyleID = amtStyleID;
                                            }

                                            clm = clm + 2;
                                        }
                                    }

                                    row++;
                                    worksheet.InsertRow(row);
                                }


                                row++;
                                worksheet.InsertRow(row);

                                cell = worksheet.Cell(row, col + 2);
                                cell.Value = htColNameValues["InvoiceAmount"].ToString().Replace(",", "");
                                cell.StyleID = acctheaderStyleID;

                                cell = worksheet.Cell(row, col + 4);
                                cell.Value = dtParent.Rows[arrayCount][htColNameValues["InvoiceAmount"].ToString()].ToString().Replace(",", "");
                                cell.StyleID = boldAmtStyleID;
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Job Costing-Reporting-JobCommercials2
        public void ReportStyle400(DataSet dsAll, DataTable dtHeader)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            Hashtable htInvoiceTotal = new Hashtable();

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int totalRowValue = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int colNameStyleID = 0;
            int bStyleID = 0;
            int bCellStyleID = 0;
            int oStyleID = 0;
            int trxInfoColStyleID = 0;
            int trxInfoStyleID = 0;
            int trxInfoAmtStyleID = 0;

            int arrayCount = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Job Cost Commercial " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-400.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-400.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Job Cost Commercial"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        bStyleID = worksheet.Cell(2, 2).StyleID;
                        bCellStyleID = worksheet.Cell(2, 3).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        oStyleID = worksheet.Cell(3, 2).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;
                        trxInfoColStyleID = worksheet.Cell(4, 2).StyleID;
                        trxInfoAmtStyleID = worksheet.Cell(4, 3).StyleID;
                        colNameStyleID = worksheet.Cell(5, 1).StyleID;
                        trxInfoStyleID = worksheet.Cell(5, 3).StyleID;

                        worksheet.Cell(2, 2).StyleID = 0;
                        worksheet.Cell(2, 3).StyleID = 0;
                        worksheet.Cell(3, 2).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 2).StyleID = 0;
                        worksheet.Cell(4, 3).StyleID = 0;
                        worksheet.Cell(5, 1).StyleID = 0;
                        worksheet.Cell(5, 3).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        DataTable dtJob = dsAll.Tables[arrayCount];

                        int clm = 0;

                        //Starting with First Job in dtJob
                        for (int jobCount = 0; jobCount < dtJob.Rows.Count; jobCount++)
                        {
                            if (jobCount != 0)
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            clm = 1;
                            foreach (DataColumn column in dtJob.Columns)
                            {
                                if (column.ColumnName != "Job#" && column.ColumnName != "TrxID")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "") + ": ";
                                    cell.StyleID = acctheaderStyleID;

                                    cell = worksheet.Cell(row, clm + 1);
                                    cell.Value = System.Security.SecurityElement.Escape(dtJob.Rows[jobCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                    if (column.ColumnName == "Telephone")
                                    {
                                        cell.Value = System.Security.SecurityElement.Escape(dtJob.Rows[jobCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "").Replace(".", "-");
                                    }

                                    row++;
                                    worksheet.InsertRow(row);
                                }
                            }

                            if (dtJob.Rows[jobCount]["TrxID"] != null && dtJob.Rows[jobCount]["TrxID"].ToString() != "")
                            {
                                int trxID = Convert.ToInt32(dtJob.Rows[jobCount]["TrxID"].ToString());

                                DataTable dtBgtGroup = dsAll.Tables[arrayCount + 1];
                                DataRow[] drBgtGroup = dtBgtGroup.Select("JobID='" + trxID + "'");

                                row++;
                                worksheet.InsertRow(row);

                                clm = 1;

                                foreach (DataColumn column in dtBgtGroup.Columns)
                                {
                                    if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = colNameStyleID;

                                        clm = clm + 2;
                                    }
                                }

                                row++;
                                worksheet.InsertRow(row);

                                int bCount = 0;
                                for (int selBgtGroupRows = 0; selBgtGroupRows < drBgtGroup.Length; selBgtGroupRows++)
                                {
                                    clm = 1;

                                    if (row > startRow)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }

                                    foreach (DataColumn column in dtBgtGroup.Columns)
                                    {
                                        if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(drBgtGroup[selBgtGroupRows][column.ColumnName].ToString().Replace(",", ""));
                                            cell.StyleID = amtStyleID;

                                            if (bCount % 2 == 0)
                                            {
                                                if (column.ColumnName == "Group")
                                                {
                                                    cell.StyleID = bCellStyleID;
                                                }
                                                else
                                                {
                                                    cell.StyleID = bStyleID;
                                                }
                                                cell = worksheet.Cell(row, clm + 1);
                                                cell.StyleID = bCellStyleID;
                                            }

                                            clm = clm + 2;
                                        }
                                    }

                                    bCount++;
                                }

                                row++;
                                worksheet.InsertRow(row);

                                DataTable dtJobInfo = dsAll.Tables[arrayCount + 2];
                                if (dtJobInfo.Columns.Count > 0)
                                {
                                    DataRow[] drJobInfo = dtJobInfo.Select("JobID='" + trxID + "'");

                                    row++;
                                    worksheet.InsertRow(row);

                                    for (int selJobInfoRows = 0; selJobInfoRows < drJobInfo.Length; selJobInfoRows++)
                                    {
                                        clm = 1;

                                        if (row > startRow)
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                        }

                                        foreach (DataColumn column in dtJobInfo.Columns)
                                        {
                                            if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(drJobInfo[selJobInfoRows][column.ColumnName].ToString().Replace(",", ""));
                                                //cell.StyleID = amtStyleID;
                                                cell.StyleID = oStyleID;

                                                cell = worksheet.Cell(row, clm + 1);
                                                cell.StyleID = oStyleID;

                                                clm = clm + 2;
                                            }
                                        }
                                    }

                                    row++;
                                    worksheet.InsertRow(row);
                                }

                                row++;
                                worksheet.InsertRow(row);

                                //Load InvoiceInfo
                                if (dsAll.Tables.Count > 5)
                                {
                                    DataTable dtInvoiceInfo = dsAll.Tables[arrayCount + 5];
                                    if (dtInvoiceInfo.Columns.Count > 0)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);

                                        DataRow[] drInvoiceInfo = dtInvoiceInfo.Select("JobID='" + trxID + "'");

                                        if (drInvoiceInfo.Length > 0)
                                        {
                                            clm = 1;
                                            foreach (DataColumn column in dtInvoiceInfo.Columns)
                                            {
                                                if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                    cell.StyleID = colNameStyleID;

                                                    clm = clm + 2;
                                                }
                                            }

                                            for (int invoiceCount = 0; invoiceCount < drInvoiceInfo.Length; invoiceCount++)
                                            {
                                                clm = 1;
                                                if (row > startRow)
                                                {
                                                    row++;
                                                    worksheet.InsertRow(row);
                                                }

                                                foreach (DataColumn column in dtInvoiceInfo.Columns)
                                                {
                                                    if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                                    {
                                                        cell = worksheet.Cell(row, clm);
                                                        cell.Value = System.Security.SecurityElement.Escape(drInvoiceInfo[invoiceCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        cell.StyleID = bCellStyleID;

                                                        if (column.ColumnName == "Invoice Amount" || column.ColumnName == "Paid-To-Date" || column.ColumnName == "Balance Due")
                                                        {
                                                            cell.Value = cell.Value.ToString().Replace(",", "");
                                                            cell.StyleID = bStyleID;
                                                            if (invoiceCount == 0)
                                                            {
                                                                htInvoiceTotal[column.ColumnName] = cell.CellAddress;
                                                            }
                                                        }

                                                        cell = worksheet.Cell(row, clm + 1);
                                                        cell.StyleID = bCellStyleID;

                                                        clm = clm + 2;
                                                    }
                                                }
                                            }

                                            row++;
                                            worksheet.InsertRow(row);

                                            clm = 1;
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = "Total";
                                            cell.StyleID = bCellStyleID;

                                            foreach (DataColumn column in dtInvoiceInfo.Columns)
                                            {
                                                if (column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    if (clm != 1)
                                                    {
                                                        cell.StyleID = bCellStyleID;
                                                    }

                                                    if (htInvoiceTotal[column.ColumnName] != null)
                                                    {
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", htInvoiceTotal[column.ColumnName], worksheet.Cell(row - 1, clm).CellAddress);
                                                        cell.StyleID = bStyleID;
                                                    }

                                                    cell = worksheet.Cell(row, clm + 1);
                                                    cell.StyleID = bCellStyleID;

                                                    clm = clm + 2;
                                                }
                                            }
                                        }
                                    }
                                }

                                row++;
                                worksheet.InsertRow(row);

                                DataTable dtBgtTotal = dsAll.Tables[arrayCount + 3];

                                if (dtBgtTotal.Columns.Count > 0)
                                {
                                    DataRow[] drBgtTotal = dtBgtTotal.Select("JobID='" + trxID + "'");

                                    row++;
                                    worksheet.InsertRow(row);

                                    for (int selBgtTotalRows = 0; selBgtTotalRows < drBgtTotal.Length; selBgtTotalRows++)
                                    {
                                        clm = 1;

                                        if (row > startRow)
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                        }

                                        foreach (DataColumn column in dtBgtTotal.Columns)
                                        {
                                            if (column.ColumnName != "AccountID" && column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                cell.StyleID = colNameStyleID;

                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        clm = 1;

                                        foreach (DataColumn column in dtBgtTotal.Columns)
                                        {
                                            if (column.ColumnName != "AccountID" && column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                                if (column.ColumnName != "AICP Line")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        long AccountID = Convert.ToInt64(drBgtTotal[selBgtTotalRows]["TrxID"].ToString());

                                        DataTable dtTrxInfo = dsAll.Tables[arrayCount + 4];
                                        DataRow[] drTrxInfo = dtTrxInfo.Select("AccountID='" + AccountID + "'");

                                        if (drTrxInfo.Length > 0)
                                        {
                                            row++;
                                            worksheet.InsertRow(row);

                                            clm = 1;
                                            foreach (DataColumn column in dtTrxInfo.Columns)
                                            {
                                                if (column.ColumnName != "AccountID" && column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                    cell.StyleID = trxInfoColStyleID;

                                                    cell = worksheet.Cell(row, clm + 1);
                                                    cell.StyleID = trxInfoStyleID;

                                                    clm = clm + 2;
                                                }
                                            }

                                            row++;
                                            worksheet.InsertRow(row);

                                            clm = 1;

                                            for (int selTrxInfoRows = 0; selTrxInfoRows < drTrxInfo.Length; selTrxInfoRows++)
                                            {
                                                clm = 1;
                                                if (selTrxInfoRows > 0)
                                                {
                                                    row++;
                                                    worksheet.InsertRow(row);
                                                }

                                                foreach (DataColumn column in dtTrxInfo.Columns)
                                                {
                                                    if (column.ColumnName != "AccountID" && column.ColumnName != "JobID" && column.ColumnName != "TrxID")
                                                    {
                                                        cell = worksheet.Cell(row, clm);
                                                        cell.Value = System.Security.SecurityElement.Escape(drTrxInfo[selTrxInfoRows][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", ""); ;

                                                        if (column.ColumnName == "Amount")
                                                        {
                                                            cell.Value = cell.Value.ToString().Replace(",", "");
                                                            //cell.StyleID = trxInfoAmtStyleID;
                                                            cell.StyleID = trxInfoAmtStyleID;
                                                        }
                                                        else
                                                            cell.StyleID = trxInfoStyleID;

                                                        cell = worksheet.Cell(row, clm + 1);
                                                        cell.StyleID = trxInfoStyleID;

                                                        clm = clm + 2;
                                                    }
                                                }
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                    }
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        public void ReportStyle405(DataTable dtParent, DataTable dtBranch, DataTable dtHeader, Hashtable htColFormats, Hashtable htColNameValues, Hashtable htCGbColFormats, Hashtable htCGbColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string strAmtEndAddr = string.Empty;

            Hashtable htParentColNames = new Hashtable();
            Hashtable htChildColNames = new Hashtable();
            Hashtable htAmtStartAddr = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 5;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int colNameStyleID = 0;
            int bStyleID = 0;
            int bCellStyleID = 0;
            int oStyleID = 0;
            int parentColStyleID = 0;
            int parentSpanColStyleID = 0;
            int trxInfoAmtStyleID = 0;
            int boldAmtStyleID = 0;
            int clm = 0;

            int arrayCount = 0;

            try
            {
                IDictionaryEnumerator enumColFormats = htColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htColNameValues.GetEnumerator();

                if (htColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htParentColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htParentColNames.Add(enumColNameValues.Value, "");
                    }
                }

                enumColFormats = htCGbColFormats.GetEnumerator();
                enumColNameValues = htCGbColNameValues.GetEnumerator();

                if (htCGbColFormats.Count > 0)
                {
                    while (enumColNameValues.MoveNext())
                    {
                        enumColFormats.Reset();

                        while (enumColFormats.MoveNext())
                        {
                            if (enumColNameValues.Key == enumColFormats.Key)
                            {
                                htChildColNames.Add(enumColNameValues.Value, enumColFormats.Value);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (enumColNameValues.MoveNext())
                    {
                        htChildColNames.Add(enumColNameValues.Value, "");
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Report " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-405.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-405.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Report"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        bStyleID = worksheet.Cell(2, 2).StyleID;
                        bCellStyleID = worksheet.Cell(2, 3).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        oStyleID = worksheet.Cell(3, 2).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;
                        parentColStyleID = worksheet.Cell(4, 2).StyleID;
                        trxInfoAmtStyleID = worksheet.Cell(4, 3).StyleID;
                        colNameStyleID = worksheet.Cell(5, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(5, 2).StyleID;
                        parentSpanColStyleID = worksheet.Cell(5, 3).StyleID;

                        worksheet.Cell(2, 2).StyleID = 0;
                        worksheet.Cell(2, 3).StyleID = 0;
                        worksheet.Cell(3, 2).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 2).StyleID = 0;
                        worksheet.Cell(4, 3).StyleID = 0;
                        worksheet.Cell(5, 1).StyleID = 0;
                        worksheet.Cell(5, 2).StyleID = 0;
                        worksheet.Cell(5, 3).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            DisplayParentsOnly(dtParent, dtBranch, ref strAmtEndAddr, htParentColNames, htAmtStartAddr, startRow, ref row, amtStyleID, parentColStyleID, parentSpanColStyleID, ref clm, worksheet, ref cell);

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);

                            for (int bgtRowCount = 0; bgtRowCount < dtParent.Rows.Count; bgtRowCount++)
                            {
                                strAmtEndAddr = string.Empty;
                                htAmtStartAddr.Clear();

                                clm = 1;

                                if (row > startRow)
                                {
                                    if (dtBranch != null)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }

                                clm = 1;

                                //if (dtBranch != null)
                                //{
                                foreach (DataColumn column in dtParent.Columns)
                                {
                                    //if (column.ColumnName != "TrxID")
                                    if (column.ColumnName != "TrxID" && column.ColumnName != "Group")
                                    {
                                        //if (column.ColumnName == "Group" || column.ColumnName == "Description")
                                        if (column.ColumnName == "Description")
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = parentColStyleID;

                                            cell = worksheet.Cell(row, clm + 1);
                                            cell.StyleID = parentSpanColStyleID;

                                            clm = clm + 2;
                                        }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    //if (bgtRowCount == 0)
                                //    //{
                                //        foreach (DataColumn column in dtParent.Columns)
                                //        {
                                //            //if (column.ColumnName != "TrxID")
                                //            if (column.ColumnName != "TrxID" && column.ColumnName != "Group")
                                //            {
                                //                //if (column.ColumnName == "Group" || column.ColumnName == "Description")
                                //                if (column.ColumnName == "Description")
                                //                {
                                //                    cell = worksheet.Cell(row, clm);
                                //                    cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                //                    cell.StyleID = parentColStyleID;

                                //                    cell = worksheet.Cell(row, clm + 1);
                                //                    cell.StyleID = parentSpanColStyleID;

                                //                    clm = clm + 2;
                                //                }
                                //            }
                                //        }
                                //    //}
                                //}

                                #region Commented Code
                                //cell = worksheet.Cell(row, arrayCount + 1);
                                //cell.Value = "Group";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 3);
                                //cell.Value = "Description";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 5);
                                //cell.Value = "Caption";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 7);
                                //cell.Value = "Bid / Contract";
                                //cell.StyleID = colNameStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 9);
                                //cell.Value = "Budget Variance";
                                //cell.StyleID = colNameStyleID;
                                #endregion

                                //New Row Insertion
                                row++;
                                worksheet.InsertRow(row);

                                clm = 1;

                                foreach (DataColumn column in dtParent.Columns)
                                {
                                    //if (column.ColumnName != "TrxID")
                                    if (column.ColumnName != "TrxID" && column.ColumnName != "Group")
                                    {
                                        //if (column.ColumnName == "Group" || column.ColumnName == "Description")
                                        if (column.ColumnName == "Description")
                                        {
                                            cell = worksheet.Cell(row, clm);
                                            cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount][column.ColumnName].ToString());
                                            cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                            if (htParentColNames[column.ColumnName] != null)
                                            {
                                                if (htParentColNames[column.ColumnName].ToString() == "SUMMED" || htParentColNames[column.ColumnName].ToString() == "AMOUNT")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }
                                            }

                                            clm = clm + 2;
                                        }
                                    }
                                }

                                #region Commented Code
                                //cell = worksheet.Cell(row, arrayCount + 1);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Group"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 3);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Description"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 5);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Caption"].ToString().Replace(",", ""));

                                //cell = worksheet.Cell(row, arrayCount + 7);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Bid / Contract"].ToString().Replace(",", ""));
                                //cell.StyleID = amtStyleID;

                                //cell = worksheet.Cell(row, arrayCount + 9);
                                //cell.Value = System.Security.SecurityElement.Escape(dtParent.Rows[bgtRowCount]["Budget Variance"].ToString().Replace(",", ""));
                                //cell.StyleID = amtStyleID;
                                #endregion

                                if (dtBranch != null)
                                {
                                    //New Row Insertion
                                    row++;
                                    worksheet.InsertRow(row);

                                    if (dtParent.Rows[bgtRowCount]["TrxID"] != null && dtParent.Rows[bgtRowCount]["TrxID"].ToString() != "")
                                    {
                                        int trxID = Convert.ToInt32(dtParent.Rows[bgtRowCount]["TrxID"].ToString());

                                        DataRow[] drBgtTotal = dtBranch.Select(dtParent.TableName + "_TrxID='" + trxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        //int colCount = 1;
                                        clm = 3;

                                        foreach (DataColumn column in dtBranch.Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                cell.Value = System.Security.SecurityElement.Escape(column.ColumnName).Replace("amp;", "").Replace("&apos;", "");
                                                cell.StyleID = colNameStyleID;

                                                clm = clm + 2;
                                            }
                                        }

                                        #region Commented Code
                                        //cell = worksheet.Cell(row, arrayCount + 1);
                                        //cell.Value = "AICP Line";
                                        //cell.StyleID = colNameStyleID;

                                        //cell = worksheet.Cell(row, arrayCount + 7);
                                        //cell.Value = "Bid / Contract";
                                        //cell.StyleID = colNameStyleID;

                                        //cell = worksheet.Cell(row, arrayCount + 9);
                                        //cell.Value = "Budget Variance";
                                        //cell.StyleID = colNameStyleID;
                                        #endregion

                                        if (drBgtTotal.Length > 0)
                                        {
                                            for (int selBgtTotalRows = 0; selBgtTotalRows < drBgtTotal.Length; selBgtTotalRows++)
                                            {
                                                clm = 3;

                                                if (row > startRow)
                                                {
                                                    row++;
                                                    worksheet.InsertRow(row);
                                                }

                                                foreach (DataColumn column in dtBranch.Columns)
                                                {
                                                    if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                                    {
                                                        cell = worksheet.Cell(row, clm);
                                                        cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows][column.ColumnName].ToString());
                                                        cell.Value = cell.Value.ToString().Replace("amp;", "").Replace("&apos;", "");

                                                        if (htChildColNames[column.ColumnName] != null)
                                                        {
                                                            if (htChildColNames[column.ColumnName].ToString() == "SUMMED" || htChildColNames[column.ColumnName].ToString() == "AMOUNT")
                                                            {
                                                                cell.Value = cell.Value.ToString().Replace(",", "");
                                                                cell.StyleID = amtStyleID;

                                                                if (selBgtTotalRows == 0)
                                                                {
                                                                    htAmtStartAddr[column.ColumnName] = cell.CellAddress;
                                                                }
                                                            }
                                                        }

                                                        clm = clm + 2;
                                                    }
                                                }

                                                #region Commented Code
                                                //cell = worksheet.Cell(row, arrayCount + 1);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["AICP Line"].ToString().Replace(",", ""));

                                                //cell = worksheet.Cell(row, arrayCount + 7);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["Bid / Contract"].ToString().Replace(",", ""));
                                                //cell.StyleID = amtStyleID;

                                                //if (selBgtTotalRows == 0)
                                                //    bidCtStartAddr = cell.CellAddress;

                                                //cell = worksheet.Cell(row, arrayCount + 9);
                                                //cell.Value = System.Security.SecurityElement.Escape(drBgtTotal[selBgtTotalRows]["Budget Variance"].ToString().Replace(",", ""));
                                                //cell.StyleID = amtStyleID;

                                                //if (selBgtTotalRows == 0)
                                                //    bidVarStartAddr = cell.CellAddress;
                                                #endregion
                                            }

                                            row++;
                                            worksheet.InsertRow(row);

                                            cell = worksheet.Cell(row, arrayCount + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            clm = 3;
                                            foreach (DataColumn column in dtBranch.Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != dtParent.TableName.Replace(" ", "") + "_TrxID")
                                                {
                                                    if (htChildColNames[column.ColumnName] != null)
                                                    {
                                                        if (htChildColNames[column.ColumnName] == "SUMMED" || htChildColNames[column.ColumnName] == "AMOUNT")
                                                        {
                                                            cell = worksheet.Cell(row, clm);
                                                            cell.RemoveValue();

                                                            strAmtEndAddr = worksheet.Cell(row - 1, clm).CellAddress;
                                                            cell.Formula = string.Format("SUM({0}:{1})", htAmtStartAddr[column.ColumnName], strAmtEndAddr);
                                                            cell.StyleID = boldAmtStyleID;
                                                        }
                                                    }
                                                    clm = clm + 2;
                                                }
                                            }

                                            #region Commented Code
                                            ////Formula for Bid / Contract
                                            //cell = worksheet.Cell(row, arrayCount + 7);
                                            //cell.RemoveValue();
                                            //bidCtEndAddr = worksheet.Cell(row - 1, arrayCount + 7).CellAddress;
                                            //cell.Formula = string.Format("SUM({0}:{1})", bidCtStartAddr, bidCtEndAddr);
                                            //cell.StyleID = boldAmtStyleID;

                                            ////Formula for Bid Variance
                                            //cell = worksheet.Cell(row, arrayCount + 9);
                                            //cell.RemoveValue();
                                            //bidVarEndAddr = worksheet.Cell(row - 1, arrayCount + 9).CellAddress;
                                            //cell.Formula = string.Format("SUM({0}:{1})", bidVarStartAddr, bidVarEndAddr);
                                            //cell.StyleID = boldAmtStyleID;
                                            #endregion

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                    }
                                }
                                else
                                {
                                    //if (dtBranch != null)
                                    //{
                                    row++;
                                    worksheet.InsertRow(row);
                                    //} 
                                    row++;
                                }
                            }
                        }
                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Payables-Reporting-Aged Payables History
        public void ReportStyle501(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            int clmCount = 0;
            const int startRow = 6;
            int row = startRow;
            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int col = 0;

            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\VendorDetail " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-501.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-501.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["VendorDetail"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                clmCount = 1;

                                for (int colVal = 1; colVal < dtParent.Columns.Count; colVal++)
                                {
                                    if (dtParent.Columns[colVal].ColumnName != "" && dtParent.Columns[colVal].ColumnName != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount);
                                        cell.Value = System.Security.SecurityElement.Escape(dtParent.Columns[colVal].ColumnName);
                                        cell.StyleID = acctheaderStyleID;
                                    }

                                    if (dParentRow[colVal].ToString() != "" && dParentRow[colVal].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, clmCount + 2);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow[colVal].ToString()).Replace("&apos;", "");
                                    }
                                    clmCount = clmCount + 4;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        childCol = 0;
                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (childCount == 0)
                                            {
                                                for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                                {
                                                    if (dtChild.Columns[childColCount].ColumnName != null && dtChild.Columns[childColCount].ColumnName != "")
                                                    {
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        childCol++;
                                                        cell.Value = dtChild.Columns[childColCount].ColumnName.Replace("amp;", "").Replace("&apos;", "");
                                                        cell.StyleID = centerTextStyleID;
                                                        childCol++;
                                                    }
                                                }
                                                row++;
                                            }

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 0;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(row, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString()).Replace("amp;", "").Replace("&apos;", "").Replace(",", "");

                                                if (childColCount > 5)
                                                {
                                                    cell.StyleID = amtStyleID;
                                                }
                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED"))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                        }
                                        //For Totals
                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 0;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(row - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);
                                                        cell.StyleID = amtStyleID;

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                        else
                                        {
                                            row++;
                                            worksheet.InsertRow(row);
                                            rowInserted = false;
                                            //row++;
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);

                            //For Grand Totals
                            childCol = 0;
                            cell = worksheet.Cell(row, childCol + 1);
                            cell.Value = "Grand Total";
                            cell.StyleID = acctheaderStyleID;

                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                            {
                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                while (enumCol.MoveNext())
                                {
                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                    {
                                        cell = worksheet.Cell(row, childCol + 1);
                                        cell.RemoveValue();
                                        cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                        cell.StyleID = amtStyleID;
                                        break;
                                    }
                                }

                                childCol = childCol + 2;
                            }
                        }
                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }

            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Payables-Reporting-Aged Payables(Current)
        public void ReportStyle502(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int totalRowValue = 0;

            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\CustomerDetail " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-502.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-502.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["CustomerDetail"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtChild != null)
                        {
                            IEnumerator dColoumnRow = dtChild.Columns.GetEnumerator();

                            childCol = 0;
                            if (dColoumnRow != null)
                            {
                                while (dColoumnRow.MoveNext())
                                {
                                    if (dColoumnRow.Current.ToString() != dtChild.Columns[0].ColumnName.ToString() && dColoumnRow.Current.ToString() != dtChild.Columns[1].ColumnName.ToString())
                                    {
                                        cell = worksheet.Cell(row, childCol + 1);
                                        childCol++;

                                        cell.Value = dColoumnRow.Current.ToString();
                                        cell.StyleID = centerTextStyleID;
                                        childCol++;
                                    }
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);
                        }

                        row++;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                cell = worksheet.Cell(row, col + 1);

                                if (dParentRow[col + 1].ToString() != "" && dParentRow[col + 1].ToString() != null)
                                {
                                    cell = worksheet.Cell(row, col + 2);
                                    cell.Value = System.Security.SecurityElement.Escape(dParentRow[col + 1].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = centerTextStyleID;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != "" && dParentRow["TrxID"].ToString() != null)
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        childCol = 0;
                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 0;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(row, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                                if (childColCount > 5)
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && (htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED") || htColName[dtChild.Columns[childColCount].ColumnName].Equals("AMOUNT")))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                        }

                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 0;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(row - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);
                                                        cell.StyleID = amtStyleID;

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }

                            if (dtChild != null)
                            {
                                //For Grand Totals
                                row++;
                                worksheet.InsertRow(row);

                                childCol = 0;
                                cell = worksheet.Cell(row, childCol + 1);
                                cell.Value = "Grand Total";
                                cell.StyleID = acctheaderStyleID;

                                //row++;

                                for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                {
                                    IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                    while (enumCol.MoveNext())
                                    {
                                        if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                        {
                                            cell = worksheet.Cell(row, childCol + 1);
                                            cell.RemoveValue();
                                            cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                    }

                                    childCol = childCol + 2;
                                }
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Payables-Reporting-UnPaid Invoices
        public void ReportStyle503(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htBColFormats, Hashtable htBColNameValues)
        {
            Hashtable htColName = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string endRowCellAddr = string.Empty;
            string cellAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            ExcelCell cell;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int clmCount = 0;
            int totalRowValue = 0;

            int childCol = 0;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                IDictionaryEnumerator enumColFormats = htBColFormats.GetEnumerator();
                IDictionaryEnumerator enumColNameValues = htBColNameValues.GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htColName.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\UnPaid Invoices " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-503.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-503.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["UnPaid Invoices"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(2, 1).StyleID;
                        centerTextStyleID = worksheet.Cell(3, 1).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(3, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtChild != null)
                        {
                            IEnumerator dColoumnRow = dtChild.Columns.GetEnumerator();

                            childCol = 0;
                            if (dColoumnRow != null)
                            {
                                while (dColoumnRow.MoveNext())
                                {
                                    if (dColoumnRow.Current.ToString() != dtChild.Columns[0].ColumnName.ToString() && dColoumnRow.Current.ToString() != dtChild.Columns[1].ColumnName.ToString())
                                    {
                                        cell = worksheet.Cell(row, childCol + 1);
                                        childCol++;

                                        cell.Value = System.Security.SecurityElement.Escape(dColoumnRow.Current.ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = centerTextStyleID;
                                        childCol++;
                                    }
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);
                        }

                        row++;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];
                                cell = worksheet.Cell(row, col + 1);

                                clmCount = 1;

                                for (int colVal = 1; colVal < dtParent.Columns.Count; colVal++)
                                {
                                    //if (dtParent.Columns[colVal].ColumnName != null && dtParent.Columns[colVal].ColumnName != "")
                                    //{
                                    //    cell = worksheet.Cell(row, clmCount);
                                    //    cell.Value = dtParent.Columns[colVal].ColumnName;
                                    //    cell.StyleID = acctheaderStyleID;
                                    //}

                                    if (dParentRow[colVal].ToString() != null && dParentRow[colVal].ToString() != "")
                                    {
                                        cell = worksheet.Cell(row, clmCount);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow[colVal].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    }
                                    clmCount = clmCount + 4;
                                }

                                if (dtChild != null)
                                {
                                    if (dParentRow["TrxID"].ToString() != null && dParentRow["TrxID"].ToString() != "")
                                    {
                                        int TrxID = Convert.ToInt32(dParentRow["TrxID"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select(dtParent.Columns[1].ColumnName + "_TrxID ='" + TrxID + "'");

                                        row++;
                                        worksheet.InsertRow(row);

                                        bool noChild = false;

                                        if (drLinkedChildRows.Length == 0)
                                            noChild = true;

                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            childCol = 0;
                                            for (int childColCount = 2; childColCount < dtChild.Columns.Count; childColCount++)
                                            {
                                                cell = worksheet.Cell(row, childCol + 1);
                                                childCol++;
                                                cell.Value = System.Security.SecurityElement.Escape(dRow[childColCount].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                                if (htColName[dtChild.Columns[childColCount].ColumnName] == "SUMMED" || htColName[dtChild.Columns[childColCount].ColumnName] == "AMOUNT")
                                                {
                                                    cell.Value = cell.Value.ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                childCol++;

                                                if (childCount == 0)
                                                {
                                                    if (htColName[dtChild.Columns[childColCount].ColumnName] != null && (htColName[dtChild.Columns[childColCount].ColumnName].Equals("SUMMED") || htColName[dtChild.Columns[childColCount].ColumnName].Equals("AMOUNT")))
                                                    {
                                                        htTotal[dtChild.Columns[childColCount].ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            row++;
                                        }

                                        if (!noChild)
                                        {
                                            worksheet.InsertRow(row);
                                            rowInserted = true;

                                            cell = worksheet.Cell(row, col + 1);
                                            cell.Value = "Total";
                                            cell.StyleID = acctheaderStyleID;

                                            childCol = 0;
                                            //to skip trxid and _trxid colcount starts from 2
                                            for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                            {
                                                cellAddr = string.Empty;
                                                IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                                while (enumCol.MoveNext())
                                                {
                                                    if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                                    {
                                                        endRowCellAddr = worksheet.Cell(row - 1, childCol + 1).CellAddress;
                                                        cell = worksheet.Cell(row, childCol + 1);
                                                        cell.RemoveValue();
                                                        cell.Formula = string.Format("SUM({0}:{1})", enumCol.Value, endRowCellAddr);

                                                        if (htColName[dtChild.Columns[colCount].ColumnName] == "SUMMED" || htColName[dtChild.Columns[colCount].ColumnName] == "AMOUNT")
                                                        {
                                                            cell.StyleID = amtStyleID;
                                                        }

                                                        string colName = dtChild.Columns[colCount].ColumnName;

                                                        if (htGrandTotal[colName] != null && htGrandTotal[colName].ToString() != "")
                                                        {
                                                            htGrandTotal[colName] = htGrandTotal[colName].ToString() + "+" + cell.CellAddress;
                                                        }
                                                        else
                                                        {
                                                            htGrandTotal[colName] = cell.CellAddress;
                                                        }
                                                        break;
                                                    }
                                                }

                                                childCol = childCol + 2;
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }
                                    }
                                }
                                else
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                    row++;
                                }
                            }

                            if (dtChild != null)
                            {
                                //For Grand Totals
                                row++;
                                worksheet.InsertRow(row);

                                childCol = 0;
                                cell = worksheet.Cell(row, childCol + 1);
                                cell.Value = "Grand Total";
                                cell.StyleID = acctheaderStyleID;

                                //row++;

                                for (int colCount = 2; colCount < dtChild.Columns.Count; colCount++)
                                {
                                    IDictionaryEnumerator enumCol = htTotal.GetEnumerator();

                                    while (enumCol.MoveNext())
                                    {
                                        if (dtChild.Columns[colCount].ColumnName == enumCol.Key.ToString())
                                        {
                                            cell = worksheet.Cell(row, childCol + 1);
                                            cell.RemoveValue();
                                            cell.Formula = htGrandTotal[dtChild.Columns[colCount].ColumnName].ToString();
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                    }

                                    childCol = childCol + 2;
                                }
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        //Report Style for Trail Balance regular (Parent)
        public void ReportStyle601(DataTable dtParent, DataTable dtHeader, Hashtable htColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string tDebitStartAddr = string.Empty;
            string tCreditStartAddr = string.Empty;
            string iDebitStartAddr = string.Empty;
            string iCreditStartAddr = string.Empty;
            string bDebitStartAddr = string.Empty;
            string bCreditStartAddr = string.Empty;

            string tDebitEndAddr = string.Empty;
            string tCreditEndAddr = string.Empty;
            string iDebitEndAddr = string.Empty;
            string iCreditEndAddr = string.Empty;
            string bDebitEndAddr = string.Empty;
            string bCreditEndAddr = string.Empty;

            string totalTdebitAddr = string.Empty;
            string totalTcreditAddr = string.Empty;
            string totalIdebitAddr = string.Empty;
            string totalIcreditAddr = string.Empty;
            string totalBdebitAddr = string.Empty;
            string totalBcreditAddr = string.Empty;

            string TdebitProfitAddr = string.Empty;
            string TcreditProfitAddr = string.Empty;
            string IdebitProfitAddr = string.Empty;
            string IcreditProfitAddr = string.Empty;
            string BdebitProfitAddr = string.Empty;
            string BcreditProfitAddr = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            int acctheaderStyleID = 0;
            const int startRow = 8;
            int row = startRow;
            int col = 0;
            int totalRowValue = 0;

            int amtStyleID;
            int arrayCount = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\TB regular" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-601.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-601.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["TB regular"];
                        ExcelCell cell;

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        amtStyleID = worksheet.Cell(8, 5).StyleID;

                        tDebitStartAddr = worksheet.Cell(8, 5).CellAddress;
                        tCreditStartAddr = worksheet.Cell(8, 6).CellAddress;

                        iDebitStartAddr = worksheet.Cell(8, 8).CellAddress;
                        iCreditStartAddr = worksheet.Cell(8, 9).CellAddress;

                        bDebitStartAddr = worksheet.Cell(8, 11).CellAddress;
                        bCreditStartAddr = worksheet.Cell(8, 12).CellAddress;

                        if (dtParent != null)
                        {
                            for (int i = 0; i < dtParent.Rows.Count - 1; i++)
                            {
                                if (row > startRow)
                                {
                                    worksheet.InsertRow(row);
                                }

                                DataRow dRow = dtParent.Rows[i];

                                if (dRow["Account"].ToString() != "SKIP")
                                {
                                    if (dRow["Account"].ToString() != "" && dRow["Account"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    }

                                    if (dRow["Description"].ToString() != "" && dRow["Description"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 2);
                                        if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                        {
                                            dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                            cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                        }
                                    }

                                    if (dRow["TBDB"].ToString() != "" && dRow["TBDB"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 5);
                                        cell.Value = dRow["TBDB"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    if (dRow["TBCR"].ToString() != "" && dRow["TBCR"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 6);
                                        cell.Value = dRow["TBCR"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    if (dRow["PLDB"].ToString() != "" && dRow["PLDB"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 8);
                                        cell.Value = dRow["PLDB"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    if (dRow["PLCR"].ToString() != "" && dRow["PLCR"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 9);
                                        cell.Value = dRow["PLCR"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    if (dRow["BSDB"].ToString() != "" && dRow["BSDB"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 11);
                                        cell.Value = dRow["BSDB"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }

                                    if (dRow["BSCR"].ToString() != "" && dRow["BSCR"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 12);
                                        cell.Value = dRow["BSCR"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;
                                    }
                                }

                                row++;
                                totalRowValue = row + 1;
                            }

                            tDebitEndAddr = worksheet.Cell(row - 1, 5).CellAddress;
                            tCreditEndAddr = worksheet.Cell(row - 1, 6).CellAddress;

                            iDebitEndAddr = worksheet.Cell(row - 1, 8).CellAddress;
                            iCreditEndAddr = worksheet.Cell(row - 1, 9).CellAddress;

                            bDebitEndAddr = worksheet.Cell(row - 1, 11).CellAddress;
                            bCreditEndAddr = worksheet.Cell(row - 1, 12).CellAddress;

                            //Formula for Total
                            cell = worksheet.Cell(totalRowValue, 5);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", tDebitStartAddr, tDebitEndAddr);
                            totalTdebitAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 6);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", tCreditStartAddr, tCreditEndAddr);
                            totalTcreditAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 8);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", iDebitStartAddr, iDebitEndAddr);
                            totalIdebitAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 9);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", iCreditStartAddr, iCreditEndAddr);
                            totalIcreditAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 11);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", bDebitStartAddr, bDebitEndAddr);
                            totalBdebitAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 12);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", bCreditStartAddr, bCreditEndAddr);
                            totalBcreditAddr = cell.CellAddress;

                            //Formula for NetProfit
                            int netProfitRowValue = totalRowValue + 2;

                            cell = worksheet.Cell(netProfitRowValue, 5);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalTcreditAddr + ">" + totalTdebitAddr + "," + totalTcreditAddr + "-" + totalTdebitAddr + "," + "0" + ")";
                            TdebitProfitAddr = cell.CellAddress;

                            cell = worksheet.Cell(netProfitRowValue, 6);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalTcreditAddr + "<" + totalTdebitAddr + "," + totalTdebitAddr + "-" + totalTcreditAddr + "," + "0" + ")";
                            TcreditProfitAddr = cell.CellAddress;

                            cell = worksheet.Cell(netProfitRowValue, 8);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalIcreditAddr + ">" + totalIdebitAddr + "," + totalIcreditAddr + "-" + totalIdebitAddr + "," + "0" + ")";
                            IdebitProfitAddr = cell.CellAddress;

                            cell = worksheet.Cell(netProfitRowValue, 9);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalIcreditAddr + "<" + totalIdebitAddr + "," + totalIdebitAddr + "-" + totalIcreditAddr + "," + "0" + ")";
                            IcreditProfitAddr = cell.CellAddress;

                            cell = worksheet.Cell(netProfitRowValue, 11);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalBcreditAddr + ">" + totalBdebitAddr + "," + totalBcreditAddr + "-" + totalBdebitAddr + "," + "0" + ")";
                            BdebitProfitAddr = cell.CellAddress;

                            cell = worksheet.Cell(netProfitRowValue, 12);
                            cell.RemoveValue();
                            cell.Formula = "if" + "(" + totalBcreditAddr + "<" + totalBdebitAddr + "," + totalBdebitAddr + "-" + totalBcreditAddr + "," + "0" + ")";
                            BcreditProfitAddr = cell.CellAddress;

                            //Formula for GrandTotal
                            int grandTotalRowValue = netProfitRowValue + 2;

                            cell = worksheet.Cell(grandTotalRowValue, 5);
                            cell.RemoveValue();
                            cell.Formula = totalTdebitAddr + "+" + TdebitProfitAddr;

                            cell = worksheet.Cell(grandTotalRowValue, 6);
                            cell.RemoveValue();
                            cell.Formula = totalTcreditAddr + "+" + TcreditProfitAddr;

                            cell = worksheet.Cell(grandTotalRowValue, 8);
                            cell.RemoveValue();
                            cell.Formula = totalIdebitAddr + "+" + IdebitProfitAddr;

                            cell = worksheet.Cell(grandTotalRowValue, 9);
                            cell.RemoveValue();
                            cell.Formula = totalIcreditAddr + "+" + IcreditProfitAddr;

                            cell = worksheet.Cell(grandTotalRowValue, 11);
                            cell.RemoveValue();
                            cell.Formula = totalBdebitAddr + "+" + BdebitProfitAddr;

                            cell = worksheet.Cell(grandTotalRowValue, 12);
                            cell.RemoveValue();
                            cell.Formula = totalBcreditAddr + "+" + BcreditProfitAddr;
                        }
                        xlPackage.Save();
                    }
                }

                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Report Style for Trail Balance forward (Parent)
        public void ReportStyle602(DataTable dtParent, DataTable dtHeader, Hashtable htColNameValues)
        {
            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string bForwardStartAddr = string.Empty;
            string aDebitStartAddr = string.Empty;
            string aCreditStartAddr = string.Empty;
            string eBalanceStartAddr = string.Empty;

            string bForwardEndAddr = string.Empty;
            string aDebitEndAddr = string.Empty;
            string aCreditEndAddr = string.Empty;
            string eBalanceEndAddr = string.Empty;

            string totalBforwardAddr = string.Empty;
            string totalAdebitAddr = string.Empty;
            string totalAcreditAddr = string.Empty;
            string totalEbalanceAddr = string.Empty;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            ExcelCell cell;

            int acctheaderStyleID = 0;
            const int startRow = 8;
            int row = startRow;
            int col = 0;
            int totalRowValue = 0;

            int bForwardStyleID;
            int aDebitStyleID;
            int aCreditStyleID;
            int eBalanceStyleID;
            int arrayCount = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\TB bal forward" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-602.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-602.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["TB bal forward"];

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        bForwardStyleID = worksheet.Cell(8, 7).StyleID;
                        aDebitStyleID = worksheet.Cell(8, 9).StyleID;
                        aCreditStyleID = worksheet.Cell(8, 10).StyleID;
                        eBalanceStyleID = worksheet.Cell(8, 12).StyleID;

                        bForwardStartAddr = worksheet.Cell(8, 7).CellAddress;
                        aDebitStartAddr = worksheet.Cell(8, 9).CellAddress;
                        aCreditStartAddr = worksheet.Cell(8, 10).CellAddress;
                        eBalanceStartAddr = worksheet.Cell(8, 12).CellAddress;

                        string balforward = string.Empty;
                        string activityDebit = string.Empty;
                        string activityCredit = string.Empty;

                        if (dtParent != null)
                        {
                            for (int i = 0; i < dtParent.Rows.Count - 1; i++)
                            {
                                if (row > startRow)
                                {
                                    worksheet.InsertRow(row);
                                }

                                DataRow dRow = dtParent.Rows[i];

                                if (dRow["Account"].ToString() != "SKIP")
                                {
                                    if (dRow["Account"].ToString() != "" && dRow["Account"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    }

                                    if (dRow["Description"].ToString() != "" && dRow["Description"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 2);
                                        if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                        {
                                            dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                            cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                        }
                                    }

                                    if (dRow["begbal"].ToString() != "" && dRow["begbal"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 7);
                                        cell.Value = dRow["begbal"].ToString().Replace(",", "");
                                        cell.StyleID = bForwardStyleID;
                                        balforward = cell.CellAddress;
                                    }

                                    if (dRow["debits"].ToString() != "" && dRow["debits"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 9);
                                        cell.Value = dRow["debits"].ToString().Replace(",", "");
                                        cell.StyleID = aDebitStyleID;
                                        activityDebit = cell.CellAddress;
                                    }

                                    if (dRow["credits"].ToString() != "" && dRow["credits"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 10);
                                        cell.Value = dRow["credits"].ToString().Replace(",", "");
                                        cell.StyleID = aCreditStyleID;
                                        activityCredit = cell.CellAddress;
                                    }

                                    //if (dRow["Ending Balance"].ToString() != "" && dRow["Ending Balance"].ToString() != null)
                                    //{
                                    //    cell = worksheet.Cell(row, col + 12);
                                    //    cell.Value = dRow["Ending Balance"].ToString().Replace(",", "");
                                    //    cell.StyleID = eBalanceStyleID;
                                    //}

                                    cell = worksheet.Cell(row, col + 12);
                                    cell.RemoveValue();
                                    cell.Formula = balforward + "+" + activityDebit + "-" + activityCredit;
                                    cell.StyleID = aCreditStyleID;
                                }

                                row++;
                                totalRowValue = row + 1;
                            }

                            bForwardEndAddr = worksheet.Cell(row - 1, 7).CellAddress;
                            aDebitEndAddr = worksheet.Cell(row - 1, 9).CellAddress;
                            aCreditEndAddr = worksheet.Cell(row - 1, 10).CellAddress;
                            eBalanceEndAddr = worksheet.Cell(row - 1, 12).CellAddress;

                            //Formula for Total
                            cell = worksheet.Cell(totalRowValue, 7);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", bForwardStartAddr, bForwardEndAddr);
                            totalBforwardAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 9);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", aDebitStartAddr, aDebitEndAddr);
                            totalAdebitAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 10);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", aCreditStartAddr, aCreditEndAddr);
                            totalAcreditAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 12);
                            cell.RemoveValue();
                            cell.Formula = string.Format("SUM({0}:{1})", eBalanceStartAddr, eBalanceEndAddr);
                            totalEbalanceAddr = cell.CellAddress;

                            ////For NetProfit
                            //int netProfitRowValue = totalRowValue + 2;

                            //cell = worksheet.Cell(netProfitRowValue, 9);
                            //cell.RemoveValue();
                            //cell.Formula = "if" + "(" + totalAcreditAddr + ">" + totalAdebitAddr + "," + totalAcreditAddr + "-" + totalAdebitAddr + "," + "0" + ")";

                            //cell = worksheet.Cell(netProfitRowValue, 10);
                            //cell.RemoveValue();
                            //cell.Formula = "if" + "(" + totalAcreditAddr + "<" + totalAdebitAddr + "," + totalAdebitAddr + "-" + totalAcreditAddr + "," + "0" + ")";
                        }
                        xlPackage.Save();
                    }

                    SaveToClient(newFile);
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Report Style for Trail Balance Sub-Accounts forward (Parent & Child)
        public void ReportStyle603(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;

            string totalBForward = string.Empty;
            string totalADebit = string.Empty;
            string totalACredit = string.Empty;
            string totalEBalance = string.Empty;

            string cBForwardStartAddr = string.Empty;
            string cActivityDStartAddr = string.Empty;
            string cActivityCStartAddr = string.Empty;
            string eBalanceStartAddr = string.Empty;

            string cbForwardEndAddr = string.Empty;
            string cActivityDEndAddr = string.Empty;
            string cActivityCEndAddr = string.Empty;
            string eBalanceEndAddr = string.Empty;

            string tBForwardCellAddress = string.Empty;
            string aDebitCellAddress = string.Empty;
            string aCreditCellAddress = string.Empty;
            string eBalanceCellAddress = string.Empty;

            string bForwardAddr = string.Empty;
            string aDebitAddr = string.Empty;
            string aCreditAddr = string.Empty;
            string eBalanceAddr = string.Empty;

            int bForwardStyleID;
            int aDebitStyleID;
            int aCreditStyleID;
            int eBalanceStyleID;

            int acctHeaderStyleID;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            ExcelCell cell;

            const int startRow = 8;
            int row = startRow;
            int col = 0;
            int arrayCount = 0;

            int totalRowValue = 0;
            bool rowInserted = false;
            bool noChild = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\TB bal sub-accounts forward" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-603.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-603.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["TB bal sub-accounts forward"];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;

                        bForwardStyleID = worksheet.Cell(8, 7).StyleID;

                        aDebitStyleID = worksheet.Cell(8, 9).StyleID;
                        aCreditStyleID = worksheet.Cell(8, 10).StyleID;

                        eBalanceStyleID = worksheet.Cell(8, 12).StyleID;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                            {
                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];

                                if (dParentRow["Account"].ToString() != "SKIP")
                                {
                                    if (dParentRow["Account"].ToString() != "" && dParentRow["Account"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = acctHeaderStyleID;
                                    }

                                    if (dParentRow["Description"].ToString() != "" && dParentRow["Description"].ToString() != null)
                                    {
                                        cell = worksheet.Cell(row, col + 2);
                                        if (!string.IsNullOrEmpty(dParentRow["Description"].ToString()))
                                        {
                                            dParentRow["Description"] = System.Security.SecurityElement.Escape(dParentRow["Description"].ToString());
                                            cell.Value = dParentRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                        }

                                        cell.StyleID = acctHeaderStyleID;
                                    }

                                    if (dParentRow["Link1"].ToString() != "" && dParentRow["Link1"].ToString() != null)
                                    {
                                        int link = Convert.ToInt32(dParentRow["Link1"].ToString());

                                        DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link + "'");

                                        row++;

                                        cBForwardStartAddr = "";
                                        cActivityDStartAddr = "";
                                        cActivityCStartAddr = "";
                                        eBalanceStartAddr = "";

                                        cBForwardStartAddr = worksheet.Cell(row, col + 7).CellAddress;
                                        cActivityDStartAddr = worksheet.Cell(row, col + 9).CellAddress;
                                        cActivityCStartAddr = worksheet.Cell(row, col + 10).CellAddress;
                                        eBalanceStartAddr = worksheet.Cell(row, col + 12).CellAddress;

                                        string balForward = string.Empty;
                                        string activityDebit = string.Empty;
                                        string activityCredit = string.Empty;

                                        if (drLinkedChildRows.Length == 0)
                                        {
                                            noChild = true;
                                        }

                                        for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                        {
                                            DataRow dRow = drLinkedChildRows[childCount];

                                            if (row > startRow)
                                            {
                                                worksheet.InsertRow(row);
                                            }

                                            if (dRow["Description"].ToString() != "" && dRow["Description"].ToString() != null)
                                            {
                                                cell = worksheet.Cell(row, col + 1);

                                                if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                                {
                                                    dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                    cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                }
                                            }

                                            if (dRow.Table.Columns.Contains("Account"))
                                            {
                                                if (dRow["Account"].ToString() != "" && dRow["Account"].ToString() != null)
                                                {
                                                    cell = worksheet.Cell(row, col + 2);
                                                    cell.Value = System.Security.SecurityElement.Escape(dRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                }
                                            }

                                            if (dRow["begbal"].ToString() != "" && dRow["begbal"].ToString() != null)
                                            {
                                                cell = worksheet.Cell(row, col + 7);
                                                cell.Value = dRow["begbal"].ToString().Replace(",", "");
                                                cell.StyleID = bForwardStyleID;
                                                balForward = cell.CellAddress;
                                            }

                                            if (dRow["debits"].ToString() != "" && dRow["debits"].ToString() != null)
                                            {
                                                cell = worksheet.Cell(row, col + 9);
                                                cell.Value = dRow["debits"].ToString().Replace(",", "");
                                                cell.StyleID = aDebitStyleID;
                                                activityDebit = cell.CellAddress;
                                            }

                                            if (dRow["credits"].ToString() != "" && dRow["credits"].ToString() != null)
                                            {
                                                cell = worksheet.Cell(row, col + 10);
                                                cell.Value = dRow["credits"].ToString().Replace(",", "");
                                                cell.StyleID = aCreditStyleID;
                                                activityCredit = cell.CellAddress;
                                            }

                                            //if (dRow["Ending Balance"].ToString() != "" && dRow["Ending Balance"].ToString() != null)
                                            //{
                                            //    cell = worksheet.Cell(row, col + 12);
                                            //    cell.Value = dRow["Ending Balance"].ToString().Replace(",", "");
                                            //    cell.StyleID = eBalanceStyleID;
                                            //}

                                            cell = worksheet.Cell(row, col + 12);
                                            cell.Formula = balForward + "+" + activityDebit + "-" + activityCredit;
                                            cell.StyleID = aCreditStyleID;

                                            row++;

                                            cell = worksheet.Cell(row, 7);
                                            if (totalBForward == string.Empty)
                                                totalBForward = cell.CellAddress;
                                            else
                                                totalBForward = totalBForward + "+" + cell.CellAddress;

                                            cell = worksheet.Cell(row, 9);
                                            if (totalADebit == string.Empty)
                                                totalADebit = cell.CellAddress;
                                            else
                                                totalADebit = totalADebit + "+" + cell.CellAddress;

                                            cell = worksheet.Cell(row, 10);
                                            if (totalACredit == string.Empty)
                                                totalACredit = cell.CellAddress;
                                            else
                                                totalACredit = totalACredit + "+" + cell.CellAddress;

                                            cell = worksheet.Cell(row, 12);
                                            if (totalEBalance == string.Empty)
                                                totalEBalance = cell.CellAddress;
                                            else
                                                totalEBalance = totalEBalance + "+" + cell.CellAddress;
                                        }
                                    }

                                    worksheet.InsertRow(row);
                                    rowInserted = true;

                                    if (!noChild)
                                    {
                                        cell = worksheet.Cell(row, 1);
                                        cell.Value = "Total of " + dParentRow["Account"].ToString() + "-" + dParentRow["Description"].ToString();

                                        cbForwardEndAddr = "";
                                        cActivityDEndAddr = "";
                                        cActivityCEndAddr = "";
                                        eBalanceEndAddr = "";

                                        cbForwardEndAddr = worksheet.Cell(row - 1, 7).CellAddress;
                                        cActivityDEndAddr = worksheet.Cell(row - 1, 9).CellAddress;
                                        cActivityCEndAddr = worksheet.Cell(row - 1, 10).CellAddress;
                                        eBalanceEndAddr = worksheet.Cell(row - 1, 12).CellAddress;

                                        cell = worksheet.Cell(row, 7);
                                        cell.RemoveValue();
                                        cell.StyleID = bForwardStyleID;
                                        cell.Formula = string.Format("SUM({0}:{1})", cBForwardStartAddr, cbForwardEndAddr);

                                        cell = worksheet.Cell(row, 9);
                                        cell.RemoveValue();
                                        cell.StyleID = aDebitStyleID;
                                        cell.Formula = string.Format("SUM({0}:{1})", cActivityDStartAddr, cActivityDEndAddr);

                                        cell = worksheet.Cell(row, 10);
                                        cell.RemoveValue();
                                        cell.StyleID = aCreditStyleID;
                                        cell.Formula = string.Format("SUM({0}:{1})", cActivityCStartAddr, cActivityCEndAddr);

                                        cell = worksheet.Cell(row, 12);
                                        cell.RemoveValue();
                                        cell.StyleID = eBalanceStyleID;
                                        cell.Formula = string.Format("SUM({0}:{1})", eBalanceStartAddr, eBalanceEndAddr);
                                    }
                                }

                                totalRowValue = row;
                            }

                            //Formula for Total
                            totalRowValue = totalRowValue + 2;

                            cell = worksheet.Cell(totalRowValue, 7);
                            cell.RemoveValue();
                            cell.Formula = totalBForward;
                            bForwardAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 9);
                            cell.RemoveValue();
                            cell.Formula = totalADebit;
                            aDebitAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 10);
                            cell.RemoveValue();
                            cell.Formula = totalACredit;
                            aCreditAddr = cell.CellAddress;

                            cell = worksheet.Cell(totalRowValue, 12);
                            cell.RemoveValue();
                            cell.Formula = totalEBalance;
                            eBalanceAddr = cell.CellAddress;

                            ////Formula for NetProfit Loss
                            //totalRowValue = totalRowValue + 2;

                            //cell = worksheet.Cell(totalRowValue, 9);
                            //cell.RemoveValue();
                            //cell.Formula = "if" + "(" + aCreditAddr + ">" + aDebitAddr + "," + aCreditAddr + "-" + aDebitAddr + "," + "0" + ")";

                            //cell = worksheet.Cell(totalRowValue, 10);
                            //cell.RemoveValue();
                            //cell.Formula = "if" + "(" + aCreditAddr + "<" + aDebitAddr + "," + aDebitAddr + "-" + aCreditAddr + "," + "0" + ")";
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Report Style for Trail Balance Sub-Accounts regular (Parent & Child)
        public void ReportStyle604(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColNameValues)
        {
            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string totalTDebit = string.Empty;
            string totalTCredit = string.Empty;

            string totalIDebit = string.Empty;
            string totalICredit = string.Empty;

            string totalBDebit = string.Empty;
            string totalBCredit = string.Empty;

            string tDebitCellAddress = string.Empty;
            string tCreditCellAddress = string.Empty;
            string iDebitCellAddress = string.Empty;
            string iCreditCellAddress = string.Empty;
            string bDebitCellAddress = string.Empty;
            string bCreditCellAddress = string.Empty;

            string tDebitAccTotal = string.Empty;
            string tCreditAccTotal = string.Empty;
            string iDebitAccTotal = string.Empty;
            string iCreditAccTotal = string.Empty;
            string bDebitAccTotal = string.Empty;
            string bCreditAccTotal = string.Empty;

            string tDebitAddr = string.Empty;
            string tCreditAddr = string.Empty;
            string iDebitAddr = string.Empty;
            string iCreditAddr = string.Empty;
            string bDebitAddr = string.Empty;
            string bCreditAddr = string.Empty;

            string cTrailDStartAddr = string.Empty;
            string cTrailCStartAddr = string.Empty;
            string cIncomeDStartAddr = string.Empty;
            string cIncomeCStartAddr = string.Empty;
            string cBalanceDStartAddr = string.Empty;
            string cBalanceCStartAddr = string.Empty;

            string cTrailDEndAddr = string.Empty;
            string cTrailCEndAddr = string.Empty;
            string cIncomeDEndAddr = string.Empty;
            string cIncomeCEndAddr = string.Empty;
            string cBalanceDEndAddr = string.Empty;
            string cBalanceCEndAddr = string.Empty;

            string tProfitAddr = string.Empty;
            string tProfitCreditAddr = string.Empty;
            string iProfitAddr = string.Empty;
            string iProfitCreditAddr = string.Empty;
            string bProfitAddr = string.Empty;
            string bProfitCreditAddr = string.Empty;

            int amtStyleID;

            int acctHeaderStyleID;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            ExcelCell cell;

            const int startRow = 8;
            int row = startRow;
            int col = 0;
            int arrayCount = 0;

            int totalRowValue = 0;
            bool rowInserted = false;
            bool noChild = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\TB sub-accounts regular" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-604.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-604.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["TB sub-accounts regular"];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;

                        amtStyleID = worksheet.Cell(8, 5).StyleID;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        for (int parentCount = 0; parentCount < dtParent.Rows.Count - 1; parentCount++)
                        {
                            if (row > startRow)
                            {
                                if (rowInserted)
                                {
                                    row = row + 1;
                                    worksheet.InsertRow(row);
                                }
                                else
                                {
                                    worksheet.InsertRow(row);
                                }
                            }

                            DataRow dParentRow = dtParent.Rows[parentCount];

                            if (dParentRow["Account"].ToString() != "SKIP")
                            {
                                if (dParentRow["Account"].ToString() != "" && dParentRow["Account"].ToString() != null)
                                {
                                    cell = worksheet.Cell(row, col + 1);
                                    cell.Value = System.Security.SecurityElement.Escape(dParentRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                }

                                if (DoesCellExist("Description", dParentRow))
                                {
                                    cell = worksheet.Cell(row, col + 2);
                                    if (!string.IsNullOrEmpty(dParentRow["Description"].ToString()))
                                    {
                                        dParentRow["Description"] = System.Security.SecurityElement.Escape(dParentRow["Description"].ToString());
                                        cell.Value = dParentRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                    }

                                    cell.StyleID = acctHeaderStyleID;
                                }

                                if (DoesCellExist("Link1", dParentRow))
                                {
                                    int link = Convert.ToInt32(dParentRow["Link1"].ToString());

                                    DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link + "'");

                                    row++;

                                    cTrailDStartAddr = "";
                                    cTrailCStartAddr = "";

                                    cIncomeDStartAddr = "";
                                    cIncomeCStartAddr = "";

                                    cBalanceDStartAddr = "";
                                    cBalanceCStartAddr = "";

                                    cTrailDStartAddr = worksheet.Cell(row, col + 5).CellAddress;
                                    cTrailCStartAddr = worksheet.Cell(row, col + 6).CellAddress;

                                    cIncomeDStartAddr = worksheet.Cell(row, col + 8).CellAddress;
                                    cIncomeCStartAddr = worksheet.Cell(row, col + 9).CellAddress;

                                    cBalanceDStartAddr = worksheet.Cell(row, col + 11).CellAddress;
                                    cBalanceCStartAddr = worksheet.Cell(row, col + 12).CellAddress;

                                    if (drLinkedChildRows.Length == 0)
                                    {
                                        noChild = true;
                                    }

                                    for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                    {
                                        DataRow dRow = drLinkedChildRows[childCount];

                                        if (row > startRow)
                                        {
                                            worksheet.InsertRow(row);
                                        }

                                        if (DoesCellExist("Description", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 1);

                                            if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                            {
                                                dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                            }
                                        }


                                        if (DoesCellExist("Account", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 2);
                                            cell.Value = System.Security.SecurityElement.Escape(dRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        }

                                        if (DoesCellExist("TBDB", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 5);
                                            cell.Value = dRow["TBDB"].ToString().Replace(",", "");

                                            if (tDebitCellAddress == string.Empty)
                                            {
                                                tDebitCellAddress = cell.CellAddress;
                                            }
                                            else
                                                tDebitCellAddress = tDebitCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }


                                        if (DoesCellExist("TBCR", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 6);
                                            cell.Value = dRow["TBCR"].ToString().Replace(",", "");

                                            if (tCreditCellAddress == string.Empty)
                                            {
                                                tCreditCellAddress = cell.CellAddress;
                                            }
                                            else
                                                tCreditCellAddress = tCreditCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }


                                        if (DoesCellExist("PLDB", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 8);
                                            cell.Value = dRow["PLDB"].ToString().Replace(",", "");

                                            if (iDebitCellAddress == string.Empty)
                                            {
                                                iDebitCellAddress = cell.CellAddress;
                                            }
                                            else
                                                iDebitCellAddress = iDebitCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }

                                        if (DoesCellExist("PLCR", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 9);
                                            cell.Value = dRow["PLCR"].ToString().ToString().Replace(",", "");

                                            if (iCreditCellAddress == string.Empty)
                                            {
                                                iCreditCellAddress = cell.CellAddress;
                                            }
                                            else
                                                iCreditCellAddress = iCreditCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }

                                        if (DoesCellExist("BSDB", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 11);
                                            cell.Value = dRow["BSDB"].ToString().Replace(",", "");

                                            if (bDebitCellAddress == string.Empty)
                                            {
                                                bDebitCellAddress = cell.CellAddress;
                                            }
                                            else
                                                bDebitCellAddress = bDebitCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }

                                        if (DoesCellExist("BSCR", dRow))
                                        {
                                            cell = worksheet.Cell(row, col + 12);
                                            cell.Value = dRow["BSCR"].ToString().Replace(",", "");

                                            if (bCreditCellAddress == string.Empty)
                                            {
                                                bCreditCellAddress = cell.CellAddress;
                                            }
                                            else
                                                bCreditCellAddress = bCreditCellAddress + "+" + cell.CellAddress;

                                            cell.StyleID = amtStyleID;
                                        }

                                        row++;

                                        cell = worksheet.Cell(row, 5);
                                        if (totalTDebit == string.Empty)
                                            totalTDebit = cell.CellAddress;
                                        else
                                            totalTDebit = totalTDebit + "+" + cell.CellAddress;

                                        cell = worksheet.Cell(row, 6);
                                        if (totalTCredit == string.Empty)
                                            totalTCredit = cell.CellAddress;
                                        else
                                            totalTCredit = totalTCredit + "+" + cell.CellAddress;

                                        cell = worksheet.Cell(row, 8);
                                        if (totalIDebit == string.Empty)
                                            totalIDebit = cell.CellAddress;
                                        else
                                            totalIDebit = totalIDebit + "+" + cell.CellAddress;

                                        cell = worksheet.Cell(row, 9);
                                        if (totalICredit == string.Empty)
                                            totalICredit = cell.CellAddress;
                                        else
                                            totalICredit = totalICredit + "+" + cell.CellAddress;

                                        cell = worksheet.Cell(row, 11);
                                        if (totalBDebit == string.Empty)
                                            totalBDebit = cell.CellAddress;
                                        else
                                            totalBDebit = totalBDebit + "+" + cell.CellAddress;

                                        cell = worksheet.Cell(row, 12);
                                        if (totalBCredit == string.Empty)
                                            totalBCredit = cell.CellAddress;
                                        else
                                            totalBCredit = totalBCredit + "+" + cell.CellAddress;
                                    }
                                }
                                worksheet.InsertRow(row);
                                rowInserted = true;

                                if (!noChild)
                                {
                                    cell = worksheet.Cell(row, 1);
                                    cell.Value = "Total of " + dParentRow["Account"].ToString() + "-" + dParentRow["Description"].ToString();

                                    cTrailDEndAddr = "";
                                    cTrailCEndAddr = "";

                                    cIncomeDEndAddr = "";
                                    cIncomeCEndAddr = "";

                                    cBalanceDEndAddr = "";
                                    cBalanceCEndAddr = "";

                                    cTrailDEndAddr = worksheet.Cell(row - 1, 5).CellAddress;
                                    cTrailCEndAddr = worksheet.Cell(row - 1, 6).CellAddress;

                                    cIncomeDEndAddr = worksheet.Cell(row - 1, 8).CellAddress;
                                    cIncomeCEndAddr = worksheet.Cell(row - 1, 9).CellAddress;

                                    cBalanceDEndAddr = worksheet.Cell(row - 1, 11).CellAddress;
                                    cBalanceCEndAddr = worksheet.Cell(row - 1, 12).CellAddress;

                                    cell = worksheet.Cell(row, 5);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cTrailDStartAddr, cTrailDEndAddr);

                                    cell = worksheet.Cell(row, 6);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cTrailCStartAddr, cTrailCEndAddr);

                                    cell = worksheet.Cell(row, 8);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cIncomeDStartAddr, cIncomeDEndAddr);

                                    cell = worksheet.Cell(row, 9);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cIncomeCStartAddr, cIncomeCEndAddr);

                                    cell = worksheet.Cell(row, 11);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cBalanceDStartAddr, cBalanceDEndAddr);

                                    cell = worksheet.Cell(row, 12);
                                    cell.RemoveValue();
                                    cell.StyleID = amtStyleID;
                                    cell.Formula = string.Format("SUM({0}:{1})", cBalanceCStartAddr, cBalanceCEndAddr);
                                }
                            }

                            totalRowValue = row;
                        }

                        //Formula for Total
                        totalRowValue = totalRowValue + 2;

                        cell = worksheet.Cell(totalRowValue, 5);
                        cell.RemoveValue();
                        cell.Formula = totalTDebit;
                        tDebitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 6);
                        cell.RemoveValue();
                        cell.Formula = totalTCredit;
                        tCreditAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 8);
                        cell.RemoveValue();
                        cell.Formula = totalIDebit;
                        iDebitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 9);
                        cell.RemoveValue();
                        cell.Formula = totalICredit;
                        iCreditAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 11);
                        cell.RemoveValue();
                        cell.Formula = totalBDebit;
                        bDebitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 12);
                        cell.RemoveValue();
                        cell.Formula = totalBCredit;
                        bCreditAddr = cell.CellAddress;

                        //Formula for NetProfit Loss
                        totalRowValue = totalRowValue + 2;

                        cell = worksheet.Cell(totalRowValue, 5);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + tCreditAddr + ">" + tDebitAddr + "," + tCreditAddr + "-" + tDebitAddr + "," + "0" + ")";
                        tProfitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 6);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + tCreditAddr + "<" + tDebitAddr + "," + tDebitAddr + "-" + tCreditAddr + "," + "0" + ")";
                        tProfitCreditAddr = worksheet.Cell(totalRowValue, 6).CellAddress;

                        cell = worksheet.Cell(totalRowValue, 8);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + iCreditAddr + ">" + iDebitAddr + "," + iCreditAddr + "-" + iDebitAddr + "," + "0" + ")";
                        iProfitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 9);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + iCreditAddr + "<" + iDebitAddr + "," + iDebitAddr + "-" + iCreditAddr + "," + "0" + ")";
                        iProfitCreditAddr = worksheet.Cell(totalRowValue, 9).CellAddress;

                        cell = worksheet.Cell(totalRowValue, 11);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + bCreditAddr + ">" + bDebitAddr + "," + bCreditAddr + "-" + bDebitAddr + "," + "0" + ")";
                        bProfitAddr = cell.CellAddress;

                        cell = worksheet.Cell(totalRowValue, 12);
                        cell.RemoveValue();
                        cell.Formula = "if" + "(" + bCreditAddr + "<" + bDebitAddr + "," + bDebitAddr + "-" + bCreditAddr + "," + "0" + ")";
                        bProfitCreditAddr = worksheet.Cell(totalRowValue, 12).CellAddress;

                        //Formula for GrandTotal
                        totalRowValue = totalRowValue + 2;

                        cell = worksheet.Cell(totalRowValue, 5);
                        cell.RemoveValue();
                        cell.Formula = tDebitAddr + "+" + tProfitAddr;

                        cell = worksheet.Cell(totalRowValue, 6);
                        cell.RemoveValue();
                        cell.Formula = tCreditAddr + "+" + tProfitCreditAddr;

                        cell = worksheet.Cell(totalRowValue, 8);
                        cell.RemoveValue();
                        cell.Formula = iDebitAddr + "+" + iProfitAddr;

                        cell = worksheet.Cell(totalRowValue, 9);
                        cell.RemoveValue();
                        cell.Formula = iCreditAddr + "+" + iProfitCreditAddr;

                        cell = worksheet.Cell(totalRowValue, 11);
                        cell.RemoveValue();
                        cell.Formula = bDebitAddr + "+" + bProfitAddr;

                        cell = worksheet.Cell(totalRowValue, 12);
                        cell.RemoveValue();
                        cell.Formula = iCreditAddr + "+" + bProfitCreditAddr;

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        /// <summary>
        /// Checks whether the cell belonging the specified column exists and has a string(not empty) in the table of the DataRow.
        /// </summary>
        /// <param name="col">Name of the column to check.</param>
        /// <param name="dRow">DataRow container.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool DoesCellExist(string col, DataRow dRow)
        {
            if (dRow.Table.Columns.Contains(col) && Convert.ToString(dRow[col]).Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ledger Report generation using Excel Package.
        /// </summary>
        /// <param name="dtParent"></param>
        /// <param name="dtChild"></param>
        /// <param name="dtHeader"></param>
        /// <param name="htColNameValues"></param>
        /// <param name="htBColNameValues"></param>
        public void ReportStyle621Old(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColNameValues, Hashtable htBColNameValues)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            StringBuilder balForwardAddr = new StringBuilder();
            StringBuilder dateRangeAddr = new StringBuilder();
            string endBalanceAddr = string.Empty;

            string strAmtStartAddr = string.Empty;
            string strAmtEndAddr = string.Empty;

            string pBalForwardAddr = string.Empty;
            string childDtRangeAddr = string.Empty;

            int parentStyleID;
            int dtStyleID;
            int amtStyleID;
            int dtRangeStyleID;
            int linkStyleID;
            int colHeaderStyleID;
            int boldAmtStyleID;

            int acctHeaderStyleID;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            ExcelCell cell;

            const int startRow = 7;
            int row = startRow;
            int col = 0;
            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\ledger detail" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-621.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-621.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["ledger detail"];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;

                        cell = worksheet.Cell(3, 1);
                        linkStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        cell = worksheet.Cell(3, 2);
                        boldAmtStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        cell = worksheet.Cell(5, 1);
                        colHeaderStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 1);
                        parentStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 6);
                        dtStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 14);
                        amtStyleID = cell.StyleID;

                        cell = worksheet.Cell(10, 11);
                        dtRangeStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        //Modify ColNames in the template with Captions
                        cell = worksheet.Cell(5, 1);
                        if (htBColNameValues["PayeePayor"] != null)
                            cell.Value = htBColNameValues["PayeePayor"].ToString();

                        cell = worksheet.Cell(5, 3);
                        if (htBColNameValues["Description"] != null)
                            cell.Value = htBColNameValues["Description"].ToString();

                        cell = worksheet.Cell(5, 6);
                        if (htBColNameValues["ItemMemo"] != null)
                            cell.Value = htBColNameValues["ItemMemo"].ToString();

                        cell = worksheet.Cell(5, 7);
                        if (htBColNameValues["LedgerDate"] != null)
                            cell.Value = htBColNameValues["LedgerDate"].ToString();

                        cell = worksheet.Cell(5, 8);
                        if (htBColNameValues["DocSrc"] != null)
                            cell.Value = htBColNameValues["DocSrc"].ToString();

                        int currCol = 8;
                        if (htBColNameValues["VoucherRef"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["VoucherRef"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header1"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header1"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header2"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header2"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header3"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header3"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header4"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header4"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        cell = worksheet.Cell(5, 14);
                        if (htBColNameValues["Amount"] != null)
                            cell.Value = htBColNameValues["Amount"].ToString();


                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                            {
                                System.Diagnostics.Stopwatch swLoop = new System.Diagnostics.Stopwatch();
                                swLoop.Start();

                                if (dtParent.Rows[parentCount]["TrxID"] == null || dtParent.Rows[parentCount]["TrxID"].ToString() == "")
                                {
                                    break;
                                }

                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        //worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        //worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];

                                if (dParentRow["Account"] != null && dParentRow["Account"].ToString() != "SKIP")
                                {
                                    if (dParentRow["Account"].ToString().Trim() != "")
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = parentStyleID;

                                        cell = worksheet.Cell(row, col + 2);
                                        cell.StyleID = parentStyleID;

                                        cell = worksheet.Cell(row, col + 3);
                                        cell.StyleID = parentStyleID;
                                    }

                                    cell = worksheet.Cell(row, col + 12);
                                    cell.Value = "Balance Forward:";
                                    cell.StyleID = acctHeaderStyleID;

                                    pBalForwardAddr = "";

                                    if (dParentRow["OpenBal"] != null && dParentRow["OpenBal"].ToString().Trim() != "")
                                    {
                                        cell = worksheet.Cell(row, col + 14);
                                        cell.Value = dParentRow["OpenBal"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;

                                        pBalForwardAddr = cell.CellAddress;

                                        if (balForwardAddr.Length != 0)
                                        {
                                            balForwardAddr.Append("+" + cell.CellAddress);
                                        }
                                        else
                                        {
                                            balForwardAddr.Append(cell.CellAddress);
                                        }
                                    }

                                    if (dtChild != null)
                                    {
                                        if (dParentRow["Link1"] != null && dParentRow["Link1"].ToString().Trim() != "")
                                        {
                                            int link = Convert.ToInt32(dParentRow["Link1"].ToString());

                                            DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link + "'");

                                            #region Commented Code
                                            //DataTable dtLinkedChildRows = new DataTable();

                                            //for (int colCount = 0; colCount < dtChild.Columns.Count; colCount++)
                                            //{
                                            //    dtLinkedChildRows.Columns.Add(dtChild.Columns[colCount].ColumnName);
                                            //}

                                            //for (int rowCount = 0; rowCount < drLinkedChildRows.Length; rowCount++)
                                            //{
                                            //    DataRow drLinkedChildRow = dtLinkedChildRows.NewRow();

                                            //    for (int cCount = 0; cCount < dtLinkedChildRows.Columns.Count; cCount++)
                                            //    {
                                            //        drLinkedChildRow[cCount] = drLinkedChildRows[rowCount].ItemArray[cCount].ToString();

                                            //        if (cCount == dtLinkedChildRows.Columns.Count - 1)
                                            //            dtLinkedChildRows.Rows.Add(drLinkedChildRow);
                                            //    }
                                            //}
                                            #endregion

                                            row++;

                                            strAmtStartAddr = worksheet.Cell(row, col + 13).CellAddress;

                                            bool noChild = false;

                                            if (drLinkedChildRows.Length == 0)
                                                noChild = true;

                                            DataRow dRow = null;
                                            for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                            {
                                                dRow = drLinkedChildRows[childCount];

                                                if (row > startRow)
                                                {
                                                    //worksheet.InsertRow(row);
                                                }

                                                if (DoesCellExist("PayeePayor", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 1);

                                                    if (Convert.ToString(dRow["PayeePayor"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["PayeePayor"].ToString()))
                                                    {
                                                        dRow["PayeePayor"] = System.Security.SecurityElement.Escape(dRow["PayeePayor"].ToString());
                                                        cell.Value = dRow["PayeePayor"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("Description", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 3);

                                                    if (Convert.ToString(dRow["Description"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                                    {
                                                        dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                        cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("ItemMemo", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 6);

                                                    if (Convert.ToString(dRow["ItemMemo"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["ItemMemo"].ToString()))
                                                    {
                                                        dRow["ItemMemo"] = System.Security.SecurityElement.Escape(dRow["ItemMemo"].ToString());
                                                        cell.Value = dRow["ItemMemo"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("LedgerDate", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 7);
                                                    cell.Value = dRow["LedgerDate"].ToString();
                                                    cell.StyleID = dtStyleID;
                                                }

                                                if (DoesCellExist("DocSrc", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 8);
                                                    cell.Value = dRow["DocSrc"].ToString();
                                                }

                                                currCol = 8;
                                                if (DoesCellExist("VoucherRef", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    cell.Value = dRow["VoucherRef"].ToString();
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header1", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);

                                                    if (Convert.ToString(dRow["Header1"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["Header1"].ToString()))
                                                    {
                                                        dRow["Header1"] = System.Security.SecurityElement.Escape(dRow["Header1"].ToString());
                                                        cell.Value = dRow["Header1"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header2", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);

                                                    if (Convert.ToString(dRow["Header2"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["Header2"].ToString()))
                                                    {
                                                        dRow["Header2"] = System.Security.SecurityElement.Escape(dRow["Header2"].ToString());
                                                        cell.Value = dRow["Header2"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header3", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);

                                                    if (Convert.ToString(dRow["Header3"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["Header3"].ToString()))
                                                    {
                                                        dRow["Header3"] = System.Security.SecurityElement.Escape(dRow["Header3"].ToString());
                                                        cell.Value = dRow["Header3"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header4", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);

                                                    if (Convert.ToString(dRow["Header4"]).Length > 0)
                                                    //if (!string.IsNullOrEmpty(dRow["Header4"].ToString()))
                                                    {
                                                        dRow["Header4"] = System.Security.SecurityElement.Escape(dRow["Header4"].ToString());
                                                        cell.Value = dRow["Header3"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Amount", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 14);
                                                    cell.Value = dRow["Amount"].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                row++;
                                            }

                                            cell = worksheet.Cell(row - 1, col + 14);
                                            strAmtEndAddr = cell.CellAddress;
                                            childDtRangeAddr = "";

                                            //if (!noChild)
                                            //{
                                            if (!noChild)
                                            {
                                                //worksheet.InsertRow(row);

                                                cell = worksheet.Cell(row - 1, col + 14);
                                                cell.StyleID = linkStyleID;

                                                row++;
                                                //worksheet.InsertRow(row);
                                                cell = worksheet.Cell(row, col + 12);
                                                cell.Value = "Date Range Total:";
                                                cell.StyleID = dtRangeStyleID;

                                                cell = worksheet.Cell(row, col + 13);
                                                cell.StyleID = dtRangeStyleID;

                                                //if (!noChild)
                                                //{
                                                cell = worksheet.Cell(row, col + 14);
                                                cell.RemoveValue();
                                                cell.Formula = string.Format("SUM({0}:{1})", strAmtStartAddr, strAmtEndAddr);
                                                cell.StyleID = boldAmtStyleID;

                                                childDtRangeAddr = cell.CellAddress;

                                                if (dateRangeAddr.Length != 0)
                                                {
                                                    dateRangeAddr.Append("+" + cell.CellAddress);
                                                }
                                                else
                                                {
                                                    dateRangeAddr.Append(cell.CellAddress);
                                                }


                                                row++;
                                                //worksheet.InsertRow(row);

                                                cell = worksheet.Cell(row, col + 12);
                                                cell.Value = "Ending Balance:";
                                                cell.StyleID = acctHeaderStyleID;

                                                cell = worksheet.Cell(row, col + 14);
                                                cell.RemoveValue();
                                                if (pBalForwardAddr == "")
                                                {
                                                    cell.Formula = childDtRangeAddr;
                                                    cell.StyleID = boldAmtStyleID;
                                                }
                                                else
                                                {
                                                    if (childDtRangeAddr == "")
                                                    {
                                                        cell.Formula = childDtRangeAddr;
                                                        cell.StyleID = boldAmtStyleID;
                                                    }

                                                    else
                                                    {
                                                        cell.Formula = pBalForwardAddr + "+" + childDtRangeAddr;
                                                        cell.StyleID = boldAmtStyleID;
                                                    }
                                                }

                                                //if (endBalanceAddr != string.Empty)
                                                //{
                                                //    endBalanceAddr = endBalanceAddr + "+" + cell.CellAddress;
                                                //}
                                                //else
                                                //{
                                                //    endBalanceAddr = cell.CellAddress;
                                                //}
                                                //}

                                                row++;

                                                //worksheet.InsertRow(row);
                                                rowInserted = true;
                                            }
                                            else
                                            {
                                                //worksheet.InsertRow(row);
                                                rowInserted = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        row++;
                                        //worksheet.InsertRow(row);
                                        rowInserted = true;

                                    }
                                }

                                System.Diagnostics.Debugger.Log(1, "Row", "\nPrinting row : " + parentCount + " in " + swLoop.ElapsedMilliseconds + "ms");
                            }

                            int totalBalrow = row + 2;
                            cell = worksheet.Cell(totalBalrow, col + 11);
                            cell.Value = "Total Balance Forward: ";
                            cell.StyleID = acctHeaderStyleID;

                            cell = worksheet.Cell(totalBalrow, col + 14);
                            cell.RemoveValue();
                            cell.Formula = balForwardAddr.ToString();
                            string totalBalAddr = cell.CellAddress;
                            cell.StyleID = boldAmtStyleID;

                            int totalDtRangerow = totalBalrow + 1;
                            cell = worksheet.Cell(totalDtRangerow, col + 11);
                            cell.Value = "Total Date Range: ";
                            cell.StyleID = worksheet.Cell(totalDtRangerow, col + 12).StyleID
                                = worksheet.Cell(totalDtRangerow, col + 13).StyleID = dtRangeStyleID;
                            cell = worksheet.Cell(totalDtRangerow, col + 14);
                            cell.RemoveValue();
                            cell.Formula = dateRangeAddr.ToString();
                            string totalDtRangeAddr = cell.CellAddress;
                            cell.StyleID = boldAmtStyleID;

                            int totalEBalancerow = totalDtRangerow + 1;
                            cell = worksheet.Cell(totalEBalancerow, col + 11);
                            cell.Value = "Total Ending Balance: ";
                            cell.StyleID = acctHeaderStyleID;
                            cell = worksheet.Cell(totalEBalancerow, col + 14);
                            cell.RemoveValue();
                            //cell.Formula = endBalanceAddr;
                            cell.Formula = totalBalAddr + "+" + totalDtRangeAddr;
                            cell.StyleID = boldAmtStyleID;
                        }
                        xlPackage.Save();
                    }
                }
                System.Diagnostics.Debugger.Log(1, "cat", "\nGeneration took" + sw.ElapsedMilliseconds + "ms");
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        /// <summary>
        /// Ledger Report generation using Microsoft Office Interop.
        /// </summary>
        /// <param name="dtParent"></param>
        /// <param name="dtChild"></param>
        /// <param name="dtHeader"></param>
        /// <param name="htColNameValues"></param>
        /// <param name="htBColNameValues"></param>
        public void ReportStyle621(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColNameValues, Hashtable htBColNameValues)
        {
            string strCurrentDir = ConfigurationManager.AppSettings["TempFilePath"] + "\\";
            string fileName = "LedgerDetail" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xlsx";
            string filePath = strCurrentDir + fileName;

            //COM Objects.
            Excel.Application xlApp = null;
            Excel._Workbook xlWB = null;
            Excel._Worksheet xlSheet = null;
            Excel.Workbooks xlWBooks = null;
            Excel.Range xlCells = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            try
            {
                #region Excel Initialisation

                GC.Collect();// clean up any other excel guys hangin' around...
                xlApp = new Excel.Application();
                if (xlApp == null)
                {
                    throw new Exception("EXCEL could not be started. Check that your office installation and project references are correct.");
                }

                xlApp.Visible = true;
                xlApp.SheetsInNewWorkbook = 1;
                xlApp.AskToUpdateLinks = false;
                xlApp.AlertBeforeOverwriting = false;

                System.Diagnostics.Debugger.Log(1, "Test", "Excel now running under the user : " + xlApp.UserName);

                //Get a new workbook.
                xlWBooks = xlApp.Workbooks;
                xlWB = xlWBooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                xlSheet = (Excel._Worksheet)xlWB.ActiveSheet;
                xlCells = xlSheet.Cells;

                #endregion

                ErrorLogger.LogError("Excel initialised successfully under the user :" + xlApp.UserName);

                #region Local Variables
                string excelDocPath = string.Empty;
                string excelDir = string.Empty;
                string strHeader = string.Empty;
                string compName = string.Empty;
                string compDetails = string.Empty;
                string compDate = string.Empty;
                string endBalanceAddr = string.Empty;
                string strAmtStartAddr = string.Empty;
                string strAmtEndAddr = string.Empty;
                string pBalForwardAddr = string.Empty;
                string childDtRangeAddr = string.Empty;
                string accountingFormat = @"_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)";
                string dateFormat = "mm/dd/yy;@";
                StringBuilder balForwardAddr = new StringBuilder();
                StringBuilder dateRangeAddr = new StringBuilder();
                DataRow drCompDetails;
                DataRow drCompDate;
                DataRow drHeader;
                const int startRow = 7;
                int row = startRow;
                int col = 0;
                int arrayCount = 0;
                bool rowInserted = false;
                #endregion

                #region Initial Formatting

                xlSheet.Name = "General Ledger";
                //Requested By
                SetCell(1, 1, "Requested by:", true, "", "", 0, xlCells);

                //Payee
                SetCell(5, 1, "Payee/Payor", true, "", "", 0, xlCells);
                //Description
                SetCell(5, 3, "Description", true, "", "", 0, xlCells);
                //Item Memo
                //SetCell(5, 6, "Item Memo", true, "", "", 0, xlCells);
                //Date
                SetCell(5, 7, "Date", true, "", "", 0, xlCells);
                //Srce
                SetCell(5, 8, "Srce", true, "", "", 0, xlCells);
                // Amount
                SetCell(5, 14, "Amount", true, "", accountingFormat, 0, xlCells);
                //Run:
                SetCell(2, 10, "Run:", true, "", "", 0, xlCells);

                drHeader = dtHeader.Rows[0];
                strHeader = drHeader["Column1"].ToString();
                string[] strRequestedBy = strHeader.Split(':');

                if (strRequestedBy.Length > 1)
                {
                    SetCell(2, 2, strRequestedBy[arrayCount + 1].Trim().ToString(), true, "", "", 0, xlCells);
                }
                else
                {
                    SetCell(2, 2, strRequestedBy[arrayCount].Trim().ToString(), true, "", "", 0, xlCells);
                }

                compName = drHeader["Column2"].ToString();
                SetCell(1, 5, compName, true, "", "", 0, xlCells);

                if (dtHeader.Rows.Count > 1)
                {
                    drCompDetails = dtHeader.Rows[1];
                    compDetails = drCompDetails["Column2"].ToString();
                    SetCell(2, 5, compDetails, true, "", "", 0, xlCells);
                }

                if (dtHeader.Rows.Count > 2)
                {
                    drCompDate = dtHeader.Rows[2];
                    compDate = drCompDate["Column2"].ToString();
                    SetCell(3, 5, compDate, true, "", "", 0, xlCells);
                }

                string strReportDate = dtHeader.Rows[0]["Column3"].ToString();
                if (strReportDate != null && strReportDate != "")
                {
                    string[] dateArray = strReportDate.Split(' ');
                    string col3Date = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                    strReportDate = col3Date + " " + dateArray[3].ToString();
                }
                SetCell(3, 11, strReportDate, true, "[$-409]m/d/yy h:mm AM/PM;@", "", 0, xlCells);

                //Modify ColNames in the template with Captions
                if (htBColNameValues["PayeePayor"] != null)
                {
                    SetCell(5, 1, htBColNameValues["PayeePayor"].ToString(), true, "", "", 14, xlCells);
                }

                this.ApplyBorder(5, 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);

                if (htBColNameValues["Description"] != null)
                {
                    this.ApplyBorder(5, 3, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, 3, htBColNameValues["Description"].ToString(), true, "", "", 44, xlCells);
                }

                if (htBColNameValues["ItemMemo"] != null)
                {
                    this.ApplyBorder(5, 6, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, 6, htBColNameValues["ItemMemo"].ToString(), true, "", "", 0, xlCells);
                }

                if (htBColNameValues["LedgerDate"] != null)
                {
                    this.ApplyBorder(5, 7, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, 7, htBColNameValues["LedgerDate"].ToString(), true, "", dateFormat, 12, xlCells);
                }

                if (htBColNameValues["DocSrc"] != null)
                {
                    this.ApplyBorder(5, 8, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, 8, htBColNameValues["DocSrc"].ToString(), true, "", "", 0, xlCells);
                }

                int currCol = 8;
                if (htBColNameValues["VoucherRef"] != null)
                {
                    this.ApplyBorder(5, currCol + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, currCol + 1, htBColNameValues["VoucherRef"].ToString(), true, "", "", 0, xlCells);
                    currCol++;
                }

                if (htBColNameValues["Header1"] != null)
                {
                    this.ApplyBorder(5, currCol + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, currCol + 1, htBColNameValues["Header1"].ToString(), true, "", "", 0, xlCells);
                    currCol++;
                }

                if (htBColNameValues["Header2"] != null)
                {
                    this.ApplyBorder(5, currCol + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, currCol + 1, htBColNameValues["Header2"].ToString(), true, "", "", 0, xlCells);
                    currCol++;
                }

                if (htBColNameValues["Header3"] != null)
                {
                    this.ApplyBorder(5, currCol + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, currCol + 1, htBColNameValues["Header3"].ToString(), true, "", "", 0, xlCells);
                    currCol++;
                }

                if (htBColNameValues["Header4"] != null)
                {
                    this.ApplyBorder(5, currCol + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, currCol + 1, htBColNameValues["Header4"].ToString(), true, "", "", 0, xlCells);
                    currCol++;
                }

                if (htBColNameValues["Amount"] != null)
                {
                    this.ApplyBorder(5, 14, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                    SetCell(5, 14, htBColNameValues["Amount"].ToString(), true, "", "", 15, xlCells);
                }
                #endregion

                #region Work Code

                if (dtParent != null)
                {
                    for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                    {
                        System.Diagnostics.Stopwatch swLoop = new System.Diagnostics.Stopwatch();
                        swLoop.Start();
                        if (dtParent.Rows[parentCount]["TrxID"] == null || dtParent.Rows[parentCount]["TrxID"].ToString() == "")
                        {
                            break;
                        }

                        if (row > startRow)
                        {
                            if (rowInserted)
                            {
                                row = row + 1;
                            }
                        }
                        DataRow dParentRow = dtParent.Rows[parentCount];
                        if (dParentRow["Account"] != null && dParentRow["Account"].ToString() != "SKIP")
                        {
                            if (dParentRow["Account"].ToString().Trim() != "")
                            {
                                SetCell(row, col + 1, System.Security.SecurityElement.Escape(dParentRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", ""), true, "", "", 0, xlCells);
                                this.ApplyBorder(row, col + 1, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                                //Apply bottom border to the two cells following Description
                                this.ApplyBorder(row, col + 2, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                                this.ApplyBorder(row, col + 3, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                            }

                            SetCell(row, col + 12, "Balance Forward:", true, "", "", 0, xlCells);
                            pBalForwardAddr = "";

                            if (dParentRow["OpenBal"] != null && dParentRow["OpenBal"].ToString().Trim() != "")
                            {
                                SetCell(row, col + 14, dParentRow["OpenBal"].ToString().Replace(",", ""), false, "", "", 0, xlCells);
                                pBalForwardAddr = GetCellAddress(row, col + 14, xlCells);
                                if (balForwardAddr.Length != 0)
                                {
                                    balForwardAddr.Append("+" + pBalForwardAddr);
                                }
                                else
                                {
                                    balForwardAddr.Append(pBalForwardAddr);
                                }
                            }

                            if (dtChild != null)
                            {
                                if (dParentRow["Link1"] != null && dParentRow["Link1"].ToString().Trim() != "")
                                {
                                    int link = Convert.ToInt32(dParentRow["Link1"].ToString());
                                    DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link + "'");
                                    row++;
                                    strAmtStartAddr = GetCellAddress(row, col + 13, xlCells);
                                    bool noChild = false;
                                    if (drLinkedChildRows.Length == 0)
                                        noChild = true;

                                    DataRow dRow = null;
                                    for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                    {
                                        dRow = drLinkedChildRows[childCount];
                                        if (DoesCellExist("PayeePayor", dRow))
                                        {
                                            if (Convert.ToString(dRow["PayeePayor"]).Length > 0)
                                            {
                                                dRow["PayeePayor"] = System.Security.SecurityElement.Escape(dRow["PayeePayor"].ToString());
                                                SetCell(row, col + 1, dRow["PayeePayor"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                        }

                                        if (DoesCellExist("Description", dRow))
                                        {
                                            if (Convert.ToString(dRow["Description"]).Length > 0)
                                            {
                                                dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                SetCell(row, col + 3, dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                        }

                                        if (DoesCellExist("ItemMemo", dRow))
                                        {
                                            if (Convert.ToString(dRow["ItemMemo"]).Length > 0)
                                            {
                                                dRow["ItemMemo"] = System.Security.SecurityElement.Escape(dRow["ItemMemo"].ToString());
                                                SetCell(row, col + 6, dRow["ItemMemo"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                        }

                                        if (DoesCellExist("LedgerDate", dRow))
                                        {
                                            SetCell(row, col + 7, dRow["LedgerDate"].ToString(), false, "", "", 0, xlCells);
                                        }

                                        if (DoesCellExist("DocSrc", dRow))
                                        {
                                            SetCell(row, col + 8, dRow["DocSrc"].ToString(), false, "", "", 0, xlCells);
                                        }

                                        currCol = 8;
                                        if (DoesCellExist("VoucherRef", dRow))
                                        {
                                            SetCell(row, currCol + 1, dRow["VoucherRef"].ToString(), false, "", "", 0, xlCells);
                                            currCol++;
                                        }

                                        if (DoesCellExist("Header1", dRow))
                                        {
                                            if (Convert.ToString(dRow["Header1"]).Length > 0)
                                            {
                                                dRow["Header1"] = System.Security.SecurityElement.Escape(dRow["Header1"].ToString());
                                                SetCell(row, currCol + 1, dRow["Header1"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                            currCol++;
                                        }

                                        if (DoesCellExist("Header2", dRow))
                                        {
                                            if (Convert.ToString(dRow["Header2"]).Length > 0)
                                            {
                                                dRow["Header2"] = System.Security.SecurityElement.Escape(dRow["Header2"].ToString());
                                                SetCell(row, currCol + 1, dRow["Header2"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                            currCol++;
                                        }

                                        if (DoesCellExist("Header3", dRow))
                                        {
                                            if (Convert.ToString(dRow["Header3"]).Length > 0)
                                            {
                                                dRow["Header3"] = System.Security.SecurityElement.Escape(dRow["Header3"].ToString());
                                                SetCell(row, currCol + 1, dRow["Header3"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                            currCol++;
                                        }

                                        if (DoesCellExist("Header4", dRow))
                                        {
                                            if (Convert.ToString(dRow["Header4"]).Length > 0)
                                            {
                                                dRow["Header4"] = System.Security.SecurityElement.Escape(dRow["Header4"].ToString());
                                                SetCell(row, currCol + 1, dRow["Header4"].ToString().Replace("amp;", "").Replace("&apos;", ""), false, "", "", 0, xlCells);
                                            }
                                            currCol++;
                                        }

                                        if (DoesCellExist("Amount", dRow))
                                        {
                                            SetCell(row, col + 14, dRow["Amount"].ToString().Replace(",", ""), false, "", "", 0, xlCells);
                                        }
                                        row++;
                                    }

                                    strAmtEndAddr = GetCellAddress(row - 1, col + 14, xlCells);
                                    childDtRangeAddr = "";

                                    if (!noChild)
                                    {
                                        ApplyBorder(row - 1, col + 14, Excel.XlBordersIndex.xlEdgeBottom, xlCells);
                                        row++;
                                        SetCell(row, col + 12, "Date Range Total:", true, "", "", 0, 15, xlCells);
                                        //Do the styling for the next cell also.
                                        SetCell(row, col + 13, "", false, "", "", 0, 15, xlCells);
                                        SetCellFormula(row, col + 14, string.Format("=SUM({0}:{1})", strAmtStartAddr, strAmtEndAddr),
                                                true, xlCells);
                                        childDtRangeAddr = GetCellAddress(row, col + 14, xlCells);
                                        if (dateRangeAddr.Length != 0)
                                        {
                                            dateRangeAddr.Append("+" + childDtRangeAddr);
                                        }
                                        else
                                        {
                                            dateRangeAddr.Append(childDtRangeAddr);
                                        }
                                        row++;
                                        SetCell(row, col + 12, "Ending Balance:", true, "", "", 0, xlCells);
                                        SetCell(row, col + 14, "Ending Balance:", true, accountingFormat, "", 0, xlCells);
                                        if (pBalForwardAddr == "")
                                        {
                                            SetCellFormula(row, col + 14, "=" + childDtRangeAddr, true, xlCells);
                                        }
                                        else
                                        {
                                            if (childDtRangeAddr == "")
                                            {
                                                SetCellFormula(row, col + 14, "=" + childDtRangeAddr, true, xlCells);
                                            }
                                            else
                                            {
                                                SetCellFormula(row, col + 14, "=" + pBalForwardAddr + "+" + childDtRangeAddr, true, xlCells);
                                            }
                                        }
                                        row++;
                                        rowInserted = true;
                                    }
                                    else
                                    {
                                        rowInserted = true;
                                    }
                                }
                            }
                            else
                            {
                                row++;
                                rowInserted = true;

                            }
                        }
                        System.Diagnostics.Debugger.Log(1, "Row", "\nPrinting row : " + parentCount + " in " + swLoop.ElapsedMilliseconds + "ms");
                    }

                    int totalBalrow = row + 2;
                    SetCell(totalBalrow, col + 11, "Total Balance Forward: ", true, "", "", 0, xlCells);
                    SetCellFormula(totalBalrow, col + 14, "=" + balForwardAddr.ToString(), true, xlCells);
                    string totalBalAddr = GetCellAddress(totalBalrow, col + 14, xlCells);
                    int totalDtRangerow = totalBalrow + 1;
                    SetCell(totalDtRangerow, col + 11, "Total Date Range: ", true, "", "", 0, xlCells);
                    SetCellFormula(totalDtRangerow, col + 14, "=" + dateRangeAddr.ToString(), true, xlCells);
                    string totalDtRangeAddr = GetCellAddress(totalDtRangerow, col + 14, xlCells);
                    int totalEBalancerow = totalDtRangerow + 1;
                    SetCell(totalEBalancerow, col + 11, "Total Ending Balance: ", true, "", "", 0, xlCells);
                    SetCellFormula(totalEBalancerow, col + 14, "=" + totalBalAddr + "+" + totalDtRangeAddr, true, xlCells);
                }
                #endregion

                ErrorLogger.LogError("Preparing to save the file : " + filePath);

                #region Save and Emit to Response

                xlApp.Visible = false;
                xlApp.UserControl = false;
                //Save the Excel file to a phsyical path.
                //xlWB.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal,
                //     Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, Type.Missing, Type.Missing, Type.Missing);

                xlWB.SaveCopyAs(filePath);

                //Write it to response.
                FileInfo fiExcel = new FileInfo(filePath);
                SaveToClient(fiExcel);

                #endregion

                ErrorLogger.LogError("Save complete");

                System.Diagnostics.Debugger.Log(1, "cat", "\nGeneration took " + sw.ElapsedMilliseconds + "ms");
            }
            catch (Exception theException)
            {
                #region NLog
                logger.Fatal(theException);
                #endregion


                #region Commented for the sake of NLog
                //String errorMessage;
                //errorMessage = "Error in 621: ";
                //errorMessage = String.Concat(errorMessage, theException.Message);
                //errorMessage = String.Concat(errorMessage, " Line: ");
                //errorMessage = String.Concat(errorMessage, theException.Source);
                //ErrorLogger.LogError(errorMessage);
                //throw new Exception(errorMessage); 
                #endregion
            }
            finally
            {
                // Need all following code to clean up and extingush all references!!!
                if (xlWB != null) xlWB.Close(false, filePath, false);
                if (xlWBooks != null) xlWBooks.Close();
                if (xlApp != null) xlApp.Quit();

                if (xlCells != null) while (Marshal.ReleaseComObject(xlCells) > 0) { }
                if (xlSheet != null) while (Marshal.ReleaseComObject(xlSheet) > 0) { }
                if (xlWB != null) while (Marshal.ReleaseComObject(xlWB) > 0) { }
                if (xlWBooks != null) while (Marshal.ReleaseComObject(xlWBooks) > 0) { }
                if (xlApp != null) while (Marshal.ReleaseComObject(xlApp) > 0) { }

                xlCells = null;
                xlSheet = null;
                xlWB = null;
                xlWBooks = null;
                xlApp = null;
                GC.Collect(); // Force final cleanup! 
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void SetCellFormula(int row, int col, string formula, bool boldFont, Excel.Range xlCells)
        {
            Excel.Range cell = xlCells[row, col];
            cell.Clear();
            cell.Formula = formula;
            cell.NumberFormat = @"_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)";

            //Set the font weight.
            if (boldFont)
            {
                Excel.Font cellFont = cell.Font;
                cellFont.Bold = true;
                while (Marshal.ReleaseComObject(cellFont) > 0) { }
                cellFont = null;
            }
            while (Marshal.ReleaseComObject(cell) > 0) { }
            cell = null;
        }

        private string GetCellAddress(int row, int col, Excel.Range xlCells)
        {
            Excel.Range xlCell = xlCells[row, col];
            string addr = xlCell.get_Address(Type.Missing, Type.Missing
                                    , Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing).Replace("$", "");
            while (Marshal.ReleaseComObject(xlCell) > 0) { }
            xlCell = null;
            return addr;
            //return "A1";
        }

        private void ApplyBorder(int row, int col, Excel.XlBordersIndex xlBordersIndex, Excel.Range xlCells)
        {
            Excel.Range xlCell = xlCells[row, col];
            Excel.Borders xlBorders = xlCell.Borders;
            Excel.Border xlBorderSide = xlBorders[xlBordersIndex];
            xlBorderSide.LineStyle = Excel.XlLineStyle.xlContinuous;
            xlBorderSide.ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            while (Marshal.ReleaseComObject(xlBorderSide) > 0) { }
            xlBorderSide = null;
            while (Marshal.ReleaseComObject(xlBorders) > 0) { }
            xlBorders = null;
            while (Marshal.ReleaseComObject(xlCell) > 0) { }
            xlCell = null;
        }

        private void SetCell(int row, int col, string text, bool boldFont, string cellFmt, string colFmt, int colWidth, Excel.Range xlCells)
        {
            Excel.Range cell = xlCells[row, col];
            cell.Value2 = text;

            //Set the font weight.
            if (boldFont)
            {
                Excel.Font cellFont = cell.Font;
                cellFont.Bold = true;
                while (Marshal.ReleaseComObject(cellFont) > 0) { }
                cellFont = null;
            }

            //Set the format for an entire column.
            if (colFmt.Length > 0)
            {
                Excel.Range xlCol = cell.EntireColumn;
                xlCol.NumberFormat = colFmt;
                while (Marshal.ReleaseComObject(xlCol) > 0) { }
                xlCol = null;
            }

            //Set the format for a single cell.
            if (cellFmt.Length > 0)
            {
                cell.NumberFormat = cellFmt;
            }

            //Set the width of the column
            if (colWidth > 0)
            {
                Excel.Range xlCol = cell.EntireColumn;
                xlCol.ColumnWidth = colWidth;
                while (Marshal.ReleaseComObject(xlCol) > 0) { }
                xlCol = null;
            }

            while (Marshal.ReleaseComObject(cell) > 0) { }
            cell = null;
        }

        private void SetCell(int row, int col, string text, bool boldFont, string cellFmt, string colFmt, int colWidth, int colorIndex, Excel.Range xlCells)
        {
            Excel.Range cell = xlCells[row, col];
            cell.Value2 = text;

            //Set the font weight.
            if (boldFont)
            {
                Excel.Font cellFont = cell.Font;
                cellFont.Bold = true;
                while (Marshal.ReleaseComObject(cellFont) > 0) { }
                cellFont = null;
            }

            //Set the format for an entire column.
            if (colFmt.Length > 0)
            {
                Excel.Range xlCol = cell.EntireColumn;
                xlCol.NumberFormat = colFmt;
                while (Marshal.ReleaseComObject(xlCol) > 0) { }
                xlCol = null;
            }

            //Set the format for a single cell.
            if (cellFmt.Length > 0)
            {
                cell.NumberFormat = cellFmt;
            }

            //Set the width of the column
            if (colWidth > 0)
            {
                Excel.Range xlCol = cell.EntireColumn;
                xlCol.ColumnWidth = colWidth;
                while (Marshal.ReleaseComObject(xlCol) > 0) { }
                xlCol = null;
            }

            if (colorIndex > 0)
            {
                Excel.Interior xlInterior = cell.Interior;
                xlInterior.ColorIndex = colorIndex;
                xlInterior.Pattern = Excel.XlPattern.xlPatternSolid;
                xlInterior.PatternColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                while (Marshal.ReleaseComObject(xlInterior) > 0) { }
                xlInterior = null;
            }

            while (Marshal.ReleaseComObject(cell) > 0) { }
            cell = null;
        }

        /// <summary>
        /// Obsolete.
        /// </summary>
        /// <param name="side"></param>
        private void ApplyBorder(Excel.Border side)
        {
            side.LineStyle = Excel.XlLineStyle.xlContinuous;
            side.ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
        }

        //Report Style for Ledger Detail Monthly
        public void ReportStyle622(DataTable dtParent, DataTable dtChild, DataTable dtHeader, Hashtable htColNameValues, Hashtable htBColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            StringBuilder balForwardAddr = new StringBuilder();
            StringBuilder dateRangeAddr = new StringBuilder();
            StringBuilder endBalanceAddr = new StringBuilder();

            string strAmtStartAddr = string.Empty;
            string strAmtEndAddr = string.Empty;

            string pBalForwardAddr = string.Empty;
            string childDtRangeAddr = string.Empty;

            //string monthAmtAddr = string.Empty;
            string month = string.Empty;

            int parentStyleID;
            int dtStyleID;
            int amtStyleID;
            int dtRangeStyleID;
            int tBalFwdStyleID;
            int linkStyleID;
            int colHeaderStyleID;
            int boldAmtStyleID;
            int acctHeaderStyleID;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            ExcelCell cell;

            const int startRow = 7;
            int row = startRow;
            int col = 0;
            int arrayCount = 0;

            bool rowInserted = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\ledger detail monthly" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-622.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-622.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["ledger detail monthly"];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;

                        cell = worksheet.Cell(3, 1);
                        linkStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        cell = worksheet.Cell(3, 2);
                        boldAmtStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        cell = worksheet.Cell(5, 1);
                        colHeaderStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 1);
                        parentStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 6);
                        dtStyleID = cell.StyleID;

                        cell = worksheet.Cell(7, 14);
                        amtStyleID = cell.StyleID;

                        cell = worksheet.Cell(9, 10);
                        tBalFwdStyleID = cell.StyleID;

                        cell = worksheet.Cell(10, 11);
                        dtRangeStyleID = cell.StyleID;
                        cell.StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        //Modify ColNames in the template with Captions
                        cell = worksheet.Cell(5, 1);
                        if (htBColNameValues["PayeePayor"] != null)
                            cell.Value = htBColNameValues["PayeePayor"].ToString();

                        cell = worksheet.Cell(5, 3);
                        if (htBColNameValues["Description"] != null)
                            cell.Value = htBColNameValues["Description"].ToString();

                        cell = worksheet.Cell(5, 6);
                        if (htBColNameValues["ItemMemo"] != null)
                            cell.Value = htBColNameValues["ItemMemo"].ToString();

                        cell = worksheet.Cell(5, 7);
                        if (htBColNameValues["LedgerDate"] != null)
                            cell.Value = htBColNameValues["LedgerDate"].ToString();

                        cell = worksheet.Cell(5, 8);
                        if (htBColNameValues["DocSrc"] != null)
                            cell.Value = htBColNameValues["DocSrc"].ToString();

                        int currCol = 8;
                        if (htBColNameValues["VoucherRef"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["VoucherRef"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header1"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header1"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header2"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header2"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header3"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header3"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        if (htBColNameValues["Header4"] != null)
                        {
                            cell = worksheet.Cell(5, currCol + 1);
                            cell.Value = htBColNameValues["Header4"].ToString();
                            cell.StyleID = colHeaderStyleID;
                            currCol++;
                        }

                        cell = worksheet.Cell(5, 14);
                        if (htBColNameValues["Amount"] != null)
                            cell.Value = htBColNameValues["Amount"].ToString();

                        if (dtParent != null)
                        {
                            for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                            {
                                if (dtParent.Rows[parentCount]["TrxID"] == null || dtParent.Rows[parentCount]["TrxID"].ToString() == "")
                                {
                                    break;
                                }

                                if (row > startRow)
                                {
                                    if (rowInserted)
                                    {
                                        row = row + 1;
                                        //worksheet.InsertRow(row);
                                    }
                                    else
                                    {
                                        //worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dParentRow = dtParent.Rows[parentCount];

                                if (dParentRow["Account"] != null && dParentRow["Account"].ToString() != "SKIP")
                                {
                                    if (dParentRow["Account"].ToString().Trim() != "")
                                    {
                                        cell = worksheet.Cell(row, col + 1);
                                        cell.Value = System.Security.SecurityElement.Escape(dParentRow["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = parentStyleID;

                                        cell = worksheet.Cell(row, col + 2);
                                        cell.StyleID = parentStyleID;

                                        cell = worksheet.Cell(row, col + 3);
                                        cell.StyleID = parentStyleID;
                                    }

                                    cell = worksheet.Cell(row, col + 12);
                                    cell.Value = "Balance Forward:";
                                    cell.StyleID = acctHeaderStyleID;

                                    pBalForwardAddr = "";

                                    if (dParentRow["OpenBal"] != null && dParentRow["OpenBal"].ToString().Trim() != "")
                                    {
                                        cell = worksheet.Cell(row, col + 14);
                                        cell.Value = dParentRow["OpenBal"].ToString().Replace(",", "");
                                        cell.StyleID = amtStyleID;

                                        pBalForwardAddr = cell.CellAddress;

                                        if (balForwardAddr.Length != 0)
                                        {
                                            balForwardAddr.Append("+" + cell.CellAddress);
                                        }
                                        else
                                        {
                                            balForwardAddr.Append(cell.CellAddress);
                                        }
                                    }

                                    if (dtChild != null)
                                    {
                                        if (dParentRow["Link1"].ToString() != "" && dParentRow["Link1"].ToString() != null)
                                        {
                                            int link = Convert.ToInt32(dParentRow["Link1"].ToString());

                                            string strSort = "SubTotal1 ASC";

                                            DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link + "'", strSort);

                                            #region Commented Code
                                            //DataTable dtLinkedChildRows = new DataTable();

                                            //for (int colCount = 0; colCount < dtChild.Columns.Count; colCount++)
                                            //{
                                            //    dtLinkedChildRows.Columns.Add(dtChild.Columns[colCount].ColumnName);
                                            //}

                                            //for (int rowCount = 0; rowCount < drLinkedChildRows.Length; rowCount++)
                                            //{
                                            //    DataRow drLinkedChildRow = dtLinkedChildRows.NewRow();

                                            //    for (int cCount = 0; cCount < dtLinkedChildRows.Columns.Count; cCount++)
                                            //    {
                                            //        drLinkedChildRow[cCount] = drLinkedChildRows[rowCount].ItemArray[cCount].ToString();

                                            //        if (cCount == dtLinkedChildRows.Columns.Count - 1)
                                            //            dtLinkedChildRows.Rows.Add(drLinkedChildRow);
                                            //    }
                                            //}
                                            #endregion

                                            row++;

                                            bool noChild = false;

                                            if (drLinkedChildRows.Length == 0)
                                                noChild = true;

                                            //monthAmtAddr = "";
                                            StringBuilder monthAmtAddr = new StringBuilder();
                                            month = "";

                                            for (int childCount = 0; childCount < drLinkedChildRows.Length; childCount++)
                                            {
                                                DataRow dRow = drLinkedChildRows[childCount];

                                                if (row > startRow)
                                                {
                                                    //worksheet.InsertRow(row);
                                                }

                                                if (childCount == 0)
                                                {
                                                    if (dRow["SubTotal1"] != null && dRow["SubTotal1"].ToString().Trim() != "")
                                                    {
                                                        month = dRow["SubTotal1"].ToString();
                                                    }

                                                    strAmtStartAddr = worksheet.Cell(row, col + 14).CellAddress;
                                                }

                                                if (dRow["SubTotal1"] != null && dRow["SubTotal1"].ToString().Trim() != "")
                                                {
                                                    if (month != dRow["SubTotal1"].ToString())
                                                    {
                                                        row++;
                                                        //worksheet.InsertRow(row);

                                                        month = dRow["SubTotal1"].ToString();
                                                        strAmtEndAddr = worksheet.Cell(row - 1, col + 14).CellAddress;

                                                        DataRow dcRow = drLinkedChildRows[childCount - 1];
                                                        cell = worksheet.Cell(row, col + 12);
                                                        cell.Value = dcRow["SubTotal1Description"].ToString();
                                                        cell.StyleID = tBalFwdStyleID;

                                                        cell = worksheet.Cell(row, col + 14);
                                                        cell.Formula = string.Format("SUM({0}:{1})", strAmtStartAddr, strAmtEndAddr);
                                                        cell.StyleID = boldAmtStyleID;
                                                        strAmtStartAddr = "";

                                                        row++;
                                                        //worksheet.InsertRow(row);

                                                        if (monthAmtAddr.Length == 0)
                                                        {
                                                            monthAmtAddr.Append(cell.CellAddress);
                                                        }
                                                        else
                                                        {
                                                            monthAmtAddr.Append("+" + cell.CellAddress);
                                                        }

                                                        row++;
                                                        //worksheet.InsertRow(row);

                                                        strAmtStartAddr = worksheet.Cell(row, col + 14).CellAddress;
                                                    }
                                                }

                                                if (DoesCellExist("PayeePayor", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 1);

                                                    if (!string.IsNullOrEmpty(dRow["PayeePayor"].ToString()))
                                                    {
                                                        dRow["PayeePayor"] = System.Security.SecurityElement.Escape(dRow["PayeePayor"].ToString());
                                                        cell.Value = dRow["PayeePayor"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("Description", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 3);

                                                    if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                                    {
                                                        dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                        cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("ItemMemo", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 6);

                                                    if (!string.IsNullOrEmpty(dRow["ItemMemo"].ToString()))
                                                    {
                                                        dRow["ItemMemo"] = System.Security.SecurityElement.Escape(dRow["ItemMemo"].ToString());
                                                        cell.Value = dRow["ItemMemo"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                }

                                                if (DoesCellExist("LedgerDate", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 7);
                                                    cell.Value = dRow["LedgerDate"].ToString();
                                                    cell.StyleID = dtStyleID;
                                                }

                                                if (DoesCellExist("DocSrc", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 8);
                                                    cell.Value = dRow["DocSrc"].ToString();
                                                }

                                                currCol = 8;
                                                if (DoesCellExist("VoucherRef", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    cell.Value = dRow["VoucherRef"].ToString();
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header1", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    if (!string.IsNullOrEmpty(dRow["Header1"].ToString()))
                                                    {
                                                        dRow["Header1"] = System.Security.SecurityElement.Escape(dRow["Header1"].ToString());
                                                        cell.Value = dRow["Header1"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header2", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    if (!string.IsNullOrEmpty(dRow["Header2"].ToString()))
                                                    {
                                                        dRow["Header2"] = System.Security.SecurityElement.Escape(dRow["Header2"].ToString());
                                                        cell.Value = dRow["Header2"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header3", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    if (!string.IsNullOrEmpty(dRow["Header3"].ToString()))
                                                    {
                                                        dRow["Header3"] = System.Security.SecurityElement.Escape(dRow["Header3"].ToString());
                                                        cell.Value = dRow["Header3"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Header4", dRow))
                                                {
                                                    cell = worksheet.Cell(row, currCol + 1);
                                                    if (!string.IsNullOrEmpty(dRow["Header4"].ToString()))
                                                    {
                                                        dRow["Header4"] = System.Security.SecurityElement.Escape(dRow["Header4"].ToString());
                                                        cell.Value = dRow["Header3"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                    }
                                                    currCol++;
                                                }

                                                if (DoesCellExist("Amount", dRow))
                                                {
                                                    cell = worksheet.Cell(row, col + 14);
                                                    cell.Value = dRow["Amount"].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                }

                                                #region Commented Code
                                                //if (dRow["Description"].ToString() != null && dRow["Description"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 1);

                                                //    if (!string.IsNullOrEmpty(dRow["Description"].ToString()))
                                                //    {
                                                //        dRow["Description"] = System.Security.SecurityElement.Escape(dRow["Description"].ToString());
                                                //        cell.Value = dRow["Description"].ToString().Replace("amp;", "").Replace("&apos;", "");
                                                //    }
                                                //}

                                                //if (dRow["LedgerDate"].ToString() != null && dRow["LedgerDate"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 6);
                                                //    cell.Value = dRow["LedgerDate"].ToString();
                                                //    cell.StyleID = dtStyleID;
                                                //}

                                                //if (dRow["DocSrc"].ToString() != null && dRow["DocSrc"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 7);
                                                //    cell.Value = dRow["DocSrc"].ToString();
                                                //}

                                                //if (dRow["VoucherRef"].ToString() != null && dRow["VoucherRef"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 8);
                                                //    cell.Value = dRow["VoucherRef"].ToString();
                                                //}

                                                //if (dRow["Header1"].ToString() != null && dRow["Header1"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 9);
                                                //    cell.Value = dRow["Header1"].ToString();
                                                //}

                                                //if (dRow["Header2"].ToString() != null && dRow["Header2"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 10);
                                                //    cell.Value = dRow["Header2"].ToString();
                                                //}

                                                //if (dRow["Header3"].ToString() != null && dRow["Header3"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 11);
                                                //    cell.Value = dRow["Header3"].ToString();
                                                //}

                                                //if (dRow["Header4"].ToString() != null && dRow["Header4"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 12);
                                                //    cell.Value = dRow["Header4"].ToString();
                                                //}

                                                //if (dRow["Amount"].ToString() != null && dRow["Amount"].ToString() != "")
                                                //{
                                                //    cell = worksheet.Cell(row, col + 13);
                                                //    cell.Value = dRow["Amount"].ToString().Replace(",", "");
                                                //    cell.StyleID = amtStyleID;
                                                //}
                                                #endregion

                                                row++;

                                                if (childCount == drLinkedChildRows.Length - 1)
                                                {
                                                    //worksheet.InsertRow(row);

                                                    row++;
                                                    //worksheet.InsertRow(row);

                                                    strAmtEndAddr = worksheet.Cell(row - 1, col + 14).CellAddress;

                                                    cell = worksheet.Cell(row, col + 12);
                                                    cell.Value = dRow["SubTotal1Description"].ToString();
                                                    cell.StyleID = tBalFwdStyleID;

                                                    cell = worksheet.Cell(row, col + 14);
                                                    cell.RemoveValue();
                                                    cell.Formula = string.Format("SUM({0}:{1})", strAmtStartAddr, strAmtEndAddr);
                                                    cell.StyleID = boldAmtStyleID;
                                                    strAmtStartAddr = "";

                                                    row++;
                                                    //worksheet.InsertRow(row);

                                                    if (monthAmtAddr.Length == 0)
                                                    {
                                                        monthAmtAddr.Append(cell.CellAddress);
                                                    }
                                                    else
                                                    {
                                                        monthAmtAddr.Append("+" + cell.CellAddress);
                                                    }

                                                    row++;
                                                }
                                            }

                                            childDtRangeAddr = "";

                                            if (!noChild)
                                            {
                                                cell = worksheet.Cell(row - 2, col + 14);
                                                cell.StyleID = linkStyleID;

                                                //worksheet.InsertRow(row);
                                                cell = worksheet.Cell(row, col + 12);
                                                cell.Value = "Date Range Total:";
                                                cell.StyleID = dtRangeStyleID;

                                                //if (!noChild)
                                                //{
                                                cell = worksheet.Cell(row, col + 13);
                                                cell.StyleID = dtRangeStyleID;

                                                cell = worksheet.Cell(row, col + 14);
                                                cell.RemoveValue();
                                                cell.Formula = monthAmtAddr.ToString();
                                                cell.StyleID = boldAmtStyleID;


                                                childDtRangeAddr = cell.CellAddress;

                                                if (dateRangeAddr.Length != 0)
                                                {
                                                    dateRangeAddr.Append("+" + cell.CellAddress);
                                                }
                                                else
                                                {
                                                    dateRangeAddr.Append(cell.CellAddress);
                                                }

                                                row++;
                                                //worksheet.InsertRow(row);

                                                cell = worksheet.Cell(row, col + 12);
                                                cell.Value = "Ending Balance:";
                                                cell.StyleID = acctHeaderStyleID;

                                                cell = worksheet.Cell(row, col + 14);
                                                cell.RemoveValue();
                                                if (pBalForwardAddr == "")
                                                {
                                                    cell.Formula = childDtRangeAddr;
                                                    cell.StyleID = boldAmtStyleID;
                                                }
                                                else
                                                {
                                                    if (childDtRangeAddr == "")
                                                    {
                                                        cell.Formula = childDtRangeAddr;
                                                        cell.StyleID = boldAmtStyleID;
                                                    }

                                                    else
                                                    {
                                                        cell.Formula = pBalForwardAddr + "+" + childDtRangeAddr;
                                                        cell.StyleID = boldAmtStyleID;
                                                    }
                                                }

                                                //if (endBalanceAddr != string.Empty)
                                                //{
                                                //    endBalanceAddr = endBalanceAddr + "+" + cell.CellAddress;
                                                //}
                                                //else
                                                //{
                                                //    endBalanceAddr = cell.CellAddress;
                                                //}
                                                //}

                                                row++;
                                                //worksheet.InsertRow(row);
                                                rowInserted = true;
                                            }
                                            else
                                            {
                                                //worksheet.InsertRow(row);
                                                rowInserted = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        row++;
                                        //worksheet.InsertRow(row);
                                        rowInserted = true;
                                    }
                                }
                            }

                            int totalBalrow = row + 2;
                            cell = worksheet.Cell(totalBalrow, col + 11);
                            cell.Value = "Total Balance Forward: ";
                            cell.StyleID = acctHeaderStyleID;
                            cell = worksheet.Cell(totalBalrow, col + 14);
                            cell.RemoveValue();
                            cell.Formula = balForwardAddr.ToString();
                            cell.StyleID = boldAmtStyleID;
                            string totalBalAddr = cell.CellAddress;

                            int totalDtRangerow = totalBalrow + 1;
                            cell = worksheet.Cell(totalDtRangerow, col + 11);
                            cell.Value = "Total Date Range: ";
                            cell.StyleID = worksheet.Cell(totalDtRangerow, col + 12).StyleID
                                = worksheet.Cell(totalDtRangerow, col + 13).StyleID = dtRangeStyleID;
                            cell = worksheet.Cell(totalDtRangerow, col + 14);
                            cell.RemoveValue();
                            cell.Formula = dateRangeAddr.ToString();
                            cell.StyleID = boldAmtStyleID;
                            string totalDtRangeAddr = cell.CellAddress;

                            int totalEBalancerow = totalDtRangerow + 1;
                            cell = worksheet.Cell(totalEBalancerow, col + 11);
                            cell.Value = "Total Ending Balance: ";
                            cell.StyleID = acctHeaderStyleID;
                            cell = worksheet.Cell(totalEBalancerow, col + 14);
                            cell.RemoveValue();
                            cell.StyleID = boldAmtStyleID;
                            //cell.Formula = endBalanceAddr;
                            cell.Formula = totalBalAddr + "+" + totalDtRangeAddr;
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Report Style for Balance Sheet
        public void ReportStyle641(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            ReportSeries64(dtAllArray, dtHeader, "641", ArrhtColNameValues);
            #region Commented Code
            //string excelDocPath = string.Empty;
            //string excelDir = string.Empty;
            //string strHeader = string.Empty;

            //string compName = string.Empty;
            //string compDate = string.Empty;
            //string compDetails = string.Empty;

            //int acctHeaderStyleID;
            //int arrayCount = 0;

            //DataRow drCompDetails;
            //DataRow drCompDate;
            //DataRow drHeader;

            //Hashtable htTotal = new Hashtable();
            //Hashtable htGrandTotal = new Hashtable();

            //ExcelCell cell;

            //const int startRow = 6;
            //int row = startRow;
            //int col = 0;
            //bool rowInserted = false;

            //try
            //{
            //    excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
            //    excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
            //    }

            //    FileInfo newFile = new FileInfo(excelDocPath + @"\Balance Sheet" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
            //    }

            //    if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-641.xlsx"))
            //    {
            //        FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-641.xlsx");

            //        using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            //        {
            //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["BALANCE SHEET"];

            //            acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;

            //            drHeader = dtHeader.Rows[arrayCount];

            //            strHeader = drHeader["Column1"].ToString();
            //            string[] strRequestedBy = strHeader.Split(':');
            //            cell = worksheet.Cell(1, 2);

            //            if (strRequestedBy.Length > 1)
            //            {
            //                cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
            //            }
            //            else
            //            {
            //                cell.Value = strRequestedBy[arrayCount].Trim().ToString();
            //            }

            //            compName = drHeader["Column2"].ToString();
            //            cell = worksheet.Cell(1, 5);
            //            cell.Value = compName;
            //            //cell.StyleID = acctHeaderStyleID;

            //            drCompDetails = dtHeader.Rows[arrayCount + 1];
            //            compDetails = drCompDetails["Column2"].ToString();
            //            cell = worksheet.Cell(2, 5);
            //            cell.Value = compDetails;
            //            //cell.StyleID = acctHeaderStyleID;

            //            drCompDate = dtHeader.Rows[2];
            //            compDate = drCompDate["Column2"].ToString();
            //            cell = worksheet.Cell(3, 5);
            //            cell.Value = compDate;
            //            //cell.StyleID = acctHeaderStyleID;

            //            DataTable dtInitArray = dtAllArray[arrayCount];

            //            for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
            //            {
            //                htGrandTotal.Clear();

            //                DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];

            //                if (row > startRow)
            //                {
            //                    if (rowInserted)
            //                    {
            //                        row = row + 1;
            //                        worksheet.InsertRow(row);
            //                    }
            //                    else
            //                    {
            //                        worksheet.InsertRow(row);
            //                    }
            //                }

            //                //for loading the parent
            //                if (drInitArrayRow[arrayCount + 1] != null && drInitArrayRow[arrayCount + 1].ToString() != "")
            //                {
            //                    cell = worksheet.Cell(row, col + 2);
            //                    foreach (DataColumn coloumn in dtInitArray.Columns)
            //                    {
            //                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
            //                        {
            //                            cell.Value = drInitArrayRow[coloumn.ColumnName].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;
            //                            break;
            //                        }
            //                    }

            //                    row++;
            //                    worksheet.InsertRow(row);
            //                }

            //                if (drInitArrayRow["Link1"].ToString() != "" && drInitArrayRow["Link1"].ToString() != null)
            //                {
            //                    row++;
            //                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

            //                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

            //                    for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
            //                    {
            //                        if (row > startRow)
            //                        {
            //                            worksheet.InsertRow(row);
            //                        }
            //                        //for loading the linked array
            //                        cell = worksheet.Cell(row, col + 3);
            //                        foreach (DataColumn coloumn in dtAllArray[arrayCount + 1].Columns)
            //                        {
            //                            if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
            //                            {
            //                                cell.Value = drLinkedArray[LinkedArrCount][coloumn.ColumnName].ToString();
            //                                //cell.StyleID = acctHeaderStyleID;
            //                                break;
            //                            }
            //                        }

            //                        row++;
            //                        worksheet.InsertRow(row);

            //                        if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
            //                        {
            //                            int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
            //                            DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

            //                            for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
            //                            {
            //                                if (drLinkedAcctArray[linkedAcctCount]["Link3"] != null && drLinkedAcctArray[linkedAcctCount]["Link3"].ToString() != "")
            //                                {
            //                                    //for loading acct details
            //                                    cell = worksheet.Cell(row, col + 4);
            //                                    foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
            //                                    {
            //                                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2" && coloumn.ColumnName != "Link3")
            //                                        {
            //                                            cell.Value = drLinkedAcctArray[linkedAcctCount][coloumn.ColumnName].ToString();
            //                                            break;
            //                                        }
            //                                    }

            //                                    int link3 = Convert.ToInt32(drLinkedAcctArray[linkedAcctCount]["Link3"].ToString());
            //                                    DataRow[] drLinkedDetailArray = dtAllArray[arrayCount + 3].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 8);

            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");
            //                                        }
            //                                    }

            //                                    if (linkedAcctCount == 0)
            //                                    {
            //                                        htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;
            //                                    }

            //                                    row++;
            //                                    worksheet.InsertRow(row);
            //                                }
            //                            }

            //                            cell = worksheet.Cell(row - 1, col + 8);
            //                            string endAmtAddr = cell.CellAddress;

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            cell = worksheet.Cell(row, col + 3);
            //                            cell.Value = "Total" + " " + drLinkedArray[LinkedArrCount][arrayCount + 1].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;

            //                            cell = worksheet.Cell(row, col + 8);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endAmtAddr);

            //                            if (htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            row++;
            //                        }
            //                    }
            //                }
            //                worksheet.InsertRow(row);

            //                row++;
            //                worksheet.InsertRow(row);

            //                cell = worksheet.Cell(row, col + 3);
            //                cell.Value = "Total" + " " + drInitArrayRow[arrayCount + 1].ToString();
            //                //cell.StyleID = acctHeaderStyleID;

            //                cell = worksheet.Cell(row, col + 8);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                row++;
            //                worksheet.InsertRow(row);

            //                rowInserted = true;
            //            }
            //            xlPackage.Save();
            //        }
            //    }
            //    SaveToClient(newFile);
            //}
            //catch (Exception ex)
            //{ }
            #endregion
        }

        //Report Style for Balance Sheet Compare
        public void ReportStyle642(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            ReportSeries64(dtAllArray, dtHeader, "642", ArrhtColNameValues);
            #region Commented Code
            //string excelDocPath = string.Empty;
            //string excelDir = string.Empty;
            //string strHeader = string.Empty;

            //string compName = string.Empty;
            //string compDate = string.Empty;
            //string compDetails = string.Empty;

            //int acctHeaderStyleID;
            //int arrayCount = 0;

            //DataRow drCompDetails;
            //DataRow drCompDate;
            //DataRow drHeader;

            //Hashtable htTotal = new Hashtable();
            //Hashtable htTotalCompare = new Hashtable();

            //Hashtable htGrandTotal = new Hashtable();
            //Hashtable htGrandTotalCompare = new Hashtable();

            //ExcelCell cell;

            //const int startRow = 6;
            //int row = startRow;
            //int col = 0;
            //bool rowInserted = false;

            //try
            //{
            //    excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
            //    excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
            //    }

            //    FileInfo newFile = new FileInfo(excelDocPath + @"\Balance Sheet Compare" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
            //    }

            //    if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-642.xlsx"))
            //    {
            //        FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-642.xlsx");

            //        using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            //        {
            //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["BALANCE SHEET compare"];

            //            acctHeaderStyleID = worksheet.Cell(1, 2).StyleID;

            //            drHeader = dtHeader.Rows[arrayCount];

            //            strHeader = drHeader["Column1"].ToString();
            //            string[] strRequestedBy = strHeader.Split(':');
            //            cell = worksheet.Cell(1, 2);

            //            if (strRequestedBy.Length > 1)
            //            {
            //                cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
            //            }
            //            else
            //            {
            //                cell.Value = strRequestedBy[arrayCount].Trim().ToString();
            //            }

            //            compName = drHeader["Column2"].ToString();
            //            cell = worksheet.Cell(1, 5);
            //            cell.Value = compName;
            //            cell.StyleID = acctHeaderStyleID;

            //            drCompDetails = dtHeader.Rows[arrayCount + 1];
            //            compDetails = drCompDetails["Column2"].ToString();
            //            cell = worksheet.Cell(2, 5);
            //            cell.Value = compDetails;
            //            cell.StyleID = acctHeaderStyleID;

            //            drCompDate = dtHeader.Rows[2];
            //            compDate = drCompDate["Column2"].ToString();
            //            cell = worksheet.Cell(3, 5);
            //            cell.Value = compDate;
            //            cell.StyleID = acctHeaderStyleID;

            //            DataTable dtInitArray = dtAllArray[arrayCount];

            //            for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
            //            {
            //                htGrandTotal.Clear();
            //                htGrandTotalCompare.Clear();

            //                DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];

            //                if (row > startRow)
            //                {
            //                    if (rowInserted)
            //                    {
            //                        row = row + 1;
            //                        worksheet.InsertRow(row);
            //                    }
            //                    else
            //                    {
            //                        worksheet.InsertRow(row);
            //                    }
            //                }

            //                //for loading the parent
            //                if (drInitArrayRow[arrayCount + 1] != null && drInitArrayRow[arrayCount + 1].ToString() != "")
            //                {
            //                    cell = worksheet.Cell(row, col + 2);
            //                    foreach (DataColumn coloumn in dtInitArray.Columns)
            //                    {
            //                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
            //                        {
            //                            cell.Value = drInitArrayRow[coloumn.ColumnName].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;
            //                            break;
            //                        }
            //                    }

            //                    row++;
            //                    worksheet.InsertRow(row);
            //                }

            //                if (drInitArrayRow["Link1"].ToString() != "" && drInitArrayRow["Link1"].ToString() != null)
            //                {
            //                    row++;
            //                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

            //                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

            //                    for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
            //                    {
            //                        if (row > startRow)
            //                        {
            //                            worksheet.InsertRow(row);
            //                        }
            //                        //for loading the linked array
            //                        cell = worksheet.Cell(row, col + 3);
            //                        foreach (DataColumn coloumn in dtAllArray[arrayCount + 1].Columns)
            //                        {
            //                            if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
            //                            {
            //                                cell.Value = drLinkedArray[LinkedArrCount][coloumn.ColumnName].ToString();
            //                                //cell.StyleID = acctHeaderStyleID;
            //                                break;
            //                            }
            //                        }

            //                        row++;
            //                        worksheet.InsertRow(row);

            //                        if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
            //                        {
            //                            int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
            //                            DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

            //                            for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
            //                            {
            //                                if (drLinkedAcctArray[linkedAcctCount]["Link3"] != null && drLinkedAcctArray[linkedAcctCount]["Link3"].ToString() != "")
            //                                {
            //                                    //for loading acct details
            //                                    cell = worksheet.Cell(row, col + 4);
            //                                    foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
            //                                    {
            //                                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2" && coloumn.ColumnName != "Link3")
            //                                        {
            //                                            cell.Value = drLinkedAcctArray[linkedAcctCount][coloumn.ColumnName].ToString();
            //                                            break;
            //                                        }
            //                                    }

            //                                    int link3 = Convert.ToInt32(drLinkedAcctArray[linkedAcctCount]["Link3"].ToString());
            //                                    DataRow[] drLinkedDetailArray = dtAllArray[arrayCount + 3].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 8);
            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");
            //                                        }
            //                                    }

            //                                    drLinkedDetailArray = dtAllArray[arrayCount + 4].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 10);
            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");
            //                                        }
            //                                    }

            //                                    if (linkedAcctCount == 0)
            //                                    {
            //                                        cell = worksheet.Cell(row, col + 8);
            //                                        htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;

            //                                        cell = worksheet.Cell(row, col + 10);
            //                                        htTotalCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;
            //                                    }

            //                                    row++;
            //                                    worksheet.InsertRow(row);
            //                                }
            //                            }

            //                            cell = worksheet.Cell(row - 1, col + 8);
            //                            string endAmtAddr = cell.CellAddress;

            //                            cell = worksheet.Cell(row - 1, col + 10);
            //                            string endCompareAmtAddr = cell.CellAddress;

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            cell = worksheet.Cell(row, col + 3);
            //                            cell.Value = "Total" + " " + drLinkedArray[LinkedArrCount][arrayCount + 1].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;

            //                            cell = worksheet.Cell(row, col + 8);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endAmtAddr);

            //                            if (htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            cell = worksheet.Cell(row, col + 10);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotalCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endCompareAmtAddr);

            //                            if (htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            row++;
            //                        }
            //                    }
            //                }
            //                worksheet.InsertRow(row);

            //                row++;
            //                worksheet.InsertRow(row);

            //                cell = worksheet.Cell(row, col + 3);
            //                cell.Value = "Total" + " " + drInitArrayRow[arrayCount + 1].ToString();
            //                //cell.StyleID = acctHeaderStyleID;

            //                cell = worksheet.Cell(row, col + 8);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                cell = worksheet.Cell(row, col + 10);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                row++;
            //                worksheet.InsertRow(row);

            //                rowInserted = true;
            //            }
            //            xlPackage.Save();
            //        }
            //    }
            //    SaveToClient(newFile);
            //}
            //catch (Exception ex)
            //{ }
            #endregion
        }

        //Report Style for Balance Sheet Division
        public void ReportStyle643(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            ReportSeries64(dtAllArray, dtHeader, "643", ArrhtColNameValues);
            #region Commented Code
            //string excelDocPath = string.Empty;
            //string excelDir = string.Empty;
            //string strHeader = string.Empty;

            //string compName = string.Empty;
            //string compDate = string.Empty;
            //string compDetails = string.Empty;
            //string divTotal = string.Empty;

            //int acctHeaderStyleID;
            //int arrayCount = 0;

            //DataRow drCompDetails;
            //DataRow drCompDate;
            //DataRow drHeader;

            //Hashtable htTotal = new Hashtable();
            //Hashtable htTotalCompare = new Hashtable();
            //Hashtable htTotalDivCompare = new Hashtable();
            //Hashtable htDivTotal = new Hashtable();

            //Hashtable htGrandTotal = new Hashtable();
            //Hashtable htGrandTotalCompare = new Hashtable();
            //Hashtable htGrandTotalDivCompare = new Hashtable();
            //Hashtable htDivGrandTotal = new Hashtable();

            //ExcelCell cell;

            //const int startRow = 6;
            //int row = startRow;
            //int col = 0;
            //bool rowInserted = false;

            //try
            //{
            //    excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
            //    excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
            //    }

            //    FileInfo newFile = new FileInfo(excelDocPath + @"\Balance Sheet" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

            //    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
            //    {
            //        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
            //    }

            //    if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-641.xlsx"))
            //    {
            //        FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-641.xlsx");

            //        using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            //        {
            //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["BALANCE SHEET"];

            //            acctHeaderStyleID = worksheet.Cell(1, 2).StyleID;

            //            drHeader = dtHeader.Rows[arrayCount];

            //            #region Commented Code
            //            //strHeader = drHeader["Column1"].ToString();
            //            //string[] strRequestedBy = strHeader.Split(':');
            //            //cell = worksheet.Cell(1, 2);

            //            //if (strRequestedBy.Length > 1)
            //            //{
            //            //    cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
            //            //}
            //            //else
            //            //{
            //            //    cell.Value = strRequestedBy[arrayCount].Trim().ToString();
            //            //}
            //            #endregion

            //            compName = drHeader["Column2"].ToString();
            //            cell = worksheet.Cell(1, 5);
            //            cell.Value = compName;
            //            //cell.StyleID = acctHeaderStyleID;
            //            cell.StyleID = 9;

            //            drCompDetails = dtHeader.Rows[arrayCount + 1];
            //            compDetails = drCompDetails["Column2"].ToString();
            //            cell = worksheet.Cell(2, 5);
            //            cell.Value = compDetails;
            //            //cell.StyleID = acctHeaderStyleID;
            //            cell.StyleID = 9;

            //            drCompDate = dtHeader.Rows[2];
            //            compDate = drCompDate["Column2"].ToString();
            //            cell = worksheet.Cell(3, 5);
            //            cell.Value = compDate;
            //            //cell.StyleID = acctHeaderStyleID;
            //            cell.StyleID = 9;

            //            DataTable dtInitArray = dtAllArray[arrayCount];

            //            for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
            //            {
            //                htGrandTotal.Clear();
            //                htGrandTotalCompare.Clear();
            //                htDivGrandTotal.Clear();

            //                DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];

            //                if (row > startRow)
            //                {
            //                    if (rowInserted)
            //                    {
            //                        row = row + 1;
            //                        worksheet.InsertRow(row);
            //                    }
            //                    else
            //                    {
            //                        worksheet.InsertRow(row);
            //                    }
            //                }

            //                //for loading the parent
            //                if (drInitArrayRow[arrayCount + 1] != null && drInitArrayRow[arrayCount + 1].ToString() != "")
            //                {
            //                    cell = worksheet.Cell(row, col + 2);
            //                    foreach (DataColumn coloumn in dtInitArray.Columns)
            //                    {
            //                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
            //                        {
            //                            cell.Value = drInitArrayRow[coloumn.ColumnName].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;
            //                            break;
            //                        }
            //                    }

            //                    row++;
            //                    worksheet.InsertRow(row);
            //                }

            //                if (drInitArrayRow["Link1"].ToString() != "" && drInitArrayRow["Link1"].ToString() != null)
            //                {
            //                    row++;
            //                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

            //                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

            //                    for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
            //                    {
            //                        divTotal = "";

            //                        if (row > startRow)
            //                        {
            //                            worksheet.InsertRow(row);
            //                        }
            //                        //for loading the linked array
            //                        cell = worksheet.Cell(row, col + 3);
            //                        foreach (DataColumn coloumn in dtAllArray[arrayCount + 1].Columns)
            //                        {
            //                            if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
            //                            {
            //                                cell.Value = drLinkedArray[LinkedArrCount][coloumn.ColumnName].ToString();
            //                                //cell.StyleID = acctHeaderStyleID;
            //                                break;
            //                            }
            //                        }

            //                        row++;
            //                        worksheet.InsertRow(row);

            //                        if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
            //                        {
            //                            int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
            //                            DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

            //                            for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
            //                            {
            //                                divTotal = "";
            //                                if (drLinkedAcctArray[linkedAcctCount]["Link3"] != null && drLinkedAcctArray[linkedAcctCount]["Link3"].ToString() != "")
            //                                {
            //                                    //for loading acct details
            //                                    cell = worksheet.Cell(row, col + 4);
            //                                    foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
            //                                    {
            //                                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2" && coloumn.ColumnName != "Link3")
            //                                        {
            //                                            cell.Value = drLinkedAcctArray[linkedAcctCount][coloumn.ColumnName].ToString();
            //                                            break;
            //                                        }
            //                                    }

            //                                    int link3 = Convert.ToInt32(drLinkedAcctArray[linkedAcctCount]["Link3"].ToString());
            //                                    DataRow[] drLinkedDetailArray = dtAllArray[arrayCount + 3].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 8);

            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");

            //                                            if (divTotal == "")
            //                                            {
            //                                                divTotal = cell.CellAddress;
            //                                            }
            //                                            else
            //                                            {
            //                                                divTotal = divTotal + "+" + cell.CellAddress;
            //                                            }
            //                                        }
            //                                    }

            //                                    drLinkedDetailArray = dtAllArray[arrayCount + 4].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 10);

            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");

            //                                            if (divTotal == "")
            //                                            {
            //                                                divTotal = cell.CellAddress;
            //                                            }
            //                                            else
            //                                            {
            //                                                divTotal = divTotal + "+" + cell.CellAddress;
            //                                            }
            //                                        }
            //                                    }

            //                                    drLinkedDetailArray = dtAllArray[arrayCount + 5].Select("Link3 ='" + link3 + "'");
            //                                    cell = worksheet.Cell(row, col + 12);

            //                                    if (drLinkedDetailArray.Length > 0)
            //                                    {
            //                                        if (drLinkedDetailArray[arrayCount][arrayCount + 1] != null && drLinkedDetailArray[arrayCount][arrayCount + 1].ToString() != "")
            //                                        {
            //                                            //for loading amt
            //                                            cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");

            //                                            if (divTotal == "")
            //                                            {
            //                                                divTotal = cell.CellAddress;
            //                                            }
            //                                            else
            //                                            {
            //                                                divTotal = divTotal + "+" + cell.CellAddress;
            //                                            }
            //                                        }
            //                                    }

            //                                    cell = worksheet.Cell(row, col + 14);
            //                                    cell.RemoveValue();
            //                                    cell.Formula = divTotal;

            //                                    if (linkedAcctCount == 0)
            //                                    {
            //                                        cell = worksheet.Cell(row, col + 8);
            //                                        htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;

            //                                        cell = worksheet.Cell(row, col + 10);
            //                                        htTotalCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;

            //                                        cell = worksheet.Cell(row, col + 12);
            //                                        htTotalDivCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;

            //                                        cell = worksheet.Cell(row, col + 14);
            //                                        htDivTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()] = cell.CellAddress;
            //                                    }

            //                                    row++;
            //                                    worksheet.InsertRow(row);
            //                                }
            //                            }

            //                            cell = worksheet.Cell(row - 1, col + 8);
            //                            string endAmtAddr = cell.CellAddress;

            //                            cell = worksheet.Cell(row - 1, col + 10);
            //                            string endCompareAmtAddr = cell.CellAddress;

            //                            cell = worksheet.Cell(row - 1, col + 12);
            //                            string endDivCompareAmtAddr = cell.CellAddress;

            //                            cell = worksheet.Cell(row - 1, col + 14);
            //                            string endDivTotalAddr = cell.CellAddress;

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            cell = worksheet.Cell(row, col + 3);
            //                            cell.Value = "Total" + " " + drLinkedArray[LinkedArrCount][arrayCount + 1].ToString();
            //                            //cell.StyleID = acctHeaderStyleID;

            //                            cell = worksheet.Cell(row, col + 8);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endAmtAddr);

            //                            if (htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            cell = worksheet.Cell(row, col + 10);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotalCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endCompareAmtAddr);

            //                            if (htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            cell = worksheet.Cell(row, col + 12);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htTotalDivCompare[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endDivCompareAmtAddr);
            //                            if (htGrandTotalDivCompare[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htGrandTotalDivCompare[drInitArrayRow[arrayCount + 1].ToString()] = htGrandTotalDivCompare[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htGrandTotalDivCompare[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }


            //                            cell = worksheet.Cell(row, col + 14);
            //                            cell.RemoveValue();
            //                            cell.Formula = string.Format("SUM({0}:{1})", htDivTotal[drLinkedArray[LinkedArrCount][arrayCount + 1].ToString()], endDivTotalAddr);

            //                            if (htDivGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] != null)
            //                            {
            //                                htDivGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = htDivGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] + "+" + cell.CellAddress;
            //                            }
            //                            else
            //                            {
            //                                htDivGrandTotal[drInitArrayRow[arrayCount + 1].ToString()] = cell.CellAddress;
            //                            }

            //                            row++;
            //                            worksheet.InsertRow(row);

            //                            row++;
            //                        }
            //                    }
            //                }
            //                worksheet.InsertRow(row);

            //                row++;
            //                worksheet.InsertRow(row);

            //                cell = worksheet.Cell(row, col + 3);
            //                cell.Value = "Total" + " " + drInitArrayRow[arrayCount + 1].ToString();
            //                //cell.StyleID = acctHeaderStyleID;

            //                cell = worksheet.Cell(row, col + 8);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotal[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                cell = worksheet.Cell(row, col + 10);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotalCompare[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                cell = worksheet.Cell(row, col + 12);
            //                cell.RemoveValue();
            //                cell.Formula = htGrandTotalDivCompare[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                cell = worksheet.Cell(row, col + 14);
            //                cell.RemoveValue();
            //                cell.Formula = htDivGrandTotal[drInitArrayRow[arrayCount + 1].ToString()].ToString();

            //                row++;
            //                worksheet.InsertRow(row);

            //                rowInserted = true;
            //            }
            //            xlPackage.Save();
            //        }
            //    }
            //    SaveToClient(newFile);
            //}
            //catch (Exception ex)
            //{

            //}
            #endregion
        }

        //Common Method for '64' series Reports
        public void ReportSeries64(DataTable[] dtAllArray, DataTable dtHeader, string ReportStyle, Hashtable[] ArrhtColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htCompare = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            Hashtable htAstLiability = new Hashtable();

            ExcelCell cell;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            bool rowInserted = false;

            int clm = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                if (ReportStyle == "641")
                {
                    fileName = @"\Balance Sheet";
                    xlTemplate = @"\ExcelReportTemplate-641.xlsx";
                    wrkSheet = "BALANCE SHEET";
                }

                if (ReportStyle == "642")
                {
                    fileName = @"\Balance Sheet Compare";
                    xlTemplate = @"\ExcelReportTemplate-642.xlsx";
                    wrkSheet = "BALANCE SHEET compare";
                }

                if (ReportStyle == "643")
                {
                    fileName = @"\Balance Sheet Division";
                    xlTemplate = @"\ExcelReportTemplate-643.xlsx";
                    wrkSheet = "BALANCE SHEET division";
                }
                FileInfo newFile = null;

                if (fileName != string.Empty)
                {
                    newFile = new FileInfo(excelDocPath + fileName + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");
                }

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + xlTemplate))
                {
                    FileInfo template = new FileInfo(excelDir + xlTemplate);

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[wrkSheet];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 8).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
                        {
                            htGrandTotal.Clear();

                            DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];

                            if (row > startRow)
                            {
                                if (rowInserted)
                                {
                                    row = row + 1;
                                    worksheet.InsertRow(row);
                                }
                                else
                                {
                                    worksheet.InsertRow(row);
                                }
                            }

                            //for loading the parent
                            if (drInitArrayRow[arrayCount + 1] != null && drInitArrayRow[arrayCount + 1].ToString() != "")
                            {
                                cell = worksheet.Cell(row, col + 2);
                                foreach (DataColumn coloumn in dtInitArray.Columns)
                                {
                                    if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
                                    {
                                        cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                        cell.StyleID = acctHeaderStyleID;

                                        break;
                                    }
                                }

                                row++;
                                worksheet.InsertRow(row);
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }
                                    //for loading the Child Acct Name
                                    cell = worksheet.Cell(row, col + 3);
                                    foreach (DataColumn coloumn in dtAllArray[arrayCount + 1].Columns)
                                    {
                                        if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[LinkedArrCount][coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = acctHeaderStyleID;
                                            break;
                                        }
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            divTotal = "";
                                            if (drLinkedAcctArray[linkedAcctCount]["Link3"] != null && drLinkedAcctArray[linkedAcctCount]["Link3"].ToString() != "")
                                            {
                                                //for loading acct details
                                                cell = worksheet.Cell(row, col + 4);
                                                foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
                                                {
                                                    if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2" && coloumn.ColumnName != "Link3")
                                                    {
                                                        cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount][coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        break;
                                                    }
                                                }

                                                int link3 = Convert.ToInt32(drLinkedAcctArray[linkedAcctCount]["Link3"].ToString());
                                                clm = 8;

                                                for (int lnkArrayCount = 3; lnkArrayCount < dtAllArray.Length; lnkArrayCount++)
                                                {
                                                    DataRow[] drLinkedDetailArray = dtAllArray[arrayCount + lnkArrayCount].Select("Link3 ='" + link3 + "'");
                                                    cell = worksheet.Cell(row, col + clm);
                                                    if (drLinkedDetailArray.Length > 0)
                                                    {
                                                        cell.Value = drLinkedDetailArray[arrayCount][arrayCount + 1].ToString().Replace(",", "");
                                                        cell.StyleID = amtStyleID;
                                                    }

                                                    if (divTotal == "")
                                                    {
                                                        divTotal = cell.CellAddress;
                                                    }
                                                    else
                                                    {
                                                        divTotal = divTotal + "+" + cell.CellAddress;
                                                    }

                                                    if (linkedAcctCount == 0)
                                                    {
                                                        htCompare["tblArrayVal" + clm] = cell.CellAddress;
                                                    }

                                                    clm = clm + 2;
                                                }

                                                if (ReportStyle != "641" && ReportStyle != "642")
                                                {
                                                    cell = worksheet.Cell(row, col + clm);
                                                    cell.RemoveValue();
                                                    cell.Formula = divTotal;
                                                    cell.StyleID = amtStyleID;
                                                }

                                                //for Calculating Variance for each row
                                                if (ReportStyle == "642")
                                                {
                                                    cell = worksheet.Cell(row, col + clm);
                                                    cell.RemoveValue();
                                                    cell.Formula = worksheet.Cell(row, clm - 2).CellAddress + "-" + worksheet.Cell(row, clm - 4).CellAddress;
                                                    cell.StyleID = amtStyleID;
                                                }

                                                if (linkedAcctCount == 0)
                                                {
                                                    htCompare["tblArrayVal" + clm] = cell.CellAddress;
                                                }

                                                row++;
                                                worksheet.InsertRow(row);
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        cell = worksheet.Cell(row, col + 3);
                                        cell.Value = "Total" + " " + drLinkedArray[LinkedArrCount][arrayCount + 1].ToString();
                                        cell.StyleID = acctHeaderStyleID;

                                        //Formula For Coloumn totals
                                        for (int j = 8; j <= clm; j++)
                                        {
                                            if (j == clm)
                                            {
                                                if (ReportStyle == "641")
                                                    break;

                                                if (ReportStyle == "642")
                                                {
                                                    cell = worksheet.Cell(row - 1, col + j);
                                                    cell.StyleID = totAmtStyleID;

                                                    cell = worksheet.Cell(row, col + j);
                                                    cell.RemoveValue();
                                                    cell.Formula = worksheet.Cell(row, clm - 2).CellAddress + "-" + worksheet.Cell(row, clm - 4).CellAddress;
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }
                                            }

                                            cell = worksheet.Cell(row - 1, col + j);
                                            cell.StyleID = totAmtStyleID;

                                            cell = worksheet.Cell(row, col + j);
                                            cell.RemoveValue();
                                            cell.Formula = string.Format("SUM({0}:{1})", htCompare["tblArrayVal" + j], worksheet.Cell(row - 2, col + j).CellAddress);
                                            cell.StyleID = amtStyleID;

                                            if (htGrandTotal["tblArrayVal" + j] != null)
                                            {
                                                htGrandTotal["tblArrayVal" + j] = htGrandTotal["tblArrayVal" + j] + "+" + cell.CellAddress;
                                            }
                                            else
                                            {
                                                htGrandTotal["tblArrayVal" + j] = cell.CellAddress;
                                            }
                                            j++;
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                    }
                                }
                            }
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);

                            // Row Value Formula for Total Assets, Total Liabilities (Parent Totals)
                            cell = worksheet.Cell(row, col + 2);
                            cell.Value = "Total" + " " + drInitArrayRow[arrayCount + 1].ToString();
                            cell.StyleID = acctHeaderStyleID;

                            for (int colCount = 8; colCount <= clm; colCount++)
                            {
                                if (colCount == clm)
                                {
                                    if (ReportStyle == "641")
                                        break;

                                    if (ReportStyle == "642")
                                    {
                                        cell = worksheet.Cell(row - 2, col + colCount);
                                        cell.StyleID = totAmtStyleID;

                                        cell = worksheet.Cell(row, col + colCount);
                                        cell.RemoveValue();
                                        cell.Formula = worksheet.Cell(row, clm - 2).CellAddress + "-" + worksheet.Cell(row, clm - 4).CellAddress;
                                        cell.StyleID = totAmtStyleID;

                                        htAstLiability[dtarrayCount.ToString() + colCount.ToString()] = cell.CellAddress;

                                        break;
                                    }
                                }

                                cell = worksheet.Cell(row - 2, col + colCount);
                                cell.StyleID = totAmtStyleID;

                                cell = worksheet.Cell(row, col + colCount);
                                cell.RemoveValue();
                                cell.Formula = htGrandTotal["tblArrayVal" + colCount].ToString();

                                htAstLiability[dtarrayCount.ToString() + colCount.ToString()] = cell.CellAddress;

                                cell.StyleID = totAmtStyleID;
                                colCount++;
                            }

                            row++;
                            worksheet.InsertRow(row);

                            rowInserted = true;
                        }

                        //For Calculating Net Profit
                        for (int colCount = 8; colCount <= clm; colCount++)
                        {
                            if (colCount == clm)
                            {
                                if (ReportStyle == "641")
                                    break;
                            }

                            cell = worksheet.Cell(row, col + colCount);
                            cell.RemoveValue();
                            cell.Formula = htAstLiability[arrayCount.ToString() + colCount.ToString()].ToString() + "-" + htAstLiability[(arrayCount + 1).ToString() + colCount.ToString()].ToString();

                            cell.StyleID = amtStyleID;
                            colCount++;
                        }

                        int divCount = 3;

                        //Display ColNames
                        for (int colCount = 8; colCount <= clm; colCount++)
                        {
                            cell = worksheet.Cell(startRow, colCount);
                            if (colCount < clm)
                            {
                                //cell.Value = "Division#" + divCount;
                                //cell.Value = htColNameValues["EndBal"].ToString();
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[divCount]["EndBal"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;

                                divCount++;
                                colCount++;
                            }
                            else
                            {
                                if (ReportStyle != "641" && ReportStyle != "642")
                                {
                                    cell.Value = "Total";
                                    cell.StyleID = colStyleID;
                                }

                                if (ReportStyle == "642")
                                {
                                    cell.Value = "Variance";
                                    cell.StyleID = colStyleID;
                                }
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        //Report Styles for Income Statement
        //IS vs Budget
        public void ReportStyle651(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtCNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string cellVal = String.Empty;
            string parentName = string.Empty;
            string cellAddr = string.Empty;

            string actAmt = string.Empty;
            string bgtAmt = string.Empty;

            string varActAmt = string.Empty;
            string varbgtAmt = string.Empty;

            string varGrossProfit = string.Empty;
            string varOtherCosts = string.Empty;
            string varOtherIncome = string.Empty;
            string varNetIncBefTaxes = string.Empty;
            string varTaxes = string.Empty;

            Hashtable htTotalAmt = new Hashtable();
            Hashtable htTotalRevenue = new Hashtable();
            Hashtable htTotalProdCost = new Hashtable();
            Hashtable htTotalGrossProfit = new Hashtable();
            Hashtable htTotalOtherCosts = new Hashtable();
            Hashtable htTotalOtherIncome = new Hashtable();
            Hashtable htTotNetIncBefTax = new Hashtable();
            Hashtable htTotalTaxes = new Hashtable();
            Hashtable htVariance = new Hashtable();
            Hashtable htIsRevEmpty = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 10;
            int row = startRow;
            int col = 0;

            int headerStyleID = 0;
            int acctHeaderStyleID = 0;
            int monthStyleID = 0;
            int amtStyleID = 0;
            int amtCellStyleID = 0;
            int boldAmtStyleID = 0;
            int percentColStyleID = 0;

            int arrayCount = 0;

            bool noChild = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\IS QTR VS budget" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-651.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-651.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["IS QTR VS budget"];
                        ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtCellStyleID = worksheet.Cell(4, 6).StyleID;
                        boldAmtStyleID = worksheet.Cell(4, 1).StyleID;
                        percentColStyleID = worksheet.Cell(4, 4).StyleID;
                        amtStyleID = worksheet.Cell(5, 6).StyleID;
                        monthStyleID = worksheet.Cell(6, 5).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 4).StyleID = 0;
                        worksheet.Cell(5, 6).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        headerStyleID = worksheet.Cell(1, 1).StyleID;

                        //Loading The date in 6th row
                        cell = worksheet.Cell(6, 5);
                        cell.Value = ArrhtCNameValues[arrayCount + 2]["EndBal"].ToString();

                        //First Data Table (Report Detail)
                        DataTable dtInitArray = dtAllArray[arrayCount];

                        //Starting with first parent
                        for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
                        {
                            htTotalAmt.Clear();

                            parentName = "";

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];
                            cell = worksheet.Cell(row, col + 1);

                            //Display Parent
                            foreach (DataColumn coloumn in dtInitArray.Columns)
                            {
                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    parentName = cell.Value.ToString();
                                    cellVal = drInitArrayRow["Link1"].ToString();
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            //If != Revenue
                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            string colName = string.Empty;

                            //If != Revenue && != NetIncomeBeforeTaxes && != NetIncome
                            if (cellVal != "30" && cellVal != "60" && cellVal != "900")
                            {
                                if (drInitArrayRow["Link1"] != null && drInitArrayRow["Link1"].ToString() != "")
                                {
                                    // Getting Data Based on Link1 from DataTable Array (2nd Data Table, Account Detail))
                                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());
                                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                    for (int lnkArrCount = 0; lnkArrCount < drLinkedArray.Length; lnkArrCount++)
                                    {
                                        //for loading Account Name 
                                        cell = worksheet.Cell(row, col + 2);

                                        //If != Revenue
                                        if (cellVal != "10")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[lnkArrCount][arrayCount + 1].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            noChild = false;
                                        }
                                        else
                                        {
                                            noChild = true;
                                        }

                                        //for loading Amount
                                        // Getting Data Based on Link2 from DataTable Array (3rd Data Table, TrxDetail1))
                                        int link2 = Convert.ToInt32(drLinkedArray[lnkArrCount]["Link2"].ToString());
                                        DataRow[] drPrimaryAmtArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 5);

                                        if (drPrimaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drPrimaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtCellStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Actual"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Actual"] = "Empty";
                                            }

                                            htTotalRevenue["Actual"] = cell.CellAddress;
                                        }

                                        actAmt = cell.CellAddress;

                                        // Getting Data Based on Link2 from DataTable Array (4th Data Table, TrxDetail2))
                                        DataRow[] drSecondaryAmtArray = dtAllArray[arrayCount + 3].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 7);

                                        if (drSecondaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 3].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drSecondaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtCellStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Budget"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Budget"] = "Empty";
                                            }

                                            htTotalRevenue["Budget"] = cell.CellAddress;
                                        }

                                        bgtAmt = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 9);
                                        cell.RemoveValue();
                                        cell.Formula = actAmt + "-" + bgtAmt;
                                        cell.StyleID = amtCellStyleID;

                                        if (lnkArrCount == 0)
                                        {
                                            htVariance["Variance"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Variance"] = "Empty";
                                            }

                                            htTotalRevenue["Variance"] = cell.CellAddress;
                                        }

                                        row++;
                                        worksheet.InsertRow(row);
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    //formula for Sub totalamt
                                    if (!noChild)
                                    {
                                        cell = worksheet.Cell(row, col + 3);
                                        cell.Value = "Total" + " " + parentName;
                                        cell.StyleID = acctHeaderStyleID;

                                        cell = worksheet.Cell(row, col + 5);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Actual"], worksheet.Cell(row - 2, col + 5).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        string actAmtAddr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 7);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Budget"], worksheet.Cell(row - 2, col + 7).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        string bgtAmtAddr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 9);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htVariance["Variance"], worksheet.Cell(row - 2, col + 9).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        switch (cellVal)
                                        {
                                            //If Production Costs
                                            case "20":
                                                {
                                                    htTotalProdCost["Actual"] = actAmtAddr;
                                                    htTotalProdCost["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 2, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 9);
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }

                                            //If Other Costs
                                            case "40":
                                                {
                                                    htTotalOtherCosts["Actual"] = actAmtAddr;
                                                    htTotalOtherCosts["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varOtherCosts = cell.CellAddress;
                                                    break;
                                                }

                                            //If Other Income
                                            case "50":
                                                {
                                                    htTotalOtherIncome["Actual"] = actAmtAddr;
                                                    htTotalOtherIncome["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varOtherIncome = cell.CellAddress;
                                                    break;
                                                }

                                            //If Taxes
                                            case "70":
                                                {
                                                    htTotalTaxes["Actual"] = actAmtAddr;
                                                    htTotalTaxes["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varTaxes = cell.CellAddress;
                                                    break;
                                                }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            else
                            {
                                //cell = worksheet.Cell(row, col + 4);
                                //cell.Value = "%";
                                //cell.StyleID = percentStyleID;

                                //for calculating NetAmt && Percentage
                                switch (cellVal)
                                {
                                    //If Gross Profit
                                    case "30":
                                        {
                                            //for Coloumn ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Actual"].ToString() + "-" + htTotalProdCost["Actual"].ToString();
                                            cell.StyleID = amtCellStyleID;
                                            varActAmt = cell.CellAddress;
                                            htTotalGrossProfit["Actual"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Actual"].ToString() + "/" + htTotalRevenue["Actual"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 5);
                                            cell.StyleID = amtStyleID;

                                            //for Coloumn BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Budget"].ToString() + "-" + htTotalProdCost["Budget"].ToString();
                                            cell.StyleID = amtCellStyleID;
                                            varbgtAmt = cell.CellAddress;

                                            htTotalGrossProfit["Budget"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Budget"].ToString() + "/" + htTotalRevenue["Budget"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 7);
                                            cell.StyleID = amtStyleID;

                                            //for Coloumn Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varActAmt + "-" + varbgtAmt;
                                            varGrossProfit = cell.CellAddress;
                                            cell.StyleID = amtCellStyleID;

                                            cell = worksheet.Cell(row, col + 9);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Variance"] == null)
                                            {
                                                cell.Formula = varGrossProfit + "/" + htTotalRevenue["Variance"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 9);
                                            cell.StyleID = amtStyleID;

                                            break;
                                        }

                                    //If NetIncomeBeforeTaxes
                                    case "60":
                                        {
                                            //for Coloumn ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Actual"].ToString() + "-" + htTotalOtherCosts["Actual"].ToString() + "+" + htTotalOtherIncome["Actual"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Actual"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Actual"].ToString() + "/" + htTotalRevenue["Actual"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for Coloumn BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Budget"].ToString() + "-" + htTotalOtherCosts["Budget"].ToString() + "+" + htTotalOtherIncome["Budget"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Budget"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Budget"].ToString() + "/" + htTotalRevenue["Budget"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for Coloumn Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varGrossProfit + "+" + varOtherCosts + "+" + varOtherIncome;
                                            cell.StyleID = boldAmtStyleID;

                                            varNetIncBefTaxes = cell.CellAddress;
                                            break;
                                        }

                                    //If NetIncome
                                    case "900":
                                        {
                                            //for Coloumn ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Actual"].ToString() + "-" + htTotalTaxes["Actual"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Actual"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 5);
                                            cell.StyleID = amtStyleID;

                                            //for Coloumn BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Budget"].ToString() + "-" + htTotalTaxes["Budget"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Budget"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 7);
                                            cell.StyleID = amtStyleID;

                                            //for Coloumn Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varNetIncBefTaxes + "+" + varTaxes;
                                            cell.StyleID = boldAmtStyleID;

                                            cell = worksheet.Cell(row - 2, col + 9);
                                            cell.StyleID = amtStyleID;
                                            break;
                                        }
                                }

                                row++;
                                worksheet.InsertRow(row);
                            }

                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //IS BY Month
        public void ReportStyle652(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtCNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string cellVal = String.Empty;
            string parentName = string.Empty;
            string keyName = string.Empty;
            string cellAddr = string.Empty;

            Hashtable htTotalAmt = new Hashtable();
            Hashtable htTotalRevenue = new Hashtable();
            Hashtable htTotalProdCost = new Hashtable();
            Hashtable htTotalGrossProfit = new Hashtable();
            Hashtable htTotalOtherCosts = new Hashtable();
            Hashtable htTotalOtherIncome = new Hashtable();
            Hashtable htTotNetIncBefTax = new Hashtable();
            Hashtable htTotalTaxes = new Hashtable();
            Hashtable htRowTotal = new Hashtable();
            Hashtable IsRevEmpty = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 6;
            int row = startRow;
            int col = 0;

            int headerStyleID = 0;
            int acctHeaderStyleID = 0;
            int monthStyleID = 0;
            int amtStyleID = 0;
            int amtCellStyleID = 0;
            int boldAmtStyleID = 0;
            int percentColStyleID = 0;

            int arrayCount = 0;
            int clmValue = 0;

            bool noChild = false;
            bool rowInserted = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\IS BY MONTH" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-652.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-652.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["IS BY MONTH"];
                        ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(4, 1).StyleID;
                        percentColStyleID = worksheet.Cell(4, 4).StyleID;
                        amtCellStyleID = worksheet.Cell(5, 6).StyleID;
                        monthStyleID = worksheet.Cell(6, 5).StyleID;
                        amtStyleID = worksheet.Cell(6, 6).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 4).StyleID = 0;
                        worksheet.Cell(5, 6).StyleID = 0;
                        worksheet.Cell(6, 6).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        headerStyleID = worksheet.Cell(1, 1).StyleID;

                        clmValue = 5;
                        DataTable dtAcctDetail = dtAllArray[arrayCount + 1];

                        //Display Coloumn Names(Month Names from DataTable Array (Account Detail DataTable))
                        foreach (DataColumn coloumn in dtAcctDetail.Columns)
                        {
                            if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "AccountName")
                            {
                                cell = worksheet.Cell(6, clmValue);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtCNameValues[arrayCount + 1][coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                //cell.Value = coloumn.ColumnName;
                                cell.StyleID = monthStyleID;
                                clmValue = clmValue + 2;
                            }
                        }

                        //Display Coloumn 'Total'
                        cell = worksheet.Cell(6, clmValue);
                        cell.Value = "Total";
                        cell.StyleID = monthStyleID;

                        row++;

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        //Starting from the First Parent
                        for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
                        {
                            htTotalAmt.Clear();
                            htRowTotal.Clear();

                            parentName = "";

                            if (row > startRow)
                            {
                                if (rowInserted)
                                {
                                    row = row + 1;
                                    worksheet.InsertRow(row);
                                }
                                else
                                {
                                    worksheet.InsertRow(row);
                                }
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];
                            cell = worksheet.Cell(row, col + 1);

                            //Display Parent
                            foreach (DataColumn coloumn in dtInitArray.Columns)
                            {
                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "").Replace(",", "");
                                    parentName = cell.Value.ToString();
                                    cellVal = drInitArrayRow["Link1"].ToString();
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }
                            //If Parent != 'Revenue'
                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            string colName = string.Empty;
                            //If Parent != GrossProfit and != NetIncomeBeforeTaxes and != NetIncome
                            if (cellVal != "30" && cellVal != "60" && cellVal != "900")
                            {
                                if (drInitArrayRow["Link1"] != null && drInitArrayRow["Link1"].ToString() != "")
                                {
                                    //Getting Data Based on Link1 from DataTableArray (AccountDetail DataTable)
                                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());
                                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                    for (int lnkArrCount = 0; lnkArrCount < drLinkedArray.Length; lnkArrCount++)
                                    {
                                        htRowTotal.Clear();

                                        //for loading Account Name 
                                        clmValue = 5;
                                        cell = worksheet.Cell(row, col + 2);
                                        if (cellVal != "10")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[lnkArrCount][arrayCount + 1].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            noChild = false;
                                        }
                                        else
                                        {
                                            noChild = true;
                                        }

                                        //for loading Month Amt
                                        for (int linkedAcctCount = 0; linkedAcctCount < dtAllArray[arrayCount + 1].Columns.Count; linkedAcctCount++)
                                        {
                                            colName = "";
                                            colName = dtAllArray[arrayCount + 1].Columns[linkedAcctCount].ColumnName.ToString();

                                            cell = worksheet.Cell(row, clmValue);
                                            if (colName != "TrxID" && colName != "Link1" && colName != "AccountName")
                                            {
                                                cell.Value = drLinkedArray[lnkArrCount][linkedAcctCount].ToString().Replace(",", "");
                                                cell.StyleID = amtCellStyleID;

                                                if (lnkArrCount == 0)
                                                {
                                                    htTotalAmt[colName] = cell.CellAddress;
                                                }

                                                //if Revenue
                                                if (cellVal == "10")
                                                {
                                                    if (cell.Value == null || cell.Value == "")
                                                    {
                                                        IsRevEmpty[colName] = "Empty";
                                                    }
                                                    htTotalRevenue[colName] = cell.CellAddress;
                                                }

                                                clmValue = clmValue + 2;

                                                if (htRowTotal["Total"] != null)
                                                {
                                                    htRowTotal["Total"] = htRowTotal["Total"] + "+" + cell.CellAddress;
                                                }
                                                else
                                                {
                                                    htRowTotal["Total"] = cell.CellAddress;
                                                }
                                            }
                                        }

                                        //formula for row level totals
                                        cell = worksheet.Cell(row, clmValue);
                                        cell.RemoveValue();
                                        cell.Formula = htRowTotal["Total"].ToString();
                                        cell.StyleID = amtCellStyleID;

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Total"] = cell.CellAddress;
                                        }

                                        //if Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                IsRevEmpty["Total"] = "Empty";
                                            }

                                            htTotalRevenue["Total"] = cell.CellAddress;
                                        }

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                    row++;
                                    worksheet.InsertRow(row);

                                    clmValue = 5;

                                    //formula for Sub totalamt
                                    if (!noChild)
                                    {
                                        cell = worksheet.Cell(row, col + 3);
                                        cell.Value = "Total" + " " + parentName;
                                        cell.StyleID = acctHeaderStyleID;

                                        for (int htlength = 0; htlength < dtAllArray[arrayCount + 1].Columns.Count; htlength++)
                                        {
                                            keyName = dtAllArray[arrayCount + 1].Columns[htlength].ColumnName;
                                            if (keyName != "TrxID" && keyName != "AccountName" && keyName != "Link1")
                                            {
                                                cell = worksheet.Cell(row, clmValue);
                                                cell.RemoveValue();
                                                cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt[keyName], worksheet.Cell(row - 2, clmValue).CellAddress);
                                                cell.StyleID = amtCellStyleID;

                                                switch (cellVal)
                                                {
                                                    //If Production Costs
                                                    case "20":
                                                        {
                                                            htTotalProdCost[keyName] = cell.CellAddress;

                                                            cell = worksheet.Cell(row - 2, clmValue);
                                                            cell.StyleID = amtStyleID;

                                                            break;
                                                        }

                                                    //If Other Costs
                                                    case "40":
                                                        {
                                                            htTotalOtherCosts[keyName] = cell.CellAddress;

                                                            cell = worksheet.Cell(row - 1, clmValue);
                                                            cell.StyleID = amtStyleID;

                                                            break;
                                                        }

                                                    //If Other Income
                                                    case "50":
                                                        {
                                                            htTotalOtherIncome[keyName] = cell.CellAddress;

                                                            cell = worksheet.Cell(row - 1, clmValue);
                                                            cell.StyleID = amtStyleID;

                                                            break;
                                                        }

                                                    //If Taxes
                                                    case "70":
                                                        {
                                                            htTotalTaxes[keyName] = cell.CellAddress;

                                                            cell = worksheet.Cell(row - 1, clmValue);
                                                            cell.StyleID = amtStyleID;

                                                            break;
                                                        }
                                                }

                                                clmValue = clmValue + 2;
                                            }
                                        }

                                        //Formula for Total(sum of all row level totals)
                                        cell = worksheet.Cell(row, clmValue);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Total"], worksheet.Cell(row - 2, clmValue).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        //applying styleID for total(sum of all child totals)
                                        switch (cellVal)
                                        {
                                            //If Production Costs
                                            case "20":
                                                {
                                                    htTotalProdCost["Total"] = cell.CellAddress;
                                                    cell = worksheet.Cell(row - 2, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }

                                            //If Other Costs
                                            case "40":
                                                {
                                                    htTotalOtherCosts["Total"] = cell.CellAddress;
                                                    cell = worksheet.Cell(row - 1, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }

                                            //If Other Income
                                            case "50":
                                                {
                                                    htTotalOtherIncome["Total"] = cell.CellAddress;
                                                    cell = worksheet.Cell(row - 1, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }

                                            //If Taxes
                                            case "70":
                                                {
                                                    htTotalTaxes["Total"] = cell.CellAddress;
                                                    cell = worksheet.Cell(row - 1, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            else
                            {
                                //for calculating percentage
                                //cell = worksheet.Cell(row, col + 4);
                                //cell.Value = "%";
                                //cell.StyleID = percentStyleID;

                                clmValue = 5;

                                for (int htlength = 0; htlength < dtAllArray[arrayCount + 1].Columns.Count; htlength++)
                                {
                                    keyName = dtAllArray[arrayCount + 1].Columns[htlength].ColumnName;

                                    if (keyName != "TrxID" && keyName != "AccountName" && keyName != "Link1")
                                    {
                                        cell = worksheet.Cell(row - 1, clmValue);
                                        cell.RemoveValue();

                                        switch (cellVal)
                                        {
                                            //If Gross Profit
                                            case "30":
                                                {
                                                    cell.Formula = htTotalRevenue[keyName].ToString() + "-" + htTotalProdCost[keyName].ToString();
                                                    cell.StyleID = amtCellStyleID;
                                                    htTotalGrossProfit[keyName] = cell.CellAddress;

                                                    cell = worksheet.Cell(row, clmValue);
                                                    cell.RemoveValue();

                                                    if (IsRevEmpty[keyName] == null)
                                                    {
                                                        cell.Formula = htTotalGrossProfit[keyName].ToString() + "/" + htTotalRevenue[keyName].ToString();
                                                    }
                                                    cell.StyleID = percentColStyleID;

                                                    cell = worksheet.Cell(row - 3, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }

                                            //If Net Income Before Taxes
                                            case "60":
                                                {
                                                    cell.Formula = htTotalGrossProfit[keyName].ToString() + "-" + htTotalOtherCosts[keyName].ToString() + "+" + htTotalOtherIncome[keyName].ToString();
                                                    cell.StyleID = boldAmtStyleID;

                                                    htTotNetIncBefTax[keyName] = cell.CellAddress;

                                                    cell = worksheet.Cell(row, clmValue);
                                                    cell.RemoveValue();

                                                    if (IsRevEmpty[keyName] == null)
                                                    {
                                                        cell.Formula = htTotNetIncBefTax[keyName].ToString() + "/" + htTotalRevenue[keyName].ToString();
                                                    }
                                                    cell.StyleID = percentColStyleID;

                                                    break;
                                                }

                                            //If Net Income
                                            case "900":
                                                {
                                                    cell.Formula = htTotNetIncBefTax[keyName].ToString() + "-" + htTotalTaxes[keyName].ToString();
                                                    cell.StyleID = boldAmtStyleID;

                                                    cellAddr = cell.CellAddress;

                                                    cell = worksheet.Cell(row, clmValue);
                                                    cell.RemoveValue();

                                                    if (IsRevEmpty[keyName] == null)
                                                    {
                                                        cell.Formula = cellAddr + "/" + htTotalRevenue[keyName].ToString();
                                                    }
                                                    cell.StyleID = percentColStyleID;

                                                    cell = worksheet.Cell(row - 2, clmValue);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }
                                        }

                                        //for the coloumn Total
                                        clmValue = clmValue + 2;
                                        cell.RemoveValue();
                                    }
                                }

                                cell = worksheet.Cell(row - 1, clmValue);
                                cell.RemoveValue();

                                //formulas for gross profit etc for total coloumn
                                switch (cellVal)
                                {
                                    //If Gross Profit
                                    case "30":
                                        {
                                            cell.Formula = htTotalRevenue["Total"].ToString() + "-" + htTotalProdCost["Total"].ToString();

                                            cell.StyleID = amtCellStyleID;
                                            htTotalGrossProfit["Total"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, clmValue);
                                            cell.RemoveValue();

                                            if (IsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Total"].ToString() + "/" + htTotalRevenue["Total"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, clmValue);
                                            cell.StyleID = amtStyleID;

                                            break;
                                        }

                                    //If Net Income Before Taxes
                                    case "60":
                                        {
                                            cell.Formula = htTotalGrossProfit["Total"].ToString() + "-" + htTotalOtherCosts["Total"].ToString() + "+" + htTotalOtherIncome["Total"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Total"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, clmValue);
                                            cell.RemoveValue();

                                            if (IsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Total"].ToString() + "/" + htTotalRevenue["Total"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            break;
                                        }

                                    //If Net Income
                                    case "900":
                                        {
                                            cell.Formula = htTotNetIncBefTax["Total"].ToString() + "-" + htTotalTaxes["Total"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, clmValue);
                                            cell.RemoveValue();

                                            if (IsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Total"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, clmValue);
                                            cell.StyleID = amtStyleID;

                                            break;
                                        }
                                }

                                row++;
                                worksheet.InsertRow(row);
                            }

                            //If Not Revenue
                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //IS BY Quarter
        public void ReportStyle653(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtCNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string cellVal = String.Empty;
            string parentName = string.Empty;
            string cellAddr = string.Empty;

            string qAmt1 = string.Empty;
            string qAmt2 = string.Empty;
            string qAmt3 = string.Empty;
            string qAmt4 = string.Empty;

            string qAmt1Addr = string.Empty;
            string qAmt2Addr = string.Empty;
            string qAmt3Addr = string.Empty;
            string qAmt4Addr = string.Empty;

            string varGrossProfit = string.Empty;
            string varOtherCosts = string.Empty;
            string varOtherIncome = string.Empty;
            string varNetIncBefTaxes = string.Empty;
            string varTaxes = string.Empty;

            Hashtable htTotalAmt = new Hashtable();
            Hashtable htTotalRevenue = new Hashtable();
            Hashtable htTotalProdCost = new Hashtable();
            Hashtable htTotalGrossProfit = new Hashtable();
            Hashtable htTotalOtherCosts = new Hashtable();
            Hashtable htTotalOtherIncome = new Hashtable();
            Hashtable htTotNetIncBefTax = new Hashtable();
            Hashtable htTotalTaxes = new Hashtable();
            Hashtable htIsRevEmpty = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 8;
            int row = startRow;
            int col = 0;
            int clmValue = 0;

            int headerStyleID = 0;
            int acctHeaderStyleID = 0;
            int monthStyleID = 0;
            int amtStyleID = 0;
            int amtCellStyleID = 0;
            int boldAmtStyleID = 0;
            int percentColStyleID = 0;

            int arrayCount = 0;

            bool noChild = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\IS BY QUARTER" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-653.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-653.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["IS BY QUARTER"];
                        ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(4, 1).StyleID;
                        percentColStyleID = worksheet.Cell(4, 4).StyleID;
                        amtStyleID = worksheet.Cell(5, 6).StyleID;
                        monthStyleID = worksheet.Cell(6, 5).StyleID;
                        amtCellStyleID = worksheet.Cell(6, 6).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 4).StyleID = 0;
                        worksheet.Cell(5, 6).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        headerStyleID = worksheet.Cell(1, 1).StyleID;

                        //Loading The date in 6th row
                        cell = worksheet.Cell(6, 5);
                        cell.Value = System.Security.SecurityElement.Escape(ArrhtCNameValues[arrayCount + 2]["EndBal"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                        clmValue = 5;

                        //Setting Coloumn Names in the Excel Sheet
                        for (int array = 2; array < dtAllArray.Length; array++)
                        {
                            DataTable dtAcctDetail = dtAllArray[arrayCount + array];

                            foreach (DataColumn coloumn in dtAcctDetail.Columns)
                            {
                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2" && coloumn.ColumnName != "AccountName")
                                {
                                    cell = worksheet.Cell(6, clmValue);
                                    cell.Value = System.Security.SecurityElement.Escape(ArrhtCNameValues[arrayCount + array][coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = monthStyleID;
                                    clmValue = clmValue + 2;

                                    break;
                                }
                            }
                        }

                        cell = worksheet.Cell(6, clmValue);
                        cell.Value = "Total";
                        cell.StyleID = monthStyleID;

                        //First Data Table (Report Detail)
                        DataTable dtInitArray = dtAllArray[arrayCount];

                        //Starting with first parent
                        for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
                        {
                            htTotalAmt.Clear();

                            parentName = "";

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];
                            cell = worksheet.Cell(row, col + 1);

                            //Display Parent
                            foreach (DataColumn coloumn in dtInitArray.Columns)
                            {
                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[coloumn.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    parentName = cell.Value.ToString();
                                    cellVal = drInitArrayRow["Link1"].ToString();
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            //If != Revenue
                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            string colName = string.Empty;

                            //If != Revenue && != NetIncomeBeforeTaxes && != NetIncome
                            if (cellVal != "30" && cellVal != "60" && cellVal != "900")
                            {
                                if (drInitArrayRow["Link1"] != null && drInitArrayRow["Link1"].ToString() != "")
                                {
                                    // Getting Data Based on Link1 from DataTable Array (2nd Data Table, Account Detail))
                                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());
                                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                    for (int lnkArrCount = 0; lnkArrCount < drLinkedArray.Length; lnkArrCount++)
                                    {
                                        //for loading Account Name 
                                        cell = worksheet.Cell(row, col + 2);

                                        //If != Revenue
                                        if (cellVal != "10")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[lnkArrCount][arrayCount + 1].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            noChild = false;
                                        }
                                        else
                                        {
                                            noChild = true;
                                        }

                                        //for loading Amount
                                        // Getting Data Based on Link2 from DataTable Array (3rd Data Table, TrxDetail1))
                                        int link2 = Convert.ToInt32(drLinkedArray[lnkArrCount]["Link2"].ToString());
                                        DataRow[] drPrimaryAmtArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 5);

                                        //for 1st Quarter
                                        if (drPrimaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drPrimaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Quarter1"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Quarter1"] = "Empty";
                                            }

                                            htTotalRevenue["Quarter1"] = cell.CellAddress;
                                        }

                                        qAmt1 = cell.CellAddress;


                                        //for 2nd Quarter
                                        // Getting Data Based on Link2 from DataTable Array (4th Data Table, TrxDetail2))
                                        DataRow[] drSecondaryAmtArray = dtAllArray[arrayCount + 3].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 7);

                                        if (drSecondaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 3].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drSecondaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Quarter2"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Quarter2"] = "Empty";
                                            }

                                            htTotalRevenue["Quarter2"] = cell.CellAddress;
                                        }

                                        qAmt2 = cell.CellAddress;

                                        //for 3rd Quarter
                                        // Getting Data Based on Link2 from DataTable Array (5th Data Table, TrxDetail3))
                                        DataRow[] drThirdAmtArray = dtAllArray[arrayCount + 3].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 9);

                                        if (drThirdAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 3].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drThirdAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Quarter3"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Quarter3"] = "Empty";
                                            }

                                            htTotalRevenue["Quarter3"] = cell.CellAddress;
                                        }

                                        qAmt3 = cell.CellAddress;

                                        //for 4th Quarter
                                        // Getting Data Based on Link2 from DataTable Array (6th Data Table, TrxDetail3))
                                        DataRow[] drFourthAmtArray = dtAllArray[arrayCount + 3].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 11);

                                        if (drFourthAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 3].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drFourthAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Quarter4"] = cell.CellAddress;
                                        }

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Quarter4"] = "Empty";
                                            }

                                            htTotalRevenue["Quarter4"] = cell.CellAddress;
                                        }

                                        qAmt4 = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 13);
                                        cell.RemoveValue();
                                        cell.Formula = qAmt1 + "+" + qAmt2 + "+" + qAmt3 + "+" + qAmt4;
                                        cell.StyleID = amtStyleID;

                                        //If Revenue
                                        if (cellVal == "10")
                                        {
                                            htTotalRevenue["Total"] = cell.CellAddress;
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Total"] = cell.CellAddress;
                                        }

                                        row++;
                                        worksheet.InsertRow(row);
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    //formula for totalamt
                                    if (!noChild)
                                    {
                                        cell = worksheet.Cell(row, col + 3);
                                        cell.Value = "Total" + " " + parentName;
                                        cell.StyleID = acctHeaderStyleID;

                                        cell = worksheet.Cell(row, col + 5);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Quarter1"], worksheet.Cell(row - 2, col + 5).CellAddress);
                                        cell.StyleID = amtStyleID;

                                        qAmt1Addr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 7);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Quarter2"], worksheet.Cell(row - 2, col + 7).CellAddress);
                                        cell.StyleID = amtStyleID;

                                        qAmt2Addr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 9);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Quarter3"], worksheet.Cell(row - 2, col + 9).CellAddress);
                                        cell.StyleID = amtStyleID;

                                        qAmt3Addr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 11);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Quarter4"], worksheet.Cell(row - 2, col + 11).CellAddress);
                                        cell.StyleID = amtStyleID;

                                        qAmt4Addr = cell.CellAddress;

                                        //for Row Total
                                        cell = worksheet.Cell(row, col + 13);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Total"], worksheet.Cell(row - 2, col + 13).CellAddress);
                                        cell.StyleID = amtStyleID;

                                        switch (cellVal)
                                        {
                                            //if Production Cost
                                            case "20":
                                                {
                                                    htTotalProdCost["Quarter1"] = qAmt1Addr;
                                                    htTotalProdCost["Quarter2"] = qAmt2Addr;
                                                    htTotalProdCost["Quarter3"] = qAmt3Addr;
                                                    htTotalProdCost["Quarter4"] = qAmt4Addr;

                                                    cell = worksheet.Cell(row - 2, col + 5);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 7);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 9);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 11);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 13);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row, col + 13);
                                                    htTotalProdCost["Total"] = cell.CellAddress;

                                                    row++;
                                                    worksheet.InsertRow(row);

                                                    break;
                                                }

                                            //if Other Cost
                                            case "40":
                                                {
                                                    htTotalOtherCosts["Quarter1"] = qAmt1Addr;
                                                    htTotalOtherCosts["Quarter2"] = qAmt2Addr;
                                                    htTotalOtherCosts["Quarter3"] = qAmt3Addr;
                                                    htTotalOtherCosts["Quarter4"] = qAmt4Addr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 11);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 13);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row, col + 13);
                                                    htTotalOtherCosts["Total"] = cell.CellAddress;
                                                    break;
                                                }

                                            //If Other Income
                                            case "50":
                                                {
                                                    htTotalOtherIncome["Quarter1"] = qAmt1Addr;
                                                    htTotalOtherIncome["Quarter2"] = qAmt2Addr;
                                                    htTotalOtherIncome["Quarter3"] = qAmt3Addr;
                                                    htTotalOtherIncome["Quarter4"] = qAmt4Addr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 11);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 13);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row, col + 13);
                                                    htTotalOtherIncome["Total"] = cell.CellAddress;
                                                    break;
                                                }

                                            //If Taxes
                                            case "70":
                                                {
                                                    htTotalTaxes["Quarter1"] = qAmt1Addr;
                                                    htTotalTaxes["Quarter2"] = qAmt2Addr;
                                                    htTotalTaxes["Quarter3"] = qAmt3Addr;
                                                    htTotalTaxes["Quarter4"] = qAmt4Addr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 11);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 13);
                                                    cell.StyleID = amtCellStyleID;

                                                    cell = worksheet.Cell(row, col + 13);
                                                    htTotalTaxes["Total"] = cell.CellAddress;

                                                    break;
                                                }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            else
                            {
                                //cell = worksheet.Cell(row, col + 4);
                                //cell.Value = "%";
                                //cell.StyleID = percentStyleID;

                                //for calculating NetAmt && Percentage
                                switch (cellVal)
                                {
                                    //If Gross Profit
                                    case "30":
                                        {
                                            //for 1st Quarter
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Quarter1"].ToString() + "-" + htTotalProdCost["Quarter1"].ToString();
                                            cell.StyleID = amtStyleID;
                                            htTotalGrossProfit["Quarter1"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter1"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Quarter1"].ToString() + "/" + htTotalRevenue["Quarter1"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 5);
                                            cell.StyleID = amtCellStyleID;

                                            //for 2nd Quarter
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Quarter2"].ToString() + "-" + htTotalProdCost["Quarter2"].ToString();
                                            cell.StyleID = amtStyleID;

                                            htTotalGrossProfit["Quarter2"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter2"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Quarter2"].ToString() + "/" + htTotalRevenue["Quarter2"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 7);
                                            cell.StyleID = amtCellStyleID;

                                            //for 3rd Quarter
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Quarter3"].ToString() + "-" + htTotalProdCost["Quarter3"].ToString();
                                            cell.StyleID = amtStyleID;

                                            htTotalGrossProfit["Quarter3"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 9);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter3"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Quarter3"].ToString() + "/" + htTotalRevenue["Quarter3"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 9);
                                            cell.StyleID = amtCellStyleID;

                                            //for 4th Quarter
                                            cell = worksheet.Cell(row - 1, col + 11);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Quarter4"].ToString() + "-" + htTotalProdCost["Quarter4"].ToString();
                                            cell.StyleID = amtStyleID;

                                            htTotalGrossProfit["Quarter4"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 11);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter4"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Quarter4"].ToString() + "/" + htTotalRevenue["Quarter4"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 11);
                                            cell.StyleID = amtCellStyleID;

                                            //for Total
                                            cell = worksheet.Cell(row - 1, col + 13);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Total"].ToString() + "-" + htTotalProdCost["Total"].ToString();
                                            htTotalGrossProfit["Total"] = cell.CellAddress;
                                            cell.StyleID = amtStyleID;

                                            cell = worksheet.Cell(row, col + 13);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Total"] + "/" + htTotalRevenue["Total"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 13);
                                            cell.StyleID = amtCellStyleID;

                                            break;
                                        }

                                    //If NetIncomeBeforeTaxes
                                    case "60":
                                        {
                                            //for 1st Quarter
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Quarter1"].ToString() + "-" + htTotalOtherCosts["Quarter1"].ToString() + "+" + htTotalOtherIncome["Quarter1"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Quarter1"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Quarter1"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Quarter1"].ToString() + "/" + htTotalRevenue["Quarter1"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for 2nd Quarter
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Quarter2"].ToString() + "-" + htTotalOtherCosts["Quarter2"].ToString() + "+" + htTotalOtherIncome["Quarter2"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Quarter2"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Quarter2"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Quarter2"].ToString() + "/" + htTotalRevenue["Quarter2"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for 3rd Quarter
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Quarter3"].ToString() + "-" + htTotalOtherCosts["Quarter3"].ToString() + "+" + htTotalOtherIncome["Quarter3"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Quarter3"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 9);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Quarter3"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Quarter3"].ToString() + "/" + htTotalRevenue["Quarter3"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for 4th Quarter
                                            cell = worksheet.Cell(row - 1, col + 11);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Quarter4"].ToString() + "-" + htTotalOtherCosts["Quarter4"].ToString() + "+" + htTotalOtherIncome["Quarter4"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Quarter4"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 11);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Quarter4"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Quarter4"].ToString() + "/" + htTotalRevenue["Quarter4"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for Total
                                            cell = worksheet.Cell(row - 1, col + 13);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Total"].ToString() + "-" + htTotalOtherCosts["Total"].ToString() + "+" + htTotalOtherIncome["Total"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Total"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 13);

                                            if (htIsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Total"] + "/" + htTotalRevenue["Total"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            break;
                                        }

                                    //If NetIncome
                                    case "900":
                                        {
                                            //for 1st Quarter
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Quarter1"].ToString() + "-" + htTotalTaxes["Quarter1"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter1"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Quarter1"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 5);
                                            cell.StyleID = amtCellStyleID;

                                            //for 2nd Quarter
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Quarter2"].ToString() + "-" + htTotalTaxes["Quarter2"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter2"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Quarter2"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 7);
                                            cell.StyleID = amtCellStyleID;

                                            //for 3rd Quarter
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Quarter3"].ToString() + "-" + htTotalTaxes["Quarter3"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 9);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter3"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Quarter3"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 9);
                                            cell.StyleID = amtCellStyleID;

                                            //for 4th Quarter
                                            cell = worksheet.Cell(row - 1, col + 11);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Quarter4"].ToString() + "-" + htTotalTaxes["Quarter4"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 11);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Quarter4"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Quarter4"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 11);
                                            cell.StyleID = amtCellStyleID;

                                            //for Total
                                            cell = worksheet.Cell(row - 1, col + 13);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Total"].ToString() + "-" + htTotalTaxes["Total"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row - 2, col + 13);
                                            cell.StyleID = amtStyleID;

                                            cell = worksheet.Cell(row, col + 13);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Total"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Total"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 13);
                                            cell.StyleID = amtCellStyleID;

                                            break;
                                        }
                                }

                                row++;
                                worksheet.InsertRow(row);
                            }

                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //IS QTR vs Last Yr
        public void ReportStyle654(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtCNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            string cellVal = String.Empty;
            string parentName = string.Empty;
            string cellAddr = string.Empty;

            string actAmt = string.Empty;
            string bgtAmt = string.Empty;

            string varActAmt = string.Empty;
            string varbgtAmt = string.Empty;

            string varGrossProfit = string.Empty;
            string varOtherCosts = string.Empty;
            string varOtherIncome = string.Empty;
            string varNetIncBefTaxes = string.Empty;
            string varTaxes = string.Empty;

            Hashtable htTotalAmt = new Hashtable();
            Hashtable htTotalRevenue = new Hashtable();
            Hashtable htTotalProdCost = new Hashtable();
            Hashtable htTotalGrossProfit = new Hashtable();
            Hashtable htTotalOtherCosts = new Hashtable();
            Hashtable htTotalOtherIncome = new Hashtable();
            Hashtable htTotNetIncBefTax = new Hashtable();
            Hashtable htTotalTaxes = new Hashtable();
            Hashtable htVariance = new Hashtable();
            Hashtable htIsRevEmpty = new Hashtable();

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 10;
            int row = startRow;
            int col = 0;

            int headerStyleID = 0;
            int acctHeaderStyleID = 0;
            int monthStyleID = 0;
            int amtStyleID = 0;
            int amtCellStyleID = 0;
            int boldAmtStyleID = 0;
            int percentColStyleID = 0;

            int arrayCount = 0;

            bool noChild = false;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\IS QTR VS last yr" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-654.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-654.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["IS QTR VS last yr"];
                        ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(4, 1).StyleID;
                        percentColStyleID = worksheet.Cell(4, 4).StyleID;
                        amtCellStyleID = worksheet.Cell(4, 6).StyleID;
                        amtStyleID = worksheet.Cell(5, 6).StyleID;
                        monthStyleID = worksheet.Cell(6, 5).StyleID;

                        worksheet.Cell(2, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(4, 4).StyleID = 0;
                        worksheet.Cell(5, 6).StyleID = 0;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        headerStyleID = worksheet.Cell(1, 1).StyleID;

                        //Loading The date in 6th row
                        cell = worksheet.Cell(6, 5);
                        cell.Value = ArrhtCNameValues[arrayCount + 2]["EndBal"].ToString();

                        cell = worksheet.Cell(6, 6);
                        cell.Value = "-";

                        cell = worksheet.Cell(6, 7);
                        cell.Value = System.Security.SecurityElement.Escape(ArrhtCNameValues[arrayCount + 3]["EndBal"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int dtarrayCount = 0; dtarrayCount < dtInitArray.Rows.Count; dtarrayCount++)
                        {
                            htTotalAmt.Clear();

                            parentName = "";

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[dtarrayCount];
                            cell = worksheet.Cell(row, col + 1);

                            foreach (DataColumn coloumn in dtInitArray.Columns)
                            {
                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1")
                                {
                                    cell.Value = drInitArrayRow[coloumn.ColumnName].ToString();
                                    parentName = cell.Value.ToString();
                                    cellVal = System.Security.SecurityElement.Escape(drInitArrayRow["Link1"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            string colName = string.Empty;
                            if (cellVal != "30" && cellVal != "60" && cellVal != "900")
                            {
                                if (drInitArrayRow["Link1"] != null && drInitArrayRow["Link1"].ToString() != "")
                                {
                                    int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());
                                    DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                    for (int lnkArrCount = 0; lnkArrCount < drLinkedArray.Length; lnkArrCount++)
                                    {
                                        //for loading Account Name 
                                        cell = worksheet.Cell(row, col + 2);
                                        if (cellVal != "10")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[lnkArrCount][arrayCount + 1].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            noChild = false;
                                        }
                                        else
                                        {
                                            noChild = true;
                                        }

                                        //for loading Amount
                                        int link2 = Convert.ToInt32(drLinkedArray[lnkArrCount]["Link2"].ToString());
                                        DataRow[] drPrimaryAmtArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 5);

                                        if (drPrimaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drPrimaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtCellStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Actual"] = cell.CellAddress;
                                        }

                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Actual"] = "Empty";
                                            }

                                            htTotalRevenue["Actual"] = cell.CellAddress;
                                        }

                                        actAmt = cell.CellAddress;

                                        DataRow[] drSecondaryAmtArray = dtAllArray[arrayCount + 3].Select("Link2 ='" + link2 + "'");

                                        cell = worksheet.Cell(row, col + 7);

                                        if (drSecondaryAmtArray.Length > 0)
                                        {
                                            foreach (DataColumn coloumn in dtAllArray[arrayCount + 3].Columns)
                                            {
                                                if (coloumn.ColumnName != "TrxID" && coloumn.ColumnName != "Link1" && coloumn.ColumnName != "Link2")
                                                {
                                                    colName = "";
                                                    colName = coloumn.ColumnName.ToString();

                                                    cell.Value = drSecondaryAmtArray[arrayCount][coloumn.ColumnName].ToString().Replace(",", "");
                                                    cell.StyleID = amtCellStyleID;
                                                    break;
                                                }
                                            }
                                        }

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Budget"] = cell.CellAddress;
                                        }

                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Budget"] = "Empty";
                                            }

                                            htTotalRevenue["Budget"] = cell.CellAddress;
                                        }

                                        bgtAmt = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 9);
                                        cell.RemoveValue();
                                        cell.Formula = actAmt + "-" + bgtAmt;
                                        cell.StyleID = amtCellStyleID;

                                        if (lnkArrCount == 0)
                                        {
                                            htTotalAmt["Variance"] = cell.CellAddress;
                                        }

                                        if (cellVal == "10")
                                        {
                                            if (cell.Value == null || cell.Value == "")
                                            {
                                                htIsRevEmpty["Variance"] = "Empty";
                                            }

                                            htTotalRevenue["Variance"] = cell.CellAddress;
                                        }

                                        htVariance["Variance"] = cell.CellAddress;

                                        row++;
                                        worksheet.InsertRow(row);
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    //formula for totalamt
                                    if (!noChild)
                                    {
                                        cell = worksheet.Cell(row, col + 3);
                                        cell.Value = "Total" + " " + parentName;
                                        cell.StyleID = acctHeaderStyleID;

                                        cell = worksheet.Cell(row, col + 5);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Actual"], worksheet.Cell(row - 2, col + 5).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        string actAmtAddr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 7);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htTotalAmt["Budget"], worksheet.Cell(row - 2, col + 7).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        string bgtAmtAddr = cell.CellAddress;

                                        cell = worksheet.Cell(row, col + 9);
                                        cell.RemoveValue();
                                        cell.Formula = string.Format("SUM({0}:{1})", htVariance["Variance"], worksheet.Cell(row - 2, col + 9).CellAddress);
                                        cell.StyleID = amtCellStyleID;

                                        switch (cellVal)
                                        {
                                            case "20":
                                                {
                                                    htTotalProdCost["Actual"] = actAmtAddr;
                                                    htTotalProdCost["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 2, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 2, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    break;
                                                }

                                            case "40":
                                                {
                                                    htTotalOtherCosts["Actual"] = actAmtAddr;
                                                    htTotalOtherCosts["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varOtherCosts = cell.CellAddress;

                                                    break;
                                                }

                                            case "50":
                                                {
                                                    htTotalOtherIncome["Actual"] = actAmtAddr;
                                                    htTotalOtherIncome["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varOtherIncome = cell.CellAddress;

                                                    break;
                                                }

                                            case "70":
                                                {
                                                    htTotalTaxes["Actual"] = actAmtAddr;
                                                    htTotalTaxes["Budget"] = bgtAmtAddr;

                                                    cell = worksheet.Cell(row - 1, col + 5);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 7);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row - 1, col + 9);
                                                    cell.StyleID = amtStyleID;

                                                    cell = worksheet.Cell(row, col + 9);
                                                    varTaxes = cell.CellAddress;

                                                    break;
                                                }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            else
                            {
                                //cell = worksheet.Cell(row, col + 4);
                                //cell.Value = "%";
                                //cell.StyleID = percentStyleID;

                                //for calculating NetAmt && Percentage
                                switch (cellVal)
                                {
                                    case "30":
                                        {
                                            //for ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Actual"].ToString() + "-" + htTotalProdCost["Actual"].ToString();
                                            cell.StyleID = amtCellStyleID;
                                            varActAmt = cell.CellAddress;
                                            htTotalGrossProfit["Actual"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Actual"].ToString() + "/" + htTotalRevenue["Actual"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 5);
                                            cell.StyleID = amtStyleID;

                                            //for BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalRevenue["Budget"].ToString() + "-" + htTotalProdCost["Budget"].ToString();
                                            cell.StyleID = amtCellStyleID;
                                            varbgtAmt = cell.CellAddress;

                                            htTotalGrossProfit["Budget"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = htTotalGrossProfit["Budget"].ToString() + "/" + htTotalRevenue["Budget"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 7);
                                            cell.StyleID = amtStyleID;

                                            //for Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varActAmt + "-" + varbgtAmt;
                                            varGrossProfit = cell.CellAddress;
                                            cell.StyleID = amtCellStyleID;

                                            cell = worksheet.Cell(row, col + 9);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Variance"] == null)
                                            {
                                                cell.Formula = varGrossProfit + "/" + htTotalRevenue["Variance"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 3, col + 9);
                                            cell.StyleID = amtStyleID;

                                            break;
                                        }

                                    case "60":
                                        {
                                            //for ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Actual"].ToString() + "-" + htTotalOtherCosts["Actual"].ToString() + "+" + htTotalOtherIncome["Actual"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Actual"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Actual"].ToString() + "/" + htTotalRevenue["Actual"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotalGrossProfit["Budget"].ToString() + "-" + htTotalOtherCosts["Budget"].ToString() + "+" + htTotalOtherIncome["Budget"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            htTotNetIncBefTax["Budget"] = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();
                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = htTotNetIncBefTax["Budget"].ToString() + "/" + htTotalRevenue["Budget"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            //for Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varGrossProfit + "+" + varOtherCosts + "+" + varOtherIncome;
                                            cell.StyleID = boldAmtStyleID;

                                            varNetIncBefTaxes = cell.CellAddress;

                                            break;
                                        }

                                    case "900":
                                        {
                                            //for ActualAmt
                                            cell = worksheet.Cell(row - 1, col + 5);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Actual"].ToString() + "-" + htTotalTaxes["Actual"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 5);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Actual"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Actual"].ToString();
                                            }
                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 5);
                                            cell.StyleID = amtStyleID;

                                            //for BudgetAmt
                                            cell = worksheet.Cell(row - 1, col + 7);
                                            cell.RemoveValue();

                                            cell.Formula = htTotNetIncBefTax["Budget"].ToString() + "-" + htTotalTaxes["Budget"].ToString();
                                            cell.StyleID = boldAmtStyleID;

                                            cellAddr = cell.CellAddress;

                                            cell = worksheet.Cell(row, col + 7);
                                            cell.RemoveValue();

                                            if (htIsRevEmpty["Budget"] == null)
                                            {
                                                cell.Formula = cellAddr + "/" + htTotalRevenue["Budget"].ToString();
                                            }

                                            cell.StyleID = percentColStyleID;

                                            cell = worksheet.Cell(row - 2, col + 7);
                                            cell.StyleID = amtStyleID;

                                            //for Variance
                                            cell = worksheet.Cell(row - 1, col + 9);
                                            cell.RemoveValue();

                                            cell.Formula = varNetIncBefTaxes + "+" + varTaxes;
                                            cell.StyleID = boldAmtStyleID;

                                            cell = worksheet.Cell(row - 2, col + 9);
                                            cell.StyleID = amtStyleID;

                                            break;
                                        }
                                }

                                row++;
                                worksheet.InsertRow(row);
                            }

                            if (cellVal != "10")
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //JobCosting-Reporting-Production Cost Report
        public void ReportStyle660(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htGrandTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htSubTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            ExcelCell cell;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            bool rowInserted = false;

            int clm = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-660.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-660.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Production Cost Report"];
                        //ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            htGrandTotal.Clear();

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }

                                    cell = worksheet.Cell(row, col + 3);

                                    foreach (DataColumn column in dtAllArray[arrayCount + 1].Columns)
                                    {
                                        if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[LinkedArrCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = acctHeaderStyleID;
                                            break;
                                        }
                                    }

                                    row++;
                                    worksheet.InsertRow(row);

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            htColValues.Clear();

                                            cell = worksheet.Cell(row, col + 4);
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount]["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    //if (column.ColumnName != "CurrentPeriod" && column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdETC" && column.ColumnName != "ProdEFC" && column.ColumnName != "ProdVariance")
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        cell.RemoveValue();

                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            cell.Value = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");

                                                            if (cell.Value == "")
                                                                cell.Value = "0";

                                                            cell.StyleID = amtStyleID;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        }

                                                        htColValues[column.ColumnName] = cell.CellAddress;
                                                    }

                                                    clm = clm + 2;

                                                    if (linkedAcctCount == 0)
                                                    {
                                                        htSubTotal[column.ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);


                                                    switch (column.ColumnName)
                                                    {
                                                        //case "CurrentPeriod":
                                                        //    {
                                                        //        if (htColValues["ProdInitCost"] != null)
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualToDate"].ToString() + "-" + htColValues["ProdInitCost"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualToDate"].ToString();
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }

                                                        //case "ProdETC":
                                                        //    {
                                                        //        if (htColValues["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = "(" + htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString() + ")" + "-" + "(" + htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString() + "-" + "(" + htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString() + ")";
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        //case "ProdEFC":
                                                        //    {
                                                        //        if (htColValues["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString();
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdEFC"].ToString();
                                                                //if (htColValues["ProdInitActual"] != null)
                                                                //{
                                                                //    cell.Formula = htColValues["ProdBudget"].ToString() + "-" + "(" + htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString() + ")";
                                                                //}
                                                                //else
                                                                //{
                                                                //    cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdActualAdj"].ToString();
                                                                //}

                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                    }

                                                    clm = clm + 2;
                                                }
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = "Total " + drLinkedArray[LinkedArrCount]["Description"].ToString();
                                        cell.StyleID = acctHeaderStyleID;

                                        //for Total
                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                string cellEndAddr = worksheet.Cell(row - 1, clm).CellAddress;

                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    cell.RemoveValue();
                                                    cell.Formula = string.Format("SUM({0}:{1})", htSubTotal[column.ColumnName], cellEndAddr);

                                                    if (htGrandTotal[column.ColumnName] != null)
                                                        htGrandTotal[column.ColumnName] = htGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                                    else
                                                        htGrandTotal[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = totAmtStyleID;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = htGrandTotal[column.ColumnName].ToString();
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        public void ReportStyle661(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            Hashtable htRowFormula = new Hashtable();
            ExcelCell cell;

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int clm = 0;

            decimal currPeriodTotal = 0;
            decimal actToDateTotal = 0;
            decimal pocToDateTotal = 0;
            decimal adjTotal = 0;
            decimal etcTotal = 0;
            decimal efcTotal = 0;
            decimal grandBudgetTotal = 0;
            decimal grandInitialACTTotal = 0;
            decimal grandInitialCost = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-661.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-661.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Production Cost Report"];
                        //ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Comment" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            //htGrandTotal.Clear();

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        currPeriodTotal = 0;
                                        actToDateTotal = 0;
                                        pocToDateTotal = 0;
                                        adjTotal = 0;
                                        etcTotal = 0;
                                        efcTotal = 0;
                                        grandBudgetTotal = 0;
                                        grandInitialACTTotal = 0;
                                        grandInitialCost = 0;

                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            clm = 8;
                                            string amount = string.Empty;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    //if (column.ColumnName != "CurrentPeriod" && column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdETC" && column.ColumnName != "ProdEFC" && column.ColumnName != "ProdVariance")
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            decimal columnValue = 0;
                                                            amount = "";
                                                            if (drLinkedAcctArray[linkedAcctCount][column.ColumnName] != null && drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString() != "")
                                                            {
                                                                amount = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");
                                                                columnValue = Convert.ToDecimal(amount);
                                                            }

                                                            switch (column.ColumnName)
                                                            {
                                                                case "CurrentPeriod":
                                                                    {
                                                                        currPeriodTotal = currPeriodTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdActualToDate":
                                                                    {
                                                                        actToDateTotal = actToDateTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdPOCommit":
                                                                    {
                                                                        pocToDateTotal = pocToDateTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdActualAdj":
                                                                    {
                                                                        adjTotal = adjTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdETC":
                                                                    {
                                                                        etcTotal = etcTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdEFC":
                                                                    {
                                                                        efcTotal = efcTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdBudget":
                                                                    {
                                                                        grandBudgetTotal = grandBudgetTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdInitActual":
                                                                    {
                                                                        grandInitialACTTotal = grandInitialACTTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdInitCost":
                                                                    {
                                                                        grandInitialCost = grandInitialCost + columnValue;
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }

                                                    clm = clm + 2;
                                                }
                                            }


                                            if (htColValues["ProdInitCost"] == null)
                                            {
                                                htColValues["ProdInitCost"] = 0;
                                            }

                                            if (htColValues["ProdInitActual"] == null)
                                            {
                                                htColValues["ProdInitActual"] = 0;
                                            }
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = drLinkedArray[LinkedArrCount]["Description"].ToString();

                                        //for Total
                                        clm = 8;
                                        htRowFormula.Clear();

                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        case "CurrentPeriod":
                                                            {
                                                                cell.Value = currPeriodTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdActualToDate":
                                                            {
                                                                cell.Value = actToDateTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdPOCommit":
                                                            {
                                                                cell.Value = pocToDateTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdActualAdj":
                                                            {
                                                                cell.Value = adjTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdETC":
                                                            {
                                                                cell.Value = etcTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdEFC":
                                                            {
                                                                cell.Value = efcTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.Value = grandBudgetTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdInitActual":
                                                            {
                                                                cell.Value = grandInitialACTTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdInitCost":
                                                            {
                                                                cell.Value = grandInitialCost.ToString();
                                                                break;
                                                            }
                                                    }

                                                    htRowFormula[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = amtStyleID;

                                                    if (LinkedArrCount == 0)
                                                        htTotal[column.ColumnName] = cell.CellAddress;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);

                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        //case "CurrentPeriod":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitCost"] != null)
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "-" + htRowFormula["ProdInitCost"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualToDate"].ToString();
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString();
                                                                break;
                                                            }

                                                        //case "ProdETC":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = "(" + htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString() + ")" + "-" + "(" + htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString() + "-" + "(" + htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        //case "ProdEFC":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString();
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();

                                                                cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdEFC"].ToString();

                                                                //if (htRowFormula["ProdInitActual"] != null)
                                                                //{
                                                                //    cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + "(" + htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString() + ")";
                                                                //}
                                                                //else
                                                                //{
                                                                //    cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdActualAdj"].ToString();
                                                                //}
                                                                break;
                                                            }
                                                    }

                                                    cell.StyleID = amtStyleID;
                                                }

                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = String.Format("SUM({0}:{1})", htTotal[column.ColumnName].ToString(), worksheet.Cell(row - 2, clm).CellAddress);
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        public void ReportStyle662(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htSubTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            Hashtable htRowFormula = new Hashtable();
            ExcelCell cell;

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int clm = 0;

            decimal currPeriodTotal = 0;
            decimal actToDateTotal = 0;
            decimal pocToDateTotal = 0;
            decimal adjTotal = 0;
            decimal etcTotal = 0;
            decimal efcTotal = 0;
            decimal grandBudgetTotal = 0;
            decimal grandInitialACTTotal = 0;
            decimal grandInitialCost = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Detailed Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-662.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-662.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Detailed Production Cost Report"];
                        //ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Comment" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            //htGrandTotal.Clear();

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        currPeriodTotal = 0;
                                        actToDateTotal = 0;
                                        pocToDateTotal = 0;
                                        adjTotal = 0;
                                        etcTotal = 0;
                                        efcTotal = 0;
                                        grandBudgetTotal = 0;
                                        grandInitialACTTotal = 0;
                                        grandInitialCost = 0;

                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            string amount = string.Empty;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    //if (column.ColumnName != "CurrentPeriod" && column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdETC" && column.ColumnName != "ProdEFC" && column.ColumnName != "ProdVariance")
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            decimal columnValue = 0;
                                                            amount = "";
                                                            if (drLinkedAcctArray[linkedAcctCount][column.ColumnName] != null && drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString() != "")
                                                            {
                                                                amount = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");
                                                                columnValue = Convert.ToDecimal(amount);
                                                            }

                                                            switch (column.ColumnName)
                                                            {
                                                                case "CurrentPeriod":
                                                                    {
                                                                        currPeriodTotal = currPeriodTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdActualToDate":
                                                                    {
                                                                        actToDateTotal = actToDateTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdPOCommit":
                                                                    {
                                                                        pocToDateTotal = pocToDateTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdActualAdj":
                                                                    {
                                                                        adjTotal = adjTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdETC":
                                                                    {
                                                                        etcTotal = etcTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdEFC":
                                                                    {
                                                                        efcTotal = efcTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdBudget":
                                                                    {
                                                                        grandBudgetTotal = grandBudgetTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdInitActual":
                                                                    {
                                                                        grandInitialACTTotal = grandInitialACTTotal + columnValue;
                                                                        break;
                                                                    }
                                                                case "ProdInitCost":
                                                                    {
                                                                        grandInitialCost = grandInitialCost + columnValue;
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = drLinkedArray[LinkedArrCount]["Description"].ToString();

                                        //for Total
                                        clm = 8;
                                        htRowFormula.Clear();

                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        case "CurrentPeriod":
                                                            {
                                                                cell.Value = currPeriodTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdActualToDate":
                                                            {
                                                                cell.Value = actToDateTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdPOCommit":
                                                            {
                                                                cell.Value = pocToDateTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdActualAdj":
                                                            {
                                                                cell.Value = adjTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdETC":
                                                            {
                                                                cell.Value = etcTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdEFC":
                                                            {
                                                                cell.Value = efcTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.Value = grandBudgetTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdInitActual":
                                                            {
                                                                cell.Value = grandInitialACTTotal.ToString();
                                                                break;
                                                            }
                                                        case "ProdInitCost":
                                                            {
                                                                cell.Value = grandInitialCost.ToString();
                                                                break;
                                                            }
                                                    }

                                                    htRowFormula[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = amtStyleID;

                                                    if (LinkedArrCount == 0)
                                                        htTotal[column.ColumnName] = cell.CellAddress;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        //case "CurrentPeriod":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitCost"] != null)
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "-" + htRowFormula["ProdInitCost"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualToDate"].ToString();
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString();
                                                                break;
                                                            }

                                                        //case "ProdETC":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = "(" + htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString() + ")" + "-" + "(" + htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString() + "-" + "(" + htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        //case "ProdEFC":
                                                        //    {
                                                        //        cell.RemoveValue();
                                                        //        if (htRowFormula["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htRowFormula["ProdActualAdj"].ToString();
                                                        //        }
                                                        //        break;
                                                        //    }

                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();

                                                                cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdEFC"].ToString();

                                                                //if (htRowFormula["ProdInitActual"] != null)
                                                                //{
                                                                //    cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + "(" + htRowFormula["ProdActualAdj"].ToString() + "+" + htRowFormula["ProdInitActual"].ToString() + ")";
                                                                //}
                                                                //else
                                                                //{
                                                                //    cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdActualAdj"].ToString();
                                                                //}
                                                                break;
                                                            }
                                                    }

                                                    cell.StyleID = amtStyleID;
                                                }

                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = String.Format("SUM({0}:{1})", htTotal[column.ColumnName].ToString(), worksheet.Cell(row - 2, clm).CellAddress);
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }


                        //reportstyle 660

                        row++;
                        worksheet.InsertRow(row);

                        row++;
                        worksheet.InsertRow(row);

                        row++;
                        worksheet.InsertRow(row);

                        row++;

                        htOverallGrandTotal.Clear();
                        dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            htGrandTotal.Clear();

                            if (row > startRow)
                            {
                                worksheet.InsertRow(row);
                            }

                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }

                                    cell = worksheet.Cell(row, col + 3);

                                    foreach (DataColumn column in dtAllArray[arrayCount + 1].Columns)
                                    {
                                        if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[LinkedArrCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = acctHeaderStyleID;
                                            break;
                                        }
                                    }

                                    htSubTotal.Clear();

                                    row++;
                                    worksheet.InsertRow(row);

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            htColValues.Clear();

                                            cell = worksheet.Cell(row, col + 4);
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount]["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    //if (column.ColumnName != "CurrentPeriod" && column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdETC" && column.ColumnName != "ProdEFC" && column.ColumnName != "ProdVariance")
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        cell.RemoveValue();

                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            cell.Value = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");

                                                            if (cell.Value == "")
                                                                cell.Value = "0";

                                                            cell.StyleID = amtStyleID;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        }

                                                        htColValues[column.ColumnName] = cell.CellAddress;
                                                    }

                                                    clm = clm + 2;

                                                    if (linkedAcctCount == 0)
                                                    {
                                                        htSubTotal[column.ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);

                                                    switch (column.ColumnName)
                                                    {
                                                        //case "CurrentPeriod":
                                                        //    {
                                                        //        cell.RemoveValue();

                                                        //        if (htColValues["ProdInitCost"] != null)
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualToDate"].ToString() + "-" + htColValues["ProdInitCost"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualToDate"].ToString();
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }

                                                        //case "ProdETC":
                                                        //    {
                                                        //        cell.RemoveValue();

                                                        //        if (htColValues["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = "(" + htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString() + ")" + "-" + "(" + htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString() + ")";
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString() + "-" + "(" + htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString() + ")";
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        //case "ProdEFC":
                                                        //    {
                                                        //        cell.RemoveValue();

                                                        //        if (htColValues["ProdInitActual"] != null)
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString();
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            cell.Formula = htColValues["ProdActualAdj"].ToString();
                                                        //        }

                                                        //        cell.StyleID = amtStyleID;
                                                        //        break;
                                                        //    }

                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();

                                                                cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdEFC"].ToString();

                                                                //if (htColValues["ProdInitActual"] != null)
                                                                //{
                                                                //    cell.Formula = htColValues["ProdBudget"].ToString() + "-" + "(" + htColValues["ProdActualAdj"].ToString() + "+" + htColValues["ProdInitActual"].ToString() + ")";
                                                                //}
                                                                //else
                                                                //{
                                                                //    cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdActualAdj"].ToString();
                                                                //}

                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                    }

                                                    clm = clm + 2;
                                                }
                                            }

                                            row++;
                                            worksheet.InsertRow(row);
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = "Total " + drLinkedArray[LinkedArrCount]["Description"].ToString();
                                        cell.StyleID = acctHeaderStyleID;

                                        //for Total
                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                string cellEndAddr = worksheet.Cell(row - 1, clm).CellAddress;

                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    if (htSubTotal[column.ColumnName] == null)
                                                    {
                                                        htSubTotal[column.ColumnName] = cellEndAddr;
                                                    }

                                                    cell.RemoveValue();
                                                    cell.Formula = string.Format("SUM({0}:{1})", htSubTotal[column.ColumnName], cellEndAddr);

                                                    if (htGrandTotal[column.ColumnName] != null)
                                                        htGrandTotal[column.ColumnName] = htGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                                    else
                                                        htGrandTotal[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = totAmtStyleID;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        worksheet.InsertRow(row);

                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = htGrandTotal[column.ColumnName].ToString();
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);

                            row++;
                            worksheet.InsertRow(row);
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }


                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        public void ReportStyle663(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htGrandTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htSubTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            ExcelCell cell;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            bool rowInserted = false;

            int clm = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-663.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-663.xlsx");
                    //FileInfo template = new FileInfo(excelDir + "\\" + "ExcelReportTemplate-" + styleNo);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Production Cost Report"];
                        //ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            htGrandTotal.Clear();

                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    cell = worksheet.Cell(row, col + 3);

                                    foreach (DataColumn column in dtAllArray[arrayCount + 1].Columns)
                                    {
                                        if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[LinkedArrCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = acctHeaderStyleID;
                                            break;
                                        }
                                    }

                                    row++;

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            htColValues.Clear();

                                            cell = worksheet.Cell(row, col + 4);
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount]["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance" && column.ColumnName != "ProdBudget")
                                                    {
                                                        cell.RemoveValue();

                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            cell.Value = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");

                                                            if (cell.Value == "")
                                                                cell.Value = "0";

                                                            cell.StyleID = amtStyleID;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        }

                                                        htColValues[column.ColumnName] = cell.CellAddress;
                                                    }

                                                    clm = clm + 2;

                                                    if (linkedAcctCount == 0)
                                                    {
                                                        htSubTotal[column.ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);

                                                    switch (column.ColumnName)
                                                    {
                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["Budget"].ToString() + "+" + htColValues["BudOverage"].ToString();
                                                                htColValues["ProdBudget"] = cell.CellAddress;
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdEFC"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }

                                                    }

                                                    clm = clm + 2;
                                                }
                                            }

                                            row++;
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = "Total " + drLinkedArray[LinkedArrCount]["Description"].ToString();
                                        cell.StyleID = acctHeaderStyleID;

                                        //for Total
                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                string cellEndAddr = worksheet.Cell(row - 1, clm).CellAddress;

                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    cell.RemoveValue();
                                                    cell.Formula = string.Format("SUM({0}:{1})", htSubTotal[column.ColumnName], cellEndAddr);

                                                    if (htGrandTotal[column.ColumnName] != null)
                                                        htGrandTotal[column.ColumnName] = htGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                                    else
                                                        htGrandTotal[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = totAmtStyleID;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        row++;
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = htGrandTotal[column.ColumnName].ToString();
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            row++;
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        public void ReportStyle664(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            Hashtable htRowFormula = new Hashtable();
            Hashtable htValue = new Hashtable();
            ExcelCell cell;

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int clm = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-664.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-664.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Production Cost Report"];
                        //ExcelCell cell;

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Comment" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (row > startRow)
                                    {
                                        worksheet.InsertRow(row);
                                    }

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        htValue.Clear();

                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            clm = 8;
                                            string amount = string.Empty;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    //if (column.ColumnName != "CurrentPeriod" && column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdETC" && column.ColumnName != "ProdEFC" && column.ColumnName != "ProdVariance")
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            decimal columnValue = 0;
                                                            amount = "";
                                                            if (drLinkedAcctArray[linkedAcctCount][column.ColumnName] != null && drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString() != "")
                                                            {
                                                                amount = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");
                                                                columnValue = Convert.ToDecimal(amount);
                                                            }

                                                            switch (column.ColumnName)
                                                            {
                                                                default:
                                                                    {
                                                                        if (htValue[column.ColumnName] == null)
                                                                        {
                                                                            htValue[column.ColumnName] = columnValue;
                                                                        }
                                                                        else
                                                                        {
                                                                            htValue[column.ColumnName] = Convert.ToDecimal(htValue[column.ColumnName].ToString()) + columnValue;
                                                                        }
                                                                        break;
                                                                    }
                                                                case "ProdTotalCost":
                                                                    {
                                                                        break;
                                                                    }
                                                                case "ProdBudget":
                                                                    {
                                                                        break;
                                                                    }
                                                                case "ProdVariance":
                                                                    {
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }

                                                    clm = clm + 2;
                                                }
                                            }

                                            if (htColValues["ProdInitCost"] == null)
                                            {
                                                htColValues["ProdInitCost"] = 0;
                                            }

                                            if (htColValues["ProdInitActual"] == null)
                                            {
                                                htColValues["ProdInitActual"] = 0;
                                            }
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = drLinkedArray[LinkedArrCount]["Description"].ToString();

                                        //for Total
                                        clm = 8;
                                        htRowFormula.Clear();

                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        default:
                                                            {
                                                                cell.Value = htValue[column.ColumnName].ToString();
                                                                break;
                                                            }
                                                        case "ProdTotalCost":
                                                            {
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                break;
                                                            }
                                                    }

                                                    htRowFormula[column.ColumnName] = cell.CellAddress;
                                                    cell.StyleID = amtStyleID;

                                                    if (LinkedArrCount == 0)
                                                        htTotal[column.ColumnName] = cell.CellAddress;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);

                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString();
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["Budget"].ToString() + "+" + htRowFormula["BudOverage"].ToString();
                                                                htRowFormula["ProdBudget"] = cell.CellAddress;
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdEFC"].ToString();
                                                                break;
                                                            }
                                                    }
                                                    cell.StyleID = amtStyleID;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        row++;
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = String.Format("SUM({0}:{1})", htTotal[column.ColumnName].ToString(), worksheet.Cell(row - 2, clm).CellAddress);
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            row++;
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }

        }

        public void ReportStyle665(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColNameValues)
        {
            DataRow drCompDetails;
            DataRow drCompDate;
            DataRow drHeader;

            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            Hashtable htOverallGrandTotal = new Hashtable();
            Hashtable htSubTotal = new Hashtable();
            Hashtable htColValues = new Hashtable();
            Hashtable htRowFormula = new Hashtable();
            Hashtable htValue = new Hashtable();
            ExcelCell cell;

            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDate = string.Empty;
            string compDetails = string.Empty;
            string divTotal = string.Empty;

            string fileName = string.Empty;
            string xlTemplate = string.Empty;
            string wrkSheet = string.Empty;

            int acctHeaderStyleID;
            int arrayCount = 0;
            int amtStyleID = 0;
            int totAmtStyleID = 0;
            int colStyleID = 0;

            const int startRow = 6;
            int row = startRow;
            int col = 0;
            int clm = 0;

            try
            {
                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Detailed Production Cost Report" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-665.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-665.xlsx");
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Detailed Production Cost Report"];

                        acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
                        totAmtStyleID = worksheet.Cell(3, 1).StyleID;
                        colStyleID = worksheet.Cell(4, 1).StyleID;
                        amtStyleID = worksheet.Cell(6, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;
                        worksheet.Cell(6, 1).StyleID = 0;

                        drHeader = dtHeader.Rows[arrayCount];

                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 6);
                        cell.Value = compName;
                        cell.StyleID = acctHeaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 6);
                            cell.Value = compDetails;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 6);
                            cell.Value = compDate;
                            cell.StyleID = acctHeaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        clm = 8;
                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Comment" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                cell.Value = System.Security.SecurityElement.Escape(ArrhtColNameValues[arrayCount + 2][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                cell.StyleID = colStyleID;
                                clm = clm + 2;
                            }
                        }

                        DataTable dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        htValue.Clear();

                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            string amount = string.Empty;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance")
                                                    {
                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            decimal columnValue = 0;
                                                            amount = "";
                                                            if (drLinkedAcctArray[linkedAcctCount][column.ColumnName] != null && drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString() != "")
                                                            {
                                                                amount = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");
                                                                columnValue = Convert.ToDecimal(amount);
                                                            }

                                                            switch (column.ColumnName)
                                                            {
                                                                default:
                                                                    {
                                                                        if (htValue[column.ColumnName] == null)
                                                                        {
                                                                            htValue[column.ColumnName] = columnValue;
                                                                        }
                                                                        else
                                                                        {
                                                                            htValue[column.ColumnName] = Convert.ToDecimal(htValue[column.ColumnName].ToString()) + columnValue;
                                                                        }
                                                                        break;
                                                                    }
                                                                case "ProdTotalCost":
                                                                    {
                                                                        break;
                                                                    }
                                                                case "ProdBudget":
                                                                    {
                                                                        break;
                                                                    }
                                                                case "ProdVariance":
                                                                    {
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = drLinkedArray[LinkedArrCount]["Description"].ToString();

                                        //for Total
                                        clm = 8;
                                        htRowFormula.Clear();

                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        default:
                                                            {
                                                                cell.Value = htValue[column.ColumnName].ToString();
                                                                break;
                                                            }
                                                        case "ProdTotalCost":
                                                            {
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                break;
                                                            }
                                                    }

                                                    htRowFormula[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = amtStyleID;

                                                    if (LinkedArrCount == 0)
                                                        htTotal[column.ColumnName] = cell.CellAddress;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    switch (column.ColumnName)
                                                    {
                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdActualToDate"].ToString() + "+" + htRowFormula["ProdPOCommit"].ToString();
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["Budget"].ToString() + "+" + htRowFormula["BudOverage"].ToString();
                                                                htRowFormula["ProdBudget"] = cell.CellAddress;
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htRowFormula["ProdBudget"].ToString() + "-" + htRowFormula["ProdEFC"].ToString();
                                                                break;
                                                            }
                                                    }

                                                    cell.StyleID = amtStyleID;
                                                }

                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        row++;
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = String.Format("SUM({0}:{1})", htTotal[column.ColumnName].ToString(), worksheet.Cell(row - 2, clm).CellAddress);
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            row++;
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }

                        //reportstyle 660
                        row++;
                        row++;
                        row++;
                        row++;

                        htOverallGrandTotal.Clear();
                        dtInitArray = dtAllArray[arrayCount];

                        for (int parentTableCount = 0; parentTableCount < dtInitArray.Rows.Count; parentTableCount++)
                        {
                            htGrandTotal.Clear();
                            DataRow drInitArrayRow = dtInitArray.Rows[parentTableCount];

                            cell = worksheet.Cell(row, col + 2);

                            foreach (DataColumn column in dtInitArray.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "NumberID")
                                {
                                    cell.Value = System.Security.SecurityElement.Escape(drInitArrayRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                    cell.StyleID = acctHeaderStyleID;
                                    break;
                                }
                            }

                            if (drInitArrayRow["Link1"].ToString() != null && drInitArrayRow["Link1"].ToString() != "")
                            {
                                row++;
                                int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

                                DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

                                for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
                                {
                                    cell = worksheet.Cell(row, col + 3);

                                    foreach (DataColumn column in dtAllArray[arrayCount + 1].Columns)
                                    {
                                        if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                        {
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedArray[LinkedArrCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                            cell.StyleID = acctHeaderStyleID;
                                            break;
                                        }
                                    }

                                    htSubTotal.Clear();

                                    row++;

                                    if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
                                    {
                                        int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
                                        DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

                                        for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
                                        {
                                            htColValues.Clear();

                                            cell = worksheet.Cell(row, col + 4);
                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount]["Account"].ToString()).Replace("amp;", "").Replace("&apos;", "");

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);
                                                    if (column.ColumnName != "ProdTotalCost" && column.ColumnName != "ProdVariance" && column.ColumnName != "ProdBudget")
                                                    {
                                                        cell.RemoveValue();

                                                        if (column.ColumnName != "Comment")
                                                        {
                                                            cell.Value = drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString().Replace(",", "");

                                                            if (cell.Value == "")
                                                                cell.Value = "0";

                                                            cell.StyleID = amtStyleID;
                                                        }
                                                        else
                                                        {
                                                            cell.Value = System.Security.SecurityElement.Escape(drLinkedAcctArray[linkedAcctCount][column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "");
                                                        }

                                                        htColValues[column.ColumnName] = cell.CellAddress;
                                                    }

                                                    clm = clm + 2;

                                                    if (linkedAcctCount == 0)
                                                    {
                                                        htSubTotal[column.ColumnName] = cell.CellAddress;
                                                    }
                                                }
                                            }

                                            clm = 8;

                                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                            {
                                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                                {
                                                    cell = worksheet.Cell(row, clm);

                                                    switch (column.ColumnName)
                                                    {
                                                        case "ProdTotalCost":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdActualToDate"].ToString() + "+" + htColValues["ProdPOCommit"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                        case "ProdBudget":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["Budget"].ToString() + "+" + htColValues["BudOverage"].ToString();
                                                                htColValues["ProdBudget"] = cell.CellAddress;
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                        case "ProdVariance":
                                                            {
                                                                cell.RemoveValue();
                                                                cell.Formula = htColValues["ProdBudget"].ToString() + "-" + htColValues["ProdEFC"].ToString();
                                                                cell.StyleID = amtStyleID;
                                                                break;
                                                            }
                                                    }

                                                    clm = clm + 2;
                                                }
                                            }

                                            row++;
                                        }

                                        cell = worksheet.Cell(row, arrayCount + 3);
                                        cell.Value = "Total " + drLinkedArray[LinkedArrCount]["Description"].ToString();
                                        cell.StyleID = acctHeaderStyleID;

                                        //for Total
                                        clm = 8;
                                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                                        {
                                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                            {
                                                string cellEndAddr = worksheet.Cell(row - 1, clm).CellAddress;

                                                cell = worksheet.Cell(row, clm);
                                                if (column.ColumnName != "Comment")
                                                {
                                                    if (htSubTotal[column.ColumnName] == null)
                                                    {
                                                        htSubTotal[column.ColumnName] = cellEndAddr;
                                                    }

                                                    cell.RemoveValue();
                                                    cell.Formula = string.Format("SUM({0}:{1})", htSubTotal[column.ColumnName], cellEndAddr);

                                                    if (htGrandTotal[column.ColumnName] != null)
                                                        htGrandTotal[column.ColumnName] = htGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                                    else
                                                        htGrandTotal[column.ColumnName] = cell.CellAddress;

                                                    cell.StyleID = totAmtStyleID;
                                                }
                                                clm = clm + 2;
                                            }
                                        }

                                        row++;
                                        row++;
                                    }
                                }
                            }
                            //for GrandTotal(Sum of all Individual Parent SubTotals)
                            cell = worksheet.Cell(row, arrayCount + 2);
                            cell.Value = "Total " + System.Security.SecurityElement.Escape(drInitArrayRow["Description"].ToString()).Replace("amp;", "").Replace("&apos;", "");
                            cell.StyleID = acctHeaderStyleID;

                            clm = 8;

                            foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    if (column.ColumnName != "Comment")
                                    {
                                        cell.RemoveValue();
                                        cell.Formula = htGrandTotal[column.ColumnName].ToString();
                                        cell.StyleID = totAmtStyleID;

                                        if (htOverallGrandTotal[column.ColumnName] != null)
                                        {
                                            htOverallGrandTotal[column.ColumnName] = htOverallGrandTotal[column.ColumnName] + "+" + cell.CellAddress;
                                        }
                                        else
                                        {
                                            htOverallGrandTotal[column.ColumnName] = cell.CellAddress;
                                        }
                                    }
                                    clm = clm + 2;
                                }
                            }

                            row++;
                            row++;
                        }

                        //Formula for Overall Grand Total
                        cell = worksheet.Cell(row, arrayCount + 2);
                        cell.Value = "Grand Total";
                        cell.StyleID = acctHeaderStyleID;

                        clm = 8;

                        foreach (DataColumn column in dtAllArray[arrayCount + 2].Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Account" && column.ColumnName != "ProdSetRef" && column.ColumnName != "Link1" && column.ColumnName != "Link2")
                            {
                                cell = worksheet.Cell(row, clm);
                                if (column.ColumnName != "Comment")
                                {
                                    cell.RemoveValue();
                                    cell.Formula = htOverallGrandTotal[column.ColumnName].ToString();
                                    cell.StyleID = totAmtStyleID;
                                }
                                clm = clm + 2;
                            }
                        }

                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        public void ReportStyle670(DataTable[] dtAllArray, DataTable dtHeader, Hashtable[] ArrhtColFormats, Hashtable[] ArrhtColNameValues)
        {
            string excelDocPath = string.Empty;
            string excelDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            Hashtable htControlTypes = new Hashtable();
            Hashtable htParentColNames = new Hashtable();
            Hashtable htAccountColNames = new Hashtable();
            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            const int startRow = 7;
            int row = startRow;
            int acctheaderStyleID = 0;
            int amtStyleID = 0;
            int centerTextStyleID = 0;
            int boldAmtStyleID = 0;
            int clm = 0;
            int col = 0;

            int arrayCount = 0;

            try
            {
                //to Load Labels and their corresponding Controls Types into hashtable
                htAccountColNames = ArrhtColNameValues[0];
                htParentColNames = ArrhtColNameValues[1];

                IDictionaryEnumerator enumColFormats = ArrhtColFormats[arrayCount].GetEnumerator();
                IDictionaryEnumerator enumColNameValues = ArrhtColNameValues[arrayCount].GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htControlTypes.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                enumColFormats.Reset();
                enumColNameValues.Reset();

                enumColFormats = ArrhtColFormats[arrayCount + 1].GetEnumerator();
                enumColNameValues = ArrhtColNameValues[arrayCount + 1].GetEnumerator();

                while (enumColNameValues.MoveNext())
                {
                    enumColFormats.Reset();

                    while (enumColFormats.MoveNext())
                    {
                        if (enumColNameValues.Key == enumColFormats.Key)
                        {
                            htControlTypes.Add(enumColNameValues.Value, enumColFormats.Value);
                            break;
                        }
                    }
                }

                excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
                excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(excelDocPath + @"\Bank Reconciliation Report " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
                }

                if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-670.xlsx"))
                {
                    FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-670.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Bank Reconciliation Report"];
                        ExcelCell cell;

                        drHeader = dtHeader.Rows[0];
                        strHeader = drHeader["Column1"].ToString();
                        string[] strRequestedBy = strHeader.Split(':');
                        cell = worksheet.Cell(2, 2);

                        if (strRequestedBy.Length > 1)
                        {
                            cell.Value = strRequestedBy[arrayCount + 1].Trim().ToString();
                        }
                        else
                        {
                            cell.Value = strRequestedBy[arrayCount].Trim().ToString();
                        }

                        acctheaderStyleID = worksheet.Cell(1, 1).StyleID;
                        amtStyleID = worksheet.Cell(3, 1).StyleID;
                        boldAmtStyleID = worksheet.Cell(3, 2).StyleID;
                        centerTextStyleID = worksheet.Cell(4, 1).StyleID;

                        worksheet.Cell(3, 1).StyleID = 0;
                        worksheet.Cell(3, 2).StyleID = 0;
                        worksheet.Cell(4, 1).StyleID = 0;

                        compName = drHeader["Column2"].ToString();
                        cell = worksheet.Cell(1, 5);
                        cell.Value = compName;
                        cell.StyleID = acctheaderStyleID;

                        if (dtHeader.Rows.Count > 1)
                        {
                            drCompDetails = dtHeader.Rows[1];
                            compDetails = drCompDetails["Column2"].ToString();
                            cell = worksheet.Cell(2, 5);
                            cell.Value = compDetails;
                            cell.StyleID = acctheaderStyleID;
                        }

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            compDate = drCompDate["Column2"].ToString();
                            cell = worksheet.Cell(3, 5);
                            cell.Value = compDate;
                            cell.StyleID = acctheaderStyleID;
                        }

                        cell = worksheet.Cell(3, 11);
                        string strReportDate = dtHeader.Rows[0]["Column3"].ToString();

                        if (strReportDate != null && strReportDate != "")
                        {
                            string[] dateArray = strReportDate.Split(' ');
                            string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                            strReportDate = dateFormat + " " + dateArray[3].ToString();
                        }

                        cell.Value = strReportDate;

                        cell = worksheet.Cell(5, col + 2);

                        DataTable dtFirstParent = dtAllArray[0];
                        DataRow dFirstParentRow = dtFirstParent.Rows[0];

                        foreach (DataColumn column in dtFirstParent.Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Link1")
                            {
                                cell.Value = htAccountColNames[column.ColumnName].ToString() + ": ";
                                cell.StyleID = acctheaderStyleID;

                                cell = worksheet.Cell(5, col + 4);

                                string cellValue = dFirstParentRow[column.ColumnName].ToString();

                                string colControlType = htAccountColNames[column.ColumnName].ToString();

                                if (cellValue != null && cellValue.Trim() != "")
                                {
                                    if (htControlTypes[colControlType] != null)
                                    {
                                        if (htControlTypes[colControlType].ToString() == "DATE")
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cellValue);
                                            cellValue = dateTime.ToString("MM/dd/yy");
                                        }
                                    }
                                }
                                cell.Value = System.Security.SecurityElement.Escape(cellValue).Replace("amp;", "").Replace("&apos;", "");
                                break;
                            }
                        }

                        for (int parentCount = 1; parentCount < dtAllArray.Length; parentCount++)
                        {
                            if (row > startRow)
                            {
                                row++;
                                worksheet.InsertRow(row);
                            }

                            clm = 2;
                            DataTable dtParent = dtAllArray[parentCount];

                            foreach (DataColumn column in dtParent.Columns)
                            {
                                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1")
                                {
                                    cell = worksheet.Cell(row, clm);
                                    cell.Value = htParentColNames[column.ColumnName].ToString();
                                    cell.StyleID = centerTextStyleID;
                                    clm = clm + 2;
                                }
                            }

                            for (int parentRowcount = 0; parentRowcount < dtParent.Rows.Count; parentRowcount++)
                            {
                                if (parentCount == 1)
                                {
                                    row++;
                                    worksheet.InsertRow(row);
                                }
                                else
                                {
                                    if (row > startRow)
                                    {
                                        row++;
                                        worksheet.InsertRow(row);
                                    }
                                }

                                DataRow dRow = dtParent.Rows[parentRowcount];

                                clm = 2;

                                foreach (DataColumn column in dtParent.Columns)
                                {
                                    if (column.ColumnName != "TrxID" && column.ColumnName != "Link1")
                                    {
                                        cell = worksheet.Cell(row, clm);
                                        cell.Value = System.Security.SecurityElement.Escape(dRow[column.ColumnName].ToString()).Replace("amp;", "").Replace("&apos;", "").Replace(",", "");

                                        string colCaption = htParentColNames[column.ColumnName].ToString();

                                        if (htControlTypes[colCaption] != null)
                                        {
                                            if (htControlTypes[colCaption].ToString() == "AMOUNT" || htControlTypes[colCaption].ToString() == "SUMMED")
                                            {
                                                cell.StyleID = amtStyleID;
                                            }
                                        }

                                        clm = clm + 2;
                                    }
                                }

                                if (parentRowcount == dtParent.Rows.Count - 1)
                                {
                                    cell = worksheet.Cell(row, col + 2);
                                    cell.Value = "Total";
                                    cell.StyleID = acctheaderStyleID;

                                    cell = worksheet.Cell(row, clm - 4);
                                    cell.StyleID = boldAmtStyleID;

                                    cell = worksheet.Cell(row, clm - 2);
                                    cell.StyleID = boldAmtStyleID;
                                }
                            }

                            row++;
                            worksheet.InsertRow(row);
                        }

                        row++;
                        worksheet.InsertRow(row);


                        foreach (DataColumn column in dtFirstParent.Columns)
                        {
                            if (column.ColumnName != "TrxID" && column.ColumnName != "Link1" && column.ColumnName != "StateDate")
                            {
                                cell = worksheet.Cell(row, col + 8);
                                cell.Value = htAccountColNames[column.ColumnName].ToString() + ": ";
                                cell.StyleID = acctheaderStyleID;

                                cell = worksheet.Cell(row, col + 10);

                                string cellValue = dFirstParentRow[column.ColumnName].ToString();

                                string colControlType = htAccountColNames[column.ColumnName].ToString();

                                if (cellValue != null && cellValue.Trim() != "")
                                {
                                    if (htControlTypes[colControlType] != null)
                                    {
                                        if (htControlTypes[colControlType].ToString() == "DATE")
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cellValue);
                                            cellValue = dateTime.ToString("MM/dd/yy");
                                        }
                                        else
                                        {
                                            if (htControlTypes[colControlType].ToString() == "AMOUNT" || htControlTypes[colControlType].ToString() == "SUMMED")
                                            {
                                                cell.StyleID = amtStyleID;
                                            }
                                        }
                                    }
                                }

                                cell.Value = System.Security.SecurityElement.Escape(cellValue).Replace("amp;", "").Replace("&apos;", "").Replace(",", "");

                                row++;
                                worksheet.InsertRow(row);
                            }
                        }
                        xlPackage.Save();
                    }
                }
                SaveToClient(newFile);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        #region 642 Commented Code
        //internal void ReportStyle642(DataTable[] dtAllArray, DataTable dtHeader)
        //{
        //    string strHeader = string.Empty;

        //    string compName = string.Empty;
        //    string compDate = string.Empty;
        //    string compDetails = string.Empty;

        //    int acctHeaderStyleID;
        //    int arrayCount = 0;

        //    DataRow drCompDetails;
        //    DataRow drCompDate;
        //    DataRow drHeader;

        //    Hashtable htTotal = new Hashtable();
        //    Hashtable htGrandTotal = new Hashtable();

        //    ExcelCell cell;

        //    const int startRow = 6;
        //    int row = startRow;
        //    int col = 0;
        //    bool rowInserted = false;

        //    try
        //    {
        //        string excelDocPath = ConfigurationManager.AppSettings["TempFilePath"];
        //        string excelDir = ConfigurationManager.AppSettings["ExcelTemplatePath"];

        //        if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
        //        {
        //            System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
        //        }

        //        FileInfo newFile = new FileInfo(excelDocPath + @"\ledger detail" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx");

        //        if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["ExcelTemplatePath"]))
        //        {
        //            System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["ExcelTemplatePath"]);
        //        }

        //        if (System.IO.File.Exists(excelDir + @"\ExcelReportTemplate-621.xlsx"))
        //        {
        //            FileInfo template = new FileInfo(excelDir + @"\ExcelReportTemplate-621.xlsx");

        //            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
        //            {
        //                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["ledger detail"];
        //                acctHeaderStyleID = worksheet.Cell(1, 1).StyleID;
        //                drHeader = dtHeader.Rows[0];

        //                compName = drHeader["Column2"].ToString();
        //                cell = worksheet.Cell(1, 5);
        //                cell.Value = compName;
        //                cell.StyleID = acctHeaderStyleID;

        //                drCompDetails = dtHeader.Rows[1];
        //                compDetails = drCompDetails["Column2"].ToString();
        //                cell = worksheet.Cell(2, 5);
        //                cell.Value = compDetails;
        //                cell.StyleID = acctHeaderStyleID;

        //                drCompDate = dtHeader.Rows[2];
        //                compDate = drCompDate["Column2"].ToString();
        //                cell = worksheet.Cell(3, 5);
        //                cell.Value = compDate;
        //                cell.StyleID = acctHeaderStyleID;

        //                DataTable dtInitArray = dtAllArray[arrayCount];

        //                for (int dtArrayCount = 0; dtArrayCount < dtInitArray.Rows.Count; dtArrayCount++)
        //                {
        //                    htGrandTotal.Clear();

        //                    DataRow drInitArrayRow = dtInitArray.Rows[dtArrayCount];

        //                    if (row > startRow)
        //                    {
        //                        if (rowInserted)
        //                        {
        //                            row = row + 1;
        //                            worksheet.InsertRow(row);
        //                        }
        //                        else
        //                        {
        //                            worksheet.InsertRow(row);
        //                        }
        //                    }

        //                    //for loading the parent
        //                    if (drInitArrayRow[1] != null && drInitArrayRow[1].ToString() != "")
        //                    {
        //                        cell = worksheet.Cell(row, col + 2);
        //                        cell.Value = drInitArrayRow[1].ToString();
        //                        cell.StyleID = acctHeaderStyleID;

        //                        row++;
        //                        worksheet.InsertRow(row);
        //                    }

        //                    if (drInitArrayRow["Link1"].ToString() != "" && drInitArrayRow["Link1"].ToString() != null)
        //                    {
        //                        row++;
        //                        int link = Convert.ToInt32(drInitArrayRow["Link1"].ToString());

        //                        DataRow[] drLinkedArray = dtAllArray[arrayCount + 1].Select("Link1 ='" + link + "'");

        //                        for (int LinkedArrCount = 0; LinkedArrCount < drLinkedArray.Length; LinkedArrCount++)
        //                        {
        //                            if (row > startRow)
        //                            {
        //                                worksheet.InsertRow(row);
        //                            }
        //                            //for loading the linked array
        //                            cell = worksheet.Cell(row, col + 3);
        //                            cell.Value = drLinkedArray[LinkedArrCount][1].ToString();
        //                            cell.StyleID = acctHeaderStyleID;

        //                            row++;
        //                            worksheet.InsertRow(row);

        //                            if (drLinkedArray[LinkedArrCount]["Link2"] != null && drLinkedArray[LinkedArrCount]["Link2"].ToString() != "")
        //                            {
        //                                int link2 = Convert.ToInt32(drLinkedArray[LinkedArrCount]["Link2"].ToString());
        //                                DataRow[] drLinkedAcctArray = dtAllArray[arrayCount + 2].Select("Link2 ='" + link2 + "'");

        //                                for (int linkedAcctCount = 0; linkedAcctCount < drLinkedAcctArray.Length; linkedAcctCount++)
        //                                {
        //                                    if (drLinkedAcctArray[linkedAcctCount]["Link3"] != null && drLinkedAcctArray[linkedAcctCount]["Link3"].ToString() != "")
        //                                    {
        //                                        //for loading acct details
        //                                        cell = worksheet.Cell(row, col + 4);
        //                                        cell.Value = drLinkedAcctArray[linkedAcctCount][1].ToString();

        //                                        int link3 = Convert.ToInt32(drLinkedAcctArray[linkedAcctCount]["Link3"].ToString());

        //                                        DataRow[] drLinkedDetailArray = dtAllArray[arrayCount + 3].Select("Link3 ='" + link3 + "'");
        //                                        cell = worksheet.Cell(row, col + 8);
        //                                        if (drLinkedDetailArray.Length > 0)
        //                                        {
        //                                            if (drLinkedDetailArray[0][1] != null && drLinkedDetailArray[0][1].ToString() != "")
        //                                            {
        //                                                //for loading amt
        //                                                cell.Value = drLinkedDetailArray[0][1].ToString().Replace(",", "");
        //                                            }
        //                                        }

        //                                        DataRow[] drTrxDetail2 = dtAllArray[arrayCount + 4].Select("Link3 ='" + link3 + "'");
        //                                        cell = worksheet.Cell(row, col + 10);
        //                                        if (drTrxDetail2.Length > 0)
        //                                        {
        //                                            if (drTrxDetail2[0][1] != null && drTrxDetail2[0][1].ToString() != "")
        //                                            {
        //                                                //for loading amt
        //                                                cell.Value = drTrxDetail2[0][1].ToString().Replace(",", "");
        //                                            }
        //                                        }

        //                                        if (linkedAcctCount == 0)
        //                                        {
        //                                            htTotal[drLinkedArray[LinkedArrCount][1].ToString()] = cell.CellAddress;
        //                                        }

        //                                        row++;
        //                                        worksheet.InsertRow(row);
        //                                    }
        //                                }

        //                                cell = worksheet.Cell(row - 1, col + 8);
        //                                string endAmtAddr = cell.CellAddress;

        //                                row++;
        //                                worksheet.InsertRow(row);

        //                                cell = worksheet.Cell(row, col + 3);
        //                                cell.Value = "Total" + " " + drLinkedArray[LinkedArrCount][1].ToString();

        //                                cell = worksheet.Cell(row, col + 8);
        //                                cell.RemoveValue();
        //                                cell.Formula = string.Format("SUM({0}:{1})", htTotal[drLinkedArray[LinkedArrCount][1].ToString()], endAmtAddr);

        //                                if (htGrandTotal[drInitArrayRow[1].ToString()] != null)
        //                                {
        //                                    htGrandTotal[drInitArrayRow[1].ToString()] = htGrandTotal[drInitArrayRow[1].ToString()] + "+" + cell.CellAddress;
        //                                }
        //                                else
        //                                {
        //                                    htGrandTotal[drInitArrayRow[1].ToString()] = cell.CellAddress;
        //                                }

        //                                row++;
        //                                worksheet.InsertRow(row);

        //                                row++;
        //                            }
        //                        }
        //                    }
        //                    worksheet.InsertRow(row);

        //                    row++;
        //                    worksheet.InsertRow(row);

        //                    cell = worksheet.Cell(row, col + 3);
        //                    cell.Value = "Total" + " " + drInitArrayRow[1].ToString();

        //                    cell = worksheet.Cell(row, col + 8);
        //                    cell.RemoveValue();
        //                    cell.Formula = htGrandTotal[drInitArrayRow[1].ToString()].ToString();

        //                    row++;
        //                    worksheet.InsertRow(row);

        //                    rowInserted = true;
        //                }
        //                xlPackage.Save();
        //            }
        //        }
        //        SaveToClient(newFile);
        //    }
        //    catch (Exception ex)
        //    { 

        //    }
        //}
        #endregion

        private void CreateDataset(DataTable[] dtAllArray)
        {
            DataSet dsMaster = new DataSet("Master");
            for (int i = 0; i < dtAllArray.Length; i++)
            {
                dsMaster.Tables.Add(dtAllArray[i]);
            }

            DataColumn dcLink1PK = dtAllArray[0].Columns["Link1"];
            DataColumn dcLink1FK = dtAllArray[1].Columns["Link1"];

            DataColumn dcLink2PK = dtAllArray[1].Columns["Link2"];
            DataColumn dcLink2FK = dtAllArray[2].Columns["Link2"];

            DataColumn dcLink3PK = dtAllArray[2].Columns["Link3"];
            DataColumn dcLink3FK = dtAllArray[3].Columns["Link3"];

            DataRelation dRelLink1 = new DataRelation("Link1", dcLink1PK, dcLink1FK);
            DataRelation dRelLink2 = new DataRelation("Link2", dcLink2PK, dcLink2FK);
            DataRelation dRelLink3 = new DataRelation("Link3", dcLink3PK, dcLink3FK);

            dsMaster.Relations.Add(dRelLink1);
            dsMaster.Relations.Add(dRelLink2);
            dsMaster.Relations.Add(dRelLink3);

            //dtAllArray[0].PrimaryKey = new DataColumn[] { dcLink1PK };
            //dtAllArray[1].PrimaryKey = new DataColumn[] { dcLink2PK };
            //dtAllArray[2].PrimaryKey = new DataColumn[] { dcLink3PK };
            //dtAllArray[3].PrimaryKey = new DataColumn[] { dcLink1PK };

            foreach (DataRow dr0 in dtAllArray[0].Rows)
            {
                //Level 0
                WriteToScreen(dr0, 0);
                DataRow[] arrDR0 = dr0.GetChildRows(dRelLink1);
                foreach (DataRow dr1 in arrDR0)
                {
                    WriteToScreen(dr1, 1);
                    //Level 1
                    DataRow[] arrDR1 = dr1.GetChildRows(dRelLink2);
                    foreach (DataRow dr2 in arrDR1)
                    {
                        WriteToScreen(dr2, 2);
                        //Level 2
                        DataRow[] arrDR2 = dr2.GetChildRows(dRelLink3);
                    }
                }
            }

        }

        private void WriteToScreen(DataRow dr0, int p)
        {
            System.Diagnostics.Debugger.Log(1, "Data", "\n");
            //Write the tabs
            for (int i = 0; i < p; i++)
            {
                System.Diagnostics.Debugger.Log(1, "Data", "\t");
            }
            foreach (string str in dr0.ItemArray)
            {
                System.Diagnostics.Debugger.Log(1, "Data", str + "   ");
            }

        }
        #endregion

        #region SaveToClient
        public void SaveToClient(FileInfo newFile)
        {
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
        #endregion
    }
}