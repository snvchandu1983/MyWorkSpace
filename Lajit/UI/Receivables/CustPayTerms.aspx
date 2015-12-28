<%@ Page Language="C#"  AutoEventWireup="true" Codebehind="CustPayTerms.aspx.cs" Inherits="LAjitDev.Receivables.CustomerPayTerms"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
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
                                            <!-- entry form start -->
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
                                                <!--Page Controls-->
                                                <tr>
                                                    <td align="left" valign="top">
                                                         <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="350px" ></LCtrl:LajitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr> 
                                                            <tr id="trPayInstruct" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblPayInstruct"  SkinID="Label"></asp:Label>
                                                               </td>
                                                               <td valign="middle">
                                                                   <LCtrl:LajitTextBox ID="txtPayInstruct" runat="server" MapXML="PayInstruct" Width="350px" ></LCtrl:LajitTextBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqPayInstruct" MapXML="PayInstruct" runat="server" ControlToValidate="txtPayInstruct" ValidationGroup="LAJITEntryForm"
                                                                       ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                               </td>
                                                            </tr>
                                                            <tr id="trDueDays" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblDueDays" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtDueDays" runat="server" MapXML="DueDays" Width="350px" ></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDueDays" MapXML="DueDays" runat="server" ControlToValidate="txtDueDays" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regDueDays" runat="server" ControlToValidate="txtDueDays" MapXML="DueDays"
                                                                    ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                    Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDiscDays" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblDiscDays" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtDiscDays" runat="server" MapXML="DiscDays" Width="350px" ></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDiscDays" MapXML="DiscDays" runat="server" ControlToValidate="txtDiscDays" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                    <LCtrl:LAjitRegularExpressionValidator ID="regDiscDays" runat="server" ControlToValidate="txtDiscDays" MapXML="DiscDays"
                                                                        ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                        Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDiscount" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblDiscount" SkinID="Label"></asp:Label>
                                                               </td>
                                                               <td valign="middle">
                                                                   <LCtrl:LAjitTextBox ID="txtDiscount" runat="server" MapXML="Discount" Width="350px" ></LCtrl:LAjitTextBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqDiscount" MapXML="Discount" runat="server" ControlToValidate="txtDiscount" ValidationGroup="LAJITEntryForm"
                                                                       ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                   <LCtrl:LAjitRegularExpressionValidator ID="regDiscount" runat="server" ControlToValidate="txtDiscount" MapXML="Discount"
                                                                       ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                       Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                               </td>
                                                            </tr>
                                                            <tr id="trOverNightIns" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblOverNightIns" SkinID="Label"></asp:Label>
                                                               </td>
                                                               <td valign="middle">
                                                                   <LCtrl:LAjitTextBox ID="txtOverNightIns" runat="server" MapXML="OverNightIns" Width="350px" ></LCtrl:LAjitTextBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqOverNightIns" MapXML="OverNightIns" runat="server" ControlToValidate="txtOverNightIns" ValidationGroup="LAJITEntryForm"
                                                                       ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                               </td>
                                                            </tr>
                                                            <tr id="trWiringInst" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblWiringInst" SkinID="Label"></asp:Label>
                                                               </td>
                                                               <td valign="middle">
                                                                   <LCtrl:LAjitTextBox ID="txtWiringInst" runat="server" MapXML="WiringInst" Width="350px" ></LCtrl:LAjitTextBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqWiringInst" MapXML="WiringInst" runat="server" ControlToValidate="txtWiringInst" ValidationGroup="LAJITEntryForm"
                                                                       ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                               </td>
                                                            </tr>
                                                            <tr id="trFooterMessage" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblFooterMessage" SkinID="Label"></asp:Label>
                                                               </td>
                                                               <td valign="middle">
                                                                   <LCtrl:LAjitTextBox ID="txtFooterMessage" runat="server" MapXML="FooterMessage" Width="350px" ></LCtrl:LAjitTextBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqFooterMessage" MapXML="FooterMessage" runat="server" ControlToValidate="txtFooterMessage" ValidationGroup="LAJITEntryForm"
                                                                       ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                  
                                                               </td>
                                                            </tr>
                                                            <tr id="trIsLogoPrinted" align="left" valign="middle" runat="server">
                                                               <td class="formtext" style="height: 24px; width: 160px">
                                                                   <asp:Label runat="server" ID="lblIsLogoPrinted" SkinID="Label"></asp:Label>
                                                               </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitCheckBox ID="chkIsLogoPrinted" runat="server" MapXML="IsLogoPrinted"/>
                                                                </td>
                                                           </tr>
                                                           <tr id="trLogoCaption" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px" valign="top">
                                                                    <asp:Label runat="server" ID="lblLogoCaption"  SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LajitTextBox ID="txtLogoCaption" runat="server" MapXML="LogoCaption" TextMode="MultiLine"  Rows="2" Columns="5" Width="350px" ></LCtrl:LajitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLogoCaption" MapXML="LogoCaption" runat="server" ControlToValidate="txtLogoCaption" ValidationGroup="LAJITEntryForm"
                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
                                                                                 OnClientClick="javascript:return ValidateControls()" Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click" 
                                                                                 OnClientClick="javascript:ValidateControls()"  Height="18" AlternateText="ContinueAdd" />
                                                                            </td>
                                                                           <td>
                                                                                <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click" 
                                                                                 OnClientClick="javascript:ValidateControls()"  Height="18" AlternateText="AddClone"/>
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
                                            <table cellpadding="4" cellspacing="4" border="1"  width="800px" align="center">
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
</asp:Content>