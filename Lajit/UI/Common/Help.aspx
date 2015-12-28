<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Help.aspx.cs" Inherits="LAjitDev.Common.Help" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
  <%-- <script type="text/javascript" src="../JavaScript/ParentChild.js"></script>--%>
   <script type="text/javascript">

      function OpenHelpPopUp(isPopUp,redirectPage)
        {
           if(isPopUp == "1")
           {
               popWin=window.open(redirectPage,'FlvPlayer','left=100,top=100,width=800,height=500,location=0,menubar=0,toolbar=0,status=0, scrollbars=1, resizable=0');
               popWin.focus();
               return false;
            } 
        }

        function JQueryPageEvents(){
            AttachAutoComplete("ctl00_cphPageContents_txthelpcatalog",'helpcatalog','<%=this.Page.ToString() %>');    
        }
</script>
  
   <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
               <script type="text/javascript" >
                   Sys.Application.add_load(JQueryPageEvents);
                 </script> 
                <table cellpadding="0" cellspacing="0" border="0"  width="100%">
                    <tr>
                      <%-- <td style="width: 1px;">
                            <img src="../App_Themes/<%=Session["MyTheme"]%>/Images/spacer.gif" alt="spacer" width="5"
                                height="1" />
                        </td>--%>
                        <!--GV-->
                        <td valign="top">
                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                <!--EntryForm tr-->
                                <tr align="left">
                                    <td valign="top">
                                      <%--<asp:Panel ID="pnlEntryFormTitle" runat="server" Width="100%">
                                             <table cellpadding="0" cellspacing="0" class="tblFormTitle">
                                            <tr style="height: 24px;">
                                                <td class="grdVwCurveLeft">
                                                    <img alt="Spacer" src="../App_Themes/<%=Session["MyTheme"]%>/Images/spacer.gif" height="1" />
                                                </td>
                                                <td id="htcCPGV1" class="grdVwtitle" style="width:40px">
                                                Help
                                                </td>
                                                <td class="grdVwCurveRight">
                                                    <img alt="Spacer" src="../App_Themes/<%=Session["MyTheme"]%>/Images/spacer.gif" height="1" />
                                                </td>
                                                <td id="htcCPGV1Auto" class="grdVwTitleAuto" runat="server">
                                                </td>
                                                <td class="grdVwTitleAuto" style="width: 20px" align="center">
                                                    <img id="imgCPGV1Expand" alt="Slide" src="../App_Themes/<%=Session["MyTheme"]%>/Images/minus-icon.png" />
                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>--%>
                                                                               
                                        <asp:Panel ID="pnlEntryForm" runat="server"  Visible="true" Height="511px" width="100%">
                                            <!-- entry form start height: 483px width:925px class="formmiddle" 480   width="922"-->
                                            <table id="tblEntryForm" runat="server" style="height: 500px;width:100%"  border="0"
                                                cellspacing="0" cellpadding="0" class="formmiddle">
                                                <!-- height 470 width 920  -->
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                                              <tr>
                                                                    <td align="left" valign="middle" class="helpsearch-border" style="height:49px;">
                                                                       <table  border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr align="left" valign="middle">
                                                                            <td style="width:5px;"><img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                                            <td>
                                                                             <asp:ImageButton id="imgbtnSearch" runat="server" OnClick="imgbtnGo_Click" width="27"></asp:ImageButton>
                                                                            </td>
                                                                            <td style="width:200px;">
                                                                             <LCtrl:LAjitTextBox ID="txthelpcatalog" Width="200px" Height="20px" runat="server"></LCtrl:LAjitTextBox>
                                                                          </td>
                                                                          <td style="width:29px;">
                                                                           <asp:ImageButton id="imgbtnGo" runat="server" OnClick="imgbtnGo_Click" width="29" height="20"></asp:ImageButton>
                                                                        </td> 
                                                                        <td align="center"><a href="#" onclick="javascript:window.alert('UNDERCONSTRUCTION')" class="black-smalllinks">Chat</a>  |  <a href="mailto:help@lajit.biz" class="black-smalllinks">Contact Us</a></td>
                                                                        <td align="right" style="width:62px;">
                                                                          <img alt="imghelplogo" runat="server" id="imghelplogo" width="62" border="0" title="LAjit Logo" />
                                                                        </td>
                                                                      </tr>
                                                                     </table>
                                                                    </td>
                                                             </tr> 
                                                            <tr align="left" valign="top">
                                                                <td align="center">
                                                                    <asp:Panel ID="pnlHelpTable" runat="server"  ScrollBars="auto" visible="false">
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                               <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <!-- form entry fields end -->
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
                                            <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
     <asp:HiddenField ID="hdnAutoFillBPGID" runat="server" />
</asp:content>
