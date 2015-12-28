<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Attachement.aspx.cs" Inherits="LAjitDev.PopUps.Attachement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="LAjitControls" Namespace="LAjitControls" TagPrefix="LCtrl" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lajit :: Attachments</title>

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>

    <script type="text/javascript" src="../JavaScript/jquery.dialog.js"></script>
    
    <script type="text/javascript" src="../JavaScript/Utility.js"></script>
    
    <script type="text/javascript" src="../JavaScript/Common.js"></script>--%>
    
    <script type="text/javascript" src="../JavaScript/FrameManager.js"></script>

    <script language="javascript" type="text/javascript">
    
            function InitPopUps()
            {
              jQuery(document).ready(
                 function() {
                 $('#pnlNote').dialog({ autoOpen: false,
                                         bgiframe: false,
                                         width:'auto',
                                         position: 'center',
                                         minHeight: 10,
	                                     modal: true,
	                                     draggable:false,
	                                     resizable:false,
                                        });
                    $("#pnlNote").removeClass("ui-dialog-content ui-widget-content");
                    $("#pnlNote").dialog().parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                  }
                );    
            }
                    
            function UpdateRadioNotesData(index)
            {
                var radObj=document.getElementsByName("rbtngvCell")[index];
                $get("hdnSectedRow").value=radObj.value;
                $get("hdnRadIndex").value=index;
                var hdnObj=document.getElementsByName("hdnNotes")[index];
                $get("txtDescription").value=hdnObj.value;
                $get("txtDescription").disabled=true;
                $get("FileAttachment").disabled=true;
                $get("ddlFileType").disabled=true;
                $get("txtDescription").style.backgroundColor="#FFFFFF";
                $get("FileAttachment").style.backgroundColor="#FFFFFF";
                $get("pnlSubmit").style.display="none";
                parseXML(radObj.value);
            }
             
            function parseXML(xmlText)
            {
                //Create an XML document object.
                var xmlDoc=loadXMLString(xmlText);
                //Setting Attributes for imagebtnAttachSecure BPGID    string TrxID    string TrxType
                var nodeRows = xmlDoc.getElementsByTagName("Rows")[0];
                var control=$get("imgbtnAttachSecure");
                control.setAttribute("BPGID",$get("hdnCurrentBPGID").value);       
                control.setAttribute("TrxID",nodeRows.getAttribute("TrxID"));       
                control.setAttribute("TrxType",nodeRows.getAttribute("TrxType"));           
             }
             
            function AttachAdd()
            {
                var index=$get("hdnRadIndex").value;
                var radObj=document.getElementsByName("rbtngvCell")[index];
                if( radObj != null)
                {
                    radObj.checked=false;
                }
                //Clear selection
                $get("hdnRadIndex").value="";
                //Textbox
                $get("txtDescription").disabled=false;
                $get("txtDescription").value="";
                $get("hdnFldNoteAction").value="Add";
                $get("FileAttachment").disabled=false;
                $get("ddlFileType").disabled=false;
                $get("ddlFileType").selectedIndex=0;
                if($get("lblmsgNote")!=null)
                {
                    $get("lblmsgNote").innerHTML ="";
                }
                $get("txtDescription").style.backgroundColor="#FFFFFF";
                $get("FileAttachment").style.backgroundColor="#FFFFFF";
                $get("ddlFileType").style.backgroundColor="#FFFFFF";
                $get("pnlSubmit").style.display="block";
                $get("FileAttachment").readonly="true";
                $get("txtDescription").focus(); 
                return false;
            }
             
            function AttachDelete()
            {
                //Clearing label message
                if($get("lblmsgNote")!=null)
                {
                    $get("lblmsgNote").value="";
                }
                $get("pnlSubmit").style.display="none";
                if($get("hdnRadIndex").value=="")
                {
                    jqAlert("Please select radio button");
                    return false;
                } 
                else
                {
                    return confirm('Are you sure you want to delete?');
                }
            }
             
             function AttachModify()
             {
               if($get("hdnRadIndex").value=="")
               {
                 jqAlert("Please select radio button");
                 return false;
               } 
               else
               {
                  $get("hdnFldNoteAction").value="Modify";
                  $get("txtDescription").disabled=false;
                  $get("FileAttachment").disabled=false;
                  $get("ddlFileType").disabled=false;
                  $get("ddlFileType").selectedIndex=0;
                  $get("txtDescription").style.backgroundColor="LightGoldenrodYellow";
                  $get("FileAttachment").style.backgroundColor="LightGoldenrodYellow";
                  $get("ddlFileType").style.backgroundColor="LightGoldenrodYellow";
                  if($get("lblmsgNote")!=null)
                  {
                     $get("lblmsgNote").innerHTML ="";
                  }
                  $get("pnlSubmit").style.display="block";
                  $get("txtDescription").focus();
                  return false;
               }
             }
             
            function AttachDownload()
            {
                if($get("hdnRadIndex").value=="")
                {
                    jqAlert("Please select radio button");
                    return false;
                } 
                else
                {
                    return true;
                }
            }
             
            function NoteCancel()
            {
                parent.document.HideNotesModel();
            }
              

            function Openframe(framename,pagename)
            {

               if($get("hdnRadIndex").value=="")
               {
                 jqAlert("Please select radio button");
                 return false;
               }
              else
              {
                    //Setting Display
                    switch (framename){
                        case "iframeSecure":             
                            //string BPGID    string TrxID    string TrxType ;
                            var control=$get("imgbtnAttachSecure");
                            var QueryStringValues="?BPGID="+control.getAttribute('BPGID')+"&TrxID="+ control.getAttribute('TrxID')+"&TrxType="+ control.getAttribute('TrxType');
                            //                                     $get("iframeSecure").style.display='';
                            //Sending Selected ParentRow as QueryString Paramenter       
                            // pagename=pagename+"?ParentRow="+$get("ctl00_cphPageContents_GVUC_hdnRowObject").value;    
                            pagename="../PopUps/"+pagename+QueryStringValues;  
                        break;                              
                    }
                    ShowIFrame(pagename,{isModal:true,width:545,height:239,allowMaximise:false,resize:false,title:framename.replace('iframe','')});
//                         DisplayPopUp(true,'pnlNote');      
//Setting Pagename
//                         $get(framename).src=pagename; //"../PopUps/"+pagename;                  
                    //$find('mpeAttachmentBehaviourID').show();
                    return false;
                    }
            } 

            //FootNote and Attachment Scripts Usage
            function HideSecureModel()
            {
                DisplayPopUp(false,'pnlNote');
            }
             
             
             // this fn gets called after 2 secs from the loading of the page in the browser
             // mean while the IFrame will be ready.  onload="setTimeOut(2000,'DelayFocus()');"
             function DelayFocus()
             {
               // jqAlert("HI");
                $get("txtDescription").focus(); 
             }
             
             function SetFilePath()
             {
             //FileAttachment
              $get("hdnUplFullPath").value=$get("FileAttachment").value;
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
<body onload="HideParentProgress();InitPopUps();">
    <form id="frmAttachement" defaultfocus="txtDescription" runat="server">
        <asp:ScriptManager ID="AttachScriptMngr" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <div id="divInitAttachement" runat="server">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <!--Pop up header tr-->
                <tr visible="false" valign="top">
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
                    <td bgcolor="#ffffff">
                        &nbsp;</td>
                    <td width="50px">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr valign="top">
                                <td valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle"
                                        style="height: 335px;">
                                        <tr valign="top">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr style="height: 60px;" valign="top" id="trDownload" runat="server">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnAttachDownload" AlternateText="Download"
                                                                OnClientClick="javascript:return AttachDownload();" OnClick="imgbtnAttachDownload_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 60px;" valign="top" id="trAdd" runat="server">
                                                        <td align="center">
                                                            <asp:ImageButton runat="server" ID="imgbtnAttachAdd" AlternateText="Add" OnClientClick="javascript:return AttachAdd();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 60px;" valign="top" id="trDelete" runat="server">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnAttachDelete" AlternateText="Delete" OnClientClick="javascript:return AttachDelete();"
                                                                OnClick="imgbtnAttachDelete_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 60px;" valign="top" id="trSecure" runat="server">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnAttachSecure" AlternateText="Secure" OnClientClick="javascript:return Openframe('iframeSecure','Secure.aspx');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!-- left side icons end -->
                    </td>
                    <td bgcolor="#ffffff">
                        &nbsp;</td>
                    <td valign="top" bgcolor="#ffffff">
                        <!-- Gridview and entry start -->
                        <table cellpadding="0" cellspacing="0" border="0" style="height: 335px; width: 540px;"
                            class="formborder" bgcolor="white">
                            <tr id="trattachgrid" runat="server" style="height: 166px;">
                                <td valign="top">
                                    <!-- foot note gridview start -->
                                    <asp:GridView ID="gvAttach" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        SkinID="dshbrdCntPnlGrd" Width="100%" BorderColor="#BBD2FD" CellPadding="2" CellSpacing="0"
                                        OnRowDataBound="gvAttach_RowDataBound" AllowPaging="True" AllowSorting="True"
                                        OnPageIndexChanging="gvAttach_PageIndexChanging" OnSorting="gvAttach_Sorting"
                                        PageSize="5">
                                        <AlternatingRowStyle CssClass="AlternatingCOARowStyle" />
                                        <HeaderStyle CssClass="myreportHeaderText" BackColor="#A7CED8" ForeColor="#253D62"
                                            HorizontalAlign="Left" />
                                        <FooterStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRbtn" runat="server" Visible="true" EnableViewState="true"></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="myreportItemText" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblNotesGVmsg" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 166px;">
                                <td valign="top">
                                    <asp:Panel ID="pnlAttachment" runat="server">
                                        <table cellpadding="4" cellspacing="0" border="0">
                                            <tr align="left">
                                                <td>
                                                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblDescription" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" BackColor="#CCCCCC"
                                                        TextMode="MultiLine" Enabled="false" Width="250px"></LCtrl:LAjitTextBox>
                                                    <asp:RequiredFieldValidator Display="None" ID="reqDescription" runat="server" ControlToValidate="txtDescription"
                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                        SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblFileName" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="FileAttachment" runat="server" Enabled="false" BackColor="#CCCCCC" />
                                                    <asp:RequiredFieldValidator Display="None" ID="reqFileName" runat="server" ControlToValidate="FileAttachment"
                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                        SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblFileType" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <LCtrl:LAjitDropDownList ID="ddlFileType" runat="server" MapXML="FileType" Enabled="false">
                                                    </LCtrl:LAjitDropDownList>
                                                    <asp:RequiredFieldValidator ID="reqFileType" Display="None" runat="server" ControlToValidate="ddlFileType"
                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                        SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblmsgNote" runat="server" Visible="false" CssClass="msummary"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td>
                                                    &nbsp;</td>
                                                <td align="center">
                                                    <asp:Panel ID="pnlSubmit" runat="server">
                                                        <asp:ImageButton runat="server" ID="imgbtnAttachSubmit" OnClick="imgbtnAttachSubmit_Click"
                                                            Width="51" ValidationGroup="LAJITEntryForm" AlternateText="Submit" Height="18" />&nbsp;&nbsp;&nbsp;
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!-- foot note entry page end style="padding-right: 10px; padding-bottom: 3px" -->
                                </td>
                            </tr>
                        </table>
                        <!-- Gridview and entry end -->
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnSectedRow" runat="server" />
            <asp:HiddenField ID="hdnFldNoteAction" runat="server" />
            <asp:HiddenField ID="hdnRadIndex" runat="server" />
            <asp:HiddenField ID="hdnParentRow" runat="server" />
            <asp:HiddenField ID="hdnCurrentBPGID" runat="server" />
            <asp:HiddenField ID="hdnButtons" runat="server" />
            <asp:HiddenField ID="hdnUplFullPath" runat="server" />
            <asp:XmlDataSource ID="xdsAttachment" runat="server"></asp:XmlDataSource>
        </div>
        <!-- Secure PopupCode Start -->
        <asp:HiddenField ID="hdfDummy" runat="server" />
        <div id="div1">
            <asp:Panel ID="pnlNote" runat="server" Style="display: none" BackColor="white" BorderWidth="0"
                BorderColor="#000c19">
                <iframe id="iframeSecure" name="iframeSecure" visible="false" height="239px" width="545px"
                    frameborder="0"></iframe>
                <!-- close button start -->
                <center>
                    <a id='lnkBtnCloseIFrame' href="javascript:HideSecureModel();">Close</a>
                </center>
                <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" />
                <!-- close button end -->
            </asp:Panel>
        </div>
        <asp:ValidationSummary ID="valSummmaryEntryForm" ShowSummary="false" runat="server"
            ShowMessageBox="true" HeaderText="Invalid input for the following fields" DisplayMode="BulletList"
            Enabled="true" EnableClientScript="true" ValidationGroup="LAJITEntryForm" />
    </form>
</body>
</html>
