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
using LAjit_BO;

namespace LAjitDev.Common
{
    public partial class FlashViewer : System.Web.UI.Page
    {
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();
        CommonBO objBO = new CommonBO();

        private static string m_StreamingServerPath = ConfigurationManager.AppSettings["StreamingServerPath"];
        private static string m_FlashVideosPath = ConfigurationManager.AppSettings["HelpFilesPath"];


        public string strFilePath = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            if (!Page.IsPostBack)
            {
                if (Request.Params["File"] != null)
                {
                    string strCurrentPath = m_FlashVideosPath + @"\StreamingVideo\" + Request.Params["File"].ToString();
                    if (VideoFileExist(strCurrentPath))
                    {
                        strFilePath = m_StreamingServerPath + GetFileCodec(strCurrentPath) + Request.Params["File"].ToString();
                    }
                    else
                    {
                        strFilePath = m_StreamingServerPath + "mp4:sample1_150kbps.f4v";
                    }
                }
            }
            if (ConfigurationManager.AppSettings["AutoRedirectUponSessionExpire"] == "1")//Client-side redirect.
            {
                commonObjUI.InjectSessionExpireScript(this);
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
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


        #region Private Methods
        /// <summary>
        /// Check video file exist or not.
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        private bool VideoFileExist(string strFilePath)
        {

            bool fileStatus;
            if (System.IO.File.Exists(strFilePath))
            {
                fileStatus = true;
            }
            else
            {
                fileStatus = false;
            }
            return fileStatus;
        }
        /// <summary>
        /// Get streamingfile codec
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        private string GetFileCodec(string strFilePath)
        {
            string strFileExtension = System.IO.Path.GetExtension(strFilePath);

            string strCodec = string.Empty;

            switch (strFileExtension)
            {
                case ".mp4":
                case ".MPEG-4":
                case ".m4v":
                case ".f4v":
                case ".3GPP":
                    strCodec = "mp4:";
                    break;
                default:
                    break;

            }
            return strCodec;
        }


        private void AddScriptReferences()
        {
            //CDN Added Scripts

            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "AC_RunActiveContent.js");
            Page.Header.Controls.Add(hgcScript1);
        }

        #endregion
    }
}
