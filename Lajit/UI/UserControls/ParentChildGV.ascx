<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ParentChildGV.ascx.cs"
    Inherits="LAjitDev.ParentChildGV" %>
<asp:UpdatePanel ID="updtPnlGrdVw" runat="server" UpdateMode="Always">
    <ContentTemplate>

        <script type="text/javascript">

             function expandcollapse(imgObj, divID)
             { 
                var div = document.getElementById(divID);
                if( div != null) 
                {
                    if (div.style.display == "none")
                    {
                        jQuery(div).slideDown('slow');
                        imgObj.src =g_cdnImagesPath+ "Images/minus-icon.png";
                    }
                    else
                    {
                        jQuery(div).slideUp('slow');
                        imgObj.src=g_cdnImagesPath+"Images/add_symbol.png";
                    }
                }
            }   
          
        </script>

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
                                    <asp:DataGrid ID="grdVwContents" runat="server" OnItemDataBound="grdVwContents_DataBound"
                                        AllowSorting="true" EnableViewState="True" GridLines="None" Width="100%" AutoGenerateColumns="false"
                                        OnSortCommand="grdVwContents_OnSorting">
                                        <HeaderStyle BackColor="#d7e0f1" HorizontalAlign="Left" Height="18px" Font-Bold="true" />
                                        <ItemStyle HorizontalAlign="Left" Height="18px" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-Width="20px">
                                                <ItemStyle CssClass="myreportHeaderText" />
                                                <ItemTemplate>
                                                    <asp:Image ID="imgExpand" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td colspan="97%" align="center">
                                                            <asp:Panel ID="pnlChildGV" runat="server" Style="display: none;">
                                                                <asp:GridView ID="gvBranch" runat="server" AutoGenerateColumns="false" GridLines="Horizontal"
                                                                    BorderColor="#BBD2FD" Width="97%" CellPadding="2" CellSpacing="0" AllowSorting="true"
                                                                    EmptyDataText="No records found!!">
                                                                    <HeaderStyle CssClass="myreportHeaderText" BackColor="#A7CED8" ForeColor="#253d62"
                                                                        HorizontalAlign="left" />
                                                                    <FooterStyle BackColor="White" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="0px" ItemStyle-Width="0px">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlPagingCtrls" runat="server" HorizontalAlign="Right">
                                    <table width="100%">
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
                            <td style="width: 120px" align="right">
                                <asp:Panel ID="pnlPrint" runat="server" Width="150px">
                                    <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnPrint" ToolTip="Print this page." runat="server" ImageUrl="~/App_Themes/LAjit/Images/grid-print.png"
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnChart" ToolTip="Print Chart" runat="server" ImageUrl="~/App_Themes/LAjit/Images/grid-chart.png"
                                                    OnClientClick="return false" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnPDF" ToolTip="Print PDF" runat="server" ImageUrl="~/App_Themes/LAjit/Images/grid-pdf.png"
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnExcel" ToolTip="Print Excel" runat="server" ImageUrl="~/App_Themes/LAjit/Images/grid-excel.png"
                                                    OnClick="imgBtnExcel_Click" OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnHtml" ToolTip="Print HTML" runat="server" ImageUrl="~/App_Themes/LAjit/Images/grid-html.png"
                                                    OnClick="imgBtnHtml_Click" OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnEmailPDF" ToolTip="Email PDF" runat="server" ImageUrl="~/App_Themes/LAjit/Images/email_icon2.png"
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnMSWord" ToolTip="Print MSWord" runat="server" ImageUrl="~/App_Themes/LAjit/Images/Grid_Word.png"
                                                    OnClick="imgBtnMSWord_Click" OnClientClick="return false;" />
                                            </td>
                                            <td width="20%">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" />
                                            </td>
                                        </tr>
                                    </table>--%>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnPrint" ToolTip="Print this page." runat="server" 
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnChart" ToolTip="Print Chart" runat="server" 
                                                    OnClientClick="return false" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnPDF" ToolTip="Print PDF" runat="server" 
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnExcel" ToolTip="Print Excel" runat="server" 
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnHtml" ToolTip="Print HTML" runat="server" 
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnMSWord" ToolTip="Print MSWord" runat="server" 
                                                    OnClientClick="return false;" />
                                            </td>
                                            <td align="left" style="width: 16%">
                                                <asp:ImageButton ID="imgBtnEmailPDF" ToolTip="Email PDF" runat="server" 
                                                    OnClientClick="return false;" />
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
                                                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=curr','<%=this.ClientID %>','ctl00$cphPageContents$PCGVUC$ddlXAxis','ctl00$cphPageContents$PCGVUC$ddlYAxis');">
                                                            Current</a></td>
                                                    <td>
                                                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowChartViewer('Common/ChartViewer.aspx?page=all','<%=this.ClientID %>','ctl00$cphPageContents$PCGVUC$ddlXAxis','ctl00$cphPageContents$PCGVUC$ddlYAxis');">
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
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=PDF&PBD=1','<%=this.ClientID %>');">
                            Current</a>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=PDF&PBD=1','<%=this.ClientID %>');">
                            All Pages</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divHoverPageSelectExcel" style="display: none;">
            <%--   <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnExcelCurrent" runat="server" Text="Current" OnClick="imgBtnExcelCurrent_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnExcelAll" runat="server" Text="All Pages" OnClick="imgBtnExcelAll_Click" />
                        </td>
                    </tr>
                </table>--%>
            <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=EXCEL&PBD=1','<%=this.ClientID %>');">
                            Current</a>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=EXCEL&PBD=1','<%=this.ClientID %>');">
                            All Pages</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divHoverPageSelectHTML" style="display: none;">
            <%--     <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnHTMLCurrent" runat="server" Text="Current" OnClick="imgBtnHTMLCurrent_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnHTMLAll" runat="server" Text="All Pages" OnClick="imgBtnHTMLAll_Click" />
                        </td>
                    </tr>
                </table>--%>
            <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=HTML&PBD=1','<%=this.ClientID %>');">
                            Current</a>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=HTML&PBD=1','<%=this.ClientID %>');">
                            All Pages</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divHoverPageSelectMSWord" style="display: none;">
            <%--<table cellpadding="0" cellspacing="0" class="GridPrintHover">
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnWordCurrent" runat="server" Text="Current" OnClick="imgBtnWordCurrent_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <LCtrl:LAjitLinkButton ID="lnkBtnWordAll" runat="server" Text="All Pages" OnClick="imgBtnWordAll_Click" />
                        </td>
                    </tr>
                </table>--%>
            <table cellpadding="0" cellspacing="0" class="GridPrintHover">
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=curr&RptType=WORD&PBD=1','<%=this.ClientID %>');">
                            Current</a>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <a target="_blank" style="cursor: pointer" href="#" onclick="javascript:return ShowPrintViewer('Common/PrintViewer.aspx?page=all&RptType=WORD&PBD=1','<%=this.ClientID %>');">
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
                        <%--<input type="radio" id="radCurrent" runat="server" value="Current" onclick="javascript:return SentEmail('CURR','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />Current--%>
                    </td>
                    <td align="left" valign="middle">
                        <asp:RadioButton runat="server" ID="radAll1" onclick="javascript:return SentEmail('ALL','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />All
                        Pages
                        <%-- <input type="radio" id="radAll" runat="server" value="All Pages" onclick="javascript:return SentEmail('ALL','ctl00_cphPageContents_GVUC_hdnSearchBPInfo',this);" />All
                            Pages--%>
                    </td>
                </tr>
            </table>
        </div>
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
        <!-- Stores the BPEInfo used by the classs object to request data at any point of time -->
        <asp:HiddenField ID="hdnSearchBPInfo" runat="server" />
        <!-- Specifes whether the generated PDF should be displayed in IFrame(popup) or a new tab -->
        <asp:HiddenField ID="hdnPreviewInPopup" runat="server" />
    </ContentTemplate>
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="imgBtnPDF" />
         <asp:PostBackTrigger ControlID="imgBtnPDFCurrent" />
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
