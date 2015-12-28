using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using LAjit_BO;
using LAjitControls;
using System.Web.SessionState;

namespace LAjitDev.Classes
{
    public class AutoFill
    {
        private static HttpContext m_CurrentContext;

        public static HttpContext CurrentContext
        {
            set { m_CurrentContext = value; }
            get
            {
                if (m_CurrentContext != null)
                {
                    return m_CurrentContext;
                }
                else
                {
                    return HttpContext.Current;
                }
            }
        }

        /// <summary>
        /// Returns the Rowlist which matched the search criteria.
        /// </summary>
        /// <param name="prefix">AutoFill Search Criteria</param>
        /// <param name="storedProcedureName">The Sp to run.</param>
        /// <param name="contextKey">The Column Label pertaining the AutoFill.</param>
        /// <param name="COA">JOBCOA if any.</param>
        /// <returns></returns>
        public static XmlNode GetAutoFillDataNode(string prefix, string storedProcedureName, string contextKey, string COA)
        {
            CommonUI objCommonUI = new CommonUI();
            XmlDocument xDocBPE = objCommonUI.loadXmlFile(Convert.ToString(CurrentContext.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(xDocBPE.SelectSingleNode("Root/bpe").OuterXml);

            CommonBO objBO = new CommonBO();
            prefix = "%" + prefix + "%";//Seach anywhere in the text.
            string reqXML = "<Root>" + strBPE + "<bpinfo><ContextKey>" + contextKey
                                + "</ContextKey><SearchCriterion>" + prefix + "</SearchCriterion><COA>" + COA + "</COA></bpinfo></Root>";

            string outXML = objBO.GetAutoFillData(reqXML, storedProcedureName);

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml("<Root>" + outXML + "</Root>");
            //if (xDocOut.DocumentElement.ChildNodes.Count == 2)
            //{
            return xDocOut.DocumentElement.ChildNodes[1];
            //}
            //else
            //{
            //    return null;
            //}
            //return xDocOut.SelectSingleNode("Root/AutoFill" + contextKey);
        }


        /// <summary>
        /// Gets the DB server's recognized context key.
        /// </summary>
        /// <param name="ctxKey">Orginal Context Key.</param>
        public static string GetSPContextKey(string ctxKey)
        {
            switch (ctxKey)
            {
                case "AutoFillJob":
                case "AutoFillJob_1":
                case "AutoFillJob_2":
                    {
                        ctxKey = "AutoFillJob";
                        break;
                    }
                case "AutoFillAccount":
                case "AutoFillAccount_1":
                case "AutoFillAccount_2":
                    {
                        ctxKey = "AutoFillAccount";
                        break;
                    }
                case "AutoFillSubAccount1":
                case "AutoFillSubAcct1_1":
                case "AutoFillSubAcct1_2":
                    {
                        ctxKey = "AutoFillSubAccount1";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return ctxKey;
        }

        //To use AutoFill Functionality
        //Developed to fill autofill data
        /// <summary>
        /// Requested xml sent to db
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns>outerxml</returns>
        /// 
        public static string GetAutoFillData(string Prefix, string StoredProcedureName, string cacheToken, string cacheName)
        {
            CommonUI commonObjUI = new CommonUI();
            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(CurrentContext.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);


            XmlDocument xdoc = new XmlDocument();
            XmlNode nodeRoot = xdoc.CreateNode(XmlNodeType.Element, "Root", null);
            string outxml = string.Empty;
            CommonBO objBO = new CommonBO();
            nodeRoot.InnerXml = strBPE;

            if (cacheToken != string.Empty)
            {
                if ((CurrentContext.Session[cacheToken] != null) && (CurrentContext.Cache[cacheName] != null))
                {
                    nodeRoot.InnerXml += "<bpinfo>" + CurrentContext.Session[cacheToken].ToString() + "</bpinfo>";
                }
            }

            //if (Prefix != string.Empty)
            //{
            //    nodeRoot.InnerXml += "<bpinfo><autofill>" + Prefix + "</autofill></bpinfo>";
            //}

            outxml = objBO.GetAutoFillData(nodeRoot.OuterXml.ToString(), StoredProcedureName);



            string message = "<MessageInfo>" +
                           "<Status>Success</Status>" +
                           "<Message>" +
                           "<Status>Success</Status>" +
                           "<Label>Process Complete.</Label>" +
                           "</Message>" +
                           "</MessageInfo>";

            outxml = outxml.Replace(message, string.Empty);
            return outxml;
        }

        #region Obsolete Code

       
        /// <summary>
        /// Autofill data filled to dataset.
        /// </summary>
        /// <param name="Prefix">sending prefix</param>
        /// <returns>DataSet</returns>
        /*    public static void LoadAutoFillData(string Prefix, string contextKey, string StoredProcedureName, string cacheToken)
            {
                string OuterXml = string.Empty;
                string cacheName = GetLoggedInCompanyID() + contextKey;

                OuterXml = GetAutoFillData(Prefix, StoredProcedureName, cacheToken, cacheName);
                XmlDocument xdoc = new XmlDocument();
                DataSet dsAutoFill = new DataSet();
                string CacheTokenNode = string.Empty;

                if (OuterXml != null)
                {
                    xdoc.LoadXml("<Root>" + OuterXml.ToString() + "</Root>");

                    XmlNode nodeRows = xdoc.SelectSingleNode("Root/" + contextKey + "/RowList");
                    DataSet dsRowList = new DataSet();
                    if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                    {
                        XmlNodeReader read = new XmlNodeReader(nodeRows);
                        dsAutoFill.ReadXml(read);
                        dsAutoFill.Tables["Rows"].TableName = contextKey;
                        dsAutoFill.AcceptChanges();

                        //Dataset having rows then update session

                        if (dsAutoFill.Tables.Count > 0)
                        {
                            if (dsAutoFill.Tables[contextKey].Rows.Count > 0)
                            {
                                //Insert Token Session
                                XmlNode xnode = xdoc.SelectSingleNode("Root/" + cacheToken);
                                if ((xnode != null) && (cacheToken != string.Empty))
                                {
                                    CacheTokenNode = xnode.OuterXml; //"<" + cacheToken + ">" + xnode.InnerText + "</" + cacheToken + ">";
                                    CurrentContext.Session[cacheToken] = CacheTokenNode;
                                }
                                //Insert or Update Cache
                                string slidingExpiration = CurrentContext.Session.Timeout.ToString();
                                CurrentContext.Cache.Insert(cacheName, dsAutoFill, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Convert.ToDouble(slidingExpiration.ToString())));
                            }
                        }

                    }
                }
            }

            /// <summary>
            /// Storing autofill data in Cache as Dataset
            /// </summary>
            /// <param name="columnName"></param>
            public static void FillAutoDataCache(string columnName)
            {
                string storedProcedureName = string.Empty;
                string cacheToken = string.Empty;
                //DataSet dsChildElements = null;

                if (columnName != "")
                {
                    switch (columnName)
                    {
                        case "AutoFillVendor":
                            cacheToken = "VendorCacheToken";
                            storedProcedureName = "AutoFillVendorCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillJob":
                            cacheToken = "JobCacheToken";
                            storedProcedureName = "AutoFillJobCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillAccount":
                            cacheToken = "AccountCacheToken";
                            storedProcedureName = "AutoFillAccountCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillSubAccount1":
                            cacheToken = "SubAccount1CacheToken";
                            storedProcedureName = "AutoFillSUbAccount1Cache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillSubAccount2":
                            cacheToken = "SubAccount2CacheToken";
                            storedProcedureName = "AutoFillSUbAccount2Cache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillSubAccount3":
                            cacheToken = "SubAccount3CacheToken";
                            storedProcedureName = "AutoFillSUbAccount3Cache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillSubAccount4":
                            cacheToken = "SubAccount4CacheToken";
                            storedProcedureName = "AutoFillSUbAccount4Cache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AutoFillSubAccount5":
                            cacheToken = "SubAccount5CacheToken";
                            storedProcedureName = "AutoFillSUbAccount5Cache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "Customer":
                            cacheToken = "CustomerCacheToken";
                            storedProcedureName = "AutoFillCustomerCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "SelectCustInvoice":
                            cacheToken = "CustInvoiceCacheToken";
                            storedProcedureName = "AutoFillCustInvoiceCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        case "AREntryJob":
                            cacheToken = "CustJobCacheToken";
                            storedProcedureName = "AutoFillCustJobCache";
                            LoadAutoFillData(string.Empty, columnName, storedProcedureName, cacheToken);
                            break;
                        default:
                            break;
                    }


                }

            }

            /// <summary>
            /// Based on branch columns set autofill
            /// </summary>
            /// <param name="xdoc"></param>
            public static void SetBranchsAutoFillCache(XmlDocument xdoc)
            {
                string treeNodeName = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                string ChildBranchName = string.Empty;

                //Parent Columns
                XmlNodeList nodeParentColumns = xdoc.SelectNodes("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns//Col[@ControlType='TBox' and @IsLink='1']");

                //Branch Columns
                XmlNode nodeBranches = xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                if (nodeParentColumns != null)
                {
                    SetColumnCache(nodeParentColumns);
                }

                if (nodeBranches != null)
                { //Getting all the branchnames
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                        if (branchNodeName != string.Empty)
                        {

                            XmlNodeList nodeBranchColumns = xdoc.SelectNodes("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns//Col[@ControlType='TBox' and @IsLink='1']");
                            SetColumnCache(nodeBranchColumns);
                        }
                    }
                }
            }

            /// <summary>
            ///  Based on parent column set cache
            /// </summary>
            /// <param name="nodeBranchColumns"></param>
            private static void SetColumnCache(XmlNodeList nodeBranchColumns)
            {
                if (nodeBranchColumns != null)
                {
                    foreach (XmlNode colNode in nodeBranchColumns)
                    {
                        //Add the column only if FullViewLength is not equal to zero
                        int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                        if ((colFullViewLength != 0))
                        {
                            string currentLabel = colNode.Attributes["Label"].Value.Trim();
                            //Check for IsDisplayOnly
                            bool isDisplayOnly = false;
                            if (colNode.Attributes["IsDisplayOnly"] != null
                            && colNode.Attributes["IsDisplayOnly"].Value.Trim() == "1")
                            {
                                isDisplayOnly = true;
                            }
                            //Checking controltype and Islink and IsdDisplayOnly
                            if ((colNode.Attributes["ControlType"] != null) && (colNode.Attributes["IsLink"] != null) && (!isDisplayOnly))
                            {
                                //Control Type is Textbox and IsLink is One adding DataSetCache
                                if (colNode.Attributes["IsLink"].Value == "1")
                                {
                                    //string cacheName = GetLoggedInCompanyID() + currentLabel;
                                    //Check Cache is available don't hit db again
                                    //if (System.Web.CurrentContext.Cache[cacheName]==null)
                                    //{
                                    //Call Every Time
                                    FillAutoDataCache(currentLabel);
                                    //}
                                }
                            }

                        }

                    }
                }
            }*/

        /// <summary>
        /// Get logged in company ID from BPE 
        /// This method reference used in other pages
        /// </summary>
        /// <returns></returns>
        public static string GetLoggedInCompanyID()
        {
            CommonUI commonObjUI = new CommonUI();
            XmlDocument XDocUserInfo = new XmlDocument();
            string CompanyID = string.Empty;
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(CurrentContext.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            if (strBPE != null && strBPE != "")
            {

                if (XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText.Trim() != string.Empty)
                {
                    CompanyID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText.Trim();
                }
            }
            return CompanyID;
        }

        //Newly written to hit every time db
        /* public static void GetAutoFillDatails(string columnName, DataSet dsAutofill)
         {
             string storedProcedureName = string.Empty;
             string cacheToken = string.Empty;
             //DataSet dsChildElements = null;

             if (columnName != "")
             {
                 switch (columnName)
                 {
                     case "AutoFillVendor":
                         cacheToken = "VendorCacheToken";
                         storedProcedureName = "AutoFillVendorCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillJob":
                         cacheToken = "JobCacheToken";
                         storedProcedureName = "AutoFillJobCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillAccount":
                         cacheToken = "AccountCacheToken";
                         storedProcedureName = "AutoFillAccountCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillSubAccount1":
                         cacheToken = "SubAccount1CacheToken";
                         storedProcedureName = "AutoFillSUbAccount1Cache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillSubAccount2":
                         cacheToken = "SubAccount2CacheToken";
                         storedProcedureName = "AutoFillSUbAccount2Cache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillSubAccount3":
                         cacheToken = "SubAccount3CacheToken";
                         storedProcedureName = "AutoFillSUbAccount3Cache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillSubAccount4":
                         cacheToken = "SubAccount4CacheToken";
                         storedProcedureName = "AutoFillSUbAccount4Cache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AutoFillSubAccount5":
                         cacheToken = "SubAccount5CacheToken";
                         storedProcedureName = "AutoFillSUbAccount5Cache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "Customer":
                         cacheToken = "CustomerCacheToken";
                         storedProcedureName = "AutoFillCustomerCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "SelectCustInvoice":
                         cacheToken = "CustInvoiceCacheToken";
                         storedProcedureName = "AutoFillCustInvoiceCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     case "AREntryJob":
                         cacheToken = "CustJobCacheToken";
                         storedProcedureName = "AutoFillCustJobCache";
                         LoadAutoFill(string.Empty, columnName, storedProcedureName, string.Empty, dsAutofill);
                         break;
                     default:
                         break;
                 }


             }

         }

         private static void LoadAutoFill(string Prefix, string contextKey, string StoredProcedureName, string cacheToken, DataSet dsAutoFill)
         {
             string OuterXml = string.Empty;
             string cacheName = GetLoggedInCompanyID() + contextKey;

             OuterXml = GetAutoFillData(Prefix, StoredProcedureName, cacheToken, cacheName);
             XmlDocument xdoc = new XmlDocument();
             //DataSet dsAutoFill = new DataSet();
             string CacheTokenNode = string.Empty;

             if (OuterXml != null)
             {
                 xdoc.LoadXml("<Root>" + OuterXml.ToString() + "</Root>");

                 XmlNode nodeRows = xdoc.SelectSingleNode("Root/" + contextKey + "/RowList");
                 DataSet dsRowList = new DataSet();
                 if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                 {
                     XmlNodeReader read = new XmlNodeReader(nodeRows);
                     dsAutoFill.ReadXml(read);
                     dsAutoFill.Tables["Rows"].TableName = contextKey;
                     dsAutoFill.AcceptChanges();

                     //Dataset having rows then update session

                     //if (dsAutoFill.Tables.Count > 0)
                     //{
                     //    if (dsAutoFill.Tables[contextKey].Rows.Count > 0)
                     //    {
                     //        //Insert Token Session
                     //        XmlNode xnode = xdoc.SelectSingleNode("Root/" + cacheToken);
                     //        if ((xnode != null) && (cacheToken != string.Empty))
                     //        {
                     //            CacheTokenNode = xnode.OuterXml; //"<" + cacheToken + ">" + xnode.InnerText + "</" + cacheToken + ">";
                     //            System.Web.CurrentContext.Session[cacheToken] = CacheTokenNode;
                     //        }

                     //    }
                     //}

                 }
             }
             //return dsAutoFill;
         }

        public static string GetAutoFillHandlerData(string Prefix, string ColoumnName, int limit, string strCOA)
        {
            DataSet dsAutoFill = new DataSet();

            CommonUI commonObjUI = new CommonUI();
            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(CurrentContext.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            XmlDocument xdoc = new XmlDocument();
            XmlNode nodeRoot = xdoc.CreateNode(XmlNodeType.Element, "Root", null);
            string outxml = string.Empty;
            CommonBO objBO = new CommonBO();
            nodeRoot.InnerXml = strBPE;

            string queryCol = string.Empty;
            switch (ColoumnName)
            {
                case "AutoFillJob":
                case "AutoFillJob_1":
                case "AutoFillJob_2":
                    {
                        outxml = objBO.GetAutoFillData(nodeRoot.OuterXml.ToString(), "AutoFillJobList");
                        queryCol = "AutoFillJob";
                        break;
                    }
                case "AutoFillAccount":
                case "AutoFillAccount_1":
                case "AutoFillAccount_2":
                    {
                        outxml = objBO.GetAutoFillData(nodeRoot.OuterXml.ToString(), "AutoFillAccountList");
                        queryCol = "AutoFillAccount";
                        break;
                    }
                case "AutoFillSubAccount1":
                case "AutoFillSubAcct1_1":
                case "AutoFillSubAcct1_2":
                    {
                        outxml = objBO.GetAutoFillData(nodeRoot.OuterXml.ToString(), "AutoFillSubAccount1List");
                        queryCol = "AutoFillSubAccount1";
                        break;
                    }
                case "AutoFillVendor":
                    {
                        //AutoFillVendorList
                        outxml = objBO.GetAutoFillData(nodeRoot.OuterXml.ToString(), "AutoFillVendorList");
                        queryCol = "AutoFillVendor";
                        break;
                    }

            }
            ColoumnName = queryCol;
            if (outxml != null)
            {
                xdoc.LoadXml("<Root>" + outxml.ToString() + "</Root>");

                XmlNode nodeRows = xdoc.SelectSingleNode("Root/" + ColoumnName + "/RowList");
                DataSet dsRowList = new DataSet();
                if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    dsAutoFill.ReadXml(read);
                    dsAutoFill.Tables["Rows"].TableName = ColoumnName;
                    dsAutoFill.AcceptChanges();
                }
            }

            StringBuilder JsonString = new StringBuilder();

            if (ColoumnName != "")
            {
                if (dsAutoFill != null)
                {
                    if (dsAutoFill.Tables.Count > 0)
                    {
                        if (dsAutoFill.Tables[0].Rows.Count > 0)
                        {
                            bool commaValPresent = false;
                            if (dsAutoFill.Tables[0].Columns.Contains("CommaValue"))
                            {
                                commaValPresent = true;
                            }
                            bool COAPresent = false;
                            if (dsAutoFill.Tables[0].Columns.Contains("COA"))
                            {
                                COAPresent = true;
                            }


                            DataRow[] foundrows;
                            string strSort = ColoumnName.Trim() + " ASC";              //"SortOrder ASC";

                            if (COAPresent && strCOA != string.Empty)
                            {
                                foundrows = dsAutoFill.Tables[0].Select(ColoumnName.Trim() + " like '%" + Prefix.Replace("'", "''").Trim() + "%' AND COA=" + strCOA, strSort);
                            }
                            else
                            {
                                foundrows = dsAutoFill.Tables[0].Select(ColoumnName.Trim() + " like '%" + Prefix.Replace("'", "''").Trim() + "%'", strSort);
                            }
                            for (int j = 0; j < foundrows.Length; j++)
                            {
                                if (j == limit) break;
                                //"$key|$value\n" DataValueField;
                                //Get only top m_AutoFillItemsCount items  + foundrows[j]["CommaValue"].ToString() + "|" + "\n"
                                JsonString.Append(foundrows[j][ColoumnName.Trim()].ToString() + "|" + foundrows[j]["DataValueField"].ToString() + "|");

                                if (commaValPresent)
                                {
                                    JsonString.Append(foundrows[j]["CommaValue"].ToString() + "|" + "\n");
                                }
                                else
                                {
                                    JsonString.Append("\n");
                                }
                            }
                        }
                    }
                }
            }

            if (JsonString.Length == 0)
            {
                JsonString.Append("Data not available| | ");
            }

            return JsonString.ToString();
        } */

        #endregion
    }
}
