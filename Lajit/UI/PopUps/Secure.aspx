<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Secure.aspx.cs" Inherits="LAjitDev.PopUps.Secure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="LAjitControls" Namespace="LAjitControls" TagPrefix="LCtrl" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lajit :: Secure</title>
    
<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>

    <script type="text/javascript" src="../JavaScript/jquery.dialog.js"></script>

    <script type="text/javascript" src="../JavaScript/Utility.js"></script>

    <script type="text/javascript" src="../JavaScript/Common.js"></script>--%>

    <script type="text/javascript" language="javascript">
    function $get(eId) {
        return document.getElementById(eId);
    }
 
    function SecureAdd()
    {
      $get("hdnFldSecureAction").value="Add";
      $get("ddlSecureCategory").disabled=false;
      $get("ddlSecureCategory").selectedIndex=0;
      $get("divSubmit").style.display="block";
      return false;
    }
    function SecureModify()
    {
        $get("ddlSecureCategory").disabled=false;
        $get("ddlSecureCategory").style.backgroundColor="LightGoldenrodYellow";
        $get("hdnFldSecureAction").value="Modify";
        $get("divSubmit").style.display="block";
        return false;
    }
    function SecureDelete()
    {
       return confirm('Are you sure you want to delete?');
    }
    
    function HideParentProgress()
    {
        var divLayer = parent.$get('layer');
        var divImage = parent.$get('box');
        if(divLayer)
        {
            parent.document.body.removeChild(divLayer);
        }
        if(divImage)
        {
            parent.document.body.removeChild(divImage);
        }
    }  
      
       
    </script>

</head>
<body onload="HideParentProgress();">
    <form id="form1" runat="server">
        <div id="divSecure">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr valign="top">
                    <td colspan="4">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                            cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                            <tr style="height: 24px;">
                                <td align="center" class="grdVwRPTitle" style="height: 24px; border-width: 0px">
                                    <asp:Label ID="lblPopupEntry" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="2px" colspan="4">
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#ffffff" valign="top">
                        &nbsp;</td>
                    <td width="45px" valign="top">
                        <!-- left side icons start  -->
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr valign="top">
                                <td valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle"
                                        style="height: 200px;">
                                        <tr valign="top">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr id="trAdd" runat="server" style="height: 60px;" valign="top">
                                                        <td align="center">
                                                            <asp:ImageButton runat="server" ID="imgbtnSecureAdd" AlternateText="Add" OnClientClick="javascript:return SecureAdd();" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trModify" runat="server" style="height: 60px;" valign="top">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnSecureModify" AlternateText="Modify" OnClientClick="javascript:return SecureModify();" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trDelete" runat="server" style="height: 60px;" valign="top">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnSecureDelete" AlternateText="Delete" OnClientClick="javascript:return SecureDelete();"
                                                                OnClick="imgbtnSecureDelete_Click" />
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
                    <td bgcolor="#ffffff" valign="top">
                        &nbsp;</td>
                    <td valign="top">
                        <asp:Panel ID="pnlSecure" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0" style="height: 200px; width: 490px;"
                                class="formborder">
                                <tr style="height: 200px;">
                                    <td valign="top">
                                        <table cellpadding="4" cellspacing="0" border="0">
                                            <tr align="left" id="trCreatedBy" runat="server" visible="false">
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblCreatedBy" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCreatedByData" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td>
                                                    <asp:Label ID="lblSecureCategory" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <LCtrl:LAjitDropDownList ID="ddlSecureCategory" runat="server" MapXML="SecureCategory"
                                                        Enabled="false">
                                                    </LCtrl:LAjitDropDownList>
                                                    <asp:RequiredFieldValidator ID="reqSecureCategory" runat="server" Display="None"
                                                        ControlToValidate="ddlSecureCategory" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblmsgNote" runat="server" Visible="false" CssClass="msummary"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td>
                                                    &nbsp;</td>
                                                <td align="center">
                                                    <div id="divSubmit" style="display: none;">
                                                        <asp:ImageButton runat="server" ID="imgbtnSubmit" OnClick="imgbtnSubmit_Click" ValidationGroup="LAJITEntryForm"
                                                            AlternateText="Submit" />&nbsp;&nbsp;&nbsp;
                                                    </div>
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
            <asp:HiddenField ID="hdnSectedRow" runat="server" />
            <asp:HiddenField ID="hdnFldSecureAction" runat="server" />
            <asp:HiddenField ID="hdnRadIndex" runat="server" />
            <asp:HiddenField ID="hdnParentRow" runat="server" />
            <asp:HiddenField ID="hdnButtons" runat="server" />
            <asp:ValidationSummary ID="valSummmaryEntryForm" ShowSummary="false" runat="server"
                ShowMessageBox="true" HeaderText="Invalid input for the following fields" DisplayMode="BulletList"
                Enabled="true" EnableClientScript="true" ValidationGroup="LAJITEntryForm" />
        </div>
    </form>
</body>
</html>
