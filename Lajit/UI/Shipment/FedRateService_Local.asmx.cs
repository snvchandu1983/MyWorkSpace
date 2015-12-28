using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using LAjitDev.Fedex_Rate_Services;


namespace LAjitDev.Shipment
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FedRateService_Local : System.Web.Services.WebService
    {
        Fedex_Rate_Services.RateAvailableServicesRequest request =new Fedex_Rate_Services.RateAvailableServicesRequest();

        DataSet retds1 =new DataSet();

        #region CheckAllDetails
        public DataSet CheckAllDetails(string[] inputarray)
        {
            try
            {
                request.WebAuthenticationDetail =new WebAuthenticationDetail();
                request.WebAuthenticationDetail.UserCredential =new WebAuthenticationCredential();
                request.WebAuthenticationDetail.UserCredential.Key = inputarray[0].ToString();
                request.WebAuthenticationDetail.UserCredential.Password = inputarray[1].ToString();

                request.ClientDetail =new ClientDetail();
                request.ClientDetail.AccountNumber = inputarray[2].ToString();
                request.ClientDetail.MeterNumber = inputarray[3].ToString();

                request.TransactionDetail = new TransactionDetail();
                request.TransactionDetail.CustomerTransactionId = "Lajit First Fedex Shipment"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.

                request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl            

                request.Origin = new Address(); // Origin information
                request.Origin.StreetLines = new string[1] { inputarray[4].ToString() };
                request.Origin.City = inputarray[5].ToString();
                request.Origin.StateOrProvinceCode = inputarray[6].ToString();
                request.Origin.CountryCode = inputarray[7].ToString();
                request.Origin.PostalCode = inputarray[8].ToString();

                request.Destination = new Address(); // Destination information
                request.Destination.StreetLines = new string[1] { inputarray[9].ToString() };
                request.Destination.City = inputarray[10].ToString();
                request.Destination.StateOrProvinceCode = inputarray[11].ToString();
                request.Destination.CountryCode = inputarray[12].ToString();
                request.Destination.PostalCode = inputarray[13].ToString(); ;

                request.CurrencyType = "USD";
                if (inputarray[24] == "DROP_BOX")
                {
                    request.DropoffType = DropoffType.DROP_BOX;
                }
                else if (inputarray[24] == "BUSINESS_SERVICE_CENTER")
                {
                    request.DropoffType = DropoffType.BUSINESS_SERVICE_CENTER;
                }
                else if (inputarray[24] == "REGULAR_PICKUP")
                {
                    request.DropoffType = DropoffType.REGULAR_PICKUP;
                }
                else if (inputarray[24] == "REQUEST_COURIER")
                {
                    request.DropoffType = DropoffType.REQUEST_COURIER;
                }
                else
                {
                    request.DropoffType = DropoffType.STATION;
                }
                //
                request.DropoffTypeSpecified = true;
                request.ShipDate = Convert.ToDateTime(inputarray[26].ToString());
                request.RateRequestTypes = new RateRequestType[1] { RateRequestType.ACCOUNT }; // Request options are: ACCOUNT, LIST
                // 
                request.ServiceType =new ServiceType();
                if (inputarray[23] == "EUROPE_FIRST_INTERNATIONAL_PRIORITY")
                {
                    request.ServiceType = ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                }
                else if (inputarray[23] == "FEDEX_1_DAY_FREIGHT")
                {
                    request.ServiceType = ServiceType.FEDEX_1_DAY_FREIGHT;
                }
                else if (inputarray[23] == "FEDEX_2_DAY")
                {
                    request.ServiceType = ServiceType.FEDEX_2_DAY;
                }
                else if (inputarray[23] == "FEDEX_2_DAY_FREIGHT")
                {
                    request.ServiceType = ServiceType.FEDEX_2_DAY_FREIGHT;
                }
                else if (inputarray[23] == "FEDEX_3_DAY_FREIGHT")
                {
                    request.ServiceType = ServiceType.FEDEX_3_DAY_FREIGHT;
                }
                else if (inputarray[23] == "FEDEX_EXPRESS_SAVER")
                {
                    request.ServiceType = ServiceType.FEDEX_EXPRESS_SAVER;
                }
                else if (inputarray[23] == "FEDEX_GROUND")
                {
                    request.ServiceType = ServiceType.FEDEX_GROUND;
                }
                else if (inputarray[23] == "FIRST_OVERNIGHT")
                {
                    request.ServiceType = ServiceType.FIRST_OVERNIGHT;
                }
                else if (inputarray[23] == "GROUND_HOME_DELIVERY")
                {
                    request.ServiceType = ServiceType.GROUND_HOME_DELIVERY;
                }
                else if (inputarray[23] == "INTERNATIONAL_DISTRIBUTION_FREIGHT")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_DISTRIBUTION_FREIGHT;
                }
                else if (inputarray[23] == "INTERNATIONAL_ECONOMY")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_ECONOMY;
                }
                else if (inputarray[23] == "INTERNATIONAL_ECONOMY_DISTRIBUTION")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_ECONOMY_DISTRIBUTION;
                }
                else if (inputarray[23] == "INTERNATIONAL_ECONOMY_FREIGHT")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                }
                else if (inputarray[23] == "INTERNATIONAL_FIRST")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_FIRST;
                }
                else if (inputarray[23] == "INTERNATIONAL_PRIORITY")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_PRIORITY;
                }
                else if (inputarray[23] == "INTERNATIONAL_PRIORITY_DISTRIBUTION")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_PRIORITY_DISTRIBUTION;
                }
                else if (inputarray[23] == "INTERNATIONAL_PRIORITY_FREIGHT")
                {
                    request.ServiceType = ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                }
                else if (inputarray[23] == "PRIORITY_OVERNIGHT")
                {
                    request.ServiceType = ServiceType.PRIORITY_OVERNIGHT;
                }
                else
                {
                    request.ServiceType = ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                }
                //
                request.RateRequestPackageSummary = new RateRequestPackageSummary(); // package summary
                request.RateRequestPackageSummary.PieceCount = inputarray[16].ToString();
                request.RateRequestPackageSummary.TotalWeight = new Weight();
                request.RateRequestPackageSummary.TotalWeight.Value = Convert.ToDecimal(inputarray[14]);
                //request.RateRequestPackageSummary.TotalInsuredValue.Amount = Convert.ToDecimal(inputarray[27].ToString());

                //
                request.PackagingType =new PackagingType();
                if (inputarray[21] == "FEDEX_10KG_BOX")
                {
                    request.PackagingType = PackagingType.FEDEX_10KG_BOX;
                }
                else if (inputarray[21] == "FEDEX_25KG_BOX")
                {
                    request.PackagingType = PackagingType.FEDEX_25KG_BOX;
                }
                else if (inputarray[21] == "FEDEX_BOX")
                {
                    request.PackagingType = PackagingType.FEDEX_BOX;
                }
                else if (inputarray[21] == "FEDEX_ENVELOPE")
                {
                    request.PackagingType = PackagingType.FEDEX_ENVELOPE;
                }
                else if (inputarray[21] == "FEDEX_PAK")
                {
                    request.PackagingType = PackagingType.FEDEX_PAK;
                }
                else if (inputarray[21] == "FEDEX_TUBE")
                {
                    request.PackagingType = PackagingType.FEDEX_TUBE;
                }
                else
                {
                    request.PackagingType = PackagingType.YOUR_PACKAGING;
                }
                //
                string weightunitsKG = string.Empty;
                string weightunitsLB = string.Empty;
                weightunitsKG = WeightUnits.KG.ToString();
                weightunitsLB = WeightUnits.LB.ToString();
                if (inputarray[15].ToString().ToUpper() == weightunitsKG)
                {
                    request.RateRequestPackageSummary.TotalWeight.Units = WeightUnits.KG;
                }
                else
                {
                    request.RateRequestPackageSummary.TotalWeight.Units = WeightUnits.LB;
                }
                request.RateRequestPackageSummary.PerPieceDimensions = new Dimensions();
                request.RateRequestPackageSummary.PerPieceDimensions.Length = inputarray[18].ToString();
                request.RateRequestPackageSummary.PerPieceDimensions.Width = inputarray[19].ToString();
                request.RateRequestPackageSummary.PerPieceDimensions.Height = inputarray[20].ToString();
                request.RateRequestPackageSummary.PerPieceDimensions.Units = LinearUnits.IN;
                request.RateRequestPackageSummary.SpecialServicesRequested = new PackageSpecialServicesRequested();

                request.RateRequestPackageSummary.SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[1] { PackageSpecialServiceType.DANGEROUS_GOODS };
                request.RateRequestPackageSummary.SpecialServicesRequested.DangerousGoodsDetail = new DangerousGoodsDetail();
                request.RateRequestPackageSummary.SpecialServicesRequested.DangerousGoodsDetail.Accessibility = DangerousGoodsAccessibilityType.ACCESSIBLE;
                request.RateRequestPackageSummary.SpecialServicesRequested.DangerousGoodsDetail.AccessibilitySpecified = true;

                Fedex_Rate_Services.RateService service =new Fedex_Rate_Services.RateService();
                RateAvailableServicesReply reply = service.rateAvailableServices(request);
                if ((reply.HighestSeverity != NotificationSeverityType.ERROR) && (reply.HighestSeverity != NotificationSeverityType.FAILURE))// check if the call was successful
                {
                    DataSet ds =new DataSet();
                    DataTable dt =new DataTable();

                    dt.Columns.Add(new DataColumn("S.No", System.Type.GetType("System.Int32")));
                    dt.Columns.Add(new DataColumn("Delivery date", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Delivery day", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Destination station id", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Packaging type", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Packaging service type", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Transit time", System.Type.GetType("System.String")));

                    dt.Columns.Add(new DataColumn("Total billing weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total billing weight Value", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total dim weight Value", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total dim weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net charge Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net charge Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net freight Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total net freight Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Currency", System.Type.GetType("System.String")));

                    dt.Columns.Add(new DataColumn("Billing weight Value", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Billing weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Dim weight Value", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Dim weight Units", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Base charge Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Base charge Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Net charge Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Net charge Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Net freight Amount", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Net freight Currency", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Amount1", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total surcharges Currency1", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Amount1", System.Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("Total freight discount Currency1", System.Type.GetType("System.String")));

                    dt.Columns["S.No"].AutoIncrement = true;
                    dt.Columns["S.No"].AutoIncrementStep = 1;

                    foreach (RateAndServiceOptions option in reply.Options)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Delivery date"] = option.ServiceDetail.DeliveryDate;
                        dr["Delivery day"] = option.ServiceDetail.DeliveryDay;
                        dr["Destination station id"] = option.ServiceDetail.DestinationStationId;
                        dr["Packaging type"] = option.ServiceDetail.PackagingType;
                        dr["Packaging service type"] = option.ServiceDetail.ServiceType;
                        dr["Transit time"] = option.ServiceDetail.TransitTime;
                        foreach (RatedShipmentDetail ratedShipmentDetail in option.RatedShipmentDetails)
                        {
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalBillingWeight != null)
                            {
                                dr["Total billing weight Units"] = ratedShipmentDetail.ShipmentRateDetail.TotalBillingWeight.Units.ToString();
                                dr["Total billing weight Value"] = ratedShipmentDetail.ShipmentRateDetail.TotalBillingWeight.Value.ToString();
                            }
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalDimWeight != null)
                            {
                                dr["Total dim weight Value"] = ratedShipmentDetail.ShipmentRateDetail.TotalDimWeight.Value.ToString();
                                dr["Total dim weight Units"] = ratedShipmentDetail.ShipmentRateDetail.TotalDimWeight.Units.ToString();
                            }
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalFreightDiscounts != null)
                            {
                                dr["Total freight discount Amount"] = ratedShipmentDetail.ShipmentRateDetail.TotalFreightDiscounts.Amount.ToString();
                                dr["Total freight discount Currency"] = ratedShipmentDetail.ShipmentRateDetail.TotalFreightDiscounts.Currency.ToString();
                            }
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalNetCharge != null)
                            {
                                dr["Total net charge Amount"] = ratedShipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount.ToString();
                                dr["Total net charge Currency"] = ratedShipmentDetail.ShipmentRateDetail.TotalNetCharge.Currency.ToString();
                            }
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalNetFreight != null)
                            {
                                dr["Total net freight Amount"] = ratedShipmentDetail.ShipmentRateDetail.TotalNetFreight.Amount.ToString();
                                dr["Total net freight Currency"] = ratedShipmentDetail.ShipmentRateDetail.TotalNetFreight.Currency.ToString();
                            }
                            if (ratedShipmentDetail.ShipmentRateDetail.TotalSurcharges != null)
                            {
                                dr["Total surcharges Amount"] = ratedShipmentDetail.ShipmentRateDetail.TotalSurcharges.Amount.ToString();
                                dr["Total surcharges Currency"] = ratedShipmentDetail.ShipmentRateDetail.TotalSurcharges.Currency.ToString();
                            }
                            if (null != ratedShipmentDetail.RatedPackages)
                            {
                                foreach (RatedPackageDetail package in ratedShipmentDetail.RatedPackages) // This is package details
                                {
                                    if (null != package.PackageRateDetail.BillingWeight)
                                    {
                                        dr["Billing weight Value"] = package.PackageRateDetail.BillingWeight.Value.ToString();
                                        dr["Billing weight Units"] = package.PackageRateDetail.BillingWeight.Units.ToString();
                                    }
                                    if (null != package.PackageRateDetail.DimWeight)
                                    {
                                        dr["Dim weight Value"] = package.PackageRateDetail.DimWeight.Value.ToString();
                                        dr["Dim weight Units"] = package.PackageRateDetail.DimWeight.Units.ToString();
                                    }
                                    if (null != package.PackageRateDetail.BaseCharge)
                                    {
                                        dr["Base charge Amount"] = package.PackageRateDetail.BaseCharge.Amount.ToString();
                                        dr["Base charge Currency"] = package.PackageRateDetail.BaseCharge.Currency.ToString();
                                    }
                                    if (null != package.PackageRateDetail.NetCharge)
                                    {
                                        dr["Net charge Amount"] = package.PackageRateDetail.NetCharge.Amount.ToString();
                                        dr["Net charge Currency"] = package.PackageRateDetail.NetCharge.Currency.ToString();
                                    }
                                    if (null != package.PackageRateDetail.NetFreight)
                                    {
                                        dr["Net freight Amount"] = package.PackageRateDetail.NetFreight.Amount.ToString();
                                        dr["Net freight Currency"] = package.PackageRateDetail.NetFreight.Currency.ToString();
                                    }
                                    if (null != package.PackageRateDetail.TotalSurcharges)
                                    {
                                        dr["Total surcharges Amount"] = package.PackageRateDetail.TotalSurcharges.Amount.ToString();
                                        dr["Total surcharges Currency"] = package.PackageRateDetail.TotalSurcharges.Currency.ToString();
                                    }
                                    if (null != package.PackageRateDetail.TotalFreightDiscounts)
                                    {
                                        dr["Total freight discount Amount"] = package.PackageRateDetail.TotalFreightDiscounts.Amount.ToString();
                                        dr["Total freight discount Currency"] = package.PackageRateDetail.TotalFreightDiscounts.Currency.ToString();
                                    }
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                    retds1 = ds;
                }
                else
                {
                    DataTable dterror =new DataTable();
                    DataRow dr = dterror.NewRow();
                    DataSet dsError =new DataSet();
                    dterror.Columns.Add(new DataColumn("ErrorMessage", System.Type.GetType("System.String")));
                    dr["ErrorMessage"] = reply.Notifications[0].Message.ToString();
                    dterror.Rows.Add(dr);
                    dsError.Tables.Add(dterror);
                    retds1 = dsError;
                }
                return retds1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}

