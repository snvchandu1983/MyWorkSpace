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
using LAjit_BO;
using System.Xml;
using NLog;


namespace LAjitDev.Common
{
    public partial class PrintViewer : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            CommonUI objCommonUI = new CommonUI();
            //Show PDF/EXCEL/HTML from grid buttom links click
            if (Page.Request.Params["page"] != null)
            {
                string pageNumber = Page.Request.Params["page"];
                string rptType = Page.Request.Params["RptType"];
                string PBDval = Page.Request.Params["PBD"];
                if (HttpContext.Current.Session["LinkBPinfo"] != null && pageNumber != null)
                {
                    string BPInfo = HttpContext.Current.Session["LinkBPinfo"].ToString();
                    PagesToPrint pages = PagesToPrint.Current;
                    if (pageNumber == "all")
                    {
                        pages = PagesToPrint.All;
                    }

                    string groupByCol = Request["GRP"] == null ? string.Empty : Request["GRP"];

                    string[] arrColumns = (string[])HttpContext.Current.Session["PrintCols"];
                    GridReports objGridReports = new GridReports(objCommonUI);
                    objGridReports.BPInfo = BPInfo;
                    objGridReports.ShowPDF = true;
                    objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
                    objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
                    switch (rptType)
                    {
                        case "PDF":
                            {
                                objGridReports.PrintData(ReportType.PDF.ToString().ToUpper(), pages, PBDval,groupByCol, arrColumns);
                                break;
                            }
                        case "EXCEL":
                            {
                                objGridReports.PrintData(ReportType.Excel.ToString().ToUpper(), pages, PBDval, groupByCol, arrColumns);
                                break;
                            }
                        case "HTML":
                            {
                                objGridReports.PrintData(ReportType.HTML.ToString().ToUpper(), pages, PBDval, groupByCol, arrColumns);
                                break;
                            }
                        case "WORD":
                            {
                                objGridReports.PrintData(ReportType.Word.ToString().ToUpper(), pages, PBDval, groupByCol, arrColumns);
                                break;
                            }
                    }
                    //Release the session once the task is completed.
                    #region NLog
                    logger.Debug("Releasing the Print Columns Session once the task is compete."); 
                    #endregion
                    HttpContext.Current.Session["PrintCols"] = null;
                }
            }
            else
            {
                //Exclusivly for chart pdf show
                if (Page.Request.Params["CCF"] != null)
                {
                    //Display chart image in PDF
                    Classes.ExportChart objExportCharts = new Classes.ExportChart();
                    objExportCharts.ExportPDF(Page.Request.Params["CCF"].ToString());
                }
            }
            objCommonUI.InjectSessionExpireScript(this);
        }
    }
}
