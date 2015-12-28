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
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
//using AjaxControlToolkit;
using LAjitDev;
using NLog;


namespace LAjitDev.UserControls
{
    public partial class ButtonsUserControl : System.Web.UI.UserControl
    {
        //LAjit_BO.CommonBO commonObjBO = new LAjit_BO.CommonBO();
        //LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        //LAjitDev.clsBranchUI objBranchUI = new clsBranchUI();
        //LAjitDev.clsReportsUI objReportsUI = new clsReportsUI();
        private CommonUI m_ObjCommonUI;
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public CommonUI ObjCommonUI
        {
            get
            {
                if (m_ObjCommonUI == null)
                {
                    return new CommonUI();
                }
                else
                {
                    return m_ObjCommonUI;
                }
            }
            set { m_ObjCommonUI = value; }
        }

        XmlDocument XDocUserInfo = new XmlDocument();

        protected GridViewControl ucGridViewControl;
        public GridViewControl UserCtrl
        {
            get { return ucGridViewControl; }
            set { ucGridViewControl = value; }
        }

        public String TreeNode
        {
            get { return (String)ViewState["m_treeNodeName"]; }
            set { ViewState["m_treeNodeName"] = value; }
        }

        public String RwToBeModified
        {
            get
            {
                if (hdnRwToBeModified != null)
                {
                    return hdnRwToBeModified.Value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (hdnRwToBeModified != null)
                {
                    hdnRwToBeModified.Value = value;
                }
                SetNoteAttachSecure();
            }
        }

        public String GVDataXml
        {
            get { return (String)ViewState["GVDataXml"]; }
            set { ViewState["GVDataXml"] = value; }
        }

        public String BranchXML
        {
            get { return hdnBranchXML.Value; }
            set { hdnBranchXML.Value = value; }
        }

        /// <summary>
        /// Gets the current action of the ButtonsUserControl.
        /// </summary>
        public string CurrentAction
        {
            get { return (hdnCurrAction != null) ? hdnCurrAction.Value : string.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //PreloadImageButtonImages();//moved to commonui.cs
                if (!string.IsNullOrEmpty(GVDataXml))
                {
                    UIVisibility();
                }
                if (hdnNeedToConfirmExit.Value == "")
                {
                    hdnNeedToConfirmExit.Value = "False";
                }
                hdnCnfmSbmt.Value = "False";
            }
            //lnkBtn.Click += new EventHandler(lnkBtn_Click);
        }

        /// <summary>
        /// The Close Click event of the Page's Iframe.(Will be called only if the calling process is a RefreshProcess.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkBtn_Click(object sender, EventArgs e)
        {
            //Request for BPOut and reload the current page.(ProcessRefresh functionality)
            CommonBO objBO = new CommonBO();
            XDocUserInfo = ObjCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml;

            //Have to pass Trx Id and trxType.
            XmlDocument xDocRow = new XmlDocument();
            xDocRow.LoadXml(this.RwToBeModified);
            string trxID = xDocRow.DocumentElement.Attributes["TrxID"].Value;
            string trxType = xDocRow.DocumentElement.Attributes["TrxType"].Value;
            ObjCommonUI.ReloadPage(trxID, trxType, strBPE, (Control)sender);
            //AjaxControlToolkit.CollapsiblePanelExtender cpeExpand = (AjaxControlToolkit.CollapsiblePanelExtender)ObjCommonUI.UpdatePanelContent.FindControl("cpeCPGV1");
            //bool coll = cpeExpand.Collapsed;
            //cpeExpand.Collapsed = true;
        }

        public void OnCloseIframeClick(object sender)
        {
            //Request for BPOut and reload the current page.(ProcessRefresh functionality)
            CommonBO objBO = new CommonBO();
            XDocUserInfo = ObjCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml;

            //Have to pass Trx Id and trxType.
            XmlDocument xDocRow = new XmlDocument();
            xDocRow.LoadXml(this.RwToBeModified);
            string trxID = xDocRow.DocumentElement.Attributes["TrxID"].Value;
            string trxType = xDocRow.DocumentElement.Attributes["TrxType"].Value;
            ObjCommonUI.ReloadPage(trxID, trxType, strBPE, (Control)sender);
            //AjaxControlToolkit.CollapsiblePanelExtender cpeExpand = (AjaxControlToolkit.CollapsiblePanelExtender)ObjCommonUI.UpdatePanelContent.FindControl("cpeCPGV1");
            //bool coll = cpeExpand.Collapsed;
            //cpeExpand.Collapsed = true;
        }

        private void SetNoteAttachSecure()
        {
            if ((RwToBeModified != null) && (RwToBeModified != ""))
            {
                string BPGID = string.Empty;
                string TrxID = string.Empty;
                string TrxType = string.Empty;

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(RwToBeModified.ToString());

                XmlNode nodeRow = xDoc.SelectSingleNode("Rows");
                XmlAttributeCollection attcolRow = nodeRow.Attributes;

                if (attcolRow["TrxID"] != null)
                {
                    TrxID = attcolRow["TrxID"].Value.ToString();
                }
                if (attcolRow["TrxType"] != null)
                {
                    TrxType = attcolRow["TrxType"].Value.ToString();
                }

                if (ObjCommonUI != null && !string.IsNullOrEmpty(ObjCommonUI.SessionLinkBPInfo))
                {
                    XmlDocument xDocInfo = new XmlDocument();
                    //xDocInfo.LoadXml(Session["BPINFO"].ToString());
                    xDocInfo.LoadXml(ObjCommonUI.SessionLinkBPInfo);
                    BPGID = xDocInfo.SelectSingleNode("bpinfo/BPGID").InnerXml.ToString();
                }
                //Note
                imgbtnNote.Attributes.Add("BPGID", BPGID);
                imgbtnNote.Attributes.Add("TrxID", TrxID);
                imgbtnNote.Attributes.Add("TrxType", TrxType);
                //Attachment
                imgbtnAttachment.Attributes.Add("BPGID", BPGID);
                imgbtnAttachment.Attributes.Add("TrxID", TrxID);
                imgbtnAttachment.Attributes.Add("TrxType", TrxType);
                //Security
                imgbtnSecure.Attributes.Add("BPGID", BPGID);
                imgbtnSecure.Attributes.Add("TrxID", TrxID);
                imgbtnSecure.Attributes.Add("TrxType", TrxType);

                hdnPrintInfo.Value = TrxID + "~" + TrxType;
            }
        }

        /// <summary>
        /// Setting the images for all image buttons
        /// </summary>
        public void PreloadImageButtonImages()
        {
            ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
            UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
            string cdnImagePath = Application["ImagesCDNPath"].ToString();

            imgbtnAdd.ImageUrl = cdnImagePath+ "Images/add-icon.png";
            imgbtnAdd.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/add-icon-over.png'");
            imgbtnAdd.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/add-icon.png'");

            imgbtnClone.ImageUrl = cdnImagePath+ "Images/add-clone.png";
            imgbtnClone.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/add-clone-over.png'");
            imgbtnClone.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/add-clone.png'");

            imgbtnModify.ImageUrl = cdnImagePath+ "Images/modify-icon.png";
            imgbtnModify.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/modify-icon-over.png'");
            imgbtnModify.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/modify-icon.png'");

            imgbtnDelete.ImageUrl = cdnImagePath+ "Images/delete_icon.png";
            imgbtnDelete.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/delete-icon-over.png'");
            imgbtnDelete.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/delete_icon.png'");

            imgbtnFind.ImageUrl = cdnImagePath+ "Images/find-icon.png";
            imgbtnFind.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/find-icon-over.png'");
            imgbtnFind.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/find-icon.png'");

            imgbtnPrint.ImageUrl = cdnImagePath+ "Images/print-icon.png";
            imgbtnPrint.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/print-icon-over.png'");
            imgbtnPrint.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/print-icon.png'");

            imgbtnAttachment.ImageUrl = cdnImagePath+ "Images/attachment-icon.png";
            imgbtnAttachment.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/attachment-icon-over.png'");
            imgbtnAttachment.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/attachment-icon.png'");

            imgbtnNote.ImageUrl = cdnImagePath+ "Images/footnote-icon.png";
            imgbtnNote.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/footnote-icon-over.png'");
            imgbtnNote.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/footnote-icon.png'");

            imgbtnSecure.ImageUrl = cdnImagePath+ "Images/security-icon.png";
            imgbtnSecure.Attributes.Add("onmouseover", "this.src='" + cdnImagePath + "Images/security-icon-over.png'");
            imgbtnSecure.Attributes.Add("onmouseout", "this.src='" + cdnImagePath + "Images/security-icon.png'");

            ImageButton imgBtnSubmit = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnSubmit");
            imgBtnSubmit.ImageUrl = cdnImagePath+ "Images/submit-but.png";
            imgBtnSubmit.Attributes.Add("attrSave", "");//To differentiate 'Add' and 'AutoSave' functionality. previously in SetButtonAttributes fn.

            ImageButton imgBtnCancel = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnCancel");
            imgBtnCancel.ImageUrl = cdnImagePath+ "Images/cancel-but.png";

            ImageButton imgBtnContinueAdd = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnContinueAdd");
            imgBtnContinueAdd.ImageUrl = cdnImagePath+ "Images/continue-add-but.png";
            imgBtnContinueAdd.Width = Unit.Pixel(100);
            imgBtnContinueAdd.ToolTip = "F-8";

            ImageButton imgBtnAddClone = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnAddClone");
            if (imgBtnAddClone != null)
            {
                imgBtnAddClone.ImageUrl = cdnImagePath+ "Images/add-clone-but.png";
                imgBtnAddClone.Width = Unit.Pixel(92);
            }

            //AjaxControlToolkit.CollapsiblePanelExtender cpeExpand = (AjaxControlToolkit.CollapsiblePanelExtender)cntrlUpdtPnl.FindControl("cpeCPGV1");
            //cpeExpand.CollapsedImage = Application["ImagesCDNPath"].ToString() + "/Images/plus-icon.png";
            //cpeExpand.ExpandedImage = "~/App_Themes/" + theme + "/Images/minus-icon.png";
            //cpeExpand.BehaviorID = "BIDMainGrid";

            ImageButton imgbtnNext = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnNext");
            if (imgbtnNext != null)
            {
                //imgbtnNext.ImageUrl = Application["ImagesCDNPath"].ToString() + "/images/arrow-right.gif";
                imgbtnNext.ImageUrl = cdnImagePath+ "Images/next-arrow.png";
            }
            ImageButton imgbtnPrevious = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnPrevious");
            if (imgbtnPrevious != null)
            {
                //imgbtnPrevious.ImageUrl = "~/App_Themes/" + theme + "/images/arrow-left.gif";
                imgbtnPrevious.ImageUrl = cdnImagePath+ "Images/prev-arrow.png";

            }
        }

        /// <summary>
        /// To Enable or Disable buttons in the left panel depending on xml.
        /// </summary>
        /// <param name="rxml">return xml for the BPGID</param>
        public void UIVisibility()
        {
            XmlDocument xDocRPGV = new XmlDocument();
            xDocRPGV.LoadXml(GVDataXml);

            XDocUserInfo = ObjCommonUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Add']") == null))
            {
                trImgbtnAdd.Visible = false;
                trImgbtnClone.Visible = false;
            }
            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Modify']") == null))
            {
                trImgbtnModify.Visible = false;
            }
            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Find']") == null) || this.Page.ToString().Contains("fullview"))
            {//Should not show 'Find' button in FullView page.
                trImgbtnFind.Visible = false;
            }
            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='Delete']") == null))
            {
                trImgbtnDelete.Visible = false;
            }
            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsSecured").InnerText != "1"))
            {
                trImgbtnSecure.Visible = false;
            }
            if ((XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='32']") == null)
            && (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsNoted").InnerText != "1"))
            {
                trImgbtnNote.Visible = false;
            }
            if ((XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@FormID='33']") == null)
            && (xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/GridHeading/GridExtendedColumns/IsAttached").InnerText != "1"))
            {
                trImgbtnAttachment.Visible = false;
            }
            if ((xDocRPGV.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree").Attributes["IsOkToPrint"].Value) != "1")
            {
                trImgbtnPrint.Visible = false;
            }
        }

        /// <summary>
        /// To show/hide trSOXApproval and trProcessLinks.
        /// <param name="cntrlUpdtPnl">UpdatePanel object to find controls.</param>
        /// <param name="Xdoc">Xdoc contains current GVDataXML.</param>
        /// <param name="mode">mode can be ISSUBMITCLICK/VIEWMODE to show the trProcessLinks.</param>
        /// </summary>
        public void HandleFormLinks(Panel pnlEntryForm, XmlDocument Xdoc)
        {
            XmlDocument xRowDoc = new XmlDocument();
            XmlNode rowNode = null;
            if (!string.IsNullOrEmpty(RwToBeModified))
            {
                xRowDoc.LoadXml(RwToBeModified);
                rowNode = xRowDoc.SelectSingleNode("Rows");
            }

            if (rowNode == null)
            {
                return;
            }

            LAjitControls.LAjitImageButton imgBtnIsApproved = (LAjitControls.LAjitImageButton)pnlEntryForm.FindControl("imgbtnIsApproved");
            bool AddScriptStatus = false;
            HtmlTableRow trSOX = (HtmlTableRow)pnlEntryForm.FindControl("trSoxApprovedStatus");
            if (trSOX == null)
            {
                return;
            }
            if ((Xdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess[@ID='SOXApproval']") != null))
            {
                if (rowNode.Attributes["IsApproved"] != null)
                {
                    if ((rowNode.Attributes["IsApproved"].Value.Trim() == "1") && (rowNode.Attributes["SoxApprovedStatus"].Value.Trim() == string.Empty))
                    {
                        trSOX.Visible = false;
                    }
                    else
                    {
                        trSOX.Visible = true;
                        //----------Handles only imgBtnIsApproved which is a parent control.
                        if (imgBtnIsApproved != null)
                        {
                            if (rowNode.Attributes["IsApproved"].Value == "0")
                            {
                                //Waiting for Approval
                                //imgBtnIsApproved.Visible = true;
                                imgBtnIsApproved.Attributes.Add("style", "DISPLAY: Block;");
                                imgBtnIsApproved.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";
                                AddScriptStatus = true;
                            }
                            else
                            {   //Approved If SoxApprovedStatus is empty hide other wise show.
                                if ((rowNode.Attributes["SoxApprovedStatus"].Value.Trim() != string.Empty) && (rowNode.Attributes["SoxApprovedStatus"].Value.Trim() != null))
                                {
                                    //imgBtnIsApproved.Visible = true;
                                    imgBtnIsApproved.Attributes.Add("style", "DISPLAY: Block;");
                                    imgBtnIsApproved.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/Approved.png";
                                    AddScriptStatus = true;
                                }
                                else
                                {
                                    //imgBtnIsApproved.Visible = false;
                                    imgBtnIsApproved.Attributes.Add("style", "DISPLAY: none;");
                                }
                            }

                            if (AddScriptStatus)
                            {
                                imgBtnIsApproved.OnClientClick = "javascript:return SoxApprovedStatus(" + rowNode.Attributes["IsApproved"].Value + ");";
                                imgBtnIsApproved.Attributes.Add("onmouseover", "javascript:SoxApprovalToolTipShow('" + rowNode.Attributes["SoxApprovedStatus"].Value + "','"
                                                                + rowNode.Attributes["IsApproved"].Value + "','" + imgBtnIsApproved.ClientID + "')");
                                //imgBtnIsApproved.Attributes.Add("onmouseout", "javascript:SoxApprovalToolTipHide('"+  imgBtnIsApproved.ClientID   +"');");
                            }
                        }//ends----------Handles only imgBtnIsApproved which is a parent control.
                    }
                }//IsApproved
            }//trSOX
            else
            {
                trSOX.Visible = false;
            }
        }

        /// <summary>
        /// To set the given row value(from BPInfo)in a viewstate variable and to tell whether the page is landed from the menupanel or any grid.
        /// </summary>
        /// <param name="currentBPValue">BPGID or BPInfo from the page</param>
        /// <returns>Returns a bool value to tell whether the page is landed from the menupanel or any grid.</returns>
        public bool SetGVData(string currentBPValue)
        {
            #region NLog
            logger.Info("To set the given row value(from BPInfo = " + currentBPValue + ")" + "in a viewstate variable and to tell whether the page is landed from the menupanel or any grid."); 
            #endregion

            //If string contains '&' symbol, it can't be loaded into xmldocument so below is the conversion to html.
            if (currentBPValue.Contains("&"))
            {
                currentBPValue = currentBPValue.Replace("&", "&amp;");
                //Session["BPINFO"] = currentBPValue;
                ObjCommonUI.SessionLinkBPInfo = currentBPValue;
            }
            Boolean gridExists = false;
            if (currentBPValue != string.Empty)
            {
                XmlDocument xDocBPInfo = new XmlDocument();
                xDocBPInfo.LoadXml(currentBPValue);
                XmlNode noderwlst = xDocBPInfo.SelectSingleNode("bpinfo//RowList");
                XmlNode nodeCallingObj = xDocBPInfo.SelectSingleNode("bpinfo//CallingObject");
                if (noderwlst != null)
                {
                    if (noderwlst.InnerXml != null)
                    {
                        //RwToBeModified = noderwlst.InnerXml;
                    }
                }
                else
                {
                    gridExists = true;
                }
                if ((nodeCallingObj != null) || (noderwlst != null && nodeCallingObj != null))
                {
                    gridExists = true;
                }
            }
            return gridExists;
        }

        /// <summary>
        /// Method caller for InitialiseBranchObjects in CommonUI
        /// </summary>
        public void InitialiseBranchObjects(object sender)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVDataXml);
            XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            bool isChildGridPresent = false;
            if (nodeBranches != null)
            {
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    XmlAttribute attrCntrlType = nodeBranch.Attributes["ControlType"];
                    if (attrCntrlType != null && attrCntrlType.Value.Trim() == "GView")
                    {
                        isChildGridPresent = true;
                        break;
                    }
                }
            }
            //Run this method only for pages containing a child gridview
            //if (currentPage.Contains("jobbudget") || currentPage.Contains("totalengine") ||
            //    currentPage.Contains("fiscalyearperiod") || currentPage.Contains("journal"))
            if (isChildGridPresent)
            {
                ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
                UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");
                Panel pnlEntryForm = (Panel)cntrlUpdtPnl.FindControl("pnlEntryForm");
                ObjCommonUI.GridViewUserControl = ucGridViewControl;
                ObjCommonUI.ButtonsUserControl = this;
                ObjCommonUI.InitialiseBranchObjects(xDoc, pnlEntryForm);

                ChildGridView CGVUC = (ChildGridView)pnlEntryForm.FindControl("CGVUC");
                if (CGVUC != null)
                {
                    CGVUC.InitialiseBranchGrid(xDoc, (Control)sender);
                }
            }
        }

        /// <summary>
        /// Common Update method for Add, Modify, Delete, Find to update ViewStateXML.
        /// </summary>
        /// <param name="Action">Current Action.</param>
        /// <param name="ReqXMLTrxID">TrxID For Delete operation.</param>
        /// <param name="bpout">OutXML from the respective Action.</param>
        public void UpdateViewStateXml(String Action, string ReqXMLTrxID, string bpout)
        {
            #region NLog
            logger.Info("Common Update method for Add, Modify, Delete, Find to update ViewStateXML where action :" + Action + " and Request XML Trx Id is :" + ReqXMLTrxID + " and  bpout " ); 
            #endregion

            //Modifying the Viewstate RetXML which is used for GV Binding 
            XmlDocument returnXML = new XmlDocument();
            returnXML.LoadXml(bpout);

            //success messge
            XmlNode nodeMsg = returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            XmlNode nodeMsgStatus = returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Status");

            //success message2 updated on 26/10/09
            XmlNode nodeMsg2 = null;
            if (returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Message[2]") != null)
            {
                nodeMsg2 = returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Message[2]");
            }
           



            //Finding Pagelevel Update Panel
            ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Page.Master.FindControl("cphPageContents");
            UpdatePanel cntrlUpdtPnl = (UpdatePanel)cntPlaceHolder.FindControl("updtPnlContent");

            ImageButton imgBtnSubmit = (ImageButton)cntrlUpdtPnl.FindControl("imgbtnSubmit");
            if (imgBtnSubmit.Attributes["attrSave"].ToUpper().Trim() == "SAVE")
            {
                Action = "Save";
            }

            Label lblmsg = (Label)cntrlUpdtPnl.FindControl("lblmsg");
            lblmsg.Attributes.Add("style", "DISPLAY: Block;");

            if (nodeMsgStatus.InnerText == "Success")
            {
                XmlDocument Xdoc = new XmlDocument();
                Xdoc.LoadXml(GVDataXml.ToString());

                if (Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList") == null)
                {
                    //Creating the Row List node
                    XmlNode nodeRowList = Xdoc.CreateNode(XmlNodeType.Element, "RowList", null);

                    //Appending Row List Node
                    Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode).AppendChild(nodeRowList);
                }
                if (Action.ToUpper().Trim() == "FIND" || Action.ToUpper().Trim() == "PREVIOUSPAGELOAD")
                {
                    XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList");
                    nodeRowList.RemoveAll();
                    XmlNode nodeResRowList = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList");
                    nodeRowList.InnerXml += nodeResRowList.InnerXml;

                    //Updating the Total Page Size
                    XmlNode nodeNewRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList");
                    if (nodeNewRowList != null)
                    {
                        XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/Gridresults");
                        nodeGridResults.RemoveAll();
                        XmlNode nodeResGridResults = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/Gridresults");
                        nodeGridResults.InnerXml += nodeResGridResults.InnerXml;
                    }
                    GVDataXml = Xdoc.OuterXml;
                }
                else
                {
                    XmlNode nodeResRow = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList/Rows");

                    //TrxID of the Updated record
                    string trxID = string.Empty;
                    if (Action.ToUpper().Trim() == "DELETE")
                    {
                        trxID = ReqXMLTrxID;
                    }
                    else if (Action.ToUpper().Trim() == "SAVE")
                    {
                        //Do nothing
                    }
                    else
                    {
                        trxID = nodeResRow.Attributes["TrxID"].Value;
                    }

                    if (trxID != string.Empty)
                    {
                        XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList");
                        if ((Action.ToUpper().Trim() == "DELETE") || (Action.ToUpper().Trim() == "MODIFY") || (Action.ToUpper().Trim() == "SOXAPPROVAL"))
                        {
                            XmlNode nodeRow = nodeRowList.SelectSingleNode("Rows[@TrxID='" + trxID + "']");
                            nodeRowList.RemoveChild(nodeRow);
                            if (Action.ToUpper().Trim() == "MODIFY" || Action.ToUpper().Trim() == "SOXAPPROVAL")
                            {
                                nodeRowList.InnerXml += nodeResRow.OuterXml;
                            }
                            else if (Action.ToUpper().Trim() == "DELETE")
                            {
                                //Updating total Page Size
                                XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/Gridresults");
                                int newPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value) - 1;
                                nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value = Convert.ToString(newPageSize);
                                int newCurrentPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value) - 1;
                                nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value = Convert.ToString(newCurrentPageSize);
                            }
                        }
                        else if (Action.ToUpper().Trim() == "ADD")
                        {
                            //Creating the AddedRow node
                            nodeRowList.InnerXml += nodeResRow.OuterXml;
                            ////Updating total Page Size
                            XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/Gridresults");
                            int newPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value) + 1;
                            nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value = Convert.ToString(newPageSize);
                            int newCurrentPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value) + 1;
                            nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value = Convert.ToString(newCurrentPageSize);
                        }
                        //Updating the GV XML
                        GVDataXml = Xdoc.OuterXml;
                    }
                    if (returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList") != null)
                    {
                        RwToBeModified = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + TreeNode + "/RowList").InnerXml.ToString();
                    }
                }
            }
            //Need to handle in case of Multiple Error Labels
            //Displaying the message in the label
            if (Action.ToUpper().Trim() != "PREVIOUSPAGELOAD")
            {
                if (Action.ToUpper().Trim() == "SAVE")
                {
                    //lblmsg.Visible = false;
                    //lblConfirm.Visible = true;
                    lblmsg.Text = "This record is automatically saved";
                }
                else
                {
                    if (nodeMsgStatus != null && nodeMsg != null)
                    {
                       //Check Second message exist or not
                        if (nodeMsg2 != null)
                        { 
                            //Two Messages
                            lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText + "<br>" + nodeMsg2.SelectSingleNode("Label").InnerText;
                        }
                        else
                        {
                            //Single Messsage
                            lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else if (nodeMsgStatus != null)
                    {
                        lblmsg.Text = nodeMsgStatus.InnerText;
                    }
                }
                lblmsg.Page.MaintainScrollPositionOnPostBack = true;
                if (nodeMsgStatus != null)
                {
                    if (nodeMsgStatus.InnerText == "Error")
                    {
                        if (nodeMsg != null)
                        {
                            if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                            {   
                                //Check Second message exist or not
                                if (nodeMsg2 != null)
                                {   
                                    // Display Two messages
                                    lblmsg.Text = nodeMsg.SelectSingleNode("OtherInfo").InnerText + ".<br>"  + nodeMsg2.SelectSingleNode("OtherInfo").InnerText +".";
                                }
                                else
                                {
                                    //lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                                    lblmsg.Text = nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                                }
                            }
                            //Set Focus
                            lblmsg.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Note Attachements Secure based on available data change the image style
        /// </summary>
        public void NoteAttachSecurePicChange(string Action)
        {
            #region NLog
            logger.Info("Note Attachements Secure based on available data change the image style where action is : "+Action); 
            #endregion

            if (Action.ToUpper().Trim() == "SELECT")
            {
                if ((RwToBeModified != string.Empty) && (RwToBeModified != null))
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(RwToBeModified);
                    XmlNode xnode = xdoc.SelectSingleNode("Rows");

                    if (xnode.Attributes["Notes"] != null)
                    {
                        if (xnode.Attributes["Notes"].Value != string.Empty)
                        {
                            if ((bool)imgbtnNote.Enabled)
                            {
                                imgbtnNote.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-data.png";
                                imgbtnNote.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-data-over.png'");
                                imgbtnNote.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-data.png'");

                            }
                        }
                        else
                        {
                            imgbtnNote.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png";
                            imgbtnNote.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-over.png'");
                            imgbtnNote.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png'");
                        }
                    }

                    if (xnode.Attributes["Attachments"] != null)
                    {
                        if (xnode.Attributes["Attachments"].Value != string.Empty)
                        {
                            if ((bool)imgbtnAttachment.Enabled)
                            {
                                imgbtnAttachment.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-data.png";
                                imgbtnAttachment.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-data-over.png'");
                                imgbtnAttachment.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-data.png'");

                            }
                        }
                        else
                        {
                            imgbtnAttachment.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png";
                            imgbtnAttachment.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-over.png'");
                            imgbtnAttachment.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png'");
                        }
                    }

                    if (xnode.Attributes["SecuredBy"] != null)
                    {
                        if (xnode.Attributes["SecuredBy"].Value != string.Empty)
                        {
                            if ((bool)imgbtnSecure.Enabled)
                            {
                                imgbtnSecure.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/security-icon-data.png";
                                imgbtnSecure.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon-data-over.png'");
                                imgbtnSecure.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon-data.png'");
                            }
                        }
                        else
                        {
                            imgbtnSecure.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/security-icon.png";
                            imgbtnSecure.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon-over.png'");
                            imgbtnSecure.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon.png'");
                        }
                    }
                }
            }
            else
            {
                //Preload previous images
                if (imgbtnAttachment != null)
                {
                    if ((bool)imgbtnAttachment.Enabled)
                    {
                        imgbtnAttachment.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png";
                        imgbtnAttachment.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon-over.png'");
                        imgbtnAttachment.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/attachment-icon.png'");
                    }
                }

                if (imgbtnNote != null)
                {
                    if ((bool)imgbtnNote.Enabled)
                    {
                        imgbtnNote.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png";
                        imgbtnNote.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon-over.png'");
                        imgbtnNote.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/footnote-icon.png'");
                    }
                }
                if (imgbtnSecure != null)
                {
                    if ((bool)imgbtnSecure.Enabled)
                    {
                        imgbtnSecure.ImageUrl =  Application["ImagesCDNPath"].ToString() + "Images/security-icon.png";
                        imgbtnSecure.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon-over.png'");
                        imgbtnSecure.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/security-icon.png'");
                    }
                }
            }
        }



    }
}