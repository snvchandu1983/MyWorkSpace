using System;
using System.Web.UI;
using System.Xml;

namespace LAjitDev.Financials
{
    public partial class Pettycashjournal : Classes.BasePage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;
            
            PreloadImages();

            if (!Page.IsPostBack)
            {
                //txtEnvelopeTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
                //txtEnvelopeTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                ////To Show Decimals
                txtEnvelopeTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            //else
            //{
            //    txtEnvelopeTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != null && Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != string.Empty)
            {
                if (!Page.IsPostBack)
                {
                    AddScriptEvent();
                }
            }
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

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);            
        }

        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;
            if ((!ddlSelectBatch_JournalDoc.SelectedValue.Contains("-1")) || (!ddlSelectPCVendor_JournalDoc.SelectedValue.Contains("-1")))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
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
    }
}