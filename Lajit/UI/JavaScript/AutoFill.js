// AutoFill Add popup features
function ShowAutoFillPage(processBPGID,redirectPage,isPopUp,Columnlabel,objTBox)
{
    var myTBox=jQuery('#'+objTBox);
    objTBox=objTBox.replace('ctl00_cphPageContents_','');//Target the clone.
    if(jQuery('#'+objTBox).attr('disabled')!=true)
    {
        var COXML="<INIT>1</INIT>";
        var processName="";//No Process Name.
        var objautoTxt=document.getElementById(objTBox);
        //check jquery autofill div display and hide
        setTimeout(function(){ jQuery(".ac_results").css({'display':'none'}); },50);
        redirectPage = redirectPage+"?CL="+Columnlabel+"&LName="+objautoTxt.value;
        //Clearing textbox control text
        objautoTxt.value="";
        myTBox.val('');//Clear the parent also
        CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML);
    }
}

//...................................................................................................//    

//Update hdnAutoFillBPEINFO on autofill value slected to view
function UpdateAutoFill(BranchName,autofillValue,Result)
{
    if(autofillValue!='')
    {
        if(Result!='')
        {
            Result=Result.split(";");
            var BPINFO;
            var processBPGID;
            BPINFO='<'+ BranchName +'>';
            BPINFO=BPINFO+'<RowList>';
            BPINFO=BPINFO+'<Rows  '+ Result[0] +' '+ Result[1] + ' />';
            BPINFO=BPINFO+'</RowList>';
            BPINFO=BPINFO+'</'+ BranchName +'>';
            BPINFO=BPINFO+'<CallingObject>';
            BPINFO=BPINFO+'<BPGID>770</BPGID>';
            BPINFO=BPINFO+'<PageInfo>payables/APInvoice.aspx</PageInfo>';
            BPINFO=BPINFO+'<Caption>'+ autofillValue +'</Caption>';
            BPINFO=BPINFO+'</CallingObject>';
            var hdnAutoFillBPEINFO=document.getElementById('ctl00_cphPageContents_BtnsUC_hdnAutoFillBPEINFO');
            if(hdnAutoFillBPEINFO)
            {
                hdnAutoFillBPEINFO.value=BPINFO;
            }
        }
    }   
}

//...................................................................................................//

// Auto fill selected value validate and redirect to next page
function ValidateAndRedirect(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp,columnName,BranchName,TBoxID)
{
    var autofillValue=document.getElementById(TBoxID).value;
    var autofillText=document.getElementById(TBoxID).getAttribute('AFText');
    if(autofillValue=='')
    {
         alert("Please select an item first!!");
    }
    else
    {
       // var response=CommonUI.GetAutoFillValues(cacheName,autofillValue,columnName);
        var Result;
        //Result=response.value;
        var autofillIDS;
        autofillIDS=autofillValue.split("~");
        
        Result = columnName + "_TrxID='" + autofillIDS[0] + "';" + columnName + "_TrxType='" + autofillIDS[1] + "'";
        //Result="";
        if (Result!='')
        {
          //Update hdnAutoFillBPEINFO value 
          UpdateAutoFill(BranchName,autofillText,Result);
          //Redirect to next page
          OnFormBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp);
         }
         else
         {
            alert('Record not found.');
         }
    }
}


//Creates a clone of the passed element and inits all the required attributes and handlers.
function DoCloning(elemId)
{
    var jqID=elemId;
    var trueEl=jQuery(jqID);
    if(trueEl.length==0)return null;
    var cloneElId=elemId.replace('#ctl00_cphPageContents_','');
    var cloneEl=trueEl.clone(true);
    cloneEl.attr('id',cloneElId)            //Set the id of the clone element.
            .attr('name',cloneElId.replace(/_/g,'$'))//Change the name of the clone element.
            .attr('AutoFill',true);          //Add an attribute to mark it as an Autofill.
    
    if(trueEl.val().length>0) {
        //Set the text of the clone.
        cloneEl.val(trueEl.attr('AFText'));
    }

//    //**********Testing Block start
//    
//    cloneEl.css('border','2px solid yellow')
//            .attr('title','This is clone of the textbox which is lying hidden behind.');
//            
//    //Add a tip
//    trueEl.dblclick(function(){ 
//                                var text="AF Text : "+jQuery(this).attr('AFText');
//                                text+="\nAutoFill ID : "+jQuery(this).attr('AutoFillId')
//                                alert(text);
//                                });
//    //*********Testing Block end
    
    //Hide the original element and replace it with its clone.
    trueEl.css('display','none').attr('disabled',false).parent().append(cloneEl);
    
    //On Key Press of the clone if the text has been changed manually then clear the parent also.
    cloneEl.change(function(){
        var AFText=this.getAttribute('AFText');
        if(AFText) {
            var clParent=jQuery('#ctl00_cphPageContents_'+this.id);
            if(this.value.toUpperCase()!==AFText.toUpperCase()) {
                //Clear the parent of this clone.
                clParent.val('').attr('AutoFillID','').attr('AFText','');
            }
            else {
                //Re-insert the values which were present in the clone.
                var clone=jQuery(this);
                var afID=clone.attr('AutoFillID');
                clParent.val(afID).attr('AutoFillID',afID).attr('AFText',clone.val());
            }
        }
    });
    

    return cloneEl;
}

//Params: isDDL - Bool. Specifies whether the the behaviour of the AF is like a DDL or not.
//                 i.e TrxID~TrxType is posted rather than the text.

function AttachAutoComplete(elemId,ctxKey,pageName,targetCol,isDDL)
{
    elemId='#'+elemId;
    var GetCOA;//A function pointer.
    if(ctxKey=='AutoFillAccount') {
        //Get COA attribute of the previous textbox   
        GetCOA=function(){return jQuery(elemId).parent('td').prev().children('input').attr('COA');};
    }
    var ACElement;
    if(isDDL&&isDDL==true) {
        var clone=DoCloning(elemId);
        if(!clone) return;
        ACElement=clone;
    }
    else {
        ACElement=jQuery(elemId);
    }
    //ACElement.autocomplete('../HttpHandlers/AutoFillHandler.ashx',
    ACElement.autocomplete(g_virtualPath+'HttpHandlers/AutoFillHandler.ashx',
                      {
                         width:400,multiple:false,
                         matchContains:true,
                         scrollHeight:300,
                         max:900,
                         cacheLength:0,
                         minChars:1,
                         formatItem:FormatACItem,
                         formatResult:FormatACResult,
                         extraParams:{ contextkey:ctxKey,page:pageName,COA:GetCOA }
                         
                       }).result(function(evt,data,formatted)
                                {
                                    //Init the following attributes based on the selection made.
                                    jQuery(elemId).attr('AutoFillID',data[0]).attr('AFText',data[1]);
                                    if(isDDL&&isDDL==true) {
                                        //Set the TrxID~TrxType as text in the original element upon item selection 
                                        //in the clone.
                                        jQuery(elemId).val(data[0]);
                                        jQuery(elemId.replace('ctl00_cphPageContents_',''))
                                            .attr('AutoFillID',data[0]).attr('AFText',data[1]);//Set the same for the clone also.
                                    }
                                    
                                    switch(ctxKey)
                                    {
                                        case 'AutoFillVendor':
                                            Set1099Defaults(data,elemId);
                                        case 'Customer':
                                            OnACItemSelected(evt,data,formatted,elemId);
                                            break;
                                        case 'AutoFillJob':
                                            jQuery(elemId).attr('COA', data[2]);
                                            break;
                                        case 'SelectCustInvoice'://Newly added on 16-11-09 To set Own attribute for amount field
                                            var targetElem=elemId.replace("SelectCustInvoice", targetCol);
                                            jQuery(targetElem).attr('Own', data[2]);
                                            break;
                                    }
                                    
                                    //Special case of UploadTextJobOvr.aspx
                                    if(pageName.indexOf('uploadtextjobovr_aspx')!=-1)
                                    {
                                        jQuery('#ctl00_cphPageContents_hdnAutofillJobID').val(data[0]);
                                    }
                                    else if(pageName.indexOf('help_aspx')!=-1)
                                    {
                                        jQuery('#ctl00_cphPageContents_hdnAutoFillBPGID').val(data[2]);
                                    }
                                });
 }
 
function FormatACItem(row) { 
    return row[1]; 
}

function FormatACResult(row) { 
    return row[1].replace(/(<.+?>)/gi, ''); 
}

function OnACItemSelected(evt,data,formatted,eId)
{
    SetAutoFillAttributes(eId.replace('#',''));
}
 
//Sets the 1099 Defaults for DDLs in child grid based on AutoFillVendor selection.
function Set1099Defaults(data,eId)
{
    try {
        if(data.length>2 && eId.indexOf('_CGVUC_')==-1)
        {
            jQuery('#ctl00_cphPageContents_CGVUC_grdVwBranch select[id$=ddlTypeOf1099T4]').each(function(i,obj) {
                if(obj.selectedIndex==0)//Only if nothing has been selected.
                {
                    var target=jQuery(obj);
                    target.unbind('focus');
                    target.bind('focus',function() {
                        var trxID=parseInt(data[2]);
                        var idx=GetDDLIndexByTrxID(trxID,this);
                        obj.value=obj.options[idx].value;
                        target.unbind('focus');//Remove the focus handler once the default has been set.
                    });
                }
            });
            //Display an alert.
            if(data[2]!=='0') {
            
               //Display alert only if the messsage not equal to 'data not found'.
               if(data[1].indexOf('<b>Data not found.</b>')==-1)
               {
                var displayTxt='Vendor <b>'+data[1]+'</b> has a default 1099 Code.\nClick the 1099 code below to accept.';
                jqAlert(displayTxt,null,false,function(){ jQuery(eId.replace('ctl00_cphPageContents_','')).select(); });
               }
            }
        }
    }
    catch(e){}
}
 
//Vendor& Customer AutoFill Select case.
function SetAutoFillAttributes(textboxname)
{  
    try {
        var txtTarget=document.getElementById(textboxname);
        var title =txtTarget.getAttribute('AutoFillID');
        var callerType=txtTarget.getAttribute('callertype');
        var POLinkId=txtTarget.getAttribute('polinkid');
        CheckForPOs(txtTarget,callerType,POLinkId);
    }
    catch(e){}
}

//...................................................................................................//

//Displays the PO's for the selected AutoFill Vendor/Customer
//objLnkBtn: Sender Object
//autoFillId: Client ID of the AutoFill TextBox
//COContainerID: Client ID of the Hidden Variable used to transfer COXML from various methods.
function ShowPOs(objLnkBtn, autoFillId)
{
    var COContainerID='ctl00_cphPageContents_BtnsUC_hdnBPIconBPEINFO';
    var processBPGID=objLnkBtn.getAttribute("bpgid");
    var navigatePage=objLnkBtn.getAttribute("pageinfo");
    var formBPGID=objLnkBtn.getAttribute("formbpgid");
    var isPopUp=objLnkBtn.getAttribute("ispopup");
    var processName=objLnkBtn.getAttribute("procname");
    var objAutoFill = _get(autoFillId);
    
    if(trim(objAutoFill.value).length==0) {
        return false;
    }
    var objCOContainer = _get(COContainerID);
    var autoFillTrxID=objAutoFill.getAttribute("autofillid").split('~')[0];
    var autoFillTrxType=objAutoFill.getAttribute("autofillid").split('~')[1];
    var COXML=GetCallingObjectXML(autoFillTrxID,autoFillTrxType,formBPGID,objAutoFill.value,navigatePage);
    objCOContainer.value=COXML;
    OnFormBPCLinkClick(processBPGID,navigatePage,COContainerID,processName,isPopUp);
    return false;
}
   
//...................................................................................................// 
var g_LnkBtnPOId;
function CheckForPOs(objTxtAutoFill,callerType,lnkBtnPOId)
{
    var autoFillTrxID=objTxtAutoFill.getAttribute("autofillid").split('~')[0];
    g_tempAutoFillID=objTxtAutoFill.getAttribute("autofillid");
    g_LnkBtnPOId=lnkBtnPOId;
    AjaxMethods.SeekPOs(autoFillTrxID,callerType,callback_ShowPOs);
}

//...................................................................................................//

var g_tempAutoFillID;
var g_POCount;
function HidePOLink(obj)
{
    var AFActualText=obj.getAttribute('AFText');
    var lnkPO=_get(g_LnkBtnPOId);
    if(AFActualText && lnkPO)
    {
        if(obj.value==AFActualText &&(g_POCount&&g_POCount>0)) {
            lnkPO.style.display='';//Show the link button
        }
        else {
            lnkPO.style.display='none';
        }
    }
}

//...................................................................................................//

function callback_ShowPOs(response)
{
    if (response.error != null)
    {
      alert(response.error);
      return;
    }
    var PoCount=parseInt(response.value);
    g_POCount=PoCount;
    
    var objPOLnkBtn=_get(g_LnkBtnPOId);
    if(PoCount >0)
    {
        var POString=(PoCount==1)?"PO":"PO's";
        var displayPOString = PoCount +" "+POString+" found";
        objPOLnkBtn.innerHTML=displayPOString;
        objPOLnkBtn.style.display='';//Show the link button
    }
    else
    {
        objPOLnkBtn.style.display='none';
    }
}

//Converts a JSON string to Object form. 
function ParseJSON(src)
{
    if (typeof(JSON) == 'object' && JSON.parse)
        return JSON.parse(src);
    var filtered = src;
    filtered = filtered.replace(/\\["\\\/bfnrtu]/g, '@');
    filtered = filtered.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']');
    filtered = filtered.replace(/(?:^|:|,)(?:\s*\[)+/g, '');
    if (/^[\],:{}\s]*$/.test(filtered))
        return eval("(" + src + ")");
    else
        throw new SyntaxError("Error parsing JSON, source is not valid.");
}
 
//Gets the index of the DDL item having a matched TrxID.
function GetDDLIndexByTrxID(trxId,objDDL)
{
    var arrOpts=objDDL.options;
    for(var i=0;i<arrOpts.length;i++)
    {
        var optVal=arrOpts[i].value;
        var cTrxId=optVal.split('~')[0];
        if(parseInt(cTrxId)==trxId)
        {
            return i;
        }
    }
    return 0;//Not found
}
