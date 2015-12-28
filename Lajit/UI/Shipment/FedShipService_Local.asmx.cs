using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using LAjitDev.Fedex_Express_US_Domestic_Shipping;
using System.IO;

namespace LAjitDev.Shipment
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FedShipService_Local : System.Web.Services.WebService
    {
        Fedex_Express_US_Domestic_Shipping.ProcessShipmentRequest request =new Fedex_Express_US_Domestic_Shipping.ProcessShipmentRequest();

        public DataSet CreateShipment(string[] inputShipment)
        {
            DataSet dsnew =new DataSet();
            try
            {
                request.WebAuthenticationDetail = new WebAuthenticationDetail();
                request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
                request.WebAuthenticationDetail.UserCredential.Key = inputShipment[0].ToString(); // Replace "XXX" with the Key
                request.WebAuthenticationDetail.UserCredential.Password = inputShipment[1].ToString(); ; // Replace "XXX" with the Password
                //
                request.ClientDetail = new ClientDetail();
                request.ClientDetail.AccountNumber = inputShipment[2].ToString(); ; // Replace "XXX" with clients account number
                request.ClientDetail.MeterNumber = inputShipment[3].ToString(); ; // Replace "XXX" with clients meter number
                //
                request.TransactionDetail = new TransactionDetail();
                request.TransactionDetail.CustomerTransactionId = "***Ship v3 Request using VC#***"; // The client will get the same value back in the response
                //
                request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl
                //          
                request.RequestedShipment = new RequestedShipment();
                request.RequestedShipment.DropoffType =new DropoffType();
                request.RequestedShipment.DropoffTypeSpecified = true;
                //
                if (inputShipment[24] == "DROP_BOX")
                {
                    request.RequestedShipment.DropoffType = DropoffType.DROP_BOX;
                }
                else if (inputShipment[24] == "BUSINESS_SERVICE_CENTER")
                {
                    request.RequestedShipment.DropoffType = DropoffType.BUSINESS_SERVICE_CENTER;
                }
                else if (inputShipment[24] == "REGULAR_PICKUP")
                {
                    request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP;
                }
                else if (inputShipment[24] == "REQUEST_COURIER")
                {
                    request.RequestedShipment.DropoffType = DropoffType.REQUEST_COURIER;
                }
                else
                {
                    request.RequestedShipment.DropoffType = DropoffType.STATION;
                }
                //
                request.RequestedShipment.PackageCount = inputShipment[16].ToString();
                //
                request.RequestedShipment.PackagingType =new PackagingType();
                //
                if (inputShipment[21] == "FEDEX_10KG_BOX")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_10KG_BOX;
                }
                else if (inputShipment[21] == "FEDEX_25KG_BOX")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_25KG_BOX;
                }
                else if (inputShipment[21] == "FEDEX_BOX")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_BOX;
                }
                else if (inputShipment[21] == "FEDEX_ENVELOPE")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_ENVELOPE;
                }
                else if (inputShipment[21] == "FEDEX_PAK")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_PAK;
                }
                else if (inputShipment[21] == "FEDEX_TUBE")
                {
                    request.RequestedShipment.PackagingType = PackagingType.FEDEX_TUBE;
                }
                else
                {
                    request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING;
                }
                //
                request.RequestedShipment.RateRequestTypes =new RateRequestType[1] { RateRequestType.ACCOUNT };
                //
                request.RequestedShipment.Shipper = new Party(); // Sender information
                request.RequestedShipment.Shipper.Contact = new Contact();
                request.RequestedShipment.Shipper.Contact.PersonName = "NBLajit";
                request.RequestedShipment.Shipper.Contact.CompanyName = "Lajit Inc..";
                request.RequestedShipment.Shipper.Contact.PhoneNumber = "205-555-5555";
                request.RequestedShipment.Shipper.Address = new Address();
                request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { inputShipment[4].ToString() };
                request.RequestedShipment.Shipper.Address.City = inputShipment[5].ToString();
                request.RequestedShipment.Shipper.Address.StateOrProvinceCode = inputShipment[6].ToString();
                request.RequestedShipment.Shipper.Address.PostalCode = inputShipment[8].ToString();
                request.RequestedShipment.Shipper.Address.CountryCode = inputShipment[7].ToString();
                //
                request.RequestedShipment.Recipient = new Party(); // Recipient information
                request.RequestedShipment.Recipient.Contact = new Contact();
                request.RequestedShipment.Recipient.Contact.PersonName = "Sekhar";
                request.RequestedShipment.Recipient.Contact.CompanyName = "Google";
                request.RequestedShipment.Recipient.Contact.PhoneNumber = "205-555-5555";
                request.RequestedShipment.Recipient.Address = new Address();
                request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { inputShipment[9].ToString() };
                request.RequestedShipment.Recipient.Address.City = inputShipment[10].ToString();
                request.RequestedShipment.Recipient.Address.StateOrProvinceCode = inputShipment[11].ToString();
                request.RequestedShipment.Recipient.Address.PostalCode = inputShipment[13].ToString();
                request.RequestedShipment.Recipient.Address.CountryCode = inputShipment[12].ToString();
                request.RequestedShipment.Recipient.Address.Residential = true;
                //
                request.RequestedShipment.ServiceType =new ServiceType();
                //
                if (inputShipment[23] == "EUROPE_FIRST_INTERNATIONAL_PRIORITY")
                {
                    request.RequestedShipment.ServiceType = ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                }
                else if (inputShipment[23] == "FEDEX_1_DAY_FREIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_1_DAY_FREIGHT;
                }
                else if (inputShipment[23] == "FEDEX_2_DAY")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_2_DAY;
                }
                else if (inputShipment[23] == "FEDEX_2_DAY_FREIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_2_DAY_FREIGHT;
                }
                else if (inputShipment[23] == "FEDEX_3_DAY_FREIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_3_DAY_FREIGHT;
                }
                else if (inputShipment[23] == "FEDEX_EXPRESS_SAVER")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_EXPRESS_SAVER;
                }
                else if (inputShipment[23] == "FEDEX_GROUND")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FEDEX_GROUND;
                }
                else if (inputShipment[23] == "FIRST_OVERNIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.FIRST_OVERNIGHT;
                }
                else if (inputShipment[23] == "GROUND_HOME_DELIVERY")
                {
                    request.RequestedShipment.ServiceType = ServiceType.GROUND_HOME_DELIVERY;
                }
                else if (inputShipment[23] == "INTERNATIONAL_ECONOMY")
                {
                    request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_ECONOMY;
                }
                else if (inputShipment[23] == "INTERNATIONAL_ECONOMY_FREIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                }
                else if (inputShipment[23] == "INTERNATIONAL_FIRST")
                {
                    request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_FIRST;
                }
                else if (inputShipment[23] == "INTERNATIONAL_PRIORITY")
                {
                    request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_PRIORITY;
                }
                else if (inputShipment[23] == "INTERNATIONAL_PRIORITY_FREIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                }
                else if (inputShipment[23] == "PRIORITY_OVERNIGHT")
                {
                    request.RequestedShipment.ServiceType = ServiceType.PRIORITY_OVERNIGHT;
                }
                else
                {
                    request.RequestedShipment.ServiceType = ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                }
                //
                request.RequestedShipment.ShippingChargesPayment = new Payment(); // Payment information
                request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER;
                request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
                request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = inputShipment[2].ToString(); // Replace "XXX" with clients account number
                request.RequestedShipment.ShippingChargesPayment.Payor.CountryCode = inputShipment[7].ToString();
                //
                request.RequestedShipment.ShipTimestamp = DateTime.Now;
                //
                request.RequestedShipment.PackageCount = "1";
                string weightunitsKG = string.Empty;
                string weightunitsLB = string.Empty;
                weightunitsKG = WeightUnits.KG.ToString();
                weightunitsLB = WeightUnits.LB.ToString();
                request.RequestedShipment.RequestedPackages = new RequestedPackage[1] { new RequestedPackage() };
                request.RequestedShipment.RequestedPackages[0].Weight = new Weight(); // Package weight information
                request.RequestedShipment.TotalWeight =new Weight();
                request.RequestedShipment.TotalWeight.Value = Convert.ToInt32(inputShipment[14].ToString());
                if (inputShipment[15].ToString().ToUpper() == weightunitsKG)
                {
                    request.RequestedShipment.TotalWeight.Units = WeightUnits.KG;
                }
                else
                {
                    request.RequestedShipment.TotalWeight.Units = WeightUnits.LB;
                }
                request.RequestedShipment.RequestedPackages[0].Dimensions =new Dimensions();
                request.RequestedShipment.RequestedPackages[0].Dimensions.Length = inputShipment[18].ToString();
                request.RequestedShipment.RequestedPackages[0].Dimensions.Width = inputShipment[19].ToString();
                request.RequestedShipment.RequestedPackages[0].Dimensions.Height = inputShipment[20].ToString();
                request.RequestedShipment.RequestedPackages[0].Dimensions.Units = LinearUnits.IN;
                request.RequestedShipment.TotalInsuredValue =new Money();
                //request.RequestedShipment.TotalInsuredValue.Amount = Convert.ToDecimal(inputShipment[27]);
                request.RequestedShipment.TotalInsuredValue.Currency = "USD";
                //
                Boolean bCodShipment = false; // Set this to true to process a COD shipment and COD return Label
                //
                if (bCodShipment)
                {
                    request.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
                    request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1];
                    request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.COD;
                    request.RequestedShipment.SpecialServicesRequested.CodDetail = new CodDetail();
                    request.RequestedShipment.SpecialServicesRequested.CodDetail.CollectionType = CodCollectionType.GUARANTEED_FUNDS;
                    request.RequestedShipment.SpecialServicesRequested.CodCollectionAmount = new Money();
                    request.RequestedShipment.SpecialServicesRequested.CodCollectionAmount.Currency = "USD";
                    request.RequestedShipment.SpecialServicesRequested.CodCollectionAmount.Amount = 250.00M;
                    request.RequestedShipment.RequestedPackages[0].SpecialServicesRequested.CodCollectionAmount = new Money();
                    request.RequestedShipment.RequestedPackages[0].SpecialServicesRequested.CodCollectionAmount.Currency = "USD";
                    request.RequestedShipment.RequestedPackages[0].SpecialServicesRequested.CodCollectionAmount.Amount = 250.00M;
                }
                //
                request.RequestedShipment.LabelSpecification = new LabelSpecification(); // Label specification
                request.RequestedShipment.LabelSpecification.ImageType = LabelSpecificationImageType.PDF; // Use this line for a PDF label
                //request.RequestedShipment.LabelSpecification.ImageType = LabelSpecificationImageType.ZPLII; // Use this line for a ZPLII thermal printer label
                request.RequestedShipment.LabelSpecification.ImageTypeSpecified = true;
                request.RequestedShipment.LabelSpecification.LabelFormatType = LabelFormatType.COMMON2D;
                request.RequestedShipment.LabelSpecification.LabelFormatTypeSpecified = true;
                //request.RequestedShipment.LabelSpecification.LabelStockType = LabelStockType.STOCK_4X6;
                request.RequestedShipment.LabelSpecification.LabelStockTypeSpecified = true;
                request.RequestedShipment.LabelSpecification.LabelPrintingOrientation = LabelPrintingOrientationType.BOTTOM_EDGE_OF_TEXT_FIRST;
                request.RequestedShipment.LabelSpecification.LabelPrintingOrientationSpecified = true;
                //
                request.RequestedShipment.TotalWeight =new Weight();
                request.RequestedShipment.TotalWeight.Value = Convert.ToDecimal(inputShipment[14].ToString());
                request.RequestedShipment.RequestedPackages[0].Weight =new Weight();
                request.RequestedShipment.RequestedPackages[0].Weight.Value = Convert.ToDecimal(inputShipment[14].ToString());
                request.RequestedShipment.RequestedPackages[0].Weight.Units =new WeightUnits();
                request.RequestedShipment.RequestedPackages[0].Weight.Units = WeightUnits.LB;
                //
                ShipService shipService = new ShipService(); // Initialize the service
                try
                {
                    ProcessShipmentReply reply = shipService.processShipment(request); // This is the call to the ship web service passing in a request object and returning a reply object
                    if ((reply.HighestSeverity != NotificationSeverityType.ERROR) && (reply.HighestSeverity != NotificationSeverityType.FAILURE) && (reply.HighestSeverity != NotificationSeverityType.WARNING)) // check if the call was successful
                    {
                        DataSet ds =new DataSet();
                        DataTable dt =new DataTable();
                        //
                        dt.Columns.Add(new DataColumn("S.No", System.Type.GetType("System.Int32")));
                        dt.Columns.Add(new DataColumn("Tracking ID", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Form ID", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Billing Weight Value", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Billing Weight Units", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Base charge Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Base charge Currency", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Net charge Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Net charge Currency", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Surcharge Type", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Surcharge Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Surcharge Currency", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Total Surcharge Amount", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Total Surcharge Currency", System.Type.GetType("System.String")));
                        //
                        dt.Columns.Add(new DataColumn("Astra barcode", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("2D barcode", System.Type.GetType("System.String")));
                        //
                        dt.Columns.Add(new DataColumn("URSA Prefix Code", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("URSA Suffix Code", System.Type.GetType("System.String")));
                        //
                        dt.Columns.Add(new DataColumn("Service Commitment ID", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Service Airport ID", System.Type.GetType("System.String")));
                        //
                        dt.Columns.Add(new DataColumn("Delivery day", System.Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("Delivery date", System.Type.GetType("System.String")));
                        //
                        dt.Columns["S.No"].AutoIncrement = true;
                        dt.Columns["S.No"].AutoIncrementStep = 1;
                        //
                        DataRow dr = dt.NewRow();
                        foreach (CompletedPackageDetail packageDetail in reply.CompletedShipmentDetail.CompletedPackageDetails) // Package details / Rating information for each package
                        {
                            dr["Tracking ID"] = packageDetail.TrackingId.TrackingNumber;
                            dr["Form ID"] = packageDetail.TrackingId.FormId;
                            try
                            {
                                foreach (PackageRateDetail ratedPackage in packageDetail.PackageRating.PackageRateDetails)
                                {
                                    if (ratedPackage.BillingWeight != null)
                                    {
                                        dr["Billing Weight Units"] = ratedPackage.BillingWeight.Units;
                                        dr["Billing Weight Value"] = ratedPackage.BillingWeight.Value;
                                    }
                                    if (ratedPackage.BaseCharge != null)
                                    {
                                        dr["Base charge Amount"] = ratedPackage.BaseCharge.Amount;
                                        dr["Base charge Currency"] = ratedPackage.BaseCharge.Currency;
                                    }
                                    if (ratedPackage.NetCharge != null)
                                    {
                                        dr["Net charge Amount"] = ratedPackage.NetCharge.Amount;
                                        dr["Net charge Currency"] = ratedPackage.NetCharge.Currency;
                                    }
                                    if (ratedPackage.Surcharges != null)
                                    {
                                        foreach (Surcharge surcharge in ratedPackage.Surcharges) // Individual surcharge for each package
                                        {
                                            dr["Surcharge Type"] = surcharge.SurchargeType;
                                            dr["Surcharge Amount"] = surcharge.Amount.Amount;
                                            dr["Surcharge Currency"] = surcharge.Amount.Currency;
                                        }
                                    }
                                    if (ratedPackage.TotalSurcharges != null)
                                    {
                                        dr["Total Surcharge Amount"] = ratedPackage.TotalSurcharges.Amount;
                                        dr["Total Surcharge Currency"] = ratedPackage.TotalSurcharges.Currency;
                                    }
                                }
                            }
                            catch (Exception exratedPackage)
                            {
                                string address = string.Empty;
                                address = exratedPackage.Source.ToString();
                            }
                            //
                            dr["Astra barcode"] = packageDetail.Barcodes.AstraBarcode;
                            dr["2D barcode"] = packageDetail.Barcodes.Common2DBarcode;
                            //
                            if (null != packageDetail.Label.Parts[0].Image)
                            {
                                //Save label buffer to file
                                string LabelFileName = "c:\\" + packageDetail.TrackingId.TrackingNumber + ".pdf";
                                FileStream LabelFile = new FileStream(LabelFileName, FileMode.Create);
                                LabelFile.Write(packageDetail.Label.Parts[0].Image, 0, packageDetail.Label.Parts[0].Image.Length);
                                LabelFile.Close();

                                // Display label in Acrobat
                                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(LabelFileName.ToString());
                                info.UseShellExecute = true;

                                info.Verb = "open";
                                System.Diagnostics.Process.Start(info);

                                if (bCodShipment)
                                {
                                    //Save label buffer to file
                                    LabelFileName = "c:\\" + reply.CompletedShipmentDetail.CodReturnDetail.CodRoutingDetail.AstraDetails[0].TrackingId.TrackingNumber + "CR" + ".pdf";
                                    LabelFile = new FileStream(LabelFileName, FileMode.Create);
                                    LabelFile.Write(reply.CompletedShipmentDetail.CodReturnDetail.Label.Parts[0].Image, 0, reply.CompletedShipmentDetail.CodReturnDetail.Label.Parts[0].Image.Length);
                                    LabelFile.Close();
                                    // Display label in Acrobat 
                                    info = new System.Diagnostics.ProcessStartInfo(LabelFileName.ToString());
                                    info.UseShellExecute = true;
                                    info.Verb = "open";
                                    System.Diagnostics.Process.Start(info);
                                }
                            }
                        }
                        dr["URSA Prefix Code"] = reply.CompletedShipmentDetail.RoutingDetail.UrsaPrefixCode;
                        dr["URSA Suffix Code"] = reply.CompletedShipmentDetail.RoutingDetail.UrsaSuffixCode;
                        //
                        dr["Service Commitment ID"] = reply.CompletedShipmentDetail.RoutingDetail.DestinationLocationId;
                        dr["Service Airport ID"] = reply.CompletedShipmentDetail.RoutingDetail.AirportId;
                        //
                        dr["Delivery day"] = reply.CompletedShipmentDetail.RoutingDetail.DeliveryDay;
                        dr["Delivery date"] = reply.CompletedShipmentDetail.RoutingDetail.DeliveryDate;
                        //
                        dt.Rows.Add(dr);
                        ds.Tables.Add(dt);
                        dsnew = ds;
                    }
                    else
                    {
                        DataSet dsexeceptions =new DataSet();
                        DataSet ds =new DataSet();
                        DataTable dt =new DataTable();
                        try
                        {
                            dt.Columns.Add(new DataColumn("S.No", System.Type.GetType("System.Int32")));
                            dt.Columns.Add(new DataColumn("Exception_Name", System.Type.GetType("System.String")));
                            dt.Columns.Add(new DataColumn("Exception_Source", System.Type.GetType("System.String")));
                            //
                            dt.Columns["S.No"].AutoIncrement = true;
                            dt.Columns["S.No"].AutoIncrementStep = 1;
                            //
                            DataRow dr = dt.NewRow();
                            dr["Exception_Name"] = reply.Notifications[0].Message;
                            dr["Exception_Source"] = reply.Notifications[0].Source;
                            //
                            dt.Rows.Add(dr);
                            ds.Tables.Add(dt);
                            dsnew = ds;
                        }
                        catch (Exception ex12)
                        {
                            throw;
                        }
                    }
                }
                catch (SoapException e)
                {
                    Console.WriteLine(e.Detail.InnerText);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                string ex1 = string.Empty;
                ex1 = ex.Source.ToString();
            }
            return dsnew;
        }
    }
}
