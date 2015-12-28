using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace LAjitExcelCheck
{
    public class Mail
    {
        // Declare all the private members of the class 

        // variables which are must for sending an emails 
        private string _FromAddress;
        private string _ToAddress;
        private string _MailServer;
        private string _MailSubject;
        private string _MailBody;


        // variables which are optional for sending an email
        private string _FromName;
        private string _ToName;
        private string _CcEmails; // You can send email IDs seperated by ';'.
        private string _BccEmails; // You can send email IDs seperated by ';'
        private string _AttachementPath;
        private string _ReplyTo;    // Most of the time it is From mail ID
        private string _Priority;


        // Variables which are having default values 
        private bool _boolIsBodyHTML = true; // By default the body is HTML
        private string _MailPort = "25";  // Default port is 25      
        private bool _boolEnableSsl = false; // By default SSL is disabled


        // Error details to be returned 
        private string _ErrorSource;
        private string _ErrorMessage;

        //Class Constructor 
        public Mail()
        {

            _FromName = System.Configuration.ConfigurationSettings.AppSettings["FromName"]; 
            _FromAddress = System.Configuration.ConfigurationSettings.AppSettings["FromAddress"];
            _ToAddress = System.Configuration.ConfigurationSettings.AppSettings["ToAddress"];
            _ReplyTo = System.Configuration.ConfigurationSettings.AppSettings["FromAddress"];
            _MailServer = System.Configuration.ConfigurationSettings.AppSettings["MailServer"];
        }

        // Property set and get methods for all  the variables 
        public string ErrorNumber
        {
            get { return _ErrorSource; }
            set { _ErrorSource = value; }
        }
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        public string From
        {
            get { return _FromAddress; }
            set { _FromAddress = value; }
        }
        public string To
        {
            get { return _ToAddress; }
            set { _ToAddress = value; }
        }
        public string Subject
        {
            get { return _MailSubject; }
            set { _MailSubject = value; }
        }
        public string Body
        {
            get { return _MailBody; }
            set { _MailBody = value; }
        }
        public string Cc
        {
            get { return _CcEmails; }
            set { _CcEmails = value; }
        }
        public string Bcc
        {
            get { return _BccEmails; }
            set { _BccEmails = value; }
        }
        public string Attachment
        {
            get { return _AttachementPath; }
            set { _AttachementPath = value; }
        }
        public bool HtmlBody
        {
            get { return _boolIsBodyHTML; }
            set { _boolIsBodyHTML = value; }
        }

        public bool SendMail()
        {
            ///Create mail objects 
            ///
            MailMessage lobjMail = new MailMessage();
            SmtpClient lobjSMTP = new SmtpClient();
            try
            {
                lobjMail.From = new MailAddress(this._FromAddress, this._FromName);
                lobjMail.To.Add(this._ToAddress);
                if (this._CcEmails != null)
                {
                    lobjMail.CC.Add(this._CcEmails);
                }
                if (this._BccEmails != null)
                {
                    lobjMail.Bcc.Add(this._BccEmails);
                }
                lobjMail.Body = Body;
                lobjMail.Subject = Subject;
                if (Attachment != null)
                {
                    lobjMail.Attachments.Add(new Attachment(Attachment));
                }
                lobjMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                System.Net.NetworkCredential ncLajiUser = new System.Net.NetworkCredential("alerts@LAjit.biz", "l@j!t123");
                lobjSMTP.Credentials = ncLajiUser;

                // Server details 
                lobjMail.IsBodyHtml = _boolIsBodyHTML;
                
                lobjSMTP.Host = this._MailServer;
                lobjSMTP.Port = 25;
                lobjSMTP.EnableSsl = this._boolEnableSsl;
                lobjSMTP.Timeout.Equals(90);
                lobjSMTP.Send(lobjMail);
                lobjMail.Priority = MailPriority.High;
                return true;
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message.ToString();
                _ErrorSource = ex.Source;
                return false;
            }
        }
    }
}

