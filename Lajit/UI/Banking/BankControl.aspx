<%@ Page Language="C#" AutoEventWireup="true" Codebehind="BankControl.aspx.cs" Inherits="LAjitDev.Banking.BankControl"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
  <asp:Panel ID="pnlContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updtPnlContent">
    <ContentTemplate>
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
                                            <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0"
                                                cellspacing="0" cellpadding="0" class="formmiddle">
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
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" visible="false" style="height: 24px; background-color: Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height: 439px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height: 1px;" align="right" valign="middle">
                                                                <td align="right" valign="middle" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" style="width: 50%;">
                                                                    <!-- Left Table Start -->
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left" valign="top" id="trEntryBank" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblEntryBank" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitDropDownList ID="ddlEntryBank" runat="server" MapXML="EntryBank" Width="206px">
                                                                                </LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank" runat="server" MapXML="EntryBank"
                                                                                    ControlToValidate="ddlEntryBank" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left" valign="top" id="trEndDate" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblEndDate" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <LCtrl:LAjitTextBox ID="txtEndDate" runat="server" MapXML="EndDate" Width="70px"></LCtrl:LAjitTextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEndDate" MapXML="EndDate" runat="server"
                                                                                                ControlToValidate="txtEndDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trOpenBalance" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblOpenBalance" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtOpenBalance" runat="server" MapXML="OpenBalance" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqOpenBalance" MapXML="OpenBalance" runat="server"
                                                                                    ControlToValidate="txtOpenBalance" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trEndBal" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblEndBal" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtEndBal" runat="server" MapXML="EndBal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqEndBal" MapXML="EndBal" runat="server"
                                                                                    ControlToValidate="txtEndBal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trGLEndBal" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblGLEndBal" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtGLEndBal" runat="server" MapXML="GLEndBal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqGLEndBal" MapXML="GLEndBal" runat="server"
                                                                                    ControlToValidate="txtGLEndBal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- Left Table End -->
                                                                </td>
                                                                <td valign="top" style="width: 50%;">
                                                                    <!-- Right Table Start -->
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr id="trClearedChecks" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblClearedChecks" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtClearedChecks" runat="server" MapXML="ClearedChecks" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqClearedChecks" runat="server" ControlToValidate="txtClearedChecks"
                                                                                    ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" MapXML="ClearedChecks"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trClearedDeposits" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblClearedDeposits" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtClearedDeposits" runat="server" MapXML="ClearedDeposits"
                                                                                    Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqClearedDeposits" runat="server" ControlToValidate="txtClearedDeposits"
                                                                                    ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" MapXML="ClearedDeposits"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trClearedAdjs" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblClearedAdjs" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtClearedAdjs" runat="server" MapXML="ClearedAdjs" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqClearedAdjs" runat="server" ControlToValidate="txtClearedAdjs"
                                                                                    ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" MapXML="ClearedAdjs"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trBankCharges" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblBankCharges" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtBankCharges" runat="server" MapXML="BankCharges" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqBankCharges" runat="server" ControlToValidate="txtBankCharges"
                                                                                    ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" MapXML="BankCharges"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trLedgerBalance" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblLedgerBalance" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtLedgerBalance" runat="server" MapXML="LedgerBalance" Width="180px"></LCtrl:LAjitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqLedgerBalance" runat="server" ControlToValidate="txtLedgerBalance"
                                                                                    ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                                    ToolTip="Value is required." SetFocusOnError="True" MapXML="LedgerBalance"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- Right Table End -->
                                                                </td>
                                                            </tr>
                                                            <!--space between page controls and submit button-->
                                                            <tr>
                                                                <td style="height: 15px">
                                                                </td>
                                                            </tr>
                                                            <!--Submit and Cancel buttons-->
                                                            <tr style="height: 24px">
                                                                <td align="center" colspan="2" style="height: 24px;">
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" AlternateText="Submit" Height="18"
                                                                                    OnClick="imgbtnSubmit_Click" OnClientClick="javascript:return ValidateControls()" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnContinueAdd" runat="server" AlternateText="ContinueAdd"
                                                                                    Height="18" OnClick="imgbtnContinueAdd_Click" OnClientClick="javascript:ValidateControls()" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnAddClone" runat="server" AlternateText="AddClone" Height="18"
                                                                                    OnClick="imgbtnAddClone_Click" OnClientClick="javascript:ValidateControls()" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnCancel" runat="server" AlternateText="Cancel" Height="18" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 24px">
                                                                <td align="center" colspan="2" style="height: 24px;">
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:ImageButton ID="imgbtnPrevious" runat="server" ToolTip="Previous" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton ID="imgbtnNext" runat="server" ToolTip="Next" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 10px">
                                                                <td>
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
                                            <table cellpadding="4" cellspacing="4" border="1" width="800px" align="center">
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
     </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Panel>   
</asp:Content>
