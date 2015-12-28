<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Pettycashjournal.aspx.cs"
    Inherits="LAjitDev.Financials.Pettycashjournal" EnableEventValidation="false" %>

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
                                    <asp:Panel ID="pnlEntryForm" runat="server">
                                        <!-- entry form start -->
                                        <table  width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
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
                                                <td align="left" valign="top" style="height:500px">
                                                    <table border="0" cellspacing="0" cellpadding="0" align="left" style="height:14px" width="100%">
                                                        <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                           <td align="right" valign="middle" colspan="2">
                                                               &nbsp;
                                                           </td>
                                                        </tr>
                                                         
                                                        <!-- SOX approval tr --> 
                                                         <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                            <td valign="middle" colspan="2" align="center">
                                                               <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" 
                                                                AlternateText="IsApproved"/>
                                                            </td>
                                                         </tr>
                                                         <tr>
                                                            <td valign="top"  style="width: 50%;"> 
                                                               <!-- Left Table Start -->
                                                                <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                                     <tr align="left" valign="top" id="trSelectPCVendor_JournalDoc" runat="server">
                                                                            <td class="formtext" style="height:24px; width: 141px;">
                                                                                    <asp:Label  runat="server" ID="lblSelectPCVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                             <td valign="middle" style="width:205px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectPCVendor_JournalDoc" runat="server" MapXML="SelectPCVendor" MapBranchNode="JournalDoc" Width="204px"  ></LCtrl:LAjitDropDownList>
                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqSelectPCVendor_JournalDoc" MapXML="SelectPCVendor" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectPCVendor_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>                                                                                     
                                                                        </tr>
                                                                     <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 141px" valign="Top">
                                                                            <asp:Label   runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width:205px;">
                                                                            <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="200px" ></LCtrl:LajitTextBox>                                                                                     
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                        </td>
                                                                     </tr>
                                                                     <tr id="trEnvelopeReference" aalign="left" valign="middle" runat="server">
                                                                        <td  class="formtext" style="height: 24px; width:141px" valign="top">
                                                                            <asp:Label   runat="server" ID="lblEnvelopeReference" SkinID="Label" ></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width:205px; ">
                                                                            <LCtrl:LAjitTextBox ID="txtEnvelopeReference" runat="server" MapXML="EnvelopeReference" Width="200px"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEnvelopeReference" runat="server" MapXML="EnvelopeReference" ControlToValidate="txtEnvelopeReference" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                        </td>
                                                                    </tr> 
                                                                     <tr id="trEnvelopeDate" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                            <asp:Label runat="server" ID="lblEnvelopeDate"  SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width:205px; ">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <LCtrl:LajitTextBox ID="txtEnvelopeDate" runat="server" MapXML="EnvelopeDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    </td>
                                                                                     <td>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEnvelopeDate" MapXML="EnvelopeDate" runat="server" ControlToValidate="txtEnvelopeDate" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                    </td>
                                                                                   
                                                                                </tr>
                                                                            </table>  
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                               <!-- Left Table End -->      
                                                            </td>
                                                           <td valign="top" style="width: 50%;">
                                                               <!-- Right Table Start -->
                                                                  <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                  <!-- Branch JournalDoc Start OnSelectedIndexChanged="ddlSelectBatch_JournalDoc_SelectedIndexChanged"  AutoPostBack="true"-->
                                                                        <tr align="left" valign="top" id="trSelectBatch_JournalDoc" runat="server">
                                                                            <td class="formtext" style="height:24px; width:125px;" valign="top">
                                                                                 <asp:Label runat="server" ID="lblSelectBatch_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectBatch_JournalDoc" runat="server" MapXML="SelectBatch" MapBranchNode="JournalDoc" Width="184px"  ></LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectBatch_JournalDoc" MapXML="SelectBatch" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectBatch_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>                                                                                     
                                                                        </tr>
                                                                       
                                                                       <!-- Branch JournalDoc End --> 
                                                                     <tr id="trJournalRef" align="left" valign="top" runat="server">
                                                                            <td class="formtext" style="height:24px; width:125px;" valign="top">
                                                                                <asp:Label runat="server" ID="lblJournalRef"  SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px" ></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server" ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>    
                                                                         <tr id="trEnvelopeTotal" align="left" valign="top" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                <asp:Label runat="server" ID="lblEnvelopeTotal"  SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtEnvelopeTotal" runat="server" MapXML="EnvelopeTotal" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqEnvelopeTotal" MapXML="EnvelopeTotal" runat="server" ControlToValidate="txtEnvelopeTotal" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>      
                                                                                 <LCtrl:LAjitRegularExpressionValidator ID="regEnvelopeTotal" runat="server" ControlToValidate="txtEnvelopeTotal" MapXML="EnvelopeTotal"
                                                                                    ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                     Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>     
                                                                            </td>
                                                                        </tr> 
                                                                        <tr id="trCurrencyTypeCompany" align="left" valign="top" runat="server">
                                                                            <td  class="formtext" style="height: 24px; width:125px" valign="top">
                                                                                <asp:Label   runat="server" ID="lblCurrencyTypeCompany" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
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
                                                       <!-- Branch AccountingItem Start style="padding-top: 25px;" -->
                                                          <tr align="center">
                                                            <td colspan="2">
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
                                                                <td style="height: 24px;" colspan="2" align="center">
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
                                                            <tr style="height:10px">
                                                                <td>
                                                                    <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                             <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="2" align="center">
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
                                        <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
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
