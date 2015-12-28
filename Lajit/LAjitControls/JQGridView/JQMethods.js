var gridTableId="#tblGrid";//With the hash-jQuery friendly.
var editButtonId="#btnEdit";
var addButtonId="#btnAdd";
var searchButtonId="#btnSearch";

//var lastsel;
//function onselectRowGrid(id)
//{  
//    if(id && id!==lastsel){ 
//        jQuery('#tblGrid').restoreRow(lastsel);
//        jQuery('#tblGrid').editRow(id,true);
//        lastsel=id; 
//    }  
//}

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

var xDocOut;
function GridLoadComplete(a,b,c,d)
{
    xDocOut=a.responseXML;
    //TODO: Function needs to be emitted dynamically.
}

//Modify
jQuery(document).ready(function(){
    jQuery(editButtonId).click(function()
    { 
        var objGrid=jQuery(gridTableId);
        var editUrl=objGrid.getGridParam('url');
        if(editUrl.indexOf('?')!= -1) {
            editUrl=editUrl.split('?')[0];
        }
        var gr = objGrid.getGridParam('selrow');
        
        if( gr != null )
        {
            var strRow=ConvertToString(xDocOut.getElementsByTagName('Rows')[parseInt(gr)]);
            var editPrms=GetEditParams()+"&Row="+strRow;
            //objGrid.setPostDataItem("Row",strRow); 
            objGrid.editGridRow(gr,{width:400,reloadAfterSubmit:false,url:editUrl+"?"+editPrms
            ,afterSubmit : function(response, postdata)
            {
                alert(response.responseText);
                return [true,response.responseText];
            }
            }); 
        }
        else 
        {
            alert("Please Select Row");
        }
    });
      
      
    //Add.
    jQuery(addButtonId).click(function()
            {
                var objGrid=jQuery(gridTableId);
                var editUrl=objGrid.getGridParam('url');
                if(editUrl.indexOf('?')!= -1) {
                    editUrl=editUrl.split('?')[0];
                }
                objGrid.editGridRow( "new", {reloadAfterSubmit:false, url:editUrl+"?"+GetEditParams()
                ,afterSubmit : function(response, postdata)
                        {
                           alert(response.responseText);
                           return [true,response.responseText];
                        } 
                });
            });
        
    
    //Search Invoke.
    jQuery(searchButtonId).click(function()
            {
            
                jQuery(gridTableId).searchGrid( {sopt:['cn','bw','eq','ne','lt','gt','ew']} ); 
            
            });
            
    //Search Invoke.
    jQuery('#btnTest').click(function()
    {
        var udata = jQuery(gridTableId).getGridParam('userData');
        var rData = jQuery(gridTableId).getRowData(1);
        var v = jQuery(gridTableId).jqGrid({
                    xmlReader: { root:"result" },
                    });
        
    });
});        

   
function GetEditParams()
{
    var strParams="";
    var items = jQuery(gridTableId).getGridParam('userData');
    for(var index in items) 
    {
        if(strParams=="")
            strParams = index + "=" + items[index];
        else
            strParams= strParams + "&" + index + "=" + items[index];
    }
    return strParams;  
} 

function OnJQColLinkClick(processName, processBPGID, pageInfo, isPopUp, sender)
{
    //console.log(processName +" "+ processBPGID+" "+  pageInfo+" "+  isPopUp);
    var objGrid=jQuery(gridTableId);
    
    //console.log(selRow);
}