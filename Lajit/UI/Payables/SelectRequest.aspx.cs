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
using NLog;


namespace LAjitDev.Payables
{
    public partial class SelectRequest : Classes.BasePage
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public LAjitDev.clsReportsUI reportObjUI = new clsReportsUI();
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        //Creating object of ProcessEngine page to use its methods and properties.
        LAjitDev.Common.ProcessEngine procEngObj = new LAjitDev.Common.ProcessEngine();
        XmlDocument XDocUserInfo = new XmlDocument();

        public String RetXML
        {
            get { return (String)ViewState["RetXML"]; }
            set { ViewState["RetXML"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Setting the BPInfo when the data is posted
            string BPInfo = string.Empty;
            if (Convert.ToString(Context.Request["hdnBPInfo"]) != null && Convert.ToString(Context.Request["hdnBPInfo"]) != string.Empty)
            {
                BPInfo = Convert.ToString(Context.Request["hdnBPInfo"]);
            }
            else if (HttpContext.Current.Session["LinkBPinfo"] != null)
            {
                //only for pages whose master page is 'PopUp', we need to take Session 'LinkBPinfo'.
                if (!this.Page.MasterPageFile.Contains("TopLeft.Master"))
                {
                    //setting the  Session for childpage.
                    BPInfo = HttpContext.Current.Session["LinkBPinfo"].ToString();
                }
            }
            XmlDocument xDoc = new XmlDocument();
            if (!Page.IsPostBack)
            {
                string retXML = string.Empty;
                BindPage(BPInfo, pnlContent, out retXML);
                RetXML = retXML;
                xDoc.LoadXml(RetXML);

            }
            else
            {
                xDoc.LoadXml(RetXML);
                //Branch objects
                commonObjUI.InitialiseBranchObjects(xDoc, pnlEntryForm);
            }

            if (RetXML != null && RetXML != string.Empty)
            {
                procEngObj.SetLabelMessage(RetXML);
                //Get the Form-Level BPC links table and add it to the panel in the form.           
                pnlReport.Controls.Clear();              
                pnlReport.Controls.Add(reportObjUI.CreateProcessLinksTable(RetXML));
            }

            //ChildGridView requirements.
            UserControls.ButtonsUserControl objBtnsUC = new LAjitDev.UserControls.ButtonsUserControl();
            objBtnsUC.GVDataXml = RetXML;
            commonObjUI.ButtonsUserControl = objBtnsUC;
            XmlNode nodeTree = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node");
            if (nodeTree != null)
            {
                commonObjUI.ButtonsUserControl.TreeNode = nodeTree.InnerText;
            }
            CGVUC.ObjCommonUI = commonObjUI;
            CGVUC.OnlyChildGVPresent = true;
            CGVUC.HideSelectColumn = true;
        }

        public void BindPage(string BPInfo, Control CurrentPage, out string retXML)
        {
            #region NLog
            logger.Info("Rendering the page with BPInfo : " + BPInfo + " Currrent Page as : " + CurrentPage);
            #endregion

            retXML = string.Empty;
            Panel pnlEntryForm = (Panel)CurrentPage.FindControl("pnlEntryForm");
            Panel pnlEntryFormTitle = (Panel)CurrentPage.FindControl("pnlEntryFormTitle");

            Page curntPage = (Page)CurrentPage.Page;

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);


            string strReqXml = reportsBO.GenGenericProcessRequestXML("PAGELOAD", BPInfo, "", strBPE, false, "", "", BPInfo, "", "");
            //BPOUT from DB
            string strOutXml = reportsBO.GetReportBPEOut(strReqXml);

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(strOutXml);

            //success messge            
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeMsgStatus != null)
            {
                if (nodeMsgStatus.InnerText == "Success")
                {                  
                    retXML = strOutXml;
                    string m_treeNodeName = string.Empty;
                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
                    {
                        m_treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                    }                   

                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");
                    if (nodeColumns != null)
                    {
                        //Invisible all the columns whose IsHidden Attribute is one
                        commonObjUI.HandleIsHidden(pnlEntryForm, xDoc, false);

                        //For Filling DD
                        ArrayList alColIslink = new ArrayList();
                        reportObjUI.SetLabelText(xDoc, out alColIslink, pnlEntryForm, m_treeNodeName);

                        //Filling dropdown data
                        if (alColIslink.Count > 0)
                        {
                            commonObjUI.FillDropDownData(strOutXml, alColIslink, pnlEntryForm);
                        }

                        //To set the default values for all page controls(both parent and child).
                        commonObjUI.SetDefault(pnlEntryForm, nodeColumns);
                    }

                    XmlNode xNodeRow = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows");
                    commonObjUI.GVSelectedRow = xNodeRow;                    
                   
                    //Visible or Invisible the EntryForm and CPGV based on different scenarios.            
                    XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList");

                    //Empty EntryForm for 'Add'
                    if (nodeRowList == null)
                    {
                        Label lblStatus = (Label)CurrentPage.FindControl("lblStatus");
                        if (xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label") != null)
                        {
                            lblStatus.Text = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Label").InnerText;
                        }
                    }
                    else//modify mode
                    {
                        pnlEntryForm.Visible = true;
                        
                        //Filling Tree node controls
                        commonObjUI.EnableDisableAndFillUI(true, pnlEntryForm, xDoc, nodeRowList.ChildNodes[0], string.Empty, true);
                        //Setting the Description label value
                        Label lbldescValue = (Label)pnlEntryForm.FindControl("lblDescriptionValue");
                        if (lbldescValue != null)
                        {
                            if (nodeRowList.ChildNodes[0] != null)
                            {
                                if (nodeRowList.ChildNodes[0].Attributes["Description"] != null)
                                {
                                    lbldescValue.Text = nodeRowList.ChildNodes[0].Attributes["Description"].Value; ;
                                }
                            }
                        }
                    }

                    HtmlTableCell htcEntryForm = (HtmlTableCell)CurrentPage.FindControl("pnlCPGV1Title").FindControl("htcCPGV1");
                    HtmlTableCell htcEntryFormAuto = (HtmlTableCell)CurrentPage.FindControl("pnlCPGV1Title").FindControl("htcCPGV1Auto");

                    pnlCPGV1Title.Visible = true;
                    if (xDoc.SelectSingleNode("Root/bpeout/FormControls/PageSubject") != null)
                    {
                        string pageSubject = xDoc.SelectSingleNode("Root/bpeout/FormControls/PageSubject").InnerText;
                        commonObjUI.SetPanelHeading(htcEntryForm, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Title").InnerText + " >>> " + pageSubject);
                        //commonObjUI.SetPanelHeading(htcEntryForm, "Shanti" + " >>> " + "dont know");
                    }
                    else
                    {
                        commonObjUI.SetPanelHeading(htcEntryForm, xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Title").InnerText);
                    }

                    
                    htcEntryFormAuto.Width = Convert.ToString(825 + 50 + 24 - Convert.ToInt32(htcEntryForm.Width));//IframeWidth + BtnUCWidth +SpacerWidth-htcEntryForm.Width. need to change.
                    
                    //Set the heights of Entry Form
                    pnlEntryForm.Height = Unit.Pixel(487);//(511-24(for the title element))
                    //Find the immediate table in the Entry Panel
                    HtmlTable tblEntryForm = (HtmlTable)pnlEntryForm.FindControl("tblEntryForm");
                    if (tblEntryForm != null)
                    {
                        tblEntryForm.Style["height"] = pnlEntryForm.Height.ToString();
                    }

                    //Set the Page Title for the current page.
                    ((Page)CurrentPage.TemplateControl).Title = xDoc.SelectSingleNode("Root/bpeout/FormInfo/Title").InnerText;
                }
                else
                {
                    if (pnlEntryFormTitle != null)
                    {
                        pnlEntryFormTitle.Visible = false;
                    }
                    pnlEntryForm.Visible = false;
                    Panel pnlContentError = (Panel)CurrentPage.FindControl("pnlContentError");
                    pnlContentError.Visible = true;
                    Label lblError = (Label)CurrentPage.FindControl("lblError");
                    XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                    if (nodeMsg != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                        else if (nodeMsg.SelectSingleNode("Label") != null)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else
                    {
                        lblError.Text = nodeMsgStatus.InnerText;
                    }
                }
            }
        }        
    }
}