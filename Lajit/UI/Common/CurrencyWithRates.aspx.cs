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

namespace LAjitDev.Common
{
    public partial class CurrencyWithRates : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PreloadImages();
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

        }




        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("CurrencyRate", CurrencyRateEntered(), pnlEntryForm);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("CurrencyRate", CurrencyRateEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }

        private bool CurrencyRateEntered()
        {
            bool currencyRateEntered = false;
            if ((txtTenantCurrency_CurrencyRate.Text != string.Empty) || (txtConversionRate_CurrencyRate.Text != string.Empty) || (txtEffectiveDate_CurrencyRate.Text != string.Empty))
            {
                currencyRateEntered = true;
                return currencyRateEntered;
            }
            return currencyRateEntered;
        }             

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            objBranchUI.saveBranchUI("CurrencyRate", CurrencyRateEntered(), pnlEntryForm);
            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("CurrencyRate", CurrencyRateEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }
    }
}