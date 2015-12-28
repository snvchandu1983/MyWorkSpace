<%@ Page Language="C#" AutoEventWireup="true" Codebehind="JobType.aspx.cs" Inherits="LAjitDev.Financials.JobType" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">

<script language="javascript">
    function DisplayImage()
    { 
        debugger;
        var hdnFld=document.getElementById("ctl00_cphPageContents_hdnFldTypeOfJob");
        var ddlTypeOfJob=document.getElementById("ctl00_cphPageContents_ddlTypeOfJob");
        var arr = (hdnFld.value).split(",");
        for(i = 0; i < arr.length; i++ )
        {
            var selval=arr[i].split("-");
            var image = document.getElementById("ctl00_cphPageContents_imgTypeOfJob");
            if(ddlTypeOfJob.value==selval[0])
            {
                var imgSrc=selval[1];
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
</script>

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
                                             <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
                                                <!--Pop up header tr-->
                                                <tr style="height:24px;" runat="server" id="trPopupHeader" visible="false" valign="top">
                                                    <td> 
                                                       <table border="0" width="100%" cellpadding="0" cellspacing="0" style="height: 24px;
                                                        cursor: hand; border-right-width: 1px; border-right-style: double;  border-right-color: #d7e0f1;">
                                                            <tr style="height: 24px;">
                                                                <td id="htcPopEntryForm" runat="server" align="center" class="grdVwRPTitle" style="height: 24px; border-width: 0px">
                                                                    <asp:Label id="lblPopupEntry" runat="server" height="24px"></asp:Label>
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
                                                <tr id="trSubject" runat="server" visible="false" style="height:24px;background-color:Green">
                                                    <td class="bigtitle" valign="middle">
                                                        <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                            height="6px" />
                                                        <asp:Label  SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="height:439px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
                                                            <tr style="height:1px;" align="right" valign="middle" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <tr id="trSoxApprovedStatus" valign="middle" align="left" runat="server">
                                                                <td valign="middle" colspan="2" align="center">
                                                                   <LCtrl:LAjitImageButton ID="imgbtnIsApproved" runat="server"  MapXML="SoxApprovedStatus" OnClick="imgbtnIsApproved_Click" />
                                                                </td>
                                                             </tr>
                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblDescription"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LajitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="200px"></LCtrl:LajitTextBox>                                                                                     
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" runat="server" MapXML="Description" ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                   
                                                                </td>
                                                                 <td rowspan="3" align="left" valign="middle" style="width:360px">
                                                                    <LCtrl:LAjitImage ID="imgTypeOfJob" runat="server"/>
                                                                 </td>
                                                            </tr>   
                                                            <tr id="trTypeOfJob" align="left" valign="middle" runat="server">  
                                                                 <td class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblTypeOfJob" ></asp:Label>
                                                                 </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlTypeOfJob" runat="server" MapXML="TypeOfJob" Width="206px"></LCtrl:LAjitDropDownList><!--OnSelectedIndexChanged="ddlTypeOfJob_SelectedIndexChanged" AutoPostBack="True" -->
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqTypeOfJob" runat="server" MapXML="TypeOfJob" ControlToValidate="ddlTypeOfJob" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                </td>
                                                            </tr>  
                                                            <tr  id="trJobAccountHeading" align="left" valign="middle" runat="server">  
                                                                 <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblJobAccountHeading" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                     <LCtrl:LAjitDropDownList ID="ddlJobAccountHeading" runat="server" MapXML="JobAccountHeading" Width="206px"></LCtrl:LAjitDropDownList>
                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqJobAccountHeading" runat="server" MapXML="JobAccountHeading" ControlToValidate="ddlJobAccountHeading" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                     
                                                                </td>
                                                            </tr>                                                                                                                                                                                                                                                                     
                                                            <tr id="trJobBudgetLayout" align="left" valign="middle" runat="server">   
                                                                <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblJobBudgetLayout" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitDropDownList ID="ddlJobBudgetLayout" runat="server" MapXML="JobBudgetLayout" Width="206px"></LCtrl:LAjitDropDownList>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqJobBudgetLayout" runat="server" MapXML="JobBudgetLayout" ControlToValidate="ddlJobBudgetLayout" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                                                                                           
                                                                </td>
                                                            </tr>
                                                            <tr id="trJobCOA" align="left" valign="middle" runat="server">  
                                                                 <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblJobCOA" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                     <LCtrl:LAjitDropDownList ID="ddlJobCOA" runat="server" MapXML="JobCOA" Width="206px"></LCtrl:LAjitDropDownList>
                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqJobCOA" runat="server" MapXML="JobCOA" ControlToValidate="ddlJobCOA" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                </td>
                                                            </tr>                                                                                                                                        
                                                            <tr  id="trJobFormatMask" align="left" valign="middle" runat="server">  
                                                                 <td  class="formtext" style="height: 24px; width:160px">
                                                                    <asp:Label  SkinID="Label" runat="server" ID="lblJobFormatMask" ></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                     <LCtrl:LAjitDropDownList ID="ddlJobFormatMask" runat="server" MapXML="JobFormatMask" Width="206px"></LCtrl:LAjitDropDownList>
                                                                     <LCtrl:LAjitRequiredFieldValidator ID="reqJobFormatMask" runat="server" MapXML="JobFormatMask" ControlToValidate="ddlJobFormatMask" ValidationGroup="LAJITEntryForm"
                                                                        ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>                                                                    
                                                                </td>
                                                            </tr> 
                                                            <!--space between page controls and submit button--> 
                                                            <tr>
                                                                <td style="height:15px"></td>
                                                            </tr>                                                                                                                                                                                                                             
                                                            <!--Submit and Cancel buttons-->                                                            
                                                             <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
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
                                                             <tr style="height:24px">
                                                                <td style="height: 24px;" colspan="4" align="center">
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
                                                             <tr style="height:10px">
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
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="1"  width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td><asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            <asp:HiddenField id="hdnFldTypeOfJob" runat="server"></asp:HiddenField>
            </contenttemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:content>