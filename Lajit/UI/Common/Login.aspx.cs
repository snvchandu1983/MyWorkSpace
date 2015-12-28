using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using LAjit_BO;
using NLog;

namespace LAjitDev
{
    public partial class Login : System.Web.UI.Page
    {
        NLog.Logger logger = LogManager.GetCurrentClassLogger();
        string nlog = string.Empty;
        CommonBO objUsr_BO = new CommonBO();
        string m_strTheme = "LAjit";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Register ajax methods
            Ajax.Utility.RegisterTypeForAjax(typeof(Classes.AjaxMethods));
           
            //Add CSS References
            AddCSSReferences();
            
            if (!IsPostBack)
            {
                //Check browser supports cookies
                CheckBrowserCookies();

                //Add Script to read cookie
                string JScript = "javascript:return checkCookie();";
                login_but.Attributes.Add("OnClientClick", "if(Page_ClientValidate()) {" + JScript + "} else {return false;}");

                //Set ImageUrl
                login_but.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/login-but.gif";
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            //  Session.Add("MyTheme", "LAjit");

            //base.OnPreInit(e);
            //if (HttpContext.Current.Session["MyTheme"] == null)
            //{

            // HttpContext.Current.Session.Add("MyTheme", "LAjit");

             //HttpContext.Current.Session.Add("MyTheme", Application["ImagesCDNPath"].ToString() + "LAjit");
            
            //}
            //Page.Theme = ((string)HttpContext.Current.Session["MyTheme"]);
            Page.Theme = m_strTheme;

            //Update CDN Path
            string  strImagesCDNPath = ConfigurationManager.AppSettings["ImagesCDNPath"].ToString()+ m_strTheme +"/";
            Application["ImagesCDNPath"] = strImagesCDNPath;
        }

        protected void login_but_Click(object sender, ImageClickEventArgs e)
        {
           
            

            Session["LoggedInUserName"] = txtUserName.Text;
            //Session.Add("MyTheme", "Sony");
            //Server.Transfer(Request.FilePath);
            AuthenticateCredentails(GenerateLoginXML());

           // Session["ClientName"] = "BOB";

            #region NLog
            logger.Debug("The User is logging in with username as : " + txtUserName.Text);
            #endregion

            
        }


        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink css = new HtmlLink();
            css.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            css.Attributes["rel"] = "stylesheet";
            css.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(css);


            //shortcut icon favicon.ico
            HtmlLink hlShortCutIcon = new HtmlLink();
            hlShortCutIcon.Href = Application["ImagesCDNPath"].ToString() + "Images/favicon.ico";
            hlShortCutIcon.Attributes["rel"] = "shortcut icon";
            hlShortCutIcon.Attributes["type"] = "image/x-icon";
            Page.Header.Controls.Add(hlShortCutIcon);
        }


        /// <summary>
        /// check browser cookies availbility
        /// </summary>
        private void CheckBrowserCookies()
        {
            //Check cookie available or not
            if (Request.Cookies["LACookie"] == null)
            { 
                //Write Cookie
                Response.Cookies["LACookie"].Value = "TestCookie";
            }
        }


        /// <summary>
        /// Authenticates the inputXML string.
        /// </summary>
        /// <param name="inputXML">The XML string consisting of the User Credentials.</param>
        /// <returns>The redirect page if the validation successful else null.</returns>
        private string AuthenticateCredentails(string inputXML)
        {
            string returnXML = objUsr_BO.GetLoginInfo(inputXML);
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(returnXML);
            XmlNode nodeErrorInfo = xDocReturn.SelectSingleNode("Root/bpeout/MessageInfo/Message/Status");
            if (nodeErrorInfo.InnerText != "Error")
            {
                string userID = xDocReturn.SelectSingleNode("Root/bpe/userinfo/UserID").InnerText;

                //Creating Folders based on the UserId to store the BPOUT XMLs
                //Session["USERINFOXML"] = ConfigurationManager.AppSettings["XMLStoragePath"] +"/"+ xDocReturn.SelectSingleNode("Root/bpe/userinfo/TenantID").InnerText + xDocReturn.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText + xDocReturn.SelectSingleNode("Root/bpe/companyinfo/RoleID").InnerText + Session["USERID"] + "UserInfo.xml";
                if (!string.IsNullOrEmpty(userID))
                {
                    if (!Directory.Exists(ConfigurationManager.AppSettings["XMLStoragePath"]))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["XMLStoragePath"]);
                    }
                    if (!Directory.Exists(ConfigurationManager.AppSettings["XMLStoragePath"] + "/" + userID))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["XMLStoragePath"] + "/" + userID);
                    }

                    //Save the XML File
                    //if (xDocReturn.SelectSingleNode("Root/bpe/companyinfo/CompanyID") != null)
                    //{
                    //    Session["USERINFOXML"] = ConfigurationManager.AppSettings["XMLStoragePath"] + "/" +Session.SessionID+ userID + "/" + xDocReturn.SelectSingleNode("Root/bpe/userinfo/TenantID").InnerText + xDocReturn.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText + xDocReturn.SelectSingleNode("Root/bpe/companyinfo/RoleID").InnerText + "UserInfo.xml";
                    //    xDocReturn.Save(Session["USERINFOXML"].ToString());
                    //}
                    //else
                    //{
                    //    Session["USERINFOXML"] = ConfigurationManager.AppSettings["XMLStoragePath"] + "/" +Session.SessionID+ "UserInfo.xml";
                    //    xDocReturn.Save(Session["USERINFOXML"].ToString());
                    //}

                    Session["USERINFOXML"] = ConfigurationManager.AppSettings["XMLStoragePath"] + "/" + userID + "/" + Session.SessionID + "UserInfo.xml";
                    //Session["USERINFOXML"] = ConfigurationManager.AppSettings["XMLStoragePath"] +"/"+ Session["CompanyEntityID"].ToString() + "/" + userID + "/" + Session.SessionID + "UserInfo.xml";
                    xDocReturn.Save(Session["USERINFOXML"].ToString());
                }

                //Set Theme from return xml
                if (Session["MyTheme"] == null)
                {
                    string keyTheme = "430";
                    if (xDocReturn.SelectSingleNode("Root/bpe/companyinfo") != null)
                    {
                        string theme = xDocReturn.SelectSingleNode("Root/bpe/companyinfo/userpreference/Preference[@TypeOfPreferenceID='" + keyTheme + "']").Attributes["Value"].Value;
                        Session.Add("MyTheme", theme);
                    }
                    else
                    {
                        Session.Add("MyTheme", "LAjit");
                    }
                }

           
                  if (xDocReturn.SelectSingleNode("Root/bpe/companyinfo") != null)
                  {
                        string strcompanyName = xDocReturn.SelectSingleNode("Root/bpe/companyinfo/CompanyName").InnerText;
                        Session["CompanyName"]=strcompanyName;
                  }


                //Using Sessions
                //Session["BPE"] = xDocReturn.SelectSingleNode("Root/bpe").OuterXml;
                //Session["GBPC"] = xDocReturn.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml;
                //Session["FORMINFO"] = xDocReturn.SelectSingleNode("Root/bpeout/FormInfo").OuterXml;
                string redirectpage = xDocReturn.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText;
                //redirectpage = "../Finance/COA.aspx";
                redirectpage = "../" + redirectpage;

                Response.Redirect(redirectpage);

                //Using Hidden Variables
                //hdnvarCols.Value = storedCols.ToString();
                //RegisterStartupScript("PageBasis", "<script>Redirect('"+redirectpage+"');</script>");                 
            }
            else
            {
                string errorMessage = string.Empty;
                XmlNode nodeError = xDocReturn.SelectSingleNode("Root/bpeout/MessageInfo/Message");
                foreach (XmlNode nodeErrMess in nodeError.ChildNodes)
                {
                    if (nodeErrMess.Name == "Label")
                    {
                        errorMessage += nodeErrMess.InnerText;
                        errorMessage += "\n";
                    }
                }
                lblError.Text = errorMessage;
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates an XML string consisting of the login credentials
        /// </summary>
        /// <returns>The generated XMl string.</returns>
        private string GenerateLoginXML()
        {
            XmlDocument xmlLogin = new XmlDocument();
            XmlNode nodeRoot = xmlLogin.CreateNode(XmlNodeType.Element, "Root", null);
            XmlNode nodeBPE = xmlLogin.CreateNode(XmlNodeType.Element, "bpe", null);

            //Adding the User ID
            XmlNode nodeCurrent = xmlLogin.CreateNode(XmlNodeType.Element, "username", null);
            nodeCurrent.InnerText = txtUserName.Text;
            nodeBPE.AppendChild(nodeCurrent);

            //Adding the Password
            nodeCurrent = xmlLogin.CreateNode(XmlNodeType.Element, "password", null);
            nodeCurrent.InnerText = txtPassword.Text;
            nodeBPE.AppendChild(nodeCurrent);

            //Adding the Browser Type and version
            nodeCurrent = xmlLogin.CreateNode(XmlNodeType.Element, "browser", null);
            XmlNode nodeBrwsr = xmlLogin.CreateNode(XmlNodeType.Element, "type", null);
            nodeBrwsr.InnerText = Request.Browser.Browser + Request.Browser.MajorVersion.ToString();
            nodeCurrent.AppendChild(nodeBrwsr);//Adding the browser type.
            //nodeBrwsr = xmlLogin.CreateNode(XmlNodeType.Element, "version", null);
            //nodeBrwsr.InnerText = Request.Browser.Version;
            //nodeCurrent.AppendChild(nodeBrwsr);//Adding the browser version.
            nodeBPE.AppendChild(nodeCurrent);

            nodeRoot.AppendChild(nodeBPE);
            return nodeRoot.OuterXml;
        }
    }
}
