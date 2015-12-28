<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Login.aspx.cs" Inherits="LAjitDev.Login"
    Theme="LAjit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<link rel="shortcut icon" type="image/x-icon" href="../App_Themes/LAjit/Images/favicon.ico" />--%>
    <title>LAjit Technologies, LLC</title>
    <script type="text/javascript">
        window.onload=function(){
            if(window.localStorage)
                window.localStorage.clear();
        }
    </script>
</head>
<body class="loginbg">
    <form id="Form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="z-index: 100;
            left: 0px; position: absolute; top: 0px">
            <tr>
                <td align="center" valign="top" style="width: 100%">
                    <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
                        <tr>
                            <td align="left" valign="top" style="height: 149px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" style="height: 229px;">
                                <!-- login details start -->
                                <table border="0" cellspacing="0" cellpadding="0" style="width: 565px; height: 229px;">
                                    <tr align="left">
                                        <td style="width: 317px;" class="loginmainbg">
                                        </td>
                                        <td style="width: 12px;" class="loginmid">
                                        </td>
                                        <td style="width: 12px;" class="loginmid">
                                        </td>
                                        <td class="loginmid" valign="top">
                                            <table border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left" valign="top" style="height: 25px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height: 35px;" class="memberlgn">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" class="filedstext" style="height: 17px;">
                                                        Username:</td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height: 29px;">
                                                        <asp:TextBox ID="txtUserName" runat="server" SkinID="LoginTextBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName"
                                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="LAJITLogin"
                                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" class="filedstext" style="height: 17px;">
                                                        Password:</td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height: 29px;">
                                                        <asp:TextBox ID="txtPassword" runat="server" SkinID="LoginTextBox" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LAJITLogin"
                                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table width="97%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr valign="middle" align="left">
                                                                <td align="left" style="width: 20px;">
                                                                    <input type="checkbox" name="checkbox" value="checkbox" /></td>
                                                                <td class="filedstext">
                                                                    <strong>Remember me</strong></td>
                                                                <td align="right">
                                                                    <asp:ImageButton ID="login_but" runat="server"  OnClick="login_but_Click"
                                                                        ValidationGroup="LAJITLogin" BorderStyle="None" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblError" runat="server" CssClass="msummary"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height: 19px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <a href="#" class="blacklinks">Forgot Password?</a></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 7px;" class="loginrght">
                                        </td>
                                    </tr>
                                </table>
                                <!-- login details start -->
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="hdnvarRows" type="hidden" runat="server" name="hdnvarRows" /></td>
                            <td align="left" valign="top" style="height: 200px;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
