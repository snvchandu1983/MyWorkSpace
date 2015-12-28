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
using Dundas.Charting.WebControl;
using LAjit_BO;
using WebChart;
//using LAjitControls;

namespace LAjitDev.Common
{
    public partial class ChartView : Classes.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //GVUC.Visible = false;
                //Regular page
                ((UserControls.ChartUserControl)CUC).GVDataXml = ((UserControls.ButtonsUserControl)BtnsUC).GVDataXml;
            }
        }
        //protected void Page_PreRender(object sender, EventArgs e)
        //{

        //    int width = 50;
        //    if (htcCPGV1Auto.Width != string.Empty)
        //    {
        //        width = width + Int32.Parse(htcCPGV1Auto.Width);
        //    }
        //    htcCPGV1Auto.Width = width.ToString();
           
        //}
    }
}
