using System;
using System.Web.UI;
using System.Drawing;
using System.Xml;

namespace LAjitDev.Common
{
    public partial class Currency : Classes.BasePage
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
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
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