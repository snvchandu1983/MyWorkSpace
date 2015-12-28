<%@ Page Language="C#" MasterPageFile="~/MasterPages/TopLeft.Master" AutoEventWireup="true" CodeBehind="COAEntry.aspx.cs" Inherits="LAjitDev.Financials.COAEntry" Title="Untitled Page"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">
<script type="text/javascript">
        var styleToSelect;
        function onOk() {
            ///$get('Paragraph1').className = styleToSelect;
            alert("OK event fire");
        }
        // Add click handlers for buttons to show and hide modal popup on pageLoad
        function pageLoad() {
            $addHandler($get("showModalPopupClientButton"), 'click', showModalPopupViaClient);
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);        
        }
        
        function showModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }
        
        function hideModalPopupViaClient(ev) {
            ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
    </script>    

<!-- COA Entry Start -->
<!-- inner table start -->
<table width="100%"  border="0" bordercolor="yellow" cellspacing="0" cellpadding="0">
     <tr align="left" valign="top">
            <td style="width:46px;"><table width="100%"  border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td align="left" valign="top" style="height:7px; height:21px; background-color:#c5d5fc;">
                    <img src="/App_Themes/<%=Session["MyTheme"]%>/images/left-top.gif" width="46" height="7"></td>
              </tr>
              <tr>
                <td align="left" valign="top">
                 <table width="100%"  border="0" cellpadding="0" cellspacing="0" class="formmiddle">
                  <tr>
                    <td align="left" valign="top">
                    <!-- left icons start -->
                     <table width="100%"  cellspacing="0" cellpadding="0" border="0" bordercolor="red">
                      <tr>
                        <td style="width:5px;"><img src="/App_Themes/<%=Session["MyTheme"]%>/images/spacer.gif" width="1" height="1"></td>
                        <td valign="top"><table width="100%"  border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                                <asp:ImageButton runat="server" ID="imgbtnAdd"  OnClick="imgbtnAdd_Click" width="32" AlternateText="Add" height="40" />
                            </td>
                          </tr>
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                               <asp:ImageButton runat="server" ID="imgbtnFind"  OnClick="imgbtnFind_Click" width="32" AlternateText="Find" height="40" />
                             </td>
                          </tr>
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                                <asp:ImageButton runat="server" ID="imgbtnDelete"  OnClick="imgbtnDelete_Click" width="32" AlternateText="Delete" height="40" />
                           </td>
                          </tr>
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                               <asp:ImageButton runat="server" ID="imgbtnUndo"  OnClick="imgbtnUndo_Click" width="32" AlternateText="Undo" height="40" />
                              </td>
                          </tr>
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                               <asp:ImageButton runat="server" ID="imgbtnSubmit"  OnClick="imgbtnSubmit_Click" width="32" AlternateText="Submit" height="40" />
                          </tr>
                          <tr>
                            <td align="center" valign="top" style="height:47px;">
                              <asp:ImageButton runat="server" ID="imgbtnUpdate"  OnClick="imgbtnUpdate_Click" width="32" AlternateText="Update" height="40" />
                            </td>
                          </tr>
                        </table></td>
                        <td align="right" valign="middle" style="width:9px; "><!--<img src="/App_Themes//images/close-arrow.gif" width="8" height="50">--></td>
                      </tr>
                    </table>
                      <!-- left icons end-->
                    </td>
                  </tr>
                  <tr><!-- style="height:143px; "-->
                    <td align="left" valign="top" style="height:143px;">&nbsp;</td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
            <td style="width:4px;"><img src="/App_Themes/<%=Session["MyTheme"]%>/images/spacer.gif" width="1" height="1"></td>
            <td>
             <!-- entry form start -->
            <table width="100%"  border="0" bordercolor="blue" cellspacing="0" cellpadding="0">
              <tr>
                <td align="left" valign="top">
                      <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                      <tr align="left" valign="bottom">
                        <td style="width:5px;"><img src="/App_Themes/<%=Session["MyTheme"]%>/images/title-left-curve.gif" width="5" height="21"></td>
                        <td class="bluebold" style="width:90px; background-color:#c5d5fc; ">SUBJECT </td>
                        <td style="width:5px;"><img src="/App_Themes/<%=Session["MyTheme"]%>/images/title-right-curve.gif" width="5" height="21"></td>
                        <td>&nbsp;</td>
                      </tr>
                    </table>
                </td>
              </tr>            
              <tr>
                <td align="left" valign="top">
                  <table width="100%"  border="0" cellpadding="0" cellspacing="0" class="formmiddle">
                  <tr>
                    <td><table width="100%"  border="0" cellspacing="0" cellpadding="0">
                      <tr align="left" valign="top">
                        <td style="width:21px;"><img src="/App_Themes/<%=Session["MyTheme"]%>/images/spacer.gif" width="1" height="1"></td>
                        <td><table width="100%"  border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td align="left" valign="middle" class="bigtitle" style="height:66px; ">Chart Of Account Entry Form</td>
                          </tr>
                          <tr>
                            <td align="left" valign="top">
                             <!-- form entry fields start -->
                             <table width="100%"  border="0" cellspacing="0" cellpadding="0">
                              <tr align="left" valign="top">
                                <td align="left" valign="middle" class="formtext" style="height:24px; width:161px;">Name</td>
                                <td valign="middle" style="width:196px; "><asp:TextBox ID="txtName" runat="server" CssClass="formfieldsyel" style="width:180px; height:18px;"></asp:TextBox></td>
                                <td valign="middle"><!--<input type="checkbox" name="checkbox" value="checkbox">Is Overridable--> </td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Display Sequence</td>
                                <td valign="middle"><asp:DropDownList ID=ddlDisplaySequence runat="server" SkinID="COADropDownList"></asp:DropDownList>
                                </td>
                                <td valign="middle"><!--<input type="checkbox" name="checkbox" value="checkbox">Is Overridable --></td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Type of charted Account No</td>
                                <td valign="middle"><asp:DropDownList ID="ddlAccountNo" runat="server" SkinID="COADropDownList"></asp:DropDownList>
                                </td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Format Mask</td>
                                <td valign="middle"><asp:DropDownList ID="ddlFormatMask" runat="server" SkinID="COADropDownList"></asp:DropDownList>
                                </td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Type of Heading</td>
                                <td valign="middle"><asp:DropDownList ID="ddlTypeofHeading" runat="server" SkinID="COADropDownList"></asp:DropDownList>
                                </td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Standard Total Group</td>
                                <td valign="middle"><asp:DropDownList ID="ddlStandarTotalGroup" runat="server" SkinID="COADropDownList"></asp:DropDownList></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Cash Flow Group</td>
                                <td valign="middle"><asp:DropDownList ID="ddlCashFlowGroup" runat="server" SkinID="COADropDownList"></asp:DropDownList></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Is AccountNumber Required</td>
                                <td valign="middle"><asp:CheckBox ID="chkAccountNumberRequired" runat="server" /></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Is Numeric</td>
                                <td valign="middle"><asp:CheckBox ID="chkIsNumeric" runat="server" /></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Is Approved</td>
                                <td valign="middle"><asp:CheckBox ID="chkIsApproved" runat="server" /></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Is Active</td>
                                <td valign="middle"><asp:CheckBox ID="chkIsActive" runat="server" /></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                             <tr align="left" valign="top">
                                <td align="right" valign="middle" class="formtext" style="height:24px;">Type of InActive Status</td>
                                <td valign="middle"><asp:DropDownList ID="ddlTypeOfInActiveStatus" runat="server" SkinID="COADropDownList"></asp:DropDownList></td>
                                <td valign="middle">&nbsp;</td>
                                <td>&nbsp;</td>
                              </tr>
                              
                            </table>
                            <!-- form entry fields end -->
                            </td>
                          </tr>
                          <tr>
                            <td align="left" valign="top" style="height:167px; ">&nbsp;</td>
                          </tr>
                        </table></td>
                      </tr>
                    </table></td>
                  </tr>
                </table>
                <!-- entry form end -->
                </td>
              </tr>
            </table></td>
          </tr>
        </table>

<!-- inner table end-->
<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" style="display:none"/>
<asp:Panel ID="pnlDelete" runat="server" Style="display: none" CssClass="modalPopup">
    <table cellpadding="0" cellspacing="0">
      <tr>
        <td class="formtext" style="height:24px;">Do you want to delete?</td>
      </tr>
      <tr>
        <td>
          <asp:Button ID="btnOk" Text="Ok" runat="server" />&nbsp;
          <asp:Button ID="btnCancel" Text="Cancel" runat="server" />
        </td>
      </tr>
    </table>
</asp:Panel>
   <%-- <Ajax:ModalPopupExtender ID="ModalPopupExtender"  runat="server"
            TargetControlID="imgbtnDelete"
            PopupControlID="pnlDelete" 
            BackgroundCssClass="modalBackground" 
            OkControlID="btnOk"
            OnOkScript="onOk()" 
            CancelControlID="btnCancel" 
            DropShadow="true"       
            PopupDragHandleControlID="pnlDeletebtn">  
    </Ajax:ModalPopupExtender>
    <Ajax:ModalPopupExtender runat="server" ID="programmaticModalPopup"
            BehaviorID="programmaticModalPopupBehavior"
            TargetControlID="hiddenTargetControlForModalPopup"
            PopupControlID="programmaticPopup" 
            BackgroundCssClass="modalBackground"
            DropShadow="True"
            PopupDragHandleControlID="programmaticPopupDragHandle"
             >
    </Ajax:ModalPopupExtender>--%>
<!-- COA Entry End -->
</asp:Content>
