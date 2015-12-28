using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using LAjitControls;
using LAjit_BO;

namespace LAjitDev.Classes
{
    public class BasePagePopUp : System.Web.UI.Page
    {
        public CommonBO objBO = new CommonBO();
        public CommonUI commonObjUI = new CommonUI();
        XmlDocument XDocUserInfo = new XmlDocument();

        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            XmlDocument XDocUserInfo = new XmlDocument();
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            if (XDocUserInfo.SelectSingleNode("Root/bpe") != null)
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
    }
}
