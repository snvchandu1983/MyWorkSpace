<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SetCompany.aspx.cs" Inherits="LAjitDev.Common.SetCompany" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">

<asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <contenttemplate>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="width: 5px;">
                            <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                height="1" />
                        </td>
                        <!--GV-->
                        <td valign="top">
                            <table cellpadding="0" cellspacing="0">
                                <!--EntryForm tr-->                                                                
                                <tr align="left">
                                    <td valign="top">
                                        <asp:Panel ID="pnlEntryFormTitle" runat="server" Width="100%" Visible="true">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1" >
                                                <tr style="height: 24px;">
                                                    <td style="width: 5px;" class="grdVwCurveLeft">
                                                    </td>
                                                    <td id="htcEntryForm" width="105" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                        border-width:0px;" >
                                                         Change Company
                                                    </td>
                                                    <td style="width: 6px;" class="grdVwCurveRight">
                                                    </td>
                                                    <td id="htcEntryFormAuto" class="grdVwTitleAuto" runat="server" style="height: 24px;">
                                                    </td>
                                                    <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlEntryForm" runat="server" Visible="true" Height="511px">
                                            <!-- entry form start -->
                                            <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
                                                <tr>
                                                      <td align="left" valign="top" style="height:460px;width:925px">
                                                        <table  border="0" cellspacing="0" cellpadding="2">
                                                            <tr style="height:1px;" align="right" valign="top">                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td colspan="2" height="10px">&nbsp;</td>
                                                            </tr> 
                                                            <tr align="left" valign="top">
                                                               <td  class="formtext" style="height: 24px; width:160px">
                                                                   <asp:Label ID="lblCompany" Text="Roles" runat="server" SkinID="Label"></asp:Label>
                                                              </td>
                                                              <td valign="middle" align="left">
                                                                    <LCtrl:LAjitDropDownList ID="ddlEntity" runat="server" MapXML="EntityID" AutoPostBack="false"></LCtrl:LAjitDropDownList>
                                                              </td>
                                                            </tr>
                                                            
                                                         <tr>
                                                           <td colspan="2">  
                                                              <div id="serverStatus" style="width: auto; height: auto; visibility: hidden; position: absolute;
                                                                top: 20px; right: 20px;">
                                                                <br />
                                                               <img src="<%=Application["ImagesCDNPath"]%>Images/ajax-loader.gif" />
                                                               </div>
                                                            </td>
                                                          </tr> 
                                                            <!--Submit and Cancel buttons-->
                                                            <tr  style="height:24px">
                                                                <td colspan="2" align="center">
                                                                    <asp:Label  SkinID="LabelMsg" ID="lblConfirm" runat="server" Visible="false" ></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="2" align="center">
                                                                <table cellpadding="0" cellspacing="0"  border="0">
                                                                    <tr valign="top">
                                                                         <td >&nbsp;</td>
                                                                         <td align="left" valign="middle">
                                                                           <asp:ImageButton runat="server" ID="imgbtnSubmit" OnClick="imgbtnSubmit_Click"
                                                                          Width="51" AlternateText="Submit" Height="18" onClientClick="javascript:ShowPageProgress();return true;"/>                                                              
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                               </td>                                                                                                                                   
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblmsg" runat="server" Visible="false"   SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <!-- form entry fields end -->
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
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
          </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
       <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" /> 
       <input type="hidden" id="hidChecked" name="hidChecked"  runat="server"/> 
        <!--parentBPInfo and parentGVDataXML stores the parent page info.They are reset to session when child popup is closed-->       
        <asp:HiddenField ID="parentBPInfo" runat="server" />
        <asp:HiddenField ID="ParentGVDataXML" runat="server" />        
        <asp:HiddenField ID="hdnFldBPInfo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFldBPOut" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFldReportName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnNeedToConfirmExit" runat="server" />
        <!-- Stores the BPEInfo required while posting to another page upon BPC Icon Click-->
        <asp:HiddenField ID="hdnBPIconBPEINFO" runat="server" /> 
        
        <script type="text/javascript">
    function ShowPageProgress()
    {
        var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
        //Add a div in the middle of the page on top of the semi-transparent layer.
        if(document.getElementById('box') == null && !prm.get_isInAsyncPostBack())//Check whether postback has completed within the ShowProgress Delay.
        {
            //var currentTheme = document.getElementById('ctl00_hdnThemeName').value;
            var div = document.createElement('div');
            div.style.zIndex = 100002009;
            div.id = 'box';
            div.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
            div.style.top = '200px';
            div.style.left = (width / 2) - 50 + 'px'; 
            div.innerHTML = '<img alt="Please wait.." src='+<%=Application["ImagesCDNPath"]%>+'Images/ajax-loader.gif" />';
            document.body.appendChild(div);

            if(Sys.Browser.name == 'WebKit')
            {
                setTimeout('HideProgress()', 700);
            }
        }
    }   
</script>
</asp:content>
