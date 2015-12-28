<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PaymentRequest.aspx.cs"
    Inherits="LAjitDev.Payables.PaymentRequest" %>
<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
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
                                            <!-- entry form start -->
                                            <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0"
                                                class="formmiddle">
                                                <!--Pop up header tr-->
                                                <tr style="height: 24px;" runat="server" id="trPopupHeader" visible="false" valign="top">
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                            cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                                                            <tr style="height: 24px;">
                                                                <td id="htcPopEntryForm" runat="server" align="center" class="grdVwRPTitle" style="height: 24px;
                                                                    border-width: 0px">
                                                                    <asp:Label ID="lblPopupEntry" runat="server" Height="24px"></asp:Label>
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
                                                <!--Page subject tr style="height: 439px"-->
                                                <tr id="trSubject" runat="server" visible="false" style="height: 24px; background-color: Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                           <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <!-- SOX Approval tr -->
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td valign="middle" colspan="2" align="center">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" 
                                                                    AlternateText="IsApproved"/>
                                                                </td>
                                                             </tr>
                                                            <tr>
                                                                <td valign="top" style="width: 75%;">
                                                                   <!-- Left Table Start -->
                                                                    <table cellpadding="0" border="0" cellspacing="0" >
                                                                            <tr id="trEntryBank" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width:141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblEntryBank" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryBank" runat="server" MapXML="EntryBank" Width="388px"></LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank" runat="server" MapXML="EntryBank" ControlToValidate="ddlEntryBank" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trPaymentDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblPaymentDate"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:196px; ">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <LCtrl:LajitTextBox ID="txtPaymentDate" runat="server" MapXML="PaymentDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentDate" MapXML="PaymentDate" runat="server" ControlToValidate="txtPaymentDate" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>                                                                                        
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trStartCheck" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblStartCheck" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtStartCheck" runat="server" MapXML="StartCheck" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqStartCheck" MapXML="StartCheck" runat="server" ControlToValidate="txtStartCheck"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regStartCheck" runat="server" ControlToValidate="txtStartCheck" MapXML="StartCheck"
                                                                                        ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>                                                                                        
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trEntryVendor" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width:141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblEntryVendor" SkinID="Label" ></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryVendor" runat="server" MapXML="EntryVendor" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqEntryVendor" runat="server" MapXML="EntryVendor" ControlToValidate="ddlEntryVendor" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="StartCheck" runat="server" ControlToValidate="txtDescription"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>                                                                           
                                                                    </table>
                                                                   <!-- Left Table End -->      
                                                                </td>
                                                               <td valign="top" style="width: 25%;">
                                                                   <!-- Right Table Start -->
                                                                      <table cellpadding="0" cellspacing="0" border="0" >
                                                                            <tr id="trAccountName" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblAccountName"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtAccountName" runat="server" MapXML="AccountName" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAccountName" MapXML="AccountName" runat="server" ControlToValidate="txtAccountName"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trAmountEntered" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblAmountEntered"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtAmountEntered" runat="server" MapXML="AmountEntered" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAmountEntered" MapXML="AmountEntered" runat="server" ControlToValidate="txtAmountEntered"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regAmountEntered" runat="server" ControlToValidate="txtAmountEntered" MapXML="AmountEntered"
                                                                                        ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                                </td>
                                                                            </tr>    
                                                                            <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblControlTotal"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" runat="server" ControlToValidate="txtControlTotal"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                     <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal" MapXML="ControlTotal"
                                                                                        ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>  
                                                                                </td>
                                                                            </tr> 
                                                                            <tr id="trCGITotal" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblCGITotal"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtCGITotal" runat="server" MapXML="CGITotal" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCGITotal" MapXML="CGITotal" runat="server" ControlToValidate="txtCGITotal"  ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regCGITotal" runat="server" ControlToValidate="txtCGITotal" MapXML="CGITotal"
                                                                                        ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
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
                                                            <tr>
                                                                <td colspan="2">
                                                                    <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="InvoiceSelect" />
                                                                </td>
                                                            </tr>
                                                            <!-- FiscalPeriod End -->
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
                                            <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td>
                                                        <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label></td>
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
