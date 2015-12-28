using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using LAjitDev.Fedex_Track_Services;

namespace LAjitDev.Shipment
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]

    public class FedTrackService_Local : System.Web.Services.WebService
    {
        Fedex_Track_Services.TrackRequest request =new Fedex_Track_Services.TrackRequest();

        public DataSet GetTrackingDetails(string[] inputarray)
        {
            DataSet dsTrackDetails =new DataSet();
            try
            {
                request.WebAuthenticationDetail = new WebAuthenticationDetail();
                request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
                request.WebAuthenticationDetail.UserCredential.Key = inputarray[0].ToString();
                request.WebAuthenticationDetail.UserCredential.Password = inputarray[1].ToString();
                //
                request.ClientDetail = new ClientDetail();
                request.ClientDetail.AccountNumber = inputarray[2].ToString();
                request.ClientDetail.MeterNumber = inputarray[3].ToString();
                //
                request.TransactionDetail = new TransactionDetail();
                request.TransactionDetail.CustomerTransactionId = "***Tracking v2 Request using VC#***";
                //
                request.Version = new VersionId();
                //request.TrackingNumberUniqueIdentifier = inputarray[4].ToString();
                request.ShipmentAccountNumber = inputarray[2].ToString();
                //
                request.PackageIdentifier = new TrackPackageIdentifier();
                request.PackageIdentifier.Value = inputarray[4];
                request.PackageIdentifier.Type = TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG;
                ////
                request.ShipDateRangeBegin = DateTime.Now;
                request.ShipDateRangeEnd = DateTime.Now.AddDays(2);
                //
                request.Destination = new Address();
                request.Destination.City = inputarray[5].ToString();
                request.Destination.StateOrProvinceCode = inputarray[6].ToString();
                request.Destination.PostalCode = inputarray[7].ToString();
                request.Destination.CountryCode = inputarray[8].ToString();
                //
                TrackService trackService = new TrackService();
                //
                try
                {
                    TrackReply reply = trackService.track(request);
                    if (reply.HighestSeverity == NotificationSeverityType.SUCCESS)
                    {
                        Console.WriteLine("Trackinbg details\n");
                        foreach (TrackDetail trackDetail in reply.TrackDetails)
                        {
                            //Console.WriteLine("Package # {} - Package count {1}", trackDetail.PackageSequenceNumber.ToString().Trim(), trackDetail.PackageCount.ToString());
                            Console.WriteLine("Package # {} " + trackDetail.PackageCount);
                            Console.WriteLine("Tracking number {0}", trackDetail.TrackingNumber);
                            Console.WriteLine("Tracking number unique identifier {0}", trackDetail.TrackingNumberUniqueIdentifier);
                            Console.WriteLine("Status {0} description {1}", trackDetail.StatusCode, trackDetail.StatusDescription);
                            if (trackDetail.OtherIdentifiers != null)
                            {
                                Console.WriteLine("Other identifiers\n");
                                foreach (TrackPackageIdentifier identifier in trackDetail.OtherIdentifiers)
                                {
                                    Console.WriteLine("{0} {1}", identifier.Type, identifier.Value);
                                }
                            }
                            Console.WriteLine("Packaging {0}", trackDetail.Packaging);
                            Console.WriteLine("Package weight {0} {1}", trackDetail.PackageWeight.Value, trackDetail.PackageWeight.Units);
                            Console.WriteLine("Shipment weight {0} {1}", trackDetail.ShipmentWeight.Value, trackDetail.ShipmentWeight.Units);
                            Console.WriteLine("Packaging {0}", trackDetail.Packaging);
                            Console.WriteLine("Ship date & time", trackDetail.ShipTimestamp);
                            Console.WriteLine("Destination {0}, {1}, {2}", trackDetail.DestinationAddress.City, trackDetail.DestinationAddress.PostalCode, trackDetail.DestinationAddress.CountryCode);
                        }
                    }
                    else
                    {
                        Console.WriteLine(reply.Notifications[0].Message);
                    }
                }
                catch (SoapException e)
                {
                    Console.WriteLine(e.Detail.InnerText);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception exs)
            {
                string exceptions = string.Empty;
                exceptions = exs.Source.ToString();
            }
            return dsTrackDetails;
        }
    }
}
