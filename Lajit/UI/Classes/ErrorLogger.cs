using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace LAjitDev.Classes
{
    /// <summary>
    /// Type of the logging desired which specifies the destination objects.
    /// </summary>
    public enum LogType
    {

        /// <summary>
        /// Log error to Windows Event Log
        /// </summary>
        EventLog,
        /// <summary>
        /// Send the error to a Mail-recepient
        /// </summary>
        Mail,
        /// <summary>
        /// Write to a log file.
        /// </summary>
        LogFile,
        /// <summary>
        /// Mail and write to a log file.
        /// </summary>
        MailAndLogFile,
        /// <summary>
        /// Write to event log and write to log file.
        /// </summary>
        EventLogAndLogFile,
        /// <summary>
        /// Send Mail and write to event log.
        /// </summary>
        MailAndEventLog,
        /// <summary>
        /// Send Mail, write to Windows Event Log and write to a log file.
        /// </summary>
        All
    }

    public class ErrorLogger
    {
        private static string m_ErrorLogPath = ConfigurationManager.AppSettings["ErrorLogPath"];

        public static void Log(string str)
        {
            System.Diagnostics.Debugger.Log(1, "Logging", "\n" + str);
        }

        /// <summary>
        /// Logs the passed error message to the destination specified in the Config file.
        /// </summary>
        /// <param name="errorMessage">Error to be logged.</param>
        public static void LogError(string errorMessage)
        {
            LogError(errorMessage, LogType.LogFile);
        }

        /// <summary>
        /// Logs the passed error message to the destination specified.
        /// </summary>
        /// <param name="errorMessage">Error to be logged.</param>
        /// <param name="logType">The destination where the logging has to be done.</param>
        public static void LogError(string errorMessage, LogType logType)
        {
            switch (logType)
            {
                case LogType.LogFile:
                    {
                        WriteToFile(errorMessage);
                        break;
                    }
                case LogType.Mail:
                    {
                        SendMail(errorMessage);
                        break;
                    }
                case LogType.EventLog:
                    {
                        WriteToEventLog(errorMessage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Writes the passed error message to a text file.
        /// </summary>
        /// <param name="errorMessage">The error to log.</param>
        private static void WriteToFile(string errorMessage)
        {
            try
            {
                //if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/ErrorLog")))
                //{
                //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/ErrorLog"));
                //}
                if (!Directory.Exists(m_ErrorLogPath))
                {
                    Directory.CreateDirectory(m_ErrorLogPath);
                }

                string serverMappedPath = m_ErrorLogPath + @"\LAjitErrorLog" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
                //string path = "C:\\LAjitErrorLog" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
                //string serverMappedPath = HttpContext.Current.Server.MapPath(path);
                if (!File.Exists(serverMappedPath))
                {
                    File.Create(serverMappedPath).Close();
                }

                using (StreamWriter swLogger = File.AppendText(serverMappedPath))
                {
                    swLogger.WriteLine(swLogger.NewLine + "Log Entry:{0}", DateTime.Now.ToString());
                    swLogger.WriteLine(errorMessage);//Log the passed error
                    swLogger.WriteLine(GetUserInfo());//Log the LAjit User Information
                    swLogger.WriteLine(GetBrowserInfo());//Log the User's Browser Information.
                    swLogger.WriteLine(swLogger.NewLine + "------------------------------------------------------------------------------------------------");
                    swLogger.Flush();
                    swLogger.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Writes the error message to Windows Event Log
        /// </summary>
        /// <param name="errorMessage">The error to log.</param>
        private static void WriteToEventLog(string errorMessage)
        {
            try
            {
                string eventLogName = "LAjit";
                //Create our own log if it not already exists
                if (!EventLog.SourceExists(eventLogName))
                {
                    EventLog.CreateEventSource(eventLogName, eventLogName);
                }
                EventLog myLog = new EventLog();
                myLog.Source = eventLogName;
                myLog.WriteEntry(errorMessage, EventLogEntryType.Warning);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Send the error message as a mail.
        /// </summary>
        /// <param name="errorMessage">The error to log.</param>
        private static void SendMail(string errorMessage)
        {
            LAjit_BO.Mail objMailer = new LAjit_BO.Mail();
            objMailer.To = "daniel.devarampally@valuelabs.net";
            objMailer.Subject = "Error";
            objMailer.HtmlBody = false;
            objMailer.Body = errorMessage;
            objMailer.SendMail();
        }

        /// <summary>
        /// Gets the LAjit User information from the Session object
        /// </summary>
        /// <returns>Formattted string.</returns>
        private static string GetUserInfo()
        {
            try
            {
                CommonUI commonObjUI = new CommonUI();
                System.Xml.XmlDocument XDocUserInfo = new System.Xml.XmlDocument();
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
                string userInfo = "\r\nUSER INFO:\r\n" +
                       "User ID = " + XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/UserID").InnerText + "\r\n" +
                       "User Full Name = " + XDocUserInfo.SelectSingleNode("Root/bpe/userinfo/FullName").InnerText + "\r\n" +
                       "Company ID = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText + "\r\n" +
                       "Company Name = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText + "\r\n" +
                       "Entity ID = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/EntityID").InnerText + "\r\n" +
                       "Entity Name = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/EntityName").InnerText + "\r\n" +
                       "Role ID = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/RoleID").InnerText + "\r\n" +
                       "Role Name = " + XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/RoleName").InnerText;
                return userInfo;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Gets the information of the remote user's browser.
        /// </summary>
        /// <returns>Formattted string.</returns>
        private static string GetBrowserInfo()
        {
            try
            {
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                string browserInfo = "\r\nBROWSER INFORMATION:\r\n"
                    + "IP Address = " + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() + "\r\n"
                    + "System User Name = " + HttpContext.Current.User.Identity.Name + "\r\n"
                    + "Type = " + browser.Type + "\r\n"
                    + "Name = " + browser.Browser + "\r\n"
                    + "Version = " + browser.Version + "\r\n"
                    + "Major Version = " + browser.MajorVersion + "\r\n"
                    + "Minor Version = " + browser.MinorVersion + "\r\n"
                    + "Platform = " + browser.Platform + "\r\n"
                    + "Is Beta = " + browser.Beta + "\r\n"
                    + "Is Crawler = " + browser.Crawler + "\r\n"
                    + "Is AOL = " + browser.AOL + "\r\n"
                    + "Is Win16 = " + browser.Win16 + "\r\n"
                    + "Is Win32 = " + browser.Win32 + "\r\n"
                    + "Supports Frames = " + browser.Frames + "\r\n"
                    + "Supports Tables = " + browser.Tables + "\r\n"
                    + "Supports Cookies = " + browser.Cookies + "\r\n"
                    + "Supports VBScript = " + browser.VBScript + "\r\n"
                    + "Supports JavaScript = " + browser.EcmaScriptVersion + "\r\n"
                    + "Supports Java Applets = " + browser.JavaApplets + "\r\n"
                    + "Supports BackgroundSounds = " + browser.BackgroundSounds + "\r\n"
                    + "Supports ActiveX Controls = " + browser.ActiveXControls + "\r\n"
                    + "Browser = " + browser.Browser + "\r\n"
                    + "CDF = " + browser.CDF + "\r\n"
                    + "CLR Version = " + browser.ClrVersion + "\r\n"
                    + "ECMA Script version = " + browser.EcmaScriptVersion + "\r\n"
                    + "MSDOM version = " + browser.MSDomVersion + "\r\n"
                    + "Supports tables = " + browser.Tables + "\r\n"
                    + "W3C DOM version = " + browser.W3CDomVersion;
                return browserInfo;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
