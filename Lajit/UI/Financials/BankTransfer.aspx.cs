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
using LAjit_BO;

namespace LAjitDev.Financials
{
    public partial class BankTransfer : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //To Show Decimals
                txtDepTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            else
            {
                txtDepTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            }
            PreloadImages();
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

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

        #region Private Methods

        /// <summary>
        /// Adding Javascript validation for amount.
        /// </summary>
        private void AddScriptEvent()
        {
            if (imgbtnSubmit.OnClientClick == string.Empty)
            {
                imgbtnSubmit.OnClientClick = "ValidateControls()";
                imgbtnContinueAdd.OnClientClick = "ValidateControls()";
                imgbtnAddClone.OnClientClick = "ValidateControls()";
            }
        }

        /// <summary>
        /// journaldoc branch data entry is available or not
        /// </summary>
        /// <returns></returns>
        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;

            if ((!ddlSelectBatch_JournalDoc.SelectedValue.Contains("-1")) || (!ddlEntryBank2_JournalDoc.SelectedValue.Contains("-1")) || (!ddlEntryBank_JournalDoc.SelectedValue.Contains("-1")))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
        }

        #endregion
    }
}
