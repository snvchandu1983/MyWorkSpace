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
using NLog;



namespace LAjitDev.PopUps
{
    public partial class Secure : Classes.BasePagePopUp
        
        //System.Web.UI.Page
    {
        LAjit_BO.CommonBO commonObjBO = new LAjit_BO.CommonBO();
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
       
        private string m_treeNode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();

            AddCSSReferences();

            if (!Page.IsPostBack)
            {
                //Getting parent selected row in querystring
                if ((Request.QueryString["BPGID"] != null) && (Request.QueryString["TrxID"] != null) && (Request.QueryString["TrxType"] != null))
                {
                    //hdnParentRow.Value = Request.QueryString["BPGID"].ToString() + "," + Request.QueryString["TrxID"].ToString() + Request.QueryString["TrxType"].ToString();

                    //Loading images
                    preloadSecureImages();

                    //Set ReturnXML
                    setReturnXML();

                    //set Page level labels
                    SetCaptionsValidations();
                   
                    //Fill dropdown data
                    FillDropDownData();

                    //Set treenode global
                    SetTreeNode();

                    //Rowlist available preselect dropdown data and store updatedrow in viewstate
                    SetUpdateRow(ViewState["returnXML"].ToString());

                    //Setting ui visiblity of buttons
                    BPCVisbility();

                }
                else
                {
                    Response.Write("Required Information Not Found.");
                }
            }
        }

        #region Private Methods..

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

        private void preloadSecureImages()
        {

            imgbtnSecureAdd.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png";
            imgbtnSecureAdd.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon_over.png'");
            imgbtnSecureAdd.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/add_popup_icon.png'");

            imgbtnSecureModify.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon.png";
            imgbtnSecureModify.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon_over.png'");
            imgbtnSecureModify.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/modify_popup_icon.png'");

            imgbtnSecureDelete.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png";
            imgbtnSecureDelete.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon_over.png'");
            imgbtnSecureDelete.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_popup_icon.png'");

            imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/secure-but.png";
        }

        private void setReturnXML()
        {
            //GBPC getting secure id
            string SecureBPGID = commonObjUI.GetBPCBPGID("SecureItems"); //"58"; 
            string reqXmL = GenerateGVRequestXML(SecureBPGID);
            string returnXML = commonObjBO.GetDataForCPGV1(reqXmL);
            ViewState["returnXML"] = returnXML;
        }

        private string GenerateGVRequestXML(string BPGID)
        {
            #region NLog
            logger.Info("This method is used to generate the GV Request XMl based on the BPGID : " + BPGID);
            #endregion

            XmlDocument xDocGV = new XmlDocument();
            XmlDocument XDocUserInfo = new XmlDocument();
            CommonUI commonObjUI = new CommonUI();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

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


        private void SetCaptionsValidations()
        {
            Label lbltext = new Label();
            string Controlname = string.Empty;
            string treeNode = string.Empty;

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(ViewState["returnXML"].ToString());


            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            {
                treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            }

            //Initialise Hidden input variable here for POST functionality.
           // SetPanelHeading(htcCPGV1, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "/GridHeading/Title").InnerText);
           // htcCPGV1Auto.Width = Convert.ToString(537 - Convert.ToInt32(htcCPGV1.Width) - 28);
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

                //IsRequired="1" IsUnique="0" IsNumeric="1" IsLink="1" IsHidden="0" IsDisplayOnly="0"

                if ((colNode.Attributes["IsRequired"].Value=="1") && (colNode.Attributes["IsDisplayOnly"].Value=="0"))
                {
                    if (colNode.Attributes["Label"].Value.ToUpper() == "SECURECATEGORY")
                    {
                        reqSecureCategory.Enabled = true;
                        reqSecureCategory.ErrorMessage = colNode.Attributes["Caption"].Value;
                        reqSecureCategory.InitialValue = "-1~1005";
                    }
                }

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
        /// <summary>
        ///  To Fill dropdownlist data
        /// </summary>
        private void FillDropDownData()
        {
            XmlDataSource xdsSecure = new XmlDataSource();
            xdsSecure.EnableCaching = false;
            xdsSecure.Data = ViewState["returnXML"].ToString();
            xdsSecure.XPath = "/Root/bpeout/FormControls/SecureCategory/RowList/Rows";
            ddlSecureCategory.DataValueField = "DataValueField"; //listColumns[count].ToString() + "_TrxID"; //+ "~" + listColumns[count].ToString() + "_TrxType";
            ddlSecureCategory.DataTextField = "SecureCategory";  //listColumns[count].ToString();
            ddlSecureCategory.DataSource = xdsSecure;
            ddlSecureCategory.DataBind();
        }

        private void SecureSubmitEntries()
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(ViewState["returnXML"].ToString());

            string currBPGID = string.Empty;
            string CntrlValues = string.Empty;

            if (hdnFldSecureAction.Value.ToUpper().Trim() == "ADD")
            {
                currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']").Attributes["BPGID"].Value;
            }
            else if (hdnFldSecureAction.Value.ToUpper().Trim() == "MODIFY")
            {
                currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']").Attributes["BPGID"].Value;
            }
            else if (hdnFldSecureAction.Value.ToUpper().Trim() == "DELETE")
            {
                currBPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']").Attributes["BPGID"].Value;
            }

            //get new row
            CntrlValues = commonObjUI.GetNewRowForAtchSecrNote(pnlSecure, xDoc, hdnFldSecureAction.Value);


            //Reqxml
            string strReqXml = ActionRequestXML(hdnFldSecureAction.Value.ToString(), currBPGID, CntrlValues);

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
     
                SetUpdateRow(strOutXml); 
                //PreSelectDropDownControls();
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

        private string ActionRequestXML(string BPAction, string BPGID, string CntrlValues)
        {
            #region NLog
            logger.Info("This to get the action request XML with BPAction : " + BPAction + " and BPGID as : " + BPGID + " and CntrlValues : " + CntrlValues);
            #endregion

            string TreeNodeName = string.Empty;
            string reqxml = string.Empty;
            XmlDocument returnXML = new XmlDocument();
            returnXML.LoadXml(ViewState["returnXML"].ToString());

            TreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            if (BPAction.ToUpper().Trim() == "ADD")
            {
                XmlDocument xDocAddVendor = new XmlDocument();
                xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");
                XmlNode nodeNote = xDocAddVendor.CreateNode(XmlNodeType.Element, TreeNodeName, null);
                nodeBPINFO.AppendChild(nodeNote);

                XmlNode nodeRows = xDocAddVendor.CreateNode(XmlNodeType.Element, "RowList", null);
                nodeRows.InnerXml = CntrlValues;
                nodeNote.AppendChild(nodeRows);

                //XmlNode nodeRow = xDocAddVendor.CreateNode(XmlNodeType.Element, "Rows", null);
                //nodeRows.AppendChild(nodeRow);

                XmlNode nodeRow = xDocAddVendor.SelectSingleNode("Root/bpinfo/" + TreeNodeName + "/RowList/Rows");

                XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                attBPAction.Value = BPAction;
                nodeRow.Attributes.Append(attBPAction);

                //XmlAttribute attNote = xDocAddVendor.CreateAttribute("Note");
                //attNote.Value = txtNotes.Text.ToString();
                //nodeRow.Attributes.Append(attNote);

                //XmlAttribute attSharedUpdate = xDocAddVendor.CreateAttribute("SharedUpdate");
                //attSharedUpdate.Value = Shared;
                //nodeRow.Attributes.Append(attSharedUpdate);

                XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                nodeBPGID.InnerXml = BPGID;

                //Retriieving the BPEOut and ReportOut XML
                //string reqxml = nodeRoot.OuterXml;
                reqxml = xDocAddVendor.OuterXml;

                //Creating the Vendor node

            }

            if (BPAction.ToUpper().Trim() == "MODIFY")
            {

                XmlDocument xDocAddVendor = new XmlDocument();
                xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");

               // nodeBPINFO.InnerXml += hdnSectedRow.Value.ToString();

                nodeBPINFO.InnerXml += "<Secure><RowList>" + ViewState["UpdateRow"].ToString() + "</RowList></Secure>";

                XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                nodeBPGID.InnerXml = BPGID;

                XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Secure/RowList/Rows");//.Attributes["Note"];
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
                            att.Value = ddlSecureCategory.SelectedValue.ToString();
                            nodeRowlist.Attributes.Append(att);
                        }
                        else
                        {
                            attcolNote[attcolRow[i].Name.ToString()].InnerXml = attcolRow[i].Value.ToString();
                        }
                    }
                }

                //attcolNote["Note"].InnerXml = txtNotes.Text.ToString();
                //attcolNote["SharedUpdate"].InnerXml = Shared.ToString();

                XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                attBPAction.Value = BPAction;
                nodeRowlist.Attributes.Append(attBPAction);

                reqxml = xDocAddVendor.OuterXml;
            }

            if (BPAction.ToUpper().Trim() == "DELETE")
            {
                XmlDocument xDocAddVendor = new XmlDocument();
                xDocAddVendor.LoadXml(ViewState["BPEbpInfo"].ToString());

                XmlNode nodeBPINFO = xDocAddVendor.SelectSingleNode("Root/bpinfo");

                //nodeBPINFO.InnerXml += hdnSectedRow.Value.ToString();

                nodeBPINFO.InnerXml += "<Secure><RowList>"+ ViewState["UpdateRow"].ToString() +"</RowList></Secure>";


                XmlNode nodeBPGID = xDocAddVendor.SelectSingleNode("Root/bpinfo/BPGID");
                nodeBPGID.InnerXml = BPGID;


                XmlNode nodeRowlist = xDocAddVendor.SelectSingleNode("Root/bpinfo/Secure/RowList/Rows");

                XmlAttribute attBPAction = xDocAddVendor.CreateAttribute("BPAction");
                attBPAction.Value = BPAction;
                nodeRowlist.Attributes.Append(attBPAction);

                reqxml = xDocAddVendor.OuterXml;

            }

            return reqxml;
        }


        private void SetTreeNode()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(ViewState["returnXML"].ToString());

            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            {
                m_treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            }
        }

        private void SetUpdateRow(string ReturnXML)
        {
            //string treeNode = string.Empty;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(ReturnXML);

            //if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            //{
            //    treeNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            //}
            if(xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNode + "/RowList") != null)
            {
                trAdd.Visible = false;
                trDelete.Visible = true;
                trModify.Visible = true;

                imgbtnSecureAdd.Visible = false;
                imgbtnSecureDelete.Visible = true;
                imgbtnSecureModify.Visible = true;

                lblCreatedBy.Visible = true;
                lblCreatedByData.Visible = true;

                //SecureCategory_TrxID="1" SecureCategory_TrxType="1005" 
                XmlNode nodeRow = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNode + "/RowList/Rows");
                ViewState["UpdateRow"] = nodeRow.OuterXml.ToString();
                PreSelectDropDownControls(ViewState["UpdateRow"].ToString());
            }
            else
            {
                trAdd.Visible = true;
                trDelete.Visible = false;
                trModify.Visible = false;
                lblCreatedBy.Visible = false;
                lblCreatedByData.Visible = false;
                ddlSecureCategory.SelectedIndex = 0;

                imgbtnSecureAdd.Visible = true;
                imgbtnSecureDelete.Visible = false;
                imgbtnSecureModify.Visible = false;
            }
        }

        private void PreSelectDropDownControls(string UpdateRow)
        {
            string SecureCategory_TrxID = string.Empty;
            string SecureCategory_TrxType = string.Empty;

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(UpdateRow);

            XmlNode nodeRow = xDoc.SelectSingleNode("Rows");
            XmlAttributeCollection attRow = nodeRow.Attributes;

            string DatavalueField = attRow["SecureCategory_TrxID"].Value.ToString() + "~" + attRow["SecureCategory_TrxType"].Value.ToString();
            ddlSecureCategory.SelectedValue = DatavalueField;

            if (attRow["CreatedBy"] != null)
            {
                trCreatedBy.Visible = true;
                lblCreatedByData.Text = attRow["CreatedBy"].Value.ToString();
            }
        }


        private void BPCVisbility()
        {
            if (ViewState["returnXML"] != null)
            {
                XmlDataDocument xDoc = new XmlDataDocument();
                xDoc.LoadXml(ViewState["returnXML"].ToString());

                if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null))
                {
                    trAdd.Visible = false;
                    imgbtnSecureAdd.Visible = false;
                    
                }
                else if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']") == null))
                {
                    trModify.Visible = false;
                    imgbtnSecureModify.Visible = false;
                    
                }
                else if ((xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") == null))
                {
                    trDelete.Visible = false;
                    imgbtnSecureDelete.Visible = false;
                    
                }
            }
        }

        #endregion

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            SecureSubmitEntries();
            hdnFldSecureAction.Value = "";
        }

        protected void imgbtnSecureDelete_Click(object sender, ImageClickEventArgs e)
        {
            hdnFldSecureAction.Value = "Delete";
            SecureSubmitEntries();
            hdnFldSecureAction.Value = "";
            hdnSectedRow.Value = "";
            hdnRadIndex.Value = "";
        }
    }
}
