//Contains all the utility functions commonly used across the project.

//Loads an XML string into a DOM object
function loadXMLString(txt)
{
    var xmlDoc;
    try //Internet Explorer
      {
          xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
          xmlDoc.async = "false";
          xmlDoc.loadXML(txt);
          return(xmlDoc); 
      }
    catch(e){
        try //Firefox, Mozilla, Opera, etc.
        {
            parser = new DOMParser();
            xmlDoc = parser.parseFromString(txt,"text/xml");
            return(xmlDoc);
        }
         catch(e){
           alert(e.message)
         }
      }
    return(null);
}

//...................................................................................................//

//Gets the value of the Query String Parameter.
function queryString(parameter) { 
  var loc = location.search.substring(1, location.search.length);
  var param_value = false;
  var params = loc.split("&");
  for (i=0; i<params.length;i++) {
      param_name = params[i].substring(0,params[i].indexOf('='));
      if (param_name == parameter) {
          param_value = params[i].substring(params[i].indexOf('=')+1)
      }
  }
  if (param_value) {
      return param_value;
  }
  else {
      return false; //Here determine return if no parameter is found
  }
}

//...................................................................................................//

//Toggles the passed object's image to its corresponding hover image and vice versa.
function toggleImage(obj){
    if(obj.src.indexOf('-over') == -1){
        obj.src = obj.src.replace("-but.","-but-over.");
    }
    else{
          obj.src = obj.src.replace("-but-over.","-but.");
    }
}

//...................................................................................................//

//Toggles the passed object's background to its corresponding hover image and vice versa.
function toggleBckgrnd(obj){
    if(obj.style.backgroundImage.indexOf('-over') == -1){
        obj.style.backgroundImage = obj.style.backgroundImage.replace("-but.","-but-over.");
    }
    else{
        obj.style.backgroundImage = obj.style.backgroundImage.replace("-but-over.","-but.");
    }
}

//...................................................................................................//

//Redirects to the page using POST method.
function Redirect(redirectPage)
{
    var theForm=document.forms["aspnetForm"];
    theForm.action = redirectPage ,"","target=_child","toolbar=no","menubar=no","statusbar=no","scrollbars=no","width=350,height=300","left=0","top=0";
    if (theForm.__VIEWSTATE)
        theForm.__VIEWSTATE.name = 'NOVIEWSTATE';           
    if (theForm.__EVENTTARGET)
        theForm.__EVENTTARGET.name = ''; 
    if (theForm.__EVENTARGUMENT)
        theForm.__EVENTARGUMENT.name = ''; 
    theForm.method = "post";
    theForm.submit();   
}

//...................................................................................................//

//Sets the opacity for a given control.
function setOpacity(value, controlID){
     document.getElementById(controlID).style.opacity = value / 10;
     document.getElementById(controlID).style.filter = 'alpha(opacity=' + value * 10 + ')';
}

//...................................................................................................//

//Removes leading whitespaces
function LTrim(value) {
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");
}

//...................................................................................................//

//Removes ending whitespaces
function RTrim(value) {
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");
}

//...................................................................................................//

//Removes leading and ending whitespaces
function trim(value) {
    try {
        var trimmmed=LTrim(RTrim(value));
        return trimmmed;
    }
    catch(er)  {
        return value;
    }
}

//...................................................................................................//

//validating numerics. which only permits the entry of characters 0 to 9, a decimal point and a negative sign
function IsNumeric(strString)
{
    var strValidChars = "0123456789.-,";
    var strChar;
    var blnResult = true;
    if (strString.length == 0) return false;
    //  test strString consists of valid characters listed above
    for (i = 0; i < strString.length && blnResult == true; i++)
    {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1)
        {
            blnResult = false;
        }
    }
    return blnResult;
}

//...................................................................................................//

function ConvertToString(xmlNode){
    var xmlString;
    if(document.all){//IE
        xmlString =xmlNode.xml;
    }
    else{//Others
        xmlString = (new XMLSerializer()).serializeToString(xmlNode);
    }
    return xmlString;
}

//...................................................................................................//

//Checks if the passed URL is a valid one containing the folder separator and a aspx page.
function IsURLValid(url){
    if(url && url.length>0){
        if(url.indexOf("/") != -1 && url.indexOf(".aspx") != -1)//If not a valid URL
        {return true; }
    }
    return false;
}

//...................................................................................................//

function ChangeURL(sURL){
    document.location=sURL;
}

//...................................................................................................//

function escapeHTML(str)
{
   var div = document.createElement('div');
   var text = document.createTextNode(str);
   div.appendChild(text);
   return div.innerHTML;
}

//...................................................................................................//

function ClearIFrame(frame)
{
    if(frame.document) {
        window[frame.name].document.body.innerHTML="";
        //frame.document.documentElement.innerHTML = "";
    }
    else if(frame.contentDocument) {
        frame.contentDocument.documentElement.innerHTML = "";
    }
}

//...................................................................................................//

function unescapeHTML(html) {
    var htmlNode = document.createElement("DIV");
    htmlNode.innerHTML = html;
    if(htmlNode.innerText)
    return htmlNode.innerText; // IE
    return htmlNode.textContent; // FF
}

//...................................................................................................//
//To get scrolling height numeric value
function f_scrollTop() {
	return f_filterResults (
		window.pageYOffset ? window.pageYOffset : 0,
		document.documentElement ? document.documentElement.scrollTop : 0,
		document.body ? document.body.scrollTop : 0	);
}

//...................................................................................................//
	
function f_filterResults(n_win, n_docel, n_body) {
	var n_result = n_win ? n_win : 0;
	if (n_docel && (!n_result || (n_result > n_docel)))
		n_result = n_docel;
	return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}

//...................................................................................................//
//To add two float values
function f_floatAdd(a, b){
        return (a*10000 + b*10000)/10000.0;
}

//Removes duplicate values in a given array.
function RemoveDuplicates(a)
{
   var r = new Array();
   o:for(var i = 0, n = a.length; i < n; i++)
   {
      for(var x = 0, y = r.length; x < y; x++)
      {
         if(r[x]==a[i]) continue o;
      }
      r[r.length] = a[i];
   }
   return r;
}

//Gets the scroll amounts of the horizontal and the vertical scrolls.
function GetScrollXY() {
    var scrOfX = 0, scrOfY = 0;
    if( typeof( window.pageYOffset ) == 'number' ) {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
    } else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
    } else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
    }
    return [ scrOfX, scrOfY ];
}

function GetPage(win)
{
    return win.location.toString().toUpperCase();
}