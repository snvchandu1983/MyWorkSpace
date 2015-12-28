//JS files related top left master
/*IncludeJavaScript('../JavaScript/Master.js');
IncludeJavaScript('../JavaScript/menu.js');*/

IncludeJavaScript(g_cdnScriptsPath+'Master.js');
IncludeJavaScript(g_cdnScriptsPath+'menu.js');

//Includes the given js file into the markup.
function IncludeJavaScript(jsFile)
{
    document.write('<script type="text/javascript" src="'+ jsFile + '"></script>');
}
