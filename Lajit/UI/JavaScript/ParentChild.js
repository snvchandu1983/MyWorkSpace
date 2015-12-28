
//CDN

IncludeJavaScript(g_cdnScriptsPath+'Utility.js');
IncludeJavaScript(g_cdnScriptsPath+'Common.js');
IncludeJavaScript(g_cdnScriptsPath+'AutoFill.js');
IncludeJavaScript(g_cdnScriptsPath+'ButtonsUserControl.js');
IncludeJavaScript(g_cdnScriptsPath+'Calculations.js');
IncludeJavaScript(g_cdnScriptsPath+'ChildGridView.js');
//IncludeJavaScript('../JavaScript/LeftCollapsiblePanel.js')
IncludeJavaScript(g_cdnScriptsPath+'ExplodedView.js');
IncludeJavaScript(g_cdnScriptsPath+'GridViewUserControl.js');
IncludeJavaScript(g_cdnScriptsPath+'FrameManager.js');


/*IncludeJavaScript('../JavaScript/Utility.js');
IncludeJavaScript('../JavaScript/Common.js');
IncludeJavaScript('../JavaScript/AutoFill.js');
IncludeJavaScript('../JavaScript/ButtonsUserControl.js');
IncludeJavaScript('../JavaScript/Calculations.js');
IncludeJavaScript('../JavaScript/ChildGridView.js');
//IncludeJavaScript('../JavaScript/LeftCollapsiblePanel.js')
IncludeJavaScript('../JavaScript/ExplodedView.js');
IncludeJavaScript('../JavaScript/GridViewUserControl.js');
IncludeJavaScript('../JavaScript/FrameManager.js');*/



//LoadScripts();

//function LoadScripts()
//{
//    var locSplit=window.location.href.split('/');
//    var pageName=locSplit[locSplit.length-1];
//    if(pageName=='GenProcessEngine.aspx'||pageName=='GenView.aspx')
//    {
//        //console.log('Loading Custom scripts for JqGrid');
//    }
//    else
//    {
//        //console.log('Loading Conventional scripts');
//        IncludeJavaScript('../JavaScript/AutoFill.js');
//        IncludeJavaScript('../JavaScript/ButtonsUserControl.js');
//        IncludeJavaScript('../JavaScript/Calculations.js');
//        IncludeJavaScript('../JavaScript/ChildGridView.js');
//    }
//    //Common scripts
//    IncludeJavaScript('../JavaScript/Common.js');
//    IncludeJavaScript('../JavaScript/ExplodedView.js');
//    IncludeJavaScript('../JavaScript/GridViewUserControl.js');
//    IncludeJavaScript('../JavaScript/Utility.js');
//}

//Includes the given js file into the markup.
function IncludeJavaScript(jsFile)
{
    var oHead = document.getElementsByTagName('head')[0];
    var oScript = document.createElement('script');
    oScript.type = 'text/javascript';
    oScript.charset = 'utf-8';
    oScript.src = jsFile;
    oHead.appendChild(oScript);        
}