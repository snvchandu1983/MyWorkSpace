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

namespace LAjitDev.Common
{
    public partial class UserInfo : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            CGVUC.ObjCommonUI = commonObjUI;
            if (!IsPostBack)
            {
                AddScriptReferences();
            }

            PreloadImages();
            //if (!IsPostBack)
            //{
                //commonObjUI.ButtonsUserControl.BranchXML = string.Empty;
            //}
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";
        }


        #region Methods 
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
                            case "USERADDRESS":
                                {
                                    controlID = "ddlAddressType_UserAddress";
                                    controlType = "AddressType";
                                    break;
                                }
                            case "USERPHONE":
                                {
                                    controlID = "ddlPhoneType_UserPhone";
                                    controlType = "PhoneType";
                                    break;
                                }
                            case "USEREMAIL":
                                {
                                    controlID = "ddlEmailType_UserEmail";
                                    controlType = "EmailType";
                                    break;
                                }
                            case "USERWEBSITE":
                                {
                                    controlID = "ddlWebAddressType_UserWebsite";
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
            if ((txtAddress1_UserAddress.Text != string.Empty) || (txtAddress2_UserAddress.Text != string.Empty) || (txtAddress3_UserAddress.Text != string.Empty) || (txtAddress4_UserAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }

            if ((txtCity_UserAddress.Text != string.Empty) || (txtArea_UserAddress.Text != string.Empty) || (txtPostalCode_UserAddress.Text != string.Empty))
            {
                addEntered = true;
                return addEntered;
            }
            if ((!ddlSelectCountry_UserAddress.SelectedValue.Contains("-1")) || (!ddlSelectRegion_UserAddress.SelectedValue.Contains("-1")))
            {
                addEntered = true;
                return addEntered;
            }
            return addEntered;
        }

        private bool PhoneEntered()
        {
            bool phoneEntered = false;
            if ((txtTelephone_UserPhone.Text != string.Empty))
            {
                phoneEntered = true;
                return phoneEntered;
            }
            return phoneEntered;
        }

        private bool EmailEntered()
        {
            bool emailEntered = false;
            if (txtEmail_UserEmail.Text != string.Empty)
            {
                emailEntered = true;
                return emailEntered;
            }
            return emailEntered;
        }

        private bool WebsiteEntered()
        {
            bool websiteEntered = false;
            if (txtWebsite_UserWebsite.Text != string.Empty)
            {
                websiteEntered = true;
                return websiteEntered;
            }
            return websiteEntered;
        }

        #endregion

        private void SetBranchXML(object sender, ImageClickEventArgs e)
        {
            if (ddlAddressType_UserAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserAddress", "AddressType", AddressEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlPhoneType_UserPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserPhone", "PhoneType", PhoneEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlEmailType_UserEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserEmail", "EmailType", EmailEntered(), pnlEntryForm, "SUBMIT");
            }
            if (ddlWebAddressType_UserWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, "SUBMIT");
            }            
            string s = "javascript:HideShowDiv('Account Information');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Account Information", s, true);
            SetBranchPrimary();
        }

        private void AddScriptReferences()
        {
            //ljq.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "LAjitListBox.js");
            Page.Controls.Add(hgcScript1);

            //ChildGridView.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ChildGridView.js");
            Page.Controls.Add(hgcScript2);
        }




        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            SetBranchXML(sender, e);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
        }      

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            SetBranchXML(sender, e);
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
            SetBranchXML(sender, e);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
        }

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);                
        }

        protected void ddlAddressType_UserAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAddressType_UserAddress.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("UserAddress", "AddressType", AddressEntered(), pnlEntryForm);
            }
            else
            {
                ddlAddressType_UserAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_UserAddress.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("UserAddress");
                arrCtrls.Remove("ddlAddressType_UserAddress");
                objBranchUI.ClearBranchControls(pnlEntryForm, "UserAddress", arrCtrls);                
            }

            //Focusing the tab
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Address');", true);
            txtAddress1_UserAddress.Focus();
        }

        protected void ddlPhoneType_UserPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhoneType_UserPhone.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserPhone", "PhoneType", PhoneEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("UserPhone", "PhoneType", PhoneEntered(), pnlEntryForm);
            }
            else
            {
                ddlPhoneType_UserPhone.Attributes["MapPreviousSelItem"] = ddlPhoneType_UserPhone.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("UserPhone");
                arrCtrls.Remove("ddlPhoneType_UserPhone");
                objBranchUI.ClearBranchControls(pnlEntryForm, "UserPhone", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Phone", "javascript:HideShowDiv('Address');", true);
            txtTelephone_UserPhone.Focus();
        }

        protected void ddlEmailType_UserEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmailType_UserEmail.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserEmail", "EmailType", EmailEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("UserEmail", "EmailType", EmailEntered(), pnlEntryForm);
            }
            else
            {
                ddlEmailType_UserEmail.Attributes["MapPreviousSelItem"] = ddlEmailType_UserEmail.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("UserEmail");
                arrCtrls.Remove("ddlEmailType_UserEmail");
                objBranchUI.ClearBranchControls(pnlEntryForm, "UserEmail", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Email", "javascript:HideShowDiv('Address');", true);
            txtEmail_UserEmail.Focus();
        }

        protected void ddlWebAddressType_UserWebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWebAddressType_UserWebsite.SelectedIndex != 0)
            {
                objBranchUI.saveBranch("UserWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm, string.Empty);
                objBranchUI.FillBranch("UserWebsite", "WebAddressType", WebsiteEntered(), pnlEntryForm);
            }
            else
            {
                ddlWebAddressType_UserWebsite.Attributes["MapPreviousSelItem"] = ddlWebAddressType_UserWebsite.SelectedItem.Text.ToString();
                //Retrieving list of branch controls
                ArrayList arrCtrls = objBranchUI.GetBranchControls("UserWebsite");
                arrCtrls.Remove("ddlWebAddressType_UserWebsite");
                objBranchUI.ClearBranchControls(pnlEntryForm, "UserWebsite", arrCtrls);
            }
            //Focusing the tab            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Website", "javascript:HideShowDiv('Address');", true);
            txtWebsite_UserWebsite.Focus();
        }

        //protected void ddlSelectRole_UserRole_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlAddressType_UserAddress.SelectedIndex != 0)
        //    {
        //        objBranchUI.saveBranch("UserAddress", "AddressType", AddressEntered(), pnlEntryForm, string.Empty);
        //        objBranchUI.FillBranch("UserAddress", "AddressType", AddressEntered(), pnlEntryForm);
        //    }
        //    else
        //    {
        //        ddlAddressType_UserAddress.Attributes["MapPreviousSelItem"] = ddlAddressType_UserAddress.SelectedItem.Text.ToString();
        //        //Retrieving list of branch controls
        //        ArrayList arrCtrls = objBranchUI.GetBranchControls("UserAddress");
        //        arrCtrls.Remove("ddlAddressType_UserAddress");
        //        objBranchUI.ClearBranchControls(pnlEntryForm, "UserAddress", arrCtrls);
        //        //Showing the EntryForm in a modal pop up.
        //        commonObjUI.ButtonsUserControl.ShowEntryFormInPopUp();
        //    }

        //    //Focusing the tab
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Address", "javascript:HideShowDiv('Account Information');", true);
        //    txtAddress1_UserAddress.Focus();
        //}
    }
}