<%@ Page Language="C#" AutoEventWireup="true" Codebehind="VoidCheckHistory.aspx.cs" Inherits="LAjitDev.Payables.VoidCheckHistory"%>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
  
    <script language="javascript" type="text/javascript">

    function DoChildValidations(ddlInvoiceType,lblSelectBatch,ddlSelectBatch,txtVoidDate,chkVoidOrigDate)
    {
        var ddlSelectAPInvoiceType=document.getElementById(ddlInvoiceType);
        var ddlSelectBatch=document.getElementById(ddlSelectBatch);
        var lblSelectBatch=document.getElementById(lblSelectBatch);
        var trxID = ddlSelectAPInvoiceType.value.split('~')[0];
        if(trxID>50)
        {
            ddlSelectBatch.style.display="";
            lblSelectBatch.style.display="";
        }
        else
        {
            ddlSelectBatch.style.display="none";
            lblSelectBatch.style.display="none";
        }
    }
        
    function ValidateVoids()
    {
        var ddlSelectAPInvoiceType=document.getElementById("ctl00_cphPageContents_ddlSelectAPInvoiceType_JournalDoc");
        var trxID = ddlSelectAPInvoiceType.value.split('~')[0];
        if(trxID<70 && trxID!=-1)
        {
            //Blink
        }
        else
        {
            //Stop Blinking.
            ClearBlinkObjects();
            return;
        }
        var txtVoidDate = document.getElementById("ctl00_cphPageContents_txtVoidDate_JournalDoc");
        var chkVoidOrigDate = document.getElementById("ctl00_cphPageContents_chkVoidOrigDate_JournalDoc");
        if(txtVoidDate.value.length>0 || chkVoidOrigDate.checked==true)
        {
            ClearBlinkObjects();
        }
        else
        {
            document.getElementById("chkVoidOrigDate").style.border="1px";
            document.getElementById("chkVoidOrigDate").style.borderStyle="solid";
            controlsToBlink[0] = txtVoidDate.id;
            controlsToBlink[1] = "chkVoidOrigDate";
            blinkElements();
        }
    }
    
    function ClearBlinkObjects()
    {
        var chkSpan=document.getElementById("chkVoidOrigDate");
        chkSpan.style.border="";
        chkSpan.style.borderStyle="";
        chkSpan.style.borderColor="";

        var txtVoidDate=document.getElementById("ctl00_cphPageContents_txtVoidDate_JournalDoc");
        txtVoidDate.style.border="";
        txtVoidDate.style.borderStyle="";
        txtVoidDate.style.borderColor="";

        controlsToBlink = new Array();
    }
        
    //Updates the hidden variable hdnGVBPEINFO in BtnsUC with changed one.
    function UpdateBPCLinkXML(obj,branchName,attKey)
    {
        ValidateVoids();   
        var COXML ="" ;
        var hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnGVBPEINFO');
        if(hdnGVBPEINFO)
        {
            COXML=hdnGVBPEINFO.value;                   
        }
        else
        {
            if(g_IsDebugging)
            {
                alert('No BPC Link XML containers found.\nRaised by UpdateBPCLinkXML() in VoidCheckHistory.aspx');
                return;
            }
        }
        var xDocBPC=loadXMLString("<Root>" + COXML + "</Root>");
        var nodeBranch=xDocBPC.getElementsByTagName(branchName);
        if(nodeBranch&&nodeBranch.length>0)
        {
            nodeBranch=nodeBranch[0];
        }
        else
        {
            //No Branch node present so create it..
        }
        var nodeRow=nodeBranch.getElementsByTagName("Rows")[0];
        if(obj.nodeName=="SELECT")//DropDownList
        {
            var trxID = obj.value.split('~')[0];
            var trxType = obj.value.split('~')[1];
            if(trxID ==-1||trxID =="")
            {
                nodeRow.removeAttribute(attKey+'_TrxID');
                nodeRow.removeAttribute(attKey+'_TrxType');
            }
            else
            {    
                nodeRow.setAttribute(attKey+'_TrxID',trxID);
                nodeRow.setAttribute(attKey+'_TrxType',trxType);
            }
        }
        else if(obj.nodeName == "SPAN")//Checkbox with an attibute
        {
            var childObj = GetSpanChild(obj);
            if(childObj.type == "checkbox")
            {
                if(childObj.checked==true)
                {
                    nodeRow.setAttribute(attKey,"1");
                }
                else
                {
                    nodeRow.setAttribute(attKey,"0");
                }
            }
        }
        else
        {
            var attValue = obj.value;
            nodeRow.setAttribute(attKey,attValue);
        }
        var updatedXML=ConvertToString(xDocBPC).replace("<Root>","").replace("</Root>","");//Get rid of the Root tag.
        document.getElementById('ctl00_cphPageContents_BtnsUC_hdnGVBPEINFO').value=updatedXML;
        document.getElementById('ctl00_cphPageContents_GVUC_hdnGVBPEINFO').value=updatedXML;
    }
    
    window.onload = EnableMemoControls;
 
    function EnableMemoControls()
    {
        document.getElementById("ctl00_cphPageContents_ddlSelectAPInvoiceType_JournalDoc").disabled="";
        document.getElementById("ctl00_cphPageContents_ddlSelectAPInvoiceType_JournalDoc").style.backgroundColor="white";

        document.getElementById("ctl00_cphPageContents_txtVoidDate_JournalDoc").disabled="";
        document.getElementById("ctl00_cphPageContents_txtVoidDate_JournalDoc").style.backgroundColor="white";

        document.getElementById("ctl00_cphPageContents_chkVoidOrigDate_JournalDoc").disabled="";
        document.getElementById("ctl00_cphPageContents_chkVoidOrigDate_JournalDoc").style.backgroundColor="white";

        document.getElementById("ctl00_cphPageContents_ddlSelectBatch_JournalDoc").disabled="";
        document.getElementById("ctl00_cphPageContents_ddlSelectBatch_JournalDoc").style.backgroundColor="white";
    }
    </script>
    
    <script type="text/javascript">
    function JQueryPageEvents(){
   
       //Child grid in panel
        jQuery('#ctl00_cphPageContents_pnlCPGV2Title').click(
        function()
        {
            ExpandCollapse(this.id);
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
                   jQuery("#"+oImgID).attr("src", g_cdnImagesPath+"Images/minus-icon.png"); 
               }
               else
               {
                   jQuery("#"+oImgID).attr("src",g_cdnImagesPath+"Images/plus-icon.png"); 
               }
            }  
    }
    </script>
   
    
    <asp:Panel ID="pnlContent" runat="server" >
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <contenttemplate>
              <script type="text/javascript" >
                   Sys.Application.add_load(JQueryPageEvents);
              </script> 
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
                    <tr>
                        <!--Btns Usercontrol-->
                       <td class="tdBtnsUC">
                            <asp:Panel ID="pnlBtnsContent" runat="server" align="left">
                                <BtnsUC:BtnsUserControl ID="BtnsUC" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="width: 5px;">
                            <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                height="1" />
                        </td>
                        <!--GV-->
                       <td class="tdMainCont">
                            <table class="tblMainCont" cellpadding="0" cellspacing="0" >
                            <!--Title tr-->
                                    <tr>
                                        <td>
                                        <asp:Panel ID="pnlCPGV1Title" runat="server" Width="100%">
                                            <table cellpadding="0" cellspacing="0" class="tblFormTitle">
                                            <tr style="height: 24px;">
                                                <td class="grdVwCurveLeft">
                                                    <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                                                </td>
                                                <td id="htcCPGV1" runat="server" class="grdVwtitle">
                                                </td>
                                                <td class="grdVwCurveRight">
                                                    <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                                                </td>
                                                <td id="htcCPGV1Auto" class="grdVwTitleAuto" runat="server">
                                                </td>
                                                <td class="grdVwTitleAuto" style="width: 20px" align="center">
                                                    <img id="imgCPGV1Expand" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                        </td>
                                    </tr>
                                <!--GV tr-->
                                <tr align="left">
                                    <td valign="top">
                                        <!--Panel CPGV1 Content-->
                                        <asp:Panel ID="pnlGVContent" runat="server" align="left" >
                                           <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <!--EntryForm tr-->                                                                
                                <tr align="left">
                                    <td class="tdEntryFrm">
                                     <asp:Panel ID="pnlEntryForm" runat="server"  height="511px">
                                            <!-- entry form start -->
                                         <!-- entry form start -->
                                            <table id="tblEntryForm" runat="server" style="height: 511px;width:100%"  border="0" cellspacing="0" cellpadding="0">
                                                <!--Pop up header tr-->
                                                <tr style="height:24px;" runat="server" id="trPopupHeader" visible="false" valign="top">
                                                    <td> 
                                                       <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                        cursor: hand; border-right-width: 1px; border-right-style: double;  border-right-color: #d7e0f1;">
                                                            <tr style="height: 24px;">
                                                                <td id="htcPopEntryForm" runat="server" align="center" class="grdVwRPTitle" style="height: 24px; border-width: 0px">
                                                                    <asp:Label id="lblPopupEntry" runat="server" height="24px"></asp:Label>
                                                                </td>                                                           
                                                            </tr>
                                                        </table>
                                                    </td> 
                                                </tr>
                                                <!--ProcessLinks tr-->
                                                 <tr style="height: 10px;" id="trProcessLinks" align="right" valign="middle" runat="server">                                                                                                                             
                                                    <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">                                                                    
                                                        </asp:Panel>
                                                    </td>
                                                 </tr>
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" visible="false" style="height:10px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label  SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>   
                                                
                                                 <tr>
                                                <td>
                                                 <!-- Quick Check Start  -->
                                                   <table cellpadding="0" width="100%" border="0" cellspacing="0" class="formcheckborder" >
                                                        <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                           <td valign="top">
                                                             <!-- Table Autofill and  check no start SkinID="Label" LabelBig -->
                                                              <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                              <tr>
                                                                 <td align="left" style="width:70%">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                      <tr align="left" valign="middle" id="trEntryBank_JournalDoc" runat="server">
                                                                          <td align="left" class="formtext" style="height:24px; width:100px;">
                                                                                <asp:Label  runat="server" ID="lblEntryBank_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                          </td>
                                                                          <td valign="left" style="width:216px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryBank_JournalDoc" runat="server" MapXML="EntryBank" MapBranchNode="JournalDoc" Width="204px"  ></LCtrl:LAjitDropDownList>
                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank_JournalDoc" MapXML="EntryBank" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlEntryBank_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                         </td>                                                                                     
                                                                       </tr>
                                                                    </table>
                                                                 </td>
                                                                <td  align="right" style="width:30%">
                                                                     <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr align="left" valign="top" id="trCheckNumber_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height:24px; width:121px;">
                                                                                         <asp:Label  runat="server" ID="lblCheckNumber_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:136px; ">
                                                                                        <LCtrl:LAjitTextBox ID="txtCheckNumber_JournalDoc" runat="server" MapXML="CheckNumber" MapBranchNode="JournalDoc" Width="68px"></LCtrl:LAjitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqCheckNumber_JournalDoc" MapXML="CheckNumber" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtCheckNumber_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>                                                                                     
                                                                            </tr>
                                                                     </table> 
                                                                 </td>   
                                                              </tr>
                                                              </table>
                                                             <!-- Table Autofill and  check no end -->
                                                           </td>
                                                        </tr>
                                                       <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                       <tr>
                                                          <td valign="top">
                                                          <!-- pyament date -->
                                                          <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                           <tr>
                                                             <td style="width:70%">
                                                              <!-- auto fill start -->
                                                                 <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr id="trVendor_JournalDoc" align="left" valign="top" runat="server">
                                                                                <td class="formtext" style="height:24px;width:100px;"  valign="top">
                                                                                    <asp:Label   runat="server" ID="lblVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                               <td valign="middle" style="width:386px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtVendor_JournalDoc" runat="server" MapXML="Vendor" MapBranchNode="JournalDoc" Width="360px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendor_JournalDoc" MapXML="Vendor" runat="server" MapBranchNode="JournalDoc" ControlToValidate="txtVendor_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>     
                                                                      
                                                                    </table>
                                                                    <!-- auto fill end -->
                                                             
                                                             </td>
                                                             <td valign="top" align="right" style="width:30%">
                                                              <!-- payment date start-->
                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                      <tr id="trPaymentDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width:121px" valign="top">
                                                                            <asp:Label runat="server" ID="lblPaymentDate_JournalDoc"  SkinID="LabelBig"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width:136px; ">
                                                                           <table cellpadding="0" cellspacing="0" border="0">
                                                                           <tr>
                                                                            <td>
                                                                               <LCtrl:LajitTextBox ID="txtPaymentDate_JournalDoc" runat="server" MapXML="PaymentDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                            </td>
                                                                            <td>
                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentDate_JournalDoc" MapXML="PaymentDate" runat="server" ControlToValidate="txtPaymentDate_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                             </td>
                                                                           </tr>
                                                                           </table>                  
                                                                        </td>
                                                                     </tr>
                                                                   </table>
                                                              <!-- payment date  end-->
                                                             </td>
                                                           </tr>
                                                          </table>
                                                         <!-- pyament date -->
                                                         </td>
                                                       </tr>
                                                        <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                      
                                                       <tr>
                                                          <td valign="top">
                                                             <!-- AutoFill vendor amount start -->
                                                             <table cellpadding="0" cellspacing="0" width="100%" border="0" bordercolor="red">
                                                              <tr>
                                                                <td style="width:70%" align="center">
                                                                 <!-- Check Messsage and Currency Start -->
                                                                      <!-- Check Message Start -->
                                                                      <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr align="center" valign="middle" id="trCheckMessage_JournalDoc" runat="server">
                                                                          <td style="width:236px" align="center">
                                                                             <%-- <asp:Label ID="lblDummmyText" runat="server" SkinID="LabelBig" Text="Display Only Text" ></asp:Label>--%>
                                                                            <LCtrl:LAjitLabel ID="lblCheckMessage_JournalDoc_Value" MapBranchNode="JournalDoc"   runat="server" MapXML="CheckMessage" ></LCtrl:LAjitLabel>
                                                                          </td>
                                                                        </tr>
                                                                      </table>
                                                                  <!-- Check Message Start -->
                                                                
                                                                
                                                                </td>
                                                                <td style="width:30%">
                                                                   <!-- control total start -->
                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                         <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width:121px;" valign="top">
                                                                                <asp:Label runat="server" ID="lblControlTotal" text="$" SkinID="LabelBig"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:136px; ">
                                                                                <LCtrl:LajitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtControlTotal" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal" MapXML="ControlTotal"
                                                                                ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="Value should be numeric"
                                                                                Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>        
                                                                            </td>
                                                                        </tr>  
                                                                   </table>
                                                                   <!-- control total end -->
                                                                </td>
                                                              </tr>
                                                             </table> 
                                                                 <!-- AutoFill vendor amount end -->
                                                             </td>
                                                       </tr>
                                                       <tr>
                                                           <td style="height:2px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                          <td>
                                                            <!-- MEMO start -->
                                                           <table cellpaddin="0" cellpadding="0" border="0" width="100%" bordercolor="blue">
                                                            <tr>
                                                               <td  valign="top"  align="left"  style="width:70%">                                                
                                                               <table cellpadding="0" cellspacing="0"  border="0" bordercolor="yellow">
                                                                 <tr>
                                                                    <td valign="top" Style="width:5%">
                                                                           <!-- left table 2 start -->
                                                                           <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                               <td class="formtext" style="height: 24px; width:60px" valign="top">
                                                                                 <asp:Label  runat="server" ID="lblMemo" SkinID="LabelBig" Text="MEMO"></asp:Label>
                                                                               </td>
                                                                               <td valign="top">
                                                                                 &nbsp;
                                                                               </td>
                                                                            </tr>
                                                                           </table>
                                                                           <!-- left table 2 end -->
                                                                     </td>
                                                                     <td Style="width:65%">
                                                                       <!-- Right table with border start class="formcheckborder" style="margin-left:2px;margin-right:2px;margin-top:2px;margin-bottom:2px"-->
                                                                        <table cellpadding="3"  cellspacing="0" width="100%" border="0" class="formcheckborder">
                                                                           <tr>
                                                                            <td >
                                                                               <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                                                                                <tr>
                                                                                    <td style="width:100%" colspan="2">
                                                                                   <!-- SelectApInvoiceType start -->
                                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                                             <tr align="left" valign="top" id="trSelectAPInvoiceType_JournalDoc" runat="server">
                                                                                                <td class="formtext" style="height:24px; width:121px;" NOWRAP="NOWRAP">
                                                                                                      <asp:Label  runat="server" ID="lblSelectAPInvoiceType_JournalDoc"  SkinID="Label"></asp:Label>
                                                                                                </td>
                                                                                                 <td valign="middle" style="width:405px; ">
                                                                                                      <LCtrl:LAjitDropDownList ID="ddlSelectAPInvoiceType_JournalDoc" runat="server" MapXML="SelectAPInvoiceType" MapBranchNode="JournalDoc" Width="390px"  ></LCtrl:LAjitDropDownList>
                                                                                                      <LCtrl:LAjitRequiredFieldValidator ID="reqSelectAPInvoiceType_JournalDoc" MapXML="SelectAPInvoiceType" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectAPInvoiceType_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>                                                                                     
                                                                                            </tr>
                                                                                       </table>
                                                                                   <!-- Select ApInvoice end -->
                                                                                  </td>
                                                                                </tr>
                                                                              </table>
                                                                            </td>
                                                                             
                                                                           </tr>
                                                                           <tr>
                                                                             <td>
                                                                               <!-- Reversal Date and UseOriginalDates start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                 <tr>
                                                                                   <td style="width:55%">
                                                                                     <!-- VoidDate start-->
                                                                                           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                              <tr id="trVoidDate_JournalDoc" valign="top" align="left"  runat="server">
                                                                                                <td  class="formtext" style="height: 24px; width:132px" valign="top" NOWRAP="NOWRAP">
                                                                                                    <asp:Label runat="server" ID="lblVoidDate_JournalDoc"  SkinID="Label"></asp:Label>
                                                                                                </td>
                                                                                                <td valign="middle" style="width:194px; ">
                                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                                  <tr>
                                                                                                    <td>
                                                                                                    <LCtrl:LajitTextBox ID="txtVoidDate_JournalDoc" runat="server" MapXML="VoidDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                                     </td>
                                                                                                    <td>
                                                                                                      <LCtrl:LAjitRequiredFieldValidator ID="reqVoidDate_JournalDoc" MapXML="VoidDate" runat="server" ControlToValidate="txtVoidDate_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                                     </td>
                                                                                                  </tr>
                                                                                                  </table>                  
                                                                                                </td>
                                                                                             </tr>
                                                                                           </table>
                                                                                      <!-- VoidDate  end-->
                                                                                   </td>
                                                                                     <td style="width:45%">
                                                                                     <!-- VoidOrigDate start -->
                                                                                       <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                            <tr align="left" valign="middle" id="trVoidOrigDate_JournalDoc" runat="server">
                                                                                                <td align="right" valign="middle" class="formtext" style="height:24px; width:156px;" NOWRAP="NOWRAP">
                                                                                                     <asp:Label runat="server" ID="lblVoidOrigDate_JournalDoc" SkinID="Label"></asp:Label>
                                                                                                </td>
                                                                                                <td valign="middle" style="width:50px;">
                                                                                                   <LCtrl:LAjitCheckBox ID="chkVoidOrigDate_JournalDoc" runat="server"  MapXML="VoidOrigDate" />
                                                                                                </td>
                                                                                           </tr> 
                                                                                        </table>    
                                                                                      <!-- VoidOrigDate end -->
                                                                                   </td>
                                                                                 </tr>
                                                                                </table>
                                                                               <!-- Reversal Date and UseOriginalDates end -->
                                                                             </td>
                                                                           </tr>
                                                                           <tr>
                                                                             <td>
                                                                                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                                                                                 <tr>
                                                                                     <td style="width:100%">
                                                                                        <!--  SelectBatch start --> 
                                                                                       <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                             <tr align="left" valign="top" id="trSelectBatch_JournalDoc" runat="server">
                                                                                                <td class="formtext" style="height:24px; width:121px;" NOWRAP="NOWRAP">
                                                                                                      <asp:Label  runat="server" ID="lblSelectBatch_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                                </td>
                                                                                                 <td valign="middle" style="width:405px; ">
                                                                                                      <LCtrl:LAjitDropDownList ID="ddlSelectBatch_JournalDoc" runat="server" MapXML="SelectBatch" MapBranchNode="JournalDoc" Width="390px"></LCtrl:LAjitDropDownList>
                                                                                                      <LCtrl:LAjitRequiredFieldValidator  ID="reqSelectBatch_JournalDoc" MapXML="SelectBatch" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectBatch_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>                                                                                     
                                                                                            </tr>
                                                                                       </table>
                                                                                        <!--  SelectBatch end -->
                                                                                     </td>
                                                                                 </tr>
                                                                                </table>
                                                                             </td>
                                                                           </tr>
                                                                         </table>
                                                                       <!-- Right table with border end-->
                                                                     </td>
                                                                  </tr>
                                                                </table>
                                                               </td>
                                                               <td valign="top"  align="right" style="width:30%">
                                                                 <!-- Currency Start -->         
                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                            <td  class="formtext" style="height: 24px; width:121px;" valign="top">
                                                                                <asp:Label   runat="server" ID="lblCurrencyTypeCompany" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:136px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="100px"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>  
                                                                  </table>
                                                                  <!-- Currency Start -->
                                                               </td>
                                                            </tr>
                                                           </table>    
                                                           <!-- MEMO end -->
                                                           </td>     
                                                         </tr>
                                                      <!-- Invoice Date and -->
                                                      <tr>
                                                        <td>
                                                           <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                             <tr>
                                                                <td valign="top">
                                                                  <!-- table Inv Date start -->
                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr id="trInvDate" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 136px" valign="top">
                                                                            <asp:Label   runat="server" ID="lblInvDate"  SkinID="LabelBig"></asp:Label>
                                                                        </td>
                                                                         <td valign="middle" style="width:216px; ">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                            <td>
                                                                               <LCtrl:LajitTextBox ID="txtInvDate" runat="server" MapXML="InvDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                            </td>
                                                                            <td>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqInvDate" MapXML="InvDate" runat="server" ControlToValidate="txtInvDate" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                              </td>
                                                                            </tr>
                                                                            </table>           
                                                                        </td>
                                                                    </tr> 
                                                                  </table>
                                                                  <!-- table Inv Date end -->
                                                                </td>
                                                             </tr>
                                                           </table>
                                                        </td>
                                                      </tr>
                                                      <!-- Invoice Date -->
                                                      
                                                      
                                                      <!-- hidden fields start-->
                                                         
                                                          <tr id="trInvNumber" align="left" valign="middle" runat="server">
                                                                                            <td  class="formtext" style="height: 24px; width:100px" valign="top">
                                                                                               <asp:Label   runat="server" ID="lblInvNumber" SkinID="LabelBig" ></asp:Label>
                                                                                            </td>
                                                                                             <td valign="middle" style="width:116px; ">
                                                                                               <LCtrl:LajitTextBox ID="txtInvNumber" runat="server" MapXML="InvNumber" Width="100px"></LCtrl:LajitTextBox>
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqInvNumber" runat="server" MapXML="InvNumber" ControlToValidate="txtInvNumber" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                            </td>
                                                                                        </tr> 
                                                         
                                                         <tr id="trPOReference_JournalDoc" align="left" valign="middle" runat="server">
                                                            <td class="formtext" style="height: 24px; width: 136px">
                                                                <asp:Label   runat="server" ID="lblPOReference_JournalDoc" MapBranchNode="JournalDoc"  SkinID="Label"></asp:Label>
                                                            </td>
                                                             <td valign="middle" style="width:196px; ">
                                                                <LCtrl:LajitTextBox ID="txtPOReference_JournalDoc" runat="server" MapXML="POReference" MapBranchNode="JournalDoc" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPOReference_JournalDoc" MapXML="POReference" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtPOReference_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                            </td>
                                                         </tr>   
                                                         <tr id="trVendor" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 136px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblVendor"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtVendor" runat="server" MapXML="Vendor" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendor" MapXML="Vendor" runat="server" ControlToValidate="txtVendor" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                         <tr id="trJournalRef" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 136px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblJournalRef"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server" ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>   
                                                     <!-- hidden fields end-->                                                  
                                              </table>    
                                               <!-- Quick Check End -->
                                               </td>
                                           </tr>                                          
                                           <!-- Branch AccountingItem Start -->                                         
                                          <!-- Branch AccountingItem Collapse Start -->
                                        <tr align="center" valign="top" style="height:255px">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                                           <tr>
                                             <td>
                                               <!-- branch accounting item title and grid start-->
                                                <table cellpadding="0" cellspacing="0" border="0" style="width:100%" class="formmiddle">
                                                 <tr>
                                                   <td>
                                                     <asp:Panel ID="pnlCPGV2Title" runat="server" Height="100%" Width="100%">
                                                         <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" style="width: 100%; cursor: pointer; border-width: 1px;
                                                                            border-color: #d7e0f1; border-left-style: double; border-right-style: double;">
                                                                            <tr>
                                                                                 <td class="grdVwRPTitle" style="height: 24px; width: 20px">&nbsp;
                                                                                 </td>
                                                                                 <td class="grdVwRPTitle" style="height: 24px; width: 20px" align="center">
                                                                                    <img id="imgCPGV2TitleExpand" alt="curveRight" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                                                                 </td>
                                                                                <td id="htcHeaderAccountingItem" runat="server" align="center" class="grdVwRPTitle"
                                                                                    style="width: 100%">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                     </asp:Panel>
                                                    <asp:Panel ID="pnlCPGV2TitleContent" runat="server" Height="100%" Width="100%" HorizontalAlign="Center">
                                                    <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                  <td align="left">
                                                                        <asp:Panel ID="pnlGVBranch" runat="server" Width="100%" ScrollBars="horizontal"
                                                                            BorderColor="#d7e0f1">
                                                                            <asp:GridView runat="server" ID="grdVwAccountingItem" SkinID="BranchGVStyle">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkBxSelectRow" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr align="left">
                                                                    <td>
                                                                       <asp:Table ID="tblOperationLinks" runat="server">
                                                                        <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                <asp:LinkButton ID="lnkBtnAddNewRow" runat="server"  OnClick="lnkBtnAddNewRow_Click" OnClientClick="javascript:AddRowsClick('ctl00_cphPageContents_grdVwAccountingItem')" >Add More Records</asp:LinkButton>
                                                                            </asp:TableCell>
                                                                            <asp:TableCell  style="padding-left: 3px; padding-right: 3px;">
                                                                                |</asp:TableCell>
                                                                            <asp:TableCell>
                                                                                <asp:LinkButton ID="lnkBtnDeleteRow" runat="server" OnClientClick="javascript:DeleteRow('ctl00_cphPageContents_grdVwAccountingItem', this);return false;"
                                                                                    Text="Delete">
                                                                                </asp:LinkButton>
                                                                            </asp:TableCell>
                                                                            <asp:TableCell style="padding-left: 3px; padding-right: 3px;">
                                                                                |</asp:TableCell>
                                                                            <asp:TableCell>
                                                                                <asp:LinkButton ID="lnkBtnToggle" runat="server" OnClientClick="javascript:ToggleSelection('ctl00_cphPageContents_grdVwAccountingItem', this);return false;"
                                                                                    Text="Toggle Selection">
                                                                                </asp:LinkButton>
                                                                            </asp:TableCell>
                                                                        </asp:TableRow>
                                                                       </asp:Table>
                                                                    </td>
                                                                </tr>
                                                          </table>
                                                      </asp:Panel>
                                                   </td>
                                                 </tr>
                                                </table>
                                                <!-- branch accounting item title and grid end-->
                                             </td>
                                           </tr>
                                          
                                             <!--Submit and Cancel buttons-->                                                    
                                                  <tr style="height:24px">
                                                            <td colspan="2" align="center">
                                                                <table border="0">
                                                                   <tr>
                                                                        <td>
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click" OnClientClick="javascript:return ValidateControls()" 
                                                                              Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="ContinueAdd" />
                                                                        </td>
                                                                       <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="AddClone"/>
                                                                        </td> 
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel"/>
                                                                        </td>
                                                                       </tr>
                                                                    
                                                                </table>
                                                               </td>                                                                                                                                   
                                                             </tr>
                                                           <tr style="height:24px">
                                                                <td colspan="2" align="center">
                                                                   <table border="0">
                                                                     <tr>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnPrevious" Tooltip="Previous"/>
                                                                       </td>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnNext" Tooltip="Next"/>
                                                                       </td> 
                                                                     </tr> 
                                                                    </table>
                                                                  </td>
                                                             </tr> 
                                                    <tr>
                                                        <td colspan="2" valign="top">
                                                            <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                        </td>
                                                    </tr>
                                                <!--Submit and Cancel buttons-->
                                               </table>
                                           </td>
                                        </tr>
                                           <!-- Branch AccountingItem end -->                                                   
                                           </table>
                                                <!-- form entry fields end -->
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="0"  width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td><asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>                
          </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
