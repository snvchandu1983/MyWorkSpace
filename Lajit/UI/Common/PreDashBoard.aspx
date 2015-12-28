<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PreDashBoard.aspx.cs" Inherits="LAjitDev.PreDashBoard"
    Theme="Lajit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>LAjit Technologies, LLC</title>

    <script language="javascript" type="text/javascript">
    
    function Visible(hoverStr, refer)
    {
        var my_array1;
        var my_array2;
        var divId;
        var i; var p;           
        var cmn_enty_str = document.forms["Form1"].elements["hiddenEntyStrID"].value;
        var cmn_cmpny_str = document.forms["Form1"].elements["hiddenCmpnyStrID"].value;
        if(refer == "EntyID")
        {
            //icon image
            my_array1 = cmn_enty_str.split(":");
            for(i=0;i < my_array1.length; i++)
            {
                if (hoverStr == my_array1[i])
                {
                    my_array2 = my_array1[i].split("~");
                    LoadCompanies();
                    for(p=0; p<my_array2[1]; p++)
                    {
                        divId = refer+ my_array2[0]+ p;
                        document.getElementById(my_array2[0]).style.visibility = 'visible';
                        document.getElementById(divId).style.visibility = 'visible';
                    }
                }           
            }
        }
        else if(refer == "CmpnyID")
        {
            my_array1 = cmn_cmpny_str.split(":");
            for(i=0; i< my_array1.length; i++)
            {
                if (hoverStr == my_array1[i])
                {
                    my_array2 = my_array1[i].split("~");
                    LoadRoles();
                    for(p=0; p<my_array2[1]; p++)
                    {
                        divId = refer+ my_array2[0]+ p;                       
                        document.getElementById(divId).style.visibility = 'visible';
                    }
                }           
            }
        }
    }
   
    //for companies  
    function LoadCompanies()
    {
        var cmn_enty_str = document.forms["Form1"].elements["hiddenEntyStrID"].value;
        var my_enty_array1 = cmn_enty_str.split(":");
        for(var i=0; i< my_enty_array1.length; i++)
        {
            var my_enty_array2 = my_enty_array1[i].split("~");
            //To invisible arrow
            document.getElementById(my_enty_array2[0]).style.visibility = 'hidden';
            for(var p=0; p<my_enty_array2[1]; p++)
            {
                var divId = "EntyID"+ my_enty_array2[0]+ p;
                document.getElementById(divId).style.visibility = 'hidden';
            }
        } 
        LoadRoles();
    }
   
    //for roles
    function LoadRoles()
    {
        var cmn_cmpny_str = document.forms["Form1"].elements["hiddenCmpnyStrID"].value;
        var my_cmpny_array1 = cmn_cmpny_str.split(":");
        for(var j=0; j< my_cmpny_array1.length; j++)
        {
            var my_cmpny_array2 = my_cmpny_array1[j].split("~");
            for(q=0; q<my_cmpny_array2[1]; q++)
            {
                var divId = "CmpnyID"+ my_cmpny_array2[0]+ q;                      
                document.getElementById(divId).style.visibility = 'hidden';
            }
        }
    }
    
    //changing the image when mouseover
    function ToggleImage(obj,imgExt)
    {
        if(obj.src.indexOf('-over') == -1)
        {
            var splitValues = obj.src.split('/');
            var img = splitValues[splitValues.length-1];
            var path= obj.src.substring(0,eval(obj.src.length-img.length))
            var spVal = img.split('.');            
            obj.src = path+ spVal[0] + "-over"+ imgExt;
        }
        else
        {
            var temp = obj.src;
            temp = temp.substring(0, temp.lastIndexOf('-over'));
            temp = temp + imgExt;
            obj.src = temp;
        }
    }
    
    //Redirecting to DashBoard when clicking on Role.
    function RedirectPage(rlcmpnyId, rluserId, BPGId)
    {
        document.forms[0].action = "DashBoard.aspx?rcId=" +rlcmpnyId + "&ruId="+rluserId + "&bpgId="+BPGId;
        if (document.forms[0].__VIEWSTATE)
        {
            document.getElementById('__VIEWSTATE').name = 'NOVIEWSTATE';           
        }
        if (document.forms[0].__EVENTTARGET)
        {
            document.getElementById('__EVENTTARGET').name = ''; 
        }   
         if (document.forms[0].__EVENTARGUMENT)
        {
            document.getElementById('__EVENTTARGET').name = ''; 
            
        }
        document.forms[0].method= "post";
        document.forms[0].submit();
        return ;
    }
    </script>

</head>
<body onload="LoadCompanies()" style="background: url(<%=Application["ImagesCDNPath"]%>images/powered-page-bg.png) repeat-x #ffffff;">
    <form id="Form1" action="PreDashBoard.aspx" runat="server">
        <table style="width: 984px;" border="0" cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td valign="top" style="height: 101px;" class="predashhead">
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr align="left" valign="top">
                            <td style="height: 380px">
                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="12" height="1" alt="" />
                            </td>
                            <td style="height: 450px">
                                <table border="0" cellspacing="0" cellpadding="0" style="width: 646px;" align="center">
                                    <tr>
                                        <td align="left" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" align="center">
                                                <tr>
                                                    <td style="background: url(<%=Application["ImagesCDNPath"]%>Images/powered-top.png)" height="9px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="background: url(<%=Application["ImagesCDNPath"]%>images/mid-new-bg.png) repeat-x;
                                                            border-left: solid 1px #ffffff; border-right: solid 1px #ffffff; height: 240px;"
                                                            width="647">
                                                            <tr>
                                                                <td align="left" valign="top" style="background: url(<%=Application["ImagesCDNPath"]%>Images/powered-by-lajit-logo.png) no-repeat;
                                                                    height: 40px">
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="right">
                                                                    <a href="Login.aspx" class="topboldlinks" title="Log Out">Log Out</a>&nbsp;&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 380px; width: 130px;" class="text">
                                                                    <asp:Table ID="entyTbl" runat="server" HorizontalAlign="Right" Height="80px" Width="100px"
                                                                        BorderColor="black" BorderStyle="Dashed" BorderWidth="0">
                                                                    </asp:Table>
                                                                </td>
                                                                <td width="90px" style="height: 380px" class="text" align="center">
                                                                    <asp:Table ID="cmpnyTbl" runat="server" HorizontalAlign="Right" Height="200px" Width="70px"
                                                                        BorderColor="black" BorderStyle="Dashed" BorderWidth="0">
                                                                    </asp:Table>
                                                                </td>
                                                                <td style="height: 380px" width="360px" align="left" valign="middle" class="text">
                                                                    <asp:Table ID="rlTbl" runat="server" HorizontalAlign="Left" Height="80px" Width="200px"
                                                                        BorderColor="black" BorderStyle="Dashed" BorderWidth="0" CssClass="text">
                                                                    </asp:Table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background: url(<%=Application["ImagesCDNPath"]%>Images/powered-bot.png)" height="7px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--<img src="/App_Themes/<%=Session["MyTheme"]%>/Images/waiting-but.gif" onmouseout="javascript:display('EntyID')"></td>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" style="height: 22px;">
                                <img src="<%=Application["ImagesCDNPath"]%>Images/spacer.gif" width="1" height="1" alt="" /></td>
                        </tr>
                        <tr>
                            <td>
                                <input id="hiddenEntyStrID" name="hiddenEntyStrID" runat="server" type="hidden" />
                                <input id="hiddenCmpnyStrID" name="hiddenCmpnyStrID" runat="server" type="hidden" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
