<%@ Page Language="C#" MasterPageFile="~/MasterPages/TopLeft.Master" AutoEventWireup="true"
    Codebehind="GenView.aspx.cs" Inherits="LAjitDev.Common.GenView" Title="Untitled Page" %>

<%@ Register TagPrefix="LCtrl" Assembly="LAjitControls" Namespace="LAjitControls.JQGridView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
    <%-- <script src="../JavaScript/JQMethods.js" type="text/javascript"></script>

    <script src="../JavaScript/JqGridDebug/grid.locale-en.js" type="text/javascript"></script>

    <script src="../JavaScript/JqGridDebug/grid.loader.js" type="text/javascript"></script>

    <script type="text/javascript" src="../JavaScript/jquery-ui-1.7.2.custom.min.js"></script>

    <script src="../JavaScript/AjaxFileUpload.js" type="text/javascript"></script>
    
    <script src="../JavaScript/FrameManager.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
    
        jQuery(document).ready(function(){
            var winWidth=this.width;
            var XPos=eval(winWidth/2)-65;
            //Float the Submit/Cancel buttons.
            jQuery('#pnlSubmit').floating({targetX:XPos,targetY:"bottom"});
        });
    </script>

    <table border="0" cellspacing="0" cellpadding="0" style="width: 100%">
        <tr>
            <td valign="top" style="width: 50px">
                <div id="divBtnsUC">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr style="height: 15px">
                            <td align="left" valign="bottom" style="background-image: url('<%=Application["ImagesCDNPath"]%>images/left-top.png');
                                background-repeat: no-repeat">
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Panel ID="pnlBtns" runat="server" CssClass="formmiddle">
                                    <table cellpadding="0" cellspacing="0" width="40px">
                                        <%if (BPCAdd)
                                      { %>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <input type="image" name='Add' title="Add" id='btnAdd' onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/add-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/add-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/add-icon.png" onclick="return false;" />
                                            </td>
                                        </tr>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <input type="image" name='Clone' title="Clone" id='btnAddClone' src="<%=Application["ImagesCDNPath"]%>Images/add-clone.png"
                                                    onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/add-clone.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/add-clone-over.png'"
                                                    onclick="return false;" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/add-icon-disable.png" title="Add Permission Void" />
                                            </td>
                                        </tr>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/lite--add-clone.png" title="Add Clone Permission Void" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (BPCModify)
                                      { %>
                                        <tr style="height: 55px;" valign="top">
                                            <td align="center" valign="top">
                                                <input type="image" name='Edit' title="Modify" id='btnEdit' onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/modify-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/modify-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/modify-icon.png" onclick="return false;" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/modify-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (BPCDelete)
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <input type="image" name='Delete' title="Delete" id='btnDelete' onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/delete_icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/delete-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/delete_icon.png" onclick="return false;" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr style="height: 55px;" valign="top">
                                            <td>
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/delete-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (BPCFind)
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <input type="image" name='Search' title="AdvancedSearch" id='btnAdvancedSearch' onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/find-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/find-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/find-icon.png" onclick="return false;" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/find-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (enablePrint)
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/print-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/print-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (enableNote)
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <input type="image" name='Note' title="Note" id='btnNote' onclick="return false;"
                                                    value="<%=noteBPGID%>" bpgid="<%=noteBPGID%>" onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/footnote-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/footnote-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/footnote-icon.png" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/footnote-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (enableSecurity)
                                      { %>
                                        <tr style="height: 55px;">
                                            <td>
                                                <input type="image" name="Secure" title="Secure" id="btnSecure" onclick="return false;"
                                                    value="<%=secureBPGID%> " bpgid="<%=secureBPGID%> " onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/security-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/security-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/security-icon.png" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/security-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <%if (enableAttachment)
                                      { %>
                                        <tr style="height: 55px;">
                                            <td>
                                                <input type="image" name="Attachment" title="Attachment" id="btnAttachment" onclick="return false;"
                                                    value="<%=attachmentBPGID%>" bpgid="<%=attachmentBPGID%>" onmouseout="this.src='<%=Application["ImagesCDNPath"]%>Images/attachment-icon.png'"
                                                    onmouseover="this.src='<%=Application["ImagesCDNPath"]%>Images/attachment-icon-over.png'"
                                                    src="<%=Application["ImagesCDNPath"]%>Images/attachment-icon.png" />
                                            </td>
                                        </tr>
                                        <%}
                                      else
                                      { %>
                                        <tr valign="top">
                                            <td align="center" valign="top" style="height: 55px;">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/attachment-icon-disable.png" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <tr>
                                            <td style="height: 23px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="width: 5px;">
                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                    height="1" />
            </td>
            <td valign="top" style="height: 480px">
                <table border="0" cellspacing="0" cellpadding="0" style="width: 100%">
                    <tr>
                        <td>
                            <LCtrl:JQGridView ID="jqGrid" runat="server"></LCtrl:JQGridView>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <table border="0" cellspacing="0" cellpadding="0" width="770px" class="ui-jqdialog-content ui-widget-content">
                                <tr>
                                    <td>
                                        <div id="SearchFilter" style="margin-left: .3%; display: none;" class="FormGrid">
                                            <table>
                                                <tr>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                </table>
            </td>
        </tr>
    </table>
    <div id="pnlSubmit" style="position: absolute; padding-bottom: 18px">
        <table cellpadding="0" cellspacing="2">
            <tr>
                <td align="center">
                    <input type="image" style="height: 18px; display: none;" onclick="javascript:SaveInLineEdits();return false;"
                        alt="Submit" src="<%=Application["ImagesCDNPath"]%>Images/Modify-but.png" id="imgbtnInLineSave" />
                </td>
                <td align="center">
                    <input type="image" style="height: 18px; display: none;" onclick="javascript:CancelInLineEdits();return false;"
                        alt="Submit" src="<%=Application["ImagesCDNPath"]%>Images/Cancel-but.png" id="imgbtnInLineCancel" />
                </td>
            </tr>
        </table>
    </div>
    <!-- Hover Panels for the Print Buttons -->
    <asp:UpdatePanel ID="upReportsHoverPanel" runat="server" UpdateMode="Conditional"
        ChildrenAsTriggers="true">
        <contenttemplate>
            <div id="divHvrDirectPrint" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return PrintPage('CURR','ctl00_cphPageContents');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return PrintPage('ALL','ctl00_cphPageContents');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectChart" style="display: none;">
                <table width="222" cellspacing="0" cellpadding="0" border="0" style="height: 91px;"
                    class="GridPrintHover">
                    <tr>
                        <td valign="middle" align="center">
                            <table width="85%" cellspacing="2" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblXAxis" runat="server" Text="Group By" CssClass="LabelHoverMenu">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <LCtrl:LAjitDropDownList ID="ddlXAxis" runat="server">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblYAxis" runat="server" Text="Sum" CssClass="LabelHoverMenu">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <LCtrl:LAjitDropDownList ID="ddlYAxis" runat="server">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: CreateBPCLinks(;">
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr valign="middle" align="left">
                                                        <td style="width: 56px;">
                                                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=curr','<%=this.ClientID %>','ctl00$cphPageContents$ddlXAxis','ctl00$cphPageContents$ddlYAxis');">
                                                                Current</a></td>
                                                        <td>
                                                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=all','<%=this.ClientID %>','ctl00$cphPageContents$ddlXAxis','ctl00$cphPageContents$ddlYAxis');">
                                                                All Pages</a></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectPDF" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=PDF&PBD=0','ctl00_cphPageContents');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=PDF&PBD=0','ctl00_cphPageContents');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectExcel" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=EXCEL&PBD=0','ctl00_cphPageContents');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=EXCEL&PBD=0','ctl00_cphPageContents');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectHTML" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=HTML&PBD=0','ctl00_cphPageContents');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=HTML&PBD=0','ctl00_cphPageContents');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectMSWord" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=WORD&PBD=0','ctl00_cphPageContents');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=WORD&PBD=0','ctl00_cphPageContents');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectEmailPDF" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblEmailIDs" runat="server" SkinID="Label">
                                    <b><center>Enter Email IDS</center>
                                    </b>
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" width="200px" colspan="2">
                            <LCtrl:LAjitTextBox ID="txtEmailing" runat="server" TextMode="MultiLine" Width="200px"></LCtrl:LAjitTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="middle">
                            <asp:RadioButton runat="server" ID="radCurrent1" onclick="javascript:return SentEmail('CURR','ctl00_cphPageContents_hdnBPInfo',this);" />Current
                            <%--<input type="radio" id="radCurrent" runat="server" value="Current" onclick="javascript:return SentEmail('CURR','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />Current--%>
                        </td>
                        <td align="left" valign="middle">
                            <asp:RadioButton runat="server" ID="radAll1" onclick="javascript:return SentEmail('ALL','ctl00_cphPageContents_hdnBPInfo',this);" />All
                            Pages
                            <%-- <input type="radio" id="radAll" runat="server" value="All Pages" onclick="javascript:return SentEmail('ALL','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />All
                            Pages--%>
                        </td>
                    </tr>
                </table>
            </div>
        </contenttemplate>
        <triggers>
            <%--<asp:PostBackTrigger ControlID="lnkBtnExcelCurrent" />
            <asp:PostBackTrigger ControlID="lnkBtnExcelAll" />
            <asp:PostBackTrigger ControlID="lnkBtnHTMLCurrent" />
            <asp:PostBackTrigger ControlID="lnkBtnHTMLAll" />
            <asp:PostBackTrigger ControlID="lnkBtnWordCurrent" />
            <asp:PostBackTrigger ControlID="lnkBtnWordAll" />--%>
        </triggers>
    </asp:UpdatePanel>
    <div id="divPrintReports" style="display: none">
        <table style="width: 172px" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/grid-print.png"
                        title="Print this page." id="imgBtnPrint" name="imgBtnPrint" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/grid-chart.png"
                        title="Print Chart" id="imgBtnChart" name="imgBtnChart" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/grid-pdf.png"
                        title="Print PDF" id="imgBtnPDF" name="imgBtnPDF" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/grid-excel.png"
                        title="Print Excel" id="imgBtnExcel" name="imgBtnExcel" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/grid-html.png"
                        title="Print HTML" id="imgBtnHtml" name="imgBtnHtml" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/Grid_Word.png"
                        title="Print MSWord" id="imgBtnMSWord" name="imgBtnMSWord" />
                </td>
                <td align="left" style="width: 24px;">
                    <input type="image" onclick="return false;" src="<%=Application["ImagesCDNPath"]%>Images/email_icon2.png"
                        title="Email PDF" id="imgBtnEmailPDF" name="imgBtnEmailPDF" />
                </td>
                <td width="20px">
                    <img alt="spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnFormLvlLinks" runat="server" />
    <asp:HiddenField ID="hdnFormInfo" runat="server" />
    <asp:HiddenField ID="hdnSOXApprStatus" runat="server" />
    <asp:HiddenField ID="hdnBPInfo" runat="server" />
    <!--For Posting the data to the next form(Populated just before posting to another page) -->
    <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />
    <asp:HiddenField ID="hdnPreviewInPopup" Value="false" runat="server" />
    <asp:HiddenField ID="hdnNodeRow" Value="false" runat="server" />
    <asp:HiddenField ID="hdnSelectedCols" Value="" runat="server" />
    <asp:HiddenField ID="hdnJobCOADefault" runat="server" Value="" />
</asp:Content>
