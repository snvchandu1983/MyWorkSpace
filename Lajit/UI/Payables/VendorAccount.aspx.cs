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

namespace LAjitDev.Payables
{
    public partial class VendorAccount : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AddScriptReferences();
                hdnEntryJob.Value = commonObjUI.GetDropDownData("EntryJob", "COA");
                hdnEntryAccount.Value = commonObjUI.GetDropDownData("EntryAccount", "COA");
                ddlEntryJob.Attributes.Add("onchange", "javascript:FillSingLevelDropDowns('" + ddlEntryJob.ClientID + "','" + ddlEntryAccount.ClientID + "','" + hdnEntryJob.ClientID + "','" + hdnEntryAccount.ClientID + "');");
            }
        }
      

        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //CascadingDropDowns.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "CascadingDropDowns.js");
            Page.Header.Controls.Add(hgcScript1);
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

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }
    }
}