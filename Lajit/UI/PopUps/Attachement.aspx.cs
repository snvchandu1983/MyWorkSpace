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
using System.Text;
using NLog;


namespace LAjitDev.PopUps
{
    public partial class Attachement : Classes.BasePagePopUp
    {
        LAjit_BO.CommonBO commonObjBO = new LAjit_BO.CommonBO();
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        string m_treeNode = string.Empty;

        private static string m_AttachmentsPath; 
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();

        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();

            AddCSSReferences();
            
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            //html div tag
            HtmlControl hcDiv = (HtmlControl)this.Page.FindControl("divInitAttachement");
            if (!Page.IsPostBack)
            {
                
                m_AttachmentsPath =  ConfigurationManager.AppSettings["AttachmentsPath"] + "/" + HttpContext.Current.Session["CompanyEntityID"].ToString();
                //Getting parent selected row in querystring
                if ((Request.QueryString["BPGID"] != null) && (Request.QueryString["TrxID"] != null) && (Request.QueryString["TrxType"] != null))
                {
                    //hdnParentRow.Value = Request.QueryString["BPGID"].ToString() + "," + Request.QueryString["TrxID"].ToString() + Request.QueryString["TrxType"].ToString();
                    if ((Request.QueryString["BPGID"].ToString() != "null") && (Request.QueryString["TrxID"] != null) && (Request.QueryString["TrxType"] != null))
                    {
                        preloadAttachmentImages();

                        PreloadAttachementGV(XDocUserInfo);
                        //PreloadAttachementGV();
                        SetLabelCaptions();
                        FillDropDownData();
                        BPCVisbility();
                        pnlSubmit.Attributes.Add("style", "display:none;");
                        FileAttachment.Attributes.Add("onchange", "SetFilePath();");

                        if (hdnFldNoteAction.Value == string.Empty)
                        {
                            //by default it is add mode updated on 18-12-08
                            hdnFldNoteAction.Value = "ADD";
                            txtDescription.Enabled = true;
                            FileAttachment.Enabled = true;  //#CCCCCC
                            txtDescription.BackColor = Color.White;
                            FileAttachment.BackColor = Color.White;
                            pnlSubmit.Attributes.Add("style", "display:block;");
                            ddlFileType.Enabled = true;


                        }
                    }
                    else
                    {
                        //hide div display
                        hcDiv.Attributes.Add("style", "display:none");
                    }
                    //FileAttachment.Attributes.Add("onpropertychange", "SetFilePath();");
                    // imgbtnAttachAdd.OnClientClick = "SetFilePath()";
                    //txtDescription.Focus();
                    //Page.RegisterStartupScript("SetFocus", "<script>document.getElementById('" + txtDescription.ClientID + "').focus();</script>");
                }
                else
                {
                    //Unable process the page
                    //hide div display
                    hcDiv.Attributes.Add("style", "display:none");
                }

            }
            else
            {
                FileAttachment.Attributes.Add("onchange", "SetFilePath();");
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SetFocus(txtDescription.ClientID);
        }

        private void AddScriptReferences()
        {
        
            //CDN Added Scripts

            //jquery-1.3.2.min.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-1.3.2.min.js");
            Page.Header.Controls.Add(hgcScript1);

            //jquery.dialog.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery.dialog.js");
            Page.Header.Controls.Add(hgcScript2);

            //Utility.js
            HtmlGenericControl hgcScript3 = new HtmlGenericControl();
            hgcScript3.TagName = "script";
            hgcScript3.Attributes.Add("type", "text/javascript");
            hgcScript3.Attributes.Add("language", "javascript");
            hgcScript3.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Utility.js");
            Page.Header.Controls.Add(hgcScript3);

            //Common.js
            HtmlGenericControl hgcScript4 = new HtmlGenericControl();
            hgcScript4.TagName = "script";
            hgcScript4.Attributes.Add("type", "text/javascript");
            hgcScript4.Attributes.Add("language", "javascript");
            hgcScript4.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Common.js");
            Page.Header.Controls.Add(hgcScript4);
        }

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink css1 = new HtmlLink();
            css1.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            css1.Attributes["rel"] = "stylesheet";
            css1.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css1);

        }

        #region FootNote Methods

        protected void imgbtnAttachDelete_Click(object sender, ImageClickEventArgs e)
        {
            //EnableDisableNote(false);
            hdnFldNoteAction.Value = "Delete";

            //AttachSubmitEntries();
            AttachSubmitEntries(XDocUserInfo);

            txtDescription.Text = "";
            hdnFldNoteAction.Value = "";

            hdnFldNoteAction.Value = "";

            hdnRadIndex.Value = "";



            //attSubmitEntries();

            //PreloadNotesGV();
            // mpeNotes.Show();
        }

        protected void imgbtnAttachSubmit_Click(object sender, ImageClickEventArgs e)
        {
            bool boolAttach = false;
            if (FileAttachment.HasFile)
            {
                int fileSize = FileAttachment.PostedFile.ContentLength;
                fileSize = fileSize / 1024;
                string m_Extn = Path.GetExtension(FileAttachment.PostedFile.FileName);
                if (m_Extn.ToUpper().ToString() == ".JPG" || m_Extn.ToUpper().ToString() == ".JPEG" || m_Extn.ToUpper().ToString() == ".BMP" || m_Extn.ToUpper().ToString() == ".GIF" || m_Extn.ToUpper().ToString() == ".PNG" || m_Extn.ToUpper().ToString() == ".TIFF")
                {
                    if (fileSize < 100)
                    {
                        boolAttach = true;
                    }
                    else
                    {
                        boolAttach = false;
                        lblmsgNote.Visible=true;
                        lblmsgNote.Text = "File Size is Exceeding the Specified Image Size to Upload .So Please Upload image size less than 100 kb";
                    }
                }
                else
                {
                    boolAttach = true;
                }
            }
            if ((bool)boolAttach)
            {
                //if (CheckFileExist())
                //{
                //AttachSubmitEntries();
                AttachSubmitEntries(XDocUserInfo);
                hdnFldNoteAction.Value = "";
                hdnRadIndex.Value = "";
                //PreloadNotesGV();
                //mpeNotes.Show();
                //}
                //else
                //{
                //    //Attached is not a file
                //    txtDescription.Enabled = true;
                //    FileAttachment.Enabled = true;
                //    txtDescription.BackColor = Color.White;
                //    FileAttachment.BackColor = Color.White;
                //    pnlSubmit.Attributes.Add("style", "display:block;");
                //    ddlFileType.Enabled = true;
                //    PreloadAttachementGV();
                //    lblmsgNote.Visible = true;
                //    lblmsgNote.Text = "File not exist";
                //}
            }
        }

        protected void gvAttach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Option = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem == null)
                {
                    return;
                }
                else
                {
                    DataRowView drvCurrent = (DataRowView)e.Row.DataItem;

                    //Finding Literal control to add radio button
                    Literal litAttachment = (Literal)e.Row.Cells[0].FindControl("litRbtn");
                    if (litAttachment != null)
                    {
                        litAttachment.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "ROWXML") + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);

                        litAttachment.Text += String.Format("<input type='hidden' ID='hdnNotes' name='hdnNotes'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "Description") + "'", e.Row.RowIndex);


                    }
                    else
                    {
                        Literal litAttachmentR = new Literal();

                        if ((ViewState["SelectedRadIndex"] != null) && (hdnFldNoteAction.Value.ToUpper().ToString() == "MODIFY"))
                        {
                            if (ViewState["SelectedRadIndex"].ToString() == e.Row.RowIndex.ToString())
                            {
                                litAttachmentR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell' checked='checked'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "ROWXML") + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                            }
                            else
                            {
                                litAttachmentR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "ROWXML") + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                            }
                        }
                        else
                        {
                            litAttachmentR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "ROWXML") + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                        }

                        litAttachmentR.Text += String.Format("<input type='hidden' ID='hdnNotes' name='hdnNotes'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "Description") + "'", e.Row.RowIndex);
                        e.Row.Cells[0].Controls.Add(litAttachmentR);
                    }
                    //Renaming filename
                    if (e.Row.Cells[2].Text != string.Empty)
                    {
                        //string filename = e.Row.Cells[2].Text();

                        string fileName = System.IO.Path.GetFileNameWithoutExtension(e.Row.Cells[2].Text);
                        string fileExtension = System.IO.Path.GetExtension(e.Row.Cells[2].Text);
                        string[] fileTitle = fileName.Split(new Char[] { '~' });

                        if (fileTitle.Length > 1)
                        {
                            e.Row.Cells[2].Text = fileTitle[0].ToString() + fileExtension;
                        }


                    }

                }
            }
        }

        protected void gvAttach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAttach.PageIndex = e.NewPageIndex;
            if (ViewState["SORTEXP"] != null)
            {
                PreloadAttachementGVSort();
            }
            else
            {
                //PreloadAttachementGV();
                PreloadAttachementGV(XDocUserInfo);
            }
        }

        private void preloadAttachmentImages()
        {
            try
            {
                imgbtnAttachAdd.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png";
                imgbtnAttachAdd.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon_over.png'");
                imgbtnAttachAdd.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png'");

                //imgbtnAttachModify.ImageUrl = "~/App_Themes/" + Session["MyTheme"].ToString() + "/Images/modify_popup_icon.png";
                //imgbtnAttachModify.Attributes.Add("onmouseover", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/modify_popup_icon_over.png'");
                //imgbtnAttachModify.Attributes.Add("onmouseout", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/modify_popup_icon.png'");


                imgbtnAttachDelete.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png";
                imgbtnAttachDelete.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon_over.png'");
                imgbtnAttachDelete.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png'");

                imgbtnAttachSecure.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/secure_popup_icon.png";
                imgbtnAttachSecure.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/secure_popup_icon_over.png'");
                imgbtnAttachSecure.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/secure_popup_icon.png'");

                imgbtnAttachDownload.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/download_popup_icon.png";
                imgbtnAttachDownload.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/download_popup_icon_over.png'");
                imgbtnAttachDownload.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/download_popup_icon.png'");

                imgbtnAttachSubmit.ImageUrl =  Application["ImagesCDNPath"].ToString() + "images/attach-but.png";
                //imgbtnAttachCancel.ImageUrl = "~/App_Themes/" + Session["MyTheme"].ToString() + "/images/cancel-but.png";
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion

                throw ex;
            }
        }

        public void PreloadAttachementGV(XmlDocument XDocUserInfo)
        {
            try
            {
                string AttachBPGID = commonObjUI.GetBPCBPGID("Attachments");
                hdnCurrentBPGID.Value = AttachBPGID;
                string reqXmL = GenerateGVRequestXML(AttachBPGID, XDocUserInfo);
                string returnXML = commonObjBO.GetDataForCPGV1(reqXmL);
                ViewState["returnXML"] = returnXML;
                SetTreeNode();
                if (ViewState["returnXML"] != null)
                {
                    DisplayFoundResults(returnXML);
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

        private void SetTreeNode()
        {
            try
            {

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(ViewState["returnXML"].ToString());

                string treeNode = string.Empty;

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                {
                    m_treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
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


        private void PreloadAttachementGVSort()
        {
            try
            {
                if (ViewState["returnXML"].ToString() != "")
                {

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(ViewState["returnXML"].ToString());

                    string treeNode = string.Empty;

                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                    {
                        treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                    }

                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList");
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsVendors = new DataSet();
                    dsVendors.ReadXml(read);
                    DataTable dtVendors = new DataTable();
                    dtVendors = dsVendors.Tables[0];
                    DataView dvVendors = new DataView(dtVendors);
                    dvVendors.Sort = ViewState["SORTEXP"].ToString() + " " + ViewState["SORTDIRECTION"].ToString();

                    gvAttach.DataSource = dvVendors;
                    gvAttach.DataBind();
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


        private string GenerateGVRequestXML(string BPGID, XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("This is to generate a GV RequestXML basedon the BPGID : " + BPGID + " and USERINFO : " + XDocUserInfo); 
            #endregion

            try
            {
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                //nodeRoot.InnerXml = Session["BPE"].ToString();
                nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                //Creating the bpinfo node
                XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
                nodeRoot.AppendChild(nodeBPInfo);

                //Creating the BPGID node
                XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodeBPGID.InnerText = BPGID;
                nodeBPInfo.AppendChild(nodeBPGID);
                //<Object><TrxType>50</TrxType><TrxID>8</TrxID><BPGID>12</BPGID></Object>

                //  nodeBPInfo.InnerXml += hdnParentRow.Value.ToString();

                XmlNode nodeObejct = xDocGV.CreateNode(XmlNodeType.Element, "Object", null);
                nodeBPInfo.AppendChild(nodeObejct);

                XmlNode nodeObjBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodeObjBPGID.InnerText = Request.QueryString["BPGID"].ToString();
                nodeObejct.AppendChild(nodeObjBPGID);

                XmlNode nodeObjTrxID = xDocGV.CreateNode(XmlNodeType.Element, "TrxID", null);
                nodeObjTrxID.InnerText = Request.QueryString["TrxID"].ToString();
                nodeObejct.AppendChild(nodeObjTrxID);

                XmlNode nodeObjTrxType = xDocGV.CreateNode(XmlNodeType.Element, "TrxType", null);
                nodeObjTrxType.InnerText = Request.QueryString["TrxType"].ToString();
                nodeObejct.AppendChild(nodeObjTrxType);

                //Regular values

                // nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";

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

        public void DisplayFoundResults(string returnXML)
        {
            #region NLog
            logger.Info("This is to display the found results");
            #endregion

            try
            {

                string columnName = string.Empty;
                string treeNode = string.Empty;

                int colCntr = 0;
                int gridcolumn = 0;

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

                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList");
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsVendors = new DataSet();
                    dsVendors.ReadXml(read);

                    //Adding columns
                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns");

                    int gvColCount = gvAttach.Columns.Count;
                    for (int cntr = 1; cntr < gvColCount; cntr++)
                    {
                        gvAttach.Columns.Remove(gvAttach.Columns[1]);
                    }

                    //Add the columns only if no columns are present.
                    //if (gvNotes.Columns.Count == 1)
                    //{
                    //Creating the Columns.
                    foreach (XmlNode colNode in nodeColumns.ChildNodes)
                    {
                        if (colNode.Attributes["FullViewLength"] != null)
                        {
                            if (colNode.Attributes["FullViewLength"].Value != "0")
                            {
                                BoundField newField = new BoundField();
                                newField.DataField = colNode.Attributes["Label"].Value;
                                newField.HeaderText = colNode.Attributes["Caption"].Value;
                                newField.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                //newField.ItemStyle.CssClass = "myreportItemText";
                                //ItemStyle-HorizontalAlign="Left"  //
                                if (colNode.Attributes["IsSortable"].Value == "1")
                                {
                                    newField.SortExpression = colNode.Attributes["Label"].Value;
                                }

                                //Adding the current column node the ROWS dataset if not present.
                                if (!dsVendors.Tables[0].Columns.Contains(colNode.Attributes["Label"].Value))
                                {
                                    DataColumn dcNew = new DataColumn(colNode.Attributes["Label"].Value, typeof(string));
                                    dcNew.AllowDBNull = true;
                                    dsVendors.Tables[0].Columns.Add(dcNew);
                                }

                                gvAttach.Columns.Add(newField);
                                gridcolumn = gridcolumn + 1;
                                colCntr++;
                            }
                        }
                    }
                    //}
                    AddValidators(treeNode, returnXML);
                    trattachgrid.Visible = true;
                    gvAttach.Visible = true;
                    gvAttach.DataSource = dsVendors;
                    gvAttach.DataBind();
                    ButtonsVisbility(true);
                }
                else
                {
                    AddValidators(treeNode, returnXML);
                    trattachgrid.Visible = false;
                    gvAttach.Visible = false;
                    ButtonsVisbility(true);

                    //lblNotesGVmsg.Visible = true;
                    //lblNotesGVmsg.Text = "Data not found.";
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

        private void ButtonsVisbility(bool value)
        {
            trDelete.Visible = value;
            imgbtnAttachDelete.Visible = value;
            //trModify.Visible = value;
            //imgbtnAttachModify.Visible = value;
            trSecure.Visible = value;
            imgbtnAttachSecure.Visible = value;
            trDownload.Visible = value;
            imgbtnAttachDownload.Visible = value;
        }

        private void AddValidators(string treeNode, string returnXML)
        {
            try
            {

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(returnXML);

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns") != null)
                {

                    //Adding columns
                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns");

                    //Add the columns only if no columns are present.
                    //if (gvNotes.Columns.Count == 1)
                    //{
                    //Creating the Columns.
                    foreach (XmlNode colNode in nodeColumns.ChildNodes)
                    {

                        //Adding Required field validatator form controls
                        //IsRequired="1"  IsDisplayOnly="0"
                        if ((colNode.Attributes["IsRequired"].Value == "1") && (colNode.Attributes["IsDisplayOnly"].Value == "0"))
                        {
                            switch (colNode.Attributes["Label"].Value.ToUpper().ToString())
                            {
                                case "DESCRIPTION":
                                    reqDescription.Enabled = true;
                                    reqDescription.ErrorMessage = colNode.Attributes["Caption"].Value;
                                    break;
                                case "FILENAME":
                                    reqFileName.Enabled = true;
                                    reqFileName.ErrorMessage = colNode.Attributes["Caption"].Value;
                                    break;
                                case "FILETYPE":
                                    reqFileType.Enabled = true;
                                    reqFileType.InitialValue = "-1~51";
                                    reqFileType.ErrorMessage = colNode.Attributes["Caption"].Value;
                                    break;
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

                throw ex;
            }

        }


        private void SetLabelCaptions()
        {
            #region NLog
            logger.Info("This method is to set the lable captions");
            #endregion

            Label lbltext = new Label();
            TextBox txtmap = new TextBox();
            DropDownList ddlmap = new DropDownList();
            FileUpload fileupl = new FileUpload();

            string Controlname = string.Empty;
            string treeNode = string.Empty;

            XmlDocument xDoc = new XmlDocument();

            try
            {

                xDoc.LoadXml(ViewState["returnXML"].ToString());


                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                {
                    treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                }

                //Initialise Hidden input variable here for POST functionality.
                //SetPanelHeading(htcCPGV1, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title").InnerText);

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title") != null)
                {
                    lblPopupEntry.Text = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title").InnerText;
                }

                //htcCPGV1Auto.Width = Convert.ToString(600 - Convert.ToInt32(htcCPGV1.Width) - 28);

                //Adding columns
                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns");

                //Add the columns only if no columns are present.
                //if (gvNotes.Columns.Count == 1)
                //{
                //Creating the Columns.
                foreach (XmlNode colNode in nodeColumns.ChildNodes)
                {
                    //setting labeltext catpions in entry form
                    Controlname = "lbl" + colNode.Attributes["Label"].Value;

                    lbltext = (Label)this.Page.FindControl(Controlname);

                    if (lbltext != null)
                    {
                        lbltext.Text = colNode.Attributes["Caption"].Value;
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

        /// <summary>
        /// Sets the title of the given grid view.
        /// </summary>
        /// <param name="htcWork">Target HTML Table Cell.</param>
        /// <param name="gridTitle">String title to be set.</param>
        private void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            #region NLog
            logger.Info("Sets the title of the given grid view with : "+gridTitle);
            #endregion

            try
            {
                Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
                // need bitmap to call the MeasureString method
                Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
                //Padding 
                intScrollLength = intScrollLength + 20;
                htcWork.InnerText = gridTitle;
                htcWork.Width = intScrollLength.ToString();
                objGraphics.Dispose();
                objBitmap.Dispose();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }


        private string GetRowXml(string TrxID, string Option)
        {
            #region NLog
            logger.Info("This method gets the row  XML with trixid : "+TrxID+" and option : "+Option);
            #endregion

            string treeNodeName = string.Empty;
            string returnValue = string.Empty;

            StringBuilder sbXml = new StringBuilder();

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.LoadXml(ViewState["returnXML"].ToString());


                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                {
                    treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                }

                if (Option.ToUpper().ToString() == "ROWXML")
                {

                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']");

                    if (nodeRows.Attributes["Description"] != null)
                    {
                        nodeRows.Attributes["Description"].Value = commonObjUI.CharactersToHtmlCodes(nodeRows.Attributes["Description"].Value);
                    }

                    sbXml.Append("<" + treeNodeName + ">");
                    sbXml.Append("<RowList>");
                    sbXml.Append(nodeRows.OuterXml);
                    sbXml.Append("</RowList>");
                    sbXml.Append("</" + treeNodeName + ">");
                    returnValue = sbXml.ToString();
                }
                else if (Option.ToUpper().ToString() == "NOTE")
                {

                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Note"] != null)
                    {
                        returnValue = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Note"].Value.ToString();
                    }

                }
                else if (Option.ToUpper().ToString() == "DESCRIPTION")
                {

                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Description"] != null)
                    {
                        returnValue = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Description"].Value.ToString();
                    }
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private string GetTrxID(string SelectedRow)
        {
            #region NLog
            logger.Info("This method is used to get the TrxID for the selected row : "+SelectedRow);
            #endregion

            string TrxID;
            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.LoadXml(SelectedRow);
                TrxID = xDoc.SelectSingleNode("Attachments/RowList/Rows").Attributes["TrxID"].Value;
                return TrxID;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void AttachSubmitEntries(XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("This is to attach the submit entries with user info as : "+XDocUserInfo);
            #endregion

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.LoadXml(ViewState["returnXML"].ToString());

                string currBPGID = string.Empty;
                string CntrlValues = string.Empty;

                if (hdnFldNoteAction.Value.ToUpper().Trim() == "ADD")
                {
                    currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']").Attributes["BPGID"].Value;
                }
                else if (hdnFldNoteAction.Value.ToUpper().Trim() == "MODIFY")
                {
                    currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']").Attributes["BPGID"].Value;
                }
                else if (hdnFldNoteAction.Value.ToUpper().Trim() == "DELETE")
                {
                    currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']").Attributes["BPGID"].Value;
                }

                //get new row
                CntrlValues = commonObjUI.GetNewRowForAtchSecrNote(pnlAttachment, xDoc, hdnFldNoteAction.Value);


                //string trxID = string.Empty;
                //if (hdnFldNoteAction.Value.ToUpper().Trim() == "MODIFY" || hdnFldNoteAction.Value.ToUpper().Trim() == "DELETE")
                //{
                //    trxID = GetTrxID(hdnSectedRow.Value.ToString());
                //    //CntrlValues=hdnSectedRow.Value.ToString()
                //}



                // string strReqXml = commonObjBO.GenActionRequestXML(hdnFldNoteAction.Value.ToString(), currBPGID, CntrlValues, trxID, ViewState["returnXML"].ToString(), Session["BPE"].ToString(), true, "0");

                string strReqXml = ActionRequestXML(hdnFldNoteAction.Value.ToString(), currBPGID, CntrlValues);

                //BPOUT from DB
                string strOutXml = commonObjBO.GetDataForCPGV1(strReqXml);
                // GVUC.GVRequestXml = strReqXml;

                //BtnsUC.updateViewStateXml(hdnFldAction.Value.ToUpper().Trim(), trxID, strOutXml);

                XmlDocument returnXMLDoc = new XmlDocument();
                returnXMLDoc.LoadXml(strOutXml);
                //success messge

                XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                XmlNode nodeMsg = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");

                if (nodeMsgStatus.InnerText == "Success")
                {
                    //If action is delete remove file
                    if (hdnFldNoteAction.Value.ToUpper().Trim() == "DELETE")
                    {
                        DeleteFile();
                    }


                    lblmsgNote.Visible = true;

                    lblmsgNote.Text = nodeMsgStatus.InnerText + " " + nodeMsg.SelectSingleNode("Label").InnerText;
                    //lblmsgNote.Text = nodeMsgStatus.InnerText;



                    txtDescription.Enabled = false;
                    FileAttachment.Enabled = false;  //#CCCCCC
                    txtDescription.BackColor = Color.LightGray;
                    FileAttachment.BackColor = Color.LightGray;
                    pnlSubmit.Attributes.Add("style", "display:none;");
                    ddlFileType.Enabled = false;

                    //Bind data
                    PreloadAttachementGV(XDocUserInfo);

                }
                else
                {
                    //Fail
                    lblmsgNote.Visible = true;
                    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                    {
                        lblmsgNote.Text = nodeMsg.SelectSingleNode("OtherInfo").InnerText;
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

        private string ActionRequestXML(string BPAction, string BPGID, string CntrlValues)
        {
            #region NLog
            logger.Info("This to get the action request XML with BPAction : "+BPAction+" and BPGID as : "+BPGID+" and CntrlValues : "+CntrlValues);
            #endregion

            string TreeNodeName = string.Empty;
            string reqxml = string.Empty;
            XmlDocument returnXML = new XmlDocument();
            // string TrxID = hdnSectedRow.Value.ToString();
            string SelectedRow = string.Empty;

            try
            {
                returnXML.LoadXml(ViewState["returnXML"].ToString());

                TreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                string DataValueField = string.Empty;

                if (BPAction.ToUpper().Trim() == "ADD")
                {

                    ArrayList alFileInfo = new ArrayList();
                    //Uploading file and binding information to arraylist
                    UploadFile(alFileInfo);

                    ArrayList alTrxInfo = new ArrayList();


                    //Check dropdown selected value is DataValueField="-1~51" not to take

                    DataValueField = ddlFileType.SelectedValue.ToString();

                    string[] strarrddl = DataValueField.Split('~');


                    XmlDocument xDocAddVendor = new XmlDocument();
                    xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());


                    XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");
                    XmlNode nodeNote = xDocAddVendor.CreateNode(XmlNodeType.Element, TreeNodeName, null);
                    nodeBPINFO.AppendChild(nodeNote);

                    XmlNode nodeRows = xDocAddVendor.CreateNode(XmlNodeType.Element, "RowList", null);
                    nodeNote.AppendChild(nodeRows);

                    //XmlNode nodeRow = xDocAddVendor.CreateNode(XmlNodeType.Element, "Rows", null);
                    //nodeRows.AppendChild(nodeRow);

                    // XmlNode nodeRow = xDocAddVendor.CreateNode(XmlNodeType.Element, "Rows", null);
                    nodeRows.InnerXml = CntrlValues;
                    //nodeRows.AppendChild(nodeRow);

                    XmlNode nodeRow = xDocAddVendor.SelectSingleNode("Root/bpinfo/" + TreeNodeName + "/RowList/Rows");

                    XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                    attBPAction.Value = BPAction;
                    nodeRow.Attributes.Append(attBPAction);

                    //XmlAttribute attNote = xDocAddVendor.CreateAttribute("Description");
                    //attNote.Value = txtDescription.Text.ToString();
                    //nodeRow.Attributes.Append(attNote);


                    XmlAttribute attFileName = xDocAddVendor.CreateAttribute("FileName");
                    if (alFileInfo.Count > 0)
                    {
                        attFileName.Value = alFileInfo[0].ToString();
                    }
                    else
                    {
                        attFileName.Value = "";
                    }
                    nodeRow.Attributes.Append(attFileName);



                    XmlAttribute attSize = xDocAddVendor.CreateAttribute("Size");
                    if (alFileInfo.Count > 0)
                    {
                        attSize.Value = alFileInfo[2].ToString();
                    }
                    else
                    {
                        attSize.Value = "";
                    }
                    nodeRow.Attributes.Append(attSize);

                    //Based on file type get the  //TrxID="1" TrxType="1004" 

                    if (alFileInfo.Count > 0)
                    {
                        //dropdownlist selectedvalue is -1 
                        if (strarrddl[0].ToString() == "-1")
                        {
                            //Assigning FileType hot coded values pick from row xml
                            GetTrxInfoAndFileType(alFileInfo[1].ToString(), alTrxInfo);

                            DataValueField = alTrxInfo[0].ToString();
                        }
                    }

                    string[] strarr = DataValueField.Split('~');

                    XmlAttribute attTrxID = xDocAddVendor.CreateAttribute("TrxID");

                    if (strarr.Length > 0)
                    {
                        attTrxID.Value = strarr[0].ToString();
                    }
                    else
                    {
                        attTrxID.Value = "";
                    }
                    nodeRow.Attributes.Append(attTrxID);

                    XmlAttribute attTrxType = xDocAddVendor.CreateAttribute("TrxType");
                    if (strarr.Length > 0)
                    {
                        attTrxType.Value = strarr[1].ToString();
                    }
                    else
                    {
                        attTrxType.Value = "";
                    }
                    nodeRow.Attributes.Append(attTrxType);


                    //XmlAttribute attFileType = xDocAddVendor.CreateAttribute("FileType");

                    //XmlAttribute attFileType = xDocAddVendor.CreateAttribute("FileType_TrxID");

                    ////If selected text otherwise get the value from xml

                    ////dropdownlist selectedvalue is -1 
                    //if (strarrddl[0].ToString() == "-1")
                    //{
                    //    if (alTrxInfo.Count > 0)
                    //    {
                    //        attFileType.Value = alTrxInfo[1].ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    attFileType.Value =GetFileTypeTrxID(ddlFileType.SelectedValue.ToString());
                    //}
                    //nodeRow.Attributes.Append(attFileType);


                    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                    nodeBPGID.InnerXml = BPGID;

                    //Retriieving the BPEOut and ReportOut XML
                    //string reqxml = nodeRoot.OuterXml;
                    reqxml = xDocAddVendor.OuterXml;
                    //Creating the Vendor node

                }

                if ((BPAction.ToUpper().Trim() == "MODIFY") || (BPAction.ToUpper().Trim() == "DELETE"))
                {
                    //XmlNode nodeRows = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']");

                    //SelectedRow = "<" + TreeNodeName + ">";
                    //SelectedRow += "<RowList>";
                    //SelectedRow += nodeRows.OuterXml;
                    //SelectedRow += "</RowList>";
                    //SelectedRow += "</" + TreeNodeName + ">";
                    SelectedRow = hdnSectedRow.Value.ToString();


                    XmlDocument xDocAddVendor = new XmlDocument();
                    xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                    XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");

                    nodeBPINFO.InnerXml += SelectedRow;

                    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                    nodeBPGID.InnerXml = BPGID;

                    if (BPAction.ToUpper().Trim() == "DELETE")
                    {

                        XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/" + TreeNodeName + "/RowList/Rows");

                        XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                        attBPAction.Value = BPAction;
                        nodeRowlist.Attributes.Append(attBPAction);
                    }
                    else
                    {

                        ArrayList alFileInfo = new ArrayList();
                        //Uploading file and binding information to arraylist
                        UploadFile(alFileInfo);

                        ArrayList alTrxInfo = new ArrayList();

                        DataValueField = ddlFileType.SelectedValue.ToString();

                        string[] strarrddl = DataValueField.Split('~');

                        XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/" + TreeNodeName + "/RowList/Rows");//.Attributes["Note"];
                        XmlAttributeCollection attcolNote = nodeRowlist.Attributes;

                        //TrxID="8" TrxType="1004" Description="test 656565" FileName="14pan3a_inner.jpg"


                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(CntrlValues.ToString());
                        XmlNode nodeRowCntrls = xDoc.SelectSingleNode("Rows");
                        XmlAttributeCollection attcolRow = nodeRowCntrls.Attributes;
                        for (int i = 0; i < attcolRow.Count; i++)
                        {
                            if (attcolRow[i].Name.ToString() != null)
                            {
                                if (attcolNote[attcolRow[i].Name.ToString()] == null)
                                {
                                    XmlAttribute att = xDocAddVendor.CreateAttribute(attcolRow[i].Name.ToString());
                                    att.Value = txtDescription.Text.ToString();
                                    nodeRowlist.Attributes.Append(att);
                                }
                                else
                                {
                                    attcolNote[attcolRow[i].Name.ToString()].InnerXml = commonObjUI.CharactersToHtmlCodes(attcolRow[i].Value.ToString());
                                }
                            }
                        }


                        //attcolNote["Description"].InnerXml = txtDescription.Text.ToString();

                        if (alFileInfo.Count > 0)
                        {

                            attcolNote["FileName"].InnerXml = alFileInfo[0].ToString();
                            attcolNote["Size"].InnerXml = alFileInfo[2].ToString();
                        }


                        //Based on file type get the  TrxID and TrxType 

                        if (alFileInfo.Count > 0)
                        {
                            //dropdownlist selectedvalue is -1 
                            if (strarrddl[0].ToString() == "-1")
                            {
                                //Assigning FileType hot coded values pick from row xml
                                GetTrxInfoAndFileType(alFileInfo[1].ToString(), alTrxInfo);

                                DataValueField = alTrxInfo[0].ToString();

                            }
                            else
                            {
                                DataValueField = ddlFileType.SelectedValue.ToString();

                            }

                            string[] strarr = DataValueField.Split('~');

                            if (strarr.Length > 0)
                            {
                                attcolNote["FileType_TrxID"].InnerXml = strarr[0].ToString();
                                attcolNote["FileType_TrxType"].InnerXml = strarr[1].ToString();
                            }
                        }

                        XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                        attBPAction.Value = BPAction;
                        nodeRowlist.Attributes.Append(attBPAction);
                    }
                    reqxml = xDocAddVendor.OuterXml;


                }

                //if (BPAction.ToUpper().Trim() == "DELETE")
                //{
                //    XmlDocument xDocAddVendor = new XmlDocument();
                //    xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                //    XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");

                //    nodeBPINFO.InnerXml += hdnSectedRow.Value.ToString();

                //    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                //    nodeBPGID.InnerXml = BPGID;

                //    //<Attachments><RowList><Rows 
                //    XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Attachments/RowList/Rows");

                //    XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                //    attBPAction.Value = BPAction;
                //    nodeRowlist.Attributes.Append(attBPAction);

                //    reqxml = xDocAddVendor.OuterXml;

                //}
                return reqxml;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void FillDropDownData()
        {
            //XmlDataSource xDataSourceAttach = new XmlDataSource();  //(XmlDataSource)this.FindControl("xdsMenu");
            //xDataSourceAttach.Data = ViewState["returnXML"].ToString();
            //xDataSourceAttach.XPath = "/Root/bpeout/FormControls/FileType/RowList/Rows";
            //ddlFileType.DataValueField = "DataValueField"; //listColumns[count].ToString() + "_TrxID"; //+ "~" + listColumns[count].ToString() + "_TrxType";
            //ddlFileType.DataTextField = "FileType";  //listColumns[count].ToString();
            //ddlFileType.DataSource = xDataSourceAttach;
            //ddlFileType.DataBind();
            //xDataSourceAttach.EnableCaching = true;
            //xDataSourceAttach.EnableViewState = true;
            //xDataSourceAttach.Dispose();

            try
            {
                xdsAttachment.Data = ViewState["returnXML"].ToString();
                xdsAttachment.XPath = "/Root/bpeout/FormControls/FileType/RowList/Rows";
                ddlFileType.DataValueField = "DataValueField"; //listColumns[count].ToString() + "_TrxID"; //+ "~" + listColumns[count].ToString() + "_TrxType";
                ddlFileType.DataTextField = "FileType";  //listColumns[count].ToString();
                ddlFileType.DataSource = xdsAttachment;
                ddlFileType.DataBind();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void GetTrxInfoAndFileType(string fileExtension, ArrayList alTrxInfo)
        {

            //FileType="Excel Document" DataValueField="1~51" FileType_TrxID

            /*  XML FileExtension is not availble
             * 
             * XmlDocument xDoc=new XmlDocument();
             xDoc.LoadXml(ViewState["returnXML"].ToString()); 
           
            if(xDoc.SelectSingleNode("Root/bpeout/FormControls/FileType/RowList/Rows[@FileExtension='"+ fileExtension +"']").Attributes["DataValueField"]!=null)
            {
              alTrxInfo.Add(xDoc.SelectSingleNode("Root/bpeout/FormControls/FileType/RowList/Rows[@FileExtension='"+ fileExtension +"']").Attributes["DataValueField"].Value);
              alTrxInfo.Add(xDoc.SelectSingleNode("Root/bpeout/FormControls/FileType/RowList/Rows[@FileExtension='" + fileExtension + "']").Attributes["FileType_TrxID"].Value);
            }*/
            //hot coded Miscellaneous
            //<Rows TrxID="99" TrxType="51" FileType_TrxID="99" FileType_TrxType="51" FileType="Miscellaneous" DataValueField="99~51" SortOrder="7" /> 

            alTrxInfo.Add("99~51");
            alTrxInfo.Add("99");
        }

        private string GetFileTypeTrxID(string DataValueField)
        {
            #region NLog
            logger.Info("This is to geth file type and TrxID : "+DataValueField);
            #endregion

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.LoadXml(ViewState["returnXML"].ToString());

                string returnValue = string.Empty;

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/FileType/RowList/Rows[@DataValueField='" + DataValueField + "']").Attributes["FileType_TrxID"] != null)
                {
                    returnValue = xDoc.SelectSingleNode("Root/bpeout/FormControls/FileType/RowList/Rows[@DataValueField='" + DataValueField + "']").Attributes["FileType_TrxID"].Value;
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }


        private void UploadFile(ArrayList alFileInfo)
        {
            try
            {
                if (FileAttachment.HasFile)
                {
                    string Filename = System.IO.Path.GetFileNameWithoutExtension(FileAttachment.PostedFile.FileName.ToString().Replace(" ", "")) + "~" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + System.IO.Path.GetExtension(FileAttachment.PostedFile.FileName.ToString());

                    
                   
                    if (!System.IO.Directory.Exists(m_AttachmentsPath))
                    {
                        System.IO.Directory.CreateDirectory(m_AttachmentsPath);
                    }
                    FileAttachment.PostedFile.SaveAs(m_AttachmentsPath + @"\" + Filename);

                    alFileInfo.Add(Filename);

                    string fileExtension = System.IO.Path.GetExtension(FileAttachment.PostedFile.FileName.ToString());
                    //File Extension
                    alFileInfo.Add(fileExtension.Substring(1, fileExtension.Length - 1));
                    //File Size
                    string filesizeKB = ConvertBytesToKilobytes(FileAttachment.PostedFile.ContentLength).ToString("0");

                    alFileInfo.Add(filesizeKB);
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

        private string GetFileName()
        {
            //hdnSectedRow.ToString();

            XmlDocument xdoc = new XmlDocument();

            try
            {
                xdoc.LoadXml(hdnSectedRow.Value.ToString());

                string filename = string.Empty;

                if ((xdoc.SelectSingleNode("Attachments/RowList/Rows").Attributes["FileName"]) != null)
                {
                    filename = xdoc.SelectSingleNode("Attachments/RowList/Rows").Attributes["FileName"].Value;
                }
                return filename;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private double ConvertBytesToKilobytes(long b)
        {
            return (b / 1024f);
        }

        private void DeleteFile()
        {
            string SelectedRow = hdnSectedRow.Value.ToString();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(SelectedRow.ToString());

                XmlNode nodeRow = xDoc.SelectSingleNode("Attachments/RowList/Rows");
                XmlAttributeCollection attcolRow = nodeRow.Attributes;


                //XmlNode xnode=
                if (attcolRow["FileName"] != null)
                {
                    string Filepath = m_AttachmentsPath + @"\" + attcolRow["FileName"].Value.ToString();
                    bool FileExist = File.Exists(Filepath);

                    if (FileExist)
                    {
                        File.Delete(Filepath);
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

        //private void BPCVisbility()
        //{
        //    if (ViewState["returnXML"] != null)
        //    {
        //        XmlDataDocument xDoc = new XmlDataDocument();
        //        xDoc.LoadXml(ViewState["returnXML"].ToString());


        //        if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null))
        //        {
        //            imgbtnAttachAdd.Visible = false;
        //        }
        //        else if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") == null))
        //        {
        //            imgbtnAttachDelete.Visible = false;
        //        }
        //    }
        //}


        private void BPCVisbility()
        {
            XmlDocument xDocRPGV = new XmlDocument();

            try
            {
                xDocRPGV.LoadXml(ViewState["returnXML"].ToString());

                //XmlDocument xDocBPE = new XmlDocument();
                //xDocBPE.LoadXml(Session["BPE"].ToString());

                if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null))
                {
                    trAdd.Visible = false;
                    imgbtnAttachAdd.Visible = false;
                }

                //if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']") == null))
                //{
                //trModify.Visible = false;
                //imgbtnAttachModify.Visible = false;
                //}

                if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") == null))
                {
                    trDelete.Visible = false;
                    imgbtnAttachDelete.Visible = false;
                }

                if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNode + "/GridHeading/GridExtendedColumns/IsSecured").InnerText != "1"))
                {
                    trSecure.Visible = false;
                    imgbtnAttachSecure.Visible = false;
                }


                //if ((xDocBPE.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='32']") == null)
                // && (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsNoted").InnerText != "1"))
                //{
                //    HtmlTableRow tr = (HtmlTableRow)this.FindControl("trImgbtnNote");
                //    if (tr != null)
                //    {
                //        tr.Visible = false;
                //        imgbtnNote.Attributes["CmnDisplay"] = "false";
                //    }
                //}
                //if ((xDocBPE.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='33']") == null)
                //&& (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsAttached").InnerText != "1"))
                //{
                //    HtmlTableRow tr = (HtmlTableRow)this.FindControl("trImgbtnAttachment");
                //    if (tr != null)
                //    {
                //        tr.Visible = false;
                //        imgbtnAttachment.Attributes["CmnDisplay"] = "false";
                //    }
                //}

            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private bool CheckFileExist()
        {
            return System.IO.File.Exists(hdnUplFullPath.Value);
        }


        #endregion

        protected void imgbtnAttachDownload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // string filePath = System.AppDomain.CurrentDomain.BaseDirectory +"Attachments\\" + GetFileName();  //"250408err.doc";

                //  string folderPath = ConfigurationManager.AppSettings["AttachmentsPath"].ToString();
                string filePath = m_AttachmentsPath + @"\" + GetFileName();

                if (File.Exists(filePath))
                {
                    commonObjUI.DownloadFile(filePath);
                }
                else
                {
                    lblmsgNote.Visible = true;
                    lblmsgNote.Text = "File not found..";
                    PreloadAttachementGV(XDocUserInfo);
                }
            }
            catch(Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        protected void gvAttach_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                if (ViewState["SORTDIRECTION"] != null)
                {

                    if (ViewState["SORTDIRECTION"].ToString() == "ASC")
                    {
                        ViewState["SORTDIRECTION"] = "DESC";
                    }
                    else
                    {
                        ViewState["SORTDIRECTION"] = "ASC";
                    }
                }
                else
                {
                    ViewState["SORTDIRECTION"] = "DESC";
                }

                ViewState["SORTEXP"] = e.SortExpression;

                PreloadAttachementGVSort();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
    }
}
