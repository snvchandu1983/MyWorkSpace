var bIdLeftPanel = "BIDLeftPanelCPE";

function SetLeftCollapsibleHandlers()
{
    //Find the Left panel collapser and add event handlers for the Expand/Collapse events.
    var cpeObjectMainGrid = $find(bIdLeftPanel);
    if(cpeObjectMainGrid != null)
    {
        cpeObjectMainGrid.add_expandComplete(expandHandler);
        cpeObjectMainGrid.add_collapseComplete(collapseHandler);
    }
}

//...................................................................................................//

//Sets the widht of the active header element of the page to it's appropriate width on Page Load depending on the 
//state of the left panel(Expanded or collapsed)
function SetTitleWidth()
{
//    var cpeLeftPanel = $find(bIdLeftPanel);
//    if(cpeLeftPanel && cpeLeftPanel._checkCollapseHide())
    var imgPath=$get('ctl00_pnlArrow').src;
        if(imgPath && imgPath.toUpperCase().indexOf('CLOSE-ARROW') !==-1) 
    {
        var pnlHeaderTitle = document.getElementById('tdCenterPanel');//DashBoard
        if(pnlHeaderTitle == null)
        {
            return;
            //Check for other header elements such as pnlCPGV1Title.
            pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_pnlCPGV1Title');
            if(pnlHeaderTitle == null)
            {
                pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_pnlEntryFormTitle');
                if(pnlHeaderTitle == null)
                {
                    pnlHeaderTitle = document.getElementById('divShippingProviders');//Shipment Page
                    if(pnlHeaderTitle == null)
                    {
                        alert('No header elements were not found in the page.');
                    }
                }
                else
                {
                    var temp = document.getElementById('ctl00_cphPageContents_htcEntryFormAuto');
                    temp.width = 926 - get_previoussibling(get_previoussibling(temp)).width - 28;
                }
            }
            else
            {
                var temp = document.getElementById('ctl00_cphPageContents_htcCPGV1Auto');
                temp.width = 926 - get_previoussibling(get_previoussibling(temp)).width - 28;
            }
        }
        else
        {
            //pnlHeaderTitle(actaully the TD containing all the headers) comes from DashBoard
            //642 was previously 926
            var temp = document.getElementById('ctl00_cphPageContents_htcCPGV1Auto');
            temp.width = 642 - get_previoussibling(get_previoussibling(temp)).width - 28;
            temp = document.getElementById('ctl00_cphPageContents_htcCPGraphicsAuto');
            temp.width = 642 - get_previoussibling(get_previoussibling(temp)).width - 28;
            temp = document.getElementById('ctl00_cphPageContents_htcCPGV2Auto');
            temp.width = 642 - get_previoussibling(get_previoussibling(temp)).width - 28;
            temp = document.getElementById('tdExpViewBG');
            temp.width = 642 - get_previoussibling(get_previoussibling(temp)).width - 28;
        }
    }
}

//...................................................................................................//

function get_previoussibling(n)
{
    x = n.previousSibling;
    while (x.nodeType != 1)
    {
        x = x.previousSibling;
    }
    return x;
}

//...................................................................................................//

//Event handler for Expand Complete event of the CPE
function expandHandler(sender, args)
{
//    SetCookie("Collapsed", "false","/");
    AjaxMethods.SetCollapsedState("0");
//    document.getElementById('ctl00_hdnLPCollapsed').value="0";
    SetModalPopUpCoordinates();
    if(document.all)
    {
//        document.getElementById('tdContentPage').style.width = "100%";
    }
    else
    {
          document.getElementById('tdContentPage').style.width = "100%";
    }
}

//...................................................................................................//

//Event handler for Collapse Complete event of the CPE
function collapseHandler(sender, args)
{
//    SetCookie("Collapsed", "true","/");
    AjaxMethods.SetCollapsedState("1");
//    document.getElementById('ctl00_hdnLPCollapsed').value="1";
    SetModalPopUpCoordinates();
    if(!document.all)
    {
        document.getElementById('tdContentPage').style.width = "auto";
    }
}

//...................................................................................................//

//Event handler for the Left Panel Arrow Image Button click
function LeftPanelClick()
{

}   
 
 //...................................................................................................//
 
function MaintainCollapsibleState()
{
    //alert(GetCookie("Collapsed"));
//    var cpeLeftPanel = $find(bIdLeftPanel);
//    if(!cpeLeftPanel)
//     return;
//    var state=GetCookie("Collapsed");
////    var animation= cpeLeftPanel._animation;
////    cpeLeftPanel._animation=null;
//    if(state&&state=="true")
//    {
//        //cpeLeftPanel.set_Collapsed(true);//Collapse the CPE
//        cpeLeftPanel._doClose();
//    }
//    else
//    {
//        //cpeLeftPanel.set_Collapsed(false);//Expand the CPE   
//        cpeLeftPanel._doOpen();
//    }
}   

//...................................................................................................//

//Sets a cookie with the given values. Expires value is automatically set to a date one year from present.
function SetCookie( name, value, path, domain, secure)
{
    var cookie_string = name + "=" + escape ( value );
    var expires = new Date ();
    expires.setTime(expires.getTime() + (24 * 60 * 60 * 1000 * 365));
    cookie_string += "; expires=" + expires.toGMTString();
    if ( path )
        cookie_string += "; path=" + escape ( path );
    if ( domain )
        cookie_string += "; domain=" + escape ( domain );
    if ( secure )
        cookie_string += "; secure";
    document.cookie = cookie_string;
}

//...................................................................................................//

function GetCookie(cookie_name)
{
  var results = document.cookie.match('(^|;)?' + cookie_name + '=([^;]*)(;|$)');
  if (results)
    return (unescape(results[2]));
  else
    return null;
}

//...................................................................................................//
