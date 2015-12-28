using System;
using System.Configuration;
using System.Web;
using NLog;

namespace LAjitDev
{
    public class Global : System.Web.HttpApplication
    {
       
        protected void Session_End(object sender, EventArgs e)
        {
             NLog.Logger logger = LogManager.GetCurrentClassLogger();
             logger.Debug("An active user session has ended.");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //string SessionId = Session.SessionID; ;
            SetCdnPaths();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        

        //It will be executed whenever Error occures in any page
        protected void Application_Error(object sender, EventArgs e)
        {
            //Capturing the Runtime error
            HttpContext context = HttpContext.Current;
            Exception ex = context.Server.GetLastError().GetBaseException();
            string referer = string.Empty;
            try
            {
                if (context.Request.ServerVariables["HTTP_REFERER"] != null)
                {
                    context.Request.ServerVariables["HTTP_REFERER"].ToString();
                }
            }
            catch (NullReferenceException nre)
            {
                  NLog.Logger logger = LogManager.GetCurrentClassLogger();
                #region NLog
                logger.Fatal(nre); 
                #endregion

                referer = nre.Message;
            }

            string sQuery = (context.Request.QueryString != null) ? context.Request.QueryString.ToString() : string.Empty;
            string strErrorData = "\r\nSOURCE: " + ex.Source + "\r\nMESSAGE: " + ex.Message +
                    "\r\nTARGETSITE: " + ex.TargetSite + "\r\nSTACKTRACE: " + ex.StackTrace + "\r\nREFERER: " + referer;
           // ErrorLogger.LogError(strErrorData, LogType.LogFile);




            //if (exc.Message != null & exc.Message != "")
            //{
            //Response.Write(exc.Message);
            //LAjit_BO.Mail obj = new Mail();
            //obj.Subject = "Error Occured in the Application";
            //StringBuilder MailBody = new StringBuilder();

            ////For HTML body display
            ////MailBody.Append("<table border=1><tr><td>Employee Name:</td><td>" + Session.Contents["LoggedInUserName"].ToString() + "<br>" +
            ////           "</td></tr><br><tr><td>Error Type:</td><td>" + exc.Message.ToString() + "<br>" +
            ////           "</td></tr><br><tr><td>Error Details:</td><td>" + exc.InnerException.Message.ToString() + "<br>" +
            ////           "</td></tr><br><tr><td>Error Page:</td><td>" + Request.CurrentExecutionFilePath.ToString() + "</td></tr></table>");

            //string str = MailBody.ToString();
            //obj.Body = str;

            ////Calling the Function whenever Error occures
            ////obj.SendMail();

            ////Redirecting the user to error page along with the encrypted Error Source Page Path.
            //string EncryptedErrorFilePath = clsEncryptDecryptBO.Encrypt(Request.CurrentExecutionFilePath.ToString()).ToString();
            //Response.Redirect("../Common/ContentError.aspx?src=" + EncryptedErrorFilePath.ToString());
            //}
        }
        #region Private Methods
        private void SetCdnPaths()
        {

            //#region NLog
            //logger.Info("Setting the CDN (Content Delivery Network) paths to the application."); 
            //#endregion

           string g_ScriptCDNPath = string.Empty;
           //string g_ImagesCDNPath = string.Empty;
           string g_VirtualPath = string.Empty;

           //Set Javascript CDN Path
           g_ScriptCDNPath = ConfigurationManager.AppSettings["ScriptsCDNPath"].ToString();
           Application.Add("ScriptsCDNPath", g_ScriptCDNPath);
    
           //Set Images CDN Path
           //g_ImagesCDNPath = ConfigurationManager.AppSettings["ImagesCDNPath"].ToString();
           //Application.Add("ImagesCDNPath", g_ImagesCDNPath);
           
           //Set Virtual Path for handlers
           //HttpContext context = HttpContext.Current;
           //g_VirtualPath = context.Request.Url.GetLeftPart(System.UriPartial.Authority) + context.Request.ApplicationPath;

           g_VirtualPath = ConfigurationManager.AppSettings["HostedURL"].ToString();
            Application.Add("VirtualPath", g_VirtualPath);
        }
        #endregion

    }
}