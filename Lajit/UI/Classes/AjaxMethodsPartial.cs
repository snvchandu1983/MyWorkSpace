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
using LAjit_BO;

namespace LAjitDev.Classes
{
    public partial class AjaxMethodsPartial
    {
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string GeneratePrintHTML(string BPInfo, string pages, string[] arrColumns)
        {
            BPInfo = Convert.ToString(HttpContext.Current.Session["LinkBPInfo"]);
            string pageSize = "";
            if (pages == "ALL")
            {
                pageSize = "-1";
            }
            LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
            GridReports objGridReports = new GridReports(new CommonUI());
            //objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
            //objGridReports.Theme = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
            string reqXML = objGridReports.GenerateRequestXML(BPInfo, pageSize);
            string BPOut = reportsBO.GetReportBPEOut(reqXML);

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(BPOut);
            string generatedHTML = objGridReports.GenerateHTML(xDocOut, false);
            return generatedHTML;
        }

        /// Set the Session so that it can be accessed in the PrintViewer.aspx
        /// </summary>
        /// <param name="arrCols"></param>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetPrintColumns(string[] arrCols)
        {
            HttpContext.Current.Session["PrintCols"] = arrCols;
        }

        //Ajax Method to Handle the SOXAprrovalImage Click Event
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string SubmitSOXApproval(string rowXML, string BPGID, string ApprovedStatus)
        {
            string treeNode = "Project";
            CommonUI objCommmonUI = new CommonUI();
            LAjit_BO.CommonBO objBO = new LAjit_BO.CommonBO();

            XmlDocument XDocUserInfo = objCommmonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml += strBPE;

            xDocGV.AppendChild(nodeRoot);

            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            XmlNode nodeTree = xDocGV.CreateNode(XmlNodeType.Element, treeNode, null);
            nodeTree.InnerXml += "<RowList>" + rowXML + "</RowList>";
            nodeBPInfo.AppendChild(nodeTree);

            XmlNode nodeRow = xDocGV.SelectSingleNode("Root/bpinfo/" + treeNode + "/RowList/Rows");
            XmlAttribute attIsApproved = nodeRow.Attributes["IsApproved"];
            XmlAttribute SOXStatus = nodeRow.Attributes["SoxApprovedStatus"];

            if (attIsApproved.Value == "0")
            {
                attIsApproved.Value = "1";
            }
            else
            {
                attIsApproved.Value = "0";
            }

            string trxID = xDocGV.SelectSingleNode("Root/bpinfo/" + treeNode + "/RowList/Rows").Attributes["TrxID"].Value;
            string CntrlValues = xDocGV.OuterXml;
            string trxType = xDocGV.SelectSingleNode("Root/bpinfo/" + treeNode + "/RowList/Rows").Attributes["TrxType"].Value;

            string strOutXml = objBO.GetDataForCPGV1(xDocGV.OuterXml);

            XmlDocument outXML = new XmlDocument();
            outXML.LoadXml(strOutXml);

            string Status = outXML.SelectSingleNode("Root/bpeout/MessageInfo/Status").InnerText;
            string LabelInfo = outXML.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;
            string StatusMessage = outXML.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo").InnerText;

            string strStatus = Status + "~" + LabelInfo + " : " + StatusMessage;

            return strStatus;
        }
    }
}
