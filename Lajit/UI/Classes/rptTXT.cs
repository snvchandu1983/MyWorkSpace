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
using LAjitControls;
using LAjit_BO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Xml.Xsl;
using System.Xml.XPath;
using LAjitDev.UserControls;
using Gios.Pdf;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using OfficeOpenXml;
using NLog;


namespace LAjitDev
{
    public class rptTXT
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public void ReportStyle1(DataTable dtParent, DataTable dtHeader, Hashtable htColFormats, Hashtable htColNameValues)
        {
            string docPath = string.Empty;
            string strHeader = string.Empty;

            string compDetails = string.Empty;
            string compDate = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            int arrayCount = 0;
            int appendTabs = 0;

            try
            {
                docPath = ConfigurationManager.AppSettings["TempFilePath"];

                FileInfo file = new FileInfo(docPath + @"\1099 Report " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".txt");
                FileStream newfile = new FileStream(file.FullName, FileMode.Create);
                StreamWriter sWriter = new StreamWriter(newfile);
                //StreamWriter sw = new StreamWriter(newfile);

                drHeader = dtHeader.Rows[0];
                strHeader = drHeader["Column1"].ToString();
                string[] strRequestedBy = strHeader.Split(':');

                if (strRequestedBy.Length > 1)
                {
                    sWriter.Write("Requested By: " + strRequestedBy[arrayCount + 1].Trim().ToString());
                }
                else
                {
                    sWriter.Write("Requested By: " + strRequestedBy[arrayCount].Trim().ToString());
                }

                appendTabs = 2;
                AppendSpaces(sWriter, appendTabs);
                sWriter.Write(drHeader["Column2"].ToString());

                appendTabs = 1;
                AppendSpaces(sWriter, appendTabs);
                sWriter.Write("Run: " + DateTime.Now.ToString());

                if (dtHeader.Rows.Count > 1)
                {
                    drCompDetails = dtHeader.Rows[1];

                    sWriter.WriteLine();
                    appendTabs = 5;
                    AppendSpaces(sWriter, appendTabs);
                    sWriter.Write(drCompDetails["Column2"].ToString());

                    if (dtHeader.Rows.Count > 2)
                    {
                        drCompDate = dtHeader.Rows[2];

                        sWriter.WriteLine();
                        appendTabs = 5;
                        AppendSpaces(sWriter, appendTabs);
                        sWriter.Write(drCompDate["Column2"].ToString());
                    }
                }

                sWriter.WriteLine();
                sWriter.WriteLine();
                sWriter.WriteLine();

                foreach (DataColumn column in dtParent.Columns)
                {
                    if (column.ColumnName != htColNameValues["TrxID"].ToString())
                    {
                        sWriter.Write(column.ColumnName);

                        appendTabs = 1;
                        AppendSpaces(sWriter, appendTabs);
                    }
                }

                sWriter.WriteLine();

                for (int rowCount = 0; rowCount < dtParent.Rows.Count; rowCount++)
                {
                    foreach (DataColumn column in dtParent.Columns)
                    {
                        if (column.ColumnName != htColNameValues["TrxID"].ToString())
                        {
                            sWriter.Write(dtParent.Rows[rowCount][column.ColumnName].ToString());

                            appendTabs = 1;
                            AppendSpaces(sWriter, appendTabs);
                        }
                    }
                }
                sWriter.Close();
                newfile.Close();

                SaveToClient(file);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Append Spaces
        private void AppendSpaces(StreamWriter sWriter, int appendTabs)
        {
            while (appendTabs > 0)
            {
                sWriter.Write("\t");
                appendTabs--;
            }
        }

        //SaveToClient
        private void SaveToClient(FileInfo newFile)
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
                context.Response.End();
            }
        }
    }
}
