<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ChartViewer.aspx.cs" Inherits="LAjitDev.Common.ChartViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chart Viewer</title>

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>

    <script type="text/javascript" src="../JavaScript/jquery.dialog.js"></script>

    <script type="text/javascript" src="../JavaScript/Utility.js"></script>

    <script type="text/javascript" src="../JavaScript/Common.js"></script>

    <script type="text/javascript" src="../JavaScript/ChartUserControl.js"></script>--%>

    
</head>
<body onload="HideParentProgress();">
    <form id="aspnetForm" runat="server">
        <asp:ScriptManager ID="ChartViewerScriptMngr" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" bordercolor="red" style="width: 100%;
                    height: 100%;">
                    <tr>
                        <!--GV height: 100%;-->
                        <td class="tdMainCont">
                            <table cellpadding="0" cellspacing="1" border="0" class="tblMainCont">
                                <!--Title tr-->
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlCPGV1Title" runat="server" Width="100%">
                                            <table cellpadding="0" cellspacing="0" class="tblFormTitle">
                                                <tr style="height: 24px;">
                                                    <td class="grdVwCurveLeft">
                                                        <img alt="Spacer" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" height="1" />
                                                    </td>
                                                    <td id="htcCPGV1" runat="server" class="grdVwtitle">
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
                                <tr>
                                    <td class="tdEntryFrm">
                                        <!--Panel CPGV1 Content  pnlGVContent -->
                                        <asp:Panel ID="pnlEntryForm" runat="server" align="left">
                                            <table cellpadding="1" cellspacing="0" style="width: 100%; height: 490px" id="tblentryform"
                                                class="formmiddle">
                                                <tr id="XYDDLEntryform" runat="server">
                                                    <td valign="top">
                                                        <!-- Page level controls x and y axis -->
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td class="formtext" style="height: 24px; width: 100px">
                                                                                <!-- X-Axis -->
                                                                                <asp:Label runat="server" ID="lblXAxis" Text="Group by" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="ddlXAxis" Width="131px" AutoPostBack="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td class="formtext" style="height: 24px; width: 100px">
                                                                                <!-- Y-Axis -->
                                                                                <asp:Label runat="server" ID="lblYAxis" Text="Sum" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="ddlYAxis" Width="131px" AutoPostBack="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr id="trExtendedCols" align="left">
                                                                            <td class="formtext" style="height: 24px; width: 100px">
                                                                                <asp:Label runat="server" ID="lblChartType" Text="Chart Type" SkinID="Label"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList runat="server" ID="ddlChartTypes" Width="131px" AutoPostBack="false">
                                                                                    <asp:ListItem Value="Choose">Choose</asp:ListItem>
                                                                                    <asp:ListItem Value="Area">Area</asp:ListItem>
                                                                                    <asp:ListItem Value="Bar" Selected="true">Bar</asp:ListItem>
                                                                                    <asp:ListItem Value="Bubble">Bubble</asp:ListItem>
                                                                                    <asp:ListItem Value="CandleStick">CandleStick</asp:ListItem>
                                                                                    <asp:ListItem Value="Column">Column</asp:ListItem>
                                                                                    <asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
                                                                                    <asp:ListItem Value="Line">Line</asp:ListItem>
                                                                                    <asp:ListItem Value="Pie">Pie</asp:ListItem>
                                                                                    <asp:ListItem Value="Point">Point</asp:ListItem>
                                                                                    <asp:ListItem Value="Spline">Spline</asp:ListItem>
                                                                                    <asp:ListItem Value="SplineArea">SplineArea</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedArea">StackedArea</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedArea100">StackedArea100</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedBar">StackedBar</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedBar100">StackedBar100</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedColumn">StackedColumn</asp:ListItem>
                                                                                    <asp:ListItem Value="StackedColumn100">StackedColumn100</asp:ListItem>
                                                                                    <asp:ListItem Value="StepLine">StepLine</asp:ListItem>
                                                                                    <asp:ListItem Value="Stock">Stock</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td class="formtext" style="height: 24px; width: 25px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Submit" OnClick="Submit_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <!-- Page level controls x and y axis -->
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:Panel ID="pnlChart" runat="server">
                                                            <CUC:CVUserControl ID="CUC" runat="server" ChartHeight="456" ChartWidth="898" ChartEnableViewState="false" />
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlExportPDF" runat="server">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label runat="server" ID="lblPDF" Text=" Export to PDF" SkinID="Label"></asp:Label>
                                                                        <asp:ImageButton ID="imgbtnExportPDF" runat="server" AlternateText="Print PDF" OnClick="imgbtnExportPDF_Click" />
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <!-- Export PDF table End -->
                                                        <!-- Page panel errors start -->
                                                        <asp:Panel ID="pnlDataError" runat="server" Visible="false">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
                                                                <tr align="center" valign="middle">
                                                                    <td>
                                                                        <asp:Label ID="lblErrMsg" runat="server" SkinID="LabelMsg"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <!-- Page panel errors end -->
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <!-- DDL Error Status Panel start-->
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="2" cellspacing="0" bordercolor="yellow" style="height: 490px;
                                                width: 100%" border="0" class="formmiddle">
                                                <tr align="center" class="formmiddle">
                                                    <td>
                                                        <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <!-- DDL Error Status Panel end-->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnChartCFN" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divPagePopUp">
            <asp:Panel ID="pnlPagePopUp" runat="server" Style="display: none" BorderWidth="0"
                BackColor="white" BorderColor="#000c19" ScrollBars="None">
                <iframe id="iframePage" name="iframePage" visible="false" height="525" width="932"
                    frameborder="0"></iframe>
                <center>
                    <asp:LinkButton ID="lnkBtnCloseIFrame" runat="server" OnClientClick="OnCloseLinkClick();return false;">Close</asp:LinkButton>
                </center>
                <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
