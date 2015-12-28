<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ModificationRequest.aspx.cs" Inherits="LAjitDev.Financials.ModificationRequest"%>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server" style="width: 100%;">
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
                                        <asp:Panel ID="pnlGVContent" runat="server" align="left">
                                            <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <!--EntryForm tr-->                                                                
                                <tr align="left">
                                   <!-- <td valign="top" style="height:720px;">-->
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
                                                 <tr style="height: 24px;" id="trProcessLinks" align="right" valign="top" runat="server">                                                                                                                             
                                                    <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">                                                                    
                                                        </asp:Panel>
                                                    </td>
                                                 </tr>
                                                <!--Page subject tr style="height:439px"-->
                                                <tr id="trSubject" runat="server" visible="false" style="height:24px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label  SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            
                                                           <tr>
                                                                <td valign="top" style="width: 50%;">
                                                                   <!-- Left Table Start -->
                                                                    <table cellpadding="0" border="0" cellspacing="0" >
                                                                        <tr id="trModReqestID" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblModReqestID" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtModReqestID" runat="server" MapXML="ModReqestID" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqModReqestID" MapXML="ModReqestID" runat="server" ControlToValidate="txtModReqestID" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>

                                                                        <tr id="trRequestOwner" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblRequestOwner" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtRequestOwner" runat="server" MapXML="RequestOwner" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqRequestOwner" MapXML="RequestOwner" runat="server" ControlToValidate="txtRequestOwner" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                                                                                                                                                                    
                                                                        <tr id="trBprocess" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblBprocess" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtBprocess" runat="server" MapXML="Bprocess" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqBprocess" MapXML="Bprocess" runat="server" ControlToValidate="txtBprocess" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trModificationStatus" align="left" valign="middle" runat="server">   
                                                                             <td  class="formtext" style="height: 24px; width:141px">
                                                                                <asp:Label runat="server" ID="lblModificationStatus" SkinID="Label" ></asp:Label>
                                                                             </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LAjitDropDownList ID="ddlModificationStatus" runat="server" MapXML="ModificationStatus" Width="184px"  ></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqModificationStatus" runat="server" MapXML="ModificationStatus" ControlToValidate="ddlModificationStatus" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trCreatedBy" align="left" valign="middle" runat="server">  
                                                                             <td  class="formtext" style="height: 24px; width:141px">
                                                                                <asp:Label runat="server" ID="lblCreatedBy" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlCreatedBy" runat="server" MapXML="CreatedBy" Width="184px" ></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqCreatedBy" runat="server" MapXML="CreatedBy" ControlToValidate="ddlCreatedBy" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr>                                                                     
                                                                        <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblDescription"  SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="240px" Height="110px" TextMode="MultiLine" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPrioritySeq" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblPrioritySeq" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtPrioritySeq" runat="server" MapXML="PrioritySeq" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPrioritySeq" MapXML="PrioritySeq" runat="server" ControlToValidate="txtPrioritySeq" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                                                                                                                                                                                                                                                                           
                                                                        <tr  id="trDeveloperType" align="left" valign="middle" runat="server" >  
                                                                             <td  class="formtext" style="height: 24px; width:141px">
                                                                                <asp:Label runat="server" ID="lblDeveloperType" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlDeveloperType" runat="server" MapXML="DeveloperType" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqDeveloperType" runat="server" MapXML="DeveloperType" ControlToValidate="ddlDeveloperType" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr>
                                                                        <tr  id="trDeveloper2Type" align="left" valign="middle" runat="server" >  
                                                                             <td  class="formtext" style="height: 24px; width:141px">
                                                                                <asp:Label runat="server" ID="lblDeveloper2Type" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlDeveloper2Type" runat="server" MapXML="Developer2Type" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqDeveloper2Type" runat="server" MapXML="Developer2Type" ControlToValidate="ddlDeveloper2Type" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr>                                                            
                                                                        <tr id="trDeveloperNotes" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblDeveloperNotes" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtDeveloperNotes" runat="server" MapXML="DeveloperNotes" Width="240px" Height="110px" TextMode="MultiLine"  ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqDeveloperNotes" MapXML="DeveloperNotes" runat="server" ControlToValidate="txtDeveloperNotes" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>   
                                                                       <tr id="trQANotes" align="left" valign="middle" runat="server">
                                                                            <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblQANotes" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LajitTextBox ID="txtQANotes" runat="server" MapXML="QANotes" Width="240px" Height="110px" TextMode="MultiLine"  ></LCtrl:LajitTextBox>                                                                                     
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqQANotes" MapXML="QANotes" runat="server" ControlToValidate="txtQANotes" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                            </td>
                                                                        </tr>                                                                                                                                          
                                                                        <tr  id="trUser2Type" align="left" valign="middle" runat="server" >  
                                                                             <td  class="formtext" style="height: 24px; width:141px">
                                                                                <asp:Label runat="server" ID="lblUser2Type" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                 <LCtrl:LAjitDropDownList ID="ddlUser2Type" runat="server" MapXML="User2Type" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqUser2Type" runat="server" MapXML="User2Type" ControlToValidate="ddlUser2Type" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                            </td>
                                                                        </tr>                                                                                                                                                 
                                                                    </table>
                                                                   <!-- Left Table End -->      
                                                                </td>
                                                                <td valign="top" style="width: 50%;">
                                                                    <!-- Right Table Start -->
                                                                     <table cellpadding="0" cellspacing="0" border="0" > 
                                                                        <tr id="trDateCreated" align="left" valign="middle" runat="server" >
                                                                           <td  class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                               <asp:Label runat="server" ID="lblDateCreated" SkinID="Label"></asp:Label>
                                                                           </td>
                                                                           <td valign="middle" style="width:196px; ">
                                                                              <table cellpadding="0" cellspacing="0" border="0">
                                                                               <tr>
                                                                                <td>
                                                                                  <LCtrl:LajitTextBox ID="txtDateCreated" runat="server" MapXML="DateCreated" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                </td>
                                                                             </tr>
                                                                              </table>      
                                                                           </td>
                                                                       </tr>     
                                                                        <tr id="trModificationSeverity" align="left" valign="middle" runat="server" >   
                                                                            <td  class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblModificationSeverity" SkinID="Label" ></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                                <LCtrl:LAjitDropDownList ID="ddlModificationSeverity" runat="server" MapXML="ModificationSeverity" Width="180px"></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqModificationSeverity" runat="server" MapXML="ModificationSeverity" ControlToValidate="ddlModificationSeverity" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>                                                                                                                                                                                              
                                                                        <tr id="trModificationType" align="left" valign="middle" runat="server">   
                                                                             <td  class="formtext" style="height: 24px; width:141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblModificationType" SkinID="Label" ></asp:Label>
                                                                             </td>
                                                                            <td valign="middle" style="width:196px;">
                                                                                <LCtrl:LAjitDropDownList ID="ddlModificationType" runat="server" MapXML="ModificationType" Width="180px"  ></LCtrl:LAjitDropDownList>
                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqModificationType" runat="server" MapXML="ModificationType" ControlToValidate="ddlModificationType" ValidationGroup="LAJITEntryForm"
                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                            </td>
                                                                        </tr>                                                                         
                                                                        <tr id="trDatedAssigned" align="left" valign="middle" runat="server" >
                                                                                <td  class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblDatedAssigned" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                  <table cellpadding="0" cellspacing="0" border="0">
                                                                                   <tr>
                                                                                    <td>
                                                                                      <LCtrl:LajitTextBox ID="txtDatedAssigned" runat="server" MapXML="DatedAssigned" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                   </td>
                                                                                    </tr>
                                                                                  </table>       
                                                                                </td>
                                                                          </tr>

                                                                        <tr id="trDateComplete" align="left" valign="middle" runat="server" >
                                                                            <td  class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblDateComplete" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                               <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr>
                                                                                <td>
                                                                                    <LCtrl:LajitTextBox ID="txtDateComplete" runat="server" MapXML="DateComplete" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                </td>
                                                                               </table>     
                                                                            </td>
                                                                      </tr>                                                                                                                                      

                                                                        <tr id="trReviseDate" align="left" valign="middle" runat="server" >
                                                                            <td  class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblReviseDate" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px; ">
                                                                               <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr>
                                                                                  <td>
                                                                                     <LCtrl:LajitTextBox ID="txtReviseDate" runat="server" MapXML="ReviseDate" Width="68px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                 </td>
                                                                               </tr>
                                                                               </table>     
                                                                            </td>
                                                                      </tr>        
                                                                       <tr id="trNotify" align="left" valign="middle">
                                                                            <td  class="formtext" style="height: 24px; width: 340px" valign="top" colspan="2">
                                                                            <div runat="server" id="divChecks" style="display:block;width:100%" >
                                                                            <table style="width:100%" border="0">
                                                                            <tr> 
                                                                              <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                <asp:Label runat="server" ID="lblNotify" SkinID="Label" visible=true>Notify</asp:Label>
                                                                            </td>
                                                                            <td valign="middle" style="width:196px;" align="left">
                                                                                <table style="width:100%" border="0">
                                                                                  <tr><td><input type="checkbox" id="chkOwner" runat="server"/>Owner</td></tr> 
                                                                                 <tr><td><input type="checkbox" id="chkRevisedBy" runat="server"/>Revised By</td></tr>
                                                                                 <tr><td><input type="checkbox" id="chkDevManager" runat="server" />Dev Manager</td></tr>
                                                                                 <tr><td><input type="checkbox" id="chkAssignedToDev" runat="server"/>Assigned To Dev</td></tr>
                                                                                 <tr><td><input type="checkbox" id="chkAssignedToQA" runat="server"/>Assigned To QA</td></tr>
                                                                               </table>
                                                                           </td>
                                                                           </tr>
                                                                         </table>
                                                                       </div> 
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
                                            <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
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
    <script type="text/javascript">

   window.onload = function() 
   {
      CheckSelected('ctl00_cphPageContents_ddlModificationStatus');
   }

  function CheckSelected(ddl)
  {
    if(document.getElementById(ddl)!=null)
   {  
    var ddlStatusIDs=document.getElementById(ddl).selectedIndex;
    var ddlType=ddl.split('_');
    var ddlName=ddlType[2];
    if(ddlName=='ddlModificationStatus')
    {
        if(ddlStatusIDs>0)
        {
             document.getElementById('ctl00_cphPageContents_divChecks').style.display='block';  
	         var ddlStatusValues=document.getElementById(ddl).options[ddlStatusIDs].value; 
	         var splitvalues=ddlStatusValues.split('~');
	         var ddlCheckValues=splitvalues[0];
	        
	         if(ddlCheckValues=='1')
	         {
	            document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
	            document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
	         }
	         if(ddlCheckValues=='90')
	         {
	            document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
	            document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
	         }   
	         if(ddlCheckValues=='110' || ddlCheckValues=='120')
	         {
	            document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
	            document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true; 
	         } 
	         if(ddlCheckValues=='210' || ddlCheckValues=='220')
	         {
	            document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
	            document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
	            document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
	            document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
	         }
	         if(ddlCheckValues=='300')
	         {
	           document.getElementById('ctl00_cphPageContents_chkOwner').checked=true;  
	           document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
	           document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
	           document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
	           document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
	         }
        }
		else
		{ 
			if(ddlStatusIDs==0)
			{
			  document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true;
			  document.getElementById('ctl00_cphPageContents_divChecks').style.display='none'; 
			} 
		}    
    }
	else
	{
		if(ddlName=='ddlDeveloperType')
		{ 
			if(ddlStatusIDs>0)
			{ 
			 var ddlStatusSelectedIDs=document.getElementById('ctl00_cphPageContents_ddlModificationStatus').selectedIndex;
			 var ddlStatusSelectedValues=document.getElementById('ctl00_cphPageContents_ddlModificationStatus').options[ddlStatusSelectedIDs].value;
			 
			 var ddlDeveloper2SelectedIDs=document.getElementById('ctl00_cphPageContents_ddlDeveloper2Type').selectedIndex;
				     
			 var splitvalues=ddlStatusSelectedValues.split('~');
			 var ddlCheckValues=splitvalues[0];
			
			 if(ddlStatusSelectedIDs>0)
			 {
			   if(ddlDeveloper2SelectedIDs==0)
			   { 
			     if(ddlCheckValues=='1' || ddlCheckValues=='90')
			     {
			       document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			       document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			       document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
			     }
			     if(ddlCheckValues=='110' || ddlCheckValues=='120')
			     {
			       document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			       document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			       document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false;
			     } 
			     if(ddlCheckValues=='210' || ddlCheckValues=='220')
		        {
		         document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
		         document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
		         document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
		         document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
		         document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true; 
		         }
			     if(ddlCheckValues=='300')
			     {
			       document.getElementById('ctl00_cphPageContents_chkOwner').checked=true;  
			       document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			       document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
			       document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false;
			     }
			    }
			    else
			    {
			      document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			      document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			      document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			      document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
			      document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true; 
			    }   
			  }
			}
			if(ddlStatusIDs==0)
			{
			  document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			  document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			  document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			  document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
			  document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false;
			} 
		}
		else
		{
			if(ddlName=='ddlDeveloper2Type')
			{ 
				if(ddlStatusIDs>0)
				{ 
				     var ddlStatusSelectedIDs=document.getElementById('ctl00_cphPageContents_ddlModificationStatus').selectedIndex;
				     var ddlStatusSelectedValues=document.getElementById('ctl00_cphPageContents_ddlModificationStatus').options[ddlStatusSelectedIDs].value;
    				 
				     var ddlDeveloperSelectedIDs=document.getElementById('ctl00_cphPageContents_ddlDeveloperType').selectedIndex;
				    
				     var splitvalues=ddlStatusSelectedValues.split('~');
				     var ddlCheckValues=splitvalues[0];
					 
				     if(ddlStatusSelectedIDs>0)
					 { 
					    if(ddlDeveloperSelectedIDs>0)
					    { 
						   if(ddlCheckValues=='1' || ddlCheckValues=='90' )
						   {
					           document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
					           document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
					           document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true; 
					         }
					        if(ddlCheckValues=='210' || ddlCheckValues=='220')
					        {
					         document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
					         document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
					         document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
					         document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
					         document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
					         }
					         if(ddlCheckValues=='110' || ddlCheckValues=='120')
					         {
					           document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
					           document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
					           document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true;
					         } 
					         if(ddlCheckValues=='300')
					         {
					           document.getElementById('ctl00_cphPageContents_chkOwner').checked=true;  
					           document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
					           document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
					           document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false;
					         } 
						} 
						else
						{
						   document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			               document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			               document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			               document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
			               document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true;
						}   
					 }
					 else
					 {
					      document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
					      document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
					      document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
					      document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=true; 
					      document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=true; 
	                 }					
				}    
			}
			if(ddlStatusIDs==0)
			{
			  document.getElementById('ctl00_cphPageContents_chkOwner').checked=false;  
			  document.getElementById('ctl00_cphPageContents_chkRevisedBy').checked=false; 
			  document.getElementById('ctl00_cphPageContents_chkDevManager').checked=true; 
			  document.getElementById('ctl00_cphPageContents_chkAssignedToDev').checked=false; 
			  document.getElementById('ctl00_cphPageContents_chkAssignedToQA').checked=false; 
			}  
		} 
	} 
	if(ddlStatusIDs==null)
	{
	 return true;
    } 
   }  
}
</script>
</asp:content>
