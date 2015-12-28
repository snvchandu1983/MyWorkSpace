<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DetailedView.aspx.cs" Inherits="LAjitDev.Common.DetailedView" %>

<%@ Register Src="~/UserControls/GridViewControl.ascx" TagName="GVUC" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" language="javascript">
          //Set cdn paths and variables are defined in common.js
          g_cdnImagesPath='<%= Application["ImagesCDNPath"] %>';
          g_cdnScriptsPath='<%= Application["ScriptsCDNPath"] %>';
          g_virtualPath='<%= Application["VirtualPath"] %>';          
</script>



<head id="Head1" runat="server">
    <title>Detailed View</title>

    <script type="text/javascript">
    //Closes the DetailedView IFrame and redirects to another page.
    function CloseDetailedView(newURL)
    {
        top.location=newURL;
    }
    </script>

</head>
<body onload="HideParentProgress();" onkeydown="return OnAppKeyDown(event);">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>

     <%--   <script type="text/javascript" src="../JavaScript/jquery-1.3.2.min.js"></script>

        <script type="text/javascript" src="../JavaScript/jquery.qtip.js"></script>

        <script type="text/javascript" src="../JavaScript/jquery.dialog.js"></script>

        <script type="text/javascript" src="../JavaScript/Utility.js"></script>

        <script type="text/javascript" src="../JavaScript/Common.js"></script>

        <script type="text/javascript" src="../JavaScript/GridViewUserControl.js"></script>
        
        <script type="text/javascript" src="../JavaScript/FrameManager.js"></script>--%>
        
        

        
        <table id="tblContents" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <uc1:GVUC ID="ucGridView" runat="server" GridViewType="DashBoard" />
                </td>
            </tr>
        </table>
        <!--For Posting the data to the next form -->
        <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />
        <input type="hidden" id="ctl00_hdnThemeName" name="MyTheme" runat="server" />
        <asp:HiddenField ID="hdnFindBPGID" runat="server" />
        <asp:Panel ID="pnlPagePopUp" runat="server" Style="display: none" BorderWidth="0"
            BackColor="white" BorderColor="#000c19">
            <iframe id="iframePage" name="iframePage" visible="false" height="525" width="932"
                frameborder="0"></iframe>
            <!--Also update the height and width in CommonUI-BindPage()-->
            <!-- close button start -->
            <center>
                <asp:LinkButton ID="lnkBtnCloseIFrame" runat="server" Text="Close" OnClientClick="OnCloseLinkClick();return false;"></asp:LinkButton>
            </center>
            <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" />
            <!-- close button end -->
        </asp:Panel>
    </form>
</body>
</html>
