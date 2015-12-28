<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Profit.aspx.cs" Inherits="LAjitDev.Financials.Profit" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">

    <script language="javascript" type="text/javascript">
    
    function HideShowDiv(divname)
    {
        switch (divname)
        {
            case "GeneralInfo":
                //Content
                document.getElementById("divGeneralInfo").style.display="block";
                document.getElementById("divVariables").style.display="none";
                //Tabs  
                document.getElementById("divGeneralInfoTab").style.display="block";
                document.getElementById("divVariablesTab").style.display="none";
                break;
            case "Variables":
                //Content
                document.getElementById("divGeneralInfo").style.display="none";
                document.getElementById("divVariables").style.display="block"; 
                //Tabs
                document.getElementById("divGeneralInfoTab").style.display="none";
                document.getElementById("divVariablesTab").style.display="block"; 
                break;              
            }
    }
    </script>
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="600px" Style="background-color: White;" Width="100%">
                                            <!-- entry form start -->
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 521px" width="100%">
                                                <!--Pop up header tr-->
                                                <tr id="trPopupHeader" runat="server" style="height: 24px;" valign="top" visible="false">
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 24px; cursor: hand;
                                                            border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;"
                                                            width="100%">
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
                                                <tr id="trProcessLinks" runat="server" align="right" style="height: 10px;" valign="middle">
                                                    <td id="tdProcessLinks" runat="server" align="right" valign="middle">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                        </asp:Panel>
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
                                                <tr style="height: 25px;">
                                                    <td valign="bottom">
                                                        <div id="divGeneralInfoTab" style="display: block;">
                                                            <table align="left" bgcolor="#ffffff" border="0" cellpadding="0" cellspacing="0"
                                                                width="45%">
                                                                <tr align="left" valign="bottom">
                                                                    <td style="width: 5px;">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif"
                                                                            width="5"></td>
                                                                    <td align="center" class="bluebold" style="width: 85px; background-color: #c5d5fc;"
                                                                        valign="middle">
                                                                        General Info</td>
                                                                    <td align="center" style="width: 5px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif"
                                                                            width="5"></td>
                                                                    <td align="center" style="width: 3px;" valign="middle">
                                                                        <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                    <td align="center" style="width: 1px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif"
                                                                            width="5"></td>
                                                                    <td align="center" class="blueboldlinks01" onclick="javascript:HideShowDiv('Variables');"
                                                                        style="width: 80px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;"
                                                                        valign="middle">
                                                                        Variables
                                                                    </td>
                                                                    <td align="center" style="width: 6px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif"
                                                                            width="6"></td>
                                                                    <td align="center" style="width: 3px;" valign="middle">
                                                                        <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divVariablesTab" style="display: none">
                                                            <table align="left" bgcolor="#ffffff" border="0" cellpadding="0" cellspacing="0"
                                                                width="45%">
                                                                <tr align="left" valign="bottom">
                                                                    <td align="center" style="width: 5px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/top-left01.gif"
                                                                            width="5"></td>
                                                                    <td align="center" class="blueboldlinks01" onclick="javascript:HideShowDiv('GeneralInfo');"
                                                                        style="width: 85px; background: url(<%=Application["ImagesCDNPath"]%>Images/top-midbg01.gif) repeat-x;"
                                                                        valign="middle">
                                                                        General Info
                                                                    </td>
                                                                    <td align="center" style="width: 6px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/top-right01.gif"
                                                                            width="6"></td>
                                                                    <td align="center" style="width: 3px;" valign="middle">
                                                                        <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                    <td style="width: 5px;">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif"
                                                                            width="5"></td>
                                                                    <td align="center" class="bluebold" style="width: 80px; background-color: #c5d5fc;"
                                                                        valign="middle">
                                                                        Variables
                                                                    </td>
                                                                    <td align="center" style="width: 5px;" valign="middle">
                                                                        <img height="21" src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif"
                                                                            width="5"></td>
                                                                    <td align="center" style="width: 3px;" valign="middle">
                                                                        <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
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
                                                    <td align="left" valign="top" colspan="2">
                                                        <!-- form entry fields start -->
                                                        <table border="0" cellpadding="0" cellspacing="0" class="formmiddle" style="height: 460px"
                                                            width="100%">
                                                            <tr style="border-style: none;">
                                                                <td colspan="5" valign="top">
                                                                    <!-- Panel General Info Start -->
                                                                    <div id="divGeneralInfo" style="display: block;">
                                                                        <table border="0" bordercolor="red" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td colspan="4" style="height: 10px;">
                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td colspan="2">
                                                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                                <tr id="trTypeOfJob" valign="middle" runat="server">
                                                                                                                    <td class="formtext" style="height: 24px; width: 160px">
                                                                                                                        <asp:Label runat="server" ID="lblTypeOfJob" SkinID="Label"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td valign="middle">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlTypeOfJob" runat="server" MapXML="TypeOfJob" Width="456px">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqTypeOfJob" MapXML="TypeOfJob" runat="server"
                                                                                                                            ControlToValidate="ddlTypeOfJob" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
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
                                                                                            <td colspan="4">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td colspan="2">
                                                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                                <tr id="trSelectDirVendor" runat="server" valign="middle">
                                                                                                                    <td class="formtext" style="height: 24px; width: 160px">
                                                                                                                        <asp:Label ID="lblSelectDirVendor" runat="server" SkinID="Label"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td valign="middle">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectDirVendor" runat="server" MapXML="SelectDirVendor"
                                                                                                                            Width="456px">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectDirVendor" runat="server" ControlToValidate="ddlSelectDirVendor"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectDirVendor" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4" style="height: 24px;">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr id="trSelectFrequency" runat="server" valign="middle">
                                                                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                                                                            <asp:Label ID="lblSelectFrequency" runat="server" SkinID="Label"></asp:Label>
                                                                                                        </td>
                                                                                                        <td valign="middle">
                                                                                                            <LCtrl:LAjitDropDownList ID="ddlSelectFrequency" runat="server" MapXML="SelectFrequency"
                                                                                                                Width="456px">
                                                                                                            </LCtrl:LAjitDropDownList>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqSelectFrequency" runat="server" ControlToValidate="ddlSelectFrequency"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="SelectFrequency" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4" style="height: 24px;">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr id="trProfitCalc" runat="server" valign="middle">
                                                                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                                                                            <asp:Label ID="lblProfitCalc" runat="server" SkinID="Label"></asp:Label>
                                                                                                        </td>
                                                                                                        <td valign="middle">
                                                                                                            <LCtrl:LAjitDropDownList ID="ddlProfitCalc" runat="server" MapXML="ProfitCalc" Width="456px">
                                                                                                            </LCtrl:LAjitDropDownList>
                                                                                                            <LCtrl:LAjitRequiredFieldValidator ID="reqProfitCalc" runat="server" ControlToValidate="ddlProfitCalc"
                                                                                                                Enabled="false" ErrorMessage="Value is required." MapXML="ProfitCalc" SetFocusOnError="True"
                                                                                                                ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                &nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td colspan="2" align="left" style="width: 50%">
                                                                                                            <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0">
                                                                                                                <tr align="right" valign="top" id="trContAniv" runat="server">
                                                                                                                    <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                                                                        <asp:Label runat="server" ID="lblContAniv" SkinID="Label"></asp:Label></td>
                                                                                                                    <td valign="middle" align="left">
                                                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <LCtrl:LAjitTextBox ID="txtContAniv" runat="server" MapXML="ContAniv" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <LCtrl:LAjitRequiredFieldValidator ID="reqContAniv" MapXML="ContAniv" runat="server"
                                                                                                                                ControlToValidate="txtContAniv" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                                ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2" style="width: 50%" valign="middle">
                                                                                                            <table width="100%" border="0" bordercolor="green" cellspacing="0" cellpadding="0"
                                                                                                                align="left">
                                                                                                                <tr align="right" valign="top" id="trEffectiveDate" runat="server">
                                                                                                                    <td align="left" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                                                                                        <asp:Label runat="server" ID="lblEffectiveDate" SkinID="Label"></asp:Label></td>
                                                                                                                    <td style="width: 120px;" valign="middle" align="left">
                                                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtEffectiveDate" runat="server" MapXML="EffectiveDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqEffectiveDate" MapXML="EffectiveDate" runat="server"
                                                                                                                                    ControlToValidate="txtEffectiveDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                                                                    ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>                                                                                      
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left" colspan="2" style="width: 50%">
                                                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                                <tr id="trOverheadPct">
                                                                                                                    <td class="formtext" style="height: 24px; width: 160px">
                                                                                                                        <asp:Label ID="lblOverheadPct" runat="server" SkinID="Label"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtOverheadPct" runat="server" MapXML="OverheadPct" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqOverheadPct" runat="server" ControlToValidate="txtOverheadPct"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="OverheadPct" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2" style="width: 50%" valign="middle">
                                                                                                            <table align="left" border="0" bordercolor="green" cellpadding="0" cellspacing="1"
                                                                                                                width="100%">
                                                                                                                <tr id="trProfitThreshold" runat="server" align="left" valign="top">
                                                                                                                    <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                        <asp:Label ID="lblProfitThreshold" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                    <td style="width: 120px;" valign="middle" align="left">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtProfitThreshold" runat="server" MapXML="ProfitThreshold"
                                                                                                                            Style="vertical-align: middle" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqProfitThreshold" runat="server" ControlToValidate="txtProfitThreshold"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="ProfitThreshold" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left" colspan="2" style="width: 50%">
                                                                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                                <tr id="trExcludeNonProift">
                                                                                                                    <td class="formtext" style="height: 24px; width: 160px" align="right">
                                                                                                                        <asp:Label ID="lblExcludeNonProift" runat="server" SkinID="Label" Style="vertical-align: middle"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <LCtrl:LAjitCheckBox ID="chkExcludeNonProift" runat="server" MapXML="ExcludeNonProift"
                                                                                                                            Style="vertical-align: middle" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td align="right" colspan="2" style="width: 50%" valign="middle">
                                                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                                <tr id="trSelectProfitThresh" runat="server" align="left" valign="top">
                                                                                                                    <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                        <asp:Label ID="lblSelectProfitThresh" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                    <td style="width: 120px" valign="bottom" align="left">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitThresh" runat="server" MapXML="SelectProfitThresh"
                                                                                                                            Width="105px">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitThresh" runat="server" ControlToValidate="ddlSelectProfitThresh"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitThresh"
                                                                                                                            SetFocusOnError="True" ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" style="height: 5px;">
                                                                                                <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4" style="width: 100%">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 6%">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="4" style="width: 90px" valign="top">
                                                                                                            <asp:Label ID="lblSalesRulesHeading" runat="server" SkinID="LabelBig"><u>Profit Steps :</u></asp:Label></td>
                                                                                                        <td align="left" colspan="4" style="width: 280px" valign="top">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 9%">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" colspan="4">
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left" colspan="4" style="width: 50%" valign="top">
                                                                                                            <!--Left Table Start -->
                                                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td colspan="2">
                                                                                                                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                                            <tr>
                                                                                                                                <td colspan="2" style="height: 10px;">
                                                                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trLimit1" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblLimit1" runat="server" SkinID="Label"></asp:Label>
                                                                                                                                </td>
                                                                                                                                <td style="width: 120px" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtLimit1" runat="server" MapXML="Limit1" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLimit1" runat="server" ControlToValidate="txtLimit1"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="Limit1" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trProfPct1" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblProfPct1" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtProfPct1" runat="server" MapXML="ProfPct1" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProfPct1" runat="server" ControlToValidate="txtProfPct1"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="ProfPct1" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trUnderBudgetPct1" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblUnderBudgetPct1" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtUnderBudgetPct1" runat="server" MapXML="UnderBudgetPct1"
                                                                                                                                        Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqUnderBudgetPct1" runat="server" ControlToValidate="txtUnderBudgetPct1"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="UnderBudgetPct1" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td colspan="2" style="height: 10px;">
                                                                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trLimit2" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblLimit2" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtLimit2" runat="server" MapXML="Limit2" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLimit2" runat="server" ControlToValidate="txtLimit2"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="Limit2" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trProfPct2" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblProfPct2" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtProfPct2" runat="server" MapXML="ProfPct2" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProfPct2" runat="server" ControlToValidate="txtProfPct2"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="ProfPct2" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trUnderBudgetPct2" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblUnderBudgetPct2" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtUnderBudgetPct2" runat="server" MapXML="UnderBudgetPct2"
                                                                                                                                        Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqUnderBudgetPct2" runat="server" ControlToValidate="txtUnderBudgetPct2"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="UnderBudgetPct2" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                            <!--Left Table End -->
                                                                                                        </td>
                                                                                                        <td align="right" colspan="4" style="width: 50%" valign="top">
                                                                                                            <!--Right Table Start-->
                                                                                                            <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                                <tr valign="top">
                                                                                                                    <td colspan="4" style="width: 100%;" valign="middle">
                                                                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" style="height: 100%;
                                                                                                                            width: 100%">
                                                                                                                            <tr>
                                                                                                                                <td colspan="2" style="height: 10px;">
                                                                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trLimit3" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblLimit3" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtLimit3" runat="server" MapXML="Limit3" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLimit3" runat="server" ControlToValidate="txtLimit3"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="Limit3" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trProfPct3" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblProfPct3" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtProfPct3" runat="server" MapXML="ProfPct3" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProfPct3" runat="server" ControlToValidate="txtProfPct3"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="ProfPct3" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trUnderBudgetPct3" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblUnderBudgetPct3" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtUnderBudgetPct3" runat="server" MapXML="UnderBudgetPct3"
                                                                                                                                        Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqUnderBudgetPct3" runat="server" ControlToValidate="txtUnderBudgetPct3"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="UnderBudgetPct3" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td colspan="2" style="height: 10px;">
                                                                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1">
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trLimit4" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblLimit4" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtLimit4" runat="server" MapXML="Limit4" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqLimit4" runat="server" ControlToValidate="txtLimit4"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="Limit4" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trProfPct4" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblProfPct4" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtProfPct4" runat="server" MapXML="ProfPct4" Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProfPct4" runat="server" ControlToValidate="txtProfPct4"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="ProfPct4" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr id="trUnderBudgetPct4" runat="server" align="left" valign="top">
                                                                                                                                <td align="right" class="formtext" style="height: 24px; width: 160px;" valign="top">
                                                                                                                                    <asp:Label ID="lblUnderBudgetPct4" runat="server" SkinID="Label"></asp:Label></td>
                                                                                                                                <td style="width: 120px;" valign="middle">
                                                                                                                                    <LCtrl:LAjitTextBox ID="txtUnderBudgetPct4" runat="server" MapXML="UnderBudgetPct4"
                                                                                                                                        Width="100px"></LCtrl:LAjitTextBox>
                                                                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqUnderBudgetPct4" runat="server" ControlToValidate="txtUnderBudgetPct4"
                                                                                                                                        Enabled="false" ErrorMessage="Value is required." MapXML="UnderBudgetPct4" SetFocusOnError="True"
                                                                                                                                        ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                 </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <!-- Panel Director Profit Start-->
                                                                    <div id="divVariables" style="display: none">
                                                                        <table border="0" bordercolor="green" cellpadding="0" cellspacing="0" width="100%"
                                                                            height="300 px">
                                                                            <tr>
                                                                                <td colspan="4" style="height: 10px;">
                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 85%">
                                                                                        <tr valign="top">
                                                                                            <td colspan="2" style="width: 70%;" valign="middle">
                                                                                                <table align="center" border="0" cellpadding="0" cellspacing="0" style="height: 100%;
                                                                                                    width: 100%">
                                                                                                    <tr>
                                                                                                        <td colspan="4" style="height: 10px;">
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="2">
                                                                                                            <table align="center" border="0" cellpadding="0" cellspacing="0" style="height: 100%;
                                                                                                                width: 100%">
                                                                                                                <tr>
                                                                                                                    <td width="25px">
                                                                                                                    </td>
                                                                                                                    <td align="left" valign="middle">
                                                                                                                        <asp:Label ID="lblOtherVariablesHeading" runat="server" SkinID="LabelBig"><u>Profit Variables:</u></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="4" style="height: 10px;">
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                                                    </tr>
                                                                                                    <!--Row Header-->
                                                                                                    <tr>
                                                                                                        <td colspan="5">
                                                                                                            <table align="center" border="0" cellpadding="0" cellspacing="0" style="height: 100%;
                                                                                                                width: 75%;">
                                                                                                                <tr align="left" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <asp:Label ID="lblUse" runat="server" SkinID="Label" Text="Estimate Profit" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="top">
                                                                                                                        <asp:Label ID="lblFlatAmount" runat="server" SkinID="Label" Text="Flat Amount" Width="95%"></asp:Label></td>
                                                                                                                </tr>
                                                                                                                <!--Row1-->
                                                                                                                <tr id="trFlatAtoK" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblFlatAtoK" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt6" runat="server" MapXML="SelectProfitAmt6"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt6" runat="server" ControlToValidate="ddlSelectProfitAmt6"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt6" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtFlatAtoK" runat="server" MapXML="FlatAtoK" Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqFlatAtoK" runat="server" ControlToValidate="txtFlatAtoK"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="FlatAtoK" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row2-->
                                                                                                                <tr id="trAtoKoverage" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblAtoKoverage" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt1" runat="server" MapXML="SelectProfitAmt1"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt1" runat="server" ControlToValidate="ddlSelectProfitAmt1"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt1" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtAtoKoverage" runat="server" MapXML="AtoKoverage" Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAtoKoverage" runat="server" ControlToValidate="txtAtoKoverage"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="AtoKoverage" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row3-->
                                                                                                                <tr id="trProdExpEst" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblProdExpEst" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt3" runat="server" MapXML="SelectProfitAmt3"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt3" runat="server" ControlToValidate="ddlSelectProfitAmt3"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt3" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtProdExpEst" runat="server" MapXML="ProdExpEst" Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqProdExpEst" runat="server" ControlToValidate="txtProdExpEst"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="ProdExpEst" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row4-->
                                                                                                                <tr id="trProdFeeOverage" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblProdFeeOverage" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt2" runat="server" MapXML="SelectProfitAmt2"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt2" runat="server" ControlToValidate="ddlSelectProfitAmt2"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt2" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtProdFeeOverage" runat="server" MapXML="ProdFeeOverage"
                                                                                                                            Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqProdFeeOverage" runat="server" ControlToValidate="txtProdFeeOverage"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="ProdFeeOverage" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row5-->
                                                                                                                <tr id="trFlatProductionFee" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblFlatProductionFee" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt4" runat="server" MapXML="SelectProfitAmt4"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt4" runat="server" ControlToValidate="ddlSelectProfitAmt4"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt4" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtFlatProductionFee" runat="server" MapXML="FlatProductionFee"
                                                                                                                            Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqFlatProductionFee" runat="server" ControlToValidate="txtFlatProductionFee"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="FlatProductionFee"
                                                                                                                            SetFocusOnError="True" ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row6-->
                                                                                                                <tr id="trFlatDirFee" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblFlatDirFee" runat="server" SkinID="Label" Width="100%"></asp:Label></td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt5" runat="server" MapXML="SelectProfitAmt5"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt5" runat="server" ControlToValidate="ddlSelectProfitAmt5"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt5" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtFlatDirFee" runat="server" MapXML="FlatDirFee" Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqFlatDirFee" runat="server" ControlToValidate="txtFlatDirFee"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="FlatDirFee" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row7-->
                                                                                                                <tr id="trRevEst" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 10px; width: 35%">
                                                                                                                        <asp:Label ID="lblRevEst" runat="server" SkinID="Label" Width="100%"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt7" runat="server" MapXML="SelectProfitAmt7"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt7" runat="server" ControlToValidate="ddlSelectProfitAmt7"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt3" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtRevEst" runat="server" MapXML="RevEst" Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqRevEst" runat="server" ControlToValidate="txtRevEst"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="RevEst" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <!--Row8-->
                                                                                                                <tr id="trOverheadExpense" runat="server" valign="top">
                                                                                                                    <td class="formtext" style="height: 24px; width: 35%">
                                                                                                                        <asp:Label ID="lblOverheadExpense" runat="server" SkinID="Label" Width="100%"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 40%" valign="top">
                                                                                                                        <LCtrl:LAjitDropDownList ID="ddlSelectProfitAmt8" runat="server" MapXML="SelectProfitAmt8"
                                                                                                                            Width="90%">
                                                                                                                        </LCtrl:LAjitDropDownList>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqSelectProfitAmt8" runat="server" ControlToValidate="ddlSelectProfitAmt8"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="SelectProfitAmt3" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                    <td align="center" style="height: 24px; width: 25%" valign="middle">
                                                                                                                        <LCtrl:LAjitTextBox ID="txtOverheadExpense" runat="server" MapXML="OverheadExpense"
                                                                                                                            Width="90%"></LCtrl:LAjitTextBox>
                                                                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqOverheadExpense" runat="server" ControlToValidate="txtOverheadExpense"
                                                                                                                            Enabled="false" ErrorMessage="Value is required." MapXML="OverheadExpense" SetFocusOnError="True"
                                                                                                                            ToolTip="Value is required." ValidationGroup="LAJITEntryForm"></LCtrl:LAjitRequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="4" style="height: 10px;">
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="4" style="height: 10px;">
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="4" style="height: 10px;">
                                                                                                            <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <!--Right Table End-->
                                                                                </td>
                                                                                <td colspan="4" style="height: 5px;">
                                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr id="trNotePct">
                                                                <td align="Left" colspan="4" style="height: 7px;">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <td width="175px" colspan="2">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="center" colspan="2">
                                                                            <asp:Label ID="lblNotePct" runat="server" SkinID="Label">Note : Enter percentages as decimal numbers: Twenty-Five and a half percent = 25.5</asp:Label>
                                                                        </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="height: 2px;">
                                                                    <img height="1" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"></td>
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
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                    <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                    </asp:Timer>
                    <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                        <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
                            <tr align="center" class="formmiddle">
                                <td>
                                    <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label>
                                 </td>
                            </tr>
                        </table>
                    </asp:Panel>                  
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
