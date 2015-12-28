using System;
using System.Web;
using System.Xml;
using LAjit_BO;

namespace LAjitDev.Classes
{
    /// <summary>
    /// Contains(Supposedly) all the methods called through Ajax technique.
    /// </summary>
    public class AjaxMethods : System.Web.UI.Page
    {
        public CommonUI commonObjUI = new CommonUI();
        public CommonBO objBO = new CommonBO();
        XmlDocument XDocUserInfo = new XmlDocument();
        LAjit_BO.Reports objReports = new LAjit_BO.Reports();
        clsReportsUI objReportsUI = new clsReportsUI();

        /// <summary>
        /// Checks to see if there are any PO's on the current TrxID
        /// </summary>
        /// <param name="ID">TrxID of the Vendor/Customer</param>
        /// <param name="callerType">Vendor/Customer</param>
        /// <returns>Number of PO's</returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string SeekPOs(string ID, string callerType)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            string spName = "SeekVendorPO";
            string bpeInfo = "<Root>" + strBPE
                + "<bpinfo><" + callerType + "ID>" + ID + "</" + callerType + "ID></bpinfo></Root>";
            CommonBO objBO = new CommonBO();
            string outXML = objBO.GetPOs(bpeInfo, spName);
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(outXML);
            XmlNode nodePO = xDocOut.SelectSingleNode("Root/bpeout/PosFound");
            if (nodePO != null)
            {
                return nodePO.InnerText;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Set the Left Panel collapsed state from javascript.
        /// </summary>
        /// <param name="state">1 if collapsed,0 otherwise.</param>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetCollapsedState(string state)
        {
            if (state != null && state.Length > 0)
            {
                System.Web.HttpContext.Current.Session["LPCollapsed"] = state;
            }
        }

        /// <summary>
        /// Set the passed BpeInfo value into the session variable.
        /// </summary>
        /// <param name="BpeInfo"></param>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetBPEINFO(string BpeInfo)
        {
            if (BpeInfo.Length > 0)
            {
                System.Web.HttpContext.Current.Session["BPINFO"] = BpeInfo;
            }
        }

        /// <summary>
        /// Used in Upload Text.(Moved to here from UploadText)
        /// </summary>
        /// <param name="sessions"></param>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void KillSession(string sessions)
        {
            if (sessions != null)
            {
                System.Web.HttpContext.Current.Session.Contents.Remove("Row");
            }
        }

        /// <summary>
        /// To Set the Session "BPINFO" for print.(Moved to here from ButtonsUserControl)
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetPrintBPINFO(string BPINFO)
        {
            if ((BPINFO != null) && (BPINFO != string.Empty))
            {
                System.Web.HttpContext.Current.Session["BPINFO"] = BPINFO;
            }
        }

        /// <summary>
        /// To Set the Session "LinkBPinfo".(Moved to here from ButtonsUserControl)
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void ResetLinkPopUpSession(string parentBPINFO)
        {
            //Resetting the parent BPInfo to the page after closing the child(Page PopUP).
            if (parentBPINFO != string.Empty)
            {
                HttpContext.Current.Session["LinkBPinfo"] = parentBPINFO;
            }

            ////Setting LinkBPInfo to empty string
            //if (HttpContext.Current.Session["LinkBPinfo"] != null)
            //    HttpContext.Current.Session["LinkBPinfo"] = string.Empty;
        }

        /// <summary>
        /// Generates the HTML for a given form requested from GridViewControl/ParentChildGVControl
        /// </summary>
        /// <param name="BPInfo">The BPInfo of the form for which the HTML has to be generated.</param>
        /// <param name="pages">Current  or All</param>
        /// <returns>HTML String.</returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string GeneratePrintHTML(string BPInfo, string pages)
        {
            string pageSize = "";
            if (pages == "ALL")
            {
                pageSize = "-1";
            }
            //Page Numbers and sort orders not getting updated in the parameter BPInfo
            BPInfo = Convert.ToString(HttpContext.Current.Session["LinkBpInfo"]);

            LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
            GridReports objGridReports = new GridReports(new CommonUI());
            objGridReports.SessionUserInfo = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
            string reqXML = objGridReports.GenerateRequestXML(BPInfo, pageSize);
            string BPOut = reportsBO.GetReportBPEOut(reqXML);

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(BPOut);
            
            string generatedHTML = objGridReports.GenerateHTML(xDocOut, false);
            return generatedHTML;
        }

        /// <summary>
        /// Set the Session so that it can be accessed in the PrintViewer.aspx
        /// </summary>
        /// <param name="arrCols"></param>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetPrintColumns(string[] arrCols)
        {
            HttpContext.Current.Session["PrintCols"] = arrCols;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string GetMenuPanelXML(string dataPath)
        {
            try
            {
                XmlDocument xDocMenu = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                XmlNode nodeMenuPanel = xDocMenu.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/menupanel");
                int panelCount = nodeMenuPanel.ChildNodes.Count;

                string[] split = dataPath.Split('/');
                dataPath = split[split.Length - 1];
                int position = Convert.ToInt32(dataPath.Replace("*[position()=", "").Replace("]", ""));
                bool reset = false;//Variable to identify at the client-side when the datapath has to be started again from the
                //beginning while tabbing through the panels.
                if (position > panelCount)
                {
                    dataPath = "*[position()=1]";
                    reset = true;
                }
                string panelXML = nodeMenuPanel.SelectSingleNode(dataPath).OuterXml;
                if (reset)
                {
                    return "1" + panelXML;
                }
                else
                {
                    return "0" + panelXML; ;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// To submit process to get bpeout status
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string SubmitProcess(string processBPGID, string COXML)
        {
            try
            {
                string strMsg = string.Empty;
                //Submitting BPINFO and getting BPEOUT status and next bpinfo
                if (processBPGID != null && COXML != null)
                {
                    string BPINFO = "<bpinfo><BPGID>" + processBPGID + "</BPGID>" + COXML + "</bpinfo>";
                    CommonUI objCommmonUI = new CommonUI();
                    CommonBO objBO = new CommonBO();
                    XDocUserInfo = objCommmonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                    string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                    string strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPINFO, "", "", "", strBPE, false, "1", "1", null);
                    //BPOUT from DB
                    string strOutXml = objBO.GetDataForCPGV1(strReqXml);

                    string callingObjXML = "<CallingObject></CallingObject>";//Dummy CallingObject for SetGVData-->gridExists to work.
                    //XmlDocument xDocBPInfo = new XmlDocument();
                    //xDocBPInfo.LoadXml(BPINFO);
                    //XmlNode nodeCO = xDocBPInfo.SelectSingleNode("bpinfo/CallingObject");
                    //if (nodeCO != null)
                    //{
                    //    callingObjXML = nodeCO.OuterXml;
                    //}


                    //Test string strOutXml = "<Root><bpeout><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Petty Cash Expense Journal Created.</Label></Message></MessageInfo><NextPage><bpinfo><FormInfo><BPGID>304</BPGID><PageInfo>financials/Pettycashjournal.aspx</PageInfo><FormID>142</FormID><Label>Petty Cash Journals</Label><Title>Petty Cash Journals</Title><IsPopup>0</IsPopup></FormInfo><CashAccountingDoc><RowList><Rows TrxID=\"10626\" TrxType=\"213\"/></RowList></CashAccountingDoc></bpinfo></NextPage><PageTitle>Petty Cash Expense Journal Builder</PageTitle><GridLayout><Tree IsOkToPrint=\"0\" IsOkToPage=\"0\" ReportStyle=\"0\"><Node>GenericProcess</Node></Tree></GridLayout><GenericProcess><GridHeading><Title>Petty Cash Expense Journal Builder</Title><GridExtendedColumns><IsSecured>0</IsSecured><IsProtected>0</IsProtected><IsNoted>0</IsNoted><IsAttached>0</IsAttached><IsChangedHistory>0</IsChangedHistory><IsActive>0</IsActive><TypeOfInactiveStatusID>0</TypeOfInactiveStatusID><InactiveStatus>0</InactiveStatus><ToolTip>0</ToolTip><SecureAccessCategoryID>0</SecureAccessCategoryID></GridExtendedColumns><Columns><Col Label=\"Description\" Caption=\"Description\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"60\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"1\" ControlType=\"TBox\"/><Col Label=\"GenericSelect1\" Caption=\"Batch\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"1\" IsUnique=\"0\" IsNumeric=\"1\" IsLink=\"1\" IsHidden=\"0\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"2\" ControlType=\"DDL\"/><Col Label=\"GenericSelect2\" Caption=\"Choose 2\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"1\" IsLink=\"1\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"3\" ControlType=\"DDL\"/><Col Label=\"GenericSelect3\" Caption=\"Choose 3\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"1\" IsLink=\"1\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"4\" ControlType=\"DDL\"/><Col Label=\"GenericSelect4\" Caption=\"Choose 4\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"1\" IsLink=\"1\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"5\" ControlType=\"DDL\"/><Col Label=\"GenericSelect5\" Caption=\"Choose 5\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"1\" IsLink=\"1\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"6\" ControlType=\"DDL\"/><Col Label=\"GenSDate1\" Caption=\"Start Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"10\" ControlType=\"Cal\"/><Col Label=\"GenEDate1\" Caption=\"End Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"11\" ControlType=\"Cal\"/><Col Label=\"GenSDate2\" Caption=\"Start Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"12\" ControlType=\"Cal\"/><Col Label=\"GenEDate2\" Caption=\"End Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"13\" ControlType=\"Cal\"/><Col Label=\"GenSDate3\" Caption=\"Start Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"14\" ControlType=\"Cal\"/><Col Label=\"GenEDate3\" Caption=\"End Date\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"15\" ControlType=\"Cal\"/><Col Label=\"GenSRange1\" Caption=\"Start Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"20\" ControlType=\"TBox\"/><Col Label=\"GenERange1\" Caption=\"End Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"21\" ControlType=\"TBox\"/><Col Label=\"GenSRange2\" Caption=\"Start Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"22\" ControlType=\"TBox\"/><Col Label=\"GenERange2\" Caption=\"End Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"23\" ControlType=\"TBox\"/><Col Label=\"GenSRange3\" Caption=\"Start Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"24\" ControlType=\"TBox\"/><Col Label=\"GenERange3\" Caption=\"End Range\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"25\" ControlType=\"TBox\"/><Col Label=\"GenSelect1\" Caption=\"Post To Ledger\" BPControl=\" \" SmallViewLength=\"10\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"0\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"40\" ControlType=\"Check\"/><Col Label=\"GenSelect2\" Caption=\"Option 2\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"42\" ControlType=\"Check\"/><Col Label=\"GenSelect3\" Caption=\"Option 3\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"44\" ControlType=\"Check\"/><Col Label=\"GenSelect4\" Caption=\"Option 4\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"46\" ControlType=\"Check\"/><Col Label=\"GenSelect5\" Caption=\"Option 5\" BPControl=\" \" SmallViewLength=\"0\" FullViewLength=\"0\" IsRequired=\"0\" IsUnique=\"0\" IsNumeric=\"0\" IsLink=\"0\" IsHidden=\"1\" IsDisplayOnly=\"0\" IsParentLink=\"0\" IsSortable=\"0\" IsSummed=\"0\" IsSearched=\"0\" SortOrder=\"48\" ControlType=\"Check\"/></Columns></GridHeading><Gridresults><Totalpage Pagesize=\"1\"/><Currentpage Pagesize=\"1\"/></Gridresults><RowList><Rows TrxID=\"407\" TrxType=\"219\" Description=\"Petty Cash Upload\" GenericSelect1_TrxType=\"216\" GenericSelect2_TrxType=\"220\" GenericSelect3_TrxType=\"220\" GenericSelect4_TrxType=\"220\" GenericSelect5_TrxType=\"220\" OkToDelete=\"1\" OkToUpdate=\"1\" IsAccessOK=\"1\" RowNumber=\"1\"/></RowList></GenericProcess></bpeout></Root>";
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(strOutXml);
                    XmlNode nodeMsg = xdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                    XmlNode nodeMsgStatus = xdoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                    if (nodeMsgStatus.InnerText.ToUpper() == "SUCCESS")
                    {
                        //Look for next NextPage BPINFO
                        XmlNode nodeBPINFO = xdoc.SelectSingleNode("Root/bpeout/FormControls/NextPage");
                        if (nodeBPINFO != null)
                        {
                            //Set Session BPINFO 
                            XmlNode nodeFormInfo = xdoc.SelectSingleNode("Root/bpeout/FormControls/NextPage/bpinfo/FormInfo");
                            if (nodeFormInfo != null)
                            {
                                string BPGID = nodeFormInfo.SelectSingleNode("BPGID").InnerText;
                                //nodeFormInfo.RemoveAll();
                                nodeFormInfo.ParentNode.RemoveChild(nodeFormInfo);
                                XmlNode nodeNewBPInfo = xdoc.SelectSingleNode("Root/bpeout/FormControls/NextPage/bpinfo");

                                /*XmlNode nodeBPGID = xdoc.CreateNode(XmlNodeType.Element, "BPGID", "");
                                nodeBPGID.InnerText = BPGID;
                                //nodeNewBPInfo.AppendChild(nodeBPGID);
                                nodeNewBPInfo.PrependChild(nodeBPGID);*/

                                // SET BPINFO PARAMETERS
                                strMsg = BPGID + "~" + nodeFormInfo.SelectSingleNode("PageInfo").InnerText + "~" + "Success" + "~" + nodeFormInfo.SelectSingleNode("IsPopup").InnerText + "~" + nodeNewBPInfo.InnerXml + callingObjXML;
                            }
                        }

                    }
                    if (strMsg == string.Empty)
                    {
                        if (nodeMsgStatus.InnerText == "Success")
                        {
                            // Set Success Message
                            if (nodeMsg != null)
                            {
                                if (nodeMsg.SelectSingleNode("Label") != null)
                                {
                                    strMsg = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                                }
                            }
                        }
                        else
                        {   // Set Error Message
                            if (nodeMsg != null)
                            {
                                if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                                {
                                    strMsg = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                                }
                            }
                        }
                    }
                }
                return strMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// To submit defaut company role and get bpeout status
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string SubmitCompanyRole(string RoleCompanyID, string UserRoleID)
        {
            CommonUI objCommmonUI = new CommonUI();
            CommonBO objBO = new CommonBO();
            XDocUserInfo = objCommmonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strResult = string.Empty;
            try
            {
                string DefaultBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='CompanyDefault']").Attributes["BPGID"].Value;
                string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                string BPINFO = GenarateCompanyBPINFO(RoleCompanyID, UserRoleID, DefaultBPGID);
                string strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPINFO, "", "", "", strBPE, false, "1", "1", null);
                //BPOUT from DB
                string strOutXml = objBO.GetDataForCPGV1(strReqXml);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(strOutXml);
                XmlNode nodeMsg = xdoc.SelectSingleNode("MessageInfo/Message");
                XmlNode nodeMsgStatus = xdoc.SelectSingleNode("MessageInfo/Status");
                if (nodeMsgStatus.InnerText.ToUpper() == "SUCCESS")
                {
                    //SUCCESS
                    strResult = nodeMsg.SelectSingleNode("Label").InnerText;
                }
                else
                {
                    //ERROR
                    strResult = nodeMsg.SelectSingleNode("Label").InnerText;
                }
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Genarating Company Role BPINFO
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="RoleID"></param>
        /// <param name="DefaultBPGID"></param>
        /// <returns></returns>
        private string GenarateCompanyBPINFO(string CompanyID, string RoleID, string DefaultBPGID)
        {
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = DefaultBPGID;
            nodeBPInfo.AppendChild(nodeBPGID);
            XmlNode nodeCompany = xDocGV.CreateNode(XmlNodeType.Element, "CompanyDefault", null);
            nodeBPInfo.AppendChild(nodeCompany);
            XmlAttribute attCompany = xDocGV.CreateAttribute("CompanyID");
            attCompany.Value = CompanyID;
            nodeCompany.Attributes.Append(attCompany);
            XmlAttribute attRoleID = xDocGV.CreateAttribute("RoleID");
            attRoleID.Value = RoleID;
            nodeCompany.Attributes.Append(attRoleID);
            return nodeBPInfo.OuterXml;
        }

        /// <summary>
        /// To read cookie and get status 
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool GetCookieStatus()
        {
            bool cookiestatus = false;
            if (System.Web.HttpContext.Current.Request.Cookies["LACookie"] != null)
            {
                cookiestatus = true;
            }
            return cookiestatus;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string Submit(string m_CurrBPGID, string m_COXML, string m_EmailIDS)
        {
            string reqXML = string.Empty;
            string outXML = string.Empty;
            string statusmessage = string.Empty;
            //
            if (m_COXML != string.Empty)
            {
                XmlDocument xNewBpInfo = new XmlDocument();
                XmlNode nodeBpAppend = xNewBpInfo.CreateNode(XmlNodeType.Element, "bpinfo", null);
                XmlElement eleBPGID = xNewBpInfo.CreateElement("BPGID");
                nodeBpAppend.AppendChild(eleBPGID);
                eleBPGID.InnerText = m_CurrBPGID;
                nodeBpAppend.InnerXml += m_COXML;
                //
                reqXML = nodeBpAppend.OuterXml.ToString();
                outXML = BindPage(reqXML, m_EmailIDS);
                XmlDocument xDocOut = new XmlDocument();
                xDocOut.LoadXml(outXML);
                //
                if (outXML != string.Empty)
                {
                    if (outXML.ToString().ToUpper().Contains("SUCCESS"))
                    {
                        //objReportsUI.GenerateReport(outXML);
                        objReportsUI.GenerateReport(xDocOut, "PDF");
                        if ((bool)objReportsUI.EmailStatus == false)
                        {
                            statusmessage = "No Email Sent.Please Check Your E-Mail ID";
                        }
                        else
                        {
                            statusmessage = "E-Mail Sent Successfully";
                        }
                    }
                    else
                    {
                        string msgStat1 = string.Empty;
                        string msgStat2 = string.Empty;
                        string msgStat3 = string.Empty;
                        //
                        XmlDocument xDocError = new XmlDocument();
                        xDocError.LoadXml(outXML);
                        XmlNode nodeMsgStatus1 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/Status");
                        if (nodeMsgStatus1 != null)
                        {
                            msgStat1 = nodeMsgStatus1.InnerText.ToString();
                        }
                        XmlNode nodeMsgStatus2 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                        if (nodeMsgStatus2 != null)
                        {
                            msgStat2 = nodeMsgStatus2.InnerText.ToString();
                        }
                        XmlNode nodeMsgStatus3 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                        if (nodeMsgStatus3 != null)
                        {
                            msgStat3 = nodeMsgStatus3.InnerText.ToString();
                        }
                        statusmessage = msgStat1 + "-" + msgStat2 + "-" + msgStat3;
                    }
                }
            }
            return statusmessage;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string GridEmailPDF(string m_BPInfo, string m_Pages, string m_EmailIDS, string[] arrColumns)
        {
            LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
            GridReports objGridReports = new GridReports(new CommonUI());
            string reqXML = string.Empty;
            string outXML = string.Empty;
            string statusmessage = string.Empty;
            //
            if (m_BPInfo != string.Empty)
            {
                string pageSize = "";
                if (m_Pages == "ALL")
                {
                    pageSize = "-1";
                }
                //
                string[] xmlPath = new string[1];
                xmlPath[0] = Convert.ToString(HttpContext.Current.Session["USERINFOXML"]);
                reqXML = objGridReports.GenerateRequestXML(m_BPInfo, pageSize, xmlPath[0].ToString());
                outXML = BindPage(reqXML, m_EmailIDS);
                XmlDocument xDocOut = new XmlDocument();
                xDocOut.LoadXml(outXML);
                //
                if (outXML != string.Empty)
                {
                    if (outXML.ToString().ToUpper().Contains("SUCCESS"))
                    {
                        if (arrColumns.Length > 0)
                        {
                            objReportsUI.GenerateReport(xDocOut, "PDF", arrColumns);
                        }
                        else
                        {
                            objReportsUI.GenerateReport(xDocOut, "PDF");
                        }
                        if ((bool)objReportsUI.EmailStatus == false)
                        {
                            statusmessage = "No Email Sent.Please Check Your E-Mail ID";
                        }
                        else
                        {
                            statusmessage = "E-Mail Sent Successfully";
                        }
                    }
                    else
                    {
                        string msgStat1 = string.Empty;
                        string msgStat2 = string.Empty;
                        string msgStat3 = string.Empty;
                        //
                        XmlDocument xDocError = new XmlDocument();
                        xDocError.LoadXml(outXML);
                        XmlNode nodeMsgStatus1 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/Status");
                        if (nodeMsgStatus1 != null)
                        {
                            msgStat1 = nodeMsgStatus1.InnerText.ToString();
                        }
                        XmlNode nodeMsgStatus2 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                        if (nodeMsgStatus2 != null)
                        {
                            msgStat2 = nodeMsgStatus2.InnerText.ToString();
                        }
                        XmlNode nodeMsgStatus3 = xDocError.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                        if (nodeMsgStatus3 != null)
                        {
                            msgStat3 = nodeMsgStatus3.InnerText.ToString();
                        }
                        statusmessage = msgStat1 + "-" + msgStat2 + "-" + msgStat3;
                    }
                }
            }
            return statusmessage;
        }


        public string BindPage(string BPInfo, string m_EmailIDs)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlDocument XDocUserInfo = new XmlDocument();
            //
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            string TotalOutXML = string.Empty;
            //
            if (BPInfo.ToString().ToUpper().Contains("CALLINGOBJECT"))
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(System.Web.HttpContext.Current.Session["USERINFOXML"]));
                strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPInfo, "", "", "", Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml), false, "", "", null);
            }
            else
            {
                strReqXml = BPInfo;
            }
            strOutXml = objBO.GetDataForCPGV1(strReqXml);
            xDoc.LoadXml(strOutXml);
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeMsgStatus.InnerText == "Success")
            {
                int reportStyle = 0;
                XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                string treeNodeName = string.Empty;
                if (nodeTreenode.Attributes["ReportStyle"] != null)
                {
                    reportStyle = int.Parse(nodeTreenode.Attributes["ReportStyle"].Value);
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
                                    attrReportStyle.Value = "1";
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
                TotalOutXML = SendPrintReport(xDoc, m_EmailIDs);
            }
            else
            {
                TotalOutXML = strOutXml;
            }
            return TotalOutXML;
        }

        public string SendPrintReport(XmlDocument xDoc, string m_EmailIDS)
        {
            string GVXml = string.Empty;
            XmlDocument xPrintOption = new XmlDocument();
            XmlNode nodePrintOption = xPrintOption.CreateNode(XmlNodeType.Element, "Root", null);
            nodePrintOption.InnerXml = xDoc.SelectSingleNode("//bpeout").OuterXml.ToString();
            if (m_EmailIDS != string.Empty)
            {
                nodePrintOption.InnerXml += "<EmailIDS>" + m_EmailIDS + "</EmailIDS>";
            }
            GVXml = nodePrintOption.OuterXml.ToString();
            return GVXml;
        }
    }
}
