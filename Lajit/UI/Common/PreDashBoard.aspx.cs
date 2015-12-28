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
using System.Xml;

namespace LAjitDev
{
    public partial class PreDashBoard : System.Web.UI.Page
    {
        XmlDocument xDoc = new XmlDocument();
        string comnEntyStr = string.Empty;
        string comnCmpnyStr = string.Empty;
        string entyID = string.Empty;
        string CmpnyID = string.Empty;
        string refer = string.Empty;
        string imageSrc = string.Empty;
        CommonUI commonObjUI = new CommonUI();
        XmlDocument XDocUserInfo = new XmlDocument();
        string m_strTheme = "LAjit";

        protected void Page_Load(object sender, EventArgs e)
        {

            AddCSSReferences();

            //string num_tbl_rows = Context.Request["hdnvarRows"].ToString();
            if (!IsPostBack)
            {
                DesignPage();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            Page.Theme = m_strTheme;

            //Update CDN Path
            string strImagesCDNPath = ConfigurationManager.AppSettings["ImagesCDNPath"].ToString() + m_strTheme + "/";
            Application["ImagesCDNPath"] = strImagesCDNPath;
        }

        private void AddCSSReferences()
        {
            //LajitCDN.css
            HtmlLink hlCDNCss = new HtmlLink();
            hlCDNCss.Href = Application["ImagesCDNPath"].ToString() + "LajitCDN.css";
            hlCDNCss.Attributes["rel"] = "stylesheet";
            hlCDNCss.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(hlCDNCss);
        }

        public void DesignPage()
        {
            XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

           // xDoc.LoadXml(Session["GBPC"].ToString());

            XmlNode nodeEntities = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities");
            
            //EntityHandling
            foreach (XmlNode nodeEntityElement in nodeEntities.ChildNodes)
            {
              refer = "EntyID";
              entyID = nodeEntityElement.Attributes["EntityID"].Value.ToString();
              string entyHoverVal  = entyID + "~" + nodeEntityElement.SelectSingleNode("Companies").ChildNodes.Count;
              string entyCaption = nodeEntityElement.Attributes["Caption"].Value.ToString();
              string[] arr = new string[5];
              arr[1] = "png";

              if (nodeEntityElement.Attributes["ImgSrc"].Value.ToString() != " ")
              {
                  arr = nodeEntityElement.Attributes["ImgSrc"].Value.Split('.');
                  imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + nodeEntityElement.Attributes["ImgSrc"].Value;
              }
              else
              {
                  imageSrc = Application["ImagesCDNPath"].ToString() + "Images/entity3" + "." + arr[1];
              }
              if (comnEntyStr == "")
              {
                  comnEntyStr = entyID + "~" + nodeEntityElement.SelectSingleNode("Companies").ChildNodes.Count;
              }
              else
              {
                  comnEntyStr = comnEntyStr + ":" + entyID + "~" + nodeEntityElement.SelectSingleNode("Companies").ChildNodes.Count;
              }
               TableRow entyTblRow = new TableRow();
               //entyTblRow.BorderWidth = 1;
               entyTbl.Rows.Add(entyTblRow);
               for (int i = 0; i < 2; i++)
               {
                  TableCell entyTblCell = new TableCell();
                  entyTblRow.Cells.Add(entyTblCell);
                  //entyTblCell.Width = 100;
                  //entyTblCell.BorderWidth = 1;
                  entyTblCell.Text = "<table><tr><td><img src=" + imageSrc + " " + " " + "style = 'cursor:pointer'"
                                 + " " + "onmouseover=\"ToggleImage(this,'." + arr[1] + "');Visible('" + entyHoverVal + "','"
                                 + refer + "'); \" " + "onmouseout=\"ToggleImage(this, '." + arr[1] + "');\" " + "/>"
                                 + "</td></tr>" + "<tr><td class='text' align='center' style='vertical-align:top'>"
                                 + entyCaption + "</td></tr>" + "</table>";
                  if (i == 1)
                  {
                      entyTblCell.Text = "<div id =" + entyID + ">" + "<img src='" + Application["ImagesCDNPath"].ToString() + "Images/dash-arrow." + arr[1] + "'"
                                     + " onmouseover=\"Visible('" + entyHoverVal + "','" + refer + "');\" " + "/>";
                  }
                  else
                  { 

                  }

               }

                #region companies
                int cnt = 0;
                XmlNode nodeCmpnyElement = nodeEntityElement.SelectSingleNode("Companies");
                //CompanyHandlingStarts
                foreach (XmlNode nodeSubCmpnyElement in nodeCmpnyElement.ChildNodes)
                {
                    refer = "CmpnyID";
                    CmpnyID = nodeSubCmpnyElement.Attributes["CompanyID"].Value;
                    string cmpny_HoverVal = CmpnyID + "~" + nodeSubCmpnyElement.SelectSingleNode("Roles").ChildNodes.Count;
                    string strCmpnyID = "CmpnyID" + CmpnyID;
                    string entyReferId = "EntyID" + entyID + cnt;
                    string cmpnyCaption = nodeSubCmpnyElement.Attributes["Caption"].Value;
                    arr[1] = "png";

                    if (nodeSubCmpnyElement.Attributes["ImgSrc"].Value != " ")
                    {
                        arr = nodeSubCmpnyElement.Attributes["ImgSrc"].Value.Split('.');
                        imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + nodeSubCmpnyElement.Attributes["ImgSrc"].Value;
                    }
                    else
                    {
                        imageSrc = Application["ImagesCDNPath"].ToString() + "Images/company1.png";
                    }
                    if (comnCmpnyStr == "")
                    {
                        comnCmpnyStr = CmpnyID + "~" + nodeSubCmpnyElement.SelectSingleNode("Roles").ChildNodes.Count;
                    }
                    else
                    {
                        comnCmpnyStr = comnCmpnyStr + ":" + CmpnyID + "~" + nodeSubCmpnyElement.SelectSingleNode("Roles").ChildNodes.Count;
                    }
                    TableRow cmpnyTblRow = new TableRow();
                    //cmpnyTblRow.BorderWidth = 1;
                    cmpnyTbl.Rows.Add(cmpnyTblRow);

                    TableCell cmpnyTblCell = new TableCell();
                    cmpnyTblRow.Cells.Add(cmpnyTblCell);
                    cmpnyTblCell.Text = "<div id =" + entyReferId + ">" + "<table width='100'><tr><td style='height: 25px'><img src="
                                         + imageSrc + " " + " " + "style = 'cursor:pointer'" + " " + "onmouseover=\"ToggleImage(this,'."+arr[1]+"'); Visible('" + cmpny_HoverVal + "','" + refer + "');\" "
                                         + "onmouseout=\"ToggleImage(this,'."+arr[1]+"');\"  />" + "</td><tr>" + "<tr><td class='text' align='center' style='vertical-align:top'>" + cmpnyCaption
                                         + "</td></tr>" + "</table>" + "</div>";

                    #region for roles
                    XmlNode noderole = nodeSubCmpnyElement.SelectSingleNode("Roles");
                    //RolesHandlingEnds
                    foreach (XmlNode nodeRoleElement in nodeSubCmpnyElement.ChildNodes)
                    {
                        XmlNode nodeRoles = nodeSubCmpnyElement.SelectSingleNode("Roles");
                        arr[1] = "png";

                        TableRow rlTblRow = new TableRow();
                        //rlTblRow.BorderWidth = 1;
                        rlTbl.Rows.Add(rlTblRow);
                        int rcnt = 0;
                        for (int n = 0; n < nodeRoles.ChildNodes.Count; n++)
                        {
                            string cmpnyReferID = "CmpnyID" + CmpnyID + rcnt;
                            string rlCaption = nodeRoles.ChildNodes[n].Attributes["Caption"].Value.ToString();
                            string rlCmpnyID = nodeRoles.ChildNodes[n].Attributes["RoleCompanyID"].Value.ToString();
                            //string strRoleID = "'RoleID" + rlCmpnyID + "'";
                            //string rlUserID = nodeRoles.ChildNodes[n].Attributes["UserRoleID"].Value.ToString();
                            string rlUserID = nodeRoles.ChildNodes[n].Attributes["RoleID"].Value.ToString();
                            string BPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Dashboard']").Attributes["BPGID"].Value;

                            if (nodeRoles.ChildNodes[n].Attributes["ImgSrc"].Value.ToString() != " ")
                            {
                                arr = nodeRoles.ChildNodes[n].Attributes["ImgSrc"].Value.Split('.');
                                imageSrc = Application["ImagesCDNPath"].ToString() + "Images/" + nodeRoles.ChildNodes[n].Attributes["ImgSrc"].Value;
                            }
                            else
                            {
                                imageSrc = Application["ImagesCDNPath"].ToString() + "Images/users-but.png";
                            }
                            TableCell rlTblCell = new TableCell();
                            //rlTblCell.BorderWidth = 1;
                            rlTblRow.Cells.Add(rlTblCell);
                            
                            if (rcnt == 0)
                            {   
                                rlTblCell.Text = "<div id =" + cmpnyReferID + ">" + "<table cellpadding='0' cellspacing='0' width='100px' border='0'>"
                                                   + "<tr>" + "<td ><img src='" + Application["ImagesCDNPath"].ToString() + "Images/dash-arrow." + arr[1] + "'" + " onmouseover=\"Visible('" + cmpnyReferID + "','" + refer + "');\" "
                                                   + "/>" + "</td>" + "<td style='height: 50px;width=190px' align='center'><img src=" + imageSrc + " " + "style='cursor:pointer'"
                                                   + "onmouseover=\"ToggleImage(this,'."+arr[1]+"');\" " + "onmouseout=\"ToggleImage(this,'."+arr[1]+"');\" " + "onclick=\"RedirectPage('" + rlCmpnyID + "','" + rlUserID + "','" + BPGID + "');\" " + "/>"
                                                   + "</td></tr>" + "<tr><td width='170px'></td>" + "<td class='text' style='vertical-align:top'>" + rlCaption + "</td></tr>" + "</table>" + "</div>";
                            }
                            else
                            {
                                rlTblCell.Text = "<div id =" + cmpnyReferID + ">" + "<table cellpadding='0' cellspacing='0' border='0' width='100px'><tr><td align='center'><img src="
                                                    + imageSrc + " " + "style='cursor:pointer'" + "onmouseover=\"ToggleImage(this,'."+arr[1]+"');Visible('" + cmpnyReferID + "','" + refer + "');\" "
                                                    + "onmouseout=\"ToggleImage(this,'."+arr[1]+"');\" " + "onclick=\"RedirectPage('" + rlCmpnyID + "','" + rlUserID + "','" + BPGID + "');\" " + "/>"
                                                    + "</td></tr>" + "<tr><td class='text' style='vertical-align:top;width=170px' align='center'>" + rlCaption + "</td></tr>" + "</table></div>";
                            }
                            rcnt++;
                        }
                    }
                    #endregion
                    cnt++;
                }
                #endregion
            }
            hiddenEntyStrID.Value = comnEntyStr;
            hiddenCmpnyStrID.Value = comnCmpnyStr;
        }
    }
}