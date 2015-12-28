var testCntr=-1;
if(top==this)
{
    //Create for Top-level variable to store the references to the frames.
    top.FrameManager=new Object();
    top.FrameManager.Frames=new Object();
    top.FrameManager.Dialogs=new Object();
    top.FrameManager.fCntr=0;
    top.FrameManager.IdPrefix='Frame';
    top.FrameManager.DlgPrefix='Dialog';
    top.FrameManager.InFocus='';
        
    //Test harness.
    if(testCntr!=-1) {
       jQuery(function(){
            //Test all popups in Menu
//            jQuery("img[onclick*='PopUpPage']").eq(testCntr++).click();
            
//            //Test all items in Menu 
//            setTimeout(function(){
//                testCntr=parseInt(top.localStorage['Test'])||0;
//                eval(top.jQuery("a[href*='MainMenuItemClick']").eq(testCntr++).attr('href').replace('javascript:',''));
//                top.localStorage['Test']=testCntr.toString();
//            },2000);
           
       });
    }
}

//Creates an Iframe pointed to the specified page and options.
function ShowIFrame(page,opts)
{
    var loadingText="Loading...";
    var defaults={
        isModal:false,
        title:loadingText,
        width:top.g_IFrameWidth-80,
        height:top.g_IFrameHeight-70,
        allowMaximise:true,
        reloadParent:false,
        resize:true,
        onClose:function(){},
        beforeClose:function(){}
    };
    jQuery.extend(defaults,opts||{});
    opts=defaults;
    top.ShowIframeProgress();
    var mngr=top.FrameManager;
    i=mngr.fCntr++;
    
    var frameID=mngr.IdPrefix+i;
    var iFWidth=opts.width;
    var iFHeight=opts.height;
    var iframe=top.jQuery('<iframe />')
        .attr('height',iFHeight).attr('width',iFWidth).attr('frameborder',0)
        .css('height',iFHeight).css('width',iFWidth) //IFrame dimensions setting
        .css('padding','1px 1px')
        .attr("name",frameID).attr("id",frameID)
        .load(function(){  //After Load processing function.
                try{
                    var cFrame=mngr.Frames[frameID];
                    if(cFrame.IsLoading && cFrame[0].src.length>0) {
                        top.HideIframeProgress();
                        cFrame.IsLoading=false;
                        cFrame.dialog('moveToTop');
                        var fId=frameID.replace(mngr.IdPrefix,'');
                        var fDialog=mngr.Dialogs[mngr.DlgPrefix+fId];
                        var fWin=fDialog[0].contentWindow;
                        
//                        //Move the frame to top when clicked upon the body.
                            //causing the attachments modal to appaear in the background
                            //zindex getting incremented multiple times due to this. check for active frame.
                            //if current frame is active dont movetoTop.
//                        fWin.onclick=function(){mngr.Frames[frameID].dialog('moveToTop');};
                        if(!cFrame.IsTitleSet) {
                            var title='';
                            var page=fWin.location.href.toString().toUpperCase();
                            //About:blank case occurring in IE where the content is a PDF document.
                            if(page.indexOf('PDF.ASPX')!==-1||page=='ABOUT:BLANK') {
                                title='PDF Report';
                            }
                            else {
                                title=GetTitleFromFrame(fWin);
                            }
                            fDialog.dialog('option','title',title);
                            UpdateFramePointers(fId,title);
                        }
                        
//                        //************************************************************                        
//                        //***************Test Harness for PopUps*********************
//                        //************************************************************
//                        if(testCntr!=-1) {
//                            setTimeout(function() {
//                                fDialog.dialog('close');
//                            },1000);
//                            setTimeout(function() {
//                                if((testCntr-1)!==jQuery("img[onclick*='PopUpPage']").size()) {
//                                     jQuery("img[onclick*='PopUpPage']").eq(testCntr++).click();
//                                }
//                            },2000);
//                        }

                    }
                    
                }catch(e){ alert('On Frame Load: '+e.message); }
            });
         
    mngr.Frames[frameID]=iframe;
    mngr.Frames[frameID].IsLoading=true;//Change the value once the iframe has loaded.
    mngr.Frames[frameID].Minimised=false;
    //Update the Title updation if it was specified in the options.
    if(opts.title!==loadingText) {
        iframe.IsTitleSet=true;
    }
    else {
        iframe.IsTitleSet=false;
    }
    
    var jqdialog=iframe.dialog({ autoopen:false,
             title:'<span id="frameTitle'+i+'" >'+opts.title+'</span>',
             bgiframe: false,
             width:iFWidth,
             height:iFHeight,
             modal: opts.isModal,
             show:'clip',
             hide:'clip',
             draggable:true,
             resizable:opts.resize,
             closeOnEscape:false,
             open:function(){ iframe.attr("src",page);  },
             beforeclose: function(e){ opts.beforeClose(iframe); e.target.src='';return true; },
             close:OnFrameClose,
             focus:OnFrameFocus,
             dragStop:function(){ top.jQuery(this).parent().css('height','auto'); }
        });
        
    if(opts.allowMaximise)
    {
        //Insert the Maximise icon into the title bar.
        top.jQuery('<span class="ui-icon ui-icon-newwin" style="cursor:pointer;right:2.4em;position:absolute">Maximise</span>')
            .bind('click',{fIndex:i},Maximise)
            .insertBefore(top.jQuery(".ui-dialog-titlebar-close:last"));

        //Maximise the window when Title Bar is double-clicked.
        top.jQuery('.ui-dialog-titlebar').bind('dblclick',{fIndex:i},Maximise);
    }
    
    //Insert the minimise icon into the title bar.
    if(!opts.isModal) {
        top.jQuery('<span class="ui-icon ui-icon-minusthick"  style="cursor:pointer;right:4.0em;position:absolute">Minimise</span>')
            .bind('click',{fIndex:i},HideFrame)
            .insertBefore(top.jQuery(".ui-dialog-titlebar-close:last"))
            .parent().css('min-width','150px');
    }
        
    iframe.css({height:iFHeight,width:iFWidth});
    iframe.parent().css({height:iFHeight,width:iFWidth});

    //Insert a pointer the Frame into the side bar list
    top.jQuery('#tblFrameList').append(jQuery('<tr id="trFrame'+i+'"><td><div class="FrameListItem"><a onclick="javascript:return ShowFrame('+i+');" >'+opts.title+'</a></div></td></tr>'));
    
    //Init the parent window if the current process is RefreshProcess.
    if(opts.reloadParent==true) {
        mngr.Frames[frameID].ParentFrame=this;
    }
    
    //Save the Frame options
    mngr.Frames[frameID].Options=opts;
    mngr.Dialogs[mngr.DlgPrefix+i]=jqdialog;
    top.FrameManager=mngr;
    return i;
}

var g_frameFocusEv;
//Frame Focus Event handler.
function OnFrameFocus(e) {
    g_frameFocusEv=e;
    if(top==null)return;
    var isLoading=top.IsLoadInProgress();
    if(typeof(isLoading)=='string') return false;
    //Get the embedded Bpinfo of the iFrame and update the SessionLinkBpInfo via Ajax method.
    setTimeout(function(){
        try {
            var dialog=g_frameFocusEv.target;
            var fId=dialog.id;
            var fMngr=top.FrameManager;
            var frame=fMngr.Frames[fId];
            var idx=fId.replace('Frame','');
            var maxZ=jQuery.ui.dialog.maxZ;
            var currentZ=frame.parent().css('z-index');
            //Store the Frame ID which is in focus.
            if(!frame.IsLoading && fMngr.InFocus!=fId) {
                //Update the SessionLinkBPINFO
               UpdateBpInfo(frame[0]);
            }
            fMngr.InFocus=fId;
        }
        catch(e) {
//            alert('OnFrameFocus : '+e.message);
        }
    },300);
}

//Maximise/Minimise Handler.
function Maximise(e)
{
    var fMngr=top.FrameManager;
    var fId;
    if(e.data==null||e.data.length==0) {
        fId=fMngr.InFocus;
    }
    else {
        fId=fMngr.IdPrefix+e.data.fIndex;
    }
    var fIdx=fId.replace('Frame','');
    var isMaximised=fMngr.Frames[fId].Maximised;
    //Get the base window's dimensions.
    if(typeof(isMaximised)=="undefined" || isMaximised==false)
    {
        //Maximise.
        var baseWidth=top.jQuery(top).width();
        var baseHeight=top.jQuery(top).height();
        var current=fMngr.Frames[fId];//$get('iframePage');
        var winParent=top;
        var currDims=new Object();//Store the existing values.
        var setWidth=baseWidth-8;//The amount to which maximise should happen.
        var setHeight=baseHeight-8;
        var jqWin=jQuery(current);
        currDims.width=jqWin.width();//Store the current dimensions
        currDims.height=jqWin.height();
        
        //Set the Frame Dimensions.
        if(setWidth!=0&&setHeight!=0) {
            jqWin.width(setWidth).height(setHeight-27);//-27 is to remove the titlebar height of the Dialog.
        }
        else{alert('Dims are zero!!!');}
        
        current=winParent.frames[fId];    
        current.g_IFrameWidth=top.jQuery('#'+fId).width();
        current.g_IFrameHeight=top.jQuery('#'+fId).height();
        
        //Set Resizable to false in Full mode.
        fMngr.Dialogs[fMngr.DlgPrefix+fIdx].dialog('option','resizable',false);
        
        //Set the Dialog's position and dimensions.
        var winDialog=fMngr.Dialogs[fMngr.DlgPrefix+fIdx].parent();//winParent.jQuery('#ctl00_pnlPagePopUp,#pnlPagePopUp').parent();
        currDims.top=winDialog.css('top');//Store the previous values first.
        currDims.left=winDialog.css('left');
        winDialog.css('top','0px').css('left','0px');
        winDialog.css({width:setWidth,height:setHeight});
        
        if(typeof(current.jQuery)=='function')
        {
            //ChildGrid.
            var cgvuc=current.jQuery('#ctl00_cphPageContents_CGVUC_pnlGVBranch');
            currDims.CGWidth=cgvuc.width();
            var isCustom=(GetPage(current).indexOf('COMMERCIAL.ASPX') != -1)?true:false;
            if(!isCustom) {
                cgvuc.css('width',setWidth-18);//-3:To avoid overflow scrollbars.
            }
            
            var jqGrid=current.jQuery('#tblGrid');
            if(jqGrid.size()>0) {
                var jqChange=setWidth-jqGrid.getGridParam('width')-22;
                currDims.jqChange=jqChange;
                jqGrid.setGridWidth(jqGrid.getGridParam('width')+jqChange,true);
            }
        }
        fMngr.Frames[fId].Dimensions=currDims;//Store the dimensions in a global variable.
        fMngr.Frames[fId].Maximised=true;
    }
    else
    {
        //Restore.
        var current=fMngr.Frames[fId];//$get('iframePage');
        var winParent=top;
        var currDims=fMngr.Frames[fId].Dimensions;//Retreive the previous values.
        if(!currDims) {
            alert('Current Dims not defined.');
        }
        
        var jqWin=jQuery(current);
        if(currDims&&currDims.width!=0&&currDims.height!=0) {
            jqWin.width(currDims.width).height(currDims.height);
        }
        
        current=winParent.frames[fId];    
        current.g_IFrameWidth=currDims.width;
        current.g_IFrameHeight=currDims.height;
        
        var winDialog=fMngr.Dialogs[fMngr.DlgPrefix+fIdx].parent();
        winDialog.css({top:currDims.top,left:currDims.left});
        winDialog.css({width:currDims.width,height:currDims.height});
        
        //Enable the resizing back.
        fMngr.Dialogs[fMngr.DlgPrefix+fIdx].dialog('option', 'resizable', true);

        if(typeof(current.jQuery)=='function')
        {
            //ChildGrid.
            var cgvuc=current.jQuery('#ctl00_cphPageContents_CGVUC_pnlGVBranch');
            cgvuc.css('width',currDims.CGWidth);
            if(currDims.jqChange) {
                var jqGrid=current.jQuery('#tblGrid');
                jqGrid.setGridWidth(jqGrid.getGridParam('width')-currDims.jqChange,true);
            }
        }
        delete fMngr.Frames[fId].Dimensions;//Nullify the object
        fMngr.Frames[fId].Maximised=false;
    }
}

//Updates the Frame Title and the left panel list.
//This function will be called from script executing from within the page.
function UpdateTitle(txt)
{
    if(trim(txt).length==0) {
        return;
    }
    //Get the current execution context's frame id
    var idx=this.frameElement.id.replace(top.FrameManager.IdPrefix,'');
    UpdateFramePointers(idx,txt);
    top.FrameManager.Frames[this.frameElement.id].IsTitleSet=true;
}

//Updates the Frame dependencies such as FrameList and MyHistory.
function UpdateFramePointers(idx,txt)
{
    //Set the Frame window's title.
    parent.jQuery('#frameTitle'+idx).html(txt);
    
    //Retreive the Title text in the frame.
    var fMngr=top.FrameManager;
    var frame=fMngr.Frames[fMngr.IdPrefix+idx];
    var key=frame.HKey;
    var thisWin=frame[0].contentWindow;    
    var title=GetTitleFromFrame(thisWin);
    if(!title || title.length==0)title=txt;
    //Set the SideBar reference.
    top.jQuery('#tblFrameList tr[id=trFrame'+idx+'] a').attr('title',title).text(title);
    UpdateHistoryItemText(idx,title);
}

//Changes the location of an Iframe.
function RedirectIFrame(page)
{
    var iframe=this.frameElement;
    iframe.src=page;
}

//Shows the frame matching the passed Id
function ShowFrame(id)
{
    var fMngr=top.FrameManager;
    if(fMngr.Frames['Frame'+id].Minimised) {
        var target=top.jQuery("#Frame"+id).dialog('moveToTop').parent();
        var x=target.data('x');
        var y=target.data('y');
        target.animate({   left:x,
                           top:y,
                           width:'toggle',
                           height:'toggle'
                        }, 450);
        fMngr.Frames['Frame'+id].Minimised=false;
    }
    else {
        var ev={data:{fIndex:id}};
        HideFrame(ev);
    }
    return false;
}

//Hides the frame matching the passed Id
function HideFrame(e)
{
    var fMngr=top.FrameManager;
    var fId;
    if(typeof(e)=='undefined') {
        fId='#'+fMngr.InFocus;
    }
    else {
        fId="#Frame"+e.data.fIndex;
    }
    //Check whether the frame is minimisable i.e not modal etc.,
    var currFrame=fMngr.Frames[fId.replace('#','')];
    if(currFrame) {
        if(currFrame.Options.isModal!=true) {
            if(fMngr.Frames[fId.replace('#','')].Minimised==false) {
                fMngr.Frames[fId.replace('#','')].Minimised=true;
                var target=top.jQuery(fId).parent();
                var dest=top.jQuery(fId.replace('#','#tr'));
                var minDest;
                minDest=Sys.UI.DomElement.getLocation(dest[0]);
                target.data('x',target.css('left'));
                target.data('y',target.css('top'));
                target.animate({ 
                               left: minDest.x,
                               top: minDest.y,
                               width: 'toggle',
                               height: 'toggle'
                             }, 500);
            }
        }
        else{
            //Minimise will close the frame in this case.
            currFrame.dialog('close');
        }
    }
}

//Close Frame Event handler.
function OnFrameClose()
{
    try {
        var fMngr=top.FrameManager;
        var iframe=top.jQuery(this);
        var frameId=iframe.attr('id');//Get the id of the current instance being dealt with.
        var idx=frameId.replace(fMngr.IdPrefix,'');
        var currFrame=fMngr.Frames[fMngr.IdPrefix+idx];
        //Call the After close function specified in the Frame options.
        currFrame.Options.onClose(currFrame);
        iframe.dialog('destroy');//Clean up the dialog.
        iframe.remove();//Remove the iframe's html.
        var isParent=false;
        //Check if there is any parent for the current frame that has to be refreshed.
        if(currFrame.ParentFrame) {
            var refreshDelay=0;
            var win=currFrame.ParentFrame;
            if(currFrame.ParentFrame.frameElement) { //Check whether the window to be refreshed is a frame.
                var e=new Object();
                e.target=currFrame.ParentFrame.frameElement;
                OnFrameFocus(e);//Update the BPInfo before refreshing the parent.
                refreshDelay=500;
                win=currFrame.ParentFrame.frameElement;
            }
            setTimeout(function () {
                win.onload = function () { alert('The current window has been refreshed automatically.'); };
                currFrame.ParentFrame.location.reload(true);
            }, refreshDelay);
            isParent=true;
        }
        
        var Hkey=currFrame.HKey;
        if(currFrame.IsLoading) {
            RemoveHistItem(Hkey);//Delete the History Object and UI pointer if the frame is still loading.
        }
        delete fMngr.Frames[fMngr.IdPrefix+idx];//Delete the Frame object.
        top.jQuery('#tblFrameList tr[id=trFrame' + idx + ']').slideUp(300, function () { jQuery(this).remove(); }); //Remove from the side bar menu.
        
        if(!isParent && currFrame.Options.isModal!=true) {
            //Revert the SessionLinkBpInfo to that of the most active window.
            var activeWin=GetActiveWin();
    //        alert(activeWin.src||activeWin.location.href);
            UpdateBpInfo(activeWin);
        }
    }
    catch(e) {
//        alert('OnFrameClose : '+e.message);
    }
    top.HideIframeProgress();
}

function GetActiveWin()
{
    var frames=top.FrameManager.Frames;
    var maxZ=0;//top.jQuery.ui.dialog.maxZ;
    var activeWin;
    for(var id in frames) {
        var frame=frames[id];
        var isMin=frame.Minimised;
        var currZ=frame.parent().css('z-index');
        if(!currZ)currZ=0;
        if(!isMin && currZ>maxZ) {
            maxZ=currZ;
            activeWin=frame;
        }
    }
    if(activeWin) { 
        return activeWin[0]; 
    }
    else {
        return top;
    }
}

//Checks whether any of the open frames are still loading or not.
function IsLoadInProgress()
{
    var fMngr=top.FrameManager;
    for(var id in fMngr.Frames)
    {
        var frame=fMngr.Frames[id];
        if(frame.IsLoading) {
            return frame.attr('id');
        }
    }
    return false;
}

//Updates the SessionLinkBpInfo in the server with hidden field BPInfo present within the frame.
function UpdateBpInfo(frame)
{
    try {
        var objThisDoc;
        if(frame.document) {
            objThisDoc=frame.document;
        }
        else {
            objThisDoc=frame.contentWindow || frame.contentDocument;
            if (objThisDoc.document) {
                objThisDoc=objThisDoc.document;
            }
        }
        var bpInfo=objThisDoc.getElementById('ctl00_cphPageContents_BtnsUC_parentBPInfo') ||
                objThisDoc.getElementById('ctl00_cphPageContents_hdnBPInfo')||
                objThisDoc.getElementById('ucGridView_hdnMasterBPIn');
        if(bpInfo==null&& objThisDoc==top.document)
        {
           
            bpInfo={value:'<bpinfo><BPGID>1</BPGID></bpinfo>'};
        }
        if(bpInfo && trim(bpInfo.value).length>0) {
            AjaxMethods.ResetLinkPopUpSession(bpInfo.value);
        }
    }
    catch(e) {
//        alert('UpdateBpInfo : '+ e.message);
    }
}


/******************************************** History Manager **************************************************/


var g_HistoryOn=true;
if(top==this) {
    if(localStorage==null && GetPage(window).indexOf('DASHBOARD')!=-1 && g_HistoryOn) {
        g_HistoryOn=false;
    }
    else {
        jQuery(function(){
            $('#tblHistory').css('display','block');
            RenderHistory();
        });
    }
}

//Saves the info to the Storage.
function SaveToHistory(bpgId,bpInfo,redirectPage,id)
{
    if(g_HistoryOn) {
        var isDup=IsDuplicateEntry(bpgId,bpInfo);
        if(isDup!=false){
            //Remove the old entry and insert the new one.
            RemoveHistItem(isDup);
        }
        var key=new Date().getTime();
        top.localStorage.setItem('BPGID'+key,bpgId);
        top.localStorage.setItem('BPINFO'+key,bpInfo);
        top.localStorage.setItem('Page'+key,redirectPage);
        top.localStorage.setItem('Text'+key,'');//Will be updated after the frame has finished loading.
        CreateHistItem(key,'Loading...');

        if(typeof(id)!='undefined') {
            //Set the FrameManager Object with the key.
            var fMngr=top.FrameManager;
            fMngr.Frames[fMngr.IdPrefix+id].HKey=key;//Using this key the title can be updated into the History Object.
        }
    }
}

//Renders the History stored in Storage to the UI.
function RenderHistory()
{
    var arrKeys=new Array();
    for(i=0;i<localStorage.length;i++)  {
        var key=localStorage.key(i);
        var val=localStorage.getItem(key);
        if(key.startsWith('BPGID'))
        {
            key=key.replace('BPGID','');
            arrKeys.push(key);
        }
    }
    arrKeys= arrKeys.sort(function(a,b){return a-b;});//Sort the keys by time visited.
    for(var i=0;i<arrKeys.length;i++)  {
        var key=arrKeys[i];
        var title=localStorage.getItem('Text'+key);
        var tip=GetTTFormat(title,key);

        if(title.length==0) {
            //In case of the page redirect get the title from the page itself.
            title=GetTitleFromFrame(window);
            //Update the storage also.
            localStorage.setItem('Text'+key,title);
        }
        CreateHistItem(key,title,tip);
    }
}   

//On click event handler on the History Item.
function ExecHistory(key)
{
    if(IsLoadInProgress())  {
        jqAlert('Please wait while the background window loads.');
        return;
    }
    var hist=GetHistoryObj(key);
    CommonUI.SetLinkPopUpSession(hist.BPGID,hist.BpInfo);
    var page=hist.Page;
    
    //Check if this page has been previously navigated from the Menu.
    if(page.indexOf('PopUp')==-1) { 
        if(page.toLowerCase().endsWith('x')) {
            page+='?'
        }
        else {
            page+='&'
        }
        page+='PopUp=PopUp';
    }
    ShowIFrame(page);
}

//Gets the History object from the Storage.
function GetHistoryObj(key)
{
    var hist=new Object();
    hist.BPGID=top.localStorage.getItem('BPGID'+key);
    hist.BpInfo=top.localStorage.getItem('BPINFO'+key);
    hist.Page=top.localStorage.getItem('Page'+key);
    return hist;
}

//Updates thee History Item's text to the updated text after the window has loaded.
function UpdateHistoryItemText(fId,txt) {
    var fMngr=top.FrameManager;
    var frame=fMngr.Frames[fMngr.IdPrefix+fId];
    var key=frame.HKey;
    
    if(trim(txt).length>0) {
        //Update the history list.
        top.jQuery('#ulMyHist li[id='+key+'] a').text(txt).attr('title',GetTTFormat(txt,key));
        //Update the storage object.
        top.localStorage.setItem('Text'+key,txt);
    }
}

//Creates a new history item in MyHistory based on the passed info.
function CreateHistItem(key,title,tip)
{
    if(!tip)tip=title;
    var $a=jQuery('<a href="#" onclick="ExecHistory('+key+');">'+title+'</a>');
    var $li=jQuery('<li class="liHistory" id="'+key+'" title="'+tip+'"></li>');
    var $div=jQuery('<div></div>');
    //Init the hover close image.
    $div.hover(function() {
                try{
                    var $close=jQuery('<div style="position:relative"><img class="ui-corner-all" alt="" border="0" id="close_message" style="float: right; cursor: pointer;padding:4px;background-color:white" title="Close this popup" src="'+g_cdnImagesPath+'Images/floating-close.png" /></div>');
                    $close.click(function(){
                                     var delKey=jQuery(this).closest('li').attr('id');
                                     RemoveHistItem(delKey);
                                });
                    jQuery(this).prepend($close);
                 }catch(e){  }
                },
                function(){
                    jQuery(this).children('div').remove();
              });
    $div.append($a);
    $li.append($div);
    var $ul=top.jQuery('#ulMyHist');
    var liCount=$ul.children('li').size();
    if(liCount==0) {
        $ul.css({height:20});
    }
    var ulheight=$ul.height();
    //Append the new history item and increase the height.
    $ul.append($li).css({ height: ulheight+21 });
    if(ulheight<80) {
        $ul.parent().animate({ height:'+='+eval(ulheight) });
    }
    //Scroll to the current li
    jQuery('#divHistory').animate({scrollTop:'+='+eval((liCount-3)*21)}, 100);
}

function RemoveHistItem(key)
{
    top.jQuery('#ulMyHist li[id='+key+']').slideUp(300,function(){ 
                                            jQuery(this).remove();
                                        });
    //Remove from Storage also
    top.localStorage.removeItem('BPGID'+key);
    top.localStorage.removeItem('BPINFO'+key);
    top.localStorage.removeItem('Page'+key);
    top.localStorage.removeItem('Text'+key);
}

//Checks for Duplicate entries into the storage.
function IsDuplicateEntry(bpgId,bpInfo)
{
    for(i=0;i<localStorage.length;i++) {
        var key=localStorage.key(i);
        if(key.startsWith('BPINFO')) {
            var val=localStorage.getItem(key);
            var trueKey=key.replace('BPINFO','');
            var BPGID=localStorage.getItem('BPGID'+trueKey);
            if(val===bpInfo && BPGID===bpgId){
                return trueKey;
            }
        }
    }
    return false;
}

//Returns the Tooltip format.
function GetTTFormat(title,key)
{
    var date=new Date(parseInt(key));
    var timeVisited=date.toLocaleTimeString();
    return title+'\t Time Visited : '+timeVisited;
}

function GetTitleFromFrame(win)
{
    try {
        var page=GetPage(win);
        if(typeof(win.gridTableId)!='undefined') {
            return win.jQuery('.ui-jqgrid-titlebar').text();
        }
        else if(page.indexOf('MYREPORTS')!==-1) {
            return win.jQuery(".grdVwRPTitle:first").text()||'My Reports';
        }
        else if(page.indexOf('EMAILPDF.ASPX')!==-1) {
            return 'Email PDF';
        }
        else if(page.indexOf('PDF.ASPX')!==-1) {
            return 'PDF Report';
        }
        else {
            var $titles=win.jQuery(".grdVwtitle:not([id*=CGVUC]):first");//,.grdVwRPTitle
    //        alert('Titles count : '+$titles.size()+ "\n" +$titles.text());
            if(jQuery.trim($titles.text()).length>0) {
                return $titles.text();
            }
            else {
                //Attachments,Secure,Notes etc.,
                $titles=win.jQuery(".grdVwRPTitle");
                if($titles.text().length>0) {
                    return $titles.text();
                }
                else {
                    return 'Error';
                }
            }
        }
    }
    catch(e) {
        return 'Error:';
    }
}

//Removes the Loading in Progress item from the MyHistory list.
//Returns true if such an item was found and sucessfully removed, else false.
function RemoveLoadingItem()
{
    var key=jQuery("#ulMyHist a:contains('Loading...')").closest('li').attr('id');
    if(key) {
        RemoveHistItem(key);
        return true;
    }
    return false;
}

function ScrollHistory(dir)
{
    var divH=jQuery('#ulMyHist').parent();
    var animationTime=300;
    var scrollLen=80;
    if(dir=='Up') {
        divH.animate({scrollTop: '-='+scrollLen}, animationTime);
    }
    else {
        divH.animate({scrollTop: '+='+scrollLen}, animationTime);
    }
}
