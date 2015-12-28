<%@ Page Language="C#" AutoEventWireup="true" Codebehind="APMInvoice.aspx.cs" Inherits="LAjitDev.Payables.APMInvoice"
    MasterPageFile="~/MasterPages/TopLeft.Master" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
            <contenttemplate>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
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
                                                    <tr style="height: 10px;" id="trProcessLinks" align="right" valign="middle" runat="server">
                                                        <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                            <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <!--Page subject tr-->
                                                    <tr id="trSubject" runat="server" visible="false" style="height: 10px; background-color: Green">
                                                        <td class="bigtitle" valign="middle">
                                                            <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                                height="6px" />
                                                            <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" style="height: 467px">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                                <tr style="height: 1px;" align="right" valign="middle">
                                                                    <td align="right" valign="middle" colspan="3">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                    <td valign="middle" colspan="3" align="center">
                                                                        <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                            OnClick="imgbtnIsApproved_Click" 
                                                                            AlternateText="IsApproved" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td valign="top" style="width: 354px">
                                                                        <!-- Left Table Start -->
                                                                        <table cellpadding="0" border="0" cellspacing="0">
                                                                            <tr align="left" valign="middle" id="trAutoFillVendor_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                    <asp:Label runat="server" ID="lblAutoFillVendor_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label">
                                                                                    </asp:Label>
                                                                                    <%--<asp:LinkButton runat="server" ID="lblAutoFillVendor_JournalDoc1" MapBranchNode="JournalDoc" Text="Vendor" OnClientClick="javascript:return OnAutoFillClick('1AutoFillVendor','AutoFillVendor');"></asp:LinkButton>--%>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtAutoFillVendor_JournalDoc" runat="server" MapXML="AutoFillVendor"
                                                                                        MapBranchNode="JournalDoc" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAutoFillVendor_JournalDoc" runat="server"
                                                                                        ControlToValidate="txtAutoFillVendor_JournalDoc" MapBranchNode="JournalDoc" MapXML="AutoFillVendor"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="false" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="middle" id="trSelectPCVendor_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px;">
                                                                                    <asp:Label runat="server" ID="lblSelectPCVendor_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectPCVendor_JournalDoc" runat="server" MapXML="SelectPCVendor"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectPCVendor_JournalDoc" MapXML="SelectPCVendor"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectPCVendor_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblDescription" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                                        ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trInvNumber" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblInvNumber" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtInvNumber" runat="server" MapXML="InvNumber" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqInvNumber" runat="server" MapXML="InvNumber"
                                                                                        ControlToValidate="txtInvNumber" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trPOReference_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblPOReference_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtPOReference_JournalDoc" runat="server" MapXML="POReference"
                                                                                        MapBranchNode="JournalDoc" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPOReference_JournalDoc" runat="server"
                                                                                        MapXML="POReference" MapBranchNode="JournalDoc" ControlToValidate="txtPOReference_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="top" id="trSelectAPInvoiceType_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px;">
                                                                                    <asp:Label runat="server" ID="lblSelectAPInvoiceType_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectAPInvoiceType_JournalDoc" runat="server" MapXML="SelectAPInvoiceType"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectAPInvoiceType_JournalDoc" MapXML="SelectAPInvoiceType"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectAPInvoiceType_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trInvDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblInvDate" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <LCtrl:LAjitTextBox ID="txtInvDate" runat="server" MapXML="InvDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqInvDate" MapXML="InvDate" runat="server"
                                                                                                    ControlToValidate="txtInvDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <!-- Left Table End -->
                                                                    </td>
                                                                    <!--  middle to appear link button start width:20px padding-top:2px OnClick="lnkBtnAutoFillVendor_JournalDoc_OnClick"-->
                                                                    <td style="width: 10%; padding-top: 5px;" align="center" valign="top">
                                                                        <LCtrl:LAjitLinkButton ID="lnkBtnAutoFillVendor_JournalDoc" runat="server" Visible="false"
                                                                            MapXML="AutoFillVendor">
                                                                        </LCtrl:LAjitLinkButton>
                                                                    </td>
                                                                    <!-- middle to appear link button end-->
                                                                    <td valign="top">
                                                                        <!-- Right Table Start -->
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr align="left" valign="top" id="trSelectBatch_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;">
                                                                                    <asp:Label runat="server" ID="lblSelectBatch_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectBatch_JournalDoc" runat="server" MapXML="SelectBatch"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectBatch_JournalDoc" MapXML="SelectBatch"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectBatch_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                    <asp:Label runat="server" ID="lblControlTotal" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" MapBranchNode="JournalDoc"
                                                                                        runat="server" ControlToValidate="txtControlTotal" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                        Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal"
                                                                                        MapXML="ControlTotal" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                                        ErrorMessage="Value should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                                        Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trPostDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 130px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblPostDate" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" colspan="2">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <LCtrl:LAjitTextBox ID="txtPostDate" runat="server" MapXML="PostDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostDate" MapXML="PostDate" runat="server"
                                                                                                    ControlToValidate="txtPostDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>          
                                                                                    </table>
                                                                                 </td>       
                                                                             </tr>
                                                                                            
                                                                             <tr id="trPostToLedger" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; " valign="top">
                                                                                    <asp:Label runat="server" ID="lblPostToLedger" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:28px">
                                                                                    <LCtrl:LAjitCheckBox ID="chkPostToLedger" runat="server" MapXML="PostToLedger" />
                                                                                </td>
                                                                            </tr>                           
                                                                                                                
                                                                            
                                                                            <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                    <asp:Label runat="server" ID="lblCurrencyTypeCompany" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                        Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                        ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="top" id="trEntryBank_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;">
                                                                                    <asp:Label runat="server" ID="lblEntryBank_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryBank_JournalDoc" runat="server" MapXML="EntryBank"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank_JournalDoc" MapXML="EntryBank"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlEntryBank_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="top" id="trCheckNumber_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;">
                                                                                    <asp:Label runat="server" ID="lblCheckNumber_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtCheckNumber_JournalDoc" runat="server" MapXML="CheckNumber"
                                                                                        MapBranchNode="JournalDoc" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCheckNumber_JournalDoc" MapXML="CheckNumber"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtCheckNumber_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trJournalRef" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                    <asp:Label runat="server" ID="lblJournalRef" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server"
                                                                                        ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="top" id="trVendPayTerm_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;">
                                                                                    <asp:Label runat="server" ID="lblVendPayTerm_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlVendPayTerm_JournalDoc" runat="server" MapXML="VendPayTerm"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendPayTerm_JournalDoc" MapXML="VendPayTerm"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlVendPayTerm_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left" valign="top" id="trSelectCheckGroup_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px;">
                                                                                    <asp:Label runat="server" ID="lblSelectCheckGroup_JournalDoc" MapBranchNode="JournalDoc"
                                                                                        SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectCheckGroup_JournalDoc" runat="server" MapXML="SelectCheckGroup"
                                                                                        MapBranchNode="JournalDoc" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCheckGroup_JournalDoc" MapXML="SelectCheckGroup"
                                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectCheckGroup_JournalDoc"
                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trPaymentDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblPaymentDate_JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <LCtrl:LAjitTextBox ID="txtPaymentDate_JournalDoc" runat="server" MapXML="PaymentDate"
                                                                                                    Width="68px"></LCtrl:LAjitTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentDate_JournalDoc" MapXML="PaymentDate"
                                                                                                    runat="server" ControlToValidate="txtPaymentDate_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <!-- Right Table End -->
                                                                    </td>
                                                                </tr>
                                                                <%-- <tr  align="right" valign="middle" >                                                                                                                             
                                                       <td align="right" valign="middle" colspan="3">
                                                           <img src="../App_Themes/<%=Session["MyTheme"]%>/Images/spacer.gif" alt="spacer" width="1px" height="1px" />
                                                       </td>
                                                    </tr>--%>
                                                                <!-- Branch AccountingItem Start -->
                                                                <tr align="center">
                                                                    <td colspan="3">
                                                                        <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                                    </td>
                                                                </tr>
                                                                <!--Submit and Cancel buttons-->
                                                                <tr style="height: 24px">
                                                                    <td align="center" colspan="3">
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click"
                                                                                         Height="18" AlternateText="Submit">
                                                                                    </LCtrl:LAjitImageButton>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click"
                                                                                         Height="18" AlternateText="ContinueAdd" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click"
                                                                                         Height="18" AlternateText="AddClone" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 24px">
                                                                    <td colspan="3" align="center">
                                                                        <table border="0" cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="imgbtnPrevious" ToolTip="Previous" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton runat="server" ID="imgbtnNext" ToolTip="Next" />
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
</asp:Content>
