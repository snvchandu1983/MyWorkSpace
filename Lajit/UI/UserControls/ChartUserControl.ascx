<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ChartUserControl.ascx.cs"
    Inherits="LAjitDev.UserControls.ChartUserControl" %>
<%@ Register TagPrefix="dcwc" Namespace="Dundas.Charting.WebControl" Assembly="DundasWebChart" %>

<%--<script language="javascript" type="text/javascript" src="../JavaScript/ChartUserControl.js"></script>--%>

<asp:UpdatePanel ID="updtPnlChartUC" runat="server">
    <ContentTemplate>
        <table cellpadding="1" cellspacing="0" border="0">
            <tr>
                <td valign="top">
                    <dcwc:Chart ID="centrChart" EnableViewState="true" runat="server" BackColor="#D3DFF0"
                        Width="468px" Height="356px" BorderLineColor="26, 59, 105" Palette="Dundas" BorderLineStyle="Solid"
                        BackGradientType="TopBottom" BackGradientEndColor="White" BorderLineWidth="2">
                        <Titles>
                            <dcwc:Title Name="Default" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold"
                                ShadowOffset="3" Color="26, 59, 105">
                            </dcwc:Title>
                        </Titles>
                       <%-- <Series>
                            <dcwc:Series Name="Default">
                            </dcwc:Series>
                        </Series>--%>
                        <ChartAreas>
                            <dcwc:ChartArea Name="Default" BorderWidth="0">
                            </dcwc:ChartArea>
                        </ChartAreas>
                    </dcwc:Chart>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlErrmsg" runat="server" Visible="false">
            <table cellpadding="0" cellspacing="0" border="0" style="height: 356px; width: 100%">
                <tr>
                    <td valign="middle" align="center">
                        <asp:Label ID="lblmsg" runat="server" Visible="false" SkinID="LabelMsg"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
