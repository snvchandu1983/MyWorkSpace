<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Vendor.aspx.cs" Inherits="LAjitDev.Payables.Vendor" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
 <script type="text/javascript">
    function JQueryPageEvents(){
	    AttachAutoComplete("ctl00_cphPageContents_txtLastName",'AutoFillVendor','<%=this.Page.ToString() %>');
    }
    </script>
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <contenttemplate>
              <script type="text/javascript" >
                   Sys.Application.add_load(JQueryPageEvents);
                 </script> 
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
                          <table class="tblMainCont" cellpadding="0" cellspacing="0">
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
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                         <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
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
                                                                    <td onclick="javascript:HideShowDiv('Contact');" class="blueboldlinks01" align="center" valign="middle" style="width:90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                    Contact
                                                                    </td>
                                                                    <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
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
                                                         <div id="divContactTab" style="display:none">
                                                           <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0" cellspacing="0">
                                                              <tr align="left" valign="bottom">
                                                                <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                <td onclick="javascript:HideShowDiv('General');" class="blueboldlinks01" align="center" valign="middle" style="width:85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                General
                                                                </td>
                                                                <td align="center" valign="middle" style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
                                                                <td align="center" valign="middle" style="width:3px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5" height="21"></td>
                                                                <td align="center" valign="middle" class="bluebold" style="width:90px; background-color:#c5d5fc; ">Contact</td>
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
                                                                    <td align="center" valign="middle" style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5" height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('Contact');" class="blueboldlinks01" align="center" valign="middle" style="width:90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                        Contact
                                                                    </td>
                                                                    <td style="width:6px;"><img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6" height="21"></td>
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
                                                <!-- tab headings end -->
                                                                                              
                                                <tr>
                                                    <td align="left" valign="top">
                                                          <table  width="100%" style="height:390px"   border="0" cellspacing="0" cellpadding="0" class="formmiddle"> 
                                                            <tr style="border-style:none;">
                                                                <td colspan="2" valign="top">                                                                
                                                                    <!-- Panel General  Start -->
                                                                    <div id="divGeneral" style="display:block;">
                                                                      <table width="100%"  border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                        
                                                                        <tr>
                                                                            <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                        </tr>
                                                                         
                                                                        <tr>
                                                                            <td valign="top">
                                                                              <!--Left Table Start -->
                                                                                 <table cellspacing="0" cellpadding="0" border="0">                                                                                  
                                                                                  <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                                        <td style="width: 160px; height: 24px" class="formtext">
                                                                                           &nbsp;
                                                                                          </td>
                                                                                        <td valign="middle">
                                                                                           <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" />
                                                                                        </td>
                                                                                   </tr> 
                                                                                    <tr id="Tr1" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblName" SkinID="LabelSubHead">Name</asp:Label>
                                                                                        </td> 
                                                                                    </tr>
                                                                                   <tr align="left" valign="top" id="trLastName" runat="server">
                                                                                       <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                        <asp:Label  runat="server" ID="lblLastName" SkinID="LabelBranch" ></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px;">
                                                                                            <LCtrl:LajitTextBox ID="txtLastName" runat="server" MapXML="LastName" Width="180px"  TabIndex=1 ></LCtrl:LajitTextBox>                                                                                          
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
                                                                                    <tr align="left" valign="top" id="trPayTo" runat="server">
                                                                                       <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblPayTo"  SkinID="LabelBranch"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtPayTo" runat="server" MapXML="PayTo" Width="180px" TabIndex=3 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqPayTo" MapXML="PayTo" runat="server" ControlToValidate="txtPayTo" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    
                                                                                   <tr align="left" valign="top" id="trVendorType" runat="server">
                                                                                        <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblVendorType"  SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlVendorType" runat="server" MapXML="VendorType" Width="184px" TabIndex=4></LCtrl:LAjitDropDownList>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqVendorType" MapXML="VendorType" runat="server" ControlToValidate="ddlVendorType" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                    </tr>                                                                                     
                                                                                    <tr align="left" valign="top" id="trNumberID" runat="server">
                                                                                        <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblNumberID"  SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtNumberID" runat="server" MapXML="NumberID" Width="180px" TabIndex=5 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqNumberID" MapXML="NumberID" runat="server" ControlToValidate="txtNumberID" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr> 
                                                                                    <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">                                                                                            
                                                                                            <asp:Label runat="server" ID="lblAddress1_VendorAddress" SkinID="LabelSubHead" MapBranchNode="VendorAddress"></asp:Label>                                                                                            
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddressType_VendorAddress" runat="server">
                                                                                        <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlAddressType_VendorAddress" runat="server" MapXML="AddressType" MapBranchNode="VendorAddress" Width="130px" TabIndex="6" ></LCtrl:LAjitDropDownList>    <!--AutoPostBack=true OnSelectedIndexChanged="ddlAddressType_VendorAddress_SelectedIndexChanged"-->
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_VendorAddress" MapXML="AddressType" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtAddress1_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress1_VendorAddress" runat="server" MapXML="Address1" MapBranchNode="VendorAddress" Width="180px" TabIndex=8 ></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_VendorAddress" MapXML="Address1" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtAddress1_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                                     
                                                                                        </td>
                                                                                    </tr>                                                                                
                                                                                    <tr align="left" valign="top" id="trAddress2_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorAddress" runat="server" MapXML="IsPrimary" MapBranchNode="VendorAddress" TabIndex=7 />
                                                                                            <asp:Label runat="server" ID="lblIsPrimary_VendorAddress"  SkinID="LabelBranch" MapBranchNode="VendorAddress"></asp:Label>                                                                                   
                                                                                       </td> 
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress2_VendorAddress" runat="server" MapXML="Address2" MapBranchNode="VendorAddress" Width="180px" TabIndex=9></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_VendorAddress" MapXML="Address2" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtAddress2_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddress3_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            </td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress3_VendorAddress" runat="server" MapXML="Address3" MapBranchNode="VendorAddress" Width="180px" TabIndex=68></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_VendorAddress" MapXML="Address3" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtAddress3_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddress4_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                           </td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress4_VendorAddress" runat="server" MapXML="Address4" MapBranchNode="VendorAddress" Width="180px" TabIndex=69></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_VendorAddress" MapXML="Address4" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtAddress4_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>                                                                               
                                                                                    <tr align="left" valign="top" id="trCity_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblCity_VendorAddress"  SkinID="LabelBranch" MapBranchNode="VendorAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtCity_VendorAddress" runat="server" MapXML="City" MapBranchNode="VendorAddress" Width="180px" TabIndex=10 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCity_VendorAddress" MapXML="City" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtCity_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                  
                                                                                    <tr align="left" valign="top" id="trSelectRegion_VendorAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblSelectRegion_VendorAddress"  SkinID="LabelBranch" MapBranchNode="VendorAddress"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlSelectRegion_VendorAddress" runat="server" MapXML="SelectRegion" MapBranchNode="VendorAddress" Width="184px" TabIndex=13></LCtrl:LAjitDropDownList>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_VendorAddress" MapXML="SelectRegion" MapBranchNode="VendorAddress" runat="server" ControlToValidate="ddlSelectRegion_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>   
                                                                                    </tr>                                                                                
                                                                                   <tr align="left" valign="top" id="trPostalCode_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblPostalCode_VendorAddress"  SkinID="LabelBranch" MapBranchNode="VendorAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtPostalCode_VendorAddress" runat="server" MapXML="PostalCode" MapBranchNode="VendorAddress" Width="180px" TabIndex=14></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_VendorAddress" MapXML="PostalCode" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtPostalCode_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                       </td>
                                                                                   </tr> 
                                                                                    <tr align="left" valign="top" id="trArea_VendorAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblArea_VendorAddress"  MapBranchNode="VendorAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtArea_VendorAddress" runat="server" MapXML="Area" MapBranchNode="VendorAddress" Width="180px" TabIndex=11 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqArea_VendorAddress" MapXML="Area" MapBranchNode="VendorAddress" runat="server" ControlToValidate="txtArea_VendorAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trSelectCountry_VendorAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                             <asp:Label runat="server" ID="lblSelectCountry_VendorAddress" MapBranchNode="VendorAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlSelectCountry_VendorAddress" runat="server" MapXML="SelectCountry" MapBranchNode="VendorAddress" Width="184px" TabIndex=12></LCtrl:LAjitDropDownList>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_VendorAddress" MapXML="SelectCountry" MapBranchNode="VendorAddress" runat="server" ControlToValidate="ddlSelectCountry_VendorAddress" ValidationGroup="LAJITEntryForm"
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
                                                                                     <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblTelephone_VendorPhone"  MapBranchNode="VendorPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>                                                                                                                                            
                                                                                     </tr>
                                                                                   <tr align="left" valign="top" id="trPhoneType_VendorPhone" runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlPhoneType_VendorPhone" runat="server" MapXML="PhoneType" MapBranchNode="VendorPhone" Width="130px" TabIndex="15"></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlPhoneType_VendorPhone_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_VendorPhone" MapXML="PhoneType" MapBranchNode="VendorPhone" runat="server" ControlToValidate="txtTelephone_VendorPhone" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                          <LCtrl:LajitTextBox ID="txtTelephone_VendorPhone" runat="server" MapXML="Telephone" MapBranchNode="VendorPhone" Width="180px" TabIndex=17></LCtrl:LajitTextBox>                                                                                     
                                                                                          <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_VendorPhone" MapXML="Telephone" MapBranchNode="VendorPhone" runat="server" ControlToValidate="txtTelephone_VendorPhone" ValidationGroup="LAJITEntryForm"
                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trIsPrimary_VendorPhone" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorPhone" runat="server" MapXML="IsPrimary" MapBranchNode="VendorPhone" TabIndex=16/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorPhone"  SkinID="LabelBranch" MapBranchNode="VendorPhone"></asp:Label>                                                                                                                                                                             
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>                                                                               
                                                                                
                                                                                   <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblEmail_VendorEmail" MapBranchNode="VendorEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                   </tr>                                                                                 
                                                                                   <tr align="left" valign="top" id="trEmailType_VendorEmail"  runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlEmailType_VendorEmail" runat="server" MapXML="EmailType" MapBranchNode="VendorEmail" Width="130px" TabIndex=18></LCtrl:LAjitDropDownList> <!-- AutoPostBack=true OnSelectedIndexChanged="ddlEmailType_VendorEmail_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_VendorEmail" MapXML="EmailType" MapBranchNode="VendorEmail" runat="server" ControlToValidate="txtEmail_VendorEmail" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtEmail_VendorEmail" runat="server" MapXML="Email" MapBranchNode="VendorEmail" Width="180px" TabIndex=20></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_VendorEmail" MapXML="Email" MapBranchNode="VendorEmail" runat="server" ControlToValidate="txtEmail_VendorEmail" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            <LCtrl:LAjitRegularExpressionValidator ID="regEmail_VendorEmail" runat="server" ControlToValidate="txtEmail_VendorEmail" MapXML="Email"
                                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>                                                                                            
                                                                                        </td>
                                                                                   </tr>
                                                                                    <tr align="left" valign="top" id="trIsPrimary_VendorEmail" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorEmail" runat="server" MapXML="IsPrimary" MapBranchNode="VendorEmail" TabIndex=19/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorEmail"  SkinID="LabelBranch" MapBranchNode="VendorEmail"></asp:Label>                                                                                        
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>                                                                                                                                                         
                                                                                  <tr align="left" valign="top" id="trContactInfo_VendorEmail" runat="server">
                                                                                         <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <asp:Label  runat="server" ID="lblContactInfo_VendorEmail" MapBranchNode="VendorEmail"  SkinID="LabelBranch"></asp:Label></td>
                                                                                         <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtContactInfo_VendorEmail" runat="server" MapXML="ContactInfo" MapBranchNode="VendorEmail" Width="180px" TabIndex=21></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqContact_VendorEmail" MapXML="ContactInfo" MapBranchNode="VendorEmail" runat="server" ControlToValidate="txtContactInfo_VendorEmail" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                         </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trEmail_VendorEmail" runat="server">
                                                                                       <td align="right" valign="top"  class="formtext" style="height:20px; width:125px;"></td>
                                                                                        <td valign="middle" style="width:196px; "></td> 
                                                                                   </tr>

                                                                                   <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblWebsite_VendorWebsite" MapBranchNode="VendorWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                   </tr>                                                                                 
                                                                                   <tr align="left" valign="top" id="trWebAddressType_VendorWebsite" runat="server">
                                                                                        <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlWebAddressType_VendorWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="VendorWebsite" Width="130px" TabIndex=22 ></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlWebAddressType_VendorWebsite_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_VendorWebsite" MapXML="WebAddressType" MapBranchNode="VendorWebsite" runat="server" ControlToValidate="txtWebsite_VendorWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px;">
                                                                                                <LCtrl:LajitTextBox ID="txtWebsite_VendorWebsite" runat="server" MapXML="Website" MapBranchNode="VendorWebsite" Width="180px" TabIndex=24 ></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_VendorWebsite" MapXML="Website" MapBranchNode="VendorWebsite" runat="server" ControlToValidate="txtWebsite_VendorWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trIsPrimary_VendorWebsite" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="VendorWebsite" TabIndex=23 />
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorWebsite"  SkinID="LabelBranch" MapBranchNode="VendorWebsite"></asp:Label>                                                                                                                                                                                
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>    

                                                                                   <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblVendorGroupSubHead_VendorGroup" MapBranchNode="VendorGroup" SkinID="LabelSubHead">VendorGroup</asp:Label>
                                                                                        </td>
                                                                                   </tr>                                                                                     
                                                                                    <tr id="trVendorGroup_VendorGroupItem" align="left" valign="top" visible="true" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                         <asp:Label SkinID="LabelBranch" ID="lblVendorGroup" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px;">
                                                                                            <LCtrl:LAjitListBox ID="lstBxVendorGroup" runat="server" SelectionMode="Multiple"
                                                                                                MapXML="VendorGroup" XMLType="ParentChild" Height="120" Width="180" TabIndex="25">
                                                                                            </LCtrl:LAjitListBox>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqVendorGroup_VendorGroupItem" runat="server" MapXML="VendorGroup" ControlToValidate="lstBxVendorGroup" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required."  ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
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
                                                                    <!-- Panel General  End -->
                                                                     <!-- Panel Contact  Start -->
                                                                    <div id="divContact" style="display:none;">
                                                                      <table width="100%"  border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                        
                                                                        <tr>
                                                                            <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                        </tr>
                                                                         
                                                                        <tr>
                                                                            <td valign="top">
                                                                              <!--Left Table Start -->
                                                                                 <table cellspacing="0" cellpadding="0" border="0"> 
                                                                                    <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblAddress1_VendorContactAddress"  SkinID="LabelSubHead" MapBranchNode="VendorContactAddress"></asp:Label>
                                                                                        </td> 
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddressType_VendorContactAddress" runat="server">
                                                                                        <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlAddressType_VendorContactAddress" runat="server" MapXML="AddressType" MapBranchNode="VendorContactAddress" Width="130px" TabIndex=26></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlAddressType_VendorContactAddress_SelectedIndexChanged"-->
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_VendorContactAddress" MapXML="AddressType" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtAddress1_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>                                                                                     
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress1_VendorContactAddress" runat="server" MapXML="Address1" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=28></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_VendorContactAddress" MapXML="Address1" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtAddress1_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                                     
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddress2_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorContactAddress" runat="server" MapXML="IsPrimary" MapBranchNode="VendorContactAddress" TabIndex=27 />
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorContactAddress"  SkinID="LabelBranch" MapBranchNode="VendorContactAddress"></asp:Label>                                                                                   
                                                                                       </td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress2_VendorContactAddress" runat="server" MapXML="Address2" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=29 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_VendorContactAddress" MapXML="Address2" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtAddress2_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                       </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddress3_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                       </td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress3_VendorContactAddress" runat="server" MapXML="Address3" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=70></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_VendorContactAddress" MapXML="Address3" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtAddress3_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trAddress4_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                       </td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress4_VendorContactAddress" runat="server" MapXML="Address4" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=71 ></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_VendorContactAddress" MapXML="Address4" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtAddress4_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>                                                                               
                                                                                    <tr align="left" valign="top" id="trCity_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblCity_VendorContactAddress"  SkinID="LabelBranch" MapBranchNode="VendorContactAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtCity_VendorContactAddress" runat="server" MapXML="City" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=30></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCity_VendorContactAddress" MapXML="City" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtCity_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                    <tr align="left" valign="top" id="trArea_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblArea_VendorContactAddress"  MapBranchNode="VendorContactAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtArea_VendorContactAddress" runat="server" MapXML="Area" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=31></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqArea_VendorContactAddress" MapXML="Area" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtArea_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trSelectCountry_VendorContactAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblSelectCountry_VendorContactAddress" MapBranchNode="VendorContactAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlSelectCountry_VendorContactAddress" runat="server" MapXML="SelectCountry" MapBranchNode="VendorContactAddress" Width="184px" TabIndex=32></LCtrl:LAjitDropDownList>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_VendorContactAddress" MapXML="SelectCountry" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="ddlSelectCountry_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trSelectRegion_VendorContactAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblSelectRegion_VendorContactAddress"  SkinID="LabelBranch" MapBranchNode="VendorContactAddress"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlSelectRegion_VendorContactAddress" runat="server" MapXML="SelectRegion" MapBranchNode="VendorContactAddress" Width="184px" TabIndex=33></LCtrl:LAjitDropDownList>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_VendorContactAddress" MapXML="SelectRegion" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="ddlSelectRegion_VendorContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>   
                                                                                    </tr>                                                                                
                                                                                    <tr align="left" valign="top" id="trPostalCode_VendorContactAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblPostalCode_VendorContactAddress"  SkinID="LabelBranch" MapBranchNode="VendorContactAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtPostalCode_VendorContactAddress" runat="server" MapXML="PostalCode" MapBranchNode="VendorContactAddress" Width="180px" TabIndex=34></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_VendorContactAddress" MapXML="PostalCode" MapBranchNode="VendorContactAddress" runat="server" ControlToValidate="txtPostalCode_VendorContactAddress" ValidationGroup="LAJITEntryForm"
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
                                                                                            <asp:Label  runat="server" ID="lblTelephone_VendorContactPhone"  MapBranchNode="VendorContactPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>                                                                                                                                            
                                                                                     </tr>
                                                                                   <tr align="left" valign="top" id="trPhoneType_VendorContactPhone" runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlPhoneType_VendorContactPhone" runat="server" MapXML="PhoneType" MapBranchNode="VendorContactPhone" Width="130px" TabIndex=35></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlPhoneType_VendorContactPhone_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_VendorContactPhone" MapXML="PhoneType" MapBranchNode="VendorContactPhone" runat="server" ControlToValidate="txtTelephone_VendorContactPhone" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                          <LCtrl:LajitTextBox ID="txtTelephone_VendorContactPhone" runat="server" MapXML="Telephone" MapBranchNode="VendorContactPhone" Width="180px" TabIndex=37></LCtrl:LajitTextBox>                                                                                     
                                                                                          <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_VendorContactPhone" MapXML="Telephone" MapBranchNode="VendorContactPhone" runat="server" ControlToValidate="txtTelephone_VendorContactPhone" ValidationGroup="LAJITEntryForm"
                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>    
                                                                                        </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trIsPrimary_VendorContactPhone" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorContactPhone" runat="server" MapXML="IsPrimary" MapBranchNode="VendorContactPhone" TabIndex=36/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorContactPhone"  SkinID="LabelBranch" MapBranchNode="VendorContactPhone"></asp:Label>                                                                                                                                                                             
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>                                                                               
                                                                                   <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblEmail_VendorContactEmail" MapBranchNode="VendorContactEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                   </tr>                                                                                 
                                                                                   <tr align="left" valign="top" id="trEmailType_VendorContactEmail"  runat="server">
                                                                                        <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlEmailType_VendorContactEmail" runat="server" MapXML="EmailType" MapBranchNode="VendorContactEmail" Width="130px" TabIndex=38></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlEmailType_VendorContactEmail_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_VendorContactEmail" MapXML="EmailType" MapBranchNode="VendorContactEmail" runat="server" ControlToValidate="txtEmail_VendorContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtEmail_VendorContactEmail" runat="server" MapXML="Email" MapBranchNode="VendorContactEmail" Width="180px" TabIndex=40></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_VendorContactEmail" MapXML="Email" MapBranchNode="VendorContactEmail" runat="server" ControlToValidate="txtEmail_VendorContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            <LCtrl:LAjitRegularExpressionValidator ID="regEmail_VendorContactEmail" runat="server" ControlToValidate="txtEmail_VendorContactEmail" MapXML="Email"
                                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>                                                                                                                                                                                        
                                                                                        </td>
                                                                                   </tr>
                                                                                    <tr align="left" valign="top" id="trIsPrimary_VendorContactEmail" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorContactEmail" runat="server" MapXML="IsPrimary" MapBranchNode="VendorContactEmail" TabIndex=39/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorContactEmail"  SkinID="LabelBranch" MapBranchNode="VendorContactEmail"></asp:Label>                                                                                        
                                                                                        </td>
                                                                                        <td valign="middle" style="width:196px; "></td>
                                                                                   </tr>                                                                                                                                                         
                                                                                   <tr align="left" valign="top" id="trContactInfo_VendorContactEmail" runat="server">
                                                                                         <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <asp:Label  runat="server" ID="lblContactInfo_VendorContactEmail" MapBranchNode="VendorContactEmail"  SkinID="LabelBranch"></asp:Label></td>
                                                                                         <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtContactInfo_VendorContactEmail" runat="server" MapXML="ContactInfo" MapBranchNode="VendorContactEmail" Width="180px" TabIndex=41></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_VendorContactEmail" MapXML="ContactInfo" MapBranchNode="VendorContactEmail" runat="server" ControlToValidate="txtContactInfo_VendorContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                         </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trEmail_VendorContactEmail" runat="server">
                                                                                       <td align="right" valign="top"  class="formtext" style="height:20px; width:125px;"></td>
                                                                                        <td valign="middle" style="width:196px; "></td> 
                                                                                   </tr>

                                                                                   <tr align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblWebsite_VendorContactWebsite" MapBranchNode="VendorContactWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                   </tr>                                                                                 
                                                                                   <tr align="left" valign="top" id="trWebAddressType_VendorContactWebsite" runat="server">
                                                                                        <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlWebAddressType_VendorContactWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="VendorContactWebsite" Width="130px" TabIndex=42></LCtrl:LAjitDropDownList> <!--AutoPostBack=true OnSelectedIndexChanged="ddlWebAddressType_VendorContactWebsite_SelectedIndexChanged"-->
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_VendorContactWebsite" MapXML="WebAddressType" MapBranchNode="VendorContactWebsite" runat="server" ControlToValidate="txtWebsite_VendorContactWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td> 
                                                                                        <td valign="middle" style="width:196px;">
                                                                                                <LCtrl:LajitTextBox ID="txtWebsite_VendorContactWebsite" runat="server" MapXML="Website" MapBranchNode="VendorContactWebsite" Width="180px" TabIndex=44></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_VendorContactWebsite" MapXML="Website" MapBranchNode="VendorContactWebsite" runat="server" ControlToValidate="txtWebsite_VendorContactWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                   <tr align="left" valign="top" id="trIsPrimary_VendorContactWebsite" runat="server">
                                                                                        <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_VendorContactWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="VendorContactWebsite" TabIndex=43/>
                                                                                            <asp:Label  runat="server" ID="lblIsPrimary_VendorContactWebsite"  SkinID="LabelBranch" MapBranchNode="VendorContactWebsite"></asp:Label>                                                                                                                                                                                
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
                                                                     <!--Panel Contact  End-->                                                                
                                                                    <!-- Panel Other  Start -->
                                                                     <div id="divOther" style="display:none">
                                                                         <table width="100%"  border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                          <tr>
                                                                              <td style="width:21px; height:15px;" colspan="4"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                          </tr> 
                                                                          <tr>
                                                                             <td valign="top">
                                                                                 <!--Left Table Start -->
                                                                                  <table cellspacing="0" cellpadding="0" border="0">
                                                                                   <tr id="trId" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">                                                                                            
                                                                                            <asp:Label  runat="server" ID="lblId" SkinID="LabelSubHead">Other Information</asp:Label>
                                                                                        </td>
                                                                                   </tr> 
                                                                                                                                                                                                                                                                                                                                              
                                                                                      <tr align="left" valign="top" id="trVendAcct" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblVendAcct"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                   <LCtrl:LajitTextBox ID="txtVendAcct" runat="server" MapXML="VendAcct" Width="180px" TabIndex=45></LCtrl:LajitTextBox>                                                                                     
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqVendAcct" MapXML="VendAcct" runat="server" ControlToValidate="txtVendAcct" ValidationGroup="LAJITEntryForm"
                                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                      </tr>
                                                                                      <tr align="left" valign="top" id="trStudioVendorID" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label  runat="server" ID="lblStudioVendorID"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle"> <LCtrl:LajitTextBox ID="txtStudioVendorID" runat="server" MapXML="StudioVendorID" Width="180px" TabIndex=46></LCtrl:LajitTextBox>                                                                                     
                                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqStudioVendorID" MapXML="StudioVendorID" runat="server" ControlToValidate="txtStudioVendorID" ValidationGroup="LAJITEntryForm"
                                                                                                  ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                      </tr>                                                                                                                                                                            
                                                                                       <tr align="left" valign="top" id="trNewActivity" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label  runat="server" ID="lblNewActivity"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                            <LCtrl:LAjitCheckBox ID="chkNewActivity" runat="server" MapXML="NewActivity" TabIndex=47/>
                                                                                            </td>
                                                                                       </tr>
                                                                                       <tr id="trOther" align="left" valign="top" runat="server">
                                                                                            <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">                                                                                            
                                                                                                <asp:Label  runat="server" ID="lblOther" SkinID="LabelSubHead">Payment Information</asp:Label>
                                                                                            </td>
                                                                                       </tr>
                                                                                                                                                                                                                                                                                                                                             
                                                                                      <tr align="left" valign="top" id="trCreditLimit" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblCreditLimit"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle"> <LCtrl:LajitTextBox ID="txtCreditLimit" runat="server" MapXML="CreditLimit" Width="180px" TabIndex=48></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCreditLimit" MapXML="CreditLimit" runat="server" ControlToValidate="txtCreditLimit" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> </td>
                                                                                      </tr>  
                                                                                      
                                                                                      
                                                                                       <tr align="left" valign="top" id="trVendPayTerm" runat="server">
                                                                                            <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblVendPayTerm"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LAjitDropDownList ID="ddlVendPayTerm" runat="server" MapXML="VendPayTerm" Width="184px" TabIndex=49></LCtrl:LAjitDropDownList>
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqVendPayTerm" MapXML="VendPayTerm" runat="server" ControlToValidate="ddlVendPayTerm" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td> 
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trPaymentMethod" runat="server">
                                                                                            <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblPaymentMethod"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlPaymentMethod" runat="server" MapXML="PaymentMethod" Width="184px" TabIndex=50></LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentMethod" MapXML="PaymentMethod" runat="server" ControlToValidate="ddlPaymentMethod" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>   
                                                                                        </tr>
                                                                                         
                                                                                        <tr align="left" valign="top" id="trCurrencyTypeCompany" runat="server">
                                                                                             <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblCurrencyTypeCompany"  SkinID="LabelBranch"></asp:Label></td>
                                                                                             <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="184px" TabIndex=51></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" MapXML="CurrencyTypeCompany" runat="server" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>

                                                                                      <tr align="left" valign="top" id="trWireInfo" runat="server">
                                                                                           <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblWireInfo"  SkinID="LabelBranch"></asp:Label></td>
                                                                                           <td valign="middle"> <LCtrl:LajitTextBox ID="txtWireInfo" runat="server" MapXML="WireInfo" Width="180px" TabIndex=52></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqWireInfo" MapXML="WireInfo" runat="server" ControlToValidate="txtWireInfo" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> </td>
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
                                                                                   <tr id="trTax" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">                                                                                            
                                                                                            <asp:Label  runat="server" ID="lblTax" SkinID="LabelSubHead">Tax Information</asp:Label>
                                                                                        </td>
                                                                                   </tr> 
                                                                                      <tr align="left" valign="top" id="trSSNTaxID" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblSSNTaxID"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtSSNTaxID" runat="server" MapXML="SSNTaxID" Width="180px" TabIndex=53 ></LCtrl:LajitTextBox>                                                                                     
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSSNTaxID" MapXML="SSNTaxID" runat="server" ControlToValidate="txtSSNTaxID" ValidationGroup="LAJITEntryForm"
                                                                                                  ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                      </tr>
                                                                                    
                                                                                      <tr align="left" valign="top" id="trFederalTaxID" runat="server">
                                                                                             <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblFederalTaxID"  SkinID="LabelBranch"></asp:Label></td>
                                                                                             <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtFederalTaxID" runat="server" MapXML="FederalTaxID" Width="180px" TabIndex=54 ></LCtrl:LajitTextBox>                                                                                     
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqFederalTaxID" MapXML="FederalTaxID" runat="server" ControlToValidate="txtFederalTaxID" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                      </tr>
                                                                                       <tr align="left" valign="top" id="trIsCorp" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblIsCorp"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="top"><LCtrl:LAjitCheckBox ID="chkIsCorp" runat="server" MapXML="IsCorp" TabIndex=55/></td>
                                                                                       </tr>  
                                                                                      <tr align="left" valign="top" id="trDBAName" runat="server">
                                                                                         <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblDBAName"  SkinID="LabelBranch"></asp:Label></td>
                                                                                         <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtDBAName" runat="server" MapXML="DBAName" Width="180px" TabIndex=56></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDBAName" MapXML="DBAName" runat="server" ControlToValidate="txtDBAName" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                                            
                                                                                         </td>
                                                                                      </tr>                                                                                                                                                                                                                                                                    
                                                                                       <tr align="left" valign="top" id="trIs1099" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblIs1099"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="top"><LCtrl:LAjitCheckBox ID="chkIs1099" runat="server" MapXML="Is1099" TabIndex=57/></td>
                                                                                       </tr>
                                                                                                                                                                             
                                                                                       <tr align="left" valign="top" id="trIndContractor" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblIndContractor"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="top"><LCtrl:LAjitCheckBox ID="chkIndContractor" runat="server" MapXML="IndContractor" TabIndex=58 /></td>
                                                                                       </tr>   
                                                                                       <tr align="left" valign="top" id="trEDDStartDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblEDDStartDate"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                       <LCtrl:LajitTextBox ID="txtEDDStartDate" runat="server" MapXML="EDDStartDate" Width="68px" TabIndex="59"></LCtrl:LajitTextBox>
                                                                                                    </td> 
                                                                                                    <td>
                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqEDDStartDate" MapXML="EDDStartDate" runat="server" ControlToValidate="txtEDDStartDate" ValidationGroup="LAJITEntryForm"
                                                                                                         ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>                                                                                                             
                                                                                            </td>
                                                                                      </tr>
                                                                                       <tr align="left" valign="top" id="trEDDEndDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblEDDEndDate"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                           <LCtrl:LajitTextBox ID="txtEDDEndDate" runat="server" MapXML="EDDEndDate" Width="68px" TabIndex=60></LCtrl:LajitTextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEDDEndDate" MapXML="EDDEndDate" runat="server" ControlToValidate="txtEDDEndDate" ValidationGroup="LAJITEntryForm"
                                                                                                             ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>                                                                                                             
                                                                                            </td>
                                                                                      </tr>                                                                                 
                                                                                           <tr align="left" valign="top" id="trEDDContOnGoing" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblEDDContOnGoing"  SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="top"><LCtrl:LAjitCheckBox ID="chkEDDContOnGoing" runat="server" MapXML="EDDContOnGoing" TabIndex=61/></td>
                                                                                           </tr>                                                                                                                                                                         
                                                                                     
                                                                                      <tr align="left" valign="top" id="trEDDContAmt" runat="server">
                                                                                         <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblEDDContAmt"  SkinID="LabelBranch"></asp:Label></td>
                                                                                         <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtEDDContAmt" runat="server" MapXML="EDDContAmt" Width="180px" TabIndex=62></LCtrl:LajitTextBox>                                                                                     
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEDDContAmt" MapXML="EDDContAmt" runat="server" ControlToValidate="txtEDDContAmt" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            <LCtrl:LAjitRegularExpressionValidator ID="regEDDContAmt" runat="server" ControlToValidate="txtEDDContAmt" MapXML="EDDContAmt"
                                                                                            ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value." ErrorMessage="should be numeric"
                                                                                            Display="dynamic" ValidationGroup="LAJITEntryForm" Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                                         </td>
                                                                                      </tr>
                                                                                      <tr align="left" valign="top" id="trW9OnFile" runat="server">
                                                                                        <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblW9OnFile"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="top"><LCtrl:LAjitCheckBox ID="chkW9OnFile" runat="server" MapXML="W9OnFile" TabIndex=63/></td>
                                                                                        </tr>  
                                                                                                                                                                          
                                                                                        <tr align="left" valign="top" id="trSelectRegion" runat="server">
                                                                                             <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblSelectRegion"  SkinID="LabelBranch"></asp:Label></td>
                                                                                             <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectRegion" runat="server" MapXML="SelectRegion" Width="184px" TabIndex=64></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion" MapXML="SelectRegion" runat="server" ControlToValidate="ddlSelectRegion" ValidationGroup="LAJITEntryForm"
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
                                                                     <!-- Panel Other  End -->
                                                                </td>
                                                            </tr>                                                                                                                                                                                     
                                                            <!--Submit and Cancel buttons-->                                                           
                                                        <tr style="height:24px">
                                                                <td align="center" colspan="2">
                                                                <table border="0">
                                                                   <tr>
                                                                        <td>
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click" OnClientClick="javascript:retun ValidateControls()" 
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
                                                            <tr style="height:10px">
                                                                <td colspan="2">
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
    <%--<script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>
<script type="text/javascript" src="../JavaScript/BranchDropDowns.js"></script>--%>
<script language="javascript" type="text/javascript">
function HideShowDiv(divname)
{
    switch (divname)
    {
        case "General":
                          //content
                          document.getElementById("divGeneral").style.display="block";
                          document.getElementById("divContact").style.display="none";
                          document.getElementById("divOther").style.display="none";
                          //Tabs  
                          document.getElementById("divGeneralTab").style.display="block";
                          document.getElementById("divContactTab").style.display="none";
                          document.getElementById("divOtherTab").style.display="none"; 
                          break;
        case "Contact": 
                          //content  
                          document.getElementById("divGeneral").style.display="none";
                          document.getElementById("divContact").style.display="block";
                          document.getElementById("divOther").style.display="none";                          
                          //Tabs  
                          document.getElementById("divGeneralTab").style.display="none";
                          document.getElementById("divContactTab").style.display="block";
                          document.getElementById("divOtherTab").style.display="none";                          
                          break; 
        case "Other": 
                          //content  
                          document.getElementById("divGeneral").style.display="none";
                          document.getElementById("divContact").style.display="none";
                          document.getElementById("divOther").style.display="block";                          
                          //Tabs  
                          document.getElementById("divGeneralTab").style.display="none";
                          document.getElementById("divContactTab").style.display="none";
                          document.getElementById("divOtherTab").style.display="block";                          
                          break;                            
    }   
}
    
</script>
</asp:content>
