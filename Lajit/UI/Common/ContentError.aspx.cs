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
using LAjit_BO;

namespace LAjitDev.Common
{
    public partial class ContentError : System.Web.UI.Page
    {
        //Redirecting to the page from where Error occures
        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("DashBoard.aspx");
            //Decrypt ErrorSourceFile and assign it to link
            //string ErrorSource = Request.QueryString["src"].ToString().Replace(' ', '+');
            //Response.Redirect(clsEncryptDecryptBO.DecryptData(ErrorSource.ToString()));
        }
    }
}
