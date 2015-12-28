var g_arrCPClipBoard;//To store the copied contents.
function CopyGridRows(gvClientID, sender)
{
    if(sender.getAttribute("disabled") == 'disabled')
    {
        return;
    }
    var strExpression = '';
    var objGrid = document.getElementById(gvClientID);
    var cpObjCntr = 0;
    g_arrCPClipBoard = new Array();
    //Loop and see which rows are selected.
    for (var rowCount = 1; rowCount < objGrid.rows.length ; rowCount++ )
    {
        var str = '';
        var delimiter = '~::~';
        var trCurrent = objGrid.rows[rowCount];
        //Check if previously this row was deleted.If deleted don't consider.
        if(trCurrent.style.display == "none")
        {
            continue;
        }
        var checkBoxContainer = GetFirstChild(trCurrent.cells[0]);
        if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == false)
        {
            continue;
        }
        //If this point has been reached implies rows are selected.
         jQuery(trCurrent)
         .css({backgroundColor:'orange'})
         .animate({backgroundColor:'#D3DFFD'},{duration:2000,complete:function(){jQuery(this).css({backgroundColor:''}) } });
         
        g_arrCPClipBoard[cpObjCntr++] = trCurrent.cloneNode(true);//Copy the node
        checkBoxContainer.checked = false;
    }
}

//...................................................................................................//

function PasteGridRows(gvClientID, sender)
{
    if(sender.getAttribute("disabled")=='disabled')
    {
        return;
    }
    try
    {
        var isIE;
        var nv=navigator.appVersion;
        var IEVer=parseFloat(nv.substring(nv.indexOf("MSIE") + 5));
        if(document.all) {
            if(IEVer && IEVer==8)  {
                isIE=false;
            }
            else  {
                isIE=true;
            }
        }
        var objGrid = document.getElementById(gvClientID);
        var rowsToPasteCount = g_arrCPClipBoard.length;
        var pasteCntr = 0;
        var selectedRowsCntr = 0;
        var lastSelectedRowIndex = 0;
        var atLeastOne=false;//Check for atleast one row to be selected.
        //Get the selected row and paste the info over there.
        //Loop and see which rows are selected.
        for (var rowCount = 1; rowCount < objGrid.rows.length ; rowCount++)
        {
            var trCurrent = objGrid.rows[rowCount];
            //Check if previously this row was deleted.If deleted don't consider.
            if(trCurrent.style.display == "none")
            {
                continue;
            }
            var checkBoxContainer = GetFirstChild(trCurrent.cells[0]);
            if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == true)
            {
                atLeastOne=true;
                lastSelectedRowIndex = rowCount;
                var objRowToPaste = g_arrCPClipBoard[pasteCntr];
                if(isIE)
                {
                    var arrTDs = objRowToPaste.getElementsByTagName('TD');
                    for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
                    {
                       var cellElement = getChildElement(trCurrent.cells[cellCount]);
                       CopyContents(cellElement , getChildElement(arrTDs[cellCount]));
                    }
                }
                else
                {
                    for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
                    {
                        var cellElement = getChildElement(trCurrent.cells[cellCount]);
                        CopyContents(cellElement , getChildElement(objRowToPaste.cells[cellCount]))
                    }
                    
                }
                
                var CGVCClientID=gvClientID.replace('_grdVwBranch','');
                if(IsPaging(CGVCClientID))
                {
                    //If Paging is ON update the hdnModRows variable upon pasting of a row.
                    //Get the pasted row's index
                    var rowIndex=checkBoxContainer.parentNode.getAttribute("RowIndex");
                    var hdnModId=CGVCClientID+'_hdnModRows';
                    SetModRowIndices(hdnModId,rowIndex);
                }
                
                selectedRowsCntr++;
                if(selectedRowsCntr >= rowsToPasteCount)
                {
                    break;
                }
                pasteCntr++;
            }
        }
        //Check if all the rows are pasted.
        var isSelected=true;//Check for the current row to be selected.
        if(selectedRowsCntr<rowsToPasteCount)
        {
            for (var cntr = pasteCntr; cntr < rowsToPasteCount ; cntr++)
            {
                var objRowToPaste=g_arrCPClipBoard[cntr];
                var trCurrent = objGrid.rows[++lastSelectedRowIndex];            
                var checkBoxContainer = GetFirstChild(trCurrent.cells[0]);
                if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked==true) {
                    isSelected=true;
                }
                else {
                    isSelected=false;
                }
              
                if(isSelected||atLeastOne) {
                    if(isIE)
                    {
                        var arrTDs = objRowToPaste.getElementsByTagName('TD');
                        for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
                        {
                            var cellElement = getChildElement(trCurrent.cells[cellCount]);
                            CopyContents(cellElement , getChildElement(arrTDs[cellCount]));
                        }
                    }
                    else
                    {
                        for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
                        {
                            var cellElement = getChildElement(trCurrent.cells[cellCount]);
                            CopyContents(cellElement , getChildElement(objRowToPaste.cells[cellCount]));
                        }
                    }
                    var CGVCClientID=gvClientID.replace('_grdVwBranch','');
                    if(IsPaging(CGVCClientID))
                    {
                        //If Paging is ON update the hdnModRows variable upon pasting of a row.
                        //Get the pasted row's index
                        var rowIndex=checkBoxContainer.parentNode.getAttribute("RowIndex");
                        var hdnModId=CGVCClientID+'_hdnModRows';
                        SetModRowIndices(hdnModId,rowIndex);
                    }
                }
            }
        }
        
        //Update the Instantaneous sum at the footer when a grid row is copied and pasted.
        if(objGrid.rows.length > 1)
        {
            var trCurrent= objGrid.rows[1];
            for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
            {
                var cellElement = getChildElement(trCurrent.cells[cellCount]);
                ShowInstantaneousSumOnPaste(cellElement,gvClientID)
            }
        }
    }
    catch(error)
    {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : PasteGridRows(gvClientID, sender)\nFileName : ChildGridView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
    }
}

//...................................................................................................//

//A refactored block of code from PasteGridRows to calculate the column sum upon pasting rows.
function ShowInstantaneousSumOnPaste(cellElement,gvClientID)
{
    var isAmountIndex = cellElement.getAttribute('isAmount');
    if(cellElement.type && cellElement.type=='text' && isAmountIndex)
    {
        ShowInsantaneousSum(gvClientID,isAmountIndex);
    }
}

//...................................................................................................//

function CopyContents(objTo , objFrom)
{
    if(objFrom.disabled)
    {
        return;
    }
    try
    {
        if(objTo.nodeName == "SELECT")//DropDownList
        {   
            if(objTo.options.length > 0)
            {
                var selectedIndex = document.getElementById(objFrom.id).selectedIndex;
                objTo.options[selectedIndex].selected = true;
            }
        }
        else if(objTo.nodeName == "SPAN")//Checkbox with an attibute
        {
            try
            {
                var childObjFrom = getChildElement(objFrom);
            }
            catch(e)
            {
               return;
            }
            if(childObjFrom && childObjFrom.nodeName != '#text')
            {
                getChildElement(objTo).checked = childObjFrom.checked;
            }
        }
        else//Others
        {   
            objTo.value = objFrom.value;
        }
    }
    catch(error)
    {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : CopyContents(objTo , objFrom)\nFileName : ChildGridView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
    }
}

//...................................................................................................//

//Validates the child grid view controls for data to be entered
var g_IsValidating = false;
var g_TimerID;
var g_BlinkState = true;//Blink On or Off
var controlsToBlink = new Array();
function ValidateChildControl(objControl)
{
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    //alert(hdnCurrAction.value);
     if(trim(hdnCurrAction.value.toUpperCase())!="FIND")
     {
            if(!g_IsValidating && (objControl.value == "" || objControl.value.indexOf('-1') == 1))
            {
                objControl.title = "This field is mandatory!!";
                controlsToBlink[controlsToBlink.length] = objControl.id;
                blinkElements();
            }
            else
            {   
                if(!(objControl.value == "" || objControl.value.indexOf('-1') == 1))
                {
                        for( var cntr = 0; cntr < controlsToBlink.length; cntr++)
                        {
                            if(controlsToBlink[cntr] == objControl.id)
                            {
                                controlsToBlink[cntr] = "";
                                break;
                            }
                        }
                        objControl.style.borderColor = '';
                        blinkElements();
                }
            }
     }

}

//...................................................................................................//

function blinkElements() 
{
    if (g_TimerID)
    {
        window.clearTimeout(g_TimerID);
        g_TimerID = null;
    }
    for( var cntr = 0; cntr < controlsToBlink.length; cntr++)
    {
        if(controlsToBlink[cntr] == "")
        {
            continue;
        }
        var obj = document.getElementById(controlsToBlink[cntr]);
        if(g_BlinkState)
        {
            obj.style.borderColor = 'orange';
        }
        else 
        {
             obj.style.borderColor = '';
        }
    }
    g_BlinkState = !g_BlinkState;//For synchronous blinking effect.
    g_TimerID = setTimeout("blinkElements()", 500);//Blink time interval.
}

//...................................................................................................//

// To reset the bliking effect upon selected row change and cancel events.
function ResetBlinkObjects(postBackCtrlID)
{
    if (g_TimerID) {
        window.clearTimeout(g_TimerID);
        g_TimerID = null;
    }
    for( var cntr = 0; cntr < controlsToBlink.length; cntr++)  {
        if(controlsToBlink[cntr] == "")  {
            continue;
        }
        var obj = document.getElementById(controlsToBlink[cntr]);
        if(obj) {
             obj.style.borderColor = '';
        }
    }
    //Stop blinking-Clear the array.
    controlsToBlink = new Array();
}

//...................................................................................................//

function CheckDate(input)
{   
//    if(input.value != "__/__/__")
//    {
//        if(!IsDate(input))
//        {
//            input.value="";
//        }
//    }
    return;        
}

//...................................................................................................//

function IsDate(field){
var checkstr = "0123456789";
var DateField = field;
var Datevalue = "";
var DateTemp = "";
var seperator = "/";
var day;
var month;
var year;
var leap = 0;
var err = 0;
var i;
   err = 0;
   DateValue = DateField.value;
   /* Delete all chars except 0..9 */
   for (i = 0; i < DateValue.length; i++) {
	  if (checkstr.indexOf(DateValue.substr(i,1)) >= 0) {
	     DateTemp = DateTemp + DateValue.substr(i,1);
	  }
   }
   DateValue = DateTemp;
   /* Always change date to 8 digits - string*/
   /* if year is entered as 2-digit / always assume 20xx */
   if (DateValue.length == 6) {
      DateValue = DateValue.substr(0,4) + '20' + DateValue.substr(4,2); }
   if (DateValue.length != 8) {
      err = 19;}
   /* year is wrong if year = 0000 */
   year = DateValue.substr(4,4);
   if (year == 0) {
      err = 20;
   }
   /* Validation of month*/
   month = DateValue.substr(2,2);
   if ((month < 1) || (month > 12)) {
      err = 21;
   }
   /* Validation of day*/
   day = DateValue.substr(0,2);
   if (day < 1) {
     err = 22;
   }
   /* Validation leap-year / february / day */
   if ((year % 4 == 0) || (year % 100 == 0) || (year % 400 == 0)) {
      leap = 1;
   }
   if ((month == 2) && (leap == 1) && (day > 29)) {
      err = 23;
   }
   if ((month == 2) && (leap != 1) && (day > 28)) {
      err = 24;
   }
   /* Validation of other months */
   if ((day > 31) && ((month == "01") || (month == "03") || (month == "05") || (month == "07") || (month == "08") || (month == "10") || (month == "12"))) {
      err = 25;
   }
   if ((day > 30) && ((month == "04") || (month == "06") || (month == "09") || (month == "11"))) {
      err = 26;
   }
   /* if 00 ist entered, no error, deleting the entry */
   if ((day == 0) && (month == 0) && (year == 00)) {
      err = 0; day = ""; month = ""; year = ""; seperator = "";
   }
   /* if no error, write the completed date to Input-Field (e.g. 13.12.2001) */
   if (err == 0) {
      DateField.value = day + seperator + month + seperator + year;
   }
   /* Error-message if err != 0 */
   else {
      alert("Date is incorrect!");
      DateField.select();
	  DateField.focus();
   }
}

//...................................................................................................//

//Validates and alerts the user if any of the changes in the child GV are not submitted yet to the server.
function ValidateChangesSubmitted(gvClientID)
{
   var strDeletedRows = document.getElementById(gvClientID+'_hdnRowsToDelete').value;
   // var areAllDataRows = EnsureDataInRows(gvClientID, strDeletedRows);
    if(strDeletedRows.trim() != "")//Also make sure that deleted rows do not consist of blank rows.
    {
        var confirmOut = confirm("Changes made in the child Grid View are not yet submitted to the server!!\nClick OK to continue losing the changes or Cancel and then press Submit in the page to persist the changes in the server");
        if(confirmOut==true)//On ok
        {
            //Clear Delete Row selection
             document.getElementById(gvClientID+'_hdnRowsToDelete').value="";
            //To reset all control settings in cancel except for selectInvoice page(just disable the controls and dont close the form).
            //var currentPage = window.location + "";//Cast the object to string.
//            if(document.getElementById('ctl00_cphPageContents_pnlGVContent')==null)//if(currentPage.toUpperCase().indexOf('SELECTINVOICE.ASPX') != -1)            
//                OnButtonClick('Cancel');
//            else
                OnButtonClick('PageLoad');
        }
        return false;
    }
    else
    {
        //var currentPage = window.location + "";//Cast the object to string.
//        if(document.getElementById('ctl00_cphPageContents_pnlGVContent')==null)
//            OnButtonClick('Cancel');
//        else
            OnButtonClick('PageLoad');

        return false;
    }
}

//...................................................................................................//

function EnsureDataInRows(gvClientID, strDeletedRows)
{
    var strDeletedRowsSplit = strDeletedRows.split(',');
    var objGridView = document.getElementById(gvClientID);
    for(var i = 0; i < strDeletedRowsSplit.length; i++)
    {
        var rowIndex = strDeletedRowsSplit[i];
        var objGridViewRow = objGridView.rows[rowIndex];
        for(var j = 0 ;j< objGridViewRow.cells.length; j++)
        {   
            var objGridViewCell = objGridViewRow.cells[j];
            
        }
    }
}

//...................................................................................................//

//Makes sure that atleast and atmost one record is selected in the gridview
function ValidateForSingleSelection(objGridView)
{
    var selectionCntr = 0;
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        var checkBoxContainer = GetFirstChild(objGridView.rows[i].cells[0]);
        if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == true)
        {
            selectionCntr++;
        }
    }
    if(selectionCntr == 1)
    {
        return true;
    }
    else if(selectionCntr == 0)
    {
        alert('Please select a row first');
    }
    else if(selectionCntr > 1)
    {
        alert('Select only a single row');
    }
}

//...................................................................................................//

//Called when the Child-Level process links are clicked upon in the page.
function OnChildBPCLinkClick(sender, processBPGID, redirectPage, gridClientID, processName, isPopUp)
{
    if(sender.getAttribute("disabled") == 'disabled')
    {
        return;
    }
    //alert(processBPGID+"\n"+ redirectPage+"\n"+  gridClientID+"\n"+  processName+"\n"+  isPopUp)
    var objGridView  = document.getElementById(gridClientID);
    
    //First check that only single checkbox is selected
    if(!ValidateForSingleSelection(objGridView))
    {
        return;
    }
    
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        var checkBoxContainer = GetFirstChild(objGridView.rows[i].cells[0]);
        if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == true)
        {
            var strRowInfo = checkBoxContainer.parentNode.getAttribute("RowInfo");
            if(trim(strRowInfo) == "")
            {
                alert("Please select a row containing data");
                return;
            }
            //Everthing is fine so go on.
            //Load the Row Info into an DOM object
            var xDoc = loadXMLString(strRowInfo);
            var nodeRow = xDoc.getElementsByTagName("Rows")[0];
            var childTrxID = nodeRow.getAttribute("TrxID");
            var childTrxType = nodeRow.getAttribute("TrxType");
            
            //Get the hidden form info varible to retreive the page BPGID and PageInfo
            var hdnFormInfo = document.getElementById('ctl00_cphPageContents_GVUC_hdnFormInfo');
            if(hdnFormInfo==null)
            {
                hdnFormInfo = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnFormInfo')
            }
            if(hdnFormInfo)
            {
                hdnFormInfo = hdnFormInfo.value;
            }
            var formBPGID = hdnFormInfo.split('~::~')[0];
            var formPageInfo = hdnFormInfo.split('~::~')[1];
            var COCaption = nodeRow.getAttribute("LinkCaption");
            if(COCaption==null)
            {
                COCaption=nodeRow.getAttribute("Caption");
            }
            if(COCaption==null)
            {
                COCaption=nodeRow.getAttribute("Description");
            }
            if(COCaption==null)
            {
                COCaption=childTrxID;
            }
           
            //Now that we have all the necessary data frame the Calling Object XML
            var callingObjXML = "<CallingObject><BPGID>" + formBPGID + "</BPGID><TrxID>" + childTrxID + "</TrxID><TrxType>" + childTrxType + "</TrxType><PageInfo>" + formPageInfo + "</PageInfo><Caption>" + COCaption + "</Caption></CallingObject>";
            var COXML = strRowInfo + callingObjXML;
            CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML)
        }
    }
}

//...................................................................................................//

function getChildElement(parentNode)
{
//    x = parentNode.firstChild;
//    while (x.nodeType != 1)
//    {
//        if(x.nodeType == 3)
//        {
//            return x;//If the node is a Text node just return the same back.
//        }
//        x = x.nextSibling;
//    }
//    return x;
  
    var cellElement =jQuery(parentNode).find('input, select, span')[0];
    return cellElement;
}

//...................................................................................................//

function GetFirstChild(parentNode)
{
    try
    {
        x = parentNode.firstChild;
        while (x.nodeType != 1)
        {
            x = x.nextSibling;
        }
        //Due to the addition of attributes to the 
        //checkbox an additional span tag has been introduced as the parent node to the required checkbox.
        if(x.firstChild!=null && x.firstChild.nodeName!="#text")
        {
            return x.firstChild;
        }
        else
        {
            return x;
        }  
    }  
    catch(error)
    {
       if(g_IsDebugging)
       {
           g_ErrorText = "";
           g_ErrorText += "An error has occurred in this page";
           g_ErrorText += "\nFunction Name : GetFirstChild(gvClientID, sender)\nFileName : ChildGridView.js";
           g_ErrorText += "\nError Description : " + error.message;
           alert(g_ErrorText);
       }
    }
}

//...................................................................................................//

//This function is meant to be called on the Client Click of AddMoreRows
function AddRowsClick(gvClientID)
{
    if(g_temp){g_temp = false;}
}

//...................................................................................................//

//function AddRowsToGrid(gvClientID, sender)
//{
//    if(sender.getAttribute("disabled") == 'disabled')
//    {
//        return;
//    }
//    var objGridView  = document.getElementById(gvClientID);
//    var referenceRow = objGridView.rows[1];//The first data row.
//    var newRow = objGridView.insertRow(objGridView.rows.length);
//    for(var i = 0; i < referenceRow.cells.length; i++)
//    {
//        var cellElement = getChildElement(referenceRow.cells[i]);
//        var newCell = newRow.insertCell(newRow.cells.length);
//        if(cellElement.nodeName =='INPUT' && cellElement.type == "text")
//        {
//            var newTxtBox = document.createElement('input');
//            newTxtBox.type = 'text';
//            newTxtBox.value = '';
//            newTxtBox.style.width = cellElement.style.width;//Clone the node from the reference node.
//            newTxtBox.className = cellElement.className;
//            newCell.appendChild(newTxtBox);
//        }
//        else if(cellElement.nodeName =='SPAN')
//        {
//            var spanClone = cellElement.cloneNode(true);
//            spanClone.setAttribute("RowInfo", "");
//            spanClone.firstChild.checked = false;
//            newCell.align = "center";
//            newCell.appendChild(spanClone);
//        }
//        else if(cellElement.nodeName =='SELECT')
//        {
//            var ddlClone = cellElement.cloneNode(true);
//            ddlClone.childNodes[1].selected = 'selected';
//            newCell.appendChild(ddlClone);
//        }
//    }
//}

//...................................................................................................//

//Deletes the selected rows in the specified gridview(Client-side only)
function DeleteRow(gvClientID, sender)
{
    if(sender&&sender.getAttribute("disabled") == 'disabled')
    {
        return;
    }
    var objGridView  = document.getElementById(gvClientID);
    var strDeletedRows = "";
    var deleteCntr = 0;
    var arrNotDeleted=new Array();
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        if(objGridView.rows[i].style.display == "none")
        {
            //If the row is hidden it implies that it has already been marked for deletion in the hidden variable
            //hdnRowsToDelete, so ignore such a case.
            deleteCntr++;//To collapse the grid as the number decreases.
            continue;
        }
        var checkBoxContainer = GetFirstChild(objGridView.rows[i].cells[0]);
        if(checkBoxContainer.type && checkBoxContainer.type == "checkbox" && checkBoxContainer.checked == true)
        {
            var isOkToDelete=objGridView.rows[i].getAttribute("isoktodelete");
            if(isOkToDelete=="1")
            {
                arrNotDeleted[arrNotDeleted.length]=i;
            }
            else
            {
                //Temporary delete at the client-side only.Row still exists at the server!!
                //objGridView.deleteRow(i);
                //i--;
                //objGridView.rows[i].style.display = "none";
                jQuery(objGridView.rows[i]).fadeOut(600,
                                function()
                                {
                                    //Update the footer amount labels upon delete click
                                    if(objGridView.rows.length > 1)
                                    {
                                        var trCurrent= objGridView.rows[1];
                                        for(var cellCount = 1; cellCount < trCurrent.cells.length ; cellCount++)//1 - start from the second cell
                                        {
                                            var cellElement = getChildElement(trCurrent.cells[cellCount]);
                                            ShowInstantaneousSumOnPaste(cellElement,gvClientID);
                                        }
                                    }
                                }
                        );
                strDeletedRows += checkBoxContainer.parentNode.getAttribute("RowIndex");
                strDeletedRows += ",";
                deleteCntr++;
            }
        }
    }
    
   
    
    //pnlGVBranch
    var branchNodeName = gvClientID.replace('ctl00_cphPageContents_grdVw','');
    var CGVCClientID=gvClientID.replace('_grdVwBranch','');
    if(i - deleteCntr < 8)//Set height to auto only if rows in grid are less than 8.
    {
        var gvContainer = document.getElementById(CGVCClientID+'_pnlGVBranch');
        gvContainer.style.height = "auto";
        gvContainer.style.overflow = "";
        gvContainer.style.overflowX = "scroll";
    }
    
    //Alert the rows which were not deleted as they were marked as IsOkToDelete="0"
    var strNotDeleted="";
    for(var i=0;i<arrNotDeleted.length;i++)
    {
        if(i==arrNotDeleted.length-1 || i==0) {
            if(i>0) {
                strNotDeleted+=" and "+arrNotDeleted[i];
            }
            else {
                strNotDeleted+=" "+arrNotDeleted[i]
            }
        }
        else {
            strNotDeleted+=","+arrNotDeleted[i];
        }
    }
    if(strNotDeleted&&strNotDeleted.length>0)
    {
        var alertString=arrNotDeleted.length==1?"Row":"Rows";
        jqAlert(alertString+strNotDeleted+" cannot be deleted.",'Deletion Error',true);
    }
    
    var hdnRowsToDelete=document.getElementById(CGVCClientID+'_hdnRowsToDelete').value;
    hdnRowsToDelete += strDeletedRows;
    document.getElementById(CGVCClientID+'_hdnRowsToDelete').value =hdnRowsToDelete;
    
}

//Set the rows which modified in a hidden variable.
//Initaited in cs code.
function SetModRowIndices(hdnModId,rowIndex,columnIndex)
{
    var hdnModRows=document.getElementById(hdnModId);
    var arrMod=hdnModRows.value.split(',');
    arrMod.push(rowIndex);
    document.getElementById(hdnModId).value=RemoveDuplicates(arrMod).join(',');
    //To calcualte page level amounts in child grid view
    CalculatePagingAmounts(rowIndex,columnIndex);
}
//...................................................................................................//
// Calculate e paging child grid total amount
function CalculatePagingAmounts(rowIndex,columnIndex)
{
   var gvClientID='ctl00_cphPageContents_CGVUC_grdVwBranch';
   var ucId=gvClientID.replace('_grdVwBranch','');
    //Format of hdnAmounts: TotalAmt1-AmtColIndex1~TotalAmt2-AmtColIndex2~...........
   var hdnCurrAction=$get('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
   var sumValue=0;
   if((IsPaging(ucId))&&(hdnCurrAction.value=="Modify"))
   {
        var arrColAmts=$get(ucId+'_hdnAmounts').value.split('~');
        var hdnAmounts=$get(ucId+'_hdnAmounts');

        for(var index=0;index<arrColAmts.length;index++)
        {
          if(arrColAmts[index]!="")
          {
              var arrAmt=arrColAmts[0].split('|');
              var colamt=arrAmt[0];
              var colIndexs=arrAmt[1];
              if(colIndexs == columnIndex)
              {   
                var objGridView  = document.getElementById(gvClientID+'_GVRow'+rowIndex+'_txtAmount');
                var currentAmount=objGridView.value;
                var cellAmount = objGridView.getAttribute("OriginalAmount");
                var gridrowid=gvClientID+'_GVRow'+rowIndex+'_txtAmount';
                
                if(eval(currentAmount) > eval(cellAmount))
                {
                  //ADD
                   var sumTrx=trim(colamt.replace(/,/g,""))
                   var addAmount=parseFloat(currentAmount)-parseFloat(cellAmount);
                       addAmount=addAmount.toFixed(2);   
                       
                       sumValue=parseFloat(eval(sumTrx))+parseFloat(addAmount);  
                       sumValue=sumValue.toFixed(2);                  
                       hdnAmounts.value=hdnAmounts.value.replace(colamt,sumValue);
                
                     objGridView.setAttribute("OriginalAmount",currentAmount);
                }
                else if(eval(currentAmount) < eval(cellAmount))
                {
                  // SUBTRACT
                  var sumTrx=trim(colamt.replace(/,/g,""))
                     
                  var subAmount=parseFloat(cellAmount)-parseFloat(currentAmount);
                      subAmount=subAmount.toFixed(2);   
                     
                      sumValue = parseFloat(eval(sumTrx))-parseFloat(subAmount);
                      sumValue = sumValue.toFixed(2);
                  hdnAmounts.value=hdnAmounts.value.replace(colamt,sumValue);
                  objGridView.setAttribute("OriginalAmount",currentAmount);
                }
                //  alert(colamt);
              }// colindex end
          }// arr index end
        } // for loop end
    } // if end
}
//...................................................................................................//

//Toggles the checkbox selection in the specified grid view.
function ToggleSelection(gvClientID, sender)
{
    if(sender.getAttribute("disabled") == 'disabled')
    {
        return;
    }
    var objGridView  = document.getElementById(gvClientID);
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        var checkBoxContainer = GetFirstChild(objGridView.rows[i].cells[0]);
        if(checkBoxContainer.type && checkBoxContainer.type == "checkbox")
        {
            if(checkBoxContainer.checked == true)
            {
                checkBoxContainer.checked = false;
            }
            else
            {
                checkBoxContainer.checked = true;
            }
        }
    }
}

//...................................................................................................//

//Returns the sum of values in a specified column in the grid view.
//Note : trim function is defination is defined in common.js
function ColumnSum(gvClientID,colIndex)
{
    
    var ucId=gvClientID.replace('_grdVwBranch','');
    //Format of hdnAmounts: TotalAmt1-AmtColIndex1~TotalAmt2-AmtColIndex2~...........
    var hdnCurrAction=$get('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    var sumValue=0;
    var parsedText=0;    
   if((IsPaging(ucId))&&(hdnCurrAction.value=="Modify"))
   {
       //Calculate sum Modify case pageing child grid view
        var arrColAmts=$get(ucId+'_hdnAmounts').value.split('~');
        for(var index=0;index<arrColAmts.length;index++)
        {
          if(arrColAmts[index]!="")
          {
              var arrAmt=arrColAmts[0].split('|');
              var colamt=arrAmt[0];
              var colIndexs=arrAmt[1];
                  colIndex=colIndex-1;
              if(colIndex == colIndexs)
              {   
                var sumTrx=trim(colamt.replace(/,/g,""))
                   //Convert To float
                   parsedText=parseFloat(eval(sumTrx));
                   //Float value is valid or not
                   if(isFinite(parsedText))
                   {
                       sumValue=f_floatAdd(sumValue,parsedText);
                   }
                  
              }
          }
        }
        sumValue=sumValue.toFixed(2);
    }
    else
    {
        //Calculate sum ADD case pageing child gridview and all other child gridviews
        var objGridView  = document.getElementById(gvClientID);
        for( var i = 1; i < objGridView.rows.length; i++)
        {
            var columnTxtbx = getChildElement(objGridView.rows[i].cells[colIndex]);
            
            if(columnTxtbx && columnTxtbx.value.length > 0)
            {
               //none is verifying if the rows deleted
              if(objGridView.rows[i].style.display != "none")
              {
               var sumTrx=trim(columnTxtbx.value.replace(/,/g,""))
                   //console.log(columnTxtbx +"\n"+ sumValue);
                   //Convert To float
                   parsedText=parseFloat(eval(sumTrx));
                   //Float value is valid or not
                   if(isFinite(parsedText))
                   {
                       sumValue=f_floatAdd(sumValue,parsedText);
                   }
                   
                   
               }
            }
        }
         //negative values it show exponent value this fixing 2 digits only
        sumValue=sumValue.toFixed(2);
    }   
    return sumValue;
}

//...................................................................................................//

//Returns the sum of values in a specified column in the grid view.
//Note : trim function is defination is defined in common.js
function ColumnSumPositiveNegative(gvClientID,colIndex)
{
    var objGridView  = document.getElementById(gvClientID);
    var sumPositiveValue=0;
    var sumNegativeValue=0;
    var sumCorrected=0;
    var parsedText=0;
    var bothvalues=new Array();
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        var columnTxtbx = getChildElement(objGridView.rows[i].cells[colIndex]);
        if(columnTxtbx && columnTxtbx.value.length > 0)
        {   
            //none is verifying if the rows deleted
            if( objGridView.rows[i].style.display != "none")
            {
                sumCorrected=trim(columnTxtbx.value.replace(/,/g,""));
                //convert to float
                parsedText=parseFloat(sumCorrected);
                if(eval(sumCorrected)<0)
                {   //Check it is valid number or not
                    //functions isFinite and f_floatAdd are exist in Utility.js file
                    if(isFinite(parsedText))
                    {
                       sumNegativeValue=f_floatAdd(sumNegativeValue,parsedText);
                    }
                }
                else
                { 
                  if(isFinite(parsedText))
                  {
                    sumPositiveValue=f_floatAdd(sumPositiveValue,parsedText);
                  }
                }
            }
        }
    }
    bothvalues[0]=sumPositiveValue;
    bothvalues[1]=sumNegativeValue;
    return bothvalues;
}

//...................................................................................................//

//Validate the entered columns. Is numeric or not.
//Note : IsNumeric function is defination is defined in common.js
function ValidateColumns(gvClientID,colIndex)
{
    var objGridView  = document.getElementById(gvClientID);
    var ErrorCode=0;
    for( var i = 1; i < objGridView.rows.length; i++)
    {
        var columnTxtbx = getChildElement(objGridView.rows[i].cells[colIndex]);
        if(columnTxtbx && columnTxtbx.value.length > 0)
        {
            if (IsNumeric(trim(columnTxtbx.value))==false)
            {
                 ErrorCode="111";
                 return ErrorCode;
                 break;
            }
            if (ValidateDecimals(trim(columnTxtbx.value))==false)
            {
                ErrorCode="222";
                return ErrorCode;
                break;
            }
        }
    }
    return ErrorCode;
}

//...................................................................................................//

//Calculates the column sums on the page load event.
function DisplayOnGridLoadSum(gridID,targetColIndices)
{
    var objGrid = document.getElementById(gridID);
    var arrColIndices = targetColIndices.split('*');
    var ucId=gridID.replace('_grdVwBranch','');
    
    //Format of hdnAmounts: TotalAmt1-AmtColIndex1~TotalAmt2-AmtColIndex2~...........
    if(document.getElementById(ucId+'_hdnAmounts')==null)
    {
        return;
    }
    var arrColAmts=document.getElementById(ucId+'_hdnAmounts').value.split('~');
    for(var index=0;index<arrColIndices.length;index++)
    {
        if(arrColIndices[index].length>0)
        {
            var colIndex = parseInt(arrColIndices[index]);
            if(IsPaging(ucId))
            {
                //Show the amount values from the Hidden variable hdnAmounts.
                 var tdLabel = document.getElementById('tdAmt'+colIndex);
                 if(tdLabel){
                    tdLabel.innerHTML = arrColAmts[index].split('|')[0];
                 }
            }
            else
            {
                ShowInsantaneousSum(gridID, colIndex);
            }
        }
    }
}

function IsPaging(ucId)
{
    var hdnIsPaging=document.getElementById(ucId+'_hdnIsPaging');
    if(hdnIsPaging&&hdnIsPaging.value=="1")
    {
        return true;
    }
    else
    {
        return false;
    }
}
//...................................................................................................//

//Displays the instantaneaous sum of the Amount Column in the Grid. Called in the onchange event of the textbox.
function ShowInsantaneousSum(parentGridID, colIndex)
{
    var objGrid = document.getElementById(parentGridID);
    if(objGrid)
    {
        var colSum = 0;
        var cellIndex = parseInt(colIndex)+1;
        var decCellIndex = false;
        for (var rowCount = 1; rowCount < objGrid.rows.length ; rowCount++ )
        {   
            //Check if previously this row was deleted.If deleted don't consider.
            if(objGrid.rows[rowCount].style.display == "none")
            {
                continue;
            }
            
            //Check if the last row is a row added as a result of inclusion of paging. If true,ignore it.
            if(rowCount==objGrid.rows.length-1)  //Last Row check.
            { 
                if( objGrid.rows[rowCount] && objGrid.rows[rowCount-1] )
                {
                    if(objGrid.rows[rowCount].childNodes.length!=objGrid.rows[rowCount-1].childNodes.length)
                    {
                        continue;
                    }
                }
            }
           
            var chkSelect=GetFirstChild(objGrid.rows[rowCount].cells[0]);
            if(chkSelect.id.indexOf('chkBxSelectRow')==-1)
            {
                //Implies the first selection checkbox column is missing so decrement the cellIndex variable.
               decCellIndex = true;
            }
             //Sum only checked rows
            if(g_AddSelected)
            {
                if(chkSelect.checked==false)
                {
                    continue;
                }
            }
            var cellIndex2=cellIndex;
            if(decCellIndex)
            {
                cellIndex2=cellIndex2-1;
            }
            var currentRowTB = getChildElement(objGrid.rows[rowCount].cells[cellIndex2]);//+1 - override the checkbox
            if(currentRowTB.type != "text")
            {
//                alert("Erroorr-Testing Phase:ChildGridview.js-ShowInsantaneousSum()");
//                var cellId="cell"+rowCount+cellIndex;
//                objGrid.rows[rowCount].cells[cellIndex2].id=cellId;
                currentRowTB=jQuery(objGrid.rows[rowCount].cells[cellIndex2]).find('input:visible')[0];
            }
            var dataRowCount = rowCount-1;
            if(currentRowTB)
            {
                var strNumber = currentRowTB.value.replace(/,/g,"");
                var isNotaNumber = isNaN(strNumber);
                if(!isNotaNumber)
                {
                    var parsedText = parseFloat(strNumber);
                    if(isFinite(parsedText))
                    {
                        colSum+= parsedText;
                    }
                }
            }
        }
        var tdLabel = document.getElementById('tdAmt'+colIndex);
        if(tdLabel) {
            tdLabel.innerHTML = formatCurrency(colSum);
        }
   }
}

function AutoBalance(objTxt, ctrlTotTxt)
{
   // Check attribute Own exist or not. 
   // If own attribute available set default amount as own amount.
   // added on 16-11-09
    var attOwn=objTxt.getAttribute('Own');
    if(attOwn!=null)
    {
      objTxt.value=attOwn;
      return;
    }


    var controlname="ctl00_cphPageContents_txt"+ctrlTotTxt;
    var ControlTotal=document.getElementById(controlname).value.replace(/,/g,"");
    ControlTotal=parseFloat(ControlTotal);
    //alert('Original Control Total is '+ControlTotal);
    //1.Get the current row's index
    //ctl00_cphPageContents_CGVUC_grdVwBranch_GVRow1_txtAmount
    var splitId=objTxt.id.split('_');
    var currentIndex=parseInt(splitId[4].replace('GVRow',''));
    //alert('Current Row Index is '+currentIndex);
    var gvClientID=splitId[0]+'_'+splitId[1]+'_'+splitId[2]+'_'+splitId[3];
    var objGrid=$get(gvClientID);
    
    //2.Loop and sum all textboxes upto the current row index
    var colSum=0;
    for(var rowCntr=0;rowCntr<=objGrid.rows.length;rowCntr++)
    {
        splitId[4]='GVRow'+ rowCntr;
        var rowTB=document.getElementById(splitId.join('_'));
        if(rowTB)
        {
            //Check if previously this row was deleted.If deleted don't consider.
            var targetTR=jQuery(rowTB).parents("tr[id*='GVRow']");//Get the parent row of the current TextBox.
            if(targetTR.length>0 && targetTR[0].style.display=="none")
            {
                continue;
            }
            var strNumber = rowTB.value.replace(/,/g,"");
            var isNotaNumber = isNaN(strNumber);
            if(!isNotaNumber)
            {
                var parsedText = parseFloat(strNumber);
                if(isFinite(parsedText))
                {
                    colSum+= parsedText;
                }
            }
        }
    }
    //alert('The column sum is '+colSum);
    //3.Find the target textbox(The first visible textbox after the current tabbed out txtbx(Old Requirement)) and fill in the difference.
    splitId[4]='GVRow'+(currentIndex);
    var rowTB=document.getElementById(splitId.join('_'));
    if(rowTB)
    {
        //alert(rowTB.offsetParent.parentNode.style.display);
//        while(rowTB.offsetParent==null || rowTB.offsetParent.parentNode.style.display == "none")
//        {
//            splitId[4]='GVRow'+(++currentIndex);;
//            rowTB=document.getElementById(splitId.join('_'));
//            if(rowTB==null)
//            {
//                break;
//            }
//        }
//        var diff=ControlTotal-colSum;
//        if(diff!=0)
//        {
//            //rowTB.value=formatCurrency(diff);
//            rowTB.value=formatCurrency(parseCustomInt(rowTB.value)+diff);
//            ShowInstantaneousSumOnPaste(rowTB,gvClientID)
//        }

        var diff=ControlTotal-colSum;
        var tbVal=parseCustomInt(rowTB.value);
        //alert('ColSum:'+colSum+"\nTBVal:"+tbVal);
        if(rowTB.value.length==0)
        {
            rowTB.value=formatCurrency(diff);
            ShowInstantaneousSumOnPaste(rowTB,gvClientID);
            
            var CGVCClientID=gvClientID.replace('_grdVwBranch','');
            if(IsPaging(CGVCClientID))
            {
                //If Paging is ON update the hdnModRows variable upon pasting of a row.
                //Get the pasted row's index
                var rowIndex=currentIndex;//checkBoxContainer.parentNode.getAttribute("RowIndex");
                var hdnModId=CGVCClientID+'_hdnModRows';
                SetModRowIndices(hdnModId,rowIndex);
            }
        }
    }
}

//...................................................................................................//

function parseCustomInt(str)
{
    var strNumber = str.replace(/,/g,"");
    var isNotaNumber = isNaN(strNumber);
    if(!isNotaNumber)
    {
        var parsedText = parseFloat(strNumber);
        if(isFinite(parsedText))
        {
           return parsedText;
        }
    }
    return 0;
}

//Formats a given string/float to comma delimited currency notation.
function formatCurrency(num) {
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
return (((sign)?'':'-') + num + '.' + cents);
}

//...................................................................................................//

//Set tab focus on next row
function SetFocusNextRow(e,sender)
{
    var keyCode = e.keyCode||e.which; 
    if (keyCode==9) { 
        var idSplit=sender.id.split('_');
        var rowIndex=parseInt(idSplit[4].replace('GVRow',''))+1;
        var gridId=idSplit[0]+'_'+idSplit[1]+'_'+idSplit[2]+'_'+idSplit[3];
        var rowSelector="#"+gridId+" tr[id*='GVRow']:eq("+rowIndex+")";
        //Select input text box. here 0 th element is checkbox   
        var inputSelector=":input:visible:eq(1)";
        var target=jQuery(rowSelector).find(inputSelector);
        if(target.size()>0)
        {
            if(target.closest('tr').css('display')==='none') {
                //Call recursively for deleted rows
                SetFocusNextRow(e,target[0]);
            }
            else {
                if(e.preventDefault)e.preventDefault();
                else e.returnValue=false;
                setTimeout(function(){target[0].focus();},4);
            }
        }
    }
}

//...................................................................................................//
//Reset COA Attribute for Job/Division when TextBox is empty
// Add on 27-11-09
function ReSetCOA(TBoxId)
{
    //Account/Line TextBoxId
    var curr = jQuery("#"+TBoxId);
    //Get job/Division textbox id
    curr = curr.parent('td').prev().children('input');
    //Check Job/Division textbox length
    if(curr.val().length=="0")
    {
        if(curr.attr('COA').length > 0)
        { 
           //Set COA attribute empty
           //curr.attr('COA','');
           
           //Set COA defaut value when ever Account/Line text box is empty. New requirement on 08-01-09
           var coaDefault=jQuery("#ctl00_cphPageContents_CGVUC_hdnJobCOADefault").val();
           curr.attr('COA',coaDefault);
        }
    }
}

