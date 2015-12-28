<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ButtonsUserControl.ascx.cs"
    Inherits="LAjitDev.UserControls.ButtonsUserControl" %>
<asp:UpdatePanel ID="updtPnlGrdVw" runat="server" EnableViewState="true" Visible="true">
    <ContentTemplate>
        <table style="width: 50px" cellspacing="0" cellpadding="0" border="0">
            <tr style="height: 15px">
                <td align="left" valign="bottom" style="background-image: url('<%=Application["ImagesCDNPath"]%>images/left-top.png');
                    background-repeat: no-repeat">
                </td>
            </tr>
            <tr valign="top">
                <td valign="top">
                    <asp:Panel ID="pnlBtnsUC" runat="server" Height="540px" CssClass="formmiddle">
                        <table cellpadding="0" cellspacing="0">
                            <tr id="trImgbtnAdd" runat="server" style="height: 60px;" valign="top">
                                <td align="center" valign="top">
                                    <LCtrl:LAjitImageButton ID="imgbtnAdd" runat="server" AlternateText="Add" ToolTip="Add">
                                    </LCtrl:LAjitImageButton>
                                </td>
                            </tr>
                            <tr id="trImgbtnClone" runat="server" style="height: 60px;" valign="top">
                                <td align="center" valign="top">
                                    <LCtrl:LAjitImageButton ID="imgbtnClone" runat="server" AlternateText="Add-Clone"
                                        ToolTip="Add-Clone"></LCtrl:LAjitImageButton>
                                </td>
                            </tr>
                            <tr id="trImgbtnModify" runat="server" style="height: 60px;" valign="top">
                                <td align="center" valign="top">
                                    <LCtrl:LAjitImageButton runat="server" ID="imgbtnModify" AlternateText="Modify" ToolTip="Modify">
                                    </LCtrl:LAjitImageButton>
                                </td>
                            </tr>
                            <tr id="trImgbtnDelete" runat="server" valign="top">
                                <td align="center" valign="top" style="height: 60px;">
                                    <LCtrl:LAjitImageButton runat="server" ID="imgbtnDelete" AlternateText="Delete" ToolTip="Delete" />
                                </td>
                            </tr>
                            <tr id="trImgbtnFind" runat="server" valign="top">
                                <td align="center" valign="top" style="height: 60px;">
                                    <LCtrl:LAjitImageButton runat="server" ID="imgbtnFind" ToolTip="Find" />
                                </td>
                            </tr>
                            <tr id="trImgbtnPrint" runat="server" style="height: 60px;" valign="top">
                                <td align="center" valign="top">
                                    <asp:Panel ID="pnlMenuPrint" runat="server">
                                        <LCtrl:LAjitImageButton runat="server" ID="imgbtnPrint" AlternateText="Print" OnClientClick="javascript:return ShowPDF('ShowPDF.aspx')"
                                            ToolTip="Print" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr id="trImgbtnNote" runat="server" valign="top">
                                <td align="center" valign="top" style="height: 60px;">
                                    <asp:ImageButton runat="server" ID="imgbtnNote" AlternateText="Note" OnClientClick="javascript:return Openframe('Note','Notes.aspx');"
                                        ToolTip="Secure" />
                                </td>
                            </tr>
                            <tr id="trImgbtnSecure" runat="server" valign="top">
                                <td align="center" valign="top" style="height: 60px;">
                                    <asp:ImageButton runat="server" ID="imgbtnSecure" AlternateText="Secure" OnClientClick="javascript:return Openframe('Secure','Secure.aspx');" />
                                </td>
                            </tr>
                            <tr id="trImgbtnAttachment" runat="server" valign="top">
                                <td align="center" valign="top" style="height: 60px;">
                                    <asp:ImageButton runat="server" ID="imgbtnAttachment" AlternateText="Attachment"
                                        OnClientClick="javascript:return Openframe('Attachment','Attachement.aspx');"
                                        ToolTip="Attachment" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnCnfmSbmt" runat="server" Value="False" />
        <!-- To store the current selected/modified/added row to access both in client side & serverside(shanti) -->
        <asp:HiddenField ID="hdnRwToBeModified" runat="server" />
        <!--For Posting the data to the next form(Stores the current Calling Object BPInfo) -->
        <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />
        <!-- Variable to store the client-side Selected Row Info which is made in the child grid view jst to make it as true or false-->
        <asp:HiddenField ID="hdnSelected" runat="server" />
        <!--parentBPInfo and parentGVDataXML stores the parent page info.They are reset to session when child popup is closed-->
        <asp:HiddenField ID="parentBPInfo" runat="server" />
        <asp:HiddenField ID="hdnBranchColsXML" runat="server" />
        <asp:HiddenField ID="hdnFldBPInfo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFldReportName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnNeedToConfirmExit" runat="server" />
        <asp:HiddenField ID="hdnFormInfo" runat="server" />
        <!-- Stores the BPEInfo required while posting to another page upon BPC Icon Click-->
        <asp:HiddenField ID="hdnBPIconBPEINFO" runat="server" />
        <!-- Stores the Branch XML(if any)for client-side handling DDL Change event.(Vendors,Customers,Banks etc.,-->
        <asp:HiddenField ID="hdnBranchXML" runat="server" Value=""/>
        <!-- Stores the Navigation Links BPInfo-->
        <input type="hidden" id="hdnNavBPInfo" name="hdnNavBPInfo" runat="server" />
        <!-- Stores the BPGID and PageInfo and calling object info(Set only for Modify Load case and Accessed in OnFormLinkClick();) -->
        <asp:HiddenField ID="hdnGVBPEINFO" runat="server" />
        <asp:HiddenField ID="hdnPrintInfo" runat="server" />
        <!--Stores the BPINFO of autofill selected item -->
        <asp:HiddenField ID="hdnAutoFillBPEINFO" runat="server" />
        <!--Stores Chart BPINFO of selected area -->
        <asp:HiddenField ID="hdnChartBPEINFO" runat="server" />
        <!-- Stores the column attributes as a string to be accessed in javacsript(shanti)-->
        <asp:HiddenField ID="hdnParentColNode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnCurrAction" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnTimerValue" runat="server"></asp:HiddenField>
        <!-- This would store true or false if the submit action is success or failure-->
        <asp:HiddenField ID="hdnSubmitstatus" runat="server"></asp:HiddenField>
    </ContentTemplate>
</asp:UpdatePanel>
