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

namespace LAjitDev.Financials
{
    public partial class Employee : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PreloadImages();

            if (!IsPostBack)
            {
                AddScriptReferences();

                //commonObjUI.ButtonsUserControl.BranchXML = string.Empty;
                SetDateFormats();
                InitialiseBranchDropDowns();
            }
        }

        private void InitialiseBranchDropDowns()
        {
            string onSubmitJS = "javascript:SaveData('EmployeeAddress', 'AddressType','" + ddlAddressType_EmployeeAddress.ClientID + "','SUBMIT');" +
                 "SaveData('EmployeePhone', 'PhoneType','" + ddlPhoneType_EmployeePhone.ClientID + "','SUBMIT');" +
                 "SaveData('EmployeeEmail', 'EmailType','" + ddlEmailType_EmployeeEmail.ClientID + "','SUBMIT');" +
                 "SaveData('EmployeeWebsite', 'WebAddressType','" + ddlWebAddressType_EmployeeWebsite.ClientID + "','SUBMIT');" +
                 "setBPAction();return true;";

            ddlAddressType_EmployeeAddress.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlPhoneType_EmployeePhone.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlEmailType_EmployeeEmail.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";
            ddlWebAddressType_EmployeeWebsite.Attributes["onchange"] = "OnBranchDropDownChange(this);return false;";

            //imgbtnSubmit.OnClientClick = onSubmitJS;
            //imgbtnContinueAdd.OnClientClick = onSubmitJS;
            imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
            imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + onSubmitJS + "} else {return false;}";
        }

        #region Methods
       
        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

        }


        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //BranchDropDowns.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "BranchDropDowns.js");
            Page.Header.Controls.Add(hgcScript1);
        }
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
                            case "EMPLOYEEADDRESS":
                                {
                                    controlID = "ddlAddressType_EmployeeAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "EMPLOYEEPHONE":
                                {
                                    controlID = "ddlPhoneType_EmployeePhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "EMPLOYEEEMAIL":
                                {
                                    controlID = "ddlEmailType_EmployeeEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "EMPLOYEEWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_EmployeeWebsite";
                                    controlType = "WebAddressType";
                                    break;
                                }
                            //case "EMPLOYEECONTACTADDRESS":
                            //    {
                            //        controlID = "ddlAddressType_EmployeeContactAddress";
                            //        controlType = "AddressType";
                            //        break;
                            //    }
                            //case "EMPLOYEECONTACTPHONE":
                            //    {
                            //        controlID = "ddlPhoneType_EmployeeContactPhone";
                            //        controlType = "PhoneType";
                            //        break;
                            //    }
                            //case "EMPLOYEECONTACTEMAIL":
                            //    {
                            //        controlID = "ddlEmailType_EmployeeContactEmail";
                            //        controlType = "EmailType";
                            //        break;
                            //    }
                            //case "EMPLOYEECONTACTWEBSITE":
                            //    {
                            //        controlID = "ddlWebAddressType_EmployeeContactWebsite";
                            //        controlType = "WebAddressType";
                            //        break;
                            //    }
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
            if ((txtAddress1_EmployeeAddress.Text != string.Empty) || (txtAddress2_EmployeeAddress.Text != string.Empty) || (txtAddress3_EmployeeAddress.Text != string.Empty) || (txtAddress4_EmployeeAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }

            if ((txtCity_EmployeeAddress.Text != string.Empty) || (txtArea_EmployeeAddress.Text != string.Empty) || (txtPostalCode_EmployeeAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }
            if ((!ddlSelectCountry_EmployeeAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_EmployeeAddress.SelectedValue.Contains("-1")))
            {
                addEntered = true;
                return addEntered;
            }
            return addEntered;
        }

        private bool PhoneEntered()
        {
            bool phoneEntered = false;
            if ((txtTelephone_EmployeePhone.Text != string.Empty))
            {
                phoneEntered = true;
                return phoneEntered;
            }
            return phoneEntered;
        }

        private bool EmailEntered()
        {
            bool emailEntered = false;
            //arrCtrls.Add("ddlAddressType_EmployeeAddress");
            if (txtEmail_EmployeeEmail.Text != string.Empty)
            {
                emailEntered = true;
                return emailEntered;
            }
            return emailEntered;
        }

        private bool WebsiteEntered()
        {
            bool websiteEntered = false;
            if (txtWebsite_EmployeeWebsite.Text != string.Empty)
            {
                websiteEntered = true;
                return websiteEntered;
            }
            return websiteEntered;
        }
        #endregion

        private void SetBranchXML(object sender, ImageClickEventArgs e)
        {
            //ddlAddressType_EmployeeAddress_SelectedIndexChanged(sender, e);
            //ddlPhoneType_EmployeePhone_SelectedIndexChanged(sender, e);
            //ddlEmailType_EmployeeEmail_SelectedIndexChanged(sender, e);
            //ddlWebAddressType_EmployeeWebsite_SelectedIndexChanged(sender, e);           
            //string s = "javascript:HideShowDiv('General');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "General", s, true);
            //SetBranchPrimary();
            if (ddlAddressType_EmployeeAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeAddress", "AddressType", AddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_EmployeePhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeePhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_EmployeeEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_EmployeeWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            }
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
            string onSubmitJS = "javascript:SaveData('EmployeeAddress', 'AddressType','" + ddlAddressType_EmployeeAddress.ClientID + "','SUBMIT');" +
                    "SaveData('EmployeePhone', 'PhoneType','" + ddlPhoneType_EmployeePhone.ClientID + "','SUBMIT');" +
                    "SaveData('EmployeeEmail', 'EmailType','" + ddlEmailType_EmployeeEmail.ClientID + "','SUBMIT');" +
                    "SaveData('EmployeeWebsite', 'WebAddressType','" + ddlWebAddressType_EmployeeWebsite.ClientID + "','SUBMIT');" +
                    "setBPAction();return true;";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", onSubmitJS, true);

            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void lnkBtnIsApproved_Click(object sender, EventArgs e)
        {
            //Calling the Submit Method to Update Sox Approval Status
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //SetBranchXML(sender, e);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void ddlAddressType_EmployeeAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_EmployeeAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("EmployeeAddress", "AddressType", AddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_EmployeeAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_EmployeeAddress.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("EmployeeAddress");
                arrCtrls.Remove("ddlAddressType_EmployeeAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "EmployeeAddress", arrCtrls);
            }
            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('General');", true);
            txtAddress1_EmployeeAddress.Focus();
        }

        protected void ddlPhoneType_EmployeePhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_EmployeePhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeePhone", "PhoneType", PhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("EmployeePhone", "PhoneType", PhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_EmployeePhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_EmployeePhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("EmployeePhone");
                arrCtrls.Remove("ddlPhoneType_EmployeerPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "EmployeePhone", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Phone", "javascript:HideShowDiv('General');", true);
            txtTelephone_EmployeePhone.Focus();
        }

        protected void ddlEmailType_EmployeeEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_EmployeeEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeEmail", "EmailType", EmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("EmployeeEmail", "EmailType", EmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_EmployeeEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_EmployeeEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("EmployeeEmail");
                arrCtrls.Remove("ddlEmailType_EmployeeEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "EmployeeEmail", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Email", "javascript:HideShowDiv('General');", true);
            txtEmail_EmployeeEmail.Focus();
        }

        protected void ddlWebAddressType_EmployeeWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_EmployeeWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("EmployeeWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("EmployeeWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_EmployeeWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_EmployeeWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("EmployeeWebsite");
                arrCtrls.Remove("ddlWebAddressType_EmployeeWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "EmployeeWebsite", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Website", "javascript:HideShowDiv('General');", true);
            txtWebsite_EmployeeWebsite.Focus();
        }

        //private bool AddressEntered()
        //{
        //    bool addEntered = false;           
        //    if ((txtAddress1_EmployeeAddress.Text != string.Empty) || (txtAddress2_EmployeeAddress.Text != string.Empty) || (txtAddress3_EmployeeAddress.Text != string.Empty) || (txtAddress4_EmployeeAddress.Text != string.Empty))
        //    {
        //        addEntered = true;
        //        return addEntered;
        //    }

        //    if ((txtCity_EmployeeAddress.Text != string.Empty) || (txtArea_EmployeeAddress.Text != string.Empty) || (txtPostalCode_EmployeeAddress.Text != string.Empty))
        //    {
        //        addEntered = true;
        //        return addEntered;
        //    }
        //    if ((!ddlSelectCountry_EmployeeAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_EmployeeAddress.SelectedValue.Contains("-1")))
        //    {
        //        addEntered = true;
        //        return addEntered;
        //    }
        //    return addEntered;
        //}

        //private bool PhoneEntered()
        //{
        //    bool phoneEntered = false;            
        //    if ((txtTelephone_EmployeePhone.Text != string.Empty))
        //    {
        //        phoneEntered = true;
        //        return phoneEntered;
        //    }
        //    return phoneEntered;
        //}

        //private bool EmailEntered()
        //{
        //    bool emailEntered = false;
        //    //arrCtrls.Add("ddlAddressType_EmployeeAddress");
        //    if (txtEmail_EmployeeEmail.Text != string.Empty)
        //    //||(txtContactInfo_EmployeeEmail.Text!=string.Empty))
        //    {
        //        emailEntered = true;
        //        return emailEntered;
        //    }
        //    return emailEntered;
        //}

        //private bool WebsiteEntered()
        //{
        //    bool websiteEntered = false;            
        //    if(txtWebsite_EmployeeWebsite.Text != string.Empty)
        //    {
        //        websiteEntered = true;
        //        return websiteEntered;
        //    }
        //    return websiteEntered;
        //}

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);           
        }
    }
}