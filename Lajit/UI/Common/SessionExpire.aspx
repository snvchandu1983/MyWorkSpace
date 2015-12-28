<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SessionExpire.aspx.cs"
    Inherits="LAjitDev.Common.SessionExpire" Theme="Lajit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LAjit Technologies, LLC</title>
</head>
<body class="loginbg">
    <form id="frmSessionExpire" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top">
                    <table border="0" cellspacing="0" cellpadding="0" style="width: 565px">
                        <tr>
                            <td align="left" valign="top" style="height:150px;">
                                <img src="<%=Application["ImagesCDNPath"]%>images/spacer.gif" width="1" height="1"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" style="height: 229px;">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 229px; width: 565px">
                                    <tr>
                                        <td style="height: 5px; width: 565px" class="errortop">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="errorbg" style="height: 228px; width: 565px">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="center" valign="top" style="height: 47px;">
                                                        <img src="<%=Application["ImagesCDNPath"]%>images/spacer.gif" width="1" height="1"></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" style="height: 83px; width: 68px; background-position: center" class="errorimg">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top" class="errortext-big">
                                                        Your current session has been timed out.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top" class="errortext-small">
                                                        Click the link below to go to the Login Page.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top" class="errortext-small">
                                                        <asp:LinkButton ID="btnLogOut" Text="Exit" runat="server" OnClick="btnLogOut_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px; width: 565px" class="errorbot">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
