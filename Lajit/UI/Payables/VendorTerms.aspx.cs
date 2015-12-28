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
//using LAjitControls;

namespace LAjitDev.Payables
{
    public partial class VendorTerms : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
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