<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ProcessEngine.aspx.cs"
    Inherits="LAjitDev.Common.ProcessEngine" Title="LAjit :: ProcessEngine" Theme="LAjit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContents" runat="server">

    <script type="text/javascript">

    function CloseWindow()
    {
        DisplayPopUp(false);//Function in Common.js
    }
    
    function LabelClose()
    {
        var lblName=document.getElementById("ctl00_cphPageContents_lblmsg");
        if(lblName!="")
        {
            if(lblName.innerText!="")
            {
                lblName.innerText="";
            } 
        } 
    }

    function HideNotesModel()
    {
        AjaxMethods.KillSession('<%=Session["strOUTXML"]%>');  
        //$find('mpePrintBehaviourID').hide();
        DisplayPopUp(false,'ctl00_cphPageContents_pnlPrint');
    }
         
    function Openframe(framename,pagename)
    {
        //Setting Display
        switch (framename)
        {
            case "iframePrint":                                               
                document.getElementById("iframePrint").style.display='block';
                break;  
        }      
        //Setting Pagename
        document.getElementById(framename).src = "../PopUps/"+pagename;                               
        if(framename!=="iframePrint")
        {
            //$find('mpeNotesBehaviourID').show();
            DisplayPopUp(true,'ctl00_cphPageContents_BtnsUC_pnlNote');
        }
        return false;
    } 

    function GetPageControls(branchName)
    {
        var hdnPageControls=document.getElementById('ctl00_cphPageContents_hdnControls').value;   
        if(hdnPageControls)
        {
            hdnPageControls=hdnPageControls.split(",");
            for(i = 0; i < hdnPageControls.length; i++)
            {
                var control=document.getElementById(hdnPageControls[i]);
                if(control)
                {
                    var mapxml=hdnPageControls[i].split("_");
                    var mapxmlstr=mapxml[2].substr(3,eval(mapxml[2].length-3));
                    UpdateBPCLinkXML(control,branchName,mapxmlstr);
                }
            }
        }      
    }
  
    //Updates the hidden variable hdnGVBPEINFO in BtnsUC with changed one.
    function UpdateBPCLinkXML(obj,branchName,attKey)
    {
        var COXML ="" ;
        var hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_hdnGVBPEINFO');
        if(hdnGVBPEINFO)
        {
            COXML=hdnGVBPEINFO.value;                   
        }
        else
        {
            if(g_IsDebugging)
            {
                alert('No BPC Link XML containers found.\nRaised by UpdateBPCLinkXML() in VoidCheckHistory.aspx');
                return;
            }
        }
        var xDocBPC=loadXMLString("<Root>" + COXML + "</Root>");
        var nodeBranch=xDocBPC.getElementsByTagName(branchName);
        if(nodeBranch&&nodeBranch.length>0)
        {
            nodeBranch=nodeBranch[0];
        }
        else
        {
        //No Branch node present so create it..
        }
        var nodeRow=nodeBranch.getElementsByTagName("Rows")[0];
        if(obj.nodeName=="SELECT")//DropDownList
        {
            var ddlSelectedValue = obj.value;
            var trxID = ddlSelectedValue.split('~')[0];
            var trxType = ddlSelectedValue.split('~')[1];
            if(trxID ==-1||trxID =="")
            {
                nodeRow.removeAttribute(attKey+'_TrxID');
                nodeRow.removeAttribute(attKey+'_TrxType');
            }
            else
            {    
                nodeRow.setAttribute(attKey+'_TrxID',trxID);
                nodeRow.setAttribute(attKey+'_TrxType',trxType);
            }
        }
        else if(obj.nodeName == "SPAN")//Checkbox with an attibute
        {
            var childObj = GetSpanChild(obj);
            if(childObj.type == "checkbox")
            {
                if(childObj.checked==true)
                {
                    nodeRow.setAttribute(attKey,"1");
                }
                else
                {
                    nodeRow.setAttribute(attKey,"0");
                }
            }
        }
        else if(obj.nodeName == "INPUT" && obj.type=="checkbox")//Checkbox with an attibute
        {
            if(obj.checked==true)
            {
                nodeRow.setAttribute(attKey,"1");
            }
            else
            {
                nodeRow.setAttribute(attKey,"0");
            }
        }
        else
        {
            var attValue = obj.value;
            nodeRow.setAttribute(attKey,attValue);
        }
        //Get rid of the Root tag.
        hdnGVBPEINFO.value=ConvertToString(xDocBPC).replace("<Root>","").replace("</Root>","");
    }
  
    //Called when the form-level process links are clicked upon in the page.
    function OnProcessBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp, treeNodeName,confirmMessage)
    {
        var previousPage;
        var previousPageBPInfo;
        var COXML ="" ;
        var hdnGVBPEINFO=document.getElementById(bpeInfoClientID);
        if(hdnGVBPEINFO)
        {
            COXML=hdnGVBPEINFO.value;        
        }
        else//Taking BtnsUC hdnBPInfo which is set in Modify Load event
        {                               
            hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_hdnGVBPEINFO');
            if(hdnGVBPEINFO)
            {
                //Createing new row xml based on page controls.
                GetPageControls(treeNodeName);
                //Updated BPEINFO
                hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_hdnGVBPEINFO');
                COXML=hdnGVBPEINFO.value;      
            }
            else
            {
                if(g_IsDebugging)
                {
                    alert("No RowXML was found in Hidden variables either in GVUC or BtnsUC!!This alert was raised by Common.js>OnFormBPCLinkClick()");
                }
            }
        }
        CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML,confirmMessage);
    }
	
	 jQuery(function() {
           //GET all Calculator Fields
           var calFields=jQuery("input[JCal]");
           if(calFields.length > 0)
           {
              for(i=0;i<calFields.length;i++)
              {
               //jQuery("#"+calFields[i].id).datepicker({dateFormat:'"+ jQuery("#"+calFields[i].id).attr("JCal") +"',changeMonth: true,changeYear: true});
               jQuery("#"+calFields[i].id).datepicker({dateFormat:'mm/dd/y',changeMonth: true,changeYear: true});
              }
          }  
          //GET all MaskEditor Fields
          var maskFields=jQuery("input[JMask]");
           if(maskFields.length > 0)
           {
              for(j=0;j<maskFields.length;j++)
              {
                jQuery("#"+ maskFields[j].id).mask('99/99/99');
                //console.log(maskFields[j].id +' '+ jQuery("#"+maskFields[j].id).attr("JMask"));
              }
          }  
      });   
    </script>

    <asp:Panel ID="pnlContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <!--GV-->
                <td class="tdMainCont">
                    <table class="tblMainCont" cellpadding="0" cellspacing="0" border="0" >
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
                                             <% if (Page.Request.Browser.Browser == "AppleMAC-Safari")
                                                   { %>
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" height="1" />
                                            <%} %>
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
                                <asp:Panel ID="pnlEntryForm" runat="server" Visible="True">
                                    <!-- entry form start  -->
                                    <table id="tblEntryForm" runat="server" style="height: 511px" width="100%" border="0"
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
                                        <tr style="height: 10px;" id="trProcessLinks" align="right" valign="top" runat="server">
                                            <td align="right" valign="middle" id="tdProcessLinks" runat="server">
                                                <asp:Panel ID="pnlBPCContainer" runat="server" Visible="true">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <!--Report Links tr-->
                                        <tr style="height: 10px;" id="trReportLinks" align="right" valign="middle" runat="server">
                                            <td align="right" valign="middle" id="tdReportLinks" runat="server">
                                                <asp:Panel ID="pnlReport" runat="server" Visible="true">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <!--Page subject tr style="height:439px"-->
                                        <tr style="height: 10px;" id="trSubject" runat="server" visible="false">
                                            <td class="bigtitle" valign="middle">
                                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" alt="spacer" width="5"
                                                    height="6px" />
                                                <asp:Label SkinID="Label" ID="lblPageSubject" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="height: 457px">
                                                <table width="500px" border="0" cellspacing="0" cellpadding="0" align="left">
                                                    <tr id="trDescription" align="left" valign="middle" runat="server">
                                                        <td class="bigtitle" align="left" colspan="2">
                                                            &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblDescriptionValue" SkinID="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1" align="left" valign="top" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            &nbsp;
                                                        </td>
                                                        <td valign="middle" style="height: 24px; width: 340px">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenericSelect1" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenericSelect1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitDropDownList ID="ddlGenericSelect1" runat="server" MapXML="GenericSelect1"
                                                                Width="320px">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenericSelect2" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenericSelect2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitDropDownList ID="ddlGenericSelect2" runat="server" MapXML="GenericSelect2"
                                                                Width="320px">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenericSelect3" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenericSelect3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitDropDownList ID="ddlGenericSelect3" runat="server" MapXML="GenericSelect3"
                                                                Width="320px" SkinID="ddlDisabled">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenericSelect4" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenericSelect4" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitDropDownList ID="ddlGenericSelect4" runat="server" MapXML="GenericSelect4"
                                                                Width="320px">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenericSelect5" runat="server">
                                                        <td class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenericSelect5" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitDropDownList ID="ddlGenericSelect5" runat="server" MapXML="GenericSelect5"
                                                                Width="320px">
                                                            </LCtrl:LAjitDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenSDate1" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenSDate1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSDate1" runat="server" MapXML="GenSDate1" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenEDate1" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenEDate1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenEDate1" runat="server" MapXML="GenEDate1" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenSDate2" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenSDate2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSDate2" runat="server" MapXML="GenSDate2" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenEDate2" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenEDate2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenEDate2" runat="server" MapXML="GenEDate2" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenSDate3" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenSDate3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSDate3" runat="server" MapXML="GenSDate3" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left" valign="top" id="trGenEDate3" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px;">
                                                            <asp:Label runat="server" ID="lblGenEDate3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenEDate3" runat="server" MapXML="GenEDate3" Width="68px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSRange1" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSRange1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSRange1" runat="server" MapXML="GenSRange1" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenERange1" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenERange1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenERange1" runat="server" MapXML="GenERange1" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSRange2" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSRange2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSRange2" runat="server" MapXML="GenSRange2" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenERange2" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenERange2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenERange2" runat="server" MapXML="GenERange2" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSRange3" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSRange3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenSRange3" runat="server" MapXML="GenSRange3" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenERange3" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenERange3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitTextBox ID="txtGenERange3" runat="server" MapXML="GenERange3" Width="200px"></LCtrl:LAjitTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSelect1" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSelect1" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitCheckBox ID="chkGenSelect1" runat="server" MapXML="GenSelect1" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSelect2" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSelect2" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitCheckBox ID="chkGenSelect2" runat="server" MapXML="GenSelect2" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSelect3" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSelect3" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitCheckBox ID="chkGenSelect3" runat="server" MapXML="GenSelect3" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSelect4" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSelect4" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitCheckBox ID="chkGenSelect4" runat="server" MapXML="GenSelect4" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trGenSelect5" align="left" valign="top" runat="server">
                                                        <td align="right" valign="top" class="formtext" style="height: 24px; width: 160px">
                                                            <asp:Label runat="server" ID="lblGenSelect5" SkinID="Label"></asp:Label>
                                                        </td>
                                                        <td valign="middle">
                                                            <LCtrl:LAjitCheckBox ID="chkGenSelect5" runat="server" MapXML="GenSelect5" />
                                                        </td>
                                                    </tr>
                                                    <!--Label Msg-->
                                                    <tr>
                                                        <td colspan="2" align="left" style="height: 24px;">
                                                            <asp:Label ID="lblmsg" runat="server" SkinID="LabelMsg"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- form entry fields end -->
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlContentError" runat="server" Visible="False" Height="511px">
                                    <table cellpadding="4" cellspacing="4" border="0" width="100%" align="center">
                                        <tr align="center" class="formmiddle">
                                            <td>
                                                <asp:Label ID="lblError" runat="server" SkinID="LabelMsg"></asp:Label></td>
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
    <div id="divPrint">
        <asp:UpdatePanel runat="server" ID="UpdPnlPrint">
            <ContentTemplate>
                <asp:Panel ID="pnlPrint" runat="server" Style="display: none" BorderWidth="0" BackColor="white"
                    BorderColor="#000c19">
                    <iframe id="iframePrint" name="iframePrint" visible="false" height="475px" width="930px"
                        frameborder="0"></iframe>
                    <!-- close button start -->
                    <center>
                        <a href="javascript:HideNotesModel();" onclick="close_click">Close</a>
                    </center>
                    <img alt="" src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" />
                    <!-- close button end -->
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <input type="hidden" id="hdnBPInfo" name="hdnBPInfo" />
    <!--parentBPInfo and parentGVDataXML stores the parent page info.They are reset to session when child popup is closed-->
    <asp:HiddenField ID="parentBPInfo" runat="server" />
    <asp:HiddenField ID="hdnGVBPEINFO" runat="server" />
    <asp:HiddenField ID="hdnControls" runat="server" />
    <!-- To store the current selected/modified/added row to access both in client side & serverside(shanti) -->
    <asp:HiddenField ID="hdnRwToBeModified" runat="server" />
</asp:Content>
