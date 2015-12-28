using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing;
using NLog;


using Gios.Pdf;

namespace LAjitDev.Classes
{
    public partial class ExportChart
    {
        private PdfDocument myPdfDocument;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public void ExportPDF(string ChartFilename)
        {

            string chartDir = ConfigurationManager.AppSettings["ChartFilePath"];

            string chartpath = chartDir + "\\" + ChartFilename + ".Jpeg";

            try
            {

                // Starting instantiate the document.

                myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);

                // Now the funny thing: Once we have a PdfArea, we can use it to do many many thigs...
                PdfArea textArea = new PdfArea(myPdfDocument, 20, 40, 557, 22);
                // ...we can generate two recatangles... (or circle, or lines...)
                PdfRectangle pr1 = new PdfRectangle(myPdfDocument, textArea, Color.LightGray, 1, Color.LightGray);

                string appDir = System.AppDomain.CurrentDomain.BaseDirectory;



                // This will load the image without placing into the document. The good thing
                // is that the image will be written into the document just once even if we put it
                // more times and in different sizes and places!

                //Lajit logo
                string logopath = appDir + "App_Themes\\" + HttpContext.Current.Session["MyTheme"].ToString() + "\\Images \\lajit_small-greylogo_03.JPG";
                PdfImage LogoImage = myPdfDocument.NewImage(@logopath);



                //To Test
                // string chartpath = appDir + "App_Themes\\" + HttpContext.Current.Session["MyTheme"].ToString() + "\\Images \\login-page.JPG";
                //string chartpath = appDir + "WebCharts\\" + ChartFilename + ".Jpeg";


                PdfImage ChartImage = myPdfDocument.NewImage(@chartpath);


                // we create a new page to put the generation of the new TablePage:
                PdfPage newPdfPage = myPdfDocument.NewPage();


                //while (!myHeaderPdfTable.AllTablePagesCreated)
                //{
                //then we create the first table of the Page adding it to the layout.
                // PdfTablePage newPdfTablePage1 =
                //myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 50,140,200,420));
                //myHeaderPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 50, 50, myPdfDocument.PageWidth - 100, 50));
                //  newPdfPage.Add(newPdfTablePage1);

                newPdfPage.Add(pr1);
                // now we start putting the logo into the right place with a high resoluton...
                newPdfPage.Add(LogoImage, textArea.Width - 20, 40);
                newPdfPage.Add(ChartImage, textArea.PosX, 70, 119);


                // now we start putting the logo into the right place with a high resoluton...
                //newPdfPage.Add(LogoImage,60,60,300);//ChartImage, 20, 20, 122);
                // newPdfPage.Add(ChartImage, 150, 150, 122);
                //}

                newPdfPage.SaveToDocument();
                // and now we are ready to export the PDF!
                SaveToResponse("Chart.PDF");
                //Delete exported pdf file

            }
            catch(Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
            finally
            {
                //bool FileExist = System.IO.File.Exists(chartpath);
                //if (FileExist)
                //{
                //    System.IO.File.Delete(chartpath);
                //}
            }
        }

        private void SaveToResponse(string fileName)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            //if (ShowPDF)
            //{
            HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", fileName));
            //}
            //else
            //{
            //    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", fileName));
            //}
            myPdfDocument.SaveToStream(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();

        }
    }
}
