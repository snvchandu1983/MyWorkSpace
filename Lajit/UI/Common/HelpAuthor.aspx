<%@ Page Language="C#" AutoEventWireup="true" Codebehind="HelpAuthor.aspx.cs" Inherits="LAjitDev.Common.AuthorHelp" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <script type="text/javascript">
    
        function OnFileTypeChange(ddlRef,txtHelp,fileUpHelp)
        {
            var ddlSelText=ddlRef.options[ddlRef.selectedIndex].text;
            var objtxtFileAttachment=document.getElementById('ctl00_cphPageContents_txtFileAttachment');
            var objtxtHelpFile=document.getElementById('ctl00_cphPageContents_txtHelpFile');
      
            if(ddlSelText.toUpperCase().indexOf('URL')!=-1)
            {
                objtxtFileAttachment.style.display = 'none';
                objtxtHelpFile.style.display = '';
                
                objtxtHelpFile.value='';//Clear any previous File Names present in it.
            }
            else
            {
                objtxtFileAttachment.style.display = '';
                objtxtHelpFile.style.display = 'none';
            }
        }
        
    </script>

    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <Triggers>
                <asp:PostBackTrigger ControlID="imgbtnSubmit" />
                <asp:PostBackTrigger ControlID="imgbtnContinueAdd" />
                <asp:PostBackTrigger ControlID="imgbtnAddClone" />
            </Triggers>
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
                        <!--GV-->
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
                                    <td valign="top">
                                        <asp:Panel ID="pnlEntryFormTitle" runat="server" Width="100%" Visible="False">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                cursor: hand; border-right-width: 1px; border-right-style: double; border-right-color: #d7e0f1;">
                                                <tr style="height: 24px;">
                                                    <td style="width: 5px;" class="grdVwCurveLeft">
                                                    </td>
                                                    <td id="htcEntryForm" runat="server" align="center" class="grdVwtitle" style="height: 24px;
                                                        border-width: 0px">
                                                    </td>
                                                    <td style="width: 6px;" class="grdVwCurveRight">
                                                    </td>
                                                    <td id="htcEntryFormAuto" class="grdVwTitleAuto" runat="server" style="height: 24px;">
                                                    </td>
                                                    <td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center">
                                                        <%--<img id="img1" alt="Slide" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
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
                                                <tr style="height: 24px;">
                                                    <td align="right">
                                                        <table cellpadding="0" cellspacing="0" border="0" style="height: 24px;">
                                                            <tr style="height: 24px;" id="trProcessLinks" align="right" valign="middle" runat="server">
                                                                <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                                    <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                                    <td align="left" valign="top" style="height: 439px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td style="width: 160px; height: 24px" class="formtext">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                    OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trDisplaySeq" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblDisplaySeq" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtDisplaySeq" runat="server" MapXML="DisplaySeq" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDisplaySeq" MapXML="DisplaySeq" runat="server"
                                                                        ControlToValidate="txtDisplaySeq" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpCatalog" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpCatalog" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtHelpCatalog" runat="server" MapXML="HelpCatalog" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpCatalog" MapXML="HelpCatalog" runat="server"
                                                                        ControlToValidate="txtHelpCatalog" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpFileCaption">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpFileCaption" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlHelpFileCaption" runat="server" MapXML="HelpFileCaption"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpFileCaption" runat="server" MapXML="HelpFileCaption"
                                                                        ControlToValidate="ddlHelpFileCaption" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpCaption" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpCaption" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtHelpCaption" runat="server" MapXML="HelpCaption" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpCaption" MapXML="HelpCaption" runat="server"
                                                                        ControlToValidate="txtHelpCaption" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpText" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpText" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtHelpText" runat="server" MapXML="HelpText" Width="400px"
                                                                        TextMode="MultiLine" Rows="6"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpText" MapXML="HelpText" runat="server"
                                                                        ControlToValidate="txtHelpText" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpFile" align="left" valign="middle" runat="server" style="margin: 30px">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpFile" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:FileUpload ID="txtFileAttachment" runat="server" BackColor="white" Width="200px" size="30" />
                                                                    <LCtrl:LAjitTextBox ID="txtHelpFile" runat="server" MapXML="HelpFile" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpFile" MapXML="HelpFile" runat="server"
                                                                        ControlToValidate="txtFileAttachment" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <%--<LCtrl:LAjitRegularExpressionValidator 
                                                                             id="regHelpFile" runat="server" 
                                                                             ErrorMessage="Only upload valid files" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.flv|.FLV|.mpeg|.MPEG|.mpg|.MPG)$" ControlToValidate="txtFileAttachment">
                                                                            </LCtrl:LAjitRegularExpressionValidator>--%>

                                                                </td>
                                                            </tr>
                                                            <tr id="trHelpFileType">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblHelpFileType" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlHelpFileType" runat="server" MapXML="HelpFileType"
                                                                        Width="206px">
                                                                    </LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqHelpFileType" runat="server" MapXML="HelpFileType"
                                                                        ControlToValidate="ddlHelpFileType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
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
                                                                <td style="height: 24px;" colspan="2" align="center">
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
                                                                <td style="height: 24px;" colspan="2" align="center">
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
                                                                <td colspan="2">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
