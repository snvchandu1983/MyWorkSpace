function OnChartProcessClick(BPINFO, processBPGID, redirectPage, processName, isPopUp)
{
    var COXML=BPINFO.replace(/~::~/g,'"');
    
    if( COXML!="" && processBPGID!="" && redirectPage !="" && processName != "" && isPopUp !="")
    {
      CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML);
    }
}

//...................................................................................................//
//Show pdf chart in a new window
function ShowChartPDF(pageurl,pagetitle)
{
   if(document.getElementById("hdnChartCFN").value!="")
   {
     pageurl=pageurl+"?CCF="+document.getElementById("hdnChartCFN").value;
     OpenNewWindow(pageurl,pagetitle);
   }
   else
   {
      //return false; 
   }
}

//...................................................................................................//

function OpenNewWindow(url,name) {
	newwindow=window.open(url,name);
	if (window.focus) {newwindow.focus()}
	//return false;
}

//...................................................................................................//

