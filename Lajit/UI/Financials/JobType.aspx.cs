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
    public partial class JobType : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imgTypeOfJob.Attributes.Add("style", "DISPLAY: none;");
        }

        #region Methods

        public void PageSpecific(params object[] list)
        {
            SetTypeOfJobHdnValue(); 
        }

        /// <summary>
        /// Sets the hidden Type Of Job variable value
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void SetTypeOfJobHdnValue()
        {
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);

            ddlTypeOfJob.Attributes.Add("OnChange", "javascript:DisplayImage();");
            hdnFldTypeOfJob.Value = string.Empty;
            if (xDocOut.SelectSingleNode("Root/bpeout/FormControls/TypeOfJob/RowList") != null)
            {
                XmlNode nodeRowlist = xDocOut.SelectSingleNode("Root/bpeout/FormControls/TypeOfJob/RowList");
                foreach (XmlNode nodeRow in nodeRowlist.ChildNodes)
                {
                    string DataValFld = string.Empty;
                    string ImgSrc = string.Empty;
                    if (nodeRow.Attributes["DataValueField"] != null)
                    {
                        DataValFld = nodeRow.Attributes["DataValueField"].Value;
                    }
                    if (nodeRow.Attributes["ImgSrcLarge"] != null)
                    {
                        ImgSrc = nodeRow.Attributes["ImgSrcLarge"].Value;
                    }
                    hdnFldTypeOfJob.Value += DataValFld + "-" + ImgSrc + ",";
                }
            } 
        }
        #endregion

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

        protected void imgbtnIsApproved_Click(object sender, EventArgs e)
        {
            commonObjUI.SubmitEntries("SOXAPPROVAL", updtPnlContent);
        }
    }
}