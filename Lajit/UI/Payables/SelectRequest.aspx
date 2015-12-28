<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SelectRequest.aspx.cs"
    Inherits="LAjitDev.Payables.SelectRequest" %>

    <%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
     <script type="text/javascript">
	    jQuery(function() {
		    jQuery("#ctl00_cphPageContents_txtPaymentDate").datepicker({dateFormat:'mm/dd/y',changeMonth: true,changeYear: true});
		    jQuery("#ctl00_cphPageContents_txtPaymentDate").mask('99/99/99');
   	    });
   	    
   	    //Checks for a checkbox to be selected.
        function ValidateSelection()
        {
            var gridId='ctl00_cphPageContents_CGVUC_grdVwBranch';
            var objGrid = document.getElementById(gridId);
            //Loop and see which rows are selected.
            for (var rowCount = 1; rowCount < objGrid.rows.length ; rowCount++ )
            {
                var trCurrent = objGrid.rows[rowCount];
                //Check if previously this row was deleted.If deleted don't consider.
                if(trCurrent.style.display == "none")
                {
                    continue;
                }
                var checkBoxContainer = GetFirstChild(trCurrent.cells[0]);
                if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == true)
                {
                    return true;
                }
            }
            jqAlert('Please select a row.','No Selection',false);
            //alert('Please select a row.');
            return false;
        }
	</script>
    <asp:Panel ID="pnlContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <!--GV-->
               <td class="tdMainCont">
                    <table class="tblMainCont" cellpadding="0" cellspacing="0" >
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
                        <!--EntryForm tr-->
                        <tr align="left">
                              <td class="tdEntryFrm">
                                <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                    <!-- entry form start  -->
                                    <table id="tblEntryForm" runat="server" style="height: 400px" width="100%" border="0"
                                        cellspacing="0" cellpadding="0" class="formmiddle">
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
                                        <tr id="trProcessLinks" align="right" valign="middle" runat="server">
                                            <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <!--Report Link tr-->
                                        <tr style="height: 24px;" id="trReportLinks" align="right" valign="middle" runat="server">
                                            <td align="right" valign="middle" id="tdReportLinks" runat="server">
                                                <asp:Panel ID="pnlReport" runat="server" Visible="true">
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
                                        <!--Page Controls tr-->
                                        <tr>
                                            <td align="left" valign="top">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                    <tr style="height: 1px;" align="right" valign="middle">
                                                        <td align="right" valign="middle" colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 50%;">
                                                            <!-- Left Table Start -->
                                                            <table cellpadding="0" border="0" cellspacing="0">
                                                                <tr id="trEntryBank" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblEntryBank" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitDropDownList ID="ddlEntryBank" runat="server" MapXML="EntryBank" Width="184px">
                                                                        </LCtrl:LAjitDropDownList>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqEntryBank" runat="server" MapXML="EntryBank"
                                                                            ControlToValidate="ddlEntryBank" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPaymentDate" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblPaymentDate" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <LCtrl:LAjitTextBox ID="txtPaymentDate" runat="server" MapXML="PaymentDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPaymentDate" MapXML="PaymentDate" runat="server"
                                                                                        ControlToValidate="txtPaymentDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountName" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblAccountName" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtAccountName" runat="server" MapXML="AccountName" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAccountName" MapXML="AccountName" runat="server"
                                                                            ControlToValidate="txtAccountName" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trEntryVendor" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblEntryVendor" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitDropDownList ID="ddlEntryVendor" runat="server" MapXML="EntryVendor"
                                                                            Width="184px">
                                                                        </LCtrl:LAjitDropDownList>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqEntryVendor" runat="server" MapXML="EntryVendor"
                                                                            ControlToValidate="ddlEntryVendor" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <LCtrl:LAjitRegularExpressionValidator ID="regEntryVendor" runat="server" ControlToValidate="ddlEntryVendor"
                                                                            MapXML="EntryVendor" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                            ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                            Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCGITotal" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblCGITotal" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtCGITotal" runat="server" MapXML="CGITotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqCGITotal" MapXML="CGITotal" runat="server"
                                                                            ControlToValidate="txtCGITotal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <LCtrl:LAjitRegularExpressionValidator ID="regCGITotal" runat="server" ControlToValidate="txtCGITotal"
                                                                            MapXML="CGITotal" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                            ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                            Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- Left Table End -->
                                                        </td>
                                                        <td valign="top" style="width: 50%;">
                                                            <!-- Right Table Start -->
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblDescription" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                            ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trStartCheck" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblStartCheck" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtStartCheck" runat="server" MapXML="StartCheck" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqStartCheck" MapXML="StartCheck" runat="server"
                                                                            ControlToValidate="txtStartCheck" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <LCtrl:LAjitRegularExpressionValidator ID="regStartCheck" runat="server" ControlToValidate="txtStartCheck"
                                                                            MapXML="StartCheck" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                            ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                            Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAmountEntered" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblAmountEntered" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtAmountEntered" runat="server" MapXML="AmountEntered" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqAmountEntered" MapXML="AmountEntered" runat="server"
                                                                            ControlToValidate="txtAmountEntered" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <LCtrl:LAjitRegularExpressionValidator ID="regAmountEntered" runat="server" ControlToValidate="txtAmountEntered"
                                                                            MapXML="AmountEntered" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                            ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                            Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trControlTotal" align="left" valign="middle" runat="server">
                                                                    <td class="formtext" style="height: 24px; width: 141px" valign="top">
                                                                        <asp:Label runat="server" ID="lblControlTotal" SkinID="Label"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle" style="width: 196px;">
                                                                        <LCtrl:LAjitTextBox ID="txtControlTotal" runat="server" MapXML="ControlTotal" Width="180px"></LCtrl:LAjitTextBox>
                                                                        <LCtrl:LAjitRequiredFieldValidator ID="reqControlTotal" MapXML="ControlTotal" runat="server"
                                                                            ControlToValidate="txtControlTotal" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                            ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                        <LCtrl:LAjitRegularExpressionValidator ID="regControlTotal" runat="server" ControlToValidate="txtControlTotal"
                                                                            MapXML="ControlTotal" ValidationExpression="^[-+]?\d*\.?\d*$" ToolTip="Please enter a numeric value."
                                                                            ErrorMessage="should be numeric" Display="dynamic" ValidationGroup="LAJITEntryForm"
                                                                            Enabled="false"></LCtrl:LAjitRegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- Right Table End -->
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 1px;" align="right" valign="middle">
                                                        <td align="right" valign="middle" colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="InvoiceSelect" />
                                                        </td>
                                                    </tr>
                                                    <!--Submit and Cancel buttons-->
                                                    <tr style="height: 24px">
                                                        <td align="center" colspan="2">
                                                            <table border="0">
                                                                <tr>
                                                                    <td>
                                                                        <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClientClick="javascript:ValidateControls()"
                                                                            Height="18" AlternateText="Submit" Visible="false"></LCtrl:LAjitImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClientClick="javascript:ValidateControls()"
                                                                            Height="18" AlternateText="ContinueAdd" Visible="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClientClick="javascript:ValidateControls()"
                                                                            Height="18" AlternateText="AddClone" Visible="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel"
                                                                            Visible="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 24px">
                                                        <td colspan="2" align="center">
                                                            <table border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton runat="server" ID="imgbtnPrevious" ToolTip="Previous" Visible="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton runat="server" ID="imgbtnNext" ToolTip="Next" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- form entry fields end -->
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                    <table cellpadding="4" cellspacing="4" border="0" width="100%" align="center">
                                        <tr align="center" class="formmiddle">
                                            <td>
                                                <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlProcessEngine" runat="server" Visible="False" Height="511px">
                                    <table cellpadding="4" cellspacing="4" border="0" width="100%" align="center">
                                        <tr align="center" class="formmiddle">
                                            <td>
                                                <asp:Label ID="lblStatus" runat="server" SkinID="LabelMsg"></asp:Label></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />

    <script type="text/javascript">
    function CloseWindow()
    {
        alert('Close');              
        parent.$find("mpePagePopUpBehaviourID").hide();
    }
    
    function LabelClose()
    {
        var lblName=document.getElementById("ctl00_cphPageContents_lblmsg");
        if(lblName.innerText!="")
        {
            lblName.innerText="";
        } 
    }
         
    //Closes the current IFrame and reloads the Parent Window so that the parent Window will request the already 
    //Session['BPINFO']  automatically.
    function CreatePaymentsForPOs()
    {
        var browser=Sys.Browser.name;
        var activeWindow = top;//CloseTopMostFrame();
        //Override Page-Exit comfirmation.
        activeWindow.g_temp=false; 
        if(browser=='WebKit')
        {
            activeWindow.Redirect(activeWindow.location.href);
        }
        else
        {
            activeWindow.location.href=activeWindow.location.href;
        }
    }
    
    </script>

</asp:content>
