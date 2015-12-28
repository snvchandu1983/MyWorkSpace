<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Customer.aspx.cs" Inherits="LAjitDev.Receivables.Customer"%>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
   <script type="text/javascript">
    function JQueryPageEvents(){
        AttachAutoComplete("ctl00_cphPageContents_txtLastName",'Customer','<%=this.Page.ToString() %>');
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="511px"  width="100%" style="background-color:White;">
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
                                                          <table width="100%" style="height:390px" border="0" cellspacing="0" cellpadding="0" class="formmiddle"> 
                                                            <tr style="border-style:none;">
                                                                <td colspan="2" valign="top">
                                                                    <!-- Panel General  Start -->
                                                                    <div id="divGeneral" style="display:block;">
                                                                      <table width="100%" border="0" bordercolor="red" cellspacing="0" cellpadding="0">
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
                                                                                           <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus"  OnClick="imgbtnIsApproved_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="trName" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label  runat="server" ID="lblName" SkinID="LabelSubHead">Name</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trLastName" runat="server">
                                                                                           <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblLastName" SkinID="LabelBranch" ></asp:Label></td>
                                                                                           <td valign="middle" style="width:196px;">
                                                                                                <LCtrl:LajitTextBox ID="txtLastName" runat="server" MapXML="LastName" Width="180px" TabIndex=1 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqLastName" MapXML="LastName" runat="server" ControlToValidate="txtLastName" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trFirstName" runat="server">
                                                                                           <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblFirstName"  SkinID="LabelBranch"></asp:Label></td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtFirstName" runat="server" MapXML="FirstName" Width="180px"   TabIndex=2 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqFirstName" MapXML="FirstName" runat="server" ControlToValidate="txtFirstName" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trNumberID" runat="server">
                                                                                            <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblNumberID"  SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtNumberID" runat="server" MapXML="NumberID" Width="180px"  TabIndex=3 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqNumberID" MapXML="NumberID" runat="server" ControlToValidate="txtNumberID" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                     
                                                                                    <tr id="trCustomerAddress" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblAddress1_CustomerAddress" SkinID="LabelSubHead" MapBranchNode="CustomerAddress"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trAddressType_CustomerAddress" runat="server">
                                                                                            <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlAddressType_CustomerAddress" runat="server" MapXML="AddressType" MapBranchNode="CustomerAddress" Width="130px" TabIndex=4 ></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlAddressType_CustomerAddress_SelectedIndexChanged" -->
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_CustomerAddress" MapXML="AddressType" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtAddress1_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtAddress1_CustomerAddress" runat="server" MapXML="Address1" MapBranchNode="CustomerAddress" Width="180px" TabIndex=6></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_CustomerAddress" MapXML="Address1" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtAddress1_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAddress2_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerAddress" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerAddress" TabIndex=5 />
                                                                                            <asp:Label runat="server" ID="lblIsPrimary_CustomerAddress"  SkinID="LabelBranch" MapBranchNode="CustomerAddress"></asp:Label>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress2_CustomerAddress" runat="server" MapXML="Address2" MapBranchNode="CustomerAddress" Width="180px" TabIndex=7 ></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_CustomerAddress" MapXML="Address2" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtAddress2_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trAddress3_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress3_CustomerAddress" runat="server" MapXML="Address3" MapBranchNode="CustomerAddress" Width="180px" TabIndex=56></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_CustomerAddress" MapXML="Address3" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtAddress3_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trAddress4_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtAddress4_CustomerAddress" runat="server" MapXML="Address4" MapBranchNode="CustomerAddress" Width="180px" TabIndex=57></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_CustomerAddress" MapXML="Address4" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtAddress4_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trCity_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblCity_CustomerAddress"  SkinID="LabelBranch" MapBranchNode="CustomerAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtCity_CustomerAddress" runat="server" MapXML="City" MapBranchNode="CustomerAddress" Width="180px" TabIndex=8  ></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCity_CustomerAddress" MapXML="City" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtCity_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                   </tr>
                                                                                        <tr align="left" valign="top" id="trArea_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label  runat="server" ID="lblArea_CustomerAddress"  MapBranchNode="CustomerAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtArea_CustomerAddress" runat="server" MapXML="Area" MapBranchNode="CustomerAddress" Width="180px" TabIndex=9 ></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqArea_CustomerAddress" MapXML="Area" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtArea_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trSelectCountry_CustomerAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblSelectCountry_CustomerAddress" MapBranchNode="CustomerAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                             <LCtrl:LAjitDropDownList ID="ddlSelectCountry_CustomerAddress" runat="server" MapXML="SelectCountry" MapBranchNode="CustomerAddress" Width="184px" TabIndex=10 ></LCtrl:LAjitDropDownList>
                                                                                             <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_CustomerAddress" MapXML="SelectCountry" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="ddlSelectCountry_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                        </td> 
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trSelectRegion_CustomerAddress" runat="server">
                                                                                        <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblSelectRegion_CustomerAddress"  SkinID="LabelBranch" MapBranchNode="CustomerAddress"></asp:Label></td>
                                                                                        <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LAjitDropDownList ID="ddlSelectRegion_CustomerAddress" runat="server" MapXML="SelectRegion" MapBranchNode="CustomerAddress" Width="184px" TabIndex=11 ></LCtrl:LAjitDropDownList>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_CustomerAddress" MapXML="SelectRegion" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="ddlSelectRegion_CustomerAddress" ValidationGroup="LAJITEntryForm"
                                                                                            ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>   
                                                                                    </tr>
                                                                                    <tr align="left" valign="top" id="trPostalCode_CustomerAddress" runat="server">
                                                                                       <td class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label runat="server" ID="lblPostalCode_CustomerAddress"  SkinID="LabelBranch" MapBranchNode="CustomerAddress"></asp:Label></td>
                                                                                       <td valign="middle" style="width:196px; ">
                                                                                            <LCtrl:LajitTextBox ID="txtPostalCode_CustomerAddress" runat="server" MapXML="PostalCode" MapBranchNode="CustomerAddress" Width="180px" TabIndex=12 ></LCtrl:LajitTextBox>
                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_CustomerAddress" MapXML="PostalCode" MapBranchNode="CustomerAddress" runat="server" ControlToValidate="txtPostalCode_CustomerAddress" ValidationGroup="LAJITEntryForm"
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
                                                                                     <tr id="trCustomerPhone" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblTelephone_CustomerPhone" MapBranchNode="CustomerPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                         <tr align="left" valign="top" id="trPhoneType_CustomerPhone" runat="server">
                                                                                            <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                               <LCtrl:LAjitDropDownList ID="ddlPhoneType_CustomerPhone" runat="server" MapXML="PhoneType" MapBranchNode="CustomerPhone" Width="130px"  TabIndex=13  ></LCtrl:LAjitDropDownList> <!-- AutoPostBack=true OnSelectedIndexChanged="ddlPhoneType_CustomerPhone_SelectedIndexChanged" -->
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_CustomerPhone" MapXML="PhoneType" MapBranchNode="CustomerPhone" runat="server" ControlToValidate="txtTelephone_CustomerPhone" ValidationGroup="LAJITEntryForm"
                                                                                                  ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td> 
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                              <LCtrl:LajitTextBox ID="txtTelephone_CustomerPhone" runat="server" MapXML="Telephone" MapBranchNode="CustomerPhone" Width="180px"  TabIndex=15 ></LCtrl:LajitTextBox>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_CustomerPhone" MapXML="Telephone" MapBranchNode="CustomerPhone" runat="server" ControlToValidate="txtTelephone_CustomerPhone" ValidationGroup="LAJITEntryForm"
                                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trIsPrimary_CustomerPhone" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerPhone" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerPhone"  TabIndex=14 />
                                                                                                <asp:Label runat="server" ID="lblIsPrimary_CustomerPhone" SkinID="LabelBranch" MapBranchNode="CustomerPhone"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px;"></td>
                                                                                         </tr>

                                                                                     <tr id="trCustomerEmail" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblEmail_CustomerEmail" MapBranchNode="CustomerEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                      </tr>
                                                                                         <tr align="left" valign="top" id="trContactInfo_CustomerEmail" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                               <asp:Label runat="server" ID="lblContactInfo_CustomerEmail" MapBranchNode="CustomerEmail" SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                               <LCtrl:LajitTextBox ID="txtContactInfo_CustomerEmail" runat="server" MapXML="ContactInfo" MapBranchNode="CustomerEmail" Width="180px" TabIndex=16 ></LCtrl:LajitTextBox>
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_CustomerEmail" MapXML="ContactInfo" MapBranchNode="CustomerEmail" runat="server" ControlToValidate="txtContactInfo_CustomerEmail" ValidationGroup="LAJITEntryForm"
                                                                                               ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trEmailType_CustomerEmail"  runat="server">
                                                                                           <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                               <LCtrl:LAjitDropDownList ID="ddlEmailType_CustomerEmail" runat="server" MapXML="EmailType" MapBranchNode="CustomerEmail" Width="130px" TabIndex=17  ></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlEmailType_CustomerEmail_SelectedIndexChanged" -->
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_CustomerEmail" MapXML="EmailType" MapBranchNode="CustomerEmail" runat="server" ControlToValidate="txtEmail_CustomerEmail" ValidationGroup="LAJITEntryForm"
                                                                                                   ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                           </td> 
                                                                                           <td valign="middle" style="width:196px;">
                                                                                               <LCtrl:LajitTextBox ID="txtEmail_CustomerEmail" runat="server" MapXML="Email" MapBranchNode="CustomerEmail" Width="180px" TabIndex=19 ></LCtrl:LajitTextBox>
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_CustomerEmail" MapXML="Email" MapBranchNode="CustomerEmail" runat="server" ControlToValidate="txtEmail_CustomerEmail" ValidationGroup="LAJITEntryForm"
                                                                                                   ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                               <LCtrl:LAjitRegularExpressionValidator ID="regEmail_CustomerEmail" runat="server" ControlToValidate="txtEmail_CustomerEmail" MapXML="Email"
                                                                                                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                   Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>
                                                                                          </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trIsPrimary_CustomerEmail" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerEmail" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerEmail" TabIndex=18/>
                                                                                                <asp:Label runat="server" ID="lblIsPrimary_CustomerEmail" SkinID="LabelBranch" MapBranchNode="CustomerEmail"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px;"></td>
                                                                                        </tr>

                                                                                     <tr id="trCustomerWebsite" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblWebsite_CustomerWebsite" MapBranchNode="CustomerWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                         <tr align="left" valign="top" id="trWebAddressType_CustomerWebsite" runat="server">
                                                                                            <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                                 <LCtrl:LAjitDropDownList ID="ddlWebAddressType_CustomerWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="CustomerWebsite" Width="130px" TabIndex=20 ></LCtrl:LAjitDropDownList><!-- AutoPostBack="true" OnSelectedIndexChanged="ddlWebAddressType_CustomerWebsite_SelectedIndexChanged" -->
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_CustomerWebsite" MapXML="WebAddressType" MapBranchNode="CustomerWebsite" runat="server" ControlToValidate="txtWebsite_CustomerWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td> 
                                                                                            <td valign="middle" style="width:196px;">
                                                                                                <LCtrl:LajitTextBox ID="txtWebsite_CustomerWebsite" runat="server" MapXML="Website" MapBranchNode="CustomerWebsite" Width="180px" TabIndex=22 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_CustomerWebsite" MapXML="Website" MapBranchNode="CustomerWebsite" runat="server" ControlToValidate="txtWebsite_CustomerWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trIsPrimary_CustomerWebsite" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerWebsite" TabIndex=21 />
                                                                                                <asp:Label runat="server" ID="lblIsPrimary_CustomerWebsite" SkinID="LabelBranch" MapBranchNode="CustomerWebsite"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px; "></td>
                                                                                         </tr>

                                                                                     <tr id="trCustomerGroupItem" align="left" valign="top" runat="server">
                                                                                         <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblCustomerGroupSubHead_CustomerGroup" MapBranchNode="CustomerGroup" SkinID="LabelSubHead">CustomerGroup</asp:Label>
                                                                                         </td>
                                                                                     </tr>
                                                                                        <tr id="trCustomerGroup_CustomerGroupItem" align="left" valign="top" visible="true" runat="server">
                                                                                           <td align="right" valign="top" class="formtext" style="height:24px; width:141px;">
                                                                                            <asp:Label SkinID="LabelBranch" ID="lblCustomerGroup" runat="server"></asp:Label>
                                                                                           </td>
                                                                                           <td valign="middle" style="width:196px;">
                                                                                               <LCtrl:LAjitListBox ID="lstBxCustomerGroup" runat="server" SelectionMode="Multiple"
                                                                                                   MapXML="CustomerGroup" XMLType="ParentChild" Height="120" Width="180" TabIndex=23 >
                                                                                               </LCtrl:LAjitListBox>
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqCustomerGroup_CustomerGroupItem" runat="server" MapXML="CustomerGroup" ControlToValidate="lstBxCustomerGroup" ValidationGroup="LAJITEntryForm"
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
                                                                                    <tr id="trCustContactAddress" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblAddress1_CustContactAddress" SkinID="LabelSubHead" MapBranchNode="CustContactAddress"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                        <tr align="left" valign="top" id="trContactInfo_CustContactAddress" runat="server">
                                                                                              <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                 <asp:Label runat="server" ID="lblContactInfo_CustContactAddress" MapBranchNode="CustContactAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                              <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtContactInfo_CustContactAddress" runat="server" MapXML="ContactInfo" MapBranchNode="CustContactAddress" Width="180px" TabIndex=24></LCtrl:LajitTextBox>
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_CustContactAddress" MapXML="ContactInfo" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtContactInfo_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                              </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAddressType_CustContactAddress" runat="server">
                                                                                            <td align="right" valign="top" style="height:24px; width:141px;padding-right: 20px;">
                                                                                                 <LCtrl:LAjitDropDownList ID="ddlAddressType_CustContactAddress" runat="server" MapXML="AddressType" MapBranchNode="CustContactAddress" Width="130px" TabIndex=25></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlAddressType_CustContactAddress_SelectedIndexChanged" -->
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqAddressType_CustContactAddress" MapXML="AddressType" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtAddress1_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtAddress1_CustContactAddress" runat="server" MapXML="Address1" MapBranchNode="CustContactAddress" Width="180px" TabIndex=27 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress1_CustContactAddress" MapXML="Address1" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtAddress1_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAddress2_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustContactAddress" runat="server" MapXML="IsPrimary" MapBranchNode="CustContactAddress" TabIndex=26/>
                                                                                                <asp:Label  runat="server" ID="lblIsPrimary_CustContactAddress" SkinID="LabelBranch" MapBranchNode="CustContactAddress"></asp:Label>
                                                                                           </td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtAddress2_CustContactAddress" runat="server" MapXML="Address2" MapBranchNode="CustContactAddress" Width="180px" TabIndex=28 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress2_CustContactAddress" MapXML="Address2" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtAddress2_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                           </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAddress3_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                           </td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtAddress3_CustContactAddress" runat="server" MapXML="Address3" MapBranchNode="CustContactAddress" Width="180px" TabIndex=58></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress3_CustContactAddress" MapXML="Address3" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtAddress3_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAddress4_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                           </td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtAddress4_CustContactAddress" runat="server" MapXML="Address4" MapBranchNode="CustContactAddress" Width="180px" TabIndex=59></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAddress4_CustContactAddress" MapXML="Address4" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtAddress4_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trCity_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label  runat="server" ID="lblCity_CustContactAddress" SkinID="LabelBranch" MapBranchNode="CustContactAddress"></asp:Label></td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtCity_CustContactAddress" runat="server" MapXML="City" MapBranchNode="CustContactAddress" Width="180px" TabIndex=29 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCity_CustContactAddress" MapXML="City" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtCity_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                           </td>
                                                                                       </tr>
                                                                                        <tr align="left" valign="top" id="trArea_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label runat="server" ID="lblArea_CustContactAddress" MapBranchNode="CustContactAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtArea_CustContactAddress" runat="server" MapXML="Area" MapBranchNode="CustContactAddress" Width="180px" TabIndex=30></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqArea_CustContactAddress" MapXML="Area" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtArea_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trSelectCountry_CustContactAddress" runat="server">
                                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label runat="server" ID="lblSelectCountry_CustContactAddress" MapBranchNode="CustContactAddress" SkinID="LabelBranch"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LAjitDropDownList ID="ddlSelectCountry_CustContactAddress" runat="server" MapXML="SelectCountry" MapBranchNode="CustContactAddress" Width="184px" TabIndex=31></LCtrl:LAjitDropDownList>
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqSelectCountry_CustContactAddress" MapXML="SelectCountry" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="ddlSelectCountry_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td> 
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trSelectRegion_CustContactAddress" runat="server">
                                                                                            <td class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label runat="server" ID="lblSelectRegion_CustContactAddress" SkinID="LabelBranch" MapBranchNode="CustContactAddress"></asp:Label></td>
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlSelectRegion_CustContactAddress" runat="server" MapXML="SelectRegion" MapBranchNode="CustContactAddress" Width="184px" TabIndex=32></LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSelectRegion_CustContactAddress" MapXML="SelectRegion" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="ddlSelectRegion_CustContactAddress" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trPostalCode_CustContactAddress" runat="server">
                                                                                           <td class="formtext" style="height:24px; width:141px;">
                                                                                                <asp:Label runat="server" ID="lblPostalCode_CustContactAddress" SkinID="LabelBranch" MapBranchNode="CustContactAddress"></asp:Label></td>
                                                                                           <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtPostalCode_CustContactAddress" runat="server" MapXML="PostalCode" MapBranchNode="CustContactAddress" Width="180px" TabIndex=33 ></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostalCode_CustContactAddress" MapXML="PostalCode" MapBranchNode="CustContactAddress" runat="server" ControlToValidate="txtPostalCode_CustContactAddress" ValidationGroup="LAJITEntryForm"
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
                                                                                     <tr id="trCustomerContactPhone" align="left" valign="top" runat="server">
                                                                                        <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblTelephone_CustomerContactPhone"  MapBranchNode="CustomerContactPhone" SkinID="LabelSubHead"></asp:Label>
                                                                                        </td>
                                                                                     </tr>
                                                                                         <tr align="left" valign="top" id="trContactInfo_CustomerContactPhone" runat="server">
                                                                                             <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <asp:Label runat="server" ID="lblContactInfo_CustomerContactPhone" MapBranchNode="CustomerContactPhone" SkinID="LabelBranch"></asp:Label></td>
                                                                                             <td valign="middle" style="width:196px; ">
                                                                                                <LCtrl:LajitTextBox ID="txtContactInfo_CustomerContactPhone" runat="server" MapXML="ContactInfo" MapBranchNode="CustomerContactPhone" Width="180px" TabIndex=34></LCtrl:LajitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_CustomerContactPhone" MapXML="ContactInfo" MapBranchNode="CustomerContactPhone" runat="server" ControlToValidate="txtContactInfo_CustomerContactPhone" ValidationGroup="LAJITEntryForm"
                                                                                                ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                             </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trPhoneType_CustomerContactPhone" runat="server">
                                                                                            <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                                 <LCtrl:LAjitDropDownList ID="ddlPhoneType_CustomerContactPhone" runat="server" MapXML="PhoneType" MapBranchNode="CustomerContactPhone" Width="130px" TabIndex=35 ></LCtrl:LAjitDropDownList> <!-- AutoPostBack=true OnSelectedIndexChanged="ddlPhoneType_CustomerContactPhone_SelectedIndexChanged" -->
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqPhoneType_CustomerContactPhone" MapXML="PhoneType" MapBranchNode="CustomerContactPhone" runat="server" ControlToValidate="txtTelephone_CustomerContactPhone" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                            </td> 
                                                                                            <td valign="middle" style="width:196px; ">
                                                                                              <LCtrl:LajitTextBox ID="txtTelephone_CustomerContactPhone" runat="server" MapXML="Telephone" MapBranchNode="CustomerContactPhone" Width="180px" TabIndex=37></LCtrl:LajitTextBox>
                                                                                              <LCtrl:LAjitRequiredFieldValidator ID="reqTelephone_CustomerContactPhone" MapXML="Telephone" MapBranchNode="CustomerContactPhone" runat="server" ControlToValidate="txtTelephone_CustomerContactPhone" ValidationGroup="LAJITEntryForm"
                                                                                             ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>    
                                                                                            
                                                                                            </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trIsPrimary_CustomerContactPhone" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerContactPhone" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerContactPhone" TabIndex=36 />
                                                                                                <asp:Label  runat="server" ID="lblIsPrimary_CustomerContactPhone"  SkinID="LabelBranch" MapBranchNode="CustomerContactPhone"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width:196px; "></td>
                                                                                         </tr>

                                                                                     <tr id="trCustomerContactEmail" align="left" valign="top" runat="server">
                                                                                       <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                           <asp:Label runat="server" ID="lblEmail_CustomerContactEmail" MapBranchNode="CustomerContactEmail" SkinID="LabelSubHead"></asp:Label>
                                                                                       </td>
                                                                                     </tr>
                                                                                         <tr align="left" valign="top" id="trContactInfo_CustomerContactEmail" runat="server">
                                                                                                 <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                    <asp:Label runat="server" ID="lblContactInfo_CustomerContactEmail" MapBranchNode="CustomerContactEmail" SkinID="LabelBranch"></asp:Label></td>
                                                                                                 <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LajitTextBox ID="txtContactInfo_CustomerContactEmail" runat="server" MapXML="ContactInfo" MapBranchNode="CustomerContactEmail" Width="180px" TabIndex=38 ></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_CustomerContactEmail" MapXML="ContactInfo" MapBranchNode="CustomerContactEmail" runat="server" ControlToValidate="txtContactInfo_CustomerContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trEmailType_CustomerContactEmail"  runat="server">
                                                                                             <td align="right" valign="top" style="height:20px; width:141px;">
                                                                                                  <LCtrl:LAjitDropDownList ID="ddlEmailType_CustomerContactEmail" runat="server" MapXML="EmailType" MapBranchNode="CustomerContactEmail" Width="130px" TabIndex=39 ></LCtrl:LAjitDropDownList> <!-- AutoPostBack=true OnSelectedIndexChanged="ddlEmailType_CustomerContactEmail_SelectedIndexChanged" -->
                                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqEmailType_CustomerContactEmail" MapXML="EmailType" MapBranchNode="CustomerContactEmail" runat="server" ControlToValidate="txtEmail_CustomerContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                             </td> 
                                                                                             <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtEmail_CustomerContactEmail" runat="server" MapXML="Email" MapBranchNode="CustomerContactEmail" Width="180px" TabIndex=41></LCtrl:LajitTextBox>
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqEmail_CustomerContactEmail" MapXML="Email" MapBranchNode="CustomerContactEmail" runat="server" ControlToValidate="txtEmail_CustomerContactEmail" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                 <LCtrl:LAjitRegularExpressionValidator ID="regEmail_CustomerContactEmail" runat="server" ControlToValidate="txtEmail_CustomerContactEmail" MapXML="Email"
                                                                                                     ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Please enter a valid email address like name@internet.com." ErrorMessage="Should be Valid Email address"
                                                                                                     Display="dynamic" ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRegularExpressionValidator>
                                                                                             </td>
                                                                                         </tr>
                                                                                         <tr align="left" valign="top" id="trIsPrimary_CustomerContactEmail" runat="server">
                                                                                             <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                 <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustomerContactEmail" runat="server" MapXML="IsPrimary" MapBranchNode="CustomerContactEmail" TabIndex=40/>
                                                                                                 <asp:Label  runat="server" ID="lblIsPrimary_CustomerContactEmail"  SkinID="LabelBranch" MapBranchNode="CustomerContactEmail"></asp:Label>
                                                                                             </td>
                                                                                             <td valign="middle" style="width:196px;"></td>
                                                                                         </tr>

                                                                                     <tr id="trCustContactWebsite" align="left" valign="top" runat="server">
                                                                                         <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                            <asp:Label runat="server" ID="lblWebsite_CustContactWebsite" MapBranchNode="CustContactWebsite" SkinID="LabelSubHead"></asp:Label>
                                                                                         </td>
                                                                                     </tr>
                                                                                        <tr align="left" valign="top" id="trContactInfo_CustContactWebsite" runat="server">
                                                                                              <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                 <asp:Label runat="server" ID="lblContactInfo_CustContactWebsite" MapBranchNode="CustContactWebsite" SkinID="LabelBranch"></asp:Label></td>
                                                                                              <td valign="middle" style="width:196px; ">
                                                                                                 <LCtrl:LajitTextBox ID="txtContactInfo_CustContactWebsite" runat="server" MapXML="ContactInfo" MapBranchNode="CustContactWebsite" Width="180px" TabIndex=42></LCtrl:LajitTextBox>
                                                                                                 <LCtrl:LAjitRequiredFieldValidator ID="reqContactInfo_CustContactWebsite" MapXML="ContactInfo" MapBranchNode="CustContactWebsite" runat="server" ControlToValidate="txtContactInfo_CustContactWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                 ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                              </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trWebAddressType_CustContactWebsite" runat="server">
                                                                                             <td align="right" valign="top" style="height:24px; width:141px;">
                                                                                                  <LCtrl:LAjitDropDownList ID="ddlWebAddressType_CustContactWebsite" runat="server" MapXML="WebAddressType" MapBranchNode="CustContactWebsite" Width="130px" TabIndex=43></LCtrl:LAjitDropDownList> <!-- AutoPostBack="true" OnSelectedIndexChanged="ddlWebAddressType_CustContactWebsite_SelectedIndexChanged" -->
                                                                                                  <LCtrl:LAjitRequiredFieldValidator ID="reqWebAddressType_CustContactWebsite" MapXML="WebAddressType" MapBranchNode="CustContactWebsite" runat="server" ControlToValidate="ddlWebAddressType_CustContactWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                             </td> 
                                                                                             <td valign="middle" style="width:196px;">
                                                                                                   <LCtrl:LajitTextBox ID="txtWebsite_CustContactWebsite" runat="server" MapXML="Website" MapBranchNode="CustContactWebsite" Width="180px" TabIndex=45></LCtrl:LajitTextBox>
                                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqWebsite_CustContactWebsite" MapXML="Website" MapBranchNode="CustContactWebsite" runat="server" ControlToValidate="txtWebsite_CustContactWebsite" ValidationGroup="LAJITEntryForm"
                                                                                                     ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                             </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trIsPrimary_CustContactWebsite" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height:24px; width:125px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkIsPrimary_CustContactWebsite" runat="server" MapXML="IsPrimary" MapBranchNode="CustContactWebsite" TabIndex=44/>
                                                                                                <asp:Label runat="server" ID="lblIsPrimary_CustContactWebsite" SkinID="LabelBranch" MapBranchNode="CustContactWebsite"></asp:Label>
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
                                                                                          <tr id="trOther" align="left" valign="top" runat="server">
                                                                                            <td colspan="2" align="right" valign="top" class="formSubHeadText" style="height:30px">
                                                                                              <asp:Label runat="server" ID="lblOther" SkinID="LabelSubHead">Payment Information</asp:Label>
                                                                                            </td>
                                                                                          </tr>
                                                                                             <tr align="left" valign="top" id="trNewActivity" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label runat="server" ID="lblNewActivity" SkinID="LabelBranch"></asp:Label>
                                                                                                </td>
                                                                                                <td valign="top"><LCtrl:LAjitCheckBox ID="chkNewActivity" runat="server" MapXML="NewActivity" TabIndex=46 />
                                                                                                </td>
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trCreditLimit" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblCreditLimit"  SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle"> <LCtrl:LajitTextBox ID="txtCreditLimit" runat="server" MapXML="CreditLimit" Width="180px" TabIndex=47></LCtrl:LajitTextBox>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCreditLimit" MapXML="CreditLimit" runat="server" ControlToValidate="txtCreditLimit" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> </td>
                                                                                             </tr> 
                                                                                             <tr align="left" valign="top" id="trVendPayTerm" runat="server">
                                                                                                <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblVendPayTerm"  SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                     <LCtrl:LAjitDropDownList ID="ddlVendPayTerm" runat="server" MapXML="VendPayTerm" Width="184px" TabIndex=48></LCtrl:LAjitDropDownList>
                                                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqVendPayTerm" MapXML="VendPayTerm" runat="server" ControlToValidate="ddlVendPayTerm" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td> 
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trPaymentMethod" runat="server">
                                                                                                <td align="right" valign="top"  class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblPaymentMethod"  SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlPaymentMethod" runat="server" MapXML="PaymentMethod" Width="184px" TabIndex=49></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentMethod" MapXML="PaymentMethod" runat="server" ControlToValidate="ddlPaymentMethod" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td>   
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trCurrencyTypeCompany" runat="server">
                                                                                                 <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label  runat="server" ID="lblCurrencyTypeCompany"  SkinID="LabelBranch"></asp:Label></td>
                                                                                                 <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany" Width="184px" TabIndex=50></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" MapXML="CurrencyTypeCompany" runat="server" ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                </td>
                                                                                             </tr>
                                                                                             <tr align="left" valign="top" id="trCreditRating" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label runat="server" ID="lblCreditRating" SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlCreditRating" runat="server" MapXML="CreditRating" Width="184px" TabIndex=51></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCreditRating" MapXML="CreditRating" runat="server" ControlToValidate="ddlCreditRating" ValidationGroup="LAJITEntryForm"
                                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                             <tr align="left" valign="top" id="trSalesRep" runat="server">
                                                                                                <td align="right" valign="top" class="formtext" style="height:24px; width:141px;"><asp:Label runat="server" ID="lblSalesRep" SkinID="LabelBranch"></asp:Label></td>
                                                                                                <td valign="middle" style="width:196px; ">
                                                                                                    <LCtrl:LAjitDropDownList ID="ddlSalesRep" runat="server" MapXML="SalesRep" Width="184px" TabIndex=52></LCtrl:LAjitDropDownList>
                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSalesRep" MapXML="SalesRep" runat="server" ControlToValidate="ddlSalesRep" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                                </td>
                                                                                            </tr>
                                                                                     </table>
                                                                                 <!--Left Table End -->        
                                                                             </td>
                                                                             <td valign="middle" style="width:5px; ">
                                                                                <!-- Spacer start -->
                                                                                     <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                 <!-- Spacer end --> 
                                                                             </td>
                                                                          </tr>
                                                                        </table>
                                                                     </div>
                                                                     <!-- Panel Other End -->
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
                                                                              Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click" 
                                                                              Height="18" AlternateText="ContinueAdd" />
                                                                        </td>
                                                                       <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click" 
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
                <asp:HiddenField ID="hdnAutoFillNewEntry" runat="server" />
            </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
 <%--   <script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>
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
