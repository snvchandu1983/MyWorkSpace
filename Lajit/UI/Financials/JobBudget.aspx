<%@ Page Language="C#" AutoEventWireup="true" Codebehind="JobBudget.aspx.cs" Inherits="LAjitDev.Financials.JobBudget" %>

<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
            <contenttemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <!--Btns Usercontrol-->
                   <td class="tdBtnsUC">
                         <BtnsUC:BtnsUserControl ID="BtnsUC" runat="server" />
                    </td>
                    <td style="width: 5px;">
                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                            height="1" />
                    </td>
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
                                    <asp:Panel ID="pnlEntryForm" runat="server"  Height="511px">
                                        <!-- entry form start -->
                                        <table id="tblEntryForm" runat="server" style="height: 511px;width:100%" border="0" cellspacing="0"
                                            cellpadding="0" class="formmiddle">
                                            <!--Pop up header tr-->
                                            <tr style="height: 24px;" runat="server" id="trPopupHeader" visible="false">
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" class="plainHeaderTitle">
                                                        <tr>
                                                            <td id="htcPopEntryForm" runat="server" valign="middle" align="center" class="grdVwRPTitle">
                                                                <asp:Label ID="lblPopupEntry" runat="server"></asp:Label>
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
                                                <td align="left" valign="top" >
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                       <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                           <td align="right" valign="middle" colspan="2">
                                                               &nbsp;
                                                           </td>
                                                        </tr>
                                                        <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                            <td valign="middle" colspan="2" align="center">
                                                                <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server" MapXML="SoxApprovedStatus"
                                                                    OnClick="imgbtnIsApproved_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                                <td valign="top" style="width: 50%;">
                                                                   <!-- Left Table Start -->
                                                                 <table cellpadding="0" cellspacing="0" border="0" >
                                                                         <!-- moved from right table start -->
                                                                             <tr id="trAICPGroup" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label runat="server" ID="lblAICPGroup"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtAICPGroup" runat="server" MapXML="AICPGroup" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqAICPGroup" MapXML="AICPGroup" runat="server" ControlToValidate="txtAICPGroup" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           
                                                                           <tr id="trCaption" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblCaption"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtCaption" runat="server" MapXML="Caption" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCaption" MapXML="Caption" runat="server" ControlToValidate="txtCaption" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           
                                                                            <tr id="trBidContract" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblBidContract"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtBidContract" runat="server" MapXML="BidContract" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBidContract" MapXML="BidContract" runat="server" ControlToValidate="txtBidContract" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr> 
                                                                              <tr id="trProdActual" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblProdActual"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtProdActual" runat="server" MapXML="ProdActual" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProdActual" MapXML="ProdActual" runat="server" ControlToValidate="txtProdActual" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           
                                                                            <tr id="trCommitPO" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblCommitPO"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtCommitPO" runat="server" MapXML="CommitPO" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCommitPO" MapXML="CommitPO" runat="server" ControlToValidate="txtCommitPO" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                         <!-- moved from right table end -->
                                                                 
                                                                            <tr id="trBudetType" style="height: 24px" align="left" valign="top" runat="server">
                                                                                <td valign="middle" class="formtext" style="height:24px; width:125px;">
                                                                                    <asp:Label runat="server" ID="lblBudetType" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlBudetType" runat="server" MapXML="BudetType" Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBudetType" runat="server" MapXML="BudetType"
                                                                                        ControlToValidate="ddlBudetType" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                                <td class="formtext">
                                                                                    <asp:Label runat="server" ID="lblDescription" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="180px"></LCtrl:LAjitTextBox>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                                        ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trCurrencyTypeCompany" style="height: 24px">
                                                                                <td class="formtext">
                                                                                    <asp:Label runat="server" ID="lblCurrencyTypeCompany" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LAjitDropDownList ID="ddlCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                        Width="184px">
                                                                                    </LCtrl:LAjitDropDownList>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqCurrencyTypeCompany" runat="server" MapXML="CurrencyTypeCompany"
                                                                                        ControlToValidate="ddlCurrencyTypeCompany" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                          
                                                                           <tr id="trYearEndDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext">
                                                                                    <asp:Label runat="server" ID="lblYearEndDate" SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                     <td>
                                                                                        <LCtrl:LAjitTextBox ID="txtYearEndDate" runat="server" MapXML="YearEndDate" Width="68px"></LCtrl:LAjitTextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqYearEndDate" MapXML="YearEndDate" runat="server"
                                                                                        ControlToValidate="txtYearEndDate" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                                    </td>
                                                                                    </tr>
                                                                                    </table>    
                                                                                </td>
                                                                            </tr>
                                                                      </table>
                                                                   <!-- Left Table End -->      
                                                                </td>
                                                               <td valign="top" style="width: 50%;">
                                                                   <!-- Right Table Start -->
                                                                        <table cellpadding="0" border="0" cellspacing="0" >
                                                                           <tr id="trPaidToDate" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblPaidToDate"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtPaidToDate" runat="server" MapXML="PaidToDate" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqPaidToDate" MapXML="PaidToDate" runat="server" ControlToValidate="txtPaidToDate" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           
                                                                           <tr id="trTotalCost" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblTotalCost"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtTotalCost" runat="server" MapXML="TotalCost" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqTotalCost" MapXML="TotalCost" runat="server" ControlToValidate="txtTotalCost" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           <tr id="trBudgetVariance" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblBudgetVariance"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtBudgetVariance" runat="server" MapXML="BudgetVariance" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqBudgetVariance" MapXML="BudgetVariance" runat="server" ControlToValidate="txtBudgetVariance" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                            </tr>    
                                                                           <tr id="trProdVariance" align="left" valign="middle" runat="server">
                                                                                <td class="formtext" style="height: 24px; width: 125px" valign="top">
                                                                                    <asp:Label   runat="server" ID="lblProdVariance"  SkinID="Label"></asp:Label>
                                                                                </td>
                                                                                <td valign="middle" style="width:196px; ">
                                                                                    <LCtrl:LajitTextBox ID="txtProdVariance" runat="server" MapXML="ProdVariance" Width="180px" ></LCtrl:LajitTextBox>                                                                                     
                                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqProdVariance" MapXML="ProdVariance" runat="server" ControlToValidate="txtProdVariance" ValidationGroup="LAJITEntryForm"
                                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                                </td>
                                                                           </tr>    
                                                                    </table>
                                                                   <!-- Right Table End -->      
                                                                </td>
                                                            </tr>
                                                        
                                                       <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                           <td align="right" valign="middle" colspan="2">
                                                               &nbsp;
                                                           </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td colspan="4">
                                                                <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="BudgetTotal" />
                                                            </td>
                                                        </tr>
                                                        <!--space between page controls and submit button--> 
                                                        <tr>
                                                            <td style="height:15px"></td>
                                                        </tr>
                                                        <!--Submit and Cancel buttons-->                                                        
                                                 <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="2" align="center">
                                                                     <table border="0">
                                                                   <tr>
                                                                        <td>
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click" OnClientClick="javascript:return ValidateControls()" 
                                                                              Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnContinueAdd" OnClick="imgbtnContinueAdd_Click" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="ContinueAdd" />
                                                                        </td>
                                                                       <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnAddClone" OnClick="imgbtnAddClone_Click" OnClientClick="javascript:ValidateControls()" 
                                                                              Height="18" AlternateText="AddClone"/>
                                                                        </td> 
                                                                        <td>
                                                                            <asp:ImageButton runat="server" ID="imgbtnCancel" Height="18" AlternateText="Cancel"/>
                                                                        </td>
                                                                       </tr>
                                                                </table>
                                                               </td>                                                                                                                                   
                                                             </tr>
                                                            <tr style="height:10px">
                                                                <td>
                                                                    <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                                </td>
                                                            </tr>
                                                             <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="2" align="center">
                                                                     <table border="0">
                                                                    <tr>
                                                                     <td align="right"> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnPrevious" Tooltip="Previous"/>
                                                                    </td>
                                                                     <td align="left"> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnNext" Tooltip="Next"/>
                                                                    </td> 
                                                                   </tr> 
                                                                   </table>
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
                                    <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                        <table cellpadding="4" cellspacing="4" border="0" width="800px" align="center">
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
        </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>
