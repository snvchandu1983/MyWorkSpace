<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ARinvoice.aspx.cs" Inherits="LAjitDev.Receivables.ARinvoice" %>
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
                                            <!--Page subject tr-->
                                            <tr id="trSubject" runat="server" visible="false" style="height: 24px; background-color: Green">
                                                <td class="bigtitle" valign="middle">
                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                        height="6px" />
                                                    <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" style="height: auto">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                        <tr style="height: 1px;" align="right" valign="middle">
                                                            <td align="right" valign="middle" colspan="3">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                            <td valign="middle" colspan="2" align="center">
                                                                <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                    OnClick="imgbtnIsApproved_Click"  AlternateText="IsApproved"/>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td valign="top" style="width:48%" align="center">
                                                                <!-- Left Table Start -->
                                                                <table cellpadding="0" border="0" cellspacing="0">
                                                                    <!-- Branch JournalDoc Start-->
                                                                     <tr align="left" valign="middle" id="trCheckMessage_JournalDoc" runat="server">
                                                                          <td style="width:236px" align="center" valign="top">
                                                                            <LCtrl:LAjitLabel ID="lblCheckMessage_JournalDoc_Value" MapBranchNode="JournalDoc" runat="server" MapXML="CheckMessage" CssClass="mlabelbig" ></LCtrl:LAjitLabel>
                                                                          </td>
                                                                    </tr>
                                                                    <tr align="left" valign="middle" id="trCustomer_JournalDoc" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblCustomer_JournalDoc" MapBranchNode="JournalDoc"
                                                                                SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitTextBox ID="txtCustomer_JournalDoc" runat="server" MapXML="Customer"
                                                                                MapBranchNode="JournalDoc" Width="225px" autocomplete="false"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCustomer_JournalDoc" MapXML="Customer"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtCustomer_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="false" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>                                                                   
                                                                    <tr align="left" valign="middle" id="trSelectCustContact_JournalDoc" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblSelectCustContact_JournalDoc" MapBranchNode="JournalDoc"
                                                                                SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitDropDownList ID="ddlSelectCustContact_JournalDoc" runat="server" MapXML="SelectCustContact"
                                                                                MapBranchNode="JournalDoc" Width="229px">
                                                                            </LCtrl:LAjitDropDownList>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCustContact_JournalDoc" MapXML="SelectCustContact"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSelectCustContact_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblDescription" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="225px"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                                ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInvoiceNumber" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblInvoiceNumber" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitTextBox ID="txtInvoiceNumber" runat="server" MapXML="InvoiceNumber" Width="225px"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqInvoiceNumber" MapXML="InvoiceNumber" runat="server"
                                                                                ControlToValidate="txtInvoiceNumber" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trTrxDate" align="left" valign="top" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px;">
                                                                            <asp:Label runat="server" ID="lblTrxDate" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                               <tr>
                                                                                    <td>
                                                                                        <LCtrl:LajitTextBox ID="txtTrxDate" runat="server" MapXML="TrxDate" Width="68px"></LCtrl:LajitTextBox>                                                                                     
                                                                                    </td>
                                                                                    <td>
                                                                                         <LCtrl:LAjitRequiredFieldValidator ID="reqTrxDate" MapXML="TrxDate" runat="server" ControlToValidate="txtTrxDate" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                    </td>    
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" valign="middle" id="trSelectCustInvoice_JournalDoc" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblSelectCustInvoice_JournalDoc" MapBranchNode="JournalDoc"
                                                                                SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitTextBox ID="txtSelectCustInvoice_JournalDoc" runat="server" MapXML="SelectCustInvoice"
                                                                                MapBranchNode="JournalDoc" Width="225px" autocomplete="false"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCustInvoice_JournalDoc" MapXML="SelectCustInvoice"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtSelectCustInvoice_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" valign="middle" id="trCustPayTerm_JournalDoc" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px" valign="top">
                                                                            <asp:Label runat="server" ID="lblCustPayTerm_JournalDoc" MapBranchNode="JournalDoc"
                                                                                SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <LCtrl:LAjitDropDownList ID="ddlCustPayTerm_JournalDoc" runat="server" MapXML="CustPayTerm"
                                                                                MapBranchNode="JournalDoc" Width="229px">
                                                                            </LCtrl:LAjitDropDownList>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCustPayTerm_JournalDoc" MapXML="CustPayTerm"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlCustPayTerm_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <!-- Left Table End -->
                                                            </td>
                                                            <!-- Middle Link button start -->
                                                            <td style="width: 4%;" align="center" valign="top">
                                                                <LCtrl:LAjitLinkButton ID="lnkBtnCustomer_JournalDoc" runat="server" Visible="false"
                                                                    MapXML="Customer" OnClientClick="return ValidateBPCText('ctl00_cphPageContents_txtCustomer_JournalDoc')">
                                                                </LCtrl:LAjitLinkButton>
                                                            </td>
                                                            <!-- Middle Link button end -->
                                                             <td valign="top" style="width:48%" align="center">
                                                                <!-- Right Table Start -->
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
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
                                                                    <tr id="trInvoiceAmount" align="left" valign="top" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 125px;">
                                                                            <asp:Label runat="server" ID="lblInvoiceAmount" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 196px;">
                                                                            <LCtrl:LAjitTextBox ID="txtInvoiceAmount" runat="server" MapXML="InvoiceAmount" Width="180px"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqInvoiceAmount" MapXML="InvoiceAmount" runat="server"
                                                                                ControlToValidate="txtInvoiceAmount" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            <LCtrl:LAjitRegularExpressionValidator ID="regInvoiceAmount" runat="server" ControlToValidate="txtInvoiceAmount"
                                                                                MapXML="InvoiceAmount" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                                ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                                Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                     <tr id="trPostDate" align="left" valign="top" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 161px;">
                                                                            <asp:Label runat="server" ID="lblPostDate" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 225px;">
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                               <tr>
                                                                                    <td>
                                                                                        <LCtrl:LajitTextBox ID="txtPostDate" runat="server" MapXML="PostDate" Width="68px"></LCtrl:LajitTextBox>                                                                                     
                                                                                    </td>
                                                                                    <td>
                                                                                         <LCtrl:LAjitRequiredFieldValidator ID="reqPostDate" MapXML="PostDate" runat="server" ControlToValidate="txtPostDate" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                    </td>    
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCurrencyTypeCompany" align="left" valign="top" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 125px;">
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
                                                                    <tr align="left" valign="top" id="trSalesRep_JournalDoc" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 125px;">
                                                                            <asp:Label runat="server" ID="lblSalesRep_JournalDoc" MapBranchNode="JournalDoc"
                                                                                SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 196px;">
                                                                            <LCtrl:LAjitDropDownList ID="ddlSalesRep_JournalDoc" runat="server" MapXML="SalesRep"
                                                                                MapBranchNode="JournalDoc" Width="184px">
                                                                            </LCtrl:LAjitDropDownList>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSalesRep_JournalDoc" MapXML="SalesRep"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlSalesRep_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInvoiceComment_JournalDoc" align="left" valign="top" runat="server">
                                                                        <td class="formtext" style="height: 24px; width: 125px;">
                                                                            <asp:Label runat="server" ID="lblInvoiceComment_JournalDoc" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                        <td valign="middle" style="width: 196px;">
                                                                            <LCtrl:LAjitTextBox ID="txtInvoiceComment_JournalDoc" TextMode="MultiLine"  Rows="1" Columns="5" runat="server" MapXML="InvoiceComment"
                                                                                MapBranchNode="JournalDoc" Width="180px"></LCtrl:LAjitTextBox>
                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqInvoiceComment_JournalDoc" MapXML="InvoiceComment"
                                                                                MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtInvoiceComment_JournalDoc"
                                                                                ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                                SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        </td>
                                                                    </tr>                                                                            
                                                                </table>
                                                                <!-- Branch JournalDoc End-->
                                                                <!-- Right Table End -->
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 1px;" align="right" valign="middle">
                                                            <td align="right" valign="middle" colspan="3">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <!--Branch AccountingItem Start style="padding-top: 25px;" -->
                                                        <tr align="center">
                                                            <td colspan="3">
                                                                <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                            </td>
                                                        </tr>
                                                        <!-- Branch AccountingItem End -->
                                                        <!--space between page controls and submit button--> 
                                                        <tr>
                                                            <td style="height:15px" colspan="3"></td>
                                                        </tr>
                                                        <!--Submit and Cancel buttons -->                                                                
                                                    <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
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
