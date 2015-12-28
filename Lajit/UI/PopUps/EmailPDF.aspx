<%@ Page AutoEventWireup="true" Codebehind="EmailPDF.aspx.cs" Inherits="LAjitDev.PopUps.EmailPDF"
    Language="C#" Theme="LAjit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email PDF</title>
    <script type="text/javascript">
        function DoSessionExpire() {
            //This is a dummy function. 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="pnlPrintScriptMgr" runat="server" EnablePartialRendering="true">
            <scripts>
            </scripts>
        </asp:ScriptManager>
        <asp:Panel ID="pnlTotalPrint" runat="server" Visible="false">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <!--Label Message-->
                <asp:Panel ID="pnllblMsg" runat="server" Visible="true">
                    <tr valign="top">
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr id="trMsg" align="left" valign="top">
                        <td align="center" valign="top">
                            <asp:Label ID="lblMsg" runat="server" CssClass="msummary"></asp:Label>
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlEmailing" runat="server" Visible="false">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 815px; height: 70px;">
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr id="trEmailing" runat="server" align="left" valign="middle">
                    <td class="formtext" style="height: 24px; width: 260px">
                        &nbsp;
                    </td>
                    <td valign="middle">
                        <table border="0" cellpadding="0" cellspacing="0" class="formtext" style="width: 100%;
                            height: 100%; border-right-width: 1px; border-right-style: solid; border-right-color: #d7e0f1;
                            border-left-width: 1px; border-left-style: solid; border-left-color: #d7e0f1;
                            border-top-width: 1px; border-top-style: solid; border-top-color: #d7e0f1; border-bottom-width: 1px;
                            border-bottom-style: solid; border-bottom-color: #d7e0f1;">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label1" runat="server" SkinID="Label"><b>Please Enter Email IDS to send
                                        report </b></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="formtext" style="height: 50px; width: 120px">
                                    <asp:Label ID="lblEmailIDS" runat="server" SkinID="Label">Email IDS</asp:Label>
                                </td>
                                <td valign="middle" width="330px">
                                    <LCtrl:LAjitTextBox ID="txtEmailing" runat="server" TextMode="MultiLine" Width="190px"></LCtrl:LAjitTextBox>
                                    <LCtrl:LAjitRegularExpressionValidator ID="regEmailing" runat="server" ControlToValidate="txtEmailing"
                                        EnableClientScript="true" ErrorMessage="Please Enter Valid Email ids" ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$"
                                        ValidationGroup="LAJITEntryForm">
                                    </LCtrl:LAjitRegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblNote" runat="server" SkinID="Label"><b>Note : Please Enter Multiple
                                        Email IDS with ';' ending for each of the email id </b></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="height: 24px; width: 120px" valign="middle">
                                    <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" AlternateText="Submit" CausesValidation="false"
                                        Height="18" OnClick="imgbtnSubmit_Click" ValidationGroup="LAJITEntryForm" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
     <%--   <asp:Literal ID="littxt" runat="server"></asp:Literal>--%>
    </form>
</body>
</html>
