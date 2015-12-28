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
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using LAjit_BO;
using LAjitControls;

namespace LAjitDev.Payables
{
    public partial class APInvoice : Classes.BasePage
    {
        LAjit_BO.CommonBO commonObjBO = new LAjit_BO.CommonBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;
            PreloadImages();
            if (!Page.IsPostBack)
            {
                //Autofill vendor textbox on double click open new entry and link to view
                AddAutoFillScript();

                //Calculator 
                //txtControlTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
                //txtControlTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                //To Show Decimals
                txtControlTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            //else
            //{
            //    txtControlTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //htcCPGV1.Width = "245";


            //if ((imgbtnSubmit.AlternateText.ToUpper() == "ADD") || (imgbtnSubmit.AlternateText.ToUpper() == "MODIFY"))
            //{
            if (Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != null && Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != string.Empty)
            {
                if (!Page.IsPostBack)
                {
                    AddScriptEvent();
                }
            }
            //}
            //else
            //{
            //    imgbtnSubmit.OnClientClick = "";
            //}
            //Convert label into hyperlink based on available  autofillBPGID1
            /*bool status = AutofillBPGID1Status();
            if (status)
            {
                string CompanyID=Classes.AutoFill.GetLoggedInCompanyID();
                string CacheName = CompanyID + txtAutoFillVendor_JournalDoc.MapXML.ToString();
                lblAutoFillVendor_JournalDoc.Text = "<a href=javascript:OnAutoFillClick('"+CacheName+"','"+txtAutoFillVendor_JournalDoc.MapXML.ToString()+"');>"+lblAutoFillVendor_JournalDoc.Text + "</a>";
            }*/
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;

            if ((txtAutoFillVendor_JournalDoc.Text != string.Empty) || (txtCheckNumber_JournalDoc.Text != string.Empty) || (txtPaymentDate_JournalDoc.Text != string.Empty))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }


            if ((!ddlSelectAPInvoiceType_JournalDoc.SelectedValue.Contains("-1")) || (!ddlSelectPCVendor_JournalDoc.SelectedValue.Contains("-1")) || (!ddlVendPayTerm_JournalDoc.SelectedValue.Contains("-1")) || (!ddlSelectCheckGroup_JournalDoc.SelectedValue.Contains("-1")) || (!ddlEntryBank_JournalDoc.SelectedValue.Contains("-1")) || (!ddlSelectBatch_JournalDoc.SelectedValue.Contains("-1")))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

        }

        private void AddScriptEvent()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
            int columnIndex = 0;
            if ((bool)CGVUC.HideSelectColumn)
            {
                columnIndex = 0;
            }
            else
            {
                columnIndex = 1;
            }
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/AccountingItem/GridHeading/Columns");
            if (nodeColumns != null)
            {
                foreach (XmlNode colnode in nodeColumns.ChildNodes)
                {
                    if ((colnode.Attributes["FullViewLength"].Value.Trim() != "0"))
                    {
                        //Collecting all the columns which have IsRequired property.

                        if (colnode.Attributes["IsSummed"] != null)
                        {
                            if (colnode.Attributes["IsSummed"].Value == "1")
                            {
                                if (colnode.Attributes["BalanceMethod"] != null)
                                {
                                    if (colnode.Attributes["BalanceMethod"].Value.Trim() != "0")
                                    {
                                        //columnIndex
                                        //imgbtnSubmit.Attributes.Add("onclick", "javascript:return ControlsTotal('" + grdVwAccountingItem.ClientID.ToString() + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');");
                                        string JScript = "javascript:return ControlsTotal('" + CGVUC.ClientID + "_grdVwBranch" + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');";
                                        //OnClientClick="if(Page_ClientValidate()) {here call your Js function...} else {return false;}"
                                        imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                        imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                        imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                    }
                                }
                            }
                        }
                        columnIndex = columnIndex + 1;
                    }
                }
            }
            if (imgbtnSubmit.OnClientClick == string.Empty)
            {
                imgbtnSubmit.OnClientClick = "ValidateControls()";
                imgbtnContinueAdd.OnClientClick = "ValidateControls()";
                imgbtnAddClone.OnClientClick = "ValidateControls()";
            }

        }

        private void AddAutoFillScript()
        {
            if (commonObjUI.ButtonsUserControl.GVDataXml != null)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml.ToString());
                string cdnImagesPath = Convert.ToString(Application["ImagesCDNPath"]);
                
                XmlNode nodeProc = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='AutofillBPGID1']");
                if (nodeProc != null)
                {
                    if ((nodeProc.Attributes["BPGID"] != null) && (nodeProc.Attributes["PageInfo"] != null))
                    {
                        string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                        string pageInfo = nodeProc.Attributes["PageInfo"].Value;
                        string IsPopUp = "1";

                        //Textbox double click event
                        //txtAutoFillVendor_JournalDoc.Attributes.Add("ondblclick", "javascript:ShowAutoFillPage('" + currentBPGID + "','" + pageInfo + "','" + IsPopUp + "','AutoFillVendor',this)");
                       
                        //Add double click event to icon at right corner
                       // string themeName = Convert.ToString(HttpContext.Current.Session["MyTheme"]);
                        txtAutoFillVendor_JournalDoc.Attributes.Add("ShowIcon", "true");
                        txtAutoFillVendor_JournalDoc.Attributes.Add("IconAlign", "Right");
                        txtAutoFillVendor_JournalDoc.Attributes.Add("MaintainWidth", "true");
                        txtAutoFillVendor_JournalDoc.Attributes.Add("IconOnClick", "javascript:ShowAutoFillPage('" + currentBPGID + "','" + pageInfo + "','" + IsPopUp + "','AutoFillVendor','" + txtAutoFillVendor_JournalDoc.ClientID + "');");
                        txtAutoFillVendor_JournalDoc.Attributes.Add("IconAlternateText", "Click-Add");
                        txtAutoFillVendor_JournalDoc.Attributes.Add("IconPath",  cdnImagesPath + "images/row_add.gif");

                        //Label html anchor to view vendor details
                        //string CompanyID = Classes.AutoFill.GetLoggedInCompanyID();
                        //string CacheName = CompanyID + txtAutoFillVendor_JournalDoc.MapXML.ToString();
                        lblAutoFillVendor_JournalDoc.Text = "<a href=javascript:ValidateAndRedirect('" + currentBPGID + "','" + pageInfo + "','ctl00_cphPageContents_BtnsUC_hdnAutoFillBPEINFO','AutofillBPGID1','1','" + txtAutoFillVendor_JournalDoc.MapXML.ToString() + "','" + txtAutoFillVendor_JournalDoc.MapBranchNode.ToString() + "','" + txtAutoFillVendor_JournalDoc.ClientID.ToString() + "');>" + lblAutoFillVendor_JournalDoc.Text + "</a>";
                    }

                }
            }
        }

        //Customer Select & Vendor Select
        /*protected void imgBtnAutoFillVendor_JournalDoc_OnClick(object sender, ImageClickEventArgs e)
        {
            commonObjUI.imgBPCIcon_Click(sender, e);
        }

        protected void lnkBtnAutoFillVendor_JournalDoc_OnClick(object sender, EventArgs e)
        {
            commonObjUI.BPCIconLinkClick(sender, e);
        }*/
    }
}