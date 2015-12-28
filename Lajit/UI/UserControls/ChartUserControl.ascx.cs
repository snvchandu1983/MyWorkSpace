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
using System.Text;
using System.Drawing;
using NLog;


using Dundas.Charting.WebControl;



namespace LAjitDev.UserControls
{
    public partial class ChartUserControl : System.Web.UI.UserControl
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        LAjitDev.CommonUI commonObjUI = new CommonUI();

        /// <summary>
        /// Property containd gridview dataxml
        /// </summary>
        public String GVDataXml
        {
            get { return (String)ViewState["GVDataXml"]; }
            set { ViewState["GVDataXml"] = value; }
        }


        public String ChartTemplateName
        {
            get { return (String)ViewState["ChartTemplateName"]; }
            set { ViewState["ChartTemplateName"] = value; }
        }

        public String ChartIframeID
        {
            get { return (String)ViewState["ChartIframeID"]; }
            set { ViewState["ChartIframeID"] = value; }
        }
        public String ChartModalPopupID
        {
            get { return (String)ViewState["ChartModalPopupID"]; }
            set { ViewState["ChartModalPopupID"] = value; }
        }

        public String ChartHeight
        {
            get { return (String)ViewState["ChartHeight"]; }
            set { ViewState["ChartHeight"] = value; }
        }

        public String ChartWidth
        {
            get { return (String)ViewState["ChartWidth"]; }
            set { ViewState["ChartWidth"] = value; }
        }

        public String ChartCurrentImage
        {
            get { return (String)ViewState["ChartCurrentImage"]; }
            set { ViewState["ChartCurrentImage"] = value; }
        }

        public String ChartEnableViewState
        {
            get { return (String)ViewState["ChartEnableViewState"]; }
            set { ViewState["ChartEnableViewState"] = value; }
        }

        string chartPath = ConfigurationManager.AppSettings["ChartFilePath"];
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    AddScriptReferences();

                    BindChart();
                }
            }
            catch
            { 
            }
        }

        #region Private Methods
        
      
        private void AddScriptReferences()
        {
      
            //CDN Added Scripts

            //ChartUserControl.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ChartUserControl.js");
            Page.Header.Controls.Add(hgcScript1);
        }


        public void BindChart()
        {
            try
            {
                if ((GVDataXml != string.Empty) && (GVDataXml != null))
                {
                    centrChart.Visible = true;
                    pnlErrmsg.Visible = false;
                    lblmsg.Visible = false;

                    XmlDocument m_xdoc = new XmlDocument();
                    m_xdoc.LoadXml(GVDataXml);

                   // m_xdoc.Load("D:\\ZSwami\\My Needs\\2010\\April\\21\\chat1.xml");

                    //m_xdoc.Load("D:\\ZSwami\\My Needs\\2010\\April\\21\\BPschema2.xml");

                    //GVDataXml = m_xdoc.OuterXml.ToString();

                    DataSet ds = new DataSet();

                    string m_GVTreeNodeName = m_xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                
                   // XmlNodeList m_XnodeList = m_xdoc.SelectNodes("//RowList/Rows");

                    XmlNodeList m_XnodeList = m_xdoc.SelectNodes("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList/Rows");

                    if (m_XnodeList.Count == 0)
                    {
                        centrChart.Visible = false;
                        pnlErrmsg.Visible = true;
                        lblmsg.Visible = true;
                        lblmsg.Text = "Data not available";
                        return;
                    }


                    XmlNode nodeRows;
                    if (m_XnodeList.Count > 0)
                    {
                        string m_XEleName = m_xdoc.SelectSingleNode("//RowList/Rows").Name;
                        nodeRows = m_xdoc.CreateElement(m_XEleName);
                        foreach (XmlNode xnode in m_XnodeList)
                        {
                            //Removing duplicates and sum the y axis values
                            XmlNodeList m_InnerRows = nodeRows.SelectNodes("Rows");
                            bool SumStatus = false;

                            // X-Axis value is date Value change format to MM/DD/YYYY
                            if (commonObjUI.IsDate(xnode.Attributes["XaxisValue"].Value))
                            {
                                //if the value is IsDate then change format MM/DD/YYYY
                                DateTime date;
                                DateTime.TryParse(xnode.Attributes["XaxisValue"].Value, out date);
                                string formateDate = date.ToString("MM/dd/yy");
                                xnode.Attributes["XaxisValue"].Value = formateDate;
                            }
                   

                            if (m_InnerRows.Count == 0)
                            {
                                nodeRows.AppendChild(xnode);
                            }
                            else
                            {
                                // Check added rows having value
                                foreach (XmlNode m_innerRow in m_InnerRows)
                                {
                                    if (m_innerRow.Attributes["XaxisValue"].Value == xnode.Attributes["XaxisValue"].Value)
                                    {
                                        double Y_AxisValue, InnerValue, OuterValue;
                                        double.TryParse(m_innerRow.Attributes["YaxisValue"].Value, out InnerValue);
                                        double.TryParse(xnode.Attributes["YaxisValue"].Value, out OuterValue);
                                        Y_AxisValue=InnerValue+OuterValue;

                                        
                                      m_innerRow.Attributes["YaxisValue"].Value = string.Format("{0:#.00}",Y_AxisValue.ToString());
                                      SumStatus = true;
                                    }
                                }
                                if (!SumStatus)
                                { 
                                  // Add row
                                    nodeRows.AppendChild(xnode);
                                }
                            }
                            //nodeRows.AppendChild(xnode);
                        }
                        if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                        {
                            XmlNodeReader read = new XmlNodeReader(nodeRows);
                            ds.ReadXml(read);
                        }
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // Based on the user selection the chart changes

                        //Checking if any yaxis value is empty nothing to show in chart..
                        if ((ds.Tables[0].Rows[0]["YaxisValue"].ToString() == string.Empty) || (ds.Tables[0].Rows[0]["YaxisValue"].ToString() == string.Empty))
                        {
                            centrChart.Visible = false;
                            pnlErrmsg.Visible = true;
                            lblmsg.Visible = true;
                            lblmsg.Text = "Data not available";
                            return;
                        }


                        //ChartType from db 
                        string m_ChartType = string.Empty;

                        if (ds.Tables[0].Rows[0]["ChartType"] != null)
                        {
                            m_ChartType = ds.Tables[0].Rows[0]["ChartType"].ToString();
                        }

                        //  m_ChartType = "PieChart";

                        // m_ChartType = "AreaChart";


                       //Create Chart Series Default
                       centrChart.Series.Add("Default");


                        if (m_ChartType != string.Empty)
                        {
                            switch (m_ChartType)
                            {
                                case "AreaChart":     //old chartname in xml
                                case "Area":
                                    centrChart.Series["Default"].Type = SeriesChartType.Area;

                                    // Set Show of marker Lines
                                    // centrChart.Series["Default"]["ShowMarkerLines"] ="True";
                                    // Disable/enable X axis margin
                                    //centrChart.Series["Default"].AxisX.Margin = "True";

                                    //None Simplistic Realistic

                                    //centrChart.ChartAreas["Default"].Area3DStyle.Light = (LightStyle)LightStyle.Parse(typeof(LightStyle), "Simplistic");

                                    ////Set the AxisX Interval to avoid automatic interval feature
                                    //centrChart.ChartAreas["Default"].AxisX.Interval = 1;
                                    //show as 3D
                                    centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = true;
                                    //show as 2D
                                    // centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = false;
                                    break;
                                case "Bar":
                                    centrChart.Series["Default"].Type = SeriesChartType.Bar;
                                    //show as 3D
                                    centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = true;
                                    //show as 2D
                                    //centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = false;
                                    break;
                                case "Bubble":
                                    centrChart.Series["Default"].Type = SeriesChartType.Bubble;
                                    break;
                                case "CandleStick":
                                    centrChart.Series["Default"].Type = SeriesChartType.CandleStick;
                                    break;
                                case "ColumnChart":
                                case "Column":
                                    centrChart.Series["Default"].Type = SeriesChartType.Column;
                                    break;
                                case "Doughnut":
                                    centrChart.Series["Default"].Type = SeriesChartType.Doughnut;
                                    centrChart.Series["Default"]["PieLabelStyle"] = "Disabled";
                                    centrChart.Legends["Default"].Docking = LegendDocking.Bottom;
                                    break;
                                case "SmoothLineChart": //old chartname in xml
                                case "Line":
                                    centrChart.Series["Default"].Type = SeriesChartType.Line;
                                    break;
                                case "PieChart": //old chartname in xml
                                case "Pie":
                                    centrChart.Series["Default"].Type = SeriesChartType.Pie;
                                    centrChart.Series["Default"]["PieDrawingStyle"] = "SoftEdge";
                                    //Inside Outside Disabled
                                    centrChart.Series["Default"]["PieLabelStyle"] = "Disabled";
                                    centrChart.Legends["Default"].Docking = LegendDocking.Bottom;

                                    //centrChart.Legends[0].Alignment = LegendDocking.Bottom;
                                    // Set the collected pie slice to be exploded

                                    //Series series1 = centrChart.Series[0];
                                    //series1["CollectedSliceExploded"] = "True";
                                    //series1.Type = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), "Pie", true);

                                    // Explode selected slice
                                    //if (this.Page.Request["pointIndex"] != null)
                                    //{
                                    //    int pointIndex = int.Parse(this.Page.Request["pointIndex"]);
                                    //    if (pointIndex >= 0 && pointIndex < centrChart.Series["Default"].Points.Count)
                                    //    {
                                    //        centrChart.Series["Default"].Points[pointIndex].CustomAttributes += "Exploded=true";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    centrChart.Series["Default"].Points[0].CustomAttributes += "Exploded=true";
                                    //}

                                    break;
                                case "Point":
                                    centrChart.Series["Default"].Type = SeriesChartType.Point;
                                    break;
                                case "Spline":
                                    centrChart.Series["Default"].Type = SeriesChartType.Spline;
                                    break;
                                case "SplineArea":
                                    centrChart.Series["Default"].Type = SeriesChartType.SplineArea;
                                    break;
                                case "StackedArea":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedArea;
                                    break;
                                case "StackedArea100":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedArea100;
                                    break;
                                case "StackedBar":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedBar;
                                    break;
                                case "StackedBar100":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedBar100;
                                    break;
                                case "StackedColumn":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedColumn;
                                    break;
                                case "StackedColumn100":
                                    centrChart.Series["Default"].Type = SeriesChartType.StackedColumn100;
                                    break;
                                case "StepLine":
                                    centrChart.Series["Default"].Type = SeriesChartType.StepLine;
                                    break;
                                case "Stock":
                                    centrChart.Series["Default"].Type = SeriesChartType.Stock;
                                    break;
                            }
                            // centrChart.Series["Default"].Type = SeriesChartType.Pie;

                        }

                        //Set Area3DStyle Expect Pie and doughnut
                        if ((m_ChartType == "PieChart") || (m_ChartType == "Pie") || (m_ChartType == "Doughnut"))
                        {
                            //2D
                            centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = false;
                        }
                        else
                        {   //3D
                            centrChart.ChartAreas["Default"].Area3DStyle.Enable3D = true;
                        }
                        
                        //Set Datasource                        
                        centrChart.DataSource = ds.Tables[0];
                        
                        //Series1
                        centrChart.Series["Default"].ValueMemberX = "XaxisValue";
                        centrChart.Series["Default"].ValueMembersY = "YaxisValue";

                        //Series2
                        bool legendSeries2 = false;
                        if (ds.Tables[0].Columns.Contains("YaxisValue2"))
                        {
                            //Create Chart Series2
                            centrChart.Series.Add("Default2");

                            //Show or hide legend
                            legendSeries2 = true;

                            //Set Legend Series1
                             if (ds.Tables[0].Columns.Contains("YaxisLegend"))
                             {
                                 centrChart.Series["Default"].LegendText = ds.Tables[0].Rows[0]["YaxisLegend"].ToString(); 
                             }
                             //Set Legend Series2
                             if (ds.Tables[0].Columns.Contains("YaxisLegend2"))
                             {
                                centrChart.Series["Default2"].LegendText = ds.Tables[0].Rows[0]["YaxisLegend2"].ToString(); 
                             }
                            //Set Series X and Y values
                            centrChart.Series["Default2"].ValueMemberX = "XaxisValue";
                            centrChart.Series["Default2"].ValueMembersY = "YaxisValue2";
                        }
                        
                        centrChart.DataBind();


                        //Set Chart Border Color
                        centrChart.Series["Default"].BorderColor = Color.FromArgb(180, 26, 59, 105);

                        //Set Y Axis Type to double
                        centrChart.Series["Default"].YValueType = ChartValueTypes.Double;

                        //Set XAxis Font  
                        //Note : both X and Y font should be same other wise chart not created
                        /*    if (ds.Tables[0].Rows[0]["XaxisFont"] != null)
                           {
                               switch (ds.Tables[0].Rows[0]["XaxisFont"].ToString())
                               {
                                   case "1":
                                       centrChart.ChartAreas["Default"].AxisX.TitleFont = new Font("Arial", float.Parse("8"));
                                       break;
                                   case "2":
                                       centrChart.ChartAreas["Default"].AxisX.TitleFont = new Font("Trebuchet MS", float.Parse("9"));
                                       break;
                               }

                           }
                          
                           //Set YAxis Font
                           if (ds.Tables[0].Rows[0]["YaxisFont"] != null)
                           {
                               switch (ds.Tables[0].Rows[0]["YaxisFont"].ToString())
                               {
                                   case "1":
                                       centrChart.ChartAreas["Default"].AxisY.TitleFont = new Font("Arial", float.Parse("8"));
                                       break;
                                   case "2":
                                       centrChart.ChartAreas["Default"].AxisY.TitleFont = new Font("Trebuchet MS", float.Parse("9"));
                                       break;
                               }

                           }*/





                        //Set the AxisX Title
                        if (ds.Tables[0].Rows[0]["XaxisText"] != null)
                        {
                            centrChart.ChartAreas["Default"].AxisX.Title = ds.Tables[0].Rows[0]["XaxisText"].ToString();
                        }

                        //Set the AxisY Title
                        if (ds.Tables[0].Rows[0]["YaxisText"] != null)
                        {
                            centrChart.ChartAreas["Default"].AxisY.Title = ds.Tables[0].Rows[0]["YaxisText"].ToString();
                        }

                        //Set the Legends Visiblity
                        if (ds.Tables[0].Rows[0]["IsLegendDisplayed"] != null)
                        {
                            if ((m_ChartType == "Pie") || (m_ChartType == "Doughnut") || (m_ChartType == "PieChart") || (legendSeries2 == true))
                            {
                                int IsLegendValue = Convert.ToInt32(ds.Tables[0].Rows[0]["IsLegendDisplayed"].ToString());

                                if (IsLegendValue == 1)
                                {
                                    this.centrChart.Legends[0].Enabled = true;

                                }
                                else
                                {
                                    this.centrChart.Legends[0].Enabled = false;
                                }
                            }
                            else
                            {
                                this.centrChart.Legends[0].Enabled = false;
                            }
                        }

                        //Set Chart Title

                        //if (m_xdoc.SelectSingleNode("//GridHeading/Title") != null)
                        //{
                        //    centrChart.Titles.Add(m_xdoc.SelectSingleNode("//GridHeading/Title").InnerText);

                        //    /*  Temlate style is blue not working 
                        //     * Font titleFont=new Font("Trebuchet MS",14.25f,FontStyle.Bold);
                        //      centrChart.Titles.Add(m_xdoc.SelectSingleNode("//GridHeading/Title").InnerText, Docking.Top, titleFont, Color.FromArgb(26, 59, 105));
                        //      centrChart.Titles["Default"].ShadowOffset = 3;
                        //      centrChart.Titles["Default"].ShadowColor=Color.FromArgb(32,0,0,0);*/
                        //}


                        //Set the ChartTitle Visiblity
                        if (ds.Tables[0].Rows[0]["IsChartNameDisplayed"] != null)
                        {
                            int IsChartNameDisplayed = Convert.ToInt32(ds.Tables[0].Rows[0]["IsChartNameDisplayed"].ToString());

                            if (IsChartNameDisplayed == 1)
                            {
                                //this.centrChart.Titles[0]
                                this.centrChart.Titles[0].Visible = true;
                            }
                            else
                            {
                                //this.centrChart.Legends[0].Enabled=false;
                                this.centrChart.Titles[0].Visible = false;
                            }
                        }
                      

                        //Set ChartTitle
                        string strChartTitle = string.Empty;
                        
                        if (m_xdoc.SelectSingleNode("//GridHeading/Title") != null)
                        {
                            strChartTitle = m_xdoc.SelectSingleNode("//GridHeading/Title").InnerText;
                           
                            if (m_xdoc.SelectSingleNode("//GridHeading/SubTitle") != null)
                            {
                                strChartTitle = strChartTitle + m_xdoc.SelectSingleNode("//GridHeading/SubTitle").InnerText;
                            }
                        }
                        //Set the Template
                        if ((ChartTemplateName != string.Empty) && (ChartTemplateName != null))
                        {
                            // Before Template set title
                            if (strChartTitle != string.Empty)
                            {
                                centrChart.Titles.Add(strChartTitle);
                            }
                            //Set Template file
                            centrChart.Serializer.TemplateMode = true;
                            string fileNameString = System.AppDomain.CurrentDomain.BaseDirectory;
                            fileNameString += "\\ChartTemplates\\" + ChartTemplateName + ".xml";
                            centrChart.LoadTemplate(fileNameString);
                        }
                        else
                        {
                            //Set Title 
                            if (strChartTitle != string.Empty)
                            {
                                Title chartTitle = new Title(strChartTitle, Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
                                chartTitle.ShadowColor = Color.FromArgb(32, 0, 0, 0);
                                chartTitle.ShadowOffset = 3;
                                centrChart.Titles.Add(chartTitle);
                            }

                            // Set AxisX Title font Trebuchet MS, Times New Roman
                            centrChart.ChartAreas["Default"].AxisX.TitleFont = new Font("Trebuchet MS", 10, FontStyle.Bold);
                            // Set AxisX Title color
                            centrChart.ChartAreas["Default"].AxisX.TitleColor = Color.Blue;
                            // Set AxisY Title font
                            centrChart.ChartAreas["Default"].AxisY.TitleFont = new Font("Trebuchet MS", 10, FontStyle.Bold);
                            // Set AxisY Title color
                            centrChart.ChartAreas["Default"].AxisY.TitleColor = Color.Blue;
                            // Set AxisY Label Style font
                            centrChart.ChartAreas["Default"].AxisY.LabelStyle.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
                            // Set AxisX Label Style font
                            centrChart.ChartAreas["Default"].AxisX.LabelStyle.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
                        }


                        //Set Chart Width
                        if (ChartWidth != string.Empty)
                        {
                            int cwidth = Convert.ToInt32(ChartWidth.ToString());
                            centrChart.Width = Unit.Pixel(cwidth);
                        }

                        //Set Chart Height
                        if (ChartHeight != string.Empty)
                        {
                            int cheight = Convert.ToInt32(ChartHeight.ToString());
                            centrChart.Height = Unit.Pixel(cheight);
                        }

                        //Set EnableViewState Property 
                        if (ChartEnableViewState != null)
                        {
                            if (ChartEnableViewState != string.Empty)
                            {
                                bool status;
                                bool.TryParse(ChartEnableViewState.ToString(), out status);
                                centrChart.EnableViewState = status;
                            }
                        }

                        //Set Hyperlinks
                        if (m_xdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess") != null)
                        {
                            if (m_xdoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["ID"] != null)
                            {
                                //update chart attributes based on process links

                                UpdateChartAttrib();
                            }
                        }

                        //Set Chart Format
                        centrChart.ImageType = ChartImageType.Jpeg;

               
                        ////Check WebCharts Directory Exist or not otherwise create it
                        if (!System.IO.Directory.Exists(chartPath))
                        {
                            System.IO.Directory.CreateDirectory(chartPath);
                        }

                        //Set Chart Image Saved Location not required now storing in charthandler
                        //centrChart.ImageUrl = @"~\WebCharts\ChartPic_#SEQ(300,3)";

                        //Set Storage Mode
                        centrChart.ImageStorageMode = ImageStorageMode.UseHttpHandler;
                        
                        //Set Current Image filename
                        ChartCurrentImage = centrChart.GetCurrentImageUrl();
                    }
                } // Gvdataxml
                else
                {
                    centrChart.Visible = false;
                    pnlErrmsg.Visible = true;
                    lblmsg.Visible = true;
                    lblmsg.Text = "Data not available";
                }
            } //Try
            catch(Exception ex)
            {
                #region NLog
                logger.Fatal("ex"); 
                #endregion
                
               centrChart.Visible = false;
            }
        }

        private void UpdateChartAttrib()
        {

            XmlDocument xDocout = new XmlDocument();
            xDocout.LoadXml(GVDataXml);

            string m_treeNodeName = string.Empty;
            string m_processName = string.Empty;
            string m_processBPGID = string.Empty;
            string m_pageInfo = string.Empty;
            string m_IsPopup=string.Empty;

            if (xDocout.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText != null)
            {
                m_treeNodeName = xDocout.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            }

            //<Col Label="XaxisValue" Caption="XaxisValue" BPControl="Process1" SmallViewLength="0" FullViewLength="30" IsRequired="0" IsUnique="0" IsNumeric="0" IsLink="0" IsHidden="0" IsDisplayOnly="0" IsParentLink="0" IsSortable="0" IsSummed="0" IsSearched="0" SortOrder="1" ControlType="TBox" />


            XmlNode nodeColumns = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/GridHeading/Columns");


            if (xDocout.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["ID"] != null)
            {
                //Process1
                m_processName = xDocout.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess").Attributes["ID"].Value;

                if (m_processName.ToLower()!="process1")
                {
                   m_processName="Process1";
                }

                XmlNode nodeProc = xDocout.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + m_processName + "']");
                if (nodeProc != null)
                {
                    m_processBPGID = nodeProc.Attributes["BPGID"].Value;
                    m_pageInfo = nodeProc.Attributes["PageInfo"].Value;

                    if (m_pageInfo.ToLower() == "common/chartview.aspx")
                    {
                        m_pageInfo = "Common/ChartViewer.aspx";
                    }

                    m_pageInfo = m_pageInfo.Replace(" ", "");
                    m_IsPopup = nodeProc.Attributes["IsPopup"].Value;
                }
                //string pageInfo = "PopUps/PrintPopUp.aspx";
                // string processinfo = string.Empty;
                // processinfo = currentBPGID + "," + pageInfo.Replace(" ", "") + "," + nodeProc.Attributes["ID"].Value + "," + nodeProc.Attributes["IsPopup"].Value;
            }


            // Set series tooltips
            foreach (Series series in centrChart.Series)
            {
                //string tt = series.LegendText.ToString();

                for (int pointIndex = 0; pointIndex < series.Points.Count; pointIndex++)
                {
                     
                     string toolTip = string.Empty;
                     string BPINFO = string.Empty;

                     //Set ToolTip for each points
                     toolTip = series.Points[pointIndex].AxisLabel.ToString() + " - " + Convert.ToString(series.Points[pointIndex].YValues[0]);
                     series.Points[pointIndex].ToolTip = toolTip;

                
                    //toolTip = "<IMG SRC=TestRegionChart.aspx?region=" + series.Points[pointIndex].AxisLabel + ">";
                    //series.Points[pointIndex].MapAreaAttributes = "onmouseover=\"DisplayTooltip('" + toolTip + "');\" onmouseout=\"DisplayTooltip('');\"";
                    //series.Points[pointIndex].Href = "TestDetailedRegionChart.aspx?region=" + series.Points[pointIndex].AxisLabel;
                    //Wrong TrxID and XAxisValue both are same in dashboard this case ok other cases wrong
                    //XmlNode nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@TrxID='"+ series.Points[pointIndex].AxisLabel +"']");

                   XmlNode nodeRow=null;
                   if ((m_pageInfo != string.Empty) && (m_pageInfo != null))
                   {
                       /*if (series.Legend.ToString()== "Default")

                       {
                           nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue='" + series.Points[pointIndex].GetValueY(0) + "']");
                       }*/
                        switch (series.Name.ToString())
                        {
                            case "Default":
                                 nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue='" + series.Points[pointIndex].GetValueY(0) + "']");
                                 break;
                           case "Default2":
                                 nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue2='" + series.Points[pointIndex].GetValueY(0) + "']");
                                 break;
                        }

                   }
                         if (nodeRow == null)
                         {
                             //Issue with 2 digits to pickup record
                             string YValueDigits = string.Empty;
                             double checknum = 0.00;
                             if (series.Points[pointIndex].GetValueY(0) == checknum)
                             {
                                 YValueDigits = "0.00";
                             }
                             else
                             {
                                 YValueDigits = string.Format("{0:#.00}", series.Points[pointIndex].GetValueY(0));
                             }
                             //nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue='" + YValueDigits + "']");
                             switch (series.Name.ToString())
                             {
                                 case "Default":
                                     nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue='" + YValueDigits + "']");
                                     break;
                                 case "Default2":
                                     nodeRow = xDocout.SelectSingleNode("Root/bpeout/FormControls/" + m_treeNodeName + "/RowList/Rows [@YaxisValue2='" + YValueDigits + "']");
                                     break;
                             }
                         }
                    
                        
                         if (nodeRow != null)
                         {
                             //Set Hyperlinks to each segment.

                             BPINFO = GetCOXML(xDocout, nodeColumns, nodeRow.OuterXml, m_treeNodeName);

                             //Check valid url or not
                             if (m_pageInfo != string.Empty)
                             {
                                 if (!IsURLValid(m_pageInfo))
                                 {
                                     //Not a valid url pick up from rowlist 
                                     if (nodeRow.Attributes["PageInfo"] != null)
                                     {
                                         m_pageInfo = nodeRow.Attributes["PageInfo"].Value;
                                         m_processBPGID = nodeRow.Attributes["BPGID"].Value;
                                         m_IsPopup = "1";
                                     }
                                 }
                                 else
                                 { 
                                    //Look for BPControl attribute exist in YaxisValue and YaxisValue2
                                     XmlNode nodeYcolumn=null;
                                     switch (series.Name.ToString())
                                     {
                                         case "Default":
                                             nodeYcolumn = nodeColumns.SelectSingleNode("//Columns/Col[@Label='YaxisValue']");
                                             break;
                                         case "Default2":
                                             nodeYcolumn = nodeColumns.SelectSingleNode("//Columns/Col[@Label='YaxisValue2']");
                                             break;
                                     }
                                     if (nodeYcolumn != null)
                                     {
                                         if (nodeYcolumn.Attributes["BPControl"] != null)
                                         {
                                             //Check column wise  Process if it is not equal to process1 then updated links
                                             if (nodeYcolumn.Attributes["BPControl"].Value.Trim() != m_processName)
                                             {
                                                 m_processName = nodeYcolumn.Attributes["BPControl"].Value;

                                                 XmlNode nodeProc = xDocout.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + m_processName + "']");
                                                 if (nodeProc != null)
                                                 {
                                                     m_processBPGID = nodeProc.Attributes["BPGID"].Value;
                                                     m_pageInfo = nodeProc.Attributes["PageInfo"].Value;

                                                     if (m_pageInfo.ToLower() == "common/chartview.aspx")
                                                     {
                                                         m_pageInfo = "Common/ChartViewer.aspx";
                                                     }

                                                     m_pageInfo = m_pageInfo.Replace(" ", "");
                                                     m_IsPopup = nodeProc.Attributes["IsPopup"].Value;
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }


                             BPINFO = BPINFO.Replace("\"", "~::~");
                            


                             //series.Points[pointIndex].MapAreaAttributes = "onmouseover=\"DisplayTooltip('" + toolTip + "');\" onmouseout=\"DisplayTooltip('');\"";
                             series.Points[pointIndex].Href = "javascript:OnChartProcessClick('" + BPINFO + "','" + m_processBPGID + "','" + m_pageInfo + "','" + m_processName + "','" + m_IsPopup + "');";
                         }
                }
            }

        }

        private bool IsURLValid(string url)
        {
            if(url.Length > 0)
            {
                if (url.IndexOf("/") != -1 && url.IndexOf(".aspx") != -1)//If not a valid URL
                {
                    return true;
                }
            }
            return false;
        }

        public void ExportTOPDF()
        {
            //Save the chart image for export pdf
            this.centrChart.Save(@chartPath+"\\"+this.ChartCurrentImage , Dundas.Charting.WebControl.ChartImageFormat.Jpeg);

            string script = "javascript:ShowChartPDF('../Common/PrintViewer.aspx','ChartPDFViewer');";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ButtonClick", script, true);
        }

        private string GetCOXML(XmlDocument xDocOut, XmlNode nodeColumns, string RwToBeModified, string treeNodeName)
        {
            //setting Treenode name 
       
            string m_finalResult=string.Empty;
       
            StringBuilder sbRowXML = new StringBuilder();

            sbRowXML.Append("<" + treeNodeName + "><RowList>" + RwToBeModified + "</RowList></" + treeNodeName + ">");

           
            XmlDocument xDocRowInfoXml = new XmlDocument();
            xDocRowInfoXml.LoadXml(RwToBeModified);
            
            string TrxID = xDocRowInfoXml.FirstChild.Attributes["TrxID"].Value;
            string TrxType = xDocRowInfoXml.FirstChild.Attributes["TrxType"].Value;

            //Retrieving the BPGID and Navigate page from GVDataXML
            string BPGID = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
            string pageInfo = xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;

            //if (pageInfo.Contains("VoidCheckHistory"))
            //{
                //Append the Branch node row XML also to the rowXML.
                XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        string branchName = nodeBranch.FirstChild.InnerText;
                        //Find the child row of the Parent Row in the RowList of the current branch.
                        XmlNode nodeBranchRow = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchName + "/RowList/Rows[@" + treeNodeName + "_TrxID='" + TrxID + "' and @" + treeNodeName + "_TrxType='" + TrxType + "']");
                        if (nodeBranchRow != null)
                        {
                            //If there is a associated branch child row.
                            sbRowXML.Append("<" + branchName + "><RowList>" + nodeBranchRow.OuterXml + "</RowList></" + branchName + ">");
                        }
                    }
                }
            //}

            //Assign the first FVL>0 column value to the caption.
            string COCaption = string.Empty;
            string firstFVLColName = string.Empty;
            if (nodeColumns.ChildNodes.Count > 0)
            {
                foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
                {
                    if (nodeCol.Attributes["FullViewLength"] != null)
                    {
                        if (Convert.ToInt32(nodeCol.Attributes["FullViewLength"].Value) > 0)
                        {
                            firstFVLColName = nodeCol.Attributes["Label"].Value;
                            break;
                        }
                    }
                }
            }
            if (xDocRowInfoXml.FirstChild.Attributes[firstFVLColName] != null)// && xDocRowInfoXml.SelectSingleNode(m_treeNodeName + "/RowList/Rows").Attributes[firstFVLColName] != "")
            {
                COCaption = xDocRowInfoXml.FirstChild.Attributes[firstFVLColName].Value;
            }

            string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + pageInfo + "</PageInfo><Caption>" + commonObjUI.CharactersToHtmlCodes(COCaption) + "</Caption></CallingObject>";

            m_finalResult = sbRowXML.ToString() + callingObjXML;

            return m_finalResult;

        }

        #endregion

        /// <summary>
        /// This event for water mark if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void centrChart_PostPaint(object sender, Dundas.Charting.WebControl.ChartPaintEventArgs e)
        {
            if (sender is ChartArea)
            {
                //load image file  to work this need to add OnPostPaint="centrChart_PostPaint"

                string logopath = System.AppDomain.CurrentDomain.BaseDirectory + "App_Themes\\" + Convert.ToString(Session["MyTheme"]) + "\\Images\\lajit-logo.jpg";
                System.Drawing.Image img = System.Drawing.Image.FromFile(logopath);

                //call the drawing method
                CreateWaterMark(img, 10, Color.White, 200, 150, e);
            }
        }


        /// <summary>
        /// Creates the water mark and applies alpha blending
        /// </summary>
        /// <param name="img">A refrence to the image file to be placed in the chart (System.Drawing.Image not the ASP.NET control)</param>
        /// <param name="alpha">A value between 0-255 that determines how faded the image will appear on the chart (255 is not faded at all)</param>
        /// <param name="color">The Color that will be made completely transparent, if you don't want to do this just pass System.Drawing.Color.Transparent</param>
        /// <param name="width">The width of the image to be drawn on the chart</param>
        /// <param name="height">The height of the image to be drawn on the chart</param>
        /// <param name="e">Dundas.Charting.WebControl.ChartPaintEventArgs e, this needs to be passed to the function so the drawing can be done  <see cref="T:Dundas.Charting.WebControl.ChartPaintEventArgs"/> instance containing the event data.</param>
        public void CreateWaterMark(System.Drawing.Image img, Single alpha, System.Drawing.Color color, int width, int height, Dundas.Charting.WebControl.ChartPaintEventArgs e)
        {
            //The object is used to apply changes to the image
            System.Drawing.Imaging.ImageAttributes attrib = new System.Drawing.Imaging.ImageAttributes();

            //Apply a transparency color
            if (color != null)
            {
                System.Drawing.Imaging.ColorMap[] map = new System.Drawing.Imaging.ColorMap[1];
                map[0] = new System.Drawing.Imaging.ColorMap();
                map[0].OldColor = color;
                map[0].NewColor = System.Drawing.Color.Transparent;
                attrib.SetRemapTable(map);
            }

            //Apply the alpha blend
            System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
            matrix[3, 3] = 256 - alpha;
            attrib.SetColorMatrix(matrix);

            //Get the coordinates for the chart area
            //Calculate the rectangle that is the chartarea
            double xmint =centrChart.ChartAreas[0].AxisX.Minimum;
            double xmaxt = centrChart.ChartAreas[0].AxisX.Maximum;
            double ymint = centrChart.ChartAreas[0].AxisY.Minimum;
            double ymaxt = centrChart.ChartAreas[0].AxisY.Maximum;

            float xmin = (float)e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, xmint);
            float xmax = (float)e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, xmaxt);
            float ymin = (float)e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, ymint);
            float ymax = (float)e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, ymaxt);

            RectangleF rect = new RectangleF(xmin, ymax, xmax - xmin, ymin - ymax);

            //Convert the rectangle to absolute coordinates
            rect = e.ChartGraphics.GetAbsoluteRectangle(rect);

            //This rectangle is the rectangle representing the chartarea
            Rectangle areaRectangle = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

            //Find the center point of the chartarea
            double midX = areaRectangle.X + (0.5 * areaRectangle.Width);
            double midY = areaRectangle.Y + (0.5 * areaRectangle.Height);

            //Calculate the actual rectangle used to draw the image
            Rectangle innerRectangle = new Rectangle((int)(midX - (0.5 * width)), (int)(midY - (0.5 * height)), width, height);

            //Draw the Image
            e.ChartGraphics.DrawImage(img, innerRectangle, 0, 0, img.Width, img.Height, System.Drawing.GraphicsUnit.Pixel, attrib);
        }

    }
}