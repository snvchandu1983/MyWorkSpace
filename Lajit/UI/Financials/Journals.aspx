<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Journals.aspx.cs" Inherits="LAjitDev.Financials.Journals" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
        <asp:Panel ID="pnlContent" runat="server">
            <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
                <ContentTemplate>
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
                                            <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                                <!-- entry form start-->
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
                                                                <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                    <td valign="middle" colspan="2" align="center">
                                                                        <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                             AlternateText="IsApproved" OnClick="imgbtnIsApproved_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" style="width: 50%;">
                                                                        <!-- Left Table Start -->
                                                                        <table cellpadding="0" border="0" cellspacing="0">
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
                                                                            <tr id="trTrxDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblTrxDate" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <LCtrl:LAjitTextBox ID="txtTrxDate" runat="server" MapXML="TrxDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTrxDate" MapXML="TrxDate" runat="server"
                                                                                                    ControlToValidate="txtTrxDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
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
                                                                            <tr id="trJournalRef" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 160px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblJournalRef" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtJournalRef" runat="server" MapXML="JournalRef" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJournalRef" MapXML="JournalRef" runat="server"
                                                                                        ControlToValidate="txtJournalRef" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblControlTotal" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" runat="server"
                                                                                        ControlToValidate="txtControlTotal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal"
                                                                                        MapXML="ControlTotal" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                                        ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                                        Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <!-- Left Table End -->
                                                                    </td>
                                                                    <td valign="top" style="width: 50%;">
                                                                        <!-- Right Table Start -->
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <!-- Branch JournalDoc Start -->
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
                                                                            <tr id="trRecurEntry_JournalDoc" align="left" valign="top" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px">
                                                                                    <asp:Label runat="server" ID="lblRecurEntry_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitCheckBox ID="chkRecurEntry_JournalDoc" runat="server" MapXML="RecurEntry"
                                                                                        MapBranchNode="JournalDoc" />
                                                                                </td>
                                                                            </tr>
                                                                            
                                                                            <tr id="trStartDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                                    <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblStartDate_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle" style="width: 55px;" colspan="2">
                                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <LCtrl:LAjitTextBox ID="txtStartDate_JournalDoc" runat="server" MapXML="StartDate"
                                                                                                        MapBranchNode="JournalDoc" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqStartDate_JournalDoc" MapXML="StartDate"
                                                                                                        runat="server" ControlToValidate="txtStartDate_JournalDoc" MapBranchNode="JournalDoc"
                                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            
                                                                               <tr id="trFinalPost_JournalDoc" align="left" valign="middle" runat="server">
                                                                                    <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblFinalPost_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle" style="width: 55px;" colspan="2">
                                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <LCtrl:LAjitTextBox ID="txtFinalPost_JournalDoc" runat="server" MapXML="FinalPost"
                                                                                                        MapBranchNode="JournalDoc" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqFinalPost_JournalDoc" MapXML="FinalPost"
                                                                                                        runat="server" ControlToValidate="txtFinalPost_JournalDoc" MapBranchNode="JournalDoc"
                                                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            <tr id="trRevEntry_JournalDoc" align="left" valign="top" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px">
                                                                                    <asp:Label runat="server" ID="lblRevEntry_JournalDoc" SkinID="Label" MapBranchNode="JournalDoc"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <LCtrl:LAjitCheckBox ID="chkRevEntry_JournalDoc" runat="server" MapXML="RevEntry"
                                                                                        MapBranchNode="JournalDoc" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trRevDate_JournalDoc" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblRevDate_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width: 196px;">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <LCtrl:LAjitTextBox ID="txtRevDate_JournalDoc" runat="server" MapXML="RevDate" MapBranchNode="JournalDoc"
                                                                                                    Width="68px"></LCtrl:LAjitTextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqRevDate_JournalDoc" MapXML="RevDate" runat="server"
                                                                                                    MapBranchNode="JournalDoc" ControlToValidate="txtRevDate_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <!-- Branch JournalDoc End -->
                                                                        </table>
                                                                        <!-- Right Table End -->
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 1px;" align="right" valign="middle">
                                                                    <td align="right" valign="middle" colspan="2">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <!-- Branch AccountingItem Start -->
                                                                <tr align="center">
                                                                    <td colspan="2" valign="top">
                                                                        <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                                    </td>
                                                                </tr>
                                                                <!-- Branch AccountingItem End -->
                                                                <!--space between page controls and submit button-->
                                                                <tr>
                                                                    <td style="height: 15px">
                                                                    </td>
                                                                </tr>
                                                                <!--Submit and Cancel buttons-->
                                                                <tr style="height: 24px">
                                                                    <td style="height: 24px;" colspan="2" align="center">
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click"
                                                                                        Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
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
                                                                <tr style="height: 10px">
                                                                    <td>
                                                                        <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 24px">
                                                                    <td style="height: 24px;" colspan="2" align="center">
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:ImageButton runat="server" ID="imgbtnPrevious" ToolTip="Previous" />
                                                                                </td>
                                                                                <td align="left">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
</asp:content>
