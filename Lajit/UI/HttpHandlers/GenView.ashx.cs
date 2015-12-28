using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using LAjitDev.Classes;
using System.Web.SessionState;
using NLog;


namespace LAjitDev.HttpHandlers
{
    /// <summary>
    /// HttpHandler for Ajax Calls intiated by the JqGrid.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GenView : IHttpHandler, IRequiresSessionState
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        HttpContext _Context;
        XmlDocument xDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();
        XmlDocument XDocUserInfo = new XmlDocument();
        private string strBPE;
        private static string m_AttachmentsPath; 

        private string m_DefaultPageSize;

        public string DefaultPageSize
        {
            get { return m_DefaultPageSize; }
            set { m_DefaultPageSize = value; }
        }

        private double ConvertBytesToKilobytes(long b)
        {
            return (b / 1024f);
        }

        public void ProcessRequest(HttpContext context)
        {
            #region NLog
            logger.Info("Processing the request context : " + context);
            #endregion

            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string pageSize = context.Request["rows"];
            DefaultPageSize = !string.IsNullOrEmpty(pageSize) ? pageSize : "-1";
            JQCommonUI objJQCommonUI = new JQCommonUI();
            objJQCommonUI.PageSize = DefaultPageSize;
            objJQCommonUI.Context = context;
            _Context = context;


            m_AttachmentsPath = ConfigurationManager.AppSettings["AttachmentsPath"] + "/" + context.Session["CompanyEntityID"].ToString();

            if (context.Request.HttpMethod != null && context.Request.HttpMethod == "POST")
            {
                if (context.Request["oper"] != null)
                {
                    Hashtable htReqVar = objJQCommonUI.InitializeReqVar(context);
                    string cntrlVals = objJQCommonUI.GetCntrlValues(context, htReqVar);
                    string strSelectedRw = string.Empty;
                    if (htReqVar["selectedRw"] != null)
                    {
                        XmlDocument selectedRwXML = new XmlDocument();
                        selectedRwXML.LoadXml(htReqVar["selectedRw"].ToString());
                        XmlNode nodeSelRw = objJQCommonUI.ReverseXMLNode(selectedRwXML.SelectSingleNode("Rows"));
                        strSelectedRw = nodeSelRw.InnerXml.ToString();
                    }
                    //Submit the entry and get back the Response.
                    string response = objJQCommonUI.SubmitEntry(htReqVar, strSelectedRw, cntrlVals);
                    context.Response.Write(response);
                }
                else
                {
                    //For file Upload
                    if (context.Request.Files.Count != 0)
                    {
                        UploadFile(context);
                    }
                }
            }
            else//PageLoad,
            {
                context.Response.ContentType = "application/xml";
                string searchOper = context.Request["_search"].ToString();
                //For Search operation
                if (searchOper == "true")
                {
                    Hashtable htReqVar = objJQCommonUI.InitializeReqVar(context);
                    string cntrlVals = "";
                    if (context.Request["filters"] == null || context.Request["filters"] == "")
                    {
                        cntrlVals = objJQCommonUI.GetXMLRow(context, htReqVar);
                    }
                    else
                    {
                        cntrlVals = objJQCommonUI.GetCntrlValuesForFind(context, htReqVar["BPAction"].ToString());
                    }
                    objJQCommonUI.Context = context;
                    string jqXML = objJQCommonUI.SubmitEntry(htReqVar, "", cntrlVals);
                    context.Response.Write(jqXML);
                    return;
                }

                string pageNo = context.Request["page"];
                string sortColumn = context.Request["sidx"].Length > 0 ? context.Request["sidx"] : "";
                string sortOrder = context.Request["sord"].ToUpper();
                int defaultPageSize = Convert.ToInt32(DefaultPageSize);

                string GVTreeNodeName = "";
                string bpInfoProjects = GenerateRequestXML(pageNo, sortColumn, sortOrder, out GVTreeNodeName);

                strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
                LAjit_BO.CommonBO objBO = new LAjit_BO.CommonBO();
                string outXML = objBO.GetLoginInfo(bpInfoProjects);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(outXML);

                string rowCount = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/Gridresults/Currentpage").Attributes["Pagesize"].Value;
                string totalCount = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/Gridresults/Totalpage").Attributes["Pagesize"].Value;
                double pageCount = Math.Ceiling(Convert.ToDouble(totalCount) / defaultPageSize);

                if (Convert.ToInt32(rowCount) > defaultPageSize)
                {
                    pageCount = 1;
                }

                string userdata = objJQCommonUI.setUserData(xDoc, GVTreeNodeName, null);

                XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");
                string xml = string.Empty;
                if (nodeRowList != null)
                {
                    ////Test block
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    nodeRowList.InnerXml += nodeRowList.InnerXml;
                    //    totalCount = Convert.ToString(Convert.ToInt32(totalCount) * 2);
                    //}
                    ////Test Block End.

                    XmlNode nodePivot = objJQCommonUI.TransposeXMLNode(nodeRowList);
                    nodePivot.InnerXml = userdata + "<PageNo>" + pageNo + "</PageNo><Pages>" + pageCount + "</Pages><TotalRows>" + totalCount + "</TotalRows>" + nodePivot.InnerXml;
                    xml = nodePivot.OuterXml;
                    context.Response.Write(xml);
                }
                else
                {
                    xml = "<RowList>" + userdata + "<PageNo>" + pageNo + "</PageNo><Pages>" + pageCount + "</Pages><TotalRows>" + totalCount + "</TotalRows>" + "</RowList>";
                    context.Response.Write(xml);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GenerateRequestXML(string pageNo, string sortColumn, string sortOrder, out string GVTreeNodeName)
        {
            #region NLog
            logger.Info("Generating the XML to form a Gen View based on the pageno : " + pageNo + " sort column : " + sortColumn + " sort order : " + sortOrder); 
            #endregion

            //XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            XmlDocument xDoc = new XmlDocument();
            XmlNode nodeRoot = xDoc.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = strBPE;
            XmlDocument xDocBPEInfo = new XmlDocument();
            xDocBPEInfo.LoadXml(Convert.ToString(_Context.Session["LinkBPINFO"]));
            if (_Context.Request["_search"] != null && _Context.Request["_search"] != "true")
            {
                //Undo any Search related entries if any, in the Session[LinkBPInfo]
                XmlNodeList xnlFind = xDocBPEInfo.SelectNodes("bpinfo//Rows[@BPAction='Find']");
                foreach (XmlNode node in xnlFind)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }
            XmlNode nodeGridView = xDocBPEInfo.SelectSingleNode("bpinfo//Gridview");
            nodeGridView.SelectSingleNode("Pagesize").InnerText = DefaultPageSize;
            nodeGridView.SelectSingleNode("Pagenumber").InnerText = pageNo;
            nodeGridView.SelectSingleNode("Sortcolumn").InnerText = sortColumn;
            nodeGridView.SelectSingleNode("Sortorder").InnerText = sortOrder;
            GVTreeNodeName = nodeGridView.ParentNode.LocalName;
            nodeRoot.InnerXml += xDocBPEInfo.OuterXml;
            _Context.Session["LinkBPINFO"] = xDocBPEInfo.OuterXml;
            return nodeRoot.OuterXml;
        }

        private void UploadFile(HttpContext context)
        {
            #region NLog
            logger.Info("This method is used to upload a file : " + context); 
            #endregion

            string strSaveLocation = string.Empty;
            string strFileName = string.Empty;
            string strExtension = string.Empty;
            string strDateTag = string.Empty;
            string filesizeKB = string.Empty;

            XmlDocument xDocBPE = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string CompanyID = xDocBPE.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText;

            HttpPostedFile FileAttachment = context.Request.Files[0];
            strFileName = Path.GetFileName(FileAttachment.FileName) + "~" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            strExtension = Path.GetExtension(FileAttachment.FileName).ToLower();

            XmlDocument xFileResponse = new XmlDocument();
            XmlNode nodeRoot = xFileResponse.CreateNode(XmlNodeType.Element, "Data", null);
            XmlNode nodeMessageInfo = xFileResponse.CreateNode(XmlNodeType.Element, "MessageInfo", null);

            if (strExtension != ".bat" && strExtension != ".exe")
            {
               /*if (System.IO.Directory.Exists(m_AttachmentsPath + @"\" + CompanyID))
                {
                    strSaveLocation = m_AttachmentsPath + @"\" + CompanyID + @"\" + strFileName + strDateTag + strExtension;
                }
                else
                {
                    System.IO.Directory.CreateDirectory(m_AttachmentsPath + @"\" + CompanyID);
                    strSaveLocation = m_AttachmentsPath + @"\" + CompanyID + @"\" + strFileName + strDateTag + strExtension;
                }*/

                if (System.IO.Directory.Exists(m_AttachmentsPath))
                {
                    strSaveLocation = m_AttachmentsPath + @"\" + strFileName + strDateTag + strExtension;
                }
                else
                {
                    System.IO.Directory.CreateDirectory(m_AttachmentsPath + @"\" + CompanyID);
                    strSaveLocation = m_AttachmentsPath + @"\" + strFileName + strDateTag + strExtension;
                }


                string strFilesizeKB = ConvertBytesToKilobytes(FileAttachment.ContentLength).ToString("0");
                FileAttachment.SaveAs(strSaveLocation);

                nodeMessageInfo.InnerText = "Success";

                XmlNode nodeFileName = xFileResponse.CreateNode(XmlNodeType.Element, "FileName", null);
                nodeFileName.InnerText = strFileName;
                nodeRoot.AppendChild(nodeFileName);

                XmlNode nodeFileExtension = xFileResponse.CreateNode(XmlNodeType.Element, "FileExtension", null);
                nodeFileExtension.InnerText = strExtension;
                nodeRoot.AppendChild(nodeFileExtension);

                XmlNode nodeFileSize = xFileResponse.CreateNode(XmlNodeType.Element, "FileSize", null);
                nodeFileSize.InnerText = strFilesizeKB;
                nodeRoot.AppendChild(nodeFileSize);

            }
            else
            {
                nodeMessageInfo.InnerText = "Error";
            }
            nodeRoot.AppendChild(nodeMessageInfo);
            context.Response.ContentType = "application/xml";
            context.Response.Write(nodeRoot.OuterXml);
        }
    }
}
