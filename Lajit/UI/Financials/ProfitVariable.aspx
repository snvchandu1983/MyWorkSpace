<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ProfitVariable.aspx.cs"
    Inherits="LAjitDev.Financials.ProfitVariable" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
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
                                        <asp:Panel ID="pnlEntryForm" runat="server"  Height="511px">
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
                                                <tr style="height: 24px;">
                                                    <td align="right">
                                                        <table cellpadding="0" cellspacing="0" border="0" style="height: 24px;">
                                                            <tr style="height: 24px;" id="trProcessLinks" align="right" valign="middle" runat="server">
                                                                <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                                    <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td style="width: 160px; height: 24px" class="formtext">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                         OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trSelectProfitVariable" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblSelectProfitVariable" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectProfitVariable" runat="server" MapXML="SelectProfitVariable"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitVariable" runat="server" MapXML="SelectProfitVariable"
                                                                        ControlToValidate="ddlSelectProfitVariable" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                        Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        
                                                            <tr id="trSelectProfitAmt1" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblSelectProfitAmt1" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt1" runat="server" MapXML="SelectProfitAmt1"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt1" runat="server" MapXML="SelectProfitAmt1"
                                                                        ControlToValidate="ddlSelectProfitAmt1" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSelectProfVarExInc" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label ID="lblSelectProfVarExInc" runat="server" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectProfVarExInc" runat="server" MapXML="SelectProfVarExInc"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfVarExInc" runat="server" ControlToValidate="ddlSelectProfVarExInc"
                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfVarExInc" SetFocusOnError="True"
                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                              <td colspan="2">&nbsp;</td>
                                                            </tr>
                                                            <tr id="trAutoFillJob" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label ID="lblAutoFillJob" runat="server" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtAutoFillJob" runat="server" MapXML="AutoFillJob" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAutoFillJob" runat="server" ControlToValidate="txtAutoFillJob"
                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="AutoFillJob" SetFocusOnError="True"
                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trAutoFillAccount" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label ID="lblAutoFillAccount" runat="server" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtAutoFillAccount" runat="server" MapXML="AutoFillAccount" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAutoFillAccount" runat="server" ControlToValidate="txtAutoFillAccount"
                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="AutoFillAccount" SetFocusOnError="True"
                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trAutoFillSubAccount1" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label ID="lblAutoFillSubAccount1" runat="server" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtAutoFillSubAccount1" runat="server" MapXML="AutoFillSubAccount1" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAutoFillSubAccount1" runat="server" ControlToValidate="txtAutoFillSubAccount1"
                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="AutoFillSubAccount1" SetFocusOnError="True"
                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                              <td colspan="2">&nbsp;</td>
                                                            </tr>
                                                            <tr id="trSelectARBillType" runat="server" align="left" valign="middle">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblSelectARBillType" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectARBillType" runat="server" MapXML="SelectARBillType"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectARBillType" runat="server" MapXML="SelectARBillType"
                                                                        ControlToValidate="ddlSelectARBillType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
</asp:content>
