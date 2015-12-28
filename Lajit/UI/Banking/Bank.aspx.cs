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

namespace LAjitDev.Banking
{
    public partial class Bank : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                AddScriptReferences();
                //commonObjUI.ButtonsUserControl.BranchXML = string.Empty;
                InitialiseBranchDropDowns();
            }
        }

        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //LAjitListBox.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "LAjitListBox.js");
          
            Page.Controls.Add(hgcScript1);

            //BranchDropDowns.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "BranchDropDowns.js");
            Page.Controls.Add(hgcScript2);

          
        }



        private void InitialiseBranchDropDowns()
        {
            string onSubmitJS = "javascript:SaveData('BankAddress', 'AddressType','" + ddlAddressType_BankAddress.ClientID + "','SUBMIT');" +
                 "SaveData('BankPhone', 'PhoneType','" + ddlPhoneType_BankPhone.ClientID + "','SUBMIT');" +
                 "SaveData('BankEmail', 'EmailType','" + ddlEmailType_BankEmail.ClientID + "','SUBMIT');" +
                 "SaveData('BankWebsite', 'WebAddressType','" + ddlWebAddressType_BankWebsite.ClientID + "','SUBMIT');" +
                 "setBPAction();return true;";

            ddlAddressType_BankAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_BankPhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_BankEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_BankWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";

            //imgbtnSubmit.OnClientClick = onSubmitJS;
            //imgbtnContinueAdd.OnClientClick = onSubmitJS;
            imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
        }       

        private void SetBranchPrimary()
        {
            string branchXML = string.Empty;
            XmlDocument xBranchDoc = new XmlDocument();
            if (commonObjUI.ButtonsUserControl.BranchXML != string.Empty)
            {
                xBranchDoc.LoadXml(commonObjUI.ButtonsUserControl.BranchXML);
                XmlNode nodeRoot = xBranchDoc.SelectSingleNode("Root");
                XmlNodeList listBranches = nodeRoot.ChildNodes;
                bool primarySel = false;
                foreach (XmlNode nodeBranchRow in listBranches)
                {
                    XmlNode nodeRowlist = nodeBranchRow.SelectSingleNode("RowList");
                    if (nodeRowlist != null)
                    {
                        foreach (XmlNode nodeRow in nodeRowlist.ChildNodes)
                        {
                            if (nodeRow.Attributes["IsPrimary"].Value == "1")
                            {
                                primarySel = true;
                            }
                        }
                    }
                    if (!primarySel)
                    {

                        string branchNodeName = nodeBranchRow.LocalName;
                        string controlID = string.Empty;
                        string controlType = string.Empty;

                        switch (branchNodeName.ToUpper().Trim())
                        {
                            case "BANKADDRESS":
                                {
                                    controlID = "ddlAddressType_BankAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "BANKPHONE":
                                {
                                    controlID = "ddlPhoneType_BankPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "BANKEMAIL":
                                {
                                    controlID = "ddlEmailType_BankEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "BANKWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_BankWebsite";
                                    controlType = "WebAddressType";
                                    break;
                                }                           
                            default:
                                {
                                    break;
                                }
                        }
                        if (controlID != string.Empty)
                        {
                            LAjitControls.LAjitDropDownList ddlDropDown = (LAjitControls.LAjitDropDownList)pnlEntryForm.FindControl(controlID);
                            foreach (ListItem ddlValue in ddlDropDown.Items)
                            {
                                if (nodeRowlist != null)
                                {
                                    XmlNode nodeRow = nodeRowlist.SelectSingleNode("Rows[@" + controlType + "='" + ddlValue.Text.ToString().Trim() + "']");
                                    if (nodeRow != null)
                                    {
                                        nodeRow.Attributes["IsPrimary"].Value = "1";
                                        commonObjUI.ButtonsUserControl.BranchXML = xBranchDoc.OuterXml;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        private bool AddressEntered()
        {
            bool addEntered = false;
            if ((txtAddress1_BankAddress.Text != string.Empty) || (txtAddress2_BankAddress.Text != string.Empty) || (txtAddress3_BankAddress.Text != string.Empty) || (txtAddress4_BankAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }

            if ((txtCity_BankAddress.Text != string.Empty) || (txtArea_BankAddress.Text != string.Empty) || (txtPostalCode_BankAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }
            if ((!ddlSelectCountry_BankAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_BankAddress.SelectedValue.Contains("-1")))
            {
                addEntered = true;
                return addEntered;
            }
            return addEntered;
        }

        private bool PhoneEntered()
        {
            bool phoneEntered = false;
            if ((txtTelephone_BankPhone.Text != string.Empty))
            {
                phoneEntered = true;
                return phoneEntered;
            }
            return phoneEntered;
        }

        private bool EmailEntered()
        {
            bool emailEntered = false;
            //arrCtrls.Add("ddlAddressType_BankAddress");
            if (txtEmail_BankEmail.Text != string.Empty)
            //||(txtContactInfo_BankEmail.Text!=string.Empty))
            {
                emailEntered = true;
                return emailEntered;
            }
            return emailEntered;
        }

        private bool WebsiteEntered()
        {
            bool websiteEntered = false;
            if (txtWebsite_BankWebsite.Text != string.Empty)
            {
                websiteEntered = true;
                return websiteEntered;
            }
            return websiteEntered;
        }

        private void SetBranchXML(object sender, ImageClickEventArgs e)
        {
            if (ddlAddressType_BankAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankAddress", "AddressType", AddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_BankPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankPhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_BankEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_BankWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            }            
            string s = "javascript:HideShowDiv('General');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "General", s, true);
            SetBranchPrimary();
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
            string onSubmitJS = "javascript:SaveData('BankAddress', 'AddressType','" + ddlAddressType_BankAddress.ClientID + "','SUBMIT');" +
                "SaveData('BankPhone', 'PhoneType','" + ddlPhoneType_BankPhone.ClientID + "','SUBMIT');" +
                "SaveData('BankEmail', 'EmailType','" + ddlEmailType_BankEmail.ClientID + "','SUBMIT');" +
                "SaveData('BankWebsite', 'WebAddressType','" + ddlWebAddressType_BankWebsite.ClientID + "','SUBMIT');" +
                "setBPAction();return true;";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", onSubmitJS, true);

            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }       

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void ddlAddressType_BankAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_BankAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("BankAddress", "AddressType", AddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_BankAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_BankAddress.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("BankAddress");
                arrCtrls.Remove("ddlAddressType_BankAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "BankAddress", arrCtrls);                
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Other');", true);
            txtAddress1_BankAddress.Focus();
        }

        protected void ddlPhoneType_BankPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_BankPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankPhone", "PhoneType", PhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("BankPhone", "PhoneType", PhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_BankPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_BankPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("BankPhone");
                arrCtrls.Remove("ddlPhoneType_BankPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "BankPhone", arrCtrls);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Other');", true);
            txtTelephone_BankPhone.Focus();
        }

        protected void ddlEmailType_BankEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_BankEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankEmail", "EmailType", EmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("BankEmail", "EmailType", EmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_BankEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_BankEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("BankEmail");
                arrCtrls.Remove("ddlEmailType_BankEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "BankEmail", arrCtrls);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Other');", true);
            trEmailType_BankEmail.Focus();
        }

        protected void ddlWebAddressType_BankWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_BankWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("BankWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("BankWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_BankWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_BankWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("BankWebsite");
                arrCtrls.Remove("ddlWebAddressType_BankWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "BankWebsite", arrCtrls);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Other');", true);
            txtWebsite_BankWebsite.Focus();
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);            
        }
    }
}