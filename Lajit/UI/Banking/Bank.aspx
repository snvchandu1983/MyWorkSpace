<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Bank.aspx.cs" Inherits="LAjitDev.Banking.Bank" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
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
                                <asp:Panel ID="pnlEntryForm" runat="server" Height="511px" width="100%" style="background-color:White;">
                                    <!-- entry form start -->
                                    <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <!--Pop up header tr-->
                                        <tr style="height:24px;" runat="server" id="trPopupHeader" visible="false" valign="top">
                                            <td> 
                                               <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                                                    <tr style="height: 24px;">
                                                        <td id="htcPopEntryForm" runat="server" align="center" class="grdVwRPTitle" style="height: 24px; border-width: 0px">
                                                            <asp:Label id="lblPopupEntry" runat="server" height="24px" ></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td> 
                                        </tr>
                                        <!--ProcessLinks tr-->
                                        <tr  style="height:10px;">
                                           <td align="right">
                                               <table cellpadding="0" cellspacing="0" border="0" style="height: 10px;">
                                                     <tr style="height: 10px;" id="trProcessLinks" align="right" valign="middle" runat="server">
                                                        <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                            <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                            </asp:Panel>
                                                        </td>
                                                     </tr>
                                                </table>
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
                                        <!-- tab headings start -->
                                        <tr style="height:21px;">
                                           <td valign="bottom">
                                                <div id="divGeneralTab" style="display:block;">
                                                    <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                         <tr align="left" valign="bottom" >
                                                            <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                            <td align="center" valign="middle" class="bluebold" style="width:85px; background-color:#c5d5fc;">General</td>
                                                            <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                            <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>                                                         
                                                            <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                            <td onclick="javascript:HideShowDiv('Other');" align="center" class="blueboldlinks01" valign="middle" style="width:110px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                Other
                                                            </td>
                                                            <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                            <td style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                     </table>
                                                </div>
                                                <div id="divOtherTab" style="display:none">
                                                    <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                          <tr align="left" valign="bottom">
                                                            <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                            <td onclick="javascript:HideShowDiv('General');"  class="blueboldlinks01" align="center" valign="middle" style=" width:85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                            General
                                                            </td>
                                                            <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                            <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                            <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                            <td align="center" valign="middle" class="bluebold" style="width:110px; background-color:#c5d5fc; ">Other</td>
                                                            <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                            <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                          </tr>
                                                   </table>
                                               </div>
                                           </td>
                                         </tr>
                                        <!-- form entry fields start -->
                                        <tr>
                                            <td align="left" valign="top">
                                                  <table width="100%" style="height:390px" border="0" cellspacing="0" cellpadding="0" class="formmiddle"> 
                                                    <tr style="border-style:none;">
                                                        <td colspan="2" valign="top">
                                                              <!-- Panel General Info Start -->
                                                              <table width="100%"  border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                      <!--Left Table Start -->
                                                                      <div id="divGeneral" style="display:block;">
                                                                       <table cellspacing="0" cellpadding="0" border="0">
                                                                         <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                            <td style="width: 160px; height: 24px" class="formtext">
                                                                             &nbsp;
                                                                            </td>
                                                                            <td valign="middle">
                                                                               <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />
                                                                            </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trFinInst" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblFinInst" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtFinInst" runat="server" MapXML="FinInst" Width="430px" TabIndex="1"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqFinInst" MapXML="FinInst" runat="server" ControlToValidate="txtFinInst" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trAccountName" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblAccountName" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtAccountName" runat="server" MapXML="AccountName" Width="430px" TabIndex="2"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAccountName" MapXML="AccountName" runat="server" ControlToValidate="txtAccountName" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trBankType" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblBankType" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlBankType" runat="server" MapXML="BankType" Width="434px" TabIndex="3"></LCtrl:LAjitDropDownList>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqBankType" MapXML="BankType" runat="server" ControlToValidate="ddlBankType" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trAccountNumber" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblAccountNumber" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtAccountNumber" runat="server" MapXML="AccountNumber" Width="430px" TabIndex="4"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAccountNumber" MapXML="AccountNumber" runat="server" ControlToValidate="txtAccountNumber" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trRoutingNumber" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblRoutingNumber" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:196px;">
                                                                                    <LCtrl:LajitTextBox ID="txtRoutingNumber" runat="server" MapXML="RoutingNumber" Width="180px" TabIndex="5"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqRoutingNumber" MapXML="RoutingNumber" runat="server" ControlToValidate="txtRoutingNumber" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trCorpBankAccount" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCorpBankAccount" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCorpBankAccount" runat="server" MapXML="CorpBankAccount" Width="434px" TabIndex="6"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCorpBankAccount" MapXML="CorpBankAccount" runat="server" ControlToValidate="ddlCorpBankAccount" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trCorpJob1" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCorpJob1" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCorpJob1" runat="server" MapXML="CorpJob1" Width="434px" TabIndex="7"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCorpJob1" MapXML="CorpJob1" runat="server" ControlToValidate="ddlCorpJob1" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trCheckStock" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCheckStock" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCheckStock" runat="server" MapXML="CheckStock" Width="434px" TabIndex="8"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCheckStock" MapXML="CheckStock" runat="server" ControlToValidate="ddlCheckStock" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trLastCheckNumber" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblLastCheckNumber" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtLastCheckNumber" runat="server" MapXML="LastCheckNumber" Width="430px" TabIndex="9"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLastCheckNumber" MapXML="LastCheckNumber" runat="server" ControlToValidate="txtLastCheckNumber" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trControlAccount" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblControlAccount" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtControlAccount" runat="server" MapXML="ControlAccount" Width="430px" TabIndex="10"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqControlAccount" MapXML="ControlAccount" runat="server" ControlToValidate="txtControlAccount" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trCorpAccounts" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCorpAccounts" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCorpAccounts" runat="server" MapXML="CorpAccounts" Width="434px" TabIndex="11"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCorpAccounts" MapXML="CorpAccounts" runat="server" ControlToValidate="ddlCorpAccounts" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trCorpJob2" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCorpJob2" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCorpJob2" runat="server" MapXML="CorpJob2" Width="434px" TabIndex="12"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCorpJob2" MapXML="CorpJob2" runat="server" ControlToValidate="ddlCorpJob2" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trExpiresMMYYYY" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblExpiresMMYYYY" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtExpiresMMYYYY" runat="server" MapXML="ExpiresMMYYYY" Width="430px" TabIndex="13"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqExpiresMMYYYY" MapXML="ExpiresMMYYYY" runat="server" ControlToValidate="txtExpiresMMYYYY" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr id="trClosedAccount" align="left" valign="top" runat="server">
                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblClosedAccount" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:450px;">
                                                                                <LCtrl:LAjitCheckBox ID="chkClosedAccount" runat="server" MapXML="ClosedAccount" />
                                                                           </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trUploadType" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblUploadType" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlUploadType" runat="server" MapXML="UploadType" Width="434px" TabIndex="14"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqUploadType" MapXML="UploadType" runat="server" ControlToValidate="ddlUploadType" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trBPGID" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label runat="server" ID="lblBPGID" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtBPGID" runat="server" MapXML="BPGID" Width="430px" TabIndex="10"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBPGID" MapXML="BPGID" runat="server" ControlToValidate="txtBPGID" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trPageInfo" runat="server">
                                                                               <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label runat="server" ID="lblPageInfo" SkinID="Label"></asp:Label></td>
                                                                               <td valign="middle" style="width:450px;">
                                                                                    <LCtrl:LajitTextBox ID="txtPageInfo" runat="server" MapXML="PageInfo" Width="430px" TabIndex="10"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPageInfo" MapXML="PageInfo" runat="server" ControlToValidate="txtPageInfo" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                               </td>
                                                                         </tr>
                                                                         <tr id="trCCUser_UserAdmin" align="left" valign="top" visible="true" runat="server">
                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label ID="lblCCUser" runat="server" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LAjitListBox ID="lstBxCCUser" runat="server" SelectionMode="Multiple" MapXML="CCUser" 
                                                                                    AutoPostBack="false" XMLType="ParentChild" Height="120" Width="180">
                                                                                </LCtrl:LAjitListBox>
                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqCCUser" runat="server" MapXML="CCUser" ControlToValidate="lstBxCCUser" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr> 
                                                                        </table>
                                                                       </div> 
                                                                      <div id="divAddress" style="display:none";>
                                                                       <table cellspacing="0" cellpadding="0" border="0">
                                                                         <tr align="left" valign="top" runat="server">
                                                                            <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                <asp:Label runat="server" ID="lblAddress1_BankAddress" SkinID="LabelSubHead" MapBranchNode="BankAddress"></asp:Label>
                                                                            </td>
                                                                         </tr>
                                                                         <tr align="left" valign="top" id="trAddressType_BankAddress" runat="server">
                                                                            <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlAddressType_BankAddress" runat="server" MapXML="AddressType" MapBranchNode="BankAddress" Width="130px" TabIndex="14"></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlAddressType_BankAddress_SelectedIndexChanged" -->
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_BankAddress" MapXML="AddressType" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtAddress1_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtAddress1_BankAddress" runat="server" MapXML="Address1" MapBranchNode="BankAddress" Width="180px" TabIndex="16"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_BankAddress" MapXML="Address1" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtAddress1_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trAddress2_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_BankAddress" runat="server" MapXML="IsPrimary" MapBranchNode="BankAddress" TabIndex="15"/>
                                                                                <asp:Label  runat="server" ID="lblIsPrimary_BankAddress"  SkinID="LabelBranch" MapBranchNode="BankAddress"></asp:Label>
                                                                             </td>   
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtAddress2_BankAddress" runat="server" MapXML="Address2" MapBranchNode="BankAddress" Width="180px" TabIndex="17"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_BankAddress" MapXML="Address2" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtAddress2_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trAddress3_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;"></td>
                                                                                
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtAddress3_BankAddress" runat="server" MapXML="Address3" MapBranchNode="BankAddress" Width="180px" TabIndex="35"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_BankAddress" MapXML="Address3" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtAddress3_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trAddress4_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;"></td>
                                                                                
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtAddress4_BankAddress" runat="server" MapXML="Address4" MapBranchNode="BankAddress" Width="180px" TabIndex="36"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_BankAddress" MapXML="Address4" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtAddress4_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trCity_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblCity_BankAddress"  SkinID="LabelBranch" MapBranchNode="BankAddress"></asp:Label></td>
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtCity_BankAddress" runat="server" MapXML="City" MapBranchNode="BankAddress" Width="180px" TabIndex="18"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCity_BankAddress" MapXML="City" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtCity_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                       </tr>
                                                                         <tr align="left" valign="top" id="trArea_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblArea_BankAddress"  MapBranchNode="BankAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtArea_BankAddress" runat="server" MapXML="Area" MapBranchNode="BankAddress" Width="180px" TabIndex="19"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqArea_BankAddress" MapXML="Area" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtArea_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trSelectCountry_BankAddress" runat="server">
                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblSelectCountry_BankAddress" MapBranchNode="BankAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlSelectCountry_BankAddress" runat="server" MapXML="SelectCountry" MapBranchNode="BankAddress" Width="184px" TabIndex="20"></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_BankAddress" MapXML="SelectCountry" MapBranchNode="BankAddress" runat="server" ControlToValidate="ddlSelectCountry_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td> 
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trSelectRegion_BankAddress" runat="server">
                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label runat="server" ID="lblSelectRegion_BankAddress"  SkinID="LabelBranch" MapBranchNode="BankAddress"></asp:Label></td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlSelectRegion_BankAddress" runat="server" MapXML="SelectRegion" MapBranchNode="BankAddress" Width="184px" TabIndex="21"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_BankAddress" MapXML="SelectRegion" MapBranchNode="BankAddress" runat="server" ControlToValidate="ddlSelectRegion_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>   
                                                                        </tr>
                                                                         <tr align="left" valign="top" id="trPostalCode_BankAddress" runat="server">
                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                <asp:Label  runat="server" ID="lblPostalCode_BankAddress"  SkinID="LabelBranch" MapBranchNode="BankAddress"></asp:Label></td>
                                                                           <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LajitTextBox ID="txtPostalCode_BankAddress" runat="server" MapXML="PostalCode" MapBranchNode="BankAddress" Width="180px" TabIndex="22"></LCtrl:LajitTextBox>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_BankAddress" MapXML="PostalCode" MapBranchNode="BankAddress" runat="server" ControlToValidate="txtPostalCode_BankAddress" ValidationGroup="LAJITEntryForm"
                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                           </td>
                                                                       </tr>
                                                                       </table>
                                                                       </div>
                                                                      <!--Left Table End -->
                                                                    </td>
                                                                     
                                                                    <td valign="middle" style="width:10px; ">
                                                                        <!-- Spacer start -->
                                                                          <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                        <!-- Spacer end --> 
                                                                    </td>    
                                                                   
                                                                    <td valign="top">
                                                                         <div id="divOther" style="display:none">
                                                                        <!--Right Table Start-->
                                                                         <table cellspacing="0" cellpadding="0" border="0">
                                                                             <tr id="Tr2" align="left" valign="top" runat="server">
                                                                                <td colspan="2" align="left" valign="top" class="formSubHeadText" style="height:30px">
                                                                                    <asp:Label runat="server" ID="lblTelephone_BankPhone"  MapBranchNode="BankPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                </td>
                                                                             </tr>
                                                                             <tr align="left" valign="top" id="trPhoneType_BankPhone" runat="server">
                                                                                <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                     <LCtrl:LAjitDropDownList ID="ddlPhoneType_BankPhone" runat="server" MapXML="PhoneType" MapBranchNode="BankPhone" Width="130px" TabIndex="23"></LCtrl:LAjitDropDownList><!-- AutoPostBack="true" OnSelectedIndexChanged="ddlPhoneType_BankPhone_SelectedIndexChanged" -->
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_BankPhone" MapXML="PhoneType" MapBranchNode="BankPhone" runat="server" ControlToValidate="txtTelephone_BankPhone" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td> 
                                                                                <td valign="middle" style="width:196px; ">
                                                                                  <LCtrl:LajitTextBox ID="txtTelephone_BankPhone" runat="server" MapXML="Telephone" MapBranchNode="BankPhone" Width="180px" TabIndex="25"></LCtrl:LajitTextBox>
                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_BankPhone" MapXML="Telephone" MapBranchNode="BankPhone" runat="server" ControlToValidate="txtTelephone_BankPhone" ValidationGroup="LAJITEntryForm"
                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                
                                                                                </td>
                                                                           </tr>
                                                                             <tr align="left" valign="top" id="trIsPrimary_BankPhone" runat="server">
                                                                                <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_BankPhone" runat="server" MapXML="IsPrimary" MapBranchNode="BankPhone" TabIndex="24"/>
                                                                                    <asp:Label  runat="server" ID="lblIsPrimary_BankPhone"  SkinID="LabelBranch" MapBranchNode="BankPhone"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; "></td>
                                                                           </tr>
                                                                            
                                                                             <tr id="Tr4" align="left" valign="top" runat="server">
                                                                                <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                    <asp:Label  runat="server" ID="lblEmail_BankEmail" MapBranchNode="BankEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                </td>
                                                                             </tr>
                                                                             <tr align="left" valign="top" id="trEmailType_BankEmail"  runat="server">
                                                                                <td align="left" valign="top" style="height:20px; width:141px;">
                                                                                     <LCtrl:LAjitDropDownList ID="ddlEmailType_BankEmail" runat="server" MapXML="EmailType" MapBranchNode="BankEmail" Width="130px" TabIndex="26"></LCtrl:LAjitDropDownList><!-- AutoPostBack="true" OnSelectedIndexChanged="ddlEmailType_BankEmail_SelectedIndexChanged" -->
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_BankEmail" MapXML="EmailType" MapBranchNode="BankEmail" runat="server" ControlToValidate="txtEmail_BankEmail" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td> 
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtEmail_BankEmail" runat="server" MapXML="Email" MapBranchNode="BankEmail" Width="180px" TabIndex="28"></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_BankEmail" MapXML="Email" MapBranchNode="BankEmail" runat="server" ControlToValidate="txtEmail_BankEmail" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regEmail_BankEmail" runat="server" ControlToValidate="txtEmail_BankEmail" MapXML="Email"
                                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>
                                                                                </td>
                                                                              </tr>
                                                                             <tr align="left" valign="top" id="trIsPrimary_BankEmail" runat="server">
                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_BankEmail" runat="server" MapXML="IsPrimary" MapBranchNode="BankEmail" TabIndex="27"/>
                                                                                    <asp:Label  runat="server" ID="lblIsPrimary_BankEmail"  SkinID="LabelBranch" MapBranchNode="BankEmail"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; "></td>
                                                                             </tr>
                                                                           
                                                                             <tr id="Tr5" align="left" valign="top" runat="server">
                                                                                <td colspan="2" align="left" valign="top" class="formSubHeadText" style="height:30px">
                                                                                    <asp:Label  runat="server" ID="lblWebsite_BankWebsite" MapBranchNode="BankWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                </td>
                                                                           </tr>
                                                                             <tr align="left" valign="top" id="trWebAddressType_BankWebsite" runat="server">
                                                                                <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                     <LCtrl:LAjitDropDownList ID="ddlWebAddressType_BankWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="BankWebsite" Width="130px" TabIndex="29"></LCtrl:LAjitDropDownList><!-- AutoPostBack="true" OnSelectedIndexChanged="ddlWebAddressType_BankWebsite_SelectedIndexChanged" -->
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_BankWebsite" MapXML="WebAddressType" MapBranchNode="BankWebsite" runat="server" ControlToValidate="txtWebsite_BankWebsite" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td> 
                                                                                <td valign="middle" style="width:196px;">
                                                                                        <LCtrl:LajitTextBox ID="txtWebsite_BankWebsite" runat="server" MapXML="Website" MapBranchNode="BankWebsite" Width="180px" TabIndex="31"></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_BankWebsite" MapXML="Website" MapBranchNode="BankWebsite" runat="server" ControlToValidate="txtWebsite_BankWebsite" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>
                                                                           </tr>
                                                                             <tr align="left" valign="top" id="trIsPrimary_BankWebsite" runat="server">
                                                                                <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_BankWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="BankWebsite" TabIndex="30"/>
                                                                                    <asp:Label runat="server" ID="lblIsPrimary_BankWebsite" SkinID="LabelBranch" MapBranchNode="BankWebsite"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; "></td>
                                                                           </tr> 
                                                                         </table>
                                                                         </div>
                                                                        <!--Right Table End--> 
                                                                    </td>
                                                                    
                                                                    <td>
                                                                        <!-- spacer start -->
                                                                        &nbsp;
                                                                        <!-- spacer end -->
                                                                   </td>
                                                                </tr>
                                                              </table>
                                                              <!-- Panel Other Info Start -->
                                                             <table width="100%"  border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                              <tr>
                                                                  <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                              </tr> 
                                                              <tr>
                                                                 <td valign="top">
                                                                 </td>
                                                                 <td valign="middle" style="width:5px; ">
                                                                    <!-- Spacer start -->
                                                                         <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                     <!-- Spacer end --> 
                                                                 </td>
                                                                 <td>
                                                                   <!--Right Table Start-->
                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                    </table>
                                                                   <!--Right Table End-->
                                                                 </td>
                                                                 <td>
                                                                    <!-- spacer start -->
                                                                           &nbsp;
                                                                    <!-- spacer end -->
                                                                 </td>
                                                              </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
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
                                                             <tr style="height:10px">
                                                            <td>
                                                                <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                    </asp:Timer>
                                </asp:Panel>
                                <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                    <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
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
<%--<script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>
<script type="text/javascript" src="../JavaScript/BranchDropDowns.js"></script>--%>
<script language="javascript" type="text/javascript">
function HideShowDiv(divname)
{
    switch (divname)
    {
        case "General":
              //content
              document.getElementById("divGeneral").style.display="block"; 
              document.getElementById("divOther").style.display="none";
              //Tabs  
              document.getElementById("divGeneralTab").style.display="block";
              document.getElementById("divOtherTab").style.display="none";
              document.getElementById("divAddress").style.display="none"; 
              break;
        case "Other": 
              //content
              document.getElementById("divOther").style.display="block";
              document.getElementById("divGeneral").style.display="none";
              //Tabs
              document.getElementById("divOtherTab").style.display="block"; 
              document.getElementById("divGeneralTab").style.display="none";
              document.getElementById("divAddress").style.display="block";
              
              break;
    }
}    
    </script>
</asp:content>
