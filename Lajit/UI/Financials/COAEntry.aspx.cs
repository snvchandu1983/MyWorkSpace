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

namespace LAjitDev.Financials
{
    public partial class COAEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreloadImageButtonImages();
                EnableDisableUI(false);
                EnableDisableUIColor(false);
            }
        }
        #region Methods
        
        /// <summary>
        /// Prefilling images for image buttons
        /// </summary>
        private void PreloadImageButtonImages()
        {
            imgbtnAdd.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/add-icon.gif";
            imgbtnFind.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/find-icon.gif";
            imgbtnDelete.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/delete-icon.gif";
            imgbtnUndo.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/undo-icon.gif";
            imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/submit-icon.gif";
            imgbtnUpdate.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/update-icon.gif";
        }
        /// <summary>
        /// Enableing and disabling UI form conrols
        /// </summary>
        /// <param name="isEnabled">boolean</param>
        private void EnableDisableUI(bool isEnabled)
        {
            txtName.Enabled = isEnabled;
            ddlDisplaySequence.Enabled = isEnabled;
            ddlAccountNo.Enabled = isEnabled;
            ddlFormatMask.Enabled = isEnabled;
            ddlTypeofHeading.Enabled = isEnabled;
            ddlStandarTotalGroup.Enabled = isEnabled;
            ddlCashFlowGroup.Enabled = isEnabled;
            chkAccountNumberRequired.Enabled = isEnabled;
            chkIsNumeric.Enabled = isEnabled;
            chkIsApproved.Enabled = isEnabled;
            chkIsActive.Enabled = isEnabled;
            ddlTypeOfInActiveStatus.Enabled = isEnabled;
        }
        /// <summary>
        /// Clearing UIControls
        /// </summary>
        private void ClearUIContents()
        {
            txtName.Text = "";
            chkAccountNumberRequired.Checked = false;
            chkIsNumeric.Checked = false;
            chkIsApproved.Checked = false;
            chkIsActive.Checked = false;
        }

        private void EnableDisableUIColor(bool isEnabled)
        {
            if (isEnabled)
            {
                txtName.BackColor = System.Drawing.Color.White;
               
            }
            else
            {
                txtName.BackColor = System.Drawing.Color.LightGray;
               
            }
        }

        #endregion


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = ((string)Session["MyTheme"]);
        }

        protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Write("add");
            EnableDisableUI(true);
            EnableDisableUIColor(true);

            imgbtnDelete.Enabled = false;
            imgbtnUpdate.Enabled = false;


        }
        protected void imgbtnFind_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("find");
            EnableDisableUI(true);
            EnableDisableUIColor(true);

        }
        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Write("delete");
        }
        protected void imgbtnUndo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("undo");
        }
        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("Submit");
        }
        protected void imgbtnUpdate_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Write("update");
            EnableDisableUI(true);
            EnableDisableUIColor(true);

            imgbtnDelete.Enabled = false;
            imgbtnAdd.Enabled = false;
        }

        //protected void showModalPopupServerOperatorButton_Click(object sender, EventArgs e)
        //{
        //    this.programmaticModalPopup.Show();
        //}

        //protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
        //{
        //    this.programmaticModalPopup.Hide();
        //}


    }
}
