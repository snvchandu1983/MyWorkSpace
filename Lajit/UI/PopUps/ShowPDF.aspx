<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ShowPDF.aspx.cs" Inherits="LAjitDev.PopUps.ShowPDF"
    Theme="LAjit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Show PDF</title>
</head>

<%--<script type="text/javascript" src="../JavaScript/Common.js"></script>--%>

<body onload="HideParentProgress();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="pnlPrintScriptMgr" runat="server" EnablePartialRendering="true">
            <Scripts>
            </Scripts>
        </asp:ScriptManager>
        <asp:Panel runat="server" ID="pnlTotalPrint" Visible="false">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <!--Label Message-->
                <asp:Panel runat="server" ID="pnllblMsg" Visible="true">
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
    </form>
</body>
</html>
