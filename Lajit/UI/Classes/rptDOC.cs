using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Novacode;


namespace LAjitDev
{
    public class rptDOC
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public void ReportStyle1(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle2(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle3(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle5(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle10(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }

        //AR-Invoice History (Word)
        public void ReportStyle200(DataTable dtParent, DataTable dtChild, Hashtable htColNameValues, Hashtable htCGbColNameValues, DataTable dtHeader)
        {
            string docPath = string.Empty;
            string docDir = string.Empty;
            string strHeader = string.Empty;

            string compName = string.Empty;
            string compDetails = string.Empty;
            string compDate = string.Empty;

            DataRow drHeader;
            DataRow drCompDetails;
            DataRow drCompDate;

            int arrayCount = 0;
            int appendLineCount = 0;

            try
            {
                docPath = ConfigurationManager.AppSettings["TempFilePath"];

                if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["TempFilePath"]))
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["TempFilePath"]);
                }

                FileInfo newFile = new FileInfo(docPath + @"\Invoice " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx");

                using (DocX document = DocX.Create(newFile.FullName))
                {
                    drHeader = dtHeader.Rows[0];
                    strHeader = drHeader["Column1"].ToString();
                    string[] strRequestedBy = strHeader.Split(':');

                    Novacode.Formatting boldFormat = new Novacode.Formatting();
                    boldFormat.Size = 11;
                    boldFormat.Bold = true;

                    Novacode.Formatting spaceFormat = new Novacode.Formatting();
                    spaceFormat.Spacing = 70;

                    Novacode.Formatting normalFormat = new Novacode.Formatting();
                    normalFormat.Size = 11;

                    Novacode.Formatting lineFormat = new Novacode.Formatting();
                    lineFormat.Size = 11;
                    lineFormat.Bold = true;

                    string imgName = System.AppDomain.CurrentDomain.BaseDirectory + @"App_Themes\LAjit\Images\lajit_small-greylogo_03.JPG";
                    Novacode.Image ImgItem = document.AddImage(imgName);

                    Paragraph headerImage = document.InsertParagraph(" ", false);
                    headerImage.InsertPicture(ImgItem.Id);

                    Paragraph headerParagraph = document.InsertParagraph(" ", false, normalFormat);

                    Novacode.Table tblHeader = headerParagraph.InsertTableAfterSelf(3, 3);
                    tblHeader.Design = TableDesign.None;
                    document.Tables.Add(tblHeader);

                    if (strRequestedBy.Length > 1)
                    {
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Width = 300;
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.Alignment = Alignment.left;
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.InsertText("Requested By: ", false, boldFormat);
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.InsertText(strRequestedBy[arrayCount + 1].Trim().ToString(), false, normalFormat);
                    }
                    else
                    {
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Width = 300;
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.Alignment = Alignment.left;
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.InsertText("Requested By: ", false, normalFormat);
                        tblHeader.Rows[arrayCount].Cells[arrayCount].Paragraph.InsertText(strRequestedBy[arrayCount].Trim().ToString(), false, boldFormat);
                    }

                    tblHeader.Rows[arrayCount].Cells[arrayCount + 1].Width = 300;
                    tblHeader.Rows[arrayCount].Cells[arrayCount + 1].Paragraph.Alignment = Alignment.center;
                    tblHeader.Rows[arrayCount].Cells[arrayCount + 1].Paragraph.InsertText(drHeader["Column2"].ToString(), false, boldFormat);

                    tblHeader.Rows[arrayCount].Cells[arrayCount + 2].Width = 300;
                    tblHeader.Rows[arrayCount].Cells[arrayCount + 2].Paragraph.Alignment = Alignment.right;
                    tblHeader.Rows[arrayCount].Cells[arrayCount + 2].Paragraph.InsertText("Run:  ", false, boldFormat);
                    tblHeader.Rows[arrayCount].Cells[arrayCount + 2].Paragraph.InsertText(DateTime.Now.ToString(), false, normalFormat);

                    if (dtHeader.Rows.Count > 1)
                    {
                        drCompDetails = dtHeader.Rows[1];

                        tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Width = 300;
                        tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Paragraph.Alignment = Alignment.center;
                        tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Paragraph.InsertText(drCompDetails["Column2"].ToString(), false, boldFormat);

                        if (dtHeader.Rows.Count > 2)
                        {
                            drCompDate = dtHeader.Rows[2];
                            tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Width = 300;
                            tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Paragraph.Alignment = Alignment.center;
                            tblHeader.Rows[arrayCount + 1].Cells[arrayCount + 1].Paragraph.InsertText(drCompDate["Column2"].ToString(), false, boldFormat);
                        }
                    }

                    string invDate = dtParent.Rows[arrayCount][htColNameValues["InvDate"].ToString()].ToString();
                    string customer = dtParent.Rows[arrayCount][htColNameValues["Customer"].ToString()].ToString();

                    Paragraph infoParagraph = document.InsertParagraph(" ", false, normalFormat);

                    appendLineCount = 2;
                    AppendLines(infoParagraph, appendLineCount);

                    infoParagraph.InsertText(invDate, false, boldFormat);

                    infoParagraph.AppendLine("");
                    infoParagraph.InsertText(customer, false, normalFormat);

                    appendLineCount = 1;
                    AppendLines(infoParagraph, appendLineCount);

                    string[] strAddress = dtParent.Rows[arrayCount][htColNameValues["Address1"].ToString()].ToString().Split('~');

                    for (int arrayLength = 0; arrayLength < strAddress.Length; arrayLength++)
                    {
                        infoParagraph.AppendLine(strAddress[arrayLength]);
                    }

                    appendLineCount = 1;
                    AppendLines(infoParagraph, appendLineCount);

                    infoParagraph.InsertText(htColNameValues["InvNumber"] + ":  ", false, boldFormat);
                    infoParagraph.InsertText(dtParent.Rows[arrayCount][htColNameValues["InvNumber"].ToString()].ToString(), false, normalFormat);

                    appendLineCount = 2;
                    AppendLines(infoParagraph, appendLineCount);

                    infoParagraph.InsertText(htColNameValues["ARInfo1"] + ":  ", false, boldFormat);
                    infoParagraph.InsertText(dtParent.Rows[arrayCount][htColNameValues["ARInfo1"].ToString()].ToString(), false, normalFormat);

                    appendLineCount = 1;
                    AppendLines(infoParagraph, appendLineCount);

                    Paragraph lineParagraph = document.InsertParagraph("--------------", false, lineFormat);
                    int lineCount = 9;
                    while (lineCount > 1)
                    {
                        lineParagraph.InsertText("---------------", false, lineFormat);
                        lineCount--;
                    }

                    appendLineCount = 1;
                    AppendLines(lineParagraph, appendLineCount);

                    Paragraph prodParagraph = document.InsertParagraph(htColNameValues["ForProd"].ToString() + "  ", false, boldFormat);
                    prodParagraph.InsertText(dtParent.Rows[arrayCount][htColNameValues["ForProd"].ToString()].ToString(), false, normalFormat);

                    appendLineCount = 1;
                    AppendLines(prodParagraph, appendLineCount);

                    spaceFormat.Spacing = 315;

                    //To Print Child Coloumn Names
                    Paragraph childParagraph = document.InsertParagraph(" ", false, normalFormat);

                    int iterateCount = 1;
                    int tblRowCount = dtChild.Rows.Count + 3;

                    Novacode.Table tblChild = childParagraph.InsertTableAfterSelf(tblRowCount, 2);
                    tblChild.Design = TableDesign.TableGrid;
                    document.Tables.Add(tblChild);

                    foreach (DataColumn column in dtChild.Columns)
                    {
                        if (column.ColumnName == htCGbColNameValues["Description"].ToString() || column.ColumnName == htCGbColNameValues["InvoiceAmount"].ToString())
                        {
                            if (iterateCount == 1)
                            {
                                tblChild.Rows[arrayCount].Cells[iterateCount - 1].Width = 406;
                                tblChild.Rows[arrayCount].Cells[iterateCount - 1].Paragraph.Alignment = Alignment.left;
                            }
                            else
                            {
                                tblChild.Rows[arrayCount].Cells[iterateCount - 1].Width = 200;
                                tblChild.Rows[arrayCount].Cells[iterateCount - 1].Paragraph.Alignment = Alignment.right;
                            }

                            tblChild.Rows[arrayCount].Cells[iterateCount - 1].Paragraph.InsertText(column.ColumnName, false, boldFormat);
                            iterateCount++;
                        }
                    }

                    //iterateCount = 1;

                    //To Print Child Rows
                    for (int rowCount = 0; rowCount < dtChild.Rows.Count; rowCount++)
                    {
                        iterateCount = 1;
                        foreach (DataColumn column in dtChild.Columns)
                        {
                            if (column.ColumnName == htCGbColNameValues["Description"].ToString() || column.ColumnName == htCGbColNameValues["InvoiceAmount"].ToString())
                            {
                                if (iterateCount == 1)
                                {
                                    tblChild.Rows[rowCount + 1].Cells[iterateCount - 1].Width = 406;
                                    tblChild.Rows[rowCount + 1].Cells[iterateCount - 1].Paragraph.Alignment = Alignment.left;
                                }
                                else
                                {
                                    tblChild.Rows[rowCount + 1].Cells[iterateCount - 1].Width = 200;
                                    tblChild.Rows[rowCount + 1].Cells[iterateCount - 1].Paragraph.Alignment = Alignment.right;
                                }

                                tblChild.Rows[rowCount + 1].Cells[iterateCount - 1].Paragraph.InsertText(dtChild.Rows[rowCount][column.ColumnName].ToString(), false, normalFormat);
                                iterateCount++;
                            }
                        }
                    }

                    iterateCount = 1;

                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount - 1].Width = 406;
                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount - 1].Paragraph.Alignment = Alignment.left;
                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount - 1].Paragraph.InsertText(htColNameValues["InvoiceAmount"].ToString() + ": ", false, boldFormat);

                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount].Width = 200;
                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount].Paragraph.Alignment = Alignment.right;
                    tblChild.Rows[tblRowCount - 1].Cells[iterateCount].Paragraph.InsertText(dtParent.Rows[arrayCount][htColNameValues["InvoiceAmount"].ToString()].ToString(), false, boldFormat);

                    document.Save();
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

        public void ReportStyle400(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle405(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }

        public void ReportStyle501(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle502(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle503(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle601(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle602(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle603(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle604(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle621(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle622(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle641(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle642(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle643(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle651(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle652(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle653(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }
        public void ReportStyle654(DataTable dt, string fileName)
        {
            ExportDatatable(dt, fileName);
        }

        public void ExportDatatable(DataTable dt, string fileName)
        {
            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string filename = fileName;
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "";
            response.ContentType = "application/vnd.ms-word";
            response.ContentType = "application/ms-word";
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}.doc", fileName));

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

        //To Append Lines in Word
        private void AppendLines(Paragraph paragraph, int appendLineCount)
        {
            while (appendLineCount > 0)
            {
                paragraph.AppendLine(" ");
                appendLineCount--;
            }
        }

        //SaveToClient
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
                context.Response.End();
            }
        }

    }
}