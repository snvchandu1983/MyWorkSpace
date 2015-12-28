<%@ Page Language="C#" AutoEventWireup="true" Codebehind="FlashViewer.aspx.cs" Inherits="LAjitDev.Common.FlashViewer" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Lajit :: Flash Viewer</title>

   <%-- <script type="text/javascript" src="../JavaScript/AC_RunActiveContent.js"></script>--%>

    <script type="text/javascript">
    function closePage()
    {
        window.close()
    }

    </script>

</head>
<body>
    <form id="frmFlashViewer" runat="server">
        <asp:Panel ID="pnlContent" runat="server">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="width: 5px;">
                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                            height="1" />
                    </td>
                    <!--GV-->
                    <td class="tblMainCont" >
                        <table class="tblMainCont" cellpadding="0" cellspacing="0" border="0">
                            <!--Title tr-->
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCPGV1Title" runat="server" Width="100%">
                                        <table cellpadding="0" cellspacing="0" class="tblFormTitle">
                                            <tr style="height: 24px;">
                                                <td class="grdVwCurveLeft">
                                                    <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                                                </td>
                                                <td id="htcCPGV1" runat="server" class="grdVwtitle" style="width: 50px;">
                                                Help
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
                            <!--EntryForm tr-->
                            <tr align="left" valign="top">
                                <td class="tdEntryFrm">
                                    <asp:Panel ID="pnlEntryForm" runat="server" Visible="true" Height="511px">
                                        <!-- entry form start width:925px-->
                                        <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0"
                                            cellspacing="0" cellpadding="0" class="formmiddle">
                                            <tr>
                                                <td align="center" valign="top" style="height: 460px; width: 925px">
                                                    <table border="0" cellspacing="0" cellpadding="2">
                                                        <tr style="height: 1px;" align="right" valign="top">
                                                            <td align="right" valign="middle" colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" height="10px">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr align="left" valign="top">
                                                            <td id="flashcontent">
                                                                <!-- flash player start -->

                                                                <script language="javascript" type="text/javascript">
				                                                        <!--
					                                                        // Version check based upon the values defined in globals
					                                                        var hasRequestedVersion = DetectFlashVer(10, 0, 0);
					                                                        var strTheme='<%=(string)Session["MyTheme"]%>';
					                                                        var strImagesCDNPath='<%=(string)Application["ImagesCDNPath"]%>'; 
					                                                        //var strTheme="LAjit";
                                                                            if(!hasRequestedVersion)
					                                                        {
						                                                        var div = document.getElementById("flashcontent");
						                                                        var imagePath=strImagesCDNPath +"images/ERROR_getFlashPlayer.gif";
						                                                        //alert(imagePath);
						                                                        div.innerHTML = "<a href='http://www.adobe.com/go/getflashplayer/' style='color:black'><img src='"+ imagePath +"' width='641' height='377' /></a>";
                                								
					                                                        }
					                                                        else{
					                                                        
					                                                            /*flash variable start */
					                                                            var playerSWF = strImagesCDNPath +"images/videoPlayer";
					                                                            var streamPath = '<%=(string)strFilePath%>';
					                                                            var flashVars = "";
		                                                                     
           	                                                                    flashVars += "&videoWidth=";
                                                                                flashVars += 0;

                                                                                flashVars += "&videoHeight=";
                                                                                flashVars += 0;

                                                                                flashVars += "&dsControl=";
                                                                                flashVars += unescape("manual");

                                                                                flashVars += "&dsSensitivity=";
                                                                                flashVars += 100;

                                                                                flashVars += "&serverURL=";
                                                                                flashVars += streamPath;                                      
                                                                                
			                                                                    flashVars += "&DS_Status=";
			                                                                    flashVars += "true";

                                                                                flashVars += "&streamType=";
                                                                                flashVars +=  "vod"                         

                                                                                flashVars += "&autoStart=";
                                                                                flashVars += unescape("true");

			                                                                    tag = "&lt;object width='640' height='377' id='videoPlayer' name='videoPlayer' type='application/x-shockwave-flash' classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' &gt;&lt;param name='movie' value='"+ playerSWF +"' /&gt; &lt;param name='quality' value='high' /&gt; &lt;param name='bgcolor' value='#000000' /&gt; &lt;param name='allowfullscreen' value='true' /&gt; &lt;param name='flashvars' value= '"+					
			                                                                    flashVars +"'/&gt;&lt;embed src='"+ playerSWF +"' width='640' height='377' id='videoPlayer' quality='high' bgcolor='#000000' name='videoPlayer' allowfullscreen='true' pluginspage='http://www.adobe.com/go/getflashplayer'   flashvars='"+ flashVars +"' type='application/x-shockwave-flash'&gt; &lt;/embed&gt;&lt;/object&gt;";
					                                                        
					                                                          /*flash variable end */
					                                                        
							                                                        AC_FL_RunContent(
								                                                        "src", playerSWF,
								                                                        "width", "640",
								                                                        "height", "377",
								                                                        "id", "videoPlayer",
								                                                        "quality", "high",
								                                                        "bgcolor", "#000000",
								                                                        "name", "videoPlayer",
								                                                        "allowfullscreen","true",
								                                                        "type", "application/x-shockwave-flash",
								                                                        "pluginspage", "http://www.adobe.com/go/getflashplayer",
								                                                        "flashvars", flashVars 
							                                                        );
                                												
					                                                        }
			                                                          // -->    
                                                                </script>

                                                                <noscript>
                                                                    // Provide alternate content for browsers that do not support scripting // or for
                                                                    those that have scripting disabled. <a href="http://www.adobe.com/go/getflashplayer/"
                                                                        style="color: black">
                                                                        <img src="images/ERROR_getFlashPlayer.gif" width="640" height="377" /></a>
                                                                </noscript>
                                                                <!-- flash player start -->
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
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <a href="#" class="topboldlinks" onclick="javascript:window.close();">Close</a>
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
        </asp:Panel>
    </form>
</body>
</html>
