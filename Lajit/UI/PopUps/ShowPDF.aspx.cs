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
    public partial class ShowPDF : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        LAjit_BO.Reports objReports = new LAjit_BO.Reports();
        clsReportsUI objReportsUI = new clsReportsUI();
        CommonUI objCommonUI = new CommonUI();
        CommonBO objBO = new CommonBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            string BPInfo = string.Empty;

            AddScriptReferences();

            if (!IsPostBack)
            {
                if (Page.Request.Params["PopUp"] != null)
                {
                    switch (Page.Request.Params["PopUP"].ToString())
                    {
                        case "PopUp":
                            {
                                if (Session["LinkBPinfo"] != null)
                                {
                                    BPInfo = Session["LinkBPinfo"].ToString();
                                }
                                break;
                            }
                        case "BTN":
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
                if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
                {
                    objCommonUI.InjectSessionExpireScript(this);
                }
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


        public void BindPage(string BPInfo)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + BPInfo);
            #endregion

            XmlDocument xDocUserInfo = new XmlDocument();
            xDocUserInfo = objCommonUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            //
            #region REQUEST XML AND OUT XML
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            //Request XML
            strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPInfo, "", "", "", Convert.ToString(xDocUserInfo.SelectSingleNode("Root/bpe").OuterXml), false, "", "", null);
            //Out XML
            strOutXml = objBO.GetDataForCPGV1(strReqXml);
            #endregion
            //
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(strOutXml);
            //
            int reportStyle = 0;
            string treeNodeName = string.Empty;
            string rptType = string.Empty;
            //
            XmlNode nodeTreenode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            XmlNode nodeMsgStatus = xDocOut.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeErrorLabel = xDocOut.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
            XmlNode nodeOtherInfo = xDocOut.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
            XmlNode nodeMsg = xDocOut.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            //
            if (Page.Request.Params.ToString().Contains("RptType"))
            {
                rptType = Page.Request.Params["RptType"].ToString();
            }
            else
            {
                rptType = "PDF";
            }
            //
            #region SUCCESS STATUS
            if (nodeMsgStatus.InnerText == "Success")
            {
                bool m_PageNumber = false;
                bool m_WaterMark = false;
                string m_PageLayout = string.Empty;
                string m_PageSize = string.Empty;
                string m_FontName = string.Empty;
                string m_Password = string.Empty;
                bool m_Annotations = false;
                //
                if (Request.QueryString["PN"] != null)
                {
                    m_PageNumber = Convert.ToBoolean(Request.Params["PN"].ToString());
                }
                if (Request.QueryString["WM"] != null)
                {
                    m_WaterMark = Convert.ToBoolean(Request.Params["WM"].ToString());
                }
                if (Request.QueryString["PL"] != null)
                {
                    m_PageLayout = Request.Params["PL"].ToString();
                }
                if (Request.QueryString["PS"] != null)
                {
                    m_PageSize = Request.Params["PS"].ToString();
                }
                if (Request.QueryString["FN"] != null)
                {
                    m_FontName = Request.Params["FN"].ToString();
                }
                if (Request.QueryString["PD"] != null)
                {
                    m_Password = Request.Params["PD"].ToString();
                }
                if (Request.QueryString["AT"] != null)
                {
                    m_Annotations = Convert.ToBoolean(Request.Params["AT"].ToString());
                }
                //
                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    int trxID = Convert.ToInt32(Page.Request.Params["TrxID"]);
                    string treeNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree/Node").InnerText;
                    XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList");
                    if (nodeRowList != null)
                    {
                        if (nodeRowList.ChildNodes.Count > 0)
                        {
                            if (Page.Request.Params["PopUp"].ToString() == "BTN")
                            {
                                if (trxID != 0)
                                {
                                    XmlNode nodePrint = xDocOut.SelectSingleNode("//" + treeNode + "/RowList/Rows[@TrxID=" + trxID + "]");
                                    XmlNodeList xnl = xDocOut.SelectNodes("//" + treeNode + "/RowList/Rows");
                                    foreach (XmlNode xnls in xnl)
                                    {
                                        if (xnls.Attributes["TrxID"].InnerText != trxID.ToString())
                                        {
                                            nodeRowList.RemoveChild(xnls);
                                        }
                                    }
                                }
                            }
                            reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
                            if (reportStyle == 0)
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
                            else
                            {
                                if (m_FontName != "")
                                {
                                    objReportsUI.Annotaions = m_Annotations;
                                    objReportsUI.FontName = m_FontName;
                                    objReportsUI.PageLayout = m_PageLayout;
                                    objReportsUI.PageNumber = m_PageNumber;
                                    objReportsUI.PageSize = m_PageSize;
                                    objReportsUI.Password = m_Password;
                                    objReportsUI.WaterMark = m_WaterMark;
                                }
                                //
                                objReportsUI.GenerateReport(xDocOut, rptType.ToUpper().ToString());
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
                    else
                    {
                        #region NLog
                        logger.Debug("If there is no root node list."); 
                        #endregion

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
                else if (nodeTreenode.Attributes["ReportStyle"] == null)
                {
                    #region NLog
                    logger.Debug("If there is no report style specified.");
                    #endregion

                    int trxID = Convert.ToInt32(Page.Request.Params["TrxID"]);
                    string treeNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree/Node").InnerText;
                    XmlNode nodeRowList = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList");
                    if (Page.Request.Params["PopUp"].ToString() == "BTN")
                    {
                        if (trxID != 0)
                        {
                            XmlNode nodePrint = xDocOut.SelectSingleNode("//" + treeNode + "/RowList/Rows[@TrxID=" + trxID + "]");
                            XmlNodeList xnl = xDocOut.SelectNodes("//" + treeNode + "/RowList/Rows");
                            foreach (XmlNode xnls in xnl)
                            {
                                if (xnls.Attributes["TrxID"].InnerText != trxID.ToString())
                                {
                                    nodeRowList.RemoveChild(xnls);
                                }
                            }
                        }
                    }
                    XmlNode isBranches = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree");
                    foreach (XmlNode xnBranches in isBranches.ChildNodes)
                    {
                        if (xnBranches.Name == "Node")
                        {
                            XmlNode xReportStyleNode = xDocOut.SelectSingleNode("//FormControls/GridLayout/Tree");
                            XmlAttribute attrReportStyle = xDocOut.CreateAttribute("ReportStyle");
                            if (attrReportStyle.Name.ToString() != string.Empty)
                            {
                                attrReportStyle.Value = "4";
                            }
                            reportStyle = Convert.ToInt32(attrReportStyle.Value.ToString());
                            xReportStyleNode.Attributes.Append(attrReportStyle);
                        }
                    }
                    if (m_FontName != null)
                    {
                        objReportsUI.Annotaions = m_Annotations;
                        objReportsUI.FontName = m_FontName;
                        objReportsUI.PageLayout = m_PageLayout;
                        objReportsUI.PageNumber = m_PageNumber;
                        objReportsUI.PageSize = m_PageSize;
                        objReportsUI.Password = m_Password;
                        objReportsUI.WaterMark = m_WaterMark;
                    }
                    //
                    objReportsUI.GenerateReport(xDocOut, rptType.ToUpper().ToString());
                }
            }
            #endregion
            //
            #region ERROR STATUS
            //Error
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
            #endregion
        }
    }
}

