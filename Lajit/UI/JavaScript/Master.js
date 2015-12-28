//initalization
var g_IntialValue;
var g_expandCollapseSize;
var g_arrTDs;
var g_arrTdsItemCount;
var g_ObjJqGrid;
var firstRun=true;
var firstRun1=true;
//var MyThemevalue; //='<%=Session("MyTheme")%>'; 
//

jQuery(document).ready(MasterLoad);


//Floating message
function MasterLoad()
{
    //scroll the message box to the top offset of browser's scrool bar
    /* jQuery('#floating_box').floating({targetX:eval((screen.width-1024)/2+915), targetY:"bottom"});
    jQuery('#floating_box').click(function(){ jQuery('#floating_box').fadeOut("slow"); });*/

    //Qtip Start
    jQuery('#ctl00_tableShortcuts td[qtip]' ) .each(function(i, qTipTd){
    var jqTd= jQuery(qTipTd)
    var qTipText=jqTd.attr('qtip');
    jqTd.children().qtip
    ({
        content: qTipText,
        show: 'mouseover',
        hide: 'mouseout',
        position:{corner:{target:'topLeft',tooltip:'bottomRight'},adjust: { screen: true } },
        style: { background:'white','font-weight': 'bold', tip:'bottomRight',padding:5,name: 'grid',border: { width: 1, radius: 3,color:'#4C708A' } }
        });
    });
    //Qtip End
    
    //Initalise  home autofill
    HomeAutofill();
}
	        
//...................................................................................................//
      
function GoToHome()
{
    window.location = "../Common/DashBoard.aspx";
}
//...................................................................................................//
function FormatMenuItem(row)
{
    return row.text;
}
//...................................................................................................//

function InitTT()
{
    return $get('divMenuSearch').innerHTML;
}
//...................................................................................................//
//Home menu autofill getdata
function GetData()
{
    var arr=jQuery('li:has(ul) a[href]');
    var retArr=new Array();
    var containerID='';
    var parent='';
    arr.each(function(i,n){
    if(n.getAttribute('class')=='parent')
    {
        parent=jQuery(this).clone(true).children('span').css({color: 'black'}).html();
    }
    else
    {
        var obj=new Object();
        var item=jQuery(this).clone(true).children('span').css({color: 'black'});
        obj.text='<b>'+parent+'</b> >> '+item.html();
        obj.value=n.getAttribute('href');
        retArr.push(obj);
    }
    });
    return retArr;
}
//...................................................................................................//
 
function HomeAutofill()
{
    var obj$Home=jQuery('#MenuHome');
    var objQTip = obj$Home.qtip(
    {
    content: { text:InitTT()  , title: { text: 'Menu Search'} },
    position:{corner: { target: 'bottomLeft' },adjust: {screen: true} },
    // Make it fixed so it can be hovered over
    hide: {delay:1000, fixed: true },
    style: { padding: '1px 1px',name: 'grid', border: { width: 1, radius: 0 } },
    show: { effect: { type: 'slide' } },
    api:{beforeShow:function(){
    if(!firstRun)
    {
        return;
    }
    var objTarget=jQuery(".qtip.qtip-grid input");
    objTarget.autocomplete(GetData(), {
                        width: 300,
                        multiple: false,
                        matchContains: true,
                        scrollHeight: 300,
                        max: 900,
                        cacheLength: 0,
                        minChars: 1,
                        formatItem: FormatMenuItem
               })
          .result(function(event, item){
                                        jQuery('#MenuHome').qtip('hide');
                                        setTimeout(item.value,0);
                                    })
          .bind("keyup",function(){ if(firstRun1){jQuery('.ac_results').bind('mouseover',function(){jQuery('#MenuHome').qtip('show');});}firstRun1=false;   });
    firstRun=false;
    }
     /*,onShow:function(obj){ toggleImage(obj$Home[0]); }//jQuery(".qtip.qtip-grid input").focus(); 
                            ,onHide:function(obj){ toggleImage(obj$Home[0]); }*/
   }
  });
}
//...................................................................................................//

function LeftClick(sender)
{
    var jQSender=jQuery(sender);
    var imgPath=jQSender.attr('src'); 
    var animationTime=400;
    InitValues();
    GetTDArray();
    var isCollaspsing=false;
    if(imgPath.toUpperCase().indexOf('CLOSE-ARROW') !==-1)  {    
        //Reset the width of the main title if set previously
        var pnlTitle=$get('ctl00_cphPageContents_pnlCPGV1Title');
        if(pnlTitle) {
            pnlTitle.style.width='100%';   
        }
       
    jQSender.attr("src",g_cdnImagesPath+"Images/open-arrow.gif"); 
    isCollaspsing=true;
    AjaxMethods.SetCollapsedState("1");
    //jQuery('#floating_box').animate({width :"+=147"},animationTime);
    }
    else {
        jQSender.attr("src",g_cdnImagesPath+"Images/close-arrow.gif");
        AjaxMethods.SetCollapsedState("0");
    }  
    jQuery("#ctl00_pnlLeftPanel").animate({ width: "toggle" }, {duration:animationTime, step:function(val){SetHeaderWidth(val,isCollaspsing); } });
    return true; 
}
//...................................................................................................//
        
function SetHeaderWidth(val,isCollapsing)
{
    var percentage;
    if(!isCollapsing) {
    //Animation is expanding the target(Left Panel)
    percentage=val/149; 
    }
    else {
    percentage=(val/149)-1; 
    }
    var percentageChange = (g_expandCollapseSize * percentage);// / 100;
    //            //For Tabbed Headers
    //            for(var i = 0; i < g_arrTDs.length - 1; i++)
    //            {
    //                var htcElement = document.getElementById(g_arrTDs[i]);
    //                if(htcElement)
    //                {
    //                    // New Auto Grow Element's width = Total Header's width - Width of title element - Width of Curves & plus/minus(28) - percentageDecrease
    //                    var titleElementWidth = get_previoussibling(get_previoussibling(htcElement)).width;
    //                    var calcNewAGWidth = g_IntialValue - percentageChange - titleElementWidth.replace("px","") - 28;
    //                    if(htcElement.id == 'ctl00_cphPageContents_htcShipTo' || htcElement.id == 'ctl00_cphPageContents_htcShipFrom')
    //                    {
    //                        htcElement.width = calcNewAGWidth / 2;
    //                    }
    //                    else
    //                    {
    //                        if(Sys.Browser.name == 'WebKit')//Safari patch
    //                        {
    //                            calcNewAGWidth += 149;
    //                        }
    //                        htcElement.width = calcNewAGWidth;
    //                    }
    //                }
    //            }
    //For Plain Headers
    var pnlChildGV = document.getElementById(g_arrTDs[g_arrTdsItemCount-1]);
    if(pnlChildGV) {
    var calcWidth = g_IntialValue - 4 - percentageChange;//-4 is to compensate the decrease in width of the branch panel as compared to the title element
    pnlChildGV.style.width = calcWidth + "px";
    }

    //For jqGrid
    if(typeof(gridTableId)!='undefined')
    {
    var calcWidth = g_IntialValue - percentageChange;
    g_ObjJqGrid.setGridWidth(calcWidth,true);

    //If a edit/add form is in view expand/collapse that also
    var jqForm=$get('editmodtblGrid')||$get('viewmodtblGrid');
    if(jqForm)
    {
        var newWidth=calcWidth;
        if(g_IsProcEngine)
        {
            newWidth+=50;
        }
        jqForm.style.width=newWidth+'px';
    }
    }        
}
//...................................................................................................//
       
function GetTDArray()
{
    var arrTDsItemCount = 1;//12;
    g_arrTdsItemCount = 1;//12;
    var arrTDs = new Array(arrTDsItemCount);


    arrTDs[0] = 'ctl00_cphPageContents_CGVUC_pnlGVBranch';


    //            arrTDs[0] = 'ctl00_cphPageContents_htcCPGV1Auto';//All Pages
    //            arrTDs[1] = 'ctl00_cphPageContents_htcCPGraphicsAuto';//DashBoard
    //            arrTDs[2] = 'ctl00_cphPageContents_htcCPGV2Auto';//DashBoard
    //            arrTDs[3] = 'tdExpViewBG';//DashBoard
    //            arrTDs[4] = 'ctl00_cphPageContents_htcShipperDetails';//Ship
    //            arrTDs[5] = 'ctl00_cphPageContents_htcCredentialInformation';//Ship
    //            arrTDs[6] = 'ctl00_cphPageContents_htcTrackingDetails';//Ship
    //            arrTDs[7] = 'ctl00_cphPageContents_htcShipTo';//Tab Header spanning half the length of the main header//Ship
    //            arrTDs[8] = 'ctl00_cphPageContents_htcShipFrom';//Tab Header spanning half the length of the main header//Ship
    //            arrTDs[9] = 'ctl00_cphPageContents_htcPackageDetails';//Ship
    //            arrTDs[10] = 'ctl00_cphPageContents_htcEntryFormAuto';//All Pages except DashBoard
    //            var currentPage = window.location + "";
    //            if(currentPage.toUpperCase().indexOf('COMMERCIAL.ASPX') == -1) {
    //                //For all pages except Commercial adjust the widths.
    //                arrTDs[11] = 'ctl00_cphPageContents_CGVUC_pnlGVBranch';
    //            }
    //            else  {
    //                arrTDs[11]='';
    //            }
    g_arrTDs=arrTDs;
    return arrTDs;
    }

    //Inits the global variables like the Total Title bar width and animation size.
    function InitValues()
    {
    //Added by Danny to set the widths of the Headers  when the left panel is animating
    //Get the dimensions of the title header element.
    var pnlHeaderTitle = document.getElementById('tdCenterPanel');//DashBoard
    if(pnlHeaderTitle == null)  {
    //Check for other header elements such as pnlCPGV1Title.
    pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_pnlCPGV1Title');
    if(pnlHeaderTitle == null)  {
        pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_pnlEntryFormTitle');
        if(pnlHeaderTitle == null) {
            pnlHeaderTitle = document.getElementById('divShippingProviders');//Shipment Page
        }
    }
    }

    //Set the g_expandCollapseSize variable
    var currentPage = window.location + "";
    if(currentPage.toUpperCase().indexOf('DASHBOARD.ASPX') == -1)  {
    //Not DashBoard
    g_expandCollapseSize = 149;
    }
    else if(currentPage.toUpperCase().indexOf('SHIP.ASPX') != -1)  {
    //Shipment Page
    g_expandCollapseSize = 149;
    }
    else  {
    //DashBoard
    g_expandCollapseSize = 98.34;
    }

    if(pnlHeaderTitle)  {
    var elementBounds = Sys.UI.DomElement.getBounds(pnlHeaderTitle);
    g_IntialValue = elementBounds.width;
    }
    else  {
    g_IntialValue = null;
    //Check for existence of JqGrid
    if(typeof(gridTableId)!='undefined')
    {
        g_ObjJqGrid=jQuery(gridTableId);
        g_IntialValue=g_ObjJqGrid.getGridParam('width');
    }
    }
    return pnlHeaderTitle;
}
//...................................................................................................//
function get_previoussibling(n)
{
    var x = n.previousSibling;
    while (x.nodeType != 1)
    {
        x = x.previousSibling;
    }
    return x;
}
//...................................................................................................//
function GetElementWidth(element)
{
    var elementBounds = Sys.UI.DomElement.getBounds(element);
    return elementBounds.width;
}
//...................................................................................................//
function onSendEmailClick()
{
   var arrElements = document.getElementsByName('ctl00$txtEmailing');
   var txtemailids;
   for(var i=0;i<arrElements.length;i++)
   {
    if(arrElements[i].value !='')
    {
        txtemailids=arrElements[i].value;
        if(txtemailids!=null)
        {
          break;
        }
     } 
   }
   var currBPGID=document.getElementById('hdnCurrentBPGID').value; 
   var currCOXMLID=document.getElementById('hdnCurrentCOXMLValues').value; 
   var COXML=document.getElementById(currCOXMLID).value; 
   AjaxMethods.Submit(currBPGID,COXML,txtemailids,callback_SentMails);
}   
//...................................................................................................//
function OnLogOut()
{
    var pnlLeft=jQuery("#ctl00_pnlLeftPanel");
    if(pnlLeft.css('display')=='none')
    {
        var jQSender=jQuery('#ctl00_pnlArrow');
        jQSender.attr("src",g_cdnImagesPath+"Images/close-arrow.gif"); 
        pnlLeft.css('display','block');
    }
    window.scrollBy(0,400);
    //var confirmStr='You  have ' + rowCnt + ' unposted invoice transaction(s). Please click "Ok" to logout else press "Cancel"';
    var confirmStr='You have unposted transaction(s). Please click "Ok" to logout else press "Cancel"';
    return confirm(confirmStr);
}
//...................................................................................................//
     
        
      