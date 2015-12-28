<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CheckHistory.aspx.cs" Inherits="LAjitDev.Payables.CheckHistory" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
            <contenttemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                            <!-- entry form start -->
                                            <table id="tblEntryForm" runat="server" style="height: 511px; width: 100%" border="0"
                                                cellspacing="0" cellpadding="0">
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
                                                    <td>
                                                        <!-- Quick Check Start -->
                                                        <table cellpadding="0" width="100%" border="0" cellspacing="0" class="formcheckborder">
                                                            <tr>
                                                                <td style="height: 1px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <!-- Table Autofill and  check no start SkinID="Label" LabelBig -->
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td align="left" style="width: 70%">
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr align="left" valign="middle" id="trEntryBank_JournalDoc" runat="server">
                                                                                        <td align="left" class="formtext" style="height: 24px; width: 100px;">
                                                                                            <asp:Label runat="server" ID="lblEntryBank_JournalDoc" MapBranchNode="JournalDoc"
                                                                                                SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="left" style="width: 216px;">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlEntryBank_JournalDoc" runat="server" MapXML="EntryBank"
                                                                                                MapBranchNode="JournalDoc" Width="204px">
                                                                                            </LCtrl:LAjitDropDownList>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank_JournalDoc" MapXML="EntryBank"
                                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlEntryBank_JournalDoc"
                                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right" style="width: 30%">
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr align="left" id="trCheckNumber_JournalDoc" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width:50%;">
                                                                                            <asp:Label runat="server" ID="lblCheckNumber_JournalDoc" MapBranchNode="JournalDoc"
                                                                                                SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 50%;">
                                                                                            <LCtrl:LAjitTextBox ID="txtCheckNumber_JournalDoc" runat="server" MapXML="CheckNumber"
                                                                                                MapBranchNode="JournalDoc" Width="97px"></LCtrl:LAjitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCheckNumber_JournalDoc" MapXML="CheckNumber"
                                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtCheckNumber_JournalDoc"
                                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
                                                                <td style="height: 1px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <!-- pyament date -->
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td style="width: 70%">
                                                                                &nbsp;</td>
                                                                            <td valign="top" align="right" style="width: 30%">
                                                                                <!-- payment date start-->
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr id="trPaymentDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width: 50%" >
                                                                                            <asp:Label runat="server" ID="lblPaymentDate_JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 50%;">
                                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                               <tr>
                                                                                                    <td>
                                                                                                        <LCtrl:LajitTextBox ID="txtPaymentDate_JournalDoc" runat="server" MapXML="PaymentDate" Width="78px" ></LCtrl:LajitTextBox>                                                                                     
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
                                                                <td style="height: 1px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <!-- AutoFill vendor amount start -->
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" bordercolor="red">
                                                                        <tr>
                                                                            <td style="width: 70%">
                                                                                <!-- auto fill start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr id="trVendor_JournalDoc" align="left" valign="top" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width: 100px;" valign="top">
                                                                                            <asp:Label runat="server" ID="lblVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width: 386px;">
                                                                                            <LCtrl:LAjitTextBox ID="txtVendor_JournalDoc" runat="server" MapXML="Vendor" MapBranchNode="JournalDoc"
                                                                                                Width="360px"></LCtrl:LAjitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqVendor_JournalDoc" MapXML="Vendor" runat="server"
                                                                                                MapBranchNode="JournalDoc" ControlToValidate="txtVendor_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <!-- auto fill end -->
                                                                            </td>
                                                                            <td style="width: 30%">
                                                                                <!-- control total start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width: 50%;">
                                                                                            <asp:Label runat="server" ID="lblControlTotal" Text="$" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 50%;">
                                                                                            <LCtrl:LAjitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="98px"></LCtrl:LAjitTextBox>
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
                                                                                </table>
                                                                                <!-- control total end -->
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- AutoFill vendor amount end -->
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 2px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <!-- Check Messsage and Currency Start -->
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td align="center" width="70%">
                                                                                <!-- Check Message Start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0" >
                                                                                    <tr align="center" valign="middle" id="trCheckMessage_JournalDoc" runat="server">
                                                                                        <td style="width: 236px" align="center">
                                                                                            <LCtrl:LAjitLabel ID="lblCheckMessage_JournalDoc_Value" MapBranchNode="JournalDoc"
                                                                                                runat="server" MapXML="CheckMessage"></LCtrl:LAjitLabel>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <!-- Check Message Start -->
                                                                            </td>
                                                                            <td align="right" style="width: 30%">
                                                                                <!-- Currency Start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width: 50%;">
                                                                                            <asp:Label runat="server" ID="lblCurrencyTypeCompany" SkinID="Label"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width:50%;">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                                Width="100px">
                                                                                            </LCtrl:LAjitDropDownList>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                                ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
                                                                <td style="height: 2px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <!-- left table 2 start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                        <td class="formtext" style="height: 24px; width: 60px" valign="top">
                                                                                            <asp:Label runat="server" ID="lblMemo" SkinID="LabelBig" Text="MEMO"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="top">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <!--  Description start -->
                                                                                    <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                        <td>
                                                                                            &nbsp;</td>
                                                                                        <td valign="middle" style="width: 476px;">
                                                                                            <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="450px"></LCtrl:LAjitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                                                ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <!--  Description end -->
                                                                                </table>
                                                                                <!-- left table 2 end -->
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                                                                            <asp:Label runat="server" ID="lblInvDate" SkinID="LabelBig"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width: 216px;">
                                                                                           <table cellpadding="0" cellspacing="0">
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
                                                                        <tr>
                                                                            <td>
                                                                                <!-- SelectApInvoiceType start -->
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr align="left" valign="top" id="trSelectAPInvoiceType_JournalDoc" runat="server">
                                                                                        <td class="formtext" style="height: 24px; width: 136px;">
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
                                                                                </table>
                                                                                <!-- Select ApInvoice end -->
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <!-- Invoice Date -->
                                                            <!-- hidden fields start-->
                                                            <tr id="trInvNumber" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 100px" valign="top">
                                                                    <asp:Label runat="server" ID="lblInvNumber" SkinID="LabelBig"></asp:Label>
                                                                </td>
                                                                <td valign="middle" style="width: 116px;">
                                                                    <LCtrl:LAjitTextBox ID="txtInvNumber" runat="server" MapXML="InvNumber" Width="100px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqInvNumber" runat="server" MapXML="InvNumber"
                                                                        ControlToValidate="txtInvNumber" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPOReference_JournalDoc" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 136px">
                                                                    <asp:Label runat="server" ID="lblPOReference_JournalDoc" MapBranchNode="JournalDoc"
                                                                        SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle" style="width: 196px;">
                                                                    <LCtrl:LAjitTextBox ID="txtPOReference_JournalDoc" runat="server" MapXML="POReference"
                                                                        MapBranchNode="JournalDoc" Width="180px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPOReference_JournalDoc" MapXML="POReference"
                                                                        MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtPOReference_JournalDoc"
                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trVendor" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 136px" valign="top">
                                                                    <asp:Label runat="server" ID="lblVendor" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle" style="width: 196px;">
                                                                    <LCtrl:LAjitTextBox ID="txtVendor" runat="server" MapXML="Vendor" Width="180px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendor" MapXML="Vendor" runat="server"
                                                                        ControlToValidate="txtVendor" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trJournalRef" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 136px" valign="top">
                                                                    <asp:Label runat="server" ID="lblJournalRef" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle" style="width: 196px;">
                                                                    <LCtrl:LAjitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server"
                                                                        ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <!-- hidden fields end-->
                                                            <tr>
                                                                <td style="height: 40px">
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <!-- Quick Check End -->
                                                    </td>
                                                </tr>
                                                <!-- Branch AccountingItem Start -->
                                                <!-- Branch AccountingItem Collapse Start -->
                                                <tr align="center" valign="top" style="height: 255px">
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <!-- branch accounting item title and grid start Height="100%" Height="100%" -->
                                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" class="formmiddle">
                                                                        <tr>
                                                                            <td>
                                                                                <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- branch accounting item title and grid end-->
                                                                </td>
                                                            </tr>
                                                         
                                                            <!--Submit and Cancel buttons-->                                                            
                                                        <tr style="height:24px">
                                                                <td align="left" colspan="3">
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
                                                                <td colspan="3" align="center">
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
                                                <!-- Branch AccountingItem End -->
                                            </table>
                                            <!-- Branch AccountingItem End -->
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
    </td> </tr> </table> </td> </tr>
    </table>
   </ContentTemplate> 
   </asp:UpdatePanel> 
  </asp:Panel>
</asp:content>
