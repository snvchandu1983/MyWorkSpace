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
using System.IO;
using LAjit_BO;
using LAjitDev.Classes;
using NLog;


namespace LAjitDev.Common
{
    public partial class Help : System.Web.UI.Page
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();
        CommonBO objBO = new CommonBO();

        private static string m_HelpFilesPath = ConfigurationManager.AppSettings["HelpFilesPath"];
        private static string m_HelpVirtualPath = ConfigurationManager.AppSettings["HelpVirtualPath"];

        private static string m_AttachmentsPath; 
        private static string m_AttachmentsVirtualPath; 

        protected void Page_Load(object sender, EventArgs e)
        {

            AddScriptReferences();

            if (!Page.IsPostBack)
            {
            
                 m_AttachmentsPath = ConfigurationManager.AppSettings["AttachmentsPath"]  + "/" + HttpContext.Current.Session["CompanyEntityID"].ToString();
                 m_AttachmentsVirtualPath = ConfigurationManager.AppSettings["AttachmentsVirtualPath"] + HttpContext.Current.Session["CompanyEntityID"].ToString() + "/";


                SetUIHeaders(460, 500);
                preloadImages();
                BindTable(string.Empty);
            }
            if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            //changing masterpage file dynamically
            string masterfile = string.Empty;
            if (Page.Request.QueryString["PopUp"] != null && Page.Request.QueryString["PopUp"] != string.Empty)
            {
                masterfile = "../MasterPages/PopUp.Master";
            }
            else
            {
                masterfile = "../MasterPages/TopLeft.Master";
            }
            if (!masterfile.Equals(string.Empty))
            {
                base.MasterPageFile = masterfile;
            }
            base.OnPreInit(e);

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            if (XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml != null)
            {
                //string keyTheme = System.Configuration.ConfigurationManager.AppSettings["AssignedTheme"].ToString();
                string keyTheme = "430";
                if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo") != null)
                {
                    string theme = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                    Session.Add("MyTheme", theme);
                }
                Page.Theme = ((string)HttpContext.Current.Session["MyTheme"]);
            }

        }

        //protected void Page_PreRender(object sender, EventArgs e)
        //{

        //     //SetUIHeaders(460, 500);

        // }

        protected void imgbtnGo_Click(object sender, ImageClickEventArgs e)
        {
            if ((hdnAutoFillBPGID.Value != string.Empty) && (txthelpcatalog.Text != string.Empty))
            {

                BindTable(hdnAutoFillBPGID.Value);
            }
            else
            {
                BindTable(string.Empty);
            }

        }



        #region Private Methods


        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //jquery-1.3.2.min.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "ParentChild.js");
            Page.Controls.Add(hgcScript1);
        }



        private void SetPanelHeading(HtmlTableCell htcWork, string gridTitle)
        {
            #region NLog
            logger.Info("Sets the title of the given grid view with : " + gridTitle);
            #endregion

            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // Need a bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(gridTitle, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;  //+ 20;
            htcWork.InnerText = gridTitle;
            htcWork.Height = "24px";
            htcWork.Width = intScrollLength.ToString();
            objGraphics.Dispose();
            objBitmap.Dispose();
        }

        private void SetUIHeaders(int entryFormWidth, int entryFormHeight)
        {
            //return;
            ////int entryFormWidth = 460; //395;    //925;// 860 IframeWidth -SpacerWidth-Vertical scrollbar width.
            ////int entryFormHeight = 483;//-24 for the title element


            //string strDepth = pnlEntryForm.Page.Request.QueryString["depth"];
            //if (!string.IsNullOrEmpty(strDepth))
            //{
            //    int depth = Convert.ToInt32(strDepth);
            //    if (depth != 1 && depth != 0)
            //    {
            //        depth--;
            //        //entryFormWidth = entryFormWidth - (depth * 30);
            //        //entryFormHeight = entryFormHeight - (depth * 30);
            //        entryFormWidth = entryFormWidth - 12;
            //    }
            //}

            ////Setting the header widths
            //SetPanelHeading(htcCPGV1, "Help");
            //htcCPGV1.Width = "100"; //Convert.ToString(entryFormWidth - (Convert.ToInt32(htcCPGV1.Width)+250));
            //htcEntryFormAuto.Width = Convert.ToString(entryFormWidth - Convert.ToInt32(htcCPGV1.Width) - 31);

            ////Set the heights of Entry Form

            //pnlEntryForm.Height = Unit.Pixel(entryFormHeight);
            //pnlEntryForm.Width = Unit.Pixel(entryFormWidth);

            //pnlEntryFormTitle.Width = Unit.Pixel(entryFormWidth);
            //tblEntryFormTitle.Width = Convert.ToString(entryFormWidth);
            //tblEntryForm.Width = Convert.ToString(entryFormWidth);

        }


        private void BindTable(string strBPGID)
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string HelpBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Help']").Attributes["BPGID"].Value;
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string strBPINFO = string.Empty;

            //logo url binding
            bindlogo();

            //SET BPINFO  Session["BPINFO"] 
            if (Session["LinkBPinfo"] != null)
            {
                string BPGID = string.Empty;
                XmlDocument xDocformBPGID = new XmlDocument();
                xDocformBPGID.LoadXml(Session["LinkBPinfo"].ToString());

                XmlNode nodeBPGID = xDocformBPGID.SelectSingleNode("bpinfo/BPGID");
                BPGID = nodeBPGID.InnerText;

                XmlNode nodeformBPGID = xDocformBPGID.SelectSingleNode("bpinfo");
                nodeformBPGID.RemoveAll();

                XmlNode nodenewBPGID = xDocformBPGID.CreateNode(XmlNodeType.Element, "BPGID", null);
                nodenewBPGID.InnerText = HelpBPGID;
                nodeformBPGID.AppendChild(nodenewBPGID);

                XmlNode nodehelpBPGID = xDocformBPGID.CreateNode(XmlNodeType.Element, "HelpBPGID", null);
                if (strBPGID == string.Empty)
                {
                    nodehelpBPGID.InnerText = BPGID;
                }
                else
                {
                    nodehelpBPGID.InnerText = strBPGID;
                }

                nodeformBPGID.AppendChild(nodehelpBPGID);

                strBPINFO = nodeformBPGID.OuterXml.ToString();
            }
            //generate Request XMl 
            string strReqXml = objBO.GenActionRequestXML("PAGELOAD", strBPINFO, "", "", "", strBPE, false, "1", "1", null);
            //BPOUT from DB
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(strOutXml);
            //xDoc.Load(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\XMLFile3.xml");
            XmlNode nodeMsgStatus = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeMsg = xDoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");

            if (nodeMsgStatus.InnerText != "Success")
            {
                //Error 
                if (nodeMsg != null)
                {
                    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo").InnerText != null && nodeMsg.SelectSingleNode("OtherInfo").InnerText != string.Empty)
                        {
                            lblmsg.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                        else
                        {
                            lblmsg.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else
                        lblmsg.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                }
            }
            else
            {
                //Success
                HtmlTable htable = new HtmlTable();
                htable = GetHtmlTable(xDoc);
                if (htable.Rows.Count > 0)
                {
                    pnlHelpTable.Visible = true;

                    pnlHelpTable.Controls.Add(htable);

                    SetUIHeaders(448, 500);

                }

                //Rename the close hyperlink in the Popup frame according to the header text.
                string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                string headerTitle = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Title").InnerText;
                string changeCloseJS = "ChangeCloseLinkText('" + headerTitle + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeCloseText", changeCloseJS, true);
            }
        }

        private void preloadImages()
        {


            imgbtnSearch.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/helpsearch.png";
            imgbtnGo.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/go-but1.gif";
        }


        private void bindlogo()
        {
            string imgpath = string.Empty;
            string m_CompanyImageSrc = string.Empty;
            string m_PhysicalImgPath = string.Empty;
            XmlNode m_CompanyNode = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/DashboardLogo");
            if (m_CompanyNode != null)
            {
                m_CompanyImageSrc = m_CompanyNode.InnerXml.ToString();
                m_PhysicalImgPath = m_AttachmentsPath + "\\" + m_CompanyImageSrc;
                imgpath = m_AttachmentsVirtualPath + m_CompanyImageSrc;
                if (System.IO.File.Exists(m_PhysicalImgPath))
                {
                    imghelplogo.Src = imgpath;
                }
                else
                {
                    imgpath = Application["ImagesCDNPath"].ToString() + "Images/lajit_logo.jpg";
                    imghelplogo.Src = imgpath;
                }
            }
            else
            {
                imgpath = Application["ImagesCDNPath"].ToString() + "Images/lajit_logo.jpg";
                imghelplogo.Src = imgpath;
            }
        }

        private HtmlTable GetHtmlTable(XmlDocument xdoc)
        {
            string treeNodeName = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            string ChildBranchName = string.Empty;
            //Parent Columns
            XmlNodeList nodeColumns = xdoc.SelectNodes("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
            XmlNode childNode = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches/Branch/Node");
            HtmlTable htable = new HtmlTable();

            if (childNode != null)
            {

                //pnlHelpTable.Style.Add("width", "auto");
                htable.CellPadding = Convert.ToInt32("0");
                htable.CellSpacing = Convert.ToInt32("0");
                htable.Align = "center";
                htable.Width = "360px";       //"624px";//"100%";  //"780px";
                htable.Border = Convert.ToInt32("0");
                ChildBranchName = childNode.InnerText;
                XmlNode childNodeRows = xdoc.SelectSingleNode("Root/bpeout/FormControls/" + ChildBranchName + "/RowList");
                if (childNodeRows != null)
                {
                    if (childNodeRows.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode nodeRow in childNodeRows.ChildNodes)
                        {

                            //Each new table creation add on spacer image to table cell of row
                            HtmlTableRow htrSpacer = new HtmlTableRow();
                            HtmlTableCell htdSpacer = new HtmlTableCell();

                            HtmlImage htimgSpacer = new HtmlImage();
                            htimgSpacer.Src = Application["ImagesCDNPath"].ToString() + "Images/spacer.gif";
                            htimgSpacer.Height = Convert.ToInt32("1");
                            htimgSpacer.Width = Convert.ToInt32("1");

                            htdSpacer.Align = "left";
                            htdSpacer.VAlign = "top";
                            htdSpacer.Height = "18px";
                            htdSpacer.Controls.Add(htimgSpacer);

                            htrSpacer.Cells.Add(htdSpacer);
                            htable.Rows.Add(htrSpacer);

                            // Next table row contains html table

                            HtmlTableRow htr = new HtmlTableRow();
                            HtmlTableCell htd = new HtmlTableCell();

                            //<td align="left" valign="top" class="help-content-bg" style="padding:20px;">
                            htd.Align = "left";
                            htd.VAlign = "top";
                            htd.Attributes.Add("class", "help-content-bg");
                            htd.Attributes.Add("padding", "20px");


                            HtmlTable htable1 = new HtmlTable();
                            //htable1.Border = Convert.ToInt32("1");
                            htable1.CellPadding = Convert.ToInt32("4");
                            htable1.CellSpacing = Convert.ToInt32("0");
                            htable1.Width = "350px";
                            //htable1.Width = "100%"; 



                            //Help Caption
                            if (nodeRow.Attributes["HelpCaption"] != null)
                            {
                                HtmlTableRow htr1 = new HtmlTableRow();
                                HtmlTableCell htd1 = new HtmlTableCell();
                                htd1.InnerHtml = nodeRow.Attributes["HelpCaption"].Value;
                                htd1.Attributes.Add("class", "help-text-big");
                                //htd1.Width="157px";
                                htd1.VAlign = "top";
                                htr1.Cells.Add(htd1);
                                //dummy

                                // <td style="width:20px;"><img src="images/spacer.gif" width="1" height="1"></td>

                                /*HtmlTableCell htdSpacer1= new HtmlTableCell();
                            
                                HtmlImage htimgSpacer1 = new HtmlImage();
                                htimgSpacer1.Src="~/App_Themes/" + Session["MyTheme"].ToString() + "/Images/spacer.gif";
                                htimgSpacer1.Height=Convert.ToInt32("1");
                                htimgSpacer1.Width=Convert.ToInt32("1");
                                htdSpacer1.Width = "18px";
                                htdSpacer1.Controls.Add(htimgSpacer1);
                                htr1.Cells.Add(htdSpacer1);*/

                                HtmlTableRow htr2 = new HtmlTableRow();
                                HtmlTableCell htd2 = new HtmlTableCell();

                                // HelpText
                                if (nodeRow.Attributes["HelpText"] != null)
                                {


                                    //Inner Table
                                    HtmlTable htable2 = new HtmlTable();
                                    htable2.Width = "100%";
                                    htable2.CellPadding = Convert.ToInt32("0");
                                    htable2.CellSpacing = Convert.ToInt32("0");

                                    //Help Text Row and Cell
                                    HtmlTableRow htrHelpText = new HtmlTableRow();

                                    HtmlTableCell htdHelpText = new HtmlTableCell();
                                    htdHelpText.Align = "left";
                                    htdHelpText.VAlign = "top";
                                    htdHelpText.Attributes.Add("class", "help-content");
                                    //htdHelpText.InnerHtml ="<strong>"+ nodeRow.Attributes["HelpText"].Value+"</<strong>";
                                    htdHelpText.InnerHtml = nodeRow.Attributes["HelpText"].Value;
                                    htrHelpText.Cells.Add(htdHelpText);

                                    htable2.Rows.Add(htrHelpText);

                                    //links Help file Caption
                                    if (nodeRow.Attributes["HelpFileTypeID"] != null)
                                    {

                                        string strHelpFileTypeID = string.Empty;
                                        string strHelpFileCaption = string.Empty;
                                        string strHelpFile = string.Empty;
                                        string strDetails = string.Empty;
                                        if (nodeRow.Attributes["HelpFileTypeID"] != null)
                                        {
                                            strHelpFileTypeID = nodeRow.Attributes["HelpFileTypeID"].Value;
                                        }
                                        if (nodeRow.Attributes["HelpFile"] != null)
                                        {
                                            strHelpFile = nodeRow.Attributes["HelpFile"].Value;
                                        }
                                        if (nodeRow.Attributes["HelpFileCaption"] != null)
                                        {
                                            strHelpFileCaption = nodeRow.Attributes["HelpFileCaption"].Value;
                                        }

                                        if ((strHelpFileTypeID != string.Empty) && (strHelpFile != string.Empty))
                                        {
                                            strDetails = GetTypeOfHelp(strHelpFileTypeID, strHelpFileCaption, strHelpFile);
                                        }
                                        if (strDetails != string.Empty)
                                        {
                                            HtmlTableRow htrFile = new HtmlTableRow();
                                            HtmlTableCell htdFile = new HtmlTableCell();
                                            htdFile.Align = "left";
                                            htdFile.VAlign = "top";
                                            htdFile.Height = "20px";
                                            htdFile.InnerHtml = strDetails;

                                            htrFile.Cells.Add(htdFile);
                                            htable2.Rows.Add(htrFile);
                                        }

                                    }
                                    htd2.Controls.Add(htable2);
                                    htr2.Cells.Add(htd2);

                                }
                                htable1.Rows.Add(htr1);

                                if (htr2.Cells.Count > 0)
                                {
                                    htable1.Rows.Add(htr2);
                                }
                            }

                            htd.Controls.Add(htable1);
                            htr.Cells.Add(htd);
                            htable.Rows.Add(htr);
                        }
                    }
                }
            }
            return htable;
        }

        private string GetTypeOfHelp(string strTypeOfHelp, string strLinkCaption, string strLink)
        {
            string strDetails = string.Empty;
            string strScript = string.Empty;
            string strCurrentPath = string.Empty;
            string strVirtualPath = string.Empty;

            switch (strTypeOfHelp)
            {

                case "1": // Image link      image open in new window

                    strCurrentPath = m_HelpFilesPath + @"\Image\" + strLink;

                    strVirtualPath = m_HelpVirtualPath + "Image/" + strLink;

                    //Any spaces are replaced by%20
                    strScript = "OpenHelpPopUp('1','" + strVirtualPath.Replace(" ", "%20") + "')";

                    // Verify image exist or not
                    if (!File.Exists(strCurrentPath))
                    {
                        //hide link if image not available
                        strDetails = string.Empty;
                    }
                    else
                    {
                        strDetails = "<a href='#' class='blue-links-small'  onclick=" + strScript + ">Click here to view image</a>";
                    }
                    break;

                case "2": // Pdf link

                    strCurrentPath = m_HelpFilesPath + @"\PDF\" + strLink;

                    strVirtualPath = m_HelpVirtualPath + "PDF/" + strLink;

                    if (!File.Exists(strCurrentPath))
                    {   // PDF not found alert
                        strScript = "window.alert('PDF-NOTFOUND')";
                        strDetails = "<a href='#' class='blue-links-small' onclick=" + strScript + ">" + strLinkCaption + "</a>";
                    }
                    else
                    {
                        // PDF found open in new window
                        strScript = "OpenHelpPopUp('1','" + strVirtualPath.Replace(" ", "%20") + "')";
                        strDetails = "<a href='#' class='blue-links-small' onclick=" + strScript + ">" + strLinkCaption + "</a>";
                    }
                    break;

                case "3": // html link

                    strScript = "OpenHelpPopUp('1','http://" + strLink.Replace(" ", "%20") + "')";

                    strDetails = "<a href='#' class='blue-links-small' onclick=" + strScript + ">" + strLinkCaption + "</a>";
                    break;

                case "4": // Streaming Video link

                    strLink = "../Common/FlashViewer.aspx?File=" + strLink;

                    strScript = "OpenHelpPopUp('1','" + strLink.Replace(" ", "%20") + "')";

                    strDetails = "<a href='#' class='blue-links-small' onclick=" + strScript + ">" + strLinkCaption + "</a>";

                    break;

                case "5": // Image Embedded

                    strCurrentPath = m_HelpFilesPath + @"\Image\" + strLink;
                    strVirtualPath = m_HelpVirtualPath + "Image/" + strLink;

                    // Verify image exist or not
                    if (!File.Exists(strCurrentPath))
                    {
                        //Set Default Image
                        strVirtualPath = Application["ImagesCDNPath"].ToString() + "images/lajit-rptlogo.JPG";
                    }
                    strDetails = "<img src='" + strVirtualPath + "' title='" + strLinkCaption + "' />";
                    break;

                default:
                    break;
            }
            return strDetails;
        }
        #endregion
    }
}
