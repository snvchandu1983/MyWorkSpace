<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Codebehind="MyReports.aspx.cs"
    EnableEventValidation="false" Inherits="LAjitDev.Reports.MyReports"%>

<asp:content id="Content1" contentplaceholderid="cphPageContents" runat="server">

    <script type="text/javascript">
    
    window.onload=function(e)
            {
                var jqObj=$('#ctl00_cphPageContents_pnlPrint');
                if(jqObj)
                {
                     jqObj.dialog({ autoOpen: false,
                                         bgiframe: false,
                                         width:'auto',
                                         position: 'center',
                                         minHeight: 10,
			                             modal: true,
			                             draggable:false,
			                             resizable:false,
                                        });
                    jqObj.removeClass("ui-dialog-content ui-widget-content");
                    jqObj.dialog().parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                }
                
                jqObj=$('#ctl00_cphPageContents_pnlDelete');
                if(jqObj)
                {
                     jqObj.dialog({ autoOpen: false,
                                         bgiframe: false,
                                         width:'auto',
                                         position: 'center',
                                         minHeight: 10,
			                             modal: true,
			                             draggable:false,
			                             resizable:false,
                                        });
                    jqObj.removeClass("ui-dialog-content ui-widget-content");
                    jqObj.dialog().parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                }
            }

    function expandcollapse(obj,row)
    { 
 
    
        var img = document.getElementById('img' + obj);
        var div = document.getElementById(obj);
        //var cdnImagesPath=$get('ctl00_hdnImagesCDNPath').value;
        if( div != null) 
        {
            if (div.style.display == "none")
            {
                div.style.display = "block";
                if (row == 'alt')
                {
                    img.src = g_cdnImagesPath+"Images/minus-icon.png";
                }
                else
                {
                    img.src =  g_cdnImagesPath+"Images/minus-icon.png";
                }
            }
            else
            {
                div.style.display = "none";
                if (row == 'alt')
                {
                    img.src= g_cdnImagesPath+"Images/add_symbol.gif";
                }
                else
                {
                    img.src= g_cdnImagesPath+"Images/add_symbol.gif";
                }
            }
        }
    }   
  
    function UpdateRadioData(index)
    {
        var radObj=document.getElementsByName("rbtngvCell")[index];
        document.getElementById("ctl00_cphPageContents_hdnSelectedRows").value=radObj.value;
        MyReports.SetSelectedRadiobutton(radObj.value, callback_SetSelectedRadiobutton);
    }
 
    //CallBack handler for UpdateShrtCuts method.
    function callback_SetSelectedRadiobutton(response)
    {
        if (response.error != null)
        {
            alert(response.error);
            return;
        }
    }
 
    function ValidateRadioData()
    {
        var strAlert;
        strAlert=0;
        //var MyTheme="";
        //MyTheme='<%=Session["MyTheme"]%>';
        //var cdnImagesPath=$get('ctl00_hdnImagesCDNPath').value;
        if(document.getElementById("ctl00_cphPageContents_hdnSelectedRows").value=="")
        {
            document.getElementById("divMsg").style.display="block"; 
            document.getElementById("divPrint").style.display="none"; 
            document.getElementById("ctl00_cphPageContents_imgbtnPrintOk").style.visibility="hidden";
            document.getElementById("divExport").style.display="none"; 
            document.getElementById("ctl00_cphPageContents_imgbtnPrintCancel").src=g_cdnImagesPath+"Images/ok-but.png";
        }
        else
        {
            document.getElementById("divMsg").style.display="none"; 
            document.getElementById("divPrint").style.display="block"; 
            document.getElementById("divExport").style.display="block"; 
            document.getElementById("ctl00_cphPageContents_imgbtnPrintOk").style.visibility="visible";
            document.getElementById("ctl00_cphPageContents_imgbtnPrintCancel").src=g_cdnImagesPath+"Images/cancel-but.png";
        }
    }

    function ValidateRadioForDelete()
    {
        var strAlert;
        strAlert=0;
        //var MyTheme="";
        //MyTheme='<%=Session["MyTheme"]%>';
        if(document.getElementById("ctl00_cphPageContents_hdnSelectedRows").value=="")
        {
            document.getElementById("divDelete").style.display="block"; 
            document.getElementById("divDeleteConfimBox").style.display="none"; 
            document.getElementById("divOkToDelete").style.display="none"; 
            document.getElementById("ctl00_cphPageContents_imgbtnDelOk").style.visibility="hidden";
            document.getElementById("ctl00_cphPageContents_imgbtnDelCancel").src=g_cdnImagesPath+"Images/ok-but.png";
            return false;
        }
        //divOkToDelete
        else
        {
           if(GetAttributeStatus("OkToDelete"))
           { 
              //OkToDelete is True   
              document.getElementById("divDelete").style.display="none"; 
              document.getElementById("divDeleteConfimBox").style.display="block"; 
              document.getElementById("divOkToDelete").style.display="none"; 
              document.getElementById("ctl00_cphPageContents_imgbtnDelOk").style.visibility="visible";
              document.getElementById("ctl00_cphPageContents_imgbtnDelCancel").src=g_cdnImagesPath+"Images/cancel-but.png";
              return true;
           }
           else
           {
             //OkToDelete is flase   
            document.getElementById("divDelete").style.display="none"; 
            document.getElementById("divDeleteConfimBox").style.display="none"; 
            document.getElementById("divOkToDelete").style.display="block"; 
            document.getElementById("ctl00_cphPageContents_imgbtnDelOk").style.visibility="hidden";
            document.getElementById("ctl00_cphPageContents_imgbtnDelCancel").src=g_cdnImagesPath+"Images/ok-but.png";
            return false;
           }
        }
    }

    var _source;
    //Report Panel        
    function cancelClick()
    {
         DisplayPopUp(false,'ctl00_cphPageContents_pnlPrint')
    }
 
    function Hide()
    {
        DisplayPopUp(false,'ctl00_cphPageContents_pnlPrint')
        ShowPDF(); 
        return false;
    }
    
    function ShowPDF()
    {
      MyReports.SetSelectedRadiobuttonSession(callback_SetSelectedRadiobutton);
      var redirectPage='ShowPDF.aspx'; 
      var rptType;
     //PDF, Excel, HTML 
      if(document.getElementById('ctl00_cphPageContents_rbPDF').checked==true)
      {
         rptType="PDF";
      }
      else if(document.getElementById('ctl00_cphPageContents_rbExcel').checked==true)
      {
         rptType="Excel";
      }
      else if(document.getElementById('ctl00_cphPageContents_rbHTML').checked==true)
      {
        rptType="HTML";
      }
      var curobject=document.getElementById('ctl00_cphPageContents_hdnSelectedRows').value; 
      var xmlDoc = loadXMLString(curobject);
      var nodeRow = xmlDoc.getElementsByTagName("Rows")[0]; 
      var curobjTrxID=nodeRow.getAttribute("TrxID");;
      var curobjTrxType=nodeRow.getAttribute("TrxType");;
      redirectPage='../PopUps/'+redirectPage+'?PopUp=PopUp&RptType='+rptType; 
      window.open(redirectPage,'_blank');
      return false;
    }  
     
    function ShowModalPopup()
    {
        //$find("mpePrint").click();
        DisplayPopUp(true,'ctl00_cphPageContents_pnlPrint')
    }
    
    function HideModalPopup()
    {
        //$find("mpePrint").hide();        
         DisplayPopUp(true,'ctl00_cphPageContents_pnlPrint')
    }
    
    // Message Panel            
    function OnCancel()
    {
        DisplayPopUp(false,'ctl00_cphPageContents_pnlPrint')
    }
    
    //Delete panel options  
    function DeleteCancelClick()
    {
        DisplayPopUp(false,'ctl00_cphPageContents_pnlDelete');
    }
    
    function DeleteHide()
    {
        DisplayPopUp(false,'ctl00_cphPageContents_pnlDelete');
    }
    
    function DeleteOkClick(sender, e)
    { 
        alert('DeleteOkClick(sender, e)');
       __doPostBack('ctl00$cphPageContents$imgbtnDelOk', e); 
    }
    
    
    function GetAttributeStatus(strAttribute)
    {
        var hdnRwXML = document.getElementById('ctl00_cphPageContents_hdnSelectedRows').value;
       
        var xDocRowXML = loadXMLString(hdnRwXML);
       
        if(xDocRowXML.getElementsByTagName("Rows")[0].getAttribute(strAttribute)!=null && xDocRowXML.getElementsByTagName("Rows")[0].getAttribute(strAttribute)=="0")
        {
            return false;
        }
        else
            return true;
    }
    
    function ShowToolTip(objTarget,ToolTipTable)
    {
    //alert(objTarget +'CHECK OUT'+ ToolTipTable);
    
         jQuery("#"+objTarget).qtip({content: {text: ToolTipTable,
                                    title: { text: 'My Reports' } },
					                position:{corner:{target:'bottomRight',tooltip:'topLeft'},adjust: { screen: true } },
                                   hide: {
                                   fixed: false //Make it fixed so it can be hovered over
                                  // hide: { when: { event: 'unfocus' } }
                             },
                             style: {
                                 //tip:'topLeft',padding: '5px 5px',name: 'grid', border: { width: 2, radius: 6 }
                                 tip:'topLeft',padding: '5px 5px',name: 'grid', border: { width: 1, radius: 3 ,color:'#4C708A'}
                             },
                             show: { effect: { type: 'fade' } }
                         });
    }
    
    
    
    </script>

    <!-- inner table start -->
    <table id="tblmain" runat="server" width="100%" border="0" bordercolor="yellow" cellspacing="0" cellpadding="0">
        <tr align="left" valign="top">
            <td style="width: 25px">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <img src="<%=Application["ImagesCDNPath"]%>Images/title-left-curve.gif" height="21"></td>
                        <td valign="top" style="height: 7px; height: 21px; background-color: #c5d5fc;">
                            <img src="<%=Application["ImagesCDNPath"]%>Images/left-top.png" alt="" height="7"
                                width="50"></td>
                        <td valign="middle">
                            <img src="<%=Application["ImagesCDNPath"]%>Images/title-right-curve.gif" height="21"></td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" colspan="3">
                            <table  width="100%" border="0" cellpadding="0" cellspacing="0" class="formmiddle">
                                <tr>
                                    <td align="left" valign="top">
                                        <!-- icons start -->
                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" bordercolor="red">
                                            <tr>
                                                <td style="width: 5px;">
                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1" /></td>
                                                <td valign="top">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 56px;">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" Width="48" ToolTip="Delete" Height="48"
                                                                    OnClientClick="javascript:DisplayPopUp(true,'ctl00_cphPageContents_pnlDelete');ValidateRadioForDelete();return false;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 5px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 56px;">
                                                                <asp:ImageButton ID="imgbtnPrint" runat="server" Width="48" ToolTip="Print" Height="48"
                                                                    OnClientClick="javascript:DisplayPopUp(true,'ctl00_cphPageContents_pnlPrint');ValidateRadioData();return false;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 47px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 47px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top" style="height: 47px;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" valign="middle" style="width: 9px;">
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- icons end-->
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="height: 143px;">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 4px;">
                <img src='<%=Application["ImagesCDNPath"]%>Images/spacer.gif' width="1" height="1"></td>
            <td>
                <table width="100%" border="0" bordercolor="blue" cellspacing="0" cellpadding="0">
                    <tr>
                        <!--class="formmiddle" -->
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr align="left" valign="top">
                                                <td style="width: 6px;">
                                                    <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1"></td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td align="left" valign="middle" class="grdVwRPTitle">
                                                                &nbsp;My Reports</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" bgcolor="White">
                                                                <!-- Reports Tree View Start  class="bigtitle" style="height:66px; " AssociatedUpdatePanelID="UpdPnlReports"-->
                                                                <asp:UpdatePanel ID="UpdPnlReports" runat="server">
                                                                    <ContentTemplate>
                                                                    <script type="text/javascript" >
                                                                       Sys.Application.add_load(JQueryPageEvents);
                                                                      </script> 
                                                                        <asp:Panel ID="pnlMyReports" runat="server">
                                                                            
                                                                            <!-- Nested GridViews Start  #4f9db1 #A7CED8 <HeaderStyle BackColor="#0083C1" ForeColor="White" /> -->
                                                                            <asp:GridView ID="gvTree" runat="server" Width="100%" CellSpacing="0" CellPadding="0"
                                                                                EnableViewState="true" GridLines="None" AutoGenerateColumns="false" Font-Names="Verdana"
                                                                                BorderColor="#cae1e7" OnRowDataBound="gvTree_RowDataBound">
                                                                                <RowStyle BackColor="#A7CED8" />
                                                                                <AlternatingRowStyle BackColor="White" />
                                                                                <HeaderStyle CssClass="myreportHeaderText" BackColor="White" HorizontalAlign="Left" />
                                                                                <FooterStyle BackColor="White" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="myreportHeaderText" ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal ID="litCompanyName" runat="server" Text='<%# Eval("EntityName")%>'></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                                <td colspan="98%" align="right" bgcolor="White">
                                                                                                    <!-- Branch1 code start BorderColor="#0083C1" BorderStyle="Double" -->
                                                                                                    <asp:GridView ID="gvBranch1" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                                                                                                        GridLines="None" BorderColor="#cae1e7" Width="98%" OnRowDataBound="gvBranch1_RowDataBound"
                                                                                                        CellPadding="2" CellSpacing="0">
                                                                                                        <RowStyle BackColor="#BBD2FD" />
                                                                                                        <AlternatingRowStyle BackColor="White" />
                                                                                                        <HeaderStyle CssClass="myreportHeaderText" BackColor="white" ForeColor="#253d62"
                                                                                                            HorizontalAlign="left" />
                                                                                                        <FooterStyle BackColor="White" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="myreportHeaderText" ItemStyle-HorizontalAlign="Left">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Literal ID="litReportCategoryName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Literal>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <!-- Leaf1 start  BorderColor="#0083C1" BorderStyle="Double" OnRowCreated="gvLeaf1_RowCreated"   -->
                                                                                                                    <tr>
                                                                                                                        <td colspan="98%" align="right" bgcolor="White">
                                                                                                                            <asp:GridView ID="gvLeaf1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvLeaf1_RowDataBound"
                                                                                                                                GridLines="None" BorderColor="#cae1e7" Width="98%" EnableViewState="true" CellPadding="0"
                                                                                                                                CellSpacing="0">
                                                                                                                                <RowStyle BackColor="#c6e2ff" />
                                                                                                                                <AlternatingRowStyle BackColor="#c6e2ff" />
                                                                                                                                <HeaderStyle CssClass="myreportHeaderText" ForeColor="#253d62" HorizontalAlign="left" />
                                                                                                                                <FooterStyle BackColor="White" />
                                                                                                                                <Columns>
                                                                                                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="myreportHeaderText" ItemStyle-HorizontalAlign="Left">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <a href="javascript:expandcollapse('b1divLeafCell<%# Container.DataItemIndex + 1 %>', 'one');"
                                                                                                                                                border="0">
                                                                                                                                                <img id="imgb1divLeafCell<%# Container.DataItemIndex + 1 %>" alt="" border="0" src="<%=Application["ImagesCDNPath"]%>Images/add_symbol.gif" /></a>
                                                                                                                                            &nbsp;
                                                                                                                                            <asp:Literal ID="litFileName" runat="server" Text='<%# Eval("ReportCategoryName") %>'></asp:Literal>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField>
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <!-- cell1 colspan="95%" start  BorderColor="#0083C1" OnRowCreated="gvCell1_RowCreated"    BorderStyle="Double" -->
                                                                                                                                            <tr>
                                                                                                                                                <td colspan="97%" align="right">
                                                                                                                                                    &nbsp;
                                                                                                                                                    <div id="b1divLeafCell<%# Container.DataItemIndex + 1 %>" style="display: none; overflow: auto;">
                                                                                                                                                        <asp:GridView ID="gvCell1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvCell1_RowDataBound"
                                                                                                                                                            GridLines="Both" BorderColor="#BBD2FD" Width="97%" CellPadding="2" CellSpacing="0">
                                                                                                                                                            <AlternatingRowStyle BackColor="White" />
                                                                                                                                                            <HeaderStyle CssClass="myreportHeaderText" BackColor="#A7CED8" ForeColor="#253d62"
                                                                                                                                                                HorizontalAlign="left" />
                                                                                                                                                            <FooterStyle BackColor="White" />
                                                                                                                                                            <Columns>
                                                                                                                                                                <asp:TemplateField Visible="true" ItemStyle-CssClass="myreportItemText" ItemStyle-HorizontalAlign="center">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <asp:Literal ID="litRbtn" runat="server" Visible="true" EnableViewState="true"></asp:Literal>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                                <asp:TemplateField ItemStyle-CssClass="myreportItemText" HeaderText="Title" ItemStyle-HorizontalAlign="left"
                                                                                                                                                                    Visible="true">
                                                                                                                                                                    <ItemTemplate>
                                                                                                                                                                        <a id="hypReportNode" class="myreportItemText" runat="server">
                                                                                                                                                                            <%# Eval("ReportTitle") %>
                                                                                                                                                                        </a>
                                                                                                                                                                    </ItemTemplate>
                                                                                                                                                                </asp:TemplateField>
                                                                                                                                                            </Columns>
                                                                                                                                                        </asp:GridView>
                                                                                                                                                    </div>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <!-- cell1 end-->
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                </Columns>
                                                                                                                            </asp:GridView>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <!-- Leaf1 end -->
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                    <!-- Branch1 code end -->
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            <!-- Nested GridViews Start -->
                                                                            <!-- panel for PopControl Extender-->
                                                                            <!-- panel for PopControl Extender-->
                                                                            <asp:HiddenField ID="hdnSelectedRows" runat="server"></asp:HiddenField>
                                                                            <asp:HiddenField ID="hdnReportBPINFO" runat="server" />
                                                                        </asp:Panel>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <!-- Reports Tree View End -->
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" style="height: 167px;">
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!-- inner table end  -->
    <!-- My Reports Section End-->
    <!-- Print Report Start-->
    <asp:Panel ID="pnlPrint" runat="server" Style="display: none;"
        Width="100%">
        <asp:Panel ID="pnlDrag" runat="server" Width="100%">
            <div id="divPrint" visible="false" style="height: 24px; background: url('<%=Application["ImagesCDNPath"]%>Images/right-title-bg.gif') repeat;">
               
                <table border="0" cellpadding="0" width="300px" cellspacing="0">
                    <tr>
                        <td class="grdVwRPTitle">
                            Select type of report to print</td>
                    </tr>
                </table>
            </div>
            <div id="divMsg" visible="false" style="display: block; cursor: move; background-color: #DDDDDD;
                border: solid 1px Gray; color: Black">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="center" valign="top">
                            <p>
                                You have not selected anything.</p>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <div id="divExport" visible="false" style="display: none;">
            <table cellspacing="0" cellpadding="4" border="0">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbPDF" runat="server" Text="PDF" Checked="true" GroupName="Report">
                        </asp:RadioButton></td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbExcel" runat="server" Text="Excel" GroupName="Report"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbHTML" runat="server" Text="HTML" GroupName="Report"></asp:RadioButton>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="4" cellspacing="0" border="0">
            <tr>
                <td valign="top" align="center">
                    <asp:ImageButton ID="imgbtnPrintOk" runat="server" OnClientClick="javascript:return Hide();">
                    </asp:ImageButton>
                    <asp:ImageButton ID="imgbtnPrintCancel" runat="server" OnClientClick="javascript:cancelClick();return false;"></asp:ImageButton>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlDelete" runat="server"  Style="display: none;"
        Width="100%">
        <asp:Panel ID="pnlDeleteDrag" runat="server">
            <div id="divDeleteConfimBox" visible="false" style="display: none; cursor: move;
                background-color: #DDDDDD; border: solid 1px Gray; color: Black">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="center" valign="top">
                            <p>
                                Are you sure to delete this report?</p>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divDelete" visible="false" style="display: block; cursor: move; background-color: #DDDDDD;
                border: solid 1px Gray; color: Black">
                <table cellpadding="0" cellspacing="0" border="0" style="height: 20px">
                    <tr>
                        <td align="center" valign="top">
                            <p>
                                You have not selected anything.</p>
                        </td>
                    </tr>
                </table>
            </div>
             <div id="divOkToDelete" visible="false" style="display: block; cursor: move; background-color: #DDDDDD;
                border: solid 1px Gray; color: Black">
                <table cellpadding="0" cellspacing="0" border="0" style="height: 20px">
                    <tr>
                        <td align="center" valign="top">
                            <p>
                                This Record can not be deleted.</p>
                        </td>
                    </tr>
                </table>
            </div>
            
            <table cellpadding="2" cellspacing="0" border="0" style="width: 200px">
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdpnlDelete" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="imgbtnDelOk" runat="server" OnClick="imgbtnDelOk_Click" OnClientClick="DeleteHide();">
                                </asp:ImageButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgbtnDelCancel" runat="server"  OnClientClick="DeleteHide();"></asp:ImageButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:content>
