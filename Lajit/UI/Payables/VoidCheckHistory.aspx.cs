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
    public partial class VoidCheckHistory : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblCheckMessage_JournalDoc_Value.CssClass = "mbodybig";
                //Intalizing collapase images
               // cpeCPGV2.ExpandedImage = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/minus-icon.png";
                //cpeCPGV2.CollapsedImage = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/plus-icon.png";


                //Add onchange attributes to all the objects which are enabled when not in Moidify mode.
                string onChangeScript =  "javascript:UpdateBPCLinkXML(this,'JournalDoc','SelectAPInvoiceType');"
                        + "DoChildValidations('" + ddlSelectAPInvoiceType_JournalDoc.ClientID 
                        + "','" + lblSelectBatch_JournalDoc.ClientID
                        + "','" + ddlSelectBatch_JournalDoc.ClientID
                        + "','" + txtVoidDate_JournalDoc.ClientID
                        + "','" + chkVoidOrigDate_JournalDoc.ClientID + "')";
                ddlSelectAPInvoiceType_JournalDoc.Attributes.Add("onchange", onChangeScript);
                txtVoidDate_JournalDoc.Attributes.Add("onchange", "javascript:UpdateBPCLinkXML(this,'JournalDoc','VoidDate');");
                chkVoidOrigDate_JournalDoc.Attributes.Add("onchange", "javascript:UpdateBPCLinkXML(this,'JournalDoc','VoidOrigDate');");
                ddlSelectBatch_JournalDoc.Attributes.Add("onchange", "javascript:UpdateBPCLinkXML(this,'JournalDoc','SelectBatch');");
                ddlSelectBatch_JournalDoc.Style.Add("display", "none");
                lblSelectBatch_JournalDoc.Style.Add("display", "none");

                chkVoidOrigDate_JournalDoc.Attributes.Add("id", "chkVoidOrigDate");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Basepage height and width are reduced based on popup.
            // IE version bottom is not visible toovercome this following height is updated
            int entryFormHeight = 511;
            pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
            tblEntryForm.Style["height"] = pnlEntryForm.Height.ToString();
            pnlCPGV2Title.Width = pnlEntryForm.Width;


            // Process links to move farward added dummy table cell
            if (pnlBPCContainer != null)
            {
                TableRow tr = (TableRow)pnlBPCContainer.FindControl("trDynamicProcessLinks");
                if (tr != null)
                {
                    TableCell tdDummy = new TableCell();
                    tdDummy.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    tr.Controls.Add(tdDummy);
                }
            }

            //enable the controls

            ddlSelectAPInvoiceType_JournalDoc.Enabled = true;
            ddlSelectAPInvoiceType_JournalDoc.BackColor = System.Drawing.Color.Empty;
            txtVoidDate_JournalDoc.Enabled = true;
            txtVoidDate_JournalDoc.BackColor = System.Drawing.Color.Empty;
           // ceVoidDate_JournalDoc.Enabled = true;
            chkVoidOrigDate_JournalDoc.Enabled = true;
            chkVoidOrigDate_JournalDoc.BackColor = System.Drawing.Color.Empty;
            ddlSelectBatch_JournalDoc.Enabled = true;
            ddlSelectBatch_JournalDoc.BackColor = System.Drawing.Color.Empty;
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

        protected void lnkBtnAddNewRow_Click(object sender, EventArgs e)
        {
            commonObjUI.AddNewRowToGrid(grdVwAccountingItem);
        }

        #region Private Methods
        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;

            if ((txtCheckNumber_JournalDoc.Text != string.Empty) || (txtPaymentDate_JournalDoc.Text != string.Empty) || (txtPOReference_JournalDoc.Text != string.Empty) || (txtVendor_JournalDoc.Text != string.Empty))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }


            if ((!ddlSelectAPInvoiceType_JournalDoc.SelectedValue.Contains("-1")) || (!ddlEntryBank_JournalDoc.SelectedValue.Contains("-1")))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
        }


        #endregion
    }
}