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
using System.Text;
using System.Drawing;
using LAjit_BO;
using System.IO;
using NLog;



namespace LAjitDev.PopUps
{
    public partial class Notes : Classes.BasePagePopUp
    {
        LAjit_BO.CommonBO commonObjBO = new LAjit_BO.CommonBO();
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public String TreeNode
        {
            get { return (String)ViewState["m_treeNodeName"]; }
            set { ViewState["m_treeNodeName"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //html div tag
            HtmlControl hcDiv = (HtmlControl)this.Page.FindControl("divNote");

            AddScriptReferences();

            AddCSSReferences();

            if (!Page.IsPostBack)
            {
                try
                {
               
                    //Getting parent selected row in querystring
                    if ((Request.QueryString["BPGID"] != null) && (Request.QueryString["TrxID"] != null) && (Request.QueryString["TrxType"] != null))
                    {
                        if ((Request.QueryString["BPGID"].ToString() != "null") && (Request.QueryString["TrxID"] != null) && (Request.QueryString["TrxType"] != null))
                        {
                            hdnParentRow.Value = Request.QueryString["BPGID"].ToString() + "," + Request.QueryString["TrxID"].ToString() + Request.QueryString["TrxType"].ToString();

                            preloadNoteImages();

                            PreloadNotesGV();

                            SetLabelCaptions();

                            ClearContols();

                            BPCVisbility();

                            //txtNotes.Focus();

                            //Page.RegisterStartupScript("SetFocus", "<script>document.getElementById('" + txtNotes.ClientID + "').focus();</script>");
                        }
                        else
                        {
                            //hide div display
                            hcDiv.Attributes.Add("style", "display:none");
                        }
                    }
                    else
                    {
                        //Unable process this page
                        //hide div display
                        hcDiv.Attributes.Add("style", "display:none");
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
        protected void Page_Init(object sender, EventArgs e)
        {
            SetFocus(txtNote.ClientID);
        }


        protected void imgbtnNoteDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EnableDisableNote(false);
                hdnFldNoteAction.Value = "Delete";
                NoteSubmitEntries();
                txtNote.Text = "";
                //PreloadNotesGV();
                // mpeNotes.Show();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        protected void imgbtnNoteSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                NoteSubmitEntries();

                hdnFldNoteAction.Value = "";

                hdnRadIndex.Value = "";

                // PreloadNotesGV();

                //mpeNotes.Show();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        protected void imgbtnNoteCancel_Click(object sender, ImageClickEventArgs e)
        {

            //mpeNotes.Hide();
            PreloadNotesGV();
        }


        protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           string SharedUpdate = string.Empty;
                try
                {
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
                        Literal litNote = (Literal)e.Row.Cells[0].FindControl("litRbtn");
                        if (litNote != null)
                        {
                            litNote.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + drvCurrent.Row.ItemArray[0].ToString() + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                            litNote.Text += String.Format("<input type='hidden' ID='hdnNotes' name='hdnNotes'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "NOTE-SHAREDUPDATE") + "'>", e.Row.RowIndex);
                            
                        }
                        else
                        {
                            Literal litNoteR = new Literal();

                            if ((ViewState["SelectedRadIndex"] != null) && (hdnFldNoteAction.Value.ToUpper().ToString() == "MODIFY"))
                            {
                                if (ViewState["SelectedRadIndex"].ToString() == e.Row.RowIndex.ToString())
                                {
                                    litNoteR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell' checked='checked'  value='" + drvCurrent.Row.ItemArray[0].ToString() + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                                }
                                else
                                {
                                    litNoteR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + drvCurrent.Row.ItemArray[0].ToString() + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                                }
                            }
                            else
                            {
                                litNoteR.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + drvCurrent.Row.ItemArray[0].ToString() + "' onclick=javascript:UpdateRadioNotesData('" + e.Row.RowIndex + "') />", e.Row.RowIndex);
                            }

                            litNoteR.Text += String.Format("<input type='hidden' ID='hdnNotes' name='hdnNotes'  value='" + GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "NOTE-SHAREDUPDATE") + "'>", e.Row.RowIndex);

                            e.Row.Cells[0].Controls.Add(litNoteR);
                        }

                        //Setting shared update value
                        SharedUpdate = GetRowXml(drvCurrent.Row.ItemArray[0].ToString(), "SharedUpdate");
                        if (SharedUpdate == "1")
                        {
                            e.Row.Cells[4].Text = "&radic;";
                        }
                        else
                        {
                            e.Row.Cells[4].Text = "x";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        protected void gvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvNotes.PageIndex = e.NewPageIndex;

                if (ViewState["SORTEXP"] != null)
                {
                    PreloadNotesGVSort();
                }
                else
                {
                    PreloadNotesGV();
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }


        #region FootNote Methods

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink css1 = new HtmlLink();
            css1.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            css1.Attributes["rel"] = "stylesheet";
            css1.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css1);

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


        private void preloadNoteImages()
        {

            try
            {
                imgbtnNoteAdd.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png";
                imgbtnNoteAdd.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon_over.png'");
                imgbtnNoteAdd.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png'");

                imgbtnNoteModify.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon.png";
                imgbtnNoteModify.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon_over.png'");
                imgbtnNoteModify.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon.png'");

                imgbtnNoteDelete.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png";
                imgbtnNoteDelete.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon_over.png'");
                imgbtnNoteDelete.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png'");

                imgbtnNoteSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/note-but.png";
                //imgbtnNoteCancel.ImageUrl = "~/App_Themes/" + Session["MyTheme"].ToString() + "/images/cancel-but.png";

            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void PreloadNotesGV()
        {
            try
            {
                string NotesBPGID = commonObjUI.GetBPCBPGID("Notes");
                string reqXmL = GenerateGVRequestXML(NotesBPGID);
                string returnXML = commonObjBO.GetDataForCPGV1(reqXmL);
                ViewState["returnXML"] = returnXML;
                if (returnXML != null)
                {
                    lblNotesGVmsg.Visible = false;
                    DisplayFoundResults(returnXML);
                }
                else
                {
                    lblNotesGVmsg.Visible = true;
                    lblNotesGVmsg.Text = "Data not found.";
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

        private void PreloadNotesGVSort()
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

                    gvNotes.DataSource = dvVendors;
                    gvNotes.DataBind();
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

        private string GenerateGVRequestXML(string BPGID)
        {
            #region NLog
            logger.Info("This method is used to generate the GV Request XMl based on the BPGID as : " + BPGID); 
            #endregion

            try
            {
                XmlDocument XDocUserInfo = new XmlDocument();
                CommonUI commonObjUI = new CommonUI();
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

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


                //nodeBPInfo.InnerXml += hdnParentRow.Value.ToString();

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

            string columnName = string.Empty;
            string treeNode = string.Empty;

            int colCntr = 0;
            int gridcolumn = 0;

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.LoadXml(returnXML);

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                {
                    treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                    TreeNode = treeNode;
                    //to be remove afterwards
                }
           
                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList") != null)
                {

                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/RowList");
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsVendors = new DataSet();
                    dsVendors.ReadXml(read);

                    //Adding columns
                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns");

                    int gvColCount = gvNotes.Columns.Count;
                    for (int cntr = 1; cntr < gvColCount; cntr++)
                    {
                        gvNotes.Columns.Remove(gvNotes.Columns[1]);
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

                                gvNotes.Columns.Add(newField);
                                gridcolumn = gridcolumn + 1;
                                colCntr++;
                            }
                        }
                    }
                    //}
                    //Data found

                    AddValidators(treeNode, returnXML);
                    trgview.Visible = true;
                    gvNotes.Visible = true;
                    gvNotes.DataSource = dsVendors;
                    gvNotes.DataBind();
                    ButtonsVisbility(true);
                    //hdnButtons.Value = "block";
                }
                else
                {
                    //Data not found
                    AddValidators(treeNode, returnXML);
                    trgview.Visible = false;
                    gvNotes.Visible = false;
                    ButtonsVisbility(false);
                    //hdnButtons.Value = "none";
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
            imgbtnNoteDelete.Visible = value;
            trModify.Visible = value;
            imgbtnNoteModify.Visible = value;
        }

        private void AddValidators(string treeNode,string returnXML)
        {
            XmlDocument xDoc = new XmlDocument();

            try
            {
            xDoc.LoadXml(returnXML);
        
            if( xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Columns") != null)
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
                                case "NOTE":
                                            reqNote.Enabled = true;
                                            reqNote.ErrorMessage = colNode.Attributes["Caption"].Value;
                                            regNote.Enabled = true;

                                            break;
                                //case "SHAREDUPDATE":
                                //            reqShared.Enabled = true;
                                //            break;
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
            //htcCPGV1Auto.Width = Convert.ToString(600 - Convert.ToInt32(htcCPGV1.Width) - 28);

            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title") != null)
            {
                lblPopupEntry.Text = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title").InnerText;
            }


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
            logger.Info("Sets the title of the given grid view with : " + gridTitle);
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
            logger.Info("This method gets the row  XML with trixid : " + TrxID + " and option : " + Option);
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

                //sbXml.Append("<" + treeNodeName + ">");
                //sbXml.Append("<RowList>");
                //sbXml.Append(nodeRows.OuterXml);
                //sbXml.Append("</RowList>");
                //sbXml.Append("</" + treeNodeName + ">");
                sbXml.Append(TrxID);
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
            else if (Option.ToUpper().ToString() == "SHAREDUPDATE")
            {

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["SharedUpdate"] != null)
                {
                    returnValue = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["SharedUpdate"].Value.ToString();
                }
            }
            else if (Option.ToUpper().ToString() == "NOTE-SHAREDUPDATE")
            {
                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Note"] != null)
                {
                    returnValue = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["Note"].Value.ToString();
                }

                if (xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["SharedUpdate"] != null)
                {
                    returnValue = returnValue  +"~" + xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']").Attributes["SharedUpdate"].Value.ToString();
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

        private void EnableDisableNote(bool value)
        {
            txtNote.Enabled = value;
            //chkAgree.Enabled = value;
            //imgbtnNoteSubmit.Enabled = value;
        }

        private string GetTrxID(string SelectedRow)
        {
            #region NLog
            logger.Info("This method is used to get the TrxID for the selected row : " + SelectedRow);
            #endregion

            string TrxID;

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(SelectedRow);
            TrxID = xDoc.SelectSingleNode("Notes/RowList/Rows").Attributes["TrxID"].Value;
            return TrxID;
        }

        private void NoteSubmitEntries()
        {

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

                string Shared = "0";

                if (chkAgree.Checked == true)
                {
                    Shared = "1";
                }               
                
                //get new row
                CntrlValues = commonObjUI.GetNewRowForAtchSecrNote(pnlNote, xDoc, hdnFldNoteAction.Value);

                //Reqxml
                string strReqXml = ActionRequestXML(hdnFldNoteAction.Value.ToString(), currBPGID, Shared,CntrlValues);

                //BPOUT from DB
                string strOutXml = commonObjBO.GetDataForCPGV1(strReqXml);
                // GVUC.GVRequestXml = strReqXml;
              
                XmlDocument returnXMLDoc = new XmlDocument();
                returnXMLDoc.LoadXml(strOutXml);
                //success messge
                XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                XmlNode nodeMsg = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");

                if (nodeMsgStatus.InnerText == "Success")
                {
                    //Success
                    lblmsgNote.Visible = true;
                    //lblmsgNote.Text = nodeMsgStatus.InnerText;
                    lblmsgNote.Text = nodeMsgStatus.InnerText + " " + nodeMsg.SelectSingleNode("Label").InnerText;

                    PreloadNotesGV();

                    hdnButtons.Value = "block";
                }
                else
                {
                    //Fail
                    lblmsgNote.Visible = true;
                    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                    {
                        lblmsgNote.Text = nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                    }
                    PreloadNotesGV();
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

        private string ActionRequestXML(string BPAction, string BPGID, string Shared, string CntrlValues)
        {
            #region NLog
            logger.Info("This to get the action request XML with BPAction : " + BPAction + " and BPGID as : " + BPGID + " and CntrlValues : " + CntrlValues);
            #endregion

            string TreeNodeName = string.Empty;
            string reqxml = string.Empty;
            XmlDocument returnXML = new XmlDocument();
            string SelectedRow = string.Empty;
            string TrxID = hdnSectedRow.Value.ToString();

            try
            {
                returnXML.LoadXml(ViewState["returnXML"].ToString());

               // TreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                if (BPAction.ToUpper().Trim() == "ADD")
                {
                    XmlDocument xDocAddVendor = new XmlDocument();
                    xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                    XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");
                    XmlNode nodeNote = xDocAddVendor.CreateNode(XmlNodeType.Element, TreeNode, null);
                    nodeBPINFO.AppendChild(nodeNote);

                    XmlNode nodeRows = xDocAddVendor.CreateNode(XmlNodeType.Element, "RowList", null);
                    nodeRows.InnerXml = CntrlValues;
                    nodeNote.AppendChild(nodeRows);

                    //XmlNode nodeRow = xDocAddVendor.CreateNode(XmlNodeType.Element, "Rows", null);
                    //nodeRows.AppendChild(nodeRow);


                    XmlNode nodeRow = xDocAddVendor.SelectSingleNode("Root/bpinfo/" + TreeNode + "/RowList/Rows");


                    XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                    attBPAction.Value = BPAction;
                    nodeRow.Attributes.Append(attBPAction);

                    //XmlAttribute attNote = xDocAddVendor.CreateAttribute("Note");
                    //attNote.Value = txtNotes.Text.ToString();
                    //nodeRow.Attributes.Append(attNote);


                    XmlAttribute attSharedUpdate = xDocAddVendor.CreateAttribute("SharedUpdate");
                    attSharedUpdate.Value = Shared;
                    nodeRow.Attributes.Append(attSharedUpdate);


                    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                    nodeBPGID.InnerXml = BPGID;

                    //Retriieving the BPEOut and ReportOut XML
                    //string reqxml = nodeRoot.OuterXml;
                    reqxml = xDocAddVendor.OuterXml;

                    //Creating the Vendor node

                }

                if ((BPAction.ToUpper().Trim() == "MODIFY")||(BPAction.ToUpper().Trim() == "DELETE"))
                {
                     //XmlDocument xDoc1 = new XmlDocument();
                     //xDoc1.LoadXml(ViewState["returnXML"].ToString());

                     XmlNode nodeRows = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList//Rows[@TrxID='" + TrxID + "']");
                  
                    SelectedRow = "<" + TreeNode + ">";
                    SelectedRow += "<RowList>";
                    SelectedRow += nodeRows.OuterXml;
                    SelectedRow += "</RowList>";
                    SelectedRow += "</" + TreeNode + ">";
           
                    XmlDocument xDocAddVendor = new XmlDocument();
                    xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                    XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");
                    nodeBPINFO.InnerXml += SelectedRow;


                    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                    nodeBPGID.InnerXml = BPGID;

                   

                    if (BPAction.ToUpper().Trim() == "DELETE")
                    {
                       

                        XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Notes/RowList/Rows");

                        XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                        attBPAction.Value = BPAction;
                        nodeRowlist.Attributes.Append(attBPAction);
                    }
                    else
                    {
                        //MODIFY
                    
                        XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Notes/RowList/Rows");//.Attributes["Note"];
                        XmlAttributeCollection attcolNote = nodeRowlist.Attributes;

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
                                    att.Value = txtNote.Text.ToString();
                                    nodeRowlist.Attributes.Append(att);
                                }
                                else
                                {
                                    attcolNote[attcolRow[i].Name.ToString()].InnerXml = commonObjUI.CharactersToHtmlCodes(attcolRow[i].Value.ToString());
                                }
                            }
                        }

                        attcolNote["SharedUpdate"].InnerXml = Shared.ToString();
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

                //    //nodeBPINFO.InnerXml += hdnSectedRow.Value.ToString();
                    
                //    nodeBPINFO.InnerXml += SelectedRow;

                //    XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                //    nodeBPGID.InnerXml = BPGID;
                   
                    
                //    XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Notes/RowList/Rows");

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

        private void ClearContols()
        {
            txtNote.Text = "";
            lblmsgNote.Text = "";
            lblNotesGVmsg.Text = "";
            chkAgree.Checked = false;
        }

        private void BPCVisbility()
        {
            try
            {
                if (ViewState["returnXML"] != null)
                {
                    XmlDataDocument xDoc = new XmlDataDocument();
                    xDoc.LoadXml(ViewState["returnXML"].ToString());

                    if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null))
                    {
                        trAdd.Visible = false;
                        imgbtnNoteAdd.Visible = false;
                    }
                    else if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']") == null))
                    {
                        trModify.Visible = false;
                        imgbtnNoteModify.Visible = false;
                    }
                    else if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") == null))
                    {
                        trDelete.Visible = false;
                        imgbtnNoteDelete.Visible = false;
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

        #endregion

        protected void gvNotes_Sorting(object sender, GridViewSortEventArgs e)
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

                PreloadNotesGVSort();
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
