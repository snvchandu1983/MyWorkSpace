<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Notes.aspx.cs" Inherits="LAjitDev.PopUps.Notes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="LAjitControls" Namespace="LAjitControls" TagPrefix="LCtrl" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lajit::Notes</title>

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>

    <script type="text/javascript" src="../JavaScript/jquery.dialog.js"></script>

    <script type="text/javascript" src="../JavaScript/Utility.js"></script>

    <script type="text/javascript" src="../JavaScript/Common.js"></script>--%>

    <script language="javascript" type="text/javascript">
    function $get(eId) {
        return document.getElementById(eId);
    }
 
    function UpdateRadioNotesData(index)
    { 
        var radObj=document.getElementsByName("rbtngvCell")[index];
        $get("hdnSectedRow").value=radObj.value;
        $get("hdnRadIndex").value=index;
        var hdnObj=document.getElementsByName("hdnNotes")[index];
        var Result = hdnObj.value.split("~");
        if(Result.length == 2) {
            $get("txtNote").value=Result[0];
            if(Result[1]=="1")
            {
                $get("chkAgree").checked=true;
            }
            else
            {
                $get("chkAgree").checked=false;
            }
            $get("chkAgree").disabled=true;
        }
        $get("txtNote").disabled=true;
        $get("txtNote").style.backgroundColor="#FFFFFF";    
        $get("divSubmit").style.display="none";  
    }
 
     function NoteAdd()
     {
        //Radio button selection
        var index=$get("hdnRadIndex").value;
        var radObj=document.getElementsByName("rbtngvCell")[index];
        if( radObj != null)
        {
          radObj.checked=false;
        }
        //Clear selection
        $get("hdnRadIndex").value="";
        $get("txtNote").disabled=false;
        $get("chkAgree").disabled=false;
        $get("txtNote").value="";
        $get("hdnFldNoteAction").value="Add";
        $get("imgbtnNoteSubmit").disabled=false;
        if($get("chkAgree").checked==true)
        {
          $get("chkAgree").checked=false;
        }
        if($get("lblmsgNote")!=null)
        {
          $get("lblmsgNote").innerHTML="";
        }
        $get("txtNote").style.backgroundColor="#FFFFFF";
        $get("chkAgree").style.backgroundColor="#FFFFFF";
        $get("txtNote").focus();    
        $get("divSubmit").style.display="block";
        return false;
     }
 
    function NoteModify()
    { 
        if($get("hdnRadIndex").value=="")
        {
            jqAlert("Please select radio button");
            return false;
        } 
        else
        {
            $get("txtNote").disabled=false;
            $get("chkAgree").disabled=false;
            //firefox ok IE is not working
            $get("txtNote").style.backgroundColor="LightGoldenrodYellow";
            $get("chkAgree").style.backgroundColor="LightGoldenrodYellow";
            $get("hdnFldNoteAction").value="Modify";
            if($get("lblmsgNote")!=null)
            {
                $get("lblmsgNote").innerHTML ="";
            }      
            $get("txtNote").focus();      
            $get("divSubmit").style.display="block";
            return false;
        }
    }

    function NoteDelete()
    {  
        $get("divSubmit").style.display="none"; 
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
 
     function NoteCancel()
     {  
        parent.document.HideNotesModel();
     }
       
     function Noteclose()
     {
        $get("mpeNotesBehaviourID").hide();
     }
      
     function SubmitHideShow()
     {
        var visiblity= $get("hdnButtons").value;
        $get("divSubmit").style.display=visiblity;
     }

    </script>

</head>
<body onload="HideParentProgress();">
    <form id="frmNotes" defaultfocus="txtNote" runat="server">
        <div id="divNote" runat="server">
            <table cellpadding="0" cellspacing="0" border="0" width="550px">
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
                    <td width="50px" valign="top">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr valign="top">
                                <td valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle"
                                        style="height: 345px;">
                                        <tr valign="top">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr id="trAdd" runat="server" style="height: 60px;" valign="top">
                                                        <td align="center">
                                                            <asp:ImageButton runat="server" ID="imgbtnNoteAdd" AlternateText="Add" OnClientClick="javascript:return NoteAdd();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 60px;" valign="top" id="trModify" runat="server">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnNoteModify" AlternateText="Modify" OnClientClick="javascript:return NoteModify();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 60px;" valign="top" id="trDelete" runat="server">
                                                        <td align="center" valign="top">
                                                            <asp:ImageButton runat="server" ID="imgbtnNoteDelete" AlternateText="Delete" OnClientClick="javascript:return NoteDelete();"
                                                                OnClick="imgbtnNoteDelete_Click" />
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
                        <table cellpadding="0" cellspacing="0" border="0" style="height: 345px; width: 550px;"
                            class="formborder">
                            <tr id="trgview" runat="server" style="height: 173px;">
                                <td valign="top">
                                    <asp:GridView ID="gvNotes" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        SkinID="dshbrdCntPnlGrd" Width="100%" BorderColor="#BBD2FD" CellPadding="2" CellSpacing="0"
                                        OnRowDataBound="gvNotes_RowDataBound" AllowPaging="True" AllowSorting="True"
                                        OnPageIndexChanging="gvNotes_PageIndexChanging" OnSorting="gvNotes_Sorting" PageSize="5">
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
                            <tr style="height: 173px;">
                                <td valign="top">
                                    <asp:Panel ID="pnlNote" DefaultButton="imgbtnNoteSubmit" runat="server">
                                        <table cellpadding="4" cellspacing="0" border="0">
                                            <tr align="left">
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblNote" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <LCtrl:LAjitTextBox ID="txtNote" runat="server" MapXML="Note" BackColor="#CCCCCC"
                                                        TextMode="MultiLine" Enabled="false" Width="350px"></LCtrl:LAjitTextBox>
                                                    <asp:RequiredFieldValidator ID="reqNote" runat="server" Display="None" ControlToValidate="txtNote"
                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                        SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="regNote" runat="server" ControlToValidate="txtNote"
                                                        ValidationExpression="[^~]+" ToolTip="Special Character ~ not allowed." ErrorMessage="Special Character ~ not allowed."
                                                        Display="none" ValidationGroup="LAJITEntryForm" Enabled="false"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1"
                                                        height="3" /></td>
                                                <td>
                                                    <asp:Label ID="lblSharedUpdate" runat="server" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkAgree" runat="server" />
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
                                                        <asp:ImageButton runat="server" ID="imgbtnNoteSubmit" OnClick="imgbtnNoteSubmit_Click"
                                                            Width="40" ValidationGroup="LAJITEntryForm" AlternateText="Submit" Height="18" />&nbsp;&nbsp;&nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnSectedRow" runat="server" />
            <asp:HiddenField ID="hdnFldNoteAction" runat="server" />
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
