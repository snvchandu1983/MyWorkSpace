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

namespace LAjitDev.Financials
{
    public partial class JobBudget : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Basepage height and width are reduced based on popup.
            // IE version bottom is not visible toovercome this following height is updated
           int entryFormHeight = 511;
           pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
            //tblEntryForm.Style["height"] = pnlEntryForm.Height.ToString();
            //pnlCPGV2Title.Width = pnlEntryForm.Width;
        }
        
        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
            //grdVwBudgetTotal.Attributes["CheckBoxSel"] = "";
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }      

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }       
    }
}