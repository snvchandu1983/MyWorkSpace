<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ChartView.aspx.cs" Inherits="LAjitDev.Common.ChartView"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
                    <tr>
                        <!--Btns Usercontrol-->
                        <td class="tdBtnsUC">
                            <asp:Panel ID="pnlBtnsContent" runat="server" align="left" Style="display: none">
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
                                                                <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" Visible="false" />
                                                                 <!-- Chart control start --> 
                                                                 <CUC:CVUserControl ID="CUC" runat="server" ChartHeight="452" ChartWidth="807" ChartEnableViewState="false" />
                                                                 <!-- Chart control end -->
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
                                                            <tr id="trXaxisText" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblXaxisText" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtXaxisText" runat="server" MapXML="XaxisText" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqXaxisText" MapXML="XaxisText" runat="server"
                                                                        ControlToValidate="txtXaxisText" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trXaxisValue" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblXaxisValue" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtXaxisValue" runat="server" MapXML="XaxisValue" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqXaxisValue" MapXML="XaxisValue" runat="server"
                                                                        ControlToValidate="txtXaxisValue" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trYaxisText" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblYaxisText" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtYaxisText" runat="server" MapXML="YaxisText" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqYaxisText" MapXML="YaxisText" runat="server"
                                                                        ControlToValidate="txtYaxisText" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trYaxisValue" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblYaxisValue" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtYaxisValue" runat="server" MapXML="YaxisValue" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqYaxisValue" MapXML="YaxisValue" runat="server"
                                                                        ControlToValidate="txtYaxisValue" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trXaxisColor" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblXaxisColor" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtXaxisColor" runat="server" MapXML="XaxisColor" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqXaxisColor" MapXML="XaxisColor" runat="server"
                                                                        ControlToValidate="txtXaxisColor" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trXaxisFont" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblXaxisFont" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtXaxisFont" runat="server" MapXML="XaxisFont" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqXaxisFont" MapXML="XaxisFont" runat="server"
                                                                        ControlToValidate="txtXaxisFont" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trYaxisColor" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblYaxisColor" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtYaxisColor" runat="server" MapXML="YaxisColor" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqYaxisColor" MapXML="YaxisColor" runat="server"
                                                                        ControlToValidate="txtYaxisColor" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trYaxisFont" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblYaxisFont" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtYaxisFont" runat="server" MapXML="YaxisFont" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqYaxisFont" MapXML="YaxisFont" runat="server"
                                                                        ControlToValidate="txtYaxisFont" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trIsLegendDisplayed" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblIsLegendDisplayed" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtIsLegendDisplayed" runat="server" MapXML="IsLegendDisplayed"
                                                                        Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqIsLegendDisplayed" MapXML="IsLegendDisplayed"
                                                                        runat="server" ControlToValidate="txtIsLegendDisplayed" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                        Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trIsChartNameDisplayed" align="left" valign="top" runat="server">
                                                                <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblIsChartNameDisplayed" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtIsChartNameDisplayed" runat="server" MapXML="IsChartNameDisplayed"
                                                                        Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqIsChartNameDisplayed" MapXML="IsChartNameDisplayed"
                                                                        runat="server" ControlToValidate="txtIsChartNameDisplayed" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                        Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="ContinueAdd" />
                                                                        </td>
                                                                       <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClientClick="javascript:ValidateControls()" 
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
                                            <asp:Timer ID="timerEntryForm" runat="server"  Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
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
    <input type="hidden" runat="server" id="hidXML" enableviewstate="true" />
</asp:Content>
