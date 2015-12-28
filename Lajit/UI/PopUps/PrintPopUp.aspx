<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PrintPopUp.aspx.cs" Inherits="LAjitDev.Financials.PrintPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Generate Report</title>
</head>

<script language="javascript" type="text/javascript">
   
    function ShowHideMPPrintCreteria(mpePrintCriteriaBehavID,action)
     {
        switch (action)
        {
            case "SHOW":
                $find(mpePrintCriteriaBehavID).show();
                break;
            case "HIDE":
                $find(mpePrintCriteriaBehavID).hide();
                break;
        }  
        return true;             
     }
       
    //Visibility of Report Info dropdown and Retrieve rpt dropdown
    function ShowHideRptOptions()
    {                   
        var SaveReport=document.getElementById("lstSaveToMyReports");     
        var ReportInfo = document.getElementById("trDistribution");        
        var RetrieveRpt = document.getElementById("trChkRptRetrieve");
        var ChkRetrieveRpt = document.getElementById("lstRetrieveRpt");
        var ChkReportInfo = document.getElementById("lstDistribution");
        
        if(SaveReport.value=="YES")
        {              
            ReportInfo.style.visibility =  "visible";
            RetrieveRpt.style.visibility ="visible";
        }
        else
        {
            //Changing the lstRetrieveRpt selected value to "NO"
            ChkRetrieveRpt.selectedIndex=1;
            ChkReportInfo.selectedIndex=0;
            ReportInfo.style.visibility = "hidden";            
            RetrieveRpt.style.visibility = "hidden";       
        }
    }
        
    //Report Panel                 
    function Pageload()
    {    
        var hdnreq = parent.document.getElementById('ctl00_cphPageContents_BtnsUC_hdnFldBPInfo');
        //var hdnret = parent.document.getElementById('ctl00_cphPageContents_BtnsUC_hdnFldBPOut'); 
        //var hdnrptName= parent.document.getElementById('ctl00_cphPageContents_BtnsUC_hdnFldReportName');
        document.getElementById('hdnGVReqXml').value = hdnreq.value;
//        document.getElementById('hdnGVRetXml').value = hdnret.value;  
//        document.getElementById('hdnReportName').value= hdnrptName.value;
        return false; 
    }
    
    function Clickheretoprint()
    { 
      //window.open("PrinterFriendly.aspx"); 
      self.print(); 
    }


function HideParentProgress()
{
    var divLayer = parent.document.getElementById('layer');
    var divImage = parent.document.getElementById('box');
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
<body onload="HideParentProgress();">
    <form id="frmPrintPopUp" runat="server">
        <asp:ScriptManager ID="pnlPrintScriptMgr" runat="server" EnablePartialRendering="true">
            <Scripts>
                <%--<asp:ScriptReference Name="MicrosoftAjax.js" ScriptMode="Auto" Path="../JavaScript/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.js" />
                <asp:ScriptReference Name="MicrosoftAjax.debug.js" ScriptMode="Auto" Path="../JavaScript/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js" />--%>
            </Scripts>
        </asp:ScriptManager>
        <asp:Panel runat="server" ID="pnlTotalPrint" Visible="true">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 925px; height: 400px;">
                <!--Heading-->
                <tr valign="top">
                    <td>
                        <asp:Panel runat="server" ID="pnlChooseOptions" Style="display: block">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                                <tr style="height: 24px;">
                                    <td align="center" class="grdVwRPTitle" style="height: 24px; border-width: 0px">
                                        <asp:Label ID="lblPopupEntry" runat="server" Text="Choose Output Options"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <!--Spacer-->
                <tr style="height: 3px">
                    <td>
                        <%--<img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" height="3" />--%>
                    </td>
                </tr>
                <!-- Content -->
                <tr style="height: 435" valign="top">
                    <td style="width: 100%; height: 100%">
                        <table cellpadding="0" cellspacing="0" border="0" style="border-color: Red; width: 100%;
                            height: 435">
                            <tr>
                                <!--Spacer-->
                                <td style="width: 3px; height: 100%">
                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="3"
                                        height="1" />
                                </td>
                                <!--Operations Panel-->
                                <asp:Panel runat="server" ID="pnlImgButtons">
                                    <td valign="top" align="center" style="width: 52px; height: 435px" class="formmiddle">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 435">
                                            <tr style="height: 60px;" valign="top" id="trPDF" runat="server">
                                                <td align="center">
                                                    <asp:ImageButton runat="server" ID="imgbtnPdf" OnClick="imgbtnPdf_Click" AlternateText="PDF"
                                                        OnClientClick="Pageload()" />
                                                </td>
                                            </tr>
                                            <tr style="height: 60px;" valign="top" id="trExcel" runat="server">
                                                <td align="center" valign="top">
                                                    <asp:ImageButton runat="server" ID="imgbtnExcel" OnClick="imgbtnExcel_Click" AlternateText="EXCEL"
                                                        OnClientClick="Pageload()" />
                                                </td>
                                            </tr>
                                            <tr style="height: 60px;" valign="top" id="trHtml" runat="server">
                                                <td align="center" valign="top">
                                                    <asp:ImageButton runat="server" ID="imgbtnHtml" OnClick="imgbtnHtml_Click" AlternateText="HTML"
                                                        OnClientClick="Pageload()" />
                                                </td>
                                            </tr>
                                            <tr style="height: 20px; width: 20px" valign="top" id="trPrint" runat="server">
                                                <td align="center" valign="top">
                                                    <asp:ImageButton runat="server" ID="imgbtnPrintOption" AlternateText="Print" OnClick="imgbtnPrintOption_Click" />
                                                    <%--OnClientClick="javascript:return Clickheretoprint()" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </asp:Panel>
                                <!--Spacer-->
                                <td style="width: 3px; height: 100%">
                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="3"
                                        height="1" />
                                </td>
                                <!--Inner Panel-->
                                <td valign="top" style="height: 435px" class="formmiddle">
                                    <table border="0" cellpadding="2" cellspacing="0" width="840px">
                                        <!--Spacer-->
                                        <tr>
                                            <td colspan="2">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="3"
                                                    height="5px" />
                                            </td>
                                        </tr>
                                        <asp:Panel runat="server" ID="pnlCurrentAll" Visible="true">
                                            <tr id="trPage" align="left" valign="top">
                                                <td class="formtext" style="width: 50%">
                                                    <asp:Label runat="server" ID="lblPage" Text="Select Page Info" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td valign="top" align="left" style="width: 50%">
                                                    <select name="Page" id="lstPage" runat="server" style="width: 103px">
                                                        <option id="pageYes" value="Current" runat="server">Current Page</option>
                                                        <option id="pageNo" value="All" runat="server" selected="selected">All Pages</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlExtendedCols" Visible="true">
                                            <tr id="trExtendedCols" align="left">
                                                <td class="formtext" style="width: 50%">
                                                    <asp:Label runat="server" ID="lblExtendedCols" Text="Print Extended Data" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td valign="top" align="left" style="width: 50%">
                                                    <select name="ExtendedCols" id="lstExtendedCols" runat="server" style="width: 103px">
                                                        <option id="extColsYes" value="YES" runat="server">Yes</option>
                                                        <option id="extColsNo" value="NO" runat="server" selected="selected">No</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlSavetoMyReports" Visible="true">
                                            <tr id="trSaveToMyReports" align="left" valign="top">
                                                <td class="formtext" style="width: 50%">
                                                    <asp:Label runat="server" ID="lblSaveToMyReports" Text="Save To My Reports" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" style="width: 50%">
                                                    <select name="SaveToMyReports" id="lstSaveToMyReports" runat="server" style="width: 103px"
                                                        onchange="ShowHideRptOptions()">
                                                        <option id="saverptYes" value="YES" runat="server">Yes</option>
                                                        <option id="saverptNo" value="NO" runat="server" selected="selected">No</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlDistributionList" Visible="true">
                                            <tr id="trDistribution" align="left" valign="top" style="visibility: hidden;">
                                                <td class="formtext" style="width: 50%">
                                                    <asp:Label ID="lblDistribution" runat="server" Text="Distribution" CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td align="left" valign="top">
                                                    <select name="Distribution" id="lstDistribution" runat="server" style="width: 103px">
                                                    </select>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlChkReportRetrieve" Visible="true">
                                            <tr id="trChkRptRetrieve" align="left" valign="top" style="visibility: hidden;">
                                                <td class="formtext" style="width: 50%">
                                                    <asp:Label runat="server" ID="lblChkRptRetrieve" Text="Would You Like To View Your Report ?"
                                                        CssClass="mbodyb"></asp:Label>
                                                </td>
                                                <td valign="middle" id="tdChkRptRetrieve" runat="server" align="left">
                                                    <select name="RetrieveRpt" id="lstRetrieveRpt" runat="server" style="width: 103px">
                                                        <option id="retrieveYes" value="YES" runat="server">Yes</option>
                                                        <option id="retrieveNo" value="NO" runat="server" selected="selected">No</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <!--Label Message-->
                                        <asp:Panel runat="server" ID="pnllblMsg" Visible="true">
                                            <tr id="trMsg" align="left" valign="top">
                                                <td colspan="2" align="center" valign="top">
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="msummary"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlPrintOptionBtn" Visible="true">
                                            <tr id="trPrintbtnOption" align="left" valign="top">
                                                <td colspan="2" align="center" valign="top">
                                                    <asp:Literal runat="server" ID="litPrintOptionBtn"></asp:Literal>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrinting" Visible="true">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 925px; height: 400px;
                border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                <tr id="trPrinting" align="left" valign="top">
                    <td colspan="2" align="center" valign="top">
                        <asp:Literal runat="server" ID="litPrinting"></asp:Literal>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:HiddenField ID="hdnGVReqXml" runat="server" />
        <asp:HiddenField ID="hdnGVRetXml" runat="server" />
        <asp:HiddenField ID="hdnShowPDF" runat="server" />
        <asp:HiddenField ID="hdnReportName" runat="server" />
        <asp:HiddenField ID="hdntest" runat="server" />
        <asp:HiddenField runat="server" ID="hdnPrint" />
        <input type="hidden" runat="server" id="hdnPrintReport" />
    </form>
</body>
</html>
