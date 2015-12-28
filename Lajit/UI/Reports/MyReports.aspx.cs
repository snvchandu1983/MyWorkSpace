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
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
// Namespace for PDF generation
using Gios.Pdf;
//using AjaxControlToolkit;

using LAjit_BO;
using NLog;


namespace LAjitDev.Reports
{
    public partial class MyReports : Classes.BasePage
    {
        // CommonBO objBO = new CommonBO();
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        MyReportsBO objBO = new MyReportsBO();
        LAjit_BO.Reports objReportsBO = new LAjit_BO.Reports();
        public LAjitDev.clsReportsUI reportObjUI = new clsReportsUI();
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();

        string i;
        int RowsIndex = 0;

        /// <summary>
        /// Contains the Session["GBPC"]/BusinessProcessControls string.
        /// </summary>

        private string m_ReturnXML = string.Empty;

        public string GridViewBPE
        {
            get { return ViewState["GridBPE"].ToString(); }
            set { ViewState["GridBPE"] = value; }
        }

        public string GridViewInitData
        {
            get { return ViewState["GridViewInitData"].ToString(); }
            set { ViewState["GridViewInitData"] = value; }
        }

        public string GridViewBPInfo
        {
            get { return ViewState["GridBPEInfo"].ToString(); }
            set { ViewState["GridBPEInfo"] = value; }
        }
        //Reports
        Hashtable m_htGVColumns = new Hashtable();
        Hashtable m_htGVColsPtn = new Hashtable();

        public string CurrentBPGID
        {
            get { return ViewState["CurrentBPGID"].ToString(); }
            set { ViewState["CurrentBPGID"] = value; }
        }


        public string strJQueryTips = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string masterfile = string.Empty;
                if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
                {
                    if (Session["LinkBPinfo"] != null)
                    {
                        Session["BPINFO"] = Session["LinkBPinfo"].ToString();
                        Session.Remove("LinkBPinfo");
                    }

                    //Set height and width for popup
                    // int entryFormWidth = 860;
                    tblmain.Width = "900px";
                }
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                string currentBPGID = string.Empty;//"10"; //"79"; //"77"; //"78" //62;
                //Registering ajax method to fetch selectedrow 
                Ajax.Utility.RegisterTypeForAjax(typeof(MyReports));
                PreLoadImageUrls();
                //trStatusMsg.Visible = false;
                //lblStatusMsg.Text = string.Empty;

                //if (!Page.IsPostBack)
                //{
                string returnXML = objBO.GetDataForReports(GenerateGVRequestXML());

                XmlDocument returnXMLDoc = new XmlDocument();
                returnXMLDoc.LoadXml(returnXML);
                //success message
                XmlNode nodeMsg = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                XmlNode nodeMsgStatus = returnXMLDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                if (nodeMsgStatus != null)
                {
                    if (nodeMsgStatus.InnerText == "Success")
                    {
                        //trStatusMsg.Visible = false;


                        if (Session["BPINFO"] != null && Session["BPINFO"].ToString() != string.Empty)
                        {
                            XmlDocument xDocBPInfo = new XmlDocument();
                            xDocBPInfo.LoadXml(Session["BPINFO"].ToString());
                            XmlNode nodeBPInfo = xDocBPInfo.SelectSingleNode("bpinfo/BPGID");
                            if (nodeBPInfo != null)
                                currentBPGID = nodeBPInfo.InnerText;
                        }

                        // GridViewBPE = Session["BPE"].ToString();
                        GridViewBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                        GridViewBPInfo = GenerateGVBPEInfo(currentBPGID);
                        GridViewInitData = returnXML;
                        DataBind();

                        //Enable or disable print button
                        EnablePrintButton();

                        //Enable Disable images
                        EnableDisableImages(returnXMLDoc);

                    }
                    else//Error
                    {
                        //trStatusMsg.Visible = true;
                        //if (nodeMsg != null)
                        //{
                        //    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                        //    {
                        //        lblStatusMsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        //    }
                        //    else if (nodeMsg.SelectSingleNode("Label") != null)
                        //    {                                
                        //        lblStatusMsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                        //    }
                        //    else
                        //    {                                                             
                        //        lblStatusMsg.Text = nodeMsgStatus.InnerText;
                        //    }
                        //}
                        //else
                        //{                                                            
                        //    lblStatusMsg.Text = nodeMsgStatus.InnerText;
                        //}
                    }
                }

                //Register QTIP for reports
                string strLoadScript = "function JQueryPageEvents(){" + strJQueryTips + " }";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), strLoadScript, true);


                //Reports
                //BindReportInfo();
                //}
            }
            catch//(Exception ex)
            {
                // ex.InnerException.ToString();
            }
        }
        #region Private Methods...
        ///// <summary>
        ///// Applying the page theme
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPreInit(EventArgs e)
        //{
        //    base.OnPreInit(e);
        //    Page.Theme = ((string)Session["MyTheme"]);
        //}
        /// <summary>
        /// Preloading image urls
        /// </summary>
        private void PreLoadImageUrls()
        {
            /*  imgbtnDelete.ImageUrl = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/delete_icon.png";
              imgbtnDelete.Attributes.Add("onmouseover", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/delete-icon-over.png'");
              imgbtnDelete.Attributes.Add("onmouseout", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/delete_icon.png'");

              imgbtnPrint.ImageUrl = "../App_Themes/" + Session["MyTheme"].ToString() + "/Images/print-icon.png";
              imgbtnPrint.Attributes.Add("onmouseover", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/print-icon-over.png'");
              imgbtnPrint.Attributes.Add("onmouseout", "this.src='../App_Themes/" + Session["MyTheme"].ToString() + "/Images/print-icon.png'");
               */

            imgbtnPrintOk.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/print-but.png";
            imgbtnPrintCancel.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/cancel-but.png";

            imgbtnDelOk.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/ok-but.png";
            imgbtnDelCancel.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/cancel-but.png";
        }


        private void EnableDisableImages(XmlDocument xDoc)
        {

            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess [@ID='Delete']") != null)
            {
                //Enable Delete Option
                imgbtnDelete.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/delete_icon.png";
                imgbtnDelete.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete-icon-over.png'");
                imgbtnDelete.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/delete_icon.png'");
                imgbtnDelete.Enabled = true;
            }
            else
            {
                //Disable Delete Option
                imgbtnDelete.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/delete-icon-disable.png";
                imgbtnDelete.Attributes.Remove("onmouseover");
                imgbtnDelete.Attributes.Remove("onmouseout");
                imgbtnDelete.Enabled = false;
            }

            if (xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess [@ID='GetMyReport']") != null)
            {
                //Enable Print Option
                imgbtnPrint.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/print-icon.png";
                imgbtnPrint.Attributes.Add("onmouseover", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon-over.png'");
                imgbtnPrint.Attributes.Add("onmouseout", "this.src='" + Application["ImagesCDNPath"].ToString() + "Images/print-icon.png'");
                imgbtnPrint.Enabled = true;

            }
            else
            {
                //Disable Print Option
                imgbtnPrint.ImageUrl = Application["ImagesCDNPath"].ToString()+ "Images/print-icon-disable.png";
                imgbtnPrint.Attributes.Remove("onmouseover");
                imgbtnPrint.Attributes.Remove("onmouseout");
                imgbtnPrint.Enabled = false;

                //delete-icon-disable.png
            }
        }



        /// <summary>
        /// Generic method to be used for requesting data for all the GridViews in the current page.
        /// </summary>
        /// <param name="BPGID">The BPGID to request for.</param>
        /// <returns>Return XML.</returns>
        private string GenerateGVBPEInfo(string BPGID)
        {
            #region NLog
            logger.Info("Generic method to be used for requesting data for all the GridViews in the current page with BPGID as : " + BPGID); 
            #endregion

            try
            {
                XmlDocument xDocGV = new XmlDocument();

                //Creating the bpinfo node
                XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);

                //Creating the BPGID node
                XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodeBPGID.InnerText = BPGID;
                nodeBPInfo.AppendChild(nodeBPGID);

                nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>10</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                return nodeBPInfo.OuterXml;
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion

                throw ex;
            }
        }
        /// <summary>
        /// Binds the first set of data to the user control without hitting the BPE.
        /// </summary>
        private void DataBind()
        {
            #region NLog
            logger.Info("Binding the first set of data to the user control without hitting the BPE.");
            #endregion

            try
            {
                //if (!Page.IsPostBack)
                //{

                //ReserGVType();
                ViewState["VENDSORT"] = "ASC";

                //Getting the returnXml from the BPE which also consists of the gridview contents.
                XmlDocument xDocReturn = new XmlDocument();
                xDocReturn.LoadXml(GridViewInitData);

                string bpcXML = string.Empty;
                if (xDocReturn.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls") != null)
                {
                    bpcXML = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls").OuterXml;
                }
                else
                {
                    bpcXML = string.Empty;
                }
                Session["returnXML"] = GridViewInitData;
                DisplayFoundResults(GridViewInitData);
                GridViewInitData = string.Empty;
                //}

            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        /// <summary>
        /// Get Selected Transaction ID
        /// </summary>
        /// <returns></returns>
        private string GetSelectedTraxID(string XMLNode)
        {
            #region NLog
            logger.Info("Get Selected Transaction ID for the XMl Node : "+XMLNode);
            #endregion

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(XMLNode);
            string TrxID = "";
            XmlNode xnode = xDoc.SelectSingleNode("ReportNode/RowList/Rows");
            TrxID = xnode.Attributes["TrxID"].Value.ToString();
            return TrxID;
        }

        /// <summary>
        /// Bind data and remove the row.
        /// </summary>
        private void RemoveAndDataBind()
        {
            try
            {
                //if (!Page.IsPostBack)
                //{

                //ReserGVType();
                ViewState["VENDSORT"] = "ASC";

                //Getting the returnXml from the BPE which also consists of the gridview contents.
                XmlDocument xDocReturn = new XmlDocument();
                xDocReturn.LoadXml(Session["returnXML"].ToString());

                string bpcXML = string.Empty;
                if (xDocReturn.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls") != null)
                {
                    bpcXML = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls").OuterXml;
                }
                else
                {
                    bpcXML = string.Empty;
                }
                string treeNode = CheckLeafAvailable("Branch1LeafCell1", Session["returnXML"].ToString());
                XmlNode nodeRow = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + treeNode + "//Rows [@TrxID='" + GetSelectedTraxID(Session["DeleteReport"].ToString()) + "']");
                nodeRow.RemoveAll();
                //
                //nodeRow.InnerXml = Session["DeleteReport"].ToString();
                //xDocReturn.RemoveChild(nodeRow);
                // XmlNodeList xnodeList = xDocReturn.SelectNodes();

                Session["returnXML"] = xDocReturn.OuterXml.ToString();          //GridViewInitData;
                GridViewInitData = xDocReturn.OuterXml.ToString();
                DisplayFoundResults(GridViewInitData);
                GridViewInitData = string.Empty;
                //}

            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
        /// <summary>
        /// Enable print button in Leaf Cells
        /// </summary>
        private void EnablePrintButton()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(Session["returnXML"].ToString());
            XmlNode nodecolumn = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Leaves/Leaf/Cells/Cell");
            string IsOkToPrint = nodecolumn.Attributes["IsOkToPrint"].Value.ToString();

            if (IsOkToPrint == "1")
            {
                //Enable print button
                imgbtnPrint.Enabled = true;
            }
            else
            {
                //Disable print button
                imgbtnPrint.Enabled = false;
            }
        }
        /// <summary>
        /// Binding Parent Grid Data
        /// </summary>
        /// <param name="returnXML"></param>
        public void DisplayFoundResults(string returnXML)
        {
            #region NLog
            logger.Info("Binding Parent Grid Data based on the returend XML");
            #endregion

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(returnXML);

                //Get the Grid Layout nodes
                string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                ViewState["Tree"] = treeNodeName;

                //Code from this point onwards can be recursive!!
                //RowList
                //Getting the dataset to be bound to the grid.
                XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                DataSet dsVendors = new DataSet();
                dsVendors.ReadXml(read);

                //setting grid title
                //string GridTitle = "";
                //XmlNode nodeColumn = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                //GridTitle = nodeColumn.InnerText.ToString();
                //gvTree.Caption = GridTitle;

                bool showProcesses = true;
                if (ViewState["ControlsVisible"] != null)
                {
                    showProcesses = (bool)ViewState["ControlsVisible"];
                }

                //Setting the Alternating Row Style of the GridView
                //FullViewAlternatingStyle
                string isAlternating =commonObjUI.GetPreferenceValue("56");
                if (isAlternating == "1")
                {
                    if (showProcesses)
                    {
                        gvTree.AlternatingRowStyle.CssClass = "AlternatingRowStyle";
                    }
                    else
                    {
                        gvTree.AlternatingRowStyle.CssClass = "AlternatingCOARowStyle";
                    }
                }
                gvTree.DataSource = dsVendors;
                gvTree.DataBind();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
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
            logger.Info("Getting the BPGID of the Businees Process Control with process name as : "+processName);
            #endregion

            XmlDocument xDocBPC = new XmlDocument();
            xDocBPC.LoadXml(bpcXml);
            return xDocBPC.SelectSingleNode("BusinessProcessControls/BusinessProcess[@ID='" + processName + "']").Attributes["BPGID"].Value;
        }
        /// <summary>
        /// Displaying Results Static
        /// </summary>
        /// <param name="returnXML"></param>
        /// <param name="gvChild"></param>
        /// <param name="TrxID"></param>
        /// <param name="TrxType"></param>
        /// <param name="treeNodeName"></param>
        /// <param name="gridname"></param>
        /// <param name="PreviousNode"></param>        
        public void DisplayFoundResultsChild(string returnXML, GridView gvChild, string TrxID, string TrxType, string treeNodeName, string gridname, string PreviousNode)
        {
            #region NLog
            logger.Info("Displaying the child results based on the retun XML RrxID : "+TrxID+"and TrxType : "+TrxType+" and tree node name : "+treeNodeName+" adn grid nane as : "+gridname+" and previous node is : "+PreviousNode);
            #endregion

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);
            if (treeNodeName != "")
            {
                XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                DataSet dsVendors = new DataSet();
                dsVendors.ReadXml(read);
                DataTable ChildDataTable = new DataTable();
                ChildDataTable = dsVendors.Tables[0];
                DataView ChildDataView = new DataView(ChildDataTable);

                //Setting grid title
                //string GridTitle = "";
                //XmlNode nodeColumn = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                //GridTitle = nodeColumn.InnerText.ToString();
                //gvChild.Caption = GridTitle;

                string columnName = "";

                //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns/Col[1]");
                //columnName = nodeColumns.Attributes["Label"].Value;

                columnName = PreviousNode;
                //Checking column name exist or not
                //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns//col[@Label='" + PreviousNode + "']");
                //columnName = nodeColumns.Attributes["Label"].Value;

                if (columnName.ToString() != "")
                {
                    ChildDataView.RowFilter = columnName + "_TrxID='" + TrxID + "' and " + columnName + "_TrxType='" + TrxType + "'";

                    //string[] DataKeyNames = new string[ChildDataView.Columns.Count];

                    //for (int i = 0; i <= Convert.ToInt64(ChildDataView.Columns.Count - 1); i++)
                    //{
                    //    DataKeyNames[i] = ChildDataView.Columns[i].ToString();
                    //}
                    //gvChild.DataKeyNames = DataKeyNames;
                    gvChild.DataSource = ChildDataView;
                    gvChild.DataBind();
                }
            }
        }
        /// <summary>
        /// Displaying Results Dyanmic for cell node
        /// </summary>
        /// <param name="returnXML"></param>
        /// <param name="gvChild"></param>
        /// <param name="TrxID"></param>
        /// <param name="TrxType"></param>
        /// <param name="treeNodeName"></param>
        /// <param name="gridname"></param>
        /// <param name="PreviousNode"></param>        
        public void DisplayFoundResultsDynamic(string returnXML, GridView gvChild, string TrxID, string TrxType, string treeNodeName, string gridname, string PreviousNode)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);
            if (treeNodeName != "")
            {
                //DataTable ChildDataTable = new DataTable();
                //ChildDataTable = dsVendors.Tables[0];
                //DataView ChildDataView = new DataView(ChildDataTable);

                //Setting grid title
                //string GridTitle = "";
                //XmlNode nodeColumn = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title");
                //GridTitle = nodeColumn.InnerText.ToString();
                //gvChild.Caption = GridTitle;

                string columnName = "";

                int colCntr = 0;
                int gridcolumn = 1;

                //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns/Col[1]");
                //columnName = nodeColumns.Attributes["Label"].Value;

                columnName = PreviousNode;
                //Checking column name exist or not
                //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns//col[@Label='" + PreviousNode + "']");
                //columnName = nodeColumns.Attributes["Label"].Value;

                if (columnName.ToString() != "")
                {
                    //Rows TrxID="25" TrxType="47  //title[@lang='eng']][@
                    //string Filter = "[@" + columnName + "_TrxID='" + TrxID + "'][@" + columnName + "_TrxType='" + TrxType + "']";
                    //string Filter1 = "[@" + columnName + "_TrxID='" + TrxID + "']";
                    //string Filter2 = "[@" + columnName + "_TrxType='" + TrxType + "']";

                    //Method1 Featches 1 row
                    //XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows" + Filter);
                    //XmlNodeReader read = new XmlNodeReader(nodeRows);
                    //DataSet dsVendors = new DataSet();
                    //dsVendors.ReadXml(read);

                    //Method2
                    //XmlNodeList nodeRows1 = xDoc.SelectNodes("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows" + Filter);
                    //XmlDocument xdoc1 = new XmlDocument();

                    //Method3
                    XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    DataSet dsVendors = new DataSet();
                    dsVendors.ReadXml(read);
                    DataTable ChildDataTable = new DataTable();
                    ChildDataTable = dsVendors.Tables[0];
                    DataView ChildDataView = new DataView(ChildDataTable);
                    ChildDataView.RowFilter = columnName + "_TrxID='" + TrxID + "' and " + columnName + "_TrxType='" + TrxType + "'";

                    //Adding columns
                    XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");

                    //Add the columns only if no columns are present.
                    if (gvChild.Columns.Count == gvChild.Columns.Count)
                    {
                        //Creating the Columns.
                        foreach (XmlNode colNode in nodeColumns.ChildNodes)
                        {
                            if (colNode.Attributes["FullViewLength"].Value != "0")
                            {

                                if (colNode.Attributes["Label"].Value != "ReportTitle")
                                {
                                    BoundField newField = new BoundField();
                                    newField.DataField = colNode.Attributes["Label"].Value;
                                    newField.HeaderText = colNode.Attributes["Caption"].Value;
                                    newField.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                    newField.ItemStyle.CssClass = "myreportItemText";
                                    //ItemStyle-HorizontalAlign="Left"  //

                                    gridcolumn = gridcolumn + 1;
                                    gvChild.Columns.Insert(gridcolumn, newField);
                                    colCntr++;
                                }
                            }
                        }
                    }
                    if (ChildDataView.Table.Rows.Count != 0)
                    {
                        foreach (DataRow dr in ChildDataView.Table.Rows)
                        {
                            DateTime dateTime;
                            if (DateTime.TryParse(dr["DateCreated"].ToString(), out dateTime))
                            {
                                dr["DateCreated"] = dateTime.ToString("MM/dd/yyyy");
                            }
                        }
                    }
                    gvChild.DataSource = ChildDataView;
                    gvChild.DataBind();
                }
            }
        }
        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        private string GenerateGVRequestXML()
        {
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            // nodeRoot.InnerXml = Session["BPE"].ToString();

            if (Convert.ToString(Context.Request["hdnBPInfo"]) != null && Convert.ToString(Context.Request["hdnBPInfo"]) != string.Empty)
            {
                nodeRoot.InnerXml += Context.Request["hdnBPInfo"].ToString();
            }
            else
            {
                if (hdnReportBPINFO.Value != string.Empty)
                {
                    Session["BPINFO"] = hdnReportBPINFO.Value.ToString();
                }
                else
                {
                    //To view PDF changing BPINFO maintain previous BPINFO stored in ReportBPINFO
                    //if (Session["ReportBPINFO"] != null)
                    //{
                    //    Session["BPINFO"] = Session["ReportBPINFO"].ToString();
                    //}
                    //else
                    //{
                    hdnReportBPINFO.Value = Session["BPINFO"].ToString();
                    //    Session["ReportBPINFO"] = Session["BPINFO"].ToString();
                    //}
                }

                nodeRoot.InnerXml += Session["BPINFO"].ToString();
            }
            XmlDocument xDocum = new XmlDocument();
            xDocum.LoadXml(nodeRoot.OuterXml);
            XmlNode nodeBPInfo = xDocum.SelectSingleNode("Root/bpinfo");

            ////Creating the bpinfo node
            //XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            //nodeRoot.AppendChild(nodeBPInfo);

            ////Creating the BPGID node
            //XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            //nodeBPGID.InnerText = BPGID;
            //nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return xDocum.OuterXml;
        }
        /// <summary>
        /// Check leaf node available or not
        /// </summary>
        /// <returns></returns>
        private string CheckLeafAvailable(string TreeNode, string returnXML)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(returnXML);
            //Get the Grid Layout nodes
            string treeNodeName = "";
            switch (TreeNode)
            {
                case "Branch1":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node") != null)

                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText;
                        break;
                    }
                case "Branch2":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[2]/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[2]/Node").InnerText;
                        break;
                    }
                case "Branch3":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[3]/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[3]/Node").InnerText;
                        break;

                    }
                case "Branch1Leaf":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[0]/Leaves/Leaf/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[0]/Leaves/Leaf/Node").InnerText;
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Leaves/Leaf/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Leaves/Leaf/Node").InnerText;

                        break;
                    }
                case "Branch2Leaf":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[2]/Leaves/Leaf/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[2]/Leaves/Leaf/Node").InnerText;
                        break;
                    }
                case "Branch3Leaf":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[3]/Leaves/Leaf/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[3]/Leaves/Leaf/Node").InnerText;
                        break;
                    }
                case "Branch1LeafCell1":
                    {
                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[0]/Leaves/Leaf/Cells/Cell/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch[0]/Leaves/Leaf/Cells/Cell/Node").InnerText;

                        if (xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Leaves/Leaf/Cells/Cell/Node") != null)
                            treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Leaves/Leaf/Cells/Cell/Node").InnerText;

                        break;
                    }
                default:
                    break;
            }
            return treeNodeName;
        }
        /// <summary>
        /// Generates the request xml to be sent to the XML processor
        /// </summary>
        /// <returns>string XML</returns>
        private string GenerateRequestXML(string pageNo, string sortColumn, string sortOrder)
        {
            XmlDocument xDocRequest = new XmlDocument();
            XmlNode nodeRoot = xDocRequest.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = ViewState["GridBPE"].ToString();

            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(ViewState["GridBPEInfo"].ToString());
            XmlNode nodeGridResults = xDocBPEInfo.SelectSingleNode("bpinfo/Gridview");
            nodeGridResults.SelectSingleNode("Pagesize").InnerText = gvTree.PageSize.ToString();
            nodeGridResults.SelectSingleNode("Pagenumber").InnerText = pageNo;

            //nodeGridResults.SelectSingleNode("Sortcolumn").InnerText = sortColumn;
            //nodeGridResults.SelectSingleNode("Sortorder").InnerText = sortOrder;

            nodeRoot.InnerXml += xDocBPEInfo.OuterXml;
            CommonBO objBO = new CommonBO();
            //return objBO.FindThisVendor(nodeRoot.OuterXml);

            return objBO.GetVendorHistory(nodeRoot.OuterXml);
        }
      
        /// <summary>
        /// Get row xml
        /// </summary>
        /// <param name="TrxID"></param>
        /// <returns></returns>
        private string GetRowXml(string TrxID, string treeNodeName)
        {
            StringBuilder sbXml = new StringBuilder();

            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(Session["returnXML"].ToString());
            XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList//Rows[@TrxID='" + TrxID + "']");

            sbXml.Append("<" + treeNodeName + ">");
            sbXml.Append("<RowList>");
            sbXml.Append(nodeRows.OuterXml);
            sbXml.Append("</RowList>");
            sbXml.Append("</" + treeNodeName + ">");
            return sbXml.ToString();
        }
        /// <summary>
        /// To get tooltip text for each row
        /// </summary>
        /// <param name="LeafNode"></param>
        /// <param name="TrxID"></param>
        /// <returns></returns>
        private void GetToolTipText(string TrxID, string LeafNode, StringBuilder sbTipTable)
        {
            //  string LeafNode = CheckLeafAvailable("Branch1LeafCell1", Session["returnXML"].ToString());


            if (LeafNode != "")
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(Session["returnXML"].ToString()); //Rows TrxID="43"

                //Columns Reading from XML
                XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + LeafNode + "/GridHeading/Columns");
                //Binding columns to dataset 
                // XmlNodeReader colread = new XmlNodeReader(nodeColumns);

                string BGIMG = Application["ImagesCDNPath"].ToString()+ "Images/ztest.jpg";
                //border=\'0\'
                ArrayList alColumnLabel = new ArrayList();

                //http://localhost:1403/App_Themes/Lajit/Images/form-bg.gif

                //sbTipTable.Append("<table cellpading='7' cellspacing='2' border='1' rules='all' BorderColor='#BBD2FD'  style='border: #060F40 1px solid;color: #060F40;background: #ffffff;'>");
                //style='background:#060F40;' 

                // sbTipTable.Append("<table cellpading='0' width='70%' cellspacing='0' border='1' rules='all' background='" + BGIMG + "' class='formmiddle'>");

                sbTipTable.Append("<table cellpading='0' width='70%' cellspacing='0' border='1' rules='all' class='formmiddle'>");

                //Rows Reading from XML
                XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + LeafNode + "/RowList//Rows[@TrxID=" + TrxID + "]");
                //SelectedRow = nodeRows.OuterXml;

                //Binding rows to dataset
                DataSet dsReports = new DataSet();
                XmlNodeReader read = new XmlNodeReader(nodeRows);
                dsReports.ReadXml(read);

                foreach (XmlNode colNode in nodeColumns.ChildNodes)
                {

                    if (colNode.Attributes["Caption"].Value != " ")
                    {
                        // alColumnLabel.Add(colNode.Attributes["Label"].Value) style='background:#ffcccc;';

                        sbTipTable.Append("<tr  class='myreportHeaderText'>");

                        sbTipTable.Append("<td NOWRAP> <b>" + colNode.Attributes["Caption"].Value + "</b></td>");

                        if (dsReports.Tables[0].Columns.Contains(colNode.Attributes["Label"].Value))
                        {
                            sbTipTable.Append("<td NOWRAP>" + dsReports.Tables[0].Rows[0][colNode.Attributes["Label"].Value].ToString() + "</td>");
                        }
                        else
                        {
                            sbTipTable.Append("<td>&nbsp;</td>");
                        }

                        sbTipTable.Append("</tr>");
                    }
                }


                //if (dsReports.Tables[0].Rows.Count > 0)
                //{
                //    //Filling rows
                //    sbTipTable.Append("<tr class='myreportItemText'>");
                //    for (int i = 0; i <= alColumnLabel.Count - 1; i++)
                //    {
                //        if (dsReports.Tables[0].Columns.Contains(alColumnLabel[i].ToString()))
                //        {
                //            sbTipTable.Append("<td NOWRAP>" + dsReports.Tables[0].Rows[0][alColumnLabel[i].ToString()].ToString() + "</td>");
                //        }
                //        else
                //        {
                //            sbTipTable.Append("<td></td>");
                //        }

                //    }
                //    sbTipTable.Append("</tr>");
                //}
                sbTipTable.Append("</table>");
            }

        }


        #endregion
        protected void gvTree_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // parentflag = "Y";
            Control container = e.Row;

            //ListItemType itemType = (ListItemType)e.Row.RowState;

            DataControlRowState itemType = (DataControlRowState)e.Row.RowState;
            string ItemID = "";
            ItemID = e.Row.Cells[0].Text.Trim();
            string TreeNode = "";

            //DataControlRowState.Alternate
            //if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            if (itemType == DataControlRowState.Normal || itemType == DataControlRowState.Alternate)
            {
                if (e.Row.DataItem == null)
                {
                    return;
                }
                else
                {
                    //placing hyperlinks based on business rules

                    DataRowView drvCurrent = (DataRowView)e.Row.DataItem;


                    GridView gvBranch1 = new GridView();

                    gvBranch1 = (GridView)container.FindControl("gvBranch1");


                    //check branch availability

                    TreeNode = CheckLeafAvailable("Branch1", Session["returnXML"].ToString());

                    if ((gvBranch1 != null) && (TreeNode.ToString() != ""))
                    {
                        // DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
                        string Vendor_TrxID = drvCurrent.Row.ItemArray[0].ToString();
                        string Vendor_Type = drvCurrent.Row.ItemArray[1].ToString();

                        string PreviousNode = ViewState["Tree"].ToString();

                        ViewState["Branch1"] = TreeNode.ToString();
                        //DisplayFoundResultsChild(Session["returnXML"].ToString(), ChildCriteriaGrid1, Vendor_TrxID, Vendor_Type, "Branch1");
                        DisplayFoundResultsChild(Session["returnXML"].ToString(), gvBranch1, Vendor_TrxID, Vendor_Type, TreeNode, "gvBranch1", PreviousNode);
                        // ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["VendorID"].ToString() + "','one');</script>");

                    }
                }
            }
        }

        protected void gvBranch1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //CHECK BRACH NODE CONTAINS LEAFS
            string LeafNode = CheckLeafAvailable("Branch1Leaf", Session["returnXML"].ToString());
            Control container = e.Row;

            DataControlRowState itemType = (DataControlRowState)e.Row.RowState;
            string ItemID = "";
            ItemID = e.Row.Cells[0].Text.Trim();

            //creating hyperlinks for the columns
            if (LeafNode != "")
            {
                if (itemType == DataControlRowState.Normal || itemType == DataControlRowState.Alternate)
                {
                    if (e.Row.DataItem == null)
                    {
                        return;
                    }
                    else
                    {
                        DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
                        string Vendor_TrxID = drvCurrent.Row.ItemArray[0].ToString();
                        string Vendor_Type = drvCurrent.Row.ItemArray[1].ToString();

                        GridView gvLeaf1 = new GridView();

                        //imgb1divLeaf
                        /* System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                         img = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgb1divLeaf");
                         img.Visible = true;
                         img.Attributes.Add("onclick", "javascript:expandcollapseSingle('b1divLeaf', 'one');");
                         */

                        //HtmlImage himg = new HtmlImage();
                        //string imgcontrol = "imgl1div" + drvCurrent.Row.ItemArray[["VendorID"].ToString();
                        //himg = (HtmlImage)e.Row.FindControl("imggvBranch1");
                        //himg.Visible = true;


                        string PreviousNode = ViewState["Branch1"].ToString();

                        ViewState["Branch1Leaf"] = LeafNode.ToString();

                        gvLeaf1 = (GridView)container.FindControl("gvLeaf1");

                        DisplayFoundResultsChild(Session["returnXML"].ToString(), gvLeaf1, Vendor_TrxID, Vendor_Type, LeafNode, "gvLeaf1", PreviousNode);
                    }
                }
                //CriteriaChildDG1Leaf
            }
        }

        protected void gvLeaf1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string LeafNode = CheckLeafAvailable("Branch1LeafCell1", Session["returnXML"].ToString());
            Control container = e.Row;


            //creating hyperlinks for the columns
            DataControlRowState itemType = (DataControlRowState)e.Row.RowState;

            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;

            if (itemType == DataControlRowState.Normal || itemType == DataControlRowState.Alternate)
            {
                if (e.Row.DataItem == null)
                {
                    return;
                }
                else
                {

                    if (LeafNode != "")
                    {
                        //  DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
                        string Vendor_TrxID = drvCurrent.Row.ItemArray[0].ToString();
                        string Vendor_Type = drvCurrent.Row.ItemArray[1].ToString();

                        GridView gvCell1 = new GridView();

                        //imggvBranch1
                        //imggvBranch1
                        /*  System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                          img = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgb1divLeafCell");
                          img.Visible = true;
                          img.Attributes.Add("onclick", "javascript:expandcollapseSingle('b1divLeafCell', 'one');");
                          */


                        string PreviousNode = ViewState["Branch1Leaf"].ToString();

                        ViewState["Branch1LeafCell"] = LeafNode.ToString();

                        gvCell1 = (GridView)container.FindControl("gvCell1");

                        // DisplayFoundResultsChild(Session["returnXML"].ToString(), gvCell1, Vendor_TrxID, Vendor_Type, LeafNode, "gvCell1", PreviousNode);

                        DisplayFoundResultsDynamic(Session["returnXML"].ToString(), gvCell1, Vendor_TrxID, Vendor_Type, LeafNode, "gvCell1", PreviousNode);
                    }
                }
            }
        }

        protected void gvCell1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem == null)
                {
                    return;
                }
                else
                {
                    DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
                    string PreviousNode = ViewState["Branch1LeafCell"].ToString();
                    //Finding Literal control to add radio button
                    Literal lit = (Literal)e.Row.Cells[0].FindControl("litRbtn");
                    if (e.Row.RowIndex == 0 && RowsIndex != 0)
                    {
                        RowsIndex = RowsIndex;
                        if ((Session["SelectedRow"] != null) && (Session["DeleteReport"] == null))
                        {
                            if (Session["SelectedRow"].ToString() == e.Row.RowIndex.ToString())
                            {
                                lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell' checked='checked'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            }
                            else
                            {
                                lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            }
                        }
                        else
                        {
                            lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            // lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + drvCurrent.Row.ItemArray[0].ToString() + "') />", e.Row.RowIndex);
                        }
                        RowsIndex = RowsIndex + 1;
                    }
                    else
                    {
                        RowsIndex = RowsIndex;
                        if ((Session["SelectedRow"] != null) && (Session["DeleteReport"] == null))
                        {
                            if (Session["SelectedRow"].ToString() == e.Row.RowIndex.ToString())
                            {
                                lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell' checked='checked'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            }
                            else
                            {
                                lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            }
                        }
                        else
                        {
                            lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + RowsIndex + "') />", RowsIndex);
                            // lit.Text = String.Format("<input type='radio' ID='rbtngvCell' name='rbtngvCell'  value='" + GetRowXml(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode) + "' onchange=javascript:UpdateRadioData('" + drvCurrent.Row.ItemArray[0].ToString() + "') />", e.Row.RowIndex);
                        }
                        RowsIndex = RowsIndex + 1;
                    }

                    //// Programmatically reference the Hyperlink
                    HtmlAnchor link = (HtmlAnchor)e.Row.Cells[1].FindControl("hypReportNode");

                    //GetToolTipText(string LeafNode, string TrxID)
                    StringBuilder sbTipTable = new StringBuilder();

                    GetToolTipText(drvCurrent.Row.ItemArray[1].ToString(), PreviousNode, sbTipTable);

                    string ToolTipTable = sbTipTable.ToString();
                    ToolTipTable = ToolTipTable.Replace("'", "&quot;");
                    ToolTipTable = ToolTipTable.Replace("&amp;", "");

                    string BGIMG = Application["ImagesCDNPath"].ToString()+ "Images/gradiant2.jpg";

                    //string OnMouseOverScript = string.Format("Tip('{0}',COPYCONTENT, false, BORDERWIDTH, 1, PADDING, 10 ,BGIMG,'" + BGIMG + "')", ToolTipTable);
                    //link.Attributes.Add("onmouseover", OnMouseOverScript);
                    //link.Attributes.Add("onmouseout", "UnTip()");

                    strJQueryTips = strJQueryTips + " ShowToolTip('" + link.ClientID + "','" + ToolTipTable + "');";
                    //strJQueryTips = strJQueryTips + " ShowToolTip('" + link.ClientID + "','<table><tr><td>Lajit</td></tr><tr><td>PSKR</td></tr></table>');";

                }
            }

        }

        #region Ajax Method to maintain radiobutton selected value
        /// <summary>
        /// To Set ViewState of Selected Radio button value...
        /// </summary>
        /// <param name="selectedRow"></param>
        ///[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetSelectedRadiobutton(string RowIndex)
        {
            System.Web.HttpContext.Current.Session["SelectedRow"] = RowIndex;
        }
        #endregion


        #region Ajax Method to maintain radiobutton selected value
        /// <summary>
        /// To Set ViewState of Selected Radio button value...
        /// </summary>
        /// <param name="selectedRow"></param>
        ///[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetSelectedRadiobuttonSession()
        {
            ForeGroundRpting();
        }
        #endregion
        protected void btnOK_Click(object sender, EventArgs e)
        {
            //if (mpePrint.PopupControlID == "pnlMsg")
            //{
            //    mpePrint.PopupControlID = "pnlPrint";
            //}
            //mpePrint.Hide();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //if (mpePrint.PopupControlID == "pnlMsg")
            //{
            //    mpePrint.PopupControlID = "pnlPrint";
            //}
            //mpePrint.Hide();
        }
        protected void imgbtnPrintOk_Click(object sender, EventArgs e)
        {
            ////Message Panel
            //if (mpePrint.PopupControlID == "pnlMsg")
            //{
            //    mpePrint.Hide();
            //    mpePrint.Dispose();
            //}
            //// Print Panel
            //else if (mpePrint.PopupControlID == "pnlPrint")
            //{
            //    //Getting the data from
            //    //ForeGroundRpting();

            //    //Generates the report and displays it to the user
            //    //GenerateReport();
            //}
        }

        #region Report Methods
        /// <summary>
        /// Get Report BPGID
        /// </summary>
        /// <returns></returns>
        private string GetReportBPGID()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(Session["returnXML"].ToString());

            string BPGID = "";
            BPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess [@ID='GetMyReport']").Attributes["BPGID"].Value;
            return BPGID;
        }
        /// <summary>
        /// Saves the report in the DB 
        /// <BusinessProcess ID="GetMyReport" BPGID="9" /> 
        /// </summary>
        /// <param></param>
        private void ForeGroundRpting()
        {
            try
            {

                string currentBPGID = GetReportBPGID();

                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                //nodeRoot.InnerXml = Session["BPE"].ToString();
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                //Creating the bpinfo node
                XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
                nodeRoot.AppendChild(nodeBPInfo);

                //Creating the BPGID node
                XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodeBPGID.InnerText = currentBPGID;
                nodeBPInfo.AppendChild(nodeBPGID);

                nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>5</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";


                string requestXMl = nodeRoot.OuterXml; //GenerateGVRequestXML(currentBPGID);   //objBO.GetDataForCPGV1(GenerateGVRequestXML(currentBPGID));
                //selected Rows
                //string SelectedRow = hdnSelectedRows.Value.ToString();
                string SelectedRow = Session["SelectedRow"].ToString();
                Session.Remove("SelectedRow");

                XmlDocument xdocRows = new XmlDocument();
                // xdocRows.LoadXml(nodeRoot.OuterXml.ToString());
                xdocRows.LoadXml(requestXMl);

                XmlNode nodeSelctedRow = xdocRows.SelectSingleNode("Root/bpinfo");
                nodeSelctedRow.InnerXml += SelectedRow.ToString();

                // Adding BPAction="Find"
                XmlNode nodeRows = xdocRows.SelectSingleNode("Root/bpinfo/ReportNode/RowList/Rows");
                XmlAttribute attBPAction = xdocRows.CreateAttribute("BPAction");
                attBPAction.Value = "Find";
                nodeRows.Attributes.Append(attBPAction);
                //Retriieving the BPEOut and ReportOut XML

                string reqxml = xdocRows.OuterXml;
                //Chandu
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(reqxml);
                Session["LinkBPinfo"] = "<bpinfo>" + xDoc.SelectSingleNode("//bpinfo").InnerXml.ToString() + "</bpinfo>";
                //
                //Session["BPINFO"] = nodeBPInfo.OuterXml.ToString();
                //string ReturnXML = objBO.GetDataForReports(reqxml);
                //ViewState["ReportReturnXml"] = ReturnXML;

            }
            catch //(Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// Generates the selected report
        /// </summary>
        /// <param></param>
        private void GenerateReport()
        {
            try
            {
                //If xml exists in phsical path, loading and saving it in viewstate
                if (ViewState["ReportReturnXml"].ToString().Contains("FileName"))
                {
                    XmlDocument xDoc = new XmlDocument();
                    XmlNode nodeRows;
                    string filenodename = string.Empty;
                    xDoc.LoadXml(ViewState["ReportReturnXml"].ToString());

                    filenodename = xDoc.SelectSingleNode("Root/Reportout/FileName").InnerText;
                    string reportFilePath = ConfigurationManager.AppSettings["XmlFilePath"].ToString().Trim();
                    //string appPath = HttpContext.Current.Request.ApplicationPath;
                    //string physicalPath = HttpContext.Current.Request.MapPath(appPath);
                    // Checking for the report path existence as per the config key
                    //if (Directory.Exists(physicalPath + reportFilePath))
                    if (Directory.Exists(reportFilePath))
                    {
                        // Naming convention for the Report generated 
                        //string xmlfilepath = physicalPath + reportFilePath + @"\" + filenodename + ".xml";
                        string xmlfilepath = reportFilePath + @"\" + filenodename + ".xml";
                        // Getting the file info from the specified file path
                        FileInfo f = new FileInfo(xmlfilepath);
                        // Read the file if it already exist 
                        if (f.Exists)
                        {
                            string fileRead = string.Empty;
                            StreamReader reader = new StreamReader(xmlfilepath);
                            fileRead = reader.ReadToEnd();
                            reader.Close();
                            xDoc.RemoveAll();
                            xDoc.LoadXml(fileRead);
                            ViewState["ReportReturnXml"] = xDoc.OuterXml;
                        }
                    }
                }

                if (ViewState["ReportReturnXml"].ToString() != string.Empty)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(ViewState["ReportReturnXml"].ToString());

                    XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
                    string treeNodeName = string.Empty;
                    if (nodeTreenode.SelectSingleNode("Node") != null)
                    {
                        treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
                    }

                    //Generating the Document
                    //PDF
                    if (rbPDF.Checked)
                    {
                        //reportObjUI.GenerateReport(ViewState["ReportReturnXml"].ToString());
                        XmlDocument xDocOut = new XmlDocument();
                        xDocOut.LoadXml(ViewState["ReportReturnXml"].ToString());
                        reportObjUI.GenerateReport(xDocOut, "PDF");

                        ////Get the Datatable to print
                        ////DataTable dt = XMLToDataTable();
                        //// XMLToDataTable(string GVXml)
                        //DataTable dt = reportObjUI.XMLToDataTable(ViewState["ReportReturnXml"].ToString());
                        ////Export to PDF
                        //GVExportToPDF(dt, "PDFRpt");
                    }
                    //Excel
                    else if (rbExcel.Checked)
                    {
                        //Get the Datatable to print
                        DataTable dt = reportObjUI.XMLToDataTable(ViewState["ReportReturnXml"].ToString(), treeNodeName, "EXCEL");
                        if (dt.Rows.Count > 0)
                        {
                            //Removing TrxID column
                            if (dt.Columns.Contains("TrxID"))
                            {
                                dt.Columns.Remove("TrxID");
                            }
                            //if (dt.Columns.Contains("Notes"))
                            //{
                            //    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            //    dt.Columns.Remove("Notes");
                            //}
                            //Export to Excel
                            ExportDatatableToExcel(dt, "ExcelRpt");
                        }
                    }
                    //HTML
                    else if (rbHTML.Checked)
                    {
                        //Get the Datatable to print
                        // DataTable dt = XMLToDataTable();
                        DataTable dt = reportObjUI.XMLToDataTable(ViewState["ReportReturnXml"].ToString(), treeNodeName, "");
                        if (dt.Rows.Count > 0)
                        {
                            //Removing TrxID column
                            //if (dt.Columns.Contains("TrxID"))
                            //{
                            //    dt.Columns.Remove("TrxID");
                            //}
                            //if (dt.Columns.Contains("Notes"))
                            //{
                            //    NotesDT = reportsBO.GenerateNotesDatatable(dt);
                            //    dt.Columns.Remove("Notes");
                            //}
                            //Export to HTML
                            //ExportDatatableToHTML(dt, "HtmlRpt", NotesDT, rptPrintNotes);
                            XmlDocument xDocOut = new XmlDocument();
                            xDocOut.LoadXml(ViewState["ReportReturnXml"].ToString());
                            clsReportsUI objReportsUI = new clsReportsUI();
                            objReportsUI.GenerateReport(xDocOut, "HTML");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }

        private void GVExportToPDF(DataTable dt, string fileName)
        {
            try
            {
                // Varaible to get the Row and Column count of three tables
                int rowsInTab = dt.Rows.Count;
                int colsInTab = dt.Columns.Count;

                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.Letter_8_5x11_Horizontal);

                Font FontBold = new Font("Verdana", 9, FontStyle.Bold);

                PdfTable myPdfTable = myPdfDocument.NewTable(FontBold, rowsInTab, colsInTab, 4);

                //Import DT to PDF table
                myPdfTable.ImportDataTable(dt);

                myPdfTable.HeadersRow.SetColors(Color.Black, Color.SkyBlue);
                myPdfTable.SetColors(Color.Black, Color.White, Color.Gainsboro);
                myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                //Setting columns width based on the No of columns
                int[] Arraywidth = new int[colsInTab];
                for (int colCnt = 0; colCnt < colsInTab; colCnt++)
                {
                    Arraywidth[colCnt] = 25;
                }
                myPdfTable.SetColumnsWidth(Arraywidth);
                //myPdfTable.SetColumnsWidth(new int[] { 100,100});
                myPdfTable.SetRowHeight(5);

                //Now we set some alignment... for the whole table and then, for a column:
                myPdfTable.SetContentAlignment(ContentAlignment.MiddleCenter);

                while (!myPdfTable.AllTablePagesCreated)
                {
                    PdfPage newPdfPage = myPdfDocument.NewPage();

                    PdfTextArea pta = new PdfTextArea(new Font("Verdana", 10, FontStyle.Bold),
                                        Color.Red, new PdfArea(myPdfDocument, 30, 20, 100, 50),
                                        ContentAlignment.MiddleCenter, "Vendor List");

                    PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 50, 60, 650, 250));

                    newPdfPage.Add(newPdfTablePage);
                    newPdfPage.Add(pta);

                    newPdfPage.SaveToDocument();

                }

                Response.ClearHeaders();
                Response.AppendHeader("Content-disposition",
                string.Format("attachment;filename={0}", "Report.pdf"));
                Response.ContentType = "application/pdf";
                myPdfDocument.SaveToStream(Response.OutputStream);
                Response.End();
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }

        }
        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        private string GenerateGVRequestXML(string BPGID, string pageSize)
        {
            #region NLog
            logger.Info("Generated the xml to request data to be bound for the grid view with BPGID as : "+BPGID+" adn page size as : " + pageSize);
            #endregion

            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            // nodeRoot.InnerXml = Session["BPE"].ToString();
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = BPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>1</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            return nodeRoot.OuterXml;

        }
        #endregion Report Methods

        #region Export To Excel
        //Excel document to be downloadable
        /// <summary>
        /// Exports the grid view to Excel
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="fileName">Name of the file to be printed</param>
        private void ExportDatatableToExcel(DataTable dt, string fileName)
        {
            #region NLog
            logger.Info("This to export the gridview to the excel document with filename as : " + fileName);
            #endregion

            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string filename = fileName;

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "";
            response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition",
            string.Format("attachment;filename={0}.xls", fileName));

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    GridView dv = new GridView();
                    dv.DataSource = dt;
                    dv.DataBind();
                    dv.RenderControl(htw);
                    response.Write(style);
                    response.Write(sw.ToString());
                    //response.End();
                    response.Flush();
                    response.Close();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        #endregion

        #region Export To HTML

        /// <summary>
        /// Exports the grid view to HTML
        /// </summary>
        /// <param name="dt">Data table to be printed</param>
        /// <param name="fileName">Name of the file to be printed</param>
        private void ExportDatatableToHTML(DataTable dt, string fileName)
        {
            #region NLog
            logger.Info("This to export the gridview to the HTML document with filename as : " + fileName);
            #endregion

            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string filename = fileName;

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "";
            response.ContentType = "application/vnd.html";
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition",
            string.Format("attachment;filename={0}.html", fileName));

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    GridView dv = new GridView();
                    dv.DataSource = dt;
                    dv.DataBind();
                    dv.RenderControl(htw);
                    response.Write(style);
                    response.Write(sw.ToString());
                    //response.End();
                    response.Flush();
                    response.Close();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        #endregion

        #region Delete SelectedReport
        /// <summary>
        /// Get Report BPGID
        /// </summary>
        /// <returns></returns>
        private string GetDeleteReportBPGID()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(Session["returnXML"].ToString());

            string BPGID = "";
            BPGID = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls//BusinessProcess [@ID='Delete']").Attributes["BPGID"].Value;
            return BPGID;
        }
        /// <summary>
        /// Delete Report
        /// </summary>
        /// <returns></returns>
        private string DeleteReport()
        {

            // <BusinessProcess ID="Delete" BPGID="15" /> 

            XmlDocument xDocAddVendor = new XmlDocument();
            XmlNode nodeRoot = xDocAddVendor.CreateNode(XmlNodeType.Element, "Root", null);
            //nodeRoot.InnerXml = Session["BPE"].ToString();
            nodeRoot.InnerXml = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocAddVendor.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            //prevFormInfo = prevFormInfo.Replace("<FormInfo>", "<PriorFormInfo>");
            //prevFormInfo = prevFormInfo.Replace("</FormInfo>", "</PriorFormInfo>");
            //nodeBPInfo.InnerXml += prevFormInfo;


            //Creating the BPGID node
            XmlNode nodeBPGID = xDocAddVendor.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = GetDeleteReportBPGID();
            nodeBPInfo.AppendChild(nodeBPGID);


            //selected Rows

            string SelectedRow = hdnSelectedRows.Value.ToString();

            XmlDocument xdocRows = new XmlDocument();
            xdocRows.LoadXml(nodeRoot.OuterXml.ToString());

            XmlNode nodeSelctedRow = xdocRows.SelectSingleNode("Root/bpinfo");
            nodeSelctedRow.InnerXml += SelectedRow.ToString();

            // Adding BPAction="Find"
            XmlNode nodeRows = xdocRows.SelectSingleNode("Root/bpinfo/ReportNode/RowList/Rows");
            XmlAttribute attBPAction = xdocRows.CreateAttribute("BPAction");
            attBPAction.Value = "Delete";
            nodeRows.Attributes.Append(attBPAction);


            //Retriieving the BPEOut and ReportOut XML
            //string reqxml = nodeRoot.OuterXml;
            string reqxml = xdocRows.OuterXml;

            //Creating the Vendor node

            //XmlNode nodeVendor = xDocAddVendor.CreateNode(XmlNodeType.Element, "MyReports", null);
            //nodeVendor.InnerXml += "<BPAction>Delete</BPAction>";
            //nodeBPInfo.AppendChild(nodeVendor);
            //Creating the Vendor ID node
            //XmlNode nodeVendorID = xDocAddVendor.CreateNode(XmlNodeType.Element, "VendorID", null);
            //nodeVendorID.InnerText = hdnVendorId.Value;
            //nodeVendor.AppendChild(nodeVendorID);
            //nodeRoot.AppendChild(nodeBPInfo);

            //return objBO.DeleteReport(nodeRoot.OuterXml);
            return objBO.DeleteReport(reqxml);
        }
        #endregion

        protected void imgbtnDelOk_Click(object sender, EventArgs e)
        {

            // Thread.Sleep(3000);

            //Generate XML request and get response 
            string deleteStatus;
            XmlDocument xDoc = new XmlDocument();
            deleteStatus = DeleteReport();
            xDoc.LoadXml(deleteStatus);
            string msgInfo = "";
            //Get the Message node
            msgInfo = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message/Status").InnerText;
            //<Status>Success</Status> Status>Success
            if (msgInfo == "Success")
            {
                Session["DeleteReport"] = hdnSelectedRows.Value.ToString();
                hdnSelectedRows.Value = "";
                RowsIndex = 0;
                RemoveAndDataBind();
            }
            else
            {
                hdnSelectedRows.Value = "";
            }

        }
    }
}
