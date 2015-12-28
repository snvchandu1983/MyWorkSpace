<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AccountFin.aspx.cs" Inherits="LAjitDev.Financials.AccountFin"%>

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
                                        <asp:Panel ID="pnlGVContent" runat="server" align="left" >
                                              <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <!--EntryForm tr-->                                                                
                                <tr align="left">
                                    <td class="tdEntryFrm">
                                        <asp:Panel ID="pnlEntryForm" runat="server"  Height="511px">                                           
                                            <!-- entry form start  -->
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
                                                            <!-- VIEW only display -->
                                                           <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td style="width: 160px; height: 24px" class="formtext">&nbsp</td>
                                                                <td valign="middle">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click"/>
                                                                </td>
                                                             </tr>
                                                           
                                                            <tr id="trNumberID" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblNumberID" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitTextBox ID="txtNumberID" runat="server" Width="290px" MapXML="NumberID"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqtxtNumberID" runat="server" Enabled="false"
                                                                            MapXML="NumberID" SetFocusOnError="True" ToolTip="Value is required." ErrorMessage="Value is required."
                                                                            ValidationGroup="LAJITEntryForm" ControlToValidate="txtNumberID"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDescription" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblDescription" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitTextBox ID="txtDescription" runat="server" Width="290px" MapXML="Description"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqTxtDescription" runat="server" Enabled="false"
                                                                            MapXML="Description" SetFocusOnError="True" ToolTip="Value is required." ErrorMessage="Value is required."
                                                                            ValidationGroup="LAJITEntryForm" ControlToValidate="txtDescription"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trNoOverride" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblNoOverride" SkinID="Label" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitCheckBox ID="chkNoOverride" runat="server" MapXML="NoOverride"></LCtrl:LAjitCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountType" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblAccountType" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitDropDownList ID="ddlAccountType" runat="server" Width="294px" MapXML="AccountType">
                                                                        </LCtrl:LAjitDropDownList>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAccountType" runat="server" Enabled="false"
                                                                            MapXML="AccountType" SetFocusOnError="True" ToolTip="Value is required." ErrorMessage="Value is required."
                                                                            ValidationGroup="LAJITEntryForm" ControlToValidate="ddlAccountType"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAllowJournal" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblAllowJournal" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitCheckBox ID="chkAllowJournal" runat="server" MapXML="AllowJournal"></LCtrl:LAjitCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trRollupAccount" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblRollupAccount" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitCheckBox ID="chkRollupAccount" runat="server" MapXML="RollupAccount"></LCtrl:LAjitCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCurrencyTypeCompany" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblCurrencyTypeCompany" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" Width="146px"
                                                                            MapXML="CurrencyTypeCompany">
                                                                        </LCtrl:LAjitDropDownList>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" Enabled="false"
                                                                            MapXML="CurrencyTypeCompany" SetFocusOnError="True" ToolTip="Value is required."
                                                                            ErrorMessage="Value is required." ValidationGroup="LAJITEntryForm" ControlToValidate="ddlCurrencyTypeCompany"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDocConsol" valign="middle" align="left" runat="server">
                                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                                        <asp:Label ID="lblDocConsol" runat="server" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <LCtrl:LAjitDropDownList ID="ddlDocConsol" runat="server" Width="146px" MapXML="DocConsol">
                                                                        </LCtrl:LAjitDropDownList>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqDocConsol" runat="server" Enabled="false"
                                                                            MapXML="DocConsol" SetFocusOnError="True" ToolTip="Value is required." ErrorMessage="Value is required."
                                                                            ValidationGroup="LAJITEntryForm" ControlToValidate="ddlDocConsol"></LCtrl:LAjitRequiredFieldValidator>
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
                                                               <td style="height: 24px;" colspan="2" align="left">
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
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
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
