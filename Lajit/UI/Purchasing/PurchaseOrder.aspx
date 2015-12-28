<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PurchaseOrder.aspx.cs"
    Inherits="LAjitDev.Purchasing.PurchaseOrder" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
            <contenttemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
                    <tr>
                        <!--Btns Usercontrol-->
                        <td class="tdBtnsUC">
                                <BtnsUC:BtnsUserControl ID="BtnsUC" runat="server" />
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="700px">
                                            <!-- entry form start -->
                                            <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
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
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">                                                                    
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                      
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" visible="false" style="height:24px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label  SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="top" style="height:439px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <!-- SOX Approval Status tr-->
                                                             <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td valign="middle" colspan="2" align="center">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" 
                                                                   AlternateText="IsApproved"/>
                                                                </td>
                                                             </tr>
                                                             <tr>
                                                                <td valign="top" style="width: 50%;">
                                                                   <!-- Left Table Start -->
                                                                    <table cellpadding="0" border="0" cellspacing="0" >
                                                                          <tr align="left" valign="top" id="trAutoFillVendor_JournalDoc" runat="server">
                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                     <asp:Label  runat="server" ID="lblAutoFillVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitTextBox ID="txtAutoFillVendor_JournalDoc" runat="server" MapXML="AutoFillVendor" MapBranchNode="JournalDoc" Width="180px"  autocomplete="off"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAutoFillVendor_JournalDoc" MapXML="AutoFillVendor" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtAutoFillVendor_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>  
                                                                                     
                                                                            </td>                                                                                     
                                                                           </tr>
                                                                          <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                           </tr> 
                                                                          <tr id="trPurchOrderNumber" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblPurchOrderNumber"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtPurchOrderNumber" runat="server" MapXML="PurchOrderNumber" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPurchOrderNumber" MapXML="PurchOrderNumber" runat="server" ControlToValidate="txtPurchOrderNumber" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                          <tr id="trInvDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblInvDate"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:196px; ">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
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
                                                                          <tr id="trJournalRef" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblJournalRef"  SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server" ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                           </tr> 
                                                                          <tr align="left" valign="top" id="trVendPayTerm_JournalDoc" runat="server">
                                                                              <td class="formtext" style="height:24px; width: 141px;">
                                                                                     <asp:Label runat="server" ID="lblVendPayTerm_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                             </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlVendPayTerm_JournalDoc" runat="server" MapXML="VendPayTerm" MapBranchNode="JournalDoc" Width="184px"  ></LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendPayTerm_JournalDoc" MapXML="VendPayTerm" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlVendPayTerm_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                          </tr>
                                                                          <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width:141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblCurrencyTypeCompany" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>     
                                                                    </table>
                                                                   <!-- Left Table End -->      
                                                                </td>
                                                                <td valign="top" style="width: 50%;">
                                                                   <!-- Right Table Start -->
                                                                    <table cellpadding="0" cellspacing="0" border="0" >                                                                   
                                                                       <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblControlTotal"  SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" runat="server" ControlToValidate="txtControlTotal" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>  
                                                                                 <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal" MapXML="ControlTotal"
                                                                                 ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                 Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator> 
                                                                            </td>
                                                                       </tr>                                                                
                                                                       <tr id="trCreditCard_JournalDoc" align="left" valign="top" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px">
                                                                                <asp:Label runat="server" ID="lblCreditCard_JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitCheckBox ID="chkCreditCard_JournalDoc" runat="server" MapXML="CreditCard" />                                                                                                                                                       
                                                                            </td>
                                                                       </tr> 
                                                                       <tr align="left" valign="top" id="trSelectCreditCard_JournalDoc" runat="server">
                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label runat="server" ID="lblSelectCreditCard_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlSelectCreditCard_JournalDoc" runat="server" MapXML="SelectCreditCard" MapBranchNode="JournalDoc" Width="184px"  ></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCreditCard_JournalDoc" MapXML="SelectCreditCard" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectCreditCard_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>                                                                                     
                                                                        </tr>
                                                                       <tr id="trPrintPO_JournalDoc" align="left" valign="top" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px">
                                                                                <asp:Label runat="server" ID="lblPrintPO_JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitCheckBox ID="chkPrintPO_JournalDoc" runat="server" MapXML="PrintPO" />                                                                                                                                                       
                                                                            </td>
                                                                       </tr>
                                                                       <tr id="trIsRetired_JournalDoc" align="left" valign="top" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px">
                                                                                <asp:Label runat="server" ID="lblIsRetired_JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitCheckBox ID="chkIsRetired_JournalDoc" runat="server" MapXML="IsRetired" />                                                                                                                                                       
                                                                            </td>
                                                                       </tr>                                                                        
                                                                    </table>
                                                                   <!-- Right Table End -->      
                                                                </td>
                                                            </tr>
                                                            <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            
                                                            <!-- new check boxes start -->
                                                            <tr>
                                                              <td colspan="2">
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                    <tr>
                                                                      <td style="width:20%">
                                                                         <!--checkbox DepCheck -->
                                                                          <table cellpadding="0" cellspacing="0" border="0" >
                                                                             <tr id="trDepCheck_JournalDoc" align="left" valign="middle" runat="server">
                                                                                    <td class="formtext" style="height: 24px; width: 200px">
                                                                                        <asp:Label runat="server" ID="lblDepCheck_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                    </td>
                                                                                     <td valign="middle" style="width:30px;">
                                                                                        <LCtrl:LAjitCheckBox ID="chkDepCheck_JournalDoc" runat="server" MapXML="DepCheck" MapBranchNode="JournalDoc" />                                                                                                                                                       
                                                                                    </td>
                                                                               </tr>    
                                                                          </table>
                                                                           <!--checkbox DepCheck -->
                                                                      </td>
                                                                       <td style="width:20%">
                                                                          <!--checkbox PaymentCheck -->
                                                                          <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr id="trPaymentCheck_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 200px">
                                                                                    <asp:Label runat="server" ID="lblPaymentCheck_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:30px; ">
                                                                                    <LCtrl:LAjitCheckBox ID="chkPaymentCheck_JournalDoc" runat="server" MapXML="PaymentCheck" MapBranchNode="JournalDoc" />                                                                                                                                                       
                                                                                </td>
                                                                           </tr>    
                                                                           </table>
                                                                          <!--checkbox PaymentCheck --> 
                                                                      </td>
                                                                      <td style="width:20%">
                                                                           <!--checkbox PettyCash -->
                                                                          <table cellpadding="0" cellspacing="0" border="0">
                                                                             <tr id="trPettyCash_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 200px">
                                                                                    <asp:Label runat="server" ID="lblPettyCash_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:30px; ">
                                                                                    <LCtrl:LAjitCheckBox ID="chkPettyCash_JournalDoc" runat="server" MapXML="PettyCash" MapBranchNode="JournalDoc" />                                                                                                                                                       
                                                                                </td>
                                                                             </tr>    
                                                                           </table>
                                                                          <!--checkbox PettyCash --> 
                                                                      </td>
                                                                       <td style="width:20%">
                                                                          <!--checkbox WillBill -->
                                                                          <table cellpadding="0" cellspacing="0" border="0">
                                                                               <tr id="trWillBill_JournalDoc" align="left" valign="middle" runat="server">
                                                                                    <td class="formtext" style="height: 24px; width: 200px">
                                                                                        <asp:Label runat="server" ID="lblWillBill_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                    </td>
                                                                                     <td valign="middle" style="width:30px; ">
                                                                                        <LCtrl:LAjitCheckBox ID="chkWillBill_JournalDoc" runat="server" MapXML="WillBill" MapBranchNode="JournalDoc" />                                                                                                                                                       
                                                                                    </td>
                                                                               </tr>  
                                                                          </table>
                                                                          <!--checkbox WillBill --> 
                                                                      </td>
                                                                       <td style="width:20%">
                                                                          <!--checkbox AddBill -->
                                                                          <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr id="trAddBill_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 200px">
                                                                                    <asp:Label runat="server" ID="lblAddBill_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:30px; ">
                                                                                    <LCtrl:LAjitCheckBox ID="chkAddBill_JournalDoc" runat="server" MapXML="AddBill" MapBranchNode="JournalDoc" />                                                                                                                                                       
                                                                                </td>
                                                                             </tr>    
                                                                           </table>
                                                                          <!--checkbox AddBill --> 
                                                                      </td>
                                                                    </tr>
                                                                 </table>
                                                              </td> 
                                                            </tr>
                                                            <!-- new check boxes end -->
                                                             <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                           
                                                           <!-- Branch AccountingItem Start-->
                                                              <tr align="center">
                                                                <td colspan="4">
                                                                    <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                                </td>
                                                            </tr>
                                                           <!-- Branch AccountingItem End -->
                                                           <!--space between page controls and submit button--> 
                                                            <tr>
                                                                <td style="height:15px"></td>
                                                            </tr>
                                                            <!--Submit and Cancel buttons-->                                                            
                                                            <tr style="height:24px">
                                                                <td align="center" colspan="2">
                                                                 <table>
                                                                    <tr>
                                                                        <td>
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click"  
                                                                            Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>                                                                
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click"
                                                                              Height="18" AlternateText="ContinueAdd"/>
                                                                        </td>
                                                                       <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click"
                                                                              Height="18" AlternateText="AddClone"/>
                                                                        </td> 
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel"/>
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
                                                            <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
                                                                     <table border="0">
                                                                        <tr>
                                                                         <td align="right"> 
                                                                            <asp:ImageButton runat="server" ID="imgbtnPrevious" Tooltip="Previous"/>
                                                                        </td>
                                                                         <td align="left"> 
                                                                            <asp:ImageButton runat="server" ID="imgbtnNext" Tooltip="Next"/>
                                                                        </td> 
                                                                       </tr> 
                                                                   </table>
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
                                            <table cellpadding="4" cellspacing="4" border="0"  width="800px" align="center">
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
</asp:content>
