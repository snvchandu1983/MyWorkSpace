//Generates the full blown view of the Menu Panel and updates the shortcut selection in the Left Panel.
var g_ErrorText = "";
var g_showShortCuts = false;
var g_DataPath;
var checkedValues;
var explodedVWCollapser;
var tblMainContentsClientID;

//...................................................................................................//

function UpdateShrtCuts()
{
    try
    {
        var pnlExplodedView = document.getElementById(tblMainContentsClientID);
        var checkBoxList = document.getElementsByTagName('input');
        var cntr = 0;
        checkedValues = new Array();
        for(i = 0; i < checkBoxList.length; i++ )
        {
            //Checking whether the checkboxes belong to the ExplodedView Panel or not.
            if(checkBoxList[i].id.indexOf(tblMainContentsClientID) != -1)
            {
                var chkBxUniqueID = checkBoxList[i].id.split('*')[1];
                var innerText = checkBoxList[i].parentNode.nextSibling.innerHTML;
                var hRef = checkBoxList[i].parentNode.nextSibling.onclick;
                if(document.all)
                {
                    //Do nothing
                }
                else
                {   
                  hRef = hRef + "";
                  hRef = hRef.split('{')[1].replace("}","").replace(";","");
                }
                
                if (checkBoxList[i].checked == true)
                {
                      checkedValues[cntr] = chkBxUniqueID + ',True'; 
                }
                else
                {
                     checkedValues[cntr] = chkBxUniqueID + ',False';
                }  
                cntr++; 
            }
        }
        
       //Calling the C# method to update the changes in the server.
       CommonUI.UpdateShortCuts(checkedValues, callback_UpdateShrtCuts);
   }
   catch(error)
   {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : UpdateShrtCuts()\nFileName : ExplodedView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
   }
}

//...................................................................................................//

function ShowHideShrtCuts()
{
    try
    {
        g_showShortCuts = !g_showShortCuts;//toggle
        RenderThePanel(g_DataPath);
        var imgShowHideShrtCuts = document.getElementById('imgBtnShowHideShrtCuts');
        var imgBtnUpdateShrtCuts = document.getElementById('imgBtnUpdateShrtCuts');
        var currentTheme = document.getElementById('ctl00_hdnThemeName').value;
        
       // var cdnImagePath='<%=Application["ImagesCDNPath"]%>';
        if(g_showShortCuts)
        {
            imgShowHideShrtCuts.src = g_cdnImagesPath + "Images/hide-shortcuts-but.png";
            imgBtnUpdateShrtCuts.style.visibility = "visible";
//            imgBtnUpdateShrtCuts.style.display = "";
        }
        else
        {
            imgShowHideShrtCuts.src = g_cdnImagesPath + "Images/show-shortcuts-but.png";
            imgBtnUpdateShrtCuts.style.visibility = "hidden";
//            imgBtnUpdateShrtCuts.style.display = "none";
        }
    }
    catch(error)
    {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : ShowHideShrtCuts()\nFileName : ExplodedView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
    }
}

//...................................................................................................//

function RenderThePanel(dataPath)
{
    ShowProgress(true);
    AjaxMethods.GetMenuPanelXML(dataPath,callback_GetMenuPanelXML);
    g_DataPath=dataPath;
}

//...................................................................................................//

//CallBack handler for UpdateShrtCuts method.
function callback_GetMenuPanelXML(response)
{
    try  {
        if (response.error != null)  {
          alert(response.error);
          HideProgress();
          return;
        }
        document.getElementById('ctl00_hdnMenuPanel').value=response.value.substring(1);
        var reset=response.value.substring(0,1);
        if(reset=="1") {
            var dataPathSplit=g_DataPath.split('/*');
            dataPathSplit[3]="[position()=1]";
            g_DataPath=dataPathSplit.join("/*");
        }
        RenderPanel(g_DataPath);
        var currentPage = window.location + "";//Cast the object to string.
        if(currentPage.toUpperCase().indexOf('DASHBOARD.ASPX')==-1)
        {
            DisplayPopUp(true,'ctl00_pnlExplodedView');
        }
        HideProgress();
    }
    catch(error) {
        if(g_IsDebugging) {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : callback_GetMenuPanelXML()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
        HideProgress();
    }
}

//...................................................................................................//

var thisPanelLabel;
function RenderPanel(dataPath)
{
    g_DataPath=dataPath;
    var dataPathSplit=dataPath.split('/*');
    var strSplit=dataPathSplit[3];
    var indexOfNumber=strSplit.indexOf('=');
    var strPosition=strSplit.substring(indexOfNumber+1,strSplit.length-1);
    var xmlMenuPanel=document.getElementById('ctl00_hdnMenuPanel').value;
    var xmlDoc=loadXMLString(xmlMenuPanel);//Load the XML string into a DOM object
    
    var arrPanelNodes=xmlDoc.getElementsByTagName("Panel");
//    if(parseInt(strPosition)>arrPanelNodes.length)
//    {
//        dataPathSplit[3]="[position()=1]";
//        RenderThePanel(dataPathSplit.join("/*"));
//        return;
//    }
//    var nodePanel=arrPanelNodes[strPosition - 1];
    var nodePanel=arrPanelNodes[0];
    var tblMainContents = document.getElementById('ctl00_cphPageContents_tblMainContents');
    if(tblMainContents==null)
    {
        tblMainContents=document.getElementById('ctl00_tblMainContents');
    }
    tblMainContentsClientID=tblMainContents.id;
    
    //Clear the table contents before adding new contents.
    while(tblMainContents.hasChildNodes())
    {
       tblMainContents.removeChild(tblMainContents.firstChild);
    }
    var tblBodyMainContents=document.createElement("tbody");
    var rowMainContents=document.createElement("tr");
    tblBodyMainContents.appendChild(rowMainContents);
    tblMainContents.appendChild(tblBodyMainContents);
    tblMainContents.border="0";
    tblMainContents.width="100%";
    
    //Variable to count the number of table rows.
    var rowCntr=0;
    var arrRowHeights=new Array();
    var thisPanelID=nodePanel.getAttribute("ID");
    thisPanelLabel=nodePanel.getAttribute("Label");
    
    for (var i=0;i<nodePanel.childNodes.length;i++)
    {   
        var nodeItem=nodePanel.childNodes[i];
        var thisItemID=nodeItem.getAttribute("ID");
        var tableHeight=0;
        //Header part.
        var tblItem=document.createElement("table");//An item table for each of the items in a panel.
        tblItem.border="0px";
        tblItem.cellPadding="0px";
        tblItem.cellSpacing="1px";
        
        var tblBody=document.createElement("tbody");//It's body.
        var rowItem=document.createElement("tr");
        var cellItem=document.createElement("th");
        cellItem.className="ExpVwHeader";
        cellItem.colSpan="2";
        cellItem.align="center";
        tableHeight+=28;//The table headers height...
        
        //var cellText = document.createTextNode(nodeItem.attributes["Label"].value);
        var cellText=document.createTextNode(nodeItem.getAttribute("Label"));
        cellItem.appendChild(cellText);
        rowItem.appendChild(cellItem);
        tblBody.appendChild(rowItem);
        tblItem.appendChild(tblBody);
        
        for(var j=0;j<nodeItem.childNodes.length;j++)
        {
            var nodeProcess = nodeItem.childNodes[j];
            var BPGID = nodeProcess.getAttribute("BPGID");
            var chkBxUniqueID = thisPanelID + "_" + thisItemID + "_" + BPGID;
            var hRef = "javascript:OnMenuItemClick('" + nodeProcess.getAttribute("PageInfo") + "','" + BPGID + "')";
            rowItem = document.createElement("tr");
            var cellProc = document.createElement("td");
            var cellCheckBox = document.createElement("td");
            var isChecked = false;
            if(nodeProcess.getAttribute("IsShortcut")=="True")  {
                isChecked=true;
            }
            else {
                isChecked=false;
            }
            
            if(g_showShortCuts==true)
            {
                var ckkBxSpanTag = document.createElement("span");
                //Frame the checkbox id..
                //Changed the passing of unique id within the checkbox id itself.
                //var chBxId = "ctl00_cphPageContents_tblMainContents*" + chkBxUniqueID;
                var chBxId = tblMainContentsClientID+"*" + chkBxUniqueID;
                // Create an input checkbox the wacky IE-way
                if (document.all) {
                    var strInput;
                    if(isChecked) {
                        strInput = "<input type='checkbox' id='" + chBxId + "' checked='checked' />";
                    }
                    else {
                        strInput = "<input type='checkbox' id='" + chBxId + "'/>";
                    }
                    var chkBxObject=document.createElement(strInput);
                    ckkBxSpanTag.appendChild(chkBxObject);
                    cellCheckBox.appendChild(chkBxObject);
                }
                else // Other browsers...
                { 
                    var chkBxObject = document.createElement('input');
                    chkBxObject.type = 'checkbox';
                    chkBxObject.id = chBxId;
                    chkBxObject.checked = isChecked;
                    ckkBxSpanTag.appendChild(chkBxObject);
                    cellCheckBox.appendChild(chkBxObject);
                }
                $get('imgBtnUpdateShrtCuts').style.visibility = "";
            }
            cellCheckBox.style.width = "20px";
            cellCheckBox.style.height = "19px";
            cellCheckBox.style.border = "0px";
            cellCheckBox.style.padding = "0px";
            cellCheckBox.style.margin = "0px";
            
            cellProc.className = "ExplodedViewText";
            cellProc.onmouseout = function() { OnTDMouseOut(this); }
            cellProc.onmouseover = function() { OnTDMouseEnter(this); }
            cellProc.onclick = function(){ OnMenuItemClick(this) }
            cellProc.align = "center";
            cellProc.style.height = "20px";
            cellProc.style.width = "148px";
            cellProc.style.cursor = "pointer";
            cellProc.className = "ExplodedViewText";
            //Frame a unique id to the cell element
            cellProc.id = "td_" + thisPanelID + "_" + thisItemID + "_" + BPGID;
            cellProc.setAttribute("BPGID", BPGID);
            cellProc.setAttribute("NavigatePage", nodeProcess.getAttribute("PageInfo"));
            var cellInnerText = nodeProcess.getAttribute("Label");//Contains the inner text of the cell.
            cellProc.title = cellInnerText;
            
            //Make sure that the text fits into the cell without over-flowing else clip it and appends dots.
            if(cellInnerText.length > 22)
            {
                cellInnerText = cellInnerText.substring(0, 20) + "..";
            }
            var cellProcText = document.createTextNode(cellInnerText);
            cellProc.appendChild(cellProcText);
            rowItem.appendChild(cellCheckBox);
            rowItem.appendChild(cellProc);
            tblBody.appendChild(rowItem);
            tableHeight += 19;//Summing the cell's height in each loop.
        }
        
        var currentCell = document.createElement("td");
        currentCell.align = "center";
        currentCell.vAlign = "top";
        currentCell.appendChild(tblItem);
        
        //As long as the current row contains not more than 3 cells use the same row, else create a new one.
        if(tblMainContents.rows[rowCntr].cells.length >= 3)
        {
            rowMainContents = document.createElement("tr");
            rowMainContents.appendChild(currentCell);
            tblBodyMainContents.appendChild(rowMainContents);
            rowCntr++;
        }
        else
        {
            rowMainContents.appendChild(currentCell);
        }
    }
    
    //Setting the title and its width.(Ajax Method)
    //CommonUI.GetStringWidth(thisPanelLabel, callback_GetStrWidth);
    var visualLength=getVisualLength(thisPanelLabel);
    SetExpHdrTitleWidth(visualLength);
    //alert(visualLength);
    
    var divExpViewContent = document.getElementById('ctl00_cphPageContents_pnlExpViewContent');
    var divExpViewTitle = document.getElementById('ctl00_cphPageContents_pnlExpView');
    if(divExpViewContent==null||divExpViewTitle==null)
    {
        divExpViewContent = document.getElementById('ctl00_pnlExpViewContent');
        divExpViewTitle = document.getElementById('ctl00_pnlExpViewTitle');
    }
    var tblButtonsContainer = document.getElementById('tblButtons');
    divExpViewContent.style.height = "auto";
    divExpViewTitle.style.height = "24px";
    tblButtonsContainer.style.marginTop = "15px";
    tblButtonsContainer.cellSpacing = "6";
    //Finally make the div element visible.
    divExpViewContent.style.visibility = "visible";
    if(divExpViewContent.firstChild.style)
        divExpViewContent.firstChild.style.visibility = "visible";
    divExpViewTitle.style.visibility = "visible";
}

//...................................................................................................//

//Cycles through the Menu Panel Tabs.
function IteratePanel(direction)
{
    //CheckDataPath();//Return the Max Number of Panels nodes if a invalid datapath is passed.
    //Check if popup already is being diplayed
    //Retain the focus to the MPE on successful iteration
    if(!jQuery('#ctl00_pnlExplodedView').dialog('isOpen'))
    {
        return;
    }
    
    DisplayPopUp(false,'ctl00_pnlExplodedView');
    var datePathSplit=g_DataPath.split("/*");
    var searchStrtStr="position()=";
    var indexStart=datePathSplit[3].indexOf(searchStrtStr);
    indexStart=indexStart+searchStrtStr.length;
    var indexEnd=datePathSplit[3].indexOf("]");
    var finalIndex=datePathSplit[3].substring(indexStart,indexEnd);
    if(direction=='forward')
    {
        finalIndex=parseInt(finalIndex)+1;
    }
    else
    {
        if(finalIndex>1)
        {
            finalIndex=parseInt(finalIndex)-1;
        }
    }
    datePathSplit[3]='['+searchStrtStr+finalIndex+']';
    var newDataPath=datePathSplit.join("/*");
    //alert(g_DataPath+"\n"+newDataPath);
    RenderThePanel(newDataPath);
    DisplayPopUp(true,'ctl00_pnlExplodedView');
//    this.window.focus();
//    var focusElement=document.getElementById(objMPE.get_PopupControlID());
//    focusElement.focus();
    return false;
}

//...................................................................................................//

//CallBack handler for RenderMenuPanel method.
function callback_GetStrWidth(response)
{
    try
    {
        if (response.error != null)
        {
          alert(response.error);
          return;
        }
        SetExpHdrTitleWidth(response.value)
    }
    catch(error)
    {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : ShowHideShrtCuts()\nFileName : ExplodedView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
    }
}

//...................................................................................................//

function OnMenuItemClick(cellObj)
{
    try
    {
        var processBPGID = cellObj.getAttribute("BPGID");
        var redirectPage = cellObj.getAttribute("NavigatePage");
        MainMenuItemClick(processBPGID,redirectPage);
    }
    catch(error)
    {
        if(g_IsDebugging)
        {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : OnMenuItemClick()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}


//...................................................................................................//
function SetExpHdrTitleWidth(value)
{
    var totalHeaderWidth;
    var imgPath=document.getElementById('pnlArrow').getAttribute('src'); 
    var  imgLocation=imgPath.split('/');
    var imgFullName=imgLocation[4].toUpperCase();              
    var imgFullSplitName=imgFullName.split('.');
    var imgName=imgFullSplitName[0]; 
    if(imgName=='CLOSE-ARROW')
    {
        totalHeaderWidth=643;
    }
    else
    {
        totalHeaderWidth=545;
    }  
    //    var cpeLeftPanel=$find('BIDLeftPanelCPE')
    //    if(cpeLeftPanel._checkCollapseHide()) {
    //        //Left Panel is in collapsed state
    //       
    //    }
    //    else  {
    //Left Panel is in expanded state
    //    }
    var stringWidth=value;
    var bgWidth=totalHeaderWidth-stringWidth-28;
    var nodeTitle=document.createTextNode(thisPanelLabel);
    var tdExpViewText=document.getElementById('tdExpViewText');
//    var tdExpViewBG=document.getElementById('tdExpViewBG');
    tdExpViewText.innerHTML="";
    tdExpViewText.appendChild(nodeTitle);
    tdExpViewText.width=stringWidth+"px";
//    tdExpViewBG.width=bgWidth+"px";
}

//...................................................................................................//

function OnTDMouseEnter(obj)
{
    obj.className = "ExplodedViewTextOver";
}

//...................................................................................................//

function OnTDMouseOut(obj)
{
    obj.className = "ExplodedViewText";
}

//...................................................................................................//

function ExpViewTitleClick()
{  
    try
    { 
        explodedVWCollapser.slideit();
        var imgExpView = document.getElementById('imgExpViewExpand');
        if(explodedVWCollapser.divObj.style.height != "0px")
        {
            imgExpView.src = imgExpView.src.replace("minus_sym","add_symbol");
        }
        else
        {
            imgExpView.src = imgExpView.src.replace("add_symbol","minus_sym");
        }
    }
    catch(error)
    {
        if(g_IsDebugging)
        {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : ExpViewTitleClick()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}

//...................................................................................................//

function UpdateLeftPnlShortcuts()
{
    try
    {
        var tableShortCuts = document.getElementById('ctl00_tableShortcuts');
        for(i = 0; i < checkedValues.length; i++ )
        {
            var BPGID = checkedValues[i].split(',')[0].split('_')[2];
            var isChecked = checkedValues[i].split(',')[1];
            var found = false;
            var currrentBPGID;
            //Loop through the Left Shortcuts panel and see whether the current update selection is present or not.
            for(var cntr = 1; cntr < tableShortCuts.rows.length; cntr++)
            {
                //Retreive the BPGID embedded within the innerHTML
                var cell = tableShortCuts.rows[cntr].cells[0].firstChild.firstChild;//.attributes["href"].value;
                cell = cell + "";
                cell = cell.substring(cell.indexOf('('),cell.indexOf(')'));
                cell = cell.split(',')[0].replace(/'/g,"");
                cell = cell.replace('(','');
                currrentBPGID = cell.replace(/^\s*/, "").replace(/\s*$/, "");//Trim
                if(currrentBPGID == BPGID)
                {
                    found = true;
                    break;
                }
            }
            //If the selection is present
            if(found)
            {
                // and is unchecked then remove that entry from the Left Shortcuts panel
                if(isChecked == 'False')
                {
                    //Remove the node.
                    tableShortCuts.deleteRow(cntr);
                }
            }
            else
            {
                // If the selection is not found and is checked then add a new row to the Left Shortcuts panel
                if(isChecked == 'True')
                {
               //debugger; 
                    //Add the node.
                    //Get the required parameters from the cell's attribute collection.
                    var cellProc = document.getElementById("td_" + checkedValues[i].split(',')[0]);
                    var BPGIDinArr = cellProc.getAttribute("BPGID");
                    var innerText = cellProc.getAttribute("title")
                    var redirectPage = cellProc.getAttribute("NavigatePage");
                    var row = document.createElement("tr");
                    var cell = document.createElement("td");
                    cell.className = "shortcutsbg";
                    cell.paddingLeft = "2px";
                    cell.style.height ="20px";
                    cell.title = innerText;
//                    cell.setAttribute("QTIP","chandu");
                    
                    //Make sure that the text fits into the cell without over-flowing else clip it and appends dots.
                    if(innerText.length > 22)
                    {
                        innerText = innerText.substring(0, 20) + "..";
                    }
                    var hRef = "MainMenuItemClick('" + BPGID + "','"+ redirectPage + "')";// The OnShrtCutLinkClick method is functionally same as that of OnMenuItemClick.
                    cell.innerHTML ='<span><a class="shourtcutslinks" href="javascript:'+ hRef +'">'+ innerText+'</a></span>';
                    row.appendChild(cell);
                    //Add the new row object to the table.
                    if (document.all) //Interner Explorer
                    {
                        tableShortCuts.firstChild.appendChild(row);
                    }
                    else//Other Browsers
                    {
                        tableShortCuts.childNodes[1].appendChild(row);
                    }
                    HighLightElement(cell);
                }
            }
            found = false;
        }
    }
    catch(error)
    {
        if(g_IsDebugging)
        {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : ShowHideShrtCuts()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}

//...................................................................................................//

function HighLightElement(element)
{
    //alert(element.style.backgroundColor);
}

//...................................................................................................//

//CallBack handler for UpdateShrtCuts method.
function callback_UpdateShrtCuts(response)
{
    try
    {
        if (response.error != null)
        {
          alert(response.error);
          return;
        }
        //Return message is in the format - Status~::~MessageLabel~::~UpdatedXml
        var updateMessage = response.value.split('~::~');
        if(updateMessage[0] == "Success")
        {
            document.aspnetForm.ctl00_hdnMenuPanel.value = updateMessage[2];
            UpdateLeftPnlShortcuts();
            alert(updateMessage[1]);
        }
    }
    catch(error)
    {
        if(g_IsDebugging)
        {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : callback_UpdateShrtCuts()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
        alert('Shortcuts updation failed.');
    }
}

//...................................................................................................//

//Loads an XML string into a DOM object
function loadXMLString(txt) 
{
    try //Internet Explorer
      {
          xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
          xmlDoc.async="false";
          xmlDoc.loadXML(txt);
          return(xmlDoc); 
      }
    catch(e)
      {
        try //Firefox, Mozilla, Opera, etc.
        {
            parser=new DOMParser();
            xmlDoc=parser.parseFromString(txt,"text/xml");
            return(xmlDoc);
        }
         catch(e) { alert(e.message)
         }
      }
    return(null);
}

//...................................................................................................//

function getVisualLength(str)
{
    var ruler = document.getElementById("ruler");
    ruler.className = "grdVwtitle";
    ruler.innerHTML = str;
    return ruler.offsetWidth + 20;//20 for padding
}
//...................................................................................................//

