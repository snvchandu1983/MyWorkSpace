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
using NLog;


namespace LAjitDev.PopUps
{
    public partial class FramePDF : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        LAjit_BO.Reports objReports = new LAjit_BO.Reports();
        clsReportsUI objReportsUI = new clsReportsUI();
        CommonUI commonObjUI = new CommonUI();
        public CommonBO objBO = new CommonBO();

        public string ReportPrintXML
        {
            get
            {
                return ViewState["ReturnXML"].ToString();

            }
            set
            {
                value = ViewState["ReturnXML"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string theme = Convert.ToString(Session["MyTheme"]);
            imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/submit-but.png";
            string BPInfo = string.Empty;
            AddScriptReferences();
            if (!IsPostBack)
            {
                if (Page.Request.Params["PopUp"] != null && Page.Request.Params["PopUP"].ToString() != string.Empty)
                {
                    switch (Page.Request.Params["PopUP"].ToString())
                    {
                        case "PopUp":
                        case "PE":
                            {
                                if (Session["LinkBPinfo"] != null)
                                {
                                    BPInfo = Session["LinkBPinfo"].ToString();
                                }
                                break;
                            }
                        case "BTN":
                        case "BPP":
                            {
                                if (Session["BPINFO"] != null)
                                {
                                    BPInfo = Session["BPINFO"].ToString();
                                }
                                break;
                            }
                    }
                    BindPage(BPInfo);
                }
            }
            if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
            }
        }

        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //Common.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Common.js");
            Page.Header.Controls.Add(hgcScript1);
        }

        public void BindPage(string SessionBPInfo)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + SessionBPInfo);
            #endregion

            XmlDocument xDoc = new XmlDocument();
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            XmlDocument XDocUserInfo = new XmlDocument();

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

            //generate Request XML 
            strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml), false, "", "", null);
            //  strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Session["BPE"].ToString(), false, "", "", null);
            //BPOUT from DB
            strOutXml = objBO.GetDataForCPGV1(strReqXml);
            xDoc.LoadXml(strOutXml);
            //For successful BPOut
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeErrorLabel = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
            XmlNode nodeOtherInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
            XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            if (nodeMsgStatus.InnerText == "Success")
            {
                if (nodeErrorLabel.InnerText != null)
                {
                    if (nodeErrorLabel.InnerText.ToString().Trim().ToUpper() != "PROCESS COMPLETE.")
                    {
                        pnlTotalPrint.Visible = true;
                        lblMsg.Text = nodeMsgStatus.InnerText + "-" + nodeErrorLabel.InnerText;
                    }
                    else
                    {
                        ViewState["ReturnXML"] = strOutXml.ToString();
                        GenerateReport(strOutXml);
                    }
                }
            }
            else
            {
                pnlTotalPrint.Visible = true;
                if (nodeMsgStatus != null)
                {
                    lblMsg.Text = nodeMsgStatus.InnerText;
                }
                if (nodeErrorLabel != null)
                {
                    lblMsg.Text = lblMsg.Text + "-" + nodeErrorLabel.InnerText;
                }
                if (nodeOtherInfo != null)
                {
                    lblMsg.Text = lblMsg.Text + "." + nodeOtherInfo.InnerText;
                }
            }
        }

        public string GenerateReport(string GVXml)
        {
            #region NLog
            logger.Info("This is used to generate a report based on the GenView XML");
            #endregion

            DataTable NotesDT = new DataTable();
            DataTable dt = new DataTable();
            DataTable pNotesDT = new DataTable();

            int trxID = Convert.ToInt32(Page.Request.Params["TrxID"]);
            int trxType = Convert.ToInt32(Page.Request.Params["TrxType"]);
            string rptType = string.Empty;

            if (Page.Request.Params.ToString().Contains("RptType"))
            {
                rptType = Page.Request.Params["RptType"].ToString();
            }
            else
            {
                rptType = "PDF";
            }
            int reportStyle = 0;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(GVXml);

                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;

                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
                    if (reportStyle == 0)
                    {
                        //For successful BPOut
                        XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                        XmlNode nodeErrorLabel = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                        XmlNode nodeOtherInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                        XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                        //if (nodeMsgStatus.InnerText == "Success")
                        //{
                        //    pnlTotalPrint.Visible = true;
                        //    lblMsg.Text = "Your Process has been Submitted Successfully";
                        //}
                        //else
                        //{
                        pnlTotalPrint.Visible = true;
                        if (nodeMsgStatus != null)
                        {
                            lblMsg.Text = nodeMsgStatus.InnerText;
                        }
                        if (nodeErrorLabel != null)
                        {
                            lblMsg.Text = lblMsg.Text + "-" + nodeErrorLabel.InnerText;
                        }
                        if (nodeOtherInfo != null)
                        {
                            lblMsg.Text = lblMsg.Text + "." + nodeOtherInfo.InnerText;
                        }
                        //}
                    }
                }
                else if (nodeTreenode.Attributes["ReportStyle"] == null)
                {
                    XmlNode isBranches = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree");
                    foreach (XmlNode xnBranches in isBranches.ChildNodes)
                    {
                        if (xnBranches.Name == "Node")
                        {
                            int isPrint = Convert.ToInt32(xDoc.SelectSingleNode("//FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value);
                            if (isPrint == 1)
                            {
                                XmlNode xReportStyleNode = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree");
                                XmlAttribute attrReportStyle = xDoc.CreateAttribute("ReportStyle");
                                if (attrReportStyle.Value != null)
                                {
                                    attrReportStyle.Value = "4";
                                }
                                else
                                {
                                    attrReportStyle.Value = attrReportStyle.Value;
                                }
                                xReportStyleNode.Attributes.Append(attrReportStyle);
                            }
                        }
                    }
                }
                if (reportStyle != 0)
                {
                    string PrefValues = GetUserPreferenceValue("ShowEmailIDS");
                    if (PrefValues != string.Empty)
                    {
                        pnlEmailing.Visible = true;
                        ViewState["ReturnXML"] = null;
                        ViewState["ReturnXML"] = xDoc.OuterXml.ToString();
                    }
                    else
                    {
                        pnlEmailing.Visible = false;
                        ViewState["ReturnXML"] = null;
                        ViewState["ReturnXML"] = xDoc.OuterXml.ToString();
                        XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                        XmlNode nodeErrorLabel = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                        XmlNode nodeOtherInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                        XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                        pnlTotalPrint.Visible = true;
                        if (nodeMsgStatus != null)
                        {
                            lblMsg.Text = nodeMsgStatus.InnerText;
                        }
                        if (nodeErrorLabel != null)
                        {
                            lblMsg.Text = lblMsg.Text + "-" + nodeErrorLabel.InnerText;
                        }
                        if (nodeOtherInfo != null)
                        {
                            lblMsg.Text = lblMsg.Text + "." + nodeOtherInfo.InnerText;
                        }
                        GenerateReport(rptType, "", trxID);
                    }
                }
                else
                {
                    pnlEmailing.Visible = false;
                    ViewState["ReturnXML"] = null;
                    ViewState["ReturnXML"] = xDoc.OuterXml.ToString();
                    GenerateReport(rptType, "", trxID);
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

        /// <summary>
        /// Gets the value of the user preference from the given XML.
        /// </summary>
        /// <param name="ruleName">Preference name as found in the Web.Config file.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        private string GetUserPreferenceValue(string ruleName)
        {
            #region NLog
            logger.Info("Getting the value of the user preference from the given XML with rule name as : " + ruleName);
            #endregion

            //Getting the corresponding rule name ID from the config file.
            string bRuleID = ConfigurationManager.AppSettings[ruleName];
            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            XmlNode xNodeUserPrefValue = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + bRuleID + "']");
            if (xNodeUserPrefValue != null)
            {
                return xNodeUserPrefValue.Attributes["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public void GenerateReport(string rptType, string emailIDS, int trxID)
        {
            #region NLog
            logger.Info("This is used to generate a report based on the report type as " + rptType + " and emailIds : " + emailIDS + " and trxID as : " + trxID);
            #endregion

            DataTable dt = new DataTable();
            DataTable NotesDT = new DataTable();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(ViewState["ReturnXML"].ToString());
            string GVXml = string.Empty;
            string pageNode = xDoc.SelectSingleNode("//FormControls/GridLayout/Tree/Node").InnerText;
            //if (!Page.ToString ( ).Contains ("showpdf"))
            //{
            XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + pageNode + "/RowList");
            if (Page.Request.Params["PopUp"].ToString() == "BTN")
            {
                if (trxID != 0)
                {
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
            }
            //}
            string _flag = string.Empty;
            _flag = Page.Request.Params["PopUp"].ToString();

            if (_flag == "BTN" || _flag == "PE" || _flag == "PopUP")
            {
                _flag = "BPP";
            }
            XmlDocument xPrintOption = new XmlDocument();
            XmlNode nodePrintOption = xPrintOption.CreateNode(XmlNodeType.Element, "Root", null);
            nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
            nodePrintOption.InnerXml += "<PrintOption>" + _flag + "</PrintOption>";
            if (emailIDS != string.Empty)
            {
                nodePrintOption.InnerXml += "<EmailIDS>" + emailIDS + "</EmailIDS>";
            }
            GVXml = nodePrintOption.OuterXml.ToString();

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(GVXml);

            LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
            if (GVXml != string.Empty)
            {
                switch (rptType.ToString().ToUpper())
                {
                    case "PDF":
                        {
                            //objReportsUI.GenerateReport(GVXml);
                            objReportsUI.GenerateReport(xDocOut, "PDF");
                            break;
                        }
                    case "EXCEL":
                        {
                            dt = objReportsUI.XMLToDataTable(GVXml, pageNode, "");
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
                            }
                            //Export to Excel
                            ExportDatatableToExcel(dt, "ExcelRpt", NotesDT, "");
                            break;
                        }
                    case "HTML":
                        {
                            dt = objReportsUI.XMLToDataTable(GVXml, pageNode, "");
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("Notes"))
                                {

                                    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                                    dt.Columns.Remove("Notes");
                                }
                                XmlDocument xDocFull = new XmlDocument();
                                xDocFull.LoadXml(GVXml);
                                rptHTML objHTML = new rptHTML();
                                string htmlText = objHTML.CreateHTMLTemplate(xDocFull, dt, "4", "Html Rpt");
                                littxt.Text = htmlText;
                            }
                            break;
                        }
                }
            }
        }

        protected void imgbtnSubmit_Click(object sender, EventArgs e)
        {
            int trxID = Convert.ToInt32(Page.Request.Params["TrxID"]);
            int trxType = Convert.ToInt32(Page.Request.Params["TrxType"]);
            string gvXML = ViewState["ReturnXML"].ToString();
            string emailIDS = txtEmailing.Text.ToString();
            pnlEmailing.Visible = false;
            GenerateReport("PDF", emailIDS, trxID);
        }

        #region Export To Excel
        public void ExportDatatableToExcel(DataTable dt, string fileName, DataTable NotesDT, string rptPrintNotes)
        {
            #region NLog
            logger.Info("This to export the datatable data to the excel document filename as : "+fileName+" and report print notes : "+rptPrintNotes);
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
    }
}
