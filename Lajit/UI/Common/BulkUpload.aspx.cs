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
using System.IO;
using System.Xml;
using System.Data.OleDb;
using System.Reflection;
using LAjit_BO;
using LAjitControls;
using System.Collections.Generic;
using LAjitDev.UserControls;
using System.Web.Script.Serialization;
using LAjitDev.Classes;
using NLog;
using NLog.Targets;
using NLog.Config;
using NLog.Targets.Wrappers;
using System.Diagnostics;

namespace LAjitDev.Common
{
    public partial class BulkUpload : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private GridViewControl GVUC;
        private ChildGridView CGVUC;
        private ButtonsUserControl BtnsUC;
        private ParentChildGV PCGVUC;
        public CommonBO objBO = new CommonBO();
        public CommonUI commonObjUI = new CommonUI();
        private OleDbConnection cn;
        private OleDbDataAdapter daAdapter;
        private string ExcelCon = @"Provider=Microsoft.Jet.OLEDB.4.0;";
        private string strConnectionString;
        private string SheetName, Range;
        private Stream objStream;
        private StreamReader objReader;
        string m_treeNode = string.Empty;
        XmlDocument XDocUserInfo = new XmlDocument();

        protected override void OnPreInit(EventArgs e)
        {
            //changing masterpage file dynamically
            string masterfile = string.Empty;
            if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
            {
                masterfile = "../MasterPages/PopUp.Master";
            }
            else
            {
                masterfile = "../MasterPages/TopLeft.Master";
            }
            if (!masterfile.Equals(string.Empty))
            {
                base.MasterPageFile = masterfile;
            }
            base.OnPreInit(e);

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            if (XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml != null)
            {
                //string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
                string keyTheme = "430";
                if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo") != null)
                {
                    string theme = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                    Session.Add("MyTheme", theme);
                }
                Page.Theme = ((string)HttpContext.Current.Session["MyTheme"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string changeCloseJS = "ChangeCloseLinkText('File Upload');";
            ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);

            string HideProgressJS = "HideParentProgress();HideProgress();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideProgress", HideProgressJS, true);

            string BPInfo = string.Empty;
            string masterfile = string.Empty;
            string row = string.Empty;

            //lblDescriptionText.Text = "";
            //lblDescriptionText.Visible = false;

            Ajax.Utility.RegisterTypeForAjax(typeof(LAjitDev.Common.BulkUpload));
            Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
            Ajax.Utility.RegisterTypeForAjax(typeof(Classes.AjaxMethods));

            XmlDocument doc = new XmlDocument();
            if (!Page.IsPostBack)
            {
                if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
                {
                    masterfile = "../MasterPages/PopUp.Master";
                }
                else
                {
                    masterfile = "../MasterPages/TopLeft.Master";
                }
                if (masterfile.Contains("PopUp"))
                {
                    if (Session["LinkBPinfo"] != null)
                    {
                        //only for pages whose master page is 'PopUp', we need to take Session 'LinkBPinfo'.
                        if (!this.Page.MasterPageFile.Contains("TopLeft.Master"))
                        {
                            //setting the  Session for childpage.
                            BPInfo = Session["LinkBPinfo"].ToString();
                        }
                        tblEntryForm.Style["height"] = "490";
                        tblEntryForm.Style["width"] = "925";
                        //Session["BPINFO"] = BPInfo;
                        doc.LoadXml(BPInfo);
                        XmlNode xrowNode = doc.SelectSingleNode("//RowList");
                        if (xrowNode != null)
                        {
                            row = xrowNode.OuterXml.ToString();
                        }
                        //Passing the BPInfo and Content Page Update panel as parameters to bind the page
                        BindPage(BPInfo, updtPnlContent, XDocUserInfo);
                    }
                    if (row != null)
                    {
                        Session["Row"] = row.ToString();
                    }
                }
                else
                {
                    string bpgid = Session["BPINFO"].ToString();
                    BindPage(bpgid, updtPnlContent, XDocUserInfo);
                }
                //The code for setting UI headers skips as there is no update panel.so set it here
                int entryFormWidth = 818;//TotalWindowWidth - BtnUCWidth -SpacerWidth
                //If Left panel is collapsed the width increases by an amount equal to that of Left Panel
                if (Convert.ToString(Session["LPCollapsed"]) == "1")
                {
                    entryFormWidth += 149;
                }
                //if (htcEntryForm.Width != "")//shanti
                //{
                //    htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcEntryForm.Width) - 31);//50
                //}
            }
            imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/Upload_but.png";
            //Load Process Links
            GenerateProcessLinks(ViewState["returnXML"].ToString());
        }

        public void BindPage(string SessionBPInfo, Control CurrentPage, XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + SessionBPInfo + " Currrent Page as : " + CurrentPage + " and user info :" + XDocUserInfo);
            #endregion

            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            XmlDocument xDocBPINFo = new XmlDocument();
            xDocBPINFo.LoadXml(SessionBPInfo);
            XmlNode xBPGIDNode = xDocBPINFo.SelectSingleNode("bpinfo");
            XmlNode xInitNode = xDocBPINFo.CreateNode(XmlNodeType.Element, "INIT", "");
            xInitNode.InnerText = "1";
            xBPGIDNode.AppendChild(xInitNode);
            XmlDocument xDoc = new XmlDocument();
            if (SessionBPInfo != string.Empty)
            {
                SessionBPInfo = xDocBPINFo.OuterXml.ToString();
                Session["BPINFO"] = SessionBPInfo;
                //Keeping Parent BPInfo backup in the hidden variable
                parentBPInfo.Value = SessionBPInfo;
            }
            //generate Request XMl 
            string strReqXml = objBO.GenActionRequestXML("PAGELOAD", parentBPInfo.Value, "", "", "", Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml), false, "", "", null);
            //BPOUT from DB
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);
            if (strOutXml != null)
            {
                xDoc.LoadXml(strOutXml);
                //For successful BPOut
                XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                XmlNode nodeBusinessProcessBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess");
                if (nodeBusinessProcessBPGID == null)
                {
                    imgbtnSubmit.Visible = false;
                }
                if (nodeMsgStatus.InnerText == "Success")
                {
                    ViewState["returnXML"] = strOutXml;
                }
            }
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //if (!chkQuickUpload.Checked)
            //{
            UploadData();
            //}
            //else
            //{
            //    UploadDataNew();
            //}

            //TestAutoFillAnamolies();
        }

        /// <summary>
        /// Test Method. Not for Live Use
        /// </summary>
        private void TestAutoFillAnamolies()
        {
            CommonUI objCUI = new CommonUI();
            string xlInit = UploadTexts(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\bobautofil.xls", false);
            string[] xlSheets = GetExcelSheetNames();
            string strInfo = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AutoFillLogInfo.txt";
            string strFaults = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AutoFillFaultsLog.txt";
            StreamWriter sw = new StreamWriter(strInfo);
            StreamWriter swFaults = new StreamWriter(strFaults);
            try
            {
                for (int i = 0; i < xlSheets.Length; i++)
                {
                    string xlSheet = xlSheets[i];
                    string context = GetAFContext(xlSheet);
                    System.Diagnostics.Debugger.Log(1, "AfLog", "\n\n\n*********Running Sheet***** : " + xlSheet);
                    sw.Write("\n\n\n******************Running Sheet***** : " + xlSheet + "\n");
                    swFaults.Write("\n\n\n******************Running Sheet***** : " + xlSheet + "\n");

                    DataTable dtSheet = GetDataTable(xlSheet);
                    int xlRow = 0;
                    for (int rowCntr = 0; rowCntr < dtSheet.Rows.Count; rowCntr++)
                    {
                        xlRow = rowCntr + 1;
                        string query = objCUI.HtmlEncode(dtSheet.Rows[rowCntr][0].ToString());
                        string actualText = dtSheet.Rows[rowCntr][0].ToString();
                        XmlNode nodeResults = AutoFill.GetAutoFillDataNode(query, "AutoFillGeneric", context, "");
                        System.Diagnostics.Debugger.Log(1, "AfLog", "\n" + rowCntr + " Running Query : " + actualText);
                        if (nodeResults != null && nodeResults.FirstChild != null)
                        {
                            XmlNodeList xnlRows = nodeResults.FirstChild.ChildNodes;
                            if (xnlRows.Count > 1)
                            {
                                //System.Diagnostics.Debugger.Log(1, "AfLog", "\tMore than one result found!!");
                                //System.Diagnostics.Debugger.Log(1, "AfLog", "\n" + nodeResults.FirstChild.OuterXml);
                                //swFaults.Write("\n" + actualText + " - \tMore than one result found!!");
                                //swFaults.WriteLine("\n\t\t" + nodeResults.FirstChild.OuterXml);

                                if (xnlRows[0].Attributes[context].Value.Trim() != query.Trim())
                                {
                                    System.Diagnostics.Debugger.Log(1, "AfLog", "Upload is Invalid.");
                                    swFaults.WriteLine(string.Format("\nExcel Row {2} Upload is InValid : {0} has been uploaded instead of {1} "
                                            , xnlRows[0].Attributes[context].Value, query, xlRow));

                                    swFaults.WriteLine("\n\t\tXML : " + nodeResults.FirstChild.OuterXml);
                                }
                            }
                        }
                        else
                        {
                            if (query.Length > 48)
                            {
                                System.Diagnostics.Debugger.Log(1, "AfLog", "\tLength OverFlow Error!!");
                                sw.Write("\nCould not upload :" + actualText + " at row " + xlRow + " - Probable SP Length OverFlow Error!!");
                            }
                            else
                            {
                                System.Diagnostics.Debugger.Log(1, "AfLog", "\tNo results found!!");
                                sw.Write("\nCould not upload :" + actualText + " at row " + xlRow + " - No result found!!");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                #region NLog
                logger.Fatal(e);
                #endregion

                sw.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                sw.Close();
                swFaults.Close();

                Mail objMailer = new Mail();
                objMailer.Attachment = strInfo;
                objMailer.Subject = "AutoFill Ananamolies";
                objMailer.From = "daniel.devarampally@valuelabs.net";
                objMailer.To = "partha.maharatha@valuelabs.net";
                objMailer.Cc = "daniel.devarampally@valuelabs.net";
                objMailer.Body = "Please find attached the list of entries that could not be uploaded due the SP bug(indicated by LENGTH OVERFLOW ERROR in the file)" +
                    " and No Results Found entries.\n\nP.S: Use WordPad to view the log.";
                objMailer.SendMail();

                objMailer = new Mail();
                objMailer.Attachment = strFaults;
                objMailer.Subject = "AutoFill Duplicates(Wrong ones)";
                objMailer.From = "daniel.devarampally@valuelabs.net";
                objMailer.To = "partha.maharatha@valuelabs.net";
                objMailer.Cc = "daniel.devarampally@valuelabs.net";
                objMailer.Body = "Please find attached the list of entries that were incorrectly uploaded.\n\nP.S: Use WordPad to view the log.";
                objMailer.SendMail();
            }
        }

        /// <summary>
        /// Test Method. Not for Live Use
        /// </summary>
        /// <param name="xlSheet"></param>
        /// <returns></returns>
        private string GetAFContext(string xlSheet)
        {
            xlSheet = xlSheet.Replace("$", "");
            if (xlSheet == "Vendors")
            {
                return "AutoFillVendor";
            }
            if (xlSheet == "Customers")
            {
                return "Customer";
            }
            if (xlSheet == "Lines" || xlSheet == "Account")
            {
                return "AutoFillAccount";
            }
            if (xlSheet == "Jobs")
            {
                return "AutoFillJob";
            }
            return "";
        }

        protected void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //UploadData();
            //sw.Stop();
            //System.Diagnostics.Debugger.Log(1, "Test", "\nOld Method : " + sw.ElapsedMilliseconds);
            //sw.Reset();

            //sw.Start();
            UploadDataNew();
            //sw.Stop();
            //System.Diagnostics.Debugger.Log(1, "Test", "\nNew Method : " + sw.ElapsedMilliseconds);
        }

        private void UploadDataNew()
        {
            //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            //st.Start();

            string m_ReqXML = string.Empty;
            string m_FilePath = string.Empty;
            string m_Extn = string.Empty;
            string m_UserBPE = string.Empty;
            string m_PhysicalFilePath = string.Empty;
            string strReqXml = string.Empty;
            string strReqBPGID = string.Empty;
            string strErrorBPGID = string.Empty;

            //Document to load the ViewState XML
            XmlDocument xReturnDoc = new XmlDocument();

            //List to load Parent and Child Col Values
            List<GetColAttr> m_listColumns = new List<GetColAttr>();

            //try
            {
                m_Extn = Path.GetExtension(FileAttachment.PostedFile.FileName);
                m_UserBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                m_PhysicalFilePath = ConfigurationManager.AppSettings["TempFilePath"].ToString();

                //Start Forming the RequestXML
                strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                if (FileAttachment.HasFile)
                {
                    string m_FileName = string.Empty;
                    int fileSize = FileAttachment.PostedFile.ContentLength;
                    fileSize = fileSize / 1024;
                    m_UserBPE = string.Empty;
                    m_UserBPE = Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString());
                    m_UserBPE = m_UserBPE + "~" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                    m_UserBPE = m_UserBPE + m_Extn;

                    if (!Directory.Exists(m_PhysicalFilePath))
                    {
                        Directory.CreateDirectory(m_PhysicalFilePath);
                    }

                    FileAttachment.PostedFile.SaveAs(m_PhysicalFilePath + "\\" + m_UserBPE);
                    m_FilePath = m_PhysicalFilePath + "\\" + m_UserBPE;
                    lblmsg.Visible = false;
                    UploadTexts(m_FilePath, true);

                    //Load the viewState return XML in returnDoc
                    if (ViewState["returnXML"].ToString() != string.Empty)
                    {
                        xReturnDoc.LoadXml(ViewState["returnXML"].ToString());
                    }

                    //Get the BPGID
                    XmlNode xBusProc = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                    strReqBPGID = xBusProc.SelectSingleNode("BusinessProcess[@ID='Add']").Attributes["BPGID"].Value.ToString();

                    if (xBusProc.SelectSingleNode("BusinessProcess[@ID='ErrorAddBPGID']") != null)
                        strErrorBPGID = xBusProc.SelectSingleNode("BusinessProcess[@ID='ErrorAddBPGID']").Attributes["BPGID"].Value.ToString();

                    //Get the Parent Node Name
                    XmlNode nodeTree = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    string strParentNodeName = nodeTree.SelectSingleNode("Node").InnerText;

                    //Get the Branch Node Names
                    ArrayList arrBranchNodeList = new ArrayList();
                    XmlNode nodeAllBranches = nodeTree.SelectSingleNode("Branches");

                    if (nodeAllBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeAllBranches.ChildNodes)
                        {
                            string strBranchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            arrBranchNodeList.Add(strBranchNodeName);
                        }
                    }

                    //Navigate to the Parent Node
                    List<GetColAttr> parentColAttrList = new List<GetColAttr>();
                    XmlNode nodeParentCols = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strParentNodeName + "/GridHeading/Columns");

                    //Load the the ParentColAttributesList
                    foreach (XmlNode nodeChildCol in nodeParentCols.ChildNodes)
                    {
                        GetColAttr parentColAttr = ParseColAttr(nodeChildCol);
                        parentColAttrList.Add(parentColAttr);
                    }

                    //ArrayList arrBranchColList = new ArrayList();
                    List<List<GetColAttr>> arrBranchColList = new List<List<GetColAttr>>();

                    if (arrBranchNodeList.Count > 0)
                    {
                        IEnumerator enumBranch = arrBranchNodeList.GetEnumerator();

                        //ArrayList which contains the ColAttributesLists of all the branchNodes 

                        while (enumBranch.MoveNext())
                        {
                            List<GetColAttr> branchColAttrList = new List<GetColAttr>();

                            //Load Individual ColListAttributes of each Treenode and save it in branchColAttrList
                            XmlNode nodeBranchCols = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + enumBranch.Current.ToString() + "/GridHeading/Columns");
                            foreach (XmlNode nodeChildCol in nodeBranchCols.ChildNodes)
                            {
                                GetColAttr branchColAttr = ParseColAttr(nodeChildCol);
                                branchColAttrList.Add(branchColAttr);
                            }

                            //Once we get the colAttributes List of all the columns load the List into ArrayList
                            arrBranchColList.Add(branchColAttrList);
                        }
                    }

                    //Data Table to Load the Excel Sheet Data
                    DataTable dt = new DataTable();
                    string strSheetNameRange = string.Empty;

                    //Array to get the SheetNames
                    string[] sheetnames = GetExcelSheetNames();
                    string str = string.Empty;
                    int recordCount = 0;

                    Dictionary<string, XmlNode> dictAutoFills = new Dictionary<string, XmlNode>();
                    XmlNodeList xAutoFillNodesList = xReturnDoc.SelectNodes("Root/bpeout/FormControls//Columns//Col[@ControlType='TBox' and @IsLink='1']");

                    for (int nodesCount = 0; nodesCount < xAutoFillNodesList.Count; nodesCount++)
                    {
                        string strLabel = xAutoFillNodesList[nodesCount].Attributes["Label"].Value;

                        if (!dictAutoFills.ContainsKey(strLabel))
                        {
                            //string requestLabel = string.Empty;
                            //if (strLabel == "AutoFillJob" || strLabel == "AutoFillAccount")
                            //{
                            //    requestLabel = strLabel + "List";
                            //}
                            //else
                            //{
                            //    requestLabel = strLabel;
                            //}
                            XmlNode xNode = AutoFill.GetAutoFillDataNode("%", "AutoFillGeneric", strLabel, "");
                            XmlNode xRowNode = xNode.SelectSingleNode("RowList");
                            dictAutoFills[strLabel] = xRowNode;
                        }
                    }

                    if (sheetnames != null)
                    {
                        int count = sheetnames.Length;
                        //Iterate through the Sheets
                        //for (int i = 0; i < count; i++)
                        //{
                        //strSheetNameRange = sheetnames[i].ToString();
                        strSheetNameRange = sheetnames[0].ToString();
                        if (strSheetNameRange.Substring(strSheetNameRange.Length - 1, 1).ToString() == "$")
                        {
                            //Get the content of the Sheet into the DataTable
                            dt = GetDataTable(strSheetNameRange);


                            //***************************Test Stub***********************

                            //for (int tCntr = 1; tCntr < dt.Rows.Count; tCntr++)
                            //{
                            //    string testString = dt.Rows[tCntr]["SelectPCVendor"].ToString();
                            //    string str1 = CommonUI.ParseXpathString(testString);
                            //    string str2 = CommonUI.ParseXPath(testString);

                            //    if (str1.Replace(" ", "") != str2.Replace(" ", ""))
                            //    {

                            //    }
                            //}

                            //***********************************************************

                            //Add Columns which are present in xml but not in datatable and whose IsRequired Attribute is "1" to DataTable
                            AddColToDataTable(xReturnDoc, dt, strParentNodeName, arrBranchNodeList);
                            recordCount = recordCount + dt.Rows.Count;
                            int excelRowCount = 0;

                            //DataTable for Loading Rows which are not uploaded 
                            //DataTable dtNotUploaded = new DataTable();
                            //dtNotUploaded = dt.Clone();
                            //string fileName = ConfigurationManager.AppSettings["TempFilePath"] + @"\TrackingReferenceList" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".txt";
                            //FileStream notUplRowFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                            //StreamWriter sWriter = new StreamWriter(notUplRowFile);

                            //Iterate through each row
                            for (int rowCount = 0; rowCount <= dt.Rows.Count; rowCount++)
                            {
                                excelRowCount++;
                                System.Diagnostics.Debugger.Log(1, "Upload", "\n Current Row : " + excelRowCount);
                                rowCount = 0;
                                DataRow dRow = dt.Rows[0];
                                if (dt.Columns.Contains("Import"))
                                {
                                    if (dRow["Import"].ToString().ToLower() == "true")
                                    {
                                        DataRow[] drTrackedRows = null;
                                        StringBuilder strBRowBPInfo = new StringBuilder();
                                        strBRowBPInfo.Append("<bpinfo>");
                                        strBRowBPInfo.Append("<BPGID>" + strReqBPGID + "</BPGID>");

                                        //Iterate through the parent and ChildCol Lists.....arrBranchColList.Count + 1 is to include the parent also
                                        for (int listCount = 0; listCount < arrBranchColList.Count + 1; listCount++)
                                        {
                                            string strNodeName = string.Empty;
                                            string strTRef = dRow["TrackingReference"].ToString();
                                            if (listCount == 0)
                                            {
                                                strNodeName = strParentNodeName;
                                                m_listColumns = parentColAttrList;

                                                strBRowBPInfo.Append("<" + strNodeName + ">");
                                                strBRowBPInfo.Append("<RowList>");

                                                StringBuilder strBRowAttributes = new StringBuilder();
                                                //Get the colNames of the List
                                                for (int colCount = 0; colCount < m_listColumns.Count; colCount++)
                                                {
                                                    string strLabel = m_listColumns[colCount].Label;
                                                    //Get the DefAultValue from the corresponding list 
                                                    string strDefValue = m_listColumns[colCount].DefaultValue;

                                                    //If colLabel in list is Equal to ColLabel in the DataTable
                                                    if (dt.Columns.Contains(strLabel))
                                                    {
                                                        //Check if the Default Value Exists
                                                        if (dRow[strLabel].ToString().Trim() == string.Empty)
                                                        {
                                                            if (m_listColumns[colCount].DefaultValue != string.Empty)
                                                            {
                                                                //strDefValue = m_listColumns[colCount].DefaultValue;
                                                                switch (m_listColumns[colCount].ControlType)
                                                                {
                                                                    default:
                                                                        {
                                                                            if (strDefValue != string.Empty)
                                                                            {
                                                                                strBRowAttributes.Append(strLabel + "=\"" + strDefValue + "\" ");
                                                                            }
                                                                            break;
                                                                        }
                                                                    //Load the TrxType and TrxID 
                                                                    case "DDL":
                                                                        {
                                                                            XmlNode xDDLNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                                                            string nodeTrxType = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxType"].Value.ToString();
                                                                            string nodeTrxID = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxID"].Value.ToString();

                                                                            strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                                                            strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");
                                                                            break;
                                                                        }
                                                                    case "TBox":
                                                                        {
                                                                            if (m_listColumns[colCount].ControlType == "TBox" && m_listColumns[colCount].IsLink == true)
                                                                            {
                                                                                string[] autoFillDef = strDefValue.Split('~');
                                                                                string attrTrxID = autoFillDef[0].ToString();
                                                                                string attrTrxType = autoFillDef[1].ToString();

                                                                                strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                                                strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                                                            }
                                                                            else
                                                                            {
                                                                                if (strDefValue != string.Empty)
                                                                                {
                                                                                    strBRowAttributes.Append(strLabel + "=\"" + strDefValue + "\" ");
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            switch (m_listColumns[colCount].ControlType)
                                                            {
                                                                default:
                                                                    {
                                                                        strBRowAttributes.Append(strLabel + "=\"" + dRow[strLabel].ToString() + "\" ");
                                                                        break;
                                                                    }
                                                                case "TBox":
                                                                    {
                                                                        if (m_listColumns[colCount].ControlType == "TBox" && m_listColumns[colCount].IsLink == true)
                                                                        {
                                                                            XmlNode xNode = dictAutoFills[strLabel];
                                                                            string strAutoFillData = dRow[strLabel].ToString();
                                                                            XmlNode xRowNode = GetAutoFillItem(strLabel, strAutoFillData, xNode);

                                                                            //XmlNode xNode = AutoFill.GetAutoFillDataNode(strAutoFillData.Replace("&", "&amp;"), "AutoFillGeneric", strLabel, string.Empty);
                                                                            //XmlNode xRowNode = xNode.SelectSingleNode("RowList");
                                                                            //if (xRowNode != null)
                                                                            //{
                                                                            //    if (xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"] != null && xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"] != null)
                                                                            //    {
                                                                            //        string attrTrxID = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"].Value;
                                                                            //        string attrTrxType = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"].Value;
                                                                            //        strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                                            //        strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                                                            //    }
                                                                            //}
                                                                            //if (xRowNodes != null)

                                                                            if (xRowNode != null)
                                                                            {
                                                                                if (xRowNode.Attributes[strLabel + "_TrxID"] != null && xRowNode.Attributes[strLabel + "_TrxType"] != null)
                                                                                {
                                                                                    string attrTrxID = xRowNode.Attributes[strLabel + "_TrxID"].Value;
                                                                                    string attrTrxType = xRowNode.Attributes[strLabel + "_TrxType"].Value;
                                                                                    strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                                                    strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (strDefValue != string.Empty)
                                                                                {
                                                                                    string[] autoFillDef = strDefValue.Split('~');
                                                                                    string attrTrxID = autoFillDef[0].ToString();
                                                                                    string attrTrxType = autoFillDef[1].ToString();

                                                                                    strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                                                    strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            strBRowAttributes.Append(strLabel + "=\"" + dRow[strLabel].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
                                                                        }
                                                                        break;
                                                                    }
                                                                case "DDL":
                                                                    {
                                                                        XmlNode xDdlRowNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                                                        if (xDdlRowNode != null)
                                                                        {
                                                                            XmlNode xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "=" + CommonUI.ParseXpathString(dRow[strLabel].ToString()) + "]");
                                                                            string strRowValue = dRow[strLabel].ToString().ToLower();

                                                                            if (xDDLNode == null)
                                                                            {
                                                                                foreach (XmlNode xNode in xDdlRowNode.ChildNodes)
                                                                                {
                                                                                    if (strRowValue.Replace(" ", string.Empty) == xNode.Attributes[strLabel].Value.ToLower().Replace(" ", string.Empty))
                                                                                    {
                                                                                        strRowValue = xNode.Attributes[strLabel].Value;
                                                                                        xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "='" + strRowValue + "']");
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (xDDLNode != null)
                                                                            {
                                                                                string nodeTrxType = xDDLNode.Attributes[strLabel + "_TrxType"].Value;
                                                                                string nodeTrxID = xDDLNode.Attributes[strLabel + "_TrxID"].Value;
                                                                                strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                                                                strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");
                                                                            }
                                                                            else
                                                                            {
                                                                                if (strDefValue != string.Empty)
                                                                                {
                                                                                    XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                                                                                    string nodeTrxType = xDefaultNode.Attributes[strLabel + "_TrxType"].Value;
                                                                                    string nodeTrxID = xDefaultNode.Attributes[strLabel + "_TrxID"].Value;

                                                                                    strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                                                                    strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");

                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (strBRowAttributes.Length > 0)
                                                {
                                                    strBRowBPInfo.Append("<Rows BPAction=\"Add\" ");
                                                    strBRowBPInfo.Append(strBRowAttributes.ToString());
                                                    strBRowBPInfo.Append("/>");
                                                }
                                            }
                                            else
                                            {
                                                strNodeName = arrBranchNodeList[listCount - 1].ToString();
                                                m_listColumns = arrBranchColList[listCount - 1];
                                                strBRowBPInfo.Append("<" + strNodeName + ">");
                                                strBRowBPInfo.Append("<RowList>");
                                                drTrackedRows = dt.Select("TrackingReference='" + strTRef + "'");
                                                //XmlDocument xRowDoc = new XmlDocument();
                                                //XmlNode nodeRowList = xRowDoc.CreateNode(XmlNodeType.Element, "RowList", "");
                                                //xRowDoc.AppendChild(nodeRowList);

                                                List<string> lstRowList = new List<string>();

                                                foreach (DataRow dr in drTrackedRows)
                                                {
                                                    //RenderTrackedChildRec(xReturnDoc, m_listColumns, dr, strBRowBPInfo, dt, arrBranchNodeList, listCount, xRowDoc);
                                                    RenderTrackedChildRec(xReturnDoc, m_listColumns, dr, strBRowBPInfo, dt, arrBranchNodeList, listCount, lstRowList, dictAutoFills);
                                                }
                                            }

                                            strBRowBPInfo.Append("</RowList>");
                                            strBRowBPInfo.Append("</" + strNodeName + ">");
                                        }

                                        strBRowBPInfo.Append("</bpinfo>");
                                        //Add the nodes to the requestXML and send to the DB one row at a time
                                        strReqXml = string.Empty;
                                        strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                                        strReqXml = strReqXml + strBRowBPInfo.ToString() + "</Root>";
                                        //Get the OutXML for the particular individual rowEntry
                                        string strOutXml = string.Empty;
                                        strOutXml = objBO.GetDataForCPGV1(strReqXml);

                                        XmlDocument xOutXML = new XmlDocument();
                                        xOutXML.LoadXml(strOutXml);

                                        #region Message Info Logging using Error BPGID

                                        if (strErrorBPGID != string.Empty)
                                        {
                                            string strTrackingRef = string.Empty;
                                            string strTrackingRefAlt1 = string.Empty;
                                            string strTrackingRefAlt2 = string.Empty;
                                            string strTrackingRefAlt3 = string.Empty;

                                            string strTrackingInfo = string.Empty;
                                            string strTrackingInfoAlt1 = string.Empty;
                                            string strTrackingInfoAlt2 = string.Empty;
                                            string strTrackingInfoAlt3 = string.Empty;

                                            string strDateUpdated = string.Empty;

                                            if (dRow["TrackingReference"] != null)
                                                strTrackingRef = dRow["TrackingReference"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt1"))
                                                strTrackingRefAlt1 = dRow["TrackingRefAlt1"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt2"))
                                                strTrackingRefAlt2 = dRow["TrackingRefAlt2"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt3"))
                                                strTrackingRefAlt3 = dRow["TrackingRefAlt3"].ToString();

                                            if (dt.Columns.Contains("TrackingInfo"))
                                                strTrackingInfo = dRow["TrackingInfo"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt1"))
                                                strTrackingInfoAlt1 = dRow["TrackingInfoAlt1"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt2"))
                                                strTrackingInfoAlt2 = dRow["TrackingInfoAlt2"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt3"))
                                                strTrackingInfoAlt3 = dRow["TrackingInfoAlt3"].ToString();

                                            if (dt.Columns.Contains("DateUpdated"))
                                                strDateUpdated = dRow["DateUpdated"].ToString();

                                            string strMsgInfo = xOutXML.SelectSingleNode("Root/bpeout/MessageInfo").OuterXml.ToString();
                                            StringBuilder strBBPInfo = new StringBuilder();

                                            strBBPInfo.Append("<bpinfo>");
                                            strBBPInfo.Append("<BPGID>" + strErrorBPGID + "</BPGID>");
                                            strBBPInfo.Append("<BulkError><RowList><Rows ");
                                            strBBPInfo.Append("BPAction=\"Add\" ");
                                            strBBPInfo.Append("BPMessage=\"" + System.Security.SecurityElement.Escape(strMsgInfo) + "\" ");
                                            strBBPInfo.Append("TrackingReference=\"" + strTrackingRef + "\" ");
                                            strBBPInfo.Append("TrackingRefAlt1=\"" + strTrackingRefAlt1 + "\" ");
                                            strBBPInfo.Append("TrackingRefAlt2=\"" + strTrackingRefAlt2 + "\" ");
                                            strBBPInfo.Append("TrackingRefAlt3=\"" + strTrackingRefAlt3 + "\" ");
                                            strBBPInfo.Append("TrackingInfo=\"" + strTrackingInfo + "\" ");
                                            strBBPInfo.Append("TrackingInfoAlt1=\"" + strTrackingInfoAlt1 + "\" ");
                                            strBBPInfo.Append("TrackingInfoAlt2=\"" + strTrackingInfoAlt2 + "\" ");
                                            strBBPInfo.Append("TrackingInfoAlt3=\"" + strTrackingInfoAlt3 + "\" ");
                                            strBBPInfo.Append("DateUpdated=\"" + strDateUpdated + "\"></Rows>");
                                            strBBPInfo.Append("</RowList></BulkError></bpinfo>");

                                            //Add the nodes to the requestXML and send to the DB one row at a time
                                            strReqXml = string.Empty;
                                            strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                                            strReqXml = strReqXml + strBBPInfo.ToString() + "</Root>";

                                            //Get the OutXML for the particular individual rowEntry
                                            strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                        }

                                        #endregion

                                        if (drTrackedRows != null)
                                        {
                                            foreach (DataRow dTrackedRow in drTrackedRows)
                                            {
                                                dt.Rows.Remove(dTrackedRow);
                                            }
                                        }
                                        else
                                        {
                                            dt.Rows.Remove(dRow);
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows.Remove(dRow);
                                    }
                                }
                            }

                            //sWriter.Close();
                            //notUplRowFile.Close();
                            //// creating a file and saving it on the cleints machine.
                            //FileInfo newFile = new FileInfo(fileName);
                            //commonObjUI.SaveToClient(newFile);

                            lblDescriptionText.Visible = true;
                            lblDescriptionText.ForeColor = Color.Red;
                            lblDescriptionText.Text = "Successfully Uploaded " + recordCount + " Records";
                        }
                        //}
                    }
                }
            }
            //catch (Exception ex)
            //{
            //    lblDescriptionText.Visible = true;
            //    lblDescriptionText.ForeColor = Color.Red;
            //    lblDescriptionText.Text = "Upload Failed!!";
            //}
            //System.Diagnostics.Debugger.Log(1, "Upload", "\n Time :" + st.ElapsedMilliseconds.ToString());
            //st.Stop();
        }

        private void UploadData()
        {
            string m_ReqXML = string.Empty;
            string m_FilePath = string.Empty;
            string m_Extn = string.Empty;
            string m_UserBPE = string.Empty;
            string m_PhysicalFilePath = string.Empty;
            string strReqXml = string.Empty;
            string strReqBPGID = string.Empty;
            string strErrorBPGID = string.Empty;

            //Document to load the ViewState XML
            XmlDocument xReturnDoc = new XmlDocument();
            int recordCount = 0;
            //List to load Parent and Child Col Values
            List<GetColAttr> m_listColumns = new List<GetColAttr>();
            try
            {
                m_Extn = Path.GetExtension(FileAttachment.PostedFile.FileName);
                m_UserBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                m_PhysicalFilePath = ConfigurationManager.AppSettings["TempFilePath"].ToString();

                //Start Forming the RequestXML
                strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                if (FileAttachment.HasFile)
                {
                    string m_FileName = string.Empty;
                    int fileSize = FileAttachment.PostedFile.ContentLength;
                    fileSize = fileSize / 1024;
                    m_UserBPE = string.Empty;
                    m_UserBPE = Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString());
                    m_UserBPE = m_UserBPE + "~" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                    m_UserBPE = m_UserBPE + m_Extn;

                    if (!Directory.Exists(m_PhysicalFilePath))
                    {
                        Directory.CreateDirectory(m_PhysicalFilePath);
                    }

                    FileAttachment.PostedFile.SaveAs(m_PhysicalFilePath + "\\" + m_UserBPE);
                    m_FilePath = m_PhysicalFilePath + "\\" + m_UserBPE;
                    lblmsg.Visible = false;
                    object obj;
                    obj = UploadTexts(m_FilePath, true);

                    //Load the viewState return XML in returnDoc
                    if (ViewState["returnXML"].ToString() != string.Empty)
                    {
                        xReturnDoc.LoadXml(ViewState["returnXML"].ToString());
                    }

                    //Get the BPGID
                    XmlNode xBusProc = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                    strReqBPGID = xBusProc.SelectSingleNode("BusinessProcess[@ID='Add']").Attributes["BPGID"].Value.ToString();

                    if (xBusProc.SelectSingleNode("BusinessProcess[@ID='ErrorAddBPGID']") != null)
                        strErrorBPGID = xBusProc.SelectSingleNode("BusinessProcess[@ID='ErrorAddBPGID']").Attributes["BPGID"].Value.ToString();

                    //Get the Parent Node Name
                    XmlNode nodeTree = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    string strParentNodeName = nodeTree.SelectSingleNode("Node").InnerText;

                    //Get the Branch Node Names
                    ArrayList arrBranchNodeList = new ArrayList();
                    XmlNode nodeAllBranches = nodeTree.SelectSingleNode("Branches");

                    if (nodeAllBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeAllBranches.ChildNodes)
                        {
                            string strBranchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            arrBranchNodeList.Add(strBranchNodeName);
                        }
                    }

                    //Navigate to the Parent Node
                    List<GetColAttr> parentColAttrList = new List<GetColAttr>();
                    XmlNode nodeParentCols = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strParentNodeName + "/GridHeading/Columns");

                    //Load the the ParentColAttributesList
                    foreach (XmlNode nodeChildCol in nodeParentCols.ChildNodes)
                    {
                        GetColAttr parentColAttr = ParseColAttr(nodeChildCol);
                        parentColAttrList.Add(parentColAttr);
                    }

                    ArrayList arrBranchColList = new ArrayList();
                    if (arrBranchNodeList.Count > 0)
                    {
                        IEnumerator enumBranch = arrBranchNodeList.GetEnumerator();
                        //ArrayList which contains the ColAttributesLists of all the branchNodes 
                        while (enumBranch.MoveNext())
                        {
                            List<GetColAttr> branchColAttrList = new List<GetColAttr>();
                            //Load Individual ColListAttributes of each Treenode and save it in branchColAttrList
                            XmlNode nodeBranchCols = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + enumBranch.Current.ToString() + "/GridHeading/Columns");
                            foreach (XmlNode nodeChildCol in nodeBranchCols.ChildNodes)
                            {
                                GetColAttr branchColAttr = ParseColAttr(nodeChildCol);
                                branchColAttrList.Add(branchColAttr);
                            }
                            //Once we get the colAttributes List of all the columns load the List into ArrayList
                            arrBranchColList.Add(branchColAttrList);
                        }
                    }

                    //Data Table to Load the Excel Sheet Data
                    DataTable dt = new DataTable();
                    string strSheetNameRange = string.Empty;

                    //Array to get the SheetNames
                    string[] sheetnames = GetExcelSheetNames();
                    string str = string.Empty;

                    if (sheetnames != null)
                    {
                        int count = sheetnames.Length;
                        //Iterate through the Sheets
                        //for (int i = 0; i < count; i++)
                        //{
                        //strSheetNameRange = sheetnames[i].ToString();
                        strSheetNameRange = sheetnames[0].ToString();
                        if (strSheetNameRange.Substring(strSheetNameRange.Length - 1, 1).ToString() == "$")
                        {
                            //Get the content of the Sheet into the DataTable
                            dt = GetDataTable(strSheetNameRange);
                            //Add Columns which are present in xml but not in datatable and whose IsRequired Attribute is "1" to DataTable
                            AddColToDataTable(xReturnDoc, dt, strParentNodeName, arrBranchNodeList);
                            recordCount = recordCount + dt.Rows.Count;
                            int excelRowCount = 0;

                            //DataTable for Loading Rows which are not uploaded 
                            //DataTable dtNotUploaded = new DataTable();
                            //dtNotUploaded = dt.Clone();
                            //string fileName = ConfigurationManager.AppSettings["TempFilePath"] + @"\TrackingReferenceList" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".txt";
                            //FileStream notUplRowFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                            //StreamWriter sWriter = new StreamWriter(notUplRowFile);

                            //Iterate through each row
                            for (int rowCount = 0; rowCount <= dt.Rows.Count; rowCount++)
                            {
                                excelRowCount++;
                                //System.Diagnostics.Debugger.Log(1, "Upload", "\nOld -- Current Row : " + excelRowCount);
                                rowCount = 0;
                                DataRow dRow = dt.Rows[0];
                                //int rowcount = dt.Rows.IndexOf(dRow);

                                if (dt.Columns.Contains("Import"))
                                {
                                    if (dRow["Import"].ToString().ToLower() == "true")
                                    {
                                        DataRow[] drTrackedRows = null;

                                        XmlDocument xRowListDoc = new XmlDocument();

                                        //Initiate Forming the BPFInfo
                                        XmlNode nodeBPInfo = xRowListDoc.CreateNode(XmlNodeType.Element, "bpinfo", string.Empty);
                                        xRowListDoc.AppendChild(nodeBPInfo);

                                        XmlNode nodeBPGID = xRowListDoc.CreateNode(XmlNodeType.Element, "BPGID", string.Empty);
                                        nodeBPGID.InnerText = strReqBPGID;
                                        nodeBPInfo.AppendChild(nodeBPGID);

                                        //Iterate through the parent and ChildCol Lists.....arrBranchColList.Count + 1 is to include the parent also
                                        for (int listCount = 0; listCount < arrBranchColList.Count + 1; listCount++)
                                        {
                                            //Get the colNames of the DataTable
                                            IEnumerator colNameEnum = dt.Columns.GetEnumerator();
                                            string strNodeName = string.Empty;
                                            string strBranchNodeName = string.Empty;

                                            //If ListCount = 0 set m_ListColumns to parentColAttrList else to the corresponding branchColAttrList
                                            if (listCount == 0)
                                            {
                                                strNodeName = strParentNodeName;
                                                m_listColumns = parentColAttrList;
                                            }
                                            else
                                            {
                                                strBranchNodeName = arrBranchNodeList[listCount - 1].ToString();
                                                strNodeName = strBranchNodeName;
                                                m_listColumns = (List<GetColAttr>)arrBranchColList[listCount - 1];
                                            }

                                            //Attach the corresponding nodeName to the bpinfo
                                            XmlNode xTreeNode = xRowListDoc.CreateNode(XmlNodeType.Element, strNodeName, string.Empty);
                                            nodeBPInfo.AppendChild(xTreeNode);

                                            //Add RowList
                                            XmlNode nodeRowList = xRowListDoc.CreateNode(XmlNodeType.Element, "RowList", string.Empty);
                                            xTreeNode.AppendChild(nodeRowList);

                                            //DataRow[] drTrackedRows = null;
                                            string strTRef = dRow["TrackingReference"].ToString();

                                            if (listCount == 0)
                                            {
                                                //Node for adding the rowContent
                                                XmlNode nodeRows = xRowListDoc.CreateNode(XmlNodeType.Element, "Rows", string.Empty);
                                                Hashtable htDefInserted = new Hashtable();

                                                XmlAttribute attrBPAction = xRowListDoc.CreateAttribute("BPAction");
                                                attrBPAction.Value = "Add";
                                                nodeRows.Attributes.Append(attrBPAction);

                                                while (colNameEnum.MoveNext())
                                                {
                                                    string strLabel = colNameEnum.Current.ToString();

                                                    //Iterate Through the Content of the list
                                                    for (int subListCount = 0; subListCount < m_listColumns.Count; subListCount++)
                                                    {
                                                        string strColLabel = m_listColumns[subListCount].Label;

                                                        //Attributes to load the TrxType and TrxID for caorresponding or default dropdowns
                                                        XmlAttribute nodeTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");
                                                        XmlAttribute nodeTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");

                                                        XmlAttribute xAttr = xRowListDoc.CreateAttribute(strColLabel);
                                                        XmlAttribute xAttrTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");
                                                        XmlAttribute xAttrTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");

                                                        //Get the DefAultValue from the corresponding list 
                                                        string strDefValue = m_listColumns[subListCount].DefaultValue;

                                                        //If colLabel in list is Equal to ColLabel in the DataTable
                                                        if (strColLabel == strLabel)
                                                        {
                                                            //Check if the Default Value Exists
                                                            if (dRow[strLabel].ToString().Trim() == string.Empty)
                                                            {
                                                                if (m_listColumns[subListCount].DefaultValue != string.Empty)
                                                                {
                                                                    strDefValue = m_listColumns[subListCount].DefaultValue;

                                                                    switch (m_listColumns[subListCount].ControlType)
                                                                    {
                                                                        default:
                                                                            {
                                                                                if (strDefValue != string.Empty)
                                                                                {
                                                                                    xAttr.Value = strDefValue;
                                                                                    nodeRows.Attributes.Append(xAttr);
                                                                                }
                                                                                break;
                                                                            }
                                                                        //Load the TrxType and TrxID 
                                                                        case "DDL":
                                                                            {
                                                                                Debug.WriteLine("1Appending Default Values for the DDL : " + strLabel);
                                                                                XmlNode xDDLNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                                                                nodeTrxType.Value = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxType"].Value.ToString();
                                                                                nodeTrxID.Value = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxID"].Value.ToString();

                                                                                nodeRows.Attributes.Append(nodeTrxType);
                                                                                nodeRows.Attributes.Append(nodeTrxID);
                                                                                break;
                                                                            }
                                                                        case "TBox":
                                                                            {
                                                                                if (m_listColumns[subListCount].ControlType == "TBox" && m_listColumns[subListCount].IsLink == true)
                                                                                {
                                                                                    string[] autoFillDef = strDefValue.Split('~');
                                                                                    xAttrTrxID.Value = autoFillDef[0].ToString();
                                                                                    nodeRows.Attributes.Append(xAttrTrxID);

                                                                                    xAttrTrxType.Value = autoFillDef[1].ToString();
                                                                                    nodeRows.Attributes.Append(xAttrTrxType);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (strDefValue != string.Empty)
                                                                                    {
                                                                                        xAttr.Value = strDefValue;
                                                                                        nodeRows.Attributes.Append(xAttr);
                                                                                    }
                                                                                }
                                                                                break;
                                                                            }
                                                                    }
                                                                    htDefInserted[strLabel] = "true";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                switch (m_listColumns[subListCount].ControlType)
                                                                {
                                                                    default:
                                                                        {
                                                                            xAttr.Value = dRow[strLabel].ToString();
                                                                            nodeRows.Attributes.Append(xAttr);
                                                                            break;
                                                                        }
                                                                    #region Case TBox
                                                                    case "TBox":
                                                                        {
                                                                            if (m_listColumns[subListCount].ControlType == "TBox" && m_listColumns[subListCount].IsLink == true)
                                                                            {
                                                                                string strAutoFillData = dRow[strLabel].ToString();
                                                                                XmlNode xNode = AutoFill.GetAutoFillDataNode(strAutoFillData.Replace("&", "&amp;"), "AutoFillGeneric", strLabel, string.Empty);
                                                                                XmlNode xRowNode = xNode.SelectSingleNode("RowList");
                                                                                if (xRowNode != null)
                                                                                {
                                                                                    //Check if the query returned exactly one row.
                                                                                    if (xRowNode.ChildNodes.Count == 1)
                                                                                    {
                                                                                        if (xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"] != null && xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"] != null)
                                                                                        {
                                                                                            xAttrTrxID.Value = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"].Value;
                                                                                            nodeRows.Attributes.Append(xAttrTrxID);

                                                                                            xAttrTrxType.Value = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"].Value;
                                                                                            nodeRows.Attributes.Append(xAttrTrxType);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        XmlNode nodeAF = null;
                                                                                        //Get the exact row from the results list.
                                                                                        for (int i = 0; i < xRowNode.ChildNodes.Count; i++)
                                                                                        {
                                                                                            XmlNode nodeCurr = xRowNode.ChildNodes[i];
                                                                                            if (nodeCurr.Attributes[strLabel].Value.ToUpper().Trim() == strAutoFillData.ToUpper().Trim())
                                                                                            {
                                                                                                nodeAF = nodeCurr;
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                        if (nodeAF != null)
                                                                                        {
                                                                                            if (nodeAF.Attributes[strLabel + "_TrxID"] != null && nodeAF.Attributes[strLabel + "_TrxType"] != null)
                                                                                            {
                                                                                                xAttrTrxID.Value = nodeAF.Attributes[strLabel + "_TrxID"].Value;
                                                                                                nodeRows.Attributes.Append(xAttrTrxID);
                                                                                                xAttrTrxType.Value = nodeAF.Attributes[strLabel + "_TrxType"].Value;
                                                                                                nodeRows.Attributes.Append(xAttrTrxType);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //TODO
                                                                                            //throw new Exception("No matching records found.");
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (strDefValue != string.Empty)
                                                                                    {
                                                                                        string[] autoFillDef = strDefValue.Split('~');
                                                                                        xAttrTrxID.Value = autoFillDef[0].ToString();
                                                                                        nodeRows.Attributes.Append(xAttrTrxID);

                                                                                        xAttrTrxType.Value = autoFillDef[1].ToString();
                                                                                        nodeRows.Attributes.Append(xAttrTrxType);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                xAttr.Value = dRow[strLabel].ToString();
                                                                                nodeRows.Attributes.Append(xAttr);
                                                                            }
                                                                            break;
                                                                        }
                                                                    #endregion
                                                                    case "DDL":
                                                                        {
                                                                            XmlNode xDdlRowNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                                                            if (xDdlRowNode != null)
                                                                            {
                                                                                Debug.WriteLine("1.Node DDL Search using XPath Label: " + strLabel + " Value: " + CommonUI.ParseXpathString(dRow[strLabel].ToString()));
                                                                                XmlNode xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "=" + CommonUI.ParseXpathString(dRow[strLabel].ToString()) + "]");
                                                                                string strRowValue = dRow[strLabel].ToString().ToLower();

                                                                                if (xDDLNode == null)
                                                                                {
                                                                                    Debug.WriteLine("1.Searching for DDL text : " + CommonUI.ParseXpathString(dRow[strLabel].ToString()) + " failed. Looping through all the records.");
                                                                                    foreach (XmlNode xNode in xDdlRowNode.ChildNodes)
                                                                                    {
                                                                                        if (strRowValue.Replace(" ", string.Empty) == xNode.Attributes[strLabel].Value.ToLower().Replace(" ", string.Empty))
                                                                                        {
                                                                                            strRowValue = xNode.Attributes[strLabel].Value;
                                                                                            xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "='" + strRowValue + "']");
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                }

                                                                                if (xDDLNode != null)
                                                                                {
                                                                                    Debug.WriteLine("1.Node found. " + xDDLNode.OuterXml);
                                                                                    nodeTrxType.Value = xDDLNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                                                                    nodeTrxID.Value = xDDLNode.Attributes[strLabel + "_TrxID"].Value.ToString();

                                                                                    nodeRows.Attributes.Append(nodeTrxType);
                                                                                    nodeRows.Attributes.Append(nodeTrxID);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (strDefValue != string.Empty)
                                                                                    {
                                                                                        Debug.WriteLine("1.All attempts to find data for Label " + strLabel +" and value "+CommonUI.ParseXpathString(dRow[strLabel].ToString())+" have failed. Default value is available so appending that.");
                                                                                        XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                                                                                        nodeTrxType.Value = xDefaultNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                                                                        nodeTrxID.Value = xDefaultNode.Attributes[strLabel + "_TrxID"].Value.ToString();

                                                                                        nodeRows.Attributes.Append(nodeTrxType);
                                                                                        nodeRows.Attributes.Append(nodeTrxID);
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                }
                                                            }


                                                            break;
                                                        }
                                                        #region CommentedCode
                                                        //else
                                                        //{
                                                        //    //Even if the Col Label in the xml doesnt equal with the label in the list include the default values of the dropDownLists
                                                        //    if (htDefInserted[strLabel] == null || htDefInserted[strLabel].ToString() != "true")
                                                        //    {
                                                        //        if (m_listColumns[subListCount].DefaultValue != string.Empty)
                                                        //        {
                                                        //            switch (m_listColumns[subListCount].ControlType)
                                                        //            {
                                                        //                default:
                                                        //                    {
                                                        //                        xAttr.Value = m_listColumns[subListCount].DefaultValue;
                                                        //                        nodeRows.Attributes.Append(xAttr);
                                                        //                        break;
                                                        //                    }
                                                        //                case "DDL":
                                                        //                    {
                                                        //                        XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strColLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                                                        //                        nodeTrxType.Value = xDefaultNode.Attributes[strColLabel + "_TrxType"].Value.ToString();
                                                        //                        nodeTrxID.Value = xDefaultNode.Attributes[strColLabel + "_TrxID"].Value.ToString();

                                                        //                        nodeRows.Attributes.Append(nodeTrxType);
                                                        //                        nodeRows.Attributes.Append(nodeTrxID);
                                                        //                        break;
                                                        //                    }
                                                        //                case "TBox":
                                                        //                    {
                                                        //                        if (m_listColumns[subListCount].IsLink == true)
                                                        //                        {
                                                        //                            string[] autoFillDef = strDefValue.Split('~');
                                                        //                            xAttrTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");
                                                        //                            xAttrTrxID.Value = autoFillDef[0].ToString();
                                                        //                            nodeRows.Attributes.Append(xAttrTrxID);

                                                        //                            xAttrTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");
                                                        //                            xAttrTrxType.Value = autoFillDef[1].ToString();
                                                        //                            nodeRows.Attributes.Append(xAttrTrxType);
                                                        //                        }
                                                        //                        else
                                                        //                        {
                                                        //                            xAttr.Value = m_listColumns[subListCount].DefaultValue;
                                                        //                            nodeRows.Attributes.Append(xAttr);
                                                        //                        }
                                                        //                        break;
                                                        //                    }
                                                        //            }
                                                        //        }
                                                        //    }
                                                        //}
                                                        #endregion
                                                    }
                                                }
                                                if (nodeRows.Attributes.Count != 0)
                                                    nodeRowList.AppendChild(nodeRows);
                                            }
                                            else
                                            {
                                                drTrackedRows = dt.Select("TrackingReference='" + strTRef + "'");

                                                foreach (DataRow dr in drTrackedRows)
                                                {
                                                    RenderTrackedChildRec(xReturnDoc, m_listColumns, dr, xRowListDoc, dt, nodeRowList, arrBranchNodeList, listCount);
                                                }
                                            }
                                        }

                                        //Add the nodes to the requestXML and send to the DB one row at a time
                                        strReqXml = string.Empty;
                                        strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                                        strReqXml = strReqXml + xRowListDoc.OuterXml.ToString() + "</Root>";

                                        //Get the OutXML for the particular individual rowEntry
                                        string strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                        XmlDocument xOutXML = new XmlDocument();
                                        xOutXML.LoadXml(strOutXml);

                                        #region Commented Code
                                        //Upload the row into dtNotUploaded DataTable
                                        //DataRow dUnUploadedRow = dtNotUploaded.NewRow();
                                        //dUnUploadedRow.ItemArray = dRow.ItemArray;
                                        //dtNotUploaded.Rows.Add(dUnUploadedRow);

                                        ////Writing the TrackingReference column value to the file
                                        //sWriter.WriteLine(excelRowCount + ". " + dRow["TrackingReference"]);
                                        //sWriter.WriteLine("__________________");

                                        //Get the Message Status from the outXML
                                        //string strMessage = xOutXML.SelectSingleNode("Root/bpeout/MessageInfo/Status").InnerText.ToString();
                                        //if (strMessage == "Error")
                                        //{
                                        //} 
                                        #endregion

                                        #region Error BPGID Re-Submission

                                        if (strErrorBPGID != string.Empty)
                                        {
                                            xRowListDoc = new XmlDocument();
                                            XmlNode xBPInfo = xRowListDoc.CreateNode(XmlNodeType.Element, "bpinfo", string.Empty);
                                            xRowListDoc.AppendChild(xBPInfo);

                                            XmlNode xErrorBPGID = xRowListDoc.CreateNode(XmlNodeType.Element, "BPGID", string.Empty);
                                            xErrorBPGID.InnerText = strErrorBPGID;
                                            xBPInfo.AppendChild(xErrorBPGID);

                                            XmlNode xBulkError = xRowListDoc.CreateNode(XmlNodeType.Element, "BulkError", string.Empty);
                                            xBPInfo.AppendChild(xBulkError);

                                            XmlNode xRowList = xRowListDoc.CreateNode(XmlNodeType.Element, "RowList", string.Empty);
                                            xBulkError.AppendChild(xRowList);

                                            //Node for adding the rowContent
                                            XmlNode xRows = xRowListDoc.CreateNode(XmlNodeType.Element, "Rows", string.Empty);
                                            xRowList.AppendChild(xRows);

                                            string strMsgInfo = xOutXML.SelectSingleNode("Root//bpeout//MessageInfo").OuterXml.ToString();
                                            XmlAttribute xAttrBPMessage = xRowListDoc.CreateAttribute("BPMessage");
                                            xAttrBPMessage.Value = strMsgInfo;
                                            xRows.Attributes.Append(xAttrBPMessage);

                                            string strTrackingRef = string.Empty;
                                            string strTrackingRefAlt1 = string.Empty;
                                            string strTrackingRefAlt2 = string.Empty;
                                            string strTrackingRefAlt3 = string.Empty;

                                            string strTrackingInfo = string.Empty;
                                            string strTrackingInfoAlt1 = string.Empty;
                                            string strTrackingInfoAlt2 = string.Empty;
                                            string strTrackingInfoAlt3 = string.Empty;

                                            string strDateUpdated = string.Empty;

                                            if (dRow["TrackingReference"] != null)
                                                strTrackingRef = dRow["TrackingReference"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt1"))
                                                strTrackingRefAlt1 = dRow["TrackingRefAlt1"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt2"))
                                                strTrackingRefAlt2 = dRow["TrackingRefAlt2"].ToString();

                                            if (dt.Columns.Contains("TrackingRefAlt3"))
                                                strTrackingRefAlt3 = dRow["TrackingRefAlt3"].ToString();

                                            if (dt.Columns.Contains("TrackingInfo"))
                                                strTrackingInfo = dRow["TrackingInfo"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt1"))
                                                strTrackingInfoAlt1 = dRow["TrackingInfoAlt1"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt2"))
                                                strTrackingInfoAlt2 = dRow["TrackingInfoAlt2"].ToString();

                                            if (dt.Columns.Contains("TrackingInfoAlt3"))
                                                strTrackingInfoAlt3 = dRow["TrackingInfoAlt3"].ToString();

                                            if (dt.Columns.Contains("DateUpdated"))
                                                strDateUpdated = dRow["DateUpdated"].ToString();

                                            XmlAttribute xAttrTrackingRef = xRowListDoc.CreateAttribute("TrackingReference");
                                            xAttrTrackingRef.Value = strTrackingRef;
                                            xRows.Attributes.Append(xAttrTrackingRef);

                                            XmlAttribute xAttrTrackingRefAlt1 = xRowListDoc.CreateAttribute("TrackingRefAlt1");
                                            xAttrTrackingRefAlt1.Value = strTrackingRefAlt1;
                                            xRows.Attributes.Append(xAttrTrackingRefAlt1);

                                            XmlAttribute xAttrTrackingRefAlt2 = xRowListDoc.CreateAttribute("TrackingRefAlt2");
                                            xAttrTrackingRefAlt2.Value = strTrackingRefAlt2;
                                            xRows.Attributes.Append(xAttrTrackingRefAlt2);

                                            XmlAttribute xAttrTrackingRefAlt3 = xRowListDoc.CreateAttribute("TrackingRefAlt3");
                                            xAttrTrackingRefAlt3.Value = strTrackingRefAlt3;
                                            xRows.Attributes.Append(xAttrTrackingRefAlt3);

                                            XmlAttribute xAttrTrackingInfo = xRowListDoc.CreateAttribute("TrackingInfo");
                                            xAttrTrackingInfo.Value = strTrackingInfo;
                                            xRows.Attributes.Append(xAttrTrackingInfo);

                                            XmlAttribute xAttrTrackingInfoAlt1 = xRowListDoc.CreateAttribute("TrackingInfoAlt1");
                                            xAttrTrackingInfoAlt1.Value = strTrackingInfoAlt1;
                                            xRows.Attributes.Append(xAttrTrackingInfoAlt1);

                                            XmlAttribute xAttrTrackingInfoAlt2 = xRowListDoc.CreateAttribute("TrackingInfoAlt2");
                                            xAttrTrackingInfoAlt2.Value = strTrackingInfoAlt2;
                                            xRows.Attributes.Append(xAttrTrackingInfoAlt2);

                                            XmlAttribute xAttrTrackingInfoAlt3 = xRowListDoc.CreateAttribute("TrackingInfoAlt3");
                                            xAttrTrackingInfoAlt3.Value = strTrackingInfoAlt3;
                                            xRows.Attributes.Append(xAttrTrackingInfoAlt3);

                                            XmlAttribute xAttrDateUpdated = xRowListDoc.CreateAttribute("DateUpdated");
                                            xAttrDateUpdated.Value = strDateUpdated;
                                            xRows.Attributes.Append(xAttrDateUpdated);

                                            XmlAttribute xAttrBPAction = xRowListDoc.CreateAttribute("BPAction");
                                            xAttrBPAction.Value = "Add";
                                            xRows.Attributes.Append(xAttrBPAction);

                                            //Add the nodes to the requestXML and send to the DB one row at a time
                                            strReqXml = string.Empty;
                                            strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                                            strReqXml = strReqXml + xRowListDoc.OuterXml.ToString() + "</Root>";

                                            //Get the OutXML for the particular individual rowEntry
                                            strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                        }
                                        #endregion

                                        if (drTrackedRows != null)
                                        {
                                            foreach (DataRow dTrackedRow in drTrackedRows)
                                            {
                                                dt.Rows.Remove(dTrackedRow);
                                            }
                                        }
                                        else
                                        {
                                            dt.Rows.Remove(dRow);
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows.Remove(dRow);
                                    }
                                }
                            }

                            //sWriter.Close();
                            //notUplRowFile.Close();
                            //// creating a file and saving it on the cleints machine.
                            //FileInfo newFile = new FileInfo(fileName);
                            //commonObjUI.SaveToClient(newFile);

                            lblDescriptionText.Visible = true;
                            lblDescriptionText.ForeColor = Color.Red;
                            lblDescriptionText.Text = "Successfully Uploaded " + recordCount + " Records";
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                lblDescriptionText.Visible = true;
                lblDescriptionText.ForeColor = Color.Red;
                lblDescriptionText.Text = string.Format("Upload Failed at row {1} : {0}", ex.Message, recordCount);
            }
        }

        private void AddColToDataTable(XmlDocument xReturnDoc, DataTable dt, string strParentNodeName, ArrayList arrBranchNodeList)
        {
            IEnumerator colEnum = dt.Columns.GetEnumerator();

            XmlNode parentColNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strParentNodeName + "/GridHeading/Columns");
            while (colEnum.MoveNext())
            {
                XmlNode colNode = parentColNode.SelectSingleNode("Col[@Label='" + colEnum.Current.ToString() + "']");
                if (colNode != null)
                {
                    parentColNode.RemoveChild(colNode);
                }
            }

            foreach (XmlNode xNode in parentColNode.ChildNodes)
            {
                /*This is to add those columns which are not their in excel but in xml and there is a default
               value asscoiated with it*/
                string strLabel = xNode.Attributes["Label"].Value;
                if (xNode.Attributes["Default"] != null)
                {
                    dt.Columns.Add(strLabel);
                }

                //string strRequired = xNode.Attributes["IsRequired"].Value;
                //string strLabel = xNode.Attributes["Label"].Value;
                //if (strRequired == "1")
                //{
                //    dt.Columns.Add(strLabel);
                //}
            }

            for (int branchCount = 0; branchCount < arrBranchNodeList.Count; branchCount++)
            {
                XmlNode branchColNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + arrBranchNodeList[branchCount] + "/GridHeading/Columns");
                colEnum = dt.Columns.GetEnumerator();
                while (colEnum.MoveNext())
                {
                    XmlNode colNode = branchColNode.SelectSingleNode("Col[@Label='" + colEnum.Current.ToString() + "']");
                    if (colNode != null)
                    {
                        branchColNode.RemoveChild(colNode);
                    }
                }

                foreach (XmlNode xNode in branchColNode.ChildNodes)
                {
                    //string strRequired = xNode.Attributes["IsRequired"].Value;
                    //string strLabel = xNode.Attributes["Label"].Value;
                    //if (strRequired == "1")
                    //{
                    //    dt.Columns.Add(strLabel);
                    //}

                    string strLabel = xNode.Attributes["Label"].Value;
                    if (xNode.Attributes["Default"] != null)
                    {
                        dt.Columns.Add(strLabel);
                    }
                }
            }

        }

        private static void RenderTrackedChildRec(XmlDocument xReturnDoc, List<GetColAttr> m_listColumns, DataRow dRow, XmlDocument xRowListDoc, DataTable dt, XmlNode nodeRowList, ArrayList arrBranchNodeList, int listCount)
        {
            #region NLog
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("This method is used to render the tracke child records from tehe return xml document");
            #endregion

            string strBranchName = arrBranchNodeList[listCount - 1].ToString();
            //Node for adding the rowContent
            XmlNode nodeRows = xRowListDoc.CreateNode(XmlNodeType.Element, "Rows", "");
            Hashtable htDefInserted = new Hashtable();
            IEnumerator colNameEnum = dt.Columns.GetEnumerator();

            XmlAttribute attrBPAction = xRowListDoc.CreateAttribute("BPAction");
            attrBPAction.Value = "Add";
            nodeRows.Attributes.Append(attrBPAction);

            while (colNameEnum.MoveNext())
            {
                string strLabel = colNameEnum.Current.ToString();

                //Iterate Through the Content of the list
                for (int subListCount = 0; subListCount < m_listColumns.Count; subListCount++)
                {
                    string strColLabel = m_listColumns[subListCount].Label;

                    //Attributes to load the TrxType and TrxID for caorresponding or default dropdowns
                    XmlAttribute nodeTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");
                    XmlAttribute nodeTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");

                    XmlAttribute xAttr = xRowListDoc.CreateAttribute(strColLabel);
                    XmlAttribute xAttrTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");
                    XmlAttribute xAttrTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");

                    //Get the DefAultValue from the corresponding list 
                    string strDefValue = m_listColumns[subListCount].DefaultValue;

                    //If colLabel in list is Equal to ColLabel in the DataTable
                    if (strColLabel == strLabel)
                    {
                        //If there is no data for the current column cell,check for the Default Value
                        if (dRow[strLabel].ToString().Trim() == "")
                        {
                            if (m_listColumns[subListCount].DefaultValue != "")
                            {
                                strDefValue = m_listColumns[subListCount].DefaultValue;

                                switch (m_listColumns[subListCount].ControlType)
                                {
                                    default:
                                        {
                                            if (strDefValue != "")
                                            {
                                                xAttr.Value = strDefValue;
                                                nodeRows.Attributes.Append(xAttr);
                                            }
                                            break;
                                        }
                                    //Load the TrxType and TrxID 
                                    case "DDL":
                                        {
                                            XmlNode xDDLNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                            XmlNode nodeDDLItem = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']");
                                            nodeTrxType.Value = nodeDDLItem.Attributes[strLabel + "_TrxType"].Value.ToString();
                                            nodeTrxID.Value = nodeDDLItem.Attributes[strLabel + "_TrxID"].Value.ToString();
                                            nodeRows.Attributes.Append(nodeTrxType);
                                            nodeRows.Attributes.Append(nodeTrxID);
                                            break;
                                        }
                                    case "TBox":
                                        {
                                            if (m_listColumns[subListCount].ControlType == "TBox" && m_listColumns[subListCount].IsLink == true)
                                            {
                                                string[] autoFillDef = strDefValue.Split('~');
                                                xAttrTrxID.Value = autoFillDef[0].ToString();
                                                nodeRows.Attributes.Append(xAttrTrxID);

                                                xAttrTrxType.Value = autoFillDef[1].ToString();
                                                nodeRows.Attributes.Append(xAttrTrxType);
                                            }
                                            else
                                            {
                                                if (strDefValue != "")
                                                {
                                                    xAttr.Value = strDefValue;
                                                    nodeRows.Attributes.Append(xAttr);
                                                }
                                            }
                                            break;
                                        }
                                }
                                htDefInserted[strLabel] = "true";
                            }
                        }
                        else
                        {
                            switch (m_listColumns[subListCount].ControlType)
                            {
                                default:
                                    {
                                        xAttr.Value = dRow[strLabel].ToString();
                                        nodeRows.Attributes.Append(xAttr);
                                        break;
                                    }
                                #region Case TBox
                                case "TBox":
                                    {
                                        if (m_listColumns[subListCount].ControlType == "TBox" && m_listColumns[subListCount].IsLink == true)
                                        {
                                            string strAutoFillData = dRow[strLabel].ToString();
                                            XmlNode xNode = AutoFill.GetAutoFillDataNode(strAutoFillData.Replace("&", "&amp;"), "AutoFillGeneric", strLabel, "");
                                            XmlNode xRowNode = xNode.SelectSingleNode("RowList");
                                            if (xRowNode != null)
                                            {
                                                //Check if the query returned exactly one row.
                                                if (xRowNode.ChildNodes.Count == 1)
                                                {
                                                    if (xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"] != null && xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"] != null)
                                                    {
                                                        xAttrTrxID.Value = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"].Value;
                                                        nodeRows.Attributes.Append(xAttrTrxID);

                                                        xAttrTrxType.Value = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"].Value;
                                                        nodeRows.Attributes.Append(xAttrTrxType);
                                                    }
                                                }
                                                else
                                                {
                                                    XmlNode nodeAF = null;
                                                    //Get the exact row from the results list.
                                                    for (int i = 0; i < xRowNode.ChildNodes.Count; i++)
                                                    {
                                                        XmlNode nodeCurr = xRowNode.ChildNodes[i];
                                                        if (nodeCurr.Attributes[strLabel].Value.ToUpper().Trim() == strAutoFillData.ToUpper().Trim())
                                                        {
                                                            nodeAF = nodeCurr;
                                                            break;
                                                        }
                                                    }
                                                    if (nodeAF != null)
                                                    {
                                                        if (nodeAF.Attributes[strLabel + "_TrxID"] != null && nodeAF.Attributes[strLabel + "_TrxType"] != null)
                                                        {
                                                            xAttrTrxID.Value = nodeAF.Attributes[strLabel + "_TrxID"].Value;
                                                            nodeRows.Attributes.Append(xAttrTrxID);
                                                            xAttrTrxType.Value = nodeAF.Attributes[strLabel + "_TrxType"].Value;
                                                            nodeRows.Attributes.Append(xAttrTrxType);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //TODO
                                                        //throw new Exception("No matching records found.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (strDefValue != "")
                                                {
                                                    string[] autoFillDef = strDefValue.Split('~');
                                                    xAttrTrxID.Value = autoFillDef[0].ToString();
                                                    nodeRows.Attributes.Append(xAttrTrxID);

                                                    xAttrTrxType.Value = autoFillDef[1].ToString();
                                                    nodeRows.Attributes.Append(xAttrTrxType);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            xAttr.Value = dRow[strLabel].ToString();
                                            nodeRows.Attributes.Append(xAttr);
                                        }
                                        break;
                                    } 
                                #endregion
                                case "DDL":
                                    {
                                        XmlNode xDdlRowNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                        if (xDdlRowNode != null)
                                        {
                                            XmlNode xDDLNode = null;
                                            string ddlText = CommonUI.ParseXpathString(dRow[strLabel].ToString());
                                            Debug.Write("\n2.DDL: " + strLabel + " Value: " + ddlText);
                                            try
                                            {
                                                xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "=" + CommonUI.ParseXpathString(dRow[strLabel].ToString()) + "]");
                                            }
                                            catch
                                            {
                                                string ddlText2 = CommonUI.ParseXPath(dRow[strLabel].ToString());
                                                #region NLog
                                                NLog.Logger loger = LogManager.GetCurrentClassLogger();
                                                loger.Fatal("Could not find the DDL node using XPath.First try text : " + ddlText + "Second try text using different XPath formatting : " + ddlText2);
                                                #endregion
                                                xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "=" + ddlText2 + "]");
                                            }

                                            //If XPath has failed to yield a valid xml node then try looping through the entire list.
                                            if (xDDLNode == null)
                                            {
                                                string strRowValue = dRow[strLabel].ToString().ToLower();
                                                foreach (XmlNode xNode in xDdlRowNode.ChildNodes)
                                                {
                                                    if (strRowValue.Replace(" ", string.Empty) == xNode.Attributes[strLabel].Value.ToLower().Replace(" ", string.Empty))
                                                    {
                                                        strRowValue = xNode.Attributes[strLabel].Value;
                                                        xDDLNode = xDdlRowNode.SelectSingleNode("Rows[@" + strLabel + "='" + strRowValue + "']");
                                                        Debug.Write(" Found after looping : " + strRowValue);
                                                        break;
                                                    }
                                                }
                                            }

                                            if (xDDLNode != null)
                                            {
                                                nodeTrxType.Value = xDDLNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                                nodeTrxID.Value = xDDLNode.Attributes[strLabel + "_TrxID"].Value.ToString();
                                                nodeRows.Attributes.Append(nodeTrxType);
                                                nodeRows.Attributes.Append(nodeTrxID);
                                                Debug.Write(" 2.Found : " + xDDLNode.Attributes[strLabel].Value);
                                            }
                                            else
                                            {
                                                if (strDefValue != "")
                                                {
                                                    XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                                                    nodeTrxType.Value = xDefaultNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                                    nodeTrxID.Value = xDefaultNode.Attributes[strLabel + "_TrxID"].Value.ToString();

                                                    nodeRows.Attributes.Append(nodeTrxType);
                                                    nodeRows.Attributes.Append(nodeTrxID);
                                                    Debug.Write(" 2.Setting Default after failing in all other attempts: " + xDefaultNode.Attributes[strLabel].Value);
                                                }
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }

                    #region CommentedCode
                    //else
                    //{
                    //    if (m_listColumns[subListCount].IsRequired != false)
                    //    {
                    //        //Even if the Col Label in the xml doesnt equal with the label in the list include the default values of the dropDownLists
                    //        if (htDefInserted[strLabel] == null || htDefInserted[strLabel].ToString() != "true")
                    //        {
                    //            if (m_listColumns[subListCount].DefaultValue != "")
                    //            {
                    //                switch (m_listColumns[subListCount].ControlType)
                    //                {
                    //                    default:
                    //                        {
                    //                            xAttr.Value = m_listColumns[subListCount].DefaultValue;
                    //                            nodeRows.Attributes.Append(xAttr);
                    //                            break;
                    //                        }
                    //                    case "DDL":
                    //                        {
                    //                            XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strColLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                    //                            nodeTrxType.Value = xDefaultNode.Attributes[strColLabel + "_TrxType"].Value.ToString();
                    //                            nodeTrxID.Value = xDefaultNode.Attributes[strColLabel + "_TrxID"].Value.ToString();

                    //                            nodeRows.Attributes.Append(nodeTrxType);
                    //                            nodeRows.Attributes.Append(nodeTrxID);
                    //                            break;
                    //                        }
                    //                    case "TBox":
                    //                        {
                    //                            if (m_listColumns[subListCount].IsLink == true)
                    //                            {
                    //                                string[] autoFillDef = strDefValue.Split('~');
                    //                                xAttrTrxID = xRowListDoc.CreateAttribute(strColLabel + "_TrxID");
                    //                                xAttrTrxID.Value = autoFillDef[0].ToString();
                    //                                nodeRows.Attributes.Append(xAttrTrxID);

                    //                                xAttrTrxType = xRowListDoc.CreateAttribute(strColLabel + "_TrxType");
                    //                                xAttrTrxType.Value = autoFillDef[1].ToString();
                    //                                nodeRows.Attributes.Append(xAttrTrxType);
                    //                            }
                    //                            else
                    //                            {
                    //                                xAttr.Value = m_listColumns[subListCount].DefaultValue;
                    //                                nodeRows.Attributes.Append(xAttr);
                    //                            }
                    //                            break;
                    //                        }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }

            int recurrentCount = 0;
            XmlNode xBranchNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node[text()='" + strBranchName + "']").ParentNode;

            if (xBranchNode.Attributes["ControlType"] == null)
            {
                if (nodeRowList.ChildNodes.Count != 0)
                {
                    foreach (XmlNode xNode in nodeRowList.ChildNodes)
                    {
                        if (xNode.OuterXml == nodeRows.OuterXml)
                        {
                            recurrentCount++;
                        }
                    }
                }
            }

            if (recurrentCount == 0)
            {
                if (nodeRows.Attributes.Count != 1)
                    nodeRowList.AppendChild(nodeRows);
            }
        }

        //New OverLoaded Method
        //private void RenderTrackedChildRec(XmlDocument xReturnDoc, List<GetColAttr> m_listColumns, DataRow dr, StringBuilder strBRowBPInfo, DataTable dt, ArrayList arrBranchNodeList, int listCount, XmlDocument xRowDoc)
        private void RenderTrackedChildRec(XmlDocument xReturnDoc, List<GetColAttr> m_listColumns, DataRow dr, StringBuilder strBRowBPInfo, DataTable dt, ArrayList arrBranchNodeList, int listCount, List<string> lstRowList, Dictionary<string, XmlNode> dictAutoFills)
        {
            string strOldRow = string.Empty;
            string strBranchName = arrBranchNodeList[listCount - 1].ToString();

            StringBuilder strBRowAttributes = new StringBuilder();

            //Get the colNames of the List
            IEnumerator colEnum = m_listColumns.GetEnumerator();

            for (int colCount = 0; colCount < m_listColumns.Count; colCount++)
            {
                string strLabel = m_listColumns[colCount].Label;

                //Get the DefAultValue from the corresponding list 
                string strDefValue = m_listColumns[colCount].DefaultValue;

                //If colLabel in list is Equal to ColLabel in the DataTable
                if (dt.Columns.Contains(strLabel))
                {
                    //Check if the Default Value Exists
                    if (dr[strLabel].ToString().Trim() == string.Empty)
                    {
                        if (m_listColumns[colCount].DefaultValue != string.Empty)
                        {
                            //strDefValue = m_listColumns[colCount].DefaultValue;
                            switch (m_listColumns[colCount].ControlType)
                            {
                                default:
                                    {
                                        if (strDefValue != string.Empty)
                                        {
                                            strBRowAttributes.Append(strLabel + "=\"" + strDefValue + "\" ");
                                        }
                                        break;
                                    }
                                //Load the TrxType and TrxID 
                                case "DDL":
                                    {
                                        XmlNode xDDLNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                        string nodeTrxType = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxType"].Value.ToString();
                                        string nodeTrxID = xDDLNode.SelectSingleNode("Rows[@TrxID='" + strDefValue + "']").Attributes[strLabel + "_TrxID"].Value.ToString();

                                        strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                        strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");
                                        break;
                                    }
                                case "TBox":
                                    {
                                        if (m_listColumns[colCount].ControlType == "TBox" && m_listColumns[colCount].IsLink == true)
                                        {
                                            string[] autoFillDef = strDefValue.Split('~');
                                            string attrTrxID = autoFillDef[0].ToString();
                                            string attrTrxType = autoFillDef[1].ToString();

                                            strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                            strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                        }
                                        else
                                        {
                                            if (strDefValue != string.Empty)
                                            {
                                                strBRowAttributes.Append(strLabel + "=\"" + strDefValue + "\" ");
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        switch (m_listColumns[colCount].ControlType)
                        {
                            default:
                                {
                                    strBRowAttributes.Append(strLabel + "=\"" + dr[strLabel].ToString() + "\" ");
                                    break;
                                }
                            case "TBox":
                                {
                                    if (m_listColumns[colCount].ControlType == "TBox" && m_listColumns[colCount].IsLink == true)
                                    {
                                        XmlNode xNode = dictAutoFills[strLabel];
                                        string strAutoFillData = dr[strLabel].ToString();
                                        XmlNode xRowNode = GetAutoFillItem(strLabel, strAutoFillData, xNode);

                                        //XmlNode xNodeRow = AutoFill.GetAutoFillDataNode(strAutoFillData.Replace("&", "&amp;"), "AutoFillGeneric", strLabel, string.Empty);
                                        //XmlNode xRowNode = xNode.SelectSingleNode("RowList");
                                        //if (xRowNode != null)
                                        //{
                                        //    if (xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"] != null && xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"] != null)
                                        //    {
                                        //        string attrTrxID = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxID"].Value;
                                        //        string attrTrxType = xRowNode.ChildNodes[0].Attributes[strLabel + "_TrxType"].Value;
                                        //        strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                        //        strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                        //    }
                                        //}
                                        //if (xRowNodes != null)
                                        if (xRowNode != null)
                                        {
                                            if (xRowNode.Attributes[strLabel + "_TrxID"] != null && xRowNode.Attributes[strLabel + "_TrxType"] != null)
                                            {
                                                string attrTrxID = xRowNode.Attributes[strLabel + "_TrxID"].Value;
                                                string attrTrxType = xRowNode.Attributes[strLabel + "_TrxType"].Value;
                                                strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                            }
                                        }
                                        else
                                        {
                                            if (strDefValue != string.Empty)
                                            {
                                                string[] autoFillDef = strDefValue.Split('~');
                                                string attrTrxID = autoFillDef[0].ToString();
                                                string attrTrxType = autoFillDef[1].ToString();

                                                strBRowAttributes.Append(strLabel + "_TrxID=\"" + attrTrxID + "\" ");
                                                strBRowAttributes.Append(strLabel + "_TrxType=\"" + attrTrxType + "\" ");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strBRowAttributes.Append(strLabel + "=\"" + dr[strLabel].ToString().Replace("&", "&amp;").Replace("\"", "'") + "\" ");
                                    }
                                    break;
                                }
                            case "DDL":
                                {
                                    XmlNode xDdlRowNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList");
                                    if (xDdlRowNode != null)
                                    {
                                        XmlNode xDDLNode = null;
                                        string strRowValue = dr[strLabel].ToString().ToLower();

                                        for (int i = 0; i < xDdlRowNode.ChildNodes.Count; i++)
                                        {
                                            XmlNode xNode = xDdlRowNode.ChildNodes[i];
                                            if (strRowValue.Replace(" ", string.Empty) == xNode.Attributes[strLabel].Value.ToLower().Replace(" ", string.Empty))
                                            {
                                                strRowValue = xNode.Attributes[strLabel].Value;
                                                xDDLNode = xNode;
                                                break;
                                            }
                                        }

                                        if (xDDLNode != null)
                                        {
                                            string nodeTrxType = xDDLNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                            string nodeTrxID = xDDLNode.Attributes[strLabel + "_TrxID"].Value.ToString();

                                            strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                            strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");
                                        }
                                        else
                                        {
                                            if (strDefValue != string.Empty)
                                            {
                                                XmlNode xDefaultNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/" + strLabel + "/RowList/Rows[@TrxID='" + strDefValue + "']");
                                                string nodeTrxType = xDefaultNode.Attributes[strLabel + "_TrxType"].Value.ToString();
                                                string nodeTrxID = xDefaultNode.Attributes[strLabel + "_TrxID"].Value.ToString();

                                                strBRowAttributes.Append(strLabel + "_TrxID=\"" + nodeTrxID + "\" ");
                                                strBRowAttributes.Append(strLabel + "_TrxType=\"" + nodeTrxType + "\" ");
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }

            //XmlNode nodeRowList = lstRowList.SelectSingleNode("RowList");
            string strRowDef = "<Rows BPAction=\"Add\" ";
            string strRowEnd = "/>";

            int recurrentCount = 0;
            XmlNode xBranchNode = xReturnDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node[text()='" + strBranchName + "']").ParentNode;
            if (xBranchNode.Attributes["ControlType"] == null)
            {
                //if (nodeRowList.ChildNodes.Count != 0)
                if (lstRowList.Count != 0)
                {
                    //foreach (XmlNode xNode in nodeRowList.ChildNodes)
                    //{
                    //    if (xNode.OuterXml == strRowDef + strBRowAttributes.ToString() + strRowEnd)
                    //    {
                    if (lstRowList.Contains(strRowDef + strBRowAttributes.ToString() + strRowEnd))
                    {
                        recurrentCount++;
                    }
                    //    }
                    //}
                }
            }

            if (recurrentCount == 0)
            {
                if (strBRowAttributes.Length > 0)
                {
                    strBRowBPInfo.Append(strRowDef);
                    strBRowBPInfo.Append(strBRowAttributes.ToString());
                    strBRowBPInfo.Append(strRowEnd);

                    lstRowList.Add(strRowDef + strBRowAttributes.ToString() + strRowEnd);
                    //nodeRowList.InnerXml = nodeRowList.InnerXml + strRowDef + strBRowAttributes.ToString() + strRowEnd;
                }
            }
        }

        private XmlNode GetAutoFillItem(string attName, string attValue, XmlNode xNode)
        {
            for (int nodeRowCount = 0; nodeRowCount < xNode.ChildNodes.Count; nodeRowCount++)
            {
                if (xNode.ChildNodes[nodeRowCount].Attributes[attName].Value.ToLower().Replace(" ", string.Empty) == attValue.ToLower().Replace(" ", string.Empty))
                {
                    return xNode.ChildNodes[nodeRowCount];
                }
            }
            return null;
        }

        private GetColAttr ParseColAttr(XmlNode nodeParentCols)
        {
            GetColAttr colAttr = new GetColAttr(nodeParentCols);
            return colAttr;
        }

        private void OldUploadData()
        {
            string m_ReqXML = string.Empty;
            string m_FilePath = string.Empty;
            string m_Extn = Path.GetExtension(FileAttachment.PostedFile.FileName);
            string getTrxRowString = string.Empty;
            string m_PhysicalFilePath = string.Empty;
            string[] strBranchNodeNames = null;
            string m_UserBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            XmlDocument xTemp = new XmlDocument();
            //
            Hashtable htPCTrxTypes = null;
            List<string> listParentCols = new List<string>();
            List<ArrayList> listBranchCols = null;
            List<string> listColumns = new List<string>();
            List<string> listColumnsTemp = new List<string>();
            List<string> lstBranchCol1 = new List<string>();
            List<string> lstBranchCol2 = new List<string>();
            List<string> lstBranchCol3 = new List<string>();
            List<string> lstBranchCol4 = new List<string>();
            //

            string strRowXML = null;
            //
            m_PhysicalFilePath = ConfigurationManager.AppSettings["TempFilePath"].ToString();
            XmlDocument xdc = new XmlDocument();
            if (ViewState["returnXML"].ToString() != string.Empty)
            {
                xTemp.LoadXml(ViewState["returnXML"].ToString());
                listParentCols = ParseOUTXML(xTemp, out listBranchCols, out strBranchNodeNames, out htPCTrxTypes);
            }
            if (FileAttachment.HasFile)
            {
                string m_FileName = string.Empty;
                int fileSize = FileAttachment.PostedFile.ContentLength;
                fileSize = fileSize / 1024;
                m_UserBPE = string.Empty;
                m_UserBPE = Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString());
                m_UserBPE = m_UserBPE + "~" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                m_UserBPE = m_UserBPE + m_Extn;
                if (!Directory.Exists(m_PhysicalFilePath))
                {
                    Directory.CreateDirectory(m_PhysicalFilePath);
                }
                FileAttachment.PostedFile.SaveAs(m_PhysicalFilePath + "\\" + m_UserBPE);
                m_FilePath = m_PhysicalFilePath + "\\" + m_UserBPE;
                lblmsg.Visible = false;
                object obj;
                obj = UploadTexts(m_FilePath, true);
                try
                {
                    DataTable dt = new DataTable();
                    string strSheetNameRange = string.Empty;
                    string[] sheetnames = GetExcelSheetNames();
                    string str = string.Empty;
                    if (sheetnames != null)
                    {
                        int count = sheetnames.Length;
                        for (int i = 0; i < count; i++)
                        {
                            strSheetNameRange = sheetnames[i].ToString();
                            if (strSheetNameRange.Substring(strSheetNameRange.Length - 1, 1).ToString() == "$")
                            {
                                dt = GetDataTable(strSheetNameRange);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Columns.Count > 1)
                                    {
                                        if (listColumns.Count == 0)
                                        {
                                            foreach (DataColumn dcVendor in dt.Columns)
                                            {
                                                if (dcVendor.ColumnName.ToString() != "Import")
                                                {
                                                    listColumns.Add(dcVendor.ColumnName.ToString());
                                                }
                                            }
                                        }
                                    }
                                    IEnumerator enumerator1 = listParentCols.GetEnumerator();
                                    while (enumerator1.MoveNext())
                                    {
                                        if (listColumns.Contains(enumerator1.Current.ToString()))
                                        {
                                            listColumnsTemp.Add(enumerator1.Current.ToString());
                                        }
                                    }
                                    //
                                    for (int indexs = 0; indexs < strBranchNodeNames.Length; indexs++)
                                    {
                                        ArrayList srArray = (ArrayList)listBranchCols[indexs];
                                        switch (strBranchNodeNames[indexs].ToString())
                                        {
                                            case "VendorAddress":
                                                {
                                                    IEnumerator enum1 = srArray.GetEnumerator();
                                                    while (enum1.MoveNext())
                                                    {
                                                        if (listColumns.Contains(enum1.Current.ToString()))
                                                        {
                                                            lstBranchCol1.Add(enum1.Current.ToString());
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "VendorPhone":
                                                {
                                                    IEnumerator enum2 = srArray.GetEnumerator();
                                                    while (enum2.MoveNext())
                                                    {
                                                        if (listColumns.Contains(enum2.Current.ToString()))
                                                        {
                                                            lstBranchCol2.Add(enum2.Current.ToString());
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "VendorEmail":
                                                {
                                                    IEnumerator enum3 = srArray.GetEnumerator();
                                                    while (enum3.MoveNext())
                                                    {
                                                        if (listColumns.Contains(enum3.Current.ToString()))
                                                        {
                                                            lstBranchCol3.Add(enum3.Current.ToString());
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "VendorWebsite":
                                                {
                                                    IEnumerator enum4 = srArray.GetEnumerator();
                                                    while (enum4.MoveNext())
                                                    {
                                                        if (listColumns.Contains(enum4.Current.ToString()))
                                                        {
                                                            lstBranchCol4.Add(enum4.Current.ToString());
                                                        }
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                    string strReqBPGID = xTemp.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Add']").Attributes["BPGID"].Value.ToString();
                                    string strColVariable = string.Empty;
                                    string strTemp = string.Empty;
                                    foreach (DataRow drVendor in dt.Rows)
                                    {
                                        StringBuilder srRBuilder1 = new StringBuilder();
                                        StringBuilder srRBuilder2 = new StringBuilder();
                                        StringBuilder srRBuilder3 = new StringBuilder();
                                        StringBuilder srRBuilder4 = new StringBuilder();
                                        StringBuilder srRBuilder5 = new StringBuilder();
                                        StringBuilder srRBuilder6 = new StringBuilder();
                                        string strHTML = string.Empty;
                                        string strReplace = string.Empty;
                                        strRowXML = string.Empty;
                                        string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                                        #region Vendor Parent Row
                                        srRBuilder1.Append("<Vendor><RowList>");
                                        srRBuilder1.Append("<Rows ");
                                        strColVariable = "Vendor~VendorType";
                                        foreach (string Col in listColumnsTemp)
                                        {
                                            strReplace = drVendor[Col].ToString().Replace("\"", "'");
                                            srRBuilder1.Append(" " + Col + "=" + '\"' + strReplace + '\"');
                                        }
                                        if (htPCTrxTypes.ContainsKey(strColVariable))
                                        {
                                            strTemp = "VendorType_TrxID=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[0].ToString() + '\"' + "   " + "VendorType_TrxType=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[1].ToString() + '\"';
                                            srRBuilder1.Append(" " + strTemp);
                                        }
                                        srRBuilder1.Append(" " + "BPAction=\"Add\" />");
                                        srRBuilder1.Append("</RowList></Vendor>");
                                        //
                                        strHTML = srRBuilder1.ToString().Replace("&", "&amp;");
                                        srRBuilder1 = new StringBuilder();
                                        srRBuilder1.Append(strHTML);
                                        #endregion
                                        //
                                        #region Vendor Address
                                        srRBuilder2.Append("<VendorAddress><RowList>");
                                        srRBuilder2.Append("<Rows ");
                                        strColVariable = "VendorAddress~AddressType";
                                        foreach (string Col in lstBranchCol1)
                                        {
                                            strReplace = drVendor[Col].ToString().Replace("\"", "'");
                                            switch (Col)
                                            {
                                                //case "SelectCountry":
                                                //    {
                                                //        string strSCTrxID = xTemp.SelectSingleNode("//SelectCountry/RowList/Rows[@SelectCountry='" + drVendor[Col].ToString() + "']").Attributes["SelectCountry_TrxID"].Value.ToString();
                                                //        string strSCTrxType = xTemp.SelectSingleNode("//SelectCountry/RowList/Rows[@SelectCountry='" + drVendor[Col].ToString() + "']").Attributes["SelectCountry_TrxType"].Value.ToString();
                                                //        srRBuilder2.Append(" " + "SelectCountry_TrxID=" + '\"' + strSCTrxID + '\"' + "   " + "SelectCountry_TrxType=" + '\"' + strSCTrxType + '\"');
                                                //        break;
                                                //    }
                                                //case "SelectRegion":
                                                //    {
                                                //        string strSRTrxID = xTemp.SelectSingleNode("//SelectRegion/RowList/Rows[@SelectRegion='" + drVendor[Col].ToString() + "']").Attributes["SelectRegion_TrxID"].Value.ToString();
                                                //        string strSRTrxType = xTemp.SelectSingleNode("//SelectRegion/RowList/Rows[@SelectRegion='" + drVendor[Col].ToString() + "']").Attributes["SelectRegion_TrxType"].Value.ToString();
                                                //        srRBuilder2.Append(" " + "SelectRegion_TrxID=" + '\"' + strSRTrxID + '\"' + "   " + "SelectRegion_TrxType=" + '\"' + strSRTrxType + '\"');
                                                //        break;
                                                //    }
                                                default:
                                                    {
                                                        srRBuilder2.Append(" " + Col + "=" + '\"' + strReplace + '\"');
                                                        break;
                                                    }
                                            }
                                        }
                                        if (htPCTrxTypes.ContainsKey(strColVariable))
                                        {
                                            strTemp = "AddressType_TrxID=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[0].ToString() + '\"' + "   " + "AddressType_TrxType=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[1].ToString() + '\"';
                                            srRBuilder2.Append(" " + strTemp);
                                        }
                                        srRBuilder2.Append(" " + "BPAction=\"Add\" />");
                                        srRBuilder2.Append("</RowList></VendorAddress>");
                                        //
                                        strHTML = srRBuilder2.ToString().Replace("&", "&amp;");
                                        srRBuilder2 = new StringBuilder();
                                        srRBuilder2.Append(strHTML);
                                        #endregion
                                        //
                                        #region Vendor Phone
                                        srRBuilder3.Append("<VendorPhone><RowList>");
                                        srRBuilder3.Append("<Rows ");
                                        strColVariable = "VendorPhone~PhoneType";
                                        foreach (string Col in lstBranchCol2)
                                        {
                                            strReplace = drVendor[Col].ToString().Replace("\"", "'");
                                            srRBuilder3.Append(" " + Col + "=" + '\"' + strReplace + '\"');
                                        }
                                        if (htPCTrxTypes.ContainsKey(strColVariable))
                                        {
                                            strTemp = "PhoneType_TrxID=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[0].ToString() + '\"' + "   " + "PhoneType_TrxType=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[1].ToString() + '\"';
                                            srRBuilder3.Append(" " + strTemp);
                                        }
                                        srRBuilder3.Append(" " + "BPAction=\"Add\" />");
                                        srRBuilder3.Append("</RowList></VendorPhone>");
                                        //
                                        strHTML = srRBuilder3.ToString().Replace("&", "&amp;");
                                        srRBuilder3 = new StringBuilder();
                                        srRBuilder3.Append(strHTML);
                                        #endregion
                                        //
                                        #region Vendor Email
                                        srRBuilder4.Append("<VendorEmail><RowList>");
                                        srRBuilder4.Append("<Rows ");
                                        strColVariable = "VendorEmail~EmailType";
                                        foreach (string Col in lstBranchCol3)
                                        {
                                            strReplace = drVendor[Col].ToString().Replace("\"", "'");
                                            srRBuilder4.Append(" " + Col + "=" + '\"' + strReplace + '\"');
                                        }
                                        if (htPCTrxTypes.ContainsKey(strColVariable))
                                        {
                                            strTemp = "EmailType_TrxID=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[0].ToString() + '\"' + "   " + "EmailType_TrxType=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[1].ToString() + '\"';
                                            srRBuilder4.Append(" " + strTemp);
                                        }
                                        srRBuilder4.Append(" " + "BPAction=\"Add\" />");
                                        srRBuilder4.Append("</RowList></VendorEmail>");
                                        //
                                        strHTML = srRBuilder4.ToString().Replace("&", "&amp;");
                                        srRBuilder4 = new StringBuilder();
                                        srRBuilder4.Append(strHTML);
                                        #endregion
                                        //
                                        #region Vendor Website
                                        srRBuilder5.Append("<VendorWebsite><RowList>");
                                        srRBuilder5.Append("<Rows ");
                                        strColVariable = "VendorWebsite~WebAddressType";
                                        foreach (string Col in lstBranchCol4)
                                        {
                                            strReplace = drVendor[Col].ToString().Replace("\"", "'");
                                            srRBuilder5.Append(" " + Col.ToString() + "=" + '\"' + strReplace + '\"');
                                        }
                                        if (htPCTrxTypes.ContainsKey(strColVariable))
                                        {
                                            strTemp = "WebAddressType_TrxID=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[0].ToString() + '\"' + "   " + "WebAddressType_TrxType=" + '\"' + htPCTrxTypes[strColVariable].ToString().Split('~')[1].ToString() + '\"';
                                            srRBuilder5.Append(" " + strTemp);
                                        }
                                        srRBuilder5.Append(" " + "BPAction=\"Add\" />");
                                        srRBuilder5.Append("</RowList></VendorWebsite>");
                                        //
                                        strHTML = srRBuilder5.ToString().Replace("&", "&amp;");
                                        srRBuilder5 = new StringBuilder();
                                        srRBuilder5.Append(strHTML);
                                        #endregion
                                        //
                                        #region Vendor Group Item
                                        srRBuilder6.Append("<VendorGroupItem><RowList/></VendorGroupItem>");
                                        #endregion
                                        //}
                                        strRowXML = "<bpinfo><BPGID>" + strReqBPGID + "</BPGID>" + srRBuilder1.ToString() + srRBuilder2.ToString() + srRBuilder3.ToString() + srRBuilder4.ToString() + srRBuilder5.ToString() + srRBuilder6.ToString() + "</bpinfo>";
                                        strReqXml = strReqXml + strRowXML.ToString() + "</Root>";
                                        string strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                        XmlDocument xOutXML = new XmlDocument();
                                        xOutXML.LoadXml(strOutXml);
                                        string strMessage = xOutXML.SelectSingleNode("Root/bpeout/MessageInfo/Status").InnerText.ToString();
                                        //string strErrBPGID=xOutXML.SelectSingleNode(
                                        //if (strMessage.ToUpper() == "SUCCESS")
                                        //{
                                        continue;
                                        //}
                                        //else
                                        //{

                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    #region NLog
                    logger.Fatal(ex);
                    #endregion

                    Response.Write(ex.Message);
                }
            }
        }



        public static List<string> ParseOUTXML(XmlDocument xTemp, out List<ArrayList> listBranchCols, out string[] strBranchNodeNames, out Hashtable htPCTrxTypes)
        {
            List<string> listParentCols = new List<string>();
            htPCTrxTypes = new Hashtable();
            strBranchNodeNames = null;
            listBranchCols = new List<ArrayList>();
            XmlNode xParent = xTemp.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node");
            if (xParent != null)
            {
                string strPTNName = xParent.InnerText.ToString();
                XmlNodeList xnlColumnsList = xTemp.SelectNodes("Root/bpeout/FormControls/" + strPTNName + "/GridHeading/Columns/Col");
                foreach (XmlNode xNodeColumns in xnlColumnsList)
                {
                    if (xNodeColumns.Attributes["IsHidden"].Value.ToString() != "1")
                    {
                        if (xNodeColumns.Attributes["Default"] != null)
                        {
                            if (xNodeColumns.Attributes["ControlType"].Value.ToString() == "DDL")
                            {
                                string strNodeColValue = xNodeColumns.Attributes["Label"].Value.ToString();
                                string strDefaultVal = xNodeColumns.Attributes["Default"].Value.ToString();
                                if (strDefaultVal.ToString() != null)
                                {
                                    string trxID = xTemp.SelectSingleNode("//" + strNodeColValue + "/RowList/Rows[@" + strNodeColValue + "_TrxID='" + strDefaultVal.ToString() + "']").Attributes[strNodeColValue + "_TrxID"].Value.ToString();
                                    string trxType = xTemp.SelectSingleNode("//" + strNodeColValue + "/RowList/Rows[@" + strNodeColValue + "_TrxID='" + strDefaultVal.ToString() + "']").Attributes[strNodeColValue + "_TrxType"].Value.ToString();
                                    if (!htPCTrxTypes.ContainsKey(strNodeColValue))
                                    {
                                        htPCTrxTypes.Add(strPTNName + "~" + strNodeColValue, trxID + "~" + trxType);
                                    }
                                }
                            }
                        }
                        listParentCols.Add(xNodeColumns.Attributes["Label"].Value.ToString());
                    }
                }
            }
            XmlNode xChild = xTemp.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (xChild != null)
            {
                strBranchNodeNames = new string[xChild.ChildNodes.Count];
                int i = 0;
                foreach (XmlNode xChildTreeNodes in xChild.ChildNodes)
                {
                    if (xChildTreeNodes.SelectSingleNode("Node").InnerText.ToString() != null)
                    {
                        strBranchNodeNames[i] = xChildTreeNodes.SelectSingleNode("Node").InnerText.ToString();
                    }
                    i++;
                }
                foreach (string sr in strBranchNodeNames)
                {
                    XmlNodeList xnlChildColumnsList = xTemp.SelectNodes("Root/bpeout/FormControls/" + sr + "/GridHeading/Columns/Col");
                    if (xnlChildColumnsList != null)
                    {
                        ArrayList arrBranchCols = new ArrayList();
                        foreach (XmlNode xBranchCols in xnlChildColumnsList)
                        {
                            if (xBranchCols.Attributes["IsHidden"].Value.ToString() != "1")
                            {
                                if (xBranchCols.Attributes["Default"] != null)
                                {
                                    if (xBranchCols.Attributes["ControlType"].Value.ToString() == "DDL")
                                    {
                                        string strChildNodeVal = xBranchCols.Attributes["Label"].Value.ToString();
                                        string strChildDefaultVal = xBranchCols.Attributes["Default"].Value.ToString();
                                        if (strChildDefaultVal.ToString() != null)
                                        {
                                            string trxID = xTemp.SelectSingleNode("//" + strChildNodeVal + "/RowList/Rows[@" + strChildNodeVal + "_TrxID='" + strChildDefaultVal.ToString() + "']").Attributes[strChildNodeVal + "_TrxID"].Value.ToString();
                                            string trxType = xTemp.SelectSingleNode("//" + strChildNodeVal + "/RowList/Rows[@" + strChildNodeVal + "_TrxID='" + strChildDefaultVal.ToString() + "']").Attributes[strChildNodeVal + "_TrxType"].Value.ToString();
                                            if (!htPCTrxTypes.ContainsKey(strChildNodeVal))
                                            {
                                                htPCTrxTypes.Add(sr + "~" + strChildNodeVal, trxID + "~" + trxType);
                                            }
                                        }
                                    }
                                }
                                arrBranchCols.Add(xBranchCols.Attributes["Label"].Value.ToString());
                            }
                        }
                        listBranchCols.Add(arrBranchCols);
                    }
                }
            }
            return listParentCols;
        }

        public string UploadTexts(string strFileName, Boolean _blnHeaders)
        {
            string extn = Path.GetExtension(strFileName);
            string m_ExcVersion = string.Empty;
            string m_ImexVersion = string.Empty;
            if (extn.ToString().ToUpper() == ".XLSX")
            {
                ExcelCon = "Provider=Microsoft.ACE.OLEDB.12.0;";
                m_ExcVersion = "12.0";
                //m_ImexVersion = "2";
                m_ImexVersion = "1";
            }
            else
            {
                m_ExcVersion = "8.0";
                m_ImexVersion = "1";
            }
            if (_blnHeaders)
            {
                strConnectionString = ExcelCon + "Data Source=" + strFileName + ";Extended Properties=" + Convert.ToChar(34).ToString() + "Excel " + m_ExcVersion + ";HDR=Yes;IMEX=" + m_ImexVersion + "" + Convert.ToChar(34).ToString();
            }
            else
            {
                strConnectionString = ExcelCon + "Data Source=" + strFileName + ";Extended Properties=" + Convert.ToChar(34).ToString() + "Excel " + m_ExcVersion + ";HDR=No;IMEX=" + m_ImexVersion + "" + Convert.ToChar(34).ToString();
            }
            cn = new OleDbConnection();
            cn.ConnectionString = strConnectionString;
            return cn.ConnectionString;
        }

        public String[] GetExcelSheetNames()
        {
            System.Data.DataTable dt = null;
            string strSheetTableName = string.Empty;
            string[] excelSheets = null;
            string[] filteredExcelSheets = null;
            try
            {
                cn.Open();
                // Get the data table containing the schema
                dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                else
                {
                    excelSheets = new string[dt.Rows.Count];
                    int i = 0;
                    // Add the sheet name to the string array.
                    foreach (DataRow row in dt.Rows)
                    {
                        strSheetTableName = row["TABLE_NAME"].ToString();
                        if (strSheetTableName.Contains("$"))
                        {
                            //excelSheets[i] = strSheetTableName.Substring(0, strSheetTableName.Length - 1);
                            strSheetTableName = strSheetTableName.Trim().Replace("'", "");
                            excelSheets[i] = strSheetTableName.ToString().Trim();
                            i++;
                        }
                    }
                    filteredExcelSheets = new string[i];
                    for (int j = 0; j <= i - 1; j++)
                    {
                        filteredExcelSheets[j] = excelSheets[j].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                lblmsg.Text = "";
                lblmsg.Text = ex.Message.ToString() + "." + "Please Select Supported Format Of .XLS.";
                //throw new Exception(ex.Message);
            }
            finally
            {
                // Clean up.
                cn.Close();
            }
            if (filteredExcelSheets != null)
            {
                return filteredExcelSheets;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetDataTable(string strSheetName)
        {
            try
            {
                string strComand;
                if (strSheetName.IndexOf("|") > 0)
                {
                    SheetName = strSheetName.Substring(0, strSheetName.IndexOf("|"));
                    Range = strSheetName.Substring(strSheetName.IndexOf("|") + 1);
                    strComand = "select * from [" + SheetName + "$" + Range + "]";
                }
                else
                {
                    //strComand = "select * from [" + strSheetName + "$]";
                    strComand = "select * from [" + strSheetName + "]";
                }
                string m_SheetNames = string.Empty;
                m_SheetNames = strSheetName.Replace("$", "");
                daAdapter = new OleDbDataAdapter(strComand, cn);
                DataTable dt = new DataTable(m_SheetNames);
                //daAdapter.FillSchema(dt, SchemaType.Source);
                daAdapter.Fill(dt);
                //DataColumn dc = new DataColumn();
                //dc.ColumnName = "Row";

                //dc.DataType = System.Type.GetType("System.String");
                //dt.Columns.Add(dc);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["Row"] = i.ToString();
                //}
                //dt.Columns["Row"].SetOrdinal(0);
                daAdapter.Dispose();
                cn.Close();

                return dt;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw new Exception(ex.Message);
            }
        }

        public string GetXML(string strSheetName, Boolean _blnSchema)
        {
            string str = string.Empty;
            str = strSheetName;
            str = str.Replace("$", "");
            DataSet ds = new DataSet(str);
            ds.Tables.Add(this.GetDataTable(strSheetName));
            if (_blnSchema)
            {
                return ds.GetXmlSchema() + ds.GetXml();
            }
            else
            {
                return ds.GetXml();
            }
        }

        public void CSVReader(Stream filestream, Encoding enc)
        {
            this.objStream = filestream;
            //check the Pass Stream whether it is readable or not
            if (!filestream.CanRead)
            {
                return;
            }
            objReader = (enc != null) ? new StreamReader(filestream, enc) : new StreamReader(filestream);
        }

        public string[] GetCSVLine()
        {
            string data = objReader.ReadLine();
            if (data == null) return null;
            if (data.Length == 0) return new string[0];
            ArrayList result = new ArrayList();
            result.Add(data);
            //ParseCSVData(result, data);
            return (string[])result.ToArray(typeof(string));
        }

        private void ParseCSVData(ArrayList result, string data)
        {
            int position = -1;
            while (position < data.Length)
                result.Add(ParseCSVField(ref data, ref position));
        }

        private string ParseCSVField(ref string data, ref int StartSeperatorPos)
        {
            if (StartSeperatorPos == data.Length - 1)
            {
                StartSeperatorPos++;
                return "";
            }
            int fromPos = StartSeperatorPos + 1;
            if (data[fromPos] == '"')
            {
                int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                int lines = 1;
                while (nextSingleQuote == -1)
                {
                    data = data + "\n" + objReader.ReadLine();
                    nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                    lines++;
                    if (lines > 20)
                        throw new Exception("lines overflow: " + data);
                }
                StartSeperatorPos = nextSingleQuote + 1;
                string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
                tempString = tempString.Replace("'", "''");
                return tempString.Replace("\"\"", "\"");
            }
            int nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                StartSeperatorPos = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                StartSeperatorPos = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }

        private int GetSingleQuote(string data, int SFrom)
        {
            int i = SFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    else
                        return i;
                }
            return -1;
        }

        public void GenerateProcessLinks(string strOutXml)
        {
            try
            {
                XmlDocument xDocLinks = new XmlDocument();
                xDocLinks.LoadXml(strOutXml);

                //Remove Process1 link
                XmlNode xNodeProcess = xDocLinks.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                string XPath = "Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='Process1']";
                XmlNode xnode1 = xDocLinks.SelectSingleNode(XPath);
                xNodeProcess.RemoveChild(xnode1);

                XmlNode xnodeBPC = xDocLinks.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                //If the process controls are exist then include links
                if (xnodeBPC.ChildNodes.Count > 0)
                {
                    string strLinksXML = xDocLinks.OuterXml.ToString();
                    HtmlTableRow trProcessLinks = (HtmlTableRow)Page.FindControl("trProcessLinks");
                    Table tblBPCLinks = commonObjUI.GetBusinessProcessLinksTable(strLinksXML);
                    if (tblBPCLinks.Rows.Count > 0)
                    {
                        if (tblBPCLinks.Rows[0].Cells.Count > 0)
                        {
                            //Get the Form-Level BPC links table and add it to the panel in the form.
                            pnlBPCContainer.Controls.Clear();
                            pnlBPCContainer.Controls.Add(tblBPCLinks);
                        }
                        else
                        {
                            trProcessLinks.Attributes.Add("style", "DISPLAY: none;");
                        }
                    }
                    else
                    {
                        trProcessLinks.Attributes.Add("style", "DISPLAY: none;");
                    }
                }
                else
                {
                    trProcessLinks.Attributes.Add("style", "DISPLAY: none;");
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        private class GetColAttr
        {
            #region Member Variables
            private string m_label = string.Empty;
            private string m_caption = string.Empty;
            private string m_bpControl = string.Empty;
            private int m_smallViewLength;
            private int m_fullViewLength;
            private bool m_isRequired = false;
            private bool m_isUnique = false;
            private bool m_isNumeric = false;
            private bool m_isLink = false;
            private bool m_isHidden = false;
            private bool m_isDisplayOnly = false;
            private bool m_isParentLink = false;
            private bool m_isSortable = false;
            private bool m_isSummed = false;
            private bool m_isSearched = false;
            private int m_sortOrder;
            private string m_controlType = string.Empty;
            private string m_defaultValue = string.Empty;

            private int m_numColAttr = 0;
            #endregion

            #region Properties
            public string Label
            {
                get { return m_label; }
                set { m_label = value; }
            }

            public string Caption
            {
                get { return m_caption; }
                set { m_caption = value; }
            }

            public string BpControl
            {
                get { return m_bpControl; }
                set { m_bpControl = value; }
            }

            public int SmallViewLength
            {
                get { return m_smallViewLength; }
                set { m_smallViewLength = value; }
            }

            public int FullViewLength
            {
                get { return m_fullViewLength; }
                set { m_fullViewLength = value; }
            }

            public bool IsRequired
            {
                get { return m_isRequired; }
                set { m_isRequired = value; }
            }

            public bool IsUnique
            {
                get { return m_isUnique; }
                set { m_isUnique = value; }
            }

            public bool IsNumeric
            {
                get { return m_isNumeric; }
                set { m_isNumeric = value; }
            }

            public bool IsLink
            {
                get { return m_isLink; }
                set { m_isLink = value; }
            }

            public bool IsHidden
            {
                get { return m_isHidden; }
                set { m_isHidden = value; }
            }

            public bool IsDisplayOnly
            {
                get { return m_isDisplayOnly; }
                set { m_isDisplayOnly = value; }
            }

            public bool IsParentLink
            {
                get { return m_isParentLink; }
                set { m_isParentLink = value; }
            }

            public bool IsSortable
            {
                get { return m_isSortable; }
                set { m_isSortable = value; }
            }

            public bool IsSummed
            {
                get { return m_isSummed; }
                set { m_isSummed = value; }
            }

            public bool IsSearched
            {
                get { return m_isSearched; }
                set { m_isSearched = value; }
            }

            public int SortOrder
            {
                get { return m_sortOrder; }
                set { m_sortOrder = value; }
            }

            public string ControlType
            {
                get { return m_controlType; }
                set { m_controlType = value; }
            }

            public string DefaultValue
            {
                get { return m_defaultValue; }
                set { m_defaultValue = value; }
            }

            public int NumColAttr
            {
                get { return m_numColAttr; }
                set { m_numColAttr = value; }
            }
            #endregion

            public GetColAttr(XmlNode nodeCol)
            {
                SetColAttributes(nodeCol);
            }

            private void SetColAttributes(XmlNode nodeCol)
            {
                try
                {
                    m_label = nodeCol.Attributes["Label"].Value.ToString();
                    m_caption = nodeCol.Attributes["Caption"].Value.ToString();
                    m_bpControl = nodeCol.Attributes["BPControl"].Value.ToString();

                    int m_numColAttr = 3;

                    if (nodeCol.Attributes["SmallViewLength"] != null)
                    {
                        m_smallViewLength = Convert.ToInt32(nodeCol.Attributes["SmallViewLength"].Value.ToString());
                        m_numColAttr++;
                    }

                    if (nodeCol.Attributes["FullViewLength"] != null)
                    {
                        m_fullViewLength = Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value.ToString());
                        m_numColAttr++;
                    }

                    if (nodeCol.Attributes["SortOrder"] != null)
                    {
                        m_sortOrder = Convert.ToInt32(nodeCol.Attributes["SortOrder"].Value.ToString());
                        m_numColAttr++;
                    }

                    if (nodeCol.Attributes["ControlType"] != null)
                    {
                        m_controlType = nodeCol.Attributes["ControlType"].Value.ToString();
                        m_numColAttr++;
                    }

                    if (nodeCol.Attributes["ControlType"] != null)
                    {
                        if (nodeCol.Attributes["Default"] != null)
                        {
                            m_defaultValue = nodeCol.Attributes["Default"].Value.ToString();
                            m_numColAttr++;
                        }
                    }

                    if (nodeCol.Attributes["IsRequired"].Value.ToString() != "0")
                        IsRequired = true;
                    else
                        IsRequired = false;

                    if (nodeCol.Attributes["IsUnique"].Value.ToString() != "0")
                        m_isUnique = true;
                    else
                        m_isUnique = false;

                    if (nodeCol.Attributes["IsNumeric"].Value.ToString() != "0")
                        m_isNumeric = true;
                    else
                        m_isNumeric = false;

                    if (nodeCol.Attributes["IsLink"].Value.ToString() != "0")
                        m_isLink = true;
                    else
                        m_isLink = false;

                    if (nodeCol.Attributes["IsHidden"].Value.ToString() != "0")
                        m_isHidden = true;
                    else
                        m_isHidden = false;

                    if (nodeCol.Attributes["IsDisplayOnly"].Value.ToString() != "0")
                        m_isDisplayOnly = true;
                    else
                        m_isDisplayOnly = false;

                    if (nodeCol.Attributes["IsParentLink"].Value.ToString() != "0")
                        m_isParentLink = true;
                    else
                        m_isParentLink = false;

                    if (nodeCol.Attributes["IsSortable"].Value.ToString() != "0")
                        m_isSortable = true;
                    else
                        m_isSortable = false;

                    if (nodeCol.Attributes["IsSummed"].Value.ToString() != "0")
                        m_isSummed = true;
                    else
                        m_isSummed = false;

                    if (nodeCol.Attributes["IsSearched"].Value.ToString() != "0")
                        m_isSearched = true;
                    else
                        m_isSearched = false;

                    m_numColAttr = m_numColAttr + 10;
                }
                catch (Exception ex)
                {
                    #region NLog
                    NLog.Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Fatal(ex);
                    #endregion
                }
            }
        }
    }

}
