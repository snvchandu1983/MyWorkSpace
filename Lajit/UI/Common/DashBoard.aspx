<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DashBoard.aspx.cs" Inherits="LAjitDev.DashBoard"
    EnableEventValidation="false" MasterPageFile="~/MasterPages/TopLeft.Master"  ValidateRequest="false" %>

<%@ Register Src="~/UserControls/GridViewControl.ascx" TagName="GVUC" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ChartUserControl.ascx" TagName="CUC" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="Server">
    <!--For Posting the data to the next form -->
    <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />
    <!--For Posting the Navigation History through links to the next page.Pages without BtnsUC are suppossed have 
        this variable with the same name as below. -->
    <input type="hidden" id="ctl00_cphPageContents_BtnsUC_hdnNavBPInfo" name="ctl00$cphPageContents$BtnsUC$hdnNavBPInfo" />
    <!-- Stores the grid BPGID and PageInfo(Accessed in OnDBColumnLinkClick();) -->
    <asp:HiddenField ID="hdnFormInfo" runat="server" />
    <asp:HiddenField ID="parentBPInfo" runat="server" />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border-style: double;
        border-color: White;" bordercolor="White" align="left">
        <tr>
            <td id="tdCenterPanel" style="width: 66%;" valign="top" align="center">
                <asp:Panel ID="pnlExpView" runat="server" Width="100%">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="tblFormTitle">
                        <tr style="height: 24px;">
                            <td class="grdVwCurveLeft">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="tdExpViewText" align="center" class="grdVwtitle">
                            </td>
                            <td class="grdVwCurveRight">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="tdExpViewBG" class="grdVwTitleAuto">
                                <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td class="grdVwTitleAuto" style="width: 20px" align="center">
                                <img id="imgExpViewExpand" alt="expandCollapse" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlExpViewContent" runat="server" Width="100%" HorizontalAlign="Center">
                    <asp:Table ID="tblMainContents" runat="server" BorderWidth="0">
                    </asp:Table>
                    <table id="tblButtons" cellpadding="0" cellspacing="0" border="0" style="height: 0px"
                        width="100%">
                        <tr>
                            <td align="center" style="cursor: pointer;">
                                <img id="imgBtnShowHideShrtCuts" src="<%=Application["ImagesCDNPath"]%>Images/show-shortcuts-but.png"
                                    alt="Show/Hide" onclick="ShowHideShrtCuts();" />
                                <img id="imgBtnUpdateShrtCuts" src="<%=Application["ImagesCDNPath"]%>Images/update-shortcuts-but.png"
                                    style="visibility: hidden;" alt="Update" onclick="UpdateShrtCuts();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCPGV2" runat="server" Width="100%">
                    <table width="100%" cellpadding="0" cellspacing="0" class="tblFormTitle">
                        <tr>
                            <td style="width: 5px;" class="grdVwCurveLeft">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGV2" runat="server" align="center" class="grdVwtitle">
                            </td>
                            <td class="grdVwCurveRight">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGV2Auto" class="grdVwTitleAuto">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                <img id="imgCPGV2Expand" alt="curveRight" src="<%=Application["ImagesCDNPath"]%>Images/plus-icon.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCPGV2Content" runat="server" Width="100%" HorizontalAlign="Center" Style="display:none">
                    <table cellspacing="0" cellpadding="0" width="100%" class="RPContent">
                        <tr>
                            <td valign="top" align="center" style="width: 100%">
                                <asp:GridView ID="grdvCenterPanel2" runat="server" OnRowDataBound="grdVw_OnRowDataBound"
                                    SkinID="dshbrdCntPnlGrd">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                <table cellpadding="2" style="padding-left: 1%">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkBtnCPGV2FV" runat="server">Show Full View</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCPGV1" runat="server" Width="100%">
                    <table width="100%" cellpadding="0" cellspacing="0" class="tblFormTitle">
                        <tr>
                            <td style="width: 5px;" class="grdVwCurveLeft">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGV1" runat="server" align="center" class="grdVwtitle">
                            </td>
                            <td class="grdVwCurveRight">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGV1Auto" class="grdVwTitleAuto">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                <img id="imgCPGV1Expand" alt="curveRight" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCPGV1Content" runat="server" Width="100%" HorizontalAlign="Center">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td valign="top" align="center">
                                <div>
                                    <asp:GridView ID="grdvCenterPanel1" runat="server" OnRowDataBound="grdVw_OnRowDataBound"
                                        SkinID="dshbrdCntPnlGrd">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                <table cellpadding="2" style="padding-left: 1%">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkBtnCPGV1FV" runat="server">Show Full View</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </asp:Panel>
                <asp:Panel ID="pnlCPGraphics" Width="100%" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" class="tblFormTitle" border="0">
                        <tr>
                            <td class="grdVwCurveLeft">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGraphics" runat="server" class="grdVwtitle" align="center" width="116">
                                Graphics Panel
                            </td>
                            <td class="grdVwCurveRight">
                                <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td id="htcCPGraphicsAuto" class="grdVwTitleAuto">
                                <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                            </td>
                            <td class="grdVwTitleAuto" style="width: 21px;" align="center">
                                <img id="imgCPGraphicsExpand" alt="Slide" title="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCPGraphicsContent" runat="server" Width="98%" BorderWidth="0" >
                    <asp:Table ID="tblCPGraphics" CellPadding="0" CellSpacing="0" runat="server" HorizontalAlign="Center"
                        Width="100%">
                    </asp:Table>
                    <!-- dundas chart start -->
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <uc2:CUC ID="CUC1" runat="server" ChartHeight="356" ChartWidth="518" ChartEnableViewState="True" />
                            </td>
                        </tr>
                    </table>
                    <!-- dundas chart start -->
                </asp:Panel>
            </td>
            <td style="width: 1%;">
                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="9"
                    height="1" />
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Panel ID="pnlRPGV1" runat="server" Width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRPGV1Title" runat="server" Width="100%" Height="24px">
                                    <table cellpadding="0" cellspacing="0" class="RPHeader">
                                        <tr>
                                            <td style="width: 5%" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td id="htcRPGV1" style="width: 87%" runat="server" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td style="width: 8%;" class="grdVwRPTitle" align="center">
                                                <img id="imgRPGV1Expand" alt="Slide" title="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRPGV1Content" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table class="RPContent" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" style="width: 100%">
                                                <asp:GridView ID="grdVwRightPanel1" runat="server" OnRowDataBound="grdVw_OnRowDataBound"
                                                    SkinID="dshbrdRgtPnlGrd">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="2" style="padding-left: 1%">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkBtnRPGV1FV" runat="server">Show Full View</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 3px;">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRPGV2" runat="server" Width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRPGV2Title" runat="server" Width="100%" Height="24px">
                                    <table cellpadding="0" cellspacing="0" class="RPHeader" border="0">
                                        <tr>
                                            <td width="5%" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td id="htcRPGV2" style="width: 87%;" runat="server" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td style="width: 8%;" align="center" class="grdVwRPTitle">
                                                <img id="imgRPGV2Expand" alt="Slide" title="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRPGV2Content" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table class="RPContent" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" width="100%">
                                                <div style="height: auto;">
                                                    <asp:GridView ID="grdVwRightPanel2" runat="server" SkinID="dshbrdRgtPnlGrd" OnRowDataBound="grdVw_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="2" style="padding-left: 1%">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkBtnRPGV2FV" runat="server">Show Full View</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 3px;">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRPGV3" runat="server" Width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRPGV3Title" runat="server" Width="100%" Height="24px">
                                    <table cellpadding="0" cellspacing="0" class="RPHeader">
                                        <tr>
                                            <td style="width: 5%;" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td id="htcRPGV3" style="width: 87%;" runat="server" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td style="width: 8%;" class="grdVwRPTitle" align="center">
                                                <img id="imgRPGV3Expand" alt="Slide" title="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRPGV3Content" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table class="RPContent" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" width="100%">
                                                <div style="height: auto;">
                                                    <asp:GridView ID="grdVwRightPanel3" runat="server" SkinID="dshbrdRgtPnlGrd" OnRowDataBound="grdVw_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="2" style="padding-left: 1%">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkBtnRPGV3FV" runat="server">Show Full View</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 3px;">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRPGV4" runat="server" Width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRPGV4Title" runat="server" Width="100%" Height="24px">
                                    <table cellpadding="0" cellspacing="0" class="RPHeader">
                                        <tr>
                                            <td style="width: 5%;" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td id="htcRPGV4" style="width: 87%;" runat="server" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td style="width: 8%;" class="grdVwRPTitle" align="center">
                                                <%--<asp:Image ID="imgRPGV4Expand" runat="server" ImageUrl="../App_Themes/<%=Session['MyTheme']%>/Images/minus-icon.png" />--%>
                                                <img id="imgRPGV4Expand" alt="Slide" title="Slide" src='<%=Application["ImagesCDNPath"]%>Images/minus-icon.png' />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRPGV4Content" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table class="RPContent" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" width="100%">
                                                <div style="height: auto;">
                                                    <asp:GridView ID="grdVwRightPanel4" runat="server" SkinID="dshbrdRgtPnlGrd" OnRowDataBound="grdVw_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnRowInfo" runat="server"></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="2" style="padding-left: 1%">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkBtnRPGV4FV" runat="server" Text="Show Full View" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 3px;">
                                                <%--<img src="../App_Themes/LAjit/Images/spacer.gif" height="3px" border="0">--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRPGV5" runat="server" Width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRPGV5Title" runat="server" Width="100%" Height="24px">
                                    <table cellpadding="0" cellspacing="0" class="RPHeader">
                                        <tr>
                                            <td style="width: 5%;" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td id="htcRPGV5" style="width: 87%;" runat="server" class="grdVwRPTitle" align="left">
                                            </td>
                                            <td style="width: 8%;" class="grdVwRPTitle" align="center">
                                                <img id="imgRPGV5Expand" alt="Slide" title="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRPGV5Content" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table class="RPContent" style="overflow: scroll" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" width="100%" valign="top">
                                                <asp:Literal runat="server" ID="litRss"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 3px;">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="updtPnlRssTimer">
                    <contenttemplate>
                        <asp:Timer ID="timerRSSUpdater" runat="server" Enabled="false" OnTick="timerRSSUpdater_Tick">
                        </asp:Timer>
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript">

    jQuery(document).ready(function()
    {  
        var arrPnlLst=new Array();
        arrPnlLst.push('ctl00_cphPageContents_pnlExpView');
        arrPnlLst.push('ctl00_cphPageContents_pnlCPGV1');
        arrPnlLst.push('ctl00_cphPageContents_pnlCPGV2');
        arrPnlLst.push('ctl00_cphPageContents_pnlCPGraphics');
        arrPnlLst.push('ctl00_cphPageContents_pnlRPGV1Title');
        arrPnlLst.push('ctl00_cphPageContents_pnlRPGV2Title');
        arrPnlLst.push('ctl00_cphPageContents_pnlRPGV3Title');
        arrPnlLst.push('ctl00_cphPageContents_pnlRPGV4Title');
        arrPnlLst.push('ctl00_cphPageContents_pnlRPGV5Title');
        jQuery.each(arrPnlLst, function(index, item){jQuery('#'+item).click(function(){ExpandCollapse(item);});});
       // jQuery('#floating_box').hide();
        
    });

    function ExpandCollapse(oTitleID)
    {
       var oImgID; 
       var TitleID=oTitleID.split('_');
       var oName=TitleID[2].replace('Title','');
       var oContentID='ctl00_cphPageContents_'+oName+'Content';
       var i=oName.indexOf('pnl');
       if(i!==-1)
       {
        i=i+3;
        var j=oName.length;
        oImgID='img'+oName.substring(i,j)+'Expand';
       }
       jQuery('#'+oContentID).slideToggle('slow');
       var imgIcon=document.getElementById(oImgID).getAttribute('src'); 
       var imgIconSrc=imgIcon.split('/');
       if(imgIconSrc[imgIconSrc.length-1]=="plus-icon.png")
       {
           jQuery("#"+oImgID).attr("src",g_cdnImagesPath+"Images/minus-icon.png"); 
       }
       else
       {
           jQuery("#"+oImgID).attr("src",g_cdnImagesPath+"Images/plus-icon.png"); 
       }
    }  
    
    function OnGraphicsPnlLinkClick(processBPGID, redirectPage)
    {
        redirectPage = "../" + redirectPage;
        document.getElementById('hdnBPInfo').value = "<bpinfo><BPGID>" + processBPGID + "</BPGID></bpinfo>";
        Redirect(redirectPage);
    }
    </script>

</asp:Content>
