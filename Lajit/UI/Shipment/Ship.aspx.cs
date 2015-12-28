#region Namespaces
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
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Net;
using System.IO;
using System.Drawing;
using LAjit_BO;
#endregion

namespace LAjitDev.Shipment
{
    public partial class Ship : Classes.BasePagePopUp
    {
        #region Private Variables
        LAjitDev.Shipment.FedRateService_Local objFedRateserviceLocal = new LAjitDev.Shipment.FedRateService_Local();
        LAjitDev.Shipment.FedShipService_Local objFedShipServiceLocal = new LAjitDev.Shipment.FedShipService_Local();
        LAjitDev.Shipment.FedTrackService_Local objFedTrackServiceLocal = new LAjitDev.Shipment.FedTrackService_Local();

        DataSet dsAll = new DataSet();
        #endregion

        #region Page PreInit
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Page.Request.Params["PopUp"] != null)
            {
                if ((Page.Request.Params["PopUp"] == "PopUp"))
                {
                    Page.MasterPageFile = "../MasterPages/PopUp.Master";
                    //pnlTotalTable.Width = Unit.Pixel(900);
                }
            }
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AddScriptReferences();
            PreloadImages();

            Ajax.Utility.RegisterTypeForAjax(typeof(CommonUI));
            Ajax.Utility.RegisterTypeForAjax(typeof(Classes.AjaxMethods));

            //int entryFormWidth = 920;
            ////If Left panel is collapsed the width increases by an amount equal to that of Left Panel
            //if (Convert.ToString(Session["LPCollapsed"]) == "1")
            //{
            //    entryFormWidth += 149;
            //}
            //pnlTotalTable.Width = Unit.Pixel(entryFormWidth);


            //if (Page.MasterPageFile.Contains("../MasterPages/PopUp.Master"))
            //{
            //    Panel pnlPopUp = (Panel)Page.Master.FindControl("pnlPopUp");
            //    if (pnlPopUp != null)
            //    {
            //        pnlPopUp.Width = Unit.Percentage(100);
            //    }
            //}
        }
        #endregion

        private void AddScriptReferences()
        {
            //CDN Added Scripts

            //Common.js
            HtmlGenericControl hgcScript1 = new HtmlGenericControl();
            hgcScript1.TagName = "script";
            hgcScript1.Attributes.Add("type", "text/javascript");
            hgcScript1.Attributes.Add("language", "javascript");
            hgcScript1.Attributes.Add("src", Application["ScriptsCDNPath"].ToString() + "Common.js");
            Page.Controls.Add(hgcScript1);
        }


        private void PreloadImages()
        { 
       
            btnRates.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/rates-but.png";
 
            btnRatesboth.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/rates-but.png";

            btnShipboth.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/ship-but.png";

            btnShip.ImageUrl = Application["ImagesCDNPath"].ToString() + "Images/ship-but.png";

            btnTrack.ImageUrl= Application["ImagesCDNPath"].ToString() + "Images/ship-but.png";

        }


        #region All Buttons Click Events
        protected void All_Click(object sender, ImageClickEventArgs e)
        {
            #region Variables
            string result = string.Empty;
            gvFEDEX.Visible = false;
            gvDHL.Visible = false;
            gvUPS.Visible = false;
            #endregion
            #region Get Rates
            if (hid_ID_Values.Value.ToString() == "Get Rates")
            {
                #region FEDEX
                if (radFedex.Checked == true)
                {
                    result = CreateFedexXMLSchema("Rates");
                    if (result == "Grid")
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + gvFEDEX.ClientID + "','Grid')</script>");
                    }
                    else
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                    }
                }
                #endregion
                #region UPS
                //else if (radUPS.Checked == true)
                //{
                //    result = CreateUPSXMLSchema("Rates");
                //    if (result == "Grid")
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + gvUPS.ClientID + "','Grid')</script>");
                //    }
                //    else
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                //    }

                //}
                #endregion
                #region DHL
                else
                {
                    if (radDHL.Checked == true)
                    {
                        result = CreateDHLXmlSchema("Rates");
                        if (result == "Grid")
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + gvDHL.ClientID + "','Grid')</script>");
                        }
                        else
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td11','Get Rates','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                        }
                    }
                }
                #endregion
                #region Expand Collapse
                /*cpeCredentialDetails.ClientState = "True";
                cpeShipFrom.ClientState = "True";
                cpeShipTo.ClientState = "True";
                cpePackageDetails.ClientState = "True";

                cpeCredentialDetails.Collapsed = true;
                cpeShipFrom.Collapsed = true;
                cpeShipTo.Collapsed = true;
                cpePackageDetails.Collapsed = true;*/
                #endregion
            }
            #endregion
            #region Create Shipment
            else if (hid_ID_Values.Value.ToString() == "Create Shipment")
            {
                #region FEDEX
                if (radFedex.Checked == true)
                {
                    result = CreateFedexXMLSchema("Ship");
                    if (result == "Grid")
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + gvFEDEX.ClientID + "','Grid')</script>");
                    }
                    else
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                    }
                }
                #endregion
                //#region UPS
                //else if (radUPS.Checked == true)
                //{
                //    result = CreateUPSXMLSchema("Ship");
                //    if (result == "Grid")
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + gvUPS.ClientID + "','Grid')</script>");
                //    }
                //    else
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                //    }
                //}
                //#endregion
                #region DHL
                else
                {
                    if (radDHL.Checked == true)
                    {
                        result = CreateDHLXmlSchema("Ship");
                        if (result == "Grid")
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + gvDHL.ClientID + "','Grid')</script>");
                        }
                        else
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td16','Create Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                        }
                    }
                }
                #endregion
                #region Expand Collapse
                /*cpeShipperDetials.ClientState = "True";
                cpeCredentialDetails.ClientState = "True";
                cpeShipFrom.ClientState = "True";
                cpeShipTo.ClientState = "True";
                cpePackageDetails.ClientState = "True";

                cpeShipperDetials.Collapsed = true;
                cpeCredentialDetails.Collapsed = true;
                cpeShipFrom.Collapsed = true;
                cpeShipTo.Collapsed = true;
                cpePackageDetails.Collapsed = true;*/
                #endregion
            }
            #endregion
            #region Track Shipment
            else
            {
                #region FEDEX
                if (radFedex.Checked == true)
                {
                    result = CreateFedexXMLSchema("Track");
                    if (result == "Grid")
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + gvFEDEX.ClientID + "','Grid')</script>");
                    }
                    else
                    {
                        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                    }
                }
                #endregion
                //#region UPS
                //else if (radUPS.Checked == true)
                //{
                //    result = CreateUPSXMLSchema("Track");
                //    if (result == "Grid")
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + gvUPS.ClientID + "','Grid')</script>");
                //    }
                //    else
                //    {
                //        Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                //    }
                //}
                //#endregion
                #region DHL
                else
                {
                    if (radDHL.Checked == true)
                    {
                        result = CreateDHLXmlSchema("Track");
                        if (result == "Grid")
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + gvDHL.ClientID + "','Grid')</script>");
                        }
                        else
                        {
                            Page.RegisterStartupScript("ScriptKey", "<script>Get_All_Values('ctl00_cphPageContents_Td13','Track Shipment','Tabs','" + lblExceptions.ClientID + "','Label')</script>");
                        }
                    }
                }
            }
                #endregion
            #endregion
            #region Expand Collapse
            /*cpeCredentialDetails.ClientState = "True";
            cpeShipFrom.ClientState = "True";
            cpeShipTo.ClientState = "True";
            cpePackageDetails.ClientState = "True";

            cpeCredentialDetails.Collapsed = true;
            cpeShipFrom.Collapsed = true;
            cpeShipTo.Collapsed = true;
            cpePackageDetails.Collapsed = true;*/
            #endregion
        }
        #endregion

        #region FEDEX XML Schema
        //Sending All Inputs To FEDEX WSDL and getting all the results for the inputs
        public string CreateFedexXMLSchema(string types)
        {
            string result = string.Empty;
            if (types == "Rates" || types == "Ship")
            {
                string[] inputarray = new string[28];
                inputarray[0] = txtKeys.Value.ToString();
                inputarray[1] = txtPasswords.Value.ToString();
                inputarray[2] = txtAccountNumbers.Value.ToString();
                inputarray[3] = txtMeterNumbers.Value.ToString();

                inputarray[4] = txtStreetFrom.Text.ToString();
                inputarray[5] = txtCityAll.Text.ToString();
                inputarray[6] = ddlStateAll.Value.ToString();
                inputarray[7] = ddlCountryAll.Value.ToString();
                inputarray[8] = txtPostalCodeAll.Text.ToString();

                inputarray[9] = txtStreetReceiver.Text.ToString();
                inputarray[10] = txtReceiverCity.Text.ToString();
                inputarray[11] = ddlReceiverState.Value.ToString();
                inputarray[12] = ddlReceiverCountry.Value.ToString();
                inputarray[13] = txtReceiverPostalCode.Text.ToString();
                inputarray[14] = txtWeight.Text.ToString();
                if (ddlUnits.Value.ToString() == "KGS")
                {
                    inputarray[15] = "KG";
                }
                else
                {
                    inputarray[15] = "LB";
                }
                inputarray[16] = txtFedexPackageCount.Text.ToString();
                if (radYes.Checked == true)
                {
                    inputarray[17] = "YES";
                }
                else
                {
                    inputarray[17] = "NO";
                }
                inputarray[18] = txtLength.Text.ToString();
                inputarray[19] = txtWidth.Text.ToString();
                inputarray[20] = txtHeight.Text.ToString();
                inputarray[21] = ddlFedexPackagingOptions.Value.ToString();
                inputarray[22] = ddlFedexPayementType.Value.ToString();
                inputarray[23] = ddlFedexServiceType.Value.ToString();
                inputarray[24] = ddlFedexDropofftype.Value.ToString();
                if (chkFedexResidence.Checked == true)
                {
                    inputarray[25] = "Yes";
                }
                else
                {
                    inputarray[25] = "No";
                }
                inputarray[26] = txtDHLDate.Text.ToString();
                if (ddlFedexServiceType.Value.ToString() == "FEDEX_EXPRESS_SAVER" || ddlFedexServiceType.Value.ToString() == "FEDEX_GROUND")
                {
                    inputarray[27] = txtFedexDeclaredValue.Text.ToString();
                }
                else
                {
                    inputarray[27] = "";
                }
                DataSet dsRatesShip = new DataSet();
                if (types == "Rates")
                {
                    try
                    {
                        dsRatesShip = objFedRateserviceLocal.CheckAllDetails(inputarray);
                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("Delivery Date", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Delivery Day", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Packaging Type", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Billing Weight", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Freight Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Surcharges Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Net charges Amount", System.Type.GetType("System.String")));
                        if (dsRatesShip.Tables[0].Columns.Count > 20)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Delivery date"] = dsRatesShip.Tables[0].Rows[0]["Delivery date"].ToString();
                            dr["Delivery day"] = dsRatesShip.Tables[0].Rows[0]["Delivery day"].ToString();
                            dr["Packaging Type"] = dsRatesShip.Tables[0].Rows[0]["Packaging Type"].ToString();
                            dr["Billing Weight"] = dsRatesShip.Tables[0].Rows[0]["Total billing weight value"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Total billing weight units"].ToString();
                            dr["Freight Amount"] = dsRatesShip.Tables[0].Rows[0]["Total net freight Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Total net freight Currency"].ToString();
                            dr["Surcharges Amount"] = dsRatesShip.Tables[0].Rows[0]["Total surcharges Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Total surcharges Currency"].ToString();
                            dr["Net charges Amount"] = dsRatesShip.Tables[0].Rows[0]["Total net charge Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Total net charge Currency"].ToString();
                            dt.Rows.Add(dr);
                            gvFEDEX.Visible = true;
                            gvFEDEX.DataSource = dt;
                            gvFEDEX.DataBind();
                            result = "Grid";
                        }
                        else
                        {
                            lblExceptions.Text = "";
                            lblExceptions.Text = dsRatesShip.Tables[0].Rows[0][0].ToString();
                            result = "Label";
                        }
                    }
                    catch (Exception exds)
                    {
                        lblExceptions.Text = "";
                        if (dsRatesShip.Tables.Count == 0)
                        {
                            lblExceptions.Text = "Not Correct Input Data is Found";
                        }
                        else
                        {
                            lblExceptions.Text = dsRatesShip.Tables[0].Rows[0][1].ToString() + dsRatesShip.Tables[0].Rows[0][2].ToString();
                        }
                        result = "Label";
                    }
                }
                else
                {
                    if (types == "Ship")
                    {
                        try
                        {
                            dsRatesShip = objFedShipServiceLocal.CreateShipment(inputarray);
                            //
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn("Shipment No", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Delivery Day", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Delivery Date", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Billing Weight Units", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Surcharge Type", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Surcharge Amount", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Base Charges", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Total Charges", System.Type.GetType("System.String")));

                            if (dsRatesShip.Tables.Count == 1)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Shipment No"] = dsRatesShip.Tables[0].Rows[0]["Tracking ID"].ToString();
                                dr["Delivery Day"] = dsRatesShip.Tables[0].Rows[0]["Delivery Day"].ToString();
                                dr["Delivery Date"] = dsRatesShip.Tables[0].Rows[0]["Delivery Date"].ToString();
                                dr["Billing Weight Units"] = dsRatesShip.Tables[0].Rows[0]["Billing Weight Units"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Billing Weight Value"].ToString();
                                dr["Surcharge Type"] = dsRatesShip.Tables[0].Rows[0]["Surcharge Type"].ToString();
                                dr["Surcharge Amount"] = dsRatesShip.Tables[0].Rows[0]["Surcharge Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Surcharge Currency"].ToString();
                                dr["Base Charges"] = dsRatesShip.Tables[0].Rows[0]["Base Charge Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Base Charge Currency"].ToString();
                                dr["Total Charges"] = dsRatesShip.Tables[0].Rows[0]["Net Charge Amount"].ToString() + " " + dsRatesShip.Tables[0].Rows[0]["Net Charge Currency"].ToString();
                                dt.Rows.Add(dr);
                                gvFEDEX.Visible = true;
                                gvFEDEX.DataSource = dt;
                                gvFEDEX.DataBind();
                                result = "Grid";
                            }
                            else
                            {
                                lblExceptions.Text = "";
                                if (dsRatesShip.Tables.Count == 0)
                                {
                                    lblExceptions.Text = "Not Correct Input Data is Found";
                                }
                                else
                                {
                                    lblExceptions.Text = dsRatesShip.Tables[0].Rows[0][0].ToString();
                                }
                                result = "Label";
                            }
                        }
                        catch (Exception exds)
                        {
                            lblExceptions.Text = "";
                            lblExceptions.Text = dsRatesShip.Tables[0].Rows[0][1].ToString() + dsRatesShip.Tables[0].Rows[0][2].ToString();
                            result = "Label";
                        }
                    }
                }
            }
            else
            {
                if (types == "Track")
                {
                    string[] input = new string[10];
                    DataSet dsTrackShip = new DataSet();
                    input[0] = txtKeys.Value.ToString();
                    input[1] = txtPasswords.Value.ToString();
                    input[2] = txtAccountNumbers.Value.ToString();
                    input[3] = txtMeterNumbers.Value.ToString();
                    input[4] = txtTracking.Text.ToString();
                    input[5] = txtReceiverCity.Text.ToString();
                    input[6] = ddlReceiverState.Value.ToString();
                    input[7] = ddlReceiverCountry.Value.ToString();
                    input[8] = txtReceiverPostalCode.Text.ToString();
                    input[9] = txtAccountNumbers.Value.ToString();
                    dsTrackShip = objFedTrackServiceLocal.GetTrackingDetails(input);
                }
            }
            return result;
        }
        #endregion

        #region UPS XML Schema
        //Creation Of UPS XML Schema
        public string CreateUPSXMLSchema(string types)
        {
            string result = string.Empty;
            XmlDocument doc1 = new XmlDocument();
            XmlDocument doc = new XmlDocument();
            //UPS Rates XML Schema
            if (types == "Rates")
            {
                XmlNode nodeDeclaration = doc1.CreateXmlDeclaration("1.0", "utf-16", string.Empty);
                doc1.AppendChild(nodeDeclaration);
                //
                XmlNode eleAccessRequest = doc1.CreateElement("AccessRequest");
                XmlElement eleaccesslicenseno = doc1.CreateElement("AccessLicenseNumber");
                XmlElement eleuserid = doc1.CreateElement("UserId");
                XmlElement elePassword = doc1.CreateElement("Password");
                eleAccessRequest.AppendChild(eleaccesslicenseno);
                eleAccessRequest.AppendChild(eleuserid);
                eleAccessRequest.AppendChild(elePassword);
                eleaccesslicenseno.InnerText = txtKeys.Value.ToString();
                eleuserid.InnerText = txtPasswords.Value.ToString();
                elePassword.InnerText = txtAccountNumbers.Value.ToString();
                XmlAttribute attraccessrequest1 = doc1.CreateAttribute("xmlns:xsi");
                attraccessrequest1.Value = "http://www.w3.org/2001/XMLSchema-instance";
                eleAccessRequest.Attributes.Append(attraccessrequest1);
                XmlAttribute attraccessrequest2 = doc1.CreateAttribute("xmlns:xsd");
                attraccessrequest2.Value = "http://www.w3.org/2001/XMLSchema";
                eleAccessRequest.Attributes.Append(attraccessrequest2);
                // 
                XmlNode nodeDeclaration1 = doc.CreateXmlDeclaration("1.0", "utf-16", string.Empty);
                doc.AppendChild(nodeDeclaration1);
                XmlNode eleRatingServiceSelectionRequest = doc.CreateElement("RatingServiceSelectionRequest");
                XmlElement eleRequest = doc.CreateElement("Request");
                XmlElement eleTransactionReference = doc.CreateElement("TransactionReference");
                XmlElement eleXpciVersion = doc.CreateElement("XpciVersion");
                XmlElement eleCustomerContext = doc.CreateElement("CustomerContext");
                eleXpciVersion.InnerText = "1.0001";
                eleCustomerContext.InnerText = "1";
                XmlElement eleRequestAction = doc.CreateElement("RequestAction");
                XmlElement eleRequestOption = doc.CreateElement("RequestOption");
                eleRequestAction.InnerText = "Rate";
                eleRequestOption.InnerText = "Rate";
                eleTransactionReference.AppendChild(eleXpciVersion);
                eleTransactionReference.AppendChild(eleCustomerContext);
                eleRequest.AppendChild(eleTransactionReference);
                eleRequest.AppendChild(eleRequestAction);
                eleRequest.AppendChild(eleRequestOption);
                eleRatingServiceSelectionRequest.AppendChild(eleRequest);
                XmlAttribute attraccessrequest3 = doc.CreateAttribute("xmlns:xsi");
                attraccessrequest3.Value = "http://www.w3.org/2001/XMLSchema-instance";
                eleRatingServiceSelectionRequest.Attributes.Append(attraccessrequest3);
                XmlAttribute attraccessrequest4 = doc.CreateAttribute("xmlns:xsd");
                attraccessrequest4.Value = "http://www.w3.org/2001/XMLSchema";
                eleRatingServiceSelectionRequest.Attributes.Append(attraccessrequest4);
                //
                XmlElement elePickUpType = doc.CreateElement("PickUpType");
                XmlElement eleCode = doc.CreateElement("Code");
                XmlElement eleDescreption = doc.CreateElement("Description");
                eleCode.InnerText = ddlUPSPickUp.Value.ToString();
                eleDescreption.InnerText = "Hi This iS new PickUp";
                elePickUpType.AppendChild(eleCode);
                elePickUpType.AppendChild(eleDescreption);
                eleRatingServiceSelectionRequest.AppendChild(elePickUpType);
                //
                XmlElement eleShipment = doc.CreateElement("Shipment");
                //
                XmlElement eleShipper = doc.CreateElement("Shipper");
                //
                XmlElement eleShipperAddress = doc.CreateElement("Address");
                XmlElement eleAddressLine1 = doc.CreateElement("AddressLine1");
                XmlElement eleCity = doc.CreateElement("City");
                XmlElement eleStateProvinceCode = doc.CreateElement("StateProvinceCode");
                XmlElement elePostalCode = doc.CreateElement("PostalCode");
                XmlElement eleCountryCode = doc.CreateElement("CountryCode");
                eleShipperAddress.AppendChild(eleAddressLine1);
                eleShipperAddress.AppendChild(eleCity);
                eleShipperAddress.AppendChild(eleStateProvinceCode);
                eleShipperAddress.AppendChild(elePostalCode);
                eleShipperAddress.AppendChild(eleCountryCode);
                eleAddressLine1.InnerText = txtShipperAddress.Text.ToString();
                eleCity.InnerText = txtCity.Text.ToString();
                eleStateProvinceCode.InnerText = inptState.Value.ToString();
                elePostalCode.InnerText = txtPostalCode.Text.ToString();
                eleCountryCode.InnerText = ddlShipCountry.Value.ToString();
                //
                XmlElement eleShipperName = doc.CreateElement("ShipperName");
                eleShipperName.InnerText = txtShipperName.Text.ToString();
                XmlElement eleShipperPhoneNo = doc.CreateElement("PhoneNumber");
                eleShipperPhoneNo.InnerText = txtPhoneNo.Text.ToString();
                XmlElement eleShipperShipNo = doc.CreateElement("ShipNumber");
                eleShipperShipNo.InnerText = "9C2749BB941D3908";
                eleShipper.AppendChild(eleShipperAddress);
                eleShipper.AppendChild(eleShipperName);
                eleShipper.AppendChild(eleShipperPhoneNo);
                eleShipper.AppendChild(eleShipperShipNo);
                //
                XmlElement eleShipTo = doc.CreateElement("ShipTo");
                XmlElement eleShipToAddress = doc.CreateElement("Address");
                XmlElement eleShipToAddressLine1 = doc.CreateElement("AddressLine1");
                XmlElement eleShipToCity = doc.CreateElement("City");
                XmlElement eleShipToStateProvinceCode = doc.CreateElement("StateProvinceCode");
                XmlElement eleShipToPostalCode = doc.CreateElement("PostalCode");
                XmlElement eleShipToCountryCode = doc.CreateElement("CountryCode");
                eleShipToAddress.AppendChild(eleShipToAddressLine1);
                eleShipToAddress.AppendChild(eleShipToCity);
                eleShipToAddress.AppendChild(eleShipToStateProvinceCode);
                eleShipToAddress.AppendChild(eleShipToPostalCode);
                eleShipToAddress.AppendChild(eleShipToCountryCode);
                eleShipToAddressLine1.InnerText = txtStreetReceiver.Text.ToString();
                eleShipToCity.InnerText = txtReceiverCity.Text.ToString();
                eleShipToStateProvinceCode.InnerText = ddlReceiverState.Value.ToString();
                eleShipToPostalCode.InnerText = txtReceiverPostalCode.Text.ToString();
                eleShipToCountryCode.InnerText = ddlReceiverCountry.Value.ToString();
                XmlElement eleShipToCompanyName = doc.CreateElement("CompanyName");
                XmlElement eleShipToPhoneNo = doc.CreateElement("PhoneNumber");
                eleShipToCompanyName.InnerText = txtDHLCompany.Text.ToString();
                eleShipToPhoneNo.InnerText = txtReceiverPhoneNo.Text.ToString();
                eleShipTo.AppendChild(eleShipToAddress);
                eleShipTo.AppendChild(eleShipToCompanyName);
                eleShipTo.AppendChild(eleShipToPhoneNo);
                //
                XmlElement eleShipFrom = doc.CreateElement("ShipFrom");
                XmlElement eleShipFromAddress = doc.CreateElement("Address");
                XmlElement eleShipFromAddressLine1 = doc.CreateElement("AddressLine1");
                XmlElement eleShipFromCity = doc.CreateElement("City");
                XmlElement eleShipFromStateProvinceCode = doc.CreateElement("StateProvinceCode");
                XmlElement eleShipFromPostalCode = doc.CreateElement("PostalCode");
                XmlElement eleShipFromCountryCode = doc.CreateElement("CountryCode");
                eleShipFromAddress.AppendChild(eleShipFromAddressLine1);
                eleShipFromAddress.AppendChild(eleShipFromCity);
                eleShipFromAddress.AppendChild(eleShipFromStateProvinceCode);
                eleShipFromAddress.AppendChild(eleShipFromPostalCode);
                eleShipFromAddress.AppendChild(eleShipFromCountryCode);
                eleShipFromAddressLine1.InnerText = txtStreetFrom.Text.ToString();
                eleShipFromCity.InnerText = txtCityAll.Text.ToString();
                eleShipFromStateProvinceCode.InnerText = ddlStateAll.Value.ToString();
                eleShipFromPostalCode.InnerText = txtPostalCodeAll.Text.ToString();
                eleShipFromCountryCode.InnerText = ddlCountryAll.Value.ToString();
                XmlElement eleShipFromCompanyName = doc.CreateElement("CompanyName");
                XmlElement eleShipFromAttentionName = doc.CreateElement("AttentionName");
                XmlElement eleShipFromPhoneNo = doc.CreateElement("PhoneNumber");
                XmlElement eleShipFromFaxNo = doc.CreateElement("FaxNumber");
                eleShipFromCompanyName.InnerText = txtDHLCompany.Text.ToString();
                eleShipFromAttentionName.InnerText = txtDHLSentBy.Text.ToString();
                eleShipFromPhoneNo.InnerText = txtPhoneNoAll.Text.ToString();
                eleShipFromFaxNo.InnerText = "";
                eleShipFrom.AppendChild(eleShipFromAddress);
                eleShipFrom.AppendChild(eleShipFromCompanyName);
                eleShipFrom.AppendChild(eleShipFromPhoneNo);
                //
                XmlElement eleService = doc.CreateElement("Service");
                XmlElement eleSerCode = doc.CreateElement("Code");
                eleService.AppendChild(eleSerCode);
                eleSerCode.InnerText = ddlUPSServiceTypes.Value.ToString();
                //
                XmlElement elePayementInformation = doc.CreateElement("PayementInformation");
                XmlElement elePrepaid = doc.CreateElement("Prepaid");
                XmlElement eleBillshipper = doc.CreateElement("BillShipper");
                XmlElement elePrepaidPrepaid = doc.CreateElement("Prepaid");
                XmlElement eleAccountNo = doc.CreateElement("AccountNumber");
                elePrepaid.AppendChild(eleBillshipper);
                elePrepaid.AppendChild(elePrepaidPrepaid);
                eleBillshipper.AppendChild(eleAccountNo);
                elePayementInformation.AppendChild(elePrepaid);
                eleAccountNo.InnerText = "555EY1";
                elePrepaidPrepaid.InnerText = "Hi This is New";
                //
                XmlElement elePackage = doc.CreateElement("Package");
                XmlElement elePackagingType = doc.CreateElement("PackagingType");
                XmlElement elePackagingWeight = doc.CreateElement("PackageWeight");
                XmlElement elePackagingDescreption = doc.CreateElement("Description");
                elePackage.AppendChild(elePackagingType);
                elePackage.AppendChild(elePackagingWeight);
                elePackage.AppendChild(elePackagingDescreption);
                elePackagingDescreption.InnerText = "Rate";
                XmlElement elePackagingTypeCode = doc.CreateElement("Code");
                XmlElement elePackagingTypeDesc = doc.CreateElement("Description");
                elePackagingType.AppendChild(elePackagingTypeCode);
                elePackagingType.AppendChild(elePackagingTypeDesc);
                elePackagingTypeCode.InnerText = ddlUPSPackagingType.Value.ToString();
                elePackagingTypeDesc.InnerText = "New Package";
                XmlElement elePackageWgtUnitOfMeasurment = doc.CreateElement("UnitOfmeasurment");
                XmlElement elePackageWgtUnitOfMeasurmentCode = doc.CreateElement("Code");
                elePackageWgtUnitOfMeasurment.AppendChild(elePackageWgtUnitOfMeasurmentCode);
                XmlElement elePackageWgtUnitOfMeasurmentWeight = doc.CreateElement("Weight");
                elePackageWgtUnitOfMeasurmentWeight.InnerText = txtWeight.Text.ToString();
                elePackageWgtUnitOfMeasurmentCode.InnerText = ddlUnits.Value.ToString();
                elePackagingWeight.AppendChild(elePackageWgtUnitOfMeasurment);
                elePackagingWeight.AppendChild(elePackageWgtUnitOfMeasurmentWeight);
                //
                XmlElement eleShipmentServiceOptions = doc.CreateElement("ShipmentServiceOptions");
                XmlElement eleOnCallAir = doc.CreateElement("OnCallAir");
                XmlElement eleSchedule = doc.CreateElement("Schedule");
                XmlElement elePickUpDay = doc.CreateElement("PickUpDay");
                XmlElement eleMethod = doc.CreateElement("Method");
                elePickUpDay.InnerText = "02";
                eleMethod.InnerText = "02";
                eleSchedule.AppendChild(elePickUpDay);
                eleSchedule.AppendChild(eleMethod);
                eleOnCallAir.AppendChild(eleSchedule);
                eleShipmentServiceOptions.AppendChild(eleOnCallAir);
                //
                XmlElement eleShipmentDescreption = doc.CreateElement("Description");
                eleShipmentDescreption.InnerText = "This is New Shipment";
                //
                eleShipment.AppendChild(eleShipper);
                eleShipment.AppendChild(eleShipTo);
                eleShipment.AppendChild(eleShipFrom);
                eleShipment.AppendChild(eleService);
                eleShipment.AppendChild(elePayementInformation);
                eleShipment.AppendChild(elePackage);
                eleShipment.AppendChild(eleShipmentServiceOptions);
                eleShipment.AppendChild(eleShipmentDescreption);
                //
                eleRatingServiceSelectionRequest.AppendChild(eleShipment);
                doc1.AppendChild(eleAccessRequest);
                doc.AppendChild(eleRatingServiceSelectionRequest);
                //
                string xml1 = doc1.OuterXml.ToString();
                string xml2 = doc.OuterXml.ToString();
                string xml3 = xml1 + xml2;
                //string xml3 = "<?xml version='1.0'?><AccessRequest xml:lang='en-US'><AccessLicenseNumber>9C2749BB941D3908</AccessLicenseNumber><UserId>nblajit</UserId><Password>N9111N9111</Password></AccessRequest><?xml version='1.0'?><AddressValidationRequest xml:lang='en-US'><Request><TransactionReference><CustomerContext>Customer Data</CustomerContext><XpciVersion>1.0001</XpciVersion></TransactionReference><RequestAction>AV</RequestAction></Request><Address><City>MIAMI</City><StateProvinceCode>FL</StateProvinceCode></Address></AddressValidationRequest>";
                string xmlresponse = GetRequest(xml3, types);
                result = SendResponse(xmlresponse, "UPS", types);
            }
            else
            {
                if (types == "Ship")
                {
                    lblExceptions.Text = "";
                    lblExceptions.Text = "No Shipping Services are Provided";
                    result = "Label";
                }
                else
                {
                    //UPS Track XML Schema
                    if (types == "Track")
                    {
                        XmlNode nodeDeclaration = doc1.CreateXmlDeclaration("1.0", "utf-16", string.Empty);
                        doc1.AppendChild(nodeDeclaration);
                        //
                        XmlNode eleAccessRequest = doc1.CreateElement("AccessRequest");
                        XmlElement eleaccesslicenseno = doc1.CreateElement("AccessLicenseNumber");
                        XmlElement eleuserid = doc1.CreateElement("UserId");
                        XmlElement elePassword = doc1.CreateElement("Password");
                        eleAccessRequest.AppendChild(eleaccesslicenseno);
                        eleAccessRequest.AppendChild(eleuserid);
                        eleAccessRequest.AppendChild(elePassword);
                        eleaccesslicenseno.InnerText = txtKeys.Value.ToString();
                        eleuserid.InnerText = txtPasswords.Value.ToString();
                        elePassword.InnerText = txtMeterNumbers.Value.ToString();
                        //
                        XmlNode nodeDeclaration1 = doc.CreateXmlDeclaration("1.0", "utf-16", string.Empty);
                        doc.AppendChild(nodeDeclaration1);
                        XmlNode eleTrackRequest = doc.CreateElement("TrackRequest");
                        XmlElement eleRequest = doc.CreateElement("Request");
                        XmlElement eleTransactionReference = doc.CreateElement("TransactionReference");
                        XmlElement eleXpciVersion = doc.CreateElement("XpciVersion");
                        XmlElement eleCustomerContext = doc.CreateElement("CustomerContext");
                        XmlElement eleInternalKey = doc.CreateElement("InternalKey");
                        eleXpciVersion.InnerText = "1.0001";
                        eleInternalKey.InnerText = "Chandu";
                        eleCustomerContext.AppendChild(eleInternalKey);
                        XmlElement eleRequestAction = doc.CreateElement("RequestAction");
                        XmlElement eleRequestOption = doc.CreateElement("RequestOption");
                        eleRequestAction.InnerText = "Track";
                        eleTransactionReference.AppendChild(eleXpciVersion);
                        eleTransactionReference.AppendChild(eleCustomerContext);
                        eleRequest.AppendChild(eleTransactionReference);
                        eleRequest.AppendChild(eleRequestAction);
                        eleRequest.AppendChild(eleRequestOption);
                        eleTrackRequest.AppendChild(eleRequest);
                        XmlElement eleTrackingNumber = doc.CreateElement("TrackingNumber");
                        eleTrackingNumber.InnerText = "";
                        eleTrackRequest.AppendChild(eleTrackingNumber);
                        //
                        doc1.AppendChild(eleAccessRequest);
                        doc.AppendChild(eleTrackRequest);
                        //
                        string xml1 = doc1.OuterXml.ToString();
                        string xml2 = doc.OuterXml.ToString();
                        string xml3 = xml1 + xml2;
                        //string xml3 = "<?xml version='1.0'?><AccessRequest xml:lang='en-US'><AccessLicenseNumber>9C2749BB941D3908</AccessLicenseNumber><UserId>nblajit</UserId><Password>N9111N9111</Password></AccessRequest><?xml version='1.0'?><AddressValidationRequest xml:lang='en-US'><Request><TransactionReference><CustomerContext>Customer Data</CustomerContext><XpciVersion>1.0001</XpciVersion></TransactionReference><RequestAction>AV</RequestAction></Request><Address><City>MIAMI</City><StateProvinceCode>FL</StateProvinceCode></Address></AddressValidationRequest>";
                        string xmlresponse = GetRequest(xml3, types);
                        result = SendResponse(xmlresponse, "UPS", types);
                    }
                }
            }
            return result;
            //string xml1 = doc1.OuterXml.ToString();
            //string xml2 = doc.OuterXml.ToString();
            //string xml3 = xml1 + xml2;
            ////string xml3 = "<?xml version='1.0'?><AccessRequest xml:lang='en-US'><AccessLicenseNumber>9C2749BB941D3908</AccessLicenseNumber><UserId>nblajit</UserId><Password>N9111N9111</Password></AccessRequest><?xml version='1.0'?><AddressValidationRequest xml:lang='en-US'><Request><TransactionReference><CustomerContext>Customer Data</CustomerContext><XpciVersion>1.0001</XpciVersion></TransactionReference><RequestAction>AV</RequestAction></Request><Address><City>MIAMI</City><StateProvinceCode>FL</StateProvinceCode></Address></AddressValidationRequest>";
            //string xmlresponse = GetRequest(xml3, types);
            //result = SendResponse(xmlresponse, "UPS", types);
            //return result;
        }
        #endregion

        #region DHL XML Schema
        public string CreateDHLXmlSchema(string types)
        {
            string result = string.Empty;
            //DHL Rates And Ship XML Schema
            if (types == "Rates" || types == "Ship")
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode nodeDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
                    doc.AppendChild(nodeDeclaration);
                    XmlNode nodeECommerce = doc.CreateElement("ECommerce");
                    XmlAttribute attaction = doc.CreateAttribute("action");
                    XmlAttribute attversion = doc.CreateAttribute("version");
                    attaction.Value = "Request";
                    attversion.Value = "1.0";
                    nodeECommerce.Attributes.Append(attaction);
                    nodeECommerce.Attributes.Append(attversion);
                    //
                    XmlElement eleRequestor = doc.CreateElement("Requestor");
                    XmlElement eleID = doc.CreateElement("ID");
                    XmlElement elePassword = doc.CreateElement("Password");
                    eleID.InnerText = txtKeys.Value.ToString();
                    elePassword.InnerText = txtPasswords.Value.ToString();
                    eleRequestor.AppendChild(eleID);
                    eleRequestor.AppendChild(elePassword);
                    doc.AppendChild(nodeECommerce);
                    doc.DocumentElement.AppendChild(eleRequestor);
                    //
                    XmlElement eleShipment = doc.CreateElement("Shipment");
                    XmlAttribute attraction1 = doc.CreateAttribute("action");
                    XmlAttribute attrversion1 = doc.CreateAttribute("version");
                    attraction1.Value = "GenerateLabel";
                    attrversion1.Value = "1.0";
                    eleShipment.Attributes.Append(attraction1);
                    eleShipment.Attributes.Append(attrversion1);
                    //
                    XmlElement eleShippingCredentials = doc.CreateElement("ShippingCredentials");
                    XmlElement eleShippingKey = doc.CreateElement("ShippingKey");
                    XmlElement eleAccountNbr = doc.CreateElement("AccountNbr");
                    eleShippingCredentials.AppendChild(eleShippingKey);
                    eleShippingCredentials.AppendChild(eleAccountNbr);
                    eleShipment.AppendChild(eleShippingCredentials);
                    eleShippingKey.InnerText = txtAccountNumbers.Value.ToString();
                    eleAccountNbr.InnerText = txtMeterNumbers.Value.ToString();
                    //
                    XmlElement eleShipmentDetail = doc.CreateElement("ShipmentDetail");
                    XmlElement eleShipDate = doc.CreateElement("ShipDate");
                    XmlElement eleService = doc.CreateElement("Service");
                    XmlElement eleCode = doc.CreateElement("Code");
                    eleService.AppendChild(eleCode);

                    DateTime dtdate = Convert.ToDateTime(txtDHLDate.Text.ToString());
                    string yy = Convert.ToString(dtdate.Year.ToString());
                    string mm = Convert.ToString(dtdate.Month.ToString());
                    string dd = Convert.ToString(dtdate.Day.ToString());
                    if (mm.Length == Convert.ToInt32("1"))
                    {
                        mm = "0" + mm;
                    }
                    else
                    {
                        mm = mm;
                    }
                    if (dd.Length == Convert.ToInt32("1"))
                    {
                        dd = "0" + dd;
                    }
                    else
                    {
                        dd = dd;
                    }
                    string dtxml = yy + "-" + mm + "-" + dd;
                    eleShipDate.InnerText = dtxml;
                    eleCode.InnerText = ddlDHLServiceType.Value.ToString();
                    eleShipmentDetail.AppendChild(eleShipDate);
                    eleShipmentDetail.AppendChild(eleService);
                    eleShipment.AppendChild(eleShipmentDetail);
                    XmlElement eleShipmentType = doc.CreateElement("ShipmentType");
                    XmlElement eleShipmentCode = doc.CreateElement("Code");
                    eleShipmentType.AppendChild(eleShipmentCode);
                    eleShipmentCode.InnerText = ddlDHLShipmentType.Value.ToString();
                    XmlElement eleweight = doc.CreateElement("Weight");
                    XmlElement eleContentDesc = doc.CreateElement("ContentDesc");
                    XmlElement eleShipperRef = doc.CreateElement("ShipperReference");
                    XmlElement eleDimensions = doc.CreateElement("Dimensions");
                    XmlElement eleLength = doc.CreateElement("Length");
                    XmlElement eleWidth = doc.CreateElement("Width");
                    XmlElement eleHeight = doc.CreateElement("Height");
                    eleDimensions.AppendChild(eleLength);
                    eleDimensions.AppendChild(eleWidth);
                    eleDimensions.AppendChild(eleHeight);
                    //
                    eleweight.InnerText = txtWeight.Text.ToString();
                    eleContentDesc.InnerText = txtDHLContentDescription.Text.ToString();
                    eleShipperRef.InnerText = txtDHLShipReference.Text.ToString();
                    eleLength.InnerText = txtLength.Text.ToString();
                    eleWidth.InnerText = txtWidth.Text.ToString();
                    eleHeight.InnerText = txtHeight.Text.ToString();
                    //
                    XmlElement eleAdditProtec = doc.CreateElement("AdditonalProtection");
                    XmlElement eleAddProtecCode = doc.CreateElement("Code");
                    XmlElement eleAddProtecValue = doc.CreateElement("Value");
                    eleAdditProtec.AppendChild(eleAddProtecCode);
                    eleAdditProtec.AppendChild(eleAddProtecValue);
                    eleShipmentDetail.AppendChild(eleweight);
                    eleShipmentDetail.AppendChild(eleContentDesc);
                    eleShipmentDetail.AppendChild(eleShipperRef);
                    eleShipmentDetail.AppendChild(eleShipmentType);
                    eleShipmentDetail.AppendChild(eleDimensions);
                    eleShipmentDetail.AppendChild(eleAdditProtec);
                    if (ddlDHLAdditionalProtection.Value.ToString() == "NR")
                    {
                        eleAddProtecCode.InnerText = ddlDHLAdditionalProtection.Value.ToString();
                        eleAddProtecValue.InnerText = "";
                    }
                    else
                    {
                        eleAddProtecCode.InnerText = ddlDHLAdditionalProtection.Value.ToString();
                        eleAddProtecValue.InnerText = txtDHLDeclaredValue.Text.ToString();
                    }
                    eleShipment.AppendChild(eleShipmentDetail);
                    //
                    XmlElement eleBilling = doc.CreateElement("Billing");
                    XmlElement eleParty = doc.CreateElement("Party");
                    XmlElement elePartyCode = doc.CreateElement("Code");
                    XmlElement eleAccountNbBilling = doc.CreateElement("AccountNbr");
                    //
                    eleParty.AppendChild(elePartyCode);
                    eleBilling.AppendChild(eleParty);
                    eleBilling.AppendChild(eleAccountNbBilling);
                    //
                    XmlElement eleCodPayment = doc.CreateElement("CODPayment");
                    XmlElement eleCodPayCode = doc.CreateElement("Code");
                    XmlElement eleCodPayValue = doc.CreateElement("Value");
                    //
                    eleCodPayment.AppendChild(eleCodPayCode);
                    eleCodPayment.AppendChild(eleCodPayValue);
                    eleBilling.AppendChild(eleCodPayment);
                    eleShipment.AppendChild(eleBilling);
                    if (ddlDHLBilling.Value.ToString() == "S")
                    {
                        elePartyCode.InnerText = ddlDHLBilling.Value.ToString();
                        eleAccountNbBilling.InnerText = txtMeterNumbers.Value.ToString();
                    }
                    else
                    {
                        elePartyCode.InnerText = ddlDHLBilling.Value.ToString();
                        eleAccountNbBilling.InnerText = txtMeterNumbers.Value.ToString();
                    }
                    eleCodPayCode.InnerText = ddlDHLCODCode.Value.ToString();
                    eleCodPayValue.InnerText = txtDHlCODValue.Text.ToString();
                    //
                    XmlElement eleSender = doc.CreateElement("Sender");
                    XmlElement eleSentBy = doc.CreateElement("SentBy");
                    XmlElement elePhoneNbr = doc.CreateElement("PhoneNbr");
                    XmlElement eleAddress = doc.CreateElement("Address");
                    eleSender.AppendChild(eleSentBy);
                    eleSender.AppendChild(elePhoneNbr);
                    eleSender.AppendChild(eleAddress);
                    //
                    eleSentBy.InnerText = txtDHLSentBy.Text.ToString();
                    elePhoneNbr.InnerText = txtPhoneNoAll.Text.ToString();
                    //
                    XmlElement eleSAComName = doc.CreateElement("CompanyName");
                    XmlElement eleSAStreet = doc.CreateElement("Street");
                    XmlElement eleSACity = doc.CreateElement("City");
                    XmlElement eleSAState = doc.CreateElement("State");
                    XmlElement eleSAPostalCode = doc.CreateElement("PostalCode");
                    XmlElement eleSACountry = doc.CreateElement("Country");
                    eleAddress.AppendChild(eleSAComName);
                    eleAddress.AppendChild(eleSAStreet);
                    eleAddress.AppendChild(eleSACity);
                    eleAddress.AppendChild(eleSAState);
                    eleAddress.AppendChild(eleSAPostalCode);
                    eleAddress.AppendChild(eleSACountry);

                    eleShipment.AppendChild(eleSender);

                    eleSAComName.InnerText = txtDHLCompany.Text.ToString();
                    eleSAStreet.InnerText = txtStreetFrom.Text.ToString();
                    eleSACity.InnerText = txtCityAll.Text.ToString();
                    eleSAState.InnerText = ddlStateAll.Value.ToString();
                    eleSAPostalCode.InnerText = txtPostalCodeAll.Text.ToString();
                    eleSACountry.InnerText = ddlCountryAll.Value.ToString();
                    //
                    XmlElement eleReceiver = doc.CreateElement("Receiver");
                    XmlElement eleRecAddress = doc.CreateElement("Address");
                    XmlElement eleRecAttnTo = doc.CreateElement("AttnTo");
                    XmlElement eleRecPhoneNbr = doc.CreateElement("PhoneNbr");
                    eleReceiver.AppendChild(eleRecAddress);
                    eleReceiver.AppendChild(eleRecAttnTo);
                    eleReceiver.AppendChild(eleRecPhoneNbr);
                    XmlElement eleRAComName = doc.CreateElement("CompanyName");
                    XmlElement eleRASuiteDeptName = doc.CreateElement("SuiteDepartmentName");
                    XmlElement eleRAStreet = doc.CreateElement("Street");
                    XmlElement eleRAStreetLine2 = doc.CreateElement("StreetLine2");
                    XmlElement eleRACity = doc.CreateElement("City");
                    XmlElement eleRAState = doc.CreateElement("State");
                    XmlElement eleRAPostalCode = doc.CreateElement("PostalCode");
                    XmlElement eleRACountry = doc.CreateElement("Country");
                    eleRecAddress.AppendChild(eleRAComName);
                    eleRecAddress.AppendChild(eleRASuiteDeptName);
                    eleRecAddress.AppendChild(eleRAStreet);
                    eleRecAddress.AppendChild(eleRAStreetLine2);
                    eleRecAddress.AppendChild(eleRACity);
                    eleRecAddress.AppendChild(eleRAState);
                    eleRecAddress.AppendChild(eleRAPostalCode);
                    eleRecAddress.AppendChild(eleRACountry);
                    //
                    eleRAComName.InnerText = txtCompanyNameReceiver.Text.ToString();
                    eleRASuiteDeptName.InnerText = txtDepartmentReceiverName.Text.ToString();
                    eleRAStreet.InnerText = txtStreetReceiver.Text.ToString();
                    eleRAStreetLine2.InnerText = txtReceiverStreet2.Text.ToString();
                    eleRACity.InnerText = txtReceiverCity.Text.ToString();
                    eleRAState.InnerText = ddlReceiverState.Value.ToString();
                    eleRAPostalCode.InnerText = txtReceiverPostalCode.Text.ToString();
                    eleRACountry.InnerText = ddlReceiverCountry.Value.ToString();
                    //
                    eleShipment.AppendChild(eleReceiver);
                    //
                    XmlElement eleShipInstructions = doc.CreateElement("ShipmentProcessingInstructions");
                    XmlElement eleLabel = doc.CreateElement("Label");
                    XmlElement eleImgType = doc.CreateElement("ImageType");
                    eleImgType.InnerText = ddlDHLImage.Value.ToString();
                    eleLabel.AppendChild(eleImgType);
                    eleShipInstructions.AppendChild(eleLabel);
                    eleShipment.AppendChild(eleShipInstructions);
                    //
                    XmlElement eleTrace = doc.CreateElement("TransactionTrace");
                    XmlElement eleCustomer = doc.CreateElement("customer");
                    XmlElement eleTraceID = doc.CreateElement("id");
                    XmlElement eleDemographic = doc.CreateElement("demographic");
                    eleCustomer.AppendChild(eleTraceID);
                    eleCustomer.AppendChild(eleDemographic);
                    eleTrace.AppendChild(eleCustomer);
                    eleTraceID.InnerText = txtDHLCustomerTranTraceID.Text.ToString();
                    eleDemographic.InnerText = txtDHLDemographic.Text.ToString();
                    //
                    eleShipment.AppendChild(eleTrace);
                    //
                    doc.DocumentElement.AppendChild(eleShipment);
                    //
                    string xmlformat = doc.OuterXml.ToString();
                    string xmlresponse = GetRequest(xmlformat, types);
                    result = SendResponse(xmlresponse, "DHL", types);
                }
                catch (Exception exs)
                {
                    lblExceptions.Text = exs.ToString();
                }

            }
            return result;
        }
        #endregion

        #region Sending Response For All XML Schemas
        public string SendResponse(string xmlresponse, string radSelected, string types)
        {
            string result = string.Empty;
            try
            {
                DataSet dsRowList = new DataSet();
                if (radSelected == "UPS" || radSelected == "DHL")
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlresponse);
                    XmlNode nodeRows;
                    if (radSelected == "UPS")
                    {
                        nodeRows = xDoc.SelectSingleNode("RatingServiceSelectionResponse");
                        //nodeRows = xDoc.SelectSingleNode("AddressValidationResponse");
                        if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                        {
                            XmlNodeReader read = new XmlNodeReader(nodeRows);
                            dsRowList.ReadXml(read);
                        }
                    }
                    else
                    {
                        nodeRows = xDoc.SelectSingleNode("ECommerce");
                        if (nodeRows != null && nodeRows.ChildNodes.Count > 0)
                        {
                            XmlNodeReader read = new XmlNodeReader(nodeRows);
                            dsRowList.ReadXml(read);
                        }
                    }
                }
                if (radSelected == "UPS")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("Delivery Date", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Delivery Day", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Packaging service type", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total billing weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net charge Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total Charges", System.Type.GetType("System.String")));
                    //
                    if (dsRowList.Tables.Count > 3)
                    {
                        DataRow dr = dt.NewRow();
                        if (dsRowList.Tables[2].Rows[0][2].ToString() == "1")
                        {
                            dr["Delivery date"] = "Tomorrow " + dsRowList.Tables[2].Rows[0][3].ToString();
                        }
                        else
                        {
                            dr["Delivery date"] = dsRowList.Tables[2].Rows[0][2].ToString();
                        }
                        dr["Delivery day"] = dsRowList.Tables[2].Rows[0][2].ToString();
                        dr["Packaging service type"] = ddlUPSPackagingType.Items[ddlUPSPackagingType.SelectedIndex].Text.ToString();
                        dr["Total billing weight Units"] = dsRowList.Tables["BillingWeight"].Rows[0][1].ToString() + " " + dsRowList.Tables["UnitOfMeasurement"].Rows[0][0].ToString();
                        dr["Total freight discount Amount"] = "0";
                        dr["Total net charge Amount"] = dsRowList.Tables[8].Rows[0][1].ToString();
                        dr["Total surcharges Amount"] = dsRowList.Tables["ServiceOptionsCharges"].Rows[0][1].ToString();
                        dr["Total Charges"] = dsRowList.Tables["TotalCharges"].Rows[0][1].ToString() + " " + dsRowList.Tables["TotalCharges"].Rows[0][0].ToString();
                        dt.Rows.Add(dr);
                        gvUPS.Visible = true;
                        gvUPS.DataSource = dt;
                        gvUPS.DataBind();
                        result = "Grid";
                    }
                    else
                    {
                        if (dsRowList.Tables[0].Rows[0][2].ToString() == "Failure")
                        {
                            lblExceptions.Text = "";
                            lblExceptions.Text = dsRowList.Tables[2].Rows[0][2].ToString();
                            result = "Label";
                        }
                    }
                }
                else if (radSelected == "DHL")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("AirBillNo", System.Type.GetType("System.Int64")));
                    dt.Columns.Add(new DataColumn("Delivery Date", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Delivery Day", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Packaging service type", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total billing weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net charge Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Amount", System.Type.GetType("System.String")));
                    if (dsRowList.Tables.Contains("Fault"))
                    {
                        lblExceptions.Text = "";
                        lblExceptions.Text = dsRowList.Tables[4].Rows[0][1].ToString();
                        result = "Label";
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["AirBillNo"] = Convert.ToInt64(dsRowList.Tables[5].Rows[0][1].ToString());
                        dr["Delivery date"] = dsRowList.Tables[5].Rows[0][3].ToString();
                        dr["Delivery day"] = dsRowList.Tables[8].Rows[0][0].ToString();
                        dr["Packaging service type"] = dsRowList.Tables[7].Rows[0][0].ToString();
                        dr["Total billing weight Units"] = ddlUnits.Value.ToString();
                        dr["Total freight discount Amount"] = dsRowList.Tables[8].Rows[0][1].ToString();
                        dr["Total net charge Amount"] = dsRowList.Tables[9].Rows[0][0].ToString();
                        dr["Total surcharges Amount"] = dsRowList.Tables[11].Rows[0][1].ToString();
                        dt.Rows.Add(dr);
                        gvDHL.Visible = true;
                        gvDHL.DataSource = dt;
                        gvDHL.DataBind();
                        result = "Grid";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion

        #region Getting Request For All XML Schemas
        public string GetRequest(string xmlstring, string types)
        {
            try
            {
                byte[] postData;
                HttpWebRequest req;
                Stream requestStream;
                string responseXml;
                WebResponse response;
                Stream responseStream;
                StreamReader sr;
                string StrUrl = string.Empty;
                //Convert the xml to bytes
                postData = System.Text.Encoding.UTF8.GetBytes(xmlstring);
                //Set up the request
                /* if (radUPS.Checked == true)
                 {
                     if (types == "Rates")
                     {
                         StrUrl = "https://www.ups.com/ups.app/xml/Rate";
                     }
                     else
                     {
                         if (types == "Address")
                         {
                             StrUrl = "https://www.ups.com/ups.app/xml/AV";
                         }
                     }
                 }
                 else
                 {*/
                if (radDHL.Checked == true)
                {
                    if (types == "Rates" || types == "Ship")
                    {
                        StrUrl = "HTTPS://eCommerce.airborne.com/ApiLandingTest.asp";
                    }
                }
                /*}*/
                req = (HttpWebRequest)WebRequest.Create(StrUrl);
                req.Method = "POST";
                req.ContentType = "application/xwwwformurlencoded";
                req.ContentLength = postData.Length;
                try
                {
                    requestStream = req.GetRequestStream();
                    // Send the data.
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();
                    //Get the response
                    response = req.GetResponse();
                    responseStream = response.GetResponseStream();
                    sr = new StreamReader(responseStream);
                    responseXml = sr.ReadToEnd();
                    return responseXml.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}
