using System;
using System.Web;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public delegate void BindEventHandler(object sender);
    //public delegate void AddEventHandler(HttpContext _context);
    //public delegate void EditEventHandler(HttpContext _context);
    //public delegate void DeleteEventHandler(HttpContext _context);
    //public delegate void PageEventHandler(HttpContext _context);


    [DefaultProperty("Text")]
    [ToolboxData("<{0}:JQGridView runat=server></{0}:JQGridView>")]
    public class JQGridView : WebControl//, IHttpHandler, IRequiresSessionState
    {
        public event BindEventHandler OnScriptGenerated;
        //public event AddEventHandler AddBegin;
        //public event EditEventHandler EditBegin;
        //public event DeleteEventHandler DeleteBegin;
        //public event PageEventHandler PageBind;

        private string m_ContainerTableName = "tblGrid";
        private string m_PagerDivName = "divPager";

        private XMLReader m_XmlReader;

        internal XMLReader XmlReader
        {
            get { return m_XmlReader; }
            set { m_XmlReader = value; }
        }

        /// <summary>
        /// Stores the options of the NavGrid(Footer Row).
        /// </summary>
        private GridFooter m_FooterStyle;

        public GridFooter FooterStyle
        {
            get { return m_FooterStyle; }
            set { m_FooterStyle = value; }
        }

        private ColModelCollection m_Columns;

        private string m_Caption;

        private int m_RowDisplayCount;

        private string m_EditURL;

        private SortDirection m_SortOrder;

        private bool m_ShowEditButton;

        private bool m_ShowDeleteButton;

        private bool m_ShowAddButton;

        private bool m_EmitJQueryCore;

        private bool m_ShowSearchButton;

        private bool m_IsInLineEditable;

        private bool m_DisplayRowNumbers;

        private bool m_PagerVisible = true;//Default is True.

        private bool m_AutoLoadOnScroll;

        private bool m_ShowSubGrid;

        private string m_Height = "'auto'";

        private Unit m_Width = Unit.Empty;

        public bool ShowSubGrid
        {
            get { return m_ShowSubGrid; }
            set { m_ShowSubGrid = value; }
        }

        public override Unit Height
        {
            set
            {
                m_Height = value.Value.ToString();
            }
        }

        public bool AutoLoadOnScroll
        {
            get { return m_AutoLoadOnScroll; }
            set
            {
                m_AutoLoadOnScroll = value;
                m_Height = value ? "400px" : "auto";
            }
        }

        public bool PagerVisible
        {
            get { return m_PagerVisible; }
            set { m_PagerVisible = value; }
        }

        public bool DisplayRowNumbers
        {
            get { return m_DisplayRowNumbers; }
            set { m_DisplayRowNumbers = value; }
        }

        public bool IsInLineEditable
        {
            get { return m_IsInLineEditable; }
            set { m_IsInLineEditable = value; }
        }

        public bool ShowSearchButton
        {
            get { return m_ShowSearchButton; }
            set { m_ShowSearchButton = value; }
        }

        /// <summary>
        /// Whether to include the Jquery core package into the client or not.
        /// Default is false.
        /// </summary>
        public bool EmitJQueryCore
        {
            get { return m_EmitJQueryCore; }
            set { m_EmitJQueryCore = value; }
        }

        public bool ShowAddButton
        {
            get { return m_ShowAddButton; }
            set { m_ShowAddButton = value; }
        }

        public bool ShowEditButton
        {
            get { return m_ShowEditButton; }
            set { m_ShowEditButton = value; }
        }

        public bool ShowDeleteButton
        {
            get { return m_ShowDeleteButton; }
            set { m_ShowDeleteButton = value; }
        }

        /// <summary>
        /// Show the Gridview initially sorted in this direction.
        /// </summary>
        public SortDirection SortOrder
        {
            get { return m_SortOrder; }
            set { m_SortOrder = value; }
        }

        /// <summary>
        /// The URL to hit when user enters the edit mode.
        /// </summary>
        public string EditURL
        {
            get { return m_EditURL; }
            set { m_EditURL = value; }
        }

        /// <summary>
        /// Gets or sets the number of rows to display in the grid in a given page.
        /// </summary>
        public int RowDisplayCount
        {
            get { return m_RowDisplayCount; }
            set { m_RowDisplayCount = value; }
        }

        /// <summary>
        /// Gets or sets the Caption for the GridView.
        /// </summary>
        public string Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }

        /// <summary>
        /// Collection of the Columns held by the Grid.
        /// </summary>
        public ColModelCollection Columns
        {
            get { return m_Columns; }
            set { m_Columns = value; }
        }

        private string m_HandlerUrl;

        public string HandlerUrl
        {
            get { return m_HandlerUrl; }
            set { m_HandlerUrl = value; }
        }

        //Class Constructor.
        public JQGridView()
        {
            m_Columns = new ColModelCollection();
            m_FooterStyle = new GridFooter();
            XmlReader = new XMLReader();

            //Defaults - If below fields are not initialised go with the defaults.
            m_Caption = "LAjit GridView";
            m_RowDisplayCount = 10;
            m_ShowEditButton = false;
            m_ShowDeleteButton = false;
            m_ShowAddButton = false;
            m_ShowSearchButton = false;
            m_EmitJQueryCore = false;
            m_ShowSubGrid = false;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string jsJQuery = "LAjitControls.JQGridView.jquery.js";
            string jsLocale = "LAjitControls.JQGridView.locale-en.js";
            string jsJQGrid = "LAjitControls.JQGridView.jqGrid.js";
            string jsMethods = "LAjitControls.JQGridView.JQMethods.js";
            string cssGrid = "LAjitControls.JQGridView.jqgrid.css";
            ClientScriptManager cs = this.Page.ClientScript;
            if (EmitJQueryCore)
            {
                cs.RegisterClientScriptResource(typeof(JQGridView), jsJQuery);
                string jsLocation = cs.GetWebResourceUrl(this.GetType(), jsJQuery);
                cs.RegisterClientScriptInclude(jsJQuery, jsLocation);
            }

            //First emit the resources to the client.
            //cs.RegisterClientScriptResource(typeof(JQGridView), jsLocale);
            //cs.RegisterClientScriptResource(typeof(JQGridView), jsJQGrid);
            // cs.RegisterClientScriptResource(typeof(JQGridView), jsMethods);
            //cs.RegisterClientScriptResource(typeof(JQGridView), cssGrid);

            //Append the include tags of the emitted scripts to the page's headers.
            string scriptLocation = cs.GetWebResourceUrl(this.GetType(), jsLocale);
            //cs.RegisterClientScriptInclude(jsLocale, scriptLocation);
            scriptLocation = cs.GetWebResourceUrl(this.GetType(), jsJQGrid);
            //cs.RegisterClientScriptInclude(jsJQGrid, scriptLocation);
            //scriptLocation = cs.GetWebResourceUrl(this.GetType(), jsMethods);
            //cs.RegisterClientScriptInclude(jsMethods, scriptLocation);



            ////Append the style sheet link to the page's header.
            //string includeTemplate = "<link rel='stylesheet' text='text/css' href='{0}' />";
            //string includeLocation = cs.GetWebResourceUrl(this.GetType(), cssGrid);
            //LiteralControl include = new LiteralControl(String.Format(includeTemplate, includeLocation));
            //((HtmlHead)Page.Header).Controls.Add(include);



        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            //m_ContainerTableName = this.ID;
            //m_PagerDivName = "divPager_" + this.ID;

            //Write the container's HTML
            output.Write("<table id=\"" + m_ContainerTableName + "\" class=\"scroll\" cellpadding=\"0\" cellspacing=\"0\"></table>");
            output.Write("<div id=\"" + m_PagerDivName + "\" class=\"scroll\" style=\"text-align: center;\"></div>");

            //if (ShowAddButton)
            //{
            //    output.Write("<input type='button' value='Add' accesskey='a' id='btnAdd' />");
            //}
            //if (ShowEditButton)
            //{
            //    output.Write("<input type='button' value='Edit' accesskey='e' id='btnEdit' />");
            //}
            //if (ShowDeleteButton)
            //{
            //    output.Write("<input type='button' value='Delete' accesskey='d' id='btnDelete' />");
            //}
            //if (ShowSearchButton)
            //{
            //    output.Write("<input type='button' value='Search' accesskey='s' id='btnSearch' />");
            //}
            //output.Write("<input type='button' value='Test' accesskey='t' id='btnTest' />");
        }

        public override void DataBind()
        {
            string sortOrder = (SortOrder.ToString() == "Ascending") ? "asc" : "desc";
            string colNames = this.Columns.ColumnNames;
            string cmString = this.Columns.ToString();
            string jsString = @"jQuery(document).ready(function(){
                    var jqGridObj=jQuery('#" + m_ContainerTableName + @"').jqGrid({
                    url:'" + HandlerUrl + @"?nd='+new Date().getTime(),
                    autowidth: false,
                    height: " + m_Height + @",
                    mtype: 'GET',
                    datatype: 'xml',
                    colNames:[" + colNames + @"],
                    colModel:[" + cmString + @" ],
                    xmlReader: {    root : 'RowList',
                                    row: 'Rows',
                                    page: 'RowList>PageNo',
                                    total: 'RowList>Pages',
                                    records : 'RowList>TotalRows',
                                    repeatitems: false,
                                    id: 'Id'
                                },
                     pager:'#" + m_PagerDivName + @"',
                     scrollrows : true,
                     subGrid:" + m_ShowSubGrid.ToString().ToLower() + @",
                     pgbuttons:" + m_PagerVisible.ToString().ToLower() + @",
                     pginput:" + m_PagerVisible.ToString().ToLower() + @",
                     rowNum:" + m_RowDisplayCount + @",
                     scroll: " + m_AutoLoadOnScroll.ToString().ToLower() + @", 
                     rownumbers:" + m_DisplayRowNumbers.ToString().ToLower() + @",
                     isInlineEditable: " + m_IsInLineEditable.ToString().ToLower() + @", 
                     editurl:'" + HandlerUrl + @"',
                     viewrecords:false,
                     sortorder:'" + sortOrder + @"', 
                     caption:'" + Caption + @"', 
                     loadComplete:GridLoadComplete,
                     onSelectRow:OnSelectRow,
                     footerrow:true,
                     userDataOnFooter:true,
                     afterInsertRow:OnRowDataBound,
                     onPaging:OnPageChange,
                     subGridBeforeExpand:OnSubGridBeforeExpand,
                     subGridRowExpanded:OnSubGridExpanded,
                     subGridRowColapsed:OnSubGridCollapsed
                    }).navGrid('#" + m_PagerDivName + @"',{add:false,edit:false,del:false,search:false,refresh:false,view:false,position:'left'});
                     
                    }); ";

            //onSelectRow:onselectRowGrid,  imgpath: '../App_Themes/LAjit/Images',
            //loadComplete: GridLoadComplete(),
            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "JqGrid", jsString, true);

            if (OnScriptGenerated != null)
            {
                OnScriptGenerated(this);
            }
        }

        #region IHttpHandler Members

        //public bool IsReusable
        //{
        //    get { return false; }
        //}

        //public void ProcessRequest(HttpContext context)
        //{
        //    if (context.Request.HttpMethod != null && context.Request.HttpMethod == "POST")
        //    {
        //        if (context.Request["oper"] != null)
        //        {
        //            switch (context.Request["oper"].ToString().ToUpper())
        //            {
        //                case "EDIT":
        //                    {
        //                        if (EditBegin != null)
        //                        {
        //                            EditBegin(context);
        //                        }
        //                        break;
        //                    }
        //                case "ADD":
        //                    {
        //                        if (AddBegin != null)
        //                        {
        //                            AddBegin(context);
        //                        }
        //                        break;
        //                    }
        //                case "DEL":
        //                    {
        //                        if (DeleteBegin != null)
        //                        {
        //                            DeleteBegin(context);
        //                        }
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (PageBind != null)
        //        {
        //            PageBind(context);
        //        }
        //    }
        //}

        #endregion
    }
}
