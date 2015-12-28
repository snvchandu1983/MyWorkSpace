<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UserInfo.aspx.cs" Inherits="LAjitDev.Common.UserInfo" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
<%--<script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>--%>
<script language="javascript" type="text/javascript">
function HideShowDiv(divname)
{
    switch (divname)
    {
        case "Account Information":
                          //content
                          document.getElementById("divAccountInformation").style.display="block"; 
                          document.getElementById("divAddress").style.display="none";
                          //Tabs  
                          document.getElementById("divAccountInformationTab").style.display="block";
                          document.getElementById("divAddressTab").style.display="none"; 
                          break;
        case "Address": 
                          //content
                          document.getElementById("divAddress").style.display="block";
                          document.getElementById("divAccountInformation").style.display="none";
                          //Tabs
                          document.getElementById("divAddressTab").style.display="block"; 
                          document.getElementById("divAccountInformationTab").style.display="none";
                          break;
    }
}    
</script>

    <asp:Panel ID="pnlContent" runat="server">
    <%-- <script type="text/javascript" src="../JavaScript/ChildGridView.js"></script>--%>
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
                                                <tr style="height:10px;">                                                                                                  
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
                                                        <div id="divAccountInformationTab" style="display:block;">
                                                            <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                                 <tr align="left" valign="bottom" >
                                                                    <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width:135px; background-color:#c5d5fc;">Account Information</td>
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>                                                         
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('Address');" class="blueboldlinks01" align="center" valign="middle" style="width:135px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                        Address
                                                                    </td>
                                                                    <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                             </table>
                                                        </div>
                                                        <div id="divAddressTab" style="display:none">
                                                            <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                                  <tr align="left" valign="bottom">
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('Account Information');" class="blueboldlinks01" align="center" valign="middle" style="width:135px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                        Account Information
                                                                    </td>
                                                                    <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                    <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width:135px; background-color:#c5d5fc; ">Address</td>
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
                                               <!-- Form entry fields start -->
                                                <tr>
                                                    <td align="left" valign="top">
                                                         <table width="100%" style="height:390px" border="0" cellspacing="0" cellpadding="0" class="formmiddle"> 
                                                            <tr style="border-style:none;">
                                                              <td colspan="2" valign="top">
                                                                <!-- Panel Account Information Start -->
                                                                <div id="divAccountInformation" style="display:block;">
                                                                   <table width="100%" border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                         <td valign="top">
                                                                          <!--Left Table Start -->
                                                                           <table cellspacing="0" cellpadding="0" border="0">
                                                                             <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                                <td style="width: 160px; height: 24px" class="formtext" valign="middle">
                                                                                 &nbsp;
                                                                                </td>
                                                                                <td valign="middle">
                                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />                                                                  
                                                                                </td>
                                                                             </tr>
                                                                             <tr id="trLogon" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 160px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblLogon" SkinID="LabelBranch"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle">
                                                                                    <LCtrl:LajitTextBox ID="txtLogon" runat="server" MapXML="Logon" Width="200px" ></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLogon" MapXML="Logon" runat="server" ControlToValidate="txtLogon" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                             <tr id="trFullName" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblFullName" SkinID="LabelBranch"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle">
                                                                                    <LCtrl:LajitTextBox ID="txtFullName" runat="server" MapXML="FullName" Width="200px" ></LCtrl:LajitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqFullName" MapXML="FullName" runat="server" ControlToValidate="txtFullName" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                             <tr id="trSelectLanguage" align="left" valign="middle" runat="server">  
                                                                                 <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblSelectLanguage" SkinID="LabelBranch"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle">
                                                                                     <LCtrl:LAjitDropDownList ID="ddlSelectLanguage" runat="server" MapXML="SelectLanguage" Width="206px"></LCtrl:LAjitDropDownList>
                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqSelectLanguage" runat="server" MapXML="SelectLanguage" ControlToValidate="ddlSelectLanguage" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                                </td>
                                                                            </tr>
                                                                             <tr id="trSelectRoleLevel" align="left" valign="middle" runat="server">   
                                                                                 <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblSelectRoleLevel" SkinID="LabelBranch"></asp:Label>
                                                                                 </td>
                                                                                <td valign="middle">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectRoleLevel" runat="server" MapXML="SelectRoleLevel" Width="206px"></LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRoleLevel" runat="server" MapXML="SelectRoleLevel" ControlToValidate="ddlSelectRoleLevel" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>
                                                                              
                                                                             <tr id="Tr1" align="left" valign="top" runat="server">
                                                                                <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;font:caption;">
                                                                                    <asp:Label runat="server" ID="lblUserRole" SkinID="LabelSubHead" MapBranchNode="UserRole"></asp:Label>
                                                                                </td>
                                                                             </tr>
                                                                                <tr id="trSelectRole_UserRole" align="left" valign="top" visible="true" runat="server">
                                                                                    <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label SkinID="LabelBranch" ID="lblSelectRole" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle" style="width:196px;">
                                                                                        <LCtrl:LAjitListBox ID="lstBxSelectRole" runat="server" SelectionMode="Multiple" MapXML="SelectRole" 
                                                                                            AutoPostBack="false" XMLType="ParentChild" Height="120" Width="180">
                                                                                        </LCtrl:LAjitListBox>
                                                                                          <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRole" runat="server" MapXML="SelectRole" ControlToValidate="lstBxSelectRole" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                                    </td>
                                                                                </tr>    
                                                                          </table>
                                                                         </td>
                                                                         
                                                                         <td valign="middle" style="width:10px; ">
                                                                            <!-- Spacer start -->
                                                                              <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                            <!-- Spacer end --> 
                                                                         </td>
                                                                       
                                                                         <td valign="top">
                                                                            <!--Right Table Start-->
                                                                             <table cellspacing="0" cellpadding="0" border="0">                                                                             
                                                                                 <tr id="trPassword" align="left" valign="middle" runat="server">  
                                                                                     <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblPassword" SkinID="LabelBranch"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle">
                                                                                        <LCtrl:LajitTextBox ID="txtPassword" runat="server" MapXML="Password" TextMode="Password"  Width="200px"></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqPassword" MapXML="Password" runat="server" ControlToValidate="txtPassword" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr id="trAbbrevName" align="left" valign="middle" runat="server">  
                                                                                     <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblAbbrevName" SkinID="LabelBranch"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle">
                                                                                        <LCtrl:LajitTextBox ID="txtAbbrevName" runat="server" MapXML="AbbrevName" Width="200px" ></LCtrl:LajitTextBox>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAbbrevName" MapXML="AbbrevName" runat="server" ControlToValidate="txtAbbrevName" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr id="trSelectTimeZone" align="left" valign="middle" runat="server">
                                                                                     <td class="formtext" style="height: 24px; width:160px" valign="top">
                                                                                        <asp:Label runat="server" ID="lblSelectTimeZone" SkinID="LabelBranch"></asp:Label>
                                                                                     </td>
                                                                                    <td valign="middle">
                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectTimeZone" runat="server" MapXML="SelectTimeZone" Width="206px"></LCtrl:LAjitDropDownList>
                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectTimeZone" runat="server" MapXML="SelectTimeZone" ControlToValidate="ddlSelectTimeZone" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                    </td>
                                                                                </tr>                                                                             
                                                                                 <tr align="left" valign="top" id="trActive" runat="server">
                                                                                    <td align="right" valign="middle" class="formtext" style="height:24px; width:125px;">
                                                                                        <LCtrl:LAjitCheckBox ID="chkActive" runat="server" MapXML="Active"/>
                                                                                        <asp:Label runat="server" ID="lblActive" SkinID="LabelBranch"></asp:Label>
                                                                                    </td>
                                                                                    <td valign="middle" style="width:196px; "></td>
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
                                                            <tr align="center">
                                                                <td colspan="4">
                                                                    <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="UserPref" />
                                                                </td>
                                                            </tr>
                                                                  </table>
                                                                </div>
                                                                <!-- Panel Address Info Start -->
                                                                <div id="divAddress" style="display:none">
                                                                     <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                           <tr>
                                                                                <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                  <!--Left Table Start -->
                                                                                     <table cellspacing="0" cellpadding="0" border="0"> 
                                                                                        <tr id="trUserAddress" align="left" valign="top" runat="server">
                                                                                            <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;font:caption;">
                                                                                                <asp:Label runat="server" ID="lblAddress1_UserAddress" SkinID="LabelSubHead" MapBranchNode="UserAddress"></asp:Label>
                                                                                            </td>
                                                                                        </tr>                                                                                       
                                                                                            <tr align="left" valign="top" id="trAddressType_UserAddress" runat="server">
                                                                                                <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                                     <LCtrl:LAjitDropDownList ID="ddlAddressType_UserAddress" runat="server" MapXML="AddressType" MapBranchNode="UserAddress" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlAddressType_UserAddress_SelectedIndexChanged"></LCtrl:LAjitDropDownList>
                                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_UserAddress" MapXML="AddressType" MapBranchNode="UserAddress" runat="server" ControlToValidate="ddlAddressType_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtAddress1_UserAddress" runat="server" MapXML="Address1" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_UserAddress" MapXML="Address1" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtAddress1_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trAddress2_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_UserAddress" runat="server" MapXML="IsPrimary" MapBranchNode="UserAddress" />
                                                                                                    <asp:Label runat="server" ID="lblIsPrimary_UserAddress" SkinID="LabelBranch" MapBranchNode="UserAddress"></asp:Label>
                                                                                               </td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtAddress2_UserAddress" runat="server" MapXML="Address2" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_UserAddress" MapXML="Address2" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtAddress2_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                               </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trAddress3_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                               </td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtAddress3_UserAddress" runat="server" MapXML="Address3" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_UserAddress" MapXML="Address3" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtAddress3_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trAddress4_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                               </td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtAddress4_UserAddress" runat="server" MapXML="Address4" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_UserAddress" MapXML="Address4" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtAddress4_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trCity_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <asp:Label runat="server" ID="lblCity_UserAddress" SkinID="LabelBranch" MapBranchNode="UserAddress"></asp:Label></td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtCity_UserAddress" runat="server" MapXML="City" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCity_UserAddress" MapXML="City" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtCity_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                               </td>
                                                                                           </tr>
                                                                                            <tr align="left" valign="top" id="trArea_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <asp:Label runat="server" ID="lblArea_UserAddress" MapBranchNode="UserAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtArea_UserAddress" runat="server" MapXML="Area" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqArea_UserAddress" MapXML="Area" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtArea_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trSelectCountry_UserAddress" runat="server">
                                                                                                <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <asp:Label runat="server" ID="lblSelectCountry_UserAddress" MapBranchNode="UserAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                     <LCtrl:LAjitDropDownList ID="ddlSelectCountry_UserAddress" runat="server" MapXML="SelectCountry" MapBranchNode="UserAddress" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_UserAddress" MapXML="SelectCountry" MapBranchNode="UserAddress" runat="server" ControlToValidate="ddlSelectCountry_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td> 
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trSelectRegion_UserAddress" runat="server">
                                                                                                <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <asp:Label runat="server" ID="lblSelectRegion_UserAddress" SkinID="LabelBranch" MapBranchNode="UserAddress"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectRegion_UserAddress" runat="server" MapXML="SelectRegion" MapBranchNode="UserAddress" Width="184px"></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_UserAddress" MapXML="SelectRegion" MapBranchNode="UserAddress" runat="server" ControlToValidate="ddlSelectRegion_UserAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trPostalCode_UserAddress" runat="server">
                                                                                               <td class="formtext" style="height:24px; width:141px;">
                                                                                                    <asp:Label runat="server" ID="lblPostalCode_UserAddress" SkinID="LabelBranch" MapBranchNode="UserAddress"></asp:Label></td>
                                                                                               <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtPostalCode_UserAddress" runat="server" MapXML="PostalCode" MapBranchNode="UserAddress" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_UserAddress" MapXML="PostalCode" MapBranchNode="UserAddress" runat="server" ControlToValidate="txtPostalCode_UserAddress" ValidationGroup="LAJITEntryForm"
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
                                                                                         <tr id="trUserPhone" align="left" valign="top" runat="server">
                                                                                            <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;font:caption;">
                                                                                                <asp:Label runat="server" ID="lblTelephone_UserPhone" MapBranchNode="UserPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                            </td>
                                                                                         </tr>
                                                                                             <tr align="left" valign="top" id="trPhoneType_UserPhone" runat="server">
                                                                                                <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                                     <LCtrl:LAjitDropDownList ID="ddlPhoneType_UserPhone" runat="server" MapXML="PhoneType" MapBranchNode="UserPhone" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlPhoneType_UserPhone_SelectedIndexChanged"></LCtrl:LAjitDropDownList>
                                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_UserPhone" MapXML="PhoneType" MapBranchNode="UserPhone" runat="server" ControlToValidate="ddlPhoneType_UserPhone" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td> 
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                  <LCtrl:LajitTextBox ID="txtTelephone_UserPhone" runat="server" MapXML="Telephone" MapBranchNode="UserPhone" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_UserPhone" MapXML="Telephone" MapBranchNode="UserPhone" runat="server" ControlToValidate="txtTelephone_UserPhone" ValidationGroup="LAJITEntryForm"
                                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>    
                                                                                                </td>
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trIsPrimary_UserPhone" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_UserPhone" runat="server" MapXML="IsPrimary" MapBranchNode="UserPhone" />
                                                                                                    <asp:Label runat="server" ID="lblIsPrimary_UserPhone" SkinID="LabelBranch" MapBranchNode="UserPhone"></asp:Label>
                                                                                                </td>
                                                                                                <td valign="middle" style="width:196px; "></td>
                                                                                             </tr>

                                                                                         <tr id="trUserEmail" align="left" valign="top" runat="server">
                                                                                           <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;font:caption;">
                                                                                               <asp:Label runat="server" ID="lblEmail_UserEmail" MapBranchNode="UserEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                           </td>
                                                                                         </tr>
                                                                                             <tr align="left" valign="top" id="trContactInfo_UserEmail" runat="server">
                                                                                                 <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                    <asp:Label runat="server" ID="lblContactInfo_UserEmail" MapBranchNode="UserEmail" SkinID="LabelBranch"></asp:Label></td>
                                                                                                 <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtContactInfo_UserEmail" runat="server" MapXML="ContactInfo" MapBranchNode="UserEmail" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_UserEmail" MapXML="ContactInfo" MapBranchNode="UserEmail" runat="server" ControlToValidate="txtContactInfo_UserEmail" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 </td>
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trEmailType_UserEmail"  runat="server">
                                                                                                 <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                                      <LCtrl:LAjitDropDownList ID="ddlEmailType_UserEmail" runat="server" MapXML="EmailType" MapBranchNode="UserEmail" Width="130px" AutoPostBack=true OnSelectedIndexChanged="ddlEmailType_UserEmail_SelectedIndexChanged"></LCtrl:LAjitDropDownList>
                                                                                                      <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_UserEmail" MapXML="EmailType" MapBranchNode="UserEmail" runat="server" ControlToValidate="ddlEmailType_UserEmail" ValidationGroup="LAJITEntryForm"
                                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 </td> 
                                                                                                 <td valign="middle" style="width:196px; ">
                                                                                                     <LCtrl:LajitTextBox ID="txtEmail_UserEmail" runat="server" MapXML="Email" MapBranchNode="UserEmail" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_UserEmail" MapXML="Email" MapBranchNode="UserEmail" runat="server" ControlToValidate="txtEmail_UserEmail" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                     <LCtrl:LAjitRegularExpressionValidator ID="regEmail_UserEmail" runat="server" ControlToValidate="txtEmail_UserEmail" MapXML="Email"
                                                                                                         ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                         Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>
                                                                                                 </td>
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trIsPrimary_UserEmail" runat="server">
                                                                                                 <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                     <LCtrl:LAjitCheckBox ID="chkIsPrimary_UserEmail" runat="server" MapXML="IsPrimary" MapBranchNode="UserEmail" />
                                                                                                     <asp:Label  runat="server" ID="lblIsPrimary_UserEmail" SkinID="LabelBranch" MapBranchNode="UserEmail"></asp:Label>
                                                                                                 </td>
                                                                                                 <td valign="middle" style="width:196px;"></td>
                                                                                             </tr>

                                                                                         <tr id="trUserWebsite" align="left" valign="top" runat="server">
                                                                                             <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px;font:caption;">
                                                                                                <asp:Label runat="server" ID="lblWebsite_UserWebsite" MapBranchNode="UserWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                             </td>
                                                                                         </tr>
                                                                                            <tr align="left" valign="top" id="trWebAddressType_UserWebsite" runat="server">
                                                                                                 <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                                      <LCtrl:LAjitDropDownList ID="ddlWebAddressType_UserWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="UserWebsite" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlWebAddressType_UserWebsite_SelectedIndexChanged"></LCtrl:LAjitDropDownList>
                                                                                                      <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_UserWebsite" MapXML="WebAddressType" MapBranchNode="UserWebsite" runat="server" ControlToValidate="ddlWebAddressType_UserWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 </td> 
                                                                                                 <td valign="middle" style="width:196px;">
                                                                                                       <LCtrl:LajitTextBox ID="txtWebsite_UserWebsite" runat="server" MapXML="Website" MapBranchNode="UserWebsite" Width="180px" ></LCtrl:LajitTextBox>
                                                                                                       <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_UserWebsite" MapXML="Website" MapBranchNode="UserWebsite" runat="server" ControlToValidate="txtWebsite_UserWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 </td>
                                                                                            </tr>
                                                                                            <tr align="left" valign="top" id="trIsPrimary_UserWebsite" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                    <LCtrl:LAjitCheckBox ID="chkIsPrimary_UserWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="UserWebsite" />
                                                                                                    <asp:Label runat="server" ID="lblIsPrimary_UserWebsite" SkinID="LabelBranch" MapBranchNode="UserWebsite"></asp:Label>
                                                                                                </td>
                                                                                                <td valign="middle" style="width:196px; "></td>
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
                                                    </td>
                                                </tr>
                                          </table>
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="1" width="800px" align="center">
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
