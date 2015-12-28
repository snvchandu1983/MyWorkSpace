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

namespace LAjitDev.Receivables
{
    public partial class Customer : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AddScriptReferences();
                //commonObjUI.ButtonsUserControl.BranchXML = string.Empty;
                SetDateFormats();
                InitialiseBranchDropDowns();
                //Add autofill attribute to identiry in javascript and fill lastname at client side
                TextBox tbox = (TextBox)this.Master.FindControl("cphPageContents").FindControl("txtLastName");
                tbox.Attributes.Add("AddLastname", "1");
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
            Page.Header.Controls.Add(hgcScript1);

            //BranchDropDowns.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "BranchDropDowns.js");
            Page.Header.Controls.Add(hgcScript2);
        }


        private void InitialiseBranchDropDowns()
        {
            string onSubmitJS = "javascript:SaveData('CustomerAddress', 'AddressType','" + ddlAddressType_CustomerAddress.ClientID + "','SUBMIT');" +
                 "SaveData('CustomerPhone', 'PhoneType','" + ddlPhoneType_CustomerPhone.ClientID + "','SUBMIT');" +
                 "SaveData('CustomerEmail', 'EmailType','" + ddlEmailType_CustomerEmail.ClientID + "','SUBMIT');" +
                 "SaveData('CustomerWebsite', 'WebAddressType','" + ddlWebAddressType_CustomerWebsite.ClientID + "','SUBMIT');" +
                 "SaveData('CustContactAddress', 'AddressType','" + ddlAddressType_CustContactAddress.ClientID + "','SUBMIT');" +
                 "SaveData('CustomerContactPhone', 'PhoneType','" + ddlPhoneType_CustomerContactPhone.ClientID + "','SUBMIT');" +
                 "SaveData('CustomerContactEmail', 'EmailType','" + ddlEmailType_CustomerContactEmail.ClientID + "','SUBMIT');" +
                 "SaveData('CustContactWebsite', 'WebAddressType','" + ddlWebAddressType_CustContactWebsite.ClientID + "','SUBMIT');" +
                 "setBPAction();return true;";

            ddlAddressType_CustomerAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_CustomerPhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_CustomerEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_CustomerWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlAddressType_CustContactAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_CustomerContactPhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_CustomerContactEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_CustContactWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";

            //imgbtnSubmit.OnClientClick = onSubmitJS;
            //imgbtnContinueAdd.OnClientClick = onSubmitJS;
            imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
        }

        #region Methods
        private void SetDateFormats()
        {
            //meeEDDStartDate.Mask = "99/99/9999";
            //meeEDDEndDate.Mask = "99/99/9999";            
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
                            case "CUSTOMERADDRESS":
                                {
                                    controlID = "ddlAddressType_CustomerAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "CUSTOMERPHONE":
                                {
                                    controlID = "ddlPhoneType_CustomerPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "CUSTOMEREMAIL":
                                {
                                    controlID = "ddlEmailType_CustomerEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "CUSTOMERWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_CustomerWebsite";
                                    controlType = "WebAddressType";
                                    break;
                                }
                            case "CUSTCONTACTADDRESS":
                                {
                                    controlID = "ddlAddressType_CustContactAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "CUSTOMERCONTACTPHONE":
                                {
                                    controlID = "ddlPhoneType_CustomerContactPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "CUSTOMERCONTACTEMAIL":
                                {
                                    controlID = "ddlEmailType_CustomerContactEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "CUSTCONTACTWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_CustContactWebsite";
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
            if ((txtAddress1_CustomerAddress.Text != string.Empty) || (txtAddress2_CustomerAddress.Text != string.Empty) || (txtAddress3_CustomerAddress.Text != string.Empty) || (txtAddress4_CustomerAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }

            if ((txtCity_CustomerAddress.Text != string.Empty) || (txtArea_CustomerAddress.Text != string.Empty) || (txtPostalCode_CustomerAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }
            if ((!ddlSelectCountry_CustomerAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_CustomerAddress.SelectedValue.Contains("-1")))
            {
                addEntered = true;
                return addEntered;
            }
            return addEntered;
        }

        private bool PhoneEntered()
        {
            bool phoneEntered = false;
            if ((txtTelephone_CustomerPhone.Text != string.Empty))
            {
                phoneEntered = true;
                return phoneEntered;
            }
            return phoneEntered;
        }

        private bool EmailEntered()
        {
            bool emailEntered = false;
            //arrCtrls.Add("ddlAddressType_CustomerAddress");
            if (txtEmail_CustomerEmail.Text != string.Empty)
            {
                emailEntered = true;
                return emailEntered;
            }
            return emailEntered;
        }

        private bool WebsiteEntered()
        {
            bool websiteEntered = false;
            if (txtWebsite_CustomerWebsite.Text != string.Empty)
            {
                websiteEntered = true;
                return websiteEntered;
            }
            return websiteEntered;
        }

        private bool ContactAddressEntered()
        {
            bool contactAddEntered = false;
            if ((txtAddress1_CustContactAddress.Text != string.Empty) || (txtAddress2_CustContactAddress.Text != string.Empty) || (txtAddress3_CustContactAddress.Text != string.Empty) || (txtAddress4_CustContactAddress.Text != string.Empty) || (txtContactInfo_CustContactAddress.Text != string.Empty))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }

            if ((txtCity_CustContactAddress.Text != string.Empty) || (txtArea_CustContactAddress.Text != string.Empty) || (txtPostalCode_CustContactAddress.Text != string.Empty))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }
            if ((!ddlSelectCountry_CustContactAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_CustContactAddress.SelectedValue.Contains("-1")))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }
            return contactAddEntered;
        }

        private bool ContactPhoneEntered()
        {
            bool contactPhoneEntered = false;
            if ((txtTelephone_CustomerContactPhone.Text != string.Empty) || (txtContactInfo_CustomerContactPhone.Text!=string.Empty))
            {
                contactPhoneEntered = true;
                return contactPhoneEntered;
            }
            return contactPhoneEntered;
        }

        private bool ContactEmailEntered()
        {
            bool contactEmailEntered = false;
            //arrCtrls.Add("ddlAddressType_CustomerAddress");
            if (txtEmail_CustomerContactEmail.Text != string.Empty || txtContactInfo_CustomerContactEmail.Text!=string.Empty)
            {
                contactEmailEntered = true;
                return contactEmailEntered;
            }
            return contactEmailEntered;
        }

        private bool ContactWebsiteEntered()
        {
            bool contactWebsiteEntered = false;
            if (txtWebsite_CustContactWebsite.Text != string.Empty || txtContactInfo_CustContactWebsite.Text!=string.Empty)
            {
                contactWebsiteEntered = true;
                return contactWebsiteEntered;
            }
            return contactWebsiteEntered;
        }

        #endregion

        private void SetBranchXML(object sender, ImageClickEventArgs e)
        {
            //ddlAddressType_CustomerAddress_SelectedIndexChanged(sender, e);
            //ddlPhoneType_CustomerPhone_SelectedIndexChanged(sender, e);
            //ddlEmailType_CustomerEmail_SelectedIndexChanged(sender, e);
            //ddlWebAddressType_CustomerWebsite_SelectedIndexChanged(sender, e);

            //ddlAddressType_CustContactAddress_SelectedIndexChanged(sender, e);
            //ddlPhoneType_CustomerContactPhone_SelectedIndexChanged(sender, e);
            //ddlEmailType_CustomerContactEmail_SelectedIndexChanged(sender, e);
            //ddlWebAddressType_CustContactWebsite_SelectedIndexChanged(sender, e);

            //string s = "javascript:HideShowDiv('General');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "General", s, true);
            //SetBranchPrimary();

            if (ddlAddressType_CustomerAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerAddress", "AddressType", AddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_CustomerPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerPhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_CustomerEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_CustomerWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlAddressType_CustContactAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_CustomerContactPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_CustomerContactEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_CustContactWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm, "SUBMIT");
            }            
            string s = "javascript:HideShowDiv('General');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "General", s, true);
            SetBranchPrimary();
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //SetBranchXML(sender,e);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
            
            //update Autofill hidden box if querystring CL=Cutomer
            if (Request["CL"] != null)
            {
                //Update parent autofilltextbox value
                hdnAutoFillNewEntry.Value = "txtCustomer_JournalDoc;" + txtLastName.Text + ' ' + txtFirstName.Text;
            }
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
            //Refersh AutofillCache if querystring CL=Cutomer
            if (Request["CL"] != null)
            {
                //Update parent autofilltextbox value
                hdnAutoFillNewEntry.Value = "txtCustomer_JournalDoc;" + txtLastName.Text + ' ' + txtFirstName.Text;
            }
        }

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            string onSubmitJS = "javascript:SaveData('CustomerAddress', 'AddressType','" + ddlAddressType_CustomerAddress.ClientID + "','SUBMIT');" +
                                "SaveData('CustomerPhone', 'PhoneType','" + ddlPhoneType_CustomerPhone.ClientID + "','SUBMIT');" +
                                "SaveData('CustomerEmail', 'EmailType','" + ddlEmailType_CustomerEmail.ClientID + "','SUBMIT');" +
                                "SaveData('CustomerWebsite', 'WebAddressType','" + ddlWebAddressType_CustomerWebsite.ClientID + "','SUBMIT');" +
                                "SaveData('CustContactAddress', 'AddressType','" + ddlAddressType_CustContactAddress.ClientID + "','SUBMIT');" +
                                "SaveData('CustomerContactPhone', 'PhoneType','" + ddlPhoneType_CustomerContactPhone.ClientID + "','SUBMIT');" +
                                "SaveData('CustomerContactEmail', 'EmailType','" + ddlEmailType_CustomerContactEmail.ClientID + "','SUBMIT');" +
                                "SaveData('CustContactWebsite', 'WebAddressType','" + ddlWebAddressType_CustContactWebsite.ClientID + "','SUBMIT');" +
                                "setBPAction();return true;";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", onSubmitJS, true);

            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //SetBranchXML(sender, e);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
             if((bool)commonObjUI.AddMultipleRecords(updtPnlContent))
             {
                if (Request["CL"] != null)
                {
                    //Update parent autofilltextbox value
                    hdnAutoFillNewEntry.Value = "txtCustomer_JournalDoc;" + txtLastName.Text + ' ' + txtFirstName.Text;
                }
             }
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
            if (Request["CL"] != null)
            {
                //Update parent autofilltextbox value
                hdnAutoFillNewEntry.Value = "txtCustomer_JournalDoc;" + txtLastName.Text + ' ' + txtFirstName.Text;
            }
        }

        protected void ddlAddressType_CustomerAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_CustomerAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustomerAddress", "AddressType", AddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_CustomerAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_CustomerAddress.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerAddress");
                arrCtrls.Remove("ddlAddressType_CustomerAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerAddress", arrCtrls);               
            }
            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('General');", true);
            txtAddress1_CustomerAddress.Focus();
        }

        protected void ddlPhoneType_CustomerPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_CustomerPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerPhone", "PhoneType", PhoneEntered(), pnlEntryForm,string.Empty);
                objBranchUI.FillBranch("CustomerPhone", "PhoneType", PhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_CustomerPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_CustomerPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerPhone");
                arrCtrls.Remove("ddlPhoneType_CustomerPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerPhone", arrCtrls);                
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Phone", "javascript:HideShowDiv('General');", true);
            txtTelephone_CustomerPhone.Focus();
        }

        protected void ddlEmailType_CustomerEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_CustomerEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerEmail", "EmailType", EmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustomerEmail", "EmailType", EmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_CustomerEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_CustomerEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerEmail");
                arrCtrls.Remove("ddlEmailType_CustomerEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerEmail", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Email", "javascript:HideShowDiv('General');", true);
            txtEmail_CustomerEmail.Focus();
        }

        protected void ddlWebAddressType_CustomerWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_CustomerWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustomerWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_CustomerWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_CustomerWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerWebsite");
                arrCtrls.Remove("ddlWebAddressType_CustomerWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerWebsite", arrCtrls);                
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Website", "javascript:HideShowDiv('General');", true);
            txtWebsite_CustomerWebsite.Focus();
        }

        protected void ddlAddressType_CustContactAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_CustContactAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_CustContactAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_CustContactAddress.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustContactAddress");
                arrCtrls.Remove("ddlAddressType_CustContactAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustContactAddress", arrCtrls);
            }

            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactAddress", "javascript:HideShowDiv('Contact');", true);
            txtAddress1_CustContactAddress.Focus();
        }

        protected void ddlPhoneType_CustomerContactPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_CustomerContactPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustomerContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_CustomerContactPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_CustomerContactPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerContactPhone");
                arrCtrls.Remove("ddlPhoneType_CustomerContactPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerContactPhone", arrCtrls);               
            }
            //Focusing the tab 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactPhone", "javascript:HideShowDiv('Contact');", true);
            txtTelephone_CustomerContactPhone.Focus();
        }

        protected void ddlEmailType_CustomerContactEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_CustomerContactEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustomerContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustomerContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_CustomerContactEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_CustomerContactEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustomerContactEmail");
                arrCtrls.Remove("ddlEmailType_CustomerContactEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustomerContactEmail", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactEmail", "javascript:HideShowDiv('Contact');", true);
            txtEmail_CustomerContactEmail.Focus();
        }

        protected void ddlWebAddressType_CustContactWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_CustContactWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("CustContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("CustContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_CustContactWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_CustContactWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("CustContactWebsite");
                arrCtrls.Remove("ddlWebAddressType_CustContactWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "CustContactWebsite", arrCtrls);               
            }
            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactWebsite", "javascript:HideShowDiv('Contact');", true);
            txtWebsite_CustContactWebsite.Focus();
        }        
    }
}
