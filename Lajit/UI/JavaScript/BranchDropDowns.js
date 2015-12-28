//Purpose: To update the dependencies of a DDL upon change in the client-side itself.
//This file contains methods used to save and update values of branch data into an hidden variable upon Drop-Down
//selected index change.
//References: "hdnBranchXML" found in ButtonsUC - Acts as a scrath pad for all the rendering and saving of data on each change of DDL

//Event handler for the branch DDL change event.
function OnBranchDropDownChange(objDDL)
{
    var splitIDValues = objDDL.id.split('_');
    var branchNode = splitIDValues[3];
    var ddlLabelName = splitIDValues[2].replace('ddl','');
    var selectedIndex = document.getElementById(objDDL.id).selectedIndex;
    if(selectedIndex != 0)
    {
        var prevSelItem=objDDL.getAttribute("MapPreviousSelItem");
        SaveData(branchNode, ddlLabelName, objDDL.id, "");
        FillData(branchNode, ddlLabelName, objDDL);
    }
    else
    {
        //Choose selected..
        var arrBranchControls = GetBranchControls(branchNode);             
        SaveData(branchNode, ddlLabelName, objDDL.id, "");
        ClearBranchControls(branchNode, arrBranchControls, ddlLabelName);
    }
}

//...................................................................................................//

function FillData(branchNodeName, ddlLabelName, objDDL)
{
    var hdnBranchXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML').value;
    if(trim(hdnBranchXML).length> 0)
    {
        var xDocBranchXML = loadXMLString(hdnBranchXML);
        var nodeBranch =  xDocBranchXML.getElementsByTagName(branchNodeName);//Branch Node
        
        if(nodeBranch.length >0)//If Branch does not exist
        {
            var nodeBranchRowList = nodeBranch[0].getElementsByTagName("RowList");
            if (nodeBranchRowList.length >0)
            {
                var currSplitIds = objDDL.value.split('~');
                var currTrxId = currSplitIds[0];
                var currTrxType = currSplitIds[1];
                nodeBranchRowList = nodeBranchRowList[0];
                var nodeRow = GetRowByTrxIdAndTrxType(nodeBranchRowList, currSplitIds[0], currSplitIds[1], ddlLabelName);
                if (nodeRow)
                {
                    var arrBranchControls = GetBranchControls(branchNodeName);
                    
                    for(var i=0;i< arrBranchControls.length;i++)
                    {
                        var thisControl = document.getElementById(arrBranchControls[i]);
                        if(!thisControl)
                        {
                            continue;
                        }
                        var splitIDValues = thisControl.id.split('_');
                        var branchNode = splitIDValues[3];
                        if(thisControl.nodeName == "SELECT")//DropDownList
                        {   
                            var currentDDLLabelName = splitIDValues[2].replace('ddl','');
                            if(currentDDLLabelName != ddlLabelName)
                            {
                                var valueToBeSelected = nodeRow.getAttribute(currentDDLLabelName + '_TrxID') + "~" + nodeRow.getAttribute(currentDDLLabelName + '_TrxType');
                                if(valueToBeSelected.toString().length>1){
                                    thisControl.value=valueToBeSelected;
                                }
//                                else {
//                                    //Default to the first item.
//                                    if(thisControl.options.length>0) {
//                                        thisControl.options[0].selected = true;
//                                    }
//                                }
                            }
                        }
                        else if(thisControl.nodeName == "SPAN")//Checkbox with an attibute
                        {
                            var currentChkLabelName = splitIDValues[2].replace('chk','');
                            var thisCheckBox = getChildElement(thisControl);
                            if(nodeRow.getAttribute(currentChkLabelName))
                            {
                                if(nodeRow.getAttribute(currentChkLabelName) == "1")
                                {
                                    thisCheckBox.checked = true;
                                }
                                else
                                {
                                    thisCheckBox.checked = false;
                                }
                            }
                        }
                        else//Others
                        {   
                            //Check whether control is a checkbox
                            if(thisControl.type=='checkbox')
                            {
                                var currentChkLabelName = splitIDValues[2].replace('chk','');
                                if(nodeRow.getAttribute(currentChkLabelName))
                                {
                                    if(nodeRow.getAttribute(currentChkLabelName) == "1")
                                    {
                                        thisControl.checked=true;
                                    }
                                    else
                                    {
                                        thisControl.checked=false;
                                    }
                                }
                            }
                            else
                            {
                                var txtLabelName = splitIDValues[2].replace('txt','');
                                if(nodeRow.getAttribute(txtLabelName))                               
                                    thisControl.value= nodeRow.getAttribute(txtLabelName);
                                    //Set focus for textbox added 190209
                                    thisControl.focus();    
                                    objDDL.focus();
                            }
                        }
                    }
                                        
                    //Setting MapPrevSelItem.
                    var ddlSelText = objDDL.options[objDDL.selectedIndex].text;
                    objDDL.setAttribute("MapPreviousSelItem", ddlSelText);
                }
            }
        }
    }

    
    
}

//...................................................................................................//

//Setting BPAction
function setBPAction()
{    
    var hdnBranchXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML').value;
    if(trim(hdnBranchXML).length != 0)
    {
        var xDocBranchXML = loadXMLString(hdnBranchXML);
        var nodeRoot =  xDocBranchXML.getElementsByTagName("Root");//Root Node
        var listBranches = nodeRoot[0].getElementsByTagName("Rows");
        if(listBranches.length>0)
        { 
            //Get the Submit Image Button's Alternate Text(BPAction).
            var imgBtnSubmit = document.getElementById('ctl00_cphPageContents_imgbtnSubmit');
            //var imgBtnSubmitAlt = imgBtnSubmit.alt;
            var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
            var imgBtnSubmitAlt=hdnCurrAction.value;
                   
            for(i = 0; i < listBranches.length; i++ )
            {
                var nodeRow=listBranches[i];
                if(imgBtnSubmit.attrSave == "save")
                {
                    nodeRow.setAttribute("BPAction", "Add");
                }
                else if(imgBtnSubmitAlt!="Save")
                {
                    if(nodeRow.getAttribute('TrxID'))
                    {
                        nodeRow.setAttribute("BPAction",imgBtnSubmitAlt );
                    }
                    else
                    {
                        if(imgBtnSubmitAlt=="Find")
                        {
                            nodeRow.setAttribute("BPAction",imgBtnSubmitAlt );
                        }
                        else
                        {
                            nodeRow.setAttribute("BPAction", "Add");
                        }
                    }                                    
                }
            }
            //Convert the Document back to a string.
            document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML').value = ConvertToString(xDocBranchXML); 
        }                
    }    
}

//...................................................................................................//

//Saves the data entered in the dependant controls of the target drop-downs.
function SaveData(branchNodeName, ddlLabelName, objDDL, action)
{
//debugger;
    objDDL = document.getElementById(objDDL);//Convert the Id To the object
    var arrBranchControls = GetBranchControls(branchNodeName);
    //Variable where the branch XML is stored.
    var hdnBranchXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML').value;
    if(trim(hdnBranchXML).length == 0)
    {
        hdnBranchXML = "<Root></Root>";
    }
    var xDocBranchXML = loadXMLString(hdnBranchXML);
    var nodeBranch =  xDocBranchXML.getElementsByTagName(branchNodeName);//Branch Node
    var save = true;
    var primaryExists=true;
    //Get the Submit Image Button's Alternate Text(BPAction).
    var imgBtnSubmit = document.getElementById('ctl00_cphPageContents_imgbtnSubmit');
    //var imgBtnSubmitAlt = imgBtnSubmit.alt;
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    var imgBtnSubmitAlt=hdnCurrAction.value;
    
    var dataEntered=CheckDataEntered(arrBranchControls, ddlLabelName);
    
    if(nodeBranch.length >0)//If Branch exists
    {
        var nodeBranchRowList = nodeBranch[0].getElementsByTagName("RowList");
        if (nodeBranchRowList.length >0)
        {
            nodeBranchRowList = nodeBranchRowList[0];
            var prevSelectedText = objDDL.getAttribute("MapPreviousSelItem");//**Dont forget to set this attribute with the current selected
            
          
            
            var selectedValue = GetDDLValueByText(objDDL,prevSelectedText);
            var arrSplitValue = selectedValue.split('~'); 
            var nodeRow = GetRowByTrxIdAndTrxType(nodeBranchRowList, arrSplitValue[0], arrSplitValue[1], ddlLabelName);
            
            if (nodeRow)// If row exists.. modifying it
            {
                save = false;
                if((!dataEntered) && (nodeRow.getAttribute('TrxID')))// a DB row but data not entered 
                {
                    dataEntered=true;
                }
                
                if(dataEntered)
                {
                    for(var i=0;i< arrBranchControls.length;i++)
                    {
                        var thisControl = document.getElementById(arrBranchControls[i]);
                        if(!thisControl)
                        {
                            continue;
                        }
                        var splitIDValues = thisControl.id.split('_');
                        var branchNode = splitIDValues[3];
                        if(thisControl.nodeName == "SELECT")//DropDownList
                        {   
                            var currentDDLLabelName = splitIDValues[2].replace('ddl','');
                            if(currentDDLLabelName != ddlLabelName)
                            {
                                var currSplitIds = thisControl.value.split('~');
                                var currTrxId = currSplitIds[0];
                                var currTrxType = currSplitIds[1];
                                nodeRow.setAttribute(currentDDLLabelName + '_TrxID', currTrxId);
                                nodeRow.setAttribute(currentDDLLabelName + '_TrxType', currTrxType);
                            }
                        }
                        else if(thisControl.nodeName == "SPAN")//Checkbox with an attibute
                        {
                        
                            var currentChkLabelName = splitIDValues[2].replace('chk','');
                            if(getChildElement(thisControl).checked==true)
                            {
                                nodeRow.setAttribute(currentChkLabelName, "1");
                            }
                            else
                            {
                                nodeRow.setAttribute(currentChkLabelName, "0");
                            }
                        }
                        else//Others
                        {   
                            //Check whether control is a checkbox
                            if(thisControl.type=='checkbox')
                            {
                                var currentChkLabelName = splitIDValues[2].replace('chk','');
                                if(thisControl.checked==true)
                                {
                                    
                                    nodeRow.setAttribute(currentChkLabelName, "1");
                                }
                                else
                                {
                                    nodeRow.setAttribute(currentChkLabelName, "0");
                                }
                            }
                            else
                            {
                                var txtLabelName = splitIDValues[2].replace('txt','');
                                nodeRow.setAttribute(txtLabelName, thisControl.value);
                            }
                        }
                    }
                    //Check for IsPrimary
                    var isPrimary = nodeRow.getAttribute("IsPrimary");
                    var nodePrimaryRow = GetRowByAttributeValue(nodeBranchRowList,"IsPrimary","1");
                    if (isPrimary && isPrimary=="1")//If Current new row is set as primary, Making the one which is already IsPrimary as 0.
                    {                    
                        if (nodePrimaryRow && nodeRow != nodePrimaryRow)
                        {
                            nodePrimaryRow.setAttribute("IsPrimary","0");
                        }
                    }
                    else
                    {
                        if (!nodePrimaryRow)
                        {
                           primaryExists=false; 
                        }
                    }                   
                }
                else//If Data not entered , removing that row
                {
                    //nodeBranchRowList.removeChild(nodeRow);
                    if(!nodeRow.getAttribute('TrxID'))// Not a DB row
                    {
                         nodeBranchRowList.removeChild(nodeRow);
                    }
                }
                
            }
        }
    }
    
//    //Check whether the Root-Level has been created or not. If created initialise a variable indicating the same.
//    var doesRootExist = false;
//    if(xDocBranchXML)
//    {
//      doesRootExist = true;
//    }
    
    //New Row is suppossed to be created if true.
    if(save)
    {
        if(dataEntered)
        {
            if(xDocBranchXML)
            {
                var nodeBranch =  xDocBranchXML.getElementsByTagName(branchNodeName);//Branch Node
                if(nodeBranch.length ==0)//If Branch does not exist
                {
                    var newBranch = xDocBranchXML.createElement(branchNodeName);//Create a new Branch Node
                    xDocBranchXML.firstChild.appendChild(newBranch);
                    var newRowList = xDocBranchXML.createElement("RowList");//Create a new RowList Node
                    newBranch.appendChild(newRowList);
                }
                else
                {
                    var nodeBranchRowList = nodeBranch[0].getElementsByTagName("RowList");
                    if (nodeBranchRowList.length == 0)//If RowList does not Exist
                    {  
                        var newRowList = xDocBranchXML.createElement("RowList");//Create a new RowList Node
                        nodeBranch[0].appendChild(newRowList);
                    }
                }
            }
            else
            {
                xDocBranchXML = loadXMLString("<Root></Root>");//Create the Document.
    //            var nodeRoot = xDocBranchXML.getElementsByTagName("Root");
                var newBranch = xDocBranchXML.createElement(branchNodeName);//Create a new Branch Node
                xDocBranchXML.appendChild(newBranch);
                var newRowList = xDocBranchXML.createElement("RowList");//Create a new RowList Node
                newBranch.appendChild(newRowList);
            }
            
            
            //Case of adding a new row which did not exist previously.
            var newRow = xDocBranchXML.createElement("Rows");//Create a new Row and set the attributes.
            
            for(var i=0;i< arrBranchControls.length;i++)
            {
                var thisControl = document.getElementById(arrBranchControls[i]);
                if(!thisControl)
                {
                    continue;
                }
                var splitIDValues = thisControl.id.split('_');
                var branchNode = splitIDValues[3];
                if(thisControl.nodeName == "SELECT")//DropDownList
                {   
                    var currentDDLLabelName = splitIDValues[2].replace('ddl','');
                    if(currentDDLLabelName != ddlLabelName)
                    {
                        var currSplitIds = thisControl.value.split('~');
                        var currTrxId = currSplitIds[0];
                        var currTrxType = currSplitIds[1];
                        if(currTrxId == -1)
                        {
                            currTrxId = "";
                            currTrxType = "";
                        }
                        newRow.setAttribute(currentDDLLabelName + '_TrxID', currTrxId);
                        newRow.setAttribute(currentDDLLabelName + '_TrxType', currTrxType);
                    }
                    else
                    {
                        var prevSelectedText = objDDL.getAttribute("MapPreviousSelItem");//**Dont forget to set this attribute with the current selected
                        var arrSplitValue = "";
                        if(prevSelectedText != objDDL.options[0].text)//If Prevoius Selected item is not equal to Choose.
                        {
                            var selectedValue = GetDDLValueByText(objDDL,prevSelectedText);
                            arrSplitValue = selectedValue.split('~'); 
                        }
                        else
                        {
                            arrSplitValue = objDDL.value;
                        }
                        var currTrxId = arrSplitValue[0];
                        var currTrxType = arrSplitValue[1];
                        if(currTrxId == -1)
                        {
                            currTrxId = "";
                            currTrxType = "";
                        }
                        newRow.setAttribute(ddlLabelName + '_TrxID', currTrxId);
                        newRow.setAttribute(ddlLabelName + '_TrxType', currTrxType);                                        
                    }
                }
                else if(thisControl.nodeName == "SPAN")//Checkbox with an attibute
                {
                    var currentChkLabelName = splitIDValues[2].replace('chk','');
                    if(getChildElement(thisControl).checked==true)
                    {
                        newRow.setAttribute(currentChkLabelName, "1");
                    }
                    else
                    {
                        newRow.setAttribute(currentChkLabelName, "0");
                    }
                }
                else//Others
                {   
                    //Check whether control is a checkbox
                    if(thisControl.type=='checkbox')
                    {
                        var currentChkLabelName = splitIDValues[2].replace('chk','');
                        if(thisControl.checked==true)
                        {
                            newRow.setAttribute(currentChkLabelName, "1");
                        }
                        else
                        {
                            newRow.setAttribute(currentChkLabelName, "0");
                        }
                    }
                    else
                    {
                        var txtLabelName = splitIDValues[2].replace('txt','');
                        newRow.setAttribute(txtLabelName, thisControl.value);
                    }
                }
            }
            
            var nodeNewBranchRowList = xDocBranchXML.getElementsByTagName(branchNodeName)[0].getElementsByTagName("RowList")[0];
            nodeNewBranchRowList.appendChild(newRow);
    //        alert((new XMLSerializer()).serializeToString(newRow));
    //        alert((new XMLSerializer()).serializeToString(nodeNewBranchRowList));
        
            //Check for IsPrimary
            var isPrimary = newRow.getAttribute("IsPrimary");
            var nodePrimaryRow = GetRowByAttributeValue(nodeNewBranchRowList,"IsPrimary","1");
            if (isPrimary && isPrimary=="1")//If Current new row is set as primary, Making the one which is already IsPrimary as 0.
            {   
                if (nodePrimaryRow && newRow != nodePrimaryRow)         
                //if (nodePrimaryRow)
                {
                    nodePrimaryRow.setAttribute("IsPrimary","0");
                }
            }
            else //If current Row is not Primary
            {
                if(!nodePrimaryRow)// Also if no other is set as Primary, if Default exists Setting it as IsPrimary else making DDL FirstItem as IsPrimary
                {
                    primaryExists=false;
                }
            }
        }
    }
   
//    if(!primaryExists)// If no row is set as Primary, if Default exists Setting it as IsPrimary else making DDL First entered Item as IsPrimary
//    {            
//        //Check for default value        
//        var xDocGVDataXML = loadXMLString(document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchColsXML').value);
//        
//        var branchNode = xDocGVDataXML.getElementsByTagName(branchNodeName);
//        var nodeColumns = null;
//        if(branchNode.length > 0)
//        {
//            nodeColumns = branchNode[0].getElementsByTagName('Columns');                  
//        }
//        if(nodeColumns.length > 0)
//        {
//            var nodeUpdatedBranchRowList = nodeBranch[0].getElementsByTagName("RowList")[0];
//            var nodeColDefault = GetRowByAttributeValue(nodeColumns[0], "Label", ddlLabelName);
//            var defaultAttribute = nodeColDefault.getAttribute("Default");
//            var defaultToFirstEnteredItem=false;
//            if(defaultAttribute && defaultAttribute!="") //If Default exists setting that value as Primary
//            {
//               var nodeDefaultRow = GetRowByAttributeValue(nodeUpdatedBranchRowList ,ddlLabelName+"_TrxID",defaultAttribute); 
//               if(nodeDefaultRow)
//               {
//                    nodeDefaultRow.setAttribute("IsPrimary","1");
//               }
//               else
//               {
//                    defaultToFirstEnteredItem=true;
//               }
//            }
//            else//Setting First entered Item(Next to "Choose") in the DDL as primary
//            {
//                defaultToFirstEnteredItem=true;
//            }
//            if(defaultToFirstEnteredItem)
//            {
//                for(var index =0;index<objDDL.length;index++)
//                {                            
//                    var firstItemSplitIds = objDDL[index].value.split('~');
//                    var firstItemTrxId = firstItemSplitIds[0];
//                    var firstItemTrxType = firstItemSplitIds[1];
//                    //nodeBranchRowList = nodeBranchRowList[0];
//                    var nodeFirstItemRow = GetRowByTrxIdAndTrxType(nodeUpdatedBranchRowList , firstItemSplitIds[0], firstItemSplitIds[1], ddlLabelName);
//                    if (nodeFirstItemRow)
//                    {
//                        nodeFirstItemRow.setAttribute("IsPrimary","1");
//                        break ;
//                    }
//                }
//                
//            }
//            
//        }
//    }
    
        
    //Common
    //Setting MapPrevSelItem.
    var ddlSelectedIndex = objDDL.selectedIndex;
    var ddlSelectedText = objDDL.options[ddlSelectedIndex].text;
    objDDL.setAttribute("MapPreviousSelItem", ddlSelectedText);
    
    var prevSelectedText = objDDL.getAttribute("MapPreviousSelItem");
    var previousSelValue = GetDDLValueByText(objDDL,prevSelectedText);
    var arrSplitValue = previousSelValue.split('~'); 
    //Clearing the branch control once the entered values hae been saved.
    if (arrSplitValue[0]!=-1 && action=="")
    {
        ClearBranchControls(branchNodeName, arrBranchControls, ddlLabelName);
    }      
    //Convert the XML back to string and store it back into the repository.
    document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML').value = ConvertToString(xDocBranchXML);
}

//...................................................................................................//

function CheckDataEntered(arrBranchControls, ddlLabelName)
{
             
    for(var i=0; i<arrBranchControls.length; i++)
    {
        var thisControl = document.getElementById(arrBranchControls[i]);
        if(!thisControl)
        {
            continue;
        }
        var splitIDValues = thisControl.id.split('_');
        var branchNode = splitIDValues[3];
        if(thisControl.nodeName == "SELECT")//DropDownList
        {   
            var currentDDLLabelName = splitIDValues[2].replace('ddl','');
            if(currentDDLLabelName != ddlLabelName)
            {
                var trxID = thisControl.options[0].value.split('~')[0];
                //var trxType = thisControl.options[0].value.split('~')[1];                                                                
                if(trxID!="-1")
                {                   
                    return true;                    
                }                
            }
        }
        else if(thisControl.nodeName == "SPAN")//Checkbox with an attibute
        {
            var currentChkLabelName = splitIDValues[2].replace('chk','');
            if(currentChkLabelName.indexOf("IsPrimary")==-1)
            {
                if(thisControl.checked==true)
                {
                    return true;
                }
            }                                       
        }
        else//Others
        {   
            //Check whether control is a checkbox
            if(thisControl.type=='checkbox')
            {
                var currentChkLabelName = splitIDValues[2].replace('chk','');
                if(currentChkLabelName.indexOf("IsPrimary")==-1)
                {
                    if(thisControl.checked==true)
                    {
                        return true;
                    }
                }               
            }
            else
            {
               //var currentTxtLabelName = splitIDValues[2].replace('txt','');                
                if(thisControl.value != "")
                {
                        return true;
                }
            }
        }
    }
    return false;
}

//...................................................................................................//

function ClearBranchControls(branchNodeName, arrBranchControls, ddlLabelName)
{
   
    var xDocGVDataXML = loadXMLString(document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchColsXML').value);
    var branchNode = xDocGVDataXML.getElementsByTagName(branchNodeName);
    var nodeColumns = null;
    if(branchNode.length > 0)
    {
        nodeColumns = branchNode[0].getElementsByTagName('Columns');
      
    }
    
    for(var i=0; i<arrBranchControls.length; i++)
    {
        var thisControl = document.getElementById(arrBranchControls[i]);
        if(!thisControl)
        {
            continue;
        }
        var splitIDValues = thisControl.id.split('_');
        var branchNode = splitIDValues[3];
        if(thisControl.nodeName == "SELECT")//DropDownList
        {   
            var currentDDLLabelName = splitIDValues[2].replace('ddl','');
            if(currentDDLLabelName != ddlLabelName)
            {
                //thisControl.options[0].selected = true;                               
                if(nodeColumns.length > 0)
                {
                    var nodeColDefault = GetRowByAttributeValue(nodeColumns[0], "Label", currentDDLLabelName);
                    var defaultAttribute = nodeColDefault.getAttribute("Default");
                    if(defaultAttribute && defaultAttribute!="")
                    {
                        var trxType = thisControl.options[0].value.split('~')[1];
                        var valueToBeSelected = defaultAttribute + "~" + trxType;
                        //alert(valueToBeSelected);
                        thisControl.value=valueToBeSelected;
                    }
                    else
                    {
                        thisControl.options[0].selected = true;
                    }
                }
                else
                {
                    thisControl.options[0].selected = true;
                }
            }
        }
        else if(thisControl.nodeName == "SPAN")//Checkbox with an attibute
        {
            var currentChkLabelName = splitIDValues[2].replace('chk','');
            getChildElement(thisControl).checked== false;            
            
        }
        else//Others
        {   
            //Check whether control is a checkbox
            if(thisControl.type=='checkbox')
            {
                var currentChkLabelName = splitIDValues[2].replace('chk','');
                if(thisControl.checked==true)
                {
                    thisControl.checked = false;
                }
               
            }
            else
            {
                var txtLabelName = splitIDValues[2].replace('txt','');
                thisControl.value = '';
            }
        }
    }
}

//...................................................................................................//

//Rows[@attName=attvalue]
function GetRowByAttributeValue(xmlNode, attName, attValue)
{
    if(xmlNode.childNodes)
    {
        for(var cntr=0;cntr < xmlNode.childNodes.length; cntr++)
        {
            var currentValue = xmlNode.childNodes[cntr].getAttribute(attName);
            if( currentValue == attValue)
            {
                return xmlNode.childNodes[cntr];
            }
        }
    }
    else
    {
       //alert('No Child Nodes');
       //alert(xmlNode);
       //alert((new XMLSerializer()).serializeToString(xmlNode));
    }
} 

//...................................................................................................//

function GetRowByTrxIdAndTrxType(xmlNode, trxID, trxType, prefix)
{
    for(var cntr=0;cntr < xmlNode.childNodes.length; cntr++)
    {
        var currentTrxID = xmlNode.childNodes[cntr].getAttribute(prefix+"_TrxID");
        var currentTrxType = xmlNode.childNodes[cntr].getAttribute(prefix+"_TrxType");
        if( currentTrxID == trxID && currentTrxType == trxType)
        {
            return xmlNode.childNodes[cntr];
        }
    }
}

//...................................................................................................//

function GetDDLValueByText(objDDL, text)
{   
    //Replace double spaces with a single space.
    text = text.replace(/  /g," ").replace(/  /g," ");
    for(var index =0;index<objDDL.length;index++)
    {
        if(objDDL[index].text==text)
        {
            return objDDL[index].value;
        }
    }
}

//...................................................................................................//

function GetBranchControls(branchNodeName)
{
    //Loading GVdata XML
    //var xDocGVDataXML = loadXMLString(document.getElementById('ctl00_cphPageContents_BtnsUC_ParentGVDataXML').value);
    var xDocGVDataXML = loadXMLString(document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchColsXML').value);
    var nodeBranchColumns= null;
    var nodeBranch =  xDocGVDataXML.getElementsByTagName(branchNodeName)
    if( nodeBranch.length > 0)
    {
//        var nodeGridHeading= nodeBranch[0].childNodes;
//        if(nodeGridHeading.length>0)
//        {        
//            nodeBranchColumns = nodeGridHeading[0].childNodes[2];
//        }
        var listBranchCols = nodeBranch[0].getElementsByTagName("Columns")
        if(listBranchCols.length >0)
        {
           nodeBranchColumns = listBranchCols[0]; 
        }
    }
    var arrControls = new Array();
    
    if (nodeBranchColumns)
    {
        for(var cntr=0;cntr< nodeBranchColumns.childNodes.length; cntr++ )
        {
            var label = nodeBranchColumns.childNodes[cntr].getAttribute("Label");
            var controlType =nodeBranchColumns.childNodes[cntr].getAttribute("ControlType");
            var controlID = "";

            switch (trim(controlType.toUpperCase()))
            {
                case "PHONE":
                case "TBOX":
                    {
                        if(label)
                            controlID = "ctl00_cphPageContents_txt" + label + "_" + branchNodeName;
                        break;
                    }
                case "DDL":
                    {
                        if(label)
                            controlID = "ctl00_cphPageContents_ddl" + label + "_" + branchNodeName;
                        break;
                    }
                case "CHECK":
                    {
                        if(label)
                            controlID = "ctl00_cphPageContents_chk" + label + "_" + branchNodeName;
                        break;
                    }
                case "CAL":
                    {
                        if(label)
                            controlID = "ctl00_cphPageContents_txt" + label + "_" + branchNodeName;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
           arrControls[cntr]= controlID;
        }
    }
    return arrControls;
}

//...................................................................................................//

function getChildElement(parentNode)
{
    x = parentNode.firstChild;
    while (x.nodeType != 1)
    {
        if(x.nodeType == 3)
        {
            return x;//If the node is a Text node just return the same back.
        }
        x = x.nextSibling;
    }
    return x;
}

//...................................................................................................//