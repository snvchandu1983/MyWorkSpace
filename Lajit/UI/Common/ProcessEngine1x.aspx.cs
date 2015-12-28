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
using System.Drawing;
using System.Text;
using System.Xml;
using LAjit_BO;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
// Namespace for PDF generation
using Gios.Pdf;

namespace LAjitDev.Common
{
    public partial class ProcessEngine1x : Classes.BasePage
    {
        public LAjitDev.clsReportsUI reportObjUI = new clsReportsUI();
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        LAjitDev.CommonUI objCommonUI = new CommonUI();

        public String RetXML
        {
            get { return (String)ViewState["RetXML"]; }
            set { ViewState["RetXML"] = value; }
        }

        public String OutXML
        {
            get { return (String)Session["OutXML"]; }
            set { Session["OutXML"] = value; }
        }

        public String RwToBeModified
        {
            get { if (hdnRwToBeModified != null) { return hdnRwToBeModified.Value; } else { return null; } }
            set { if (hdnRwToBeModified != null) { hdnRwToBeModified.Value = value; } }
        }

        protected void lnkBtnCloseIFrame_Click(object sender, EventArgs e)
        {
            //Request for BPOut and reload the current page.(ProcessRefresh functionality)
            CommonBO objBO = new CommonBO();
            XmlDocument xDocUserInfo = objCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = xDocUserInfo.SelectSingleNode("Root/bpe").OuterXml;

            //Have to pass Trx Id and trxType.
            XmlDocument xDocRow = new XmlDocument();
            xDocRow.LoadXml(this.RwToBeModified);
            string trxID = xDocRow.DocumentElement.Attributes["TrxID"].Value;
            string trxType = xDocRow.DocumentElement.Attributes["TrxType"].Value;
            objCommonUI.ReloadPage(trxID, trxType, strBPE, (Control)sender);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax.Utility.RegisterTypeForAjax(typeof(ProcessEngine));
            //string theme = Convert.ToString(Session["MyTheme"]);
            //imgbtnSubmit.ImageUrl = "~/App_Themes/" + theme + "/images/submit-but.png";

            //Find out whether the page is being displayed in a popup or not.
            string popUp = Page.Request.QueryString["PopUp"];
            bool isPopUp = false;
            if (!string.IsNullOrEmpty(popUp) && popUp == "PopUp")
            {
                isPopUp = true;
            }
            string BPInfo = string.Empty;
            if (Convert.ToString(Context.Request["hdnBPInfo"]) != null && Convert.ToString(Context.Request["hdnBPInfo"]) != string.Empty)
            {
                BPInfo = Convert.ToString(Context.Request["hdnBPInfo"]);
            }
            else if (isPopUp)
            {
                //only for pages whose master page is 'PopUp', we need to take Session 'LinkBPinfo'.
                if (HttpContext.Current.Session["LinkBPinfo"] != null)
                {
                    //setting the  Session for childpage.
                    BPInfo = HttpContext.Current.Session["LinkBPinfo"].ToString();
                }
            }
            else
            {
                BPInfo = HttpContext.Current.Session["BPinfo"].ToString();
            }
            if (!Page.IsPostBack)
            {
                string treeNodeName = string.Empty;
                string retXML = string.Empty;
                string RwToBeModified = string.Empty;
                reportObjUI.BindPage(BPInfo, pnlContent, out retXML);
                RetXML = retXML;
                OutXML = retXML;
                if (RetXML != null && RetXML != string.Empty)
                {
                    SetLabelMessage(RetXML);
                    //Get the Form-Level BPC links table and add it to the panel in the form.           
                    pnlReport.Controls.Clear();
                    pnlReport.Controls.Add(reportObjUI.CreateProcessLinks(RetXML));
                    //Newly added code on 08-01-2009
                    XmlDocument xDocout = new XmlDocument();
                    xDocout.LoadXml(RetXML);
                    if (xDocout != null)
                    {
                        if (xDocout.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                        {
                            treeNodeName = xDocout.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                        }
                        XmlNode nodeColumns = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                        if (xDocout.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList") != null)
                        {
                            RwToBeModified = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList").InnerXml.ToString();
                            this.RwToBeModified = RwToBeModified;
                        }
                        SetCOXML(xDocout, nodeColumns, treeNodeName, RwToBeModified);
                    }
                }
                //The code for setting UI headers skips as there is no update panel.so set it here
                int entryFormWidth = 818;//TotalWindowWidth - BtnUCWidth -SpacerWidth
                if (pnlEntryForm.Page.MasterPageFile.Contains("PopUp.Master"))
                {
                    entryFormWidth = 910;
                    int entryFormHeight = 500;
                    pnlEntryForm.Width = Unit.Pixel(entryFormWidth);
                    pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
                    tblEntryForm.Style["height"] = entryFormHeight.ToString();
                }
                else
                {
                    //If Left panel is collapsed the width increases by an amount equal to that of Left Panel
                    if (Convert.ToString(Session["LPCollapsed"]) == "1")
                    {
                        entryFormWidth += 149;
                    }
                }
                //if (htcEntryForm.Width == string.Empty)
                //{
                //    htcEntryForm.Width = "0";
                //}
                //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31);//50
            }
        }

        private void SetUIHeaders(XmlDocument xDocout)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected void close_click(object sender, EventArgs e)
        {
            Session.Remove("strOUTXML");
        }

        public void SetLabelMessage(string returnXML)
        {
            XmlDocument xlblDoc = new XmlDocument();
            xlblDoc.LoadXml(returnXML);
            XmlNodeList xlblNodes = xlblDoc.SelectNodes("Root/bpeout/FormControls/GenericProcess/GridHeading/Columns/Col[@ControlType='DDL']");
            ArrayList ar_lblNodes = new ArrayList();
            foreach (XmlNode lblNode in xlblNodes)
            {
                string lblNodeNames = lblNode.Attributes["Label"].Value;
                ar_lblNodes.Add(lblNodeNames);
            }
            if (ar_lblNodes.Count > 0)
            {
                for (int i = 0; i < ar_lblNodes.Count; i++)
                {
                    string arNames = ar_lblNodes[i].ToString();
                    XmlNodeList xlblRowNodes = xlblDoc.SelectNodes("Root/bpeout/FormControls/" + arNames + "/RowList/Rows");
                    if (xlblRowNodes.Count >= 2)
                    {
                        LAjitControls.LAjitDropDownList ddl = new LAjitControls.LAjitDropDownList();
                        ddl = (LAjitControls.LAjitDropDownList)this.Master.FindControl("cphPageContents").FindControl("ddl" + ar_lblNodes[i].ToString());
                        //ddl.ID = "ddl" + ar_lblNodes[i].ToString();
                        ddl.Attributes.Add("OnChange", "javascript:LabelClose();");
                    }
                }
            }
        }

        public void BindPage(string SessionBPInfo, string EmailIDs)
        {
            XmlDocument xDoc = new XmlDocument();
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            //generate Request XML 
            strReqXml = objBO.GenActionRequestXML("PAGELOAD", SessionBPInfo, "", "", "", Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml), false, "", "", null);
            strOutXml = objBO.GetDataForCPGV1(strReqXml);
            xDoc.LoadXml(strOutXml);
            //For successful BPOut
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeErrorLabel = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
            XmlNode nodeOtherInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
            XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            if (nodeMsgStatus.InnerText == "Success")
            {
                GenerateReport("PDF", EmailIDs, strOutXml);
            }
        }

        public void GenerateReport(string reportType, string m_EmailIDs, string outXML)
        {
            int reportStyle = 0;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(outXML);

                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                if (nodeTreenode.Attributes["ReportStyle"] == null)
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
                                attrReportStyle.Value = "4";
                                xReportStyleNode.Attributes.Append(attrReportStyle);
                            }
                        }
                    }
                }
                else
                {
                    reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
                    if (reportStyle == 0)
                    {
                        //For successful BPOut
                        XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                        XmlNode nodeErrorLabel = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                        XmlNode nodeOtherInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                        XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                    }
                    else
                    {
                        XmlDocument xPrintOption = new XmlDocument();
                        XmlNode nodePrintOption = xPrintOption.CreateNode(XmlNodeType.Element, "Root", null);
                        nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
                        nodePrintOption.InnerXml += "<PrintOption>BPP</PrintOption>";
                        if (m_EmailIDs != string.Empty)
                        {
                            nodePrintOption.InnerXml += "<EmailIDS>" + m_EmailIDs + "</EmailIDS>";
                        }
                        clsReportsUI objReportsUI = new clsReportsUI();
                        if (nodePrintOption.OuterXml.ToString() != string.Empty)
                        {
                            objReportsUI.GenerateReport(xPrintOption, "PDF");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// setting rowxml and page controls for hidden field controls
        /// </summary>
        /// <param name="xDocOut"></param>
        /// <param name="nodeColumns"></param>
        /// <param name="TreeNode"></param>
        /// <param name="RwToBeModified"></param>
        public void SetCOXML(XmlDocument xDocOut, XmlNode nodeColumns, string TreeNode, string RwToBeModified)
        {
            //setting Treenode name 
            string m_treeNodeName = TreeNode;
            StringBuilder sbRowXML = new StringBuilder();
            string pageControls = string.Empty;
            string COXML = string.Empty;
            XmlDocument xDocRowInfoXml = new XmlDocument();
            if ((RwToBeModified != null) && (RwToBeModified != string.Empty))
            {
                xDocRowInfoXml.LoadXml(RwToBeModified);
                if (xDocRowInfoXml != null)
                {
                    string TrxID = xDocRowInfoXml.FirstChild.Attributes["TrxID"].Value;
                    string TrxType = xDocRowInfoXml.FirstChild.Attributes["TrxType"].Value;

                    //Retrieving the BPGID and Navigate page from GVDataXML
                    string BPGID = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
                    string pageInfo = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
                    XmlNode nodeRow = xDocRowInfoXml.SelectSingleNode("Rows[@TrxID='" + TrxID + "']");
                    if (nodeColumns.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
                        {
                            if (nodeCol.Attributes["FullViewLength"] != null)
                            {
                                if ((nodeCol.Attributes["IsHidden"].Value != "1") && (nodeCol.Attributes["IsDisplayOnly"].Value != "1"))
                                {
                                    if (nodeRow.Attributes[nodeCol.Attributes["Label"].Value] == null)
                                    {
                                        /* based on control type create attributes */
                                        if (nodeCol.Attributes["ControlType"].Value == "DDL")
                                        {
                                            //GenericSelect1_TrxType="11" GenericSelect2_TrxType="220" 
                                            string DDLTrxID = nodeCol.Attributes["Label"].Value + "_TrxID";
                                            string DDLTrxType = nodeCol.Attributes["Label"].Value + "_TrxType";
                                            if (nodeRow.Attributes[DDLTrxID] == null)
                                            {
                                                XmlAttribute attrNew1 = xDocRowInfoXml.CreateAttribute(DDLTrxID);
                                                attrNew1.Value = "";
                                                nodeRow.Attributes.Append(attrNew1);
                                            }
                                            if (nodeRow.Attributes[DDLTrxType] == null)
                                            {
                                                XmlAttribute attrNew2 = xDocRowInfoXml.CreateAttribute(DDLTrxType);
                                                attrNew2.Value = "";
                                                nodeRow.Attributes.Append(attrNew2);
                                            }
                                        }
                                        else
                                        {
                                            XmlAttribute attrNew = xDocRowInfoXml.CreateAttribute(nodeCol.Attributes["Label"].Value);
                                            attrNew.Value = "";
                                            nodeRow.Attributes.Append(attrNew);
                                        }
                                        //Create Available controls
                                    }
                                    string controlName = string.Empty;
                                    switch (nodeCol.Attributes["ControlType"].Value.Trim())
                                    {
                                        case "TBox":
                                        case "Cal":
                                            controlName = "txt" + nodeCol.Attributes["Label"].Value;
                                            TextBox txtCurrent = (TextBox)this.Master.FindControl("cphPageContents").FindControl(controlName);
                                            if (txtCurrent != null)
                                            {
                                                if (pageControls == string.Empty)
                                                {
                                                    pageControls = txtCurrent.ClientID.ToString();
                                                }
                                                else
                                                {
                                                    pageControls = pageControls + "," + txtCurrent.ClientID.ToString();
                                                }
                                            }
                                            //To Add JQUERY CALENDAR and MASKEditor attribute to get from client side
                                            if (nodeCol.Attributes["ControlType"].Value.Trim() == "Cal")
                                            {
                                                txtCurrent.Attributes.Add("JCal", "mm/dd/y");
                                                txtCurrent.Attributes.Add("JMask", "99/99/99");
                                            }
                                            break;
                                        case "DDL":
                                            controlName = "ddl" + nodeCol.Attributes["Label"].Value;
                                            DropDownList ddlCurrent = (DropDownList)this.Master.FindControl("cphPageContents").FindControl(controlName);
                                            if (ddlCurrent != null)
                                            {
                                                if (pageControls == string.Empty)
                                                {
                                                    pageControls = ddlCurrent.ClientID.ToString();
                                                }
                                                else
                                                {
                                                    pageControls = pageControls + "," + ddlCurrent.ClientID.ToString();
                                                }
                                            }
                                            break;
                                        case "Check":
                                            controlName = "chk" + nodeCol.Attributes["Label"].Value;
                                            CheckBox chkCurrent = (CheckBox)this.Master.FindControl("cphPageContents").FindControl(controlName);
                                            if (chkCurrent != null)
                                            {
                                                if (pageControls == string.Empty)
                                                {
                                                    pageControls = chkCurrent.ClientID.ToString();
                                                }
                                                else
                                                {
                                                    pageControls = pageControls + "," + chkCurrent.ClientID.ToString();
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    //ADD BPACTION AS REPORT
                    XmlAttribute attrBPAction = xDocRowInfoXml.CreateAttribute("BPAction");
                    attrBPAction.Value = "Report";
                    nodeRow.Attributes.Append(attrBPAction);
                    RwToBeModified = nodeRow.OuterXml.ToString();
                    COXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + pageInfo + "</PageInfo><Caption></Caption></CallingObject>";
                }
            }
            sbRowXML.Append("<" + m_treeNodeName + "><RowList>" + RwToBeModified + "</RowList></" + m_treeNodeName + ">");
            hdnGVBPEINFO.Value = sbRowXML.ToString() + COXML;
            hdnControls.Value = pageControls.ToString();
        }
    }
}