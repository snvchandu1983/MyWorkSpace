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
using LAjitControls;
using LAjit_BO;
using System.Reflection;
using System.IO;

namespace LAjitDev
{
    public partial class JournalsHyb : Classes.PCBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;

            if (!Page.IsPostBack && commonObjUI.ButtonsUserControl != null
                && commonObjUI.ButtonsUserControl.GVDataXml != null && commonObjUI.ButtonsUserControl.GVDataXml.Length > 0)
            {
                //In case of JournalHyb page the Branch node name does not remain constant. So read the OUT XMl and assign it from there.
                XmlDocument xDocOut = new XmlDocument();
                xDocOut.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches == null)
                {
                    return;
                }
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    if (nodeBranch.Attributes["ControlType"] != null)
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        //Check for ListBox or GridView
                        if (nodeBranch.Attributes["ControlType"].Value == "GView")
                        {
                            CGVUC.BranchNodeName = nodeBranch.FirstChild.InnerText;
                        }
                    }
                }
            }
            ////Calculator DropDown for the Field Control Total
            //if (!Page.IsPostBack)
            //{
            //    txtControlTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
            //    txtControlTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
            //    //To Show Decimals
            //    txtControlTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            //}
            //else
            //{
            //    txtControlTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if ((imgbtnSubmit.AlternateText.ToUpper() == "ADD") || (imgbtnSubmit.AlternateText.ToUpper() == "MODIFY"))
            //{
            //  AddScriptEvent();
            //}
            //else
            //{
            //    imgbtnSubmit.OnClientClick = "";
            //   imgbtnSubmit.Attributes.Remove("onclick");
            //}
        }

        /*  protected void Page_SaveStateComplete(object sender, EventArgs e)
          {


          }

          protected void Page_Render(object sender, EventArgs e)
          {


          }*/

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);

            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);

            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);


            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            string s = "javascript:Hidden();"; // Openframe() is JavaScript Function,Passing page and frame as parameters.
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hidden", s, true);

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

        protected void lnkBtnAddNewRow_Click(object sender, EventArgs e)
        {
            //commonObjUI.AddNewRowToGrid(grdVwAccountingItem);
        }
        #region Private Methods
        private void AddScriptEvent()
        {
            //XmlDocument xDoc = new XmlDocument();

            //xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml.ToString());

            //int columnIndex = 0;

            //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/AccountingItem/GridHeading/Columns");

            //if (nodeColumns != null)
            //{
            //    foreach (XmlNode colnode in nodeColumns.ChildNodes)
            //    {
            //        if ((colnode.Attributes["FullViewLength"].Value.Trim() != "0"))
            //        {
            //            //Collecting all the columns which have IsRequired property.
            //            columnIndex = columnIndex + 1;
            //            if (colnode.Attributes["IsSummed"] != null)
            //            {
            //                if (colnode.Attributes["IsSummed"].Value == "1")
            //                {
            //                    if (colnode.Attributes["BalanceMethod"] != null)
            //                    {
            //                        if (colnode.Attributes["BalanceMethod"].Value.Trim() != "0")
            //                        {
            //                            //columnIndex
            //                            //imgbtnSubmit.Attributes.Add("onclick", "javascript:return ControlsTotal('" + grdVwAccountingItem.ClientID.ToString() + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');");
            //                            string JScript = "javascript:return ControlsTotal('" + grdVwAccountingItem.ClientID.ToString() + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');";
            //                            //OnClientClick="if(Page_ClientValidate()) {here call your Js function...} else {return false;}"
            //                            imgbtnSubmit.OnClientClick = "if(Page_ClientValidate()) {" + JScript + "} else {return false;}";
            //                            imgbtnContinueAdd.OnClientClick = "if(Page_ClientValidate()) {" + JScript + "} else {return false;}";
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }


        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;

            //if ((txtRevDate_JournalDoc.Text != string.Empty) || (txtStartDate_JournalDoc.Text != string.Empty))
            //{
            //    journalDocEntered = true;
            //    return journalDocEntered;
            //}


            //if ((!ddlSelectBatch_JournalDoc.SelectedValue.Contains("-1")) || (chkRecurEntry_JournalDoc.Checked) || (chkRevEntry_JournalDoc.Checked))
            //{
            //    journalDocEntered = true;
            //    return journalDocEntered;
            //}
            return journalDocEntered;
        }

        #endregion
    }
}