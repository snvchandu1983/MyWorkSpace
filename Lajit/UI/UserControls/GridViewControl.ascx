<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewControl.ascx.cs"
    Inherits="LAjitDev.GridViewControl" %>
<asp:UpdatePanel ID="updtPnlGrdVw" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:Panel ID="pnlWrapper" runat="server" Visible="true">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr id="trTitle" runat="server">
                    <td style="width: 100%; height: 24px;">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="cursor: hand;
                            border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                            <tr style="height: 24px;">
                                <td style="width: 4px" class="grdVwRPTitle" align="left">
                                </td>
                                <td id="htcGridTitle" runat="server" class="grdVwRPTitle" align="left">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border-width: 1px;
                            border-right-style: double; border-left-style: double; border-bottom-style: double;
                            border-top-style: none; border-color: #d7e0f1;">
                            <tr>
                                <td style="width: 100%;" colspan="2">
                                    <div style="width: 100%">
                                        <asp:GridView ID="grdVwContents" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdVwContents_RowDataBound"
                                            AllowSorting="true" EnableViewState="True" EmptyDataText="No Records found!!"
                                            OnSorting="grdVwContents_OnSorting" SkinID="dshbrdPopUpControl" GridLines="vertical"
                                            OnPreRender="grdVwContents_PreRender">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="2px">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Process" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBxProcess" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="left">
                                                    <HeaderTemplate>
                                                        <table id="tblFind" runat="server">
                                                            <tr>
                                                                <td>
                                                                    <LCtrl:LAjitImageButton ID="imgBtnExpandFind" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <LCtrl:LAjitImageButton ID="imgBtnQuickSearch" runat="server" OnClick="imgBtnQuickSearch_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnTTnNavigate" Width="19" runat="server" OnClick="imgBtnTTnNavigate_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlPagingCtrls" runat="server" HorizontalAlign="Right">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 5%">
                                                </td>
                                                <td align="center" style="width: 5%">
                                                    <asp:LinkButton ID="lbtnFirst" runat="server" OnClick="lnkBtnFirst_Click" OnClientClick="javascript:SetExitConfirmationToFalse();return true;">
                                                    </asp:LinkButton>
                                                </td>
                                                <td align="center" style="width: 5%">
                                                    <asp:LinkButton ID="lbtnPrevious" runat="server" OnClick="lnkBtnPrev_Click" OnClientClick="javascript:SetExitConfirmationToFalse();return true;">                  
                                                    </asp:LinkButton>
                                                </td>
                                                <td align="center" style="width: 20%">
                                                    <asp:Label ID="lblPageStatus" runat="server"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 5%">
                                                    <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lnkBtnNext_Click" OnClientClick="javascript:SetExitConfirmationToFalse();return true;">
                                                    </asp:LinkButton>
                                                </td>
                                                <td align="center" style="width: 5%">
                                                    <asp:LinkButton ID="lbtnLast" runat="server" OnClick="lnkBtnLast_Click" OnClientClick="javascript:SetExitConfirmationToFalse();return true;">
                                                    </asp:LinkButton>
                                                </td>
                                                <td style="width: 10%">
                                                </td>
                                                <td align="right" style="width: 30%">
                                                    <asp:Label ID="lblPageNo" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 15%">
                                                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td align="right">
                                    <asp:Panel ID="pnlPrint" runat="server" Width="150px">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnPrint" ToolTip="Print this page." runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnChart" ToolTip="Print Chart" runat="server" OnClientClick="return false" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnPDF" ToolTip="Print PDF" runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnExcel" ToolTip="Print Excel" runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnHtml" ToolTip="Print HTML" runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnMSWord" ToolTip="Print MSWord" runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td align="left" style="width: 16%">
                                                    <asp:ImageButton ID="imgBtnEmailPDF" ToolTip="Email PDF" runat="server" OnClientClick="return false;" />
                                                </td>
                                                <td width="20%">
                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" />
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
            <div id="divHvrDirectPrint" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return PrintPage('CURR','<%=this.ClientID %>');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return PrintPage('ALL','<%=this.ClientID %>');">
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
                                        <td valign="middle" align="left" style="height: 20px;">
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr valign="middle" align="left">
                                                        <td style="width: 56px;">
                                                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=curr','<%=this.ClientID %>','ctl00$cphPageContents$GVUC$ddlXAxis','ctl00$cphPageContents$GVUC$ddlYAxis');">
                                                                Current</a>
                                                        </td>
                                                        <td>
                                                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=all','<%=this.ClientID %>','ctl00$cphPageContents$GVUC$ddlXAxis','ctl00$cphPageContents$GVUC$ddlYAxis');">
                                                                All Pages</a>
                                                        </td>
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
                <table cellpadding="0" cellspacing="2" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top" style="padding-left: 3px">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=PDF&PBD=0','<%=this.ClientID %>');">
                                Current</a>
                        </td>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=PDF&PBD=0','<%=this.ClientID %>');">
                                All Pages</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Group By" CssClass="LabelHoverMenu">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <LCtrl:LAjitDropDownList ID="ddlPDFGrpBy" runat="server">
                                            <asp:ListItem Text="Choose" Value="-1" Selected="True"></asp:ListItem>
                                        </LCtrl:LAjitDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectExcel" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top" style="padding-left: 3px">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=EXCEL&PBD=0','<%=this.ClientID %>');">
                                Current</a>
                        </td>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=EXCEL&PBD=0','<%=this.ClientID %>');">
                                All Pages</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEXCELGrpBy" runat="server" Text="Group By" CssClass="LabelHoverMenu">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <LCtrl:LAjitDropDownList ID="ddlEXCELGrpBy" runat="server">
                                            <asp:ListItem Text="Choose" Value="-1" Selected="True"></asp:ListItem>
                                        </LCtrl:LAjitDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectHTML" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=HTML&PBD=0','<%=this.ClientID %>');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=HTML&PBD=0','<%=this.ClientID %>');">
                                All Pages</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divHoverPageSelectMSWord" style="display: none;">
                <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=WORD&PBD=0','<%=this.ClientID %>');">
                                Current</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=WORD&PBD=0','<%=this.ClientID %>');">
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
                            <asp:RadioButton runat="server" ID="radCurrent1" onclick="javascript:return SentEmail('CURR','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />Current
                        </td>
                        <td align="left" valign="middle">
                            <asp:RadioButton runat="server" ID="radAll1" onclick="javascript:return SentEmail('ALL','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />All
                            Pages
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Specifes to the Server side whether at the client-side the Find Bar has been toggled or not.(Visible=1/Invisible=0)-->
            <asp:HiddenField ID="hdnCSFindVisible" runat="server" />
            <asp:HiddenField ID="hdnFindEnabled" runat="server" />
            <asp:HiddenField ID="hdnMainViewState" runat="server" />
            <asp:HiddenField ID="hdnMaxPages" runat="server" />
            <asp:HiddenField ID="hdnCurrPageNo" runat="server" />
            <%--<asp:HiddenField ID="hdnSelectedRows" runat="server" />--%>
            <asp:HiddenField ID="hdnGVTreeNodeName" runat="server" />
            <asp:HiddenField ID="hdnRowObject" runat="server" />
            <asp:HiddenField ID="hdnDefaultPageSize" runat="server" />
            <asp:HiddenField ID="hdnFormInfo" runat="server" />
            <!-- Stores the grid BPGID and PageInfo(Accessed in OnColumnLinkClick();) -->
            <asp:HiddenField ID="hdnGVBPEINFO" runat="server" />
            <asp:HiddenField ID="hdnFindCriteria" runat="server" />
            <!-- Stores the BPEInfo used by the classs object to request data at any point of time -->
            <asp:HiddenField ID="hdnSearchBPInfo" runat="server" />
            <!-- Specifes whether the generated PDF should be displayed in IFrame(popup) or a new tab -->
            <asp:HiddenField ID="hdnPreviewInPopup" runat="server" />
            <!-- Specifes current selected row index which can be used to get previous/next record-->
            <asp:HiddenField ID="hdnSelectedRowIndex" runat="server" />
            <asp:HiddenField ID="hdnMasterBPIn" runat="server" />
            <asp:HiddenField ID="hdnToolTipJS" runat="server" />
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="imgBtnPDF" />
        <%--<asp:PostBackTrigger ControlID="imgBtnPDFCurrent" />
        <asp:PostBackTrigger ControlID="imgBtnPDFAll" />
        <asp:PostBackTrigger ControlID="imgBtnExcel" />
        <asp:PostBackTrigger ControlID="lnkBtnExcelCurrent" />
        <asp:PostBackTrigger ControlID="lnkBtnExcelAll" />
        <asp:PostBackTrigger ControlID="imgBtnHtml" />
        <asp:PostBackTrigger ControlID="lnkBtnHTMLCurrent" />
        <asp:PostBackTrigger ControlID="lnkBtnHTMLAll" />
        <asp:PostBackTrigger ControlID="lnkBtnWordCurrent" />
        <asp:PostBackTrigger ControlID="lnkBtnWordAll" />--%>
    </Triggers>
</asp:UpdatePanel>
<div class="contextMenu" id="clearMenu" style="display: none">
    <ul>
        <li id="open">
            <img src="<%=Application["ImagesCDNPath"]%>Images/intoo.png" />Clear Search</li>
    </ul>
</div>
