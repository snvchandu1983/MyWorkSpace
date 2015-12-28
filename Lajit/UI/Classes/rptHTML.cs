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
    public class rptHTML
    {

        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public CommonUI commonObjUI = new CommonUI();

        #region  CreateHTMLTemplates

        public string CreateHTMLTemplate(XmlDocument htmlXML, DataTable dtGV, string reportStyle, string fileName, params string[] printType)
        {
            //#region NLog
            //logger.Info("Creating HTML Template for generating the HTML report.");
            //#endregion

            clsReportsUI objReportsUI = new clsReportsUI();
            string htmltext = string.Empty;
            string appDir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (dtGV.Rows.Count > 0)
            {
                #region VariableDeclaration
                string imgPath = string.Empty;
                string gvColName = string.Empty;
                string treeNodeName = string.Empty;
                string titleName = string.Empty;
                string Append = string.Empty;
                string remstring = string.Empty;
                string[] filePaths = new string[2];
                string createdFileNames = string.Empty;
                string totalTable = string.Empty;
                int tdper = 0;
                int tblCnt = 0;
                FileInfo logo_fp = null;
                //
                if (!Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"].ToString()))
                {
                    Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"].ToString());
                }
                titleName = htmlXML.SelectSingleNode("Root/bpeout/FormControls/PageTitle").InnerText;
                FileInfo fp = null;
                StreamWriter sw = null;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (titleName.Contains("(") || titleName.Contains(" "))
                {
                    fileName = titleName.Split('(').GetValue(0).ToString();
                    fileName = titleName.Replace(" ", "_");
                }
                else
                {
                    fileName = titleName;
                }
                if (ConfigurationManager.AppSettings["zipNormal"].ToString() != "Normal")
                {
                    fp = new FileInfo(ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\" + fileName + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".html");
                    if (!(fp.Exists))
                    {
                        FileStream fs = File.Create(ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\" + fileName + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".html");
                        fs.Close();
                    }
                    sw = new StreamWriter(ConfigurationManager.AppSettings["TempFilePath"].ToString() + "\\" + titleName + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".html");
                }
                gvColName = dtGV.Columns[0].ToString();
                DataTable dtHeader = new DataTable();
                dtHeader = objReportsUI.GetHeaderDT(titleName, "");
                if (dtGV.Columns.Count == 1)
                {
                    htmltext = "<html><head><title>" + titleName + "</title></head><body><table style='height: 95%; z-index: 0; align: center; width: 95%;border-color: Black;' border='1' cellpadding='0' cellspacing='0'><b>" + dtGV.Columns[0].ToString() + "</b></td></tr>";
                    for (int i = 0; i < dtGV.Rows.Count; i++)
                    {
                        sb.Append("<tr><td style='font-family: Verdana; font-size: 12px' align='right'>" + dtGV.Rows[i][gvColName].ToString() + "</td></tr>");
                    }
                }
                else
                {
                    tdper = Convert.ToInt32(100 / dtGV.Columns.Count);
                    System.Text.StringBuilder sbTotal = new System.Text.StringBuilder();
                    sbTotal.Append("<html>");
                    sbTotal.Append("<head><title>" + titleName + "</title></head>");
                    sbTotal.Append("<body>");
                    sbTotal.Append("<table style='height: 100%; z-index: 0; align: center; width: 100%;border-color: Black;' border='1' cellpadding='0' cellspacing='0'>");
                    sbTotal.Append("<tr style='height: 4%; width: 100%' valign='top'>");
                    sbTotal.Append("<td style='height: 5%; width: 100%'>");
                    sbTotal.Append("<table style='height: 100%; width: 100%; background-color: #D6D3D6' cellpadding='0' cellspacing='0' border='0'>");
                    sbTotal.Append("<tr style='height: 100%; width: 100%'>");
                    sbTotal.Append("<td>");
                    sbTotal.Append("<table style='height: 100%; width: 100%'>");
                    sbTotal.Append("<tr>");
                    sbTotal.Append("<td style='height: 100%; width: 30%'>");
                    sbTotal.Append("<table style='height: 100%; width: 100%'>");
                    sbTotal.Append("<tr>");
                    sbTotal.Append("<td style='width: 100%; font-family: Verdana; font-size: 13px' align='left'>" + dtHeader.Rows[0][0].ToString() + "</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("</table>");
                    sbTotal.Append("</td>");
                    if (ConfigurationManager.AppSettings["zipNormal"].ToString() != "Normal")
                    {
                        sbTotal.Append("<td style='height: 100%; width: 30%; font-family: Verdana; font-size: 16px' align='center'><b>" + dtHeader.Rows[0][1].ToString() + "</b></td>");
                        sbTotal.Append("<td style='height: 100%; width: 40%'>");
                    }
                    else
                    {
                        sbTotal.Append("<td style='height: 100%; width: 40%; font-family: Verdana; font-size: 16px' align='center'><b>" + dtHeader.Rows[0][1].ToString() + "</b></td>");
                        sbTotal.Append("<td style='height: 100%; width: 30%'>");
                    }
                    sbTotal.Append("<table style='height: 100%; width: 100%'>");
                    sbTotal.Append("<tr>");
                    if (ConfigurationManager.AppSettings["zipNormal"].ToString() != "Normal")
                    {
                        sbTotal.Append("<td style='height: 100%; width: 50%;'>");
                    }
                    else
                    {
                        sbTotal.Append("<td style='height: 100%; width: 100%;'>");
                    }
                    sbTotal.Append("<table style='height: 100%; width: 100%'>");
                    sbTotal.Append("<tr>");
                    sbTotal.Append("<td style='height: 100%; width: 100%; font-family: Verdana; font-size: 13px' align='right'>" + dtHeader.Rows[0][2].ToString() + "</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("<tr>");
                    sbTotal.Append("<td style='height: 100%; width: 100%; font-family: Verdana; font-size: 13px' align='right'>" + dtHeader.Rows[1][2].ToString() + "</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("</table>");
                    sbTotal.Append("</td>");
                    if (ConfigurationManager.AppSettings["zipNormal"].ToString() != "Normal")
                    {
                        sbTotal.Append("<td style='height: 100%; width: 50%' align='right'>");
                        sbTotal.Append("<img src='LogoImage.JPG' alt=''/>");
                        sbTotal.Append("</td>");
                    }
                    sbTotal.Append("</tr>");
                    sbTotal.Append("</table>");
                    sbTotal.Append("</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("</table>");
                    sbTotal.Append("</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("</table>");
                    sbTotal.Append("</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("<tr style='height: 3%; width: 100%' valign='top'>");
                    sbTotal.Append("<td>&nbsp;</td>");
                    sbTotal.Append("</tr>");
                    sbTotal.Append("<tr style='height: 93%; width: 100%' valign='top'>");
                    sbTotal.Append("<td>");
                    htmltext = sbTotal.ToString();
                }
                #endregion
                switch (reportStyle)
                {
                    #region REPORT STYLE 0
                    //Doesn't produce any output , just displays Msg only
                    case "0":
                        {
                            break;
                        }
                    #endregion
                    #region REPORT STYLE 2 AND 3
                    //"PARENTCHILDPIVOT": New page && Continuos page:
                    case "2":
                    case "3":
                        {
                            if (dtGV.Columns.Contains("Notes"))
                            {
                                DataTable NotesDT = new DataTable();
                                NotesDT = reportsBO.GenerateNotesDatatable(dtGV);
                                dtGV.Columns.Remove("Notes");
                            }
                            //Branch DT
                            XmlNode nodeBranches = htmlXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                            if (nodeBranches != null)
                            {
                                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                {
                                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                                    DataSet dsBranches = new DataSet("Branches");
                                    for (int i = 0; i < dtGV.Rows.Count; i++)
                                    {
                                        DataTable BranchDT = new DataTable();
                                        BranchDT = objReportsUI.GetBranchDataToPrint(htmlXML.OuterXml.ToString(), dtGV.Rows[i]["TrxID"].ToString(), branchNodeName);
                                        BranchDT.TableName = "Branch" + i.ToString();
                                        dsBranches.Tables.Add(BranchDT.Copy());
                                    }
                                    string concatString = string.Empty;
                                    for (int prnt = 0; prnt < dtGV.Rows.Count; prnt++)
                                    {
                                        if (dsBranches.Tables.Count > 0)
                                        {
                                            if (dtGV.Rows.Count > 0)
                                            {
                                                if (dtGV.Columns.Contains("TrxID"))
                                                {
                                                    dtGV.Columns.Remove("TrxID");
                                                }
                                                tdper = 0;
                                                int tblheightPer = 0;
                                                tblheightPer = Convert.ToInt32(100 / dtGV.Rows.Count);
                                                tdper = Convert.ToInt32(100 / 2);
                                                sb = new System.Text.StringBuilder();
                                                Append = string.Empty;
                                                if (dtGV.Columns.Count >= 1)
                                                {
                                                    sb.Append("<table style='height:2%; width: 100%' cellpadding='0' cellspacing='0'>");
                                                    sb.Append("<tr>");
                                                    sb.Append("<td style='height: 100%; width: 100%; font-family: Verdana; font-size: 13px;' align='left'>");
                                                    sb.Append("<table tyle='height: 100%; width: 100%; font-family: Verdana; font-size: 13px;' align='left'>");
                                                    for (int col = 0; col < dtGV.Columns.Count; col++)
                                                    {
                                                        sb.Append("<tr>");
                                                        for (int rowCnts = 0; rowCnts < dtGV.Rows.Count; rowCnts++)
                                                        {
                                                            if (dtGV.Columns[col].ToString().Contains("Date"))
                                                            {
                                                                DateTime date;
                                                                string dts = string.Empty;
                                                                if (commonObjUI.IsDate(dtGV.Rows[rowCnts][col].ToString()))
                                                                {
                                                                    //if the value is IsDate then change format MM/DD/YYYY
                                                                    DateTime.TryParse(dtGV.Rows[rowCnts][col].ToString(), out date);
                                                                    dts = date.ToString("MM/dd/yy");
                                                                    sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px;'align='left'><b>" + dtGV.Columns[col].ColumnName.ToString() + "</b></td><td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px;'align='left'><b>" + dts.ToString() + "</b></td>");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px;'align='left'><b>" + dtGV.Columns[col].ColumnName.ToString() + "</b></td><td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px;'align='left'><b>" + dtGV.Rows[rowCnts][col].ToString() + "</b></td>");
                                                            }
                                                            break;
                                                        }
                                                        sb.Append("</tr>");
                                                    }
                                                    sb.Append("</table>");
                                                    sb.Append("</td>");
                                                    sb.Append("</tr>");
                                                    sb.Append("<tr>");
                                                    remstring = remstring + sb.ToString();
                                                    int rowCnt = prnt;
                                                    sb = new System.Text.StringBuilder();
                                                    sb.Append(remstring + "<tr><td colspan='" + dtGV.Columns.Count + "'>&nbsp;</td></tr>");
                                                    sb.Append("<tr><td colspan='" + dtGV.Columns.Count + "'>");
                                                    tdper = 0;
                                                    DataTable dtNew = dsBranches.Tables[tblCnt];
                                                    if (dtNew.Columns.Count != 0)
                                                    {
                                                        tdper = Convert.ToInt32(100 / (dtNew.Columns.Count));
                                                        sb.Append("<table style='width: 100%; height: 100%; font-family: Verdana; font-size: 13px; border-color: Black;' cellpadding='0' cellspacing='1'>");
                                                        sb.Append("<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>");
                                                    }
                                                    if (dtNew.Columns.Count > 1)
                                                    {
                                                        string amt = string.Empty;
                                                        tdper = Convert.ToInt32(100 / (dtNew.Columns.Count));
                                                        foreach (XmlNode xns in htmlXML.SelectSingleNode("//" + branchNodeName + "//Columns").ChildNodes)
                                                        {
                                                            if (xns.Attributes["ControlType"].Value == "Amount")
                                                            {
                                                                amt = xns.Attributes["Caption"].Value;
                                                            }
                                                        }
                                                        for (int childCols = 0; childCols < dtNew.Columns.Count; childCols++)
                                                        {
                                                            if (dtNew.Columns[childCols].ColumnName == amt)
                                                            {
                                                                sb.Append("<td style='height: 100%; width:" + tdper + "%; font-family: Verdana; font-size: 12px;' align='right'><b>" + dtNew.Columns[childCols].ToString() + "</b></td>");
                                                            }
                                                            else
                                                            {
                                                                sb.Append("<td style='height: 100%; width:" + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'><b>" + dtNew.Columns[childCols].ToString() + "</b></td>");
                                                            }
                                                        }
                                                        sb.Append("</tr>");
                                                        string childTblString = string.Empty;
                                                        childTblString = sb.ToString();
                                                        string subStrings = string.Empty;
                                                        for (int childrwCnt = 0; childrwCnt < dtNew.Rows.Count; childrwCnt++)
                                                        {
                                                            sb = new System.Text.StringBuilder();
                                                            for (int childCols = 0; childCols < dtNew.Columns.Count; childCols++)
                                                            {
                                                                if (childCols == dtNew.Columns.Count - 1)
                                                                {
                                                                    if (dtNew.Columns[childCols].ColumnName.Contains("Date"))
                                                                    {
                                                                        DateTime date;
                                                                        string dts = string.Empty;
                                                                        if (commonObjUI.IsDate(dtNew.Rows[0][childCols].ToString()))
                                                                        {
                                                                            //if the value is IsDate then change format MM/DD/YYYY
                                                                            DateTime.TryParse(dtNew.Rows[0][childCols].ToString(), out date);
                                                                            dts = date.ToString("MM/dd/yy");
                                                                            subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dts.ToString() + "</td>" + "</tr>";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dtNew.Columns[childCols].ColumnName.Contains("Amount"))
                                                                        {
                                                                            subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='right'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";
                                                                        }
                                                                        else
                                                                        {
                                                                            subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (dtNew.Columns[childCols].ColumnName.Contains("Date"))
                                                                    {
                                                                        DateTime date;
                                                                        string dts = string.Empty;
                                                                        if (commonObjUI.IsDate(dtNew.Rows[0][childCols].ToString()))
                                                                        {
                                                                            //if the value is IsDate then change format MM/DD/YYYY
                                                                            DateTime.TryParse(dtNew.Rows[0][childCols].ToString(), out date);
                                                                            dts = date.ToString("MM/dd/yy");
                                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dts.ToString() + "</td>");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dtNew.Columns[childCols].ColumnName.Contains("Amount"))
                                                                        {
                                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='right'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>");
                                                                        }
                                                                        else
                                                                        {
                                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        childTblString = childTblString + subStrings;
                                                        concatString = childTblString + "</table></td></tr></table><br />";
                                                        remstring = string.Empty;
                                                        childTblString = string.Empty;
                                                        sb = new System.Text.StringBuilder();
                                                        Append = string.Empty;
                                                        subStrings = string.Empty;
                                                    }
                                                    totalTable = totalTable + concatString;
                                                    concatString = string.Empty;
                                                    break;
                                                }
                                            }
                                        }
                                        tblCnt++;
                                    }
                                }
                                htmltext = htmltext + totalTable + "</td></tr></table></body></html>";
                            }
                            break;
                        }
                    #endregion
                    #region REPORT STYLES 4 AND 5
                    //"PARENTCHILDNORMAL" New page && Continuous page:
                    case "4":
                    case "5":
                        {
                            string childTblString = string.Empty;
                            string subStrings = string.Empty;
                            if (dtGV.Columns.Contains("Notes"))
                            {
                                DataTable NotesDT = new DataTable();
                                NotesDT = reportsBO.GenerateNotesDatatable(dtGV);
                                dtGV.Columns.Remove("Notes");
                            }
                            //Branch DT
                            XmlNode nodeBranches = htmlXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                            if (nodeBranches != null)
                            {
                                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                {
                                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                                    DataSet dsBranches = new DataSet("Branches");
                                    for (int i = 0; i < dtGV.Rows.Count; i++)
                                    {
                                        DataTable BranchDT = new DataTable();
                                        BranchDT = objReportsUI.GetBranchDataToPrint(htmlXML.OuterXml.ToString(), dtGV.Rows[i]["TrxID"].ToString(), branchNodeName);
                                        BranchDT.TableName = "Branch" + i.ToString();
                                        dsBranches.Tables.Add(BranchDT.Copy());
                                    }
                                    string concatString = string.Empty;
                                    for (int prnt = 0; prnt < dtGV.Rows.Count; prnt++)
                                    {
                                        if (dsBranches.Tables.Count > 0)
                                        {
                                            if (dtGV.Rows.Count > 0)
                                            {
                                                if (dtGV.Columns.Contains("TrxID"))
                                                {
                                                    dtGV.Columns.Remove("TrxID");
                                                }
                                                tdper = 0;
                                                int tblheightPer = 0;
                                                tblheightPer = Convert.ToInt32(100 / dtGV.Rows.Count);
                                                tdper = Convert.ToInt32(100 / dtGV.Columns.Count);
                                                sb = new System.Text.StringBuilder();
                                                Append = string.Empty;
                                                if (dtGV.Columns.Count >= 1)
                                                {
                                                    concatString = string.Empty;
                                                    sb.Append("<table style='width: 100%' cellpadding='0' cellspacing='0'>");
                                                    sb.Append("<tr>");
                                                    for (int col = 0; col < dtGV.Columns.Count; col++)
                                                    {
                                                        sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px; background-color: #D6D3D6;'align='left'><b>" + dtGV.Columns[col].ToString() + "</b></td>");
                                                    }
                                                    sb.Append("</tr>");
                                                    sb.Append("<tr>");
                                                    int rowCnt = prnt;
                                                    for (; rowCnt < dtGV.Rows.Count; )
                                                    {
                                                        for (int col = 0; col < dtGV.Columns.Count; col++)
                                                        {
                                                            if (col == dtGV.Columns.Count - 1)
                                                            {
                                                                remstring = remstring + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtGV.Rows[rowCnt][col].ToString() + "</td>" + "</tr>";
                                                            }
                                                            else
                                                            {
                                                                sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtGV.Rows[rowCnt][col].ToString() + "</td>");
                                                            }
                                                        }
                                                        sb = new System.Text.StringBuilder();
                                                        sb.Append(remstring + "<tr><td colspan='" + dtGV.Columns.Count + "'>&nbsp;</td></tr>");
                                                        sb.Append("<tr><td colspan='" + dtGV.Columns.Count + "'>");
                                                        tdper = 0;
                                                        DataTable dtNew = dsBranches.Tables[tblCnt];
                                                        if (dtNew.Columns.Count != 0)
                                                        {
                                                            tdper = Convert.ToInt32(100 / (dtNew.Columns.Count));

                                                            sb.Append("<table style='width: 100%; height: 100%; font-family: Verdana; font-size: 13px; border-color: Black;'  cellpadding='0' cellspacing='0'>");

                                                            sb.Append("<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>");

                                                        }

                                                        if (dtNew.Columns.Count >= 1)
                                                        {

                                                            for (int childCols = 0; childCols < dtNew.Columns.Count; childCols++)
                                                            {

                                                                sb.Append("<td style='height: 100%; width:" + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'><b>" + dtNew.Columns[childCols].ToString() + "</b></td>");

                                                            }

                                                            sb.Append("</tr>");



                                                            childTblString = sb.ToString();





                                                            for (int childrwCnt = 0; childrwCnt < dtNew.Rows.Count; childrwCnt++)
                                                            {

                                                                sb = new System.Text.StringBuilder();



                                                                for (int childCols = 0; childCols < dtNew.Columns.Count; childCols++)
                                                                {

                                                                    if (childCols == dtNew.Columns.Count - 1)
                                                                    {

                                                                        if (dtNew.Columns[childCols].ColumnName.Contains("Date"))
                                                                        {

                                                                            DateTime date;

                                                                            string dts = string.Empty;

                                                                            if (commonObjUI.IsDate(dtNew.Rows[0][childCols].ToString()))
                                                                            {

                                                                                //if the value is IsDate then change format MM/DD/YYYY

                                                                                DateTime.TryParse(dtNew.Rows[0][childCols].ToString(), out date);

                                                                                dts = date.ToString("MM/dd/yy");

                                                                                subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dts.ToString() + "</td>" + "</tr>";

                                                                            }

                                                                        }

                                                                        else
                                                                        {

                                                                            subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                                        }

                                                                    }

                                                                    else
                                                                    {

                                                                        if (dtNew.Columns[childCols].ColumnName.Contains("Date"))
                                                                        {

                                                                            DateTime date;

                                                                            string dts = string.Empty;

                                                                            if (commonObjUI.IsDate(dtNew.Rows[0][childCols].ToString()))
                                                                            {

                                                                                //if the value is IsDate then change format MM/DD/YYYY

                                                                                DateTime.TryParse(dtNew.Rows[0][childCols].ToString(), out date);

                                                                                dts = date.ToString("MM/dd/yy");

                                                                                sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dts.ToString() + "</td>");

                                                                            }

                                                                        }

                                                                        else
                                                                        {

                                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dtNew.Rows[childrwCnt][childCols].ToString() + "</td>");

                                                                        }

                                                                    }

                                                                }

                                                            }

                                                        }

                                                        if (dtNew.Columns.Count >= 1)
                                                        {

                                                            childTblString = childTblString + subStrings;

                                                            concatString = childTblString + "</table></td></tr></table><br />";

                                                            remstring = string.Empty;

                                                            childTblString = string.Empty;

                                                            sb = new System.Text.StringBuilder();

                                                            Append = string.Empty;

                                                            subStrings = string.Empty;

                                                            totalTable = totalTable + concatString;

                                                            concatString = string.Empty;

                                                            sb = new StringBuilder();

                                                            break;

                                                        }

                                                        else
                                                        {

                                                            concatString = concatString + sb.ToString() + "</table></td></tr></table><br />";

                                                            sb = new StringBuilder();

                                                            break;

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                        tblCnt++;

                                    }

                                    totalTable = totalTable + concatString;

                                    concatString = string.Empty;

                                }

                                htmltext = htmltext + totalTable + "</td></tr></table></body></html>";

                            }

                            break;

                        }

                    #endregion
                    #region REPORT STYLE 100

                    //"CHECKS - PARENTCHILDNORMAL" New page:

                    case "100":
                        {

                            sb = new System.Text.StringBuilder();

                            htmltext = string.Empty;



                            string checkamtHTML = string.Empty;

                            string paytoHTML = string.Empty;

                            string vendorHTML = string.Empty;

                            string chqdateHTML = string.Empty;

                            string chqnoHTML = string.Empty;

                            string writtenamtHTML = string.Empty;

                            string sendtoHTML = string.Empty;

                            string invnoHTML = string.Empty;

                            string descHTML = string.Empty;

                            string invdtHTML = string.Empty;

                            string invamtHTML = string.Empty;

                            string parentTrxID = string.Empty;

                            string concatString = string.Empty;



                            if (dtGV.Rows.Count > 0)
                            {

                                parentTrxID = dtGV.Rows[0]["TrxID"].ToString();

                                if (dtGV.Columns.Contains("TrxID"))
                                {

                                    dtGV.Columns.Remove("TrxID");

                                }

                                foreach (DataColumn dcol in dtGV.Columns)
                                {

                                    switch (dcol.ColumnName.Trim())
                                    {

                                        case "Check Amount":
                                            {

                                                checkamtHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "(Pay To) Name":
                                            {

                                                paytoHTML = "<td colspan='3' style='width: 33%; height: 4%; font-family: Verdana; font-size: 14px;font-weight: bold' align='right' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td>";

                                                break;

                                            }

                                        case "Vendor":
                                            {

                                                vendorHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td>";

                                                break;

                                            }

                                        case "Check Date":
                                            {

                                                DateTime date;

                                                string dts = string.Empty;

                                                if (commonObjUI.IsDate(dtGV.Rows[0][dcol].ToString()))
                                                {

                                                    //if the value is IsDate then change format MM/DD/YYYY

                                                    DateTime.TryParse(dtGV.Rows[0][dcol].ToString(), out date);

                                                    dts = date.ToString("MM/dd/yy");

                                                }

                                                chqdateHTML = "<td colspan='3' style='width: 33%; height: 4%; font-family: Verdana; font-size: 14px;font-weight: bold' align='left' valign='top'>" + dts + "</td>";

                                                break;

                                            }

                                        case "Check Number":
                                            {

                                                chqnoHTML = "<td colspan='3' style='width: 33%; height: 4%; font-family: Verdana; font-size: 14px;font-weight: bold' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td>";

                                                break;

                                            }

                                        case "Written Amount":
                                            {

                                                writtenamtHTML = "<td colspan='3' style='width: 33%; height: 4%; font-family: Verdana; font-size: 14px;font-weight: bold' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td>";

                                                break;

                                            }

                                        case "Sent To:":
                                            {

                                                string[] strarr = dtGV.Rows[0][dcol].ToString().Split('~');

                                                for (int arrLgh = 0; arrLgh < strarr.Length; arrLgh++)
                                                {

                                                    sendtoHTML = sendtoHTML + "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' valign='top' align='left'>" + strarr[arrLgh].ToString() + "</td></tr>";

                                                }

                                                break;

                                            }

                                        default:
                                            {

                                                break;

                                            }

                                    }

                                }

                            }

                            //Printing BranchDT

                            XmlNode nodeBranches = htmlXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                            if (nodeBranches != null)
                            {

                                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                {
                                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                                    DataTable BranchDT = new DataTable();
                                    BranchDT = objReportsUI.GetBranchDataToPrint(htmlXML.OuterXml.ToString(), parentTrxID, branchNodeName);
                                    if (BranchDT.Rows.Count > 0)
                                    {
                                        bool sumExists = false;
                                        //Getting the columns to be displayed in grid
                                        XmlNode nodeCols = htmlXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
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



                                        if (BranchDT.Columns.Count != 0)
                                        {

                                            tdper = Convert.ToInt32(100 / (BranchDT.Columns.Count));

                                            sb.Append("<table style='width: 100%; height: 100%; font-family: Verdana; font-size: 13px; border-color: Black;' border='1' cellpadding='0' cellspacing='0'>");

                                            //sb.Append("<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>");

                                            sb.Append("<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>");

                                        }

                                        if (BranchDT.Columns.Count > 1)
                                        {

                                            for (int childCols = 0; childCols < BranchDT.Columns.Count; childCols++)
                                            {

                                                sb.Append("<td style='height: 100%; width:" + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'><b>" + BranchDT.Columns[childCols].ToString() + "</b></td>");

                                            }

                                            sb.Append("</tr>");

                                            string childTblString = string.Empty;

                                            childTblString = sb.ToString();



                                            string subStrings = string.Empty;

                                            for (int childrwCnt = 0; childrwCnt < BranchDT.Rows.Count; childrwCnt++)
                                            {

                                                sb = new System.Text.StringBuilder();

                                                for (int childCols = 0; childCols < BranchDT.Columns.Count; childCols++)
                                                {

                                                    if (BranchDT.Columns[childCols].ColumnName.Contains("Date"))
                                                    {

                                                        if (childCols == BranchDT.Columns.Count - 1)
                                                        {

                                                            DateTime date;

                                                            string dts = string.Empty;

                                                            if (commonObjUI.IsDate(dtGV.Rows[0][childCols].ToString()))
                                                            {

                                                                //if the value is IsDate then change format MM/DD/YYYY

                                                                DateTime.TryParse(dtGV.Rows[0][childCols].ToString(), out date);

                                                                dts = date.ToString("MM/dd/yy");

                                                                //subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                                subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                            }

                                                        }

                                                        else
                                                        {

                                                            DateTime date;

                                                            string dts = string.Empty;

                                                            if (commonObjUI.IsDate(dtGV.Rows[0][childCols].ToString()))
                                                            {

                                                                //if the value is IsDate then change format MM/DD/YYYY

                                                                DateTime.TryParse(dtGV.Rows[0][childCols].ToString(), out date);

                                                                dts = date.ToString("MM/dd/yy");

                                                            }

                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dts.ToString() + "</td>");

                                                        }

                                                    }

                                                    else
                                                    {

                                                        if (childCols == BranchDT.Columns.Count - 1)
                                                        {

                                                            DateTime date;

                                                            string dts = string.Empty;

                                                            if (commonObjUI.IsDate(dtGV.Rows[0][childCols].ToString()))
                                                            {

                                                                //if the value is IsDate then change format MM/DD/YYYY

                                                                DateTime.TryParse(dtGV.Rows[0][childCols].ToString(), out date);

                                                                dts = date.ToString("MM/dd/yy");

                                                                //subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                                subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                            }

                                                            else
                                                            {

                                                                //subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                                subStrings = subStrings + "<tr style='width: 100%; font-family: Verdana; font-size: 12px;'>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>" + "</tr>";

                                                            }

                                                        }

                                                        else
                                                        {

                                                            //sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>");

                                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + BranchDT.Rows[childrwCnt][childCols].ToString() + "</td>");

                                                        }

                                                    }

                                                }

                                            }

                                            childTblString = childTblString + subStrings;

                                            concatString = childTblString + "</table>";

                                            remstring = string.Empty;

                                            childTblString = string.Empty;

                                            sb = new System.Text.StringBuilder();

                                            Append = string.Empty;

                                            subStrings = string.Empty;

                                        }

                                    }

                                }

                            }

                            int colspan = dtGV.Columns.Count;

                            sb.Append("<html>");

                            sb.Append("<head>");

                            sb.Append("<title>" + titleName + "</title>");

                            sb.Append("</head>");

                            sb.Append("<body>");

                            //sb.Append("<table border='0' style='height: 100%; width: 100%' cellpadding='0' cellspacing='0'>");

                            //sb.Append("<table style='height: 100%; width: 100%; border-right-width: 1px; border-right-style: solid;border-right-color: #d7e0f1; border-left-width: 1px; border-left-style: solid;border-left-color: #d7e0f1; border-top-width: 1px; border-top-style: solid; border-top-color: #d7e0f1;border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #d7e0f1;' cellpadding='0' cellspacing='0'>");

                            sb.Append("<table style='height: 100%; z-index: 0; align: center; width: 100%;border-color: Black;' border='1' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td colspan='4' style='width: 100%; height: 100%' align='center' valign='top'>");

                            sb.Append("<table style='height: 100%; width: 100%; border: 0' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr style='height: 7%'>");

                            sb.Append("<td>");

                            sb.Append("&nbsp;");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 5%'>");

                            sb.Append("<td colspan=" + colspan + ">");

                            sb.Append(concatString);

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 28%'>");

                            sb.Append("<td colspan='4'>");

                            sb.Append("&nbsp;");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 15%; width: 100%'>");

                            sb.Append("<td colspan='4'>");

                            sb.Append("<table border='0' style='height: 100%; width: 100%' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr>");

                            sb.Append(paytoHTML);

                            sb.Append(chqdateHTML);

                            sb.Append("<td colspan='3'>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 15%; width: 100%'>");

                            sb.Append("<td colspan='4'>");

                            sb.Append("<table border='0' style='height: 100%; width: 100%' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td colspan='3' style='width: 33%; height: 4%'>");

                            sb.Append("</td>");

                            sb.Append("<td colspan='3' style='width: 33%; height: 4%'>");

                            sb.Append("</td>");

                            sb.Append(chqnoHTML);

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 35%; width: 100%'>");

                            sb.Append("<td colspan='2'>");

                            sb.Append("<table border='0' style='height: 100%; width: 100%' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td colspan='2' style='width: 50%;' valign='top'>");

                            sb.Append("<table border='0' style='height: 100%; width: 100%' cellpadding='0' cellspacing='0'>");

                            sb.Append(sendtoHTML);

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("<td colspan='2' style='width: 50%;'>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</body>");

                            sb.Append("</html>");

                            htmltext = sb.ToString();

                            break;

                        }

                    #endregion
                    #region REPORT STYLE 200
                    //"ARInvoice - PARENTCHILDNORMAL" New page:
                    case "200":
                        {

                            sb = new System.Text.StringBuilder();

                            htmltext = string.Empty;



                            string imageHTML = string.Empty;

                            string cusomerHTML = string.Empty;

                            string addressHTML = string.Empty;

                            string attentionHTML = string.Empty;

                            string invoicedateHTML = string.Empty;

                            string invoicenumberHTML = string.Empty;

                            string ourjobHTML = string.Empty;

                            string agencynoHTML = string.Empty;

                            string agencypoHTML = string.Empty;

                            string titleHTML = string.Empty;

                            string invoiceamountHTML = string.Empty;

                            string descriptionHTML = string.Empty;

                            string invoiceinstrHTML = string.Empty;

                            string parentTrxID = string.Empty;

                            string descHTML = string.Empty;



                            if (dtGV.Rows.Count > 0)
                            {

                                parentTrxID = dtGV.Rows[0]["TrxID"].ToString();



                                foreach (DataColumn dcol in dtGV.Columns)
                                {

                                    switch (dcol.ColumnName.Trim())
                                    {

                                        case "Image":
                                            {

                                                string imgSrc = "lajit - logos.JPG";// dtGV.Rows[0][dcol].ToString();                                   

                                                string imgpath = appDir + "App_Themes\\" + HttpContext.Current.Session["MyTheme"].ToString() + "\\Images \\lajit-logos.JPG";// +imgSrc;

                                                break;

                                            }

                                        case "Customer":
                                            {

                                                cusomerHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Address":
                                            {

                                                string[] strarr = dtGV.Rows[0][dcol].ToString().Split('~');

                                                for (int arrLgh = 0; arrLgh < strarr.Length; arrLgh++)
                                                {

                                                    addressHTML = addressHTML + "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' valign='top' align='left'>" + strarr[arrLgh].ToString() + "</td></tr>";

                                                }

                                                break;

                                            }

                                        case "Attention":
                                            {

                                                attentionHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='right' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Invoice Date":
                                            {

                                                invoicedateHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='left' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Invoice Number":
                                            {

                                                invoicenumberHTML = "Invoice No :     " + dtGV.Rows[0][dcol].ToString();

                                                break;

                                            }

                                        case "Our Job#":
                                            {

                                                ourjobHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='left' valign='top'>" + dcol.ColumnName.Trim() + " :     " + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Agency No:":
                                            {

                                                agencynoHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='left' valign='top'>" + dcol.ColumnName.Trim() + " :     " + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Agency PO#:":
                                            {

                                                agencypoHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='left' valign='top'>" + dcol.ColumnName.Trim() + " :     " + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        case "Title":
                                            {

                                                titleHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='right' valign='top'>" + dtGV.Rows[0][dcol].ToString() + "</td></tr>";

                                                break;

                                            }

                                        default:
                                            {

                                                break;

                                            }

                                    }

                                }

                            }



                            //Printing BranchDT

                            XmlNode nodeBranches = htmlXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                            if (nodeBranches != null)
                            {

                                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                                {
                                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                                    DataTable BranchDT = new DataTable();
                                    BranchDT = objReportsUI.GetBranchDataToPrint(htmlXML.OuterXml.ToString(), parentTrxID, branchNodeName);
                                    if (BranchDT.Rows.Count > 0)
                                    {
                                        bool sumExists = false;
                                        //Getting the columns to be displayed in grid
                                        XmlNode nodeCols = htmlXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
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

                                            foreach (DataColumn dcol in BranchDT.Columns)
                                            {

                                                switch (dcol.ColumnName.Trim())
                                                {

                                                    case "Description":
                                                        {

                                                            descriptionHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left'>" + dBranchRow[dcol].ToString() + ":" + "</td></tr>";

                                                            descHTML = descriptionHTML;

                                                            break;

                                                        }

                                                    case "Invoice Instructions":
                                                        {

                                                            string noTrs = string.Empty;

                                                            string invHTML = string.Empty;

                                                            if (descriptionHTML != string.Empty)
                                                            {

                                                                if (descriptionHTML.Contains("<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left'>"))
                                                                {

                                                                    descriptionHTML = descriptionHTML.Replace("<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 14px;font-weight:bold' align='left'>", "");

                                                                }

                                                                if (descriptionHTML.Contains("</td></tr>"))
                                                                {

                                                                    descriptionHTML = descriptionHTML.Replace("</td></tr>", "");

                                                                }

                                                                invoiceinstrHTML = dBranchRow[dcol].ToString();

                                                                noTrs = invoiceinstrHTML.Length.ToString();

                                                                if (Convert.ToInt32(noTrs) > descriptionHTML.Length)
                                                                {

                                                                    int cnt = Convert.ToInt32(noTrs) / (descriptionHTML.Length);



                                                                    for (int trds = 0; trds < cnt; trds++)
                                                                    {

                                                                        invHTML = invHTML + "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='left' valign='top'>" + invoiceinstrHTML.Substring(0, descriptionHTML.Length) + "</td></tr>";

                                                                    }

                                                                }

                                                            }

                                                            invoiceinstrHTML = string.Empty;

                                                            invoiceinstrHTML = invHTML;

                                                            break;

                                                        }

                                                    case "Invoice Amount":
                                                        {

                                                            invoiceamountHTML = "<tr style='height: 4px'><td style='width: 100%; font-family: Verdana; font-size: 12px' align='middle' valign='top'>$" + dBranchRow[dcol].ToString() + "</td></tr>";

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

                            sb.Append("<html>");

                            sb.Append("<head>");

                            sb.Append("<title>" + titleName + "</title>");

                            sb.Append("</head>");

                            sb.Append("<body>");

                            //sb.Append("<table style='height: 100%; width: 100%; border-right-width: 1px; border-right-style: solid;border-right-color: #d7e0f1; border-left-width: 1px; border-left-style: solid;border-left-color: #d7e0f1; border-top-width: 1px; border-top-style: solid; border-top-color: #d7e0f1;border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #d7e0f1;' cellpadding='0' cellspacing='0'>");

                            sb.Append("<table style='height: 100%; z-index: 0; align: center; width: 100%;border-color: Black;' border='1' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr style='height: 5%; width: 100%'>");

                            sb.Append("<td colspan='5' valign='top'>&nbsp;</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 15%; width: 100%'>");

                            sb.Append("<td valign='top' colspan='5'>");

                            sb.Append("<table border='0' style='height: 100%; width: 100%'>");

                            sb.Append("<tr>");

                            sb.Append("<td style='height: 100%; width: 40%'>");

                            sb.Append("<table style='height: 100%; width: 90%' border='0'>");

                            sb.Append(cusomerHTML);

                            sb.Append(addressHTML);

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("<td style='height: 100%; width: 20%'>&nbsp;</td>");

                            sb.Append("<td style='height: 100%; width: 40%' valign='top'>");

                            sb.Append("<table style='height: 100%; width: 90%' border='0'>");

                            sb.Append(invoicedateHTML.ToString());

                            sb.Append(ourjobHTML.ToString());

                            sb.Append(agencynoHTML.ToString());

                            sb.Append(agencypoHTML.ToString());

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 5%; width: 100%'>");

                            sb.Append("<td colspan='5'><hr style='color: Black; line-height: normal; vertical-align: middle; width: 100%;align: center' /></td>");

                            sb.Append("</tr>");

                            sb.Append("<tr style='height: 35%; width: 100%'>");

                            sb.Append("<td>");

                            sb.Append("<table style='height: 100%; width: 100%' border='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td style='height: 100%; width: 100%'>");

                            sb.Append("<table style='height: 100%; width: 100%'>");

                            sb.Append("<tr>");

                            sb.Append("<td style='height: 100%; width: 50%' colspan='3'>");

                            sb.Append("<table style='height: 100%; width: 100%' border='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td style='height: 100%; width: 35%'>");

                            sb.Append("</td>");

                            sb.Append("<td style='height: 100%; width: 65%'>");

                            sb.Append("<table style='height: 50%; width: 90%' border='0'>");

                            sb.Append(descHTML);

                            sb.Append(invoiceinstrHTML);

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("<td style='height: 100%; width: 50%' colspan='3'>");

                            sb.Append("<table style='height: 100%; width: 100%' border='0'>");

                            sb.Append("<tr>");

                            sb.Append("<td style='height: 100%; width: 35%'>");

                            sb.Append("</td>");

                            sb.Append("<td style='height: 100%; width: 65%' colspan='3'>");

                            sb.Append("<table style='height: 50%; width: 90%' border='0'>");

                            sb.Append(invoiceamountHTML);

                            sb.Append("<tr>");

                            sb.Append("<td valign='bottom' align='center'>");

                            sb.Append("<hr style='color: Black; line-height: normal; vertical-align: middle; width: 30%;align: center' />");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr>");

                            sb.Append("<td valign='bottom' align='center'>");

                            sb.Append(invoiceamountHTML);

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</td>");

                            sb.Append("</tr>");

                            sb.Append("</table>");

                            sb.Append("</body>");

                            sb.Append("</html>");

                            htmltext = sb.ToString();

                            break;

                        }

                    #endregion
                    #region REPORT STYLE 1 AND DEFAULT
                    //PARENT
                    case "1":
                    default:
                        {
                            if (dtGV.Columns.Count > 1)
                            {
                                if (dtGV.Columns.Contains("TrxID"))
                                {
                                    dtGV.Columns.Remove("TrxID");
                                }
                                sb = new System.Text.StringBuilder();
                                //htmltext = "<html><head><title>" + titleName + "</title></head><body><table style='height: 90%; width: 90%;border-right-width: 1px; border-right-style: solid; border-right-color: #d7e0f1;border-left-width: 1px; border-left-style: solid; border-left-color: #d7e0f1;border-top-width: 1px; border-top-style: solid; border-top-color: #d7e0f1;border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #d7e0f1;' cellpadding='0' cellspacing='0'><tr style='height: 4%; width: 100%' valign='top'><td style='height: 5%; width: 100%'><table style='height: 100%; width: 100%;background-color: #D6D3D6' cellpadding='0' cellspacing='0' border='0'><tr style='height: 100%; width: 100%'><td><table style='height: 100%; width: 100%'><tr><td style='height: 100%; width: 30%'><table style='height: 100%; width: 100%'><tr><td style='width: 100%; font-family: Verdana; font-size: 13px' align='left'>" + dtHeader.Rows[0][0].ToString() + "</td></tr></table></td><td style='height: 100%; width: 30%; font-family: Verdana; font-size: 16px' align='center'><b>" + dtHeader.Rows[0][1].ToString() + "</b></td><td style='height: 100%; width: 40%'><table style='height: 100%; width: 100%'><tr><td style='height: 100%; width: 50%;'><table style='height: 100%; width: 100%'><tr><td style='height: 100%; width: 100%; font-family: Verdana; font-size: 13px' align='left'>" + dtHeader.Rows[0][2].ToString() + "</td></tr><tr><td style='height: 100%; width: 100%; font-family: Verdana; font-size: 13px' align='left'>" + dtHeader.Rows[1][2].ToString() + "</td></tr></table></td><td style='height: 100%; width: 50%' align='right'><img src='" + logo_fp.FullName + "' /></td></tr></table></td></tr></table></td></tr></table></td></tr><tr style='height: 3%; width: 100%' valign='top'><td>&nbsp;</td></tr><tr style='height: 93%; width: 100%' valign='top'><td><table style='height: 2%; width: 100%; position: fixed'><tr>";
                                for (int col = 0; col < dtGV.Columns.Count; col++)
                                {
                                    sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 13px; background-color: #D6D3D6;'align='left'><b>" + dtGV.Columns[col].ToString() + "</b></td>");
                                }
                                Append = "<table><tr>" + sb.ToString() + "</tr>";
                                foreach (DataRow dr in dtGV.Rows)
                                {
                                    sb = new System.Text.StringBuilder();
                                    for (int col = 0; col < dtGV.Columns.Count; col++)
                                    {
                                        if (col == dtGV.Columns.Count - 1)
                                        {
                                            remstring = remstring + "<tr>" + sb.ToString() + "<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dr[col].ToString() + "</td>" + "</tr>";
                                        }
                                        else
                                        {
                                            sb.Append("<td style='height: 100%; width: " + tdper + "%; font-family: Verdana; font-size: 12px;' align='left'>" + dr[col].ToString() + "</td>");
                                        }
                                    }
                                }
                                remstring = remstring + "<table>";
                                sb.Append("</td></table>");
                                htmltext = htmltext + Append + remstring + "</tr></table></body></html>";
                            }
                            break;
                        }
                    #endregion
                }
                if (ConfigurationManager.AppSettings["zipNormal"].ToString() != "Normal")
                {
                    sw.WriteLine(htmltext);
                    sw.Close();
                    filePaths[0] = imgPath;
                    filePaths[1] = fp.FullName;
                    createdFileNames = fp.FullName.ToString();
                    zipfiles(titleName, createdFileNames, filePaths);
                }
                else
                {
                    XmlNode xnodes = htmlXML.SelectSingleNode("//PrintOption");
                    string PrintOption = string.Empty;
                    if (xnodes != null)
                    {
                        PrintOption = htmlXML.SelectSingleNode("//PrintOption").InnerText;
                    }
                }
                if (printType.Length == 0)
                {
                    SaveHTMLFiles(htmltext, fileName, "attachment");
                }
            }
            return htmltext;
        }



        public void SaveHTMLFiles(string htmlText, string fileName, string printType)
        {
            //#region NLog
            //logger.Info("This method is used to save the HTML files.");
            //#endregion

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
            //#region NLog
            //logger.Info("This method id used to zip the files.");
            //#endregion

            try
            {
                string zipPath = string.Empty;
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
    }
}
