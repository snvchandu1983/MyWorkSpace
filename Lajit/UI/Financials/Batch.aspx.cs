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

namespace LAjitDev.Financials
{
    public partial class Batch : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtControlTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
                txtControlTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                //To Show Decimals
                txtControlTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            else
            {
                txtControlTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            }
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

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }
    }
}