<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Commercial.aspx.cs" Inherits="LAjitDev.Financials.Commercial" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <script language="javascript" type="text/javascript">
    
    function HideShowDiv(divname)
    {
        switch (divname)
        {
            case "GeneralInfo":
                //Content
                document.getElementById("divGeneralInfo").style.display="block";
                document.getElementById("divProductInfo").style.display="none";
                document.getElementById("divAmountDetails").style.display="none";
                //document.getElementById("divProfitRules").style.display="none";
                //Tabs  
                document.getElementById("divGeneralInfoTab").style.display="block";
                document.getElementById("divProductInfoTab").style.display="none";
                document.getElementById("divAmountDetailsTab").style.display="none";
                //document.getElementById("divProfitRulesTab").style.display="none";
                break;
            case "ProductInfo": 
                //Content
                document.getElementById("divGeneralInfo").style.display="none";
                document.getElementById("divProductInfo").style.display="block";
                document.getElementById("divAmountDetails").style.display="none";
                //document.getElementById("divProfitRules").style.display="none";
                //Tabs
                document.getElementById("divGeneralInfoTab").style.display="none";
                document.getElementById("divProductInfoTab").style.display="block";
                document.getElementById("divAmountDetailsTab").style.display="none";
                //document.getElementById("divProfitRulesTab").style.display="none";
                break;
            case "AmountDetails":  
                //Content
                document.getElementById("divGeneralInfo").style.display="none";
                document.getElementById("divProductInfo").style.display="none";
                document.getElementById("divAmountDetails").style.display="block";
                //document.getElementById("divProfitRules").style.display="none"; 
                //Tabs
                document.getElementById("divGeneralInfoTab").style.display="none";
                document.getElementById("divProductInfoTab").style.display="none";
                document.getElementById("divAmountDetailsTab").style.display="block";
                //document.getElementById("divProfitRulesTab").style.display="none"; 
                break; 
        }
    }
    </script>
   
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
            <contenttemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <!--Btns Usercontrol-->
                         <td class="tdBtnsUC">                      
                            <BtnsUC:BtnsUserControl ID="BtnsUC" runat="server" />
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="555px" Style="background-color: White;" Width="100%">
                                            <!-- entry form start -->
                                            <table style="height: 500px" width="100%" border="0" cellspacing="0" cellpadding="0">
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
                                                <tr style="height: 10px;">
                                                    <td align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 10px;">
                                                            <tr id="trProcessLinks" runat="server" align="right" style="height: 10px;" valign="middle">
                                                                <td id="tdProcessLinks" runat="server" align="right" valign="middle">
                                                                    <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" style="height: 10px; background-color: Green" visible="false">
                                                    <td class="bigtitle" valign="middle">
                                                        <img alt="spacer" height="6px" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif"
                                                            width="5" />
                                                        <asp:Label ID="lblPageSubject" runat="server" SkinID="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <!-- tab headings start -->
                                                <tr style="height:10px;">
                                                    <td valign="bottom">
                                                        <div id="divGeneralInfoTab" style="display: block;">
                                                            <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0"
                                                                cellspacing="0">
                                                                <tr align="left" valign="bottom">
                                                                    <td style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width: 85px; background-color: #c5d5fc;">
                                                                        General Info</td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('ProductInfo');" class="blueboldlinks01" align="center"
                                                                        valign="middle" style="width: 90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;">
                                                                        Product Info
                                                                    </td>
                                                                    <td align="center" valign="middle" style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('AmountDetails');" class="blueboldlinks01" align="center"
                                                                        valign="middle" style="width: 110px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;">
                                                                        Amount Details
                                                                    </td>
                                                                    <td style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divProductInfoTab" style="display: none">
                                                            <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0"
                                                                cellspacing="0">
                                                                <tr align="left" valign="bottom">
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('GeneralInfo');" class="blueboldlinks01" align="center" valign="middle" style="width:85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                    General Info
                                                                </td>
                                                                    <td align="center" valign="middle" style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width: 90px; background-color: #c5d5fc;">
                                                                        Product Info</td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                     <td onclick="javascript:HideShowDiv('AmountDetails');" class="blueboldlinks01" align="center" valign="middle" style="width:110px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                Amount Details
                                                                </td>
                                                                    <td style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divAmountDetailsTab" style="display: none">
                                                            <table width="100%" border="0" bgcolor="#ffffff" bordercolor="red" align="left" cellpadding="0"
                                                                cellspacing="0">
                                                                <tr align="left" valign="bottom">
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('GeneralInfo');" class="blueboldlinks01" align="center" valign="middle" style="width:85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                    General Info
                                                                    </td>
                                                                    <td align="center" valign="middle" style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif" width="5"
                                                                            height="21"></td>
                                                                    <td onclick="javascript:HideShowDiv('ProductInfo');" class="blueboldlinks01" align="center" valign="middle" style="width:90px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;; ">
                                                                        Product Info
                                                                    </td>
                                                                    <td style="width: 6px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif" width="6"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                    <td style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" class="bluebold" style="width: 110px; background-color: #c5d5fc;">
                                                                        Amount Details</td>
                                                                    <td align="center" valign="middle" style="width: 5px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" width="5"
                                                                            height="21"></td>
                                                                    <td align="center" valign="middle" style="width: 3px;">
                                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                   <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <!-- tab headings end -->
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <!-- form entry fields start -->
                                                        <table width="100%" style="height: 512px" border="0" cellspacing="0" cellpadding="0"
                                                            class="formmiddle">
                                                            <tr style="border-style: none;">
                                                                <td colspan="2" valign="top">
                                                                    <!-- Panel General Info Start -->
                                                                    <div id="divGeneralInfo" style="display: block;">
                                                                        <table width="100%" border="0" bordercolor="red" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="height: 10px;" colspan="4">
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <!--Left Table Start -->
                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr id="trProductName" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblProductName" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtProductName" runat="server" MapXML="ProductName" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqProductName" runat="server" ControlToValidate="txtProductName"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="ProductName" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trDirectorEmployee" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblDirectorEmployee" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlDirectorEmployee" runat="server" MapXML="DirectorEmployee"
                                                                                                    Width="185px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqDirectorEmployee" runat="server" ControlToValidate="ddlDirectorEmployee"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="DirectorEmployee" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trExecProducer" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblExecProducer" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtExecProducer" runat="server" MapXML="ExecProducer" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqExecProducer" runat="server" ControlToValidate="txtExecProducer"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="ExecProducer" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trDirPhotoEmployee" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblDirPhotoEmployee" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlDirPhotoEmployee" runat="server" MapXML="DirPhotoEmployee"
                                                                                                    Width="185px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqDirPhotoEmployee" runat="server" ControlToValidate="ddlDirPhotoEmployee"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="DirPhotoEmployee" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trCustomer" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblCustomer" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlCustomer" runat="server" MapXML="Customer" Width="185px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCustomer" runat="server" ControlToValidate="ddlCustomer"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Customer" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trAgencyAddr" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblAgencyAddr" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyAddr" runat="server" MapXML="AgencyAddr" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyAddr" runat="server" ControlToValidate="txtAgencyAddr"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="AgencyAddr" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trAgencyPhone" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblAgencyPhone" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyPhone" runat="server" MapXML="AgencyPhone" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyPhone" runat="server" ControlToValidate="txtAgencyPhone"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="AgencyPhone" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trAgencyProducer" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblAgencyProducer" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyProducer" runat="server" MapXML="AgencyProducer"
                                                                                                    Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyProducer" runat="server" ControlToValidate="txtAgencyProducer"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="AgencyProducer" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trContact1" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblContact1" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtContact1" runat="server" MapXML="Contact1" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContact1" runat="server" ControlToValidate="txtContact1"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Contact1" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr valign="middle">
                                                                                            <td align="center" colspan="2">
                                                                                                &#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;</td>
                                                                                        </tr>
                                                                                        <tr id="trTitle1" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle1" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle1" runat="server" MapXML="Title1" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle1" runat="server" ControlToValidate="txtTitle1"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title1" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTitle2" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle2" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle2" runat="server" MapXML="Title2" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle2" runat="server" ControlToValidate="txtTitle2"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title2" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTitle3" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle3" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle3" runat="server" MapXML="Title3" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle3" runat="server" ControlToValidate="txtTitle3"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title3" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTitle4" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle4" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle4" runat="server" MapXML="Title4" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle4" runat="server" ControlToValidate="txtTitle4"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title4" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTitle5" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle5" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle5" runat="server" MapXML="Title5" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle5" runat="server" ControlToValidate="txtTitle5"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title5" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTitle6" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblTitle6" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtTitle6" runat="server" MapXML="Title6" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqTitle6" runat="server" ControlToValidate="txtTitle6"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="Title6" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trClosedJob" runat="server" align="right" valign="top">
                                                                                            <td align="left" class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                                <asp:Label ID="lblClosedJob" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" nowrap="NOWRAP" valign="middle">
                                                                                                <LCtrl:LAjitCheckBox ID="chkClosedJob" runat="server" MapXML="ClosedJob" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trCloseDate" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                                                <asp:Label ID="lblCloseDate" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                          <LCtrl:LAjitTextBox ID="txtCloseDate" runat="server" MapXML="CloseDate" Width="70px"></LCtrl:LAjitTextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqCloseDate" runat="server" ControlToValidate="txtCloseDate"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="CloseDate" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                        
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <!--Left Table End -->
                                                                                </td>
                                                                                <td valign="middle" style="width: 1px;">
                                                                                    <!-- Spacer start -->
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                    <!-- Spacer end -->
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <!--Right Table Start-->
                                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <!-- New Frame Start  -->
                                                                                                <table align="center" border="0" cellpadding="0" cellspacing="0" class="formblackborderDashed">
                                                                                                    <tr>
                                                                                                        <td style="height: 10px;" valign="middle">
                                                                                                            <!-- Spacer start -->
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                            <!-- Spacer end -->
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="height: 10px;" valign="middle">
                                                                                                            <!-- Spacer start -->
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                            <!-- Spacer end -->
                                                                                                        </td>
                                                                                                        <td align="center" colspan="2">
                                                                                                            <asp:Label ID="lblheading" runat="server" SkinID="LabelBig" Text="Budgeted"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trAtoK" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblAtoK" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAtoK" runat="server" CssClass="LajitTextBox1" MapXML="AtoK"
                                                                                                                Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAtoK" runat="server" ControlToValidate="txtAtoK"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="AtoK" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trDirCreativeFees" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblDirCreativeFees" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtDirCreativeFees" runat="server" CssClass="LajitTextBox1"
                                                                                                                MapXML="DirCreativeFees" Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqDirCreativeFees" runat="server" ControlToValidate="txtDirCreativeFees"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="DirCreativeFees" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trInsurance" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblInsurance" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtInsurance" runat="server" CssClass="LajitTextBox1" MapXML="Insurance"
                                                                                                                Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqInsurance" runat="server" ControlToValidate="txtInsurance"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="Insurance" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trProdFee" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblProdFee" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtProdFee" runat="server" CssClass="LajitTextBox1" MapXML="ProdFee"
                                                                                                                Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqProdFee" runat="server" ControlToValidate="txtProdFee"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="ProdFee" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trTalentCost" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblTalentCost" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtTalentCost" runat="server" CssClass="LajitTextBox1" MapXML="TalentCost"
                                                                                                                Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqTalentCost" runat="server" ControlToValidate="txtTalentCost"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="TalentCost" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trEditorialFinish" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblEditorialFinish" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtEditorialFinish" runat="server" CssClass="LajitTextBox1"
                                                                                                                MapXML="EditorialFinish" Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEditorialFinish" runat="server" ControlToValidate="txtEditorialFinish"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="EditorialFinish" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trEditorialFee" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblEditorialFee" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td cssclass="LajitTextBox1" style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtEditorialFee" runat="server" MapXML="EditorialFee" Style="text-align: center"
                                                                                                                Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqEditorialFee" runat="server" ControlToValidate="txtEditorialFee"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="EditorialFee" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trOtherExp" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblOtherExp" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtOtherExp" runat="server" CssClass="LajitTextBox1" MapXML="OtherExp"
                                                                                                                Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqOtherExp" runat="server" ControlToValidate="txtOtherExp"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="OtherExp" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trContractTotal" runat="server" align="left" valign="top">
                                                                                                        <td align="right" class="formtext" style="height: 24px; width: 125px;" valign="top">
                                                                                                            <asp:Label ID="lblContractTotal" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                        <td style="width: 196px;" valign="middle">
                                                                                                            <LCtrl:LAjitTextBox ID="txtContractTotal" runat="server" CssClass="LajitTextBox1"
                                                                                                                MapXML="ContractTotal" Style="text-align: center" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqContractTotal" runat="server" ControlToValidate="txtContractTotal"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="ContractTotal" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <!-- New Frame End  -->
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                &nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <table cellpadding="0" cellspacing="0" align="center" border="0">
                                                                                                    <tr align="left" valign="top" id="trLocationSite1" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblLocationSite1" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtLocationSite1" runat="server" MapXML="LocationSite1" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqLocationSite1" MapXML="LocationSite1" runat="server"
                                                                                                                ControlToValidate="txtLocationSite1" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trLocationSite2" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblLocationSite2" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtLocationSite2" runat="server" MapXML="LocationSite2" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqLocationSite2" MapXML="LocationSite2" runat="server"
                                                                                                                ControlToValidate="txtLocationSite2" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trLocationSite3" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblLocationSite3" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtLocationSite3" runat="server" MapXML="LocationSite3" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqLocationSite3" MapXML="LocationSite3" runat="server"
                                                                                                                ControlToValidate="txtLocationSite3" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trLocationSite4" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblLocationSite4" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtLocationSite4" runat="server" MapXML="LocationSite4" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqLocationSite4" MapXML="LocationSite4" runat="server"
                                                                                                                ControlToValidate="txtLocationSite4" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="2">
                                                                                                            &nbsp;</td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trBidDate" runat="server">
                                                                                                        <td nowrap="NOWRAP" align="left" valign="top" class="formtext" style="height: 24px;
                                                                                                            width: 125px">
                                                                                                            <asp:Label runat="server" ID="lblBidDate" SkinID="Label"></asp:Label></td>
                                                                                                        <td nowrap="NOWRAP" valign="middle" align="left">
                                                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                    <LCtrl:LAjitTextBox ID="txtBidDate" runat="server" MapXML="BidDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBidDate" MapXML="BidDate" runat="server"
                                                                                                                        ControlToValidate="txtBidDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                             </table> 
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAwardDate" runat="server">
                                                                                                        <td nowrap="NOWRAP" align="right" valign="top" class="formtext" style="height: 24px;
                                                                                                            width: 125px">
                                                                                                            <asp:Label runat="server" ID="lblAwardDate" SkinID="Label"></asp:Label></td>
                                                                                                        <td nowrap="NOWRAP" valign="middle" align="left">
                                                                                                            <table cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                      <LCtrl:LAjitTextBox ID="txtAwardDate" runat="server" MapXML="AwardDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAwardDate" MapXML="AwardDate" runat="server"
                                                                                                                        ControlToValidate="txtAwardDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                               </tr>
                                                                                                           </table>     
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <table cellpadding="0" cellspacing="0" align="center" border="0">
                                                                                                    <tr align="left" valign="top" id="trAICPGroup5" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPGroup5" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPGroup5" runat="server" MapXML="AICPGroup5" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup5" MapXML="AICPGroup5" runat="server"
                                                                                                                ControlToValidate="txtAICPGroup5" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPGroup6" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPGroup6" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPGroup6" runat="server" MapXML="AICPGroup6" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup6" MapXML="AICPGroup6" runat="server"
                                                                                                                ControlToValidate="txtAICPGroup6" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPGroup7" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPGroup7" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPGroup7" runat="server" MapXML="AICPGroup7" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup7" MapXML="AICPGroup7" runat="server"
                                                                                                                ControlToValidate="txtAICPGroup7" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPGroup8" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPGroup8" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPGroup8" runat="server" MapXML="AICPGroup8" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup8" MapXML="AICPGroup8" runat="server"
                                                                                                                ControlToValidate="txtAICPGroup8" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPGroup9" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPGroup9" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPGroup9" runat="server" MapXML="AICPGroup9" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup9" MapXML="AICPGroup9" runat="server"
                                                                                                                ControlToValidate="txtAICPGroup9" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPMisc1" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPMisc1" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPMisc1" runat="server" MapXML="AICPMisc1" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPMisc1" MapXML="AICPMisc1" runat="server"
                                                                                                                ControlToValidate="txtAICPMisc1" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr align="left" valign="top" id="trAICPMisc2" runat="server">
                                                                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                            <asp:Label runat="server" ID="lblAICPMisc2" SkinID="Label"></asp:Label></td>
                                                                                                        <td valign="middle" style="width: 196px;">
                                                                                                            <LCtrl:LAjitTextBox ID="txtAICPMisc2" runat="server" MapXML="AICPMisc2" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAICPMisc2" MapXML="AICPMisc2" runat="server"
                                                                                                                ControlToValidate="txtAICPMisc2" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <!--Right Table End-->
                                                                                </td>
                                                                                <td>
                                                                                    <!-- spacer start -->
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                    <!-- spacer end -->
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <!-- Panel Genaral Info End -->
                                                                    <!-- Panel Product Info Start -->
                                                                    <div id="divProductInfo" style="display: none">
                                                                        <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="height: 10px;" colspan="4">
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <!--Left Table Start -->
                                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr align="left" valign="top" id="trProducerEmployee" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblProducerEmployee" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlProducerEmployee" runat="server" MapXML="ProducerEmployee"
                                                                                                    Width="184px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqProducerEmployee" MapXML="ProducerEmployee"
                                                                                                    runat="server" ControlToValidate="ddlProducerEmployee" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trArtDirectorEmployee" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblArtDirectorEmployee" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlArtDirectorEmployee" runat="server" MapXML="ArtDirectorEmployee"
                                                                                                    Width="184px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqArtDirectorEmployee" MapXML="ArtDirectorEmployee"
                                                                                                    runat="server" ControlToValidate="ddlArtDirectorEmployee" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trSetDesignerEmployeer" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblSetDesignerEmployee" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlSetDesignerEmployee" runat="server" MapXML="SetDesignerEmployee"
                                                                                                    Width="184px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSetDesignerEmployee" MapXML="SetDesignerEmployee"
                                                                                                    runat="server" ControlToValidate="ddlSetDesignerEmployee" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trEmployee" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblEmployee" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlEmployee" runat="server" MapXML="Employee" Width="184px">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqEmployee" MapXML="Employee" runat="server"
                                                                                                    ControlToValidate="ddlEmployee" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trProductOwner" runat="server" style="height: 20px">
                                                                                            <td align="right" valign="top" class="formtext" style="width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblProductOwner" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtProductOwner" runat="server" MapXML="ProductOwner" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqProductOwner" MapXML="ProductOwner" runat="server"
                                                                                                    ControlToValidate="txtProductOwner" ValidationGroup="LAJITEntryForm" ErrorMessage="*"
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAgnJobNo" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblAgnJobNo" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgnJobNo" runat="server" MapXML="AgnJobNo" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgnJobNo" MapXML="AgnJobNo" runat="server"
                                                                                                    ControlToValidate="txtAgnJobNo" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAgnPONo" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblAgnPONo" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgnPONo" runat="server" MapXML="AgnPONo" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgnPONo" MapXML="AgnPONo" runat="server"
                                                                                                    ControlToValidate="txtAgnPONo" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAgencyArtDir" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblAgencyArtDir" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyArtDir" runat="server" MapXML="AgencyArtDir" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyArtDir" MapXML="AgencyArtDir" runat="server"
                                                                                                    ControlToValidate="txtAgencyArtDir" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trContact2" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblContact2" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtContact2" runat="server" MapXML="Contact2" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContact2" MapXML="Contact2" runat="server"
                                                                                                    ControlToValidate="txtContact2" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAgencyWriter" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblAgencyWriter" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyWriter" runat="server" MapXML="AgencyWriter" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyWriter" MapXML="AgencyWriter" runat="server"
                                                                                                    ControlToValidate="txtAgencyWriter" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trContact3" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblContact3" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtContact3" runat="server" MapXML="Contact3" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContact3" MapXML="Contact3" runat="server"
                                                                                                    ControlToValidate="txtContact3" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAgencyBusMgr" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblAgencyBusMgr" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtAgencyBusMgr" runat="server" MapXML="AgencyBusMgr" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqAgencyBusMgr" MapXML="AgencyBusMgr" runat="server"
                                                                                                    ControlToValidate="txtAgencyBusMgr" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trContact4" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px;">
                                                                                                <asp:Label runat="server" ID="lblContact4" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtContact4" runat="server" MapXML="Contact4" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContact4" MapXML="Contact4" runat="server"
                                                                                                    ControlToValidate="txtContact4" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trSalesRep" runat="server" align="right" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px;" valign="top">
                                                                                                <asp:Label ID="lblSalesRep" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td align="left" valign="middle">
                                                                                                <LCtrl:LAjitDropDownList ID="ddlSalesRep" runat="server" MapXML="SalesRep" Width="184">
                                                                                                </LCtrl:LAjitDropDownList>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqSalesRep" runat="server" ControlToValidate="ddlSalesRep"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="SalesRep" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr> 
                                                                                    </table>
                                                                                    <!--Left Table End -->
                                                                                </td>
                                                                                <td valign="middle" style="width: 1px;">
                                                                                    <!-- Spacer start -->
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                    <!-- Spacer end -->
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <!--Right Table Start-->
                                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr align="left" valign="top" id="trCommercialNo" runat="server">
                                                                                            <td nowrap="NOWRAP" align="right" valign="top" class="formtext" style="height: 24px;
                                                                                                width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblCommercialNo" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtCommercialNo" runat="server" MapXML="CommercialNo" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCommercialNo" MapXML="CommercialNo" runat="server"
                                                                                                    ControlToValidate="txtCommercialNo" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trCommLength" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblCommLength" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtCommLength" runat="server" MapXML="CommLength" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCommLength" MapXML="CommLength" runat="server"
                                                                                                    ControlToValidate="txtCommLength" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trComment" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblComment" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtComment" runat="server" MapXML="Comment" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqComment" MapXML="Comment" runat="server"
                                                                                                    ControlToValidate="txtComment" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trPreProddays" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblPreProddays" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtPreProddays" runat="server" MapXML="PreProddays" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPreProddays" MapXML="PreProddays" runat="server"
                                                                                                    ControlToValidate="txtPreProddays" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trPreLightDays" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblPreLightDays" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtPreLightDays" runat="server" MapXML="PreLightDays" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPreLightDays" MapXML="PreLightDays" runat="server"
                                                                                                    ControlToValidate="txtPreLightDays" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trPreLightHours" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblPreLightHours" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtPreLightHours" runat="server" MapXML="PreLightHours" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPreLightHours" MapXML="PreLightHours" runat="server"
                                                                                                    ControlToValidate="txtPreLightHours" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr valign="middle">
                                                                                            <td align="center" colspan="2" style="height: 24px">
                                                                                                &#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;</td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trStrikeDays" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblStrikeDays" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtStrikeDays" runat="server" MapXML="StrikeDays" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqStrikeDays" MapXML="StrikeDays" runat="server"
                                                                                                    ControlToValidate="txtStrikeDays" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trStrikeHours" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblStrikeHours" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtStrikeHours" runat="server" MapXML="StrikeHours" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqStrikeHours" MapXML="StrikeHours" runat="server"
                                                                                                    ControlToValidate="txtStrikeHours" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr valign="middle">
                                                                                            <td align="center" colspan="2" style="height: 24px">
                                                                                                &#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;</td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trShootDay" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblShootDay" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtShootDay" runat="server" MapXML="ShootDay" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqShootDay" MapXML="ShootDay" runat="server"
                                                                                                    ControlToValidate="txtShootDay" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trShootHours" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblShootHours" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtShootHours" runat="server" MapXML="ShootHours" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqShootHours" MapXML="ShootHours" runat="server"
                                                                                                    ControlToValidate="txtShootHours" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr valign="middle">
                                                                                            <td align="center" colspan="2" style="height: 24px">
                                                                                                &#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;&#46;</td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trLocationDays" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblLocationDays" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtLocationDays" runat="server" MapXML="LocationDays" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqLocationDays" MapXML="LocationDays" runat="server"
                                                                                                    ControlToValidate="txtLocationDays" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trLocationHours" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblLocationHours" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtLocationHours" runat="server" MapXML="LocationHours" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqLocationHours" MapXML="LocationHours" runat="server"
                                                                                                    ControlToValidate="txtLocationHours" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
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
                                                                    <!-- Panel Product Info End -->
                                                                    <!-- Panel Amount Details Start  style="height:368px" -->
                                                                    <div id="divAmountDetails" style="display: none">
                                                                        <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td style="height: 10px;" colspan="4">
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <!--Left Table Start -->
                                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr align="left" valign="top" id="trPostProdCompany" runat="server">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblPostProdCompany" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtPostProdCompany" runat="server" MapXML="PostProdCompany"
                                                                                                    Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqPostProdCompany" MapXML="PostProdCompany"
                                                                                                    runat="server" ControlToValidate="txtPostProdCompany" ValidationGroup="LAJITEntryForm"
                                                                                                    ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True"
                                                                                                    Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trDubbingHouse" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblDubbingHouse" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtDubbingHouse" runat="server" MapXML="DubbingHouse" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqDubbingHouse" MapXML="DubbingHouse" runat="server"
                                                                                                    ControlToValidate="txtDubbingHouse" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trStorageHouse" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblStorageHouse" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtStorageHouse" runat="server" MapXML="StorageHouse" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqStorageHouse" MapXML="StorageHouse" runat="server"
                                                                                                    ControlToValidate="txtStorageHouse" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trShootDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblShootDate" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <table cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <LCtrl:LAjitTextBox ID="txtShootDate" runat="server" MapXML="ShootDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                           <LCtrl:LAjitRequiredFieldValidator ID="reqShootDate" MapXML="ShootDate" runat="server"
                                                                                                            ControlToValidate="txtShootDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trRoughCutDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblRoughCutDate" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                               <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr>
                                                                                                         <td>
                                                                                                            <LCtrl:LAjitTextBox ID="txtRoughCutDate" runat="server" MapXML="RoughCutDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                          </td>
                                                                                                         <td>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqRoughCutDate" MapXML="RoughCutDate" runat="server"
                                                                                                                ControlToValidate="txtRoughCutDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                         </td>
                                                                                                    </tr>
                                                                                                </table>  
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trWrapDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblWrapDate" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                               <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <tr>
                                                                                                         <td>
                                                                                                           <LCtrl:LAjitTextBox ID="txtWrapDate" runat="server" MapXML="WrapDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqWrapDate" MapXML="WrapDate" runat="server"
                                                                                                            ControlToValidate="txtWrapDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table> 
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trAirDate" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblAirDate" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <LCtrl:LAjitTextBox ID="txtAirDate" runat="server" MapXML="AirDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                        </td> 
                                                                                                        <td>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqAirDate" MapXML="AirDate" runat="server"
                                                                                                                ControlToValidate="txtAirDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trLockedBudget" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 141px;">
                                                                                                <asp:Label runat="server" ID="lblLockedBudget" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitCheckBox ID="chkLockedBudget" runat="server" MapXML="LockedBudget" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trRDARevenue" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblRDARevenue" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtRDARevenue" runat="server" MapXML="RDARevenue" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqRDARevenue" runat="server" ControlToValidate="txtRDARevenue"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="RDARevenue" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trMarkUpTotal" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblMarkUpTotal" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtMarkUpTotal" runat="server" MapXML="MarkUpTotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqMarkUpTotal" runat="server" ControlToValidate="txtMarkUpTotal"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="MarkUpTotal" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trMarkUpPercent" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblMarkUpPercent" runat="server" SkinID="Label"></asp:Label></td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitTextBox ID="txtMarkUpPercent" runat="server" MapXML="MarkUpPercent" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqMarkUpPercent" runat="server" ControlToValidate="txtMarkUpPercent"
                                                                                                    Enabled="false" ErrorMessage="Value is required." MapXML="MarkUpPercent" SetFocusOnError="True"
                                                                                                    ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trSepFringeAcct" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblSepFringeAcct" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitCheckBox ID="chkSepFringeAcct" runat="server" MapXML="SepFringeAcct" />
                                                                                            </td>
                                                                                        </tr> 
                                                                                        <tr id="trWrapIns" runat="server" align="left" valign="top">
                                                                                            <td align="right" class="formtext" style="height: 24px; width: 141px;" valign="top">
                                                                                                <asp:Label ID="lblWrapIns" runat="server" SkinID="Label"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 196px;" valign="middle">
                                                                                                <LCtrl:LAjitCheckBox ID="chkWrapIns" runat="server" MapXML="WrapIns" />
                                                                                            </td>
                                                                                        </tr>

                                                                                    </table>
                                                                                    <!--Left Table End -->
                                                                                </td>
                                                                                <td valign="middle" style="width: 1px;">
                                                                                    <!-- Spacer start -->
                                                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1">
                                                                                    <!-- Spacer end -->
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <!--Right Table Start-->
                                                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                                                        <tr align="left" valign="top" id="trCGITotal" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblCGITotal" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                <LCtrl:LAjitTextBox ID="txtCGITotal" runat="server" MapXML="CGITotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqCGITotal" MapXML="CGITotal" runat="server"
                                                                                                    ControlToValidate="txtCGITotal" ValidationGroup="LAJITEntryForm" ErrorMessage="*"
                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trProjectTotal" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblProjectTotal" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="middle" style="width: 196px;">                                                                                               
                                                                                               <LCtrl:LAjitTextBox ID="txtProjectTotal" runat="server" MapXML="ProjectTotal" Width="180px"></LCtrl:LAjitTextBox>                                                                                                
                                                                                               <LCtrl:LAjitRequiredFieldValidator ID="reqProjectTotal" MapXML="ProjectTotal" runat="server"
                                                                                                ControlToValidate="txtProjectTotal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="left" valign="top" id="trCostPlus" runat="server">
                                                                                            <td align="right" valign="top" class="formtext" style="height: 24px; width: 125px;">
                                                                                                <asp:Label runat="server" ID="lblCostPlus" SkinID="Label"></asp:Label></td>
                                                                                            <td valign="top">
                                                                                                <LCtrl:LAjitCheckBox ID="chkCostPlus" runat="server" MapXML="CostPlus" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>                                                                                                                                                                                    
                                                                                            <td colspan="2" align="right">
                                                                                               <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="JobPO" />
                                                                                            </td>                                                                                        
                                                                                        </tr>
                                                                                        <tr align="left" valign="top">
                                                                                            <td align="right" valign="middle" class="formtext" style="height: 24px; width: 125px;">
                                                                                                &nbsp;</td>
                                                                                            <td valign="middle" style="width: 196px;">
                                                                                                &nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                &nbsp;</td>
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
                                                        <!-- form entry fields end -->
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="0" width="100%" align="center">
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
             </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
