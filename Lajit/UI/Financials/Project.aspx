<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Project.aspx.cs" Inherits="LAjitDev.Financials.Project" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">

    <script language="javascript">
    function DisplayImage()
    { 
        var hdnFld=document.getElementById("ctl00_cphPageContents_hdnFldTypeOfJob");              
        var ddlTypeOfJob=document.getElementById("ctl00_cphPageContents_ddlTypeOfJob");  
        var arr = hdnFld.value.split(",");       
        for(i = 0; i < arr.length; i++ )
        {
            var selval = arr[i].split("-");
            var image = document.getElementById("ctl00_cphPageContents_imgTypeOfJob");
            if(ddlTypeOfJob.value == selval[0])
            {
                var imgSrc = selval[1];
                image.src="<%=Application["ImagesCDNPath"]%>Images/"+imgSrc;
                image.style.display = "block";
                break;
            } 
            else
            {
                image.style.display = "none";
            }                          
        }                 
    }

    function IsImageOk(img) 
    {
        if (!img.complete)
        {
            return false;
        }
        if (typeof img.naturalWidth != "undefined" && img.naturalWidth == 0)
        {
            return false;
        }
        return true;
    }
    
    </script>

  <%--  <script type="text/javascript" src="../JavaScript/LAjitListBox.js"></script>--%>

    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
                    <tr>
                        <!--Btns Usercontrol-->
                        <td class="tdBtnsUC">
                            <asp:Panel ID="pnlBtnsContent" runat="server" align="left">
                                <BtnsUC:BtnsUserControl ID="BtnsUC" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="width: 5px;">
                            <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                height="1" />
                        </td>
                        <!--Content-->
                        <td class="tdMainCont">
                            <table class="tblMainCont" cellpadding="0" cellspacing="0">
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
                                <tr align="left">
                                    <td valign="top">
                                        <!--Panel CPGV1 Content-->
                                        <asp:Panel ID="pnlGVContent" runat="server" align="left">
                                            <GVUC:GVUserControl ID="GVUC" GridViewType="Default" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <!--EntryForm tr-->
                                <tr align="left">
                                    <td class="tdEntryFrm">
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                            <!-- entry form start -->
                                            <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0"
                                                class="formmiddle">
                                                <!--Pop up header tr-->
                                                <tr style="height: 24px;" runat="server" id="trPopupHeader" visible="false" valign="top">
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                            cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                                                            <tr style="height: 24px;">
                                                                <td id="htcPopEntryForm" runat="server" align="center" class="grdVwRPTitle" style="height: 24px;
                                                                    border-width: 0px">
                                                                    <asp:Label ID="lblPopupEntry" runat="server" Height="24px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <!--ProcessLinks tr-->
                                                <tr style="height: 24px;" id="trProcessLinks" align="right" valign="middle" runat="server">
                                                    <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                        <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <!--Page subject tr-->
                                                <tr id="trSubject" runat="server" visible="false" style="height: 24px; background-color: Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height: 1px;" align="right" valign="middle">
                                                                <td align="right" valign="middle" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="middle" align="left">
                                                                    <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                        OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trNumberID" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblNumberID" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtNumberID" runat="server" MapXML="NumberID" Width="200px"
                                                                        SkinID="txtDisabled"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqNumberID" MapXML="NumberID" runat="server"
                                                                        ControlToValidate="txtNumberID" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trTitleDef" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblTitleDef" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtTitleDef" runat="server" MapXML="TitleDef" Width="200px"
                                                                        SkinID="txtDisabled"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqTitleDef" MapXML="TitleDef" runat="server"
                                                                        ControlToValidate="txtTitleDef" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                                <td rowspan="3" align="left" valign="middle" style="width: 360px">
                                                                    <%--<img id="imgTypeOfJob" style="display:none" alt="Type Of Project" runat="server" />--%>
                                                                    <LCtrl:LAjitImage ID="imgTypeOfJob" runat="server"  />
                                                                </td>
                                                            </tr>
                                                            <tr id="trTypeOfJob" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblTypeOfJob"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlTypeOfJob" runat="server" MapXML="TypeOfJob" Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqTypeOfJob" runat="server" MapXML="TypeOfJob"
                                                                        ControlToValidate="ddlTypeOfJob" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <!--The associated image for this tr is found in the preceeding tr-->
                                                            <tr id="trCenterType" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblCenterType"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlCenterType" runat="server" MapXML="CenterType" Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCenterType" runat="server" MapXML="CenterType"
                                                                        ControlToValidate="ddlCenterType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trCOAType" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblCOAType"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlCOAType" runat="server" MapXML="COAType" Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCOAType" runat="server" MapXML="COAType"
                                                                        ControlToValidate="ddlCOAType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSharedCOA" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblSharedCOA"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitCheckBox ID="chkbxSharedCOA" runat="server" MapXML="SharedCOA" Width="206px">
                                                                    </LCtrl:LAjitCheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trRollupAccount" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblRollupAccount"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlRollupAccount" runat="server" MapXML="RollupAccount"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqRollupAccount" runat="server" MapXML="RollupAccount"
                                                                        ControlToValidate="ddlRollupAccount" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trRollupJob" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblRollupJob"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlRollupJob" runat="server" MapXML="RollupJob" Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqRollupJob" runat="server" MapXML="RollupJob"
                                                                        ControlToValidate="ddlRollupJob" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSelectVATRecovery" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblSelectVATRecovery"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlSelectVATRecovery" runat="server" MapXML="SelectVATRecovery"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectVATRecovery" runat="server" MapXML="SelectVATRecovery"
                                                                        ControlToValidate="ddlSelectVATRecovery" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trCurrencyTypeCompany" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblCurrencyTypeCompany"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                        ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trImgSrc" align="left" valign="middle" runat="server" visible="false">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblImgSrc"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtImgSrc" runat="server" MapXML="ImgSrc" Width="200px" SkinID="txtDisabled"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqImgSrc" MapXML="ImgSrc" runat="server"
                                                                        ControlToValidate="txtImgSrc" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trBPGID" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblBPGID"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtBPGID" runat="server" MapXML="BPGID" Width="200px" SkinID="txtDisabled"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBPGID" MapXML="BPGID" runat="server" ControlToValidate="txtBPGID"
                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                        SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trPageInfo" align="left" valign="middle" runat="server" visible="false">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblPageInfo"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtPageInfo" runat="server" MapXML="PageInfo" Width="200px"
                                                                        SkinID="txtDisabled"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPageInfo" MapXML="PageInfo" runat="server"
                                                                        ControlToValidate="txtPageInfo" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trIsApproved" align="left" valign="middle" runat="server" visible="false">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" ID="lblIsApproved" runat="server"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtIsApproved" runat="server" MapXML="IsApproved"></LCtrl:LAjitTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSelectSubAccount" align="left" valign="middle" visible="true" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" ID="lblSelectSubAccount" runat="server"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitListBox ID="lstBxSelectSubAccount" runat="server" SelectionMode="Multiple"
                                                                        MapXML="SelectSubAccount" XMLType="ParentChild" Height="120" Width="206">
                                                                    </LCtrl:LAjitListBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqSelectSubAccount" runat="server" MapXML="SelectSubAccount"
                                                                        ControlToValidate="lstBxSelectSubAccount" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trBidSoftwareType" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblBidSoftwareType"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlBidSoftwareType" runat="server" MapXML="BidSoftwareType"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBidSoftwareType" runat="server" MapXML="BidSoftwareType"
                                                                        ControlToValidate="ddlBidSoftwareType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trJobStatus" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblJobStatus"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlJobStatus" runat="server" MapXML="JobStatus" Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJobStatus" runat="server" MapXML="JobStatus"
                                                                        ControlToValidate="ddlJobStatus" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <!--space between page controls and submit button-->
                                                            <tr>
                                                                <td style="height: 15px">
                                                                </td>
                                                            </tr>
                                                            <!--Submit and Cancel buttons-->
                                                            <tr style="height: 24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click"
                                                                                    OnClientClick="javascript:return ValidateControls()" Height="18" AlternateText="Submit">
                                                                                </LCtrl:LAjitImageButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click"
                                                                                    OnClientClick="javascript:ValidateControls()" Height="18" AlternateText="ContinueAdd" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click"
                                                                                    OnClientClick="javascript:ValidateControls()" Height="18" AlternateText="AddClone" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:ImageButton runat="server" ID="imgbtnPrevious" ToolTip="Previous" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton runat="server" ID="imgbtnNext" ToolTip="Next" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 10px">
                                                                <td>
                                                                    <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <!-- form entry fields end -->
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
                                            <table cellpadding="4" cellspacing="4" border="1" width="800px" align="center">
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
                <asp:HiddenField ID="hdnFldTypeOfJob" runat="server"></asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
