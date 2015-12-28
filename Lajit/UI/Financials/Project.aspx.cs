using System;
using System.Web.UI;
using System.Drawing;
using System.Xml;
using System.Web.UI.HtmlControls;

namespace LAjitDev.Financials
{
    public partial class Project : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            PreloadImages();
        }

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
                    string dataValField = string.Empty;
                    string imgSrc = string.Empty;
                    if (nodeRow.Attributes["DataValueField"] != null)
                    {
                        dataValField = nodeRow.Attributes["DataValueField"].Value;
                    }
                    if (nodeRow.Attributes["ImgSrcLarge"] != null)
                    {
                        imgSrc = nodeRow.Attributes["ImgSrcLarge"].Value;
                    }
                    hdnFldTypeOfJob.Value += dataValField + "-" + imgSrc + ",";
                }
            }
        }


        private void PreloadImages()
        {
            imgTypeOfJob.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/spacer.gif";
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