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
using NLog;


namespace LAjitDev.Common
{
    public partial class UploadAmex : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
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
                // string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
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

            chkUploadOptionC.Attributes.Add("onChange", "javascript:checked()");

            string BPInfo = string.Empty;
            string masterfile = string.Empty;
            string row = string.Empty;

            Ajax.Utility.RegisterTypeForAjax(typeof(LAjitDev.Common.UploadText));
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
                        //BindPage(BPInfo, updtPnlContent);
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
                    //BindPage(bpgid, updtPnlContent);
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
            preloadAttachmentImages();

            //Load Process Links
            GenerateProcessLinks(ViewState["returnXML"].ToString());
        }



        public void BindPage(string SessionBPInfo, Control CurrentPage, XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + SessionBPInfo + " Currrent Page as : " + CurrentPage);
            #endregion

            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            XmlDocument xDoc = new XmlDocument();
            if (SessionBPInfo != string.Empty)
            {
                //Session["BPINFO"] = SessionBPInfo;
                //Keeping Parent BPInfo backup in the hidden variable
                parentBPInfo.Value = SessionBPInfo;
            }
            //generate Request XMl 
            //string strReqXml = objBO.GenActionRequestXML("PAGELOAD", parentBPInfo.Value, "", "", "", Session["BPE"].ToString(), false, "", "", null);
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
                    if (ViewState["returnXML"] != null)
                    {
                        PreloadAttachementGV(strOutXml);
                    }
                }
            }
        }

        public void preloadAttachmentImages()
        {
            imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/Upload_but.png";
        }

        public void PreloadAttachementGV(string returnXML)
        {
            try
            {
                if (returnXML != null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(returnXML);

                    XmlNodeList xControls = xDoc.SelectNodes("//GridHeading/Columns/Col");
                    pnlEntryForm.Height = Unit.Pixel(460);
                    foreach (XmlNode xnodes in xControls)
                    {
                        string controlNames = xnodes.Attributes["ControlType"].Value;
                        if (controlNames == "Check")
                        {
                            pnlUpload.Visible = true;

                            string IsHidden = xnodes.Attributes["IsHidden"].Value;
                            string IsDisplayOnly = xnodes.Attributes["IsDisplayOnly"].Value;

                            switch (xnodes.Attributes["Label"].Value)
                            {
                                case "UploadOptionA":
                                    {
                                        lblUploadOptionA.Text = xnodes.Attributes["Caption"].Value;
                                        if (IsDisplayOnly == "0" && IsHidden == "0")
                                        {
                                            lblUploadOptionA.Visible = true;
                                            chkUploadOptionA.Visible = true;
                                        }
                                        else
                                        {
                                            lblUploadOptionA.Visible = false;
                                            chkUploadOptionA.Visible = false;
                                        }
                                        break;
                                    }
                                case "UploadOptionB":
                                    {
                                        lblUploadOptionB.Text = xnodes.Attributes["Caption"].Value;
                                        if (IsDisplayOnly == "0" && IsHidden == "0")
                                        {
                                            lblUploadOptionB.Visible = true;
                                            chkUploadOptionB.Visible = true;
                                        }
                                        else
                                        {
                                            lblUploadOptionB.Visible = false;
                                            chkUploadOptionB.Visible = false;
                                        }
                                        break;
                                    }
                                case "UploadOptionC":
                                    {
                                        lblUploadOptionC.Text = xnodes.Attributes["Caption"].Value;
                                        if (IsDisplayOnly == "0" && IsHidden == "0")
                                        {
                                            lblUploadOptionC.Visible = true;
                                            chkUploadOptionC.Visible = true;
                                        }
                                        else
                                        {
                                            lblUploadOptionC.Visible = false;
                                            chkUploadOptionC.Visible = false;
                                        }
                                        break;
                                    }
                                case "UploadOptionD":
                                    {
                                        lblUploadOptionD.Text = xnodes.Attributes["Caption"].Value;
                                        if (IsDisplayOnly == "0" && IsHidden == "0")
                                        {
                                            lblUploadOptionD.Visible = true;
                                            chkUploadOptionD.Visible = true;
                                        }
                                        else
                                        {
                                            lblUploadOptionD.Visible = false;
                                            chkUploadOptionD.Visible = false;
                                        }
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            if (controlNames == "DDL")
                            {
                                pnlUpload.Visible = true;


                                string IsHidden = xnodes.Attributes["IsHidden"].Value;
                                string IsDisplayOnly = xnodes.Attributes["IsDisplayOnly"].Value;
                                string IsRequired = xnodes.Attributes["IsRequired"].Value;

                                switch (xnodes.Attributes["Label"].Value)
                                {
                                    case "SecureCategory":
                                        //lblSecureCategory.Text = xnodes.Attributes["Caption"].Value;

                                        if (IsDisplayOnly == "0" && IsHidden == "0" && IsRequired == "0")
                                        {
                                            //lblSecureCategory.Visible = true;
                                            ddlSecureCategory.Visible = true;
                                            reqSecureCategory.Visible = false;
                                            ArrayList alCols = new ArrayList();
                                            XmlNode nodeColumns = xDoc.SelectSingleNode("//GridHeading/Columns");
                                            if (nodeColumns != null)
                                            {
                                                foreach (XmlNode colnode in nodeColumns.ChildNodes)
                                                {
                                                    //Collecting all the columns which have IsRequired property.                    
                                                    if (colnode.Attributes["ControlType"] != null)
                                                    {
                                                        if (colnode.Attributes["ControlType"].Value == "DDL")
                                                        {
                                                            alCols.Add(colnode.Attributes["Label"].Value);
                                                        }
                                                    }
                                                }
                                            }
                                            commonObjUI.FillDropDownData(returnXML, alCols, pnlEntryForm);
                                        }
                                        else
                                        {
                                            //lblSecureCategory.Visible = false;
                                            ddlSecureCategory.Visible = false;
                                            reqSecureCategory.Visible = true;
                                            reqSecureCategory.Validate();
                                        }
                                        break;
                                }
                            }
                        }
                        ViewState["returnXML"] = returnXML;
                        if (ViewState["returnXML"] != null)
                        {
                            DisplayFoundResults(returnXML);
                        }
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
        }

        public void DisplayFoundResults(string returnXML)
        {
            #region NLog
            logger.Info("Binding Parent Grid Data based on the returend XML");
            #endregion

            try
            {
                string columnName = string.Empty;
                string treeNode = string.Empty;
                Label lbltext = new Label();
                string Controlname = string.Empty;
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(returnXML);

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                {
                    treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                }
                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList") != null)
                {
                    lblDescriptionText.Text = xDoc.SelectSingleNode("//" + treeNode + "/RowList/Rows").Attributes["Description"].Value;
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                string s = ex.Message.ToString();
            }
        }

        private string GenerateGVRequestXML(string BPGID)
        {
            #region NLog
            logger.Info("This method is used to generate the GV Request XMl based on the BPGID as : " + BPGID);
            #endregion

            try
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + BPGID;
                //nodeRoot.InnerXml = Session["BPE"].ToString() + BPGID;
                ViewState["BPEbpInfo"] = nodeRoot.OuterXml;
                return nodeRoot.OuterXml;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            string m_ReqXML = string.Empty;
            string m_FilePath = string.Empty;
            string m_Extn = Path.GetExtension(FileAttachment.PostedFile.FileName);
            string getTrxRowString = string.Empty;
            string m_PhysicalFilePath = string.Empty;

            if (chkUploadOptionC.Checked)
            {
                m_PhysicalFilePath = ConfigurationManager.AppSettings["AttachmentsPath"].ToString() + "/" + Session["CompanyEntityID"].ToString();
            }
            else
            {
                m_PhysicalFilePath = ConfigurationManager.AppSettings["TempFilePath"].ToString();
            }
            //string m_UserBPE = Session["BPE"].ToString();
            string m_UserBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string m_Options = string.Empty;
            XmlDocument xdc = new XmlDocument();
            XmlNode xnode = xdc.CreateElement("OPTIONS");
            if (ViewState["returnXML"].ToString() != string.Empty)
            {
                XmlDocument xTemp = new XmlDocument();
                xTemp.LoadXml(ViewState["returnXML"].ToString());
                bool _formControls = false;
                if (xTemp.SelectSingleNode("Root/bpeout/FormControls").ChildNodes.Count > 0)
                {
                    _formControls = true;
                }
                if ((bool)_formControls)
                {
                    foreach (Control c in pnlUpload.Controls)
                    {
                        foreach (Control c1 in c.Controls)
                        {
                            foreach (Control c2 in c1.Controls)
                            {
                                if (c2 is LAjitControls.LAjitCheckBox)
                                {
                                    if (c2 is LAjitControls.LAjitCheckBox)
                                    {
                                        if (((LAjitControls.LAjitCheckBox)c2).Checked)
                                        {
                                            XmlElement xeles = xdc.CreateElement(c2.ClientID.Replace("ctl00_cphPageContents_chk", ""));
                                            XmlAttribute xattr = xdc.CreateAttribute("IsChecked");
                                            xattr.Value = "1";
                                            xeles.Attributes.Append(xattr);
                                            xnode.AppendChild(xeles);
                                            if (xeles.ToString() == "UploadOptionC")
                                            {
                                                hidChecked.Value = "Checked";
                                            }
                                        }
                                        else
                                        {
                                            XmlElement xeles = xdc.CreateElement(c2.ClientID.Replace("ctl00_cphPageContents_chk", ""));
                                            XmlAttribute xattr = xdc.CreateAttribute("IsChecked");
                                            xattr.Value = "0";
                                            xeles.Attributes.Append(xattr);
                                            xnode.AppendChild(xeles);
                                        }
                                    }
                                    xdc.AppendChild(xnode);
                                }
                                else
                                {
                                    if (c2 is LAjitControls.LAjitDropDownList)
                                    {
                                        XmlDocument xdocs = new XmlDocument();
                                        xdocs.LoadXml(ViewState["returnXML"].ToString());
                                        getTrxRowString = commonObjUI.GetNewRow(pnlEntryForm, xdocs, imgbtnSubmit.AlternateText);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            m_Options = xdc.OuterXml.ToString();
            if (FileAttachment.HasFile)
            {
                string m_FileName = string.Empty;
                int fileSize = FileAttachment.PostedFile.ContentLength;
                fileSize = fileSize / 1024;
                XmlDocument xtxtDoc = new XmlDocument();
                switch (m_Extn.ToString().Trim().ToUpper())
                {
                    #region 'XLS' && 'XLSX'
                    case ".XLSX":
                    case ".XLS":
                        {
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
                            obj = UploadTexts(m_FilePath, false);
                            try
                            {
                                DataSet dsFile = new DataSet();
                                DataTable dt = new DataTable();
                                string strSheetNameRange = string.Empty;
                                string[] sheetnames = GetExcelSheetNames();
                                string str = string.Empty;
                                if (sheetnames != null)
                                {
                                    int count = sheetnames.Length;
                                    XmlDocument xdocNew = new XmlDocument();
                                    StringBuilder sb = new StringBuilder();
                                    for (int i = 0; i < count; i++)
                                    {
                                        strSheetNameRange = sheetnames[i].ToString();
                                        if (strSheetNameRange.Substring(strSheetNameRange.Length - 1, 1).ToString() == "$")
                                        {
                                            dt = GetDataTable(strSheetNameRange);
                                            string m_XMLFormat = string.Empty;
                                            m_XMLFormat = GetXML(strSheetNameRange, false);
                                            sb.Append(m_XMLFormat + "\n");
                                        }
                                    }
                                    string xmlFormat = string.Empty;
                                    string m_TotalRowXmlString = string.Empty;
                                    if (Convert.ToString(Session["Row"]) != string.Empty || Session["Row"] != null)
                                    {
                                        string m_RowString = string.Empty;
                                        m_RowString = Session["Row"].ToString();
                                        xmlFormat = "<ExcelData>" + sb.ToString() + "</ExcelData>";
                                        xdocNew.LoadXml(xmlFormat);
                                        m_TotalRowXmlString = m_RowString + xdocNew.OuterXml.ToString();
                                    }
                                    else
                                    {
                                        xmlFormat = "<ExcelData>" + sb.ToString() + "</ExcelData>";
                                        xdocNew.LoadXml(xmlFormat);
                                        m_TotalRowXmlString = xdocNew.OuterXml.ToString();
                                    }
                                    XmlDocument xbpdoc = new XmlDocument();
                                    xbpdoc.LoadXml(ViewState["returnXML"].ToString());
                                    if (xbpdoc.OuterXml.ToString().Contains("Error"))
                                    {
                                        lblmsg.Visible = true;
                                        lblmsg.Text = "";
                                        lblmsg.Text = xbpdoc.SelectSingleNode("//Error/Label").InnerText;
                                        return;
                                    }
                                    else
                                    {
                                        string bpgidno = xbpdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["BPGID"].Value;
                                        if (chkUploadOptionC.Checked)
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + m_TotalRowXmlString + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                        }
                                        else
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + m_TotalRowXmlString + getTrxRowString + "</bpinfo>";
                                        }
                                        string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + m_ReqXML + "</Root>";
                                        string strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                        //
                                        XmlDocument m_outXml = new XmlDocument();
                                        m_outXml.LoadXml(strOutXml);

                                        lblmsg.Visible = true;
                                        lblmsg.Text = "";
                                        lblmsg.Text = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;

                                        if (!chkUploadOptionC.Checked)
                                        {
                                            if (File.Exists(m_FilePath))
                                            {
                                                File.Delete(m_FilePath);
                                            }
                                        }
                                        //FileDownload(strReqXml);
                                    }
                                }
                                else
                                {
                                    lblmsg.Visible = true;
                                    lblmsg.Text = "";
                                    lblmsg.Text = "External Table Is not in Supported Format.Please Select Supported .XLS Format.";
                                }
                            }
                            catch (Exception ex)
                            {
                                #region NLog
                                logger.Fatal(ex);
                                #endregion

                                Response.Write(ex.Message);
                            }
                            break;
                        }
                    #endregion
                    #region 'TXT'
                    case ".TXT":
                        {
                            try
                            {
                                m_UserBPE = string.Empty;
                                m_UserBPE = Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString());
                                m_UserBPE = m_UserBPE + "~" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                                m_UserBPE = m_UserBPE + m_Extn;

                                if (!Directory.Exists(m_PhysicalFilePath))
                                {
                                    Directory.CreateDirectory(m_PhysicalFilePath);
                                }
                                if (chkUploadOptionC.Checked)
                                {
                                    FileAttachment.PostedFile.SaveAs(m_PhysicalFilePath + "\\" + m_UserBPE);
                                    m_FilePath = m_PhysicalFilePath + "\\" + m_UserBPE;
                                }
                                int BUFFER_SIZE = Convert.ToInt32(FileAttachment.PostedFile.ContentLength);
                                int nBytesRead = 0;
                                Byte[] Buffer = new Byte[BUFFER_SIZE];
                                StringBuilder strUploadedContent = new StringBuilder();
                                Stream theStream = FileAttachment.PostedFile.InputStream;
                                nBytesRead = theStream.Read(Buffer, 0, BUFFER_SIZE);
                                while (0 != nBytesRead)
                                {
                                    strUploadedContent.Append(Encoding.ASCII.GetString(Buffer, 0, nBytesRead));
                                    nBytesRead = theStream.Read(Buffer, 0, BUFFER_SIZE);
                                }
                                string txtOutput = "";
                                txtOutput = Server.HtmlEncode(strUploadedContent.ToString());
                                XmlNode xtxtNode = xtxtDoc.CreateElement("TextData");
                                int counter = 0;
                                char[] ch = { '\n' };
                                foreach (string sr in txtOutput.Split(ch))
                                {
                                    XmlNode xtxtElementRow = xtxtDoc.CreateElement("Row");
                                    XmlElement xtxtElementLine = xtxtDoc.CreateElement("Line");
                                    xtxtElementLine.InnerText = sr;
                                    xtxtElementRow.AppendChild(xtxtElementLine);
                                    xtxtNode.AppendChild(xtxtElementRow);
                                }
                                txtOutput = string.Empty;
                                txtOutput = xtxtNode.OuterXml.ToString();
                                if (txtOutput.Contains("&#x0;"))
                                {
                                    txtOutput = txtOutput.Replace("&#x0;", " ");
                                }
                                xtxtDoc.LoadXml(txtOutput);
                                XmlDocument xbpdoc = new XmlDocument();
                                xbpdoc.LoadXml(ViewState["returnXML"].ToString());
                                if (xbpdoc.OuterXml.ToString().Contains("Error"))
                                {
                                    lblmsg.Visible = true;
                                    lblmsg.Text = "";
                                    XmlNode nodeMsgStatus = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                    XmlNode nodeErrorLabel = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                    XmlNode nodeOtherInfo = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                    XmlNode nodeMsg = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                    if (nodeMsgStatus != null)
                                    {
                                        lblmsg.Text = nodeMsgStatus.InnerText;
                                    }
                                    if (nodeErrorLabel != null)
                                    {
                                        lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                    }
                                    if (nodeOtherInfo != null)
                                    {
                                        lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                    }
                                    return;
                                }
                                else
                                {
                                    if (xbpdoc.SelectSingleNode("Root/bpeout/FormControls").ChildNodes.Count > 0)
                                    {
                                        string bpgidno = xbpdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["BPGID"].Value;
                                        if (Session["Row"] != null)
                                        {
                                            if (chkUploadOptionC.Checked)
                                            {
                                                m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xtxtDoc.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                            }
                                            else
                                            {
                                                m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xtxtDoc.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                            }
                                        }
                                        else
                                        {
                                            if (chkUploadOptionC.Checked)
                                            {
                                                m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xtxtDoc.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                            }
                                            else
                                            {
                                                m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xtxtDoc.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                            }
                                        }
                                    }
                                    string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + m_ReqXML + "</Root>";
                                    string strOutXml = objBO.GetDataForCPGV1(strReqXml);

                                    XmlDocument m_outXml = new XmlDocument();
                                    m_outXml.LoadXml(strOutXml);

                                    lblmsg.Visible = true;
                                    XmlNode nodeMsgStatus = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                    XmlNode nodeErrorLabel = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                    XmlNode nodeOtherInfo = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                    XmlNode nodeMsg = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                    if (nodeMsgStatus != null)
                                    {
                                        lblmsg.Text = nodeMsgStatus.InnerText;
                                    }
                                    if (nodeErrorLabel != null)
                                    {
                                        lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                    }
                                    if (nodeOtherInfo != null)
                                    {
                                        lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                    }
                                    //FileDownload(strReqXml);
                                }
                            }
                            catch (XmlException xex)
                            {
                                #region NLog
                                logger.Fatal(xex);
                                #endregion

                                lblmsg.Text = "";
                                lblmsg.Visible = true;
                                lblmsg.Text = xex.Message.ToString();
                            }
                            break;
                        }
                    #endregion
                    #region 'CSV'
                    case ".CSV":
                        {
                            CSVReader(FileAttachment.PostedFile.InputStream, null);
                            DataTable dt = new DataTable();
                            DataTable dtNew = new DataTable();
                            dt.Columns.Add("Column");
                            dtNew.Columns.Add("Columns");
                            string[] data;
                            while ((data = GetCSVLine()) != null)
                            {
                                dt.Rows.Add(data);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr.ItemArray[0].ToString() != string.Empty)
                                {
                                    dtNew.Rows.Add(dr.ItemArray[0].ToString());
                                }
                            }
                            XmlDocument xdocs = new XmlDocument();
                            XmlNode xtxtNode = xdocs.CreateElement("TextData");
                            int counter = 0;
                            foreach (DataRow dr in dtNew.Rows)
                            {
                                XmlNode xtxtElementRow = xdocs.CreateElement("Row");
                                XmlElement xtxtElementLine = xdocs.CreateElement("Line");
                                xtxtElementLine.InnerText = dr[0].ToString();
                                xtxtElementRow.AppendChild(xtxtElementLine);
                                xtxtNode.AppendChild(xtxtElementRow);
                            }
                            xdocs.AppendChild(xtxtNode);
                            XmlDocument xbpdoc = new XmlDocument();
                            xbpdoc.LoadXml(ViewState["returnXML"].ToString());

                            if (xbpdoc.OuterXml.ToString().Contains("Error"))
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "";
                                //lblmsg.Text = xbpdoc.SelectSingleNode("//Error/Label").InnerText;
                                XmlNode nodeMsgStatus = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                XmlNode nodeErrorLabel = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                XmlNode nodeOtherInfo = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                XmlNode nodeMsg = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                if (nodeMsgStatus != null)
                                {
                                    lblmsg.Text = nodeMsgStatus.InnerText;
                                }
                                if (nodeErrorLabel != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                }
                                if (nodeOtherInfo != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                }
                                return;
                            }
                            else
                            {
                                if (xbpdoc.SelectSingleNode("Root/bpeout/FormControls").ChildNodes.Count > 0)
                                {
                                    string bpgidno = xbpdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["BPGID"].Value;
                                    if (Session["Row"] != null)
                                    {
                                        if (chkUploadOptionC.Checked)
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xdocs.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                        }
                                        else
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xdocs.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                        }
                                    }
                                    else
                                    {
                                        if (chkUploadOptionC.Checked)
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xdocs.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                        }
                                        else
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xdocs.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                        }
                                    }
                                }
                                string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + m_ReqXML + "</Root>";
                                string strOutXml = objBO.GetDataForCPGV1(strReqXml);

                                XmlDocument m_outXml = new XmlDocument();
                                m_outXml.LoadXml(strOutXml);

                                lblmsg.Visible = true;
                                XmlNode nodeMsgStatus = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                XmlNode nodeErrorLabel = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                XmlNode nodeOtherInfo = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                XmlNode nodeMsg = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                if (nodeMsgStatus != null)
                                {
                                    lblmsg.Text = nodeMsgStatus.InnerText;
                                }
                                if (nodeErrorLabel != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                }
                                if (nodeOtherInfo != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                }
                            }
                            //FileDownload(strReqXml);
                            break;
                        }
                    #endregion
                    #region 'DAT'
                    case ".DAT":
                        {
                            m_UserBPE = string.Empty;
                            m_UserBPE = Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString());
                            m_UserBPE = m_UserBPE + "~" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                            m_UserBPE = m_UserBPE + m_Extn;

                            if (!Directory.Exists(m_PhysicalFilePath))
                            {
                                Directory.CreateDirectory(m_PhysicalFilePath);
                            }
                            if (chkUploadOptionC.Checked)
                            {
                                FileAttachment.PostedFile.SaveAs(m_PhysicalFilePath + "\\" + m_UserBPE);
                                m_FilePath = m_PhysicalFilePath + "\\" + m_UserBPE;
                            }
                            int BUFFER_SIZE = Convert.ToInt32(FileAttachment.PostedFile.ContentLength);
                            int nBytesRead = 0;
                            Byte[] Buffer = new Byte[BUFFER_SIZE];
                            StringBuilder strUploadedContent = new StringBuilder();
                            Stream theStream = FileAttachment.PostedFile.InputStream;
                            nBytesRead = theStream.Read(Buffer, 0, BUFFER_SIZE);
                            while (0 != nBytesRead)
                            {
                                strUploadedContent.Append(Encoding.ASCII.GetString(Buffer, 0, nBytesRead));
                                nBytesRead = theStream.Read(Buffer, 0, BUFFER_SIZE);
                            }
                            string txtOutput = "";
                            txtOutput = Server.HtmlEncode(strUploadedContent.ToString());
                            if (txtOutput.Contains("&quot;"))
                            {
                                txtOutput = txtOutput.Replace("&quot;", "");
                            }
                            char[] ch = { '\n' };
                            XmlDocument xdocs = new XmlDocument();
                            XmlNode xtxtNode = xdocs.CreateElement("TextData");

                            foreach (string sr in txtOutput.Split(ch))
                            {
                                if (sr != string.Empty)
                                {
                                    XmlNode xtxtElementRow = xdocs.CreateElement("Row");
                                    XmlElement xtxtElementLine = xdocs.CreateElement("Line");
                                    xtxtElementLine.InnerText = sr;
                                    xtxtElementRow.AppendChild(xtxtElementLine);
                                    xtxtNode.AppendChild(xtxtElementRow);
                                }
                            }
                            xdocs.AppendChild(xtxtNode);
                            XmlDocument xbpdoc = new XmlDocument();
                            xbpdoc.LoadXml(ViewState["returnXML"].ToString());
                            if (xbpdoc.OuterXml.ToString().Contains("Error"))
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "";
                                XmlNode nodeMsgStatus = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                XmlNode nodeErrorLabel = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                XmlNode nodeOtherInfo = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                XmlNode nodeMsg = xbpdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                if (nodeMsgStatus != null)
                                {
                                    lblmsg.Text = nodeMsgStatus.InnerText;
                                }
                                if (nodeErrorLabel != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                }
                                if (nodeOtherInfo != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                }
                                return;
                            }
                            else
                            {
                                if (xbpdoc.SelectSingleNode("Root/bpeout/FormControls").ChildNodes.Count > 0)
                                {
                                    string bpgidno = xbpdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["BPGID"].Value;
                                    if (Session["Row"] != null)
                                    {
                                        if (chkUploadOptionC.Checked)
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xdocs.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                        }
                                        else
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + xdocs.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                        }
                                    }
                                    else
                                    {
                                        if (chkUploadOptionC.Checked)
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xdocs.InnerXml.ToString() + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                        }
                                        else
                                        {
                                            m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + xdocs.InnerXml.ToString() + getTrxRowString + "</bpinfo>";
                                        }
                                    }
                                }
                                string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + m_ReqXML + "</Root>";
                                string strOutXml = objBO.GetDataForCPGV1(strReqXml);

                                XmlDocument m_outXml = new XmlDocument();
                                m_outXml.LoadXml(strOutXml);

                                lblmsg.Visible = true;
                                XmlNode nodeMsgStatus = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                                XmlNode nodeErrorLabel = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label");
                                XmlNode nodeOtherInfo = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/OtherInfo");
                                XmlNode nodeMsg = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                                if (nodeMsgStatus != null)
                                {
                                    lblmsg.Text = nodeMsgStatus.InnerText;
                                }
                                if (nodeErrorLabel != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "-" + nodeErrorLabel.InnerText;
                                }
                                if (nodeOtherInfo != null)
                                {
                                    lblmsg.Text = lblmsg.Text + "." + nodeOtherInfo.InnerText;
                                }
                            }
                            //FileDownload(strReqXml);
                            break;
                        }
                    #endregion
                    #region '.XML'
                    case ".XML":
                        {
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
                            //
                            string totString = string.Empty;
                            XmlTextReader reader = new XmlTextReader(m_FilePath);
                            while (reader.Read())
                            {
                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        {
                                            totString = reader.Name;
                                            break;
                                        }
                                }
                                if (!string.IsNullOrEmpty(totString))
                                {
                                    break;
                                }
                            }
                            XmlDocument xDocXML = new XmlDocument();
                            xDocXML.Load(m_FilePath);
                            string readString = "<XML_DATA>" + xDocXML.SelectSingleNode("//" + totString).OuterXml.ToString() + "</XML_DATA>";
                            XmlDocument xbpdoc = new XmlDocument();
                            xbpdoc.LoadXml(ViewState["returnXML"].ToString());
                            if (xbpdoc.OuterXml.ToString().Contains("Error"))
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "";
                                lblmsg.Text = xbpdoc.SelectSingleNode("//Error/Label").InnerText;
                                return;
                            }
                            else
                            {
                                string bpgidno = xbpdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["BPGID"].Value;
                                if (Session["Row"] != null)
                                {
                                    if (chkUploadOptionC.Checked)
                                    {
                                        m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + readString + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                    }
                                    else
                                    {
                                        m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + Session["Row"].ToString() + readString + getTrxRowString + "</bpinfo>";
                                    }
                                }
                                else
                                {
                                    if (chkUploadOptionC.Checked)
                                    {
                                        m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + readString + "<AttachedFile>" + m_UserBPE + "</AttachedFile>" + "<AttachedFileSize>" + fileSize + "</AttachedFileSize>" + getTrxRowString + "</bpinfo>";
                                    }
                                    else
                                    {
                                        m_ReqXML = "<bpinfo><BPGID>" + bpgidno + "</BPGID>" + readString + getTrxRowString + "</bpinfo>";
                                    }
                                }
                                //
                                string strReqXml = "<Root>" + Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml) + m_ReqXML + "</Root>";
                                string strOutXml = objBO.GetDataForCPGV1(strReqXml);
                                //
                                XmlDocument m_outXml = new XmlDocument();
                                m_outXml.LoadXml(strOutXml);

                                lblmsg.Visible = true;
                                lblmsg.Text = "";
                                lblmsg.Text = m_outXml.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;

                                //if (!chkUploadOptionC.Checked)
                                //{
                                //    if (File.Exists(m_FilePath))
                                //    {
                                //        File.Delete(m_FilePath);
                                //    }
                                //}
                                //FileDownload(strReqXml);
                            }
                            break;
                        }
                    #endregion
                }
            }
            #region 'Error'
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "";
                lblmsg.Text = "Please Select Only '.XLS' or '.txt' or '.CSV' File Formats Only";
            }
            #endregion
        }

        public void FileDownload(string m_ReqXML)
        {
            Response.ContentType = "application/XML";
            Response.AddHeader("content-disposition", "attachment; filename=InputFile.xml");
            Response.ClearContent();
            Response.Write(m_ReqXML);
            Response.End();
        }

        public string UploadTexts(string strFileName, Boolean _blnHeaders)
        {
            #region NLog
            logger.Info("This method is used to upload texts with file name as : " + strFileName);
            #endregion

            string extn = Path.GetExtension(strFileName);
            string m_ExcVersion = string.Empty;
            string m_ImexVersion = string.Empty;
            if (extn.ToString().ToUpper() == ".XLSX")
            {
                ExcelCon = "Provider=Microsoft.ACE.OLEDB.12.0;";
                m_ExcVersion = "12.0";
                m_ImexVersion = "2";
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
                DataColumn dc = new DataColumn();
                dc.ColumnName = "Row";

                dc.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(dc);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Row"] = i.ToString();
                }
                dt.Columns["Row"].SetOrdinal(0);
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

        #region XML Functionality
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
        #endregion

        //protected void imgbtnCancel_Click(object sender, ImageClickEventArgs e)
        //{
        //    //Calling the Cancel method by passing the content page Update Panel as parameter
        //    if (Page.MasterPageFile.Contains("PopUp.Master"))
        //    {
        //        //commonObjUI.CancelPagePopUpEntries(updtPnlContent);
        //    }
        //    else
        //    {
        //        //commonObjUI.CancelEntries(updtPnlContent);
        //    }
        //}

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
        }

        public void CSVReader(Stream filestream, Encoding enc)
        {
            #region NLog
            logger.Info("This method reads the CSV file from file stream : " + filestream);
            #endregion

            this.objStream = filestream;
            //check the Pass Stream whether it is readable or not
            if (!filestream.CanRead)
            {
                return;
            }
            objReader = (enc != null) ? new StreamReader(filestream, enc) : new StreamReader(filestream);
        }

        //parse the Line
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


        /// <summary>
        /// Page level process links
        /// </summary>
        /// <param name="strOutXml"></param>
        public void GenerateProcessLinks(string strOutXml)
        {
            #region NLog
            logger.Info("Generating page level process links");
            #endregion

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
    }
}
