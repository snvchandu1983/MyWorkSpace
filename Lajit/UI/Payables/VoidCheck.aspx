<%@ Page Language="C#"  AutoEventWireup="true" Codebehind="VoidCheck.aspx.cs" Inherits="LAjitDev.Payables.VoidCheck"%>

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
                                            <table style="height: 200px" width="100%" border="0" cellspacing="0" cellpadding="0">
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
                                              
                                               <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                    <td style="width: 160px; height: 24px" class="formtext">
                                                     &nbsp;
                                                    </td>
                                                    <td valign="middle">
                                                       <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />
                                                    </td>
                                                </tr>
                                              
                                             <tr>
                                                  <td style="height:2px;">
                                                     <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5" height="1" />
                                                  </td>
                                              </tr>    
                                             <tr>
                                                <td width="100%" valign="top">
                                                 <!-- Quick Check Start -->
                                                   <table cellpadding="0" width="100%" border="0" cellspacing="0"  class="formcheckborder">
                                                        <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                           <td>
                                                             <!-- Table Autofill and  check no start SkinID="Label" LabelBig -->
                                                              <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                              <tr>
                                                                 <td align="left">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                      <tr align="left" valign="middle" id="trEntryBank_JournalDoc" runat="server">
                                                                          <td align="left" class="formtext" style="height:24px; width:75px;">
                                                                                <asp:Label  runat="server" ID="lblEntryBank_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                          </td>
                                                                          <td valign="left" style="width:216px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlEntryBank_JournalDoc" runat="server" MapXML="EntryBank" MapBranchNode="JournalDoc" Width="204px"  ></LCtrl:LAjitDropDownList>
                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank_JournalDoc" MapXML="EntryBank" MapBranchNode="JournalDoc" runat="server" ControlToValidate="ddlEntryBank_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                         </td>                                                                                     
                                                                       </tr>
                                                                    </table>
                                                                 </td>
                                                                 <td align="right">
                                                                     <table cellpadding="0" cellspacing="0" border="0">
                                                                       <tr align="left" valign="top" id="trCheckNumber_JournalDoc" runat="server">
                                                                                <td class="formtext" style="height:24px; width:121px;">
                                                                                         <asp:Label  runat="server" ID="lblCheckNumber_JournalDoc" MapBranchNode="JournalDoc" SkinID="LabelBig"></asp:Label>
                                                                                </td>
                                                                                 <td valign="middle" style="width:136px; ">
                                                                                        <LCtrl:LAjitTextBox ID="txtCheckNumber_JournalDoc" runat="server" MapXML="CheckNumber" MapBranchNode="JournalDoc" Width="68px"></LCtrl:LAjitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqCheckNumber_JournalDoc" MapXML="CheckNumber" MapBranchNode="JournalDoc" runat="server" ControlToValidate="txtCheckNumber_JournalDoc" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                </td>                                                                                     
                                                                            </tr>
                                                                     </table> 
                                                                 </td>   
                                                              </tr>
                                                              </table>
                                                             <!-- Table Autofill and  check no end -->
                                                           </td>
                                                        </tr>
                                                       <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                       <tr>
                                                         <td align="right">
                                                          <!-- pyament date -->
                                                           <table cellpadding="0" cellspacing="0" border="0">
                                                             <tr id="trVoidDate" align="left" valign="middle" runat="server">
                                                              <td class="formtext" style="height: 24px; width:100px" valign="top">
                                                                <asp:Label   runat="server" ID="lblVoidDate"  SkinID="Label"></asp:Label>
                                                            </td>
                                                             <td valign="middle" style="width:136px; ">
                                                               <table cellpadding="0" cellspacing="0" border="0">
                                                               <tr>
                                                                 <td>
                                                                    <LCtrl:LajitTextBox ID="txtVoidDate" runat="server" MapXML="VoidDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                 </td>
                                                                <td>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVoidDate" MapXML="VoidDate" runat="server" ControlToValidate="txtVoidDate" ValidationGroup="LAJITEntryForm"
                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                 </td>
                                                                 </tr>
                                                               </table>           
                                                            </td>
                                                            </tr> 
                                                             
                                                           </table>
                                                         <!-- pyament date -->
                                                         </td>
                                                       </tr>
                                                        <tr>
                                                           <td style="height:1px">&nbsp;</td>
                                                        </tr>
                                                      
                                                       <tr>
                                                          <td>
                                                             <!-- AutoFill vendor amount start -->
                                                             <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                              <tr>
                                                                <td>
                                                                 <!-- auto fill start -->
                                                                   <table cellpadding="0" cellspacing="0" border="0">
                                                                     <tr id="trVendor_JournalDoc" align="left" valign="middle" runat="server">
                                                                        <td class="formtext" style="height:24px;width:80px;"  valign="top">
                                                                            <asp:Label  runat="server" ID="lblVendor_JournalDoc" MapBranchNode="JournalDoc" SkinID="Label"></asp:Label>
                                                                        </td>
                                                                       <td valign="middle" style="width:396px; ">
                                                                         <LCtrl:LAjitLabel ID="lblVendor_JournalDoc_Value" MapBranchNode="JournalDoc"   runat="server" MapXML="Vendor" ></LCtrl:LAjitLabel>
                                                                        </td>
                                                                      </tr>     
                                                                    </table>
                                                                    <!-- auto fill end -->
                                                                </td>
                                                              </tr>
                                                             </table> 
                                                            <!-- AutoFill vendor amount end -->
                                                          </td>
                                                       </tr>
                                                       <tr>
                                                           <td style="height:50px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                           <td align="center">                                                
                                                            <!-- Check Messsage and Currency Start -->
                                                             <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                             <tr>
                                                               <td align="center" width="65%">
                                                                  <!-- Check Message Start -->
                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                   <tr align="center" valign="middle" id="trCheckMessage_JournalDoc" runat="server">
                                                                      <td style="width:236px" align="center">
                                                                        <LCtrl:LAjitLabel ID="lblCheckMessage_JournalDoc_Value" MapBranchNode="JournalDoc"   runat="server" MapXML="CheckMessage" ></LCtrl:LAjitLabel>
                                                                      </td>
                                                                    </tr>
                                                                  </table>
                                                                  <!-- Check Message Start -->
                                                               </td>
                                                             </tr>
                                                             </table>
                                                            <!-- Check Messsage and Currency End -->
                                                           </td>  
                                                        </tr>
                                                        <tr>
                                                           <td style="height:67px">&nbsp;</td>
                                                        </tr>
                                                      <!-- hidden fields start-->
                                                          
                                                        <tr id="trFinInst" align="left" valign="middle" runat="server">
                                                            <td class="formtext" style="height: 24px; width: 160px">
                                                                <asp:Label   runat="server" ID="lblFinInst"  SkinID="Label"></asp:Label>
                                                            </td>
                                                            <td valign="middle">
                                                                <LCtrl:LajitTextBox ID="txtFinInst" runat="server" MapXML="FinInst" Width="200px" ></LCtrl:LajitTextBox>                                                                                     
                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqFinInst" MapXML="FinInst" runat="server" ControlToValidate="txtFinInst" ValidationGroup="LAJITEntryForm"
                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                            </td>
                                                        </tr>   
                                                     <!-- hidden fields end-->
                                              </table>    
                                               <!-- Quick Check End -->
                                            </td>
                                           </tr>
                                                    <!--space between page controls and submit button--> 
                                                    <tr>
                                                        <td style="height:15px"></td>
                                                    </tr>   
                                                    <!--Submit and Cancel buttons-->                                                   
                                                <tr style="height:24px">
                                                              <td colspan="2" align="center">
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
                                                                <td colspan="2" align="center">
                                                                   <table border="0">
                                                                     <tr>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnPrevious" Tooltip="Previous"/>
                                                                       </td>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnNext" Tooltip="Next"/>
                                                                       </td> 
                                                                     </tr> 
                                                                    </table>
                                                                  </td>
                                                             </tr> 
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                         
                                                <!-- form entry fields end -->
                                         </asp:Panel>
                                       </td>
                                   </tr>
                                 </table>
                                     
                                      <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                      </asp:Timer>
                                       <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="0"  width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td><asp:Label ID="lblError" runat="server"  SkinID="LabelMsg"></asp:Label></td>
                                                </tr>
                                            </table>
                                       </asp:Panel>
                                    </td>
                   </tr>
                </table>
          </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>