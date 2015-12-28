<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Employee.aspx.cs" Inherits="LAjitDev.Financials.Employee" %>

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
                                                                    <td onclick="javascript:HideShowDiv('Other');" class="blueboldlinks01" align="center" valign="middle" style="width:110px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
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
                                                                    <td onclick="javascript:HideShowDiv('General');" class="blueboldlinks01" align="center" valign="middle" style="width:85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
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
                                                
                                                <tr>
                                                    <td align="left" valign="top">
                                                          <table  width="100%" style="height:390px" border="0" cellspacing="0" cellpadding="0" class="formmiddle"> 
                                                           <!-- form entry fields start -->
                                                            <tr style="border-style:none;">
                                                                <td colspan="2" valign="top">
                                                                    <!-- Panel General Info Start -->
                                                                    <div id="divGeneral" style="display:block;">
                                                                      <table width="100%"  border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">
                                                                              <!--Left Table Start -->
                                                                               <table cellspacing="0" cellpadding="0" border="0">
                                                                                 <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                                     <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                        &nbsp;
                                                                                     </td>
                                                                                    <td valign="middle" align="left">
                                                                                       <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" />
                                                                                    </td>
                                                                                 </tr>
                                                                                 <tr id="Tr3" align="left" valign="top" runat="server">
                                                                                     <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">                                                                                            
                                                                                         <asp:Label  runat="server" ID="lblName"  SkinID="LabelSubHead">Name</asp:Label>
                                                                                     </td> 
                                                                                 </tr> 
                                                                                 <tr align="left" valign="top" id="trLastName" runat="server">
                                                                                       <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label  runat="server" ID="lblLastName" SkinID="LabelBranch" ></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px;">
                                                                                            <LCtrl:LajitTextBox ID="txtLastName" runat="server" MapXML="LastName" Width="180px" TabIndex=1></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqLastName" MapXML="LastName" runat="server" ControlToValidate="txtLastName" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                 </tr>
                                                                                 <tr align="left" valign="top" id="trFirstName" runat="server">
                                                                                       <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblFirstName"  SkinID="LabelBranch"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtFirstName" runat="server" MapXML="FirstName" Width="180px" TabIndex=2></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqFirstName" MapXML="FirstName" runat="server" ControlToValidate="txtFirstName" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                  
                                                                                 <tr id="Tr1" align="left" valign="top" runat="server">
                                                                                    <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                        <asp:Label  runat="server" ID="lblAddress1_EmployeeAddress"  SkinID="LabelSubHead" MapBranchNode="EmployeeAddress"></asp:Label>
                                                                                    </td>
                                                                                 </tr>
                                                                                 <tr align="left" valign="top" id="trAddressType_EmployeeAddress" runat="server">
                                                                                    <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlAddressType_EmployeeAddress" runat="server" MapXML="AddressType" MapBranchNode="EmployeeAddress" Width="130px" TabIndex=3></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlAddressType_EmployeeAddress_SelectedIndexChanged" -->
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_EmployeeAddress" MapXML="AddressType" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtAddress1_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>
                                                                                    <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtAddress1_EmployeeAddress" runat="server" MapXML="Address1" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=5></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_EmployeeAddress" MapXML="Address1" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtAddress1_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trAddress2_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        <LCtrl:LAjitCheckBox ID="chkIsPrimary_EmployeeAddress" runat="server" MapXML="IsPrimary" MapBranchNode="EmployeeAddress" TabIndex=4/>
                                                                                        <asp:Label  runat="server" ID="lblIsPrimary_EmployeeAddress"  SkinID="LabelBranch" MapBranchNode="EmployeeAddress"></asp:Label>
                                                                                        
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtAddress2_EmployeeAddress" runat="server" MapXML="Address2" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=6></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_EmployeeAddress" MapXML="Address2" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtAddress2_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trAddress3_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtAddress3_EmployeeAddress" runat="server" MapXML="Address3" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=30></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_EmployeeAddress" MapXML="Address3" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtAddress3_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trAddress4_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtAddress4_EmployeeAddress" runat="server" MapXML="Address4" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=31></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_EmployeeAddress" MapXML="Address4" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtAddress4_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trCity_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label  runat="server" ID="lblCity_EmployeeAddress"  SkinID="LabelBranch" MapBranchNode="EmployeeAddress"></asp:Label></td>
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtCity_EmployeeAddress" runat="server" MapXML="City" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=7></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqCity_EmployeeAddress" MapXML="City" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtCity_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    </td>
                                                                               </tr>
                                                                                 <tr align="left" valign="top" id="trArea_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label  runat="server" ID="lblArea_EmployeeAddress"  MapBranchNode="EmployeeAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtArea_EmployeeAddress" runat="server" MapXML="Area" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=8></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqArea_EmployeeAddress" MapXML="Area" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtArea_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trSelectCountry_EmployeeAddress" runat="server">
                                                                                    <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label  runat="server" ID="lblSelectCountry_EmployeeAddress" MapBranchNode="EmployeeAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                    <td valign="middle" style="width:196px; ">
                                                                                         <LCtrl:LAjitDropDownList ID="ddlSelectCountry_EmployeeAddress" runat="server" MapXML="SelectCountry" MapBranchNode="EmployeeAddress" Width="184px" TabIndex=9></LCtrl:LAjitDropDownList>
                                                                                         <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_EmployeeAddress" MapXML="SelectCountry" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="ddlSelectCountry_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td> 
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trSelectRegion_EmployeeAddress" runat="server">
                                                                                    <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label runat="server" ID="lblSelectRegion_EmployeeAddress"  SkinID="LabelBranch" MapBranchNode="EmployeeAddress"></asp:Label></td>
                                                                                    <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectRegion_EmployeeAddress" runat="server" MapXML="SelectRegion" MapBranchNode="EmployeeAddress" Width="184px" TabIndex=10></LCtrl:LAjitDropDownList>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_EmployeeAddress" MapXML="SelectRegion" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="ddlSelectRegion_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>   
                                                                                </tr>
                                                                                 <tr align="left" valign="top" id="trPostalCode_EmployeeAddress" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label runat="server" ID="lblPostalCode_EmployeeAddress"  SkinID="LabelBranch" MapBranchNode="EmployeeAddress"></asp:Label></td>
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtPostalCode_EmployeeAddress" runat="server" MapXML="PostalCode" MapBranchNode="EmployeeAddress" Width="180px" TabIndex=11></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_EmployeeAddress" MapXML="PostalCode" MapBranchNode="EmployeeAddress" runat="server" ControlToValidate="txtPostalCode_EmployeeAddress" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                   </td>
                                                                               </tr>
                                                                               </table>
                                                                              <!--Left Table End -->
                                                                            </td>
                                                                             
                                                                            <td valign="middle" style="width:10px; ">
                                                                                <!-- Spacer start -->
                                                                                  <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                <!-- Spacer end --> 
                                                                            </td>    
                                                                           
                                                                            <td valign="top">
                                                                                <!--Right Table Start-->
                                                                                 <table cellspacing="0" cellpadding="0" border="0">
                                                                                     <tr id="Tr2" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblTelephone_EmployeePhone"  MapBranchNode="EmployeePhone" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                     <tr align="left" valign="top" id="trPhoneType_EmployeePhone" runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlPhoneType_EmployeePhone" runat="server" MapXML="PhoneType" MapBranchNode="EmployeePhone" Width="130px" TabIndex=12></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlPhoneType_EmployeePhone_SelectedIndexChanged" -->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_EmployeePhone" MapXML="PhoneType" MapBranchNode="EmployeePhone" runat="server" ControlToValidate="txtTelephone_EmployeePhone" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                          <LCtrl:LajitTextBox ID="txtTelephone_EmployeePhone" runat="server" MapXML="Telephone" MapBranchNode="EmployeePhone" Width="180px" TabIndex=14></LCtrl:LajitTextBox>
                                                                                          <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_EmployeePhone" MapXML="Telephone" MapBranchNode="EmployeePhone" runat="server" ControlToValidate="txtTelephone_EmployeePhone" ValidationGroup="LAJITEntryForm"
                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                   </tr>
                                                                                     <tr align="left" valign="top" id="trIsPrimary_EmployeePhone" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_EmployeePhone" runat="server" MapXML="IsPrimary" MapBranchNode="EmployeePhone" TabIndex=13/>
                                                                                            <asp:Label runat="server" ID="lblIsPrimary_EmployeePhone"  SkinID="LabelBranch" MapBranchNode="EmployeePhone"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>
                                                                                    
                                                                                     <tr id="Tr4" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblEmail_EmployeeEmail" MapBranchNode="EmployeeEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                     <tr align="left" valign="top" id="trEmailType_EmployeeEmail"  runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlEmailType_EmployeeEmail" runat="server" MapXML="EmailType" MapBranchNode="EmployeeEmail" Width="130px" TabIndex=15></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlEmailType_EmployeeEmail_SelectedIndexChanged" -->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_EmployeeEmail" MapXML="EmailType" MapBranchNode="EmployeeEmail" runat="server" ControlToValidate="txtEmail_EmployeeEmail" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtEmail_EmployeeEmail" runat="server" MapXML="Email" MapBranchNode="EmployeeEmail" Width="180px" TabIndex=17></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_EmployeeEmail" MapXML="Email" MapBranchNode="EmployeeEmail" runat="server" ControlToValidate="txtEmail_EmployeeEmail" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            <LCtrl:LAjitRegularExpressionValidator ID="regEmail_EmployeeEmail" runat="server" ControlToValidate="txtEmail_EmployeeEmail" MapXML="Email"
                                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>
                                                                                        </td>
                                                                                      </tr>
                                                                                     <tr align="left" valign="top" id="trIsPrimary_EmployeeEmail" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_EmployeeEmail" runat="server" MapXML="IsPrimary" MapBranchNode="EmployeeEmail" TabIndex=16/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_EmployeeEmail"  SkinID="LabelBranch" MapBranchNode="EmployeeEmail"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                     </tr>
                                                                                     <tr id="Tr5" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblWebsite_EmployeeWebsite" MapBranchNode="EmployeeWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                   </tr>
                                                                                     <tr align="left" valign="top" id="trWebAddressType_EmployeeWebsite" runat="server">
                                                                                        <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlWebAddressType_EmployeeWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="EmployeeWebsite" Width="130px" TabIndex=18></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlWebAddressType_EmployeeWebsite_SelectedIndexChanged" -->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_EmployeeWebsite" MapXML="WebAddressType" MapBranchNode="EmployeeWebsite" runat="server" ControlToValidate="txtWebsite_EmployeeWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px;">
                                                                                                <LCtrl:LajitTextBox ID="txtWebsite_EmployeeWebsite" runat="server" MapXML="Website" MapBranchNode="EmployeeWebsite" Width="180px" TabIndex=20></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_EmployeeWebsite" MapXML="Website" MapBranchNode="EmployeeWebsite" runat="server" ControlToValidate="txtWebsite_EmployeeWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                     <tr align="left" valign="top" id="trIsPrimary_EmployeeWebsite" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_EmployeeWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="EmployeeWebsite" TabIndex=19/>
                                                                                            <asp:Label runat="server" ID="lblIsPrimary_EmployeeWebsite" SkinID="LabelBranch" MapBranchNode="EmployeeWebsite"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr> 
                                                                                  <tr>
                                                                                    <td colspan="2">
                                                                                       <!-- Set of 3 checkboxes start-->
                                                                                       <table cellpadding="0" cellspacing="0" border="0">
                                                                                         <tr>
                                                                                            <td align="left" valign="top" NOWRAP>
                                                                                              <!--1st Check box start-->
                                                                                                 <table cellpadding="0" cellspacing="0" border="0">
                                                                                                     <tr align="left" valign="top" id="trDirector" runat="server">
                                                                                                            <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                                  <LCtrl:LAjitCheckBox ID="chkDirector" runat="server" MapXML="Director"  TabIndex="20"/>&nbsp;
                                                                                                                  <asp:Label runat="server" ID="lblDirector" SkinID="LabelBranch"></asp:Label>
                                                                                                            </td>
                                                                                                      </tr> 
                                                                                                 </table>
                                                                                              <!--1st Check box end-->
                                                                                           </td>
                                                                                            <td align="left" valign="top" NOWRAP>
                                                                                               <!--2nd Check box start-->
                                                                                                 <table cellpadding="0" cellspacing="0" border="0">
                                                                                                       <tr align="left" valign="top" id="trProducer" runat="server">
                                                                                                            <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                              <LCtrl:LAjitCheckBox ID="chkProducer" runat="server" MapXML="Producer"  TabIndex="21"/>&nbsp;
                                                                                                              <asp:Label runat="server" ID="lblProducer" SkinID="LabelBranch"></asp:Label>
                                                                                                            </td>
                                                                                                      </tr>    
                                                                                                 </table>
                                                                                              <!--2nd Check box end-->
                                                                                           </td>
                                                                                            <td align="left" valign="top" NOWRAP>
                                                                                             <!--3rd Check box start-->
                                                                                                 <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr align="left" valign="top" id="trDirPhoto" runat="server">
                                                                                                        <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                          <LCtrl:LAjitCheckBox ID="chkDirPhoto" runat="server" MapXML="DirPhoto"  TabIndex="22"/>&nbsp;
                                                                                                          <asp:Label runat="server" ID="lblDirPhoto" SkinID="LabelBranch"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr> 
                                                                                                 </table>
                                                                                              <!--3rd Check box end-->
                                                                                           </td>
                                                                                         </tr>
                                                                                       </table>
                                                                                       <!-- Set of 3 checkboxes end-->
                                                                                    </td>
                                                                                  </tr>
                                                                                  
                                                                                  <tr>
                                                                                    <td colspan="2">
                                                                                      <!-- Set of 3 Checkboxes start -->
                                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                                            <tr>
                                                                                                 <td align="left" valign="top" NOWRAP>
                                                                                                      <!--1st Check box start-->
                                                                                                         <table cellpadding="0" cellspacing="0" border="0">
                                                                                                          <tr align="left" valign="top" id="trEditor" runat="server">
                                                                                                                <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                                  <LCtrl:LAjitCheckBox ID="chkEditor" runat="server" MapXML="Editor"  TabIndex="23"/>&nbsp;
                                                                                                                  <asp:Label runat="server" ID="lblEditor" SkinID="LabelBranch"></asp:Label>
                                                                                                                </td>
                                                                                                          </tr> 
                                                                                                         </table>
                                                                                                      <!--1st Check box end-->
                                                                                                 </td>
                                                                                                 <td align="left" valign="top" NOWRAP>
                                                                                                       <!--2nd Check box start-->
                                                                                                         <table cellpadding="0" cellspacing="0" border="0">
                                                                                                             <tr align="left" valign="top" id="trSetDesigner" runat="server">
                                                                                                                    <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                                      <LCtrl:LAjitCheckBox ID="chkSetDesigner" runat="server" MapXML="SetDesigner"  TabIndex="24"/>&nbsp;
                                                                                                                       <asp:Label runat="server" ID="lblSetDesigner" SkinID="LabelBranch"></asp:Label>
                                                                                                                    </td>
                                                                                                              </tr> 
                                                                                                         </table>
                                                                                                      <!--2nd Check box end-->  
                                                                                                 </td>
                                                                                                 <td align="left" valign="top" NOWRAP>
                                                                                                       <!--3rd Check box start-->
                                                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                                                             <tr align="left" valign="top" id="trArtDirector" runat="server">
                                                                                                                <td align="left" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                                    <LCtrl:LAjitCheckBox ID="chkArtDirector" runat="server" MapXML="ArtDirector" TabIndex="24"/>&nbsp;
                                                                                                                   <asp:Label runat="server" ID="lblArtDirector" SkinID="LabelBranch"></asp:Label>
                                                                                                                </td>
                                                                                                              </tr> 
                                                                                                       </table>
                                                                                                      <!--3rd Check box end-->  
                                                                                                 </td>
                                                                                             </tr>
                                                                                        </table>
                                                                                      <!-- Set of 3 Checkboxes end -->
                                                                                    </td>
                                                                                  </tr>
                                                                                 
                                                                                 <tr><td colspan="2">&nbsp;</td></tr> 
                                                                                 <tr align="left" valign="top" id="trSelectSalesRegion" runat="server">
                                                                                    <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label runat="server" ID="lblSelectSalesRegion"  SkinID="Label"></asp:Label></td>
                                                                                    <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectSalesRegion" runat="server" MapXML="SelectSalesRegion" Width="184px" TabIndex=10></LCtrl:LAjitDropDownList>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectSalesRegion" MapXML="SelectSalesRegion" runat="server" ControlToValidate="ddlSelectSalesRegion" ValidationGroup="LAJITEntryForm"                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>   
                                                                                </tr>
                                                                                 
																		         <tr align="left" valign="top" id="trSalesComm" runat="server">
                                                                                   <td class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label runat="server" ID="lblSalesComm"  SkinID="Label" MapXML="SalesComm"></asp:Label></td>
                                                                                   <td valign="middle" style="width:196px; ">
                                                                                        <LCtrl:LajitTextBox ID="txtSalesComm" runat="server" MapXML="SalesComm" Width="180px" TabIndex=11></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSalesComm" MapXML="SalesComm" runat="server" ControlToValidate="txtSalesComm" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                   </td>
                                                                               </tr>
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
                                                                    </div>
                                                                    <!-- Panel Other Info Start -->
                                                                    <div id="divOther" style="display:none">
                                                                         <table width="100%"  border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                          <tr>
                                                                              <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                          </tr> 
                                                                          <tr>
                                                                             <td valign="top">
                                                                                 <!--Left Table Start -->
                                                                                 <table cellspacing="0" cellpadding="0" border="0">
                                                                                     <tr id="trTax" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;">
                                                                                            <asp:Label  runat="server" ID="lblTax" SkinID="LabelSubHead">Tax</asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                     <tr align="left" valign="top" id="trSSNTaxID" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblSSNTaxID" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtSSNTaxID" runat="server" MapXML="SSNTaxID" Width="180px" TabIndex=21></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSSNTaxID" MapXML="SSNTaxID" runat="server" ControlToValidate="txtSSNTaxID" ValidationGroup="LAJITEntryForm"
                                                                                              ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                      </tr>
                                                                                     <tr align="left" valign="top" id="trFederalTaxID" runat="server">
                                                                                         <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblFederalTaxID" SkinID="LabelBranch"></asp:Label></td>
                                                                                         <td valign="middle" style="width:196px; ">
                                                                                             <LCtrl:LajitTextBox ID="txtFederalTaxID" runat="server" MapXML="FederalTaxID" Width="180px" TabIndex=22></LCtrl:LajitTextBox>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqFederalTaxID" MapXML="FederalTaxID" runat="server" ControlToValidate="txtFederalTaxID" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                      </tr>
                                                                                     <tr align="left" valign="top" id="trIsCorp" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblIsCorp" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="top">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsCorp" runat="server" MapXML="IsCorp" TabIndex=23/></td>
                                                                                     </tr>  
                                                                                     <tr align="left" valign="top" id="trIs1099" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblIs1099"  SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="top">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIs1099" runat="server" MapXML="Is1099" TabIndex=24/></td>
                                                                                     </tr>
                                                                                     <tr align="left" valign="top" id="trIndContractor" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblIndContractor" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="top">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIndContractor" runat="server" MapXML="IndContractor" TabIndex=25/></td>
                                                                                     </tr> 
                                                                                     <tr align="left" valign="top" id="trW9OnFile" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblW9OnFile"  SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="top">
                                                                                            <LCtrl:LAjitCheckBox ID="chkW9OnFile" runat="server" MapXML="W9OnFile" TabIndex=26/></td>
                                                                                     </tr>
                                                                                   </table>
                                                                                 <!--Left Table End -->
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
                                                                    </div>
                                                                </td>
                                                            </tr>                                                                   
                                                            <!-- form entry fields end -->
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
                                                        <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label>
                                                    </td>
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
    
 <%--   <script type="text/javascript" src="../JavaScript/BranchDropDowns.js"></script>--%>
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
                          break;
        case "Other": 
                          //content
                          document.getElementById("divOther").style.display="block";
                          document.getElementById("divGeneral").style.display="none";
                          //Tabs
                          document.getElementById("divOtherTab").style.display="block"; 
                          document.getElementById("divGeneralTab").style.display="none";
                          break;
    }
}    
</script>
</asp:content>
