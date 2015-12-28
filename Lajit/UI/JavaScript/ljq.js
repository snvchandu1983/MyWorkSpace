
//IncludeJavaScript('../JavaScript/jquery.metadata.js');
//IncludeJavaScript('../JavaScript/jquery.mbMenu.js');
//IncludeJavaScript('../JavaScript/jquery.hoverIntent.js');

//CDN
IncludeJavaScript(g_cdnScriptsPath+'jquery.floatingbox.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.autocomplete.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.calculator.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.clean.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.datepicker.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.dialog.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.maskedinput.js');
IncludeJavaScript(g_cdnScriptsPath+'jquery.qtip.js');
//IncludeJavaScript('../JavaScript/jquery.sidebar.js');



//Includes the given js file into the markup.
function IncludeJavaScript(jsFile)
{
    document.write('<script type="text/javascript" src="'+ jsFile + '"></script>');
}