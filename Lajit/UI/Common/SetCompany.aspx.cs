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
//using LAjitControls;

namespace LAjitDev.Common
{
    public partial class SetCompany : System.Web.UI.Page
    {
        XmlDocument XDocUserInfo = new XmlDocument();
        CommonUI commonObjUI = new CommonUI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitialiseRoles();
            }

        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            string masterfile = string.Empty;
            if (Request.QueryString["PopUp"] != null)
            {
                masterfile = "../MasterPages/PopUp.Master";
            }
            else
            {
                masterfile = "../MasterPages/TopLeft.Master";
            }
            if (!masterfile.Equals(string.Empty))
            {
                base.MasterPageFile = masterfile;
            }
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
        /// Entity Company Roles Counts Hide/Show Controls and Fill dropdownlist data
        /// </summary>
        private void InitialiseRoles()
        {
            //Get Entity Company Roles Count
            string strRole = EntityCompanyRoleCount();
            string[] strRoles = strRole.Split(';');
            int EntityCount = Convert.ToInt32(strRoles[0]);
            int CompanyCount = Convert.ToInt32(strRoles[1]);
            int RoleCount = Convert.ToInt32(strRoles[2]);

            //Entity,Company,Role Counts are 1 then hide dropdownlist and table cell

            if ((EntityCount == 1) && (CompanyCount == 1) && (RoleCount == 1))
            {
                //Hide Company Label Dropdownlist and Related TableRows

                lblCompany.Visible = false;
                ddlEntity.Visible = false;
                imgbtnSubmit.Visible = false;
            }
            else
            {
                //Show Company Label Dropdownlist and Related TableRows
                lblCompany.Visible = true;
                ddlEntity.Visible = true;
                imgbtnSubmit.Visible = true;
                imgbtnSubmit.ImageUrl = Application["ImagesCDNPath"].ToString() + "images/submit-but.png";
                
                //Fill Role DropDowns
                FillRoleDropDowns(EntityCount, CompanyCount, RoleCount);
            }
        }

        /// <summary>
        /// Fill Roles DropDownlist Data
        /// </summary>
        /// <param name="EntityCount"></param>
        /// <param name="CompanyCount"></param>
        /// <param name="RoleCount"></param>
        private void FillRoleDropDowns(int EntityCount, int CompanyCount, int RoleCount)
        {
            //XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
            //Test
            //2 2
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"2\" UserRoleID=\"16\" RoleID=\"7\" Description=\"Assistant To The Regional Director\" Caption=\"Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity><Entity EntityID=\"3\" Description=\"CAPS  Gemini\" Caption=\"Proto Gemini\" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"3\" Description=\"Valuelabs\" Caption=\"CAPS VL\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"3\" UserRoleID=\"1\" RoleID=\"6\" Description=\"VL SEE (Full Access)\" Caption=\"SSE Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"3\" UserRoleID=\"16\" RoleID=\"7\" Description=\"SE Director\" Caption=\"SE Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //1 1
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //XDocUserInfo.LoadXml(test);

            //To Store all entityids
            ArrayList alEntity = new ArrayList();
            //ArrayList alDescription = new ArrayList();


            //PreSelect DropDown with Current Loged Role
            string LogInCompanyID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/CompanyID").InnerText;
            string LogInUserRoleID = XDocUserInfo.SelectSingleNode("Root/bpe/companyinfo/UserRoleID").InnerText;

            // Get Entities
            XmlNode nodeEntities = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities");
            foreach (XmlNode nodeEntity in nodeEntities.ChildNodes)
            {
                if ((nodeEntity.Attributes["EntityID"] != null) && (nodeEntity.Attributes["Description"] != null))
                {
                    alEntity.Add(nodeEntity.Attributes["EntityID"].Value);
                    //alDescription.Add(nodeEntity.Attributes["Description"].Value);
                }
            }

            //Get Comapnies
            for (int i = 0; i < alEntity.Count; i++)
            {
                XmlNode nodeEntity = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities/Entity[@EntityID='" + alEntity[i].ToString() + "']");
                XmlNode nodeCompanies = nodeEntity.SelectSingleNode("Companies");
                /*if (ddlEntity.Items.Count == 0)
                {
                    ddlEntity.Items.Add(new ListItem("Choose", "-1~1"));
                    reqEntity.Enabled = true;
                    reqEntity.ErrorMessage = "Company";
                    reqEntity.InitialValue = ddlEntity.Items[0].Value;
                    imgbtnRoleSubmit.ValidationGroup = "LAJITChangeRole";
                }*/

                string Roles = string.Empty;
                int currentIndex = 0;
                foreach (XmlNode nodeCompany in nodeCompanies.ChildNodes)
                {

                    //Get Roles
                    XmlNode nodeRoles = nodeCompany.SelectSingleNode("Roles");
                    foreach (XmlNode nodeRole in nodeRoles.ChildNodes)
                    {
                        if (CompanyCount == 1)
                        {
                            ddlEntity.Items.Add(new ListItem(nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                        }
                        else if ((CompanyCount > 1) && (RoleCount == 1))
                        {
                            ddlEntity.Items.Add(new ListItem(nodeRole.Attributes["Description"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                        }
                        else
                        {
                            ddlEntity.Items.Add(new ListItem(nodeCompany.Attributes["Description"].Value + "-" + nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                            //ddlEntity.Items.Add(new ListItem(alDescription[i].ToString() + "-" + nodeCompany.Attributes["Description"].Value + "-" + nodeRole.Attributes["Description"].Value, nodeRole.Attributes["RoleCompanyID"].Value + "~" + nodeRole.Attributes["UserRoleID"].Value));
                        }

                        //Pre Select LogIn Role
                        if ((LogInCompanyID == nodeRole.Attributes["RoleCompanyID"].Value) && (LogInUserRoleID == nodeRole.Attributes["UserRoleID"].Value))
                        {
                            ddlEntity.SelectedValue = ddlEntity.Items[currentIndex].Value;
                        }
                        currentIndex++;

                    }
                }
            }
        }
        /// <summary>
        /// Genarate Change Role BPINFO 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="RoleID"></param>
        /// <param name="DefaultBPGID"></param>
        /// <returns></returns>
        private string GenarateRequest(string CompanyID, string RoleID, string DefaultBPGID)
        {

            XmlDocument xDocGV = new XmlDocument();
            // XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            // nodeRoot.AppendChild(nodeBPInfo);

            XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = DefaultBPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            XmlNode nodeCompany = xDocGV.CreateNode(XmlNodeType.Element, "CompanyDefault", null);
            nodeBPInfo.AppendChild(nodeCompany);

            XmlAttribute attCompany = xDocGV.CreateAttribute("CompanyID");
            attCompany.Value = CompanyID;
            nodeCompany.Attributes.Append(attCompany);

            XmlAttribute attRoleID = xDocGV.CreateAttribute("RoleID");
            attRoleID.Value = RoleID;
            nodeCompany.Attributes.Append(attRoleID);

            return nodeBPInfo.OuterXml;
            //  nodeBPInfo.InnerXml = "<CompanyDefault CompanyID=”1”  RoleID=”4” />";

        }

        /// <summary>
        /// Get Available Entity Company Role Counts
        /// </summary>
        /// <returns>string array</returns>
        private string EntityCompanyRoleCount()
        {
            int Entities = 0;
            int Companies = 0;
            int Roles = 0;

            string Result = string.Empty;

            //XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));

            //Test
            //2 2
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"2\" UserRoleID=\"16\" RoleID=\"7\" Description=\"Assistant To The Regional Director\" Caption=\"Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity><Entity EntityID=\"3\" Description=\"CAPS  Gemini\" Caption=\"Proto Gemini\" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"3\" Description=\"Valuelabs\" Caption=\"CAPS VL\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"3\" UserRoleID=\"1\" RoleID=\"6\" Description=\"VL SEE (Full Access)\" Caption=\"SSE Proto\" Seq=\"1\" ImgSrc=\" \" /><Role RoleCompanyID=\"3\" UserRoleID=\"16\" RoleID=\"7\" Description=\"SE Director\" Caption=\"SE Assistant\" Seq=\"2\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //1 1
            //string test = "<Root><bpe><userinfo><UserID>13</UserID><FullName>Alex</FullName><SmallName>Alex</SmallName><LanguageID>1</LanguageID><TimeZoneID>1</TimeZoneID><DayLightSaving>0</DayLightSaving><GMTAdjustment>0</GMTAdjustment><BrowserType>2</BrowserType><StandardBrowserType>1</StandardBrowserType><TypeOfUserID>1</TypeOfUserID><RoleLevel>1</RoleLevel><TenantID>1001</TenantID></userinfo></bpe><bpeout><FormInfo><BPGID>2</BPGID><FormID>14</FormID><PageInfo>Common/PreDashboard.aspx</PageInfo><Title>Pre-Dashboard</Title></FormInfo><MessageInfo><Status>Success</Status><Message><Status>Success</Status><Label>Process Complete.</Label></Message></MessageInfo><GlobalBusinessProcessControls><BusinessProcessControls><BusinessProcess ID=\"Workflow Processor\" BPGID=\"8\" /><BusinessProcess ID=\"Workflow Selection\" BPGID=\"7\" /><BusinessProcess FormID=\"1\" ID=\"Approvals\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"CompanyDefault\" BPGID=\"1012\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Dashboard\" BPGID=\"1\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Error\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Logoff\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"Messaging\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"1\" ID=\"ShortCuts\" BPGID=\"3\" Label=\"Dashboard\" PageInfo=\"Common/Dashboard.aspx\" /><BusinessProcess FormID=\"14\" ID=\"PreDashboard\" BPGID=\"2\" Label=\"Pre-Dashboard\" PageInfo=\"Common/PreDashboard.aspx\" /><BusinessProcess FormID=\"31\" ID=\"Help\" BPGID=\"43\" Label=\"Generic Help Form\" PageInfo=\"Common/Help.aspx\" ImgSrc=\"Help-icon.gif\" /><BusinessProcess FormID=\"32\" ID=\"Notes\" BPGID=\"48\" Label=\"Notes\" PageInfo=\"Common/Notes.aspx\" /><BusinessProcess FormID=\"33\" ID=\"Attachments\" BPGID=\"54\" Label=\"Attachments\" PageInfo=\"Common/Attachement.aspx\" /><BusinessProcess FormID=\"34\" ID=\"SecureItems\" BPGID=\"58\" Label=\"Secure\" PageInfo=\"Common/Secure.aspx\" /><BusinessProcess FormID=\"157\" ID=\"My Reports\" BPGID=\"10\" Label=\"My Reports\" PageInfo=\"Reports/MyReports.aspx\" /></BusinessProcessControls><UserAccess UserID=\"13\" TenantID=\"1001\" LogonName=\"AA\" FullName=\"Alex\" SmallName=\"Alex\" RoleLevel=\"1\"><Entities><Entity EntityID=\"2\" Description=\"CAPS Proto-Entity\" Caption=\"Proto Entity        \" Seq=\"1\" ImgSrc=\" \"><Companies><Company CompanyID=\"2\" Description=\"CAPS Proto\" Caption=\"CAPS Proto\" Seq=\"1\" ImgSrc=\" \"><Roles><Role RoleCompanyID=\"2\" UserRoleID=\"9\" RoleID=\"6\" Description=\"Accountant Prototype (Full Access)\" Caption=\"Accountant Proto\" Seq=\"1\" ImgSrc=\" \" /></Roles></Company></Companies></Entity></Entities></UserAccess></GlobalBusinessProcessControls></bpeout></Root>";
            //XDocUserInfo.LoadXml(test);


            XmlNode nodeEntities = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/UserAccess/Entities");

            foreach (XmlNode nodeEntity in nodeEntities.ChildNodes)
            {
                Entities = Entities + 1;

                XmlNode nodeCompanies = nodeEntity.SelectSingleNode("Companies");

                foreach (XmlNode nodeCompany in nodeCompanies.ChildNodes)
                {
                    Companies = Companies + 1;

                    XmlNode nodeRoles = nodeCompany.SelectSingleNode("Roles");

                    foreach (XmlNode nodeRole in nodeRoles.ChildNodes)
                    {
                        Roles = Roles + 1;
                    }
                }
            }

            Result = Entities.ToString() + ";" + Companies.ToString() + ";" + Roles.ToString();

            return Result;
        }

        /// <summary>
        /// Navigating next page based on selected role
        /// </summary>
        /// <param name="SelectedRole"></param>
        private void RoleNavigation(string SelectedRole)
        {
                XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                string BPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Dashboard']").Attributes["BPGID"].Value;
                //string PageInfo = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='Dashboard']").Attributes["PageInfo"].Value;
                string[] strarr = SelectedRole.Split('~');  //ddlCompanyRole.SelectedValue.Split('~');
                string RoleCompanyID = strarr[0].ToString();
                string UserRoleID = strarr[1].ToString();
                string navigatePage = string.Empty;
            
                //Submit Company Default DB
                CommonBO objBO = new CommonBO();

                //<BusinessProcess FormID="1" ID="CompanyDefault" BPGID="1012" Label="Dashboard" PageInfo="Common/Dashboard.aspx" /> 
                string DefaultBPGID = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='CompanyDefault']").Attributes["BPGID"].Value;
                string DefaultPageInfo = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[@ID='CompanyDefault']").Attributes["PageInfo"].Value;

                string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

                string BPINFO = GenarateRequest(RoleCompanyID, UserRoleID, DefaultBPGID);

                string strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPINFO, "", "", "", strBPE, false, "1", "1", null);

                //BPOUT from DB
                string strOutXml = objBO.GetDataForCPGV1(strReqXml);

                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(strOutXml);

                XmlNode nodeMsg = xdoc.SelectSingleNode("MessageInfo/Message");
                //xdoc.SelectSingleNode("Root/bpeout/MessageInfo/Message");

                XmlNode nodeMsgStatus = xdoc.SelectSingleNode("MessageInfo/Status");
                //xdoc.SelectSingleNode("Root/bpeout/MessageInfo/Status");
                if (nodeMsgStatus.InnerText.ToUpper() == "SUCCESS")
                {
                    //SUCCESS
                    navigatePage = "../" + DefaultPageInfo + "?rcId=" + RoleCompanyID + "&ruId=" + UserRoleID + "&bpgId=" + BPGID;

                    lblmsg.Visible = true;
                    lblmsg.Text = nodeMsg.SelectSingleNode("Label").InnerText;
                 
                    //Response.Redirect(navigatePage);
                }
                else
                {
                    //ERROR
                    lblmsg.Visible = true;
                    lblmsg.Text = nodeMsg.SelectSingleNode("Label").InnerText;
                }
        }
       
        #endregion


        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            RoleNavigation(ddlEntity.SelectedValue);
        }


        //protected void imgbtnCancel_Click(object sender, ImageClickEventArgs e)
        //{
        //    //Calling the Cancel method by passing the content page Update Panel as parameter
        //    if (Page.MasterPageFile.Contains("PopUp.Master"))
        //    {
        //        commonObjUI.CancelPagePopUpEntries(updtPnlContent);
        //    }
        //    else
        //    {
        //        commonObjUI.CancelEntries(updtPnlContent);
        //    }
        //}

        //protected void timerEntryForm_Tick(object sender, EventArgs e)
        //{
        //    //Calling the Save method by passing the content page Update Panel as parameter
           
        //}

    }
}
