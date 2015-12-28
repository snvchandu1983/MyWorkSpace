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
// Namespace for PDF generation
using Gios.Pdf;
using ICSharpCode.SharpZipLib.Zip;
using NLog;


namespace LAjitDev.Financials
{
    public partial class PrintPopUp : LAjitDev.Classes.BasePagePopUp
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        public LAjitDev.clsReportsUI reportObjUI = new clsReportsUI();
        public LAjitDev.PopUps.ShowPDF objPrintData = new LAjitDev.PopUps.ShowPDF();
        string m_sessionOutXml = string.Empty;
        private GridViewControl GVUC;

        public GridViewControl GridViewUserControl
        {
            get { return GVUC; }
            set { GVUC = value; }
        }

        public String GVRequestXml
        {
            get { return (String)ViewState["GVRequestXml"]; }
            set { ViewState["GVRequestXml"] = value; }
        }

        public String GVReturnXml
        {
            get { return (String)ViewState["GVReturnXml"]; }
            set { ViewState["GVReturnXml"] = value; }
        }

        public string flag
        {
            get { return (String)ViewState["flag"]; }
            set { ViewState["flag"] = value; }
        }

        public string curPage
        {
            get { return (String)ViewState["curPage"]; }
            set { ViewState["curPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string BPInfo = string.Empty;

            if (!IsPostBack)
            {
                if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
                {
                    switch (Page.Request.QueryString["PopUP"].ToString())
                    {
                        case "PE":
                        case "PopUp":
                            {
                                if (Session["LinkBPinfo"] != null)
                                {
                                    BPInfo = Session["LinkBPinfo"].ToString();
                                    flag = Page.Request.QueryString["PopUp"].ToString();
                                    BindPage(BPInfo, this, flag);
                                }
                            }
                            break;
                        case "BPP":
                        case "BU":
                            {
                                if (Session["BPINFO"] != null)
                                {
                                    BPInfo = Session["BPINFO"].ToString();
                                    flag = Page.Request.QueryString["PopUp"].ToString();
                                    BindPage(BPInfo, this, flag);
                                }
                            }
                            break;
                    }
                }
            }
            BindReportInfo();
            PreLoadImages();
            if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
            }
        }

        private bool GridviewExist(string BPINFO)
        {
            bool status = false;
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(BPINFO);
            XmlNode nodegv = xdoc.SelectSingleNode("bpinfo//Gridview");
            if (nodegv != null)
            {
                status = true;
            }
            return status;
        }

        public void BindPage(string SessionBPInfo, Control CurrentPage, string _flag)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + SessionBPInfo);
            #endregion

            XmlDocument xDoc = new XmlDocument();
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            //generate Request XML 
            if (_flag == "BPP" || _flag == "PE" || _flag == "PopUp")
            {
                if (GridviewExist(SessionBPInfo))
                {
                    strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Session["BPE"].ToString(), false, "-1", "", null);
                }
                else
                {
                    strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Session["BPE"].ToString(), true, "-1", "", null);
                }
                //BPOUT from DB
                strOutXml = objBO.GetDataForCPGV1(strReqXml);
            }
            else
            {
                strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Session["BPE"].ToString(), false, "", "", null);
                //BPOUT from DB
                strOutXml = objBO.GetDataForCPGV1(strReqXml);
            }
            GVReturnXml = strOutXml;
            if (strOutXml != null)
            {
                xDoc.LoadXml(strOutXml);
                //For successful BPOut
                XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                if (nodeMsgStatus.InnerText == "Success")
                {
                    if (_flag == "BU")
                    {
                        pnlImgButtons.Visible = true;
                        pnlCurrentAll.Visible = true;
                        pnlExtendedCols.Visible = true;
                        pnlSavetoMyReports.Visible = true;
                        pnlDistributionList.Visible = true;
                        pnlChkReportRetrieve.Visible = true;
                        if (Page.Request.QueryString["CurPage"] != null && Page.Request.QueryString["CurPage"] != string.Empty)
                        {
                            curPage = Page.Request.QueryString["CurPage"].ToString();
                        }
                    }
                    //else if (_flag == "BPP")
                    //{
                    //    PrintAll("PAGELOAD", _flag);
                    //}
                    //else
                    //{
                    //    if (_flag == "PopUp" || _flag == "PE")
                    //    {
                    //        string xpaths = "//RowList";
                    //        if (xDoc.SelectSingleNode(xpaths) != null)
                    //        {
                    //            int rowsCount = xDoc.SelectSingleNode(xpaths).ChildNodes.Count;
                    //            XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    //            int reportStyle = 0;
                    //            if (nodeTreenode.Attributes["ReportStyle"] != null)
                    //            {
                    //                reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
                    //                //Based on reportstyle hidding controls
                    //                if ((reportStyle >= 100) && (reportStyle <= 200))
                    //                {
                    //                    PrintAll("PAGELOAD", _flag);
                    //                }
                    //                else
                    //                {
                    //                    if ((reportStyle != 100) && (reportStyle != 200) && (reportStyle != 0))
                    //                    {
                    //                        PrintAll("PAGELOAD", _flag);
                    //                        trPDF.Visible = true;
                    //                        trHtml.Visible = true;
                    //                        trExcel.Visible = true;
                    //                        trPrint.Visible = true;
                    //                    }
                    //                    else if (reportStyle == 0)
                    //                    {
                    //                        trPDF.Visible = false;
                    //                        trHtml.Visible = false;
                    //                        trExcel.Visible = false;
                    //                        trPrint.Visible = false;

                    //                        pnlCurrentAll.Visible = false;
                    //                        pnlExtendedCols.Visible = false;
                    //                        pnlSavetoMyReports.Visible = false;
                    //                        pnlDistributionList.Visible = false;
                    //                        pnlChkReportRetrieve.Visible = false;
                    //                        //pnlImgButtons.Visible = false;
                    //                        pnllblMsg.Visible = true;

                    //                        lblMsg.Text = "";
                    //                        lblMsg.Visible = true;
                    //                        lblMsg.Text = "Your Process has been Submitted Successfully";
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    else
                    {
                        if (_flag == "PopUp" || _flag == "PE" || _flag == "BPP")
                        {
                            string xpaths = "//RowList";
                            if (xDoc.SelectSingleNode(xpaths) != null)
                            {
                                int rowsCount = xDoc.SelectSingleNode(xpaths).ChildNodes.Count;
                                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                                int reportStyle = 0;
                                if (nodeTreenode.Attributes["ReportStyle"] != null)
                                {
                                    reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
                                    if (reportStyle == 0)
                                    {
                                        trPDF.Visible = false;
                                        trHtml.Visible = false;
                                        trExcel.Visible = false;
                                        trPrint.Visible = false;

                                        pnlCurrentAll.Visible = false;
                                        pnlExtendedCols.Visible = false;
                                        pnlSavetoMyReports.Visible = false;
                                        pnlDistributionList.Visible = false;
                                        pnlChkReportRetrieve.Visible = false;
                                        pnlImgButtons.Visible = false;
                                        pnllblMsg.Visible = true;

                                        lblMsg.Text = "";
                                        lblMsg.Visible = true;
                                        lblMsg.Text = "Your Process has been Submitted Successfully";
                                    }
                                    else
                                    {
                                        GenerateReport("PDF", "", "", _flag);
                                    }
                                }
                                else
                                {
                                    GenerateReport("PDF", "", "", _flag);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //pnlImgButtons.Visible = false;
                    pnlCurrentAll.Visible = false;
                    pnlExtendedCols.Visible = false;
                    pnlSavetoMyReports.Visible = false;
                    pnlDistributionList.Visible = false;
                    pnlChkReportRetrieve.Visible = false;
                    pnllblMsg.Visible = true;
                    lblMsg.Visible = true;

                    if (xDoc.SelectSingleNode("//OtherInfo") != null)
                    {
                        if (xDoc.SelectSingleNode("//OtherInfo").InnerText != null && xDoc.SelectSingleNode("//OtherInfo").InnerText != string.Empty)
                        {
                            lblMsg.Text = xDoc.SelectSingleNode("//Status").InnerText + " - " + xDoc.SelectSingleNode("//OtherInfo").InnerText;
                        }
                        else
                        {
                            lblMsg.Text = xDoc.SelectSingleNode("//Status").InnerText + "-" + xDoc.SelectSingleNode("//Label").InnerText;
                        }
                    }
                    else
                    {
                        lblMsg.Text = xDoc.SelectSingleNode("//Status").InnerText + "-" + xDoc.SelectSingleNode("//Label").InnerText;
                    }
                }
            }
        }

        private void PreLoadImages()
        {
            imgbtnPdf.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/pdf_icon.png";
            imgbtnPdf.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/pdf_icon-over.PNG'");
            imgbtnPdf.Attributes.Add("onmouseout", "this.src='/" + Application["ImagesCDNPath"].ToString() + "Images/pdf_icon.png'");

            imgbtnExcel.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/excel_icon.PNG";
            imgbtnExcel.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/excel_icon-over.PNG'");
            imgbtnExcel.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/excel_icon.PNG'");

            imgbtnHtml.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/html_icon.png";
            imgbtnHtml.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/html_icon-over.PNG'");
            imgbtnHtml.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/html_icon.png'");

            imgbtnPrintOption.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/print-icon.png";
            imgbtnPrintOption.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon-over.png'");
            imgbtnPrintOption.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon.png'");
        }

        #region REPORTS METHODS

        private void PrintData(string rptType, string _flag)
        {
            #region NLog
            logger.Info("This method is used to print the data for report type as : "+rptType);
            #endregion

            #region VARIABLES
            lblMsg.Text = string.Empty;
            GVRequestXml = hdnGVReqXml.Value;
            if (GVRequestXml != "")
            {
                if (GVRequestXml != null && GVRequestXml != string.Empty)
                {
                    GVRequestXml = GVRequestXml;
                }
            }
            if (GVReturnXml != "")
            {
                if (GVReturnXml != null && GVReturnXml != string.Empty)
                {
                    GVReturnXml = GVReturnXml;
                }
            }
            if (hdnReportName.Value != string.Empty)
            {
                string reportname = hdnReportName.Value;
            }
            //Selected Distribution info from Dropdown
            string rptinfo = lstDistribution.Value.ToString();
            //Selected Page Info from Dropdown
            string rptPageInfo = lstPage.Value.ToString();

            string rptPrintNotes = string.Empty;
            string msgInfo = string.Empty;

            //Checking if Notes Column shuold be printed or not
            if (lstExtendedCols.Value.ToUpper().Trim() == "YES")
            {
                rptPrintNotes = lstExtendedCols.Value.ToUpper().Trim();
            }
            #endregion
            #region FOREGROUND REPORTING
            //ForeGround Reporting
            if (lstSaveToMyReports.Value.ToUpper().Trim() == "YES" && lstRetrieveRpt.Value.ToUpper().Trim() == "YES")//Checking for saving in DB and Displaying it to the User
            {
                lblMsg.Text = string.Empty;
                // Saves the report in the DB                     
                ForeGroundRpting(rptinfo);
                //Generates the report and displays it to the user
                msgInfo = GenerateReport(rptType, rptPageInfo, rptPrintNotes, flag);
                //Saving the report to File System based on userpreference
                XmlDocument xDocBPE = new XmlDocument();
                xDocBPE.LoadXml(Convert.ToString(Session["BPE"]));
                XmlNode nodeRptToFileSystem = xDocBPE.SelectSingleNode("bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='203']");
                if (nodeRptToFileSystem != null)
                {
                    if (nodeRptToFileSystem.Attributes["Value"] != null)
                    {
                        if (nodeRptToFileSystem.Attributes["Value"].Value == "1")
                        {
                            if (msgInfo != string.Empty)
                            {
                                SaveReport();
                            }
                        }
                    }
                }
            }
            #endregion
            #region BACKGROUND REPORTING
            //BackGround Reporting
            else if (lstSaveToMyReports.Value.ToUpper().Trim() == "YES" && lstRetrieveRpt.Value.ToUpper().Trim() == "NO")//Checking for saving in DB and it should not be Displayed to the User
            {
                //Saves the report in the engine
                String RetXML = BackGroundRpting(rptinfo);
                //BackGround 
                if (lstSaveToMyReports.Value.ToUpper().Trim() == "YES" && lstRetrieveRpt.Value.ToUpper().Trim() == "NO")
                {
                    string ReturnXML = RetXML;
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(ReturnXML);
                    //Get the Message node
                    msgInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;
                    //Displays the Status Message Panel
                    pnllblMsg.Visible = true;
                    lblMsg.Text = msgInfo;
                    lstSaveToMyReports.SelectedIndex = 1;
                    string s = "javascript:ShowHideRptOptions();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDropDown", s, true);
                }
                else
                {
                    pnllblMsg.Visible = false;
                    lblMsg.Text = string.Empty;
                }
            }
            #endregion
            #region USER SELECTED REPORT
            //Displaying report to the User when he doesn't select either ForeGround or BackGround Reporting
            else if (lstSaveToMyReports.Value.ToUpper().Trim() == "NO" && lstRetrieveRpt.Value.ToUpper().Trim() == "NO")//When Report should be just Displayed to the User
            {
                pnllblMsg.Visible = false;
                lblMsg.Text = string.Empty;
                //Generates the report and displays it to the user
                msgInfo = GenerateReport(rptType, rptPageInfo, rptPrintNotes, _flag);
            }
            #endregion
        }

        private void SaveReport()
        {
            //Return xml to store
            string xmlToStore = GVReturnXml.ToString();

            XmlDocument xmlReturnDoc = new XmlDocument();
            xmlReturnDoc.LoadXml(xmlToStore);

            // node File name
            XmlNode rptFileName = xmlReturnDoc.SelectSingleNode("Root/Reportout/FileName");
            //File name to store return xml
            String xmlFileName = rptFileName.InnerText;

            // To get the path from Web.Config to save the reports generated
            string reportFilePath = ConfigurationManager.AppSettings["XmlFilePath"].ToString().Trim();

            //string appPath = HttpContext.Current.Request.ApplicationPath;
            //string physicalPath = HttpContext.Current.Request.MapPath(appPath);

            // Naming convention for the Report generated 
            //string xmlfilepath = physicalPath + reportFilePath + @"\" + xmlFileName + ".xml";
            string xmlfilepath = reportFilePath + @"\" + xmlFileName + ".xml";

            //DateTime.Now.ToFileTime() + "_" +
            // Checking for the report path existence as per the config key
            //if (!(Directory.Exists(physicalPath + reportFilePath)))
            //{
            //    Directory.CreateDirectory(physicalPath + reportFilePath);
            //}
            if (!(Directory.Exists(reportFilePath)))
            {
                Directory.CreateDirectory(reportFilePath);
            }

            // Getting the file info from the specified file path
            FileInfo f = new FileInfo(xmlfilepath);
            // delete the file if it already exist and add the latest file
            if (f.Exists)
            {
                f.Delete();
            }

            //Saving the XML in the SpecifiedPath
            xmlReturnDoc.Save(xmlfilepath);
        }

        private void BindReportInfo()
        {
            #region NLog
            logger.Info("To Bind the ReportInfo from BPE to Dropdown"); 
            #endregion

            //To Bind the ReportInfo from BPE to Dropdown
            XmlDocument xDocRptInfo = new XmlDocument();
            xDocRptInfo.LoadXml(Convert.ToString(Session["BPE"]));

            if (xDocRptInfo.SelectSingleNode("bpe/companyinfo/reportinfo") != null)
            {
                XmlNodeList nodeLstValues = xDocRptInfo.SelectSingleNode("bpe/companyinfo/reportinfo").ChildNodes;

                string ddlvalue = string.Empty;

                foreach (XmlNode nodeDDLRow in nodeLstValues)
                {
                    string ddlText = nodeDDLRow.Attributes["Label"].Value;
                    string ddlValue = nodeDDLRow.Attributes["ID"].Value;
                    lstDistribution.Items.Add(new ListItem(ddlText, ddlValue));
                }
            }
            else
            {
                string ddlText = "None";
                string ddlValue = "8";
                lstDistribution.Items.Add(new ListItem(ddlText, ddlValue));
            }

        }

        public string BackGroundRpting(string rptinfo)
        {
            #region NLog
            logger.Info("RequestXML for BackGround Reporting with report info as : "+rptinfo);
            #endregion

            string ReturnXML = string.Empty;
            try
            {
                string requestXML = GVRequestXml.ToString();
                //RequestXML for BackGround Reporting
                string reqxml = reportsBO.GetBackGroundReqXML(requestXML, rptinfo, Convert.ToString(Session["BPE"]));
                //ReturnXML for BackGround Reporting
                ReturnXML = reportsBO.GetReportBPEOut(reqxml);
            }
            catch
            {
                //throw ex;
            }
            return ReturnXML;
        }

        public void ForeGroundRpting(string rptinfo)
        {
            #region NLog
            logger.Info("RequestXML for ForeGround Reporting with report info as : " + rptinfo);
            #endregion

            try
            {
                string requestXML = GVRequestXml.ToString();
                //RequestXML for ForeGround Reporting
                string reqxml = reportsBO.GetForeGroundReqXML(requestXML, rptinfo, Convert.ToString(Session["BPE"]));
                //ReturnXML for ForeGround Reporting
                string ReturnXML = reportsBO.GetReportBPEOut(reqxml);
                //Saving request and return xml's in ViewState for generating reports                 
                GVReturnXml = ReturnXML;
            }
            catch
            {
                //throw ex;
            }
        }

        public string GenerateReport(string rptType, string rptPageInfo, string rptPrintNotes, string _flag)
        {
            #region NLog
            logger.Info("This method is used to generate the report based on the report type as : " + rptType + " and report page info as : " + rptPageInfo + " adn report print notes as : " + rptPrintNotes + " and flag as : " + _flag); 
            #endregion

            DataTable NotesDT = new DataTable();
            DataTable dt = new DataTable();
            DataTable pNotesDT = new DataTable();
            try
            {
                //GVReturn XML Based on Current or All Pages
                string GVXml = string.Empty;
                if (_flag == "PopUp" || _flag == "PE" || _flag == "BPP")
                {
                    GVXml = GVReturnXml;
                }
                else
                {
                    if (_flag == "BU")
                    {
                        if (rptPageInfo.Trim().ToUpper().ToString() == "CURRENT")
                        {
                            if (GVXml != null)
                            {
                                if (curPage == "1")
                                {
                                    GVXml = GVRequestXml.ToString();
                                }
                                else
                                {
                                    if (GVRequestXml != "")
                                    {
                                        if (GVRequestXml != null && GVRequestXml != string.Empty)
                                        {
                                            string GVRequestXMLstr = GenerateGVRequestXML(GVRequestXml.ToString(), "20", curPage);
                                            GVXml = reportsBO.GetReportBPEOut(GVRequestXMLstr);
                                        }
                                        else
                                        {
                                            GVXml = GVReturnXml.ToString();
                                        }
                                    }
                                    else
                                    {
                                        GVXml = GVReturnXml.ToString();
                                    }
                                }
                            }
                        }
                        //All Pages
                        else if (rptPageInfo.Trim().ToUpper().ToString() == "ALL")
                        {
                            if (GVXml != null)
                            {
                                if (GVRequestXml != string.Empty && GVRequestXml != null)
                                {
                                    string GVRequestXMLstr = GenerateGVRequestXML(GVRequestXml.ToString(), "-1", "");
                                    GVXml = reportsBO.GetReportBPEOut(GVRequestXMLstr);
                                }
                                else
                                {
                                    if (GVReturnXml != null && GVReturnXml != string.Empty)
                                    {
                                        GVXml = GVReturnXml;
                                    }
                                }
                            }
                        }

                    }
                }
                int trxID = Convert.ToInt32(Page.Request.Params["TrxID"]);
                int trxType = Convert.ToInt32(Page.Request.Params["TrxType"]);

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(GVXml);

                if (trxID != 0 || trxType != 0)
                {
                    XmlNode isBranches = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree");
                    bool rpt = true;
                    foreach (XmlNode xnBranches in isBranches.ChildNodes)
                    {
                        if (xnBranches.Name == "Branches")
                        {
                            int isPrint = Convert.ToInt32(xDoc.SelectSingleNode("//FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value);
                            if (isPrint == 1)
                            {
                                XmlNode xReportStyleNode = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree");
                                for (int xa = 0; xa < xReportStyleNode.Attributes.Count; xa++)
                                {
                                    if (xReportStyleNode.Attributes[xa].Name != "ReportStyle")
                                    {
                                        rpt = false;
                                        continue;
                                    }
                                    else
                                    {
                                        if (xReportStyleNode.Attributes[xa].Name == "ReportStyle")
                                        {
                                            XmlAttribute attrReportStyle = xDoc.CreateAttribute("ReportStyle");
                                            attrReportStyle.Value = "4";
                                            xReportStyleNode.Attributes.Append(attrReportStyle);
                                            break;
                                        }
                                    }
                                }
                                if (rpt == false)
                                {
                                    XmlAttribute attrReportStyle = xDoc.CreateAttribute("ReportStyle");
                                    attrReportStyle.Value = "4";
                                    xReportStyleNode.Attributes.Append(attrReportStyle);
                                    break;
                                }
                            }
                        }
                    }
                    string pageNode = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree/Node").InnerXml;
                    XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + pageNode + "/RowList");
                    XmlNode nodePrint = xDoc.SelectSingleNode("//" + pageNode + "/RowList/Rows[@TrxID=" + trxID + "]");
                    XmlNodeList xnl = xDoc.SelectNodes("//" + pageNode + "/RowList/Rows");
                    foreach (XmlNode xnls in xnl)
                    {
                        if (xnls.Attributes["TrxID"].InnerText != trxID.ToString())
                        {
                            nodeRowList.RemoveChild(xnls);
                        }
                    }
                }
                XmlDocument xPrintOption = new XmlDocument();
                XmlNode nodePrintOption = xPrintOption.CreateNode(XmlNodeType.Element, "Root", null);
                nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
                nodePrintOption.InnerXml += "<PrintOption>" + _flag + "</PrintOption>";


                GVXml = string.Empty;
                GVXml = nodePrintOption.OuterXml.ToString();

                if (GVXml != string.Empty)
                {
                    XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    string treeNodeName = string.Empty;
                    if (nodeTreenode.ToString() != null)
                    {
                        if (nodeTreenode.SelectSingleNode("Node") != null)
                        {
                            treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                        }
                    }
                    //Generating the Document
                    //PDF
                    if (rptType.Trim().ToUpper().ToString() == "PDF")
                    {
                        //reportObjUI.GenerateReport(GVXml);
                        reportObjUI.GenerateReport(xPrintOption, "PDF");
                    }
                    //Excel
                    else if (rptType.Trim().ToUpper().ToString() == "EXCEL")
                    {
                        //Get the Datatable to print

                        //dt = reportObjUI.XMLToDataTable(GVXml, treeNodeName, rptType);
                        //dt = reportObjUI.GetSingleRowDataToPrint(GVXml, treeNodeName, rptType, trxID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    //Removing TrxID column
                        //    if (dt.Columns.Contains("TrxID"))
                        //    {
                        //        dt.Columns.Remove("TrxID");
                        //    }
                        //    if (dt.Columns.Contains("Notes"))
                        //    {
                        //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                        //        dt.Columns.Remove("Notes");
                        //    }
                        //    //Export to Excel
                        //    ExportDatatableToExcel(dt, "ExcelRpt", NotesDT, rptPrintNotes);
                        //}
                    }
                    //HTML
                    else if (rptType.Trim().ToUpper().ToString() == "HTML")
                    {
                        //Get the Datatable to print
                        //dt = reportObjUI.XMLToDataTable(GVXml, treeNodeName, "");
                        //dt = reportObjUI.GetSingleRowDataToPrint(GVXml, treeNodeName, "", trxID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    if (dt.Columns.Contains("Notes"))
                        //    {
                        //        NotesDT = reportsBO.GenerateNotesDatatable(dt);
                        //        dt.Columns.Remove("Notes");
                        //    }
                        //    //Export to HTML
                        //    //ExportDatatableToHTML(GVXml, dt, "HtmlRpt", NotesDT, rptPrintNotes);
                        //    string htmlText = reportObjUI.CreateHTMLTemplate(GVXml, dt);
                        //    pnlTotalPrint.Style.Add("display", "");

                        //    pnlTotalPrint.Visible = true;
                        //    pnlPrinting.Visible = false;
                        //    pnlPrintOptionBtn.Visible = true;

                        //    trPrint.Visible = true;
                        //    trPDF.Visible = true;
                        //    trHtml.Visible = true;
                        //    trExcel.Visible = true;

                        //    pnlChooseOptions.Visible = true;
                        //    lblPopupEntry.Text = "";
                        //    lblPopupEntry.Text = "Print Preveiw";

                        //    pnlCurrentAll.Visible = false;
                        //    pnlExtendedCols.Visible = false;
                        //    pnlSavetoMyReports.Visible = false;
                        //    pnlDistributionList.Visible = false;
                        //    pnlChkReportRetrieve.Visible = false;

                        //    litPrintOptionBtn.Text = htmlText;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion

                throw ex;
            }
            if (dt.Rows.Count > 0)
            {
                return "Successfully Generated Report";
            }
            else
            {
                return "";
            }
        }

        #region Export To Excel
        public void ExportDatatableToExcel(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes)
        {
            #region NLog
            logger.Info("This to export the gridview to the excel document with filename as : " + fileName + " and report print notes : " + rptPrintNotes);
            #endregion

            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string filename = fileName;

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "";
            response.ContentType = "application/vnd.ms-excel";
            response.ContentType = "application/ms-excel";
            response.AddHeader("content-disposition",
            string.Format("attachment;filename={0}.xls", fileName));

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
            if (rptPrintNotes.Trim().ToUpper().ToString() == "YES")
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        GridView dv = new GridView();
                        dv.DataSource = NotesDT;
                        dv.DataBind();
                        dv.RenderControl(htw);
                        sw.Flush();
                        sw.Dispose();
                        response.Write(style);
                        response.Write(sw.ToString());
                    }
                }
            }
            response.Flush();
            response.Close();
        }
        #endregion

        #region GenerateGVReqXML
        private string GenerateGVRequestXML(string requestXML, string pageSize, string pageNumber)
        {
            #region NLog
            logger.Info("This method is used to generate the GV Request XMl based on the request XML and pages size as : " + pageSize+" and page number : "+pageNumber);
            #endregion

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(requestXML);
            string bpinfo = xDoc.SelectSingleNode("bpinfo").OuterXml;
            //Creating the Root and bpe node
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = Session["BPE"].ToString();
            //Creating the bpinfo node
            nodeRoot.InnerXml += bpinfo;
            //XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/Gridresults");
            XmlNode nodePageSize = nodeRoot.SelectSingleNode("bpinfo//Pagesize");
            XmlNode nodePageNumber = nodeRoot.SelectSingleNode("bpinfo//Pagenumber");
            if (nodePageSize.InnerText != null && pageSize != null)
            {
                nodePageSize.InnerXml = pageSize;
            }
            if (nodePageNumber.InnerText != null && pageNumber != null)
            {
                nodePageNumber.InnerXml = pageNumber;
            }
            string reqxml = nodeRoot.OuterXml;
            return reqxml;
        }
        #endregion

        #region CREATE PRINT CRITERIA TABLE
        private Table CreatePrintCriteriaTable(string GVRetXml)
        {
            #region NLog
            logger.Info("This method is used to print the Criteria Table"); 
            #endregion

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(GVRetXml);

                Table tblPrintCriteria = new Table();
                //Add a new row to the table.
                TableRow tr = new TableRow();
                tr.ID = "trDynamicPrintCriteria";
                tblPrintCriteria.Rows.Add(tr);
                //tblPrintCriteria.Width = Unit.Percentage(100);
                int rowCntr = 0;

                XmlNode nodeTree = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");

                string treeNodeName = nodeTree.SelectSingleNode("Node").InnerText;
                //Go the tree node and pick up the columns node.
                XmlNode nodeTreeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                foreach (XmlNode nodeCol in nodeTreeColumns.ChildNodes)
                {
                    if (nodeCol.Attributes["Caption"] != null && nodeCol.Attributes["Caption"].Value.Trim() != string.Empty)
                    {
                        CheckBox chkBx = new CheckBox();
                        chkBx.ID = "chk" + nodeCol.Attributes["Label"].Value.Trim();
                        chkBx.Text = nodeCol.Attributes["Caption"].Value.Trim();
                        //TD for the Link Text
                        TableCell tdLinkText = new TableCell();
                        tdLinkText.Wrap = false;
                        tdLinkText.Controls.Add(chkBx);
                        tr.Cells.Add(tdLinkText);
                    }
                }
                return tblPrintCriteria;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                Table tblErrorContainer = new Table();
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = ex.Message;
                td.ForeColor = Color.Red;
                tr.Cells.Add(td);
                tblErrorContainer.Rows.Add(tr);
                return tblErrorContainer;
            }
        }
        #endregion

        protected void imgbtnPdf_Click(object sender, ImageClickEventArgs e)
        {
            PrintData(imgbtnPdf.AlternateText.ToString().Trim().ToUpper(), flag);
        }

        protected void imgbtnExcel_Click(object sender, ImageClickEventArgs e)
        {
            PrintData(imgbtnExcel.AlternateText.ToString().Trim().ToUpper(), flag);
        }

        protected void imgbtnHtml_Click(object sender, ImageClickEventArgs e)
        {
            PrintData(imgbtnHtml.AlternateText.ToString().Trim().ToUpper(), flag);
        }

        protected void imgbtnPrintOption_Click(object sender, ImageClickEventArgs e)
        {
            PrintAll("PRINT", flag);
        }

        public void PrintAll(string printType, string _flag)
        {
            string GVXml = string.Empty;
            XmlDocument xDoc = new XmlDocument();
            string treeNodeName = string.Empty;
            GVRequestXml = hdnGVReqXml.Value;

            if (_flag == "PopUp" || _flag == "PE" || _flag == "BPP")
            {
                if (printType == "PAGELOAD" || printType == "PRINT")
                {
                    GVXml = GVReturnXml;
                }
            }
            else
            {
                if (_flag == "BU")
                {
                    if (printType == "PAGELOAD")
                    {
                        if (lstPage.Value.ToString().Trim().ToUpper().ToString() == "CURRENT")
                        {
                            GVXml = GVReturnXml;
                        }
                        else if (lstPage.Value.ToString().Trim().ToUpper().ToString() == "ALL")
                        {
                            if (GVRequestXml != string.Empty && GVRequestXml != null)
                            {
                                string GVRequestXMLstr = GenerateGVRequestXML(GVRequestXml.ToString(), "-1", "");
                                GVXml = reportsBO.GetReportBPEOut(GVRequestXMLstr);
                            }
                            else
                            {
                                GVXml = GVReturnXml;
                            }
                        }
                    }
                    else
                    {
                        if (printType == "PRINT")
                        {
                            if (lstPage.Value.ToString().Trim().ToUpper().ToString() == "CURRENT")
                            {
                                GVXml = GVReturnXml;
                            }
                            else if (lstPage.Value.ToString().Trim().ToUpper().ToString() == "ALL")
                            {
                                if (GVRequestXml != string.Empty && GVRequestXml != null)
                                {
                                    string GVRequestXMLstr = GenerateGVRequestXML(GVRequestXml.ToString(), "-1", "");
                                    GVXml = reportsBO.GetReportBPEOut(GVRequestXMLstr);
                                }
                                else
                                {
                                    GVXml = GVReturnXml;
                                }
                            }
                        }
                    }
                }
            }
            DataTable NotesDT = new DataTable();
            DataTable dt = new DataTable();
            DataTable pNotesDT = new DataTable();
            //Get the Datatable to print
            if (GVXml != string.Empty)
            {
                xDoc.LoadXml(GVXml);
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                if (nodeTreenode.ToString() != null)
                {
                    if (nodeTreenode.SelectSingleNode("Node") != null)
                    {
                        treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                    }
                }
            }
            dt = reportObjUI.XMLToDataTable(GVXml, treeNodeName, "");
            if (dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("Notes"))
                {
                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                    dt.Columns.Remove("Notes");
                }
                xDoc = new XmlDocument();
                xDoc.LoadXml(GVXml);
                XmlDocument xPrintOption = new XmlDocument();
                XmlNode nodePrintOption = xPrintOption.CreateNode(XmlNodeType.Element, "Root", null);

                if (printType == "PAGELOAD")
                {
                    nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
                    nodePrintOption.InnerXml += "<PrintOption>" + printType + "</PrintOption>";
                }
                else
                {
                    nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
                    nodePrintOption.InnerXml += "<PrintOption>" + printType + "</PrintOption>";
                }
                string htmlText = string.Empty;
                if (GVXml != string.Empty)
                {
                    XmlDocument xDocs = new XmlDocument();
                    xDocs.LoadXml(nodePrintOption.OuterXml.ToString());

                    XmlNode nodeTreenodes = xDocs.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    string treeNodeNames = string.Empty;

                    if (nodeTreenodes.ToString() != null)
                    {
                        if (nodeTreenodes.SelectSingleNode("Node") != null)
                        {
                            treeNodeNames = nodeTreenodes.SelectSingleNode("Node").InnerText;
                        }
                    }
                    //Get the Datatable to print
                    dt = reportObjUI.XMLToDataTable(nodePrintOption.OuterXml.ToString(), treeNodeName, "HTML");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Columns.Contains("Notes"))
                        {
                            NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            dt.Columns.Remove("Notes");
                        }
                        rptHTML rptHTML = new rptHTML();
                        XmlDocument xDocOut=new XmlDocument();
                        xDocOut.LoadXml(nodePrintOption.OuterXml.ToString());
                        htmlText = rptHTML.CreateHTMLTemplate(xDocOut, dt, "4", treeNodeName);
                    }
                }
                if (printType == "PRINT")
                {
                    trPrint.Visible = false;
                    trPDF.Visible = false;
                    trHtml.Visible = false;
                    trExcel.Visible = false;

                    pnlCurrentAll.Visible = false;
                    pnlExtendedCols.Visible = false;
                    pnlSavetoMyReports.Visible = false;
                    pnlDistributionList.Visible = false;
                    pnlChkReportRetrieve.Visible = false;
                    pnlPrinting.Visible = true;
                    pnlChooseOptions.Visible = false;
                    pnlImgButtons.Visible = false;
                    pnlTotalPrint.Visible = false;
                    pnlPrintOptionBtn.Visible = false;
                    lblPopupEntry.Text = "";
                    lblPopupEntry.Text = "Print Preveiw";
                    litPrinting.Text = htmlText;
                    //litPrintOptionBtn.Text = htmlText;
                }
                else
                {
                    if (printType == "PAGELOAD")
                    {
                        pnlTotalPrint.Visible = true;
                        pnlPrinting.Visible = false;
                        pnlPrintOptionBtn.Visible = true;

                        trPrint.Visible = true;
                        trPDF.Visible = true;
                        trHtml.Visible = true;
                        trExcel.Visible = true;

                        pnlChooseOptions.Visible = true;
                        lblPopupEntry.Text = "";
                        lblPopupEntry.Text = "Print Preveiw";

                        pnlCurrentAll.Visible = false;
                        pnlExtendedCols.Visible = false;
                        pnlSavetoMyReports.Visible = false;
                        pnlDistributionList.Visible = false;
                        pnlChkReportRetrieve.Visible = false;

                        litPrintOptionBtn.Text = htmlText;
                    }
                }
                //StringWriter stringWrite = new StringWriter();
                //System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
                //string Script = string.Empty;
                //this.ClientScript.RegisterStartupScript(this.GetType(), "PrintJavaScript", Script);
                //HtmlForm frm = new HtmlForm();
                //this.Controls.Add(frm);
                //frm.Attributes.Add("runat", "server");
                ////frm.Controls.Add(ctrl);
                //this.DesignerInitialize();
                ////pg.RenderControl(htmlWrite);
                //string strHTML = stringWrite.ToString();

                if (printType == "PRINT")
                {
                    RegisterStartupScript("Print", "<script>Clickheretoprint();</script>");
                }
            }
        }

        #endregion
    }
}
