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
using System.Xml;



namespace LAjitDev.Common
{
    public partial class ChartViewer : System.Web.UI.Page
    {
        LAjit_BO.Reports reportsBO = new LAjit_BO.Reports();
        CommonUI commonObjUI = new CommonUI();
        XmlDocument XDocUserInfo = new XmlDocument();

        public string ChartFileName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Register ajax
                Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
                if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
                {
                    commonObjUI.InjectSessionExpireScript(this);
                }
                if (!Page.IsPostBack)
                {

                    //Adding javascript
                    AddScriptReferences();

                    AddCSSReferences();
                    if (Session["LinkBPinfo"] != null)
                    {
                        string BPInfo = Session["LinkBPinfo"].ToString();
                        string pageSize = string.Empty;

                        if (Page.Request.Params["page"] != null)
                        {
                            //Page request from grid btoom link
                            pnlChart.Visible = false;

                            string pageNumber = Page.Request.Params["page"];
                            if (pageNumber == "all" || pageNumber == "all?PopUp=PopUp")
                            {
                                pageSize = "-1";
                            }

                            //Enable entry controls
                            XYDDLEntryform.Visible = true;

                            string reqXML = GenerateRequestXML(BPInfo, pageSize);
                            string returnXML = reportsBO.GetReportBPEOut(reqXML);
                            //Set View State
                            ViewState["returnXML"] = returnXML;
                            //Fill Dropdowndata
                            FillXYDropdownData(returnXML);

                            if (ddlXAxis.Items.Count == 0 || ddlYAxis.Items.Count == 0)
                            {
                                //Reload header
                                htcCPGV1Auto.InnerHtml = "&nbsp;";
                                //htcEntryFormAuto.InnerHtml = "&nbsp;";
                                //Disable dropdownlist entry if items are empty
                                XYDDLEntryform.Visible = false;
                                pnlEntryForm.Visible = false;
                                pnlContentError.Visible = true;
                                lblError.Text = "The Chart cannot be generated for this Grid";
                            }
                            else
                            {
                                XYDDLEntryform.Visible = true;
                                pnlContentError.Visible = false;
                                pnlEntryForm.Visible = true;

                                //Preselect chart type based on preference
                                SetChartType();

                                //Preselect dropdownlist and load chart
                                if ((Page.Request.Params["x"] != null) && (Page.Request.Params["y"] != null))
                                {
                                    //Preselect X and Y Dropdownlist
                                    ddlXAxis.SelectedValue = Page.Request.Params["x"].ToString();
                                    ddlYAxis.SelectedValue = Page.Request.Params["y"].ToString();

                                    //Load chart
                                    GenerateChart(false, false);

                                }

                            }

                            btnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/submit-but.png"; ;
                        }
                        else
                        {
                            //Page request from chart hyperlinks to show chart directly
                            //Disable entry controls
                            XYDDLEntryform.Visible = false;
                            string reqXML = GenerateRequestXML(Session["LinkBPinfo"].ToString(), "-1");
                            string returnXML = reportsBO.GetReportBPEOut(reqXML);
                            ViewState["returnXML"] = returnXML;

                            XmlDocument xDocOut = new XmlDocument();
                            xDocOut.LoadXml(returnXML);
                            //Set Page header
                            SetUIHeaders(updtPnlContent, pnlEntryForm, xDocOut);
                            //Set Chart GVDataXML
                            ((UserControls.ChartUserControl)CUC).GVDataXml = returnXML;
                            // Based on chartusercontrol visiblity show or hide Export PDF
                            ShowHideExportPDF();
                        }
                    }
                }

            }
            catch
            {

            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["USERINFOXML"] != null)
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                //string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
                string keyTheme = "430";
                if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo") != null)
                {
                    string theme = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                    Session.Add("MyTheme", theme);
                }
               // Page.Theme = Session["MyTheme"].ToString();
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            //If relevant sessions have not been initialised, redirect the user to the Login Page.
            //To handle the case where the user directly types in a page name other than Login and ends up with an error.
            if (HttpContext.Current.Session["USERINFOXML"] == null && Context.Session != null && !HttpContext.Current.Session.IsNewSession)
            {
                Response.Redirect("Login.aspx");
            }

            if (Context.Session != null)
            {
                //Tested and the IsNewSession is more advanced then simply checking if 
                // a cookie is present, it does take into account a session timeout,
                if (Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must 
                    // have timed out.
                    string szCookieHeader = Request.Headers["Cookie"];
                    if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        Response.Redirect("../Common/SessionExpire.aspx");
                    }
                }
            }
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Set Current Imagename
            if (((UserControls.ChartUserControl)CUC).ChartCurrentImage != null)
            {
                ChartFileName = ((UserControls.ChartUserControl)CUC).ChartCurrentImage.ToString();
                if (ChartFileName != string.Empty)
                { //Set only filename without extension
                    ChartFileName = System.IO.Path.GetFileNameWithoutExtension(ChartFileName).ToString();
                    hdnChartCFN.Value = ChartFileName;
                }
            }
        }
        #region PrivateMethods

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink css = new HtmlLink();
            css.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            css.Attributes["rel"] = "stylesheet";
            css.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css);
        }

        private void AddScriptReferences()
        {
            //CDN Added Scripts
           
            //jquery-1.3.2.min.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery-1.3.2.min.js");
            Page.Header.Controls.Add(hgcScript1);

            //jquery.dialog.js
            HtmlGenericControl hgcScript2 = new HtmlGenericControl();
            hgcScript2.TagName = "script";
            hgcScript2.Attributes.Add("type", "text/javascript");
            hgcScript2.Attributes.Add("language", "javascript");
            hgcScript2.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "jquery.dialog.js");
            Page.Header.Controls.Add(hgcScript2);

            //Utility.js
            HtmlGenericControl hgcScript3 = new HtmlGenericControl();
            hgcScript3.TagName = "script";
            hgcScript3.Attributes.Add("type", "text/javascript");
            hgcScript3.Attributes.Add("language", "javascript");
            hgcScript3.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Utility.js");
            Page.Header.Controls.Add(hgcScript3);

            //Common.js
            HtmlGenericControl hgcScript4 = new HtmlGenericControl();
            hgcScript4.TagName = "script";
            hgcScript4.Attributes.Add("type", "text/javascript");
            hgcScript4.Attributes.Add("language", "javascript");
            hgcScript4.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Common.js");
            Page.Header.Controls.Add(hgcScript4);

            //FrameManager.js
            HtmlGenericControl hgcScript5 = new HtmlGenericControl();
            hgcScript5.TagName = "script";
            hgcScript5.Attributes.Add("type", "text/javascript");
            hgcScript5.Attributes.Add("language", "javascript");
            hgcScript5.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "FrameManager.js");
            Page.Header.Controls.Add(hgcScript5);

            //ChartUserControl.js
            HtmlGenericControl hgcScript6 = new HtmlGenericControl();
            hgcScript6.TagName = "script";
            hgcScript6.Attributes.Add("type", "text/javascript");
            hgcScript6.Attributes.Add("language", "javascript");
            hgcScript6.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ChartUserControl.js");
            Page.Header.Controls.Add(hgcScript6);
        }


        private void SetChartType()
        {
            string ChartType = string.Empty;
            //Chart Preference
            ChartType = commonObjUI.GetPreferenceValue("801");
            if (ChartType != string.Empty)
            {
                ddlChartTypes.SelectedValue = ChartType;
            }
        }

        private void ShowHideExportPDF()
        {
            if (((UserControls.ChartUserControl)CUC).FindControl("pnlErrmsg").Visible)
            {
                pnlExportPDF.Visible = false;
                imgbtnExportPDF.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-pdf.png";
            }
            else
            {
                pnlExportPDF.Visible = true;
                imgbtnExportPDF.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/grid-pdf.png";
            }
        }

        private string GenerateRequestXML(string requestXML, string pageSize)
        {
            //Creating the Root and bpe node
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml); ;
            // nodeRoot.InnerXml = HttpContext.Current.Session["BPE"].ToString();

            //Creating the bpinfo node
            nodeRoot.InnerXml += requestXML;

            XmlNode nodePageSize = nodeRoot.SelectSingleNode("bpinfo//Pagesize");
            if (nodePageSize != null)
            {
                if (nodePageSize.InnerText != null && pageSize != null)
                {
                    nodePageSize.InnerXml = pageSize;
                }
                if (pageSize == "-1")
                {
                    XmlNode nodePageNo = nodeRoot.SelectSingleNode("bpinfo//Pagenumber");
                    nodePageNo.InnerXml = "";
                }
            }
            return nodeRoot.OuterXml;
        }

        private void FillXYDropdownData(string returnXML)
        {
            string m_GVTreeNodeName = string.Empty;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);

            XmlNode nodeStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");

            if (nodeStatus != null && nodeStatus.InnerText == "Error")
            {
                return;
            }

            //Clear dropdownlist
            ddlXAxis.Items.Clear();
            ddlYAxis.Items.Clear();

            //Get the Grid Layout nodes
            m_GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            //Set Page header
            SetUIHeaders(updtPnlContent, pnlEntryForm, xDoc);

            int colCntr = 0;
            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

            foreach (XmlNode colNode in nodeColumns.ChildNodes)
            {
                int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)//All displayable columns processing...
                {
                    //Add the Column Length
                    XmlAttribute attControlType = colNode.Attributes["ControlType"];
                    if (attControlType != null)
                    {

                        if ((attControlType.Value == "Amount") || (attControlType.Value == "Calc"))
                        {
                            //Y AXIS
                            ddlYAxis.Items.Add(new ListItem(colNode.Attributes["Caption"].Value, colNode.Attributes["Label"].Value));

                        }
                        else
                        {
                            //X AXIS
                            ddlXAxis.Items.Add(new ListItem(colNode.Attributes["Caption"].Value, colNode.Attributes["Label"].Value));
                        }
                    }
                    colCntr++;
                }
            }

            //If y axis items default add  Count Item

            ddlYAxis.Items.Add(new ListItem("Count", "Count"));
        }

        private void SetUIHeaders(UpdatePanel updPanel, Panel pnlEntryForm, XmlDocument xDocOut)
        {
            HtmlTableCell htcEntryForm = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1");
            HtmlTableCell htcEntryFormAuto = (HtmlTableCell)updPanel.FindControl("pnlCPGV1Title").FindControl("htcCPGV1Auto");

            string m_treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Title").InnerText;

            commonObjUI.SetPanelHeading(htcEntryForm, headerTitle);
            if (pnlEntryForm.Page.MasterPageFile != null)
            {
                if (pnlEntryForm.Page.MasterPageFile.Contains("PopUp.Master"))
                {
                    //Modified By Danny.
                    int entryFormWidth = 876;//IframeWidth - BtnUCWidth -SpacerWidth.
                    int entryFormHeight = 483;//-24 for the title element

                    //string isPopUp = CurrentPage.Page.Request.QueryString["PopUp"];
                    string strDepth = pnlEntryForm.Page.Request.QueryString["depth"];
                    if (!string.IsNullOrEmpty(strDepth))
                    {
                        int depth = Convert.ToInt32(strDepth);
                        if (depth != 1 && depth != 0)
                        {
                            depth--;
                            entryFormWidth = entryFormWidth - (depth * 30);
                            entryFormHeight = entryFormHeight - (depth * 30);

                            ((UserControls.ChartUserControl)CUC).ChartHeight = Convert.ToString(entryFormHeight - 26);
                            ((UserControls.ChartUserControl)CUC).ChartWidth = Convert.ToString(entryFormWidth);


                        }
                    }

                    //Set the heights of Entry Form
                    pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
                    pnlEntryForm.Width = Unit.Pixel(entryFormWidth);

                    //Find the immediate table in the Entry Panel
                    HtmlTable tblEntryForm = (HtmlTable)pnlEntryForm.FindControl("tblEntryForm");
                    if (tblEntryForm != null)
                    {
                        tblEntryForm.Style["height"] = pnlEntryForm.Height.ToString();
                        tblEntryForm.Style["width"] = Convert.ToString(pnlEntryForm.Width.Value - 1);
                    }
                    ((UserControls.ChartUserControl)CUC).ChartHeight = "482";
                    ((UserControls.ChartUserControl)CUC).ChartWidth = "922";

                    //Rename the close hyperlink in the Popup frame according to the header text.s
                    string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + " Chart');";
                    ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
                }
                else
                {
                    // htcCPGV1Auto.Width = Convert.ToString(825 - Convert.ToInt32(htcCPGV1.Width) - 28);
                    htcEntryFormAuto.Width = Convert.ToString(825 - Convert.ToInt32(htcEntryForm.Width) - 40);

                    ((UserControls.ChartUserControl)CUC).ChartHeight = "482";
                    ((UserControls.ChartUserControl)CUC).ChartWidth = "900";

                }
            }
            else
            {
                int entryFormWidth = 927;//IframeWidth - BtnUCWidth -SpacerWidth.
                int entryFormHeight = 483;//-24 for the title element
                pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
                //pnlEntryForm.Width = Unit.Pixel(entryFormWidth-13);
                htcEntryFormAuto.Width = Convert.ToString(927 - Convert.ToInt32(htcEntryForm.Width) - 25);
                pnlEntryForm.Width = Unit.Pixel(entryFormWidth);

                //Set Chart Height based on Panel height
                if (XYDDLEntryform.Visible) //true
                {
                    ((UserControls.ChartUserControl)CUC).ChartHeight = "436"; //"456";
                }
                else
                {
                    ((UserControls.ChartUserControl)CUC).ChartHeight = "456"; //"456";
                }

                //Set Chart Width based on Panel width
                ((UserControls.ChartUserControl)CUC).ChartWidth = Convert.ToString(entryFormWidth - 6);

                //Rename the close hyperlink in the Popup frame according to the header text.s
                string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + " Chart');";
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
            }
        }

        private void GenerateChart(bool ChartBindStatus, bool ChartExportPDF)
        {

            try
            {

                //Load all required Chart Attributes
                Hashtable m_htChartAttb = new Hashtable();
                m_htChartAttb.Add("XaxisText", ddlXAxis.SelectedItem.Text);
                m_htChartAttb.Add("XaxisValue", ddlXAxis.SelectedItem.Value);
                m_htChartAttb.Add("YaxisValue", ddlYAxis.SelectedItem.Value);
                m_htChartAttb.Add("YaxisText", ddlYAxis.SelectedItem.Text);
                m_htChartAttb.Add("XaxisColor", "2");
                m_htChartAttb.Add("XaxisFont", "1");
                m_htChartAttb.Add("YaxisColor", "2");
                m_htChartAttb.Add("YaxisFont", "2");
                m_htChartAttb.Add("IsLegendDisplayed", "1");
                m_htChartAttb.Add("IsChartNameDisplayed", "1");
                m_htChartAttb.Add("ChartType", ddlChartTypes.SelectedItem.Value);


                //Check Return xml
                if (ViewState["returnXML"] != null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(ViewState["returnXML"].ToString());

                    string m_GVTreeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;



                    //Selected values
                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList");
                    //XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList/Rows [@"+ ddlYAxis.SelectedItem.Value +"]");

                    if (nodeRows == null)
                    {
                        pnlExportPDF.Visible = false;
                        pnlDataError.Visible = true;
                        lblErrMsg.Text = "Data not available.";
                        return;
                    }
                    else
                    {
                        pnlDataError.Visible = false;
                    }

                    if (nodeRows.ChildNodes.Count > 0)
                    {
                        //Adding the chart rows
                        XmlNodeList nodelist = nodeRows.ChildNodes;

                        for (int r = 0; r < nodelist.Count; r++)
                        {
                            XmlNode rowNode = nodelist[r];

                            if ((rowNode.Attributes[ddlYAxis.SelectedItem.Value] == null) && (ddlYAxis.SelectedItem.Value.ToUpper() != "COUNT"))
                            {
                                //Remove row if Y-Axis selected attribute is not availble
                                XmlNode removalNode = rowNode;
                                nodeRows.RemoveChild(rowNode);
                                r--;
                            }
                            else
                            {
                                for (int i = 0; i < rowNode.Attributes.Count; i++)
                                {
                                    foreach (DictionaryEntry de in m_htChartAttb)
                                    {
                                        switch (de.Key.ToString())
                                        {
                                            case "YaxisValue":
                                                {
                                                    XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                                    //if null remove
                                                    if (rowNode.Attributes[de.Value.ToString()] != null)
                                                    {
                                                        if ((rowNode.Attributes[de.Value.ToString()].Value != string.Empty))
                                                        {
                                                            attrNew.Value = rowNode.Attributes[de.Value.ToString()].Value;
                                                            rowNode.Attributes.Append(attrNew);
                                                        }
                                                    }
                                                    else if (de.Value.ToString().ToUpper() == "COUNT")
                                                    {
                                                        attrNew.Value = "1";
                                                        rowNode.Attributes.Append(attrNew);
                                                    }
                                                    else
                                                    {
                                                        nodeRows.RemoveChild(rowNode);
                                                        break;
                                                    }
                                                }
                                                break;
                                            case "XaxisValue":
                                                {
                                                    XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                                    if (rowNode.Attributes[de.Value.ToString()] != null)
                                                    {
                                                        attrNew.Value = rowNode.Attributes[de.Value.ToString()].Value;
                                                    }
                                                    rowNode.Attributes.Append(attrNew);
                                                }
                                                break;
                                            default:
                                                {
                                                    XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                                    attrNew.Value = de.Value.ToString();
                                                    rowNode.Attributes.Append(attrNew);
                                                }
                                                break;
                                        }  // Swich close

                                    }  // Dictonary loop
                                }
                            }
                        }
                    }
                    else if (nodeRows != null)
                    {
                        // Single Row
                        for (int i = 0; i < nodeRows.Attributes.Count; i++)
                        {
                            foreach (DictionaryEntry de in m_htChartAttb)
                            {
                                switch (de.Key.ToString())
                                {
                                    case "YaxisValue":
                                        {
                                            XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                            //if null remove

                                            if ((nodeRows.Attributes[de.Value.ToString()].Value != string.Empty) || (nodeRows.Attributes[de.Value.ToString()].Value != null))
                                            {
                                                attrNew.Value = nodeRows.Attributes[de.Value.ToString()].Value;
                                                nodeRows.Attributes.Append(attrNew);
                                            }
                                        }
                                        break;
                                    case "XaxisValue":
                                        {
                                            XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                            if (nodeRows.Attributes[de.Value.ToString()] != null)
                                            {
                                                attrNew.Value = nodeRows.Attributes[de.Value.ToString()].Value;
                                            }
                                            nodeRows.Attributes.Append(attrNew);
                                        }
                                        break;
                                    default:
                                        {
                                            XmlAttribute attrNew = xDoc.CreateAttribute(de.Key.ToString());
                                            attrNew.Value = de.Value.ToString();
                                            nodeRows.Attributes.Append(attrNew);
                                        }
                                        break;
                                }  // Swich close

                            }  // Dictonary loop
                        }


                    }

                    //Business Process links update on user selection

                    //ddlXAxis.SelectedItem.Text;

                    XmlNode nodeColumn = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns/Col [@Caption='" + ddlXAxis.SelectedItem.Text + "']");

                    if (nodeColumn != null)
                    {
                        string BPConrol = string.Empty;

                        BPConrol = nodeColumn.Attributes["BPControl"].Value.Trim();

                        if (BPConrol != string.Empty)
                        {
                            XmlNode nodeBPRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                            if (nodeBPRows != null)
                            {
                                foreach (XmlNode rowNode in nodeBPRows.ChildNodes)
                                {
                                    if (rowNode.Attributes[BPConrol] == null)
                                    {
                                        //Remove remaing processes
                                        nodeBPRows.RemoveChild(rowNode);
                                    }
                                }

                                //After removing all remaining row

                                XmlNode nodeBPSingleRow = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess");

                                if (nodeBPSingleRow != null)
                                {
                                    if (nodeBPSingleRow.Attributes["ID"].Value.ToLower().ToString() != "process1")
                                    {
                                        //update to process1
                                        nodeBPSingleRow.Attributes["ID"].Value = "Process1";
                                    }
                                }
                            }
                        }
                    }

                    //Show the Chatviewcontrol
                    pnlChart.Visible = true;
                    ((UserControls.ChartUserControl)CUC).GVDataXml = xDoc.OuterXml;
                    //((UserControls.ChartUserControl)CUC).ChartTemplateName = "Blue";
                    // ((UserControls.ChartUserControl)CUC).ChartHeight = "456";
                    // ((UserControls.ChartUserControl)CUC).ChartWidth = "898";
                    if (ChartBindStatus)
                    {
                        //BindChart
                        ((UserControls.ChartUserControl)CUC).BindChart();

                        //Set Page header again
                        SetUIHeaders(updtPnlContent, pnlEntryForm, xDoc);
                    }

                    if (ChartExportPDF)
                    {

                        ((UserControls.ChartUserControl)CUC).ExportTOPDF();

                    }

                    // Based on chartusercontrol visiblity show or hide Export PDF
                    ShowHideExportPDF();

                }

            }// Try close
            catch
            {
            }
        }

        #endregion

        protected void Submit_Click(object sender, EventArgs e)
        {
            GenerateChart(true, false);
        }



        protected void imgbtnExportPDF_Click(object sender, EventArgs e)
        {

            if ((bool)XYDDLEntryform.Visible)
            {
                //XY Dropdownlists are visiable
                GenerateChart(true, true);
            }
            else
            {
                //XY Dropdownlists not visiable
                if (ViewState["returnXML"] != null)
                {
                    string returnXML=ViewState["returnXML"].ToString();

                    XmlDocument xDocOut = new XmlDocument();
                    xDocOut.LoadXml(returnXML);
                  
                    //Set Chart GVDataXML
                    ((UserControls.ChartUserControl)CUC).GVDataXml = returnXML;

                    ((UserControls.ChartUserControl)CUC).GVDataXml = xDocOut.OuterXml;
                     //BindChart
                    ((UserControls.ChartUserControl)CUC).BindChart();

                    
                    ((UserControls.ChartUserControl)CUC).ExportTOPDF();
                }

            }
        }

    }
}
