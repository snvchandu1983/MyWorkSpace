<%@ Page Language="C#" MasterPageFile="~/MasterPages/TopLeft.Master" AutoEventWireup="true"
    Codebehind="Ship.aspx.cs" Inherits="LAjitDev.Shipment.Ship" Theme="LAjit" ValidateRequest="false"
    Title="Shipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
       <%-- <script type="text/javascript" src="../JavaScript/Common.js">
        </script>--%>
    <script type="text/javascript" language="javascript">
                
        window.onload = function() 
        {
          //document.getElementById('tblContents').style.width="920px"; 
           Validate('ctl00_cphPageContents_Td11','Get Rates','Tabs');
            HideParentProgress(); 
            
//           if(document.getElementById('ctl00_cphPageContents_hidGridValues').value=='' && document.getElementById('ctl00_cphPageContents_hidLabelValues').value=='') 
//           {
//              if(document.getElementById('ctl00_cphPageContents_Td11').style.color.toUpperCase()== 'BLACK')
//              {
//                 Validate('ctl00_cphPageContents_Td11','Get Rates','Tabs');
//               }
//            }
        }
        
         function Validate(ids,titles,types)
         {
            if(types=='Tabs')
            {
	             document.getElementById('ctl00_cphPageContents_hid_IDS').value =ids; 
	             document.getElementById('ctl00_cphPageContents_hid_ID_Values').value =titles;
	             Get_Tabs_Click(document.getElementById('ctl00_cphPageContents_hid_IDS').value,document.getElementById('ctl00_cphPageContents_hid_ID_Values').value);
            }
            else
            {
               if(types=='Radio')
               {
			      document.getElementById('ctl00_cphPageContents_hidRadios').value=ids.id;
				}
            }
            if(document.getElementById('ctl00_cphPageContents_hid_ID_Values').value=='Get Rates')
            {    
                document.getElementById('divTracking').style.display='none';
	            if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radFedex')
				{
					if(document.getElementById('ctl00_cphPageContents_radFedex').checked==true)
					{
					    document.getElementById('divShipperDetails').style.display='none';
					    document.getElementById('divShip').style.display='none';  
					    document.getElementById('divRatesBtn').style.display='block'; 
                        document.getElementById('divRatesGrdBtn').style.display='none';  
                        document.getElementById('divShpBtn').style.display='none'; 
					    //
					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
					    //Credential Information
					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','Credential Key :');
					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','Password :');
					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Account Number :');
					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Meter Number :');
					    //
					    if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
					    {
					       document.getElementById('ctl00_cphPageContents_txtKeys').value='';
					       document.getElementById('ctl00_cphPageContents_txtKeys').value='V6OldVFrNynJaODS';
					     }
					     if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
					     {
					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='sBz5xZe0ZaznE49w5GigZn5xK';
					     }
					    if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
					    {
					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='510087267';
					     } 
					    if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
					    {
					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='1230563';
					     }    
					    // 
		                document.getElementById('divShipperDetails').style.display='none';
		                document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
		                document.getElementById('divDHLShow').style.display= 'none';
		                document.getElementById('divDHLStreet2').style.display= 'none';
		                document.getElementById('divPhoneNoDHLUPS').style.display= 'none';
		                // 
		                document.getElementById('divDHLReceiver').style.display='none';
		                document.getElementById('divReceiverStreet2').style.display='none';
		                document.getElementById('divReceiverPhoneNo').style.display='none';
		                //Credential Information
		                document.getElementById('divCredentialInformation').style.display='block';
		                document.getElementById('divFEDEXCredentials').style.display='block';
		                document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
		                //Package Details
		                document.getElementById('divPackageDetails').style.display='block';
		                //FEDEX
		                document.getElementById('divFedexPackagesCount').style.display='block';
		                document.getElementById('divFedexPackagesIdentical').style.display='block';
		                document.getElementById('divFedexPackagingServices').style.display='block';
		                document.getElementById('divFedexPayementType').style.display='block';
		                document.getElementById('divFedexServiceType').style.display='block';
		                document.getElementById('divFedexdropofftype').style.display='block';
		                document.getElementById('divFedexCheck').style.display='block';
		                //UPS
		                document.getElementById('divUpsPackagingType').style.display='none';        
		                document.getElementById('divUPSServiceType').style.display='none';
		                document.getElementById('divUPSPickUp').style.display='none';
		                //DHL
		                document.getElementById('divDHLShipdate').style.display='block';
		                document.getElementById('divDHLAdditonalProtection').style.display='none';
		                document.getElementById('divDHLBillAcctNo').style.display='none';
		                document.getElementById('divDHLBilling').style.display='none';
		                document.getElementById('divDHLCustomerTransTraceID').style.display='none';
		                document.getElementById('divDHLDeclaredValue').style.display='none';
		                document.getElementById('divDHLDemographic').style.display='none';
		                document.getElementById('divDHLServices').style.display='none';
		                document.getElementById('divDHLShipmentImage').style.display='none';
		                document.getElementById('divDHLShipmentType').style.display='none';
		                document.getElementById('divDHLShipReference').style.display='none';
		                document.getElementById('divDHlContentDesc').style.display='none';
		                document.getElementById('divDHLCODValue').style.display='none';
		                document.getElementById('divDHLCODCode').style.display='none';	
						//
						if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
						{
							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
							{
								var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
								if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
								{
								     document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='block';
							         document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							         document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
								}
							}
						}  
					}
				} 
//	            else if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radUPS') 
//				{
//					if(document.getElementById('ctl00_cphPageContents_radUPS').checked==true)
//					{
//					    document.getElementById('divShipperDetails').style.display='none';
//					    document.getElementById('divShip').style.display='none';  
//					    document.getElementById('divRatesBtn').style.display='block'; 
//                        document.getElementById('divRatesGrdBtn').style.display='none';  
//                        document.getElementById('divShpBtn').style.display='none';  
//					    //
//					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
//					    //Credential Information
//					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','License Number :');
//					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','User Name :');
//					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Password :');
//					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Account Number :');
//					    //
//					    if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtKeys').value='';
//					       document.getElementById('ctl00_cphPageContents_txtKeys').value='9C2749BB941D3908';
//					     }
//					     if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
//					     {
//					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
//					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='nblajit';
//					     }
//					    if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
//					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='N9111N9111';
//					     } 
//					    if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
//					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='555EY1';
//					     }    
//					    //
//						document.getElementById('divShipperDetails').style.display='none';
//						// 
//						document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
//						document.getElementById('divDHLShow').style.display= 'block';
//						document.getElementById('divDHLStreet2').style.display= 'block';
//						document.getElementById('divPhoneNoDHLUPS').style.display= 'block';
//						document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
//						// 
//						document.getElementById('divDHLReceiver').style.display='block';
//						document.getElementById('divReceiverStreet2').style.display='block';
//						document.getElementById('divReceiverPhoneNo').style.display='block';
//						//Credential Information
//						document.getElementById('divCredentialInformation').style.display='block';
//						document.getElementById('divFEDEXCredentials').style.display='block';
//						//Package Details
//						document.getElementById('divPackageDetails').style.display='block';
//						//UPS
//						document.getElementById('divUpsPackagingType').style.display='block';        
//						document.getElementById('divUPSServiceType').style.display='block';
//						document.getElementById('divUPSPickUp').style.display='block';
//						//FEDEX
//						document.getElementById('divFedexPackagesCount').style.display='none';
//						document.getElementById('divFedexPackagesIdentical').style.display='none';
//						document.getElementById('divFedexPackagingServices').style.display='none';
//						document.getElementById('divFedexPayementType').style.display='none';
//						document.getElementById('divFedexServiceType').style.display='none';
//						document.getElementById('divFedexdropofftype').style.display='none';
//						document.getElementById('divFedexCheck').style.display='none';
//						//DHL
//						document.getElementById('divDHLShipdate').style.display='none';
//						document.getElementById('divDHLAdditonalProtection').style.display='none';
//						document.getElementById('divDHLBillAcctNo').style.display='none';
//						document.getElementById('divDHLBilling').style.display='none';
//						document.getElementById('divDHLCustomerTransTraceID').style.display='none';
//						document.getElementById('divDHLDeclaredValue').style.display='none';
//						document.getElementById('divDHLDemographic').style.display='none';
//						document.getElementById('divDHLServices').style.display='none';
//						document.getElementById('divDHLShipmentImage').style.display='none';
//						document.getElementById('divDHLShipmentType').style.display='none';
//						document.getElementById('divDHLShipReference').style.display='none';
//						document.getElementById('divDHlContentDesc').style.display='none';
//						document.getElementById('divDHLCODValue').style.display='none';
//						document.getElementById('divDHLCODCode').style.display='none';
//						//
//						if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
//						{
//							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
//							{
//								var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
//								if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
//								{
//								 document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
//							     document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='block';
//							     document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
//								} 
//							}
//						}
//					}
//				}
				else
				{
					if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radDHL')
					{
						if(document.getElementById('ctl00_cphPageContents_radDHL').checked==true)
						{
						    document.getElementById('divShipperDetails').style.display='none';
					        document.getElementById('divShip').style.display='none';  
					        document.getElementById('divRatesBtn').style.display='block'; 
                            document.getElementById('divRatesGrdBtn').style.display='none';  
                            document.getElementById('divShpBtn').style.display='none'; 
						    // 
						    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
						    //Credential Information
					        document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','Requestor ID :');
					        document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','Requestor Password :');
					        document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Shipping Key  :');
					        document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Customer Account No :');
					        //
					         if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
					        {
					           document.getElementById('ctl00_cphPageContents_txtKeys').value='';
					           document.getElementById('ctl00_cphPageContents_txtKeys').value='CAPS_3003';
					         }
					         if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
					         {
					           document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
					           document.getElementById('ctl00_cphPageContents_txtPasswords').value='56LD9M9GKH';
					         }
					        if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
					        {
					           document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
					           document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='52233F2B2C485347425C525C515B3050444D5543475D57';
					         } 
					        if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
					        {
					           document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
					           document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='165721049';
					         } 
					        // 
							document.getElementById('divShipperDetails').style.display='none';
							// 
							document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
							document.getElementById('divDHLShow').style.display= 'block';
							document.getElementById('divDHLStreet2').style.display= 'block';
							document.getElementById('divPhoneNoDHLUPS').style.display= 'block';
							// 
							document.getElementById('divDHLReceiver').style.display='block';
							document.getElementById('divReceiverStreet2').style.display='block';
							document.getElementById('divReceiverPhoneNo').style.display='block';
							//Credential Information
							document.getElementById('divCredentialInformation').style.display='block';
							document.getElementById('divFEDEXCredentials').style.display='block';
							//Package Details
							document.getElementById('divPackageDetails').style.display='block';
							//DHL
							document.getElementById('divDHLShipdate').style.display='block';
							document.getElementById('divDHLAdditonalProtection').style.display='block';
							document.getElementById('divDHLBillAcctNo').style.display='block';
							document.getElementById('divDHLBilling').style.display='block';
							document.getElementById('divDHLCustomerTransTraceID').style.display='block';
							document.getElementById('divDHLDeclaredValue').style.display='block';
							document.getElementById('divDHLDemographic').style.display='block';
							document.getElementById('divDHLServices').style.display='block';
							document.getElementById('divDHLShipmentImage').style.display='block';
							document.getElementById('divDHLShipmentType').style.display='block';
							document.getElementById('divDHLShipReference').style.display='block';
							document.getElementById('divDHlContentDesc').style.display='block';
							document.getElementById('divDHLCODValue').style.display='block';
							document.getElementById('divDHLCODCode').style.display='block';
							document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
							//FEDEX
							document.getElementById('divFedexPackagesCount').style.display='none';
							document.getElementById('divFedexPackagesIdentical').style.display='none';
							document.getElementById('divFedexPackagingServices').style.display='none';
							document.getElementById('divFedexPayementType').style.display='none';
							document.getElementById('divFedexServiceType').style.display='none';
							document.getElementById('divFedexdropofftype').style.display='none';
							document.getElementById('divFedexCheck').style.display='none';
							//UPS
							document.getElementById('divUpsPackagingType').style.display='none';        
							document.getElementById('divUPSServiceType').style.display='none';
							document.getElementById('divUPSPickUp').style.display='none';
							//
							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
							{
								if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
								{
									var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
								  if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
								  {
								     document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
							         document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							         document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='block';
								    }
								}
							}
						}
					}
				}
	            Get_Tabs_Click(document.getElementById('ctl00_cphPageContents_hid_IDS').value,document.getElementById('ctl00_cphPageContents_hid_ID_Values').value);
	           document.getElementById("divGetRatesTab").style.display="block";
               document.getElementById("divCreateShipmentTab").style.display="none";
               document.getElementById("divTrackShipmentTab").style.display="none"; 
			}
			else if( document.getElementById('ctl00_cphPageContents_hid_ID_Values').value=='Create Shipment')
			{
				if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radFedex')
				{
					if(document.getElementById('ctl00_cphPageContents_radFedex').checked==true)
					{
					     document.getElementById('divTracking').style.display='none';
				        document.getElementById('divRatesBtn').style.display='none';
				        document.getElementById('divRatesGrdBtn').style.display='none';
				        document.getElementById('divShpBtn').style.display='block';
					   //
					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
					    //Credential Information
					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','Credential Key :');
					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','Password :');
					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Account Number :');
					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Meter Number :');
					    //
					    if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
					    {
					       document.getElementById('ctl00_cphPageContents_txtKeys').value='';
					       document.getElementById('ctl00_cphPageContents_txtKeys').value='V6OldVFrNynJaODS';
					     }
					     if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
					     {
					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='sBz5xZe0ZaznE49w5GigZn5xK';
					     }
					    if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
					    {
					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='510087267';
					     } 
					    if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
					    {
					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='1230563';
					     }    
					    // 
					   document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
		             
		               document.getElementById('divShip').style.display='block';
		               document.getElementById('divShipperDetails').style.display='block'; 
		               document.getElementById('divDHLShow').style.display= 'none';
		               document.getElementById('divDHLStreet2').style.display= 'none';
		               document.getElementById('divPhoneNoDHLUPS').style.display= 'none';
		               // 
		               document.getElementById('divDHLReceiver').style.display='none';
		               document.getElementById('divReceiverStreet2').style.display='none';
		               document.getElementById('divReceiverPhoneNo').style.display='none';
		               //Credential Information
		               document.getElementById('divCredentialInformation').style.display='block';
		               document.getElementById('divFEDEXCredentials').style.display='block';
		               //Package Details
		               document.getElementById('divPackageDetails').style.display='block';
		               //FEDEX
		               document.getElementById('divFedexPackagesCount').style.display='block';
		               document.getElementById('divFedexPackagesIdentical').style.display='block';
		               document.getElementById('divFedexPackagingServices').style.display='block';
		               document.getElementById('divFedexPayementType').style.display='block';
		               document.getElementById('divFedexServiceType').style.display='block';
		               document.getElementById('divFedexdropofftype').style.display='block';
		               document.getElementById('divFedexCheck').style.display='block';
		               //UPS
		               document.getElementById('divUpsPackagingType').style.display='none';        
		               document.getElementById('divUPSServiceType').style.display='none';
		               document.getElementById('divUPSPickUp').style.display='none';
		               //DHL
		               document.getElementById('divDHLShipdate').style.display='block';
		               document.getElementById('divDHLAdditonalProtection').style.display='none';
		               document.getElementById('divDHLBillAcctNo').style.display='none';
		               document.getElementById('divDHLBilling').style.display='none';
		               //document.getElementById('divDHLCredentials').style.display='none';
		               document.getElementById('divDHLCustomerTransTraceID').style.display='none';
		               document.getElementById('divDHLDeclaredValue').style.display='none';
		               document.getElementById('divDHLDemographic').style.display='none';
		               document.getElementById('divDHLServices').style.display='none';
		               document.getElementById('divDHLShipmentImage').style.display='none';
		               document.getElementById('divDHLShipmentType').style.display='none';
		               document.getElementById('divDHLShipReference').style.display='none';
		               document.getElementById('divDHlContentDesc').style.display='none';
		               document.getElementById('divDHLCODValue').style.display='none';
		               document.getElementById('divDHLCODCode').style.display='none';
		               document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
		               if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
		               {
		                  if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
		                  {
		                    var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
	                        if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
							{
							 document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='block';
							 document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							 document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
							 }
		                  }
		                }   
					}
				}
//				else if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radUPS') 
//				{
//					if(document.getElementById('ctl00_cphPageContents_radUPS').checked==true)
//					{
//					     document.getElementById('divTracking').style.display='none';
//				        document.getElementById('divRatesBtn').style.display='none';
//				        document.getElementById('divRatesGrdBtn').style.display='none';
//				        document.getElementById('divShpBtn').style.display='block';
//					     //
//					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
//					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
//					    //Credential Information
//					    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','License Number :');
//					    document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','User Name :');
//					    document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Password :');
//					    document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Account Number :');
//					    //
//					    if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtKeys').value='';
//					       document.getElementById('ctl00_cphPageContents_txtKeys').value='9C2749BB941D3908';
//					     }
//					     if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
//					     {
//					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
//					       document.getElementById('ctl00_cphPageContents_txtPasswords').value='nblajit';
//					     }
//					    if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
//					      document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='N9111N9111';
//					     } 
//					    if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
//					    {
//					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
//					       document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='555EY1';
//					     }    
//					    //
//						document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
//		                document.getElementById('divShip').style.display='block';
//		                document.getElementById('divShipperDetails').style.display='block'; 
//		 			    document.getElementById('divDHLShow').style.display= 'block';
//				        document.getElementById('divDHLStreet2').style.display= 'block';
//				        document.getElementById('divPhoneNoDHLUPS').style.display= 'block';
//				        // 
//				        document.getElementById('divDHLReceiver').style.display='block';
//				        document.getElementById('divReceiverStreet2').style.display='block';
//				        document.getElementById('divReceiverPhoneNo').style.display='block';
//				        //Credential Information
//				        document.getElementById('divCredentialInformation').style.display='block';
//				        document.getElementById('divFEDEXCredentials').style.display='block';
//				        //Package Details
//				        document.getElementById('divPackageDetails').style.display='block';
//				        //UPS
//				        document.getElementById('divUpsPackagingType').style.display='block';        
//				        document.getElementById('divUPSServiceType').style.display='block';
//				        document.getElementById('divUPSPickUp').style.display='block';
//				        //FEDEX
//				        document.getElementById('divFedexPackagesCount').style.display='none';
//				        document.getElementById('divFedexPackagesIdentical').style.display='none';
//				        document.getElementById('divFedexPackagingServices').style.display='none';
//				        document.getElementById('divFedexPayementType').style.display='none';
//				        document.getElementById('divFedexServiceType').style.display='none';
//				        document.getElementById('divFedexdropofftype').style.display='none';
//				        document.getElementById('divFedexCheck').style.display='none';
//				        //DHL
//				        document.getElementById('divDHLShipdate').style.display='none';
//				        document.getElementById('divDHLAdditonalProtection').style.display='none';
//				        document.getElementById('divDHLBillAcctNo').style.display='none';
//				        document.getElementById('divDHLBilling').style.display='none';
//				        //document.getElementById('divDHLCredentials').style.display='none';
//				        document.getElementById('divDHLCustomerTransTraceID').style.display='none';
//				        document.getElementById('divDHLDeclaredValue').style.display='none';
//				        document.getElementById('divDHLDemographic').style.display='none';
//				        document.getElementById('divDHLServices').style.display='none';
//				        document.getElementById('divDHLShipmentImage').style.display='none';
//				        document.getElementById('divDHLShipmentType').style.display='none';
//				        document.getElementById('divDHLShipReference').style.display='none';
//				        document.getElementById('divDHlContentDesc').style.display='none';
//				        document.getElementById('divDHLCODValue').style.display='none';
//				        document.getElementById('divDHLCODCode').style.display='none';
//				        document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
//						//
//						if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
//						{
//							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
//							{
//								var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
//								if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
//								{
//								    document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
//							        document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='block';
//							        document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
//                                }
//							}
//						}
//					}
//				}
				else
				{
					if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radDHL')
					{
						if(document.getElementById('ctl00_cphPageContents_radDHL').checked==true)
						{
						   document.getElementById('divTracking').style.display='none';
				           document.getElementById('divRatesBtn').style.display='none';
				           document.getElementById('divRatesGrdBtn').style.display='none';
				           document.getElementById('divShpBtn').style.display='block';
						    // 
						    document.getElementById('ctl00_cphPageContents_lblKey').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML='';
					        document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML='';
						    //Credential Information
					        document.getElementById('ctl00_cphPageContents_lblKey').innerHTML=document.getElementById('ctl00_cphPageContents_lblKey').innerHTML.replace('','Requestor ID :');
					        document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML=document.getElementById('ctl00_cphPageContents_lblPasswords').innerHTML.replace('','Requestor Password :');
					        document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblAccountNos').innerHTML.replace('','Shipping Key  :');
					        document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML=document.getElementById('ctl00_cphPageContents_lblMeterNos').innerHTML.replace('','Customer Account No :');
					        //
					         if(document.getElementById('ctl00_cphPageContents_txtKeys').id=='ctl00_cphPageContents_txtKeys')
					        {
					           document.getElementById('ctl00_cphPageContents_txtKeys').value='';
					           document.getElementById('ctl00_cphPageContents_txtKeys').value='CAPS_3003';
					         }
					         if(document.getElementById('ctl00_cphPageContents_txtPasswords').id=='ctl00_cphPageContents_txtPasswords')
					         {
					           document.getElementById('ctl00_cphPageContents_txtPasswords').value='';
					           document.getElementById('ctl00_cphPageContents_txtPasswords').value='56LD9M9GKH';
					         }
					        if(document.getElementById('ctl00_cphPageContents_txtAccountNumbers').id=='ctl00_cphPageContents_txtAccountNumbers')
					        {
					           document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='';
					           document.getElementById('ctl00_cphPageContents_txtAccountNumbers').value='52233F2B2C485347425C525C515B3050444D5543475D57';
					         } 
					        if(document.getElementById('ctl00_cphPageContents_txtMeterNumbers').id=='ctl00_cphPageContents_txtMeterNumbers')
					        {
					           document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='';
					           document.getElementById('ctl00_cphPageContents_txtMeterNumbers').value='165721049';
					         }
					         //
							document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='block';
							document.getElementById('divShip').style.display='block';
							document.getElementById('divShipperDetails').style.display='block'; 
							document.getElementById('divDHLShow').style.display= 'block';
							document.getElementById('divDHLStreet2').style.display= 'block';
							document.getElementById('divPhoneNoDHLUPS').style.display= 'block';
							// 
							document.getElementById('divDHLReceiver').style.display='block';
							document.getElementById('divReceiverStreet2').style.display='block';
							document.getElementById('divReceiverPhoneNo').style.display='block';
							//Credential Information
							document.getElementById('divCredentialInformation').style.display='block';
							document.getElementById('divFEDEXCredentials').style.display='block';
							//Package Details
							document.getElementById('divPackageDetails').style.display='block';
							//DHL
							document.getElementById('divDHLShipdate').style.display='block';
							document.getElementById('divDHLAdditonalProtection').style.display='block';
							document.getElementById('divDHLBillAcctNo').style.display='block';
							document.getElementById('divDHLBilling').style.display='block';
							//document.getElementById('divDHLCredentials').style.display='block';
							document.getElementById('divDHLCustomerTransTraceID').style.display='block';
							document.getElementById('divDHLDeclaredValue').style.display='block';
							document.getElementById('divDHLDemographic').style.display='block';
							document.getElementById('divDHLServices').style.display='block';
							document.getElementById('divDHLShipmentImage').style.display='block';
							document.getElementById('divDHLShipmentType').style.display='block';
							document.getElementById('divDHLShipReference').style.display='block';
							document.getElementById('divDHlContentDesc').style.display='block';
							document.getElementById('divDHLCODValue').style.display='block';
							document.getElementById('divDHLCODCode').style.display='block';
							//FEDEX
							document.getElementById('divFedexPackagesCount').style.display='none';
							document.getElementById('divFedexPackagesIdentical').style.display='none';
							document.getElementById('divFedexPackagingServices').style.display='none';
							document.getElementById('divFedexPayementType').style.display='none';
							document.getElementById('divFedexServiceType').style.display='none';
							document.getElementById('divFedexdropofftype').style.display='none';
							document.getElementById('divFedexCheck').style.display='none';
							//UPS
							document.getElementById('divUpsPackagingType').style.display='none';        
							document.getElementById('divUPSServiceType').style.display='none';
							document.getElementById('divUPSPickUp').style.display='none'; 
							document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
							//
							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
							{
								if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
								{
									var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
									if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') ||(values=='ctl00_cphPageContents_gvDHL'))
								    {
								    document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
							        document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							        document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='block';
								    }
								}
							}
						}
					}
				}
			    Get_Tabs_Click(document.getElementById('ctl00_cphPageContents_hid_IDS').value,document.getElementById('ctl00_cphPageContents_hid_ID_Values').value); 
			   document.getElementById("divGetRatesTab").style.display="none";
               document.getElementById("divCreateShipmentTab").style.display="block";
               document.getElementById("divTrackShipmentTab").style.display="none"; 
			}
            else
            {
               if(document.getElementById('ctl00_cphPageContents_hid_ID_Values').value=='Track Shipment')
               {
				    document.getElementById('divShip').style.display='none';
	                document.getElementById('divShipperDetails').style.display='none';
					if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radFedex')
					{
						if(document.getElementById('ctl00_cphPageContents_radFedex').checked==true)
						{
						   document.getElementById('divTracking').style.display='block';
						   document.getElementById('divShpBtn').style.display='block';
						   //
						   document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='none';
						   document.getElementById('divDHLShow').style.display= 'none';
						   document.getElementById('divDHLStreet2').style.display= 'none';
						   document.getElementById('divPhoneNoDHLUPS').style.display= 'none';
						   // 
						   document.getElementById('divDHLReceiver').style.display='none';
						   document.getElementById('divReceiverStreet2').style.display='none';
						   document.getElementById('divReceiverPhoneNo').style.display='none';
						   //Credential Information
						   document.getElementById('divCredentialInformation').style.display='none';
						   document.getElementById('divFEDEXCredentials').style.display='none';
						   //Package Details
						   document.getElementById('divPackageDetails').style.display='none';
						   //FEDEX
						   document.getElementById('divFedexPackagesCount').style.display='none';
						   document.getElementById('divFedexPackagesIdentical').style.display='none';
						   document.getElementById('divFedexPackagingServices').style.display='none';
						   document.getElementById('divFedexPayementType').style.display='none';
						   document.getElementById('divFedexServiceType').style.display='none';
						   document.getElementById('divFedexdropofftype').style.display='none';
						   document.getElementById('divFedexCheck').style.display='none';
						   //UPS
						   document.getElementById('divUpsPackagingType').style.display='none';        
						   document.getElementById('divUPSServiceType').style.display='none';
						   document.getElementById('divUPSPickUp').style.display='none';
						   //DHL
						   document.getElementById('divDHLShipdate').style.display='none';
						   document.getElementById('divDHLAdditonalProtection').style.display='none';
						   document.getElementById('divDHLBillAcctNo').style.display='none';
						   document.getElementById('divDHLBilling').style.display='none';
						   
						   document.getElementById('divDHLCustomerTransTraceID').style.display='none';
						   document.getElementById('divDHLDeclaredValue').style.display='none';
						   document.getElementById('divDHLDemographic').style.display='none';
						   document.getElementById('divDHLServices').style.display='none';
						   document.getElementById('divDHLShipmentImage').style.display='none';
						   document.getElementById('divDHLShipmentType').style.display='none';
						   document.getElementById('divDHLShipReference').style.display='none';
						   document.getElementById('divDHlContentDesc').style.display='none';
						   document.getElementById('divDHLCODValue').style.display='none';
						   document.getElementById('divDHLCODCode').style.display='none';
						   document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
							//
							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
							{
								if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
								{
									var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
									if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') || (values=='ctl00_cphPageContents_gvDHL'))
								    {
								        document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
							            document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							            document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
								    }
								}
							}
					   }
					}
//					else if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radUPS') 
//					{
//						if(document.getElementById('ctl00_cphPageContents_radUPS').checked==true)
//						{
//							 document.getElementById('divTracking').style.display='block';
//							 document.getElementById('divShpBtn').style.display='block';
//			                //
//			                document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='none';
//					        document.getElementById('divDHLShow').style.display= 'none';
//					        document.getElementById('divDHLStreet2').style.display= 'none';
//					        document.getElementById('divPhoneNoDHLUPS').style.display= 'none';
//					        // 
//					        document.getElementById('divDHLReceiver').style.display='block';
//					        document.getElementById('divReceiverStreet2').style.display='none';
//					        document.getElementById('divReceiverPhoneNo').style.display='none';
//					        //Credential Information
//					        document.getElementById('divCredentialInformation').style.display='none';
//					        document.getElementById('divFEDEXCredentials').style.display='none';
//					        //Package Details
//					        document.getElementById('divPackageDetails').style.display='none';
//					        //UPS
//					        document.getElementById('divUpsPackagingType').style.display='none';        
//					        document.getElementById('divUPSServiceType').style.display='none';
//					        document.getElementById('divUPSPickUp').style.display='none';
//					        //FEDEX
//					        document.getElementById('divFedexPackagesCount').style.display='none';
//					        document.getElementById('divFedexPackagesIdentical').style.display='none';
//					        document.getElementById('divFedexPackagingServices').style.display='none';
//					        document.getElementById('divFedexPayementType').style.display='none';
//					        document.getElementById('divFedexServiceType').style.display='none';
//					        document.getElementById('divFedexdropofftype').style.display='none';
//					        document.getElementById('divFedexCheck').style.display='none';
//					        //DHL
//					        document.getElementById('divDHLShipdate').style.display='none';
//					        document.getElementById('divDHLAdditonalProtection').style.display='none';
//					        document.getElementById('divDHLBillAcctNo').style.display='none';
//					        document.getElementById('divDHLBilling').style.display='none';
//					        document.getElementById('divDHLCustomerTransTraceID').style.display='none';
//					        document.getElementById('divDHLDeclaredValue').style.display='none';
//					        document.getElementById('divDHLDemographic').style.display='none';
//					        document.getElementById('divDHLServices').style.display='none';
//					        document.getElementById('divDHLShipmentImage').style.display='none';
//					        document.getElementById('divDHLShipmentType').style.display='none';
//					        document.getElementById('divDHLShipReference').style.display='none';
//					        document.getElementById('divDHlContentDesc').style.display='none';
//					        document.getElementById('divDHLCODValue').style.display='none';
//					        document.getElementById('divDHLCODCode').style.display='none';
//					       document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none'; 
//							//
//							if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
//							{
//								if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
//								{
//									var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
//									if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') || (values=='ctl00_cphPageContents_gvDHL'))
//								    {
//								     document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
//							         document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
//							         document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none';
//								    }
//								}
//							}
//						}				
//					}	
					else
					{
						if(document.getElementById('ctl00_cphPageContents_hidRadios').value=='ctl00_cphPageContents_radDHL')
						{
							if(document.getElementById('ctl00_cphPageContents_radDHL').checked==true)
							{
								document.getElementById('divTracking').style.display='block';
							    document.getElementById('divShpBtn').style.display='block';
							    //
				                document.getElementById('ctl00_cphPageContents_divShipDetails').style.display='none';
						        document.getElementById('divDHLShow').style.display= 'none';
						        document.getElementById('divDHLStreet2').style.display= 'none';
						        document.getElementById('divPhoneNoDHLUPS').style.display= 'none';
						        // 
						        document.getElementById('divDHLReceiver').style.display='block';
						        document.getElementById('divReceiverStreet2').style.display='none';
						        document.getElementById('divReceiverPhoneNo').style.display='none';
						        //Credential Information
						        document.getElementById('divCredentialInformation').style.display='none';
						        document.getElementById('divFEDEXCredentials').style.display='none';
						        //Package Details
						        document.getElementById('divPackageDetails').style.display='none';
						        //DHL
						        document.getElementById('divDHLShipdate').style.display='none';
						        document.getElementById('divDHLAdditonalProtection').style.display='none';
						        document.getElementById('divDHLBillAcctNo').style.display='none';
						        document.getElementById('divDHLBilling').style.display='none';
						        document.getElementById('divDHLCustomerTransTraceID').style.display='none';
						        document.getElementById('divDHLDeclaredValue').style.display='none';
						        document.getElementById('divDHLDemographic').style.display='none';
						        document.getElementById('divDHLServices').style.display='none';
						        document.getElementById('divDHLShipmentImage').style.display='none';
						        document.getElementById('divDHLShipmentType').style.display='none';
						        document.getElementById('divDHLShipReference').style.display='none';
						        document.getElementById('divDHlContentDesc').style.display='none';
						        document.getElementById('divDHLCODValue').style.display='none';
						        document.getElementById('divDHLCODCode').style.display='none';
						        //FEDEX
						        document.getElementById('divFedexPackagesCount').style.display='none';
						        document.getElementById('divFedexPackagesIdentical').style.display='none';
						        document.getElementById('divFedexPackagingServices').style.display='none';
						        document.getElementById('divFedexPayementType').style.display='none';
						        document.getElementById('divFedexServiceType').style.display='none';
						        document.getElementById('divFedexdropofftype').style.display='none';
						        document.getElementById('divFedexCheck').style.display='none';
						        //UPS
						        document.getElementById('divUpsPackagingType').style.display='none';        
						        document.getElementById('divUPSServiceType').style.display='none';
						        document.getElementById('divUPSPickUp').style.display='none';
						        document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='none';
								//
								if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='' || document.getElementById('ctl00_cphPageContents_hidLabelValues').value!='')
								{
									if(document.getElementById('ctl00_cphPageContents_hidGridValues').value!='')
									{
										var values=document.getElementById('ctl00_cphPageContents_hidGridValues').value; 
									    if((values=='ctl00_cphPageContents_gvFEDEX') || (values=='ctl00_cphPageContents_gvUPS') || (values=='ctl00_cphPageContents_gvDHL'))
								        {
								           document.getElementById('ctl00_cphPageContents_divGrdFedex').style.display='none';
							               document.getElementById('ctl00_cphPageContents_divGrdUPS').style.display='none';
							               document.getElementById('ctl00_cphPageContents_divGrdDHL').style.display='none'; 
								        }
									}
								}	
							}
						}                 
					}
					Get_Tabs_Click(document.getElementById('ctl00_cphPageContents_hid_IDS').value,document.getElementById('ctl00_cphPageContents_hid_ID_Values').value); 
					document.getElementById("divGetRatesTab").style.display="none";
                    document.getElementById("divCreateShipmentTab").style.display="none";
                    document.getElementById("divTrackShipmentTab").style.display="block";
				}
			}
        }
       
        function GridCalling(Valuetypes,types)
        {
           if(types=='Grid')
           {
              document.getElementById('ctl00_cphPageContents_hidGridValues').value=Valuetypes;
           }
           else 
           {
                 if(types=='Label')
                {
                  document.getElementById('ctl00_cphPageContents_divLblExceptions').style.display='block';
                } 
           }
         } 
         
         function Get_All_Values(clientsids,innerhtmls,types,grdlblids,grdlbltypes)
         {
           var ids=clientsids;
           var htmls=innerhtmls;
           var types=types;
           var grdlblidss=grdlblids;
           var grdlbltypess=grdlbltypes;
           Validate(ids,htmls,types);
           GridCalling(grdlblidss,grdlbltypess);
           if(document.getElementById('divRatesBtn').style.display=='block')
           {
            document.getElementById('divRatesBtn').style.display='none';
            document.getElementById('divRatesGrdBtn').style.display='block';
           }
         }   
         
     function Get_Tabs_Click(id,title)
     {
          if(title=='Get Rates')
          {
            document.getElementById('divShippingProviders').style.display='block';
          }
          else 
          {
              if(title=='Create Shipment')
              {
                   document.getElementById('divShippingProviders').style.display='block';
              }
              else
              {
                   document.getElementById('divShippingProviders').style.display='block';
               } 
           }
      }
        
	    function ButtonNames(id)
	    {
           document.getElementById('ctl00_cphPageContents_hidButtonValues').value=id.id; 	 
	       if(document.getElementById('ctl00_cphPageContents_hidButtonValues').value=='ctl00_cphPageContents_btnShipboth')
	       {
	         document.getElementById('ctl00_cphPageContents_hid_ID_Values').value=''; 
	         document.getElementById('ctl00_cphPageContents_hid_ID_Values').value='Create Shipment';
	       }
	       return true;
	    }
	
	    function CheckShipmentNames()
	    {
	           var myindex  = document.getElementById('ctl00_cphPageContents_ddlFedexServiceType').selectedIndex;
               var SelValue = document.getElementById('ctl00_cphPageContents_ddlFedexServiceType').options[myindex].value;
               if(SelValue=='FEDEX_EXPRESS_SAVER' || SelValue=='FEDEX_GROUND')
               {
                 document.getElementById('divDeclaredValue').style.display='block';
                }
                else
                {
                   document.getElementById('divDeclaredValue').style.display='none';
                }
           } 
           
             function CheckDHLValues()
             {
               var myindex1=document.getElementById('ctl00_cphPageContents_ddlDHLAdditionalProtection').selectedIndex;
               var SelValue1=document.getElementById('ctl00_cphPageContents_ddlDHLAdditionalProtection').options[myindex1].value;
               if(SelValue1=='AP')
               {
                  document.getElementById('divDHLDeclaredValue').style.display='block';
               }
               else
               {
                 document.getElementById('divDHLDeclaredValue').style.display='none';
               }
             }  
         
             function CheckDHLBillAcctNoValues()
             {
               var myindex2=document.getElementById('ctl00_cphPageContents_ddlDHLBilling').selectedIndex;
               var SelValue2=document.getElementById('ctl00_cphPageContents_ddlDHLBilling').options[myindex2].value;
               if(SelValue2=='R')
               {
                document.getElementById('divDHLBillAcctNo').style.display='block';
               }
               else
               {
                   if(SelValue2=='S')
                   {
                     document.getElementById('divDHLBillAcctNo').style.display='none';
                   }
                   else
                   {
                       if(SelValue2=='3')
                       {
                        document.getElementById('divDHLBillAcctNo').style.display='block';
                       }
                   }
               }
             }
            
           
    </script>
    
    
    <script type="text/javascript">
	jQuery(function() {
		jQuery("#ctl00_cphPageContents_txtDHLDate").datepicker({dateFormat:'mm/dd/y',changeMonth: true,changeYear: true});
		jQuery("#ctl00_cphPageContents_txtDHLDate").mask('99/99/99');
	});
	
	
	jQuery(document).ready(function()
    {  
       //Exploded View
        jQuery('#ctl00_cphPageContents_pnlShipperDetails').click(
        function()
        {
            ExpandCollapse(this.id);
        }); 
        //Central Panel1  
        jQuery('#ctl00_cphPageContents_pnlCredentialInformation').click(
        function()
        {
            ExpandCollapse(this.id);
        });
        //Central Panel 2 
        jQuery('#ctl00_cphPageContents_pnlShipFrom').click(
        function()
        {
            ExpandCollapse(this.id);
        });
        //Graphics Panel
        jQuery('#ctl00_cphPageContents_pnlShipTo').click(
        function()
        {
            ExpandCollapse(this.id);
        });
        //Right Panel1
        jQuery('#ctl00_cphPageContents_pnlPackageDetails').click(
        function()
        {
            ExpandCollapse(this.id);
        });
        //Right Panel1
        jQuery('#ctl00_cphPageContents_pnlTrackingDetails').click(
        function()
        {
            ExpandCollapse(this.id);
        });
        
        
    });

    function ExpandCollapse(oTitleID)
    {
       var oImgID; 
       var TitleID=oTitleID.split('_');
       var oContentID='ctl00_cphPageContents_'+TitleID[2]+'Content';
       var i=TitleID[2].indexOf('pnl');
       if(i!==-1)
       {
        i=i+3;
        var j=TitleID[2].length;
        oImgID='img'+TitleID[2].substring(i,j)+'Expand';
       }
       jQuery('#'+oContentID).slideToggle('slow');
       var imgIcon=document.getElementById(oImgID).getAttribute('src'); 
       var imgIconSrc=imgIcon.split('/');
       if(imgIconSrc[4]=="plus-icon.png")
       {
           jQuery("#"+oImgID).attr("src", g_cdnImagesPath;+"Images/minus-icon.png"); 
       }
       else
       {
           jQuery("#"+oImgID).attr("src", g_cdnImagesPath+"Images/plus-icon.png"); 
       }
    }  
	
	
	
	</script>

    
<asp:Panel  runat="server" ID="pnlTotalTable">
    <table style="height: 100%;" width="100%" cellpadding="0" cellspacing="0" class="blueboldlinks01" border="0" id="tblContents">
        <!-- tab headings start -->
        <tr style="height: 21px;">
            <td valign="bottom" style="height: 21px;">
            <div id="divGetRatesTab" style="display:block;">
                                                            <table width="350px" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0" border="0">
                                                                 <tr align="left" valign="bottom" >
                                                                    <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width:90px; background-color:#c5d5fc;">Get Rates</td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:120px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td16','Create Shipment','Tabs')" class="blueboldlinks01">Create Shipment</a></td>
                                                                    <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:130px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td13','Track Shipment','Tabs')" class="blueboldlinks01">Track Shipment</a></td>
                                                                    <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                   </tr>
                                                             </table>   
                                                        </div>
            <div id="divCreateShipmentTab" style="display:none">
                                                           <table width="350px" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                              <tr align="left" valign="bottom">
                                                                <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                <td align="center" valign="middle" style="width:90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td11','Get Rates','Tabs')" class="blueboldlinks01">Get Rates</a></td>
                                                                <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                <td align="center" valign="middle" class="bluebold" style="width:120px; background-color:#c5d5fc; ">Create Shipment</td>
                                                                <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                                <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                <td align="center" valign="middle" style="width:130px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td13','Track Shipment','Tabs')" class="blueboldlinks01">Track Shipment</a></td>
                                                                <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                <td style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                              </tr>
                                                            </table>
                                                         </div>
            <div id="divTrackShipmentTab" style="display:none">
                                                             <table width="350px" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                                  <tr align="left" valign="bottom">
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td11','Get Rates','Tabs')" class="blueboldlinks01">Get Rates</a></td>
                                                                    <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:120px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; "><a href="javascript:Validate('ctl00_cphPageContents_Td16','Create Shipment','Tabs')" class="blueboldlinks01">Create Shipment</a></td>
                                                                    <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width:130px; background-color:#c5d5fc; ">Track Shipment</td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                  </tr>
                                                                </table>
                                                         </div>
        <%--<table width="350px" border="0" bgcolor="#ffffff" bordercolor="black" align="left"
                    cellpadding="0" cellspacing="0" style="cursor: pointer;">
                    <tr align="left" valign="bottom">
                        <td style="width: 5px; background: url(../App_Themes/Lajit/Images/top-midbg01.gif) repeat-x;">
                            <img src="../App_Themes/Lajit/Images/title-left-curve.gif" width="5" height="21"></td>
                        <td valign="middle" class="bluebold"  id="Td11" runat="server" align="center"  onclick="javascript:return Validate('ctl00_cphPageContents_Td11','Get Rates','Tabs')"
                            style="width: 90px; background: url(../App_Themes/Lajit/Images/top-midbg01.gif) repeat-x;
                            color:Black;background-color:#c5d5fc; " title="Get Rates">
                            Get Rates
                        </td>
                        <td align="center" valign="middle" style="width: 5px; background: url(../App_Themes/Lajit/Images/top-midbg01.gif) repeat-x;">
                            <img src="../App_Themes/Lajit/Images/title-right-curve.gif" width="5" height="21"></td>
                        <td align="center" valign="middle" style="width: 3px;">
                            <img src="../App_Themes/Lajit/Images/spacer.gif" width="1" height="1"></td>
                        <td align="center" valign="middle" style="width: 5px;">
                            <img src="../App_Themes/Lajit/Images/top-left01.gif" width="5" height="21"></td>
                        <td id="Td16" runat="server" align="center" class="blueboldlinks01" onclick="javascript:return Validate('ctl00_cphPageContents_Td16','Create Shipment','Tabs')"
                            style="width: 100px; background: url(../App_Themes/Lajit/Images/top-midbg01.gif) repeat-x;;"
                            title="Create Shipment">
                            Create Shipment
                        </td>
                        <td align="center" valign="middle" style="width: 6px;">
                            <img src="../App_Themes/Lajit/Images/top-right01.gif" width="6" height="21"></td>
                        <td align="center" valign="middle" style="width: 3px;">
                            <img src="../App_Themes/Lajit/Images/spacer.gif" width="1" height="1"></td>
                        <td align="center" valign="middle" style="width: 5px;">
                            <img src="../App_Themes/Lajit/Images/top-left01.gif" width="5" height="21"></td>
                        <td id="Td13" runat="server" align="center" class="blueboldlinks01" onclick="javascript:return Validate('ctl00_cphPageContents_Td13','Track Shipment','Tabs')"
                            style="width: 100px; background: url(../App_Themes/Lajit/Images/top-midbg01.gif) repeat-x;;"
                            title="Track Shipment">
                            Track Shipment</td>
                        <td style="width: 6px;">
                            <img src="../App_Themes/Lajit/Images/top-right01.gif" width="6" height="21"></td>
                        <td style="width: 3px;">
                            <img src="../App_Themes/Lajit/Images/spacer.gif" width="1" height="1"></td>
                    </tr>
                </table>--%>
            </td>
        </tr>
        <!-- tab headings end -->
        <tr>
            <td style="height: 16%;">
                <table border="0" cellpadding="0" cellspacing="0" class="formmiddle" style="height: 100%;
                    cursor: pointer; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;
                    border-bottom-style: none" width="100%" runat="server" id="tblTotal" visible="true">
                    <tr>
                        <td colspan="2" style="height: 14px; width: 100%">
                            <div id="divShippingProviders" style="display: block">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100%; width: 100%">
                                    <tr>
                                        <td align="Right" class="mbodyb" style="height: 24px; width: 30%" valign="middle">
                                            Shipping Service Providers :</td>
                                         <td align="left" class="mbodyb" style="height: 24px; width: 70%" valign="top">
                                            <table border="0" style="height: 100%; width: 80%">
                                                <tr>
                                                  <td align="left" class="mbodyb" style="height: 17px; vertical-align: middle">
                                                        <asp:RadioButton ID="radFedex" runat="server" GroupName="Shippers" onclick="javascript:return Validate(this,'Fedex','Radio')" />
                                                    </td>
                                                    <td align="left" class="mbodyb" style="height: 17px;" valign="middle">
                                                        <img id="imgFedex" alt="FEDEX" height="42px" src="<%=Application["ImagesCDNPath"]%>images/FEDEX.GIF"
                                                            width="92px"/></td>
                                                    <td align="left" class="mbodyb" style="height: 62px; vertical-align: middle">
                                                        <asp:RadioButton ID="radDHL" runat="server" GroupName="Shippers" onclick="javascript:return Validate(this,'DHL','Radio')" />
                                                    </td>
                                                    <td align="left" class="mbodyb" valign="middle">
                                                        <img id="imgDHL" alt="DHL" height="39px" src="<%=Application["ImagesCDNPath"]%>images/DHL.gif"
                                                            width="89px" />
                                                    </td>
                                                    <td>
                                                        <input type="hidden" runat="server" id="hidRadios" /></td>
                                                 <%--<td align="left" class="mbodyb" style="height: 17px; vertical-align: middle">
                                                        <asp:RadioButton ID="radUPS" runat="server" GroupName="Shippers" Style="vertical-align: middle;
                                                            height: 22px" onclick="javascript:return Validate(this,'UPS','Radio')" />
                                                    </td>
                                                    <td align="left" class="mbodyb" style="height: 20px;" valign="middle">
                                                        <img id="imgUPS" alt="UPS" height="42px" src="../App_Themes/Lajit/images/LOGO_M.gif"
                                                            width="92px" />
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 100%;" width="100%"
                                runat="server" id="tblGrids">
                                <tr>
                                    <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div runat="server" id="divGrdFedex" style="display: block">
                                                        <table style="height: 100%; width: 100%">
                                                            <tr>
                                                                <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                                                    <asp:GridView runat="server" ID="gvFEDEX" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                        Visible="false" AutoGenerateColumns="true">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <RowStyle BackColor="#EFF3FB" />
                                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#2461BF" />
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                                    <div runat="server" id="divGrdUPS" style="display: block">
                                                        <table>
                                                            <tr>
                                                                <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                                                    <asp:GridView runat="server" ID="gvUPS" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                        Visible="false" AutoGenerateColumns="true">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <RowStyle BackColor="#EFF3FB" />
                                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#2461BF" />
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                                    <div runat="server" id="divGrdDHL" style="display: block">
                                                        <table>
                                                            <tr>
                                                                <td style="height: 100%; width: 100%" colspan="4" valign="top" align="center">
                                                                    <asp:GridView runat="server" ID="gvDHL" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                        Visible="false" AutoGenerateColumns="true">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <RowStyle BackColor="#EFF3FB" />
                                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#2461BF" />
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divShipperDetails" style="display: none">
                                <asp:Panel ID="pnlShipperDetails" runat="server" Width="100%">
                                    <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; cursor: pointer;
                                        border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;"
                                        width="100%">
                                        <tr style="height: 24px;">
                                            <td class="grdVwCurveLeft" style="width: 5px;">
                                            </td>
                                            <td id="htcCPGV1" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                border-width: 0px; width: 100px">
                                                Shipper Details
                                            </td>
                                            <td class="grdVwCurveRight" style="width: 6px;">
                                            </td>
                                            <td id="htcShipperDetails" runat="server" class="grdVwTitleAuto" style="height: 24px;"
                                                width="717px">
                                            </td>
                                            <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                <img id="imgShipperDetailsExpand" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                <tr>
                                    <td>
                                        <div id="divShip" style="display: none">
                                            <asp:Panel ID="pnlShipperDetailsContent" runat="server" Height="100%" Width="100%"
                                                HorizontalAlign="Center">
                                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Shipper Name :</td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtShipperName" runat="server" SkinID="LoginTextBox">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ShipperNameRequired" runat="server" ControlToValidate="txtShipperName"
                                                                ErrorMessage="Shipper Name is required." SetFocusOnError="True" ToolTip="Shipper Name is required.">*</asp:RequiredFieldValidator>
                                                            <LCtrl:LAjitRegularExpressionValidator ID="regtxtShipperName" runat="server" ControlToValidate="txtShipperName" MapXML="ShipperName"
                                                             ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                            Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
    
                                                                </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Shipper Address :
                                                        </td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtShipperAddress" runat="server" SkinID="LoginTextBox">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqShipperAddress" runat="server" ControlToValidate="txtShipperAddress"
                                                                ErrorMessage="Shipper Address is required." SetFocusOnError="True" ToolTip="Shipper Address is required.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Street Lines :
                                                        </td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtStreet" runat="server" SkinID="LoginTextBox">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqStreet" runat="server" ControlToValidate="txtStreet"
                                                                ErrorMessage="Street is required." SetFocusOnError="True" ToolTip="Street is required.">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            City :</td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtCity" runat="server" SkinID="LoginTextBox" >
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqCity" runat="server" ControlToValidate="txtCity"
                                                                ErrorMessage="City is required." SetFocusOnError="True" ToolTip="City is required.">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            State/Provinence:</td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <select id="inptState" runat="server" class="formfields" style="width: 155px;">
                                                                <option value="">-No state/prov-</option>
                                                                <option value="AL">Alabama</option>
                                                                <option value="AB">Alberta (Canada)</option>
                                                                <option value="AK">Alaska</option>
                                                                <option value="AA">AFO - Americas not US/CA</option>
                                                                <option value="AE">AFO - Other Overseas</option>
                                                                <option value="AP">AFO - Pacific and Ind. Ocean</option>
                                                                <option value="AZ">Arizona</option>
                                                                <option value="AR">Arkansas</option>
                                                                <option value="BC">British Columbia (Canada)</option>
                                                                <option selected="selected" value="CA">California</option>
                                                                <option value="CO">Colorado</option>
                                                                <option value="CT">Connecticut</option>
                                                                <option value="DE">Delaware</option>
                                                                <option value="DC">Dist. of Columbia</option>
                                                                <option value="FL">Florida</option>
                                                                <option value="GA">Georgia</option>
                                                                <option value="HI">Hawaii</option>
                                                                <option value="ID">Idaho</option>
                                                                <option value="IL">Illinois</option>
                                                                <option value="IN">Indiana</option>
                                                                <option value="IA">Iowa</option>
                                                                <option value="KS">Kansas</option>
                                                                <option value="KY">Kentucky</option>
                                                                <option value="LA">Louisiana</option>
                                                                <option value="ME">Maine</option>
                                                                <option value="MB">Manitoba (Canada)</option>
                                                                <option value="MD">Maryland</option>
                                                                <option value="MA">Massachusetts</option>
                                                                <option value="MI">Michigan</option>
                                                                <option value="MN">Minnesota</option>
                                                                <option value="MS">Mississippi</option>
                                                                <option value="MO">Missouri</option>
                                                                <option value="MT">Montana</option>
                                                                <option value="NE">Nebraska</option>
                                                                <option value="NV">Nevada</option>
                                                                <option value="NB">New Brunswick (Canada)</option>
                                                                <option value="NL">Newfoundland and Labrador (Canada)</option>
                                                                <option value="NH">New Hampshire</option>
                                                                <option value="NJ">New Jersey</option>
                                                                <option value="NM">New Mexico</option>
                                                                <option value="NY">New York</option>
                                                                <option value="NC">North Carolina</option>
                                                                <option value="ND">North Dakota</option>
                                                                <option value="NS">Nova Scotia (Canada)</option>
                                                                <option value="NT">Northwest Territories (Canada)</option>
                                                                <option value="NU">Nunavut (Canada)</option>
                                                                <option value="OH">Ohio</option>
                                                                <option value="OK">Oklahoma</option>
                                                                <option value="ON">Ontario (Canada)</option>
                                                                <option value="OR">Oregon</option>
                                                                <option value="QC">Qu&#233;bec (Canada)</option>
                                                                <option value="PA">Pennsylvania</option>
                                                                <option value="PE">Prince Edward Island (Canada)</option>
                                                                <option value="RI">Rhode Island</option>
                                                                <option value="SK">Saskatchewan (Canada)</option>
                                                                <option value="SC">South Carolina</option>
                                                                <option value="SD">South Dakota</option>
                                                                <option value="TN">Tennessee</option>
                                                                <option value="TX">Texas</option>
                                                                <option value="UT">Utah</option>
                                                                <option value="VT">Vermont</option>
                                                                <option value="VA">Virginia</option>
                                                                <option value="WA">Washington</option>
                                                                <option value="WV">West Virginia</option>
                                                                <option value="WI">Wisconsin</option>
                                                                <option value="WY">Wyoming</option>
                                                                <option value="YT">Yukon (Canada)</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Postal Code :
                                                        </td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtPostalCode" runat="server" SkinID="LoginTextBox" >
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqShipCode" runat="server" ControlToValidate="txtPostalCode"
                                                                ErrorMessage="Postal Code is required." SetFocusOnError="True" ToolTip="Postal Code is required.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Country :
                                                        </td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <select id="ddlShipCountry" runat="server" class="formfields">
                                                                <option value="AF">Afghanistan</option>
                                                                <option value="AL">Albania</option>
                                                                <option value="DZ">Algeria</option>
                                                                <option value="AS">American Samoa</option>
                                                                <option value="AD">Andorra</option>
                                                                <option value="AO">Angola</option>
                                                                <option value="AI">Anguilla</option>
                                                                <option value="AG">Antigua/Barbuda</option>
                                                                <option value="AR">Argentina</option>
                                                                <option value="AM">Armenia</option>
                                                                <option value="AW">Aruba</option>
                                                                <option value="AU">Australia</option>
                                                                <option value="AT">Austria</option>
                                                                <option value="AZ">Azerbaijan</option>
                                                                <option value="BS">Bahamas</option>
                                                                <option value="BH">Bahrain</option>
                                                                <option value="BD">Bangladesh</option>
                                                                <option value="BB">Barbados</option>
                                                                <option value="BY">Belarus</option>
                                                                <option value="BE">Belgium</option>
                                                                <option value="BZ">Belize</option>
                                                                <option value="BJ">Benin</option>
                                                                <option value="BM">Bermuda</option>
                                                                <option value="BT">Bhutan</option>
                                                                <option value="BO">Bolivia</option>
                                                                <option value="BA">Bosnia-Herzegovina</option>
                                                                <option value="BW">Botswana</option>
                                                                <option value="BR">Brazil</option>
                                                                <option value="VG">British Virgin Islands</option>
                                                                <option value="BN">Brunei</option>
                                                                <option value="BG">Bulgaria</option>
                                                                <option value="BF">Burkina Faso</option>
                                                                <option value="BI">Burundi</option>
                                                                <option value="KH">Cambodia</option>
                                                                <option value="CM">Cameroon</option>
                                                                <option value="CA">Canada</option>
                                                                <option value="CV">Cape Verde</option>
                                                                <option value="KY">Cayman Islands</option>
                                                                <option value="TD">Chad</option>
                                                                <option value="CL">Chile</option>
                                                                <option value="CN">China</option>
                                                                <option value="CO">Colombia</option>
                                                                <option value="CG">Congo Brazzaville</option>
                                                                <option value="CD">Congo Democratic Rep. of</option>
                                                                <option value="CK">Cook Islands</option>
                                                                <option value="CR">Costa Rica</option>
                                                                <option value="HR">Croatia</option>
                                                                <option value="CY">Cyprus</option>
                                                                <option value="CZ">Czech Republic</option>
                                                                <option value="DK">Denmark</option>
                                                                <option value="DJ">Djibouti</option>
                                                                <option value="DM">Dominica</option>
                                                                <option value="DO">Dominican Republic</option>
                                                                <option value="TL">East Timor</option>
                                                                <option value="EC">Ecuador</option>
                                                                <option value="EG">Egypt</option>
                                                                <option value="SV">El Salvador</option>
                                                                <option value="GQ">Equatorial Guinea</option>
                                                                <option value="ER">Eritrea</option>
                                                                <option value="EE">Estonia</option>
                                                                <option value="ET">Ethiopia</option>
                                                                <option value="FO">Faeroe Islands</option>
                                                                <option value="FJ">Fiji</option>
                                                                <option value="FI">Finland</option>
                                                                <option value="FR">France</option>
                                                                <option value="GF">French Guiana</option>
                                                                <option value="PF">French Polynesia</option>
                                                                <option value="GA">Gabon</option>
                                                                <option value="GM">Gambia</option>
                                                                <option value="GE">Georgia</option>
                                                                <option value="DE">Germany</option>
                                                                <option value="GH">Ghana</option>
                                                                <option value="GI">Gibraltar</option>
                                                                <option value="GR">Greece</option>
                                                                <option value="GL">Greenland</option>
                                                                <option value="GD">Grenada</option>
                                                                <option value="GP">Guadeloupe</option>
                                                                <option value="GU">Guam</option>
                                                                <option value="GT">Guatemala</option>
                                                                <option value="GN">Guinea</option>
                                                                <option value="GY">Guyana</option>
                                                                <option value="HT">Haiti</option>
                                                                <option value="HN">Honduras</option>
                                                                <option value="HK">Hong Kong</option>
                                                                <option value="HU">Hungary</option>
                                                                <option value="IS">Iceland</option>
                                                                <option value="IN">India</option>
                                                                <option value="ID">Indonesia</option>
                                                                <option value="IQ">Iraq</option>
                                                                <option value="IE">Ireland</option>
                                                                <option value="IL">Israel</option>
                                                                <option value="IT">Italy</option>
                                                                <option value="CI">Ivory Coast</option>
                                                                <option value="JM">Jamaica</option>
                                                                <option value="JP">Japan</option>
                                                                <option value="JO">Jordan</option>
                                                                <option value="KZ">Kazakhstan</option>
                                                                <option value="KE">Kenya</option>
                                                                <option value="KW">Kuwait</option>
                                                                <option value="KG">Kyrgyzstan</option>
                                                                <option value="LA">Laos</option>
                                                                <option value="LV">Latvia</option>
                                                                <option value="LB">Lebanon</option>
                                                                <option value="LS">Lesotho</option>
                                                                <option value="LR">Liberia</option>
                                                                <option value="LY">Libya</option>
                                                                <option value="LI">Liechtenstein</option>
                                                                <option value="LT">Lithuania</option>
                                                                <option value="LU">Luxembourg</option>
                                                                <option value="MO">Macau</option>
                                                                <option value="MK">Macedonia</option>
                                                                <option value="MG">Madagascar</option>
                                                                <option value="MW">Malawi</option>
                                                                <option value="MY">Malaysia</option>
                                                                <option value="MV">Maldives</option>
                                                                <option value="ML">Mali</option>
                                                                <option value="MT">Malta</option>
                                                                <option value="MH">Marshall Islands</option>
                                                                <option value="MQ">Martinique</option>
                                                                <option value="MR">Mauritania</option>
                                                                <option value="MU">Mauritius</option>
                                                                <option value="MX">Mexico</option>
                                                                <option value="FM">Micronesia</option>
                                                                <option value="MD">Moldova</option>
                                                                <option value="MC">Monaco</option>
                                                                <option value="MN">Mongolia</option>
                                                                <option value="ME">Montenegro</option>
                                                                <option value="MS">Montserrat</option>
                                                                <option value="MA">Morocco</option>
                                                                <option value="MZ">Mozambique</option>
                                                                <option value="NA">Namibia</option>
                                                                <option value="NP">Nepal</option>
                                                                <option value="NL">Netherlands</option>
                                                                <option value="AN">Netherlands Antilles</option>
                                                                <option value="NC">New Caledonia</option>
                                                                <option value="NZ">New Zealand</option>
                                                                <option value="NI">Nicaragua</option>
                                                                <option value="NE">Niger</option>
                                                                <option value="NG">Nigeria</option>
                                                                <option value="NO">Norway</option>
                                                                <option value="OM">Oman</option>
                                                                <option value="PK">Pakistan</option>
                                                                <option value="PW">Palau</option>
                                                                <option value="PS">Palestine Autonomous</option>
                                                                <option value="PA">Panama</option>
                                                                <option value="PG">Papua New Guinea</option>
                                                                <option value="PY">Paraguay</option>
                                                                <option value="PE">Peru</option>
                                                                <option value="PH">Philippines</option>
                                                                <option value="PL">Poland</option>
                                                                <option value="PT">Portugal</option>
                                                                <option value="PR">Puerto Rico</option>
                                                                <option value="QA">Qatar</option>
                                                                <option value="RE">Reunion</option>
                                                                <option value="RO">Romania</option>
                                                                <option value="RU">Russian Federation</option>
                                                                <option value="RW">Rwanda</option>
                                                                <option value="MP">Saipan</option>
                                                                <option value="WS">Samoa</option>
                                                                <option value="SA">Saudi Arabia</option>
                                                                <option value="SN">Senegal</option>
                                                                <option value="RS">Serbia</option>
                                                                <option value="SC">Seychelles</option>
                                                                <option value="SG">Singapore</option>
                                                                <option value="SK">Slovak Republic</option>
                                                                <option value="SI">Slovenia</option>
                                                                <option value="ZA">South Africa</option>
                                                                <option value="KR">South Korea</option>
                                                                <option value="ES">Spain</option>
                                                                <option value="LK">Sri Lanka</option>
                                                                <option value="KN">St. Kitts/Nevis</option>
                                                                <option value="LC">St. Lucia</option>
                                                                <option value="VC">St. Vincent</option>
                                                                <option value="SR">Suriname</option>
                                                                <option value="SZ">Swaziland</option>
                                                                <option value="SE">Sweden</option>
                                                                <option value="CH">Switzerland</option>
                                                                <option value="SY">Syria</option>
                                                                <option value="TW">Taiwan</option>
                                                                <option value="TZ">Tanzania</option>
                                                                <option value="TH">Thailand</option>
                                                                <option value="TG">Togo</option>
                                                                <option value="TO">Tonga</option>
                                                                <option value="TT">Trinidad/Tobago</option>
                                                                <option value="TN">Tunisia</option>
                                                                <option value="TR">Turkey</option>
                                                                <option value="TM">Turkmenistan</option>
                                                                <option value="TC">Turks &amp; Caicos Islands</option>
                                                                <option selected="selected" value="US">U.S.A.</option>
                                                                <option value="VI">U.S. Virgin Islands</option>
                                                                <option value="UG">Uganda</option>
                                                                <option value="UA">Ukraine</option>
                                                                <option value="AE">United Arab Emirates</option>
                                                                <option value="GB">United Kingdom</option>
                                                                <option value="UY">Uruguay</option>
                                                                <option value="UZ">Uzbekistan</option>
                                                                <option value="VU">Vanuatu</option>
                                                                <option value="VE">Venezuela</option>
                                                                <option value="VN">Vietnam</option>
                                                                <option value="WF">Wallis &amp; Futuna</option>
                                                                <option value="YE">Yemen</option>
                                                                <option value="ZM">Zambia</option>
                                                                <option value="ZW">Zimbabwe</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                            Phone Number :
                                                        </td>
                                                        <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                            <asp:TextBox ID="txtPhoneNo" runat="server" SkinID="LoginTextBox" >
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqPhoneNumber" runat="server" ControlToValidate="txtPhoneNo"
                                                                ErrorMessage="Phone Number is required." SetFocusOnError="True" ToolTip="Phone Number is required.">*</asp:RequiredFieldValidator></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="mbodyb" colspan="6">
                                        <div id="divCredentialInformation" style="display: none">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; border-right-width: 0px;
                                                border-right-style: none; border-right-color: #d7e0f1;" width="100%">
                                                <tr>
                                                    <td class="mbodyb" colspan="6">
                                                        <asp:Panel ID="pnlCredentialInformation" runat="server" Width="100%">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; cursor: pointer;
                                                                border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;"
                                                                width="100%">
                                                                <tr style="height: 24px;">
                                                                    <td class="grdVwCurveLeft" style="width: 5px;">
                                                                    </td>
                                                                    <td id="Td5" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                                        border-width: 0px; width: 140px">
                                                                        Credential Information
                                                                    </td>
                                                                    <td class="grdVwCurveRight" style="width: 6px;">
                                                                    </td>
                                                                    <td id="htcCredentialInformation" runat="server" class="grdVwTitleAuto" style="height: 24px;"
                                                                        width="757px">
                                                                    </td>
                                                                    <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                                        <img id="imgCredentialInformationExpand" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="mbodyb">
                                                        <asp:Panel ID="pnlCredentialInformationContent" runat="server" Height="100%" Width="100%"
                                                            HorizontalAlign="Center">
                                                            <div id="divFEDEXCredentials" style="display: none">
                                                                <table style="height: 100%; width: 100%" border="0">
                                                                    <tr>
                                                                        <td align="Right" class="mbodyb" style="width: 40%" valign="middle">
                                                                            <span runat="server" id="lblKey"></span>
                                                                        </td>
                                                                        <td align="left" class="mbodyb" style="height: 17px; width: 60%" valign="top">
                                                                            <input type="text" id="txtKeys" runat="server" skinid="LoginTextBox" value="V6OldVFrNynJaODS" />
                                                                            <asp:RequiredFieldValidator ID="reqCredentialKey" runat="server" ControlToValidate="txtKeys"
                                                                                ErrorMessage="Credential Key is required." SetFocusOnError="True" ToolTip="Credential Key is required.">*</asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="Right" class="mbodyb" style="width: 40%" valign="middle">
                                                                            <span runat="server" id="lblPasswords"></span>
                                                                        </td>
                                                                        <td align="left" class="mbodyb" style="height: 17px; width: 60%" valign="top">
                                                                            <input type="text" id="txtPasswords" runat="server" skinid="LoginTextBox" value="sBz5xZe0ZaznE49w5GigZn5xK" />
                                                                            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPasswords"
                                                                                ErrorMessage="Password is required." SetFocusOnError="True" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="Right" class="mbodyb" style="width: 40%" valign="middle">
                                                                            <span runat="server" id="lblAccountNos"></span>
                                                                        </td>
                                                                        <td align="left" class="mbodyb" style="height: 17px; width: 60%" valign="top">
                                                                            <input type="text" id="txtAccountNumbers" runat="server" skinid="LoginTextBox" value="510087267" />
                                                                            <asp:RequiredFieldValidator ID="reqAccountNo" runat="server" ControlToValidate="txtAccountNumbers"
                                                                                ErrorMessage="Account Number is required." SetFocusOnError="True" ToolTip="Account Number is required.">*</asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="Right" class="mbodyb" style="width: 40%" valign="middle">
                                                                            <span runat="server" id="lblMeterNos"></span>
                                                                        </td>
                                                                        <td align="left" class="mbodyb" style="height: 17px; width: 60%" valign="top">
                                                                            <input type="text" id="txtMeterNumbers" runat="server" skinid="LoginTextBox" value="1230563" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMeterNumbers"
                                                                                ErrorMessage="Meter Number is required." SetFocusOnError="True" ToolTip="Meter Number is required.">*</asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div id="divShipDetails" runat="server" style="display: none;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="border-right-width: 1px;
                                                border-right-style: double; border-right-color: #d7e0f1;" width="100%">
                                                <tr>
                                                    <td style="width: 50%" valign="top">
                                                        <table style="width: 100%" border="0">
                                                            <tr>
                                                                <td class="mbodyb" colspan="4">
                                                                    <asp:Panel ID="pnlShipFrom" runat="server" Width="100%">
                                                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; cursor: pointer;
                                                                            border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;"
                                                                            width="100%">
                                                                            <tr style="height: 24px;">
                                                                                <td class="grdVwCurveLeft" style="width: 5px;">
                                                                                </td>
                                                                                <td id="Td1" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                                                    border-width: 0px; width: 72px">
                                                                                    Ship From
                                                                                </td>
                                                                                <td class="grdVwCurveRight" style="width: 6px;">
                                                                                </td>
                                                                                <td id="htcShipFrom" runat="server" class="grdVwTitleAuto" style="height: 24px;"
                                                                                    width="362px">
                                                                                </td>
                                                                                <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                                                    <img id="imgShipFromExpand" alt="expandCollapse" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlShipFromContent" runat="server" Height="100%" Width="100%" HorizontalAlign="Center">
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <div id="divDHLShow" visible="false">
                                                                                        <table style="height: 100%; width: 100%">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Sent By :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtDHLSentBy" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqDHLSentBy" runat="server" ControlToValidate="txtDHlSentBy"
                                                                                                        ErrorMessage="Sent By Name is required." SetFocusOnError="True" ToolTip="Sent By Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Company Name :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtDHLCompany" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqDHlCompanyName" runat="server" ControlToValidate="txtDHLCompany"
                                                                                                        ErrorMessage="Company Name is required." SetFocusOnError="True" ToolTip="Company Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Department Name :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtDHLSuiteDepartmentName" runat="server" SkinID="LoginTextBox"
                                                                                                        >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqDHLSuiteDepartmentName" runat="server" ControlToValidate="txtDHLCompany"
                                                                                                        ErrorMessage="Suite Department  Name is required." SetFocusOnError="True" ToolTip="Suite Department Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                    Street :</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtStreetFrom" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqStreetFrom" runat="server" ControlToValidate="txtStreetFrom"
                                                                                        ErrorMessage="Street  Name is required." SetFocusOnError="True" ToolTip="Street Name is required.">*</asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <div id="divDHLStreet2" visible="false">
                                                                                        <table style="height: 100%; width: 100%">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Street 2 :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtStreet2DHL" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqStreet2DHL" runat="server" ControlToValidate="txtStreet2DHL"
                                                                                                        ErrorMessage="Street Line 2 is required." SetFocusOnError="True" ToolTip="Street Line 2 is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                    City :</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtCityAll" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqcityfrom" runat="server" ControlToValidate="txtCityAll"
                                                                                        ErrorMessage="City is required." SetFocusOnError="True" ToolTip="City is required.">*</asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                    State/Provinence:</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <select id="ddlStateAll" runat="server" class="formfields" style="width: 155px;">
                                                                                        <option value="">-No state/prov-</option>
                                                                                        <option value="AL">Alabama</option>
                                                                                        <option value="AB">Alberta (Canada)</option>
                                                                                        <option value="AK">Alaska</option>
                                                                                        <option value="AA">AFO - Americas not US/CA</option>
                                                                                        <option value="AE">AFO - Other Overseas</option>
                                                                                        <option value="AP">AFO - Pacific and Ind. Ocean</option>
                                                                                        <option value="AZ">Arizona</option>
                                                                                        <option value="AR">Arkansas</option>
                                                                                        <option value="BC">British Columbia (Canada)</option>
                                                                                        <option selected="selected" value="CA">California</option>
                                                                                        <option value="CO">Colorado</option>
                                                                                        <option value="CT">Connecticut</option>
                                                                                        <option value="DE">Delaware</option>
                                                                                        <option value="DC">Dist. of Columbia</option>
                                                                                        <option value="FL">Florida</option>
                                                                                        <option value="GA">Georgia</option>
                                                                                        <option value="HI">Hawaii</option>
                                                                                        <option value="ID">Idaho</option>
                                                                                        <option value="IL">Illinois</option>
                                                                                        <option value="IN">Indiana</option>
                                                                                        <option value="IA">Iowa</option>
                                                                                        <option value="KS">Kansas</option>
                                                                                        <option value="KY">Kentucky</option>
                                                                                        <option value="LA">Louisiana</option>
                                                                                        <option value="ME">Maine</option>
                                                                                        <option value="MB">Manitoba (Canada)</option>
                                                                                        <option value="MD">Maryland</option>
                                                                                        <option value="MA">Massachusetts</option>
                                                                                        <option value="MI">Michigan</option>
                                                                                        <option value="MN">Minnesota</option>
                                                                                        <option value="MS">Mississippi</option>
                                                                                        <option value="MO">Missouri</option>
                                                                                        <option value="MT">Montana</option>
                                                                                        <option value="NE">Nebraska</option>
                                                                                        <option value="NV">Nevada</option>
                                                                                        <option value="NB">New Brunswick (Canada)</option>
                                                                                        <option value="NL">Newfoundland and Labrador (Canada)</option>
                                                                                        <option value="NH">New Hampshire</option>
                                                                                        <option value="NJ">New Jersey</option>
                                                                                        <option value="NM">New Mexico</option>
                                                                                        <option value="NY">New York</option>
                                                                                        <option value="NC">North Carolina</option>
                                                                                        <option value="ND">North Dakota</option>
                                                                                        <option value="NS">Nova Scotia (Canada)</option>
                                                                                        <option value="NT">Northwest Territories (Canada)</option>
                                                                                        <option value="NU">Nunavut (Canada)</option>
                                                                                        <option value="OH">Ohio</option>
                                                                                        <option value="OK">Oklahoma</option>
                                                                                        <option value="ON">Ontario (Canada)</option>
                                                                                        <option value="OR">Oregon</option>
                                                                                        <option value="QC">Qu&#233;bec (Canada)</option>
                                                                                        <option value="PA">Pennsylvania</option>
                                                                                        <option value="PE">Prince Edward Island (Canada)</option>
                                                                                        <option value="RI">Rhode Island</option>
                                                                                        <option value="SK">Saskatchewan (Canada)</option>
                                                                                        <option value="SC">South Carolina</option>
                                                                                        <option value="SD">South Dakota</option>
                                                                                        <option value="TN">Tennessee</option>
                                                                                        <option value="TX">Texas</option>
                                                                                        <option value="UT">Utah</option>
                                                                                        <option value="VT">Vermont</option>
                                                                                        <option value="VA">Virginia</option>
                                                                                        <option value="WA">Washington</option>
                                                                                        <option value="WV">West Virginia</option>
                                                                                        <option value="WI">Wisconsin</option>
                                                                                        <option value="WY">Wyoming</option>
                                                                                        <option value="YT">Yukon (Canada)</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                    Postal Code :
                                                                                </td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtPostalCodeAll" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqPostalCodeAll" runat="server" ControlToValidate="txtPostalCodeAll"
                                                                                        ErrorMessage="Postal Code is required." SetFocusOnError="True" ToolTip="Postal Code is required.">*</asp:RequiredFieldValidator></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                    Country :
                                                                                </td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <select id="ddlCountryAll" runat="server" class="formfields">
                                                                                        <option value="AF">Afghanistan</option>
                                                                                        <option value="AL">Albania</option>
                                                                                        <option value="DZ">Algeria</option>
                                                                                        <option value="AS">American Samoa</option>
                                                                                        <option value="AD">Andorra</option>
                                                                                        <option value="AO">Angola</option>
                                                                                        <option value="AI">Anguilla</option>
                                                                                        <option value="AG">Antigua/Barbuda</option>
                                                                                        <option value="AR">Argentina</option>
                                                                                        <option value="AM">Armenia</option>
                                                                                        <option value="AW">Aruba</option>
                                                                                        <option value="AU">Australia</option>
                                                                                        <option value="AT">Austria</option>
                                                                                        <option value="AZ">Azerbaijan</option>
                                                                                        <option value="BS">Bahamas</option>
                                                                                        <option value="BH">Bahrain</option>
                                                                                        <option value="BD">Bangladesh</option>
                                                                                        <option value="BB">Barbados</option>
                                                                                        <option value="BY">Belarus</option>
                                                                                        <option value="BE">Belgium</option>
                                                                                        <option value="BZ">Belize</option>
                                                                                        <option value="BJ">Benin</option>
                                                                                        <option value="BM">Bermuda</option>
                                                                                        <option value="BT">Bhutan</option>
                                                                                        <option value="BO">Bolivia</option>
                                                                                        <option value="BA">Bosnia-Herzegovina</option>
                                                                                        <option value="BW">Botswana</option>
                                                                                        <option value="BR">Brazil</option>
                                                                                        <option value="VG">British Virgin Islands</option>
                                                                                        <option value="BN">Brunei</option>
                                                                                        <option value="BG">Bulgaria</option>
                                                                                        <option value="BF">Burkina Faso</option>
                                                                                        <option value="BI">Burundi</option>
                                                                                        <option value="KH">Cambodia</option>
                                                                                        <option value="CM">Cameroon</option>
                                                                                        <option value="CA">Canada</option>
                                                                                        <option value="CV">Cape Verde</option>
                                                                                        <option value="KY">Cayman Islands</option>
                                                                                        <option value="TD">Chad</option>
                                                                                        <option value="CL">Chile</option>
                                                                                        <option value="CN">China</option>
                                                                                        <option value="CO">Colombia</option>
                                                                                        <option value="CG">Congo Brazzaville</option>
                                                                                        <option value="CD">Congo Democratic Rep. of</option>
                                                                                        <option value="CK">Cook Islands</option>
                                                                                        <option value="CR">Costa Rica</option>
                                                                                        <option value="HR">Croatia</option>
                                                                                        <option value="CY">Cyprus</option>
                                                                                        <option value="CZ">Czech Republic</option>
                                                                                        <option value="DK">Denmark</option>
                                                                                        <option value="DJ">Djibouti</option>
                                                                                        <option value="DM">Dominica</option>
                                                                                        <option value="DO">Dominican Republic</option>
                                                                                        <option value="TL">East Timor</option>
                                                                                        <option value="EC">Ecuador</option>
                                                                                        <option value="EG">Egypt</option>
                                                                                        <option value="SV">El Salvador</option>
                                                                                        <option value="GQ">Equatorial Guinea</option>
                                                                                        <option value="ER">Eritrea</option>
                                                                                        <option value="EE">Estonia</option>
                                                                                        <option value="ET">Ethiopia</option>
                                                                                        <option value="FO">Faeroe Islands</option>
                                                                                        <option value="FJ">Fiji</option>
                                                                                        <option value="FI">Finland</option>
                                                                                        <option value="FR">France</option>
                                                                                        <option value="GF">French Guiana</option>
                                                                                        <option value="PF">French Polynesia</option>
                                                                                        <option value="GA">Gabon</option>
                                                                                        <option value="GM">Gambia</option>
                                                                                        <option value="GE">Georgia</option>
                                                                                        <option value="DE">Germany</option>
                                                                                        <option value="GH">Ghana</option>
                                                                                        <option value="GI">Gibraltar</option>
                                                                                        <option value="GR">Greece</option>
                                                                                        <option value="GL">Greenland</option>
                                                                                        <option value="GD">Grenada</option>
                                                                                        <option value="GP">Guadeloupe</option>
                                                                                        <option value="GU">Guam</option>
                                                                                        <option value="GT">Guatemala</option>
                                                                                        <option value="GN">Guinea</option>
                                                                                        <option value="GY">Guyana</option>
                                                                                        <option value="HT">Haiti</option>
                                                                                        <option value="HN">Honduras</option>
                                                                                        <option value="HK">Hong Kong</option>
                                                                                        <option value="HU">Hungary</option>
                                                                                        <option value="IS">Iceland</option>
                                                                                        <option value="IN">India</option>
                                                                                        <option value="ID">Indonesia</option>
                                                                                        <option value="IQ">Iraq</option>
                                                                                        <option value="IE">Ireland</option>
                                                                                        <option value="IL">Israel</option>
                                                                                        <option value="IT">Italy</option>
                                                                                        <option value="CI">Ivory Coast</option>
                                                                                        <option value="JM">Jamaica</option>
                                                                                        <option value="JP">Japan</option>
                                                                                        <option value="JO">Jordan</option>
                                                                                        <option value="KZ">Kazakhstan</option>
                                                                                        <option value="KE">Kenya</option>
                                                                                        <option value="KW">Kuwait</option>
                                                                                        <option value="KG">Kyrgyzstan</option>
                                                                                        <option value="LA">Laos</option>
                                                                                        <option value="LV">Latvia</option>
                                                                                        <option value="LB">Lebanon</option>
                                                                                        <option value="LS">Lesotho</option>
                                                                                        <option value="LR">Liberia</option>
                                                                                        <option value="LY">Libya</option>
                                                                                        <option value="LI">Liechtenstein</option>
                                                                                        <option value="LT">Lithuania</option>
                                                                                        <option value="LU">Luxembourg</option>
                                                                                        <option value="MO">Macau</option>
                                                                                        <option value="MK">Macedonia</option>
                                                                                        <option value="MG">Madagascar</option>
                                                                                        <option value="MW">Malawi</option>
                                                                                        <option value="MY">Malaysia</option>
                                                                                        <option value="MV">Maldives</option>
                                                                                        <option value="ML">Mali</option>
                                                                                        <option value="MT">Malta</option>
                                                                                        <option value="MH">Marshall Islands</option>
                                                                                        <option value="MQ">Martinique</option>
                                                                                        <option value="MR">Mauritania</option>
                                                                                        <option value="MU">Mauritius</option>
                                                                                        <option value="MX">Mexico</option>
                                                                                        <option value="FM">Micronesia</option>
                                                                                        <option value="MD">Moldova</option>
                                                                                        <option value="MC">Monaco</option>
                                                                                        <option value="MN">Mongolia</option>
                                                                                        <option value="ME">Montenegro</option>
                                                                                        <option value="MS">Montserrat</option>
                                                                                        <option value="MA">Morocco</option>
                                                                                        <option value="MZ">Mozambique</option>
                                                                                        <option value="NA">Namibia</option>
                                                                                        <option value="NP">Nepal</option>
                                                                                        <option value="NL">Netherlands</option>
                                                                                        <option value="AN">Netherlands Antilles</option>
                                                                                        <option value="NC">New Caledonia</option>
                                                                                        <option value="NZ">New Zealand</option>
                                                                                        <option value="NI">Nicaragua</option>
                                                                                        <option value="NE">Niger</option>
                                                                                        <option value="NG">Nigeria</option>
                                                                                        <option value="NO">Norway</option>
                                                                                        <option value="OM">Oman</option>
                                                                                        <option value="PK">Pakistan</option>
                                                                                        <option value="PW">Palau</option>
                                                                                        <option value="PS">Palestine Autonomous</option>
                                                                                        <option value="PA">Panama</option>
                                                                                        <option value="PG">Papua New Guinea</option>
                                                                                        <option value="PY">Paraguay</option>
                                                                                        <option value="PE">Peru</option>
                                                                                        <option value="PH">Philippines</option>
                                                                                        <option value="PL">Poland</option>
                                                                                        <option value="PT">Portugal</option>
                                                                                        <option value="PR">Puerto Rico</option>
                                                                                        <option value="QA">Qatar</option>
                                                                                        <option value="RE">Reunion</option>
                                                                                        <option value="RO">Romania</option>
                                                                                        <option value="RU">Russian Federation</option>
                                                                                        <option value="RW">Rwanda</option>
                                                                                        <option value="MP">Saipan</option>
                                                                                        <option value="WS">Samoa</option>
                                                                                        <option value="SA">Saudi Arabia</option>
                                                                                        <option value="SN">Senegal</option>
                                                                                        <option value="RS">Serbia</option>
                                                                                        <option value="SC">Seychelles</option>
                                                                                        <option value="SG">Singapore</option>
                                                                                        <option value="SK">Slovak Republic</option>
                                                                                        <option value="SI">Slovenia</option>
                                                                                        <option value="ZA">South Africa</option>
                                                                                        <option value="KR">South Korea</option>
                                                                                        <option value="ES">Spain</option>
                                                                                        <option value="LK">Sri Lanka</option>
                                                                                        <option value="KN">St. Kitts/Nevis</option>
                                                                                        <option value="LC">St. Lucia</option>
                                                                                        <option value="VC">St. Vincent</option>
                                                                                        <option value="SR">Suriname</option>
                                                                                        <option value="SZ">Swaziland</option>
                                                                                        <option value="SE">Sweden</option>
                                                                                        <option value="CH">Switzerland</option>
                                                                                        <option value="SY">Syria</option>
                                                                                        <option value="TW">Taiwan</option>
                                                                                        <option value="TZ">Tanzania</option>
                                                                                        <option value="TH">Thailand</option>
                                                                                        <option value="TG">Togo</option>
                                                                                        <option value="TO">Tonga</option>
                                                                                        <option value="TT">Trinidad/Tobago</option>
                                                                                        <option value="TN">Tunisia</option>
                                                                                        <option value="TR">Turkey</option>
                                                                                        <option value="TM">Turkmenistan</option>
                                                                                        <option value="TC">Turks &amp; Caicos Islands</option>
                                                                                        <option selected="selected" value="US">U.S.A.</option>
                                                                                        <option value="VI">U.S. Virgin Islands</option>
                                                                                        <option value="UG">Uganda</option>
                                                                                        <option value="UA">Ukraine</option>
                                                                                        <option value="AE">United Arab Emirates</option>
                                                                                        <option value="GB">United Kingdom</option>
                                                                                        <option value="UY">Uruguay</option>
                                                                                        <option value="UZ">Uzbekistan</option>
                                                                                        <option value="VU">Vanuatu</option>
                                                                                        <option value="VE">Venezuela</option>
                                                                                        <option value="VN">Vietnam</option>
                                                                                        <option value="WF">Wallis &amp; Futuna</option>
                                                                                        <option value="YE">Yemen</option>
                                                                                        <option value="ZM">Zambia</option>
                                                                                        <option value="ZW">Zimbabwe</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <div id="divPhoneNoDHLUPS" visible="false">
                                                                                        <table style="height: 100%; width: 100%">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Phone Number :
                                                                                                </td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtPhoneNoAll" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqPhoneNoAll" runat="server" ControlToValidate="txtPhoneNoAll"
                                                                                                        ErrorMessage="Phone Number is required." SetFocusOnError="True" ToolTip="Phone Number is required.">*</asp:RequiredFieldValidator></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 50%" valign="Top">
                                                        <table style="width: 100%" border="0">
                                                            <tr>
                                                                <td class="mbodyb" colspan="4" valign="Top">
                                                                    <asp:Panel ID="pnlShipTo" runat="server" Width="100%">
                                                                        <table border="0" cellpadding="0" cellspacing="0" style="cursor: pointer; border-right-width: 1px;
                                                                            border-right-style: double; border-right-color: #d7e0f1; vertical-align: top"
                                                                            width="100%">
                                                                            <tr style="height: 24px;">
                                                                                <td class="grdVwCurveLeft" style="width: 5px;" valign="Top">
                                                                                </td>
                                                                                <td id="Td3" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                                                    border-width: 0px; width: 60px">
                                                                                    Ship To
                                                                                </td>
                                                                                <td class="grdVwCurveRight" style="width: 6px;">
                                                                                </td>
                                                                                <td id="htcShipTo" runat="server" class="grdVwTitleAuto" style="height: 24px;" width="380px">
                                                                                </td>
                                                                                <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                                                    <img id="imgShipToExpand" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlShipToContent" runat="server" Height="100%" Width="100%" HorizontalAlign="Center">
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="3" valign="Top">
                                                                                    <div id="divDHLReceiver" style="display: none">
                                                                                        <table border="0" style="height: 100%; width: 100%">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Received By:</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtReceivedDHL" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqReceivedDHL" runat="server" ControlToValidate="txtReceivedDHL"
                                                                                                        ErrorMessage="Received By Name is required." SetFocusOnError="True" ToolTip="Received By Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Company Name :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtCompanyNameReceiver" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqCompanyReceiverName" runat="server" ControlToValidate="txtCompanyNameReceiver"
                                                                                                        ErrorMessage="Company Name is required." SetFocusOnError="True" ToolTip="Company Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Department Name :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtDepartmentReceiverName" runat="server" SkinID="LoginTextBox"
                                                                                                        >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqDepReceiverName" runat="server" ControlToValidate="txtDepartmentReceiverName"
                                                                                                        ErrorMessage="Suite Department  Name is required." SetFocusOnError="True" ToolTip="Suite Department Name is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                    Street :</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtStreetReceiver" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqStreetReceiver" runat="server" ControlToValidate="txtStreetReceiver"
                                                                                        ErrorMessage="Street  Name is required." SetFocusOnError="True" ToolTip="Street Name is required.">*</asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" valign="Top">
                                                                                    <div id="divReceiverStreet2" visible="false">
                                                                                        <table border="0" style="height: 100%; width: 100%">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                                    Street 2 :</td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtReceiverStreet2" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqReceiverStreet2" runat="server" ControlToValidate="txtReceiverStreet2"
                                                                                                        ErrorMessage="Street Line 2 is required." SetFocusOnError="True" ToolTip="Street Line 2 is required.">*</asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                    City :</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtReceiverCity" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqReceiverCity" runat="server" ControlToValidate="txtReceiverCity"
                                                                                        ErrorMessage="City is required." SetFocusOnError="True" ToolTip="City is required.">*</asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                    State/Provinence:</td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <select id="ddlReceiverState" runat="server" class="formfields" style="width: 155px;">
                                                                                        <option value="">-No state/prov-</option>
                                                                                        <option value="AL">Alabama</option>
                                                                                        <option value="AB">Alberta (Canada)</option>
                                                                                        <option value="AK">Alaska</option>
                                                                                        <option value="AA">AFO - Americas not US/CA</option>
                                                                                        <option value="AE">AFO - Other Overseas</option>
                                                                                        <option value="AP">AFO - Pacific and Ind. Ocean</option>
                                                                                        <option value="AZ">Arizona</option>
                                                                                        <option value="AR">Arkansas</option>
                                                                                        <option value="BC">British Columbia (Canada)</option>
                                                                                        <option selected="selected" value="CA">California</option>
                                                                                        <option value="CO">Colorado</option>
                                                                                        <option value="CT">Connecticut</option>
                                                                                        <option value="DE">Delaware</option>
                                                                                        <option value="DC">Dist. of Columbia</option>
                                                                                        <option value="FL">Florida</option>
                                                                                        <option value="GA">Georgia</option>
                                                                                        <option value="HI">Hawaii</option>
                                                                                        <option value="ID">Idaho</option>
                                                                                        <option value="IL">Illinois</option>
                                                                                        <option value="IN">Indiana</option>
                                                                                        <option value="IA">Iowa</option>
                                                                                        <option value="KS">Kansas</option>
                                                                                        <option value="KY">Kentucky</option>
                                                                                        <option value="LA">Louisiana</option>
                                                                                        <option value="ME">Maine</option>
                                                                                        <option value="MB">Manitoba (Canada)</option>
                                                                                        <option value="MD">Maryland</option>
                                                                                        <option value="MA">Massachusetts</option>
                                                                                        <option value="MI">Michigan</option>
                                                                                        <option value="MN">Minnesota</option>
                                                                                        <option value="MS">Mississippi</option>
                                                                                        <option value="MO">Missouri</option>
                                                                                        <option value="MT">Montana</option>
                                                                                        <option value="NE">Nebraska</option>
                                                                                        <option value="NV">Nevada</option>
                                                                                        <option value="NB">New Brunswick (Canada)</option>
                                                                                        <option value="NL">Newfoundland and Labrador (Canada)</option>
                                                                                        <option value="NH">New Hampshire</option>
                                                                                        <option value="NJ">New Jersey</option>
                                                                                        <option value="NM">New Mexico</option>
                                                                                        <option value="NY">New York</option>
                                                                                        <option value="NC">North Carolina</option>
                                                                                        <option value="ND">North Dakota</option>
                                                                                        <option value="NS">Nova Scotia (Canada)</option>
                                                                                        <option value="NT">Northwest Territories (Canada)</option>
                                                                                        <option value="NU">Nunavut (Canada)</option>
                                                                                        <option value="OH">Ohio</option>
                                                                                        <option value="OK">Oklahoma</option>
                                                                                        <option value="ON">Ontario (Canada)</option>
                                                                                        <option value="OR">Oregon</option>
                                                                                        <option value="QC">Qu&#233;bec (Canada)</option>
                                                                                        <option value="PA">Pennsylvania</option>
                                                                                        <option value="PE">Prince Edward Island (Canada)</option>
                                                                                        <option value="RI">Rhode Island</option>
                                                                                        <option value="SK">Saskatchewan (Canada)</option>
                                                                                        <option value="SC">South Carolina</option>
                                                                                        <option value="SD">South Dakota</option>
                                                                                        <option value="TN">Tennessee</option>
                                                                                        <option value="TX">Texas</option>
                                                                                        <option value="UT">Utah</option>
                                                                                        <option value="VT">Vermont</option>
                                                                                        <option value="VA">Virginia</option>
                                                                                        <option value="WA">Washington</option>
                                                                                        <option value="WV">West Virginia</option>
                                                                                        <option value="WI">Wisconsin</option>
                                                                                        <option value="WY">Wyoming</option>
                                                                                        <option value="YT">Yukon (Canada)</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                    Postal Code :
                                                                                </td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <asp:TextBox ID="txtReceiverPostalCode" runat="server" SkinID="LoginTextBox" >
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqReceiverPostalCode" runat="server" ControlToValidate="txtReceiverPostalCode"
                                                                                        ErrorMessage="Postal Code is required." SetFocusOnError="True" ToolTip="Postal Code is required.">*</asp:RequiredFieldValidator></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="Top">
                                                                                    Country :
                                                                                </td>
                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                    <select id="ddlReceiverCountry" runat="server" class="formfields">
                                                                                        <option value="AF">Afghanistan</option>
                                                                                        <option value="AL">Albania</option>
                                                                                        <option value="DZ">Algeria</option>
                                                                                        <option value="AS">American Samoa</option>
                                                                                        <option value="AD">Andorra</option>
                                                                                        <option value="AO">Angola</option>
                                                                                        <option value="AI">Anguilla</option>
                                                                                        <option value="AG">Antigua/Barbuda</option>
                                                                                        <option value="AR">Argentina</option>
                                                                                        <option value="AM">Armenia</option>
                                                                                        <option value="AW">Aruba</option>
                                                                                        <option value="AU">Australia</option>
                                                                                        <option value="AT">Austria</option>
                                                                                        <option value="AZ">Azerbaijan</option>
                                                                                        <option value="BS">Bahamas</option>
                                                                                        <option value="BH">Bahrain</option>
                                                                                        <option value="BD">Bangladesh</option>
                                                                                        <option value="BB">Barbados</option>
                                                                                        <option value="BY">Belarus</option>
                                                                                        <option value="BE">Belgium</option>
                                                                                        <option value="BZ">Belize</option>
                                                                                        <option value="BJ">Benin</option>
                                                                                        <option value="BM">Bermuda</option>
                                                                                        <option value="BT">Bhutan</option>
                                                                                        <option value="BO">Bolivia</option>
                                                                                        <option value="BA">Bosnia-Herzegovina</option>
                                                                                        <option value="BW">Botswana</option>
                                                                                        <option value="BR">Brazil</option>
                                                                                        <option value="VG">British Virgin Islands</option>
                                                                                        <option value="BN">Brunei</option>
                                                                                        <option value="BG">Bulgaria</option>
                                                                                        <option value="BF">Burkina Faso</option>
                                                                                        <option value="BI">Burundi</option>
                                                                                        <option value="KH">Cambodia</option>
                                                                                        <option value="CM">Cameroon</option>
                                                                                        <option value="CA">Canada</option>
                                                                                        <option value="CV">Cape Verde</option>
                                                                                        <option value="KY">Cayman Islands</option>
                                                                                        <option value="TD">Chad</option>
                                                                                        <option value="CL">Chile</option>
                                                                                        <option value="CN">China</option>
                                                                                        <option value="CO">Colombia</option>
                                                                                        <option value="CG">Congo Brazzaville</option>
                                                                                        <option value="CD">Congo Democratic Rep. of</option>
                                                                                        <option value="CK">Cook Islands</option>
                                                                                        <option value="CR">Costa Rica</option>
                                                                                        <option value="HR">Croatia</option>
                                                                                        <option value="CY">Cyprus</option>
                                                                                        <option value="CZ">Czech Republic</option>
                                                                                        <option value="DK">Denmark</option>
                                                                                        <option value="DJ">Djibouti</option>
                                                                                        <option value="DM">Dominica</option>
                                                                                        <option value="DO">Dominican Republic</option>
                                                                                        <option value="TL">East Timor</option>
                                                                                        <option value="EC">Ecuador</option>
                                                                                        <option value="EG">Egypt</option>
                                                                                        <option value="SV">El Salvador</option>
                                                                                        <option value="GQ">Equatorial Guinea</option>
                                                                                        <option value="ER">Eritrea</option>
                                                                                        <option value="EE">Estonia</option>
                                                                                        <option value="ET">Ethiopia</option>
                                                                                        <option value="FO">Faeroe Islands</option>
                                                                                        <option value="FJ">Fiji</option>
                                                                                        <option value="FI">Finland</option>
                                                                                        <option value="FR">France</option>
                                                                                        <option value="GF">French Guiana</option>
                                                                                        <option value="PF">French Polynesia</option>
                                                                                        <option value="GA">Gabon</option>
                                                                                        <option value="GM">Gambia</option>
                                                                                        <option value="GE">Georgia</option>
                                                                                        <option value="DE">Germany</option>
                                                                                        <option value="GH">Ghana</option>
                                                                                        <option value="GI">Gibraltar</option>
                                                                                        <option value="GR">Greece</option>
                                                                                        <option value="GL">Greenland</option>
                                                                                        <option value="GD">Grenada</option>
                                                                                        <option value="GP">Guadeloupe</option>
                                                                                        <option value="GU">Guam</option>
                                                                                        <option value="GT">Guatemala</option>
                                                                                        <option value="GN">Guinea</option>
                                                                                        <option value="GY">Guyana</option>
                                                                                        <option value="HT">Haiti</option>
                                                                                        <option value="HN">Honduras</option>
                                                                                        <option value="HK">Hong Kong</option>
                                                                                        <option value="HU">Hungary</option>
                                                                                        <option value="IS">Iceland</option>
                                                                                        <option value="IN">India</option>
                                                                                        <option value="ID">Indonesia</option>
                                                                                        <option value="IQ">Iraq</option>
                                                                                        <option value="IE">Ireland</option>
                                                                                        <option value="IL">Israel</option>
                                                                                        <option value="IT">Italy</option>
                                                                                        <option value="CI">Ivory Coast</option>
                                                                                        <option value="JM">Jamaica</option>
                                                                                        <option value="JP">Japan</option>
                                                                                        <option value="JO">Jordan</option>
                                                                                        <option value="KZ">Kazakhstan</option>
                                                                                        <option value="KE">Kenya</option>
                                                                                        <option value="KW">Kuwait</option>
                                                                                        <option value="KG">Kyrgyzstan</option>
                                                                                        <option value="LA">Laos</option>
                                                                                        <option value="LV">Latvia</option>
                                                                                        <option value="LB">Lebanon</option>
                                                                                        <option value="LS">Lesotho</option>
                                                                                        <option value="LR">Liberia</option>
                                                                                        <option value="LY">Libya</option>
                                                                                        <option value="LI">Liechtenstein</option>
                                                                                        <option value="LT">Lithuania</option>
                                                                                        <option value="LU">Luxembourg</option>
                                                                                        <option value="MO">Macau</option>
                                                                                        <option value="MK">Macedonia</option>
                                                                                        <option value="MG">Madagascar</option>
                                                                                        <option value="MW">Malawi</option>
                                                                                        <option value="MY">Malaysia</option>
                                                                                        <option value="MV">Maldives</option>
                                                                                        <option value="ML">Mali</option>
                                                                                        <option value="MT">Malta</option>
                                                                                        <option value="MH">Marshall Islands</option>
                                                                                        <option value="MQ">Martinique</option>
                                                                                        <option value="MR">Mauritania</option>
                                                                                        <option value="MU">Mauritius</option>
                                                                                        <option value="MX">Mexico</option>
                                                                                        <option value="FM">Micronesia</option>
                                                                                        <option value="MD">Moldova</option>
                                                                                        <option value="MC">Monaco</option>
                                                                                        <option value="MN">Mongolia</option>
                                                                                        <option value="ME">Montenegro</option>
                                                                                        <option value="MS">Montserrat</option>
                                                                                        <option value="MA">Morocco</option>
                                                                                        <option value="MZ">Mozambique</option>
                                                                                        <option value="NA">Namibia</option>
                                                                                        <option value="NP">Nepal</option>
                                                                                        <option value="NL">Netherlands</option>
                                                                                        <option value="AN">Netherlands Antilles</option>
                                                                                        <option value="NC">New Caledonia</option>
                                                                                        <option value="NZ">New Zealand</option>
                                                                                        <option value="NI">Nicaragua</option>
                                                                                        <option value="NE">Niger</option>
                                                                                        <option value="NG">Nigeria</option>
                                                                                        <option value="NO">Norway</option>
                                                                                        <option value="OM">Oman</option>
                                                                                        <option value="PK">Pakistan</option>
                                                                                        <option value="PW">Palau</option>
                                                                                        <option value="PS">Palestine Autonomous</option>
                                                                                        <option value="PA">Panama</option>
                                                                                        <option value="PG">Papua New Guinea</option>
                                                                                        <option value="PY">Paraguay</option>
                                                                                        <option value="PE">Peru</option>
                                                                                        <option value="PH">Philippines</option>
                                                                                        <option value="PL">Poland</option>
                                                                                        <option value="PT">Portugal</option>
                                                                                        <option value="PR">Puerto Rico</option>
                                                                                        <option value="QA">Qatar</option>
                                                                                        <option value="RE">Reunion</option>
                                                                                        <option value="RO">Romania</option>
                                                                                        <option value="RU">Russian Federation</option>
                                                                                        <option value="RW">Rwanda</option>
                                                                                        <option value="MP">Saipan</option>
                                                                                        <option value="WS">Samoa</option>
                                                                                        <option value="SA">Saudi Arabia</option>
                                                                                        <option value="SN">Senegal</option>
                                                                                        <option value="RS">Serbia</option>
                                                                                        <option value="SC">Seychelles</option>
                                                                                        <option value="SG">Singapore</option>
                                                                                        <option value="SK">Slovak Republic</option>
                                                                                        <option value="SI">Slovenia</option>
                                                                                        <option value="ZA">South Africa</option>
                                                                                        <option value="KR">South Korea</option>
                                                                                        <option value="ES">Spain</option>
                                                                                        <option value="LK">Sri Lanka</option>
                                                                                        <option value="KN">St. Kitts/Nevis</option>
                                                                                        <option value="LC">St. Lucia</option>
                                                                                        <option value="VC">St. Vincent</option>
                                                                                        <option value="SR">Suriname</option>
                                                                                        <option value="SZ">Swaziland</option>
                                                                                        <option value="SE">Sweden</option>
                                                                                        <option value="CH">Switzerland</option>
                                                                                        <option value="SY">Syria</option>
                                                                                        <option value="TW">Taiwan</option>
                                                                                        <option value="TZ">Tanzania</option>
                                                                                        <option value="TH">Thailand</option>
                                                                                        <option value="TG">Togo</option>
                                                                                        <option value="TO">Tonga</option>
                                                                                        <option value="TT">Trinidad/Tobago</option>
                                                                                        <option value="TN">Tunisia</option>
                                                                                        <option value="TR">Turkey</option>
                                                                                        <option value="TM">Turkmenistan</option>
                                                                                        <option value="TC">Turks &amp; Caicos Islands</option>
                                                                                        <option selected="selected" value="US">U.S.A.</option>
                                                                                        <option value="VI">U.S. Virgin Islands</option>
                                                                                        <option value="UG">Uganda</option>
                                                                                        <option value="UA">Ukraine</option>
                                                                                        <option value="AE">United Arab Emirates</option>
                                                                                        <option value="GB">United Kingdom</option>
                                                                                        <option value="UY">Uruguay</option>
                                                                                        <option value="UZ">Uzbekistan</option>
                                                                                        <option value="VU">Vanuatu</option>
                                                                                        <option value="VE">Venezuela</option>
                                                                                        <option value="VN">Vietnam</option>
                                                                                        <option value="WF">Wallis &amp; Futuna</option>
                                                                                        <option value="YE">Yemen</option>
                                                                                        <option value="ZM">Zambia</option>
                                                                                        <option value="ZW">Zimbabwe</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" valign="Top">
                                                                                    <div id="divReceiverPhoneNo" visible="false">
                                                                                        <table style="height: 100%; width: 100%" border="0">
                                                                                            <tr>
                                                                                                <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                                    Phone Number :
                                                                                                </td>
                                                                                                <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                                    <asp:TextBox ID="txtReceiverPhoneNo" runat="server" SkinID="LoginTextBox" >
                                                                                                    </asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="reqReceiverPhoneNo" runat="server" ControlToValidate="txtReceiverPhoneNo"
                                                                                                        ErrorMessage="Phone Number is required." SetFocusOnError="True" ToolTip="Phone Number is required.">*</asp:RequiredFieldValidator></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" valign="Top">
                                        <div id="divPackageDetails" style="display: none">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; border-right-width: 1px;
                                                border-right-style: double; border-right-color: #d7e0f1; border-bottom-color: #d7e0f1;
                                                border-bottom-width: 1px; border-bottom-style: double;" width="100%">
                                                <tr>
                                                    <td class="mbodyb" colspan="4">
                                                        <asp:Panel ID="pnlPackageDetails" runat="server" Width="100%">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; cursor: pointer;
                                                                border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;"
                                                                width="100%">
                                                                <tr style="height: 24px;">
                                                                    <td class="grdVwCurveLeft" style="width: 5px;">
                                                                    </td>
                                                                    <td id="Td7" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                                        border-width: 0px; width: 100px">
                                                                        Package Details
                                                                    </td>
                                                                    <td class="grdVwCurveRight" style="width: 6px;">
                                                                    </td>
                                                                    <td id="htcPackageDetails" runat="server" class="grdVwTitleAuto" style="height: 24px;"
                                                                        width="715px">
                                                                    </td>
                                                                    <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                                        <img id="imgPackageDetailsExpand" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlPackageDetailsContent" runat="server" Height="100%" Width="100%"
                                                            HorizontalAlign="Center">
                                                            <table border="0" style="width: 100%; vertical-align: top">
                                                                <tr>
                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                        Weight :
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td align="left" class="mbodyb" style="height: 17px" valign="top">
                                                                        <asp:TextBox ID="txtWeight" runat="server"  Width="120px">
                                                                        </asp:TextBox>
                                                                       
                                                                        <select id="ddlUnits" runat="server" class="formfields" name="Units">
                                                                            <option value="KGS">KGS</option>
                                                                            <option value="LBS">LBS</option>
                                                                        </select>
                                                                       <asp:RegularExpressionValidator runat="server" ID="regWeight" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ControlToValidate="txtWeight"></asp:RegularExpressionValidator>
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexPackagesCount" style="display: none">
                                                                            <table style="width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Package Count :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtFedexPackageCount" runat="server" SkinID="LoginTextBox" >
                                                                                        </asp:TextBox>
                                                                                       <asp:RegularExpressionValidator runat="server" ID="regPackageCount" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ControlToValidate="txtFedexPackageCount"></asp:RegularExpressionValidator> 
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexPackagesIdentical" style="display: none">
                                                                            <table style="width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Are Packages Identical :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <input id="radYes" runat="server" type="radio" />Yes
                                                                                        <input id="radNo" runat="server" type="radio" />
                                                                                        No
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                        Dimensions :</td>
                                                                    <td align="left" class="mbodyb" colspan="6" style="width: 75%">
                                                                        <table style="width: 70%" border="0" style="border-color: Black">
                                                                            <tr>
                                                                                <td align="left" class="mbodyb" colspan="6" style="height: 17px;" valign="middle">
                                                                                    Length :
                                                                                </td>
                                                                                <td colspan="6" valign="bottom">
                                                                                    <asp:TextBox ID="txtLength" runat="server"  Width="50px">
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqLength" runat="server" ControlToValidate="txtLength"
                                                                                        ErrorMessage="Length is required." SetFocusOnError="True" ToolTip="Length is required.">*</asp:RequiredFieldValidator></td>
                                                                                     <asp:RegularExpressionValidator runat="server" ID="regLength" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ControlToValidate="txtLength"></asp:RegularExpressionValidator>    
                                                                                <td align="left" class="mbodyb" colspan="6" style="height: 17px;" valign="middle">
                                                                                    Width :
                                                                                </td>
                                                                                <td colspan="6" valign="bottom">
                                                                                    <asp:TextBox ID="txtWidth" runat="server"  Width="50px">
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqWidth" runat="server" ControlToValidate="txtWidth"
                                                                                        ErrorMessage="Width is required." SetFocusOnError="True" ToolTip="Width is required.">*</asp:RequiredFieldValidator></td>
                                                                                        <asp:RegularExpressionValidator runat="server" ID="regWidth" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ControlToValidate="txtWidth"></asp:RegularExpressionValidator>    
                                                                                <td align="left" class="mbodyb" colspan="6" style="height: 17px;" valign="middle">
                                                                                    Height :
                                                                                </td>
                                                                                <td colspan="6" valign="bottom">
                                                                                    <asp:TextBox ID="txtHeight" runat="server"  Width="50px">
                                                                                    </asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqHeight" runat="server" ControlToValidate="txtHeight"
                                                                                        ErrorMessage="Height is required." SetFocusOnError="True" ToolTip="Height is required.">*</asp:RequiredFieldValidator>
                                                                                       <asp:RegularExpressionValidator runat="server" ID="regHeight" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ControlToValidate="txtHeight"></asp:RegularExpressionValidator>   
                                                                                        </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexPackagingServices" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="top">
                                                                                        Packaging Type :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <select id="ddlFedexPackagingOptions" runat="server">
                                                                                            <option value="FEDEX_10KG_BOX"></option>
                                                                                            <option value="FEDEX_25KG_BOX"></option>
                                                                                            <option value="FEDEX_BOX"></option>
                                                                                            <option value="FEDEX_ENVELOPE"></option>
                                                                                            <option value="FEDEX_PAK"></option>
                                                                                            <option value="FEDEX_TUBE"></option>
                                                                                            <option value="YOUR_PACKAGING"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexPayementType" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Payement Type :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <select id="ddlFedexPayementType" runat="server">
                                                                                            <option value="COLLECT"></option>
                                                                                            <option value="RECIPIENT"></option>
                                                                                            <option value="SENDER"></option>
                                                                                            <option value="THIRD_PARTY"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexServiceType" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Fedex Service Type :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <select id="ddlFedexServiceType" runat="server" onchange="CheckShipmentNames();">
                                                                                            <option value="EUROPE_FIRST_INTERNATIONAL_PRIORITY"></option>
                                                                                            <option value="FEDEX_1_DAY_FREIGHT"></option>
                                                                                            <option value="FEDEX_2_DAY"></option>
                                                                                            <option value="FEDEX_2_DAY_FREIGHT"></option>
                                                                                            <option value="FEDEX_3_DAY_FREIGHT"></option>
                                                                                            <option value="FEDEX_EXPRESS_SAVER"></option>
                                                                                            <option value="FEDEX_GROUND"></option>
                                                                                            <option value="FIRST_OVERNIGHT"></option>
                                                                                            <option value="GROUND_HOME_DELIVERY"></option>
                                                                                            <option value="INTERNATIONAL_DISTRIBUTION_FREIGHT"></option>
                                                                                            <option value="INTERNATIONAL_ECONOMY"></option>
                                                                                            <option value="INTERNATIONAL_ECONOMY_DISTRIBUTION"></option>
                                                                                            <option value="INTERNATIONAL_ECONOMY_FREIGHT"></option>
                                                                                            <option value="INTERNATIONAL_FIRST"></option>
                                                                                            <option value="INTERNATIONAL_PRIORITY"></option>
                                                                                            <option value="INTERNATIONAL_PRIORITY_DISTRIBUTION"></option>
                                                                                            <option value="INTERNATIONAL_PRIORITY_FREIGHT"></option>
                                                                                            <option value="PRIORITY_OVERNIGHT"></option>
                                                                                            <option value="STANDARD_OVERNIGHT"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDeclaredValue" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Declared Value :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtFedexDeclaredValue" runat="server" Width="60px">
                                                                                        </asp:TextBox>
                                                                                        $USD
                                                                                        <asp:RequiredFieldValidator ID="reqFedexDeclaredValue" runat="server" ControlToValidate="txtFedexDeclaredValue"
                                                                                            ErrorMessage="Declared Value is required." SetFocusOnError="True" ToolTip="Declared Value is required.">*</asp:RequiredFieldValidator></td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexdropofftype" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        DropOff Type :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <select id="ddlFedexDropofftype" runat="server">
                                                                                            <option value="BUSINESS_SERVICE_CENTER"></option>
                                                                                            <option value="DROP_BOX"></option>
                                                                                            <option value="REGULAR_PICKUP"></option>
                                                                                            <option value="REQUEST_COURIER"></option>
                                                                                            <option value="STATION"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divFedexCheck" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        &nbsp;</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <input id="chkFedexResidence" runat="server" name="shipToResidence" type="checkbox"
                                                                                            value="on" />&nbsp;Ship to residence
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divUpsPackagingType" style="display: none">
                                                                            <table style="height: 100%; width: 100%; vertical-align: top">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Packaging Type :</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <select id="ddlUPSPackagingType" runat="server">
                                                                                            <option value="01">UPS LETTER</option>
                                                                                            <option value="02">PACKAGE / CUSTOMER SUPPLIED</option>
                                                                                            <option value="03">UPS TUBE</option>
                                                                                            <option value="04">UPS PAK</option>
                                                                                            <option value="21">EXPRESS BOX</option>
                                                                                            <option value="24">25 KG BOX</option>
                                                                                            <option value="25">10 KG BOX</option>
                                                                                            <option value="30">PALLET</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divUPSServiceType" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Service Type :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlUPSServiceTypes" runat="server" onchange="CheckShipmentNames();">
                                                                                            <option value="01">EUROPE_FIRST_INTERNATIONAL_PRIORITY</option>
                                                                                            <option value="02">SECOND DAY AIR</option>
                                                                                            <option value="03">GROUND </option>
                                                                                            <option value="12">3 DAY SELECT</option>
                                                                                            <option value="13">NEXT DAY AIR SAVER</option>
                                                                                            <option value="14">NEXT DAY AIR EARLY AM</option>
                                                                                            <option value="11">STANDARD</option>
                                                                                            <option value="07">WORLDWIDE EXPRESS</option>
                                                                                            <option value="54">WORLDWIDE EXPRESS PLUS</option>
                                                                                            <option value="08">WORLDWIDE EXPRESS EXPEDITED</option>
                                                                                            <option value="65">SAVER</option>
                                                                                            <option value="82">UPS TODAY STANDARD</option>
                                                                                            <option value="83">UPS TODAY DEDICATED COURIER</option>
                                                                                            <option value="84">UPS TODAY INTERCITY</option>
                                                                                            <option value="86">UPS TODAY EXPRESS SAVER</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divUPSPickUp" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        PickUp Type :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlUPSPickUp" runat="server">
                                                                                            <option value="01">DAILY PICKUP</option>
                                                                                            <option value="03">CUSTOMER COUNTER</option>
                                                                                            <option value="06">ONE TIME PICKUP</option>
                                                                                            <option value="07">ON CALL Pickup</option>
                                                                                            <option value="19">LETTER CENTER</option>
                                                                                            <option value="20">AIR SERVICE CENTER</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLShipdate" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Ship Date:</td>
                                                                                    <td align="left" class="mbodyb" style="width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLDate" runat="server" ReadOnly="true">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLShipReference" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Shipper Reference :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLShipReference" runat="server"  Width="60px">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="reqDHLShipreference" runat="server" ControlToValidate="txtDHLShipReference"
                                                                                            ErrorMessage="Ship Reference is required." SetFocusOnError="True" ToolTip="Ship Reference is required.">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHlContentDesc" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Content Description :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLContentDescription" runat="server"  Width="60px">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="reqDHLContentDesc" runat="server" ControlToValidate="txtDHLContentDescription"
                                                                                            ErrorMessage="Content Description is required." SetFocusOnError="True" ToolTip="Content Description is required.">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLServices" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Service Type :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLServiceType" runat="server">
                                                                                            <option value="E">Express</option>
                                                                                            <option value="E1030">Express 10:30 A.M</option>
                                                                                            <option value="ESAT">Express Saturday</option>
                                                                                            <option value="N">Next Afternoon</option>
                                                                                            <option value="S">Second Day Service</option>
                                                                                            <option value="G">Ground</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLCODCode" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Collect On Delivary (COD) Payement Type:</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLCODCode" runat="server">
                                                                                            <option selected="selected" value="M">Cashier's Check or Money Order</option>
                                                                                            <option value="P">Personal or Company Check</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLCODValue" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Collect on Delivery (COD) Payement Value :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHlCODValue" runat="server"  Width="60px">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="reqDHLCODValue" runat="server" ControlToValidate="txtDHlCODValue"
                                                                                            ErrorMessage="COD Value is required." SetFocusOnError="True" ToolTip="COD Value is required.">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLShipmentType" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Shipment Type :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLShipmentType" runat="server">
                                                                                            <option value="P">Package</option>
                                                                                            <option value="L">Letter</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLAdditonalProtection" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Additional Protection:</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLAdditionalProtection" runat="server" onchange="CheckDHLValues();">
                                                                                            <option selected="selected" value="NR">Not Required</option>
                                                                                            <option value="AP">Asset Protection</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLDeclaredValue" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Declared Value :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLDeclaredValue" runat="server"  Width="60px">
                                                                                        </asp:TextBox>$USD
                                                                                        <asp:RequiredFieldValidator ID="reqDHLDeclaredValue" runat="server" ControlToValidate="txtDHLDeclaredValue"
                                                                                            ErrorMessage="Declared Value is required." SetFocusOnError="True" ToolTip="Declared Value is required.">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLBilling" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Billing:</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLBilling" runat="server" onchange="CheckDHLBillAcctNoValues();">
                                                                                            <option selected="selected" value="S">Sender</option>
                                                                                            <option value="R">Receiver</option>
                                                                                            <option value="3">Third Party</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLBillAcctNo" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Billing Account Number :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHlBillAcctNo" runat="server"  Width="180px">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="reqDHLAcctNo" runat="server" ControlToValidate="txtDHlBillAcctNo"
                                                                                            ErrorMessage="Acoount Number is required." SetFocusOnError="True" ToolTip="Acoount Number is required.">*</asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLShipmentImage" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        Shipment Image:</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <select id="ddlDHLImage" runat="server">
                                                                                            <option value="PNG">PNG</option>
                                                                                            <option value="GIF">GIF</option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLCustomerTransTraceID" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        DHL Customer Transaction Trace :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLCustomerTranTraceID" runat="server"  Width="120px">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="width: 100%" valign="top">
                                                                        <div id="divDHLDemographic" style="display: none">
                                                                            <table style="height: 100%; width: 100%">
                                                                                <tr>
                                                                                    <td align="Right" class="mbodyb" style="width: 30%" valign="middle">
                                                                                        DHL Customer Transaction Demographic :</td>
                                                                                    <td align="left" class="mbodyb" style="height: 17px; width: 70%" valign="top">
                                                                                        <asp:TextBox ID="txtDHLDemographic" runat="server"  Width="180px">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="width: 100%" valign="top">
                                                        <div id="divRatesBtn" style="display: none">
                                                            <table id="Table1" style="height: 100%; width: 100%" runat="server">
                                                                <tr>
                                                                    <td align="center" colspan="4" style="height: 100%; width: 100%" valign="top">
                                                                        <asp:ImageButton runat="server" ID="btnRates" BorderStyle="none" OnClick="All_Click"
                                                                             Width="56" Height="20" OnClientClick="javascript:return ButtonNames(this)" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divRatesGrdBtn" style="display: none">
                                                            <table style="height: 100%; width: 100%">
                                                                <tr>
                                                                    <td align="center" colspan="4" style="height: 100%; width: 50%" valign="top">
                                                                        <asp:ImageButton runat="server" ID="btnRatesboth" BorderStyle="none" OnClick="All_Click"
                                                                             Width="56" Height="20" OnClientClick="javascript:return ButtonNames(this)"  CausesValidation="false"/>
                                                                    </td>
                                                                    <td align="center" colspan="4" style="height: 100%; width: 50%" valign="top">
                                                                        <asp:ImageButton ID="btnShipboth" runat="server" BorderStyle="None" OnClick="All_Click"
                                                                             OnClientClick="javascript:return ButtonNames(this)" CausesValidation="false"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divShpBtn" style="display: none">
                                                            <table style="height: 100%; width: 100%">
                                                                <tr>
                                                                    <td align="center" colspan="4" style="height: 100%; width: 100%" valign="top">
                                                                        <asp:ImageButton ID="btnShip" runat="server" BorderStyle="None" OnClick="All_Click"
                                                                            OnClientClick="javascript:return ButtonNames(this)" CausesValidation="false"/>
                                                                    </td>
                                                                    <td>
                                                                        <input type="hidden" runat="server" id="hidButtonValues" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div runat="server" id="divLblExceptions" style="display: none">
                                                            <asp:Label runat="server" ID="lblExceptions" Visible="true"></asp:Label></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">
                <div id="divTracking" style="display: none;">
                    <asp:Panel ID="pnlTrackingDetails" runat="server" Width="100%">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="formmiddle"
                            style="height: 100%; cursor: pointer; border-right-width: 1px; border-right-style: double;
                            border-right-color: #d7e0f1; border-left-width: 1px; border-left-style: double;
                            border-left-color: #d7e0f1;">
                            <tr style="height: 24px;">
                                <td class="grdVwCurveLeft" style="width: 5px;">
                                </td>
                                <td id="Td9" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                    border-width: 0px; width: 100px">
                                    Tracking Details
                                </td>
                                <td class="grdVwCurveRight" style="width: 6px;">
                                </td>
                                <td id="htcTrackingDetails" runat="server" class="grdVwTitleAuto" style="height: 24px;"
                                    width="717px">
                                </td>
                                <td align="center" class="grdVwTitleAuto" style="height: 24px; width: 20px">
                                </td>
                                <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                    <img id="imgTrackingDetailsExpand" alt="expandCollapse" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlTrackingDetailsContent">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="formmiddle"
                            style="height: 10%; cursor: pointer; border-right-width: 1px; border-right-style: double;
                            border-right-color: #d7e0f1; border-left-width: 1px; border-left-style: double;
                            border-left-color: #d7e0f1;">
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 100%; width: 100%" valign="top" class="mbodyb">
                                    Tracking Number :
                                    <asp:TextBox ID="txtTracking" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 100%; width: 100%" valign="top">
                                    <asp:ImageButton ID="btnTrack" runat="server" BorderStyle="None" OnClick="All_Click"
                                         OnClientClick="javascript:return ButtonNames()" /></td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <%--<Ajax:CollapsiblePanelExtender ID="cpeShipperDetials" runat="server" TargetControlID="pnlShipperInformationDetails"
                    ExpandControlID="pnlShipperDetails" CollapseControlID="pnlShipperDetails" Collapsed="false"
                    ImageControlID="imgShipperDetails" SuppressPostBack="True" ScrollContents="false"
                    CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif" ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>
                <Ajax:CollapsiblePanelExtender ID="cpeCredentialDetails" runat="server" TargetControlID="pnlCredentialInformationDetails"
                    ExpandControlID="pnlCredentialInformation" CollapseControlID="pnlCredentialInformation"
                    Collapsed="false" ImageControlID="imgCPGV1Expand" SuppressPostBack="True" ScrollContents="false"
                    CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif" ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>
                <Ajax:CollapsiblePanelExtender ID="cpeShipFrom" runat="server" TargetControlID="pnlShipFromDetails"
                    ExpandControlID="pnlShipFrom" CollapseControlID="pnlShipFrom" Collapsed="false"
                    ImageControlID="imgShipFrom" SuppressPostBack="True" ScrollContents="false" CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif"
                    ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>
                <Ajax:CollapsiblePanelExtender ID="cpeShipTo" runat="server" TargetControlID="pnlShipToDetails"
                    ExpandControlID="pnlShipTo" CollapseControlID="pnlShipTo" Collapsed="false" ImageControlID="imgShipTo"
                    SuppressPostBack="True" ScrollContents="false" CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif"
                    ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>
                <Ajax:CollapsiblePanelExtender ID="cpePackageDetails" runat="server" TargetControlID="pnlPackageInformationDetails"
                    ExpandControlID="pnlPackageDetails" CollapseControlID="pnlPackageDetails" Collapsed="false"
                    ImageControlID="imgPackageDetails" SuppressPostBack="True" ScrollContents="false"
                    CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif" ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>
                <Ajax:CollapsiblePanelExtender ID="cpeTracking" runat="server" TargetControlID="pnlTrackingInformation"
                    ExpandControlID="pnlTrackingDetails" CollapseControlID="pnlTrackingDetails" Collapsed="false"
                    ImageControlID="imgPackageDetails" SuppressPostBack="True" ScrollContents="false"
                    CollapsedImage="~/App_Themes/LAjit/Images/add_symbol.gif" ExpandedImage="~/App_Themes/LAjit/Images/minus-icon.png">
                </Ajax:CollapsiblePanelExtender>--%>
            </td>
            <td>
                <input type="hidden" runat="server" id="hid_IDS" />
                <input type="hidden" runat="server" id="hid_ID_Values" />
                <input type="hidden" runat="server" id="hidGridValues" />
                <input type="hidden" runat="server" id="hidLabelValues" />
                <input type="hidden" runat="server" id="hidGrdSelectedValues" />
                <!--For Posting the data to the next form(Stores the current Calling Object BPInfo) -->
                <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" /></td>
        </tr>
    </table>
    </asp:Panel>  
</asp:Content>
