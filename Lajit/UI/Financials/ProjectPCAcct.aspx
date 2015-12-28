<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ProjectPCAcct.aspx.cs"
    Inherits="LAjitDev.Financials.ProjectPCAcct" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <contenttemplate>
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
                                            <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
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
                                                            
                                                            <tr>
                                                               <td valign="top" style="width: 50%;">
                                                                    <!-- Left Table Start -->
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr id="trPCVendor" align="left" valign="middle" runat="server">   
                                                                            <td  class="formtext" style="height: 24px; width:160px">
                                                                                <asp:Label   runat="server" ID="lblPCVendor" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitDropDownList ID="ddlPCVendor" runat="server" MapXML="PCVendor" Width="246px"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPCVendor" runat="server" MapXML="PCVendor" ControlToValidate="ddlPCVendor" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPayTo" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblPayTo" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtPayTo" runat="server" MapXML="PayTo" Width="240px"></LCtrl:LAjitTextBox>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqPayTo" runat="server" ControlToValidate="txtPayTo" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="PayTo"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPCAccounts" align="left" valign="middle" runat="server">  
                                                                             <td  class="formtext" style="height: 24px; width:160px">
                                                                                <asp:Label runat="server" ID="lblPCAccounts" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlPCAccounts" runat="server" MapXML="PCAccounts" Width="246px" ></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqPCAccounts" runat="server" MapXML="PCAccounts" ControlToValidate="ddlPCAccounts" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trFloatAmount" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label   runat="server" ID="lblFloatAmount" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtFloatAmount" runat="server" MapXML="FloatAmount" Width="240px"></LCtrl:LAjitTextBox>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqFloatAmount" runat="server" ControlToValidate="txtFloatAmount" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="FloatAmount"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trCustodianAcct" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label runat="server" ID="lblCustodianAcct" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitCheckBox ID="chkCustodianAcct" runat="server" MapXML="CustodianAcct" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- Left Table End -->
                                                               </td>
                                                                <td valign="top" style="width: 50%;">
                                                                    <!-- Right Table Start -->
                                                                     <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr id="trAdvances" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label   runat="server" ID="lblAdvances" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtAdvances" runat="server" MapXML="Advances" Width="200px"></LCtrl:LAjitTextBox>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqAdvances" runat="server" ControlToValidate="txtAdvances" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="Advances"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                    
                                                                        <tr id="trExpAlloc" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label   runat="server" ID="lblExpAlloc" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtExpAlloc" runat="server" MapXML="ExpAlloc" Width="200px"></LCtrl:LAjitTextBox>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqExpAlloc" runat="server" ControlToValidate="txtExpAlloc" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="ExpAlloc"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>

                                                                        <tr id="trRetDep" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label   runat="server" ID="lblRetDep" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtRetDep" runat="server" MapXML="RetDep" Width="200px"></LCtrl:LAjitTextBox>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqRetDep" runat="server" ControlToValidate="txtRetDep" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="RetDep"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>

                                                                        <tr id="trOwed" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                                <asp:Label   runat="server" ID="lblOwed" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <LCtrl:LAjitTextBox ID="txtOwed" runat="server" MapXML="Owed" Width="200px"></LCtrl:LAjitTextBox>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqOwed" runat="server" ControlToValidate="txtOwed" ValidationGroup="LAJITEntryForm" Enabled="false"
                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" MapXML="Owed"></LCtrl:LAjitRequiredFieldValidator> 
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <!-- Right Table End -->
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
