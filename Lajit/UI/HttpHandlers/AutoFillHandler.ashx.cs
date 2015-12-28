using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Xml;
using System.Text;
using LAjitDev.Classes;
using NLog;


namespace LAjitDev.HttpHandlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AutoFillHandler : IHttpHandler, IRequiresSessionState
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private HttpContext m_Context;
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            #region NLog
            logger.Info("This is to process the request context : " + context); 
            #endregion

            m_Context = context;
            //if (!context.Request.UrlReferrer.AbsolutePath.ToUpper().Contains("GENVIEW.ASPX")
            //    && !context.Request.UrlReferrer.AbsolutePath.ToUpper().Contains("GENPROCESSENGINE.ASPX"))
            //{
            string key = context.Request.Params["contextkey"];
            string query = context.Request.Params["q"];
            string maxCount = context.Request.Params["limit"];
            string jobCOA = context.Request.Params["COA"];
            if (key != null && query != null)
            {
                if (query.Length > 0)
                {
                    int limit = 20;
                    string COA = string.Empty;
                    if (maxCount != null)
                    {
                        limit = Int32.Parse(maxCount);
                    }
                    if (jobCOA != null)
                    {
                        COA = jobCOA;
                    }
                    string response = GetAutoFillRows(key, query, limit, COA);
                    context.Response.Write(response);
                }
            }
            //}
            //else
            //{
            //    // In use for jqGrid AutoFills.
            //    int resultCount = Convert.ToInt32(context.Request["limit"]);
            //    string COA = context.Request.Params["COA"] != null ? context.Request.Params["COA"] : "";
            //    string results = GetAutoFillData(context.Request.Params["contextkey"].ToString(), context.Request.Params["q"].ToString(), resultCount, COA);
            //    context.Response.Write(results);
            //}
            m_Context = null;
        }

        private string GetAutoFillRows(string contextKey, string prefixText, int limit, string COA)
        {
            #region NLog
            logger.Info("This is Get all the auto fill rows with context key : " + contextKey + " prefixtext : " + prefixText + " limit : " + limit + " and COA : " + COA); 
            #endregion

            contextKey = AutoFill.GetSPContextKey(contextKey);
            CommonUI objCommonUI = new CommonUI();
            prefixText = objCommonUI.HtmlEncode(prefixText);
            XmlNode nodeRowList = AutoFill.GetAutoFillDataNode(prefixText, "AutoFillGeneric", contextKey, COA);
            if (nodeRowList == null || nodeRowList.FirstChild == null)
            {
                string retString = "|<b>Data not found.";
                contextKey = contextKey.Replace("AutoFill", "");
                if (contextKey == "Vendor" || contextKey == "Customer")
                {
                    if ((m_Context.Request.Params["page"] == "ASP.payables_apminvoice_aspx")
                        || (m_Context.Request.Params["page"] == "ASP.payables_apinvoice_aspx")
                        || (m_Context.Request.Params["page"] == "ASP.purchasing_purchaseorder_aspx")
                        || (m_Context.Request.Params["page"] == "ASP.payables_manualcheck_aspx")
                        || (m_Context.Request.Params["page"] == "ASP.receivables_arinvoice_aspx"))
                    {
                        retString += " Click on Add icon to add a new " + contextKey + ".";
                    }
                }
                return retString + "</b>|";
            }

            string multiple = m_Context.Request.Params["multiple"];
            bool isMultiple = (multiple == "1") ? true : false;

            //Loop through the matched rows and frame the return pipe-delimited string.
            StringBuilder sbAF = new StringBuilder();
            for (int i = 0; i < nodeRowList.FirstChild.ChildNodes.Count; i++)
            {
                XmlNode nodeRow = nodeRowList.FirstChild.ChildNodes[i];
                if (i == limit) break;
                string autoFillId = nodeRow.Attributes["DataValueField"].Value;//TrxID~TrxType
                string text = nodeRow.Attributes[contextKey.Trim()].Value;
                sbAF.Append(autoFillId + "|" + text);
                bool thirdParamAppended = false;

                switch (contextKey)
                {
                    case "AutoFillJob":
                        {
                            //AutofillID | Text | COA
                            sbAF.Append("|" + nodeRow.Attributes["COA"].Value);
                            thirdParamAppended = true;
                            break;
                        }
                    case "SelectCustInvoice":
                        {
                            //AutofillID | Text | Owed
                            sbAF.Append("|" + nodeRow.Attributes["Owed"].Value);
                            thirdParamAppended = true;
                            break;
                        }
                    case "AutoFillVendor":
                        {
                            //APMInvoice AutoFill Vendor- set defaults in ChildGrid TypeOf1099 DDL based on
                            //Vendor selection.
                            XmlAttribute attTypeOf1099 = nodeRow.Attributes["TypeOf1099"];
                            if (attTypeOf1099 != null)
                            {
                                sbAF.Append("|" + attTypeOf1099.Value);
                                thirdParamAppended = true;
                            }
                            break;
                        }
                    case "helpcatalog":
                        {
                            //AutofillID | Text | BPGID
                            sbAF.Append("|" + nodeRow.Attributes["BPGID"].Value);
                            thirdParamAppended = true;
                            break;
                        }
                    default:
                        break;
                }


                if (isMultiple)
                {
                    XmlAttribute attCommaValue = nodeRow.Attributes["CommaValue"];
                    string commaVal = string.Empty;
                    if (attCommaValue != null && attCommaValue.Value.Length > 0)
                    {
                        commaVal = attCommaValue.Value;
                    }
                    else
                    {
                        //If no Comma Value is coming from the XML send the text itself as the comma value.
                        commaVal = text;
                    }

                    //Make sure the comma value appears at the fourth place.
                    //AutofillID | Text | [COA/Owed/1099/helpBPGID] | CommaValue
                    if (thirdParamAppended)
                    {
                        sbAF.Append("|" + commaVal);
                    }
                    else
                    {
                        sbAF.Append("||" + commaVal);
                    }
                }

                sbAF.Append("\n");
            }
            return sbAF.ToString();
        }

        #region Obsolete

        //private string GetAutoFillData(string label, string prefix, int resultCount, string strCOA)
        //{
        //    string json = LAjitDev.Classes.AutoFill.GetAutoFillHandlerData(prefix, label, resultCount, strCOA);

        //    ////Read the data from a temp file for now.
        //    //XmlDocument xDoc = new XmlDocument();
        //    //xDoc.Load(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\LedgerReport.xml");
        //    //XmlNode nodeRowList = xDoc.SelectSingleNode("Root/bpeout/FormControls/"+label+"/RowList");
        //    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    //foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
        //    //{
        //    //    sb.Append(nodeRow.Attributes[label].Value + "|" + nodeRow.Attributes["DataValueField"].Value + "\n");
        //    //}
        //    return json;
        //}



        ///// <summary>
        ///// Data retrive from cache
        ///// </summary>
        ///// <param name="contextKey"></param>
        ///// <param name="prefixText"></param>
        ///// <param name="limit"></param>
        //private string GetJSON_CacheData(string contextKey, string prefixText, int limit, string COA)
        //{
        //    DataSet dsAutoFill = null;
        //    string cacheName = AutoFill.GetLoggedInCompanyID() + contextKey;
        //    StringBuilder JsonString = new StringBuilder();

        //    //To fetch cache dataset data
        //    if (contextKey != null && m_Context.Cache[cacheName] != null)
        //    {
        //        dsAutoFill = (DataSet)m_Context.Cache[cacheName];
        //    }
        //    else
        //    {
        //        if ((dsAutoFill == null) && (contextKey != null))
        //        {
        //            AutoFill.FillAutoDataCache(contextKey);
        //            dsAutoFill = (DataSet)m_Context.Cache[cacheName];
        //        }
        //    }

        //    if (contextKey != "")
        //    {
        //        if (dsAutoFill.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow[] foundrows;
        //            string strSort = contextKey.Trim() + " ASC";
        //            //Remove SQL LIKE SPECIAL CHARACTER
        //            prefixText = prefixText.Replace("[", "");
        //            prefixText = prefixText.Replace("]", "");
        //            switch (contextKey)
        //            {
        //                case "AutoFillVendor":
        //                case "Customer":
        //                case "SelectCustInvoice":
        //                    {
        //                        //WILD CARD SEARCH
        //                        foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '%" + prefixText.Replace("'", "''").Trim() + "%'", strSort);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        if ((COA != string.Empty) && (COA != "0"))
        //                        {
        //                            if (prefixText.Contains("%") || prefixText.Contains("*"))
        //                            {
        //                                //WILD CARD SEARCH
        //                                prefixText = prefixText.Replace("%", "");
        //                                prefixText = prefixText.Replace("*", "");
        //                                foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '%" + prefixText.Replace("'", "''").Trim() + "%' AND COA=" + COA, strSort);
        //                            }
        //                            else
        //                            {
        //                                //STARTS WITH SEARCH
        //                                foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '" + prefixText.Replace("'", "''").Trim() + "%' AND COA=" + COA, strSort);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (prefixText.Contains("%") || prefixText.Contains("*"))
        //                            {
        //                                //WILD CARD SEARCH
        //                                prefixText = prefixText.Replace("%", "");
        //                                prefixText = prefixText.Replace("*", "");
        //                                foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '%" + prefixText.Replace("'", "''").Trim() + "%'", strSort);
        //                            }
        //                            else
        //                            {
        //                                //STARTS WITH SEARCH
        //                                foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '" + prefixText.Replace("'", "''").Trim() + "%'", strSort);
        //                            }
        //                        }
        //                        break;
        //                    }
        //            }

        //            //Loop through the matched rows and frame the return pipe-delimited string.
        //            for (int j = 0; j < foundrows.Length; j++)
        //            {
        //                if (j == limit) break;
        //                string autoFillId = foundrows[j]["DataValueField"].ToString();//TrxID~TrxType
        //                string text = foundrows[j][contextKey.Trim()].ToString();

        //                JsonString.Append(autoFillId + "|" + text);
        //                if (contextKey == "AutoFillJob")
        //                {
        //                    //AutofillID | Text | COA
        //                    JsonString.Append("|" + foundrows[j]["COA"].ToString());
        //                }
        //                else if (contextKey == "SelectCustInvoice")
        //                {
        //                    //AutofillID | Text | Owed
        //                    JsonString.Append("|" + foundrows[j]["Owed"].ToString());
        //                }
        //                else if (contextKey == "AutoFillVendor")
        //                {
        //                    //APMInvoice AutoFill Vendor- set defaults in ChildGrid TypeOf1099 DDL based on
        //                    //Vendor selection.
        //                    if (dsAutoFill.Tables[0].Columns.Contains("TypeOf1099"))
        //                    {
        //                        JsonString.Append("|" + foundrows[j]["TypeOf1099"].ToString());
        //                    }
        //                }

        //                JsonString.Append("\n");
        //            }

        //            //if (JsonString.Length > 0)
        //            //    JsonString.Remove(0, JsonString.Length - 1);
        //            //JsonString.Append("{\"id\":1,\"txt\":\"Danny\",\"coa\":11}\n{\"id\":2,\"txt\":\"Danny2\",\"coa\":11}");
        //        }
        //    }
        //    if (JsonString.Length == 0)
        //    {
        //        switch (contextKey)
        //        {
        //            case "AutoFillVendor":
        //                if (m_Context.Request.Params["page"] != null)
        //                {
        //                    if ((m_Context.Request.Params["page"] == "ASP.payables_apminvoice_aspx") || (m_Context.Request.Params["page"] == "ASP.payables_apinvoice_aspx") || (m_Context.Request.Params["page"] == "ASP.purchasing_purchaseorder_aspx") || (m_Context.Request.Params["page"] == "ASP.payables_manualcheck_aspx"))
        //                    {
        //                        JsonString.Append(" |Data not available. Click textbox icon to add |");
        //                    }
        //                    else
        //                    {
        //                        JsonString.Append(" |Data not available|");
        //                    }
        //                }
        //                else
        //                {
        //                    JsonString.Append(" |Data not available|");
        //                }
        //                break;
        //            case "AutoFillJob":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillAccount":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillSubAccount1":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillSubAccount2":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillSubAccount3":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillSubAccount4":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AutoFillSubAccount5":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "Customer":
        //                if (m_Context.Request.Params["page"] != null)
        //                {
        //                    if ((m_Context.Request.Params["page"] == "ASP.receivables_arinvoice_aspx"))
        //                    {
        //                        JsonString.Append(" |Data not available. Click textbox icon to add |");
        //                    }
        //                    else
        //                    {
        //                        JsonString.Append(" |Data not available|");
        //                    }
        //                }
        //                else
        //                {
        //                    JsonString.Append(" |Data not available|");
        //                }
        //                break;
        //            case "SelectCustInvoice":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            case "AREntryJob":
        //                JsonString.Append(" |Data not available| ");
        //                break;
        //            default:
        //                JsonString.Append(" |Data not available| ");
        //                break;

        //        }
        //    }
        //    return JsonString.ToString();
        //}

        ///// <summary>
        ///// GetData on each request from DB
        ///// </summary>
        ///// <param name="contextKey"></param>
        ///// <param name="prefixText"></param>
        ///// <param name="limit"></param>
        ///// <returns></returns>
        //private string GetJSON_DBHelpData(string contextKey, string prefixText, int limit)
        //{

        //    string DataString = string.Empty;
        //    string storedProcedureName = "AutoFillHelpList";
        //    DataSet dsAutoFill = new DataSet();
        //    //string cacheName = AutoFill.GetLoggedInCompanyID() + contextKey.Trim();

        //    //To fetch cache dataset data
        //    string OuterXml = string.Empty;

        //    OuterXml = AutoFill.GetAutoFillData("", storedProcedureName, "", "");
        //    XmlDocument xdoc = new XmlDocument();


        //    if (OuterXml != null)
        //    {
        //        xdoc.LoadXml("<Root>" + OuterXml.ToString() + "</Root>");

        //        XmlNode nodeRows = xdoc.SelectSingleNode("Root/" + contextKey + "/RowList");
        //        DataSet dsRowList = new DataSet();
        //        if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
        //        {
        //            XmlNodeReader read = new XmlNodeReader(nodeRows);
        //            dsAutoFill.ReadXml(read);
        //            dsAutoFill.Tables["Rows"].TableName = contextKey;
        //            dsAutoFill.AcceptChanges();
        //        }
        //    }




        //    StringBuilder JsonString = new StringBuilder();

        //    // AutoFill.GetAutoFillDatails(contextKey, dsAutoFill);

        //    if (contextKey != "")
        //    {
        //        if (dsAutoFill != null)
        //        {
        //            if (dsAutoFill.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow[] foundrows;
        //                string strSort = contextKey.Trim() + " ASC";              //"SortOrder ASC";
        //                foundrows = dsAutoFill.Tables[0].Select(contextKey.Trim() + " like '%" + prefixText.Replace("'", "''").Trim() + "%'", strSort);

        //                for (int j = 0; j < foundrows.Length; j++)
        //                {
        //                    if (j == limit) break;
        //                    //"$key|$value\n" DataValueField;
        //                    //Get only top m_AutoFillItemsCount items
        //                    if (JsonString.Length == 0)
        //                    {
        //                        //JsonString.Append("\n{Id:\"" + foundrows[j][0].ToString() + "\",Value:\"" + foundrows[j][contextKey.Trim()].ToString() + "\"}");
        //                        JsonString.Append(foundrows[j][0].ToString() + "|" + foundrows[j][contextKey.Trim()].ToString() + "|" + foundrows[j]["BPGID"].ToString() + "\n");
        //                    }
        //                    else
        //                    {
        //                        //JsonString.Append(",{\"Value\":\"" + foundrows[j][contextKey.Trim()].ToString() + "\"}");
        //                        //JsonString.Append(",\n{Id:\"" + foundrows[j][0].ToString() + "\",Value:\"" + foundrows[j][contextKey.Trim()].ToString() + "\"}");
        //                        JsonString.Append(foundrows[j][0].ToString() + "|" + foundrows[j][contextKey.Trim()].ToString() + "|" + foundrows[j]["BPGID"].ToString() + "\n");
        //                    }


        //                }
        //            }
        //        }
        //    }

        //    if (JsonString.Length == 0)
        //    {
        //        JsonString.Append(" |Data not available| ");
        //    }

        //    return JsonString.ToString();

        //} 

        #endregion
    }
}
