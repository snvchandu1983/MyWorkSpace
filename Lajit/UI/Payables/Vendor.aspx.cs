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
    public partial class Vendor : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            PreloadImages();
            if (!IsPostBack)
            {
                //commonObjUI.ButtonsUserControl.BranchXML = string.Empty;
                SetDateFormats();
                InitialiseBranchDropDowns();
            }
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";
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
            string onSubmitJS = "javascript:SaveData('VendorAddress', 'AddressType','" + ddlAddressType_VendorAddress.ClientID + "','SUBMIT');" +
                 "SaveData('VendorPhone', 'PhoneType','" + ddlPhoneType_VendorPhone.ClientID + "','SUBMIT');" +
                 "SaveData('VendorEmail', 'EmailType','" + ddlEmailType_VendorEmail.ClientID + "','SUBMIT');" +
                 "SaveData('VendorWebsite', 'WebAddressType','" + ddlWebAddressType_VendorWebsite.ClientID + "','SUBMIT');" +
                 "SaveData('VendorContactAddress', 'AddressType','" + ddlAddressType_VendorContactAddress.ClientID + "','SUBMIT');" +
                 "SaveData('VendorContactPhone', 'PhoneType','" + ddlPhoneType_VendorContactPhone.ClientID + "','SUBMIT');" +
                 "SaveData('VendorContactEmail', 'EmailType','" + ddlEmailType_VendorContactEmail.ClientID + "','SUBMIT');" +
                 "SaveData('VendorContactWebsite', 'WebAddressType','" + ddlWebAddressType_VendorContactWebsite.ClientID + "','SUBMIT');" +
                 "setBPAction();return true;";

            ddlAddressType_VendorAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_VendorPhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_VendorEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_VendorWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlAddressType_VendorContactAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_VendorContactPhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_VendorContactEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_VendorContactWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";

            //need to change so that validation should not fire for find operation.
            //imgbtnSubmit.OnClientClick = onSubmitJS;
            //imgbtnContinueAdd.OnClientClick = onSubmitJS;
           
            //imgbtnSubmit.OnClientClick = "if(Page_ClientValidate()) {" + onSubmitJS + "} else {return false;}";
            imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";

            //if (ddlPhoneType_VendorPhone.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorPhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlEmailType_VendorEmail.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlWebAddressType_VendorWebsite.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlAddressType_VendorContactAddress.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlPhoneType_VendorContactPhone.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlEmailType_VendorContactEmail.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm, "SUBMIT");
            //}
            //if (ddlWebAddressType_VendorContactWebsite.SelectedIndex != 0)
            //{
            //    objBranchUI.saveBranch("VendorContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm, "SUBMIT");
            //}
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
                            case "VENDORADDRESS":
                                {
                                    controlID = "ddlAddressType_VendorAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "VENDORPHONE":
                                {
                                    controlID = "ddlPhoneType_VendorPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "VENDOREMAIL":
                                {
                                    controlID = "ddlEmailType_VendorEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "VENDORWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_VendorWebsite";
                                    controlType = "WebAddressType";
                                    break;
                                }
                            case "VENDORCONTACTADDRESS":
                                {
                                    controlID = "ddlAddressType_VendorContactAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "VENDORCONTACTPHONE":
                                {
                                    controlID = "ddlPhoneType_VendorContactPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "VENDORCONTACTEMAIL":
                                {
                                    controlID = "ddlEmailType_VendorContactEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "VENDORCONTACTWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_VendorContactWebsite";
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
            if ((txtAddress1_VendorAddress.Text != string.Empty) || (txtAddress2_VendorAddress.Text != string.Empty) || (txtAddress3_VendorAddress.Text != string.Empty) || (txtAddress4_VendorAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }

            if ((txtCity_VendorAddress.Text != string.Empty) || (txtArea_VendorAddress.Text != string.Empty) || (txtPostalCode_VendorAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }
            if ((!ddlSelectCountry_VendorAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_VendorAddress.SelectedValue.Contains("-1")))
            {
                addEntered = true;
                return addEntered;
            }
            return addEntered;
        }

        private bool PhoneEntered()
        {
            bool phoneEntered = false;
            if ((txtTelephone_VendorPhone.Text != string.Empty))
            {
                phoneEntered = true;
                return phoneEntered;
            }
            return phoneEntered;
        }

        private bool EmailEntered()
        {
            bool emailEntered = false;
            //arrCtrls.Add("ddlAddressType_VendorAddress");
            if (txtEmail_VendorEmail.Text != string.Empty)
            {
                emailEntered = true;
                return emailEntered;
            }
            return emailEntered;
        }

        private bool WebsiteEntered()
        {
            bool websiteEntered = false;
            if (txtWebsite_VendorWebsite.Text != string.Empty)
            {
                websiteEntered = true;
                return websiteEntered;
            }
            return websiteEntered;
        }

        private bool ContactAddressEntered()
        {
            bool contactAddEntered = false;
            if ((txtAddress1_VendorContactAddress.Text != string.Empty) || (txtAddress2_VendorContactAddress.Text != string.Empty) || (txtAddress3_VendorContactAddress.Text != string.Empty) || (txtAddress4_VendorContactAddress.Text != string.Empty))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }

            if ((txtCity_VendorContactAddress.Text != string.Empty) || (txtArea_VendorContactAddress.Text != string.Empty) || (txtPostalCode_VendorContactAddress.Text != string.Empty))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }
            if ((!ddlSelectCountry_VendorContactAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_VendorContactAddress.SelectedValue.Contains("-1")))
            {
                contactAddEntered = true;
                return contactAddEntered;
            }
            return contactAddEntered;
        }

        private bool ContactPhoneEntered()
        {
            bool contactPhoneEntered = false;
            if ((txtTelephone_VendorContactPhone.Text != string.Empty))
            {
                contactPhoneEntered = true;
                return contactPhoneEntered;
            }
            return contactPhoneEntered;
        }

        private bool ContactEmailEntered()
        {
            bool contactEmailEntered = false;
            //arrCtrls.Add("ddlAddressType_VendorAddress");
            if (txtEmail_VendorContactEmail.Text != string.Empty)
            {
                contactEmailEntered = true;
                return contactEmailEntered;
            }
            return contactEmailEntered;
        }

        private bool ContactWebsiteEntered()
        {
            bool contactWebsiteEntered = false;
            if (txtWebsite_VendorContactWebsite.Text != string.Empty)
            {
                contactWebsiteEntered = true;
                return contactWebsiteEntered;
            }
            return contactWebsiteEntered;
        }

        #endregion

        private void SetBranchXML(object sender, ImageClickEventArgs e)
        {
            if (ddlAddressType_VendorAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorAddress", "AddressType", AddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_VendorPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorPhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_VendorEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_VendorWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlAddressType_VendorContactAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_VendorContactPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_VendorContactEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_VendorContactWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm, "SUBMIT");
            }
            //ddlAddressType_VendorAddress_SelectedIndexChanged(sender, e);
            //ddlPhoneType_VendorPhone_SelectedIndexChanged(sender, e);
            //ddlEmailType_VendorEmail_SelectedIndexChanged(sender, e);
            //ddlWebAddressType_VendorWebsite_SelectedIndexChanged(sender, e);
            //ddlAddressType_VendorContactAddress_SelectedIndexChanged(sender, e);
            //ddlPhoneType_VendorContactPhone_SelectedIndexChanged(sender, e);
            //ddlEmailType_VendorContactEmail_SelectedIndexChanged(sender, e);
            //ddlWebAddressType_VendorContactWebsite_SelectedIndexChanged(sender, e);
            string s = "javascript:HideShowDiv('General');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "General", s, true);
            SetBranchPrimary();
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //SetBranchXML(sender, e);
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
            string onSubmitJS = "javascript:SaveData('VendorAddress', 'AddressType','" + ddlAddressType_VendorAddress.ClientID + "','SUBMIT');" +
                                "SaveData('VendorPhone', 'PhoneType','" + ddlPhoneType_VendorPhone.ClientID + "','SUBMIT');" +
                                "SaveData('VendorEmail', 'EmailType','" + ddlEmailType_VendorEmail.ClientID + "','SUBMIT');" +
                                "SaveData('VendorWebsite', 'WebAddressType','" + ddlWebAddressType_VendorWebsite.ClientID + "','SUBMIT');" +
                                "SaveData('VendorContactAddress', 'AddressType','" + ddlAddressType_VendorContactAddress.ClientID + "','SUBMIT');" +
                                "SaveData('VendorContactPhone', 'PhoneType','" + ddlPhoneType_VendorContactPhone.ClientID + "','SUBMIT');" +
                                "SaveData('VendorContactEmail', 'EmailType','" + ddlEmailType_VendorContactEmail.ClientID + "','SUBMIT');" +
                                "SaveData('VendorContactWebsite', 'WebAddressType','" + ddlWebAddressType_VendorContactWebsite.ClientID + "','SUBMIT');" +
                                "setBPAction();return true;";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", onSubmitJS, true);

            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //SetBranchXML(sender, e);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void ddlAddressType_VendorAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_VendorAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorAddress", "AddressType", AddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_VendorAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_VendorAddress.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorAddress");
                arrCtrls.Remove("ddlAddressType_VendorAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorAddress", arrCtrls);
            }

            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('General');", true);
            txtAddress1_VendorAddress.Focus();
        }

        protected void ddlPhoneType_VendorPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_VendorPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorPhone", "PhoneType", PhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorPhone", "PhoneType", PhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_VendorPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_VendorPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorPhone");
                arrCtrls.Remove("ddlPhoneType_VendorPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorPhone", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Phone", "javascript:HideShowDiv('General');", true);
            txtTelephone_VendorPhone.Focus();
        }

        protected void ddlEmailType_VendorEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_VendorEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorEmail", "EmailType", EmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorEmail", "EmailType", EmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_VendorEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_VendorEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorEmail");
                arrCtrls.Remove("ddlEmailType_VendorEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorEmail", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Email", "javascript:HideShowDiv('General');", true);
            txtEmail_VendorEmail.Focus();
        }

        protected void ddlWebAddressType_VendorWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_VendorWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_VendorWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_VendorWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorWebsite");
                arrCtrls.Remove("ddlWebAddressType_VendorWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorWebsite", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Website", "javascript:HideShowDiv('General');", true);
            txtWebsite_VendorWebsite.Focus();
        }

        protected void ddlAddressType_VendorContactAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_VendorContactAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorContactAddress", "AddressType", ContactAddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_VendorContactAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_VendorContactAddress.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorContactAddress");
                arrCtrls.Remove("ddlAddressType_VendorContactAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorContactAddress", arrCtrls);
            }

            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactAddress", "javascript:HideShowDiv('Contact');", true);
            txtAddress1_VendorContactAddress.Focus();
        }

        protected void ddlPhoneType_VendorContactPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_VendorContactPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorContactPhone", "PhoneType", ContactPhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_VendorContactPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_VendorContactPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorContactPhone");
                arrCtrls.Remove("ddlPhoneType_VendorContactPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorContactPhone", arrCtrls);
            }

            //Focusing the tab 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactPhone", "javascript:HideShowDiv('Contact');", true);
            txtTelephone_VendorContactPhone.Focus();
        }

        protected void ddlEmailType_VendorContactEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_VendorContactEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorContactEmail", "EmailType", ContactEmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_VendorContactEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_VendorContactEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorContactEmail");
                arrCtrls.Remove("ddlEmailType_VendorContactEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorContactEmail", arrCtrls);
            }

            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactEmail", "javascript:HideShowDiv('Contact');", true);
            txtEmail_VendorContactEmail.Focus();
        }

        protected void ddlWebAddressType_VendorContactWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_VendorContactWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("VendorContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("VendorContactWebsite", "WebAddressType", ContactWebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_VendorContactWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_VendorContactWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("VendorContactWebsite");
                arrCtrls.Remove("ddlWebAddressType_VendorContactWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "VendorContactWebsite", arrCtrls);
            }

            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ContactWebsite", "javascript:HideShowDiv('Contact');", true);
            txtWebsite_VendorContactWebsite.Focus();
        }

        protected void LAjitDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void LAjitDropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        { }
    }
}