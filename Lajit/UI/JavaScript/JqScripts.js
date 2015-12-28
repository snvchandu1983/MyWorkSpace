
LoadScripts();

function LoadScripts()
{
    /*IncludeJavaScript('../JavaScript/Common.js');
    IncludeJavaScript('../JavaScript/ExplodedView.js');
    IncludeJavaScript('../JavaScript/GridViewUserControl.js');
    IncludeJavaScript('../JavaScript/Utility.js');*/
    
    
    //CDN
    
    IncludeJavaScript(g_cdnScriptsPath+'Common.js');
    IncludeJavaScript(g_cdnScriptsPath+'ExplodedView.js');
    IncludeJavaScript(g_cdnScriptsPath+'GridViewUserControl.js');
    IncludeJavaScript(g_cdnScriptsPath+'Utility.js');
    
    
//    IncludeJavaScript('../JavaScript/JQMethods.js');
//    IncludeJavaScript('../JavaScript/grid.locale-en.js');
//    IncludeJavaScript('../JavaScript/grid.loader.js');
//    IncludeJavaScript('../JavaScript/jquery-ui-1.7.2.custom.min.js');
//    IncludeJavaScript('../JavaScript/AjaxFileUpload.js');
}

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
