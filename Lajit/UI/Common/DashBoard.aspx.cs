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
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Net;
using System.IO;
using System.Drawing;
using WebChart;
using Dundas.Charting.WebControl;
using LAjit_BO;
using NLog;


namespace LAjitDev
{
    public partial class DashBoard : LAjitDev.Classes.BasePage
    {
       // public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        int m_IndexOfIsProtected;
        int m_IndexOfIsActive;
        int m_IndexOfIsSecured;
        int m_IndexOfIsNoted;
        int m_IndexOfIsChangedHistory;
        int m_IndexOfIsAttached;
        int m_CenterPnlWidth = 545;
        bool m_IsFirstRun = true;
        bool m_IsRowAlternating = false;
        string m_BPCXml = string.Empty;
        string m_BusinessRules = string.Empty;
        //CommonBO objBO = new CommonBO();
        Hashtable m_htBPCColumns = new Hashtable();
        Hashtable m_htBPCntrls = new Hashtable();
        //Contains the column names and their corresponding index values in the gridview.
        private Hashtable m_htGVColumns = new Hashtable();
        //Contains the column full view and small view lenghts.
        private Hashtable m_htGVColWidths = new Hashtable();

        /// <summary>
        /// Contains the Session["GBPC"]/BusinessProcessControls xpath string.
        /// </summary>
        string m_GBPCXml = string.Empty;
        string m_GVTreeNodeName = string.Empty;
        string m_primaryKeyFieldName = string.Empty;
        string m_CurrentBPGID = string.Empty;
        string m_CurrentPageInfo = string.Empty;
        string m_GVClientID = string.Empty;
        private string m_RowHoverColour = string.Empty;
        XmlDocument XDocUserInfo = new XmlDocument();
        /// <summary>
        /// Page Initialise event to initialise necessary event handlers.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Page Load event of the page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax.Utility.RegisterTypeForAjax(typeof(DashBoard));
            //Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));//For ModRequest.
            string returnXML = string.Empty;
            string graphicsPnlBPGID = string.Empty;
            if (!Page.IsPostBack)
            {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                //Check whether redirected from Login page or PreDashboard page.
                //if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("PreDashboard.aspx"))
                //if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.ToUpper().Contains("PREDASHBOARD.ASPX"))
                if (Request.UrlReferrer != null && (Request.QueryString.Count > 0))
                {
                    XDocUserInfo.LoadXml(UpdateUserRoleInfo(XDocUserInfo));
                    XDocUserInfo.Save(Session["USERINFOXML"].ToString());
                    //Session["GBPC"] = xDocUpdateRoles.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml;
                    //Session["BPE"] = xDocUpdateRoles.SelectSingleNode("Root/bpe").OuterXml;
                    //Session["FORMINFO"] = xDocUpdateRoles.SelectSingleNode("Root/bpeout/FormInfo").OuterXml;
                }

                //Setting the page title.
                //XmlDocument xDocTitle = new XmlDocument();
                //xDocTitle.LoadXml(Convert.ToString(Session["FORMINFO"]));
                Page.Title = XDocUserInfo.SelectSingleNode("Root/bpeout/FormInfo/Title").InnerText;


                //Set Theme and CDNImagesPath
                string keyTheme = "430";
                if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo") != null)
                {
                    string theme = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                    Session.Add("MyTheme", theme);
                }
                else
                {
                    Session.Add("MyTheme", "LAjit");
                }

                SetCDNImagesPath();

                SetEntityCompanyPath();

                //Set the Session[BPEINFO] with the current values-For ModificationRequest functionality
                string strBpInfo = "<bpinfo>" + XDocUserInfo.SelectSingleNode("Root/bpeout/FormInfo/BPGID").OuterXml + "</bpinfo>";
                Session["BPINFO"] = strBpInfo;
                Session["LinkBPinfo"] = strBpInfo;
                parentBPInfo.Value = strBpInfo;

                //Check the Session if previously collapsed.If collapsed then the update the with to the new one.
                if (Session["LPCollapsed"] != null && Session["LPCollapsed"].ToString() == "1")
                {
                    m_CenterPnlWidth = 642;
                }

                //Below variables apply both for the Center Panel as well as the Right Panels
                const int COMBINEDPAGESIZE = 10;
                int noOfGVsToDisplay = 0;
                //DashBoardCPGV1
                string CPGV1BPGID = GetBPGID("41", "Center", XDocUserInfo);
                //DashBoardCPGV2
                string CPGV2BPGID = GetBPGID("42", "Center", XDocUserInfo);
                //DashBoardGraphicsPnl
                graphicsPnlBPGID = GetBPGID("436", "Center", XDocUserInfo);

                //if (graphicsPnlBPGID != string.Empty)
                //{
                //    noOfGVsToDisplay++;
                //}

                //CPGraphics
                if (graphicsPnlBPGID != string.Empty)
                {
                    returnXML = objBO.GetDataForCPGV1(GenerateGraphicsRequestXML(graphicsPnlBPGID, XDocUserInfo));
                    //CUC1.GVDataXml = returnXML;
                    //CUC1.ChartTemplateName = "Blue";
                    RenderGraphicsPanel(returnXML);
                }
                else
                {
                    pnlCPGraphics.Visible = false;
                    pnlCPGraphicsContent.Visible = false;
                }

                if (CPGV1BPGID != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnCPGV1FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnCPGV1FV.OnClientClick = "return ShowDetailedView('" + CPGV1BPGID + "', '20');return false;";
                }
                if (CPGV2BPGID != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnCPGV2FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnCPGV2FV.OnClientClick = "return ShowDetailedView('" + CPGV2BPGID + "', '20');return false;";
                }

                int newPageSize = 0;
                if (noOfGVsToDisplay > 0)
                {
                    newPageSize = Convert.ToInt32(Math.Ceiling((float)COMBINEDPAGESIZE / noOfGVsToDisplay));
                    grdvCenterPanel1.PageSize = newPageSize;
                    grdvCenterPanel2.PageSize = newPageSize;
                }
                BindCPGV1(XDocUserInfo);
                BindCPGV2(XDocUserInfo);
                // Handling the right panel's gridview's page size dynamically depending upon the number of gridviews
                // being displayed.
                noOfGVsToDisplay = 0;
                //DashBoardRPGV1
                string BPGIDRPGV1 = GetBPGID("46", "Right", XDocUserInfo);
                //DashBoardRPGV2
                string BPGIDRPGV2 = GetBPGID("47", "Right", XDocUserInfo);
                //DashBoardRPGV3
                string BPGIDRPGV3 = GetBPGID("48", "Right", XDocUserInfo);
                //DashBoardRPGV4
                string BPGIDRPGV4 = GetBPGID("49", "Right", XDocUserInfo);
                string BPGIDRPGV5 = GetBPGID("DashBoardRPGV5", "Right", XDocUserInfo);


                if (BPGIDRPGV1 != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnRPGV1FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnRPGV1FV.OnClientClick = "return ShowDetailedView('" + BPGIDRPGV1 + "', '20');return false;";
                }
                if (BPGIDRPGV2 != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnRPGV2FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnRPGV2FV.OnClientClick = "return ShowDetailedView('" + BPGIDRPGV2 + "', '20');return false;";
                }
                if (BPGIDRPGV3 != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnRPGV3FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnRPGV3FV.OnClientClick = "return ShowDetailedView('" + BPGIDRPGV3 + "', '20');return false;";
                }
                if (BPGIDRPGV4 != string.Empty)
                {
                    noOfGVsToDisplay++;
                    lnkBtnRPGV4FV.Attributes["oncontextmenu"] = "return false;";
                    lnkBtnRPGV4FV.OnClientClick = "return ShowDetailedView('" + BPGIDRPGV4 + "', '20');return false;";
                }
                if (noOfGVsToDisplay != 0)
                {
                    newPageSize = Convert.ToInt32(Math.Ceiling((float)COMBINEDPAGESIZE / noOfGVsToDisplay));
                    grdVwRightPanel1.PageSize = newPageSize;
                    grdVwRightPanel2.PageSize = newPageSize;
                    grdVwRightPanel3.PageSize = newPageSize;
                    grdVwRightPanel4.PageSize = newPageSize;
                }

                //RPGV1
                if (BPGIDRPGV1 == string.Empty)
                {
                    pnlRPGV1.Visible = false;
                    //pnlRPGV1PopUp.Visible = false;
                }
                else
                {
                    returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGIDRPGV1, grdVwRightPanel1.PageSize.ToString(), XDocUserInfo));
                    bool done = DisplayFoundResults(returnXML, grdVwRightPanel1, htcRPGV1);
                    if (!done)
                    {
                        pnlRPGV1.Visible = false;
                    }
                }

                //RPGV2
                if (BPGIDRPGV2 == string.Empty)
                {
                    pnlRPGV2.Visible = false;
                    //pnlRPGV2PopUp.Visible = false;
                }
                else
                {
                    returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGIDRPGV2, grdVwRightPanel2.PageSize.ToString(), XDocUserInfo));
                    bool done = DisplayFoundResults(returnXML, grdVwRightPanel2, htcRPGV2);
                    if (!done)
                    {
                        pnlRPGV2.Visible = false;
                    }
                }

                //RPGV3
                if (BPGIDRPGV3 == string.Empty)
                {
                    pnlRPGV3.Visible = false;
                    //pnlRPGV3PopUp.Visible = false;
                }
                else
                {
                    returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGIDRPGV3, grdVwRightPanel3.PageSize.ToString(), XDocUserInfo));
                    bool done = DisplayFoundResults(returnXML, grdVwRightPanel3, htcRPGV3);
                    if (!done)
                    {
                        pnlRPGV3.Visible = false;
                    }
                }

                //RPGV4
                if (BPGIDRPGV4 == string.Empty)
                {
                    pnlRPGV4.Visible = false;
                    //pnlRPGV4PopUp.Visible = false;
                }
                else
                {
                    returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGIDRPGV4, grdVwRightPanel4.PageSize.ToString(), XDocUserInfo));
                    bool done = DisplayFoundResults(returnXML, grdVwRightPanel4, htcRPGV4);
                    if (!done)
                    {
                        pnlRPGV4.Visible = false;
                    }
                }
                //RPGV5
                //Display RSS Feeds
                string showFeeds = commonObjUI.GetPreferenceValue("438");

                try
                {
                    if (Convert.ToInt32(showFeeds) == 1)
                    {
                        pnlRPGV5.Visible = true;
                        BindRSSFeeds();
                    }
                    else
                    {
                        pnlRPGV5.Visible = false;
                    }
                    SetPanelHeading(htcRPGV5, "Latest Headlines");
                }
                catch (Exception ex)
                {
                    #region NLog
                    logger.Fatal(ex);
                    #endregion
                }
                InitialiseCollapsibles();

            }
        }

        protected void timerRSSUpdater_Tick(object sender, EventArgs e)
        {
            //BindRSSFeeds();
        }

        public void BindRSSFeeds()
        {
            #region NLog
            logger.Info("Binding RSSFeeds.");
            #endregion

            try
            {
                /*if (timerRSSUpdater.Enabled == false)
                {
                    string refreshRate = GetUserPreferenceValue("RSS Refresh Rate");
                    //refreshRate = "2";
                    if (refreshRate != string.Empty)
                    {
                        timerRSSUpdater.Enabled = true;
                        //Assigning the timer's interval in milliseconds.(Assuming input is in seconds)
                        timerRSSUpdater.Interval = Convert.ToInt32(refreshRate) * 1000;
                    }
                    else
                    {
                        timerRSSUpdater.Enabled = false;
                    }
                }*/
                //RSS Feed 1
                string rssURL = commonObjUI.GetPreferenceValue("440");
                rssURL = rssURL.ToLower().ToString();
                if (rssURL.Contains(" "))
                {
                    rssURL = rssURL.Replace(" ", "_");
                }
                if (rssURL != null || rssURL != "NONE")
                {
                    WebRequest myRequest = WebRequest.Create(rssURL);
                    WebResponse myResponse = myRequest.GetResponse();
                    DataSet dsXMLReadRowsNew = new DataSet();
                    Stream rssStream = myResponse.GetResponseStream();
                    XmlDocument rssDoc = new XmlDocument();
                    XmlNode nodeRows;
                    XmlNodeList m_XnodeList;
                    rssDoc.Load(rssStream);
                    m_XnodeList = rssDoc.SelectNodes("rss/channel/item");
                    if (m_XnodeList.Count > 0)
                    {
                        string m_XEleName = rssDoc.SelectSingleNode("rss/channel/item").Name;
                        nodeRows = rssDoc.CreateElement(m_XEleName);
                        foreach (XmlNode xnode in m_XnodeList)
                        {
                            nodeRows.AppendChild(xnode);
                        }
                        //if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                        //{
                        //    XmlNodeReader read = new XmlNodeReader(nodeRows);
                        //    dsXMLReadRowsNew.ReadXml(read);
                        //}
                        string m_Text = string.Empty;
                        string m_desc = string.Empty;
                        string m_imgLogo = string.Empty;

                        m_imgLogo = rssDoc.SelectSingleNode("//channel/image/url").InnerText;

                        for (int i = 0; i < nodeRows.ChildNodes.Count; i++)
                        {
                            string title = string.Empty;
                            string link = string.Empty;
                            string description = string.Empty;

                            XmlNode rssDetail;
                            rssDetail = m_XnodeList.Item(i).SelectSingleNode("title");
                            if (rssDetail != null)
                            {
                                title = rssDetail.InnerText;
                            }
                            else
                            {
                                title = "";
                            }
                            rssDetail = m_XnodeList.Item(i).SelectSingleNode("link");
                            if (rssDetail != null)
                            {
                                link = rssDetail.InnerText;
                            }
                            else
                            {
                                link = "";
                            }
                            //rssDetail = m_XnodeList.Item(i).SelectSingleNode("description");
                            //if (rssDetail != null)
                            //{
                            //    description = rssDetail.InnerText;
                            //}
                            //else
                            //{
                            //    description = "";
                            //}
                            m_Text = m_Text + "<tr><td align='left'><b><a href='" + link + "' target='new'>" + title + "</a></b><br/></td></tr>";
                            //m_desc = m_Text + description + "</p>";
                        }
                        litRss.Text = "<table><tr><td align='left'><img src='" + m_imgLogo + "'/></td></tr>" + "<br/>" + m_Text + "</table>";
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                //Classes.ErrorLogger.LogError(ex.Message.ToString(), Classes.LogType.Mail);
            }
        }

        private void SetCDNImagesPath()
        {
            string strImagesCDNPath = ConfigurationManager.AppSettings["ImagesCDNPath"].ToString() + Session["MyTheme"].ToString() + "/";
            Application["ImagesCDNPath"] = strImagesCDNPath;
        }

        private void SetEntityCompanyPath()
        {
            string companyID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText;
            string entityID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/EntityID").InnerText;
            Session["CompanyEntityID"] = entityID + "/" + companyID;
        }



        /// <summary>
        /// Initialises the image url's for the collapsible panel extenders.
        /// </summary>
        private void InitialiseCollapsibles()
        {
            #region NLog
            logger.Info("Initialises the image url's for the collapsible panel extenders.");
            #endregion

            string currentTheme = Convert.ToString(Session["MyTheme"]);
            pnlExpView.Style.Add("visibility", "hidden");
            pnlExpView.Style.Add("height", "0px");
            pnlExpViewContent.Style.Add("visibility", "hidden");
            pnlExpViewContent.Style.Add("height", "0px");
            pnlExpViewContent.BackImageUrl = Application["ImagesCDNPath"].ToString() + "Images/predash-bg.gif";


        }

        /// <summary>
        /// Initialises and binds the data for CenterPanel Grid View 1.
        /// </summary>
        private string BindCPGV1(XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Initializing and binds the data for CenterPanel Grid View 1.");
            #endregion

            //DashBoardCPGV1
            string currentBPGID = GetBPGID("41", "Center", XDocUserInfo);
            if (currentBPGID == string.Empty)
            {
                //Dont display the griview panel.
                pnlCPGV1.Visible = false;
                pnlCPGV1Content.Visible = false;
                //pnlCPGV1PopUp.Visible = false;
                return "";
            }
            string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(currentBPGID, grdvCenterPanel1.PageSize.ToString(), XDocUserInfo));
            //Initialise Hidden input variable here for POST functionality.
            bool done = DisplayFoundResults(returnXML, grdvCenterPanel1, htcCPGV1);
            if (!done)
            {
                pnlCPGV1.Visible = false;
                pnlCPGV1Content.Visible = false;
            }
            //else
            //{
            //    htcCPGV1Auto.Width = Convert.ToString(m_CenterPnlWidth - Convert.ToInt32(htcCPGV1.Width) - 28);
            //}

            #region NLog
            logger.Info("Returning current BPGID as : " + currentBPGID); 
            #endregion 

            return currentBPGID;
        }

        /// <summary>
        /// Initialises and binds the data for CenterPanel Grid View 2.
        /// </summary>
        private string BindCPGV2(XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Initializing and binds the data for CenterPanel Grid View 2.");
            #endregion

            //DashBoardCPGV2
            string currentBPGID = GetBPGID("42", "Center", XDocUserInfo);
            if (currentBPGID == string.Empty)
            {
                //Dont display the griview panel.
                pnlCPGV2.Visible = false;
                pnlCPGV2Content.Visible = false;
                return string.Empty;
            }
            string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(currentBPGID, grdvCenterPanel2.PageSize.ToString(), XDocUserInfo));
            bool done = DisplayFoundResults(returnXML, grdvCenterPanel2, htcCPGV2);
            if (!done)
            {
                pnlCPGV2.Visible = false;
                pnlCPGV2Content.Visible = false;
            }
            //else
            //{
            //    htcCPGV2Auto.Width = Convert.ToString(m_CenterPnlWidth - Convert.ToInt32(htcCPGV2.Width) - 28);
            //}

            #region NLog
            logger.Info("Returning current BPGID as : " + currentBPGID);
            #endregion 

            return currentBPGID;
        }

        /// <summary>
        /// Binds the specified grid view with the given data.
        /// </summary>
        /// <param name="returnXML">The XML data consisting of Column and row data.</param>
        /// <param name="grdVwToBind"></param>
        public bool DisplayFoundResults(string returnXML, GridView grdVwToBind, HtmlTableCell htcCurrent)
        {
            #region NLog
            logger.Info("Binding the GridView " + grdVwToBind.ID);
            #endregion

            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo.LoadXml(returnXML);

            ////Re-initialising the Session["PriorFormInfo"] variable with most recently called BPID's return xml's FORMInfo.
            //Session["PriorFormInfo"] = xDoc.SelectSingleNode("Root/bpeout/FormInfo").OuterXml;
            XmlNode nodeError = XDocUserInfo.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            if (nodeError != null && nodeError.InnerText == "Error")
            {
                return false;
            }

            //Initialising the variables m_CurrentBPGID and m_CurrentPageInfo for the view details functionality
            m_CurrentBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            m_CurrentPageInfo = XDocUserInfo.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
            //Keep one variable for the entire formInfo..
            hdnFormInfo.Value = m_CurrentBPGID + "~::~" + m_CurrentPageInfo;
            grdVwToBind.Attributes.Add("FormInfo", m_CurrentBPGID + "~::~" + m_CurrentPageInfo);

            //Get the Grid Layout nodes
            m_GVTreeNodeName = XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText.Trim();

            //Setting the title of the grid view container panel.
            SetPanelHeading(htcCurrent, XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Title").InnerText);
            //Getting the dataset to be bound to the grid.
            XmlNode nodeRows = XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList");
            if (nodeRows == null)
            {
                return false;
            }
            XmlNodeReader read = new XmlNodeReader(nodeRows);
            DataSet dsRows = new DataSet();
            dsRows.ReadXml(read);

            ArrayList arrAmountCols = new ArrayList();
            m_htBPCntrls.Clear();
            m_htBPCColumns.Clear();
            m_htGVColumns.Clear();
            m_htGVColWidths.Clear();
            int colCntr = 0;
            int gridColumnCount = new int();
            uint colFVWidths = new uint();
            int gridTotalUsableWidth = 100;//In percentage 

            //Get the Columns Node from the XML.
            XmlNode nodeColumns = XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                uint colFullViewLength = Convert.ToUInt32(nodeCol.Attributes["SmallViewLength"].Value);
                if (colFullViewLength != 0)
                {
                    gridColumnCount++;
                    colFVWidths += colFullViewLength;
                }
            }

            //Creating the Columns.
            if (grdVwToBind.Columns.Count <= 1)
            {
                foreach (XmlNode colNode in nodeColumns.ChildNodes)
                {
                    int smallViewLength = Convert.ToInt32(colNode.Attributes["SmallViewLength"].Value);
                    string label = colNode.Attributes["Label"].Value;

                    if (smallViewLength != 0)
                    {
                        //Adding the current column node the ROWS dataset if not present.
                        if (!dsRows.Tables[0].Columns.Contains(label))
                        {
                            DataColumn dcNew = new DataColumn(label, typeof(string));
                            dcNew.AllowDBNull = true;
                            dsRows.Tables[0].Columns.Add(dcNew);
                        }

                        //For tooltip functionality.
                        m_htGVColumns.Add(label, colCntr);
                        m_htGVColWidths.Add(colCntr, smallViewLength);

                        BoundField newField = new BoundField();
                        newField.DataField = label;
                        newField.HeaderText = colNode.Attributes["Caption"].Value;

                        BoundField dummyField = null;//A dummy field at the end of the columns to fill in the gap
                        bool addDummyField = false;

                        smallViewLength = Convert.ToInt32(((float)smallViewLength / (int)colFVWidths) * 100);

                        if (colCntr == gridColumnCount - 1)
                        {
                            //Last Column - Make it span the remaining width
                            newField.ItemStyle.Width = Unit.Percentage(smallViewLength);//Add the column normally
                            //Check for the no. of columns <= 4
                            if (gridTotalUsableWidth > colFVWidths)
                            {
                                //Add a new column with blank contents
                                dummyField = new BoundField();
                                dummyField.HeaderText = "";
                                dummyField.ItemStyle.Width = Unit.Percentage(gridTotalUsableWidth - colFVWidths);
                                //addDummyField = true;
                            }
                        }
                        else
                        {
                            //Columns excluding the last one.
                            newField.ItemStyle.Width = Unit.Percentage(smallViewLength);
                        }

                        //Initialising hash tables to store info about process column indices in the grid view and dataset.
                        //Added the "AND" condition to handle empty BPControl atttribute values - 21/05/08
                        XmlAttribute attBPControl = colNode.Attributes["BPControl"];
                        if (attBPControl != null && attBPControl.Value.Trim() != "")
                        {
                            m_htBPCntrls.Add(attBPControl.Value, GetColumnIndex(label, dsRows.Tables[0]));
                            m_htBPCColumns.Add(attBPControl.Value, colCntr);
                        }

                        //Set justification to Right-Justify for columns with ControlType as Amount
                        if (colNode.Attributes["ControlType"] != null && colNode.Attributes["ControlType"].Value == "Amount")
                        {
                            newField.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            newField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            newField.ItemStyle.CssClass = "grdVwColRightJustify";
                            arrAmountCols.Add(label);
                        }

                        //Add the fieds now
                        grdVwToBind.Columns.Add(newField);
                        if (addDummyField)
                        {
                            grdVwToBind.Columns.Add(dummyField);
                        }

                        //Adding the current column node the ROWS dataset if not present.
                        if (!dsRows.Tables[0].Columns.Contains(label))
                        {
                            DataColumn dcNew = new DataColumn(label, typeof(string));
                            dcNew.AllowDBNull = true;
                            dsRows.Tables[0].Columns.Add(dcNew);
                        }
                        ////Tracking all IsNumeric columns
                        //if (colNode.Attributes["IsNumeric"] != null && colNode.Attributes["IsNumeric"].Value == "1")
                        //{
                        //    arrIsNumericCols.Add(label);
                        //}
                        colCntr++;
                    }
                }
            }

            //foreach (XmlNode colNode in nodeColumns.ChildNodes)
            //{
            //    //Adding the current column node the ROWS dataset if not present.
            //    if (!dsRows.Tables[0].Columns.Contains(label))
            //    {
            //        DataColumn dcNew = new DataColumn(label, typeof(string));
            //        dcNew.AllowDBNull = true;
            //        dsRows.Tables[0].Columns.Add(dcNew);
            //    }
            //    if (colNode.Attributes["IsNumeric"] != null && colNode.Attributes["IsNumeric"].Value == "1")
            //    {
            //        arrIsNumericCols.Add(label);
            //    }
            //}


            //Format the IsNumeric columns specified by the m_arrIsNumericCols object in the data source
            foreach (string colName in arrAmountCols)
            {
                int colIndex = dsRows.Tables[0].Columns[colName].Ordinal;
                foreach (DataRow dr in dsRows.Tables[0].Rows)
                {
                    decimal amount;
                    if (Decimal.TryParse(dr[colIndex].ToString(), out amount))
                    {
                        dr[colIndex] = string.Format("{0:N}", amount);
                    }
                }
            }


            //Initialsing variables to be used in OnRowDataBound
            string bpcXML = string.Empty;
            if (XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls") != null)
            {
                bpcXML = XDocUserInfo.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls").OuterXml;
            }
            else
            {
                bpcXML = string.Empty;
            }
            m_BPCXml = bpcXML;

            ////Initialising the Business rules used for ColumnHyperlinks enabling in the OnRowDataBound
            //if (xDoc.SelectSingleNode("Root/bpeout/BusinessRules") != null)
            //{
            //    m_BusinessRules = xDoc.SelectSingleNode("Root/bpeout/BusinessRules").OuterXml;
            //}
            //else
            //{
            //    m_BusinessRules = string.Empty;
            //}

            //xDoc = new XmlDocument();
            //xDoc.LoadXml(Session["GBPC"].ToString());
            //m_GBPCXml = xDoc.SelectSingleNode("GlobalBusinessProcessControls/BusinessProcessControls").OuterXml;
            //m_primaryKeyFieldName = dsRows.Tables[0].Columns[0].ColumnName;

            //Setting the Alternating Row Style of the GridView
            string isAlternating = string.Empty;
            //Center Panel gridviews should always consider the full view parameters.
            if (grdVwToBind.ID.Contains("CenterPanel"))
            {   //FullViewAlternatingStyle
                isAlternating = commonObjUI.GetPreferenceValue("56");
            }
            else
            {
                //SmallViewAlternatingStyle
                isAlternating = commonObjUI.GetPreferenceValue("55");
            }

            if (isAlternating == string.Empty)
            {
                //FullViewAlternatingStyle
                isAlternating = commonObjUI.GetPreferenceValue("56");
            }
            m_RowHoverColour = string.Empty;
            if (isAlternating == "1")
            {
                grdVwToBind.AlternatingRowStyle.CssClass = "AlternatingCOARowStyle";
                m_IsRowAlternating = true;
                m_RowHoverColour = ConfigurationManager.AppSettings["GridRowHoverColor"];
            }
            else
            {
                m_IsRowAlternating = false;
            }
            m_IsFirstRun = true;
            grdVwToBind.DataSource = dsRows;
            grdVwToBind.DataBind();
            return true;
        }

        /// <summary>
        /// Gets the position of the column in the given data table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="dtSearch">Data table to search in.</param>
        /// <returns>Integer column position, zero being the first column.</returns>
        /// <returns>Returns -1 if column is not found.</returns>
        private int GetColumnIndex(string columnName, DataTable dtSearch)
        {
            int colCntr = 0;
            foreach (DataColumn dc in dtSearch.Columns)
            {
                if (dc.ColumnName.ToUpper() == columnName.ToUpper())
                {
                    return colCntr;
                }
                colCntr++;
            }
            return -1;
            //throw new Exception("Specified column" + columnName + " not found in the data source!!");
        }

        /// <summary>
        /// Requests for updation of new role information which has been changed in the PreDashBoard Page.
        /// </summary>
        /// <returns>New updated GBPC and other stuff.</returns>
        private string UpdateUserRoleInfo(XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Requests for updation of new role information which has been changed in the PreDashBoard Page.");
            #endregion

            string roleCompanyID = Request.Params["rcId"].ToString();
            string roleUserID = Request.Params["ruId"].ToString();
            string BPGID = Request.Params["bpgId"].ToString();

            //Generating the request XML.
            //XmlNode nodeSelRole = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities//Entity//Companies//Company//Roles//Role[@RoleCompanyID = '" + roleCompanyID + "'][@UserRoleID = '" + roleUserID + "']");

            XmlNode nodeSelRole = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities//Entity//Companies//Company//Roles//Role[@RoleCompanyID = '" + roleCompanyID + "'][@RoleID = '" + roleUserID + "']");

            XmlDocument xDocGV = new XmlDocument();

            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            nodeRoot.InnerXml = strBPE;

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += nodeSelRole.OuterXml;
            nodeRoot.AppendChild(nodeBPInfo);

            //Requesting the change.
            return objBO.AddThisVendor(nodeRoot.OuterXml);
        }

        //<summary>
        //Renders the Center Graphics Panel in the DashBoard
        //</summary>
        //<param name="preferenceXml">The rendering XML</param>
        public void RenderGraphicsPanel(string preferenceXml)
        {
            //chrtPie.Visible = false;

            XmlDocument m_xdoc = new XmlDocument();

            m_xdoc.LoadXml(preferenceXml);

            string m_ChartGrphsType = string.Empty;
            XmlNode nodePageInfo = m_xdoc.SelectSingleNode("Root/bpeout/FormInfo/PageInfo");
            if (nodePageInfo != null)
            {
                m_ChartGrphsType = nodePageInfo.InnerText;
            }

            #region Charts

            if (m_ChartGrphsType != string.Empty)
            {

                string m_GVTreeNodeName = m_xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                XmlNodeList m_XnodeList = m_xdoc.SelectNodes("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList/Rows");

                // Rows count
                if (m_XnodeList.Count > 0)
                {
                    //Enable UserControl Chart
                    CUC1.Visible = true;
                    CUC1.GVDataXml = preferenceXml;
                    CUC1.ChartTemplateName = "Blue";
                }
                else
                {
                    //Disable UserControl Chart
                    CUC1.Visible = false;
                }
            }
            #endregion

            #region PanelGraphics
            else
            {
                int maxrows = 0;
                int maxcols = 0;
                int rows;
                int cols;
                string imageSrc = string.Empty;
                string image = string.Empty;
                //tblCPGV2Graphics.BorderWidth = Unit.Pixel(1);
                XmlDocument xDocPrf = new XmlDocument();
                xDocPrf.LoadXml(preferenceXml);
                tblCPGraphics.CellPadding = 2;
                tblCPGraphics.CellSpacing = 2;
                if (xDocPrf.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls") != null)
                {
                    XmlNode nodePreferenceInfo = xDocPrf.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                    //To find max number of rows and columns
                    foreach (XmlNode nodeSubPrefElement in nodePreferenceInfo.ChildNodes)
                    {
                        rows = Convert.ToInt32(nodeSubPrefElement.Attributes["ImgRow"].Value);

                        if (rows > maxrows)

                            maxrows = rows;

                        cols = Convert.ToInt32(nodeSubPrefElement.Attributes["ImgCol"].Value);

                        if (cols > maxcols)

                            maxcols = cols;

                    }

                    //creating table dynamically

                    for (int i = 0; i < maxrows; i++)
                    {

                        TableRow tblRw = new TableRow();

                        tblCPGraphics.Rows.Add(tblRw);

                        for (int j = 0; j < maxcols; j++)
                        {

                            TableCell tblCell = new TableCell();

                            tblCell.CssClass = "DshBrdGrapPnlLabelText";

                            tblRw.Cells.Add(tblCell);



                        }

                    }

                    //dynamically placing the images

                    foreach (XmlNode nodeSubPrefElement in nodePreferenceInfo.ChildNodes)
                    {

                        rows = Convert.ToInt32(nodeSubPrefElement.Attributes["ImgRow"].Value);

                        cols = Convert.ToInt32(nodeSubPrefElement.Attributes["ImgCol"].Value);

                        string toolTip = nodeSubPrefElement.Attributes["Label"].Value;

                        string pageInfo = nodeSubPrefElement.Attributes["PageInfo"].Value;

                        string BPGID = nodeSubPrefElement.Attributes["BPGID"].Value;

                        if (nodeSubPrefElement.Attributes["ImgSrc"] != null)
                        {
                            imageSrc = nodeSubPrefElement.Attributes["ImgSrc"].Value;
                            imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + imageSrc;
                        }
                        else
                        {
                            imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + "icon1.png";
                        }

                        imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + "icon1.png";

                        tblCPGraphics.Rows[rows - 1].Cells[cols - 1].Text = "<center><img src= " + imageSrc

                            + " alt='" + toolTip + "' onclick=\"OnGraphicsPnlLinkClick('"

                            + BPGID + "','" + pageInfo + "')\" onmouseout=\"toggleImage(this);\" onmouseover=\"toggleImage(this);\" style='cursor:pointer;vertical-align:middle' align='center'/></center>" + toolTip;

                    }

                }

            }

            #endregion

            //htcCPGraphicsAuto.Width = Convert.ToString(m_CenterPnlWidth - Convert.ToInt32(htcCPGraphics.Width) - 28);
        }




        /// <summary>
        /// Sets the title of the given grid view.
        /// </summary>
        /// <param name="htcWork">Target HTML Table Cell.</param>
        /// <param name="gridTitle">String title to be set.</param>
        private void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            #region NLog
            logger.Info("Setting the title of the given grid view with title as : "+gridTitle);
            #endregion

            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // Need a bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;
            htcWork.InnerText = gridTitle;
            htcWork.Height = "24px";
            htcWork.Width = intScrollLength.ToString();
            objGraphics.Dispose();
            objBitmap.Dispose();
        }

        /// <summary>
        /// Returns the string width in pixels.
        /// </summary>
        /// <param name="title">The string to measure.</param>
        /// <returns>Integer width in pixels.</returns>
        [Ajax.AjaxMethod()]
        public int GetStringWidth(string title)
        {
            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // Need a bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(title, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;
            return intScrollLength;
        }

        /// <summary>
        /// On RowDataBound Event for grdvCenterPanel1
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        protected void grdVw_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
            int colCntr = 0;
            if (drvCurrent != null && m_IsFirstRun)
            {
                m_IsFirstRun = false;
                //Reset
                m_IndexOfIsProtected = -1;
                m_GVClientID = ((GridView)sender).ClientID;
                foreach (DataColumn dc in drvCurrent.DataView.Table.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "IsProtected":
                            {
                                m_IndexOfIsProtected = colCntr;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    colCntr++;
                }
            }

            m_GVClientID = ((GridView)sender).ClientID;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add javascript to highlight row
                //if (m_RowHoverColour.Length > 0)
                //{
                //e.Row.Attributes["onmouseover"] = "javascript:ChangeColor(this, true, '" + m_RowHoverColour + "');";
                //e.Row.Attributes["onmouseout"] = "javascript:ChangeColor(this, false, '');";
                //}
                //Row Highlighting upon hover.
                if (m_IsRowAlternating)
                {
                    if (e.Row.RowState == DataControlRowState.Normal)
                    {
                        e.Row.CssClass = "GVRowHover";
                    }
                }
                else
                {
                    e.Row.CssClass = "GVRowHover";
                }

                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();
                //SmallViewHyperlinks
                string hyperLinksEnabled = commonObjUI.GetPreferenceValue("57");
                if (hyperLinksEnabled == string.Empty)
                {
                    //FullViewHyperlinks
                    hyperLinksEnabled = commonObjUI.GetPreferenceValue("58");
                }

                //Adding the row XML to the template fields.
                HiddenField hdnCurrentRow = (HiddenField)e.Row.Cells[0].FindControl("hdnRowInfo");
                string rowXMLWithOuterNode = "<" + m_GVTreeNodeName + "><RowList>" + GetRowXml(drvCurrent.DataView.Table, primaryKeyValue) + "</RowList></" + m_GVTreeNodeName + ">";
                hdnCurrentRow.Value = rowXMLWithOuterNode;
                //hdnCurrentRow.ID = "hdn" + primaryKeyValue;

                //If BusinessProcessControls node is absent or Show hyperlinks is false dont display the hyperlinks
                if (m_BPCXml != string.Empty && hyperLinksEnabled == "1")
                {
                    foreach (DictionaryEntry de in m_htBPCntrls)
                    {
                        //Index of the column to which BPC is assigned.
                        string processName = de.Key.ToString();
                        int currentBPCIndex = Convert.ToInt32(de.Value);
                        int grdVwColIndex = Convert.ToInt32(m_htBPCColumns[processName]);

                        //Calling Object functionality.
                        string BPCColName = GetColumnName(Convert.ToInt32(m_htBPCntrls[processName])
                                                            , drvCurrent.DataView.Table);
                        string TrxID = string.Empty;
                        string TrxType = string.Empty;
                        int indexOfColNameTrxID = GetColumnIndex(BPCColName + "_TrxID", drvCurrent.DataView.Table);
                        int indexOfColNameTrxType = GetColumnIndex(BPCColName + "_TrxType", drvCurrent.DataView.Table);
                        if (indexOfColNameTrxID != -1 && indexOfColNameTrxType != -1)
                        {
                            //No ColumnName_TrxID and ColumnName_TrxType
                            TrxID = drvCurrent.Row.ItemArray[indexOfColNameTrxID].ToString();
                            TrxType = drvCurrent.Row.ItemArray[indexOfColNameTrxType].ToString();
                        }
                        else
                        {
                            //Take the normal TrxID and TrxType present at the first and second positions respectively.
                            TrxID = drvCurrent.Row.ItemArray[0].ToString();
                            TrxType = drvCurrent.Row.ItemArray[1].ToString();
                        }


                        try
                        {
                            string currentBPGID = GetBPCBPGID(processName, m_BPCXml);
                            string pageInfo = GetBPCPageInfo(processName, m_BPCXml);
                            int BPCColIndex = GetColumnIndex(processName, drvCurrent.DataView.Table);
                            string processLink = string.Empty;//Specifies whether the cell should contain a link or not.
                            if (BPCColIndex == -1)
                            {
                                //Always show the process link if the process Name is not found in the data source.
                                processLink = "1";
                            }
                            else
                            {
                                processLink = drvCurrent.Row.ItemArray[BPCColIndex].ToString();
                            }

                            if (processLink == "1")
                            {
                                string processLabel = GetBPCAttributeValue(processName, m_BPCXml, "Label");
                                string isPopUp = GetBPCAttributeValue(processName, m_BPCXml, "IsPopup");
                                string linkText = drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace("'", "\\'").Replace(" ", "~::~");
                                linkText = commonObjUI.HtmlEncode(linkText);
                                if (m_IndexOfIsProtected != -1 && drvCurrent.Row.ItemArray[m_IndexOfIsProtected].ToString() != "1")
                                {
                                    //The +1 is for compensating the presence of an Image Field in the beginning
                                    e.Row.Cells[grdVwColIndex + 1].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','"
                                        + pageInfo + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','"
                                        + TrxType + "','" + m_GVClientID + "','" + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                }
                                else if (m_IndexOfIsProtected == -1)
                                {
                                    //The +1 is for compensating the presence of an Image Field in the beginning
                                    e.Row.Cells[grdVwColIndex + 1].Text = "<a oncontextmenu='return false;' Title='" + processLabel
                                        + "'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','"
                                        + pageInfo + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','"
                                        + TrxType + "','" + m_GVClientID + "','"
                                        + linkText + "','" + isPopUp + "','" + processName + "')>"
                                        + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            //Do nothing
                            //Dont show any link for the current row.
                        }
                    }
                }

                //For Tooltip functionality
                string dotString = "...";
                foreach (DictionaryEntry de in m_htGVColumns)
                {
                    string colName = de.Key.ToString();
                    int currentColIndex = Convert.ToInt32(de.Value) + 1;
                    int colLength = Convert.ToInt32(m_htGVColWidths[de.Value]);
                    TableCell tcCurrent = e.Row.Cells[currentColIndex];
                    if (tcCurrent.Controls.Count == 0)
                    {
                        //tcCurrent.Attributes.Add("Title", DataBinder.Eval(e.Row.DataItem, colName).ToString());
                        tcCurrent.ToolTip = DataBinder.Eval(e.Row.DataItem, colName).ToString();

                        if (tcCurrent.Text.StartsWith("<a") && tcCurrent.Text.EndsWith("</a>"))
                        {
                            int startIndex = tcCurrent.Text.IndexOf(">") + 1;
                            int endIndex = tcCurrent.Text.IndexOf("</a>", startIndex);
                            string strtoInsert = tcCurrent.Text.Substring(startIndex, endIndex - startIndex);
                            if (strtoInsert.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Remove(startIndex, endIndex - startIndex);
                                strtoInsert = strtoInsert.Substring(0, colLength - dotString.Length) + dotString;
                                tcCurrent.Text = tcCurrent.Text.Insert(startIndex, strtoInsert);
                            }
                        }
                        else
                        {
                            if (tcCurrent.Text.Length > colLength)
                            {
                                tcCurrent.Text = tcCurrent.Text.Substring(0, colLength - dotString.Length) + dotString;
                            }
                        }
                    }
                }
            }
        }

        private string GetColumnName(int grdVwColIndex, DataTable dataTable)
        {
            return dataTable.Columns[grdVwColIndex].ColumnName;
        }

        /// <summary>
        /// Gets the value of the business rule from the given XML.
        /// </summary>
        /// <param name="ruleName">Rule name.</param>
        /// <param name="m_BusinessRules">XML string containing the Business Rules XML.</param>
        /// <returns></returns>
        private string GetBusinessRulesValue(string ruleName, string businessRulesNode)
        {
            #region NLog
            logger.Info("Getting the value of the business rule :"+ ruleName+ " and business rules node : "+businessRulesNode+" from the given XML.");
            #endregion

            if (businessRulesNode.Length == 0)
            {
                return string.Empty;
            }
            //Getting the corresponding rule name ID from the config file.
            string bRuleID = ConfigurationManager.AppSettings[ruleName];
            XmlDocument xDocBR = new XmlDocument();
            xDocBR.LoadXml(businessRulesNode);
            XmlNode xNodeBRV = xDocBR.SelectSingleNode("BusinessRules/BusinessRule[@TypeOfPreferenceID='"
                                   + bRuleID + "']");
            if (xNodeBRV != null)
            {
                return xNodeBRV.Attributes["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets the Page Info value from the given XML.
        /// </summary>
        /// <param name="processName">Process Name</param>
        /// <param name="bpcXml">Business Process Controls XML.</param>
        /// <returns>String page info node value.</returns>
        private string GetBPCPageInfo(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Retrieving the Page Info value from the given XML with process name as : " + processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["PageInfo"].Value;
        }

        /// <summary>
        /// Gets the attribute value for a given attribute name based on a process.
        /// </summary>
        /// <param name="processName">The Process name to look out for.</param>
        /// <param name="m_BPCXml">The BPC node XML</param>
        /// <param name="AttributeName">The attribute name in the row matching the process</param>
        /// <returns>Attribute Value</returns>
        private string GetBPCAttributeValue(string processName, string m_BPCXml, string AttributeName)
        {
            #region NLog
            logger.Info("Retrieving the attribute value for a given attribute name based on a process name : " + processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(m_BPCXml);
            XmlNode nodeProcess = xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
            if (nodeProcess != null)
            {
                return nodeProcess.Attributes[AttributeName].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Creates the xml structure of the given row.
        /// </summary>
        /// <param name="dtGridData">The table which contains the row.</param>
        /// <param name="primaryKey">The unique key to identify the row.</param>
        /// <returns>xml string.</returns>
        private string GetRowXml(DataTable dtGridData, string primaryKey)
        {
            DataRow[] drFoundRows = dtGridData.Select("TrxID" + "='" + primaryKey + "'");
            XmlDocument xDocRow = new XmlDocument();
            XmlNode nodeRow = xDocRow.CreateNode(XmlNodeType.Element, dtGridData.TableName, null);
            for (int i = 0; i < dtGridData.Columns.Count; i++)
            {
                XmlAttribute attCurrentCol = xDocRow.CreateAttribute(dtGridData.Columns[i].ColumnName);
                attCurrentCol.Value = drFoundRows[0][i].ToString();
                nodeRow.Attributes.Append(attCurrentCol);
            }
            return nodeRow.OuterXml;
        }

        /// <summary>
        /// Gets the BPGID of the Businees Process Control.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="bpcXml">The BPC node XML</param>
        /// <returns>string BPGID</returns>
        private string GetBPCBPGID(string processName, string bpcXml)
        {
            #region NLog
            logger.Info("Retrieving the BPGID of the Businees Process Control with process name as : " + processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["BPGID"].Value;
        }

        /// <summary>
        /// Gets the BPGID for a given CenterPanel GridView.
        /// </summary>
        /// <param name="index">The position of the CenterPanel node in the returned XML.</param>
        /// <returns></returns>
        private string GetBPGID(string typeOfPrefId, string GVContainer, XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Retrieving the BPGID for a given CenterPanel GridView with preference is as : " + typeOfPrefId);
            #endregion

            XmlDocument xDocBPGID = new XmlDocument();
            //xDocBPGID.LoadXml(Convert.ToString(Session["GBPC"]));
            //string keyValue = ConfigurationManager.AppSettings[typeOfPrefId];

            XmlNode nodeBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/" + GVContainer + "Panel/" + GVContainer + "Process[@TypeOfPreferenceID = '" + typeOfPrefId + "']");
            if (nodeBPGID != null)
            {
                return nodeBPGID.Attributes["BPGID"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        private string GenerateGVRequestXML(string BPGID, string pageSize, XmlDocument XDocUserInfo)
        {
            #region NLog
            logger.Info("Generated the xml to request data to be bound for the grid view with BPGID as : "+BPGID+" and page size : "+pageSize);
            #endregion

            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            ////Adding the PriorFormInfo Node.
            //if (Session["PriorFormInfo"] != null)
            //{
            //    string formInfo = Session["PriorFormInfo"].ToString();
            //    nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");
            //}

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeRoot.OuterXml;
        }

        /// <summary>
        /// Generates the xml to request data to be bound for the graphics panel.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        private string GenerateGraphicsRequestXML(string BPGID, XmlDocument XDocUserInfo)
        {
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);


            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            ////Adding the PriorFormInfo XML.
            //string formInfo = Session["PriorFormInfo"].ToString();
            //nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);
            return nodeRoot.OuterXml;
        }

        /// <summary>
        /// Generic method to be used for requesting data for all the GridViews in the current page.
        /// </summary>
        /// <param name="BPGID">The BPGID to request for.</param>
        /// <returns>Return XML.</returns>
        private string GenerateGVBPEInfo(string BPGID)
        {
            #region NLog
            logger.Info("Generic method to be used for requesting data for all the GridViews in the current page with BPGID as : "+BPGID);
            #endregion

            XmlDocument xDocGV = new XmlDocument();

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);

            //string formInfo = Session["PriorFormInfo"].ToString();
            //nodeBPInfo.InnerXml += formInfo.Replace("<FormInfo>", "<PriorFormInfo>").Replace("</FormInfo>", "</PriorFormInfo>");

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            //No paging stuff is required when requesting data for the first page.
            //Page size will be same as the Default page size in the User Preferences.
            //nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeBPInfo.OuterXml;
        }

        /// <summary>
        /// Timer tick event for the Center Panel GV refresh functionliaty.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        protected void timerCPGV1_Tick(object sender, EventArgs e)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            BindCPGV1(XDocUserInfo);
        }
        //protected void lnkBtnCPGV1FV_Click(object sender, EventArgs e)
        //{
        //    //GridViewControl GVUC = (GridViewControl)LoadControl("~/UserControls/GridViewControl.ascx");
        //    //phGVContainer.Controls.Add(GVUC);
        //    //GVUC.GridViewType = GridViewType.DashBoard;
        //    string BPGID = GetBPGID("DashBoardCPGV1", "Center");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, defaultPageSize));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucCPGV1.DefaultPageSize = defaultPageSize;
        //    ucCPGV1.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucCPGV1.GridViewInitData = returnXML;
        //    ucCPGV1.DataBind();
        //    //mpeDashBoard.Show();
        //}
        //protected void lnkBtnCPGV2FV_Click(object sender, EventArgs e)
        //{
        //    string BPGID = GetBPGID("DashBoardCPGV2", "Center");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, defaultPageSize));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucCPGV2.DefaultPageSize = defaultPageSize;
        //    ucCPGV2.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucCPGV2.GridViewInitData = returnXML;
        //    ucCPGV2.DataBind();
        //}
        //protected void lnkBtnRPGV1FV_Click(object sender, EventArgs e)
        //{
        //    string BPGID = GetBPGID("DashBoardRPGV1", "Right");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, defaultPageSize));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucRPGV1.DefaultPageSize = defaultPageSize;
        //    ucRPGV1.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucRPGV1.GridViewInitData = returnXML;
        //    ucRPGV1.DataBind();
        //}
        //protected void lnkBtnRPGV2FV_Click(object sender, EventArgs e)
        //{
        //    string BPGID = GetBPGID("DashBoardRPGV2", "Right");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, defaultPageSize));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucRPGV2.DefaultPageSize = defaultPageSize;
        //    ucRPGV2.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucRPGV2.GridViewInitData = returnXML;
        //    ucRPGV2.DataBind();
        //}
        //protected void lnkBtnRPGV3FV_Click(object sender, EventArgs e)
        //{
        //    string BPGID = GetBPGID("DashBoardRPGV3", "Right");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, grdVwRightPanel1.PageSize.ToString()));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucRPGV3.DefaultPageSize = defaultPageSize;
        //    ucRPGV3.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucRPGV3.GridViewInitData = returnXML;
        //    ucRPGV3.DataBind();
        //}
        //protected void lnkBtnRPGV4FV_Click(object sender, EventArgs e)
        //{
        //    string BPGID = GetBPGID("DashBoardRPGV4", "Right");
        //    string defaultPageSize = GetUserPreferenceValue("DefaultGridSize");
        //    string returnXML = objBO.GetDataForCPGV1(GenerateGVRequestXML(BPGID, grdVwRightPanel1.PageSize.ToString()));

        //    //Initialising the corresponding user control in the pop-up panel.
        //    ucRPGV4.DefaultPageSize = defaultPageSize;
        //    ucRPGV4.GridViewBPInfo = GenerateGVBPEInfo(BPGID);
        //    ucRPGV4.GridViewInitData = returnXML;
        //    ucRPGV4.DataBind();
        //}
        protected void lnkBtnClosePopUp_Click(object sender, EventArgs e)
        {
            //Resetting the parent BPInfo to the page after closing the child(Page PopUP).
            // mpePagePopUp.Hide();
        }




    }
}
