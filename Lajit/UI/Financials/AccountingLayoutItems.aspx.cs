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
    public partial class AccountingLayoutItems : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PreloadImages();
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";

        }

        #region Methods

        /// <summary>
        /// Binds the data for a given GridView or EntryForm depending on different scenarios.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        //private void BindPage()
        //{
        //    //Setting the BPInfo when the data is posted
        //    string BPInfo = string.Empty;
        //    if (Convert.ToString(Context.Request["hdnBPInfo"]) != null && Convert.ToString(Context.Request["hdnBPInfo"]) != string.Empty)
        //    {
        //        BPInfo = Convert.ToString(Context.Request["hdnBPInfo"]);
        //    }
        //    //Passing the BPInfo and Content Page Update panel as parameters to bind the page
        //    commonObjUI.BindPage(BPInfo, updtPnlContent);
        //}
        #endregion

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
            ////Preload dropdown data every time.
            FillDropDownData();
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

       #region Methods
        /// <summary>
        /// Page level exclusive method to fill dropdown box based on trxtype
        /// </summary>
        /// <param name="list"></param>
        public void PageExclusive(params object[] list)
        {
           //  for (int i = 0; i < list.Length; i++)
            // Console.WriteLine(list[i]);
            try
            {
                string TypeOfHeading_TrxType = string.Empty;

                ddlTypeOfHeading.ClearSelection();
                
                ddlTypeOfHeading.SelectedIndex = 0;
             
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(commonObjUI.ButtonsUserControl.RwToBeModified.ToString());
                XmlNode nodeSelectedRow = xDoc.SelectSingleNode("Rows");
                if (nodeSelectedRow.Attributes["TypeOfHeading_TrxType"] != null)
                {
                    TypeOfHeading_TrxType = nodeSelectedRow.Attributes["TypeOfHeading_TrxType"].Value;
                }

                XmlDataSource xDS1 = new XmlDataSource();
                xDS1.EnableCaching = false;
                
                xDS1.Data = commonObjUI.ButtonsUserControl.GVDataXml.ToString();

                xDS1.XPath = "Root/bpeout/FormControls/" + ddlTypeOfHeading.MapXML.ToString().Trim() + "/RowList//Rows[@TypeOfHeading_TrxType='" + TypeOfHeading_TrxType + "']";

                ddlTypeOfHeading.DataValueField = "DataValueField";
                ddlTypeOfHeading.DataTextField = ddlTypeOfHeading.MapXML.ToString().Trim();
                ddlTypeOfHeading.DataSource = xDS1;
                ddlTypeOfHeading.DataBind();

                //Select row
                if ((nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"] != null) && (nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"] != null))
                {
                    string dataValueField = nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"].Value + '~' + nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"].Value;
                    if ((nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"].Value != string.Empty) && (nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"].Value != string.Empty))
                    {
                        ddlTypeOfHeading.SelectedIndex = ddlTypeOfHeading.Items.IndexOf(ddlTypeOfHeading.Items.FindByValue(dataValueField));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fill Dropdown with all the available values
        /// </summary>
        private void FillDropDownData()
        {
            XmlDataSource xDS = new XmlDataSource();
            xDS.EnableCaching = false;
            xDS.Data = commonObjUI.ButtonsUserControl.GVDataXml.ToString();

            xDS.XPath = "Root/bpeout/FormControls/" + ddlTypeOfHeading.MapXML.ToString().Trim() + "/RowList/Rows";

            ddlTypeOfHeading.DataValueField = "DataValueField";
            ddlTypeOfHeading.DataTextField = ddlTypeOfHeading.MapXML.ToString().Trim();
            ddlTypeOfHeading.DataSource = xDS;
            ddlTypeOfHeading.DataBind();

            XmlDocument xDoc = new XmlDocument();

            if (commonObjUI.ButtonsUserControl.RwToBeModified != null)
            {
                xDoc.LoadXml(commonObjUI.ButtonsUserControl.RwToBeModified.ToString());
                XmlNode nodeSelectedRow = xDoc.SelectSingleNode("Rows");

                //Select row
                if ((nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"] != null) && (nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"] != null))
                {
                    string dataValueField = nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"].Value + '~' + nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"].Value;
                    if ((nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxID"].Value != string.Empty) && (nodeSelectedRow.Attributes[ddlTypeOfHeading.MapXML.Trim() + "_TrxType"].Value != string.Empty))
                    {
                        ddlTypeOfHeading.SelectedIndex = ddlTypeOfHeading.Items.IndexOf(ddlTypeOfHeading.Items.FindByValue(dataValueField));
                    }
                }

            }
        }
        #endregion
    }
}