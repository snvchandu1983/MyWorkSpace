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
using LAjitDev.Classes;
using LAjitDev.UserControls;
using System.IO;
using NLog;


namespace LAjitDev.Common
{
    public partial class AuthorHelp : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private CommonUI objCommonUI = new CommonUI();
        protected void Page_Load(object sender, EventArgs e)
        {
            objCommonUI.ButtonsUserControl = (UserControls.ButtonsUserControl)BtnsUC;
            objCommonUI.GridViewUserControl = (LAjitDev.GridViewControl)GVUC;
            objCommonUI.GridViewUserControl.ObjCommonUI = objCommonUI;
            objCommonUI.ButtonsUserControl.ObjCommonUI = objCommonUI;
            objCommonUI.UpdatePanelContent = updtPnlContent;
            objCommonUI.IsHelpAuthPage = true;
            if (!Page.IsPostBack)
            {
                imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/Waiting-Approval.png";
                BindPage();
                ddlHelpFileType.Attributes["onchange"] = "OnFileTypeChange(this,'" + txtHelpFile.ClientID + "','" + txtFileAttachment.ClientID + "');";
            }

            string jQueryEvents = "function JQueryEvents(){"
                + JQueryUI.RegisterJQueryCollapsableExtender(Page, pnlCPGV1Title.ClientID, pnlGVContent.ClientID, "imgCPGV1Expand")
                + "}";

            if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                objCommonUI.InjectSessionExpireScript(this);
            }
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), jQueryEvents, true);

            PreloadImages();
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Registering types for ajax methods.
                RegisterTypesForAjax();
            }

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
            Page.Theme = (Convert.ToString(HttpContext.Current.Session["MyTheme"]));
            base.OnPreInit(e);
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

        }

        private void BindPage()
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo.");
            #endregion

            txtHelpFile.Attributes.Add("style", "DISPLAY: none;");

            XmlDocument xDocUserInfo = objCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string HelpBPGID = xDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Help Authoring']").Attributes["BPGID"].Value;
            string strBPE = Convert.ToString(xDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string strBPINFO = string.Empty;

            //SET BPINFO
            if (Session["LinkBPinfo"] != null)
            {
                string BPGID = string.Empty;
                XmlDocument xDocformBPGID = new XmlDocument();
                xDocformBPGID.LoadXml(Session["LinkBPinfo"].ToString());

                XmlNode nodeBPGID = xDocformBPGID.SelectSingleNode("bpinfo/BPGID");
                BPGID = nodeBPGID.InnerText;

                XmlNode nodeformBPGID = xDocformBPGID.SelectSingleNode("bpinfo");
                nodeformBPGID.RemoveAll();

                XmlNode nodenewBPGID = xDocformBPGID.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodenewBPGID.InnerText = HelpBPGID;
                nodeformBPGID.AppendChild(nodenewBPGID);


                XmlNode nodehelpBPGID = xDocformBPGID.CreateNode(XmlNodeType.Element, "HelpBPGID", null);
                nodehelpBPGID.InnerText = BPGID;
                nodeformBPGID.AppendChild(nodehelpBPGID);

                strBPINFO = nodeformBPGID.OuterXml.ToString();
            }
            CommonBO objBO = new CommonBO();
            string strReqXml = objBO.GenActionRequestXML("PAGELOAD", strBPINFO, "", "", "", strBPE, false, "1", "1", null);
            //BPOUT from DB
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);

            //Modify the request XML to insert the GridView node.
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(strReqXml);
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(strOutXml);

            ////Add the FileAttachement(File Upload) to the Control Collection
            //objCommonUI.GetPageControls(xDocOut, pnlEntryForm);
            //objCommonUI.PageControls.Add("FileUpload", FileAttachment);

            XmlNode nodeBPInfo = xDoc.SelectSingleNode("//bpinfo");
            nodeBPInfo.InnerXml += " <Gridview><Pagenumber>1</Pagenumber><Pagesize>20</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            objCommonUI.BindOutXML((Control)updtPnlContent, true, xDoc.OuterXml, strOutXml);

            //Change the Control name of XML Dictated node to FileAttachment(File Upload) controls ID
            //HiddenField hdnParentColNode = (HiddenField)BtnsUC.FindControl("hdnParentColNode");
            //hdnParentColNode.Value = hdnParentColNode.Value.Replace(":HelpFile~", ":" + txtFileAttachment.ID.Replace("txt", "") + "~");

            BasePage objBasePage = new BasePage();
            objBasePage.IsHelpPage = true;
            objBasePage.SetUIHeaders(updtPnlContent, (ButtonsUserControl)BtnsUC, pnlEntryForm, xDocOut);

            //Rename the close hyperlink in the Popup frame according to the header text.
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + objCommonUI.ButtonsUserControl.TreeNode + "/GridHeading/Title").InnerText;
            string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
        }

        private string UploadFile()
        {
            try
            {
                string fileName = string.Empty;
                if (txtFileAttachment.HasFile)
                {
                    string fileType = GetFileType(ddlHelpFileType.SelectedValue);
                    string helpFolderPath = ConfigurationManager.AppSettings["HelpFilesPath"] + "\\" + fileType.Replace(" ", "");
                    if (!System.IO.Directory.Exists(helpFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(helpFolderPath);
                    }
                    fileName = System.IO.Path.GetFileNameWithoutExtension(txtFileAttachment.PostedFile.FileName) + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + System.IO.Path.GetExtension(txtFileAttachment.PostedFile.FileName);
                    txtFileAttachment.PostedFile.SaveAs(helpFolderPath + @"\" + fileName);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Returns the application-friendly folder name for the storage of the help files.
        /// </summary>
        /// <param name="dbString">FileType from DB.</param>
        /// <returns>Application-friendly Folder Name.</returns>
        private string GetFileType(string dbString)
        {
            string trxID = dbString.Split('~')[0];
            string fileType = string.Empty;
            switch (trxID)
            {
                case "1":
                case "5"://Images
                    {
                        fileType = "Image";
                        break;
                    }
                case "2"://PDF
                    {
                        fileType = "PDF";
                        break;
                    }
                case "4"://StreamingVideo
                    {
                        fileType = "StreamingVideo";
                        break;
                    }
                default:
                    {
                        fileType = "Choose";
                        break;
                    }
            }
            return fileType;
        }

        private string DoUpload()
        {
            string uploadedFile = UploadFile();
            //In Modify Mode :
            //If uploaded file is different from the one specified in the XML delete the one in the XML.
            //If the uploaded file is empty delete the one specified in the XML.
            //In Delete mode delete the file specified in the XML.
            string prevFile = txtHelpFile.Attributes["File"];
            string currentAction = objCommonUI.ButtonsUserControl.CurrentAction;
            //Find the file and delete it if the record is deleted.
            if (prevFile != null && prevFile.Length > 0)
            {
                if (currentAction == "Delete")
                {
                    string helpFolderPath = ConfigurationManager.AppSettings["HelpFilesPath"];
                    string fileName = helpFolderPath + "\\" + GetFileType(ddlHelpFileType.SelectedValue) + "\\" + prevFile;
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
                if (currentAction == "Modify")
                {
                    //Check if the FileType has been changed. If yes then move the associated file into the correspoding
                    //FileType folder.

                    //Get the Original selection before the DDL change.
                    if (objCommonUI.ButtonsUserControl != null && !string.IsNullOrEmpty(objCommonUI.ButtonsUserControl.RwToBeModified))
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(objCommonUI.ButtonsUserControl.RwToBeModified);
                        string selVal = xDoc.FirstChild.Attributes[ddlHelpFileType.MapXML + "_TrxID"].Value
                                        + "~" + xDoc.FirstChild.Attributes[ddlHelpFileType.MapXML + "_TrxType"].Value;
                        if (!string.IsNullOrEmpty(selVal) && ddlHelpFileType.SelectedValue != selVal)
                        {
                            string helpFolderPath = ConfigurationManager.AppSettings["HelpFilesPath"];
                            string fileSource = helpFolderPath + "\\" + GetFileType(selVal) + "\\" + prevFile;
                            string fileDestFolder = helpFolderPath + "\\" + GetFileType(ddlHelpFileType.SelectedValue);
                            string fileDest = fileDestFolder + "\\" + prevFile;
                            if (fileSource != fileDest)
                            {
                                if (File.Exists(fileSource))
                                {
                                    //If selected file type is URL no need of creating any folder as it is not a file.
                                    //So just delete the file associated with this record.
                                    if (GetFileType(ddlHelpFileType.SelectedValue) != "Choose")
                                    {
                                        if (!Directory.Exists(fileDestFolder))
                                        {
                                            Directory.CreateDirectory(fileDestFolder);
                                        }
                                        //Move it.(First Copy the file and then delete the source)
                                        //File.Copy(fileSource, fileDest);
                                        File.Move(fileSource, fileDest);
                                    }
                                    else
                                    {
                                        File.Delete(fileSource);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (ddlHelpFileType.SelectedItem.Text != "URL")
            {
                txtHelpFile.Text = uploadedFile;//Pass the file name with the time stamp so that SubmitEntries will read it from here.
            }
            return uploadedFile;
        }

        private void RegisterTypesForAjax()
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxMethods));
            Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            string uploadedFile = DoUpload();

            //Calling the Submit method by passing the content page Update Panel as parameter
            objCommonUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);

            ResetHelpFileText(uploadedFile);
        }

        /// <summary>
        /// Reset the File attribute of the textbox with the uploaded file.
        /// If uploaded file is empty don't assign this to the textbox instead assign the file present in the file attribute.
        /// </summary>
        /// <param name="uploadedFile">Uploaded File.</param>
        private void ResetHelpFileText(string uploadedFile)
        {
            if (ddlHelpFileType.SelectedItem.Text != "URL" && uploadedFile.Length > 0)
            {
                //Change the file name to its original one(Remove the time stamp).
                txtHelpFile.Text = objCommonUI.TrimTimeStamp(uploadedFile);
                txtHelpFile.Attributes["File"] = uploadedFile;
            }
            else if (!string.IsNullOrEmpty(txtHelpFile.Attributes["File"]) && objCommonUI.ButtonsUserControl.CurrentAction == "Modify")
            {
                txtHelpFile.Text = txtHelpFile.Attributes["File"];
            }
        }

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            string uploadedFile = DoUpload();
            //Calling the Save method by passing the content page Update Panel as parameter
            objCommonUI.SaveEntries(updtPnlContent);

            ResetHelpFileText(uploadedFile);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            string uploadedFile = DoUpload();
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            objCommonUI.AddMultipleRecords(updtPnlContent);

            ResetHelpFileText(uploadedFile);
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            string uploadedFile = DoUpload();
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            objCommonUI.AddCloneRecords(updtPnlContent);

            ResetHelpFileText(uploadedFile);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            objCommonUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }
    }
}
