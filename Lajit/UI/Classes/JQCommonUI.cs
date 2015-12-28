using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Collections;
using LAjitControls.JQGridView;
using System.Drawing;
using System.Collections.Generic;

namespace LAjitDev.Classes
{
    public class JQCommonUI
    {
        public CommonUI commonObjUI = new CommonUI();
        LAjit_BO.CommonBO objBO = new LAjit_BO.CommonBO();
        private HttpContext _Context;
        XmlDocument XDocUserInfo = new XmlDocument();

        private string m_PageSize;

        public string PageSize
        {
            get { return m_PageSize; }
            set { m_PageSize = value; }
        }

        public HttpContext Context
        {
            get { return _Context; }
            set { _Context = value; }
        }

        //Submits the info to the server, get the response and emit it to the client.
        public string SubmitEntry(Hashtable htReqVar, string strSelectedRw, string cntrlVals)
        {
            string strBPAction = htReqVar["BPAction"].ToString();
            string strBPGID = htReqVar["actionBPGID"].ToString();
            string strTreeNode = htReqVar["treeNode"].ToString();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string pageNo = (Context.Request["page"] == null) ? "" : Context.Request["page"];
            string sortBy = (Context.Request["sidx"] == null) ? "" : Context.Request["sidx"];
            string sortOrder = (Context.Request["sord"] == null) ? "" : Context.Request["sord"];
            string strReqXml = "";

            if (HttpContext.Current.Session["LinkBPinfo"] != null && HttpContext.Current.Session["LinkBPinfo"].ToString() != "")
            {
                // Passing the Calling Object as BPINFO
                strReqXml = GenCurrActionRequestXML(strBPAction, strBPGID, cntrlVals, strTreeNode, strSelectedRw, strBPE, true, this.PageSize, pageNo, sortOrder, sortBy, HttpContext.Current.Session["LinkBPinfo"].ToString(), "");
            }
            else
            {
                strReqXml = GenCurrActionRequestXML(strBPAction, strBPGID, cntrlVals, strTreeNode, strSelectedRw, strBPE, true, this.PageSize, pageNo, sortOrder, sortBy, "", "");
            }
            ////string strReqXml = objBO.GenCurrActionRequestXML(strBPAction, strBPGID, cntrlVals, strTreeNode, trxID, trxType, strBPE, true, "10", "1", "", "");
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);

            XmlDocument returnXMLDoc = new XmlDocument();
            returnXMLDoc.LoadXml(strOutXml);

            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(Convert.ToString(Context.Session["LinkBPINFO"]));

            XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("bpinfo//Gridview");
            string GVTreeNodeName = nodeGridView.ParentNode.LocalName;



            string strResult = CreateResponse(returnXMLDoc);

            if (strBPAction == "Find")
            {
                string userdata = setUserData(returnXMLDoc, GVTreeNodeName, Context);
                if (strResult == null || strResult.Contains("Error"))
                {
                    //Handle the Error Nothing Found for the Search operation.
                    strResult = "<RowList>" + userdata + "<PageNo>0</PageNo><Pages>0</Pages><TotalRows>0</TotalRows></RowList>";
                }
                else
                {
                    //Update the local repository if the BPInfo in SessionLinkBPInfo
                    XmlDocument xDocBPInfo = new XmlDocument();
                    xDocBPInfo.LoadXml(strReqXml);
                    XmlNode nodeBpInfo = xDocBPInfo.SelectSingleNode("Root/bpinfo");
                    Context.Session["LinkBPinfo"] = nodeBpInfo.OuterXml;

                    XmlNode nodeGridResults = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/Gridresults");
                    string rowCount = nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value;
                    string totalCount = nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value;

                    double pageCount = Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToInt32(this.PageSize));
                    //string pageNo = Context.Request["page"];
                    XmlNode nodeRowList = returnXMLDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");
                    if (nodeRowList != null)
                    {
                        //string[] arrCheckBoxCols = GetCheckBoxCols(htReqVar["parentCols"].ToString(), GVTreeNodeName);
                        //FormatCheckBoxValues(nodeRowList, arrCheckBoxCols);
                        XmlNode nodePivot = TransposeXMLNode(nodeRowList);
                        //Also add a node search as 1 to let the client-side know that this result set is from a search operation.
                        nodePivot.InnerXml = "<search>1</search>" + userdata + "<PageNo>" + pageNo + "</PageNo><Pages>" + pageCount + "</Pages><TotalRows>" + totalCount + "</TotalRows>" + nodePivot.InnerXml;
                        return nodePivot.OuterXml;
                    }
                }
                //return string.Empty;
            }

            return strResult;
        }

        public string CreateResponse(XmlDocument returnXMLDoc)
        {
            string strResult = "";
            XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeMsg = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            if (nodeMsgStatus != null)
            {
                if (nodeMsgStatus.InnerText == "Error")
                {
                    strResult = "Error";
                    if (nodeMsg != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                        {
                            strResult += " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                    }
                }
                else
                {
                    if (nodeMsgStatus != null && nodeMsg != null)
                    {
                        strResult = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                    }
                    else if (nodeMsgStatus != null)
                    {
                        strResult = nodeMsgStatus.InnerText;
                    }
                }
            }
            return strResult;
        }

        public string GetXMLRow(HttpContext context, Hashtable htReqVar)
        {
            string strColumns = htReqVar["parentCols"].ToString();
            string[] arrControlAttributeColl = GenerateControls(strColumns);

            StringBuilder sbRow = new StringBuilder();
            sbRow.Append("<Rows ");

            for (int i = 0; i < arrControlAttributeColl.Length; i++)
            {
                if (arrControlAttributeColl[i] != null)
                {
                    string[] arrSplit = arrControlAttributeColl[i].Split(',');
                    string keyLabel = arrSplit[7];
                    string value = context.Request.Params[keyLabel];
                    if (!string.IsNullOrEmpty(value))
                    {
                        string controlType = arrSplit[6];
                        string isLink =arrSplit[8];
                        if (controlType == "DDL")
                        {
                            string[] trxIDType = value.Split('~');
                            if (trxIDType[0] != "-1")
                            {
                                sbRow.Append(keyLabel + "_TrxID=\"" + trxIDType[0] + "\" " + keyLabel + "_TrxType=\"" + trxIDType[1] + "\" ");
                            }
                        }
                        else if (controlType == "TBox" && isLink == "1")
                        {
                            //Look for TrxID and TrxType values in the context.
                            string TrxID = context.Request.Params[keyLabel + "_TrxID"];
                            string TrxType = context.Request.Params[keyLabel + "_TrxType"];
                            sbRow.Append(keyLabel + "_TrxID=\"" + TrxID + "\" " + keyLabel + "_TrxType=\"" + TrxType + "\" ");
                        }
                        else
                        {
                            sbRow.Append(keyLabel + "=\"" + value + "\" ");
                        }
                    }
                    else //If the value was not posted even then frame the XML for textboxes which are Searchable.
                    {
                        if (arrSplit[6] == "TBox" && arrSplit[5] == "1")
                        {
                            sbRow.Append(keyLabel + "=\"" + value + "\" ");
                        }
                    }
                }
            }
            sbRow.Append("/>");

            //return "<Rows NumberID=\"100\" />";
            return sbRow.ToString();
        }

        //For Add/Modify/Delete. This is a Post httpmethod.
        public string GetCntrlValues(HttpContext context, Hashtable htReqVar)
        {
            string strFileName = string.Empty;
            string strFileExtension = string.Empty;
            string strFileSize = string.Empty;

            string strBPAction = htReqVar["BPAction"].ToString();
            string strColumns = htReqVar["parentCols"].ToString();
            if (htReqVar["FileName"] != null)
            {
                strFileName = htReqVar["FileName"].ToString();

                if (htReqVar["FileExtension"] != null)
                {
                    strFileExtension = htReqVar["FileExtension"].ToString();
                }

                if (htReqVar["FileSize"] != null)
                {
                    strFileSize = htReqVar["FileSize"].ToString();
                }
            }

            StringBuilder sbNewRow = new StringBuilder();
            sbNewRow.Append("<Rows ");

            if (strColumns == null)
            {
                return string.Empty;
            }
            string[] arrControlAttributeColl = GenerateControls(strColumns);
            //EnterAllUpperCase
            bool convertToUpper = commonObjUI.GetPreferenceValue("16") == "1" ? true : false;

            string[] arrcurrentCol = new string[100];
            for (int i = 0; i < arrControlAttributeColl.Length; i++)
            {
                //arrcurrentCol[2]
                string currentCol = arrControlAttributeColl[i];
                if (currentCol != null)
                {
                    arrcurrentCol = currentCol.Split(',');
                    for (int index = 0; index < context.Request.Form.Count; index++)
                    {
                        if (context.Request.Form.GetKey(index) == "id" && context.Request.Form.GetKey(index) == "oper"
                            && context.Request.Form.GetKey(index) == "Add" && context.Request.Form.GetKey(index) == "Modify"
                            && context.Request.Form.GetKey(index) == "Delete" && context.Request.Form.GetKey(index) == "Find"
                            && context.Request.Form.GetKey(index) == "Node" && context.Request.Form.GetKey(index) == "currRw")
                        {
                        }
                        else
                        {
                            string colName = arrcurrentCol[7].Trim();
                            string colCntrlType = arrcurrentCol[6];
                            if (colName == context.Request.Form.GetKey(index))
                            {
                                switch (colCntrlType)
                                {
                                    case "Cal":
                                    case "Calc":
                                    case "SCalc":
                                    case "TBox":
                                    case "Amount":
                                        {
                                            string txtboxValue = context.Request.Form[index].ToString();
                                            bool IsHelpAuthPage = false;
                                            bool isAutoFill = arrcurrentCol[8] == "1" ? true : false;
                                            string UpperCaseValue = string.Empty;

                                            if (colName == "Size")
                                            {
                                                txtboxValue = strFileSize;
                                            }

                                            // IsNumberic is 1 or ControlType is Amount remove comma characters added on 06-11-08
                                            if (arrcurrentCol[3] == "1" || colCntrlType == "Amount")
                                            {
                                                if (context.Request.Form[index].ToString().Contains(","))
                                                {
                                                    txtboxValue = context.Request.Form[index].ToString().Replace(",", "");
                                                }
                                            }

                                            if (convertToUpper && !IsHelpAuthPage)
                                            {
                                                sbNewRow.Append(colName + "=\"" + commonObjUI.CharactersToHtmlCodes(txtboxValue.ToUpper()) + "\" ");
                                            }
                                            else
                                            {
                                                sbNewRow.Append(colName + "=\"" + commonObjUI.CharactersToHtmlCodes(context.Request.Form[index]) + "\" ");
                                            }

                                            if (isAutoFill)
                                            {
                                                string trxIDKey = colName + "_TrxID";
                                                string trxTypeKey = colName + "_TrxType";
                                                string trxID = context.Request[trxIDKey];
                                                string trxType = context.Request[trxTypeKey];
                                                sbNewRow.AppendFormat("{0}=\"{1}\" {2}=\"{3}\" ", trxIDKey, trxID, trxTypeKey, trxType);
                                            }
                                        }
                                        break;
                                    case "DDL":
                                        {
                                            string ddlRow = string.Empty;
                                            string newTrxIDType = string.Empty;
                                            newTrxIDType = context.Request.Form[index].Trim();

                                            if (newTrxIDType.Length == 0)
                                            {
                                                continue;
                                            }
                                            string[] strarr = newTrxIDType.Split('~');
                                            string trxID = strarr[0].ToString();
                                            string trxType = strarr[1].ToString();

                                            if (strBPAction.ToUpper() != "MODIFY")
                                            {
                                                if (trxID != string.Empty && trxType != string.Empty)
                                                {
                                                    //TrxID should not -1
                                                    if (trxID != "-1")
                                                    {
                                                        ddlRow = colName + "_TrxID=\"" + trxID + "\" "
                                                                + colName + "_TrxType=\"" + trxType + "\" ";
                                                    }
                                                }
                                                else
                                                {
                                                    //ddlRow = colName + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                                }
                                            }
                                            else
                                            {
                                                if (trxID != string.Empty && trxType != string.Empty)
                                                {
                                                    if (trxID == "-1")
                                                    {
                                                        ddlRow = colName + "_TrxID=\"" + string.Empty + "\" "
                                                                + colName + "_TrxType=\"" + string.Empty + "\" ";
                                                    }
                                                    else
                                                    {
                                                        ddlRow = colName + "_TrxID=\"" + trxID + "\"  "
                                                                + colName + "_TrxType=\"" + trxType + "\" ";
                                                    }
                                                }
                                                else
                                                {
                                                    //Appending only text when TrxId and TrxType are empty
                                                    if (trxID == "-1")
                                                    {
                                                        ddlRow = colName + "=\"" + string.Empty + "\" ";
                                                    }
                                                    else
                                                    {
                                                        //ddlRow = colName + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                                    }
                                                }
                                            }
                                            if (ddlRow != string.Empty)
                                            {
                                                sbNewRow.Append(ddlRow);
                                            }
                                        }
                                        break;
                                    case "Check":
                                        {
                                            string cbValue = context.Request.Form[index];
                                            sbNewRow.Append(colName + "=\"" + cbValue + "\" ");
                                        }
                                        break;

                                    case "File":
                                        {
                                            if (strFileName != string.Empty && strFileSize != string.Empty && strFileExtension != string.Empty)
                                            {
                                                sbNewRow.Append("FileName=" + "\"" + strFileName + strFileExtension + "\"" + " ");
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }


                    }
                }
            }
            sbNewRow.Append("/>");
            return sbNewRow.ToString();
        }

        //Taking all the basic info into hashtable which can be reused.
        public Hashtable InitializeReqVar(HttpContext context)
        {
            Hashtable htReqVal = new Hashtable();
            //To get the current action
            string strBPGID = string.Empty;
            string strBPAction = string.Empty;
            string keyAction = string.Empty;
            if (context.Request["oper"] != null)
            {
                if (context.Request["status"] != null && context.Request["status"] == "CLONE")
                {
                    keyAction = "ADD";
                    //strBPAction = "Add";
                    //strBPGID = context.Request["Add"].ToString();
                }
                else
                {
                    if (context.Request["status"] != null && context.Request["status"] == "FIND")
                    {
                        keyAction = "TRUE"; //for search val will be true/false
                    }
                    else
                    {
                        keyAction = context.Request["oper"];
                    }
                }
            }
            else
            {
                keyAction = "TRUE"; //for search val will be true/false
            }

            switch (keyAction.ToUpper())
            {
                case "EDIT":
                    {
                        strBPAction = "Modify";
                        strBPGID = context.Request["Modify"].ToString();
                    }
                    break;
                case "ADD":
                    {
                        strBPAction = "Add";
                        strBPGID = context.Request["Add"].ToString();
                    }
                    break;
                case "DEL":
                    {
                        strBPAction = "Delete";
                        strBPGID = context.Request["Delete"].ToString();
                    }
                    break;
                case "TRUE"://for search val will be true/false
                    {
                        strBPAction = "Find";
                        strBPGID = context.Request["Find"].ToString();
                    }
                    break;
                default:
                    break;
            }
            htReqVal.Add("BPAction", strBPAction);
            htReqVal.Add("actionBPGID", strBPGID);
            htReqVal.Add("treeNode", context.Request["Node"].ToString());
            htReqVal.Add("parentCols", context.Request["parentCols"].ToString());

            if (context.Request["selectedRw"] != null)
            {
                htReqVal.Add("selectedRw", context.Request["selectedRw"].ToString());
            }

            if (context.Request["FileName"] != null)
            {
                htReqVal.Add("FileName", context.Request["FileName"].ToString());

                if (context.Request["FileExtension"] != null)
                {
                    htReqVal.Add("FileExtension", context.Request["FileExtension"].ToString());
                }

                if (context.Request["FileSize"] != null)
                {
                    htReqVal.Add("FileSize", context.Request["FileSize"].ToString());
                }
            }

            return htReqVal;
        }

        //For Find. This is a Get httpmethod.
        public string GetCntrlValuesForFind(HttpContext context, string strBPAction)
        {
            string ReqXMl = string.Empty;

            XmlDocument xDocRowList = new XmlDocument();
            XmlNode nodeRowList = xDocRowList.CreateNode(XmlNodeType.Element, "RowList", null);

            string BPGID = context.Request["Find"].ToString();
            StringBuilder sbNewRow = new StringBuilder();

            //XmlAttribute findBPGID = xDocRowList.CreateAttribute("BPGID");
            //findBPGID.Value = BPGID;
            //nodeRowsList.Attributes.Append(findBPGID);

            string strfilters = context.Request.QueryString["filters"].ToString();

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            SearchFilter objSearchFilters = oSerializer.Deserialize<SearchFilter>(strfilters);

            //XmlAttribute findOperation = xDocRowList.CreateAttribute("MatchCase");
            //findOperation.Value = objSearchFilters.GroupOp;
            //nodeRow.Attributes.Append(findOperation);

            XmlNode nodeRow = xDocRowList.CreateNode(XmlNodeType.Element, "Rows", null);

            for (int filterCount = 0; filterCount < objSearchFilters.Rules.Length; filterCount++)
            {
                string opCondition = string.Empty;
                opCondition = objSearchFilters.Rules[filterCount].Op;
                char param = Convert.ToChar("~");
                string[] fieldData = objSearchFilters.Rules[filterCount].Data.Split(param);

                if (fieldData.Length > 1)
                {
                    //for dropdowns
                    XmlAttribute fieldTrxID = null;
                    XmlAttribute fieldTrxType = null;

                    if (nodeRow.Attributes[objSearchFilters.Rules[filterCount].Field + "_TrxID"] == null)
                    {
                        fieldTrxID = xDocRowList.CreateAttribute(objSearchFilters.Rules[filterCount].Field + "_TrxID");
                        fieldTrxType = xDocRowList.CreateAttribute(objSearchFilters.Rules[filterCount].Field + "_TrxType");
                    }
                    else
                    {
                        fieldTrxID = nodeRow.Attributes[objSearchFilters.Rules[filterCount].Field + "_TrxID"];
                        fieldTrxType = nodeRow.Attributes[objSearchFilters.Rules[filterCount].Field + "_TrxType"];
                    }

                    if (objSearchFilters.Rules[filterCount].Op == "eq")
                    {
                        fieldTrxID.Value = fieldData[0].ToString();
                        fieldTrxType.Value = fieldData[1].ToString();
                    }
                    else
                    {
                        fieldTrxID.Value = "<>" + fieldData[0].ToString();
                        fieldTrxType.Value = "<>" + fieldData[1].ToString();
                    }

                    nodeRow.Attributes.Append(fieldTrxID);
                    nodeRow.Attributes.Append(fieldTrxType);
                }

                else
                {
                    XmlAttribute fieldName = null;

                    if (nodeRow.Attributes[objSearchFilters.Rules[filterCount].Field] == null)
                    {
                        fieldName = xDocRowList.CreateAttribute(objSearchFilters.Rules[filterCount].Field);
                    }
                    else
                    {
                        fieldName = nodeRow.Attributes[objSearchFilters.Rules[filterCount].Field];
                    }

                    switch (opCondition)
                    {
                        case "cn":
                            {
                                fieldName.Value = "%" + objSearchFilters.Rules[filterCount].Data + "%";
                                break;
                            }

                        case "bw":
                            {
                                fieldName.Value = "%" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "eq":
                            {
                                fieldName.Value = objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "ne":
                            {
                                fieldName.Value = "<>" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "lt":
                            {
                                fieldName.Value = "<" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "le":
                            {
                                fieldName.Value = "<=" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "gt":
                            {
                                fieldName.Value = ">" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "ge":
                            {
                                fieldName.Value = ">=" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "ew":
                            {
                                fieldName.Value = objSearchFilters.Rules[filterCount].Data + "%";
                                break;
                            }
                        case "bn":
                            {
                                fieldName.Value = "<> %" + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "nc":
                            {
                                fieldName.Value = "<> %" + objSearchFilters.Rules[filterCount].Data + "%";
                                break;
                            }
                        case "en":
                            {
                                fieldName.Value = "<> " + objSearchFilters.Rules[filterCount].Data + "%";
                                break;
                            }
                        case "in":
                            {
                                fieldName.Value = "in " + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                        case "ni":
                            {
                                fieldName.Value = "not in " + objSearchFilters.Rules[filterCount].Data;
                                break;
                            }
                    }

                    nodeRow.Attributes.Append(fieldName);
                }

                #region Commented Code
                //XmlAttribute searchOperand = xDocRowList.CreateAttribute("Operand");

                //if (objSearchFilters.Rules[filterCount].Op == "cn")
                //    searchOperand.Value = "contains";

                //if (objSearchFilters.Rules[filterCount].Op == "bw")
                //    searchOperand.Value = "beginswith";

                //if (objSearchFilters.Rules[filterCount].Op == "eq")
                //    searchOperand.Value = "equals";

                //if (objSearchFilters.Rules[filterCount].Op == "ne")
                //    searchOperand.Value = "notequals";

                //if (objSearchFilters.Rules[filterCount].Op == "lt")
                //    searchOperand.Value = "lessthan";

                //if (objSearchFilters.Rules[filterCount].Op == "gt")
                //    searchOperand.Value = "greaterthan";

                //if (objSearchFilters.Rules[filterCount].Op == "ew")
                //    searchOperand.Value = "endswith";

                //nodeRow.Attributes.Append(searchOperand);
                #endregion
            }

            XmlAttribute findBPAction = xDocRowList.CreateAttribute("BPAction");
            findBPAction.Value = strBPAction;
            nodeRow.Attributes.Append(findBPAction);

            nodeRowList.AppendChild(nodeRow);

            #region Commented Code
            //for (int i = 0; i < arrControlAttributeColl.Length; i++)
            //{
            //    //arrcurrentCol[2]
            //    string currentCol = arrControlAttributeColl[i];
            //    if (currentCol != null)
            //    {
            //        arrcurrentCol = currentCol.Split(',');
            //        string colLabel = arrcurrentCol[7].Trim();
            //        string colCntrlType = arrcurrentCol[6];

            //        if (objSearchFilters.Rules[0].Field == colLabel)
            //        {
            //            string op = string.Empty;
            //            if (objSearchFilters.Rules[0].Op == "eq")
            //            {
            //                op = "=";
            //            }
            //            else if (objSearchFilters.Rules[0].Op == "")
            //            {
            //                op = "";//handle different op types
            //            }

            //            switch (colCntrlType)
            //            {
            //                case "TBox":
            //                    {
            //                        string txtboxValue = objSearchFilters.Rules[0].Data.TrimEnd().TrimStart().ToString();
            //                        bool AutoFillStatus = false;
            //                        bool IsHelpAuthPage = false;
            //                        string UpperCaseValue = string.Empty;

            //                        if (arrcurrentCol[3] != "-1")
            //                        {
            //                            // IsNumberic is 1 or ControlType is Amount remove comma characters added on 06-11-08
            //                            if (arrcurrentCol[3] == "1" || arrcurrentCol[3] == "Amount")
            //                            {
            //                                if (objSearchFilters.Rules[0].Data.Contains(","))
            //                                {
            //                                    txtboxValue = objSearchFilters.Rules[0].Data.Replace(",", "");
            //                                }
            //                            }
            //                        }
            //                        ////AutoFill Textboxes IsLink="1" 
            //                        //XmlAttribute attrIsLink = nodecols.Attributes["IsLink"];
            //                        //if (attrIsLink != null)
            //                        //{
            //                        //    if (attrIsLink.Value == "1")
            //                        //    {
            //                        //        string autofillrow = string.Empty;
            //                        //        autofillrow = GetNewAutoFillRow(txtCurrent.MapXML, txtCurrent.Text, nodecols);
            //                        //        if (autofillrow != string.Empty)
            //                        //        {
            //                        //            AutoFillStatus = true;
            //                        //            sbNewRowXML.Append(autofillrow);
            //                        //        }
            //                        //    }
            //                        //}
            //                        if (!AutoFillStatus)
            //                        {
            //                            //Check Entry To Upper key                                                
            //                            UpperCaseValue = commonObjUI.GetPreferenceValue("EnterAllUpperCase");
            //                            if (UpperCaseValue == "1" && !IsHelpAuthPage)
            //                            {
            //                                sbNewRow.Append(colLabel + op + "\"" + commonObjUI.CharactersToHtmlCodes(txtboxValue.ToUpper()) + "\" ");
            //                            }
            //                            else
            //                            {
            //                                sbNewRow.Append(colLabel + op + "\"" + commonObjUI.CharactersToHtmlCodes(txtboxValue) + "\" ");
            //                            }
            //                        }
            //                    }
            //                    break;
            //                case "DDL":
            //                    {
            //                        string ddlRow = string.Empty;
            //                        string newTrxIDType = string.Empty;
            //                        newTrxIDType = objSearchFilters.Rules[0].Data.Trim();
            //                        if (newTrxIDType.Length == 0)
            //                        {
            //                            continue;
            //                        }
            //                        string[] strarr = newTrxIDType.Split('~');
            //                        string trxID = strarr[0].ToString();
            //                        string trxType = strarr[1].ToString();

            //                        if (strBPAction.ToUpper() != "MODIFY")
            //                        {
            //                            if (trxID != string.Empty && trxType != string.Empty)
            //                            {
            //                                //TrxID should not -1
            //                                if (trxID != "-1")
            //                                {
            //                                    ddlRow = colLabel + "_TrxID" + op + "\"" + trxID + "\"  "
            //                                            + colLabel + "_TrxType" + op + "\"" + trxType + "\"  ";
            //                                }
            //                            }
            //                            else
            //                            {
            //                                //ddlRow = colLabel + op + "\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (trxID != string.Empty && trxType != string.Empty)
            //                            {
            //                                if (trxID == "-1")
            //                                {
            //                                    ddlRow = colLabel + "_TrxID" + op + "\"" + string.Empty + "\"  "
            //                                            + colLabel + "_TrxType" + op + "\"" + string.Empty + "\"  ";
            //                                }
            //                                else
            //                                {
            //                                    ddlRow = colLabel + "_TrxID" + op + "\"" + trxID + "\"  "
            //                                            + colLabel + "_TrxType" + op + "\"" + trxType + "\"  ";
            //                                }
            //                            }
            //                            else
            //                            {
            //                                //Appending only text when TrxId and TrxType are empty
            //                                if (trxID == "-1")
            //                                {
            //                                    ddlRow = colLabel + op + "\"" + string.Empty + "\"  ";
            //                                }
            //                                else
            //                                {
            //                                    //ddlRow = colLabel + op + "\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
            //                                }
            //                            }
            //                        }
            //                        if (ddlRow != string.Empty)
            //                        {
            //                            sbNewRow.Append(ddlRow);
            //                        }
            //                    }
            //                    break;
            //                case "CheckBox":
            //                    {
            //                        //if (chkbxCurrent.Checked)//need to check
            //                        //{
            //                        //    //True
            //                        //    sbNewRow.Append(colName + "=\"1\" ");
            //                        //}
            //                        //else
            //                        //{   //False
            //                        //    sbNewRow.Append(colName + "=\"0\" ");
            //                        //}
            //                    }
            //                    break;
            //                default:
            //                    break;
            //            }

            //        }
            //    }
            //}
            #endregion

            //nodeRowsList.Append("/>");
            return nodeRowList.InnerXml.ToString();
            //return nodeRowList.OuterXml.ToString();
        }

        public XmlNode TransposeXMLNode(XmlNode inputnode)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode nodeRowlist = xDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            foreach (XmlNode rowNode in inputnode.ChildNodes)
            {
                XmlNode nodeRows = xDoc.CreateNode(XmlNodeType.Element, "Rows", null);
                XmlAttributeCollection attcolRow = rowNode.Attributes;
                for (int i = 0; i < attcolRow.Count; i++)
                {
                    if (attcolRow[i].Name.ToString() != null)
                    {
                        XmlNode childRow = xDoc.CreateNode(XmlNodeType.Element, attcolRow[i].Name.ToString(), null);
                        childRow.InnerText = attcolRow[i].Value.ToString();
                        nodeRows.AppendChild(childRow);
                    }
                }

                XmlAttribute attcol = xDoc.CreateAttribute("Id");
                attcol.Value = rowNode.Attributes["TrxID"].Value + "~" + rowNode.Attributes["TrxType"].Value;
                nodeRows.Attributes.Append(attcol);
                nodeRowlist.AppendChild(nodeRows);
            }
            return nodeRowlist;
        }

        //To reverse the xmlnode which is transposed
        public XmlNode ReverseXMLNode(XmlNode inputnode)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode nodeRowlist = xDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            XmlNode nodeRows = xDoc.CreateNode(XmlNodeType.Element, "Rows", null);
            foreach (XmlNode rowNode in inputnode.ChildNodes)
            {
                XmlAttribute attcol = xDoc.CreateAttribute(rowNode.Name);
                attcol.Value = rowNode.InnerText;
                nodeRows.Attributes.Append(attcol);
            }
            nodeRowlist.AppendChild(nodeRows);
            return nodeRowlist;
        }

        //Generate an array of control cntrlId collection along with attributes.
        public string[] GenerateControls(string controlCollection)
        {
            //controlCollection would store the parent and child controls along with the columns

            // This would store an array of Parent and Branches along with the column attributes.
            string[] arrParentChild = controlCollection.Split('>');

            // This would store an array of control ids along with the attributes.
            string[] arrControlAttributeColl = new string[100];
            int cntr = 0;
            for (int i = 0; i < arrParentChild.Length; i++) //ArrayParentChild
            {
                // This array size is always two. The first element stores the Node name and control type. Teh second element stores the corresponsing column info for that particular node.
                string[] arrNodeControlColmuns = arrParentChild[i].Split(';');
                //This array will store the Parent ,branch and child name along with the Control Type
                string[] NodeControl = arrNodeControlColmuns[0].Split('/');

                string currBranchNodeName = NodeControl[0]; //This stores the Node name
                string brnchCtrlType = NodeControl[1]; //control type

                string arrCurrNodeCols = arrNodeControlColmuns[1];

                // This array would store the list of controls as well as their attributes for each nodeof the OUT xml.
                string[] arrCols = arrCurrNodeCols.Split(':');

                for (int j = 0; j < arrCols.Length - 1; j++)//currentnodecolumns
                {
                    string[] arrColAttr = arrCols[j].Split('~');

                    //arrColAttr[2]-->IsDefault Attribute
                    //arrColAttr[3]-->IsDisplayOnly Attribute, values 0/1
                    //arrColAttr[4]-->IsRequired Attribute, values 0/1
                    //arrColAttr[5]-->IsNumeric Attribute, values 0/1
                    //arrColAttr[6]-->IsHidden Attribute, values 0/1
                    //arrColAttr[7]-->IsSearched Attribute, values 0/1                   
                    //arrColAttr[1]-->Control Type Attribute
                    //arrColAttr[0]-->Label Attribute
                    //arrColAttr[8]-->IsLink Attribute

                    arrControlAttributeColl[cntr] = arrColAttr[2] + "," + arrColAttr[3] + "," + arrColAttr[4] + "," + arrColAttr[5] + "," + arrColAttr[6] + "," + arrColAttr[7] + "," + arrColAttr[1] + "," + arrColAttr[0] + "," + arrColAttr[8];
                    cntr++;

                }//for currentnodecolumns
            } //for ArrayParentChild  

            //arrControlAttributeColl[0]-->IsDefault Attribute
            //arrControlAttributeColl[1]-->IsDisplayOnly Attribute, values 0/1
            //arrControlAttributeColl[2]-->IsRequired Attribute, values 0/1
            //arrControlAttributeColl[3]-->IsNumeric Attribute, values 0/1
            //arrControlAttributeColl[4]-->IsHidden Attribute, values 0/1
            //arrControlAttributeColl[5]-->IsSearched Attribute, values 0/1
            //arrControlAttributeColl[6]-->Control Type Attribute   
            //arrControlAttributeColl[7]-->Label Attribute    
            //arrControlAttributeColl[8]-->IsLink Attribute

            return arrControlAttributeColl;
        }

        #region CommentedCode "GenActionRequestXML"
        //private string GenActionRequestXML(string TreeNodeName, string BPEAction, string BPValue, string trxId, string trxType, string CntrlValues)
        //{
        //    string BPE = HttpContext.Current.Session["BPE"].ToString();
        //    string ReqXMl = string.Empty;
        //    try
        //    {
        //        XmlDocument xDocGV = new XmlDocument();
        //        XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
        //        nodeRoot.InnerXml += BPE;
        //        if (BPEAction != null)
        //        {
        //            //Creating the bpinfo node
        //            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
        //            nodeRoot.AppendChild(nodeBPInfo);

        //            //Creating the BPGID node
        //            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
        //            nodeBPGID.InnerText = BPValue;
        //            nodeBPInfo.AppendChild(nodeBPGID);
        //            if (BPEAction != "")
        //            {
        //                XmlNode nodeAccountingLayout = xDocGV.CreateNode(XmlNodeType.Element, TreeNodeName, null);
        //                nodeBPInfo.AppendChild(nodeAccountingLayout);
        //                //Creating the Rowlist node
        //                XmlNode nodeRowList = xDocGV.CreateNode(XmlNodeType.Element, "RowList", null);

        //                if ((BPEAction.ToUpper().Trim() == "ADD") || (BPEAction.ToUpper().Trim() == "FIND"))
        //                {
        //                    xDocGV.AppendChild(nodeRowList);
        //                    nodeRowList.InnerXml = CntrlValues;

        //                    XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
        //                    attrBPAction.Value = BPEAction;
        //                    nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
        //                }
        //                else if ((BPEAction.ToUpper().Trim() == "MODIFY") || ((BPEAction.ToUpper().Trim() == "DELETE")))
        //                {
        //                    xDocGV.AppendChild(nodeRowList);
        //                    nodeRowList.InnerXml = CntrlValues;

        //                    XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
        //                    attrBPAction.Value = BPEAction;
        //                    nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
        //                }
        //                nodeAccountingLayout.AppendChild(nodeRowList);
        //            }
        //        }
        //        else
        //        {
        //            nodeRoot.InnerXml += BPValue.ToString();
        //        }
        //        ReqXMl = nodeRoot.OuterXml;
        //    }
        //    catch
        //    {

        //    }
        //    return ReqXMl;
        //}

        #endregion

        //This UserData can be accessed in PostBack.
        //Contains basic required information like treenode,cols info,BPGIDs of Add/Modify/Delete/Find
        public string setUserData(XmlDocument xDoc, string GVTreeNodeName, HttpContext context)
        {
            string AddBPGID = string.Empty;
            string ModifyBPGID = string.Empty;
            string DeleteBPGID = string.Empty;
            string FindBPGID = string.Empty;
            string strParentColNode = string.Empty;

            if (context != null)
            {
                AddBPGID = context.Request.Params["Add"];
                ModifyBPGID = context.Request.Params["Modify"];
                DeleteBPGID = context.Request.Params["Delete"];
                FindBPGID = context.Request.Params["Find"];
                strParentColNode = context.Request.Params["parentcols"];
            }
            else
            {
                XmlNode nodeWork = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Add']");
                if (nodeWork != null)
                {
                    AddBPGID = nodeWork.Attributes["BPGID"].Value;
                }
                nodeWork = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Modify']");
                if (nodeWork != null)
                {
                    ModifyBPGID = nodeWork.Attributes["BPGID"].Value;
                }
                nodeWork = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Delete']");
                if (nodeWork != null)
                {
                    DeleteBPGID = nodeWork.Attributes["BPGID"].Value;
                }
                nodeWork = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Find']");
                if (nodeWork != null)
                {
                    FindBPGID = nodeWork.Attributes["BPGID"].Value;
                }

                strParentColNode = this.GetPageColumns(xDoc);
            }
            //string strFormLevelLinks = GetBusinessProcessLinksTable(xDoc);

            string userdata = "<userdata name=\"Node\">" + GVTreeNodeName + "</userdata><userdata name=\"Add\">" + AddBPGID + "</userdata><userdata name=\"Delete\">" + DeleteBPGID + "</userdata><userdata name=\"Modify\">" + ModifyBPGID
                            + "</userdata><userdata name=\"Find\">" + FindBPGID + "</userdata><userdata name=\"parentCols\">" + strParentColNode + "</userdata>";
            //<userdata name=\"FormLevelLinks\">" + strFormLevelLinks + "</userdata>";

            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            //Append column sums of the Amount/Calc columns
            XmlNodeList xnlAmounts = xDoc.SelectNodes("Root/bpeout/FormControls/" + GVTreeNodeName + "/GridHeading/Columns//Col[@ControlType='Amount' or @ControlType='Calc' or @ControlType='SCalc']");
            //Do summing here.
            XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");
            if (nodeRowList != null)
            {
                Dictionary<string, double> dictColSums = GetColSum(xnlAmounts, nodeRowList);

                foreach (XmlNode node in xnlAmounts)
                {
                    string label = node.Attributes["Label"].Value;
                    userdata += "<userdata name=\"" + label + "\">" + dictColSums[label].ToString() + "</userdata>";
                }
            }
            //watch.Stop();
            //System.Diagnostics.Debugger.Log(1, "Amounts Summation", watch.ElapsedMilliseconds.ToString() + "ms");

            return userdata;
        }

        private Dictionary<string, double> GetColSum(XmlNodeList xnlAmounts, XmlNode nodeRowList)
        {
            Dictionary<string, double> dictColSums = new Dictionary<string, double>();
            foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
            {
                foreach (XmlNode nodeCol in xnlAmounts)
                {
                    string label = nodeCol.Attributes["Label"].Value;
                    if (!dictColSums.ContainsKey(label))
                    {
                        dictColSums[label] = 0;
                    }
                    if (nodeRow.Attributes[label] != null)
                    {
                        dictColSums[label] = dictColSums[label] + Convert.ToDouble(nodeRow.Attributes[label].Value);
                    }
                }
            }
            return dictColSums;
        }

        public string GetPageColumns(XmlDocument xDocForm)
        {
            string strPageControls = string.Empty;
            XmlNode nodeGridLayOut = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
            {
                string treeNodeName = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                //Go the tree node and pick up the columns node.
                XmlNode nodeTreeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                strPageControls = FindPageColumns(nodeTreeColumns, null, string.Empty);

                //XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    //int grdVwCntr = 0;
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlAttribute attControlType = nodeBranch.Attributes["ControlType"];
                        if (attControlType != null && attControlType.Value == "GView")
                        {
                            //Add the GridView exclusively.
                            //string controlType = attControlType.Value;
                            //GridView grdVw = (GridView)container.FindControl("grdVw" + branchNodeName);
                            //if (grdVw != null)
                            //{
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, attControlType.Value);
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                            //}
                        }
                        else
                        {
                            //Rest of the Branch Controls.
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, string.Empty);
                            //Add the above Branch controls to the Master Page Controls Container.
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                        }
                    }
                }
            }
            return strPageControls;
        }

        public string FindPageColumns(XmlNode nodeColumns, string branchNodeName, string brnchCtrlType)
        {
            string formCol = string.Empty;
            string strCol = string.Empty;
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                XmlAttribute attrLabel = nodeCol.Attributes["Label"];
                XmlAttribute attrControlType = nodeCol.Attributes["ControlType"];
                XmlAttribute attrDefault = nodeCol.Attributes["Default"];
                XmlAttribute attrDisplayOnly = nodeCol.Attributes["IsDisplayOnly"];
                XmlAttribute attrIsRequired = nodeCol.Attributes["IsRequired"];
                XmlAttribute attrIsNumeric = nodeCol.Attributes["IsNumeric"];
                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                XmlAttribute attrIsSearched = nodeCol.Attributes["IsSearched"];
                XmlAttribute attIsLink = nodeCol.Attributes["IsLink"];
                if (attrLabel != null)
                {
                    formCol = attrLabel.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrControlType != null)
                {
                    commonObjUI.FormatDefaultAttributes(nodeCol);
                    formCol = formCol + attrControlType.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrDefault != null)
                {
                    formCol = formCol + attrDefault.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrDisplayOnly != null)
                {
                    formCol = formCol + attrDisplayOnly.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrIsRequired != null)
                {
                    formCol = formCol + attrIsRequired.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrIsNumeric != null)
                {
                    formCol = formCol + attrIsNumeric.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrIsHidden != null)
                {
                    formCol = formCol + attrIsHidden.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attrIsSearched != null)
                {
                    formCol = formCol + attrIsSearched.Value + "~";
                }
                else
                {
                    formCol = formCol + "-1" + "~";
                }
                if (attIsLink != null)
                {
                    formCol += attIsLink.Value + "~";
                }
                else
                {
                    formCol += "-1~";
                }
                strCol = strCol + formCol + ":";
            }
            return branchNodeName + "/" + brnchCtrlType + ";" + strCol;
        }

        public string GetBusinessProcessLinksTable(XmlDocument xDoc)
        {
            try
            {
                //Check for pageXML to be null.
                //if (pageXML == null)
                //{
                //    //return new Table();
                //}
                //XmlDocument xDoc = new XmlDocument();
                //xDoc.LoadXml(pageXML);
                XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                if (nodeBPC == null)
                {
                    //No links to add so return an empty table.
                    return string.Empty;
                }

                //Store all the processes assigned to columns and the Buttons User Control
                ArrayList arrColumnProcs = new ArrayList();
                arrColumnProcs.Add("Add");
                arrColumnProcs.Add("Modify");
                arrColumnProcs.Add("Delete");
                arrColumnProcs.Add("Find");
                arrColumnProcs.Add("AutoSave");
                arrColumnProcs.Add("SOXApproval");
                arrColumnProcs.Add("VendorSelect");
                arrColumnProcs.Add("CustomerSelect");
                arrColumnProcs.Add("AutofillBPGID1");
                arrColumnProcs.Add("AutofillBPGID2");

                //Get the relevant branch node.For BPCIcon functionality
                string BPCBranchName = string.Empty;

                XmlNode nodeGridLayOut = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
                {
                    string treeNodeName = nodeTree.SelectSingleNode("Node").InnerText;
                    //Go the tree node and pick up the columns node.
                    XmlNode nodeTreeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                    foreach (XmlNode nodeCol in nodeTreeColumns.ChildNodes)
                    {
                        XmlAttribute attBPControl = nodeCol.Attributes["BPControl"];
                        //Get all the processes list assigned to the columns except Process BPGID
                        if (attBPControl != null && attBPControl.Value.Trim() != string.Empty
                                            && !attBPControl.Value.Contains("BPGID"))
                        {
                            arrColumnProcs.Add(attBPControl.Value.Trim());
                        }
                    }

                    XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
                            {
                                XmlAttribute attBPControl = nodeCol.Attributes["BPControl"];
                                if (attBPControl != null && attBPControl.Value.Trim() != string.Empty
                                            && !attBPControl.Value.Contains("BPGID"))
                                {
                                    arrColumnProcs.Add(attBPControl.Value.Trim());
                                }
                            }

                            if (BPCBranchName.Length == 0)
                            {
                                if (nodeBranch.Attributes["ControlType"] == null)
                                {
                                    BPCBranchName = branchNodeName;
                                }
                            }
                        }
                    }
                }

                ArrayList arrFormLevelProcs = new ArrayList();
                string strFormLevelLinks = string.Empty;
                string formCol = string.Empty;
                foreach (XmlNode nodeProcess in nodeBPC.ChildNodes)
                {
                    string processName = nodeProcess.Attributes["ID"].Value.Trim();
                    if (!arrColumnProcs.Contains(processName))
                    {
                        arrFormLevelProcs.Add(processName);
                    }
                }

                for (int procCntr = 0; procCntr < arrFormLevelProcs.Count; procCntr++)
                {
                    string process = Convert.ToString(arrFormLevelProcs[procCntr]);
                    XmlNode nodeProc = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + process + "']");
                    XmlAttribute currentBPGID = nodeProc.Attributes["BPGID"];
                    XmlAttribute pageInfo = nodeProc.Attributes["PageInfo"];
                    XmlAttribute processName = nodeProc.Attributes["Label"];
                    XmlAttribute processID = nodeProc.Attributes["ID"];
                    XmlAttribute isPopup = nodeProc.Attributes["IsPopup"];

                    if (processID != null)
                    {
                        formCol = processID.Value + "~";
                    }
                    else
                    {
                        formCol = formCol + "-1" + "~";
                    }
                    if (processName != null)
                    {

                        formCol = formCol + processName.Value + "~";
                    }
                    else
                    {
                        formCol = formCol + "-1" + "~";
                    }
                    if (currentBPGID != null)
                    {
                        formCol = formCol + currentBPGID.Value + "~";
                    }
                    else
                    {
                        formCol = formCol + "-1" + "~";
                    }
                    if (pageInfo != null)
                    {
                        formCol = formCol + pageInfo.Value + "~";
                    }
                    else
                    {
                        formCol = formCol + "-1" + "~";
                    }
                    if (isPopup != null)
                    {
                        formCol = formCol + isPopup.Value + "~";
                    }
                    else
                    {
                        formCol = formCol + "-1" + "~";
                    }

                    strFormLevelLinks = strFormLevelLinks + formCol + ":";
                }

                //Create the Table..
                //return CreateProcessLinksTable(xDoc, arrFormLevelProcs);
                return strFormLevelLinks;
            }
            catch (Exception ex)
            {
                Table tblErrorContainer = new Table();
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = ex.Message;
                td.ForeColor = Color.Red;
                tr.Cells.Add(td);
                tblErrorContainer.Rows.Add(tr);
                return string.Empty;
            }
        }

        public string GenCurrActionRequestXML(string BPEAction, string BPValue, string CntrlValues, string treeNode, string strSelectedRw, string BPE, bool gridExists, string pageSize, string pageNo, string sortOrder, string sortBy, string BPInfo, string lstBxParams)
        {
            string ReqXMl = string.Empty;
            try
            {
                string m_GVTreeNodeName = string.Empty;
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                if (BPEAction == "Save")
                {
                    nodeRoot.InnerXml = "<Autosave Process='1'></Autosave>";
                }
                nodeRoot.InnerXml += BPE;

                if (BPEAction.ToUpper().Trim() != "PAGELOAD" && BPEAction.ToUpper().Trim() != "PREVIOUSPAGELOAD")
                {
                    //Creating the bpinfo node
                    XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
                    nodeRoot.AppendChild(nodeBPInfo);

                    //Creating the BPGID node
                    XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                    nodeBPGID.InnerText = BPValue;
                    nodeBPInfo.AppendChild(nodeBPGID);
                    if (BPEAction != "")
                    {
                        //Appending the List Box's Xml
                        if (lstBxParams != string.Empty)
                        {
                            nodeBPInfo.InnerXml += lstBxParams;
                        }

                        //nodeBPInfo.InnerXml += "<JobSubAccount><RowList /></JobSubAccount>";//need to remove

                        //XmlDocument returnXML = new XmlDocument();
                        //returnXML.LoadXml(GVDataXML);
                        //m_GVTreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                        m_GVTreeNodeName = treeNode;

                        XmlNode nodeAccountingLayout = xDocGV.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                        nodeBPInfo.AppendChild(nodeAccountingLayout);
                        //Creating the Rowlist node
                        XmlNode nodeRowList = xDocGV.CreateNode(XmlNodeType.Element, "RowList", null);

                        if (BPEAction.ToUpper().Trim() == "ADD" || BPEAction.ToUpper().Trim() == "FIND")
                        {
                            HttpContext context = _Context;

                            if (BPEAction.ToUpper().Trim() == "FIND" && context.Request.QueryString["filters"] != null && context.Request.QueryString["filters"] != "")
                            {
                                string strfilters = context.Request.QueryString["filters"].ToString();

                                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                                SearchFilter objSearchFilters = oSerializer.Deserialize<SearchFilter>(strfilters);

                                //XmlAttribute findOperation = xDocGV.CreateAttribute("MatchCase");
                                //findOperation.Value = objSearchFilters.GroupOp;
                                //nodeRowList.Attributes.Append(findOperation);

                                nodeRowList.InnerXml = CntrlValues;

                                //XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                                //attrBPAction.Value = BPEAction;
                                //nodeRowList.Attributes.Append(attrBPAction);
                            }
                            else
                            {
                                nodeRowList.InnerXml = CntrlValues;

                                XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                                attrBPAction.Value = BPEAction;
                                nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                            }
                        }
                        else if (BPEAction.ToUpper().Trim() == "SAVE")
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;
                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = "Add";
                            nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        else if ((BPEAction.ToUpper().Trim() == "MODIFY") || ((BPEAction.ToUpper().Trim() == "DELETE")))
                        {
                            XmlDocument xNewDoc = new XmlDocument();
                            xNewDoc.LoadXml(strSelectedRw);
                            XmlNode nodeRowLst = xNewDoc.SelectSingleNode("Rows");

                            //XmlNode nodeRow = nodeRowLst.SelectSingleNode("Rows[@TrxID='" + trxID + "']");
                            XmlNode nodeRow = nodeRowLst;
                            //if (BPEAction.ToUpper().Trim() == "DELETE")
                            //{
                            //    nodeRowList.InnerXml = nodeRow.InnerXml;
                            //}
                            if (BPEAction.ToUpper().Trim() == "MODIFY")
                            {
                                XmlDocument xDc = new XmlDocument();
                                xDc.LoadXml(CntrlValues);
                                XmlNode xNodeMod = xDc.SelectSingleNode("Rows");

                                //Iterating through all attributes in modified row and changing those values in GvDataXMl, if not present creating new one
                                for (int i = 0; i < xNodeMod.Attributes.Count; i++)
                                {
                                    if (nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()] != null)
                                    {
                                        nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                    }
                                    else
                                    {
                                        XmlAttribute attrNew = xNewDoc.CreateAttribute(xNodeMod.Attributes[i].Name.ToString());
                                        attrNew.Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                        nodeRow.Attributes.Append(attrNew);
                                    }
                                }
                                ////Iterating through all attributes in GvdataXMl row and deleting if it doesnt exist in Modified row
                                //for (int attCnt = 0; attCnt < nodeRow.Attributes.Count; attCnt++)
                                //{
                                //    foreach (DataColumn dCol in nodeColumns.ChildNodes)
                                //    { 
                                //        if(nodeRow.Attributes[attCnt].Name.ToString().Contains())
                                //    }
                                //}

                            }
                            XmlAttribute attrBPAction = xNewDoc.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            nodeRow.Attributes.Append(attrBPAction);

                            nodeRowList.InnerXml = nodeRow.OuterXml.ToString();

                            //xDocGV.AppendChild(nodeRowList);
                            //nodeRowList.InnerXml = CntrlValues;

                            //XmlAttribute attrTrxID = xDocGV.CreateAttribute("TrxID");
                            //attrTrxID.Value = trxID;
                            //nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrTrxID);

                            //XmlAttribute attrTrxType = xDocGV.CreateAttribute("TrxType");
                            //attrTrxType.Value = trxType;
                            //nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrTrxType);

                            //XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            //attrBPAction.Value = BPEAction;
                            //nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        nodeAccountingLayout.AppendChild(nodeRowList);
                        if (BPInfo != null && BPInfo != string.Empty)
                        {
                            XmlDocument xdocBPInfo = new XmlDocument();
                            xdocBPInfo.LoadXml(BPInfo);
                            XmlNode nodecallingobj;
                            XmlNode nodeCallingObject;
                            if (xdocBPInfo.SelectSingleNode("bpinfo/CallingObject") != null)
                            {
                                nodecallingobj = xdocBPInfo.SelectSingleNode("bpinfo/CallingObject");
                                nodeCallingObject = xDocGV.CreateNode(XmlNodeType.Element, "CallingObject", null);
                                nodeCallingObject.InnerXml = nodecallingobj.InnerXml;
                                nodeBPInfo.AppendChild(nodeCallingObject);

                            }
                            else if (xdocBPInfo.SelectSingleNode("bpinfo/Object") != null)
                            {
                                nodecallingobj = xdocBPInfo.SelectSingleNode("bpinfo/Object");
                                nodeCallingObject = xDocGV.CreateNode(XmlNodeType.Element, "Object", null);
                                nodeCallingObject.InnerXml = nodecallingobj.InnerXml;
                                nodeBPInfo.AppendChild(nodeCallingObject);

                            }

                        }
                    }
                }
                else
                {
                    nodeRoot.InnerXml += BPValue.ToString();
                }
                ReqXMl = nodeRoot.OuterXml;
                if (gridExists)
                {
                    XmlDocument xDocum = new XmlDocument();
                    xDocum.LoadXml(nodeRoot.OuterXml);
                    XmlNode bpInfoNode = xDocum.SelectSingleNode("Root/bpinfo");
                    if (pageNo == string.Empty)
                    {
                        pageNo = "1";
                    }
                    if (BPEAction.ToUpper().Trim() == "FIND")
                    {
                        //pageNo = "1";
                        XmlNode treenode = xDocum.SelectSingleNode("Root/bpinfo/" + m_GVTreeNodeName);
                        treenode.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn>" + sortBy + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder></Gridview>";
                    }
                    else
                    {
                        bpInfoNode.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn>" + sortBy + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder></Gridview>";
                    }

                    ReqXMl = xDocum.OuterXml;
                }

                if (BPEAction.ToUpper().Trim() == "PREVIOUSPAGELOAD")
                {
                    XmlDocument xBPinfo = new XmlDocument();
                    xBPinfo.LoadXml(ReqXMl);
                    XmlNode nodeTreeNode = xBPinfo.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                    XmlNode nodeBPInfo = xBPinfo.SelectSingleNode("Root/bpinfo");
                    XmlNode nodegridView = xBPinfo.SelectSingleNode("Root/bpinfo/Gridview");
                    string gridview = nodegridView.OuterXml;
                    nodeBPInfo.RemoveChild(nodegridView);
                    nodeTreeNode.InnerXml = gridview;
                    nodeBPInfo.AppendChild(nodeTreeNode);
                    ReqXMl = xBPinfo.OuterXml;
                }
            }
            catch (Exception ex)
            {

            }
            return ReqXMl;
        }

        public static void FormatCheckBoxValues(XmlNode nodeRowList, string[] columns)
        {
            foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    string attName = columns[i];
                    XmlAttribute att = nodeRow.Attributes[attName];
                    if (att != null)
                    {
                        if (att.Value == "1")
                        {
                            att.Value = "Yes";
                        }
                        else// if (att.Value == "0")
                        {
                            att.Value = "No";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of all columns with ControlType as Checkbox
        /// </summary>
        /// <param name="xDoc">Xml Document</param>
        /// <param name="treeNodeName">The Node which to consider.</param>
        /// <returns>Array of the Checkbox columns names.</returns>
        public string[] GetCheckBoxCols(XmlDocument xDoc, string treeNodeName)
        {
            XmlNodeList xnlColumns = xDoc.SelectNodes("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns//Col[@ControlType='Check']");
            string[] arrCols = new string[xnlColumns.Count];
            for (int i = 0; i < xnlColumns.Count; i++)
            {
                arrCols[i] = xnlColumns[i].Attributes["Label"].Value;
            }
            return arrCols;
        }

        /// <summary>
        /// Gets the list of all columns with ControlType as Checkbox
        /// </summary>
        /// <param name="xDoc">Delimited string of the column info.</param>
        /// <param name="treeNodeName">The Node which to consider.</param>
        /// <returns>Array of the Checkbox columns names.</returns>
        public string[] GetCheckBoxCols(string columns, string treeNodeName)
        {
            string[] arrParentChild = columns.Split('>');
            List<string> lstCols = new List<string>();
            //Loop through all the parent and branch nodes.
            for (int i = 0; i < arrParentChild.Length; i++)
            {
                try
                {
                    string[] arrNodeControlColmuns = arrParentChild[i].Split(';');//Format: NodeName/ControlType;Columns
                    string[] arrColInfo = arrNodeControlColmuns[1].Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < arrColInfo.Length; j++)
                    {
                        string[] splitColInfo = arrColInfo[j].Split('~');
                        if (splitColInfo[1] == "Check")
                        {
                            lstCols.Add(splitColInfo[0]);
                        }
                    }
                }
                catch
                {
                    //Do nothing continue with the next iteration.
                }
            }
            return lstCols.ToArray();
        }
    }
}
