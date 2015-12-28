<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SelectInvoice.aspx.cs"
    Inherits="LAjitDev.Payables.SelectInvoice" %>
<%@ Register TagPrefix="LCtrl" Src="~/UserControls/ChildGridView.ascx" TagName="ChildGridView" %>
<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">
<asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel runat="server" ID="updtPnlContent" UpdateMode="Conditional">
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
                                        <asp:Panel ID="pnlEntryForm" runat="server" Height="511px">
                                            <!-- entry form start -->
                                            <table style="height: 511px" width="100%" border="0" cellspacing="0" cellpadding="0" class="formmiddle">
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
                                                <!--Page Controls tr-->
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">                                                           
                                                            <tr id="trNeedLabelName" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label runat="server" ID="lblNeedLabelName" SkinID="Label"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtNeedLabelName" runat="server" MapXML="NeedLabelName" Width="146px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqNeedLabelName" runat="server" ControlToValidate="txtNeedLabelName"
                                                                        ValidationGroup="LAJITEntryForm" Enabled="false" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" MapXML="Description"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDescription" align="left" valign="middle" runat="server">
                                                                <td class="formtext" style="height: 24px; width: 160px">
                                                                    <asp:Label SkinID="Label" runat="server" ID="lblDescription"></asp:Label>
                                                                </td>
                                                                <td valign="middle">
                                                                    <LCtrl:LAjitTextBox ID="txtDescription" runat="server" MapXML="Description" Width="200px"></LCtrl:LAjitTextBox>
                                                                    <LCtrl:LAjitRequiredFieldValidator ID="reqDescription" MapXML="Description" runat="server"
                                                                        ControlToValidate="txtDescription" ValidationGroup="LAJITEntryForm" ErrorMessage="Value is required."
                                                                        ToolTip="Value is required." SetFocusOnError="True" Enabled="false"></LCtrl:LAjitRequiredFieldValidator>
                                                                </td>
                                                            </tr>                                                            
                                                            <!-- Branch AccountingItem Start -->
                                                            <tr align="center">
                                                                <td colspan="2" valign="top">
                                                                    <LCtrl:ChildGridView ID="CGVUC" runat="server" BranchNodeName="AccountingItem" />
                                                                </td>
                                                            </tr>
                                                           
                                                            <!--Submit and Cancel buttons-->                                                            
                                                            <tr style="height:24px">
                                                                <td align="center" colspan="2">
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
                                                                <td colspan="2" align="center">
                                                                   <table border="0">
                                                                     <tr>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnPrevious" Tooltip="Previous"/>
                                                                       </td>
                                                                       <td> 
                                                                        <asp:ImageButton runat="server" ID="imgbtnNext" Tooltip="Next"/>
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
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Timer ID="timerEntryForm" runat="server" OnTick="timerEntryForm_Tick" Enabled="False">
                                            </asp:Timer>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlContentError" runat="server" Visible="False">
                                            <table cellpadding="4" cellspacing="4" border="1" width="800px" align="center">
                                                <tr align="center" class="formmiddle">
                                                    <td>
                                                        <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label>
                                                    </td>
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
      <script type="text/javascript" language="javascript">

    //Called when the form-level process links are clicked upon in the page.
    function OnSelectFormBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp, confirmText)
    {
        if(document.getElementById('ctl00_cphPageContents_BtnsUC_hdnSelected').value=="TRUE")
        {  
            var COXML ="" ;
            var hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnGVBPEINFO');
            if(hdnGVBPEINFO)
            {
                COXML=hdnGVBPEINFO.value;                   
            }
            else
            {
                if(g_IsDebugging)
                {
                    alert("No RowXML was found in Hidden variables either in GVUC or BtnsUC!!This alert was raised by Common.js>OnFormBPCLinkClick()");
                    return;
                }
            }
            CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML, confirmText);
        }   
        else
        {
            if(document.getElementById('ctl00_cphPageContents_BtnsUC_hdnSelected').value=="FALSE")
            {
                alert("No Payments Selected"); 
                return false; 
            } 
        } 
    }

    var m_CurrentPageName=null;
       
    //Updates the Navigation History XML when a non-PopUp link is clicked.
    //Parameter bpeInfoXML:XML being posted to the next page without the Root Tags.
    function UpdateNavHistoryXML(bpeInfoXML)
    {
        var previousPage;
        var previousPageBPInfo;
        var nodeSelPageRoot;
        if(bpeInfoXML!="")
        {
            var co= "<Root>" + bpeInfoXML + "</Root>";
            var xCOXMLDoc=loadXMLString(co); 
            var nodeCORoot=xCOXMLDoc.getElementsByTagName("Root");//Root Node            
            nodeSelPageRoot=nodeCORoot[0].firstChild;
            if(nodeSelPageRoot)
            {
                var pageRowlist = nodeSelPageRoot.getElementsByTagName("RowList");
                var pageRows = pageRowlist[0].getElementsByTagName("Rows");
                pageRows[0].setAttribute("BPAction", "Find");
            }
            var nodeCallingObj=nodeCORoot[0].getElementsByTagName("CallingObject");
            var nodeBPGID=nodeCallingObj[0].firstChild;
            previousPageBPInfo= ConvertToString(nodeBPGID) + ConvertToString(nodeSelPageRoot);
            var nodePageInfo=nodeCallingObj[0].getElementsByTagName("PageInfo");
            previousPage=nodePageInfo[0].firstChild;                                
        }
        //Setting the hdn Navigation BPInfo
        var navBPInfo= document.getElementById('ctl00_cphPageContents_BtnsUC_hdnNavBPInfo');
        if (navBPInfo != null)
        {
            if(trim(navBPInfo.value).length == 0)
            {
                navBPInfo.value = "<Root></Root>";
            }
            var NavBPInfoXML =navBPInfo.value;
            var xNavDoc = loadXMLString(NavBPInfoXML);
            var nodeRoot= xNavDoc.getElementsByTagName("Root");
            var linkText="Link1";
            if(nodeRoot!=null)
            {               
                nodeRoot=nodeRoot[0];
                var navRootChildNodes=nodeRoot.childNodes;
                if(navRootChildNodes)
                {
                    var navBPInfoCount = navRootChildNodes.length;
                    if (navBPInfoCount > 0)//From 2nd link onwards
                    {     
                        linkText="Link"+(navBPInfoCount+1);
                    }
                }
                var nodeLink = xNavDoc.createElement(linkText);//Create Link1 Node
                nodeRoot.appendChild(nodeLink);
                var nodeRedirectPage = xNavDoc.createElement("RedirectPage");//Create RedirectPage Node
                nodeLink.appendChild(nodeRedirectPage);
                //var newRedirectPagetext=xNavDoc.createTextNode(previousPage);
                nodeRedirectPage.appendChild(previousPage.cloneNode(true));
                var nodeBpInfo = xNavDoc.createElement("bpinfo");//Create bpinfo Node
                nodeLink.appendChild(nodeBpInfo);
                //var newBpInfotext=xNavDoc.createTextNode(previousPageBPInfo);
                previousPageBPInfo="<Root>"+previousPageBPInfo+"</Root>";
                var newBpInfotext=loadXMLString(previousPageBPInfo).getElementsByTagName("Root")[0];
                nodeBpInfo.appendChild(newBpInfotext.firstChild.cloneNode(true));
                nodeBpInfo.appendChild(newBpInfotext.firstChild.nextSibling.cloneNode(true));
                //Convert the Document back to a string and update
                var updatedNavBPInfo=ConvertToString(nodeRoot);
                document.getElementById("ctl00_cphPageContents_BtnsUC_hdnNavBPInfo").setAttribute("value",updatedNavBPInfo);                  
            }                 
        } 
    }  

    </script>
</asp:content>
