<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ChildGridView.ascx.cs"
    Inherits="LAjitDev.UserControls.ChildGridView" %>
    <script type="text/javascript">
     function CPGVExpandCollapse(oTitleID)
     {
          var oImgID; 
          var TitleID=oTitleID.split('_');
          var oContentID='ctl00_cphPageContents_CGVUC_'+TitleID[3]+'Target';
          var i=TitleID[3].indexOf('pnl');
          if(i!==-1)
          {
            i=i+3;
            var j=TitleID[3].length;
            oImgID='img'+TitleID[3].substring(i,j)+'Expand';
          }
           
           jQuery('#'+oContentID).slideToggle('slow');
           var imgIcon=document.getElementById(oImgID).getAttribute('src'); 
           var imgIconSrc=imgIcon.split('/');
           
           if(imgIconSrc[4]=="plus-icon.png")
           {
               jQuery("#"+oImgID).attr("src",g_cdnImagesPath+"Images/minus-icon.png"); 
           }
           else
           {
               jQuery("#"+oImgID).attr("src",g_cdnImagesPath+"Images/plus-icon.png"); 
           }
           
       }  
    </script>
<asp:UpdatePanel ID="upCGVUC" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="tblCGVTop" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlTitle" runat="server">
                        <table cellpadding="0" cellspacing="0" style="width: 100%; cursor: pointer; border-width: 1px;
                            border-color: #d7e0f1; border-left-style: double; border-right-style: double;">
                            <tr>
                                <td id="htcStartSpacer" runat="server" visible="false" class="grdVwRPTitle" style="height: 24px;
                                    width: 20px" > 
                                    &nbsp;
                                </td>
                                <td id="htcExpandImage" runat="server" visible="false" class="grdVwRPTitle" style="height: 24px;
                                    width: 20px" align="center">
                                    <img id="imgTitleExpand" alt="curveRight" src="<%=Application["ImagesCDNPath"]%>Images/minus-icon.png" />
                                </td>
                                <td id="htcHeader" runat="server" align="center" class="grdVwRPTitle" style="width: 100%" >
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlTitleTarget" runat="server">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlGVBranch" runat="server" ScrollBars="horizontal" Width="768px"
                                        BorderColor="#d7e0f1" HorizontalAlign="Left">
                                        <asp:GridView runat="server" ID="grdVwBranch" SkinID="BranchGVStyle" OnRowDataBound="OnRowDataBound"  OnPageIndexChanging="grdVwBranch_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBxSelectRow" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Table ID="tblBPCLinks" runat="server">
                                                    <asp:TableRow HorizontalAlign="Center">
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td>
                                                <asp:Table ID="tblOpLinks" runat="server">
                                                    <asp:TableRow HorizontalAlign="Center">
                                                        <asp:TableCell>
                                                            <asp:LinkButton ID="lnkBtnAddRows" runat="server" OnClick="lnkBtnAddNewRow_Click"
                                                                Text="Add More Records"></asp:LinkButton>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="addSeperator" Style="padding-left: 3px; padding-right: 3px;">|</asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:LinkButton ID="lnkBtnDeleteRow" runat="server" Text="Delete">
                                                            </asp:LinkButton>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="deleteSeperator" Style="padding-left: 3px; padding-right: 3px;">|</asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:LinkButton ID="lnkBtnToggle" runat="server" Text="Toggle Selection">
                                                            </asp:LinkButton>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="toggleSeperator" Style="padding-left: 3px; padding-right: 3px;">|</asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:LinkButton ID="lnkBtnCopy" runat="server" Text="Copy">
                                                            </asp:LinkButton>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="copySeperator" Style="padding-left: 3px; padding-right: 3px;">|</asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:LinkButton ID="lnkBtnPaste" runat="server" Text="Paste">
                                                            </asp:LinkButton>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td>
                                                <asp:Table ID="tblAmounts" runat="server">
                                                    <asp:TableRow HorizontalAlign="Center">
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <!-- Variable to store the client-side deletions made in the child grid view -->
        <asp:HiddenField ID="hdnRowsToDelete" runat="server" />
        <!-- Variable to store the number of rows in the child grid view before addition-->
        <asp:HiddenField ID="hdnRowCount" runat="server" />
        <!-- Variable to store the rows added in the child grid view-->
        <asp:HiddenField ID="hdnAddedRows" runat="server" />
        <asp:HiddenField ID="hdnRowsToDisplay" runat="server" />
        <asp:HiddenField ID="hdnBranchNodeName" runat="server" />
        <asp:HiddenField ID="hdnModRows" runat="server" />
        <asp:HiddenField ID="hdnAmounts" runat="server" />
        <asp:HiddenField ID="hdnIsPaging" runat="server" Value="0" />
         <asp:HiddenField ID="hdnJobCOADefault" runat="server" Value="0" />
    </ContentTemplate>
</asp:UpdatePanel>
 