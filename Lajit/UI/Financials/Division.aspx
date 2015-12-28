<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Division.aspx.cs" Inherits="LAjitDev.Financials.Division" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
<%-- <script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>--%>
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
                                                 <tr style="height: 10px;" id="trProcessLinks" align="right" valign="middle" runat="server">                                                                                                                             
                                                    <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">                                                                    
                                                        </asp:Panel>
                                                    </td>
                                                 </tr>
                                                <!--Page subject tr 439-->
                                                 <tr id="trSubject" runat="server" visible="false" style="height:24px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label  SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="top" style="height:463px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                             <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                             <!-- VIEW only display -->
                                                             <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                  &nbsp;
                                                                </td>
                                                                <td valign="middle" align="left">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                             </tr>
                                                            
                                                            <tr id="trIsApproved" align="left" valign="middle" runat="server" visible="false">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label   runat="server" ID="lblIsApproved" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                  <LCtrl:LAjitCheckBox ID="chkIsApproved" runat="server" MapXML="IsApproved" />
                                                                </td>
                                                            </tr>
                                                            
                                                            
                                                            <tr id="trNumberID" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblNumberID"  SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LajitTextBox ID="txtNumberID" runat="server" MapXML="NumberID" Width="200px" ></LCtrl:LajitTextBox>                                                                                     
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqNumberID" MapXML="NumberID" runat="server" ControlToValidate="txtNumberID" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                </td>
                                                            </tr>   
                                                                     
                                                                     
                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="200px" ></LCtrl:LajitTextBox>                                                                                     
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                </td>
                                                            </tr>         
                                                                                                                                                                                                          
                                                            <tr id="trTypeOfJob" align="left" valign="middle" runat="server">   
                                                                <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label   runat="server" ID="lblTypeOfJob" SkinID="Label" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlTypeOfJob" runat="server" MapXML="TypeOfJob" Width="206px"></LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqTypeOfJob" runat="server" MapXML="TypeOfJob" ControlToValidate="ddlTypeOfJob" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                </td>
                                                            </tr>
                                                     
                                                            <tr id="trCenterType" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblCenterType"  SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                  <LCtrl:LAjitDropDownList ID="ddlCenterType" runat="server" MapXML="CenterType" Width="206px" ></LCtrl:LAjitDropDownList>
                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCenterType" runat="server" MapXML="CenterType" ControlToValidate="ddlCenterType" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>  
                                                                </td>
                                                            </tr>    
                                                                 
                                                            <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">  
                                                                 <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label   runat="server" ID="lblCurrencyTypeCompany" SkinID="Label" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                     <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="206px" ></LCtrl:LAjitDropDownList>
                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr id="trSelectSubAccount" align="left" valign="middle" visible="true" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px" valign="top">
                                                                    <asp:Label SkinID="Label" ID="lblSelectSubAccount" runat="server"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitListBox ID="lstBxSelectSubAccount" runat="server" SelectionMode="Multiple"
                                                                        MapXML="SelectSubAccount" XMLType="ParentChild" Height="120" Width="206">
                                                                    </LCtrl:LAjitListBox>
                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqSelectSubAccount" runat="server" MapXML="SelectSubAccount" ControlToValidate="lstBxSelectSubAccount" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required."  ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                </td>
                                                            </tr>
                                                            <!--space between page controls and submit button--> 
                                                            <tr>
                                                                <td style="height:15px"></td>
                                                            </tr>
                                                           <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
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
                                            <table cellpadding="4" cellspacing="4" border="0"  width="800px" align="center">
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
</asp:content>
