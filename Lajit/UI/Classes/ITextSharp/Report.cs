using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using System.Web;
using NLog;
using LAjitDev.Classes.ITextSharp;


namespace LAjitDev
{
    #region Enums
    public enum PaperSize
    {
        LetterUS,
        LegalUS,
        A4
    }
    #endregion

    public class Report
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        #region Private Field Variables
        private XmlDocument m_xDoc;
        private PaperSize m_PaperSize;
        private Document pdfDocument;

        private ITextXMLStore parentXStore;
        private ITextXMLStore childXStore;
        private ITextXMLStore subChildxStore;
        private XmlNodeList mxTrxNodeListColData;

        private PdfWriter m_pdfWriter;
        private Rectangle m_PageSize;
        private PdfPTable m_RequestTable;
        private PdfPTable m_parentPdfPTable;

        private Hashtable htSubTotal = new Hashtable();
        private Hashtable htParentTotal = new Hashtable();
        private Hashtable htChildTotal = new Hashtable();
        private Hashtable htTotal = new Hashtable();
        private Hashtable htGrandTotal = new Hashtable();

        private Hashtable htRevenue = new Hashtable();
        private Hashtable htProdCost = new Hashtable();
        private Hashtable htOtherCost = new Hashtable();
        private Hashtable htOtherIncome = new Hashtable();
        private Hashtable htTaxes = new Hashtable();
        private Hashtable htGrossProfit = new Hashtable();
        private Hashtable htNetIncBefTaxes = new Hashtable();
        private Hashtable htNetIncome = new Hashtable();
        private Dictionary<string, Decimal> m_dicNodeTotals;

        private string m_BPE;
        private string m_strXmlDoc;
        private string m_ErrorMessage;
        private string m_Message = "";
        private string m_DocTitle;
        private string m_HeaderFont;
        private string m_TreeNodeName;
        private string m_DocPassWord;
        private string m_FontName;
        private static string m_reportStyle;
        private string m_ActReportStyle;
        private string parentName;
        private string childName;
        private string m_strImagesCDNPath;

        private bool m_ApplyAlternatingColors;
        private bool m_Status = false;
        private bool m_ShowTitle;
        private bool m_ShowPageNumber;
        private bool m_ShowWatermark;
        private bool m_ShowLandscape;
        private bool m_FontBold;
        private bool m_FontItalic;
        private bool m_HeaderFontBold;
        private bool m_HeaderFontItalic;
        private bool m_ShowAnnotations;
        private bool m_IsCheckBox = false;

        private float m_WidthScaleFactor = 1.01f;
        private float m_LeftMargin;
        private float m_RightMargin;
        private float m_HeaderMargin;
        private float m_FooterMargin;
        private float m_HeaderFontSize;
        private float m_FontSize;
        private float[] m_ParentColWidthPct;
        private float[] m_subChildWidthPct;
        private float[] m_childColWidthPct;

        private int m_setFontName = 0;
        private int xmlChildCount = 0;
        private int xmlParentCount = 0;
        private int m_ChildColCount = 0;
        private int xmlSubChildCount = 0;
        private int subChdColHdrCount = 0;

        private decimal m_totalBalanceForward = 0;
        private decimal m_totalDateRange = 0;
        private decimal monthTotal = 0;
        private decimal monthDtRngTotal = 0;
        private decimal monthEndBalance = 0;
        private decimal variance = 0;
        private decimal total = 0;

        private string pageNum;
        private string strRevenue;
        private int curentPageNumber;
        private float pageWidth = 0;

        private static readonly Color LIGHT_BLUE = new Color(230, 230, 255);
        Color THISTLE = new Color(216, 191, 216);
        Color WHEAT = new Color(System.Drawing.Color.Linen);
        #endregion

        #region Properties
        public string BPE
        {
            get { return m_BPE; }
            set { m_BPE = value; }
        }

        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
            set { m_ErrorMessage = value; }
        }

        public string DocTitle
        {
            get { return m_DocTitle; }
            set { m_DocTitle = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }

        public string HeaderFont
        {
            get { return m_HeaderFont; }
            set { m_HeaderFont = value; }
        }

        public string DocPassWord
        {
            get { return m_DocPassWord; }
            set { m_DocPassWord = value; }
        }

        public PaperSize PaperSize
        {
            get { return m_PaperSize; }
            set { m_PaperSize = value; }
        }

        public bool ShowTitle
        {
            get { return m_ShowTitle; }
            set { m_ShowTitle = value; }
        }

        public bool ShowPageNumber
        {
            get { return m_ShowPageNumber; }
            set { m_ShowPageNumber = value; }
        }

        public bool ShowWatermark
        {
            get { return m_ShowWatermark; }
            set { m_ShowWatermark = value; }
        }

        public bool ShowLandscape
        {
            get { return m_ShowLandscape; }
            set { m_ShowLandscape = value; }
        }

        public bool ApplyAlternatingColors
        {
            get { return m_ApplyAlternatingColors; }
            set { m_ApplyAlternatingColors = value; }
        }

        public bool FontBold
        {
            get { return m_FontBold; }
            set { m_FontBold = value; }
        }

        public bool FontItalic
        {
            get { return m_FontItalic; }
            set { m_FontItalic = value; }
        }

        public bool ShowAnnotations
        {
            get { return m_ShowAnnotations; }
            set { m_ShowAnnotations = value; }
        }

        public bool HeaderFontBold
        {
            get { return m_HeaderFontBold; }
            set { m_HeaderFontBold = value; }
        }

        public bool HeaderFontItalic
        {
            get { return m_HeaderFontItalic; }
            set { m_HeaderFontItalic = value; }
        }

        public bool Success
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        public float FontSize
        {
            get { return m_FontSize; }
            set { m_FontSize = value; }
        }

        public float HeaderFontSize
        {
            get { return m_HeaderFontSize; }
            set { m_HeaderFontSize = value; }
        }
        #endregion

        #region Class Constructers
        public Report()
        {
            this.PaperSize = PaperSize.LetterUS;

            this.ShowTitle = true;
            this.ShowPageNumber = true;
            this.ShowWatermark = false;
            this.ShowLandscape = false;
            this.ShowAnnotations = true;
            this.HeaderFontBold = true;
            this.HeaderFontItalic = false;
            this.FontBold = false;
            this.FontItalic = false;
            this.m_ApplyAlternatingColors = true;

            this.FontName = "Arial";
            this.FontSize = 7.0f;
            this.m_DocPassWord = "";
            this.HeaderFont = "Times";
            this.HeaderFontSize = 7.5f;

            this.m_LeftMargin = 108;
            this.m_RightMargin = 108;
            this.m_HeaderMargin = 5;
            this.m_FooterMargin = 5;
            this.m_WidthScaleFactor = 1.01f;
        }
        #endregion

        #region Public Methods
        //To Convert an Amount string to Currency Format
        public static String ConvertToCurrencyFormat(string strAmount)
        {
            decimal amount;
            Decimal.TryParse(strAmount, out amount);
            strAmount = string.Format("{0:N}", amount);
            return strAmount;
        }

        //Method to Generate Report for Style 622
        public bool GenerateReport(XmlDocument xDoc, FileInfo file, DataTable dtParent, DataTable dtChild, Hashtable htColNameValues, Hashtable htBColNameValues)
        {
            #region NLog
            logger.Info("Generating the report for  style 622 with file name : " + file.FullName);
            #endregion

            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;
            string subChildTreeNode = string.Empty;
            string trxTreeNodeName = string.Empty;
            string trxSecondTreeNodeName = string.Empty;
            string trxThirdTreeNodeName = string.Empty;
            int treeNodesCount = 0;
            bool isSuccess = false;

            Hashtable htColNames = new Hashtable();
            m_strImagesCDNPath = ConfigurationManager.AppSettings["ImagesPath"].ToString() + "/" + HttpContext.Current.Session["MyTheme"] + "/";

            //set WidthTotal and PageSize
            float widthTotal = 145;
            this.m_PageSize = ConvertToPageSize(this.m_PaperSize);

            //Rotate Page if Landscape is Selected
            if (m_ShowLandscape)
            {
                this.m_PageSize = this.m_PageSize.Rotate();
            }

            m_xDoc = xDoc;
            m_strXmlDoc = xDoc.OuterXml;
            m_setFontName = GetFontName(m_FontName);

            //Initiate the pdfDocument and pdfWriter
            pdfDocument = new Document(this.m_PageSize, m_LeftMargin, m_RightMargin, m_HeaderMargin, m_FooterMargin);
            m_pdfWriter = PdfWriter.GetInstance(pdfDocument, new FileStream(file.FullName, FileMode.Create));

            //Set PassWord
            if (m_DocPassWord != null && m_DocPassWord.Trim() != "")
            {
                m_pdfWriter.SetEncryption(PdfWriter.STRENGTH128BITS, m_DocPassWord, m_DocPassWord, PdfWriter.ALLOW_PRINTING);
            }

            //AddMetaData
            this.AddMetaData(pdfDocument);

            //To get HeaderTreeNode
            XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string treeNodeName = string.Empty;
            if (nodeTreenode.SelectSingleNode("Node") != null)
            {
                treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
            }

            //Generate and Display Header
            DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);
            DisplayHeader(dtHeader);

            //Set the PageFooter
            if (m_ShowPageNumber)
            {
                this.DisplayFooter();
            }

            pdfDocument.Open();

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            childTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 2:
                        {
                            subChildTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 3:
                        {
                            trxTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 4:
                        {
                            trxSecondTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 5:
                        {
                            trxThirdTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                }

                treeNodesCount++;
            }

            //To LoadDocument622
            this.LoadDoc622(dtParent, dtChild, htColNameValues, htBColNameValues, parentTreeNode, childTreeNode, widthTotal);

            pdfDocument.Close();
            isSuccess = true;

            //To write the Document to the Context
            System.Net.WebClient client = new System.Net.WebClient();
            Byte[] buffer = client.DownloadData(file.FullName.ToString());
            if (buffer != null)
            {
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", file.Name));
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            return isSuccess;
        }

        // Method to Generate Report for Style400
        public bool GenerateReport400(XmlDocument xDoc, FileInfo file, DataSet dsAll)
        {
            #region NLog
            logger.Info("Generating the report for  style 400 with file name : " + file.FullName);
            #endregion

            m_strImagesCDNPath = ConfigurationManager.AppSettings["ImagesPath"].ToString() + "/" + HttpContext.Current.Session["MyTheme"] + "/";

            Hashtable htTreeNodeNames = new Hashtable();
            this.m_Status = false;

            //Set PageSize
            this.m_PageSize = ConvertToPageSize(this.m_PaperSize);

            //Rotate if LandScape is Selected
            if (this.m_ShowLandscape)
            {
                this.m_PageSize = this.m_PageSize.Rotate();
            }

            m_reportStyle = "400";
            m_xDoc = xDoc;
            m_strXmlDoc = xDoc.OuterXml;
            m_setFontName = GetFontName(m_FontName);

            //Initiate the Document and writer
            pdfDocument = new Document(this.m_PageSize, m_LeftMargin, m_RightMargin, m_HeaderMargin, m_FooterMargin);
            m_pdfWriter = PdfWriter.GetInstance(pdfDocument, new FileStream(file.FullName, FileMode.Create));

            //Set PassWord
            if (m_DocPassWord != null && m_DocPassWord.Trim() != "")
            {
                m_pdfWriter.SetEncryption(PdfWriter.STRENGTH128BITS, m_DocPassWord, m_DocPassWord, PdfWriter.ALLOW_PRINTING);
            }

            //Get the Header TreeNodeName
            XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string treeNodeName = string.Empty;

            if (nodeTreenode.SelectSingleNode("Node") != null)
            {
                treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
            }

            //Generate and Display Header
            DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);
            this.AddMetaData(pdfDocument);
            this.DisplayHeader(dtHeader);

            //Set the PageFooter
            if (m_ShowPageNumber)
            {
                this.DisplayFooter();
            }

            pdfDocument.Open();

            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + treeNodeName + "//GridHeading//Columns/Col");
            //string strAgency = DisplayJobDetails(pdfDocument, firstTreeNode, xNodelistFields, 0);
            string strAgency = DisplayJobDetails(pdfDocument, treeNodeName, 0, true);
            //this.LoadDocument400(pdfDocument, dsAll);

            pdfDocument.Close();

            //Write the Dcoument to the Context
            System.Net.WebClient client = new System.Net.WebClient();
            Byte[] buffer = client.DownloadData(file.FullName.ToString());
            if (buffer != null)
            {
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", file.Name));
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }

            this.m_Status = true;
            return this.m_Status;
        }

        //Common Method to Generate ReportStyles
        public bool GenerateReport(XmlDocument xDoc, FileInfo file)
        {
            #region NLog
            logger.Info("Common method to generate reports with file name : " + file.FullName);
            #endregion

            m_strImagesCDNPath = ConfigurationManager.AppSettings["ImagesPath"].ToString() + "/" + HttpContext.Current.Session["MyTheme"] + "/";

            //xDoc.Load("C:/Documents and Settings/rknandamuri/Desktop/1.xml");
            //xDoc.Load("C:/Documents and Settings/rknandamuri/Desktop/Report XML/621.xml");
            m_xDoc = xDoc;
            m_strXmlDoc = xDoc.OuterXml;
            m_setFontName = GetFontName(m_FontName);

            bool isSuccess = false;

            //try
            //{
            //To get the TreeNodeName of Header 
            XmlNode nodeTreenode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string treeNodeName = string.Empty;
            if (nodeTreenode.SelectSingleNode("Node") != null)
            {
                treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
            }

            //Generate Datatable for the Header
            DataTable dtHeader = GetReportHeader(xDoc, treeNodeName);

            //Call this Method to Generate Reports from the XMLStore
            if (CreateDocFromXmlStore(file.FullName, dtHeader))
            {
                isSuccess = true;
            }

            //Write the Document to the Context
            System.Net.WebClient client = new System.Net.WebClient();
            Byte[] buffer = client.DownloadData(file.FullName.ToString());
            if (buffer != null)
            {
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", file.Name));
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            return isSuccess;
        }

        //Method to Generate Report OrderedBy Specific Column
        public bool GenerateReport(XmlDocument xDoc, FileInfo file, DataTable dtParent, string strOrderByCol, Hashtable htColNameValues, Hashtable htColFormats)
        {
            m_strImagesCDNPath = ConfigurationManager.AppSettings["ImagesPath"].ToString() + "/" + HttpContext.Current.Session["MyTheme"] + "/";
            m_xDoc = xDoc;
            m_strXmlDoc = xDoc.OuterXml;
            m_setFontName = GetFontName(m_FontName);

            bool isSuccess = false;

            float widthTotal = 145;
            this.m_PageSize = ConvertToPageSize(this.m_PaperSize);

            //Rotate Page if Landscape is Selected
            if (m_ShowLandscape)
            {
                this.m_PageSize = this.m_PageSize.Rotate();
            }

            //Call this Method to Generate Reports from the XMLStore
            if (CreateOrderedByReport(file.FullName, dtParent, strOrderByCol, htColNameValues, htColFormats, widthTotal))
            {
                isSuccess = true;
            }

            //Write the Document to the Context
            System.Net.WebClient client = new System.Net.WebClient();
            Byte[] buffer = client.DownloadData(file.FullName.ToString());
            if (buffer != null)
            {
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-disposition", string.Format("inline;filename={0}.pdf", file.Name));
                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Private Methods
        // Intital Method Called to Genrate Report from XMLStore
        private bool CreateDocFromXmlStore(string sFilePDF, DataTable dtHeader)
        {
            #region NLog
            logger.Info("Method called to generate the XML store with file name as : " + sFilePDF);
            #endregion

            Hashtable htTreeNodeNames = new Hashtable();
            int treeNodesCount = 0;
            this.m_Status = false;
            this.m_PageSize = ConvertToPageSize(this.m_PaperSize);

            //To Get the TreeNode from reading the ReportStyle
            XmlNode nodeTreenode = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            if (nodeTreenode != null && nodeTreenode.Attributes["ReportStyle"] != null)
            {
                m_reportStyle = nodeTreenode.Attributes["ReportStyle"].Value;
            }
            else
            {
                m_reportStyle = "1";
            }

            if (m_reportStyle == "663" || m_reportStyle == "664" || m_reportStyle == "665")
            {
                m_ShowLandscape = true;
            }

            //If landscape, then rotate the page (90 degrees)
            if (this.m_ShowLandscape)
            {
                this.m_PageSize = this.m_PageSize.Rotate();
            }

            //Initiate the pdfDocument and pdfWriter
            pdfDocument = new Document(this.m_PageSize, m_LeftMargin, m_RightMargin, m_HeaderMargin, m_FooterMargin);
            m_pdfWriter = PdfWriter.GetInstance(pdfDocument, new FileStream(sFilePDF, FileMode.Create));

            //Set Password
            if (m_DocPassWord != null && m_DocPassWord.Trim() != "")
            {
                m_pdfWriter.SetEncryption(PdfWriter.STRENGTH128BITS, m_DocPassWord, m_DocPassWord, PdfWriter.ALLOW_PRINTING);
            }


            if (m_reportStyle == "671")//Add a custom PageEvent to handle the overflow error in 671.
            {
                ITextPageEvent rptPageEvent = new ITextPageEvent();
                m_pdfWriter.PageEvent = rptPageEvent;
            }

            //Add MetaData
            this.AddMetaData(pdfDocument);

            //Display PageHeader and PageFooter
            if (m_reportStyle != "300")
            {
                this.DisplayHeader(dtHeader);
                if (m_ShowPageNumber)
                {
                    this.DisplayFooter();
                }
            }

            //CodeBlock for Style400
            //To Display the firstJob in the Header of the FirstPage
            if (m_reportStyle == "400")
            {
                string firstTreeNode = string.Empty;
                XmlNode ndParent = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

                foreach (XmlNode treeNode in ndParent.ChildNodes)
                {
                    firstTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                    break;
                }

                XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + firstTreeNode + "//GridHeading//Columns/Col");
                string strAgency = DisplayJobDetails(pdfDocument, firstTreeNode, 0, true);
                //
            }

            pdfDocument.Open();

            #region Commented DrawLine
            //xMoveTo = pdfDocument.PageSize.Width / 34;
            //yMoveTo = pdfDocument.PageSize.Height - 90;

            //xLineto = pdfDocument.PageSize.Width - 18;
            //yLineto = pdfDocument.PageSize.Height - 90;
            //this.DrawLine(xMoveTo, yMoveTo, xLineto, yLineto);
            #endregion

            //Redirect ReportStyle to the Appropriate Methods
            if (m_reportStyle != "2" && m_reportStyle != "3" && m_reportStyle != "7" && m_reportStyle != "200"
                && m_reportStyle != "300" && m_reportStyle != "400" && m_reportStyle != "405"
                && m_reportStyle != "662" && m_reportStyle != "663" && m_reportStyle != "664"
                && m_reportStyle != "665" && m_reportStyle != "670" && m_reportStyle != "671")
            {
                //Generic Method for ReportStyles
                this.LoadDocument(pdfDocument);
            }
            else
            {
                switch (m_reportStyle)
                {
                    default://for 662(Cobination of 661 and 660)
                        {
                            m_ActReportStyle = "662";
                            //To Generate pages using 661
                            m_reportStyle = "661";
                            this.LoadDocument(pdfDocument);

                            //Add and New Page
                            pdfDocument.NewPage();

                            //To Generate pages using 660
                            m_reportStyle = "660";
                            htGrandTotal.Clear();
                            xmlChildCount = 0;
                            this.LoadDocument(pdfDocument);
                            break;
                        }
                    case "2":
                    case "3":
                        {
                            this.LoadHistoryDocument(pdfDocument);
                            break;
                        }
                    case "7":
                        {
                            this.LoadDoc7(pdfDocument);
                            break;
                        }
                    case "200":
                        {
                            this.LoadDocument200(pdfDocument);
                            break;
                        }
                    case "300":
                        {
                            this.LoadDocument300(pdfDocument);
                            break;
                        }
                    case "400":
                        {
                            this.LoadDocument400(pdfDocument);
                            break;
                        }
                    case "405":
                        {
                            this.LoadDocument405(pdfDocument);
                            break;
                        }
                    case "663":
                        {
                            m_reportStyle = "660";
                            this.LoadDocument(pdfDocument);
                            break;
                        }
                    case "664":
                        {
                            m_reportStyle = "661";
                            this.LoadDocument(pdfDocument);
                            break;
                        }
                    case "665":
                        {
                            m_ActReportStyle = "665";
                            //To Generate pages using 661
                            m_reportStyle = "661";
                            this.LoadDocument(pdfDocument);

                            //Add and New Page
                            pdfDocument.NewPage();

                            //To Generate pages using 660
                            m_reportStyle = "660";
                            htGrandTotal.Clear();
                            xmlChildCount = 0;
                            this.LoadDocument(pdfDocument);
                            break;

                        }
                    case "670":
                        {
                            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

                            foreach (XmlNode treeNode in nodeParents.ChildNodes)
                            {
                                htTreeNodeNames[treeNodesCount] = treeNode.SelectSingleNode("Node").InnerText.ToString();
                                treeNodesCount++;
                            }

                            this.DisplayAccountDetails(pdfDocument, htTreeNodeNames);
                            break;
                        }
                    case "671":
                        {
                            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                            foreach (XmlNode treeNode in nodeParents.ChildNodes)
                            {
                                htTreeNodeNames[treeNodesCount] = treeNode.SelectSingleNode("Node").InnerText.ToString();
                                treeNodesCount++;
                            }
                            this.DisplayAccountDetails671(pdfDocument, htTreeNodeNames);
                            break;
                        }
                }
            }

            this.m_Status = true;
            pdfDocument.Close();

            #region Commented Add Image to All pages
            //string img = "D:/Lajithv3.0Jquery/UI/App_Themes/LAjit/Images/lajit_small-greylogo_03.JPG";
            ////Image image = Image.GetInstance(new Uri(img));
            ////image.SetAbsolutePosition(35, 45);
            //int pageNum = 1;
            //Stream inputPdfStream = new FileStream(sFilePDF, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            ////Stream inputImageStream = new FileStream("D:/Lajithv3.0Jquery/UI/App_Themes/LAjit/Images/lajit_small-greylogo_03.JPG", FileMode.Open, FileAccess.Read, FileShare.Read);

            //PdfReader reader = new PdfReader(inputPdfStream);
            //PdfStamper stamper = new PdfStamper(reader, inputPdfStream);

            //while (pageNum <= reader.NumberOfPages)
            //{
            //    PdfContentByte pdfContentByte = stamper.GetOverContent(pageNum);

            //    Image image = Image.GetInstance(new Uri(img));
            //    image.SetAbsolutePosition(200, 200);
            //    pdfContentByte.AddImage(image);
            //    pageNum++;
            //}
            //if (pageNum > reader.NumberOfPages)
            //{
            //    stamper.Close();
            //}
            #endregion

            return this.m_Status;
        }

        private bool CreateOrderedByReport(string sFilePDF, DataTable dtParent, string strOrderByCol, Hashtable htColNameValues, Hashtable htColFormats, float widthTotal)
        {
            //To get the TreeNodeName of Header 
            XmlNode nodeTreenode = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");
            string treeNodeName = string.Empty;
            if (nodeTreenode.SelectSingleNode("Node") != null)
            {
                treeNodeName = nodeTreenode.SelectSingleNode("Node").InnerText;
            }

            //Generate Datatable for the Header
            DataTable dtHeader = GetReportHeader(m_xDoc, treeNodeName);
            Hashtable htTreeNodeNames = new Hashtable();
            this.m_Status = false;
            this.m_PageSize = ConvertToPageSize(this.m_PaperSize);

            //To Get the TreeNode from reading the ReportStyle
            if (nodeTreenode != null && nodeTreenode.Attributes["ReportStyle"] != null)
            {
                m_reportStyle = nodeTreenode.Attributes["ReportStyle"].Value;
            }
            else
            {
                m_reportStyle = "1";
            }

            //If landscape, then rotate the page (90 degrees)
            if (this.m_ShowLandscape)
            {
                this.m_PageSize = this.m_PageSize.Rotate();
            }

            //Initiate the pdfDocument and pdfWriter
            pdfDocument = new Document(this.m_PageSize, m_LeftMargin, m_RightMargin, m_HeaderMargin, m_FooterMargin);
            m_pdfWriter = PdfWriter.GetInstance(pdfDocument, new FileStream(sFilePDF, FileMode.Create));

            //Set Password
            if (m_DocPassWord != null && m_DocPassWord.Trim() != "")
            {
                m_pdfWriter.SetEncryption(PdfWriter.STRENGTH128BITS, m_DocPassWord, m_DocPassWord, PdfWriter.ALLOW_PRINTING);
            }

            //Add MetaData
            this.AddMetaData(pdfDocument);

            //Display PageHeader and PageFooter
            this.DisplayHeader(dtHeader);
            if (m_ShowPageNumber)
            {
                this.DisplayFooter();
            }

            pdfDocument.Open();

            //Redirect ReportStyle to the Appropriate Methods
            switch (m_reportStyle)
            {
                default:
                    {
                        this.LoadOrderedByColDoc(pdfDocument, dtParent, strOrderByCol, htColNameValues, htColFormats, widthTotal);
                        break;
                    }
            }

            this.m_Status = true;
            pdfDocument.Close();

            return this.m_Status;
        }

        //To Display the Page Header
        private void DisplayHeader(DataTable dtHeader)
        {
            try
            {
                DataRow drHeader;
                DataRow drCompDetails;
                DataRow drCompDate;

                string strHeader = string.Empty;
                string compDetails = string.Empty;
                string compDate = string.Empty;
                string strReportDate = string.Empty;

                //Complete Header Table Containing 3 Columns
                PdfPTable headerTable = new PdfPTable(3);

                //Table to be inserted in the FirstColumn of headerTable which contains "Requested By:" details
                PdfPTable requestTable = new PdfPTable(1);
                requestTable.DefaultCell.Border = 0;

                //Table to be inserted in the SecondColumn of headerTable which contains Company Specific details
                PdfPTable companyTable = new PdfPTable(1);
                companyTable.DefaultCell.Border = 0;
                companyTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

                //Table to be inserted in the ThirdColumn of headerTable which contains Company Logo
                PdfPTable imageTable = new PdfPTable(3);

                //Table which is used for changing headerTables FirstColumn for ReportStyle400
                PdfPTable emptyTable = new PdfPTable(1);
                emptyTable.DefaultCell.Border = 0;

                int[] widths = { 47, 52, 46 };
                int[] landscapeWidths = { 41, 45, 39 };

                //Modify Widths if Style is 400
                if (m_reportStyle == "400")
                {
                    widths[0] = 39;
                    widths[1] = 60;
                    widths[2] = 48;

                    landscapeWidths[0] = 32;
                    landscapeWidths[1] = 60;
                    landscapeWidths[2] = 33;
                }

                //Set the TotalWidth
                float widthTotal;
                if (m_ShowLandscape)
                {
                    widthTotal = 125;
                    headerTable.SetWidths(landscapeWidths);
                    headerTable.TotalWidth = widthTotal;
                }
                else
                {
                    widthTotal = 145;
                    headerTable.SetWidths(widths);
                    headerTable.TotalWidth = widthTotal;
                }

                headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                headerTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                requestTable.TotalWidth = widthTotal;
                requestTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                drHeader = dtHeader.Rows[0];
                strHeader = drHeader["Column1"].ToString();
                string[] strRequestedBy = strHeader.Split(':');
                int hdrCount = 0;

                //Add Requested By Details to the request Table
                if (strRequestedBy.Length > 1)
                {
                    Chunk chunk = new Chunk("Requested By: " + strRequestedBy[hdrCount + 1].Trim().ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);
                    requestTable.AddCell(phrase);
                }
                else
                {
                    Chunk chunk = new Chunk("Requested By: " + strRequestedBy[hdrCount].Trim().ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);
                    requestTable.AddCell(phrase);
                }

                //Add Company Details to CompanyTable
                Chunk compchunk = new Chunk(drHeader["Column2"].ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase compPhrase = new Phrase(compchunk);
                companyTable.AddCell(compPhrase);

                //Get the Image Instance
                clsReportsUI objReportsUI = new clsReportsUI();
                string img = objReportsUI.PDFImagePath();
                Image image = Image.GetInstance(img);
                image.ScaleAbsolute(40, 60);

                PdfPCell imageCell = new PdfPCell();
                imageCell.Border = 0;
                imageCell.AddElement(image);

                //Add image to the Image Table based on ReportStyle
                imageTable.DefaultCell.Border = 0;
                if (m_reportStyle != "400")
                {
                    imageTable.AddCell("");
                    imageTable.AddCell("");
                }
                else
                {
                    imageTable.DefaultCell.Colspan = 2;
                    imageTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    imageTable.AddCell(requestTable);
                    imageTable.DefaultCell.Colspan = 1;
                }

                imageTable.AddCell(imageCell);
                //imageTable.AddCell("");

                //Add Date
                strReportDate = drHeader["Column3"].ToString();
                if (strReportDate != null && strReportDate != "")
                {
                    string[] dateArray = strReportDate.Split(' ');
                    string dateFormat = (Convert.ToDateTime(dateArray[2].ToString())).ToString("MM/dd/yyyy");
                    strReportDate = dateFormat + " " + dateArray[3].ToString();
                }

                Chunk datechunk = new Chunk("Run: " + strReportDate, new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase datePhrase = new Phrase(datechunk);
                requestTable.AddCell(datePhrase);

                //Add more Company Details if Exist
                if (dtHeader.Rows.Count > 1)
                {
                    drCompDetails = dtHeader.Rows[1];
                    compDetails = drCompDetails["Column2"].ToString();

                    Chunk compDetailsChunk = new Chunk(compDetails, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase compDetailsPhrase = new Phrase(compDetailsChunk);
                    companyTable.AddCell(compDetailsPhrase);
                }

                if (dtHeader.Rows.Count > 2)
                {
                    drCompDate = dtHeader.Rows[2];
                    compDate = drCompDate["Column2"].ToString();

                    Chunk compDateDetails = new Chunk(compDate, new Font(m_setFontName, m_FontSize));
                    Phrase compDateDetailsPhrase = new Phrase(compDateDetails);
                    companyTable.AddCell(compDateDetailsPhrase);
                }

                //Add EmptyTable to the first column of headerTable if reportStyle is 400
                if (m_reportStyle != "400")
                {
                    headerTable.AddCell(requestTable);
                }
                else
                {
                    headerTable.AddCell(emptyTable);
                }

                //Add tables to the HeaderTable
                headerTable.AddCell(companyTable);
                headerTable.AddCell(imageTable);

                //Add the Header Table to a Paragraph
                Paragraph headerParagraph = new Paragraph();
                headerParagraph.Add(headerTable);

                //Create a HeaderFooter object using the Paragraph and set the object as Header 
                HeaderFooter header = new HeaderFooter(headerParagraph, false);
                header.Border = Rectangle.NO_BORDER;
                header.Alignment = Element.ALIGN_LEFT;
                pdfDocument.Header = header;

                m_RequestTable = emptyTable;

                if (m_reportStyle == "400")
                {
                    //m_RequestTable.AddCell(new PdfPTable(1));
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //To Display the Page Footer(Page Number)
        private void DisplayFooter()
        {
            //Get the CurrentPageNumber
            string pageNumber = pdfDocument.PageNumber.ToString();

            //Change format from '01' to '1'
            if (pageNumber.StartsWith("0"))
            {
                string[] numSplit = pageNumber.Split('0');
                pageNumber = numSplit[1];
            }

            this.pageNum = pageNumber;

            //Create a HeaderFooter object with the formatted PageNumber nad set the object to the PageFooter
            HeaderFooter footer = new HeaderFooter(new Phrase("Page " + pageNumber, new Font(this.GetFontName(m_FontName), 7.5f, Font.BOLD)), true);
            footer.Border = Rectangle.NO_BORDER;
            footer.Alignment = Element.ALIGN_CENTER;
            pdfDocument.Footer = footer;
        }

        //To Add Document Specific MetaData
        private bool AddMetaData(Document pdfDocument)
        {
            bool bRet = true;

            if (pdfDocument != null)
            {
                // step 3: we add some metadata and open the pdfDocument
                pdfDocument.AddTitle(this.m_DocTitle);
                pdfDocument.AddCreator("Report.PDF");
                //pdfDocument.AddHeader("Expires", "0");
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }

        //Method to Load ReportStyle(OrderedByColumn)
        private void LoadOrderedByColDoc(Document pdfDocument, DataTable dtParent, string strOrderByCol, Hashtable htColNameValues, Hashtable htColFormats, float widthTotal)
        {
            try
            {
                Hashtable htSubTotal = new Hashtable();
                string parentTreeNode = string.Empty;
                XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                parentTreeNode = nodeParents.ChildNodes[0].SelectSingleNode("Node").InnerText.ToString();

                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                if (strOrderByCol.Length > 0)
                {
                    DataView dView = dtParent.DefaultView;
                    dView.Sort = htColNameValues[strOrderByCol] + " ASC";
                    dtParent = dView.ToTable();
                }
                //To get the nodelist containing columns of the parentTreeNode
                XmlNodeList xNodelistFields = m_xDoc.SelectNodes("Root/bpeout/FormControls/" + parentTreeNode + "/GridHeading/Columns/Col");

                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
                xmlStore.ReportStyle = m_reportStyle;

                //To Load the Records and to get the column and row Counts
                int numRecordsInXml = xmlStore.LoadRecords();
                int numColumnsInXml = xmlStore.Fields.Length;

                float[] clmWidths = new float[numColumnsInXml - 1];
                this.pageWidth = widthTotal;

                //For setting the Column widths
                if (strOrderByCol.Length > 0)
                {
                    SetOrderByColWidths(xNodelistFields, xmlStore, numColumnsInXml, clmWidths, widthTotal, strOrderByCol, htColNameValues);
                }

                //Table to Load the OrderedBy Column Name
                PdfPTable parentColTable = new PdfPTable(dtParent.Columns.Count - 2);
                parentColTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                ////Set the Width Percentage based on LandScape value
                if (m_ShowLandscape)
                {
                    parentColTable.WidthPercentage = 120;
                }

                parentColTable.TotalWidth = widthTotal;
                parentColTable.SetWidths(clmWidths);
                parentColTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                parentColTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                parentColTable.DefaultCell.Colspan = numColumnsInXml - 1;
                parentColTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                parentColTable.DefaultCell.Padding = 0.95f;

                Chunk textChunk = new Chunk(htColNameValues[strOrderByCol].ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase textPhrase = new Phrase(textChunk);
                parentColTable.AddCell(textPhrase);

                //Table to Load the all the Parent Column Names
                PdfPTable colPdfPTable = new PdfPTable(dtParent.Columns.Count - 2);
                colPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                //Set the Width Percentage based on LandScape value
                if (m_ShowLandscape)
                {
                    colPdfPTable.WidthPercentage = 120;
                }

                colPdfPTable.TotalWidth = widthTotal;
                colPdfPTable.SetWidths(clmWidths);
                colPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                colPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                colPdfPTable.DefaultCell.BackgroundColor = WHEAT;
                colPdfPTable.DefaultCell.Padding = 0.95f;

                for (int colCount = 0; colCount < dtParent.Columns.Count; colCount++)
                {
                    if (dtParent.Columns[colCount].ColumnName != "TrxID" && dtParent.Columns[colCount].ColumnName != htColNameValues[strOrderByCol].ToString())
                    {
                        string colName = dtParent.Columns[colCount].ColumnName;
                        string colFormat = xmlStore.Fields[colCount - 1].controlType;

                        if (colFormat == "Amount" || colFormat == "Calc")
                            colPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        else
                            colPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        Chunk cellChunk = new Chunk(colName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase cellPhrase = new Phrase(cellChunk);
                        colPdfPTable.AddCell(cellPhrase);
                    }
                }

                //Table to Insert Space
                PdfPTable spaceTable = new PdfPTable(numColumnsInXml - 1);
                spaceTable.WidthPercentage = parentColTable.WidthPercentage;
                spaceTable.TotalWidth = parentColTable.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, 1));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                string currColData = null;
                for (int parentCount = 0; parentCount < dtParent.Rows.Count; parentCount++)
                {
                    string columnName = htColNameValues[strOrderByCol].ToString();
                    string colHeaderData = dtParent.Rows[parentCount][columnName].ToString();

                    PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInXml - 1);
                    parentRowPdfPTable.WidthPercentage = parentColTable.WidthPercentage;

                    //Change width percentage based on landscape value
                    if (m_ShowLandscape)
                    {
                        parentRowPdfPTable.WidthPercentage = 120;
                    }
                    parentRowPdfPTable.TotalWidth = widthTotal;
                    parentRowPdfPTable.SetWidths(clmWidths);

                    //Set Cell Attributes
                    parentRowPdfPTable.DefaultCell.BorderWidth = 0;
                    parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    parentRowPdfPTable.DefaultCell.Colspan = numColumnsInXml;
                    parentRowPdfPTable.DefaultCell.Padding = 0.95f;

                    string orderedColName = htColNameValues[strOrderByCol].ToString();
                    string colData = dtParent.Rows[parentCount][orderedColName].ToString();
                    Chunk cellTxtChunk = new Chunk(colData, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase cellTxtPhrase = new Phrase(cellTxtChunk);
                    parentRowPdfPTable.AddCell(cellTxtPhrase);

                    if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                    {
                        if (currColData == colHeaderData)
                        {
                            pdfDocument.Add(parentRowPdfPTable);
                            pdfDocument.Add(spaceTable);
                            pdfDocument.Add(colPdfPTable);
                            curentPageNumber = m_pdfWriter.CurrentPageNumber;
                        }
                    }

                    if (currColData == null || currColData != colHeaderData)
                    {
                        if (parentCount != 0)
                        {
                            htSubTotal.Clear();
                            pdfDocument.Add(spaceTable);

                            //To Display SubTotal
                            PdfPTable subTotalPdfPTable = new PdfPTable(numColumnsInXml - 1);
                            subTotalPdfPTable.WidthPercentage = parentColTable.WidthPercentage;
                            subTotalPdfPTable.DefaultCell.Padding = 1.5f;
                            subTotalPdfPTable.DefaultCell.BackgroundColor = WHEAT;

                            //Change width percentage based on landscape value
                            if (m_ShowLandscape)
                            {
                                subTotalPdfPTable.WidthPercentage = 120;
                            }
                            subTotalPdfPTable.TotalWidth = widthTotal;
                            subTotalPdfPTable.SetWidths(clmWidths);

                            //Set Cell Attributes
                            subTotalPdfPTable.DefaultCell.BorderWidth = 0;
                            subTotalPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                            //Method to Generate Total Table
                            GenerateTotalTable(dtParent, htSubTotal, xmlStore, subTotalPdfPTable);

                            pdfDocument.Add(subTotalPdfPTable);
                            pdfDocument.Add(spaceTable);
                            pdfDocument.Add(spaceTable);
                        }

                        pdfDocument.Add(parentColTable);
                        pdfDocument.Add(parentRowPdfPTable);
                        pdfDocument.Add(spaceTable);
                        pdfDocument.Add(colPdfPTable);
                        currColData = colHeaderData;
                    }

                    PdfPTable orderedRowPdfPTable = new PdfPTable(numColumnsInXml - 1);
                    orderedRowPdfPTable.WidthPercentage = parentColTable.WidthPercentage;
                    orderedRowPdfPTable.DefaultCell.Padding = 0.95f;

                    //Change width percentage based on landscape value
                    if (m_ShowLandscape)
                    {
                        orderedRowPdfPTable.WidthPercentage = 120;
                    }
                    orderedRowPdfPTable.TotalWidth = widthTotal;
                    orderedRowPdfPTable.SetWidths(clmWidths);

                    //Set Cell Attributes
                    orderedRowPdfPTable.DefaultCell.BorderWidth = 0;
                    orderedRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    for (int colCount = 0; colCount < dtParent.Columns.Count; colCount++)
                    {
                        if (dtParent.Columns[colCount].ColumnName != "TrxID" && dtParent.Columns[colCount].ColumnName != htColNameValues[strOrderByCol].ToString())
                        {
                            string colName = dtParent.Columns[colCount].ColumnName;
                            string colFormat = xmlStore.Fields[colCount - 1].controlType;
                            string cellText = dtParent.Rows[parentCount][colCount].ToString();
                            decimal colValue = 0;

                            if (colFormat == "TBox")
                            {
                                int fullViewLength = Convert.ToInt32(xmlStore.Fields[colCount - 1].fullViewLength);
                                int charLength = Convert.ToInt32(Math.Round(fullViewLength / 2.2));
                                if (cellText.Length > charLength)
                                {
                                    cellText = cellText.Substring(0, charLength) + "..";
                                }
                            }

                            if (colFormat == "Amount" || colFormat == "Calc")
                            {
                                if (cellText.Trim() == string.Empty)
                                    cellText = "0";

                                if (cellText != null && cellText.Trim() != "")
                                    colValue = Convert.ToDecimal(cellText);

                                if (colValue < 0)
                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colValue * (-1))) + ")";
                                else
                                    cellText = ConvertToCurrencyFormat(cellText);

                                if (htParentTotal[colName] != null)
                                    htParentTotal[colName] = Convert.ToDecimal(htParentTotal[colName].ToString()) + Convert.ToDecimal(cellText);
                                else
                                    htParentTotal[colName] = cellText;

                                htSubTotal[colName] = cellText;
                            }

                            Chunk cellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase cellPhrase = new Phrase(cellChunk);
                            orderedRowPdfPTable.AddCell(cellPhrase);
                        }
                    }

                    pdfDocument.Add(orderedRowPdfPTable);
                    pdfDocument.Add(new Paragraph(""));
                    pdfDocument.Add(new Paragraph(""));

                    if (parentCount == dtParent.Rows.Count - 1)
                    {
                        //To Display SubTotal
                        PdfPTable subTotalPdfPTable = new PdfPTable(numColumnsInXml - 1);
                        subTotalPdfPTable.WidthPercentage = parentColTable.WidthPercentage;
                        subTotalPdfPTable.DefaultCell.Padding = 0.95f;
                        subTotalPdfPTable.DefaultCell.BackgroundColor = WHEAT;

                        //Change width percentage based on landscape value
                        if (m_ShowLandscape)
                        {
                            subTotalPdfPTable.WidthPercentage = 120;
                        }
                        subTotalPdfPTable.TotalWidth = widthTotal;
                        subTotalPdfPTable.SetWidths(clmWidths);

                        //Set Cell Attributes
                        subTotalPdfPTable.DefaultCell.BorderWidth = 0;
                        subTotalPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        //Method to Generate Total Table
                        GenerateTotalTable(dtParent, htSubTotal, xmlStore, subTotalPdfPTable);

                        pdfDocument.Add(spaceTable);
                        pdfDocument.Add(subTotalPdfPTable);
                        pdfDocument.Add(spaceTable);
                    }
                }

                pdfDocument.Add(spaceTable);

                //Compute Grand Total
                PdfPTable grandTotalPdfPTable = new PdfPTable(numColumnsInXml);
                grandTotalPdfPTable.WidthPercentage = parentColTable.WidthPercentage;
                grandTotalPdfPTable.DefaultCell.Padding = 0.95f;

                //Change width percentage based on landscape value
                if (m_ShowLandscape)
                {
                    grandTotalPdfPTable.WidthPercentage = 120;
                }
                grandTotalPdfPTable.TotalWidth = widthTotal;
                grandTotalPdfPTable.SetWidths(clmWidths);

                //Set Cell Attributes
                grandTotalPdfPTable.DefaultCell.BorderWidth = 0;
                grandTotalPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                grandTotalPdfPTable.DefaultCell.BackgroundColor = WHEAT;

                for (int colCount = 0; colCount < dtParent.Columns.Count; colCount++)
                {
                    if (dtParent.Columns[colCount].ColumnName != "TrxID" && dtParent.Columns[colCount].ColumnName != htColNameValues[strOrderByCol].ToString())
                    {
                        string colName = dtParent.Columns[colCount].ColumnName;
                        string colFormat = xmlStore.Fields[colCount - 1].controlType;

                        Chunk cellChunk = null;
                        Phrase cellPhrase = null;

                        if (colCount == 1)
                        {
                            cellChunk = new Chunk("Grand Total", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            cellPhrase = new Phrase(cellChunk);
                        }
                        else
                        {
                            if (colFormat == "Amount" || colFormat == "Calc")
                            {
                                string cellText = htParentTotal[colName].ToString();
                                decimal colValue = 0;

                                if (cellText.Trim() == string.Empty)
                                    cellText = "0";

                                if (cellText != null && cellText.Trim() != "")
                                    colValue = Convert.ToDecimal(cellText);

                                if (colValue < 0)
                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colValue * (-1))) + ")";
                                else
                                    cellText = ConvertToCurrencyFormat(cellText);

                                cellChunk = new Chunk(ConvertToCurrencyFormat(cellText), new Font(m_setFontName, m_FontSize, Font.BOLD));
                                cellPhrase = new Phrase(cellChunk);
                                grandTotalPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                cellChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                                cellPhrase = new Phrase(cellChunk);
                            }
                        }
                        grandTotalPdfPTable.AddCell(cellPhrase);
                    }
                }

                pdfDocument.Add(grandTotalPdfPTable);
                pdfDocument.Add(spaceTable);
                pdfDocument.Add(spaceTable);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }

        //Method to Generate Total for OrderByColReport
        private void GenerateTotalTable(DataTable dtParent, Hashtable htSubTotal, ITextXMLStore xmlStore, PdfPTable subTotalPdfPTable)
        {
            for (int colCount = 0; colCount < dtParent.Columns.Count; colCount++)
            {
                if (dtParent.Columns[colCount].ColumnName != "TrxID")
                {
                    string colName = dtParent.Columns[colCount].ColumnName;
                    string colFormat = xmlStore.Fields[colCount - 1].controlType;

                    Chunk cellChunk = null;
                    Phrase cellPhrase = null;

                    if (colCount == 1)
                    {
                        cellChunk = new Chunk("Total", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        cellPhrase = new Phrase(cellChunk);
                    }
                    else
                    {
                        if (colFormat == "Amount" || colFormat == "Calc")
                        {
                            string cellText = htSubTotal[colName].ToString();
                            decimal colValue = 0;

                            if (cellText != null && cellText.Trim() != "")
                            {
                                colValue = Convert.ToDecimal(cellText);
                            }

                            if (colValue < 0)
                            {
                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colValue * (-1))) + ")";
                            }
                            else
                            {
                                cellText = ConvertToCurrencyFormat(cellText);
                            }

                            cellChunk = new Chunk(ConvertToCurrencyFormat(cellText), new Font(m_setFontName, m_FontSize, Font.BOLD));
                            cellPhrase = new Phrase(cellChunk);
                            subTotalPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            cellChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                            cellPhrase = new Phrase(cellChunk);
                        }
                    }

                    subTotalPdfPTable.AddCell(cellPhrase);
                }
            }
        }

        //Method to Load Details into the Pdf Document
        private bool LoadDocument(Document pdfDocument)
        {
            #region NLog
            logger.Info("Method to Load Details into the Pdf Document : " + pdfDocument);
            #endregion

            bool bRet = false;
            decimal balanceForward = 0;
            float childTableWidth = 0f;

            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            int treeNodesCount = 0;
            int amtFieldCount = 0;

            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;
            string subChildTreeNode = string.Empty;
            string trxTreeNodeName = string.Empty;
            string trxSecondTreeNodeName = string.Empty;
            string trxThirdTreeNodeName = string.Empty;
            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            childTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 2:
                        {
                            subChildTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 3:
                        {
                            trxTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 4:
                        {
                            trxSecondTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 5:
                        {
                            trxThirdTreeNodeName = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                }

                treeNodesCount++;
            }

            //If Report Style equals "5" || "501" || "502" || "503" set the ChildNodeName to BranchNodeName
            if (m_reportStyle == "5" || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
            {
                branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
                childTreeNode = branchNodeName;
            }

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//GridHeading//Columns/Col");

            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
                xmlStore.ReportStyle = m_reportStyle;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlStore.LoadRecords();
                if (xmlStore.Fields != null)
                {
                    numColumnsInXml = xmlStore.Fields.Length;
                }
                else
                {
                    numColumnsInXml = 0;
                }
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    //Font for Report Styles '66' series
                    if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                    {
                        m_FontSize = 7.0f;
                        if (m_ShowLandscape)
                            m_FontSize = 7.5f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    //Table to Load ParentColumnNames
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                    //Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //--- see if we have width data for the Fields in XmlStore
                    //int widthTotal = xmlStore.GetColumnWidthsTotal();

                    //Set TotalWidth Based on Report Styles
                    float widthTotal;
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;

                        if (m_reportStyle == "10")
                        {
                            widthTotal = 90;
                        }
                    }
                    else
                    {
                        widthTotal = 145;
                        if (m_reportStyle == "10")
                        {
                            widthTotal = 110;
                        }
                    }

                    this.pageWidth = widthTotal;

                    //Set ParentColWidths
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //Set the widthPercentage of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);

                    //Set Cell Attributes
                    parentPdfPTable.DefaultCell.BorderWidth = 1;
                    parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Table for creating Space between rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //Table to be used for Report Styles 603,604
                    PdfPTable columnTypeTable;
                    if (m_reportStyle == "604")
                    {
                        columnTypeTable = new PdfPTable(8);
                    }
                    else
                    {
                        columnTypeTable = new PdfPTable(numColumnsInXml);
                        columnTypeTable.SetWidths(columnWidthInPct);
                    }

                    columnTypeTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    columnTypeTable.TotalWidth = parentPdfPTable.TotalWidth;
                    columnTypeTable.DefaultCell.Border = 0;
                    columnTypeTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                    columnTypeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //To Display Parent Col Names
                    if (m_reportStyle != "641" && m_reportStyle != "642" && m_reportStyle != "643" && m_reportStyle != "651" && m_reportStyle != "652" && m_reportStyle != "653" && m_reportStyle != "660" && m_reportStyle != "661")
                    {
                        CustomizeTBalReports(pdfDocument, spaceTable, columnTypeTable);
                        DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);
                    }

                    //Add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        if (m_reportStyle == "5" || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
                        {
                            if (row > 0)
                            {
                                if (m_reportStyle == "5")
                                {
                                    pdfDocument.Add(spaceTable);
                                    pdfDocument.Add(spaceTable);
                                    pdfDocument.Add(spaceTable);
                                }

                                //when row changes add the columnHeader Table
                                pdfDocument.Add(parentPdfPTable);
                            }
                        }

                        //If PageNumber changes display the columnHeader
                        if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                        {
                            if (childTreeNode == null || childTreeNode == "")
                            {
                                pdfDocument.Add(columnTypeTable);
                                pdfDocument.Add(spaceTable);
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                pdfDocument.Add(parentPdfPTable);
                            }
                        }

                        balanceForward = 0;
                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = parentTreeNode;

                        //Table to load Rows of the Parent Table
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        parentRowPdfPTable.DefaultCell.Padding = 0.95f;
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        //Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.BorderWidth = 0;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        //Get the RowListFields of the treeNode from the XML
                        XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList/Rows");

                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            StringBuilder sbTip = new StringBuilder();
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            Image checkedImage = null;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            if (col == 0)
                            {
                                strRevenue = rowData[col];
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(elem.Attributes["FullViewLength"].Value);

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    controlType = elem.Attributes["ControlType"].Value;

                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        //Customize if Report Style is 10
                                        if (m_reportStyle == "10")
                                        {
                                            if (elem.Attributes["Label"].Value == "EndBal")
                                            {
                                                if (xRowlistFields[row].Attributes["Description"] != null)
                                                {
                                                    strDescription = xRowlistFields[row].Attributes["Description"].Value;

                                                    if (strDescription == "Gross Profit" || strDescription == "EBITDA"
                                                        || strDescription == "Net Profit Before Tax" || strDescription == "Net Profit After Tax"
                                                        || strDescription == "Total Current Assets" || strDescription == "Total Assets"
                                                        || strDescription == "Total Current Liabilities" || strDescription == "Total Equity"
                                                        || strDescription == "Total Liabilities & Equity")
                                                    {
                                                        if (cellText.Trim() == "")
                                                        {
                                                            cellText = xRowlistFields[row].Attributes["Total"].Value;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (cellText != null && cellText.Trim() != "")
                                        {
                                            if (elem.Attributes["Label"].Value == "OpenBal")
                                            {
                                                balanceForward = Convert.ToDecimal(cellText);
                                            }

                                            m_totalBalanceForward = m_totalBalanceForward + balanceForward;
                                            parentTblAmount = Convert.ToDecimal(cellText);

                                            if (parentTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }

                                            //Load the Amount to HashTable for calculating Totals
                                            if (htParentTotal[xmlStore.Fields[col].label] == null)
                                            {
                                                htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                            }
                                            else
                                            {
                                                htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                            }
                                        }

                                        //Align Amount Data to the Right
                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        amtFieldCount++;
                                    }
                                    else
                                    {
                                        //Customize if controlType is Calender
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Customize if controlType is Check
                                            if (elem.Attributes["ControlType"].Value == "Check")
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null && cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                            }
                                        }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        XmlAttribute attTip = xRowlistFields[row].Attributes["Notes"];
                                        if (attTip != null && attTip.Value != "")
                                        {
                                            sbTip.Append("Notes: " + attTip.Value + "\n\n");
                                        }

                                        attTip = xRowlistFields[row].Attributes["Attachments"];
                                        if (attTip != null && attTip.Value != "")
                                        {
                                            sbTip.Append("Attachments: " + attTip.Value + "\n\n");
                                        }

                                        attTip = xRowlistFields[row].Attributes["SecuredBy"];
                                        if (attTip != null && attTip.Value != "")
                                        {
                                            sbTip.Append("SecuredBy: " + attTip.Value + "\n\n");
                                        }

                                        attTip = xRowlistFields[row].Attributes["ChangedBy"];
                                        if (attTip != null && attTip.Value != "")
                                        {
                                            sbTip.Append("ChangedBy: " + attTip.Value + "\n\n");
                                        }

                                        attTip = xRowlistFields[row].Attributes["ToolTip"];
                                        if (attTip != null && attTip.Value != "")
                                        {
                                            sbTip.Append("ToolTip: " + attTip.Value + "\n\n");
                                        }
                                    }
                                    break;
                                }
                            }

                            Chunk parentCellChunk;

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            StringBuilder strCellTextBuilder = new StringBuilder();
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            //Add Image for CheckBox if bool value is true 
                            if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643" || m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653" || m_reportStyle == "660" || m_reportStyle == "661")
                            {
                                if (m_IsCheckBox)
                                {
                                    parentCellChunk = new Chunk(checkedImage, 5, 5, false);
                                }
                                else
                                {
                                    parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                }
                            }
                            else
                            {
                                if (m_IsCheckBox)
                                {
                                    parentCellChunk = new Chunk(checkedImage, 5, 5, false);
                                }
                                else
                                {
                                    parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                                }
                            }

                            m_IsCheckBox = false;
                            parentRowPdfPTable.DefaultCell.Border = 0;

                            //Customize Data if ReportStyle is 10
                            if (m_reportStyle == "10")
                            {
                                if (col == 0)
                                {
                                    parentCellChunk.Font = new Font(m_setFontName, m_FontSize, Font.BOLD);
                                }
                                else
                                {
                                    if (col == 2)
                                    {
                                        if (strDescription == "Gross Profit" || strDescription == "EBITDA"
                                            || strDescription == "Net Profit Before Tax" || strDescription == "Net Profit After Tax"
                                            || strDescription == "Total Current Assets" || strDescription == "Total Assets"
                                            || strDescription == "Total Current Liabilities" || strDescription == "Total Equity"
                                            || strDescription == "Total Liabilities & Equity")
                                        {
                                            parentCellChunk.Font = new Font(m_setFontName, m_FontSize, Font.BOLD);
                                            parentRowPdfPTable.DefaultCell.Border = Rectangle.KEYWORDS;
                                            parentRowPdfPTable.DefaultCell.BorderWidth = 0.75f;
                                        }
                                    }
                                }
                            }

                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //For '65' Series dont populate the First row and first column cells data 
                            if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                            {
                                if (row != 0)
                                {
                                    parentRowPdfPTable.AddCell(parentCellPhrase);
                                }
                                else
                                {
                                    parentRowPdfPTable.AddCell("");
                                }
                            }
                            else
                            {
                                //Add Annotations to the First column of EveryRow
                                if (m_ShowAnnotations)
                                {
                                    if (sbTip.Length > 0)
                                    {
                                        string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                        Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                        annotImage.Annotation = new Annotation("Annotation", sbTip.Replace("~", "\n").ToString());

                                        Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                        Phrase imagePhrase = new Phrase(imageChunk);
                                        if (col == 0)
                                        {
                                            parentCellPhrase.Add(imagePhrase);
                                            sbTip.Clear();
                                        }
                                    }
                                }

                                parentRowPdfPTable.AddCell(parentCellPhrase);
                            }

                            parentName = cellText;
                        }

                        if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                        {
                            //Add the row table to the document if row is not equal to 2,5 and 7
                            if (row != 2 && row != 5 && row != 7)
                            {
                                pdfDocument.Add(parentRowPdfPTable);
                            }
                        }
                        else
                        {
                            pdfDocument.Add(parentRowPdfPTable);
                        }

                        m_parentPdfPTable = parentRowPdfPTable;
                        //InsertChildTable
                        int Link1 = 0;

                        //Per iteration extract the Link1 value from each row 
                        XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList//Rows");
                        if (parentRowsList[row].Attributes["Link1"] != null)
                        {
                            Link1 = Convert.ToInt32(parentRowsList[row].Attributes["Link1"].Value);
                        }

                        //If reportStyle is "5" || "501" || "502" || "503" extract TrxID values
                        if (m_reportStyle == "5" || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
                        {
                            Link1 = Convert.ToInt32(parentRowsList[row].Attributes["TrxID"].Value);
                        }

                        //Customize data for '65' Series
                        if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                        {
                            if (row == 2 || row == 5 || row == 7)
                            {
                                XmlNodeList xChilNodeColFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNode + "//GridHeading//Columns/Col");

                                #region "PdfTable for Formulas"
                                PdfPTable balanceSheetTable = new PdfPTable(m_ChildColCount);
                                balanceSheetTable.DefaultCell.Border = 0;
                                balanceSheetTable.DefaultCell.Padding = 0.95f;

                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    balanceSheetTable.WidthPercentage = 100;
                                else
                                    balanceSheetTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                balanceSheetTable.TotalWidth = widthTotal;
                                balanceSheetTable.SetWidths(m_childColWidthPct);

                                Chunk dataChunk = new Chunk(rowData[0], new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase dataPhrase = new Phrase(dataChunk);
                                balanceSheetTable.DefaultCell.Border = 0;
                                balanceSheetTable.DefaultCell.BackgroundColor = WHEAT;
                                balanceSheetTable.AddCell(dataPhrase);
                                #endregion

                                #region "PdfPTable for Percentage"
                                PdfPTable percentTable = new PdfPTable(m_ChildColCount);
                                percentTable.DefaultCell.Border = 0;
                                percentTable.DefaultCell.Padding = 0.95f;

                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    percentTable.WidthPercentage = 100;
                                else
                                    percentTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                percentTable.TotalWidth = widthTotal;
                                percentTable.SetWidths(m_childColWidthPct);

                                Chunk percentChunk = new Chunk("%", new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase percentPhrase = new Phrase(percentChunk);
                                percentTable.DefaultCell.Border = 0;
                                percentTable.DefaultCell.BackgroundColor = THISTLE;
                                percentTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                percentTable.AddCell(percentPhrase);
                                #endregion

                                string cellText = string.Empty;
                                decimal totalRevenue = 0;
                                decimal totalProdCost = 0;
                                decimal percent = 0;
                                switch (row)
                                {
                                    #region "Calculate Gross Profit"
                                    case 2:
                                        {
                                            for (int col = 1; col < m_ChildColCount; col++)
                                            {
                                                decimal childTblAmount = 0;
                                                cellText = "0";

                                                foreach (XmlElement elem in xChilNodeColFields)
                                                {
                                                    if (elem.Attributes["Caption"].Value == childXStore.Fields[col].caption)
                                                    {
                                                        if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                                        {
                                                            if (cellText != null && cellText != "")
                                                            {
                                                                if (htRevenue[childXStore.Fields[col].caption] == null || htRevenue[childXStore.Fields[col].caption] == "")
                                                                {
                                                                    htRevenue[childXStore.Fields[col].caption] = 0;
                                                                }

                                                                if (htProdCost[childXStore.Fields[col].caption] == null || htProdCost[childXStore.Fields[col].caption] == "")
                                                                {
                                                                    htProdCost[childXStore.Fields[col].caption] = 0;
                                                                }

                                                                totalRevenue = Convert.ToDecimal(htRevenue[childXStore.Fields[col].caption]);
                                                                totalProdCost = Convert.ToDecimal(htProdCost[childXStore.Fields[col].caption]);

                                                                childTblAmount = totalRevenue - totalProdCost;

                                                                if (childTblAmount < 0)
                                                                {
                                                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                                                }
                                                                else
                                                                {
                                                                    cellText = ConvertToCurrencyFormat(cellText);
                                                                }
                                                            }
                                                            balanceSheetTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                                            htGrossProfit[childXStore.Fields[col].caption] = childTblAmount;
                                                        }
                                                        else
                                                        {
                                                            if (elem.Attributes["ControlType"].Value == "Cal")
                                                            {
                                                                if (cellText != null && cellText != "")
                                                                {
                                                                    DateTime dateTime;
                                                                    DateTime.TryParse(cellText, out dateTime);
                                                                    if (dateTime != DateTime.MinValue)
                                                                    {
                                                                        dateTime = Convert.ToDateTime(cellText);
                                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        break;
                                                    }
                                                }
                                                Chunk balChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                Phrase balPhrase = new Phrase(balChunk);
                                                balanceSheetTable.DefaultCell.Border = 0;
                                                balanceSheetTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                balanceSheetTable.AddCell(balPhrase);

                                                if (totalRevenue != 0)
                                                {
                                                    percent = childTblAmount / totalRevenue;
                                                }

                                                if (percent != 0)
                                                {
                                                    Chunk percentValueChunk = new Chunk(percent.ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                    Phrase percentValuePhrase = new Phrase(percentValueChunk);
                                                    percentTable.DefaultCell.Border = 0;
                                                    percentTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    percentTable.AddCell(percentPhrase);
                                                }
                                                else
                                                {
                                                    percentTable.AddCell("");
                                                }
                                            }
                                            break;
                                        }
                                    #endregion

                                    #region "Calculate Net Income Before Taxes"
                                    case 5:
                                        {
                                            for (int col = 1; col < m_ChildColCount; col++)
                                            {
                                                decimal childTblAmount = 0;
                                                cellText = "0";

                                                foreach (XmlElement elem in xChilNodeColFields)
                                                {
                                                    if (elem.Attributes["Caption"].Value == childXStore.Fields[col].caption)
                                                    {
                                                        if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                                        {
                                                            if (cellText != null && cellText != "")
                                                            {
                                                                if (htOtherCost[childXStore.Fields[col].caption] == null || htOtherCost[childXStore.Fields[col].caption] == "")
                                                                {
                                                                    htOtherCost[childXStore.Fields[col].caption] = 0;
                                                                }

                                                                if (htOtherIncome[childXStore.Fields[col].caption] == null || htOtherIncome[childXStore.Fields[col].caption] == "")
                                                                {
                                                                    htOtherIncome[childXStore.Fields[col].caption] = 0;
                                                                }

                                                                decimal grossProfit = Convert.ToDecimal(htGrossProfit[childXStore.Fields[col].caption]);
                                                                decimal totalOtherCost = Convert.ToDecimal(htOtherCost[childXStore.Fields[col].caption]);
                                                                decimal totalOtherIncome = Convert.ToDecimal(htOtherIncome[childXStore.Fields[col].caption]);

                                                                childTblAmount = grossProfit - totalOtherCost + totalOtherIncome;

                                                                if (childTblAmount < 0)
                                                                {
                                                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                                                }
                                                                else
                                                                {
                                                                    cellText = ConvertToCurrencyFormat(cellText);
                                                                }
                                                            }
                                                            balanceSheetTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                                            htNetIncBefTaxes[childXStore.Fields[col].caption] = childTblAmount;
                                                        }
                                                        else
                                                        {
                                                            if (elem.Attributes["ControlType"].Value == "Cal")
                                                            {
                                                                if (cellText != null && cellText != "")
                                                                {
                                                                    DateTime dateTime;
                                                                    DateTime.TryParse(cellText, out dateTime);
                                                                    if (dateTime != DateTime.MinValue)
                                                                    {
                                                                        dateTime = Convert.ToDateTime(cellText);
                                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        break;
                                                    }
                                                }
                                                Chunk balChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                Phrase balPhrase = new Phrase(balChunk);
                                                balanceSheetTable.DefaultCell.Border = 0;
                                                balanceSheetTable.AddCell(balPhrase);

                                                if (totalRevenue != 0)
                                                {
                                                    percent = childTblAmount / totalRevenue;
                                                }

                                                if (percent != 0)
                                                {
                                                    Chunk percentValueChunk = new Chunk(percent.ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                    Phrase percentValuePhrase = new Phrase(percentValueChunk);
                                                    percentTable.DefaultCell.Border = 0;
                                                    percentTable.AddCell(percentPhrase);
                                                }
                                                else
                                                {
                                                    percentTable.AddCell("");
                                                }

                                            }
                                            break;
                                        }
                                    #endregion

                                    #region "Net Income"
                                    case 7:
                                        {
                                            for (int col = 1; col < m_ChildColCount; col++)
                                            {
                                                decimal childTblAmount = 0;
                                                cellText = "0";

                                                foreach (XmlElement elem in xChilNodeColFields)
                                                {
                                                    if (elem.Attributes["Caption"].Value == childXStore.Fields[col].caption)
                                                    {
                                                        if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                                        {
                                                            if (cellText != null && cellText != "")
                                                            {
                                                                if (htTaxes[childXStore.Fields[col].caption] == null || htTaxes[childXStore.Fields[col].caption] == "")
                                                                {
                                                                    htTaxes[childXStore.Fields[col].caption] = 0;
                                                                }

                                                                decimal netIncBefTaxes = Convert.ToDecimal(htNetIncBefTaxes[childXStore.Fields[col].caption]);
                                                                decimal totalTaxes = Convert.ToDecimal(htTaxes[childXStore.Fields[col].caption]);

                                                                childTblAmount = netIncBefTaxes - totalTaxes;

                                                                if (childTblAmount < 0)
                                                                {
                                                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                                                }
                                                                else
                                                                {
                                                                    cellText = ConvertToCurrencyFormat(cellText);
                                                                }
                                                            }
                                                            balanceSheetTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                                            htNetIncome[childXStore.Fields[col].caption] = childTblAmount;
                                                        }
                                                        else
                                                        {
                                                            if (elem.Attributes["ControlType"].Value == "Cal")
                                                            {
                                                                if (cellText != null && cellText != "")
                                                                {
                                                                    DateTime dateTime;
                                                                    DateTime.TryParse(cellText, out dateTime);
                                                                    if (dateTime != DateTime.MinValue)
                                                                    {
                                                                        dateTime = Convert.ToDateTime(cellText);
                                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        break;
                                                    }
                                                }
                                                Chunk balChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                Phrase balPhrase = new Phrase(balChunk);
                                                balanceSheetTable.DefaultCell.Border = 0;
                                                balanceSheetTable.AddCell(balPhrase);

                                                if (totalRevenue != 0)
                                                {
                                                    percent = childTblAmount / totalRevenue;
                                                }

                                                if (percent != 0)
                                                {
                                                    Chunk percentValueChunk = new Chunk(percent.ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                                                    Phrase percentValuePhrase = new Phrase(percentValueChunk);
                                                    percentTable.DefaultCell.Border = 0;
                                                    percentTable.AddCell(percentPhrase);
                                                }
                                                else
                                                {
                                                    percentTable.AddCell("");
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                }

                                pdfDocument.Add(balanceSheetTable);
                                pdfDocument.Add(percentTable);
                                pdfDocument.Add(new Paragraph(" "));
                            }
                            else
                            {
                                //To Insert Corresponding ChildTable for the Respective Parent
                                if (childTreeNode != "" && Link1 != 0)
                                {
                                    childTableWidth = InsertChildTable(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, balanceForward, Link1, row, columnTypeTable);
                                }
                            }
                        }
                        else
                        {
                            //if (m_reportStyle == "622")
                            //{
                            //    childTableWidth = Insert622ChildTable(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, balanceForward, Link1, row, columnTypeTable);
                            //}

                            //else
                            //{
                            //To Insert Corresponding ChildTable for the Respective Parent
                            if (childTreeNode != "" && Link1 != 0)
                            {
                                childTableWidth = InsertChildTable(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, balanceForward, Link1, row, columnTypeTable);
                            }
                            //}
                        }
                        pdfDocument.Add(new Paragraph(""));
                    }

                    //Calculate the GrandTotal for the Parent 
                    if (m_reportStyle != "1")
                    {
                        ComputeGrandTotal(numColumnsInPDF, parentPdfPTable);
                    }
                    else
                    {
                        if (amtFieldCount > 0)
                        {
                            ComputeGrandTotal(numColumnsInPDF, parentPdfPTable);
                        }
                    }
                    // let the caller know we successfully reached 'the end' of this 
                    //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                    bRet = true;
                }
            }

            if (bRet == false)
            {
                //pdfDocument.Add(new Paragraph("Failed to load data"));
            }

            return bRet;
        }

        // For Adding columnTypeTable for reportStyles 601,602,603 & 604
        private void CustomizeTBalReports(Document pdfDocument, PdfPTable spaceTable, PdfPTable columnTypeTable)
        {
            columnTypeTable.AddCell("");
            columnTypeTable.AddCell("");

            string strTab = string.Empty;

            switch (m_reportStyle)
            {
                case "601":
                    {
                        strTab = "\t\t\t\t\t\t\t\t\t";
                        columnTypeTable.DefaultCell.Colspan = 2;
                        Chunk tBalanceChunk = new Chunk(strTab + "Trial Balance", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tBalancePhrase = new Phrase(tBalanceChunk);
                        columnTypeTable.AddCell(tBalancePhrase);

                        Chunk incomeStmtChunk = new Chunk(strTab + "Income Statement", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase incomeStmtPhrase = new Phrase(incomeStmtChunk);
                        columnTypeTable.AddCell(incomeStmtPhrase);

                        Chunk balSheetChunk = new Chunk(strTab + "Balance Sheet", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase balSheetPhrase = new Phrase(balSheetChunk);
                        columnTypeTable.AddCell(balSheetPhrase);
                        break;
                    }
                case "602":
                    {
                        strTab = "\t\t\t\t\t";
                        Chunk tBalForwardChunk = new Chunk(strTab + "Balance Forward", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tBalForwardPhrase = new Phrase(tBalForwardChunk);
                        columnTypeTable.AddCell(tBalForwardPhrase);

                        columnTypeTable.DefaultCell.Colspan = 2;
                        Chunk activityChunk = new Chunk(strTab + strTab + strTab + "Activity", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase activityPhrase = new Phrase(activityChunk);
                        columnTypeTable.AddCell(activityPhrase);

                        strTab = "\t\t\t\t";
                        columnTypeTable.DefaultCell.Colspan = 1;
                        Chunk endBalChunk = new Chunk(strTab + "Ending Balance", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase endBalPhrase = new Phrase(endBalChunk);
                        columnTypeTable.AddCell(endBalPhrase);
                        break;
                    }
                case "604":
                    {
                        strTab = "\t\t\t\t\t\t\t\t\t\t";
                        columnTypeTable.DefaultCell.Colspan = 2;
                        Chunk tBalanceChunk = new Chunk("\t\t" + strTab + strTab + strTab + strTab + "Trial Balance", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tBalancePhrase = new Phrase(tBalanceChunk);
                        columnTypeTable.AddCell(tBalancePhrase);

                        Chunk incomeStmtChunk = new Chunk(strTab + strTab + strTab + "Income Statement", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase incomeStmtPhrase = new Phrase(incomeStmtChunk);
                        columnTypeTable.AddCell(incomeStmtPhrase);

                        Chunk balSheetChunk = new Chunk("\t\t\t\t\t\t" + strTab + "Balance Sheet", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase balSheetPhrase = new Phrase(balSheetChunk);
                        columnTypeTable.AddCell(balSheetPhrase);
                        break;
                    }
            }

            if (m_reportStyle == "601" || m_reportStyle == "602" || m_reportStyle == "604")
            {
                pdfDocument.Add(columnTypeTable);
                pdfDocument.Add(spaceTable);
            }
        }

        //Method to Generate Report for 2
        private void LoadHistoryDocument(Document pdfDocument)
        {
            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;
            string subChildTreeNode = string.Empty;
            string trxTreeNodeName = string.Empty;
            string trxSecondTreeNodeName = string.Empty;
            string trxThirdTreeNodeName = string.Empty;

            decimal balanceForward = 0;
            int treeNodesCount = 0;

            Hashtable htControlType = new Hashtable();
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            //To get the TreeNodeNames
            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            childTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }

                }

                treeNodesCount++;
            }

            //If Report Style equals "2" || "3" set the ChildNodeName to BranchNodeName
            if (m_reportStyle == "2" || m_reportStyle == "3")
            {
                branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
                childTreeNode = branchNodeName;
            }

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //Increase Font Size based on column Count
            if (numColumnsInXml > 10)
            {
                m_FontSize = 6f;
            }
            else
            {
                m_FontSize = 7f;
            }

            //Table for creating Space between rows
            PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
            spaceTable.DefaultCell.GrayFill = 1.0f;
            spaceTable.DefaultCell.Border = 0;
            spaceTable.DefaultCell.Colspan = numColumnsInXml;

            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
            Phrase spacePhrase = new Phrase(spaceChunk);
            spaceTable.AddCell(spacePhrase);

            //To Get the Corresponding ControlTypes of Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            xmlParentCount = numRecordsInXml;

            for (int row = 0; row < numRecordsInXml; row++)
            {
                //Get the corresponding RowData from the xmlStore 
                string[] rowData = xmlStore.GetRecord(row);

                //Load Records column by column
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    //Table to Load columnNames and values in the same row
                    PdfPTable accountPdfPTable = new PdfPTable(12);
                    accountPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
                    float[] columnWidthInPct = new float[12];
                    float widthTotal;

                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    this.pageWidth = widthTotal;

                    //Set Col Widths
                    SetAccountColWidths(numColumnsInXml, columnWidthInPct, xmlStore);

                    //Set the TotalWidth of the Table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        accountPdfPTable.WidthPercentage = 100;
                    else
                        accountPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    accountPdfPTable.TotalWidth = widthTotal;
                    accountPdfPTable.SetWidths(columnWidthInPct);
                    accountPdfPTable.DefaultCell.BorderWidth = 0;
                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    spaceTable.WidthPercentage = accountPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = accountPdfPTable.TotalWidth;

                    string colName = xmlStore.Fields[col].caption;
                    string cellText = rowData[col];
                    int fullViewLength = 0;
                    string controlType = string.Empty;

                    if (cellText != null && cellText != "")
                    {
                        if (htControlType[xmlStore.Fields[col].label] != null)
                        {
                            fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                            controlType = htControlType[xmlStore.Fields[col].label].ToString();

                            if (htControlType[xmlStore.Fields[col].label].ToString() == "Amount")
                            {
                                decimal colAmount = Convert.ToDecimal(cellText);

                                if (colAmount < 0)
                                {
                                    cellText = "(" + ConvertToCurrencyFormat(cellText) + ")";
                                }
                                else
                                {
                                    cellText = ConvertToCurrencyFormat(cellText);
                                }

                            }
                            else
                            {
                                if (htControlType[xmlStore.Fields[col].label].ToString() == "Cal")
                                {
                                    if (cellText != null && cellText != "")
                                    {
                                        DateTime dateTime;
                                        DateTime.TryParse(cellText, out dateTime);
                                        if (dateTime != DateTime.MinValue)
                                        {
                                            string[] dateArray = dateTime.ToLongDateString().Split(',');
                                            cellText = dateArray[1].Trim() + ", " + dateArray[2].Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                    if (controlType == "TBox")
                    {
                        if (cellText.Length > fullViewLength)
                        {
                            cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                        }
                    }

                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    string strCaption = xmlStore.Fields[col].caption + ": ";

                    Chunk captionCellChunk = new Chunk(strCaption, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase captionCellPhrase = new Phrase(captionCellChunk);
                    accountPdfPTable.DefaultCell.Border = 0;
                    accountPdfPTable.AddCell(captionCellPhrase);

                    accountPdfPTable.DefaultCell.Colspan = 11;
                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                    Phrase parentCellPhrase = new Phrase(parentCellChunk);
                    accountPdfPTable.DefaultCell.Border = 0;
                    accountPdfPTable.AddCell(parentCellPhrase);

                    accountPdfPTable.DefaultCell.Colspan = 1;

                    pdfDocument.Add(accountPdfPTable);
                    //pdfDocument.Add(spaceTable);
                }

                //Add the SpaceTable
                pdfDocument.Add(spaceTable);

                //Table to be used for Report Styles 601,602,603 & 604
                PdfPTable columnTypeTable = new PdfPTable(numColumnsInXml);

                //Per iteration extract the Link1 value from each row 
                int Link1 = 0;
                XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//RowList//Rows");
                if (parentRowsList[row].Attributes["TrxID"] != null)
                {
                    Link1 = Convert.ToInt32(parentRowsList[row].Attributes["TrxID"].Value);
                }

                //To Insert Corresponding ChildTable
                if (childTreeNode != "" && Link1 != 0)
                {
                    float childTableWidth = InsertChildTable(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, balanceForward, Link1, row, columnTypeTable);
                }

                if (m_reportStyle == "2")
                {
                    if (row != numRecordsInXml - 1)
                    {
                        pdfDocument.NewPage();
                    }
                }
            }
        }

        //Method to Generate Report for ReportStyle7
        private void LoadDoc7(Document pdfDocument)
        {
            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;
            string subChildTreeNode = string.Empty;
            string trxTreeNodeName = string.Empty;
            string trxSecondTreeNodeName = string.Empty;
            string trxThirdTreeNodeName = string.Empty;

            int treeNodesCount = 0;

            Hashtable htControlType = new Hashtable();

            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            //To get the TreeNodeNames
            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            childTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                }

                treeNodesCount++;
            }

            //set the ChildNodeName to BranchNodeName
            branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
            childTreeNode = branchNodeName;

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //Increase Font Size based on column Count
            if (numColumnsInXml > 10)
            {
                m_FontSize = 6f;
            }
            else
            {
                m_FontSize = 7f;
            }

            //Table for creating Space between rows
            PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
            spaceTable.DefaultCell.GrayFill = 1.0f;
            spaceTable.DefaultCell.Border = 0;
            spaceTable.DefaultCell.Colspan = numColumnsInXml;

            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
            Phrase spacePhrase = new Phrase(spaceChunk);
            spaceTable.AddCell(spacePhrase);

            //To Load Corresponding Control Types of the Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            xmlParentCount = numRecordsInXml;

            for (int row = 0; row < numRecordsInXml; row++)
            {
                if (numColumnsInXml > 10)
                {
                    m_FontSize = 6f;
                }
                else
                {
                    m_FontSize = 7f;
                }
                //Get the corresponding RowData from the xmlStore
                string[] rowData = xmlStore.GetRecord(row);

                //Table to Load Column Names and Values in the Same Row
                PdfPTable accountPdfPTable = new PdfPTable(8);
                accountPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
                float[] columnWidthInPct = new float[8];
                float widthTotal;

                //Change TotalWidth on LandScape Value
                if (m_ShowLandscape)
                {
                    widthTotal = 125;
                }
                else
                {
                    widthTotal = 145;
                }

                this.pageWidth = widthTotal;

                //Set Col Widths
                SetAccountColWidths(numColumnsInXml, columnWidthInPct, xmlStore);

                //Set the TotalWidth of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                    accountPdfPTable.WidthPercentage = 100;
                else
                    accountPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                accountPdfPTable.TotalWidth = widthTotal;
                accountPdfPTable.SetWidths(columnWidthInPct);
                accountPdfPTable.DefaultCell.BorderWidth = 0;
                accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                spaceTable.WidthPercentage = accountPdfPTable.WidthPercentage;
                spaceTable.TotalWidth = accountPdfPTable.TotalWidth;

                //Table into which accountpdfPtable and ChildTable have to be inserted
                PdfPTable cellPdfPTable = new PdfPTable(2);
                cellPdfPTable.DefaultCell.NoWrap = true;

                //Load Records column by column
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    string colName = xmlStore.Fields[col].caption;
                    string cellText = rowData[col];
                    int fullViewLength = 0;
                    string controlType = string.Empty;

                    if (cellText != null && cellText != "")
                    {
                        if (htControlType[xmlStore.Fields[col].label] != null)
                        {
                            fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                            controlType = htControlType[xmlStore.Fields[col].label].ToString();

                            if (htControlType[xmlStore.Fields[col].label].ToString() == "Amount")
                            {
                                decimal colAmount = Convert.ToDecimal(cellText);

                                if (colAmount < 0)
                                {
                                    cellText = "(" + ConvertToCurrencyFormat(cellText) + ")";
                                }
                                else
                                {
                                    cellText = ConvertToCurrencyFormat(cellText);
                                }

                            }
                            else
                            {
                                if (htControlType[xmlStore.Fields[col].label].ToString() == "Cal")
                                {
                                    if (cellText != null && cellText != "")
                                    {
                                        DateTime dateTime;
                                        DateTime.TryParse(cellText, out dateTime);
                                        if (dateTime != DateTime.MinValue)
                                        {
                                            dateTime = Convert.ToDateTime(cellText);
                                            cellText = dateTime.ToShortDateString();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                    if (controlType == "TBox")
                    {
                        if (cellText.Length > fullViewLength)
                        {
                            cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                        }
                    }

                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    cellPdfPTable.TotalWidth = 47;
                    cellPdfPTable.WidthPercentage = cellPdfPTable.TotalWidth * m_WidthScaleFactor;
                    cellPdfPTable.DefaultCell.Border = 0;
                    cellPdfPTable.DefaultCell.Padding = 0.95f;
                    float[] cellColWidths = { 13, 34 };
                    cellPdfPTable.SetWidths(cellColWidths);

                    string strCaption = xmlStore.Fields[col].caption + ": ";

                    Chunk captionCellChunk = new Chunk(strCaption, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase captionCellPhrase = new Phrase(captionCellChunk);
                    cellPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellPdfPTable.AddCell(captionCellPhrase);

                    if (cellText.Length > 28)
                    {
                        CharEnumerator charEnum = cellText.GetEnumerator();
                        int enumCount = 0;
                        cellText = "";
                        while (charEnum.MoveNext())
                        {
                            if (enumCount < 28)
                            {
                                cellText = cellText + charEnum.Current.ToString();
                            }
                            else
                            {
                                cellText = cellText + charEnum.Current.ToString() + "..";
                                break;
                            }
                            enumCount++;
                        }
                    }

                    Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                    Phrase parentCellPhrase = new Phrase(parentCellChunk);
                    cellPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellPdfPTable.AddCell(parentCellPhrase);
                }

                accountPdfPTable.AddCell(cellPdfPTable);
                accountPdfPTable.DefaultCell.Colspan = 7;

                //Per iteration extract the TrxID value from each row 
                int Link1 = 0;
                XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//RowList//Rows");
                if (parentRowsList[row].Attributes["TrxID"] != null)
                {
                    Link1 = Convert.ToInt32(parentRowsList[row].Attributes["TrxID"].Value);
                }

                //InsertChildTable
                if (childTreeNode != "" && Link1 != 0)
                {
                    float childTableWidth = InsertChildTable7(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, Link1, row, accountPdfPTable, numColumnsInXml);
                }
                //accountPdfPTable.AddCell(new Paragraph(""));
                accountPdfPTable.DefaultCell.Colspan = 1;
                pdfDocument.Add(accountPdfPTable);
                pdfDocument.Add(spaceTable);
                pdfDocument.Add(spaceTable);
            }
        }

        //Method to Generate Report for 200
        private void LoadDocument200(Document pdfDocument)
        {
            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;
            string subChildTreeNode = string.Empty;
            string trxTreeNodeName = string.Empty;
            string trxSecondTreeNodeName = string.Empty;
            string trxThirdTreeNodeName = string.Empty;

            decimal balanceForward = 0;
            int treeNodesCount = 0;

            Hashtable htControlType = new Hashtable();

            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            //To get the TreeNodeNames
            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            childTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }

                }

                treeNodesCount++;
            }

            //Set the ChildNodeName to BranchNodeName
            if (m_reportStyle == "200")
            {
                branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
                childTreeNode = branchNodeName;
            }

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //Table for creating Space between rows
            PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
            spaceTable.DefaultCell.GrayFill = 1.0f;
            spaceTable.DefaultCell.Border = 0;
            spaceTable.DefaultCell.Colspan = numColumnsInXml;

            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
            Phrase spacePhrase = new Phrase(spaceChunk);
            spaceTable.AddCell(spacePhrase);

            //To Load Corresponding Control Types of the Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            m_FontSize = 7f;

            for (int row = 0; row < numRecordsInXml; row++)
            {
                //Get the corresponding RowData from the xmlStore 
                string[] rowData = xmlStore.GetRecord(row);

                for (int col = 0; col < numColumnsInXml; col++)
                {
                    //Table to Load RowData
                    PdfPTable accountPdfPTable = new PdfPTable(numColumnsInXml);
                    accountPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
                    float[] columnWidthInPct = new float[numColumnsInXml];
                    float widthTotal;

                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    this.pageWidth = widthTotal;

                    //Set Col Widths
                    SetAccountColWidths(numColumnsInXml, columnWidthInPct, xmlStore);

                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        accountPdfPTable.WidthPercentage = 100;
                    else
                        accountPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    accountPdfPTable.TotalWidth = widthTotal;
                    accountPdfPTable.SetWidths(columnWidthInPct);
                    accountPdfPTable.DefaultCell.BorderWidth = 0;
                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    spaceTable.WidthPercentage = accountPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = accountPdfPTable.TotalWidth;

                    string colName = xmlStore.Fields[col].caption;
                    string cellText = rowData[col];
                    int fullViewLength = 0;
                    string controlType = string.Empty;

                    if (cellText != null && cellText != "")
                    {
                        if (htControlType[xmlStore.Fields[col].label] != null)
                        {
                            fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                            controlType = htControlType[xmlStore.Fields[col].label].ToString();

                            if (htControlType[xmlStore.Fields[col].label].ToString() == "Amount")
                            {
                                decimal colAmount = Convert.ToDecimal(cellText);

                                if (colAmount < 0)
                                {
                                    cellText = "(" + ConvertToCurrencyFormat(cellText) + ")";
                                }
                                else
                                {
                                    cellText = ConvertToCurrencyFormat(cellText);
                                }

                            }
                            else
                            {
                                if (htControlType[xmlStore.Fields[col].label].ToString() == "Cal")
                                {
                                    if (cellText != null && cellText != "")
                                    {
                                        DateTime dateTime;
                                        DateTime.TryParse(cellText, out dateTime);
                                        if (dateTime != DateTime.MinValue)
                                        {
                                            string[] dateArray = dateTime.ToLongDateString().Split(',');
                                            cellText = dateArray[1].Trim() + ", " + dateArray[2].Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength
                    if (controlType == "TBox")
                    {
                        if (cellText.Length > fullViewLength)
                        {
                            cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                        }
                    }

                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    if (xmlStore.Fields[col].label == "InvNumber" || xmlStore.Fields[col].label == "ARInfo1")
                    {
                        cellText = xmlStore.Fields[col].caption + ": " + cellText;
                    }

                    accountPdfPTable.DefaultCell.Colspan = numColumnsInXml;

                    //Customize if Label is Address
                    string[] addrArray;
                    if (xmlStore.Fields[col].label == "Address1")
                    {
                        addrArray = cellText.Split('~');

                        for (int arrayCount = 0; arrayCount < addrArray.Length; arrayCount++)
                        {
                            Chunk parentCellChunk = new Chunk(addrArray[arrayCount], new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase parentCellPhrase = new Phrase(parentCellChunk);
                            accountPdfPTable.DefaultCell.Border = 0;
                            accountPdfPTable.AddCell(parentCellPhrase);
                        }
                    }
                    else
                    {
                        Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase parentCellPhrase = new Phrase(parentCellChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(parentCellPhrase);

                        accountPdfPTable.DefaultCell.Colspan = 1;
                    }

                    if (xmlStore.Fields[col].label == "InvDate" || xmlStore.Fields[col].label == "InvNumber" || xmlStore.Fields[col].label == "Customer" || xmlStore.Fields[col].label == "Address1" || xmlStore.Fields[col].label == "ARInfo1")
                    {
                        pdfDocument.Add(accountPdfPTable);
                        pdfDocument.Add(spaceTable);
                    }

                    accountPdfPTable.DefaultCell.Colspan = 1;
                }

                pdfDocument.Add(spaceTable);

                //Insert a Line in between records and Table
                PdfPTable borderTable = new PdfPTable(numColumnsInXml);
                borderTable.WidthPercentage = spaceTable.WidthPercentage;
                borderTable.TotalWidth = spaceTable.TotalWidth;
                borderTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                borderTable.DefaultCell.BorderWidthBottom = 1f;
                borderTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk borderChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase borderPhrase = new Phrase(borderChunk);
                borderTable.AddCell(borderPhrase);
                pdfDocument.Add(borderTable);

                pdfDocument.Add(spaceTable);
                pdfDocument.Add(spaceTable);

                PdfPTable columnTypeTable = new PdfPTable(numColumnsInXml);

                //Extract TrxID values
                int Link1 = 0;
                XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//RowList//Rows");
                if (parentRowsList[row].Attributes["TrxID"] != null)
                {
                    Link1 = Convert.ToInt32(parentRowsList[row].Attributes["TrxID"].Value);
                }

                if (childTreeNode != "" && Link1 != 0)
                {
                    //InsertChildTable
                    parentName = "Due";
                    float childTableWidth = InsertChildTable(parentTreeNode, childTreeNode, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, numColumnsInXml, balanceForward, Link1, row, columnTypeTable);
                }
            }
        }

        //Method to Generate Report for 300
        private void LoadDocument300(Document pdfDocument)
        {
            string strAccountingDoc = string.Empty;
            string strJournalDoc = string.Empty;
            string strAccountingItem = string.Empty;
            int nodeCount = 0;

            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            XmlNode nodeTree = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree");

            strAccountingDoc = nodeTree.SelectSingleNode("Node").InnerText.ToString();

            XmlNode nodeBranch = nodeTree.SelectSingleNode("Branches");
            foreach (XmlNode node in nodeBranch.ChildNodes)
            {
                if (nodeCount == 0)
                    strJournalDoc = node.SelectSingleNode("Node").InnerText.ToString();
                else
                    strAccountingItem = node.SelectSingleNode("Node").InnerText.ToString();

                nodeCount++;
            }

            XmlNode nodeFormControls = m_xDoc.SelectSingleNode("Root/bpeout/FormControls");
            XmlNodeList xNodelistFields = nodeFormControls.SelectNodes(strJournalDoc + "/GridHeading/Columns/Col");

            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = strJournalDoc;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;
            parentXStore = xmlStore;

            float widthTotal;
            if (m_ShowLandscape)
            {
                widthTotal = 125;
            }
            else
            {
                widthTotal = 145;
            }

            this.pageWidth = widthTotal;
            float[] headerTblWidths = { widthTotal / 2, widthTotal / 2 };

            m_FontSize = 15;
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.DefaultCell.Padding = 0.95f;  //in Point
            headerTable.TotalWidth = widthTotal;
            headerTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
            headerTable.SetWidths(headerTblWidths);
            headerTable.DefaultCell.Border = 0;
            headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

            pdfDocument.Add(new Paragraph("\n"));

            clsReportsUI objReportsUI = new clsReportsUI();
            string img = objReportsUI.PDFImagePath();
            Image image = Image.GetInstance(img);
            image.ScaleAbsolute(70, 60);

            Chunk imgChunk = new Chunk(image, 0, 0);
            Phrase imgPhrase = new Phrase(imgChunk);

            XmlNode nodePurchOrderRow = nodeFormControls.SelectSingleNode(strAccountingDoc + "/RowList");
            string purchOrderNum = nodePurchOrderRow.SelectSingleNode("Rows").Attributes["PurchOrderNumber"].Value.ToString();
            Chunk purchOrderChunk = new Chunk("No." + purchOrderNum, new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase purchOrderPhrase = new Phrase(purchOrderChunk);

            string logoCaption = nodePurchOrderRow.SelectSingleNode("Rows").Attributes["LogoCaption"].Value.ToString();
            logoCaption = logoCaption.Replace("~", "\n");
            Chunk logoCaptionChunk = new Chunk("\n\n\n\n\n\n" + logoCaption, new Font(m_setFontName, 7.5f, Font.BOLD));
            Phrase logoCaptionPhrase = new Phrase(logoCaptionChunk);

            float[] logoColWidth = { widthTotal / 3, widthTotal * 2 / 3 };
            PdfPTable logoPdfPTable = new PdfPTable(2);
            logoPdfPTable.DefaultCell.Border = 0;
            logoPdfPTable.DefaultCell.Padding = 0f;
            logoPdfPTable.TotalWidth = widthTotal / 2;
            logoPdfPTable.WidthPercentage = widthTotal / 2 * m_WidthScaleFactor;
            logoPdfPTable.SetWidths(logoColWidth);

            logoPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            logoPdfPTable.AddCell(imgPhrase);
            logoPdfPTable.DefaultCell.Rowspan = 2;
            //logoPdfPTable.AddCell(new Paragraph(""));
            //logoPdfPTable.AddCell(new Paragraph(""));
            logoPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            logoPdfPTable.AddCell(logoCaptionPhrase);
            logoPdfPTable.AddCell(new Paragraph(""));

            headerTable.AddCell(logoPdfPTable);

            float[] pOrderColWidth = { widthTotal / 2 };
            PdfPTable pOrderPdfPTable = new PdfPTable(1);
            pOrderPdfPTable.DefaultCell.Border = 0;
            pOrderPdfPTable.DefaultCell.Padding = 0.95f;
            pOrderPdfPTable.TotalWidth = widthTotal / 2;
            pOrderPdfPTable.WidthPercentage = widthTotal / 2 * m_WidthScaleFactor;
            pOrderPdfPTable.SetWidths(pOrderColWidth);
            pOrderPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            Chunk headerChunk = new Chunk(new Chunk("PURCHASE ORDER/CHECK REQUEST", new Font(Font.HELVETICA, m_FontSize, Font.BOLD)));
            Phrase headerPhrase = new Phrase(headerChunk);
            pOrderPdfPTable.AddCell(headerPhrase);
            pOrderPdfPTable.AddCell(new Paragraph(" "));
            pOrderPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pOrderPdfPTable.AddCell(purchOrderPhrase);
            headerTable.AddCell(pOrderPdfPTable);

            XmlNode nodeAcctRowlist = nodeFormControls.SelectSingleNode(strAccountingDoc + "/RowList");
            XmlNodeList strAcctColList = nodeFormControls.SelectNodes(strAccountingDoc + "/GridHeading/Columns/Col");
            string strApprovedByCaption = string.Empty;
            string strDate = string.Empty;
            int acctColCount = 0;

            foreach (XmlElement elem in strAcctColList)
            {
                if (elem.Attributes["Label"].Value == "InvDate")
                {
                    strDate = elem.Attributes["Caption"].Value;
                    acctColCount++;
                }

                if (elem.Attributes["Label"].Value == "ApprovedBy")
                {
                    strApprovedByCaption = elem.Attributes["Caption"].Value;
                    acctColCount++;
                }

                if (acctColCount == 2)
                {
                    break;
                }
            }

            string strApprovedBy = nodeAcctRowlist.SelectSingleNode("Rows").Attributes["ApprovedBy"].Value;
            string imgFileName = nodeAcctRowlist.SelectSingleNode("Rows").Attributes["FileName"].Value;
            string strDateData = nodeAcctRowlist.SelectSingleNode("Rows").Attributes["InvDate"].Value;

            if (strDateData != null && strDateData != "")
            {
                strDateData = Convert.ToDateTime(strDateData).ToString("MM/dd/yyyy");
            }

            //headerTable.AddCell(new Paragraph(""));
            //headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headerTable.AddCell(purchOrderPhrase);

            pdfDocument.Add(headerTable);
            pdfDocument.Add(new Paragraph("\n"));

            m_FontSize = 8.5f;

            float[] finalJnlWidths = { 67.5f, 5, widthTotal / 2 };
            PdfPTable finalJnlPdfPTable = new PdfPTable(3);
            finalJnlPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            finalJnlPdfPTable.TotalWidth = widthTotal;
            finalJnlPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
            finalJnlPdfPTable.SetWidths(finalJnlWidths);
            finalJnlPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            finalJnlPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            float finalJnlSecTableWidth = widthTotal / 2;
            float[] finalJnlSecWidths = { widthTotal / 4, widthTotal / 4 };
            PdfPTable finalJnlSecPdfPTable = new PdfPTable(2);
            finalJnlSecPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            finalJnlSecPdfPTable.TotalWidth = finalJnlSecTableWidth;
            finalJnlSecPdfPTable.WidthPercentage = finalJnlSecTableWidth * m_WidthScaleFactor;
            finalJnlSecPdfPTable.SetWidths(finalJnlSecWidths);
            finalJnlSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            finalJnlSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            //Set TotalWidth Based
            float journalInitTableWidth = widthTotal / 2;
            //Set Column Widths
            float[] journalInitColWidth = { 15, 57.5f };

            PdfPTable journalInitPdfPTable = new PdfPTable(2);
            journalInitPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            journalInitPdfPTable.TotalWidth = journalInitTableWidth;
            journalInitPdfPTable.WidthPercentage = journalInitTableWidth * m_WidthScaleFactor;
            journalInitPdfPTable.SetWidths(journalInitColWidth);
            journalInitPdfPTable.DefaultCell.Border = 0;

            float journalSecTableWidth = widthTotal / 4;
            float[] journalSecColWidth = { 21.12f, 15.125f };
            PdfPTable journalSecPdfPTable = new PdfPTable(2);
            journalSecPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            journalSecPdfPTable.TotalWidth = journalSecTableWidth;
            journalSecPdfPTable.WidthPercentage = journalSecTableWidth * m_WidthScaleFactor;
            journalSecPdfPTable.SetWidths(journalSecColWidth);
            journalSecPdfPTable.DefaultCell.Border = 0;

            float journalThirdTableWidth = widthTotal / 4;
            float[] journalThirdColWidth = { 21.12f, 15.125f };
            PdfPTable journalThirdPdfPTable = new PdfPTable(2);
            journalThirdPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            journalThirdPdfPTable.TotalWidth = journalThirdTableWidth;
            journalThirdPdfPTable.WidthPercentage = journalThirdTableWidth * m_WidthScaleFactor;
            journalThirdPdfPTable.SetWidths(journalThirdColWidth);
            journalThirdPdfPTable.DefaultCell.Border = 0;

            float chkBoxWidth = (widthTotal) / 12;
            float[] chkBoxTblColWidth = { chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth, chkBoxWidth };
            PdfPTable chkBoxPdfPTable = new PdfPTable(12);
            chkBoxPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            chkBoxPdfPTable.TotalWidth = widthTotal;
            chkBoxPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
            chkBoxPdfPTable.SetWidths(chkBoxTblColWidth);
            chkBoxPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            //cashTypePdfPTable.DefaultCell.Border = Rectangle.TABLE;
            chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            Image unCheckedImage = null;
            //string imgUrl = System.AppDomain.CurrentDomain.BaseDirectory + "\\App_Themes\\LAjit\\Images\\cross.gif";
            string imgUrl = m_strImagesCDNPath + "Images/spacer.gif";
            unCheckedImage = Image.GetInstance(new Uri(imgUrl));
            unCheckedImage.ScaleAbsolute(15, 15);

            Chunk unCheckImgChunk = new Chunk(unCheckedImage, 0, 0, false);
            Phrase unCheckImgPhrase = new Phrase(unCheckImgChunk);

            Image checkedImage = null;
            imgUrl = m_strImagesCDNPath + "Images/tick.gif";
            checkedImage = Image.GetInstance(new Uri(imgUrl));
            checkedImage.ScaleAbsolute(15, 15);

            Chunk checkImgChunk = new Chunk(checkedImage, 0, 0, false);
            Phrase checkImgPhrase = new Phrase(checkImgChunk);

            PdfPTable spaceTable = new PdfPTable(2);
            spaceTable.TotalWidth = journalInitTableWidth;
            spaceTable.WidthPercentage = journalInitTableWidth * m_WidthScaleFactor;
            spaceTable.DefaultCell.Colspan = 2;
            spaceTable.DefaultCell.Border = 0;

            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
            Phrase spacePhrase = new Phrase(spaceChunk);
            spaceTable.AddCell(spacePhrase);

            for (int row = 0; row < numRecordsInXml; row++)
            {
                string strPOTo = string.Empty;
                string strContact1 = string.Empty;
                string strSentTo = string.Empty;
                string strTelephone = string.Empty;
                string strFederalID = string.Empty;
                string strRetired = string.Empty;
                string strJobNum = string.Empty;
                string strTitleDef = string.Empty;
                string strDateNeeded = string.Empty;
                string strTimeNeeded = string.Empty;

                string strPOToData = string.Empty;
                string strContact1Data = string.Empty;
                string strSentToData = string.Empty;
                string strTelephoneData = string.Empty;
                string strFederalIDData = string.Empty;
                string strJobNumData = string.Empty;
                string strTitleDefData = string.Empty;
                string strDateNeededData = string.Empty;
                string strTimeNeededData = string.Empty;

                string strDepCheck = string.Empty;
                string strPayCheck = string.Empty;
                string strPettyCash = string.Empty;
                string strCreditCard = string.Empty;
                string strWillBill = string.Empty;
                string strAddBill = string.Empty;

                bool isDepCheck = false;
                bool isPayCheck = false;
                bool isPettyCash = false;
                bool isCreditCard = false;
                bool isWillBill = false;
                bool isAddBill = false;
                bool isRetired = false;

                string[] rowData = xmlStore.GetRecord(row);

                for (int colCount = 0; colCount < numColumnsInXml; colCount++)
                {
                    string colText = xmlStore.Fields[colCount].caption;
                    string cellText = rowData[colCount];

                    string strColLabel = xmlStore.Fields[colCount].label;
                    switch (strColLabel)
                    {
                        case "POTo":
                            {
                                strPOTo = colText;
                                strPOToData = cellText;
                                break;
                            }
                        case "Contact1":
                            {
                                strContact1 = colText;
                                strContact1Data = cellText;
                                break;
                            }
                        case "SentTo":
                            {
                                strSentTo = colText;
                                strSentToData = cellText;
                                break;
                            }
                        case "Telephone":
                            {
                                strTelephone = colText;
                                strTelephoneData = cellText;
                                break;
                            }
                        case "FederalID":
                            {
                                strFederalID = colText;
                                strFederalIDData = cellText;
                                break;
                            }
                        case "IsRetired":
                            {
                                strRetired = colText;
                                if (strRetired == "0")
                                    isRetired = false;
                                else
                                    isRetired = true;
                                break;
                            }
                        case "JobNum":
                            {
                                strJobNum = colText;
                                strJobNumData = cellText;
                                break;
                            }
                        case "TitleDef":
                            {
                                strTitleDef = colText;
                                strTitleDefData = cellText;
                                break;
                            }
                        case "DateNeeded":
                            {
                                strDateNeeded = colText;
                                if (cellText != null && cellText != "")
                                    strDateNeededData = Convert.ToDateTime(cellText).ToString("MM/dd/yyyy");
                                break;
                            }
                        case "TimeNeeded":
                            {
                                strTimeNeeded = colText;
                                strTimeNeededData = cellText;
                                break;
                            }
                        case "DepositCheck":
                            {
                                strDepCheck = colText;
                                if (cellText == "0")
                                    isDepCheck = false;
                                else
                                    isDepCheck = true;
                                break;
                            }
                        case "PaymentCheck":
                            {
                                strPayCheck = colText;
                                if (cellText == "0")
                                    isPayCheck = false;
                                else
                                    isPayCheck = true;
                                break;
                            }
                        case "PettyCash":
                            {
                                strPettyCash = colText;
                                if (cellText == "0")
                                    isPettyCash = false;
                                else
                                    isPettyCash = true;
                                break;
                            }
                        case "CreditCard":
                            {
                                strCreditCard = colText;
                                if (cellText == "0")
                                    isCreditCard = false;
                                else
                                    isCreditCard = true;
                                break;
                            }
                        case "WillBill":
                            {
                                strWillBill = colText;
                                if (cellText == "0")
                                    isWillBill = false;
                                else
                                    isWillBill = true;
                                break;
                            }
                        case "AddBill":
                            {
                                strAddBill = colText;
                                if (cellText == "0")
                                    isAddBill = false;
                                else
                                    isAddBill = true;
                                break;
                            }
                    }
                }

                //To Display SentTo Details
                Chunk chunkColText = new Chunk(strSentTo + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase phraseColText = new Phrase(chunkColText);

                Chunk chunkRowText = new Chunk(strSentToData.Replace('~', '\n'), new Font(m_setFontName, m_FontSize));
                Phrase phraseRowText = new Phrase(chunkRowText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.AddCell(phraseColText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalInitPdfPTable.AddCell(phraseRowText);

                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.DefaultCell.Colspan = 2;
                journalInitPdfPTable.AddCell(spaceTable);
                journalInitPdfPTable.DefaultCell.Colspan = 1;

                //To Display Telephone Details
                chunkColText = new Chunk(strTelephone + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strTelephoneData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.AddCell(phraseColText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalInitPdfPTable.AddCell(phraseRowText);

                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.DefaultCell.Colspan = 2;
                journalInitPdfPTable.AddCell(spaceTable);
                journalInitPdfPTable.DefaultCell.Colspan = 1;

                //To Display PO Details
                chunkColText = new Chunk(strPOTo + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);

                chunkRowText = new Chunk(strPOToData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);

                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalInitPdfPTable.AddCell(phraseColText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalInitPdfPTable.AddCell(phraseRowText);

                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.DefaultCell.Colspan = 2;
                journalInitPdfPTable.AddCell(spaceTable);
                journalInitPdfPTable.DefaultCell.Colspan = 1;

                //To Display Contact Details
                chunkColText = new Chunk(strContact1 + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strContact1Data, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.AddCell(phraseColText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalInitPdfPTable.AddCell(phraseRowText);

                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.DefaultCell.Colspan = 2;
                journalInitPdfPTable.AddCell(spaceTable);
                journalInitPdfPTable.DefaultCell.Colspan = 1;

                //To Display retired Details
                chunkColText = new Chunk(strRetired + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                journalInitPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalInitPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalInitPdfPTable.AddCell(phraseColText);

                float retiredTblWidth = widthTotal / 4;
                float[] retiredTablColWidths = { retiredTblWidth / 4, retiredTblWidth / 4, retiredTblWidth / 4, retiredTblWidth / 4 };
                PdfPTable retiredPdfPTable = new PdfPTable(4);
                retiredPdfPTable.DefaultCell.Border = 0;
                retiredPdfPTable.TotalWidth = retiredTblWidth;
                retiredPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                retiredPdfPTable.DefaultCell.Padding = 0.95f;
                if (isRetired)
                    retiredPdfPTable.AddCell(checkImgPhrase);
                else
                    retiredPdfPTable.AddCell(unCheckImgPhrase);

                Chunk retTrueChunk = new Chunk("Yes", new Font(m_setFontName, m_FontSize));
                Phrase retTruePhrase = new Phrase(retTrueChunk);
                retiredPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                retiredPdfPTable.AddCell(retTruePhrase);

                retiredPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isRetired)
                    retiredPdfPTable.AddCell(unCheckImgPhrase);
                else
                    retiredPdfPTable.AddCell(checkImgPhrase);

                Chunk retFalseChunk = new Chunk("No", new Font(m_setFontName, m_FontSize));
                Phrase retFalsePhrase = new Phrase(retFalseChunk);
                retiredPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                retiredPdfPTable.AddCell(retFalsePhrase);

                journalInitPdfPTable.AddCell(retiredPdfPTable);

                //To Display DateNeeded Details
                chunkColText = new Chunk(strDateNeeded + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strDateNeededData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.AddCell(phraseColText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalSecPdfPTable.AddCell(phraseRowText);

                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.DefaultCell.Colspan = 2;
                journalSecPdfPTable.AddCell(spaceTable);
                journalSecPdfPTable.DefaultCell.Colspan = 1;

                //To Display TimeNeeded Details
                chunkColText = new Chunk(strTimeNeeded + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strTimeNeededData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.AddCell(phraseColText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalSecPdfPTable.AddCell(phraseRowText);

                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.DefaultCell.Colspan = 2;
                journalSecPdfPTable.AddCell(spaceTable);
                journalSecPdfPTable.DefaultCell.Colspan = 1;

                //To Display FederalID Details
                chunkColText = new Chunk(strFederalID + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strFederalIDData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.AddCell(phraseColText);
                journalSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalSecPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalSecPdfPTable.AddCell(phraseRowText);

                journalSecPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalSecPdfPTable.DefaultCell.Colspan = 2;
                journalSecPdfPTable.AddCell(spaceTable);
                journalSecPdfPTable.DefaultCell.Colspan = 1;

                //To Display Date Details
                chunkColText = new Chunk(strDate + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strDateData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.AddCell(phraseColText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalThirdPdfPTable.AddCell(phraseRowText);

                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.DefaultCell.Colspan = 2;
                journalThirdPdfPTable.AddCell(spaceTable);
                journalThirdPdfPTable.DefaultCell.Colspan = 1;

                //To Display JobNum Details
                chunkColText = new Chunk(strJobNum + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strJobNumData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.AddCell(phraseColText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalThirdPdfPTable.AddCell(phraseRowText);

                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.DefaultCell.Colspan = 2;
                journalThirdPdfPTable.AddCell(spaceTable);
                journalThirdPdfPTable.DefaultCell.Colspan = 1;

                //To Display TitleDef Details
                chunkColText = new Chunk(strTitleDef + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                phraseColText = new Phrase(chunkColText);
                chunkRowText = new Chunk(strTitleDefData, new Font(m_setFontName, m_FontSize));
                phraseRowText = new Phrase(chunkRowText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.AddCell(phraseColText);
                journalThirdPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                journalThirdPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                journalThirdPdfPTable.AddCell(phraseRowText);

                journalThirdPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                journalThirdPdfPTable.DefaultCell.Colspan = 2;
                journalThirdPdfPTable.AddCell(spaceTable);
                journalThirdPdfPTable.DefaultCell.Colspan = 1;

                //To Display Deposit Check
                chunkColText = new Chunk(strDepCheck, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isDepCheck)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);

                //To Display Payment Check
                chunkColText = new Chunk(strPayCheck, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isPayCheck)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);

                //To Display Petty Cash
                chunkColText = new Chunk(strPettyCash, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isPettyCash)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);

                //To Display Credit Card
                chunkColText = new Chunk(strCreditCard, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isCreditCard)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);

                //To Display Will Bill
                chunkColText = new Chunk(strWillBill, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isWillBill)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);

                //To Display Add Bill
                chunkColText = new Chunk(strAddBill, new Font(m_setFontName, m_FontSize));
                phraseColText = new Phrase(chunkColText);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                if (isAddBill)
                    chkBoxPdfPTable.AddCell(checkImgPhrase);
                else
                    chkBoxPdfPTable.AddCell(unCheckImgPhrase);
                chkBoxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                chkBoxPdfPTable.AddCell(phraseColText);
            }

            finalJnlPdfPTable.AddCell(journalInitPdfPTable);
            finalJnlPdfPTable.AddCell(new Paragraph(""));
            finalJnlSecPdfPTable.AddCell(journalSecPdfPTable);
            finalJnlSecPdfPTable.AddCell(journalThirdPdfPTable);

            //To display check related information
            m_FontSize = 6f;
            string info = "TO EXPEDITE PAYMENT INCLUDE PO & JOB NUMBERS ON INVOICE AND CORRESPONDENCE. PO'S MUST BE COMPLETELY WRITTEN OUT LISTING EACH ITEM ORDERED AND AND APPROXIMATE PRICE IF NOT THE EXACT AMOUNT ACCROSS FROM EACH ITEM.";
            Chunk infoChunk = new Chunk(info, new Font(m_setFontName, m_FontSize));
            Phrase infoPhrase = new Phrase(infoChunk);
            finalJnlSecPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            finalJnlSecPdfPTable.DefaultCell.Colspan = 2;
            finalJnlSecPdfPTable.AddCell(spaceTable);
            finalJnlSecPdfPTable.AddCell(spaceTable);
            finalJnlSecPdfPTable.AddCell(infoPhrase);
            finalJnlSecPdfPTable.DefaultCell.Colspan = 1;
            m_FontSize = 8.5f;

            finalJnlPdfPTable.AddCell(finalJnlSecPdfPTable);

            finalJnlPdfPTable.DefaultCell.Colspan = 3;
            finalJnlPdfPTable.AddCell(spaceTable);
            finalJnlPdfPTable.AddCell(chkBoxPdfPTable);
            finalJnlPdfPTable.AddCell(spaceTable);
            finalJnlPdfPTable.DefaultCell.Colspan = 1;

            pdfDocument.Add(finalJnlPdfPTable);
            pdfDocument.Add(new Paragraph(" "));

            //To Display Accounting Items Table
            DisplayAcctItems(pdfDocument, strAccountingItem, widthTotal);
            pdfDocument.Add(new Paragraph(" "));

            float[] amtPdfPTableColWidths = { 100, 45 };
            PdfPTable amtPdfPTable = new PdfPTable(2);
            amtPdfPTable.TotalWidth = widthTotal;
            amtPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
            amtPdfPTable.SetWidths(amtPdfPTableColWidths);
            amtPdfPTable.DefaultCell.Padding = 0.95f;
            amtPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;

            amtPdfPTable.AddCell("");

            float totalPdfPTableWidth = 45;
            float[] totalPdfPTblColWidth = { totalPdfPTableWidth / 2, totalPdfPTableWidth / 2 };
            PdfPTable totalPdfPTable = new PdfPTable(2);
            totalPdfPTable.TotalWidth = totalPdfPTableWidth;
            totalPdfPTable.WidthPercentage = totalPdfPTableWidth * m_WidthScaleFactor;
            totalPdfPTable.SetWidths(totalPdfPTblColWidth);
            totalPdfPTable.DefaultCell.Padding = 0.95f;
            totalPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;

            //To display SubTotal
            m_FontSize = 10;
            Chunk subTotalChunk = new Chunk("Sub Total: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase subTotalPhrase = new Phrase(subTotalChunk);
            totalPdfPTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
            totalPdfPTable.AddCell(subTotalPhrase);

            decimal totalAmt = 0;
            string strAmount = string.Empty;
            if (htParentTotal["Amount"] != null)
            {
                totalAmt = Convert.ToDecimal(htParentTotal["Amount"].ToString());
            }

            if (totalAmt < 0)
            {
                strAmount = "(" + ConvertToCurrencyFormat(Convert.ToString(totalAmt * (-1))) + ")";
            }
            else
            {
                strAmount = ConvertToCurrencyFormat(totalAmt.ToString());
            }

            Chunk subTotalAmtChunk = new Chunk(strAmount, new Font(m_setFontName, m_FontSize));
            totalPdfPTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            Phrase subTotalAmtPhrase = new Phrase(subTotalAmtChunk);
            totalPdfPTable.AddCell(subTotalAmtPhrase);

            amtPdfPTable.DefaultCell.Border = Rectangle.BOX;
            amtPdfPTable.DefaultCell.BorderWidth = 0.5f;
            amtPdfPTable.AddCell(totalPdfPTable);
            amtPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            amtPdfPTable.DefaultCell.Colspan = 2;
            amtPdfPTable.AddCell(new Paragraph(" "));
            amtPdfPTable.AddCell(new Paragraph(" "));
            amtPdfPTable.DefaultCell.Colspan = 1;

            string imgURL = ConfigurationManager.AppSettings["AttachmentsPath"] + "/" + HttpContext.Current.Session["CompanyEntityID"].ToString() + "/" + imgFileName;
            //string imgURL = string.Empty;
            Image imgAuthor = null;
            if (System.IO.File.Exists(imgURL))
            {
                imgAuthor = Image.GetInstance(new Uri(imgURL));
                imgAuthor.ScaleAbsolute(177, 61.92f);

                Chunk authorImgChunk = new Chunk(imgAuthor, 0, 0);
                Phrase authorImgPhrase = new Phrase(authorImgChunk);

                amtPdfPTable.AddCell(new Paragraph(""));
                amtPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                amtPdfPTable.AddCell(authorImgPhrase);
                amtPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            }
            else
            {
                amtPdfPTable.AddCell(new Paragraph(""));
                amtPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                amtPdfPTable.AddCell(new Phrase(""));
                amtPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            }

            amtPdfPTable.AddCell(new Paragraph(""));
            Chunk authorSignChunk = new Chunk(strApprovedByCaption + ": " + strApprovedBy, new Font(m_setFontName, m_FontSize));
            Phrase authorSignPhrase = new Phrase(authorSignChunk);
            amtPdfPTable.AddCell(authorSignPhrase);

            pdfDocument.Add(amtPdfPTable);
        }

        //Method to display the Accounting Item Table for ReportStyle300
        private void DisplayAcctItems(Document pdfDocument, string strAccountingItem, float widthTotal)
        {
            Image unCheckedImage = null;

            //string imgUrl = System.AppDomain.CurrentDomain.BaseDirectory + "\\App_Themes\\LAjit\\Images\\cross.gif";
            string imgUrl = m_strImagesCDNPath + "Images/cross.gif";
            unCheckedImage = Image.GetInstance(new Uri(imgUrl));
            unCheckedImage.ScaleAbsolute(10, 10);

            Chunk unCheckImgChunk = new Chunk(unCheckedImage, 0, 0, false);
            Phrase unCheckImgPhrase = new Phrase(unCheckImgChunk);

            Image checkedImage = null;
            imgUrl = m_strImagesCDNPath + "Images/tick.gif";
            checkedImage = Image.GetInstance(new Uri(imgUrl));
            checkedImage.ScaleAbsolute(10, 10);

            Chunk checkImgChunk = new Chunk(checkedImage, 0, 0, false);
            Phrase checkImgPhrase = new Phrase(checkImgChunk);

            XmlNode nodeFormControls = m_xDoc.SelectSingleNode("Root/bpeout/FormControls");
            XmlNodeList xNodelistFields = nodeFormControls.SelectNodes(strAccountingItem + "/GridHeading/Columns/Col");

            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = strAccountingItem;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;
            parentXStore = xmlStore;

            PdfPTable acctPdfPTable = new PdfPTable(numColumnsInXml);
            acctPdfPTable.TotalWidth = widthTotal;
            acctPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
            acctPdfPTable.DefaultCell.Border = Rectangle.BOX;
            acctPdfPTable.DefaultCell.BorderWidth = 0.5f;
            acctPdfPTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

            float[] acctColWidth = new float[numColumnsInXml];
            SetParentColWidths(xNodelistFields, xmlStore, numColumnsInXml, acctColWidth, widthTotal);
            acctPdfPTable.SetWidths(acctColWidth);

            m_FontSize = 8;
            //Display ColNames
            for (int col = 0; col < numColumnsInXml; col++)
            {
                string colName = xmlStore.Fields[col].caption;
                Chunk colChunk = new Chunk(colName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase colPhrase = new Phrase(colChunk);

                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                        {
                            acctPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            acctPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                }

                acctPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                acctPdfPTable.AddCell(colPhrase);
            }

            acctPdfPTable.DefaultCell.BackgroundColor = Color.WHITE;

            acctPdfPTable.HeaderRows = 1;

            if (numRecordsInXml > 0)
            {
                //Add Rows
                for (int row = 0; row < numRecordsInXml; row++)
                {
                    string[] rowData = xmlStore.GetRecord(row);

                    for (int col = 0; col < numColumnsInXml; col++)
                    {
                        int fullViewLength = 0;
                        string strControlType = string.Empty;
                        decimal tblAmount = 0;
                        string cellText = rowData[col];
                        bool isChecked = false;

                        foreach (XmlElement elem in xNodelistFields)
                        {
                            fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                            strControlType = elem.Attributes["ControlType"].Value;
                            acctPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                            if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                            {
                                if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                {
                                    if (cellText != null && cellText != "")
                                    {
                                        tblAmount = Convert.ToDecimal(cellText);

                                        if (tblAmount < 0)
                                        {
                                            cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(tblAmount * (-1))) + ")";
                                        }
                                        else
                                        {
                                            cellText = ConvertToCurrencyFormat(cellText);
                                        }
                                    }
                                    acctPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                    if (htParentTotal[elem.Attributes["Label"].Value] != null)
                                    {
                                        htParentTotal[elem.Attributes["Label"].Value] = Convert.ToDecimal(htParentTotal[elem.Attributes["Label"].Value]) + tblAmount;
                                    }
                                    else
                                    {
                                        htParentTotal[elem.Attributes["Label"].Value] = tblAmount;
                                    }
                                }
                                else
                                {
                                    //Customize if controlType is Calender
                                    if (strControlType == "Cal")
                                    {
                                        DateTime dateTime;
                                        DateTime.TryParse(cellText, out dateTime);
                                        if (dateTime != DateTime.MinValue)
                                        {
                                            dateTime = Convert.ToDateTime(cellText);
                                            cellText = dateTime.ToString("MM/dd/yy");
                                        }
                                    }
                                    else
                                    {
                                        if (strControlType == "Check")
                                        {
                                            if (cellText == "1")
                                                isChecked = true;
                                            else
                                                isChecked = false;
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        if (strControlType == "TBox")
                        {
                            if (cellText.Length > fullViewLength)
                            {
                                CharEnumerator charEnum = cellText.GetEnumerator();
                                int enumCount = 1;
                                while (charEnum.MoveNext())
                                {
                                    if (enumCount <= fullViewLength)
                                    {
                                        if (enumCount == 1)
                                        {
                                            cellText = charEnum.Current.ToString();
                                        }
                                        else
                                        {
                                            cellText = cellText + charEnum.Current.ToString();
                                        }
                                    }
                                    else
                                    {
                                        cellText = cellText + charEnum.Current.ToString() + "..";
                                        break;
                                    }

                                    enumCount++;
                                }
                            }
                        }

                        if (strControlType != "Check")
                        {
                            Chunk chunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase phrase = new Phrase(chunk);
                            acctPdfPTable.AddCell(phrase);
                        }
                        else
                        {
                            acctPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (isChecked)
                                acctPdfPTable.AddCell(checkImgPhrase);
                            else
                                acctPdfPTable.AddCell(unCheckImgPhrase);
                        }
                    }
                }
            }

            pdfDocument.Add(acctPdfPTable);
        }

        #region ReportStyle405 Methods
        //Method to generate Report for 405
        private void LoadDocument405(Document pdfDocument)
        {
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            int amtFieldCount = 0;

            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;

            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                break;
            }

            //If Report Style equals "405" set the ChildNodeName to BranchNodeName

            branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
            childTreeNode = branchNodeName;

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//GridHeading//Columns/Col");

            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
                xmlStore.ReportStyle = m_reportStyle;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlStore.LoadRecords();
                if (xmlStore.Fields != null)
                {
                    numColumnsInXml = xmlStore.Fields.Length;
                }
                else
                {
                    numColumnsInXml = 0;
                }
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    //Font for Report Styles '66' series
                    if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                    {
                        m_FontSize = 7.0f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    //Table to Load ParentColumnNames
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 2f;  //in Point

                    //Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //--- see if we have width data for the Fields in XmlStore
                    //int widthTotal = xmlStore.GetColumnWidthsTotal();

                    //Set TotalWidth Based on Report Styles
                    float widthTotal;
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;

                        if (m_reportStyle == "10")
                        {
                            widthTotal = 90;
                        }
                    }
                    else
                    {
                        widthTotal = 145;
                        if (m_reportStyle == "10")
                        {
                            widthTotal = 110;
                        }
                    }

                    this.pageWidth = widthTotal;

                    //Set ParentColWidths
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //Set the widthPercentage of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);

                    //Set Cell Attributes
                    parentPdfPTable.DefaultCell.BorderWidth = 1;
                    parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Table for creating Space between rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //To Display Parent Col Names
                    DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    //Add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        //If PageNumber changes display the columnHeader
                        if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                        {
                            if (childTreeNode == null || childTreeNode == "")
                            {
                                pdfDocument.Add(spaceTable);
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                pdfDocument.Add(parentPdfPTable);
                            }
                        }

                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = parentTreeNode;

                        //Table to load Rows of the Parent Table
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        parentRowPdfPTable.DefaultCell.Padding = 0.95f;
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        //Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.BorderWidth = 0;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        //Get the RowListFields of the treeNode from the XML
                        XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList/Rows");
                        string annotContent = string.Empty;
                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            Image checkedImage = null;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            if (col == 0)
                            {
                                strRevenue = rowData[col];
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(elem.Attributes["FullViewLength"].Value);

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    controlType = elem.Attributes["ControlType"].Value;

                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        if (cellText != null && cellText.Trim() != "")
                                        {
                                            parentTblAmount = Convert.ToDecimal(cellText);

                                            if (parentTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }
                                        }

                                        //Align Amount Data to the Right
                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        amtFieldCount++;
                                    }
                                    else
                                    {
                                        //Customize if controlType is Calender
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Customize if controlType is Check
                                            if (elem.Attributes["ControlType"].Value == "Check")
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null && cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                            }
                                        }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        if (xRowlistFields[row].Attributes["Notes"] != null && xRowlistFields[row].Attributes["Notes"].Value != "")
                                        {
                                            annotContent = "Notes: " + xRowlistFields[row].Attributes["Notes"].Value;
                                        }

                                        if (xRowlistFields[row].Attributes["Attachments"] != null && xRowlistFields[row].Attributes["Attachments"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["SecuredBy"] != null && xRowlistFields[row].Attributes["SecuredBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["ChangedBy"] != null && xRowlistFields[row].Attributes["ChangedBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["ToolTip"] != null && xRowlistFields[row].Attributes["ToolTip"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }

                            Chunk parentCellChunk;

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            StringBuilder strCellTextBuilder = new StringBuilder();
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            //Add Image for CheckBox if bool value is true 

                            if (m_IsCheckBox)
                            {
                                parentCellChunk = new Chunk(checkedImage, 5, 5, false);
                            }
                            else
                            {
                                parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            }

                            m_IsCheckBox = false;
                            parentRowPdfPTable.DefaultCell.Border = 0;

                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //Add Annotations to the First column of EveryRow
                            if (m_ShowAnnotations)
                            {
                                if (annotContent != "")
                                {
                                    annotContent = annotContent.Replace("~", "\n\t\t");
                                    string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                    Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                    annotImage.Annotation = new Annotation("Annotation", annotContent);

                                    Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                    Phrase imagePhrase = new Phrase(imageChunk);

                                    if (col == 0)
                                    {
                                        parentCellPhrase.Add(imagePhrase);
                                        annotContent = "";
                                    }
                                }
                            }

                            parentRowPdfPTable.AddCell(parentCellPhrase);
                        }

                        pdfDocument.Add(parentRowPdfPTable);
                    }

                    pdfDocument.Add(new Paragraph(" "));
                    pdfDocument.Add(new Paragraph(" "));
                    LoadChildDoc405(pdfDocument);

                    // let the caller know we successfully reached 'the end' of this 
                    //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                }
            }
        }

        //Method to Display a Parent and Corresponding Childs for 405
        private void LoadChildDoc405(Document pdfDocument)
        {
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            int amtFieldCount = 0;

            string parentTreeNode = string.Empty;
            string childTreeNode = string.Empty;
            string branchNodeName = string.Empty;

            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                parentTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                break;
            }

            //If Report Style equals "405" set the ChildNodeName to BranchNodeName

            branchNodeName = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node").InnerText.ToString();
            childTreeNode = branchNodeName;

            this.m_TreeNodeName = parentTreeNode;

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//GridHeading//Columns/Col");

            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
                xmlStore.ReportStyle = m_reportStyle;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlStore.LoadRecords();
                if (xmlStore.Fields != null)
                {
                    numColumnsInXml = xmlStore.Fields.Length;
                }
                else
                {
                    numColumnsInXml = 0;
                }
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    //Font for Report Styles '66' series
                    if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                    {
                        m_FontSize = 7.0f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    //Table to Load ParentColumnNames
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 2f;  //in Point

                    //Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //--- see if we have width data for the Fields in XmlStore
                    //int widthTotal = xmlStore.GetColumnWidthsTotal();

                    //Set TotalWidth Based on Report Styles
                    float widthTotal;
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;

                        if (m_reportStyle == "10")
                        {
                            widthTotal = 90;
                        }
                    }
                    else
                    {
                        widthTotal = 145;
                        if (m_reportStyle == "10")
                        {
                            widthTotal = 110;
                        }
                    }

                    this.pageWidth = widthTotal;

                    //Set ParentColWidths
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //Set the widthPercentage of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);

                    //Set Cell Attributes
                    parentPdfPTable.DefaultCell.BorderWidth = 1;
                    parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Table for creating Space between rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //Table to be used for Report Styles 603,604
                    PdfPTable columnTypeTable;

                    columnTypeTable = new PdfPTable(numColumnsInXml);
                    columnTypeTable.SetWidths(columnWidthInPct);

                    columnTypeTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    columnTypeTable.TotalWidth = parentPdfPTable.TotalWidth;
                    columnTypeTable.DefaultCell.Border = 0;
                    columnTypeTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                    columnTypeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //To Display Parent Col Names
                    DispColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    //Add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        if (row > 0)
                        {
                            pdfDocument.Add(spaceTable);
                            pdfDocument.Add(spaceTable);
                            pdfDocument.Add(spaceTable);

                            //when row changes add the columnHeader Table
                            pdfDocument.Add(parentPdfPTable);
                        }

                        //If PageNumber changes display the columnHeader
                        if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                        {
                            if (childTreeNode == null || childTreeNode == "")
                            {
                                pdfDocument.Add(columnTypeTable);
                                pdfDocument.Add(spaceTable);
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                pdfDocument.Add(parentPdfPTable);
                            }
                        }

                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = parentTreeNode;

                        //Table to load Rows of the Parent Table
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        parentRowPdfPTable.DefaultCell.Padding = 2f;
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        //Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.BorderWidth = 0;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        //Get the RowListFields of the treeNode from the XML
                        XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList/Rows");
                        string annotContent = string.Empty;
                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            Image checkedImage = null;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(elem.Attributes["FullViewLength"].Value);

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    controlType = elem.Attributes["ControlType"].Value;

                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        if (cellText != null && cellText.Trim() != "")
                                        {
                                            parentTblAmount = Convert.ToDecimal(cellText);

                                            if (parentTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }
                                        }

                                        //Align Amount Data to the Right
                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        amtFieldCount++;
                                    }
                                    else
                                    {
                                        //Customize if controlType is Calender
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Customize if controlType is Check
                                            if (elem.Attributes["ControlType"].Value == "Check")
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null && cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                            }
                                        }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        annotContent = GetToolTipContent(xRowlistFields[row]);
                                    }

                                    break;
                                }
                            }

                            Chunk parentCellChunk;

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            StringBuilder strCellTextBuilder = new StringBuilder();
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            //Add Image for CheckBox if bool value is true 

                            if (m_IsCheckBox)
                            {
                                parentCellChunk = new Chunk(checkedImage, 5, 5, false);
                            }
                            else
                            {
                                parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            }

                            m_IsCheckBox = false;
                            parentRowPdfPTable.DefaultCell.Border = 0;

                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //Add Annotations to the First column of EveryRow
                            if (m_ShowAnnotations)
                            {
                                if (annotContent != "")
                                {
                                    annotContent = annotContent.Replace("~", "\n\t\t");
                                    string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                    Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                    annotImage.Annotation = new Annotation("Annotation", annotContent);

                                    Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                    Phrase imagePhrase = new Phrase(imageChunk);

                                    if (col == 0)
                                    {
                                        parentCellPhrase.Add(imagePhrase);
                                        annotContent = "";
                                    }
                                }
                            }

                            if (col == 0)
                            {
                                parentRowPdfPTable.DefaultCell.Colspan = 2;
                            }
                            else
                            {
                                if (col > 1)
                                {
                                    parentCellChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                                    parentCellPhrase = new Phrase(parentCellChunk);
                                }

                                parentRowPdfPTable.AddCell(parentCellPhrase);

                                if (col > 0)
                                {
                                    parentRowPdfPTable.DefaultCell.Colspan = 1;
                                }
                            }

                            parentName = "";
                        }

                        pdfDocument.Add(parentRowPdfPTable);

                        //InsertChildTable
                        int Link1 = 0;

                        //Per iteration extract the Link1 value from each row 
                        XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList//Rows");

                        //Extract TrxID values
                        Link1 = Convert.ToInt32(parentRowsList[row].Attributes["TrxID"].Value);

                        //Customize data for '65' Series
                        float childTableWidth = 0;
                        if (childTreeNode != "" && Link1 != 0)
                        {
                            childTableWidth = InsertChildTable(parentTreeNode, childTreeNode, "", "", "", "", numColumnsInXml, 0, Link1, row, columnTypeTable);
                        }

                        pdfDocument.Add(new Paragraph(""));
                    }
                }
            }
        }

        private string GetToolTipContent(XmlNode nodeRow)
        {
            StringBuilder sbTip = new StringBuilder();
            XmlAttribute attTip = nodeRow.Attributes["Notes"];
            if (attTip != null && attTip.Value != "")
            {
                sbTip.Append("Notes: " + attTip.Value + "\n\n");
            }

            attTip = nodeRow.Attributes["Attachments"];
            if (attTip != null && attTip.Value != "")
            {
                sbTip.Append("Attachments: " + attTip.Value + "\n\n");
            }

            attTip = nodeRow.Attributes["SecuredBy"];
            if (attTip != null && attTip.Value != "")
            {
                sbTip.Append("SecuredBy: " + attTip.Value + "\n\n");
            }

            attTip = nodeRow.Attributes["ChangedBy"];
            if (attTip != null && attTip.Value != "")
            {
                sbTip.Append("ChangedBy: " + attTip.Value + "\n\n");
            }

            attTip = nodeRow.Attributes["ToolTip"];
            if (attTip != null && attTip.Value != "")
            {
                sbTip.Append("ToolTip: " + attTip.Value + "\n\n");
            }
            return sbTip.ToString();
        }
        #endregion

        #region ReportStyle400 Methods
        //Method to Generate Report for 400
        private bool LoadDocument400(Document pdfDocument)
        //private bool LoadDocument400(Document pdfDocument, DataSet dsAll)
        {
            bool bRet = false;
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            int treeNodesCount = 0;

            string firstTreeNode = string.Empty;
            string secondTreeNode = string.Empty;
            string thirdTreeNode = string.Empty;
            string fourthTreeNode = string.Empty;
            string trxInfoTreeNode = string.Empty;
            string invoiceInfoTreeNode = string.Empty;
            string strAgency = string.Empty;

            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            foreach (XmlNode treeNode in nodeParents.ChildNodes)
            {
                switch (treeNodesCount)
                {
                    case 0:
                        {
                            firstTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 1:
                        {
                            secondTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 2:
                        {
                            thirdTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 3:
                        {
                            fourthTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 4:
                        {
                            trxInfoTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                    case 5:
                        {
                            invoiceInfoTreeNode = treeNode.SelectSingleNode("Node").InnerText.ToString();
                            break;
                        }
                }

                treeNodesCount++;
            }

            m_TreeNodeName = firstTreeNode;

            //Get the RowList
            XmlNodeList xInitRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//RowList/Rows");

            int rowCount = 0;
            foreach (XmlElement rowElem in xInitRowlistFields)
            {
                //Get the TrxID
                int trxID = 0;
                if (rowElem.Attributes["TrxID"] != null)
                {
                    trxID = Convert.ToInt32(rowElem.Attributes["TrxID"].Value);
                }

                //Get the Column List
                XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + firstTreeNode + "//GridHeading//Columns/Col");
                //strAgency = DisplayJobDetails(pdfDocument, firstTreeNode, xNodelistFields, rowCount);
                strAgency = DisplayJobDetails(pdfDocument, firstTreeNode, rowCount, false);

                if (rowCount > 0)
                {
                    pdfDocument.NewPage();
                }

                xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + secondTreeNode + "//GridHeading//Columns/Col");
                //InsertBudgetDetails
                int numofBudgetRecords = LoadBudgetDetails(pdfDocument, ref bRet, ref numRecordsInXml, ref numColumnsInXml, secondTreeNode, xNodelistFields, trxID);
                pdfDocument.Add(new Paragraph(" "));

                if (numofBudgetRecords > 0)
                {
                    xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + thirdTreeNode + "//GridHeading//Columns/Col");
                    //To Load the JobInfo Details
                    LoadJobInfo(pdfDocument, ref bRet, ref numRecordsInXml, ref numColumnsInXml, thirdTreeNode, strAgency, xNodelistFields, trxID);
                    pdfDocument.NewPage();

                    if (invoiceInfoTreeNode != "")
                    {
                        xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + invoiceInfoTreeNode + "//GridHeading//Columns/Col");
                        XmlNodeList xInvRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + invoiceInfoTreeNode + "//RowList/Rows");

                        //To Load the InvoiceInfo Details
                        if (xInvRowlistFields.Count > 1)
                        {
                            LoadInvoiceInfoDetails(pdfDocument, ref bRet, ref numRecordsInXml, ref numColumnsInXml, invoiceInfoTreeNode, xNodelistFields, trxID);
                            pdfDocument.NewPage();
                        }
                    }

                    xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + fourthTreeNode + "//GridHeading//Columns/Col");
                    //To Load the BudgetTotal Details
                    LoadBudgetTotalDetails(pdfDocument, ref bRet, ref numRecordsInXml, ref numColumnsInXml, fourthTreeNode, trxInfoTreeNode, xNodelistFields, trxID);
                    //LoadBgtTotalDetails(pdfDocument, ref bRet, ref numRecordsInXml, ref numColumnsInXml, fourthTreeNode, trxInfoTreeNode, xNodelistFields, trxID, dsAll);
                }
                rowCount++;
            }

            if (bRet == false)
            {
                pdfDocument.Add(new Paragraph("Failed to load data"));
            }

            return bRet;
        }
        #region Commented DisplayJobDetails
        //private string DisplayJobDetails(Document pdfDocument, string firstTreeNode, XmlNodeList xNodelistFields, int row)
        //{
        //    string strAgency = string.Empty;
        //    int numRecordsInXml;
        //    int numColumnsInXml;

        //    float widthTotal;

        //    if (m_ShowLandscape)
        //        widthTotal = 125;
        //    else
        //        widthTotal = 145;

        //    if (m_strXmlDoc.Length > 0)
        //    {
        //        //--- create an instance of XmlStore
        //        ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
        //        xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = firstTreeNode;
        //        xmlStore.ReportStyle = m_reportStyle;
        //        numRecordsInXml = xmlStore.LoadRecords();
        //        numColumnsInXml = xmlStore.Fields.Length;
        //        parentXStore = xmlStore;

        //        if (numRecordsInXml > 0 && numColumnsInXml > 0)
        //        {
        //            int numColumnsInPDF = numColumnsInXml;

        //            PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
        //            parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

        //            // Set Column Widths
        //            float[] columnWidthInPct = new float[numColumnsInPDF];

        //            //Set Col Widths
        //            SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal,xmlStore);

        //            if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
        //                parentPdfPTable.WidthPercentage = 100;
        //            else
        //                parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

        //            parentPdfPTable.TotalWidth = widthTotal;
        //            parentPdfPTable.SetWidths(columnWidthInPct);

        //            m_isJobTypeTable = true;
        //            DisplayJobColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

        //            PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
        //            spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
        //            spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
        //            spaceTable.DefaultCell.GrayFill = 1.0f;
        //            spaceTable.DefaultCell.Border = 0;
        //            spaceTable.DefaultCell.Colspan = numColumnsInXml;

        //            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
        //            Phrase spacePhrase = new Phrase(spaceChunk);
        //            spaceTable.AddCell(spacePhrase);

        //            //add the rows of Parent
        //            subChdColHdrCount = 0;

        //            PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
        //            if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
        //                parentRowPdfPTable.WidthPercentage = 100;
        //            else
        //                parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

        //            parentRowPdfPTable.TotalWidth = widthTotal;
        //            parentRowPdfPTable.SetWidths(columnWidthInPct);

        //            // Set Column Header Cell Attributes

        //            parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

        //            XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + firstTreeNode + "//RowList/Rows");
        //            strAgency = xRowlistFields[row].Attributes["AgencyName"].Value;
        //            string strDescription = string.Empty;

        //            string[] rowData = xmlStore.GetRecord(row);

        //            for (int col = 0; col < numColumnsInXml; col++)
        //            {
        //                decimal parentTblAmount = 0;
        //                string cellText = rowData[col];

        //                foreach (XmlElement elem in xNodelistFields)
        //                {
        //                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
        //                    {
        //                        if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
        //                        {
        //                            if (cellText != null && cellText.Trim() != "")
        //                            {
        //                                parentTblAmount = Convert.ToDecimal(cellText);

        //                                if (parentTblAmount < 0)
        //                                {
        //                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
        //                                }
        //                                else
        //                                {
        //                                    cellText = ConvertToCurrencyFormat(cellText);
        //                                }
        //                            }

        //                            parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        }
        //                        else
        //                        {
        //                            if (elem.Attributes["ControlType"].Value == "Cal")
        //                            {
        //if (cellText != null && cellText != "")
        //{
        //      DateTime dateTime;
        //    DateTime.TryParse(cellText, out dateTime);
        //    if (dateTime != DateTime.MinValue)
        //    {
        //        dateTime = Convert.ToDateTime(cellText);
        //        cellText = dateTime.ToString("MM/dd/yy");
        //    }
        //}
        //                            }
        //                        }

        //                        break;
        //                    }
        //                }

        //                Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
        //                Phrase parentCellPhrase = new Phrase(parentCellChunk);

        //                parentRowPdfPTable.DefaultCell.Border = Rectangle.TOP_BORDER;
        //                parentRowPdfPTable.DefaultCell.Border = 0;
        //                parentRowPdfPTable.DefaultCell.BackgroundColor = WHEAT;
        //                parentRowPdfPTable.AddCell(parentCellPhrase);
        //                parentName = cellText;
        //            }

        //            m_jobTypeTable = parentRowPdfPTable;

        //            //pdfDocument.Add(parentRowPdfPTable);
        //            //pdfDocument.Add(new Paragraph(""));
        //        }
        //    }
        //    return strAgency;
        //}
        #endregion

        //Method To Load the Job Details
        private string DisplayJobDetails(Document pdfDocument, string firstTreeNode, int row, bool isInitialCall)
        {
            string strAgency = string.Empty;
            Hashtable htControlType = new Hashtable();

            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + firstTreeNode + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = firstTreeNode;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //Increase Font Size based on column Count
            if (numColumnsInXml > 10)
            {
                m_FontSize = 6f;
            }
            else
            {
                m_FontSize = 7f;
            }

            //To Load Corresponding Control Types of the Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            //PdfPTable jobPdfPTable = new PdfPTable(numColumnsInXml);

            //Table to load Job Specific Details(Table that has to kept in the header and the this table has to be changed based if Job Changes)
            PdfPTable jobPdfPTable = new PdfPTable(1);
            jobPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
            float[] columnWidthInPct = new float[numColumnsInXml];
            float widthTotal;

            //Change TotalWidth based on Landscape Value
            if (m_ShowLandscape)
            {
                widthTotal = 125;
            }
            else
            {
                widthTotal = 145;
            }

            this.pageWidth = widthTotal;

            //If row changes clear the table
            if (row > 0)
            {
                m_RequestTable.Rows.RemoveRange(0, 4);
            }

            //Get the corresponding RowData from the xmlStore 
            string[] rowData = xmlStore.GetRecord(row);

            //Load Records column by column
            for (int col = 0; col < numColumnsInXml; col++)
            {
                string colName = xmlStore.Fields[col].caption;
                string cellText = rowData[col];

                if (cellText != null)
                {
                    if (xmlStore.Fields[col].label.ToString() == "AgencyName")
                    {
                        strAgency = cellText;
                    }

                    if (htControlType[xmlStore.Fields[col].label] != null && xmlStore.Fields[col].label != "JobNum"
                        && xmlStore.Fields[col].label != "Contact1" && xmlStore.Fields[col].label != "AgencyName" && xmlStore.Fields[col].label != "Telephone")
                    {
                        if (htControlType[xmlStore.Fields[col].label].ToString() == "Amount")
                        {
                            decimal colAmount = Convert.ToDecimal(cellText);

                            if (colAmount < 0)
                            {
                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colAmount * (-1))) + ")";
                            }
                            else
                            {
                                cellText = ConvertToCurrencyFormat(cellText);
                            }
                        }
                        else
                        {
                            if (htControlType[xmlStore.Fields[col].label].ToString() == "Cal")
                            {
                                if (cellText != null && cellText != "")
                                {
                                    DateTime dateTime;
                                    DateTime.TryParse(cellText, out dateTime);
                                    if (dateTime != DateTime.MinValue)
                                    {
                                        dateTime = Convert.ToDateTime(cellText);
                                        cellText = dateTime.ToString("MM/dd/yy");
                                    }
                                }
                            }
                        }

                        jobPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        Chunk colChunk = new Chunk(colName + ": " + cellText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase colPhrase = new Phrase(colChunk);
                        jobPdfPTable.DefaultCell.Border = 0;
                        jobPdfPTable.AddCell(colPhrase);


                        if (isInitialCall)
                        {
                            m_RequestTable.AddCell(colPhrase);
                        }

                        if (row > 0)
                        {
                            m_RequestTable.AddCell(colPhrase);
                        }
                    }
                }
            }

            //pdfDocument.Add(jobPdfPTable);
            return strAgency;
        }

        //Method To Load the Budget Details
        private int LoadBudgetDetails(Document pdfDocument, ref bool bRet, ref int numRecordsInXml, ref int numColumnsInXml, string secondTreeNode, XmlNodeList xNodelistFields, int trxID)
        {
            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, secondTreeNode, trxID);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = secondTreeNode;
                xmlStore.ReportStyle = m_reportStyle;
                xmlStore.IsParent = true;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlStore.LoadRecords();
                numColumnsInXml = xmlStore.Fields.Length;
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    int numColumnsInPDF = numColumnsInXml;
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                    // Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    float widthTotal;

                    //Change TotalWidth based on Landscape Value
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    //Set Col Widths
                    SetBudgetDetailsColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);

                    //Set Cell Attributes
                    parentPdfPTable.DefaultCell.BorderWidth = 1;
                    parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Table to insert space between the rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //To Display Parent Col Names
                    DisplayBudgetDetailsColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    //XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + secondTreeNode + "//RowList/Rows");
                    XmlNodeList xRowlistFields = xmlStore.xStoreRowList.ChildNodes;

                    //add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = secondTreeNode;

                        //Table to insert Row Data 
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        // Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.BorderWidth = 1;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        parentRowPdfPTable.DefaultCell.Indent = 1;

                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        string annotContent = string.Empty;
                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);
                        string strGroupValue = xRowlistFields[row].Attributes["AICPGroup"].Value.TrimEnd();

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            Image checkedImage = null;

                            if (col == 0)
                            {
                                strRevenue = rowData[col];
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    switch (elem.Attributes["ControlType"].Value)
                                    {
                                        case "Amount":
                                            {
                                                if (cellText != null && cellText.Trim() != "")
                                                {
                                                    parentTblAmount = Convert.ToDecimal(cellText);

                                                    if (parentTblAmount < 0)
                                                    {
                                                        cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        cellText = ConvertToCurrencyFormat(cellText);
                                                    }

                                                    if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    else
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                    }
                                                }

                                                //Align Amount Data to the Right
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                break;
                                            }
                                        //Customize if controlType is Calender
                                        case "Cal":
                                            {
                                                if (cellText != null && cellText != "")
                                                {
                                                    DateTime dateTime;
                                                    DateTime.TryParse(cellText, out dateTime);
                                                    //dateTime = Convert.ToDateTime(cellText);
                                                    if (dateTime != DateTime.MinValue)
                                                    {
                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                    }
                                                }
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                                break;
                                            }
                                        //Customize if controlType is Check
                                        case "Check":
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null || cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                break;
                                            }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        if (xRowlistFields[row].Attributes["Notes"] != null && xRowlistFields[row].Attributes["Notes"].Value != "")
                                        {
                                            annotContent = "Notes: " + xRowlistFields[row].Attributes["Notes"].Value;
                                        }

                                        if (xRowlistFields[row].Attributes["Attachments"] != null && xRowlistFields[row].Attributes["Attachments"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["SecuredBy"] != null && xRowlistFields[row].Attributes["SecuredBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["ChangedBy"] != null && xRowlistFields[row].Attributes["ChangedBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                        }

                                        //if (xRowlistFields[row].Attributes["ToolTip"] != null && xRowlistFields[row].Attributes["ToolTip"].Value != "")
                                        //{
                                        //    if (annotContent != "")
                                        //    {
                                        //        annotContent = annotContent + "\n\n" + "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //    else
                                        //    {
                                        //        annotContent = "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //}
                                    }
                                    break;
                                }
                            }

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));

                            if (strGroupValue == "Sub-Total A to K" || strGroupValue == "Sub-Total Direct Costs"
                                || strGroupValue == "Talent Costs & Expenses" || strGroupValue == "Editorial and Finishing"
                                || strGroupValue == "Grand Total")
                            {
                                parentCellChunk.Font = new Font(m_setFontName, m_FontSize + 0.5f, Font.BOLD);
                            }

                            parentRowPdfPTable.DefaultCell.Border = 0;

                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //Add Annotations to the First column of EveryRow
                            if (m_ShowAnnotations)
                            {
                                if (annotContent != "")
                                {
                                    annotContent = annotContent.Replace("~", "\n\t\t");
                                    string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                    Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                    annotImage.Annotation = new Annotation("Annotation", annotContent);

                                    Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                    Phrase imagePhrase = new Phrase(imageChunk);

                                    if (col == 0)
                                    {
                                        parentCellPhrase.Add(imagePhrase);
                                        annotContent = "";
                                    }
                                }

                                if (row % 2 == 0)
                                {
                                    parentRowPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                                }
                                else
                                {
                                    parentRowPdfPTable.DefaultCell.BackgroundColor = Color.WHITE;
                                }

                                parentRowPdfPTable.AddCell(parentCellPhrase);
                            }

                            parentName = cellText;
                        }

                        pdfDocument.Add(parentRowPdfPTable);
                    }

                    // let the caller know we successfully reached 'the end' of this 
                    //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                    bRet = true;
                }
            }
            return numRecordsInXml;
        }

        //Method To Load the JobInfo Details
        private void LoadJobInfo(Document pdfDocument, ref bool bRet, ref int numRecordsInXml, ref int numColumnsInXml, string thirdTreeNode, string strAgency, XmlNodeList xNodelistFields, int trxID)
        {
            PdfPTable jobTable = new PdfPTable(2);
            PdfPTable jobInfoTable = new PdfPTable(1);

            //Change WidthPercentage based on LandScape Value
            if (m_ShowLandscape)
            {
                jobTable.WidthPercentage = 128;
            }
            else
            {
                jobTable.WidthPercentage = 148;
            }

            //Set WidthTotal
            float widthTotal;

            if (m_ShowLandscape)
                widthTotal = 125;
            else
                widthTotal = 145;

            float jobInfoTblWidth = widthTotal - 32;
            float[] widths = { jobInfoTblWidth, 32 };
            float[] jobInfoWidth = { jobInfoTblWidth };

            jobTable.SetWidths(widths);
            jobTable.TotalWidth = widthTotal;
            jobTable.DefaultCell.Border = 0;

            jobInfoTable.DefaultCell.Border = 0;
            jobInfoTable.DefaultCell.BackgroundColor = WHEAT;

            jobInfoTable.SetWidths(jobInfoWidth);

            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, thirdTreeNode, trxID);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = thirdTreeNode;
                xmlStore.ReportStyle = m_reportStyle;
                xmlStore.IsParent = true;

                //To Load Records and to get Row and Column Counts
                numRecordsInXml = xmlStore.LoadRecords();
                numColumnsInXml = xmlStore.Fields.Length;
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Set Font Size based on number of Columns
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    //PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    //parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                    // Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //Set Col Widths
                    this.pageWidth = jobInfoTblWidth;
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, jobInfoTblWidth);

                    //DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = jobTable.WidthPercentage;
                    spaceTable.TotalWidth = jobTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        subChdColHdrCount = 0;

                        //Table to Load rowData
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = (jobInfoTblWidth) * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = jobInfoTblWidth;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        //Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + thirdTreeNode + "//RowList/Rows");
                        string strDescription = string.Empty;

                        //get RowData from the XmlStore
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records Column by Column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        if (cellText != null && cellText.Trim() != "")
                                        {
                                            parentTblAmount = Convert.ToDecimal(cellText);

                                            if (parentTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }
                                        }

                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                    else
                                    {
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }

                            if (controlType == "TBox")
                            {
                                //if (cellText.Length > 0)
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            parentRowPdfPTable.DefaultCell.Border = 0;

                            //Customize the alignment of Data
                            if (col != 0)
                            {
                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                            }
                            else
                            {
                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            }

                            if (row == numRecordsInXml - 1)
                            {
                                if (col == 0)
                                {
                                    parentRowPdfPTable.DefaultCell.Colspan = 2;
                                    parentRowPdfPTable.AddCell(parentCellPhrase);
                                    parentRowPdfPTable.DefaultCell.Colspan = 1;
                                }
                                else
                                {
                                    parentRowPdfPTable.AddCell(parentCellPhrase);
                                }
                            }
                            else
                            {
                                parentRowPdfPTable.AddCell(parentCellPhrase);
                            }
                            parentName = cellText;
                        }

                        parentRowPdfPTable.AddCell(spaceTable);
                        jobInfoTable.AddCell(parentRowPdfPTable);
                        //pdfDocument.Add(parentRowPdfPTable);
                        //pdfDocument.Add(new Paragraph(""));
                    }

                    //Add Reconciliation text to JobTable
                    PdfPTable reconcilTable = new PdfPTable(1);
                    Chunk reconcilChunk = new Chunk("Reconciliation", new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase reconcilPhrase = new Phrase(reconcilChunk);
                    reconcilTable.DefaultCell.BackgroundColor = THISTLE;
                    reconcilTable.DefaultCell.Border = 0;
                    reconcilTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    reconcilTable.AddCell(reconcilPhrase);
                    jobTable.AddCell(reconcilTable);

                    //Add Agency text to JobTable
                    PdfPTable agencyTable = new PdfPTable(1);
                    Chunk agencyChunk = new Chunk("Agency", new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase agencyPhrase = new Phrase(agencyChunk);
                    agencyTable.DefaultCell.BackgroundColor = THISTLE;
                    agencyTable.DefaultCell.Border = 0;
                    agencyTable.DefaultCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    agencyTable.AddCell(agencyPhrase);
                    jobTable.AddCell(agencyTable);

                    //Add JobInfoTable to JobTable
                    jobTable.DefaultCell.BackgroundColor = Color.WHITE;
                    jobTable.AddCell(jobInfoTable);

                    //Add Agency Name to the JobTable
                    PdfPTable agencyNameTable = new PdfPTable(1);
                    if (strAgency.Length > 32)
                    {
                        CharEnumerator charEnum = strAgency.GetEnumerator();
                        int enumCount = 1;
                        while (charEnum.MoveNext())
                        {
                            if (enumCount <= 32)
                            {
                                if (enumCount == 1)
                                {
                                    strAgency = charEnum.Current.ToString();
                                }
                                else
                                {
                                    strAgency = strAgency + charEnum.Current.ToString();
                                }
                            }
                            else
                            {
                                strAgency = strAgency + charEnum.Current.ToString() + "..";
                                break;
                            }

                            enumCount++;
                        }
                    }
                    Chunk agencyNameChunk = new Chunk(strAgency, new Font(m_setFontName, m_FontSize));
                    Phrase agencyNamePhrase = new Phrase(agencyNameChunk);
                    agencyNameTable.DefaultCell.Border = 0;
                    agencyNameTable.DefaultCell.BackgroundColor = WHEAT;
                    agencyNameTable.AddCell(agencyNamePhrase);

                    jobTable.AddCell(agencyNameTable);

                    jobTable.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    pdfDocument.Add(jobTable);

                    bRet = true;
                }
            }
        }

        //Method To Load the InvoiceInfo Details
        private void LoadInvoiceInfoDetails(Document pdfDocument, ref bool bRet, ref int numRecordsInXml, ref int numColumnsInXml, string secondTreeNode, XmlNodeList xNodelistFields, int trxID)
        {
            htParentTotal.Clear();

            //Calculating subtotals
            Hashtable htReceivables = new Hashtable();
            Hashtable htPayroll = new Hashtable();

            if (m_strXmlDoc.Length > 0)
            {
                ArrayList arrTransList = new ArrayList();
                XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + secondTreeNode + "//RowList/Rows");
                XmlDocument xChildDoc = new XmlDocument();

                //Method to Get a RowList which is sorted based on TypeOfTrans
                GetSortedInvInfo(secondTreeNode, arrTransList, xRowlistFields, xChildDoc);
                xRowlistFields = xChildDoc.SelectNodes("//Root//bpeout//FormControls//" + secondTreeNode + "//RowList/Rows");

                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(xChildDoc, secondTreeNode, trxID);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = secondTreeNode;
                xmlStore.ReportStyle = m_reportStyle;
                xmlStore.IsParent = true;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlStore.LoadRecords();
                numColumnsInXml = xmlStore.Fields.Length;
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    // Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //--- see if we have width data for the Fields in XmlStore
                    //int widthTotal = xmlStore.GetColumnWidthsTotal();

                    //Set TotalWidth
                    float widthTotal;
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    //Set ColWidths
                    SetInvoiceInfoColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //Table to Load Parent Col Names
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 1f;  //in Point

                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);
                    parentPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                    //Table to insert spacing between the rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    string strInitTransType = xRowlistFields[0].Attributes["TypeOfTrans"].Value.ToString();

                    PdfPTable transPdfPTable = new PdfPTable(numColumnsInXml);
                    transPdfPTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    transPdfPTable.TotalWidth = parentPdfPTable.TotalWidth;
                    transPdfPTable.DefaultCell.GrayFill = 1.0f;
                    //transPdfPTable.DefaultCell.Border = 0;
                    transPdfPTable.DefaultCell.Colspan = numColumnsInXml;
                    transPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    transPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                    transPdfPTable.DefaultCell.BorderWidthBottom = 0.9f;
                   // transPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                    Chunk transChunk = new Chunk(strInitTransType, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase transPhrase = new Phrase(transChunk);
                    transPdfPTable.AddCell(transPhrase);

                    pdfDocument.Add(transPdfPTable);

                    //To Display Parent Col Names
                    DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    string strTransType = string.Empty;
                    //add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = secondTreeNode;

                        //Table to Load RowData
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        //Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.Padding = 0.95f;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        parentRowPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                        string annotContent = string.Empty;
                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            Image checkedImage = null;

                            if (col == 0)
                            {
                                strRevenue = rowData[col];
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    if (col == 0)
                                    {
                                        strTransType = xRowlistFields[row].Attributes["TypeOfTrans"].Value.ToString();
                                    }

                                    switch (elem.Attributes["ControlType"].Value)
                                    {
                                        case "Amount":
                                            {
                                                if (cellText != null && cellText.Trim() != "")
                                                {
                                                    parentTblAmount = Convert.ToDecimal(cellText);

                                                    if (parentTblAmount < 0)
                                                    {
                                                        cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        cellText = ConvertToCurrencyFormat(cellText);
                                                    }

                                                    //Load the Amount to HashTable for calculating Totals
                                                    //GrandTotals
                                                    if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    else
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    //SubTotals
                                                    if (strTransType.Contains("Receivables"))
                                                    {
                                                        if (htReceivables[xmlStore.Fields[col].label] == null)
                                                        {
                                                            htReceivables[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                        }
                                                        else
                                                        {
                                                            htReceivables[xmlStore.Fields[col].label] = Convert.ToDecimal(htReceivables[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                        }

                                                    }
                                                    if (strTransType.Contains("Payroll"))
                                                    {

                                                        if (htPayroll[xmlStore.Fields[col].label] == null)
                                                        {
                                                            htPayroll[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                        }
                                                        else
                                                        {
                                                            htPayroll[xmlStore.Fields[col].label] = Convert.ToDecimal(htPayroll[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                        }

                                                    }


                                                }

                                                //Align Amount Data to the Right
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                break;
                                            }
                                        //Customize if controlType is Calender
                                        case "Cal":
                                            {
                                                if (cellText != null && cellText != "")
                                                {
                                                    DateTime dateTime;
                                                    DateTime.TryParse(cellText, out dateTime);
                                                    //dateTime = Convert.ToDateTime(cellText);
                                                    if (dateTime != DateTime.MinValue)
                                                    {
                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                    }
                                                }
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                                break;
                                            }
                                        //Customize if controlType is Check
                                        case "Check":
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null || cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                break;
                                            }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        if (xRowlistFields[row].Attributes["Notes"] != null && xRowlistFields[row].Attributes["Notes"].Value != "")
                                        {
                                            annotContent = "Notes: " + xRowlistFields[row].Attributes["Notes"].Value;
                                        }

                                        if (xRowlistFields[row].Attributes["Attachments"] != null && xRowlistFields[row].Attributes["Attachments"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["SecuredBy"] != null && xRowlistFields[row].Attributes["SecuredBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["ChangedBy"] != null && xRowlistFields[row].Attributes["ChangedBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                        }

                                        //if (xRowlistFields[row].Attributes["ToolTip"] != null && xRowlistFields[row].Attributes["ToolTip"].Value != "")
                                        //{
                                        //    if (annotContent != "")
                                        //    {
                                        //        annotContent = annotContent + "\n\n" + "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //    else
                                        //    {
                                        //        annotContent = "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //}
                                    }

                                    break;
                                }
                            }

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));

                            parentRowPdfPTable.DefaultCell.Border = 0;

                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //Add Annotations to the First column of EveryRow
                            if (m_ShowAnnotations)
                            {
                                if (annotContent != "")
                                {
                                    annotContent = annotContent.Replace("~", "\n\t\t");
                                    string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                    Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                    annotImage.Annotation = new Annotation("Annotation", annotContent);

                                    Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                    Phrase imagePhrase = new Phrase(imageChunk);

                                    if (col == 0)
                                    {
                                        parentCellPhrase.Add(imagePhrase);
                                        annotContent = "";
                                    }
                                }

                                parentRowPdfPTable.DefaultCell.BackgroundColor = Color.WHITE;

                                parentRowPdfPTable.AddCell(parentCellPhrase);
                            }

                            parentName = cellText;
                        }

                        string colTotalText = string.Empty;
                        decimal colTotal = 0;

                        if (row > 0)
                        {
                            if (strInitTransType != strTransType)
                            {

                                //SUB TOTALS

                                InvoiceInfoSubTotals(pdfDocument, numColumnsInXml, xNodelistFields, htPayroll, numColumnsInPDF, parentPdfPTable, strInitTransType, ref colTotalText, ref colTotal);


                                //SUB TOTALS


                                strInitTransType = strTransType;
                                pdfDocument.Add(spaceTable);

                                PdfPTable newTransPdfPTable = new PdfPTable(numColumnsInXml);
                                newTransPdfPTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                                newTransPdfPTable.TotalWidth = parentPdfPTable.TotalWidth;
                                newTransPdfPTable.DefaultCell.GrayFill = 1.0f;
                                //newTransPdfPTable.DefaultCell.Border = 0;
                                newTransPdfPTable.DefaultCell.Colspan = numColumnsInXml;
                                newTransPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //newTransPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                                newTransPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                                newTransPdfPTable.DefaultCell.BorderWidthBottom = 0.9f;


                                Chunk newTransChunk = new Chunk(strTransType, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase newTransPhrase = new Phrase(newTransChunk);

                                newTransPdfPTable.AddCell(newTransPhrase);
                                pdfDocument.Add(newTransPdfPTable);

                                pdfDocument.Add(parentPdfPTable);
                            }
                        }

                        pdfDocument.Add(parentRowPdfPTable);

                        if (row == numRecordsInXml - 1)
                        {

                            //TOTALS
                            InvoiceInfoSubTotals(pdfDocument, numColumnsInXml, xNodelistFields, htReceivables, numColumnsInPDF, parentPdfPTable, strInitTransType, ref colTotalText, ref colTotal);
                            //TOTALS

                        }

                    }

                    //pdfDocument.Add(new Paragraph(" "));

                    //Calculate the GrandTotal
                    //ComputeGrandTotal(numColumnsInPDF, parentPdfPTable);

                    // let the caller know we successfully reached 'the end' of this 
                    //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                    bRet = true;
                }
            }
        }

        private void InvoiceInfoSubTotals(Document pdfDocument, int numColumnsInXml, XmlNodeList xNodelistFields, Hashtable htPayroll, int numColumnsInPDF, PdfPTable parentPdfPTable, string strInitTransType, ref string colTotalText, ref decimal colTotal)
        {
            PdfPTable newTotalsPdfPTable = new PdfPTable(numColumnsInXml);
            newTotalsPdfPTable.WidthPercentage = parentPdfPTable.WidthPercentage;
            newTotalsPdfPTable.TotalWidth = parentPdfPTable.TotalWidth;
            newTotalsPdfPTable.DefaultCell.GrayFill = 1.0f;
            newTotalsPdfPTable.DefaultCell.Border = 0;
           // newTotalsPdfPTable.DefaultCell.Padding = 0.95f;
            //newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //
            xmlSubChildCount = numColumnsInPDF;
            m_subChildWidthPct = m_ParentColWidthPct;
            subChildxStore = parentXStore;
            //htGrandTotal = htParentTotal;

            int pdfInserted = 0;

            m_FontSize = 8f;

            for (int col = 0; col < xmlSubChildCount; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == subChildxStore.Fields[col].label)
                    {
                        if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                        {
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                            if (htPayroll[subChildxStore.Fields[col].label] != null && htPayroll[subChildxStore.Fields[col].label].ToString() != "")
                            {
                                colTotalText = htPayroll[subChildxStore.Fields[col].label].ToString();
                                colTotal = Convert.ToDecimal(htPayroll[subChildxStore.Fields[col].label]);

                                if (colTotal < 0)
                                {
                                    colTotalText = "(" + ConvertToCurrencyFormat(Convert.ToString(colTotal * (-1))) + ")";
                                }
                                else
                                {
                                    colTotalText = ConvertToCurrencyFormat(colTotalText);
                                }
                            }
                            else
                            {
                                colTotalText = "0.00";
                            }
                        }
                        else
                        {
                            colTotalText = "";
                        }
                        break;
                    }
                }
                //Totals added new lines
                switch (col)
                {

                    case 0:
                        {
                            Chunk newTotalsChunk0 = new Chunk("", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase0 = new Phrase(newTotalsChunk0);
                            newTotalsPdfPTable.AddCell(newTotalsPhrase0);
                            colTotalText = "";
                            break;
                        }

                    case 1:
                        {
                            Chunk newTotalsChunk1 = new Chunk("Total " + strInitTransType, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase1 = new Phrase(newTotalsChunk1);
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            newTotalsPdfPTable.AddCell(newTotalsPhrase1);
                          
                            break;
                        }
                    case 2:
                        {
                            Chunk newTotalsChunk2 = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase2 = new Phrase(newTotalsChunk2);
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            newTotalsPdfPTable.AddCell(newTotalsPhrase2);
                            break;
                        }
                    case 3:
                        {
                            Chunk newTotalsChunk3 = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase3 = new Phrase(newTotalsChunk3);
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            newTotalsPdfPTable.AddCell(newTotalsPhrase3);
                            colTotalText = "";
                            break;
                        }
                    case 4:
                        {
                            Chunk newTotalsChunk4 = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase4 = new Phrase(newTotalsChunk4);
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            newTotalsPdfPTable.AddCell(newTotalsPhrase4);
                            colTotalText = "";
                            break;
                        }
                    case 5:
                        {
                            Chunk newTotalsChunk5 = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase newTotalsPhrase5 = new Phrase(newTotalsChunk5);
                            newTotalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            newTotalsPdfPTable.AddCell(newTotalsPhrase5);
                            colTotalText = "";
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

            }
            //


            //Chunk newTotalsChunk = new Chunk("Total" + strInitTransType + colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
            //Phrase newTotalsPhrase = new Phrase(newTotalsChunk);
            //newTotalsPdfPTable.AddCell(newTotalsPhrase);

            pdfDocument.Add(newTotalsPdfPTable);
        }
 


        //Method to Get a RowList which is sorted based on TypeOfTrans
        private void GetSortedInvInfo(string secondTreeNode, ArrayList arrTransList, XmlNodeList xRowlistFields, XmlDocument xChildDoc)
        {
            XmlNode nodeRoot = xChildDoc.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeBPE = xChildDoc.CreateNode(XmlNodeType.Element, "bpeout", null);
            nodeRoot.AppendChild(nodeBPE);

            XmlNode nodeFormControls = xChildDoc.CreateNode(XmlNodeType.Element, "FormControls", null);
            nodeBPE.AppendChild(nodeFormControls);

            XmlNode treeNode = xChildDoc.CreateNode(XmlNodeType.Element, secondTreeNode, null);
            nodeFormControls.AppendChild(treeNode);

            XmlNode nodeGridHeading = xChildDoc.CreateNode(XmlNodeType.Element, "GridHeading", null);
            treeNode.AppendChild(nodeGridHeading);

            XmlNode nodeColumns = xChildDoc.CreateNode(XmlNodeType.Element, "Columns", null);
            nodeGridHeading.AppendChild(nodeColumns);

            XmlNode colsList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + secondTreeNode + "//GridHeading//Columns");
            nodeColumns.InnerXml = colsList.InnerXml;

            XmlNode nodeRowList = xChildDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            treeNode.AppendChild(nodeRowList);
            nodeRowList.InnerXml = "";

            StringBuilder sbRowList = new StringBuilder();

            foreach (XmlElement xRow in xRowlistFields)
            {
                string strTypeofTrans = xRow.Attributes["TypeOfTrans"].Value;
                if (strTypeofTrans != null && strTypeofTrans != "")
                {
                    if (arrTransList.Count == 0)
                    {
                        arrTransList.Add(strTypeofTrans);
                    }
                    else
                    {
                        IEnumerator enumArrList = arrTransList.GetEnumerator();
                        int enumCount = 0;
                        while (enumArrList.MoveNext())
                        {
                            if (strTypeofTrans == enumArrList.Current.ToString())
                            {
                                continue;
                            }
                            else
                            {
                                enumCount++;
                            }
                        }

                        if (enumCount == arrTransList.Count)
                        {
                            arrTransList.Add(strTypeofTrans);
                        }
                    }
                }
            }

            IEnumerator enumList = arrTransList.GetEnumerator();

            while (enumList.MoveNext())
            {
                foreach (XmlElement xRow in xRowlistFields)
                {
                    if (xRow.Attributes["TypeOfTrans"].Value == enumList.Current.ToString())
                    {
                        sbRowList.Append(xRow.OuterXml);
                    }
                }
            }

            nodeRowList.InnerXml = sbRowList.ToString();
            xChildDoc.AppendChild(nodeRoot);
        }

        //Method To Load BudgetTotal Details
        private void LoadBudgetTotalDetails(Document pdfDocument, ref bool bRet, ref int numRecordsInXml, ref int numColumnsInXml, string fourthTreeNode, string trxInfoTreeNodeName, XmlNodeList xNodelistFields, int trxID)
        {
            if (m_strXmlDoc.Length > 0)
            {
                //Create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, fourthTreeNode, trxID);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = fourthTreeNode;
                xmlStore.ReportStyle = m_reportStyle;
                xmlStore.IsParent = true;

                //To Load the Records and to get the column and row Counts
                numRecordsInXml = xmlParentCount = xmlStore.LoadRecords();
                numColumnsInXml = xmlStore.Fields.Length;
                parentXStore = xmlStore;

                if (numRecordsInXml > 0 && numColumnsInXml > 0)
                {
                    //Increase Font Size based on column Count
                    if (numColumnsInXml > 10)
                    {
                        m_FontSize = 6f;
                    }
                    else
                    {
                        m_FontSize = 7f;
                    }

                    int numColumnsInPDF = numColumnsInXml;

                    //Table to Load ParentColumnNames
                    PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                    parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                    // Set Column Widths
                    float[] columnWidthInPct = new float[numColumnsInPDF];

                    //--- see if we have width data for the Fields in XmlStore
                    //int widthTotal = xmlStore.GetColumnWidthsTotal();
                    float widthTotal;
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    //Set ColWidths
                    SetBudgetTotalColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentPdfPTable.WidthPercentage = 100;
                    else
                        parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentPdfPTable.TotalWidth = widthTotal;
                    parentPdfPTable.SetWidths(columnWidthInPct);

                    // Set Cell Attributes
                    parentPdfPTable.DefaultCell.BorderWidth = 1;
                    parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Table for creating Space between rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    //To Display Parent Col Names
                    DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);

                    XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + fourthTreeNode + "//RowList/Rows");

                    //Add the rows of Parent
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        if (row > 0)
                        {
                            //If row changes add the Column Header Table
                            pdfDocument.Add(parentPdfPTable);
                        }

                        subChdColHdrCount = 0;

                        this.m_TreeNodeName = fourthTreeNode;

                        //Table to load Rows of the Parent Table
                        PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentRowPdfPTable.WidthPercentage = 100;
                        else
                            parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentRowPdfPTable.TotalWidth = widthTotal;
                        parentRowPdfPTable.SetWidths(columnWidthInPct);

                        // Set Cell Attributes
                        parentRowPdfPTable.DefaultCell.BorderWidth = 1;
                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        string annotContent = string.Empty;
                        string strDescription = string.Empty;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetRecord(row);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal parentTblAmount = 0;
                            string cellText = rowData[col];
                            string imgUrl = string.Empty;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            Image checkedImage = null;

                            if (col == 0)
                            {
                                strRevenue = rowData[col];
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    switch (elem.Attributes["ControlType"].Value)
                                    {
                                        case "Amount":
                                            {
                                                if (cellText != null && cellText.Trim() != "")
                                                {
                                                    parentTblAmount = Convert.ToDecimal(cellText);

                                                    if (parentTblAmount < 0)
                                                    {
                                                        cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        cellText = ConvertToCurrencyFormat(cellText);
                                                    }

                                                    //Load the Amount to HashTable for calculating Totals
                                                    if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    else
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                    }
                                                }

                                                //Align Amount Data to the Right
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                break;
                                            }
                                        //Customize if controlType is Calender
                                        case "Cal":
                                            {
                                                if (cellText != null && cellText != "")
                                                {
                                                    DateTime dateTime;
                                                    DateTime.TryParse(cellText, out dateTime);
                                                    if (dateTime != DateTime.MinValue)
                                                    {
                                                        cellText = dateTime.ToString("MM/dd/yy");
                                                    }
                                                }
                                                break;
                                            }
                                        //Customize if controlType is Check
                                        case "Check":
                                            {
                                                m_IsCheckBox = true;

                                                if (cellText != null || cellText != "0")
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                else
                                                {
                                                    imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                    checkedImage = Image.GetInstance(new Uri(imgUrl));
                                                }
                                                break;
                                            }
                                    }

                                    //Populate the Annotations content if Notes,Attachments etc exist
                                    if (col == 0)
                                    {
                                        if (xRowlistFields[row].Attributes["Notes"] != null && xRowlistFields[row].Attributes["Notes"].Value != "")
                                        {
                                            annotContent = "Notes: " + xRowlistFields[row].Attributes["Notes"].Value;
                                        }

                                        if (xRowlistFields[row].Attributes["Attachments"] != null && xRowlistFields[row].Attributes["Attachments"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "Attachments: " + xRowlistFields[row].Attributes["Attachments"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["SecuredBy"] != null && xRowlistFields[row].Attributes["SecuredBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "SecuredBy: " + xRowlistFields[row].Attributes["SecuredBy"].Value;
                                            }
                                        }

                                        if (xRowlistFields[row].Attributes["ChangedBy"] != null && xRowlistFields[row].Attributes["ChangedBy"].Value != "")
                                        {
                                            if (annotContent != "")
                                            {
                                                annotContent = annotContent + "\n\n" + "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                            else
                                            {
                                                annotContent = "ChangedBy: " + xRowlistFields[row].Attributes["ChangedBy"].Value;
                                            }
                                        }

                                        //if (xRowlistFields[row].Attributes["ToolTip"] != null && xRowlistFields[row].Attributes["ToolTip"].Value != "")
                                        //{
                                        //    if (annotContent != "")
                                        //    {
                                        //        annotContent = annotContent + "\n\n" + "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //    else
                                        //    {
                                        //        annotContent = "ToolTip: " + xRowlistFields[row].Attributes["ToolTip"].Value;
                                        //    }
                                        //}
                                    }

                                    break;
                                }
                            }

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            parentRowPdfPTable.DefaultCell.Border = 0;
                            Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase parentCellPhrase = new Phrase(parentCellChunk);

                            //Add Annotations to the First column of EveryRow
                            if (m_ShowAnnotations)
                            {
                                if (annotContent != "")
                                {
                                    annotContent = annotContent.Replace("~", "\n\t\t");
                                    string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                    Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                    annotImage.Annotation = new Annotation("Annotation", annotContent);

                                    Chunk imageChunk = new Chunk(annotImage, 5, 10);
                                    Phrase imagePhrase = new Phrase(imageChunk);

                                    if (col == 0)
                                    {
                                        parentCellPhrase.Add(imagePhrase);
                                        annotContent = "";
                                    }
                                }

                                parentRowPdfPTable.AddCell(parentCellPhrase);
                            }

                            parentName = cellText;
                        }

                        pdfDocument.Add(parentRowPdfPTable);

                        //extract TrxID values
                        long rowTrxID = 0;
                        if (xRowlistFields[row].Attributes["TrxID"] != null)
                        {
                            rowTrxID = Convert.ToInt64(xRowlistFields[row].Attributes["TrxID"].Value);
                        }

                        int balanceForward = 0;
                        PdfPTable columnTypeTable = new PdfPTable(numColumnsInXml);
                        columnTypeTable.SetWidths(columnWidthInPct);

                        columnTypeTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                        columnTypeTable.TotalWidth = parentPdfPTable.TotalWidth;
                        columnTypeTable.DefaultCell.Border = 0;
                        columnTypeTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                        columnTypeTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                        //Insert Corresponding ChildTable
                        if (trxInfoTreeNodeName != "" && rowTrxID != 0)
                        {
                            float childTableWidth = InsertChildTable(fourthTreeNode, trxInfoTreeNodeName, "", "", "", "", numColumnsInXml, balanceForward, rowTrxID, row, columnTypeTable);
                        }

                        if (row != numRecordsInXml - 1)
                        {
                            pdfDocument.Add(new Paragraph(" "));
                        }
                    }

                    // let the caller know we successfully reached 'the end' of this 
                    //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                    bRet = true;
                }
            }
        }

        //Method To Load BudgetTotal Details(from DataTables)
        private void LoadBgtTotalDetails(Document pdfDocument, ref bool bRet, ref int numRecordsInXml, ref int numColumnsInXml, string fourthTreeNode, string trxInfoTreeNodeName, XmlNodeList xNodelistFields, int trxID, DataSet dsAll)
        {
            int arrayCount = 0;
            DataRow[] drBgtTotalRows = dsAll.Tables[arrayCount + 3].Select("JobID='" + trxID + "'");

            if (m_strXmlDoc.Length > 0)
            {
                //--- create an instance of XmlStore
                ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, fourthTreeNode, trxID);
                xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = fourthTreeNode;
                xmlStore.ReportStyle = m_reportStyle;
                xmlStore.IsParent = true;
                numRecordsInXml = xmlParentCount = xmlStore.LoadRecords();
                numColumnsInXml = xmlStore.Fields.Length;
                parentXStore = xmlStore;

                if (numColumnsInXml > 10)
                    m_FontSize = 6f;
                else
                    m_FontSize = 7f;

                int numColumnsInPDF = numColumnsInXml;

                int col = 0;
                PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                // Set Column Widths
                float[] columnWidthInPct = new float[numColumnsInPDF];

                //--- see if we have width data for the Fields in XmlStore
                //int widthTotal = xmlStore.GetColumnWidthsTotal();
                float widthTotal;
                if (m_ShowLandscape)
                {
                    widthTotal = 125;
                }
                else
                {
                    widthTotal = 145;
                }

                //Set Col Widths
                SetBudgetTotalColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                m_ParentColWidthPct = columnWidthInPct;

                //--- set the total width of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                    parentPdfPTable.WidthPercentage = 100;
                else
                    parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                parentPdfPTable.TotalWidth = widthTotal;
                parentPdfPTable.SetWidths(columnWidthInPct);

                // Set Column Header Cell Attributes
                parentPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                parentPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                spaceTable.WidthPercentage = parentPdfPTable.WidthPercentage;
                spaceTable.TotalWidth = parentPdfPTable.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                foreach (DataColumn column in dsAll.Tables[arrayCount + 3].Columns)
                {
                    if (xmlStore.Fields[col].caption == column.ColumnName)
                    {
                        if (xmlStore.Fields[col].controlType == "Amount")
                        { parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT; }
                        else
                        { parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; }

                        Chunk colChunk = new Chunk(xmlStore.Fields[col].caption, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase colPhrase = new Phrase(colChunk);
                        parentPdfPTable.AddCell(colPhrase);
                        col++;

                        if (col >= xmlStore.Fields.Length)
                            break;
                    }
                }

                pdfDocument.Add(parentPdfPTable);

                XmlNodeList xRowlistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + fourthTreeNode + "//RowList/Rows");

                //add the rows of Parent
                for (int row = 0; row < drBgtTotalRows.Length; row++)
                {
                    if (row > 0)
                    {
                        pdfDocument.Add(parentPdfPTable);
                    }

                    subChdColHdrCount = 0;

                    this.m_TreeNodeName = fourthTreeNode;

                    PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        parentRowPdfPTable.WidthPercentage = 100;
                    else
                        parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    parentRowPdfPTable.TotalWidth = widthTotal;
                    parentRowPdfPTable.SetWidths(columnWidthInPct);

                    //Set Column Header Cell Attributes
                    parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    parentRowPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    string annotContent = string.Empty;

                    col = 0;
                    foreach (DataColumn column in dsAll.Tables[arrayCount + 3].Columns)
                    {
                        decimal parentTblAmount = 0;
                        string cellText = string.Empty;
                        string imgUrl = string.Empty;
                        Image checkedImage = null;

                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        if (xmlStore.Fields[col].caption == column.ColumnName)
                        {
                            cellText = drBgtTotalRows[row][xmlStore.Fields[col].caption].ToString();
                            switch (xmlStore.Fields[col].controlType)
                            {
                                case "Amount":
                                    {
                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        if (cellText != null && cellText.Trim() != "")
                                        {
                                            parentTblAmount = Convert.ToDecimal(cellText);

                                            if (parentTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }

                                            if (htParentTotal[xmlStore.Fields[col].label] == null)
                                            {
                                                htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                            }
                                            else
                                            {
                                                htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                            }
                                        }

                                        parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        break;
                                    }
                                case "Cal":
                                    {
                                        if (cellText != null && cellText != "")
                                        {
                                            DateTime dateTime;
                                            DateTime.TryParse(cellText, out dateTime);
                                            if (dateTime != DateTime.MinValue)
                                            {
                                                cellText = dateTime.ToString("MM/dd/yy");
                                            }
                                        }
                                        break;
                                    }
                                case "Check":
                                    {
                                        m_IsCheckBox = true;

                                        if (cellText != null || cellText != "0")
                                        {
                                            imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                            checkedImage = Image.GetInstance(new Uri(imgUrl));
                                        }
                                        else
                                        {
                                            imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                            checkedImage = Image.GetInstance(new Uri(imgUrl));
                                        }
                                        break;
                                    }
                            }

                            Chunk rowChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase rowPhrase = new Phrase(rowChunk);
                            parentPdfPTable.AddCell(rowPhrase);
                            col++;
                        }

                        if (col >= xmlStore.Fields.Length)
                            break;
                    }

                    pdfDocument.Add(parentRowPdfPTable);

                    XmlNodeList xTrxNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + trxInfoTreeNodeName + "//GridHeading//Columns/Col");
                    long rowTrxID = 0;
                    rowTrxID = Convert.ToInt64(drBgtTotalRows[row]["TrxID"].ToString());
                    DataRow[] drTrxInfoRows = dsAll.Tables[arrayCount + 4].Select("AccountID='" + rowTrxID + "'");

                    ITextXMLStore trxXmlStore = new ITextXMLStore(m_xDoc, fourthTreeNode, trxInfoTreeNodeName, "", "", "", "", m_reportStyle, rowTrxID);
                    trxXmlStore.IsParent = false;
                    trxXmlStore.IsChild = true;
                    trxXmlStore.ParentTreeNodeName = fourthTreeNode;
                    trxXmlStore.TreeNodeName = trxInfoTreeNodeName;
                    trxXmlStore.ReportStyle = m_reportStyle;

                    childXStore = trxXmlStore;
                    int numTrxRowsInXml = trxXmlStore.LoadRecords();
                    int numTrxColumnsInXml = trxXmlStore.Fields.Length;

                    PdfPTable trxPdfPTable = new PdfPTable(numTrxColumnsInXml);
                    trxPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                    // Set Column Widths
                    float[] columnTrxWidthInPct = new float[numTrxColumnsInXml];

                    //Set Col Widths
                    SetChildColWidths(xTrxNodelistFields, trxXmlStore, numTrxColumnsInXml, columnTrxWidthInPct, widthTotal);

                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        trxPdfPTable.WidthPercentage = 100;
                    else
                        trxPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    trxPdfPTable.TotalWidth = widthTotal;
                    trxPdfPTable.SetWidths(columnTrxWidthInPct);

                    // Set Column Header Cell Attributes
                    trxPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                    trxPdfPTable.DefaultCell.BorderWidth = 1;
                    trxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    trxPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                    col = 0;
                    foreach (DataColumn dColumn in dsAll.Tables[arrayCount + 4].Columns)
                    {
                        if (trxXmlStore.Fields[col].caption == dColumn.ColumnName)
                        {
                            if (trxXmlStore.Fields[col].controlType == "Amount")
                            { trxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT; }
                            else
                            { trxPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; }

                            Chunk trxColChunk = new Chunk(trxXmlStore.Fields[col].caption, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase trxColPhrase = new Phrase(trxColChunk);
                            trxPdfPTable.AddCell(trxColPhrase);
                            col++;

                            if (col >= trxXmlStore.Fields.Length)
                                break;
                        }
                    }

                    trxPdfPTable.HeaderRows = 1;
                    pdfDocument.Add(trxPdfPTable);

                    PdfPTable trxRowPdfPTable = new PdfPTable(numTrxColumnsInXml);
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        trxRowPdfPTable.WidthPercentage = 100;
                    else
                        trxRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    trxRowPdfPTable.TotalWidth = widthTotal;
                    trxRowPdfPTable.SetWidths(columnTrxWidthInPct);

                    // Set Column Header Cell Attributes
                    trxRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    trxRowPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    trxRowPdfPTable.DefaultCell.BackgroundColor = Color.WHITE;

                    col = 0;
                    for (int trxRowCount = 0; trxRowCount < drTrxInfoRows.Length; trxRowCount++)
                    {
                        foreach (DataColumn column in dsAll.Tables[arrayCount + 4].Columns)
                        {
                            decimal trxTblAmount = 0;
                            string cellText = string.Empty;
                            string imgUrl = string.Empty;
                            Image checkedImage = null;

                            trxRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                            if (trxXmlStore.Fields[col].caption == column.ColumnName)
                            {
                                cellText = drTrxInfoRows[trxRowCount][trxXmlStore.Fields[col].caption].ToString();
                                switch (trxXmlStore.Fields[col].controlType)
                                {
                                    case "Amount":
                                        {
                                            trxRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                            if (cellText != null && cellText.Trim() != "")
                                            {
                                                trxTblAmount = Convert.ToDecimal(cellText);

                                                if (trxTblAmount < 0)
                                                {
                                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(trxTblAmount * (-1))) + ")";
                                                }
                                                else
                                                {
                                                    cellText = ConvertToCurrencyFormat(cellText);
                                                }

                                                if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                {
                                                    htChildTotal[trxXmlStore.Fields[col].label] = Convert.ToDecimal(trxTblAmount);
                                                }
                                                else
                                                {
                                                    htChildTotal[trxXmlStore.Fields[col].label] = Convert.ToDecimal(htChildTotal[trxXmlStore.Fields[col].label]) + Convert.ToDecimal(trxTblAmount);
                                                }
                                            }

                                            trxRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            break;
                                        }
                                    case "Cal":
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                            break;
                                        }
                                    case "Check":
                                        {
                                            m_IsCheckBox = true;

                                            if (cellText != null || cellText != "0")
                                            {
                                                imgUrl = m_strImagesCDNPath + "Images/cross.gif";
                                                checkedImage = Image.GetInstance(new Uri(imgUrl));
                                            }
                                            else
                                            {
                                                imgUrl = m_strImagesCDNPath + "Images/tick.gif";
                                                checkedImage = Image.GetInstance(new Uri(imgUrl));
                                            }
                                            break;
                                        }
                                }

                                Chunk trxRowChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                                Phrase trxRowPhrase = new Phrase(trxRowChunk);
                                trxRowPdfPTable.AddCell(trxRowPhrase);
                                col++;
                            }

                            if (col >= xmlStore.Fields.Length)
                                break;
                        }
                    }
                    pdfDocument.Add(trxRowPdfPTable);
                }

                // let the caller know we successfully reached 'the end' of this 
                //// request, i.e. loading the data into the iTextSharp 'pdfDocument'
                bRet = true;
            }
        }

        //Method To Display the Table Header containing the Job Column Names
        private void DisplayJobColNames(XmlNodeList xNodelistFields, int numColumnsInXml, ITextXMLStore xmlStore, PdfPTable parentPdfPTable)
        {
            for (int col = 0; col < numColumnsInXml; col++)
            {
                string sHdr = xmlStore.Fields[col].caption;
                Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase phrase = new Phrase(chunk);

                //parentPdfPTable.DefaultCell.BorderWidth = 1;
                parentPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                parentPdfPTable.DefaultCell.BorderWidthBottom = 1f;

                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                        {
                            parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                }

                //parentPdfPTable.DefaultCell.BackgroundColor = WHEAT;


                parentPdfPTable.AddCell(phrase);
            }

            //pdfDocument.Add(parentPdfPTable);

            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
            {
                curentPageNumber = m_pdfWriter.CurrentPageNumber;
            }
        }

        //Method To Display the Table Header containing the BudgetDetails Column Names
        private void DisplayBudgetDetailsColNames(XmlNodeList xNodelistFields, int numColumnsInXml, ITextXMLStore xmlStore, PdfPTable parentPdfPTable)
        {
            for (int col = 0; col < numColumnsInXml; col++)
            {
                string sHdr = xmlStore.Fields[col].caption;
                Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                Phrase phrase = new Phrase(chunk);

                parentPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                parentPdfPTable.DefaultCell.BorderWidthBottom = 0.95f;

                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                        {
                            parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                }

                parentPdfPTable.AddCell(phrase);
            }

            pdfDocument.Add(parentPdfPTable);

            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
            {
                curentPageNumber = m_pdfWriter.CurrentPageNumber;
            }
        }

        //To Set the Respective ColWidths of the BudgetDetailsCols
        private void SetBudgetDetailsColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
            }

            #region CommentedCode
            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;

            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                widthCol = 27;
            //            }
            //            else
            //            {
            //                widthCol = 16.85f;
            //            }

            //            columnWidthInPct[col] = widthCol;
            //        }
            //    }
            //}
            #endregion
        }

        //To Set the Respective ColWidths of the BudgetTotalCols
        private void SetBudgetTotalColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
            }
            #region Commented Code
            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {

            //                widthCol = 45;
            //                columnWidthInPct[col] = widthCol;
            //            }
            //            else
            //            {
            //                //float widthCol = widthTotal / numColumnsInPDF;
            //                widthCol = 20;
            //                columnWidthInPct[col] = widthCol;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        //To Set the Respective ColWidths of the InvoiceInfoCols
        private void SetInvoiceInfoColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
            }

            #region Commented Code
            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            switch (elem.Attributes["ControlType"].Value)
            //            {
            //                case "TBox":
            //                    {
            //                        widthCol = 10;
            //                        break;
            //                    }
            //                case "Cal":
            //                    {
            //                        widthCol = 15;
            //                        break;
            //                    }
            //                case "Amount":
            //                    {
            //                        widthCol = 30;
            //                        break;
            //                    }
            //            }

            //            columnWidthInPct[col] = widthCol;


            //            break;
            //        }
            //    }
            //}
            #endregion
        }
        #endregion

        //Method to Generate Report for 622 from DataTable
        private void LoadDoc622(DataTable dtParent, DataTable dtChild, Hashtable htColNameValues, Hashtable htBColNameValues, string parentTreeNode, string childTreeNode, float widthTotal)
        {
            bool isNoChild = false;
            decimal totalBalForward = 0;
            decimal totalDtRange = 0;
            float[] childColWidths = null;
            Hashtable htMonthTotal = new Hashtable();

            //To get the nodelist containing columns of the parentTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNode + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = parentTreeNode;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            float[] clmWidths = new float[numColumnsInXml];

            this.pageWidth = widthTotal;

            //For setting the Column widths
            SetParentColWidths(xNodelistFields, xmlStore, numColumnsInXml, clmWidths, widthTotal);

            //Table to Load the Column Names
            PdfPTable parentColTable = new PdfPTable(dtParent.Columns.Count - 2);
            parentColTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

            ////Set the Width Percentage based on LandScape value
            if (m_ShowLandscape)
            {
                parentColTable.WidthPercentage = 120;
            }

            parentColTable.TotalWidth = widthTotal;
            parentColTable.SetWidths(clmWidths);

            parentColTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
            parentColTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

            //Display Column Names
            foreach (DataColumn column in dtParent.Columns)
            {
                if (column.ColumnName != "TrxID" && column.ColumnName != "Link1")
                {
                    if (column.ColumnName == "OpenBal")
                    {
                        parentColTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    Chunk textChunk = new Chunk(htColNameValues[column.ColumnName].ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase textPhrase = new Phrase(textChunk);
                    parentColTable.AddCell(textPhrase);
                }
            }

            pdfDocument.Add(parentColTable);

            //To Insert Spacing between the rows 
            PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
            spaceTable.WidthPercentage = parentColTable.WidthPercentage;
            spaceTable.TotalWidth = parentColTable.TotalWidth;
            spaceTable.DefaultCell.GrayFill = 1.0f;
            spaceTable.DefaultCell.Border = 0;
            spaceTable.DefaultCell.Colspan = numColumnsInXml;

            Chunk spaceChunk = new Chunk("", new Font(m_setFontName, 1));
            Phrase spacePhrase = new Phrase(spaceChunk);
            spaceTable.AddCell(spacePhrase);

            //Load the parent Rows
            for (int parentRowCount = 0; parentRowCount < dtParent.Rows.Count; parentRowCount++)
            {
                if (dtParent.Rows[parentRowCount]["TrxID"] == null || dtParent.Rows[parentRowCount]["TrxID"].ToString() == "")
                {
                    break;
                }

                DataRow dRow = dtParent.Rows[parentRowCount];
                isNoChild = false;
                decimal balanceForward = 0;
                string strCurrentMonth = string.Empty;
                decimal dtRangeTotal = 0;

                if (dRow["OpenBal"] != null && dRow["OpenBal"].ToString() != "")
                {
                    balanceForward = Convert.ToDecimal(dRow["OpenBal"]);
                }

                totalBalForward = totalBalForward + balanceForward;

                //Table to load the parent rows
                PdfPTable parentRowPdfPTable = new PdfPTable(dtParent.Columns.Count - 2);
                parentRowPdfPTable.WidthPercentage = parentColTable.WidthPercentage;

                ////Change width percentage based on landscape value
                if (m_ShowLandscape)
                {
                    parentRowPdfPTable.WidthPercentage = 120;
                }
                parentRowPdfPTable.TotalWidth = widthTotal;
                parentRowPdfPTable.SetWidths(clmWidths);

                //Set Cell Attirbutes
                parentRowPdfPTable.DefaultCell.BorderWidth = 0;
                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                //Load Data Column by Column
                foreach (DataColumn column in dtParent.Columns)
                {
                    if (column.ColumnName != "TrxID" && column.ColumnName != "Link1")
                    {
                        decimal colValue = 0;
                        string cellText = string.Empty;
                        cellText = dRow[column.ColumnName].ToString();

                        if (column.ColumnName == "OpenBal")
                        {
                            parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (dRow[column.ColumnName].ToString() != null && dRow[column.ColumnName].ToString() != "")
                            {
                                if (dRow[column.ColumnName].ToString() != null && dRow[column.ColumnName].ToString() != "")
                                {
                                    colValue = Convert.ToDecimal(dRow[column.ColumnName].ToString());
                                }

                                if (colValue < 0)
                                {
                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colValue * (-1))) + ")";
                                }
                                else
                                {
                                    cellText = ConvertToCurrencyFormat(cellText);
                                }
                            }
                        }
                        else
                        {
                            parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }

                        Chunk textChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                        Phrase textPhrase = new Phrase(textChunk);
                        parentRowPdfPTable.AddCell(textPhrase);
                    }
                }

                pdfDocument.Add(parentRowPdfPTable);

                //Per Iteration extract Link1 values of each row
                int link1 = 0;
                if (dRow["Link1"].ToString() != null && dRow["Link1"].ToString() != "")
                {
                    link1 = Convert.ToInt32(dRow["Link1"].ToString());
                }

                //Table to insert corresponding childRecords
                if (dtChild != null)
                {
                    PdfPTable childPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                    childPDFPTable.DefaultCell.Padding = 0.95f;
                    childPDFPTable.DefaultCell.Border = 0;
                    childPDFPTable.TotalWidth = widthTotal;
                    childPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                    if (m_ShowLandscape)
                    {
                        childPDFPTable.WidthPercentage = 120;
                    }

                    //To get columnsList of Child
                    xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNode + "//GridHeading//Columns/Col");

                    //Create an instance of xmlStore
                    xmlStore = new ITextXMLStore(m_xDoc, parentTreeNode, childTreeNode, "", "", "", "", m_reportStyle, link1);

                    //Load ChildTable
                    xmlStore.IsParent = false;
                    xmlStore.IsChild = true;
                    xmlStore.ParentTreeNodeName = parentTreeNode;
                    xmlStore.TreeNodeName = childTreeNode;
                    xmlStore.ReportStyle = "622";
                    numRecordsInXml = xmlStore.LoadRecords();
                    numColumnsInXml = xmlStore.Fields.Length;

                    float[] childWidths = new float[numColumnsInXml];
                    SetChildColWidths(xNodelistFields, xmlStore, numColumnsInXml, childWidths, widthTotal);

                    childPDFPTable.SetWidths(childWidths);
                    childPDFPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                    childColWidths = new float[numColumnsInXml];
                    childColWidths = childWidths;

                    //Display Child column Names
                    foreach (DataColumn dColumn in dtChild.Columns)
                    {
                        if (dColumn.ColumnName != "TrxID" && dColumn.ColumnName != "SubTotal1" && dColumn.ColumnName != "SubTotal1Description" && dColumn.ColumnName != "Link1")
                        {
                            if (dColumn.ColumnName == "Amount")
                            {
                                childPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                childPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            Chunk childChunk = new Chunk(htBColNameValues[dColumn.ColumnName].ToString(), new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase childPhrase = new Phrase(childChunk);
                            childPDFPTable.AddCell(childPhrase);
                        }
                    }

                    pdfDocument.Add(childPDFPTable);

                    //To repeat the column name header if page splits while loading the child table
                    //childPDFPTable.HeaderRows = 1;
                    childPDFPTable.DefaultCell.BackgroundColor = Color.WHITE;

                    int childCount = 0;
                    string monthAmount = string.Empty;

                    //Set the currentPageNumber
                    curentPageNumber = m_pdfWriter.CurrentPageNumber;

                    if (link1 != 0)
                    {
                        //Get corresponding childRecords based on Link1
                        DataRow[] drLinkedChildRows = dtChild.Select("Link1 ='" + link1 + "'", "SubTotal1 ASC");
                        if (drLinkedChildRows.Length > 0)
                        {
                            //Load RowData
                            foreach (DataRow dataRow in drLinkedChildRows)
                            {
                                if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                                {
                                    pdfDocument.Add(parentRowPdfPTable);
                                    pdfDocument.Add(childPDFPTable);
                                    curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                }

                                PdfPTable childRowPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                                childRowPDFPTable.DefaultCell.Padding = 0.95f;
                                childRowPDFPTable.DefaultCell.Border = 0;
                                childRowPDFPTable.TotalWidth = widthTotal;
                                childRowPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                                childRowPDFPTable.SetWidths(childWidths);

                                if (m_ShowLandscape)
                                {
                                    childRowPDFPTable.WidthPercentage = 120;
                                }

                                if (childCount == 0)
                                {
                                    htMonthTotal.Clear();
                                    strCurrentMonth = dataRow["SubTotal1Description"].ToString();
                                    childCount++;
                                }

                                //To Display Monthly Total
                                if (strCurrentMonth != dataRow["SubTotal1Description"].ToString())
                                {
                                    PdfPTable monthPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                                    monthPDFPTable.DefaultCell.Padding = 0.95f;
                                    monthPDFPTable.DefaultCell.Border = 0;
                                    monthPDFPTable.TotalWidth = widthTotal;
                                    monthPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                                    monthPDFPTable.SetWidths(childWidths);

                                    monthPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
                                    monthPDFPTable.DefaultCell.GrayFill = 0.95f;
                                    Chunk monthChunk = new Chunk(strCurrentMonth, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                    Phrase monthPhrase = new Phrase(monthChunk);

                                    monthPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    monthPDFPTable.AddCell(monthPhrase);
                                    monthPDFPTable.DefaultCell.Colspan = 1;

                                    dtRangeTotal = dtRangeTotal + Convert.ToDecimal(htMonthTotal["Amount"].ToString());
                                    monthAmount = htMonthTotal["Amount"].ToString();

                                    if (Convert.ToDecimal(htMonthTotal["Amount"].ToString()) < 0)
                                    {
                                        monthAmount = "(" + ConvertToCurrencyFormat(Convert.ToString((Convert.ToDecimal(monthAmount) * (-1)))) + ")";
                                    }
                                    else
                                    {
                                        monthAmount = ConvertToCurrencyFormat(monthAmount);
                                    }

                                    Chunk monthTotalChunk = new Chunk(monthAmount, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                    Phrase monthTotalPhrase = new Phrase(monthTotalChunk);
                                    monthPDFPTable.AddCell(monthTotalPhrase);
                                    monthPDFPTable.DefaultCell.GrayFill = 1;
                                    pdfDocument.Add(spaceTable);
                                    pdfDocument.Add(monthPDFPTable);
                                    pdfDocument.Add(spaceTable);

                                    strCurrentMonth = dataRow["SubTotal1Description"].ToString();
                                    htMonthTotal.Clear();
                                }

                                //Load Records column by column
                                int colCount = 0;
                                foreach (DataColumn dColumn in dtChild.Columns)
                                {
                                    decimal colValue = 0;
                                    string cellText = string.Empty;
                                    cellText = dataRow[dColumn.ColumnName].ToString();

                                    if (dColumn.ColumnName != "TrxID" && dColumn.ColumnName != "SubTotal1" && dColumn.ColumnName != "SubTotal1Description" && dColumn.ColumnName != "Link1")
                                    {
                                        if (dColumn.ColumnName == "Amount")
                                        {
                                            childRowPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                            if (dataRow[dColumn.ColumnName].ToString() != null && dataRow[dColumn.ColumnName].ToString() != "")
                                            {
                                                colValue = Convert.ToDecimal(dataRow[dColumn.ColumnName].ToString());
                                            }
                                            if (colValue < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colValue * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }

                                            if (htMonthTotal["Amount"] != null && htMonthTotal["Amount"].ToString() != "")
                                            {
                                                htMonthTotal["Amount"] = Convert.ToDecimal(htMonthTotal["Amount"].ToString()) + colValue;
                                            }
                                            else
                                            {
                                                htMonthTotal["Amount"] = colValue;
                                            }
                                        }
                                        else
                                        {
                                            childRowPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }

                                        //XmlNodeList xChildNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNode + "//GridHeading//Columns");
                                        //string controlType = xChildNodelistFields[colCount].Attributes["ControlType"].Value.ToString();
                                        XmlNode xChildNode = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + childTreeNode + "//GridHeading//Columns");
                                        string controlType = xChildNode.SelectSingleNode("Col[@Label='" + dColumn.ColumnName + "']").Attributes["ControlType"].Value.ToString();

                                        if (controlType == "TBox")
                                        {
                                            //int fullViewLength = Convert.ToInt32(xChildNodelistFields[colCount].Attributes["FullViewLength"].Value.ToString());
                                            int fullViewLength = Convert.ToInt32(xChildNode.SelectSingleNode("Col[@Label='" + dColumn.ColumnName + "']").Attributes["FullViewLength"].Value.ToString());
                                            if (cellText.Length > fullViewLength)
                                            {
                                                cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                            }
                                        }

                                        Chunk childChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                                        Phrase childPhrase = new Phrase(childChunk);
                                        childRowPDFPTable.AddCell(childPhrase);
                                    }

                                    colCount++;
                                }

                                pdfDocument.Add(childRowPDFPTable);
                                pdfDocument.Add(new Phrase(""));
                            }

                            pdfDocument.Add(spaceTable);

                            PdfPTable lastMonthPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                            lastMonthPDFPTable.DefaultCell.Padding = 0.95f;
                            lastMonthPDFPTable.DefaultCell.Border = 0;
                            lastMonthPDFPTable.TotalWidth = widthTotal;
                            lastMonthPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                            lastMonthPDFPTable.SetWidths(childWidths);

                            lastMonthPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
                            lastMonthPDFPTable.DefaultCell.GrayFill = 0.95f;

                            Chunk lastMonthChunk = new Chunk(strCurrentMonth, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase lastMonthPhrase = new Phrase(lastMonthChunk);
                            lastMonthPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            lastMonthPDFPTable.AddCell(lastMonthPhrase);
                            lastMonthPDFPTable.DefaultCell.Colspan = 1;

                            dtRangeTotal = dtRangeTotal + Convert.ToDecimal(htMonthTotal["Amount"].ToString());
                            totalDtRange = totalDtRange + dtRangeTotal;
                            monthAmount = htMonthTotal["Amount"].ToString();

                            if (Convert.ToDecimal(htMonthTotal["Amount"].ToString()) < 0)
                            {
                                monthAmount = "(" + ConvertToCurrencyFormat(Convert.ToString((Convert.ToDecimal(monthAmount) * (-1)))) + ")";
                            }
                            else
                            {
                                monthAmount = ConvertToCurrencyFormat(monthAmount);
                            }

                            Chunk lastMonthTotalChunk = new Chunk(monthAmount, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase lastMonthTotalPhrase = new Phrase(lastMonthTotalChunk);
                            lastMonthPDFPTable.AddCell(lastMonthTotalPhrase);
                            lastMonthPDFPTable.DefaultCell.GrayFill = 1;

                            pdfDocument.Add(lastMonthPDFPTable);
                            pdfDocument.Add(spaceTable);
                        }
                        else
                        {
                            //If No records
                            PdfPTable noRecPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                            noRecPDFPTable.DefaultCell.Padding = 0.95f;
                            noRecPDFPTable.DefaultCell.Border = 0;
                            noRecPDFPTable.TotalWidth = widthTotal;
                            noRecPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                            noRecPDFPTable.SetWidths(childWidths);

                            noRecPDFPTable.DefaultCell.Colspan = numColumnsInXml;
                            noRecPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            noRecPDFPTable.DefaultCell.GrayFill = 0.95f;

                            Chunk noRecChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase noRecPhrase = new Phrase(noRecChunk);
                            noRecPDFPTable.AddCell(noRecPhrase);

                            noRecPDFPTable.DefaultCell.Colspan = 1;
                            noRecPDFPTable.DefaultCell.GrayFill = 1;

                            isNoChild = true;

                            pdfDocument.Add(noRecPDFPTable);
                            pdfDocument.Add(spaceTable);
                        }
                    }

                    //pdfDocument.Add(childPDFPTable);

                    //If ChildRecords Exist compute Date Range, Ending Balance
                    if (!isNoChild)
                    {
                        pdfDocument.Add(spaceTable);

                        PdfPTable childTotalPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                        childTotalPDFPTable.DefaultCell.Border = 0;
                        childTotalPDFPTable.TotalWidth = widthTotal;
                        childTotalPDFPTable.WidthPercentage = parentColTable.WidthPercentage;

                        //if (m_ShowLandscape)
                        //{
                        //    childTotalPDFPTable.WidthPercentage = 120;
                        //}

                        childTotalPDFPTable.SetWidths(childWidths);

                        childTotalPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
                        childTotalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        childTotalPDFPTable.DefaultCell.GrayFill = 0.95f;

                        Chunk dtRangeChunk = new Chunk("Date Range Total: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase dtRangePhrase = new Phrase(dtRangeChunk);
                        childTotalPDFPTable.AddCell(dtRangePhrase);

                        childTotalPDFPTable.DefaultCell.Colspan = 1;

                        string dtRangeAmt = string.Empty;
                        decimal endBal = 0;
                        string endBalAmt = string.Empty;

                        if (dtRangeTotal < 0)
                        {
                            dtRangeAmt = "(" + ConvertToCurrencyFormat(Convert.ToString(dtRangeTotal * (-1))) + ")";
                        }
                        else
                        {
                            dtRangeAmt = ConvertToCurrencyFormat(dtRangeTotal.ToString());
                        }

                        Chunk dtRangeAmtChunk = new Chunk(dtRangeAmt, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase dtRangeAmtPhrase = new Phrase(dtRangeAmtChunk);
                        childTotalPDFPTable.AddCell(dtRangeAmtPhrase);

                        childTotalPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
                        childTotalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        childTotalPDFPTable.DefaultCell.GrayFill = 0.95f;

                        Chunk endBalChunk = new Chunk("Ending Balance: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase endBalPhrase = new Phrase(endBalChunk);
                        childTotalPDFPTable.AddCell(endBalPhrase);

                        childTotalPDFPTable.DefaultCell.Colspan = 1;

                        endBal = dtRangeTotal + balanceForward;
                        if (endBal < 0)
                        {
                            endBalAmt = "(" + ConvertToCurrencyFormat(Convert.ToString(endBal * (-1))) + ")";
                        }
                        else
                        {
                            endBalAmt = ConvertToCurrencyFormat(endBal.ToString());
                        }

                        Chunk endBalAmtChunk = new Chunk(endBalAmt, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase endBalAmtPhrase = new Phrase(endBalAmtChunk);
                        childTotalPDFPTable.AddCell(endBalAmtPhrase);

                        childTotalPDFPTable.DefaultCell.GrayFill = 1;

                        pdfDocument.Add(childTotalPDFPTable);
                    }

                    if (parentRowCount != dtParent.Rows.Count - 1)
                    {
                        pdfDocument.Add(spaceTable);
                        pdfDocument.Add(spaceTable);
                    }
                }
                else
                {
                    PdfPTable noRecPDFPTable = new PdfPTable(dtParent.Columns.Count - 2);
                    noRecPDFPTable.DefaultCell.Padding = 0.95f;
                    noRecPDFPTable.DefaultCell.Border = 0;
                    noRecPDFPTable.TotalWidth = widthTotal;
                    noRecPDFPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                    noRecPDFPTable.SetWidths(clmWidths);

                    noRecPDFPTable.DefaultCell.Colspan = numColumnsInXml;
                    noRecPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    noRecPDFPTable.DefaultCell.GrayFill = 0.95f;

                    Chunk noRecChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase noRecPhrase = new Phrase(noRecChunk);
                    noRecPDFPTable.AddCell(noRecPhrase);

                    noRecPDFPTable.DefaultCell.Colspan = 1;
                    noRecPDFPTable.DefaultCell.GrayFill = 1;

                    isNoChild = true;

                    pdfDocument.Add(noRecPDFPTable);
                    pdfDocument.Add(spaceTable);
                }
            }

            //for Total Balance Fwd
            //Table to Populate Total Balance Forward 
            PdfPTable totalPDFPTable = null;
            if (dtChild != null)
            {
                totalPDFPTable = new PdfPTable(dtChild.Columns.Count - 4);
                totalPDFPTable.SetWidths(childColWidths);
            }
            else
            {
                totalPDFPTable = new PdfPTable(dtParent.Columns.Count - 2);
                totalPDFPTable.SetWidths(clmWidths);
            }
            totalPDFPTable.DefaultCell.Border = 0;
            totalPDFPTable.TotalWidth = widthTotal;
            totalPDFPTable.WidthPercentage = 145;
            if (m_ShowLandscape)
            {
                totalPDFPTable.WidthPercentage = 120;
            }

            totalPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalPDFPTable.DefaultCell.BackgroundColor = WHEAT;

            Chunk totalBalFwdChunk = new Chunk("Total Balance Forward: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalBalFwdPhrase = new Phrase(totalBalFwdChunk);
            totalPDFPTable.AddCell(totalBalFwdPhrase);

            totalPDFPTable.DefaultCell.Colspan = 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            string totalBalFwd = string.Empty;
            if (totalBalForward < 0)
            {
                totalBalFwd = "(" + ConvertToCurrencyFormat(Convert.ToString(totalBalForward * (-1))) + ")";
            }
            else
            {
                totalBalFwd = ConvertToCurrencyFormat(totalBalForward.ToString());
            }

            Chunk totalBalFwdAmtChunk = new Chunk(totalBalFwd, new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalBalFwdAmtPhrase = new Phrase(totalBalFwdAmtChunk);
            totalPDFPTable.AddCell(totalBalFwdAmtPhrase);

            totalPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            //for Total Date Range
            Chunk totalDtRangeChunk = new Chunk("Total Date Range: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalDtRangePhrase = new Phrase(totalDtRangeChunk);
            totalPDFPTable.AddCell(totalDtRangePhrase);

            totalPDFPTable.DefaultCell.Colspan = 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            string grandDtRangeTotal = string.Empty;
            if (totalDtRange < 0)
            {
                grandDtRangeTotal = "(" + ConvertToCurrencyFormat(Convert.ToString(totalDtRange * (-1))) + ")";
            }
            else
            {
                grandDtRangeTotal = ConvertToCurrencyFormat(totalDtRange.ToString());
            }

            Chunk totalDtRangeAmtChunk = new Chunk(grandDtRangeTotal, new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalDtRangeAmtPhrase = new Phrase(totalDtRangeAmtChunk);
            totalPDFPTable.AddCell(totalDtRangeAmtPhrase);

            //For Total Ending Balance
            totalPDFPTable.DefaultCell.Colspan = numColumnsInXml - 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            Chunk totalEndBalChunk = new Chunk("Total Ending Balance: ", new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalEndRangePhrase = new Phrase(totalEndBalChunk);
            totalPDFPTable.AddCell(totalEndRangePhrase);

            totalPDFPTable.DefaultCell.Colspan = 1;
            totalPDFPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            string grandEndBalTotal = string.Empty;
            decimal totalEndBal = totalDtRange + totalBalForward;
            if (totalEndBal < 0)
            {
                grandEndBalTotal = "(" + ConvertToCurrencyFormat(Convert.ToString(totalEndBal * (-1))) + ")";
            }
            else
            {
                grandEndBalTotal = ConvertToCurrencyFormat(totalEndBal.ToString());
            }

            Chunk totalEndBalAmtChunk = new Chunk(grandEndBalTotal, new Font(m_setFontName, m_FontSize, Font.BOLD));
            Phrase totalEndBalAmtPhrase = new Phrase(totalEndBalAmtChunk);
            totalPDFPTable.AddCell(totalEndBalAmtPhrase);

            //pdfDocument.Add(spaceTable);
            //pdfDocument.Add(spaceTable);
            pdfDocument.Add(totalPDFPTable);
        }

        //Method to Generate Report for 670
        private bool LoadDocument670(Document pdfDocument, Hashtable htTreeNodeNames)
        {
            bool bRet = false;

            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            if (m_strXmlDoc.Length > 0)
            {
                for (int treeCount = 1; treeCount < htTreeNodeNames.Count; treeCount++)
                {
                    //Clear HahTable for every Iteration
                    htParentTotal.Clear();

                    this.m_TreeNodeName = htTreeNodeNames[treeCount].ToString();

                    string parentTitle = parentName = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//GridHeading/Title").InnerText.ToString();

                    //Get the corresponding Tree Node columns
                    XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.m_TreeNodeName + "//GridHeading//Columns/Col");

                    //Create an instance of XmlStore
                    ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                    xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = this.m_TreeNodeName;
                    xmlStore.ReportStyle = m_reportStyle;
                    parentXStore = xmlStore;

                    //To Load the Records and to get the column and row Counts
                    numRecordsInXml = xmlStore.LoadRecords();
                    numColumnsInXml = xmlStore.Fields.Length;

                    //Table to Load the Title
                    PdfPTable titlePdfPTable = new PdfPTable(numColumnsInXml);
                    titlePdfPTable.DefaultCell.Padding = 0.95f;  //in Point
                    float[] columnWidthInPct = new float[numColumnsInXml];
                    float widthTotal;

                    //Set widthTotal Based on LandScape Value
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    this.pageWidth = widthTotal;
                    //Set ColWidths
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInXml, columnWidthInPct, widthTotal);

                    m_ParentColWidthPct = columnWidthInPct;

                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        titlePdfPTable.WidthPercentage = 100;
                    else
                        titlePdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    titlePdfPTable.TotalWidth = widthTotal;
                    titlePdfPTable.SetWidths(columnWidthInPct);

                    // Set Cell Attributes
                    titlePdfPTable.DefaultCell.BorderWidth = 1;
                    titlePdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    Chunk titleChunk = new Chunk(parentTitle, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase titlePhrase = new Phrase(titleChunk);
                    titlePdfPTable.DefaultCell.Border = 0;
                    titlePdfPTable.DefaultCell.Colspan = numColumnsInXml;
                    titlePdfPTable.AddCell(titlePhrase);
                    pdfDocument.Add(titlePdfPTable);

                    //Table to insert Space Between Rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = titlePdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = titlePdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;

                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    if (numColumnsInXml > 0)
                    {
                        //Increase Font Size based on column Count
                        if (numColumnsInXml > 10)
                        {
                            m_FontSize = 6f;
                        }
                        else
                        {
                            m_FontSize = 7f;
                        }

                        int numColumnsInPDF = numColumnsInXml;

                        //Table to Load Parent Col Names
                        PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                        parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point

                        // Set ColumnWidths
                        columnWidthInPct = new float[numColumnsInPDF];

                        //--- see if we have width data for the Fields in XmlStore
                        //int widthTotal = xmlStore.GetColumnWidthsTotal();

                        //Set Width Total based on Landscape value
                        if (m_ShowLandscape)
                        {
                            widthTotal = 125;
                        }
                        else
                        {
                            widthTotal = 145;
                        }

                        //Set Col Widths
                        SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                        m_ParentColWidthPct = columnWidthInPct;

                        //--- set the total width of the table
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentPdfPTable.WidthPercentage = 100;
                        else
                            parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentPdfPTable.TotalWidth = widthTotal;
                        parentPdfPTable.SetWidths(columnWidthInPct);

                        // Set Column Header Cell Attributes
                        parentPdfPTable.DefaultCell.BorderWidth = 1;
                        parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                        //To Display Parent Col Names
                        if (numRecordsInXml > 0)
                        {
                            DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);
                        }
                        else
                        {
                            pdfDocument.Add(spaceTable);
                        }

                        //add the rows of Parent
                        if (numRecordsInXml > 0)
                        {
                            //Load RowData
                            for (int row = 0; row < numRecordsInXml; row++)
                            {
                                //If Page Number Changes insert the column Header
                                if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                                {
                                    curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                    pdfDocument.Add(parentPdfPTable);
                                }

                                //Table to insert RowData
                                PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    parentRowPdfPTable.WidthPercentage = 100;
                                else
                                    parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                parentRowPdfPTable.TotalWidth = widthTotal;
                                parentRowPdfPTable.SetWidths(columnWidthInPct);

                                // Set Column Header Cell Attributes
                                parentRowPdfPTable.DefaultCell.BorderWidth = 1;
                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                parentRowPdfPTable.DefaultCell.Indent = 1;

                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                                //Get the corresponding RowData from the xmlStore 
                                string[] rowData = xmlStore.GetRecord(row);

                                //Load Records column by column
                                for (int col = 0; col < numColumnsInXml; col++)
                                {
                                    decimal parentTblAmount = 0;
                                    string cellText = rowData[col];
                                    int fullViewLength = 0;
                                    string controlType = string.Empty;

                                    foreach (XmlElement elem in xNodelistFields)
                                    {
                                        fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                        controlType = elem.Attributes["ControlType"].Value;

                                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                        {
                                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                            {
                                                if (cellText != null && cellText != "")
                                                {
                                                    if (cellText != null && cellText.Trim() != "")
                                                    {
                                                        parentTblAmount = Convert.ToDecimal(cellText);
                                                    }

                                                    if (parentTblAmount < 0)
                                                    {
                                                        cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        cellText = ConvertToCurrencyFormat(cellText);
                                                    }

                                                    //Load the Amount to HashTable for calculating Totals
                                                    if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    else
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                    }
                                                }

                                                //Align Amount Data to the Right
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            }
                                            else
                                            {
                                                //Customize if controlType is Calender
                                                if (elem.Attributes["ControlType"].Value == "Cal")
                                                {
                                                    if (cellText != null && cellText != "")
                                                    {
                                                        DateTime dateTime;
                                                        DateTime.TryParse(cellText, out dateTime);
                                                        if (dateTime != DateTime.MinValue)
                                                        {
                                                            dateTime = Convert.ToDateTime(cellText);
                                                            cellText = dateTime.ToString("MM/dd/yy");
                                                        }
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                                    if (controlType == "TBox")
                                    {
                                        if (cellText.Length > fullViewLength)
                                        {
                                            cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                        }
                                    }

                                    Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                                    Phrase parentCellPhrase = new Phrase(parentCellChunk);
                                    parentRowPdfPTable.DefaultCell.Border = 0;

                                    parentRowPdfPTable.AddCell(parentCellPhrase);
                                }
                                pdfDocument.Add(parentRowPdfPTable);
                                //pdfDocument.Add(new Paragraph(""));
                                //pdfDocument.Add(spaceTable);

                                if (row == numRecordsInXml - 1)
                                {
                                    pdfDocument.Add(spaceTable);
                                }
                            }

                            //Calculate the Grand Total
                            ComputeGrandTotal(numColumnsInPDF, parentPdfPTable);
                        }
                        else
                        {
                            //If Page Number Chnages set the current PageNumber to the page number of pdfWriter
                            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                            {
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                //pdfDocument.Add(parentPdfPTable);
                            }

                            //If No Records
                            PdfPTable emptyDataTable = new PdfPTable(numColumnsInXml);
                            emptyDataTable.DefaultCell.Border = 0;
                            emptyDataTable.DefaultCell.Padding = 0.95f;
                            emptyDataTable.DefaultCell.Colspan = numColumnsInXml;

                            widthTotal = 145;

                            if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                emptyDataTable.WidthPercentage = 100;
                            else
                                emptyDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                            emptyDataTable.TotalWidth = widthTotal;
                            emptyDataTable.SetWidths(columnWidthInPct);
                            emptyDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                            emptyDataTable.DefaultCell.GrayFill = 0.95f;

                            //Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Chunk noRecordChunk = new Chunk("No " + parentName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase noRecordPhrase = new Phrase(noRecordChunk);

                            emptyDataTable.AddCell(noRecordPhrase);
                            emptyDataTable.DefaultCell.GrayFill = 1.0f;

                            pdfDocument.Add(emptyDataTable);
                            pdfDocument.Add(spaceTable);
                        }
                    }
                    bRet = true;
                }
            }

            if (bRet == false)
            {
                pdfDocument.Add(new Paragraph("Failed to load data"));
            }

            return bRet;
        }

        //Method to Generate Report for 671
        private bool LoadDocument671(Document pdfDocument, Hashtable htTreeNodeNames)
        {
            bool bRet = false;
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            curentPageNumber = m_pdfWriter.CurrentPageNumber;
            Color clrLightGreen = new Color(81, 123, 142);

            //To get the TreeNodeNames
            XmlNode nodeParents = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");

            if (m_strXmlDoc.Length > 0)
            {
                m_dicNodeTotals = new Dictionary<string, decimal>();

                for (int treeCount = 1; treeCount < htTreeNodeNames.Count; treeCount++)
                {
                    Decimal nodeAmountTotal = new Decimal();
                    //Clear HashTable for every Iteration
                    htParentTotal.Clear();
                    this.m_TreeNodeName = htTreeNodeNames[treeCount].ToString();
                    XmlNode nodeTree = m_xDoc.SelectSingleNode("Root/bpeout/FormControls/" + this.m_TreeNodeName);
                    XmlNode nodeGridHeading = nodeTree.SelectSingleNode("GridHeading");

                    string parentTitle = parentName = nodeGridHeading.SelectSingleNode("Title").InnerText.ToString();

                    //Get the corresponding Tree Node columns
                    XmlNodeList xNodelistFields = nodeGridHeading.SelectNodes("Columns/Col");

                    //Create an instance of XmlStore
                    ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
                    xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = this.m_TreeNodeName;
                    xmlStore.ReportStyle = m_reportStyle;
                    parentXStore = xmlStore;

                    //To Load the Records and to get the column and row Counts
                    numRecordsInXml = xmlStore.LoadRecords();
                    numColumnsInXml = xmlStore.Fields.Length;


                    float[] columnWidthInPct = new float[numColumnsInXml];
                    float widthTotal;
                    //Set widthTotal Based on LandScape Value
                    widthTotal = (m_ShowLandscape) ? 125 : 145;
                    this.pageWidth = widthTotal;
                    //Set ColWidths
                    SetParentColWidths(xNodelistFields, xmlStore, numColumnsInXml, columnWidthInPct, widthTotal);
                    m_ParentColWidthPct = columnWidthInPct;

                    //Table to Load the Title
                    PdfPTable titlePdfPTable = new PdfPTable(1);
                    titlePdfPTable.DefaultCell.BackgroundColor = new Color(202, 225, 231);
                    //--- set the total width of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        titlePdfPTable.WidthPercentage = 100;
                    else
                        titlePdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                    //titlePdfPTable.WidthPercentage +=1;
                    titlePdfPTable.TotalWidth = widthTotal;
                    // Set Cell Attributes
                    titlePdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    Chunk titleChunk = new Chunk(parentTitle, new Font(m_setFontName, 9.0F, Font.BOLD));
                    Phrase titlePhrase = new Phrase(titleChunk);
                    titlePdfPTable.DefaultCell.Border = 0;
                    titlePdfPTable.AddCell(titlePhrase);
                    titlePdfPTable.DefaultCell.PaddingLeft = 0.0F;

                    //Table to insert Space after the title
                    PdfPTable tblTitleSpacer = new PdfPTable(numColumnsInXml);
                    tblTitleSpacer.WidthPercentage = titlePdfPTable.WidthPercentage;
                    tblTitleSpacer.TotalWidth = titlePdfPTable.TotalWidth;
                    tblTitleSpacer.DefaultCell.Border = 0;
                    tblTitleSpacer.DefaultCell.Colspan = numColumnsInXml;
                    Chunk chnkSpacer = new Chunk("", new Font(m_setFontName, 1F));
                    Phrase phseSpacer = new Phrase(chnkSpacer);
                    tblTitleSpacer.AddCell(phseSpacer);

                    pdfDocument.Add(titlePdfPTable);
                    pdfDocument.Add(tblTitleSpacer);

                    //Table to insert Space Between Rows
                    PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                    spaceTable.WidthPercentage = titlePdfPTable.WidthPercentage;
                    spaceTable.TotalWidth = titlePdfPTable.TotalWidth;
                    spaceTable.DefaultCell.GrayFill = 1.0f;
                    spaceTable.DefaultCell.Border = 0;
                    spaceTable.DefaultCell.Colspan = numColumnsInXml;
                    Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                    Phrase spacePhrase = new Phrase(spaceChunk);
                    spaceTable.AddCell(spacePhrase);

                    if (numColumnsInXml > 0)
                    {
                        //Increase Font Size based on column Count
                        if (numColumnsInXml > 10)
                        {
                            m_FontSize = 6f;
                        }
                        else
                        {
                            m_FontSize = 7f;
                        }

                        int numColumnsInPDF = numColumnsInXml;

                        //Table to Load Parent Col Names
                        PdfPTable parentPdfPTable = new PdfPTable(numColumnsInPDF);
                        parentPdfPTable.DefaultCell.Padding = 0.95f;  //in Point


                        // Set ColumnWidths
                        columnWidthInPct = new float[numColumnsInPDF];

                        //--- see if we have width data for the Fields in XmlStore
                        //int widthTotal = xmlStore.GetColumnWidthsTotal();

                        //Set Width Total based on Landscape value
                        if (m_ShowLandscape)
                        {
                            widthTotal = 125;
                        }
                        else
                        {
                            widthTotal = 145;
                        }

                        //Set Col Widths
                        SetParentColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                        m_ParentColWidthPct = columnWidthInPct;

                        //--- set the total width of the table
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            parentPdfPTable.WidthPercentage = 100;
                        else
                            parentPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        parentPdfPTable.TotalWidth = widthTotal;
                        parentPdfPTable.SetWidths(columnWidthInPct);

                        // Set Column Header Cell Attributes
                        parentPdfPTable.DefaultCell.BorderWidth = 0;
                        parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        parentPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                        parentPdfPTable.DefaultCell.PaddingTop = 2F;
                        parentPdfPTable.DefaultCell.PaddingBottom = 4F;

                        ITextPageEvent pageEventHandler = (ITextPageEvent)m_pdfWriter.PageEvent;
                        pageEventHandler.TableNodeTitle = titlePdfPTable;
                        pageEventHandler.TableSpacer = spaceTable;
                        pageEventHandler.TableColHeader = parentPdfPTable;

                        //To Display Parent Col Names
                        if (numRecordsInXml > 0)
                        {
                            DisplayParentColNames(xNodelistFields, numColumnsInXml, xmlStore, parentPdfPTable);
                        }
                        else
                        {
                            pdfDocument.Add(spaceTable);
                        }

                        //add the rows of Parent
                        if (numRecordsInXml > 0)
                        {
                            //Find the index at which "Withdrawal" is found in the rowData array
                            int indexOfWithDrawal = 0;
                            foreach (ITextXMLStore.Field col in xmlStore.Fields)
                            {
                                if (col.label == "Witthdrawal")
                                {
                                    break;
                                }
                                indexOfWithDrawal++;
                            }

                            //Load RowData
                            for (int row = 0; row < numRecordsInXml; row++)
                            {
                                //If Page Number Changes insert the column Header-This is now hadled using pageevent.
                                if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                                {
                                    curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                    //pdfDocument.Add(titlePdfPTable);//Add the current Title first 
                                    //pdfDocument.Add(spaceTable);
                                    //pdfDocument.Add(parentPdfPTable);//Then add the column header.
                                }

                                //Table to insert RowData
                                PdfPTable parentRowPdfPTable = new PdfPTable(numColumnsInPDF);
                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    parentRowPdfPTable.WidthPercentage = 100;
                                else
                                    parentRowPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                parentRowPdfPTable.TotalWidth = widthTotal;
                                parentRowPdfPTable.SetWidths(columnWidthInPct);

                                // Set Column Header Cell Attributes
                                parentRowPdfPTable.DefaultCell.BorderWidth = 1;
                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                parentRowPdfPTable.DefaultCell.Indent = 1;
                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                                //Get the corresponding RowData from the xmlStore 
                                string[] rowData = xmlStore.GetRecord(row);
                                //Sum the Withdrawal amount in each iteration.
                                nodeAmountTotal += Convert.ToDecimal(rowData[indexOfWithDrawal]);

                                //Load Records column by column
                                for (int col = 0; col < numColumnsInXml; col++)
                                {
                                    decimal parentTblAmount = 0;
                                    string cellText = rowData[col];
                                    int fullViewLength = 0;
                                    string controlType = string.Empty;

                                    #region Formatting stuff based on the col control type.
                                    foreach (XmlElement elem in xNodelistFields)
                                    {
                                        fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                        controlType = elem.Attributes["ControlType"].Value;

                                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                        {
                                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                            {
                                                if (cellText != null && cellText != "")
                                                {
                                                    if (cellText != null && cellText.Trim() != "")
                                                    {
                                                        parentTblAmount = Convert.ToDecimal(cellText);
                                                    }

                                                    if (parentTblAmount < 0)
                                                    {
                                                        cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(parentTblAmount * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        cellText = ConvertToCurrencyFormat(cellText);
                                                    }

                                                    //Load the Amount to HashTable for calculating Totals
                                                    if (htParentTotal[xmlStore.Fields[col].label] == null)
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(parentTblAmount);
                                                    }
                                                    else
                                                    {
                                                        htParentTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htParentTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(parentTblAmount);
                                                    }
                                                }

                                                //Align Amount Data to the Right
                                                parentRowPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            }
                                            else
                                            {
                                                //Customize if controlType is Calender
                                                if (elem.Attributes["ControlType"].Value == "Cal")
                                                {
                                                    if (cellText != null && cellText != "")
                                                    {
                                                        DateTime dateTime;
                                                        DateTime.TryParse(cellText, out dateTime);
                                                        if (dateTime != DateTime.MinValue)
                                                        {
                                                            dateTime = Convert.ToDateTime(cellText);
                                                            cellText = dateTime.ToString("MM/dd/yy");
                                                        }
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    }


                                    //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                                    if (controlType == "TBox")
                                    {
                                        if (cellText.Length > fullViewLength)
                                        {
                                            cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                        }
                                    }
                                    #endregion

                                    Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                                    Phrase parentCellPhrase = new Phrase(parentCellChunk);
                                    parentRowPdfPTable.DefaultCell.Border = 0;
                                    parentRowPdfPTable.AddCell(parentCellPhrase);
                                }
                                pdfDocument.Add(parentRowPdfPTable);

                                if (row == numRecordsInXml - 1)//Add a spacer after the last row has been added.
                                {
                                    pdfDocument.Add(spaceTable);
                                }
                            }

                            //Add the Total for the above iteration.
                            //Table to Load the Title
                            PdfPTable totalsPdfPTable = new PdfPTable(2);
                            totalsPdfPTable.DefaultCell.BackgroundColor = new Color(202, 225, 231);
                            //--- set the total width of the table
                            if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                totalsPdfPTable.WidthPercentage = 100;
                            else
                                totalsPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;
                            totalsPdfPTable.SetWidths(new float[] { 70, 30 });
                            totalsPdfPTable.TotalWidth = widthTotal;
                            totalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalsPdfPTable.DefaultCell.PaddingTop = 5.0F;
                            totalsPdfPTable.DefaultCell.PaddingBottom = 5.0F;

                            Chunk totalChunk = new Chunk("Total " + parentTitle, new Font(m_setFontName, 9.0F, Font.BOLD));
                            Phrase totalPhrase = new Phrase(totalChunk);
                            totalsPdfPTable.DefaultCell.Border = 0;
                            totalsPdfPTable.AddCell(totalPhrase);

                            totalsPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalChunk = new Chunk(FormatText("Amount", nodeAmountTotal.ToString()), new Font(m_setFontName, 9.0F, Font.BOLD));
                            totalPhrase = new Phrase(totalChunk);
                            totalsPdfPTable.DefaultCell.Border = 0;
                            totalsPdfPTable.AddCell(totalPhrase);

                            pdfDocument.Add(totalsPdfPTable);

                            int loopSpaceCntr = (treeCount != htTreeNodeNames.Count - 1) ? 10 : 1;
                            //Add a spacer of ~1/2 inch height soon after the NodeTotal has been added.
                            for (int i = 0; i < loopSpaceCntr; i++)
                            {
                                pdfDocument.Add(spaceTable);
                            }

                            //Add the nodeTotal to the Hashtable list.
                            m_dicNodeTotals[this.m_TreeNodeName] = nodeAmountTotal;
                            //Calculate the Grand Total
                            ComputeGrandTotal(numColumnsInPDF, parentPdfPTable);
                        }
                        else
                        {
                            //If Page Number Chnages set the current PageNumber to the page number of pdfWriter
                            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                            {
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;
                                //pdfDocument.Add(parentPdfPTable);
                            }

                            //If No Records
                            PdfPTable emptyDataTable = new PdfPTable(numColumnsInXml);
                            emptyDataTable.DefaultCell.Border = 0;
                            emptyDataTable.DefaultCell.Padding = 0.95f;
                            emptyDataTable.DefaultCell.Colspan = numColumnsInXml;

                            widthTotal = 145;

                            if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                emptyDataTable.WidthPercentage = 100;
                            else
                                emptyDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                            emptyDataTable.TotalWidth = widthTotal;
                            emptyDataTable.SetWidths(columnWidthInPct);
                            emptyDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                            emptyDataTable.DefaultCell.GrayFill = 0.95f;

                            //Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Chunk noRecordChunk = new Chunk("No " + parentName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase noRecordPhrase = new Phrase(noRecordChunk);

                            emptyDataTable.AddCell(noRecordPhrase);
                            emptyDataTable.DefaultCell.GrayFill = 1.0f;

                            pdfDocument.Add(emptyDataTable);
                            pdfDocument.Add(spaceTable);
                        }
                    }
                    bRet = true;
                }
            }

            if (bRet == false)
            {
                pdfDocument.Add(new Paragraph("Failed to load data"));
            }

            return bRet;
        }

        //To Display Account Details of ReportStyle670
        private void DisplayAccountDetails(Document pdfDocument, Hashtable htTreeNodeNames)
        {
            Hashtable htControlType = new Hashtable();

            string treeNodeName = htTreeNodeNames[0].ToString();
            //Get the Column List of the TreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + treeNodeName + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = treeNodeName;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //To Load Corresponding Control Types of the Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            //Add the rows of Parent
            for (int row = 0; row < numRecordsInXml; row++)
            {
                //Get the corresponding RowData from the xmlStore 
                string[] rowData = xmlStore.GetRecord(row);

                //Load Records column by column
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    //Table to Load the RowData
                    PdfPTable accountPdfPTable = new PdfPTable(numColumnsInXml);
                    accountPdfPTable.DefaultCell.Padding = 0.95f;  //in Point
                    float[] columnWidthInPct = new float[numColumnsInXml];
                    float widthTotal;

                    //Set the WidthTotal Based on LandScape Value
                    if (m_ShowLandscape)
                    {
                        widthTotal = 125;
                    }
                    else
                    {
                        widthTotal = 145;
                    }

                    this.pageWidth = widthTotal;

                    //Set Col Widths
                    SetAccountColWidths(numColumnsInXml, columnWidthInPct, xmlStore);

                    //Set the TotalWidth Percentage of the table
                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        accountPdfPTable.WidthPercentage = 100;
                    else
                        accountPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    accountPdfPTable.TotalWidth = widthTotal;
                    accountPdfPTable.SetWidths(columnWidthInPct);
                    accountPdfPTable.DefaultCell.BorderWidth = 0;
                    accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    string colName = xmlStore.Fields[col].caption;
                    string cellText = rowData[col];

                    if (cellText != null && cellText != "")
                    {
                        if (htControlType[xmlStore.Fields[col].label] != null)
                        {
                            if (htControlType[xmlStore.Fields[col].label].ToString() == "Amount")
                            {
                                decimal colAmount = Convert.ToDecimal(cellText);

                                if (colAmount < 0)
                                {
                                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colAmount * (-1))) + ")";
                                }
                                else
                                {
                                    cellText = ConvertToCurrencyFormat(cellText);
                                }
                            }
                            else
                            {
                                //Customize if controlType is Calender
                                if (htControlType[xmlStore.Fields[col].label].ToString() == "Cal")
                                {
                                    if (cellText != null && cellText != "")
                                    {
                                        DateTime dateTime;
                                        DateTime.TryParse(cellText, out dateTime);
                                        if (dateTime != DateTime.MinValue)
                                        {
                                            dateTime = Convert.ToDateTime(cellText);
                                            cellText = dateTime.ToString("MM/dd/yy");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (col != 0)
                    {
                        accountPdfPTable.DefaultCell.Colspan = numColumnsInXml - 1;

                        Chunk colChunk = new Chunk(colName + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase colPhrase = new Phrase(colChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(colPhrase);

                        accountPdfPTable.DefaultCell.Colspan = 1;

                        Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                        Phrase parentCellPhrase = new Phrase(parentCellChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(parentCellPhrase);
                    }
                    else
                    {
                        accountPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        Chunk colChunk = new Chunk(colName + ": ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase colPhrase = new Phrase(colChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(colPhrase);

                        accountPdfPTable.DefaultCell.Colspan = 2;
                        Chunk parentCellChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                        Phrase parentCellPhrase = new Phrase(parentCellChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(parentCellPhrase);

                        accountPdfPTable.DefaultCell.Colspan = numColumnsInXml - 1;

                        Chunk cellChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                        Phrase cellPhrase = new Phrase(cellChunk);
                        accountPdfPTable.DefaultCell.Border = 0;
                        accountPdfPTable.AddCell(cellPhrase);

                        accountPdfPTable.DefaultCell.Colspan = 1;
                    }

                    pdfDocument.Add(accountPdfPTable);

                    if (col == 0)
                    {
                        //pdfDocument.Add(new Paragraph(" "));

                        PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                        spaceTable.WidthPercentage = accountPdfPTable.WidthPercentage;
                        spaceTable.TotalWidth = accountPdfPTable.TotalWidth;
                        spaceTable.DefaultCell.GrayFill = 1.0f;
                        spaceTable.DefaultCell.Border = 0;
                        spaceTable.DefaultCell.Colspan = numColumnsInXml;

                        Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                        Phrase spacePhrase = new Phrase(spaceChunk);
                        spaceTable.AddCell(spacePhrase);

                        pdfDocument.Add(spaceTable);

                        this.LoadDocument670(pdfDocument, htTreeNodeNames);
                    }

                }
            }
        }

        /// <summary>
        /// To Display Account Details of ReportStyle671.
        /// </summary>
        /// <param name="pdfDocument"></param>
        /// <param name="htTreeNodeNames"></param>
        private void DisplayAccountDetails671(Document pdfDocument, Hashtable htTreeNodeNames)
        {
            Hashtable htControlType = new Hashtable();

            string treeNodeName = htTreeNodeNames[0].ToString();
            //Get the Column List of the TreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns/Col");

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_strXmlDoc, m_reportStyle);
            xmlStore.ParentTreeNodeName = xmlStore.TreeNodeName = treeNodeName;
            xmlStore.ReportStyle = m_reportStyle;

            //To Load the Records and to get the column and row Counts
            int numRecordsInXml = xmlStore.LoadRecords();
            int numColumnsInXml = xmlStore.Fields.Length;

            //To Load Corresponding Control Types of the Respective Columns
            for (int col = 0; col < numColumnsInXml; col++)
            {
                foreach (XmlElement elem in xNodelistFields)
                {
                    if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                    {
                        htControlType[elem.Attributes["Label"].Value] = elem.Attributes["ControlType"].Value;
                    }
                }
            }

            //Add the rows of Parent
            for (int row = 0; row < numRecordsInXml; row++)
            {
                //Get the corresponding RowData from the xmlStore 
                string[] rowData = xmlStore.GetRecord(row);
                //Load Records column by column
                //for (int col = 0; col < numColumnsInXml; col++)
                //{

                float[] columnWidthInPct = new float[numColumnsInXml];
                float widthTotal;
                //Set the WidthTotal Based on LandScape Value
                widthTotal = (m_ShowLandscape) ? 125 : 145;
                this.pageWidth = widthTotal;

                #region OpenBalance Header
                //Table to display the OpenBalance info..
                PdfPTable tblOpenBal = new PdfPTable(numColumnsInXml);
                tblOpenBal.DefaultCell.Border = 0;
                tblOpenBal.DefaultCell.PaddingBottom = 5f;
                tblOpenBal.DefaultCell.BackgroundColor = WHEAT;
                //Set the TotalWidth Percentage of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                {
                    tblOpenBal.WidthPercentage = 100;
                }
                else
                {
                    tblOpenBal.WidthPercentage = widthTotal * m_WidthScaleFactor;
                }
                tblOpenBal.TotalWidth = widthTotal;
                tblOpenBal.DefaultCell.BorderWidth = 0f;
                tblOpenBal.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                //Print the Opening Balacen description table.
                var colWidth = this.pageWidth / 3;
                tblOpenBal.SetWidths(new float[] { 33, 57, 17 });
                tblOpenBal.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                Chunk colChunk = new Chunk("", new Font(m_setFontName, 10F, Font.BOLD));//Receipts/Deposits
                Phrase colPhrase = new Phrase(colChunk);
                tblOpenBal.DefaultCell.Border = 0;
                tblOpenBal.AddCell(colPhrase);

                tblOpenBal.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                var beginBal = FormatText("Cal", rowData[0]);//Date
                //Get the Opening Balance.
                var openingBal = rowData[2];//Amount
                decimal openingBalance = Convert.ToDecimal(openingBal);
                var oBalCType = "Amount";
                openingBal = FormatText(oBalCType, openingBal);

                Chunk parentCellChunk = new Chunk("Beginning Balance As Of " + beginBal, new Font(m_setFontName, 10.0F, Font.BOLD));
                Phrase parentCellPhrase = new Phrase(parentCellChunk);
                tblOpenBal.DefaultCell.Border = 0;
                tblOpenBal.AddCell(parentCellPhrase);

                //Print BeginningBalance as of "Date" adn amount.
                Chunk cellChunk = new Chunk(openingBal, new Font(m_setFontName, 10.0F, Font.BOLD));
                Phrase cellPhrase = new Phrase(cellChunk);
                tblOpenBal.DefaultCell.Border = 0;
                tblOpenBal.AddCell(cellPhrase);

                pdfDocument.Add(tblOpenBal);
                #endregion

                //Load the Child/Link tables here.
                PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                spaceTable.WidthPercentage = tblOpenBal.WidthPercentage;
                spaceTable.TotalWidth = tblOpenBal.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                pdfDocument.Add(spaceTable);

                this.LoadDocument671(pdfDocument, htTreeNodeNames);

                #region Ending Balance Info table.
                PdfPTable tblEndBal = new PdfPTable(2);
                tblEndBal.SetWidths(new float[] { 80, 20 });
                tblEndBal.DefaultCell.BackgroundColor = WHEAT;
                tblEndBal.DefaultCell.PaddingBottom = 4f;  //in Point
                //Set the TotalWidth Percentage of the table
                tblEndBal.WidthPercentage = (m_WidthScaleFactor <= 0 || widthTotal == 0f) ? 100 : widthTotal * m_WidthScaleFactor;
                tblEndBal.TotalWidth = widthTotal;

                tblEndBal.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //Print the End date and Opening balance at the end.

                string endDate = FormatText("Cal", rowData[1]);
                decimal deposits = (m_dicNodeTotals.ContainsKey("Deposits")) ? m_dicNodeTotals["Deposits"] : 0;
                decimal checks = (m_dicNodeTotals.ContainsKey("Checks")) ? m_dicNodeTotals["Checks"] : 0;
                decimal adjustments = (m_dicNodeTotals.ContainsKey("Adjs")) ? m_dicNodeTotals["Adjs"] : 0;
                string endingBal = FormatText("Amount", Convert.ToString(openingBalance + deposits - checks + adjustments));

                Chunk chnkEndBal = new Chunk("Ending Balance As Of " + endDate, new Font(m_setFontName, 10F, Font.BOLD));
                Phrase phseEndBal = new Phrase(chnkEndBal);
                tblEndBal.DefaultCell.Border = 0;
                tblEndBal.AddCell(phseEndBal);
                tblEndBal.DefaultCell.Colspan = 1;

                Chunk chnkEndBalAmt = new Chunk(endingBal, new Font(m_setFontName, 10F, Font.BOLD));
                Phrase phseEndBalAmt = new Phrase(chnkEndBalAmt);
                tblEndBal.DefaultCell.Border = 0;
                tblEndBal.AddCell(phseEndBalAmt);

                pdfDocument.Add(tblEndBal);
                #endregion
            }
        }

        private static string FormatText(string controlType, string cellText)
        {
            if (controlType == "Amount")
            {
                decimal colAmount = Convert.ToDecimal(cellText);

                if (colAmount < 0)
                {
                    cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(colAmount * (-1))) + ")";
                }
                else
                {
                    cellText = ConvertToCurrencyFormat(cellText);
                }
            }
            else if (controlType == "Cal")//Customize if controlType is Calender
            {
                if (cellText != null && cellText != "")
                {
                    DateTime dateTime;
                    DateTime.TryParse(cellText, out dateTime);
                    if (dateTime != DateTime.MinValue)
                    {
                        dateTime = Convert.ToDateTime(cellText);
                        cellText = dateTime.ToString("MM/dd/yy");
                    }
                }
            }
            return cellText;
        }

        //To Insert Child Table for ReportStyle7
        private float InsertChildTable7(string parentTreeNodeName, string childTreeNodeName, string subChildTreeNode, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, int parentColCount, long Link1, int parentRowCount, PdfPTable accountPdfPTable, int spanRows)
        {
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;

            float childTblWidth = 0f;
            decimal dateRangeTotal = 0;
            string cellText = string.Empty;

            //Set the Current Page Number
            curentPageNumber = m_pdfWriter.CurrentPageNumber;
            htTotal.Clear();

            //try
            //{
            this.m_TreeNodeName = childTreeNodeName;
            //get the ColList of the ChildTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNodeName + "//GridHeading//Columns/Col");

            //Create an Instance of xmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, parentTreeNodeName, childTreeNodeName, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, m_reportStyle, Link1);
            xmlStore.IsParent = false;
            xmlStore.IsChild = true;
            xmlStore.ParentTreeNodeName = parentTreeNodeName;
            xmlStore.TreeNodeName = childTreeNodeName;
            xmlStore.ReportStyle = m_reportStyle;

            childXStore = xmlStore;

            //To Load the Records and to get the column and row Counts
            numRecordsInXml = xmlStore.LoadRecords();
            numColumnsInXml = xmlStore.Fields.Length;
            m_ChildColCount = numColumnsInXml;

            //if (numRecordsInXml > 0 && numColumnsInXml > 0)

            if (numColumnsInXml > 0)
            {
                //Increase Font Size based on column Count

                m_FontSize = 6f;

                int numColumnsInPDF = numColumnsInXml;

                //Table to load ChildColNames
                PdfPTable childPdfPTable = new PdfPTable(numColumnsInPDF);
                childPdfPTable.DefaultCell.Padding = 0.95f;  //in Points

                // Set Column Widths
                float[] columnWidthInPct = new float[numColumnsInPDF];

                //--- see if we have width data for the Fields in XmlStore
                //int widthTotal = xmlStore.GetColumnWidthsTotal();

                //Set WidthTotal
                float widthTotal;
                if (m_ShowLandscape)
                {
                    widthTotal = 79;
                }
                else
                {
                    widthTotal = 99;
                }

                this.pageWidth = widthTotal;
                //Set Col Widths
                SetChildColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                //Set the total width of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                    childPdfPTable.WidthPercentage = 100;
                else
                    childPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                childPdfPTable.TotalWidth = widthTotal;
                childPdfPTable.SetWidths(columnWidthInPct);
                m_childColWidthPct = columnWidthInPct;

                // Set Cell Attributes
                childPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                childPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;

                //Display Column Names
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    string sHdr = xmlStore.Fields[col].caption;

                    Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);

                    foreach (XmlElement elem in xNodelistFields)
                    {
                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                        {
                            if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                            {
                                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else
                            {
                                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                    }

                    childPdfPTable.AddCell(phrase);
                }

                childPdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
                childPdfPTable.DefaultCell.Border = Rectangle.TABLE;
                childPdfPTable.HeaderRows = 1;
                //pdfDocument.Add(childPdfPTable);

                //Table to Insert Space between the Rows
                PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                spaceTable.WidthPercentage = childPdfPTable.WidthPercentage;
                spaceTable.TotalWidth = childPdfPTable.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                string startMonth = string.Empty;
                string consecMonth = string.Empty;

                //Add the rows of child
                if (numRecordsInXml > 0)
                {
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetChildRecord(row);
                        XmlNodeList xRowNodeList = xmlStore.mxChildNodeListData;

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal childTblAmount = 0;
                            int fullViewLength;
                            string controlType = string.Empty;

                            cellText = rowData[col];

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        if (cellText != null && cellText != "")
                                        {
                                            childTblAmount = Convert.ToDecimal(cellText);

                                            if (childTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }
                                        }
                                        childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        dateRangeTotal = dateRangeTotal + childTblAmount;

                                        //Load the Amount to HashTable for calculating Totals
                                        if (htChildTotal[xmlStore.Fields[col].label] != null)
                                        {
                                            htChildTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htChildTotal[xmlStore.Fields[col].label]) + childTblAmount;
                                        }
                                        else
                                        {
                                            htChildTotal[xmlStore.Fields[col].label] = childTblAmount;
                                        }

                                        monthTotal = monthTotal + childTblAmount;
                                    }
                                    else
                                    {
                                        //Customize if controlType is Calender
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                    }

                                    break;
                                }
                            }

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            StringBuilder strCellTextBuilder = new StringBuilder();
                            if (controlType == "TBox")
                            {
                                //if (cellText.Length > fullViewLength)
                                if (cellText.Length > Convert.ToInt32(columnWidthInPct[col].ToString().Split('.')[0]))
                                {
                                    cellText = cellText.Substring(0, Convert.ToInt32(columnWidthInPct[col].ToString().Split('.')[0]) - 2) + "..";
                                }
                            }

                            Chunk childChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase childPhrase = new Phrase(childChunk);

                            childPdfPTable.DefaultCell.Border = 0;

                            childPdfPTable.AddCell(childPhrase);

                            childName = cellText;
                        }
                        //pdfDocument.Add(spaceTable);
                    }

                    childPdfPTable.DefaultCell.Colspan = numColumnsInXml;
                    childPdfPTable.AddCell(new Paragraph(""));
                    childPdfPTable.DefaultCell.Colspan = 1;

                    //To Display SubTotals of Child
                    string colTotalText = string.Empty;
                    decimal colTotal = 0;

                    switch (m_reportStyle)
                    {
                        case "3":
                        case "7":
                            {
                                xmlSubChildCount = numColumnsInPDF;
                                m_subChildWidthPct = m_childColWidthPct;
                                htTotal = htChildTotal;
                                subChildxStore = xmlStore;

                                XmlNodeList xSubChildNodelist;

                                xSubChildNodelist = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + m_TreeNodeName + "//GridHeading//Columns/Col");

                                Chunk childTotalTableChunk = new Chunk("Total " + parentName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase childTotalTablePhrase = new Phrase(childTotalTableChunk);
                                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                childPdfPTable.DefaultCell.Border = 0;
                                childPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                                childPdfPTable.AddCell(childTotalTablePhrase);

                                for (int col = 1; col < xmlSubChildCount; col++)
                                {
                                    foreach (XmlElement elem in xSubChildNodelist)
                                    {
                                        if (elem.Attributes["Caption"].Value == subChildxStore.Fields[col].caption)
                                        {
                                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                            {
                                                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                                if (htTotal[subChildxStore.Fields[col].label] != null && htTotal[subChildxStore.Fields[col].label].ToString() != "")
                                                {
                                                    colTotalText = htTotal[subChildxStore.Fields[col].label].ToString();
                                                    colTotal = Convert.ToDecimal(htTotal[subChildxStore.Fields[col].label]);

                                                    if (colTotal < 0)
                                                    {
                                                        colTotalText = "(" + ConvertToCurrencyFormat(Convert.ToString(colTotal * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        colTotalText = ConvertToCurrencyFormat(colTotalText);
                                                    }

                                                    if (htGrandTotal[subChildxStore.Fields[col].label] != null)
                                                    {
                                                        htGrandTotal[subChildxStore.Fields[col].label] = Convert.ToDecimal(htGrandTotal[subChildxStore.Fields[col].label]) + colTotal;
                                                    }
                                                    else
                                                    {
                                                        htGrandTotal[subChildxStore.Fields[col].label] = colTotal;
                                                    }
                                                }
                                                else
                                                {
                                                    colTotalText = "0.00";
                                                }
                                            }
                                            else
                                            {
                                                colTotalText = "";
                                            }

                                            break;
                                        }
                                    }

                                    Chunk totalChunk = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                    Phrase totalPhrase = new Phrase(totalChunk);
                                    childPdfPTable.DefaultCell.Border = 0;
                                    childPdfPTable.AddCell(totalPhrase);
                                }

                                childPdfPTable.DefaultCell.GrayFill = 1.0f;
                            }
                            break;
                    }
                }
                else
                {
                    //pdfDocument.Add(spaceTable);
                    #region NoRecords Code
                    if (m_reportStyle == "621" || m_reportStyle == "622"
                        || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
                    {
                        childPdfPTable.DefaultCell.Colspan = numColumnsInPDF;
                        childPdfPTable.DefaultCell.GrayFill = 0.95f;

                        Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase noRecordPhrase = new Phrase(noRecordChunk);

                        childPdfPTable.AddCell(noRecordPhrase);
                        childPdfPTable.DefaultCell.GrayFill = 1.0f;
                        childPdfPTable.DefaultCell.Colspan = 1;
                    }
                    #endregion
                }

                accountPdfPTable.AddCell(childPdfPTable);
                childTblWidth = childPdfPTable.TotalWidth;
            }

            return childTblWidth;
        }

        //To Insert the Specific Child of the Parent
        private float InsertChildTable(string parentTreeNodeName, string childTreeNodeName, string subChildTreeNode, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, int parentColCount, decimal balanceForward, long Link1, int parentRowCount, PdfPTable columnTypeTable)
        {
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            float childTblWidth = 0f;
            decimal dateRangeTotal = 0;
            decimal endingBalance = 0;
            string cellText = string.Empty;
            //Set the currentPageNumber
            curentPageNumber = m_pdfWriter.CurrentPageNumber;
            htTotal.Clear();
            this.m_TreeNodeName = childTreeNodeName;
            //Get the ColList of the ChildTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNodeName + "//GridHeading//Columns/Col");

            //Create an instance of XmlStore 
            ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, parentTreeNodeName, childTreeNodeName, subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, m_reportStyle, Link1);
            xmlStore.IsParent = false;
            xmlStore.IsChild = true;
            xmlStore.ParentTreeNodeName = parentTreeNodeName;
            xmlStore.TreeNodeName = childTreeNodeName;
            xmlStore.ReportStyle = m_reportStyle;
            childXStore = xmlStore;

            //Set he Nodelist to the cmlstore nodelist if report style is 651 || 653 
            if (m_reportStyle == "651" || m_reportStyle == "653")
            {
                xNodelistFields = this.mxTrxNodeListColData = xmlStore.mxTrxNodeListColData;
            }

            //To Load the Records and to get the column and row Counts
            numRecordsInXml = xmlStore.LoadRecords();
            numColumnsInXml = xmlStore.Fields.Length;
            m_ChildColCount = numColumnsInXml;
            //if (numRecordsInXml > 0 && numColumnsInXml > 0)

            if (numColumnsInXml > 0)
            {
                //Increase Font Size based on column Count
                if (numColumnsInXml > 10)
                {
                    m_FontSize = 6f;
                }
                else
                {
                    m_FontSize = 7f;
                }

                //Set Font Size for '66' series
                if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                {
                    m_FontSize = 7.0f;
                    if (m_ShowLandscape)
                        m_FontSize = 7.5f;
                }

                int numColumnsInPDF = numColumnsInXml;

                //Table to load ChildColNames
                PdfPTable childPdfPTable = new PdfPTable(numColumnsInPDF);
                childPdfPTable.DefaultCell.Padding = 0.95f;  //in Points

                //Set Column Widths
                float[] columnWidthInPct = new float[numColumnsInPDF];

                //Set widthTotal
                float widthTotal;
                if (m_ShowLandscape)
                {
                    widthTotal = 125;
                }
                else
                {
                    widthTotal = 145;
                }

                //Change Total Width based on reportStyle
                if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643" || m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                {
                    widthTotal = 140;
                }

                if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                {
                    if (parentRowCount == 0)
                    {
                        widthTotal = 145;
                    }
                    else
                    {
                        widthTotal = 140;
                    }
                }

                this.pageWidth = widthTotal;
                //Set Col Widths
                SetChildColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                //--- set the total width of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                    childPdfPTable.WidthPercentage = 100;
                else
                    childPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                childPdfPTable.TotalWidth = widthTotal;
                childPdfPTable.SetWidths(columnWidthInPct);
                m_childColWidthPct = columnWidthInPct;

                // Set Column Header Cell Attributes
                childPdfPTable.DefaultCell.BorderWidth = 0;
                childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                //To Display ColNames
                if (m_reportStyle != "641" && m_reportStyle != "642" && m_reportStyle != "643" && m_reportStyle != "661")
                {
                    for (int col = 0; col < numColumnsInXml; col++)
                    {
                        string sHdr = xmlStore.Fields[col].caption;
                        Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase phrase = new Phrase(chunk);

                        childPdfPTable.DefaultCell.Border = 0;

                        if (m_reportStyle != "200")
                        {
                            childPdfPTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                        }

                        foreach (XmlElement elem in xNodelistFields)
                        {
                            if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                            {
                                if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                                {
                                    childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                                else
                                {
                                    childPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                            }
                        }

                        if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                        {
                            if (parentRowCount == 0 && col == 0)
                            {
                                childPdfPTable.DefaultCell.BackgroundColor = Color.WHITE;
                                childPdfPTable.AddCell("");
                            }
                            else
                            {
                                childPdfPTable.AddCell(phrase);
                            }
                        }
                        else
                        {
                            childPdfPTable.AddCell(phrase);
                        }
                    }
                }

                if (numRecordsInXml > 0)
                {
                    pdfDocument.Add(childPdfPTable);
                }

                //Table to insert space between rows
                PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                spaceTable.WidthPercentage = childPdfPTable.WidthPercentage;
                spaceTable.TotalWidth = childPdfPTable.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                string startMonth = string.Empty;
                string consecMonth = string.Empty;

                //add the rows of child
                if (numRecordsInXml > 0)
                {
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        //if the PageNumber Changes update the Variable currentPageNumber and dispaly the columnHeader Table
                        if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
                        {
                            if (subChildTreeNode == null || subChildTreeNode == "" || m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                            {
                                if (m_reportStyle != "501" && m_reportStyle != "502" && m_reportStyle != "503")
                                {
                                    pdfDocument.Add(columnTypeTable);
                                }

                                if (m_reportStyle == "621")
                                {
                                    pdfDocument.Add(m_parentPdfPTable);
                                }

                                pdfDocument.Add(spaceTable);
                                curentPageNumber = m_pdfWriter.CurrentPageNumber;

                                pdfDocument.Add(childPdfPTable);
                            }
                        }

                        //Table to Display ChildRowData
                        PdfPTable childDataTable = new PdfPTable(numColumnsInPDF);
                        childDataTable.DefaultCell.Padding = 0.95f;

                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            childDataTable.WidthPercentage = 100;
                        else
                            childDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        childDataTable.TotalWidth = widthTotal;
                        childDataTable.SetWidths(columnWidthInPct);
                        childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        childDataTable.DefaultCell.GrayFill = 1.0f;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetChildRecord(row);
                        //Get the corresponding RowList from the xmlStore 
                        XmlNodeList xRowNodeList = xmlStore.mxChildNodeListData;
                        string toolTip = GetToolTipContent(xRowNodeList[row]);

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            decimal childTblAmount = 0;
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            cellText = rowData[col];

                            if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                            {
                                if (parentRowCount == 0 && col == 0)
                                {
                                    cellText = strRevenue;
                                }
                            }

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        if (cellText != null && cellText != "")
                                        {
                                            childTblAmount = Convert.ToDecimal(cellText);

                                            if (childTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }
                                        }
                                        childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        dateRangeTotal = dateRangeTotal + childTblAmount;

                                        //Load the Amount to HashTable for calculating Totals
                                        if (htChildTotal[xmlStore.Fields[col].label] != null)
                                        {
                                            htChildTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htChildTotal[xmlStore.Fields[col].label]) + childTblAmount;
                                        }
                                        else
                                        {
                                            htChildTotal[xmlStore.Fields[col].label] = childTblAmount;
                                        }
                                        monthTotal = monthTotal + childTblAmount;
                                    }
                                    else
                                    {
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                    }

                                    break;
                                }
                            }

                            if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                            {
                                if (parentRowCount == 0 && col == 0)
                                {
                                    cellText = strRevenue;
                                }
                            }




                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            StringBuilder strCellTextBuilder = new StringBuilder();
                            if (controlType == "TBox")
                            {
                                if (m_reportStyle == "621" || m_reportStyle == "622")
                                {
                                    int charLength = Convert.ToInt32(Math.Round(fullViewLength / 2.2));
                                    if (cellText.Length > charLength)
                                    {
                                        cellText = cellText.Substring(0, charLength) + "..";
                                    }
                                }
                                else
                                {
                                    if (cellText.Length > fullViewLength)
                                    {
                                        cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                    }
                                }
                            }
                            Chunk childChunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase childPhrase = new Phrase();


                            if (toolTip.Length > 0 && col == 0)
                            {
                                string annotImgUrl = m_strImagesCDNPath + "Images/spacer.gif";
                                Image annotImage = Image.GetInstance(new Uri(annotImgUrl));
                                annotImage.Annotation = new Annotation("Annotation", toolTip.Replace("~", "\n").ToString());

                                Chunk imageChunk = new Chunk(annotImage, -17, 10);
                                //Phrase imagePhrase = new Phrase(imageChunk);
                                childPhrase.Add(imageChunk);

                            }
                            childPhrase.Add(childChunk);

                            childDataTable.DefaultCell.Border = 0;
                            if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                            {
                                if (parentRowCount == 0 && col == 0)
                                {
                                    childChunk.Font = new Font(m_setFontName, m_FontSize, Font.BOLD);
                                }
                            }
                            childDataTable.AddCell(childPhrase);
                            childName = cellText;
                            //Customize for '622'
                            if (m_reportStyle == "622")
                            {
                                if (col == numColumnsInXml - 1)
                                {
                                    startMonth = xRowNodeList[row].Attributes["SubTotal1"].Value;

                                    if (row != numRecordsInXml - 1)
                                    {
                                        consecMonth = xRowNodeList[row + 1].Attributes["SubTotal1"].Value;
                                    }
                                    else
                                    {
                                        consecMonth = "";
                                    }

                                    if (startMonth != consecMonth)
                                    {
                                        monthDtRngTotal = monthTotal;
                                        monthEndBalance = balanceForward + monthDtRngTotal;
                                        m_totalDateRange = m_totalDateRange + monthDtRngTotal;

                                        string strMonthTotalCurrency = ConvertToCurrencyFormat(Convert.ToString(monthTotal));
                                        string strMonthDtRangeCurrency = ConvertToCurrencyFormat(Convert.ToString(monthDtRngTotal));
                                        string strMonthEndBalCurrency = ConvertToCurrencyFormat(Convert.ToString(monthEndBalance));

                                        if (monthTotal < 0)
                                        {
                                            strMonthTotalCurrency = "(" + ConvertToCurrencyFormat(Convert.ToString(monthTotal * (-1))) + ")";
                                        }

                                        if (monthDtRngTotal < 0)
                                        {
                                            strMonthDtRangeCurrency = "(" + ConvertToCurrencyFormat(Convert.ToString(monthDtRngTotal * (-1))) + ")";
                                        }

                                        if (monthEndBalance < 0)
                                        {
                                            strMonthEndBalCurrency = "(" + ConvertToCurrencyFormat(Convert.ToString(monthEndBalance * (-1))) + ")";
                                        }

                                        childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        childDataTable.DefaultCell.Border = 0;
                                        childDataTable.DefaultCell.GrayFill = 0.9f;
                                        childDataTable.DefaultCell.Colspan = numColumnsInPDF;

                                        Chunk monthTotalChunk = new Chunk(xRowNodeList[row].Attributes["SubTotal1Description"].Value + ": " + strMonthTotalCurrency, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                        Phrase monthTotalPhrase = new Phrase(monthTotalChunk);
                                        childDataTable.AddCell(monthTotalPhrase);
                                        childDataTable.DefaultCell.GrayFill = 1.0f;
                                        childDataTable.DefaultCell.Colspan = 1;
                                        monthTotal = 0;
                                        monthDtRngTotal = 0;
                                        monthEndBalance = 0;
                                        startMonth = consecMonth;
                                    }
                                }
                            }
                        }

                        if (m_reportStyle != "661")
                        {
                            if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643")
                            {
                                childDataTable.DefaultCell.Colspan = 1;
                            }
                            pdfDocument.Add(childDataTable);
                        }

                        //To Insert another Child Table (SubChildTable)
                        if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643" || m_reportStyle == "660" || m_reportStyle == "661")
                        {
                            //Extract Link 2Values
                            int Link2 = 0;
                            XmlNodeList parentRowsList = this.m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNodeName + "//RowList//Rows");
                            if (parentRowsList[row].Attributes["Link2"] != null)
                            {
                                Link2 = Convert.ToInt32(parentRowsList[xmlChildCount].Attributes["Link2"].Value);
                                xmlChildCount++;
                            }

                            if (subChildTreeNode != "" && Link2 != 0)
                            {
                                //Insert the SubChild Table
                                float childTableWidth = InsertSubChildTable(subChildTreeNode, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, parentColCount, Link2, columnTypeTable);
                            }
                        }

                        pdfDocument.Add(new Phrase(""));
                        //pdfDocument.Add(spaceTable);
                    }

                    //To Display SubTotals of Child
                    string colTotalText = string.Empty;
                    decimal colTotal = 0;

                    switch (m_reportStyle)
                    {
                        case "621":
                        case "622":
                            {
                                pdfDocument.Add(spaceTable);
                                PdfPTable childDataTable = new PdfPTable(numColumnsInPDF);

                                childDataTable.DefaultCell.Padding = 0.95f;

                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    childDataTable.WidthPercentage = 100;
                                else
                                    childDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                childDataTable.TotalWidth = widthTotal;
                                childDataTable.SetWidths(columnWidthInPct);

                                endingBalance = dateRangeTotal + balanceForward;
                                string dateRangeCurrency = ConvertToCurrencyFormat(Convert.ToString(dateRangeTotal));
                                string endBalanceCurrency = ConvertToCurrencyFormat(Convert.ToString(endingBalance));

                                if (dateRangeTotal < 0)
                                {
                                    dateRangeCurrency = "(" + ConvertToCurrencyFormat(Convert.ToString(dateRangeTotal * (-1))) + ")";
                                }

                                if (endingBalance < 0)
                                {
                                    endBalanceCurrency = "(" + ConvertToCurrencyFormat(Convert.ToString(endingBalance * (-1))) + ")";
                                }

                                childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                childDataTable.DefaultCell.Border = 0;
                                childDataTable.DefaultCell.GrayFill = 0.9f;

                                //Add Date Range Total
                                Chunk dtRangeChunk = new Chunk("Date Range Total: " + dateRangeCurrency, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase dtRangePhrase = new Phrase(dtRangeChunk);
                                childDataTable.DefaultCell.Colspan = numColumnsInPDF;
                                childDataTable.AddCell(dtRangePhrase);

                                //Add Ending Balance
                                Chunk balanceChunk = new Chunk("Ending Balance: " + endBalanceCurrency, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase balancePhrase = new Phrase(balanceChunk);
                                childDataTable.DefaultCell.Colspan = numColumnsInPDF;
                                childDataTable.AddCell(balancePhrase);

                                childDataTable.DefaultCell.GrayFill = 1.0f;
                                childDataTable.DefaultCell.Colspan = 1;

                                pdfDocument.Add(childDataTable);
                                //pdfDocument.Add(new Paragraph(" "));
                                pdfDocument.Add(spaceTable);
                                pdfDocument.Add(spaceTable);

                                m_totalDateRange = m_totalDateRange + dateRangeTotal;
                                break;
                            }
                        case "2":
                        case "3":
                        case "200":
                        case "400":
                        case "405":
                        case "501":
                        case "502":
                        case "503":
                        case "604":
                        case "641":
                        case "642":
                        case "643":
                        case "651":
                        case "652":
                        case "653":
                        case "660":
                        case "661":
                            {
                                pdfDocument.Add(spaceTable);
                                if (m_reportStyle == "400")
                                {
                                    parentName = "";
                                }

                                if (m_reportStyle == "1" || m_reportStyle == "2" || m_reportStyle == "3" || m_reportStyle == "200" || m_reportStyle == "400" || m_reportStyle == "405" || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503" || m_reportStyle == "601" || m_reportStyle == "604" || m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                                {
                                    xmlSubChildCount = numColumnsInPDF;
                                    m_subChildWidthPct = m_childColWidthPct;
                                    htTotal = htChildTotal;
                                    subChildxStore = xmlStore;
                                    //pdfDocument.Add(new Paragraph(" "));
                                }

                                XmlNodeList xSubChildNodelist;
                                if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643" || m_reportStyle == "651" || m_reportStyle == "653")
                                {
                                    xSubChildNodelist = this.mxTrxNodeListColData;
                                }
                                else
                                {
                                    xSubChildNodelist = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + m_TreeNodeName + "//GridHeading//Columns/Col");
                                }

                                //Table to Load the Total
                                PdfPTable childTotalTable = new PdfPTable(xmlSubChildCount);
                                childTotalTable.DefaultCell.Padding = 0.95f;
                                childTotalTable.TotalWidth = widthTotal;

                                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                                    childTotalTable.WidthPercentage = 100;
                                else
                                    childTotalTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                                childTotalTable.SetWidths(m_subChildWidthPct);

                                Chunk childTotalTableChunk = new Chunk("Total " + parentName, new Font(m_setFontName, m_FontSize, Font.BOLD));

                                if (m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
                                {
                                    childTotalTableChunk = new Chunk("Total", new Font(m_setFontName, m_FontSize, Font.BOLD));
                                }

                                Phrase childTotalTablePhrase = new Phrase(childTotalTableChunk);
                                childTotalTable.DefaultCell.Border = 0;

                                if (m_reportStyle != "200")
                                {
                                    childTotalTable.DefaultCell.BackgroundColor = LIGHT_BLUE;
                                }
                                childTotalTable.AddCell(childTotalTablePhrase);

                                for (int col = 1; col < xmlSubChildCount; col++)
                                {
                                    foreach (XmlElement elem in xSubChildNodelist)
                                    {
                                        if (elem.Attributes["Caption"].Value == subChildxStore.Fields[col].caption)
                                        {
                                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                            {
                                                childTotalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                                if (htTotal[subChildxStore.Fields[col].label] != null && htTotal[subChildxStore.Fields[col].label].ToString() != "")
                                                {
                                                    colTotalText = htTotal[subChildxStore.Fields[col].label].ToString();
                                                    colTotal = Convert.ToDecimal(htTotal[subChildxStore.Fields[col].label]);

                                                    if (colTotal < 0)
                                                    {
                                                        colTotalText = "(" + ConvertToCurrencyFormat(Convert.ToString(colTotal * (-1))) + ")";
                                                    }
                                                    else
                                                    {
                                                        colTotalText = ConvertToCurrencyFormat(colTotalText);
                                                    }

                                                    //Load the Amount to HashTable for calculating Totals
                                                    if (htGrandTotal[subChildxStore.Fields[col].label] != null)
                                                    {
                                                        htGrandTotal[subChildxStore.Fields[col].label] = Convert.ToDecimal(htGrandTotal[subChildxStore.Fields[col].label]) + colTotal;
                                                    }
                                                    else
                                                    {
                                                        htGrandTotal[subChildxStore.Fields[col].label] = colTotal;
                                                    }
                                                }
                                                else
                                                {
                                                    colTotalText = "0.00";
                                                }
                                            }
                                            else
                                            {
                                                colTotalText = "";
                                            }

                                            //Customize for '65' Series
                                            if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                                            {
                                                switch (parentRowCount)
                                                {
                                                    case 0:
                                                        {
                                                            htRevenue[xmlStore.Fields[col].label] = colTotal;
                                                            break;
                                                        }
                                                    case 1:
                                                        {
                                                            htProdCost[xmlStore.Fields[col].label] = colTotal;
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            htOtherCost[xmlStore.Fields[col].label] = colTotal;
                                                            break;
                                                        }
                                                    case 4:
                                                        {
                                                            htOtherIncome[xmlStore.Fields[col].label] = colTotal;
                                                            break;
                                                        }
                                                    case 6:
                                                        {
                                                            htTaxes[xmlStore.Fields[col].label] = colTotal;
                                                            break;
                                                        }
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    Chunk totalChunk = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                    Phrase totalPhrase = new Phrase(totalChunk);
                                    childTotalTable.DefaultCell.Border = 0;
                                    childTotalTable.AddCell(totalPhrase);
                                }

                                childTotalTable.DefaultCell.GrayFill = 1.0f;

                                if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                                {
                                    if (parentRowCount != 0)
                                    {
                                        pdfDocument.Add(childTotalTable);
                                    }
                                    pdfDocument.Add(new Paragraph(" "));
                                }
                                else
                                {
                                    pdfDocument.Add(childTotalTable);
                                    if (m_reportStyle == "400" && parentRowCount != xmlParentCount - 1)
                                    {
                                        //pdfDocument.Add(new Paragraph(" "));
                                        pdfDocument.Add(spaceTable);
                                    }
                                    else
                                    {
                                        if (m_reportStyle == "1" || m_reportStyle == "2" || m_reportStyle == "3")
                                        {
                                            if (parentRowCount != xmlParentCount - 1)
                                            {
                                                pdfDocument.Add(new Paragraph(" "));
                                            }
                                        }
                                        else
                                        {
                                            if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                                            {
                                                pdfDocument.Add(spaceTable);
                                                pdfDocument.Add(spaceTable);
                                                pdfDocument.Add(spaceTable);
                                            }
                                            else
                                            {
                                                pdfDocument.Add(new Paragraph(" "));
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                else
                {
                    pdfDocument.Add(spaceTable);
                    #region NoRecords Code
                    if (m_reportStyle == "621" || m_reportStyle == "622"
                        || m_reportStyle == "501" || m_reportStyle == "502" || m_reportStyle == "503")
                    {
                        PdfPTable childDataTable = new PdfPTable(numColumnsInPDF);
                        childDataTable.DefaultCell.Border = 0;
                        childDataTable.DefaultCell.Padding = 0.95f;

                        if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                        {
                            if (parentRowCount == 0)
                            {
                                widthTotal = 145;
                            }
                            else
                            {
                                widthTotal = 140;
                            }
                        }

                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            childDataTable.WidthPercentage = 100;
                        else
                            childDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        childDataTable.TotalWidth = widthTotal;
                        childDataTable.SetWidths(columnWidthInPct);
                        childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                        if (m_reportStyle == "651" || m_reportStyle == "652" || m_reportStyle == "653")
                        {
                            if (parentRowCount == 0)
                            {
                                childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Chunk revenueChunk = new Chunk(strRevenue, new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase revenuePhrase = new Phrase(revenueChunk);
                                childDataTable.AddCell(revenuePhrase);

                                childDataTable.DefaultCell.GrayFill = 1.00f;
                                childDataTable.DefaultCell.Colspan = numColumnsInPDF - 1;
                                childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                childDataTable.DefaultCell.GrayFill = 0.95f;

                                Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase noRecordPhrase = new Phrase(noRecordChunk);
                                childDataTable.AddCell(noRecordPhrase);
                                childDataTable.DefaultCell.GrayFill = 1.0f;
                            }
                            else
                            {
                                childDataTable.DefaultCell.Colspan = numColumnsInPDF;
                                childDataTable.DefaultCell.GrayFill = 0.95f;

                                Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                                Phrase noRecordPhrase = new Phrase(noRecordChunk);

                                childDataTable.AddCell(noRecordPhrase);
                                childDataTable.DefaultCell.GrayFill = 1.0f;
                                childDataTable.DefaultCell.Colspan = 1;
                            }
                        }
                        else
                        {
                            childDataTable.DefaultCell.Colspan = numColumnsInPDF;
                            childDataTable.DefaultCell.GrayFill = 0.95f;

                            Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase noRecordPhrase = new Phrase(noRecordChunk);

                            childDataTable.AddCell(noRecordPhrase);
                            childDataTable.DefaultCell.GrayFill = 1.0f;
                            childDataTable.DefaultCell.Colspan = 1;
                        }
                        pdfDocument.Add(childDataTable);
                        pdfDocument.Add(spaceTable);
                    }
                    #endregion
                }
                childTblWidth = childPdfPTable.TotalWidth;
            }
            return childTblWidth;
        }

        //Method to Insert another Child(Link2) to a Child(Link1)
        private float InsertSubChildTable(string subChildTreeNodeName, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, int parentColCount, int Link2, PdfPTable columnTypeTable)
        {
            int numRecordsInXml = 0;
            int numColumnsInXml = 0;
            bool bExcludeIdColumn = true;
            float childTblWidth = 0f;
            bool trxTreeNode = false;
            htSubTotal.Clear();

            this.m_TreeNodeName = subChildTreeNodeName;

            //To get the nodelist containing columns of the subChildTreeNode
            XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//GridHeading//Columns/Col");

            if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643")
            {
                trxTreeNode = true;
            }

            //Create an instance of XmlStore
            ITextXMLStore xmlStore = new ITextXMLStore(m_xDoc, subChildTreeNodeName, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, m_reportStyle, Link2, trxTreeNode);
            xmlStore.IsParent = false;
            xmlStore.TreeNodeName = subChildTreeNodeName;
            xmlStore.IsChild = false;

            //To Load the Records and to get the column and row Counts
            numRecordsInXml = xmlStore.LoadRecords();
            numColumnsInXml = xmlStore.Fields.Length;

            subChildxStore = xmlStore;
            curentPageNumber = m_pdfWriter.CurrentPageNumber;

            //Set the nodelistFields to the xmlStore NodeList if '64' series
            if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643")
            {
                xNodelistFields = this.mxTrxNodeListColData = xmlStore.mxTrxNodeListColData;
            }

            //if (numRecordsInXml > 0 && numColumnsInXml > 0)
            if (numColumnsInXml > 0)
            {
                //Increase Font Size based on column Count
                if (numColumnsInXml > 10)
                {
                    m_FontSize = 6f;
                }
                else
                {
                    m_FontSize = 7f;
                }

                //Change Font Size for '66' series
                if (m_reportStyle == "660" || m_reportStyle == "661" || m_reportStyle == "662")
                {
                    m_FontSize = 7.0f;
                    if (m_ShowLandscape)
                        m_FontSize = 7.5f;
                }

                int numColumnsInPDF = xmlSubChildCount = numColumnsInXml;

                //Table to load ColNames
                PdfPTable subChildPdfPTable = new PdfPTable(numColumnsInPDF);
                subChildPdfPTable.DefaultCell.Padding = 0.95f;  //in Points

                // Set Column Widths
                float[] columnWidthInPct = new float[numColumnsInPDF];

                //--- see if we have width data for the Fields in XmlStore
                //int widthTotal = xmlStore.DoGetColumnWidthsTotal();

                //Set TotalWidth
                float widthTotal;
                if (m_ShowLandscape)
                {
                    widthTotal = 125;
                }
                else
                {
                    widthTotal = 145;
                }

                //Change total Width for '64' series
                if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643")
                {
                    widthTotal = 135;
                }

                //Set the SubChildColWidths
                SetSubChildColWidths(xNodelistFields, xmlStore, numColumnsInPDF, columnWidthInPct, widthTotal);

                //--- set the total width of the table
                if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                    subChildPdfPTable.WidthPercentage = 100;
                else
                    subChildPdfPTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                subChildPdfPTable.TotalWidth = widthTotal;
                subChildPdfPTable.SetWidths(columnWidthInPct);
                m_subChildWidthPct = columnWidthInPct;

                // Set Column Header Cell Attributes
                subChildPdfPTable.DefaultCell.BorderWidth = 0;

                //Display ColNames
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    string sHdr = xmlStore.Fields[col].caption;
                    Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);
                    subChildPdfPTable.DefaultCell.Border = 0;

                    foreach (XmlElement elem in xNodelistFields)
                    {
                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                        {
                            if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                            {
                                subChildPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else
                            {
                                subChildPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                    }

                    subChildPdfPTable.DefaultCell.BackgroundColor = THISTLE;
                    subChildPdfPTable.AddCell(phrase);
                }

                subChildPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                if (m_reportStyle == "661")
                {
                    if (subChdColHdrCount == 0)
                    {
                        pdfDocument.Add(subChildPdfPTable);
                    }
                }
                else
                {
                    if (numRecordsInXml > 0)
                    {
                        pdfDocument.Add(subChildPdfPTable);
                    }
                }

                subChdColHdrCount = 1;

                //Table to insert Spacing between Rows 
                PdfPTable spaceTable = new PdfPTable(numColumnsInXml);
                spaceTable.WidthPercentage = subChildPdfPTable.WidthPercentage;
                spaceTable.TotalWidth = subChildPdfPTable.TotalWidth;
                spaceTable.DefaultCell.GrayFill = 1.0f;
                spaceTable.DefaultCell.Border = 0;
                spaceTable.DefaultCell.Colspan = numColumnsInXml;

                Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                Phrase spacePhrase = new Phrase(spaceChunk);
                spaceTable.AddCell(spacePhrase);

                //add the RowInformation
                if (numRecordsInXml > 0)
                {
                    //Add Rows
                    for (int row = 0; row < numRecordsInXml; row++)
                    {
                        variance = 0;
                        total = 0;

                        //if pageNumber Changes update the variable and add the ColumnHeader Table
                        if (curentPageNumber != m_pdfWriter.PageNumber)
                        {
                            pdfDocument.Add(subChildPdfPTable);
                            curentPageNumber = m_pdfWriter.CurrentPageNumber;
                        }

                        //Table to Load the rowData
                        PdfPTable childRowDataTable = new PdfPTable(numColumnsInPDF);
                        childRowDataTable.DefaultCell.Padding = 0.95f;

                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            childRowDataTable.WidthPercentage = 100;
                        else
                            childRowDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        childRowDataTable.TotalWidth = widthTotal;
                        childRowDataTable.SetWidths(columnWidthInPct);

                        childRowDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        childRowDataTable.DefaultCell.GrayFill = 1.0f;

                        //Get the corresponding RowData from the xmlStore 
                        string[] rowData = xmlStore.GetChildRecord(row);
                        //decimal budgetAmt = 0;
                        //decimal efcAmt = 0;

                        //Load Records column by column
                        for (int col = 0; col < numColumnsInXml; col++)
                        {
                            if (bExcludeIdColumn)
                                if (col == xmlStore.ColumnUID)
                                    continue;

                            decimal childTblAmount = 0;
                            string cellText = rowData[col];
                            int fullViewLength = 0;
                            string controlType = string.Empty;

                            foreach (XmlElement elem in xNodelistFields)
                            {
                                fullViewLength = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                                controlType = elem.Attributes["ControlType"].Value;

                                if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                                {
                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        //if ( xmlStore.Fields[col].label == "ProdVariance" || xmlStore.Fields[col].caption == "Total")
                                        //{
                                        //    cellText = "0";
                                        //}

                                        if (xmlStore.Fields[col].caption == "Total")
                                        {
                                            cellText = "0";
                                        }

                                        //if (xmlStore.Fields[col].label == "ProdBudget")
                                        //{
                                        //    if (cellText != "")
                                        //    {
                                        //        budgetAmt = Convert.ToDecimal(cellText);
                                        //    }
                                        //}

                                        //if (xmlStore.Fields[col].label == "ProdEFC")
                                        //{
                                        //    if (cellText != "")
                                        //    {
                                        //        efcAmt = Convert.ToDecimal(cellText);
                                        //    }
                                        //}

                                        //if (xmlStore.Fields[col].label == "ProdVariance")
                                        //{
                                        //    variance = budgetAmt - efcAmt;
                                        //    cellText = variance.ToString();
                                        //}
                                        //else
                                        //{
                                        if (xmlStore.Fields[col].caption == "Total")
                                        {
                                            cellText = total.ToString();
                                        }
                                        //}
                                        if (cellText != null && cellText != "")
                                        {
                                            childTblAmount = Convert.ToDecimal(cellText);

                                            if (childTblAmount < 0)
                                            {
                                                cellText = "(" + ConvertToCurrencyFormat(Convert.ToString(childTblAmount * (-1))) + ")";
                                            }
                                            else
                                            {
                                                cellText = ConvertToCurrencyFormat(cellText);
                                            }


                                            if (total != 0)
                                            {
                                                total = total + childTblAmount;
                                            }
                                            else
                                            {
                                                total = childTblAmount;
                                            }

                                            //Load the Amount to HashTable for calculating Totals
                                            if (htSubTotal[xmlStore.Fields[col].label] != null)
                                            {
                                                htSubTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htSubTotal[xmlStore.Fields[col].label]) + Convert.ToDecimal(childTblAmount);
                                            }
                                            else
                                            {
                                                htSubTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(childTblAmount);
                                            }
                                        }

                                        //Align Amount Data to the Right
                                        childRowDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                    else
                                    {
                                        //Customize if controlType is Calender
                                        if (elem.Attributes["ControlType"].Value == "Cal")
                                        {
                                            if (cellText != null && cellText != "")
                                            {
                                                DateTime dateTime;
                                                DateTime.TryParse(cellText, out dateTime);
                                                if (dateTime != DateTime.MinValue)
                                                {
                                                    dateTime = Convert.ToDateTime(cellText);
                                                    cellText = dateTime.ToString("MM/dd/yy");
                                                }
                                            }
                                        }
                                    }

                                    break;
                                }
                            }

                            //If Control Type is TBox, Truncate the string Text if length of the string is greater than FullViewLength 
                            if (controlType == "TBox")
                            {
                                if (cellText.Length > fullViewLength)
                                {
                                    cellText = cellText.Substring(0, fullViewLength - 2) + "..";
                                }
                            }

                            Chunk chunk = new Chunk(cellText, new Font(m_setFontName, m_FontSize));
                            Phrase phrase = new Phrase(chunk);
                            childRowDataTable.DefaultCell.Border = 0;
                            childRowDataTable.AddCell(phrase);
                        }

                        if (m_reportStyle != "661")
                        {
                            pdfDocument.Add(new Paragraph(""));
                            pdfDocument.Add(childRowDataTable);
                        }
                    }

                    //To Calculate the SubTotal of the SubChild
                    string colTotalText = string.Empty;
                    decimal colTotal = 0;

                    //Table to insert Totals
                    PdfPTable subChildTotalTable = new PdfPTable(numColumnsInPDF);
                    subChildTotalTable.DefaultCell.Padding = 0.95f;

                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        subChildTotalTable.WidthPercentage = 100;
                    else
                        subChildTotalTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    subChildTotalTable.TotalWidth = widthTotal;
                    subChildTotalTable.SetWidths(columnWidthInPct);

                    subChildTotalTable.DefaultCell.GrayFill = 0.85f;
                    subChildTotalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                    Chunk subChildTotalChunk;

                    //Customize based on reportStyles
                    if (m_reportStyle != "661")
                    {
                        subChildTotalChunk = new Chunk("Total ", new Font(m_setFontName, m_FontSize, Font.BOLD));

                        if (m_reportStyle == "641" || m_reportStyle == "642" || m_reportStyle == "643")
                        {
                            subChildTotalChunk = new Chunk("Total " + childName, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        }
                    }
                    else
                    {
                        subChildTotalTable.DefaultCell.GrayFill = 1.0f;
                        subChildTotalChunk = new Chunk(childName, new Font(m_setFontName, m_FontSize));
                    }

                    Phrase subChildTotalPhrase = new Phrase(subChildTotalChunk);
                    subChildTotalTable.DefaultCell.Border = 0;
                    subChildTotalTable.AddCell(subChildTotalPhrase);

                    for (int col = 1; col < numColumnsInPDF; col++)
                    {
                        foreach (XmlElement elem in xNodelistFields)
                        {
                            if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                            {
                                if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                {
                                    subChildTotalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                    if (htSubTotal[xmlStore.Fields[col].label] != null && htSubTotal[xmlStore.Fields[col].label].ToString() != "")
                                    {
                                        colTotalText = htSubTotal[xmlStore.Fields[col].label].ToString();
                                        colTotal = Convert.ToDecimal(htSubTotal[xmlStore.Fields[col].label]);

                                        if (colTotal < 0)
                                        {
                                            colTotalText = "(" + ConvertToCurrencyFormat(Convert.ToString(colTotal * (-1))) + ")";
                                        }
                                        else
                                        {
                                            colTotalText = ConvertToCurrencyFormat(colTotalText);
                                        }

                                        //Load the Amount to HashTable for calculating Totals
                                        if (htTotal[xmlStore.Fields[col].label] != null)
                                        {
                                            htTotal[xmlStore.Fields[col].label] = Convert.ToDecimal(htTotal[xmlStore.Fields[col].label]) + colTotal;
                                        }
                                        else
                                        {
                                            htTotal[xmlStore.Fields[col].label] = colTotal;
                                        }
                                    }
                                    else
                                    {
                                        colTotalText = "0.00";
                                    }
                                }
                                else
                                {
                                    colTotalText = "";
                                }

                                break;
                            }
                        }

                        Chunk totalChunk = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        if (m_reportStyle == "661")
                        {
                            totalChunk.Font = new Font(m_setFontName, m_FontSize);
                        }

                        Phrase totalPhrase = new Phrase(totalChunk);
                        subChildTotalTable.DefaultCell.Border = 0;
                        subChildTotalTable.AddCell(totalPhrase);
                    }

                    subChildTotalTable.DefaultCell.GrayFill = 1.0f;

                    pdfDocument.Add(subChildTotalTable);
                    if (m_reportStyle != "661")
                    {
                        pdfDocument.Add(spaceTable);
                        pdfDocument.Add(spaceTable);
                        //pdfDocument.Add(new Paragraph(""));
                    }
                }
                else
                {
                    //No Records
                    //Table to Display 'No records'
                    PdfPTable childDataTable = new PdfPTable(numColumnsInPDF);
                    childDataTable.DefaultCell.Padding = 0.95f;

                    if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                        childDataTable.WidthPercentage = 100;
                    else
                        childDataTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                    childDataTable.TotalWidth = widthTotal;
                    childDataTable.SetWidths(columnWidthInPct);

                    childDataTable.DefaultCell.Colspan = parentColCount;
                    childDataTable.DefaultCell.GrayFill = 0.95f;
                    childDataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    Chunk noRecordChunk = new Chunk("No Records", new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase noRecordPhrase = new Phrase(noRecordChunk);

                    childDataTable.AddCell(noRecordPhrase);
                    childDataTable.DefaultCell.GrayFill = 1.0f;
                    childDataTable.DefaultCell.Colspan = 1;

                    pdfDocument.Add(childDataTable);
                }

                PdfPCell rowspan = new PdfPCell();
                rowspan.Colspan = parentColCount;
                rowspan.Border = 0;

                childTblWidth = subChildPdfPTable.TotalWidth;
            }

            return childTblWidth;
        }

        //Method To Display the Table Header containing the Parent Column Names
        private void DisplayParentColNames(XmlNodeList xNodelistFields, int numColumnsInXml, ITextXMLStore xmlStore, PdfPTable parentPdfPTable)
        {
            if (m_reportStyle != "661")
            {
                for (int col = 0; col < numColumnsInXml; col++)
                {
                    string sHdr = xmlStore.Fields[col].caption;
                    Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);

                    parentPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                    parentPdfPTable.DefaultCell.BorderWidthBottom = 0.9f;

                    foreach (XmlElement elem in xNodelistFields)
                    {
                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                        {
                            if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                            {
                                parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else
                            {
                                parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                    }

                    parentPdfPTable.AddCell(phrase);
                }
            }

            pdfDocument.Add(parentPdfPTable);

            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
            {
                curentPageNumber = m_pdfWriter.CurrentPageNumber;
            }
        }

        //Method To Display Col Names of the Parent while displaying the Child Records of 405
        private void DispColNames(XmlNodeList xNodelistFields, int numColumnsInXml, ITextXMLStore xmlStore, PdfPTable parentPdfPTable)
        {
            for (int col = 0; col < numColumnsInXml; col++)
            {
                if (col == 0)
                {
                    parentPdfPTable.DefaultCell.Colspan = 2;
                }
                else
                {
                    string sHdr = xmlStore.Fields[col].caption;
                    Chunk chunk = new Chunk(sHdr, new Font(m_setFontName, m_FontSize, Font.BOLD));
                    Phrase phrase = new Phrase(chunk);

                    parentPdfPTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                    parentPdfPTable.DefaultCell.BorderWidthBottom = 0.9f;

                    foreach (XmlElement elem in xNodelistFields)
                    {
                        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
                        {
                            if (elem.Attributes["ControlType"].Value != "Calc" && elem.Attributes["ControlType"].Value != "Amount")
                            {
                                parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else
                            {
                                parentPdfPTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                    }

                    if (col > 1)
                    {
                        chunk = new Chunk("", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        phrase = new Phrase(chunk);
                    }

                    parentPdfPTable.AddCell(phrase);

                    if (col > 0)
                    {
                        parentPdfPTable.DefaultCell.Colspan = 1;
                    }
                }
            }

            pdfDocument.Add(parentPdfPTable);

            if (curentPageNumber != m_pdfWriter.CurrentPageNumber)
            {
                curentPageNumber = m_pdfWriter.CurrentPageNumber;
            }
        }

        //To Set the Respective ColWidths of the Parent
        private void SetParentColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);

                if (m_reportStyle == "621" || m_reportStyle == "622")
                {
                    widthCol = widthCol * 2.2f;
                    columnWidthInPct[col] = widthCol;
                }
                else
                {
                    columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
                }
            }

            #region Commented ParentColWidths
            //int tBoxControls = 0;
            //int otherControls = 0;

            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                tBoxControls++;
            //            }
            //            else
            //            {
            //                otherControls++;
            //            }
            //        }

            //    }
            //}

            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "1":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description")
            //                            {
            //                                widthCol = 8;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 14;
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "10":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description")
            //                            {
            //                                widthCol = 40;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 80;
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "400":
            //                        {
            //                            widthCol = 17;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "501":
            //                    case "502":
            //                    case "503":
            //                        {
            //                            if (elem.Attributes["Label"].Value == "Vendor")
            //                            {
            //                                widthCol = 50;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 95;
            //                            }

            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "601":
            //                    case "602":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description")
            //                            {
            //                                widthCol = 13;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 42.95f;
            //                            }
            //                            //widthCol = 26.25f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "604":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description")
            //                            {
            //                                widthCol = 12;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 33;
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "670":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "MemoPayee")
            //                            {
            //                                widthCol = 15;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 67;
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                }
            //            }
            //            else
            //            {
            //                //float widthCol = widthTotal / numColumnsInPDF;
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "1":
            //                        {
            //                            widthCol = 9.133f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "10":
            //                        {
            //                            widthCol = 25;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "601":
            //                        {
            //                            widthCol = 14.8f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "602":
            //                        {
            //                            widthCol = 22.2f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "670":
            //                        {
            //                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
            //                            {
            //                                widthCol = 25;
            //                            }
            //                            else
            //                            {
            //                                if (elem.Attributes["ControlType"].Value == "Cal")
            //                                {
            //                                    widthCol = 13;
            //                                }
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                }
            //            }
            //            break;
            //        }
            //    }
            //}
            #endregion
        }

        //To Set the Respective ColWidths of the OrderBy Report
        private void SetOrderByColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal, string strOrderByCol, Hashtable htColNameValues)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                if (xmlStore.Fields[col].label != Convert.ToString(htColNameValues[strOrderByCol]))
                {
                    totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                }
            }

            int currentCol = 0;
            for (int col = 0; col < numColumnsInPDF; col++)
            {
                if (xmlStore.Fields[col].label != strOrderByCol)// htColNameValues[strOrderByCol].ToString())
                {
                    widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                    columnWidthInPct[currentCol] = this.pageWidth * (widthCol / totalFullVwLength);
                    currentCol++;
                }
            }
        }

        //To Set the Respective ColWidths of the Child
        private void SetChildColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);

                if (m_reportStyle == "621" || m_reportStyle == "622")
                {
                    widthCol = widthCol / 2.2f;
                    columnWidthInPct[col] = widthCol;
                }
                else
                {
                    columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
                }
            }

            #region Commented SetChildColWidths
            //int tboxControls = 0;
            //int otherControls = 0;
            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                tboxControls++;
            //            }
            //            else
            //            {
            //                otherControls++;
            //            }
            //        }

            //    }
            //}

            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "3":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description" && elem.Attributes["Label"].Value != "Job")
            //                            {
            //                                widthCol = 13;
            //                            }
            //                            else
            //                            {
            //                                if (elem.Attributes["Label"].Value == "Job")
            //                                {
            //                                    widthCol = 26;
            //                                }
            //                                else
            //                                {
            //                                    widthCol = 32;
            //                                }
            //                            }

            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "400":
            //                        {
            //                            switch (elem.Attributes["Label"].Value)
            //                            {
            //                                default:
            //                                    {
            //                                        widthCol = 12;
            //                                        break;
            //                                    }
            //                                case "PayeePayor":
            //                                    {
            //                                        widthCol = 34;
            //                                        break;
            //                                    }
            //                                case "ObjectType":
            //                                    {
            //                                        widthCol = 16;
            //                                        break;
            //                                    }
            //                                case "VoucherRef":
            //                                    {
            //                                        widthCol = 14;
            //                                        break;
            //                                    }
            //                                case "Description":
            //                                    {
            //                                        widthCol = 30;
            //                                        break;
            //                                    }
            //                            }

            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "501":
            //                        {
            //                            switch (elem.Attributes["Label"].Value)
            //                            {
            //                                default:
            //                                    {
            //                                        widthCol = 14;
            //                                        break;
            //                                    }
            //                                case "Description":
            //                                    {
            //                                        widthCol = 25;
            //                                        break;
            //                                    }
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "502":
            //                    case "503":
            //                        {
            //                            switch (elem.Attributes["Label"].Value)
            //                            {
            //                                default:
            //                                    {
            //                                        widthCol = 10;
            //                                        break;
            //                                    }
            //                                case "Description":
            //                                    {
            //                                        widthCol = 20;
            //                                        break;
            //                                    }
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "604":
            //                        {
            //                            widthCol = 55;
            //                            columnWidthInPct[col] = widthCol;

            //                            break;
            //                        }
            //                    case "621":
            //                    case "622":
            //                        {
            //                            if (elem.Attributes["Label"].Value != "Description" && elem.Attributes["Label"].Value != "Job")
            //                            {
            //                                widthCol = 12;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 46;
            //                            }

            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                }
            //            }
            //            else
            //            {
            //                //float widthCol = widthTotal / numColumnsInPDF;
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "3":
            //                        {
            //                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
            //                            {
            //                                widthCol = 14;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 10.6f;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            break;
            //                        }
            //                    case "400":
            //                        {
            //                            if (elem.Attributes["ControlType"].Value == "Cal")
            //                            {
            //                                widthCol = 10.5f;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 14.5f;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            break;
            //                        }
            //                    case "501":
            //                        {
            //                            if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
            //                            {
            //                                widthCol = 12.05f;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 11.22f;
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "502":
            //                    case "503":
            //                        {
            //                            switch (elem.Attributes["Label"].Value)
            //                            {
            //                                default:
            //                                    {
            //                                        widthCol = 10;
            //                                        break;
            //                                    }
            //                                case "Amount":
            //                                    {
            //                                        widthCol = 11.25f;
            //                                        break;
            //                                    }
            //                                case "Cal":
            //                                    {
            //                                        widthCol = 10;
            //                                        break;
            //                                    }
            //                            }
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "604":
            //                        {
            //                            widthCol = 15.8f;
            //                            columnWidthInPct[col] = widthCol;

            //                            break;
            //                        }
            //                    case "622":
            //                        {
            //                            widthCol = 15.5f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// Method to determine the column widths in presence of a tooltip field.
        /// </summary>
        /// <param name="xNodelistFields"></param>
        /// <param name="xmlStore"></param>
        /// <param name="numColumnsInPDF"></param>
        /// <param name="columnWidthInPct"></param>
        /// <param name="widthTotal"></param>
        private float[] GetChildColWidthsWithTip(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;
            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            totalFullVwLength = totalFullVwLength - 10;//Deduct the amount used by the ToolTip field.
            var columnWidthInPct = new float[numColumnsInPDF + 1];//Explicityly override and increment the array length by one.
            columnWidthInPct[0] = 10;
            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                columnWidthInPct[col + 1] = this.pageWidth * (widthCol / totalFullVwLength);
            }
            return columnWidthInPct;
        }

        //To Set the Respective ColWidths of the SubChild
        private void SetSubChildColWidths(XmlNodeList xNodelistFields, ITextXMLStore xmlStore, int numColumnsInPDF, float[] columnWidthInPct, float widthTotal)
        {
            float widthCol = 0;
            float totalFullVwLength = 0;

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                totalFullVwLength = totalFullVwLength + Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
            }

            for (int col = 0; col < numColumnsInPDF; col++)
            {
                widthCol = Convert.ToInt32(xmlStore.Fields[col].fullViewLength);
                columnWidthInPct[col] = this.pageWidth * (widthCol / totalFullVwLength);
            }

            #region Commented SetSubChildColWidths
            //int tboxControls = 0;
            //int otherControls = 0;
            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                tboxControls++;
            //            }
            //            else
            //            {
            //                otherControls++;
            //            }
            //        }

            //    }
            //}

            //for (int col = 0; col < numColumnsInPDF; col++)
            //{
            //    float widthCol = 0;
            //    foreach (XmlElement elem in xNodelistFields)
            //    {
            //        if (elem.Attributes["Label"].Value == xmlStore.Fields[col].label)
            //        {
            //            if (elem.Attributes["ControlType"].Value == "TBox")
            //            {
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "641":
            //                    case "642":
            //                    case "643":
            //                        {
            //                            widthCol = 105;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "660":
            //                    case "661":
            //                        {
            //                            if (elem.Attributes["Caption"].Value == "Account")
            //                            {
            //                                widthCol = 12;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            else
            //                            {
            //                                widthCol = 6;
            //                                columnWidthInPct[col] = widthCol;
            //                            }
            //                            widthTotal = widthTotal - widthCol;

            //                            break;
            //                        }
            //                }
            //            }
            //            else
            //            {
            //                //float widthCol = widthTotal / numColumnsInPDF;
            //                switch (m_reportStyle)
            //                {
            //                    default:
            //                        {
            //                            widthCol = widthTotal / numColumnsInPDF;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "641":
            //                    case "642":
            //                    case "643":
            //                        {
            //                            widthCol = 30;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                    case "660":
            //                        {
            //                            widthCol = 10.45f;
            //                            columnWidthInPct[col] = widthCol;
            //                            break;
            //                        }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private void SetAccountColWidths(int numColumnsInXml, float[] columnWidthInPct, ITextXMLStore xmlStore)
        {
            if (m_reportStyle == "2")
            {
                numColumnsInXml = 12;
            }

            if (m_reportStyle == "3" || m_reportStyle == "7")
            {
                numColumnsInXml = 8;
            }

            for (int col = 0; col < numColumnsInXml; col++)
            {
                if (col == 0)
                {
                    switch (m_reportStyle)
                    {
                        default:
                            {
                                columnWidthInPct[col] = 16;
                                break;
                            }
                        case "2":
                        case "3":
                            {
                                columnWidthInPct[col] = 16f;
                                break;
                            }
                        case "7":
                            {
                                columnWidthInPct[col] = 47f;
                                break;
                            }
                        case "400":
                            {
                                columnWidthInPct[col] = 5.5f;
                                break;
                            }
                    }
                }
                else
                {
                    if (col == 1)
                    {
                        columnWidthInPct[col] = 0;
                    }
                    else
                    {
                        switch (m_reportStyle)
                        {
                            default:
                                {
                                    columnWidthInPct[col] = 12.9f;
                                    break;
                                }
                            case "2":
                                {
                                    columnWidthInPct[col] = 11.727f;
                                    break;
                                }
                            case "3":
                                {
                                    columnWidthInPct[col] = 18.48f;
                                    break;
                                }
                            case "7":
                                {
                                    columnWidthInPct[col] = 16.33f;
                                    break;
                                }
                            case "670":
                                {
                                    if (col == numColumnsInXml - 2)
                                    {
                                        columnWidthInPct[col] = 3;
                                    }
                                    else
                                    {
                                        if (col == numColumnsInXml - 1)
                                        {
                                            columnWidthInPct[col] = 12.9f;
                                        }
                                        else
                                        {
                                            columnWidthInPct[col] = 14.13f;
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }

        //To Calculate the Grand Total for the Entire Document
        private void ComputeGrandTotal(int parentColCount, PdfPTable parentPdfPTable)
        {
            string colTotalText = string.Empty;
            decimal colTotal = 0;
            //pdfDocument.Add(new Paragraph(" "));

            float widthTotal;
            if (m_ShowLandscape)
            {
                widthTotal = 125;
            }
            else
            {
                widthTotal = 145;
            }

            switch (m_reportStyle)
            {
                //Setting GrandTotals
                case "621":
                case "622":
                    {
                        PdfPTable grandTotalTable = new PdfPTable(m_ChildColCount);
                        grandTotalTable.DefaultCell.Padding = 0.95f;
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            grandTotalTable.WidthPercentage = 100;
                        else
                            grandTotalTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        grandTotalTable.TotalWidth = widthTotal;
                        grandTotalTable.SetWidths(m_childColWidthPct);

                        grandTotalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        grandTotalTable.DefaultCell.Colspan = m_ChildColCount;
                        grandTotalTable.DefaultCell.Border = 0;
                        //parentPdfPTable.DefaultCell.GrayFill = 0.9f;
                        grandTotalTable.DefaultCell.BackgroundColor = WHEAT;

                        string tBalanceFwd = ConvertToCurrencyFormat(Convert.ToString(m_totalBalanceForward));
                        string tDateRange = ConvertToCurrencyFormat(Convert.ToString(m_totalDateRange));

                        decimal tEndBal = m_totalBalanceForward + m_totalDateRange;
                        string tEndingBalance = ConvertToCurrencyFormat(Convert.ToString(m_totalBalanceForward + m_totalDateRange));

                        if (m_totalBalanceForward < 0)
                        {
                            tBalanceFwd = "(" + ConvertToCurrencyFormat(Convert.ToString(m_totalBalanceForward * (-1))) + ")";
                        }

                        if (m_totalDateRange < 0)
                        {
                            tDateRange = "(" + ConvertToCurrencyFormat(Convert.ToString(m_totalDateRange * (-1))) + ")";
                        }

                        if (tEndBal < 0)
                        {
                            tEndingBalance = "(" + ConvertToCurrencyFormat(Convert.ToString(tEndBal * (-1))) + ")";
                        }

                        Chunk tBalChunk = new Chunk("Total Balance Forward: " + tBalanceFwd, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tBalPhrase = new Phrase(tBalChunk);
                        grandTotalTable.AddCell(tBalPhrase);

                        Chunk tDateRangeChunk = new Chunk("Total Date Range: " + tDateRange, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tDateRangePhrase = new Phrase(tDateRangeChunk);
                        grandTotalTable.AddCell(tDateRangePhrase);

                        Chunk tEndBalanceChunk = new Chunk("Total Ending Balance: " + tEndingBalance, new Font(m_setFontName, m_FontSize, Font.BOLD));
                        Phrase tEndBalancePhrase = new Phrase(tEndBalanceChunk);
                        grandTotalTable.AddCell(tEndBalancePhrase);

                        grandTotalTable.DefaultCell.GrayFill = 1.0f;
                        grandTotalTable.DefaultCell.Colspan = 1;

                        pdfDocument.Add(grandTotalTable);
                        break;
                    }
                case "1":
                case "400":
                case "501":
                case "502":
                case "503":
                case "601":
                case "602":
                case "604":
                case "660":
                case "661":
                case "670":
                    {
                        //For the Above Respective report styles set the subchild count,width,xmlstore,grandtotal to repective parent details
                        if (m_reportStyle == "1" || m_reportStyle == "400" || m_reportStyle == "601" || m_reportStyle == "602" || m_reportStyle == "670")
                        {
                            xmlSubChildCount = parentColCount;
                            m_subChildWidthPct = m_ParentColWidthPct;
                            subChildxStore = parentXStore;
                            htGrandTotal = htParentTotal;

                            if (m_reportStyle != "670")
                            {
                                pdfDocument.Add(new Paragraph(" "));
                            }
                        }

                        XmlNodeList xNodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + m_TreeNodeName + "//GridHeading//Columns/Col");

                        PdfPTable grandTotalTable = new PdfPTable(xmlSubChildCount);
                        grandTotalTable.DefaultCell.Padding = 0.95f;
                        if (m_WidthScaleFactor <= 0 || widthTotal == 0f)
                            grandTotalTable.WidthPercentage = 100;
                        else
                            grandTotalTable.WidthPercentage = widthTotal * m_WidthScaleFactor;

                        grandTotalTable.TotalWidth = widthTotal;
                        grandTotalTable.SetWidths(m_subChildWidthPct);

                        //Customize if ReportStyle 670
                        Chunk grandTotalTableChunk;
                        if (m_reportStyle == "670")
                        {
                            grandTotalTableChunk = new Chunk("Total ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        }
                        else
                        {
                            grandTotalTableChunk = new Chunk("Grand Total ", new Font(m_setFontName, m_FontSize, Font.BOLD));
                        }

                        Phrase grandTotalTablePhrase = new Phrase(grandTotalTableChunk);
                        //grandTotalTable.DefaultCell.GrayFill = 0.75f;
                        grandTotalTable.DefaultCell.BackgroundColor = WHEAT;
                        grandTotalTable.DefaultCell.Border = 0;
                        grandTotalTable.AddCell(grandTotalTablePhrase);

                        //Load Totals column by column
                        for (int col = 1; col < xmlSubChildCount; col++)
                        {
                            foreach (XmlElement elem in xNodelistFields)
                            {
                                if (elem.Attributes["Label"].Value == subChildxStore.Fields[col].label)
                                {
                                    if (elem.Attributes["ControlType"].Value == "Amount" || elem.Attributes["ControlType"].Value == "Calc")
                                    {
                                        grandTotalTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        if (htGrandTotal[subChildxStore.Fields[col].label] != null && htGrandTotal[subChildxStore.Fields[col].label].ToString() != "")
                                        {
                                            colTotalText = htGrandTotal[subChildxStore.Fields[col].label].ToString();
                                            colTotal = Convert.ToDecimal(htGrandTotal[subChildxStore.Fields[col].label]);

                                            if (colTotal < 0)
                                            {
                                                colTotalText = "(" + ConvertToCurrencyFormat(Convert.ToString(colTotal * (-1))) + ")";
                                            }
                                            else
                                            {
                                                colTotalText = ConvertToCurrencyFormat(colTotalText);
                                            }
                                        }
                                        else
                                        {
                                            colTotalText = "0.00";
                                        }
                                    }
                                    else
                                    {
                                        colTotalText = "";
                                    }

                                    break;
                                }
                            }

                            Chunk grandTotalChunk = new Chunk(colTotalText, new Font(m_setFontName, m_FontSize, Font.BOLD));
                            Phrase grandTotalPhrase = new Phrase(grandTotalChunk);
                            grandTotalTable.DefaultCell.Border = 0;
                            grandTotalTable.AddCell(grandTotalPhrase);
                        }

                        grandTotalTable.DefaultCell.GrayFill = 1.0f;
                        pdfDocument.Add(grandTotalTable);

                        //For spacing between rows
                        PdfPTable spaceTable = new PdfPTable(xmlSubChildCount);
                        spaceTable.WidthPercentage = grandTotalTable.WidthPercentage;
                        spaceTable.TotalWidth = grandTotalTable.TotalWidth;
                        spaceTable.DefaultCell.GrayFill = 1.0f;
                        spaceTable.DefaultCell.Border = 0;
                        spaceTable.DefaultCell.Colspan = xmlSubChildCount;

                        Chunk spaceChunk = new Chunk("", new Font(m_setFontName, m_FontSize));
                        Phrase spacePhrase = new Phrase(spaceChunk);
                        spaceTable.AddCell(spacePhrase);

                        if (m_reportStyle == "400" || m_reportStyle == "670")
                        {
                            pdfDocument.Add(new Paragraph(" "));
                        }
                        break;
                    }
            }
        }

        //Generate Data Table for Report Header
        private DataTable GetReportHeader(XmlDocument xDoc, string m_TreeNodeName)
        {
            string title = string.Empty;
            XmlNode nodeTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_TreeNodeName + "/GridHeading/Title");
            if (nodeTitle != null)
            {
                title = nodeTitle.InnerText.Trim().ToString();
            }

            string subTitle = string.Empty;
            XmlNode nodeSubTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_TreeNodeName + "/GridHeading/SubTitle");
            if (nodeSubTitle != null)
            {
                subTitle = nodeSubTitle.InnerText.Trim().ToString();
            }

            DataTable dtHeader = new DataTable("Header");
            dtHeader = GetHeaderDT(title, subTitle);
            return dtHeader;
        }

        private DataTable GetHeaderDT(string title, string subTitle)
        {
            XmlDocument XDocUserInfo = new XmlDocument();
            DataTable HeaderDT = new DataTable();
            for (int col = 0; col < 4; col++)
            {
                DataColumn colNames = new DataColumn();
                colNames.ColumnName = "";
                HeaderDT.Columns.Add(colNames);
            }
            //
            DataRow dCompanyRow = HeaderDT.NewRow();
            DataRow dHeaderRow = HeaderDT.NewRow();
            //
            CommonUI commonObjUI = new CommonUI();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string userName = string.Empty;
            //
            XmlNode nodeSmallName = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/SmallName");
            if (nodeSmallName != null)
            {
                userName = nodeSmallName.InnerText.Trim().ToString().ToUpper();
            }
            dCompanyRow[0] = "Requested By : " + userName;
            dCompanyRow[1] = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;
            //
            XmlNode nodeGMT = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/GMTAdjustment");
            XmlNode nodeDayLight = XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/DayLightSaving");
            int DayLightSavingVal = Convert.ToInt32(nodeDayLight.InnerText);
            string gmtValue = nodeGMT.InnerText.ToString();
            int gmtHr = 0;
            int gmtMin = 0;

            if (gmtValue == "0")
            {
                gmtValue = ConfigurationManager.AppSettings["DefGMT"].ToString();
            }

            if (gmtValue.Contains(":"))
            {
                string[] splitTime = new string[2];
                splitTime = gmtValue.Split(':');
                //
                gmtHr = Convert.ToInt32(splitTime[0].ToString());
                gmtMin = Convert.ToInt32(splitTime[1].ToString());
            }
            else if (gmtValue.Contains("."))
            {
                string[] splitTime = new string[2];
                splitTime = gmtValue.Split('.');
                //
                gmtHr = Convert.ToInt32(splitTime[0].ToString());
                gmtMin = Convert.ToInt32(splitTime[1].ToString());
            }
            else
            {
                gmtHr = Convert.ToInt32(gmtValue);
                gmtMin = 0;
            }
            // condition for DayLightSaving
            if (DayLightSavingVal > 0)
            {
                gmtHr = gmtHr - DayLightSavingVal;
            }
            DateTime dtGMT = DateTime.Now.ToUniversalTime() + new TimeSpan(gmtHr, gmtMin, 0);
            dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + dtGMT.Hour + ":" + dtGMT.Minute + ":00";

            #region coomented
            //DateTime dtGMT = DateTime.UtcNow;
            //if (nodeGMT != null)
            //{
            //    string gmtValue = nodeGMT.InnerText.ToString();
            //    int curMM = 0;
            //    int gmtMM = 0;
            //    int remMM = 0;
            //    TimeSpan timeSpan = new TimeSpan();
            //    if (gmtValue.Contains(":"))
            //    {
            //        string[] splitTime = new string[2];
            //        splitTime = gmtValue.Split(':');
            //        //
            //        int splitTimeHH = Convert.ToInt32(splitTime[0].ToString());
            //        int splitTimeMM = Convert.ToInt32(splitTime[1].ToString());
            //        //
            //        string splittedValues = string.Empty;

            //        if (splitTimeHH.ToString().Contains("-"))
            //        {
            //            splittedValues = splitTimeHH.ToString().Replace("-", "");
            //            gmtMM = Convert.ToInt32(splittedValues) * 60 + splitTimeMM;
            //            curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //            remMM = gmtMM - curMM;
            //            if (remMM.ToString().Contains("-"))
            //            {
            //                remMM = Convert.ToInt32(remMM.ToString().Replace("-", ""));
            //            }
            //        }
            //        else
            //        {
            //            gmtMM = Convert.ToInt32(splitTimeHH) * 60 + Convert.ToInt32(splitTimeMM);
            //            curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //            remMM = gmtMM + curMM;
            //        }
            //    }
            //    else
            //    {
            //        curMM = Convert.ToInt32(dtGMT.Hour) * 60 + Convert.ToInt32(dtGMT.Minute);
            //        gmtMM = Convert.ToInt32(gmtValue) * 60;
            //        remMM = gmtMM + curMM;
            //    }
            //    timeSpan = TimeSpan.FromMinutes(remMM);
            //    // gives you the rounded down value 
            //    int hours = timeSpan.Hours;
            //    // gives you the minutes left of the hour
            //    int minutes = remMM - (hours * 60);
            //    string mins = string.Empty;
            //    if (minutes.ToString().Length == 1)
            //    {
            //        mins = "0" + Convert.ToString(minutes);
            //    }
            //    else
            //    {
            //        mins = Convert.ToString(minutes);
            //    }
            //    dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + hours + ":" + mins + ":00";
            //}
            //else
            //{
            //    dCompanyRow[2] = "Date Time: " + dtGMT.ToShortDateString() + " " + dtGMT.Hour.ToString() + ":" + dtGMT.Minute.ToString() + ":" + dtGMT.Second.ToString();
            //}
            #endregion
            //
            if (title != null)
            {
                dHeaderRow[1] = title;
            }
            else
            {
                dHeaderRow[1] = string.Empty;
            }
            HeaderDT.Rows.Add(dCompanyRow);
            HeaderDT.Rows.Add(dHeaderRow);
            //
            if (subTitle != string.Empty)
            {
                DataRow dHeaderTimeRow = HeaderDT.NewRow();
                dHeaderTimeRow[1] = subTitle;
                HeaderDT.Rows.Add(dHeaderTimeRow);
            }
            return HeaderDT;
        }

        private Rectangle ConvertToPageSize(PaperSize enPaperSize)
        {
            Rectangle pageSize;
            switch (enPaperSize)
            {
                default:
                    pageSize = PageSize.LETTER;
                    break;
                case PaperSize.LegalUS:
                    pageSize = PageSize.LEGAL;
                    break;
                case PaperSize.A4:
                    pageSize = PageSize.A4;
                    break;
            }

            return pageSize;
        }

        private int GetFontName(string fName)
        {
            int m_FName;
            switch (fName)
            {
                default:
                case "Arial":
                    {
                        m_FName = Font.HELVETICA;
                        break;
                    }
                case "Times":
                    {
                        m_FName = Font.TIMES_ROMAN;
                        break;
                    }
                case "Courier":
                    {
                        m_FName = Font.COURIER;
                        break;
                    }
            }

            return m_FName;
        }

        private void DrawLine(float xMoveTo, float yMoveTo, float xLineTo, float yLineTo)
        {
            PdfContentByte contentByte = m_pdfWriter.DirectContent;

            contentByte.MoveTo(xMoveTo, yMoveTo);
            contentByte.LineTo(xLineTo, yMoveTo);
            contentByte.Stroke();

            Phrase rowSpan = new Phrase("\n");
            pdfDocument.Add(rowSpan);
        }

        #region Commented Page Event Handler Code
        //private void DoConfigPageEventHandler(XmlStoreEvent pageEvent)
        //{
        //    //page caption (in header)
        //    if (this.ShowTitle)
        //    {
        //        pageEvent.PageTitle = this.m_DocTitle;
        //    }

        //    // --- page number (in footer)
        //    if (this.ShowPageNumber)
        //    {
        //        pageEvent.PageNumberFormat = "Page {0}"; //default
        //        pageEvent.PageNumberStartingValue = 0;
        //    }

        //    if (this.ShowWatermark)
        //    {
        //        pageEvent.WatermarkFile = "";
        //    }
        //}
        #endregion

        #region Commented Code
        //private int DoCvtToStyle(bool bBold, bool bItalics)
        //{
        //    int fontStyle = 0;

        //    if (bBold)
        //        fontStyle |= Font.BOLD;

        //    if (bItalics)
        //        fontStyle |= Font.ITALIC;

        //    return fontStyle;
        //}
        #endregion

        #endregion
    }
}

