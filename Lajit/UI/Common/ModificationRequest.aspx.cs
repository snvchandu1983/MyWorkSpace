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
using System.Net.Mail;
using LAjit_BO;
using LAjitControls;

namespace LAjitDev.Financials
{
    public partial class ModificationRequest : Classes.BasePage
    {
        // variables which are must for sending an emails 
        private string m_OutXML;
        // Variables which are having default values 
        private bool _boolIsBodyHTML = true; // By default the body is HTML
        private string _MailPort = "25";  // Default port is 25
        private bool _boolEnableSsl = false; // By default SSL is disabled

        protected void Page_Load(object sender, EventArgs e)
        {
            m_OutXML = commonObjUI.ButtonsUserControl.GVDataXml;
            ddlModificationStatus.Attributes.Add("onChange", "javascript:return CheckSelected('" + ddlModificationStatus.ClientID + "');");
            ddlDeveloperType.Attributes.Add("onChange", "javascript:return CheckSelected('" + ddlDeveloperType.ClientID + "');");
            ddlDeveloper2Type.Attributes.Add("onChange", "javascript:return CheckSelected('" + ddlDeveloper2Type.ClientID + "');");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if ((imgbtnSubmit.AlternateText.ToUpper() == "ADD"))
            {
                divChecks.Visible = false;
            }
            if ((imgbtnSubmit.AlternateText.ToUpper() == "MODIFY") && (ddlModificationStatus.SelectedIndex == 0) && (ddlDeveloperType.SelectedIndex == 0) && (ddlDeveloper2Type.SelectedIndex == 0))
            {
                divChecks.Disabled = false;
            }
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the Submit method by passing the content page Update Panel as parameter
            commonObjUI.SubmitEntries("ISSUBMITCLICK", updtPnlContent);
            if (imgbtnSubmit.AlternateText == "Add" || imgbtnSubmit.AlternateText == "Add & Continue" || imgbtnSubmit.AlternateText == "Modify")
            {
                GetXMLToSendMails(m_OutXML);
            }
        }

        private void GetXMLToSendMails(string m_XML)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(m_XML);

            XmlNodeList xnl = xdoc.SelectNodes("Root/bpeout/FormControls/ModificationStatus/RowList/Rows");
            string[] m_StatusTypes = new string[xnl.Count];
            string m_Owner = string.Empty;
            string m_DevManager = string.Empty;
            string m_Developer = string.Empty;
            string m_QA = string.Empty;
            string m_RevisedBy = string.Empty;
            //
            string m_From = ConfigurationManager.AppSettings["MailFrom"].ToString();
            string m_To = string.Empty;
            string m_CC = string.Empty;
            string m_BCC = string.Empty;
            string m_Subject = string.Empty;
            string m_Body = string.Empty;
            //
            if (imgbtnSubmit.AlternateText == "Add" || imgbtnSubmit.AlternateText == "Add & Continue")
            {
                m_Subject = txtDescription.Text;
                m_Body = "This is New Issue.Not Yet Assigned";
                // m_DevManager = xdoc.SelectSingleNode("//DeveloperType/RowList/Rows[@DeveloperType='Partha']").Attributes["CCEmail"].Value;
                //Partha - I had to hard code the email IDs as because the current approach is a sophistacted version of Hard Coding. Need to review this.
                m_To = "partha.maharatha@valuelabs.net";
                SendMail(m_From, m_To, "NickB@LAjit.biz", "", m_Subject, m_Body);
            }
            else
            {
                //if ((imgbtnSubmit.AlternateText.ToUpper() == "MODIFY") && (ddlModificationStatus.SelectedIndex > 0) && (ddlDeveloperType.SelectedIndex > 0) && (ddlDeveloper2Type.SelectedIndex > 0))
                if ((imgbtnSubmit.AlternateText.ToUpper() == "MODIFY"))
                {
                    StringBuilder m_CC_sb = new StringBuilder();
                    if (txtBprocess.Text != string.Empty)
                    {
                        m_Subject = txtBprocess.Text;
                    }
                    else
                    {
                        m_Subject = "";
                    }
                    ArrayList arrCheckedValues = new ArrayList();
                    foreach (Control c in divChecks.Controls)
                    {
                        if (c is HtmlInputCheckBox)
                        {
                            if (((HtmlInputCheckBox)c).Checked)
                            {
                                arrCheckedValues.Add(c.ClientID.Replace("ctl00_cphPageContents_", ""));
                            }
                        }
                    }
                    for (int i = 0; i < arrCheckedValues.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_To = arrCheckedValues[i].ToString();
                            switch (m_To)
                            {
                                case "chkOwner":
                                    if (txtRequestOwner.Text != string.Empty)
                                    {
                                        XmlNodeList m_ownrnode = xdoc.SelectNodes("//User2Type/RowList/Rows[@User2Type='" + txtRequestOwner.Text + "']");
                                        foreach (XmlNode xns in m_ownrnode)
                                        {
                                            foreach (XmlAttribute xn in xns.Attributes)
                                            {
                                                string m_OwnrMailId = string.Empty;
                                                if (xn.Name == "CCEmail")
                                                {
                                                    m_OwnrMailId = xns.Attributes["CCEmail"].Value;
                                                    m_Owner = m_OwnrMailId;
                                                }
                                            }
                                        }
                                        m_To = string.Empty;
                                        m_To = m_Owner;
                                    }
                                    break;
                                case "chkRevisedBy":
                                    m_RevisedBy = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlUser2Type.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                    m_To = string.Empty;
                                    m_To = m_RevisedBy;
                                    break;
                                case "chkDevManager":
                                    m_DevManager = xdoc.SelectSingleNode("//ModRequest/RowList/Rows").Attributes["DevManager"].Value;
                                    m_To = string.Empty;
                                    m_To = m_DevManager;
                                    break;
                                case "chkAssignedToDev":
                                    if (ddlDeveloperType.SelectedIndex != 0)
                                    {
                                        m_Developer = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlDeveloperType.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                        m_To = string.Empty;
                                        m_To = m_Developer;
                                    }
                                    break;
                                case "chkAssignedToQA":
                                    if (ddlDeveloper2Type.SelectedIndex != 0)
                                    {
                                        m_QA = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlDeveloper2Type.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                        m_To = string.Empty;
                                        m_To = m_QA;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            m_CC = arrCheckedValues[i].ToString();
                            switch (m_CC)
                            {
                                case "chkOwner":
                                    if (txtRequestOwner.Text != string.Empty)
                                    {
                                        XmlNodeList m_ownrnode = xdoc.SelectNodes("//User2Type/RowList/Rows[@User2Type='" + txtRequestOwner.Text + "']");
                                        foreach (XmlNode xns in m_ownrnode)
                                        {
                                            foreach (XmlAttribute xn in xns.Attributes)
                                            {
                                                string m_OwnrMailId = string.Empty;
                                                if (xn.Name == "CCEmail")
                                                {
                                                    m_OwnrMailId = xns.Attributes["CCEmail"].Value;
                                                    m_Owner = m_OwnrMailId;
                                                }
                                            }
                                        }
                                        m_CC = string.Empty;
                                        m_CC = m_Owner;
                                    }
                                    break;
                                case "chkRevisedBy":
                                    m_RevisedBy = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlUser2Type.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                    m_CC = string.Empty;
                                    m_CC = m_RevisedBy;
                                    break;
                                case "chkDevManager":
                                    m_DevManager = xdoc.SelectSingleNode("//ModRequest/RowList/Rows").Attributes["DevManager"].Value;
                                    m_CC = string.Empty;
                                    m_CC = m_DevManager;
                                    break;
                                case "chkAssignedToDev":
                                    if (ddlDeveloperType.SelectedIndex != 0)
                                    {
                                        m_Developer = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlDeveloperType.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                        m_CC = string.Empty;
                                        m_CC = m_Developer;
                                    }
                                    break;
                                case "chkAssignedToQA":
                                    if (ddlDeveloper2Type.SelectedIndex != 0)
                                    {
                                        m_QA = xdoc.SelectSingleNode("//User2Type/RowList/Rows[@User2Type='" + ddlDeveloper2Type.SelectedItem.Text + "']").Attributes["CCEmail"].Value;
                                        m_CC = string.Empty;
                                        m_CC = m_QA;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            m_CC_sb.Append(m_CC + ",");
                            m_CC = string.Empty;
                            m_CC = m_CC_sb.ToString();
                            m_CC = m_CC.Substring(0, m_CC.Length - 1);
                        }
                    }
                    if (ddlDeveloperType.SelectedIndex > 0 || ddlDeveloper2Type.SelectedIndex > 0)
                    {
                        if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "1")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtDeveloperNotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }
                        else if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "90")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtDeveloperNotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }
                        else if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "110" || ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "120")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtDeveloperNotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }
                        else if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "210")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtDeveloperNotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }
                        else if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "220")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtQANotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }
                        else if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "100")
                        {
                            if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                            {
                                m_Body = txtDescription.Text + "." + txtQANotes.Text + ".";
                            }
                            else
                            {
                                m_Body = "";
                            }
                            SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                        }

                        else
                        {
                            if (ddlModificationStatus.SelectedValue.ToString().Split('~').GetValue(0).ToString() == "300")
                            {
                                if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                                {
                                    m_Body = txtDescription.Text + "." + txtQANotes.Text + ".";
                                }
                                else
                                {
                                    m_Body = "";
                                }
                                SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                            }
                        }

                    }
                    else
                    {
                        if (txtDescription.Text != string.Empty || txtDeveloperNotes.Text != string.Empty)
                        {
                            m_Body = txtDescription.Text + "." + txtQANotes.Text + ".";
                        }
                        else
                        {
                            m_Body = "";
                        }
                        SendMail(m_From, m_To, m_CC, "", m_Subject, m_Body);
                    }
                }
            }
        }

        public bool SendMail(string m_From, string m_To, string m_CC, string m_BCC, string m_Subject, string m_Body)
        {
            ///Create mail objects 
            ///
            MailMessage lobjMail = new MailMessage();
            SmtpClient lobjSMTP = new SmtpClient();
            if (m_To != string.Empty)
            {
                m_To = m_To + ";" + "NickB@LAjit.biz";
                try
                {

                    if (m_From.Contains(";"))
                    {
                        string[] mails = new string[m_From.Split(';').Length];
                        mails = m_From.Split(';');
                        lobjMail.From = new MailAddress(mails[0].ToString());
                        lobjMail.CC.Add(mails[1].ToString());
                    }
                    else
                    {
                        lobjMail.From = new MailAddress(m_From);
                    }
                    if (m_To.Contains(";"))
                    {
                        ArrayList arrTo = new ArrayList();
                        arrTo.AddRange(m_To.Split(';'));
                        string to = string.Empty;
                        for (int i = 0; i < arrTo.Count; i++)
                        {
                            to = to + arrTo[i].ToString() + ",";
                        }
                        StringBuilder sbBody = new StringBuilder();
                        sbBody.Append(m_Body + "\n" + "Owner :" + txtRequestOwner.Text + "\n" + "Dont reply again to this e-Mail");
                        m_Body = string.Empty;
                        m_Body = sbBody.ToString();
                        to = to.Substring(0, to.Length - 1);
                        lobjMail.To.Add(to);
                    }
                    else
                    {
                        lobjMail.To.Add(m_To);
                    }
                    if (m_CC != string.Empty)
                    {
                        lobjMail.CC.Add(m_CC);
                    }
                    if (m_BCC != string.Empty)
                    {
                        lobjMail.Bcc.Add(m_BCC);
                    }
                    if (m_Body != string.Empty)
                    {
                        lobjMail.Body = m_Body;
                    }
                    if (m_Subject != string.Empty)
                    {
                        lobjMail.Subject = m_Subject;
                    }
                    lobjMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    /// Server details 
                    /// 
                    lobjMail.IsBodyHtml = _boolIsBodyHTML;
                    lobjSMTP.Host = ConfigurationManager.AppSettings["MailServer"].ToString();
                    lobjSMTP.Port = 25;
                    lobjSMTP.EnableSsl = this._boolEnableSsl;
                    lobjSMTP.Timeout.Equals(90);
                    lobjSMTP.Send(lobjMail);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }       

        protected void imgbtnAddClone_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddCloneRecords(updtPnlContent);
            if (imgbtnSubmit.AlternateText == "Add" || imgbtnSubmit.AlternateText == "Add & Continue" || imgbtnSubmit.AlternateText == "Modify")
            {
                GetXMLToSendMails(m_OutXML);
            }
        }

        protected void timerEntryForm_Tick(object sender, EventArgs e)
        {
            //Calling the Save method by passing the content page Update Panel as parameter
            commonObjUI.SaveEntries(updtPnlContent);
        }

        protected void imgbtnContinueAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Calling the AddMultipleRecords method by passing the content page Update Panel as parameter
            commonObjUI.AddMultipleRecords(updtPnlContent);
            if (imgbtnSubmit.AlternateText == "Add" || imgbtnSubmit.AlternateText == "Add & Continue" || imgbtnSubmit.AlternateText == "Modify")
            {
                GetXMLToSendMails(m_OutXML);
            }
        }
    }
}