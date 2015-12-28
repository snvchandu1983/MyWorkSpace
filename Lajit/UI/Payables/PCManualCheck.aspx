<%@ Page Language="C#"  AutoEventWireup="true" Codebehind="PCManualCheck.aspx.cs" Inherits="LAjitDev.Payables.PCManualCheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <contenttemplate>
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
                                        <asp:Panel ID="pnlGVContent" runat="server" align="left">
                                              <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <!--EntryForm tr-->                                                                
                                <tr align="left">
                                   <td class="tdEntryFrm">
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                            <!-- entry form start class="formmiddle" -->
                                            <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" >
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
                                                <tr style="height: 24px;" id="trProcessLinks" align="right" valign="middle" runat="server">                                                                                                                             
                                                    <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server">                                                                    
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" visible="false" style="height:24px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="top" style="height:439px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td colspan="2" align="center">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                               <td style="height:2px;">
                                                                 <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5" height="1" />
                                                               </td>
                                                            </tr>    
                                                            <tr>
                                                                <td valign="top" colspan="2">
                                                                 <!--PC ManualCheck Start  -->
                                                                   <table cellpadding="0" width="100%" border="0" cellspacing="0" class="formcheckborder">
                                                                        <tr>
                                                                           <td style="height:1px">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                           <td>
                                                                             <!-- Table Autofill and  check no start SkinID="Label" LabelBig -->
                                                                              <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                              <tr>
                                                                                 <td align="left">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                      <tr align="left" valign="middle" id="trEntryBank_JournalDoc" runat="server">
                                                                                          <td align="left" class="formtext" style="height:24px; width:100px;">
                                                                                                <asp:Label  runat="server" ID="lblEntryBank_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                          </td>
                                                                                          <td valign="middle" style="width:388px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryBank_JournalDoc" runat="server" MapXML="EntryBank" MapBranchNode="JournalDoc" Width="376px"  ></LCtrl:LAjitDropDownList>
                                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank_JournalDoc" MapXML="EntryBank" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlEntryBank_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                         </td>                                                                                     
                                                                                       </tr>
                                                                                    </table>
                                                                                 </td>
                                                                                 <td align="right">
                                                                                     <table cellpadding="0" cellspacing="0" border="0">
                                                                                       <tr align="left" valign="top" id="trCheckNumber_JournalDoc" runat="server">
                                                                                                <td class="formtext" style="height:24px; width:100px;">
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
                                                                         <td>
                                                                          <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                              <tr>
                                                                                <td align="left">
                                                                                  <!-- SelectPCVendor start -->
                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                   <tr align="left" valign="middle" id="trAutoFillVendor_JournalDoc" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:100px;"  valign="top">
                                                                                            <asp:Label  runat="server" ID="lblSelectPCVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                         <td valign="middle" style="width:388px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlSelectPCVendor_JournalDoc" runat="server" MapXML="SelectPCVendor" MapBranchNode="JournalDoc" Width="376px"  ></LCtrl:LAjitDropDownList>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqSelectPCVendor_JournalDoc" MapXML="SelectPCVendor" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectPCVendor_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 

                                                                                         </td>         
                                                                                      </tr> 
                                                                                    </table>
                                                                                    <!-- SelectPCVendor end -->
                                                                                </td>                                                             
                                                                                <td align="right">
                                                                                 <!-- payment date -->
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                  <tr id="trPaymentDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                                    <td class="formtext" style="height: 24px; width:100px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblPaymentDate_JournalDoc"  SkinID="LabelBig"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle" style="width:136px; ">
                                                                                        <table border="0" cellpadding="0" cellspacing="0">
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
                                                                                <!-- payment date -->
                                                                             </td>
                                                                             </tr> 
                                                                          </table>           
                                                                         </td>
                                                                         
                                                                         
                                                                       </tr>
                                                                        <tr>
                                                                           <td style="height:1px">&nbsp;</td>
                                                                        </tr>
                                                                      
                                                                       <tr>
                                                                          <td>
                                                                             <!-- POReference & amount start -->
                                                                             <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                              <tr>
                                                                               <td align="left">
                                                                                  <!-- POReference start -->
                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                      <tr align="left" valign="top" id="trPOReference_JournalDoc" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:100px;" NOWRAP="NOWRAP">
                                                                                              <asp:Label  runat="server" ID="lblPOReference_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                         <td valign="middle" style="width:388px;">
                                                                                              <LCtrl:LAjitTextBox ID="txtPOReference_JournalDoc" runat="server" MapXML="POReference" MapBranchNode="JournalDoc" Width="370px"  ></LCtrl:LAjitTextBox>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqPOReference_JournalDoc" MapXML="POReference" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtPOReference_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>                                                                                     
                                                                                     </tr>
                                                                                  </table>
                                                                                  <!-- POReference end -->
                                                                               </td>
                                                                                <td  align="right">
                                                                                   <!-- control total start -->
                                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                                         <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                                            <td class="formtext" style="height: 24px; width:100px;" valign="top">
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
                                                                            <!-- POReference & amount end -->
                                                                          </td>
                                                                       </tr>
                                                                       <tr>
                                                                           <td style="height:20px">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                           <td align="center">                                                
                                                                            <!-- Payment Methods and Currency Start -->
                                                                             <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                             <tr>
                                                                               <td align="left">
                                                                                  <!-- payment methods start 196px 184px  216 204-->
                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                      <tr align="left" valign="top" id="trSelectAPInvoiceType_JournalDoc" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:100px;">
                                                                                              <asp:Label  runat="server" ID="lblSelectAPInvoiceType_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                         <td valign="middle" style="width:216px; ">
                                                                                              <LCtrl:LAjitDropDownList ID="ddlSelectAPInvoiceType_JournalDoc" runat="server" MapXML="SelectAPInvoiceType" MapBranchNode="JournalDoc" Width="204px"  ></LCtrl:LAjitDropDownList>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqSelectAPInvoiceType_JournalDoc" MapXML="SelectAPInvoiceType" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectAPInvoiceType_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>                                                                                     
                                                                                     </tr>
                                                                                  </table>
                                                                                  <!-- payment methods end -->
                                                                               </td>
                                                                               <td align="right">
                                                                                 <!-- Currency Start -->         
                                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                                       <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                                            <td  class="formtext" style="height: 24px; width:100px;" valign="top">
                                                                                                <asp:Label   runat="server" ID="lblCurrencyTypeCompany" SkinID="LabelBig" ></asp:Label>
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
                                                                            <!-- Check Messsage and Currency End -->
                                                                           </td>  
                                                                        </tr>
                                                                        <tr>
                                                                           <td style="height:20px" align="center">
                                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                             <tr>
                                                                               <td align="center" width="65%">
                                                                               <!-- Check Message Start -->
                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                   <tr align="center" valign="middle" id="trCheckMessage_JournalDoc" runat="server">
                                                                                      <td style="width:236px" align="center">
                                                                                        <LCtrl:LAjitLabel ID="lblCheckMessage_JournalDoc_Value" MapBranchNode="JournalDoc"   runat="server" MapXML="CheckMessage" CssClass="mbodybig" ></LCtrl:LAjitLabel>
                                                                                      </td>
                                                                                    </tr>
                                                                                  </table>
                                                                                  <!-- Check Message Start -->
                                                                               </td>
                                                                             </tr>  
                                                                            </table>   
                                                                           </td>
                                                                        </tr>
                                                                      <tr>
                                                                        <td>
                                                                           <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                           <tr>
                                                                                        <td valign="top">
                                                                                           <!-- left table 2 start -->
                                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                            <tr>
                                                                                               <td class="formtext" style="height: 24px; width:60px" valign="top">
                                                                                                 <asp:Label  runat="server" ID="lblMemo" SkinID="LabelBig" Text="MEMO"></asp:Label>
                                                                                               </td>
                                                                                               <td colspan="2" align="left" style="height: 24px; width:500px">
                                                                                                 <!-- table Description start -->
                                                                                                     <table cellpadding="0" cellspacing="0" border="0" width="500px">
                                                                                                      <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                                        <td class="formtext" style="height: 24px; width:110px" valign="top">
                                                                                                            <asp:Label   runat="server" ID="lblDescription"  SkinID="LabelBig"></asp:Label> 
                                                                                                        </td>
                                                                                                        <td valign="middle" width="390px">
                                                                                                            <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description"  Width="337px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description"  runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                                        </td>
                                                                                                       </tr>
                                                                                                     </table>
                                                                                                  <!-- table Description end -->
                                                                                              </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                              <td style="height: 24px; width:60px">&nbsp;</td>
                                                                                               <td valign="top">
                                                                                                  <!-- table Inv Number start -->
                                                                                                     <table cellpadding="0" cellspacing="0" border="0">
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
                                                                                                     </table>
                                                                                                   <!-- table Inv Number end -->
                                                                                               </td>
                                                                                               <td valign="top">
                                                                                                  <!-- table Inv Date start -->
                                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr id="trInvDate" align="left" valign="middle" runat="server">
                                                                                                        <td class="formtext" style="height: 24px; width: 100px" valign="top">
                                                                                                            <asp:Label   runat="server" ID="lblInvDate"  SkinID="LabelBig"></asp:Label>
                                                                                                        </td>
                                                                                                         <td valign="middle" style="width:116px; ">
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
                                                                                           <!-- left table 2 end -->
                                                                                        </td>
                                                                                       <td valign="top" align="center">
                                                                                           <!-- right table 2 start -->
                                                                                           <table cellpadding="0" cellspacing="0" border="0"  style="margin-left:20px; margin-right:20px; margin-top:20px; margin-bottom:20px;">
                                                                                            <tr>
                                                                                               <td>&nbsp;</td>
                                                                                               <td>                                                                                 
                                                                                               </td>
                                                                                            </tr>
                                                                                           </table>
                                                                                           <!-- right table 2 end -->
                                                                                        </td>
                                                                                      </tr>
                                                                           </table>
                                                                        </td>
                                                                      </tr>
                                                              </table>    
                                                                 <!--PC ManualCheck End  -->
                                                                </td>
                                                            </tr>   
                                                            <tr>
                                                                <td colspan="2">&nbsp;</td>
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
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <!-- form entry fields end -->
                                                    </td>
                                                </tr>                                                
                                            </table>
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="1"  width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td><asp:Label ID="lblError" runat="server"  SkinID="LabelMsg"></asp:Label></td>
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
</asp:Content>