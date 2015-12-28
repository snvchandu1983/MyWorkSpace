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
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Web.Script.Services;

namespace LAjitDev.Financials
{
    public partial class Journals : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;

            PreloadImages();

            //Calculator DropDown for the Field Control Total
            if (!Page.IsPostBack)
            {
               // txtControlTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
              //  txtControlTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                //To Show Decimals
                txtControlTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            //else
            //{
            //    txtControlTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            //}
        }

        /*  protected void Page_UnLoad(object sender, EventArgs e)
          {
             // string action = imgbtnSubmit.AlternateText.ToUpper();

              //if ((imgbtnSubmit.AlternateText.ToUpper() == "ADD") || (imgbtnSubmit.AlternateText.ToUpper() == "MODIFY"))
              //{
              //    AddScriptEvent();
              //}
            
          }

          protected void Page_PreInit(object sender, EventArgs e)
          { 
        
        
          }

          protected void Page_Init(object sender, EventArgs e)
          { 
        
        
          }

          protected void Page_InitComplete(object sender, EventArgs e)
          {


          }

          protected void Page_PreLoad(object sender, EventArgs e)
          {


          }

          protected void Page_LoadComplete(object sender, EventArgs e)
          {


          }*/

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //HiddenField hdnCurrAction = (HiddenField)BtnsUC.FindControl("hdnCurrAction");
            //if ((hdnCurrAction.Value.ToUpper() == "ADD") || (hdnCurrAction.Value.ToUpper() == "MODIFY"))
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
            //    //imgbtnSubmit.OnClientClick = "";
            //    imgbtnSubmit.OnClientClick = "javascript:ValidateControls();";
            //    imgbtnContinueAdd.OnClientClick = "javascript:ValidateControls();";
            //    imgbtnAddClone.OnClientClick = "javascript:ValidateControls();";
            //    //   imgbtnSubmit.Attributes.Remove("onclick");
            //}
        }

        /*  protected void Page_SaveStateComplete(object sender, EventArgs e)
          {


          }

          protected void Page_Render(object sender, EventArgs e)
          {


          }*/

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);

            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);

            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);           

            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
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
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);

            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }

        #region Private Methods

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

        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;

            if ((txtRevDate_JournalDoc.Text != string.Empty) || (txtStartDate_JournalDoc.Text != string.Empty))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }


            if ((!ddlSelectBatch_JournalDoc.SelectedValue.Contains("-1")) || (chkRecurEntry_JournalDoc.Checked) || (chkRevEntry_JournalDoc.Checked))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
        }
        #endregion
    }
}