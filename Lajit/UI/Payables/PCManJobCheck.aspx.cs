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
    public partial class PCManJobCheck : Classes.BasePage
    {
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        protected void Page_Load(object sender, EventArgs e)
        {
            CGVUC.ObjCommonUI = commonObjUI;
            CGVUC.EnableCollapse = true;
            CGVUC.RowsToDisplay = 5;
            PreloadImages();
            if (!Page.IsPostBack)
            {
                lblCheckMessage_JournalDoc_Value.CssClass = "mbodybig";

                //Calculator 
                txtControlTotal.Attributes.Add("onfocus", "javascript:Hidden(this)");
                txtControlTotal.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                //To Show Decimals
                txtControlTotal.Attributes.Add("onblur", "javascript:FilterAmount(this);");
            }
            else
            {
                txtControlTotal.Attributes.Add("onload", "javascript:Hidden(this)");
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != null && Convert.ToString(commonObjUI.ButtonsUserControl.GVDataXml) != string.Empty)
            {
                if (!Page.IsPostBack)
                {
                    AddScriptEvent();
                }

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml.ToString());

                foreach (XmlNode xnl in xDoc.SelectSingleNode("//BusinessProcessControls"))
                {
                    if (xnl.Attributes["ID"].Value == "PrintCheck")
                    {
                        Session["BPGID"] = "<BPGID>" + xnl.Attributes["BPGID"].Value + "</BPGID>";
                    }
                }
            }
        }

        private void PreloadImages()
        {
            imgbtnIsApproved.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/Waiting-Approval.png";
        }


        /*protected void imgPrintCheck_Click(object sender, EventArgs e)
        {
            string strReqXml = string.Empty;
            string strOutXml = string.Empty;
            string OUT_BPGID = string.Empty;
            string CO_BPGID = string.Empty;
            string CO_TrxID = string.Empty;
            string CO_TrxType = string.Empty;
            string CO_NavPage = string.Empty;
            XmlDocument outXML = new XmlDocument();
            outXML.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
            foreach (XmlNode xnl in outXML.SelectSingleNode("//BusinessProcessControls"))
            {
                if (xnl.Attributes["ID"].Value == "PrintCheck")
                {
                    CO_BPGID = xnl.Attributes["BPGID"].Value;
                    CO_NavPage = xnl.Attributes["PageInfo"].Value;
                }
            }
            OUT_BPGID = outXML.SelectSingleNode("//BPGID").InnerText;
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            XmlDocument rowXML = new XmlDocument();
            rowXML.LoadXml(commonObjUI.ButtonsUserControl.BranchXML);
            CO_TrxID = rowXML.SelectSingleNode("//RowList/Rows").Attributes["TrxID"].Value;
            CO_TrxType = rowXML.SelectSingleNode("//RowList/Rows").Attributes["TrxType"].Value;
            string callingObjXML = "<CallingObject><BPGID>" + OUT_BPGID + "</BPGID><TrxID>" + CO_TrxID + "</TrxID><TrxType>" + CO_TrxType + "</TrxType><PageInfo>" + CO_NavPage + "</PageInfo><Caption>" + "" + "</Caption></CallingObject>";
            string BPInfo = "<bpinfo><BPGID>" + CO_BPGID + "</BPGID>" + rowXML.SelectSingleNode("Root").InnerXml + callingObjXML + "</bpinfo>";
            //get new row(Parent)
            string CntrlValues = commonObjUI.GetNewRow(pnlEntryForm, outXML, "Report");
            //Getting request XML
            strReqXml = "<Root>" + HttpContext.Current.Session["BPE"].ToString() + BPInfo + "</Root>";
            //BPOUT from DB
            strOutXml = reportsBO.GetReportBPEOut(strReqXml);
            //XmlDocument xdoc = new XmlDocument();
            //xdoc.LoadXml(values);
            //strReqXml = reportsBO.GenGenericProcessRequestXML("Report", currentBPGID, CntrlValues, HttpContext.Current.Session["BPE"].ToString(), false, "", "", HttpContext.Current.Session["BPINFO"].ToString(), m_treeNodeName, gvXML);
            //string strReqXml = Session["BPE"].ToString() + "<bpinfo>" + Session["BPGID"] + xdoc.SelectSingleNode("//RowList").InnerXml + "</bpinfo>";
            //string strOutxml = reportsBO.GetReportBPEOut(strReqXml);
            //ContentPlaceHolder cntPlaceHolder = (ContentPlaceHolder)this.Master.FindControl("cphPageContents");
            //UpdatePanel updPnlPrint = (UpdatePanel)cntPlaceHolder.FindControl("updPnlPrint");
            //Panel pnlPrint = null;
            //if (updPnlPrint != null)
            //{
            //    pnlPrint = (Panel)updPnlPrint.FindControl("pnlPrint");
            //    string s = "javascript:Openframe('iframePrint','PrintPopUp.aspx?PopUp=PopUp');"; // Openframe() is JavaScript Function,Passing page and frame as parameters.

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Print", s, true);
            //    pnlPrint.Height = Unit.Pixel(275);
            //    pnlPrint.Width = Unit.Pixel(620);
            //    HiddenField hdnResolution = (HiddenField)this.Master.FindControl("hdnResolution");
            //    string[] coordinates = hdnResolution.Value.Split('x');

            //    AjaxControlToolkit.ModalPopupExtender mpePrint = (AjaxControlToolkit.ModalPopupExtender)updPnlPrint.FindControl("mpePrint");
            //    mpePrint.X = Convert.ToInt32(coordinates[0]);
            //    mpePrint.Y = Convert.ToInt32(coordinates[1]);
            //    mpePrint.DropShadow = false;
            //    mpePrint.PopupControlID = "pnlPrint";
            //    mpePrint.Show();
            //}
        }*/

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
        }

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            objBranchUI.saveBranchUI("JournalDoc", JournalDocEntered(), pnlEntryForm);
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
        }        

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
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

        #region Private Methods
        private void AddScriptEvent()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(commonObjUI.ButtonsUserControl.GVDataXml);
            int columnIndex = 0;
            if ((bool)CGVUC.HideSelectColumn)
            {
                columnIndex = 0;
            }
            else
            {
                columnIndex = 1;
            }
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/AccountingItem/GridHeading/Columns");
            if (nodeColumns != null)
            {
                foreach (XmlNode colnode in nodeColumns.ChildNodes)
                {
                    if ((colnode.Attributes["FullViewLength"].Value.Trim() != "0"))
                    {
                        //Collecting all the columns which have IsRequired property.

                        if (colnode.Attributes["IsSummed"] != null)
                        {
                            if (colnode.Attributes["IsSummed"].Value == "1")
                            {
                                if (colnode.Attributes["BalanceMethod"] != null)
                                {
                                    if (colnode.Attributes["BalanceMethod"].Value.Trim() != "0")
                                    {
                                        //columnIndex
                                        //imgbtnSubmit.Attributes.Add("onclick", "javascript:return ControlsTotal('" + grdVwAccountingItem.ClientID.ToString() + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');");
                                        string JScript = "javascript:return ControlsTotal('" + CGVUC.ClientID + "_grdVwBranch" + "','" + columnIndex.ToString() + "','" + colnode.Attributes["BalanceMethod"].Value + "','" + colnode.Attributes["BalanceLabel"].Value + "');";
                                        //OnClientClick="if(Page_ClientValidate()) {here call your Js function...} else {return false;}"
                                        imgbtnSubmit.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                        imgbtnContinueAdd.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                        imgbtnAddClone.OnClientClick = "if(ValidateControls()) {" + JScript + "} else {return false;}";
                                    }
                                }
                            }
                        }
                        columnIndex = columnIndex + 1;
                    }
                }
            }
            if (imgbtnSubmit.OnClientClick == string.Empty)
            {
                imgbtnSubmit.OnClientClick = "ValidateControls()";
                imgbtnContinueAdd.OnClientClick = "ValidateControls()";
                imgbtnAddClone.OnClientClick = "ValidateControls()";
            }

        }

       

        private bool JournalDocEntered()
        {
            bool journalDocEntered = false;
            if ((txtPOReference_JournalDoc.Text != string.Empty) || (txtCheckNumber_JournalDoc.Text != string.Empty) || (txtPaymentDate_JournalDoc.Text != string.Empty))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }

            if ((!ddlEntryBank_JournalDoc.SelectedValue.Contains("-1")) || ((!ddlPCVendor_JournalDoc.SelectedValue.Contains("-1"))))
            {
                journalDocEntered = true;
                return journalDocEntered;
            }
            return journalDocEntered;
        }

        #endregion


    }
}
