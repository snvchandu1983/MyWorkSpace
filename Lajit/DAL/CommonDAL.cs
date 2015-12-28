using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
using System.IO;

namespace LAjit_DAL
{
    // DAL  Class
    public class CommonDAL
    {
        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["LAjitDev_ConnectionString"].ToString();


        private static string m_ErrorLogPath = ConfigurationManager.AppSettings["ErrorLogPath"];

        private void LogRequestXML(string xml, string fileName)
        {
            try
            {
                string isLogging = ConfigurationManager.AppSettings["IOLogging"];
                if (isLogging == "0")
                {
                    return;
                }

                if (!Directory.Exists(m_ErrorLogPath))
                {
                    Directory.CreateDirectory(m_ErrorLogPath);
                }
                string serverMappedPath = m_ErrorLogPath + @"\" + fileName + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
                //Extract the BPINFO from the XML 
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xml);
                XmlNode node = xDoc.SelectSingleNode("//bpinfo");
                if (node != null)
                {
                    xml = node.OuterXml;
                }
                else
                {
                    node = xDoc.SelectSingleNode("//bpeout");
                    if (node != null)
                    {
                        XmlNode nodeFI = node.SelectSingleNode("FormInfo");
                        XmlNode nodeMI = node.SelectSingleNode("MessageInfo");
                        if (nodeFI != null)
                        {
                            xml = nodeFI.OuterXml;
                        }
                        if (nodeMI != null)
                        {
                            xml += nodeMI.OuterXml;
                        }
                    }
                }

                if (!File.Exists(serverMappedPath))
                {
                    File.Create(serverMappedPath).Close();
                }
                using (StreamWriter swLogger = File.AppendText(serverMappedPath))
                {
                    swLogger.WriteLine(swLogger.NewLine + "Log Entry:{0}", DateTime.Now.ToString());
                    swLogger.WriteLine(xml);//Log the passed error
                    swLogger.WriteLine(swLogger.NewLine + "------------------------------------------------------------------------------------------------");
                    swLogger.Flush();
                    swLogger.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Gets the processed output XML from the XML processor for the corresponding input XML
        /// </summary>
        /// <param name="inputXML">The XML to process </param>
        /// <returns>Processed XML</returns>
        public string GetBPEOut(string inputXML)
        {
            //XmlReader vXreader;
            //XmlDataDocument oXML = new XmlDataDocument();
            LogRequestXML(inputXML, "LAjitBPINFOLog");

            SqlConnection con = new SqlConnection(connString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.CommandText = "BPE";
                com.CommandType = CommandType.StoredProcedure;
                com.Connection = con;
                com.CommandTimeout = 400;
                // This would wait indefinitely for the procedure to execute. Added bcos of the Bob Upload. Should be commented ASAP.
                //com.CommandTimeout = 0;

                SqlParameter XmlParameters = new SqlParameter();
                XmlParameters.Direction = ParameterDirection.Input;
                XmlParameters.ParameterName = "@BPE_Info";
                XmlParameters.Value = inputXML;
                XmlParameters.DbType = DbType.Xml;
                com.Parameters.Add(XmlParameters);

                SqlParameter parmReturnXML = new SqlParameter();
                parmReturnXML.ParameterName = "@ReturnDOC";
                parmReturnXML.Direction = ParameterDirection.Output;
                parmReturnXML.SqlDbType = SqlDbType.Xml;
                parmReturnXML.Size = 1;
                com.Parameters.Add(parmReturnXML);

                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                LogRequestXML(parmReturnXML.Value.ToString(), "LAjitBPOUTLog");
                return parmReturnXML.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Runs a SP against a given input XML
        /// </summary>
        /// <param name="inputXML">Input Parameter</param>
        /// <param name="procedurename">The name of the proc to run.</param>
        /// <returns>Output XML</returns>
        public string RunStoredProcedure(string inputXML, string StoredProcedureName)
        {
            //XmlReader vXreader;
            //XmlDataDocument oXML = new XmlDataDocument();

            SqlConnection con = new SqlConnection(connString);
            SqlCommand com = new SqlCommand();
            try
            {
                //com.CommandText = "AutoFillVendorList";
                com.CommandText = StoredProcedureName;
                com.CommandType = CommandType.StoredProcedure;
                com.Connection = con;
                com.CommandTimeout = 200;

                SqlParameter XmlParameters = new SqlParameter();
                XmlParameters.Direction = ParameterDirection.Input;
                XmlParameters.ParameterName = "@BPE_Info";
                XmlParameters.Value = inputXML;
                XmlParameters.DbType = DbType.Xml;
                com.Parameters.Add(XmlParameters);

                SqlParameter parmReturnXML = new SqlParameter();
                parmReturnXML.ParameterName = "@ReturnDOC";
                parmReturnXML.Direction = ParameterDirection.Output;
                parmReturnXML.SqlDbType = SqlDbType.Xml;
                parmReturnXML.Size = 1;
                com.Parameters.Add(parmReturnXML);

                int timeout = con.ConnectionTimeout;


                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return parmReturnXML.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        //public string GetVendors(string inputXML)
        //{
        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_GET_VENDORS";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string UpdateShortCuts(string inputXML)
        //{
        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_GET_VENDORS";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetDataForCPGV1(string inputXML)
        //{
        //    SqlConnection con = new SqlConnection("Data Source=172.20.107.151;Initial Catalog=LAJIT;User ID=sa;Password=sql2005;");
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_GET_VENDORS";
        //    com.CommandText = "[USP_GET_DATA_CPGV1]";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        ///// <summary>
        ///// Gets the processed output XML from the XML processor for the corresponding input XML
        ///// </summary>
        ///// <param name="inputXML">The XML to process </param>
        ///// <returns>Processed XML</returns>
        //public string AddThisVendor(string inputXML)
        //{
        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_ADD_VENDOR_INFO";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        ///// <summary>
        ///// Gets the processed output XML from the XML processor for the corresponding input XML
        ///// </summary>
        ///// <param name="inputXML">The XML to process </param>
        ///// <returns>Processed XML</returns>
        //public string DeleteVendor(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_DELETE_VENDOR";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        ///// <summary>
        ///// Gets the processed output XML from the XML processor for the corresponding input XML
        ///// </summary>
        ///// <param name="inputXML">The XML to process </param>
        ///// <returns>Processed XML</returns>
        //public string ModifyVendor(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_MODIFY_VENDOR";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetLoginInfo(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    com.CommandText = "[BPE]";
        //   // com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetVendFormInfo(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "USP_GET_VENDOR_FORM_INFO";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetModVendorFormInfo(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "[USP_GET_MOD_VENDOR_FORM_INFO]";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetVendorDetails(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    //com.CommandText = "[USP_GET_VENDOR_DETAILS]";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string FindThisVendor(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();


        //    //com.CommandText = "[USP_FIND_THIS_VENDOR]";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetVendorHistory(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();


        //    //com.CommandText = "USP_GET_VENDOR_HISTORY";
        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}

        //public string GetCOAFormInfo(string inputXML)
        //{
        //    XmlReader vXreader;
        //    XmlDataDocument oXML = new XmlDataDocument();

        //    SqlConnection con = new SqlConnection(connString);
        //    SqlCommand com = new SqlCommand();

        //    com.CommandText = "BPE";
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Connection = con;

        //    SqlParameter XmlParameters = new SqlParameter();
        //    XmlParameters.Direction = ParameterDirection.Input;
        //    XmlParameters.ParameterName = "@BPE_Info";
        //    XmlParameters.Value = inputXML;
        //    XmlParameters.DbType = DbType.Xml;
        //    com.Parameters.Add(XmlParameters);

        //    SqlParameter parmReturnXML = new SqlParameter();
        //    parmReturnXML.ParameterName = "@ReturnDOC";
        //    parmReturnXML.Direction = ParameterDirection.Output;
        //    parmReturnXML.SqlDbType = SqlDbType.Xml;
        //    parmReturnXML.Size = 1;
        //    com.Parameters.Add(parmReturnXML);

        //    con.Open();
        //    com.ExecuteNonQuery();
        //    con.Close();
        //    return parmReturnXML.Value.ToString();
        //}
    }
}
