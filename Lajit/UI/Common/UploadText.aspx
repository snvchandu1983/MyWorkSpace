<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UploadText.aspx.cs" Inherits="LAjitDev.Common.UploadText" %>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent">
            <Triggers>
                <asp:postBackTrigger ControlID="imgbtnSubmit" />
             </Triggers>
            <contenttemplate>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
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
                                                 <td id="htcCPGV1" runat="server" class="grdVwtitle" width="105px">
                                                    File Upload
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Visible="true" Height="511px">
                                            <!-- entry form start -->
                                            <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
                                                <tr>
                                                    <td align="left" valign="top" style="height:460px;width:925px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr style="height:1px;" align="right" valign="top"width="100%" >                                                                                                                             
                                                               <td align="right" valign="middle" colspan="2" width="100%">
                                                                   &nbsp;
                                                               </td>
                                                            </tr>
                                                            <!--ProcessLinks tr-->
                                                             <tr style="height: 24px;" id="trProcessLinks" align="right" valign="top" runat="server">                                                                                                                             
                                                                <td colspan="2" align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                                    <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">                                                                    
                                                                    </asp:Panel>
                                                                </td>
                                                             </tr>
                                                            <tr id="trDescription" runat="server">
                                                                <td style="height: 24px;" colspan="2"  align="left" valign="middle">
                                                                    <asp:Label  runat="server" ID="lblDescriptionText" style="font-weight: bold;color: #003366;font-size: 13px;font-family:Verdana, Arial, Tahoma, verdana, arial, helvetica, univers;"></asp:Label>
                                                                </td> 
                                                            </tr>
                                                            <tr>
                                                               <td colspan="2" height="10px">&nbsp;</td>
                                                            </tr> 
                                                             <tr id="trFileName" align="left" valign="top" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 110px" align="right" valign="middle">
                                                                   <asp:Label ID="lblFileName" runat="server" CssClass="mbodyb">File Name</asp:Label>
                                                                 </td>
                                                                <td valign="middle" align="left">
                                                                  <asp:FileUpload ID="FileAttachment" runat="server" Enabled="true"  BackColor="#CCCCCC" style="height: 24px;width:300px" size="50"></asp:FileUpload>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqFileName" runat="server" ControlToValidate="FileAttachment"
                                                                        ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required." ToolTip="Value is required."
                                                                        SetFocusOnError="True" Enabled="true" MapXML="FileName" >*</LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>  
                                                           <tr>
                                                             <td colspan="2">&nbsp;</td>
                                                           </tr> 
                                                           <tr>
                                                              <td>&nbsp;</td> 
                                                              <td align="left">
                                                                   <!-- Panel Controls Start -->
                                                                   <asp:Panel ID="pnlUpload" runat="server" Visible="false" >
                                                                   <table cellpadding="0" cellspacing="0" width="100%" align="left">
                                                                    <tr id="trUploadOptionA" align="left" valign="top" runat="server">
                                                                       <td valign="middle" align="left"> 
                                                                          <LCtrl:LAjitCheckBox runat="server" ID="chkUploadOptionA" MapXML="UploadOptionA" Visible="false"/>
                                                                           <asp:Label   runat="server" ID="lblUploadOptionA"  SkinID="Label" text="UploadOptionA" Visible="false"></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                   <tr id="trUploadOptionB" align="left" valign="top" runat="server">
                                                                       <td valign="middle" align="left"> 
                                                                         <LCtrl:LAjitCheckBox runat="server" ID="chkUploadOptionB" MapXML="UploadOptionB" Visible="false" />
                                                                          <asp:Label   runat="server" ID="lblUploadOptionB"  SkinID="Label" text="UploadOptionB" Visible="false"></asp:Label> 
                                                                       </td>
                                                                   </tr>
                                                                   <tr id="trUploadOptionC" align="left" valign="top" runat="server">
                                                                        <td valign="middle" align="left" style="width:350px"> 
                                                                         <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                                                          <tr>
                                                                            <td style="width:145px">
                                                                              <LCtrl:LAjitCheckBox runat="server" ID="chkUploadOptionC" MapXML="UploadOptionC" Visible="false" />
                                                                               <asp:Label   runat="server" ID="lblUploadOptionC"  SkinID="Label" text="UploadOptionC" Visible="false"></asp:Label> 
                                                                            </td>
                                                                            <td style="width:195px" align="left" valign="middle" colspan="2">
                                                                              <div id="divddl" style="display:none"> 
                                                                              <table style="width:100%" border="0">
                                                                              <tr>
                                                                                 <td style="width:90px" align="right" valign="middle"> 
                                                                                  <asp:Label   runat="server" ID="lblSecureCateogry"  SkinID="Label" text="Secure Category" Visible="true"></asp:Label> 
                                                                                 </td>
                                                                                 <td valign="middle" align="left" style="width:100px">
                                                                                  <LCtrl:LAjitDropDownList runat="server" ID="ddlSecureCategory" MapXML="SecureCategory" Visible="true"/>
                                                                                   <LCtrl:LAjitRequiredFieldValidator ID="reqSecureCategory" MapXML="SecureCategory" runat="server" ControlToValidate="ddlSecureCategory" ValidationGroup="LAJITEntryForm"                                             ErrorMessage="Value is required." ToolTip="Value is required." SetFocusOnError="True" Enabled="false" Visible="true"></LCtrl:LAjitRequiredFieldValidator> 
                                                                                  </td>
                                                                               </tr>
                                                                               </table> 
                                                                              </div> 
                                                                           </td>                                                            
                                                                         </tr>
                                                                         </table>
                                                                        </td>
                                                                    </tr>
                                                                   <tr id="trUploadOptionD" align="left" valign="top" runat="server">
                                                                       <td valign="middle" align="left"> 
                                                                          <LCtrl:LAjitCheckBox runat="server" ID="chkUploadOptionD" MapXML="UploadOptionD" Visible="false" />
                                                                           <asp:Label   runat="server" ID="lblUploadOptionD"  SkinID="Label" text="UploadOptionD" Visible="false"></asp:Label> 
                                                                        </td>
                                                                    </tr> 
                                                                 </table>     
                                                                 </asp:Panel>
                                                                 <!-- Panel Controls end -->    
                                                        </td>
                                                       </tr>
                                                            
                                                        <tr>
                                                          <td colspan="2">  
                                                              <div id="serverStatus" style="width: auto; height: auto; visibility: hidden; position: absolute;
                                                                top: 20px; right: 20px;">
                                                                <br />
                                                               <img src="<%=Application["ImagesCDNPath"]%>Images/ajax-loader.gif" />
                                                               </div>
                                                            </td>
                                                          </tr> 
                                                            <!--Submit and Cancel buttons-->
                                                           <tr style="height:24px">
                                                                <td style="height: 24px; width: 160px">&nbsp;</td>
                                                                <td align="left" colspan="3">
                                                                <table border="0">
                                                                   <tr>
                                                                        <td>
                                                                            <LCtrl:LAjitImageButton ID="imgbtnSubmit" runat="server" OnClick="imgbtnSubmit_Click" OnClientClick="javascript:return ValidateControls()" 
                                                                              Height="18" AlternateText="Submit"></LCtrl:LAjitImageButton>
                                                                        </td>
                                                                       </tr>
                                                                </table>
                                                               </td>                                                                                                                                   
                                                             </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblmsg" runat="server" Visible="false" SkinID="LabelMsg"></asp:Label>
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
                                            <table cellpadding="4" cellspacing="4" border="0"  width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td><asp:Label ID="lblError" runat="server"  SkinID="LabelMsg"></asp:Label></td>
                                                    
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
       <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" /> 
       <input type="hidden" id="hidChecked" name="hidChecked"  runat="server"/> 
        <!--parentBPInfo and parentGVDataXML stores the parent page info.They are reset to session when child popup is closed-->       
        <asp:HiddenField ID="parentBPInfo" runat="server" />
        <asp:HiddenField ID="ParentGVDataXML" runat="server" />        
        <asp:HiddenField ID="hdnFldBPInfo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFldBPOut" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnFldReportName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnNeedToConfirmExit" runat="server" />
        <!-- Stores the BPEInfo required while posting to another page upon BPC Icon Click-->
        <asp:HiddenField ID="hdnBPIconBPEINFO" runat="server" /> 
        <script type="text/javascript" >


    window.onload=function()
    {
        show();
        //hide loading image  
        HideParentProgress();
    }
     
    function checked()
    {
        var chkID=document.getElementById('ctl00_cphPageContents_chkUploadOptionC');
        var hides=document.getElementById('ctl00_cphPageContents_hidChecked');
        if(chkID.checked)
        {
            hides.value='Checked'; 
            document.getElementById('divddl').style.display='block';
        }    
        else
        {
            document.getElementById('divddl').style.display='none';
        }
    } 

    function show()
    {
        if(document.getElementById('ctl00_cphPageContents_hidChecked').value=='Checked')
        { 
            document.getElementById('divddl').style.display='block';
        }
        else
        {
            document.getElementById('divddl').style.display='none';
        }
    }  

    function ShowPageProgress()
    {
        var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
        //Add a div in the middle of the page on top of the semi-transparent layer.
        if(document.getElementById('box') == null && !prm.get_isInAsyncPostBack())//Check whether postback has completed within the ShowProgress Delay.
        {
            //var currentTheme = document.getElementById('ctl00_hdnThemeName').value;
            var div = document.createElement('div');
            div.style.zIndex = 100002009;
            div.id = 'box';
            div.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
            div.style.top = '200px';
            div.style.left = (width / 2) - 50 + 'px'; 
            div.innerHTML = '<img alt="Please wait.." src="'+g_cdnImagesPath+'Images/ajax-loader.gif" />';
            document.body.appendChild(div);

            if(Sys.Browser.name == 'WebKit')
            {
                setTimeout('HideProgress()', 700);
            }
        }
    }
       
        
</script>
</asp:content>
